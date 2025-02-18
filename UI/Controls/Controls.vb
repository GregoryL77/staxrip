﻿
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Drawing.Drawing2D
Imports JM.LinqFaster
Imports System.Drawing.Text

Namespace UI
    Public Class TreeViewEx
        Inherits TreeView

        Private AutoCollapsValue As Boolean

        <DefaultValue(False)>
        Property AutoCollaps As Boolean

        <DefaultValue(GetType(TreeNodeExpandMode), "Disabled")>
        Property ExpandMode As TreeNodeExpandMode

        <DefaultValue(False)>
        Property SelectOnMouseDown() As Boolean

        Protected Overrides Sub OnAfterSelect(e As TreeViewEventArgs)
            BeginUpdate()
            If AutoCollaps Then
                For Each i As TreeNode In If(e.Node.Parent Is Nothing, Nodes, e.Node.Parent.Nodes)
                    If i IsNot e.Node Then
                        i.Collapse()
                    End If
                Next
            End If

            Select Case ExpandMode
                Case TreeNodeExpandMode.Normal
                    e.Node.Expand()
                Case TreeNodeExpandMode.InclusiveChilds
                    e.Node.ExpandAll()
            End Select

            MyBase.OnAfterSelect(e)
            EndUpdate()
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            If SelectOnMouseDown AndAlso m.Msg = &H201 Then 'WM_LBUTTONDOWN
                Dim n = GetNodeAt(ClientMousePos)

                If n IsNot Nothing AndAlso n.Nodes.Count = 0 Then
                    SelectedNode = n
                    Focus()
                    Exit Sub
                End If
            End If

            MyBase.WndProc(m)
        End Sub

        Sub MoveSelectionLeft()
            Dim n As TreeNode = SelectedNode

            If n IsNot Nothing AndAlso n.Parent IsNot Nothing Then
                Dim parentParentNodes As TreeNodeCollection = GetParentParentNodes(n)
                Dim parentIndex As Integer = n.Parent.Index

                If Not parentParentNodes Is Nothing Then
                    n.Remove()
                    parentParentNodes.Insert(parentIndex + 1, n)
                    SelectedNode = n
                End If
            End If
        End Sub

        Sub MoveSelectionUp()
            Dim n As TreeNode = SelectedNode

            If Not n Is Nothing Then
                If n.Index = 0 Then
                    Dim parentParentNodes As TreeNodeCollection = GetParentParentNodes(n)

                    If parentParentNodes IsNot Nothing Then
                        Dim index As Integer = n.Parent.Index
                        n.Remove()
                        parentParentNodes.Insert(index, n)
                    End If
                Else
                    Dim index As Integer = n.Index
                    Dim parentNodes As TreeNodeCollection = GetParentNodes(n)
                    n.Remove()
                    parentNodes.Insert(index - 1, n)
                End If

                SelectedNode = n
            End If
        End Sub

        Sub MoveSelectionRight()
            Dim n As TreeNode = SelectedNode

            If n IsNot Nothing Then
                If n.Index > 0 Then
                    Dim previousNode As TreeNode = n.PrevNode
                    n.Remove()
                    previousNode.Nodes.Add(n)
                    SelectedNode = n
                End If
            End If
        End Sub

        Sub MoveSelectionDown()
            Dim n As TreeNode = SelectedNode

            If n IsNot Nothing Then
                If n.NextNode Is Nothing Then
                    Dim parentParentNodes As TreeNodeCollection = GetParentParentNodes(n)

                    If parentParentNodes IsNot Nothing Then
                        Dim index As Integer = n.Parent.Index
                        n.Remove()
                        parentParentNodes.Insert(index + 1, n)
                    End If
                Else
                    Dim index As Integer = n.Index
                    Dim parentNodes As TreeNodeCollection = GetParentNodes(n)
                    n.Remove()
                    parentNodes.Insert(index + 1, n)
                End If

                SelectedNode = n
            End If
        End Sub

        Function GetParentParentNodes(n As TreeNode) As TreeNodeCollection
            Dim parent = n.Parent

            If parent Is Nothing Then
                Return Nothing
            End If

            If parent.Parent Is Nothing Then
                Return Nodes
            Else
                Return parent.Parent.Nodes
            End If
        End Function

        Function GetParentNodes(n As TreeNode) As TreeNodeCollection
            Dim parent As TreeNode = n.Parent

            If parent Is Nothing Then
                Return Nodes
            Else
                Return parent.Nodes
            End If
        End Function

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            Native.SetWindowTheme(Handle, "explorer", Nothing)
        End Sub

        Function AddNode(path As String) As TreeNode
            Dim pathElements = path.SplitNoEmptyAndWhiteSpace("|"c)
            Dim currentNodeList = Nodes
            'Dim currentPath As String
            Dim ret As TreeNode

            For Each iNodeName In pathElements
                'If currentPath?.Length > 0 Then currentPath &= "|" 'Seems to be Dead???
                'currentPath &= iNodeName

                Dim found = False
                Dim iNN As String = " " & iNodeName

                For Each iNode As TreeNode In currentNodeList
                    If String.Equals(iNode.Text, iNN) Then
                        ret = iNode
                        currentNodeList = iNode.Nodes
                        found = True
                        Exit For 'Added OK?!
                    End If
                Next iNode

                If Not found Then
                    ret = New TreeNode(iNN)
                    currentNodeList.Add(ret)
                    currentNodeList = ret.Nodes
                End If
            Next iNodeName

            Return ret
        End Function

        Function GetNodes() As List(Of TreeNode)
            Dim ret As New List(Of TreeNode)(32) '220 in Packages
            AddNodesRecursive(Nodes, ret)
            Return ret
        End Function

        Shared Sub AddNodesRecursive(searchList As TreeNodeCollection, returnList As List(Of TreeNode))
            For Each i As TreeNode In searchList
                returnList.Add(i)
                AddNodesRecursive(i.Nodes, returnList)
            Next
        End Sub
    End Class

    Public Enum TreeNodeExpandMode
        Disabled
        Normal
        InclusiveChilds
    End Enum

    Public Class ToolStripEx
        Inherits ToolStrip

        Sub New()
            MyBase.New()
        End Sub

        Sub New(ParamArray items As ToolStripItem())
            MyBase.New(items)
        End Sub

        <DefaultValue(False), Description("Overdraws a weird grey line visible at the bottom in some configurations.")>
        Property OverdrawLine As Boolean

        <DefaultValue(False), Description("Only one button can be checked at the same time.")>
        Property SingleAutoChecked As Boolean

        <DefaultValue(False), Description("Shows a themed control border.")>
        Property ShowControlBorder As Boolean

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            If OverdrawLine Then
                Dim b As New SolidBrush(BackColor)
                e.Graphics.FillRectangle(b, 0, Height - 2, Width, 2)
                b.Dispose()
            End If

            If ShowControlBorder Then 'AndAlso VisualStyleInformation.IsEnabledByUser ' Windows 10 Assume Visual stryles ON
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, VisualStyleInformation.TextControlBorder, ButtonBorderStyle.Solid)
            End If
        End Sub

        Protected Overrides Sub OnItemClicked(e As ToolStripItemClickedEventArgs)
            If SingleAutoChecked Then
                For Each i In Items.OfType(Of ToolStripButton)()
                    If i Is e.ClickedItem Then
                        i.Checked = True
                    Else
                        i.Checked = False
                    End If
                Next
            End If

            MyBase.OnItemClicked(e)
        End Sub

        'Protected Overrides ReadOnly Property CreateParams() As CreateParams 'Windows 10 Assume Visual stryles ON
        '    Get
        '        Dim ret = MyBase.CreateParams
        '        'If ShowControlBorder AndAlso Not VisualStyleInformation.IsEnabledByUser Then 'Windows 10 Assume Visual stryles ON
        '        '    ret.ExStyle = ret.ExStyle Or &H200 'WS_EX_CLIENTEDGE
        '        'End If
        '        Return ret
        '    End Get
        'End Property
    End Class

    Public Class LineControl
        Inherits Control

        Sub New()
            Margin = New Padding(4, 2, 5, 2)
            Anchor = AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            Dim textOffset As Integer
            Dim lineHeight = CInt(Height / 2)

            If Text?.Length > 0 Then
                Dim textSize = e.Graphics.MeasureString(Text, Font)
                textOffset = CInt(textSize.Width)

                Using brush = New SolidBrush(If(Enabled, ForeColor, SystemColors.GrayText))
                    e.Graphics.DrawString(Text, Font, brush, 0, CInt((Height - textSize.Height) / 2) - 1)
                End Using
            End If

            If Enabled Then
                e.Graphics.DrawLine(Pens.Silver, textOffset, lineHeight, Width, lineHeight)
                e.Graphics.DrawLine(Pens.White, textOffset, lineHeight + 1, Width, lineHeight + 1)
            Else
                e.Graphics.DrawLine(SystemPens.InactiveBorder, textOffset, lineHeight, Width, lineHeight)
            End If
        End Sub
    End Class

    Public Class CommandLink
        Inherits Button

        Const BS_COMMANDLINK As Integer = &HE

        Const BCM_SETNOTE As Integer = &H1609

        Sub New()
            FlatStyle = FlatStyle.System
        End Sub

        Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
            Get
                Dim params = MyBase.CreateParams
                params.Style = params.Style Or BS_COMMANDLINK
                Return params
            End Get
        End Property

        Private NoteValue As String = ""

        <DefaultValue("")>
        Property Note() As String
            Get
                Return NoteValue
            End Get
            Set(value As String)
                Native.SendMessage(Handle, BCM_SETNOTE, 0, value)
                NoteValue = value
            End Set
        End Property

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            If Note?.Length > 0 Then
                Text &= BR2 & Note
            End If
        End Sub
    End Class

    Public Class TextBoxEx
        Inherits TextBox

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property

        Sub SetTextWithoutTextChangedEvent(text As String)
            BlockOnTextChanged = True
            Me.Text = text
            BlockOnTextChanged = False
        End Sub

        Private BlockOnTextChanged As Boolean

        Protected Overrides Sub OnTextChanged(e As EventArgs)
            If Not BlockOnTextChanged Then
                MyBase.OnTextChanged(e)
            End If
        End Sub
    End Class

    Public Class PanelEx
        Inherits Panel

        Private ShowNiceBorderValue As Boolean

        Sub New()
            SetStyle(ControlStyles.ResizeRedraw, True)
        End Sub

        <DefaultValue(False), Description("Nicer border if themes are enabled.")>
        Property ShowNiceBorder() As Boolean
            Get
                Return ShowNiceBorderValue
            End Get
            Set(Value As Boolean)
                ShowNiceBorderValue = Value
                Invalidate()
            End Set
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            If ShowNiceBorder Then 'AndAlso VisualStyleInformation.IsEnabledByUser 'Windows 10 Assume Visual stryles ON
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, VisualStyleInformation.TextControlBorder, ButtonBorderStyle.Solid)
            End If
        End Sub
    End Class

    Public Class CheckBoxEx
        Inherits CheckBox

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property
    End Class

    Public Class RichTextBoxEx
        Inherits RichTextBox

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property BlockPaint As Boolean

        Private BorderRect As Native.RECT

        Sub New()
            'MyClass.New(True)
            'If VisualStyleInformation.IsEnabledByUser Then 'Windows 10 Assume Visual stryles ON
            BorderStyle = BorderStyle.None
            'End If
            InitMenu()
        End Sub

        Sub New(createMenu As Boolean) 'Override-No InitMenu  Workaround
            'If createMenu Then
            '    InitMenu()
            'End If
        End Sub

        Sub InitMenu()
            If DesignHelp.IsDesignMode Then
                Exit Sub
            End If

            Dim cms As New ContextMenuStripEx()

            cms.SuspendLayout()
            Dim cutItem = New ActionMenuItem("Cut", ImageHelp.GetImageC(Symbol.Cut)) With {.KeyDisplayString = "Ctrl+X"}
            Dim copyItem = New ActionMenuItem("Copy", Sub() Clipboard.SetText(SelectedText), ImageHelp.GetImageC(Symbol.Copy)) With {.KeyDisplayString = "Ctrl+C"}
            Dim pasteItem = New ActionMenuItem("Paste", ImageHelp.GetImageC(Symbol.Paste)) With {.KeyDisplayString = "Ctrl+V"}
            cms.Items.AddRange({cutItem, copyItem, pasteItem, New ActionMenuItem("Copy Everything", Sub() If Text.Length > 0 Then Clipboard.SetText(Text))})
            cms.ResumeLayout(False)

            AddHandler cutItem.Click, Sub()
                                          Clipboard.SetText(SelectedText)
                                          SelectedText = ""
                                      End Sub

            AddHandler pasteItem.Click, Sub()
                                            SelectedText = Clipboard.GetText
                                            ScrollToCaret()
                                        End Sub

            AddHandler cms.Opening, Sub()
                                        Dim isSel As Boolean = SelectionLength > 0
                                        Dim ro As Boolean = Not Me.ReadOnly
                                        cutItem.Visible = ro AndAlso isSel
                                        copyItem.Visible = isSel
                                        pasteItem.Visible = ro AndAlso Clipboard.ContainsText
                                    End Sub

            ContextMenuStrip = cms
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            ContextMenuStrip?.Dispose()
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
            If e.KeyData = (Keys.Control Or Keys.V) Then
                e.SuppressKeyPress = True
                SelectedText = Clipboard.GetText
            End If

            MyBase.OnKeyDown(e)
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            Const WM_NCPAINT = &H85
            Const WM_NCCALCSIZE = &H83
            Const WM_THEMECHANGED = &H31A

            If BlockPaint Then
                Select Case m.Msg
                    Case 15, 20 'WM_PAINT, WM_ERASEBKGND
                        Exit Sub
                    Case Else
                End Select
            End If

            MyBase.WndProc(m)

            Select Case m.Msg
                Case WM_NCPAINT
                    WmNcpaint(m)
                Case WM_NCCALCSIZE
                    WmNccalcsize(m)
                Case WM_THEMECHANGED
                    UpdateStyles()
            End Select
        End Sub

        Sub WmNccalcsize(ByRef m As Message)
            'If Not VisualStyleInformation.IsEnabledByUser Then 'Windows 10 Assume Visual stryles ON
            'Return
            'End If

            Dim par As New Native.NCCALCSIZE_PARAMS()
            Dim windowRect As Native.RECT

            If m.WParam <> IntPtr.Zero Then
                par = CType(Marshal.PtrToStructure(m.LParam, GetType(Native.NCCALCSIZE_PARAMS)), Native.NCCALCSIZE_PARAMS)
                windowRect = par.rgrc0
            End If

            Dim clientRect = windowRect

            clientRect.Left += 1
            clientRect.Top += 1
            clientRect.Right -= 1
            clientRect.Bottom -= 1

            BorderRect = New Native.RECT(clientRect.Left - windowRect.Left,
                                         clientRect.Top - windowRect.Top,
                                         windowRect.Right - clientRect.Right,
                                         windowRect.Bottom - clientRect.Bottom)

            If m.WParam = IntPtr.Zero Then
                Marshal.StructureToPtr(clientRect, m.LParam, False)
            Else
                par.rgrc0 = clientRect
                Marshal.StructureToPtr(par, m.LParam, False)
            End If

            Const WVR_HREDRAW = &H100S
            Const WVR_VREDRAW = &H200S
            Const WVR_REDRAW As Integer = (WVR_HREDRAW Or WVR_VREDRAW)

            m.Result = New IntPtr(WVR_REDRAW)
        End Sub

        Sub WmNcpaint(ByRef m As Message)
            'If Not VisualStyleInformation.IsEnabledByUser Then 'Windows 10 Assume Visual stryles ON
            'Return
            'End If

            Dim rect As Native.RECT
            Native.GetWindowRect(Handle, rect)

            rect.Right -= rect.Left
            rect.Bottom -= rect.Top
            rect.Top = 0
            rect.Left = 0

            rect.Left += BorderRect.Left
            rect.Top += BorderRect.Top
            rect.Right -= BorderRect.Right
            rect.Bottom -= BorderRect.Bottom

            Dim hDC = Native.GetWindowDC(Handle)
            Native.ExcludeClipRect(hDC, rect.Left, rect.Top, rect.Right, rect.Bottom)

            Using g = Graphics.FromHdc(hDC)
                g.Clear(Color.CadetBlue)
            End Using

            Native.ReleaseDC(Handle, hDC)
            m.Result = IntPtr.Zero
        End Sub

        'Protected Overrides Sub OnHandleCreated(e As EventArgs)
        '    MyBase.OnHandleCreated(e)
        '    'AutoWordSelection = False 'False by Default!
        'End Sub
    End Class

    Public Class TrackBarEx
        Inherits TrackBar

        <DefaultValue(False)>
        Property NoMouseWheelEvent As Boolean

        Shadows Property Value As Integer
            Get
                Return MyBase.Value
            End Get
            Set(value As Integer)
                If value > Maximum Then value = Maximum
                If value < Minimum Then value = Minimum
                If value <> MyBase.Value Then MyBase.Value = value
            End Set
        End Property
    End Class

    Public Class LabelEx
        Inherits Label

        Sub New()
            TextAlign = Drawing.ContentAlignment.MiddleLeft
        End Sub

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property
    End Class

    Public Class PropertyGridEx
        Inherits PropertyGrid

        Private Description As String

        Sub New()
            ToolbarVisible = False 'GridView is not ready when PropertySortChanged happens so Init fails
        End Sub

        Protected Overrides Sub OnSelectedGridItemChanged(e As SelectedGridItemChangedEventArgs)
            MyBase.OnSelectedGridItemChanged(e)
            Description = e.NewSelection.PropertyDescriptor.Description
            SetHelpHeight()
        End Sub

        Protected Overrides Sub OnLayout(e As LayoutEventArgs)
            SetHelpHeight()
            MyBase.OnLayout(e)
        End Sub

        Sub SetHelpHeight()
            If Description?.Length > 0 Then
                HelpVisible = True

                Using g = CreateGraphics()
                    Dim lines = CInt(Math.Ceiling(g.MeasureString(Description, Font, Width).Height / FontHeight)) 'font.height Test This Experiment!!! NoScaling

                    Dim grid As New Reflector(Me, GetType(PropertyGrid))
                    Dim doc = grid.Invoke("doccomment")
                    doc.Invoke("Lines", lines + 1)
                    doc.Invoke("userSized", True)
                    grid.Invoke("OnLayoutInternal", False)
                End Using

            Else
                HelpVisible = False
            End If
        End Sub

        Sub MoveSplitter(x As Integer)
            Dim grid As New Reflector(Me, GetType(PropertyGrid))
            grid.Invoke("gridView").Invoke("MoveSplitterTo", x)
        End Sub
    End Class

    Public Class ButtonLabel
        Inherits Label

        Private LinkColorNormal As Color
        Private LinkColorHover As Color

        Property LinkColor As Color
            Get
                Return LinkColorNormal
            End Get
            Set(value As Color)
                LinkColorNormal = value
                LinkColorHover = ControlPaint.Dark(LinkColorNormal)
                ForeColor = value
            End Set
        End Property

        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            SetFontStyle(FontStyle.Bold)
            ForeColor = LinkColorHover
            MyBase.OnMouseEnter(e)
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            SetFontStyle(FontStyle.Regular)
            ForeColor = LinkColorNormal
            MyBase.OnMouseLeave(e)
        End Sub
    End Class

    <DefaultEvent("LinkClick")>
    Public Class LinkGroupBox
        Inherits GroupBox

        Public WithEvents Label As New ButtonLabel
        Event LinkClick()

        Sub New()
            Label.Left = 4
            Label.AutoSize = True
            Controls.Add(Label)
        End Sub

        Property Color As Color

        Overrides Property Text() As String
            Get
                Return Label.Text
            End Get
            Set(value As String)
                If Not value.EndsWith(" ", StringComparison.Ordinal) Then
                    value &= " "
                End If

                Label.Text = value
            End Set
        End Property

        Sub Label_Click() Handles Label.Click
            ShowContext()
            RaiseEvent LinkClick()
        End Sub

        Sub ShowContext()
            If Not Label.ContextMenuStrip Is Nothing Then Label.ContextMenuStrip.Show(Label, 0, 16)
        End Sub
    End Class

    Public Class MenuButton
        Inherits ButtonEx

        Event ValueChangedUser(value As Object)

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property Items As New List(Of Object)
        Property Menu As ContextMenuStripEx

        Sub New()
            Menu = New ContextMenuStripEx With {.ShowImageMargin = False}
            ShowMenuSymbol = True
            AddHandler Menu.Opening, AddressOf MenuOpening
        End Sub

        Sub MenuOpening(sender As Object, e As CancelEventArgs)
            Menu.MinimumSize = New Size(Width, 0)

            'For Each mi As ActionMenuItem In Menu.Items
            For Each mi As ToolStripMenuItem In Menu.Items ' Test this !!!
                mi.Font = New Font(Me.Font, If(mi.Tag IsNot Nothing AndAlso Value?.Equals(mi.Tag), FontStyle.Bold, FontStyle.Regular))

                Dim mW As Integer = Menu.Width
                If (mW - mi.Width) > 2 Then
                    mi.AutoSize = False
                    mi.Width = mW - 1
                End If
            Next
        End Sub

        Private ValueValue As Object

        <DefaultValue(CStr(Nothing))>
        Property Value As Object
            Get
                Return ValueValue
            End Get
            Set(value As Object)
                If Menu.Items.Count = 0 Then

                    If TypeOf value Is System.Enum Then
                        Menu.SuspendLayout()
                        Dim inc As Integer
                        Dim enAr As Array = System.Enum.GetValues(value.GetType)
                        Dim amiAr(enAr.Length - 1) As ActionMenuItem
                        For Each i In enAr
                            Dim eObj = i
                            Dim text = DispNameAttribute.GetValueForEnum(eObj)
                            amiAr(inc) = New ActionMenuItem(text, Sub() OnAction(text, eObj), eObj) 'Test This for Enums
                            inc += 1
                        Next i
                        Menu.Items.AddRange(amiAr)
                        Menu.ResumeLayout(False) 'Test This !!!
                    End If
                End If

                If value IsNot Nothing Then
                    For Each i In Menu.Items.OfType(Of ActionMenuItem)()
                        If value.Equals(i.Tag) Then
                            Text = i.Text
                            Exit For
                        End If

                        If i.DropDownItems.Count > 0 Then
                            For Each i2 In i.DropDownItems.OfType(Of ActionMenuItem)()
                                If value.Equals(i2.Tag) Then
                                    Text = i2.Text
                                    Exit For
                                End If
                            Next i2
                        End If
                    Next i

                    If Text.NullOrEmptyS Then Text = value.ToString
                End If

                ValueValue = value
            End Set
        End Property

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            If e.Button = MouseButtons.Left Then
                Menu.Show(Me, 0, Height)
            Else
                MyBase.OnMouseDown(e)
            End If
        End Sub

        Protected Overridable Sub OnValueChanged(value As Object)
            RaiseEvent ValueChangedUser(value)
        End Sub

        Sub Add(items As IEnumerable(Of Object))
            Menu.SuspendLayout()
            ActionMenuItem.LayoutSuspendCreate()
            For Each i In items
                Add(i.ToString, i, Nothing)
            Next
            ActionMenuItem.LayoutResume()
            Menu.ResumeLayout()
        End Sub

        Sub Add(ParamArray items As Object())
            Menu.SuspendLayout()
            ActionMenuItem.LayoutSuspendCreate()
            For Each i In items
                Add(i.ToString, i, Nothing)
            Next
            ActionMenuItem.LayoutResume()
            Menu.ResumeLayout()
        End Sub

        Function Add(path As String, obj As Object, Optional tip As String = Nothing) As ActionMenuItem
            Items.Add(obj)
            Dim name = path
            'If path.Contains("|") Then name = path.RightLast("|").Trim
            'Dim rp = path.RightLast("|")
            'If rp.Length > 0 Then name = rp.Trim
            Dim idx As Integer = path.LastIndexOf("|"c)
            If idx >= 0 Then name = path.Substring(idx + 1).Trim

            Dim ret = ActionMenuItem.Add(Menu.Items, path, Sub(o As Object) OnAction(name, o), obj, tip)
            ret.Tag = obj
            Return ret
        End Function

        Sub AddRange(ParamArray items As Object())
            Dim retA(items.Length - 1) As ActionMenuItem
            For t = 0 To retA.Length - 1
                Dim ob = items(t)
                Me.Items.Add(ob) 'Needed?
                Dim p = ob.ToString
                retA(t) = New ActionMenuItem(p, Sub() OnAction(p, ob), ob)
            Next t

            Menu.SuspendLayout()
            Menu.Items.AddRange(retA)
            Menu.ResumeLayout(False)
        End Sub

        Function Add2(path As String, obj As Object) As ActionMenuItem ', Optional tip As String = Nothing Test it Debug ???
            'Items.Add(obj)
            ' If LayoutSuspendHS IsNot Nothing AndAlso LayoutSuspendHS.Add(Menu) Then Menu.SuspendLayout()
            Dim ret = New ActionMenuItem(path, Sub() OnAction(path, obj), obj) ' ,tip
            Menu.Items.Add(ret)
            Return ret
        End Function

        Sub AddRange2(menuTup As (path As String, obj As Object)()) 'As ActionMenuItem() 'Test it Debug, Still overhead Meh! ???
            Dim retA(menuTup.Length - 1) As ActionMenuItem
            For t = 0 To menuTup.Length - 1
                Dim ob = menuTup(t).obj
                'Items.Add(ob)
                Dim p = menuTup(t).path
                retA(t) = New ActionMenuItem(p, Sub() OnAction(p, ob), ob)
            Next t

            Menu.SuspendLayout()
            Menu.Items.AddRange(retA)
            Menu.ResumeLayout(False)
        End Sub

        Sub Clear()
            Items.Clear()
            Menu.Items.ClearAndDispose
        End Sub

        Sub OnAction(text As String, value As Object)
            Me.Text = text
            Me.Value = value
            OnValueChanged(value)
        End Sub

        Function GetValue(Of T)() As T
            Return DirectCast(Value, T)
        End Function

        Function GetInt() As Integer
            Return DirectCast(Value, Integer)
        End Function

        Public Sub BuildLangMenu()
            Dim mL1 As New List(Of ToolStripMenuItemEx)(18) '18
            Dim mL2Alph As New List(Of ToolStripMenuItemEx)(26) '26
            Dim mL3LL As New List(Of List(Of ActionMenuItem))(26)
            Dim invCultTI As Globalization.TextInfo = InvCultTxtInf
            Dim sb As New Text.StringBuilder(52) 'Maxis 50
            Dim languagesA = Language.Languages  '302

            For i = 0 To languagesA.Length - 1
                Dim lng As Language = languagesA(i)
                sb.Length = 0
                Dim lngS As String = lng.CultureInfo.EnglishName '.ToString
                sb.Append(lngS).Append(" (").Append(lng.CultureInfo.TwoLetterISOLanguageName).Append(", ").Append(lng.ThreeLetterCode).Append(")")
                Dim sbS As String = sb.ToString

                If lng.IsCommon Then
                    mL1.Add(New ActionMenuItem(sbS, Sub() OnAction(sbS, lng), lng)) 'Tag for Bold Highlight Selection
                Else
                    Dim nML3 As List(Of ActionMenuItem)
                    Dim lastCh As Char
                    Dim c1 As Char = invCultTI.ToUpper(lngS.Chars(0))
                    If c1 <> lastCh Then 'Arr must be sorted IgnCase!
                        lastCh = c1
                        mL2Alph.Add(New ToolStripMenuItemEx(c1))
                        nML3 = New List(Of ActionMenuItem)(8)
                        mL3LL.Add(nML3)
                    End If
                    nML3.Add(New ActionMenuItem(sbS, Sub() OnAction(sbS, lng)))
                End If
            Next i

            Dim miMore = New ToolStripMenuItemEx("More")
            mL1.Add(miMore)
            miMore.DropDown.SuspendLayout()
            Menu.SuspendLayout()
            Menu.Items.AddRange(mL1.ToArray)
            miMore.DropDownItems.AddRange(mL2Alph.ToArray)

            For i = 0 To mL2Alph.Count - 1
                Dim nMI As ToolStripMenuItemEx = mL2Alph(i)
                nMI.DropDown.SuspendLayout()
                nMI.DropDownItems.AddRange(mL3LL(i).ToArray)
                nMI.DropDown.ResumeLayout(False)
            Next i

            miMore.DropDown.ResumeLayout(False)
            Menu.ResumeLayout(False)
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            Menu.Dispose()
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Public Class CommandLineRichTextBox
        Inherits RichTextBoxEx

        Private FontBold As Font

        Public Sub New()
            MyBase.New(False)
            Dim tFont As New Font("Consolas", 10 * s.UIScaleFactor)
            Font = tFont
            FontBold = New Font(tFont, FontStyle.Bold)
        End Sub

        Sub SetText(cmdTxt As String, selectionsT As Task(Of Integer()))
            'If cmdTxt.NullOrEmptyS Then
            '    Clear()
            '    Exit Sub
            'End If

            Dim sel = selectionsT?.Result
            If sel Is Nothing Then Exit Sub

            BlockPaint = True
            Text = cmdTxt
            If sel.Length = 2 Then
                SelectionStart = sel(0)
                SelectionLength = sel(1)
                SelectionFont = FontBold
                'SelectionStart = cmdTxt.Length
            End If
            'Refresh() TestThis here - Blank RTB ????
            BlockPaint = False

            'Refresh()
            Invalidate()
        End Sub

        Function GetSelections(cmdTxt As Char(), lastTxt As Char()) As Integer() 'Returns: {SelStart,SelEnd}
            'If cmdTxt.NullOrEmptyS Then Return Nothing
            '''If String.Equals(cmdTxt, lastTxt) Then Return Nothing' Moved to callers
            Dim lTxtL As Integer = lastTxt.Length
            If lTxtL > 0 Then
                Dim cTxtL As Integer = cmdTxt.Length
                Dim selStart = GetCompareIndex(cmdTxt, lastTxt, cTxtL - 1, lTxtL - 1)
                Dim selEnd = cTxtL - GetCompareIndex(ReverseString(cmdTxt), ReverseString(lastTxt), cTxtL - 1, lTxtL - 1)

                If selEnd > selStart AndAlso selEnd - selStart < cTxtL - 1 Then
                    While selStart > 0 AndAlso selStart + 1 < cTxtL AndAlso (cmdTxt(selStart) <> "-"c OrElse cmdTxt(selStart - 1) <> " "c)
                        'AndAlso InvCompInf.IndexOf(cmdTxt, " -", selStart - 1, 2, CompareOptions.Ordinal) < 0 'AndAlso  Not String.Equals(cmdTxt.Substring(selStart - 1, 2), " -")
                        selStart -= 1
                    End While

                    If selEnd - selStart < 35 Then 'maybe more than 25[org]???
                        'Dim ret(1) As Integer
                        'ret(0) = selStart
                        'ret(1) = If(selEnd - selStart = cTxtL, 0, selEnd - selStart)
                        Return {selStart, If(selEnd - selStart = cTxtL, 0, selEnd - selStart)} 'ret
                    End If
                End If
            End If

            Return {}
        End Function

        <Runtime.CompilerServices.MethodImpl(AggrInlin)>
        Function GetCompareIndex(a As Char(), b As Char(), aLen As Integer, bLen As Integer) As Integer 'String.Len -1
            'Dim aLen As Integer = a.Length - 1
            'Dim bLen As Integer = b.Length - 1
            For x = 0 To aLen
                If x > bLen OrElse x > aLen OrElse a(x) <> b(x) Then Return x
            Next x
            Return 0
        End Function

        <Runtime.CompilerServices.MethodImpl(AggrInlin)>
        Function ReverseString(value As Char()) As Char()
            'Dim chars = value.ToCharArray
            Array.Reverse(value)
            Return value 'New String(chars)
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean) 'Needed???
            If FontBold IsNot Nothing Then
                FontBold?.Dispose() 'Test This Dispose !!!! or remove it ALL
                FontBold = Nothing
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class

    <ProvideProperty("Expand", GetType(Control))>
    Public Class FlowLayoutPanelEx
        Inherits FlowLayoutPanel

        <DefaultValue(False)>
        Property UseParenWidth As Boolean

        Property AutomaticOffset As Boolean

        Sub New()
            WrapContents = False
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            MyBase.OnLayout(levent)
            Dim ctrlAr As Control() = Controls.OfType(Of Control).ToArray
            If ctrlAr.Length <= 0 Then Exit Sub
            SuspendLayout() 'ToDo: Test this

            If Not WrapContents Then
                Dim flowDirLtR As Boolean = FlowDirection = FlowDirection.LeftToRight
                If flowDirLtR Then
                    For i = 0 To ctrlAr.Length - 1 'each: ctrl As Control In Controls
                        Dim nextPos As Integer
                        Dim ctrl = ctrlAr(i)
                        If ctrl.Visible Then
                            nextPos += ctrl.Margin.Left + ctrl.Width + ctrl.Margin.Right

                            Dim expandetControl = TryCast(ctrl, SimpleUI.SimpleUIControl)
                            If expandetControl IsNot Nothing AndAlso expandetControl.Expand Then
                                'Dim diff = Aggregate i2 In Controls.OfType(Of Control)() Into Sum(If(i2.Visible, i2.Width + i2.Margin.Left + i2.Margin.Right, 0))
                                'Dim diff = ctrlAr.SumF(Function(i2) If(i2.Visible, i2.Width + i2.Margin.Left + i2.Margin.Right, 0))
                                Dim diff As Integer = 0
                                For c = 0 To ctrlAr.Length - 1
                                    Dim cS = ctrlAr(c)
                                    diff += If(cS.Visible, cS.Width + cS.Margin.Left + cS.Margin.Right, 0)
                                Next c

                                Dim hostWidth = Width - 1

                                If ctrl.AutoSize Then
                                    nextPos += hostWidth - diff
                                Else
                                    ctrl.Width += hostWidth - diff
                                    nextPos += hostWidth - diff
                                End If
                            End If
                        End If

                        If i < ctrlAr.Length - 1 Then 'Controls.IndexOf(ctrl) = i
                            ctrlAr(i + 1).Left = nextPos
                        End If
                    Next i
                End If

                'vertical align middles
                If flowDirLtR OrElse FlowDirection = FlowDirection.RightToLeft Then
                    For i = 0 To ctrlAr.Length - 1 'In Controls
                        Dim ctrl As Control = ctrlAr(i)
                        ctrl.Top = CInt((Height - ctrl.Height) / 2) + If(TypeOf ctrl Is CheckBox, 1, 0)
                    Next i
                End If
            End If
            'Dim labelBl0A = ctrlAr.OfType(Of SimpleUI.LabelBlock).Where(Function(block) block.Label.Offset = 0).ToArray
            'If labelBl0A.Length > 0 Then
            '    'Dim hMax = Aggregate i In labelBlocks Into Max(TextRenderer.MeasureText(i.Label.Text, i.Label.Font).Width)
            '    Dim hMax = labelBl0A.MaxF(Function(i) TextRenderer.MeasureText(i.Label.Text, i.Label.Font).Width)
            '    For Each lb In labelBl0A
            '        lb.Label.Offset = hMax / lb.Label.Font.Height
            '    Next
            'End If
            Dim lb0List As List(Of SimpleUI.LabelBlock)
            Dim lFontH As Integer
            Dim lb0WMax As Integer
            For i = 0 To ctrlAr.Length - 1
                Dim ctrl = ctrlAr(i)
                'If TypeOf ctrl Is SimpleUI.LabelBlock Then
                'Dim lb = DirectCast (ctrl, SimpleUI.LabelBlock)
                Dim lb = TryCast(ctrl, SimpleUI.LabelBlock)
                If lb IsNot Nothing Then
                    If lb.Label.Offset = 0 Then
                        Dim lF As Font
                        Dim init As Boolean
                        If Not init Then
                            init = True
                            lb0List = New List(Of SimpleUI.LabelBlock)(4)
                            lF = lb.Label.Font
                            lFontH = lF.Height
                        End If
                        Dim mLW = TextRenderer.MeasureText(lb.Label.Text, lF).Width
                        If mLW > lb0WMax Then lb0WMax = mLW
                        lb0List.Add(lb)
                    End If
                End If
            Next i

            If lFontH > 0 Then
                For i = 0 To lb0List.Count - 1
                    lb0List(i).Label.Offset = lb0WMax / lFontH
                Next i
            End If

            'If lb0List?.Count > LLLLMaxCount Then LLLLMaxCount = lb0List.Count 'debug

            ResumeLayout(False) '??? Not False ???
        End Sub

        'Public Shared LLLLMaxCount As Integer  ' Debug

        Protected Overrides ReadOnly Property DefaultMargin As Padding
            Get
                Return Padding.Empty
            End Get
        End Property

        Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
            Dim ret = MyBase.GetPreferredSize(proposedSize)

            If UseParenWidth Then
                ret.Width = Parent.Width
            End If

            Return ret
        End Function
    End Class

    Public Class ButtonEx
        Inherits Button

        <DefaultValue(ButtonSymbol.None)>
        Property Symbol As ButtonSymbol

        <DefaultValue(False)>
        Property ShowMenuSymbol As Boolean

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property ClickAction As Action

        Sub New()
            MyBase.UseVisualStyleBackColor = True
        End Sub

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DefaultValue(0), Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property

        <DefaultValue(True), Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property UseVisualStyleBackColor() As Boolean
            Get
                Return True
            End Get
            Set(value As Boolean)
            End Set
        End Property

        Protected Overrides ReadOnly Property DefaultSize As Size
            Get
                Return New Size(250, 70)
            End Get
        End Property

        Protected Overrides Sub OnClick(e As EventArgs)
            ClickAction?.Invoke()
            ContextMenuStrip?.Show(Me, 0, Height)
            MyBase.OnClick(e)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            If ShowMenuSymbol Then
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality
                Dim fontH As Integer = FontHeight 'font.height Test This Experiment!!! NoScaling
                Dim h = CInt(fontH * 0.3)
                Dim w = h * 2

                Dim x1 = If(Text.NullOrEmptyS, Width \ 2 - w \ 2, Width - w - CInt(w * 0.7))
                Dim y1 = CInt(Height / 2 - h / 2)

                Dim x2 = CInt(x1 + w / 2)
                Dim y2 = y1 + h

                Dim x3 = x1 + w
                Dim y3 = y1

                Using pen = New Pen(MyBase.ForeColor, fontH / 16.0F)
                    e.Graphics.DrawLine(pen, x1, y1, x2, y2)
                    e.Graphics.DrawLine(pen, x2, y2, x3, y3)
                End Using
            End If

            If Symbol <> ButtonSymbol.None Then
                e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

                Dim p = New Pen(Color.Black) With {.Alignment = Drawing2D.PenAlignment.Center, .EndCap = Drawing2D.LineCap.Round, .StartCap = Drawing2D.LineCap.Round, .Width = CInt(Height / 14)}
                Dim d As New SymbolDrawer With {.Graphics = e.Graphics, .Pen = p}
                Dim cw As Integer = ClientSize.Width
                d.Point1.Width = cw
                d.Point2.Width = cw
                Dim ch As Integer = ClientSize.Height
                d.Point1.Height = ch
                d.Point2.Height = ch

                Select Case Symbol
                    Case ButtonSymbol.Open
                        d.Point1.MoveRight(0.6)
                        d.Point1.MoveDown(0.3)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point1.MoveDown(0.4)
                        d.Point2.MoveDown(0.4)
                        d.Draw()
                        d.Point1.MoveLeft(0.2)
                        d.Point1.MoveUp(0.4)
                        d.Draw()
                    Case ButtonSymbol.Close
                        d.Point1.MoveRight(0.6)
                        d.Point1.MoveDown(0.3)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point1.MoveDown(0.4)
                        d.Point2.MoveDown(0.4)
                        d.Draw()
                        d.Point2.MoveRight(0.2)
                        d.Point2.MoveUp(0.4)
                        d.Draw()
                    Case ButtonSymbol.Left3
                        d.Point1.MoveRight(0.2)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Reset()
                        d.Point1.MoveRight(0.2)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.7)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                    Case ButtonSymbol.Left2
                        d.Point1.MoveRight(0.3)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.5)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Reset()
                        d.Point1.MoveRight(0.3)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.7)
                        d.Point2.MoveRight(0.5)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                    Case ButtonSymbol.Left1
                        d.Point1.MoveRight(0.4)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.6)
                        d.Draw()
                        d.Point2.MoveDown(0.4)
                        d.Draw()
                    Case ButtonSymbol.Right1
                        d.Point1.MoveRight(0.6)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point2.MoveDown(0.4)
                        d.Draw()
                    Case ButtonSymbol.Right2
                        d.Point1.MoveRight(0.5)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.3)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Reset()
                        d.Point1.MoveRight(0.5)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.7)
                        d.Point2.MoveRight(0.3)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                    Case ButtonSymbol.Right3
                        d.Point1.MoveRight(0.4)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Reset()
                        d.Point1.MoveRight(0.4)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.7)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                    Case ButtonSymbol.Delete
                        d.Point1.MoveRight(0.3)
                        d.Point1.MoveDown(0.3)
                        d.Point2.MoveRight(0.7)
                        d.Point2.MoveDown(0.7)
                        d.Draw()
                        d.Point1.MoveRight(0.4)
                        d.Point2.MoveLeft(0.4)
                        d.Draw()
                    Case ButtonSymbol.Menu
                        d.Point1.MoveRight(0.2)
                        d.Point1.MoveDown(0.35)
                        d.Point2.MoveRight(0.5)
                        d.Point2.MoveDown(0.65)
                        d.Draw()
                        d.Point1.MoveRight(0.6)
                        d.Draw()
                End Select
            End If
        End Sub

        Sub ShowBold()
            Font = New Font(Font, FontStyle.Bold)
            For i = 0 To 10
                Application.DoEvents()
                Thread.Sleep(30)
            Next
            Font = New Font(Font, FontStyle.Regular)
        End Sub

        Enum ButtonSymbol
            None
            Left1
            Left2
            Left3
            Right1
            Right2
            Right3
            Open
            Close
            Delete
            Menu
        End Enum

        Class SymbolDrawer
            Property Point1 As Point 'was new Point
            Property Point2 As Point 'was new Point
            Property Pen As Pen
            Property Graphics As Graphics

            Sub Draw()
                Graphics.DrawLine(Pen, Point1.X, Point1.Y, Point2.X, Point2.Y)
            End Sub

            Sub Reset()
                Point1.X = 0
                Point2.X = 0
                Point1.Y = 0
                Point2.Y = 0
            End Sub

            Class Point
                Property Width As Single
                Property Height As Single
                Property X As Single
                Property Y As Single

                Sub MoveLeft(value As Single)
                    X -= Width * value
                End Sub

                Sub MoveRight(value As Single)
                    X += Width * value
                End Sub

                Sub MoveUp(value As Single)
                    Y -= Height * value
                End Sub

                Sub MoveDown(value As Single)
                    Y += Height * value
                End Sub
            End Class
        End Class
    End Class

    Public Class ListBoxEx
        Inherits ListBox

        <DefaultValue(GetType(Button), Nothing)> Property UpButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property DownButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property RemoveButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property Button1 As Button
        <DefaultValue(GetType(Button), Nothing)> Property Button2 As Button

        'Private LastTick As Long
        'Private KeyText As String = String.Empty
        Private BlockOnSelectedIndexChanged As Boolean

        Sub New()
            DrawMode = DrawMode.OwnerDrawFixed
        End Sub

        Protected Overrides Sub OnFontChanged(e As EventArgs)
            ItemHeight = CInt(Font.Height * 1.4)
            MyBase.OnFontChanged(e)
        End Sub

        Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
            If Items.Count = 0 OrElse e.Index < 0 Then Exit Sub

            Dim g = e.Graphics
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            Dim r = e.Bounds

            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                r.Width -= 1
                r.Height -= 1

                r.Inflate(-1, -1)

                Using b As New SolidBrush(Color.FromArgb(&HFFB2DFFF))
                    g.FillRectangle(b, r)
                End Using
            Else
                Using b As New SolidBrush(BackColor)
                    g.FillRectangle(b, r)
                End Using
            End If

            Dim sf As New StringFormat With {.FormatFlags = StringFormatFlags.NoWrap, .LineAlignment = StringAlignment.Center}

            Dim r2 = e.Bounds
            r2.X = 2
            r2.Width = e.Bounds.Width

            Dim caption As String '= Nothing

            If DisplayMember?.Length > 0 Then
                Try
                    Dim itm As Object = Items(e.Index)
                    caption = itm.GetType.GetProperty(DisplayMember).GetValue(itm, Nothing).ToString
                Catch ex As Exception
                    caption = Items(e.Index).ToString()
                End Try
            Else
                caption = Items(e.Index).ToString()
            End If

            e.Graphics.DrawString(caption, Font, Brushes.Black, r2, sf)
        End Sub

        Sub UpdateSelection()
            If SelectedIndex > -1 Then
                BlockOnSelectedIndexChanged = True
                Items.Item(SelectedIndex) = SelectedItem
                BlockOnSelectedIndexChanged = False

                If Sorted Then
                    Sorted = False
                    Sorted = True
                End If
            End If
        End Sub

        Private ReadOnly SavedSelection As New List(Of Integer)

        Sub SaveSelection()
            For Each i As Integer In SelectedIndices
                SavedSelection.Add(i)
            Next i
        End Sub

        Sub RestoreSelection()
            For i = 0 To SavedSelection.Count - 1
                SetSelected(SavedSelection(i), True)
            Next i
            SavedSelection.Clear()
        End Sub
        'Sub DeleteItem(text As String)
        '    If FindStringExact(text) > -1 Then Items.RemoveAt(FindStringExact(text))
        'End Sub
        Sub RemoveSelection()
            If SelectedIndex > -1 Then
                BeginUpdate()
                If SelectionMode = Windows.Forms.SelectionMode.One Then
                    Dim index = SelectedIndex

                    If Items.Count - 1 > index Then
                        SelectedIndex += 1
                    Else
                        SelectedIndex -= 1
                    End If

                    Items.RemoveAt(index)
                    UpdateControls()
                Else
                    Dim iFirst = SelectedIndex
                    'Dim indices(SelectedIndices.Count - 1) As Integer
                    'SelectedIndices.CopyTo(indices, 0)
                    'SelectedIndex = -1
                    For i = SelectedItems.Count - 1 To 0 Step -1
                        Items.RemoveAt(SelectedIndices.Item(i))
                    Next

                    Dim iC As Integer = Items.Count
                    SelectedIndex = If(iFirst > iC - 1, iC - 1, iFirst)
                    'If iFirst <> 0 AndAlso iC > 1 Then SetSelected(0, False) 'Removed in UpdCtrls RemoveButton, for profiles Form!
                End If
                EndUpdate()
            End If
        End Sub
        'Function CanMoveUp() As Boolean
        '    Return SelectedIndices.Count > 0 AndAlso SelectedIndices(0) > 0
        'End Function
        'Function CanMoveDown() As Boolean
        '    Return SelectedIndices.Count > 0 AndAlso SelectedIndices(SelectedIndices.Count - 1) < Items.Count - 1
        'End Function
        Sub MoveSelectionUp()
            If SelectedItems.Count > 0 Then
                Dim iAbove = SelectedIndices(0) - 1
                If iAbove > -1 Then
                    BeginUpdate()
                    Dim itemAbove = Items.Item(iAbove)
                    Items.RemoveAt(iAbove)
                    Dim iLastItem = SelectedIndices(SelectedItems.Count - 1)
                    Items.Insert(iLastItem + 1, itemAbove)
                    SetSelected(SelectedIndex, True)
                    EndUpdate()
                End If
            End If
        End Sub

        Sub MoveSelectionDown()
            Dim siC As Integer = SelectedItems.Count
            If siC > 0 Then
                Dim iBelow = SelectedIndices(siC - 1) + 1
                If iBelow < Items.Count Then
                    BeginUpdate()
                    Dim itemBelow = Items.Item(iBelow)
                    Items.RemoveAt(iBelow)
                    Dim iAbove = SelectedIndices(0) - 1
                    Items.Insert(iAbove + 1, itemBelow)
                    SetSelected(SelectedIndex, True)
                    EndUpdate()
                End If
            End If
        End Sub

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            If Not DesignMode Then
                If UpButton IsNot Nothing Then UpButton.AddClickAction(AddressOf MoveSelectionUp)
                If DownButton IsNot Nothing Then DownButton.AddClickAction(AddressOf MoveSelectionDown)
                If RemoveButton IsNot Nothing Then RemoveButton.AddClickAction(AddressOf RemoveSelection)
                UpdateControls()
            End If
        End Sub

        Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
            If Not BlockOnSelectedIndexChanged Then
                MyBase.OnSelectedIndexChanged(e)
                UpdateControls()
            End If
        End Sub

        Sub UpdateControls()
            Dim itmC As Integer = -2 '= Items.Count
            Dim selIdx As Integer = -2 '= SelectedIndex

            If RemoveButton IsNot Nothing Then
                itmC = Items.Count 'Added ToDO: Test this, Could be errors !!!
                selIdx = SelectedIndex
                RemoveButton.Enabled = selIdx >= 0
            End If

            If UpButton IsNot Nothing Then
                UpButton.Enabled = selIdx > 0
            End If

            If DownButton IsNot Nothing Then
                DownButton.Enabled = selIdx > -1 AndAlso selIdx < itmC - 1
            End If

            If Button1 IsNot Nothing Then
                Button1.Enabled = selIdx >= 0
            End If

            If Button2 IsNot Nothing Then
                Button2.Enabled = selIdx >= 0
            End If

            If selIdx = -1 AndAlso itmC > 0 Then SelectedIndex = 0 'Really Needed ??? ToDO: Test this, Could be errors !!!
        End Sub
    End Class

    Public Class NumEdit
        Inherits UserControl

        'Private FMHeight As Short = 16 ' = Segoe9= Measur15, TxtBox16
        WithEvents TextBox As Edit

        Private UpControl As New UpDownButton(True)
        Private DownControl As New UpDownButton(False)
        Private BorderColor As Color = Color.CadetBlue
        Private TipProvider As TipProvider

        Event ValueChanged(numEdit As NumEdit)

        Sub New()
            SuspendLayout()
            SetStyle(ControlStyles.Opaque Or ControlStyles.ResizeRedraw, True)

            TextBox = New Edit ' 'Props Moved to Edit Ctor

            Controls.Add(TextBox)
            Controls.Add(UpControl)
            Controls.Add(DownControl)

            UpControl.ClickAction = Sub()
                                        Value += Increment
                                        Value = Math.Round(Value, DecimalPlaces)
                                    End Sub

            DownControl.ClickAction = Sub()
                                          Value -= Increment
                                          Value = Math.Round(Value, DecimalPlaces)
                                      End Sub

            AddHandler UpControl.MouseDown, Sub() Focus()
            AddHandler DownControl.MouseDown, Sub() Focus()
            AddHandler TextBox.LostFocus, Sub() UpdateText()
            AddHandler TextBox.GotFocus, Sub() SetColor(Color.CornflowerBlue)
            AddHandler TextBox.LostFocus, Sub() SetColor(Color.CadetBlue)
            AddHandler TextBox.MouseWheel, AddressOf Wheel
            ResumeLayout(False)
        End Sub

        WriteOnly Property Help As String
            Set(value As String)
                If TipProvider Is Nothing Then
                    TipProvider = New TipProvider()
                End If

                TipProvider.SetTip(value, TextBox, UpControl, DownControl)
            End Set
        End Property

        <Category("Data")>
        <DefaultValue(GetType(Double), "-9000000000")> 'was all: -9000000000 Remove one digit 0 to fit MaxInteger ???
        Property Minimum As Double = -9000000000.0R

        <Category("Data")>
        <DefaultValue(GetType(Double), "9000000000")>
        Property Maximum As Double = 9000000000.0R

        <Category("Data")>
        <DefaultValue(GetType(Double), "1")>
        Property Increment As Double = 1.0R

        Private ValueValue As Double

        <Category("Data")>
        <DefaultValue(GetType(Double), "0")>
        Property Value As Double
            Get
                Return ValueValue
            End Get
            Set(value As Double)
                SetValue(value, True)
            End Set
        End Property

        Private DecimalPlacesValue As Integer

        <Category("Data")>
        <DefaultValue(0)>
        Property DecimalPlaces As Integer
            Get
                Return DecimalPlacesValue
            End Get
            Set(value As Integer)
                DecimalPlacesValue = value
                UpdateText()
            End Set
        End Property

        Protected Overrides Sub Dispose(disposing As Boolean)
            TipProvider?.Dispose()
            MyBase.Dispose(disposing)
            'Events.Dispose () '???
        End Sub

        Sub Wheel(sender As Object, e As MouseEventArgs)
            If e.Delta > 0 Then
                Value += Increment
            Else
                Value -= Increment
            End If
        End Sub

        Sub SetColor(c As Color)
            BorderColor = c
            Invalidate()
        End Sub

        Protected Overridable Sub OnValueChanged(numEdit As NumEdit)
            RaiseEvent ValueChanged(Me)
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            Dim cs As Size = ClientSize
            Dim h = (cs.Height \ 2) - 1
            h -= h Mod 2
            If h > 20 Then
                h -= 1
            End If

            Dim cW As Integer = CInt(Height * 0.8)
            Dim udLeft As Integer = cs.Width - cW - 2
            UpControl.SetBounds(udLeft, 2, cW, h)
            DownControl.SetBounds(udLeft, cs.Height - h - 2, cW, h)
            TextBox.SetBounds(2, (cs.Height - 16) \ 2 + 1, udLeft - 3, 16) ' -TextBox.Height,,FMHeight) 'NoScaling!!! Segoe9= Measur15, TxtBox16
            'UpControl.Width = cW
            'UpControl.Height = h
            'UpControl.Top = 2
            'UpControl.Left = cs.Width - UpControl.Width - 2
            'DownControl.Width = cW 'UpControl.Width
            'DownControl.Left = UpControl.Left
            'DownControl.Top = cs.Height - h - 2
            'DownControl.Height = h
            'TextBox.Top = (cs.Height - TextBox.Height) \ 2 + 1
            'TextBox.Left = 2
            'TextBox.Width = DownControl.Left - 3
            'TextBox.Height = FMHeight 'TextRenderer.MeasureText("gG", TextBox.Font).Height
            MyBase.OnLayout(levent)
        End Sub
        'Protected Overrides Async Sub OnFontChanged(e As EventArgs) 'NoScaling!!! Segoe9= Measur15, TxtBox16
        '    If s.UIScaleFactor <> 1 Then
        '        FMHeight = Await Task.Run(Function() CShort(TextRenderer.MeasureText("gG", TextBox.Font).Height))
        '    End If
        '    MyBase.OnFontChanged(e)
        'End Sub
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim r = ClientRectangle
            r.Inflate(-1, -1)
            e.Graphics.FillRectangle(If(Enabled, Brushes.White, SystemBrushes.Control), r)
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid)
            MyBase.OnPaint(e)
        End Sub

        Protected Overrides ReadOnly Property DefaultSize() As Size
            Get
                'Return New Size(250, 70)
                Return New Size(72, 21) 'Segoe9 ,w:41 AudioConv
            End Get
        End Property

        Sub SetValue(value As Double, updateText As Boolean)
            If value <> ValueValue Then
                If value > Maximum Then
                    value = Maximum
                ElseIf value < Minimum Then
                    value = Minimum
                End If

                ValueValue = value

                If updateText Then
                    Me.UpdateText()
                End If

                OnValueChanged(Me)
            End If
        End Sub

        Sub UpdateText()
            TextBox.SetTextWithoutTextChanged(ValueValue.ToString("F" & DecimalPlaces))
        End Sub

        Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox.KeyDown
            If e.KeyData = Keys.Up Then
                Value += Increment
            ElseIf e.KeyData = Keys.Down Then
                Value -= Increment
            End If
        End Sub

        Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox.TextChanged
            Dim td As Double = TextBox.Text.ToDouble(Double.NaN) 'opt IsDouble
            If Not Double.IsNaN(td) Then
                SetValue(td, False)
            End If
        End Sub

        Class Edit
            Inherits TextBox

            Private BlockTextChanged As Boolean

            Sub New() 'Moved from NumeEdit Ctor
                BorderStyle = BorderStyle.None
                TextAlign = HorizontalAlignment.Center
                Text = "0"
            End Sub
            Sub SetTextWithoutTextChanged(val As String)
                BlockTextChanged = True
                Text = val
                BlockTextChanged = False
            End Sub

            Protected Overrides Sub OnTextChanged(e As EventArgs)
                If Not BlockTextChanged Then
                    MyBase.OnTextChanged(e)
                End If
            End Sub

            Protected Overrides ReadOnly Property DefaultSize As Size
                Get
                    Return New Size(50, 16) 'MyBase.DefaultSize
                End Get
            End Property
        End Class

        Class UpDownButton
            Inherits Control

            Private IsUp As Boolean
            Private IsHot As Boolean
            Private IsPressed As Boolean
            Private LastMouseDownTick As Integer

            Property ClickAction As Action

            Sub New(isUp As Boolean)
                Me.IsUp = isUp
                TabStop = False

                SetStyle(ControlStyles.Opaque Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
            End Sub

            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                IsHot = True
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                IsHot = False
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                IsPressed = True
                Invalidate()
                ClickAction.Invoke()
                LastMouseDownTick = Environment.TickCount
                MouseDownClicks(1000, LastMouseDownTick)
            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                IsPressed = False
                Invalidate()
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim gx = e.Graphics
                gx.SmoothingMode = SmoothingMode.HighQuality

                If IsPressed Then
                    gx.Clear(Color.LightBlue)
                ElseIf IsHot Then
                    gx.Clear(Color.AliceBlue)
                Else
                    gx.Clear(SystemColors.Control)
                End If

                ControlPaint.DrawBorder(gx, ClientRectangle, Color.CadetBlue, ButtonBorderStyle.Solid)
                Dim fontH As Integer = FontHeight 'Font.Height  Test This Experiment!!! NoScaling
                Dim h = CInt(fontH * 0.2)
                Dim w = h * 2

                Dim x1 = Width \ 2 - w \ 2
                Dim y1 = CInt(Height / 2 - h / 2)

                Dim x2 = CInt(x1 + w / 2)
                Dim y2 = y1 + h

                Dim x3 = x1 + w
                Dim y3 = y1

                If IsUp Then
                    y1 = y2
                    y2 = y3
                    y3 = y1
                End If

                Using pen = New Pen(If(Enabled, Color.Black, SystemColors.GrayText), fontH / 20.0F)
                    gx.DrawLine(pen, x1, y1, x2, y2)
                    gx.DrawLine(pen, x2, y2, x3, y3)
                End Using
            End Sub

            Private SlowDownItr As UInteger  'Experiment Test This !!!!!!!!!!!
            Async Sub MouseDownClicks(sleep As Integer, tick As Integer)
Again:          Await Task.Run(Sub() Thread.Sleep(sleep))

                If IsPressed AndAlso LastMouseDownTick = tick Then
                    ClickAction.Invoke()
                    SlowDownItr += 1UI
                    'MouseDownClicks(20, tick) 'Org: 20ms
                    sleep = If(SlowDownItr < 12, 90, 30)
                    GoTo Again
                Else
                    SlowDownItr = 1UI
                End If
            End Sub
        End Class
    End Class

    Public Class TextEdit
        Inherits UserControl

        Public WithEvents TextBox As New TextBoxEx
        Public Shadows Event TextChanged()
        Private BorderColor As Color = Color.CadetBlue
        'Private FMHeight As Short = 16

        Sub New()
            SetStyle(ControlStyles.Opaque Or ControlStyles.ResizeRedraw, True)
            TextBox.BorderStyle = BorderStyle.None
            Controls.Add(TextBox)
            BackColor = Color.White
            AddHandler TextBox.GotFocus, Sub() SetColor(Color.CornflowerBlue)
            AddHandler TextBox.LostFocus, Sub() SetColor(Color.CadetBlue)
            AddHandler TextBox.TextChanged, Sub() RaiseEvent TextChanged()
        End Sub

        Sub SetColor(c As Color)
            BorderColor = c
            Invalidate()
        End Sub

        <Browsable(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
        Overrides Property Text As String
            Get
                Return TextBox.Text
            End Get
            Set(value As String)
                TextBox.Text = value
            End Set
        End Property

        WriteOnly Property [ReadOnly] As Boolean
            Set(value As Boolean)
                TextBox.ReadOnly = value
            End Set
        End Property

        Protected Overrides Sub OnLayout(args As LayoutEventArgs)
            MyBase.OnLayout(args)

            Dim cs As Size = ClientSize
            If TextBox.Multiline Then
                'TextBox.Top = 2
                'TextBox.Left = 2
                'TextBox.Width = cs.Width - 4
                'TextBox.Height = cs.Height - 4
                TextBox.SetBounds(2, 2, cs.Width - 4, cs.Height - 4)
            Else
                'TextBox.Top = ((cs.Height - TextBox.Height) \ 2) - 1
                'TextBox.Left = 2
                'TextBox.Width = cs.Width - 4
                TextBox.SetBounds(2, ((cs.Height - TextBox.Height) \ 2) - 1, cs.Width - 4, 0, BoundsSpecified.Location Or BoundsSpecified.Width)

                Dim h = 16 'FMHeight 'TextRenderer.MeasureText("gG", TextBox.Font).Height 'ToDo: NoScaling

                If TextBox.Height < h Then
                    TextBox.Multiline = True
                    TextBox.MinimumSize = New Size(0, h)
                    TextBox.Multiline = False
                End If
            End If
        End Sub
        'Protected Overrides Async Sub OnFontChanged(e As EventArgs) 'NoScaling!!! Segoe9= Measur15, TxtBox16
        '    If s.UIScaleFactor <> 1 Then
        '        FMHeight = Await Task.Run(Function() CShort(TextRenderer.MeasureText("gG", TextBox.Font).Height))
        '    End If
        '    MyBase.OnFontChanged(e)
        'End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            Dim cr = ClientRectangle
            cr.Inflate(-1, -1)
            Dim col = If(Enabled AndAlso Not TextBox.ReadOnly, BackColor, SystemColors.Control)

            Using brush As New SolidBrush(col)
                e.Graphics.FillRectangle(brush, cr)
            End Using

            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid)
        End Sub
    End Class

    Public Interface IPage
        Property Node As TreeNode
        Property Path As String
        Property TipProvider As TipProvider
        Property FormSizeScaleFactor As SizeF
    End Interface

    Public Class DataGridViewEx
        Inherits DataGridView

        Protected Overrides Property DoubleBuffered As Boolean 'DoubleBuffered DGV -  keyboard responsivness
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                Return True
            End Get
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Boolean)
                MyBase.DoubleBuffered = value
            End Set
        End Property

        Function AddTextBoxColumn() As DataGridViewTextBoxColumn
            Dim ret As New DataGridViewTextBoxColumn
            Columns.Add(ret)
            Return ret
        End Function

        Function AddComboBoxColumn() As DataGridViewComboBoxColumn
            Dim ret As New DataGridViewComboBoxColumn
            Columns.Add(ret)
            Return ret
        End Function

    End Class

    Public Class TabControlEx
        Inherits TabControl

        Private DragStartPosition As Point '= Point.Empty
        Private TabType As Type

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            DragStartPosition = New Point(e.X, e.Y)
            MyBase.OnMouseDown(e)
        End Sub

        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            If e.Button <> MouseButtons.Left Then Return

            Dim rect = New Rectangle(DragStartPosition, Size.Empty)
            Static dragSize As Size = SystemInformation.DragSize
            rect.Inflate(dragSize)

            Dim page = HoverTab()

            If page IsNot Nothing AndAlso Not rect.Contains(e.X, e.Y) Then
                TabType = page.GetType
                DoDragDrop(page, DragDropEffects.All)
            End If

            DragStartPosition = Point.Empty
            MyBase.OnMouseMove(e)
        End Sub

        Protected Overrides Sub OnDragOver(e As DragEventArgs)
            Dim hoverTab = Me.HoverTab()

            If hoverTab Is Nothing Then
                e.Effect = DragDropEffects.None
            Else
                If e.Data.GetDataPresent(TabType) Then
                    e.Effect = DragDropEffects.Move
                    Dim dragTab = DirectCast(e.Data.GetData(TabType), TabPage)

                    If hoverTab Is dragTab Then Return

                    Dim tabRect = GetTabRect(TabPages.IndexOf(hoverTab))
                    tabRect.Inflate(-3, -3)

                    If tabRect.Contains(PointToClient(New Point(e.X, e.Y))) Then
                        SwapTabPages(dragTab, hoverTab)
                        SelectedTab = dragTab
                    End If
                End If
            End If

            MyBase.OnDragOver(e)
        End Sub

        Function HoverTab() As TabPage
            For index = 0 To TabCount - 1
                If GetTabRect(index).Contains(PointToClient(Cursor.Position)) Then
                    Return TabPages(index)
                End If
            Next
        End Function

        Sub SwapTabPages(ByVal tp1 As TabPage, ByVal tp2 As TabPage)
            Dim index1 = TabPages.IndexOf(tp1)
            Dim index2 = TabPages.IndexOf(tp2)
            TabPages(index1) = tp2
            TabPages(index2) = tp1
        End Sub
    End Class

    Public Class LabelProgressBar
        Inherits Control

        Property ProgressColor As Color

        Sub New()
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.Selectable, False)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

            If BackColor.GetBrightness > 0.5 Then
                ForeColor = Color.FromArgb(10, 10, 10)
                BackColor = Color.FromArgb(240, 240, 240)
                ProgressColor = Color.FromArgb(180, 180, 180)
            Else
                ForeColor = Color.FromArgb(240, 240, 240)
                BackColor = Color.FromArgb(10, 10, 10)
                ProgressColor = Color.FromArgb(100, 100, 100)
            End If
        End Sub

        Private _Minimum As Double

        Public Property Minimum() As Double
            Get
                Return _Minimum
            End Get
            Set
                If _Minimum <> Value Then
                    _Minimum = Value
                    Invalidate()
                End If
            End Set
        End Property

        Private _Maximum As Double = 100

        Public Property Maximum() As Double
            Get
                Return _Maximum
            End Get
            Set
                If _Maximum <> Value Then
                    _Maximum = Value
                    Invalidate()
                End If
            End Set
        End Property

        Private _Value As Double

        Public Property Value() As Double
            Get
                Return _Value
            End Get
            Set
                If _Value <> Value Then
                    _Value = Value
                    Invalidate()
                End If
            End Set
        End Property

        Public Overrides Property Text As String
            Get
                Return MyBase.Text
            End Get
            Set
                MyBase.Text = Value
                Invalidate()
            End Set
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g = e.Graphics
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            Using foreBrush = New SolidBrush(ForeColor)
                Using progressBrush = New SolidBrush(ProgressColor)
                    g.FillRectangle(progressBrush, New RectangleF(0, 0, CSng(Width * (Value - Minimum) / Maximum), Height))
                    g.DrawString(Text, Font, foreBrush, 0, CInt((Height - FontHeight) / 2))
                End Using
            End Using

            MyBase.OnPaint(e)
        End Sub
    End Class
End Namespace