
Imports System.Text
Imports System.Text.RegularExpressions

<Serializable()>
Public MustInherit Class Demuxer
    MustOverride Sub Run(proj As Project)

    Overridable Property Active As Boolean = True
    Overridable Property InputExtensions As String() = {}
    Overridable Property InputFormats As String() = {}
    Overridable Property Name As String = ""
    Overridable Property OutputExtensions As String() = {}
    Overridable Property SourceFilters As String() = {}

    Property VideoDemuxing As Boolean
    Property VideoDemuxed As Boolean
    Property ChaptersDemuxing As Boolean = True
    Property OverrideExisting As Boolean

    Overridable ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return False
        End Get
    End Property

    Overridable Function ShowConfigDialog() As DialogResult
        Using form As New SimpleSettingsForm(Name)
            form.ScaleClientSize(23, 11, form.FontHeight)
            Dim ui = form.SimpleUI
            Dim page = ui.CreateFlowPage("main page")
            page.SuspendLayout()

            Dim tb = ui.AddText(page)
            tb.Label.Text = "Supported Input File Types:"
            tb.Edit.Text = InputExtensions.Join(" ")
            tb.Edit.SaveAction = Sub(value) InputExtensions = value.ToLower.SplitNoEmptyAndNoWSDelim(",", ";", " ")

            Dim cb = ui.AddBool(page)
            cb.Text = "Video Demuxing"
            cb.Checked = VideoDemuxing
            cb.SaveAction = Sub(val) VideoDemuxing = val

            cb = ui.AddBool(page)
            cb.Text = "Chapters Demuxing"
            cb.Checked = ChaptersDemuxing
            cb.SaveAction = Sub(val) ChaptersDemuxing = val

            cb = ui.AddBool(page)
            cb.Text = "Recreate already existing files"
            cb.Help = "Demux even when ouput files already exist."
            cb.Checked = OverrideExisting
            cb.SaveAction = Sub(val) OverrideExisting = val
            page.ResumeLayout()

            Dim ret = form.ShowDialog()

            If ret = DialogResult.OK Then
                ui.Save()
            End If

            Return ret
        End Using
    End Function

    Overrides Function ToString() As String
        Dim input = InputExtensions.Join(", ")

        If input.Length > 30 Then
            input = input.Shorten(30) + "..."
        End If

        Return Name + " (" + input + " -> " + OutputExtensions.Join(", ") + ")"
    End Function

    Overridable Function GetHelp() As String
        For Each i In Package.Items.Values
            If Name = i.Name Then
                Return i.Description
            End If
        Next
    End Function

    Shared Function GetDefaults() As List(Of Demuxer)
        Dim ret As New List(Of Demuxer)

        ret.Add(New ffmpegDemuxer)

        Dim tsToMkv As New CommandLineDemuxer
        tsToMkv.Name = "ffmpeg: Re-mux TS to MKV"
        tsToMkv.InputExtensions = {"ts"}
        tsToMkv.OutputExtensions = {"mkv"}
        tsToMkv.InputFormats = {"hevc", "avc"}
        tsToMkv.Command = "%app:ffmpeg%"
        tsToMkv.Arguments = "-sn -i ""%source_file%"" -c copy -ignore_unknown -sn -y -hide_banner ""%temp_file%.mkv"""
        ret.Add(tsToMkv)

        ret.Add(New mkvDemuxer)
        ret.Add(New MP4BoxDemuxer)
        ret.Add(New eac3toDemuxer)

        Dim dgIndex As New CommandLineDemuxer
        dgIndex.Name = "DGIndex: Demux & Index MPEG-2"
        dgIndex.InputExtensions = {"mpg", "vob", "m2ts", "m2v", "mts", "m2t"}
        dgIndex.OutputExtensions = {"d2v"}
        dgIndex.InputFormats = {"mpeg2"}
        dgIndex.Command = "%app:DGIndex%"
        dgIndex.Arguments = "-i %source_files% -ia 2 -fo 0 -yr 1 -tn 1 -om 2 -drc 2 -dsd 0 -dsa 0 -o ""%temp_file%"" -hide -exit"
        dgIndex.SourceFilters = {"D2VSource", "d2v.Source"}
        ret.Add(dgIndex)

        'DgindexNV Back
        Dim dgnvNoDemux As New CommandLineDemuxer
        dgnvNoDemux.Name = "DGIndexNV: Index, No Demux"
        dgnvNoDemux.InputExtensions = {"mkv", "mp4", "h264", "h265", "avc", "hevc", "hvc", "264", "265"}
        dgnvNoDemux.OutputExtensions = {"dgi"}
        dgnvNoDemux.InputFormats = {"hevc", "avc", "vc1", "mpeg2"}
        dgnvNoDemux.Command = "%app:DGIndexNV%"
        dgnvNoDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgi"" -h"
        dgnvNoDemux.SourceFilters = {"DGSource"}
        dgnvNoDemux.Active = False
        ret.Add(dgnvNoDemux)

        Dim dgnvDemux As New CommandLineDemuxer
        dgnvDemux.Name = "DGIndexNV: Demux & Index"
        dgnvDemux.InputExtensions = {"mpg", "vob", "ts", "m2ts", "m2v", "mts", "m2t"}
        dgnvDemux.OutputExtensions = {"dgi"}
        dgnvDemux.InputFormats = {"hevc", "avc", "vc1", "mpeg2"}
        dgnvDemux.Command = "%app:DGIndexNV%"
        dgnvDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgi"" -a -h"
        dgnvDemux.SourceFilters = {"DGSource"}
        dgnvDemux.Active = False
        ret.Add(dgnvDemux)
        'DgindexNV Back

        Dim d2vWitch As New CommandLineDemuxer
        d2vWitch.Name = "D2V Witch: Demux & Index MPEG-2"
        d2vWitch.InputExtensions = {"mpg", "vob", "m2ts", "m2v", "mts", "m2t"}
        d2vWitch.OutputExtensions = {"d2v"}
        d2vWitch.InputFormats = {"mpeg2"}
        d2vWitch.Command = "cmd.exe"
        d2vWitch.Arguments = "/S /C """"%app:D2V Witch%"" --audio-ids all --output ""%temp_file%.d2v"" %source_files%"""
        d2vWitch.SourceFilters = {"D2VSource", "d2v.Source"}
        d2vWitch.Active = False
        ret.Add(d2vWitch)

        Return ret
    End Function
End Class

<Serializable()>
Public Class CommandLineDemuxer
    Inherits Demuxer

    Property Command As String = ""
    Property Arguments As String = ""

    Overrides ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return True
        End Get
    End Property

    Overrides Function ShowConfigDialog() As DialogResult
        Using form As New CommandLineDemuxForm(Me)
            Return form.ShowDialog
        End Using
    End Function

    Overrides Sub Run(proj As Project)
        Using proc As New Proc
            Dim test = Macro.Expand(Command + Arguments).ToLowerEx.Replace(" ", "")

            'DGIndexNV Back Back
            If Command?.Contains("DGIndexNV") OrElse Arguments?.Contains("DGIndexNV") OrElse test.Contains("dgindexnv")  Then
                proc.Package = Package.DGIndexNV
                proc.IntegerPercentOutput = True

            ElseIf test.Contains("dgindex") AndAlso Not test.Contains("dgindexnv") Then
                proc.Package = Package.DGIndex
                proc.IntegerPercentOutput = True
            ElseIf test.Contains("d2vwitch") Then
                proc.Package = Package.D2VWitch
                proc.SkipString = "%"
            Else
                proc.SkipStrings = {"frame=", "size="}
            End If

            proc.Header = Name
            proc.File = Macro.Expand(Command)
            proc.Arguments = Macro.Expand(Arguments)
            proc.Start()

            If Command?.Contains("DGIndex") Then
                FileHelp.Move(p.SourceFile.DirAndBase + ".log", p.TempDir + p.SourceFile.Base + "_dg.log")
                FileHelp.Move(p.TempDir + p.SourceFile.Base + ".demuxed.m2v", p.TempDir + p.SourceFile.Base + ".m2v")
            End If
        End Using
    End Sub

    Shared Function IsActive(value As String) As Boolean
        For Each demuxer In s.Demuxers.OfType(Of CommandLineDemuxer)()
            If demuxer.Active AndAlso (demuxer.Name.Contains(value) OrElse demuxer.Command.Contains(value)) Then
                Return True
            End If
        Next
    End Function
End Class

<Serializable()>
Public Class eac3toDemuxer
    Inherits Demuxer

    Sub New()
        Name = "eac3to: Demux"
        InputExtensions = {"m2ts"}
        OutputExtensions = {"h264", "mkv", "m2v"}
    End Sub

    Overrides Sub Run(proj As Project)
        If proj.NoDialogs OrElse proj.BatchMode Then
            Exit Sub
        End If

        Using form As New eac3toForm(proj)
            form.M2TSFile = proj.SourceFile
            form.teTempDir.Text = proj.TempDir

            If form.ShowDialog() = DialogResult.OK Then
                Using proc As New Proc
                    proc.Project = proj
                    proc.SkipStrings = {"analyze: ", "process: "}
                    proc.TrimChars = {"-"c, " "c}
                    proc.Header = "Demux M2TS"
                    proc.Package = Package.eac3to
                    Dim outFiles As New List(Of String)
                    proc.Arguments = form.GetArgs(proj.SourceFile.Escape, proj.SourceFile.Base, outFiles)
                    proc.OutputFiles = outFiles

                    Try
                        proc.Start()
                    Catch ex As Exception
                        g.ShowException(ex)
                        Throw New AbortException
                    End Try

                    If Not String.Equals(form.cbVideoOutput.Text, "Nothing") Then
                        proj.SourceFile = form.OutputFolder + proj.SourceFile.Base + "." + form.cbVideoOutput.Text.ToLower
                        proj.SourceFiles.Clear()
                        proj.SourceFiles.Add(proj.SourceFile)
                        proj.TempDir = form.OutputFolder
                    End If
                End Using
            Else
                Throw New AbortException
            End If
        End Using
    End Sub

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class

<Serializable>
Public Class ffmpegDemuxer
    Inherits Demuxer

    Sub New()
        Name = "ffmpeg: Demux"
        InputExtensions = {"avi", "flv"}
    End Sub

    Public Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)
        Dim videoDemuxing = Me.VideoDemuxing

        Dim audioDemuxing = Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim subtitlesDemuxing = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((audioDemuxing AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (subtitlesDemuxing AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) OrElse
            Not proj Is p Then

            Using form As New StreamDemuxForm(Me, proj.SourceFile, Nothing)
                form.cbDemuxChapters.Visible = False

                If form.ShowDialog() = DialogResult.OK Then
                    videoDemuxing = form.cbDemuxVideo.Checked
                    audioStreams = form.AudioStreams
                    subtitles = form.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

        If videoDemuxing Then
            DemuxVideo(proj, OverrideExisting)
        End If

        If audioDemuxing AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then
                audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)
            End If

            For Each stream In audioStreams
                If stream.Enabled Then
                    DemuxAudio(proj.SourceFile, stream, Nothing, proj, OverrideExisting)
                End If
            Next
        End If

        Log.Save(proj)
    End Sub

    Shared Sub DemuxVideo(proj As Project, overrideExisting As Boolean)
        Dim streams = MediaInfo.GetVideoStreams(proj.SourceFile)

        If streams.Count = 0 OrElse streams(0).Ext.NullOrEmptyS Then
            Exit Sub
        End If

        Dim outPath = proj.TempDir + proj.SourceFile.Base + streams(0).ExtFull

        If outPath.Equals(proj.SourceFile) OrElse (Not overrideExisting AndAlso outPath.FileExists) Then
            Exit Sub
        End If

        Dim args = "-an -sn -i " + proj.SourceFile.Escape
        args += " -c:v copy -an -sn -y -hide_banner"
        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux video"
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.OutputFiles = {outPath}
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            proj.Log.Write("Error", "no video output found")
        End If
    End Sub

    Shared Sub DemuxAudio(
        sourcefile As String,
        stream As AudioStream,
        ap As AudioProfile,
        proj As Project,
        overrideExisting As Boolean)

        Dim outPath = (proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream) + stream.ExtFull).ToShortFilePath

        If Not overrideExisting AndAlso outPath.FileExists Then
            Exit Sub
        End If

        Dim streamIndex = stream.StreamOrder
        Dim args = "-vn -sn -dn -i " + sourcefile.ToShortFilePath.Escape

        If MediaInfo.GetAudioCount(sourcefile) > 1 Then
            args += " -map 0:a:" & stream.Index
        End If

        args += " -vn -sn -dn -y -hide_banner"

        If outPath.Ext.Equals("wav") Then
            args += " -c:a pcm_f32le"
        Else
            args += " -c:a copy"
        End If

        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux Audio"
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.OutputFiles = {outPath}
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If Not ap Is Nothing Then
                ap.File = outPath
            End If

            proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            proj.Log.Write("Error", "no audio output found")
        End If
    End Sub

    Sub DemuxSubtitles(subtitles As List(Of Subtitle), proj As Project)
        'If subtitles.Where(Function(subtitle) subtitle.Enabled).Count = 0 Then
        If Not subtitles.Exists(Function(subtitle) subtitle.Enabled) Then
            Exit Sub
        End If

        For Each subtitle In subtitles
            If Not subtitle.Enabled Then
                Continue For
            End If

            Dim args = "-vn -an -dn -i " + proj.SourceFile.ToShortFilePath.Escape
            Dim outpath = (proj.TempDir + subtitle.Filename + subtitle.ExtFull).ToShortFilePath

            If MediaInfo.GetSubtitleCount(proj.SourceFile) > 1 Then
                args += " -map 0:s:" & subtitle.Index
            End If

            args += " -c:s copy -vn -an -dn -y -hide_banner " + outpath.Escape

            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux subtitles"
                proc.SkipStrings = {"frame=", "size="}
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.ffmpeg
                proc.Arguments = args
                proc.Start()
            End Using
        Next
    End Sub

    Public Overrides Property OutputExtensions As String()
        Get
            Return FileTypes.VideoDemuxOutput
        End Get
        Set(value As String())
        End Set
    End Property

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class

<Serializable()>
Public Class MP4BoxDemuxer
    Inherits Demuxer

    Sub New()
        Name = "MP4Box: Demux"
        InputExtensions = {"mp4", "m4v", "mov"}
    End Sub

    Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)

        Dim demuxAudio = Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0
        Dim attachments = GetAttachments(proj.SourceFile)
        Dim demuxChapters = ChaptersDemuxing
        VideoDemuxed = VideoDemuxing

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) OrElse
            Not proj Is p Then

            Using form As New StreamDemuxForm(Me, proj.SourceFile, attachments)
                If form.ShowDialog() = DialogResult.OK Then
                    VideoDemuxed = form.cbDemuxVideo.Checked
                    demuxChapters = form.cbDemuxChapters.Checked
                    audioStreams = form.AudioStreams
                    subtitles = form.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

        If VideoDemuxed Then
            DemuxVideo(proj, OverrideExisting)
        End If

        If demuxAudio AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then
                audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)
            End If

            For Each stream In audioStreams
                If stream.Enabled Then
                    MP4BoxDemuxer.DemuxAudio(proj.SourceFile, stream, Nothing, proj, OverrideExisting)
                End If
            Next
        End If

        If demuxSubtitles AndAlso proj.DemuxSubtitles <> DemuxMode.None Then
            If subtitles Is Nothing Then
                subtitles = MediaInfo.GetSubtitles(proj.SourceFile)
            End If

            For Each st In subtitles
                If Not st.Enabled Then
                    Continue For
                End If

                Dim outpath = (proj.TempDir + st.Filename + st.ExtFull).ToShortFilePath

                If Not OverrideExisting AndAlso outpath.FileExists Then
                    Continue For
                End If

                FileHelp.Delete(outpath)
                Dim args As String

                Select Case st.ExtFull
                    Case ""
                        Continue For
                    Case ".srt"
                        args = "-srt "
                    Case Else
                        args = "-raw "
                End Select

                args += st.ID & " -out " + outpath.Escape + " " + proj.SourceFile.ToShortFilePath.Escape

                Using proc As New Proc
                    proc.Project = proj
                    proc.Header = "Demux subtitle"
                    proc.SkipString = "|"
                    proc.Package = Package.MP4Box
                    proc.Arguments = args
                    proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                    proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                    proc.OutputFiles = {outpath}
                    proc.Start()
                End Using
            Next
        End If

        If Not attachments.NothingOrEmpty AndAlso attachments(0).Enabled Then
            Using proc As New Proc
                proc.Project = proj
                proc.SkipString = "|"
                proc.Header = "Extract cover"
                proc.Package = Package.MP4Box
                proc.Arguments = "-dump-cover " + proj.SourceFile.Escape + " -out " + (proj.TempDir + "cover.jpg").Escape
                proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                proc.Start()
            End Using
        End If

        If demuxChapters AndAlso MediaInfo.GetMenu(proj.SourceFile, "Chapters_Pos_End").ToInt -
            MediaInfo.GetMenu(proj.SourceFile, "Chapters_Pos_Begin").ToInt > 0 Then

            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Extract chapters"
                proc.SkipString = "|"
                proc.Package = Package.MP4Box
                proc.Arguments = "-dump-chap-ogg " + proj.SourceFile.ToShortFilePath.Escape +
                    " -out " + (proj.TempDir + proj.SourceFile.Base + "_chapters.txt").ToShortFilePath.Escape
                proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                proc.Start()
            End Using
        End If

        Log.Save(proj)
    End Sub

    Public Overrides Property OutputExtensions As String()
        Get
            If VideoDemuxing OrElse VideoDemuxed Then
                Return {"avi", "mpg", "h264", "h265"}
            End If

            Return {}
        End Get
        Set(value As String())
        End Set
    End Property

    Shared Sub DemuxVideo(proj As Project, overrideExisting As Boolean)
        Dim streams = MediaInfo.GetVideoStreams(proj.SourceFile)

        If streams.Count = 0 OrElse streams(0).Ext.NullOrEmptyS Then
            Exit Sub
        End If

        Dim outpath = proj.TempDir + proj.SourceFile.Base + streams(0).ExtFull

        If outpath.Equals(proj.SourceFile) OrElse (Not overrideExisting AndAlso outpath.FileExists) Then
            Exit Sub
        End If

        Dim args = If(streams(0).Ext.Equals("avi"), "-avi ", "-raw ")
        args += streams(0).ID & " -out " + outpath.Escape + " " + proj.SourceFile.Escape

        FileHelp.Delete(outpath)

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux video"
            proc.SkipString = "|"
            proc.Package = Package.MP4Box
            proc.Arguments = args
            proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
            proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
            proc.OutputFiles = {outpath}
            proc.Start()
        End Using

        If File.Exists(outpath) Then
            proj.Log.WriteLine(MediaInfo.GetSummary(outpath))
        Else
            proj.Log.Write("Error", "no video output found")
            ffmpegDemuxer.DemuxVideo(proj, overrideExisting)
        End If
    End Sub

    Shared Sub DemuxAudio(
        sourcefile As String,
        stream As AudioStream,
        ap As AudioProfile,
        proj As Project,
        overrideExisting As Boolean)

        If String.Equals(MediaInfo.GetAudio(sourcefile, "Format"), "PCM") Then
            ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj, overrideExisting)
            Exit Sub
        End If

        Dim outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream) + stream.ExtFull

        If Not overrideExisting AndAlso outPath.FileExists Then
            Exit Sub
        End If

        FileHelp.Delete(outPath)
        Dim args As String

        If stream.Format.EqualsAny("AAC", "Opus") Then
            args += "-single"
        Else
            args += "-raw"
        End If

        args += " " & stream.ID & " -out " + outPath.ToShortFilePath.Escape + " " +
            sourcefile.ToShortFilePath.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux audio"
            proc.SkipString = "|"
            proc.Package = Package.MP4Box
            proc.Arguments = args
            proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
            proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
            proc.OutputFiles = {outPath}
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If MediaInfo.GetAudio(outPath, "Format").NullOrEmptyS Then
                proj.Log.Write("Error", "No format detected by MediaInfo.")
                ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj, overrideExisting)
            Else
                If Not ap Is Nothing Then ap.File = outPath
                proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
            End If
        Else
            proj.Log.Write("Error", "no output found")
            ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj, overrideExisting)
        End If
    End Sub

    Function GetAttachments(sourceFilePath As String) As List(Of Attachment)
        Dim cover = MediaInfo.GetGeneral(sourceFilePath, "Cover")

        If cover.NotNullOrEmptyS Then
            Return New List(Of Attachment) From {New Attachment With {.Name = "Cover"}}
        End If
    End Function

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class

<Serializable()>
Public Class mkvDemuxer
    Inherits Demuxer

    Sub New()
        Name = "mkvextract: Demux"
        InputExtensions = {"mkv", "webm"}
    End Sub

    Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)

        Dim stdout = ProcessHelp.GetConsoleOutput(Package.mkvmerge.Path, "--identify --ui-language en " +
            proj.SourceFile.Escape)

        Dim demuxAudio = (Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile)) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0
        Dim attachments = GetAttachments(stdout)
        Dim demuxChapters = ChaptersDemuxing
        VideoDemuxed = VideoDemuxing

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) OrElse
            Not proj Is p Then

            Using form As New StreamDemuxForm(Me, proj.SourceFile, attachments)
                If form.ShowDialog() <> DialogResult.OK Then
                    Throw New AbortException
                End If

                VideoDemuxed = form.cbDemuxVideo.Checked
                demuxChapters = form.cbDemuxChapters.Checked
                audioStreams = form.AudioStreams
                subtitles = form.Subtitles
            End Using
        End If

        If demuxAudio AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then
                audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)
            End If
        End If

        If demuxSubtitles AndAlso proj.DemuxSubtitles <> DemuxMode.None Then
            If subtitles Is Nothing Then
                subtitles = MediaInfo.GetSubtitles(proj.SourceFile)
            End If
        End If

        Demux(proj.SourceFile, audioStreams, subtitles, Nothing, proj, True, VideoDemuxed, OverrideExisting)

        If demuxChapters AndAlso stdout.Contains("Chapters: ") Then
            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux xml chapters"
                proc.SkipString = "Progress: "
                proc.WriteLog(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = proj.SourceFile.Escape + " chapters " + (proj.TempDir + proj.SourceFile.Base + "_chapters.xml").Escape
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using

            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux ogg chapters"
                proc.SkipString = "Progress: "
                proc.WriteLog(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = proj.SourceFile.Escape + " chapters " + (proj.TempDir + proj.SourceFile.Base + "_chapters.txt").Escape + " --simple"
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using
        End If

        Dim enabledAttachments = attachments.Where(Function(val) val.Enabled)

        If enabledAttachments.Any Then
            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux attachments"
                proc.SkipString = "Progress: "
                proc.WriteLog(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = proj.SourceFile.Escape + " attachments " + enabledAttachments.Select(
                    Function(val) val.ID & ":" + GetAttachmentPath(proj, val.Name).Escape).Join(" ")
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using
        End If

        If MediaInfo.GetVideo(proj.SourceFile, "FrameRate_Mode") = "VFR" Then
            Dim streamOrder = MediaInfo.GetVideo(proj.SourceFile, "StreamOrder").ToInt

            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux timestamps"
                proc.SkipString = "Progress: "
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = "timestamps_v2 " + proj.SourceFile.Escape + " " & streamOrder & ":" + (proj.TempDir + proj.SourceFile.Base + "_timestamps.txt").Escape
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using
        End If

        Log.Save(proj)
    End Sub

    Public Overrides Property OutputExtensions As String()
        Get
            If VideoDemuxing OrElse VideoDemuxed Then
                Return {"avi", "mpg", "h264", "h265"}
            End If

            Return {}
        End Get
        Set(value As String())
        End Set
    End Property

    Shared Sub Demux(
        sourcefile As String,
        audioStreams As IEnumerable(Of AudioStream),
        subtitles As IEnumerable(Of Subtitle),
        ap As AudioProfile,
        proj As Project,
        onlyEnabled As Boolean,
        videoDemuxing As Boolean,
        overrideExisting As Boolean)

        If audioStreams Is Nothing Then
            audioStreams = New List(Of AudioStream)
        End If

        If subtitles Is Nothing Then
            subtitles = New List(Of Subtitle)
        End If

        If onlyEnabled Then
            audioStreams = audioStreams.Where(Function(arg) arg.Enabled)
        End If

        subtitles = subtitles.Where(Function(subtitle) subtitle.Enabled)

        If Not audioStreams.Any AndAlso Not subtitles.Any AndAlso Not videoDemuxing Then
            Exit Sub
        End If

        Dim args = sourcefile.Escape + " tracks"
        Dim newCount As Integer
        Dim outPaths As New List(Of String)

        If videoDemuxing Then
            Dim stdout = ProcessHelp.GetConsoleOutput(Package.mkvmerge.Path, "--identify " + sourcefile.Escape)
            Dim id = Regex.Match(stdout, "Track ID (\d+): video").Groups(1).Value.ToInt
            Dim videoStreams = MediaInfo.GetVideoStreams(sourcefile)

            If videoStreams.Count > 0 Then
                Dim outPath = proj.TempDir + sourcefile.Base + videoStreams(0).ExtFull

                If Not outPath.Equals(sourcefile) Then
                    args += " " & id & ":" + outPath.Escape
                    outPaths.Add(outPath)

                    If Not outPath.FileExists Then
                        newCount += 1
                    End If
                End If
            End If
        End If

        For Each subtitle In subtitles
            'If Not subtitle.Enabled Then
            '    Continue For
            'End If

            Dim forced = If(subtitle.Forced, "_forced", "")
            Dim outPath = proj.TempDir + subtitle.Filename + forced + subtitle.ExtFull
            args += " " & subtitle.StreamOrder & ":" + outPath.Escape
            outPaths.Add(outPath)

            If Not outPath.FileExists Then
                newCount += 1
            End If
        Next

        Dim audioOutPaths As New Dictionary(Of String, AudioStream)(StringComparer.Ordinal)

        For Each stream In audioStreams
            Dim ext = stream.Ext

            If ext.Equals("m4a") Then
                ext = "aac"
            End If

            Dim outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream) + "." + ext
            audioOutPaths.Add(outPath, stream)
            outPaths.Add(outPath)
            args += " " & stream.StreamOrder & ":" + outPath.Escape

            If outPath.Ext.Equals("aac") Then
                outPath = outPath.ChangeExt("m4a")
            End If

            If Not outPath.FileExists Then
                newCount += 1
            End If
        Next

        If Not (newCount > 0 OrElse overrideExisting) Then
            Exit Sub
        End If

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux MKV"
            proc.SkipString = "Progress: "
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.mkvextract
            proc.Arguments = args + " --ui-language en"
            proc.AllowedExitCodes = {0, 1, 2}
            proc.OutputFiles = outPaths
            proc.Start()
        End Using

        outPaths.Clear()

        For Each outPath In audioOutPaths.Keys
            If File.Exists(outPath) Then
                If Not ap Is Nothing Then
                    ap.File = outPath
                End If

                proj.Log.WriteLine(MediaInfo.GetSummary(outPath) + BR)

                If outPath.Ext.Equals("aac") Then
                    Dim newOutPath = outPath.ChangeExt("m4a")
                    outPaths.Add(newOutPath)

                    Using proc As New Proc
                        proc.Project = proj
                        proc.Header = "Mux AAC to M4A"
                        proc.SkipString = "|"
                        proc.Package = Package.MP4Box
                        Dim sbr = If(outPath.Contains("SBR"), ":sbr", "")
                        proc.Arguments = "-add """ + outPath.ToShortFilePath + sbr +
                            ":name= "" -new " + newOutPath.ToShortFilePath.Escape
                        proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                        proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                        proc.OutputFiles = outPaths
                        proc.Start()
                    End Using

                    If File.Exists(newOutPath) Then
                        If Not ap Is Nothing Then
                            ap.File = newOutPath
                        End If

                        FileHelp.Delete(outPath)
                        proj.Log.WriteLine(BR + MediaInfo.GetSummary(newOutPath))
                    Else
                        Throw New ErrorAbortException("Error mux AAC to M4A", outPath)
                    End If
                End If
            Else
                proj.Log.Write("Error", "no output found")
                ffmpegDemuxer.DemuxAudio(sourcefile, audioOutPaths(outPath), ap, proj, overrideExisting)
            End If
        Next
    End Sub

    Shared Function GetAttachmentPath(proj As Project, name As String) As String
        Dim prefix = If(name.Base.EqualsAny("cover", "small_cover", "cover_land", "small_cover_land"), "", proj.SourceFile.Base + "_attachment_")
        Dim ret = proj.TempDir + prefix + name.Base + name.ExtFull
        Return ret
    End Function

    Function GetAttachments(stdout As String) As List(Of Attachment)
        Dim ret = New List(Of Attachment)

        For Each i In stdout.SplitLinesNoEmpty
            If i.StartsWith("Attachment ID ") Then
                Dim match = Regex.Match(i, "Attachment ID (\d+):.+, file name '(.+)'")

                If match.Success Then ret.Add(New Attachment With {
                    .ID = match.Groups(1).Value.ToInt,
                    .Name = match.Groups(2).Value})
            End If
        Next

        Return ret
    End Function

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class
