Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports KGySoft.CoreLibraries

Public Class MediaInfo
    Implements IDisposable

    Private Handle As IntPtr
    Private Shared Loaded As Boolean
    Public Const KeyDefault As Long = Long.MinValue

    Sub New(path As String)
        If Not Loaded Then
            Native.LoadLibrary(Package.MediaInfo.Path)
            Loaded = True
        End If

        Handle = MediaInfo_New()
        MediaInfo_Open(Handle, path)
    End Sub

    Private VideoStreamsValue As List(Of VideoStream)

    ReadOnly Property VideoStreams() As List(Of VideoStream)
        Get
            If VideoStreamsValue Is Nothing Then
                Dim count = MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Video, -1)

                If count > 0 Then
                    VideoStreamsValue = New List(Of VideoStream)(count)
                    For index = 0 To count - 1
                        Dim so = GetVideo(index, "StreamOrder").ToIntM 'opt.
                        'Dim streamOrder = GetVideo(index, "StreamOrder")
                        'If Not streamOrder.IsInt Then streamOrder = (index + 1).ToInvStr
                        'at.StreamOrder = streamOrder.ToInt
                        VideoStreamsValue.Add(New VideoStream With {.Index = index, .StreamOrder = If(so = -2147483646I, index + 1, so),
                                                                    .Format = GetVideo(index, "Format"), .ID = GetVideo(index, "ID").ToInt})
                    Next
                Else
                    VideoStreamsValue = New List(Of VideoStream)
                End If
            End If

            Return VideoStreamsValue
        End Get
    End Property

    ReadOnly Property AudioStreams() As List(Of AudioStream)
        Get
            Dim count = MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Audio, -1)
            'Dim offset As Integer 'Dead code ???

            If count > 0 Then
                Dim ret As New List(Of AudioStream)(count)
                For index = 0 To count - 1
                    'Dim streamOrder = GetAudio(index, "StreamOrder")
                    'If Not streamOrder.IsInt Then streamOrder = (index + 1).ToInvStr
                    'at.StreamOrder = streamOrder.ToInt + offset
                    'Dim id = GetAudio(index, "ID")
                    'If Not id.IsInt Then id = (index + 2).ToInvStr
                    'at.ID = id.ToInt + offset
                    Dim so = GetAudio(index, "StreamOrder").ToIntM ' opt.
                    Dim id = GetAudio(index, "ID").ToIntM ' opt.
                    '.StreamOrder = If(so = -2147483646I, index + 1 + offset, so + offset),
                    '.ID = If(id = -2147483646I, index + 2 + offset, id + offset),
                    Dim at As New AudioStream With {
                        .Index = index,
                        .StreamOrder = If(so = -2147483646I, index + 1, so),
                        .ID = If(id = -2147483646I, index + 2, id),
                        .Lossy = String.Equals(GetAudio(index, "Compression_Mode"), "Lossy"),
                        .SamplingRate = GetAudio(index, "SamplingRate").ToInt,
                        .BitDepth = GetAudio(index, "BitDepth").ToInt,
                        .Format = GetAudio(index, "Format"),
                        .FormatString = GetAudio(index, "Format/String"),
                        .FormatProfile = GetAudio(index, "Format_Profile"),
                        .Title = GetAudio(index, "Title").Trim,
                        .Forced = String.Equals(GetAudio(index, "Forced"), "Yes"),
                        .Default = String.Equals(GetAudio(index, "Default"), "Yes"),
                        .Delay = GetAudio(index, "Video_Delay").ToInt,
                        .Language = New Language(GetAudio(index, "Language/String2"))}

                    Dim lm = GetAudio(index, "Language_More")
                    If lm.NotNullOrEmptyS Then
                        If at.Title.NullOrEmptyS Then
                            at.Title = lm
                        Else
                            at.Title &= " - " & lm
                        End If
                    End If

                    If at.Delay = 0 Then
                        at.Delay = GetAudio(index, "Source_Delay").ToInt
                    End If

                    Dim bitrate = GetAudio(index, "BitRate")
                    Dim bi = bitrate.ToIntM()
                    If bi <> -2147483646I Then
                        at.Bitrate = (bi \ 1000)
                    ElseIf bitrate.IndexOf("/"c) >= 0 Then
                        Dim values = bitrate.Split("/"c)
                        at.Bitrate = (values(0).ToInt \ 1000)
                        at.Bitrate2 = (values(1).ToInt \ 1000)
                    End If

                    If at.Bitrate = 0 Then at.Bitrate = GetAudio(index, "FromStats_BitRate").ToInt

                    Dim channels = GetAudio(index, "Channel(s)")
                    at.Channels = channels.ToInt

                    If at.Channels = 0 Then
                        at.Channels = GetAudio(index, "Channel(s)_Original").ToInt
                    End If

                    If at.Channels = 0 Then
                        If channels.IndexOf("/"c) >= 0 Then
                            Dim values = channels.Split("/"c)
                            at.Channels = values(0).ToInt
                            at.Channels2 = values(1).ToInt
                        End If
                    End If

                    If Not AudioConverterForm.AudioConverterMode Then
                        Select Case p.DemuxAudio
                            Case DemuxMode.All
                                at.Enabled = True
                            Case DemuxMode.None
                                at.Enabled = False
                            Case DemuxMode.Preferred, DemuxMode.Dialog
                                Dim autoCode = p.PreferredAudio.ToLower(InvCult).Split({","c, ";"c, " "c}, StringSplitOptions.RemoveEmptyEntries)
                                at.Enabled = autoCode.ContainsAny({"all", at.Language.CultureInfo.TwoLetterISOLanguageName, at.Language.ThreeLetterCode})
                        End Select
                    End If

                    ret.Add(at)
                Next
                Return ret
            Else
                Return New List(Of AudioStream)
            End If

        End Get
    End Property

    ReadOnly Property Subtitles() As List(Of Subtitle)
        Get
            Dim count = MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Text, -1)
            'Dim offset As Integer 'Dead Code ???

            If count > 0 Then
                Dim ret As New List(Of Subtitle)(count)

                For index = 0 To count - 1
                    Dim streamOrder = GetText(index, "StreamOrder")
                    Dim subtitle As New Subtitle(New Language(GetText(index, "Language"))) With {
                        .Index = index,
                        .Forced = String.Equals(GetText(index, "Forced"), "Yes"),
                        .[Default] = String.Equals(GetText(index, "Default"), "Yes"),
                        .ID = GetText(index, "ID").ToInt,
                        .Title = GetText(index, "Title").Trim,
                        .CodecString = GetText(index, "Codec/String"),
                        .Format = GetText(index, "Format"),
                        .Size = GetText(index, "StreamSize").ToInt}

                    If streamOrder.NotNullOrEmptyS Then
                        subtitle.StreamOrder = If(streamOrder.LastIndexOf("-"c) >= 0, streamOrder.Right("-").ToInt, streamOrder.ToInt) ', streamOrder.Right("-").ToInt + offset,
                    End If

                    If subtitle.CodecString.NullOrEmptyS Then subtitle.CodecString = GetText(index, "Format")
                    subtitle.Enabled = p.PreferredSubtitles.ToLower(InvCult).Split({","c, ";"c, " "c}, StringSplitOptions.RemoveEmptyEntries).
                        ContainsAny({"all", subtitle.Language.TwoLetterCode, subtitle.Language.ThreeLetterCode}) OrElse p.DemuxSubtitles = DemuxMode.All

                    ret.Add(subtitle)
                Next
                Return ret
            Else
                Return New List(Of Subtitle)
            End If

        End Get
    End Property

    Shared Function GetAudioStreams(path As String, Optional Key As Long = KeyDefault) As List(Of AudioStream)
        Return GetMediaInfo(path, Key).AudioStreams
    End Function

    Shared Function GetVideoStreams(path As String, Optional Key As Long = KeyDefault) As List(Of VideoStream)
        Return GetMediaInfo(path, Key).VideoStreams
    End Function

    Shared Function GetSubtitles(path As String, Optional Key As Long = KeyDefault) As List(Of Subtitle)
        Return GetMediaInfo(path, Key).Subtitles
    End Function

    Shared Function GetSummary(path As String, Optional Key As Long = KeyDefault) As String
        Dim mi = GetMediaInfo(path, Key)
        MediaInfo_Option(mi.Handle, "Complete", "0")
        Dim ret = Marshal.PtrToStringUni(MediaInfo_Inform(mi.Handle, 0))
        If ret.Contains("UniqueID/String") Then ret = Regex.Replace(ret, "UniqueID/String +: .+\n", "")
        If ret.Contains("Unique ID") Then ret = Regex.Replace(ret, "Unique ID +: .+\n", "")
        If ret.Contains("Encoded_Library_Settings") Then ret = Regex.Replace(ret, "Encoded_Library_Settings +: .+\n", "")
        If ret.Contains("Encoding settings") Then ret = Regex.Replace(ret, "Encoding settings +: .+\n", "")
        ret = ret.Replace("Format settings, ", "Format, ")
        Return ret.FormatColumn(":"c).Trim
    End Function

    Shared Function GetCompleteSummary(path As String, Optional Key As Long = KeyDefault) As String
        Dim mi = GetMediaInfo(path, Key)
        MediaInfo_Option(mi.Handle, "Complete", "1")
        Return Marshal.PtrToStringUni(MediaInfo_Inform(mi.Handle, 0))
    End Function

    Function GetInfo(streamKind As MediaInfoStreamKind, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, streamKind, 0, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Function GetAudio(streamNumber As Integer, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, MediaInfoStreamKind.Audio, streamNumber, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Function GetText(streamNumber As Integer, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, MediaInfoStreamKind.Text, streamNumber, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Function GetInfo(streamKind As MediaInfoStreamKind, streamNumber As Integer, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, streamKind, streamNumber, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Shared Function GetInfo(path As String, streamKind As MediaInfoStreamKind, parameter As String, Optional Key As Long = KeyDefault) As String
        If path.NullOrEmptyS Then Return ""
        Return GetMediaInfo(path, Key).GetInfo(streamKind, parameter)
    End Function

    Shared Function GetMenu(path As String, parameter As String) As String
        Return GetInfo(path, MediaInfoStreamKind.Menu, parameter)
    End Function

    Shared Function GetAudio(path As String, parameter As String, Optional Key As Long = KeyDefault) As String
        Return GetInfo(path, MediaInfoStreamKind.Audio, parameter, Key)
    End Function

    Function GetVideo(parameter As String) As String
        Return GetInfo(MediaInfoStreamKind.Video, parameter)
    End Function

    Function GetVideo(streamNumber As Integer, parameter As String) As String
        Return GetInfo(MediaInfoStreamKind.Video, streamNumber, parameter)
    End Function

    Shared Function GetVideo(path As String, parameter As String) As String
        Return GetInfo(path, MediaInfoStreamKind.Video, parameter)
    End Function

    Function GetGeneral(parameter As String) As String
        Return GetInfo(MediaInfoStreamKind.General, parameter)
    End Function

    Shared Function GetGeneral(path As String, parameter As String) As String
        Return GetInfo(path, MediaInfoStreamKind.General, parameter)
    End Function

    Shared Function GetVideoFormat(path As String) As String
        Dim ret = GetInfo(path, MediaInfoStreamKind.Video, "Format")

        If ret = "MPEG Video" Then
            ret = "MPEG"
        End If

        Return ret
    End Function

    Function GetFrameRate(Optional defaultValue As Double = 25) As Double
        Dim ret = GetVideo("FrameRate_Num").ToInt / GetVideo("FrameRate_Den").ToInt

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        ret = GetVideo("FrameRate_Original_Num").ToInt / GetVideo("FrameRate_Original_Den").ToInt

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        ret = GetVideo("FrameRate").ToDouble

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        ret = GetVideo("FrameRate_Original").ToDouble

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        ret = GetVideo("FrameRate_Nominal").ToDouble

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        Return defaultValue
    End Function

    Shared Function GetFrameRate(path As String, Optional defaultValue As Double = 25, Optional Key As Long = KeyDefault) As Double
        Return GetMediaInfo(path, Key).GetFrameRate(defaultValue)
    End Function

    Function GetChannels() As Integer
        Dim channelsString = GetInfo(MediaInfoStreamKind.Audio, "Channel(s)")
        Dim ret = channelsString.ToInt
        If ret = 0 Then ret = GetInfo(MediaInfoStreamKind.Audio, "Channel(s)_Original").ToInt

        If ret = 0 Then
            If channelsString.IndexOf("/"c) >= 0 Then
                Dim values = channelsString.Split("/"c)
                Dim value0 = values(0).ToInt
                Dim value1 = values(1).ToInt
                If value0 >= value1 Then ret = value0 Else ret = value1
            End If
        End If

        If ret = 0 Then ret = 2
        Return ret
    End Function

    Shared Function GetChannels(path As String, Optional Key As Long = KeyDefault) As Integer
        Return GetMediaInfo(path, Key).GetChannels
    End Function

    Shared Function GetAudioCodecs(path As String) As String
        Dim ret = GetGeneral(path, "Audio_Codec_List")

        ret = ret.Replace("MPEG-1 Audio layer 3", "MP3")
        ret = ret.Replace("MPEG-2 Audio layer 3", "MP3")
        ret = ret.Replace("MPEG-1 Audio layer 2", "MP2")

        Return ret
    End Function

    Function GetVideoCount() As Integer
        Return MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Video, -1)
    End Function

    Shared Function GetVideoCount(Path As String, Optional Key As Long = KeyDefault) As Integer
        Return GetMediaInfo(Path, Key).GetVideoCount
    End Function

    Function GetAudioCount() As Integer
        Return MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Audio, -1)
    End Function

    Shared Function GetAudioCount(path As String, Optional Key As Long = KeyDefault) As Integer
        Return GetMediaInfo(path, Key).GetAudioCount
    End Function

    Function GetSubtitleCount() As Integer
        Return MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Text, -1)
    End Function

    Shared Function GetSubtitleCount(path As String, Optional Key As Long = KeyDefault) As Integer
        Return GetMediaInfo(path, Key).GetSubtitleCount
    End Function

    'Public Shared Cache As New Dictionary(Of Long, MediaInfo)(197)
    Public Shared Cache As Dictionary(Of Long, MediaInfo)
    'Public Shared CacheTS As LockingDictionary(Of Long, MediaInfo) = Cache.AsThreadSafe()
    'Public Shared CacheTS As KGySoft.Collections.LockingDictionary(Of Long, MediaInfo) = Cache.AsThreadSafe 'Or use ThreadSafeDictionary?

    Shared Function GetMediaInfo(path As String, Optional Key As Long = KeyDefault) As MediaInfo
        If path?.Length > 0 Then
            Dim ret As MediaInfo
            'Dim key = path & File.GetLastWriteTime(path).Ticks
            If Key <= 0 Then Key = ((path.GetHashCode + 2147483648L) << 16) + path.Length
            If Cache.TryGetValue(Key, ret) Then Return ret
            ret = New MediaInfo(path)
            'Dim cTS = Cache.AsThreadSafe
            'CacheTS.Item(Key) = ret
            Cache.Item(Key) = ret
            Return ret
        End If
    End Function

    Shared Sub ClearCache(Optional createNew As Boolean = True, Optional newCapacity As Integer = 197)
        If Cache IsNot Nothing Then
            'For Each i In Cache.Values : i?.Dispose()
            Parallel.ForEach(Cache.Values, New ParallelOptions With {.MaxDegreeOfParallelism = Math.Max(CPUsC \ 2, 1)}, Sub(m) m?.Dispose())
            Cache.Clear()
            Cache = If(createNew, New Dictionary(Of Long, MediaInfo)(newCapacity), Nothing)
        End If
    End Sub

#Region "IDisposable"

    Private Disposed As Boolean
    Sub Dispose() Implements IDisposable.Dispose
        If Not Disposed Then 'AndAlso Loaded Then
            Disposed = True
            Try
                MediaInfo_Close(Handle)
                MediaInfo_Delete(Handle)
            Catch ex As Exception 'debug
                Console.Beep(400, 200) 'debug
                If Not g.MainForm.ForceClose AndAlso Not g.MainForm.IsDisposed Then Microsoft.VisualBasic.MsgBox(Loaded & "-Loaded|MediaInfo Dispose Exception, (Catched):" & BR & ex.ToString)
            End Try
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub

#End Region

#Region "native"

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_New() As IntPtr
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Sub MediaInfo_Delete(Handle As IntPtr)
    End Sub

    <DllImport("MediaInfo.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function MediaInfo_Open(Handle As IntPtr, FileName As String) As Integer
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_Close(Handle As IntPtr) As Integer
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_Inform(Handle As IntPtr,
                                             Reserved As Integer) As IntPtr
    End Function

    <DllImport("MediaInfo.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function MediaInfo_Get(Handle As IntPtr,
                                          StreamKind As MediaInfoStreamKind,
                                          StreamNumber As Integer, Parameter As String,
                                          KindOfInfo As MediaInfoInfoKind,
                                          KindOfSearch As MediaInfoInfoKind) As IntPtr
    End Function

    <DllImport("MediaInfo.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function MediaInfo_Option(Handle As IntPtr,
                                             OptionString As String,
                                             Value As String) As IntPtr
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_State_Get(Handle As IntPtr) As Integer
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_Count_Get(Handle As IntPtr,
                                                StreamKind As MediaInfoStreamKind,
                                                StreamNumber As Integer) As Integer
    End Function

#End Region

End Class

Public Enum MediaInfoStreamKind
    General
    Video
    Audio
    Text
    Other
    Image
    Menu
    Max
End Enum

Public Enum MediaInfoInfoKind
    Name
    Text
    Measure
    Options
    NameText
    MeasureText
    Info
    HowTo
End Enum