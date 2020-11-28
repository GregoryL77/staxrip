Imports System.Reflection
Imports System.Runtime
Imports System.Threading
Imports System.Threading.Tasks
Imports KGySoft.ComponentModel
Imports StaxRip.UI
Public Class AudioConverterForm
    Inherits FormBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If Not (AudioSortableBindingList Is Nothing) Then
                AudioSortableBindingList.Dispose()
            End If
            If Not (AudioBindingSource Is Nothing) Then
                AudioBindingSource.Dispose()
            End If
            If Not (dgvAudio Is Nothing) Then
                dgvAudio.Dispose()
            End If
        End If

        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
    Friend WithEvents tpAudio As TabPage
    Friend WithEvents bnAudioPlay As ButtonEx
    Friend WithEvents bnAudioDown As ButtonEx
    Friend WithEvents bnAudioUp As ButtonEx
    Friend WithEvents bnAudioRemove As ButtonEx
    Friend WithEvents bnAudioAdd As ButtonEx
    Friend WithEvents dgvAudio As DataGridViewEx
    Friend WithEvents bnAudioEdit As ButtonEx
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents pnTab As Panel
    Friend WithEvents bnAudioConvert As ButtonEx
    Friend WithEvents bnMenuAudio As ButtonEx
    Friend WithEvents laAC As Label
    Friend WithEvents flpAudio As FlowLayoutPanel
    Friend WithEvents numThreads As NumEdit
    Friend WithEvents laThreads As Label
    Friend WithEvents bnAudioMediaInfo As ButtonEx
    Friend WithEvents bnCMD As ButtonEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.numThreads = New StaxRip.UI.NumEdit()
        Me.laThreads = New System.Windows.Forms.Label()
        Me.bnAudioRemove = New StaxRip.UI.ButtonEx()
        Me.bnAudioConvert = New StaxRip.UI.ButtonEx()
        Me.bnAudioEdit = New StaxRip.UI.ButtonEx()
        Me.laAC = New System.Windows.Forms.Label()
        Me.bnMenuAudio = New StaxRip.UI.ButtonEx()
        Me.bnAudioMediaInfo = New StaxRip.UI.ButtonEx()
        Me.bnCMD = New StaxRip.UI.ButtonEx()
        Me.bnAudioPlay = New StaxRip.UI.ButtonEx()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpAudio = New System.Windows.Forms.TabPage()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAudio = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAudioAdd = New StaxRip.UI.ButtonEx()
        Me.bnAudioUp = New StaxRip.UI.ButtonEx()
        Me.bnAudioDown = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnTab = New System.Windows.Forms.Panel()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcMain.SuspendLayout()
        Me.tpAudio.SuspendLayout()
        Me.tlpAudio.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.pnTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'numThreads
        '
        Me.numThreads.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.numThreads.BackColor = System.Drawing.SystemColors.Control
        Me.numThreads.Location = New System.Drawing.Point(652, 328)
        Me.numThreads.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.numThreads.Maximum = 16.0R
        Me.numThreads.Minimum = 1.0R
        Me.numThreads.Name = "numThreads"
        Me.numThreads.Size = New System.Drawing.Size(40, 20)
        Me.numThreads.TabIndex = 4
        Me.numThreads.TabStop = False
        Me.numThreads.Tag = "No. threads"
        Me.TipProvider.SetTipText(Me.numThreads, "Number of parallel processes, set the default in settings")
        '
        'laThreads
        '
        Me.laThreads.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laThreads.AutoSize = True
        Me.laThreads.BackColor = System.Drawing.SystemColors.Control
        Me.laThreads.Location = New System.Drawing.Point(592, 328)
        Me.laThreads.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.laThreads.Name = "laThreads"
        Me.laThreads.Size = New System.Drawing.Size(54, 18)
        Me.laThreads.TabIndex = 19
        Me.laThreads.Text = "Threads :"
        Me.laThreads.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.laThreads, "Number of parallel processes, set the default in settings")
        '
        'bnAudioRemove
        '
        Me.bnAudioRemove.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioRemove.Location = New System.Drawing.Point(144, 351)
        Me.bnAudioRemove.Size = New System.Drawing.Size(88, 26)
        Me.bnAudioRemove.Text = "    &Remove"
        Me.TipProvider.SetTipText(Me.bnAudioRemove, "Remove Selection <Delete>")
        '
        'bnAudioConvert
        '
        Me.bnAudioConvert.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioConvert.Location = New System.Drawing.Point(238, 351)
        Me.bnAudioConvert.Size = New System.Drawing.Size(88, 26)
        Me.bnAudioConvert.Text = "      &Convert..."
        Me.TipProvider.SetTipText(Me.bnAudioConvert, "Convert Selection")
        '
        'bnAudioEdit
        '
        Me.bnAudioEdit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioEdit.Location = New System.Drawing.Point(562, 351)
        Me.bnAudioEdit.Size = New System.Drawing.Size(84, 26)
        Me.bnAudioEdit.Text = "    &Edit..."
        Me.TipProvider.SetTipText(Me.bnAudioEdit, "Edit Audio Profile for selection")
        '
        'laAC
        '
        Me.laAC.AutoSize = True
        Me.tlpMain.SetColumnSpan(Me.laAC, 6)
        Me.laAC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.laAC.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.laAC.Location = New System.Drawing.Point(50, 328)
        Me.laAC.Name = "laAC"
        Me.laAC.Size = New System.Drawing.Size(506, 20)
        Me.laAC.TabIndex = 18
        Me.laAC.Text = "Please add or drag music files..."
        Me.TipProvider.SetTipText(Me.laAC, "Double Click to change autosize mode")
        '
        'bnMenuAudio
        '
        Me.bnMenuAudio.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnMenuAudio.Location = New System.Drawing.Point(5, 350)
        Me.bnMenuAudio.Margin = New System.Windows.Forms.Padding(2)
        Me.bnMenuAudio.ShowMenuSymbol = True
        Me.bnMenuAudio.Size = New System.Drawing.Size(40, 28)
        Me.TipProvider.SetTipText(Me.bnMenuAudio, "Click to open menu")
        '
        'bnAudioMediaInfo
        '
        Me.bnAudioMediaInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioMediaInfo.Location = New System.Drawing.Point(652, 351)
        Me.bnAudioMediaInfo.Size = New System.Drawing.Size(84, 26)
        Me.bnAudioMediaInfo.Text = "    &Info..."
        Me.TipProvider.SetTipText(Me.bnAudioMediaInfo, "Media Info")
        '
        'bnCMD
        '
        Me.bnCMD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnCMD.Location = New System.Drawing.Point(742, 351)
        Me.bnCMD.Size = New System.Drawing.Size(84, 26)
        Me.bnCMD.Text = "     C&MD..."
        Me.TipProvider.SetTipText(Me.bnCMD, "Show Command Line")
        '
        'bnAudioPlay
        '
        Me.bnAudioPlay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioPlay.Location = New System.Drawing.Point(472, 351)
        Me.bnAudioPlay.Size = New System.Drawing.Size(84, 26)
        Me.bnAudioPlay.Text = "  &Play"
        '
        'dgvAudio
        '
        Me.dgvAudio.AllowDrop = True
        Me.dgvAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAudio.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2
        Me.dgvAudio.Location = New System.Drawing.Point(0, 0)
        Me.dgvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.Size = New System.Drawing.Size(811, 299)
        Me.dgvAudio.StandardTab = True
        Me.dgvAudio.TabIndex = 1
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tpAudio)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.ItemSize = New System.Drawing.Size(62, 15)
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(823, 324)
        Me.tcMain.TabIndex = 5
        Me.tcMain.TabStop = False
        '
        'tpAudio
        '
        Me.tpAudio.Controls.Add(Me.tlpAudio)
        Me.tpAudio.Location = New System.Drawing.Point(4, 19)
        Me.tpAudio.Margin = New System.Windows.Forms.Padding(2, 2, 2, 0)
        Me.tpAudio.Name = "tpAudio"
        Me.tpAudio.Padding = New System.Windows.Forms.Padding(2, 2, 2, 0)
        Me.tpAudio.Size = New System.Drawing.Size(815, 301)
        Me.tpAudio.TabIndex = 4
        Me.tpAudio.Text = "   Audio   "
        Me.tpAudio.UseVisualStyleBackColor = True
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 2
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.Controls.Add(Me.flpAudio, 1, 0)
        Me.tlpAudio.Controls.Add(Me.dgvAudio, 0, 0)
        Me.tlpAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(2, 2)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 1
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.Size = New System.Drawing.Size(811, 299)
        Me.tlpAudio.TabIndex = 7
        '
        'flpAudio
        '
        Me.flpAudio.AutoSize = True
        Me.flpAudio.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAudio.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpAudio.Location = New System.Drawing.Point(811, 0)
        Me.flpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAudio.Name = "flpAudio"
        Me.flpAudio.Size = New System.Drawing.Size(0, 0)
        Me.flpAudio.TabIndex = 6
        '
        'bnAudioAdd
        '
        Me.bnAudioAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioAdd.Location = New System.Drawing.Point(50, 351)
        Me.bnAudioAdd.Size = New System.Drawing.Size(88, 26)
        Me.bnAudioAdd.Text = "    &Add..."
        '
        'bnAudioUp
        '
        Me.bnAudioUp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioUp.Location = New System.Drawing.Point(332, 351)
        Me.bnAudioUp.Size = New System.Drawing.Size(64, 26)
        Me.bnAudioUp.Text = "  &Up"
        '
        'bnAudioDown
        '
        Me.bnAudioDown.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioDown.Location = New System.Drawing.Point(402, 351)
        Me.bnAudioDown.Size = New System.Drawing.Size(64, 26)
        Me.bnAudioDown.Text = "     &Down"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 10
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.laAC, 1, 1)
        Me.tlpMain.Controls.Add(Me.bnMenuAudio, 0, 2)
        Me.tlpMain.Controls.Add(Me.pnTab, 0, 0)
        Me.tlpMain.Controls.Add(Me.bnAudioRemove, 2, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioConvert, 3, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioUp, 4, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioDown, 5, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioEdit, 7, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioPlay, 6, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioMediaInfo, 8, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioAdd, 1, 2)
        Me.tlpMain.Controls.Add(Me.bnCMD, 9, 2)
        Me.tlpMain.Controls.Add(Me.numThreads, 8, 1)
        Me.tlpMain.Controls.Add(Me.laThreads, 7, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(3, 2, 2, 3)
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(831, 383)
        Me.tlpMain.TabIndex = 2
        Me.tlpMain.TabStop = True
        '
        'pnTab
        '
        Me.tlpMain.SetColumnSpan(Me.pnTab, 10)
        Me.pnTab.Controls.Add(Me.tcMain)
        Me.pnTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnTab.Location = New System.Drawing.Point(5, 4)
        Me.pnTab.Margin = New System.Windows.Forms.Padding(2, 2, 1, 0)
        Me.pnTab.Name = "pnTab"
        Me.pnTab.Size = New System.Drawing.Size(823, 324)
        Me.pnTab.TabIndex = 8
        '
        'AudioConverterForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(831, 383)
        Me.Controls.Add(Me.tlpMain)
        Me.DoubleBuffered = True
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "AudioConverterForm"
        Me.Text = "Audio Converter"
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcMain.ResumeLayout(False)
        Me.tpAudio.ResumeLayout(False)
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.pnTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private ReadOnly AudioBindingSource As New BindingSource
    Private ReadOnly cms As ContextMenuStripEx
    Private OutPath As String
    Private AutoStream As Boolean
    Private LogHeader As Boolean
    Public Shared MaxThreads As Integer
    Private ReadOnly AudioSortableBindingList As New SortableBindingList(Of AudioProfile)(ObjectHelp.GetCopy(p.AudioTracks))
    'Private AudioOBL As New KGySoft.ComponentModel.ObservableBindingList(Of AudioProfile)(ObjectHelp.GetCopy(p.AudioTracks))


    Public Sub New()
        MyBase.New()
        'AddHandler Application.ThreadException, AddressOf g.OnUnhandledException
        InitializeComponent()
        SetMinimumSize(44, 15)
        RestoreClientSize(53, 29)

        'AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks)
        AudioBindingSource.DataSource = AudioSortableBindingList

        'var myBindingSource = New SortedBindingList < MyType > (myCollection);
        'myBindingSource.ApplySort(propertyName, ListSortDirection.Ascending);
        'dataGridView1.DataSource = myBindingSource;


        dgvAudio.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0!, FontStyle.Bold)
        dgvAudio.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvAudio.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
        dgvAudio.RowHeadersDefaultCellStyle.Padding = New Padding(0)
        dgvAudio.RowHeadersVisible = True
        dgvAudio.RowTemplate.Height = Font.Height + 4 '*1.25 or +4 AutoResize=20?
        dgvAudio.RowHeadersWidth = 70 '64
        dgvAudio.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        dgvAudio.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing
        dgvAudio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvAudio.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAudio.AllowDrop = True
        dgvAudio.MultiSelect = True
        dgvAudio.AutoGenerateColumns = False
        dgvAudio.AllowUserToResizeRows = False
        dgvAudio.AllowUserToDeleteRows = False
        dgvAudio.AllowUserToOrderColumns = True
        dgvAudio.AllowUserToResizeColumns = True

        dgvAudio.DataSource = AudioBindingSource
        'dgvAudio.DataSource = AudioSortableBindingList

        'DoubleBuffered DGV -  responsivness
        GetType(DataGridViewEx).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or
            BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, dgvAudio, New Object() {True})

        bnAudioAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnAudioRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnAudioPlay.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnAudioUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnAudioDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnAudioEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)
        bnAudioConvert.Image = ImageHelp.GetSymbolImage(Symbol.MusicInfo)
        bnAudioMediaInfo.Image = ImageHelp.GetSymbolImage(Symbol.Info)
        bnCMD.Image = ImageHelp.GetSymbolImage(Symbol.CommandPrompt)

        For Each bn In {bnAudioAdd, bnAudioRemove, bnAudioPlay, bnAudioUp,
                        bnAudioDown, bnAudioEdit, bnAudioConvert, bnAudioMediaInfo, bnCMD}

            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        Dim profileName = dgvAudio.AddTextBoxColumn()
        profileName.DataPropertyName = "Name"
        profileName.HeaderText = "Profile"
        profileName.ReadOnly = True
        profileName.FillWeight = 10

        Dim dispNameColumn = dgvAudio.AddTextBoxColumn()
        dispNameColumn.DataPropertyName = "DisplayName"
        dispNameColumn.HeaderText = "Track"
        dispNameColumn.ReadOnly = True
        dispNameColumn.FillWeight = 40

        Dim pathColumn = dgvAudio.AddTextBoxColumn()
        pathColumn.DataPropertyName = "File"
        pathColumn.HeaderText = "Full Path"
        pathColumn.ReadOnly = True
        pathColumn.FillWeight = 50

        'Dim CColumn = dgvAudio.AddTextBoxColumn() 'debug
        'CColumn.DataPropertyName = "CommandLines"
        'CColumn.HeaderText = "CMD Lines"
        'CColumn.FillWeight = 51

        For Each col As DataGridViewColumn In dgvAudio.Columns
            col.SortMode = DataGridViewColumnSortMode.Automatic
            'col.HeaderCell.ToolTipText = "F3 to sort"
        Next

        dgvAudio.Sort(dgvAudio.Columns(2), ComponentModel.ListSortDirection.Ascending)
        'AudioSortableBindingList.ApplySort(System.ComponentModel.ListSortDirection.Ascending)

        If MaxThreads = 0 Then MaxThreads = s.ParallelProcsNum
        numThreads.Value = MaxThreads
        cms = New ContextMenuStripEx(components)
        bnMenuAudio.ContextMenuStrip = cms
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        AddHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        bnMenuAudio_Click(Me, e)
        UpdateControls()
        Refresh()
        bnAudioAdd.Select()
    End Sub
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        MyBase.OnFormClosing(e)
    End Sub
    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        SaveConverterLog()
        cms.Items.ClearAndDisplose
        AudioSortableBindingList.Clear()
        'AudioSortableBindingList.Dispose()
        'AudioBindingSource.Dispose()
        AudioBindingSource.Clear()
        dgvAudio.Rows.Clear()
        dgvAudio.Columns.Clear()
        'dgvAudio.Dispose()
        Dispose(True)
    End Sub

    Private Sub bnMenuAudio_Click(sender As Object, e As EventArgs) Handles bnMenuAudio.Click
        cms.SuspendLayout()
        cms.Items.ClearAndDisplose
        Dim rExist = dgvAudio.Rows.Count > 0
        Dim rSel = rExist AndAlso dgvAudio.SelectedRows.Count > 0
        Dim ap0 As AudioProfile
        If rSel Then ap0 = AudioSortableBindingList(dgvAudio.SelectedRows(0).Index)
        Dim logNotEmpty As Boolean = Not Log.IsEmpty

        cms.Add("Select all  <Ctrl+A>", Sub() dgvAudio.SelectAll(), rExist).SetImage(Symbol.SelectAll)
        cms.Add("Remove all", Sub()
                                  AudioSortableBindingList.Clear()
                                  AudioBindingSource.Clear()
                                  dgvAudio.Rows.Clear()
                              End Sub, rExist).SetImage(Symbol.Clear)

        cms.Add("Show Source File", Sub()
                                        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.SelectedRows(0).Index
                                        g.SelectFileWithExplorer(ap0.File)
                                    End Sub, rSel AndAlso FileExists(ap0.File), "Open the source file in File Explorer.").SetImage(Symbol.FileExplorer)

        cms.Add("Show Ouput Folder", Sub() g.SelectFileWithExplorer(OutPath), DirExists(OutPath), "Open output folder in File Explorer.").SetImage(Symbol.FileExplorerApp)

        'cms.Add("Show Output File", Sub() g.SelectFileWithExplorer(OutPath & RelativeSubDirRecursive(ap0.File.Dir, 0) & ap0.GetOutputFile),
        'rSel AndAlso FileExists(OutPath & RelativeSubDirRecursive(ap0.File.Dir, 0) & ap0.GetOutputFile), "Open converted file in File explerer").SetImage(Symbol.ShowResults)

        cms.Add("Show LOG", Sub()
                                SaveConverterLog()
                                g.SelectFileWithExplorer(p.Log.GetPath)
                            End Sub, logNotEmpty, "Open current log in File Explorer").SetImage(Symbol.fa_folder_open_o)

        cms.Add("Save LOG...", Sub() SaveConverterLog(UseDialog:=True), logNotEmpty).SetImage(Symbol.Save)
        cms.Add("-")
        cms.Add("qaac Help", Sub() Package.qaac.ShowHelp())
        cms.Add("qaac Formats", Sub() MsgInfo(ProcessHelp.GetConsoleOutput(Package.qaac.Path, "--formats")))
        cms.Add("Opus Help", Sub() Package.OpusEnc.ShowHelp())
        cms.Add("ffmpeg Help", Sub() Package.ffmpeg.ShowHelp())
        cms.Add("eac3to Help", Sub() g.ShellExecute("http://en.wikibooks.org/wiki/Eac3to"))
        cms.ResumeLayout()
    End Sub

    Public Function OpenAudioConverterDialog() As DialogResult 'Debug
        Using form As New AudioConverterForm
            Return form.ShowDialog()
        End Using
    End Function

    Public Sub UpdateControls()
        Dim srC As Integer = dgvAudio.SelectedRows.Count
        If dgvAudio.Rows.Count > 0 Then
            laAC.Text = "Pos: " & (AudioBindingSource.Position + 1).ToString & "   |   Sel: " & srC & " / Tot: " & dgvAudio.Rows.Count
            'debug
            'Dim sr = dgvAudio.SelectedRows
            'laAC.Text = "B: " & (AudioBindingSource.Position + 1).ToString & " C: " & (dgvAudio.CurrentRow.Index + 1).ToString & " SR0: " & (sr(0).Index + 1).ToString & " SRL: " & (sr(sr.Count - 1).Index + 1).ToString & " | Sel: " & srC & " / Tot: " & dgvAudio.Rows.Count
        Else
            laAC.Text = "Please add or drop music files..."
        End If

        bnAudioPlay.Enabled = srC > 0
        bnAudioMediaInfo.Enabled = srC > 0
        bnAudioRemove.Enabled = srC > 0
        bnAudioEdit.Enabled = srC > 0
        bnCMD.Enabled = srC > 0
        bnAudioConvert.Enabled = srC > 0
        bnAudioUp.Enabled = srC = 1 AndAlso dgvAudio.CanMoveUp
        bnAudioDown.Enabled = srC = 1 AndAlso dgvAudio.CanMoveDown
        numThreads.Enabled = srC > 1
    End Sub

    Public Sub StatusText(InfoText As String)
        laAC.Text = InfoText
        laAC.Refresh()
    End Sub

    Public Sub AutoSizeColumns()
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        RemoveHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
        StatusText("Auto Resizing Columns...")
        IndexHeaderRows()
        'dgvAudio.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)

        If dgvAudio.AutoSizeColumnsMode <> DataGridViewAutoSizeColumnsMode.Fill Then
            dgvAudio.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        End If

        dgvAudio.AutoResizeColumnHeadersHeight()
        UpdateControls()
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        AddHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
    End Sub
    Private Sub IndexHeaderRows()
        For Each r As DataGridViewRow In dgvAudio.Rows
            r.HeaderCell.Value = (r.Index + 1).ToString
        Next
    End Sub
    Private Sub dgvAudio_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseUp
        If e.RowIndex = -1 AndAlso e.ColumnIndex > -1 AndAlso dgvAudio.Rows.Count > 0 Then
            IndexHeaderRows()
            UpdateControls()
            AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
            AddHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
        End If
    End Sub

    Private Sub dgvAudio_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseClick
        If e.RowIndex = -1 AndAlso e.ColumnIndex > -1 AndAlso dgvAudio.Rows.Count > 0 Then
            StatusText("Sorting...")
            RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
            RemoveHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
        End If
    End Sub
    Private Sub dgvAudio_SelectionChanged(sender As Object, e As EventArgs)
        UpdateControls()
    End Sub

    Public Sub SaveConverterLog(Optional LogPath As String = "", Optional UseDialog As Boolean = False)
        If Not Log.IsEmpty Then
            Log.Save()
            If UseDialog Then
                Using dialog As New FolderBrowserDialog
                    dialog.Description = "Please select LOG File location :"
                    dialog.UseDescriptionForTitle = True
                    dialog.RootFolder = Environment.SpecialFolder.MyComputer
                    If dialog.ShowDialog = DialogResult.OK Then
                        LogPath = dialog.SelectedPath.FixDir
                    End If
                End Using
            End If

            If LogPath <> "" Then
                Dim LogName = Date.Now.ToString("yy-MM-dd_HH.mm.ss") & "_staxrip.log"
                Try
                    FileHelp.Move(Log.GetPath, LogPath & LogName)
                Catch ex As Exception
                Finally
                    If UseDialog AndAlso Not g.FileExists(LogPath & LogName) Then
                        MsgWarn("Something went wrong. Locate log file manually or use Main Window's Tools menu. ")
                    End If
                End Try
            End If

        ElseIf UseDialog Then
            MsgInfo("Log is empty")
        End If
    End Sub

    Public Function RelativeSubDirRecursive(path As String, Optional SubDepth As Integer = 1) As String
        If SubDepth < 1 Then Return ""

        If path.Length <= 3 Then
            For Each ch In path
                If "/:\^".Contains(ch) Then
                    path = path.Replace(ch.ToString, "")
                End If
            Next
            Return path.FixDir
        End If

        Dim parentDir = path
        For i = 1 To SubDepth
            If parentDir.Length > 3 AndAlso DirExists(parentDir) Then
                parentDir = parentDir.Parent
            Else
                Exit For
            End If
        Next
        Return path.Replace(parentDir, "").FixDir
    End Function

    Public Sub EncodeAudio(ap As AudioProfile, Optional SubDepth As Integer = 1)
        p.TempDir = OutPath
        Dim outP As String = ap.GetOutputFile()
        'Dim inFn As String = ap.File.FileName
        Dim nOutD As String = OutPath & RelativeSubDirRecursive(ap.File.Dir, SubDepth)
        ap = ObjectHelp.GetCopy(ap)
        Audio.Process(ap)
        ap.Encode()

        If SubDepth > 0 AndAlso outP.Dir <> nOutD AndAlso FileExists(outP) Then
            Try
                FolderHelp.Create(nOutD)
                Thread.Sleep(20)
                FileHelp.Move(outP, nOutD & outP.FileName)    '     ap.File.FileName
            Catch ex As Exception
                Log.Write("Audio Converter IO copy exception", ex.ToString)
                'Throw New ErrorAbortException("Failed To create output file ", "Failed To create output file Or path: " & nOutD & outP)
            End Try

            Thread.Sleep(100)
            If Not DirExists(nOutD) Then
                Log.Write("Audio Converter error", "Failed to create output directory: " & nOutD)
            Else
                If Not FileExists(nOutD & outP.FileName) Then
                    Log.Write("Audio Converter error", "Failed to create output file in path: " & nOutD & outP.FileName)
                End If
            End If

        Else
            If Not FileExists(outP) Then
                Log.Write("Audio Converter error", "Failed to create output file: " & outP)
                'Throw New ErrorAbortException("Failed to create output file ", "Failed to create output file: " & outP)
            End If
        End If
    End Sub

    Public Sub bnAudioConvert_Click(sender As Object, e As EventArgs) Handles bnAudioConvert.Click
        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select output directory :"
            dialog.UseDescriptionForTitle = True
            dialog.RootFolder = Environment.SpecialFolder.MyComputer
            If dialog.ShowDialog <> DialogResult.OK Then Exit Sub
            If dialog.SelectedPath = "" OrElse Not DirExists(dialog.SelectedPath.FixDir) Then
                Exit Sub
            End If
            OutPath = dialog.SelectedPath.FixDir
        End Using

        p.TempDir = OutPath

        For Each sr As DataGridViewRow In dgvAudio.SelectedRows
            'If (DirectCast(AudioBindingSource(dgvAudio.Rows(sr.Index).Index), AudioProfile).File.Dir).Contains(OutPath) Then
            If AudioSortableBindingList(dgvAudio.Rows(sr.Index).Index).File.Dir.Contains(OutPath) Then
                If MsgQuestion("One of source files is inside output directory.  Are you sure ?", TaskDialogButtons.YesNo) <> DialogResult.Yes Then
                    Exit Sub
                End If
                Exit For
            End If
        Next

        Dim subDepth As Integer
        Using td As New TaskDialog(Of Integer)
            td.MainInstruction = "How many source Sub Dirs use ?"
            td.AddCommand("0 - All files in output dir", 0)
            For i = 1 To 5
                td.AddButton(i.ToString, i)
            Next
            td.SelectedValue = -1
            subDepth = td.Show
            If subDepth < 0 Then Exit Sub
        End Using

        Dim srC = dgvAudio.SelectedRows.Count
        If MsgQuestion("Confirm to convert selected: " & srC & BR & "To path: " & OutPath & BR & "Sub Dirs: " & subDepth) = DialogResult.OK Then
            Try 'is needed ???
                Dim StartTime = Date.Now
                StatusText("Converting...")
                If s.PreventStandby Then PowerRequest.SuppressStandby()
                ProcController.BlockActivation = True
                If Log.IsEmpty Then Log.WriteEnvironment()
                If Not LogHeader Then
                    Log.WriteHeader("Audio Converter")
                    LogHeader = True
                End If
                Log.WriteHeader("Audio Converter queue started: " & Date.Now.ToString)
                Dim actions As New List(Of Action)

                For Each sr As DataGridViewRow In dgvAudio.SelectedRows
                    'Dim srbs As Object = AudioBindingSource(dgvAudio.Rows(sr.Index).Index)
                    'Dim ap As AudioProfile = DirectCast(srbs, AudioProfile);
                    Dim ap As AudioProfile = AudioSortableBindingList(dgvAudio.Rows(sr.Index).Index)

                    If FileExists(ap.File) AndAlso My.Computer.FileSystem.GetFileInfo(ap.File).Length > 10 AndAlso DirExists(OutPath) Then
                        'EncodeAudio(DirectCast(srbs, AudioProfile), OutPath) STA debug
                        actions.Add(Sub() EncodeAudio(ap, subDepth))
                    End If
                Next

                Try
                    Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = MaxThreads}, actions.ToArray)
                Catch ex As AggregateException
                    Log.Write("Audio Converter encode aggr. exceptions", ex.ToString)
                    'ExceptionDispatchInfo.Capture(ex.InnerExceptions(0)).Throw()
                End Try

                StatusText("Checking results...")
                Task.Delay(1200).Wait()
                Show()
                WindowState = FormWindowState.Normal
                BringToFront()
                Activate()
                dgvAudio.Refresh()
                Refresh()
                Task.Delay(100).Wait()

                Dim OKCount As Integer
                For Each sr As DataGridViewRow In dgvAudio.SelectedRows
                    'Dim srbs As Object = AudioBindingSource(dgvAudio.Rows(sr.Index).Index)
                    'Dim ap As AudioProfile = DirectCast(srbs, AudioProfile)
                    Dim ap As AudioProfile = AudioSortableBindingList(dgvAudio.Rows(sr.Index).Index)
                    Dim outFile = OutPath & RelativeSubDirRecursive(ap.File.Dir, subDepth) & ap.GetOutputFile.FileName() 'ap.file.filename

                    If FileExists(outFile) AndAlso My.Computer.FileSystem.GetFileInfo(outFile).Length > 10 Then
                        sr.HeaderCell.Value = "OK"
                        OKCount += 1
                    Else
                        sr.HeaderCell.Value = "Error"
                    End If
                Next

                Log.Write("Conversion Complete", OKCount & " files out of " & dgvAudio.SelectedRows.Count & " converted succesfully")
                Log.WriteStats(StartTime)

            Catch ex As Exception
                Log.Write("Audio Converter exceptions", ex.ToString)
                'ProcController.Skip()
                ProcController.Abort()
                g.ShowException(ex, Nothing, Nothing, 60)
            Finally
                If s.PreventStandby Then PowerRequest.EnableStandby()
                ProcController.BlockActivation = False
                SaveConverterLog(OutPath)
            End Try

            'dgvAudio.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders)
            UpdateControls()
            bnAudioRemove.Select()
        End If
    End Sub

    Private Sub AddAudio(path As String, ap As AudioProfile)
        ap = ObjectHelp.GetCopy(ap)
        ap.File = path

        If FileTypes.VideoAudio.Contains(ap.File.Ext) Then
            ap.Streams = MediaInfo.GetAudioStreams(ap.File)
            If ap.Streams.Count > 0 Then
                ap.Stream = ap.Streams(0)
            Else
                If Log.IsEmpty Then Log.WriteEnvironment()
                If Not LogHeader Then
                    Log.WriteHeader("Audio Converter")
                    LogHeader = True
                End If
                Log.WriteLine("Audio stream not found, skipping: " & ap.File)
                Exit Sub
            End If

            If Not AutoStream AndAlso ap.Stream IsNot Nothing AndAlso ap.Streams.Count > 1 Then
                Dim streamSelection As New SelectionBox(Of AudioStream)
                Dim NullDummyStream As New AudioStream With {.Index = 29876}
                streamSelection.Title = "Stream Selection"
                streamSelection.Text = "Please select an audio stream for: " & BR & ap.File.ToString.ShortBegEnd(60, 60)

                For Each stream In ap.Streams
                    streamSelection.AddItem(stream)
                Next

                streamSelection.AddItem(" > Use first stream and don't ask me again <", NullDummyStream)

                If streamSelection.Show <> DialogResult.OK Then
                    Exit Sub
                End If

                If streamSelection.SelectedValue IsNot NullDummyStream Then
                    ap.Stream = streamSelection.SelectedValue
                Else
                    AutoStream = True
                End If
            End If
        End If

        'AudioBindingSource.Add(ap)
        AudioSortableBindingList.Add(ap)
    End Sub

    Private Sub ProcessInputAudioFiles(ByRef files As String())
        If files.Count > 300 Then
            If MsgQuestion("Add " & files.Count & " files ?") <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        If files.Count > 0 Then
            RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
            RemoveHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
            Dim OldLastIDX = If(dgvAudio.Rows.Count > 0, dgvAudio.Rows(dgvAudio.Rows.Count - 1).Index, 0)
            Array.Sort(files)
            Dim profileSelection As New SelectionBox(Of AudioProfile)
            profileSelection.Title = "Please select Audio Profile"

            If Not p.Audio0.IsNullProfile AndAlso Not p.Audio0.IsMuxProfile Then
                p.Audio0.SourceSamplingRate = 0
                profileSelection.AddItem("Current Project 1: " & p.Audio0.ToString, p.Audio0)
            End If
            If Not p.Audio1.IsNullProfile AndAlso Not p.Audio1.IsMuxProfile Then
                p.Audio1.SourceSamplingRate = 0
                profileSelection.AddItem("Current Project 2: " & p.Audio1.ToString, p.Audio1)
            End If
            For Each AudioProf In s.AudioProfiles
                If AudioProf.IsMuxProfile OrElse AudioProf.IsNullProfile Then Continue For
                AudioProf.SourceSamplingRate = 0
                profileSelection.AddItem(AudioProf)
            Next

            'SelectionBoxForm.StartPosition = FormStartPosition.CenterParent      'Drag Drop  out of center
            If profileSelection.Show <> DialogResult.OK Then Exit Sub

            StatusText("Adding Files...")

            If dgvAudio.Rows.Count > 0 Then
                For Each col As DataGridViewColumn In dgvAudio.Columns
                    col.HeaderCell.SortGlyphDirection = SortOrder.None
                Next
            Else
                dgvAudio.Columns(0).HeaderCell.SortGlyphDirection = SortOrder.None
                dgvAudio.Columns(1).HeaderCell.SortGlyphDirection = SortOrder.None
                dgvAudio.Columns(2).HeaderCell.SortGlyphDirection = SortOrder.Ascending
            End If

            Dim ap As AudioProfile = profileSelection.SelectedValue
            AutoStream = False


            ap = ObjectHelp.GetCopy(ap)
            If TypeOf ap Is GUIAudioProfile Then
                ap.Name = ap.DefaultName
            End If

            For Each path In files
                AddAudio(path, ap)
            Next

            AudioBindingSource.Position = AudioBindingSource.Count - 1
            'g.MainForm.UpdateSizeOrBitrate()
            If dgvAudio.Rows.Count > 300 Then dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            AutoSizeColumns()
            dgvAudio.Select()
            If dgvAudio.Rows.Count > 0 Then dgvAudio.Rows(OldLastIDX).Selected = True
        End If
    End Sub

    Private Sub bnAudioAdd_Click(sender As Object, e As EventArgs) Handles bnAudioAdd.Click
        Using td As New TaskDialog(Of String)
            Dim ftav = FileTypes.Audio.Union(FileTypes.VideoAudio)
            td.AddCommand("Add files", "files")
            td.AddCommand("Add folder", "folder")

            Select Case td.Show
                Case "files"
                    Using dialog As New OpenFileDialog
                        dialog.Multiselect = True
                        dialog.SetFilter(ftav)
                        If dialog.ShowDialog = DialogResult.OK Then
                            Dim av = dialog.FileNames.Where(Function(file) ftav.Contains(file.Ext))
                            ProcessInputAudioFiles(av.ToArray)
                        End If
                    End Using

                Case "folder"
                    Using dialog As New FolderBrowserDialog
                        dialog.RootFolder = Environment.SpecialFolder.MyComputer
                        If dialog.ShowDialog = DialogResult.OK Then
                            Dim subfolders = Directory.GetDirectories(dialog.SelectedPath)
                            Dim opt = SearchOption.TopDirectoryOnly

                            If Directory.GetDirectories(dialog.SelectedPath).Count > 0 Then
                                If MsgQuestion("Include sub folders?", TaskDialogButtons.YesNo) = DialogResult.Yes Then
                                    opt = SearchOption.AllDirectories
                                End If
                            End If

                            Dim avd = Directory.GetFiles(dialog.SelectedPath, "*.*", opt).Where(Function(val) ftav.Contains(val.Ext))
                            ProcessInputAudioFiles(avd.ToArray)
                        End If
                    End Using
            End Select
        End Using
    End Sub
    Private Sub dgvAudio_DragEnter(sender As Object, e As DragEventArgs) Handles dgvAudio.DragEnter
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            If FileTypes.VideoAudio.Union(FileTypes.Audio).ContainsAny(files.Select(Function(item) item.Ext)) Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        End If
    End Sub
    Private Sub dgvAudio_DragDrop(sender As Object, e As DragEventArgs) Handles dgvAudio.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            Dim ftav = FileTypes.Audio.Union(FileTypes.VideoAudio)
            Dim av = files.Where(Function(file) ftav.Contains(file.Ext))
            ProcessInputAudioFiles(av.ToArray)
        End If
    End Sub

    Private Sub bnAudioEdit_Click(sender As Object, e As EventArgs) Handles bnAudioEdit.Click
        Dim sr = dgvAudio.SelectedRows
        Dim ap0 = AudioSortableBindingList(sr(0).Index)
        'Dim ap0 = DirectCast(AudioBindingSource(sr(0).Index), AudioProfile)
        Dim OldLang = ap0.Language
        dgvAudio.FirstDisplayedScrollingRowIndex = sr(0).Index
        laAC.Text = "Editing: " & (sr(0).Index + 1).ToString & "   |   Sel: " & sr.Count & " / Tot: " & dgvAudio.Rows.Count
        Refresh()

        If ap0.EditAudioConv() = DialogResult.OK Then
            RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
            RemoveHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
            'g.MainForm.UpdateAudioMenu()
            'g.MainForm.UpdateSizeOrBitrate()
            StatusText("Applying settings...")

            If TypeOf ap0 Is GUIAudioProfile Then
                ap0.Name = ap0.DefaultName
            End If

            Dim ap0NoLangChange As Boolean = ap0.Language Is OldLang
            Dim ap0DelayNonZero As Boolean = ap0.Delay <> 0

            If sr.Count > 1 Then
                For i = 1 To sr.Count - 1
                    'Dim ap = DirectCast(AudioBindingSource(sr(i).Index), AudioProfile)
                    Dim ap = AudioSortableBindingList(sr(i).Index)
                    ap0 = ObjectHelp.GetCopy(ap0)
                    ap0.File = ap.File
                    If ap0NoLangChange Then ap0.Language = ap.Language 'needed?
                    ap0.StreamName = ap.StreamName
                    ap0.Stream = ap.Stream
                    ap0.Streams = ap.Streams

                    If ap0DelayNonZero Then
                        ap0.Delay = ap.Delay  'needed?
                    Else
                        ap0.Delay = 0
                    End If

                    ap0.SourceSamplingRate = ap.SourceSamplingRate
                    'AudioBindingSource(sr(i).Index) = ap0
                    AudioSortableBindingList(sr(i).Index) = ap0
                Next
            End If

            StatusText("Restoring selection...")
            Dim srCache(dgvAudio.Rows.Count - 1) As Boolean
            For Each oldSR As DataGridViewRow In sr
                srCache(oldSR.Index) = True
            Next

            ' AudioBindingSource.ResetBindings(False)
            AudioSortableBindingList.ResetBindings()

            If dgvAudio.CurrentRow IsNot Nothing Then dgvAudio.CurrentRow.Selected = False
            If AudioBindingSource.Position >= 0 AndAlso AudioBindingSource.Position < dgvAudio.Rows.Count Then
                dgvAudio.Rows(AudioBindingSource.Position).Selected = False
            End If

            For Each nRow As DataGridViewRow In dgvAudio.Rows
                If srCache(nRow.Index) = True Then
                    dgvAudio.Rows(nRow.Index).Selected = True
                End If
            Next

            If dgvAudio.Rows.Count < 300 Then
                AutoSizeColumns()
            Else
                IndexHeaderRows()
                UpdateControls()
                AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
                AddHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
            End If
        End If
        UpdateControls()
    End Sub

    Private Sub bnAudioRemove_Click(sender As Object, e As EventArgs) Handles bnAudioRemove.Click
        DeleteRows()
    End Sub
    Private Sub dgvAudio_KeyUp(sender As Object, e As KeyEventArgs) Handles dgvAudio.KeyUp
        If dgvAudio.Rows.Count > 0 Then
            If e.KeyCode = Keys.F3 Then
                UpdateControls()
                IndexHeaderRows()
                AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
                AddHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
            End If
        End If
    End Sub

    Private Sub dgvAudio_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvAudio.KeyDown
        If dgvAudio.Rows.Count > 0 Then
            If e.KeyCode = Keys.Delete Then
                DeleteRows()
                e.Handled = True
            End If

            If e.KeyCode = Keys.F3 Then
                StatusText("Sorting...")
                RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
                RemoveHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
                Thread.Sleep(150)
            End If
        End If
    End Sub

    Public Sub DeleteRows()
        StatusText("Removing...")
        If dgvAudio.Rows.Count = dgvAudio.SelectedRows.Count Then
            AudioSortableBindingList.Clear()
            AudioBindingSource.Clear()
            dgvAudio.Rows.Clear()
            UpdateControls()
            Exit Sub
        End If

        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        RemoveHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
        dgvAudio.RemoveSelection()

        If dgvAudio.Rows.Count < 300 Then
            AutoSizeColumns()
        Else
            IndexHeaderRows()
            UpdateControls()
            AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
            AddHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
        End If
    End Sub

    Private Sub bnAudioUp_Click(sender As Object, e As EventArgs) Handles bnAudioUp.Click
        RemoveHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        dgvAudio.MoveSelectionUp
        IndexHeaderRows()
        UpdateControls()
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        AddHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
    End Sub
    Private Sub bnAudioDown_Click(sender As Object, e As EventArgs) Handles bnAudioDown.Click
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        RemoveHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
        dgvAudio.MoveSelectionDown
        IndexHeaderRows()
        UpdateControls()
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        AddHandler dgvAudio.CurrentCellChanged, AddressOf dgvAudio_SelectionChanged
    End Sub
    Private Sub bnAudioPlay_Click(sender As Object, e As EventArgs) Handles bnAudioPlay.Click
        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.SelectedRows(0).Index
        g.Play(AudioSortableBindingList(dgvAudio.SelectedRows(0).Index).File)
    End Sub
    Private Sub bnAudioMediaInfo_Click(sender As Object, e As EventArgs) Handles bnAudioMediaInfo.Click
        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.SelectedRows(0).Index
        g.DefaultCommands.ShowMediaInfo(AudioSortableBindingList(dgvAudio.SelectedRows(0).Index).File)
    End Sub

    Private Sub bnCMD_Click(sender As Object, e As EventArgs) Handles bnCMD.Click
        Dim ap = AudioSortableBindingList(dgvAudio.SelectedRows(0).Index)
        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.SelectedRows(0).Index

        Select Case ap.GetType
            Case GetType(GUIAudioProfile)
                g.ShowCommandLinePreview("Command Line", (TryCast(ap, GUIAudioProfile)?.GetCommandLine(True)))
            Case GetType(BatchAudioProfile)
                g.ShowCommandLinePreview("Command Line", (TryCast(ap, BatchAudioProfile)?.GetCode))
        End Select
    End Sub

    Public Shared Function GetCPUCount() As Integer
        Try
            Dim val = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS")
            Return If(val.IsInt, val.ToInt, s.ParallelProcsNum)
        Catch ex As Exception
            Return s.ParallelProcsNum
        End Try
    End Function
    Private Sub numThreads_ValueChanged(numEdit As NumEdit) Handles numThreads.ValueChanged
        Static CPUCount As Integer = GetCPUCount()
        MaxThreads = CInt(numThreads.Value)
        If numThreads.Value > CPUCount Then
            numThreads.SetColor(Color.Red)
        Else
            numThreads.SetColor(Color.CadetBlue)
        End If
    End Sub
    Private Sub laThreads_DoubleClick(sender As Object, e As EventArgs) Handles laThreads.DoubleClick
        numThreads.Value = s.ParallelProcsNum
    End Sub
    Private Sub laAC_DoubleClick(sender As Object, e As EventArgs) Handles laAC.DoubleClick
        If dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None Then
            dgvAudio.Columns(0).FillWeight = 10
            dgvAudio.Columns(1).FillWeight = 40
            dgvAudio.Columns(2).FillWeight = 50
            dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ElseIf dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill Then
            dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        End If
        AutoSizeColumns()
    End Sub
    Private Sub tcMain_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles tcMain.MouseDoubleClick 'Debug

        'debug
        dgvAudio.Refresh()
        Refresh()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        dgvAudio.SuspendLayout()
        SuspendLayout()
        AudioSortableBindingList.ResetBindings()
        AudioBindingSource.ResetBindings(True)
        Task.Delay(100).Wait()
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(2, GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()
        Thread.Sleep(100)
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        dgvAudio.ResumeLayout(True)
        ResumeLayout(True)
        dgvAudio.Refresh()
        Refresh()
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(2, GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()
        Thread.Sleep(100)
        dgvAudio.Refresh()
        Refresh()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Thread.Sleep(100)

        AudioSortableBindingList.ResetBindings()
        AudioBindingSource.ResetBindings(True)
        AutoSizeColumns()
        dgvAudio.Refresh()
        Refresh()
        dgvAudio.SelectAll()
    End Sub

End Class
