
Imports System.Runtime.Serialization
Imports System.ComponentModel
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Text
Imports System.Security.Permissions

Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic
Imports Microsoft.Win32
Imports KGySoft.Collections
Imports KGySoft.CoreLibraries

Public Class Folder
    Shared ReadOnly Property Desktop As String
        Get
            Return Environment.GetFolderPath(Environment.SpecialFolder.Desktop).FixDir
        End Get
    End Property

    Shared ReadOnly Property Fonts As String
        Get
            Return Environment.GetFolderPath(Environment.SpecialFolder.Fonts).FixDir
        End Get
    End Property

    Shared StartupValue As String

    Shared ReadOnly Property Startup As String
        Get
            If StartupValue Is Nothing Then
                StartupValue = Application.StartupPath.FixDir
            End If

            Return StartupValue
        End Get
    End Property

    Shared ReadOnly Property Current As String
        Get
            Return Environment.CurrentDirectory.FixDir
        End Get
    End Property

    Shared ReadOnly Property Temp As String
        Get
            Return Path.GetTempPath.FixDir
        End Get
    End Property

    Shared ReadOnly Property System As String
        Get
            Return Environment.SystemDirectory.FixDir
        End Get
    End Property

    Shared ReadOnly Property Programs As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.ProgramFiles).FixDir
        End Get
    End Property

    Shared ReadOnly Property Home As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.UserProfile).FixDir
        End Get
    End Property

    Shared ReadOnly Property AppDataCommon As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.CommonApplicationData).FixDir
        End Get
    End Property

    Shared ReadOnly Property AppDataLocal As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.LocalApplicationData).FixDir
        End Get
    End Property

    Shared ReadOnly Property AppDataRoaming As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.ApplicationData).FixDir
        End Get
    End Property

    Shared ReadOnly Property Windows As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.Windows).FixDir
        End Get
    End Property

    Shared ReadOnly Property Apps As String
        Get
            Return Folder.Startup & "Apps\"
        End Get
    End Property

    Shared ReadOnly Property Plugins As String
        Get
            If FrameServerHelp.IsPortable Then
                If p.Script.Engine = ScriptEngine.AviSynth Then
                    Return Folder.Settings & "Plugins\AviSynth\"
                Else
                    Return Folder.Settings & "Plugins\VapourSynth\"
                End If
            Else
                If p.Script.Engine = ScriptEngine.AviSynth Then
                    Return Registry.LocalMachine.GetString("SOFTWARE\AviSynth", "plugindir+").FixDir
                Else
                    Return Registry.LocalMachine.GetString("SOFTWARE\Wow6432Node\VapourSynth", "Plugins64").FixDir
                End If
            End If
        End Get
    End Property

    Shared ReadOnly Property Scripts As String
        Get
            Return Settings & "Scripts\"
        End Get
    End Property

    Private Shared SettingsValue As String

    Shared ReadOnly Property Settings As String
        Get
            If SettingsValue Is Nothing Then
                For Each location In Registry.CurrentUser.GetValueNames("Software\StaxRip\SettingsLocation")
                    If Not Directory.Exists(location) Then
                        Registry.CurrentUser.DeleteValue("Software\StaxRip\SettingsLocation", location)
                    End If
                Next

                SettingsValue = Registry.CurrentUser.GetString("Software\StaxRip\SettingsLocation", Folder.Startup)

                If Not Directory.Exists(SettingsValue) Then
                    Dim td As New TaskDialog(Of String)

                    td.MainInstruction = "Settings Directory"
                    td.Content = "Select the location of the settings directory."

                    td.AddCommand(Folder.AppDataRoaming + "StaxRip")
                    td.AddCommand(Folder.Startup + "Settings")
                    td.AddCommand("Browse for custom directory", "custom")

                    Dim dir = td.Show

                    If String.Equals(dir, "custom") Then
                        Using dialog As New FolderBrowserDialog
                            dialog.SelectedPath = Folder.Startup

                            If dialog.ShowDialog = DialogResult.OK Then
                                dir = dialog.SelectedPath
                            Else
                                dir = Folder.AppDataCommon + "StaxRip"
                            End If
                        End Using
                    ElseIf dir.NullOrEmptyS Then
                        dir = Folder.AppDataCommon + "StaxRip"
                    End If

                    If Not dir.DirExists Then
                        Try
                            Directory.CreateDirectory(dir)
                        Catch
                            dir = Folder.AppDataCommon + "StaxRip"

                            If Not dir.DirExists Then
                                Directory.CreateDirectory(dir)
                            End If
                        End Try
                    End If

                    dir = dir.FixDir

                    Dim scriptDir = dir + "Scripts\"

                    If Not scriptDir.DirExists Then
                        Directory.CreateDirectory(scriptDir)
                        Dim code = "[MainModule]::MsgInfo('Hello World')"
                        code.WriteFileUTF8BOM(scriptDir + "Hello World.ps1")
                        Directory.CreateDirectory(scriptDir + "Auto Load")
                    End If

                    FolderHelp.Create(dir + "Plugins\AviSynth")
                    FolderHelp.Create(dir + "Plugins\VapourSynth")
                    FolderHelp.Create(dir + "Plugins\Dual")

                    Registry.CurrentUser.Write("Software\StaxRip\SettingsLocation", Folder.Startup, dir)
                    SettingsValue = dir
                End If
            End If

            Return SettingsValue
        End Get
    End Property

    Shared ReadOnly Property Template As String
        Get
            Dim ret = Settings + "Templates\"
            Dim fresh As Boolean

            If Not Directory.Exists(ret) Then
                Directory.CreateDirectory(ret)
                fresh = True
            End If

            Dim version = 44

            If fresh OrElse Not s.Storage.GetInt("template update") = version Then
                s.Storage.SetInt("template update", version)

                Dim files = Directory.GetFiles(ret, "*.srip")

                If files.Length > 0 Then
                    FolderHelp.Delete(ret + "Backup")
                    Directory.CreateDirectory(ret + "Backup")

                    For Each i In files
                        FileHelp.Move(i, i.Dir + "Backup\" + i.FileName)
                    Next
                End If

                Dim manual As New Project
                manual.Init()
                manual.Script = VideoScript.GetDefaults()(0)
                manual.Script.Filters(0) = VideoFilter.GetDefault("Source", "Manual")
                manual.DemuxAudio = DemuxMode.Dialog
                manual.DemuxSubtitles = DemuxMode.Dialog
                SafeSerialization.Serialize(manual, ret + "Manual Workflow.srip")

                Dim auto As New Project
                auto.Init()
                auto.Script.Filters(0) = VideoFilter.GetDefault("Source", "Automatic")
                auto.DemuxAudio = DemuxMode.All
                auto.DemuxSubtitles = DemuxMode.All
                SafeSerialization.Serialize(auto, ret + "Automatic Workflow.srip")

                Dim fastLoad As New Project
                fastLoad.Init()
                fastLoad.Script.Filters(0) = New VideoFilter("Source", "DSS2/L-Smash", $"srcFile = ""%source_file%""{BR}ext = LCase(RightStr(srcFile, 3)){BR}(ext == ""mp4"") ? LSMASHVideoSource(srcFile, format = ""YUV420P8"") : DSS2(srcFile)")
                fastLoad.DemuxAudio = DemuxMode.None
                fastLoad.DemuxSubtitles = DemuxMode.None
                SafeSerialization.Serialize(fastLoad, ret + "No indexing and demuxing.srip")

                Dim remux As New Project
                remux.Init()
                remux.Script.Filters(0) = fastLoad.Script.Filters(0).GetCopy
                remux.DemuxAudio = DemuxMode.None
                remux.DemuxSubtitles = DemuxMode.None
                remux.VideoEncoder = New NullEncoder
                remux.Audio0 = New MuxAudioProfile
                remux.Audio1 = New MuxAudioProfile
                SafeSerialization.Serialize(remux, ret + "Re-mux.srip")
            End If

            Return ret
        End Get
    End Property

    <DllImport("shfolder.dll", CharSet:=CharSet.Unicode)>
    Shared Function SHGetFolderPath(
        hwndOwner As IntPtr,
        nFolder As Integer,
        hToken As IntPtr,
        dwFlags As Integer,
        lpszPath As StringBuilder) As Integer
    End Function

    Shared Function GetFolderPath(folder As Environment.SpecialFolder) As String
        Dim sb As New StringBuilder(512)
        SHGetFolderPath(IntPtr.Zero, CInt(folder), IntPtr.Zero, 0, sb)
        Dim ret = sb.ToString.FixDir
        Call New FileIOPermission(FileIOPermissionAccess.PathDiscovery, ret).Demand()
        Return ret
    End Function
End Class

Public Class SafeSerialization
    Shared Sub Serialize(o As Object, path As String)
        Dim fieldInfos As FieldInfo() = o.GetType.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
        Dim list As New List(Of Object)(fieldInfos.Length - 1) 'or w/o -1 ?

        For n = 0 To fieldInfos.Length - 1
            Dim i = fieldInfos(n)

            If Not i.IsNotSerialized Then
                Dim value = i.GetValue(o)

                If value IsNot Nothing Then
                    Dim mc As New FieldContainer With {.Name = i.Name, .Value = If(IsSimpleType(i.FieldType), value, GetObjectData(value))}
                    list.Add(mc)
                End If
            End If
        Next n

        'list.Capacity = list.Count 'Needed?, seems not size diff in prof save
        Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter

        Try
            Using fs As New FileStream(path, FileMode.Create)
                bf.Serialize(fs, list)
            End Using
        Catch
        End Try
    End Sub

    Shared Function Deserialize(Of T)(instance As T, path As String) As T
        Dim safeInstance = DirectCast(instance, ISafeSerialization)

        If File.Exists(path) Then
            Dim list As List(Of Object)
            Using fs As New FileStream(path, FileMode.Open)
                Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                list = DirectCast(bf.Deserialize(fs), List(Of Object))
            End Using

            Dim fiGF As FieldInfo() = instance.GetType.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
            For f = 0 To fiGF.Length - 1
                Dim iFieldInfo = fiGF(f)
                Dim iFiName As String = iFieldInfo.Name
                Dim iFiNLn As Integer = iFiName.Length 'Is This Len Opt. really Needed, not meas.gain ???
                Dim nEqWUpd As Boolean = iFiNLn <> 11 OrElse Not String.Equals(iFiName, "_WasUpdated")
                Dim iFiNS As Boolean = Not iFieldInfo.IsNotSerialized

                For n = 0 To list.Count - 1
                    If iFiNS Then
                        Dim i = DirectCast(list(n), FieldContainer)
                        If iFiNLn = i.Name.Length AndAlso String.Equals(i.Name, iFiName) Then
                            Try
                                Dim bt = TryCast(i.Value, Byte())
                                If bt IsNot Nothing Then
                                    iFieldInfo.SetValue(instance, GetObjectInstance(bt))
                                Else
                                    If nEqWUpd Then
                                        iFieldInfo.SetValue(instance, i.Value)
                                    End If
                                End If
                            Catch ex As Exception
                                safeInstance.WasUpdated = True
                            End Try
                        End If
                    End If
                Next n
            Next f
            'For Each i As FieldContainer In list 'Orginal !!! - Not much speed lost! ~5%,1ms
            '    For Each iFieldInfo In instance.GetType.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
            '        If Not iFieldInfo.IsNotSerialized Then
            '            If String.Equals(i.Name, iFieldInfo.Name) Then
            '                Try
            '                    If i.Value.GetType Is GetType(Byte()) Then
            '                        iFieldInfo.SetValue(instance, GetObjectInstance(DirectCast(i.Value, Byte())))
            '                    Else
            '                        If Not String.Equals(iFieldInfo.Name, "_WasUpdated") Then
            '                            iFieldInfo.SetValue(instance, i.Value)
            '                        End If
            '                    End If
            '                Catch ex As Exception
            '                    safeInstance.WasUpdated = True
            '                End Try
            '            End If
            '        End If
            '    Next iFieldInfo
            'Next i
        End If

        safeInstance.Init()

        If safeInstance.WasUpdated Then
            safeInstance.WasUpdated = False
            Serialize(instance, path)
        End If

        Return instance
    End Function

    Private Shared Function IsSimpleType(t As Type) As Boolean
        Return t.IsPrimitive OrElse
            t Is GetType(String) OrElse
            t Is GetType(DateTime) OrElse
            t Is GetType(SettingBag(Of String)) OrElse
            t Is GetType(SettingBag(Of Boolean)) OrElse
            t Is GetType(SettingBag(Of Integer)) OrElse
            t Is GetType(SettingBag(Of Double)) OrElse
            t Is GetType(SettingBag(Of Single))
    End Function

    <DebuggerNonUserCode()>
    Private Shared Function GetObjectInstance(ba As Byte()) As Object
        Using ms As New MemoryStream(ba)
            Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            'Static binder As New LegacySerializationBinder
            'bf.Binder = binder
            Return bf.Deserialize(ms)
        End Using
    End Function

    Private Shared Function GetObjectData(o As Object) As Byte()
        Using ms As New MemoryStream
            Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            bf.Serialize(ms, o)
            Return ms.ToArray()
        End Using
    End Function

    <Serializable()>
    Public Class FieldContainer
        Public Value As Object
        Public Name As String
    End Class

    Shared Function Check(iface As ISafeSerialization, obj As Object, key As String, version As Integer) As Boolean
        'If obj Is Nothing OrElse Not iface.Versions.ContainsKey(key) OrElse iface.Versions(key) <> version Then
        Dim ifv As Integer
        If obj Is Nothing OrElse Not iface.Versions.TryGetValue(key, ifv) OrElse ifv <> version Then
            iface.Versions(key) = version
            iface.WasUpdated = True
            Return True
        End If
    End Function

    'legacy
    Private Class LegacySerializationBinder
        Inherits SerializationBinder

        Overrides Function BindToType(assemblyName As String, typeName As String) As Type
            'If typeName.Contains("CLIEncoder") Then
            '    typeName = typeName.Replace("CLIEncoder", "CmdlEncoder")
            'End If

            Return Type.GetType(typeName)
        End Function
    End Class
End Class

Public Interface ISafeSerialization
    Property WasUpdated() As Boolean
    ReadOnly Property Versions() As Dictionary(Of String, Integer)
    Sub Init()
End Interface

Public Class HelpDocument
    Private Path As String
    Private IsClosed As Boolean

    Property Writer As XmlTextWriter

    Sub New(path As String)
        Me.Path = path
    End Sub

    Sub WriteStart(title As String)
        WriteStart(title, True)
    End Sub

    Sub WriteStart(title As String, showTitle As Boolean)
        Dim script = "<script type=""text/javascript""></script>"

        Dim style = "<style type=""text/css"">
@import url(http://fonts.googleapis.com/css?family=Lato:700,900);

body {
    font-family: Tahoma, Geneva, sans-serif;
    color:#DDDDDD;
    background-color:#323232;
}

h1 {
    font-size: 150%;
    margin-bottom: -4pt;
}

h2 {
    font-size: 120%;
    margin-bottom: -8pt;
}

h3 {
    font-size: 100%;
    margin-bottom: -8pt;
}

a {
    color: #3C8CC8;
}

td {
    width: 50%;
    vertical-align: top;
}

table {
    table-layout: fixed;
}
</style>"

        Writer = New XmlTextWriter(Path, Encoding.UTF8)
        Writer.Formatting = Formatting.Indented
        Writer.WriteRaw("<!doctype html>")
        Writer.WriteStartElement("html")
        Writer.WriteStartElement("head")
        Writer.WriteElementString("title", title)
        Writer.WriteRaw(BR + style.ToString + BR)
        Writer.WriteRaw(BR + script.ToString + BR)
        Writer.WriteEndElement() 'head
        Writer.WriteStartElement("body")

        If showTitle Then
            Writer.WriteElementString("h1", title)
        End If
    End Sub

    Sub WriteParagraph(text As String, Optional convert As Boolean = False)
        If convert Then
            text = ConvertChars(text)
        End If

        WriteElement("p", text)
    End Sub

    Sub Write(title As String, text As String, Optional convert As Boolean = False)
        If convert Then
            text = ConvertChars(text)
        End If

        WriteElement("h2", title)
        WriteElement("p", text)
    End Sub

    Sub WriteH2(text As String)
        WriteElement("h2", text)
    End Sub

    Sub WriteH3(text As String)
        WriteElement("h3", text)
    End Sub

    Sub WriteElement(elementName As String, rawText As String)
        If rawText.NullOrEmptyS Then
            Exit Sub
        End If

        Writer.WriteStartElement(elementName)

        If MustConvert(rawText) Then
            Writer.WriteRaw(ConvertMarkup(rawText, False))
        Else
            Writer.WriteRaw(rawText)
        End If

        Writer.WriteEndElement()
    End Sub

    Shared Function ConvertChars(value As String) As String
        value = value.FixBreak
        value = value.Replace("<", "&lt;").Replace(">", "&gt;").Replace(BR, "<br>")

        Return value
    End Function

    Shared Function ConvertMarkup(value As String, stripOnly As Boolean) As String
        If stripOnly Then
            If value.IndexOf("["c) >= 0 Then
                Dim re As New Regex("\[(.+?) (.+?)\]")

                If re.IsMatch(value) Then
                    value = re.Replace(value, "$2")
                End If
            End If

            If value.Contains("'''") Then
                Dim re As New Regex("'''(.+?)'''")

                If re.IsMatch(value) Then
                    value = re.Replace(value, "$1")
                End If
            End If
        Else
            If value.IndexOf("["c) >= 0 Then
                Dim re As New Regex("\[(.+?) (.+?)\]")
                Dim m = re.Match(value)

                If m.Success Then
                    value = re.Replace(value, "<a href=""$1"">$2</a>")
                End If
            End If

            If value.Contains("'''") Then
                Dim re As New Regex("'''(.+?)'''")
                Dim m = re.Match(value)

                If m.Success Then
                    value = re.Replace(value, "<b>$1</b>")
                End If
            End If
        End If

        Return value
    End Function

    Shared Function MustConvert(value As String) As Boolean
        If value.NotNullOrEmptyS AndAlso (value.IndexOf("["c) >= 0 OrElse value.Contains("'''")) Then '=value.Contains("[")
            Return True
        End If
    End Function

    Sub WriteTips(ParamArray tips As StringPairList())
        If tips Is Nothing OrElse tips.Length <= 0 Then
            Exit Sub
        End If

        Dim list As New StringPairList

        For Each i In tips
            If i IsNot Nothing Then list.AddRange(i)
        Next

        list.Sort()

        For Each i In list
            WriteH3(HelpDocument.ConvertChars(i.Name))
            WriteParagraph(HelpDocument.ConvertChars(i.Value))
        Next
    End Sub

    Sub WriteList(ParamArray values As String())
        Writer.WriteStartElement("ul")

        For Each i In values
            Writer.WriteStartElement("li")
            Writer.WriteRaw(ConvertMarkup(i, False))
            Writer.WriteEndElement()
        Next

        Writer.WriteEndElement()
    End Sub

    Sub WriteTable(list As IEnumerable(Of StringPair))
        WriteTable(Nothing, Nothing, New StringPairList(list), True)
    End Sub

    Sub WriteTable(list As StringPairList)
        WriteTable(Nothing, Nothing, list, True)
    End Sub

    Sub WriteTable(title As String, list As StringPairList)
        WriteTable(title, Nothing, list, True)
    End Sub

    Sub WriteTable(title As String, list As StringPairList, sort As Boolean)
        WriteTable(title, Nothing, list, sort)
    End Sub

    Sub WriteTable(title As String, text As String, list As StringPairList)
        WriteTable(title, text, list, False)
    End Sub

    Sub WriteTable(title As String, text As String, list As StringPairList, sort As Boolean)
        If sort Then
            list.Sort()
        End If

        If Not title Is Nothing Then
            Writer.WriteElementString("h2", title)
        End If

        If text Is Nothing Then
            Writer.WriteElementString("p", "")
        Else
            WriteParagraph(text, True)
        End If

        Writer.WriteStartElement("table")
        Writer.WriteAttributeString("border", "1")
        Writer.WriteAttributeString("cellspacing", "0")
        Writer.WriteAttributeString("cellpadding", "3")
        Writer.WriteAttributeString("bordercolordark", "white")
        Writer.WriteAttributeString("bordercolorlight", "black")

        Writer.WriteStartElement("col")
        Writer.WriteAttributeString("style", "width: 40%")
        Writer.WriteEndElement()

        Writer.WriteStartElement("col")
        Writer.WriteAttributeString("style", "width: 60%")
        Writer.WriteEndElement()

        For Each pair In list
            Writer.WriteStartElement("tr")
            Writer.WriteStartElement("td")
            WriteElement("p", HelpDocument.ConvertChars(pair.Name))
            Writer.WriteEndElement() 'td
            Writer.WriteStartElement("td")

            If pair.Value Is Nothing Then
                WriteElement("p", "&nbsp;")
            Else
                WriteElement("p", HelpDocument.ConvertChars(pair.Value))
            End If

            Writer.WriteEndElement() 'td

            Writer.WriteEndElement() 'tr
        Next

        Writer.WriteEndElement() 'table
    End Sub

    Sub WriteDocument()
        If Not IsClosed Then
            IsClosed = True

            Writer.WriteRaw("<p>&nbsp;</p>" + BR)
            Writer.WriteRaw("<h5 align=""center"">Copyright (C) 2002-" & DateTime.Now.Year & " StaxRip authors</h5><br>")
            Writer.WriteEndElement() 'body
            Writer.WriteEndElement() 'html
            Writer.Close()
        End If
    End Sub

    Sub WriteDocument(browser As WebBrowser)
        WriteDocument()
        browser.Navigate(Path)
    End Sub
End Class

<Serializable()>
Public Class SettingBag(Of T)
    Sub New()
    End Sub

    Sub New(value As T)
        Me.Value = value
    End Sub

    Overridable Property Value As T
End Class

Public Class FieldSettingBag(Of T)
    Inherits SettingBag(Of T)

    Private Obj As Object
    Private Name As String

    Sub New(obj As Object, fieldName As String)
        Me.Obj = obj
        Me.Name = fieldName
    End Sub

    Overrides Property Value() As T
        Get
            Return DirectCast(Obj.GetType.GetField(Name).GetValue(Obj), T)
        End Get
        Set(value As T)
            Obj.GetType.GetField(Name).SetValue(Obj, value)
        End Set
    End Property
End Class

Public Class ReflectionSettingBag(Of T)
    Inherits SettingBag(Of T)

    Private Obj As Object
    Private Name As String

    Sub New(obj As Object, name As String)
        Me.Obj = obj
        Me.Name = name
    End Sub

    Overrides Property Value() As T
        Get
            Dim field = Obj.GetType.GetField(Name, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)

            If field IsNot Nothing Then
                Return DirectCast(field.GetValue(Obj), T)
            Else
                Return DirectCast(Obj.GetType.GetProperty(Name).GetValue(Obj, Nothing), T)
            End If
        End Get
        Set(value As T)
            Dim f = Obj.GetType.GetField(Name)

            If f IsNot Nothing Then
                f.SetValue(Obj, value)
            Else
                Obj.GetType.GetProperty(Name).SetValue(Obj, value, Nothing)
            End If
        End Set
    End Property
End Class

<Serializable>
Public Class StringPair
    Implements IComparable(Of StringPair)

    Property Name As String
    Property Value As String

    Sub New()
    End Sub

    Sub New(name As String, value As String)
        Me.Name = name
        Me.Value = value
    End Sub

    Function CompareTo(other As StringPair) As Integer Implements IComparable(Of StringPair).CompareTo
        If Name IsNot Nothing Then
            '   Return Name.CompareTo(other.Name)
            Return String.CompareOrdinal(Name, other.Name)
        End If
    End Function
End Class

Public Class ErrorAbortException
    Inherits ApplicationException

    Property Title As String

    Sub New(title As String, message As String, Optional proj As Project = Nothing)
        MyBase.New(message)

        If proj Is Nothing Then
            proj = p
        End If

        Me.Title = title
        proj.Log.WriteHeader(title)
        proj.Log.WriteLine(message)
    End Sub
End Class

Public Class AbortException
    Inherits ApplicationException
End Class

Public Class SkipException
    Inherits ApplicationException
End Class

'Public Class CLIArg
'    Sub New(value As String)
'        Me.Value = value
'    End Sub
'    Property Value As String
'    'Shared Function GetArgs(args As String()) As CLIArg()
'    '    Dim ret(args.Length - 2) As CLIArg
'    '    For a = 1 To args.Length - 1
'    '        ret(a) = New CLIArg(args(a))
'    '    Next a
'    '    Return ret
'    'End Function
'    'Function IsMatch(ParamArray values As String()) As Boolean
'    '    For Each st As String In values
'    '        Dim val As String = Value
'    '        If String.Equals("-" & st, val, StringComparison.OrdinalIgnoreCase) OrElse String.Equals("/" & st, val, StringComparison.OrdinalIgnoreCase) OrElse
'    '            val.StartsWith("-" & st & ":", StringComparison.OrdinalIgnoreCase) OrElse val.StartsWith("/" & st & ":", StringComparison.OrdinalIgnoreCase) Then
'    '            Return True
'    '        End If
'    '    Next
'    'End Function
'    'Function GetInt() As Integer
'    '    Return CInt(Value.Right(":"))
'    'End Function
'    'Function GetString() As String
'    '    Return Value.Right(":").Trim(""""c)
'    'End Function
'    Function IsFile() As Boolean
'        Return File.Exists(Value)
'    End Function
'End Class

<Serializable()>
Public Class StringPairList
    Inherits List(Of StringPair)

    Sub New()
    End Sub

    Sub New(capacity As Integer)
        MyBase.New(capacity)
    End Sub

    Sub New(list As IEnumerable(Of StringPair))
        MyBase.New(list)
        'AddRange(list)
    End Sub

    Overloads Sub Add(name As String, text As String)
        Add(New StringPair(name, text))
    End Sub
End Class

<Serializable()>
Public Class CommandParameters
    Sub New(methodName As String, params As List(Of Object))
        Me.MethodName = methodName
        Parameters = params 'New List(Of Object)(params)
    End Sub

    Property MethodName As String
    Property Parameters As List(Of Object)

End Class

Public Class Command
    Implements IComparable(Of Command)

    Property Attribute As CommandAttribute
    Property CmdObject As Object
    Property MethodInfo As MethodInfo

    Sub New(methodInf As MethodInfo, cAttribute As CommandAttribute, cObject As Object)
        Attribute = cAttribute
        CmdObject = cObject
        MethodInfo = methodInf
    End Sub

    Function FixParameters(params As List(Of Object)) As List(Of Object)
        Dim checkedParams = GetDefaultParameters() 'As New List(Of Object)(GetDefaultParameters)

        If checkedParams.Count > 0 AndAlso params.Count > 0 Then
            Dim copiedParams As New CircularList(Of Object)(params)

            For i = 0 To checkedParams.Count - 1
                If copiedParams.Count > 0 AndAlso copiedParams(0) IsNot Nothing AndAlso checkedParams(i).GetType Is copiedParams(0).GetType Then
                    checkedParams(i) = copiedParams(0)
                    copiedParams.RemoveFirst()
                End If
            Next i
        End If

        Return checkedParams
    End Function

    Function GetDefaultParameters() As List(Of Object)
        Dim gParIA As ParameterInfo() = MethodInfo.GetParameters
        If gParIA.Length > 0 Then
            Dim l As New List(Of Object)(gParIA.Length) 'ToDo: Check Lenght  !!!!
            For Each iParameterInfo In gParIA
                Dim parT As Type = iParameterInfo.ParameterType
                Dim tStr As Boolean = parT Is GetType(String)
                Dim tVal As Boolean = parT.IsValueType

                If Not tVal AndAlso Not tStr Then
                    Throw New Exception("Methods must have string or value type params!:" & MethodInfo.Name & iParameterInfo.Name & parT.ToString)
                End If

                Dim a = DirectCast(iParameterInfo.GetCustomAttributes(GetType(DefaultValueAttribute), False), DefaultValueAttribute())

                If a.Length > 0 Then
                    l.Add(a(0).Value)
                Else
                    If tStr Then
                        l.Add(String.Empty)
                    ElseIf tVal Then
                        l.Add(Activator.CreateInstance(parT))
                    End If
                End If
            Next

            Return l
        End If
        Return New List(Of Object)
    End Function

    Overrides Function ToString() As String
        Return MethodInfo.Name
    End Function

    Function CompareTo(other As Command) As Integer Implements System.IComparable(Of Command).CompareTo
        Return String.CompareOrdinal(MethodInfo.Name, other.MethodInfo.Name)
    End Function

    Shared Sub PopulateCommandMenu(items As ToolStripItemCollection, commandsV As Dictionary(Of String, Command).ValueCollection, clickSub As Action(Of Command))
        Dim catSA As String() = {"Add", "Execute", "MediaInfo", "Save", "Set", "Show", "Start"} 'Sorted Order!!! No "Run" Command exists???
        Dim maxS = catSA.Length - 1
        Dim l2AL(maxS) As List(Of ActionMenuItem)
        Dim l1IdxA(maxS) As Integer
        Dim l1Tsi As New List(Of ToolStripMenuItemEx)(32)
        Dim l1HS As New HashSet(Of String)(7, StringComparer.Ordinal)
        Dim hsIdx As Integer
        Dim cmdsAr(commandsV.Count - 1) As Command
        commandsV.CopyTo(cmdsAr, 0)
        Array.Sort(cmdsAr)

        For c = 0 To cmdsAr.Length - 1
            Dim found As Integer
            Dim cmd = cmdsAr(c)
            Dim path As String = cmd.MethodInfo.Name
            For i = 0 To 1 'must be >= 1
                Dim SStr As String = catSA(hsIdx + i)
                If path.StartsWith(SStr, StringComparison.Ordinal) Then
                    Dim l2Tsi As List(Of ActionMenuItem)
                    If l1HS.Add(SStr) Then
                        hsIdx = l1HS.Count - 1
                        l1IdxA(hsIdx) = l1Tsi.Count
                        l1Tsi.Add(New ToolStripMenuItemEx(SStr))
                        l2Tsi = New List(Of ActionMenuItem)(If(hsIdx = 5, 38, 8)) 'or idx=4(Show) if no MediaInfo
                        l2AL(hsIdx) = l2Tsi
                    End If
                    l2Tsi.Add(New ActionMenuItem(path, Sub() clickSub(cmd), cmd.Attribute.Description))
                    found = 1
                    Exit For
                Else
                    found = 0
                    If hsIdx + i >= maxS Then Exit For
                End If
            Next i

            If found = 0 Then
                l1Tsi.Add(New ActionMenuItem(path, Sub() clickSub(cmd), cmd.Attribute.Description))
            End If
        Next c

        items.AddRange(l1Tsi.ToArray)
        For i = 0 To hsIdx
            Dim nMI As ToolStripMenuItemEx = l1Tsi(l1IdxA(i))
            nMI.DropDown.SuspendLayout()
            nMI.DropDownItems.AddRange(l2AL(i).ToArray) 'if not nothing l2al
            nMI.DropDown.ResumeLayout(False)
        Next i
    End Sub

    Function GetParameterHelp(parameters As List(Of Object)) As String
        If parameters.Count > 0 Then
            Dim paramList As New List(Of String)
            Dim params = MethodInfo.GetParameters
            Dim fixParL As List(Of Object) = FixParameters(parameters)

            For iParams = 0 To params.Length - 1
                Dim attributs = params(iParams).GetCustomAttributes(GetType(DispNameAttribute), False)

                If attributs.Length > 0 Then
                    paramList.Add("Parameter " & DirectCast(attributs(0), DispNameAttribute).DisplayName & ": " & fixParL.Item(iParams).ToString)
                End If
            Next

            Return String.Join(BR, paramList)
        End If
    End Function
End Class

<AttributeUsage(AttributeTargets.Method)>
Public Class CommandAttribute
    Inherits Attribute

    Sub New(description As String)
        Me.Description = description
    End Sub

    Property Description As String
End Class

Public Class CommandManager
    Property Commands As New Dictionary(Of String, Command)(89, StringComparer.Ordinal)
    'Function HasCommand(name As String) As Boolean
    '    If name.NullOrEmptyS Then Return False
    '    If Commands.ContainsKey(name) Then Return True
    '    For Each i In Commands.Keys
    '        If i.IsEqualIgnoreCase(name) Then Return True
    '    Next
    'End Function
    Function GetCommand(name As String) As Command
        If name?.Length > 0 Then
            Dim ret As Command
            If Commands.TryGetValue(name, ret) Then
                Return ret
            End If

            MsgInfo("Command Not found in Dict !!!", $"{name} CmdsDicCnt:{Commands.Count}") 'Debug, IF hit try change Command Dict Comp to OrdinalIgnoreCase or Fix MethodName !!!

            For Each i In Commands.Keys
                If String.Equals(i, name, StringComparison.OrdinalIgnoreCase) Then
                    Return Commands(i)
                End If
            Next i
        End If

        Return Nothing
    End Function

    Sub AddCommandsFromObject(obj As Object)
        For Each i In obj.GetType.GetMethods(BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
            Dim attributes = DirectCast(i.GetCustomAttributes(GetType(CommandAttribute), False), CommandAttribute())

            If attributes.Length > 0 Then
                AddCommand(New Command(i, attributes(0), obj))
            End If
        Next
    End Sub

    Sub AddCommand(command As Command)
        If Commands.ContainsKey(command.MethodInfo.Name) Then
            MsgWarn("Command '" & command.MethodInfo.Name & "' already exists.")
        Else
            Commands(command.MethodInfo.Name) = command
        End If
    End Sub

    Sub Process(cp As CommandParameters)
        If cp IsNot Nothing Then
            Process(cp.MethodName, cp.Parameters)
        End If
    End Sub

    Sub Process(name As String, params As List(Of Object))
        Dim cmd As Command = GetCommand(name)
        If cmd IsNot Nothing Then
            Process(cmd, params)
        End If
    End Sub

    Sub Process(command As Command, params As List(Of Object))
        Try
            command.MethodInfo.Invoke(command.CmdObject, command.FixParameters(params).ToArray)
        Catch ex As TargetParameterCountException
            MsgError("Parameter mismatch, for the command :" & command.MethodInfo.Name)
        Catch ex As Exception
            If TypeOf ex.InnerException IsNot AbortException Then
                g.ShowException(ex)
            End If
        End Try
    End Sub

    Function ProcessCommandLineArgument(value As String) As Boolean
        Dim valR As String = value.Right(":")

        For Each i As Command In Commands.Values
            Dim switch = "-" & i.MethodInfo.Name.Replace(" ", "")

            If String.Equals(value, switch, StringComparison.OrdinalIgnoreCase) Then
                Process(i.MethodInfo.Name, New List(Of Object))
                Return True
            Else
                If value.StartsWith(switch & ":", StringComparison.OrdinalIgnoreCase) Then
                    Dim mc = Regex.Matches(valR, """(?<a>.+?)""|(?<a>[^,]+)")
                    Dim args As New List(Of Object)(mc.Count)

                    For Each match As Match In mc
                        args.Add(match.Groups.Item("a").Value)
                    Next

                    Dim params = i.MethodInfo.GetParameters

                    For x = 0 To params.Length - 1
                        If args.Count > x Then
                            args(x) = TypeDescriptor.GetConverter(params(x).ParameterType).ConvertFrom(args(x))
                        Else
                            Exit For
                        End If
                    Next

                    Process(i.MethodInfo.Name, args)
                    Return True
                End If
            End If
        Next
    End Function

    Function GetTips() As StringPairList
        Dim l As New StringPairList

        For Each i As Command In Commands.Values
            If i.Attribute.Description IsNot Nothing Then
                l.Add(New StringPair(i.MethodInfo.Name, i.Attribute.Description))
            End If
        Next

        Return l
    End Function
End Class

Public Module MainModule
    Public Const BR As String = VB6.vbCrLf
    Public Const BR2 As String = VB6.vbCrLf & VB6.vbCrLf
    Public Const BR3 As String = VB6.vbCrLf & VB6.vbCrLf & VB6.vbCrLf
    Public Log As LogBuilder

    Sub MsgInfo(text As Object, Optional content As Object = Nothing, Optional dWidth As UInteger = 0)
        Dim text1 = text?.ToString
        Dim content1 = content?.ToString
        Msg(text1, content1, MsgIcon.Info, TaskDialogButtons.Ok, dWidth:=dWidth)
    End Sub

    Sub MsgError(text As String, Optional content As String = Nothing, Optional dWidth As UInteger = 0)
        MsgError(text, content, IntPtr.Zero, dWidth)
    End Sub

    Sub MsgError(text As String, content As String, handle As IntPtr, Optional dWidth As UInteger = 0)
        If text.NullOrEmptyS Then
            text = content
        End If

        If text.NullOrEmptyS Then
            Exit Sub
        End If

        Using td As New TaskDialog(Of String)(dWidth)
            If content.NullOrEmptyS Then
                If text.Length < 400 Then
                    td.MainInstruction = text
                Else
                    td.Content = text
                End If
            Else
                td.MainInstruction = text
                td.Content = content
            End If

            If handle <> IntPtr.Zero Then
                td.Config.hwndParent = handle
            End If

            td.AllowCancel = False
            td.MainIcon = TaskDialogIcon.Error
            td.Footer = Strings.TaskDialogFooter
            td.Show()
        End Using
    End Sub

    Private ShownMessages As String

    Sub MsgWarn(text As String, Optional content As String = Nothing, Optional onlyOnce As Boolean = False, Optional dWidth As UInteger = 0)
        If onlyOnce AndAlso ShownMessages?.Contains(text & content) Then
            Exit Sub
        End If

        Msg(text, content, MsgIcon.Warning, TaskDialogButtons.Ok, dWidth:=dWidth)

        If onlyOnce Then
            ShownMessages &= text & content
        End If
    End Sub

    Function MsgOK(text As String, Optional dWidth As UInteger = 0) As Boolean
        Return Msg(text, Nothing, MsgIcon.Question, TaskDialogButtons.OkCancel, dWidth:=dWidth) = DialogResult.OK
    End Function

    Function MsgQuestion(text As String, Optional buttons As TaskDialogButtons = TaskDialogButtons.OkCancel, Optional dWidth As UInteger = 0) As DialogResult
        Return Msg(text, Nothing, MsgIcon.Question, buttons, dWidth:=dWidth)
    End Function

    Function MsgQuestion(heading As String, content As String, Optional buttons As TaskDialogButtons = TaskDialogButtons.OkCancel, Optional dWidth As UInteger = 0) As DialogResult
        Return Msg(heading, content, MsgIcon.Question, buttons, dWidth:=dWidth)
    End Function

    Function Msg(mainInstruction As String, content As String, icon As MsgIcon, buttons As TaskDialogButtons,
                 Optional defaultButton As DialogResult = DialogResult.None, Optional dWidth As UInteger = 0) As DialogResult

        If mainInstruction Is Nothing Then
            mainInstruction = ""
        End If

        Using td As New TaskDialog(Of DialogResult)(dWidth)
            td.AllowCancel = False
            td.DefaultButton = defaultButton

            If content Is Nothing Then
                If mainInstruction.Length < 400 Then '80, more(400) is also OK, only needs BR width is limited
                    td.MainInstruction = mainInstruction
                Else
                    td.Content = mainInstruction
                End If
            Else
                td.MainInstruction = mainInstruction
                td.Content = content
            End If

            Select Case icon
                Case MsgIcon.Error
                    td.MainIcon = TaskDialogIcon.Error
                Case MsgIcon.Warning
                    td.MainIcon = TaskDialogIcon.Warning
                Case MsgIcon.Info
                    td.MainIcon = TaskDialogIcon.Info
            End Select

            If buttons = TaskDialogButtons.OkCancel Then
                td.AddButton("OK", DialogResult.OK)
                td.AddButton("Cancel", DialogResult.Cancel) 'don't use system language
            Else
                td.CommonButtons = buttons
            End If

            Return td.Show()
        End Using
    End Function
End Module

Public Class Reflector
    Public Type As Type
    Private BasicFlags As BindingFlags = BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic

    Sub New(obj As Object)
        Me.Value = obj
    End Sub

    Sub New(obj As Object, type As Type)
        Me.Value = obj
        Me.Type = type
    End Sub

    Sub New(progID As String)
        Me.New(Activator.CreateInstance(Type.GetTypeFromProgID(progID)))
    End Sub

    Private ValueValue As Object

    Property Value() As Object
        Get
            Return ValueValue
        End Get
        Set(Value As Object)
            ValueValue = Value

            If Not Value Is Nothing Then
                Type = Value.GetType
            End If
        End Set
    End Property

    Function ToBool() As Boolean
        Return DirectCast(Value, Boolean)
    End Function

    Function ToInt() As Integer
        Return DirectCast(Value, Integer)
    End Function

    Overrides Function ToString() As String
        Return DirectCast(Value, String)
    End Function

    Function Cast(Of T)() As T
        Return DirectCast(Value, T)
    End Function

    Function Invoke(name As String) As Reflector
        'returns overloads as single MemberInfo
        For Each i In Type.GetMember(name, BasicFlags)
            Select Case i.MemberType
                Case MemberTypes.Property
                    Return Invoke(name, BasicFlags Or BindingFlags.GetProperty, New Object() {})
                Case MemberTypes.Field
                    Return Invoke(name, BasicFlags Or BindingFlags.GetField, New Object() {})
                Case MemberTypes.Method
                    Return Invoke(name, BasicFlags Or BindingFlags.InvokeMethod, New Object() {})
            End Select
        Next

        Return Invoke(name, BindingFlags.InvokeMethod) 'COM
    End Function

    Function Invoke(name As String, ParamArray args As Object()) As Reflector
        'returns overloads as single MemberInfo
        For Each i In Type.GetMember(name, BasicFlags)
            Select Case i.MemberType
                Case MemberTypes.Property
                    Return Invoke(name, BasicFlags Or BindingFlags.SetProperty, args)
                Case MemberTypes.Field
                    Return Invoke(name, BasicFlags Or BindingFlags.SetField, args)
                Case MemberTypes.Method
                    Return Invoke(name, BasicFlags Or BindingFlags.InvokeMethod, args)
                Case Else
                    Throw New NotImplementedException
            End Select
        Next

        Return Invoke(name, BindingFlags.InvokeMethod, args) 'COM 
    End Function

    Function Invoke(name As String, flags As BindingFlags, ParamArray args As Object()) As Reflector
        Return New Reflector(Type.InvokeMember(name, flags, Nothing, Value, args))
    End Function
End Class

Public Class Shutdown
    Shared Sub Commit(mode As ShutdownMode)
        Dim psi = New ProcessStartInfo("shutdown.exe")
        psi.CreateNoWindow = True
        psi.UseShellExecute = False

        Select Case mode
            Case ShutdownMode.Standby
                SetSuspendState(False, False, False)
            Case ShutdownMode.Hibernate
                SetSuspendState(True, False, False)
            Case ShutdownMode.Hybrid
                psi.Arguments = "/f /hybrid /t " & s.ShutdownTimeout
                Process.Start(psi)?.Dispose()
            Case ShutdownMode.Shutdown
                psi.Arguments = "/f /s /t " & s.ShutdownTimeout
                Process.Start(psi)?.Dispose()
        End Select
    End Sub

    Declare Function SetSuspendState Lib "powrprof.dll" (hibernate As Boolean, forceCritical As Boolean, disableWakeEvent As Boolean) As Boolean
End Class

Public Enum ShutdownMode
    [Nothing]
    Close
    Standby
    Hibernate
    Hybrid
    Shutdown
End Enum

Public Enum ToolStripRenderModeEx
    <DispName("System Theme Color")> SystemAuto
    <DispName("System Default Color")> SystemDefault
    <DispName("Win 10 Theme Color")> Win10Auto
    <DispName("Win 10 Default Color")> Win10Default
End Enum

Public Class PowerRequest
    Private Shared CurrentPowerRequest As IntPtr

    Shared Sub SuppressStandby()
        If CurrentPowerRequest <> IntPtr.Zero Then
            PowerClearRequest(CurrentPowerRequest, PowerRequestType.PowerRequestSystemRequired)
            CurrentPowerRequest = IntPtr.Zero
        End If

        Dim pContext As POWER_REQUEST_CONTEXT
        pContext.Flags = &H1 'POWER_REQUEST_CONTEXT_SIMPLE_STRING
        pContext.Version = 0 'POWER_REQUEST_CONTEXT_VERSION
        pContext.SimpleReasonString = "Standby suppressed by StaxRip"  'shown when the command "powercfg -requests" is executed

        CurrentPowerRequest = PowerCreateRequest(pContext)

        If CurrentPowerRequest = IntPtr.Zero Then
            Dim err = Marshal.GetLastWin32Error()

            If err <> 0 Then
                Throw New Win32Exception(err)
            End If
        End If

        Dim success = PowerSetRequest(CurrentPowerRequest, PowerRequestType.PowerRequestSystemRequired)

        If Not success Then
            CurrentPowerRequest = IntPtr.Zero
            Dim err = Marshal.GetLastWin32Error()

            If err <> 0 Then
                Throw New Win32Exception(err)
            End If
        End If
    End Sub

    Shared Sub EnableStandby()
        If CurrentPowerRequest <> IntPtr.Zero Then
            Dim success = PowerClearRequest(CurrentPowerRequest, PowerRequestType.PowerRequestSystemRequired)

            If Not success Then
                CurrentPowerRequest = IntPtr.Zero
                Dim err = Marshal.GetLastWin32Error()

                If err <> 0 Then
                    Throw New Win32Exception(err)
                End If
            Else
                CurrentPowerRequest = IntPtr.Zero
            End If
        End If
    End Sub

    Enum PowerRequestType
        PowerRequestDisplayRequired
        PowerRequestSystemRequired
        PowerRequestAwayModeRequired
        PowerRequestExecutionRequired
    End Enum

    <DllImport("kernel32.dll")>
    Shared Function PowerCreateRequest(ByRef Context As POWER_REQUEST_CONTEXT) As IntPtr
    End Function

    <DllImport("kernel32.dll")>
    Shared Function PowerSetRequest(PowerRequestHandle As IntPtr, RequestType As PowerRequestType) As Boolean
    End Function

    <DllImport("kernel32.dll")>
    Shared Function PowerClearRequest(PowerRequestHandle As IntPtr, RequestType As PowerRequestType) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Structure POWER_REQUEST_CONTEXT
        Public Version As UInt32
        Public Flags As UInt32
        <MarshalAs(UnmanagedType.LPWStr)>
        Public SimpleReasonString As String
    End Structure
End Class
