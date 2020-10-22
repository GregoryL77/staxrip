
Imports System.ComponentModel
Imports System.Globalization
Imports System.Text.RegularExpressions

Imports StaxRip.UI

Imports System.Runtime.ExceptionServices
Imports System.Threading.Tasks
Public Class MuxerForm
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

    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents bnCommandLinePreview As System.Windows.Forms.Button
    Friend WithEvents CommandLineControl As StaxRip.CommandLineControl
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
    Friend WithEvents tpCommandLine As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tpOptions As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents tpSubtitles As TabPage
    Friend WithEvents tpAudio As TabPage
    Friend WithEvents bnAudioPlay As ButtonEx
    Friend WithEvents bnAudioDown As ButtonEx
    Friend WithEvents bnAudioUp As ButtonEx
    Friend WithEvents bnAudioRemove As ButtonEx
    Friend WithEvents bnAudioAdd As ButtonEx
    Friend WithEvents dgvAudio As DataGridViewEx
    Friend WithEvents bnAudioEdit As ButtonEx
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents flpAudio As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents pnTab As Panel
    Friend WithEvents tpAttachments As TabPage
    Friend WithEvents tlpAttachments As TableLayoutPanel
    Friend WithEvents flpAttachments As FlowLayoutPanel
    Friend WithEvents bnAttachmentAdd As ButtonEx
    Friend WithEvents lbAttachments As ListBoxEx
    Friend WithEvents bnAttachmentRemove As ButtonEx
    Friend WithEvents tpTags As TabPage
    Friend WithEvents dgvTags As DataGridViewEx
    Friend WithEvents tlpSubtitles As TableLayoutPanel
    Friend WithEvents flpSubtitleButtons As FlowLayoutPanel
    Friend WithEvents bnSubtitleAdd As Button
    Friend WithEvents bnSubtitleRemove As Button
    Friend WithEvents bnSubtitleUp As Button
    Friend WithEvents bnSubtitleDown As Button
    Friend WithEvents bnSubtitleSetNames As ButtonEx
    Friend WithEvents bnSubtitlePlay As ButtonEx
    Friend WithEvents bnSubtitleBDSup2SubPP As ButtonEx
    Friend WithEvents bnSubtitleEdit As ButtonEx
    Friend WithEvents dgvSubtitles As DataGridView
    Friend WithEvents bnAudioEncodeAll As ButtonEx
    Friend WithEvents bnAudioEncodeSelected As ButtonEx
    Friend WithEvents bnAudioRemoveAll As ButtonEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.CommandLineControl = New StaxRip.CommandLineControl()
        Me.bnCommandLinePreview = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpSubtitles = New System.Windows.Forms.TabPage()
        Me.tlpSubtitles = New System.Windows.Forms.TableLayoutPanel()
        Me.dgvSubtitles = New System.Windows.Forms.DataGridView()
        Me.flpSubtitleButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnSubtitleAdd = New System.Windows.Forms.Button()
        Me.bnSubtitleRemove = New System.Windows.Forms.Button()
        Me.bnSubtitleUp = New System.Windows.Forms.Button()
        Me.bnSubtitleDown = New System.Windows.Forms.Button()
        Me.bnSubtitleSetNames = New StaxRip.UI.ButtonEx()
        Me.bnSubtitlePlay = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleBDSup2SubPP = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleEdit = New StaxRip.UI.ButtonEx()
        Me.tpAudio = New System.Windows.Forms.TabPage()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAudio = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAudioAdd = New StaxRip.UI.ButtonEx()
        Me.bnAudioRemove = New StaxRip.UI.ButtonEx()
        Me.bnAudioRemoveAll = New StaxRip.UI.ButtonEx()
        Me.bnAudioUp = New StaxRip.UI.ButtonEx()
        Me.bnAudioDown = New StaxRip.UI.ButtonEx()
        Me.bnAudioPlay = New StaxRip.UI.ButtonEx()
        Me.bnAudioEncodeSelected = New StaxRip.UI.ButtonEx()
        Me.bnAudioEncodeAll = New StaxRip.UI.ButtonEx()
        Me.bnAudioEdit = New StaxRip.UI.ButtonEx()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.tpAttachments = New System.Windows.Forms.TabPage()
        Me.tlpAttachments = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAttachments = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAttachmentAdd = New StaxRip.UI.ButtonEx()
        Me.bnAttachmentRemove = New StaxRip.UI.ButtonEx()
        Me.lbAttachments = New StaxRip.UI.ListBoxEx()
        Me.tpTags = New System.Windows.Forms.TabPage()
        Me.dgvTags = New StaxRip.UI.DataGridViewEx()
        Me.tpOptions = New System.Windows.Forms.TabPage()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.tpCommandLine = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnTab = New System.Windows.Forms.Panel()
        Me.tcMain.SuspendLayout()
        Me.tpSubtitles.SuspendLayout()
        Me.tlpSubtitles.SuspendLayout()
        CType(Me.dgvSubtitles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flpSubtitleButtons.SuspendLayout()
        Me.tpAudio.SuspendLayout()
        Me.tlpAudio.SuspendLayout()
        Me.flpAudio.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpAttachments.SuspendLayout()
        Me.tlpAttachments.SuspendLayout()
        Me.flpAttachments.SuspendLayout()
        Me.tpTags.SuspendLayout()
        CType(Me.dgvTags, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpOptions.SuspendLayout()
        Me.tpCommandLine.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.pnTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'CommandLineControl
        '
        Me.CommandLineControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommandLineControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandLineControl.Location = New System.Drawing.Point(0, 15)
        Me.CommandLineControl.Margin = New System.Windows.Forms.Padding(0)
        Me.CommandLineControl.Name = "CommandLineControl"
        Me.CommandLineControl.Size = New System.Drawing.Size(531, 260)
        Me.CommandLineControl.TabIndex = 0
        '
        'bnCommandLinePreview
        '
        Me.bnCommandLinePreview.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnCommandLinePreview.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnCommandLinePreview.Location = New System.Drawing.Point(270, 325)
        Me.bnCommandLinePreview.Name = "bnCommandLinePreview"
        Me.bnCommandLinePreview.Size = New System.Drawing.Size(107, 23)
        Me.bnCommandLinePreview.TabIndex = 4
        Me.bnCommandLinePreview.Text = "Command Line..."
        Me.bnCommandLinePreview.UseVisualStyleBackColor = True
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(472, 325)
        Me.bnCancel.Size = New System.Drawing.Size(83, 23)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(383, 325)
        Me.bnOK.Size = New System.Drawing.Size(83, 23)
        Me.bnOK.Text = "OK"
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tpSubtitles)
        Me.tcMain.Controls.Add(Me.tpAudio)
        Me.tcMain.Controls.Add(Me.tpAttachments)
        Me.tcMain.Controls.Add(Me.tpTags)
        Me.tcMain.Controls.Add(Me.tpOptions)
        Me.tcMain.Controls.Add(Me.tpCommandLine)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(549, 313)
        Me.tcMain.TabIndex = 5
        '
        'tpSubtitles
        '
        Me.tpSubtitles.Controls.Add(Me.tlpSubtitles)
        Me.tpSubtitles.Location = New System.Drawing.Point(4, 24)
        Me.tpSubtitles.Margin = New System.Windows.Forms.Padding(2)
        Me.tpSubtitles.Name = "tpSubtitles"
        Me.tpSubtitles.Padding = New System.Windows.Forms.Padding(2)
        Me.tpSubtitles.Size = New System.Drawing.Size(541, 285)
        Me.tpSubtitles.TabIndex = 3
        Me.tpSubtitles.Text = " Subtitles "
        Me.tpSubtitles.UseVisualStyleBackColor = True
        '
        'tlpSubtitles
        '
        Me.tlpSubtitles.ColumnCount = 2
        Me.tlpSubtitles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.27605!))
        Me.tlpSubtitles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSubtitles.Controls.Add(Me.dgvSubtitles, 0, 0)
        Me.tlpSubtitles.Controls.Add(Me.flpSubtitleButtons, 1, 0)
        Me.tlpSubtitles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpSubtitles.Location = New System.Drawing.Point(2, 2)
        Me.tlpSubtitles.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpSubtitles.Name = "tlpSubtitles"
        Me.tlpSubtitles.RowCount = 1
        Me.tlpSubtitles.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpSubtitles.Size = New System.Drawing.Size(537, 281)
        Me.tlpSubtitles.TabIndex = 0
        '
        'dgvSubtitles
        '
        Me.dgvSubtitles.AllowDrop = True
        Me.dgvSubtitles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSubtitles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSubtitles.Location = New System.Drawing.Point(0, 0)
        Me.dgvSubtitles.Margin = New System.Windows.Forms.Padding(0)
        Me.dgvSubtitles.Name = "dgvSubtitles"
        Me.dgvSubtitles.RowHeadersWidth = 123
        Me.dgvSubtitles.RowTemplate.Height = 28
        Me.dgvSubtitles.Size = New System.Drawing.Size(441, 281)
        Me.dgvSubtitles.TabIndex = 22
        '
        'flpSubtitleButtons
        '
        Me.flpSubtitleButtons.AutoSize = True
        Me.flpSubtitleButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleAdd)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleRemove)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleUp)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleDown)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleSetNames)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitlePlay)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleBDSup2SubPP)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleEdit)
        Me.flpSubtitleButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpSubtitleButtons.Location = New System.Drawing.Point(441, 0)
        Me.flpSubtitleButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flpSubtitleButtons.Name = "flpSubtitleButtons"
        Me.flpSubtitleButtons.Size = New System.Drawing.Size(96, 232)
        Me.flpSubtitleButtons.TabIndex = 21
        '
        'bnSubtitleAdd
        '
        Me.bnSubtitleAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitleAdd.Location = New System.Drawing.Point(3, 0)
        Me.bnSubtitleAdd.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnSubtitleAdd.Name = "bnSubtitleAdd"
        Me.bnSubtitleAdd.Size = New System.Drawing.Size(93, 27)
        Me.bnSubtitleAdd.TabIndex = 12
        Me.bnSubtitleAdd.Text = "Add..."
        '
        'bnSubtitleRemove
        '
        Me.bnSubtitleRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitleRemove.Location = New System.Drawing.Point(3, 29)
        Me.bnSubtitleRemove.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnSubtitleRemove.Name = "bnSubtitleRemove"
        Me.bnSubtitleRemove.Size = New System.Drawing.Size(93, 27)
        Me.bnSubtitleRemove.TabIndex = 13
        Me.bnSubtitleRemove.Text = " Remove"
        '
        'bnSubtitleUp
        '
        Me.bnSubtitleUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitleUp.Location = New System.Drawing.Point(3, 58)
        Me.bnSubtitleUp.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnSubtitleUp.Name = "bnSubtitleUp"
        Me.bnSubtitleUp.Size = New System.Drawing.Size(93, 27)
        Me.bnSubtitleUp.TabIndex = 11
        Me.bnSubtitleUp.Text = "Up"
        '
        'bnSubtitleDown
        '
        Me.bnSubtitleDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitleDown.Location = New System.Drawing.Point(3, 87)
        Me.bnSubtitleDown.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnSubtitleDown.Name = "bnSubtitleDown"
        Me.bnSubtitleDown.Size = New System.Drawing.Size(93, 27)
        Me.bnSubtitleDown.TabIndex = 10
        Me.bnSubtitleDown.Text = "Down"
        '
        'bnSubtitleSetNames
        '
        Me.bnSubtitleSetNames.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitleSetNames.Location = New System.Drawing.Point(3, 116)
        Me.bnSubtitleSetNames.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnSubtitleSetNames.Size = New System.Drawing.Size(93, 27)
        Me.bnSubtitleSetNames.Text = "Set Names..."
        '
        'bnSubtitlePlay
        '
        Me.bnSubtitlePlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitlePlay.Location = New System.Drawing.Point(3, 145)
        Me.bnSubtitlePlay.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnSubtitlePlay.Size = New System.Drawing.Size(93, 27)
        Me.bnSubtitlePlay.Text = "Play"
        '
        'bnSubtitleBDSup2SubPP
        '
        Me.bnSubtitleBDSup2SubPP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitleBDSup2SubPP.Location = New System.Drawing.Point(3, 174)
        Me.bnSubtitleBDSup2SubPP.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnSubtitleBDSup2SubPP.Size = New System.Drawing.Size(93, 27)
        Me.bnSubtitleBDSup2SubPP.Text = "BDSup2Sub++"
        '
        'bnSubtitleEdit
        '
        Me.bnSubtitleEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitleEdit.Location = New System.Drawing.Point(3, 203)
        Me.bnSubtitleEdit.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnSubtitleEdit.Size = New System.Drawing.Size(93, 27)
        Me.bnSubtitleEdit.Text = "Subtitle Edit"
        '
        'tpAudio
        '
        Me.tpAudio.Controls.Add(Me.tlpAudio)
        Me.tpAudio.Location = New System.Drawing.Point(4, 24)
        Me.tpAudio.Margin = New System.Windows.Forms.Padding(2)
        Me.tpAudio.Name = "tpAudio"
        Me.tpAudio.Padding = New System.Windows.Forms.Padding(2)
        Me.tpAudio.Size = New System.Drawing.Size(541, 285)
        Me.tpAudio.TabIndex = 4
        Me.tpAudio.Text = "   Audio   "
        Me.tpAudio.UseVisualStyleBackColor = True
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 2
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.Controls.Add(Me.flpAudio, 1, 0)
        Me.tlpAudio.Controls.Add(Me.dgvAudio, 0, 0)
        Me.tlpAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(2, 2)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 1
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAudio.Size = New System.Drawing.Size(537, 281)
        Me.tlpAudio.TabIndex = 7
        '
        'flpAudio
        '
        Me.flpAudio.AutoSize = True
        Me.flpAudio.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAudio.Controls.Add(Me.bnAudioAdd)
        Me.flpAudio.Controls.Add(Me.bnAudioRemove)
        Me.flpAudio.Controls.Add(Me.bnAudioRemoveAll)
        Me.flpAudio.Controls.Add(Me.bnAudioUp)
        Me.flpAudio.Controls.Add(Me.bnAudioDown)
        Me.flpAudio.Controls.Add(Me.bnAudioPlay)
        Me.flpAudio.Controls.Add(Me.bnAudioEncodeSelected)
        Me.flpAudio.Controls.Add(Me.bnAudioEncodeAll)
        Me.flpAudio.Controls.Add(Me.bnAudioEdit)
        Me.flpAudio.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpAudio.Location = New System.Drawing.Point(440, 0)
        Me.flpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAudio.Name = "flpAudio"
        Me.flpAudio.Size = New System.Drawing.Size(97, 261)
        Me.flpAudio.TabIndex = 6
        '
        'bnAudioAdd
        '
        Me.bnAudioAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioAdd.Location = New System.Drawing.Point(4, 0)
        Me.bnAudioAdd.Margin = New System.Windows.Forms.Padding(4, 0, 0, 2)
        Me.bnAudioAdd.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioAdd.Text = "Add..."
        '
        'bnAudioRemove
        '
        Me.bnAudioRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioRemove.Location = New System.Drawing.Point(4, 29)
        Me.bnAudioRemove.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnAudioRemove.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioRemove.Text = " Remove"
        '
        'bnAudioRemoveAll
        '
        Me.bnAudioRemoveAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioRemoveAll.Location = New System.Drawing.Point(4, 58)
        Me.bnAudioRemoveAll.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnAudioRemoveAll.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioRemoveAll.Text = "Remove All"
        Me.bnAudioRemoveAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bnAudioUp
        '
        Me.bnAudioUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioUp.Location = New System.Drawing.Point(4, 87)
        Me.bnAudioUp.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnAudioUp.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioUp.Text = "Up"
        '
        'bnAudioDown
        '
        Me.bnAudioDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioDown.Location = New System.Drawing.Point(4, 116)
        Me.bnAudioDown.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnAudioDown.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioDown.Text = "Down"
        '
        'bnAudioPlay
        '
        Me.bnAudioPlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioPlay.Location = New System.Drawing.Point(4, 145)
        Me.bnAudioPlay.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnAudioPlay.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioPlay.Text = "Play"
        '
        'bnAudioEncodeOne
        '
        Me.bnAudioEncodeSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioEncodeSelected.Location = New System.Drawing.Point(4, 174)
        Me.bnAudioEncodeSelected.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnAudioEncodeSelected.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioEncodeSelected.Text = "Encode"
        '
        'bnAudioEncodeAll
        '
        Me.bnAudioEncodeAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioEncodeAll.Location = New System.Drawing.Point(4, 203)
        Me.bnAudioEncodeAll.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnAudioEncodeAll.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioEncodeAll.Text = "Encode All "
        Me.bnAudioEncodeAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bnAudioEdit
        '
        Me.bnAudioEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAudioEdit.Location = New System.Drawing.Point(4, 232)
        Me.bnAudioEdit.Margin = New System.Windows.Forms.Padding(3, 0, 0, 2)
        Me.bnAudioEdit.Size = New System.Drawing.Size(93, 27)
        Me.bnAudioEdit.Text = "Edit..."
        '
        'dgvAudio
        '
        Me.dgvAudio.AllowDrop = True
        Me.dgvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAudio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAudio.Location = New System.Drawing.Point(0, 0)
        Me.dgvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.RowHeadersWidth = 123
        Me.dgvAudio.RowTemplate.Height = 28
        Me.dgvAudio.Size = New System.Drawing.Size(440, 281)
        Me.dgvAudio.TabIndex = 0
        '
        'tpAttachments
        '
        Me.tpAttachments.Controls.Add(Me.tlpAttachments)
        Me.tpAttachments.Location = New System.Drawing.Point(4, 24)
        Me.tpAttachments.Margin = New System.Windows.Forms.Padding(1)
        Me.tpAttachments.Name = "tpAttachments"
        Me.tpAttachments.Padding = New System.Windows.Forms.Padding(1)
        Me.tpAttachments.Size = New System.Drawing.Size(541, 285)
        Me.tpAttachments.TabIndex = 5
        Me.tpAttachments.Text = "  Attachments  "
        Me.tpAttachments.UseVisualStyleBackColor = True
        '
        'tlpAttachments
        '
        Me.tlpAttachments.ColumnCount = 2
        Me.tlpAttachments.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAttachments.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAttachments.Controls.Add(Me.flpAttachments, 1, 0)
        Me.tlpAttachments.Controls.Add(Me.lbAttachments, 0, 0)
        Me.tlpAttachments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAttachments.Location = New System.Drawing.Point(1, 1)
        Me.tlpAttachments.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpAttachments.Name = "tlpAttachments"
        Me.tlpAttachments.RowCount = 1
        Me.tlpAttachments.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAttachments.Size = New System.Drawing.Size(539, 283)
        Me.tlpAttachments.TabIndex = 0
        '
        'flpAttachments
        '
        Me.flpAttachments.AutoSize = True
        Me.flpAttachments.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAttachments.Controls.Add(Me.bnAttachmentAdd)
        Me.flpAttachments.Controls.Add(Me.bnAttachmentRemove)
        Me.flpAttachments.Location = New System.Drawing.Point(441, 1)
        Me.flpAttachments.Margin = New System.Windows.Forms.Padding(1)
        Me.flpAttachments.Name = "flpAttachments"
        Me.flpAttachments.Size = New System.Drawing.Size(97, 58)
        Me.flpAttachments.TabIndex = 0
        '
        'bnAttachmentAdd
        '
        Me.bnAttachmentAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAttachmentAdd.Location = New System.Drawing.Point(4, 0)
        Me.bnAttachmentAdd.Margin = New System.Windows.Forms.Padding(4, 0, 0, 2)
        Me.bnAttachmentAdd.Size = New System.Drawing.Size(93, 27)
        Me.bnAttachmentAdd.Text = "Add..."
        '
        'bnAttachmentRemove
        '
        Me.bnAttachmentRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAttachmentRemove.Location = New System.Drawing.Point(4, 29)
        Me.bnAttachmentRemove.Margin = New System.Windows.Forms.Padding(4, 0, 0, 2)
        Me.bnAttachmentRemove.Size = New System.Drawing.Size(93, 27)
        Me.bnAttachmentRemove.Text = "Remove"
        '
        'lbAttachments
        '
        Me.lbAttachments.AllowDrop = True
        Me.lbAttachments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbAttachments.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lbAttachments.FormattingEnabled = True
        Me.lbAttachments.ItemHeight = 22
        Me.lbAttachments.Location = New System.Drawing.Point(1, 1)
        Me.lbAttachments.Margin = New System.Windows.Forms.Padding(1)
        Me.lbAttachments.Name = "lbAttachments"
        Me.lbAttachments.Size = New System.Drawing.Size(438, 281)
        Me.lbAttachments.TabIndex = 1
        '
        'tpTags
        '
        Me.tpTags.Controls.Add(Me.dgvTags)
        Me.tpTags.Location = New System.Drawing.Point(4, 24)
        Me.tpTags.Margin = New System.Windows.Forms.Padding(1)
        Me.tpTags.Name = "tpTags"
        Me.tpTags.Padding = New System.Windows.Forms.Padding(1)
        Me.tpTags.Size = New System.Drawing.Size(541, 285)
        Me.tpTags.TabIndex = 6
        Me.tpTags.Text = "  Tags"
        Me.tpTags.UseVisualStyleBackColor = True
        '
        'dgvTags
        '
        Me.dgvTags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTags.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTags.Location = New System.Drawing.Point(1, 1)
        Me.dgvTags.Margin = New System.Windows.Forms.Padding(1)
        Me.dgvTags.Name = "dgvTags"
        Me.dgvTags.RowHeadersWidth = 123
        Me.dgvTags.RowTemplate.Height = 46
        Me.dgvTags.Size = New System.Drawing.Size(539, 283)
        Me.dgvTags.TabIndex = 0
        '
        'tpOptions
        '
        Me.tpOptions.Controls.Add(Me.SimpleUI)
        Me.tpOptions.Location = New System.Drawing.Point(4, 24)
        Me.tpOptions.Margin = New System.Windows.Forms.Padding(2)
        Me.tpOptions.Name = "tpOptions"
        Me.tpOptions.Padding = New System.Windows.Forms.Padding(2)
        Me.tpOptions.Size = New System.Drawing.Size(541, 285)
        Me.tpOptions.TabIndex = 2
        Me.tpOptions.Text = "  Options  "
        Me.tpOptions.UseVisualStyleBackColor = True
        '
        'SimpleUI
        '
        Me.SimpleUI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SimpleUI.FormSizeScaleFactor = New System.Drawing.SizeF(0!, 0!)
        Me.SimpleUI.Location = New System.Drawing.Point(2, 2)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(2)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(537, 281)
        Me.SimpleUI.TabIndex = 0
        Me.SimpleUI.Text = "SimpleUI1"
        '
        'tpCommandLine
        '
        Me.tpCommandLine.Controls.Add(Me.TableLayoutPanel1)
        Me.tpCommandLine.Location = New System.Drawing.Point(4, 24)
        Me.tpCommandLine.Margin = New System.Windows.Forms.Padding(2)
        Me.tpCommandLine.Name = "tpCommandLine"
        Me.tpCommandLine.Padding = New System.Windows.Forms.Padding(5)
        Me.tpCommandLine.Size = New System.Drawing.Size(541, 285)
        Me.tpCommandLine.TabIndex = 1
        Me.tpCommandLine.Text = "  Command Line  "
        Me.tpCommandLine.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.CommandLineControl, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(5, 5)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(2)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(531, 275)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 0)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(156, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Additional custom switches:"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 3
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.bnCancel, 2, 1)
        Me.tlpMain.Controls.Add(Me.bnOK, 1, 1)
        Me.tlpMain.Controls.Add(Me.bnCommandLinePreview, 0, 1)
        Me.tlpMain.Controls.Add(Me.pnTab, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tlpMain.RowCount = 2
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(561, 354)
        Me.tlpMain.TabIndex = 8
        '
        'pnTab
        '
        Me.tlpMain.SetColumnSpan(Me.pnTab, 3)
        Me.pnTab.Controls.Add(Me.tcMain)
        Me.pnTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnTab.Location = New System.Drawing.Point(6, 6)
        Me.pnTab.Name = "pnTab"
        Me.pnTab.Size = New System.Drawing.Size(549, 313)
        Me.pnTab.TabIndex = 8
        '
        'MuxerForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(561, 354)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Name = "MuxerForm"
        Me.Text = "Container"
        Me.tcMain.ResumeLayout(False)
        Me.tpSubtitles.ResumeLayout(False)
        Me.tlpSubtitles.ResumeLayout(False)
        Me.tlpSubtitles.PerformLayout()
        CType(Me.dgvSubtitles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flpSubtitleButtons.ResumeLayout(False)
        Me.tpAudio.ResumeLayout(False)
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        Me.flpAudio.ResumeLayout(False)
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpAttachments.ResumeLayout(False)
        Me.tlpAttachments.ResumeLayout(False)
        Me.tlpAttachments.PerformLayout()
        Me.flpAttachments.ResumeLayout(False)
        Me.tpTags.ResumeLayout(False)
        CType(Me.dgvTags, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpOptions.ResumeLayout(False)
        Me.tpCommandLine.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.pnTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Muxer As Muxer
    Private AudioBindingSource As New BindingSource
    Private SubtitleBindingSource As New BindingSource
    Private SubtitleItems As New BindingList(Of SubtitleItem)

    Sub New(muxer As Muxer)
        MyBase.New()
        InitializeComponent()
        SetMinimumSize(30, 21)
        RestoreClientSize(45, 22)
        Text += " - " + muxer.Name
        Me.Muxer = muxer
        AddSubtitles(muxer.Subtitles)
        CommandLineControl.tb.Text = muxer.AdditionalSwitches
        tcMain.SelectedIndex = s.Storage.GetInt("last selected muxer tab")

        lbAttachments.Items.AddRange(muxer.Attachments.Select(Function(val) New AttachmentContainer With {.Filepath = val}).ToArray)
        lbAttachments.RemoveButton = bnAttachmentRemove

        AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks)

        dgvAudio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvAudio.MultiSelect = True
        dgvAudio.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAudio.AllowUserToResizeRows = False
        dgvAudio.RowHeadersVisible = False
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvAudio.AutoGenerateColumns = False
        dgvAudio.DataSource = AudioBindingSource

        dgvTags.DataSource = muxer.Tags
        dgvTags.AllowUserToAddRows = True
        dgvTags.AllowUserToDeleteRows = True
        dgvTags.Columns(0).Width = FontHeight * 10
        dgvTags.Columns(1).Width = FontHeight * 20
        dgvTags.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        bnAudioAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnAudioRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnAudioPlay.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnAudioUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnAudioDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnAudioEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)
        bnAudioEncodeSelected.Image = ImageHelp.GetSymbolImage(Symbol.MusicNote)
        bnAudioEncodeAll.Image = ImageHelp.GetSymbolImage(Symbol.MusicInfo)
        bnAudioRemoveAll.Image = ImageHelp.GetSymbolImage(Symbol.Clear)

        bnAttachmentAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnAttachmentRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)

        For Each bn In {bnAudioAdd, bnAudioRemove, bnAudioPlay, bnAudioUp,
                        bnAudioDown, bnAudioRemoveAll, bnAudioEdit, bnAudioEncodeAll, bnAudioEncodeSelected, bnAttachmentAdd, bnAttachmentRemove}

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

        Dim pathColumn = dgvAudio.AddTextBoxColumn()
        pathColumn.DataPropertyName = "DisplayName"
        pathColumn.HeaderText = "Track"
        pathColumn.ReadOnly = True

        If dgvAudio.RowCount > 0 Then
            dgvAudio.Rows(0).Selected = True
        End If

        dgvSubtitles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvSubtitles.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvSubtitles.AutoGenerateColumns = False
        dgvSubtitles.ShowCellToolTips = False
        dgvSubtitles.AllowUserToResizeRows = False
        dgvSubtitles.AllowUserToResizeColumns = False
        dgvSubtitles.RowHeadersVisible = False
        dgvSubtitles.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvSubtitles.MultiSelect = False
        dgvSubtitles.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable

        bnSubtitleAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnSubtitleRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnSubtitlePlay.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnSubtitleUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnSubtitleDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)

        For Each bn In {bnSubtitleAdd, bnSubtitleRemove, bnSubtitlePlay, bnSubtitleUp, bnSubtitleDown}
            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        Dim enabledColumn As New DataGridViewCheckBoxColumn
        enabledColumn.HeaderText = "Enabled"
        enabledColumn.DataPropertyName = "Enabled"
        dgvSubtitles.Columns.Add(enabledColumn)

        Dim languageColumn As New DataGridViewComboBoxColumn
        languageColumn.HeaderText = "Language"
        languageColumn.Items.AddRange(Language.Languages.ToArray)
        languageColumn.DataPropertyName = "Language"
        dgvSubtitles.Columns.Add(languageColumn)

        Dim nameColumn As New DataGridViewTextBoxColumn
        nameColumn.HeaderText = "Name"
        nameColumn.DataPropertyName = "Title"
        dgvSubtitles.Columns.Add(nameColumn)

        Dim defaultColumn As New DataGridViewCheckBoxColumn
        defaultColumn.HeaderText = "Default"
        defaultColumn.DataPropertyName = "Default"
        dgvSubtitles.Columns.Add(defaultColumn)

        Dim forcedColumn As New DataGridViewCheckBoxColumn
        forcedColumn.HeaderText = "Forced"
        forcedColumn.DataPropertyName = "Forced"
        dgvSubtitles.Columns.Add(forcedColumn)

        Dim idColumn As New DataGridViewTextBoxColumn
        idColumn.ReadOnly = True
        idColumn.HeaderText = "ID"
        idColumn.DataPropertyName = "ID"
        dgvSubtitles.Columns.Add(idColumn)

        Dim typeNameColumn As New DataGridViewTextBoxColumn
        typeNameColumn.ReadOnly = True
        typeNameColumn.HeaderText = "Type"
        typeNameColumn.DataPropertyName = "TypeName"
        dgvSubtitles.Columns.Add(typeNameColumn)

        Dim sizeColumn As New DataGridViewTextBoxColumn
        sizeColumn.ReadOnly = True
        sizeColumn.HeaderText = "Size"
        sizeColumn.DataPropertyName = "Size"
        dgvSubtitles.Columns.Add(sizeColumn)

        Dim filenameColumn As New DataGridViewTextBoxColumn
        filenameColumn.ReadOnly = True
        filenameColumn.HeaderText = "Filename"
        filenameColumn.DataPropertyName = "Filename"
        dgvSubtitles.Columns.Add(filenameColumn)

        SubtitleBindingSource.AllowNew = False
        SubtitleBindingSource.DataSource = SubtitleItems
        dgvSubtitles.DataSource = SubtitleBindingSource

        TipProvider.SetTip("Additional command line switches that may contain macros.", tpCommandLine)
    End Sub

    Public Class AttachmentContainer
        Property Filepath As String

        Public Overrides Function ToString() As String
            If Filepath.Contains("_attachment_") Then
                Return Filepath.Right("_attachment_")
            End If

            Return Path.GetFileName(Filepath)
        End Function
    End Class

    Public Class SubtitleItem
        Property Enabled As Boolean
        Property Language As Language
        Property Title As String
        Property Forced As Boolean
        Property [Default] As Boolean
        Property ID As Integer
        Property TypeName As String
        Property Size As String
        Property Filename As String
        Property Subtitle As Subtitle
    End Class

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)

        Dim lastAction As Action

        Dim UI = SimpleUI
        UI.Store = Muxer
        UI.BackColor = Color.Transparent

        Dim page = UI.CreateFlowPage("main page")
        page.SuspendLayout()

        Dim tb = UI.AddTextButton()
        tb.Text = "Cover"
        tb.Expandet = True
        tb.Property = NameOf(Muxer.CoverFile)
        tb.BrowseFile("jpg, png, webp, bmp|*.jpg;*.png;*.webp;*.bmp")

        Dim mb = UI.AddTextMenu()
        mb.Text = "Chapters"
        mb.Expandet = True
        mb.Help = "Chapter file to be muxed."
        mb.Property = NameOf(Muxer.ChapterFile)
        mb.AddMenu("Browse File...", Function() g.BrowseFile("txt, xml|*.txt;*.xml"))
        mb.AddMenu("Edit with chapterEditor...", Sub() g.ShellExecute(Package.chapterEditor.Path, Muxer.ChapterFile.Escape))

        If TypeOf Muxer Is MkvMuxer Then
            CommandLineControl.Presets = s.CmdlPresetsMKV

            mb = UI.AddTextMenu()
            mb.Text = "Tags"
            mb.Expandet = True
            mb.Help = "Tag file to be muxed."
            mb.Property = NameOf(Muxer.TagFile)
            mb.AddMenu("Browse File...", Function() g.BrowseFile("xml|*.xml"))
            mb.AddMenu("Edit File...", Sub() g.ShellExecute(g.GetAppPathForExtension("xml", "txt"), Muxer.TagFile.Escape))

            tb = UI.AddTextButton()
            tb.Text = "Timestamps"
            tb.Help = "txt or mkv file"
            tb.Expandet = True
            tb.Property = NameOf(Muxer.TimestampsFile)
            tb.BrowseFile("txt, mkv|*.txt;*.mkv")

            tb = UI.AddTextButton()
            tb.Text = "Title"
            tb.Expandet = True
            tb.Property = NameOf(MkvMuxer.Title)
            tb.Button.Text = "%"
            tb.Button.ClickAction = Sub() MacrosForm.ShowDialogForm()

            Dim t = UI.AddText()
            t.Text = "Video Track Name"
            t.Help = "Optional name of the video stream that may contain macros."
            t.Expandet = True
            t.Property = NameOf(Muxer.VideoTrackName)

            Dim tm = UI.AddTextMenu()
            tm.Text = "Display Aspect Ratio"
            tm.Help = "Display Aspect Ratio to be applied by mkvmerge. By default and best practice the aspect ratio should be signalled to the encoder and not to the muxer, use this setting at your own risk."
            tm.Property = NameOf(MkvMuxer.DAR)
            tm.AddMenu(s.DarMenu)

            Dim ml = UI.AddMenu(Of Language)()
            ml.Text = "Video Track Language"
            ml.Help = "Optional language of the video stream."
            ml.Property = NameOf(MkvMuxer.VideoTrackLanguage)

            Dim compression = UI.AddMenu(Of CompressionMode)()
            compression.Text = "Subtitle Compression"
            compression.Property = NameOf(MkvMuxer.Compression)

            lastAction = Sub()
                             For Each i In Language.Languages
                                 If i.IsCommon Then
                                     ml.Button.Add(i.ToString + " (" + i.TwoLetterCode + ", " + i.ThreeLetterCode + ")", i)
                                 Else
                                     ml.Button.Add("More | " + i.ToString.Substring(0, 1).ToUpper + " | " + i.ToString + " (" + i.TwoLetterCode + ", " + i.ThreeLetterCode + ")", i)
                                 End If

                                 Application.DoEvents()
                             Next
                         End Sub

        ElseIf TypeOf Muxer Is MP4Muxer Then
            tpAttachments.Enabled = False

            CommandLineControl.Presets = s.CmdlPresetsMP4

            Dim txt = UI.AddText()
            txt.Text = "Video Track Name"
            txt.Help = "Optional name of the video stream that may contain macros."
            txt.Expandet = True
            txt.Property = NameOf(Muxer.VideoTrackName)

            Dim textMenu = UI.AddTextMenu()
            textMenu.Text = "Pixel Aspect Ratio:"
            textMenu.Help = "Display Aspect Ratio to be applied by MP4Box. By default and best practice the aspect ratio should be signalled to the encoder and not to the muxer, use this setting at your own risk."
            textMenu.Property = NameOf(MP4Muxer.PAR)
            textMenu.AddMenu(s.ParMenu)
        End If

        page.ResumeLayout()
        lastAction?.Invoke
        UpdateControls()
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        If TypeOf Muxer Is MkvMuxer Then
            s.CmdlPresetsMKV = CommandLineControl.Presets
        ElseIf TypeOf Muxer Is MP4Muxer Then
            s.CmdlPresetsMP4 = CommandLineControl.Presets
        End If

        s.Storage.SetInt("last selected muxer tab", tcMain.SelectedIndex)
        SetValues()

        If DialogResult = DialogResult.OK Then
            p.AudioTracks = DirectCast(AudioBindingSource.DataSource, List(Of AudioProfile))
            Muxer.Attachments.Clear()
            Muxer.Attachments.AddRange(lbAttachments.Items.OfType(Of AttachmentContainer).Select(Function(val) val.Filepath))
        End If
    End Sub

    Sub SetValues()
        Muxer.Subtitles.Clear()

        For Each subtitle In SubtitleItems
            subtitle.Subtitle.Language = subtitle.Language
            subtitle.Subtitle.Enabled = subtitle.Enabled
            subtitle.Subtitle.Title = subtitle.Title
            subtitle.Subtitle.Forced = subtitle.Forced
            subtitle.Subtitle.Default = subtitle.Default

            If subtitle.Subtitle.Title Is Nothing Then
                subtitle.Subtitle.Title = ""
            End If

            Muxer.Subtitles.Add(subtitle.Subtitle)
        Next

        SimpleUI.Save()
        Muxer.AdditionalSwitches = CommandLineControl.tb.Text
    End Sub

    Sub AddAttachment(paths As String())
        Dim attachmentItems = lbAttachments.Items.OfType(Of AttachmentContainer).Select(Function(val) val.Filepath).ToList
        attachmentItems.AddRange(paths)
        attachmentItems.Sort()
        lbAttachments.Items.Clear()
        lbAttachments.Items.AddRange(attachmentItems.Select(Function(val) New AttachmentContainer With {.Filepath = val}).ToArray)
        UpdateControls()
    End Sub

    Sub UpdateControls()
        bnAudioRemove.Enabled = dgvAudio.SelectedRows.Count > 0
        bnAudioPlay.Enabled = dgvAudio.SelectedRows.Count = 1
        bnAudioEdit.Enabled = dgvAudio.SelectedRows.Count > 0
        bnAudioRemoveAll.Enabled = dgvAudio.Rows.Count > 0
        bnAudioEncodeAll.Enabled = dgvAudio.Rows.Count > 0
        bnAudioEncodeSelected.Enabled = dgvAudio.SelectedRows.Count > 0
        bnAudioUp.Enabled = dgvAudio.CanMoveUp AndAlso dgvAudio.SelectedRows.Count = 1
        bnAudioDown.Enabled = dgvAudio.CanMoveDown AndAlso dgvAudio.SelectedRows.Count = 1

        Dim selectedSubtitles = dgvSubtitles.SelectedRows.Count > 0
        Dim subtitlePath = If(selectedSubtitles AndAlso dgvSubtitles.CurrentRow.Index < SubtitleItems.Count, SubtitleItems(dgvSubtitles.CurrentRow.Index).Subtitle.Path, "")
        bnSubtitleBDSup2SubPP.Enabled = selectedSubtitles AndAlso {"idx", "sup"}.Contains(subtitlePath.Ext)
        bnSubtitleEdit.Enabled = bnSubtitleBDSup2SubPP.Enabled
        bnSubtitlePlay.Enabled = p.SourceFile <> "" AndAlso selectedSubtitles
        bnSubtitleUp.Enabled = dgvSubtitles.CanMoveUp
        bnSubtitleDown.Enabled = dgvSubtitles.CanMoveDown
        bnSubtitleSetNames.Enabled = selectedSubtitles
        bnSubtitleRemove.Enabled = selectedSubtitles

        lbAttachments.UpdateControls()
    End Sub

    Sub AddAudio(path As String, Optional AssignAudioProfile As Integer = 0)
        Dim ap As AudioProfile

        Select Case AssignAudioProfile
            Case 1
                ap = ObjectHelp.GetCopy(p.Audio0)
            Case 2
                ap = ObjectHelp.GetCopy(p.Audio1)
            Case Else
                Dim profileSelection As New SelectionBox(Of AudioProfile)
                profileSelection.Title = "Please select Audio Profile for:"
                profileSelection.Text = path.FixDir.ReplaceRecursive("  ", "").ShortBegEnd(64, 64)

                If Not TypeOf p.Audio0 Is NullAudioProfile Then
                    profileSelection.AddItem("Current Project 1", p.Audio0)
                End If
                If Not TypeOf p.Audio1 Is NullAudioProfile Then
                    profileSelection.AddItem("Current Project 2", p.Audio1)
                End If

                For Each AudioProfile In s.AudioProfiles
                    profileSelection.AddItem(AudioProfile)
                Next

                If profileSelection.Show <> DialogResult.OK Then
                    Exit Sub
                End If

                ap = ObjectHelp.GetCopy(profileSelection.SelectedValue)
        End Select

        ap.File = path

        If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
            ap.Delay = g.ExtractDelay(ap.File)
        End If

        If FileTypes.VideoAudio.Contains(ap.File.Ext) Then
            ap.Streams = MediaInfo.GetAudioStreams(ap.File)
        End If

        ap.SetStreamOrLanguage()

        If Not ap.Stream Is Nothing Then
            Dim streamSelection As New SelectionBox(Of AudioStream)
            streamSelection.Title = "Stream Selection"
            streamSelection.Text = "Please select an audio stream."

            For Each stream In ap.Streams
                streamSelection.AddItem(stream)
            Next

            If streamSelection.Show <> DialogResult.OK Then
                Exit Sub
            End If

            ap.Stream = streamSelection.SelectedValue
        End If

        g.MainForm.UpdateSizeOrBitrate()
        AudioBindingSource.Add(ap)
        AudioBindingSource.Position = AudioBindingSource.Count - 1
        UpdateControls()
    End Sub

    Sub AddSubtitles(subtitles As List(Of Subtitle))
        For Each i In subtitles
            If SubtitleItems.Where(Function(item) item.Default).Count > 0 Then
                i.Default = False
            End If

            If File.Exists(i.Path) Then
                Dim size As String

                If i.Size > 0 Then
                    If i.Size > 1024 ^ 2 Then
                        size = (i.Size / 1024 ^ 2).ToString("f1") & " MB"
                    Else
                        size = (i.Size / 1024).ToString("f1") & " KB"
                    End If
                End If

                Dim _option As String

                If i.Default AndAlso i.Forced Then
                    _option = "default, forced"
                ElseIf i.Default Then
                    _option = "default"
                ElseIf i.Forced Then
                    _option = "forced"
                Else
                    _option = ""
                End If

                Dim id As Integer

                Dim match = Regex.Match(i.Path, " ID(\d+)")

                If match.Success Then
                    id = CInt(match.Groups(1).Value)
                Else
                    id = i.StreamOrder + 1
                End If

                Dim item As New SubtitleItem
                item.Enabled = i.Enabled
                item.Language = i.Language
                item.Title = i.Title
                item.Default = i.Default
                item.Forced = i.Forced
                item.ID = id
                item.TypeName = i.TypeName
                item.Size = size
                item.Filename = i.Path.FileName
                item.Subtitle = i

                SubtitleItems.Add(item)
            End If
        Next
    End Sub

    Sub bnSubtitlePlay_Click(sender As Object, e As EventArgs) Handles bnSubtitlePlay.Click
        If Not Package.mpvnet.VerifyOK(True) Then
            Exit Sub
        End If

        Dim st = SubtitleItems(dgvSubtitles.CurrentRow.Index).Subtitle
        Dim filepath = st.Path

        If st.Path.Ext = "idx" Then
            filepath = p.TempDir + p.TargetFile.Base + "_play.idx"
            Regex.Replace(st.Path.ReadAllText, "langidx: \d+", "langidx: " +
                          st.IndexIDX.ToString).WriteFileDefault(filepath)
            FileHelp.Copy(st.Path.DirAndBase + ".sub", filepath.DirAndBase + ".sub")
        End If

        If FileTypes.SubtitleExludingContainers.Contains(filepath.Ext) Then
            g.ShellExecute(Package.mpvnet.Path, "--sub-files=" + filepath.Escape + " " + p.FirstOriginalSourceFile.Escape)
        ElseIf p.FirstOriginalSourceFile = filepath Then
            g.ShellExecute(Package.mpvnet.Path, "--sub=" & (st.Index + 1) & " " + filepath.Escape)
        End If
    End Sub

    Sub bnCommandLinePreview_Click(sender As Object, e As EventArgs) Handles bnCommandLinePreview.Click
        SetValues()
        g.ShowCommandLinePreview("Command Line", Muxer.GetCommandLine)
    End Sub

    Sub bnAudioAdd_Click(sender As Object, e As EventArgs) Handles bnAudioAdd.Click
        Dim A As Integer

        Using dialog As New OpenFileDialog
            dialog.Multiselect = True
            dialog.SetFilter(FileTypes.Audio.Union(FileTypes.VideoAudio))
            dialog.SetInitDir(p.TempDir)

            If dialog.ShowDialog = DialogResult.OK Then

                If dialog.FileNames.Count > 2000 Then
                    If MsgQuestion("Are you sure to add this many (" & dialog.FileNames.Count & ")files ?") = DialogResult.Cancel Then
                        dialog.Dispose()
                        Exit Sub
                    End If
                End If

                If dialog.FileNames.Count >= 18 AndAlso TypeOf p.Audio0 Is NullAudioProfile AndAlso TypeOf p.Audio1 Is NullAudioProfile Then
                    MsgWarn("Please choose desired Audio Profile in Main Window and try again")
                    Exit Sub
                End If

                If dialog.FileNames.Count > 1 Then
                    Using td As New TaskDialog(Of Integer)
                        'td.AllowCancel = False
                        If TypeOf p.Audio0 IsNot NullAudioProfile Then
                            td.AddCommand("Use current Audio Profile 1 for all files:" & BR & p.Audio0.Name, 1)
                        End If

                        If TypeOf p.Audio1 IsNot NullAudioProfile Then
                            td.AddCommand("Use current Audio Profile 2 for all files:" & BR & p.Audio1.Name, 2)
                        End If

                        If dialog.FileNames.Count < 18 Then
                            td.AddCommand("Assign Audio Profile manually for each", 0)
                        End If

                        Select Case td.Show
                            Case 1
                                A = 1
                            Case 2
                                A = 2
                        End Select
                    End Using

                    If dialog.FileNames.Count >= 18 AndAlso A = 0 Then
                        dialog.Dispose()
                        Exit Sub
                    End If

                End If

                For Each Path In dialog.FileNames
                    AddAudio(Path, A)
                Next
            End If
        End Using
    End Sub

    Sub bnAudioRemove_Click(sender As Object, e As EventArgs) Handles bnAudioRemove.Click
        dgvAudio.RemoveSelection
        UpdateControls()
    End Sub

    Sub bnAudioUp_Click(sender As Object, e As EventArgs) Handles bnAudioUp.Click
        dgvAudio.MoveSelectionUp
        UpdateControls()
    End Sub

    Sub bnAudioDown_Click(sender As Object, e As EventArgs) Handles bnAudioDown.Click
        dgvAudio.MoveSelectionDown
        UpdateControls()
    End Sub

    Sub bnAudioPlay_Click(sender As Object, e As EventArgs) Handles bnAudioPlay.Click
        g.Play(DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File)
    End Sub

    Sub bnAudioEdit_Click(sender As Object, e As EventArgs) Handles bnAudioEdit.Click
        Dim ap = DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile)

        'Dim ap As AudioProfile
        'For Each i In AudioBindingSource
        ' Dim idx = AudioBindingSource()
        'ap = DirectCast(i, AudioProfile)
        'Next

        ap.EditProject()
        g.MainForm.UpdateAudioMenu()
        g.MainForm.UpdateSizeOrBitrate()
        AudioBindingSource.ResetBindings(False)
    End Sub

    Sub bnAttachmentAdd_Click(sender As Object, e As EventArgs) Handles bnAttachmentAdd.Click
        Using dialog As New OpenFileDialog
            dialog.SetFilter({"ttf", "txt", "jpg", "png", "otf", "jpeg", "webp", "xml", "nfo"})
            dialog.Multiselect = True
            dialog.SetInitDir(p.TempDir)

            If dialog.ShowDialog = DialogResult.OK Then
                AddAttachment(dialog.FileNames)
            End If
        End Using
    End Sub

    Sub lbAttachments_DragDrop(sender As Object, e As DragEventArgs) Handles lbAttachments.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            AddAttachment(files)
        End If
    End Sub

    Sub lbAttachments_DragEnter(sender As Object, e As DragEventArgs) Handles lbAttachments.DragEnter
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Sub dgvAudio_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvAudio.MouseUp
        UpdateControls()
    End Sub

    Sub dgvAudio_DragEnter(sender As Object, e As DragEventArgs) Handles dgvAudio.DragEnter
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            If FileTypes.VideoAudio.Union(FileTypes.Audio).ContainsAny(files.Select(Function(item) item.Ext)) Then
                e.Effect = DragDropEffects.Copy
            End If
        End If
    End Sub

    Sub dgvAudio_DragDrop(sender As Object, e As DragEventArgs) Handles dgvAudio.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            For Each filePath In files
                If FileTypes.VideoAudio.Union(FileTypes.Audio).Contains(filePath.Ext) Then
                    AddAudio(filePath)
                End If
            Next
        End If
    End Sub

    Sub MuxerForm_HelpRequested(sender As Object, ea As HelpEventArgs) Handles Me.HelpRequested
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteParagraph(Strings.Muxer)
        form.Doc.WriteTips(TipProvider.GetTips, SimpleUI.ActivePage.TipProvider.GetTips)
        form.Doc.WriteTable("Macros", Macro.GetTips(False, True, False))
        form.Show()
    End Sub

    Sub dgvSubtitles_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvSubtitles.MouseUp
        UpdateControls()
    End Sub

    Sub dgvSubtitles_DragEnter(sender As Object, ea As DragEventArgs) Handles dgvSubtitles.DragEnter
        Dim files = TryCast(ea.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            If FileTypes.SubtitleIncludingContainers.ContainsAny(files.Select(Function(item) item.Ext)) Then
                ea.Effect = DragDropEffects.Copy
            End If
        End If
    End Sub

    Sub dgvSubtitles_DragDrop(sender As Object, ea As DragEventArgs) Handles dgvSubtitles.DragDrop
        Dim files = TryCast(ea.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            For Each filePath In files
                If FileTypes.SubtitleIncludingContainers.Contains(filePath.Ext) Then
                    AddSubtitles(Subtitle.Create(filePath))
                End If
            Next
        End If
    End Sub

    Sub dgvSubtitles_CellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) Handles dgvSubtitles.CellParsing
        If TypeOf dgvSubtitles.CurrentCell.OwningColumn Is DataGridViewComboBoxColumn Then
            Dim editingControl = DirectCast(dgvSubtitles.EditingControl, DataGridViewComboBoxEditingControl)
            e.Value = editingControl.SelectedItem
            e.ParsingApplied = True
        End If
    End Sub

    Sub dgvSubtitles_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSubtitles.CellEndEdit
        UpdateControls()
    End Sub

    Sub bnSubtitleAdd_Click(sender As Object, e As EventArgs) Handles bnSubtitleAdd.Click
        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.SubtitleIncludingContainers)
            dialog.Multiselect = True
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog = DialogResult.OK Then
                For Each fn In dialog.FileNames
                    AddSubtitles(Subtitle.Create(fn))
                Next

                UpdateControls()
            End If
        End Using
    End Sub

    Sub bnSubtitleRemove_Click(sender As Object, e As EventArgs) Handles bnSubtitleRemove.Click
        dgvSubtitles.RemoveSelection
        UpdateControls()
    End Sub

    Sub bnSubtitleUp_Click(sender As Object, e As EventArgs) Handles bnSubtitleUp.Click
        dgvSubtitles.MoveSelectionUp
        UpdateControls()
    End Sub

    Sub bnSubtitleDown_Click(sender As Object, e As EventArgs) Handles bnSubtitleDown.Click
        dgvSubtitles.MoveSelectionDown
        UpdateControls()
    End Sub

    Sub bnSubtitleSetNames_Click(sender As Object, e As EventArgs) Handles bnSubtitleSetNames.Click
        Using td As New TaskDialog(Of Integer)
            td.MainInstruction = "Set names for all streams."
            td.AddCommand("Set language in English", 1)

            If CultureInfo.CurrentCulture.NeutralCulture.TwoLetterISOLanguageName <> "en" Then
                td.AddCommand("Set language in " + CultureInfo.CurrentCulture.NeutralCulture.DisplayName, 2)
            End If

            Select Case td.Show
                Case 1
                    For Each i In SubtitleItems
                        i.Title = i.Language.CultureInfo.EnglishName

                        If i.Forced Then
                            i.Title += " (forced)"
                        End If
                    Next

                    SubtitleBindingSource.ResetBindings(False)
                Case 2
                    For Each i In SubtitleItems
                        i.Title = i.Language.CultureInfo.NeutralCulture.DisplayName

                        If i.Forced Then
                            i.Title += " (forced)"
                        End If
                    Next

                    SubtitleBindingSource.ResetBindings(False)
            End Select
        End Using
    End Sub

    Sub bnSubtitleBDSup2SubPP_Click(sender As Object, e As EventArgs) Handles bnSubtitleBDSup2SubPP.Click
        Try
            Dim st = SubtitleItems(dgvSubtitles.CurrentRow.Index).Subtitle
            Dim fp = st.Path

            If fp.Ext = "idx" Then
                fp = p.TempDir + p.TargetFile.Base + "_temp.idx"
                Regex.Replace(st.Path.ReadAllText, "langidx: \d+", "langidx: " + st.IndexIDX.ToString).WriteFileDefault(fp)
                FileHelp.Copy(st.Path.DirAndBase + ".sub", fp.DirAndBase + ".sub")
            End If

            g.ShellExecute(Package.BDSup2SubPP.Path, """" + fp + """")
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Sub bnSubtitleEdit_Click(sender As Object, e As EventArgs) Handles bnSubtitleEdit.Click
        Try
            Dim st = SubtitleItems(dgvSubtitles.CurrentRow.Index).Subtitle
            Dim fp = st.Path

            If fp.ExtFull = ".idx" Then
                fp = p.TempDir + p.TargetFile.Base + "_temp.idx"
                Regex.Replace(st.Path.ReadAllText, "langidx: \d+", "langidx: " + st.IndexIDX.ToString).WriteFileDefault(fp)
                FileHelp.Copy(st.Path.DirAndBase + ".sub", fp.DirAndBase + ".sub")
            End If

            g.ShellExecute(Package.SubtitleEdit.Path, fp.Escape)
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Sub dgvSubtitles_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvSubtitles.DataBindingComplete
        UpdateControls()
    End Sub

    Private Sub bnAudioEncodeAll_Click(sender As Object, e As EventArgs) Handles bnAudioEncodeAll.Click
        Dim OutPath As String
        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select output directory or cancel to use source path"
            dialog.UseDescriptionForTitle = True
            dialog.RootFolder = Environment.SpecialFolder.MyComputer
            dialog.SelectedPath = DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File.Dir

            If dialog.ShowDialog = DialogResult.OK Then
                OutPath = dialog.SelectedPath.FixDir
                'Else
                '  dialog.Dispose()
                '  Exit Sub
            End If
        End Using

        If MsgQuestion("Confirm to process all tracks") = DialogResult.OK Then
            If OutPath Is Nothing Then OutPath = ""
            Try
                Dim actions As New List(Of Action)
                For Each af In AudioBindingSource
                    actions.Add(Sub() EncodeAudio(DirectCast(af, AudioProfile), OutPath))
                Next
                Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = s.ParallelProcsNum}, actions.ToArray)
            Catch ex As AggregateException
                ExceptionDispatchInfo.Capture(ex.InnerExceptions(0)).Throw()
            End Try
        End If
    End Sub

    Private Sub bnAudioEncodeSelected_Click(sender As Object, e As EventArgs) Handles bnAudioEncodeSelected.Click
        Dim OutPath As String
        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select output directory or cancel to use source path"
            dialog.UseDescriptionForTitle = True
            dialog.RootFolder = Environment.SpecialFolder.MyComputer
            dialog.SelectedPath = DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File.Dir

            If dialog.ShowDialog = DialogResult.OK Then
                OutPath = dialog.SelectedPath.FixDir
                'Else
                'dialog.Dispose()
                'Exit Sub
            End If
        End Using

        If OutPath Is Nothing Then OutPath = ""
        If MsgQuestion("Confirm to process selected track") = DialogResult.OK Then
            EncodeAudio(DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile), OutPath)
        End If
    End Sub
    Private Sub EncodeAudio(ap As AudioProfile, Optional OutPath As String = "")
        Try
            If p.TempDir = "" OrElse p.TempDir.Contains("%") Then
                p.TempDir = ap.File.Dir
            End If
            If OutPath <> "" Then
                p.TempDir = OutPath
            End If

            ap = ObjectHelp.GetCopy(ap)
            Audio.Process(ap)
            ap.Encode()
            p.SourceFile = p.TempDir
            Log.Save(p)
        Catch
        End Try
    End Sub

    Private Sub bnAudioRemoveAll_Click(sender As Object, e As EventArgs) Handles bnAudioRemoveAll.Click
        If dgvAudio.Rows.Count > 1 Then
            If MsgQuestion("Remove all ?") = DialogResult.OK Then
                dgvAudio.Rows.Clear()
                UpdateControls()
            End If
        Else
            dgvAudio.Rows.Clear()
            UpdateControls()
        End If
    End Sub
End Class
