
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports JM.LinqFaster
Imports StaxRip.CommandLine
Imports StaxRip.UI

Public Class CommandLineForm
    Private Params As CommandLineParams
    Private SearchIndex As Integer
    Private Items As List(Of Item)
    Private HighlightedControl As Control
    Private ComboBoxUpdated As Boolean

    Property HTMLHelp As String

    Event BeforeHelp()

    Sub New(params As CommandLineParams)
        Dim pItC As Integer = params.Items.Count
        Dim ckT = Task.Run(Sub()
                               Items = New List(Of Item)(pItC)
                               Dim singleList As New HashSet(Of String)(pItC, StringComparer.Ordinal)
                               For Each param In params.Items
                                   Dim gk As String = param.GetKey
                                   If gk.NullOrEmptyS OrElse Not singleList.Add(gk) Then
                                       Throw New Exception(pItC & "/" & singleList.Count & "/key found twice: " & gk)
                                   End If
                               Next
                           End Sub)
        InitializeComponent()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        SimpleUI.ScaleClientSize(48, 34, FontHeight) 'Was: (44, 34)
        Me.Params = params
        Text = params.Title & " (" & pItC.ToInvariantString & " options)"

        MeTextOld = Text 'Debug

        'rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None
        'rtbCommandLine.ContextMenuStrip = cmsCommandLine'inDesigner 
        'rtbCommandLine.ContextMenuStrip?.Dispose() '- NoInitMenu

        cbGoTo.Sorted = False 'true 
        cbGoTo.SendMessageCue("Search")
        cbGoTo.Select()
        AddHandler cbGoTo.Enter, AddressOf cbGoTo_DropDown

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
          New ActionMenuItem("Help about " + params.GetPackage.Name, Sub() params.GetPackage.ShowHelp(), ImageHelp.GetImageC(Symbol.Help))})
        cms.ResumeLayout(False)

        Try
            ckT.Wait()
        Catch ex As AggregateException
            Throw New Exception(ex.InnerException.ToString, ex.InnerException)
        End Try

        InitUI()
        SelectLastPage()
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)
        Task.Run(Sub()
                     IsHandleCr = True
                     Threading.Thread.Sleep(60)
                     If IsHandleCr Then
                         RTBFont = rtbCommandLine.Font
                         AddHandler Params.ValueChanged, AddressOf ValueChanged
                         Params.RaiseValueChanged(Nothing)
                         BeginInvoke(Sub() UpdateSearchComboBox())
                     End If
                 End Sub)


        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Protected Overrides Sub OnClosing(e As CancelEventArgs) 'debug
        IsHandleCr = False
        Text = MeTextOld
        MyBase.OnClosing(e)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        SimpleUI.SaveLast(Params.Title + "page selection")
        RemoveHandler Params.ValueChanged, AddressOf ValueChanged
        RemoveHandler cbGoTo.Enter, AddressOf cbGoTo_DropDown
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
        SimpleUI.SelectLast(Params.Title + "page selection")
    End Sub

    Private MeTextOld As String 'Debug
    Private RTBLastH As Integer
    Private IsHandleCr As Boolean
    Private RTBFont As Font

    Sub ValueChanged(item As CommandLineParam)
        'rtbCommandLine.SetText(Params.GetCommandLine(False, False))
        'rtbCommandLine.SelectionLength = 0
        Dim rtbTxtT = Task.Run(Function() Params.GetCommandLine(False, False))
        'Static fnt As New Font("Consolas", 9.75F * s.UIScaleFactor)
        ComboBoxUpdated = False
        If Not RTBFont.Equals(rtbCommandLine.Font) Then Console.Beep(700, 12)
        Dim rtbHeightT = Task.Run(Function() TextRenderer.MeasureText(rtbTxtT.Result, RTBFont, New Size(Width - 14, 100000), TextFormatFlags.WordBreak).Height + 1) 'width - margins
        If IsHandleCr Then
            Task.Run(Sub() If IsHandleCr Then BeginInvoke(
                         Sub()
                             Dim Ssw11 As New Stopwatch 'debug
                             Dim Tttt1 As String
                             Tttt1 = rtbTxtT.IsCompleted.ToString
                             Ssw11.Restart()
                             rtbCommandLine.SetText(rtbTxtT.Result)
                             'rtbCommandLine.UpdateHeightAsync()
                             rtbCommandLine.SelectionLength = 0
                             Ssw11.Stop()
                             Tttt1 &= Ssw11.ElapsedTicks / SWFreq & rtbHeightT.IsCompleted.ToString
                             Ssw11.Restart()

                             Dim mH As Integer = rtbHeightT.Result
                             If mH - RTBLastH > 12 AndAlso mH < ScreenResPrim.Height * 0.4 Then 'diff > Font.height
                                 rtbCommandLine.Height = mH
                             End If
                             RTBLastH = mH

                             Ssw11.Stop()
                             Tttt1 &= $"|{Ssw11.ElapsedTicks / SWFreq}ms|H:{mH}"
                             Me.Text = Tttt1
                         End Sub))
        Else
            rtbCommandLine.SetText(rtbTxtT.Result)
            'rtbCommandLine.UpdateHeightAsync()
            rtbCommandLine.SelectionLength = 0
            rtbCommandLine.Height = rtbHeightT.Result
            RTBLastH = rtbHeightT.Result
        End If
        'UpdateSearchComboBox()
    End Sub

    Sub InitUI()
        'Dim flowPanels As New HashSet(Of Control)(17)
        Dim helpControl As Control
        Dim currentFlow As SimpleUI.FlowPage

        For x = 0 To Params.Items.Count - 1
            Dim param = Params.Items(x)
            Dim parent As FlowLayoutPanelEx = SimpleUI.GetFlowPage(param.Path)
            currentFlow = DirectCast(parent, SimpleUI.FlowPage)

            'If flowPanels.Add(parent) Then 'ToDO!!!:  AutoSuspend in SimpleUI Test it
            '    parent.SuspendLayout()
            'End If

            Dim help As String = Nothing

            If param.Switch.NotNullOrEmptyS Then
                help += param.Switch + BR
            End If

            If param.HelpSwitch.NotNullOrEmptyS Then
                help += param.HelpSwitch + BR
            End If

            If param.NoSwitch.NotNullOrEmptyS Then
                help += param.NoSwitch + BR
            End If

            Dim switches = param.Switches

            If Not switches.NothingOrEmpty Then
                help += switches.Join(BR) + BR
            End If

            help += BR

            If TypeOf param Is NumParam Then
                Dim nParam = DirectCast(param, NumParam)

                If nParam.Config(0) > Double.MinValue Then
                    help += "Minimum: " & nParam.Config(0) & BR
                End If

                If nParam.Config(1) < Double.MaxValue Then
                    help += "Maximum: " & nParam.Config(1) & BR
                End If
            End If

            help += BR

            If Not param.URLs.NothingOrEmpty Then
                help += String.Join(BR, param.URLs.SelectF(Function(val) "[" + val + " " + val + "]"))
            End If

            If param.Help.NotNullOrEmptyS Then
                help += param.Help
            End If

            If help.NotNullOrEmptyS Then
                help = help.Replace(BR2 + BR, BR2)

                If help.EndsWith(BR, StringComparison.Ordinal) Then
                    help = help.Trim
                End If
            End If

            If param.Label.NotNullOrEmptyS Then
                SimpleUI.AddLabel(parent, param.Label).MarginTop = FontHeight \ 2
            End If

            If TypeOf param Is BoolParam Then
                Dim checkBox = SimpleUI.AddBool(parent)
                checkBox.Text = param.Text

                If param.HelpSwitch.NotNullOrEmptyS Then
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

                If param.HelpSwitch.NotNullOrEmptyS Then
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
                menuBlock.Label.Text = If(param.Text.EndsWith(":", StringComparison.Ordinal), param.Text, param.Text + ":")

                Dim menuBlBn As SimpleUI.SimpleUIMenuButton(Of Integer) = menuBlock.Button
                If param.HelpSwitch.NotNullOrEmptyS Then
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

                If tempItem.BrowseFileFilter.NotNullOrEmptyS Then
                    Dim textButtonBlock = SimpleUI.AddTextButton(parent)
                    textButtonBlock.BrowseFile(tempItem.BrowseFileFilter)
                    textBlock = textButtonBlock
                ElseIf tempItem.Menu.NotNullOrEmptyS Then
                    Dim textMenuBlock = SimpleUI.AddTextMenu(parent)
                    textMenuBlock.AddMenu(tempItem.Menu)
                    textBlock = textMenuBlock
                Else
                    textBlock = SimpleUI.AddText(parent)
                End If

                textBlock.Label.Text = If(param.Text.EndsWith(":", StringComparison.Ordinal), param.Text, param.Text + ":")

                If param.HelpSwitch.NotNullOrEmptyS Then
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

        If HTMLHelp.NotNullOrEmptyS Then
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
        Dim find = cbGText.ToLowerInvariant

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
                    If item.Param.Switch.ToLowerEx.Contains(find) OrElse
                    item.Param.NoSwitch.ToLowerEx.Contains(find) OrElse
                    item.Param.HelpSwitch.ToLowerEx.Contains(find) OrElse
                    item.Param.Help.ToLowerEx.Contains(find) OrElse
                    item.Param.Text.ToLowerEx.Contains(find) Then

                        matchedItems.Add(item)
                    End If

                    If item.Param.Switches IsNot Nothing Then
                        For Each switch In item.Param.Switches
                            If switch.ToLowerInvariant.Contains(find) Then
                                matchedItems.Add(item)
                            End If
                        Next
                    End If

                    If TypeOf item.Param Is OptionParam Then
                        Dim param = DirectCast(item.Param, OptionParam)

                        If param.Options IsNot Nothing Then
                            For Each value In param.Options
                                Dim valToL As String = value.ToLowerInvariant
                                If valToL.Contains(find) Then
                                    matchedItems.Add(item)
                                End If

                                Dim valNoSpaceToLow As String = value.Replace(" ", "").ToLowerInvariant
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
                                Dim valToLow As String = value.ToLowerInvariant
                                If valToLow.Contains(find) Then
                                    matchedItems.Add(item)
                                End If

                                Dim valNoSpaceToLow As String = value.Replace(" ", "").ToLowerInvariant
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
        For Each i In Items
            If i.Param.Visible Then
                If Not i.Param.Switches Is Nothing Then
                    For Each switch In i.Param.Switches
                        sItems.Add(switch)
                    Next switch
                End If

                If i.Param.Switch.NotNullOrEmptyS Then
                    sItems.Add(i.Param.Switch)
                End If

                If i.Param.NoSwitch.NotNullOrEmptyS Then
                    sItems.Add(i.Param.NoSwitch)
                End If

                If i.Param.HelpSwitch.NotNullOrEmptyS Then
                    sItems.Add(i.Param.HelpSwitch)
                End If
            End If
        Next i

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
