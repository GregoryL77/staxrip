﻿
Imports System.ComponentModel
Imports System.Reflection
Imports System.Text
Imports StaxRip.UI

Public Class Documentation
    Shared Sub GenerateDynamicFiles()
        GenerateMacroTableFile()
        GenerateToolFile()
        GenerateScreenshotsFile()

        UpdateFile(Folder.Startup + "..\docs\generated\switches.rst", GetCommands(True))
        UpdateFile(Folder.Startup + "..\docs\generated\commands.rst", GetCommands(False))
    End Sub

    Shared Function GetCommands(cli As Boolean) As String
        Dim sb As New StringBuilder

        Dim commands As New List(Of Command)(g.MainForm.CommandManager.Commands.Values)
        commands.Sort()

        For Each command In commands
            If command.Attribute.Description.StartsWith("This command is obsolete", StringComparison.Ordinal) Then
                Continue For
            End If

            Dim params = command.MethodInfo.GetParameters
            Dim title = command.MethodInfo.Name

            If cli Then
                title = "-" + title + ":"

                For Each param In params
                    title += param.Name + ","
                Next

                title = title.TrimEnd(",:".ToCharArray)
            End If

            sb.Append(".. option:: ").Append(title).Append(BR2)
            sb.Append(command.Attribute.Description.IndentLines("    ")).Append(BR2)

            If params.Length > 0 Then
                sb.Append(".. list-table::" & BR)
                sb.Append("    :widths: auto" & BR2)

                Dim hasDescription = False

                For Each param In params
                    Dim descAttrib = param.GetCustomAttribute(Of DescriptionAttribute)

                    If Not descAttrib Is Nothing AndAlso descAttrib.Description.NotNullOrEmptyS Then
                        hasDescription = True
                    End If

                    Dim nameAttrib = param.GetCustomAttribute(Of DispNameAttribute)

                    If Not nameAttrib Is Nothing AndAlso
                        Not nameAttrib.DisplayName.IsEqualIgnoreCase(param.Name) Then

                        hasDescription = True
                    End If
                Next

                For Each param In params
                    sb.Append("    * - ").Append(param.Name).Append(" <").Append(param.ParameterType.Name.ToLower.Replace("int32", "integer")).Append(">").Append({BR})

                    If hasDescription OrElse param.ParameterType.IsEnum Then
                        sb.Append("      - ")

                        Dim nameAttrib = param.GetCustomAttribute(Of DispNameAttribute)
                        Dim hasName = False

                        If Not nameAttrib Is Nothing AndAlso
                            Not nameAttrib.DisplayName.IsEqualIgnoreCase(param.Name) Then
                            sb.Append(nameAttrib.DisplayName)
                            hasName = True
                        End If

                        Dim descAttrib = param.GetCustomAttribute(Of DescriptionAttribute)

                        If Not descAttrib Is Nothing Then
                            If hasName Then
                                sb.Append(": ")
                            End If

                            sb.Append(descAttrib.Description)
                            descAttrib.Description.ThrowIfContainsNewLine
                        End If

                        If param.ParameterType.IsEnum Then
                            sb.Append(" ").Append(System.Enum.GetNames(param.ParameterType).Join(", "))
                        End If

                        sb.Append(BR)
                    End If
                Next

                sb.Append(BR)
            End If
        Next

        Return sb.ToString
    End Function

    Shared Sub GenerateMacroTableFile()
        Dim text =
            ".. csv-table::" + BR +
            "    :header: ""Name"", ""Description""" + BR +
            "    :widths: auto" + BR2 +
            "    " + g.ConvertToCSV(",", Macro.GetTips(False, True, False)).Right(BR).Right(BR).Replace(BR, BR + "    ")

        UpdateFile(Folder.Startup + "..\docs\generated\macro-table.rst", text)
    End Sub

    Shared Sub GenerateToolFile()
        Dim sb As New StringBuilder
        sb.Append("Tools" & BR & "=====" & BR2)

        Dim rows As New List(Of Object)

        For Each pack In Package.Items.Values.OrderBy(Function(i) i.GetTypeName)
            Dim row = New With {
                .Name = "", .Type = "", .Filename = "", .Version = "", .ModifiedDate = ""}

            row.Name = pack.Name
            row.Type = pack.GetTypeName
            row.Filename = pack.Filename

            If pack.IsVersionCorrect Then
                row.Version = pack.Version
            End If

            If File.Exists(pack.Path) Then
                row.ModifiedDate = File.GetLastWriteTime(pack.Path).ToString("yyyy-MM-dd")
            End If

            rows.Add(row)
        Next

        Dim text =
            ".. csv-table::" + BR +
            "    :header: ""Name"", ""Type"", ""Filename"", ""Version"", ""Modified Date""" + BR +
            "    :widths: auto" + BR2 +
            "    " + g.ConvertToCSV(",", rows).Right(BR).Right(BR).Replace(BR, BR + "    ")

        sb.Append(text).Append(BR2)

        sb.Append("Console App" & BR & "-----------" & BR)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "Console App" Then
                sb.Append(pack.Name).Append(BR).Append("~".Multiply(pack.Name.Length)).Append(BR2)
                sb.Append(pack.Description).Append(BR2)
                sb.Append(pack.WebURL).Append(BR2 & BR)
            End If
        Next

        sb.Append("GUI App" & BR & "-------" & BR)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "GUI App" Then
                sb.Append(pack.Name).Append(BR).Append("~".Multiply(pack.Name.Length)).Append(BR2)
                sb.Append(pack.Description).Append(BR2)
                sb.Append(pack.WebURL).Append(BR2 & BR)
            End If
        Next

        sb.Append("AviSynth Plugin" & BR & "---------------" & BR)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "AviSynth Plugin" Then
                sb.Append(pack.Name).Append(BR).Append("~".Multiply(pack.Name.Length)).Append(BR2)
                sb.Append(pack.Description).Append(BR2)

                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.AvsFilterNames.NothingOrEmpty Then
                    sb.Append("Filters: ").Append(plugin.AvsFilterNames.Join(", ")).Append(BR2)
                End If

                sb.Append(pack.WebURL).Append(BR2 & BR)
            End If
        Next

        sb.Append("AviSynth Script" & BR & "---------------" & BR)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "AviSynth Script" Then
                sb.Append(pack.Name).Append(BR).Append("~".Multiply(pack.Name.Length)).Append(BR2)
                sb.Append(pack.Description).Append(BR2)

                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.AvsFilterNames.NothingOrEmpty Then
                    sb.Append("Filters: ").Append(plugin.AvsFilterNames.Join(", ")).Append(BR2)
                End If

                sb.Append(pack.WebURL).Append(BR2 & BR)
            End If
        Next

        sb.Append("VapourSynth Plugin" & BR & "------------------" & BR)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "VapourSynth Plugin" Then
                sb.Append(pack.Name).Append(BR).Append("~".Multiply(pack.Name.Length)).Append(BR2)
                sb.Append(pack.Description).Append(BR2)

                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.VSFilterNames.NothingOrEmpty Then
                    sb.Append("Filters: ").Append(plugin.VSFilterNames.Join(", ")).Append(BR2)
                End If

                sb.Append(pack.WebURL).Append(BR2 & BR)
            End If
        Next

        sb.Append("VapourSynth Script" & BR & "------------------" & BR)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "VapourSynth Script" Then
                sb.Append(pack.Name).Append(BR).Append("~".Multiply(pack.Name.Length)).Append(BR2)
                sb.Append(pack.Description).Append(BR2)

                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.VSFilterNames.NothingOrEmpty Then
                    sb.Append("Filters: ").Append(plugin.VSFilterNames.Join(", ")).Append(BR2)
                End If

                sb.Append(pack.WebURL).Append(BR2 & BR)
            End If
        Next

        UpdateFile(Folder.Startup & "..\docs\generated\tools.rst", sb.ToString)
    End Sub

    Shared Sub GenerateScreenshotsFile()
        Dim screenshots = "Screenshots" + BR + "===========" + BR2 + ".. contents::" + BR2
        Dim screenshotFiles = Directory.GetFiles(Folder.Startup + "..\docs\screenshots").ToList
        screenshotFiles.Sort(New StringLogicalComparer)

        For Each i In screenshotFiles
            Dim name = i.Base.Replace("_", " ").Trim
            screenshots += name + BR + "-".Multiply(name.Length) + BR2 + ".. image:: ../screenshots/" + i.FileName + BR2
        Next

        UpdateFile(Folder.Startup + "..\docs\generated\screenshots.rst", screenshots)
    End Sub

    Shared Sub UpdateFile(filepath As String, content As String)
        Dim currentContent = filepath.ReadAllText

        If content <> currentContent Then
            content.WriteFileUTF8(filepath)
        End If
    End Sub

    Shared Sub ShowTip(message As String)
        Dim hash = message.MD5Hash

        If Not s.Storage.GetBool(hash) Then
            MsgInfo("Tip", message)
            s.Storage.SetBool(hash, True)
        End If
    End Sub
End Class
