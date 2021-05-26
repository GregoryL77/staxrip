
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports JM.LinqFaster
Imports Microsoft.Win32

Imports StaxRip.UI

Public Class CodeEditor
    Public RTBTextSizeL As New Dictionary(Of Integer, Size)(7)
    Public RTBLayReq As Integer
    Public FilterTabSLock As New Object
    Private ScrollON As Boolean

    Property ActiveTable As FilterTable
    Property Engine As ScriptEngine

    Sub New(doc As VideoScript)
        sssw.Restart()
        InitializeComponent()
        sssw.Stop()
        ttt2 &= sssw.ElapsedTicks / SWFreq & "msInit| "
        sssw.Restart()

        Me.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.MainFlowLayoutPanel.SuspendLayout()

        Engine = doc.Engine
        'MainFlowLayoutPanel.MaximumSize = New Size(CInt(ScreenResolutionPrim.Width * 0.95), CInt(ScreenResolutionPrim.Height * 0.85))
        MaximumSize = New Size(CInt(ScreenResolutionPrim.Width * 0.99), CInt(ScreenResolutionPrim.Height * 0.96))

        Using rtbScrFont As Font = New Font("Consolas", 10 * s.UIScaleFactor)
            Dim RTBFontHeight = rtbScrFont.Height
            Dim maxTextWidth As Integer
            Dim filters As List(Of VideoFilter) = doc.Filters
            Dim ttsA(filters.Count - 1) As Size
            For f = 0 To filters.Count - 1
                Dim fText = If(filters(f).Script.NullOrEmptyS, "", filters(f).Script + BR)
                ttsA(f) = TextRenderer.MeasureText(fText, rtbScrFont, New Size(100000, 100000))
                Dim tw As Integer = ttsA(f).Width
                If tw > CInt(ScreenResolutionPrim.Width * 0.85) Then tw = CInt(ScreenResolutionPrim.Width * 0.85)
                If tw > maxTextWidth Then maxTextWidth = tw  '= filterTables.MaxF(Function(i) i.TrimmedTextSize.Width)
            Next f

            For f = 0 To filters.Count - 1
                ttsA(f).Width = maxTextWidth + RTBFontHeight '- 1
                Dim th As Integer = ttsA(f).Height
                If th > RTBFontHeight * 15 Then th = RTBFontHeight * 15
                ttsA(f).Height = th + If(th < RTBFontHeight * 15 * 1.05, CInt(RTBFontHeight * 1.1), 0) '- 1
                'totalHeigth += filterTables(f).Height
            Next f

            For i = 0 To filters.Count - 1
                MainFlowLayoutPanel.Controls.Add(New FilterTable(filters(i), ttsA(i)))
            Next i
        End Using

        Dim ps As Size = MainFlowLayoutPanel.PreferredSize
        MainFlowLayoutPanel.Size = ps
        tlpMain.Size = New Size(ps.Width, ps.Height + 25)
        Me.Size = New Size(ps.Width + 19, ps.Height + 67) '42

        '  AutoSize = True
        ' MainFlowLayoutPanelLayout(Nothing, Nothing)
        ' tlpMain.AutoSize = True
        ' tlpMain.AutoSizeMode = AutoSizeMode.GrowAndShrink
        '  MainFlowLayoutPanel.AutoSize = True
        '  MainFlowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink


        sssw.Stop()
        ttt2 &= sssw.ElapsedTicks / SWFreq & "msNew| "
        sssw.Restart()
    End Sub

    Public sssw As New Stopwatch 'debug
    Public ttt2 As String

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)



        sssw.Stop()
        ttt2 &= sssw.ElapsedTicks / SWFreq & "msLoad| "
        sssw.Restart()
    End Sub
    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)

        tlpMain.ResumeLayout()
        MainFlowLayoutPanel.ResumeLayout()
        ResumeLayout()

        AddHandler MainFlowLayoutPanel.Layout, AddressOf MainFlowLayoutPanelLayout
        Refresh()
        sssw.Stop()
        Text = ttt2 & sssw.ElapsedTicks / SWFreq & "msShow"
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        If MainFlowLayoutPanel.Height + 67 > CInt(ScreenResolutionPrim.Height * 0.95) Then
            If Not ScrollON Then
                ScrollON = True
                MainFlowLayoutPanel.Padding = New Padding(16, 2, 1, 2) 'New Padding(9, 2, 9, 2)
                MainFlowLayoutPanel.AutoScroll = True
                Console.Beep(6700, 30) 'Debug
            End If
        Else
            If ScrollON Then
                ScrollON = False
                Me.MainFlowLayoutPanel.Padding = New System.Windows.Forms.Padding(6, 2, 1, 2)
                MainFlowLayoutPanel.AutoScroll = False
                Console.Beep(600, 30) 'Debug
            End If
        End If
        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs) 'Debug
        Me.Text = "Code Editor"
        'ActionMenuItem.LayoutSuspendList = Nothing
        MyBase.OnFormClosing(args)
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        Dim form As New HelpForm()
        hevent.Handled = True
        form.Doc.WriteStart(Text)
        form.Doc.WriteTable("Macros", Macro.GetTips(False, True, False))
        form.Show()
        MyBase.OnHelpRequested(hevent)
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

    Shared Function CreateFilterTable(filter As VideoFilter) As FilterTable
        Return New FilterTable(filter)
    End Function

    Function GetFilters() As List(Of VideoFilter)
        Dim ret As New List(Of VideoFilter)

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
        Dim firstTable = DirectCast(MainFlowLayoutPanel.Controls(0), FilterTable)
        firstTable.tbName.Text = "merged"
        firstTable.rtbScript.Text = MainFlowLayoutPanel.Controls.OfType(Of FilterTable).ToArray.SelectF(Function(arg) If(arg.cbActive.Checked, arg.rtbScript.Text.Trim, "#" + arg.rtbScript.Text.Trim.FixBreak.Replace(BR, "# " + BR))).Join(BR) + BR2 + BR2
        MainFlowLayoutPanel.SuspendLayout()
        For x = MainFlowLayoutPanel.Controls.Count - 1 To 1 Step -1
            Dim del = MainFlowLayoutPanel.Controls.Item(x)
            MainFlowLayoutPanel.Controls.RemoveAt(x)
            del.Dispose()
        Next
        ActiveTable = Nothing
        MainFlowLayoutPanel.ResumeLayout()
    End Sub

    Sub MainFlowLayoutPanelLayout(sender As Object, e As LayoutEventArgs)
        Dim filterTables = MainFlowLayoutPanel.Controls.OfType(Of FilterTable).ToArray
        If filterTables.Length <= 0 Then 'Any
            Exit Sub
        End If

        'Console.Beep(If(RTBLayReq = 1, 6000, 300), 90) 'debug 
        Dim maxTextWidth As Integer
        'Dim totalHeigth As Integer
        Dim sizeRTB As Size

        If RTBLayReq > 0 AndAlso RTBTextSizeL.Count >= filterTables.Length Then ' e is NULL!!!!
            Dim rtsAr = RTBTextSizeL.Values.ToArray
            maxTextWidth = rtsAr.MaxF(Function(s) s.Width)
            For f = 0 To filterTables.Length - 1
                sizeRTB.Width = maxTextWidth + filterTables(f).RTBFontHeight
                sizeRTB.Height = rtsAr(f).Height + If(rtsAr(f).Height < filterTables(f).MaxRTBTextHeight * 1.05, CInt(filterTables(f).RTBFontHeight * 1.1), 0)
                filterTables(f).rtbScript.Size = sizeRTB
                filterTables(f).rtbScript.Invalidate()
                'totalHeigth += filterTables(f).Height
            Next f
            'Console.Beep(1000, 90)
        Else
            Dim ttsA(filterTables.Length - 1) As Size
            For f = 0 To filterTables.Length - 1
                ttsA(f) = filterTables(f).TrimmedTextSize
                If ttsA(f).Width > maxTextWidth Then maxTextWidth = ttsA(f).Width  '= filterTables.MaxF(Function(i) i.TrimmedTextSize.Width)
            Next f

            For f = 0 To filterTables.Length - 1
                sizeRTB.Width = maxTextWidth + filterTables(f).RTBFontHeight
                sizeRTB.Height = ttsA(f).Height + If(ttsA(f).Height < filterTables(f).MaxRTBTextHeight * 1.05, CInt(filterTables(f).RTBFontHeight * 1.1), 0)
                filterTables(f).rtbScript.Size = sizeRTB
                filterTables(f).rtbScript.Invalidate()
                'totalHeigth += filterTables(f).Height
            Next f
        End If

        ''MainFlowLayoutPanel.VerticalScroll.Visible = totalHeigth > CInt(ScreenResolutionPrim.Height * 0.8)
        'MainFlowLayoutPanel.AutoScroll = totalHeigth + 50 > CInt(ScreenResolutionPrim.Height * 0.85)  ' ttotH+50 ...  

    End Sub

    Public Class FilterTable
        Inherits TableLayoutPanel

        Private RTBEventSem As Boolean
        ReadOnly Property RTBFontHeight As Integer ' = 16
        Property tbName As New TextEdit
        Property rtbScript As RichTextBoxEx
        Property cbActive As New CheckBox
        Property Menu As New ContextMenuStripEx
        Property LastTextSize As Size
        Property Editor As CodeEditor
        ReadOnly Property MaxRTBTextWidth As Integer
            Get
                Return CInt(ScreenResolutionPrim.Width * 0.85)   'RTBFontHeight * 108 '108-96 fh=16 'Or: Screen.PrimaryScreen.Bounds
            End Get
        End Property
        ReadOnly Property MaxRTBTextHeight As Integer
            Get
                Return RTBFontHeight * 15
            End Get
        End Property

        ReadOnly Property TrimmedTextSize As Size
            Get
                Dim ret = TextRenderer.MeasureText(rtbScript.Text, rtbScript.Font, New Size(100000, 100000))

                If ret.Width > MaxRTBTextWidth Then ret.Width = MaxRTBTextWidth
                If ret.Height > MaxRTBTextHeight Then ret.Height = MaxRTBTextHeight

                Return ret
            End Get
        End Property

        Sub New(filter As VideoFilter, Optional rtbSize As Size = Nothing)
            Me.SuspendLayout()
            'cbActive.AutoSize = True
            cbActive.Anchor = AnchorStyles.Left Or AnchorStyles.Right
            cbActive.Margin = New Padding(0)
            cbActive.Checked = filter.Active
            cbActive.Text = filter.Category
            cbActive.Size = cbActive.PreferredSize 'Test!!!

            tbName.Dock = DockStyle.Top
            tbName.Margin = New Padding(0, 0, 0, 0)
            Dim fh As Integer = 13 'Font.Height 'Opt.13(MSSansSer8) Disabled OnLayout in FilterTable Class 'was 16(Sagoe,Consolas10) in RTBfilterTableConsolas class
            tbName.Size = New Size(fh * 8, CInt(fh * 1.2)) '7 , '1.2 or +3 or +7
            tbName.Text = filter.Name

            rtbScript = New RichTextBoxEx(False)
            Dim rtbScrFont As Font = New Font("Consolas", 10 * s.UIScaleFactor)
            With rtbScript
                .EnableAutoDragDrop = True
                .Dock = DockStyle.Fill
                .WordWrap = False
                .ScrollBars = RichTextBoxScrollBars.None
                .AcceptsTab = True
                .Margin = New Padding(0)
                .Font = rtbScrFont
                .Text = If(filter.Script.NullOrEmptyS, "", filter.Script + BR)
                .Size = If(rtbSize = Size.Empty, .PreferredSize, rtbSize)  'Test!!!
            End With
            RTBFontHeight = rtbScrFont.Height
            SetColor()

            RTBEventSem = False
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
                                                                        Dim maxTW As Integer
                                                                        Dim tmts As Size
                                                                        Dim mts As Size
                                                                        If pcfa.Length <> Editor.RTBTextSizeL.Count Then Editor.RTBTextSizeL.Clear()
                                                                        Dim rtbTxtA As String()
                                                                        Dim mt As MethodInvoker = Sub() rtbTxtA = pcfa.SelectF(Function(ft) ft.rtbScript.Text)
                                                                        If Editor.IsHandleCreated Then Editor.Invoke(mt) Else Return

                                                                        For i = 0 To pcfa.Length - 1
                                                                            Dim itr = i
                                                                            mts = TextRenderer.MeasureText(rtbTxtA(itr), rtbScrFont, New Size(100000, 100000))
                                                                            If mts.Width > maxTW Then maxTW = mts.Width
                                                                            If mts.Width > MaxRTBTextWidth Then
                                                                                mts.Width = MaxRTBTextWidth
                                                                                maxTW = MaxRTBTextWidth
                                                                            End If
                                                                            If mts.Height > MaxRTBTextHeight Then mts.Height = MaxRTBTextHeight
                                                                            Editor.RTBTextSizeL(itr) = mts
                                                                            If pcfa(itr) Is Me Then tmts = mts
                                                                            'If mts.Width > MaxRTBTextWidth OrElse mts.Height > MaxRTBTextHeight Then Exit For 'Needs Tests???
                                                                        Next i
                                                                        RTBEventSem = False

                                                                        sss.Stop()

                                                                        ttt &= CStr(sss.ElapsedTicks / SWFreq) & "ms|Measur:" & mts.ToString & " Curr:" & tmts.ToString
                                                                        ' Editor.BeginInvoke(Sub() Editor.Text = ttt)

                                                                        If tmts.Width > maxTW OrElse (tmts.Width = maxTW AndAlso tmts.Width <> LastTextSize.Width) OrElse LastTextSize.Height <> tmts.Height AndAlso tmts.Height > RTBFontHeight Then
                                                                            'Task.Run(Sub() Console.Beep(3300, 30))
                                                                            Editor.RTBLayReq = 1
                                                                            LastTextSize = tmts
                                                                            Editor.BeginInvoke(Sub()
                                                                                                   sss.Restart()
                                                                                                   Parent.PerformLayout()
                                                                                                   Editor.RTBLayReq = -1
                                                                                                   sss.Stop()
                                                                                                   Editor.Text = ttt & CStr(sss.ElapsedTicks / SWFreq) & "msFinLay"
                                                                                               End Sub)
                                                                        End If
                                                                    End SyncLock

                                                                End Sub)
                                            Dim lTc = layT.ContinueWith(Sub()
                                                                            If layT.Exception IsNot Nothing Then
                                                                                RTBEventSem = False
                                                                                Editor.RTBTextSizeL.Clear()
                                                                                Console.Beep(5500, 200)
                                                                            End If
                                                                        End Sub) 'debug
                                            '  End If
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

            Dim t As New TableLayoutPanel
            With t
                .SuspendLayout()
                '.AutoSize = True
                .Dock = DockStyle.Fill
                .Margin = New Padding(0)
                .ColumnCount = 1
                .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
                .RowCount = 2
                .RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
                .RowStyles.Add(New RowStyle(SizeType.Absolute, fh * 1.2F))
                .Controls.Add(cbActive, 0, 0)
                .Controls.Add(tbName, 0, 1)
                .Size = .PreferredSize 'Test!!!

            End With

            ColumnCount = 2
            ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, fh * 8))
            ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
            RowCount = 1
            RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
            Controls.Add(t, 0, 0)
            Controls.Add(rtbScript, 1, 0)
            Margin = New Padding(0)
            AutoSize = True
            'Size = New Size(950, 50)
            'Size = PreferredSize 'Test!!!

            t.ResumeLayout()
            Me.ResumeLayout()
        End Sub
        'Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
        '    Dim fh As Integer = FontHeight 'RTBFontHeight
        ''    tbName.Height = CInt(fh * 1.2) 'Or*1.25 or +3 or +7  '  tbName.Width = fh * 8 '7
        ' tbName.Size = New Size(fh * 8, CInt(fh * 1.2))
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
