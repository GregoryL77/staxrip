Imports System.Collections.Concurrent
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
Imports Microsoft.VisualBasic
Imports JM.LinqFaster
Imports JM.LinqFaster.Parallel
Imports JM.LinqFaster.SIMD
Imports JM.LinqFaster.SIMD.Parallel
Imports StaxRip.UI

Public Class AudioConverterForm
    Inherits FormBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            PopulateTaskS = -2
            PopulateIter = 9999
            AudioConverterMode = False
            TBFindIdx = -1
            RemoveHandler CMS.Opening, AddressOf UpdateCMS
            RemoveHandler TBFind.TextChanged, AddressOf TBFind_TextChanged
            AudioSBL?.Dispose()
            dgvAudio?.Dispose()
            If BuildFindT?.IsCompleted Then
                BuildFindT.Dispose()
                BuildFindT = Nothing
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
    Friend WithEvents BnSort As Button
    Friend WithEvents TBFind As TextBox
    Friend WithEvents BnFindNext As Button
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.numThreads = New StaxRip.UI.NumEdit()
        Me.bnCMD = New StaxRip.UI.ButtonEx()
        Me.bnAudioMediaInfo = New StaxRip.UI.ButtonEx()
        Me.bnEdit = New StaxRip.UI.ButtonEx()
        Me.bnConvert = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnMenuAudio = New StaxRip.UI.ButtonEx()
        Me.laAC = New System.Windows.Forms.Label()
        Me.BnSort = New System.Windows.Forms.Button()
        Me.BnFindNext = New System.Windows.Forms.Button()
        Me.laThreads = New System.Windows.Forms.Label()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.bnPlay = New StaxRip.UI.ButtonEx()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.TBFind = New System.Windows.Forms.TextBox()
        Me.tlpMain.SuspendLayout()
        DirectCast(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'numThreads
        '
        Me.numThreads.SuspendLayout()
        Me.numThreads.BackColor = System.Drawing.SystemColors.Control
        Me.numThreads.CausesValidation = False
        Me.numThreads.Enabled = False
        Me.numThreads.Dock = System.Windows.Forms.DockStyle.Fill
        Me.numThreads.Location = New System.Drawing.Point(786, 365)
        Me.numThreads.Margin = New System.Windows.Forms.Padding(1, 2, 3, 1)
        Me.numThreads.Maximum = 64.0R
        Me.numThreads.Minimum = 1.0R
        Me.numThreads.Name = "numThreads"
        Me.numThreads.Size = New System.Drawing.Size(41, 21)
        'Me.numThreads.TabIndex = 98
        Me.numThreads.TabStop = False
        Me.TipProvider.SetTipText(Me.numThreads, "Number of parallel processes")
        '
        'bnCMD
        '
        Me.bnCMD.CausesValidation = False
        Me.tlpMain.SetColumnSpan(Me.bnCMD, 2)
        Me.bnCMD.Enabled = False
        Me.bnCMD.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnCMD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnCMD.Location = New System.Drawing.Point(743, 390)
        Me.bnCMD.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnCMD.Size = New System.Drawing.Size(84, 26)
        Me.bnCMD.Text = "   C&MD"
        Me.TipProvider.SetTipText(Me.bnCMD, "Show Command Line")
        '
        'bnAudioMediaInfo
        '
        Me.bnAudioMediaInfo.CausesValidation = False
        Me.bnAudioMediaInfo.Enabled = False
        Me.bnAudioMediaInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAudioMediaInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnAudioMediaInfo.Location = New System.Drawing.Point(653, 390)
        Me.bnAudioMediaInfo.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnAudioMediaInfo.Size = New System.Drawing.Size(84, 26)
        Me.bnAudioMediaInfo.Text = "   &Info"
        Me.TipProvider.SetTipText(Me.bnAudioMediaInfo, "Media Info")
        '
        'bnEdit
        '
        Me.bnEdit.CausesValidation = False
        Me.tlpMain.SetColumnSpan(Me.bnEdit, 2)
        Me.bnEdit.Enabled = False
        Me.bnEdit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnEdit.Location = New System.Drawing.Point(563, 390)
        Me.bnEdit.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnEdit.Size = New System.Drawing.Size(84, 26)
        Me.bnEdit.Text = "    &Edit..."
        Me.TipProvider.SetTipText(Me.bnEdit, "Edit Audio Profile for selection")
        '
        'bnConvert
        '
        Me.bnConvert.CausesValidation = False
        Me.bnConvert.Enabled = False
        Me.bnConvert.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnConvert.Location = New System.Drawing.Point(239, 390)
        Me.bnConvert.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnConvert.Size = New System.Drawing.Size(88, 26)
        Me.bnConvert.Text = "&Convert..."
        Me.bnConvert.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.bnConvert, "Convert Selection")
        '
        'bnRemove
        '
        Me.bnRemove.CausesValidation = False
        Me.bnRemove.Enabled = False
        Me.bnRemove.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnRemove.Location = New System.Drawing.Point(145, 390)
        Me.bnRemove.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnRemove.Size = New System.Drawing.Size(88, 26)
        Me.bnRemove.Text = "&Remove "
        Me.bnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.bnRemove, "Hold <Shift+Control> to delete from disk")
        '
        'bnMenuAudio
        '
        Me.bnMenuAudio.CausesValidation = False
        Me.bnMenuAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnMenuAudio.Location = New System.Drawing.Point(4, 390)
        Me.bnMenuAudio.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.bnMenuAudio.ShowMenuSymbol = True
        Me.bnMenuAudio.Size = New System.Drawing.Size(40, 26)
        Me.TipProvider.SetTipText(Me.bnMenuAudio, "Click to open menu")
        Me.bnMenuAudio.UseMnemonic = False
        '
        'laAC
        '
        Me.laAC.CausesValidation = False
        Me.tlpMain.SetColumnSpan(Me.laAC, 3)
        Me.laAC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.laAC.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0)
        Me.laAC.Location = New System.Drawing.Point(48, 365)
        Me.laAC.Margin = New System.Windows.Forms.Padding(0, 2, 0, 2)
        Me.laAC.Name = "laAC"
        Me.laAC.Size = New System.Drawing.Size(282, 20)
        'Me.laAC.TabIndex = 99
        Me.laAC.Text = "Please add or drag music files..."
        Me.laAC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.TipProvider.SetTipText(Me.laAC, "Double Click to Clean-Up")
        Me.laAC.UseMnemonic = False
        '
        'BnSort
        '
        Me.BnSort.CausesValidation = False
        Me.BnSort.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BnSort.FlatAppearance.BorderSize = 0
        Me.BnSort.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BnSort.ForeColor = System.Drawing.Color.FromArgb(0, 75, 255)
        Me.BnSort.Location = New System.Drawing.Point(4, 364)
        Me.BnSort.Margin = New System.Windows.Forms.Padding(4, 1, 4, 0)
        Me.BnSort.Name = "BnSort"
        Me.BnSort.Size = New System.Drawing.Size(40, 23)
        'Me.BnSort.TabIndex = 2
        Me.BnSort.Text = "&View"
        Me.TipProvider.SetTipText(Me.BnSort, "Auto-size columns mode")
        Me.BnSort.UseVisualStyleBackColor = True
        '
        'BnFindNext
        '
        Me.BnFindNext.CausesValidation = False
        Me.BnFindNext.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BnFindNext.Enabled = False
        Me.BnFindNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 120, 215)
        Me.BnFindNext.FlatAppearance.BorderSize = 0
        Me.BnFindNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BnFindNext.Location = New System.Drawing.Point(560, 364)
        Me.BnFindNext.Margin = New System.Windows.Forms.Padding(0, 1, 22, 0)
        Me.BnFindNext.Name = "BnFindNext"
        Me.BnFindNext.Padding = New System.Windows.Forms.Padding(2, 0, 0, 3)
        Me.BnFindNext.Size = New System.Drawing.Size(23, 23)
        'Me.BnFindNext.TabIndex = 4
        Me.TipProvider.SetTipText(Me.BnFindNext, "Find next <Enter>")
        Me.BnFindNext.UseMnemonic = False
        Me.BnFindNext.UseVisualStyleBackColor = True
        '
        'laThreads
        '
        Me.laThreads.BackColor = System.Drawing.SystemColors.Control
        Me.laThreads.CausesValidation = False
        Me.tlpMain.SetColumnSpan(Me.laThreads, 3)
        Me.laThreads.Dock = System.Windows.Forms.DockStyle.Fill
        Me.laThreads.Location = New System.Drawing.Point(605, 367)
        Me.laThreads.Margin = New System.Windows.Forms.Padding(0, 4, 0, 4)
        Me.laThreads.Name = "laThreads"
        Me.laThreads.Size = New System.Drawing.Size(180, 16)
        'Me.laThreads.TabIndex = 97
        Me.laThreads.Text = "Threads : "
        Me.laThreads.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.laThreads.UseMnemonic = False
        '
        'bnAdd
        '
        Me.bnAdd.CausesValidation = False
        Me.bnAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnAdd.Location = New System.Drawing.Point(51, 390)
        Me.bnAdd.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnAdd.Size = New System.Drawing.Size(88, 26)
        Me.bnAdd.Text = "    &Add..."
        '
        'bnPlay
        '
        Me.bnPlay.CausesValidation = False
        Me.bnPlay.Enabled = False
        Me.bnPlay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnPlay.Location = New System.Drawing.Point(473, 390)
        Me.bnPlay.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnPlay.Size = New System.Drawing.Size(84, 26)
        Me.bnPlay.Text = "   &Play"
        '
        'bnDown
        '
        Me.bnDown.CausesValidation = False
        Me.bnDown.Enabled = False
        Me.bnDown.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnDown.Location = New System.Drawing.Point(403, 390)
        Me.bnDown.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnDown.Size = New System.Drawing.Size(64, 26)
        Me.bnDown.Text = "&Down"
        Me.bnDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bnUp
        '
        Me.bnUp.CausesValidation = False
        Me.bnUp.Enabled = False
        Me.bnUp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bnUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bnUp.Location = New System.Drawing.Point(333, 390)
        Me.bnUp.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.bnUp.Size = New System.Drawing.Size(64, 26)
        Me.bnUp.Text = "  &Up"
        '
        'tlpMain
        '
        Me.tlpMain.CausesValidation = False
        Me.tlpMain.ColumnCount = 13
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.dgvAudio, 0, 0)
        Me.tlpMain.Controls.Add(Me.laAC, 1, 1)
        Me.tlpMain.Controls.Add(Me.laThreads, 8, 1)
        Me.tlpMain.Controls.Add(Me.numThreads, 11, 1)
        Me.tlpMain.Controls.Add(Me.bnMenuAudio, 0, 2)
        Me.tlpMain.Controls.Add(Me.bnAdd, 1, 2)
        Me.tlpMain.Controls.Add(Me.bnRemove, 2, 2)
        Me.tlpMain.Controls.Add(Me.bnConvert, 3, 2)
        Me.tlpMain.Controls.Add(Me.bnUp, 4, 2)
        Me.tlpMain.Controls.Add(Me.bnDown, 5, 2)
        Me.tlpMain.Controls.Add(Me.bnPlay, 6, 2)
        Me.tlpMain.Controls.Add(Me.bnEdit, 7, 2)
        Me.tlpMain.Controls.Add(Me.bnCMD, 10, 2)
        Me.tlpMain.Controls.Add(Me.BnSort, 0, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioMediaInfo, 9, 2)
        Me.tlpMain.Controls.Add(Me.TBFind, 4, 1)
        Me.tlpMain.Controls.Add(Me.BnFindNext, 7, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(3, 3)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.tlpMain.Size = New System.Drawing.Size(830, 419)
        'Me.tlpMain.TabIndex = 100
        '
        'dgvAudio
        '
        Me.dgvAudio.AllowDrop = True
        Me.dgvAudio.AllowUserToAddRows = False
        Me.dgvAudio.AllowUserToDeleteRows = False
        Me.dgvAudio.AllowUserToOrderColumns = True
        Me.dgvAudio.AllowUserToResizeRows = False
        Me.dgvAudio.CausesValidation = False
        Me.tlpMain.SetColumnSpan(Me.dgvAudio, 13)
        Me.dgvAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAudio.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvAudio.Location = New System.Drawing.Point(3, 3)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.ReadOnly = True
        Me.dgvAudio.RowHeadersWidth = 24
        Me.dgvAudio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAudio.ShowCellErrors = False
        Me.dgvAudio.ShowEditingIcon = False
        Me.dgvAudio.ShowRowErrors = False
        Me.dgvAudio.Size = New System.Drawing.Size(824, 357)
        Me.dgvAudio.StandardTab = True
        'Me.dgvAudio.TabIndex = 1
        Me.dgvAudio.VirtualMode = True
        '
        'TBFind
        '
        Me.TBFind.CausesValidation = False
        Me.TBFind.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower
        Me.tlpMain.SetColumnSpan(Me.TBFind, 3)
        Me.TBFind.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TBFind.ForeColor = System.Drawing.Color.Gray
        Me.TBFind.Location = New System.Drawing.Point(330, 364)
        Me.TBFind.Margin = New System.Windows.Forms.Padding(0, 1, 0, 0)
        Me.TBFind.MaxLength = 260
        Me.TBFind.Name = "TBFind"
        Me.TBFind.Size = New System.Drawing.Size(230, 23)
        'Me.TBFind.TabIndex = 3
        Me.TBFind.Text = " search... "
        Me.TBFind.WordWrap = False
        '
        'AudioConverterForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(836, 425)
        Me.Controls.Add(Me.tlpMain)
        'Me.DoubleBuffered = True'Overrided
        Me.KeyPreview = True
        Me.Name = "AudioConverterForm"
        'Me.Padding = New System.Windows.Forms.Padding(3)'Overrided
        Me.Text = "AudioConverter"
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        DirectCast(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Friend WithEvents CMS As ContextMenuStripEx
    Public Shared AudioConverterMode As Boolean
    Private Shared MaxThreads As Integer
    Private Shared OutPath As String
    Private Shared LastLogPath As String
    Private LogHeader As Boolean
    Private AudioCL As CircularList(Of AudioProfile)
    Private AudioSBL As SortableBindingList(Of AudioProfile)

    Private TBFindIdx As Integer = -1
    Private BuildFindT As Task(Of String())

    Private StatTextSB As StringBuilder
    Private IsStatusFlashing As Boolean
    Private CurrentType As Type
    Private SelectChangeES As Boolean
    Private LastKeyDown As Integer = -1
    Private ButtonAltImgDone As Boolean
    Private ButtonAltImageT As Task
    Private PopulateTask As Task
    Private PopulateTaskW As Task
    Private PopulateTaskS As Integer
    Private PopulateIter As Integer
    Private ReadOnly SLock As New Object
    Private ReadOnly PopulateWSW As New Stopwatch
    Private ReadOnly PopulateRSW As New Stopwatch
    Private ReadOnly SW1 As New Stopwatch
    Private ReadOnly SW2 As New Stopwatch
    Private AudioVideoFExts As HashSet(Of String)
    Private InvalidPathChHS As HashSet(Of Char)

    'Private DGVEventCount As Integer

    Private StatUpdateTask As Task
    Private ReadOnly StatUpdateTaskA As Action =
        Sub()
            Do While PopulateIter < 900 AndAlso AudioConverterMode
                Dim t As String = PopulateIter.ToInvStr & PopulateTask?.Status.ToString & PopulateWSW.ElapsedTicks / SWFreq & "msW" & PopulateTaskS.ToInvStr & "PTS" & PopulateRSW.ElapsedTicks / SWFreq & "msR|WPT" &
                PopulateTaskW?.Status.ToString & SW2.ElapsedTicks / SWFreq & "ms2|Sw" & SW1.ElapsedTicks / SWFreq & "MC" & MediaInfo.Cache.Count.ToInvStr & "ImgC" & ImageHelp.ImageCacheD.Count &
                "FTL" & BuildFindT?.Status.ToString & If(BuildFindT?.IsCompleted, BuildFindT.Result.Length.ToInvStr, "") & "FIdx" & TBFindIdx.ToInvStr ' & "cmsC:" & DGVEventCount.ToInvStr
                ' & StatTextSB.Length.ToInvStr & "VC" & DGVEventCount.ToInvStr & "SBL:"
                Me.BeginInvoke(Sub()
                                   'DGVEventCount = CMS.Items.Count
                                   Me.Text = t
                               End Sub)
                'If StatTextSB.Length > 38 Then Me.BeginInvoke(Sub() Me.BackColor = Color.HotPink)
                Thread.Sleep(135)
            Loop
            If AudioConverterMode Then Me.BeginInvoke(Sub() Me.Text = "AudioConverter")
        End Sub

    Public Sub New()
        MyBase.New()
        'Color.FromArgb(0, 120, 215) Color.FromArgb(229, 241, 251) Color.FromArgb(204, 228, 247) SystemColors-Bluish
        'If Debugger.IsAttached Then Control.CheckForIllegalCrossThreadCalls = False
        Dim _varInT = Task.Run(Sub()
                                   AudioCL = New CircularList(Of AudioProfile)
                                   AudioSBL = New SortableBindingList(Of AudioProfile)(AudioCL) With {.CheckConsistency = False, .SortOnChange = False, .RaiseListChangedEvents = True, .AllowNew = False}
                                   StatTextSB = New StringBuilder(40)
                                   AudioVideoFExts = New HashSet(Of String)(FileTypes.Audio.ConcatA(FileTypes.VideoAudio), StringComparer.Ordinal)
                                   InvalidPathChHS = New HashSet(Of Char)({":"c, "\"c, "/"c, "?"c, "^"c, "."c})
                               End Sub)
        Dim imagesArrTask = Task.Run(Function() {ImageHelp.GetImageC(Symbol.fa_arrow_right),
                                                 ImageHelp.GetImageC(Symbol.Add),
                                                 ImageHelp.GetImageC(Symbol.Remove),
                                                 ImageHelp.GetImageC(Symbol.Play),
                                                 ImageHelp.GetImageC(Symbol.Up),
                                                 ImageHelp.GetImageC(Symbol.Down),
                                                 ImageHelp.GetImageC(Symbol.Repair),
                                                 ImageHelp.GetImageC(Symbol.MusicInfo),
                                                 ImageHelp.GetImageC(Symbol.Info),
                                                 ImageHelp.GetImageC(Symbol.CommandPrompt),
                                                 ImageHelp.GetImageC(Symbol.SelectAll),
                                                 ImageHelp.GetImageC(Symbol.Clear),
                                                 ImageHelp.GetImageC(Symbol.ShowResults), 'BulletedList
                                                 ImageHelp.GetImageC(Symbol.FileExplorer),
                                                 ImageHelp.GetImageC(Symbol.FileExplorerApp),
                                                 ImageHelp.GetImageC(Symbol.fa_folder_open_o),
                                                 ImageHelp.GetImageC(Symbol.Save)})
        InitializeComponent()
        Me.tlpMain.SuspendLayout()
        DirectCast(Me.dgvAudio, ISupportInitialize).BeginInit()
        Me.SuspendLayout()

        Icon = g.Icon
        RestoreClientSize(54.0F, 29.0F)
        ' MinimumSize = New Size(16 * 22, 16 * 12) 'FontHeight '16' Overrided DefMinSz ! Test if working!!
        If MaxThreads = 0 Then MaxThreads = s.ParallelProcsNum
        numThreads.Value = MaxThreads
        'UpdateDefaultButton()
        bnAdd.Select()

        With dgvAudio 'Def Size: {Width = 836 Height = 357}
            'GetType(DataGridViewEx).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, dgvAudio, New Object() {True}) 'overrided in Parent Class
            .DefaultCellStyle.DataSourceNullValue = Nothing
            .DefaultCellStyle.FormatProvider = InvCult
            .ColumnHeadersDefaultCellStyle.Font = New Font(Me.Font, FontStyle.Bold)
            .RowTemplate.Height = 20 'FontHeight[16] + 4 'or *1.25 ?, AutoResize=20, def=22
            '[InDesigner-InitSub]-dgvAudio.RowHeadersWidth = 24 ' (0)=42 +6 per number char
            .AutoGenerateColumns = False
            .Columns.AddRange({New DataGridViewTextBoxColumn With {.SortMode = DataGridViewColumnSortMode.NotSortable, .HeaderText = "No.", .FillWeight = 10, .MinimumWidth = 28, .[ReadOnly] = True},
                      New DataGridViewTextBoxColumn With {.DataPropertyName = "Name", .HeaderText = "Profile", .FillWeight = 100, .MinimumWidth = 60, .[ReadOnly] = True},
                      New DataGridViewTextBoxColumn With {.DataPropertyName = "DisplayName", .HeaderText = "Track", .FillWeight = 400, .MinimumWidth = 46, .[ReadOnly] = True},
                      New DataGridViewTextBoxColumn With {.DataPropertyName = "File", .HeaderText = "Full Path", .FillWeight = 500, .MinimumWidth = 74, .[ReadOnly] = True}})
            'col(0).ValueType = GetType(Integer) ; col.MinimumWidth = col.HeaderText.Length * 8 + 5 <no=32-noTTip> ; 'col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End With

        CMS = New ContextMenuStripEx(components)
        '<.AddClickAction> bnMenuAudio.ClickAction = Sub() UpdateCMS(Me.bnMenuAudio, New System.ComponentModel.CancelEventArgs With {.Cancel = False})
        CMS.SuspendLayout()
        bnMenuAudio.ContextMenuStrip = CMS

        If Not imagesArrTask.IsCompleted Then MsgInfo(imagesArrTask.Status.ToString, "ImageA Task not completed yet !!!") 'Debug !!!!!!!

        Dim imgsArr = imagesArrTask.Result
        BnFindNext.Image = imgsArr(0)
        bnAdd.Image = imgsArr(1)
        bnRemove.Image = imgsArr(2)
        bnPlay.Image = imgsArr(3)
        bnUp.Image = imgsArr(4)
        bnDown.Image = imgsArr(5)
        bnEdit.Image = imgsArr(6)
        bnConvert.Image = imgsArr(7)
        bnAudioMediaInfo.Image = imgsArr(8)
        bnCMD.Image = imgsArr(9)

        CMS.Items.AddRange({
            New ActionMenuItem("Select all  <Ctrl+A>", Sub() dgvAudio.SelectAll(), imgsArr(10)),
            New ActionMenuItem("Remove all", Sub()
                                                 PopulateTaskS = -1
                                                 RemoveHandlersDGV(True)
                                                 AudioCL.Reset()
                                                 AudioSBL.Clear()
                                                 AudioSBL.InnerListChanged()
                                                 AudioSBL.ResetBindings()
                                                 dgvAudio.Rows.Clear()
                                                 TBFind.Clear()
                                                 TBFind_Leave(Nothing, Nothing)
                                                 CurrentType = Nothing
                                                 UpdateControls()
                                                 GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
                                                 GC.Collect(2, GCCollectionMode.Forced, True, True)
                                                 GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
                                             End Sub, imgsArr(11)),
            New ActionMenuItem("Remove duplicates", Sub()
                                                        PopulateTaskS = -1
                                                        RemoveHandlersDGV(True)
                                                        Dim occ As Integer = dgvAudio.CurrentCellAddress.X
                                                        Dim ocF As Long = AudioSBL.Item(dgvAudio.CurrentCellAddress.Y).FileKeyHashValue
                                                        Dim uAR = AudioCL.Distinct(New AudioProfileHComparer).ToArray
                                                        If AudioCL.Count <= uAR.Length Then
                                                            MsgInfo("No duplicates found.")
                                                        ElseIf MsgQuestion(AudioCL.Count - uAR.Length & " duplicated file(s) found." & BR & "Remove from Grid View ?") = DialogResult.OK Then
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
                                                            If crI >= 0 Then dgvAudio.CurrentCell = dgvAudio(occ, crI)
                                                            AutoSizeColumns()
                                                        End If
                                                        PopulateCache(4000000)
                                                        UpdateControls()
                                                        AddHandlersDGV()
                                                    End Sub, imgsArr(12), "Removes duplicated files from list"),
            New ActionMenuItem("Show Source File", Sub()
                                                       dgvAudio.FirstDisplayedScrollingRowIndex = dgvAudio.CurrentCellAddress.Y
                                                       g.SelectFileWithExplorer(AudioSBL.Item(dgvAudio.CurrentCellAddress.Y).FileValue)
                                                   End Sub, imgsArr(13), "Open the source file in File Explorer."),
            New ActionMenuItem("Show Ouput Folder", Sub() g.SelectFileWithExplorer(OutPath), imgsArr(14), "Open output folder in File Explorer."),
            New ActionMenuItem("Show LOG", Sub()
                                               SaveConverterLog(LastLogPath)
                                               If FileExists(LastLogPath) Then
                                                   g.SelectFileWithExplorer(LastLogPath)
                                               ElseIf FileExists(p.Log.GetPath) Then
                                                   g.SelectFileWithExplorer(p.Log.GetPath)
                                               End If
                                           End Sub, imgsArr(15), "Open current log in File Explorer"),
            New ActionMenuItem("Save LOG...", Sub() SaveConverterLog(UseDialog:=True), imgsArr(16)),
            New ToolStripSeparator,
            New ActionMenuItem("qaac Help", Sub() Package.qaac.ShowHelp()),
            New ActionMenuItem("qaac Formats", Sub() MsgInfo(ProcessHelp.GetConsoleOutput(Package.qaac.Path, "--formats"))),
            New ActionMenuItem("Opus Help", Sub() Package.OpusEnc.ShowHelp()),
            New ActionMenuItem("ffmpeg Help", Sub() Package.ffmpeg.ShowHelp()),
            New ActionMenuItem("eac3to Help", Sub() g.ShellExecute("http://en.wikibooks.org/wiki/Eac3to"))})
        CMS.ResumeLayout(False)
        numThreads.ResumeLayout(False)
        numThreads.PerformLayout()

        FontHeight = -3 'Supress FormBase mybase.OnLoad Hack
        MyBase.OnLoad(Nothing) 'TODO: Test This !!!!!!!!!!
        FontHeight = 16

        If StatTextSB Is Nothing Then 'StatTextSB ' Better-Safer ???
            'MsgInfo(AudioSBL?.ToString & _varInT.Status.ToString, "New VarInit Task not completed yet !!!", 320) 'Debug !!!!!!!
            _varInT.Wait()
        End If
        'PopulateRSW.Restart()
        dgvAudio.DataSource = AudioSBL
        'PopulateRSW.Stop()
        'MsgInfo(CStr(PopulateRSW.ElapsedTicks / SWFreq), "DGV DatasourceSet ms")

        Me.tlpMain.ResumeLayout()
        DirectCast(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout()
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        AudioConverterMode = True
        FontHeight = -2 'Supress FormBase overRide OnLoad
        MyBase.OnLoad(args)
        FontHeight = 16
        dgvAudio.Columns.Item(3).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        'Me.tlpMain.ResumeLayout()
        'DirectCast(Me.dgvAudio, ISupportInitialize).EndInit()
        'Me.ResumeLayout()
        Dim _llTT = Task.Run(Sub() 'Back to OnShow???del-90
                                 AddHandler CMS.Opening, AddressOf UpdateCMS
                                 AddHandler TBFind.TextChanged, AddressOf TBFind_TextChanged
                                 Thread.Sleep(105) '60-105
                                 GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce 'Debug
                                 'GC.Collect(2, GCCollectionMode.Forced, True, True)
                                 'GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
                                 If IsHandleCreated Then Me.BeginInvoke(Sub() If g.MainForm.Visible Then g.MainForm.Hide())
                             End Sub)
        '_llTT.ContinueWith(Sub() If _llTT.Exception IsNot Nothing Then _llTT.Exception.ToString())
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        PopulateTaskS = -2
        PopulateIter = 9999
        TBFindIdx = -1
        AudioConverterMode = False
        AudioCL.Reset()
        AudioSBL.InnerListChanged() 'This raises events on DGV
        dgvAudio.DataSource = Nothing 'Needed ?
        'AudioSBL.Clear() 'This is propably not needed ???
        'dgvAudio.Columns.Clear() 'This is propably not needed ???
        MyBase.OnFormClosing(e)
        If Not g.MainForm.Visible Then g.MainForm.Show()
    End Sub
    Protected Overrides ReadOnly Property DefaultMinimumSize As Size ' MinimumSize = New Size(16 * 22, 16 * 12) 'FontHeight '16
        <CompilerServices.MethodImpl(AggrInlin)> Get 'Test this:
            Return New Size(16 * 22, 16 * 12) 'FontHeight '16 
        End Get
    End Property
    'Protected Overrides ReadOnly Property DefaultSize As Size
    '    <CompilerServices.MethodImpl(AggressiveInlin)> Get
    '        Return New System.Drawing.Size(836, 425) 'TODO: Find Size !!!
    '    End Get
    'End Property
    Protected Overrides ReadOnly Property DefaultPadding As Padding
        <CompilerServices.MethodImpl(AggrInlin)> Get
            Return New Padding(18) '3 Test This Is working at all
        End Get
    End Property

    Protected Overrides Property DoubleBuffered As Boolean
        <CompilerServices.MethodImpl(AggrInlin)> Get
            Return True
        End Get
        <CompilerServices.MethodImpl(AggrInlin)> Set(value As Boolean)
            MyBase.DoubleBuffered = value
        End Set
    End Property
    Private Sub UpdateCMS(sender As Object, e As CancelEventArgs)
        Dim isR = AudioCL.Count > 0
        Dim isCR = isR AndAlso dgvAudio.CurrentCellAddress.Y >= 0
        Dim logNotEmpty As Boolean = Not Log.IsEmpty
        Dim cItm = CMS.Items
        cItm(0).Enabled = isR
        cItm(1).Enabled = isR
        cItm(2).Enabled = isCR
        cItm(3).Enabled = isCR AndAlso File.Exists(AudioSBL.Item(dgvAudio.CurrentCellAddress.Y).FileValue)
        cItm(4).Enabled = OutPath.NotNullOrEmptyS AndAlso Directory.Exists(OutPath)
        cItm(5).Enabled = logNotEmpty
        cItm(6).Enabled = logNotEmpty
        LastKeyDown = -1
        SelectChangeES = False
        ButtonAltImage(False)
    End Sub

    Private Sub UpdateControls(Optional selRowsCount As Integer = -1, Optional currRowI As Integer = -1)
        ' SW1.Restart()
        Dim txt As String
        Dim rC As Integer = AudioCL.Count
        If selRowsCount = -1 Then selRowsCount = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        If currRowI = -1 Then currRowI = dgvAudio.CurrentCellAddress.Y
        StatTextSB.Length = 0

        If rC > 0 Then
            StatTextSB.Append("Pos: ").Append((currRowI + 1).ToInvStr).Append(" Sel: ").Append(selRowsCount.ToInvStr).Append(" / Tot: ").Append(rC.ToInvStr)
            txt = StatTextSB.ToString
        Else
            txt = "Please add or drag music files..."
        End If

        Me.BeginInvoke(Sub()
                           'SW2.Restart()
                           'laAC.Text = "Pos: " & (currRowI + 1).ToInvStr & " | Sel: " & (selRowsCount).ToInvStr & " / Tot: " & (rC).ToInvStr
                           laAC.Text = txt
                           numThreads.Enabled = rC > 0
                           bnRemove.Enabled = selRowsCount > 0
                           bnConvert.Enabled = selRowsCount > 0
                           bnUp.Enabled = selRowsCount = 1 AndAlso currRowI > 0
                           bnDown.Enabled = selRowsCount = 1 AndAlso currRowI < rC - 1
                           bnPlay.Enabled = currRowI > -1
                           bnEdit.Enabled = selRowsCount > 0
                           bnAudioMediaInfo.Enabled = currRowI > -1
                           bnCMD.Enabled = currRowI > -1
                           ' SW2.Stop()
                       End Sub)
        '   SW1.Stop()
    End Sub

    <CompilerServices.MethodImpl(AggrInlin)> Private Sub StatusText(InfoText As String)
        laAC.Text = InfoText
        laAC.Refresh()
    End Sub

    Private Sub FlashingText(InfoText As String, Optional Count As Integer = 10, Optional Delay As Integer = 120)
        If IsStatusFlashing Then Exit Sub
        IsStatusFlashing = True

        For n = 1 To Count
            If AudioConverterMode Then
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

        IsStatusFlashing = False
        If AudioConverterMode Then
            BeginInvoke(Sub()
                            laAC.ForeColor = Color.Black
                            UpdateControls()  'laAC.Text = StatTextSB.ToString
                        End Sub)
        End If
    End Sub

    Private Sub AutoSizeColumns(Optional AllCells As Boolean = False)
        Dim miCn As Integer = MediaInfo.Cache.Count
        If AudioCL.Count > 1000 OrElse (AudioCL.Count - miCn > 100) Then StatusText("Auto Resizing Columns...")
        With dgvAudio
            .Columns.Item(0).MinimumWidth = Math.Max(28, CInt(14 + 6 * Fix(Math.Log10(AudioCL.Count + 1))))
            .ColumnHeadersHeight = 23
            .RowHeadersWidth = 24

            If .AutoSizeColumnsMode <> DataGridViewAutoSizeColumnsMode.Fill Then
                PopulateTaskS = -1
                Dim allc As Boolean = AllCells OrElse AudioCL.Count < 200 OrElse (AudioCL.Count - miCn < 500)
                If allc Then
                    UseWaitCursor = True '???
                    Cursor = Cursors.WaitCursor 'This or next one is OK!
                    Cursor.Current = Cursors.WaitCursor
                    If AudioCL.Count - miCn > 500 Then Me.Enabled = False
                    Application.DoEvents() 'Needed???
                    'Application.UseWaitCursor = True 'Not Working
                    'PostMessage(Me.Handle, &H20, Me.Handle, New IntPtr(1)) 'Send WM_SETCURSOR'Not Working
                End If
                .AutoResizeColumns(If(allc, DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader, DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader))
                If allc Then
                    Cursor = Cursors.Default
                    Cursor.Current = Cursors.Default
                    Me.Enabled = True
                    UseWaitCursor = False
                    'Application.UseWaitCursor = False
                End If
            End If
        End With
    End Sub

    '<System.Runtime.InteropServices.DllImport("user32.dll")> Shared Function PostMessage(hwnd As IntPtr, wMsg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr'Not Working
    'End Function
    <CompilerServices.MethodImpl(AggrInlin)>
    Private Sub dgvAudio_SelectionChanged(sender As Object, e As EventArgs)
        If SelectChangeES Then Exit Sub
        PopulateTaskS = 0
        'Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Task.Run(Sub()
                     UpdateControls()
                     PopulateCache(4000000)
                 End Sub)
    End Sub

    <CompilerServices.MethodImpl(AggrInlin)>
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
                         UpdateControls(selRowsCount:=1)
                         SelectChangeES = False
                     End Sub)
        End If
    End Sub

    <CompilerServices.MethodImpl(AggrInlin)>
    Private Sub dgvAudio_CellValueNeeded(sender As Object, e As DataGridViewCellValueEventArgs) Handles dgvAudio.CellValueNeeded
        '  If e.ColumnIndex = 0 Then 'DGVEventCount += 1
        e.Value = (e.RowIndex + 1).ToString(InvCult)
    End Sub

    <CompilerServices.MethodImpl(AggrInlin)>
    Private Sub dgvAudio_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvAudio.CellFormatting
        'If e.CellStyle.DataSourceNullValue IsNot Nothing OrElse e.CellStyle.FormatProvider IsNot InvCult Then Console.Beep(3700, 10) 'DGVEventCount += 1
        e.FormattingApplied = True
    End Sub

    Private Function BuildFind() As String()
        If AudioCL.Count > 0 Then
            SW2.Restart()
            PopulateTaskS = 0
            'Dim arr = AudioSBL.ToArray
            Dim isCF = CInt(Fix(AudioCL.Count * 0.9)) <= MediaInfo.Cache.Count + FindDuplicates(False) '90% cache fill
            Dim retA(AudioCL.Count - 1) As String
            Parallel.For(0, retA.Length, Sub(i)
                                             With AudioSBL(i)
                                                 If isCF Then
                                                     Dim dn As String = .DisplayNameValue
                                                     If dn IsNot Nothing Then
                                                         Dim si = dn.IndexOf(" (", StringComparison.Ordinal)
                                                         dn = If(si >= 0, dn.Substring(0, si), "")
                                                         retA(i) = (.FileValue & .Name & dn).ToLower(InvCult)
                                                     Else
                                                         retA(i) = .FileValue.ToLower(InvCult)
                                                     End If
                                                 Else
                                                     retA(i) = .FileValue.ToLower(InvCult)
                                                 End If
                                             End With
                                         End Sub)
            'PopulateCache(4000000)
            SW2.Stop()
            Return retA
        End If
        Return {}
    End Function
    Private Sub FindTextFile(FindString As String, SelRows As Boolean)
        SW1.Restart()
        Dim rC = AudioCL.Count
        If rC > 0 Then
            Dim fString As String() = BuildFindT?.Result
            If fString Is Nothing OrElse fString.Length <> rC Then
                Console.Beep(8500, 220) 'debug
                fString = BuildFind()
            End If
Again: 'Debug StopWatch Use
            Dim findC As Integer

            If Not SelRows Then
                If TBFindIdx > 0 AndAlso TBFindIdx < rC Then
                    Dim found As Boolean
                    For i = TBFindIdx To rC - 1
                        If fString(i).Contains(FindString) Then
                            TBFindIdx = i
                            found = True
                            Exit For
                        End If
                    Next i

                    If Not found Then
                        TBFindIdx = -1
                        GoTo Again 'FindTextFile()
                    End If
                Else
                    For i = 0 To rC - 1
                        If fString(i).Contains(FindString) Then
                            TBFindIdx = i
                            findC += 1
                            Exit For
                        End If
                    Next i
                End If
                If TBFindIdx > -1 AndAlso TBFindIdx < rC Then
                    If findC > 0 Then
                        Task.Run(Sub()
                                     Dim tbfi = TBFindIdx
                                     SW2.Restart()
                                     If tbfi < rC - 1 Then
                                         Parallel.For(tbfi + 1, rC, New ParallelOptions With {.MaxDegreeOfParallelism = Math.Max(CPUsC \ 2, 1)},
                                                           Sub(i) If fString(i).Contains(FindString) Then Interlocked.Increment(findC))
                                     End If
                                     'fc = fString.CountF(Function(s) s.Contains(FindString))
                                     Dim ctext = findC.ToInvStr & " file" & If(findC > 1, "s", "") & " found.           Threads : "
                                     If tbfi = TBFindIdx Then BeginInvoke(Sub() laThreads.Text = ctext)
                                     SW2.Stop()
                                 End Sub)
                    End If

                    dgvAudio.CurrentCell = dgvAudio(dgvAudio.CurrentCellAddress.X, TBFindIdx) 'Or3
                    'dgvAudio.FirstDisplayedScrollingRowIndex = TBFindIdx
                    BnFindNext.Enabled = True
                    AcceptButton = BnFindNext
                Else
                    Task.Run(Sub() FlashingText("Not Found !", 4, 120))
                    AcceptButton = Nothing
                    BnFindNext.Enabled = False
                    laThreads.Text = "Threads : "
                End If

            Else
                RemoveHandlersDGV()
                dgvAudio.ClearSelection()
                Dim ff As Integer = Integer.MaxValue
                For i = 0 To rC - 1
                    If fString(i).Contains(FindString) Then
                        If ff > i Then
                            ff = i
                            dgvAudio.CurrentCell = dgvAudio(dgvAudio.CurrentCellAddress.X, ff)
                        End If
                        dgvAudio.Rows(i).Selected = True
                        'TBFindIdx = i
                        findC += 1
                    End If
                Next i
                AddHandlersDGV()
                If findC > 0 Then
                    laThreads.Text = findC.ToInvStr & " file" & If(findC > 1, "s", "") & " found.           Threads : "
                    UpdateControls(findC, ff)
                Else
                    Task.Run(Sub() FlashingText("Not Found !", 4, 120))
                    AcceptButton = Nothing
                    BnFindNext.Enabled = False
                    laThreads.Text = "Threads : "
                    UpdateControls()
                End If
            End If
            SW1.Stop()
        End If
    End Sub
    Private Sub TBFind_TextChanged(sender As Object, e As EventArgs)
        If TBFindIdx > -2 AndAlso AudioCL.Count > 0 AndAlso TBFind.TextLength > 0 Then
            TBFindIdx = -1
            FindTextFile(TBFind.Text, False)
        Else
            AcceptButton = Nothing
            BnFindNext.Enabled = False
            laThreads.Text = "Threads : "
        End If
    End Sub

    Private Sub TBFind_Enter(sender As Object, e As EventArgs) Handles TBFind.Enter
        BuildFindT = Task.Run(Function() BuildFind())
        If String.Equals(TBFind.Text, " search... ") Then
            TBFindIdx = -2
            TBFind.ForeColor = Color.Black
            TBFind.Clear()
            TBFindIdx = -1
        ElseIf TBFind.TextLength > 0 Then
            BeginInvoke(Sub() TBFind.SelectAll())
            BnFindNext.Enabled = True
            AcceptButton = BnFindNext
        End If
    End Sub

    Private Sub TBFind_Leave(sender As Object, e As EventArgs) Handles TBFind.Leave
        If TBFind.TextLength = 0 Then
            TBFindIdx = -2
            TBFind.ForeColor = Color.Gray
            TBFind.Text = " search... "
            TBFindIdx = -1
        End If

        If ActiveControl IsNot BnFindNext Then
            AcceptButton = Nothing
            BnFindNext.Enabled = False
            BuildFindT = Nothing
            PopulateCache(4000000)
        End If
    End Sub

    Private Sub BnFindNext_Click(sender As Object, e As EventArgs) Handles BnFindNext.Click
        If TBFindIdx >= 0 AndAlso TBFind.TextLength > 0 Then
            TBFindIdx += 1
            FindTextFile(TBFind.Text, My.Computer.Keyboard.ShiftKeyDown OrElse MouseButtons = MouseButtons.Right)
        End If
    End Sub

    Private Sub BnFindNext_Leave(sender As Object, e As EventArgs) Handles BnFindNext.Leave
        If ActiveControl IsNot TBFind Then
            AcceptButton = Nothing
            BnFindNext.Enabled = False
            BuildFindT = Nothing
        End If
    End Sub

    'Private Sub dgvAudio_RowUnshared(sender As Object, e As DataGridViewRowEventArgs) Handles dgvAudio.RowUnshared
    '    'laThreads.Text = "UnShI:" & e.Row.Index() + 1
    '    'laThreads.Refresh()
    '    DGVEventCount += 1
    'End Sub
    'Private Sub dgvAudio_Layout(sender As Object, e As LayoutEventArgs) Handles dgvAudio.Layout
    '    Log.Write("Layout Events:", e.AffectedComponent?.ToString & " <-Component|Ctrl-> " & e.AffectedControl?.ToString & " Property: " & e.AffectedProperty?.ToString)
    '    Text = "Layout Events:" & e.AffectedComponent?.ToString & " <-Component|Ctrl-> " & e.AffectedControl?.ToString & " Property: " & e.AffectedProperty?.ToString
    'End Sub
    'Private Sub dgvAudio_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvAudio.DataError
    '    Task.Run(Sub()
    '                 Log.Write("DGV DataErrorException:cr " & e.ColumnIndex & e.RowIndex, e.Exception.ToString & e.ToString & e.Context.ToString)
    '                 Console.Beep(6000, 30)
    '             End Sub)
    '    e.Cancel = True
    '    e.ThrowException = False
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

    Private Function FindDuplicates(SelectedOnly As Boolean, Optional SelRowC As Integer = 0) As Integer
        Dim cUn As Integer

        If SelectedOnly Then
            If SelRowC <= 0 Then SelRowC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
            If SelRowC < 1 Then Return 0
            Dim uhs As New HashSet(Of Long)(SelRowC)
            For r = 0 To AudioCL.Count - 1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    uhs.Add(AudioSBL.Item(r).FileKeyHashValue)
                End If
            Next r
            cUn = uhs.Count
        Else
            SelRowC = AudioCL.Count
            If SelRowC < 1 Then Return 0
            Dim uhs As New HashSet(Of Long)(SelRowC)
            For r = 0 To AudioCL.Count - 1
                uhs.Add(AudioCL.Item(r).FileKeyHashValue)
            Next r
            cUn = uhs.Count
        End If

        Return SelRowC - cUn
    End Function

    Private Function RelativeSubDirRecursive(fPath As String, Optional SubDepth As Integer = 1) As String
        If SubDepth < 1 Then Return ""

        If fPath.Length <= 3 Then
            'For c = 0 To InvalidPathCh.Length - 1
            For c = fPath.Length - 1 To 0 Step -1
                If InvalidPathChHS.Contains(fPath(c)) Then fPath = fPath.Replace(fPath(c), "")
                'fPath = fPath.Replace(InvalidPathCh(c), "")
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
                td.AddButton(i.ToInvStr, i)
            Next
            td.SelectedValue = -1
            subDepth = td.Show
            If subDepth < 0 Then Exit Sub
        End Using

        If MsgQuestion("Confirm to convert selected: " & srC & BR & "To path: " & OutPath & BR & "Sub Dirs: " & subDepth) = DialogResult.OK AndAlso DirExists(OutPath) Then
            Dim StartTime = Date.Now
            StatusText("Converting...")

            If Not LogHeader Then
                LogHeader = True
                Log.WriteEnvironment()
                Log.WriteHeader("Audio Converter")
            End If
            Log.WriteHeader("Audio Converter queue started: " & Date.Now.ToString)

            Dim actions As New List(Of Action)(srC)
            For r = 0 To AudioCL.Count - 1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    Dim ap As AudioProfile = AudioSBL.Item(r)
                    Dim fi = New FileInfo(ap.FileValue)
                    If fi.Exists AndAlso fi.Length > 10 Then
                        actions.Add(Sub() EncodeAudio(ap, subDepth))
                    End If

                End If
            Next r

            Try
                If s.PreventStandby Then PowerRequest.SuppressStandby()
                ProcController.BlockActivation = True
                Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = MaxThreads}, actions.ToArray) 'ToDO : Use ParellelFor instead !!!

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
            Task.Delay(1000).Wait()
            MainForm.Hide()
            Visible = True
            WindowState = FormWindowState.Normal
            Activate()
            dgvAudio.Select()
            dgvAudio.Refresh()
            Refresh()

            Dim OKCount As Integer

            For r = 0 To AudioCL.Count - 1
                If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                    Dim ap As AudioProfile = AudioSBL.Item(r)
                    Dim outFile As String = OutPath & RelativeSubDirRecursive(ap.FileValue.Dir, subDepth) & ap.GetOutputFile.FileName() 'ap.file.filename
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
                                        vftExts = New HashSet(Of String)(FileTypes.VideoAudio, (StringComparer.Ordinal))
                                        Array.Sort(Files)
                                        Parallel.For(0, fC, New ParallelOptions With {.MaxDegreeOfParallelism = Math.Max(CPUsC - 2, 1)}, 'Or no ParOpt at all???
                                                     Sub(i)
                                                         Dim fI As String = Files(i)
                                                         Dim fLen As Integer = fI.Length
                                                         If fLen > 5 Then
                                                             fkha(i) = ((fI.GetHashCode + 2147483648L) << 16) + fLen  '-->Fast Keyhash
                                                             fExts(i) = fI.Ext
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
            Exit Sub
        End If

        Dim pSelB As New SelectionBox(Of AudioProfile) With {.Title = "Please select Audio Profile"}

        If p.Audio0 IsNot Nothing AndAlso p.Audio0.IsGapOrBatchProfile AndAlso (CurrentType Is Nothing OrElse p.Audio0.GetType Is CurrentType) Then
            pSelB.AddItem("Current Project 1: " & p.Audio0.ToString, p.Audio0)
        End If
        If p.Audio1 IsNot Nothing AndAlso p.Audio1.IsGapOrBatchProfile AndAlso (CurrentType Is Nothing OrElse p.Audio1.GetType Is CurrentType) Then
            pSelB.AddItem("Current Project 2: " & p.Audio1.ToString, p.Audio1)
        End If
        For Each aPr In s.AudioProfiles
            If aPr IsNot Nothing AndAlso aPr.IsGapOrBatchProfile AndAlso (CurrentType Is Nothing OrElse aPr.GetType Is CurrentType) Then
                pSelB.AddItem(aPr)
            End If
        Next aPr

        'SelectionBoxForm.StartPosition = FormStartPosition.CenterParent      'Drag Drop  out of center
        If pSelB.Show <> DialogResult.OK OrElse pSelB.Items.Count < 1 Then
            PopulateCache(4000000)
            Exit Sub
        End If

        SW1.Stop()
        Dim st1 = If(uKeysCountPT.IsCompleted, SW1.ElapsedTicks / SWFreq, -1.0R)
        Dim st22 As Double
        SW1.Restart()

        Dim clnT = Task.Run(Sub()
                                SW2.Restart()
                                RemoveHandlersDGV()
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

        Dim ap As AudioProfile = pSelB.SelectedValue.DeepClone
        ap.FileValue = Nothing
        ap.FileKeyHashValue = MediaInfo.KeyDefault
        ap.DisplayNameValue = Nothing
        ap.SourceSamplingRateValue = 0
        ap.Stream = Nothing
        ap.StreamName = ""
        ap.Streams.Clear()
        ap.Delay = 0
        'If ap.Language Is Nothing Then ap.Language = New Language 'or Language.CurrentCulture ???

        If ap.IsGapProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)
            gap.Name = ap.DefaultName
            gap.DefaultnameValue = Nothing
            gap.SourceDepth = 0
            gap.ChannelsValue = 0
            If gap.Params.Codec <> AudioCodec.DTS Then ap.ExtractDTSCore = False
            CurrentType = GetType(GUIAudioProfile)
        Else
            CurrentType = GetType(BatchAudioProfile)
        End If

        Dim apTS(fC - 1) As AudioProfile
        Dim dummyStream As New AudioStream
        Dim nullE As Integer

        Try
            Dim ukC As Integer = uKeysCountPT.Result
            If ukC <> fC Then
                PopulateCache(4000000)
ExitS:          MsgInfo("File Error/Key Collision! " & ukC & "|" & fC, uKeysCountPT.Exception?.ToString)
                UpdateControls()
                Exit Sub
            End If
        Catch
            GoTo ExitS
        End Try

        SW1.Stop()
        Dim st2 = SW1.ElapsedTicks / SWFreq
        SW1.Restart()
        'Dim clth = AudioCL.AsThreadSafe

        Parallel.For(0, fC, 'New ParallelOptions With {.MaxDegreeOfParallelism = Math.Max(CPUsC \ 2, 1)},
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
                                                 Log.WriteEnvironment()
                                                 Log.WriteHeader("Audio Converter")
                                             End If
                                         End SyncLock
                                     End If
                                     Interlocked.Increment(nullE)
                                     Log.WriteLine("-->Audio stream not found, skipping: " & BR & apN.FileValue)
                                     Return
                                 End If

                                 Dim autoStream As Boolean
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

        Try
            clnT.Wait()
        Catch ex As AggregateException
            MsgInfo("ClearTaskExcept!", ex.ToString)
        End Try

        If apBack?.Length > 0 Then AudioCL.AddRange(apBack)
        If nullE <= 0 Then AudioCL.AddRange(apTS) Else AudioCL.AddRange(apTS.WhereF(Function(itm As AudioProfile) itm IsNot Nothing))
        AudioSBL.InnerListChanged()
        ' If nullE = 0 Then AudioSBl.AddRange(apTS) Else AudioSBL.AddRange(apTS.Wheref(Function(itm As AudioProfile) itm IsNot Nothing)) 
        AudioSBL.RaiseListChangedEvents = True
        AudioSBL.ResetBindings()
        Dim nrC = AudioCL.Count

        If nrC > 0 Then
            dgvAudio.CurrentCell = dgvAudio.Item(occ, nrC - 1)
            dgvAudio.Rows.Item(If(rC > 0, rC - 1, 0)).Selected = True
            If nrC > 100 Then dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            AutoSizeColumns()
            PopulateIter = 1
            PopulateCache(4000000)
        End If

        SW1.Stop()
        Dim st4 = SW1.ElapsedTicks / SWFreq

        UpdateControls(selRowsCount:=If(nrC > 1, 2, 1))
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
                     FlashingText(dc.ToInvStr & " duplicated files found !", 12, 135)
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

    Private Sub PopulateCache(Delay As Integer)
        Select Case PopulateIter
            Case Is > 900
                Exit Sub
            Case Is > 170
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

                                               If PopulateTaskS > 0 AndAlso PopulateIter <= 180 AndAlso AudioConverterMode Then
                                                   Try
                                                       Dim arP As AudioProfile() = AudioCL.ToArray
                                                       If arP.Length <= 20 Then PopulateIter += 20
                                                       Dim mTS = MediaInfo.Cache.AsThreadSafe

                                                       If PopulateTaskS > 0 AndAlso PopulateIter <= 180 AndAlso AudioConverterMode Then

                                                           Dim pl = Parallel.For(0, arP.Length, New ParallelOptions With {.MaxDegreeOfParallelism = Math.Max(CPUsC \ 2, 1)},
                                                                                 Sub(i, pls)
                                                                                     If PopulateTaskS <= 0 OrElse Not AudioConverterMode Then
                                                                                         pls.Stop()
                                                                                         Return
                                                                                     End If
                                                                                     If (Me.dgvAudio.Rows.GetRowState(i) And DataGridViewElementStates.Displayed) = 0 Then
                                                                                         Dim ap As AudioProfile = arP(i)
                                                                                         If Not MediaInfo.Cache.ContainsKey(ap.FileKeyHashValue) Then
                                                                                             Dim mi = New MediaInfo(ap.FileValue)
                                                                                             If PopulateTaskS <= 0 Then
                                                                                                 pls.Stop()
                                                                                                 Return
                                                                                             End If
                                                                                             If (Me.dgvAudio.Rows.GetRowState(i) And DataGridViewElementStates.Displayed) = 0 Then
                                                                                                 mTS.Item(ap.FileKeyHashValue) = mi
                                                                                                 ''MediaInfo.Cache.Item(arP(i).FileKeyHashValue) = mi
                                                                                                 If ap.DefaultName Is Nothing Then ' Or remove it ???
                                                                                                 End If
                                                                                                 If ap.DisplayName Is Nothing Then
                                                                                                 End If
                                                                                             End If
                                                                                         Else
                                                                                             If ap.DefaultName Is Nothing Then
                                                                                             End If
                                                                                             If ap.DisplayName Is Nothing Then
                                                                                             End If
                                                                                         End If
                                                                                     End If
                                                                                 End Sub)
                                                           If pl.IsCompleted Then
                                                               PopulateIter += 20
                                                               If PopulateRSW.ElapsedTicks < 20000000 Then PopulateIter += 60
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
            PopulateTask = Task.Factory.StartNew(Sub() pTaskA(Delay), TaskCreationOptions.LongRunning)
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
                        Dim exts As String = String.Join(";*.", ave)
                        dialog.Filter = "*." & exts & "|*." & exts & "|All Files|*.*"
                        If dialog.ShowDialog = DialogResult.OK Then
                            avF = dialog.FileNames.WhereF(Function(f) AudioVideoFExts.Contains(f.Ext))
                            If avF.Length <= 0 Then MsgWarn("No supported Audio/Video files found in:", dialog.FileName.Dir, dWidth:=240)
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
                                Interlocked.Exchange(tdS, 0)
                                avF = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.TopDirectoryOnly).WhereF(Function(f) AudioVideoFExts.Contains(f.Ext()))
                            Else
                                avF = gfT.Result
                            End If
                            If avF.Length <= 0 Then MsgWarn("No supported Audio/Video files found in this location:", dialog.SelectedPath, dWidth:=240)
                        End If
                    End Using
            End Select

            If avF?.Length > 0 Then ProcessInputAudioFiles(avF)

        Catch ex As Exception
            Log.Write("Audio Converter Files opening error", ex.ToString)
            g.ShowException(ex, "Audio Converter Files opening error from selected location!")
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
            Task.Run(Sub() FlashingText("File not found ! Pos: " & (sr0idx + 1).ToInvStr))
            Exit Sub
        End If

        dgvAudio.FirstDisplayedScrollingRowIndex = sr0idx
        Dim rC As Integer = AudioCL.Count
        Dim srC As Integer = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        StatusText("Edit:" & (sr0idx + 1).ToInvStr & " Sel: " & srC.ToInvStr & " / Tot: " & rC.ToInvStr)
        Dim isGap = ap.IsGapProfile
        Dim lang As Language = If(ap.Language, New Language) ', Language.CurrentCulture) ???
        Dim oldDelay As Integer = ap.Delay

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
                Dim clang As Boolean = Not lang.Equals(ap.Language)
                If clang Then lang = ap.Language
                Dim cDelayTo0 As Boolean = ap.Delay = 0 AndAlso ap.Delay <> oldDelay
                'ap.DisplayNameValue = Nothing
                Dim srCache(rC - 1) As Boolean
                srCache(sr0idx) = True
                Dim arr(rC - 1) As AudioProfile
                AudioSBL.CopyTo(arr, 0)
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
                                                gap0.ChannelsValue = gapn.ChannelsValue
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
                dgvAudio.CurrentCell = dgvAudio(ccI, crI)
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
                                                   Dim imgDel As Image = ImageHelp.GetImageC(Symbol.Delete)
                                                   Me.BeginInvoke(Sub()
                                                                      bnRemove.Image = imgDel
                                                                      bnRemove.Text = "From Disk"
                                                                  End Sub)
                                               End If
                                           End SyncLock
                                       End Sub)
        Else
            If ButtonAltImgDone Then 'ToDo:Use only one Int32 Fiels and Remove Task Field !!! Interlocked.CompareExchange(ButtonAltImgDone, False, True)
                ButtonAltImgDone = False
                Me.BeginInvoke(Sub()
                                   bnRemove.Image = ImageHelp.GetImageC(Symbol.Remove)
                                   bnRemove.Text = "&Remove "
                               End Sub)
            End If
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Dim ekd As Integer = e.KeyData
        If ekd = (Keys.Control Or Keys.F) Then  'ekd <= 90 AndAlso ekd >= 65 OrElse ekd >= 48 AndAlso ekd <= 57 OrElse ekd >= 96 AndAlso ekd <= 105 Then ' [A-Z] [1-9]&numL
            If ActiveControl Is TBFind Then
                If BuildFindT Is Nothing OrElse BuildFindT.IsCompleted Then
                    TBFind_Enter(Nothing, Nothing)
                End If
            Else
                TBFind.Select()
            End If
            'TBFind.AppendText([Enum](Of Keys).GetName(ekd).ToLower(InvCult))
            'TBFind.ScrollToCaret()
            e.SuppressKeyPress = True
        ElseIf ekd = (Keys.ShiftKey Or Keys.Shift Or Keys.Control) OrElse ekd = (Keys.ControlKey Or Keys.Shift Or Keys.Control) Then
            If Not ButtonAltImgDone AndAlso Not e.Alt Then
                ButtonAltImage(True)
            End If
        ElseIf ekd = (Keys.Cancel Or Keys.Control) Then
            If Not Log.IsEmpty Then Log.Save()
            g.MainForm.ForceClose = True
            Me.Close()
            ActionMenuItem.ClearAdd2RangeList()
            ContextMenuStripEx.ClearAdd2RangeList()
            MediaInfo.ClearCache(False)
            ImageHelp.DisposeFonts()
            g.MainForm.Close()
            Application.Exit()
        Else
            MyBase.OnKeyDown(e)
        End If
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        If e.KeyCode = Keys.F3 Then
            SW2.Stop()
            If LastKeyDown = 3 Then Task.Run(Sub()
                                                 UpdateControls(selRowsCount:=1)
                                                 SelectChangeES = False
                                                 LastKeyDown = -1
                                             End Sub)
        Else
            Select Case e.KeyData
                Case Keys.ShiftKey Or Keys.Control, Keys.ControlKey Or Keys.Shift, Keys.ControlKey Or Keys.Shift Or Keys.Alt, Keys.ShiftKey Or Keys.Control Or Keys.Alt
                    ButtonAltImage(False)
            End Select
            MyBase.OnKeyUp(e)
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
                If AudioCL.Count > 1000 OrElse (AudioCL.Count - MediaInfo.Cache.Count > 100) Then StatusText("Sorting...")
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
        Dim srC = dgvAudio.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim fileListDup As New List(Of String)(srC)
        Dim dirListDup As New List(Of String)(srC)
        Dim rowIdxL As New List(Of Integer)(srC)
        Dim exitDel As Action = Sub()
                                    ButtonAltImage(False)
                                    UpdateControls()
                                End Sub
        For r = 0 To AudioCL.Count - 1
            If dgvAudio.Rows.GetRowState(r) >= &B1100000 Then
                Dim fPt As String = AudioSBL.Item(r).File
                If FS.FileExists(fPt) Then
                    fileListDup.Add(fPt)
                    dirListDup.Add(fPt.Dir)
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


        Dim ask As Boolean = MsgQuestion("Ask for each file to delete", TaskDialogButtons.YesNo) = DialogResult.Yes
        StatusText("Deleting from disk...")

        If Not My.Computer.Keyboard.ShiftKeyDown Then ' FailSafe
            exitDel()
            Exit Sub
        End If

        Dim fokC As Integer
        Dim dokC As Integer

        For f = 0 To fileList.Length - 1
            Dim fPath As String = fileList(f)

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

        For r = 0 To rowIdxL.Count - 1
            Dim ri As Integer = rowIdxL.Item(r)

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
            TBFind.Clear()
            TBFind_Leave(Nothing, Nothing)
            CurrentType = Nothing
            UpdateControls()
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
            GC.Collect(2, GCCollectionMode.Forced, True, True)
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
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
            Dim ccI As Integer = dgvAudio.CurrentCellAddress.X
            Dim dArr(rC - srC - 1) As AudioProfile
            AudioSBL.RaiseListChangedEvents = False 'move after clear???

            For r = 0 To rC - 1
                If dgvAudio.Rows.GetRowState(r) < &B1100000 Then
                    Dim i As Integer
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
            dgvAudio.CurrentCell = dgvAudio(ccI, crI)
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
            AudioSBL.RaiseListChangedEvents = False ' Test this !!!
            Dim current As AudioProfile = AudioSBL(crI)
            AudioSBL.RemoveAt(crI)
            crI -= 1
            AudioSBL.Insert(crI, current)
            AudioSBL.RaiseListChangedEvents = True
            AudioSBL.ResetItem(crI)
            dgvAudio.CurrentCell = dgvAudio(cc.X, crI)
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
            AudioSBL.RaiseListChangedEvents = True
            AudioSBL.ResetItem(crI)
            dgvAudio.CurrentCell = dgvAudio(cc.X, crI)
        End If

        PopulateCache(4000000)
        UpdateControls(1, crI)
        SelectChangeES = False
        SW2.Stop()
    End Sub

    Private Sub bnAudioPlay_Click(sender As Object, e As EventArgs) Handles bnPlay.Click
        Dim crI As Integer = dgvAudio.CurrentCellAddress.Y
        dgvAudio.FirstDisplayedScrollingRowIndex = crI
        If FileExists(AudioSBL(crI).File) Then g.Play(AudioSBL(crI).File) Else Task.Run(Sub() FlashingText("File not found ! Pos: " & (crI + 1).ToInvStr))
    End Sub

    Private Sub bnAudioMediaInfo_Click(sender As Object, e As EventArgs) Handles bnAudioMediaInfo.Click
        Dim crI As Integer = dgvAudio.CurrentCellAddress.Y
        dgvAudio.FirstDisplayedScrollingRowIndex = crI
        If FileExists(AudioSBL(crI).File) Then g.DefaultCommands.ShowMediaInfo(AudioSBL(crI).File) Else Task.Run(Sub() FlashingText("File not found ! Pos: " & (crI + 1).ToInvStr))
    End Sub

    Private Sub bnCMD_Click(sender As Object, e As EventArgs) Handles bnCMD.Click
        Dim crI As Integer = dgvAudio.CurrentCellAddress.Y
        If FileExists(AudioSBL(crI).File) Then
            Dim ap = AudioSBL(crI)
            dgvAudio.FirstDisplayedScrollingRowIndex = crI
            If TypeOf ap Is GUIAudioProfile Then
                g.ShowCommandLinePreview("Command Line", (DirectCast(ap, GUIAudioProfile)?.GetCommandLine(True)))
            ElseIf TypeOf ap Is BatchAudioProfile Then
                g.ShowCommandLinePreview("Command Line", (DirectCast(ap, BatchAudioProfile)?.GetCode))
            End If
        Else
            Task.Run(Sub() FlashingText("File not found ! Pos: " & (crI + 1).ToInvStr))
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
        If Not numThreads.Visible Then
            laThreads.Text = "Threads : "
            numThreads.Value = If(MaxThreads > 0, MaxThreads, s.ParallelProcsNum)
            numThreads.Visible = True
        Else
            numThreads.Visible = False
        End If
    End Sub

    Private Sub bnSort_Click(sender As Object, e As EventArgs) Handles BnSort.Click
        With dgvAudio
            If .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None Then
                .Columns(0).FillWeight = 10
                .Columns(1).FillWeight = 100
                .Columns(2).FillWeight = 400
                .Columns(3).FillWeight = 500
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ElseIf .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill Then
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            End If
        End With
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
            If PopulateTask IsNot Nothing AndAlso Not PopulateWSW.IsRunning Then PopulateTask.Wait(150)
            Dim miT = Task.Run(Sub() MediaInfo.ClearCache())
            Text = "AudioConverter"
            ImageHelp.ClearCache() 'No Dispose Version
            ActionMenuItem.ClearAdd2RangeList()
            ContextMenuStripEx.ClearAdd2RangeList()
            If BuildFindT?.IsCompleted Then TBFind.Dispose()
            TBFind.Clear()
            TBFind_Leave(Nothing, Nothing)
            Dim uAR As AudioProfile()
            If FindDuplicates(False) > 0 Then uAR = AudioCL.Distinct(New AudioProfileHComparer).ToArray Else uAR = AudioCL.ToArray
            'Dim ddup = AudioCL.GroupBy(Function(f As AudioProfile) f, New AudioProfilehComparer).Where(Function(g) g.Count > 1).Select(Function(d) d.Key.FileKeyHashValue & d.Key.File).ToArray
            AudioSBL.Clear()
            AudioCL.Reset()
            AudioCL.Capacity = uAR.Length

            SW2.Stop()
            Dim st1 = SW2.ElapsedTicks / SWFreq
            SW2.Restart()

            Dim fkhA(uAR.Length - 1) As Long
            Dim isGAP = rC > 0 AndAlso uAR(0).IsGapProfile
            If rC > 0 Then Parallel.For(0, uAR.Length, Sub(i)
                                                           With uAR(i)
                                                               .SourceSamplingRateValue = 0
                                                               .DisplayNameValue = Nothing
                                                               If isGAP Then
                                                                   Dim gap As GUIAudioProfile = DirectCast(uAR(i), GUIAudioProfile)
                                                                   gap.ChannelsValue = 0
                                                                   gap.SourceDepth = 0
                                                                   gap.DefaultnameValue = Nothing
                                                               End If
                                                               .FileKeyHashValue = ((.FileValue.GetHashCode + 2147483648L) << 16) + .FileValue.Length
                                                               fkhA(i) = .FileKeyHashValue
                                                           End With
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

            miT.Wait()
            AudioSBL.RaiseListChangedEvents = True
            dgvAudio.DataSource = AudioSBL
            AudioSBL.ResetBindings()
            AutoSizeColumns()
            If crI >= 0 Then dgvAudio.CurrentCell = dgvAudio(If(occ.X >= 0, occ.X, 2), crI)
            'dgvAudio.SelectAll()
            UpdateControls(If(rC > 0, 1, -2), If(crI >= 0, crI, -1))
            AddHandlersDGV()
            If uhC <> uAR.Length Then
                Task.Run(Sub() FlashingText("File/Key error: " & uhC.ToInvStr & " K/F:" & uAR.Length.ToInvStr, 20, 150))
                MsgInfo("File Error/Key Collision !!!", uhC.ToInvStr & " Keys / Files: " & uAR.Length.ToInvStr)
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
            PopulateIter = 2
            PopulateCache(4000000)

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

    <CompilerServices.MethodImpl(AggrInlin)> Private Sub AddHandlersDGV()
        AddHandler dgvAudio.SelectionChanged, AddressOf dgvAudio_SelectionChanged
        AddHandler dgvAudio.Scroll, AddressOf dgvAudio_Scroll
    End Sub

End Class

Public Class AudioProfileHComparer 'ToDo Add more than 1 stream from file
    Implements IEqualityComparer(Of AudioProfile)

    <CompilerServices.MethodImpl(AggrInlin)>
    Public Function Equals2(x As AudioProfile, y As AudioProfile) As Boolean Implements IEqualityComparer(Of AudioProfile).Equals
        If x Is y Then Return True
        If x Is Nothing OrElse y Is Nothing Then Return False
        Return x.FileKeyHashValue = y.FileKeyHashValue 'AndAlso x Is y -Hash col safe??
    End Function

    <CompilerServices.MethodImpl(AggrInlin)>
    Public Function GetHashCode2(ap As AudioProfile) As Integer Implements IEqualityComparer(Of AudioProfile).GetHashCode
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
