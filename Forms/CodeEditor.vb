
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports JM.LinqFaster
Imports Microsoft.Win32

Imports StaxRip.UI

Public Class CodeEditor
    Private ScrollON As Boolean
    'Private MaxPrevSizeMult As Integer = s.PreviewSize \ 100 'Test- Max Width
    Public RTBTxtCHeightA As Integer()
    Public RTBTxtMaxCWidth As Integer
    Public FilterTabSLock As New Object
    Public RTBFontHeight As Integer ' = 16
    'Public MenuImageDict As Dictionary(Of Symbol, Image) 'Experiment ' Test this
    Property ActiveTable As FilterTable
    Property Engine As ScriptEngine

    Public sssw As New Stopwatch 'debug !!!!!!!
    Public ttt2 As String

    Protected Overrides ReadOnly Property DefaultPadding As Padding
        Get
            Return New System.Windows.Forms.Padding(0, 0, 1, 3)
        End Get
    End Property
    Protected Overrides ReadOnly Property DefaultMinimumSize As Size
        Get
            Return New System.Drawing.Size(321, 127)
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
        MaximumSize = New Size((ScreenResPrim.Width - 18), (ScreenResPrim.Height - 30))
        MainFlowLayoutPanel.MaximumSize = New Size((ScreenResPrim.Width - 38), (ScreenResPrim.Height - 32 - 68))

        Dim rtbScrFont As Font = New Font("Consolas", 10.0F * s.UIScaleFactor)
        RTBFontHeight = rtbScrFont.Height

        Dim maxTW As Integer
        Dim s100k As Size = New Size(100000, 100000)
        Dim filters As List(Of VideoFilter) = doc.Filters
        Dim rtHA(filters.Count - 1) As Integer
        Dim mts As Size
        For f = 0 To rtHA.Length - 1
            mts = TextRenderer.MeasureText(If(filters(f).Script Is Nothing, "", filters(f).Script & BR), rtbScrFont, s100k)
            rtHA(f) = mts.Height
            If mts.Width > maxTW Then maxTW = mts.Width
        Next f

        maxTW = GetRTBTextWidth(maxTW)
        RTBTxtMaxCWidth = maxTW
        Dim totH As Integer
        ReDim RTBTxtCHeightA(rtHA.Length - 1)
        Dim th As Integer
        For f = 0 To rtHA.Length - 1
            th = GetRTBTextHeight(rtHA(f))
            RTBTxtCHeightA(f) = th
            'tH += If(tH < MaxRTBTextHeight * 1.05, CInt(RTBFontHeight * 1.1), 0)
            th += If(rtHA(f) < MaxRTBTextHeight, RTBFontHeight, 0)
            MainFlowLayoutPanel.Controls.Add(New FilterTable(filters(f), Me, rtbScrFont, New Size(maxTW, th)))

            totH += Math.Max(th, CInt(RTBFontHeight * 1.05))
        Next f

        Dim fPSz As Size = New Size(RTBFontHeight * 7 + 6 + maxTW, totH + 4)  'MainFlowLayoutPanel.PreferredSize
        MainFlowLayoutPanel.Size = fPSz
        Dim fpH As Integer = fPSz.Height
        Me.ClientSize = New Size(fPSz.Width + 1, fpH + 28) 'New Size(fPSz.Width + 17, fpH + 67) 

        bnCancel.Location = New Point(112, fpH + 2)
        bnOK.Location = New Point(112 + 83 + 16, fpH + 2)

        sssw.Stop()
        ttt2 &= sssw.ElapsedTicks / SWFreq & "msNew| "
        sssw.Restart()
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)
        MainFlowLayoutPanel.ResumeLayout(False)
        ResumeLayout(False)
        'MainFlowLayoutPanelLayout(Nothing, Nothing)

        sssw.Stop()
        ttt2 &= sssw.ElapsedTicks / SWFreq & "msLoad| "
        sssw.Restart()
    End Sub
    Protected Overrides Sub OnShown(e As EventArgs) 'Debug
        MyBase.OnShown(e)

        'Log.Write("PerfSize | Size | ClientSize", $"Me:{PreferredSize} {Size} {ClientSize}{BR} FP:{MainFlowLayoutPanel.PreferredSize} {MainFlowLayoutPanel.Size} {MainFlowLayoutPanel.ClientSize}{BR} FT:{MainFlowLayoutPanel.Controls(0).Size} {MainFlowLayoutPanel.Controls(0).ClientSize} {MainFlowLayoutPanel.Controls(0).PreferredSize}")
        'AddHandler MainFlowLayoutPanel.Layout, AddressOf MainFlowLayoutPanelLayout
        '  CreateMenuImageCache()

        sssw.Stop()
        Text = ttt2 & sssw.ElapsedTicks / SWFreq & "msShow"
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs) 'Debug
        Me.Text = "Code Editor"
        'ActionMenuItem.LayoutSuspendList = Nothing
        MyBase.OnFormClosing(args)
    End Sub

    'Sub CreateMenuImageCache()
    '    Task.Run(Sub()
    '                 Thread.Sleep(45)
    '                 If MenuImageDict Is Nothing AndAlso Me.IsHandleCreated Then
    '                     MenuImageDict = New Dictionary(Of Symbol, Image)(17)
    '                     For Each smb In {Symbol.Filter, Symbol.Remove, Symbol.Photo, Symbol.Play, Symbol.Code, Symbol.Info, Symbol.Lightbulb, Symbol.FavoriteStar,
    '                                          Symbol.CalculatorPercentage, Symbol.Up, Symbol.Down, Symbol.Cut, Symbol.Copy, Symbol.Paste, Symbol.Help}
    '                         MenuImageDict.Add(smb, ImageHelp.GetSymbolImage(smb))
    '                     Next smb
    '                 End If
    '             End Sub)
    'End Sub

    ReadOnly Property MaxRTBTextHeight As Integer
        Get
            Return RTBFontHeight * 20 '*15 
        End Get
    End Property
    Function GetRTBTextWidth(rtbWidth As Integer) As Integer
        Dim max = CInt(ScreenResPrim.Width * 0.91) ' Or RTBFontHeight * 108 or MaxPrevSizeMult As Integer = s.PreviewSize \ 100
        Select Case rtbWidth
            Case Is < 176
                rtbWidth = 176
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
            Dim mts As Size
            Dim s100k As Size = New Size(100000, 100000)
            Dim rtb As RichTextBoxEx
            For f = 0 To filterTables.Length - 1
                rtb = filterTables(f).rtbScript
                mts = TextRenderer.MeasureText(rtb.Text, rtb.Font, s100k)
                rtHA(f) = GetRTBTextHeight(mts.Height)
                If mts.Width > maxTW Then maxTW = mts.Width
            Next f

            maxTW = GetRTBTextWidth(maxTW)
        End If

        Dim totH As Integer
        Dim fTb As FilterTable
        MainFlowLayoutPanel.SuspendLayout()
        SuspendLayout()

        For f = 0 To filterTables.Length - 1
            fTb = filterTables(f)
            fTb.SuspendLayout()
            Dim nRtbSz As Size = New Size(maxTW,
                                          rtHA(f) + If(rtHA(f) < MaxRTBTextHeight, RTBFontHeight, 0))
            'rtHA(f) + If(rtHA(f) < MaxRTBTextHeight * 1.05, CInt(RTBFontHeight * 1.1), 0))
            fTb.rtbScript.Size = nRtbSz
            Dim ftH As Integer = Math.Max(nRtbSz.Height, CInt(RTBFontHeight * 1.05))

            fTb.Size = New Size(nRtbSz.Width + RTBFontHeight * 7, ftH)
            totH += ftH
        Next f

        Static sPadW As Integer 'Testings
        Dim fpH = MainFlowLayoutPanel.Height
        If fpH > (ScreenResPrim.Height - 32 - 88) Then '69-Threshold?? This needs Work ToDO!!!
            If Not ScrollON Then
                'Dim sssi = SystemInformation.VerticalScrollBarWidth  Test sPadW Here, maybe use this scroll width function!!!!
                sPadW = 14
                ScrollON = True
                MainFlowLayoutPanel.AutoScroll = True
                Task.Run(Sub() Console.Beep(7500, 50))
            End If
        Else
            If ScrollON Then
                sPadW = 0
                ScrollON = False
                MainFlowLayoutPanel.AutoScroll = False
                Task.Run(Sub() Console.Beep(400, 50))
            End If
        End If

        If totH > ScreenResPrim.Height - 32 - 72 Then totH = ScreenResPrim.Height - 32 - 72 '70??This needs Work ToDO!!!
        'Dim fPSz As Size = New Size(RTBFontHeight * 7 + 6 + maxTW + sPadW, totH + 4) 'H+4() 'MainFlowLayoutPanel.PreferredSize 'No FlowP max Implementation
        Dim fPSz As Size = New Size(Math.Min(ScreenResPrim.Width - 39, RTBFontHeight * 7 + 6 + maxTW + sPadW), Math.Min(ScreenResPrim.Height - 32 - 70, totH + 4)) '  -1W & 2H than ScrRes less than min!!!
        'New Size((ScreenResPrim.Width - 38), (ScreenResPrim.Height - 32 - 68))
        MainFlowLayoutPanel.Size = fPSz
        fpH = fPSz.Height

        Dim mcs = Me.ClientSize
        If mcs.Height < fpH + 28 OrElse mcs.Width < fPSz.Width + 1 Then
            Me.ClientSize = New Size(fPSz.Width + 1, fpH + 28)
        End If

        bnCancel.Location = New Point(112, fpH + 2)
        bnOK.Location = New Point(112 + 83 + 16, fpH + 2)

        For f = 0 To filterTables.Length - 1
            filterTables(f).ResumeLayout(False) ''Faster 1.5x ! Slight Flicker with LaySusp 
        Next f
        MainFlowLayoutPanel.ResumeLayout()
        ResumeLayout(False)
    End Sub

    Function CreateFilterTable(filter As VideoFilter) As FilterTable
        'RTBTxtMaxCWidth = 0
        'RTBTxtCHeightA = {}
        Dim ft As FilterTable = New FilterTable(filter, Me, New Font("Consolas", 10.0F * s.UIScaleFactor), New Size(300, 70)) 'TEst this !!! Seems Not Good!!!To Do!! Better Speparate Function to calculate RTB.SIZE
        Return ft
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
        Protected Overrides ReadOnly Property DefaultMargin As Padding
            Get
                Return Padding.Empty
            End Get
        End Property

        Sub New(filter As VideoFilter, editorForm As CodeEditor, rtbScriptFont As Font, rtbSize As Size)
            Me.SuspendLayout()
            Editor = editorForm
            Me.Font = editorForm.Font
            Menu = New ContextMenuStripEx With {.Form = Editor}
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
                                                                    SyncLock editorForm.FilterTabSLock
                                                                        If RTBEventSem OrElse Me.IsDisposed Then Return
                                                                        RTBEventSem = True
                                                                        Thread.Sleep(170) ' 150-210
                                                                        If Me.IsDisposed OrElse Not editorForm.IsHandleCreated Then Return

                                                                        Dim sss = Stopwatch.StartNew
                                                                        Dim ttt As String
                                                                        sss.Restart()

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
                                                                        editorForm.Invoke(gt) 'Thread Safe Call
                                                                        'For r = 0 To pcfa.Length - 1 : rtbTxtA(r) = pcfa(r).rtbScript.Text : Next r 'unsafe

                                                                        Dim maxTW As Integer
                                                                        Dim tmts As Size
                                                                        Dim s100k As Size = New Size(100000, 100000)
                                                                        Dim mts As Size

                                                                        For i = 0 To pcfa.Length - 1
                                                                            mts = TextRenderer.MeasureText(rtbTxtA(i), rtbScriptFont, s100k)
                                                                            editorForm.RTBTxtCHeightA(i) = editorForm.GetRTBTextHeight(mts.Height)
                                                                            If mts.Width > maxTW Then maxTW = mts.Width
                                                                            If pcfa(i) Is Me Then tmts = mts
                                                                        Next i

                                                                        maxTW = editorForm.GetRTBTextWidth(maxTW)
                                                                        editorForm.RTBTxtMaxCWidth = maxTW
                                                                        RTBEventSem = False

                                                                        'tmts.Width > maxTW  This makes NonSense???
                                                                        If tmts.Width > maxTW OrElse (tmts.Width = maxTW AndAlso tmts.Width <> LastTextSize.Width) OrElse
                                                                        (LastTextSize.Height <> tmts.Height AndAlso tmts.Height > fh) Then
                                                                            LastTextSize = tmts
                                                                            editorForm.BeginInvoke(Sub()
                                                                                                       sss.Restart()
                                                                                                       'Task.Run(Sub() Console.Beep(900, 15)) 'debug
                                                                                                       editorForm.MainFlowLayoutPanelLayout()
                                                                                                       'Parent.PerformLayout()
                                                                                                       sss.Stop()
                                                                                                       editorForm.Text = ttt & CStr(sss.ElapsedTicks / SWFreq) & "msLayM"
                                                                                                   End Sub)
                                                                        End If

                                                                        sss.Stop()
                                                                        ttt &= Parent.Size.ToString & "FP" & CStr(sss.ElapsedTicks / SWFreq) & "ms|RtMax:" & tmts.ToString
                                                                    End SyncLock

                                                                End Sub)
                                            Dim lTc = layT.ContinueWith(Sub()
                                                                            If layT.Exception IsNot Nothing Then
                                                                                RTBEventSem = False
                                                                                editorForm.RTBTxtMaxCWidth = 0
                                                                                editorForm.RTBTxtCHeightA = {}
                                                                                Console.Beep(7200, 300)
                                                                            End If
                                                                        End Sub) 'debug
                                        End Sub

            AddHandler rtbScript.TextChanged, rtceh
            Dim deh As EventHandler = Sub()
                                          RTBEventSem = True
                                          'Menu?.Items.ClearAndDisplose
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

        Sub HandleMouseUp(sender As Object, e As MouseEventArgs)
            If e.Button <> MouseButtons.Right Then
                Exit Sub
            End If

            Dim TTTT As String 'Debug!!!
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
                If String.Equals(i.Name, cbActive.Text) Then
                    Dim cat = i
                    Dim catMenuItem = Menu.Add2(i.Name & " ")

                    For Each iFilter In cat.Filters
                        Dim tip = iFilter.Script
                        ActionMenuItem.Add(Menu.Items, If(cat.Filters.Count > 1, iFilter.Category + " | ", "") + iFilter.Path, AddressOf ReplaceClick, iFilter.GetCopy, tip)
                    Next
                End If
            Next

            'Dim mImageD = Editor.MenuImageDict
            Dim mImageD = Function(ss As Symbol) ImageHelp.GetImageCache(ss)
            Dim filterMenuItem = Menu.Add2("Add, insert or replace a filter   ", mImageD(Symbol.Filter))
            filterMenuItem.DropDown.SuspendLayout()

            ActionMenuItem.Add(filterMenuItem.DropDownItems, "Empty Filter", AddressOf FilterClick, New VideoFilter("Misc", "", ""), "Filter with empty values.")

            For Each filterCategory In filterProfiles
                For Each filter In filterCategory.Filters
                    Dim tip = filter.Script
                    ActionMenuItem.Add(filterMenuItem.DropDownItems, filterCategory.Name + " | " + filter.Path, AddressOf FilterClick, filter.GetCopy, tip)
                Next
            Next
            filterMenuItem.DropDown.ResumeLayout(False)

            Dim removeMenuItem = Menu.Add2("Remove", AddressOf RemoveClick, mImageD(Symbol.Remove))
            removeMenuItem.KeyDisplayString = "Ctrl+Delete"

            Dim isPrSrcF As Boolean = p.SourceFile.NotNullOrEmptyS
            Dim previewMenuItem = Menu.Add2("Preview Video...", AddressOf Editor.VideoPreview, mImageD(Symbol.Photo), "Previews the script with solved macros.")
            previewMenuItem.Enabled = isPrSrcF
            previewMenuItem.KeyDisplayString = "F5"

            Dim mpvnetMenuItem = Menu.Add2("Play with mpv.net", AddressOf Editor.PlayScriptWithMPV, mImageD(Symbol.Play), "Plays the current script with mpv.net.")
            mpvnetMenuItem.Enabled = isPrSrcF
            mpvnetMenuItem.KeyDisplayString = "F9"

            Dim mpcMenuItem = Menu.Add2("Play with mpc", AddressOf Editor.PlayScriptWithMPC, mImageD(Symbol.Play), "Plays the current script with MPC.")
            mpcMenuItem.Enabled = isPrSrcF
            mpcMenuItem.KeyDisplayString = "F10"

            Menu.Add2("Preview Code...", AddressOf CodePreview, mImageD(Symbol.Code), "Previews the script with solved macros.")

            Dim infoMenuItem = Menu.Add2("Info...", AddressOf Editor.ShowInfo, mImageD(Symbol.Info), "Previews script parameters such as framecount and colorspace.")
            infoMenuItem.KeyDisplayString = "Ctrl+I"
            infoMenuItem.Enabled = isPrSrcF

            Menu.Add2("Advanced Info...", AddressOf Editor.ShowAdvancedInfo, mImageD(Symbol.Lightbulb)).Enabled = isPrSrcF

            Dim joinMenuItem = Menu.Add2("Join Filters", AddressOf Editor.JoinFilters, "Joins all filters into one filter.")
            joinMenuItem.Enabled = DirectCast(Parent, FlowLayoutPanel).Controls.Count > 1
            joinMenuItem.ShortcutKeyDisplayString = "Ctrl+J   "

            Dim profilesMenuItem = Menu.Add2("Profiles...", AddressOf g.MainForm.ShowFilterProfilesDialog, mImageD(Symbol.FavoriteStar), "Dialog to edit profiles.")
            profilesMenuItem.KeyDisplayString = "Ctrl+P"

            Dim macrosMenuItem = Menu.Add2("Macros...", AddressOf MacrosForm.ShowDialogForm, mImageD(Symbol.CalculatorPercentage), "Dialog to choose macros.")
            macrosMenuItem.KeyDisplayString = "Ctrl+M"

            Menu.Items.Add(New ToolStripSeparator)

            Dim moveUpMenuItem = Menu.Add2("Move Up", AddressOf MoveUp, mImageD(Symbol.Up))
            moveUpMenuItem.KeyDisplayString = "Ctrl+Up"

            Dim moveDownMenuItem = Menu.Add2("Move Down", AddressOf MoveDown, mImageD(Symbol.Down))
            moveDownMenuItem.KeyDisplayString = "Ctrl+Down"

            Menu.Items.Add(New ToolStripSeparator)

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
            Dim cutMenuItem = Menu.Add2("Cut", cutAction, mImageD(Symbol.Cut))
            cutMenuItem.Enabled = isTSel AndAlso Not rtbScript.ReadOnly
            cutMenuItem.KeyDisplayString = "Ctrl+X"

            Dim copyMenuItem = Menu.Add2("Copy", copyAction, mImageD(Symbol.Copy))
            copyMenuItem.Enabled = isTSel
            copyMenuItem.KeyDisplayString = "Ctrl+C"

            Dim pasteMenuItem = Menu.Add2("Paste", pasteAction, mImageD(Symbol.Paste)) ' Errors !! ToDO Check need
            pasteMenuItem.Enabled = Clipboard.GetText IsNot "" AndAlso Not rtbScript.ReadOnly
            pasteMenuItem.KeyDisplayString = "Ctrl+V"

            Menu.Items.Add(New ToolStripSeparator)
            Dim helpMenuItem = Menu.Add2("Help ", mImageD(Symbol.Help))
            Dim helpTempMenuItem = New ToolStripMenuItem("temp")
            helpMenuItem.DropDownItems.Add(helpTempMenuItem)

            TTTT = ActionMenuItem.LayoutSuspendList?.Count & "LayLC| "
            ActionMenuItem.LayoutResume(False)
            Menu.ResumeLayout()

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
            TTTT &= SsWw.ElapsedTicks / SWFreq & "msMain|ImgC:" & ImageHelp.ImageCacheD.Count
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
                        Dim filterTable = Editor.CreateFilterTable(filter)
                        flow.SuspendLayout()
                        flow.Controls.Add(filterTable)
                        flow.Controls.SetChildIndex(filterTable, index)
                        flow.ResumeLayout()

                        Editor.MainFlowLayoutPanelLayout() 'Test this!!!

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
                        Dim filterTable = Editor.CreateFilterTable(filter)
                        flow.Controls.Add(filterTable)

                        Editor.MainFlowLayoutPanelLayout() 'Test this!!!

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
