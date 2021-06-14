
Imports System.ComponentModel
Imports StaxRip.UI

Public Class FiltersListView
    Inherits ListViewEx

    Private BlockItemCheck As Boolean
    WithEvents Menu As New ContextMenuStripEx
    Property IsLoading As Boolean

    Event Changed()

    Sub New()
        BeginUpdate()
        AllowDrop = True
        Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        CheckBoxes = True
        View = View.Details
        HideSelection = False
        FullRowSelect = True
        MultiSelect = False
        HeaderStyle = ColumnHeaderStyle.None
        Columns.Add("")
        Columns.Add("Type")
        Columns.Add("Name")
        If s.UIScaleFactor <> 1 Then Menu.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
        ContextMenuStrip = Menu
        HideFocusRectange()
        AddHandler VideoScript.Changed, Sub(script As VideoScript)
                                            If Not p.Script Is Nothing AndAlso script Is p.Script Then
                                                OnChanged()
                                            End If
                                        End Sub
        EndUpdate()
    End Sub

    Sub Load()
        If p.Script.Engine = ScriptEngine.AviSynth Then
            g.MainForm.lgbFilters.Text = "AVS Filters"
        Else
            g.MainForm.lgbFilters.Text = "VS Filters"
        End If

        BlockItemCheck = True
        BeginUpdate()
        Items.Clear()

        For Each filter In p.Script.Filters
            Dim item As New ListViewItem
            item.Tag = filter
            item.Checked = filter.Active
            item.SubItems.Add(filter.Category)

            If filter.Name.NullOrEmptyS Then
                item.SubItems.Add(filter.Script)
            Else
                item.SubItems.Add(filter.Name)
            End If

            item.SubItems.Add(filter.Script)
            Items.Add(item)
        Next

        AutoResizeColumns(True)
        EndUpdate()
        BlockItemCheck = False
    End Sub

    Sub RebuildMenu()
        Menu.SuspendLayout()
        ActionMenuItem.LayoutSuspendCreate(96)
        Menu.Items.ClearAndDispose
        Dim filterProfiles = If(p.Script.Engine = ScriptEngine.AviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
        Dim selectedFunc = Function() SelectedItems.Count > 0
        Dim m0 = New ActionMenuItem("a") With {.VisibleFunc = selectedFunc}
        RemoveHandler Menu.Opening, AddressOf m0.Opening 'CrudeVersion ?!?
        AddHandler Menu.Opening, AddressOf m0.Opening
        Dim sep0 As New ToolStripSeparator

        Dim replaceMenuItem = New ActionMenuItem("Replace", ImageHelp.GetImageC(Symbol.Switch)) With {.VisibleFunc = selectedFunc}
        RemoveHandler Menu.Opening, AddressOf replaceMenuItem.Opening
        AddHandler Menu.Opening, AddressOf replaceMenuItem.Opening
        replaceMenuItem.DropDown.SuspendLayout()
        For Each i In filterProfiles
            For Each i2 In i.Filters
                ActionMenuItem.Add(replaceMenuItem.DropDownItems, i.Name + " | " + i2.Path, Sub() ReplaceClick(i2), i2.Script)
            Next
        Next
        replaceMenuItem.DropDown.ResumeLayout(False)

        Dim insertMenuItem = New ActionMenuItem("Insert", ImageHelp.GetImageC(Symbol.LeftArrowKeyTime0)) With {.VisibleFunc = selectedFunc}
        RemoveHandler Menu.Opening, AddressOf insertMenuItem.Opening
        AddHandler Menu.Opening, AddressOf insertMenuItem.Opening
        insertMenuItem.DropDown.SuspendLayout()
        For Each i In filterProfiles
            For Each i2 In i.Filters
                ActionMenuItem.Add(insertMenuItem.DropDownItems, i.Name + " | " + i2.Path, Sub() InsertClick(i2), i2.Script)
            Next
        Next
        insertMenuItem.DropDown.ResumeLayout(False)

        Dim add = New ActionMenuItem("Add", ImageHelp.GetImageC(Symbol.Add))
        add.DropDown.SuspendLayout()
        For Each i In filterProfiles
            For Each i2 In i.Filters
                ActionMenuItem.Add(add.DropDownItems, i.Name + " | " + i2.Path, Sub() AddClick(i2), i2.Script)
            Next
        Next
        add.DropDown.ResumeLayout(False)

        Dim remove = New ActionMenuItem("Remove", AddressOf RemoveClick, ImageHelp.GetImageC(Symbol.Remove), "Removes the selected filter.") With {.EnabledFunc = selectedFunc}
        RemoveHandler Menu.Opening, AddressOf remove.Opening
        AddHandler Menu.Opening, AddressOf remove.Opening

        Dim editMenu = New ActionMenuItem("Edit Code...", AddressOf ShowEditor, ImageHelp.GetImageC(Symbol.Code), "Dialog to edit filters.")
        Dim previeMenu = New ActionMenuItem("Preview Code...", Sub() g.CodePreview(p.Script.GetFullScript), ImageHelp.GetImageC(Symbol.View), "Script code preview.")

        Dim infoMenu = New ActionMenuItem("Info...", Sub() g.ShowScriptInfo(p.Script), ImageHelp.GetImageC(Symbol.Info), "Shows script parameters.") With {.EnabledFunc = Function() p.SourceFile.NotNullOrEmptyS}
        RemoveHandler Menu.Opening, AddressOf infoMenu.Opening
        AddHandler Menu.Opening, AddressOf infoMenu.Opening

        Dim playMenu = New ActionMenuItem("Play", Sub() g.PlayScript(p.Script), ImageHelp.GetImageC(Symbol.Play), "Plays the script with the AVI player.") With {.EnabledFunc = Function() p.SourceFile.NotNullOrEmptyS}
        RemoveHandler Menu.Opening, AddressOf playMenu.Opening
        AddHandler Menu.Opening, AddressOf playMenu.Opening

        Dim profilesMenu = New ActionMenuItem("Profiles...", AddressOf g.MainForm.ShowFilterProfilesDialog, ImageHelp.GetImageC(Symbol.FavoriteStar), "Dialog to edit profiles.")

        Dim moveUpItem = New ActionMenuItem("Move Up", AddressOf MoveUp, ImageHelp.GetImageC(Symbol.Up), "Moves the selected item up.") With {.EnabledFunc = Function() SelectedItems.Count > 0 AndAlso SelectedItems(0).Index > 0}
        RemoveHandler Menu.Opening, AddressOf moveUpItem.Opening
        AddHandler Menu.Opening, AddressOf moveUpItem.Opening

        Dim moveDownItem = New ActionMenuItem("Move Down", AddressOf MoveDown, ImageHelp.GetImageC(Symbol.Down), "Moves the selected item down.") With {.EnabledFunc = Function() SelectedItems.Count > 0 AndAlso SelectedItems(0).Index < Items.Count - 1}
        RemoveHandler Menu.Opening, AddressOf moveDownItem.Opening
        AddHandler Menu.Opening, AddressOf moveDownItem.Opening

        Dim setup = New ActionMenuItem("Filter Setup", ImageHelp.GetImageC(Symbol.MultiSelect))
        setup.DropDown.SuspendLayout()
        g.PopulateProfileMenu(setup.DropDownItems, s.FilterSetupProfiles, AddressOf g.MainForm.ShowFilterSetupProfilesDialog, AddressOf g.MainForm.LoadFilterSetup)
        setup.DropDown.ResumeLayout(False)

        Menu.Items.AddRange({m0, sep0, replaceMenuItem, insertMenuItem, add, New ToolStripSeparator, remove, editMenu, previeMenu, infoMenu, playMenu, profilesMenu, New ToolStripSeparator, moveUpItem, moveDownItem, New ToolStripSeparator, setup})
        ActionMenuItem.LayoutResume(False)
        Menu.ResumeLayout()

        Dim mop As CancelEventHandler = Sub()
                                            Dim active = DirectCast(Menu.Items(0), ActionMenuItem)
                                            Menu.SuspendLayout()
                                            active.DropDown.SuspendLayout()
                                            active.DropDownItems.ClearAndDispose()
                                            ActionMenuItem.LayoutSuspendCreate()
                                            Dim selC As Boolean = SelectedItems.Count > 0
                                            sep0.Visible = selC

                                            If selC Then
                                                Dim selectedFilter = DirectCast(SelectedItems(0).Tag, VideoFilter)
                                                active.Text = selectedFilter.Category

                                                For Each i In filterProfiles
                                                    If i.Name.EqualsEx(selectedFilter.Category) Then
                                                        For Each i2 In i.Filters
                                                            ActionMenuItem.Add(active.DropDownItems, i2.Path, Sub() ReplaceClick(i2.GetCopy), i2.Script)
                                                        Next i2
                                                    End If
                                                Next i
                                            End If

                                            ActionMenuItem.LayoutResume(False)
                                            active.DropDown.ResumeLayout()
                                            Menu.ResumeLayout(False)
                                        End Sub
        RemoveHandler Menu.Opening, mop
        AddHandler Menu.Opening, mop
    End Sub

    Sub MoveUp()
        If SelectedItems.Count = 0 Then
            Exit Sub
        End If

        Dim index = SelectedItems(0).Index

        If index = 0 Then
            Exit Sub
        End If

        Dim sel = p.Script.Filters(index)
        p.Script.Filters.Remove(sel)
        p.Script.Filters.Insert(index - 1, sel)
        Load()
        g.MainForm.Assistant()
    End Sub

    Sub MoveDown()
        If SelectedItems.Count = 0 Then
            Exit Sub
        End If

        Dim index = SelectedItems(0).Index

        If index = p.Script.Filters.Count - 1 Then
            Exit Sub
        End If

        Dim sel = p.Script.Filters(index)
        p.Script.Filters.Remove(sel)
        p.Script.Filters.Insert(index + 1, sel)
        Load()
        g.MainForm.Assistant()
    End Sub

    Sub ShowEditor()
        If p.Script.Edit = DialogResult.OK Then
            OnChanged()
        End If
    End Sub

    Sub ReplaceClick(filter As VideoFilter)
        filter = filter.GetCopy
        Dim val = Macro.ExpandGUI(filter.Script)

        If val.Cancel Then
            Exit Sub
        End If

        If val.Caption.NotNullOrEmptyS AndAlso Not val.Value.Equals(filter.Script) Then
            Dim path = filter.Path.Replace("...", "")

            If val.Caption.EndsWith(path, StringComparison.Ordinal) Then
                filter.Path = val.Caption
            Else
                filter.Path = path + " " + val.Caption
            End If
        End If

        filter.Script = val.Value
        Dim index = SelectedItems(0).Index
        p.Script.SetFilter(index, filter)
        Items(index).Selected = True
    End Sub

    Sub InsertClick(filter As VideoFilter)
        filter = filter.GetCopy
        Dim val = Macro.ExpandGUI(filter.Script)

        If val.Cancel Then
            Exit Sub
        End If

        If val.Caption.NotNullOrEmptyS AndAlso Not val.Value.Equals(filter.Script) Then
            Dim path = filter.Path.Replace("...", "")

            If val.Caption.EndsWith(path, StringComparison.Ordinal) Then
                filter.Path = val.Caption
            Else
                filter.Path = path + " " + val.Caption
            End If
        End If

        filter.Script = val.Value
        Dim index = SelectedItems(0).Index
        p.Script.InsertFilter(index, filter)
        Items(index).Selected = True
    End Sub

    Sub AddClick(filter As VideoFilter)
        filter = filter.GetCopy
        Dim val = Macro.ExpandGUI(filter.Script)

        If val.Cancel Then
            Exit Sub
        End If

        If val.Caption.NotNullOrEmptyS AndAlso Not val.Value.Equals(filter.Script) Then
            Dim path = filter.Path.Replace("...", "")

            If val.Caption.EndsWith(path, StringComparison.Ordinal) Then
                filter.Path = val.Caption
            Else
                filter.Path = path + " " + val.Caption
            End If
        End If

        filter.Script = val.Value
        p.Script.AddFilter(filter)
        Items(Items.Count - 1).Selected = True
    End Sub

    Sub OnChanged()
        If IsLoading Then
            Exit Sub
        End If

        Load()
        RaiseEvent Changed()
    End Sub

    Sub RemoveClick()
        If MsgQuestion("Remove Selection?") = DialogResult.OK AndAlso SelectedItems.Count > 0 Then
            p.Script.RemoveFilterAt(SelectedItems(0).Index)
        End If
    End Sub

    Sub UpdateDocument()
        p.Script.Filters.Clear()

        For Each item As ListViewItem In Items
            p.Script.Filters.Add(DirectCast(item.Tag, VideoFilter))
        Next

        OnChanged()
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        AutoResizeColumns(True)
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)
        AutoResizeColumns(True)
    End Sub

    Protected Overrides Sub OnDragDrop(e As DragEventArgs)
        BlockItemCheck = True
        MyBase.OnDragDrop(e)
        BlockItemCheck = False
        UpdateDocument()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.KeyData = Keys.Delete Then
            RemoveClick()
        End If
    End Sub

    Protected Overrides Sub OnItemCheck(e As ItemCheckEventArgs)
        MyBase.OnItemCheck(e)

        If Not BlockItemCheck AndAlso Focused Then
            Dim filter = DirectCast(Items(e.Index).Tag, VideoFilter)

            If e.NewValue = CheckState.Checked AndAlso filter.Category = "Resize" Then
                Dim form = FindForm()

                If Not form Is Nothing AndAlso TypeOf form Is MainForm Then
                    g.MainForm.SetTargetImageSize(p.TargetWidth, 0)
                End If
            End If

            filter.Active = e.NewValue = CheckState.Checked
            g.MainForm.BeginInvoke(Sub() OnChanged())
        End If
    End Sub
End Class
