
Imports System.Text
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class x264Enc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "x264"
        Params.ApplyValues(True)
        Params.ApplyValues(False)
    End Sub

    <NonSerialized>
    Private ParamsValue As x264Params

    Property Params As x264Params
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New x264Params
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As x264Params)
            ParamsValue = value
        End Set
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Dim pmVal As Integer = Params.Muxer.Value
            If pmVal = 0 OrElse pmVal = 1 Then
                Return "h264"
            Else
                Return Params.Muxer.ValueText.ToLowerInvariant
            End If
        End Get
    End Property

    Overrides Function GetFixedBitrate() As Integer
        Return CInt(Params.Bitrate.Value)
    End Function

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Video encoding", GetArgs(1, p.Script), s.ProcessPriority)

        Dim modeVal As Integer = Params.Mode.Value
        If modeVal = x264RateMode.TwoPass Then
            Encode("Video encoding second pass", GetArgs(2, p.Script), s.ProcessPriority)
        ElseIf modeVal = x264RateMode.ThreePass Then
            Encode("Video encoding second pass", GetArgs(3, p.Script), s.ProcessPriority)
            Encode("Video encoding third pass", GetArgs(2, p.Script), s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Package = Package.x264
            proc.Header = passName
            proc.Priority = priority
            proc.SkipStrings = {"kb/s, eta", "%]"}

            If commandLine.Contains("|") Then
                proc.File = "cmd.exe"
                proc.Arguments = "/S /C """ + commandLine + """"
            Else
                proc.CommandLine = commandLine
            End If

            proc.Start()
        End Using
    End Sub

    Overrides Sub RunCompCheck()
        If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
            Exit Sub
        End If

        Dim newParams As New x264Params
        Dim newStore = ParamsStore.GetDeepClone
        newParams.Init(newStore)

        Dim enc As New x264Enc
        enc.Params = newParams
        enc.Params.Mode.Value = x264RateMode.Quality
        enc.Params.Quant.Value = enc.Params.CompCheck.Value

        Dim script As New VideoScript
        script.Engine = p.Script.Engine
        script.Filters = p.Script.GetFiltersCopy
        Dim code As String
        Dim every = ((100 \ p.CompCheckRange) * 14).ToString

        If script.Engine = ScriptEngine.AviSynth Then
            code = "SelectRangeEvery(" + every + ",14)"
        Else
            code = "fpsnum = clip.fps_num" + BR + "fpsden = clip.fps_den" + BR +
                "clip = core.std.SelectEvery(clip = clip, cycle = " + every + ", offsets = range(14))" + BR +
                "clip = core.std.AssumeFPS(clip = clip, fpsnum = fpsnum, fpsden = fpsden)"
        End If

        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = (p.TempDir + p.TargetFile.Base + "_CompCheck." + script.FileType).ToShortFilePath
        script.Synchronize()

        Log.WriteLine(BR + script.GetFullScript + BR)

        Dim commandLine = enc.Params.GetArgs(0, script, p.TempDir + p.TargetFile.Base + "_CompCheck." + OutputExt, True, True)

        Try
            Encode("Compressibility Check", commandLine, ProcessPriorityClass.Normal)
        Catch ex As AbortException
            Exit Sub
        Catch ex As Exception
            g.ShowException(ex)
            Exit Sub
        End Try

        Dim bits = (New FileInfo(p.TempDir + p.TargetFile.Base + "_CompCheck." + OutputExt).Length) * 8
        p.Compressibility = (bits / script.GetFrameCount) / (p.TargetWidth * p.TargetHeight)

        OnAfterCompCheck()
        g.MainForm.Assistant()

        Log.WriteLine("Quality: " & CInt(Calc.GetPercent).ToString() + " %")
        Log.WriteLine("Compressibility: " + p.Compressibility.ToString("f3"))
        Log.Save()
    End Sub

    Overloads Function GetArgs(pass As Integer, script As VideoScript, Optional includePaths As Boolean = True) As String
        Return Params.GetArgs(pass, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
    End Function

    Overrides Sub ShowConfigDialog()
        Dim newParams As New x264Params
        Dim store = ParamsStore.GetDeepClone 'Test this!
        newParams.Init(store)
        newParams.ApplyValues(True)

        Using form As New CommandLineForm(newParams)
            form.HTMLHelp = "<h2>x264 Help</h2>" +
                "<p>Right-clicking a option shows the local console help for the option, pressing Ctrl or Shift while right-clicking a option shows the online help for the option.</p>" +
                "<p>Setting the Bitrate option to 0 will use the bitrate defined in the project/template in the main dialog.</p>" +
               $"<h2>x264 Online Help</h2><p><a href=""{Package.x264.HelpURL}"">x264 Online Help</a></p>" +
               $"<h2>x264 Console Help</h2><pre>{HelpDocument.ConvertChars(Package.x264.CreateHelpfile())}</pre>"

            Dim saveProfileAction = Sub()
                                        Dim enc = Me.GetDeepClone 'Test this!
                                        Dim params2 As New x264Params
                                        Dim store2 = store.GetDeepClone
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            form.cms.Items.Add(New ActionMenuItem("Save Profile...", saveProfileAction, ImageHelp.GetImageC(Symbol.Save)))

            If form.ShowDialog() = DialogResult.OK Then
                AutoCompCheckValue = CInt(newParams.CompCheckAimedQuality.Value)
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Property QualityMode() As Boolean
        Get
            Dim modeVal As Integer = Params.Mode.Value
            Return modeVal = x264RateMode.Quantizer OrElse modeVal = x264RateMode.Quality
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property

    Sub SetMuxer()
        Muxer = New MkvMuxer()
    End Sub

    Overrides Function CreateEditControl() As Control
        Return New x264Control(Me) With {.Dock = DockStyle.Fill}
    End Function

    Shared Function Test() As String
        Dim tester As New ConsolAppTester

        tester.IgnoredSwitches = "help longhelp fullhelp progress"
        tester.UndocumentedSwitches = "y4m"
        tester.Package = Package.x264
        tester.CodeFile = Folder.Startup.Parent + "Encoding\x264Enc.vb"

        Return tester.Test
    End Function
End Class

Public Class x264Params
    Inherits CommandLineParams

    Sub New()
        Title = "x264 Options"
    End Sub

    Property Mode As New OptionParam With {
        .Name = "Mode",
        .Text = "Mode",
        .Switches = {"--bitrate", "--qp", "--crf", "--pass", "--stats", "-q", "-B", "-p"},
        .Options = {"Bitrate", "Quantizer", "Quality", "Two Pass", "Three Pass"},
        .Value = 2}

    Property Quant As New NumParam With {
        .Switches = {"--crf", "--qp"},
        .Name = "Quant",
        .Text = "Quality",
        .Init = 22,
        .VisibleFunc = Function() Mode.Value = 1 OrElse Mode.Value = 2,
        .Config = {0, 69, 0.5, 1}}

    Property Bitrate As New NumParam With {
        .HelpSwitch = "--bitrate",
        .Text = "Bitrate",
        .VisibleFunc = Function() Mode.Value <> 1 AndAlso Mode.Value <> 2,
        .Config = {0, 1000000, 100}}

    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Text = "Preset",
        .Options = {"Ultra Fast", "Super Fast", "Very Fast", "Faster", "Fast", "Medium", "Slow", "Slower", "Very Slow", "Placebo"},
        .Init = 5}

    Property Tune As New OptionParam With {
        .Switch = "--tune",
        .Text = "Tune",
        .Options = {"None", "Film", "Animation", "Grain", "Still Image", "PSNR", "SSIM", "Fast Decode", "Zero Latency"}}

    Property CompCheck As New NumParam With {
        .Name = "CompCheckQuant",
        .Text = "Comp. Check",
        .Help = "CRF value used as 100%",
        .Value = 18,
        .Config = {1, 50}}

    Property CompCheckAimedQuality As New NumParam With {
        .Name = "CompCheckAimedQuality",
        .Text = "Aimed Quality",
        .Value = 50,
        .Help = "Percent value to adjusts the target file size or image size after the compressibility check accordingly.",
        .Config = {1, 100}}

    Property Custom As New StringParam With {
        .Text = "Custom",
        .Quotes = QuotesMode.Never,
        .AlwaysOn = True,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomFirstPass As New StringParam With {
        .Text = "Custom" + BR + "First Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomSecondPass As New StringParam With {
        .Text = "Custom" + BR + "Second Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property Deblock As New BoolParam With {
        .Switch = "--deblock",
        .NoSwitch = "--no-deblock",
        .Switches = {"-f"},
        .Text = "Deblocking",
        .ImportAction = Sub(param, arg)
                            Dim a = arg.Split(":"c)
                            DeblockA.Value = a(0).ToInt
                            DeblockB.Value = a(1).ToInt
                        End Sub,
        .ArgsFunc = Function() As String
                        If Deblock.Value Then
                            If DeblockA.Value = DeblockA.DefaultValue AndAlso
                                DeblockB.Value = DeblockB.DefaultValue AndAlso
                                Deblock.DefaultValue Then

                                Return Nothing
                            Else
                                Return "--deblock " & DeblockA.Value & ":" & DeblockB.Value
                            End If
                        ElseIf Deblock.DefaultValue Then
                            Return "--no-deblock"
                        End If
                    End Function}

    Property DeblockA As New NumParam With {
        .Text = "      Strength",
        .Config = {-6, 6}}

    Property DeblockB As New NumParam With {
        .Text = "      Threshold",
        .Config = {-6, 6}}

    Property BFrames As New NumParam With {
        .Switch = "--bframes",
        .Switches = {"-b"},
        .Text = "B-Frames",
        .Config = {0, 16}}

    Property AqMode As New OptionParam With {
        .Switch = "--aq-mode",
        .Text = "AQ Mode",
        .IntegerValue = True,
        .Expand = True,
        .Options = {"Disabled", "Variance AQ", "Auto-variance AQ", "Auto-variance AQ with bias to dark scenes"}}

    Property BAdapt As New OptionParam With {
        .Switch = "--b-adapt",
        .Text = "B-Adapt",
        .IntegerValue = True,
        .Options = {"Disabled", "Fast", "Optimal"}}

    Property Cabac As New BoolParam With {
        .NoSwitch = "--no-cabac",
        .Text = "Cabac"}

    Property Weightp As New OptionParam With {
        .Switch = "--weightp",
        .Text = "Weight P Prediction",
        .Expand = True,
        .Options = {"Disabled", "Weighted refs", "Weighted refs + Duplicates"},
        .IntegerValue = True}

    Property Profile As New OptionParam With {
        .Switch = "--profile",
        .Text = "Profile",
        .Options = {"Automatic", "Baseline", "Main", "High", "High 10", "High 422", "High 444"}}

    Property CQM As New OptionParam With {
        .Switch = "--cqm",
        .Options = {"Flat", "JVT"},
        .Text = "Quant Matrice Preset"}

    Property Mbtree As New BoolParam With {
        .Switch = "--mbtree",
        .NoSwitch = "--no-mbtree",
        .Text = "MB Tree"}

    Property I4x4 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .Label = "Partitions",
        .ArgsFunc = AddressOf GetPartitionsArg,
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "i4x4"}

    Property P4x4 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "p4x4"}

    Property B8x8 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "b8x8"}

    Property I8x8 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "i8x8"}

    Property P8x8 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "p8x8"}

    Property _8x8dct As New BoolParam With {
        .Switch = "--8x8dct",
        .NoSwitch = "--no-8x8dct",
        .Text = "8x8dct",
        .LeftMargin = g.MainForm.FontHeight * 1.5}

    Property RcLookahead As New NumParam With {
        .Switch = "--rc-lookahead",
        .Text = "Lookahead"}

    Property Ref As New NumParam With {
        .Switch = "--ref",
        .Switches = {"-r"},
        .Text = "Ref Frames"}

    Property Scenecut As New NumParam With {
        .Switch = "--scenecut",
        .Text = "Scenecut"}

    Property Subme As New OptionParam With {
        .Switch = "--subme",
        .Switches = {"-m"},
        .Text = "Subpel Refinement",
        .IntegerValue = True,
        .Expand = True,
        .Options = {"Fullpel only (not recommended)", "SAD mode decision, one qpel iteration", "SATD mode decision", "Progressively more qpel", "Progressively more qpel", "Progressively more qpel", "RD mode decision for I/P-frames", "RD mode decision for all frames", "RD refinement for I/P-frames", "RD refinement for all frames", "QP-RD - requires trellis=2, aq-mode>0", "Full RD disable all early terminations"}}

    Property Me_ As New OptionParam With {
        .Switch = "--me",
        .Text = "Motion Search Method",
        .Expand = True,
        .Values = {"dia", "hex", "umh", "esa", "tesa"},
        .Options = {"Diamond Search, Radius 1 (fast)", "Hexagonal Search, Radius 2", "Uneven Multi-Hexagon Search", "Exhaustive Search", "Hadamard Exhaustive Search (slow)"}}

    Property Weightb As New BoolParam With {
        .Switch = "--weightb",
        .NoSwitch = "--no-weightb",
        .Text = "Weighted prediction for B-frames"}

    Property Trellis As New OptionParam With {
        .Switch = "--trellis",
        .Switches = {"-t"},
        .Text = "Trellis",
        .Expand = True,
        .IntegerValue = True,
        .Options = {"Disabled", "Enabled only on the final encode of a MB", "Enabled on all mode decisions"}}

    Property Direct As New OptionParam With {
        .Switch = "--direct",
        .Text = "Direct MV Prediction",
        .Options = {"None", "Spatial", "Temporal", "Auto"}}

    Property Merange As New NumParam With {
        .Switch = "--merange",
        .Text = "ME Range"}

    Property Fastpskip As New BoolParam With {
        .NoSwitch = "--no-fast-pskip",
        .Text = "Fast Pskip"}

    Property Psy As New BoolParam With {
        .Switch = "--psy-rd",
        .NoSwitch = "--no-psy",
        .Text = "Psy RD",
        .ArgsFunc = Function() As String
                        If Psy.Value Then
                            Dim psRDVal As Double = PsyRD.Value
                            Dim psTrVal As Double = PsyTrellis.Value
                            If psRDVal <> PsyRD.DefaultValue OrElse psTrVal <> PsyTrellis.DefaultValue OrElse Not Psy.DefaultValue Then
                                Return "--psy-rd " & psRDVal.ToInvariantString & ":" & psTrVal.ToInvariantString
                            End If
                        Else
                            If Psy.DefaultValue Then
                                Return "--no-psy"
                            End If
                        End If
                    End Function}

    Property PsyRD As New NumParam With {
        .Config = {0, 0, 0.05, 2},
        .Text = "     RD"}

    Property PsyTrellis As New NumParam With {
        .Config = {0, 0, 0.05, 2},
        .Text = "     Trellis"}

    Property AqStrength As New NumParam With {
        .Switch = "--aq-strength",
        .Text = "AQ Strength",
        .Config = {0, 0, 0.1, 1}}

    Property DctDecimate As New BoolParam With {
        .Switch = "--dct-decimate",
        .NoSwitch = "--no-dct-decimate",
        .Text = "DCT Decimate"}

    Property DeadzoneInter As New NumParam With {
        .Switch = "--deadzone-inter",
        .Text = "Deadzone Inter",
        .Config = {0, 32}}

    Property DeadzoneIntra As New NumParam With {
        .Switch = "--deadzone-intra",
        .Text = "Deadzone Intra",
        .Config = {0, 32}}

    Property MixedRefs As New BoolParam With {
        .Switch = "--mixed-refs",
        .NoSwitch = "--no-mixed-refs",
        .Text = "Mixed References"}

    Property ForceCFR As New BoolParam With {
        .Switch = "--force-cfr",
        .Text = "Force constant framerate timestamp generation"}

    Property Ipratio As New NumParam With {
        .Switch = "--ipratio",
        .Text = "IP Ratio",
        .Config = {0, 0, 0.1, 1}}

    Property Pbratio As New NumParam With {
        .Switch = "--pbratio",
        .Text = "PB Ratio",
        .Config = {0, 0, 0.1, 1}}

    Property Qcomp As New NumParam With {
        .Text = "QComp",
        .Switch = "--qcomp",
        .Config = {0, 0, 0.1, 1}}

    Property Muxer As New OptionParam With {
        .Switch = "--muxer",
        .Text = "Output Format",
        .Options = {"Automatic", "RAW", "MKV", "FLV", "MP4"}}

    Property Demuxer As New OptionParam With {
        .Switch = "--demuxer",
        .Text = "Demuxer",
        .Options = {"Automatic", "RAW", "Y4M", "AVS", "LAVF", "FFMS"}}

    Property SlowFirstpass As New BoolParam With {
        .Switch = "--slow-firstpass",
        .Text = "Slow Firstpass"}

    Property PipingToolAVS As New OptionParam With {
        .Text = "Pipe",
        .Name = "PipingToolAVS",
        .VisibleFunc = Function() p.Script.Engine = ScriptEngine.AviSynth,
        .Options = {"Automatic", "None", "avs2pipemod y4m", "avs2pipemod raw", "ffmpeg y4m", "ffmpeg raw", "ffmpeg CUDA y4m"}}

    Property PipingToolVS As New OptionParam With {
        .Text = "Pipe",
        .Name = "PipingToolVS",
        .VisibleFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .Options = {"Automatic", "None", "vspipe y4m", "vspipe raw", "ffmpeg y4m", "ffmpeg raw", "ffmpeg CUDA y4m"}}

    Sub ApplyValues(isDefault As Boolean)
        'Dim setVal = Sub(param As CommandLineParam, value As Object) 'ToDO Throw away this Boxing !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        '                 If TypeOf param Is BoolParam Then
        '                     If isDefault Then
        '                         DirectCast(param, BoolParam).DefaultValue = CBool(value)
        '                     Else
        '                         DirectCast(param, BoolParam).Value = CBool(value)
        '                     End If
        '                 ElseIf TypeOf param Is NumParam Then
        '                     If isDefault Then
        '                         DirectCast(param, NumParam).DefaultValue = CDbl(value)
        '                     Else
        '                         DirectCast(param, NumParam).Value = CDbl(value)
        '                     End If
        '                 ElseIf TypeOf param Is OptionParam Then
        '                     If isDefault Then
        '                         DirectCast(param, OptionParam).DefaultValue = CInt(value)
        '                     Else
        '                         DirectCast(param, OptionParam).Value = CInt(value)
        '                     End If
        '                 End If
        '             End Sub
        If isDefault Then
            Deblock.DefaultValue = True
            DeblockA.DefaultValue = 0
            DeblockB.DefaultValue = 0
            BFrames.DefaultValue = 3
            AqMode.DefaultValue = 1
            BAdapt.DefaultValue = 1
            Cabac.DefaultValue = True
            Weightp.DefaultValue = 2
            Mbtree.DefaultValue = True
            Me_.DefaultValue = 1
            MixedRefs.DefaultValue = True
            I4x4.DefaultValue = True
            P4x4.DefaultValue = False
            B8x8.DefaultValue = True
            I8x8.DefaultValue = True
            P8x8.DefaultValue = True
            _8x8dct.DefaultValue = True
            RcLookahead.DefaultValue = 40
            Ref.DefaultValue = 3
            Scenecut.DefaultValue = 40
            Subme.DefaultValue = 7
            Trellis.DefaultValue = 1
            Weightb.DefaultValue = True
            Direct.DefaultValue = 1
            Merange.DefaultValue = 16
            Fastpskip.DefaultValue = True
            Psy.DefaultValue = True
            PsyRD.DefaultValue = 1
            PsyTrellis.DefaultValue = 0
            AqStrength.DefaultValue = 1
            DctDecimate.DefaultValue = True
            DeadzoneInter.DefaultValue = 21
            DeadzoneIntra.DefaultValue = 11
            Ipratio.DefaultValue = 1.4
            Pbratio.DefaultValue = 1.3
            Qcomp.DefaultValue = 0.6
            ForceCFR.DefaultValue = False
            SlowFirstpass.DefaultValue = False
        Else
            Deblock.Value = True
            DeblockA.Value = 0
            DeblockB.Value = 0
            BFrames.Value = 3
            AqMode.Value = 1
            BAdapt.Value = 1
            Cabac.Value = True
            Weightp.Value = 2
            Mbtree.Value = True
            Me_.Value = 1
            MixedRefs.Value = True
            I4x4.Value = True
            P4x4.Value = False
            B8x8.Value = True
            I8x8.Value = True
            P8x8.Value = True
            _8x8dct.Value = True
            RcLookahead.Value = 40
            Ref.Value = 3
            Scenecut.Value = 40
            Subme.Value = 7
            Trellis.Value = 1
            Weightb.Value = True
            Direct.Value = 1
            Merange.Value = 16
            Fastpskip.Value = True
            Psy.Value = True
            PsyRD.Value = 1
            PsyTrellis.Value = 0
            AqStrength.Value = 1
            DctDecimate.Value = True
            DeadzoneInter.Value = 21
            DeadzoneIntra.Value = 11
            Ipratio.Value = 1.4
            Pbratio.Value = 1.3
            Qcomp.Value = 0.6
            ForceCFR.Value = False
            SlowFirstpass.Value = False
        End If

        Select Case Preset.Value
            Case 0 'ultrafast
                If isDefault Then
                    Deblock.DefaultValue = False
                    _8x8dct.DefaultValue = False
                    BFrames.DefaultValue = 0
                    AqMode.DefaultValue = 0
                    BAdapt.DefaultValue = 0
                    Cabac.DefaultValue = False
                    Weightp.DefaultValue = 0
                    Mbtree.DefaultValue = False
                    Me_.DefaultValue = 0
                    MixedRefs.DefaultValue = False
                    RcLookahead.DefaultValue = 0
                    Ref.DefaultValue = 1
                    I4x4.DefaultValue = False
                    P4x4.DefaultValue = False
                    B8x8.DefaultValue = False
                    I8x8.DefaultValue = False
                    P8x8.DefaultValue = False
                    Scenecut.DefaultValue = 0
                    Subme.DefaultValue = 0
                    Trellis.DefaultValue = 0
                    Weightb.DefaultValue = False
                Else
                    Deblock.Value = False
                    _8x8dct.Value = False
                    BFrames.Value = 0
                    AqMode.Value = 0
                    BAdapt.Value = 0
                    Cabac.Value = False
                    Weightp.Value = 0
                    Mbtree.Value = False
                    Me_.Value = 0
                    MixedRefs.Value = False
                    RcLookahead.Value = 0
                    Ref.Value = 1
                    I4x4.Value = False
                    P4x4.Value = False
                    B8x8.Value = False
                    I8x8.Value = False
                    P8x8.Value = False
                    Scenecut.Value = 0
                    Subme.Value = 0
                    Trellis.Value = 0
                    Weightb.Value = False
                End If

            Case 1 'superfast
                If isDefault Then
                    Weightp.DefaultValue = 1
                    Mbtree.DefaultValue = False
                    Me_.DefaultValue = 0
                    MixedRefs.DefaultValue = False
                    P4x4.DefaultValue = False
                    B8x8.DefaultValue = False
                    P8x8.DefaultValue = False
                    RcLookahead.DefaultValue = 0
                    Ref.DefaultValue = 1
                    Subme.DefaultValue = 1
                    Trellis.DefaultValue = 0
                Else
                    Weightp.Value = 1
                    Mbtree.Value = False
                    Me_.Value = 0
                    MixedRefs.Value = False
                    P4x4.Value = False
                    B8x8.Value = False
                    P8x8.Value = False
                    RcLookahead.Value = 0
                    Ref.Value = 1
                    Subme.Value = 1
                    Trellis.Value = 0
                End If
            Case 2 'veryfast
                If isDefault Then
                    Weightp.DefaultValue = 1
                    MixedRefs.DefaultValue = False
                    RcLookahead.DefaultValue = 10
                    Ref.DefaultValue = 1
                    Subme.DefaultValue = 2
                    Trellis.DefaultValue = 0
                Else
                    Weightp.Value = 1
                    MixedRefs.Value = False
                    RcLookahead.Value = 10
                    Ref.Value = 1
                    Subme.Value = 2
                    Trellis.Value = 0
                End If
            Case 3 'faster
                If isDefault Then
                    Weightp.DefaultValue = 1
                    MixedRefs.DefaultValue = False
                    RcLookahead.DefaultValue = 20
                    Ref.DefaultValue = 2
                    Subme.DefaultValue = 4
                Else
                    Weightp.Value = 1
                    MixedRefs.Value = False
                    RcLookahead.Value = 20
                    Ref.Value = 2
                    Subme.Value = 4
                End If
            Case 4 'fast
                If isDefault Then
                    Weightp.DefaultValue = 1
                    RcLookahead.DefaultValue = 30
                    Ref.DefaultValue = 2
                    Subme.DefaultValue = 6
                Else
                    Weightp.Value = 1
                    RcLookahead.Value = 30
                    Ref.Value = 2
                    Subme.Value = 6
                End If
            Case 5 'medium
            Case 6 'slow
                If isDefault Then
                    RcLookahead.DefaultValue = 50
                    Ref.DefaultValue = 5
                    Subme.DefaultValue = 8
                    Trellis.DefaultValue = 2
                    Direct.DefaultValue = 3
                Else
                    RcLookahead.Value = 50
                    Ref.Value = 5
                    Subme.Value = 8
                    Trellis.Value = 2
                    Direct.Value = 3
                End If
            Case 7 'slower
                If isDefault Then
                    BAdapt.DefaultValue = 2
                    Me_.DefaultValue = 2
                    RcLookahead.DefaultValue = 60
                    Ref.DefaultValue = 8
                    I4x4.DefaultValue = True
                    P4x4.DefaultValue = True
                    B8x8.DefaultValue = True
                    I8x8.DefaultValue = True
                    P8x8.DefaultValue = True
                    Subme.DefaultValue = 9
                    Trellis.DefaultValue = 2
                    Direct.DefaultValue = 3
                Else
                    BAdapt.Value = 2
                    Me_.Value = 2
                    RcLookahead.Value = 60
                    Ref.Value = 8
                    I4x4.Value = True
                    P4x4.Value = True
                    B8x8.Value = True
                    I8x8.Value = True
                    P8x8.Value = True
                    Subme.Value = 9
                    Trellis.Value = 2
                    Direct.Value = 3
                End If
            Case 8 'veryslow
                If isDefault Then
                    BFrames.DefaultValue = 8
                    BAdapt.DefaultValue = 2
                    Me_.DefaultValue = 2
                    RcLookahead.DefaultValue = 60
                    Ref.DefaultValue = 16
                    I4x4.DefaultValue = True
                    P4x4.DefaultValue = True
                    B8x8.DefaultValue = True
                    I8x8.DefaultValue = True
                    P8x8.DefaultValue = True
                    Subme.DefaultValue = 10
                    Trellis.DefaultValue = 2
                    Direct.DefaultValue = 3
                    Merange.DefaultValue = 24
                Else
                    BFrames.Value = 8
                    BAdapt.Value = 2
                    Me_.Value = 2
                    RcLookahead.Value = 60
                    Ref.Value = 16
                    I4x4.Value = True
                    P4x4.Value = True
                    B8x8.Value = True
                    I8x8.Value = True
                    P8x8.Value = True
                    Subme.Value = 10
                    Trellis.Value = 2
                    Direct.Value = 3
                    Merange.Value = 24
                End If
            Case 9 'placebo
                If isDefault Then
                    BFrames.DefaultValue = 16
                    BAdapt.DefaultValue = 2
                    Me_.DefaultValue = 4
                    RcLookahead.DefaultValue = 60
                    Ref.DefaultValue = 16
                    I4x4.DefaultValue = True
                    P4x4.DefaultValue = True
                    B8x8.DefaultValue = True
                    I8x8.DefaultValue = True
                    P8x8.DefaultValue = True
                    Subme.DefaultValue = 11
                    Trellis.DefaultValue = 2
                    Direct.DefaultValue = 3
                    Merange.DefaultValue = 24
                    Fastpskip.DefaultValue = False
                    SlowFirstpass.DefaultValue = True
                Else
                    BFrames.Value = 16
                    BAdapt.Value = 2
                    Me_.Value = 4
                    RcLookahead.Value = 60
                    Ref.Value = 16
                    I4x4.Value = True
                    P4x4.Value = True
                    B8x8.Value = True
                    I8x8.Value = True
                    P8x8.Value = True
                    Subme.Value = 11
                    Trellis.Value = 2
                    Direct.Value = 3
                    Merange.Value = 24
                    Fastpskip.Value = False
                    SlowFirstpass.Value = True
                End If
        End Select

        Select Case Tune.Value
            Case 1 'film
                If isDefault Then
                    DeblockA.DefaultValue = -1
                    DeblockB.DefaultValue = -1
                    PsyTrellis.DefaultValue = 0.15
                Else
                    DeblockA.Value = -1
                    DeblockB.Value = -1
                    PsyTrellis.Value = 0.15
                End If

            Case 2 'animation
                If isDefault Then
                    DeblockA.DefaultValue = 1
                    DeblockB.DefaultValue = 1
                    Dim val = BFrames.DefaultValue + 2
                    BFrames.DefaultValue = If(val > 16, 16, val)
                    PsyRD.DefaultValue = 0.4
                    AqStrength.DefaultValue = 0.6
                Else
                    DeblockA.Value = 1
                    DeblockB.Value = 1
                    Dim val = BFrames.Value + 2
                    BFrames.Value = If(val > 16, 16, val)
                    PsyRD.Value = 0.4
                    AqStrength.Value = 0.6
                End If
            Case 3 'grain
                If isDefault Then
                    DeblockA.DefaultValue = -2
                    DeblockB.DefaultValue = -2
                    PsyTrellis.DefaultValue = 0.25
                    AqStrength.DefaultValue = 0.5
                    DctDecimate.DefaultValue = False
                    DeadzoneInter.DefaultValue = 6
                    DeadzoneIntra.DefaultValue = 6
                    Ipratio.DefaultValue = 1.1
                    Pbratio.DefaultValue = 1.1
                    Qcomp.DefaultValue = 0.8
                Else
                    DeblockA.Value = -2
                    DeblockB.Value = -2
                    PsyTrellis.Value = 0.25
                    AqStrength.Value = 0.5
                    DctDecimate.Value = False
                    DeadzoneInter.Value = 6
                    DeadzoneIntra.Value = 6
                    Ipratio.Value = 1.1
                    Pbratio.Value = 1.1
                    Qcomp.Value = 0.8
                End If
            Case 4 'stillimage
                If isDefault Then
                    DeblockA.DefaultValue = -3
                    DeblockB.DefaultValue = -3
                    PsyRD.DefaultValue = 2
                    PsyTrellis.DefaultValue = 0.7
                    AqStrength.DefaultValue = 1.2
                Else
                    DeblockA.Value = -3
                    DeblockB.Value = -3
                    PsyRD.Value = 2
                    PsyTrellis.Value = 0.7
                    AqStrength.Value = 1.2
                End If
            Case 5 'psnr
                If isDefault Then
                    AqMode.DefaultValue = 0
                    Psy.DefaultValue = False
                Else
                    AqMode.Value = 0
                    Psy.Value = False
                End If
            Case 6 'ssim
                If isDefault Then
                    AqMode.DefaultValue = 2
                    Psy.DefaultValue = False
                Else
                    AqMode.Value = 2
                    Psy.Value = False
                End If
            Case 7 'fastdecode
                If isDefault Then
                    Deblock.DefaultValue = False
                    Cabac.DefaultValue = False
                    Weightp.DefaultValue = 0
                    Weightb.DefaultValue = False
                Else
                    Deblock.Value = False
                    Cabac.Value = False
                    Weightp.Value = 0
                    Weightb.Value = False
                End If
            Case 8 'zerolatency
                If isDefault Then
                    BFrames.DefaultValue = 0
                    Mbtree.DefaultValue = False
                    RcLookahead.DefaultValue = 0
                    ForceCFR.DefaultValue = True
                Else
                    BFrames.Value = 0
                    Mbtree.Value = False
                    RcLookahead.Value = 0
                    ForceCFR.Value = True
                End If
        End Select

        Select Case Profile.Value
            Case 1 'baseline
                If isDefault Then
                    Cabac.DefaultValue = False
                    _8x8dct.DefaultValue = False
                    BFrames.DefaultValue = 0
                    Weightp.DefaultValue = 0
                Else
                    Cabac.Value = False
                    _8x8dct.Value = False
                    BFrames.Value = 0
                    Weightp.Value = 0
                End If
            Case 2 'main
                If isDefault Then _8x8dct.DefaultValue = False Else _8x8dct.Value = False
        End Select
    End Sub

    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)(160)

                Add("Basic",
                    Mode,
                    Preset,
                    Tune,
                    Profile,
                    New OptionParam With {.Switch = "--level", .Text = "Level", .Options = {"Automatic", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                    New OptionParam With {.Switch = "--output-depth", .Text = "Depth", .Options = {"8-Bit", "10-Bit"}, .Values = {"8", "10"}},
                    Quant,
                    Bitrate)
                Add("Analysis",
                    Trellis,
                    CQM,
                    I4x4,
                    P4x4,
                    B8x8,
                    I8x8,
                    P8x8,
                    _8x8dct,
                    Psy,
                    PsyRD,
                    PsyTrellis,
                    Fastpskip,
                    MixedRefs)
                Add("Analysis 2",
                    DeadzoneInter,
                    DeadzoneIntra,
                    New NumParam With {.Switch = "--mvrange", .Text = "MV Range", .Init = -1},
                    New NumParam With {.Switch = "--mvrange-thread", .Text = "MV Range Thread", .Init = -1},
                    New NumParam With {.Switch = "--nr", .Text = "Noise Reduction"})
                Add("Rate Control",
                    New StringParam With {.Switch = "--zones", .Text = "Zones"},
                    AqMode,
                    AqStrength,
                    Ipratio,
                    Pbratio,
                    Qcomp,
                    New NumParam With {.Switch = "--vbv-maxrate", .Text = "VBV Maxrate"},
                    New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize"},
                    New NumParam With {.Switch = "--vbv-init", .Text = "VBV Init", .Config = {0.5, 1.0, 0.1, 1}, .Init = 0.9},
                    New NumParam With {.Switch = "--crf-max", .Text = "Maximum CRF"},
                    New NumParam With {.Switch = "--qpmin", .Text = "Minimum QP"},
                    New NumParam With {.Switch = "--qpmax", .Text = "Maximum QP", .Init = 69})
                Add("Rate Control 2",
                    New NumParam With {.Switch = "--qpstep", .Text = "QP Step", .Init = 4},
                    New NumParam With {.Switch = "--ratetol", .Text = "Rate Tolerance", .Config = {0, 0, 0.1, 1}, .Init = 1},
                    New NumParam With {.Switch = "--chroma-qp-offset", .Text = "Chroma QP Offset"},
                    New NumParam With {.Switch = "--cplxblur", .Text = "T. Blur Complexity.", .Config = {0, 0, 0.1, 1}, .Init = 20},
                    New NumParam With {.Switch = "--qblur", .Text = "Temp. Blur Quants", .Config = {0, 0, 0.1, 1}, .Init = 0.5},
                    Mbtree,
                    New BoolParam With {.Switch = "--cqm4", .Text = "Set all 4x4 quant matrices"},
                    New BoolParam With {.Switch = "--cqm8", .Text = "Set all 8x8 quant matrices"})
                Add("Rate Control 3",
                    New StringParam With {.Switch = "--qpfile", .Text = "QP File", .BrowseFile = True},
                    New StringParam With {.Switch = "--cqmfile", .Text = "CQM File", .BrowseFile = True},
                    New StringParam With {.Switch = "--cqm4i", .Text = "cqm4i"},
                    New StringParam With {.Switch = "--cqm4p", .Text = "cqm4p"},
                    New StringParam With {.Switch = "--cqm8i", .Text = "cqm8i"},
                    New StringParam With {.Switch = "--cqm8p", .Text = "cqm8p"},
                    New StringParam With {.Switch = "--cqm4iy", .Text = "cqm4iy"},
                    New StringParam With {.Switch = "--cqm4ic", .Text = "cqm4ic"},
                    New StringParam With {.Switch = "--cqm4py", .Text = "cqm4py"},
                    New StringParam With {.Switch = "--cqm4pc", .Text = "cqm4pc"})
                Add("Motion Search",
                    Subme,
                    Me_,
                    Weightp,
                    Direct,
                    Merange,
                    Weightb,
                    New BoolParam With {.NoSwitch = "--no-chroma-me", .Init = True, .Text = "Use chroma in motion estimation"})
                Add("Slice Decision",
                    BAdapt,
                    New OptionParam With {.Switch = "--b-pyramid", .Text = "B-Pyramid", .Init = 2, .Options = {"None", "Strict", "Normal"}},
                    BFrames,
                    New NumParam With {.Switch = "--b-bias", .Text = "B-Bias"},
                    RcLookahead,
                    Ref,
                    Scenecut,
                    New NumParam With {.Switch = "--keyint", .Switches = {"-I"}, .Text = "Max GOP Size", .Init = 250},
                    New NumParam With {.Switch = "--min-keyint", .Switches = {"-i"}, .Text = "Min GOP Size"},
                    New NumParam With {.Switch = "--slices", .Text = "Slices"},
                    New NumParam With {.Switch = "--slices-max", .Text = "Slices Max"},
                    New NumParam With {.Switch = "--slice-max-size", .Text = "Slice Max Size"},
                    New NumParam With {.Switch = "--slice-max-mbs", .Text = "Slice Max MBS"},
                    New NumParam With {.Switch = "--slice-min-mbs", .Text = "Slice Min MBS"})
                Add("Slice Decision 2",
                    DctDecimate,
                    New BoolParam With {.Switch = "--intra-refresh", .Text = "Periodic Intra Refresh instead of IDR frames"},
                    New BoolParam With {.Switch = "--open-gop", .Text = "Open GOP"})
                Add("VUI",
                    New StringParam With {.Switch = "--sar", .Text = "Sample AR", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                    New StringParam With {.Switch = "--crop-rect", .Text = "Crop Rectangle"},
                    New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "Component", "PAL", "NTSC", "SECAM", "MAC"}},
                    New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 428", "SMPTE 431", "SMPTE 432"}},
                    New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 2085", "SMPTE 240 M", "YCgCo", "Chroma Derived C", "ICtCp"}},
                    New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 2084", "SMPTE 240 M", "SMPTE 428"}},
                    New OptionParam With {.Switch = "--alternative-transfer", .Text = "Alternative Transfer", .Options = {"Undefined", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 2084", "SMPTE 240 M", "SMPTE 428"}},
                    New OptionParam With {.Switch = "--overscan", .Text = "Overscan", .Options = {"Undefined", "Show", "Crop"}},
                    New OptionParam With {.Switch = "--range", .Text = "Range", .Options = {"Auto", "TV", "PC"}},
                    New OptionParam With {.Switch = "--nal-hrd", .Text = "Signal HRD Info", .Options = {"None", "VBR", "CBR"}},
                    New NumParam With {.Switch = "--chromaloc", .Text = "Chromaloc", .Config = {0, 5}},
                    New BoolParam With {.Switch = "--filler", .Text = "Force hard-CBR and generate filler"},
                    New BoolParam With {.Switch = "--pic-struct", .Text = "Force pic_struct in Picture Timing SEI"})
                Add("Input/Output",
                    New StringParam With {.Switch = "--opencl-clbin", .Text = "OpenCl clbin", .BrowseFile = True},
                    New StringParam With {.Switch = "--dump-yuv", .Text = "Dump YUV", .BrowseFile = True},
                    New StringParam With {.Switch = "--tcfile-in", .Text = "TC File In", .BrowseFile = True},
                    New StringParam With {.Switch = "--tcfile-out", .Text = "TC File Out", .BrowseFile = True},
                    New StringParam With {.Switch = "--index", .Text = "Index File", .BrowseFile = True},
                    New StringParam With {.Switch = "--timebase", .Text = "Timebase"},
                    New StringParam With {.Switch = "--input-fmt", .Text = "Input File Format"},
                    PipingToolAVS,
                    PipingToolVS,
                    Demuxer,
                    New OptionParam With {.Switch = "--input-depth", .Text = "Input Depth", .Options = {"Automatic", "8", "10", "12", "14", "16"}},
                    New OptionParam With {.Switch = "--input-csp", .Text = "Input Csp", .Options = {"Automatic", "I420", "YV12", "NV12", "NV21", "I422", "YV16", "NV16", "YUYV", "UYVY", "I444", "YV24", "BGR", "BGRA", "RGB"}},
                    New OptionParam With {.Switch = "--input-range", .Text = "Input Range", .Options = {"Automatic", "TV", "PC"}},
                    New OptionParam With {.Switch = "--output-csp", .Text = "Output Csp", .Options = {"Automatic", "I420", "I422", "I444", "RGB"}},
                    Muxer,
                    New OptionParam With {.Switch = "--fps", .Text = "Frame Rate", .Options = {"Automatic", "24000/1001", "24", "25", "30000/1001", "30", "50", "60000/1001", "60"}})
                Add("Input/Output 2",
                    New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"None", "Error", "Warning", "Info", "Debug"}},
                    New OptionParam With {.Switch = "--pulldown", .Text = "Pulldown", .Options = {"None", "22", "32", "64", "Double", "Triple", "Euro"}},
                    New OptionParam With {.Switch = "--avcintra-class", .Text = "AVC Intra Class", .Options = {"None", "50", "100", "200"}},
                    New OptionParam With {.Switch = "--avcintra-flavor", .Text = "AVC Intra Flavor", .Options = {"Panasonic", "Sony"}},
                    New NumParam With {.Switch = "--threads", .Text = "Threads"},
                    New NumParam With {.Switch = "--lookahead-threads", .Text = "Lookahead Threads"},
                    New NumParam With {.Switch = "--seek", .Text = "Seek"},
                    New NumParam With {.Switch = "--sync-lookahead", .Text = "Sync Lookahead"},
                    New NumParam With {.Switch = "--asm", .Text = "ASM"},
                    New NumParam With {.Switch = "--opencl-device", .Text = "OpenCl Device"},
                    New NumParam With {.Switch = "--sps-id", .Text = "SPS/PPS ID"})
                Add("Input/Output 3",
                    New BoolParam With {.Switch = "--fake-interlaced", .Text = "Fake Interlaced"},
                    New BoolParam With {.Switch = "--stitchable", .Text = "Stitchable"},
                    New BoolParam With {.Switch = "--psnr", .Text = "PSNR"},
                    New BoolParam With {.Switch = "--ssim", .Text = "SSIM"},
                    New BoolParam With {.Switch = "--sliced-threads", .Text = "Low-latency but lower-efficiency threading"},
                    New BoolParam With {.Switch = "--thread-input", .Text = "Run Avisynth in its own thread"},
                    New BoolParam With {.Switch = "--non-deterministic", .Text = "Non Deterministic"},
                    New BoolParam With {.Switch = "--cpu-independent", .Text = "Ensure reproducibility across different CPUs"},
                    New BoolParam With {.Switch = "--no-asm", .Text = "Disable all CPU optimizations"},
                    New BoolParam With {.Switch = "--opencl", .Text = "Enable use of OpenCL"},
                    ForceCFR,
                    New BoolParam With {.Switch = "--bluray-compat", .Text = "Enable compatibility hacks for Blu-ray support"},
                    New BoolParam With {.Switch = "--aud", .Text = "Use access unit delimiters"},
                    New BoolParam With {.Switch = "--quiet", .Text = "Quiet Mode"},
                    New BoolParam With {.Switch = "--verbose", .Switches = {"-v"}, .Text = "Print stats for each frame"},
                    New BoolParam With {.Switch = "--dts-compress", .Text = "Eliminate initial delay with container DTS hack"})
                Add("Other",
                    New StringParam With {.Switch = "--video-filter", .Switches = {"--vf"}, .Text = "Video Filter"},
                    New OptionParam With {.Switches = {"--tff", "--bff"}, .Text = "Interlaced", .Options = {"Progressive ", "Top Field First", "Bottom Field First"}, .Values = {"", "--tff", "--bff"}},
                    New OptionParam With {.Switch = "--frame-packing", .Text = "Frame Packing", .IntegerValue = True, .Options = {"Checkerboard", "Column Alternation", "Row Alternation", "Side By Side", "Top Bottom", "Frame Alternation", "Mono", "Tile Format"}},
                    Deblock,
                    DeblockA,
                    DeblockB,
                    CompCheck,
                    CompCheckAimedQuality,
                    SlowFirstpass,
                    New BoolParam With {.Switch = "--constrained-intra", .Text = "Constrained Intra Prediction"},
                    Cabac)
                Add("Custom",
                    Custom,
                    CustomFirstPass,
                    CustomSecondPass)

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
        If Control.ModifierKeys = Keys.Control OrElse Control.ModifierKeys = Keys.Shift Then
            g.ShellExecute("http://www.chaneru.com/Roku/HLS/X264_Settings.htm#" & id.TrimStart("-"c))
        Else
            g.ShowCommandLineHelp(Package.x264, id)
        End If
    End Sub

    Private BlockValueChanged As Boolean

    Protected Overrides Sub OnValueChanged(item As CommandLineParam)
        If BlockValueChanged Then
            Exit Sub
        End If

        If item Is Preset OrElse item Is Tune OrElse item Is Profile Then
            BlockValueChanged = True
            ApplyValues(False)
            BlockValueChanged = False
        End If

        If DeblockA.NumEdit IsNot Nothing Then
            Dim dbVal As Boolean = Deblock.Value
            DeblockA.NumEdit.Enabled = dbVal
            DeblockB.NumEdit.Enabled = dbVal

            Dim pVal As Boolean = Psy.Value
            PsyRD.NumEdit.Enabled = pVal
            PsyTrellis.NumEdit.Enabled = pVal
        End If

        MyBase.OnValueChanged(item)
    End Sub

    Overloads Overrides Function GetCommandLine(includePaths As Boolean, includeExecutable As Boolean, Optional pass As Integer = 1) As String
        Return GetArgs(1, p.Script, p.VideoEncoder.OutputPath.DirAndBase & p.VideoEncoder.OutputExtFull, includePaths, includeExecutable)
    End Function

    Overloads Function GetArgs(pass As Integer, script As VideoScript, targetPath As String, includePaths As Boolean, includeExecutable As Boolean) As String

        ApplyValues(True)

        ' Dim args As String
        Dim sb As New StringBuilder(512)
        Dim pipeTool = If(p.Script.Engine = ScriptEngine.AviSynth, PipingToolAVS, PipingToolVS).ValueText

        If includePaths AndAlso includeExecutable Then
            ' Dim pipeCmd = ""

            If pipeTool.Equals("automatic") Then
                If p.Script.Engine = ScriptEngine.AviSynth Then
                    pipeTool = "none"
                Else
                    pipeTool = "vspipe y4m"
                End If
            End If


            Select Case pipeTool
                Case "vspipe y4m"
                    sb.Append(Package.vspipe.Path.Escape).Append(" ").Append(script.Path.Escape).Append(" - --y4m | ")
                Case "vspipe raw"
                    sb.Append(Package.vspipe.Path.Escape).Append(" ").Append(script.Path.Escape).Append(" - | ")
                Case "avs2pipemod y4m"
                    Dim dll As String

                    If FrameServerHelp.IsAviSynthPortableUsed Then
                        dll = " -dll=" + Package.AviSynth.Path.Escape
                    End If

                    sb.Append(Package.avs2pipemod.Path.Escape).Append(dll).Append(" -y4mp ").Append(script.Path.Escape).Append(" | ")
                Case "avs2pipemod raw"
                    Dim dll As String

                    If FrameServerHelp.IsAviSynthPortableUsed Then
                        dll = " -dll=" + Package.AviSynth.Path.Escape
                    End If

                    sb.Append(Package.avs2pipemod.Path.Escape).Append(dll).Append(" -rawvideo ").Append(script.Path.Escape).Append(" | ")
                Case "ffmpeg y4m"
                    sb.Append(Package.ffmpeg.Path.Escape).Append(" -i ").Append(script.Path.Escape).Append(" -f yuv4mpegpipe -strict -1").Append(s.GetFFLogLevel(FfLogLevel.fatal)).Append(" -hide_banner - | ")
                Case "ffmpeg raw"
                    sb.Append(Package.ffmpeg.Path.Escape).Append(" -i ").Append(script.Path.Escape).Append(" -f rawvideo -strict -1").Append(s.GetFFLogLevel(FfLogLevel.fatal)).Append(" -hide_banner - | ")
                Case "ffmpegcuda y4m"

                    'To DO Check Vsync: 0 - passthrough Each frame Is passed with its timestamp from the demuxer to the muxer, 
                    '1, cfr Frames will be duplicated And dropped to achieve exactly the requested constant frame rate.
                    '2, vfr Frames are passed through with their timestamp Or dropped so as to prevent 2 frames from having the same timestamp.
                    '3? drop As passthrough but destroys all timestamps, making the muxer generate fresh timestamps based on frame-rate.
                    '-1, auto Chooses between 1 And 2 depending on muxer capabilities. This Is the default method.
                    ' -vsync 0 -hwaccel cuda      ( -threads 1 )
                    'seems like death switch, nvidia paper recommends vsync 0

                    Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                    sb.Append(Package.ffmpeg.Path.Escape).Append(If(p.ExtractTimestamps, " -vsync 0", " -vsync 1")).Append(" -hwaccel cuda -i ").Append(p.SourceFile.Escape).
                        Append(" -f yuv4mpegpipe -pix_fmt ").Append(pix_fmt).Append(" -strict -1").Append(s.GetFFLogLevel(FfLogLevel.fatal)).Append(" -hide_banner - | ")

            End Select

            sb.Append(Package.x264.Path.Escape)
        End If

        Dim modeVal As Integer = Mode.Value

        If modeVal = x264RateMode.TwoPass OrElse modeVal = x264RateMode.ThreePass Then
            sb.Append(" --pass ").Append(pass)

            If pass = 1 Then
                Dim cVal As String = CustomFirstPass.Value
                If cVal?.Length > 0 Then
                    sb.Append(" ").Append(cVal)
                End If
            Else
                Dim cVal As String = CustomSecondPass.Value
                If cVal?.Length > 0 Then
                    sb.Append(" ").Append(cVal)
                End If
            End If
        End If

        If modeVal = x264RateMode.Quantizer Then
            If Not IsCustom(pass, "--qp") Then
                sb.Append(" --qp ").Append(CInt(Quant.Value).ToInvariantString)
            End If
        ElseIf modeVal = x264RateMode.Quality Then
            If Not IsCustom(pass, "--crf") Then
                sb.Append(" --crf ").Append(Quant.Value.ToInvariantString)
            End If
        Else
            If Not IsCustom(pass, "--bitrate") Then
                Dim brVal As Double = Bitrate.Value
                If brVal <> 0 Then
                    sb.Append(" --bitrate ").Append(brVal)
                Else
                    sb.Append(" --bitrate ").Append(p.VideoBitrate)
                End If
            End If
        End If

        'Dim q = From i In Items Where i.GetArgs?.Length > 0 AndAlso Not IsCustom(pass, i.Switch)
        'If q.Any Then args &= " " & q.Select(Function(item) item.GetArgs).Join(" ")
        For i = 0 To Items.Count - 1
            Dim prm = Items(i)
            Dim arg As String = prm.GetArgs
            If arg?.Length > 0 AndAlso Not IsCustom(pass, prm.Switch) Then
                sb.Append(" ").Append(arg)
            End If
        Next i

        If includePaths Then
            Dim input = If(String.Equals(pipeTool, "none"), script.Path.ToShortFilePath.Escape, "-")
            Dim dmx = Demuxer.ValueText

            If String.Equals(dmx, "automatic") Then
                If String.Equals(pipeTool, "none") Then
                    dmx = ""
                ElseIf pipeTool.EndsWith(" y4m", StringComparison.Ordinal) Then
                    dmx = "y4m"
                ElseIf pipeTool.EndsWith(" raw", StringComparison.Ordinal) Then
                    dmx = "raw"
                End If
            End If

            If dmx?.Length > 0 Then
                Dim info = script.GetInfo

                sb.Append(" --demuxer ").Append(dmx).Append(" --frames ").Append(info.FrameCount.ToInvariantString)

                If dmx = "raw" Then
                    sb.Append(" --input-res ").Append(info.Width).Append("x").Append(info.Height)

                    If Not sb.ToString.Contains("--fps ") Then
                        sb.Append(" --fps ").Append(info.FrameRateNum).Append("/").Append(info.FrameRateDen)
                    End If
                End If
            End If

            If modeVal = x264RateMode.TwoPass OrElse modeVal = x264RateMode.ThreePass Then
                sb.Append(" --stats ").Append((p.TempDir & p.TargetFile.Base & ".stats").Escape)
            End If

            If (modeVal = x264RateMode.ThreePass AndAlso (pass = 1 OrElse pass = 3)) OrElse modeVal = x264RateMode.TwoPass AndAlso pass = 1 Then
                sb.Append(" --output NUL ").Append(input)
            Else
                sb.Append(" --output ").Append(targetPath.ToShortFilePath.Escape).Append(" ").Append(input)
            End If
        End If

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function

    Function GetPartitionsArg() As String
        Dim i4Val As Boolean = I4x4.Value
        Dim i8Val As Boolean = I8x8.Value
        Dim p4Val As Boolean = P4x4.Value
        Dim p8Val As Boolean = P8x8.Value
        Dim b8Val As Boolean = B8x8.Value

        If i4Val = I4x4.DefaultValue AndAlso i8Val = I8x8.DefaultValue AndAlso p4Val = P4x4.DefaultValue AndAlso p8Val = P8x8.DefaultValue AndAlso b8Val = B8x8.DefaultValue Then
            Return Nothing
        End If

        If i4Val AndAlso i8Val AndAlso p4Val AndAlso p8Val AndAlso b8Val Then
            Return "--partitions all"
        ElseIf Not i4Val AndAlso Not i8Val AndAlso Not p4Val AndAlso Not p8Val AndAlso Not b8Val Then
            Return "--partitions none"
        End If

        Dim sb As New StringBuilder(25)

        If i4Val Then sb.Append("i4x4,")
        If i8Val Then sb.Append("i8x8,")
        If p4Val Then sb.Append("p4x4,")
        If p8Val Then sb.Append("p8x8,")
        If b8Val Then sb.Append("b8x8,") '"," added at end 

        Dim sbL As Integer = sb.Length
        If sbL > 0 Then
            Return "--partitions " & sb.ToString(0, sbL - 1) 'partitions.TrimEnd(","c)
        End If
    End Function

    Function IsCustom(pass As Integer, switch As String) As Boolean
        If switch.NullOrEmptyS Then
            Return False
        End If

        Dim modeVal As Integer = Mode.Value
        If modeVal = x264RateMode.TwoPass OrElse modeVal = x264RateMode.ThreePass Then
            If pass = 1 Then
                Dim val2 As String = CustomFirstPass.Value
                If val2?.Length > 0 Then
                    If val2.EndsWith(switch, StringComparison.Ordinal) OrElse val2.Contains(switch & " ") Then
                        Return True
                    End If
                End If
            Else
                Dim val1 As String = CustomSecondPass.Value
                If val1?.Length > 0 Then
                    If val1.EndsWith(switch, StringComparison.Ordinal) OrElse val1.Contains(switch & " ") Then
                        Return True
                    End If
                End If
            End If
        End If

        Dim cval As String = Custom.Value
        If cval?.Length > 0 Then
            If cval.EndsWith(switch, StringComparison.Ordinal) OrElse cval.Contains(switch & " ") Then
                Return True
            End If
        End If
    End Function

    Public Overrides Function GetPackage() As Package
        Return Package.x264
    End Function
End Class

Public Enum x264RateMode
    Bitrate
    Quantizer
    Quality
    <DispName("Two Pass")> TwoPass
    <DispName("Three Pass")> ThreePass
End Enum
