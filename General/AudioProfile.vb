
Imports System.Text
Imports System.Text.RegularExpressions
Imports JM.LinqFaster
Imports KGySoft.CoreLibraries
Imports Microsoft.VisualBasic
Imports StaxRip.UI

<Serializable()>
Public MustInherit Class AudioProfile
    Inherits Profile
    Property Language As New Language
    Property Delay As Integer
    Property Depth As Integer
    Property StreamName As String = ""
    Property Gain As Double
    Property Streams As List(Of AudioStream) = New List(Of AudioStream)
    Property [Default] As Boolean
    Property Forced As Boolean
    Property ExtractDTSCore As Boolean
    Property Decoder As AudioDecoderMode
    Property DecodingMode As AudioDecodingMode

    Overridable Property Channels As Integer = 6
    Overridable Property OutputFileType As String = "unknown"
    Overridable Property Bitrate As Double
    Overridable Property SupportedInput As String()

    Overridable Property CommandLines As String

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Sub New(name As String,
            bitrate As Integer,
            input As String(),
            fileType As String,
            channels As Integer)

        MyBase.New(name)

        Me.Channels = channels
        Me.Bitrate = bitrate
        SupportedInput = input
        OutputFileType = fileType
    End Sub

    Public FileKeyHashValue As Long = MediaInfo.KeyDefault
    'Public Property FileKeyHash As Long
    '    Get
    '        If FileKeyHashValue = MediaInfo.KeyDefault AndAlso Not File.NullOrEmptyS Then
    '       Return() ((FileValue.GetHashCode + 2147483648L) << 16) + FileValue.Length
    '        End If
    '        Return FileKeyHash
    '    End Get
    '    Set(value As Long)
    '        FileKeyHashValue = value
    '    End Set
    'End Property

    Public FileValue As String = ""
    Property File As String
        Get
            Return FileValue
        End Get
        Set(value As String)
            If AudioConverterForm.AudioConverterMode Then 'AudioConverter Opt.
                If value IsNot Nothing Then FileValue = value
            Else
                If FileValue <> value Then
                    FileValue = value
                    Stream = Nothing
                    DisplayNameValue = Nothing
                    FileKeyHashValue = MediaInfo.KeyDefault
                    SourceSamplingRateValue = 0
                    OnFileChanged()
                End If
            End If
        End Set
    End Property

    Private StreamValue As AudioStream
    Property Stream As AudioStream
        Get
            Return StreamValue
        End Get
        Set(value As AudioStream)
            If value IsNot StreamValue Then
                StreamValue = value

                If Stream IsNot Nothing Then
                    If AudioConverterForm.AudioConverterMode OrElse Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then 'AudioConverter Opt.
                        Delay = Stream.Delay
                    End If

                    Language = Stream.Language
                    Forced = Stream.Forced
                    Me.Default = Stream.Default

                    If StreamName.NullOrEmptyS AndAlso Stream.Title.NotNullOrEmptyS Then
                        StreamName = Stream.Title
                    End If
                End If

                If Not AudioConverterForm.AudioConverterMode Then OnStreamChanged()
            End If
        End Set
    End Property

    <NonSerialized()>
    Public DisplayNameValue As String
    Property DisplayName As String
        Get
            If AudioConverterForm.AudioConverterMode Then
                If DisplayNameValue IsNot Nothing Then Return DisplayNameValue
                If FileValue Is "" Then Return ""
            End If

            Dim ret As String
            If Stream Is Nothing Then
                Dim streams = MediaInfo.GetAudioStreams(File, FileKeyHashValue)

                If streams.Count > 0 Then
                    If AudioConverterForm.AudioConverterMode Then   'AudioConverter Opt.
                        ret = streams(0).Name(True) & " (" & File.FileName & ")" ' No Substring 3 Comunicate to AudioStream
                    Else
                        ret = GetAudioText(streams(0), File)
                    End If
                Else
                    ret = File.FileName
                End If
            Else
                ret = Stream.Name & " (" & File.Ext & ")"
            End If

            DisplayNameValue = ret
            Return DisplayNameValue
        End Get
        Set(value As String)
            DisplayNameValue = value
        End Set
    End Property

    Public SourceSamplingRateValue As Integer
    Public ReadOnly Property SourceSamplingRate As Integer
        Get
            If SourceSamplingRateValue <= 0 Then
                If Stream Is Nothing Then
                    If (AudioConverterForm.AudioConverterMode AndAlso File.NotNullOrEmptyS) OrElse IO.File.Exists(File) Then ' 'AudioConverter Opt.
                        SourceSamplingRateValue = Math.Abs(MediaInfo.GetAudio(File, "SamplingRate", FileKeyHashValue).ToInt(48000))
                    End If
                Else
                    SourceSamplingRateValue = Math.Abs(Stream.SamplingRate)
                End If
            End If

            Select Case SourceSamplingRateValue
                Case Is = 0
                    SourceSamplingRateValue = 48000
                Case Is > 48000 * 256
                    SourceSamplingRateValue = 48000 * 256
            End Select

            Return SourceSamplingRateValue
        End Get
    End Property

    ReadOnly Property HasStream As Boolean
        Get
            Return Stream IsNot Nothing
        End Get
    End Property

    Overridable Sub Migrate()
        'FileValue = ""  ' or Nothing ????
        FileKeyHashValue = MediaInfo.KeyDefault
        DisplayNameValue = Nothing
        'Debug todo: Remove some of it: ???
        SourceSamplingRateValue = 0
        'Streams = Nothing
        'StreamName = Nothing
        '_Decoder = Nothing
        '_Language = New Language
        'Depth =  Org was 24???
    End Sub

    'Public Function DeepCopy(path As String) As AudioProfile
    '    Dim other As AudioProfile = DirectCast(Me.MemberwiseClone(), AudioProfile)
    'gap.Bitrate = TempProfile.Bitrate 'better, for GAP only?
    'gap.Language = TempProfile.Language
    'gap.Delay = TempProfile.Delay
    'gap.DefaultnameValue = Nothing
    'gap.Name = TempProfile.Name
    'gap.StreamName = TempProfile.StreamName
    'gap.Gain = TempProfile.Gain
    'gap.Default = TempProfile.Default
    'gap.Forced = TempProfile.Forced
    'gap.Params = TempProfile.Params
    'gap.Decoder = TempProfile.Decoder
    'gap.DecodingMode = TempProfile.DecodingMode
    'gap.ExtractDTSCore = TempProfile.ExtractDTSCore
    'gap.Depth = TempProfile.Depth
    '    other.File = path
    '    other.Name = Name
    '    other.DisplayName = Nothing
    '    ' batch cmd lines, params etc. mess
    '    Return other
    'End Function

    ReadOnly Property ConvertExt As String
        Get
            Dim ret As String
            Select Case DecodingMode
                Case AudioDecodingMode.WAVE
                    ret = "wav"
                Case AudioDecodingMode.FLAC
                    ret = "flac"
                Case Else
                    ret = "wv"
            End Select

            If Not SupportedInput.NothingOrEmpty Then
                If Not SupportedInput.ContainsString(ret) Then
                    ret = "wv"
                End If
                If Not SupportedInput.ContainsString(ret) Then
                    ret = "wav"
                End If
            End If
            'To Do: Check this VW to save temp size for cuts,  -> only AndAlso p.Ranges.Count > 0, decoder, eac3to ???
            'If DecodingMode = AudioDecodingMode.Pipe Then
            'If TypeOf Me Is GUIAudioProfile Then
            'Dim gap = DirectCast(Me, GUIAudioProfile)
            'If {AudioDecoderMode.ffmpeg, AudioDecoderMode.Automatic}.Contains((gap.Decoder)) Then
            'If gap.GetEncoder() <> GuiAudioEncoder.eac3to Then
            'ret = "wv"
            'End If
            'End If
            'End If
            'End If
            Return ret
        End Get
    End Property

    Overridable Sub OnFileChanged()
    End Sub

    Overridable Sub OnStreamChanged()
    End Sub

    Function ContainsCommand(value As String) As Boolean
        Return CommandLines.ContainsEx(value)
    End Function

    Function IsUsedAndContainsCommand(value As String) As Boolean
        Return File.NotNullOrEmptyS AndAlso CommandLines.ContainsEx(value)
    End Function

    Function GetDuration() As TimeSpan
        If AudioConverterForm.AudioConverterMode OrElse IO.File.Exists(File) Then 'AudioConverter Opt.
            If Stream Is Nothing Then
                Return TimeSpan.FromMilliseconds(MediaInfo.GetAudio(File, "Duration", FileKeyHashValue).ToDouble)
            Else
                Using mi As New MediaInfo(File)
                    Return TimeSpan.FromMilliseconds(mi.GetAudio(Stream.Index, "Duration").ToDouble)
                End Using
            End If
        End If
    End Function

    Function GetAudioText(stream As AudioStream, path As String) As String
        For Each i In Language.Languages
            If path.Contains(i.CultureInfo.EnglishName) Then
                stream.Language = i
                Exit For
            End If
        Next

        Dim matchDelay = Regex.Match(path, " (-?\d+)ms")

        If matchDelay.Success Then
            stream.Delay = matchDelay.Groups(1).Value.ToInt
        End If

        Dim matchID = Regex.Match(path, " ID(\d+)")
        Dim name As String
        name = stream.Name.Substring(3)

        If File.Base = p.SourceFile.Base Then
            Return name & " (" & File.Ext & ")"
        Else
            Return name & " (" & File.FileName & ")"
        End If
    End Function

    Sub SetStreamOrLanguage()
        If File.NullOrEmptyS Then
            Exit Sub
        End If

        If Not File.Equals(p.LastOriginalSourceFile) Then
            For Each i In Language.Languages
                If File.Contains(i.CultureInfo.EnglishName) Then
                    Language = i
                    Exit Sub
                End If
            Next
        Else
            For Each i In Streams
                If i.Language.Equals(Language) Then
                    Stream = i
                    Exit For
                End If
            Next

            If Stream Is Nothing AndAlso Streams.Count > 0 Then
                Stream = Streams(0)
            End If
        End If
    End Sub

    Function IsInputSupported() As Boolean
        Return SupportedInput.NothingOrEmpty OrElse SupportedInput.ContainsString(File.Ext)
    End Function

    Function IsMuxProfile() As Boolean
        Return TypeOf Me Is MuxAudioProfile
    End Function

    Function IsNullProfile() As Boolean
        Return TypeOf Me Is NullAudioProfile
    End Function
    Function IsBatchProfile() As Boolean
        Return TypeOf Me Is BatchAudioProfile
    End Function

    Function IsGapProfile() As Boolean
        Return TypeOf Me Is GUIAudioProfile
    End Function

    Overridable Sub Encode()
    End Sub

    Overridable Sub EditProject()
    End Sub
    Overridable Function EditAudioConv() As DialogResult
        Return DialogResult.Cancel
    End Function

    Overridable Function HandlesDelay() As Boolean
    End Function

    Function GetTrackID() As Integer
        If Me Is p.Audio0 Then Return 1
        If Me Is p.Audio1 Then Return 2

        For x = 0 To p.AudioTracks.Count - 1
            If Me Is p.AudioTracks(x) Then
                Return x + 3
            End If
        Next
    End Function

    Function GetOutputFile() As String
        Dim base As String

        If Not AudioConverterForm.AudioConverterMode AndAlso p.TempDir.EndsWithEx("_temp\") AndAlso File.Base.StartsWithEx(p.SourceFile.Base) Then
            base = File.Base.Substring(p.SourceFile.Base.Length).Trim

            'To Do: empty pipe streams temp files
            If base.NullOrEmptyS Then ShortBegEnd(File.Base)
        Else
            base = File.Base
        End If

        If base.NullOrEmptyS Then
            base = "temp"
        End If

        If Delay <> 0 Then
            If HandlesDelay() Then
                If base.Contains("ms") Then
                    Dim re As New Regex(" (-?\d+)ms")

                    If re.IsMatch(base) Then
                        base = re.Replace(base, "")
                    End If
                End If
            Else
                If Not base.Contains("ms") Then
                    base += " " & Delay & "ms"
                End If
            End If
        End If

        Dim outfile As String
        If AudioConverterForm.AudioConverterMode Then
            outfile = p.TempDir & base & "." & OutputFileType.ToLowerInvariant
            If File.IsEqualIgnoreCase(outfile) Then
                outfile = p.TempDir & base & "_new." & OutputFileType.ToLowerInvariant
            End If
        Else
            Dim tracks = g.GetAudioProfiles.WhereF(Function(track) track.File.NotNullOrEmptyS)
            Dim trackID = If(tracks.Count > 1, "_a" & GetTrackID(), "")
            outfile = p.TempDir & base & trackID & "." & OutputFileType.ToLowerInvariant

            If File.IsEqualIgnoreCase(outfile) Then
                outfile = p.TempDir & base & trackID & "_new." & OutputFileType.ToLowerInvariant
            End If
        End If

        Return outfile
    End Function

    Function ExpandMacros(value As String) As String
        Return ExpandMacros(value, True)
    End Function

    Function ExpandMacros(value As String, silent As Boolean) As String
        If value.NullOrEmptyS Then Return ""
        value = value.Replace("""%input%""", File.Escape)
        value = value.Replace("%input%", File.Escape)
        value = value.Replace("""%output%""", GetOutputFile.Escape)
        value = value.Replace("%output%", GetOutputFile.Escape)
        value = value.Replace("%bitrate%", Bitrate.ToString)
        value = value.Replace("%channels%", Channels.ToString)
        value = value.Replace("%language_native%", Language.CultureInfo.NativeName)
        value = value.Replace("%language_english%", Language.Name)
        value = value.Replace("%delay%", Delay.ToInvariantString)

        Return Macro.Expand(value)
    End Function

    Shared Function GetDefaults() As List(Of AudioProfile)
        Dim ret As New List(Of AudioProfile)

        ret.Add(New GUIAudioProfile(AudioCodec.AAC, 54))
        ret.Add(New GUIAudioProfile(AudioCodec.Opus, 1) With {.Bitrate = 256})
        ret.Add(New GUIAudioProfile(AudioCodec.FLAC, 0.3))
        ret.Add(New GUIAudioProfile(AudioCodec.WavPack, 0.3))
        ret.Add(New GUIAudioProfile(AudioCodec.Vorbis, 1))
        ret.Add(New GUIAudioProfile(AudioCodec.MP3, 2))
        ret.Add(New GUIAudioProfile(AudioCodec.AC3, 1.0) With {.Channels = 6, .Bitrate = 640})
        ret.Add(New GUIAudioProfile(AudioCodec.EAC3, 1.0) With {.Channels = 6, .Bitrate = 640})
        ret.Add(New BatchAudioProfile(640, {}, "ac3", 6, """%app:ffmpeg%"" -sn -vn -dn -i ""%input%"" -b:a %bitrate%k -y -hide_banner ""%output%"""))
        ret.Add(New MuxAudioProfile())
        ret.Add(New NullAudioProfile())

        Return ret
    End Function

End Class

<Serializable()>
Public Class BatchAudioProfile
    Inherits AudioProfile

    Sub New(bitrate As Integer,
            input As String(),
            fileType As String,
            channels As Integer,
            commandLines As String)

        MyBase.New("Command Line", bitrate, input, fileType, channels)
        Me.CommandLines = commandLines
        CanEditValue = True
    End Sub

    Overrides Function Edit() As DialogResult
        Using form As New CommandLineAudioEncoderForm(Me)
            form.mbLanguage.Enabled = True
            form.lLanguage.Enabled = True
            form.tbDelay.Enabled = False
            form.lDelay.Enabled = False
            Return form.ShowDialog()
        End Using
    End Function

    Overrides Function EditAudioConv() As DialogResult
        Using form As New CommandLineAudioEncoderForm(Me)
            form.mbLanguage.Enabled = True
            form.lLanguage.Enabled = True
            form.tbDelay.Enabled = True
            form.lDelay.Enabled = True
            Return form.ShowDialog()
        End Using
    End Function

    Function GetCode() As String
        Return ExpandMacros(CommandLines).Trim
    End Function

    Overrides Sub Encode()
        If File.NotNullOrEmptyS Then
            Dim bitrateBefore = p.VideoBitrate
            Dim targetPath = GetOutputFile()

            For Each line In Macro.Expand(GetCode).SplitLinesNoEmpty
                Using proc As New Proc
                    proc.Header = "Audio Encoding: " + Name
                    proc.SkipStrings = Proc.GetSkipStrings(CommandLines)

                    'Test this: Progress bar
                    If ContainsCommand("|") AndAlso CommandLines.ToLowerEx.ContainsAny("qaac", "opusenc", "ffmpeg", "lossywav", "\flac ") Then 'ToLowerEx(True)
                        Try
                            proc.Duration = GetDuration()
                        Catch
                        End Try
                    End If

                    proc.File = "cmd.exe"
                    proc.Arguments = "/S /C """ + line + """"

                    Try
                        proc.Start()
                    Catch ex As AbortException
                        Throw ex
                    Catch ex As Exception
                        g.ShowException(ex)
                        Throw New AbortException
                    End Try
                End Using
            Next

            If g.FileExists(targetPath) Then
                File = targetPath

                If Not p.BitrateIsFixed Then
                    Bitrate = Calc.GetBitrateFromFile(File, p.TargetSeconds)
                    p.VideoBitrate = CInt(Fix(Calc.GetVideoBitrate))

                    If Not p.VideoEncoder.QualityMode Then
                        Log.WriteLine("Video Bitrate: " + bitrateBefore.ToInvariantString + " -> " & p.VideoBitrate & BR)
                    End If
                End If

                Log.WriteLine(MediaInfo.GetSummary(File, FileKeyHashValue))
            Else
                Log.Write("Error", "no output found")

                If Not File.Ext.Equals("wav") Then
                    'TO DO test this:
                    Dim OldDecMode = DecodingMode
                    DecodingMode = AudioDecodingMode.WAVE
                    Audio.Convert(Me)

                    If File.Ext.Equals("wav") Then
                        Encode()
                    End If

                    DecodingMode = OldDecMode
                End If
            End If
        End If
    End Sub

    Overrides Sub EditProject()
        Using f As New CommandLineAudioEncoderForm(Me)
            f.ShowDialog()
        End Using
    End Sub

    Overrides Function HandlesDelay() As Boolean
        Return CommandLines.Contains("%delay%")
    End Function
End Class

<Serializable()>
Public Class NullAudioProfile
    Inherits AudioProfile

    Sub New()
        MyBase.New("No Audio", 0, {}, "ignore", 0)
    End Sub

    Overrides Function HandlesDelay() As Boolean
    End Function

    Overrides Sub EditProject()
        Using form As New SimpleSettingsForm("Null Audio Profile Options")
            form.ScaleClientSize(20, 10)
            Dim ui = form.SimpleUI
            ui.Store = Me

            Dim n = ui.AddNum()
            n.Text = "Reserved Bitrate:"
            n.Config = {0, Integer.MaxValue, 8}
            n.Property = NameOf(Bitrate)

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()
            End If
        End Using
    End Sub

    Public Overrides Property OutputFileType As String
        Get
            Return "ignore"
        End Get
        Set(value As String)
        End Set
    End Property

    Public Overrides Sub Encode()
    End Sub
End Class

<Serializable()>
Public Class MuxAudioProfile
    Inherits AudioProfile

    Sub New()
        MyBase.New("Copy/Mux", 0, Nothing, "ignore", 2)
        CanEditValue = True
    End Sub

    Public Overrides Property OutputFileType As String
        Get
            If Stream Is Nothing Then
                Return File.Ext
            Else
                Return Stream.Ext
            End If
        End Get
        Set(value As String)
        End Set
    End Property

    Overrides Property SupportedInput As String()
        Get
            Return {}
        End Get
        Set(value As String())
        End Set
    End Property

    Overrides Function Edit() As DialogResult
        Return Edit(False)
    End Function

    Overrides Sub EditProject()
        Edit(True)
    End Sub

    Overrides Sub Encode()
    End Sub

    Overrides Sub OnFileChanged()
        MyBase.OnFileChanged()
        SetBitrate()
    End Sub

    Overrides Sub OnStreamChanged()
        MyBase.OnStreamChanged()
        SetBitrate()
    End Sub

    Sub SetBitrate()
        If Stream Is Nothing Then
            Bitrate = Calc.GetBitrateFromFile(File, p.SourceSeconds)
        Else
            Bitrate = Stream.Bitrate + Stream.Bitrate2
        End If
    End Sub

    Private Overloads Function Edit(showProjectSettings As Boolean) As DialogResult
        Using form As New SimpleSettingsForm("Audio Mux Options",
            "The Audio Mux options allow to add a audio file without reencoding.")

            form.ScaleClientSize(30, 15)

            Dim ui = form.SimpleUI
            Dim page = ui.CreateFlowPage("main page")
            page.SuspendLayout()

            Dim tbb = ui.AddTextButton(page)
            tbb.Label.Text = "Track Name:"
            tbb.Label.Help = "Track name used by the muxer. The track name may contain macros."
            tbb.Edit.Expand = True
            tbb.Edit.Text = StreamName
            tbb.Edit.SaveAction = Sub(value) StreamName = value
            tbb.Button.Text = "Macro Editor..."
            tbb.Button.ClickAction = AddressOf tbb.Edit.EditMacro

            Dim nb = ui.AddNum(page)
            nb.Label.Text = "Delay:"
            nb.Label.Help = "Delay used by the muxer."
            nb.NumEdit.Config = {Integer.MinValue, Integer.MaxValue, 1}
            nb.NumEdit.Value = Delay
            nb.NumEdit.SaveAction = Sub(value) Delay = CInt(value)

            Dim mbi = ui.AddMenu(Of Language)(page)
            mbi.Label.Text = "Language:"
            mbi.Label.Help = "Language of the audio track."
            mbi.Button.Value = Language
            mbi.Button.SaveAction = Sub(value) Language = value

            For Each i In Language.Languages
                If i.IsCommon Then
                    mbi.Button.Add(i.ToString, i)
                Else
                    mbi.Button.Add("More | " + i.ToString.Substring(0, 1).ToUpper + " | " + i.ToString, i)
                End If
            Next

            Dim cb = ui.AddBool(page)
            cb.Text = "Default"
            cb.Help = "Flaged as default in MKV."
            cb.Checked = [Default]
            cb.SaveAction = Sub(value) [Default] = value

            cb = ui.AddBool(page)
            cb.Text = "Forced"
            cb.Help = "Flaged as forced in MKV."
            cb.Checked = Forced
            cb.SaveAction = Sub(value) Forced = value

            cb = ui.AddBool(page)
            cb.Text = "Extract DTS Core"
            cb.Help = "Only include DTS core using mkvmerge."
            cb.Checked = ExtractDTSCore
            cb.SaveAction = Sub(value) ExtractDTSCore = value

            page.ResumeLayout()

            Dim ret = form.ShowDialog()

            If ret = DialogResult.OK Then
                ui.Save()
            End If

            Return ret
        End Using
    End Function
End Class

<Serializable()>
Public Class GUIAudioProfile
    Inherits AudioProfile

    Property Params As New Parameters
    <NonSerialized()>
    Private GainWasNormalized As Boolean

    Sub New(codec As AudioCodec, quality As Single)
        MyBase.New(Nothing)

        Params.Codec = codec
        Params.Quality = quality

        Select Case codec
            Case AudioCodec.AC3, AudioCodec.DTS, AudioCodec.EAC3
                Params.RateMode = AudioRateMode.CBR
            Case Else
                Params.RateMode = AudioRateMode.VBR
        End Select

        Bitrate = GetBitrate()
    End Sub

    Public SourceChannels As Integer
    Public Overrides Property Channels As Integer
        Get
            Select Case Params.ChannelsMode
                Case ChannelsMode.Original

                    If SourceChannels <= 0 OrElse Not AudioConverterForm.AudioConverterMode Then
                        If Stream IsNot Nothing Then
                            SourceChannels = Math.Abs(If(Stream.Channels > Stream.Channels2, Stream.Channels, Stream.Channels2))
                        Else
                            If (AudioConverterForm.AudioConverterMode AndAlso File.NotNullOrEmptyS) OrElse IO.File.Exists(File) Then 'AudioConverter Opt.
                                SourceChannels = Math.Abs(MediaInfo.GetChannels(File, FileKeyHashValue))
                            End If
                        End If
                    End If

                    SourceChannels = If(SourceChannels > 0 AndAlso SourceChannels <= (1 << 18), SourceChannels, 6)
                    Return SourceChannels
                Case ChannelsMode._1
                    Return 1
                Case ChannelsMode._2
                    Return 2
                Case ChannelsMode._6
                    Return 6
                Case ChannelsMode._7
                    Return 7
                Case ChannelsMode._8
                    Return 8
                Case Else
                    Throw New NotImplementedException
            End Select
        End Get
        Set(value As Integer)
        End Set
    End Property

    ReadOnly Property TargetSamplingRate As Integer
        Get
            If Params.SamplingRate <> 0 Then
                Return Params.SamplingRate
            Else
                Return SourceSamplingRate
            End If
        End Get
    End Property

    Public Overrides Sub Migrate()
        MyBase.Migrate()
        Params.Migrate()
        DefaultnameValue = Nothing
        SourceDepth = 0
        SourceChannels = 0
        _SupportedInput = Nothing
        _CommandLines = Nothing
        _OutputFileType = Nothing
        If Params.Codec <> AudioCodec.DTS Then ExtractDTSCore = False
    End Sub

    Public SourceDepth As Integer
    Private Function GetCalcDepth() As Integer

        If Params.Codec = AudioCodec.WavPack AndAlso Params.WavPackPreQuant > 0 AndAlso Params.WavPackMode = 0 AndAlso Not Params.Encoder = GuiAudioEncoder.ffmpeg Then
            Return Params.WavPackPreQuant
        End If

        If Depth > 0 Then Return Depth

        If SourceDepth <= 0 OrElse Not AudioConverterForm.AudioConverterMode Then
            If Stream IsNot Nothing Then
                SourceDepth = Math.Abs(Stream.BitDepth)
            Else
                If (AudioConverterForm.AudioConverterMode AndAlso File.NotNullOrEmptyS) OrElse IO.File.Exists(File) Then
                    SourceDepth = Math.Abs(MediaInfo.GetAudio(File, "BitDepth", FileKeyHashValue).ToInt)
                End If
            End If
        End If

        SourceDepth = If(SourceDepth > 0 AndAlso SourceDepth <= 64, SourceDepth, 16)
        Return SourceDepth

    End Function

    Function GetBitrate() As Integer
        If VBRMode() Then
            Select Case Params.Codec
                Case AudioCodec.AAC
                    Dim ch = Channels
                    Select Case Params.Encoder
                        Case GuiAudioEncoder.qaac, GuiAudioEncoder.Automatic
                            Return Calc.GetYFromTwoPointForm(0F, (50.0F / 8.0F * ch), 127.0F, (1000.0F / 8.0F * ch), Params.Quality)
                        Case GuiAudioEncoder.eac3to
                            Return Calc.GetYFromTwoPointForm(0.01F, (50.0F / 8.0F * ch), 1, (1000.0F / 8.0F * ch), Params.Quality)
                        Case GuiAudioEncoder.fdkaac, GuiAudioEncoder.ffmpeg
                            Return Calc.GetYFromTwoPointForm(1.0F, (50.0F / 8.0F * ch), 5, (900.0F / 8.0F * ch), Params.Quality)
                        Case Else
                            Return Calc.GetYFromTwoPointForm(0F, (50.0F / 8 * ch), 127, (1000 / 8.0F * ch), Params.Quality)
                    End Select
                Case AudioCodec.Opus
                    Return CInt(Bitrate)
                Case AudioCodec.FLAC
                    Return CInt(Fix(TargetSamplingRate / 1000 * 0.55 * GetCalcDepth() * Channels))
                Case AudioCodec.WavPack
                    'If Params.WavPackMode = 0 Then
                    Return CInt(Fix(TargetSamplingRate / 1000 * 0.55 * GetCalcDepth() * Channels))
                    'End If
                Case AudioCodec.MP3
                    Return Calc.GetYFromTwoPointForm(9.0F, 65.0F, 0F, 245.0F, Params.Quality)
                Case AudioCodec.Vorbis
                    If Channels >= 6 Then
                        Return Calc.GetYFromTwoPointForm(0F, 120.0F, 10.0F, 1440.0F, Params.Quality)
                    Else
                        Return Calc.GetYFromTwoPointForm(0F, 64.0F, 10.0F, 500.0F, Params.Quality)
                    End If
            End Select
        Else
            Select Case Params.Codec
                Case AudioCodec.W64, AudioCodec.WAV
                    Return (TargetSamplingRate \ 1000) * GetCalcDepth() * Channels
            End Select
        End If

        Return CInt(Bitrate)
    End Function

    Public Overrides Sub Encode()
        If File.NotNullOrEmptyS Then
            Dim bitrateBefore = p.VideoBitrate
            Dim targetPath = GetOutputFile()
            Dim cl = GetCommandLine(True)

            Using proc As New Proc
                proc.Header = "Audio Encoding " & GetTrackID()

                If cl.Contains("|") Then
                    proc.File = "cmd.exe"
                    proc.Arguments = "/S /C """ + cl + """"
                Else
                    proc.CommandLine = cl
                End If

                If cl.Contains("qaac64") Then
                    proc.Package = Package.qaac
                    proc.SkipStrings = {", ETA ", "x)"}
                    If DecodingMode = AudioDecodingMode.Pipe Then proc.Duration = GetDuration()
                ElseIf cl.Contains("opusenc") Then
                    proc.Package = Package.OpusEnc
                    proc.SkipStrings = {"x realtime,"}
                    If DecodingMode = AudioDecodingMode.Pipe Then proc.Duration = GetDuration()
                ElseIf cl.Contains("fdkaac") Then
                    proc.Package = Package.fdkaac
                    proc.SkipStrings = {"%]", "x)"}
                ElseIf cl.Contains("eac3to") Then
                    proc.Package = Package.eac3to
                    proc.SkipStrings = {"process: ", "analyze: "}
                    proc.TrimChars = {"-"c, " "c}
                    g.AddToPath(Package.NeroAAC.Directory)
                ElseIf cl.Contains("ffmpeg") Then
                    If cl.Contains("libfdk_aac") Then
                        proc.Package = Package.ffmpeg_non_free
                    Else
                        proc.Package = Package.ffmpeg
                    End If

                    proc.SkipStrings = {"frame=", "size="}
                    proc.Encoding = Encoding.UTF8
                    proc.Duration = GetDuration()

                    ' Sometimes ffmpeg pipe blocks WP progress, console shows only creating .WV
                ElseIf cl.Contains("wavpack") Then
                    proc.Package = Package.WavPack
                    proc.SkipStrings = {"% done."}
                End If

                proc.Start()
            End Using

            If g.FileExists(targetPath) Then
                File = targetPath

                If Not p.BitrateIsFixed Then
                    Bitrate = Calc.GetBitrateFromFile(File, p.TargetSeconds)
                    p.VideoBitrate = CInt(Fix(Calc.GetVideoBitrate))

                    If Not p.VideoEncoder.QualityMode Then
                        Log.WriteLine("Video Bitrate: " + bitrateBefore.ToInvariantString + " -> " & p.VideoBitrate & BR)
                    End If
                End If

                Log.WriteLine(MediaInfo.GetSummary(File, FileKeyHashValue))
            Else
                Throw New ErrorAbortException("Error audio encoding", "The output file is missing")
            End If
        End If
    End Sub
    Sub NormalizeFF()
        If Not Params.Normalize OrElse ExtractCore Then 'OrElse ( SupportsNormalize())
            Exit Sub
        End If

        Select Case Params.ffmpegNormalizeMode
            Case ffmpegNormalizeMode.loudnorm

                Dim sb As New StringBuilder(128)
                If Params.ProbeSize <> 5 Then
                    sb.Append(" -probesize ").Append(Params.ProbeSize).Append("M")
                End If

                If Params.AnalyzeDuration <> 5 Then
                    sb.Append(" -analyzeduration ").Append(Params.AnalyzeDuration).Append("M")
                End If

                sb.Append(" -sn -vn -dn -i ").Append(File.LongPathPrefix.Escape).Append(" -sn -vn -dn")

                If Not Stream Is Nothing AndAlso Streams.Count > 1 Then
                    sb.Append(" -map 0:a:" & Stream.Index)
                End If

                sb.Append(s.GetFFLogLevel(FfLogLevel.info)).Append(" -hide_banner -af loudnorm=I=").Append(Params.ffmpegLoudnormIntegrated.ToInvariantString).Append(":TP=").Append(
                    Params.ffmpegLoudnormTruePeak.ToInvariantString).Append(":LRA=").Append(Params.ffmpegLoudnormLRA.ToInvariantString).Append(":print_format=summary -c:a pcm_f64le -f null NUL")
                'pcm_f64le - disable last pointless auto resempler step 64fp-16int to null, not measurable speed diff anyway

                Using proc As New Proc
                    proc.Header = "Find Gain " & GetTrackID()
                    proc.SkipStrings = {"frame=", "size="}
                    proc.Encoding = Encoding.UTF8
                    proc.Package = Package.ffmpeg
                    proc.Arguments = sb.ToString
                    proc.Duration = GetDuration()
                    proc.Start()

                    Dim match = Regex.Match(proc.Log.ToString, "Input Integrated:\s*([-+\.0-9]+)")
                    If match.Success Then Params.ffmpegLoudnormIntegratedMeasured = If(match.Groups(1).Value.ToDouble < 0, match.Groups(1).Value.ToDouble, 0)

                    match = Regex.Match(proc.Log.ToString, "Input True Peak:\s*([-+\.0-9]+)")
                    If match.Success Then Params.ffmpegLoudnormTruePeakMeasured = match.Groups(1).Value.ToDouble

                    match = Regex.Match(proc.Log.ToString, "Input LRA:\s*([-\.0-9]+)")
                    If match.Success Then Params.ffmpegLoudnormLraMeasured = match.Groups(1).Value.ToDouble

                    match = Regex.Match(proc.Log.ToString, "Input Threshold:\s*([-\.0-9]+)")
                    If match.Success Then Params.ffmpegLoudnormThresholdMeasured = match.Groups(1).Value.ToDouble

                    match = Regex.Match(proc.Log.ToString, "Target Offset:\s*([-+\.0-9]+)")
                    If match.Success Then Params.ffmpegLoudnormOffset = match.Groups(1).Value.ToDouble
                End Using

            Case ffmpegNormalizeMode.peak 'QAAC is ~x10 faster than ffmpeg,  also with --peak no PCM temp file like --normalize

                If GainWasNormalized Then 'OrElse ( SupportsNormalize())
                    Exit Sub
                End If

                Dim sb As New StringBuilder(128)
                sb.Append(Package.ffmpeg.Path.Escape)

                If Params.ProbeSize <> 5 Then
                    sb.Append(" -probesize ").Append(Params.ProbeSize).Append("M")
                End If
                String.Format("sd")
                If Params.AnalyzeDuration <> 5 Then
                    sb.Append(" -analyzeduration ").Append(Params.AnalyzeDuration).Append("M")
                End If

                sb.Append(" -sn -vn -dn -i ").Append(File.LongPathPrefix.Escape).Append(" -sn -vn -dn")

                If Not Stream Is Nothing AndAlso Streams.Count > 1 Then
                    sb.Append(" -map 0:a:").Append(Stream.Index)
                End If

                '-rematrix_maxval (def 0) Set maximum output value for rematrixing. This can be used to prevent clipping vs. preventing volume reduction. A value of 1.0 prevents clipping.
                'Without this ffmpeg tends to use different matrixes for FP and Int audio formats, also 1 is consistent with LibAV

                If Params.ChannelsMode <> ChannelsMode.Original Then
                    sb.Append(" -rematrix_maxval 1 -ac ").Append(Channels)
                    If Params.ffmpegLFEMixLevel <> 0 AndAlso Params.ChannelsMode < 3 Then
                        sb.Append(" -lfe_mix_level ").Append(Params.ffmpegLFEMixLevel.ToInvariantString)
                    End If
                End If

                Const UpSampleF As Integer = 4   'upsample true peak
                Dim SRate As Integer = SourceSamplingRate * UpSampleF
                If SRate < 44100 * UpSampleF Then SRate = 48000 * UpSampleF
                If SRate > 48000 * 256 Then SRate = 48000 * 256
                Dim QLogL As String = If(s.FfmpegLogLevel >= 24 OrElse s.FfmpegLogLevel = 0, " --verbose", "")

                sb.Append(s.GetFFLogLevel(FfLogLevel.error)).Append(" -hide_banner -c:a pcm_f32le -f wav - | ").Append(Package.qaac.Path.Escape).Append(" --rate ").Append(SRate).Append(" --peak").Append(QLogL).Append(" - ")

                Using proc As New Proc
                    proc.Header = "Find Gain " & GetTrackID()
                    proc.File = "cmd.exe"
                    proc.Arguments = "/S /C """ + sb.ToString + """"
                    proc.Package = Package.qaac
                    proc.SkipStrings = {"x)"}
                    proc.Duration = GetDuration()
                    proc.Start()

                    Dim match = Regex.Match(proc.Log.ToString, "Peak:\s*[.0-9]+\s*[(]([-.0-9]+)")
                    If match.Success Then
                        Gain = Math.Round(Gain - (match.Groups(1).Value.ToDouble + 0.05), 2)
                        GainWasNormalized = True
                    End If

                    'Dim GainNormalizedDB As Double  "Peak:\s+([.0-9]+)\s*"
                    'GainNormalizedDB = 20 * Math.Log10(Gain)
                End Using
            Case Else
                Exit Sub
        End Select

    End Sub

    Overrides Function Edit() As DialogResult
        Using form As New AudioForm()
            form.LoadProfile(Me)
            form.mbLanguage.Enabled = False
            form.numDelay.Enabled = False
            'form.numGain.Enabled = False
            Return form.ShowDialog()
        End Using
    End Function

    Overrides Sub EditProject()
        Using form As New AudioForm()
            form.LoadProfile(Me)
            form.ShowDialog()
        End Using
    End Sub

    Public Overrides Function EditAudioConv() As DialogResult
        Using form As New AudioForm()
            form.LoadProfile(Me)
            'form.mbLanguage.Enabled = False
            'form.numDelay.Enabled = False
            'form.numGain.Enabled = False
            Return form.ShowDialog()
        End Using
    End Function

    <NonSerialized()>
    Private _OutputFileType As String
    Public Overrides Property OutputFileType As String
        Get
            Select Case Params.Codec
                Case AudioCodec.AAC
                    Return "m4a"
                Case AudioCodec.Vorbis
                    Return "ogg"
                Case AudioCodec.WavPack
                    Return "wv"
                Case Else
                    Return [Enum](Of AudioCodec).ToString(Params.Codec).ToLowerInvariant
            End Select
        End Get
        Set(value As String)
            _OutputFileType = value
        End Set
    End Property

    Function GetEac3toCommandLine(includePaths As Boolean) As String
        Dim id As String
        Dim sb As New StringBuilder(128)

        If File.Ext.EqualsAny("ts", "m2ts", "mkv") AndAlso Not Stream Is Nothing Then
            id = (Stream.StreamOrder + 1) & ": "
        End If

        If includePaths Then
            sb.Append(Package.eac3to.Path.Escape).Append(" ").Append(id).Append(File.LongPathPrefix.Escape).Append(" ").Append(GetOutputFile.LongPathPrefix.Escape)
        Else
            sb.Append("eac3to")
        End If

        If Not (Params.Codec = AudioCodec.DTS AndAlso ExtractDTSCore) Then
            Select Case Params.Codec
                Case AudioCodec.AAC
                    sb.Append(" -quality=").Append(Params.Quality.ToInvariantString)
                Case AudioCodec.AC3
                    sb.Append(" -").Append(Bitrate)

                    If Not {192, 224, 384, 448, 640}.Contains(CInt(Bitrate)) Then
                        Return "Invalid bitrate, select 192, 224, 384, 448 or 640"
                    End If
                Case AudioCodec.DTS
                    sb.Append(" -").Append(Bitrate)
            End Select

            If Depth = 16 Then
                sb.Append(" -down16")
            End If

            If Params.SamplingRate <> 0 Then
                sb.Append(" -resampleTo").Append(Params.SamplingRate)
            End If

            If Params.FrameRateMode = AudioFrameRateMode.Speedup Then
                sb.Append(" -speedup")
            End If

            If Params.FrameRateMode = AudioFrameRateMode.Slowdown Then
                sb.Append(" -slowdown")
            End If

            If Delay <> 0 Then
                sb.Append(" ").Append(If(Delay > 0, "+", "")).Append(Delay).Append("ms")
            End If

            If Not GainWasNormalized Then
                If Params.Normalize Then
                    sb.Append(" -normalize")
                End If
            End If

            If Gain < 0 Then
                sb.Append(" ").Append(CInt(Gain)).Append("dB")
            End If

            If Gain > 0 Then
                sb.Append(" +").Append(CInt(Gain)).Append("dB")
            End If

            Select Case Channels
                Case 6
                    If Params.ChannelsMode <> ChannelsMode.Original Then
                        sb.Append(" -down6")
                    End If
                Case 2
                    If Params.eac3toStereoDownmixMode = 0 Then
                        If Params.ChannelsMode <> ChannelsMode.Original Then
                            sb.Append(" -downStereo")
                        End If
                    Else
                        sb.Append(" -downDpl")
                    End If
            End Select

            If Params.CustomSwitches.NotNullOrEmptyS Then
                sb.Append(" ").Append(Params.CustomSwitches)
            End If
        ElseIf ExtractDTSCore Then
            sb.Append(" -core")
        End If

        If includePaths Then
            sb.Append(" -progressnumbers")
        End If

        Return sb.ToString
    End Function

    Function GetfdkaacCommandLine(includePaths As Boolean) As String
        Dim sb As New StringBuilder(128)
        includePaths = includePaths And File.NotNullOrEmptyS

        If DecodingMode = AudioDecodingMode.Pipe Then
            sb.Append(GetPipeCommandLine(includePaths))
        End If

        If includePaths Then
            sb.Append(Package.fdkaac.Path.Escape)
        Else
            'sb.Clear()
            sb.Append("fdkaac")
        End If

        If Params.fdkaacProfile <> 2 Then
            sb.Append(" --profile ").Append(Params.fdkaacProfile)
        End If

        If Params.SimpleRateMode = SimpleAudioRateMode.CBR Then
            sb.Append(" --bitrate ").Append(CInt(Bitrate))
        Else
            sb.Append(" --bitrate-mode ").Append(Params.Quality)
        End If

        'If Params.fdkaacGaplessMode <> 0 Then sb.Append(" --gapless-mode " ).Append( Params.fdkaacGaplessMode)
        If Params.fdkaacBandwidth <> 0 Then sb.Append(" --bandwidth ").Append(Params.fdkaacBandwidth)
        If Not Params.fdkaacAfterburner Then sb.Append(" --afterburner 0")
        If Params.fdkaacAdtsCrcCheck Then sb.Append(" --adts-crc-check")
        If Params.fdkaacMoovBeforeMdat Then sb.Append(" --moov-before-mdat")
        'If Params.fdkaacIncludeSbrDelay Then sb.Append(" --include-sbr-delay")
        'If Params.fdkaacHeaderPeriod Then sb.Append(" --header-period")
        'If Params.fdkaacLowDelaySBR <> 0 Then sb.Append(" --lowdelay-sbr " ).Append( Params.fdkaacLowDelaySBR)
        If Params.fdkaacSbrRatio <> 0 Then sb.Append(" --sbr-ratio ").Append(Params.fdkaacSbrRatio)
        If Params.fdkaacTransportFormat <> 0 Then sb.Append(" --transport-format ").Append(Params.fdkaacTransportFormat)
        If Params.CustomSwitches.NotNullOrEmptyS Then sb.Append(" ").Append(Params.CustomSwitches)

        Dim input = If(DecodingMode = AudioDecodingMode.Pipe, "-", File.LongPathPrefix.Escape)

        If includePaths Then
            sb.Append(" --ignorelength -o ").Append(GetOutputFile.LongPathPrefix.Escape).Append(" ").Append(input)
        End If

        Return sb.ToString
    End Function

    Function GetQaacCommandLine(includePaths As Boolean) As String
        Dim sb As New StringBuilder(128)
        includePaths = includePaths AndAlso File.NotNullOrEmptyS

        If DecodingMode = AudioDecodingMode.Pipe Then
            sb.Append(GetPipeCommandLine(includePaths))
        End If

        If includePaths Then
            sb.Append(Package.qaac.Path.Escape)
        Else
            'sb.Clear()
            sb.Append("qaac")
        End If

        Select Case Params.qaacRateMode
            Case 0
                sb.Append(" --tvbr ").Append(CInt(Params.Quality))
            Case 1
                sb.Append(" --cvbr ").Append(CInt(Bitrate))
            Case 2
                sb.Append(" --abr ").Append(CInt(Bitrate))
            Case 3
                sb.Append(" --cbr ").Append(CInt(Bitrate))
        End Select

        If Params.qaacHE AndAlso {1, 2, 3}.Contains(Params.qaacRateMode) Then
            sb.Append(" --he")
        End If

        If Delay <> 0 Then
            sb.Append(" --delay ").Append((Delay / 1000).ToInvariantString)
        End If

        'If Params.qaacQuality <> 2 Then
        '    sb.Append(" --quality " ).Append( Params.qaacQuality)
        'End If

        If Params.SamplingRate <> 0 Then
            sb.Append(" --rate ").Append(Params.SamplingRate)
        ElseIf Params.Normalize AndAlso Params.ffmpegNormalizeMode = ffmpegNormalizeMode.loudnorm Then 'Loudnorm auto x4 upsample
            sb.Append(" --rate ").Append(SourceSamplingRate)
        End If


        If Params.Normalize AndAlso DecodingMode <> AudioDecodingMode.Pipe Then
            sb.Append(" --normalize") 'Generates huge PCM temp file
        End If

        If Gain <> 0 Then
            sb.Append(" --gain ").Append(Gain.ToInvariantString)
        End If


        Select Case DecodingMode 'is this needed?
            Case AudioDecodingMode.Pipe, AudioDecodingMode.WAVE
                sb.Append(" --ignorelength")
        End Select

        If Params.qaacLowpass <> 0 Then
            sb.Append(" --lowpass ").Append(Params.qaacLowpass)
        End If

        If Params.qaacNoDither Then
            sb.Append(" --limiter")
        End If

        If Params.CustomSwitches.NotNullOrEmptyS Then
            sb.Append(" ").Append(Params.CustomSwitches)
        End If

        '   sb.Append(" -n") force low priority

        Dim input = If(DecodingMode = AudioDecodingMode.Pipe, "-", File.Escape)

        If includePaths Then
            If s.FfmpegLogLevel <> 0 AndAlso s.FfmpegLogLevel < 16 Then
                sb.Append(" -s")
            ElseIf s.FfmpegLogLevel >= 24 OrElse s.FfmpegLogLevel = 0 Then
                sb.Append(" --verbose")
            End If
            sb.Append(" ").Append(input).Append(" -o ").Append(GetOutputFile.Escape)
        End If

        Return sb.ToString
    End Function

    Function GetPipeCommandLine(includePaths As Boolean) As String
        Dim sb As New StringBuilder(128)

        If includePaths AndAlso File.NotNullOrEmptyS Then
            sb.Append(Package.ffmpeg.Path.Escape)

            If Params.ProbeSize <> 5 Then
                sb.Append(" -probesize ").Append(Params.ProbeSize).Append("M")
            End If

            If Params.AnalyzeDuration <> 5 Then
                sb.Append(" -analyzeduration ").Append(Params.AnalyzeDuration).Append("M")
            End If

            sb.Append(" -sn -vn -dn -i ").Append(File.LongPathPrefix.Escape)
        Else
            sb.Append("ffmpeg")
        End If

        If Stream IsNot Nothing AndAlso Streams.Count > 1 Then
            sb.Append(" -sn -vn -dn -map 0:a:").Append(Stream.Index)
        End If

        If Params.ffmpegDither <> FFDither.Disabled Then
            sb.Append(" -dither_method ").Append([Enum](Of FFDither).ToString(Params.ffmpegDither))
        End If
        If Params.ffmpegResampSOX Then
            sb.Append(" -resampler soxr -precision 28")
        End If

        If Params.ChannelsMode <> ChannelsMode.Original Then
            sb.Append(" -rematrix_maxval 1 -ac ").Append(Channels)
            If Params.ffmpegLFEMixLevel <> 0 AndAlso Params.ChannelsMode < 3 Then
                sb.Append(" -lfe_mix_level ").Append(Params.ffmpegLFEMixLevel.ToInvariantString)
            End If
        End If

        If Params.Normalize Then
            Select Case Params.ffmpegNormalizeMode
                Case ffmpegNormalizeMode.dynaudnorm
                    sb.Append(" ").Append(Audio.GetDynAudNormArgs(Params))
                    If Gain <> 0 AndAlso Not SupportsGainSampR() Then
                        sb.Append(",volume=").Append(Gain.ToInvariantString).Append("dB")
                    End If
                Case ffmpegNormalizeMode.loudnorm
                    sb.Append(" ").Append(Audio.GetLoudNormArgs(Params))
                    If Gain <> 0 AndAlso Not SupportsGainSampR() Then
                        sb.Append(",volume=").Append(Gain.ToInvariantString).Append("dB")
                    End If
                    If Params.SamplingRate = 0 AndAlso Not SupportsGainSampR() Then     'Loudnorm auto x4 upsample
                        sb.Append(" -ar ").Append(SourceSamplingRate)
                    End If
                Case ffmpegNormalizeMode.peak
                    If GainWasNormalized And Not SupportsGainSampR() Then
                        sb.Append(" -af volume=").Append(Gain.ToInvariantString).Append("dB")
                    End If
            End Select
        ElseIf Gain <> 0 AndAlso Not SupportsGainSampR() Then
            sb.Append(" -af volume=").Append(Gain.ToInvariantString).Append("dB")
        End If

        If Params.SamplingRate <> 0 AndAlso Not SupportsGainSampR() Then
            sb.Append(" -ar ").Append(Params.SamplingRate)
        End If

        Select Case Depth
            Case 24
                sb.Append(" -c:a pcm_s24le")
            Case 16
                sb.Append(" -c:a pcm_s16le")
            Case Else
                sb.Append(" -c:a pcm_f32le")
        End Select

        If includePaths AndAlso File.NotNullOrEmptyS Then
            If GetEncoder() = GuiAudioEncoder.WavPack Then
                sb.Append(s.GetFFLogLevel(FfLogLevel.info)) ' Progress % workaround :(
            Else
                sb.Append(s.GetFFLogLevel(FfLogLevel.warning))
            End If
            sb.Append(" -hide_banner -f wav - | ")
        End If

        Return sb.ToString
    End Function

    Function GetffmpegCommandLine(includePaths As Boolean) As String
        Dim sb As New StringBuilder(128)
        Dim pack = If(Params.Codec = AudioCodec.AAC AndAlso Params.ffmpegLibFdkAAC,
            Package.ffmpeg_non_free, Package.ffmpeg)

        If includePaths AndAlso File.NotNullOrEmptyS Then
            sb.Append(pack.Path.Escape)

            If Params.ProbeSize <> 5 Then
                sb.Append(" -probesize ").Append(Params.ProbeSize).Append("M")
            End If

            If Params.AnalyzeDuration <> 5 Then
                sb.Append(" -analyzeduration ").Append(Params.AnalyzeDuration).Append("M")
            End If

            sb.Append(" -sn -vn -dn -i ").Append(File.LongPathPrefix.Escape)
        Else
            sb.Append("ffmpeg")
        End If

        If Stream IsNot Nothing AndAlso Streams.Count > 1 Then
            sb.Append(" -sn -vn -dn -map 0:a:").Append(Stream.Index)
        End If

        Select Case Params.Codec
            Case AudioCodec.MP3
                If Not Params.CustomSwitches.Contains("-c:a ") Then
                    sb.Append(" -c:a libmp3lame")
                End If

                Select Case Params.RateMode
                    Case AudioRateMode.ABR
                        sb.Append(" -b:a ").Append(CInt(Bitrate)).Append("k -abr 1")
                    Case AudioRateMode.CBR
                        sb.Append(" -b:a ").Append(CInt(Bitrate)).Append("k")
                    Case AudioRateMode.VBR
                        sb.Append(" -q:a ").Append(CInt(Params.Quality))
                End Select

                If Params.ffmpegMp3Cutoff <> 0 Then
                    sb.Append(" -cutoff ").Append(Params.ffmpegMp3Cutoff)
                End If
            Case AudioCodec.AC3
                If Not Params.CustomSwitches.Contains("-c:a ") Then
                    sb.Append(" -c:a ac3")
                End If

                If Not {192, 224, 384, 448, 640}.Contains(CInt(Bitrate)) Then
                    Return "Invalid bitrate, select 192, 224, 384, 448 or 640"
                End If

                sb.Append(" -b:a ").Append(CInt(Bitrate)).Append("k")
            Case AudioCodec.EAC3
                If Not Params.CustomSwitches.Contains("-c:a ") Then
                    sb.Append(" -c:a eac3")
                End If

                sb.Append(" -b:a ").Append(CInt(Bitrate)).Append("k")
            Case AudioCodec.DTS
                If ExtractDTSCore Then
                    sb.Append(" -bsf:a dca_core -c:a copy")
                Else
                    sb.Append(" -strict -2 -b:a ").Append(CInt(Bitrate)).Append("k")
                End If
            Case AudioCodec.Vorbis
                If Not Params.CustomSwitches.Contains("-c:a ") Then
                    sb.Append(" -c:a libvorbis")
                End If

                If Params.RateMode = AudioRateMode.VBR Then
                    sb.Append(" -q:a ").Append(CInt(Params.Quality))
                Else
                    sb.Append(" -b:a ").Append(CInt(Bitrate)).Append("k")
                End If

                If Params.ffmpegMp3Cutoff <> 0 Then
                    sb.Append(" -cutoff ").Append(Params.ffmpegMp3Cutoff)
                End If
            Case AudioCodec.Opus
                If Not Params.CustomSwitches.Contains("-c:a ") Then
                    sb.Append(" -c:a libopus")
                End If

                Select Case Params.ffmpegOpusRateMode
                    Case OpusRateMode.CBR
                        sb.Append(" -vbr off")
                    Case OpusRateMode.CVBR
                        sb.Append(" -vbr constrained")
                End Select

                sb.Append(" -b:a ").Append(CInt(Bitrate)).Append("k")

                'Select Case Params.ffmpegOpusApp
                '    Case OpusApp.voip
                '        sb.Append(" -application voip")
                '    Case OpusApp.lowdelay
                '        sb.Append(" -application lowdelay")
                'End Select

                If Params.ffmpegOpusFrame <> 20 Then
                    sb.Append(" -frame_duration ").Append(Params.ffmpegOpusFrame.ToInvariantString)
                End If

                'Some reads: https://hydrogenaud.io/index.php?topic=117698.0, https://trac.ffmpeg.org/ticket/5718 , https://trac.ffmpeg.org/ticket/5759
                'default mapping_family=-1 - lost 20% of compression for 5.1
                If Params.ffmpegOpusMap <> -1 Then
                    sb.Append(" -mapping_family ").Append(CInt(Params.ffmpegOpusMap))
                End If
                'If Params.ffmpegOpusCompress <> 10 Then
                '    sb.Append(" -compression_level " ).Append( CInt(Params.ffmpegOpusCompress))
                'End If
                'If Params.ffmpegOpusPacket <> 0 Then
                '    sb.Append(" -packet_loss " ).Append( CInt(Params.ffmpegOpusPacket))
                'End If
                If Params.opusEncNoPhaseInv Then
                    sb.Append(" -apply_phase_inv 0")
                End If
            Case AudioCodec.AAC
                If Params.ffmpegLibFdkAAC Then
                    sb.Append(" -c:a libfdk_aac")

                    If Params.RateMode = SimpleAudioRateMode.CBR Then
                        sb.Append(" -b:a ").Append(CInt(Bitrate)).Append("k")
                    Else
                        sb.Append(" -vbr ").Append(CInt(Params.Quality))
                    End If
                Else
                    sb.Append(" -c:a aac")

                    If Params.RateMode = SimpleAudioRateMode.CBR Then
                        sb.Append(" -b:a ").Append(CInt(Bitrate)).Append("k")
                    Else
                        sb.Append(" -q:a ").Append(CInt(Params.Quality))
                    End If
                End If

            Case AudioCodec.W64, AudioCodec.WAV
                Select Case Depth
                    Case 24
                        sb.Append(" -c:a pcm_s24le")
                    Case 32
                        sb.Append(" -c:a pcm_f32le")
                    Case 16
                        sb.Append(" -c:a pcm_s16le")
                End Select

            Case AudioCodec.FLAC
                If Params.ffmpegCompressionLevel <> 5 Then
                    sb.Append(" -compression_level ").Append(Params.ffmpegCompressionLevel)
                End If
                If Depth = 16 Then
                    sb.Append(" -sample_fmt s16")
                ElseIf Depth >= 24 Then
                    sb.Append(" -sample_fmt s32")
                End If

            Case AudioCodec.WavPack

                If Not Params.CustomSwitches.Contains("-c:a ") Then 'not really needed
                    sb.Append(" -c:a wavpack")
                End If

                If Params.ffmpegCompressionLevel <> 0 Then
                    sb.Append(" -compression_level ").Append(Params.ffmpegCompressionLevel)
                End If
                Select Case Depth
                    Case 24
                        sb.Append(" -sample_fmt s32p")
                    Case 32
                        sb.Append(" -sample_fmt fltp")
                    Case 16
                        sb.Append(" -sample_fmt s16p")
                End Select
        End Select

        If Params.ffmpegDither <> FFDither.Disabled Then
            sb.Append(" -dither_method ").Append([Enum](Of FFDither).ToString(Params.ffmpegDither))
        End If
        If Params.ffmpegResampSOX Then
            sb.Append(" -resampler soxr -precision 28")
        End If

        If Not ExtractCore Then
            If Params.Normalize Then
                Select Case Params.ffmpegNormalizeMode
                    Case ffmpegNormalizeMode.dynaudnorm
                        sb.Append(" ").Append(Audio.GetDynAudNormArgs(Params))
                        If Gain <> 0 Then
                            sb.Append(",volume=").Append(Gain.ToInvariantString).Append("dB")
                        End If
                    Case ffmpegNormalizeMode.loudnorm
                        sb.Append(" ").Append(Audio.GetLoudNormArgs(Params))
                        If Gain <> 0 Then
                            sb.Append(",volume=").Append(Gain.ToInvariantString).Append("dB")
                        End If
                        If Params.SamplingRate = 0 Then
                            sb.Append(" -ar ").Append(SourceSamplingRate)     'Loudnorm auto x4 upsample
                        End If
                    Case ffmpegNormalizeMode.peak
                        If Gain <> 0 Then
                            sb.Append(" -af volume=").Append(Gain.ToInvariantString).Append("dB")
                        End If
                End Select

            ElseIf Gain <> 0 Then
                sb.Append(" -af volume=").Append(Gain.ToInvariantString).Append("dB")
            End If
        End If

        If Not ExtractCore AndAlso Params.ChannelsMode <> ChannelsMode.Original Then
            sb.Append(" -rematrix_maxval 1 -ac ").Append(Channels)
            If Params.ffmpegLFEMixLevel <> 0 AndAlso Params.ChannelsMode < 3 AndAlso Params.ChannelsMode <> ChannelsMode.Original Then
                sb.Append(" -lfe_mix_level ").Append(Params.ffmpegLFEMixLevel.ToInvariantString)
            End If
        End If

        If Not ExtractCore Then
            If Params.SamplingRate <> 0 Then
                sb.Append(" -ar ").Append(Params.SamplingRate)
            End If
        End If

        If Params.CustomSwitches.NotNullOrEmptyS Then
            sb.Append(" ").Append(Params.CustomSwitches)
        End If

        If includePaths AndAlso File.NotNullOrEmptyS Then
            sb.Append(" -y -hide_banner").Append(s.GetFFLogLevel(FfLogLevel.info))
            sb.Append(" ").Append(GetOutputFile.LongPathPrefix.Escape)
        End If

        Return sb.ToString
    End Function

    Function GetWavPackCommandLine(includePaths As Boolean) As String
        Dim sb As New StringBuilder(128)
        includePaths = includePaths AndAlso File.NotNullOrEmptyS

        If DecodingMode = AudioDecodingMode.Pipe Then
            sb.Append(GetPipeCommandLine(includePaths))
        End If

        If includePaths Then
            sb.Append(Package.WavPack.Path.Escape)
        Else
            'sb.Clear()
            sb.Append("wavpack")
        End If

        Select Case Params.WavPackCompression
            Case 0
                sb.Append(" -f")
            Case 2
                sb.Append(" -h")
            Case 3
                sb.Append(" -hh")
        End Select

        If Params.WavPackExtraCompression > 0 Then
            sb.Append(" -x").Append(Params.WavPackExtraCompression)
        End If

        If Params.WavPackMode = 1 Then
            sb.Append(" -b").Append(CInt(Bitrate))
            If Params.WavPackCreateCorrection Then
                sb.Append(" -c")
            End If
        End If

        If Params.WavPackPreQuant > 0 Then
            sb.Append(" --pre-quantize=").Append(Params.WavPackPreQuant)
        End If

        If Params.CustomSwitches.NotNullOrEmptyS Then
            sb.Append(" ").Append(Params.CustomSwitches)
        End If

        Dim input = If(DecodingMode = AudioDecodingMode.Pipe, "-", File.Escape)

        If includePaths Then
            If s.FfmpegLogLevel <> 0 AndAlso s.FfmpegLogLevel < 16 Then
                sb.Append(" -q")
            End If
            Select Case DecodingMode
                Case AudioDecodingMode.Pipe, AudioDecodingMode.WAVE
                    sb.Append(" -i") 'ffmpeg wav pipe mandatory
            End Select
            ' -l WVPenc force low priority, is needed, -z0 cmd title?
            sb.Append(" -w encoder -w settings -m -y ").Append(input).Append(" ").Append(GetOutputFile.Escape)
        End If

        Return sb.ToString
    End Function

    Function GetOpusEncCommandLine(includePaths As Boolean) As String
        Dim sb As New StringBuilder(128)
        includePaths = includePaths AndAlso File.NotNullOrEmptyS

        If DecodingMode = AudioDecodingMode.Pipe Then
            sb.Append(GetPipeCommandLine(includePaths))
        End If

        If includePaths Then
            sb.Append(Package.OpusEnc.Path.Escape)
        Else
            'sb.Clear()
            sb.Append("opusenc")
        End If

        Select Case Params.ffmpegOpusRateMode
            Case OpusRateMode.VBR
                sb.Append(" --vbr --bitrate ").Append(CInt(Bitrate))
            Case OpusRateMode.CVBR
                sb.Append(" --cvbr --bitrate ").Append(CInt(Bitrate))
            Case OpusRateMode.CBR
                sb.Append(" --hard-cbr --bitrate ").Append(CInt(Bitrate))
        End Select

        'Select Case Params.opusEncTuning
        '    Case OpusEncTune.music
        '        sb.Append(" --music")
        '    Case OpusEncTune.speech
        '        sb.Append(" --speech")
        '    Case OpusEncTune.both
        '        sb.Append(" --music --speech")
        '        Exit Select
        'End Select

        'If Params.ffmpegOpusCompress < 10 Then
        '    sb.Append(" --comp " ).Append( Params.ffmpegOpusCompress)
        'End If

        If Params.ffmpegOpusFrame <> 20 Then
            sb.Append(" --framesize ").Append(Params.ffmpegOpusFrame.ToInvariantString)
        End If

        'If Params.ffmpegOpusPacket <> 0 Then
        '    sb.Append(" --expect-loss " ).Append( CInt(Params.ffmpegOpusPacket))
        'End If

        'If Params.opusEncDelay < 1000 Then
        '    sb.Append(" --max-delay " ).Append( Params.opusEncDelay)
        'End If

        If Params.opusEncNoPhaseInv Then
            sb.Append(" --no-phase-inv")
        End If

        Select Case DecodingMode 'is this needed?
            Case AudioDecodingMode.Pipe, AudioDecodingMode.WAVE
                sb.Append(" --ignorelength")
        End Select

        If Params.CustomSwitches.NotNullOrEmptyS Then
            sb.Append(" ").Append(Params.CustomSwitches)
        End If

        'sb.Append(" --serial 4321 --save-range """ ).Append( GetOutputFile.Dir.Escape.TrimEnd(""""c) ).Append( "Opus.txt"" ")

        Dim input = If(DecodingMode = AudioDecodingMode.Pipe, "-", File.Escape)
        If includePaths Then
            If s.FfmpegLogLevel <> 0 AndAlso s.FfmpegLogLevel < 16 Then
                sb.Append(" --quiet")
            End If
            sb.Append(" ").Append(input).Append(" ").Append(GetOutputFile.Escape)
        End If

        Return sb.ToString
    End Function

    Function SupportsNormalize() As Boolean
        Return GetEncoder() = GuiAudioEncoder.eac3to
    End Function

    Function SupportsGainSampR() As Boolean
        Return GetEncoder() = GuiAudioEncoder.qaac OrElse GetEncoder() = GuiAudioEncoder.eac3to
    End Function
    Function IntegerCodec() As Boolean
        Select Case Params.Codec
            Case AudioCodec.FLAC, AudioCodec.WavPack, AudioCodec.WAV, AudioCodec.W64
                Return True
            Case AudioCodec.AAC
                If Params.Encoder = GuiAudioEncoder.fdkaac Then Return True
        End Select
        Return False
    End Function

    Function VBRMode() As Boolean
        Select Case Params.Codec
            Case AudioCodec.AAC
                Select Case Params.Encoder
                    Case GuiAudioEncoder.ffmpeg, GuiAudioEncoder.fdkaac
                        If Params.SimpleRateMode = SimpleAudioRateMode.VBR Then Return True Else Return False
                    Case GuiAudioEncoder.eac3to
                        Return True
                    Case Else
                        If Params.qaacRateMode = 0 Then Return True
                End Select
            Case AudioCodec.Opus
                If Params.ffmpegOpusRateMode = OpusRateMode.VBR Then Return True
            Case AudioCodec.FLAC
                Return True
            Case AudioCodec.WavPack
                If Params.WavPackMode = 0 Then Return True
            Case AudioCodec.MP3, AudioCodec.Vorbis
                If Params.RateMode = AudioRateMode.VBR Then Return True Else Return False
        End Select
        Return False
    End Function

    <NonSerialized()>
    Public DefaultnameValue As String
    Public Overrides ReadOnly Property DefaultName As String
        Get
            If Params Is Nothing Then
                Exit Property
            End If
            If DefaultnameValue IsNot Nothing AndAlso AudioConverterForm.AudioConverterMode Then Return DefaultnameValue

            Dim sb As New StringBuilder(32)
            sb.Append([Enum](Of AudioCodec).ToString(Params.Codec))

            Select Case Params.ChannelsMode
                Case ChannelsMode._8
                    sb.Append(" 7.1")
                Case ChannelsMode._7
                    sb.Append(" 6.1")
                Case ChannelsMode._6
                    sb.Append(" 5.1")
                Case ChannelsMode._2
                    sb.Append(" 2.0")
                Case ChannelsMode._1
                    sb.Append(" Mono")
            End Select

            sb.Append(" ")

            If Depth <> 0 Then
                sb.Append(Depth.ToInvariantString).Append("b ")
            End If

            If Params.SamplingRate <> 0 Then
                sb.Append((Params.SamplingRate \ 1000).ToInvariantString).Append("kHz ") ' Or  / 1000 "kHz "
            End If

            If VBRMode() Then sb.Append("~")
            'Else   sb.Append(CInt(Bitrate).ToInvariantString).Append("Kbps") ' or fix
            sb.Append(GetBitrate().ToInvariantString).Append("Kbps")

            DefaultnameValue = If(ExtractDTSCore AndAlso ExtractCore, "Extract DTS Core", sb.ToString)
            Return DefaultnameValue

        End Get
    End Property

    ReadOnly Property ExtractCore As Boolean
        Get
            If ExtractDTSCore AndAlso Params.Codec = AudioCodec.DTS Then
                If GetEncoder() = GuiAudioEncoder.ffmpeg OrElse GetEncoder() = GuiAudioEncoder.eac3to Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property

    <NonSerialized()>
    Private _CommandLines As String
    Overrides Property CommandLines() As String
        Get
            Return GetCommandLine(True)
        End Get
        Set(Value As String)
            _CommandLines = Value
        End Set
    End Property

    Overrides ReadOnly Property CanEdit() As Boolean
        Get
            Return True
        End Get
    End Property

    Overrides Function HandlesDelay() As Boolean
        If {GuiAudioEncoder.qaac, GuiAudioEncoder.eac3to}.Contains(GetEncoder()) Then
            Return True
        End If
    End Function

    Function GetEncoder() As GuiAudioEncoder
        Select Case Params.Encoder
            Case GuiAudioEncoder.ffmpeg
                Return GuiAudioEncoder.ffmpeg
            Case GuiAudioEncoder.eac3to
                Select Case Params.Codec
                    Case AudioCodec.AAC, AudioCodec.FLAC, AudioCodec.AC3, AudioCodec.DTS, AudioCodec.W64, AudioCodec.WAV
                        Return GuiAudioEncoder.eac3to
                End Select
            Case GuiAudioEncoder.fdkaac
                If Params.Codec = AudioCodec.AAC Then
                    Return GuiAudioEncoder.fdkaac
                End If
        End Select

        'dedicated encoders priority:
        Select Case Params.Codec
            Case AudioCodec.AAC
                Return GuiAudioEncoder.qaac
            Case AudioCodec.Opus
                Return GuiAudioEncoder.OpusEnc
            Case AudioCodec.WavPack
                Return GuiAudioEncoder.WavPack
        End Select

        Return GuiAudioEncoder.ffmpeg
    End Function

    Function GetCommandLine(includePaths As Boolean) As String
        Select Case GetEncoder()
            Case GuiAudioEncoder.qaac
                Return GetQaacCommandLine(includePaths)
            Case GuiAudioEncoder.OpusEnc
                Return GetOpusEncCommandLine(includePaths)
            Case GuiAudioEncoder.WavPack
                Return GetWavPackCommandLine(includePaths)
            Case GuiAudioEncoder.fdkaac
                Return GetfdkaacCommandLine(includePaths)
            Case GuiAudioEncoder.eac3to
                Return GetEac3toCommandLine(includePaths)
            Case Else
                Return GetffmpegCommandLine(includePaths)
        End Select
    End Function

    <NonSerialized()>
    Private _SupportedInput As String()
    Overrides Property SupportedInput As String()
        Get
            Select Case GetEncoder()
                Case GuiAudioEncoder.qaac
                    If DecodingMode <> AudioDecodingMode.Pipe Then
                        If p.Ranges.Count > 0 Then
                            Return {"wv", "wav"}
                        Else
                            Return {"wv", "wav", "flac"}
                        End If
                    End If
                Case GuiAudioEncoder.WavPack
                    If DecodingMode <> AudioDecodingMode.Pipe Then
                        Return {"wv", "wav"}
                    End If
                Case GuiAudioEncoder.OpusEnc
                    If DecodingMode <> AudioDecodingMode.Pipe Then
                        If p.Ranges.Count > 0 Then
                            Return {"wav"}
                        Else
                            Return {"wav", "flac"}
                        End If
                    End If
                Case GuiAudioEncoder.fdkaac
                    If DecodingMode <> AudioDecodingMode.Pipe Then
                        Return {"wav"}
                    End If
                Case GuiAudioEncoder.eac3to
                    Return FileTypes.eac3toInput
            End Select

            Return {}
        End Get
        Set(value As String())
            _SupportedInput = value
        End Set
    End Property

    <Serializable()>
    Public Class Parameters
        Property AnalyzeDuration As Integer = 5
        Property ChannelsMode As ChannelsMode
        Property Codec As AudioCodec
        Property CustomSwitches As String = ""
        Property eac3toStereoDownmixMode As Integer
        Property Encoder As GuiAudioEncoder
        Property FrameRateMode As AudioFrameRateMode
        Property Normalize As Boolean = True
        Property ProbeSize As Integer = 5
        Property Quality As Single = 0.3
        Property RateMode As AudioRateMode = AudioRateMode.VBR
        Property SamplingRate As Integer

        ' Property Migrate1 As Boolean = True
        'Property MigrateffNormalizeMode As Boolean = True

        Property qaacHE As Boolean
        Property qaacLowpass As Integer
        Property qaacNoDither As Boolean
        'Property qaacQuality As Integer = 2
        Property qaacRateMode As Integer

        Property fdkaacProfile As Integer = 2
        Property fdkaacBandwidth As Integer
        Property fdkaacAfterburner As Boolean = True
        'Property fdkaacLowDelaySBR As Integer
        Property fdkaacSbrRatio As Integer
        Property fdkaacTransportFormat As Integer
        'Property fdkaacGaplessMode As Integer
        Property fdkaacAdtsCrcCheck As Boolean
        'Property fdkaacHeaderPeriod As Boolean
        'Property fdkaacIncludeSbrDelay As Boolean
        Property fdkaacMoovBeforeMdat As Boolean

        '  Property ffNormalizeMode As ffNormalizeMode
        Property ffmpegNormalizeMode As ffmpegNormalizeMode

        Property ffmpegLibFdkAAC As Boolean

        Property ffmpegLoudnormIntegrated As Double = -24
        Property ffmpegLoudnormIntegratedMeasured As Double
        Property ffmpegLoudnormLRA As Double = 7
        Property ffmpegLoudnormLraMeasured As Double
        Property ffmpegLoudnormThresholdMeasured As Double
        Property ffmpegLoudnormTruePeak As Double = -2
        Property ffmpegLoudnormTruePeakMeasured As Double
        Property ffmpegLoudnormOffset As Double = 0

        Property ffmpegDynaudnormF As Integer = 500
        Property ffmpegDynaudnormG As Integer = 31
        Property ffmpegDynaudnormP As Double = 0.95
        Property ffmpegDynaudnormM As Double = 10
        Property ffmpegDynaudnormR As Double
        Property ffmpegDynaudnormN As Boolean = True
        Property ffmpegDynaudnormC As Boolean
        Property ffmpegDynaudnormB As Boolean
        Property ffmpegDynaudnormS As Double

        'Property ffmpegOpusApp As OpusApp = OpusApp.audio
        'Property ffmpegOpusCompress As Integer = 10
        Property ffmpegOpusFrame As Double = 20
        Property ffmpegOpusMap As Integer = -1
        'Property ffmpegOpusPacket As Integer
        Property ffmpegOpusRateMode As OpusRateMode = OpusRateMode.VBR
        ' Property ffmpegOpusMigrate As Integer = 1
        Property ffmpegMp3Cutoff As Integer

        'Property opusEncMode As OpusEncMode
        'Property opusEncComplexity As Integer = 10
        'Property opusEncFramesize As Double = 20
        'Property opusEncTuning As OpusEncTune
        'Property opusEncDelay As Integer = 1000
        Property opusEncNoPhaseInv As Boolean = False

        Property ffmpegCompressionLevel As Integer = 1
        Property ffmpegLFEMixLevel As Double = 0R
        Property ffmpegResampSOX As Boolean
        Property ffmpegDither As FFDither

        Property WavPackCompression As Integer = 1
        Property WavPackExtraCompression As Integer = 0
        Property WavPackPreQuant As Integer
        Property WavPackMode As Integer = 0 'lossless
        Property WavPackCreateCorrection As Boolean

        Property SimpleRateMode As SimpleAudioRateMode
        'Get
        'If RateMode = AudioRateMode.CBR Then
        'Return SimpleAudioRateMode.CBR
        'Else
        'Return SimpleAudioRateMode.VBR
        'End If
        'End Get
        '   Set(value As SimpleAudioRateMode)
        'If Codec = AudioCodec.AAC Then
        'Select Case Encoder
        'Case GuiAudioEncoder.ffmpeg, GuiAudioEncoder.fdkaac
        'If value = SimpleAudioRateMode.CBR Then
        '                       RateMode = AudioRateMode.CBR
        'Else
        '                       RateMode = AudioRateMode.VBR
        'End If
        'End Select
        'End If
        'End Set
        'End Property

        'legacy/obsolete
        Sub Migrate()
            '2019
            'If fdkaacProfile = 0 Then
            '    fdkaacProfile = 2
            '    SimpleRateMode = SimpleAudioRateMode.VBR
            '    fdkaacAfterburner = True
            'End If

            '2019
            'If Not Migrate1 Then
            '    Normalize = True

            '    ffmpegLoudnormIntegrated = -24
            '    ffmpegLoudnormLRA = 7
            '    ffmpegLoudnormTruePeak = -2

            '    ffmpegDynaudnormF = 500
            '    ffmpegDynaudnormG = 31
            '    ffmpegDynaudnormP = 0.95
            '    ffmpegDynaudnormM = 10
            '    ffmpegDynaudnormN = True

            '    Migrate1 = True
            'End If

            '2020
            'If Not MigrateffNormalizeMode Then
            '    ffmpegNormalizeMode = CType(ffNormalizeMode, ffmpegNormalizeMode)
            '    MigrateffNormalizeMode = True
            'End If

            '2020
            'If ffmpegOpusMigrate <> 1 Then
            '    ffmpegOpusMigrate = 1
            '    'ffmpegOpusApp = OpusApp.audio
            '    'ffmpegOpusCompress = 10
            '    ffmpegOpusFrame = 20
            '    ffmpegOpusMap = -1
            '    ffmpegOpusRateMode = OpusRateMode.VBR
            '    'opusEncFramesize = 20
            '    'opusEncComplexity = 10
            '    'opusEncMode = OpusEncMode.VBR
            '    'opusEncNoPhaseInv = False
            '    'opusEncDelay = 1000
            ' If ProbeSize = 0 AndAlso AnalyzeDuration = 0 Then
            '    ProbeSize = 5
            '    AnalyzeDuration = 5
            ' End If

            '    'ffmpegOpusApp = Nothing ' Optional temporaly disabled Options 2021
            '    'ffmpegOpusCompress = Nothing
            '    'ffmpegOpusPacket = Nothing
            '    'fdkaacLowDelaySBR = Nothing
            '    'fdkaacGaplessMode = Nothing
            '    'fdkaacHeaderPeriod = Nothing
            '    'fdkaacIncludeSbrDelay = Nothing
            '    'qaacQuality = Nothing
            '    'opusEncTuning = Nothing
            '    'opusEncDelay = Nothing
            'End If

        End Sub
    End Class
End Class

Public Enum AudioCodec
    AAC
    AC3
    DTS
    FLAC
    MP3
    Opus
    Vorbis
    W64
    WAV
    EAC3
    WavPack
End Enum

Public Enum AudioRateMode
    VBR
    ABR
    CBR
End Enum

Public Enum OpusRateMode
    VBR
    CVBR
    CBR
End Enum

'Public Enum OpusApp
'    audio
'    voip
'    lowdelay
'End Enum

'Public Enum OpusEncTune
'    auto
'    music
'    speech
'    <DispName("music && speech")> both
'End Enum
'Public Enum OpusEncMode
'VBR
'CVBR
'CBR
'End Enum

Public Enum SimpleAudioRateMode
    VBR
    CBR
End Enum

Public Enum AudioAacProfile
    Automatic
    LC
    SBR
    <DispName("SBR+PS")> SBRPS = 300
End Enum

Public Enum GuiAudioEncoder
    Automatic
    eac3to
    ffmpeg
    qaac
    fdkaac
    WavPack
    OpusEnc
End Enum

Public Enum AudioFrameRateMode
    Keep
    <DispName("Apply PAL speedup")> Speedup
    <DispName("Reverse PAL speedup")> Slowdown
End Enum

Public Enum AudioDownMixMode
    <DispName("Simple")> Stereo
    <DispName("Dolby Surround")> Surround
    <DispName("Dolby Surround 2")> Surround2
End Enum

Public Enum ChannelsMode
    Original
    <DispName("1 (Mono)")> _1
    <DispName("2 (Stereo)")> _2
    <DispName("5.1")> _6
    <DispName("6.1")> _7
    <DispName("7.1")> _8
End Enum

Public Enum FFDither
    Disabled
    rectangular
    triangular
    triangular_hp
    lipshitz
    shibata
    low_shibata
    high_shibata
    f_weighted
    modified_e_weighted
    improved_e_weighted
End Enum