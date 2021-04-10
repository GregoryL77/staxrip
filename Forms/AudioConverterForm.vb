Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports Force.DeepCloner
Imports KGySoft.Collections
Imports KGySoft.ComponentModel
Imports KGySoft.CoreLibraries
Imports Microsoft.Toolkit.HighPerformance
Imports Microsoft.VisualBasic
Imports JM.LinqFaster
Imports JM.LinqFaster.Parallel
Imports JM.LinqFaster.SIMD
Imports JM.LinqFaster.SIMD.Parallel
Imports LinqFasterer
Imports StaxRip.UI

Public Class AudioConverterForm
    Inherits FormBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        PopulateTaskS = -1
        PopulateIter = 999
        AudioConverterMode = False

        If disposing Then
            AudioSBL?.Dispose()
            dgvAudio?.Dispose()
            If CMS IsNot Nothing Then
                CMS.Items.ClearAndDisplose
                CMS.Dispose()
            End If
            If components IsNot Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents laThreads As Label
    Friend WithEvents numThreads As NumEdit
    Friend WithEvents bnCMD As ButtonEx
    Friend WithEvents bnAdd As ButtonEx
    Friend WithEvents bnAudioMediaInfo As ButtonEx
    Friend WithEvents bnPlay As ButtonEx
    Friend WithEvents bnEdit As ButtonEx
    Friend WithEvents bnDown As ButtonEx
    Friend WithEvents bnUp As ButtonEx
    Friend WithEvents bnConvert As ButtonEx
    Friend WithEvents bnRemove As ButtonEx
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
        Me.bnEdit = New StaxRip.UI.ButtonEx()
        Me.bnConvert = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnMenuAudio = New StaxRip.UI.ButtonEx()
        Me.laAC = New System.Windows.Forms.Label()
        Me.bnSort = New System.Windows.Forms.Button()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.bnPlay = New StaxRip.UI.ButtonEx()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
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
        Me.laThreads.Location = New System.Drawing.Point(399, 330)
        Me.laThreads.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
        Me.laThreads.Name = "laThreads"
        Me.laThreads.Size = New System.Drawing.Size(157, 15)
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
        'bnEdit
        '
        Me.bnEdit.CausesValidation = False
        Me.bnEdit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnEdit.Location = New System.Drawing.Point(562, 351)
        Me.bnEdit.Size = New System.Drawing.Size(84, 26)
        Me.bnEdit.Text = "    &Edit..."
        Me.TipProvider.SetTipText(Me.bnEdit, "Edit Audio Profile for selection")
        '
        'bnConvert
        '
        Me.bnConvert.CausesValidation = False
        Me.bnConvert.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnConvert.Location = New System.Drawing.Point(238, 351)
        Me.bnConvert.Size = New System.Drawing.Size(88, 26)
        Me.bnConvert.Text = "&Convert..."
        Me.bnConvert.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.bnConvert, "Convert Selection")
        '
        'bnRemove
        '
        Me.bnRemove.CausesValidation = False
        Me.bnRemove.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnRemove.Location = New System.Drawing.Point(144, 351)
        Me.bnRemove.Size = New System.Drawing.Size(88, 26)
        Me.bnRemove.Text = "&Remove "
        Me.bnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.bnRemove, "Hold <Shift+Control> to delete from disk")
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
        Me.laAC.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.bnSort.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bnSort.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bnSort.Location = New System.Drawing.Point(658, 327)
        Me.bnSort.Margin = New System.Windows.Forms.Padding(3, 1, 3, 0)
        Me.bnSort.Name = "bnSort"
        Me.bnSort.Size = New System.Drawing.Size(72, 21)
        Me.bnSort.TabIndex = 49
        Me.bnSort.Text = "&View mode"
        Me.TipProvider.SetTipText(Me.bnSort, "Switches to other auto-size columns mode")
        Me.bnSort.UseVisualStyleBackColor = True
        '
        'bnAdd
        '
        Me.bnAdd.CausesValidation = False
        Me.bnAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAdd.Location = New System.Drawing.Point(50, 351)
        Me.bnAdd.Size = New System.Drawing.Size(88, 26)
        Me.bnAdd.Text = "    &Add..."
        '
        'bnPlay
        '
        Me.bnPlay.CausesValidation = False
        Me.bnPlay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnPlay.Location = New System.Drawing.Point(472, 351)
        Me.bnPlay.Size = New System.Drawing.Size(84, 26)
        Me.bnPlay.Text = "   &Play"
        '
        'bnDown
        '
        Me.bnDown.CausesValidation = False
        Me.bnDown.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnDown.Location = New System.Drawing.Point(402, 351)
        Me.bnDown.Size = New System.Drawing.Size(64, 26)
        Me.bnDown.Text = "&Down"
        Me.bnDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bnUp
        '
        Me.bnUp.CausesValidation = False
        Me.bnUp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnUp.Location = New System.Drawing.Point(332, 351)
        Me.bnUp.Size = New System.Drawing.Size(64, 26)
        Me.bnUp.Text = "  &Up"
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
        Me.tlpMain.Controls.Add(Me.bnAdd, 1, 2)
        Me.tlpMain.Controls.Add(Me.bnRemove, 2, 2)
        Me.tlpMain.Controls.Add(Me.bnConvert, 3, 2)
        Me.tlpMain.Controls.Add(Me.bnUp, 4, 2)
        Me.tlpMain.Controls.Add(Me.bnDown, 5, 2)
        Me.tlpMain.Controls.Add(Me.bnPlay, 6, 2)
        Me.tlpMain.Controls.Add(Me.bnEdit, 7, 2)
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
        Me.dgvAudio.VirtualMode = True
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
    Public Shared AudioConverterMode As Boolean
    Private Shared MaxThreads As Integer
    Private Shared OutPath As String
    Private Shared LastLogPath As String
    Private Shared LogHeader As Boolean
    Private AudioCL As CircularList(Of AudioProfile)
    Private AudioSBL As SortableBindingList(Of AudioProfile)
    Private StatTextSB As New StringBuilder(38)
    Private CurrentType As Type
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
    Private SLock As New Object
    Private SW1 As New Stopwatch
    Private SW2 As New Stopwatch
    Private ReadOnly AudioVideoFExts As New HashSet(Of String)(FileTypes.Audio.ConcatA(FileTypes.VideoAudio), StringComparer.Ordinal)
    Private ReadOnly InvalidPathCh As Char() = {":"c, "\"c, "/"c, "?"c, "^"c, "."c}
    'Private DGVEventCount As Integer
    Private StatUpdateTask As Task
    Private ReadOnly StatUpdateTaskA As Action =
        Sub()
            Do While PopulateIter < 900 AndAlso AudioConverterMode
                Dim t As String = PopulateIter.ToInvariantString & PopulateTask?.Status.ToString & PopulateWSW.ElapsedTicks / SWFreq & "msW" & PopulateTaskS.ToInvariantString & "PTS" & PopulateRSW.ElapsedTicks / SWFreq & "msR|WPT"
                t &= PopulateTaskW?.Status.ToString & SW2.ElapsedTicks / SWFreq & "ms2|Sw1" & SW1.ElapsedTicks / SWFreq & "MC" & MediaInfo.Cache.Count.ToInvariantString '& "VC" & DGVEventCount.ToInvariantString & "SBL:" & StatTextSB.Length.ToInvariantString
                Me.BeginInvoke(Sub() Me.Text = t)
                'If StatTextSB.Length > 38 Then Me.BeginInvoke(Sub() Me.BackColor = Color.HotPink)
                Thread.Sleep(105)
            Loop
            If AudioConverterMode AndAlso Not Me.IsDisposed AndAlso Me.IsHandleCreated Then Me.BeginInvoke(Sub() Me.Text = "AudioConverter")
        End Sub

    Public Sub New()
        MyBase.New()
        'Add/RemoveHandler Application.ThreadException, AddressOf g.OnUnhandledException
        'If Debugger.IsAttached Then Control.CheckForIllegalCrossThreadCalls = False
        AudioCL = New CircularList(Of AudioProfile)(128)
        AudioSBL = New SortableBindingList(Of AudioProfile)(AudioCL) With {.CheckConsistency = False, .SortOnChange = False, .RaiseListChangedEvents = True, .AllowNew = False}
        Icon = g.Icon
        MinimumSize = New Size(MyBase.Font.Height * 22, MyBase.Font.Height * 12)
        RestoreClientSize(53, 29)
        InitializeComponent()

        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        GetType(DataGridViewEx).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, dgvAudio, New Object() {True})
        dgvAudio.DefaultCellStyle.DataSourceNullValue = Nothing
        dgvAudio.DefaultCellStyle.FormatProvider = CultureInfo.InvariantCulture
        dgvAudio.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        dgvAudio.RowTemplate.Height = Font.Height + 4 'or *1.25 ?, AutoResize=20, def=22
        '[InDesigner-InitSub]-dgvAudio.RowHeadersWidth = 24 ' (0)=42 +6 per number char
        dgvAudio.AutoGenerateColumns = False
        dgvAudio.DataSource = AudioSBL

        Dim indexCol As New DataGridViewTextBoxColumn With {.SortMode = DataGridViewColumnSortMode.NotSortable, .HeaderText = "No.", .FillWeight = 10, .MinimumWidth = 28, .ReadOnly = True}
        'indexCol.ValueType = GetType(Integer) ; col.MinimumWidth = col.HeaderText.Length * 8 + 5 <no=32-noTTip> ; 'col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Dim profNameCol As New DataGridViewTextBoxColumn With {.DataPropertyName = "Name", .HeaderText = "Profile", .FillWeight = 100, .MinimumWidth = 60, .ReadOnly = True}
        Dim dispNameCol As New DataGridViewTextBoxColumn With {.DataPropertyName = "DisplayName", .HeaderText = "Track", .FillWeight = 400, .MinimumWidth = 46, .ReadOnly = True}
        Dim pathCol As New DataGridViewTextBoxColumn With {.DataPropertyName = "File", .HeaderText = "Full Path", .FillWeight = 500, .MinimumWidth = 74, .ReadOnly = True}
        dgvAudio.Columns.AddRange({indexCol, profNameCol, dispNameCol, pathCol})
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()

        bnAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnPlay.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)
        bnConvert.Image = ImageHelp.GetSymbolImage(Symbol.MusicInfo)
        bnAudioMediaInfo.Image = ImageHelp.GetSymbolImage(Symbol.Info)
        bnCMD.Image = ImageHelp.GetSymbolImage(Symbol.CommandPrompt)

        For Each bn As ButtonEx In {bnAdd, bnRemove, bnPlay, bnUp, bnDown, bnEdit, bnConvert, bnAudioMediaInfo, bnCMD}
            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad As Padding = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        If MaxThreads = 0 Then MaxThreads = s.ParallelProcsNum
        numThreads.Value = MaxThreads
        CMS = New ContextMenuStripEx(components)
        '<.AddClickAction> bnMenuAudio.ClickAction = Sub() UpdateCMS(Me.bnMenuAudio, New System.ComponentModel.CancelEventArgs With {.Cancel = False})
        bnMenuAudio.ContextMenuStrip = CMS
        bnAdd.Select()
    End Sub
    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        AudioConverterMode = True
        If AudioCL.Count <= 0 Then
            dgvAudio.Columns.Item(3).HeaderCell.SortGlyphDirection = SortOrder.Ascending
            UpdateControls()
        End If
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(2, GCCollectionMode.Forced, True, True)
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        PopulateTaskS = -1
        PopulateIter = 999
        RemoveHandlersDGV()
        AudioCL.Reset()
        AudioSBL.Clear()
        dgvAudio.Columns.Clear()
        MyBase.OnFormClosing(e)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        SaveConverterLog(LastLogPath)
        MyBase.OnFormClosed(e)
    End Sub

    Private Sub UpdateCMS(sender As Object, e As CancelEventArgs) Handles CMS.Opening
        'Dim s1 = "e::\\11TestDEl\bekowe\sdfsdf345dfgsdrhdrthdrtjhw456e44thfthjtgjh\dfgdfh456rrfgh456dfthdrtjdtyykTDHRHesfgrtye568fgtnhTHJFTGJKFT\fghFGH456utjDTGJFGHJFHJdfg456fge457fhjcghjfgjh\dxfgh456fghfghcfgHDRTHSRKSDFGHFJGHJRDGJHDRJDT\djkfbsdgkdbfjEFGDFGSDFSDGedghdfgherd34FGHFTHDFH\Akademia Pana Kleksa - 7 smuteczków.mp3"
        'Dim s2 = "<<GooDForYou/|GooDForYou>"
        'RemoveHandlersDGV(True)
        'tlpMain.SuspendLayout()
        'SuspendLayout()
        'CMS.SuspendLayout()
        ''dgvAudio.DataSource = Nothing
        'AudioSBL.RaiseListChangedEvents = False
        ''dgvAudio.Rows.Clear()
        'Dim itr = 1_000_000
        'Dim r1 As String
        'Dim r2 As String
        'WarmUpCpu()
        'SW2.Restart()
        'For n = 1 To itr
        '    Dim chars = s2.ToCharArray
        '    Array.Reverse(chars)
        '    r1 = New String(chars)
        '    'If ta1.ContainsString(s2) Then r1 += 1 'ContainsF Ordinal is fastest & Contains<NoStrCompOpt> is faster,forLoopEquals Is Fastest of ALL
        '    'For c = 0 To s2.Length - 1
        '    '    If InvalidFileSystemCh.ContainsF(s2(c)) Then '3500ms
        '    '        r1 += 1
        '    '        Exit For
        '    '    End If
        '    'Next c
        '    'Dim s2ca As Char() = s2.ToCharArray 'String.Contains-6100ms'2550ms-ContainsF ; ForLoop 2400ms ; 2xForLoop NoCharArray 2350ms-Fastest NoSimd
        '    'For c = 0 To InvalidFileSystemCh.Length - 1
        '    '    For cc = 0 To s2.Length - 1
        '    '        If s2(cc) = InvalidFileSystemCh(c) Then
        '    '            r1 += 1
        '    '            Exit For
        '    '        End If
        '    '        If r1 > 0 Then Exit For
        '    '    Next cc
        '    'Next c
        'Next n
        'SW2.Stop()
        'Dim tt1 = SW2.ElapsedTicks / SWFreq
        'WarmUpCpu()
        'SW2.Restart()
        'For n = 1 To itr
        '    'Dim invCu16 = InvalidFileSystemCh.SelectF(Function(c) System.Convert.ToUInt16(c))
        '    'For c = 0 To s2.Length - 1
        '    '    Dim chInt As UShort = System.Convert.ToUInt16(s2(c))
        '    '    If invCu16.ContainsS(chInt) Then '1150ms UInt16 is fastest String>Char Simd Op
        '    '        r2 += 1
        '    '        Exit For
        '    '    End If
        '    'Next c
        'Next n
        'SW2.Stop()
        'MsgBox($"{itr:N0}ARev. {tt1}msForLoopRev| {SW2.ElapsedTicks / SWFreq}msREf {r1 = r2 } {r1 = New String(s1.Reverse.ToArray)}")
        'AudioSBL.InnerListChanged()
        'AudioSBL.RaiseListChangedEvents = True
        ''dgvAudio.DataSource = AudioSBL
        'CMS.ResumeLayout(True)
        'AudioSBL.ResetBindings()
        'tlpMain.ResumeLayout(True)
        'ResumeLayout(True)
        ''dgvAudio.Refresh()
        'Refresh()
        'AddHandlersDGV()


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
                                  RemoveHandlersDGV(True)
                                  AudioCL.Reset()
                                  AudioSBL.Clear()
                                  AudioSBL.InnerListChanged()
                                  AudioSBL.ResetBindings()
                                  dgvAudio.Rows.Clear()
                                  UpdateControls()
                                  CurrentType = Nothing
                                  GC.Collect()
                              End Sub, isR).SetImage(Symbol.Clear)
        CMS.Add("Remove duplicates", Sub()
                                         PopulateTaskS = -1
                                         RemoveHandlersDGV(True)
                                         Dim occ As Integer = dgvAudio.CurrentCellAddress.X
                                         Dim ocF As Long = cAP.FileKeyHashValue
                                         Dim uAR = AudioCL.Distinct(New AudioProfileHComparer).ToArray

                                         If AudioCL.Count <= uAR.Length Then
                                             MsgInfo("No duplicates found.")
                                         ElseIf MsgQuestion(Me.AudioCL.Count - uAR.Length & " duplicated file(s) found." & BR & "Remove from Grid View ?") = DialogResult.OK Then
                                             StatusText("Removing duplicated files...")
                                             AudioCL.Reset()
                                             AudioSBL.Clear()
                                             AudioCL.Capacity = uAR.Length
                                             AudioSBL.RaiseListChangedEvents = False
                                             AudioSBL.InnerListChanged()
                                             AudioSBL.RemoveSort()
                                             AudioSBL.AddRange(uAR)
                                             AudioSBL.RaiseListChangedEvents = True
                                             AudioSBL.ResetBindings()
                                             Dim crI As Integer = Array.FindIndex(uAR, Function(ap) ap.FileKeyHashValue = ocF)
                                             If crI >= 0 Then dgvAudio.CurrentCell = dgvAudio.Rows.Item(crI).Cells.Item(occ)
                                             AutoSizeColumns()
                                         End If

                                         PopulateCache(4000000)
                                         UpdateControls()
                                         AddHandlersDGV()
                                     End Sub, isCR, "Removes duplicated files from list").SetImage(Symbol.ShowResults) 'BulletedList)

        CMS.Add("Show Source File", Sub()
                                        dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.CurrentCellAddress.Y
                                        g.SelectFileWithExplorer(cAP.FileValue)
                                    End Sub, isCR AndAlso FileExists(cAP.FileValue), "Open the source file in File Explorer.").SetImage(Symbol.FileExplorer)

        CMS.Add("Show Ouput Folder", Sub() g.SelectFileWithExplorer(OutPath), DirExists(OutPath), "Open output folder in File Explorer.").SetImage(Symbol.FileExplorerApp)

        'cms.Add("Show Output File", Sub() g.SelectFileWithExplorer(OutPath & RelativeSubDirRecursive(cAP.FileValue.Dir, 0) & cAP.GetOutputFile),
        'ccExists AndAlso FileExists(OutPath & RelativeSubDirRecursive(cAP.FileValue.Dir, 0) & cAP.GetOutputFile), "Open converted file in File explerer").SetImage(Symbol.ShowResults)

        CMS.Add("Show LOG", Sub()
                                SaveConverterLog(LastLogPath)
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

        CMS.ResumeLayout(False)
        LastKeyDown = -1
        SelectChangeES = False
        ButtonAltImage(False)
        e.Cancel = False
    End Sub

    Private Sub UpdateControls(Optional SelRowsCount As Integer = -1, Optional CurrentRowI As Integer = -1)
        ' SW1.Restart()
        Dim rC As Integer = AudioCL.Count
        If SelRowsCount = -1 Then SelRowsCount = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        If CurrentRowI = -1 Then CurrentRowI = dgvAudio.CurrentCellAddress.Y
        StatTextSB.Clear()
        If rC > 0 Then StatTextSB.Append("Pos: ").Append((CurrentRowI + 1).ToInvariantString).Append(" Sel: ").Append(SelRowsCount.ToInvariantString).Append(" / Tot: ").Append(rC.ToInvariantString)
        Me.BeginInvoke(Sub()
                           'SW2.Restart()
                           If rC > 0 Then
                               'laAC.Text = "Pos: " & (CurrentRowI + 1).ToInvariantString & " | Sel: " & (SelRowsCount).ToInvariantString & " / Tot: " & (rC).ToInvariantString
                               laAC.Text = StatTextSB.ToString
                               numThreads.Enabled = True  ' Or 1 selected
                           Else
                               laAC.Text = "Please add or drag music files..."
                               numThreads.Enabled = False
                           End If
                           bnRemove.Enabled = SelRowsCount > 0
                           bnConvert.Enabled = SelRowsCount > 0
                           bnUp.Enabled = SelRowsCount = 1 AndAlso CurrentRowI > 0
                           bnDown.Enabled = SelRowsCount = 1 AndAlso CurrentRowI < rC - 1
                           bnPlay.Enabled = CurrentRowI > -1
                           bnEdit.Enabled = SelRowsCount > 0
                           bnAudioMediaInfo.Enabled = CurrentRowI > -1
                           bnCMD.Enabled = CurrentRowI > -1
                           ' SW2.Stop()
                       End Sub)
        '   SW1.Stop()
    End Sub

    Private Sub StatusText(InfoText As String)
        laAC.Text = InfoText
        laAC.Refresh()
    End Sub

    Private Sub FlashingText(InfoText As String, Optional Count As Integer = 10, Optional Delay As Integer = 120)
        For n = 1 To Count
            If PopulateTaskS > -1 AndAlso AudioConverterMode AndAlso Me.IsHandleCreated Then
                BeginInvoke(Sub()
                                laAC.ForeColor = Color.Red
                                laAC.Text = InfoText
                            End Sub)
                Thread.Sleep(Delay)
                If AudioConverterMode Then BeginInvoke(Sub() laAC.ForeColor = Color.DarkRed)
                Thread.Sleep(Delay)
            Else
                Exit For
            End If
        Next n
        If AudioConverterMode AndAlso Not Me.IsDisposed AndAlso Me.IsHandleCreated Then BeginInvoke(Sub() laAC.ForeColor = Color.Black)
    End Sub

    Private Sub AutoSizeColumns(Optional AllCells As Boolean = False)
        If AudioCL.Count > 1000 OrElse (AudioCL.Count > 100 AndAlso MediaInfo.Cache.Count < 100) Then StatusText("Auto Resizing Columns...")
        Dim c0w = Math.Max(28, CInt(14 + 6 * Fix(Math.Log10(AudioCL.Count + 1))))
        If dgvAudio.Columns.Item(0).MinimumWidth <> c0w Then dgvAudio.Columns.Item(0).MinimumWidth = c0w
        If dgvAudio.ColumnHeadersHeight <> 23 Then dgvAudio.ColumnHeadersHeight = 23
        If dgvAudio.RowHeadersWidth <> 24 Then dgvAudio.RowHeadersWidth = 24
        If dgvAudio.AutoSizeColumnsMode <> DataGridViewAutoSizeColumnsMode.Fill Then
            PopulateTaskS = -1
            Dim allc As Boolean = AllCells OrElse AudioCL.Count < 200 OrElse (AudioCL.Count < 2500 AndAlso MediaInfo.Cache.Count > 2000)
            If allc Then Application.DoEvents()
            dgvAudio.AutoResizeColumns(If(allc, DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader, DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader))
        End If
    End Sub

    Private Sub dgvAudio_SelectionChanged(sender As Object, e As EventArgs)
        If SelectChangeES Then Exit Sub
        PopulateTaskS = 0
        'Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Task.Run(Sub()
                     UpdateControls()
                     PopulateCache(4000000)
                 End Sub)
    End Sub

    Private Sub dgvAudio_Scroll(sender As Object, e As ScrollEventArgs)
        'Task.Run(Sub() Log.WriteLine("--->ScrollEvents: " & e.NewValue.ToString & " :newv | oldv : " & e.OldValue.ToString & e.Type.ToString & " :type | scrollOrient : " & e.ScrollOrientation.ToString))
        Select Case e.Type
            Case ScrollEventType.LargeDecrement, ScrollEventType.LargeIncrement, ScrollEventType.ThumbTrack
                PopulateTaskS = 0
                PopulateCache(13000000)
        End Select
    End Sub

    Private Sub dgvAudio_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseClick
        If Not SelectChangeES AndAlso e.RowIndex = -1 AndAlso e.ColumnIndex > 0 AndAlso e.Button = MouseButtons.Left AndAlso AudioCL.Count > 0 Then
            PopulateTaskS = -1
            SelectChangeES = True
            If AudioCL.Count > 1000 OrElse (AudioCL.Count > 100 AndAlso MediaInfo.Cache.Count < 100) Then StatusText("Sorting...")
            SW2.Restart()
        End If
    End Sub

    Private Sub dgvAudio_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseUp
        If SelectChangeES AndAlso e.RowIndex = -1 AndAlso e.ColumnIndex > -1 AndAlso e.Button = MouseButtons.Left AndAlso AudioCL.Count > 0 Then
            SW2.Stop()
            Task.Run(Sub()
                         UpdateControls(SelRowsCount:=1)
                         SelectChangeES = False
                     End Sub)
        End If
    End Sub

    Private Sub dgvAudio_CellValueNeeded(sender As Object, e As DataGridViewCellValueEventArgs) Handles dgvAudio.CellValueNeeded
        '  If e.ColumnIndex = 0 Then 'DGVEventCount += 1
        e.Value = (e.RowIndex + 1).ToInvariantString
    End Sub

    Private Sub dgvAudio_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvAudio.CellFormatting
        'If e.CellStyle.DataSourceNullValue IsNot Nothing OrElse e.CellStyle.FormatProvider IsNot CultureInfo.InvariantCulture Then Console.Beep(3700, 10) 'DGVEventCount += 1
        e.FormattingApplied = True
    End Sub

    'Private Sub dgvAudio_RowUnshared(sender As Object, e As DataGridViewRowEventArgs) Handles dgvAudio.RowUnshared
    '    'laThreads.Text = "UnShI:" & e.Row.Index() + 1
    '    'laThreads.Refresh()
    '    DGVEventCount += 1
    'End Sub
    'Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean ' Really Needed????
    '    Get
    '        If ProcController.BlockActivation Then
    '            ProcController.BlockActivation = False
    '            If ProcController.IsLastActivationLessThan(60) Then Return True
    '        End If
    '        Return MyBase.ShowWithoutActivation
    '    End Get
    'End Property
    'debug
    'Private Sub dgvAudio_Layout(sender As Object, e As LayoutEventArgs) Handles dgvAudio.Layout
    '    Log.Write("Layout Events:", e.AffectedComponent?.ToString & " <-Component|Ctrl-> " & e.AffectedControl?.ToString & " Property: " & e.AffectedProperty?.ToString)
    '    Text = "Layout Events:" & e.AffectedComponent?.ToString & " <-Component|Ctrl-> " & e.AffectedControl?.ToString & " Property: " & e.AffectedProperty?.ToString
    'End Sub
    Private Sub dgvAudio_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAudio.DataError
        Task.Run(Sub()
                     Log.Write("DGV DataErrorException:cr " & e.ColumnIndex & e.RowIndex, e.Exception.ToString & e.ToString & e.Context.ToString)
                     Console.Beep(6000, 30)
                 End Sub)
        e.Cancel = True
        e.ThrowException = False
    End Sub
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
                Catch
                Finally
                    If UseDialog Then
                        If g.FileExists(LogPath & LogName) Then
                            LastLogPath = LogPath & LogName
                        Else
                            MsgWarn("Something went wrong. Locate log file manually or use Main Window's Tools menu. ")
                        End If
                    End If
                End Try
            End If

        ElseIf UseDialog Then
            MsgInfo("Log is empty")
        End If
    End Sub

    Private Function FindDuplicates(SelectedOnly As Boolean, Optional SelRowCount As Integer = 0) As Integer
        Dim cUn As Integer
        Dim cn As Integer

        If SelectedOnly Then
            cn = If(SelRowCount > 0, SelRowCount, dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected))
            If cn < 1 Then Return 0
            Dim uhs As New HashSet(Of Long)(cn)
            For r = 0 To AudioCL.Count - 1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    uhs.Add(AudioSBL.Item(r).FileKeyHashValue)
                End If
            Next r
            cUn = uhs.Count
        Else
            cn = AudioCL.Count
            If cn < 1 Then Return 0
            Dim uhs As New HashSet(Of Long)(cn)
            For r = 0 To AudioCL.Count - 1
                uhs.Add(AudioCL.Item(r).FileKeyHashValue)
            Next r
            cUn = uhs.Count
        End If

        Return cn - cUn
    End Function

    Private Function RelativeSubDirRecursive(fPath As String, Optional SubDepth As Integer = 1) As String
        If SubDepth < 1 Then Return ""

        If fPath.Length <= 3 Then
            For c = 0 To InvalidPathCh.Length - 1
                'fPath = fPath.Replace(InvalidFileSystemCh(c), "") 'or Inv as Hashset
                fPath = fPath.Replace(InvalidPathCh(c), "")
            Next c
            Return If(fPath.Length > 0, fPath & SeparatorD, "")
        End If

        Dim parentDir = fPath
        For i = 1 To SubDepth
            If parentDir.Length > 3 AndAlso DirExists(parentDir) Then
                parentDir = parentDir.Parent
            Else
                Exit For
            End If
        Next
        Return fPath.Replace(parentDir, "")
    End Function

    Private Sub EncodeAudio(ap As AudioProfile, Optional SubDepth As Integer = 1)
        p.TempDir = OutPath
        Dim outP As String = ap.GetOutputFile()
        Dim nOutD As String = OutPath & RelativeSubDirRecursive(ap.FileValue.Dir, SubDepth)
        ap = ap.DeepClone 'Is This Needed ??? Check This !!! ObjectHelp.GetCopy(ap)
        Audio.Process(ap)
        ap.Encode()

        If SubDepth > 0 AndAlso Not String.Equals(outP.Dir, nOutD) AndAlso FileExists(outP) Then
            Dim nOutP As String = nOutD & outP.FileName

            Try
                FolderHelp.Create(nOutD)
                Thread.Sleep(15)
                If Not FileExists(nOutP) Then  ' ap.File.FileName Prevent Overwrite ???
                    FileHelp.Move(outP, nOutP)
                Else
                    Log.Write("Audio Converter error, not copied to Output ", outP & " Already exist in: " & nOutP)
                End If
            Catch ex As Exception
                Log.Write("Audio Converter IO copy exception", ex.ToString)
            End Try

            Thread.Sleep(30)
            If Not DirExists(nOutD) Then
                Log.Write("Audio Converter error", "Failed to create output directory: " & nOutD)
            ElseIf Not FileExists(nOutP) Then
                Log.Write("Audio Converter error", "Failed to create output file in path: " & nOutP)
            End If
        Else
            If Not FileExists(outP) Then
                Log.Write("Audio Converter error", "Failed to create output file: " & outP)
            End If
        End If
    End Sub

    Private Sub bnAudioConvert_Click(sender As Object, e As EventArgs) Handles bnConvert.Click
        Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim dc As Integer = FindDuplicates(True, srC)
        If dc > 0 Then
            If MsgQuestion(dc & " duplicated file(s) found.  Are you sure ?") <> DialogResult.OK Then Exit Sub
        End If

        Using dialog As New FolderBrowserDialog With {
            .Description = "Please select output directory :",
            .UseDescriptionForTitle = True,
            .RootFolder = Environment.SpecialFolder.MyComputer}
            If dialog.ShowDialog <> DialogResult.OK Then Exit Sub
            If dialog.SelectedPath Is Nothing OrElse Not DirExists(dialog.SelectedPath) Then
                Exit Sub
            End If
            PopulateTaskS = -1
            OutPath = dialog.SelectedPath.FixDir
        End Using

        p.TempDir = OutPath

        For r = 0 To AudioCL.Count - 1
            If dgvAudio.Rows.GetRowState(r) >= &B1100000 AndAlso AudioSBL.Item(r).FileValue.Dir.Contains(OutPath) Then
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
                td.AddButton(i.ToInvariantString, i)
            Next
            td.SelectedValue = -1
            subDepth = td.Show
            If subDepth < 0 Then Exit Sub
        End Using

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
                    Dim fi = New FileInfo(ap.FileValue)
                    If fi.Exists AndAlso fi.Length > 10 Then
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
            PopulateCache(1000000)
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
                    outFile = OutPath & RelativeSubDirRecursive(ap.FileValue.Dir, subDepth) & ap.GetOutputFile.FileName() 'ap.file.filename
                    Dim fi = New FileInfo(outFile)

                    If fi.Exists AndAlso fi.Length > 10 Then
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
            bnRemove.Select()
        End If
    End Sub

    Private Sub ProcessInputAudioFiles(Files As String())
        PopulateTaskS = -1
        Dim fC As Integer = Files.Length
        Dim rC As Integer = AudioCL.Count
        Dim occ As Integer = If(dgvAudio.CurrentCellAddress.X >= 0, dgvAudio.CurrentCellAddress.X, 2) ' -1 ,-1 = Nothing CurrentRow
        Dim apBack As AudioProfile()
        Dim fExts(fC - 1) As String
        Dim fkha(fC - 1) As Long
        Dim vftExts As HashSet(Of String)
        Dim uKeysCountPT = Task.Run(Function()
                                        SW1.Restart()
                                        vftExts = FileTypes.VideoAudio.ToHashSet(StringComparer.Ordinal)
                                        Array.Sort(Files)
                                        Parallel.For(0, fC, Sub(i)
                                                                If Files(i).Length > 6 Then
                                                                    fkha(i) = ((Files(i).GetHashCode + 2147483648L) << 16) + Files(i).Length  '-->Fast Keyhash
                                                                    fExts(i) = Files(i).Ext
                                                                End If
                                                                'Dim fi As New FileInfo(Files(i)) 'Good Safe Hash
                                                                'If fi.Exists Then filesKeysH(i) = fi.Length + fi.LastWriteTime.Ticks + Files(i).Length + (CLng(Files(i).GetHashCode) << 20)
                                                            End Sub)
                                        Dim uhs As New HashSet(Of Long)(fkha.Length)
                                        For i = 0 To fkha.Length - 1
                                            If fkha(i) > 6 Then uhs.Add(fkha(i))
                                        Next

                                        If rC > 0 AndAlso uhs.Count >= Files.Length Then apBack = AudioSBL.ToArray
                                        SW1.Stop()
                                        Return uhs.Count
                                    End Function)

        If fC > 10 AndAlso MsgQuestion("Add " & fC & " files ?") <> DialogResult.OK Then
            PopulateCache(4000000)
            Erase fkha, fExts, Files, apBack
            Exit Sub
        End If

        Dim profileSelection As New SelectionBox(Of AudioProfile) With {.Title = "Please select Audio Profile"}

        If p.Audio0 IsNot Nothing AndAlso Not p.Audio0.IsNullProfile AndAlso Not p.Audio0.IsMuxProfile AndAlso (CurrentType Is Nothing OrElse p.Audio0.GetType Is CurrentType) Then
            profileSelection.AddItem("Current Project 1: " & p.Audio0.ToString, p.Audio0)
        End If
        If p.Audio1 IsNot Nothing AndAlso Not p.Audio1.IsNullProfile AndAlso Not p.Audio1.IsMuxProfile AndAlso (CurrentType Is Nothing OrElse p.Audio1.GetType Is CurrentType) Then
            profileSelection.AddItem("Current Project 2: " & p.Audio1.ToString, p.Audio1)
        End If
        For Each APr In s.AudioProfiles
            If APr Is Nothing OrElse APr.IsMuxProfile OrElse APr.IsNullProfile OrElse (CurrentType IsNot Nothing AndAlso APr.GetType IsNot CurrentType) Then Continue For
            profileSelection.AddItem(APr)
        Next

        'SelectionBoxForm.StartPosition = FormStartPosition.CenterParent      'Drag Drop  out of center
        If profileSelection.Show <> DialogResult.OK OrElse profileSelection.Items.Count < 1 Then
            PopulateCache(4000000)
            Erase fkha, fExts, Files, apBack
            Exit Sub
        End If

        SW1.Stop()
        Dim st1 = If(uKeysCountPT.IsCompleted, SW1.ElapsedTicks / SWFreq, -1.0R)
        Dim st22 As Double
        SW1.Restart()

        RemoveHandlersDGV()
        Dim clnT = Task.Run(Sub()
                                SW2.Restart()
                                AudioCL.Reset()
                                AudioSBL.Clear()
                                AudioCL.Capacity = If(rC > 0, rC + fC, fC)
                                AudioSBL.RaiseListChangedEvents = False
                                AudioSBL.InnerListChanged()
                                AudioSBL.RemoveSort()
                                SW2.Stop()
                                st22 = SW2.ElapsedTicks / SWFreq
                            End Sub)

        If fC > 500 Then StatusText("Adding Files...")

        Dim ap As AudioProfile = profileSelection.SelectedValue.DeepClone
        CurrentType = If(ap.IsGapProfile, GetType(GUIAudioProfile), GetType(BatchAudioProfile))
        ap.FileValue = Nothing
        ap.FileKeyHashValue = MediaInfo.KeyDefault
        ap.DisplayNameValue = Nothing
        ap.SourceSamplingRateValue = 0
        ap.Stream = Nothing
        ap.StreamName = ""
        ap.Streams?.Clear()
        ap.Delay = 0
        'If ap.Language Is Nothing Then ap.Language = Language.CurrentCulture

        If ap.IsGapProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)
            gap.Name = ap.DefaultName
            gap.DefaultnameValue = Nothing
            gap.SourceDepth = 0
            gap.SourceChannels = 0
            If gap.Params.Codec <> AudioCodec.DTS Then ap.ExtractDTSCore = False
        End If

        Dim apTS(fC - 1) As AudioProfile
        Dim dummyStream As New AudioStream
        Dim autoStream As Boolean
        Dim nullE As Integer

        If uKeysCountPT.Exception IsNot Nothing OrElse uKeysCountPT.Result <> fC Then
            PopulateCache(4000000)
            MsgInfo("File Error/Key Collision!" & uKeysCountPT.Result & "|" & fC, uKeysCountPT?.Exception?.ToString)
            Erase fkha, fExts, Files, apBack
            UpdateControls()
            Exit Sub
        End If

        SW1.Stop()
        Dim st2 = SW1.ElapsedTicks / SWFreq
        SW1.Restart()
        'Dim clth = AudioCL.AsThreadSafe

        Parallel.For(0, fC, ' New ParallelOptions With {.MaxDegreeOfParallelism = 1},
                         Sub(i As Integer)
                             Dim apN = ap.DeepClone
                             apN.FileValue = Files(i)
                             apN.FileKeyHashValue = fkha(i)

                             If vftExts.Contains(fExts(i)) Then
                                 apN.Streams = MediaInfo.GetAudioStreams(apN.FileValue, fkha(i))

                                 If apN.Streams.Count > 0 Then
                                     apN.Stream = apN.Streams(0)
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
                                     nullE += 1
                                     Log.WriteLine("-->Audio stream not found, skipping: " & BR & apN.FileValue)
                                     Return
                                 End If

                                 If Not autoStream AndAlso apN.Stream IsNot Nothing AndAlso apN.Streams.Count > 1 Then
                                     SyncLock dummyStream
                                         If Not autoStream Then
                                             Dim streamSelection = New SelectionBox(Of AudioStream) With {
                                             .Title = "Stream Selection",
                                             .Text = "Please select an audio stream for: " & BR & apN.FileValue.ShortBegEnd(60, 60)}
                                             For Each stream In apN.Streams
                                                 streamSelection.AddItem(stream)
                                             Next
                                             streamSelection.AddItem(" > Use first stream and don't ask me again <", dummyStream)
                                             If streamSelection.Show() <> DialogResult.OK Then Return

                                             If streamSelection.SelectedValue IsNot dummyStream Then
                                                 apN.Stream = streamSelection.SelectedValue
                                             Else
                                                 autoStream = True
                                             End If
                                         End If
                                     End SyncLock
                                 End If
                             End If
                             'clth.Add(apN.DeepClone)
                             apTS(i) = apN
                         End Sub)
        SW1.Stop()
        Dim st3 = SW1.ElapsedTicks / SWFreq
        SW1.Restart()
        If clnT.Exception IsNot Nothing Then MsgInfo("ClearTaskExcept", clnT.Exception.ToString)
        If Not clnT.IsCompleted Then clnT.Wait()

        If apBack?.Length > 0 Then AudioCL.AddRange(apBack)
        If nullE <= 0 Then AudioCL.AddRange(apTS) Else AudioCL.AddRange(apTS.WhereF(Function(itm As AudioProfile) itm IsNot Nothing))
        AudioSBL.InnerListChanged()

        ' If nullE = 0 Then AudioSBl.AddRange(apTS) Else AudioSBL.AddRange(apTS.Wheref(Function(itm As AudioProfile) itm IsNot Nothing)) 
        AudioSBL.RaiseListChangedEvents = True
        AudioSBL.ResetBindings()
        Dim nrC = AudioCL.Count

        If nrC > 0 Then
            dgvAudio.CurrentCell = dgvAudio.Rows.Item(nrC - 1).Cells(occ)
            dgvAudio.Rows.Item(If(rC > 0, rC - 1, 0)).Selected = True
            PopulateIter = 1
            If nrC > 100 Then dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            AutoSizeColumns()
            PopulateCache(4000000)
        End If

        SW1.Stop()
        Dim st4 = SW1.ElapsedTicks / SWFreq

        UpdateControls(SelRowsCount:=If(nrC > 1, 2, 1))
        LastKeyDown = -1
        SelectChangeES = False
        ButtonAltImage(False)
        AddHandlersDGV()

        Task.Run(Sub()
                     Dim dc = FindDuplicates(False)
                     BeginInvoke(Sub()
                                     dgvAudio.Columns(0).HeaderCell.SortGlyphDirection = SortOrder.None
                                     dgvAudio.Columns(1).HeaderCell.SortGlyphDirection = SortOrder.None
                                     dgvAudio.Columns(2).HeaderCell.SortGlyphDirection = SortOrder.None
                                     dgvAudio.Columns(3).HeaderCell.SortGlyphDirection = If(rC <= 0, SortOrder.Ascending, SortOrder.None)
                                     laThreads.Text = $"{st1:F2} {st2:F3} {st3:F2} {st4:F2} {st22:F3}"
                                     dgvAudio.Select()
                                 End Sub)
                     If dc <= 0 Then Return
                     Console.Beep(260, 150)
                     FlashingText(dc.ToInvariantString & " duplicated files found !", 12, 135)
                 End Sub)

        If StatUpdateTask Is Nothing OrElse StatUpdateTask.IsCompleted Then
            StatUpdateTask = Task.Factory.StartNew(StatUpdateTaskA, TaskCreationOptions.LongRunning)
            StatUpdateTask.ContinueWith(Sub()
                                            If StatUpdateTask.Exception IsNot Nothing Then
                                                Log.Write("Stat Task Exception:", $"{StatUpdateTask.Status} Inner: {StatUpdateTask.Exception.InnerException?.ToString} |Except: {StatUpdateTask.Exception} |InnType:{StatUpdateTask.Exception.InnerException.GetType()} |HRes:{StatUpdateTask.Exception.HResult}")
                                                Console.Beep(2250, 100)
                                            End If
                                        End Sub)
        End If
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
                                                   If PopulateTaskS <= 0 OrElse Not AudioConverterMode Then
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
                                                       If arP.Length <= 20 Then PopulateIter += 20
                                                       Dim mTS = MediaInfo.Cache.AsThreadSafe

                                                       If PopulateTaskS > 0 AndAlso PopulateIter < 200 AndAlso AudioConverterMode Then

                                                           Dim pl = Parallel.For(0, arP.Length, New ParallelOptions With {.MaxDegreeOfParallelism = Math.Max(CPUsC \ 2, 1)},
                                                                                 Sub(i, pls)
                                                                                     If PopulateTaskS <= 0 OrElse Not AudioConverterMode Then
                                                                                         pls.Stop()
                                                                                         Return
                                                                                     End If
                                                                                     If (dgvAudio.Rows.GetRowState(i) And DataGridViewElementStates.Displayed) = 0 Then
                                                                                         If Not MediaInfo.Cache.ContainsKey(arP(i).FileKeyHashValue) Then
                                                                                             Dim mi = New MediaInfo(arP(i).FileValue)
                                                                                             If PopulateTaskS <= 0 Then
                                                                                                 pls.Stop()
                                                                                                 Return
                                                                                             End If
                                                                                             If (dgvAudio.Rows.GetRowState(i) And DataGridViewElementStates.Displayed) = 0 Then
                                                                                                 mTS.Item(arP(i).FileKeyHashValue) = mi
                                                                                                 ''MediaInfo.Cache.Item(arP(i).FileKeyHashValue) = mi
                                                                                                 If arP(i).DefaultName Is Nothing Then ' Or remove it ???
                                                                                                 End If
                                                                                                 If arP(i).DisplayName Is Nothing Then
                                                                                                 End If
                                                                                             End If
                                                                                         Else
                                                                                             If arP(i).DefaultName Is Nothing Then
                                                                                             End If
                                                                                             If arP(i).DisplayName Is Nothing Then
                                                                                             End If
                                                                                         End If
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
                                                       Log.Write($"{Delay}Del PopulateTask Catch", $"{PopulateTaskS}tS{PopulateWSW.ElapsedTicks / SWFreq}msW {PopulateRSW?.ElapsedTicks / SWFreq}msR|iter {PopulateIter} {ex} {ex.InnerException?.ToString} | {PopulateTask?.Status.ToString} | {PopulateTask?.Exception?.ToString} {PopulateTaskW?.Status.ToString}")
                                                   End Try
                                               End If
                                               PopulateRSW.Stop()
                                           End Sub

        If PopulateTask Is Nothing OrElse PopulateTask.IsCompleted Then
            PopulateTask = Task.Factory.StartNew(Sub() pTaskA(If(Delay = -1, 7000000, Delay)), TaskCreationOptions.LongRunning)
        ElseIf PopulateTaskW Is Nothing OrElse PopulateTaskW.IsCompleted Then
            PopulateTaskW = PopulateTask.ContinueWith(Sub()
                                                          If PopulateTaskS >= 0 AndAlso PopulateTask.IsCompleted Then
                                                              PopulateTask = Task.Factory.StartNew(Sub() pTaskA(Delay + 2500000), TaskCreationOptions.LongRunning)
                                                          End If
                                                      End Sub)
        End If
    End Sub

    Private Sub bnAudioAdd_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        Dim tdS As Integer
        Using td As New TaskDialog(Of Integer)
            td.AddCommand("Add files", 1)
            td.AddCommand("Add folder", 2)
            tdS = td.Show
        End Using
        Try
            Dim avF As String()
            Select Case tdS
                Case 1
                    Using dialog As New OpenFileDialog With {.Multiselect = True}
                        Dim ave = FileTypes.Audio.ConcatA(FileTypes.VideoAudio)
                        dialog.Filter = "*." & String.Join(";*.", ave) & "|*." & String.Join(";*.", ave) & "|All Files|*.*"
                        If dialog.ShowDialog = DialogResult.OK Then
                            avF = dialog.FileNames.WhereF(Function(f) AudioVideoFExts.Contains(f.Ext))
                            If avF.Length <= 0 Then MsgWarn("No supported Audio/Video files found.")
                        End If
                    End Using
                Case 2
                    Using dialog As New FolderBrowserDialog With {.RootFolder = Environment.SpecialFolder.MyComputer}
                        If dialog.ShowDialog = DialogResult.OK Then
                            Dim opt As Integer
                            Dim gfT As Task(Of String())

                            If Directory.GetDirectories(dialog.SelectedPath).Length > 0 Then
                                gfT = Task.Run(Function()
                                                   ' Return Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories).WhereF(Function(f) AudioVideoFExts.Contains(f.Ext))
                                                   Return Directory.EnumerateFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories).TakeWhile(Function(f) tdS = 2).Where(Function(f) AudioVideoFExts.Contains(f.Ext)).ToArray
                                               End Function)
                                If MsgQuestion("Include sub folders?", TaskDialogButtons.YesNo) = DialogResult.Yes Then opt = 1
                            End If

                            If opt = 0 Then
                                tdS = 0
                                avF = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.TopDirectoryOnly).WhereF(Function(f) AudioVideoFExts.Contains(f.Ext()))
                            Else
                                avF = gfT.Result
                            End If
                            If avF.Length <= 0 Then MsgWarn("No supported Audio/Video files found.")
                        End If
                    End Using
            End Select

            If avF?.Length > 0 Then ProcessInputAudioFiles(avF)

        Catch ex As Exception
            Log.Write("Audio Converter File opening error", ex.ToString)
            MsgInfo("Files opening error", ex.ToString)
        Finally
        End Try
    End Sub

    Private Sub dgvAudio_DragEnter(sender As Object, e As DragEventArgs) Handles dgvAudio.DragEnter
        Try
            Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
            If files IsNot Nothing Then
                e.Effect = If(files.AnyF(Function(f) AudioVideoFExts.Contains(f.Ext)), DragDropEffects.Copy, DragDropEffects.None)
            End If
        Catch
            e.Effect = DragDropEffects.None
        End Try
    End Sub

    Private Sub dgvAudio_DragDrop(sender As Object, e As DragEventArgs) Handles dgvAudio.DragDrop
        Try
            Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
            If files IsNot Nothing Then
                Dim avf = files.WhereF(Function(f) AudioVideoFExts.Contains(f.Ext))
                If avf.Length > 0 Then ProcessInputAudioFiles(avf)
            End If
        Catch ex As Exception
            Log.Write("Audio Converter Files DragDrop error", ex.ToString)
            MsgBox(ex.ToString, MsgBoxStyle.Exclamation, "Files opening error")
        End Try
    End Sub

    Private Sub bnAudioEdit_Click(sender As Object, e As EventArgs) Handles bnEdit.Click
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

        If sr0idx < 0 Then Exit Sub
        Dim ap As AudioProfile = AudioSBL(sr0idx)
        If Not FileExists(ap.File) Then
            Task.Run(Sub() FlashingText("File not found ! Pos: " & (sr0idx + 1).ToInvariantString))
            Exit Sub
        End If

        Dim isGap = ap.IsGapProfile
        Dim lang As Language = If(ap.Language, Language.CurrentCulture)
        Dim oldDelay As Integer = ap.Delay
        dgvAudio.FirstDisplayedScrollingRowIndex = sr0idx
        StatusText("Edit:" & (sr0idx + 1).ToInvariantString & " Sel: " & srC.ToInvariantString & " / Tot: " & rC.ToInvariantString)

        If ap.EditAudioConv() = DialogResult.OK Then
            PopulateTaskS = -1
            RemoveHandlersDGV()
            StatusText("Applying settings...")

            If isGap Then
                Dim gap = DirectCast(ap, GUIAudioProfile)
                gap.Name = ap.DefaultName
                gap.DefaultnameValue = Nothing
                If gap.Params.Codec <> AudioCodec.DTS Then ap.ExtractDTSCore = False
            End If

            AudioSBL.ResetItem(sr0idx)

            If srC > 1 Then
                AudioSBL.RaiseListChangedEvents = False
                Dim clang As Boolean = Not ap.Language.Equals(lang)
                If clang Then lang = ap.Language
                Dim cDelayTo0 As Boolean = ap.Delay = 0 AndAlso ap.Delay <> oldDelay
                'ap.DisplayNameValue = Nothing
                Dim srCache(rC - 1) As Boolean
                srCache(sr0idx) = True
                Dim arr As AudioProfile() = AudioSBL.ToArray
                Parallel.For(0, rC, Sub(r)                      ' 
                                        If dgvAudio.Rows.GetRowState(r) >= &B1100000 AndAlso r <> sr0idx Then
                                            Dim ap0 As AudioProfile = ap.DeepClone
                                            Dim apn As AudioProfile = AudioSBL.Item(r)
                                            ap0.FileValue = apn.FileValue
                                            ap0.FileKeyHashValue = apn.FileKeyHashValue
                                            ap0.StreamName = apn.StreamName
                                            ap0.Stream = apn.Stream
                                            ap0.Streams = apn.Streams
                                            If cDelayTo0 Then ap0.Delay = 0 'needed?
                                            If clang Then ap0.Language = lang  'needed?
                                            ap0.SourceSamplingRateValue = apn.SourceSamplingRateValue
                                            ap0.DisplayNameValue = apn.DisplayNameValue

                                            If isGap Then
                                                Dim gap0 = DirectCast(ap0, GUIAudioProfile)
                                                Dim gapn = DirectCast(apn, GUIAudioProfile)
                                                gap0.SourceDepth = gapn.SourceDepth
                                                gap0.SourceChannels = gapn.SourceChannels
                                                If gap0.DefaultName Is Nothing Then
                                                End If
                                            End If

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
            PopulateCache(4000000)
            AddHandlersDGV()
        End If

        UpdateControls()
    End Sub

    Private Sub ButtonAltImage(SetAltImg As Boolean)
        If SetAltImg Then
            If ButtonAltImageT IsNot Nothing AndAlso Not ButtonAltImageT.IsCompleted Then Exit Sub
            ButtonAltImageT = Task.Run(Sub()
                                           Thread.Sleep(45)
                                           SyncLock bnRemove
                                               If Not ButtonAltImgDone AndAlso My.Computer.Keyboard.ShiftKeyDown AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso Not My.Computer.Keyboard.AltKeyDown Then
                                                   ButtonAltImgDone = True
                                                   BeginInvoke(Sub()
                                                                   bnRemove.Text = "From Disk"
                                                                   bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Delete)
                                                               End Sub)
                                               End If
                                           End SyncLock
                                       End Sub)
        Else
            If ButtonAltImgDone Then
                ButtonAltImgDone = False
                BeginInvoke(Sub()
                                bnRemove.Text = "&Remove "
                                bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
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
                Close()
                Dispose(True)
                g.MainForm.Close()
                g.MainForm.Dispose()
                Application.Exit()
        End Select
    End Sub

    Private Sub AudioConverterForm_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.F3 Then
            SW2.Stop()
            If LastKeyDown = 3 Then Task.Run(Sub()
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
                If AudioCL.Count > 1000 OrElse (AudioCL.Count > 100 AndAlso MediaInfo.Cache.Count < 100) Then StatusText("Sorting...")
                SW2.Restart()
            End If
        Else
            Select Case e.KeyData
                Case Keys.Delete

                    SW2.Restart()

                    PopulateTaskS = 0
                    DeleteRows()
                    e.Handled = True

                    SW2.Stop()

                Case Keys.Delete Or Keys.Shift Or Keys.Control
                    DeleteFiles()
                    e.Handled = True
            End Select
        End If
    End Sub

    Private Sub bnAudioRemove_Click(sender As Object, e As EventArgs) Handles bnRemove.Click
        If My.Computer.Keyboard.ShiftKeyDown AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso Not My.Computer.Keyboard.AltKeyDown Then
            DeleteFiles()
        Else

            SW2.Restart()

            PopulateTaskS = 0
            DeleteRows()

            SW2.Stop()

        End If
    End Sub

    Private Sub DeleteFiles()
        PopulateTaskS = -1
        Dim FS As MyServices.FileSystemProxy = My.Computer.FileSystem
        Dim fPath As String
        Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim fileListDup As New List(Of String)(srC)
        Dim dirListDup As New List(Of String)(srC)
        Dim rowIdxL As New List(Of Integer)(srC)
        Dim ask As Boolean
        Dim fokC As Integer
        Dim dokC As Integer
        Dim exitDel As Action = Sub()
                                    ButtonAltImage(False)
                                    UpdateControls()
                                End Sub
        For r = 0 To AudioCL.Count - 1
            If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                fPath = AudioSBL.Item(r).File
                If FS.FileExists(fPath) Then
                    fileListDup.Add(fPath)
                    dirListDup.Add(fPath.Dir)
                    rowIdxL.Add(r)
                End If
            End If
        Next r

        If fileListDup.Count = 0 Then
            exitDel()
            Exit Sub
        End If

        Dim fileList = fileListDup.Distinct(StringComparer.OrdinalIgnoreCase).ToArray
        Array.Sort(fileList, StringComparer.Ordinal)
        Dim dirC = dirListDup.Distinct(StringComparer.OrdinalIgnoreCase).Count

        If (dgvAudio.Rows.GetRowState(dgvAudio.SelectedRows.Item(0).Index) And DataGridViewElementStates.Displayed) = 0 Then
            dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.SelectedRows.Item(0).Index
        End If

        If Msg(fileList.Length & " files found in " & dirC & " directories" & BR & "Delete from disk ?", Nothing, MsgIcon.Question, TaskDialogButtons.YesNo, DialogResult.No) <> DialogResult.Yes Then
            exitDel()
            Exit Sub
        End If

        ask = MsgQuestion("Ask for each file to delete", TaskDialogButtons.YesNo) = DialogResult.Yes

        StatusText("Deleting from disk...")

        If Not My.Computer.Keyboard.ShiftKeyDown Then ' FailSafe
            exitDel()
            Exit Sub
        End If

        For f = 0 To fileList.Length - 1
            fPath = fileList(f)

            If ask Then
                Select Case MsgQuestion("Delete File ?" & BR & fPath, TaskDialogButtons.YesNoCancel)
                    Case DialogResult.No
                        Continue For
                    Case DialogResult.Cancel
                        exitDel()
                        Exit Sub
                End Select
            End If

            Try
                FS.DeleteFile(fPath, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)
                fokC += 1
            Catch ex As Exception
                Log.Write("Delete file exception", "Delete error File: " & fPath & BR & ex.ToString)
            End Try

            Try
                If FS.GetFiles(fPath.Dir, FileIO.SearchOption.SearchTopLevelOnly).Count = 0 AndAlso MsgQuestion("Delete empty Directory ?" & BR & fPath.Dir, TaskDialogButtons.YesNo) = DialogResult.Yes Then
                    FS.DeleteDirectory(fPath.Dir, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)
                    dokC = +1
                End If
            Catch ex As Exception
                Log.Write("Delete dir exception", "Delete error Dir: " & fPath.Dir & BR & ex.ToString)
            End Try

            Try 'UnTested!!! to do: Add don't ask for all or cancel for all
                If fPath.Length > 3 AndAlso FS.GetFiles(FS.GetParentPath(fPath.Dir), FileIO.SearchOption.SearchTopLevelOnly).Count = 0 AndAlso MsgQuestion("Delete empty Directory ?" & BR & FS.GetParentPath(fPath.Dir), TaskDialogButtons.YesNo) = DialogResult.Yes Then
                    FS.DeleteDirectory(FS.GetParentPath(fPath.Dir), FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)
                    dokC = +1
                End If
            Catch ex As Exception
                Log.Write("Delete dir exception", "Delete error Dir: " & FS.GetParentPath(fPath.Dir) & BR & ex.ToString)
            End Try
        Next f

        If dgvAudio.RowHeadersWidth < 80 Then dgvAudio.RowHeadersWidth = 80

        Dim ri As Integer
        For r = 0 To rowIdxL.Count - 1
            ri = rowIdxL.Item(r)

            If FS.FileExists(AudioSBL.Item(ri).File) Then ' Better Write to Log and Show Log than leaving deleted files in DGV ???
                dgvAudio.Rows.Item(ri).HeaderCell.Value = "Skipped"
            Else
                dgvAudio.Rows.Item(ri).HeaderCell.Value = "Deleted"
                'AudioSBL.Item(ri).File = String.Empty  'Or leave displayed deleted File??? File Exists Errors Handling for Multi-Editing Untested!!!
            End If
        Next

        PopulateCache(1000000)
        If fokC > 0 Then MsgInfo(fokC & " files out of " & fileList.Length & " , " & If(dokC > 0, dokC & " directories ", "") & "deleted to recycle bin")
        Task.Delay(100).Wait()
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
            RemoveHandlersDGV(True)
            AudioCL.Reset()
            AudioSBL.Clear()
            AudioSBL.InnerListChanged()
            AudioSBL.ResetBindings()
            dgvAudio.Rows.Clear()
            UpdateControls()
            CurrentType = Nothing
            GC.Collect()
            Exit Sub
        End If

        If srC > 100 Then StatusText("Removing...")
        SelectChangeES = True
        Dim crI As Integer = -1

        If srC < 250 Then
            For r = rC - 1 To 0 Step -1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    AudioSBL.RemoveAt(r)
                End If
            Next r

        Else
            RemoveHandler dgvAudio.Scroll, AddressOf dgvAudio_Scroll
            AudioSBL.RaiseListChangedEvents = False 'move after clear???
            Dim ccI As Integer = dgvAudio.CurrentCellAddress.X
            Dim dArr(rC - srC - 1) As AudioProfile
            Dim i As Integer

            For r = 0 To rC - 1
                If dgvAudio.Rows.GetRowState(r) < &B1100000 Then
                    dArr(i) = AudioSBL.Item(r)
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
        PopulateCache(4000000)
        SelectChangeES = False
    End Sub

    Private Sub bnAudioUp_Click(sender As Object, e As EventArgs) Handles bnUp.Click
        SW2.Restart()
        PopulateTaskS = 0
        SelectChangeES = True
        Dim cc As Point = dgvAudio.CurrentCellAddress
        Dim crI As Integer = cc.Y

        If crI > 0 Then
            AudioSBL.RaiseListChangedEvents = False
            Dim current As AudioProfile = AudioSBL(crI)
            AudioSBL.RemoveAt(crI)
            crI -= 1
            AudioSBL.Insert(crI, current)
            dgvAudio.CurrentCell = dgvAudio.Rows.Item(crI).Cells(cc.X)
            AudioSBL.RaiseListChangedEvents = True
            AudioSBL.ResetItem(crI)
        End If

        PopulateCache(4000000)
        UpdateControls(1, crI)
        SelectChangeES = False
        SW2.Stop()
    End Sub
    Private Sub bnAudioDown_Click(sender As Object, e As EventArgs) Handles bnDown.Click
        SW2.Restart()
        PopulateTaskS = 0
        SelectChangeES = True
        Dim cc As Point = dgvAudio.CurrentCellAddress
        Dim crI As Integer = cc.Y

        If crI < AudioCL.Count - 1 Then
            AudioSBL.RaiseListChangedEvents = False
            Dim current As AudioProfile = AudioSBL(crI)
            AudioSBL.RemoveAt(crI)
            crI += 1
            AudioSBL.Insert(crI, current)
            dgvAudio.CurrentCell = dgvAudio.Rows.Item(crI).Cells(cc.X)
            AudioSBL.RaiseListChangedEvents = False
            AudioSBL.ResetItem(crI)
        End If

        PopulateCache(4000000)
        UpdateControls(1, crI)
        SelectChangeES = False
        SW2.Stop()
    End Sub

    Private Sub bnAudioPlay_Click(sender As Object, e As EventArgs) Handles bnPlay.Click
        Dim crI As Integer = dgvAudio.CurrentCellAddress.Y
        dgvAudio.FirstDisplayedScrollingRowIndex = crI
        If FileExists(AudioSBL(crI).File) Then g.Play(AudioSBL(crI).File) Else Task.Run(Sub() FlashingText("File not found ! Pos: " & (crI + 1).ToInvariantString))
    End Sub

    Private Sub bnAudioMediaInfo_Click(sender As Object, e As EventArgs) Handles bnAudioMediaInfo.Click
        Dim crI As Integer = dgvAudio.CurrentCellAddress.Y
        dgvAudio.FirstDisplayedScrollingRowIndex = crI
        If FileExists(AudioSBL(crI).File) Then g.DefaultCommands.ShowMediaInfo(AudioSBL(crI).File) Else Task.Run(Sub() FlashingText("File not found ! Pos: " & (crI + 1).ToInvariantString))
    End Sub

    Private Sub bnCMD_Click(sender As Object, e As EventArgs) Handles bnCMD.Click
        If FileExists(AudioSBL(dgvAudio.CurrentCellAddress.Y).File) Then
            Dim ap = AudioSBL(dgvAudio.CurrentCellAddress.Y)
            dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.CurrentCellAddress.Y
            Select Case ap.GetType
                Case GetType(GUIAudioProfile)
                    g.ShowCommandLinePreview("Command Line", (TryCast(ap, GUIAudioProfile)?.GetCommandLine(True)))
                Case GetType(BatchAudioProfile)
                    g.ShowCommandLinePreview("Command Line", (TryCast(ap, BatchAudioProfile)?.GetCode))
            End Select
        Else
            Task.Run(Sub() FlashingText("File not found ! Pos: " & (dgvAudio.CurrentCellAddress.Y + 1).ToInvariantString))
        End If
    End Sub

    Private Sub numThreads_ValueChanged(numEd As NumEdit) Handles numThreads.ValueChanged
        MaxThreads = CInt(numThreads.Value)
        If numThreads.Value > CPUsC Then
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
        AutoSizeColumns(My.Computer.Keyboard.ShiftKeyDown)
        UpdateControls()
    End Sub

    Private Sub laAC_DoubleClick(sender As Object, e As EventArgs) Handles laAC.DoubleClick
        SyncLock SLock
            PopulateTaskS = -1
            PopulateIter = 999
            StatusText("Refreshing...")
            ButtonAltImgDone = True
            ButtonAltImage(False)
            RemoveHandlersDGV(True)
            LastKeyDown = -1
            SelectChangeES = False
            Visible = True
            BringToFront()
            Activate()
            dgvAudio.Select()
            Console.Beep(700, 60)

            SW2.Restart()

            Dim rC = AudioCL.Count
            Dim occ As Point = dgvAudio.CurrentCellAddress
            Dim ocF As Long = If(rC > 0 AndAlso occ.Y >= 0, AudioSBL.Item(occ.Y).FileKeyHashValue, Long.MinValue)
            AudioSBL.RemoveSort()
            AudioSBL.RaiseListChangedEvents = False
            dgvAudio.DataSource = Nothing
            dgvAudio.Rows.Clear()
            If Not PopulateTask?.IsCompleted AndAlso Not PopulateWSW.IsRunning Then PopulateTask.Wait(60)
            Dim miT = Task.Run(Sub() MediaInfo.ClearCache())
            Text = "AudioConverter"
            Dim uAR As AudioProfile()
            If FindDuplicates(False) > 0 Then uAR = AudioCL.Distinct(New AudioProfileHComparer).ToArray Else uAR = AudioCL.ToArray
            Dim isGAP = rC > 0 AndAlso uAR(0).IsGapProfile
            'Dim ddup = AudioCL.GroupBy(Function(f As AudioProfile) f, New AudioProfileFComparer).Where(Function(g) g.Count > 1).Select(Function(d) d.Key.FileKeyHashValue & d.Key.File).ToArray
            AudioSBL.Clear()
            AudioCL.Reset()
            AudioCL.Capacity = uAR.Length

            SW2.Stop()
            Dim st1 = SW2.ElapsedTicks / SWFreq
            SW2.Restart()
            Dim fkhA(uAR.Length - 1) As Long
            If rC > 0 Then Parallel.For(0, uAR.Length, Sub(i)
                                                           uAR(i).SourceSamplingRateValue = 0
                                                           uAR(i).DisplayNameValue = Nothing
                                                           If isGAP Then
                                                               DirectCast(uAR(i), GUIAudioProfile).SourceChannels = 0
                                                               DirectCast(uAR(i), GUIAudioProfile).SourceDepth = 0
                                                               DirectCast(uAR(i), GUIAudioProfile).DefaultnameValue = Nothing
                                                           End If
                                                           'If uAR(i).FileKeyHashValue <> ((uAR(i).FileValue.GetHashCode + 2147483648L) << 16) + uAR(i).FileValue.Length Then Console.Beep(5100, 90) 'Del It
                                                           'If uAR(i).FileKeyHashValue <= 0 Then 
                                                           uAR(i).FileKeyHashValue = ((uAR(i).FileValue.GetHashCode + 2147483648L) << 16) + uAR(i).FileValue.Length
                                                           fkhA(i) = uAR(i).FileKeyHashValue
                                                       End Sub)
            Dim crI As Integer = -1
            Dim uhs As New HashSet(Of Long)(fkhA.Length)
            For i = 0 To fkhA.Length - 1
                If fkhA(i) > 6 Then uhs.Add(fkhA(i))
                If fkhA(i) = ocF Then crI = i
            Next
            Dim uhC As Integer = uhs.Count
            AudioCL.AddRange(uAR)
            AudioSBL.InnerListChanged()

            SW2.Stop()
            Dim st2 = SW2.ElapsedTicks / SWFreq
            Console.Beep(900, 60)
            SW2.Restart()

            If Not miT.IsCompleted Then miT.Wait()
            AudioSBL.RaiseListChangedEvents = True
            dgvAudio.DataSource = AudioSBL
            AudioSBL.ResetBindings()
            AutoSizeColumns()
            If crI >= 0 Then dgvAudio.CurrentCell = dgvAudio.Rows.Item(crI).Cells.Item(If(occ.X >= 0, occ.X, 2))
            'dgvAudio.SelectAll()
            UpdateControls(If(rC > 0, 1, -2), If(crI >= 0, crI, -1))
            AddHandlersDGV()
            PopulateIter = 2
            PopulateCache(4000000)
            If uhC <> uAR.Length Then
                Task.Run(Sub() FlashingText("File/Key error: " & uhC.ToInvariantString & " K/F:" & uAR.Length.ToInvariantString, 20, 150))
                MsgInfo("File Error/Key Collision !!!", uhC.ToInvariantString & " Keys / Files: " & uAR.Length.ToInvariantString)
            End If
            uhs.Clear()
            uhs.TrimExcess()
            Erase uAR, fkhA

            SW2.Stop()
            laThreads.Text = $"{st1:F4}|{st2:F4}|{SW2.ElapsedTicks / SWFreq:F4}"
            dgvAudio.Refresh()
            Refresh()
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
            GC.Collect(2, GCCollectionMode.Forced, True, True)
            GC.WaitForPendingFinalizers()
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
            GC.Collect(2, GCCollectionMode.Forced, True, True)
            GC.WaitForPendingFinalizers()

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
            'Dim ap As Object
            Dim pi As PropertyInfo = dgvAudio.GetType().GetProperty("Events", BindingFlags.Instance Or BindingFlags.NonPublic)
            Dim list As EventHandlerList = DirectCast(pi.GetValue(dgvAudio, Nothing), EventHandlerList)
            'For Each fi As FieldInfo In gf
            '    ap = fi.GetValue(dgvAudio)
            '    list.RemoveHandler(ap, list(ap))
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

Public Class AudioProfileHComparer 'ToDo Add more than 1 stream from file
    Implements IEqualityComparer(Of AudioProfile)
    Public Function Equals2(ByVal x As AudioProfile, ByVal y As AudioProfile) As Boolean Implements IEqualityComparer(Of AudioProfile).Equals
        If x Is y Then Return True
        If x Is Nothing OrElse y Is Nothing Then Return False
        Return x.FileKeyHashValue = y.FileKeyHashValue
    End Function
    Public Function GetHashCode2(ByVal ap As AudioProfile) As Integer Implements IEqualityComparer(Of AudioProfile).GetHashCode
        If ap Is Nothing Then Return 0
        Return ap.FileKeyHashValue.GetHashCode()
    End Function
End Class
'Public Class AudioProfileFComparer 'ToDo Add more than 1 stream from file
'    Implements IEqualityComparer(Of AudioProfile)
'    Public Function Equals1(ByVal x As AudioProfile, ByVal y As AudioProfile) As Boolean Implements IEqualityComparer(Of AudioProfile).Equals
'        If x Is y Then Return True
'        If x Is Nothing OrElse y Is Nothing Then Return False
'        If x.FileValue.NullOrEmptyS AndAlso y.FileValue.NullOrEmptyS Then Return True
'        Return String.Equals(x.FileValue, y.FileValue, StringComparison.OrdinalIgnoreCase)
'    End Function
'    Public Function GetHashCode1(ByVal ap As AudioProfile) As Integer Implements IEqualityComparer(Of AudioProfile).GetHashCode
'        If ap Is Nothing Then Return 0
'        Return If(ap.FileValue.NotNullOrEmptyS, ap.FileValue.GetHashCode(), 0)
'    End Function
'End Class
