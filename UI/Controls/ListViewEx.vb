
Imports System.ComponentModel
Imports System.Reflection
Imports JM.LinqFaster

Namespace UI
    Public Enum AutoCheckMode
        None
        SingleClick
        DoubleClick
    End Enum

    Public Class ListViewEx
        Inherits ListView

        Private LastDragOverPos As Point
        Private LastDrawPos As Integer

        Event ItemsChanged()
        Event ItemRemoved(item As ListViewItem)
        Event UpdateContextMenu()

        <DefaultValue(GetType(Button), Nothing)>
        Property UpButton As Button

        <DefaultValue(GetType(Button), Nothing)>
        Property DownButton As Button

        <DefaultValue(GetType(Button), Nothing)>
        Property RemoveButton As Button

        <DefaultValue(GetType(Button()), Nothing)>
        Property SingleSelectionButtons As Button()

        <DefaultValue(GetType(Button()), Nothing)>
        Property MultiSelectionButtons As Button()

        <DefaultValue(CStr(Nothing))>
        Property ItemCheckProperty As String

        Sub OnItemsChanged()
            RaiseEvent ItemsChanged()
        End Sub

        <DefaultValue(False)>
        Property ShowContextMenuOnLeftClick As Boolean

        <DefaultValue(False)>
        Property RightClickOnlyForMenu As Boolean

        <DefaultValue(GetType(AutoCheckMode), "DoubleClick")>
        Property AutoCheckMode As AutoCheckMode = AutoCheckMode.DoubleClick

        Protected Overrides Property DoubleBuffered As Boolean
            Get
                Return True 'Overrrided
            End Get
            Set(value As Boolean)
                MyBase.DoubleBuffered = value
            End Set
        End Property
        Sub New()
            ' DoubleBuffered = True 'Overrrided
        End Sub

        Sub SelectFirst()
            If Items.Count > 0 Then
                Items(0).Selected = True
            End If

            UpdateControls()
        End Sub

        Function SelectedItem(Of T)() As T
            If SelectedItems.Count > 0 Then
                Return DirectCast(SelectedItems(0).Tag, T)
            End If
        End Function

        Function SelectedItem() As Object
            If SelectedItems.Count > 0 Then
                Return SelectedItems(0).Tag
            End If
        End Function

        Function AddItem(item As Object) As ListViewItem
            'Dim lvI = Items.Add("")
            'lvI.Tag = item
            Dim lvI = Items.Add(New ListViewItem(item.ToString) With {.Tag = item})

            'RefreshItem(lvI.Index)
            If ItemCheckProperty.NotNullOrEmptyS Then
                lvI.Checked = CBool(item.GetType.GetProperty(ItemCheckProperty).GetValue(item))
            End If

            'lvI.Text = item.ToString

            OnItemsChanged()
            Return lvI
        End Function

        Sub AddItems(Of T)(itemsL As List(Of T))
            Dim icpNN As Boolean = ItemCheckProperty.NotNullOrEmptyS

            Dim itmAr(Items.Count - 1) As ListViewItem
            For i = 0 To itemsL.Count - 1
                Dim itm = itemsL(i)
                Dim lvI = New ListViewItem(itm.ToString) With {.Tag = itm}
                'RefreshItem(lvI.Index)

                If icpNN Then
                    lvI.Checked = CBool(itm.GetType.GetProperty(ItemCheckProperty).GetValue(itm))
                End If
                itmAr(i) = lvI
                'lvI.Text = item .ToString
            Next i
            Items.AddRange(itmAr)

            OnItemsChanged()
        End Sub

        Sub RefreshItem(index As Integer)
            Dim lvI As ListViewItem = Items(index)
            If ItemCheckProperty.NotNullOrEmptyS Then
                lvI.Checked = CBool(lvI.Tag.GetType.GetProperty(ItemCheckProperty).GetValue(lvI.Tag))
            End If

            lvI.Text = lvI.Tag.ToString
        End Sub

        Sub RefreshSelection()
            For Each item As ListViewItem In SelectedItems
                RefreshItem(item.Index)
            Next
        End Sub

        Sub EnableListBoxMode()
            View = View.Details
            FullRowSelect = True
            Columns.Add("")
            HeaderStyle = ColumnHeaderStyle.None
            AddHandler Layout, Sub() Columns(0).Width = Width - 4 - 17 'SystemInformation.VerticalScrollBarWidth ' Default is=17 - NoWindows Scalling !!!!
            AddHandler HandleCreated, Sub() Columns(0).Width = Width - 4
        End Sub

        Sub UpdateControls()
            Dim siC As Integer = SelectedItems.Count

            If UpButton IsNot Nothing Then
                UpButton.Enabled = siC > 0 AndAlso SelectedIndices(0) > 0
            End If

            If DownButton IsNot Nothing Then
                DownButton.Enabled = siC > 0 AndAlso SelectedIndices(siC - 1) < Items.Count - 1
            End If

            If RemoveButton IsNot Nothing Then
                'RemoveButton.Enabled = SelectedItem() IsNot Nothing
                RemoveButton.Enabled = siC > 0 AndAlso SelectedItems(0).Tag IsNot Nothing
            End If

            If SingleSelectionButtons IsNot Nothing Then
                For Each button In SingleSelectionButtons
                    button.Enabled = siC = 1
                Next
            End If

            If MultiSelectionButtons IsNot Nothing Then
                For Each button In MultiSelectionButtons
                    button.Enabled = siC > 0
                Next
            End If
        End Sub

        Function CanMoveUp() As Boolean
            Return SelectedIndices.Count > 0 AndAlso SelectedIndices(0) > 0
        End Function

        Function CanMoveDown() As Boolean
            Dim siC As Integer = SelectedIndices.Count
            Return siC > 0 AndAlso SelectedIndices(siC - 1) < Items.Count - 1
        End Function

        Sub MoveSelectionTop()
            If CanMoveUp() Then
                BeginUpdate()
                Dim selected = SelectedItems.OfType(Of ListViewItem).ToArray

                For Each i In selected
                    Items.Remove(i)
                Next

                For x = 0 To selected.Length - 1
                    Items.Insert(x, selected(x))
                Next

                EndUpdate()
            End If
        End Sub

        Sub MoveSelectionBottom()
            If CanMoveDown() Then
                BeginUpdate()
                Dim selected = SelectedItems.OfType(Of ListViewItem).ToArray

                For Each item In selected
                    Items.Remove(item)
                Next

                Items.AddRange(selected)
                EndUpdate()
            End If
        End Sub

        Sub MoveSelectionUp()
            If CanMoveUp() Then
                Dim indexAbove = SelectedIndices(0) - 1

                If indexAbove = -1 Then
                    Exit Sub
                End If

                BeginUpdate()
                Dim itemAbove = Items(indexAbove)
                Items.RemoveAt(indexAbove)
                Dim indexLastItem = SelectedIndices(SelectedIndices.Count - 1)
                Items.Insert(indexLastItem + 1, itemAbove)
                UpdateControls()
                OnItemsChanged()
                EnsureVisible(indexAbove)
                EndUpdate()
            End If
        End Sub

        Sub MoveSelectionDown()
            If CanMoveDown() Then
                Dim indexBelow = SelectedIndices(SelectedIndices.Count - 1) + 1

                If indexBelow >= Items.Count Then
                    Exit Sub
                End If

                BeginUpdate()
                Dim itemBelow = Items(indexBelow)
                Items.RemoveAt(indexBelow)
                Dim iAbove = SelectedIndices(0) - 1
                Items.Insert(iAbove + 1, itemBelow)
                UpdateControls()
                OnItemsChanged()
                EnsureVisible(indexBelow)
                EndUpdate()
            End If
        End Sub

        Sub RemoveSelection()
            If MsgQuestion("Remove Selection?") <> DialogResult.OK Then
                Exit Sub
            End If

            If SelectedItems.Count > 0 Then
                BeginUpdate()

                If Not MultiSelect Then
                    Dim index = SelectedIndices(0)

                    If Items.Count - 1 > index Then
                        Items(index + 1).Selected = True
                    Else
                        If index > 0 Then
                            Items(index - 1).Selected = True
                        End If
                    End If

                    Dim removedItem = Items(index)
                    Items.RemoveAt(index)
                    RaiseEvent ItemRemoved(removedItem)
                Else
                    Dim iFirst = SelectedIndices(0)
                    Dim indices(SelectedIndices.Count - 1) As Integer
                    SelectedIndices.CopyTo(indices, 0)

                    For i = indices.Length - 1 To 0 Step -1
                        Dim removedItem = Items(indices(i))
                        Items.RemoveAt(indices(i))
                        RaiseEvent ItemRemoved(removedItem)
                    Next

                    If Items.Count > 0 Then
                        Dim index = If(iFirst > Items.Count - 1, Items.Count - 1, iFirst)
                        Items(index).Selected = True
                        EnsureVisible(index)
                    End If
                End If

                EndUpdate()
                UpdateControls()
                OnItemsChanged()
            End If
        End Sub

        Sub SortItems()
            BeginUpdate()
            Dim sortedItems = Items.OfType(Of ListViewItem).ToArray.OrderByF(Function(item) item.Text, StringComparer.OrdinalIgnoreCase) 'Added OrdinaComp !!!
            Items.Clear()
            Items.AddRange(sortedItems)
            EndUpdate()
            UpdateControls()
            OnItemsChanged()
        End Sub

        Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
            UpdateControls()
            MyBase.OnSelectedIndexChanged(e)
        End Sub

        Sub HideFocusRectange()
            Const UIS_SET = 1, UISF_HIDEFOCUS = &H1, WM_CHANGEUISTATE = &H127
            Native.SendMessage(Handle, WM_CHANGEUISTATE, MAKEWPARAM(UIS_SET, UISF_HIDEFOCUS), 0)
        End Sub

        Function MAKEWPARAM(low As Integer, high As Integer) As Integer
            Return (low And &HFFFF) Or (high << 16)
        End Function

        Protected Overrides Sub WndProc(ByRef m As Message)
            Select Case m.Msg
                Case &H203 'WM_LBUTTONDBLCLK
                    If AutoCheckMode <> AutoCheckMode.DoubleClick AndAlso CheckBoxes Then
                        OnDoubleClick(Nothing)
                        Exit Sub
                    End If
                Case &H201 'WM_LBUTTONDOWN
                    If AutoCheckMode = AutoCheckMode.SingleClick AndAlso CheckBoxes Then
                        Dim pos = ClientMousePos
                        Dim item = GetItemAt(pos.X, pos.Y)

                        If item IsNot Nothing Then
                            Dim itemBounds = item.GetBounds(ItemBoundsPortion.Entire)

                            If pos.X > itemBounds.Left + itemBounds.Height Then
                                item.Checked = Not item.Checked
                            End If
                        End If
                    End If
                Case &H204 'WM_RBUTTONDOWN
                    If RightClickOnlyForMenu Then
                        m.Result = New IntPtr(1)
                        ShowMenu()
                        Exit Sub
                    End If
            End Select

            MyBase.WndProc(m)
        End Sub

        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            If e.Button = MouseButtons.Right Then
                RaiseEvent UpdateContextMenu()
            End If

            If ShowContextMenuOnLeftClick AndAlso e.Button = MouseButtons.Left Then
                RaiseEvent UpdateContextMenu()
                ContextMenuStrip.Show(Me, e.Location)
            End If

            MyBase.OnMouseUp(e)
        End Sub

        Sub ShowMenu()
            RaiseEvent UpdateContextMenu()
            ContextMenuStrip.Show(Me, PointToClient(MousePosition))
        End Sub

        Function GetBounds(mousePos As Point) As Rectangle
            Dim x, y, w, h, columnLeft, checkLength As Integer
            Dim cb As Boolean = CheckBoxes

            For Each header As ColumnHeader In Columns

                If header.Index = 0 AndAlso cb Then
                    checkLength = 20
                Else
                    checkLength = 0
                End If

                If mousePos.X >= columnLeft + checkLength AndAlso
                    mousePos.X <= columnLeft + header.Width Then

                    x = columnLeft + checkLength
                    w = header.Width - checkLength
                    Exit For
                End If

                columnLeft += header.Width
            Next

            For Each item As ListViewItem In Items
                Dim bounds = item.GetBounds(ItemBoundsPortion.Entire)

                If mousePos.Y >= bounds.Top AndAlso mousePos.Y <= bounds.Bottom Then
                    y = bounds.Top
                    h = bounds.Height
                End If
            Next

            If w = 0 OrElse h = 0 Then
                Return Rectangle.Empty
            Else
                Return New Rectangle(x, y, w, h)
            End If
        End Function

        Function GetPos(mousePos As Point) As Point
            Dim x, y, checkLength, columnLeft As Integer
            Dim cb As Boolean = CheckBoxes

            For Each header As ColumnHeader In Columns

                If header.Index = 0 AndAlso cb Then
                    checkLength = 20
                Else
                    checkLength = 0
                End If

                If mousePos.X >= columnLeft + checkLength AndAlso
                    mousePos.X <= columnLeft + header.Width Then

                    x = header.Index
                End If

                columnLeft += header.Width
            Next header

            For Each item As ListViewItem In Items
                Dim bounds = item.GetBounds(ItemBoundsPortion.Entire)

                If mousePos.Y >= bounds.Top AndAlso mousePos.Y <= bounds.Bottom Then
                    y = item.Index
                End If
            Next item

            Return New Point(x, y)
        End Function

        Protected Overrides Sub OnDragEnter(e As DragEventArgs)
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                e.Effect = DragDropEffects.Copy
                MyBase.OnDragEnter(e)
                Exit Sub
            End If

            e.Effect = DragDropEffects.Move
            MyBase.OnDragEnter(e)
        End Sub

        Protected Overrides Sub OnItemDrag(e As ItemDragEventArgs)
            If SelectedItems.Count > 1 Then
                Exit Sub
            End If

            DoDragDrop(e.Item, DragDropEffects.Move)
            MyBase.OnItemDrag(e)
        End Sub

        Function GetMousePos() As Point
            Return PointToClient(Control.MousePosition)
        End Function

        Protected Overrides Sub OnDragOver(e As DragEventArgs)
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                MyBase.OnDragOver(e)
                Exit Sub
            End If

            If Control.MousePosition <> LastDragOverPos Then
                Dim mousePos = GetMousePos()
                Dim bounds = GetBounds(mousePos)

                If Not e.Data.GetDataPresent(GetType(ListViewItem)) Then
                    e.Effect = DragDropEffects.None
                ElseIf Not bounds = Rectangle.Empty Then
                    e.Effect = DragDropEffects.Move
                    Dim y As Integer

                    If Math.Abs(mousePos.Y - bounds.Top) <
                        Math.Abs(mousePos.Y - bounds.Bottom) Then

                        y = bounds.Top
                    Else
                        y = bounds.Bottom
                    End If

                    If y <> LastDrawPos Then
                        LastDrawPos = y
                        Refresh()

                        Using gx As Graphics = CreateGraphics()
                            gx.DrawLine(Pens.Black, 0, y, Width, y)
                        End Using
                    End If
                Else
                    LastDrawPos = -1
                    e.Effect = DragDropEffects.None
                    Refresh()
                End If
            End If

            LastDragOverPos = Control.MousePosition
            MyBase.OnDragOver(e)
        End Sub

        Protected Overrides Sub OnDragDrop(e As DragEventArgs)
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim form = FindForm()

                If form.AllowDrop Then
                    form.GetType.GetMethod("OnDragDrop", BindingFlags.Instance Or BindingFlags.NonPublic).Invoke(form, {e})
                End If

                Exit Sub
            End If

            Dim item = DirectCast(e.Data.GetData(GetType(ListViewItem)), ListViewItem)
            Dim mousePos = GetMousePos()
            Dim p = GetPos(mousePos)
            Dim r = GetBounds(mousePos)
            Dim index As Integer

            If Math.Abs(mousePos.Y - r.Top) < Math.Abs(mousePos.Y - r.Bottom) Then

                index = p.Y
            Else
                index = p.Y + 1
            End If

            If index > item.Index Then
                index -= 1
            End If

            item.Remove()
            Items.Insert(index, item)
            MyBase.OnDragDrop(e)
        End Sub

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            Native.SetWindowTheme(Handle, "explorer", Nothing)

            If UpButton IsNot Nothing Then
                UpButton.AddClickAction(AddressOf MoveSelectionUp)
            End If

            If DownButton IsNot Nothing Then
                DownButton.AddClickAction(AddressOf MoveSelectionDown)
            End If

            If RemoveButton IsNot Nothing Then
                RemoveButton.AddClickAction(AddressOf RemoveSelection)
            End If

            If ItemCheckProperty.NotNullOrEmptyS Then
                AddHandler ItemCheck, Sub(sender As Object, e2 As ItemCheckEventArgs)
                                          Items(e2.Index).Tag.GetType.GetProperty(ItemCheckProperty).SetValue(Items(e2.Index).Tag, e2.NewValue = CheckState.Checked)
                                          OnItemsChanged()
                                      End Sub
            End If
        End Sub

        Protected Overrides Sub OnColumnClick(e As ColumnClickEventArgs)
            MyBase.OnColumnClick(e)

            Dim sorter = TryCast(ListViewItemSorter, ColumnSorter)

            If sorter IsNot Nothing Then
                If sorter.LastColumn = e.Column Then
                    If Sorting = SortOrder.Ascending Then
                        Sorting = SortOrder.Descending
                    Else
                        Sorting = SortOrder.Ascending
                    End If
                Else
                    Sorting = SortOrder.Descending
                End If

                sorter.ColumnIndex = e.Column
                Sort()
            End If
        End Sub

        Class ColumnSorter
            Implements IComparer

            Property LastColumn As Integer
            Property ColumnIndex As Integer

            Function Compare(o1 As Object, o2 As Object) As Integer Implements IComparer.Compare
                Dim item1 = DirectCast(o2, ListViewItem)
                Dim item2 = DirectCast(o1, ListViewItem)

                Dim s1 = DirectCast(item1.SubItems(ColumnIndex).Tag, IComparable)
                Dim s2 = DirectCast(item2.SubItems(ColumnIndex).Tag, IComparable)

                If s1 Is Nothing Then
                    s1 = DirectCast(item1.Tag, IComparable)
                    s2 = DirectCast(item2.Tag, IComparable)
                End If

                Dim r As Integer

                If item1.ListView.Sorting = SortOrder.Ascending Then
                    r = s1.CompareTo(s2)
                Else
                    r = s2.CompareTo(s1)
                End If

                LastColumn = ColumnIndex

                Return r
            End Function
        End Class

        Shadows Sub AutoResizeColumns()
            Dim isItms As Boolean = Items.Count > 0

            For Each header As ColumnHeader In Columns
                If isItms Then
                    Dim inc As Integer
                    Dim hW As Integer
                    Select Case inc
                        Case 0
                            header.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                            hW = header.Width
                        Case Else
                            header.Width = ClientSize.Width - hW
                    End Select
                    inc += 1
                Else
                    header.Width = 0 'If(inc = 0, 0, ClientSize.Width)  'DefaultColWidth = 60
                End If
            Next header

        End Sub
    End Class
End Namespace
