
Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime
Imports System.Threading
Imports System.Threading.Tasks
Imports JM.LinqFaster
Imports KGySoft.CoreLibraries

Namespace UI
    Public Class CustomMenuEditor
        Inherits DialogBase

#Region " Designer "
        Protected Overloads Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private components As System.ComponentModel.IContainer

        Public WithEvents lParameters As System.Windows.Forms.Label
        Public WithEvents pg As PropertyGridEx
        Public WithEvents laText As System.Windows.Forms.Label
        Public WithEvents laHotkey As System.Windows.Forms.Label
        Private WithEvents tbText As System.Windows.Forms.TextBox
        Friend WithEvents TipProvider As StaxRip.UI.TipProvider
        Friend WithEvents tv As StaxRip.UI.TreeViewEx
        Friend WithEvents tbHotkey As System.Windows.Forms.TextBox
        Friend WithEvents tbCommand As System.Windows.Forms.TextBox
        Friend WithEvents bnCommand As ButtonEx
        Friend WithEvents cmsCommand As ContextMenuStripEx
        Friend WithEvents laCommand As System.Windows.Forms.Label
        Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
        Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents tlpCommand As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents flpBottom As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents laIcon As Label
        Friend WithEvents tlpSymbol As TableLayoutPanel
        Friend WithEvents laSymbol As Label
        Friend WithEvents pbSymbol As PictureBox
        Friend WithEvents bnSymbol As ButtonEx
        Friend WithEvents cmsSymbol As ContextMenuStripEx
        Friend WithEvents ToolStrip As ToolStrip
        Friend WithEvents tsbNew As ToolStripButton
        Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
        Friend WithEvents tsbCut As ToolStripButton
        Friend WithEvents tsbCopy As ToolStripButton
        Friend WithEvents tsbPaste As ToolStripButton
        Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
        Friend WithEvents tsbMoveLeft As ToolStripButton
        Friend WithEvents tsbMoveRight As ToolStripButton
        Friend WithEvents tsbMoveUp As ToolStripButton
        Friend WithEvents tsbMoveDown As ToolStripButton
        Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
        Friend WithEvents tsbRemove As ToolStripButton
        Friend WithEvents ToolsToolStripDropDownButton As ToolStripDropDownButton
        Friend WithEvents NewFromDefaultsToolStripMenuItem As ToolStripMenuItemEx
        Friend WithEvents ResetToolStripMenuItem As ToolStripMenuItemEx
        Friend WithEvents bnOK As StaxRip.UI.ButtonEx
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.laHotkey = New System.Windows.Forms.Label()
            Me.lParameters = New System.Windows.Forms.Label()
            Me.pg = New StaxRip.UI.PropertyGridEx()
            Me.tbText = New System.Windows.Forms.TextBox()
            Me.laText = New System.Windows.Forms.Label()
            Me.tv = New StaxRip.UI.TreeViewEx()
            Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
            Me.tbHotkey = New System.Windows.Forms.TextBox()
            Me.tbCommand = New System.Windows.Forms.TextBox()
            Me.bnCommand = New StaxRip.UI.ButtonEx()
            Me.cmsCommand = New StaxRip.UI.ContextMenuStripEx(Me.components)
            Me.laCommand = New System.Windows.Forms.Label()
            Me.bnCancel = New StaxRip.UI.ButtonEx()
            Me.bnOK = New StaxRip.UI.ButtonEx()
            Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
            Me.flpBottom = New System.Windows.Forms.FlowLayoutPanel()
            Me.tlpCommand = New System.Windows.Forms.TableLayoutPanel()
            Me.ToolStrip = New System.Windows.Forms.ToolStrip()
            Me.tsbNew = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
            Me.tsbCut = New System.Windows.Forms.ToolStripButton()
            Me.tsbCopy = New System.Windows.Forms.ToolStripButton()
            Me.tsbPaste = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
            Me.tsbMoveLeft = New System.Windows.Forms.ToolStripButton()
            Me.tsbMoveRight = New System.Windows.Forms.ToolStripButton()
            Me.tsbMoveUp = New System.Windows.Forms.ToolStripButton()
            Me.tsbMoveDown = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.tsbRemove = New System.Windows.Forms.ToolStripButton()
            Me.ToolsToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
            Me.NewFromDefaultsToolStripMenuItem = New StaxRip.UI.ToolStripMenuItemEx()
            Me.ResetToolStripMenuItem = New StaxRip.UI.ToolStripMenuItemEx()
            Me.laIcon = New System.Windows.Forms.Label()
            Me.tlpSymbol = New System.Windows.Forms.TableLayoutPanel()
            Me.laSymbol = New System.Windows.Forms.Label()
            Me.pbSymbol = New System.Windows.Forms.PictureBox()
            Me.bnSymbol = New StaxRip.UI.ButtonEx()
            Me.cmsSymbol = New StaxRip.UI.ContextMenuStripEx(Me.components)
            Me.tlpMain.SuspendLayout()
            Me.flpBottom.SuspendLayout()
            Me.tlpCommand.SuspendLayout()
            Me.ToolStrip.SuspendLayout()
            Me.tlpSymbol.SuspendLayout()
            CType(Me.pbSymbol, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'laHotkey
            '
            Me.laHotkey.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.laHotkey.AutoSize = True
            Me.laHotkey.Location = New System.Drawing.Point(292, 78)
            Me.laHotkey.Margin = New System.Windows.Forms.Padding(0, 7, 0, 0)
            Me.laHotkey.Name = "laHotkey"
            Me.laHotkey.Size = New System.Drawing.Size(287, 15)
            Me.laHotkey.TabIndex = 5
            Me.laHotkey.Text = "Shortcut Key:"
            '
            'lParameters
            '
            Me.lParameters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lParameters.AutoSize = True
            Me.lParameters.Location = New System.Drawing.Point(292, 221)
            Me.lParameters.Margin = New System.Windows.Forms.Padding(0, 7, 0, 0)
            Me.lParameters.Name = "lParameters"
            Me.lParameters.Size = New System.Drawing.Size(287, 15)
            Me.lParameters.TabIndex = 9
            Me.lParameters.Text = "Command Parameters:"
            Me.lParameters.Visible = False
            '
            'pg
            '
            Me.pg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pg.HelpVisible = False
            Me.pg.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pg.Location = New System.Drawing.Point(292, 236)
            Me.pg.Margin = New System.Windows.Forms.Padding(0)
            Me.pg.Name = "pg"
            Me.pg.PropertySort = System.Windows.Forms.PropertySort.NoSort
            Me.pg.Size = New System.Drawing.Size(287, 245)
            Me.pg.TabIndex = 10
            Me.pg.ToolbarVisible = False
            Me.pg.Visible = False
            '
            'tbText
            '
            Me.tbText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbText.Location = New System.Drawing.Point(292, 48)
            Me.tbText.Margin = New System.Windows.Forms.Padding(0)
            Me.tbText.Name = "tbText"
            Me.tbText.Size = New System.Drawing.Size(287, 23)
            Me.tbText.TabIndex = 4
            '
            'laText
            '
            Me.laText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.laText.AutoSize = True
            Me.laText.Location = New System.Drawing.Point(292, 33)
            Me.laText.Margin = New System.Windows.Forms.Padding(0)
            Me.laText.Name = "laText"
            Me.laText.Size = New System.Drawing.Size(287, 15)
            Me.laText.TabIndex = 3
            Me.laText.Text = "Text:"
            '
            'tv
            '
            Me.tv.AllowDrop = True
            Me.tv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tv.HideSelection = False
            Me.tv.Location = New System.Drawing.Point(5, 33)
            Me.tv.Margin = New System.Windows.Forms.Padding(0, 0, 5, 0)
            Me.tv.Name = "tv"
            Me.tlpMain.SetRowSpan(Me.tv, 10)
            Me.tv.Size = New System.Drawing.Size(282, 448)
            Me.tv.TabIndex = 2
            '
            'tbHotkey
            '
            Me.tbHotkey.AcceptsReturn = True
            Me.tbHotkey.AcceptsTab = True
            Me.tbHotkey.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbHotkey.Location = New System.Drawing.Point(292, 93)
            Me.tbHotkey.Margin = New System.Windows.Forms.Padding(0)
            Me.tbHotkey.Name = "tbHotkey"
            Me.tbHotkey.Size = New System.Drawing.Size(287, 23)
            Me.tbHotkey.TabIndex = 6
            '
            'tbCommand
            '
            Me.tbCommand.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbCommand.Location = New System.Drawing.Point(0, 0)
            Me.tbCommand.Margin = New System.Windows.Forms.Padding(0)
            Me.tbCommand.Name = "tbCommand"
            Me.tbCommand.Size = New System.Drawing.Size(261, 23)
            Me.tbCommand.TabIndex = 8
            '
            'bnCommand
            '
            Me.bnCommand.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.bnCommand.Location = New System.Drawing.Point(264, 0)
            Me.bnCommand.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
            Me.bnCommand.ShowMenuSymbol = True
            Me.bnCommand.Size = New System.Drawing.Size(23, 23)
            '
            'cmsCommand
            '
            Me.cmsCommand.Font = New System.Drawing.Font("Segoe UI", 9.0!)
            Me.cmsCommand.ImageScalingSize = New System.Drawing.Size(24, 24)
            Me.cmsCommand.Name = "cmsCommand"
            Me.cmsCommand.Size = New System.Drawing.Size(61, 4)
            '
            'laCommand
            '
            Me.laCommand.AutoSize = True
            Me.laCommand.Location = New System.Drawing.Point(292, 176)
            Me.laCommand.Margin = New System.Windows.Forms.Padding(0, 7, 0, 0)
            Me.laCommand.Name = "laCommand"
            Me.laCommand.Size = New System.Drawing.Size(67, 15)
            Me.laCommand.TabIndex = 7
            Me.laCommand.Text = "Command:"
            '
            'bnCancel
            '
            Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bnCancel.Location = New System.Drawing.Point(88, 0)
            Me.bnCancel.Margin = New System.Windows.Forms.Padding(5, 0, 0, 0)
            Me.bnCancel.Size = New System.Drawing.Size(83, 23)
            Me.bnCancel.Text = "Cancel"
            '
            'bnOK
            '
            Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bnOK.Location = New System.Drawing.Point(0, 0)
            Me.bnOK.Margin = New System.Windows.Forms.Padding(0)
            Me.bnOK.Size = New System.Drawing.Size(83, 23)
            Me.bnOK.Text = "OK"
            '
            'tlpMain
            '
            Me.tlpMain.ColumnCount = 4
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6.0!))
            Me.tlpMain.Controls.Add(Me.flpBottom, 2, 11)
            Me.tlpMain.Controls.Add(Me.pg, 2, 10)
            Me.tlpMain.Controls.Add(Me.lParameters, 2, 9)
            Me.tlpMain.Controls.Add(Me.tlpCommand, 2, 8)
            Me.tlpMain.Controls.Add(Me.laCommand, 2, 7)
            Me.tlpMain.Controls.Add(Me.ToolStrip, 0, 0)
            Me.tlpMain.Controls.Add(Me.tbHotkey, 2, 4)
            Me.tlpMain.Controls.Add(Me.tv, 1, 1)
            Me.tlpMain.Controls.Add(Me.laHotkey, 2, 3)
            Me.tlpMain.Controls.Add(Me.tbText, 2, 2)
            Me.tlpMain.Controls.Add(Me.laText, 2, 1)
            Me.tlpMain.Controls.Add(Me.laIcon, 2, 5)
            Me.tlpMain.Controls.Add(Me.tlpSymbol, 2, 6)
            Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tlpMain.Location = New System.Drawing.Point(0, 0)
            Me.tlpMain.Margin = New System.Windows.Forms.Padding(1)
            Me.tlpMain.Name = "tlpMain"
            Me.tlpMain.RowCount = 12
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.Size = New System.Drawing.Size(585, 514)
            Me.tlpMain.TabIndex = 11
            '
            'flpBottom
            '
            Me.flpBottom.AutoSize = True
            Me.flpBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.flpBottom.Controls.Add(Me.bnOK)
            Me.flpBottom.Controls.Add(Me.bnCancel)
            Me.flpBottom.Location = New System.Drawing.Point(292, 486)
            Me.flpBottom.Margin = New System.Windows.Forms.Padding(0, 5, 0, 5)
            Me.flpBottom.Name = "flpBottom"
            Me.flpBottom.Size = New System.Drawing.Size(171, 23)
            Me.flpBottom.TabIndex = 12
            '
            'tlpCommand
            '
            Me.tlpCommand.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tlpCommand.AutoSize = True
            Me.tlpCommand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.tlpCommand.ColumnCount = 2
            Me.tlpCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.tlpCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
            Me.tlpCommand.Controls.Add(Me.bnCommand, 1, 0)
            Me.tlpCommand.Controls.Add(Me.tbCommand, 0, 0)
            Me.tlpCommand.Location = New System.Drawing.Point(292, 191)
            Me.tlpCommand.Margin = New System.Windows.Forms.Padding(0)
            Me.tlpCommand.Name = "tlpCommand"
            Me.tlpCommand.RowCount = 1
            Me.tlpCommand.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpCommand.Size = New System.Drawing.Size(287, 23)
            Me.tlpCommand.TabIndex = 12
            '
            'ToolStrip
            '
            Me.ToolStrip.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ToolStrip.AutoSize = False
            Me.tlpMain.SetColumnSpan(Me.ToolStrip, 4)
            Me.ToolStrip.Dock = System.Windows.Forms.DockStyle.None
            Me.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNew, Me.ToolStripSeparator3, Me.tsbCut, Me.tsbCopy, Me.tsbPaste, Me.ToolStripSeparator4, Me.tsbMoveLeft, Me.tsbMoveRight, Me.tsbMoveUp, Me.tsbMoveDown, Me.ToolStripSeparator1, Me.tsbRemove, Me.ToolsToolStripDropDownButton})
            Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
            Me.ToolStrip.Margin = New System.Windows.Forms.Padding(0, 0, 0, 3)
            Me.ToolStrip.Name = "ToolStrip"
            Me.ToolStrip.Padding = New System.Windows.Forms.Padding(2, 1, 1, 0)
            Me.ToolStrip.Size = New System.Drawing.Size(585, 30)
            Me.ToolStrip.TabIndex = 1
            Me.ToolStrip.Text = "ToolStrip"
            '
            'tsbNew
            '
            Me.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbNew.Name = "tsbNew"
            Me.tsbNew.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbNew.Size = New System.Drawing.Size(24, 26)
            Me.tsbNew.Text = "New"
            '
            'ToolStripSeparator3
            '
            Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
            Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 29)
            '
            'tsbCut
            '
            Me.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbCut.Name = "tsbCut"
            Me.tsbCut.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbCut.Size = New System.Drawing.Size(24, 26)
            Me.tsbCut.Text = "Cut"
            '
            'tsbCopy
            '
            Me.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbCopy.Name = "tsbCopy"
            Me.tsbCopy.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbCopy.Size = New System.Drawing.Size(24, 26)
            Me.tsbCopy.Text = "Copy"
            '
            'tsbPaste
            '
            Me.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbPaste.Name = "tsbPaste"
            Me.tsbPaste.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbPaste.Size = New System.Drawing.Size(24, 26)
            Me.tsbPaste.Text = "Paste"
            '
            'ToolStripSeparator4
            '
            Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
            Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 29)
            '
            'tsbMoveLeft
            '
            Me.tsbMoveLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbMoveLeft.Name = "tsbMoveLeft"
            Me.tsbMoveLeft.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbMoveLeft.Size = New System.Drawing.Size(24, 26)
            Me.tsbMoveLeft.ToolTipText = "Move Left"
            '
            'tsbMoveRight
            '
            Me.tsbMoveRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbMoveRight.Name = "tsbMoveRight"
            Me.tsbMoveRight.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbMoveRight.Size = New System.Drawing.Size(24, 26)
            Me.tsbMoveRight.ToolTipText = "Move Right"
            '
            'tsbMoveUp
            '
            Me.tsbMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbMoveUp.Name = "tsbMoveUp"
            Me.tsbMoveUp.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbMoveUp.Size = New System.Drawing.Size(24, 26)
            Me.tsbMoveUp.ToolTipText = "Move Up"
            '
            'tsbMoveDown
            '
            Me.tsbMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbMoveDown.Name = "tsbMoveDown"
            Me.tsbMoveDown.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbMoveDown.Size = New System.Drawing.Size(24, 26)
            Me.tsbMoveDown.ToolTipText = "Move Down"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 29)
            '
            'tsbRemove
            '
            Me.tsbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.tsbRemove.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbRemove.Name = "tsbRemove"
            Me.tsbRemove.Padding = New System.Windows.Forms.Padding(10, 4, 10, 4)
            Me.tsbRemove.Size = New System.Drawing.Size(24, 26)
            Me.tsbRemove.Text = "Remove"
            '
            'ToolsToolStripDropDownButton
            '
            Me.ToolsToolStripDropDownButton.AutoToolTip = False
            Me.ToolsToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.ToolsToolStripDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewFromDefaultsToolStripMenuItem, Me.ResetToolStripMenuItem})
            Me.ToolsToolStripDropDownButton.Name = "ToolsToolStripDropDownButton"
            Me.ToolsToolStripDropDownButton.Size = New System.Drawing.Size(56, 26)
            Me.ToolsToolStripDropDownButton.Text = " Tools  "
            '
            'NewFromDefaultsToolStripMenuItem
            '
            Me.NewFromDefaultsToolStripMenuItem.Help = Nothing
            Me.NewFromDefaultsToolStripMenuItem.Name = "NewFromDefaultsToolStripMenuItem"
            Me.NewFromDefaultsToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
            Me.NewFromDefaultsToolStripMenuItem.Text = "New From Defaults..."
            '
            'ResetToolStripMenuItem
            '
            Me.ResetToolStripMenuItem.Help = Nothing
            Me.ResetToolStripMenuItem.Name = "ResetToolStripMenuItem"
            Me.ResetToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
            Me.ResetToolStripMenuItem.Text = "Reset Everything"
            '
            'laIcon
            '
            Me.laIcon.AutoSize = True
            Me.laIcon.Location = New System.Drawing.Point(292, 123)
            Me.laIcon.Margin = New System.Windows.Forms.Padding(0, 7, 0, 0)
            Me.laIcon.Name = "laIcon"
            Me.laIcon.Size = New System.Drawing.Size(33, 15)
            Me.laIcon.TabIndex = 13
            Me.laIcon.Text = "Icon:"
            '
            'tlpSymbol
            '
            Me.tlpSymbol.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tlpSymbol.ColumnCount = 3
            Me.tlpSymbol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
            Me.tlpSymbol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.tlpSymbol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
            Me.tlpSymbol.Controls.Add(Me.laSymbol, 1, 0)
            Me.tlpSymbol.Controls.Add(Me.pbSymbol, 0, 0)
            Me.tlpSymbol.Controls.Add(Me.bnSymbol, 2, 0)
            Me.tlpSymbol.Location = New System.Drawing.Point(292, 138)
            Me.tlpSymbol.Margin = New System.Windows.Forms.Padding(0)
            Me.tlpSymbol.Name = "tlpSymbol"
            Me.tlpSymbol.RowCount = 1
            Me.tlpSymbol.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.tlpSymbol.Size = New System.Drawing.Size(287, 31)
            Me.tlpSymbol.TabIndex = 14
            '
            'laSymbol
            '
            Me.laSymbol.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.laSymbol.Location = New System.Drawing.Point(32, 0)
            Me.laSymbol.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.laSymbol.Name = "laSymbol"
            Me.laSymbol.Size = New System.Drawing.Size(230, 31)
            Me.laSymbol.TabIndex = 1
            Me.laSymbol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'pbSymbol
            '
            Me.pbSymbol.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pbSymbol.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.pbSymbol.Location = New System.Drawing.Point(0, 0)
            Me.pbSymbol.Margin = New System.Windows.Forms.Padding(0)
            Me.pbSymbol.Name = "pbSymbol"
            Me.pbSymbol.Size = New System.Drawing.Size(30, 31)
            Me.pbSymbol.TabIndex = 2
            Me.pbSymbol.TabStop = False
            '
            'bnSymbol
            '
            Me.bnSymbol.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.bnSymbol.ContextMenuStrip = Me.cmsSymbol
            Me.bnSymbol.Location = New System.Drawing.Point(264, 4)
            Me.bnSymbol.Margin = New System.Windows.Forms.Padding(0)
            Me.bnSymbol.ShowMenuSymbol = True
            Me.bnSymbol.Size = New System.Drawing.Size(23, 23)
            '
            'cmsSymbol
            '
            Me.cmsSymbol.ImageScalingSize = New System.Drawing.Size(48, 48)
            Me.cmsSymbol.Name = "cmsSymbol"
            Me.cmsSymbol.Size = New System.Drawing.Size(61, 4)
            '
            'CustomMenuEditor
            '
            Me.AcceptButton = Me.bnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.CancelButton = Me.bnCancel
            Me.ClientSize = New System.Drawing.Size(585, 514)
            Me.Controls.Add(Me.tlpMain)
            Me.KeyPreview = True
            Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.Name = "CustomMenuEditor"
            Me.Text = "Menu Editor"
            Me.tlpMain.ResumeLayout(False)
            Me.tlpMain.PerformLayout()
            Me.flpBottom.ResumeLayout(False)
            Me.tlpCommand.ResumeLayout(False)
            Me.tlpCommand.PerformLayout()
            Me.ToolStrip.ResumeLayout(False)
            Me.ToolStrip.PerformLayout()
            Me.tlpSymbol.ResumeLayout(False)
            CType(Me.pbSymbol, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private Block As Boolean
        Private GridTypeDescriptor As GridTypeDescriptor
        Private ClipboardNode As TreeNode
        'Private EnumSag As Symbol()
        'Private EnumAwe As Symbol()
        'Private ESagImgsT As Task(Of Image())
        'Private EAweImgsT As Task(Of Image())
        Property GenericMenu As CustomMenu

        Sub New(menu As CustomMenu)
            MyBase.New()
            Dim sic As New Size(16, 16)
            Dim imgArT = Task.Run(Function()
                                      sic = SystemInformation.SmallIconSize
                                      Return {ImageHelp.GetSymbolImageSmall(Symbol.Page).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.Copy).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.Cut).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.Paste).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.Remove).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.Back).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.Up).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.Forward).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.Down).ResizeIconSize(sic),
                                              ImageHelp.GetSymbolImageSmall(Symbol.More).ResizeIconSize(sic)}
                                  End Function)

            'Dim symbComparer = New Comparison(Of Symbol)(Function(x, y) String.Compare([Enum](Of Symbol).GetName(x), [Enum](Of Symbol).GetName(y), StringComparison.OrdinalIgnoreCase))
            Dim esk = [Enum].GetNames(GetType(Symbol))  '[Enum](Of Symbol).GetNames 'Faster <0.1ms; Retrieving one by one ~0.3ms
            Dim esv = DirectCast([Enum].GetValues(GetType(Symbol)), Symbol()) '[Enum](Of Symbol).GetValues '~0.3ms
            Dim ESagImgsT = Task.Run(Function()
                                         Dim EnumSag As Symbol() = esv.WhereF(Function(s) s <= 61400 AndAlso s > 0) 'ToDO: Add sort Key Array; GetNames For Loop
                                         Dim eKSag(EnumSag.Length - 1) As String
                                         Array.Copy(esk, 1, eKSag, 0, EnumSag.Length) 'Test This,Image match desc!!!
                                         Array.Sort(eKSag, EnumSag, StringComparer.OrdinalIgnoreCase)
                                         'Array.Sort(eSag, symbComparer) 'Now ~3ms
                                         Dim eImgSag(EnumSag.Length - 1) As Image
                                         'Parallel.For(0, eSag.Length, New ParallelOptions With {.MaxDegreeOfParallelism = 2}, Sub(n) retA(n) = ImageHelp.GetSymbolImage(eSag(n)))
                                         For n = 0 To EnumSag.Length - 1
                                             eImgSag(n) = ImageHelp.GetSymbolImage(EnumSag(n))
                                         Next n
                                         Return (EnumSag, eImgSag, eKSag)
                                     End Function)

            Dim EAweImgsT = Task.Run(Function()
                                         Dim EnumAwe As Symbol() = esv.WhereF(Function(s) s > 61400)
                                         Dim eKAwe(EnumAwe.Length - 1) As String
                                         Array.Copy(esk, esv.Length - EnumAwe.Length, eKAwe, 0, EnumAwe.Length)
                                         Dim textInfo As Globalization.TextInfo = InvCultTxtInf
                                         eKAwe.SelectInPlaceF(Function(s) textInfo.ToTitleCase(s.Substring(3).Replace("_"c, " "c)))
                                         Array.Sort(eKAwe, EnumAwe, StringComparer.Ordinal) 'OrdinalIgnoreCase is Safer! ???
                                         'Array.Sort(eAwe, symbComparer)
                                         Dim eImgAwe(EnumAwe.Length - 1) As Image
                                         'Parallel.For(0, EnumAwe.Length, New ParallelOptions With {.MaxDegreeOfParallelism = 2}, Sub(n) retA(n) = ImageHelp.GetSymbolImage(EnumAwe(n)))
                                         For n = 0 To EnumAwe.Length - 1
                                             eImgAwe(n) = ImageHelp.GetSymbolImage(EnumAwe(n))
                                         Next n
                                         'Return retA
                                         Return (EnumAwe, eImgAwe, eKAwe)
                                     End Function)
            InitializeComponent()
            ScaleClientSize(38, 38, FontHeight)
            CancelButton = Nothing
            GridTypeDescriptor = New GridTypeDescriptor
            tv.BeginUpdate()
            PopulateTreeView(menu.MenuItem.GetClone, Nothing)
            tv.ExpandAll()
            tv.SelectedNode = tv.Nodes(0)
            tv.EndUpdate()

            GenericMenu = menu
            cmsCommand.SuspendLayout()
            Command.PopulateCommandMenu(cmsCommand.Items, GenericMenu.CommandManager.Commands.Values, AddressOf SetCommand)
            cmsCommand.ResumeLayout(False)

            TipProvider.SetTip("Parameters used when the command is executed. Please make a feature request if useful parameters are missing.", pg, lParameters)
            TipProvider.SetTip("Text to be displayed. Enter minus to create a separator.", tbText, laText)
            TipProvider.SetTip("A key can be deleted by pressing it two times.", tbHotkey, laHotkey)
            TipProvider.SetTip("Command to be executed. Please make a feature request if useful commands are missing.", tbCommand, laCommand)

            'g.SetRenderer(cmsSymbol) 'Needed???  Not RemoveIt!
            ToolStrip.SuspendLayout()
            g.SetRenderer(ToolStrip)
            If s.UIScaleFactor <> 1 Then
                ToolStrip.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
                ToolStripRendererEx.FontHeight = ToolStrip.Font.Height
            End If
            ToolStrip.ImageScalingSize = sic

            Dim imgAr = imgArT.Result
            tsbNew.Image = imgAr(0)
            tsbCopy.Image = imgAr(1)
            tsbCut.Image = imgAr(2)
            tsbPaste.Image = imgAr(3)
            tsbRemove.Image = imgAr(4)
            tsbMoveLeft.Image = imgAr(5)
            tsbMoveUp.Image = imgAr(6)
            tsbMoveRight.Image = imgAr(7)
            tsbMoveDown.Image = imgAr(8)
            ToolsToolStripDropDownButton.Image = imgAr(9)
            ToolStrip.ResumeLayout(False) 'Needed??? ToDo!!! Seems indeed needed!

            Task.Run(Sub()
                         For n = 1 To 3
                             Thread.Sleep(90) '30-60(onshown),60-105(new)
                             ESagImgsT.Wait()
                             EAweImgsT.Wait()
                             If IsDisposed OrElse Disposing Then
                                 [Enum](Of Symbol).ClearCaches()
                                 Exit Sub
                             End If
                             If IsHandleCreated Then
                                 BeginInvoke(Sub()
                                                 Application.DoEvents()
                                                 Dim enumS As Symbol() = ESagImgsT.Result.EnumSag
                                                 Dim eImgA As Image() = ESagImgsT.Result.eImgSag
                                                 Dim eKeysA As String() = ESagImgsT.Result.eKSag
                                                 'Dim m1LAlphSag As New List(Of ToolStripMenuItemEx)(26) 'List dynamic size version(no <inc> needed)
                                                 Dim m1LAlphSag(24) As ToolStripMenuItemEx
                                                 'Dim m2LLSag As New List(Of List(Of ActionMenuItem))(26)
                                                 Dim m2LLSag(24) As List(Of ActionMenuItem)
                                                 Dim invCultTI As Globalization.TextInfo = InvCultTxtInf

                                                 For i = 0 To enumS.Length - 1
                                                     Dim inc As Integer
                                                     Dim nM2LSag As List(Of ActionMenuItem)
                                                     Dim symb = enumS(i)
                                                     'Dim sName As String = [Enum](Of Symbol).GetName(symb)
                                                     Dim sName As String = eKeysA(i)
                                                     Dim lastCh As Char
                                                     Dim c1 As Char = invCultTI.ToUpper(sName.Chars(0)) '20x faster than String.ToUpper
                                                     If c1 <> lastCh Then
                                                         lastCh = c1
                                                         m1LAlphSag(inc) = New ToolStripMenuItemEx(c1)
                                                         nM2LSag = New List(Of ActionMenuItem)(32)
                                                         m2LLSag(inc) = nM2LSag
                                                         inc += 1
                                                     End If
                                                     nM2LSag.Add(New ActionMenuItem(sName, Sub() HandleSymbol(symb), eImgA(i)))
                                                 Next i

                                                 enumS = EAweImgsT.Result.EnumAwe
                                                 eImgA = EAweImgsT.Result.eImgAwe
                                                 eKeysA = EAweImgsT.Result.eKAwe
                                                 'Dim m1LAlphAwe As New List(Of ToolStripMenuItemEx)(26)
                                                 Dim m1LAlphAwe(25) As ToolStripMenuItemEx
                                                 'Dim m2LLAwe As New List(Of List(Of ActionMenuItem))(26)
                                                 Dim m2LLAwe(25) As List(Of ActionMenuItem)

                                                 For i = 0 To enumS.Length - 1
                                                     Dim inc As Integer
                                                     Dim nMm2LAwe As List(Of ActionMenuItem)
                                                     Dim symb = enumS(i)
                                                     'Dim sName As String = [Enum](Of Symbol).GetName(symb)
                                                     Dim sName As String = eKeysA(i)
                                                     Dim lastCh As Char
                                                     Dim c1 As Char = invCultTI.ToUpper(sName.Chars(0))
                                                     If c1 <> lastCh Then
                                                         lastCh = c1
                                                         m1LAlphAwe(inc) = New ToolStripMenuItemEx(c1)
                                                         nMm2LAwe = New List(Of ActionMenuItem)(24)
                                                         m2LLAwe(inc) = nMm2LAwe
                                                         inc += 1
                                                     End If
                                                     'nMm2LAwe.Add(New ActionMenuItem(textInfo.ToTitleCase(sName.Substring(3).Replace("_"c, " "c)), Sub() HandleSymbol(symb), eImgA(i)))
                                                     nMm2LAwe.Add(New ActionMenuItem(sName, Sub() HandleSymbol(symb), eImgA(i)))
                                                 Next i

                                                 If Not IsDisposed AndAlso Not Disposing AndAlso IsHandleCreated Then
                                                     cmsSymbol.SuspendLayout()
                                                     Dim tsmiS = New ToolStripMenuItemEx("Segoe MDL2 Assets")
                                                     Dim tsmiA = New ToolStripMenuItemEx("FontAwesome")
                                                     tsmiS.DropDown.SuspendLayout()
                                                     tsmiA.DropDown.SuspendLayout()
                                                     cmsSymbol.Items.AddRange({New ActionMenuItem("No Icon", Sub() HandleSymbol(Symbol.None)), tsmiS, tsmiA})
                                                     tsmiS.DropDownItems.AddRange(m1LAlphSag)
                                                     tsmiA.DropDownItems.AddRange(m1LAlphAwe)

                                                     For i = 0 To m1LAlphSag.Length - 1
                                                         Dim nMISag As ToolStripMenuItemEx = m1LAlphSag(i)
                                                         nMISag.DropDown.SuspendLayout()
                                                         nMISag.DropDownItems.AddRange(m2LLSag(i).ToArray)
                                                         nMISag.DropDown.ResumeLayout(False)
                                                     Next i

                                                     For i = 0 To m1LAlphAwe.Length - 1
                                                         Dim nMIAwe As ToolStripMenuItemEx = m1LAlphAwe(i)
                                                         nMIAwe.DropDown.SuspendLayout()
                                                         nMIAwe.DropDownItems.AddRange(m2LLAwe(i).ToArray)
                                                         nMIAwe.DropDown.ResumeLayout(False)
                                                     Next i

                                                     tsmiS.DropDown.ResumeLayout(False)
                                                     tsmiA.DropDown.ResumeLayout(False)
                                                     cmsSymbol.ResumeLayout(False)
                                                 End If

                                                 Task.Run(Sub()
                                                              EAweImgsT.Dispose()
                                                              ESagImgsT.Dispose()
                                                              [Enum](Of Symbol).ClearCaches()
                                                              GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
                                                              GC.Collect(2, GCCollectionMode.Forced, True, True)
                                                              GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
                                                          End Sub)
                                             End Sub)
                                 Exit Sub
                             End If
                         Next n
                     End Sub)
        End Sub

        Sub HandleSymbol(symbol As Symbol)
            If Not Block AndAlso tv.SelectedNode IsNot Nothing Then
                Dim item = DirectCast(tv.SelectedNode.Tag, CustomMenuItem)
                item.Symbol = symbol
                UpdateControls()
            End If
        End Sub

        Sub SetCommand(c As Command)
            tbCommand.Text = c.MethodInfo.Name
        End Sub

        Sub tv_DragDrop(sender As Object, e As DragEventArgs) Handles tv.DragDrop
            Block = False
        End Sub

        Sub tv_DragEnter(sender As Object, e As DragEventArgs) Handles tv.DragEnter
            If e.Data.GetDataPresent(GetType(TreeNode)) Then
                e.Effect = DragDropEffects.Move
            End If
        End Sub

        Sub tv_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles tv.ItemDrag
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Block = True
                DoDragDrop(e.Item, DragDropEffects.Move)
            End If
        End Sub

        Sub tv_DragOver(sender As Object, e As DragEventArgs) Handles tv.DragOver
            If e.Data.GetDataPresent(GetType(TreeNode)) Then
                Dim node As TreeNode = CType(e.Data.GetData(GetType(TreeNode)), TreeNode)
                Dim destNode As TreeNode = tv.GetNodeAt(tv.PointToClient(New Point(e.X, e.Y)))
                Dim nodes As New ArrayList
                AddNodes(node, nodes)

                If destNode IsNot node AndAlso destNode IsNot Nothing AndAlso Not nodes.Contains(destNode) Then

                    If Control.ModifierKeys = Keys.Control Then
                        If Not destNode.Nodes.Contains(node) AndAlso Not String.Equals(destNode.Text, "-") Then

                            node.Remove()
                            destNode.Nodes.Add(node)
                            destNode.ExpandAll()
                            tv.SelectedNode = node
                        End If
                    Else
                        If destNode.Parent IsNot Nothing Then
                            If destNode.Bounds.Top < node.Bounds.Top Then
                                node.Remove()

                                destNode.Parent.Nodes.Insert(
                                    destNode.Parent.Nodes.IndexOf(destNode), node)

                                destNode.ExpandAll()
                                tv.SelectedNode = node
                            Else
                                node.Remove()

                                destNode.Parent.Nodes.Insert(
                                    destNode.Parent.Nodes.IndexOf(destNode) + 1, node)

                                destNode.ExpandAll()
                                tv.SelectedNode = node
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Sub UpdateControls()
            If Not Block Then
                Block = True
                Dim n As TreeNode = tv.SelectedNode
                If n Is Nothing Then Exit Sub
                Dim item = CType(n.Tag, CustomMenuItem)
                tbText.Text = item.Text

                If item.Symbol = Symbol.None Then
                    pbSymbol.BackgroundImage = Nothing
                Else
                    pbSymbol.BackgroundImage = ImageHelp.GetSymbolImage(item.Symbol)
                End If

                laSymbol.Text = If(item.Symbol = Symbol.None, "", item.Symbol.ToString)
                tbHotkey.Text = KeysHelp.GetKeyString(item.KeyData)
                Dim found As Boolean

                For Each i As Command In GenericMenu.CommandManager.Commands.Values
                    If String.Equals(i.MethodInfo.Name, item.MethodName) Then
                        If tbCommand.Text = i.MethodInfo.Name Then
                            tbCommand_TextChanged()
                        Else
                            tbCommand.Text = i.MethodInfo.Name
                        End If

                        found = True
                    End If
                Next

                If Not found Then
                    tbCommand.Text = ""
                End If

                Dim notRoot = n.Parent IsNot Nothing

                bnSymbol.Enabled = notRoot
                bnCommand.Enabled = notRoot
                tbCommand.Enabled = notRoot
                tbHotkey.Enabled = notRoot
                tsbCopy.Enabled = notRoot
                tsbCut.Enabled = notRoot
                tsbMoveDown.Enabled = notRoot
                tsbMoveLeft.Enabled = notRoot
                tsbMoveRight.Enabled = notRoot
                tsbMoveUp.Enabled = notRoot
                tsbPaste.Enabled = ClipboardNode IsNot Nothing
                tsbRemove.Enabled = notRoot
                tbText.Enabled = notRoot

                Block = False
            End If
        End Sub

        Sub tbText_TextChanged() Handles tbText.TextChanged
            If Not Block AndAlso Not tv.SelectedNode Is Nothing Then
                Dim item = DirectCast(tv.SelectedNode.Tag, CustomMenuItem)
                Dim tbt As Boolean = String.Equals(tbText.Text, "-")

                tbCommand.Enabled = Not tbt
                bnCommand.Enabled = Not tbt
                tbHotkey.Enabled = Not tbt

                If tbt Then
                    tbCommand.Text = ""
                    item.KeyData = Keys.None
                End If

                item.Text = tbText.Text
                tv.SelectedNode.Text = item.Text
            End If
        End Sub

        Sub PopulateGrid(item As CustomMenuItem) 'moved from GetCommand
            Dim cmd As Command = GenericMenu.CommandManager.GetCommand(item.MethodName)
            Dim cPIAr As ParameterInfo() = cmd?.MethodInfo?.GetParameters

            If cPIAr?.Length > 0 Then
                pg.Visible = True
                lParameters.Visible = True
                GridTypeDescriptor.Items.Clear()
                Dim cFixParL As List(Of Object) = cmd?.FixParameters(item.Parameters)

                For i = 0 To cPIAr.Length - 1
                    Dim pI As ParameterInfo = cPIAr(i)
                    Dim cAttr As Object() = pI.GetCustomAttributes(False)
                    GridTypeDescriptor.Add(New GridProperty With {
                            .Name = If(DispNameAttribute.GetValue(cAttr), pI.Name.ToTitleCase),
                            .Value = cFixParL.Item(i),
                            .Description = DescriptionAttributeHelp.GetDescription(cAttr),
                            .TypeEditor = EditorAttributeHelp.GetEditor(cAttr)})
                Next i
            Else
                pg.Visible = False
                lParameters.Visible = False
                GridTypeDescriptor.Items.Clear()
            End If

            pg.SelectedObject = GridTypeDescriptor
        End Sub

        Sub pg_PropertyValueChanged() Handles pg.PropertyValueChanged
            If tv.SelectedNode IsNot Nothing Then
                Dim item = DirectCast(tv.SelectedNode.Tag, CustomMenuItem)
                item.Parameters.Clear()

                For Each i As DictionaryEntry In GridTypeDescriptor.Items
                    item.Parameters.Add(DirectCast(i.Value, GridProperty).Value)
                Next
            End If
        End Sub

        Sub AddNodes(node As TreeNode, list As ArrayList)
            For Each i As TreeNode In node.Nodes
                list.Add(i)
                AddNodes(i, list)
            Next
        End Sub

        Sub PopulateTreeView(item As CustomMenuItem, node As TreeNode)
            Dim newNode As New TreeNode(item.Text) With {.Tag = item}

            If node Is Nothing Then
                tv.Nodes.Add(newNode)
            Else
                node.Nodes.Add(newNode)
            End If

            Dim subItems As List(Of CustomMenuItem) = item.SubItems
            For n = 0 To subItems.Count - 1
                PopulateTreeView(subItems(n), newNode)
            Next n
        End Sub

        Function GetState() As CustomMenuItem
            Dim item = DirectCast(tv.Nodes(0).Tag, CustomMenuItem)
            BuildState(item, tv.Nodes(0))
            Return item
        End Function

        Sub BuildState(item As CustomMenuItem, node As TreeNode)
            item.SubItems.Clear()

            For Each i As TreeNode In node.Nodes
                Dim subItem As CustomMenuItem = CType(i.Tag, CustomMenuItem)
                item.SubItems.Add(subItem)
                BuildState(subItem, i)
            Next
        End Sub

        Protected Overridable Function GetTemplateForm(item As CustomMenuItem) As MenuTemplateForm
            Return New MenuTemplateForm(item)
        End Function

        Sub NewFromDefaultsToolStripMenuItem_Click() Handles NewFromDefaultsToolStripMenuItem.Click
            If tv.SelectedNode IsNot Nothing Then
                tv.BeginUpdate()
                Dim f = GetTemplateForm(GenericMenu.DefaultMenu.Invoke)

                If f.ShowDialog = DialogResult.OK Then
                    Dim n = DirectCast(f.TreeNode.Clone, TreeNode)
                    tv.SelectedNode.Nodes.Add(n)
                    tv.SelectedNode.ExpandAll()
                    tv.SelectedNode = n
                End If
                tv.EndUpdate()
            End If
        End Sub

        Sub RemoveSelectedItem()
            If tv.SelectedNode IsNot Nothing AndAlso tv.SelectedNode.Parent IsNot Nothing Then
                tv.SelectedNode.Remove()
            End If
        End Sub

        Sub tv_KeyUp(sender As Object, e As KeyEventArgs) Handles tv.KeyUp
            UpdateControls()
        End Sub

        Sub tbHotkey_KeyDown(sender As Object, e As KeyEventArgs) Handles tbHotkey.KeyDown
            If Not Block AndAlso tv.SelectedNode IsNot Nothing AndAlso
                Not e.KeyCode = Keys.ControlKey AndAlso Not e.KeyCode = Keys.Menu AndAlso Not e.KeyCode = Keys.ShiftKey Then

                Dim item = DirectCast(tv.SelectedNode.Tag, CustomMenuItem)
                Dim eKD As Keys = e.KeyData

                If item.KeyData = eKD Then
                    item.KeyData = Keys.None
                    tbHotkey.Text = KeysHelp.GetKeyString(item.KeyData)
                Else
                    item.KeyData = eKD
                    tbHotkey.Text = KeysHelp.GetKeyString(item.KeyData)
                    Dim tvnL = tv.GetNodes

                    For i = 0 To tvnL.Count - 1
                        Dim current = TryCast(tvnL(i).Tag, CustomMenuItem)
                        If current IsNot Nothing Then
                            If current.KeyData = eKD AndAlso item IsNot current Then
                                current.KeyData = Keys.None
                                MsgInfo(KeysHelp.GetKeyString(eKD) & " detached from " & current.Text.TrimEnd("."c) & " And assigned To " & item.Text.TrimEnd("."c) & " instead.")
                            End If
                        End If
                    Next i
                End If
            End If

            e.Handled = True
        End Sub

        Sub tbHotkey_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbHotkey.KeyPress
            e.Handled = True
        End Sub

        Sub tv_AfterSelect() Handles tv.AfterSelect
            UpdateControls()
        End Sub

        Sub tsbCut_Click() Handles tsbCut.Click
            If tv.SelectedNode IsNot Nothing Then
                tsbCopy.PerformClick()
                RemoveSelectedItem()
            End If
        End Sub

        Sub tsbCopy_Click() Handles tsbCopy.Click
            If tv.SelectedNode IsNot Nothing Then
                ClipboardNode = DirectCast(ObjectHelp.GetCopy(tv.SelectedNode), TreeNode)
                UpdateControls()
            End If
        End Sub

        Sub tsbPaste_Click() Handles tsbPaste.Click
            If tv.SelectedNode IsNot Nothing Then
                tv.BeginUpdate()
                tv.SelectedNode.Nodes.Add(CType(ObjectHelp.GetCopy(ClipboardNode), TreeNode))
                tv.SelectedNode.ExpandAll()
                tv.EndUpdate()
            End If
        End Sub

        Sub tsbRemove_Click() Handles tsbRemove.Click
            RemoveSelectedItem()
        End Sub

        Sub ResetToolStripMenuItem_Click() Handles ResetToolStripMenuItem.Click
            If MsgOK("Please confirm To reset the entire menu.") Then
                tv.BeginUpdate()
                tv.Nodes.Clear()
                PopulateTreeView(GenericMenu.DefaultMenu.Invoke, Nothing)
                tv.ExpandAll()
                tv.SelectedNode = tv.Nodes(0)
                tv.EndUpdate()
            End If
        End Sub

        Sub tsbMoveLeft_Click() Handles tsbMoveLeft.Click
            Dim n = tv.SelectedNode

            If n IsNot Nothing AndAlso n.Parent.Parent Is Nothing Then Exit Sub

            Block = True
            tv.BeginUpdate()
            tv.MoveSelectionLeft()
            tv.EndUpdate()
            Block = False
        End Sub

        Sub tsbMoveRight_Click() Handles tsbMoveRight.Click
            Block = True
            tv.BeginUpdate()
            tv.MoveSelectionRight()
            tv.EndUpdate()
            Block = False
        End Sub

        Sub tsbMoveUp_Click() Handles tsbMoveUp.Click
            Dim n = tv.SelectedNode

            If n IsNot Nothing AndAlso n.Parent IsNot Nothing AndAlso n.Parent.Parent Is Nothing AndAlso n.Index = 0 Then Exit Sub

            Block = True
            tv.BeginUpdate()
            tv.MoveSelectionUp()
            tv.EndUpdate()
            Block = False
        End Sub

        Sub tsbMoveDown_Click() Handles tsbMoveDown.Click
            Dim n = tv.SelectedNode

            If n IsNot Nothing AndAlso n.Parent.Parent Is Nothing AndAlso n.NextNode Is Nothing Then Exit Sub

            Block = True
            tv.BeginUpdate()
            tv.MoveSelectionDown()
            tv.EndUpdate()
            Block = False
        End Sub

        Sub bCommand_Click() Handles bnCommand.Click
            cmsCommand.Show(bnCommand, 0, bnCommand.Height)
        End Sub

        Sub tbCommand_TextChanged() Handles tbCommand.TextChanged
            If tv.SelectedNode IsNot Nothing Then
                Dim item = DirectCast(tv.SelectedNode.Tag, CustomMenuItem)

                If Not Block Then
                    Dim selectedCommand As Command = Nothing
                    Dim tbCTxt As String = tbCommand.Text

                    For Each i As Command In GenericMenu.CommandManager.Commands.Values
                        If String.Equals(i.MethodInfo.Name, tbCTxt) Then
                            selectedCommand = i
                            Exit For
                        End If
                    Next

                    If selectedCommand Is Nothing OrElse selectedCommand.MethodInfo Is Nothing Then
                        item.MethodName = String.Empty
                        item.Parameters = Nothing
                    Else
                        item.MethodName = selectedCommand.MethodInfo.Name
                        item.Parameters = selectedCommand.GetDefaultParameters
                    End If
                End If

                PopulateGrid(item)
            End If
        End Sub

        Sub tsbNew_Click(sender As Object, e As EventArgs) Handles tsbNew.Click
            If tv.SelectedNode IsNot Nothing Then
                tv.BeginUpdate()
                Dim newNode As New TreeNode With {.Text = "???", .Tag = New CustomMenuItem("???")}
                tv.SelectedNode.Nodes.Add(newNode)
                tv.SelectedNode = newNode
                tv.EndUpdate()
            End If
        End Sub

        Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
            Dim form As New HelpForm()
            hevent.Handled = True
            form.Doc.WriteStart(Text)
            form.Doc.WriteParagraph("The menu editor allows To customize the text, Location, Shortcut key And command of a menu item. Menu items can be rearranged with '''Drag & Drop'''. Pressing Ctrl while dragging moves as sub-item.")
            form.Doc.WriteParagraph("[http://fontawesome.io/cheatsheet FontAwesome icons]")
            form.Doc.WriteParagraph("[https://docs.microsoft.com/en-us/windows/uwp/style/segoe-ui-symbol-font Segoe MDL2 icons]")
            form.Doc.WriteTable("Commands", GenericMenu.CommandManager.GetTips)
            form.Show()
            MyBase.OnHelpRequested(hevent)
        End Sub
    End Class
End Namespace
