
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports JM.LinqFaster
Imports Microsoft.Win32

Imports StaxRip.UI

Public Class CodeEditor
    Public RTBTxtCHeightA As Integer()
    Public RTBTxtMaxCWidth As Integer
    Public ReadOnly FilterTabSLock As New Object
    Public RTBFontHeight As Integer ' = 16
    Property ActiveTable As FilterTable
    Property Engine As ScriptEngine

    Protected Overrides ReadOnly Property DefaultPadding As Padding
        Get
            Return New System.Windows.Forms.Padding(0, 0, 1, 3)
        End Get
    End Property
    Protected Overrides ReadOnly Property DefaultMinimumSize As Size
        Get
            Return New System.Drawing.Size(329, 127)
        End Get
    End Property

    Sub New(doc As VideoScript)
        InitializeComponent()
        Me.SuspendLayout()
        Me.MainFlowLayoutPanel.SuspendLayout()

        Engine = doc.Engine
        MaximumSize = New Size((ScreenResWAPrim.Width - 18), (ScreenResWAPrim.Height - 46))
        MainFlowLayoutPanel.MaximumSize = New Size((ScreenResWAPrim.Width - 38), (ScreenResWAPrim.Height - 48 - 68))

        Dim rtbScrFont As New Font("Consolas", 10 * s.UIScaleFactor)
        RTBFontHeight = rtbScrFont.Height

        Dim maxTW As Integer
        Dim s100k As New Size(100000, 100000)
        Dim filters As List(Of VideoFilter) = doc.Filters
        Dim rtHA(filters.Count - 1) As Integer
        For f = 0 To rtHA.Length - 1
            If filters(f).Script?.Length > 0 Then
                Dim mts As Size = TextRenderer.MeasureText(filters(f).Script & BR, rtbScrFont, s100k)
                rtHA(f) = mts.Height

                If mts.Width > maxTW Then maxTW = mts.Width
            End If
        Next f

        maxTW = GetRTBTextWidth(maxTW)
        RTBTxtMaxCWidth = maxTW
        Dim totH As Integer
        Dim rtbTxtHA(rtHA.Length - 1) As Integer
        For f = 0 To rtHA.Length - 1
            Dim th As Integer = GetRTBTextHeight(rtHA(f))
            rtbTxtHA(f) = th
            th += If(rtHA(f) < MaxRTBTextHeight, RTBFontHeight, 0)
            MainFlowLayoutPanel.Controls.Add(New FilterTable(filters(f), Me, rtbScrFont, New Size(maxTW, th)))

            totH += Math.Max(th, CInt(RTBFontHeight * 1.05))
        Next f
        RTBTxtCHeightA = rtbTxtHA

        Dim fPSz As New Size(RTBFontHeight * 7 + 6 + maxTW, totH + 4)
        MainFlowLayoutPanel.Size = fPSz
        Me.ClientSize = New Size(fPSz.Width + 1, fPSz.Height + 28) 'New Size(fPSz.Width + 17, fpH + 67)  'TODO: Change to Size!!!

        'bnCancel.Location = New Point(112, fPSz.Height + 2) 'OnResize
        'bnOK.Location = New Point(112 + 83 + 16, fPSz.Height + 2)
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)
        MainFlowLayoutPanel.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    ReadOnly Property MaxRTBTextHeight As Integer
        Get
            Return RTBFontHeight * 20 '*15 
        End Get
    End Property
    Function GetRTBTextWidth(rtbWidth As Integer) As Integer
        Dim max = CInt(ScreenResWAPrim.Width * 0.91) ' Or RTBFontHeight * 108 or MaxPrevSizeMult As Integer = s.PreviewSize \ 100
        Select Case rtbWidth
            Case Is < 194
                rtbWidth = 194
            Case Is > max
                rtbWidth = max
        End Select
        Return rtbWidth
    End Function
    Function GetRTBTextHeight(rtbHeigth As Integer) As Integer
        Dim max = MaxRTBTextHeight
        Dim min = CInt(RTBFontHeight * 1.3)
        Select Case rtbHeigth
            Case Is < min
                rtbHeigth = min
            Case Is > max
                rtbHeigth = max
        End Select
        Return rtbHeigth
    End Function

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        PlaceButtons(Me.ClientSize, MainFlowLayoutPanel.Size)
    End Sub

    Private Sub PlaceButtons(clientSZ As Size, mFlowLPSZ As Size)
        Dim x As Integer 'Buttons to right side Version
        Dim y As Integer
        x = If(clientSZ.Width < mFlowLPSZ.Width + 1, clientSZ.Width, mFlowLPSZ.Width)
        y = If(clientSZ.Height < mFlowLPSZ.Height + 28, clientSZ.Height - 26, mFlowLPSZ.Height + 2)
        bnCancel.Location = New Point(x - 15 - 83 * 2 - 16, y)
        bnOK.Location = New Point(x - 15 - 83, y)

        'y = If(clientSZ.Height < mFlowLPSZ.Height + 28, clientSZ.Height - 26, mFlowLPSZ.Height + 2) 'Buttons to left side Version
        'bnCancel.Location = New Point(116, y)
        'bnOK.Location = New Point(116 + 83 + 16, y)
    End Sub

    Sub MainFlowLayoutPanelLayout() '(sender As Object, e As LayoutEventArgs)
        Dim filterTables = MainFlowLayoutPanel.Controls.OfType(Of FilterTable).ToArray
        If filterTables.Length <= 0 Then Exit Sub
        Dim maxTW = RTBTxtMaxCWidth
        Dim rtHA() As Integer

        If maxTW > 0 AndAlso RTBTxtCHeightA.Length = filterTables.Length Then
            rtHA = RTBTxtCHeightA
        Else
            maxTW = 0
            ReDim rtHA(filterTables.Length - 1)
            Dim s100k As New Size(100000, 100000)
            For f = 0 To filterTables.Length - 1
                Dim rtb As RichTextBoxEx = filterTables(f).rtbScript
                Dim mts As Size = TextRenderer.MeasureText(rtb.Text, rtb.Font, s100k)
                rtHA(f) = GetRTBTextHeight(mts.Height)
                If mts.Width > maxTW Then maxTW = mts.Width
            Next f

            maxTW = GetRTBTextWidth(maxTW)
        End If

        Dim totH As Integer
        MainFlowLayoutPanel.SuspendLayout()
        SuspendLayout()

        For f = 0 To filterTables.Length - 1
            Dim fTb As FilterTable = filterTables(f)
            fTb.SuspendLayout()
            Dim nRtbSz As New Size(maxTW,
                                       rtHA(f) + If(rtHA(f) < MaxRTBTextHeight, RTBFontHeight, 0))
            'rtHA(f) + If(rtHA(f) < MaxRTBTextHeight * 1.05, CInt(RTBFontHeight * 1.1), 0))
            fTb.rtbScript.Size = nRtbSz
            Dim ftH As Integer = Math.Max(nRtbSz.Height, CInt(RTBFontHeight * 1.05))

            fTb.Size = New Size(nRtbSz.Width + RTBFontHeight * 7, ftH)
            totH += ftH
        Next f

        Static sPadW As Integer 'Testings
        Static ScrollON As Boolean
        Dim fpH = MainFlowLayoutPanel.Height
        If fpH > (ScreenResWAPrim.Height - 48 - 88) Then '69-Threshold?? 
            If Not ScrollON Then
                sPadW = 14
                ScrollON = True
                MainFlowLayoutPanel.AutoScroll = True
            End If
        Else
            If ScrollON Then
                sPadW = 0
                ScrollON = False
                MainFlowLayoutPanel.AutoScroll = False
            End If
        End If

        If totH > ScreenResWAPrim.Height - 48 - 72 Then totH = ScreenResWAPrim.Height - 48 - 72 '70??
        'Dim fPSz As Size = New Size(RTBFontHeight * 7 + 6 + maxTW + sPadW, totH + 4) 'H+4() 'MainFlowLayoutPanel.PreferredSize 'No FlowP max Implementation
        Dim fPSz As New Size(Math.Min(ScreenResWAPrim.Width - 39, RTBFontHeight * 7 + 6 + maxTW + sPadW), Math.Min(ScreenResWAPrim.Height - 48 - 70, totH + 4)) '  -1W & 2H than ScrRes less than min!!!
        MainFlowLayoutPanel.Size = fPSz
        fpH = fPSz.Height

        Dim cSZ = Me.ClientSize
        If cSZ.Height < fpH + 28 OrElse cSZ.Width < fPSz.Width + 1 Then
            Me.ClientSize = New Size(fPSz.Width + 1, fpH + 28)  'TODO: Change to Size!!!
        Else
            PlaceButtons(cSZ, fPSz)
        End If
        'bnCancel.Location = New Point(112, fpH + 2) 'Bn OK&Canc To FlowPanel Size REsize Event
        'bnOK.Location = New Point(112 + 83 + 16, fpH + 2)

        For f = 0 To filterTables.Length - 1
            filterTables(f).ResumeLayout(False) ''Faster 1.5x ! Slight Flicker with LaySusp 
        Next f
        MainFlowLayoutPanel.ResumeLayout()
        ResumeLayout(False)
    End Sub

    Function CreateFilterTable(filter As VideoFilter) As FilterTable
        Return New FilterTable(filter, Me, New Font("Consolas", 10 * s.UIScaleFactor), New Size(304, 64)) 'TEst this !!! Seems Not Good!!!To Do!! Better Speparate Function to calculate RTB.SIZE
    End Function

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
        Dim script As New VideoScript With {.Engine = Engine}
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

        If script IsNot Nothing Then
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
        firstTable.rtbScript.Text = String.Join(BR, MainFlowLayoutPanel.Controls.OfType(Of FilterTable).ToArray.SelectF(
                                                Function(arg) If(arg.cbActive.Checked, arg.rtbScript.Text.Trim, "#" & arg.rtbScript.Text.Trim.FixBreak.Replace(BR, "# " & BR)))) & BR2 & BR2
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
        Protected Overrides ReadOnly Property DefaultMargin As Padding
            Get
                Return Padding.Empty
            End Get
        End Property

        Sub New(filter As VideoFilter, editorForm As CodeEditor, rtbScriptFont As Font, rtbSize As Size)
            Me.SuspendLayout()
            Editor = editorForm
            Me.Font = editorForm.Font
            Menu = New ContextMenuStripEx 'With {.Form = Editor} 'Is This Form Needed ???
            Dim fh As Integer = editorForm.RTBFontHeight
            Dim noPad As Padding = Padding.Empty

            cbActive = New CheckBox With {.Margin = noPad,
                                          .Checked = filter.Active,
                                          .Size = New Size(fh * 7, CInt(fh * 1.05)),
                                          .Text = filter.Category}

            tbName = New TextEdit With {.Margin = noPad,
                                        .Size = New Size(fh * 7, CInt(fh * 1.25)),
                                        .Text = filter.Name}  'w:fh*6-7

            rtbScript = New RichTextBoxEx(False) With {.Margin = noPad,
                                                       .AcceptsTab = True,
                                                       .BorderStyle = BorderStyle.None,
                                                       .EnableAutoDragDrop = True,
                                                       .Font = rtbScriptFont,
                                                       .ScrollBars = RichTextBoxScrollBars.Vertical, 'None = no RichTextBoxEX content onShow Error !!!
                                                       .WordWrap = False,
                                                       .Size = rtbSize,
                                                       .Text = If(filter.Script Is Nothing, "", filter.Script & BR)}

            SetColor()
            ColumnCount = 2
            ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, fh * 7))
            ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F)) '(SizeType.AutoSize, rtbSize.Width)) 'SizeType.Percent, 100.0 '%Slowish with AutoSize - Auto is faster
            RowCount = 2
            RowStyles.Add(New RowStyle(SizeType.Absolute, CInt(fh * 1.05)))
            RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F)) '(SizeType.AutoSize, CInt(Math.Max(fh * 1.25, rtbSize.Height - fh * 1.05))))
            Controls.Add(cbActive, 0, 0)
            Controls.Add(tbName, 0, 1)
            SetRowSpan(rtbScript, 2)
            Controls.Add(rtbScript, 1, 0)

            Size = New Size(rtbSize.Width + fh * 7, Math.Max(rtbSize.Height, CInt(fh * 1.05)))
            Me.ResumeLayout(False)

            Dim caceh As EventHandler = Sub() SetColor()
            AddHandler cbActive.CheckedChanged, caceh
            AddHandler rtbScript.MouseUp, AddressOf HandleMouseUp
            Dim reeh As EventHandler = Sub() editorForm.ActiveTable = Me
            AddHandler rtbScript.Enter, reeh
            Dim rtceh As EventHandler = Sub()
                                            If RTBEventSem Then Return
                                            Dim layT = Task.Run(Sub()
                                                                    If RTBEventSem Then Return
                                                                    SyncLock editorForm.FilterTabSLock
                                                                        If RTBEventSem OrElse Me.IsDisposed OrElse Not editorForm.IsHandleCreated Then Return
                                                                        RTBEventSem = True
                                                                        Thread.Sleep(170) ' 150-210
                                                                    End SyncLock
                                                                    If Me.IsDisposed OrElse Not editorForm.IsHandleCreated Then Return

                                                                    Dim pcfa = Parent.Controls.OfType(Of FilterTable)().ToArray
                                                                    If pcfa.Length <> editorForm.RTBTxtCHeightA.Length Then
                                                                        editorForm.RTBTxtMaxCWidth = 0
                                                                        ReDim editorForm.RTBTxtCHeightA(pcfa.Length - 1)
                                                                    End If

                                                                    Dim rtbTxtA(pcfa.Length - 1) As String
                                                                    Dim gt As MethodInvoker = Sub()
                                                                                                  For r = 0 To pcfa.Length - 1
                                                                                                      rtbTxtA(r) = pcfa(r).rtbScript.Text
                                                                                                  Next r
                                                                                              End Sub
                                                                    If Not editorForm.IsHandleCreated Then Return
                                                                    editorForm.Invoke(gt) 'Thread Safe Call

                                                                    'For r = 0 To pcfa.Length - 1 : rtbTxtA(r) = pcfa(r).rtbScript.Text : Next r 'unsafe
                                                                    RTBEventSem = False

                                                                    Dim maxTW As Integer
                                                                    Dim tmts As Size
                                                                    Dim s100k As New Size(100000, 100000)

                                                                    For i = 0 To pcfa.Length - 1
                                                                        Dim mts As Size = TextRenderer.MeasureText(rtbTxtA(i), rtbScriptFont, s100k)
                                                                        editorForm.RTBTxtCHeightA(i) = editorForm.GetRTBTextHeight(mts.Height)
                                                                        If mts.Width > maxTW Then maxTW = mts.Width
                                                                        If pcfa(i) Is Me Then tmts = mts
                                                                    Next i

                                                                    maxTW = editorForm.GetRTBTextWidth(maxTW)
                                                                    editorForm.RTBTxtMaxCWidth = maxTW

                                                                    'tmts.Width > maxTW  This makes NonSense???
                                                                    If tmts.Width > maxTW OrElse (tmts.Width = maxTW AndAlso tmts.Width <> LastTextSize.Width) OrElse
                                                                        (LastTextSize.Height <> tmts.Height AndAlso tmts.Height > fh) Then
                                                                        LastTextSize = tmts
                                                                        If editorForm.IsHandleCreated Then editorForm.BeginInvoke(Sub() editorForm.MainFlowLayoutPanelLayout())
                                                                    End If
                                                                End Sub)
                                            Dim lTc = layT.ContinueWith(Sub()
                                                                            If layT.Exception IsNot Nothing Then
                                                                                RTBEventSem = False
                                                                                editorForm.RTBTxtMaxCWidth = 0
                                                                                editorForm.RTBTxtCHeightA = {}
                                                                                g.ShowException(layT.Exception.InnerException, "CodeEditor Layout TextChanged Event Task Exception", layT.Exception.ToString)
                                                                            End If
                                                                        End Sub) 'debug
                                        End Sub

            AddHandler rtbScript.TextChanged, rtceh
            Dim deh As EventHandler = Sub()
                                          RTBEventSem = True
                                          'Menu?.Items.ClearAndDispose
                                          Menu?.Dispose()
                                          'Me.Events.Dispose() '???
                                          RemoveHandler Me.Disposed, deh
                                          RemoveHandler cbActive.CheckedChanged, caceh
                                          RemoveHandler rtbScript.MouseUp, AddressOf HandleMouseUp
                                          RemoveHandler rtbScript.Enter, reeh
                                          RemoveHandler rtbScript.TextChanged, rtceh
                                          Editor = Nothing ' This errors?!!!
                                      End Sub
            AddHandler Disposed, deh
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
                Dim arg As String = argument.Replace(" ", "").ToLower(InvCult)

                For Each parameter In parameters.Parameters
                    If arg.Contains(parameter.Name.Replace(" ", "").ToLower(InvCult) & "=") Then
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
                    value = value.Replace(""""c, "'"c)
                End If

                newParameters.Add(parameter.Name & "=" & value)
            Next

            rtbScript.Text = Regex.Replace(code, parameters.FunctionName & "\((.+)\)",
                                           parameters.FunctionName & "(" & String.Join(", ", newParameters.ToArray) & ")")
        End Sub

        Sub HandleMouseUp(sender As Object, e As MouseEventArgs)
            If e.Button <> MouseButtons.Right Then Exit Sub

            Menu.SuspendLayout()
            ActionMenuItem.LayoutSuspendCreate(48)
            Dim mItms As ToolStripItemCollection = Menu.Items
            mItms.ClearAndDispose()
            Dim code = rtbScript.Text.FixBreak

            Dim fDefAr As FilterParameters() = FilterParameters.Definitions
            For i = 0 To fDefAr.Length - 1
                Dim fDef = fDefAr(i)
                Dim fFunName As String = fDef.FunctionName
                If code.Contains(fFunName & "(") Then
                    If Regex.Match(code, fFunName & "\((.+)\)").Success Then
                        ActionMenuItem.Add(mItms, fDef.Text, Sub() SetParameters(fDef))
                    End If
                End If
            Next i

            Dim cbATxt As String = cbActive.Text
            Dim filterProfiles = If(p.Script.Engine = ScriptEngine.AviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
            For Each cat In filterProfiles
                If String.Equals(cat.Name, cbATxt) Then
                    mItms.Add(New ToolStripMenuItemEx(cat.Name))
                    Dim filtL As List(Of VideoFilter) = cat.Filters
                    Dim fc1 As Boolean = filtL.Count > 1
                    For Each iFilter In filtL
                        ActionMenuItem.Add(mItms, If(fc1, iFilter.Category & " | ", "") & iFilter.Path, Sub() ReplaceClick(iFilter.GetCopy), iFilter.Script)
                    Next iFilter
                End If
            Next cat

            Dim filterMenuItem = New ActionMenuItem("Add, insert or replace a filter   ", ImageHelp.GetImageC(Symbol.Filter))
            Dim isPrSrcF As Boolean = p.SourceFile.NotNullOrEmptyS
            Dim isTSel As Boolean = rtbScript.SelectionLength > 0
            Dim helpMenuItem = New ToolStripMenuItemEx("Help", ImageHelp.GetImageC(Symbol.Help))
            helpMenuItem.DropDownItems.Add(New ToolStripMenuItem("t"))

            mItms.AddRange({filterMenuItem,
                New ActionMenuItem("Remove", AddressOf RemoveClick, ImageHelp.GetImageC(Symbol.Remove)) With {.KeyDisplayString = "Ctrl+Delete"},
                New ActionMenuItem("Preview Video...", AddressOf Editor.VideoPreview, ImageHelp.GetImageC(Symbol.Photo), "Previews the script with solved macros.") With {.Enabled = isPrSrcF, .KeyDisplayString = "F5"},
                New ActionMenuItem("Play with mpv.net", AddressOf Editor.PlayScriptWithMPV, ImageHelp.GetImageC(Symbol.Play), "Plays the current script with mpv.net.") With {.Enabled = isPrSrcF, .KeyDisplayString = "F9"},
                New ActionMenuItem("Play with mpc", AddressOf Editor.PlayScriptWithMPC, ImageHelp.GetImageC(Symbol.Play), "Plays the current script with MPC.") With {.Enabled = isPrSrcF, .KeyDisplayString = "F10"},
                New ActionMenuItem("Preview Code...", AddressOf CodePreview, ImageHelp.GetImageC(Symbol.Code), "Previews the script with solved macros."),
                New ActionMenuItem("Info...", AddressOf Editor.ShowInfo, ImageHelp.GetImageC(Symbol.Info), "Previews script parameters such as framecount and colorspace.") With {.Enabled = isPrSrcF, .KeyDisplayString = "Ctrl+I"},
                New ActionMenuItem("Advanced Info...", AddressOf Editor.ShowAdvancedInfo, ImageHelp.GetImageC(Symbol.Lightbulb)) With {.Enabled = .Enabled = isPrSrcF},
                New ActionMenuItem("Join Filters", AddressOf Editor.JoinFilters, ImageHelp.GetImageC(Symbol.Link), "Joins all filters into one filter.") With
                                    {.Enabled = DirectCast(Parent, FlowLayoutPanel).Controls.Count > 1, .ShortcutKeyDisplayString = "Ctrl+J"},
                New ActionMenuItem("Profiles...", AddressOf g.MainForm.ShowFilterProfilesDialog, ImageHelp.GetImageC(Symbol.FavoriteStar), "Dialog to edit profiles.") With {.KeyDisplayString = "Ctrl+P"},
                New ActionMenuItem("Macros...", AddressOf MacrosForm.ShowDialogForm, ImageHelp.GetImageC(Symbol.CalculatorPercentage), "Dialog to choose macros.") With {.KeyDisplayString = "Ctrl+M"},
                New ToolStripSeparator,
                New ActionMenuItem("Move Up", AddressOf MoveUp, ImageHelp.GetImageC(Symbol.Up)) With {.KeyDisplayString = "Ctrl+Up"},
                New ActionMenuItem("Move Down", AddressOf MoveDown, ImageHelp.GetImageC(Symbol.Down)) With {.KeyDisplayString = "Ctrl+Down"},
                New ToolStripSeparator,
                New ActionMenuItem("Cut", Sub()
                                              Clipboard.SetText(rtbScript.SelectedText)
                                              rtbScript.SelectedText = ""
                                          End Sub, ImageHelp.GetImageC(Symbol.Cut)) With {.Enabled = isTSel AndAlso Not rtbScript.ReadOnly, .KeyDisplayString = "Ctrl+X"},
                New ActionMenuItem("Copy", Sub() Clipboard.SetText(rtbScript.SelectedText), ImageHelp.GetImageC(Symbol.Copy)) With {.Enabled = isTSel, .KeyDisplayString = "Ctrl+C"},
                New ActionMenuItem("Paste", Sub()
                                                rtbScript.SelectedText = Clipboard.GetText
                                                rtbScript.ScrollToCaret()
                                            End Sub, ImageHelp.GetImageC(Symbol.Paste)) With {.Enabled = Clipboard.ContainsText AndAlso Not rtbScript.ReadOnly, .KeyDisplayString = "Ctrl+V"},
                New ToolStripSeparator, helpMenuItem})

            filterMenuItem.DropDown.SuspendLayout()
            Dim filterMDDI As ToolStripItemCollection = filterMenuItem.DropDownItems
            filterMDDI.Add(New ActionMenuItem("Empty Filter", Sub() FilterClick(New VideoFilter("Misc", "", "")), "Filter with empty values."))

            For Each filterCategory In filterProfiles
                Dim fCatN As String = filterCategory.Name & " | "
                For Each filter In filterCategory.Filters
                    Dim tip = filter.Script
                    ActionMenuItem.Add(filterMDDI, fCatN & filter.Path, Sub() FilterClick(filter.GetCopy), tip)
                Next filter
            Next filterCategory
            filterMenuItem.DropDown.ResumeLayout(False)
            ActionMenuItem.LayoutResume(False)
            Menu.ResumeLayout()

            Dim hmoeh As EventHandler = Sub()
                                            RemoveHandler helpMenuItem.DropDownOpening, hmoeh
                                            PopulateHelpMenu(helpMenuItem)
                                        End Sub
            AddHandler helpMenuItem.DropDownOpening, hmoeh
            Menu.Show(rtbScript, e.Location)
        End Sub

        Private Sub PopulateHelpMenu(helpMenuItm As ToolStripMenuItemEx)
            Menu.SuspendLayout()
            helpMenuItm.DropDown.SuspendLayout()
            helpMenuItm.DropDownItems(0).Visible = False

            Dim pPHS As New HashSet(Of Package)(7)
            Dim rtbTxt = rtbScript.Text
            Dim helpMenuIList As New List(Of ToolStripItem)(32)
            Dim avsEn As Boolean = p.Script.Engine = ScriptEngine.AviSynth
            Dim plugPkgsIA = Package.Items.Values.OfType(Of PluginPackage).ToArray

            For f = 1 To 2
                Dim fEngnDesc As String = If(avsEn, If(f = 1, " AVS", " VS"), If(f = 1, " VS", " AVS"))
                For i = 0 To plugPkgsIA.Length - 1
                    Dim pluginPack As PluginPackage = plugPkgsIA(i)
                    Dim filtNamesA As String() = If(avsEn, If(f = 1, pluginPack.AvsFilterNames, pluginPack.VSFilterNames), If(f = 1, pluginPack.VSFilterNames, pluginPack.AvsFilterNames))
                    If filtNamesA IsNot Nothing Then
                        For n = 0 To filtNamesA.Length - 1
                            If rtbTxt.Contains(filtNamesA(n)) Then
                                If pPHS.Add(pluginPack) Then helpMenuIList.Add(New ActionMenuItem(pluginPack.Name & fEngnDesc, Sub() pluginPack.ShowHelp(), pluginPack.Description))
                            End If
                        Next n
                    End If
                Next i
            Next f

            If avsEn Then
                helpMenuIList.Add(New ActionMenuItem("AviSynth Help", Sub() g.ShellExecute("http://avisynth.nl"), "http://avisynth.nl"))
            Else
                helpMenuIList.Add(New ActionMenuItem("VapourSynth Help", Sub() Package.VapourSynth.ShowHelp(), Package.VapourSynth.HelpFileOrURL))
            End If

            helpMenuIList.Add(New ToolStripSeparator)
            Dim invCultTI As Globalization.TextInfo = InvCultTxtInf
            Dim hc As Integer = helpMenuIList.Count
            Dim mL2LL As New List(Of List(Of ActionMenuItem))(28)

            For i = 0 To plugPkgsIA.Length - 1
                Dim pp = plugPkgsIA(i)
                If If(avsEn, pp.AvsFilterNames IsNot Nothing, pp.VSFilterNames IsNot Nothing) Then
                    Dim nML2 As List(Of ActionMenuItem)
                    Dim lastCH As Char
                    Dim c1 As Char = invCultTI.ToUpper(pp.Name(0))
                    If c1 <> lastCH Then 'source is sorted, if not: HashSet
                        lastCH = c1
                        helpMenuIList.Add(New ToolStripMenuItemEx(c1))
                        nML2 = New List(Of ActionMenuItem)(4)
                        mL2LL.Add(nML2)
                    End If
                    nML2.Add(New ActionMenuItem(pp.Name, Sub() pp.ShowHelp(), pp.Description))
                End If
            Next i

            helpMenuItm.DropDownItems.AddRange(helpMenuIList.ToArray)
            For i = 0 To mL2LL.Count - 1
                Dim nMI As ToolStripMenuItem = DirectCast(helpMenuIList(i + hc), ToolStripMenuItem)
                nMI.DropDown.SuspendLayout()
                nMI.DropDownItems.AddRange(mL2LL(i).ToArray)
                nMI.DropDown.ResumeLayout(False)
            Next i

            helpMenuItm.DropDown.ResumeLayout(False)
            Menu.ResumeLayout(False)
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
            Dim script As New VideoScript With {
                .Engine = Editor.Engine,
                .Filters = Editor.GetFilters()}
            g.CodePreview(script.GetFullScript)
        End Sub

        Sub RemoveClick()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)

            If flow.Controls.Count > 1 Then
                If MsgQuestion("Remove?") = DialogResult.OK Then
                    flow.Controls.Remove(Me)
                    Editor.ActiveTable = Nothing
                    Dispose()
                End If
            End If
        End Sub

        Sub FilterClick(filter As VideoFilter)
            Using td As New TaskDialog(Of String)
                td.MainInstruction = "Choose action"
                td.AddCommand("Replace selection", "Replace")
                td.AddCommand("Insert at selection", "Insert")
                td.AddCommand("Add to end", "Add")
                Dim fScriptStr0 As Boolean = filter.Script.Length > 0 AndAlso filter.Script(0) = "$"

                Select Case td.Show
                    Case "Replace"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        cbActive.Checked = filter.Active
                        cbActive.Text = filter.Category

                        If Not tup.Value.Equals(filter.Script) AndAlso tup.Caption.NotNullOrEmptyS Then
                            If fScriptStr0 Then
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
                        Menu.Items.ClearAndDispose
                    Case "Insert"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        If Not tup.Value.Equals(filter.Script) AndAlso tup.Caption.NotNullOrEmptyS Then
                            If fScriptStr0 Then
                                filter.Path = tup.Caption
                            Else
                                filter.Path = filter.Path.Replace("...", "") + " " + tup.Caption
                            End If
                        End If

                        filter.Script = tup.Value
                        Dim flow = DirectCast(Parent, FlowLayoutPanel)
                        Dim index = flow.Controls.IndexOf(Me)
                        Dim filterTable = Editor.CreateFilterTable(filter)
                        flow.SuspendLayout()
                        flow.Controls.Add(filterTable)
                        flow.Controls.SetChildIndex(filterTable, index)
                        flow.ResumeLayout()

                        Editor.MainFlowLayoutPanelLayout() 'Test this!!!

                        filterTable.rtbScript.SelectionStart = filterTable.rtbScript.Text.Length
                        filterTable.rtbScript.Focus()
                        Application.DoEvents()
                        Menu.Items.ClearAndDispose
                    Case "Add"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        If Not tup.Value.Equals(filter.Script) AndAlso tup.Caption.NotNullOrEmptyS Then
                            If fScriptStr0 Then
                                filter.Path = tup.Caption
                            Else
                                filter.Path = filter.Path.Replace("...", "") & " " & tup.Caption
                            End If
                        End If

                        filter.Script = tup.Value
                        Dim flow = DirectCast(Parent, FlowLayoutPanel)
                        Dim filterTable = Editor.CreateFilterTable(filter)
                        flow.Controls.Add(filterTable)

                        Editor.MainFlowLayoutPanelLayout() 'Test this!!!

                        filterTable.rtbScript.SelectionStart = filterTable.rtbScript.Text.Length
                        filterTable.rtbScript.Focus()
                        Application.DoEvents()
                        Menu.Items.ClearAndDispose
                End Select
            End Using
        End Sub

        Sub ReplaceClick(filter As VideoFilter)
            Dim fScrpt As String = filter.Script
            Dim tup = Macro.ExpandGUI(fScrpt)

            If tup.Cancel Then
                Exit Sub
            End If

            cbActive.Checked = filter.Active
            cbActive.Text = filter.Category

            If Not tup.Value.Equals(fScrpt) AndAlso tup.Caption.NotNullOrEmptyS Then
                If fScrpt.Length > 0 AndAlso fScrpt(0) = "$" Then
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
            Menu.Items.ClearAndDispose
        End Sub
    End Class
End Class
