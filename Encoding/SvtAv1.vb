﻿
Imports System.Text
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class SVTAV1
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "AV1 | SVT-AV1"
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
        Dim encParams As New EncoderParams
        Dim store = ParamsStore.GetDeepClone
        encParams.Init(store)

        Using form As New CommandLineForm(encParams)
            Dim saveProfileAction = Sub()
                                        Dim enc = Me.GetDeepClone
                                        Dim encParamsCopy As New EncoderParams
                                        Dim storeCopy = store.GetDeepClone
                                        encParamsCopy.Init(storeCopy)
                                        enc.Params = encParamsCopy
                                        enc.ParamsStore = storeCopy
                                        SaveProfile(enc)
                                    End Sub

            form.cms.Items.Add(New ActionMenuItem("Save Profile...", saveProfileAction, ImageHelp.GetImageC(Symbol.Save)))

            If form.ShowDialog() = DialogResult.OK Then
                Params = encParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputExt() As String
        Get
            Return "ivf"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.SVTAV1
            proc.IntegerFrameOutput = True
            proc.FrameCount = p.Script.GetFrameCount
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + Params.GetCommandLine(True, True) + """"
            proc.Start()
        End Using

        AfterEncoding()
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList
        ret.Add("Encoder Options", AddressOf ShowConfigDialog)
        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return ret
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = 0
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
            Title = "SVT-AV1 Options"
        End Sub

        Property Mode As New OptionParam With {
            .Switch = "-rc",
            .Switches = {"-tbr"},
            .Text = "Mode",
            .IntegerValue = True,
            .Options = {"0: CQP", "1: VBR", "2: CVBR"}}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)(16)

                    Add("Basic",
                        New StringParam With {.Text = "Custom", .Quotes = QuotesMode.Never, .AlwaysOn = True},
                        Mode,
                        New OptionParam With {.Switch = "--preset", .Text = "Preset", .Init = 8, .IntegerValue = True, .Options = {"0: Very Slow", "1: Slower", "2: Slow", "3: Medium", "4: Fast", "5: Faster", "6: Very Fast", "7: Super Fast", "8: Ultra Fast"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .IntegerValue = True, .Options = {"0: Main", "1: High", "2: Professional"}},
                        New OptionParam With {.Switch = "--scm", .Text = "Screen Content Mode", .IntegerValue = True, .Options = {"0: OFF", "1: ON", "2: Content Based Detection"}},
                        New OptionParam With {.Switch = "--irefresh-type", .Text = "Intra Refresh Type", .Options = {"1: CRA (Open GOP)", "2: IDR (Closed GOP)"}, .Values = {"1", "2"}},
                        New NumParam With {.Switch = "--keyint", .Text = "Intra Period", .Init = -1, .Config = {-2, 255, 1}},
                        New NumParam With {.Switch = "-q", .Text = "QP", .Init = 50, .Config = {0, 63, 1}})

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

        Public Overrides Sub ShowHelp(switch As String)
            g.ShowCommandLineHelp(Package.SVTAV1, switch)
        End Sub

        Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim sb As New StringBuilder(256)

            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then
                sb.Append(Package.ffmpeg.Path.Escape).Append(If(p.Script.Engine = ScriptEngine.VapourSynth, " -f vapoursynth", "")).Append(" -i ").Append(p.Script.Path.Escape).
                    Append(" -f yuv4mpegpipe -strict -1").Append(s.GetFFLogLevel(FfLogLevel.fatal)).Append(" -hide_banner - | ").Append(Package.SVTAV1.Path.Escape)
            End If

            For i = 0 To Items.Count - 1
                Dim arg As String = Items(i).GetArgs
                If arg?.Length > 0 Then
                    sb.Append(" ").Append(arg)
                End If
            Next i

            If Mode.Value <> 0 Then
                sb.Append(" -tbr ").Append(p.VideoBitrate)
            End If

            If includePaths Then
                sb.Append(" -n ").Append(p.Script.GetFrameCount).Append(" -i stdin -b ").Append(targetPath.Escape)
            End If

            Return sb.ToString.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.SVTAV1
        End Function
    End Class
End Class
