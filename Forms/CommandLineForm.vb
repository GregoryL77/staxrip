﻿
Imports JM.LinqFaster
Imports StaxRip.CommandLine
Imports StaxRip.UI

Public Class CommandLineForm
    Private Params As CommandLineParams
    Private SearchIndex As Integer
    Private Items As List(Of Item)
    Private HighlightedControl As Control
    Private ComboBoxUpdated As Boolean
    Private WasHandleCreated As Boolean

    Property HTMLHelp As String

    Event BeforeHelp()

    Sub New(params As CommandLineParams)
        InitializeComponent()
        'SimpleUI.ScaleClientSize(44, 38)
        SimpleUI.ScaleClientSize(48, 38)

        rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None
        rtbCommandLine.ContextMenuStrip?.Dispose()
        rtbCommandLine.ContextMenuStrip = cmsCommandLine
        Dim pItC As Integer = params.Items.Count
        Items = New List(Of Item)(pItC)
        Dim singleList As New HashSet(Of String)(pItC, StringComparer.Ordinal)
        ' Dim singleList As New List(Of String)(params.Items.Count)
        For Each param In params.Items
            If param.GetKey.NullOrEmptyS OrElse Not singleList.Add(param.GetKey) Then
                Throw New Exception("key found twice: " + param.GetKey)
            End If
            ' singleList.Add(param.GetKey)
        Next

        Me.Params = params
        Text = params.Title + " (" & pItC & " options)"


        InitUI()
        SelectLastPage()
        AddHandler params.ValueChanged, AddressOf ValueChanged
        params.RaiseValueChanged(Nothing)

        cbGoTo.Sorted = False 'true 
        cbGoTo.SendMessageCue("Search")
        cbGoTo.Select()
        AddHandler cbGoTo.Enter, AddressOf cbGoTo_DropDown

        cms.SuspendLayout()
        cms.Add("Execute Command Line", Sub() params.Execute(), p.SourceFile.NotNullOrEmptyS).SetImage(Symbol.fa_terminal)

        cms.Add("Copy Command Line", Sub()
                                         Clipboard.SetText(params.GetCommandLine(True, True))
                                         MsgInfo("Command Line was copied.")
                                     End Sub).SetImage(Symbol.Copy)

        cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Line", params.GetCommandLine(True, True)))
        cms.Add("Import Command Line...", Sub() If MsgQuestion("Import command line from clipboard?", Clipboard.GetText) = DialogResult.OK Then BasicVideoEncoder.ImportCommandLine(Clipboard.GetText, params)).SetImage(Symbol.Download)

        cms.Add("Help about this dialog", AddressOf ShowHelp).SetImage(Symbol.Help)
        cms.Add("Help about " + params.GetPackage.Name, Sub() params.GetPackage.ShowHelp()).SetImage(Symbol.Help)
        cms.ResumeLayout(False)
    End Sub

    Sub SelectLastPage()
        SimpleUI.SelectLast(Params.Title + "page selection")
    End Sub

    Sub ValueChanged(item As CommandLineParam)
        rtbCommandLine.SetText(Params.GetCommandLine(False, False))
        rtbCommandLine.SelectionLength = 0
        If WasHandleCreated Then
            Threading.Tasks.Task.Run(Sub() BeginInvoke(Sub() rtbCommandLine.UpdateHeight()))
        Else
            rtbCommandLine.UpdateHeight()
        End If
        ComboBoxUpdated = False
        'UpdateSearchComboBox()
    End Sub

    Sub InitUI()
        'Dim flowPanels As New List(Of Control)'Test Opt.??
        Dim flowPanels As New HashSet(Of Control)(17)
        Dim helpControl As Control
        Dim currentFlow As SimpleUI.FlowPage

        For x = 0 To Params.Items.Count - 1
            Dim param = Params.Items(x)
            Dim parent As FlowLayoutPanelEx = SimpleUI.GetFlowPage(param.Path)
            currentFlow = DirectCast(parent, SimpleUI.FlowPage)

            'If Not flowPanels.Contains(parent) Then
            If flowPanels.Add(parent) Then
                'flowPanels.Add(parent)
                parent.SuspendLayout()
            End If

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

                If param.HelpSwitch.NotNullOrEmptyS Then
                    Dim helpID = param.HelpSwitch
                    menuBlock.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                    menuBlock.Button.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    menuBlock.Help = help
                End If

                helpControl = menuBlock.Label
                AddHandler menuBlock.Label.MouseDoubleClick, Sub() tempOptionParam.ValueChangedUser(tempOptionParam.DefaultValue)

                If oParam.Expand Then
                    menuBlock.Button.Expand = True
                End If

                menuBlock.Button.Menu.SuspendLayout()
                For x2 = 0 To oParam.Options.Length - 1 ' Main Slowdown, add ButtMenu & ActionMenu
                    menuBlock.Button.Add(oParam.Options(x2), x2)
                Next
                menuBlock.Button.Menu.ResumeLayout(False)

                oParam.InitParam(menuBlock.Button)
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
                Dim item As New Item
                item.Control = helpControl
                item.Page = currentFlow
                item.Param = param
                Items.Add(item)
            End If
        Next

        For Each panel In flowPanels
            panel.ResumeLayout()
        Next
    End Sub

    Public Class Item
        Property Page As SimpleUI.FlowPage
        Property Control As Control
        Property Param As CommandLineParam
    End Class

    Protected Overrides Sub OnShown(e As EventArgs)
        WasHandleCreated = True
        Threading.Tasks.Task.Run(Sub()
                                     Threading.Thread.Sleep(60)
                                     If WasHandleCreated Then BeginInvoke(Sub() UpdateSearchComboBox())
                                 End Sub)
        MyBase.OnShown(e)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        WasHandleCreated = False
        SimpleUI.SaveLast(Params.Title + "page selection")
        RemoveHandler Params.ValueChanged, AddressOf ValueChanged
        RemoveHandler cbGoTo.Enter, AddressOf cbGoTo_DropDown
        g.MainForm.PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)  'Sub CommandLineForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        MyBase.OnFormClosed(e)
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        ShowHelp()
        hevent.Handled = True
        MyBase.OnHelpRequested(hevent) 'Sub CommandLineForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyData = (Keys.Control Or Keys.F) Then
            cbGoTo.Focus()
            e.SuppressKeyPress = True
            If Not ComboBoxUpdated Then UpdateSearchComboBox()
        End If
        MyBase.OnKeyDown(e)
    End Sub

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
            HighlightedControl.Font = New Font(HighlightedControl.Font.FontFamily, HighlightedControl.Font.Size, FontStyle.Regular)
            HighlightedControl = Nothing
        End If

        Dim cbGText As String = cbGoTo.Text
        Dim find = cbGText.ToLowerInvariant
        Dim findNoSpace = find.Replace(" ", "")
        Dim matchedItems As New HashSet(Of Item)(37)

        If find.Length > 1 Then
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

            Dim visibleItems(matchedItems.Count - 1) As Item 'matchedItems.Where(Function(arg) arg.Param.Visible).ToArray
            matchedItems.CopyTo(visibleItems)

            If visibleItems.Length > 0 Then
                If SearchIndex >= visibleItems.Length Then
                    SearchIndex = 0
                End If

                Dim control = visibleItems(SearchIndex).Control
                SimpleUI.ShowPage(visibleItems(SearchIndex).Page)
                control.Font = New Font(control.Font.FontFamily, control.Font.Size, FontStyle.Bold)
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
        'cbGoTo.BeginUpdate()
        cbGoTo.Items.Clear()
        Dim sItems As New HashSet(Of String)(Items.Count + 7, StringComparer.Ordinal)
        For Each i In Items
            If i.Param.Visible Then
                If Not i.Param.Switches Is Nothing Then
                    For Each switch In i.Param.Switches
                        'If Not cbGoTo.Items.Contains(switch) Then
                        '    cbGoTo.Items.Add(switch)
                        'End If
                        sItems.Add(switch)
                    Next switch
                End If

                If i.Param.Switch.NotNullOrEmptyS Then  ' AndAlso Not cbGoTo.Items.Contains(i.Param.Switch) Then
                    'cbGoTo.Items.Add(i.Param.Switch)
                    sItems.Add(i.Param.Switch)
                End If

                If i.Param.NoSwitch.NotNullOrEmptyS Then 'AndAlso Not cbGoTo.Items.Contains(i.Param.NoSwitch) Then
                    'cbGoTo.Items.Add(i.Param.NoSwitch)
                    sItems.Add(i.Param.NoSwitch)
                End If

                If i.Param.HelpSwitch.NotNullOrEmptyS Then 'AndAlso Not cbGoTo.Items.Contains(i.Param.HelpSwitch) Then
                    'cbGoTo.Items.Add(i.Param.HelpSwitch)
                    sItems.Add(i.Param.HelpSwitch)
                End If
            End If
        Next i

        Dim ia(sItems.Count - 1) As String
        sItems.CopyTo(ia)
        Array.Sort(ia, StringComparer.Ordinal)

        cbGoTo.Items.AddRange(ia)
        cbGoTo.SelectionStart = cbGoTo.Text.Length
        'cbGoTo.EndUpdate()

        'Console.Beep(3000, 30) ' Debug
    End Sub

    Sub rtbCommandLine_MouseUp(sender As Object, e As MouseEventArgs) Handles rtbCommandLine.MouseUp
        If e.Button = MouseButtons.Right Then
            cmsCommandLine.SuspendLayout()
            cmsCommandLine.Items.ClearAndDisplose()

            Dim copyItem = cmsCommandLine.Add("Copy Selection", Sub() Clipboard.SetText(rtbCommandLine.SelectedText))
            copyItem.KeyDisplayString = "Ctrl+C"
            copyItem.Visible = rtbCommandLine.SelectionLength > 0

            cmsCommandLine.Add("Copy Command Line", Sub() Clipboard.SetText(Params.GetCommandLine(True, True)))

            Dim find = rtbCommandLine.SelectedText

            If find.Length = 0 Then
                Dim pos = rtbCommandLine.SelectionStart
                Dim leftString = rtbCommandLine.Text.Substring(0, pos)
                Dim left = leftString.LastIndexOf(" ", StringComparison.Ordinal) + 1
                Dim right = rtbCommandLine.Text.Length
                Dim rightString = rtbCommandLine.Text.Substring(pos)
                Dim index = rightString.IndexOf(" ", StringComparison.Ordinal)

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

                cmsCommandLine.Add("Search " + find, Sub()
                                                         cbGoTo.Text = find
                                                         cbGoTo.Focus()
                                                     End Sub)
            End If

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
