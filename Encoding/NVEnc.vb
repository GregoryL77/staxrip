
Imports StaxRip.CommandLine
Imports StaxRip.UI
Imports JM.LinqFaster
Imports System.Text

<Serializable()>
Public Class NVEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "Nvidia | " & Params.Codec.OptionText.Replace("Nvidia ", "")
        End Get
    End Property

    <NonSerialized>
    Private ParamsValue As EncoderParams

    Property Params As EncoderParams
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New EncoderParams
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As EncoderParams)
            ParamsValue = value
        End Set
    End Property

    Overrides Sub ShowConfigDialog()
        Dim newParams As New EncoderParams
        Dim store = ParamsStore.GetDeepClone 'TEst this !!!
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            form.HTMLHelp = "<h2>NVEnc Help</h2>" & "<p>Right-clicking a option shows the local console help for the option.</p>" &
                            "<p>For Constant Quality Mode choose VBR mode, then check Constant Quality Mode and set desired VBR Quality (usually between 10-30).</p>" &
                           $"<h2>NVEnc Online Help</h2><p><a href=""{Package.NVEnc.HelpURL}"">NVEnc Online Help</a></p>" &
                           $"<h2>NVEnc Console Help</h2><pre>{HelpDocument.ConvertChars(Package.NVEnc.CreateHelpfile())}</pre>"

            Dim saveProfileAction = Sub()
                                        Dim enc = Me.GetDeepClone 'Test this!
                                        Dim params2 As New EncoderParams
                                        Dim store2 = store.GetDeepClone
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub
            form.cms.SuspendLayout()
            form.cms.Items.AddRange({New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetConsoleOutput(Package.NVEnc.Path, "--check-hw"))),
                                    New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetConsoleOutput(Package.NVEnc.Path, "--check-features"))),
                                    New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetConsoleOutput(Package.NVEnc.Path, "--check-environment"))),
                                    New ActionMenuItem("Save Profile...", saveProfileAction, ImageHelp.GetImageC(Symbol.Save))})
            form.cms.ResumeLayout(False)
            newParams.NVForm = form

            If form.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputExt() As String
        Get
            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        If OutputExt = "h265" Then
            Dim codecs = ProcessHelp.GetConsoleOutput(Package.NVEnc.Path, "--check-hw").Right("Codec(s)")

            If Not codecs.ToLowerInvariant.Contains("hevc") Then
                Throw New ErrorAbortException("NVEnc Error", "H.265/HEVC isn't supported by the graphics card.")
            End If
        End If

        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.NVEnc
            proc.SkipStrings = {"%]", " frames: "}
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ & Params.GetCommandLine(True, True) & """"
            proc.Start()
        End Using

        AfterEncoding()
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim r As New MenuList
        r.Add("Encoder Options", AddressOf ShowConfigDialog)
        r.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return r
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Dim modeVal As Integer = Params.Mode.Value
            Return modeVal = 0 OrElse (modeVal >= 2 AndAlso Params.ConstantQualityMode.Value)
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property

    Shared Function Test() As String
        Dim tester As New ConsolAppTester

        tester.IgnoredSwitches = "help version check-device input-analyze input-format output-format
            video-streamid video-track vpp-delogo vpp-delogo-cb vpp-delogo-cr vpp-delogo-depth output
            vpp-delogo-pos vpp-delogo-select vpp-delogo-y check-avversion check-codecs caption2ass log
            check-encoders check-decoders check-formats check-protocols log-framelist fps
            check-filters input raw avs vpy vpy-mt key-on-chapter audio-delay audio-ignore-decode-error
            avcuvid-analyze audio-source audio-file seek format audio-copy audio-ignore-notrack-error
            audio-copy audio-codec vpp-perf-monitor avi audio-profile check-profiles avsync mux-option
            audio-bitrate audio-ignore audio-ignore audio-samplerate audio-resampler audio-stream dar
            audio-stream audio-stream audio-stream audio-filter chapter-copy chapter sub-copy input-res
            vpp-decimate audio-disposition audio-metadata option-list sub-disposition sub-metadata
            metadata video-metadata video-tag attachment-copy chapter-no-trim"

        'tester.UndocumentedSwitches = "cbrhq vbrhq"
        tester.Package = Package.NVEnc
        tester.CodeFile = Folder.Startup.Parent & "Encoding\nvenc.vb"

        Return tester.Test
    End Function

    Public Class EncoderParams
        Inherits CommandLineParams

        Public NVForm As CommandLineForm

        Sub New()
            Title = "NVEnc Options"
        End Sub

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "NVEnc Hardware", "NVEnc Software", "QSVEnc (Intel)", "ffmpeg (Intel)", "ffmpeg (CUDA)"},
            .Values = {"avs", "nvhw", "nvsw", "qs", "ffqsv", "ffcuda"}}

        Property Mode As New OptionParam With {
            .Text = "Mode",            '.Expand = True,
            .Switches = {"--cqp", "--cbr", "--vbr"},
            .Options = {"CQP - Constant QP", "CBR - Constant Bitrate", "VBR - Variable Bitrate"},
            .VisibleFunc = Function() Not Lossless.Value,
            .ArgsFunc = AddressOf GetModeArgs,
            .ImportAction = Sub(param, arg)
                                'If Mode.Switches.Contains(param) Then Mode.Value = Array.IndexOf(Mode.Switches.ToArray, param)
                                Dim find = Array.IndexOf(Mode.Switches.ToArray(), param)
                                If find >= 0 Then Mode.Value = find
                                If param.Equals("--vbr") AndAlso arg.Equals("0") Then '(param.Equals("--vbrhq") OrElse - Obsolete!
                                    ConstantQualityMode.Value = True
                                End If
                            End Sub}

        Property Multipass As New OptionParam With {
            .Text = "Multipass",
            .Switch = "--multipass",
            .VisibleFunc = Function() Mode.Value >= 1,
            .Options = {"2pass-full", "2pass-quarter", "none"},
            .Init = 2}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .HelpSwitch = "-c",
            .Text = "Codec",
            .Options = {"Nvidia H.264", "Nvidia H.265"},
            .Values = {"h264", "h265"},
            .ImportAction = Sub(param, arg) Codec.Value = If(arg.Equals("h264") OrElse arg.Equals("avc"), 0, 1)}

        Property Profile As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile",
            .Name = "ProfileH264",
            .VisibleFunc = Function() Codec.Value = 0,
            .Options = {"Baseline", "Main", "High", "High 444"},
            .Init = 2}

        Property ProfileH265 As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile",
            .Name = "ProfileH265",
            .VisibleFunc = Function() Codec.Value = 1,
            .Options = {"Main", "Main 10", "Main 444"}}

        Property ConstantQualityMode As New BoolParam With {
            .Switches = {"--vbr-quality"},
            .Text = "Constant Quality Mode",
            .VisibleFunc = Function() Not Lossless.Value AndAlso Mode.Value >= 2
        }

        Property QPAdvanced As New BoolParam With {
            .Text = "Show advanced QP settings",
            .VisibleFunc = Function() Mode.Value = 0
        }

        Property QP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP",
            .Init = 18,
            .VisibleFunc = Function() Mode.Value = 0 AndAlso Not QPAdvanced.Value,
            .Config = {0, 51}}

        Property QPI As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP I",
            .Init = 18,
            .VisibleFunc = Function() Mode.Value = 0 AndAlso QPAdvanced.Value,
            .Config = {0, 51}}

        Property QPP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP P",
            .Init = 20,
            .VisibleFunc = Function() Mode.Value = 0 AndAlso QPAdvanced.Value,
            .Config = {0, 51}}

        Property QPB As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP B",
            .Init = 22,
            .VisibleFunc = Function() Mode.Value = 0 AndAlso QPAdvanced.Value,
            .Config = {0, 51}}

        Property VbrQuality As New NumParam With {
            .Switch = "--vbr-quality",
            .Text = "VBR Quality",
            .Config = {0, 51, 1, 1},
            .VisibleFunc = Function() Not Lossless.Value AndAlso Mode.Value >= 2,
            .ArgsFunc = Function()
                            Dim vbrQVal As Double = VbrQuality.Value
                            If ConstantQualityMode.Value OrElse vbrQVal <> VbrQuality.DefaultValue Then
                                Return "--vbr-quality " & vbrQVal
                            End If
                        End Function}

        Property AQ As New BoolParam With {
            .Switch = "--aq",
            .Text = "Adaptive Quantization (Spatial)"}

        Property Lossless As New BoolParam With {
            .Switch = "--lossless",
            .Text = "Lossless"}

        Property MaxCLL As New NumParam With {
            .Switch = "--max-cll",
            .Text = "Maximum CLL",
            .VisibleFunc = Function() Codec.Value = 1,
            .Config = {0, Integer.MaxValue, 50},
            .ArgsFunc = Function() If(MaxCLL.Value <> 0 OrElse MaxFALL.Value <> 0, "--max-cll """ & MaxCLL.Value & "," & MaxFALL.Value & """", ""),
            .ImportAction = Sub(param, arg)
                                If arg.NullOrEmptyS Then
                                    Exit Sub
                                End If

                                Dim a = arg.Trim(""""c).Split(","c)
                                MaxCLL.Value = a(0).ToInt
                                MaxFALL.Value = a(1).ToInt
                            End Sub}

        Property MaxFALL As New NumParam With {
            .Switches = {"--max-cll"},
            .Text = "Maximum FALL",
            .Config = {0, Integer.MaxValue, 50},
            .VisibleFunc = Function() Codec.Value = 1}

        Property Interlace As New OptionParam With {
            .Text = "Interlace",
            .Switch = "--interlace",
            .Options = {"Disabled", "Top Field First", "Bottom Field First"},
            .Values = {"", "tff", "bff"}}

        Property Custom As New StringParam With {
            .Text = "Custom",
            .Quotes = QuotesMode.Never,
            .AlwaysOn = True}

        Property Tweak As New BoolParam With {.Switch = "--vpp-tweak", .Text = "Tweaking", .ArgsFunc = AddressOf GetTweakArgs}
        Property TweakContrast As New NumParam With {.Text = "      Contrast", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {-2.0, 2.0, 0.1, 1}}
        Property TweakGamma As New NumParam With {.Text = "      Gamma", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.1, 10.0, 0.1, 1}}
        Property TweakSaturation As New NumParam With {.Text = "      Saturation", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.0, 3.0, 0.1, 1}}
        Property TweakHue As New NumParam With {.Text = "      Hue", .HelpSwitch = "--vpp-tweak", .Config = {-180.0, 180.0, 0.1, 1}}
        Property TweakBrightness As New NumParam With {.Text = "      Brightness", .HelpSwitch = "--vpp-tweak", .Config = {-1.0, 1.0, 0.1, 1}}
        Property TweakSwapUV As New BoolParam With {.Text = "Swap UV", .HelpSwitch = "--vpp-tweak", .LeftMargin = g.MainForm.FontHeight * 1.3}

        Property Pmd As New BoolParam With {.Switch = "--vpp-pmd", .Text = "Denoise using PMD", .ArgsFunc = AddressOf GetPmdArgs}
        Property PmdApplyCount As New NumParam With {.Text = "      Apply Count", .Init = 2}
        Property PmdStrength As New NumParam With {.Text = "      Strength", .Name = "PmdStrength", .Init = 100.0, .Config = {0, 100, 1, 1}}
        Property PmdThreshold As New NumParam With {.Text = "      Threshold", .Init = 100.0, .Config = {0, 255, 1, 1}}

        Property Knn As New BoolParam With {.Switch = "--vpp-knn", .Text = "Denoise using K-nearest neighbor", .ArgsFunc = AddressOf GetKnnArgs}
        Property KnnRadius As New NumParam With {.Text = "      Radius", .Init = 3}
        Property KnnStrength As New NumParam With {.Text = "      Strength", .Init = 0.08, .Config = {0, 1, 0.02, 2}}
        Property KnnLerp As New NumParam With {.Text = "      Lerp", .Init = 0.2, .Config = {0, Integer.MaxValue, 0.1, 1}}
        Property KnnThLerp As New NumParam With {.Text = "      TH Lerp", .Init = 0.8, .Config = {0, 1, 0.1, 1}}

        Property Pad As New BoolParam With {.Switch = "--vpp-pad", .Text = "Padding", .ArgsFunc = AddressOf GetPaddingArgs}
        Property PadLeft As New NumParam With {.Text = "      Left"}
        Property PadTop As New NumParam With {.Text = "      Top"}
        Property PadRight As New NumParam With {.Text = "      Right"}
        Property PadBottom As New NumParam With {.Text = "      Bottom"}

        Property Deband As New BoolParam With {.Text = "Deband", .Switch = "--vpp-deband", .ArgsFunc = AddressOf GetDebandArgs}
        Property DebandRange As New NumParam With {.Text = "     Range", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 127}}
        Property DebandSample As New NumParam With {.Text = "     Sample", .HelpSwitch = "--vpp-deband", .Init = 1, .Config = {0, 2}}
        Property DebandThre As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreY As New NumParam With {.Text = "          Y", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreCB As New NumParam With {.Text = "          CB", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreCR As New NumParam With {.Text = "          CR", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandDither As New NumParam With {.Text = "     Dither", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandDitherY As New NumParam With {.Text = "          Y", .HelpSwitch = "--vpp-deband", .Name = "vpp-deband_dither_y", .Init = 15, .Config = {0, 31}}
        Property DebandDitherC As New NumParam With {.Text = "          C", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandSeed As New NumParam With {.Text = "     Seed", .HelpSwitch = "--vpp-deband", .Init = 1234}
        Property DebandBlurfirst As New BoolParam With {.Text = "Blurfirst", .HelpSwitch = "--vpp-deband", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property DebandRandEachFrame As New BoolParam With {.Text = "Rand Each Frame", .HelpSwitch = "--vpp-deband", .LeftMargin = g.MainForm.FontHeight * 1.3}

        Property Afs As New BoolParam With {.Text = "Auto field shift deinterlacer", .Switches = {"--vpp-afs"}, .ArgsFunc = AddressOf GetAFS}
        Property AfsPreset As New OptionParam With {.Text = "     Preset", .HelpSwitch = "--vpp-afs", .Options = {"Default", "Triple", "Double", "Anime", "Cinema", "Min_afterimg", "24fps", "24fps_sd", "30fps"}}
        Property AfsINI As New StringParam With {.Text = "     INI", .HelpSwitch = "--vpp-afs", .BrowseFile = True}
        Property AfsLeft As New NumParam With {.Text = "     Left", .HelpSwitch = "--vpp-afs", .Init = 32}
        Property AfsRight As New NumParam With {.Text = "     Right", .HelpSwitch = "--vpp-afs", .Init = 32}
        Property AfsTop As New NumParam With {.Text = "     Top", .HelpSwitch = "--vpp-afs", .Init = 16}
        Property AfsBottom As New NumParam With {.Text = "     Bottom", .HelpSwitch = "--vpp-afs", .Init = 16}
        Property AfsMethodSwitch As New NumParam With {.Text = "     Method Switch", .HelpSwitch = "--vpp-afs", .Config = {0, 256}}
        Property AfsCoeffShift As New NumParam With {.Text = "     Coeff Shift", .HelpSwitch = "--vpp-afs", .Init = 192, .Config = {0, 256}}
        Property AfsThreShift As New NumParam With {.Text = "          Shift", .Label = "     Threshold", .HelpSwitch = "--vpp-afs", .Init = 128, .Config = {0, 1024}}
        Property AfsThreDeint As New NumParam With {.Text = "          Deint", .HelpSwitch = "--vpp-afs", .Init = 48, .Config = {0, 1024}}
        Property AfsThreMotionY As New NumParam With {.Text = "          Motion Y", .HelpSwitch = "--vpp-afs", .Init = 112, .Config = {0, 1024}}
        Property AfsThreMotionC As New NumParam With {.Text = "          Motion C", .HelpSwitch = "--vpp-afs", .Init = 224, .Config = {0, 1024}}
        Property AfsLevel As New NumParam With {.Text = "     Level", .HelpSwitch = "--vpp-afs", .Init = 3, .Config = {0, 4}}
        Property AfsShift As New BoolParam With {.Text = "Shift", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property AfsDrop As New BoolParam With {.Text = "Drop", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property AfsSmooth As New BoolParam With {.Text = "Smooth", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property Afs24fps As New BoolParam With {.Text = "24 FPS", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property AfsTune As New BoolParam With {.Text = "Tune", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property AfsRFF As New BoolParam With {.Text = "RFF", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property AfsTimecode As New BoolParam With {.Text = "Timecode", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property AfsLog As New BoolParam With {.Text = "Log", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3}

        Property Edgelevel As New BoolParam With {.Text = "Edgelevel filter to enhance edge", .Switches = {"--vpp-edgelevel"}, .ArgsFunc = AddressOf GetEdge}
        Property EdgelevelStrength As New NumParam With {.Text = "     Strength", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property EdgelevelThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-edgelevel", .Init = 20, .Config = {0, 255, 1, 1}}
        Property EdgelevelBlack As New NumParam With {.Text = "     Black", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property EdgelevelWhite As New NumParam With {.Text = "     White", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}

        Property Unsharp As New BoolParam With {.Text = "Unsharp Filter", .Switches = {"--vpp-unsharp"}, .ArgsFunc = AddressOf GetUnsharp}
        Property UnsharpRadius As New NumParam With {.Text = "     Radius", .HelpSwitch = "--vpp-unsharp", .Init = 3, .Config = {1, 9}}
        Property UnsharpWeight As New NumParam With {.Text = "     Weight", .HelpSwitch = "--vpp-unsharp", .Init = 0.5, .Config = {0, 10, 0.5, 1}}
        Property UnsharpThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-unsharp", .Init = 10, .Config = {0, 255, 1, 1}}

        Property Nnedi As New BoolParam With {.Text = "nnedi Deinterlacer", .Switches = {"--vpp-nnedi"}, .ArgsFunc = AddressOf GetNnedi}
        Property NnediField As New OptionParam With {.Text = "     Field", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "top", "bottom"}}
        Property NnediNns As New OptionParam With {.Text = "     NNS", .HelpSwitch = "--vpp-nnedi", .Init = 1, .Options = {"16", "32", "64", "128", "256"}}
        Property NnediNsize As New OptionParam With {.Text = "     N Size", .HelpSwitch = "--vpp-nnedi", .Init = 6, .Options = {"8x6", "16x6", "32x6", "48x6", "8x4", "16x4", "32x4"}}
        Property NnediQuality As New OptionParam With {.Text = "     Quality", .HelpSwitch = "--vpp-nnedi", .Options = {"fast", "slow"}}
        Property NnediPrescreen As New OptionParam With {.Text = "     Pre Screen", .HelpSwitch = "--vpp-nnedi", .Init = 4, .Options = {"none", "original", "new", "original_block", "new_block"}}
        Property NnediErrortype As New OptionParam With {.Text = "     Error Type", .HelpSwitch = "--vpp-nnedi", .Options = {"abs", "square"}}
        Property NnediPrec As New OptionParam With {.Text = "     Prec", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "fp16", "fp32"}}
        Property NnediWeightfile As New StringParam With {.Text = "     Weight File", .HelpSwitch = "--vpp-nnedi", .BrowseFile = True}

        Property SelectEvery As New BoolParam With {.Text = "Select Every", .Switches = {"--vpp-select-every"}, .ArgsFunc = AddressOf GetSelectEvery}
        Property SelectEveryValue As New NumParam With {.Text = "     Value", .HelpSwitch = "--vpp-select-every", .Init = 2}
        Property SelectEveryOffsets As New StringParam With {.Text = "     Offsets", .HelpSwitch = "--vpp-select-every", .Expand = False}

        Property TransformFlipX As New BoolParam With {.Switch = "--vpp-transform", .Text = "Flip X", .Label = "Transform", .LeftMargin = g.MainForm.FontHeight * 1.5, .ArgsFunc = AddressOf GetTransform}
        Property TransformFlipY As New BoolParam With {.Text = "Flip Y", .LeftMargin = g.MainForm.FontHeight * 1.5, .HelpSwitch = "--vpp-transform"}
        Property TransformTranspose As New BoolParam With {.Text = "Transpose", .LeftMargin = g.MainForm.FontHeight * 1.5, .HelpSwitch = "--vpp-transform"}

        Property Smooth As New BoolParam With {.Text = "Smooth", .Switch = "--vpp-smooth", .ArgsFunc = AddressOf GetSmooth}
        Property SmoothQuality As New NumParam With {.Text = "      Quality", .HelpSwitch = "--vpp-smooth", .Init = 3, .Config = {1, 6}}
        Property SmoothQP As New NumParam With {.Text = "      QP", .HelpSwitch = "--vpp-smooth", .Config = {0, 100, 10, 1}}
        Property SmoothPrec As New OptionParam With {.Text = "      Precision", .HelpSwitch = "--vpp-smooth", .Options = {"Auto", "FP16", "FP32"}}

        Property ColorSpace As New BoolParam With {.Text = "Color Space", .Switch = "--vpp-colorspace", .ArgsFunc = AddressOf GetColorSpace}
        Property ColorSpaceMatrixIn As New OptionParam With {.Text = "      Matrix In", .HelpSwitch = "--vpp-colorspace", .Options = {"disabled", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c", "auto"}}
        Property ColorSpaceMatrixOut As New OptionParam With {.Text = "      Matrix Out", .HelpSwitch = "--vpp-colorspace", .Options = {"disabled", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c", "auto"}}
        Property ColorSpaceColorPrimIn As New OptionParam With {.Text = "      ColorPrim In", .HelpSwitch = "--vpp-colorspace", .Options = {"disabled", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020", "auto"}}
        Property ColorSpaceColorPrimOut As New OptionParam With {.Text = "      ColorPrim Out", .HelpSwitch = "--vpp-colorspace", .Options = {"disabled", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020", "auto"}}
        Property ColorSpaceTransferIn As New OptionParam With {.Text = "      Transfer In", .HelpSwitch = "--vpp-colorspace", .Options = {"disabled", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "arib-srd-b67", "auto"}}
        Property ColorSpaceTransferOut As New OptionParam With {.Text = "      Transfer Out", .HelpSwitch = "--vpp-colorspace", .Options = {"disabled", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "arib-srd-b67", "auto"}}
        Property ColorSpaceRangeIn As New OptionParam With {.Text = "      Range In", .HelpSwitch = "--vpp-colorspace", .Options = {"disabled", "limited", "full", "auto"}}
        Property ColorSpaceRangeOut As New OptionParam With {.Text = "      Range Out", .HelpSwitch = "--vpp-colorspace", .Options = {"disabled", "limited", "full", "auto"}}
        Property ColorSpaceHDR2SDR As New OptionParam With {.Text = "      HDR2SDR", .HelpSwitch = "--vpp-colorspace", .Options = {"none", "hable", "mobius", "reinhard"}}
        Property ColorSpaceSourcePeak As New NumParam With {.Text = "      Source_Peak", .HelpSwitch = "--vpp-colorspace", .Init = 1000, .Config = {1, 100000, 50, 1}}
        Property ColorSpaceLDRNits As New NumParam With {.Text = "      LDR_Nits", .HelpSwitch = "--vpp-colorspace", .Init = 100, .Config = {1, 20000, 10, 1}}

        Property ColorSpaceHDRpA As New NumParam With {.Text = "      Hable A par", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function() ColorSpaceHDR2SDR.Value = 1, .Init = 0.22, .Config = {0, 10, 0.02, 2}}
        Property ColorSpaceHDRpB As New NumParam With {.Text = "      Hable B par", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function() ColorSpaceHDR2SDR.Value = 1, .Init = 0.3, .Config = {0, 10, 0.02, 2}}
        Property ColorSpaceHDRpC As New NumParam With {.Text = "      Hable C par", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function() ColorSpaceHDR2SDR.Value = 1, .Init = 0.1, .Config = {0, 10, 0.02, 2}}
        Property ColorSpaceHDRpD As New NumParam With {.Text = "      Hable D par", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function() ColorSpaceHDR2SDR.Value = 1, .Init = 0.2, .Config = {0, 10, 0.02, 2}}
        Property ColorSpaceHDRpE As New NumParam With {.Text = "      Hable E par", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function() ColorSpaceHDR2SDR.Value = 1, .Init = 0.01, .Config = {0, 10, 0.02, 2}}
        Property ColorSpaceHDRpF As New NumParam With {.Text = "      Hable F par", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function() ColorSpaceHDR2SDR.Value = 1, .Init = 0.3, .Config = {0, 10, 0.02, 2}}
        Property ColorSpaceHDRTransition As New NumParam With {.Text = "      Mobius Transition", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function() ColorSpaceHDR2SDR.Value = 2, .Init = 0.3, .Config = {0, 10, 0.02, 2}}
        Property ColorSpaceHDRPeak As New NumParam With {.Text = "      Mobius/Reinhard Peak", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function()
                                                                                                                                                    Dim csHDRVal As Integer = ColorSpaceHDR2SDR.Value
                                                                                                                                                    Return csHDRVal = 2 OrElse csHDRVal = 3
                                                                                                                                                End Function, .Init = 1, .Config = {0, 10, 0.02, 2}}
        Property ColorSpaceHDRContrast As New NumParam With {.Text = "      Reinhard Contrast", .HelpSwitch = "--vpp-colorspace", .VisibleFunc = Function() ColorSpaceHDR2SDR.Value = 3, .Init = 0.5, .Config = {0, 10, 0.02, 2}}


        'TO DO: Add: --atc-sei, --repeat-headers(check this),  Decimate,  MpDecimate and VFR

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)(208)
                    'To DO: 8 bit depth switch show and default auto. Is really needed?, can force 8b with main10 profile, checking 4:4:4 is needed
                    Add("Basic", Mode, Multipass, Decoder, Codec,
                        New OptionParam With {.Switch = "--preset", .HelpSwitch = "-u", .Text = "Presets", .Init = 6, .Options = {"Default (=P4)", "Max Quality (=P7)", "Performance (=P1)", "P1 (=Performance)", "P2", "P3", "P4 (=Default)", "P5", "P6", "P7 (=Quality)"}, .Values = {"default", "quality", "performance", "P1", "P2", "P3", "P4", "P5", "P6", "P7"}},
                        Profile, ProfileH265,
                        New OptionParam With {.Switch = "--tier", .Text = "Tier", .Value = 1, .VisibleFunc = Function() Codec.Value = 1, .Options = {"Main", "High"}, .Values = {"main", "high"}},
                        New OptionParam With {.Name = "LevelH264", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.Value = 0, .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Name = "LevelH265", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.Value = 1, .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Switch = "--output-depth", .Text = "Depth", .VisibleFunc = Function() Codec.Value = 1, .Options = {"Default", "8-Bit", "10-Bit"}, .Values = {"8", "8", "10"}},
                        QPAdvanced, QP, QPI, QPP, QPB)
                    Add("Rate Control",
                        New StringParam With {.Switch = "--dynamic-rc", .Text = "Dynamic RC"},
                        New NumParam With {.Switch = "--qp-init", .Text = "Initial QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate", .Init = 17500, .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--aq-strength", .Text = "AQ Strength", .Config = {0, 15}, .VisibleFunc = Function() AQ.Value},
                        VbrQuality,
                        ConstantQualityMode,
                        AQ,
                        New BoolParam With {.Switch = "--aq-temporal", .Text = "Adaptive Quantization (Temporal)"},
                        Lossless)
                    Add("Slice Decision",
                        New OptionParam With {.Switch = "--direct", .Text = "B-Direct Mode", .Options = {"Automatic", "None", "Spatial", "Temporal"}, .VisibleFunc = Function() Codec.Value = 0},
                        New OptionParam With {.Switch = "--bref-mode", .Text = "B-Frame Ref. Mode", .Options = {"Disabled", "Each", "Middle"}},
                        New NumParam With {.Switch = "--bframes", .HelpSwitch = "-b", .Text = "B-Frames", .Init = 3, .Config = {0, 16}},
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames", .Init = 3, .Config = {0, 16}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--lookahead", .Text = "Lookahead", .Config = {0, 32}},
                        New NumParam With {.Switch = "--slices", .Text = "Slices", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--multiref-l0", .Text = "Multi Ref L0", .Config = {0, 7}},
                        New NumParam With {.Switch = "--multiref-l1", .Text = "Multi Ref L1", .Config = {0, 7}},
                        New BoolParam With {.Switch = "--strict-gop", .Text = "Strict GOP"},
                        New BoolParam With {.NoSwitch = "--no-b-adapt", .Text = "B-Adapt", .Init = True},
                        New BoolParam With {.NoSwitch = "--no-i-adapt", .Text = "I-Adapt", .Init = True},
                        New BoolParam With {.Switch = "--nonrefp", .Text = "Enable adapt. non-reference P frame insertion"})
                    Add("Analysis",
                        New OptionParam With {.Switch = "--adapt-transform", .Text = "Adaptive Transform", .Options = {"Automatic", "Enabled", "Disabled"}, .Values = {"", "--adapt-transform", "--no-adapt-transform"}, .VisibleFunc = Function() Codec.Value = 0},
                        New NumParam With {.Switch = "--cu-min", .VisibleFunc = Function() Codec.Value = 1, .Text = "Minimum CU Size", .Config = {0, 32, 16}},
                        New NumParam With {.Switch = "--cu-max", .VisibleFunc = Function() Codec.Value = 1, .Text = "Maximum CU Size", .Config = {0, 64, 16}},
                        New BoolParam With {.Switch = "--weightp", .Text = "Enable weighted prediction in P slices"})
                    Add("Performance",
                        New StringParam With {.Switch = "--perf-monitor", .Text = "Perf. Monitor"},
                        New OptionParam With {.Switch = "--cuda-schedule", .Text = "Cuda Schedule", .Expand = True, .Init = 3, .Options = {"Let cuda driver to decide", "CPU will spin when waiting GPU tasks", "CPU will yield when waiting GPU tasks", "CPU will sleep when waiting GPU tasks"}, .Values = {"auto", "spin", "yield", "sync"}},
                        New OptionParam With {.Switch = "--output-buf", .Text = "Output Buffer", .Options = {"8", "16", "32", "64", "128"}},
                        New OptionParam With {.Switch = "--output-thread", .Text = "Output Thread", .Options = {"Automatic", "Disabled", "One Thread"}, .Values = {"-1", "0", "1"}},
                        New NumParam With {.Switch = "--perf-monitor-interval", .Init = 500, .Config = {50, Integer.MaxValue}, .Text = "Perf. Mon. Interval"},
                        New BoolParam With {.Switch = "--max-procfps", .Text = "Limit performance to lower resource usage"},
                        New BoolParam With {.Switch = "--lowlatency", .Text = "Low Latency"})
                    Add("VUI",
                        New StringParam With {.Switch = "--master-display", .Text = "Master Display", .VisibleFunc = Function() Codec.Value = 1},
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New StringParam With {.Switch = "--dhdr10-info", .Text = "HDR10 Info File", .BrowseFile = True},
                        New OptionParam With {.Switch = "--input-csp", .Text = "Input CSP", .Init = 1, .Options = {"NV12", "YV12", "YUV420P", "YUV422P", "YUV444P", "YUV420P9LE", "YUV420P10LE", "YUV420P12LE", "YUV420P14LE", "YUV420P16LE", "P010", "YUV422P9LE", "YUV422P10LE", "YUV422P12LE", "YUV422P14LE", "YUV422P16LE", "YUV444P9LE", "YUV444P10LE", "YUV444P12LE", "YUV444P14LE", "YUV444P16LE"}},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC", "Auto"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 240 M", "YCgCo", "Auto"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M", "Auto"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "ARIB-SRD-B67", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 2084", "SMPTE 428", "Auto"}},
                        New OptionParam With {.Switch = "--colorrange", .Text = "Colorrange", .Options = {"Undefined", "Limited", "TV", "Full", "PC", "Auto"}},
                        MaxCLL, MaxFALL,
                        New NumParam With {.Switch = "--chromaloc", .Text = "Chromaloc", .Config = {0, 5}},
                        New BoolParam With {.Switch = "--pic-struct", .Text = "Set the picture structure and emits it in the picture timing SEI message"},
                        New BoolParam With {.Switch = "--aud", .Text = "Insert Access Unit Delimiter NAL"},
                        New BoolParam With {.Switch = "--repeat-headers", .Text = "Output VPS, SPS and PPS for every IDR frame"})
                    Add("VPP | Misc",
                        New StringParam With {.Switch = "--vpp-subburn", .Text = "Subburn"},
                        New OptionParam With {.Switch = "--vpp-resize", .Text = "Resize", .Options = {"Disabled", "Default", "Bilinear", "Cubic_B05C03", "Cubic_bSpline", "Cubic_Catmull", "spline16", "Spline36", "spline64", "lanczos2", "lanczos3", "lanczos4", "Cubic", "Super", "NN", "NPP_Linear", "Lanczos"}},
                        New OptionParam With {.Switch = "--vpp-deinterlace", .Text = "Deinterlace", .VisibleFunc = Function()
                                                                                                                       Dim decVal As Integer = Decoder.Value
                                                                                                                       Return decVal = 1 OrElse decVal = 2
                                                                                                                   End Function, .Options = {"None", "Adaptive", "Bob"}},
                        New OptionParam With {.Switch = "--vpp-gauss", .Text = "Gauss", .Options = {"Disabled", "3", "5", "7"}},
                        New OptionParam With {.Switch = "--vpp-rotate", .Text = "Rotate", .Options = {"Disabled", "90", "180", "270"}},
                        New BoolParam With {.Switch = "--vpp-rff", .Text = "Enable repeat field flag", .VisibleFunc = Function()
                                                                                                                          Dim decVal As Integer = Decoder.Value
                                                                                                                          Return decVal = 1 OrElse decVal = 2
                                                                                                                      End Function},
                        Edgelevel,
                        EdgelevelStrength,
                        EdgelevelThreshold,
                        EdgelevelBlack,
                        EdgelevelWhite,
                        Unsharp,
                        UnsharpRadius,
                        UnsharpWeight,
                        UnsharpThreshold,
                        SelectEvery,
                        SelectEveryValue,
                        SelectEveryOffsets)
                    Add("VPP | Misc 2",
                        Tweak,
                        TweakBrightness,
                        TweakContrast,
                        TweakSaturation,
                        TweakGamma,
                        TweakHue,
                        TweakSwapUV,
                        Pad,
                        PadLeft,
                        PadTop,
                        PadRight,
                        PadBottom,
                        Smooth,
                        SmoothQuality,
                        SmoothQP,
                        SmoothPrec)
                    Add("VPP | Misc 3",
                        TransformFlipX,
                        TransformFlipY,
                        TransformTranspose)

                    Add("VPP | Color Space",
                        ColorSpace,
                        ColorSpaceMatrixIn,
                        ColorSpaceMatrixOut,
                        ColorSpaceColorPrimIn,
                        ColorSpaceColorPrimOut,
                        ColorSpaceTransferIn,
                        ColorSpaceTransferOut,
                        ColorSpaceRangeIn,
                        ColorSpaceRangeOut,
                        ColorSpaceHDR2SDR,
                        ColorSpaceSourcePeak,
                        ColorSpaceLDRNits,
                        ColorSpaceHDRpA,
                        ColorSpaceHDRpB,
                        ColorSpaceHDRpC,
                        ColorSpaceHDRpD,
                        ColorSpaceHDRpE,
                        ColorSpaceHDRpF,
                        ColorSpaceHDRTransition,
                        ColorSpaceHDRContrast,
                        ColorSpaceHDRPeak)

                    Add("VPP | Denoise",
                        Knn, KnnRadius, KnnStrength, KnnLerp, KnnThLerp,
                        Pmd, PmdApplyCount, PmdStrength, PmdThreshold)
                    Add("VPP | Deband",
                        Deband,
                        DebandRange,
                        DebandSample,
                        DebandThre,
                        DebandThreY,
                        DebandThreCB,
                        DebandThreCR,
                        DebandDither,
                        DebandDitherY,
                        DebandDitherC,
                        DebandSeed,
                        DebandBlurfirst,
                        DebandRandEachFrame)
                    Add("VPP | Deinterlace",
                        Nnedi,
                        NnediField,
                        NnediNns,
                        NnediNsize,
                        NnediQuality,
                        NnediPrescreen,
                        NnediErrortype,
                        NnediPrec,
                        NnediWeightfile,
                        New OptionParam With {.Switch = "--vpp-yadif", .Text = "Yadif", .Options = {"Disabled", "Auto", "TFF", "BFF", "Bob", "Bob TFF", "Bob BFF"}, .Values = {"", "", "mode=tff", "mode=bff", "mode=bob", "mode=bob_tff", "mode=bob_bff"}})
                    Add("VPP | AFS 1",
                        Afs,
                        AfsINI,
                        AfsPreset,
                        AfsLeft,
                        AfsRight,
                        AfsTop,
                        AfsBottom,
                        AfsMethodSwitch,
                        AfsCoeffShift,
                        AfsThreShift,
                        AfsThreDeint,
                        AfsThreMotionY,
                        AfsThreMotionC,
                        AfsLevel)
                    Add("VPP | AFS 2",
                        AfsShift,
                        AfsDrop,
                        AfsSmooth,
                        Afs24fps,
                        AfsTune,
                        AfsRFF,
                        AfsTimecode,
                        AfsLog)
                    Add("Statistic",
                        New StringParam With {.Switch = "--vmaf", .Text = "VMAF"},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error"}},
                        New BoolParam With {.Switch = "--ssim", .Text = "SSIM"},
                        New BoolParam With {.Switch = "--psnr", .Text = "PSNR"})
                    Add("Other",
                        Custom,
                        New StringParam With {.Switch = "--sub-source", .Text = "Subtitle File", .BrowseFile = True, .BrowseFileFilter = FileTypes.GetFilter(FileTypes.SubtitleExludingContainers)},
                        New StringParam With {.Switch = "--keyfile", .Text = "Keyframes File", .BrowseFile = True},
                        New StringParam With {.Switch = "--data-copy", .Text = "Data Copy"},
                        New StringParam With {.Switch = "--input-option", .Text = "Input Option"},
                        New OptionParam With {.Switch = "--mv-precision", .Text = "MV Precision", .Options = {"Automatic", "Q-pel", "Half-pel", "Full-pel"}},
                        New OptionParam With {.Switches = {"--cabac", "--cavlc"}, .VisibleFunc = Function() Codec.Value = 0, .Text = "Cabac/Cavlc", .Options = {"Disabled", "Cabac", "Cavlc"}, .Values = {"", "--cabac", "--cavlc"}},
                        Interlace,
                        New NumParam With {.Switch = "--device", .HelpSwitch = "-d", .Text = "Device", .Config = {0, 4}},
                        New BoolParam With {.Switch = "--deblock", .NoSwitch = "--no-deblock", .Text = "Deblock", .Init = True},
                        New BoolParam With {.Switch = "--bluray", .Text = "Blu-ray"})

                    For Each item In ItemsValue
                        If item.HelpSwitch?.Length > 0 Then
                            Continue For
                        End If

                        Dim switches = item.GetSwitches

                        If switches.NothingOrEmpty Then
                            Continue For
                        End If

                        item.HelpSwitch = switches(0)
                    Next
                End If

                Return ItemsValue
            End Get
        End Property

        Public Overrides Sub ShowHelp(id As String)
            g.ShowCommandLineHelp(Package.NVEnc, id)
        End Sub

        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
            'If Decoder.MenuButton IsNot Nothing AndAlso (item Is Decoder OrElse item Is Nothing) Then
            '    Dim isIntelPresent As Boolean = True '(OS.VideoControllers.Item1 And 2) = 2 '2-Intel,4-Nvidia,6-Nvidia+Intel, 128 Error
            '    'For x = 0 To Decoder.Options.Length - 1
            '    ' If Decoder.Options(x).Contains("Intel") Then 
            '    Decoder.ShowOption(3, isIntelPresent)
            '    Decoder.ShowOption(4, isIntelPresent)
            'End If
            If QPI.NumEdit IsNot Nothing Then
                Dim nnVal As Boolean = Nnedi.Value
                NnediField.MenuButton.Enabled = nnVal
                NnediNns.MenuButton.Enabled = nnVal
                NnediNsize.MenuButton.Enabled = nnVal
                NnediQuality.MenuButton.Enabled = nnVal
                NnediPrescreen.MenuButton.Enabled = nnVal
                NnediErrortype.MenuButton.Enabled = nnVal
                NnediPrec.MenuButton.Enabled = nnVal
                NnediWeightfile.TextEdit.Enabled = nnVal

                Dim elVal As Boolean = Edgelevel.Value
                EdgelevelStrength.NumEdit.Enabled = elVal
                EdgelevelThreshold.NumEdit.Enabled = elVal
                EdgelevelBlack.NumEdit.Enabled = elVal
                EdgelevelWhite.NumEdit.Enabled = elVal

                Dim usVal As Boolean = Unsharp.Value
                UnsharpRadius.NumEdit.Enabled = usVal
                UnsharpWeight.NumEdit.Enabled = usVal
                UnsharpThreshold.NumEdit.Enabled = usVal

                Dim seVal As Boolean = SelectEvery.Value
                SelectEveryValue.NumEdit.Enabled = seVal
                SelectEveryOffsets.TextEdit.Enabled = seVal

                Dim knnVal As Boolean = Knn.Value
                KnnRadius.NumEdit.Enabled = knnVal
                KnnStrength.NumEdit.Enabled = knnVal
                KnnLerp.NumEdit.Enabled = knnVal
                KnnThLerp.NumEdit.Enabled = knnVal

                Dim pVal As Boolean = Pad.Value
                PadLeft.NumEdit.Enabled = pVal
                PadTop.NumEdit.Enabled = pVal
                PadRight.NumEdit.Enabled = pVal
                PadBottom.NumEdit.Enabled = pVal

                Dim sVal As Boolean = Smooth.Value
                SmoothQuality.NumEdit.Enabled = sVal
                SmoothQP.NumEdit.Enabled = sVal
                SmoothPrec.MenuButton.Enabled = sVal

                Dim twVal As Boolean = Tweak.Value
                TweakContrast.NumEdit.Enabled = twVal
                TweakGamma.NumEdit.Enabled = twVal
                TweakSaturation.NumEdit.Enabled = twVal
                TweakHue.NumEdit.Enabled = twVal
                TweakSwapUV.CheckBox.Enabled = twVal
                TweakBrightness.NumEdit.Enabled = twVal

                Dim pmdVal As Boolean = Pmd.Value
                PmdApplyCount.NumEdit.Enabled = pmdVal
                PmdStrength.NumEdit.Enabled = pmdVal
                PmdThreshold.NumEdit.Enabled = pmdVal

                Dim dbVal As Boolean = Deband.Value
                For Each i In {DebandRange, DebandSample, DebandThre, DebandThreY, DebandThreCB,
                    DebandThreCR, DebandDither, DebandDitherY, DebandDitherC, DebandSeed}

                    i.NumEdit.Enabled = dbVal
                Next

                DebandRandEachFrame.CheckBox.Enabled = dbVal
                DebandBlurfirst.CheckBox.Enabled = dbVal

                Dim aVal As Boolean = Afs.Value
                AfsPreset.MenuButton.Enabled = aVal
                AfsINI.TextEdit.Enabled = aVal

                For Each i In {AfsLeft, AfsRight, AfsTop, AfsBottom, AfsMethodSwitch, AfsCoeffShift,
                               AfsThreShift, AfsThreDeint, AfsThreMotionY, AfsThreMotionC, AfsLevel}

                    i.NumEdit.Enabled = aVal
                Next

                For Each i In {AfsShift, AfsDrop, AfsSmooth, Afs24fps, AfsTune, AfsRFF, AfsTimecode, AfsLog}
                    i.CheckBox.Enabled = aVal
                Next

                Dim csVal As Boolean = ColorSpace.Value
                ColorSpaceMatrixIn.MenuButton.Enabled = csVal
                ColorSpaceMatrixOut.MenuButton.Enabled = csVal
                ColorSpaceColorPrimIn.MenuButton.Enabled = csVal
                ColorSpaceColorPrimOut.MenuButton.Enabled = csVal
                ColorSpaceTransferIn.MenuButton.Enabled = csVal
                ColorSpaceTransferOut.MenuButton.Enabled = csVal
                ColorSpaceRangeIn.MenuButton.Enabled = csVal
                ColorSpaceRangeOut.MenuButton.Enabled = csVal
                ColorSpaceHDR2SDR.MenuButton.Enabled = csVal

                ColorSpaceSourcePeak.NumEdit.Enabled = csVal
                ColorSpaceLDRNits.NumEdit.Enabled = csVal
                ColorSpaceHDRpA.NumEdit.Enabled = csVal
                ColorSpaceHDRpB.NumEdit.Enabled = csVal
                ColorSpaceHDRpC.NumEdit.Enabled = csVal
                ColorSpaceHDRpD.NumEdit.Enabled = csVal
                ColorSpaceHDRpE.NumEdit.Enabled = csVal
                ColorSpaceHDRpF.NumEdit.Enabled = csVal
                ColorSpaceHDRTransition.NumEdit.Enabled = csVal
                ColorSpaceHDRContrast.NumEdit.Enabled = csVal
                ColorSpaceHDRPeak.NumEdit.Enabled = csVal

                ColorSpaceMatrixOut.MenuButton.Enabled = csVal AndAlso ColorSpaceMatrixIn.Value <> ColorSpaceMatrixIn.DefaultValue
                ColorSpaceColorPrimOut.MenuButton.Enabled = csVal AndAlso ColorSpaceColorPrimIn.Value <> ColorSpaceColorPrimIn.DefaultValue
                ColorSpaceTransferOut.MenuButton.Enabled = csVal AndAlso ColorSpaceTransferIn.Value <> ColorSpaceTransferIn.DefaultValue
                ColorSpaceRangeOut.MenuButton.Enabled = csVal AndAlso ColorSpaceRangeIn.Value <> ColorSpaceRangeIn.DefaultValue
            End If

            MyBase.OnValueChanged(item)
        End Sub

        Function GetSmooth() As String
            If Smooth.Value Then
                Dim ret = ""

                If SmoothQuality.Value <> SmoothQuality.DefaultValue Then
                    ret &= ",quality=" & SmoothQuality.Value
                End If

                If SmoothQP.Value <> SmoothQP.DefaultValue Then
                    ret &= ",qp=" & SmoothQP.Value.ToInvariantString
                End If

                If SmoothPrec.Value <> SmoothPrec.DefaultValue Then
                    ret &= ",prec=" & SmoothPrec.ValueText
                End If

                If ret.Length > 0 Then
                    Return "--vpp-smooth " & ret.TrimStart(","c)
                Else
                    Return "--vpp-smooth"
                End If
            End If
        End Function

        Function GetColorSpace() As String
            If ColorSpace.Value Then
                Dim sb As New StringBuilder(208)

                If ColorSpaceMatrixIn.Value <> ColorSpaceMatrixIn.DefaultValue AndAlso ColorSpaceMatrixOut.Value <> ColorSpaceMatrixOut.DefaultValue Then
                    sb.Append(",matrix=").Append(ColorSpaceMatrixIn.ValueText).Append(":").Append(ColorSpaceMatrixOut.ValueText)
                End If

                If ColorSpaceColorPrimIn.Value <> ColorSpaceColorPrimIn.DefaultValue AndAlso ColorSpaceColorPrimOut.Value <> ColorSpaceColorPrimOut.DefaultValue Then
                    sb.Append(",colorprim=").Append(ColorSpaceColorPrimIn.ValueText).Append(":").Append(ColorSpaceColorPrimOut.ValueText)
                End If

                If ColorSpaceTransferIn.Value <> ColorSpaceTransferIn.DefaultValue AndAlso ColorSpaceTransferOut.Value <> ColorSpaceTransferOut.DefaultValue Then
                    sb.Append(",transfer=").Append(ColorSpaceTransferIn.ValueText).Append(":").Append(ColorSpaceTransferOut.ValueText)
                End If

                If ColorSpaceRangeIn.Value <> ColorSpaceRangeIn.DefaultValue AndAlso ColorSpaceRangeOut.Value <> ColorSpaceRangeOut.DefaultValue Then
                    sb.Append(",range=").Append(ColorSpaceRangeIn.ValueText).Append(":").Append(ColorSpaceRangeOut.ValueText)
                End If

                If ColorSpaceHDR2SDR.Value <> ColorSpaceHDR2SDR.DefaultValue Then
                    sb.Append(",hdr2sdr=").Append(ColorSpaceHDR2SDR.ValueText)
                    Select Case ColorSpaceHDR2SDR.Value
                        Case = 1 '"hable"
                            If ColorSpaceHDRpA.Value <> ColorSpaceHDRpA.DefaultValue Then
                                sb.Append(",a=").Append(ColorSpaceHDRpA.Value.ToInvariantString)
                            End If
                            If ColorSpaceHDRpB.Value <> ColorSpaceHDRpB.DefaultValue Then
                                sb.Append(",b=").Append(ColorSpaceHDRpB.Value.ToInvariantString)
                            End If
                            If ColorSpaceHDRpC.Value <> ColorSpaceHDRpC.DefaultValue Then
                                sb.Append(",c=").Append(ColorSpaceHDRpC.Value.ToInvariantString)
                            End If
                            If ColorSpaceHDRpD.Value <> ColorSpaceHDRpD.DefaultValue Then
                                sb.Append(",d=").Append(ColorSpaceHDRpD.Value.ToInvariantString)
                            End If
                            If ColorSpaceHDRpE.Value <> ColorSpaceHDRpE.DefaultValue Then
                                sb.Append(",e=").Append(ColorSpaceHDRpE.Value.ToInvariantString)
                            End If
                            If ColorSpaceHDRpF.Value <> ColorSpaceHDRpF.DefaultValue Then
                                sb.Append(",f=").Append(ColorSpaceHDRpF.Value.ToInvariantString)
                            End If
                        Case = 2 ' "mobius"
                            If ColorSpaceHDRTransition.Value <> ColorSpaceHDRTransition.DefaultValue Then
                                sb.Append(",transition=").Append(ColorSpaceHDRTransition.Value.ToInvariantString)
                            End If
                            If ColorSpaceHDRPeak.Value <> ColorSpaceHDRPeak.DefaultValue Then
                                sb.Append(",peak=").Append(ColorSpaceHDRPeak.Value.ToInvariantString)
                            End If
                        Case = 3 '"reinhard"
                            If ColorSpaceHDRContrast.Value <> ColorSpaceHDRContrast.DefaultValue Then
                                sb.Append(",contrast=").Append(ColorSpaceHDRContrast.Value.ToInvariantString)
                            End If
                            If ColorSpaceHDRPeak.Value <> ColorSpaceHDRPeak.DefaultValue Then
                                sb.Append(",peak=").Append(ColorSpaceHDRPeak.Value.ToInvariantString)
                            End If
                    End Select
                End If

                If ColorSpaceSourcePeak.Value <> ColorSpaceSourcePeak.DefaultValue Then
                    sb.Append(",source_peak=").Append(ColorSpaceSourcePeak.Value.ToInvariantString)
                End If

                If ColorSpaceLDRNits.Value <> ColorSpaceLDRNits.DefaultValue Then
                    sb.Append(",ldr_nits=").Append(ColorSpaceLDRNits.Value.ToInvariantString)
                End If

                Dim sbL = sb.Length
                If sbL > 0 Then
                    'Return "--vpp-colorspace " & ret.TrimStart(","c).TrimEnd
                    Return "--vpp-colorspace " & If(sb.Chars(0) = ","c, sb.ToString(1, sbL - 1), sb.ToString).TrimEnd
                Else
                    Return "--vpp-colorspace"
                End If

            End If
        End Function


        Function GetPmdArgs() As String
            If Pmd.Value Then
                Dim ret = ""

                If PmdApplyCount.Value <> PmdApplyCount.DefaultValue Then
                    ret &= ",apply_count=" & PmdApplyCount.Value
                End If

                If PmdStrength.Value <> PmdStrength.DefaultValue Then
                    ret &= ",strength=" & PmdStrength.Value
                End If

                If PmdThreshold.Value <> PmdThreshold.DefaultValue Then
                    ret &= ",threshold=" & PmdThreshold.Value
                End If

                If ret.Length > 0 Then
                    Return "--vpp-pmd " & ret.TrimStart(","c)
                Else
                    Return "--vpp-pmd"
                End If
            End If
        End Function

        Function GetTweakArgs() As String
            If Tweak.Value Then
                Dim ret = ""

                If TweakBrightness.Value <> TweakBrightness.DefaultValue Then
                    ret &= "brightness=" & TweakBrightness.Value.ToInvariantString
                End If

                If TweakContrast.Value <> TweakContrast.DefaultValue Then
                    ret &= ",contrast=" & TweakContrast.Value.ToInvariantString
                End If

                If TweakSaturation.Value <> TweakSaturation.DefaultValue Then
                    ret &= ",saturation=" & TweakSaturation.Value.ToInvariantString
                End If

                If TweakGamma.Value <> TweakGamma.DefaultValue Then
                    ret &= ",gamma=" & TweakGamma.Value.ToInvariantString
                End If

                If TweakHue.Value <> TweakHue.DefaultValue Then
                    ret &= ",hue=" & TweakHue.Value.ToInvariantString
                End If

                If TweakSwapUV.Value Then
                    ret &= ",swapuv=true"
                End If

                If ret.Length > 0 Then
                    Return "--vpp-tweak " & ret.TrimStart(","c)
                Else
                    Return "--vpp-tweak"
                End If
            End If
        End Function

        Function GetPaddingArgs() As String
            If Pad.Value Then
                Dim ret = ""

                If PadLeft.Value <> PadLeft.DefaultValue Then
                    ret &= "" & PadLeft.Value
                End If

                If PadTop.Value <> PadTop.DefaultValue Then
                    ret &= "," & PadTop.Value
                End If

                If PadRight.Value <> PadRight.DefaultValue Then
                    ret &= "," & PadRight.Value
                End If

                If PadBottom.Value <> PadBottom.DefaultValue Then
                    ret &= "," & PadBottom.Value
                End If

                If ret.Length > 0 Then
                    Return "--vpp-pad " & ret.TrimStart(","c)
                Else
                    Return "--vpp-pad "
                End If
            End If
        End Function

        Function GetKnnArgs() As String
            If Knn.Value Then
                Dim ret = ""

                If KnnRadius.Value <> KnnRadius.DefaultValue Then
                    ret &= ",radius=" & KnnRadius.Value
                End If

                If KnnStrength.Value <> KnnStrength.DefaultValue Then
                    ret &= ",strength=" & KnnStrength.Value.ToInvariantString
                End If

                If KnnLerp.Value <> KnnLerp.DefaultValue Then
                    ret &= ",lerp=" & KnnLerp.Value.ToInvariantString
                End If

                If KnnThLerp.Value <> KnnThLerp.DefaultValue Then
                    ret &= ",th_lerp=" & KnnThLerp.Value.ToInvariantString
                End If

                If ret.Length > 0 Then
                    Return "--vpp-knn " & ret.TrimStart(","c)
                Else
                    Return "--vpp-knn"
                End If
            End If
        End Function

        Function GetDebandArgs() As String
            Dim ret = ""

            If DebandRange.Value <> DebandRange.DefaultValue Then
                ret &= ",range=" & DebandRange.Value
            End If

            If DebandSample.Value <> DebandSample.DefaultValue Then
                ret &= ",sample=" & DebandSample.Value
            End If

            If DebandThre.Value <> DebandThre.DefaultValue Then
                ret &= ",thre=" & DebandThre.Value
            End If

            If DebandThreY.Value <> DebandThreY.DefaultValue Then
                ret &= ",thre_y=" & DebandThreY.Value
            End If

            If DebandThreCB.Value <> DebandThreCB.DefaultValue Then
                ret &= ",thre_cb=" & DebandThreCB.Value
            End If

            If DebandThreCR.Value <> DebandThreCR.DefaultValue Then
                ret &= ",thre_cr=" & DebandThreCR.Value
            End If

            If DebandDither.Value <> DebandDither.DefaultValue Then
                ret &= ",dither=" & DebandDither.Value
            End If

            If DebandDitherY.Value <> DebandDitherY.DefaultValue Then
                ret &= ",dither_y=" & DebandDitherY.Value
            End If

            If DebandDitherC.Value <> DebandDitherC.DefaultValue Then
                ret &= ",dither_c=" & DebandDitherC.Value
            End If

            If DebandSeed.Value <> DebandSeed.DefaultValue Then
                ret &= ",seed=" & DebandSeed.Value
            End If

            If DebandBlurfirst.Value Then
                ret &= "," & "blurfirst"
            End If

            If DebandRandEachFrame.Value Then
                ret &= "," & "rand_each_frame"
            End If

            If Deband.Value Then
                Return ("--vpp-deband " & ret.TrimStart(","c)).TrimEnd
            End If
        End Function

        Function GetUnsharp() As String
            Dim ret = ""

            If UnsharpRadius.Value <> UnsharpRadius.DefaultValue Then
                ret &= "radius=" & UnsharpRadius.Value
            End If

            If UnsharpWeight.Value <> UnsharpWeight.DefaultValue Then
                ret &= ",weight=" & UnsharpWeight.Value.ToInvariantString
            End If

            If UnsharpThreshold.Value <> UnsharpThreshold.DefaultValue Then
                ret &= ",threshold=" & UnsharpThreshold.Value
            End If

            If Unsharp.Value Then
                Return ("--vpp-unsharp " & ret.TrimStart(","c)).TrimEnd
            End If
        End Function

        Function GetEdge() As String
            Dim ret = ""

            If EdgelevelStrength.Value <> EdgelevelStrength.DefaultValue Then
                ret &= "strength=" & EdgelevelStrength.Value
            End If

            If EdgelevelThreshold.Value <> EdgelevelThreshold.DefaultValue Then
                ret &= ",threshold=" & EdgelevelThreshold.Value
            End If

            If EdgelevelBlack.Value <> EdgelevelBlack.DefaultValue Then
                ret &= ",black=" & EdgelevelBlack.Value
            End If

            If EdgelevelWhite.Value <> EdgelevelWhite.DefaultValue Then
                ret &= ",white=" & EdgelevelWhite.Value
            End If

            If Edgelevel.Value Then
                Return ("--vpp-edgelevel " & ret.TrimStart(","c)).TrimEnd
            End If
        End Function

        Function GetTransform() As String
            Dim ret = ""

            If TransformFlipX.Value Then
                ret &= "flip_x=true"
            End If

            If TransformFlipY.Value Then
                ret &= ",flip_y=true"
            End If

            If TransformTranspose.Value Then
                ret &= ",transpose=true"
            End If

            If ret.Length > 0 Then
                Return ("--vpp-transform " & ret.TrimStart(","c)).TrimEnd
            End If
        End Function

        Function GetSelectEvery() As String
            Dim ret = ""

            ret &= SelectEveryValue.Value.ToString
            ret &= "," & String.Join(","c, SelectEveryOffsets.Value.Split(({" "c, ","c, ";"c}), StringSplitOptions.RemoveEmptyEntries).SelectF(Function(item) "offset=" & item))

            If SelectEvery.Value Then
                Return ("--vpp-select-every " & ret.TrimStart(","c)).TrimEnd(","c)
            End If
        End Function

        Function GetNnedi() As String
            Dim ret = ""

            If NnediField.Value <> NnediField.DefaultValue Then
                ret &= "field=" & NnediField.ValueText
            End If

            If NnediNns.Value <> NnediNns.DefaultValue Then
                ret &= ",nns=" & NnediNns.ValueText
            End If

            If NnediNsize.Value <> NnediNsize.DefaultValue Then
                ret &= ",nsize=" & NnediNsize.ValueText
            End If

            If NnediQuality.Value <> NnediQuality.DefaultValue Then
                ret &= ",quality=" & NnediQuality.ValueText
            End If

            If NnediPrescreen.Value <> NnediPrescreen.DefaultValue Then
                ret &= ",prescreen=" & NnediPrescreen.ValueText
            End If

            If NnediErrortype.Value <> NnediErrortype.DefaultValue Then
                ret &= ",errortype=" & NnediErrortype.ValueText
            End If

            If NnediPrec.Value <> NnediPrec.DefaultValue Then
                ret &= ",prec=" & NnediPrec.ValueText
            End If

            If NnediWeightfile.Value?.Length > 0 Then
                ret &= ",weightfile=" & NnediWeightfile.Value.Escape
            End If

            If Nnedi.Value Then
                Return ("--vpp-nnedi " & ret.TrimStart(","c)).TrimEnd
            End If
        End Function

        Function GetAFS() As String
            Dim ret = ""

            If AfsPreset.Value <> AfsPreset.DefaultValue Then
                ret &= "preset=" & AfsPreset.ValueText
            End If

            If AfsINI.Value?.Length > 0 Then
                ret &= ",ini=" & AfsINI.Value.Escape
            End If

            If AfsLeft.Value <> AfsLeft.DefaultValue Then
                ret &= ",left=" & AfsLeft.Value
            End If

            If AfsRight.Value <> AfsRight.DefaultValue Then
                ret &= ",right=" & AfsRight.Value
            End If

            If AfsTop.Value <> AfsTop.DefaultValue Then
                ret &= ",top=" & AfsTop.Value
            End If

            If AfsBottom.Value <> AfsBottom.DefaultValue Then
                ret &= ",bottom=" & AfsBottom.Value
            End If

            If AfsMethodSwitch.Value <> AfsMethodSwitch.DefaultValue Then
                ret &= ",method_switch=" & AfsMethodSwitch.Value
            End If

            If AfsCoeffShift.Value <> AfsCoeffShift.DefaultValue Then
                ret &= ",coeff_shift=" & AfsCoeffShift.Value
            End If

            If AfsThreShift.Value <> AfsThreShift.DefaultValue Then
                ret &= ",thre_shift=" & AfsThreShift.Value
            End If

            If AfsThreDeint.Value <> AfsThreDeint.DefaultValue Then
                ret &= ",thre_deint=" & AfsThreDeint.Value
            End If

            If AfsThreMotionY.Value <> AfsThreMotionY.DefaultValue Then
                ret &= ",thre_motion_y=" & AfsThreMotionY.Value
            End If

            If AfsThreMotionC.Value <> AfsThreMotionC.DefaultValue Then
                ret &= ",thre_motion_c=" & AfsThreMotionC.Value
            End If

            If AfsLevel.Value <> AfsLevel.DefaultValue Then
                ret &= ",level=" & AfsLevel.Value
            End If

            If AfsShift.Value <> AfsShift.DefaultValue Then
                ret &= ",shift=" & If(AfsShift.Value, "on", "off")
            End If

            If AfsDrop.Value <> AfsDrop.DefaultValue Then
                ret &= ",drop=" & If(AfsDrop.Value, "on", "off")
            End If

            If AfsSmooth.Value <> AfsSmooth.DefaultValue Then
                ret &= ",smooth=" & If(AfsSmooth.Value, "on", "off")
            End If

            If Afs24fps.Value <> Afs24fps.DefaultValue Then
                ret &= ",24fps=" & If(Afs24fps.Value, "on", "off")
            End If

            If AfsTune.Value <> AfsTune.DefaultValue Then
                ret &= ",tune=" & If(AfsTune.Value, "on", "off")
            End If

            If AfsRFF.Value <> AfsRFF.DefaultValue Then
                ret &= ",rff=" & If(AfsRFF.Value, "on", "off")
            End If

            If AfsTimecode.Value <> AfsTimecode.DefaultValue Then
                ret &= ",timecode=" & If(AfsTimecode.Value, "on", "off")
            End If

            If AfsLog.Value <> AfsLog.DefaultValue Then
                ret &= ",log=" & If(AfsLog.Value, "on", "off")
            End If

            If Afs.Value Then
                Return ("--vpp-afs " & ret.TrimStart(","c)).TrimEnd
            End If
        End Function

        Function GetModeArgs() As String

            Select Case Mode.Value
                Case 0
                    If QPAdvanced.Value Then
                        Return "--cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
                    Else
                        Return "--cqp " & QP.Value
                    End If
                Case 1
                    Return "--cbr " & p.VideoBitrate
                Case 2
                    Dim bitrate = If(ConstantQualityMode.Value, 0, p.VideoBitrate)
                    Return "--vbr " & bitrate

            End Select
        End Function

        Overrides Function GetCommandLine(includePaths As Boolean, includeExe As Boolean, Optional pass As Integer = 1) As String
            Dim sb As New StringBuilder(512)
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExe Then
                sb.Append(Package.NVEnc.Path.Escape)
            End If

            Dim decVal As Integer = Decoder.Value

            Select Case decVal
                Case 0 '"avs"
                    sourcePath = p.Script.Path

                    'Vapoursynth fix, avisynth is not my thing BTW
                    If includePaths AndAlso FrameServerHelp.IsAviSynthPortableUsed AndAlso p.Script.Engine = ScriptEngine.AviSynth Then
                        sb.Append(" --avsdll ").Append(Package.AviSynth.Path.Escape)
                    End If
                Case 1 '"nvhw"
                    sourcePath = p.LastOriginalSourceFile
                    sb.Append(" --avhw")
                Case 2 '"nvsw"
                    sourcePath = p.LastOriginalSourceFile
                    sb.Append(" --avsw")
                Case 3 '"qs"
                    sourcePath = "-"

                    ' If includePaths Then 'ToDO Test this Remove If
                    sb.Append(If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64")).Append(" -o - -c raw").Append(" -i ").
                            Append(If(includePaths, p.SourceFile.Escape, "path")).Append(" | ").Append(If(includePaths, Package.NVEnc.Path.Escape, "NVEncC64"))
                    ' End If
                Case 4 ' "ffqsv"
                    sourcePath = "-"

                    'If includePaths Then 'ToDO Test this Remove If
                    sb.Append(If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg")).Append(" -threads 1 -hwaccel qsv -i ").Append(If(includePaths, p.SourceFile.Escape, "path")).
                            Append(" -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p").Append(s.GetFFLogLevel(FfLogLevel.fatal)).Append(" -hide_banner - | ").Append(If(includePaths, Package.NVEnc.Path.Escape, "NVEncC64"))
                    ' End If
                Case 5 '"ffcuda"
                    sourcePath = "-"

                    'If includePaths Then 'ToDO Test this Remove If

                    'To DO Check Vsync: 0 - passthrough Each frame Is passed with its timestamp from the demuxer to the muxer, 
                    '1, cfr Frames will be duplicated And dropped to achieve exactly the requested constant frame rate.
                    '2, vfr Frames are passed through with their timestamp Or dropped so as to prevent 2 frames from having the same timestamp.
                    '3? dropAs passthrough but destroys all timestamps, making the muxer generate fresh timestamps based on frame-rate.
                    '-1, auto Chooses between 1 And 2 depending on muxer capabilities. This Is the default method.
                    'p.ExtractTimestamps seems like death switch, nvidia paper uses vsync 0

                    Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                    sb.Append(If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg")).Append(If(p.ExtractTimestamps, " -vsync 0", " -vsync 1")).Append("-hwaccel cuda -i ").Append(If(includePaths, p.SourceFile.Escape, "path")).
                            Append(" -f yuv4mpegpipe -pix_fmt ").Append(pix_fmt).Append(" -strict -1").Append(s.GetFFLogLevel(FfLogLevel.fatal)).Append(" -hide_banner - | ").Append(If(includePaths, Package.NVEnc.Path.Escape, "NVEncC64"))
                    'End If
            End Select

            For i = 0 To Items.Count - 1
                Dim prm = Items(i)
                Dim arg As String = prm.GetArgs
                If arg?.Length > 0 Then
                    If Not IsCustom(prm.Switch) Then sb.Append(" ").Append(arg)
                End If
            Next i

            If (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso (p.Script.IsFilterActive("Crop", "Hardware Encoder") OrElse Not (decVal = 0 AndAlso p.Script.IsFilterActive("Crop"))) Then
                sb.Append(" --crop ").Append(p.CropLeft).Append(",").Append(p.CropTop).Append(",").Append(p.CropRight).Append(",").Append(p.CropBottom)
            End If

            If (Not decVal = 0 AndAlso p.Script.IsFilterActive("Resize")) OrElse p.Script.IsFilterActive("Resize", "Hardware Encoder") Then
                sb.Append(" --output-res ").Append(p.TargetWidth).Append("x").Append(p.TargetHeight)
            End If

            Dim pr = p.Ranges
            If pr.Count > 0 AndAlso Not decVal = 0 Then
                sb.Append(" --trim ").Append(String.Join(",", pr.ToArray.SelectF(Function(range) range.Start & ":" & range.End), 0, pr.Count))
            End If

            If String.Equals(sourcePath, "-") Then
                sb.Append(" --y4m")
            End If

            If includePaths Then
                sb.Append(" -i ").Append(sourcePath.Escape).Append(" -o ").Append(targetPath.Escape)
            End If

            Return sb.ToString.Trim
        End Function

        Function IsCustom(switch As String) As Boolean
            If switch.NullOrEmptyS Then
                Return False
            End If

            Dim cVal As String = Custom.Value
            If cVal?.Length > 0 Then
                If cVal.EndsWith(switch, StringComparison.Ordinal) OrElse cVal.Contains(switch & " ") Then Return True
            End If
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.NVEnc
        End Function
    End Class
End Class
