Imports System.Collections.Concurrent
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Diagnostics.Tracing
Imports System.Reflection
Imports System.Runtime
Imports System.Threading
Imports System.Threading.Tasks
Imports KGySoft.Collections
Imports KGySoft.Collections.ObjectModel
Imports KGySoft.ComponentModel
Imports KGySoft.CoreLibraries
Imports Microsoft.VisualBasic
Imports StaxRip.UI

Public Class AudioConverterForm
    Inherits FormBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        PopulateTaskS = True
        PopulateIter = 303
        IndexSWatch.Stop()
        PopulateSWatch.Stop()
        SW3.Stop()
        If Not (AudioSBL Is Nothing) Then
            AudioSBL.Clear()
            AudioSBL.Dispose()
        End If
        If Not (AudioCL Is Nothing) Then
            AudioCL.Reset()
        End If

        If disposing Then
            If Not (dgvAudio Is Nothing) Then
                dgvAudio.Dispose()
            End If
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If

        If Not (CMS Is Nothing) Then
            CMS.Items.ClearAndDisplose
            CMS.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents laThreads As Label
    Friend WithEvents numThreads As NumEdit
    Friend WithEvents bnCMD As ButtonEx
    Friend WithEvents bnAudioAdd As ButtonEx
    Friend WithEvents bnAudioMediaInfo As ButtonEx
    Friend WithEvents bnAudioPlay As ButtonEx
    Friend WithEvents bnAudioEdit As ButtonEx
    Friend WithEvents bnAudioDown As ButtonEx
    Friend WithEvents bnAudioUp As ButtonEx
    Friend WithEvents bnAudioConvert As ButtonEx
    Friend WithEvents bnAudioRemove As ButtonEx
    Friend WithEvents pnTab As Panel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents laAC As Label
    Friend WithEvents bnMenuAudio As ButtonEx
    Friend WithEvents tcMain As TabControl
    Friend WithEvents tpAudio As TabPage
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents flpAudio As FlowLayoutPanel
    Friend WithEvents dgvAudio As DataGridViewEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.laThreads = New System.Windows.Forms.Label()
        Me.numThreads = New StaxRip.UI.NumEdit()
        Me.bnCMD = New StaxRip.UI.ButtonEx()
        Me.bnAudioAdd = New StaxRip.UI.ButtonEx()
        Me.bnAudioMediaInfo = New StaxRip.UI.ButtonEx()
        Me.bnAudioPlay = New StaxRip.UI.ButtonEx()
        Me.bnAudioEdit = New StaxRip.UI.ButtonEx()
        Me.bnAudioDown = New StaxRip.UI.ButtonEx()
        Me.bnAudioUp = New StaxRip.UI.ButtonEx()
        Me.bnAudioConvert = New StaxRip.UI.ButtonEx()
        Me.bnAudioRemove = New StaxRip.UI.ButtonEx()
        Me.pnTab = New System.Windows.Forms.Panel()
        Me.bnMenuAudio = New StaxRip.UI.ButtonEx()
        Me.laAC = New System.Windows.Forms.Label()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tpAudio = New System.Windows.Forms.TabPage()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.flpAudio = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.pnTab.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.tpAudio.SuspendLayout()
        Me.tcMain.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tlpAudio.SuspendLayout()
        Me.SuspendLayout()
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
        'bnCMD
        '
        Me.bnCMD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnCMD.Location = New System.Drawing.Point(742, 351)
        Me.bnCMD.Size = New System.Drawing.Size(84, 26)
        Me.bnCMD.Text = "     C&MD..."
        Me.TipProvider.SetTipText(Me.bnCMD, "Show Command Line")
        '
        'bnAudioAdd
        '
        Me.bnAudioAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioAdd.Location = New System.Drawing.Point(50, 351)
        Me.bnAudioAdd.Size = New System.Drawing.Size(88, 26)
        Me.bnAudioAdd.Text = "    &Add..."
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
        'bnAudioPlay
        '
        Me.bnAudioPlay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioPlay.Location = New System.Drawing.Point(472, 351)
        Me.bnAudioPlay.Size = New System.Drawing.Size(84, 26)
        Me.bnAudioPlay.Text = "  &Play"
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
        'bnAudioDown
        '
        Me.bnAudioDown.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioDown.Location = New System.Drawing.Point(402, 351)
        Me.bnAudioDown.Size = New System.Drawing.Size(64, 26)
        Me.bnAudioDown.Text = "     &Down"
        '
        'bnAudioUp
        '
        Me.bnAudioUp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioUp.Location = New System.Drawing.Point(332, 351)
        Me.bnAudioUp.Size = New System.Drawing.Size(64, 26)
        Me.bnAudioUp.Text = "  &Up"
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
        'bnAudioRemove
        '
        Me.bnAudioRemove.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAudioRemove.Location = New System.Drawing.Point(144, 351)
        Me.bnAudioRemove.Size = New System.Drawing.Size(88, 26)
        Me.bnAudioRemove.Text = "    &Remove"
        Me.TipProvider.SetTipText(Me.bnAudioRemove, "Remove Selection <Delete>")
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
        'dgvAudio
        '
        Me.dgvAudio.AllowDrop = True
        Me.dgvAudio.CausesValidation = False
        Me.dgvAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAudio.Location = New System.Drawing.Point(0, 0)
        Me.dgvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.Size = New System.Drawing.Size(811, 299)
        Me.dgvAudio.StandardTab = True
        Me.dgvAudio.TabIndex = 1
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
        Me.pnTab.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.tpAudio.ResumeLayout(False)
        Me.tcMain.ResumeLayout(False)
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WithEvents CMS As ContextMenuStripEx
    Private AudioCL As New CircularList(Of AudioProfile)
    Private AudioSBL As SortableBindingList(Of AudioProfile)
    Private OutPath As String
    Private LastLogPath As String
    Private LogHeader As Boolean

    Private LastKeyDown As Integer = -1
    Public SW3 As New Stopwatch

    Public PopulateTask As Task
    Public PopulateTaskW As Task
    Public PopulateTaskS As Boolean
    Public PopulateSWatch As New Stopwatch
    Public PopulateIter As Integer
    Public IndexTask As Task
    Public IndexSWatch As New Stopwatch
    Public SLock As New Object

    Public CPUs As Integer = Environment.ProcessorCount
    Public Shared MaxThreads As Integer

    Public StatUpdateTask As Task
    Public StatUpdateTaskA As Action = Sub()   'debug
                                           Do While PopulateIter < 400 AndAlso Not IsDisposed
                                               Thread.Sleep(120)
                                               Dim t As String = dgvAudio.Columns.Item(0).Width & PopulateTaskS.ToString & PopulateIter & PopulateTask?.Status.ToString & PopulateSWatch?.ElapsedTicks / 10000 & "msPopul|WaitPT: " & PopulateTaskW?.Status.ToString & " IndexTS: " & IndexTask?.Status.ToString & IndexSWatch?.ElapsedTicks / 10000 & "msIndex"
                                               'Dim t As String = dgvAudio.RowHeadersWidth.ToString & " RowHeaderWidth"
                                               Me.Invoke(Sub() Me.Text = t)
                                               'Thread.Sleep(90)
                                               'If AudioCL.Count < 1 Then Return
                                           Loop
                                       End Sub

    Public Sub New()
        MyBase.New()
        'AddHandler Application.ThreadException, AddressOf g.OnUnhandledException
        InitializeComponent()
        SetMinimumSize(44, 15)
        RestoreClientSize(53, 29)
        Control.CheckForIllegalCrossThreadCalls = False

        AudioSBL = New SortableBindingList(Of AudioProfile)(AudioCL) With {.CheckConsistency = False, .SortOnChange = False, .RaiseListChangedEvents = True}
        'AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks) 'AudioSBL = New SortableBindingList(Of AudioProfile)(ObjectHelp.GetCopy(p.AudioTracks)) 

        GetType(DataGridViewEx).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or
            BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, dgvAudio, New Object() {True})

        dgvAudio.RowHeadersVisible = True
        dgvAudio.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0!, FontStyle.Bold)
        dgvAudio.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvAudio.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
        dgvAudio.RowHeadersDefaultCellStyle.Padding = New Padding(0)

        'dgvAudio.RowTemplate.HeaderCell.Style.Alignment = dgvAudio.RowHeadersDefaultCellStyle.Alignment
        'dgvAudio.RowTemplate.HeaderCell.Style.WrapMode = dgvAudio.RowHeadersDefaultCellStyle.WrapMode
        'dgvAudio.RowTemplate.HeaderCell.Style.Padding = dgvAudio.RowHeadersDefaultCellStyle.Padding

        dgvAudio.RowTemplate.Height = Font.Height + 4 'or *1.25 ?, AutoResize=20, def=22
        dgvAudio.RowHeadersWidth = 24 '70 '64 (0)=40 +6 per char
        dgvAudio.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        dgvAudio.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvAudio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAudio.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAudio.MultiSelect = True
        dgvAudio.AutoGenerateColumns = False
        dgvAudio.AllowUserToResizeColumns = True
        dgvAudio.AllowUserToResizeRows = False
        dgvAudio.AllowUserToOrderColumns = True
        dgvAudio.ReadOnly = True
        dgvAudio.AllowUserToDeleteRows = False

        dgvAudio.DataSource = AudioSBL

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

        Dim indexColumn = dgvAudio.AddTextBoxColumn()
        indexColumn.DataPropertyName = "RowIdx"
        indexColumn.HeaderText = "No."
        indexColumn.FillWeight = 50

        Dim profileName = dgvAudio.AddTextBoxColumn()
        profileName.DataPropertyName = "Name"
        profileName.HeaderText = "Profile"
        profileName.FillWeight = 100

        Dim dispNameColumn = dgvAudio.AddTextBoxColumn()
        dispNameColumn.DataPropertyName = "DisplayName"
        dispNameColumn.HeaderText = "Track"
        dispNameColumn.FillWeight = 400

        Dim pathColumn = dgvAudio.AddTextBoxColumn()
        pathColumn.DataPropertyName = "File"
        pathColumn.HeaderText = "Full Path"
        pathColumn.FillWeight = 500

        For Each col As DataGridViewColumn In dgvAudio.Columns
            col.MinimumWidth = col.HeaderText.Length * 8 + 2
            col.ReadOnly = True
            col.SortMode = DataGridViewColumnSortMode.Automatic
            'col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
            'col.HeaderCell.ToolTipText = "F3 to sort"
        Next

        If MaxThreads = 0 Then MaxThreads = s.ParallelProcsNum
        numThreads.Value = MaxThreads

        CMS = New ContextMenuStripEx(components)
        'bnMenuAudio.ClickAction = Sub() UpdateCMS(Me.bnMenuAudio, New System.ComponentModel.CancelEventArgs With {.Cancel = False})
        'bnMenuAudio.AddClickAction(Sub() UpdateCMS(Me.bnMenuAudio, New System.ComponentModel.CancelEventArgs With {.Cancel = False}))
        bnMenuAudio.ContextMenuStrip = CMS
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        KeyPreview = False
        dgvAudio.Columns.Item(3).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        UpdateControls()
        bnAudioAdd.Select()
        Refresh()
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        PopulateTaskS = True
        PopulateIter = 303
        IndexSWatch.Stop()
        PopulateSWatch.Stop()
        SW3.Stop()
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        AudioSBL.Clear()
        AudioCL.Clear()
        dgvAudio.Rows.Clear()
        dgvAudio.Columns.Clear()
        MyBase.OnFormClosing(e)
    End Sub
    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        SaveConverterLog()
        MyBase.OnFormClosed(e)
    End Sub
    'Protected Overrides Sub onload(args As EventArgs)
    '    '  MyBase.OnLoad(args)
    'End Sub

    Private Sub UpdateCMS(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CMS.Opening

        'If dgvAudio.Rows.Count > 0 Then

        '    For n = 1 To 5
        '        Task.Delay(30)
        '        Thread.Sleep(30)
        '        Application.DoEvents()
        '        Task.Delay(30)
        '    Next n

        '    SW3.Restart()
        '    For i = 1 To 10000
        '        Parallel.ForEach(Partitioner.Create(0, AudioCL.Count), New ParallelOptions With {.MaxDegreeOfParallelism = CInt(CPUs / 2)},
        '                                Sub(range)
        '                                    For r = range.Item1 To range.Item2 - 1
        '                                        AudioSBL.Item(r).RowIdx = r + 1
        '                                    Next r
        '                                    ' dgvAudio.InvalidateColumn(0)
        '                                End Sub)
        '    Next
        '    SW3.Stop()
        '    Dim tt1 = SW3.ElapsedTicks / 10000


        '    For n = 1 To 5
        '        Task.Delay(30)
        '        Thread.Sleep(30)
        '        Application.DoEvents()
        '        Task.Delay(30)
        '    Next n

        '    SW3.Restart()
        '    For i = 1 To 10000
        '        For r = 0 To AudioCL.Count - 1
        '            AudioSBL.Item(r).RowIdx = r + 1
        '        Next r
        '        'dgvAudio.InvalidateColumn(0)
        '    Next
        '    SW3.Stop()

        '    MsgInfo(" 10 000 it Index ParEachLoop Partit time ms: " & tt1.ToString, "Index SerLoop time ms: " & SW3.ElapsedTicks / 10000)
        'End If



        'For n = 1 To 5
        '    Task.Delay(30)
        '    Thread.Sleep(30)
        '    Application.DoEvents()
        '    Task.Delay(30)
        'Next n

        'SW3.Restart()
        'For i = 1 To 10000
        '    Parallel.For(0, AudioCL.Count, New ParallelOptions With {.MaxDegreeOfParallelism = CInt(CPUs / 2)},
        '                                Sub(r)
        '                                    AudioSBL.Item(r).RowIdx = r + 1
        '                                    ' dgvAudio.InvalidateColumn(0)
        '                                End Sub)
        'Next
        'SW3.Stop()
        'MsgInfo(" 10 000 it Index ParForLoop time ms: ", SW3.ElapsedTicks / 10000)


        CMS.SuspendLayout()

        'CMS.GetItems.ForEach(Sub(it) it.Image = Nothing)

        'For Each it In CMS.Items.OfType(Of ToolStripItem).ToArray
        '    RemoveHandler CMS.Opening, AddressOf TryCast(it, ActionMenuItem).Opening
        '    If Me.Owner IsNot Nothing Then RemoveHandler DirectCast(Me.Owner, ToolStripDropDown).Opening, AddressOf UpdateCMS
        '    If Me.Owner IsNot Nothing Then RemoveHandler DirectCast(Me.Owner, ToolStripDropDown).Opening, AddressOf DirectCast(it, ActionMenuItem).Opening
        '    it.Dispose()
        'Next

        CMS.Items.ClearAndDisplose
        Dim isR = AudioCL.Count > 0
        Dim isCR = isR AndAlso dgvAudio.CurrentCellAddress.Y >= 0
        Dim cAP As AudioProfile
        If isCR Then cAP = AudioSBL.Item(dgvAudio.CurrentCellAddress.Y)
        Dim logNotEmpty As Boolean = Not Log.IsEmpty

        CMS.Add("Select all  <Ctrl+A>", Sub() dgvAudio.SelectAll(), isR).SetImage(Symbol.SelectAll)
        CMS.Add("Remove all", Sub()
                                  AudioSBL.Clear()
                                  AudioCL.Clear()
                                  AudioSBL.InnerListChanged()
                                  AudioSBL.ResetBindings()
                                  dgvAudio.Rows.Clear()
                                  UpdateControls()
                              End Sub, isR).SetImage(Symbol.Clear)

        CMS.Add("Show Source File", Sub()
                                        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.CurrentCellAddress.Y
                                        g.SelectFileWithExplorer(cAP.File)
                                    End Sub, isCR AndAlso FileExists(cAP.File), "Open the source file in File Explorer.").SetImage(Symbol.FileExplorer)

        CMS.Add("Show Ouput Folder", Sub() g.SelectFileWithExplorer(OutPath), DirExists(OutPath), "Open output folder in File Explorer.").SetImage(Symbol.FileExplorerApp)

        'cms.Add("Show Output File", Sub() g.SelectFileWithExplorer(OutPath & RelativeSubDirRecursive(cAP.File.Dir, 0) & cAP.GetOutputFile),
        'ccExists AndAlso FileExists(OutPath & RelativeSubDirRecursive(cAP.File.Dir, 0) & cAP.GetOutputFile), "Open converted file in File explerer").SetImage(Symbol.ShowResults)

        CMS.Add("Show LOG", Sub()
                                SaveConverterLog()
                                If FileExists(LastLogPath) Then
                                    g.SelectFileWithExplorer(LastLogPath)
                                ElseIf FileExists(p.Log.GetPath) Then
                                    g.SelectFileWithExplorer(p.Log.GetPath)
                                End If
                            End Sub, logNotEmpty, "Open current log in File Explorer").SetImage(Symbol.fa_folder_open_o)

        CMS.Add("Save LOG...", Sub() SaveConverterLog(UseDialog:=True), logNotEmpty).SetImage(Symbol.Save)
        CMS.Add("-")
        CMS.Add("qaac Help", Sub() Package.qaac.ShowHelp())
        CMS.Add("qaac Formats", Sub() MsgInfo(ProcessHelp.GetConsoleOutput(Package.qaac.Path, "--formats")))
        CMS.Add("Opus Help", Sub() Package.OpusEnc.ShowHelp())
        CMS.Add("ffmpeg Help", Sub() Package.ffmpeg.ShowHelp())
        CMS.Add("eac3to Help", Sub() g.ShellExecute("http://en.wikibooks.org/wiki/Eac3to"))

        CMS.ResumeLayout()
        LastKeyDown = -1
        e.Cancel = False
    End Sub

    'Public Function OpenAudioConverterDialog() As DialogResult 'Debug
    '    Using form As New AudioConverterForm
    '        Return form.ShowDialog()
    '    End Using
    'End Function

    Public Sub UpdateControls(Optional SelRowsCount As Integer = -1, Optional CurrentRowI As Integer = -1)
        SuspendLayout()
        If SelRowsCount = -1 Then SelRowsCount = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim rC As Integer = AudioCL.Count
        Dim t As String
        If CurrentRowI = -1 Then CurrentRowI = dgvAudio.CurrentCellAddress.Y
        t = " Pos: " & CStr(CurrentRowI + 1) & "   |   Sel: " & CStr(SelRowsCount) & " / Tot: " & CStr(rC)

        BeginInvoke(Sub()
                        If rC > 0 Then
                            laAC.Text = t
                        Else
                            laAC.Text = "Please add or drop music files..."
                        End If

                        bnAudioPlay.Enabled = SelRowsCount > 0
                        bnAudioMediaInfo.Enabled = SelRowsCount > 0
                        bnAudioRemove.Enabled = SelRowsCount > 0
                        bnAudioEdit.Enabled = SelRowsCount > 0
                        bnCMD.Enabled = SelRowsCount > 0
                        bnAudioConvert.Enabled = SelRowsCount > 0
                        bnAudioUp.Enabled = SelRowsCount = 1 AndAlso CurrentRowI > 0
                        bnAudioDown.Enabled = SelRowsCount = 1 AndAlso CurrentRowI < rC - 1
                        numThreads.Enabled = SelRowsCount > 1
                        ResumeLayout()
                    End Sub)
        'If InvokeRequired Then  BeginInvoke(act)
    End Sub

    Private Sub dgvAudio_SelectionChanged(sender As Object, e As EventArgs)
        PopulateTaskS = True
        PopulateCache()
        Task.Run(Sub() UpdateControls())
    End Sub
    'Private Sub dgvAudio_CurrentCellChanged(sender As Object, e As EventArgs)
    ''If LCurrentRowI <> dgvAudio.CurrentCellAddress.Y Then  'OrElse LSelectedRowI <> dgvAudio.SelectedRows(0).Index Then
    ''    LCurrentRowI = dgvAudio.CurrentCellAddress.Y
    'LCurrentRowI = dgvAudio.CurrentCellAddress.Y
    'LSelectedRowC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
    'PopulateTaskS = True
    'UpdateControls()
    'PopulateCache()
    '' End If
    'End Sub

    Private Sub StatusText(InfoText As String)
        'laAC.BeginInvoke(Sub()
        'Task.Run(Sub() invoke(sub()
        laAC.Text = InfoText
        laAC.Refresh()
        '         End Sub)
    End Sub

    Private Sub AutoSizeColumns()
        dgvAudio.SuspendLayout()
        StatusText("Auto Resizing Columns...")
        dgvAudio.AutoResizeColumnHeadersHeight()
        'dgvAudio.AutoResizeRowHeadersWidth(rowHeadersWidthSizeMode:=DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders )
        'dgvAudio.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)

        If dgvAudio.AutoSizeColumnsMode <> DataGridViewAutoSizeColumnsMode.Fill Then
            PopulateTaskS = True
            dgvAudio.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
        Else
            'dgvAudio.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill)
        End If

        dgvAudio.RowHeadersWidth = 24
        dgvAudio.Columns.Item(0).Width = Math.Max(26, CInt(14 + 6 * Fix(Math.Log10(AudioCL.Count))))
        dgvAudio.ResumeLayout()
    End Sub

    Private Sub IndexHeaderRows()
        Dim ta As Action = Sub()
                               'IndexSWatch.Restart()
                               'Do Until IndexSWatch.ElapsedTicks > 570000 OrElse Not IndexSWatch.IsRunning
                               '    Thread.Sleep(58)
                               'Loop
                               'IndexSWatch.Stop()
                               ' Return
                               If AudioCL.Count > 0 Then
                                   IndexSWatch.Restart()
                                   'SyncLock AudioSBL
                                   Parallel.ForEach(Partitioner.Create(0, AudioCL.Count), New ParallelOptions With {.MaxDegreeOfParallelism = CInt(CPUs / 2)},
                                                Sub(range)
                                                    For r = range.Item1 To range.Item2 - 1
                                                        AudioSBL.Item(r).RowIdx = r + 1
                                                    Next r
                                                End Sub)
                                   ' dgvAudio.InvalidateColumn(0)

                                   'For r = 0 To AudioCL.Count - 1
                                   '    AudioSBL.Item(r).RowIdx = r + 1
                                   'Next r
                                   'dgvAudio.InvalidateColumn(0)
                                   'End SyncLock
                               End If
                               IndexSWatch.Stop()
                           End Sub
        'IndexSWatch.Restart()
        If IndexTask Is Nothing OrElse IndexTask.IsCompleted Then
            IndexTask = Task.Run(Sub() ta())
            Dim contIdx = IndexTask.ContinueWith(Sub()  'debug
                                                     If IndexTask.Exception IsNot Nothing Then
                                                         IndexSWatch.Stop()
                                                         Task.Run(Sub() Log.Write("Index Task Cont Exceptions: ", IndexTask.Exception.ToString & PopulateTaskS.ToString & PopulateIter & PopulateTask.Status.ToString & PopulateSWatch?.ElapsedTicks / 10000 & "ms-PopulateT|WaitPT: " & PopulateTaskW?.Status.ToString & " IndexTS: " & IndexTask.Status.ToString & IndexSWatch?.ElapsedTicks / 10000 & "msDelay"))
                                                         Task.Run(Sub() Console.Beep(2500, 30))
                                                     End If
                                                     If AudioCL.Count = 0 OrElse (AudioSBL.Item(0).RowIdx = 1 AndAlso AudioSBL.Item(AudioCL.Count - 1).RowIdx = AudioCL.Count) Then Return
                                                     If IndexTask.IsCompleted Then IndexTask = Task.Run(Sub()
                                                                                                            Console.Beep(380, 40)
                                                                                                            ta()
                                                                                                        End Sub)
                                                 End Sub)

        End If
    End Sub
    'Private Sub dgvAudio_RowUnshared(sender As Object, e As DataGridViewRowEventArgs) Handles dgvAudio.RowUnshared
    '    laThreads.Text = "UnShI:" & e.Row.Index.ToString()
    '    laThreads.Refresh()
    '    Console.Beep(2500, 15)
    '    'Thread.Sleep(50)
    '    'e.Row.HeaderCell.Value = CStr(e.Row.Index + 1)
    'End Sub
    'Private Sub dgvaudio_rowstatechanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles dgvAudio.RowStateChanged
    '    'Me.Text = e.Row.State.ToString & " | " & e.StateChanged.ToString & e.Row.Index.ToString
    '    'Thread.Sleep(150)
    '    If e.StateChanged = DataGridViewElementStates.Displayed Then
    '        'If e.Row.State = DataGridViewElementStates.Displayed 'or datagridviewelementstates.visible  then
    '        e.Row.HeaderCell.Value = CStr(e.Row.Index + 1)
    '    End If
    'End Sub
    'Private Sub dgvAudio_Layout(sender As Object, e As LayoutEventArgs) Handles dgvAudio.Layout
    'End Sub
    'Private Sub dgvAudio_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAudio.DataError
    '    'Task.Run(Sub()
    '    '             Log.Write("DGV DataErrorException:cr " & e.ColumnIndex & e.RowIndex, e.Exception.ToString & e.ToString)
    '    '             If Date.Now.Millisecond Mod 2 = 0 Then Console.Beep(6000, 30)
    '    '         End Sub)
    '    e.Cancel = True
    'End Sub
    'Private Sub dgvAudio_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvAudio.DataBindingComplete
    '    Task.Run(Sub() Log.WriteLine("--->DataBindingCompleteEvents: " & e.ListChangedType.ToString))
    'End Sub
    'Private Sub AudioSBL_(sender As Object, e As ListChangedEventArgs) Handles AudioSBL.ListChanged
    '    Task.Run(Sub() Log.WriteLine("--->SortBindListChangedEvents: " & e.PropertyDescriptor?.ToString & " PropDesc | ListChangeType " & If(e.ListChangedType.ToString, "") & If(e.NewIndex.ToString, "") & " NewIdx | oldIdx " & If(e.OldIndex.ToString, "")))
    'End Sub
    Private Sub dgvAudio_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvAudio.Scroll
        'Task.Run(Sub() Log.WriteLine("--->ScrollEvents: " & e.NewValue.ToString & " :newv | oldv : " & e.OldValue.ToString & e.Type.ToString & " :type | scrollOrient : " & e.ScrollOrientation.ToString))
        Select Case e.Type
            Case ScrollEventType.LargeDecrement, ScrollEventType.LargeIncrement, ScrollEventType.ThumbTrack
                PopulateTaskS = True
                PopulateCache()
        End Select
    End Sub

    Private Sub dgvAudio_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseClick
        If e.RowIndex = -1 AndAlso e.ColumnIndex > -1 AndAlso e.Button = MouseButtons.Left AndAlso AudioCL.Count > 0 Then
            PopulateTaskS = True
            StatusText("Sorting...")
            RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
            'For i = 1 To 2
            'Thread.Sleep(15)
            '   SyncLock AudioSBL
            'End SyncLock
            'Next
            'IndexSWatch.Restart()
        End If
    End Sub

    Private Sub dgvAudio_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseUp
        If e.RowIndex = -1 AndAlso e.ColumnIndex > -1 AndAlso e.Button = MouseButtons.Left AndAlso AudioCL.Count > 0 Then
            Task.Delay(30).Wait()
            UpdateControls()
            AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        End If
    End Sub

    Private Sub dgvAudio_Sorted(sender As Object, e As EventArgs) Handles dgvAudio.Sorted
        IndexHeaderRows()
    End Sub

    Public Sub SaveConverterLog(Optional LogPath As String = Nothing, Optional UseDialog As Boolean = False)
        If Not Log.IsEmpty Then
            Log.Save()

            If UseDialog Then
                Using dialog As New FolderBrowserDialog With {
                    .Description = "Please select LOG File location :",
                    .UseDescriptionForTitle = True,
                    .RootFolder = Environment.SpecialFolder.MyComputer}
                    If dialog.ShowDialog = DialogResult.OK Then LogPath = dialog.SelectedPath.FixDir
                End Using
            End If

            If LogPath IsNot Nothing AndAlso DirExists(LogPath) Then
                Dim LogName = Date.Now.ToString("yy-MM-dd_HH.mm.ss") & "_staxrip.log"
                Try
                    FileHelp.Move(Log.GetPath, LogPath & LogName)
                    LastLogPath = LogPath & LogName
                Catch
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
                    path = path.Replace(ch, "")
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

    Private Sub EncodeAudio(ap As AudioProfile, Optional SubDepth As Integer = 1)
        p.TempDir = OutPath
        Dim outP As String = ap.GetOutputFile()
        'Dim inFn As String = ap.File.FileName
        Dim nOutD As String = OutPath & RelativeSubDirRecursive(ap.File.Dir, SubDepth)
        ap = ObjectHelp.GetCopy(ap)
        Audio.Process(ap)
        ap.Encode()

        If SubDepth > 0 AndAlso Not outP.Dir.Equals(nOutD) AndAlso FileExists(outP) Then
            Try
                FolderHelp.Create(nOutD)
                Thread.Sleep(30)
                FileHelp.Move(outP, nOutD & outP.FileName)    '     ap.File.FileName
            Catch ex As Exception
                Log.Write("Audio Converter IO copy exception", ex.ToString)
                'Throw New ErrorAbortException("Failed To create output file ", "Failed To create output file Or path: " & nOutD & outP)
            End Try

            Thread.Sleep(75)
            If Not DirExists(nOutD) Then
                Log.Write("Audio Converter error", "Failed to create output directory: " & nOutD)
            ElseIf Not FileExists(nOutD & outP.FileName) Then
                Log.Write("Audio Converter error", "Failed to create output file in path: " & nOutD & outP.FileName)
            End If
        Else
            If Not FileExists(outP) Then
                Log.Write("Audio Converter error", "Failed to create output file: " & outP)
                'Throw New ErrorAbortException("Failed to create output file ", "Failed to create output file: " & outP)
            End If
        End If
    End Sub

    Private Sub bnAudioConvert_Click(sender As Object, e As EventArgs) Handles bnAudioConvert.Click
        Using dialog As New FolderBrowserDialog With {
            .Description = "Please select output directory :",
            .UseDescriptionForTitle = True,
            .RootFolder = Environment.SpecialFolder.MyComputer}
            If dialog.ShowDialog <> DialogResult.OK Then Exit Sub
            If dialog.SelectedPath Is Nothing OrElse Not DirExists(dialog.SelectedPath.FixDir) Then
                Exit Sub
            End If
            OutPath = dialog.SelectedPath.FixDir
        End Using

        PopulateTaskS = True
        p.TempDir = OutPath

        For Each sr As DataGridViewRow In dgvAudio.SelectedRows
            If AudioSBL(sr.Index).File.Dir.Contains(OutPath) Then
                If MsgQuestion("One of source files is inside output directory.  Are you sure ?", TaskDialogButtons.YesNo) <> DialogResult.Yes Then
                    Exit Sub
                End If
                Exit For
            End If
        Next

        Dim subDepth As Integer
        Using td As New TaskDialog(Of Integer) With {.MainInstruction = "How many source Sub Dirs use ?"}
            td.AddCommand("0 - All files in output dir", 0)
            For i = 1 To 5
                td.AddButton(CStr(i), i)
            Next
            td.SelectedValue = -1
            subDepth = td.Show
            If subDepth < 0 Then Exit Sub
        End Using

        Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)

        If MsgQuestion("Confirm to convert selected: " & srC & BR & "To path: " & OutPath & BR & "Sub Dirs: " & subDepth) = DialogResult.OK AndAlso DirExists(OutPath) Then
            Dim StartTime = Date.Now
            StatusText("Converting...")

            If Log.IsEmpty Then Log.WriteEnvironment()
            If Not LogHeader Then
                Log.WriteHeader("Audio Converter")
                LogHeader = True
            End If
            Log.WriteHeader("Audio Converter queue started: " & Date.Now.ToString)

            Dim ap As AudioProfile
            Dim actions As New CircularList(Of Action) With {.Capacity = srC}

            For Each sr As DataGridViewRow In dgvAudio.SelectedRows
                ap = AudioSBL.Item(sr.Index)
                If FileExists(ap.File) AndAlso My.Computer.FileSystem.GetFileInfo(ap.File).Length > 10 Then
                    'EncodeAudio(DirectCast(srbs, AudioProfile), OutPath) ST debug
                    actions.Add(Sub() EncodeAudio(ap, subDepth))
                End If
            Next

            Try
                If s.PreventStandby Then PowerRequest.SuppressStandby()
                ProcController.BlockActivation = True
                Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = MaxThreads}, actions.ToArray)

            Catch aex As AggregateException
                Log.Write("Audio Converter aggregate encode exceptions", aex.ToString)
                'For Each iex As Exception In aex.InnerExceptions
                '    Log.Write("Audio Converter inner encode exceptions", iex.ToString)
                'Next
                'ExceptionDispatchInfo.Capture(ex.InnerExceptions(0)).Throw()
                'ProcController.Abort()
                'g.ShowException(ex, Nothing, Nothing, 60)

            Catch ex As Exception
                Log.Write("Audio Converter encode exception", ex.ToString)

            Finally
                If s.PreventStandby Then PowerRequest.EnableStandby()
                ProcController.BlockActivation = False
            End Try

            StatusText("Checking results...")
            Task.Delay(1500).Wait()
            Visible = True
            Show()
            WindowState = FormWindowState.Normal
            'TopMost = True
            Activate()
            dgvAudio.Select()
            dgvAudio.Refresh()
            Refresh()

            Dim OKCount As Integer
            Dim outFile As String

            For Each sr As DataGridViewRow In dgvAudio.SelectedRows
                ap = AudioSBL.Item(sr.Index)
                outFile = OutPath & RelativeSubDirRecursive(ap.File.Dir, subDepth) & ap.GetOutputFile.FileName() 'ap.file.filename
                If FileExists(outFile) AndAlso My.Computer.FileSystem.GetFileInfo(outFile).Length > 10 Then
                    sr.HeaderCell.Value = "OK"
                    'sr.Cells.Item(0).Value = "OK"
                    OKCount += 1
                Else
                    sr.HeaderCell.Value = "Error"
                    'sr.Cells.Item(0).Value = "Error"
                End If
            Next sr

            If srC < OKCount AndAlso dgvAudio.Columns.Item(0).Width < 38 Then 'Index space for Error
                dgvAudio.RowHeadersWidth = 70
            Else
                dgvAudio.RowHeadersWidth = 52
            End If

            Log.Write("Conversion Complete", OKCount & " files out of " & srC & " converted succesfully")
            Log.WriteStats(StartTime)

            SaveConverterLog(OutPath)
            UpdateControls()
            bnAudioRemove.Select()
        End If
    End Sub

    Private Sub ProcessInputAudioFiles(Files As String())
        Dim fC As Integer = Files.Length

        If fC <= 0 Then
            Exit Sub
        ElseIf fC > 100 Then
            If MsgQuestion("Add " & fC & " files ?") <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        Dim rC As Integer = AudioCL.Count
        Dim OldLastIDX As Integer = If(rC > 0, dgvAudio.Rows(rC - 1).Index, 0)
        Dim OldCurrCol As Integer = If(dgvAudio.CurrentCellAddress.X >= 0, dgvAudio.CurrentCellAddress.X, 2) ' -1 ,-1 = Nothing CurrentRow
        'Dim sa = Task.Run(Sub() Array.Sort(Files))
        Dim profileSelection As New SelectionBox(Of AudioProfile) With {.Title = "Please select Audio Profile"}

        If Not p.Audio0.IsNullProfile AndAlso Not p.Audio0.IsMuxProfile Then
            profileSelection.AddItem("Current Project 1: " & p.Audio0.ToString, p.Audio0)
        End If
        If Not p.Audio1.IsNullProfile AndAlso Not p.Audio1.IsMuxProfile Then
            profileSelection.AddItem("Current Project 2: " & p.Audio1.ToString, p.Audio1)
        End If
        For Each AudioProf In s.AudioProfiles
            If AudioProf.IsMuxProfile OrElse AudioProf.IsNullProfile Then Continue For
            profileSelection.AddItem(AudioProf)
        Next

        'SelectionBoxForm.StartPosition = FormStartPosition.CenterParent      'Drag Drop  out of center
        If profileSelection.Show <> DialogResult.OK Then Exit Sub

        PopulateTaskS = True
        StatusText("Adding Files...")
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged

        If rC = 0 Then AudioCL.Capacity = fC
        AudioSBL.RemoveSort()

        dgvAudio.SuspendLayout()
        AudioSBL.RaiseListChangedEvents = False
        Dim ap As AudioProfile = ObjectHelp.GetCopy(profileSelection.SelectedValue)
        ap.SourceSamplingRate = 0
        If TypeOf ap Is GUIAudioProfile Then
            ap.Name = ap.DefaultName
        End If

        Dim apTS(fC - 1) As AudioProfile
        Dim dummyStream As New AudioStream 'With {.Index = 29876}
        Dim autoStream As Boolean = False

        Parallel.For(0, fC, New ParallelOptions With {.MaxDegreeOfParallelism = CInt(CPUs / 2)},
                         Sub(i)
                             Dim apL = ObjectHelp.GetCopy(ap)
                             apL.File = Files(i) ' ToDo: What if files acces is restricted 

                             If FileTypes.VideoAudio.Contains(apL.File.Ext) Then
                                 apL.Streams = MediaInfo.GetAudioStreams(apL.File)

                                 If apL.Streams.Count > 0 Then
                                     apL.Stream = apL.Streams(0)
                                 Else
                                     If Log.IsEmpty Then Log.WriteEnvironment()
                                     If Not LogHeader Then
                                         LogHeader = True
                                         Log.WriteHeader("Audio Converter")
                                     End If
                                     Log.WriteLine("-->Audio stream not found, skipping: " & BR & apL.File)
                                     Return
                                 End If

                                 If Not autoStream AndAlso apL.Stream IsNot Nothing AndAlso apL.Streams.Count > 1 Then
                                     SyncLock SLock
                                         If Not autoStream Then
                                             Dim streamSelection = New SelectionBox(Of AudioStream) With {
                                             .Title = "Stream Selection",
                                             .Text = "Please select an audio stream for: " & BR & apL.File.ShortBegEnd(60, 60)}
                                             For Each stream In apL.Streams
                                                 streamSelection.AddItem(stream)
                                             Next
                                             streamSelection.AddItem(" > Use first stream and don't ask me again <", dummyStream)

                                             If streamSelection.Show() <> DialogResult.OK Then Return

                                             If streamSelection.SelectedValue IsNot dummyStream Then
                                                 apL.Stream = streamSelection.SelectedValue
                                             Else
                                                 autoStream = True
                                             End If
                                         End If
                                     End SyncLock
                                 End If
                             End If

                             apTS(i) = apL
                         End Sub)
        'SW3.Stop()
        'MsgInfo(SW3.ElapsedTicks / 10000 & "ms ParFor")
        'AudioSBL.Clear()
        'AudioCL.Reset()
        'AudioSBL.InnerListChanged()
        'Thread.Sleep(100)
        'Application.DoEvents()
        'Task.Delay(50).Wait()
        'SW3.Restart()
        'For n = 1 To 2
        '    AudioSBL.AddRange(apTS.Where(Function(itm As AudioProfile) itm IsNot Nothing))
        'Next n
        'SW3.Stop()
        'AudioSBL.Clear()
        'AudioCL.Reset()
        'AudioSBL.InnerListChanged()
        'MsgInfo(SW3.ElapsedTicks / 10000 & "ms - AudioSBL AddRange 100it")

        AudioCL.AddRange(apTS.Where(Function(itm As AudioProfile) itm IsNot Nothing))
        AudioSBL.InnerListChanged()
        AudioSBL.RaiseListChangedEvents = True
        AudioSBL.ResetBindings()
        dgvAudio.ResumeLayout()

        If rC = 0 Then
            'dgvAudio.Sort(dgvAudio.Columns(3), ComponentModel.ListSortDirection.Ascending)
            'AudioSBL.ApplySort("File", System.ComponentModel.ListSortDirection.Ascending)
            dgvAudio.Columns(0).HeaderCell.SortGlyphDirection = SortOrder.None
            dgvAudio.Columns(1).HeaderCell.SortGlyphDirection = SortOrder.None
            dgvAudio.Columns(2).HeaderCell.SortGlyphDirection = SortOrder.None
            dgvAudio.Columns(3).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        End If

        rC = AudioCL.Count

        If rC > 0 Then
            dgvAudio.Select()
            If rC > 100 Then dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvAudio.Columns.Item(0).Width = Math.Max(26, CInt(14 + 6 * Fix(Math.Log10(rC))))
            dgvAudio.CurrentCell = dgvAudio.Rows.Item(rC - 1).Cells(OldCurrCol)
            dgvAudio.Rows(OldLastIDX).Selected = True
            IndexHeaderRows()
            AutoSizeColumns()
            PopulateIter = 1
            PopulateCache()
        End If

        UpdateControls()
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged

        If StatUpdateTask Is Nothing OrElse StatUpdateTask.IsCompleted Then      'debug
            StatUpdateTask = Task.Factory.StartNew(StatUpdateTaskA, TaskCreationOptions.LongRunning)
        End If

    End Sub

    Public Sub PopulateCache()
        Select Case PopulateIter
            Case Is > 400
                Exit Sub
            Case Is >= 200
                PopulateTaskS = True
                PopulateIter += 1
                Exit Sub
        End Select
        Dim pTaskA As Action(Of Integer) = Sub(Delay)
                                               PopulateSWatch.Restart()
                                               If PopulateIter >= 200 OrElse AudioCL.Count <= 20 Then
                                                   PopulateTaskS = True
                                                   PopulateIter = 303
                                                   PopulateSWatch.Stop()
                                                   Return
                                               End If
                                               PopulateTaskS = False
                                               Do Until PopulateSWatch.ElapsedTicks > Delay OrElse Not PopulateSWatch.IsRunning
                                                   Thread.Sleep(150)
                                                   If PopulateTaskS Then
                                                       PopulateIter += 1
                                                       PopulateSWatch.Stop()
                                                       Return
                                                   End If
                                               Loop

                                               If Not PopulateTaskS AndAlso PopulateIter < 200 AndAlso AudioCL.Count > 20 Then
                                                   Try
                                                       Dim ar(AudioCL.Count - 1) As AudioProfile
                                                       SyncLock AudioSBL
                                                           AudioCL.CopyTo(ar, 0)
                                                       End SyncLock

                                                       If Not PopulateTaskS AndAlso AudioCL.Count > 20 Then
                                                           PopulateSWatch.Restart()

                                                           Dim pl = Parallel.For(0, AudioCL.Count, New ParallelOptions With {.MaxDegreeOfParallelism = CInt(CPUs / 2)},
                                                       Sub(i, pls)
                                                           If PopulateTaskS Then
                                                               pls.Stop()
                                                               PopulateSWatch.Stop()
                                                               Return
                                                           Else
                                                               If ar(i)?.DisplayName IsNot Nothing Then
                                                               End If
                                                           End If
                                                       End Sub)

                                                           PopulateSWatch.Stop()
                                                           If pl.IsCompleted Then
                                                               PopulateIter += 20
                                                               If PopulateSWatch.ElapsedTicks < 20000000 Then PopulateIter += 20
                                                           End If
                                                       End If
                                                   Catch ex As Exception
                                                       PopulateSWatch.Stop()
                                                       Task.Run(Sub() Console.Beep(4000, 200))
                                                       Log.Write("PopulateTask Catch", PopulateTaskS.ToString & PopulateSWatch.ElapsedTicks / 10000 & "ms iter: " & PopulateIter & ex.ToString & ex.InnerException?.ToString & IndexTask?.Status.ToString & " | " & PopulateTask?.Status.ToString & " | " & PopulateTask?.Exception?.ToString & PopulateTaskW?.Status.ToString)
                                                   End Try
                                               End If
                                           End Sub

        PopulateSWatch.Restart()
        If PopulateTask Is Nothing OrElse PopulateTask.IsCompleted Then
            PopulateTask = Task.Factory.StartNew(Sub() pTaskA(6900000), TaskCreationOptions.LongRunning)

        ElseIf PopulateTaskW Is Nothing OrElse PopulateTaskW.IsCompleted Then
            PopulateTaskW = Task.Run(Sub()
                                         PopulateTask.Wait()
                                         If PopulateTaskS Then PopulateTask = Task.Factory.StartNew(Sub() pTaskA(9900000), TaskCreationOptions.LongRunning)
                                     End Sub)
        End If
    End Sub

    Private Sub bnAudioAdd_Click(sender As Object, e As EventArgs) Handles bnAudioAdd.Click
        Using td As New TaskDialog(Of String)
            Dim ftav = FileTypes.Audio.Union(FileTypes.VideoAudio)
            td.AddCommand("Add files", "files")
            td.AddCommand("Add folder", "folder")
            Select Case td.Show
                Case "files"
                    Using dialog As New OpenFileDialog With {.Multiselect = True}
                        dialog.SetFilter(ftav)
                        If dialog.ShowDialog = DialogResult.OK Then
                            Dim av = dialog.FileNames.Where(Function(file) ftav.Contains(file.Ext))
                            ProcessInputAudioFiles(av.ToArray)
                        End If
                    End Using

                Case "folder"
                    Using dialog As New FolderBrowserDialog With {.RootFolder = Environment.SpecialFolder.MyComputer}
                        If dialog.ShowDialog = DialogResult.OK Then
                            Dim subfolders = Directory.GetDirectories(dialog.SelectedPath)
                            Dim opt = SearchOption.TopDirectoryOnly

                            If Directory.GetDirectories(dialog.SelectedPath).Length > 0 Then
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
        Dim rC As Integer = AudioCL.Count
        Dim srC As Integer = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim sr0i As Integer = dgvAudio.SelectedRows.Item(0).Index ' ToDo Use CurrentRow if selected, check one up then down ' Dim crI As Integer = dgvAudio.CurrentCellAddress.Y Return If(dgvAudio.Rows.GetRowState(crI) >= 96, crI, dgvAudio.SelectedRows.Item(0).Index)
        'If dgvAudio.Rows.GetRowState(r) << 30 = 1073741824 Then 'Displayed State
        Dim ap As AudioProfile = AudioSBL(sr0i)
        Dim OldLang As Language = ap.Language
        dgvAudio.FirstDisplayedScrollingRowIndex = sr0i
        StatusText("Editing: " & (sr0i + 1) & "   |   Sel: " & srC & " / Tot: " & rC)

        If ap.EditAudioConv() = Global.System.Windows.Forms.DialogResult.OK Then
            PopulateTaskS = True
            RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
            'g.MainForm.UpdateAudioMenu()
            'g.MainForm.UpdateSizeOrBitrate()
            dgvAudio.SuspendLayout()
            StatusText("Applying settings...")

            If TypeOf ap Is GUIAudioProfile Then
                ap.Name = ap.DefaultName
            End If

            AudioSBL.ResetItem(sr0i)

            If srC > 1 Then
                AudioSBL.RaiseListChangedEvents = False
                Dim apNoLangChange As Boolean = ap.Language.Equals(OldLang)
                Dim apDelayNonZero As Boolean = ap.Delay <> 0
                Dim srCache(rC - 1) As Boolean
                srCache(sr0i) = True
                Dim lockL As New LockingList(Of AudioProfile)(AudioSBL)
                Parallel.For(1, srC, New ParallelOptions With {.MaxDegreeOfParallelism = CInt(CPUs / 2)}, ' ToDo: use loop for all rows and if dgvAudio.Rows.GetRowState(i) >= 96
                             Sub(n)
                                 Dim ap0 As AudioProfile = ObjectHelp.GetCopy(ap)
                                 Dim srI As Integer = dgvAudio.SelectedRows.Item(n).Index
                                 Dim apn As AudioProfile = AudioSBL.Item(srI)
                                 ap0.File = apn.File
                                 If apNoLangChange Then ap0.Language = apn.Language 'needed?
                                 ap0.StreamName = apn.StreamName
                                 ap0.Stream = apn.Stream
                                 ap0.Streams = apn.Streams
                                 If apDelayNonZero Then ap0.Delay = apn.Delay Else ap0.Delay = 0 'needed?
                                 ap0.SourceSamplingRate = apn.SourceSamplingRate

                                 srCache(srI) = True
                                 lockL.Item(srI) = ap0
                             End Sub)

                StatusText("Restoring selection...")
                'AudioSBL.InnerListChanged() ' Needed???
                AudioSBL.RaiseListChangedEvents = True
                AudioSBL.ResetBindings()

                If dgvAudio.CurrentCellAddress.Y >= 0 Then dgvAudio.CurrentRow.Selected = False
                For nRow = 0 To rC - 1
                    If srCache(nRow) = True Then
                        dgvAudio.Rows.Item(nRow).Selected = True
                    End If
                Next
            End If

            'IndexHeaderRows()
            If rC < 100 Then AutoSizeColumns()
            AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
            dgvAudio.ResumeLayout()
            PopulateCache()
        End If

        UpdateControls()
    End Sub

    Private Sub bnAudioRemove_Click(sender As Object, e As EventArgs) Handles bnAudioRemove.Click
        PopulateTaskS = True
        DeleteRows()
    End Sub

    Private Sub dgvAudio_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvAudio.KeyDown
        Select Case e.KeyCode
            Case Keys.Delete
                PopulateTaskS = True
                e.Handled = True 'removeit???
                DeleteRows()
                Exit Sub
            Case Keys.F3
                If LastKeyDown = 3 Then
                    e.SuppressKeyPress = True

                ElseIf AudioCL.Count > 0 Then
                    LastKeyDown = 3
                    PopulateTaskS = True
                    StatusText("Sorting...")
                    RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
                    'For i = 1 To 2
                    'Thread.Sleep(60)
                    '  SyncLock AudioSBL
                    '   End SyncLock
                    'Next
                    'IndexSWatch.Restart()
                End If
        End Select
    End Sub

    Private Sub dgvAudio_KeyUp(sender As Object, e As KeyEventArgs) Handles dgvAudio.KeyUp ', MyBase.KeyUp
        Select Case e.KeyCode
            Case Keys.F3
                'If sender.Equals(dgvAudio) Then
                'IndexHeaderRows()
                Task.Delay(30).Wait()
                UpdateControls()
                If AudioCL.Count > 0 Then
                    AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
                End If
                'End If
                LastKeyDown = -1
            Case Keys.Cancel
                g.RaiseAppEvent(ApplicationEvent.ApplicationExit)
                g.MainForm.Close()
                Application.Exit()
        End Select
    End Sub
    'Private Sub AudioConverterForm_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
    '    If e.KeyCode = Keys.Cancel Then
    '        g.RaiseAppEvent(ApplicationEvent.ApplicationExit)
    '        g.MainForm.Close()
    '        Application.Exit()
    '    End If
    'End Sub
    Public Sub DeleteRows()
        Dim srC As Integer = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim rC As Integer = AudioCL.Count
        If srC > 0 Then
            If rC = srC Then
                AudioSBL.Clear()
                AudioCL.Clear()
                AudioSBL.InnerListChanged()
                AudioSBL.ResetBindings()
                dgvAudio.Rows.Clear()
                UpdateControls()
                Exit Sub
            End If

            dgvAudio.SuspendLayout()
            'StatusText("Removing...")
            RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged

            If srC < 100 Then
                SyncLock AudioSBL
                    For Each row As DataGridViewRow In dgvAudio.SelectedRows
                        AudioSBL.RemoveAt(row.Index)
                        'dgvAudio.Rows.RemoveAt(row.Index)
                    Next
                End SyncLock
            Else
                AudioSBL.RaiseListChangedEvents = False
                Dim cL = AudioSBL.ToCircularList

                For i = rC - 1 To 0 Step -1
                    If dgvAudio.Rows.GetRowState(i) >= 96 Then ' Or (DataGridViewElementStates.Selected Or DataGridViewElementStates.Visible)
                        cL.RemoveAt(i)
                    End If
                Next
                AudioCL.Reset()
                AudioSBL.InnerListChanged()
                AudioCL.Capacity = rC - srC
                AudioCL.AddRange(cL)
                AudioSBL.InnerListChanged()
                AudioSBL.RaiseListChangedEvents = True
                AudioSBL.ResetBindings()
            End If

            rC = AudioCL.Count
            IndexHeaderRows()
            If rC < 100 Then AutoSizeColumns()
            PopulateCache()
            Dim crI As Integer = dgvAudio.CurrentCellAddress.Y

            If crI = rC - 1 Then
                dgvAudio.Rows(rC - 1).Selected = True
                dgvAudio.ResumeLayout()
                UpdateControls(1, rC - 1)
            Else
                dgvAudio.ResumeLayout()
                UpdateControls(1, crI)
            End If
            AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        End If
    End Sub

    Private Sub bnAudioUp_Click(sender As Object, e As EventArgs) Handles bnAudioUp.Click
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        PopulateTaskS = True
        'dgvAudio.MoveSelectionUp

        Dim pos = dgvAudio.CurrentCellAddress.Y
        Dim current = AudioSBL(pos)
        SyncLock AudioSBL
            AudioSBL.RemoveAt(pos)
            pos -= 1
            AudioSBL.Insert(pos, current)
        End SyncLock
        dgvAudio.CurrentCell = dgvAudio.Rows(pos).Cells(dgvAudio.CurrentCellAddress.X)

        IndexHeaderRows()
        PopulateCache()
        UpdateControls(CurrentRowI:=pos)
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
    End Sub
    Private Sub bnAudioDown_Click(sender As Object, e As EventArgs) Handles bnAudioDown.Click
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        PopulateTaskS = True
        'dgvAudio.MoveSelectionDown

        Dim pos = dgvAudio.CurrentCellAddress.Y
        Dim current = AudioSBL(pos)
        SyncLock AudioSBL
            AudioSBL.RemoveAt(pos)
            pos += 1
            AudioSBL.Insert(pos, current)
        End SyncLock
        dgvAudio.CurrentCell = dgvAudio.Rows(pos).Cells(dgvAudio.CurrentCellAddress.X)

        IndexHeaderRows()
        PopulateCache()
        UpdateControls(CurrentRowI:=pos)
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
    End Sub

    Private Sub bnAudioPlay_Click(sender As Object, e As EventArgs) Handles bnAudioPlay.Click
        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.CurrentCellAddress.Y
        g.Play(AudioSBL(dgvAudio.CurrentCellAddress.Y).File)
    End Sub

    Private Sub bnAudioMediaInfo_Click(sender As Object, e As EventArgs) Handles bnAudioMediaInfo.Click
        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.CurrentCellAddress.Y
        g.DefaultCommands.ShowMediaInfo(AudioSBL(dgvAudio.CurrentCellAddress.Y).File)
    End Sub

    Private Sub bnCMD_Click(sender As Object, e As EventArgs) Handles bnCMD.Click
        Dim ap = AudioSBL(dgvAudio.CurrentCellAddress.Y)
        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.CurrentCellAddress.Y

        Select Case ap.GetType
            Case GetType(GUIAudioProfile)
                g.ShowCommandLinePreview("Command Line", (TryCast(ap, GUIAudioProfile)?.GetCommandLine(True)))
            Case GetType(BatchAudioProfile)
                g.ShowCommandLinePreview("Command Line", (TryCast(ap, BatchAudioProfile)?.GetCode))
        End Select
    End Sub

    Private Sub numThreads_ValueChanged(numEdit As NumEdit) Handles numThreads.ValueChanged
        MaxThreads = CInt(numThreads.Value)
        If numThreads.Value > CPUs Then
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
            dgvAudio.Columns(0).FillWeight = 50
            dgvAudio.Columns(1).FillWeight = 100
            dgvAudio.Columns(2).FillWeight = 400
            dgvAudio.Columns(3).FillWeight = 500
            dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ElseIf dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill Then
            dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        End If
        IndexHeaderRows()
        AutoSizeColumns()
        UpdateControls()
    End Sub

    Public Sub tcMain_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles tcMain.MouseDoubleClick 'Debug
        'debug
        PopulateTaskS = True
        Thread.Sleep(90)
        AudioSBL.RaiseListChangedEvents = False
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        ''''''''' Remove all handlers from control  'Dim gt = GetType(MenuButton)
        'Dim f1 As FieldInfo = GetType(DataGridViewEx).GetField("SelectionChanged", BindingFlags.Instance Or BindingFlags.NonPublic)
        Dim f1 As FieldInfo = GetType(DataGridView).GetField("EVENT_DATAGRIDVIEWSELECTIONCHANGED", BindingFlags.Static Or BindingFlags.NonPublic)
        Dim obj As Object = f1.GetValue(dgvAudio)
        Dim pi As PropertyInfo = dgvAudio.GetType().GetProperty("Events", BindingFlags.Instance Or BindingFlags.NonPublic)
        Dim list As EventHandlerList = DirectCast(pi.GetValue(dgvAudio, Nothing), EventHandlerList)
        list.RemoveHandler(obj, list(obj))

        SyncLock AudioSBL
            MediaInfo.ClearCache()
            AudioCL.TrimExcess()
            AudioSBL.InnerListChanged()
        End SyncLock

        AudioSBL.RaiseListChangedEvents = True
        AudioSBL.ResetBindings()
        Task.Delay(90).Wait()

        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(2, GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()
        Console.Beep(700, 50)
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(2, GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()

        dgvAudio.Refresh()
        Refresh()
        IndexHeaderRows()
        AutoSizeColumns()
        UpdateControls()
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        PopulateIter = 2
        PopulateCache()
        LastKeyDown = -1
        dgvAudio.SelectAll()

        If StatUpdateTask Is Nothing OrElse StatUpdateTask.IsCompleted Then      'debug
            StatUpdateTask = Task.Factory.StartNew(StatUpdateTaskA, TaskCreationOptions.LongRunning)
        End If

    End Sub

End Class
