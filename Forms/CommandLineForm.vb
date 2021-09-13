
Imports System.ComponentModel
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports JM.LinqFaster
Imports StaxRip.CommandLine
Imports StaxRip.UI

Public Class CommandLineForm
    Private Const RTBFormatFlags As TextFormatFlags = TextFormatFlags.WordBreak Or TextFormatFlags.TextBoxControl Or TextFormatFlags.NoPrefix Or TextFormatFlags.NoPadding
    Private ReadOnly Params As CommandLineParams
    Private SearchIndex As Integer
    Private Items As List(Of Item)
    Private HighlightedControl As Control
    Private ComboBoxUpdated As Boolean
    Private IsHandleCr As Boolean
    Private RTBCmdFont As Font
    Private ReadOnly RTBWidth As Integer
    Private RTBLastH As Integer
    Private RTBLastTxt As String
    Private ScrollOn As Boolean
    Private ReadOnly SLock As New Object
    Private NewRun As Integer

    Property HTMLHelp As String

    Event BeforeHelp()

    Private Ssssw As Stopwatch = Stopwatch.StartNew 'debug
    Private Tttt As New StringBuilder(288) 'debug

    Sub New(params As CommandLineParams)

        'WarmUpCpu() 'DEBUG
        Ssssw.Restart()

        Dim ckT = Task.Run(Function()
                               Dim parItm As List(Of CommandLineParam) = params.Items
                               Dim pItC As Integer = parItm.Count
                               Me.Items = New List(Of Item)(pItC)
                               Dim singleList As New HashSet(Of String)(pItC, StringComparer.Ordinal)
                               For i = 0 To parItm.Count - 1
                                   Dim param = parItm(i)
                                   Dim gk As String = param.GetKey
                                   If gk Is Nothing OrElse gk.Length = 0 OrElse Not singleList.Add(gk) Then
                                       Throw New Exception(pItC & "/" & singleList.Count & "/key found twice: " & gk)
                                   End If
                               Next i
                               Return pItC
                           End Function)
        InitializeComponent()

        Me.tlpMain.SuspendLayout()
        Me.PanelBn.SuspendLayout()
        Me.SuspendLayout()
        Dim sfh = If(s.UIScaleFactor = 1, 16, New Font("Segoe UI", 9 * s.UIScaleFactor).Height)
        Me.SimpleUI.Size = New Size(sfh * 50, sfh * 30) '=800x480[fh16]Was: (44, 32) Org:(40, 26) 'ToDo: make it 52,32???
        RTBWidth = sfh * 50 'FW=828,RtbW=800
        RTBCmdFont = rtbCommandLine.Font 'New Font("Consolas", 10 * s.UIScaleFactor)

        Ssssw.Stop()
        MeTextOld = Text 'Debug
        Tttt.Append(ckT.IsCompleted).Append(Ssssw.ElapsedTicks / SWFreq).Append("msNew1| ")
        Ssssw.Restart()

        Dim rtbT = Task.Run(Sub()
                                Dim rtbTxt As String = params.GetCommandLine(False, False)
                                If rtbTxt?.Length > 0 Then
                                    RTBLastTxt = rtbTxt
                                    RTBLastH = TextRenderer.MeasureText(rtbTxt, RTBCmdFont, New Size(RTBWidth + 44, 100000), RTBFormatFlags).Height + 2
                                End If
                            End Sub)
        Me.Params = params
        InitUI()
        params.RaiseValueChanged(Nothing) 'Debug 'Not Thread Safe ???
        SelectLastPage()
        Try
            Dim pIc As Integer = ckT.Result
            Me.Text = params.Title & " (" & pIc.ToInvStr & " options)"
        Catch ex As AggregateException
            Throw New Exception(ex.InnerException.ToString, ex.InnerException)
        End Try

        Ssssw.Stop()
        Tttt.Append(Ssssw.ElapsedTicks / SWFreq).Append("msIUIRaiseValCHSelPg| RtbT:")
        Dim _unusedT = rtbT.ContinueWith(Sub() If rtbT.Exception IsNot Nothing Then g.ShowException(rtbT.Exception, "RTBTask error:")) 'Debug
        Ssssw.Restart()

        Me.cbGoTo.Sorted = False 'true 
        Me.cbGoTo.Select()
        cms.SuspendLayout()
        cms.Items.AddRange(
          {New ActionMenuItem("Execute Command Line", Sub() params.Execute(), ImageHelp.GetImageC(Symbol.fa_terminal)) With {.Enabled = p.SourceFile.NotNullOrEmptyS},
          New ActionMenuItem("Copy Command Line", Sub()
                                                      Clipboard.SetText(params.GetCommandLine(True, True))
                                                      MsgInfo("Command Line was copied.")
                                                  End Sub, ImageHelp.GetImageC(Symbol.Copy)),
          New ActionMenuItem("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Line", params.GetCommandLine(True, True))),
          New ActionMenuItem("Import Command Line...", Sub() If MsgQuestion("Import command line from clipboard?", Clipboard.GetText) = DialogResult.OK Then BasicVideoEncoder.ImportCommandLine(Clipboard.GetText, params), ImageHelp.GetImageC(Symbol.Download)),
          New ActionMenuItem("Help about this dialog", AddressOf ShowHelp, ImageHelp.GetImageC(Symbol.Help)),
          New ActionMenuItem("Help about " & params.GetPackage.Name, Sub() params.GetPackage.ShowHelp(), ImageHelp.GetImageC(Symbol.Help))})
        cms.ResumeLayout(False)

        'rtbT.Wait()

        If RTBLastTxt?.Length > 0 Then
            rtbCommandLine.Text = RTBLastTxt
        Else
            Console.Beep(7000, 900)
            RTBLastTxt = ""
        End If

        Ssssw.Stop()
        Tttt.Append(rtbT.IsCompleted).Append(Ssssw.ElapsedTicks / SWFreq).Append("msEndNew| ")
        If Not rtbT.IsCompleted Then Console.Beep(2000, 900) 'debug
        Ssssw.Restart()
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        Ssssw.Stop()
        Tttt.Append(RTBLastH > 15).Append(Ssssw.ElapsedTicks / SWFreq).Append("msOnload1| ")
        Ssssw.Restart()

        MyBase.OnLoad(args)

        Ssssw.Stop()
        Tttt.Append(Ssssw.ElapsedTicks / SWFreq).Append("msOnLoadMyBase2| ")
        Ssssw.Restart()

        If RTBLastH = rtbCommandLine.Height Then RTBLastH += 1
        If RTBLastH > 7 Then '16-RTB fh
            If RTBLastH > ScreenResWAPrim.Height * 0.4 Then
                ScrollOn = True
                rtbCommandLine.ScrollBars = RichTextBoxScrollBars.Vertical
                RTBLastH = CInt(ScreenResWAPrim.Height * 0.4)
            End If
            rtbCommandLine.Height = RTBLastH
        Else
            rtbCommandLine.Height += 1
            RTBLastH = rtbCommandLine.Height
        End If

        Me.PanelBn.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout()

        Ssssw.Stop()
        Tttt.Append(Ssssw.ElapsedTicks / SWFreq).Append("msOnLoadResumeLay3| ")
        Ssssw.Restart()

        Dim loadT = Task.Run(Sub()
                                 IsHandleCr = True
                                 Thread.Sleep(75) '61-80
                                 'Application.DoEvents()
                                 If IsHandleCr Then BeginInvoke(Sub()
                                                                    Ssssw.Stop()
                                                                    Tttt.Append(RTBLastH > 15).Append(Ssssw.ElapsedTicks / SWFreq).Append("msOnloadBeginInvoke1| ")
                                                                    Ssssw.Restart()
                                                                    'rtbCommandLine.Refresh()
                                                                    'Refresh()
                                                                    Application.DoEvents()
                                                                    If Not IsHandleCr Then Exit Sub
                                                                    cbGoTo.SendMessageCue("Search") 'Must be in GUI thread, if isHandle seems faster ???
                                                                    UpdateSearchComboBox()
                                                                    AddHandler cbGoTo.Enter, AddressOf cbGoTo_DropDown
                                                                    AddHandler Params.ValueChanged, AddressOf ValueChanged
                                                                    Ssssw.Stop()
                                                                    Tttt.Append(Ssssw.ElapsedTicks / SWFreq).Append("msOnloadBeginInvoke2SendCueUpdSerCombo| ")
                                                                    ' MsgInfo("cmdNewTimes:", ttt.ToString, 400)
                                                                    Log.Write("cmdNewTimes:", Tttt.ToString)
                                                                    Tttt.Clear()
                                                                    Tttt = Nothing
                                                                    Ssssw = Nothing
                                                                    Runtime.GCSettings.LargeObjectHeapCompactionMode = Runtime.GCLargeObjectHeapCompactionMode.CompactOnce 'Debug
                                                                    GC.Collect(2, GCCollectionMode.Forced, True, True)
                                                                    Runtime.GCSettings.LargeObjectHeapCompactionMode = Runtime.GCLargeObjectHeapCompactionMode.CompactOnce
                                                                End Sub)
                             End Sub)
        Dim _unusedT = loadT.ContinueWith(Sub() If loadT.Exception IsNot Nothing Then g.ShowException(loadT.Exception, "LoadT error:")) 'Debug
    End Sub

    Private MeTextOld As String 'Debug
    Protected Overrides Sub OnClosing(e As CancelEventArgs) 'debug
        IsHandleCr = False
        Text = MeTextOld
        MyBase.OnClosing(e)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        IsHandleCr = False
        RTBCmdFont = Nothing
        RemoveHandler Params.ValueChanged, AddressOf ValueChanged
        RemoveHandler cbGoTo.Enter, AddressOf cbGoTo_DropDown
        SimpleUI.SaveLast(Params.Title & "page selection")
        g.MainForm.PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
        MyBase.OnFormClosed(e)
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        hevent.Handled = True
        ShowHelp()
        MyBase.OnHelpRequested(hevent)
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyData = (Keys.Control Or Keys.F) Then
            cbGoTo.Focus()
            e.SuppressKeyPress = True
            If Not ComboBoxUpdated Then UpdateSearchComboBox()
        End If
        MyBase.OnKeyDown(e)
    End Sub

    Sub SelectLastPage()
        SimpleUI.SelectLast(Params.Title & "page selection")
    End Sub

    Sub ValueChanged(item As CommandLineParam)
        ComboBoxUpdated = False
        If Not IsHandleCr Then Exit Sub
        Interlocked.Increment(NewRun)
        Dim vcT = Task.Run(Sub()
                               SyncLock SLock
                                   Dim Ssw11 As New Stopwatch 'debug
                                   Ssw11.Restart()

                                   Interlocked.Decrement(NewRun)
                                   If NewRun > 0 OrElse Not IsHandleCr Then Exit Sub
                                   Dim rtbTxt = Params.GetCommandLine(False, False)
                                   If NewRun > 0 Then Exit Sub
                                   Dim rtbHeightT As Task(Of Integer)
                                   Dim selsT As Task(Of Integer())

                                   If rtbTxt?.Length > 0 Then
                                       Dim lt = RTBLastTxt
                                       If rtbTxt.Length <> lt.Length Then
                                           rtbHeightT = Task.Run(Function() TextRenderer.MeasureText(rtbTxt, RTBCmdFont, New Size(RTBWidth + 44, 100000), RTBFormatFlags).Height + 2)
                                           selsT = Task.Run(Function() rtbCommandLine.GetSelections(rtbTxt.ToCharArray, lt.ToCharArray))
                                           RTBLastTxt = rtbTxt
                                       ElseIf Not String.Equals(lt, rtbTxt) Then
                                           selsT = Task.Run(Function() rtbCommandLine.GetSelections(rtbTxt.ToCharArray, lt.ToCharArray))
                                           RTBLastTxt = rtbTxt
                                       End If
                                   End If

                                   'Dim my_sh As Integer = Math.Max(CInt(Math.Floor(rtbTxt.Result.Length * 10 / RTBWidth)) * 17 + 2, 19)  'EM=10 lh=18=16+2; 18= 16 * 1.115'Testings ToDo!! 'RemoveIt!
                                   If NewRun > 0 Then Exit Sub
                                   rtbHeightT?.Wait() 'Or Not Wait Here to UI be more up to date ???

                                   Ssw11.Stop()
                                   Dim Tttt1 As String = rtbTxt.Length & selsT?.IsCompleted.ToString & Ssw11.ElapsedTicks / SWFreq
                                   'Console.Beep(7000, 14) 'Debug

                                   If NewRun <= 0 AndAlso IsHandleCr Then
                                       BeginInvoke(Sub()
                                                       If Not IsHandleCr Then Exit Sub
                                                       Dim ssw2 As New Stopwatch
                                                       ssw2.Restart()

                                                       If rtbTxt?.Length > 0 Then
                                                           rtbCommandLine.SetText(rtbTxt, selsT)

                                                           If rtbHeightT IsNot Nothing Then
                                                               Dim rtbH = rtbHeightT.Result
                                                               Dim diff As Integer = rtbH - RTBLastH

                                                               If diff > 8 OrElse diff < -28 Then 'diff > Font.height /2,*1.6

                                                                   If rtbH <= ScreenResWAPrim.Height * 0.4 Then
                                                                       rtbCommandLine.Height = rtbH 'ToDO ADD Scroll ON/OFF ???
                                                                       'Height += rtbH - RTBLastH '???
                                                                       If ScrollOn Then
                                                                           ScrollOn = False
                                                                           rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None
                                                                       End If
                                                                   Else
                                                                       If Not ScrollOn Then
                                                                           ScrollOn = True
                                                                           rtbCommandLine.Height = CInt(ScreenResWAPrim.Height * 0.4)
                                                                           rtbCommandLine.ScrollBars = RichTextBoxScrollBars.Vertical 'WordWrap to HorizontalScroll Enable!
                                                                       End If
                                                                   End If
                                                                   RTBLastH = rtbH
                                                               End If
                                                           End If
                                                       Else
                                                           rtbCommandLine.Clear()
                                                           rtbCommandLine.Height = 19 'fh+3
                                                           RTBLastH = 19
                                                           Interlocked.Exchange(RTBLastTxt, "")
                                                       End If

                                                       ssw2.Stop()
                                                       Tttt1 &= $"msTSMHTs|{ssw2.ElapsedTicks / SWFreq}msH:{rtbHeightT?.Result}" '/{my_sh}|{RTBCmdFont.Size}"
                                                       Me.Text = Tttt1
                                                   End Sub)
                                   End If
                               End SyncLock
                           End Sub)
        Dim _unusedT = vcT.ContinueWith(Sub() If vcT.Exception IsNot Nothing Then g.ShowException(vcT.Exception)) 'Debug
        'UpdateSearchComboBox()
    End Sub

    Sub InitUI()
        'Dim flowPanels As New HashSet(Of Control)(17)
        Dim helpControl As Control
        Dim currentFlow As SimpleUI.FlowPage
        Dim parItm As List(Of CommandLineParam) = Params.Items

        For x = 0 To parItm.Count - 1
            Dim param = parItm(x)
            Dim parent As FlowLayoutPanelEx = SimpleUI.GetFlowPage(param.Path)
            currentFlow = DirectCast(parent, SimpleUI.FlowPage)

            'If flowPanels.Add(parent) Then 'ToDO!!!:  AutoSuspend in SimpleUI Test it
            '    parent.SuspendLayout()
            'End If

            Dim help As String = Nothing

            If param.Switch?.Length > 0 Then
                help &= param.Switch & BR
            End If

            If param.HelpSwitch?.Length > 0 Then
                help &= param.HelpSwitch & BR
            End If

            If param.NoSwitch?.Length > 0 Then
                help &= param.NoSwitch & BR
            End If

            Dim switches = param.Switches 'ToDo Make switches array!!!!!!

            If Not switches.NothingOrEmpty Then
                help &= String.Join(BR, switches) & BR
            End If

            help &= BR

            If TypeOf param Is NumParam Then
                Dim nParam = DirectCast(param, NumParam)

                If nParam.Config(0) > Double.MinValue Then
                    help &= "Minimum: " & nParam.Config(0) & BR
                End If

                If nParam.Config(1) < Double.MaxValue Then
                    help &= "Maximum: " & nParam.Config(1) & BR
                End If
            End If

            help &= BR

            If Not param.URLs.NothingOrEmpty Then
                help &= String.Join(BR, param.URLs.SelectF(Function(val) "[" & val & " " & val & "]"))
            End If

            If param.Help?.Length > 0 Then
                help &= param.Help
            End If

            If help?.Length > 0 Then
                help = help.Replace(BR2 & BR, BR2)

                If help.EndsWith(BR, StringComparison.Ordinal) Then
                    help = help.Trim
                End If
            End If

            If param.Label?.Length > 0 Then
                SimpleUI.AddLabel(parent, param.Label).MarginTop = 8 'FontHeight \ 2
            End If

            If TypeOf param Is BoolParam Then
                Dim checkBox = SimpleUI.AddBool(parent)
                checkBox.Text = param.Text

                If param.HelpSwitch?.Length > 0 Then
                    Dim helpID = param.HelpSwitch
                    checkBox.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    checkBox.Help = help
                End If

                checkBox.MarginLeft = param.LeftMargin
                DirectCast(param, BoolParam).InitParam(checkBox)
                helpControl = checkBox
            ElseIf TypeOf param Is NumParam Then
                Dim tempNumParam = DirectCast(param, NumParam)
                Dim nParam = DirectCast(param, NumParam)
                Dim numBlock = SimpleUI.AddNum(parent)
                numBlock.Label.Text = If(param.Text.EndsWith(":", StringComparison.Ordinal), param.Text, param.Text & ":")

                If param.HelpSwitch?.Length > 0 Then
                    Dim helpID = param.HelpSwitch
                    numBlock.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    numBlock.Label.Help = help
                End If

                numBlock.NumEdit.Config = nParam.Config
                AddHandler numBlock.Label.MouseDoubleClick, Sub() tempNumParam.Value = tempNumParam.DefaultValue
                DirectCast(param, NumParam).InitParam(numBlock.NumEdit)
                helpControl = numBlock.Label
            ElseIf TypeOf param Is OptionParam Then
                Dim tempOptionParam = DirectCast(param, OptionParam)
                Dim oParam = DirectCast(param, OptionParam)
                Dim menuBlock = SimpleUI.AddMenu(Of Integer)(parent)
                menuBlock.Label.Text = If(param.Text.EndsWith(":", StringComparison.Ordinal), param.Text, param.Text & ":")

                Dim menuBlBn As SimpleUI.SimpleUIMenuButton(Of Integer) = menuBlock.Button
                If param.HelpSwitch?.Length > 0 Then
                    Dim helpID = param.HelpSwitch
                    menuBlock.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                    menuBlBn.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    menuBlock.Help = help
                End If

                helpControl = menuBlock.Label
                AddHandler menuBlock.Label.MouseDoubleClick, Sub() tempOptionParam.ValueChangedUser(tempOptionParam.DefaultValue)

                If oParam.Expand Then
                    menuBlBn.Expand = True
                End If

                Dim oPo As String() = oParam.Options
                Dim amiAr(oPo.Length - 1) As ActionMenuItem  'Test This !!! Experimental !!!!!!!!!!!
                menuBlBn.Menu.SuspendLayout()
                For x2 = 0 To oPo.Length - 1
                    'menuBlBn.Add(oPo(x2), x2) ' Main Slowdown, add ButtMenu & ActionMenu
                    Dim o As Object = x2
                    Dim t = oPo(x2)
                    ' menuBlBn.Items.Add(o) ' Needed???
                    amiAr(x2) = New ActionMenuItem(t, Sub() menuBlBn.OnAction(t, o), o)
                Next x2
                menuBlBn.Menu.Items.AddRange(amiAr)
                menuBlBn.Menu.ResumeLayout(False)

                oParam.InitParam(menuBlBn)
            ElseIf TypeOf param Is StringParam Then
                Dim tempItem = DirectCast(param, StringParam)
                Dim textBlock As SimpleUI.TextBlock

                If tempItem.BrowseFileFilter?.Length > 0 Then
                    Dim textButtonBlock = SimpleUI.AddTextButton(parent)
                    textButtonBlock.BrowseFile(tempItem.BrowseFileFilter)
                    textBlock = textButtonBlock
                ElseIf tempItem.Menu?.Length > 0 Then
                    Dim textMenuBlock = SimpleUI.AddTextMenu(parent)
                    textMenuBlock.AddMenu(tempItem.Menu)
                    textBlock = textMenuBlock
                Else
                    textBlock = SimpleUI.AddText(parent)
                End If

                textBlock.Label.Text = If(param.Text.EndsWith(":", StringComparison.Ordinal), param.Text, param.Text & ":")

                If param.HelpSwitch?.Length > 0 Then
                    Dim helpID = param.HelpSwitch
                    textBlock.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    textBlock.Label.Help = help
                End If

                helpControl = textBlock.Label
                AddHandler textBlock.Label.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                textBlock.Edit.Expand = tempItem.Expand
                tempItem.InitParam(textBlock)
            End If

            If helpControl IsNot Nothing Then
                Dim item As New Item With {.Control = helpControl, .Page = currentFlow, .Param = param}
                Items.Add(item)
            End If
        Next x
        'For Each panel In flowPanels
        '    panel.ResumeLayout()
        'Next
    End Sub

    Public Class Item
        Property Page As SimpleUI.FlowPage
        Property Control As Control
        Property Param As CommandLineParam
    End Class

    Sub ShowHelp()
        RaiseEvent BeforeHelp()

        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)

        form.Doc.WriteH2("How to use the video encoder dialog")

        If cbGoTo.Visible Then
            form.Doc.WriteParagraph("The Search dropdown field at the dialog bottom left lists options and can be used to quickly find options, it searches command line switches, labels and dropdowns. Multiple matches can be cycled by pressing enter.")
        End If

        form.Doc.WriteParagraph("Numeric values and dropdown menu options can be reset to their default value by double clicking on the label.")
        form.Doc.WriteParagraph("The context help is shown with a right-click on a label, dropdown menu or checkbox.")
        form.Doc.WriteParagraph("The command line preview at the bottom of the dialog has a context menu that allows to quickly find and show options.")

        If HTMLHelp?.Length > 0 Then
            form.Doc.Writer.WriteRaw(HTMLHelp)
        End If

        form.Doc.WriteTips(SimpleUI.ActivePage.TipProvider.GetTips)
        form.Show()
    End Sub

    Sub cbGoTo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbGoTo.KeyDown
        If e.KeyData = Keys.Enter Then
            SearchIndex += 1
            cbGoTo_TextChanged(Nothing, Nothing)
        Else
            SearchIndex = 0
        End If

        If e.KeyData = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Sub cbGoTo_TextChanged(sender As Object, e As EventArgs) Handles cbGoTo.TextChanged

        If Not ComboBoxUpdated Then
            UpdateSearchComboBox()
        End If

        If HighlightedControl IsNot Nothing Then
            HighlightedControl.Font = New Font(HighlightedControl.Font, FontStyle.Regular)
            HighlightedControl = Nothing
        End If

        Dim cbGText As String = cbGoTo.Text
        Dim find = cbGText.ToLower(InvCult)

        If find.Length > 1 Then
            Dim findNoSpace = find.Replace(" ", "")
            Dim matchedItems As New HashSet(Of Item)(If(Items.Count > 50 AndAlso find.Length < 4, 47, 17))

            For Each item In Items
                If item.Param.Visible Then
                    If String.Equals(item.Param.Switch, cbGText) OrElse String.Equals(item.Param.NoSwitch, cbGText) OrElse String.Equals(item.Param.HelpSwitch, cbGText) Then
                        matchedItems.Add(item)
                    End If

                    If item.Param.Switches IsNot Nothing Then
                        For Each switch In item.Param.Switches
                            If String.Equals(switch, cbGText) Then
                                matchedItems.Add(item)
                            End If
                        Next
                    End If
                End If
            Next item

            For Each item In Items
                If item.Param.Visible Then
                    If item.Param.Switch.ToLowerEx.Contains(find) OrElse item.Param.NoSwitch.ToLowerEx.Contains(find) OrElse
                        item.Param.HelpSwitch.ToLowerEx.Contains(find) OrElse item.Param.Help.ToLowerEx.Contains(find) OrElse item.Param.Text.ToLowerEx.Contains(find) Then

                        matchedItems.Add(item)
                    End If

                    If item.Param.Switches IsNot Nothing Then
                        For Each switch In item.Param.Switches
                            If switch.ToLower(InvCult).Contains(find) Then
                                matchedItems.Add(item)
                            End If
                        Next
                    End If

                    If TypeOf item.Param Is OptionParam Then
                        Dim param = DirectCast(item.Param, OptionParam)

                        If param.Options IsNot Nothing Then
                            For Each value In param.Options
                                Dim valToL As String = value.ToLower(InvCult)
                                If valToL.Contains(find) Then
                                    matchedItems.Add(item)
                                End If

                                Dim valNoSpaceToLow As String = value.Replace(" ", "").ToLower(InvCult)
                                If valNoSpaceToLow.Contains(findNoSpace) Then
                                    matchedItems.Add(item)
                                End If

                                If valToL.Contains(findNoSpace) Then
                                    matchedItems.Add(item)
                                End If

                                If valNoSpaceToLow.Contains(find) Then
                                    matchedItems.Add(item)
                                End If
                            Next
                        End If

                        If param.Values IsNot Nothing Then
                            For Each value In param.Values
                                Dim valToLow As String = value.ToLower(InvCult)
                                If valToLow.Contains(find) Then
                                    matchedItems.Add(item)
                                End If

                                Dim valNoSpaceToLow As String = value.Replace(" ", "").ToLower(InvCult)
                                If valNoSpaceToLow.Contains(findNoSpace) Then
                                    matchedItems.Add(item)
                                End If

                                If valToLow.Contains(findNoSpace) Then
                                    matchedItems.Add(item)
                                End If

                                If valNoSpaceToLow.Contains(find) Then
                                    matchedItems.Add(item)
                                End If
                            Next
                        End If
                    End If
                End If
            Next item

            If matchedItems.Count > 0 Then
                If SearchIndex >= matchedItems.Count Then SearchIndex = 0
                'Dim matchI As Item = matchedItems(SearchIndex) '=ElementAtOrDefault
                Dim matchI As Item = matchedItems.ElementAt(SearchIndex)
                Dim control = matchI.Control
                SimpleUI.ShowPage(matchI.Page)
                control.Font = New Font(control.Font, FontStyle.Bold)
                HighlightedControl = control
                Exit Sub
            End If
        End If
    End Sub

    Sub cbGoTo_DropDown(sender As Object, e As EventArgs) Handles cbGoTo.Click, cbGoTo.DropDown ' AddHandler cbGoTo.Enter
        If Not ComboBoxUpdated Then
            UpdateSearchComboBox()
        End If
    End Sub

    Sub UpdateSearchComboBox()
        ComboBoxUpdated = True
        cbGoTo.Items.Clear()
        Static hsCount As Integer
        Dim sItems As New HashSet(Of String)(If(hsCount > 2, hsCount, Items.Count), StringComparer.Ordinal)
        For n = 0 To Items.Count - 1
            Dim iParam As CommandLineParam = Items(n).Param

            If iParam.Visible Then
                Dim pSwitches As IEnumerable(Of String) = iParam.Switches
                If pSwitches IsNot Nothing Then
                    For Each sw In pSwitches
                        sItems.Add(sw)
                    Next sw
                End If

                Dim swt As String = iParam.Switch
                If swt?.Length > 0 Then
                    sItems.Add(swt)
                End If

                Dim nSw As String = iParam.NoSwitch
                If nSw?.Length > 0 Then
                    sItems.Add(nSw)
                End If

                Dim hSw As String = iParam.HelpSwitch
                If hSw?.Length > 0 Then
                    sItems.Add(hSw)
                End If
            End If
        Next n

        hsCount = sItems.Count
        Dim ia(hsCount - 1) As String
        sItems.CopyTo(ia)
        Array.Sort(ia, StringComparer.Ordinal)

        cbGoTo.Items.AddRange(ia)
        cbGoTo.SelectionStart = cbGoTo.Text.Length
    End Sub

    Sub rtbCommandLine_MouseUp(sender As Object, e As MouseEventArgs) Handles rtbCommandLine.MouseUp
        If e.Button = MouseButtons.Right Then
            cmsCommandLine.SuspendLayout()
            cmsCommandLine.Items.ClearAndDispose()

            Dim cmsIL As New List(Of ActionMenuItem)(3) From {
                New ActionMenuItem("Copy Selection", Sub() Clipboard.SetText(rtbCommandLine.SelectedText)) With {.KeyDisplayString = "Ctrl+C", .Visible = rtbCommandLine.SelectionLength > 0},
                New ActionMenuItem("Copy Command Line", Sub() Clipboard.SetText(Params.GetCommandLine(True, True)))}

            Dim find = rtbCommandLine.SelectedText

            If find.Length = 0 Then
                Dim pos = rtbCommandLine.SelectionStart
                Dim left = rtbCommandLine.Text.Substring(0, pos).LastIndexOf(" ", StringComparison.Ordinal) + 1
                Dim right = rtbCommandLine.Text.Length
                Dim index = rtbCommandLine.Text.Remove(0, pos).IndexOf(" ", StringComparison.Ordinal)

                If index > -1 Then
                    right = pos + index
                End If

                If right - left > 0 Then
                    find = rtbCommandLine.Text.Substring(left, right - left)
                End If
            End If

            If find.Length > 0 Then
                If find.Contains("=") Then
                    find = find.Left("=")
                End If

                cmsIL.Add(New ActionMenuItem("Search " & find, Sub()
                                                                   cbGoTo.Text = find
                                                                   cbGoTo.Focus()
                                                               End Sub))
            End If

            cmsCommandLine.Items.AddRange(cmsIL.ToArray)
            cmsCommandLine.ResumeLayout()
            cmsCommandLine.Show(rtbCommandLine, e.Location)
        End If
    End Sub

    Sub rtbCommandLine_MouseDown(sender As Object, e As MouseEventArgs) Handles rtbCommandLine.MouseDown
        If e.Button = MouseButtons.Right AndAlso rtbCommandLine.SelectedText.NullOrEmptyS Then
            rtbCommandLine.SelectionStart = rtbCommandLine.GetCharIndexFromPosition(e.Location)
        End If
    End Sub

End Class
