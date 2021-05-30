
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports JM.LinqFaster
Imports Microsoft.Win32

Imports StaxRip.UI

Public Class CodeEditor
    Private ScrollON As Boolean
    Public RTBTextSizeA As Size()
    'Public RTBLayReq As Integer 'Is This Needed ?? try remove it, Seems Redundant
    Public FilterTabSLock As New Object
    Public RTBFontHeight As Integer ' = 16
    Property ActiveTable As FilterTable
    Property Engine As ScriptEngine

    Public sssw As New Stopwatch 'debug !!!!!!!
    Public ttt2 As String

    ReadOnly Property MaxRTBTextWidth As Integer
        Get
            Return CInt(ScreenResolutionPrim.Width * 0.85)   'RTBFontHeight * 108 '108-96 fh=16 'Or: Screen.PrimaryScreen.Bounds
        End Get
    End Property
    ReadOnly Property MaxRTBTextHeight As Integer
        Get
            Return RTBFontHeight * 15 'fh=16
        End Get
    End Property

    Sub New(doc As VideoScript)
        sssw.Restart()
        InitializeComponent()
        sssw.Stop()
        ttt2 &= sssw.ElapsedTicks / SWFreq & "msInit| "
        sssw.Restart()

        Me.SuspendLayout()
        Me.MainFlowLayoutPanel.SuspendLayout()

        Engine = doc.Engine
        MainFlowLayoutPanel.MaximumSize = New Size((ScreenResolutionPrim.Width - 128), (ScreenResolutionPrim.Height - 64 - 68))
        MaximumSize = New Size((ScreenResolutionPrim.Width * 1), (ScreenResolutionPrim.Height - 64))

        Dim rtbScrFont As Font = New Font("Consolas", 10.0F * s.UIScaleFactor)
        RTBFontHeight = rtbScrFont.Height
        Dim maxTW As Integer
        Dim filters As List(Of VideoFilter) = doc.Filters
        Dim rtsA(filters.Count - 1) As Size
        ReDim RTBTextSizeA(rtsA.Length - 1)
        For f = 0 To rtsA.Length - 1
            rtsA(f) = TextRenderer.MeasureText(If(filters(f).Script.NullOrEmptyS, "", filters(f).Script & BR), rtbScrFont, New Size(100000, 100000))
            If rtsA(f).Width > MaxRTBTextWidth Then rtsA(f).Width = MaxRTBTextWidth
            If rtsA(f).Width > maxTW Then maxTW = rtsA(f).Width
            If rtsA(f).Height > MaxRTBTextHeight Then rtsA(f).Height = MaxRTBTextHeight
            RTBTextSizeA(f) = rtsA(f)
        Next f

        For f = 0 To rtsA.Length - 1
            rtsA(f).Width = maxTW + RTBFontHeight - 1 'WorkAround For no RichTextBoxEX content onShow
            rtsA(f).Height = rtsA(f).Height + If(rtsA(f).Height < MaxRTBTextHeight * 1.05, CInt(RTBFontHeight * 1.1), 0) '- 1
            Dim ft As FilterTable = New FilterTable(filters(f), Me, rtbScrFont, rtsA(f))
            MainFlowLayoutPanel.Controls.Add(ft)
        Next f

        'RTBLayReq = 1
        Dim ps As Size = MainFlowLayoutPanel.PreferredSize
        Me.Size = New Size(ps.Width + 16, ps.Height + 66)

        sssw.Stop()
        ttt2 &= sssw.ElapsedTicks / SWFreq & "msNew| "
        sssw.Restart()

    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)
        MainFlowLayoutPanel.ResumeLayout(False)
        ResumeLayout()
        MainFlowLayoutPanelLayout(Nothing, Nothing)

        sssw.Stop()
        ttt2 &= sssw.ElapsedTicks / SWFreq & "msLoad| "
        sssw.Restart()
    End Sub
    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        AddHandler MainFlowLayoutPanel.Layout, AddressOf MainFlowLayoutPanelLayout
        AddHandler MainFlowLayoutPanel.SizeChanged, AddressOf MainFlowPanel_SizeChanged

        sssw.Stop()
        Text = ttt2 & sssw.ElapsedTicks / SWFreq & "msShow"
        'RTBLayReq = -1
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs) 'Debug
        Me.Text = "Code Editor"
        'ActionMenuItem.LayoutSuspendList = Nothing
        MyBase.OnFormClosing(args)
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
                If ActiveTable IsNot Nothing Then
                    ActiveTable.RemoveClick()
                End If
            Case Keys.Control Or Keys.Up
                If ActiveTable IsNot Nothing Then
                    ActiveTable.MoveUp()
                End If
            Case Keys.Control Or Keys.Down
                If ActiveTable IsNot Nothing Then
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
    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        Dim form As New HelpForm()
        hevent.Handled = True
        form.Doc.WriteStart(Text)
        form.Doc.WriteTable("Macros", Macro.GetTips(False, True, False))
        form.Show()
        MyBase.OnHelpRequested(hevent)
    End Sub

    Sub MainFlowPanel_SizeChanged(sender As Object, e As EventArgs)
        If MainFlowLayoutPanel.Height > (ScreenResolutionPrim.Height - 64 - 68 - RTBFontHeight) Then
            If Not ScrollON Then
                ScrollON = True
                MainFlowLayoutPanel.Padding = New Padding(6, 2, 14, 18)
                MainFlowLayoutPanel.AutoScroll = True
                Console.Beep(6700, 50) 'Debug
            End If
        Else
            If ScrollON Then
                ScrollON = False
                Me.MainFlowLayoutPanel.Padding = New System.Windows.Forms.Padding(6, 2, 3, 2)
                MainFlowLayoutPanel.AutoScroll = False
                Console.Beep(600, 50) 'Debug
            End If
        End If
        MyBase.OnSizeChanged(e)
    End Sub

    Sub MainFlowLayoutPanelLayout(sender As Object, e As LayoutEventArgs)
        Dim filterTables = MainFlowLayoutPanel.Controls.OfType(Of FilterTable).ToArray
        If filterTables.Length <= 0 Then Exit Sub
        Dim maxTextWidth As Integer
        Dim rtsA() As Size

        If RTBTextSizeA.Length = filterTables.Length Then 'RTBLayReq > 0 AndAlso 
            'RTBTextSizeD.Values.CopyTo(rtsA, 0) 'Dictionary version
            rtsA = RTBTextSizeA
            maxTextWidth = rtsA.MaxF(Function(sz) sz.Width)
        Else
            ReDim rtsA(filterTables.Length - 1)
            Dim rtb As RichTextBoxEx
            For f = 0 To filterTables.Length - 1
                rtb = filterTables(f).rtbScript
                rtsA(f) = TextRenderer.MeasureText(rtb.Text, rtb.Font, New Size(100000, 100000))
                If rtsA(f).Width > MaxRTBTextWidth Then rtsA(f).Width = MaxRTBTextWidth
                If rtsA(f).Height > MaxRTBTextHeight Then rtsA(f).Height = MaxRTBTextHeight
                If rtsA(f).Width > maxTextWidth Then maxTextWidth = rtsA(f).Width
            Next f

            Task.Run(Sub() Console.Beep(350, 200)) 'debug
        End If

        Dim fTb As FilterTable
        MainFlowLayoutPanel.SuspendLayout()
        SuspendLayout()
        For f = 0 To filterTables.Length - 1
            fTb = filterTables(f)
            Dim rtH As Integer = rtsA(f).Height
            fTb.SuspendLayout() 'Faster 2x !
            fTb.rtbScript.Size = New Size(maxTextWidth + RTBFontHeight,
                                          rtH + If(rtH < MaxRTBTextHeight * 1.05, CInt(RTBFontHeight * 1.1), 0))
            'fTb.rtbScript.Refresh()
        Next f

        For f = 0 To filterTables.Length - 1
            filterTables(f).ResumeLayout(False)
        Next f
        MainFlowLayoutPanel.ResumeLayout(False)
        ResumeLayout()

        'MainFlowLayoutPanel.Size = MainFlowLayoutPanel.PreferredSize
        'Me.Size = PreferredSize
    End Sub

    Shared Function CreateFilterTable(filter As VideoFilter, editorForm As CodeEditor) As FilterTable
        Return New FilterTable(filter, editorForm, New Font("Consolas", 10.0F * s.UIScaleFactor))
    End Function

    Function GetFilters() As List(Of VideoFilter)
        Dim ret As New List(Of VideoFilter)(8)

        For Each table As FilterTable In MainFlowLayoutPanel.Controls
            Dim filter As New VideoFilter With {
                .Active = table.cbActive.Checked,
                .Category = table.cbActive.Text,
                .Path = table.tbName.Text,
                .Script = table.rtbScript.Text.FixBreak.Trim
            }
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

        Dim form As New PreviewForm(script) With {.Owner = g.MainForm}
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
        SuspendLayout()
        MainFlowLayoutPanel.SuspendLayout()
        Dim firstTable = DirectCast(MainFlowLayoutPanel.Controls(0), FilterTable)
        firstTable.tbName.Text = "merged"
        firstTable.rtbScript.Text = MainFlowLayoutPanel.Controls.OfType(Of FilterTable).ToArray.SelectF(Function(arg) If(arg.cbActive.Checked, arg.rtbScript.Text.Trim, "#" + arg.rtbScript.Text.Trim.FixBreak.Replace(BR, "# " + BR))).Join(BR) + BR2 + BR2
        For x = MainFlowLayoutPanel.Controls.Count - 1 To 1 Step -1
            Dim del = MainFlowLayoutPanel.Controls.Item(x)
            MainFlowLayoutPanel.Controls.RemoveAt(x)
            del.Dispose()
        Next
        ActiveTable = Nothing
        MainFlowLayoutPanel.ResumeLayout()
        ResumeLayout()
    End Sub

    Public Class FilterTable
        Inherits TableLayoutPanel

        Private RTBEventSem As Boolean
        Private LastTextSize As Size
        Property tbName As TextEdit
        Property rtbScript As RichTextBoxEx
        Property cbActive As CheckBox
        Property Menu As ContextMenuStripEx
        Property Editor As CodeEditor

        Sub New(filter As VideoFilter, editorForm As CodeEditor, rtbScriptFont As Font, Optional rtbSize As Size = Nothing)
            Me.SuspendLayout()
            Editor = editorForm
            Me.Font = editorForm.Font
            Menu = New ContextMenuStripEx With {.Form = Editor}
            Dim fh As Integer = editorForm.RTBFontHeight '16 '=13(MSSansSer8.5), 16(Sagoe,Consolas9), 16("Consolas", 10.0F); 16-After Control.Add Inheritance
            Dim noPad As Padding = Padding.Empty

            cbActive = New CheckBox With {.Margin = noPad,
                                          .Checked = filter.Active,
                                          .Text = filter.Category,
                                          .Size = New Size(fh * 7, CInt(fh * 1.05))}

            tbName = New TextEdit With {.Margin = noPad,
                                        .Text = filter.Name,
                                        .Size = New Size(fh * 7, CInt(fh * 1.25))}  'w:6-8

            rtbScript = New RichTextBoxEx(False) With {.Margin = noPad,
                                                       .AcceptsTab = True,
                                                       .EnableAutoDragDrop = True,
                                                       .ScrollBars = RichTextBoxScrollBars.None,
                                                       .Font = rtbScriptFont,
                                                       .WordWrap = False,
                                                       .Text = If(filter.Script.NullOrEmptyS, "", filter.Script & BR),
                                                       .Size = If(rtbSize.IsEmpty, .PreferredSize, rtbSize)}
            SetColor()
            AutoSize = True
            ' AutoSizeMode = AutoSizeMode.GrowAndShrink 'needed ?
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize 'needed ?
            Margin = noPad
            ColumnCount = 2
            ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, fh * 7))
            ColumnStyles.Add(New ColumnStyle) '(SizeType.AutoSize, rtbSize.Width)) '(New ColumnStyle(SizeType.Percent, 100.0F)) 'Slowish - Auto is faster
            RowCount = 2 '1 
            RowStyles.Add(New RowStyle(SizeType.Absolute, CInt(fh * 1.05)))
            RowStyles.Add(New RowStyle) '(SizeType.AutoSize, CInt(Math.Max(fh * 1.25, rtbSize.Height - fh * 1.05))))
            Controls.Add(cbActive, 0, 0)
            Controls.Add(tbName, 0, 1)
            SetRowSpan(rtbScript, 2)
            Controls.Add(rtbScript, 1, 0)
            'Size = PreferredSize 'New Size(1000, 200)  'Test!!!

            Me.ResumeLayout(False)

            Dim caceh As EventHandler = Sub() SetColor()
            AddHandler cbActive.CheckedChanged, caceh
            AddHandler rtbScript.MouseUp, AddressOf HandleMouseUp
            Dim reeh As EventHandler = Sub() Editor.ActiveTable = Me
            AddHandler rtbScript.Enter, reeh
            Dim rtceh As EventHandler = Sub()
                                            If RTBEventSem Then Return
                                            Dim layT = Task.Run(Sub()
                                                                    SyncLock Editor.FilterTabSLock
                                                                        If RTBEventSem OrElse Me.IsDisposed Then Return
                                                                        RTBEventSem = True
                                                                        Thread.Sleep(180)
                                                                        If Me.IsDisposed OrElse Not Editor.IsHandleCreated Then Return

                                                                        Dim sss = Stopwatch.StartNew
                                                                        Dim ttt As String
                                                                        sss.Restart()

                                                                        Dim pcfa = Parent.Controls.OfType(Of FilterTable)().ToArray
                                                                        If pcfa.Length <> Editor.RTBTextSizeA.Length Then ReDim Editor.RTBTextSizeA(pcfa.Length - 1)
                                                                        Dim rtbTxtA(pcfa.Length - 1) As String
                                                                        'Dim gt As MethodInvoker = Sub() rtbTxtA = pcfa.SelectF(Function(ft) ft.rtbScript.Text) 'Thread Safe Call
                                                                        'Editor.Invoke(gt)

                                                                        For r = 0 To pcfa.Length - 1
                                                                            rtbTxtA(r) = pcfa(r).rtbScript.Text 'unsafe
                                                                        Next r

                                                                        Dim maxTW As Integer
                                                                        Dim tmts As Size
                                                                        Dim mts As Size
                                                                        For i = 0 To pcfa.Length - 1
                                                                            mts = TextRenderer.MeasureText(rtbTxtA(i), rtbScriptFont, New Size(100000, 100000))
                                                                            If mts.Width > Editor.MaxRTBTextWidth Then mts.Width = Editor.MaxRTBTextWidth
                                                                            If mts.Width > maxTW Then maxTW = mts.Width 'This makes NonSense???
                                                                            If mts.Height > Editor.MaxRTBTextHeight Then mts.Height = Editor.MaxRTBTextHeight
                                                                            Editor.RTBTextSizeA(i) = mts
                                                                            If pcfa(i) Is Me Then tmts = mts
                                                                        Next i
                                                                        RTBEventSem = False

                                                                        If tmts.Width > maxTW Then Console.Beep(6500, 30) 'Debug

                                                                        If tmts.Width > maxTW OrElse (tmts.Width = maxTW AndAlso tmts.Width <> LastTextSize.Width) OrElse LastTextSize.Height <> tmts.Height AndAlso tmts.Height > Editor.RTBFontHeight Then
                                                                            'Task.Run(Sub() Console.Beep(3300, 30))
                                                                            LastTextSize = tmts
                                                                            'Editor.RTBLayReq = 1
                                                                            Editor.BeginInvoke(Sub()
                                                                                                   sss.Restart()
                                                                                                   Parent.PerformLayout()
                                                                                                   'Editor.RTBLayReq = -1
                                                                                                   sss.Stop()
                                                                                                   Editor.Text = ttt & CStr(sss.ElapsedTicks / SWFreq) & "msLayM"
                                                                                               End Sub)
                                                                        End If

                                                                        sss.Stop()
                                                                        ttt &= Parent.Bounds.ToString & "FP" & CStr(sss.ElapsedTicks / SWFreq) & "ms|Measur:" & mts.ToString & " Curr:" & tmts.ToString
                                                                    End SyncLock

                                                                End Sub)
                                            Dim lTc = layT.ContinueWith(Sub()
                                                                            If layT.Exception IsNot Nothing Then
                                                                                RTBEventSem = False
                                                                                ReDim Editor.RTBTextSizeA(-1)
                                                                                Console.Beep(5500, 200)
                                                                            End If
                                                                        End Sub) 'debug
                                        End Sub

            AddHandler rtbScript.TextChanged, rtceh
            Dim deh As EventHandler = Sub()
                                          RTBEventSem = True
                                          RemoveHandler Me.Disposed, deh
                                          RemoveHandler cbActive.CheckedChanged, caceh
                                          RemoveHandler rtbScript.MouseUp, AddressOf HandleMouseUp
                                          RemoveHandler rtbScript.Enter, reeh
                                          RemoveHandler rtbScript.TextChanged, rtceh
                                          'Menu?.Items.ClearAndDisplose
                                          Menu?.Dispose()
                                      End Sub
            AddHandler Disposed, deh
        End Sub
        'Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
        '    Dim fh As Integer = FontHeight 'RTBFontHeight
        ' tbName.Size = New Size(fh * 7, CInt(fh * 1.25))
        '    MyBase.OnLayout(levent)
        'End Sub
        'Protected Overrides Sub OnHandleCreated(e As EventArgs)
        '    Menu.Form = FindForm()
        '    '   If Editor Is Nothing Then Editor = DirectCast(Menu.Form, CodeEditor)
        '    MyBase.OnHandleCreated(e)
        'End Sub

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


        Public SsWw As New Stopwatch 'Debug!!!
        Public TTTT As String 'Debug!!!

        Sub HandleMouseUp(sender As Object, e As MouseEventArgs)
            If e.Button <> MouseButtons.Right Then
                Exit Sub
            End If

            SsWw.Restart()
            Menu.SuspendLayout()
            ActionMenuItem.LayoutSuspendCreate(48)

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
            filterMenuItem.DropDown.SuspendLayout()
            filterMenuItem.SetImage(Symbol.Filter)

            ActionMenuItem.Add(filterMenuItem.DropDownItems, "Empty Filter", AddressOf FilterClick, New VideoFilter("Misc", "", ""), "Filter with empty values.")

            For Each filterCategory In filterProfiles
                For Each filter In filterCategory.Filters
                    Dim tip = filter.Script
                    ActionMenuItem.Add(filterMenuItem.DropDownItems, filterCategory.Name + " | " + filter.Path, AddressOf FilterClick, filter.GetCopy, tip)
                Next
            Next
            filterMenuItem.DropDown.ResumeLayout(False)

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

            Dim pasteMenuItem = Menu.Add("Paste", pasteAction, Clipboard.GetText IsNot "" AndAlso Not rtbScript.ReadOnly) ' Errors !! ToDO Check need
            pasteMenuItem.SetImage(Symbol.Paste)
            pasteMenuItem.KeyDisplayString = "Ctrl+V"

            Menu.Add("-")
            Dim helpMenuItem = Menu.Add("Help")
            helpMenuItem.SetImage(Symbol.Help)
            Dim helpTempMenuItem = Menu.Add("Help | temp")

            TTTT = ActionMenuItem.LayoutSuspendList?.Count & "LayLC| "
            ActionMenuItem.LayoutResume(False)
            Menu.ResumeLayout(False)

            Dim helpAction = Sub()
                                 SsWw.Restart()
                                 ActionMenuItem.LayoutSuspendCreate(32)
                                 Menu.SuspendLayout()

                                 For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)()
                                     If Not pluginPack.AvsFilterNames Is Nothing Then
                                         For Each avsFilterName In pluginPack.AvsFilterNames
                                             If rtbScript.Text.Contains(avsFilterName) Then
                                                 Menu.Add("Help | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                             End If
                                         Next
                                     End If
                                 Next

                                 Dim inc As Integer
                                 If p.Script.Engine = ScriptEngine.AviSynth Then
                                     'Dim InstallDir = Registry.LocalMachine.GetString("SOFTWARE\AviSynth", Nothing)
                                     Dim helpText = rtbScript.Text.Left("(")

                                     If helpText.EndsWith("Resize", StringComparison.Ordinal) Then helpText = "Resize"
                                     If helpText.StartsWith("ConvertTo", StringComparison.Ordinal) Then helpText = "Convert"

                                     Menu.Add("Help | AviSynth Help", Sub() g.ShellExecute("http://avisynth.nl"), "http://avisynth.nl")
                                     Menu.Add("Help | -")

                                     For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)

                                         If Not pluginPack.AvsFilterNames Is Nothing Then
                                             Menu.Add("Help | " + pluginPack.Name.Substring(0, 1).ToUpperInvariant + " | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                             inc += 1
                                             If inc Mod 8 = 0 Then Application.DoEvents()
                                         End If
                                     Next
                                 Else
                                     Menu.Add("Help | VapourSynth Help", Sub() Package.VapourSynth.ShowHelp(), Package.VapourSynth.HelpFileOrURL)
                                     Menu.Add("Help | -")
                                     For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)
                                         If Not pluginPack.VSFilterNames Is Nothing Then
                                             Menu.Add("Help | " + pluginPack.Name.Substring(0, 1).ToUpperInvariant + " | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                             inc += 1
                                             If inc Mod 8 = 0 Then Application.DoEvents()
                                         End If
                                     Next
                                 End If
                                 TTTT = ActionMenuItem.LayoutSuspendList?.Count & "LayLC| "
                                 ActionMenuItem.LayoutResume(False)
                                 Menu.ResumeLayout(False)
                                 SsWw.Stop()
                                 TTTT &= SsWw.ElapsedTicks / SWFreq & "msHelp"
                                 Editor.Text = TTTT
                             End Sub

            Dim hmoeh As EventHandler = Sub()
                                            RemoveHandler helpMenuItem.DropDownOpened, hmoeh
                                            helpTempMenuItem.Visible = False
                                            helpAction()
                                        End Sub
            AddHandler helpMenuItem.DropDownOpened, hmoeh

            SsWw.Stop()
            TTTT &= SsWw.ElapsedTicks / SWFreq & "msMain"
            Editor.Text = TTTT
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
            Dim flow = DirectCast(Parent, FlowLayoutPanel)

            If flow.Controls.Count > 1 Then
                If MsgQuestion("Remove?") = DialogResult.OK Then
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
                        Dim filterTable = CodeEditor.CreateFilterTable(filter, Editor)
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
                        Dim filterTable = CodeEditor.CreateFilterTable(filter, Editor)
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
