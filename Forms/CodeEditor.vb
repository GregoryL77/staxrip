
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports JM.LinqFaster
Imports Microsoft.Win32

Imports StaxRip.UI

Public Class CodeEditor
    Property ActiveTable As FilterTable
    Property Engine As ScriptEngine

    Sub New(doc As VideoScript)
        InitializeComponent()

        Me.tlpMain.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.MainFlowLayoutPanel.SuspendLayout()
        Me.SuspendLayout()
        MainFlowLayoutPanel.Padding = New Padding(0, 0, 0, 0)

        Width = 1280
        Height = 464
        FormBorderStyle = FormBorderStyle.Sizable
        Engine = doc.Engine

        For Each i In doc.Filters
            MainFlowLayoutPanel.Controls.Add(CreateFilterTable(i))
        Next

        'AutoSizeMode = AutoSizeMode.GrowAndShrink
        AutoSizeMode = AutoSizeMode.GrowOnly
        AutoSize = True
        KeyPreview = True
        AddHandler MainFlowLayoutPanel.Layout, AddressOf MainFlowLayoutPanelLayout

    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Me.tlpMain.ResumeLayout(True)
        Me.FlowLayoutPanel1.ResumeLayout(True)
        Me.MainFlowLayoutPanel.ResumeLayout(True)
        Me.ResumeLayout(True)
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Select Case e.KeyData
            Case Keys.F5
                VideoPreview()
            Case Keys.F9
                PlayScriptWithMPV()
            Case Keys.F10
                PlayScriptWithMPC()
            Case Keys.Control Or Keys.Delete
                If Not ActiveTable Is Nothing Then
                    ActiveTable.RemoveClick()
                End If
            Case Keys.Control Or Keys.Up
                If Not ActiveTable Is Nothing Then
                    ActiveTable.MoveUp()
                End If
            Case Keys.Control Or Keys.Down
                If Not ActiveTable Is Nothing Then
                    ActiveTable.MoveDown()
                End If
            Case Keys.Control Or Keys.I
                ShowInfo()
            Case Keys.Control Or Keys.J
                JoinFilters()
            Case Keys.Control Or Keys.P
                g.MainForm.ShowFilterProfilesDialog()
            Case Keys.Control Or Keys.M
                MacrosForm.ShowDialogForm()
        End Select

        MyBase.OnKeyDown(e)
    End Sub

    Shared Function CreateFilterTable(filter As VideoFilter) As FilterTable
        Dim ret As New FilterTable

        ret.Margin = New Padding(0)
        ret.Size = New Size(950, 50)
        ret.cbActive.Checked = filter.Active
        ret.cbActive.Text = filter.Category
        ret.tbName.Text = filter.Name

        Dim fh As Integer = ret.Font.Height 'Opt.16 Disabled OnLayout in FilterTable Class 'was 16(Sagoe) or 13 in filterTable class
        ret.tbName.Height = CInt(fh * 1.25) '1.2
        ret.tbName.Width = fh * 8 '7

        ret.rtbScript.Text = If(filter.Script.NullOrEmptyS, "", filter.Script + BR)
        ret.SetColor()

        Return ret
    End Function

    Function GetFilters() As List(Of VideoFilter)
        Dim ret As New List(Of VideoFilter)

        For Each table As FilterTable In MainFlowLayoutPanel.Controls
            Dim filter As New VideoFilter()
            filter.Active = table.cbActive.Checked
            filter.Category = table.cbActive.Text
            filter.Path = table.tbName.Text
            filter.Script = table.rtbScript.Text.FixBreak.Trim
            ret.Add(filter)
        Next

        Return ret
    End Function

    Function CreateTempScript() As VideoScript
        Dim script As New VideoScript
        script.Engine = Engine
        script.Path = p.TempDir + p.TargetFile.Base + $"_temp." + script.FileType
        script.Filters = GetFilters()

        If script.GetError.NotNullOrEmptyS Then
            MsgError("Script Error", script.GetError)
            Exit Function
        End If

        Return script
    End Function

    Sub PlayScriptWithMPV()
        g.PlayScriptWithMPV(CreateTempScript())
    End Sub

    Sub PlayScriptWithMPC()
        g.PlayScriptWithMPC(CreateTempScript())
    End Sub

    Sub VideoPreview()
        If p.SourceFile.NullOrEmptyS Then
            Exit Sub
        End If

        Dim script = CreateTempScript()

        If script Is Nothing Then
            Exit Sub
        End If

        script.RemoveFilter("Cutting")

        Dim form As New PreviewForm(script)
        form.Owner = g.MainForm
        form.Show()
    End Sub

    Sub ShowInfo()
        Dim script = CreateTempScript()

        If Not script Is Nothing Then
            g.ShowScriptInfo(script)
        End If
    End Sub

    Sub ShowAdvancedInfo()
        Dim script = CreateTempScript()

        If script Is Nothing Then
            Exit Sub
        End If

        g.ShowAdvancedScriptInfo(script)
    End Sub

    Sub JoinFilters()
        Dim firstTable = DirectCast(MainFlowLayoutPanel.Controls(0), FilterTable)
        firstTable.tbName.Text = "merged"
        firstTable.rtbScript.Text = MainFlowLayoutPanel.Controls.OfType(Of FilterTable).ToArray.SelectF(Function(arg) If(arg.cbActive.Checked, arg.rtbScript.Text.Trim, "#" + arg.rtbScript.Text.Trim.FixBreak.Replace(BR, "# " + BR))).Join(BR) + BR2 + BR2

        For x = MainFlowLayoutPanel.Controls.Count - 1 To 1 Step -1
            MainFlowLayoutPanel.Controls.RemoveAt(x)
        Next
    End Sub

    Sub MainFlowLayoutPanelLayout(sender As Object, e As LayoutEventArgs)
        'SW.Restart()
        Dim filterTables = MainFlowLayoutPanel.Controls.OfType(Of FilterTable).ToArray

        If filterTables.Length <= 0 Then 'Any
            Exit Sub
        End If
        'Dim maxTextWidth = Aggregate i In filterTables Into Max(i.TrimmedTextSize.Width)
        Dim maxTextWidth As Integer  '= filterTables.MaxF(Function(i) i.TrimmedTextSize.Width)
        Dim ttsA(filterTables.Length - 1) As Size
        For f = 0 To filterTables.Length - 1
            ttsA(f) = filterTables(f).TrimmedTextSize
            If ttsA(f).Width > maxTextWidth Then maxTextWidth = ttsA(f).Width
        Next f

        Dim fh As Integer = FontHeight
        For f = 0 To filterTables.Length - 1
            Dim sizeRTB As Size
            sizeRTB.Width = maxTextWidth + fh
            sizeRTB.Height = ttsA(f).Height + CInt(fh * 0.3)
            filterTables(f).rtbScript.Size = sizeRTB
            'filterTables(f).rtbScript.Refresh()
            filterTables(f).rtbScript.Invalidate()
        Next f
    End Sub

    Sub CodeEditor_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteTable("Macros", Macro.GetTips(False, True, False))
        form.Show()
    End Sub

    Public Class FilterTable
        Inherits TableLayoutPanel

        Property tbName As New TextEdit
        Property rtbScript As RichTextBoxEx
        Property cbActive As New CheckBox
        Property Menu As New ContextMenuStripEx
        Property LastTextSize As Size
        Property Editor As CodeEditor

        Sub New()
            AutoSize = True

            cbActive.AutoSize = True
            cbActive.Anchor = AnchorStyles.Left Or AnchorStyles.Right
            cbActive.Margin = New Padding(0)

            tbName.Dock = DockStyle.Top
            tbName.Margin = New Padding(0, 0, 0, 0)

            rtbScript = New RichTextBoxEx(False)
            rtbScript.EnableAutoDragDrop = True
            rtbScript.Dock = DockStyle.Fill
            rtbScript.WordWrap = False
            rtbScript.ScrollBars = RichTextBoxScrollBars.None
            rtbScript.AcceptsTab = True
            rtbScript.Margin = New Padding(0)
            rtbScript.Font = New Font("Consolas", 10 * s.UIScaleFactor)

            AddHandler Disposed, Sub()
                                     Menu?.Items.ClearAndDisplose
                                     Menu?.Dispose()
                                     rtbScript?.Font?.Dispose()
                                 End Sub
            AddHandler cbActive.CheckedChanged, Sub() SetColor()
            AddHandler rtbScript.MouseUp, AddressOf HandleMouseUp
            AddHandler rtbScript.Enter, Sub() Editor.ActiveTable = Me
            AddHandler rtbScript.TextChanged, Sub()
                                                  If Parent Is Nothing Then Exit Sub
                                                  Dim maxTextWidth = Parent.Controls.OfType(Of FilterTable)().ToArray.MaxF(Function(i) i.TrimmedTextSize.Width)

                                                  Dim textSizeVar = TrimmedTextSize

                                                  If textSizeVar.Width > maxTextWidth OrElse (textSizeVar.Width = maxTextWidth AndAlso textSizeVar.Width <> LastTextSize.Width) OrElse
                                                  LastTextSize.Height <> textSizeVar.Height AndAlso textSizeVar.Height > FontHeight Then
                                                      LastTextSize = textSizeVar
                                                      Task.Run(Sub()
                                                                   Thread.Sleep(150)
                                                                   BeginInvoke(Sub() Parent.PerformLayout())
                                                               End Sub)
                                                  End If
                                              End Sub
            ColumnCount = 2
            ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            RowCount = 1
            RowStyles.Add(New RowStyle(SizeType.AutoSize))

            Dim t As New TableLayoutPanel
            t.AutoSize = True
            t.SuspendLayout()
            t.Dock = DockStyle.Fill
            t.Margin = New Padding(0)
            t.ColumnCount = 1
            t.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            t.RowCount = 2
            t.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            t.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            t.Controls.Add(cbActive, 0, 0)
            t.Controls.Add(tbName, 0, 1)
            't.ResumeLayout()

            Controls.Add(t, 0, 0)
            Controls.Add(rtbScript, 1, 0)

            t.ResumeLayout()

        End Sub

        'Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
        '    Dim fh As Integer = FontHeight
        '    tbName.Height = CInt(fh * 1.2)
        '    tbName.Width = fh * 8 '7
        '    MyBase.OnLayout(levent)
        'End Sub

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            Menu.Form = FindForm()
            Editor = DirectCast(Menu.Form, CodeEditor)
            MyBase.OnHandleCreated(e)
        End Sub

        Sub SetColor()
            If cbActive.Checked Then
                rtbScript.ForeColor = Color.Black
                tbName.TextBox.ForeColor = Color.Black
                cbActive.ForeColor = Color.Black
            Else
                rtbScript.ForeColor = Color.Gray
                tbName.TextBox.ForeColor = Color.Gray
                cbActive.ForeColor = Color.Gray
            End If
        End Sub
        'ReadOnly Property TextSize As Size
        '    Get
        '        Return TextRenderer.MeasureText(rtbScript.Text, rtbScript.Font, New Size(10000, 10000))
        '    End Get
        'End Property
        'ReadOnly Property MaxTextSizeWH As (Integer, Integer)
        '    Get
        '        Return (Font.Height * 108, Font.Height * 15)
        '    End Get
        'End Property
        'ReadOnly Property MaxTextHeight As Integer
        '    Get
        '        Return Font.Height * 15
        '    End Get
        'End Property

        ReadOnly Property TrimmedTextSize As Size
            Get
                Dim ret = TextRenderer.MeasureText(rtbScript.Text, rtbScript.Font, New Size(100000, 100000))
                Dim fontH As Integer = Font.Height
                Dim maxFW As Integer = fontH * 104 '108-96 fh=16 'Or: Screen.PrimaryScreen.Bounds
                Dim maxFH As Integer = fontH * 15

                If ret.Width > maxFW Then
                    ret.Width = maxFW
                End If

                If ret.Height > maxFH Then
                    ret.Height = maxFH
                End If

                Return ret
            End Get
        End Property

        Sub SetParameters(parameters As FilterParameters)
            Dim code = rtbScript.Text.FixBreak
            Dim match = Regex.Match(code, parameters.FunctionName + "\((.+)\)")
            Dim args = FilterParameters.SplitCSV(match.Groups(1).Value)
            Dim newParameters As New List(Of String)

            For Each argument In args
                Dim skip = False
                Dim arg As String = argument.ToLowerInvariant.Replace(" ", "")

                For Each parameter In parameters.Parameters
                    If arg.Contains(parameter.Name.ToLowerInvariant.Replace(" ", "") & "=") Then
                        skip = True
                    End If
                Next

                If Not skip Then
                    newParameters.Add(argument)
                End If
            Next

            For Each parameter In parameters.Parameters
                Dim value = parameter.Value

                If Editor.Engine = ScriptEngine.VapourSynth Then
                    value = value.Replace("""", "'")
                End If

                newParameters.Add(parameter.Name + "=" + value)
            Next

            rtbScript.Text = Regex.Replace(code, parameters.FunctionName + "\((.+)\)",
                                           parameters.FunctionName + "(" + newParameters.Join(", ") + ")")
        End Sub

        Sub HandleMouseUp(sender As Object, e As MouseEventArgs)
            If e.Button <> MouseButtons.Right Then
                Exit Sub
            End If

            Menu.SuspendLayout()
            Menu.Items.ClearAndDisplose
            Dim filterProfiles = If(p.Script.Engine = ScriptEngine.AviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
            Dim code = rtbScript.Text.FixBreak

            For Each i In FilterParameters.Definitions
                If code.Contains(i.FunctionName + "(") Then
                    Dim match = Regex.Match(code, i.FunctionName + "\((.+)\)")

                    If match.Success Then
                        ActionMenuItem.Add(Menu.Items, i.Text, AddressOf SetParameters, i)
                    End If
                End If
            Next

            For Each i In filterProfiles
                If i.Name = cbActive.Text Then
                    Dim cat = i
                    Dim catMenuItem = Menu.Add(i.Name)

                    For Each iFilter In cat.Filters
                        Dim tip = iFilter.Script
                        ActionMenuItem.Add(Menu.Items, If(cat.Filters.Count > 1, iFilter.Category + " | ", "") + iFilter.Path, AddressOf ReplaceClick, iFilter.GetCopy, tip)
                    Next
                End If
            Next

            Dim filterMenuItem = Menu.Add("Add, insert or replace a filter   ")
            filterMenuItem.SetImage(Symbol.Filter)

            ActionMenuItem.Add(filterMenuItem.DropDownItems, "Empty Filter", AddressOf FilterClick, New VideoFilter("Misc", "", ""), "Filter with empty values.")

            For Each filterCategory In filterProfiles
                For Each filter In filterCategory.Filters
                    Dim tip = filter.Script
                    ActionMenuItem.Add(filterMenuItem.DropDownItems, filterCategory.Name + " | " + filter.Path, AddressOf FilterClick, filter.GetCopy, tip)
                Next
            Next

            Dim removeMenuItem = Menu.Add("Remove", AddressOf RemoveClick)
            removeMenuItem.KeyDisplayString = "Ctrl+Delete"
            removeMenuItem.SetImage(Symbol.Remove)

            Dim previewMenuItem = Menu.Add("Preview Video...", AddressOf Editor.VideoPreview, "Previews the script with solved macros.")
            previewMenuItem.Enabled = p.SourceFile.NotNullOrEmptyS
            previewMenuItem.KeyDisplayString = "F5"
            previewMenuItem.SetImage(Symbol.Photo)

            Dim mpvnetMenuItem = Menu.Add("Play with mpv.net", AddressOf Editor.PlayScriptWithMPV, "Plays the current script with mpv.net.")
            mpvnetMenuItem.Enabled = p.SourceFile.NotNullOrEmptyS
            mpvnetMenuItem.KeyDisplayString = "F9"
            mpvnetMenuItem.SetImage(Symbol.Play)

            Dim mpcMenuItem = Menu.Add("Play with mpc", AddressOf Editor.PlayScriptWithMPC, "Plays the current script with MPC.")
            mpcMenuItem.Enabled = p.SourceFile.NotNullOrEmptyS
            mpcMenuItem.KeyDisplayString = "F10"
            mpcMenuItem.SetImage(Symbol.Play)

            Menu.Add("Preview Code...", AddressOf CodePreview, "Previews the script with solved macros.").SetImage(Symbol.Code)

            Dim infoMenuItem = Menu.Add("Info...", AddressOf Editor.ShowInfo, "Previews script parameters such as framecount and colorspace.")
            infoMenuItem.SetImage(Symbol.Info)
            infoMenuItem.KeyDisplayString = "Ctrl+I"
            infoMenuItem.Enabled = p.SourceFile.NotNullOrEmptyS

            Menu.Add("Advanced Info...", AddressOf Editor.ShowAdvancedInfo, p.SourceFile.NotNullOrEmptyS).SetImage(Symbol.Lightbulb)

            Dim joinMenuItem = Menu.Add("Join Filters", AddressOf Editor.JoinFilters, "Joins all filters into one filter.")
            joinMenuItem.Enabled = DirectCast(Parent, FlowLayoutPanel).Controls.Count > 1
            joinMenuItem.ShortcutKeyDisplayString = "Ctrl+J   "

            Dim profilesMenuItem = Menu.Add("Profiles...", AddressOf g.MainForm.ShowFilterProfilesDialog, "Dialog to edit profiles.")
            profilesMenuItem.KeyDisplayString = "Ctrl+P"
            profilesMenuItem.SetImage(Symbol.FavoriteStar)

            Dim macrosMenuItem = Menu.Add("Macros...", AddressOf MacrosForm.ShowDialogForm, "Dialog to choose macros.")
            macrosMenuItem.KeyDisplayString = "Ctrl+M"
            macrosMenuItem.SetImage(Symbol.CalculatorPercentage)

            Menu.Add("-")

            Dim moveUpMenuItem = Menu.Add("Move Up", AddressOf MoveUp)
            moveUpMenuItem.KeyDisplayString = "Ctrl+Up"
            moveUpMenuItem.SetImage(Symbol.Up)

            Dim moveDownMenuItem = Menu.Add("Move Down", AddressOf MoveDown)
            moveDownMenuItem.KeyDisplayString = "Ctrl+Down"
            moveDownMenuItem.SetImage(Symbol.Down)

            Menu.Add("-")

            Dim cutAction = Sub()
                                Clipboard.SetText(rtbScript.SelectedText)
                                rtbScript.SelectedText = ""
                            End Sub

            Dim copyAction = Sub() Clipboard.SetText(rtbScript.SelectedText)

            Dim pasteAction = Sub()
                                  rtbScript.SelectedText = Clipboard.GetText
                                  rtbScript.ScrollToCaret()
                              End Sub

            Dim isTSel As Boolean = rtbScript.SelectionLength > 0
            Dim cutMenuItem = Menu.Add("Cut", cutAction, isTSel AndAlso Not rtbScript.ReadOnly)
            cutMenuItem.SetImage(Symbol.Cut)
            cutMenuItem.KeyDisplayString = "Ctrl+X"

            Dim copyMenuItem = Menu.Add("Copy", copyAction, isTSel)
            copyMenuItem.SetImage(Symbol.Copy)
            copyMenuItem.KeyDisplayString = "Ctrl+C"

            Dim pasteMenuItem = Menu.Add("Paste", pasteAction, Clipboard.GetText.NotNullOrEmptyS AndAlso Not rtbScript.ReadOnly)
            pasteMenuItem.SetImage(Symbol.Paste)
            pasteMenuItem.KeyDisplayString = "Ctrl+V"

            Menu.Add("-")
            Dim helpMenuItem = Menu.Add("Help")
            helpMenuItem.SetImage(Symbol.Help)
            Dim helpTempMenuItem = Menu.Add("Help | temp")

            Dim helpAction = Sub()
                                 For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)()
                                     If Not pluginPack.AvsFilterNames Is Nothing Then
                                         For Each avsFilterName In pluginPack.AvsFilterNames
                                             If rtbScript.Text.Contains(avsFilterName) Then
                                                 Menu.Add("Help | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                             End If
                                         Next
                                     End If
                                 Next

                                 If p.Script.Engine = ScriptEngine.AviSynth Then
                                     Dim installDir = Registry.LocalMachine.GetString("SOFTWARE\AviSynth", Nothing)
                                     Dim helpText = rtbScript.Text.Left("(")

                                     If helpText.EndsWith("Resize", StringComparison.Ordinal) Then helpText = "Resize"
                                     If helpText.StartsWith("ConvertTo", StringComparison.Ordinal) Then helpText = "Convert"

                                     Dim filterPath = installDir + "\Docs\English\corefilters\" + helpText + ".htm"

                                     If File.Exists(filterPath) Then
                                         Menu.Add("Help | " + helpText, Sub() g.ShellExecute(filterPath), filterPath)
                                     End If

                                     Dim helpIndex = installDir + "\Docs\English\overview.htm"

                                     If File.Exists(helpIndex) Then
                                         Menu.Add("Help | AviSynth local", Sub() g.ShellExecute(helpIndex), helpIndex)
                                     End If

                                     Menu.Add("Help | AviSynth Help", Sub() g.ShellExecute("http://avisynth.nl"), "http://avisynth.nl")
                                     Menu.Add("Help | -")

                                     For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)

                                         If Not pluginPack.AvsFilterNames Is Nothing Then
                                             Menu.Add("Help | " + pluginPack.Name.Substring(0, 1).ToUpperInvariant + " | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                             Application.DoEvents()
                                         End If
                                     Next
                                 Else
                                     Menu.Add("Help | VapourSynth Help", Sub() Package.VapourSynth.ShowHelp(), Package.VapourSynth.HelpFileOrURL)
                                     Menu.Add("Help | -")

                                     For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)
                                         If Not pluginPack.VSFilterNames Is Nothing Then
                                             Menu.Add("Help | " + pluginPack.Name.Substring(0, 1).ToUpperInvariant + " | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                             Application.DoEvents()
                                         End If
                                     Next
                                 End If
                             End Sub

            AddHandler helpMenuItem.DropDownOpened, Sub()
                                                        If helpMenuItem.DropDownItems.Count > 1 Then
                                                            Exit Sub
                                                        End If

                                                        helpTempMenuItem.Visible = False
                                                        helpAction()
                                                    End Sub
            Menu.ResumeLayout(True)
            Menu.Show(rtbScript, e.Location)
        End Sub

        Sub MoveUp()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim index = flow.Controls.IndexOf(Me)
            index -= 1

            If index < 0 Then
                index = 0
            End If

            flow.Controls.SetChildIndex(Me, index)
        End Sub

        Sub MoveDown()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim index = flow.Controls.IndexOf(Me)
            index += 1

            If index >= flow.Controls.Count - 1 Then
                index = flow.Controls.Count - 1
            End If

            flow.Controls.SetChildIndex(Me, index)
        End Sub

        Sub CodePreview()
            Dim script As New VideoScript
            script.Engine = Editor.Engine
            script.Filters = Editor.GetFilters()
            g.CodePreview(script.GetFullScript)
        End Sub

        Sub RemoveClick()
            If MsgQuestion("Remove?") = DialogResult.OK Then
                Dim flow = DirectCast(Parent, FlowLayoutPanel)

                If flow.Controls.Count > 1 Then
                    flow.Controls.Remove(Me)
                    Dispose()
                    Editor.ActiveTable = Nothing
                End If
            End If
        End Sub

        Sub FilterClick(filter As VideoFilter)
            Using td As New TaskDialog(Of String)
                td.MainInstruction = "Choose action"
                td.AddCommand("Replace selection", "Replace")
                td.AddCommand("Insert at selection", "Insert")
                td.AddCommand("Add to end", "Add")

                Select Case td.Show
                    Case "Replace"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        cbActive.Checked = filter.Active
                        cbActive.Text = filter.Category

                        If Not tup.Value.Equals(filter.Script) AndAlso tup.Caption.NotNullOrEmptyS Then
                            If filter.Script.StartsWith("$", StringComparison.Ordinal) Then
                                tbName.Text = tup.Caption
                            Else
                                tbName.Text = filter.Name.Replace("...", "") + " " + tup.Caption
                            End If
                        Else
                            tbName.Text = filter.Name
                        End If

                        rtbScript.Text = tup.Value.TrimEnd + BR
                        rtbScript.SelectionStart = rtbScript.Text.Length
                        Application.DoEvents()
                        Menu.Items.ClearAndDisplose
                    Case "Insert"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        If Not tup.Value.Equals(filter.Script) AndAlso tup.Caption.NotNullOrEmptyS Then
                            If filter.Script.StartsWith("$", StringComparison.Ordinal) Then
                                filter.Path = tup.Caption
                            Else
                                filter.Path = filter.Path.Replace("...", "") + " " + tup.Caption
                            End If
                        End If

                        filter.Script = tup.Value
                        Dim flow = DirectCast(Parent, FlowLayoutPanel)
                        Dim index = flow.Controls.IndexOf(Me)
                        Dim filterTable = CodeEditor.CreateFilterTable(filter)
                        flow.SuspendLayout()
                        flow.Controls.Add(filterTable)
                        flow.Controls.SetChildIndex(filterTable, index)
                        flow.ResumeLayout()
                        filterTable.rtbScript.SelectionStart = filterTable.rtbScript.Text.Length
                        filterTable.rtbScript.Focus()
                        Application.DoEvents()
                        Menu.Items.ClearAndDisplose
                    Case "Add"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        If Not tup.Value.Equals(filter.Script) AndAlso tup.Caption.NotNullOrEmptyS Then
                            If filter.Script.StartsWith("$", StringComparison.Ordinal) Then
                                filter.Path = tup.Caption
                            Else
                                filter.Path = filter.Path.Replace("...", "") + " " + tup.Caption
                            End If
                        End If

                        filter.Script = tup.Value
                        Dim flow = DirectCast(Parent, FlowLayoutPanel)
                        Dim filterTable = CodeEditor.CreateFilterTable(filter)
                        flow.Controls.Add(filterTable)
                        filterTable.rtbScript.SelectionStart = filterTable.rtbScript.Text.Length
                        filterTable.rtbScript.Focus()
                        Application.DoEvents()
                        Menu.Items.ClearAndDisplose
                End Select
            End Using
        End Sub

        Sub ReplaceClick(filter As VideoFilter)
            Dim tup = Macro.ExpandGUI(filter.Script)

            If tup.Cancel Then
                Exit Sub
            End If

            cbActive.Checked = filter.Active
            cbActive.Text = filter.Category

            If Not tup.Value.Equals(filter.Script) AndAlso tup.Caption.NotNullOrEmptyS Then
                If filter.Script.StartsWith("$", StringComparison.Ordinal) Then
                    tbName.Text = tup.Caption
                Else
                    tbName.Text = filter.Name.Replace("...", "") + " " + tup.Caption
                End If
            Else
                tbName.Text = filter.Name
            End If

            rtbScript.Text = tup.Value.TrimEnd + BR
            rtbScript.SelectionStart = rtbScript.Text.Length
            Application.DoEvents()
            Menu.Items.ClearAndDisplose
        End Sub
    End Class
End Class
