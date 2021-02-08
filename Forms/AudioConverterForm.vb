Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports KGySoft.Collections
Imports KGySoft.ComponentModel
Imports KGySoft.CoreLibraries
Imports Microsoft.VisualBasic
Imports StaxRip.UI

Public Class AudioConverterForm
    Inherits FormBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        PopulateTaskS = -1
        PopulateIter = 999
        AudioConverterMode = False
        SWt = Nothing
        PopulateRSW = Nothing
        PopulateWSW = Nothing

        If Not (AudioSBL Is Nothing) Then
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
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents laAC As Label
    Friend WithEvents bnMenuAudio As ButtonEx
    Friend WithEvents dgvAudio As DataGridViewEx
    Friend WithEvents bnSort As Button
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.laThreads = New System.Windows.Forms.Label()
        Me.numThreads = New StaxRip.UI.NumEdit()
        Me.bnCMD = New StaxRip.UI.ButtonEx()
        Me.bnAudioMediaInfo = New StaxRip.UI.ButtonEx()
        Me.bnAudioEdit = New StaxRip.UI.ButtonEx()
        Me.bnAudioConvert = New StaxRip.UI.ButtonEx()
        Me.bnAudioRemove = New StaxRip.UI.ButtonEx()
        Me.bnMenuAudio = New StaxRip.UI.ButtonEx()
        Me.laAC = New System.Windows.Forms.Label()
        Me.bnSort = New System.Windows.Forms.Button()
        Me.bnAudioAdd = New StaxRip.UI.ButtonEx()
        Me.bnAudioPlay = New StaxRip.UI.ButtonEx()
        Me.bnAudioDown = New StaxRip.UI.ButtonEx()
        Me.bnAudioUp = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.tlpMain.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'laThreads
        '
        Me.laThreads.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laThreads.BackColor = System.Drawing.SystemColors.Control
        Me.laThreads.CausesValidation = False
        Me.tlpMain.SetColumnSpan(Me.laThreads, 2)
        Me.laThreads.Location = New System.Drawing.Point(402, 330)
        Me.laThreads.Margin = New System.Windows.Forms.Padding(3)
        Me.laThreads.Name = "laThreads"
        Me.laThreads.Size = New System.Drawing.Size(154, 15)
        Me.laThreads.TabIndex = 97
        Me.laThreads.Text = "Threads :"
        Me.laThreads.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.laThreads, "Number of parallel processes, set the default in settings")
        Me.laThreads.UseMnemonic = False
        '
        'numThreads
        '
        Me.numThreads.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.numThreads.BackColor = System.Drawing.SystemColors.Control
        Me.numThreads.CausesValidation = False
        Me.numThreads.Location = New System.Drawing.Point(562, 327)
        Me.numThreads.Margin = New System.Windows.Forms.Padding(3, 1, 3, 0)
        Me.numThreads.Maximum = 32.0R
        Me.numThreads.Minimum = 1.0R
        Me.numThreads.Name = "numThreads"
        Me.numThreads.Size = New System.Drawing.Size(42, 21)
        Me.numThreads.TabIndex = 98
        Me.numThreads.TabStop = False
        Me.numThreads.Tag = "No. threads"
        Me.TipProvider.SetTipText(Me.numThreads, "Number of parallel processes, set the default in settings")
        '
        'bnCMD
        '
        Me.bnCMD.CausesValidation = False
        Me.bnCMD.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnCMD.Location = New System.Drawing.Point(742, 351)
        Me.bnCMD.Size = New System.Drawing.Size(84, 26)
        Me.bnCMD.Text = "   C&MD"
        Me.TipProvider.SetTipText(Me.bnCMD, "Show Command Line")
        '
        'bnAudioMediaInfo
        '
        Me.bnAudioMediaInfo.CausesValidation = False
        Me.bnAudioMediaInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioMediaInfo.Location = New System.Drawing.Point(652, 351)
        Me.bnAudioMediaInfo.Size = New System.Drawing.Size(84, 26)
        Me.bnAudioMediaInfo.Text = "   &Info"
        Me.TipProvider.SetTipText(Me.bnAudioMediaInfo, "Media Info")
        '
        'bnAudioEdit
        '
        Me.bnAudioEdit.CausesValidation = False
        Me.bnAudioEdit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioEdit.Location = New System.Drawing.Point(562, 351)
        Me.bnAudioEdit.Size = New System.Drawing.Size(84, 26)
        Me.bnAudioEdit.Text = "    &Edit..."
        Me.TipProvider.SetTipText(Me.bnAudioEdit, "Edit Audio Profile for selection")
        '
        'bnAudioConvert
        '
        Me.bnAudioConvert.CausesValidation = False
        Me.bnAudioConvert.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioConvert.Location = New System.Drawing.Point(238, 351)
        Me.bnAudioConvert.Size = New System.Drawing.Size(88, 26)
        Me.bnAudioConvert.Text = "&Convert..."
        Me.bnAudioConvert.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.bnAudioConvert, "Convert Selection")
        '
        'bnAudioRemove
        '
        Me.bnAudioRemove.CausesValidation = False
        Me.bnAudioRemove.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioRemove.Location = New System.Drawing.Point(144, 351)
        Me.bnAudioRemove.Size = New System.Drawing.Size(88, 26)
        Me.bnAudioRemove.Text = "&Remove "
        Me.bnAudioRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.bnAudioRemove, "Hold <Shift+Control> to delete from disk")
        '
        'bnMenuAudio
        '
        Me.bnMenuAudio.CausesValidation = False
        Me.bnMenuAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnMenuAudio.Location = New System.Drawing.Point(5, 350)
        Me.bnMenuAudio.Margin = New System.Windows.Forms.Padding(2)
        Me.bnMenuAudio.ShowMenuSymbol = True
        Me.bnMenuAudio.Size = New System.Drawing.Size(40, 28)
        Me.TipProvider.SetTipText(Me.bnMenuAudio, "Click to open menu")
        Me.bnMenuAudio.UseMnemonic = False
        '
        'laAC
        '
        Me.laAC.CausesValidation = False
        Me.tlpMain.SetColumnSpan(Me.laAC, 3)
        Me.laAC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.laAC.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.laAC.Location = New System.Drawing.Point(47, 327)
        Me.laAC.Margin = New System.Windows.Forms.Padding(0, 1, 0, 1)
        Me.laAC.Name = "laAC"
        Me.laAC.Size = New System.Drawing.Size(282, 20)
        Me.laAC.TabIndex = 99
        Me.laAC.Text = "Please add or drag music files..."
        Me.laAC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.TipProvider.SetTipText(Me.laAC, "Double Click to Clean-Up")
        Me.laAC.UseMnemonic = False
        '
        'bnSort
        '
        Me.bnSort.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.bnSort.CausesValidation = False
        Me.bnSort.FlatAppearance.BorderSize = 0
        Me.bnSort.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnSort.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.bnSort.Location = New System.Drawing.Point(658, 327)
        Me.bnSort.Margin = New System.Windows.Forms.Padding(3, 1, 3, 0)
        Me.bnSort.Name = "bnSort"
        Me.bnSort.Size = New System.Drawing.Size(72, 21)
        Me.bnSort.TabIndex = 49
        Me.bnSort.Text = "&View mode"
        Me.TipProvider.SetTipText(Me.bnSort, "Switches to other auto-size columns mode")
        Me.bnSort.UseVisualStyleBackColor = True
        '
        'bnAudioAdd
        '
        Me.bnAudioAdd.CausesValidation = False
        Me.bnAudioAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioAdd.Location = New System.Drawing.Point(50, 351)
        Me.bnAudioAdd.Size = New System.Drawing.Size(88, 26)
        Me.bnAudioAdd.Text = "    &Add..."
        '
        'bnAudioPlay
        '
        Me.bnAudioPlay.CausesValidation = False
        Me.bnAudioPlay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioPlay.Location = New System.Drawing.Point(472, 351)
        Me.bnAudioPlay.Size = New System.Drawing.Size(84, 26)
        Me.bnAudioPlay.Text = "   &Play"
        '
        'bnAudioDown
        '
        Me.bnAudioDown.CausesValidation = False
        Me.bnAudioDown.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioDown.Location = New System.Drawing.Point(402, 351)
        Me.bnAudioDown.Size = New System.Drawing.Size(64, 26)
        Me.bnAudioDown.Text = "&Down"
        Me.bnAudioDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bnAudioUp
        '
        Me.bnAudioUp.CausesValidation = False
        Me.bnAudioUp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioUp.Location = New System.Drawing.Point(332, 351)
        Me.bnAudioUp.Size = New System.Drawing.Size(64, 26)
        Me.bnAudioUp.Text = "  &Up"
        '
        'tlpMain
        '
        Me.tlpMain.CausesValidation = False
        Me.tlpMain.ColumnCount = 11
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.dgvAudio, 0, 0)
        Me.tlpMain.Controls.Add(Me.laAC, 1, 1)
        Me.tlpMain.Controls.Add(Me.laThreads, 5, 1)
        Me.tlpMain.Controls.Add(Me.numThreads, 7, 1)
        Me.tlpMain.Controls.Add(Me.bnMenuAudio, 0, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioAdd, 1, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioRemove, 2, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioConvert, 3, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioUp, 4, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioDown, 5, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioPlay, 6, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioEdit, 7, 2)
        Me.tlpMain.Controls.Add(Me.bnCMD, 9, 2)
        Me.tlpMain.Controls.Add(Me.bnSort, 8, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioMediaInfo, 8, 2)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.tlpMain.Size = New System.Drawing.Size(832, 383)
        Me.tlpMain.TabIndex = 100
        '
        'dgvAudio
        '
        Me.dgvAudio.AllowDrop = True
        Me.dgvAudio.AllowUserToAddRows = False
        Me.dgvAudio.AllowUserToDeleteRows = False
        Me.dgvAudio.AllowUserToOrderColumns = True
        Me.dgvAudio.AllowUserToResizeRows = False
        Me.dgvAudio.CausesValidation = False
        Me.tlpMain.SetColumnSpan(Me.dgvAudio, 11)
        Me.dgvAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAudio.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvAudio.Location = New System.Drawing.Point(6, 6)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.ReadOnly = True
        Me.dgvAudio.RowHeadersWidth = 24
        Me.dgvAudio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAudio.ShowCellErrors = False
        Me.dgvAudio.ShowEditingIcon = False
        Me.dgvAudio.ShowRowErrors = False
        Me.dgvAudio.Size = New System.Drawing.Size(820, 317)
        Me.dgvAudio.StandardTab = True
        Me.dgvAudio.TabIndex = 1
        '
        'AudioConverterForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(832, 383)
        Me.Controls.Add(Me.tlpMain)
        Me.DoubleBuffered = True
        Me.KeyPreview = True
        Me.Name = "AudioConverterForm"
        Me.Text = "AudioConverter"
        Me.tlpMain.ResumeLayout(False)
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WithEvents CMS As ContextMenuStripEx
    Public AudioCL As CircularList(Of AudioProfile)
    Public AudioSBL As SortableBindingList(Of AudioProfile)
    Private StatTextSB As New StringBuilder(44)
    Private OutPath As String
    Private LastLogPath As String
    Private Shared LogHeader As Boolean
    Private SelectChangeES As Boolean
    Private LastKeyDown As Integer = -1
    Private ButtonAltImgDone As Boolean
    Private ButtonAltImageT As Task
    Private PopulateTask As Task
    Private PopulateTaskW As Task
    Private PopulateTaskS As Integer
    Private PopulateWSW As New Stopwatch
    Private PopulateRSW As New Stopwatch
    Private PopulateIter As Integer
    ' Private IndexTask As Task
    Private IndexSWatch As New Stopwatch
    Private SLock As New Object
    Private ReadOnly CPUs As Integer = Environment.ProcessorCount
    Public Shared MaxThreads As Integer
    Public Shared AudioConverterMode As Boolean


    Private CellVNeededCount As Integer


    Private SWt As New Stopwatch
    Private StatUpdateTask As Task
    Private ReadOnly StatUpdateTaskA As Action =
        Sub()
            Do While PopulateIter < 900 AndAlso AudioConverterMode AndAlso Not Me.IsDisposed
                Dim t As String = PopulateIter.ToInvString & PopulateTask?.Status.ToString & PopulateWSW.ElapsedTicks / 10000 & "msW" & PopulateTaskS.ToInvString & "PopTS" & PopulateRSW.ElapsedTicks / 10000 & "msR|WaitPT" & PopulateTaskW?.Status.ToString & IndexSWatch.ElapsedTicks / 10000 & "msIdx" & SWt.ElapsedTicks / 10000 & "VC" & CellVNeededCount.ToInvString
                Me.BeginInvoke(Sub() Me.Text = t)
                'Me.Text = t
                'Console.Beep(1800, 15)
                Thread.Sleep(75)
            Loop
            If AudioConverterMode AndAlso Not Me.IsDisposed Then Me.BeginInvoke(Sub() Me.Text = "AudioConverter")
        End Sub

    Public Sub New()
        MyBase.New()
        'RemoveHandler Application.ThreadException, AddressOf g.OnUnhandledException
        'AddHandler Application.ThreadException, AddressOf g.OnUnhandledException

        Icon = g.Icon
        InitializeComponent()
        SetMinimumSize(22, 12)
        RestoreClientSize(53, 29)

        'If Debugger.IsAttached Then Control.CheckForIllegalCrossThreadCalls = False

        AudioCL = New CircularList(Of AudioProfile)(16)
        AudioSBL = New SortableBindingList(Of AudioProfile)(AudioCL) With {.CheckConsistency = False, .SortOnChange = False, .RaiseListChangedEvents = True, .AllowNew = False}
        'AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks) 'AudioSBL = New SortableBindingList(Of AudioProfile)(ObjectHelp.GetCopy(p.AudioTracks)) 

        GetType(DataGridViewEx).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or
            BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, dgvAudio, New Object() {True})

        dgvAudio.RowTemplate.Height = Font.Height + 4 'or *1.25 ?, AutoResize=20, def=22
        dgvAudio.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))

        dgvAudio.DefaultCellStyle.DataSourceNullValue = Nothing
        dgvAudio.DefaultCellStyle.FormatProvider = CultureInfo.InvariantCulture

        'dgvAudio.RowHeadersWidth = 24 ' (0)=42 +6 per number char
        dgvAudio.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        dgvAudio.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvAudio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAudio.AutoGenerateColumns = False
        dgvAudio.DataBindings.Clear()
        dgvAudio.VirtualMode = True
        dgvAudio.DataSource = AudioSBL
        'dgvAudio.DataSource = AudioVSBL

        bnSort.ForeColor = Color.FromArgb(&HFF004BFF)
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
        indexColumn.SortMode = DataGridViewColumnSortMode.NotSortable
        'indexColumn.DataPropertyName = "RowIdx"
        indexColumn.HeaderText = "No."
        'indexColumn.ValueType = GetType(String)
        indexColumn.FillWeight = 10

        Dim profileName = dgvAudio.AddTextBoxColumn()
        profileName.SortMode = DataGridViewColumnSortMode.Automatic
        profileName.DataPropertyName = "Name"
        profileName.HeaderText = "Profile"
        profileName.FillWeight = 100

        Dim dispNameColumn = dgvAudio.AddTextBoxColumn()
        dispNameColumn.SortMode = DataGridViewColumnSortMode.Automatic
        dispNameColumn.DataPropertyName = "DisplayName"
        dispNameColumn.HeaderText = "Track"
        dispNameColumn.FillWeight = 400

        Dim pathColumn = dgvAudio.AddTextBoxColumn()
        pathColumn.SortMode = DataGridViewColumnSortMode.Automatic
        pathColumn.DataPropertyName = "File"
        pathColumn.HeaderText = "Full Path"
        pathColumn.FillWeight = 500

        For Each col As DataGridViewColumn In dgvAudio.Columns
            col.MinimumWidth = col.HeaderText.Length * 8 + 5
            col.ReadOnly = True
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
        'KeyPreview = False
        dgvAudio.Columns.Item(3).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        UpdateControls()
        bnAudioAdd.Select()
        AudioConverterMode = True

        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(2, GCCollectionMode.Forced, True, True)
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        PopulateTaskS = -1
        PopulateIter = 999
        RemoveHandlersDGV(True)
        SWt = Nothing
        PopulateRSW = Nothing
        PopulateWSW = Nothing
        AudioSBL.Clear()
        dgvAudio.Columns.Clear()
        AudioConverterMode = False
        MyBase.OnFormClosing(e)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        SaveConverterLog()
        MyBase.OnFormClosed(e)
    End Sub
    Protected Overrides Sub OnDragEnter(e As DragEventArgs)
    End Sub
    Protected Overrides Sub OnDragDrop(args As DragEventArgs)
    End Sub

    'Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean ' Really Needed????
    '    Get
    '        If ProcController.BlockActivation Then
    '            ProcController.BlockActivation = False
    '            If ProcController.IsLastActivationLessThan(60) Then Return True
    '        End If
    '        Return MyBase.ShowWithoutActivation
    '    End Get
    'End Property

    Private Sub UpdateCMS(sender As Object, e As CancelEventArgs) Handles CMS.Opening
        Dim rC = AudioCL.Count
        Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim crI As Integer = dgvAudio.CurrentCellAddress.Y

        'If rC > 0 Then
        RemoveHandlersDGV(True)
        tlpMain.SuspendLayout()
        SuspendLayout()
        AudioSBL.RemoveSort()
        dgvAudio.DataSource = Nothing
        AudioSBL.RaiseListChangedEvents = False
        'Dim clt = AudioSBL.ToCircularList

        For n = 1 To 10
            Thread.Sleep(15)
            Application.DoEvents()
            Task.Delay(15)
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Task.Delay(15)
        Next n

        SWt.Restart()
        For i = 1 To 1000000

            StatTextSB.Clear()
            StatTextSB.Append("Pos: ").Append(i + 2).Append("  |  Sel: ").Append(i - 500000).Append(" / Tot: ").Append(rC * 3)

        Next i
        SWt.Stop()

        Dim tt1 = SWt.ElapsedTicks / 10000

        For n = 1 To 10
            Thread.Sleep(15)
            Application.DoEvents()
            Task.Delay(15)
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Task.Delay(15)
        Next n

        SWt.Restart()

        For i = 1 To 1000000

            StatTextSB.Clear()
            '  StatTextSB.AppendFormat(("Pos: ").Append(i + 2).Append("  |  Sel: ").Append(i - 500000).Append(" / Tot: ").Append(rC * 3)
            i.ToInvString
        Next i
        SWt.Stop()

        AudioSBL.InnerListChanged()
        AudioSBL.RaiseListChangedEvents = True
        dgvAudio.DataSource = AudioSBL
        AudioSBL.ResetBindings()
        tlpMain.ResumeLayout(True)
        ResumeLayout(True)
        dgvAudio.Refresh()
        Refresh()
        'AddHandlersDGV()
        Task.Delay(15)
        MsgInfo($"10 000 000 It ToStringInv {tt1}ms", $"ToStringCult: {SWt.ElapsedTicks / 10000}ms")
        'End If


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
                                  PopulateTaskS = -1
                                  AudioSBL.Clear()
                                  AudioCL.Clear()
                                  AudioSBL.InnerListChanged()
                                  AudioSBL.ResetBindings()
                                  dgvAudio.Rows.Clear()
                                  UpdateControls()
                                  GC.Collect()
                              End Sub, isR).SetImage(Symbol.Clear)
        CMS.Add("Remove duplicates", Sub()
                                         PopulateTaskS = -1
                                         RemoveHandlersDGV(True)
                                         Dim occ As Integer = dgvAudio.CurrentCellAddress.X
                                         Dim ocF As String = cAP.File
                                         'Dim uAR = AudioCL.Union(AudioCL).ToArray
                                         Dim uAR = AudioCL.Union(AudioCL, New AudioProfileComparer).ToArray

                                         If AudioCL.Count <= uAR.Length Then
                                             MsgInfo("No duplicates found.")

                                         ElseIf MsgQuestion(AudioCL.Count - uAR.Length & " duplicated file(s) found." & BR & "Remove from Grid View ?") = DialogResult.OK Then
                                             StatusText("Removing duplicated files...")
                                             AudioSBL.RemoveSort()
                                             dgvAudio.DataSource = Nothing
                                             dgvAudio.DataBindings.Clear()
                                             AudioSBL.RaiseListChangedEvents = False
                                             'For i = 0 To uAR.Length - 1
                                             '    uAR(i).RowIdx = i + 1
                                             'Next i
                                             AudioSBL.Clear()
                                             AudioSBL.AddRange(uAR)
                                             AudioSBL.RaiseListChangedEvents = True
                                             dgvAudio.DataSource = AudioSBL
                                             AudioSBL.ResetBindings()
                                             If ocF IsNot Nothing Then  ' AndAlso uAR.Contains(ocF)
                                                 dgvAudio.CurrentCell = dgvAudio.Rows.Item(Array.FindIndex(uAR, Function(itm) itm.File.Equals(ocF))).Cells.Item(occ)
                                             End If
                                             AutoSizeColumns()
                                         End If

                                         PopulateCache(3000000)
                                         UpdateControls()
                                         AddHandlersDGV()
                                     End Sub, isCR, "Removes duplicated files from list").SetImage(Symbol.BulletedList)

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
        SelectChangeES = False
        ButtonAltImage(False)
        e.Cancel = False
    End Sub

    Private Sub UpdateControls(Optional SelRowsCount As Integer = -1, Optional CurrentRowI As Integer = -1)
        SWt.Restart()
        Dim rC As Integer = AudioCL.Count
        If SelRowsCount = -1 Then SelRowsCount = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        If CurrentRowI = -1 Then CurrentRowI = dgvAudio.CurrentCellAddress.Y
        StatTextSB.Clear()
        StatTextSB.Append("Pos: ").Append(CurrentRowI + 1).Append("  |  Sel: ").Append(SelRowsCount).Append(" / Tot: ").Append(rC)
        Me.BeginInvoke(Sub()
                           'IndexSWatch.Restart()
                           If rC > 0 Then
                               'laAC.Text = "Pos: " & (CurrentRowI + 1).ToInvString & "  |  Sel: " & (SelRowsCount).ToInvString & " / Tot: " & (rC).ToInvString
                               laAC.Text = StatTextSB.ToString
                               numThreads.Enabled = True  ' Or 1 selected
                           Else
                               laAC.Text = "Please add or drag music files..."
                               numThreads.Enabled = False
                           End If
                           bnAudioRemove.Enabled = SelRowsCount > 0
                           bnAudioConvert.Enabled = SelRowsCount > 0
                           bnAudioUp.Enabled = SelRowsCount = 1 AndAlso CurrentRowI > 0
                           bnAudioDown.Enabled = SelRowsCount = 1 AndAlso CurrentRowI < rC - 1
                           bnAudioPlay.Enabled = CurrentRowI > -1
                           bnAudioEdit.Enabled = SelRowsCount > 0
                           bnAudioMediaInfo.Enabled = CurrentRowI > -1
                           bnCMD.Enabled = CurrentRowI > -1
                           ' IndexSWatch.Stop()
                       End Sub)
        SWt.Stop()
    End Sub

    Private Sub StatusText(InfoText As String)
        laAC.Text = InfoText
        laAC.Refresh()
    End Sub

    Private Sub AutoSizeColumns(Optional AllCells As Boolean = False)
        StatusText("Auto Resizing Columns...")
        dgvAudio.Columns.Item(0).MinimumWidth = Math.Max(28, CInt(14 + 6 * Fix(Math.Log10(AudioCL.Count + 1))))
        dgvAudio.ColumnHeadersHeight = 23
        If dgvAudio.AutoSizeColumnsMode <> DataGridViewAutoSizeColumnsMode.Fill Then
            PopulateTaskS = -1
            dgvAudio.AutoResizeColumns(If(AudioCL.Count < 2000 OrElse AllCells, DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader, DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader))
        End If
        dgvAudio.RowHeadersWidth = 24
    End Sub

    Private Sub dgvAudio_SelectionChanged(sender As Object, e As EventArgs)
        If SelectChangeES Then Return
        PopulateTaskS = 0
        'Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Task.Run(Sub()
                     UpdateControls()
                     PopulateCache(5000000)
                 End Sub)
        'scT.ContinueWith(Sub()
        '                     If scT.Exception IsNot Nothing Then
        '                         Console.Beep(30, 7500)
        '                         Log.Write("SelectionChanged Exception", scT.Exception.ToString & scT.Exception.InnerException?.ToString)
        '                     End If
        '                 End Sub)
    End Sub

    Private Sub dgvAudio_Scroll(sender As Object, e As ScrollEventArgs)
        'Task.Run(Sub() Log.WriteLine("--->ScrollEvents: " & e.NewValue.ToString & " :newv | oldv : " & e.OldValue.ToString & e.Type.ToString & " :type | scrollOrient : " & e.ScrollOrientation.ToString))
        Select Case e.Type
            Case ScrollEventType.LargeDecrement, ScrollEventType.LargeIncrement, ScrollEventType.ThumbTrack
                PopulateTaskS = 0
                PopulateCache(12000000)
        End Select
    End Sub

    Private Sub dgvAudio_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseClick
        If Not SelectChangeES AndAlso e.RowIndex = -1 AndAlso e.ColumnIndex > 0 AndAlso e.Button = MouseButtons.Left AndAlso AudioCL.Count > 0 Then
            PopulateTaskS = -1
            SelectChangeES = True
            If AudioCL.Count > 100 Then StatusText("Sorting...")
        End If
    End Sub

    Private Sub dgvAudio_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseUp
        If SelectChangeES AndAlso e.RowIndex = -1 AndAlso e.ColumnIndex > -1 AndAlso e.Button = MouseButtons.Left AndAlso AudioCL.Count > 0 Then
            Task.Run(Sub()
                         'If Not IndexTask?.IsCompleted Then IndexTask.Wait()
                         UpdateControls(SelRowsCount:=1)
                         SelectChangeES = False
                     End Sub)
        End If
    End Sub

    Private Sub dgvAudio_CellValueNeeded(sender As Object, e As DataGridViewCellValueEventArgs) Handles dgvAudio.CellValueNeeded
        'IndexSWatch.Restart()
        'If e.ColumnIndex = 0 Then 
        '    'If (e.State And DataGridViewElementStates.Displayed) = DataGridViewElementStates.Displayed Then 'e.State << 31 = Integer.MinValue AndAlso e.RowIndex > -1 AndAlso e.RowIndex < AudioCL.Count
        CellVNeededCount += 1
        '    'laThreads.Text = (e.RowIndex).ToInvString
        '    'Console.Beep(2500, 30)
        e.Value = (e.RowIndex + 1).ToInvString
        'IndexSWatch.Stop()
    End Sub

    Private Sub dgvAudio_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvAudio.CellFormatting
        'IndexSWatch.Restart()
        'If e.CellStyle.DataSourceNullValue IsNot Nothing OrElse e.CellStyle.FormatProvider IsNot CultureInfo.InvariantCulture Then Console.Beep(3700, 10)
        'If e.ColumnIndex <> 0 Then
        e.FormattingApplied = True
        'IndexSWatch.Stop()
    End Sub

    'Private Sub dgvAudio_RowUnshared(sender As Object, e As DataGridViewRowEventArgs) Handles dgvAudio.RowUnshared
    '    laThreads.Text = "UnShI:" & e.Row.Index() + 1
    '    laThreads.Refresh()
    '    'If e.Row.Index Mod 3 = 0 Then 
    '    Console.Beep(2500, 45)
    '    'e.Row.HeaderCell.Value = (e.Row.Index + 1).ToInvString
    'End Sub

    'Private Sub IndexHeaderRows()
    'Dim ta As Action = Sub()
    '                       'IndexSWatch.Restart()
    '                       'If AudioCL.Count >= 1600 Then
    '                       '    Parallel.ForEach(Partitioner.Create(0, AudioCL.Count), New ParallelOptions With {.MaxDegreeOfParallelism = CInt(CPUs / 2)},
    '                       '                     Sub(range)
    '                       '                         For r As Integer = range.Item1 To range.Item2 - 1
    '                       '                             AudioSBL.Item(r).RowIdx = r + 1
    '                       '                         Next r
    '                       '                     End Sub)
    '                       'Else
    '                       '    For r As Integer = 0 To AudioCL.Count - 1
    '                       '        AudioSBL.Item(r).RowIdx = r + 1
    '                       '    Next r
    '                       'End If
    '                       'IndexSWatch.Stop()
    '                   End Sub
    'If IndexTask Is Nothing OrElse IndexTask.IsCompleted Then
    '    IndexTask = Task.Run(Sub() ta())
    '    'IndexTask = Task.Run(Sub() ta())
    '    'IndexTask.ContinueWith(Sub()   'debug 
    '    '                           IndexSWatch.Stop()
    '    '                           If IndexTask.Exception IsNot Nothing Then
    '    '                               Log.Write("Index Task Cont Exceptions", $"{IndexTask.Exception} {PopulateTaskS} {PopulateIter} {PopulateTask.Status} {PopulateWSW?.ElapsedTicks / 10000}ms-WPopulateT {PopulateRSW?.ElapsedTicks / 10000}ms-R|WaitPT:{PopulateTaskW?.Status.ToString} IndexTS:{IndexTask.Status}{IndexSWatch?.ElapsedTicks / 10000}msIdx")
    '    '                               Console.Beep(2500, 50)
    '    '                           End If
    '    '                       End Sub)
    ' End If
    'End Sub
    'Private Sub dgvAudio_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvAudio.RowPrePaint
    '    'If (e.State And DataGridViewElementStates.Displayed) = DataGridViewElementStates.Displayed Then 'e.State << 31 = Integer.MinValue AndAlso e.RowIndex > -1 AndAlso e.RowIndex < AudioCL.Count
    '    AudioSBL.Item(e.RowIndex).RowIdx = e.RowIndex + 1
    'End Sub
    'Private Sub dgvAudio_Sorted(sender As Object, e As EventArgs) Handles dgvAudio.Sorted
    '    IndexHeaderRows()
    'End Sub
    'debug
    'Private Sub dgvAudio_Layout(sender As Object, e As LayoutEventArgs) Handles dgvAudio.Layout
    '    Log.Write("Layout Events:", e.AffectedComponent?.ToString & " <-Component|Ctrl-> " & e.AffectedControl?.ToString & " Property: " & e.AffectedProperty?.ToString)
    '    me.Text = "Layout Events:" & e.AffectedComponent?.ToString & " <-Component|Ctrl-> " & e.AffectedControl?.ToString & " Property: " & e.AffectedProperty?.ToString
    'End Sub
    'Private Sub dgvAudio_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAudio.DataError
    '    Task.Run(Sub()
    '                 Log.Write("DGV DataErrorException:cr " & e.ColumnIndex & e.RowIndex, e.Exception.ToString & e.ToString & e.Context.ToString)
    '                 If Date.Now.Millisecond Mod 2 = 0 Then Console.Beep(6000, 30)
    '             End Sub)
    '    e.Cancel = True
    '    'e.ThrowException = False
    'End Sub
    Private Sub SaveConverterLog(Optional LogPath As String = Nothing, Optional UseDialog As Boolean = False)
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

    Private Function FindDuplicates(SelectedOnly As Boolean) As Integer
        Dim cDup As Integer
        Dim cn As Integer

        If SelectedOnly Then
            cn = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
            If cn < 1 Then Return 0
            Dim selArr(cn - 1) As String
            Dim i As Integer
            For r = 0 To AudioCL.Count - 1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    selArr(i) = AudioSBL.Item(r).File
                    i += 1
                End If
            Next
            cDup = selArr.Union(selArr).Count
        Else
            Dim ar = AudioCL.ToArray
            cn = ar.Length
            If cn < 1 Then Return 0
            cDup = ar.Union(ar, New AudioProfileComparer).Count
        End If

        If cDup < cn Then
            Return cn - cDup
        End If
        Return 0
    End Function

    Private Function RelativeSubDirRecursive(path As String, Optional SubDepth As Integer = 1) As String
        If SubDepth < 1 Then Return ""

        If path.Length <= 3 Then
            For Each ch In path
                ' If "/:\^".Contains(ch) Then
                If IO.Path.GetInvalidFileNameChars().Contains(ch) Then
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
                Thread.Sleep(15)
                If Not FileExists(nOutD & outP.FileName) Then  ' ap.File.FileName Prevent Overwrite ??? If File Exixst???
                    FileHelp.Move(outP, nOutD & outP.FileName)
                Else
                    Log.Write("Audio Converter error, not copied to Output ", outP & " Already exist in: " & nOutD & outP.FileName)
                End If
            Catch ex As Exception
                Log.Write("Audio Converter IO copy exception", ex.ToString)
            End Try

            Thread.Sleep(45)
            If Not DirExists(nOutD) Then
                Log.Write("Audio Converter error", "Failed to create output directory: " & nOutD)
            ElseIf Not FileExists(nOutD & outP.FileName) Then
                Log.Write("Audio Converter error", "Failed to create output file in path: " & nOutD & outP.FileName)
            End If
        Else
            If Not FileExists(outP) Then
                Log.Write("Audio Converter error", "Failed to create output file: " & outP)
            End If
        End If
    End Sub

    Private Sub bnAudioConvert_Click(sender As Object, e As EventArgs) Handles bnAudioConvert.Click
        Dim dc As Integer = FindDuplicates(True)
        If dc > 0 Then
            If MsgQuestion(dc & " duplicated file(s) found.  Are you sure ?") <> DialogResult.OK Then Exit Sub
        End If

        Using dialog As New FolderBrowserDialog With {
            .Description = "Please select output directory :",
            .UseDescriptionForTitle = True,
            .RootFolder = Environment.SpecialFolder.MyComputer}
            If dialog.ShowDialog <> DialogResult.OK Then Exit Sub
            If dialog.SelectedPath Is Nothing OrElse Not DirExists(dialog.SelectedPath.FixDir) Then
                Exit Sub
            End If
            PopulateTaskS = -1
            OutPath = dialog.SelectedPath.FixDir
        End Using

        p.TempDir = OutPath

        For r = 0 To AudioCL.Count - 1
            If dgvAudio.Rows.GetRowState(r) >= &B1100000 AndAlso AudioSBL.Item(r).File.Dir.Contains(OutPath) Then
                If MsgQuestion("One of source files is inside output directory.  Are you sure ?", TaskDialogButtons.YesNo) <> DialogResult.Yes Then
                    Exit Sub
                End If
                Exit For
            End If
        Next r

        Dim subDepth As Integer
        Using td As New TaskDialog(Of Integer) With {.MainInstruction = "How many source Sub Dirs use ?"}
            td.AddCommand("0 - All files in output dir", 0)
            For i = 1 To 5
                td.AddButton(i.ToInvString, i)
            Next
            td.SelectedValue = -1
            subDepth = td.Show
            If subDepth < 0 Then Exit Sub
        End Using

        Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)

        If MsgQuestion("Confirm to convert selected: " & srC & BR & "To path: " & OutPath & BR & "Sub Dirs: " & subDepth) = DialogResult.OK AndAlso DirExists(OutPath) Then
            Dim StartTime = Date.Now
            StatusText("Converting...")

            If Not LogHeader Then
                If Log.IsEmpty Then Log.WriteEnvironment()
                Log.WriteHeader("Audio Converter")
                LogHeader = True
            End If
            Log.WriteHeader("Audio Converter queue started: " & Date.Now.ToString)

            Dim ap As AudioProfile
            Dim actions As New List(Of Action)(srC)

            For r = 0 To AudioCL.Count - 1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    ap = AudioSBL.Item(r)

                    If FileExists(ap.File) AndAlso My.Computer.FileSystem.GetFileInfo(ap.File).Length > 10 Then
                        actions.Add(Sub() EncodeAudio(ap, subDepth))
                    End If

                End If
            Next r

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
            MainForm.Hide()
            Visible = True
            WindowState = FormWindowState.Normal
            Activate()
            dgvAudio.Select()
            dgvAudio.Refresh()
            Refresh()

            Dim OKCount As Integer
            Dim outFile As String

            For r = 0 To AudioCL.Count - 1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    ap = AudioSBL.Item(r)
                    outFile = OutPath & RelativeSubDirRecursive(ap.File.Dir, subDepth) & ap.GetOutputFile.FileName() 'ap.file.filename

                    If FileExists(outFile) AndAlso My.Computer.FileSystem.GetFileInfo(outFile).Length > 10 Then
                        dgvAudio.Rows.Item(r).HeaderCell.Value = "OK" ' - permament or not??
                        OKCount += 1
                    Else
                        dgvAudio.Rows.Item(r).HeaderCell.Value = "Error"
                    End If

                End If
            Next r

            If srC < OKCount Then
                If dgvAudio.RowHeadersWidth < 70 Then dgvAudio.RowHeadersWidth = 70
            Else
                If dgvAudio.RowHeadersWidth < 52 Then dgvAudio.RowHeadersWidth = 52
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
        Dim sortFT = Task.Run(Sub() Array.Sort(Files))

        If fC > 100 AndAlso MsgQuestion("Add " & fC & " files ?") <> DialogResult.OK Then
            Exit Sub
        End If

        PopulateTaskS = -1
        Dim rC As Integer = AudioCL.Count
        Dim occ As Integer = If(dgvAudio.CurrentCellAddress.X >= 0, dgvAudio.CurrentCellAddress.X, 2) ' -1 ,-1 = Nothing CurrentRow
        If rC <= 0 Then AudioCL.Capacity = fC + 4
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
        If profileSelection.Show <> DialogResult.OK OrElse profileSelection.Items.Count < 1 Then
            PopulateCache(3000000)
            Exit Sub
        End If

        SWt.Restart()

        RemoveHandlersDGV()
        If rC <= 0 Then AudioSBL.RemoveSort()
        AudioSBL.RaiseListChangedEvents = False
        StatusText("Adding Files...")
        Dim ap As AudioProfile = ObjectHelp.GetCopy(profileSelection.SelectedValue)
        ap.SourceSamplingRate = 0
        If TypeOf ap Is GUIAudioProfile Then
            ap.Name = ap.DefaultName
            If DirectCast(ap, GUIAudioProfile).Params.Codec <> AudioCodec.DTS Then ap.ExtractDTSCore = False
        End If
        Dim apTS(fC - 1) As AudioProfile
        Dim dummyStream As New AudioStream
        Dim autoStream As Boolean
        If Not sortFT.IsCompleted Then sortFT.Wait()
        'If PopulateTask IsNot Nothing AndAlso Not PopulateTask.IsCompleted AndAlso PopulateRSW.IsRunning Then PopulateTask.Wait()

        SWt.Stop()
        Dim ewt = SWt.ElapsedTicks / 10000
        SWt.Restart()

        'Dim clth = AudioCL.AsThreadSafe

        Parallel.For(0, fC,
                         Sub(i As Integer)
                             Dim apL = ObjectHelp.GetCopy(ap)
                             apL.File = Files(i)
                             Dim fExt As String = apL.File.Ext
                             If FileTypes.VideoAudio.Contains(fExt) Then
                                 apL.Streams = MediaInfo.GetAudioStreams(apL.File)

                                 If apL.Streams.Count > 0 Then
                                     apL.Stream = apL.Streams(0)
                                 Else
                                     If Not LogHeader Then
                                         SyncLock SLock
                                             If Not LogHeader Then
                                                 LogHeader = True
                                                 If Log.IsEmpty Then Log.WriteEnvironment()
                                                 Log.WriteHeader("Audio Converter")
                                             End If
                                         End SyncLock
                                     End If

                                     Log.WriteLine("-->Audio stream not found, skipping: " & BR & apL.File)
                                     Return
                                 End If

                                 If Not autoStream AndAlso apL.Stream IsNot Nothing AndAlso apL.Streams.Count > 1 Then
                                     SyncLock dummyStream
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

                             'clth.Add(ObjectHelp.GetCopy(apL))

                         End Sub)

        SWt.Stop()
        Dim ept = SWt.ElapsedTicks / 10000
        SWt.Restart()
        'AudioCL.AddRange(apTS)
        'AudioSBL.InnerListChanged()

        'Dim itr As Integer = 1
        'For Each ap In AudioCL
        '    If ap IsNot Nothing Then
        '        AudioVCL.Add(New ViewAudioprofile(itr, ap.Name, ap.DisplayName, ap.File))
        '        itr += 1
        '    End If
        'Next
        'AudioVSBL.InnerListChanged()
        'AudioVSBL.ResetBindings()

        AudioSBL.AddRange(apTS.Where(Function(itm As AudioProfile) itm IsNot Nothing))
        AudioSBL.RaiseListChangedEvents = True
        AudioSBL.ResetBindings()
        Dim nrC = AudioCL.Count

        If nrC > 0 Then
            'dgvAudio.Sort(dgvAudio.Columns(3), ComponentModel.ListSortDirection.Ascending)
            'AudioSBL.ApplySort("File", System.ComponentModel.ListSortDirection.Ascending)
            dgvAudio.CurrentCell = dgvAudio.Rows.Item(nrC - 1).Cells(occ)
            dgvAudio.Rows.Item(If(rC > 0, rC - 1, 0)).Selected = True
            'IndexHeaderRows()
            PopulateIter = 1
            dgvAudio.Select()
            If nrC > 300 Then dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            AutoSizeColumns()
            PopulateCache(3000000)
        End If

        If rC <= 0 Then
            dgvAudio.Columns(0).HeaderCell.SortGlyphDirection = SortOrder.None
            dgvAudio.Columns(1).HeaderCell.SortGlyphDirection = SortOrder.None
            dgvAudio.Columns(2).HeaderCell.SortGlyphDirection = SortOrder.None
            dgvAudio.Columns(3).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        Else
            For Each col As DataGridViewColumn In dgvAudio.Columns
                col.HeaderCell.SortGlyphDirection = SortOrder.None
            Next
        End If


        SWt.Stop()
        laThreads.Text = $"{ewt:F3}|{ept:F3}|{SWt.ElapsedTicks / 10000:F3}"


        UpdateControls(SelRowsCount:=If(nrC > 1, 2, 1))

        Task.Run(Sub()
                     Dim dc = FindDuplicates(False)
                     If dc <= 0 Then Return
                     Console.Beep(260, 150)
                     For n = 1 To 12
                         If PopulateTaskS > -1 AndAlso AudioConverterMode AndAlso Not Me.IsDisposed Then
                             BeginInvoke(Sub()
                                             laAC.ForeColor = Color.Red
                                             laAC.Text = dc & " duplicated files found !!!"
                                             laAC.Invalidate()
                                         End Sub)
                             Thread.Sleep(135)
                             BeginInvoke(Sub() laAC.ForeColor = Color.DarkRed)
                             Thread.Sleep(135)
                         End If
                     Next n
                     BeginInvoke(Sub()
                                     laAC.ForeColor = Color.Black
                                     'UpdateControls()
                                 End Sub)
                 End Sub)

        If StatUpdateTask Is Nothing OrElse StatUpdateTask.IsCompleted Then      'debug
            StatUpdateTask = Task.Factory.StartNew(StatUpdateTaskA, TaskCreationOptions.LongRunning)
            StatUpdateTask.ContinueWith(Sub()
                                            If StatUpdateTask.Exception IsNot Nothing Then
                                                Log.Write("Stat Task Exception:", $"{StatUpdateTask.Status} Inner: {StatUpdateTask.Exception.InnerException?.ToString} |Except: {StatUpdateTask.Exception} |InnType:{StatUpdateTask.Exception.InnerException.GetType()} |HRes:{StatUpdateTask.Exception.HResult}")
                                                Console.Beep(2250, 100)
                                            End If
                                        End Sub)
        End If

        AddHandlersDGV()
    End Sub

    Private Sub PopulateCache(Optional Delay As Integer = -1)
        Select Case PopulateIter
            Case Is > 900
                Exit Sub
            Case Is >= 200
                PopulateTaskS = -1
                PopulateIter += 1
                Exit Sub
        End Select

        Dim pTaskA As Action(Of Integer) = Sub(Del)
                                               SyncLock PopulateWSW
                                                   If PopulateWSW.IsRunning OrElse PopulateRSW.IsRunning Then Return
                                                   PopulateWSW.Restart()
                                                   PopulateTaskS = 1
                                               End SyncLock

                                               Do Until PopulateWSW.ElapsedTicks > Del
                                                   If PopulateTaskS <= 0 OrElse Not AudioConverterMode OrElse Me.IsDisposed Then
                                                       PopulateIter += 1
                                                       PopulateWSW.Stop()
                                                       Return
                                                   End If
                                                   Thread.Sleep(120)
                                               Loop
                                               PopulateRSW.Restart()
                                               PopulateWSW.Stop()

                                               If PopulateTaskS > 0 AndAlso PopulateIter < 200 AndAlso AudioConverterMode AndAlso Not Me.IsDisposed Then
                                                   Try
                                                       Dim arP As AudioProfile() = AudioCL.ToArray

                                                       If PopulateTaskS > 0 AndAlso PopulateIter < 200 AndAlso AudioConverterMode Then

                                                           If arP.Length <= 20 Then PopulateIter += 20

                                                           Dim pl = Parallel.For(0, arP.Length, New ParallelOptions With {.MaxDegreeOfParallelism = CInt(CPUs / 2)},
                                                                                 Sub(i, pls)
                                                                                     If PopulateTaskS <= 0 OrElse Not AudioConverterMode Then
                                                                                         pls.Stop()
                                                                                         Return
                                                                                     Else
                                                                                         MediaInfo.SetMediaInfoCache(arP(i).File)
                                                                                         'arP(i).SetDisplayNameCache()
                                                                                     End If
                                                                                 End Sub)

                                                           If pl.IsCompleted Then
                                                               PopulateIter += 20
                                                               If PopulateRSW.ElapsedTicks < 30000000 Then PopulateIter += 60
                                                           End If

                                                       End If
                                                   Catch ex As Exception
                                                       PopulateRSW.Stop()
                                                       Task.Run(Sub() Console.Beep(4000, 200))
                                                       Log.Write($"{Delay}Del PopulateTask Catch", $"{PopulateTaskS}tS{PopulateWSW.ElapsedTicks / 10000}msW {PopulateRSW?.ElapsedTicks / 10000}msR|iter {PopulateIter} {ex} {ex.InnerException?.ToString} | {PopulateTask?.Status.ToString} | {PopulateTask?.Exception?.ToString} {PopulateTaskW?.Status.ToString}")
                                                   End Try
                                               End If
                                               PopulateRSW.Stop()
                                           End Sub

        If PopulateTask Is Nothing OrElse PopulateTask.IsCompleted Then
            PopulateTask = Task.Factory.StartNew(Sub() pTaskA(If(Delay = -1, 7000000, Delay)), TaskCreationOptions.LongRunning)
        ElseIf PopulateTaskW Is Nothing OrElse PopulateTaskW.IsCompleted Then
            PopulateTaskW = PopulateTask.ContinueWith(Sub()
                                                          If PopulateTaskS >= 0 AndAlso PopulateTask.IsCompleted Then
                                                              PopulateTask = Task.Factory.StartNew(Sub() pTaskA(Delay + 2000000), TaskCreationOptions.LongRunning)
                                                          End If
                                                      End Sub)
        End If
    End Sub

    Private Sub bnAudioAdd_Click(sender As Object, e As EventArgs) Handles bnAudioAdd.Click
        Dim ftav = FileTypes.Audio.Union(FileTypes.VideoAudio).ToArray
        LastKeyDown = -1
        SelectChangeES = False
        ButtonAltImage(False)
        Dim avf As String()
        Using td As New TaskDialog(Of String)
            td.AddCommand("Add files", "files")
            td.AddCommand("Add folder", "folder")
            Select Case td.Show
                Case "files"
                    Using dialog As New OpenFileDialog With {.Multiselect = True}
                        dialog.SetFilter(ftav)
                        If dialog.ShowDialog = DialogResult.OK Then
                            avf = dialog.FileNames.Where(Function(file) ftav.Contains(file.Ext)).ToArray
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
                            ''Try - Todo: add Try Catch IO getfiles
                            avf = Directory.GetFiles(dialog.SelectedPath, "*.*", opt).Where(Function(file)
                                                                                                Dim fExt As String = file.Ext
                                                                                                Return ftav.Contains(fExt)
                                                                                            End Function).ToArray
                        End If
                    End Using
            End Select
        End Using
        If avf?.Length > 0 Then ProcessInputAudioFiles(avf)
    End Sub

    Private Sub dgvAudio_DragEnter(sender As Object, e As DragEventArgs) Handles dgvAudio.DragEnter
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            If files.Any(Function(item) FileTypes.Audio.Union(FileTypes.VideoAudio).Contains(item.Ext)) Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        End If
    End Sub

    Private Sub dgvAudio_DragDrop(sender As Object, e As DragEventArgs) Handles dgvAudio.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            Dim avf = files.Where(Function(file) FileTypes.Audio.Union(FileTypes.VideoAudio).Contains(file.Ext)).ToArray
            If avf?.Length > 0 Then ProcessInputAudioFiles(avf)
        End If
    End Sub

    Private Sub bnAudioEdit_Click(sender As Object, e As EventArgs) Handles bnAudioEdit.Click
        Dim rC As Integer = AudioCL.Count
        Dim srC As Integer = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim crI As Integer = dgvAudio.CurrentCellAddress.Y
        Dim ccI As Integer = dgvAudio.CurrentCellAddress.X
        Dim sr0idx As Integer

        If dgvAudio.Rows.GetRowState(crI) >= &B1100000 Then
            sr0idx = crI
        Else
            Dim prevS As Integer
            prevS = dgvAudio.Rows.GetPreviousRow(crI, DataGridViewElementStates.Selected Or DataGridViewElementStates.Displayed)
            sr0idx = If(prevS > -1, prevS, dgvAudio.Rows.GetNextRow(crI, DataGridViewElementStates.Selected Or DataGridViewElementStates.Displayed))

            If sr0idx < 0 Then
                prevS = dgvAudio.Rows.GetPreviousRow(crI, DataGridViewElementStates.Selected)
                sr0idx = If(prevS > -1, prevS, dgvAudio.Rows.GetNextRow(crI, DataGridViewElementStates.Selected))
            End If
        End If

        If sr0idx < 0 Then Return
        Dim ap As AudioProfile = AudioSBL(sr0idx)
        Dim OldLang As Language = ap.Language
        dgvAudio.FirstDisplayedScrollingRowIndex = sr0idx
        StatusText("Editing: " & (sr0idx + 1).ToInvString & "  |  Sel: " & srC.ToInvString & " / Tot: " & rC.ToInvString)

        If ap.EditAudioConv() = DialogResult.OK Then
            PopulateTaskS = -1
            RemoveHandlersDGV(True)
            StatusText("Applying settings...")

            If TypeOf ap Is GUIAudioProfile Then
                ap.Name = ap.DefaultName
            End If

            AudioSBL.ResetItem(sr0idx)

            If srC > 1 Then
                AudioSBL.RaiseListChangedEvents = False
                Dim apNoLangChange As Boolean = ap.Language.Equals(OldLang)
                Dim apDelayNonZero As Boolean = ap.Delay <> 0
                Dim srCache(rC - 1) As Boolean
                srCache(sr0idx) = True
                Dim arr As AudioProfile() = AudioSBL.ToArray
                Parallel.For(0, rC, Sub(r)                      ' 
                                        If dgvAudio.Rows.GetRowState(r) >= &B1100000 AndAlso r <> sr0idx Then
                                            Dim ap0 As AudioProfile = ObjectHelp.GetCopy(ap)
                                            Dim apn As AudioProfile = AudioSBL.Item(r)
                                            ap0.File = apn.File
                                            If apNoLangChange Then ap0.Language = apn.Language 'needed?
                                            ap0.StreamName = apn.StreamName
                                            ap0.Stream = apn.Stream
                                            ap0.Streams = apn.Streams
                                            If apDelayNonZero Then ap0.Delay = apn.Delay Else ap0.Delay = 0 'needed?
                                            ap0.SourceSamplingRate = apn.SourceSamplingRate
                                            'ap0.RowIdx = r + 1
                                            srCache(r) = True
                                            arr(r) = ap0
                                        End If
                                    End Sub)
                StatusText("Restoring selection...")
                AudioSBL.Clear()
                AudioSBL.AddRange(arr)
                AudioSBL.RaiseListChangedEvents = True
                AudioSBL.ResetBindings()
                crI = If(dgvAudio.CurrentCellAddress.Y >= 0, dgvAudio.CurrentCellAddress.Y, AudioCL.Count - 1)
                dgvAudio.CurrentCell = dgvAudio.Rows.Item(crI).Cells.Item(ccI)
                dgvAudio.CurrentRow.Selected = False
                For nRow = 0 To rC - 1
                    If srCache(nRow) = True Then
                        dgvAudio.Rows.Item(nRow).Selected = True
                    End If
                Next
            End If

            AutoSizeColumns()
            PopulateCache()
            AddHandlersDGV()
        End If

        UpdateControls()
    End Sub

    Private Sub ButtonAltImage(SetAltImg As Boolean)
        If SetAltImg Then
            If ButtonAltImageT IsNot Nothing AndAlso Not ButtonAltImageT.IsCompleted Then Exit Sub
            ButtonAltImageT = Task.Run(Sub()
                                           Thread.Sleep(45)
                                           SyncLock bnAudioRemove
                                               If Not ButtonAltImgDone AndAlso My.Computer.Keyboard.ShiftKeyDown AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso Not My.Computer.Keyboard.AltKeyDown Then
                                                   ButtonAltImgDone = True
                                                   BeginInvoke(Sub()
                                                                   bnAudioRemove.Text = "From Disk"
                                                                   bnAudioRemove.Image = ImageHelp.GetSymbolImage(Symbol.Delete)
                                                               End Sub)
                                               End If
                                           End SyncLock
                                       End Sub)
        Else
            If ButtonAltImgDone Then
                ButtonAltImgDone = False
                BeginInvoke(Sub()
                                bnAudioRemove.Text = "&Remove   "
                                bnAudioRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
                            End Sub)
            End If
        End If
    End Sub


    Private Sub AudioConverterForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyData
            Case Keys.ShiftKey Or Keys.Shift Or Keys.Control, Keys.ControlKey Or Keys.Shift Or Keys.Control
                If Not ButtonAltImgDone AndAlso Not e.Alt Then
                    ButtonAltImage(True)
                End If
            Case Keys.Cancel Or Keys.Control
                Me.Close()
                Dispose(True)
                g.MainForm.Close()
                g.MainForm.Dispose()
                Application.Exit()
        End Select
    End Sub

    Private Sub AudioConverterForm_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.F3 Then
            If LastKeyDown = 3 Then Task.Run(Sub()
                                                 'If Not IndexTask?.IsCompleted Then IndexTask.Wait()
                                                 UpdateControls(SelRowsCount:=1)
                                                 SelectChangeES = False
                                                 LastKeyDown = -1
                                             End Sub)
        Else
            Select Case e.KeyData
                Case Keys.ShiftKey Or Keys.Control, Keys.ControlKey Or Keys.Shift, Keys.ControlKey Or Keys.Shift Or Keys.Alt, Keys.ShiftKey Or Keys.Control Or Keys.Alt
                    ButtonAltImage(False)
            End Select
        End If
    End Sub

    Private Sub dgvAudio_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvAudio.KeyDown
        If e.KeyCode = Keys.F3 Then
            If LastKeyDown = 3 Then
                e.Handled = True
            ElseIf dgvAudio.CurrentCellAddress.X > 0 AndAlso AudioCL.Count > 0 Then
                LastKeyDown = 3
                PopulateTaskS = -1
                SelectChangeES = True
                If AudioCL.Count > 100 Then StatusText("Sorting...")
            End If
        Else
            Select Case e.KeyData
                Case Keys.Delete

                    IndexSWatch.Restart()

                    PopulateTaskS = 0
                    DeleteRows()
                    e.Handled = True

                    IndexSWatch.Stop()

                Case Keys.Delete Or Keys.Shift Or Keys.Control
                    DeleteFiles()
                    e.Handled = True
            End Select
        End If
    End Sub

    Private Sub bnAudioRemove_Click(sender As Object, e As EventArgs) Handles bnAudioRemove.Click
        If My.Computer.Keyboard.ShiftKeyDown AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso Not My.Computer.Keyboard.AltKeyDown Then
            DeleteFiles()
        Else

            IndexSWatch.Restart()

            PopulateTaskS = 0
            DeleteRows()

            IndexSWatch.Stop()

        End If
    End Sub

    Private Sub DeleteFiles()
        PopulateTaskS = -1
        Dim FS As MyServices.FileSystemProxy = My.Computer.FileSystem
        Dim path As String
        Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim fileListDup As New CircularList(Of String)(srC)
        Dim dirListDup As New CircularList(Of String)(srC)
        Dim rowIdxL As New List(Of Integer)(srC)
        Dim ask As Boolean
        Dim fokC As Integer
        Dim dokC As Integer

        For r = 0 To AudioCL.Count - 1
            If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                path = AudioSBL.Item(r).File
                If FS.FileExists(path) Then
                    fileListDup.Add(path)
                    dirListDup.Add(path.Dir)
                    rowIdxL.Add(r)
                End If
            End If
        Next r

        If fileListDup.Count = 0 Then
            ButtonAltImage(False)
            Exit Sub
        End If

        Dim fileList = fileListDup.Union(fileListDup).ToArray
        fileList.Sort()
        Dim dirList = dirListDup.Union(dirListDup).ToArray

        If (dgvAudio.Rows.GetRowState(dgvAudio.SelectedRows.Item(0).Index) And DataGridViewElementStates.Displayed) = 0 Then
            dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.SelectedRows.Item(0).Index
        End If

        If Msg("Delete files from disk" & BR & fileList.Length & " files found in " & dirList.Length & " directories", Nothing, MsgIcon.Question, TaskDialogButtons.OkCancel, DialogResult.Cancel) <> TaskDialogButtons.Ok Then
            ButtonAltImage(False)
            Exit Sub
        End If

        If Not My.Computer.Keyboard.ShiftKeyDown Then Exit Sub ' FailSafe

        ask = MsgQuestion("Ask for each file to delete", TaskDialogButtons.YesNo) = DialogResult.Yes

        StatusText("Deleting from disk...")
        For f = 0 To fileList.Length - 1
            path = fileList(f)

            If ask Then
                Select Case MsgQuestion("Delete File ?" & BR & path, TaskDialogButtons.YesNoCancel)
                    Case DialogResult.No
                        Continue For
                    Case DialogResult.Cancel
                        ButtonAltImage(False)
                        UpdateControls()
                        Exit Sub
                End Select
            End If

            Try
                FS.DeleteFile(path, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)
                fokC += 1
            Catch ex As Exception
                Log.Write("Delete file exception", "Delete error File: " & path & BR & ex.ToString)
            End Try

            Try
                If FS.GetFiles(path.Dir, FileIO.SearchOption.SearchTopLevelOnly).Count = 0 AndAlso MsgQuestion("Delete empty Directory ?" & BR & path.Dir, TaskDialogButtons.YesNo) = DialogResult.Yes Then
                    FS.DeleteDirectory(path.Dir, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)
                    dokC = +1
                End If
            Catch ex As Exception
                Log.Write("Delete dir exception", "Delete error Dir: " & path.Dir & BR & ex.ToString)
            End Try

            Try 'UnTested!!! to do: Add don't ask for all or cancel for all
                If path.Length > 3 AndAlso FS.GetFiles(FS.GetParentPath(path.Dir), FileIO.SearchOption.SearchTopLevelOnly).Count = 0 AndAlso MsgQuestion("Delete empty Directory ?" & BR & FS.GetParentPath(path.Dir), TaskDialogButtons.YesNo) = DialogResult.Yes Then
                    FS.DeleteDirectory(FS.GetParentPath(path.Dir), FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)
                    dokC = +1
                End If
            Catch ex As Exception
                Log.Write("Delete dir exception", "Delete error Dir: " & FS.GetParentPath(path.Dir) & BR & ex.ToString)
            End Try
        Next f

        If fokC > 0 Then MsgInfo(fokC & " files out of " & fileList.Length & " , " & If(dokC > 0, dokC & " directories ", "") & "deleted to recycle bin")
        If dgvAudio.RowHeadersWidth < 80 Then dgvAudio.RowHeadersWidth = 80

        Dim ri As Integer
        For r = 0 To rowIdxL.Count - 1
            ri = rowIdxL.Item(r)

            If FS.FileExists(AudioSBL.Item(ri).File) Then
                dgvAudio.Rows.Item(ri).HeaderCell.Value = "DelError"
            Else
                dgvAudio.Rows.Item(ri).HeaderCell.Value = "Deleted"
            End If
        Next

        PopulateCache(1000000)
        Task.Delay(1500).Wait()
        If MsgQuestion("Remove from Grid View?", TaskDialogButtons.YesNo) = DialogResult.Yes Then
            PopulateTaskS = 0
            DeleteRows()
        End If
        ButtonAltImage(False)
        UpdateControls()
    End Sub

    Private Sub DeleteRows()
        Dim srC As Integer = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim rC As Integer = AudioCL.Count
        If srC <= 0 Then Exit Sub
        If rC = srC Then
            PopulateTaskS = -1
            AudioSBL.Clear()
            AudioCL.Clear()
            AudioSBL.InnerListChanged()
            AudioSBL.ResetBindings()
            dgvAudio.Rows.Clear()
            UpdateControls()
            GC.Collect()
            Exit Sub
        End If

        If srC > 100 Then StatusText("Removing...")
        SelectChangeES = True
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        Dim crI As Integer = -1

        If srC < 250 Then
            'i = rC - srC
            For r = rC - 1 To 0 Step -1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    AudioSBL.RemoveAt(r)
                    'Else
                    'AudioSBL.Item(r).RowIdx = i
                    ' i -= 1
                End If
            Next r

        Else
            RemoveHandler dgvAudio.Scroll, AddressOf dgvAudio_Scroll
            AudioSBL.RaiseListChangedEvents = False
            Dim ccI As Integer = dgvAudio.CurrentCellAddress.X
            Dim dArr(rC - srC - 1) As AudioProfile
            Dim i As Integer

            For r = 0 To rC - 1
                If dgvAudio.Rows.GetRowState(r) < &B1100000 Then
                    dArr(i) = AudioSBL.Item(r)
                    'dArr(i).RowIdx = i + 1
                    i += 1
                Else
                    crI = r
                End If
            Next r

            AudioSBL.Clear()
            AudioSBL.AddRange(dArr)
            AudioSBL.RaiseListChangedEvents = True
            AudioSBL.ResetBindings()

            rC = AudioCL.Count
            crI = If(crI - srC + 1 < rC, crI - srC + 1, rC - 1)
            dgvAudio.CurrentCell = dgvAudio.Rows.Item(crI).Cells.Item(ccI)
            AddHandler dgvAudio.Scroll, AddressOf dgvAudio_Scroll
        End If

        If crI < 0 Then
            rC -= srC
            crI = dgvAudio.CurrentCellAddress.Y
            If crI = rC - 1 Then
                dgvAudio.Rows.Item(crI).Selected = True
            End If
        End If

        If rC < 100 Then AutoSizeColumns()
        UpdateControls(1, crI)
        PopulateCache(5000000)
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        SelectChangeES = False
    End Sub

    Private Sub bnAudioUp_Click(sender As Object, e As EventArgs) Handles bnAudioUp.Click
        PopulateTaskS = 0
        SelectChangeES = True
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        'dgvAudio.MoveSelectionUp
        Dim cc As Point = dgvAudio.CurrentCellAddress
        Dim crI As Integer = cc.Y
        ' If Not IndexTask.IsCompleted Then IndexTask.Wait()

        If crI > 0 Then
            Dim current As AudioProfile = AudioSBL(crI)
            AudioSBL.RemoveAt(crI)
            crI -= 1
            AudioSBL.Insert(crI, current)
            '   IndexHeaderRows()
            dgvAudio.CurrentCell = dgvAudio.Rows.Item(crI).Cells(cc.X)
        End If

        PopulateCache(5000000)
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        SelectChangeES = False
        UpdateControls(1, crI)
    End Sub
    Private Sub bnAudioDown_Click(sender As Object, e As EventArgs) Handles bnAudioDown.Click
        PopulateTaskS = 0
        SelectChangeES = True
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        Dim cc As Point = dgvAudio.CurrentCellAddress
        Dim crI As Integer = cc.Y
        'If Not IndexTask.IsCompleted Then IndexTask.Wait()

        If crI < AudioCL.Count - 1 Then
            Dim current As AudioProfile = AudioSBL(crI)
            AudioSBL.RemoveAt(crI)
            crI += 1
            AudioSBL.Insert(crI, current)
            '  IndexHeaderRows()
            dgvAudio.CurrentCell = dgvAudio.Rows.Item(crI).Cells(cc.X)
        End If

        PopulateCache(5000000)
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        SelectChangeES = False
        UpdateControls(1, crI)
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
        If numThreads.Visible = False Then
            laThreads.Text = "Threads :"
            numThreads.Value = If(MaxThreads > 0, MaxThreads, s.ParallelProcsNum)
            numThreads.Visible = True
        Else
            numThreads.Visible = False
        End If
    End Sub

    Private Sub bnSort_Click(sender As Object, e As EventArgs) Handles bnSort.Click
        If dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None Then
            dgvAudio.Columns(0).FillWeight = 10
            dgvAudio.Columns(1).FillWeight = 100
            dgvAudio.Columns(2).FillWeight = 400
            dgvAudio.Columns(3).FillWeight = 500
            dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ElseIf dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill Then
            dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        End If
        'IndexHeaderRows()
        AutoSizeColumns(My.Computer.Keyboard.ShiftKeyDown)
        UpdateControls()
    End Sub

    Private Sub laAC_DoubleClick(sender As Object, e As EventArgs) Handles laAC.DoubleClick
        'debug
        SyncLock SLock
            PopulateTaskS = -1
            PopulateIter = 999
            StatusText("Refreshing...")
            ButtonAltImgDone = True
            ButtonAltImage(False)
            RemoveHandlersDGV(True)
            Visible = True
            'WindowState = FormWindowState.Normal
            BringToFront()
            Activate()
            dgvAudio.Select()
            LastKeyDown = -1
            SelectChangeES = False
            Dim occ As Point = dgvAudio.CurrentCellAddress
            Dim ocF As String = If(AudioCL.Count > 0 AndAlso occ.Y >= 0, AudioSBL.Item(occ.Y).File, Nothing)
            AudioSBL.RemoveSort()
            AudioSBL.RaiseListChangedEvents = False
            dgvAudio.DataSource = Nothing
            dgvAudio.DataBindings.Clear()
            dgvAudio.Rows.Clear()
            Console.Beep(700, 60)
            Text = "AudioConverter"
            PopulateTask?.Wait()
            Dim uAR = AudioCL.Union(AudioCL, New AudioProfileComparer).ToArray
            'For i = 0 To uAR.Length - 1
            '    uAR(i).RowIdx = i + 1
            'Next i
            MediaInfo.ClearCache()
            'AudioProfile.DisplayCache.Clear()
            AudioSBL.Clear()
            AudioCL.Clear()
            AudioCL.Capacity = uAR.Length + 4
            AudioCL.AddRange(uAR)
            AudioSBL.InnerListChanged()
            Task.Delay(15).Wait()

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
            GC.Collect(2, GCCollectionMode.Forced, True, True)
            GC.WaitForPendingFinalizers()
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
            GC.Collect(2, GCCollectionMode.Forced, True, True)
            GC.WaitForPendingFinalizers()

            AudioSBL.RaiseListChangedEvents = True
            dgvAudio.DataSource = AudioSBL
            AudioSBL.ResetBindings()
            AutoSizeColumns()

            If ocF IsNot Nothing Then  ' AndAlso uAR.Contains(ocF)
                dgvAudio.CurrentCell = dgvAudio.Rows.Item(Array.FindIndex(uAR, Function(itm) itm.File.Equals(ocF))).Cells.Item(If(occ.X >= 0, occ.X, 2))
            End If
            'dgvAudio.SelectAll()

            UpdateControls()
            PopulateIter = 2
            PopulateCache(3000000)
            Console.Beep(900, 45)
            dgvAudio.Refresh()
            Refresh()
            AddHandlersDGV()

            If Not StatUpdateTask?.IsCompleted Then StatUpdateTask.Wait(120)
            If StatUpdateTask Is Nothing OrElse StatUpdateTask.IsCompleted Then      'debug
                StatUpdateTask = Task.Factory.StartNew(StatUpdateTaskA, TaskCreationOptions.LongRunning)
                StatUpdateTask.ContinueWith(Sub()
                                                If StatUpdateTask.Exception IsNot Nothing Then
                                                    Console.Beep(2250, 100)
                                                    Log.Write("Stat Task Exception:", $"{StatUpdateTask.Status} Inner: {StatUpdateTask.Exception.InnerException?.ToString} |Except: {StatUpdateTask.Exception} |InnType:{StatUpdateTask.Exception.InnerException.GetType()} |HRes:{StatUpdateTask.Exception.HResult}")
                                                End If
                                            End Sub)
            End If
        End SyncLock
    End Sub

    Private Sub RemoveHandlersDGV(Optional All As Boolean = False)
        If All Then
            ''' Remove all handlers from control  'Dim gt = GetType(MenuButton) '.getruntimefields
            '''Dim f1 As FieldInfo = GetType(DataGridViewEx).GetField("SelectionChanged", BindingFlags.Instance Or BindingFlags.NonPublic)
            Dim gt As Type = GetType(DataGridView)
            'Dim gf As FieldInfo() = gt.GetFields(BindingFlags.Static Or BindingFlags.NonPublic)
            'Dim obj As Object
            Dim pi As PropertyInfo = dgvAudio.GetType().GetProperty("Events", BindingFlags.Instance Or BindingFlags.NonPublic)
            Dim list As EventHandlerList = DirectCast(pi.GetValue(dgvAudio, Nothing), EventHandlerList)
            'For Each fi As FieldInfo In gf
            '    obj = fi.GetValue(dgvAudio)
            '    list.RemoveHandler(obj, list(obj))
            'Next

            Dim f1 As FieldInfo = gt.GetField("EVENT_DATAGRIDVIEWSELECTIONCHANGED", BindingFlags.Static Or BindingFlags.NonPublic)
            Dim f2 As FieldInfo = gt.GetField("EVENT_DATAGRIDVIEWSCROLL", BindingFlags.Static Or BindingFlags.NonPublic)
            'Dim f3 As FieldInfo = gt.GetField("EVENT_DATAGRIDVIEWSORTED", BindingFlags.Static Or BindingFlags.NonPublic)
            Dim obj1 As Object = f1.GetValue(dgvAudio)
            Dim obj2 As Object = f2.GetValue(dgvAudio)
            'Dim obj3 As Object = f3.GetValue(dgvAudio)
            list.RemoveHandler(obj1, list(obj1))
            list.RemoveHandler(obj2, list(obj2))
            'list.RemoveHandler(obj3, list(obj3))
        End If
        RemoveHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        RemoveHandler dgvAudio.Scroll, AddressOf dgvAudio_Scroll
    End Sub

    Private Sub AddHandlersDGV()
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        AddHandler dgvAudio.Scroll, AddressOf dgvAudio_Scroll
    End Sub

End Class

Public Class AudioProfileComparer 'ToDo Add more than 1 stream from file
    Implements IEqualityComparer(Of AudioProfile)
    Public Function Equals1(ByVal x As AudioProfile, ByVal y As AudioProfile) As Boolean Implements IEqualityComparer(Of AudioProfile).Equals
        ' Check whether the compared objects reference the same data.
        If x Is y Then Return True
        'Check whether any of the compared objects is null.
        'If x Is Nothing OrElse y Is Nothing Then Return False ' True if both Null or False???
        ' Check whether the products' properties are equal.        'Return (x.File = y.File)
        Return String.Equals(x.File, y.File) ' x.File.Equals(y.File) ' True if both Null or False???
    End Function
    Public Function GetHashCode1(ByVal audioprofile As AudioProfile) As Integer Implements IEqualityComparer(Of AudioProfile).GetHashCode
        ' Check whether the object is null.
        If audioprofile Is Nothing Then Return 0
        ' Get hash code for the Name field if it is not null.
        Dim hashAudioProfileFile = If(audioprofile.File Is Nothing, 0, audioprofile.File.GetHashCode())
        ' Calculate the hash code for the product.
        Return hashAudioProfileFile
    End Function
End Class

'Public Class ViewAudioprofile
'    'Private _FKey As Integer
'    'Private AudioVCL As New CircularList(Of ViewAudioprofile)(16)
'    'Private AudioVSBL As New SortableBindingList(Of ViewAudioprofile)(AudioVCL) With {.CheckConsistency = False, .SortOnChange = False, .RaiseListChangedEvents = True, .AllowNew = False}
'    Public Shared Cache As New Dictionary(Of Integer, ViewAudioprofile)(64)
'    'Public Property FKey As Integer
'    '    Get
'    '        _FKey = DisplayName.GetHashCode
'    '        Return _FKey
'    '    End Get
'    '    Set
'    '        _FKey = Value
'    '    End Set
'    'End Property
'    Public Property RowIdx As Integer
'    Public Property Name As String
'    Public Property DisplayName As String
'    Public Property File As String
'    Public Sub New(rowIdx As Integer, name As String, displayName As String, file As String)
'        Me.RowIdx = rowIdx
'        Me.Name = name
'        Me.DisplayName = displayName
'        Me.File = file
'    End Sub
'End Class

