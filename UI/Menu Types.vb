
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Runtime
Imports System.Threading
Imports System.Threading.Tasks
Imports JM.LinqFaster

Namespace UI
    <Serializable()>
    Public Class CustomMenuItem
        Sub New()
        End Sub

        Sub New(text As String)
            Me.Text = text
        End Sub

        <NonSerialized()>
        Public CustomMenu As CustomMenu

        Overridable Property Text As String

        Property SubItems As New List(Of CustomMenuItem)
        Property KeyData As Keys
        Property Symbol As Symbol
        Property MethodName As String

        Private ParametersValue As List(Of Object)

        Property Parameters() As List(Of Object)
            Get
                If ParametersValue Is Nothing Then
                    ParametersValue = New List(Of Object)
                End If

                Return ParametersValue
            End Get
            Set(Value As List(Of Object))
                ParametersValue = Value
            End Set
        End Property

        Sub Add(path As String)
            Add(path, Nothing, Keys.None, Symbol.None, Nothing)
        End Sub

        Sub Add(path As String, symbol As Symbol)
            Add(path, Nothing, Keys.None, symbol, Nothing)
        End Sub

        Sub Add(path As String, methodName As String)
            Add(path, methodName, Keys.None, Symbol.None, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, symbol As Symbol)
            Add(path, methodName, Keys.None, symbol, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, symbol As Symbol, params As Object())
            Add(path, methodName, Keys.None, symbol, params)
        End Sub

        Sub Add(path As String, methodName As String, keyData As Keys)
            Add(path, methodName, keyData, Symbol.None, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, keyData As Keys, symbol As Symbol)
            Add(path, methodName, keyData, symbol, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, keyData As Keys, params As Object())
            Add(path, methodName, keyData, Symbol.None, params)
        End Sub

        Sub Add(path As String, methodName As String, params As Object())
            Add(path, methodName, Keys.None, params)
        End Sub

        Sub Add(path As String, methodName As String, keyData As Keys, symbol As Symbol, params As Object())

            Dim pathArray = path.Split({"|"c}, StringSplitOptions.RemoveEmptyEntries)
            Dim l = SubItems

            For i = 0 To pathArray.Length - 1
                Dim found As Boolean = False
                Dim paTxt As String = pathArray(i)

                If i < pathArray.Length - 1 Then
                    For n = l.Count - 1 To 0 'Added Backward walk
                        Dim iItem = l(n)
                        If String.Equals(iItem.Text, paTxt) Then
                            found = True
                            l = iItem.SubItems
                            Exit For
                        End If
                    Next n
                End If

                If Not found Then
                    Dim item As New CustomMenuItem(paTxt)
                    l.Add(item)
                    l = item.SubItems

                    If i = pathArray.Length - 1 Then
                        item.MethodName = methodName
                        item.KeyData = keyData
                        item.Symbol = symbol

                        If params IsNot Nothing Then
                            item.Parameters.AddRange(params)
                        End If
                    End If
                End If
            Next i
        End Sub

        <NonSerialized()>
        Public Parent As CustomMenuItem

        Shared Sub SetParents(item As CustomMenuItem)
            For Each i In item.SubItems
                i.Parent = item
                SetParents(i)
            Next
        End Sub

        Sub Remove()
            Parent.SubItems.Remove(Me)
        End Sub

        Function GetAllItems() As List(Of CustomMenuItem)
            Dim l As New List(Of CustomMenuItem)
            AddToList(Me, l)
            Return l
        End Function

        Sub AddToList(item As CustomMenuItem, list As List(Of CustomMenuItem))
            For Each i In item.SubItems
                list.Add(i)
                AddToList(i, list)
            Next
        End Sub

        Function GetClone() As CustomMenuItem
            Return ObjectHelp.GetCopy(Me)
        End Function
    End Class

    Public Class CustomMenu
        Private Items As New List(Of CustomMenuItem)(16)

        Property Menu As Menu
        Property MenuStrip As MenuStrip
        Property ToolStrip As ToolStrip
        Property MenuItems As New List(Of ToolStripMenuItemEx)
        Property DefaultMenu As Func(Of CustomMenuItem)
        Property MenuItem As CustomMenuItem
        Property CommandManager As CommandManager

        Event Command(e As CustomMenuItemEventArgs)

        Sub New(defaultMenu As Func(Of CustomMenuItem), menuItem As CustomMenuItem, commandManager As CommandManager, toolStrip As ToolStrip)
            Me.CommandManager = commandManager
            Me.DefaultMenu = defaultMenu
            Me.MenuItem = menuItem
            Me.ToolStrip = toolStrip
        End Sub

        Sub AddKeyDownHandler(control As Control)
            AddHandler control.KeyDown, AddressOf OnKeyDown
        End Sub

        Function GetKeys() As StringPairList
            Dim ret As New StringPairList

            For Each i As ToolStripMenuItemEx In MenuItems
                If i.ShortcutKeyDisplayString?.Length > 0 Then
                    Dim sp As New StringPair

                    If i.Text.EndsWith("...", StringComparison.Ordinal) Then
                        sp.Name = i.Text.TrimEnd("."c)
                    Else
                        sp.Name = i.Text
                    End If

                    sp.Value = i.ShortcutKeyDisplayString
                    ret.Add(sp)
                End If
            Next

            Return ret
        End Function

        Function GetTips() As StringPairList
            Dim ret As New StringPairList

            For Each i As ToolStripMenuItemEx In MenuItems
                Dim help = i.GetHelp

                If help IsNot Nothing Then
                    ret.Add(help)
                End If
            Next

            Return ret
        End Function

        Function Edit() As CustomMenuItem

            Using form As New CustomMenuEditor(Me)
                If form.ShowDialog = DialogResult.OK Then
                    MenuItem = form.GetState
                    BuildMenu()
                End If
            End Using
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce 'debug
            GC.Collect(2, GCCollectionMode.Forced, True, True)
            'GC.WaitForPendingFinalizers()
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
            Return MenuItem
        End Function

        Sub OnKeyDown(sender As Object, e As KeyEventArgs)
            For Each i As CustomMenuItem In Items
                If i.KeyData = e.KeyData Then
                    OnCommand(i)
                    Exit For
                End If
            Next
        End Sub

        Sub MenuClick(sender As Object, e As EventArgs)
            Dim tsmi = TryCast(sender, ToolStripMenuItemEx)
            If tsmi IsNot Nothing Then
                OnCommand(tsmi.CustomMenuItem)
            End If
        End Sub

        Sub OnCommand(item As CustomMenuItem)
            If item.MethodName Is Nothing Then Exit Sub
            Dim iMNLen As Integer = item.MethodName.Length
            If iMNLen > 0 Then
                Dim e As New CustomMenuItemEventArgs(item)
                RaiseEvent Command(e)

                If Not e.Handled Then
                    If iMNLen = 18 AndAlso item.Symbol = Symbol.MusicInfo Then '.GetValueOrDefault=18[faster???] ' Then  OrElse item.MethodName.Equals("ShowAudioConverter")
                        g.MainForm.ShowAudioConverter()
                        Exit Sub ' Debug ??? Test This
                    Else
                        Process(item)
                    End If
                End If

                Dim form = ToolStrip.FindForm
                If form IsNot Nothing Then
                    form.Invalidate()
                    'form.Refresh()
                End If
            End If
        End Sub

        Sub Process(item As CustomMenuItem)
            CommandManager.Process(item.MethodName, item.Parameters)
        End Sub

        Sub BuildMenu()
            ToolStrip.SuspendLayout()
            ToolStrip.Items.ClearAndDispose
            Items.Clear()
            MenuItems.Clear()
            Application.DoEvents()
            ToolStrip.ResumeLayout(False)
            BuildMenu(ToolStrip, MenuItem)
        End Sub

        Sub BuildMenu(menu As Object, item As CustomMenuItem)
            If item.SubItems.Count <= 0 Then Exit Sub
            Dim tsiL As New List(Of ToolStripItem)(item.SubItems.Count)
            Dim toolsST As Integer
            Dim tsmi As ToolStripMenuItem
            Dim tStrip As ToolStrip
            If TypeOf menu Is ToolStripMenuItem Then
                tsmi = DirectCast(menu, ToolStripMenuItem)
                tsmi.DropDown.SuspendLayout() 'ToDO Test This!!! Experiment!!
                toolsST = 1
            ElseIf TypeOf menu Is ToolStrip Then
                tStrip = DirectCast(menu, ToolStrip)
                tStrip.SuspendLayout()
                toolsST = 2
            End If

            For Each cmi As CustomMenuItem In item.SubItems
                cmi.CustomMenu = Me
                Dim tsi As ToolStripItem

                If String.Equals(cmi.Text, "-") Then
                    tsi = New ToolStripSeparator
                Else
                    Dim mi = If(cmi.Symbol > 0, New ToolStripMenuItemEx(ImageHelp.GetImageC(cmi.Symbol)), New ToolStripMenuItemEx()) 'Image in contructor instead of Async Await
                    MenuItems.Add(mi)
                    tsi = mi
                    mi.CustomMenuItem = cmi

                    Dim keys = KeysHelp.GetKeyString(cmi.KeyData)

                    If keys?.Length > 0 Then
                        mi.ShortcutKeyDisplayString = keys
                    End If

                    'If mi.ShortcutKeyDisplayString Is Nothing Then 'Test This  - Removed !!!! 202107
                    '    mi.ShortcutKeyDisplayString &= ""
                    'End If

                    'If Not mi.ShortcutKeyDisplayString.EndsWith(" ", StringComparison.Ordinal) Then ' Opt.: No MenuSpace Test This!!!  "Edit Menu..." & g.MenuSpace
                    '    mi.ShortcutKeyDisplayString += g.MenuSpace
                    'End If
                    AddHandler tsi.Click, AddressOf MenuClick
                End If

                Items.Add(cmi)
                tsi.Text = cmi.Text
                tsiL.Add(tsi)
                BuildMenu(tsi, cmi)
            Next cmi

            Select Case toolsST
                Case 1
                    tsmi.DropDownItems.AddRange(tsiL.ToArray)
                    tsmi.DropDown.ResumeLayout(False) 'ToDO Test This!!! Experiment!!
                Case 2
                    tStrip.Items.AddRange(tsiL.ToArray)
                    tStrip.ResumeLayout()
            End Select
        End Sub
    End Class

    Public Class CustomMenuItemEventArgs
        Inherits EventArgs

        Property Handled As Boolean
        Property Item As CustomMenuItem

        Sub New(item As CustomMenuItem)
            Me.Item = item
        End Sub
    End Class

    Public Class ToolStripMenuItemEx
        Inherits ToolStripMenuItem

        Shared Property UseTooltips As Boolean
        <CompilerServices.MethodImpl(AggrInlin)> Sub New()
        End Sub

        <CompilerServices.MethodImpl(AggrInlin)> Sub New(text As String)
            MyBase.New(text)
        End Sub

        <CompilerServices.MethodImpl(AggrInlin)> Sub New(image As Image)
            Me.ImageScaling = ToolStripItemImageScaling.None
            Me.Image = image
        End Sub
        <CompilerServices.MethodImpl(AggrInlin)> Sub New(text As String, image As Image)
            Me.Text = text
            Me.ImageScaling = ToolStripItemImageScaling.None
            Me.Image = image
        End Sub

        Overrides Function GetPreferredSize(constrainingSize As Size) As Size
            'Dim ret = MyBase.GetPreferredSize(constrainingSize) 'ret.Height = CInt(If(s.UIScaleFactor = 1.0F, 16, Font.Height) * 1.4) 'Font.Height * 'Debug Experimental - NoScaling!!!!
            Return New Size(MyBase.GetPreferredSize(constrainingSize).Width, CInt(If(s.UIScaleFactor = 1.0F, 16, Font.Height) * 1.4))
        End Function
        'Sub SetImage(symbol As Symbol)
        '    SetImageAsync(symbol, Me)
        'End Sub
        'Shared Async Sub SetImageAsync(symbol As Symbol, mi As ToolStripMenuItem)
        '    If symbol = Symbol.None Then
        '        '    mi.Image = Nothing 'Test This ???
        '        Exit Sub
        '    End If
        '    'Dim img = Await ImageHelp.GetSymbolImageAsync(symbol) 'Weird Nested Awaits
        '    Dim img = Await Task.Run(Function() ImageHelp.GetSymbolImage(symbol))
        '    Try
        '        If Not mi.IsDisposed Then
        '            mi.ImageScaling = ToolStripItemImageScaling.None
        '            mi.Image = img
        '        Else
        '            img?.Dispose()
        '        End If
        '    Catch
        '    End Try
        'End Sub
        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)
            CustomMenuItem = Nothing
            'Events.Dispose() 'Needed???
        End Sub

        Function GetHelp() As StringPair
            'If CustomMenuItem IsNot Nothing AndAlso CustomMenuItem.CustomMenu IsNot Nothing AndAlso CustomMenuItem.CustomMenu.CommandManager.HasCommand(CustomMenuItem.MethodName) Then
            Dim command = CustomMenuItem?.CustomMenu?.CommandManager.GetCommand(CustomMenuItem.MethodName)

            If command?.Attribute.Description?.Length > 0 Then

                Dim ret As New StringPair With {.Name = If(Text.EndsWith("...", StringComparison.Ordinal), Text.TrimEnd("."c), Text), .Value = command.Attribute.Description}
                Dim paramHelp = command.GetParameterHelp(CustomMenuItem.Parameters)
                If paramHelp?.Length > 0 Then ret.Value += " (" + paramHelp + ")"

                Return ret
            End If
            'End If
        End Function

        Private CustomMenuItemValue As CustomMenuItem

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property CustomMenuItem() As CustomMenuItem
            Get
                Return CustomMenuItemValue
            End Get
            Set(Value As CustomMenuItem)
                CustomMenuItemValue = Value

                'If Value IsNot Nothing AndAlso Value.CustomMenu IsNot Nothing AndAlso Value.CustomMenu.CommandManager.HasCommand(Value.MethodName) Then
                Dim c = Value?.CustomMenu?.CommandManager.GetCommand(Value.MethodName)
                If c IsNot Nothing AndAlso Not String.Equals(c.MethodInfo.Name, "DynamicMenuItem") Then
                    Help = If(String.Equals(c.MethodInfo.Name, "ExecuteCommandLine"),
                            Value.Parameters(0).ToString.Trim(""""c),
                            c.Attribute.Description)
                End If
                'End If
            End Set
        End Property

        'Private Function ShouldSerializeHelpText() As Boolean
        '    Return HelpValue?.Length > 0
        'End Function

        Private HelpValue As String

        <Editor(GetType(StringEditor), GetType(UITypeEditor))>
        <DefaultValue("")>
        Property Help() As String
            Get
                Return HelpValue
            End Get
            Set(Value As String)
                HelpValue = Value
                If UseTooltips AndAlso HelpValue?.Length > 0 Then 'Test this Mod. Was: UseToolsTips Check after assigment to value
                    If HelpValue.Length < 240 Then 'org:80 Mod.
                        ToolTipText = HelpValue '.TrimEnd("."c)
                    Else
                        ToolTipText = HelpValue.Substring(0, 240) & "..." '.TrimEnd("."c)
                        'ToolTipText = "Right-click for help"
                    End If
                End If
            End Set
        End Property

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            If e.Button = MouseButtons.Right AndAlso Help?.Length > 0 Then
                CloseAll(Me)
                g.ShowHelp(Text, Help)
            End If

            MyBase.OnMouseDown(e)
        End Sub

        Sub CloseAll(item As Object)
            If TypeOf item Is ToolStripItem Then
                Dim d = DirectCast(item, ToolStripItem)
                CloseAll(d.Owner)
            End If

            If TypeOf item Is ToolStripDropDown Then
                Dim d = DirectCast(item, ToolStripDropDown)
                d.Close()
                CloseAll(d.OwnerItem)
            End If
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            Application.DoEvents()
            MyBase.OnClick(e)
        End Sub
    End Class

    Public Class ActionMenuItem
        Inherits ToolStripMenuItemEx

        Private Action As Action

        Property EnabledFunc As Func(Of Boolean)
        Property VisibleFunc As Func(Of Boolean)

        Property Form As Form

        <CompilerServices.MethodImpl(AggrInlin)> Sub New()
        End Sub
        'ToDo: Add Overload with DropDownCollection ???
        <CompilerServices.MethodImpl(AggrInlin)> Sub New(txt As String)
            MyBase.New(txt)
        End Sub

        <CompilerServices.MethodImpl(AggrInlin)> Sub New(text As String, actn As Action)
            MyBase.New(text)
            Me.Action = actn
        End Sub

        <CompilerServices.MethodImpl(AggrInlin)> Sub New(text As String, action As Action, tag As Object)
            MyBase.New(text)
            Me.Action = action
            Me.Tag = tag
        End Sub

        <CompilerServices.MethodImpl(AggrInlin)> Sub New(text As String, action As Action, tooltip As String)
            MyBase.New(text)
            Me.Action = action
            Me.Help = tooltip
        End Sub

        <CompilerServices.MethodImpl(AggrInlin)> Sub New(text As String, image As Image, Optional tooltip As String = Nothing)
            Me.Text = text
            Me.ImageScaling = ToolStripItemImageScaling.None
            Me.Image = image
            If tooltip IsNot Nothing Then Me.Help = tooltip
        End Sub

        <CompilerServices.MethodImpl(AggrInlin)> Sub New(text As String, action As Action, image As Image)
            Me.Text = text
            Me.ImageScaling = ToolStripItemImageScaling.None
            Me.Image = image
            Me.Action = action
        End Sub

        <CompilerServices.MethodImpl(AggrInlin)> Sub New(text As String, action As Action, image As Image, tooltip As String)
            Me.Text = text
            Me.ImageScaling = ToolStripItemImageScaling.None
            Me.Image = image
            Me.Action = action
            Me.Help = tooltip
        End Sub

        Private ShortcutValue As Keys

        Property Shortcut As Keys
            Get
                Return ShortcutValue
            End Get
            Set(value As Keys)
                If value <> Keys.None Then
                    ShortcutValue = value
                    ShortcutKeyDisplayString = KeysHelp.GetKeyString(value) '& g.MenuSpace
                    AddHandler Form.KeyDown, AddressOf KeyDown
                End If
            End Set
        End Property

        WriteOnly Property KeyDisplayString As String
            Set(value As String)
                ShortcutKeyDisplayString = value '& g.MenuSpace
            End Set
        End Property

        Sub KeyDown(sender As Object, e As KeyEventArgs)
            If Enabled AndAlso e.KeyData = Shortcut AndAlso (EnabledFunc Is Nothing OrElse EnabledFunc.Invoke) AndAlso (VisibleFunc Is Nothing OrElse VisibleFunc.Invoke) Then
                PerformClick()
                e.Handled = True
            End If
        End Sub

        Sub Opening(sender As Object, e As CancelEventArgs)
            If EnabledFunc IsNot Nothing Then ' '  problem with cms dispose opening event
                Enabled = EnabledFunc.Invoke
            End If

            If VisibleFunc IsNot Nothing Then
                Visible = VisibleFunc.Invoke
            End If
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            Application.DoEvents()

            If Action IsNot Nothing Then
                Action()
            End If

            MyBase.OnClick(e)
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If Form IsNot Nothing Then
                RemoveHandler Form.KeyDown, AddressOf KeyDown
                Form = Nothing
            End If

            '' memory leak problem with cms multiple opening event
            If VisibleFunc IsNot Nothing Then
                VisibleFunc = Nothing
                'If Owner IsNot Nothing Then  'Test Experiment???
                RemoveHandler DirectCast(Owner, ToolStripDropDown).Opening, AddressOf Opening
            End If
            If EnabledFunc IsNot Nothing Then
                EnabledFunc = Nothing
                'If Owner IsNot Nothing Then'Test Experiment???
                RemoveHandler DirectCast(Owner, ToolStripDropDown).Opening, AddressOf Opening
            End If

            Action = Nothing
            MyBase.Dispose(disposing)
        End Sub
        <CompilerServices.MethodImpl(AggrInlin)>
        Shared Function Add(Of T)(items As ToolStripItemCollection, path As String, action As Action(Of T), value As T, Optional help As String = Nothing) As ActionMenuItem
            Return Add(items, path, Sub() action(value), help)
        End Function
        <CompilerServices.MethodImpl(AggrInlin)>
        Shared Function Add(items As ToolStripItemCollection, path As String) As ActionMenuItem
            Return Add(items, path, Nothing)
        End Function
        <CompilerServices.MethodImpl(AggrInlin)>
        Shared Function Add(items As ToolStripItemCollection, path As String, action As Action) As ActionMenuItem
            Return Add(items, path, action, Nothing)
        End Function

        Shared Function Add(items As ToolStripItemCollection, path As String, action As Action, tip As String) As ActionMenuItem
            Dim a = path.Split({" | "}, StringSplitOptions.RemoveEmptyEntries)

            For x = 0 To a.Length - 1
                Dim found = False
                Dim txtP As String = a(x)

                If x < a.Length - 1 Then

                    For n = items.Count - 1 To 0 Step -1 'Seems Faster if backward, except EventCmd
                        Dim i = items.Item(n)
                        If String.Equals(i.Text, txtP) Then
                            found = True
                            items = DirectCast(i, ToolStripMenuItem).DropDownItems
                            Exit For
                        End If
                    Next n
                End If

                If Not found Then
                    If x = a.Length - 1 Then
                        If txtP.Length = 1 AndAlso txtP(0) = "-"c Then
                            items.Add(New ToolStripSeparator)
                        Else
                            Dim item = If(tip Is Nothing, New ActionMenuItem(txtP, action), New ActionMenuItem(txtP, action, tip))
                            items.Add(item)
                            Return item
                        End If
                    Else
                        Dim itm As New ToolStripMenuItemEx(txtP)

                        If LayoutSuspendList IsNot Nothing Then  'ToDo: Testing!!! or use par laySuspL
                            Dim dd As ToolStripDropDown = itm.DropDown
                            LayoutSuspendList.Add(dd)
                            dd.SuspendLayout()
                        End If

                        items.Add(itm)
                        items = itm.DropDownItems
                    End If
                End If
            Next x
        End Function

        Private Shared LayoutSuspendList As List(Of ToolStripDropDown) 'ToDO Testing
        <CompilerServices.MethodImpl(AggrInlin)> Public Shared Sub LayoutResume()
            If LayoutSuspendList IsNot Nothing Then
                For Each tsdd In LayoutSuspendList
                    tsdd.ResumeLayout()
                Next tsdd
                LayoutSuspendList = Nothing
            End If
        End Sub
        <CompilerServices.MethodImpl(AggrInlin)> Public Shared Sub LayoutResume(performLayout As Boolean)
            If LayoutSuspendList IsNot Nothing Then
                For ls = 0 To LayoutSuspendList.Count - 1
                    LayoutSuspendList(ls).ResumeLayout(performLayout)
                Next ls
                LayoutSuspendList = Nothing
            End If
        End Sub
        <CompilerServices.MethodImpl(AggrInlin)> Public Shared Function LayoutSuspendCreate(Optional capacity As Integer = 0) As List(Of ToolStripDropDown)
            'If LayoutSuspendList IsNot Nothing Then Console.Beep(4100, 500) 'Debug
            LayoutSuspendList = If(capacity = 0, New List(Of ToolStripDropDown), New List(Of ToolStripDropDown)(capacity))
            Return LayoutSuspendList
        End Function
        <CompilerServices.MethodImpl(AggrInlin)> Shared Sub ClearAdd2RangeList()
            If LayoutSuspendList IsNot Nothing Then
                LayoutSuspendList.Clear()
                LayoutSuspendList = Nothing
            End If
        End Sub

    End Class

    Public Class TextCustomMenu
        Shared Function EditMenu(value As String, defaults As String, owner As Form) As String
            Using dialog As New MacroEditorDialog
                dialog.SetMacroDefaults()
                dialog.MacroEditorControl.Value = value
                dialog.MacroEditorControl.rtbDefaults.Text = defaults
                dialog.Text = "Menu Editor"

                If defaults?.Length > 0 Then
                    dialog.bnContext.Text = " Restore Defaults... "
                    dialog.bnContext.Visible = True
                    dialog.bnContext.AddClickAction(Sub() If MsgOK("Restore defaults?") Then dialog.MacroEditorControl.Value = defaults)
                End If

                If dialog.ShowDialog(owner) = DialogResult.OK Then
                    value = dialog.MacroEditorControl.Value
                End If
            End Using

            Return value
        End Function

        Shared Function GetMenu(definition As String, owner As Control, components As IContainer, action As Action(Of String)) As ContextMenuStripEx
            If owner.ContextMenuStrip Is Nothing Then owner.ContextMenuStrip = New ContextMenuStripEx(components)

            Dim ret = DirectCast(owner.ContextMenuStrip, ContextMenuStripEx)
            ret.SuspendLayout()
            ret.Items.ClearAndDispose

            For Each i In definition.Split({BR}, StringSplitOptions.None)
                Dim iLen As Integer = i.Length
                If iLen > 2 Then 'Or >1 ? ToDO: Test This NoSense when >0 ,Empty TxtMenus/Actions
                    Dim idx As Integer = i.IndexOf("="c)
                    If idx > 0 AndAlso idx < iLen - 1 Then
                        ActionMenuItem.Add(ret.Items, i.Substring(0, idx).Trim, action, i.Substring(idx + 1).Trim, Nothing)
                    ElseIf idx < 0 AndAlso i(iLen - 1) = "-"c Then
                        ActionMenuItem.Add(ret.Items, i)
                    End If
                ElseIf iLen = 0 OrElse i(iLen - 1) = "-"c Then
                    ret.Items.Add(New ToolStripSeparator) 'ActionMenuItem.Add(ret.Items, i)
                End If
            Next i
            ret.ResumeLayout(False)
            Return ret
        End Function
    End Class

    Public Class ContextMenuStripEx
        Inherits ContextMenuStrip

        Private FormValue As Form

        Sub New()
        End Sub

        Sub New(container As IContainer)
            MyBase.New(container)
        End Sub

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)

            g.SetRenderer(Me)
            If s.UIScaleFactor <> 1.0F Then 'Experimental - NoScaling without this!!!
                Font = New Font("Segoe UI", 9 * s.UIScaleFactor) 'Seems like Segoe 9 is default
                ToolStripRendererEx.FontHeight = Font.Height
            End If
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean) 'Added by me !!! EXperim!!! Needed?
            FormValue = Nothing
            MyBase.Dispose(disposing)
            'Events.Dispose() 'Needed???
        End Sub

        <DefaultValue(GetType(Form), Nothing)>
        Property Form As Form
            Get
                Return FormValue
            End Get
            Set(value As Form)
                Dim di As EventHandler = Sub() 'experim
                                             RemoveHandler value.Disposed, di
                                             Dispose()
                                         End Sub
                AddHandler value.Disposed, di
                FormValue = value
            End Set
        End Property

        Function Add(path As String, action As Action) As ActionMenuItem
            Return Add(path, action, True)
        End Function

        Function Add(path As String, action As Action, enabled As Boolean) As ActionMenuItem
            Return Add(path, action, enabled, Nothing)
        End Function

        Function Add(path As String, action As Action, enabled As Boolean, help As String) As ActionMenuItem
            Return Add(path, action, Keys.None, enabled, Nothing, help)
        End Function

        Function Add(path As String, action As Action, key As Keys, enabled As Boolean, enabledFunc As Func(Of Boolean), Optional help As String = Nothing) As ActionMenuItem

            Dim ret = ActionMenuItem.Add(Items, path, action)

            If ret Is Nothing Then
                Exit Function
            End If

            ret.Form = Form
            ret.Shortcut = key
            ret.Enabled = enabled
            If help IsNot Nothing Then ret.Help = help

            If enabledFunc IsNot Nothing Then
                ret.EnabledFunc = enabledFunc
                AddHandler Opening, AddressOf ret.Opening ' add, problem with cms dispose opening event
            End If
            Return ret
        End Function
        'Function Add2(path As String, action As Action) As ActionMenuItem
        '    Dim ret = New ActionMenuItem(path, action)
        '    Items.Add(ret)
        '    Return ret
        'End Function
        Private Shared AddMenuList As List(Of ToolStripItem)

        Shared Function CreateAdd2RangeList(Optional capacity As Integer = 0) As List(Of ToolStripItem)
            AddMenuList = If(capacity = 0, New List(Of ToolStripItem), New List(Of ToolStripItem)(capacity))
            Return AddMenuList
        End Function
        Shared Sub ClearAdd2RangeList()
            If AddMenuList Is Nothing Then Exit Sub
            AddMenuList.Clear()
            AddMenuList = Nothing
        End Sub
        Shared Sub AddRangeList2Menu(itemsCol As ToolStripItemCollection)
            If AddMenuList IsNot Nothing Then
                itemsCol.AddRange(AddMenuList.ToArray)
                AddMenuList = Nothing
            End If
        End Sub

        Sub AddSeparator2RangeList()
            AddMenuList.Add(New ToolStripSeparator)
        End Sub
        Sub AddTStripMenuItem2List(tsmItem As ToolStripMenuItem)
            AddMenuList.Add(tsmItem)
        End Sub

        Function Add2RangeList(path As String, action As Action, image As Image, key As Keys) As ActionMenuItem
            Dim ret = New ActionMenuItem(path, action, image) With {.Form = Form, .Shortcut = key}
            AddMenuList.Add(ret)
            Return ret
        End Function
        Function Add2RangeList(path As String, action As Action, image As Image, key As Keys, enabledFunc As Func(Of Boolean)) As ActionMenuItem
            Dim ret = New ActionMenuItem(path, action, image) With {.Form = Form, .Shortcut = key, .EnabledFunc = enabledFunc}
            AddHandler Opening, AddressOf ret.Opening
            AddMenuList.Add(ret)
            Return ret
        End Function
        Function Add2RangeList(path As String, action As Action, key As Keys, Optional help As String = Nothing) As ActionMenuItem
            Dim ret = If(help Is Nothing, New ActionMenuItem(path, action) With {.Form = Form, .Shortcut = key}, New ActionMenuItem(path, action, help) With {.Form = Form, .Shortcut = key})
            AddMenuList.Add(ret)
            Return ret
        End Function
        Function Add2RangeList(path As String, action As Action, key As Keys, enabledFunc As Func(Of Boolean), Optional help As String = Nothing) As ActionMenuItem
            Dim ret = If(help Is Nothing, New ActionMenuItem(path, action) With {.Form = Form, .Shortcut = key, .EnabledFunc = enabledFunc}, New ActionMenuItem(path, action, help) With {.Form = Form, .Shortcut = key, .EnabledFunc = enabledFunc})
            AddHandler Opening, AddressOf ret.Opening
            AddMenuList.Add(ret)
            Return ret
        End Function
        Function GetTips() As StringPairList
            Dim ret As New StringPairList

            For Each i In GetItems.OfType(Of ActionMenuItem)()
                If i.Help?.Length > 0 Then
                    Dim pair As New StringPair

                    If i.Text.EndsWith("...", StringComparison.Ordinal) Then
                        pair.Name = i.Text.TrimEnd("."c)
                    Else
                        pair.Name = i.Text
                    End If

                    pair.Value = i.Help
                    ret.Add(pair)
                End If
            Next

            Return ret
        End Function

        Function GetKeys() As StringPairList
            Dim ret As New StringPairList

            For Each mi In GetItems.OfType(Of ActionMenuItem)()
                If mi.ShortcutKeyDisplayString?.Length > 0 Then
                    Dim sp As New StringPair

                    If mi.Text.EndsWith("...", StringComparison.Ordinal) Then
                        sp.Name = mi.Text.TrimEnd("."c)
                    Else
                        sp.Name = mi.Text
                    End If

                    sp.Value = mi.ShortcutKeyDisplayString
                    ret.Add(sp)
                End If
            Next

            Return ret
        End Function

        Function GetItems() As List(Of ToolStripItem)
            Dim ret As New List(Of ToolStripItem)
            AddItemsRecursive(Items, ret)
            Return ret
        End Function

        Shared Sub AddItemsRecursive(searchList As ToolStripItemCollection, returnList As List(Of ToolStripItem))
            For Each i As ToolStripItem In searchList
                returnList.Add(i)

                If TypeOf i Is ToolStripDropDownItem Then
                    AddItemsRecursive(DirectCast(i, ToolStripDropDownItem).DropDownItems, returnList)
                End If
            Next
        End Sub
    End Class
End Namespace
