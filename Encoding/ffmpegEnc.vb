
Imports System.Text

Imports StaxRip.CommandLine

<Serializable()>
Public Class ffmpegEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "ffmpeg | " & Params.Codec.OptionText
        End Get
    End Property

    Sub New()
        Muxer = New ffmpegMuxer("AVI")
    End Sub

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
        Dim newParams = New EncoderParams
        Dim store = ParamsStore.GetDeepClone
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            Dim a1 = Sub()
                         Dim enc = Me.GetDeepClone
                         Dim params2 As New EncoderParams
                         Dim store2 = store.GetDeepClone
                         params2.Init(store2)
                         enc.Params = params2
                         enc.ParamsStore = store2
                         SaveProfile(enc)
                     End Sub

            form.cms.Items.Add(New StaxRip.UI.ActionMenuItem("Save Profile...", a1))

            Dim a2 = Sub()
                         Dim codecText = newParams.Codec.OptionText
                         Dim consoleHelp = ProcessHelp.GetConsoleOutput(Package.ffmpeg.Path, "-hide_banner -h encoder=" & newParams.Codec.ValueText)
                         Dim helpDic As New Dictionary(Of String, String)(11, StringComparer.Ordinal) From {
                            {"x264", "https://trac.ffmpeg.org/wiki/Encode/H.264"},
                            {"x265", "https://trac.ffmpeg.org/wiki/Encode/H.265"},
                            {"XviD", "https://trac.ffmpeg.org/wiki/Encode/MPEG-4"},
                            {"VP9", "https://trac.ffmpeg.org/wiki/Encode/VP9"},
                            {"FFV1", "https://trac.ffmpeg.org/wiki/Encode/FFV1"},
                            {"Intel H.264", "https://trac.ffmpeg.org/wiki/Hardware/QuickSync"},
                            {"Intel H.265", "https://trac.ffmpeg.org/wiki/Hardware/QuickSync"},
                            {"AV1", "https://trac.ffmpeg.org/wiki/Encode/AV1"}}

                         form.HTMLHelp = $"<h2>ffmpeg Online Help</h2>" & "<p><a href=""{Package.ffmpeg.HelpURL}"">ffmpeg Online Help</a></p>"

                         Dim helpV As String
                         If helpDic.TryGetValue(codecText, helpV) Then
                             form.HTMLHelp += $"<h2>ffmpeg {codecText} Online Help</h2>" & $"<p><a href=""{helpV}"">ffmpeg {codecText} Online Help</a></p>"
                         End If

                         form.HTMLHelp += $"<h2>ffmpeg {codecText} Console Help</h2>" & $"<pre>{HelpDocument.ConvertChars(consoleHelp) & BR}</pre>"
                     End Sub

            AddHandler form.BeforeHelp, a2

            If form.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputExt() As String
        Get
            Select Case Params.Codec.Value
                Case 6, 7, 8
                    Return "mov"
                Case 11, 12
                    Return "webm"
                Case 0
                    Return "h264"
                Case 1
                    Return "h265"
                Case 3, 4, 9, 10
                    Return "avi"
                Case Else
                    Return "mkv"
            End Select
        End Get
    End Property

    Overrides ReadOnly Property IsCompCheckEnabled() As Boolean
        Get
            Return False
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Params.RaiseValueChanged(Nothing)

        If Params.Mode.Value = 2 Then
            Encode(Params.GetCommandLine(True, False, 1))
            Encode(Params.GetCommandLine(True, False, 2))
        Else
            Encode(Params.GetCommandLine(True, False))
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(args As String)
        Using proc As New Proc
            proc.Header = "Video encoding " & Params.Codec.OptionText
            proc.SkipStrings = {"frame=", "size="}
            proc.FrameCount = p.Script.GetFrameCount
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList
        ret.Add("Encoder Options", AddressOf ShowConfigDialog)
        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return ret
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = EncodingMode.Quality
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "ffmpeg Options"
        End Sub

        Property Codec As New OptionParam With {
            .Switch = "-c:v",
            .Text = "Codec",
            .AlwaysOn = True,
            .Options = {
            "x264",                  '0
            "x265",                  '1
            "AV1",                   '2
            "XviD",                  '3
            "MPEG-4",                '4
            "Theora",                '5
            "ProRes",                '6
            "R210",                  '7
            "V210",                  '8
            "UT Video",              '9
            "FFV1",                  '10
            "VP | VP8",              '11
            "VP | VP9",              '12
            "Intel | Intel H.264",   '13
            "Intel | Intel H.265",   '14
            "Nvidia | Nvidia H.264", '15
            "Nvidia | Nvidia H.265"},'16
            .Values = {
            "libx264",      '0
            "libx265",      '1
            "libaom-av1",   '2
            "libxvid",      '3
            "mpeg4",        '4
            "libtheora",    '5
            "prores",       '6
            "r210",         '7
            "v210",         '8
            "utvideo",      '9
            "ffv1",         '10
            "libvpx",       '11
            "libvpx-vp9",   '12
            "h264_qsv",     '13
            "hevc_qsv",     '14
            "h264_nvenc",   '15
            "hevc_nvenc"}}  '16

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode",
            .VisibleFunc = Function()
                               Select Case Codec.Value
                                   Case 6, 9, 10
                                       Return False
                                   Case Else
                                       Return True
                               End Select
                               'Return Not Codec.ValueText.EqualsAny("prores", "utvideo", "ffv1")
                           End Function,
            .Options = {"Quality", "One Pass", "Two Pass"}}

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "Software", "Intel", "DXVA2", "Nvidia"},
            .Values = {"-", "sw", "qsv", "dxva2", "cuda"}}

        Property h264_nvenc_rc As New OptionParam With {
            .Name = "h264/h265_nvenc rc",
            .Switch = "-rc",
            .Text = "Rate Control",
            .Options = {"Preset", "Constqp", "VBR", "CBR", "VBR_MinQP", "LL_2Pass_Quality", "LL_2Pass_Size", "VBR_2Pass"},
            .VisibleFunc = Function() Codec.Value > 15} 'Codec.ValueText.Equals("h264_nvenc")}

        Property Custom As New StringParam With {
            .Text = "Custom",
            .Quotes = QuotesMode.Never,
            .AlwaysOn = True}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)(32)

                    ItemsValue.AddRange({Decoder, Codec, Mode,
                        New OptionParam With {.Name = "x264/x265 preset", .Text = "Preset", .Switch = "-preset", .Init = 5, .Options = {"Ultrafast", "Superfast", "Veryfast", "Faster", "Fast", "Medium", "Slow", "Slower", "Veryslow", "Placebo"}, .VisibleFunc = Function()
                                                                                                                                                                                                                                                                       Dim cv = Codec.Value
                                                                                                                                                                                                                                                                       If cv = 0 OrElse cv = 1 Then Return True
                                                                                                                                                                                                                                                                       'Return Codec.OptionText.EqualsAny("x264", "x265")
                                                                                                                                                                                                                                                                   End Function},
                        New OptionParam With {.Name = "x264/x265 tune", .Text = "Tune", .Switch = "-tune", .Options = {"None", "Film", "Animation", "Grain", "Stillimage", "Psnr", "Ssim", "Fastdecode", "Zerolatency"}, .VisibleFunc = Function()
                                                                                                                                                                                                                                            Dim cv = Codec.Value
                                                                                                                                                                                                                                            If cv = 0 OrElse cv = 1 Then Return True
                                                                                                                                                                                                                                            ' Return Codec.OptionText.EqualsAny("x264", "x265")
                                                                                                                                                                                                                                        End Function},
                        New OptionParam With {.Switch = "-profile:v", .Text = "Profile", .VisibleFunc = Function() Codec.Value = 6, .Init = 3, .IntegerValue = True, .Options = {"Proxy", "LT", "Normal", "HQ"}},
                        New OptionParam With {.Switch = "-speed", .Text = "Speed", .AlwaysOn = True, .VisibleFunc = Function()
                                                                                                                        Dim cv = Codec.Value
                                                                                                                        If cv = 11 OrElse cv = 12 Then Return True
                                                                                                                        'Return Codec.OptionText.EqualsAny("VP8", "VP9")
                                                                                                                    End Function, .Options = {"6 - Fastest", "5 - Faster", "4 - Fast", "3 - Medium", "2 - Slow", "1 - Slower", "0 - Slowest"}, .Values = {"6", "5", "4", "3", "2", "1", "0"}, .Value = 5},
                        New OptionParam With {.Switch = "-cpu-used", .Text = "CPU Used", .Init = 1, .VisibleFunc = Function() Codec.Value = 2, .IntegerValue = True, .Options = {"0 - Slowest", "1 - Very Slow", "2 - Slower", "3 - Slow", "4 - Medium", "5 - Fast", "6 - Faster", "7 - Very Fast", "8 - Fastest"}},
                        New OptionParam With {.Switch = "-aq-mode", .Text = "AQ Mode", .VisibleFunc = Function() Codec.Value = 12, .Options = {"Disabled", "0", "1", "2", "3"}, .Values = {"Disabled", "0", "1", "2", "3"}},
                        New OptionParam With {.Name = "h264_nvenc profile", .Switch = "-profile", .Text = "Profile", .Options = {"Baseline", "Main", "High", "High444p"}, .Init = 1, .VisibleFunc = Function() Codec.Value > 15},
                        New OptionParam With {.Name = "h264_nvenc preset", .Switch = "-preset", .Text = "Preset", .Options = {"Default", "Slow", "Medium", "Fast", "HP", "HQ", "BD", "LL", "LLHQ", "LLHP", "Lossless", "Losslesshp"}, .Init = 2, .VisibleFunc = Function() Codec.Value > 15},
                        New OptionParam With {.Name = "h264_nvenc level", .Switch = "-level", .Text = "Level", .Options = {"Auto", "1", "1.0", "1b", "1.0b", "1.1", "1.2", "1.3", "2", "2.0", "2.1", "2.2", "3", "3.0", "3.1", "3.2", "4", "4.0", "4.1", "4.2", "5", "5.0", "5.1"}, .VisibleFunc = Function() Codec.Value = 15},
                        h264_nvenc_rc,
                        New OptionParam With {.Name = "utVideoPred", .Switch = "-pred", .Text = "Prediction", .Init = 3, .Options = {"None", "Left", "Gradient", "Median"}, .VisibleFunc = Function() Codec.Value = 9},
                        New OptionParam With {.Name = "utVideoPixFmt", .Switch = "-pix_fmt", .Text = "Pixel Format", .Options = {"YUV420P", "YUV422P", "YUV444P", "yuv420p10le", "yuv422p10le", "yuv444p10le", "yuv420p12le", "yuv422p12le", "yuv444p12le", "RGB24", "RGBA"}},
                        New NumParam With {.Name = "Quality", .Text = "Quality", .Init = -1, .VisibleFunc = Function()
                                                                                                                Dim cv = Codec.Value
                                                                                                                Return Mode.Value = EncodingMode.Quality AndAlso Not (cv = 6 OrElse cv = 9 OrElse cv = 10)
                                                                                                                'Return Mode.Value = EncodingMode.Quality AndAlso Not Codec.ValueText.EqualsAny("prores", "utvideo", "ffv1")
                                                                                                            End Function, .ArgsFunc = AddressOf GetQualityArgs, .Config = {-1, 63}},
                        New NumParam With {.Switch = "-threads", .Text = "Threads", .Config = {0, 64}},
                        New NumParam With {.Switch = "-tile-columns", .Text = "Tile Columns", .VisibleFunc = Function() Codec.Value = 12, .Value = 6, .DefaultValue = -1},
                        New NumParam With {.Switch = "-frame-parallel", .Text = "Frame Parallel", .VisibleFunc = Function() Codec.Value = 12, .Value = 1, .DefaultValue = -1},
                        New NumParam With {.Switch = "-auto-alt-ref", .Text = "Auto Alt Ref", .VisibleFunc = Function() Codec.Value = 12, .Value = 1, .DefaultValue = -1},
                        New NumParam With {.Switch = "-lag-in-frames", .Text = "Lag In Frames", .VisibleFunc = Function() Codec.Value = 12, .Value = 25, .DefaultValue = -1},
                        New BoolParam With {.Name = "h264_qsv Lookahead", .Text = "Lookahead", .Switch = "-look_ahead 1", .NoSwitch = "-look_ahead 0", .Value = False, .DefaultValue = True, .VisibleFunc = Function() Codec.Value = 13},
                        New BoolParam With {.Name = "h264_qsv VCM", .Text = "VCM", .Switch = "-vcm 1", .NoSwitch = "-vcm 0", .Value = False, .DefaultValue = True, .VisibleFunc = Function() Codec.Value = 13},
                        Custom})
                End If

                Return ItemsValue
            End Get
        End Property

        Overloads Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim sourcePath = p.Script.Path
            Dim sb As New StringBuilder(256)

            If includePaths AndAlso includeExecutable Then
                sb.Append(Package.ffmpeg.Path.Escape)
            End If

            'To DO Check Vsync: 0 - passthrough(int values are depreciated) Each frame Is passed with its timestamp from the demuxer to the muxer, 
            '1, cfr Frames will be duplicated And dropped to achieve exactly the requested constant frame rate.
            '2, vfr Frames are passed through with their timestamp Or dropped so as to prevent 2 frames from having the same timestamp.
            '3? drop As passthrough but destroys all timestamps, making the muxer generate fresh timestamps based on frame-rate.
            '-1, auto Chooses between 1 And 2 depending on muxer capabilities. This Is the default method.
            'p.ExtractTimestamps seems like death switch, nvidia paper uses vsync 0

            If p.ExtractTimestamps Then
                sb.Append(" -vsync 0 -strict -1")
            Else
                sb.Append(" -vsync 1 -strict -1")
            End If

            Select Case Decoder.Value
                Case 1
                    sourcePath = p.LastOriginalSourceFile
                Case 2
                    sourcePath = p.LastOriginalSourceFile
                    sb.Append(" -hwaccel qsv")
                Case 3
                    sourcePath = p.LastOriginalSourceFile
                    sb.Append(" -hwaccel dxva2")
                Case 4
                    sourcePath = p.LastOriginalSourceFile
                    sb.Append(" -hwaccel_output_format cuda")
                    '        sb.Append( " -hwaccel cuda" ???
            End Select

            If sourcePath.Ext.Equals("vpy") Then
                sb.Append(" -f vapoursynth")
            End If

            If includePaths Then
                sb.Append(" -an -i ").Append(sourcePath.LongPathPrefix.Escape)
            End If

            'Dim q = From i In Items Where i.GetArgs?.Length > 0 AndAlso Not IsCustom(i.Switch)
            'If q.Any Then ret &= " " & q.Select(Function(item) item.GetArgs).Join(" ")
            For i = 0 To Me.Items.Count - 1
                Dim prm = Items(i)
                Dim arg As String = prm.GetArgs
                If arg?.Length > 0 Then
                    If Not IsCustom(prm.Switch) Then sb.Append(" ").Append(arg)
                End If
            Next i

            If Calc.IsARSignalingRequired Then
                sb.Append(" -aspect ").Append(Calc.GetTargetDAR.ToInvariantString.Shorten(8))
            End If

            Select Case Mode.Value
                Case EncodingMode.TwoPass
                    sb.Append(" -pass ").Append(pass)
                    sb.Append(" -b:v ").Append(p.VideoBitrate).Append("k")

                    If pass = 1 Then
                        sb.Append(" -f rawvideo")
                    End If
                Case EncodingMode.OnePass
                    sb.Append(" -b:v ").Append(p.VideoBitrate).Append("k")
            End Select

            Select Case Codec.Value
                Case 3
                    sb.Append(" -tag:v xvid")
                Case 2
                    sb.Append(" -strict experimental")
            End Select

            Dim targetPath As String

            If Mode.OptionText = "Two Pass" AndAlso pass = 1 Then
                targetPath = "NUL"
            Else
                targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt).LongPathPrefix.Escape
            End If

            If includePaths Then
                sb.Append(" -an -y").Append(s.GetFFLogLevel(FfLogLevel.info)).Append(" -hide_banner ").Append(targetPath)
            End If

            Return sb.ToString.Trim
        End Function

        Function IsCustom(switch As String) As Boolean
            If switch.NullOrEmptyS Then
                Return False
            End If

            Dim cVal As String = Custom.Value
            If cVal?.Length > 0 Then
                If cVal.EndsWith(switch, StringComparison.Ordinal) OrElse cVal.Contains(switch & " ") Then
                    Return True
                End If
            End If
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.ffmpeg
        End Function

        Function GetQualityArgs() As String
            If Mode.Value = EncodingMode.Quality Then
                Dim param = GetNumParamByName("Quality")
                Dim val As Double = param.Value

                If val <> param.DefaultValue Then
                    Select Case Codec.Value
                        Case 11, 12
                            Return "-crf " & val & " -b:v 0"
                        Case 0, 1, 2
                            Return "-crf " & val
                        Case 15
                            If h264_nvenc_rc.Value = 1 Then
                                Return "-qp " & val
                            Else
                                Return "-cq " & val
                            End If
                        Case 16
                            If h264_nvenc_rc.Value = 1 Then
                                Return "-qp " & val
                            Else
                                Return "-cq " & val
                            End If
                            'Return "-cq " & val 'Org Was only this !
                        Case Else
                            Return "-q:v " & val
                    End Select
                End If
            End If
        End Function
    End Class

    Enum EncodingMode
        Quality
        OnePass
        TwoPass
    End Enum
End Class
