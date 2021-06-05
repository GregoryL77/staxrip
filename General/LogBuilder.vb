
Imports System.Globalization
Imports System.Text
Imports Microsoft.Win32

<Serializable>
Public Class LogBuilder
    Private StartTime As DateTime
    Private Log As New StringBuilder(2048) '1024
    Private Last As String

    Sub Append(content As String)
        SyncLock Log
            Log.Append(content)

            If content.NotNullOrEmptyS Then
                Last = content
            End If
        End SyncLock
    End Sub

    Function EndsWith(value As String) As Boolean
        If Last.NullOrEmptyS Then
            Return False
        End If

        Return Last.EndsWith(value, StringComparison.Ordinal)
    End Function

    Private Shared WriteLock As New Object

    Sub Write(title As String, content As String)
        SyncLock WriteLock
            StartTime = DateTime.Now

            If Not EndsWith(BR2) Then
                Append(BR)
            End If

            Append(FormatHeader(title))

            If content.NotNullOrEmptyS Then
                If content.EndsWith(BR, StringComparison.Ordinal) Then
                    Append(content)
                Else
                    Append(content + BR)
                End If
            End If
        End SyncLock
    End Sub

    Sub WriteLine(value As String)
        If value.NotNullOrEmptyS Then
            If value.EndsWith(BR, StringComparison.Ordinal) Then
                Append(value)
            Else
                Append(value + BR)
            End If
        End If
    End Sub

    ReadOnly Property Length As Integer
        Get
            SyncLock Log
                Return Log.Length
            End SyncLock
        End Get
    End Property

    Sub WriteHeader(value As String)
        If value.NotNullOrEmptyS Then
            StartTime = DateTime.Now

            If Not EndsWith(BR2) Then
                Append(BR)
            End If

            Append(FormatHeader(value))
        End If
    End Sub

    Function FormatHeader(value As String) As String
        Dim len = (65 - value.Length) \ 2
        Return "--" + "-".Multiply(len) + " " + value + " " + "-".Multiply(len) + "--" + BR2
    End Function

    Shared EnvironmentString As String 'cached due to bug report

    Sub WriteEnvironment()
        If ToString.Contains("- System Environment -") Then
            Exit Sub
        End If

        WriteHeader("System Environment")

        If EnvironmentString.NullOrEmptyS Then EnvironmentString =
            "StaxRip:" + Application.ProductVersion + BR +
            "Windows:" + Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName") + " " + Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId") + BR +
            "Language:" + CultureInfo.CurrentCulture.EnglishName + BR +
            "CPU:" + Registry.LocalMachine.GetString("HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString") + BR +
            "GPU:" + String.Join(", ", OS.VideoControllers) + BR + 'OS.VideoControllers.Item2) + BR +
            "Resolution:" & ScreenResPrim.Width & " x " & ScreenResPrim.Height & BR +
            "DPI:" & g.DPI

        WriteLine(EnvironmentString.FormatColumn(":"))
    End Sub

    Shared ConfigurationString As String 'cached due to bug report

    Sub WriteConfiguration()
        If ToString.Contains("- Configuration -") Then
            Exit Sub
        End If

        WriteHeader("Configuration")

        If ConfigurationString.NullOrEmptyS Then ConfigurationString =
            $"Template: {p.TemplateName}{BR}" +
            $"Video Encoder Profile: {p.VideoEncoder.Name}{BR}" +
            $"Container/Muxer Profile: {p.VideoEncoder.Muxer.Name}{BR}"

        WriteLine(ConfigurationString.FormatColumn(":"))
    End Sub

    Sub WriteStats()
        WriteStats(StartTime)
    End Sub

    Sub WriteStats(start As DateTime)
        Dim n = DateTime.Now.Subtract(start)

        If Not EndsWith(BR2) Then
            Append(BR)
        End If

        Append("Start: ".PadRight(10) + start.ToLongTimeString + BR)
        Append("End: ".PadRight(10) + DateTime.Now.ToLongTimeString + BR)
        Append("Duration: " + CInt(Math.Floor(n.TotalHours)).ToString("d2") + ":" + n.Minutes.ToString("d2") + ":" + n.Seconds.ToString("d2") + BR2)
    End Sub

    Function IsEmpty() As Boolean
        SyncLock Log
            Return Log.Length = 0
        End SyncLock
    End Function

    Public Overrides Function ToString() As String
        SyncLock Log
            Return Log.ToString
        End SyncLock
    End Function

    Sub Save(Optional proj As Project = Nothing)
        If proj Is Nothing Then
            proj = p
        End If

        SyncLock Log
            Log.ToString.WriteFileUTF8(GetPath(proj))
        End SyncLock
    End Sub

    Function GetPath(Optional proj As Project = Nothing) As String
        If proj Is Nothing Then
            proj = p
        End If

        If proj.SourceFile.NullOrEmptyS Then
            Return Folder.Temp + "staxrip.log"
        ElseIf proj.TempDir.NullOrEmptyS Then
            Return proj.SourceFile.Dir + proj.SourceFile.Base + "_staxrip.log"
        Else
            Return proj.TempDir + proj.TargetFile.Base + "_staxrip.log"
        End If
    End Function
End Class
