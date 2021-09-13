Imports System.Collections.Immutable
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Runtime
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks

Imports JM.LinqFaster
Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic

Public Class MainForm
    Inherits FormBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        ImageHelp.DisposeFonts()
        If disposing Then
            components?.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Public WithEvents tbAudioFile0 As StaxRip.UI.TextBoxEx
    Public WithEvents tbAudioFile1 As StaxRip.UI.TextBoxEx
    Public WithEvents llEditAudio1 As ButtonLabel
    Public WithEvents llEditAudio0 As ButtonLabel
    Public WithEvents bnNext As ButtonEx
    Public WithEvents tbSourceFile As StaxRip.UI.TextBoxEx
    Public WithEvents tbTargetFile As StaxRip.UI.TextBoxEx
    Public WithEvents gbAssistant As System.Windows.Forms.GroupBox
    Public WithEvents lgbFilters As LinkGroupBox
    Public WithEvents tbTargetSize As System.Windows.Forms.TextBox
    Public WithEvents laBitrate As System.Windows.Forms.Label
    Public WithEvents tbBitrate As System.Windows.Forms.TextBox
    Public WithEvents lTarget1 As System.Windows.Forms.Label
    Public WithEvents lSource1 As System.Windows.Forms.Label
    Public WithEvents lgbResize As LinkGroupBox
    Public WithEvents lPixel As System.Windows.Forms.Label
    Public WithEvents lPixelText As System.Windows.Forms.Label
    Public WithEvents tbResize As System.Windows.Forms.TrackBar
    Public WithEvents lZoom As System.Windows.Forms.Label
    Public WithEvents lZoomText As System.Windows.Forms.Label
    Public WithEvents tbTargetHeight As System.Windows.Forms.TextBox
    Public WithEvents tbTargetWidth As System.Windows.Forms.TextBox
    Public WithEvents lTargetHeight As System.Windows.Forms.Label
    Public WithEvents lTargetWidth As System.Windows.Forms.Label
    Public WithEvents lDAR As System.Windows.Forms.Label
    Public WithEvents blTargetDarText As ButtonLabel
    Public WithEvents lCrop As System.Windows.Forms.Label
    Public WithEvents lCropText As System.Windows.Forms.Label
    Public WithEvents lgbSource As LinkGroupBox
    Public WithEvents lSource2 As System.Windows.Forms.Label
    Public WithEvents lgbTarget As LinkGroupBox
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents laTip As System.Windows.Forms.Label
    Public WithEvents lgbEncoder As LinkGroupBox
    Public WithEvents laTarget2 As System.Windows.Forms.Label
    Public WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Public WithEvents llAudioProfile0 As ButtonLabel
    Public WithEvents llAudioProfile1 As ButtonLabel
    Public WithEvents pnEncoder As System.Windows.Forms.Panel
    Public WithEvents FiltersListView As StaxRip.FiltersListView
    Public WithEvents blFilesize As ButtonLabel
    Public WithEvents llMuxer As ButtonLabel
    Public WithEvents lPAR As StaxRip.UI.LabelEx
    Public WithEvents blTargetParText As ButtonLabel
    Public WithEvents lAspectRatioError As StaxRip.UI.LabelEx
    Public WithEvents lAspectRatioErrorText As StaxRip.UI.LabelEx
    Public WithEvents gbAudio As System.Windows.Forms.GroupBox
    Public WithEvents blSourceParText As ButtonLabel
    Public WithEvents lSourcePAR As System.Windows.Forms.Label
    Public WithEvents lSourceDar As System.Windows.Forms.Label
    Public WithEvents lSAR As StaxRip.UI.LabelEx
    Public WithEvents lSarText As StaxRip.UI.LabelEx
    Friend WithEvents tlpTarget As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpSource As TableLayoutPanel
    Friend WithEvents tlpResizeValues As TableLayoutPanel
    Friend WithEvents tlpResize As TableLayoutPanel
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents tlpAssistant As TableLayoutPanel
    Friend WithEvents blSourceDarText As ButtonLabel
    Friend WithEvents tlpSourceValues As TableLayoutPanel
    Public WithEvents TipProvider As StaxRip.UI.TipProvider

    ' <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.bnNext = New StaxRip.UI.ButtonEx()
        Me.llEditAudio0 = New StaxRip.UI.ButtonLabel()
        Me.gbAssistant = New System.Windows.Forms.GroupBox()
        Me.tlpAssistant = New System.Windows.Forms.TableLayoutPanel()
        Me.laTip = New System.Windows.Forms.Label()
        Me.llEditAudio1 = New StaxRip.UI.ButtonLabel()
        Me.gbAudio = New System.Windows.Forms.GroupBox()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.tbAudioFile0 = New StaxRip.UI.TextBoxEx()
        Me.tbAudioFile1 = New StaxRip.UI.TextBoxEx()
        Me.llAudioProfile1 = New StaxRip.UI.ButtonLabel()
        Me.llAudioProfile0 = New StaxRip.UI.ButtonLabel()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.lgbTarget = New StaxRip.UI.LinkGroupBox()
        Me.tlpTarget = New System.Windows.Forms.TableLayoutPanel()
        Me.tbTargetFile = New StaxRip.UI.TextBoxEx()
        Me.laTarget2 = New System.Windows.Forms.Label()
        Me.tbTargetSize = New System.Windows.Forms.TextBox()
        Me.lTarget1 = New System.Windows.Forms.Label()
        Me.tbBitrate = New System.Windows.Forms.TextBox()
        Me.laBitrate = New System.Windows.Forms.Label()
        Me.blFilesize = New StaxRip.UI.ButtonLabel()
        Me.lgbSource = New StaxRip.UI.LinkGroupBox()
        Me.tlpSource = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpSourceValues = New System.Windows.Forms.TableLayoutPanel()
        Me.blSourceDarText = New StaxRip.UI.ButtonLabel()
        Me.lSourcePAR = New System.Windows.Forms.Label()
        Me.blSourceParText = New StaxRip.UI.ButtonLabel()
        Me.lCrop = New System.Windows.Forms.Label()
        Me.lCropText = New System.Windows.Forms.Label()
        Me.lSourceDar = New System.Windows.Forms.Label()
        Me.tbSourceFile = New StaxRip.UI.TextBoxEx()
        Me.lSource1 = New System.Windows.Forms.Label()
        Me.lSource2 = New System.Windows.Forms.Label()
        Me.lgbResize = New StaxRip.UI.LinkGroupBox()
        Me.tlpResize = New System.Windows.Forms.TableLayoutPanel()
        Me.lTargetWidth = New System.Windows.Forms.Label()
        Me.tlpResizeValues = New System.Windows.Forms.TableLayoutPanel()
        Me.blTargetDarText = New StaxRip.UI.ButtonLabel()
        Me.lAspectRatioError = New StaxRip.UI.LabelEx()
        Me.lPAR = New StaxRip.UI.LabelEx()
        Me.lZoom = New System.Windows.Forms.Label()
        Me.lSarText = New StaxRip.UI.LabelEx()
        Me.lPixel = New System.Windows.Forms.Label()
        Me.blTargetParText = New StaxRip.UI.ButtonLabel()
        Me.lAspectRatioErrorText = New StaxRip.UI.LabelEx()
        Me.lDAR = New System.Windows.Forms.Label()
        Me.lZoomText = New System.Windows.Forms.Label()
        Me.lPixelText = New System.Windows.Forms.Label()
        Me.lSAR = New StaxRip.UI.LabelEx()
        Me.tbResize = New System.Windows.Forms.TrackBar()
        Me.tbTargetWidth = New System.Windows.Forms.TextBox()
        Me.tbTargetHeight = New System.Windows.Forms.TextBox()
        Me.lTargetHeight = New System.Windows.Forms.Label()
        Me.lgbFilters = New StaxRip.UI.LinkGroupBox()
        Me.FiltersListView = New StaxRip.FiltersListView(Me.components)
        Me.lgbEncoder = New StaxRip.UI.LinkGroupBox()
        Me.llMuxer = New StaxRip.UI.ButtonLabel()
        Me.pnEncoder = New System.Windows.Forms.Panel()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.gbAssistant.SuspendLayout()
        Me.tlpAssistant.SuspendLayout()
        Me.gbAudio.SuspendLayout()
        FiltersListView.BeginUpdate()
        Me.tlpAudio.SuspendLayout()
        Me.lgbTarget.SuspendLayout()
        Me.tlpTarget.SuspendLayout()
        Me.lgbSource.SuspendLayout()
        Me.tlpSource.SuspendLayout()
        Me.tlpSourceValues.SuspendLayout()
        Me.lgbResize.SuspendLayout()
        Me.tlpResize.SuspendLayout()
        Me.tlpResizeValues.SuspendLayout()
        CType(Me.tbResize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.lgbFilters.SuspendLayout()
        Me.lgbEncoder.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnNext
        '
        Me.bnNext.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.bnNext.Location = New System.Drawing.Point(597, 6)
        Me.bnNext.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.bnNext.Size = New System.Drawing.Size(80, 33)
        Me.bnNext.Text = "Next"
        '
        'llEditAudio0
        '
        Me.llEditAudio0.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.llEditAudio0.AutoSize = True
        Me.llEditAudio0.LinkColor = System.Drawing.Color.Empty
        Me.llEditAudio0.Location = New System.Drawing.Point(650, 4)
        Me.llEditAudio0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.llEditAudio0.Name = "llEditAudio0"
        Me.llEditAudio0.Size = New System.Drawing.Size(27, 15)
        Me.llEditAudio0.TabIndex = 20
        Me.llEditAudio0.TabStop = True
        Me.llEditAudio0.Text = "Edit"
        Me.llEditAudio0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAssistant
        '
        Me.gbAssistant.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.gbAssistant, 4)
        Me.gbAssistant.Controls.Add(Me.tlpAssistant)
        Me.gbAssistant.Location = New System.Drawing.Point(3, 399)
        Me.gbAssistant.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.gbAssistant.Name = "gbAssistant"
        Me.gbAssistant.Padding = New System.Windows.Forms.Padding(2, 0, 2, 2)
        Me.gbAssistant.Size = New System.Drawing.Size(683, 64)
        Me.gbAssistant.TabIndex = 44
        Me.gbAssistant.TabStop = False
        Me.gbAssistant.Text = "Assistant"
        '
        'tlpAssistant
        '
        Me.tlpAssistant.ColumnCount = 2
        Me.tlpAssistant.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAssistant.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAssistant.Controls.Add(Me.laTip, 0, 0)
        Me.tlpAssistant.Controls.Add(Me.bnNext, 1, 0)
        Me.tlpAssistant.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAssistant.Location = New System.Drawing.Point(2, 16)
        Me.tlpAssistant.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAssistant.Name = "tlpAssistant"
        Me.tlpAssistant.RowCount = 1
        Me.tlpAssistant.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAssistant.Size = New System.Drawing.Size(679, 46)
        Me.tlpAssistant.TabIndex = 62
        '
        'laTip
        '
        Me.laTip.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laTip.Location = New System.Drawing.Point(0, 0)
        Me.laTip.Margin = New System.Windows.Forms.Padding(0)
        Me.laTip.Name = "laTip"
        Me.laTip.Size = New System.Drawing.Size(597, 46)
        Me.laTip.TabIndex = 40
        Me.laTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llEditAudio1
        '
        Me.llEditAudio1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.llEditAudio1.AutoSize = True
        Me.llEditAudio1.LinkColor = System.Drawing.Color.Empty
        Me.llEditAudio1.Location = New System.Drawing.Point(650, 30)
        Me.llEditAudio1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.llEditAudio1.Name = "llEditAudio1"
        Me.llEditAudio1.Size = New System.Drawing.Size(27, 15)
        Me.llEditAudio1.TabIndex = 21
        Me.llEditAudio1.TabStop = True
        Me.llEditAudio1.Text = "Edit"
        Me.llEditAudio1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAudio
        '
        Me.gbAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.gbAudio, 4)
        Me.gbAudio.Controls.Add(Me.tlpAudio)
        Me.gbAudio.Location = New System.Drawing.Point(3, 329)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New System.Windows.Forms.Padding(2, 0, 2, 2)
        Me.gbAudio.Size = New System.Drawing.Size(683, 70)
        Me.gbAudio.TabIndex = 59
        Me.gbAudio.TabStop = False
        Me.gbAudio.Text = "Audio"
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 3
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.Controls.Add(Me.llEditAudio1, 2, 1)
        Me.tlpAudio.Controls.Add(Me.tbAudioFile0, 0, 0)
        Me.tlpAudio.Controls.Add(Me.llEditAudio0, 2, 0)
        Me.tlpAudio.Controls.Add(Me.tbAudioFile1, 0, 1)
        Me.tlpAudio.Controls.Add(Me.llAudioProfile1, 1, 1)
        Me.tlpAudio.Controls.Add(Me.llAudioProfile0, 1, 0)
        Me.tlpAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(2, 16)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 2
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.0!))
        Me.tlpAudio.Size = New System.Drawing.Size(679, 52)
        Me.tlpAudio.TabIndex = 62
        '
        'tbAudioFile0
        '
        Me.tbAudioFile0.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile0.Location = New System.Drawing.Point(2, 0)
        Me.tbAudioFile0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.tbAudioFile0.Size = New System.Drawing.Size(628, 23)
        '
        'tbAudioFile1
        '
        Me.tbAudioFile1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile1.Location = New System.Drawing.Point(2, 26)
        Me.tbAudioFile1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.tbAudioFile1.Size = New System.Drawing.Size(628, 23)
        '
        'llAudioProfile1
        '
        Me.llAudioProfile1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.llAudioProfile1.AutoSize = True
        Me.llAudioProfile1.LinkColor = System.Drawing.Color.Empty
        Me.llAudioProfile1.Location = New System.Drawing.Point(634, 30)
        Me.llAudioProfile1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.llAudioProfile1.Name = "llAudioProfile1"
        Me.llAudioProfile1.Size = New System.Drawing.Size(12, 15)
        Me.llAudioProfile1.TabIndex = 26
        Me.llAudioProfile1.Text = "-"
        Me.llAudioProfile1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llAudioProfile0
        '
        Me.llAudioProfile0.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.llAudioProfile0.AutoSize = True
        Me.llAudioProfile0.LinkColor = System.Drawing.Color.Empty
        Me.llAudioProfile0.Location = New System.Drawing.Point(634, 4)
        Me.llAudioProfile0.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.llAudioProfile0.Name = "llAudioProfile0"
        Me.llAudioProfile0.Size = New System.Drawing.Size(12, 15)
        Me.llAudioProfile0.TabIndex = 25
        Me.llAudioProfile0.Text = "-"
        Me.llAudioProfile0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MenuStrip
        '
        Me.MenuStrip.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MenuStrip.AutoSize = False
        Me.tlpMain.SetColumnSpan(Me.MenuStrip, 4)
        Me.MenuStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Padding = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip.Size = New System.Drawing.Size(689, 27)
        Me.MenuStrip.TabIndex = 60
        Me.MenuStrip.Text = "MenuStrip"
        '
        'lgbTarget
        '
        Me.lgbTarget.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbTarget.Color = System.Drawing.Color.Empty
        Me.tlpMain.SetColumnSpan(Me.lgbTarget, 2)
        Me.lgbTarget.Controls.Add(Me.tlpTarget)
        Me.lgbTarget.Location = New System.Drawing.Point(345, 27)
        Me.lgbTarget.Margin = New System.Windows.Forms.Padding(2, 0, 3, 0)
        Me.lgbTarget.Name = "lgbTarget"
        Me.lgbTarget.Padding = New System.Windows.Forms.Padding(2, 0, 2, 2)
        Me.lgbTarget.Size = New System.Drawing.Size(341, 119)
        Me.lgbTarget.TabIndex = 58
        Me.lgbTarget.TabStop = False
        Me.lgbTarget.Text = "Target "
        '
        'tlpTarget
        '
        Me.tlpTarget.ColumnCount = 4
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.Controls.Add(Me.tbTargetFile, 0, 0)
        Me.tlpTarget.Controls.Add(Me.laTarget2, 0, 3)
        Me.tlpTarget.Controls.Add(Me.tbTargetSize, 1, 1)
        Me.tlpTarget.Controls.Add(Me.lTarget1, 0, 2)
        Me.tlpTarget.Controls.Add(Me.tbBitrate, 3, 1)
        Me.tlpTarget.Controls.Add(Me.laBitrate, 2, 1)
        Me.tlpTarget.Controls.Add(Me.blFilesize, 0, 1)
        Me.tlpTarget.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpTarget.Location = New System.Drawing.Point(2, 16)
        Me.tlpTarget.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpTarget.Name = "tlpTarget"
        Me.tlpTarget.RowCount = 4
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.Size = New System.Drawing.Size(337, 101)
        Me.tlpTarget.TabIndex = 62
        '
        'tbTargetFile
        '
        Me.tbTargetFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.tbTargetFile, 4)
        Me.tbTargetFile.Location = New System.Drawing.Point(2, 1)
        Me.tbTargetFile.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.tbTargetFile.Size = New System.Drawing.Size(333, 23)
        '
        'laTarget2
        '
        Me.laTarget2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.laTarget2, 4)
        Me.laTarget2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.laTarget2.Location = New System.Drawing.Point(2, 78)
        Me.laTarget2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.laTarget2.Name = "laTarget2"
        Me.laTarget2.Size = New System.Drawing.Size(333, 20)
        Me.laTarget2.TabIndex = 47
        Me.laTarget2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetSize
        '
        Me.tbTargetSize.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetSize.Location = New System.Drawing.Point(84, 26)
        Me.tbTargetSize.Margin = New System.Windows.Forms.Padding(0)
        Me.tbTargetSize.Name = "tbTargetSize"
        Me.tbTargetSize.Size = New System.Drawing.Size(48, 23)
        Me.tbTargetSize.TabIndex = 55
        Me.tbTargetSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTarget1
        '
        Me.lTarget1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.lTarget1, 4)
        Me.lTarget1.Location = New System.Drawing.Point(2, 52)
        Me.lTarget1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lTarget1.Name = "lTarget1"
        Me.lTarget1.Size = New System.Drawing.Size(333, 20)
        Me.lTarget1.TabIndex = 39
        Me.lTarget1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBitrate
        '
        Me.tbBitrate.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbBitrate.Location = New System.Drawing.Point(252, 26)
        Me.tbBitrate.Margin = New System.Windows.Forms.Padding(0)
        Me.tbBitrate.Name = "tbBitrate"
        Me.tbBitrate.Size = New System.Drawing.Size(49, 23)
        Me.tbBitrate.TabIndex = 41
        Me.tbBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'laBitrate
        '
        Me.laBitrate.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laBitrate.Location = New System.Drawing.Point(168, 25)
        Me.laBitrate.Margin = New System.Windows.Forms.Padding(0)
        Me.laBitrate.Name = "laBitrate"
        Me.laBitrate.Size = New System.Drawing.Size(84, 25)
        Me.laBitrate.TabIndex = 42
        Me.laBitrate.Text = "Video Bitrate:"
        Me.laBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blFilesize
        '
        Me.blFilesize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blFilesize.LinkColor = System.Drawing.Color.Empty
        Me.blFilesize.Location = New System.Drawing.Point(2, 25)
        Me.blFilesize.Margin = New System.Windows.Forms.Padding(2, 0, 0, 0)
        Me.blFilesize.Name = "blFilesize"
        Me.blFilesize.Size = New System.Drawing.Size(82, 25)
        Me.blFilesize.TabIndex = 59
        Me.blFilesize.TabStop = True
        Me.blFilesize.Text = "Size:"
        Me.blFilesize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lgbSource
        '
        Me.lgbSource.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbSource.Color = System.Drawing.Color.Empty
        Me.tlpMain.SetColumnSpan(Me.lgbSource, 2)
        Me.lgbSource.Controls.Add(Me.tlpSource)
        Me.lgbSource.Location = New System.Drawing.Point(3, 27)
        Me.lgbSource.Margin = New System.Windows.Forms.Padding(3, 0, 2, 0)
        Me.lgbSource.Name = "lgbSource"
        Me.lgbSource.Padding = New System.Windows.Forms.Padding(2, 0, 2, 2)
        Me.lgbSource.Size = New System.Drawing.Size(338, 119)
        Me.lgbSource.TabIndex = 57
        Me.lgbSource.TabStop = False
        Me.lgbSource.Text = "Source "
        '
        'tlpSource
        '
        Me.tlpSource.ColumnCount = 1
        Me.tlpSource.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpSource.Controls.Add(Me.tlpSourceValues, 0, 3)
        Me.tlpSource.Controls.Add(Me.tbSourceFile, 0, 0)
        Me.tlpSource.Controls.Add(Me.lSource1, 0, 1)
        Me.tlpSource.Controls.Add(Me.lSource2, 0, 2)
        Me.tlpSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpSource.Location = New System.Drawing.Point(2, 16)
        Me.tlpSource.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpSource.Name = "tlpSource"
        Me.tlpSource.RowCount = 4
        Me.tlpSource.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpSource.Size = New System.Drawing.Size(334, 101)
        Me.tlpSource.TabIndex = 62
        '
        'tlpSourceValues
        '
        Me.tlpSourceValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpSourceValues.ColumnCount = 7
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpSourceValues.Controls.Add(Me.blSourceDarText, 4, 0)
        Me.tlpSourceValues.Controls.Add(Me.lSourcePAR, 3, 0)
        Me.tlpSourceValues.Controls.Add(Me.blSourceParText, 2, 0)
        Me.tlpSourceValues.Controls.Add(Me.lCrop, 1, 0)
        Me.tlpSourceValues.Controls.Add(Me.lCropText, 0, 0)
        Me.tlpSourceValues.Controls.Add(Me.lSourceDar, 5, 0)
        Me.tlpSourceValues.Location = New System.Drawing.Point(0, 75)
        Me.tlpSourceValues.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpSourceValues.Name = "tlpSourceValues"
        Me.tlpSourceValues.RowCount = 1
        Me.tlpSourceValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpSourceValues.Size = New System.Drawing.Size(334, 26)
        Me.tlpSourceValues.TabIndex = 1
        '
        'blSourceDarText
        '
        Me.blSourceDarText.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.blSourceDarText.AutoSize = True
        Me.blSourceDarText.LinkColor = System.Drawing.Color.Empty
        Me.blSourceDarText.Location = New System.Drawing.Point(151, 5)
        Me.blSourceDarText.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.blSourceDarText.Name = "blSourceDarText"
        Me.blSourceDarText.Size = New System.Drawing.Size(33, 15)
        Me.blSourceDarText.TabIndex = 51
        Me.blSourceDarText.Text = "DAR:"
        Me.blSourceDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourcePAR
        '
        Me.lSourcePAR.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lSourcePAR.AutoSize = True
        Me.lSourcePAR.Location = New System.Drawing.Point(135, 5)
        Me.lSourcePAR.Name = "lSourcePAR"
        Me.lSourcePAR.Size = New System.Drawing.Size(12, 15)
        Me.lSourcePAR.TabIndex = 47
        Me.lSourcePAR.Text = "-"
        Me.lSourcePAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blSourceParText
        '
        Me.blSourceParText.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.blSourceParText.AutoSize = True
        Me.blSourceParText.LinkColor = System.Drawing.Color.Empty
        Me.blSourceParText.Location = New System.Drawing.Point(98, 5)
        Me.blSourceParText.Name = "blSourceParText"
        Me.blSourceParText.Size = New System.Drawing.Size(31, 15)
        Me.blSourceParText.TabIndex = 45
        Me.blSourceParText.TabStop = True
        Me.blSourceParText.Text = "PAR:"
        Me.blSourceParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCrop
        '
        Me.lCrop.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lCrop.AutoSize = True
        Me.lCrop.Location = New System.Drawing.Point(41, 5)
        Me.lCrop.Name = "lCrop"
        Me.lCrop.Size = New System.Drawing.Size(51, 15)
        Me.lCrop.TabIndex = 16
        Me.lCrop.Text = "disabled"
        Me.lCrop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCropText
        '
        Me.lCropText.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lCropText.AutoSize = True
        Me.lCropText.Location = New System.Drawing.Point(1, 5)
        Me.lCropText.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lCropText.Name = "lCropText"
        Me.lCropText.Size = New System.Drawing.Size(36, 15)
        Me.lCropText.TabIndex = 14
        Me.lCropText.TabStop = True
        Me.lCropText.Text = "Crop:"
        Me.lCropText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceDar
        '
        Me.lSourceDar.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lSourceDar.AutoSize = True
        Me.lSourceDar.Location = New System.Drawing.Point(186, 5)
        Me.lSourceDar.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lSourceDar.Name = "lSourceDar"
        Me.lSourceDar.Size = New System.Drawing.Size(12, 15)
        Me.lSourceDar.TabIndex = 50
        Me.lSourceDar.Text = "-"
        Me.lSourceDar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSourceFile
        '
        Me.tbSourceFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceFile.Location = New System.Drawing.Point(2, 1)
        Me.tbSourceFile.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.tbSourceFile.Size = New System.Drawing.Size(330, 23)
        '
        'lSource1
        '
        Me.lSource1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lSource1.Location = New System.Drawing.Point(2, 28)
        Me.lSource1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lSource1.Name = "lSource1"
        Me.lSource1.Size = New System.Drawing.Size(330, 19)
        Me.lSource1.TabIndex = 41
        Me.lSource1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSource2
        '
        Me.lSource2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lSource2.Location = New System.Drawing.Point(2, 53)
        Me.lSource2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lSource2.Name = "lSource2"
        Me.lSource2.Size = New System.Drawing.Size(330, 19)
        Me.lSource2.TabIndex = 33
        Me.lSource2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lgbResize
        '
        Me.lgbResize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbResize.Color = System.Drawing.Color.Empty
        Me.tlpMain.SetColumnSpan(Me.lgbResize, 2)
        Me.lgbResize.Controls.Add(Me.tlpResize)
        Me.lgbResize.Location = New System.Drawing.Point(231, 146)
        Me.lgbResize.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lgbResize.Name = "lgbResize"
        Me.lgbResize.Padding = New System.Windows.Forms.Padding(2, 0, 2, 2)
        Me.lgbResize.Size = New System.Drawing.Size(224, 183)
        Me.lgbResize.TabIndex = 55
        Me.lgbResize.TabStop = False
        Me.lgbResize.Text = "Resize "
        '
        'tlpResize
        '
        Me.tlpResize.ColumnCount = 4
        Me.tlpResize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpResize.Controls.Add(Me.lTargetWidth, 0, 0)
        Me.tlpResize.Controls.Add(Me.tlpResizeValues, 0, 2)
        Me.tlpResize.Controls.Add(Me.tbResize, 0, 1)
        Me.tlpResize.Controls.Add(Me.tbTargetWidth, 1, 0)
        Me.tlpResize.Controls.Add(Me.tbTargetHeight, 3, 0)
        Me.tlpResize.Controls.Add(Me.lTargetHeight, 2, 0)
        Me.tlpResize.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpResize.Location = New System.Drawing.Point(2, 16)
        Me.tlpResize.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpResize.Name = "tlpResize"
        Me.tlpResize.RowCount = 3
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpResize.Size = New System.Drawing.Size(220, 165)
        Me.tlpResize.TabIndex = 63
        '
        'lTargetWidth
        '
        Me.lTargetWidth.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lTargetWidth.AutoSize = True
        Me.lTargetWidth.Location = New System.Drawing.Point(1, 6)
        Me.lTargetWidth.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lTargetWidth.Name = "lTargetWidth"
        Me.lTargetWidth.Size = New System.Drawing.Size(42, 15)
        Me.lTargetWidth.TabIndex = 37
        Me.lTargetWidth.Text = "Width:"
        '
        'tlpResizeValues
        '
        Me.tlpResizeValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpResizeValues.ColumnCount = 4
        Me.tlpResize.SetColumnSpan(Me.tlpResizeValues, 4)
        Me.tlpResizeValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpResizeValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpResizeValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpResizeValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpResizeValues.Controls.Add(Me.blTargetDarText, 0, 0)
        Me.tlpResizeValues.Controls.Add(Me.lAspectRatioError, 3, 2)
        Me.tlpResizeValues.Controls.Add(Me.lPAR, 1, 2)
        Me.tlpResizeValues.Controls.Add(Me.lZoom, 3, 1)
        Me.tlpResizeValues.Controls.Add(Me.lSarText, 0, 1)
        Me.tlpResizeValues.Controls.Add(Me.lPixel, 3, 0)
        Me.tlpResizeValues.Controls.Add(Me.blTargetParText, 0, 2)
        Me.tlpResizeValues.Controls.Add(Me.lAspectRatioErrorText, 2, 2)
        Me.tlpResizeValues.Controls.Add(Me.lDAR, 1, 0)
        Me.tlpResizeValues.Controls.Add(Me.lZoomText, 2, 1)
        Me.tlpResizeValues.Controls.Add(Me.lPixelText, 2, 0)
        Me.tlpResizeValues.Controls.Add(Me.lSAR, 1, 1)
        Me.tlpResizeValues.Location = New System.Drawing.Point(0, 54)
        Me.tlpResizeValues.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpResizeValues.Name = "tlpResizeValues"
        Me.tlpResizeValues.RowCount = 3
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.Size = New System.Drawing.Size(220, 111)
        Me.tlpResizeValues.TabIndex = 62
        '
        'blTargetDarText
        '
        Me.blTargetDarText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.blTargetDarText.AutoSize = True
        Me.blTargetDarText.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.blTargetDarText.LinkColor = System.Drawing.Color.Empty
        Me.blTargetDarText.Location = New System.Drawing.Point(0, 11)
        Me.blTargetDarText.Margin = New System.Windows.Forms.Padding(0)
        Me.blTargetDarText.Name = "blTargetDarText"
        Me.blTargetDarText.Size = New System.Drawing.Size(33, 15)
        Me.blTargetDarText.TabIndex = 23
        Me.blTargetDarText.Text = "DAR:"
        Me.blTargetDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioError
        '
        Me.lAspectRatioError.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lAspectRatioError.AutoSize = True
        Me.lAspectRatioError.Location = New System.Drawing.Point(87, 85)
        Me.lAspectRatioError.Margin = New System.Windows.Forms.Padding(0)
        Me.lAspectRatioError.Size = New System.Drawing.Size(12, 15)
        Me.lAspectRatioError.Text = "-"
        Me.lAspectRatioError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPAR
        '
        Me.lPAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPAR.AutoSize = True
        Me.lPAR.Location = New System.Drawing.Point(33, 85)
        Me.lPAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lPAR.Size = New System.Drawing.Size(12, 15)
        Me.lPAR.Text = "-"
        Me.lPAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoom
        '
        Me.lZoom.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lZoom.AutoSize = True
        Me.lZoom.Location = New System.Drawing.Point(87, 48)
        Me.lZoom.Margin = New System.Windows.Forms.Padding(0)
        Me.lZoom.Name = "lZoom"
        Me.lZoom.Size = New System.Drawing.Size(12, 15)
        Me.lZoom.TabIndex = 44
        Me.lZoom.Text = "-"
        Me.lZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSarText
        '
        Me.lSarText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lSarText.AutoSize = True
        Me.lSarText.Location = New System.Drawing.Point(0, 48)
        Me.lSarText.Margin = New System.Windows.Forms.Padding(0)
        Me.lSarText.Size = New System.Drawing.Size(31, 15)
        Me.lSarText.Text = "SAR:"
        Me.lSarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixel
        '
        Me.lPixel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPixel.AutoSize = True
        Me.lPixel.Location = New System.Drawing.Point(87, 11)
        Me.lPixel.Margin = New System.Windows.Forms.Padding(0)
        Me.lPixel.Name = "lPixel"
        Me.lPixel.Size = New System.Drawing.Size(12, 15)
        Me.lPixel.TabIndex = 50
        Me.lPixel.Text = "-"
        Me.lPixel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blTargetParText
        '
        Me.blTargetParText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.blTargetParText.AutoSize = True
        Me.blTargetParText.LinkColor = System.Drawing.Color.Empty
        Me.blTargetParText.Location = New System.Drawing.Point(0, 85)
        Me.blTargetParText.Margin = New System.Windows.Forms.Padding(0)
        Me.blTargetParText.Name = "blTargetParText"
        Me.blTargetParText.Size = New System.Drawing.Size(31, 15)
        Me.blTargetParText.TabIndex = 51
        Me.blTargetParText.Text = "PAR:"
        Me.blTargetParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioErrorText
        '
        Me.lAspectRatioErrorText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lAspectRatioErrorText.AutoSize = True
        Me.lAspectRatioErrorText.Location = New System.Drawing.Point(45, 85)
        Me.lAspectRatioErrorText.Margin = New System.Windows.Forms.Padding(0)
        Me.lAspectRatioErrorText.Size = New System.Drawing.Size(35, 15)
        Me.lAspectRatioErrorText.Text = "Error:"
        Me.lAspectRatioErrorText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDAR
        '
        Me.lDAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lDAR.AutoSize = True
        Me.lDAR.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lDAR.Location = New System.Drawing.Point(33, 11)
        Me.lDAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lDAR.Name = "lDAR"
        Me.lDAR.Size = New System.Drawing.Size(12, 15)
        Me.lDAR.TabIndex = 24
        Me.lDAR.Text = "-"
        Me.lDAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoomText
        '
        Me.lZoomText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lZoomText.AutoSize = True
        Me.lZoomText.Location = New System.Drawing.Point(45, 48)
        Me.lZoomText.Margin = New System.Windows.Forms.Padding(0)
        Me.lZoomText.Name = "lZoomText"
        Me.lZoomText.Size = New System.Drawing.Size(42, 15)
        Me.lZoomText.TabIndex = 42
        Me.lZoomText.Text = "Zoom:"
        Me.lZoomText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixelText
        '
        Me.lPixelText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPixelText.AutoSize = True
        Me.lPixelText.Location = New System.Drawing.Point(45, 11)
        Me.lPixelText.Margin = New System.Windows.Forms.Padding(0)
        Me.lPixelText.Name = "lPixelText"
        Me.lPixelText.Size = New System.Drawing.Size(35, 15)
        Me.lPixelText.TabIndex = 49
        Me.lPixelText.Text = "Pixel:"
        Me.lPixelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSAR
        '
        Me.lSAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lSAR.AutoSize = True
        Me.lSAR.Location = New System.Drawing.Point(33, 48)
        Me.lSAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lSAR.Size = New System.Drawing.Size(12, 15)
        Me.lSAR.Text = "-"
        Me.lSAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbResize
        '
        Me.tbResize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbResize.AutoSize = False
        Me.tbResize.BackColor = System.Drawing.SystemColors.Control
        Me.tlpResize.SetColumnSpan(Me.tbResize, 4)
        Me.tbResize.Location = New System.Drawing.Point(0, 27)
        Me.tbResize.Margin = New System.Windows.Forms.Padding(0)
        Me.tbResize.Name = "tbResize"
        Me.tbResize.Size = New System.Drawing.Size(220, 27)
        Me.tbResize.TabIndex = 46
        '
        'tbTargetWidth
        '
        Me.tbTargetWidth.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetWidth.Location = New System.Drawing.Point(56, 2)
        Me.tbTargetWidth.Margin = New System.Windows.Forms.Padding(1)
        Me.tbTargetWidth.Name = "tbTargetWidth"
        Me.tbTargetWidth.Size = New System.Drawing.Size(51, 23)
        Me.tbTargetWidth.TabIndex = 39
        Me.tbTargetWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbTargetHeight
        '
        Me.tbTargetHeight.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetHeight.Location = New System.Drawing.Point(166, 2)
        Me.tbTargetHeight.Margin = New System.Windows.Forms.Padding(1)
        Me.tbTargetHeight.Name = "tbTargetHeight"
        Me.tbTargetHeight.Size = New System.Drawing.Size(51, 23)
        Me.tbTargetHeight.TabIndex = 40
        Me.tbTargetHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTargetHeight
        '
        Me.lTargetHeight.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lTargetHeight.AutoSize = True
        Me.lTargetHeight.Location = New System.Drawing.Point(111, 6)
        Me.lTargetHeight.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lTargetHeight.Name = "lTargetHeight"
        Me.lTargetHeight.Size = New System.Drawing.Size(46, 15)
        Me.lTargetHeight.TabIndex = 38
        Me.lTargetHeight.Text = "Height:"
        '
        'lgbFilters
        '
        Me.lgbFilters.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbFilters.Color = System.Drawing.Color.Empty
        Me.lgbFilters.Controls.Add(Me.FiltersListView)
        Me.lgbFilters.Location = New System.Drawing.Point(3, 146)
        Me.lgbFilters.Margin = New System.Windows.Forms.Padding(3, 0, 2, 0)
        Me.lgbFilters.Name = "lgbFilters"
        Me.lgbFilters.Padding = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.lgbFilters.Size = New System.Drawing.Size(224, 183)
        Me.lgbFilters.TabIndex = 53
        Me.lgbFilters.TabStop = False
        Me.lgbFilters.Text = "Filters "
        '
        'FiltersListView
        '
        'Me.FiltersListView.AllowDrop = True
        'Me.FiltersListView.AutoCheckMode = StaxRip.UI.AutoCheckMode.None
        'Me.FiltersListView.CheckBoxes = True
        Me.FiltersListView.Dock = System.Windows.Forms.DockStyle.Fill
        'Me.FiltersListView.FullRowSelect = True
        Me.FiltersListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        'Me.FiltersListView.HideSelection = False
        Me.FiltersListView.IsLoading = False
        Me.FiltersListView.Location = New System.Drawing.Point(3, 17)
        'Me.FiltersListView.Margin = New System.Windows.Forms.Padding(1)
        'Me.FiltersListView.MultiSelect = False
        Me.FiltersListView.Name = "FiltersListView"
        Me.FiltersListView.Size = New System.Drawing.Size(218, 163)
        Me.FiltersListView.TabIndex = 0
        Me.FiltersListView.UseCompatibleStateImageBehavior = False
        'Me.FiltersListView.View = System.Windows.Forms.View.Details
        '
        'lgbEncoder
        '
        Me.lgbEncoder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbEncoder.Color = System.Drawing.Color.Empty
        Me.lgbEncoder.Controls.Add(Me.llMuxer)
        Me.lgbEncoder.Controls.Add(Me.pnEncoder)
        Me.lgbEncoder.Location = New System.Drawing.Point(459, 146)
        Me.lgbEncoder.Margin = New System.Windows.Forms.Padding(2, 0, 3, 0)
        Me.lgbEncoder.Name = "lgbEncoder"
        Me.lgbEncoder.Padding = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.lgbEncoder.Size = New System.Drawing.Size(227, 183)
        Me.lgbEncoder.TabIndex = 51
        Me.lgbEncoder.TabStop = False
        Me.lgbEncoder.Text = "Encoder "
        '
        'llMuxer
        '
        Me.llMuxer.AutoSize = True
        Me.llMuxer.LinkColor = System.Drawing.Color.Empty
        Me.llMuxer.Location = New System.Drawing.Point(172, 0)
        Me.llMuxer.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.llMuxer.Name = "llMuxer"
        Me.llMuxer.Size = New System.Drawing.Size(41, 15)
        Me.llMuxer.TabIndex = 1
        Me.llMuxer.TabStop = True
        Me.llMuxer.Text = "Muxer"
        '
        'pnEncoder
        '
        Me.pnEncoder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnEncoder.Location = New System.Drawing.Point(3, 17)
        Me.pnEncoder.Margin = New System.Windows.Forms.Padding(1)
        Me.pnEncoder.Name = "pnEncoder"
        Me.pnEncoder.Size = New System.Drawing.Size(221, 163)
        Me.pnEncoder.TabIndex = 0
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 4
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.tlpMain.Controls.Add(Me.lgbTarget, 2, 1)
        Me.tlpMain.Controls.Add(Me.gbAssistant, 0, 4)
        Me.tlpMain.Controls.Add(Me.gbAudio, 0, 3)
        Me.tlpMain.Controls.Add(Me.lgbSource, 0, 1)
        Me.tlpMain.Controls.Add(Me.lgbResize, 1, 2)
        Me.tlpMain.Controls.Add(Me.MenuStrip, 0, 0)
        Me.tlpMain.Controls.Add(Me.lgbFilters, 0, 2)
        Me.tlpMain.Controls.Add(Me.lgbEncoder, 3, 2)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 5
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 119.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67.0!))
        Me.tlpMain.Size = New System.Drawing.Size(689, 466)
        Me.tlpMain.TabIndex = 61
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        'Me.ClientSize = New System.Drawing.Size(689, 466)
        ScaleClientSize(43, 27.5, FontHeight) 'moved From New - WatchOut: Designer will wipe it
        Me.Controls.Add(Me.tlpMain)
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "MainForm"
        Me.Text = "StaxRip"
        Me.gbAssistant.ResumeLayout(False)
        Me.tlpAssistant.ResumeLayout(False)
        Me.gbAudio.ResumeLayout(False)
        FiltersListView.EndUpdate()
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        Me.lgbTarget.ResumeLayout(False)
        Me.lgbTarget.PerformLayout()
        Me.tlpTarget.ResumeLayout(False)
        Me.tlpTarget.PerformLayout()
        Me.lgbSource.ResumeLayout(False)
        Me.lgbSource.PerformLayout()
        Me.tlpSource.ResumeLayout(False)
        Me.tlpSource.PerformLayout()
        Me.tlpSourceValues.ResumeLayout(False)
        Me.tlpSourceValues.PerformLayout()
        Me.lgbResize.ResumeLayout(False)
        Me.lgbResize.PerformLayout()
        Me.tlpResize.ResumeLayout(False)
        Me.tlpResize.PerformLayout()
        Me.tlpResizeValues.ResumeLayout(False)
        Me.tlpResizeValues.PerformLayout()
        CType(Me.tbResize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.lgbFilters.ResumeLayout(False)
        Me.lgbFilters.PerformLayout()
        Me.lgbEncoder.ResumeLayout(False)
        Me.lgbEncoder.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public WithEvents CustomMainMenu As CustomMenu
    Public WithEvents CustomSizeMenu As CustomMenu
    Public CurrentAssistantTipKey As String
    Public BlockSubtitlesItemCheck As Boolean
    Public AssistantPassed As Boolean
    Public CommandManager As New CommandManager
    Public ForceClose As Boolean

    Private AudioMenu0 As ContextMenuStripEx
    Private AudioMenu1 As ContextMenuStripEx
    Private TargetAspectRatioMenu As ContextMenuStripEx
    Private SizeContextMenuStrip As ContextMenuStripEx
    Private EncoderMenu As ContextMenuStripEx
    Private ContainerMenu As ContextMenuStripEx
    Private SourceAspectRatioMenu As ContextMenuStripEx
    Private TargetFileMenu As ContextMenuStripEx
    Private SourceFileMenu As ContextMenuStripEx
    Private Audio0FileMenu As ContextMenuStripEx
    Private Audio1FileMenu As ContextMenuStripEx
    Private NextContextMenuStrip As ContextMenuStripEx
    Private BlockAviSynthItemCheck As Boolean
    Private CanChangeSize As Boolean = True
    Private CanChangeBitrate As Boolean = True
    Private CanIgnoreTip As Boolean = True
    Private IsLoading As Boolean = True
    Private BlockBitrate, BlockSize As Boolean
    Private SkipAssistant As Boolean
    Private BlockSourceTextBoxTextChanged As Boolean

    Sub New()
        AddHandler Application.ThreadException, AddressOf g.OnUnhandledException
        g.MainForm = Me
        LoadSettings()

        Dim cmInIcoT = Task.Run(Function()
                                    ToolStripMenuItemEx.UseTooltips = s.EnableTooltips
                                    PowerShell.InitCode =
                                   "Using namespace StaxRip;" & BR &
                                   "Using namespace StaxRip.UI;" & BR &
                                   "[Reflection.Assembly]::LoadWithPartialName(""StaxRip"")"

                                    ImageHelp.CreateFonts()
                                    CommandManager.AddCommandsFromObject(Me)
                                    CommandManager.AddCommandsFromObject(g.DefaultCommands)
                                    g.MenuSpace = " " '.Multiply(CInt(g.DPI / 96)) 'Optim. Test! Debug???
                                    g.DPI = DeviceDpi
                                    If Language.Languages IsNot Nothing Then
                                    End If

                                    If s.WriteDebugLog Then
                                        Dim filePath = Folder.Startup & "Debug.log"
                                        If File.Exists(filePath) Then File.Delete(filePath)
                                        Dim listener = New TextWriterTraceListener(filePath) With {.TraceOutputOptions = TraceOptions.ThreadId Or TraceOptions.DateTime}
                                        'Trace.Refresh() 'debug remove it
                                        Trace.Listeners.Add(listener) 'TODO : Add Dispose of TextWriterListener
                                        Trace.AutoFlush = True
                                    End If
                                    Return g.Icon
                                End Function)
        InitializeComponent()
        ' ScaleClientSize(43, 27.5, FontHeight) 'Moved to InitComp ******* !!!! Org. CliSize InInit: Me.ClientSize = New System.Drawing.Size(689, 466) 'WatchOut: Designer will wipe it

        If components Is Nothing Then components = New System.ComponentModel.Container

        With TipProvider
            .SetTip("Target Display Aspect Ratio", lDAR, blTargetDarText)
            .SetTip("Target Pixel Aspect Ratio", lPAR, blTargetParText)
            .SetTip("Target Storage Aspect Ratio", lSAR, lSarText)
            .SetTip("Imagesize in pixel (width x height)", lPixel, lPixelText)
            .SetTip("Zoom factor between source and target image size", lZoom, lZoomText)
            .SetTip("Press Ctrl while using the slider to resize anamorphic.", tbResize)
            .SetTip("Aspect Ratio Error", lAspectRatioError, lAspectRatioErrorText)
            .SetTip("Cropped image size", lCrop, lCropText)
            .SetTip("Source Display Aspect Ratio", lSourceDar, blSourceDarText)
            .SetTip("Source Pixel Aspect Ratio", lSourcePAR, blSourceParText)
            .SetTip("Target Video Bitrate in Kbps (Up/Down key)", tbBitrate, laBitrate)
            .SetTip("Target Image Width in pixel (Up/Down key)", tbTargetWidth, lTargetWidth)
            .SetTip("Target Image Height in pixel (Up/Down key)", tbTargetHeight, lTargetHeight)
            .SetTip("Target File Size (Up/Down key)", tbTargetSize)
            .SetTip("Source file", tbSourceFile)
            .SetTip("Target file", tbTargetFile)
            .SetTip("Opens audio settings for the current project/template", llEditAudio0, llEditAudio1)
            .SetTip("Shows audio profiles", llAudioProfile0)
            .SetTip("Shows audio profiles", llAudioProfile1)
            .SetTip("Shows a menu with Container/Muxer profiles", llMuxer)
            .SetTip("Shows a menu with video encoder profiles", lgbEncoder.Label)
            .SetTip("Shows a menu with AviSynth filter options", lgbFilters.Label)
        End With

        AudioMenu0 = New ContextMenuStripEx(components)
        AudioMenu1 = New ContextMenuStripEx(components)
        TargetAspectRatioMenu = New ContextMenuStripEx(components)
        SizeContextMenuStrip = New ContextMenuStripEx(components)
        EncoderMenu = New ContextMenuStripEx(components)
        ContainerMenu = New ContextMenuStripEx(components)
        SourceAspectRatioMenu = New ContextMenuStripEx(components)
        NextContextMenuStrip = New ContextMenuStripEx(components)
        TargetFileMenu = New ContextMenuStripEx(components)
        SourceFileMenu = New ContextMenuStripEx(components)
        Audio0FileMenu = New ContextMenuStripEx(components)
        Audio1FileMenu = New ContextMenuStripEx(components)

        tbTargetFile.ContextMenuStrip = TargetFileMenu
        tbSourceFile.ContextMenuStrip = SourceFileMenu
        tbAudioFile0.ContextMenuStrip = Audio0FileMenu
        tbAudioFile1.ContextMenuStrip = Audio1FileMenu

        Const rc = "right-click"
        tbAudioFile0.SendMessageCue(rc, False)
        tbAudioFile1.SendMessageCue(rc, False)
        tbSourceFile.SendMessageCue(rc, False)
        tbTargetFile.SendMessageCue(rc, False)

        AddHandler llEditAudio0.Click, Sub() AudioEdit0ToolStripMenuItemClick()
        AddHandler llEditAudio1.Click, Sub() AudioEdit1ToolStripMenuItemClick()

        'bnNext.AutoSize = True 'Added Experiment - Debug !!!!!! 202107 - UnComment Or make it bigger !!!
        'bnNext.AutoSizeMode = AutoSizeMode.GrowAndShrink
        'bnNext.MinimumSize = New Size(CInt(FontHeight * 3.5), CInt(FontHeight * 1.5))
        NextContextMenuStrip.SuspendLayout()
        NextContextMenuStrip.Items.AddRange({
        New ActionMenuItem("Add to top and open Jobs", Sub() AddJob(True, 0)),
        New ActionMenuItem("Add to bottom and open Jobs", Sub() AddJob(True, -1)),
        New ToolStripSeparator,
        New ActionMenuItem("Add to top w/o opening Jobs", Sub() AddJob(False, 0)),
        New ActionMenuItem("Add to bottom w/o opening Jobs", Sub() AddJob(False, -1))})
        NextContextMenuStrip.ResumeLayout(False)

        'components.Add(FiltersListView.ContextMenu)
        'Debug!!!!!!!!
        Dim mmm = Macro.GetMacros(True, True, True)
        Dim dddd = "c:\fb\se fji\f-nm.mkv"
        Dim tt = dddd.DirAndBase
        tt = ".".DirAndBase
        tt = ".".Base
        tt = dddd.Base
        Dim paaa = dddd.Parent
        paaa = paaa.Parent

        If s.UIScaleFactor <> 1 Then MenuStrip.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
        CustomMainMenu = New CustomMenu(AddressOf GetDefaultMainMenu, s.CustomMenuMainForm, CommandManager, MenuStrip)
        OpenProject(g.StartupTemplatePath)

        CustomMainMenu.AddKeyDownHandler(Me)
        If g.MenuSpace Is Nothing Then cmInIcoT.Wait()
        CustomMainMenu.BuildMenu()
        g.SetRenderer(MenuStrip)
        UpdateAudioMenu()

        CustomSizeMenu = New CustomMenu(AddressOf GetDefaultMenuSize, s.CustomMenuSize, CommandManager, SizeContextMenuStrip)
        CustomSizeMenu.AddKeyDownHandler(Me)
        CustomSizeMenu.BuildMenu()

        SetMenuStyle()

        Dim wsdgjkdfbjfgb = IsHandleCreated 'Debug !!!!!************************* DELIT
        Icon = cmInIcoT.Result
    End Sub

    Sub LoadSettings()
        Try
            s = SafeSerialization.Deserialize(New ApplicationSettings, g.SettingsFile)
        Catch ex As Exception
            Using td As New TaskDialog(Of String)
                td.MainInstruction = "The settings failed to load!"
                td.Content = ex.Message
                td.MainIcon = TaskDialogIcon.Error
                td.AddButton("Retry", "Retry")
                td.AddButton("Reset", "Reset")
                td.AddButton("Exit", "Exit")

                Select Case td.Show
                    Case "Retry"
                        LoadSettings()
                    Case "Reset"
                        If MsgQuestion("Are you sure you want to reset your settings? Your current settings will be lost!") = DialogResult.OK Then
                            s = New ApplicationSettings
                            s.Init()
                        Else
                            LoadSettings()
                        End If
                    Case Else
                        Process.GetCurrentProcess.Kill()
                End Select
            End Using
        End Try
    End Sub

    Sub SetMenuStyle()
        Dim col As Color

        If ToolStripRendererEx.IsAutoRenderMode Then
            col = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0)
        Else
            col = Color.FromArgb(&HFF004BFF)
        End If

        llAudioProfile0.LinkColor = col
        llAudioProfile1.LinkColor = col
        llEditAudio0.LinkColor = col
        llEditAudio1.LinkColor = col
        blFilesize.LinkColor = col
        llMuxer.LinkColor = col
        blSourceParText.LinkColor = col
        blSourceDarText.LinkColor = col
        blTargetParText.LinkColor = col
        blTargetDarText.LinkColor = col

        lgbEncoder.Label.LinkColor = col
        lgbFilters.Label.LinkColor = col
        lgbResize.Label.LinkColor = col
        lgbSource.Label.LinkColor = col
        lgbTarget.Label.LinkColor = col
        laTip.ForeColor = col
    End Sub

    Function GetIfoFile() As String
        Dim ret = p.SourceFile.Dir

        If ret.EndsWith("_temp\", StringComparison.Ordinal) Then
            ret = ret.Parent
        End If

        ret = ret & p.SourceFile.FileName.DeleteRight(5) & "0.ifo"

        If File.Exists(ret) Then
            Return ret
        End If
    End Function

    Sub RenameDVDTracks()
        Dim ifoPath = GetIfoFile()

        If Not File.Exists(ifoPath) Then
            Exit Sub
        End If

        For Each i In Directory.GetFiles(p.TempDir)
            If g.IsSourceSame(i) AndAlso i.Contains("VTS") Then
                Dim regex As New Regex(" T(\d\d) ")
                Dim match = regex.Match(i)

                If match.Success Then
                    Dim dgID = CInt(match.Groups(1).Value)

                    Using mi As New MediaInfo(ifoPath)
                        For x = 0 To mi.AudioStreams.Count - 1
                            If mi.GetAudio(x, "ID/String").Contains(" (0x" & dgID & ")") Then
                                Dim stream = mi.AudioStreams(x)
                                Dim base = i.Base
                                base = regex.Replace(base, " ID" & x & " ")
                                base = base.Replace("2_0ch", "2ch")
                                base = base.Replace("3_2ch", "6ch")
                                base = base.Replace(" DELAY", "")
                                base = base.Replace(" 0ms", "")

                                'If Not String.Equals(stream.Language.TwoLetterCode, "iv") Then
                                If stream.Language.LCID <> 127 Then
                                    base &= " " & stream.Language.CultureInfo.EnglishName 'Language.Name
                                End If

                                If stream.Title.NotNullOrEmptyS Then
                                    base &= " " & stream.Title
                                End If

                                FileHelp.Move(i, i.Dir & base & i.ExtFull)
                            End If
                        Next
                    End Using
                End If
            End If
        Next
    End Sub

    Sub DetectAudioFiles(track As Integer, lang As Boolean, same As Boolean, hq As Boolean)
        Dim tb, tbOther As TextBox
        Dim profile As AudioProfile

        If track = 0 Then
            tb = tbAudioFile0
            tbOther = tbAudioFile1
            profile = p.Audio0
        Else
            tb = tbAudioFile1
            tbOther = tbAudioFile0
            profile = p.Audio1
        End If

        If tb.Text.NotNullOrEmptyS OrElse TypeOf profile Is NullAudioProfile Then
            Exit Sub
        End If

        Dim files = g.GetFilesInTempDirAndParent
        files.Sort(New StringLogicalComparer)

        For Each iExt In FileTypes.Audio
            If String.Equals(iExt, "avs") Then
                Continue For
            End If

            For Each iPath In files
                If track = 1 AndAlso iPath.Base = p.Audio0.File.Base Then Continue For
                If track = 0 AndAlso iPath.Base = p.Audio1.File.Base Then Continue For
                If tbOther.Text = iPath Then Continue For
                If Not iPath.Ext = iExt Then Continue For
                If iPath.Contains("_cut_") Then Continue For
                If iPath.Contains("_out") Then Continue For
                If Not g.IsSourceSame(iPath) Then Continue For
                If hq AndAlso Not iPath.Ext.EqualsAny("dtsma", "thd", "ec3", "eac3", "thd+ac3", "dtshr", "dtshd") Then Continue For

                If same AndAlso tbOther.Text.NotNullOrEmptyS AndAlso Not String.Equals(tbOther.Text.ExtFull, iPath.ExtFull) Then
                    Continue For
                End If

                If lang Then
                    Dim lng = profile.Language

                    '  If String.Equals(profile.Language.ThreeLetterCode, "und") Then
                    If track <> 0 AndAlso profile.Language.LCID = 127 Then 'Added track <> 0 AndAlso
                        'lng = If(track = 0, New Language(), New Language(9, "en"))
                        lng = New Language(9, "en", True)
                    End If

                    If Not iPath.Contains(lng.Name) Then
                        Continue For
                    End If
                End If

                If String.Equals(iPath.Ext, "mp4") AndAlso p.SourceFile.IsSameBase(iPath) Then
                    Continue For
                End If

                If Not (track = 1 AndAlso String.Equals(p.Audio0.File, iPath)) AndAlso Not (track = 0 AndAlso String.Equals(p.Audio1.File, iPath)) Then
                    tb.Text = iPath
                    Exit Sub
                End If
            Next
        Next
    End Sub

    Function IsSaveCanceled() As Boolean
        'ObjectHelp.GetCompareString(g.SavedProject).WriteFile(Folder.Desktop & "\test1.txt", Encoding.ASCII)
        'ObjectHelp.GetCompareString(p).WriteFile(Folder.Desktop & "\test2.txt", Encoding.ASCII)

        If Not EqualsExS(ObjectHelp.GetCompareString(g.SavedProject), ObjectHelp.GetCompareString(p)) Then
            Using td As New TaskDialog(Of DialogResult)
                td.MainInstruction = "Save changed project?"
                td.AddButton("Save", DialogResult.Yes)
                td.AddButton("Don't Save", DialogResult.No)
                td.AddButton("Cancel", DialogResult.Cancel)
                td.Show()
                Refresh()

                If td.SelectedValue = DialogResult.Yes Then
                    If g.ProjectPath Is Nothing Then
                        If Not OpenSaveProjectDialog() Then
                            Return True
                        End If
                    Else
                        SaveProjectPath(g.ProjectPath)
                    End If
                ElseIf td.SelectedValue = DialogResult.Cancel Then
                    Return True
                End If
            End Using
        End If
    End Function

    Sub UpdateRecentProjectsMenu()
        If Disposing OrElse IsDisposed Then
            Exit Sub
        End If

        For Each mi In CustomMainMenu.MenuItems
            If String.Equals(mi.CustomMenuItem?.MethodName, "DynamicMenuItem") AndAlso mi.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.RecentProjects) Then

                mi.DropDown.SuspendLayout()
                Dim tsddI As ToolStripItemCollection = mi.DropDownItems
                tsddI.ClearAndDispose()

                SyncLock s.RecentProjects
                    For Each recentProj In s.RecentProjects
                        If File.Exists(recentProj) AndAlso Not String.Equals(recentProj.Base, "recover") Then
                            Dim name = recentProj

                            If recentProj.Length > 130 Then
                                name = "..." & name.Substring(name.Length - 130)
                            End If

                            tsddI.Add(New ActionMenuItem(name, Sub() LoadProject(recentProj)))
                        End If
                    Next recentProj
                End SyncLock

                mi.DropDown.ResumeLayout()
                Exit For
            End If
        Next mi
    End Sub

    <DebuggerNonUserCode>
    Sub UpdateDynamicMenuAsync()
        Task.Run(Sub()
                     If MediaInfo.Cache Is Nothing Then MediaInfo.Cache = New Dictionary(Of Long, MediaInfo)(197)
                     Thread.Sleep(450)
                     Try
                         'Dim udmD As MethodInvoker = (Sub() UpdateDynamicMenu())
                         'Invoke(udmD)
                         If Not IsDisposed AndAlso IsHandleCreated Then
                             BeginInvoke(Sub()
                                             UpdateScriptsMenuAsync() ' Opt. Moved from OnActivated for now. No Dynamic Script Menu Update
                                             UpdateDynamicMenu()
                                         End Sub)
                         End If
                     Catch
                     End Try
                 End Sub)
    End Sub

    Sub UpdateDynamicMenu()
        PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
        PopulateProfileMenu(DynamicMenuItemID.MuxerProfiles)
        PopulateProfileMenu(DynamicMenuItemID.Audio1Profiles)
        PopulateProfileMenu(DynamicMenuItemID.Audio2Profiles)
        PopulateProfileMenu(DynamicMenuItemID.FilterSetupProfiles)

        For Each iMenuItem In CustomMainMenu.MenuItems
            If String.Equals(iMenuItem.CustomMenuItem.MethodName, "DynamicMenuItem") Then
                If iMenuItem.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.HelpApplications) Then
                    iMenuItem.DropDown.SuspendLayout()
                    iMenuItem.DropDownItems.ClearAndDispose()
                    Dim mL1AlphL As New List(Of ToolStripMenuItemEx)(28)
                    Dim mL2LL As New List(Of List(Of ActionMenuItem))(28) '~0.640ms
                    Dim invCultTI As Globalization.TextInfo = InvCultTxtInf
                    Dim packAr(Package.Items.Count - 1) As Package
                    Package.Items.Values.CopyTo(packAr, 0)

                    For i = 0 To packAr.Length - 1
                        Dim nML2 As List(Of ActionMenuItem)
                        Dim lastCH As Char
                        Dim pack As Package = packAr(i)
                        Dim c1 As Char = invCultTI.ToUpper(pack.Name(0))
                        If c1 <> lastCH Then 'source is sorted, if not HashSet
                            lastCH = c1
                            mL1AlphL.Add(New ToolStripMenuItemEx(c1))
                            nML2 = New List(Of ActionMenuItem)(4)
                            mL2LL.Add(nML2)
                        End If

                        nML2.Add(New ActionMenuItem(pack.Name, Sub() pack.ShowHelp()))
                    Next i

                    iMenuItem.DropDownItems.AddRange(mL1AlphL.ToArray)

                    For i = 0 To mL1AlphL.Count - 1
                        Dim nMI As ToolStripMenuItemEx = mL1AlphL(i)
                        nMI.DropDown.SuspendLayout()
                        nMI.DropDownItems.AddRange(mL2LL(i).ToArray)
                        nMI.DropDown.ResumeLayout(False)
                    Next i

                    iMenuItem.DropDown.ResumeLayout(False)
                    Exit For
                End If
            End If
        Next iMenuItem
        Application.DoEvents()
    End Sub

    Async Sub UpdateScriptsMenuAsync()
        Dim evnts As String()
        Dim files As String() = Await Task.Run(Function()
                                                   evnts = KGySoft.CoreLibraries.Enum(Of ApplicationEvent).GetNames
                                                   Return If(Folder.Scripts.DirExists, Directory.GetFiles(Folder.Scripts), {})
                                               End Function)

        If IsDisposed OrElse Native.GetForegroundWindow() <> Handle OrElse files.Length = 0 Then
            Exit Sub
        End If
        'Private lastFileC As Integer 'TODO Something like this to Update ONActivated not every time, moved to OnShow.UpdateDynamicMenuAsync() for now
        'Dim fc = files.Length
        'If fc = 0 OrElse fc = lastFileC Then Exit Sub
        'lastFileC = fc

        'Dim evnts As String() = System.Enum.GetNames(GetType(ApplicationEvent))
        For Each menuItem In CustomMainMenu.MenuItems
            If String.Equals(menuItem.CustomMenuItem.MethodName, "DynamicMenuItem") Then
                If menuItem.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.Scripts) Then
                    menuItem.DropDown.SuspendLayout()
                    Dim tsddI As ToolStripItemCollection = menuItem.DropDownItems
                    tsddI.ClearAndDispose()

                    For Each path In files
                        Dim pFN As String = path.FileName
                        If Not pFN(0) = "_"c AndAlso Not evnts.ContainsString(pFN.Left(".")) Then
                            tsddI.Add(New ActionMenuItem(pFN.Base, Sub() g.DefaultCommands.ExecuteScriptFile(path)))
                            'ActionMenuItem.Add(tsddI, pFN.Base, Sub() g.DefaultCommands.ExecuteScriptFile(path))
                        End If
                    Next

                    tsddI.AddRange({New ToolStripSeparator, New ActionMenuItem("Open Script Folder", Sub()
                                                                                                         g.ShellExecute(Folder.Scripts)
                                                                                                         UpdateScriptsMenuAsync()
                                                                                                     End Sub)})
                    menuItem.DropDown.ResumeLayout()
                    'Exit For 'Add This ???
                End If
            End If
        Next menuItem
        Dim unused = Task.Run(Sub() KGySoft.CoreLibraries.Enum(Of ApplicationEvent).ClearCaches())
    End Sub

    Async Sub UpdateTemplatesMenuAsync()
        'Dim files As String()
        Dim filesT = Await Task.Run(Function()
                                        Thread.Sleep(510)

                                        For Each iFile In Directory.GetFiles(Folder.Template, "*.srip.backup")
                                            FileHelp.Move(iFile, iFile.Dir & "Backup\" & iFile.Base)
                                        Next

                                        Return Directory.GetFiles(Folder.Template, "*.srip", SearchOption.AllDirectories)
                                    End Function)

        If Disposing OrElse IsDisposed Then Exit Sub

        Dim cmMi As List(Of ToolStripMenuItemEx) = CustomMainMenu.MenuItems

        For n = 0 To cmMi.Count - 1
            Dim i = cmMi(n)
            If String.Equals(i.CustomMenuItem.MethodName, "DynamicMenuItem") AndAlso i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.TemplateProjects) Then

                i.DropDown.SuspendLayout()
                ActionMenuItem.LayoutSuspendCreate()
                Dim tsddI As ToolStripItemCollection = i.DropDownItems
                tsddI.ClearAndDispose()

                For Each i2 In filesT
                    Dim base = i2.Base

                    If String.Equals(i2, g.StartupTemplatePath) Then
                        base &= " (Startup)"
                    End If

                    If i2.Contains("Backup\") Then
                        base = "Backup | " & base
                    End If

                    ActionMenuItem.Add(tsddI, base, AddressOf LoadProject, i2, Nothing)
                Next i2

                tsddI.AddRange({New ToolStripSeparator,
               New ActionMenuItem("Explore", Sub() g.ShellExecute(Folder.Template), "Opens the directory containing the templates."),
               New ActionMenuItem("Restore", AddressOf ResetTemplates, "Restores the default templates.")})

                ActionMenuItem.LayoutResume()
                i.DropDown.ResumeLayout()
                Exit For
            End If
        Next n

        Application.DoEvents()
    End Sub

    Sub ResetTemplates()
        If MsgQuestion("Restore the default templates?") = DialogResult.OK Then
            Try
                FolderHelp.Delete(Folder.Template)
                Folder.Template.ToString()
                UpdateTemplatesMenuAsync()
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End If
    End Sub

    <Command("Loads a template.")>
    Sub LoadTemplate(name As String)
        LoadProject(Folder.Template & name & ".srip")
    End Sub

    <Command("Adds a batch job.")>
    Sub AddBatchJob(sourcefile As String)
        Dim batchFolder = Folder.Settings & "Batch Projects\"

        If Not Directory.Exists(batchFolder) Then
            Directory.CreateDirectory(batchFolder)
        End If

        Dim batchProject = ObjectHelp.GetCopy(Of Project)(p)
        batchProject.BatchMode = True
        batchProject.SourceFiles = {sourcefile}.ToList
        Dim jobPath = batchFolder & sourcefile.Dir.Replace("\", "-").Replace(":", "-") & " " & p.TemplateName & " - " & sourcefile.FileName
        SafeSerialization.Serialize(batchProject, jobPath)
        JobManager.AddJob(sourcefile.Base, jobPath)
    End Sub

    Sub LoadProject(path As String)
        Refresh()

        If Not File.Exists(path) Then
            MsgWarn("Project file not found.")
            s.UpdateRecentProjects(path)
            UpdateRecentProjectsMenu()
            UpdateTemplatesMenuAsync()
        Else
            OpenProject(path)
        End If
    End Sub

    Function OpenSaveProjectDialog() As Boolean
        Using dialog As New SaveFileDialog
            dialog.SetInitDir(p.TempDir)

            If p.SourceFile.NotNullOrEmptyS Then
                dialog.FileName = p.TargetFile.Base
            Else
                dialog.FileName = "Untitled"
            End If

            dialog.Filter = "StaxRip Project Files (*.srip)|*.srip"

            If dialog.ShowDialog() = DialogResult.OK Then
                If Not dialog.FileName.ToLower(InvCult).EndsWith(".srip", StringComparison.Ordinal) Then
                    dialog.FileName &= ".srip"
                End If

                SaveProjectPath(dialog.FileName)
                Return True
            End If
        End Using
    End Function

    Sub SetBindings(proj As Project, add As Boolean)
        SetTextBoxBinding(tbTargetFile, proj, NameOf(Project.TargetFile), add)

        If add Then
            AddHandler proj.PropertyChanged, AddressOf ProjectPropertyChanged
        Else
            RemoveHandler proj.PropertyChanged, AddressOf ProjectPropertyChanged
        End If
    End Sub

    Sub ProjectPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        Assistant()
    End Sub

    Sub SetTextBoxBinding(tb As TextBox, obj As Object, prop As String, add As Boolean)
        If add Then
            tb.DataBindings.Add(New Binding(NameOf(TextBox.Text), obj, prop, False, DataSourceUpdateMode.OnPropertyChanged))
        Else
            tb.DataBindings.Clear()
        End If
    End Sub

    Function OpenProject(Optional path As String = Nothing) As Boolean
        Return OpenProject(path, True)
    End Function

    Function OpenProject(path As String, saveCurrent As Boolean) As Boolean
        Try
            If Not IsLoading AndAlso saveCurrent AndAlso IsSaveCanceled() Then
                Return False
            End If

            SetBindings(p, False)

            If path.NullOrEmptyS OrElse Not File.Exists(path) Then
                path = g.StartupTemplatePath
            End If

            Try
                p = SafeSerialization.Deserialize(New Project, path)
            Catch ex As Exception
                g.ShowException(ex, "Project file failed to load", "It will be reset to defaults." & BR2 & path)
                p = New Project
                p.Init()
            End Try

            If p.SourceFile.NotNullOrEmptyS AndAlso Not FrameServerHelp.VerifyAviSynthLinks() Then
                Throw New AbortException
            End If

            Dim mTxT = Task.Run(Function()
                                    If File.Exists(Folder.Temp & "staxrip.log") Then FileHelp.Delete(Folder.Temp & "staxrip.log")
                                    Return path.Base & " - " & Application.ProductName & " " & Application.ProductVersion
                                End Function)
            Log = p.Log
            SetBindings(p, True)

            SkipAssistant = True

            If path.StartsWith(Folder.Template, StringComparison.Ordinal) Then
                g.ProjectPath = Nothing
                p.TemplateName = path.Base
                p.BatchMode = False
            Else
                g.ProjectPath = path
            End If

            g.SetTempDir()

            Dim width = p.TargetWidth
            Dim height = p.TargetHeight
            Dim size = p.TargetSize
            Dim bitrate = p.VideoBitrate

            FiltersListView.Load()
            FiltersListView.Items(0).Selected = True
            p.VideoEncoder.OnStateChange()

            Dim targetPath = p.TargetFile

            Dim audio0 = p.Audio0.File
            Dim audio1 = p.Audio1.File

            Dim delay0 = p.Audio0.Delay
            Dim delay1 = p.Audio1.Delay

            BlockSourceTextBoxTextChanged = True
            tbSourceFile.Text = p.SourceFile
            BlockSourceTextBoxTextChanged = False

            If p.SourceFile.NotNullOrEmptyS Then
                s.LastSourceDir = p.SourceFile.Dir
            End If

            tbAudioFile0.Text = audio0
            tbAudioFile1.Text = audio1

            llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
            llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)

            p.Audio0.Delay = delay0
            p.Audio1.Delay = delay1

            If p.BitrateIsFixed Then
                tbBitrate.Text = ""
                tbBitrate.Text = bitrate.ToString
            Else
                tbTargetSize.Text = ""
                tbTargetSize.Text = size.ToString()
            End If

            SetSlider()

            tbTargetWidth.Text = width.ToString
            tbTargetHeight.Text = height.ToString

            Me.Text = mTxT.Result
            'If g.Is32Bit Then 'Text &= " (32 bit)"

            SetSavedProject()

            SkipAssistant = False

            Assistant()
            s.UpdateRecentProjects(path)
            UpdateRecentProjectsMenu()
            g.RaiseAppEvent(ApplicationEvent.ProjectLoaded)
            g.RaiseAppEvent(ApplicationEvent.ProjectOrSourceLoaded)
            FiltersListView.RebuildMenu()

            If p.SourceFile.NotNullOrEmptyS AndAlso Not g.VerifyRequirements Then
                Throw New AbortException
            End If

            Return True
        Catch ex As Exception
            OpenProject(g.StartupTemplatePath)
        End Try
    End Function

    Sub SetSlider()
        Dim w = If(p.ResizeSliderMaxWidth = 0, p.SourceWidth, p.ResizeSliderMaxWidth)
        tbResize.Maximum = CInt((Calc.FixMod16(w) - 320) / p.ForcedOutputMod)
    End Sub

    Sub SetSavedProject()
        g.SavedProject = ObjectHelp.GetCopy(Of Project)(p)
    End Sub

    Function GetPathFromIndexFile(sourcePath As String) As String
        For Each i In File.ReadAllLines(sourcePath)
            If i.Length >= 4 AndAlso ((i(0) = "\"c AndAlso i(1) = "\"c) OrElse i.Contains(":\")) Then  'i.StartsWith("\\", StringComparison.Ordinal) Then
                If Regex.IsMatch(i, "^.+ \d+$") Then
                    'i = i.LeftLast(" "c)
                    i = i.Substring(0, i.LastIndexOf(" "c))
                End If

                If File.Exists(i) AndAlso FileTypes.Video.ContainsString(i.Ext) Then
                    Return i
                End If
            End If
        Next
    End Function

    Function ShowSourceFilterSelectionDialog(inputFile As String) As VideoFilter
        Dim filters As New List(Of VideoFilter)

        'DGIndexNV Back
        If String.Equals(inputFile.Ext, "dgi") OrElse FileTypes.DGDecNVInput.ContainsString(inputFile.Ext) Then AddSourceFilters({"DGSource"}, filters)
        If String.Equals(inputFile.Ext, "dgim") OrElse FileTypes.DGDecNVInput.ContainsString(inputFile.Ext) Then AddSourceFilters({"DGSourceIM"}, filters)

        If inputFile.Ext.EqualsAny("mp4", "m4v", "mov") Then
            AddSourceFilters({"LSMASHVideoSource", "LibavSMASHSource"}, filters)
        End If

        If FileTypes.Video.ContainsString(inputFile.Ext) AndAlso Not FileTypes.VideoText.ContainsString(inputFile.Ext) Then
            AddSourceFilters({"FFVideoSource", "LWLibavVideoSource", "ffms2", "LWLibavSource"}, filters)
        End If

        If g.IsCOMObjectRegistered(GUIDS.LAVSplitter) AndAlso g.IsCOMObjectRegistered(GUIDS.LAVVideoDecoder) Then
            AddSourceFilters({"DSS2"}, filters)
        End If

        If {"avi", "avs", "vdr"}.ContainsString(inputFile.Ext) Then
            AddSourceFilters({"AVISource"}, filters)
        End If

        If String.Equals(inputFile.Ext, "d2v") Then
            AddSourceFilters({"d2vsource"}, filters)
        End If

        If p.Script.Engine = ScriptEngine.AviSynth Then
            filters = filters.WhereF(Function(filter) Not filter.Script.Replace(" ", "").Contains("clip=core."))
            filters.Insert(0, New VideoFilter("Source", "Automatic", "#avs"))
        Else
            filters = filters.WhereF(Function(filter) filter.Script.Replace(" ", "").Contains("clip=core."))
            filters.Insert(0, New VideoFilter("Source", "Automatic", "#vs"))
        End If

        Dim td As New TaskDialog(Of VideoFilter)

        If p.Script.Engine = ScriptEngine.AviSynth Then
            td.MainInstruction = "Select a AviSynth source filter."
        Else
            td.MainInstruction = "Select a VapourSynth source filter."
        End If

        For Each filter In filters
            td.AddCommand(filter.Name, filter)
        Next

        Dim ret = td.Show

        If ret Is Nothing Then
            Throw New AbortException
        End If

        ret.Script = Macro.ExpandGUI(ret.Script, True).Value
        Return ret
    End Function

    Sub AddSourceFilters(filterNames As String(), filters As List(Of VideoFilter))
        Dim avsProfiles = s.AviSynthProfiles.FirstF(Function(cat) String.Equals(cat.Name, "Source")).Filters
        Dim vsProfiles = s.VapourSynthProfiles.FirstF(Function(cat) String.Equals(cat.Name, "Source")).Filters
        Dim allFilters = avsProfiles.Concat(vsProfiles)

        For Each filter In allFilters
            For Each filterName In filterNames
                If filter.Script.IndexOf(filterName & "(", StringComparison.OrdinalIgnoreCase) >= 0 OrElse filter.Script.IndexOf(filterName & ".", StringComparison.OrdinalIgnoreCase) >= 0 Then

                    If filters.FindIndex(Function(val) EqualsExS(val.Name, filter.Name)) < 0 Then
                        filters.Add(filter.GetCopy)
                    End If
                End If
            Next
        Next
    End Sub

    Sub OpenAnyFile(files As String()) 'IEnumerable(Of String))
        files.SelectInPlaceF(Function(filePath) New FileInfo(filePath).FullName)
        Dim f0Ext As String = files(0).Ext

        If String.Equals(f0Ext, "srip") Then
            OpenProject(files(0))
        ElseIf FileTypes.Video.ContainsString(f0Ext) Then
            Array.Sort(files, StringComparer.OrdinalIgnoreCase)
            OpenVideoSourceFiles(files)
        ElseIf FileTypes.Audio.ContainsString(f0Ext) Then
            tbAudioFile0.Text = files(0)
        Else
            Array.Sort(files, StringComparer.OrdinalIgnoreCase)
            OpenVideoSourceFiles(files)
        End If
    End Sub

    Sub OpenVideoSourceFile(fp As String)
        OpenVideoSourceFiles({fp})
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String))
        OpenVideoSourceFiles(files, False)
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String), isEncoding As Boolean)
        Dim recoverPath = g.ProjectPath
        Dim recoverProjectPath = Folder.Temp & Guid.NewGuid.ToString & ".bin"
        Dim recoverText = Text

        SafeSerialization.Serialize(p, recoverProjectPath)
        Dim deh As EventHandler = Sub()
                                      RemoveHandler Me.Disposed, deh
                                      FileHelp.Delete(recoverProjectPath)
                                  End Sub
        AddHandler Disposed, deh

        Try
            files = files.Select(Function(filePath) New FileInfo(filePath).FullName) '.AsEnumerable

            If Not g.VerifySource(files) Then
                Throw New AbortException
            End If

            For Each i In files
                Dim name = i.FileName

                If name.ToUpper(InvCult) Like "VTS_0#_0.VOB" Then
                    If MsgQuestion("Are you sure you want to open the file " & name & "," & BR &
                           "the first VOB file usually contains a menu.") = DialogResult.Cancel Then

                        Throw New AbortException
                    End If
                End If

                If String.Equals(name, "VIDEO_TS.VOB", StringComparison.OrdinalIgnoreCase) Then
                    MsgWarn("The file VIDEO_TS.VOB can't be opened.")
                    Throw New AbortException
                End If
            Next

            If p.SourceFile.NotNullOrEmptyS AndAlso Not isEncoding Then
                Dim templates = Directory.GetFiles(Folder.Template, "*.srip")

                If templates.Length = 1 Then
                    If Not OpenProject(templates(0), True) Then
                        Throw New AbortException
                    End If
                Else
                    If s.ShowTemplateSelection Then
                        If Not LoadTemplateWithSelectionDialog() Then
                            Throw New AbortException
                        End If
                    Else
                        If Not OpenProject() Then
                            Throw New AbortException
                        End If
                    End If
                End If
            End If

            p.SourceFiles = files.ToList
            p.SourceFile = p.SourceFiles(0) ' files(0)

            If p.SourceFile.Ext.EqualsAny(FileTypes.Image) Then
                If p.SourceFile.Base(p.SourceFile.Base.Length - 1).IsDigitEx Then
                    If p.Script.Engine = ScriptEngine.AviSynth Then
                        Dim digitCount = 0

                        For i = p.SourceFile.Base.Length - 1 To 0 Step -1
                            If p.SourceFile.Base(i).IsDigitEx Then
                                digitCount += 1
                            End If
                        Next

                        Dim startText = p.SourceFile.Base.Substring(0, p.SourceFile.Base.Length - digitCount)
                        Dim images As New List(Of String)
                        Dim allFiles = Directory.GetFiles(p.SourceFile.Dir)

                        For Each file In Directory.GetFiles(p.SourceFile.Dir)
                            If file.Base.Length = p.SourceFile.Base.Length AndAlso String.Equals(file.Ext, p.SourceFile.Ext) AndAlso file.Base.StartsWith(startText, StringComparison.Ordinal) Then
                                images.Add(file)
                            End If
                        Next

                        images.Sort()

                        Dim filter = p.Script.GetFilter("Source")
                        filter.Path = "Image"
                        filter.Script = "ImageSource(""" & images(0).Dir & p.SourceFile.Base.Substring(0, p.SourceFile.Base.Length - digitCount) & "%0" & digitCount & "d." & p.SourceFile.Ext &
                            """, " & images(0).Base.Substring(p.SourceFile.Base.Length - digitCount).ToInt &
                            ", " & images(images.Count - 1).Base.Substring(p.SourceFile.Base.Length - digitCount).ToInt & ", 25)"
                    End If
                Else
                    Dim filter = p.Script.GetFilter("Source")
                    filter.Path = "Image"
                    filter.Script = "ImageSource(""%source_file%"", 0, 1000, 25)"
                End If
            End If

            FiltersListView.IsLoading = True

            Dim preferredSourceFilter As VideoFilter

            If p.SourceFiles.Count = 1 AndAlso String.Equals(p.Script.Filters(0).Name, "Manual") AndAlso Not p.NoDialogs AndAlso Not p.BatchMode AndAlso Not p.SourceFile.Ext.EqualsAny("avs", "vpy") Then
                preferredSourceFilter = ShowSourceFilterSelectionDialog(files(0))
            End If

            If preferredSourceFilter IsNot Nothing Then
                Dim isVapourSynth = preferredSourceFilter.Script.Replace(" ", "").Contains("clip=core.") OrElse
                    preferredSourceFilter.Script = "#vs"

                If isVapourSynth Then
                    If Not Package.Python.VerifyOK(True) OrElse
                        Not Package.VapourSynth.VerifyOK(True) OrElse
                        Not Package.vspipe.VerifyOK(True) Then

                        Throw New AbortException
                    End If

                    If p.Script.Engine = ScriptEngine.AviSynth Then
                        p.Script = VideoScript.GetDefaults()(1)
                    End If
                Else
                    If Not Package.AviSynth.VerifyOK(True) Then
                        Throw New AbortException
                    End If

                    If p.Script.Engine = ScriptEngine.VapourSynth Then
                        p.Script = VideoScript.GetDefaults()(0)
                    End If
                End If

                p.Script.SetFilter(preferredSourceFilter.Category, preferredSourceFilter.Name, preferredSourceFilter.Script)
            End If

            If Not g.VerifyRequirements() Then
                Throw New AbortException
            End If

            p.LastOriginalSourceFile = p.SourceFile
            p.FirstOriginalSourceFile = p.SourceFile

            If FileTypes.VideoIndex.ContainsString(p.SourceFile.Ext) Then
                Dim path = GetPathFromIndexFile(p.SourceFile)

                If path.NotNullOrEmptyS Then
                    p.LastOriginalSourceFile = path
                    p.FirstOriginalSourceFile = path
                End If
            ElseIf p.SourceFile.Ext.EqualsAny({"avs", "vpy"}) Then
                Dim code = p.SourceFile.ReadAllText
                Dim matches = Regex.Matches(code, "('|"")(.+?)\1", RegexOptions.IgnoreCase)

                For Each match As Match In matches
                    If match.Success Then
                        Dim path = match.Groups(2).Value

                        If File.Exists(path) AndAlso FileTypes.Video.ContainsString(path.Ext) Then
                            If FileTypes.VideoIndex.ContainsString(path.Ext) Then
                                path = GetPathFromIndexFile(path)

                                If path.NotNullOrEmptyS Then
                                    p.LastOriginalSourceFile = path
                                    p.FirstOriginalSourceFile = path
                                    Exit For
                                End If
                            Else
                                p.LastOriginalSourceFile = path
                                p.FirstOriginalSourceFile = path
                                Exit For
                            End If
                        End If
                    End If
                Next
            End If

            g.SetTempDir()

            Dim sourcePAR = MediaInfo.GetVideo(p.LastOriginalSourceFile, "PixelAspectRatio")

            If sourcePAR.NotNullOrEmptyS Then
                p.SourcePAR.X = CInt(Convert.ToSingle(sourcePAR, InvCult) * 1000)
                p.SourcePAR.Y = 1000
            End If

            p.SourceVideoFormat = MediaInfo.GetVideoFormat(p.LastOriginalSourceFile)
            p.SourceVideoFormatProfile = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Format_Profile")
            p.SourceVideoBitDepth = MediaInfo.GetVideo(p.LastOriginalSourceFile, "BitDepth").ToInt
            p.SourceColorSpace = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ColorSpace")
            p.SourceChromaSubsampling = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ChromaSubsampling")
            p.SourceSize = New FileInfo(p.LastOriginalSourceFile).Length
            p.SourceBitrate = CInt(MediaInfo.GetVideo(p.LastOriginalSourceFile, "BitRate").ToInt / 1000)
            p.SourceScanType = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ScanType")
            p.SourceScanOrder = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ScanOrder")

            p.VideoEncoder.SetMetaData(p.LastOriginalSourceFile)

            Dim mkvMuxer = TryCast(p.VideoEncoder.Muxer, MkvMuxer)

            If mkvMuxer IsNot Nothing AndAlso mkvMuxer.Title.NullOrEmptyS Then
                mkvMuxer.Title = MediaInfo.GetGeneral(p.LastOriginalSourceFile, "Movie")
            End If

            If Not isEncoding AndAlso p.BatchMode Then
                Assistant()
                Exit Sub
            End If

            Log.WriteEnvironment()

            Dim targetDir As String = Nothing

            If p.DefaultTargetFolder.NotNullOrEmptyS Then
                targetDir = Macro.Expand(p.DefaultTargetFolder).FixDir

                If Not Directory.Exists(targetDir) Then
                    Try
                        Directory.CreateDirectory(targetDir)
                    Catch ex As Exception
                    End Try
                End If
            End If

            If Not Directory.Exists(targetDir) Then
                targetDir = p.SourceFile.Dir
            End If

            Dim targetName = Macro.Expand(p.DefaultTargetName)

            If Not targetName.IsValidFileSystemName Then
                targetName = p.SourceFile.Base
            End If

            tbTargetFile.Text = targetDir & targetName & p.VideoEncoder.Muxer.OutputExtFull

            If EqualsExS(p.SourceFile, p.TargetFile) OrElse
                (FileTypes.VideoIndex.ContainsString(p.SourceFile.Ext) AndAlso
                p.SourceFile.ReadAllText.Contains(p.TargetFile)) Then

                tbTargetFile.Text = p.TargetFile.DirAndBase & "_new" & p.TargetFile.ExtFull
            End If

            Log.WriteHeader("Media Info Source File")

            For Each i In p.SourceFiles
                Log.WriteLine(i)
            Next

            Log.WriteLine(BR & MediaInfo.GetSummary(p.SourceFile))

            For Each i In DriveInfo.GetDrives()
                If i.DriveType = DriveType.CDRom AndAlso
                    p.TempDir.StartsWith(i.RootDirectory.ToString, StringComparison.OrdinalIgnoreCase) Then

                    MsgWarn("Opening files from a optical drive requires to set a temp files folder in the options.")
                    Throw New AbortException
                End If
            Next

            Demux()

            If Not EqualsExS(p.LastOriginalSourceFile, p.SourceFile) AndAlso
                Not FileTypes.VideoText.ContainsString(p.SourceFile.Ext) Then

                p.LastOriginalSourceFile = p.SourceFile
            End If

            s.LastSourceDir = p.SourceFile.Dir

            If p.Script.Engine = ScriptEngine.AviSynth AndAlso String.Equals(p.SourceFile.Ext, "avs") Then
                p.Script.Filters.Clear()
                p.Script.Filters.Add(New VideoFilter("Source", "AVS Script Import", "Import(""" + p.SourceFile + """)"))
            ElseIf String.Equals(p.SourceFile.Ext, "vpy") Then
                p.Script.Engine = ScriptEngine.VapourSynth
                p.Script.Filters.Clear()

                'VPY script open problem:
                'Dim code = "import os, sys" + BR + "import vapoursynth as vs" + BR + "core = vs.core" + BR +
                '"from importlib.machinery import SourceFileLoader" + BR +
                '$"SourceFileLoader('clip', r""{p.SourceFile}"").load_module()" + BR +
                '"clip = vs.get_output()"

                Dim code = "from importlib.machinery import SourceFileLoader" + BR +
                           $"SourceFileLoader('clip', r""{p.SourceFile}"").load_module()" + BR +
                           "clip = vs.get_output()"
                p.Script.Filters.Add(New VideoFilter("Source", "VS Script Import", code))
            End If

            ModifyFilters()
            FiltersListView.IsLoading = False
            FiltersListView.Load()

            RenameDVDTracks()

            If FileTypes.VideoAudio.ContainsString(p.LastOriginalSourceFile.Ext) Then
                p.Audio0.Streams = MediaInfo.GetAudioStreams(p.LastOriginalSourceFile)
                p.Audio1.Streams = p.Audio0.Streams
            End If

            If String.Equals(p.SourceFile.Ext, "d2v") Then
                Dim content = p.SourceFile.ReadAllText

                If content.Contains("Aspect_Ratio=16:9") Then
                    p.SourceAnamorphic = True
                Else
                    Dim ifoFile = GetIfoFile()

                    If ifoFile.NotNullOrEmptyS Then
                        Dim dar2 = MediaInfo.GetVideo(ifoFile, "DisplayAspectRatio")

                        If String.Equals(dar2, "1.778") Then
                            p.SourceAnamorphic = True
                        End If
                    End If
                End If

                If content.Contains("Frame_Rate=29970") Then
                    Dim m = Regex.Match(content, "FINISHED +(\d+).+FILM")

                    If m.Success Then
                        Dim film = m.Groups(1).Value.ToInt

                        If film >= 95 Then
                            content = content.Replace("Field_Operation=0" + BR + "Frame_Rate=29970 (30000/1001)", "Field_Operation=1" + BR + "Frame_Rate=23976 (24000/1001)")
                            content.WriteFileDefault(p.SourceFile)
                        End If
                    End If
                End If
            End If

            Dim errorMsg = ""

            Try
                p.SourceScript.Synchronize()
                errorMsg = p.SourceScript.GetError
            Catch ex As Exception
                errorMsg = ex.Message
            End Try

            If errorMsg.NotNullOrEmptyS Then
                Log.WriteHeader("Error opening source")
                Log.WriteLine(errorMsg + BR2)
                Log.WriteLine(p.SourceScript.GetFullScript)
                Log.Save()

                MsgError("Script Error", errorMsg, Handle)
                p.Script.Synchronize()
                Throw New AbortException
            End If

            UpdateSourceParameters()
            SetSlider()

            DetectAudioFiles(0, True, True, True)
            DetectAudioFiles(1, True, True, True)
            DetectAudioFiles(0, True, False, True)
            DetectAudioFiles(1, True, False, True)

            DetectAudioFiles(0, True, True, False)
            DetectAudioFiles(1, True, True, False)
            DetectAudioFiles(0, True, False, False)
            DetectAudioFiles(1, True, False, False)

            DetectAudioFiles(0, False, True, False)
            DetectAudioFiles(1, False, True, False)

            DetectAudioFiles(0, False, False, False)
            DetectAudioFiles(1, False, False, False)

            If p.UseScriptAsAudioSource Then
                tbAudioFile0.Text = p.Script.Path
            Else
                If p.Audio0.File.NullOrEmptyS AndAlso p.Audio1.File.NullOrEmptyS AndAlso TypeOf p.Audio0 IsNot NullAudioProfile AndAlso Not FileTypes.VideoText.ContainsString(p.LastOriginalSourceFile.Ext) Then
                    tbAudioFile0.Text = p.LastOriginalSourceFile

                    If p.Audio0.Streams.Count = 0 Then
                        tbAudioFile0.Text = ""
                    End If

                    If TypeOf p.Audio1 IsNot NullAudioProfile AndAlso p.Audio0.Streams.Count > 1 Then
                        tbAudioFile1.Text = p.LastOriginalSourceFile

                        'For Each i In p.Audio1.Streams 'WTF???
                        If p.Audio0.Stream IsNot Nothing AndAlso p.Audio1.Stream IsNot Nothing AndAlso p.Audio0.Stream.StreamOrder = p.Audio1.Stream.StreamOrder Then
                            For Each i2 In p.Audio1.Streams
                                If i2.StreamOrder <> p.Audio1.Stream.StreamOrder Then
                                    tbAudioFile1.Text = i2.Name + " (" + p.Audio1.File.Ext + ")"
                                    p.Audio1.Stream = i2
                                    UpdateSizeOrBitrate()
                                    Exit For
                                End If
                            Next
                        End If
                        'Next
                    End If
                End If
            End If

            BlockSourceTextBoxTextChanged = True
            tbSourceFile.Text = p.SourceFile
            BlockSourceTextBoxTextChanged = False

            s.LastPosition = 0

            UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
            DemuxVobSubSubtitles()
            ConvertBluRaySubtitles()
            ExtractForcedVobSubSubtitles()
            p.VideoEncoder.Muxer.Init()

            If p.HarcodedSubtitle Then
                g.AddHardcodedSubtitle()
            End If

            Dim isCropActive = p.Script.IsFilterActive("Crop")

            If isCropActive AndAlso (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) = 0 Then
                p.SourceScript.Synchronize(True, True, True)

                Using proc As New Proc
                    proc.Header = "Auto Crop"
                    proc.SkipString = "%"
                    proc.Package = Package.AutoCrop
                    proc.Arguments = p.SourceScript.Path.Escape + " " & s.CropFrameCount & " " &
                        If(FrameServerHelp.IsVfwUsed, 1, 0)
                    proc.Start()

                    Dim output = proc.Log.ToString
                    Dim match = Regex.Match(proc.Log.ToString, "(\d+),(\d+),(\d+),(\d+)")

                    If match.Success Then
                        p.CropLeft = match.Groups(1).Value.ToInt
                        p.CropTop = match.Groups(2).Value.ToInt
                        p.CropRight = match.Groups(3).Value.ToInt
                        p.CropBottom = match.Groups(4).Value.ToInt
                        g.CorrectCropMod()
                        DisableCropFilter()
                    End If
                End Using
            End If

            AutoResize()

            If isCropActive Then
                g.OvercropWidth()

                If p.AutoSmartCrop Then
                    g.SmartCrop()
                End If
            End If

            If p.AutoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled Then
                p.VideoEncoder.RunCompCheck()
            End If

            Assistant()
            g.RaiseAppEvent(ApplicationEvent.AfterSourceLoaded)
            g.RaiseAppEvent(ApplicationEvent.ProjectOrSourceLoaded)
            Log.Save()
        Catch ex As AbortException
            Log.Save()
            SetSavedProject()
            OpenProject(recoverProjectPath)
            Text = recoverText
            g.ProjectPath = recoverPath

            If isEncoding Then
                Throw New AbortException
            End If
        Catch ex As Exception
            g.OnException(ex)
            OpenProject("", False)
        Finally
            If Not isEncoding Then
                ProcController.Finished()
            End If
        End Try
    End Sub

    Sub ModifyFilters()
        If p.SourceFile.NullOrEmptyS Then
            Exit Sub
        End If

        Dim profiles = If(p.Script.Engine = ScriptEngine.AviSynth,
            s.AviSynthProfiles, s.VapourSynthProfiles)

        Dim preferences = If(p.Script.Engine = ScriptEngine.AviSynth,
            s.AviSynthFilterPreferences, s.VapourSynthFilterPreferences)

        Dim sourceFilter = p.Script.GetFilter("Source")

        SetSourceFilter(sourceFilter, preferences, profiles, True, True, False, False)
        SetSourceFilter(sourceFilter, preferences, profiles, False, True, True, False)
        SetSourceFilter(sourceFilter, preferences, profiles, True, False, False, True)
        SetSourceFilter(sourceFilter, preferences, profiles, False, False, False, False)

        Dim editAVS = p.Script.Engine = ScriptEngine.AviSynth AndAlso Not String.Equals(p.SourceFile.Ext, "avs")
        Dim editVS = p.Script.Engine = ScriptEngine.VapourSynth AndAlso Not String.Equals(p.SourceFile.Ext, "vpy")

        If editAVS Then
            If sourceFilter.Script.IndexOf("("c) < 0 Then
                Dim filter = FilterCategory.GetAviSynthDefaults.FirstF(Function(cat) String.Equals(cat.Name, "Source")).Filters.FirstF(Function(cat) String.Equals(cat.Name, "FFVideoSource"))
                p.Script.SetFilter(filter.Category, filter.Name, filter.Script)
            End If
        ElseIf editVS Then
            If sourceFilter.Script.IndexOf("("c) > 0 Then
                Dim filter = FilterCategory.GetVapourSynthDefaults.FirstF(Function(cat) String.Equals(cat.Name, "Source")).Filters.FirstF(Function(cat) String.Equals(cat.Name, "ffms2"))
                p.Script.SetFilter(filter.Category, filter.Name, filter.Script)
            End If
        End If

        For Each iFilter In p.Script.Filters
            If iFilter.Script.IndexOf("$"c) >= 0 Then
                iFilter.Script = Macro.ExpandGUI(iFilter.Script, True).Value
            End If
        Next

        If editAVS Then
            Dim miFPS = MediaInfo.GetFrameRate(p.FirstOriginalSourceFile, 25)
            Dim avsFPS = p.SourceScript.GetFramerate

            If (CInt(miFPS) * 2) = CInt(avsFPS) Then
                Dim src = p.Script.GetFilter("Source")
                src.Script = src.Script + BR + "SelectEven().AssumeFPS(" & miFPS.ToInvStr + ")"
                p.SourceScript.Synchronize()
            End If
        End If

        If s.ConvertChromaSubsampling AndAlso Not String.Equals(p.SourceChromaSubsampling, "4:2:0") Then
            If editVS Then
                Dim sourceHeight = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Height").ToInt
                Dim matrix As String

                If sourceHeight = 0 OrElse sourceHeight > 576 Then
                    matrix = "709"
                Else
                    matrix = "470bg"
                End If

                'TODO: 10 bit support 
                p.Script.GetFilter("Source").Script += BR + "clip = clip.resize.Bicubic(matrix_s = '" + matrix + "', format = vs.YUV420P8)"
            ElseIf editAVS AndAlso Not sourceFilter.Script.ContainsAny("ConvertToYV12", "ConvertToYUV420") AndAlso
                Not sourceFilter.Script.Contains("ConvertToYUV420") Then

                p.Script.GetFilter("Source").Script += BR + "ConvertToYUV420()"
            End If
        End If

        If Not sourceFilter.Script.Contains("Crop(") Then
            Dim sourceWidth = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Width").ToInt
            Dim sourceHeight = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Height").ToInt

            If sourceWidth Mod 4 <> 0 OrElse sourceHeight Mod 4 <> 0 Then
                p.CropRight = sourceWidth Mod 4
                p.CropBottom = sourceHeight Mod 4
                p.Script.ActivateFilter("Crop")
            End If
        End If
    End Sub

    Sub SetSourceFilter(sourceFilter As VideoFilter, preferences As StringPairList, profiles As List(Of FilterCategory), skipAnyExtension As Boolean, skipAnyFormat As Boolean, skipNonAnyExtension As Boolean, skipNonAnyFormat As Boolean)

        If sourceFilter.Script.IndexOf("("c) < 0 Then
            For Each pref In preferences
                Dim extensions = pref.Name.Split({","c, " "c, ";"c}, StringSplitOptions.RemoveEmptyEntries)

                For Each extension In extensions
                    extension = extension.ToLower(InvCult)
                    Dim format = "*"

                    If extension.IndexOf(":"c) >= 0 Then
                        format = extension.Right(":").ToLower(InvCult)
                        extension = extension.Left(":").ToLower(InvCult)
                    End If

                    If skipAnyExtension AndAlso String.Equals(extension, "*") Then Continue For
                    If skipAnyFormat AndAlso String.Equals(format, "*") Then Continue For
                    If skipNonAnyExtension AndAlso Not String.Equals(extension, "*") Then Continue For
                    If skipNonAnyFormat AndAlso Not String.Equals(format, "*") Then Continue For

                    If (String.Equals(extension, p.SourceFile.Ext) OrElse String.Equals(extension, "*")) AndAlso
                        (String.Equals(format, "*") OrElse EqualsExS(format, MediaInfo.GetVideo(p.SourceFile, "Format").ToLower(InvCult))) Then

                        Dim filters = profiles.FirstF(Function(cat) String.Equals(cat.Name, "Source")).Filters.FirstOrDefaultF(Function(cat) EqualsExS(cat.Name, pref.Value))

                        If filters IsNot Nothing Then
                            p.Script.SetFilter("Source", filters.Name, filters.Script)
                            Exit Sub
                        End If
                    End If
                Next
            Next
        End If
    End Sub

    Sub AutoResize()
        If p.Script.IsFilterActive("Resize") Then
            If p.AutoResizeImage <> 0 Then
                SetTargetImageSizeByPixel(p.AutoResizeImage)
            Else
                If p.AdjustHeight Then
                    Dim h = Calc.FixMod(CInt(p.TargetWidth / Calc.GetTargetDAR()), p.ForcedOutputMod)
                    tbTargetHeight.Text = h.ToInvStr()
                End If
            End If
        Else
            Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
            tbTargetWidth.Text = cropw.ToInvStr
            tbTargetHeight.Text = (p.SourceHeight - p.CropTop - p.CropBottom).ToInvStr
        End If
    End Sub

    Sub ConvertBluRaySubtitles()
        If Not p.ConvertSup2Sub Then
            Exit Sub
        End If

        For Each i In g.GetFilesInTempDirAndParent
            If i.Ext.Equals("sup") AndAlso g.IsSourceSameOrSimilar(i) AndAlso Not File.Exists(i.DirAndBase + ".idx") Then
                Using proc As New Proc
                    proc.Header = "Convert sup to sub"
                    proc.SkipStrings = {"#>", "#<", "Decoding frame"}
                    proc.File = Package.BDSup2SubPP.Path
                    proc.Arguments = "-o """ + i.DirAndBase + ".idx"" """ + i + """"
                    proc.AllowedExitCodes = {}
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub ExtractForcedVobSubSubtitles()
        For Each path In g.GetFilesInTempDirAndParent
            If path.ExtFull.Equals(".idx") AndAlso g.IsSourceSameOrSimilar(path) AndAlso
                    Not path.Contains("_forced") AndAlso
                    Not File.Exists(path.DirAndBase & "_forced.idx") Then

                Dim idxContent = path.ReadAllTextDefault

                If idxContent.Contains(VB6.ChrW(&HA) & VB6.ChrW(&H0) & VB6.ChrW(&HD) & VB6.ChrW(&HA)) Then
                    idxContent = idxContent.FixBreak
                    idxContent = idxContent.Replace(BR & VB6.ChrW(&H0) & BR, BR & "langidx: 0" & BR)
                    File.WriteAllText(path, idxContent, Encoding.Default)
                End If

                Using proc As New Proc
                    proc.Header = "Extract forced subtitles if existing"
                    proc.SkipString = "# "
                    proc.WriteLog(path.FileName + BR2)
                    proc.File = Package.BDSup2SubPP.Path
                    proc.Arguments = "--forced-only -o " + (path.DirAndBase + "_forced.idx").Escape + " " + path.Escape
                    proc.AllowedExitCodes = {}
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub DemuxVobSubSubtitles()
        If Not {"vob", "m2v"}.ContainsString(p.LastOriginalSourceFile.Ext) Then
            Exit Sub
        End If

        Dim ifoPath = GetIfoFile()

        If ifoPath.NullOrEmptyS Then
            Exit Sub
        End If

        If File.Exists(p.TempDir + p.SourceFile.Base + ".idx") Then
            Exit Sub
        End If

        Dim subtitleCount = MediaInfo.GetSubtitleCount(ifoPath)

        If subtitleCount > 0 Then
            Dim pgcCount As Byte
            Dim sectorPointer As Integer
            Dim buffer = File.ReadAllBytes(ifoPath)
            sectorPointer = (((buffer(&HCC) * &H1000000) + (buffer(&HCD) * &H10000)) + (buffer(&HCE) * &H100)) + (buffer(&HCF) * 1)
            pgcCount = CByte((buffer(sectorPointer * &H800) * &H100) + buffer((sectorPointer * &H800) + 1))

            If pgcCount <> 0 Then
                For i = 0 To pgcCount - 1
                    Dim a = ((sectorPointer * &H800) + (i * 8)) + 12
                    Dim b = (((buffer(a + 0) * &H1000000) + (buffer(a + 1) * &H10000)) + (buffer(a + 2) * &H100)) + (buffer(a + 3) * 1) + sectorPointer * &H800
                    Dim hour, min, sec As Integer

                    hour = CByte((((buffer(b + 4) And 240) >> 4) * 10) + (buffer(b + 4) And 15))
                    min = CByte((((buffer(b + 5) And 240) >> 4) * 10) + (buffer(b + 5) And 15))
                    sec = CByte((((buffer(b + 6) And 240) >> 4) * 10) + (buffer(b + 6) And 15))

                    If Math.Abs(p.SourceSeconds - ((hour * 60 ^ 2) + (min * 60) + sec)) < 30 Then
                        Dim args =
                            ifoPath + BR +
                            p.TempDir + p.SourceFile.Base + BR &
                            (i + 1) & BR +
                            "1" + BR +
                            "ALL" + BR +
                            "CLOSE"

                        Dim fileContent = p.TempDir + p.TargetFile.Base + "_vsrip.txt"
                        args.WriteFileDefault(fileContent)

                        Using proc As New Proc
                            proc.Header = "Demux subtitles using VSRip"
                            proc.WriteLog(args + BR2)
                            proc.File = Package.VSRip.Path
                            proc.Arguments = """" + fileContent + """"
                            proc.WorkingDirectory = Package.VSRip.Directory
                            proc.AllowedExitCodes = {0, 1, 2}
                            proc.Start()
                        End Using

                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Function ProcessTip(message As String) As Boolean
        message = message.Replace(BR2, BR)

        If message.Contains(VB6.vbLf & VB6.vbLf) Then
            message = message.FixBreak.Replace(BR2, BR)
        End If

        CurrentAssistantTipKey = message.MD5Hash
        ''Shared Mmaaxxxx As Integer 'Debug !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        'If Not p.SkippedAssistantTips.Contains(CurrentAssistantTipKey) Then
        '    If message.NotNullOrEmptyS Then 'Test This, Removed !!!!!!!!!!
        '        'If Mmaaxxxx < message.Length Then Mmaaxxxx = message.Length
        '        If message.Length > 130 Then
        '            laTip.SetFontSize(8 * s.UIScaleFactor)
        '        Else
        '            laTip.SetFontSize(9 * s.UIScaleFactor)
        '        End If
        '    End If
        laTip.Text = message
        Return True
        'End If
    End Function

    Private AssistantMethodValue As Action

    Property AssistantMethod() As Action
        Get
            Return AssistantMethodValue
        End Get
        Set(Value As Action)
            AssistantMethodValue = Value
        End Set
    End Property

    Function Assistant() As Boolean
        If SkipAssistant Then
            Return False
        End If

        Dim isCropped = p.Script.IsFilterActive("Crop")
        Dim isResized = p.Script.IsFilterActive("Resize")

        tbTargetWidth.ReadOnly = Not isResized
        tbTargetHeight.ReadOnly = Not isResized

        g.Highlight(False, lSAR)
        g.Highlight(False, llAudioProfile0)
        g.Highlight(False, llAudioProfile1)
        g.Highlight(False, lAspectRatioError)
        g.Highlight(False, lZoom)
        g.Highlight(False, tbAudioFile0)
        g.Highlight(False, tbAudioFile1)
        g.Highlight(False, tbBitrate)
        g.Highlight(False, tbTargetSize)
        g.Highlight(False, tbSourceFile)
        g.Highlight(False, tbTargetHeight)
        g.Highlight(False, tbTargetFile)
        g.Highlight(False, tbTargetWidth)
        g.Highlight(False, llMuxer)
        g.Highlight(False, lgbEncoder.Label)
        g.Highlight(False, laTarget2)

        Dim cropw = p.SourceWidth
        Dim croph = p.SourceHeight

        If isCropped Then
            cropw = p.SourceWidth - p.CropLeft - p.CropRight
            croph = p.SourceHeight - p.CropTop - p.CropBottom
        End If

        Dim isValidAnamorphicSize = (p.TargetWidth = 1440 AndAlso p.TargetHeight = 1080) OrElse
            (p.TargetWidth = 960 AndAlso p.TargetHeight = 720)

        If Not isResized Then
            If p.TargetWidth <> cropw Then
                tbTargetWidth.Text = cropw.ToInvStr
            End If

            If p.TargetHeight <> croph Then
                tbTargetHeight.Text = croph.ToInvStr
            End If
        End If

        lAspectRatioError.Text = Calc.GetAspectRatioError.ToString("f2") & "%"

        If isCropped Then
            lCrop.Text = cropw.ToInvStr() & "/" & croph.ToInvStr()
        Else
            lCrop.Text = "disabled"
        End If

        Dim widthZoom = p.TargetWidth / cropw * 100
        Dim heightZoom = p.TargetHeight / croph * 100

        lZoom.Text = widthZoom.ToString("f1") & "/" & heightZoom.ToString("f1")
        lPixel.Text = CInt(p.TargetWidth * p.TargetHeight).ToInvStr

        Dim trackBarValue = CInt((p.TargetWidth - 320) / p.ForcedOutputMod)

        If trackBarValue < tbResize.Minimum Then
            trackBarValue = tbResize.Minimum
        End If

        If trackBarValue > tbResize.Maximum Then
            trackBarValue = tbResize.Maximum
        End If

        tbResize.Value = trackBarValue

        Dim par = Calc.GetTargetPAR

        If Calc.IsARSignalingRequired OrElse (par.X = 1 AndAlso par.Y = 1) Then
            lPAR.Text = par.X & ":" & par.Y
        Else
            lPAR.Text = "n/a"
        End If

        lDAR.Text = Calc.GetTargetDAR.ToString.Shorten(8)
        lSAR.Text = (p.TargetWidth / p.TargetHeight).ToString.Shorten(8)
        lSourceDar.Text = Calc.GetSourceDAR.ToString.Shorten(8)
        par = Calc.GetSimpleSourcePAR
        lSourcePAR.Text = par.X & ":" & par.Y

        If p.SourceSeconds > 0 Then
            lSource1.Text = lSource1.GetMaxTextSpace(
                g.GetTimeString(p.SourceSeconds),
                If(p.SourceSize / 1024 ^ 2 < 1024, CInt(p.SourceSize / 1024 ^ 2).ToInvStr & "MB", (p.SourceSize / 1024 ^ 3).ToString("f1") & "GB"),
                If(p.SourceBitrate > 0, (p.SourceBitrate / 1000).ToString("f1") & "Mb/s", ""),
                p.SourceFrameRate.ToString.Shorten(9) & "fps",
                p.SourceVideoFormat, p.SourceVideoFormatProfile)

            lSource2.Text = lSource1.GetMaxTextSpace(
                p.SourceWidth.ToInvStr & "x" & p.SourceHeight.ToInvStr, p.SourceColorSpace,
                p.SourceChromaSubsampling, If(p.SourceVideoBitDepth <> 0, p.SourceVideoBitDepth & "Bits", ""),
                p.SourceScanType, If(p.SourceScanType = "Interlaced", p.SourceScanOrder, ""))

            lTarget1.Text = lSource1.GetMaxTextSpace(g.GetTimeString(p.TargetSeconds),
                p.TargetFrameRate.ToString.Shorten(9) & "fps", "Audio Bitrate: " & CInt(Calc.GetAudioBitrate))

            If p.VideoEncoder.IsCompCheckEnabled Then
                laTarget2.Text = lSource1.GetMaxTextSpace(
                    "Quality: " & CInt(Calc.GetPercent).ToInvStr() & " %",
                    "Compressibility: " & p.Compressibility.ToString("f2"))
            End If
        Else
            lTarget1.Text = ""
            lSource1.Text = ""
            laTarget2.Text = ""
            lSource2.Text = ""
        End If

        laTip.Text = ""
        gbAssistant.Text = ""
        AssistantMethod = Nothing
        CanIgnoreTip = True
        AssistantPassed = False

        If p.VideoEncoder.Muxer.TagFile.NotNullOrEmptyS AndAlso File.Exists(p.VideoEncoder.Muxer.TagFile) AndAlso p.VideoEncoder.Muxer.Tags.Count > 0 Then
            If ProcessTip("In the container options there is both a tag file and tags in the Tags tab defined. Only one can be used, the file will be ignored.") Then
                gbAssistant.Text = "Tags are defined twice"
                Return False
            End If
        End If

        If p.VideoEncoder.Muxer.CoverFile.NotNullOrEmptyS AndAlso TypeOf p.VideoEncoder.Muxer Is MkvMuxer Then
            If Not p.VideoEncoder.Muxer.CoverFile.Base.EqualsAny("cover", "small_cover", "cover_land", "small_cover_land") OrElse Not p.VideoEncoder.Muxer.CoverFile.Ext.EqualsAny("jpg", "png") Then
                If ProcessTip("The cover file name bust be cover, small_cover, cover_land or small_cover_land, the file type must be jpg or png.") Then
                    gbAssistant.Text = "Invalid cover file name"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        If TypeOf p.VideoEncoder Is BasicVideoEncoder Then
            Dim enc = DirectCast(p.VideoEncoder, BasicVideoEncoder)
            Dim param = enc.CommandLineParams.GetOptionParam("--vpp-resize")

            If param IsNot Nothing AndAlso param.Value > 0 AndAlso
                Not p.Script.IsFilterActive("Resize", "Hardware Encoder") Then

                If ProcessTip("In order to use a resize filter of the hardware encoder select 'Hardware Encoder' as resize filter from the filters menu.") Then
                    gbAssistant.Text = "Invalid filter setting"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        If Not p.BatchMode Then
            If p.Script.Filters.Count = 0 OrElse
                Not p.Script.Filters(0).Active OrElse
               Not String.Equals(p.Script.Filters(0).Category, "Source") Then

                If ProcessTip("The first filter must have the category Source.") Then
                    gbAssistant.Text = "Invalid filter setup"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.SourceSeconds = 0 Then
                If ProcessTip("Click here to open a source file.") Then
                    AssistantMethod = AddressOf ShowOpenSourceDialog
                    gbAssistant.Text = "Assistant"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If EqualsExS(p.SourceFile, p.TargetFile) Then
                If ProcessTip("The source and target filepath is identical.") Then
                    g.Highlight(True, tbTargetFile)
                    gbAssistant.Text = "Invalid Targetpath"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.RemindToCrop AndAlso TypeOf p.VideoEncoder IsNot NullEncoder AndAlso
                ProcessTip("Click here to open the crop dialog. When done continue with Next.") Then

                AssistantMethod = AddressOf ShowCropDialog
                gbAssistant.Text = "Crop"
                Return False
            End If

            If (EqualsExS(p.Audio0.File, p.Audio1.File) AndAlso p.Audio0.Stream Is Nothing) OrElse
                (p.Audio0.Stream IsNot Nothing AndAlso p.Audio1.Stream IsNot Nothing AndAlso
                p.Audio0.Stream.StreamOrder = p.Audio1.Stream.StreamOrder) Then

                If ProcessTip("The first and second audio source files or streams are identical.") Then
                    g.Highlight(True, tbAudioFile0)
                    g.Highlight(True, tbAudioFile1)
                    gbAssistant.Text = "Invalid Audio Settings"
                    Return False
                End If
            End If

            If Not p.VideoEncoder.Muxer.IsSupported(p.VideoEncoder.OutputExt) Then
                If ProcessTip("The encoder outputs '" & p.VideoEncoder.OutputExt & "' but the container '" & p.VideoEncoder.Muxer.Name & "' supports only " & String.Join(", ", p.VideoEncoder.Muxer.SupportedInputTypes) & ".") Then
                    gbAssistant.Text = "Encoder conflicts with container"
                    g.Highlight(True, llMuxer)
                    g.Highlight(True, lgbEncoder.Label)
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            For Each ap In g.GetAudioProfiles
                If ap.File.EqualsExS(p.TargetFile) Then
                    If ProcessTip("The audio source and target filepath is identical.") Then
                        g.Highlight(True, tbTargetFile)
                        gbAssistant.Text = "Invalid Targetpath"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If

                If Math.Abs(ap.Delay) > 2000 Then
                    If ProcessTip("The audio delay is unusual high indicating a sync problem.") Then
                        g.Highlight(True, tbAudioFile0)
                        gbAssistant.Text = "Unusual high audio delay"
                        Return False
                    End If
                End If

                If ap.File.NotNullOrEmptyS AndAlso Not p.VideoEncoder.Muxer.IsSupported(ap.OutputFileType) AndAlso Not String.Equals(ap.OutputFileType, "ignore") Then
                    If ProcessTip("The audio format is '" & ap.OutputFileType & "' but the container '" & p.VideoEncoder.Muxer.Name & "' supports only " & String.Join(", ", p.VideoEncoder.Muxer.SupportedInputTypes) & ". Select another audio profile or another container.") Then
                        g.Highlight(True, llMuxer)
                        gbAssistant.Text = "Audio format conflicts with container"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If
            Next

            If Not EqualsExS(p.VideoEncoder.Muxer.OutputExtFull, p.TargetFile.ExtFull) Then
                If ProcessTip("The container requires " & p.VideoEncoder.Muxer.OutputExt.ToUpper(InvCult) & " as target file type.") Then
                    g.Highlight(True, tbTargetFile)
                    gbAssistant.Text = "Invalid File Type"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If Not p.VideoEncoder.GetError Is Nothing Then
                If ProcessTip(p.VideoEncoder.GetError) Then
                    gbAssistant.Text = "Encoder Error"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            Dim ae = Calc.GetAspectRatioError()

            If Not isValidAnamorphicSize AndAlso (ae > p.MaxAspectRatioError OrElse
                ae < -p.MaxAspectRatioError) AndAlso isResized AndAlso
                p.RemindArError AndAlso Not String.Equals(p.CustomTargetPAR, "1:1") Then

                If ProcessTip("Use the resize slider to correct the aspect ratio error or click next to encode anamorphic.") Then
                    g.Highlight(True, lAspectRatioError)
                    gbAssistant.Text = "Aspect Ratio Error"
                    Return False
                End If
            End If

            If p.RemindToSetFilters AndAlso ProcessTip("Verify the filter setup, when done continue with Next.") Then
                gbAssistant.Text = "Filter Setup"
                Return False
            End If

            If p.Ranges.Count = 0 Then
                If p.RemindToCut AndAlso Not TypeOf p.VideoEncoder Is NullEncoder AndAlso
                    ProcessTip("Click here to open the preview for cutting if necessary. When done continue with Next.") Then

                    AssistantMethod = AddressOf ShowPreview
                    gbAssistant.Text = "Cutting"
                    Return False
                End If
            Else
                If p.CutFrameRate <> p.Script.GetFramerate Then
                    If ProcessTip("The frame rate was changed after cutting was performed, please ensure that this change is happening after the Cutting filter section in the AviSynth script.") Then
                        gbAssistant.Text = "Illegal frame rate change"
                        Return False
                    End If
                End If

                If Not p.Script.IsFilterActive("Cutting") AndAlso Form.ActiveForm Is Me Then
                    If ProcessTip("The cutting filter settings don't match with the cutting settings used in the preview.") Then
                        gbAssistant.Text = "Invalid Cutting Settings"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If
            End If

            If p.RemindToDoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility = 0 Then
                If ProcessTip("Click here to start the compressibility check. The compressibility check helps to finds the ideal bitrate or image size.") Then
                    AssistantMethod = AddressOf p.VideoEncoder.RunCompCheck
                    g.Highlight(True, laTarget2)
                    gbAssistant.Text = "Compressibility Check"
                    Return False
                End If
            End If

            If File.Exists(p.TargetFile) Then
                If FileTypes.VideoText.ContainsString(p.SourceFile.Ext) AndAlso
                    p.SourceFile.ReadAllText.Contains(p.TargetFile) Then

                    If ProcessTip("Source and target name are identical, please select another target name.") Then
                        CanIgnoreTip = False
                        tbTargetFile.BackColor = Color.Yellow
                        gbAssistant.Text = "Target File"
                        Return False
                    End If
                Else
                    If ProcessTip("The target file already exist." & BR & p.TargetFile) Then
                        tbTargetFile.BackColor = Color.Yellow
                        gbAssistant.Text = "Target File"
                        Return False
                    End If
                End If
            End If

            If p.Script.Info.Width Mod p.ForcedOutputMod <> 0 Then
                If ProcessTip("Change output width to be divisible by " & p.ForcedOutputMod &
                              " or customize:" & BR & "Options > Image > Output Mod") Then
                    CanIgnoreTip = Not p.AutoCorrectCropValues
                    g.Highlight(True, tbTargetWidth)
                    g.Highlight(True, lSAR)
                    gbAssistant.Text = "Invalid Target Width"
                    Return False
                End If
            End If

            If p.Script.Info.Height Mod p.ForcedOutputMod <> 0 Then
                If ProcessTip("Change output height to be divisible by " & p.ForcedOutputMod &
                              " or customize:" & BR & "Options > Image > Output Mod") Then
                    CanIgnoreTip = Not p.AutoCorrectCropValues
                    g.Highlight(True, tbTargetHeight)
                    g.Highlight(True, lSAR)
                    gbAssistant.Text = "Invalid Target Height"
                    Return False
                End If
            End If

            If p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility > 0 Then
                Dim value = Calc.GetPercent

                If value < (p.VideoEncoder.AutoCompCheckValue - 20) OrElse
                    value > (p.VideoEncoder.AutoCompCheckValue + 20) Then

                    If ProcessTip("Aimed quality value is more than 20% off, change the image or file size to get something between 50% and 70% quality.") Then
                        g.Highlight(True, tbTargetSize)
                        g.Highlight(True, tbBitrate)
                        g.Highlight(True, tbTargetWidth)
                        g.Highlight(True, tbTargetHeight)
                        g.Highlight(True, laTarget2)
                        laTarget2.BackColor = Color.Red
                        gbAssistant.Text = "Quality"
                        Return False
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder.Muxer Is MP4Muxer Then
                For Each i In p.VideoEncoder.Muxer.Subtitles
                    If Not {"idx", "srt"}.ContainsString(i.Path.Ext) Then
                        If ProcessTip("MP4 supports only SRT and IDX subtitles.") Then
                            CanIgnoreTip = False
                            gbAssistant.Text = "Invalid subtitle format"
                            Return False
                        End If
                    End If
                Next
            End If

            If Not (MouseButtons = MouseButtons.Left AndAlso ActiveControl Is tbResize) Then
                Dim err = p.Script.GetError
                Dim title = If(err.Count(Function(chr) chr = BR(1)) > 2, "Click Preview to show full error", "Script Error")

                If err.NotNullOrEmptyS AndAlso Not g.VerifyRequirements Then
                    If ProcessTip(err) Then
                        CanIgnoreTip = False
                        gbAssistant.Text = title
                        Return False
                    End If
                End If

                If err.NotNullOrEmptyS Then
                    If ProcessTip(err) Then
                        CanIgnoreTip = False
                        gbAssistant.Text = title
                        Return False
                    End If
                End If
            End If
        Else
            If p.SourceFiles.Count = 0 Then
                If ProcessTip("Click here to open a source file.") Then
                    AssistantMethod = AddressOf ShowOpenSourceDialog
                    gbAssistant.Text = "Assistant"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        gbAssistant.Text = "Add Job"

        If s.UIScaleFactor <> 1 AndAlso laTip.Font.Size <> (9 * s.UIScaleFactor) Then
            laTip.SetFontSize(9 * s.UIScaleFactor)
        End If

        laTip.Text = "Click on the next button to add a job."
        AssistantPassed = True
    End Function

    Sub OpenTargetFolder()
        g.ShellExecute(p.TargetFile.Dir)
    End Sub

    Dim BlockAudioTextChanged As Boolean

    Sub AudioTextChanged(tb As TextBox, ap As AudioProfile)
        If BlockAudioTextChanged Then
            Exit Sub
        End If

        If System.Text.Encoding.Default.CodePage <> 65001 AndAlso
             p.Script.Engine = ScriptEngine.AviSynth AndAlso Not tb.Text.IsANSICompatible Then

            MsgWarn(Strings.NoUnicode)
            tb.Text = ""
            Exit Sub
        End If

        If tb.Text.Contains(":\") OrElse tb.Text.StartsWith("\\", StringComparison.Ordinal) Then
            If EqualsExS(tb.Text, ap.File) Then
                ap.File = tb.Text

                If FileTypes.Audio.ContainsString(ap.File.Ext) Then
                    If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                        ap.Delay = g.ExtractDelay(ap.File)
                    End If

                    If ap.StreamName.NullOrEmptyS AndAlso ap.File.Contains("{") Then
                        Dim title = ap.File.Right("{")
                        ap.StreamName = title.Left("}").UnescapeIllegalFileSysChars
                    End If
                End If

                ap.SetStreamOrLanguage()
            End If

            UpdateSizeOrBitrate()
            BlockAudioTextChanged = True
            tb.Text = ap.DisplayName

            If tb Is tbAudioFile0 Then
                llAudioProfile0.Text = g.ConvertPath(ap.Name)
            Else
                llAudioProfile1.Text = g.ConvertPath(ap.Name)
            End If

            BlockAudioTextChanged = False
        ElseIf tb.Text.NullOrEmptyS Then
            ap.File = ""
            UpdateSizeOrBitrate()
        End If
    End Sub

    Sub tbAudioFile0_TextChanged() Handles tbAudioFile0.TextChanged
        AudioTextChanged(tbAudioFile0, p.Audio0)
    End Sub

    Sub tbAudioFile1_TextChanged() Handles tbAudioFile1.TextChanged
        AudioTextChanged(tbAudioFile1, p.Audio1)
    End Sub

    Function AbortDueToLowDiskSpace() As Boolean
        Try 'crashes with network shares
            If p.TargetFile.NullOrEmptyS OrElse p.TargetFile.StartsWith("\\", StringComparison.Ordinal) Then
                Exit Function
            End If

            Dim di As New DriveInfo(p.TargetFile.Dir)

            If di.AvailableFreeSpace / 1024 ^ 3 < s.MinimumDiskSpace Then
                Using td As New TaskDialog(Of String)
                    td.MainInstruction = "Low Disk Space"
                    td.Content = $"The target drive {Path.GetPathRoot(p.TargetFile)} has only " +
                                 $"{(di.AvailableFreeSpace / 1024 ^ 3).ToString("f2")} GB free disk space."
                    td.MainIcon = TaskDialogIcon.Warning
                    td.AddButton("Continue", "Continue")
                    td.AddButton("Abort", "Abort")

                    If Not td.Show.Equals("Continue") Then
                        Return True
                    End If
                End Using
            End If
        Catch
        End Try
    End Function

    <Command("Creates a job and runs the job list.")>
    Sub StartEncoding()
        AssistantPassed = True
        AddJob(False, "")
        g.ProcessJobs()
    End Sub

    Sub Demux()
        Dim getFormat = Function() As String
                            Dim ret = MediaInfo.GetVideo(p.SourceFile, "Format")

                            Select Case ret
                                Case "MPEG Video"
                                    ret = "mpeg2"
                                Case "VC-1"
                                    ret = "vc1"
                            End Select

                            Return ret.ToLower(InvCult)
                        End Function

        Dim srcScript = p.Script.GetFilter("Source").Script.ToLower(InvCult)

        For Each i In s.Demuxers
            If Not i.Active AndAlso (i.SourceFilters.NothingOrEmpty OrElse
                Not srcScript.ContainsAny(i.SourceFilters.SelectF(Function(val) val.ToLower(InvCult) & "("))) Then

                Continue For
            End If

            If i.InputExtensions?.Length = 0 OrElse i.InputExtensions.ContainsString(p.SourceFile.Ext) Then
                If srcScript?.IndexOf("("c) < 0 OrElse i.SourceFilters.NothingOrEmpty OrElse
                    srcScript.ContainsAny(i.SourceFilters.SelectF(Function(val) val.ToLower(InvCult) & "(")) Then

                    Dim inputFormats = i.InputFormats.NothingOrEmpty OrElse
                        i.InputFormats.ContainsString(getFormat())

                    If inputFormats Then
                        i.Run(p)
                        Refresh()

                        For Each iExt In i.OutputExtensions
                            Dim exitFor = False

                            For Each iFile In Directory.GetFiles(p.TempDir, "*." & iExt)
                                If g.IsSourceSame(iFile) AndAlso
                                   Not String.Equals(p.SourceFile, iFile) AndAlso
                                    Not iFile.Base.EndsWith("_out", StringComparison.Ordinal) AndAlso
                                    Not iFile.Base.Contains("_cut_") Then

                                    p.SourceFile = iFile
                                    p.SourceFiles.Clear()
                                    p.SourceFiles.Add(p.SourceFile)

                                    BlockSourceTextBoxTextChanged = True
                                    tbSourceFile.Text = p.SourceFile
                                    BlockSourceTextBoxTextChanged = False

                                    exitFor = True
                                    Exit For
                                End If
                            Next

                            If exitFor Then
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
        Next

        p.Script.Synchronize()
    End Sub

    'Private BlockIndexingRecursion As Boolean = False 'Dead Code ???

    Sub Indexing()
        If p.SourceFile.Ext.EqualsAny("avs", "vpy") Then
            Exit Sub
        End If

        Dim codeLower = p.Script.GetFilter("Source").Script.ToLower(InvCult)

        If codeLower.Contains("ffvideosource(") OrElse codeLower.Contains("ffms2.source") Then
            If FileTypes.VideoIndex.ContainsString(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If

            If codeLower.Contains("cachefile") AndAlso p.TempDir.EndsWithEx("_temp\") Then
                g.ffmsindex(p.SourceFile, p.TempDir + g.GetSourceBase + ".ffindex")
            Else
                g.ffmsindex(p.SourceFile, p.SourceFile + ".ffindex")
            End If
        ElseIf codeLower.Contains("lwlibavvideosource(") OrElse codeLower.Contains("lwlibavsource(") Then
            If FileTypes.VideoIndex.ContainsString(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If

            If Not File.Exists(p.SourceFile + ".lwi") AndAlso
                Not File.Exists(p.TempDir + g.GetSourceBase + ".lwi") AndAlso
                File.Exists(p.Script.Path) AndAlso Not FileTypes.VideoText.ContainsString(p.SourceFile.Ext) Then

                Using proc As New Proc
                    proc.Header = "Index LWLibav"
                    proc.Encoding = Encoding.UTF8
                    proc.SkipString = "Creating lwi"

                    If p.Script.Engine = ScriptEngine.AviSynth Then
                        proc.File = Package.ffmpeg.Path
                        proc.Arguments = "-i " + p.Script.Path.Escape + " -hide_banner"
                    Else
                        proc.File = Package.vspipe.Path
                        proc.Arguments = p.Script.Path.Escape + " NUL -i"
                    End If

                    proc.AllowedExitCodes = {0, 1}
                    proc.Start()
                End Using
            End If
        ElseIf codeLower.Contains("lsmashvideosource(") OrElse
            codeLower.Contains("libavsmashsource(") OrElse
            codeLower.Contains("directshowsource(") OrElse
            codeLower.Contains("dss2(") Then

            If FileTypes.VideoIndex.ContainsString(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If

            'DGIndexNV Back
        ElseIf codeLower.Contains("dgsource(") AndAlso Not p.SourceFile.Ext.Equals("dgi") Then
            If FileTypes.VideoIndex.ContainsString(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
            End If

            Dim dgIndexNV = Demuxer.GetDefaults.Find(Function(demuxer) String.Equals(demuxer.Name, "DGIndexNV: Index, No Demux"))
            Dim outFile = p.TempDir & p.SourceFile.Base & ".dgi"

            If Not File.Exists(outFile) Then
                dgIndexNV.Run(p)
            End If

            If File.Exists(outFile) Then
                p.SourceFile = outFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = outFile
                BlockSourceTextBoxTextChanged = False
            End If

        End If
    End Sub

    <Command("Shows a file browser to open a project file.")>
    Sub ShowFileBrowserToOpenProject()
        Using dialog As New OpenFileDialog
            dialog.Filter = "Project Files|*.srip"

            If dialog.ShowDialog() = DialogResult.OK Then
                Refresh()
                OpenProject(dialog.FileName)
            End If
        End Using
    End Sub

    Function LoadTemplateWithSelectionDialog() As Boolean
        Dim td As New TaskDialog(Of String)
        td.MainInstruction = "Please select a template"

        For Each fp In Directory.GetFiles(Folder.Template, "*.srip")
            td.AddCommand(fp.Base, fp)
        Next

        If td.Show.NotNullOrEmptyS Then
            Return OpenProject(td.SelectedValue, True)
        End If
    End Function

    <Command("Shows the Event Command dialog.")>
    Sub ShowEventCommandsDialog()
        Using form As New EventCommandsEditor(s.EventCommands)
            If form.ShowDialog() = DialogResult.OK Then
                s.EventCommands.Clear() 'What is capacity ???

                For Each i As ListViewItem In form.lv.Items
                    s.EventCommands.Add(DirectCast(i.Tag, EventCommand))
                Next
            End If
        End Using

        g.SaveSettings()
    End Sub

    Function GetRestartID() As String
        Return s.AviSynthMode & s.VapourSynthMode
    End Function

    <Command("Shows the settings dialog.")>
    Sub ShowSettingsDialog()
        Dim restartID = GetRestartID()

        Using form As New SimpleSettingsForm("Settings")
            Dim ui = form.SimpleUI
            ui.Store = s

            '################# General
            Dim pageGen = ui.CreateFlowPage("General")
            pageGen.SuspendLayout()

            Dim b = ui.AddBool(pageGen)
            b.Text = "Check for updates once per day"
            b.Field = NameOf(s.CheckForUpdates)

            b = ui.AddBool(pageGen)
            b.Text = "Show template selection when loading new files"
            b.Field = NameOf(s.ShowTemplateSelection)

            b = ui.AddBool(pageGen)
            b.Text = "Reverse mouse wheel video seek direction"
            b.Field = NameOf(s.ReverseVideoScrollDirection)

            b = ui.AddBool(pageGen)
            b.Text = "Write Event Commands to log file"
            b.Field = NameOf(s.LogEventCommand)

            b = ui.AddBool(pageGen)
            b.Text = "Enable debug logging"
            b.Field = NameOf(s.WriteDebugLog)

            pageGen.ResumeLayout(False)

            Dim mb = ui.AddMenu(Of String)(pageGen)
            mb.Text = "Startup Template"
            mb.Help = "Template loaded when StaxRip starts."
            mb.Field = NameOf(s.StartupTemplate)
            mb.Add(Directory.GetFiles(Folder.Template).SelectF(Function(i) i.Base))

            pageGen.SuspendLayout()
            Dim n = ui.AddNum(pageGen)
            n.Text = "Number of log files to keep"
            n.Help = "Log files can be found at: Tools > Folders > Log Files"
            n.Config = {0, Integer.MaxValue}
            n.Field = NameOf(s.LogFileNum)

            n = ui.AddNum(pageGen)
            n.Text = "Number of most recently used projects to keep"
            n.Help = "MRU list shown in the main menu under: File > Recent Projects"
            n.Config = {0, 15}
            n.Field = NameOf(s.ProjectsMruNum)

            n = ui.AddNum(pageGen)
            n.Text = "Maximum number of parallel processes"
            n.Help = "Maximum number of parallel processes used for audio and video processing. Chunk encoding can be enabled in the x265 dialog."
            n.Config = {1, 16}
            n.Field = NameOf(s.ParallelProcsNum)

            n = ui.AddNum(pageGen)
            n.Text = "Preview size compared to screen size (percent)"
            n.Config = {10, 99, 5}
            n.Field = NameOf(s.PreviewSize)
            pageGen.ResumeLayout()

            '################# Frameserver
            Dim page = ui.CreateFlowPage("Frameserver", True)

            Dim avsMode = ui.AddMenu(Of FrameServerMode)(page)
            avsMode.Text = "AviSynth Mode"
            avsMode.Field = NameOf(s.AviSynthMode)

            Dim vsMode = ui.AddMenu(Of FrameServerMode)(page)
            vsMode.Text = "VapourSynth Mode"
            vsMode.Field = NameOf(s.VapourSynthMode)

            b = ui.AddBool(page)
            b.Text = "Load AviSynth plugins"
            b.Help = "Detects and adds necessary LoadPlugin calls."
            b.Field = NameOf(s.LoadAviSynthPlugins)

            b = ui.AddBool(page)
            b.Text = "Load VapourSynth plugins"
            b.Help = "Detects and adds necessary LoadPlugin calls."
            b.Field = NameOf(s.LoadVapourSynthPlugins)

            '################# User Interface
            page = ui.CreateFlowPage("User Interface", True)

            Dim l = ui.AddLabel("Icon File")
            l.Help = "The Windows Startmenu uses Windows Links which allow to use custom icon files."

            Dim tb = ui.AddTextButton(page)
            tb.Label.Visible = False
            tb.BrowseFile("ico|*.ico", Folder.Startup + "Apps\Icons")
            tb.Edit.Expand = True
            tb.Edit.Text = s.IconFile
            tb.Edit.SaveAction = Sub(value) s.IconFile = value

            Dim renderMode = ui.AddMenu(Of ToolStripRenderModeEx)(page)
            renderMode.Text = "Menu Style"
            renderMode.Help = "Defines the style used to render main menus, context menus and toolbars."
            renderMode.Field = NameOf(s.ToolStripRenderModeEx)

            Dim t = ui.AddText(page)
            t.Text = "Remember Window Positions"
            t.Help = "Title or beginning of the title of windows of which the location should be remembered. For all windows enter '''all'''."
            t.Label.Offset = 12
            t.Edit.Expand = True
            t.Edit.Text = String.Join(", ", s.WindowPositionsRemembered)
            t.Edit.SaveAction = Sub(value) s.WindowPositionsRemembered = value.SplitNoEmptyAndWhiteSpace(","c)

            n = ui.AddNum(page)
            n.Text = "UI Scale Factor"
            n.Help = "Requires to restart StaxRip."
            n.Config = {0.3, 3, 0.05, 2}
            n.Field = NameOf(s.UIScaleFactor)

            b = ui.AddBool(page)
            b.Text = "Enable tooltips in menus (restart required)"
            b.Help = "If you disable this you can still right-click menu items to show the tooltip."
            b.Field = NameOf(s.EnableTooltips)

            '############# Preprocessing
            ui.AddControlPage(New PreprocessingControl, "Preprocessing").FormSizeScaleFactor = New Size(50, 24)  '(40, 22)
            ui.FormSizeScaleFactor = New Size(37, 24) '(33, 22)

            '############# System
            Dim systemPage = ui.CreateFlowPage("System", True)

            Dim procPriority = ui.AddMenu(Of ProcessPriorityClass)(systemPage)
            procPriority.Text = "Process Priority"
            procPriority.Help = "Process priority of the applications StaxRip launches."
            procPriority.Field = NameOf(s.ProcessPriority)

            Dim tempDelete = ui.AddMenu(Of DeleteMode)(systemPage)
            tempDelete.Text = "Delete temp files"
            tempDelete.Field = NameOf(s.DeleteTempFilesMode)

            n = ui.AddNum(systemPage)
            n.Text = "Minimum Disk Space"
            n.Help = "Minimum allowed disk space in GB before StaxRip shows an error message."
            n.Config = {0, 10000}
            n.Field = NameOf(s.MinimumDiskSpace)

            n = ui.AddNum(systemPage)
            n.Text = "Shutdown Timeout"
            n.Help = "Timeout in seconds before the shutdown is executed."
            n.Field = NameOf(s.ShutdownTimeout)

            b = ui.AddBool(systemPage)
            b.Text = "Prevent system entering standby mode while encoding"
            b.Field = NameOf(s.PreventStandby)

            b = ui.AddBool(systemPage)
            b.Text = "Minimize processing dialog to tray"
            b.Field = NameOf(s.MinimizeToTray)

            '############# Video
            Dim videoPage = ui.CreateFlowPage("Video", True)

            b = ui.AddBool(videoPage)
            b.Text = "Add filter to convert chroma subsampling to 4:2:0"
            b.Help = "After a source is loaded, automatically add a filter to convert chroma subsampling to 4:2:0"
            b.Field = NameOf(s.ConvertChromaSubsampling)

            n = ui.AddNum(videoPage)
            n.Text = "Number of frames used for auto crop"
            n.Config = {5, 20}
            n.Field = NameOf(s.CropFrameCount)

            '############# Source Filters
            Dim bsAVS = AddFilterPreferences(ui, "Source Filters | AviSynth", s.AviSynthFilterPreferences, s.AviSynthProfiles)
            Dim bsVS = AddFilterPreferences(ui, "Source Filters | VapourSynth", s.VapourSynthFilterPreferences, s.VapourSynthProfiles)

            '############### Danger Zone
            Dim dangerZonePage = ui.CreateFlowPage("Danger Zone", True)

            l = ui.AddLabel(dangerZonePage, "")

            l = ui.AddLabel(dangerZonePage, "Don't change Danger Zone settings unless you are" + BR +
                            "an absolute power user with debugging experience." + BR)

            l.ForeColor = Color.Red

            l = ui.AddLabel(dangerZonePage, "")

            b = ui.AddBool(dangerZonePage)
            b.Text = "Allow using tools with unknown version"
            b.Field = NameOf(s.AllowToolsWithWrongVersion)

            b = ui.AddBool(dangerZonePage)
            b.Text = "Allow using custom paths in startup folder"
            b.Field = NameOf(s.AllowCustomPathsInStartupFolder)

            b = ui.AddBool(dangerZonePage)
            b.Text = "Verify tool status"
            b.Field = NameOf(s.VerifyToolStatus)

            n = ui.AddNum(dangerZonePage)
            n.Text = "Character limit for folders"
            n.Help = "Character limit of source file folder paths. Windows does not have usable long path support."
            n.Config = {50, 900, 10}
            n.Field = NameOf(s.CharacterLimitFolder)

            n = ui.AddNum(dangerZonePage)
            n.Text = "Character limit for filenames"
            n.Help = "Windows does not have usable long path support."
            n.Config = {20, 200, 10}
            n.Field = NameOf(s.CharacterLimitFilename)

            'ffMpeg LogLevel
            Dim mFFLog = ui.AddMenu(Of FfLogLevel)(dangerZonePage)
            mFFLog.Text = "Apps logging level"
            mFFLog.Field = NameOf(s.FfmpegLogLevel)

            ui.SelectLast("last settings page")

            If form.ShowDialog() = DialogResult.OK Then
                s.AviSynthFilterPreferences = DirectCast(bsAVS.DataSource, StringPairList)
                s.AviSynthFilterPreferences.Sort()
                s.VapourSynthFilterPreferences = DirectCast(bsVS.DataSource, StringPairList)
                s.VapourSynthFilterPreferences.Sort()
                ui.Save()
                g.SetRenderer(MenuStrip)
                SetMenuStyle()
                s.UpdateRecentProjects(Nothing)
                UpdateRecentProjectsMenu()

                If Not Icon Is g.Icon Then
                    Icon = g.Icon
                End If

                g.SaveSettings()
            End If

            If Not restartID.Equals(GetRestartID()) Then
                MsgInfo("Please restart StaxRip.")
            End If

            ui.SaveLast("last settings page")
        End Using
    End Sub

    Function AddFilterPreferences(ui As SimpleUI, pagePath As String, preferences As StringPairList, profiles As List(Of FilterCategory)) As BindingSource
        Dim filterPage = ui.CreateDataPage(pagePath)
        Dim fn = Function() As StringPairList
                     Dim ret As New StringPairList From {{" Filters Menu", "StaxRip allows to assign a source filter profile to a particular source file type or format. The source filter profiles can be customized by right-clicking the filters menu in the main dialog."}}

                     For Each i In profiles.FirstF(Function(v) String.Equals(v.Name, "Source")).Filters
                         If i.Script.NotNullOrEmptyS Then
                             ret.Add(i.Name, i.Script)
                         End If
                     Next

                     Return ret
                 End Function

        filterPage.TipProvider.TipsFunc = fn

        Dim c1 = filterPage.AddTextBoxColumn()
        c1.DataPropertyName = "Name"
        c1.HeaderText = "Extension[:Format]"

        Dim c2 = filterPage.AddComboBoxColumn
        c2.DataPropertyName = "Value"
        c2.HeaderText = "Source Filter"

        Dim filterNames = profiles.FirstF(Function(v) String.Equals(v.Name, "Source")).Filters.
            WhereSelectF(Function(v) Not String.Equals(v.Name, "Automatic") AndAlso Not String.Equals(v.Name, "Manual") AndAlso Not v.Name.EndsWith("...", StringComparison.Ordinal), Function(v) v.Name).ToArray
        Array.Sort(filterNames, StringComparer.Ordinal)

        c2.Items.AddRange(filterNames)

        filterPage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        Dim ret2 As New BindingSource With {.DataSource = New StringPairList(preferences.WhereF(Function(a) filterNames.ContainsString(a.Value) AndAlso a.Name.NotNullOrEmptyS)).GetDeepClone}
        'ret2.DataSource = ObjectHelp.GetCopy(New StringPairList(preferences.Where(Function(a) filterNames.ContainsString(a.Value) AndAlso a.Name.NotNullOrEmptyS))) 'Is deepClone needed???

        filterPage.DataSource = ret2
        DirectCast(filterPage, ISupportInitialize).EndInit()
        Return ret2
    End Function

    <Command("Saves the current project.")>
    Sub SaveProject()
        If g.ProjectPath Is Nothing Then
            OpenSaveProjectDialog()
        Else
            SaveProjectPath(g.ProjectPath)
        End If
    End Sub

    <Command("Saves the current project at the specified path.")>
    Sub SaveProjectPath(<Description("The path may contain macros."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> path As String)

        path = Macro.Expand(path)

        If path.StartsWith(Folder.Template, StringComparison.Ordinal) Then
            If p.SourceFile.NotNullOrEmptyS Then
                MsgWarn("A template cannot be created after a source file was opened.")
                Exit Sub
            End If
        Else
            g.ProjectPath = path
        End If

        Try
            SafeSerialization.Serialize(p, path)
            SetSavedProject()
            Text = path.Base + " - " + Application.ProductName + " " + Application.ProductVersion
            s.UpdateRecentProjects(path)
            UpdateRecentProjectsMenu()
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Saves the current project.")>
    Sub SaveProjectAs()
        OpenSaveProjectDialog()
    End Sub

    <Command("Saves the current project as template.")>
    Sub SaveProjectAsTemplate()
        If p.SourceFile.NullOrEmptyS Then
            Dim box As New InputBox With {
                .Text = "Enter the name of the template.",
                .Title = "Save Template",
                .Value = p.TemplateName,
                .CheckBoxText = "Load template on startup"}

            Dim invFileNameChHs = Task.Run(Function() Path.GetInvalidFileNameChars.ToHashSet)
            If box.Show = DialogResult.OK Then
                p.TemplateName = box.Value.RemoveChars(invFileNameChHs.Result)
                SaveProjectPath(Folder.Template & p.TemplateName & ".srip")
                UpdateTemplatesMenuAsync()

                If box.Checked Then
                    s.StartupTemplate = box.Value
                End If
            End If
        Else
            MsgWarn("A template cannot be created after a source file was opened.")
        End If
    End Sub

    <Command("Starts the compressibility check.")>
    Sub StartCompCheck()
        p.VideoEncoder.RunCompCheck()
    End Sub

    <Command("Exits StaxRip")>
    Sub [Exit]()
        Close()
    End Sub

    <Command("Dialog to manage external tools.")>
    Sub ShowAppsDialog()
        Using form As New AppsForm
            Dim found As Boolean
            Dim valD As String

            If s.StringDictionary.TryGetValue("RecentExternalApplicationControl", valD) Then
                For Each pack In Package.Items.Values
                    If EqualsExS(pack.Name & pack.Version, valD) Then
                        form.ShowPackage(pack)
                        found = True
                        Exit For
                    End If
                Next
            End If

            If Not found Then
                form.ShowPackage(Package.x265)
            End If

            form.ShowDialog()
            g.SaveSettings()
        End Using
    End Sub

    <Command("Shows a dialog to manage video encoder profiles.")>
    Sub ShowEncoderProfilesDialog()
        Using form As New ProfilesForm("Encoder Profiles", s.VideoEncoderProfiles, AddressOf g.LoadVideoEncoder, AddressOf GetNewVideoEncoderProfile, AddressOf VideoEncoder.GetDefaults)

            If form.ShowDialog() = DialogResult.OK Then
                PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
            End If
        End Using
    End Sub

    <Command("Dialog to manage Muxer profiles.")>
    Sub ShowMuxerProfilesDialog()
        If p.VideoEncoder.OpenMuxerProfilesDialog() = DialogResult.OK Then
            PopulateProfileMenu(DynamicMenuItemID.MuxerProfiles)
        End If

        Assistant()
    End Sub

    Sub RecalcBitrate()
        UpdateSizeOrBitrate()
    End Sub

    Function GetNewVideoEncoderProfile() As Profile
        Dim sb As New SelectionBox(Of VideoEncoder)
        sb.Title = "Add New Profile"
        sb.Text = "Please select a profile from the defaults."
        sb.AddItem("Current Project", p.VideoEncoder)

        For Each i In VideoEncoder.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedValue
        End If
    End Function

    <Command("Placeholder for dynamically updated menu items.")>
    Sub DynamicMenuItem(<DispName("ID")> id As DynamicMenuItemID)
    End Sub

    Sub PopulateProfileMenu(id As DynamicMenuItemID)
        For Each i In CustomMainMenu.MenuItems.OfType(Of ToolStripMenuItemEx)()
            If String.Equals(i.CustomMenuItem.MethodName, "DynamicMenuItem") AndAlso i.CustomMenuItem.Parameters(0).Equals(id) Then

                i.DropDown.SuspendLayout()
                i.DropDownItems.ClearAndDispose

                Select Case id
                    Case DynamicMenuItemID.EncoderProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.VideoEncoderProfiles, AddressOf ShowEncoderProfilesDialog, AddressOf g.LoadVideoEncoder)
                    Case DynamicMenuItemID.MuxerProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.MuxerProfiles, AddressOf ShowMuxerProfilesDialog, AddressOf p.VideoEncoder.LoadMuxer)
                    Case DynamicMenuItemID.Audio1Profiles
                        g.PopulateProfileMenu(i.DropDownItems, s.AudioProfiles, Sub() ShowAudioProfilesDialog(0), AddressOf g.LoadAudioProfile0)
                    Case DynamicMenuItemID.Audio2Profiles
                        g.PopulateProfileMenu(i.DropDownItems, s.AudioProfiles, Sub() ShowAudioProfilesDialog(1), AddressOf g.LoadAudioProfile1)
                    Case DynamicMenuItemID.FilterSetupProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.FilterSetupProfiles, AddressOf ShowFilterSetupProfilesDialog, AddressOf LoadFilterSetup)
                End Select

                i.DropDown.ResumeLayout(False)
                Exit For
            End If
        Next
    End Sub

    <Command("Shows the crop dialog to crop borders.")>
    Sub ShowCropDialog()
        If p.SourceFile.NullOrEmptyS Then
            ShowOpenSourceDialog()
        Else
            If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
                Exit Sub
            End If

            If Not g.EnableFilter("Crop") Then
                If p.Script.Engine = ScriptEngine.AviSynth Then
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)"))
                Else
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "clip = core.std.Crop(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)"))
                End If
            End If

            Using form As New CropForm
                form.ShowDialog()
            End Using

            DisableCropFilter()
            Assistant()
        End If
    End Sub

    <Command("Dialog to preview or cut the video.")>
    Sub ShowPreview()
        If p.SourceFile.NullOrEmptyS Then
            ShowOpenSourceDialog()
        Else
            If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
                Exit Sub
            End If

            Dim script = p.Script.GetNewScript
            script.Path = (p.TempDir + p.TargetFile.Base + "_view." + script.FileType).ToShortFilePath
            script.RemoveFilter("Cutting")

            If script.GetError.NotNullOrEmptyS Then
                MsgError("Script Error", script.GetError)
                Exit Sub
            End If

            Dim form As New PreviewForm(script)
            form.Owner = g.MainForm
            form.Show()
        End If
    End Sub

    <Command("Dialog to configure the main menu.")>
    Sub ShowMainMenuEditor()
        s.CustomMenuMainForm = CustomMainMenu.Edit()
        UpdateTemplatesMenuAsync()
        UpdateScriptsMenuAsync()
        UpdateRecentProjectsMenu()
        UpdateDynamicMenuAsync()
        g.SetRenderer(MenuStrip)
        Refresh()
        g.SaveSettings()
    End Sub

    <Command("Dialog to manage batch jobs.")>
    Sub ShowJobsDialog()
        JobsForm.ShowForm()
    End Sub

    <Command("Clears the job list.")>
    Sub ClearJobs()
        FileHelp.Delete(Folder.Settings + "Jobs.dat")
    End Sub

    <Command("Loads a audio or video profile.")>
    Sub LoadProfile(<DispName("Video")> videoProfile As String,
                    <DispName("Audio 1")> audioProfile1 As String,
                    <DispName("Audio 2")> audioProfile2 As String)

        If videoProfile.NotNullOrEmptyS Then
            For Each i In s.VideoEncoderProfiles
                If String.Equals(i.Name, videoProfile) Then
                    g.LoadVideoEncoder(i)
                End If
            Next
        End If

        If audioProfile1.NotNullOrEmptyS Then
            For Each i In s.AudioProfiles
                If String.Equals(i.Name, audioProfile1) Then
                    g.LoadAudioProfile0(i)
                End If
            Next
        End If

        If audioProfile2.NotNullOrEmptyS Then
            For Each i In s.AudioProfiles
                If String.Equals(i.Name, audioProfile2) Then
                    g.LoadAudioProfile1(i)
                End If
            Next
        End If
    End Sub

    Sub AddJob(
        Optional showJobsDialog As Boolean = True,
        Optional position As Integer = -1)

        If Not CanIgnoreTip Then
            MsgWarn("The current assistant warning cannot be skipped.")
            Exit Sub
        End If

        If Not p.SkippedAssistantTips.Contains(CurrentAssistantTipKey) Then
            p.SkippedAssistantTips.Add(CurrentAssistantTipKey)
        End If

        If Not g.VerifyRequirements() Then
            Exit Sub
        End If

        If AssistantPassed Then
            If AbortDueToLowDiskSpace() Then
                Exit Sub
            End If

            If Not TypeOf p.VideoEncoder Is NullEncoder AndAlso File.Exists(p.VideoEncoder.OutputPath) Then
                Select Case p.FileExistVideo
                    Case FileExistMode.Ask
                        Using td As New TaskDialog(Of String)
                            td.MainInstruction = "A video encoding output file already exists"
                            td.Content = "Would you like to skip video encoding and reuse the existing video encoder output file or would you like to re-encode and overwrite it?"
                            td.AddCommand("Reuse", "skip")
                            td.AddCommand("Re-encode", "encode")

                            Select Case td.Show
                                Case "skip"
                                    p.SkipVideoEncoding = True
                                Case "encode"
                                    p.SkipVideoEncoding = False
                                Case Else
                                    Exit Sub
                            End Select
                        End Using
                    Case FileExistMode.Overwrite
                        p.SkipVideoEncoding = False
                    Case FileExistMode.Skip
                        p.SkipVideoEncoding = True
                End Select
            End If

            If (p.Audio0.File.NotNullOrEmptyS AndAlso (TypeOf p.Audio0 Is GUIAudioProfile OrElse
                TypeOf p.Audio0 Is BatchAudioProfile) AndAlso File.Exists(p.Audio0.GetOutputFile)) OrElse
                (p.Audio1.File.NotNullOrEmptyS AndAlso (TypeOf p.Audio1 Is GUIAudioProfile OrElse
                TypeOf p.Audio1 Is BatchAudioProfile) AndAlso File.Exists(p.Audio1.GetOutputFile)) Then

                Select Case p.FileExistAudio
                    Case FileExistMode.Ask
                        Using td As New TaskDialog(Of String)
                            td.MainInstruction = "An audio encoding output file already exists"
                            td.Content = "Would you like to skip audio encoding and reuse existing audio encoding output files or would you like to re-encode and overwrite?"
                            td.AddCommand("Reuse", "skip")
                            td.AddCommand("Re-encode", "encode")

                            Select Case td.Show
                                Case "skip"
                                    p.SkipAudioEncoding = True
                                Case "encode"
                                    p.SkipAudioEncoding = False
                                Case Else
                                    Exit Sub
                            End Select
                        End Using
                    Case FileExistMode.Overwrite
                        p.SkipAudioEncoding = False
                    Case FileExistMode.Skip
                        p.SkipAudioEncoding = True
                End Select
            End If

            AddJob(False, Nothing, position)

            If showJobsDialog Then
                Me.ShowJobsDialog()
            Else
                bnNext.ShowBold()
            End If
        Else
            Assistant()
        End If
    End Sub

    <Command("Adds a job to the job list.")>
    Sub AddJob(
        <DispName("Show Confirmation")>
        showConfirmation As Boolean,
        <DispName("Template Name"),
        Description("Name of the template to be loaded after the job was added. Empty to load no template.")>
        templateName As String,
        <DispName("Position to insert new job")>
        Optional position As Integer = -1)

        AddJob(showConfirmation, templateName, True, position)
    End Sub

    Sub AddJob(
        showConfirmation As Boolean,
        templateName As String,
        showAssistant As Boolean,
        Optional position As Integer = -1)

        If Not g.VerifyRequirements() Then
            Exit Sub
        End If

        If showAssistant AndAlso Not IsLoading AndAlso Not AssistantPassed Then
            MsgWarn("Please follow the assistant.")
            Exit Sub
        End If

        Dim jobPath = JobManager.GetJobPath()
        SaveProjectPath(jobPath)
        JobManager.AddJob(jobPath, jobPath, position)

        If showConfirmation Then
            MsgInfo("Job added")
        End If

        If templateName.NotNullOrEmptyS Then
            LoadProject(Folder.Template & templateName & ".srip")
        End If
    End Sub

    <Command("Shows a dialog to compare different videos.")>
    Sub ShowVideoComparison()
        Dim form As New VideoComparisonForm
        form.Show()
    End Sub

    <Command("Audio Converter.")>
    Sub ShowAudioConverter()
        Try
            AudioConverterForm.AudioConverterMode = True
            Using aform As New AudioConverterForm
                'Me.Hide()
                aform.ShowDialog()
            End Using
        Catch ex As Exception
            AudioConverterForm.AudioConverterMode = False
            If Not Me.Visible Then Me.Show()
            Log.Write("AudioConverter Form Exception:", ex.ToString & BR & ex.Message?.ToString & BR & ex.InnerException?.ToString & BR & ex.GetBaseException?.ToString)
            Log.Save()
            MediaInfo.ClearCache()
            g.ShowException(ex, timeout:=90)
        Finally
            AudioConverterForm.AudioConverterMode = False
            If Not Me.IsDisposed Then
                Task.Run(Sub()
                             If Not Log.IsEmpty Then Log.Save()
                             If MediaInfo.Cache.Count > 5350 Then BeginInvoke(Sub()
                                                                                  MediaInfo.ClearCache() ' Ask User to Remove???
                                                                                  Application.DoEvents()
                                                                                  GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
                                                                                  GC.Collect(2, GCCollectionMode.Forced, True, True)
                                                                                  GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
                                                                              End Sub)
                         End Sub)
                If Not Me.Visible Then Me.Show()
                Application.DoEvents()
                'Else
                'Process.GetCurrentProcess.Kill()
            End If
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
            GC.Collect(2, GCCollectionMode.Forced, True, True)
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        End Try
    End Sub

    <Command("Dialog to edit filters.")>
    Sub ShowFiltersEditor()
        FiltersListView.ShowEditor()
    End Sub

    <Command("Dialog to configure project options.")>
    Sub ShowOptionsDialog()
        ShowOptionsDialog(Nothing)
    End Sub

    Sub ShowOptionsDialog(pagePath As String)
        Using form As New SimpleSettingsForm(
            "Project Options",
            "In order to save project options go to:",
            "File > Save Project As Template",
            "In order to select a template to be loaded on program startup go to:",
            "Tools > Settings > General > Templates > Default Template")

            form.ScaleClientSize(32, 22, form.FontHeight) 'was:30,21

            Dim ui = form.SimpleUI
            ui.Store = p

            Dim page = ui.CreateFlowPage("Image", True)

            Dim n = ui.AddNum(page)
            n.Text = "Auto resize image size"
            n.Help = "Resizes to a given pixel size after loading a source file."
            n.Config = {0, Integer.MaxValue, 10000}
            n.Field = NameOf(p.AutoResizeImage)

            ui.AddLabel(n, "(0 = disabled)")

            n = ui.AddNum(page)
            n.Text = "Resize slider width"
            n.Config = {0, Integer.MaxValue, 64}
            n.Field = NameOf(p.ResizeSliderMaxWidth)

            ui.AddLabel(n, "(0 = auto)")

            Dim m = ui.AddMenu(Of Integer)(page)
            m.Text = "Output Mod"
            m.Add(2, 4, 8, 16)
            m.Field = NameOf(p.ForcedOutputMod)

            page = ui.CreateFlowPage("Image | Aspect Ratio", True)

            Dim b = ui.AddBool(page)
            b.Text = "Use ITU-R BT.601 compliant aspect ratio"
            b.Help = "Calculates the aspect ratio according to ITU-R BT.601 standard. "
            b.Field = NameOf(p.ITU)

            b = ui.AddBool(page)
            b.Text = "Adjust height according to target display aspect ratio"
            b.Help = "Adjusts the height to match the target display aspect ratio in case the auto resize option is disabled."
            b.Field = NameOf(p.AdjustHeight)

            n = ui.AddNum(page)
            n.Text = "Max AR Error"
            n.Help = "Maximum aspect ratio error. In case of a higher value the AR signaled to the encoder or muxer."
            n.Config = {1, 10, 0.1, 1}
            n.Field = NameOf(p.MaxAspectRatioError)

            Dim t = ui.AddText(page)
            t.Text = "Source DAR"
            t.Help = "Custom source display aspect ratio."
            t.Field = NameOf(p.CustomSourceDAR)

            t = ui.AddText(page)
            t.Text = "Source PAR"
            t.Help = "Custom source pixel aspect ratio."
            t.Field = NameOf(p.CustomSourcePAR)

            t = ui.AddText(page)
            t.Text = "Target DAR"
            t.Help = "Custom target display aspect ratio."
            t.Field = NameOf(p.CustomTargetDAR)

            t = ui.AddText(page)
            t.Text = "Target PAR"
            t.Help = "Custom target pixel aspect ratio."
            t.Field = NameOf(p.CustomTargetPAR)

            Dim cropPage = ui.CreateFlowPage("Image | Crop", True)

            b = ui.AddBool(cropPage)
            b.Text = "Auto correct crop values"
            b.Help = "Force crop values compatible with YUV/YV12 colorspace and with the forced output mod value. "
            b.Field = NameOf(p.AutoCorrectCropValues)

            b = ui.AddBool(cropPage)
            b.Text = "Auto crop borders until proper aspect ratio is found"
            b.Help = "Automatically crops borders until the proper aspect ratio is found."
            b.Field = NameOf(p.AutoSmartCrop)

            n = ui.AddNum(cropPage)
            n.Text = "Auto overcrop width to limit aspect ratio to"
            n.Help = "On small devices it can help to restrict the aspect ratio and overcrop the width instead."
            n.Config = {0, 2, 0.1, 3}
            n.Field = NameOf(p.AutoSmartOvercrop)

            ui.AddLine(cropPage, "Crop Values")

            Dim eb = ui.AddEmptyBlock(cropPage)

            ui.AddLabel(eb, "Left:", 2)

            Dim te = ui.AddEdit(eb)
            te.Text = p.CropLeft.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value)
                                If value.IsInt Then
                                    p.CropLeft = CInt(value)
                                End If
                            End Sub

            Dim l = ui.AddLabel(eb, "Right:", 4)

            te = ui.AddEdit(eb)
            te.Text = p.CropRight.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value)
                                If value.IsInt Then
                                    p.CropRight = CInt(value)
                                End If
                            End Sub

            eb = ui.AddEmptyBlock(cropPage)

            ui.AddLabel(eb, "Top:", 2)

            te = ui.AddEdit(eb)
            te.Text = p.CropTop.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value)
                                If value.IsInt Then
                                    p.CropTop = CInt(value)
                                End If
                            End Sub

            l = ui.AddLabel(eb, "Bottom:", 4)

            te = ui.AddEdit(eb)
            te.Text = p.CropBottom.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value)
                                If value.IsInt Then
                                    p.CropBottom = CInt(value)
                                End If
                            End Sub

            Dim audioPage = ui.CreateFlowPage("Audio", True)

            t = ui.AddText(audioPage)
            t.Text = "Preferred Languages"
            Dim lngS As String = BR2 & String.Join(BR, Language.Languages.WhereSelectF(Function(lw) lw.IsCommon, Function(ls) ls.ToString & ": " & ls.CultureInfo.TwoLetterISOLanguageName & ", " & ls.ThreeLetterCode))
            t.Help = "Preferred audio languages using [http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes two or three letter language code] separated by space, comma or semicolon. For all languages just enter 'all'." & lngS
            t.Field = NameOf(p.PreferredAudio)

            Dim cut = ui.AddMenu(Of CuttingMode)(audioPage)
            cut.Text = "Cutting Method"
            cut.Help = "Defines which method to use for cutting."
            cut.Field = NameOf(p.CuttingMode)

            Dim audioDemux = ui.AddMenu(Of DemuxMode)(audioPage)
            audioDemux.Text = "Demux Audio"
            audioDemux.Field = NameOf(p.DemuxAudio)

            Dim audioExist = ui.AddMenu(Of FileExistMode)(audioPage)
            audioExist.Text = "Existing Output"
            audioExist.Help = "What to do in case a audio encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite."
            audioExist.Field = NameOf(p.FileExistAudio)

            b = ui.AddBool(audioPage)
            b.Text = "On load use AviSynth script as audio source"
            b.Help = "Sets the AviSynth script (*.avs) as audio source file when loading a source file."
            b.Field = NameOf(p.UseScriptAsAudioSource)

            Dim videoPage = ui.CreateFlowPage("Video", True)

            Dim thumbOptions = ui.AddMenu(Of Integer)(videoPage)
            Dim videoExist = ui.AddMenu(Of FileExistMode)(videoPage)
            Dim staxRipThumbnailOption = ui.AddBool(videoPage)
            Dim mtnThumbnailOption = ui.AddBool(videoPage)


            thumbOptions.Text = "Thumbnail Choices:"
            thumbOptions.Add("StaxRip Thumbnails", 0)
            thumbOptions.Add("MTN Thumbnails", 1)
            thumbOptions.Button.Value = s.Storage.GetInt("Thumbnail Choices", 1)
            thumbOptions.Button.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Choices", value)
            AddHandler thumbOptions.Button.ValueChangedUser, Sub()
                                                                 staxRipThumbnailOption.Visible = thumbOptions.Button.Value = 0
                                                                 mtnThumbnailOption.Visible = thumbOptions.Button.Value = 1
                                                             End Sub

            staxRipThumbnailOption.Text = "Create Thumbnails"
            staxRipThumbnailOption.Help = "Saves thumbnails to Source Location using the StaxRip Engine"
            staxRipThumbnailOption.Field = NameOf(p.SaveThumbnails)

            mtnThumbnailOption.Text = "Create Thumbnails"
            mtnThumbnailOption.Visible = True
            mtnThumbnailOption.Help = "Saves thumbnails to Source Location using the MTN Engine"
            mtnThumbnailOption.Field = NameOf(p.MTN)

            staxRipThumbnailOption.Visible = thumbOptions.Button.Value = 0
            mtnThumbnailOption.Visible = thumbOptions.Button.Value = 1

            videoExist.Text = "Existing Video Output"
            videoExist.Help = "What to do in case the video encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite. The 'Copy/Mux' video encoder profile is also capable of reusing existing video encoder output.'"
            videoExist.Field = NameOf(p.FileExistVideo)

            b = ui.AddBool(videoPage)
            b.Text = "Import VUI metadata"
            b.Help = "Imports VUI metadata such as HDR from the source file to the video encoder."
            b.Field = NameOf(p.ImportVUIMetadata)

            b = ui.AddBool(videoPage)
            b.Text = "HDR Ingest"
            b.Help = "Adds the Remaining Metadata Required to be Compliant to HDR10 or HLG Standards"
            b.Field = NameOf(p.MKVHDR)

            Dim subPage = ui.CreateFlowPage("Subtitles", True)

            t = ui.AddText(subPage)
            t.Text = "Preferred Languages"
            t.Help = "Subtitles demuxed and loaded automatically using [http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes two or three letter language code] separated by space, comma or semicolon. For all subtitles just enter all." & lngS
            t.Field = NameOf(p.PreferredSubtitles)

            Dim tbm = ui.AddTextMenu(subPage)
            tbm.Text = "Track Name"
            tbm.Help = "Track name used for muxing, may contain macros."
            tbm.Field = NameOf(p.SubtitleName)
            tbm.AddMenu("Language English", "%language_english%")
            tbm.AddMenu("Language Native", "%language_native%")

            Dim subDemux = ui.AddMenu(Of DemuxMode)(subPage)
            subDemux.Text = "Demux Subtitles"
            subDemux.Field = NameOf(p.DemuxSubtitles)

            Dim mb = ui.AddMenu(Of DefaultSubtitleMode)(subPage)
            mb.Text = "Default Subtitle"
            mb.Field = NameOf(p.DefaultSubtitle)

            b = ui.AddBool(subPage)
            b.Text = "Convert Sup (PGS/Blu-ray) to IDX (Sub/VobSub/DVD)"
            b.Help = "Works only with demuxed subtitles."
            b.Field = NameOf(p.ConvertSup2Sub)

            b = ui.AddBool(subPage)
            b.Text = "Add hardcoded subtitle"
            b.Help = "Automatically hardcodes a subtitle." & BR2 & "Supported formats are SRT, ASS and VobSub."
            b.Field = NameOf(p.HarcodedSubtitle)

            Dim pathPage = ui.CreateFlowPage("Paths", True)

            l = ui.AddLabel(pathPage, "Default Target Folder:")
            l.Help = "Leave empty to use the source file folder."

            Dim macroAction = Function() As Object
                                  MacrosForm.ShowDialogForm()
                              End Function

            Dim tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.DefaultTargetFolder
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetFolder = value
            tm.Button.ContextMenuStrip.SuspendLayout()
            tm.AddMenu("Browse Folder...", Function() g.BrowseFolder(p.DefaultTargetFolder))
            tm.AddMenu("Directory of source file", "%source_dir%")
            tm.AddMenu("Parent directory of source file directory", "%source_dir_parent%")
            tm.AddMenu("Macros...", macroAction)
            tm.Button.ContextMenuStrip.ResumeLayout(False)

            l = ui.AddLabel(pathPage, "Default Target Name:")
            l.Help = "Leave empty to use the source filename"
            l.MarginTop = FontHeight

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.DefaultTargetName
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetName = value
            tm.AddMenu("Name of source file without extension", "%source_name%")
            tm.AddMenu("Name of source file directory", "%source_dir_name%")
            tm.AddMenu("Macros...", macroAction)

            l = ui.AddLabel(pathPage, "Temp Files Folder:")
            l.Help = "Leave empty to use the source file folder."
            l.MarginTop = FontHeight

            Dim tempDirFunc = Function()
                                  Dim tempDir = g.BrowseFolder(p.TempDir)

                                  If tempDir.NotNullOrEmptyS Then
                                      Return tempDir.FixDir & "%source_name%_temp"
                                  End If
                              End Function

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.TempDir
            tm.Edit.SaveAction = Sub(value) p.TempDir = value
            tm.AddMenu("Browse Folder...", tempDirFunc)
            tm.AddMenu("Source File Directory", "%source_dir%%source_name%_temp")
            tm.AddMenu("Macros...", macroAction)

            page = ui.CreateFlowPage("Assistant", True)

            b = ui.AddBool(page)
            b.Text = "Remind to crop"
            b.Field = NameOf(p.RemindToCrop)

            b = ui.AddBool(page)
            b.Text = "Remind to cut"
            b.Field = NameOf(p.RemindToCut)

            b = ui.AddBool(page)
            b.Text = "Remind to do compressibility check"
            b.Field = NameOf(p.RemindToDoCompCheck)

            b = ui.AddBool(page)
            b.Text = "Remind to set filters"
            b.Field = NameOf(p.RemindToSetFilters)

            b = ui.AddBool(page)
            b.Text = "Remind about aspect ratio error"
            b.Field = NameOf(p.RemindArError)

            Dim filtersPage = ui.CreateFlowPage("Filters", True)

            l = ui.AddLabel(filtersPage, "Code appended to trim functions:")
            l.Help = "Code appended to trim functions StaxRip generates using the cut feature."
            l.MarginTop = FontHeight \ 2

            t = ui.AddText(filtersPage)
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.TextBox.Multiline = True
            t.Edit.UseMacroEditor = True
            t.Edit.Text = p.TrimCode
            t.Edit.SaveAction = Sub(value) p.TrimCode = value

            l = ui.AddLabel(filtersPage, "Code inserted at top of scripts:")
            l.Help = "Code inserted at the top of every script StaxRip generates."
            l.MarginTop = FontHeight \ 2

            t = ui.AddText(filtersPage)
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.TextBox.Multiline = True
            t.Edit.UseMacroEditor = True
            t.Edit.Text = p.CodeAtTop
            t.Edit.SaveAction = Sub(value) p.CodeAtTop = value

            Dim miscPage = ui.CreateFlowPage("Misc")
            miscPage.SuspendLayout()

            b = ui.AddBool(miscPage)
            b.Text = "Hide dialogs asking to demux, source filter etc."
            b.Checked = p.NoDialogs
            b.SaveAction = Sub(value) p.NoDialogs = value

            b = ui.AddBool(miscPage)
            b.Text = "Extract timestamps from VFR MKV files [use vsync 0 VFR]"
            b.Checked = p.ExtractTimestamps
            b.SaveAction = Sub(value) p.ExtractTimestamps = value

            ui.AddLine(miscPage, "Compressibility Check")

            b = ui.AddBool(miscPage)
            b.Text = "Auto run compressibility check"
            b.Help = "Performs a compressibility check after loading a source file."
            b.Checked = p.AutoCompCheck
            b.SaveAction = Sub(value) p.AutoCompCheck = value

            n = ui.AddNum(miscPage)
            n.Label.Text = "Percentage for comp. check"
            n.NumEdit.Config = {2, 20}
            n.NumEdit.Value = p.CompCheckRange
            n.NumEdit.SaveAction = Sub(value) p.CompCheckRange = CInt(value)

            Dim compCheckButton = ui.AddMenu(Of CompCheckAction)(miscPage)
            compCheckButton.Label.Text = "After comp. check adjust"
            compCheckButton.Button.Value = p.CompCheckAction
            compCheckButton.Button.SaveAction = Sub(value) p.CompCheckAction = value

            miscPage.ResumeLayout()

            If pagePath.NotNullOrEmptyS Then
                ui.ShowPage(pagePath)
            Else
                ui.SelectLast("last options page")
            End If

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()

                If p.CompCheckRange < 2 OrElse p.CompCheckRange > 20 Then
                    p.CompCheckRange = 5
                End If

                If p.TempDir.NotNullOrEmptyS Then
                    p.TempDir = p.TempDir.FixDir
                End If

                UpdateSizeOrBitrate()
                tbBitrate_TextChanged()
                SetSlider()
                Assistant()
            End If

            ui.SaveLast("last options page")
        End Using
    End Sub

    Sub DisableCropFilter()
        Dim f = p.Script.GetFilter("Crop")

        If f IsNot Nothing AndAlso CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) = 0 Then
            f.Active = False
            FiltersListView.Load()
        End If
    End Sub

    <Command("Dialog to configure filter setup profiles.")>
    Sub ShowFilterSetupProfilesDialog()
        Using form As New ProfilesForm("Filter Setup Profiles", s.FilterSetupProfiles, AddressOf LoadFilterSetup, AddressOf GetScriptAsProfile, AddressOf VideoScript.GetDefaults)

            If form.ShowDialog() = DialogResult.OK Then
                PopulateProfileMenu(DynamicMenuItemID.FilterSetupProfiles)
                FiltersListView.RebuildMenu()
            End If
        End Using
    End Sub

    Function GetFilterProfilesText(categories As List(Of FilterCategory)) As String
        Dim ret = ""
        Dim wasMultiline As Boolean

        For Each i In categories
            ret &= "[" & i.Name & "]" & BR

            For Each filter In i.Filters
                If filter.Script.Contains(BR) Then
                    Dim lines = filter.Script.Split({BR}, StringSplitOptions.RemoveEmptyEntries)

                    For x = 0 To lines.Length - 1
                        lines(x) = "    " & lines(x)
                    Next

                    ret &= BR & filter.Path & " =" & BR & String.Join(BR, lines) & BR
                    wasMultiline = True
                Else
                    If wasMultiline Then
                        ret &= BR
                    End If

                    ret &= filter.Path & " = " & filter.Script & BR
                    wasMultiline = False
                End If
            Next

            If Not ret.EndsWith(BR2, StringComparison.Ordinal) Then
                ret &= BR
            End If
        Next

        Return ret
    End Function

    <Command("Dialog to configure AviSynth filter profiles.")>
    Sub ShowFilterProfilesDialog()
        Dim filterProfiles = If(p.Script.Engine = ScriptEngine.AviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
        Dim getDefaults = If(p.Script.Engine = ScriptEngine.AviSynth, Function() FilterCategory.GetAviSynthDefaults, Function() FilterCategory.GetVapourSynthDefaults)

        Using dialog As New MacroEditorDialog
            dialog.SetScriptDefaults()
            dialog.Text = "Filter Profiles"
            dialog.MacroEditorControl.Value = GetFilterProfilesText(filterProfiles)
            dialog.bnContext.Text = " Restore Defaults... "
            dialog.bnContext.Visible = True
            dialog.MacroEditorControl.rtbDefaults.Text = GetFilterProfilesText(getDefaults())
            dialog.bnContext.AddClickAction(Sub()
                                                If MsgOK("Restore defaults?") Then
                                                    dialog.MacroEditorControl.Value = GetFilterProfilesText(getDefaults())
                                                End If
                                            End Sub)

            If dialog.ShowDialog(Me) = DialogResult.OK Then
                filterProfiles.Clear()
                Dim cat As FilterCategory
                Dim filter As VideoFilter

                For Each line In dialog.MacroEditorControl.Value.Split({BR}, StringSplitOptions.RemoveEmptyEntries)
                    'VB6.ChrW(9)

                    If line(0) = "["c AndAlso line(line.Length - 1) = "]"c Then
                        cat = New FilterCategory(line.Substring(1, line.Length - 2).Trim)
                        filterProfiles.Add(cat)
                    End If

                    Dim l0Tab As Boolean = line(0) = CChar(VB6.vbTab)
                    Dim l0ST As Boolean = line.StartsWith("    ", StringComparison.Ordinal)
                    If l0Tab OrElse l0ST Then 'MultiLine
                        If filter IsNot Nothing Then
                            If filter.Script.NullOrEmptyS Then
                                If l0Tab Then
                                    filter.Script &= line.Substring(1)
                                End If

                                If l0ST Then
                                    filter.Script &= line.Substring(4)
                                End If
                            Else
                                If l0Tab Then
                                    filter.Script &= BR & line.Substring(1)
                                End If

                                If l0ST Then
                                    filter.Script &= BR & line.Substring(4)
                                End If
                            End If
                        End If
                    Else
                        Dim filterName = line.Left("=").Trim

                        If filterName.NotNullOrEmptyS Then
                            filter = New VideoFilter(cat.Name, filterName, line.Right("=").Trim)
                            cat.Filters.Add(filter)
                        End If
                    End If
                Next line

                For Each i In getDefaults()
                    Dim found As Boolean

                    For Each i2 In filterProfiles
                        If i.Name.EqualsExS(i2.Name) Then
                            found = True
                            Exit For
                        End If
                    Next i2

                    If Not found AndAlso {"Source", "Crop", "Resize"}.ContainsString(i.Name) Then
                        MsgWarn("The category '" & i.Name & "' was recreated. A Source, Crop and Resize category is mandatory.")
                        filterProfiles.Add(i)
                    End If
                Next i

                g.SaveSettings()
                FiltersListView.RebuildMenu()
            End If
        End Using
    End Sub

    Shared Function GetDefaultMainMenu() As CustomMenuItem
        Dim ret As New CustomMenuItem("Root")

        ret.Add("File|Open Video File...", NameOf(ShowOpenSourceDialog), Keys.O Or Keys.Control)
        ret.Add("File|-")
        ret.Add("File|Open Project...", NameOf(ShowFileBrowserToOpenProject))
        ret.Add("File|Save Project", NameOf(SaveProject), Keys.S Or Keys.Control, Symbol.Save)
        ret.Add("File|Save Project As...", NameOf(SaveProjectAs))
        ret.Add("File|Save Project As Template...", NameOf(SaveProjectAsTemplate))
        ret.Add("File|-")
        ret.Add("File|Project Templates", NameOf(DynamicMenuItem), {DynamicMenuItemID.TemplateProjects})
        ret.Add("File|Recent Projects", NameOf(DynamicMenuItem), {DynamicMenuItemID.RecentProjects})

        ret.Add("Crop", NameOf(ShowCropDialog), Keys.F4)
        ret.Add("Preview", NameOf(ShowPreview), Keys.F5)

        ret.Add("Options", NameOf(ShowOptionsDialog), Keys.F8)

        ret.Add("Tools|Jobs...", NameOf(ShowJobsDialog), Keys.F6, Symbol.MultiSelectLegacy)

        ret.Add("Tools|Audio Converter...", NameOf(ShowAudioConverter), Keys.F3, Symbol.MusicInfo)

        ret.Add("Tools|Log File", NameOf(g.DefaultCommands.ShowLogFile), Keys.F7, Symbol.Page)
        ret.Add("Tools|Folders", Symbol.Folder)
        ret.Add("Tools|Folders|Log Files", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%Log Files"""})
        ret.Add("Tools|Folders|Plugins", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%plugin_dir%"""})
        ret.Add("Tools|Folders|Programs", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%programs_dir%"""})
        ret.Add("Tools|Folders|Scripts", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%script_dir%"""})
        ret.Add("Tools|Folders|Settings", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%"""})
        ret.Add("Tools|Folders|Source", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%source_dir%"""})
        ret.Add("Tools|Folders|Startup", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%startup_dir%"""})
        ret.Add("Tools|Folders|System", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%system_dir%"""})
        ret.Add("Tools|Folders|Target", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%target_dir%"""})
        ret.Add("Tools|Folders|Templates", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%Templates"""})
        ret.Add("Tools|Folders|Temp", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%temp_dir%"""})
        ret.Add("Tools|Folders|Working", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%working_dir%"""})

        ret.Add("Tools|Scripts", NameOf(DynamicMenuItem), Symbol.Code, {DynamicMenuItemID.Scripts})

        ret.Add("Tools|Advanced", Symbol.More)

        If g.IsDevelopmentPC Then
            ret.Add("Tools|Advanced|Test and create files...", NameOf(g.DefaultCommands.TestAndDynamicFileCreation), Keys.F12)
        End If

        ret.Add("Tools|Advanced|Video Comparison...", NameOf(ShowVideoComparison))
        ret.Add("Tools|Advanced|Demux...", NameOf(g.DefaultCommands.ShowDemuxTool))
        ret.Add("Tools|Advanced|Add Hardcoded Subtitle...", NameOf(ShowHardcodedSubtitleDialog), Keys.Control Or Keys.H)
        ret.Add("Tools|Advanced|Event Command...", NameOf(ShowEventCommandsDialog), Symbol.LightningBolt)
        ret.Add("Tools|Advanced|Reset Setting...", NameOf(g.DefaultCommands.ResetSettings))
        ret.Add("Tools|Advanced|Command Prompt", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.fa_terminal, {"cmd.exe", False, False, False, Folder.Desktop})

        If g.IsWindowsTerminalAvailable Then
            ret.Add("Tools|Advanced|Windows Terminal", NameOf(g.DefaultCommands.ExecuteCommandLine), Keys.Control Or Keys.T, Symbol.fa_terminal, {"wt.exe", False, False, False, Folder.Desktop})
        Else
            ret.Add("Tools|Advanced|PowerShell Terminal", NameOf(g.DefaultCommands.ExecuteCommandLine), Keys.Control Or Keys.T, Symbol.fa_terminal, {"powershell.exe -nologo -executionpolicy unrestricted", False, False, False, Folder.Desktop})
        End If

        ret.Add("Tools|Edit Menu...", NameOf(ShowMainMenuEditor))
        ret.Add("Tools|Settings...", NameOf(ShowSettingsDialog), Symbol.Settings, {""})

        ret.Add("Apps|Subtitles|Subtitle Edit", NameOf(g.DefaultCommands.StartTool), {"Subtitle Edit"})
        ret.Add("Apps|Subtitles|BDSup2Sub++", NameOf(g.DefaultCommands.StartTool), {"BDSup2Sub++"})
        ret.Add("Apps|Subtitles|VSRip", NameOf(g.DefaultCommands.StartTool), {"VSRip"})
        ret.Add("Apps|Media Info|mkvinfo", NameOf(g.DefaultCommands.ShowMkvInfo))
        ret.Add("Apps|Media Info|MediaInfo File", NameOf(g.DefaultCommands.ShowMediaInfo))
        ret.Add("Apps|Media Info|MediaInfo Folder", NameOf(g.DefaultCommands.ShowMediaInfoFolderViewDialog))
        ret.Add("Apps|Media Info|Ingest HDR", NameOf(g.DefaultCommands.SaveMKVHDR))
        ret.Add("Apps|Players|mpv.net", NameOf(g.DefaultCommands.StartTool), {"mpv.net"})
        ret.Add("Apps|Players|MPC-BE", NameOf(g.DefaultCommands.StartTool), {"MPC-BE"})
        ret.Add("Apps|Players|MPC-HC", NameOf(g.DefaultCommands.StartTool), {"MPC-HC"})
        ret.Add("Apps|Indexing|D2V Witch", NameOf(g.DefaultCommands.StartTool), {"D2V Witch"})
        'DGIndex Back
        ret.Add("Apps|Indexing|DGIndexNV", NameOf(g.DefaultCommands.StartTool), {"DGIndexNV"})
        ret.Add("Apps|Indexing|DGIndex", NameOf(g.DefaultCommands.StartTool), {"DGIndex"})
        ret.Add("Apps|Thumbnails|MTN Thumbnailer", NameOf(g.DefaultCommands.SaveMTN))
        ret.Add("Apps|Thumbnails|StaxRip Thumbnailer", NameOf(g.DefaultCommands.ShowBatchGenerateThumbnailsDialog))
        ret.Add("Apps|Animation|Animated GIF", NameOf(g.DefaultCommands.SaveGIF))
        ret.Add("Apps|Animation|Animated PNG", NameOf(g.DefaultCommands.SavePNG))
        ret.Add("Apps|Other|MKVToolnix GUI", NameOf(g.DefaultCommands.StartTool), {"MKVToolnix GUI"})
        ret.Add("Apps|Other|chapterEditor", NameOf(g.DefaultCommands.StartTool), {"chapterEditor"})
        ret.Add("Apps|-")
        ret.Add("Apps|Manage...", NameOf(ShowAppsDialog), Keys.F9)

        ret.Add("Help|Documentation", NameOf(g.DefaultCommands.ExecuteCommandLine), Keys.F1, Symbol.Help, {"http://staxrip.readthedocs.io"})
        ret.Add("Help|Website", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Globe, {"https://github.com/staxrip/staxrip"})
        ret.Add("Help|Apps", NameOf(DynamicMenuItem), {DynamicMenuItemID.HelpApplications})
        ret.Add("Help|Check for Updates", NameOf(g.DefaultCommands.CheckForUpdate))
        ret.Add("Help|-")
        ret.Add("Help|Info...", NameOf(g.DefaultCommands.OpenHelpTopic), Symbol.Info, {"info"})

        Return ret
    End Function

    <Command("Shows a dialog to add a hardcoded subtitle.")>
    Sub ShowHardcodedSubtitleDialog()
        Using dialog As New OpenFileDialog
            dialog.Filter = FileTypes.GetFilter(FileTypes.SubtitleExludingContainers)
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog = DialogResult.OK Then
                If dialog.FileName.Ext.Equals("idx") Then
                    Dim subs = Subtitle.Create(dialog.FileName)

                    If subs.Count = 0 Then
                        MsgInfo("No subtitles found.")
                        Exit Sub
                    End If

                    Dim sb As New SelectionBox(Of Subtitle) With {.Title = "Language", .Text = "Please select a subtitle."}

                    For Each i In subs
                        sb.AddItem(i.Language.Name, i)
                    Next

                    If sb.Show = DialogResult.Cancel Then
                        Exit Sub
                    End If

                    Regex.Replace(dialog.FileName.ReadAllText, "langidx: \d+", "langidx: " & sb.SelectedValue.IndexIDX.ToString).WriteFileDefault(dialog.FileName)
                End If

                p.AddHardcodedSubtitleFilter(dialog.FileName, True)
            End If
        End Using
    End Sub

    Sub tbResize_MouseUp(sender As Object, e As MouseEventArgs) Handles tbResize.MouseUp
        p.Script.GetInfo()
        Assistant()
    End Sub

    Sub tbResize_Scroll() Handles tbResize.Scroll
        SkipAssistant = True

        If Not g.EnableFilter("Resize") Then
            If p.Script.Engine = ScriptEngine.AviSynth Then
                p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
            Else
                p.Script.AddFilter(New VideoFilter("Resize", "Bicubic", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)"))
            End If
        End If

        tbTargetWidth.Text = (320 + tbResize.Value * p.ForcedOutputMod).ToInvStr
        SetImageHeight()
        SkipAssistant = False
        Assistant()
    End Sub

    Sub SetImageHeight()
        Try
            Dim modval = p.ForcedOutputMod
            Dim ar As Double

            If ModifierKeys = Keys.Control Then
                Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
                Dim croph = p.SourceHeight - p.CropTop - p.CropBottom

                ar = cropw / croph
            Else
                ar = Calc.GetTargetDAR
            End If

            tbTargetHeight.Text = (CInt(p.TargetWidth / ar / modval) * modval).ToInvStr

            If p.TargetWidth = 1920 AndAlso p.TargetHeight = 1088 Then
                tbTargetHeight.Text = 1080.ToInvStr
            End If
        Catch
        End Try
    End Sub

    Sub SetImageWidth()
        Try
            Dim modval = p.ForcedOutputMod
            tbTargetWidth.Text = (CInt(p.TargetHeight * Calc.GetTargetDAR / modval) * modval).ToInvStr
        Catch
        End Try
    End Sub

    Sub tbBitrate_KeyDown(sender As Object, e As KeyEventArgs) Handles tbBitrate.KeyDown
        If tbBitrate.ReadOnly Then
            Exit Sub
        End If

        If e.KeyData = Keys.Up Then
            e.Handled = True
            tbBitrate.Text = Math.Max(0, Calc.GetPreviousMod(tbBitrate.Text.ToInt, 50)).ToInvStr
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            tbBitrate.Text = Math.Max(0, Calc.GetNextMod(tbBitrate.Text.ToInt, 50)).ToInvStr
        End If
    End Sub

    Sub tbTargetSize_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetSize.KeyDown
        If tbTargetSize.ReadOnly Then
            Exit Sub
        End If

        Dim modValue As Integer

        Select Case p.TargetSeconds
            Case Is > 50 * 60
                modValue = 50
            Case Is > 10 * 60
                modValue = 10
            Case Is > 60
                modValue = 5
            Case Else
                modValue = 1
        End Select

        If e.KeyData = Keys.Up Then
            e.Handled = True
            tbTargetSize.Text = Math.Max(0, Calc.GetPreviousMod(tbTargetSize.Text.ToInt, modValue)).ToInvStr
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            tbTargetSize.Text = Math.Max(0, Calc.GetNextMod(tbTargetSize.Text.ToInt, modValue)).ToInvStr
        End If
    End Sub

    Sub UpdateSizeOrBitrate()
        If p.BitrateIsFixed Then
            tbBitrate_TextChanged()
        Else
            tbSize_TextChanged()
        End If
    End Sub

    Sub tbSize_TextChanged() Handles tbTargetSize.TextChanged
        Try
            If tbTargetSize.Focused Then
                p.BitrateIsFixed = False
            End If

            Dim tsn = tbTargetSize.Text.ToInt(Integer.MaxValue)

            If tsn <> Integer.MaxValue Then
                p.TargetSize = Math.Max(0, tsn)
                BlockSize = True

                If Not BlockBitrate Then
                    tbBitrate.Text = CInt(Calc.GetVideoBitrate).ToInvStr
                End If

                BlockSize = False
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbBitrate_TextChanged() Handles tbBitrate.TextChanged
        Try
            If tbBitrate.Focused Then
                p.BitrateIsFixed = True
            End If

            Dim tbn = tbBitrate.Text.ToInt(Integer.MaxValue)

            If tbn <> Integer.MaxValue Then
                p.VideoBitrate = Math.Max(0, tbn)
                BlockBitrate = True

                If Not BlockSize Then
                    tbTargetSize.Text = CInt(Calc.GetSize).ToInvStr
                End If

                BlockBitrate = False
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbTargetWidth_TextChanged() Handles tbTargetWidth.TextChanged
        Try
            Dim twn = tbTargetWidth.Text.ToInt(Integer.MaxValue)

            If twn <> Integer.MaxValue Then
                p.TargetWidth = twn
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbTargetHeight_TextChanged() Handles tbTargetHeight.TextChanged
        Try
            Dim thn = tbTargetHeight.Text.ToInt(Integer.MaxValue)

            If thn <> Integer.MaxValue Then
                p.TargetHeight = thn
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbTargetWidth_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetWidth.KeyDown
        If e.KeyData = Keys.Up Then
            e.Handled = True
            Dim modVal = p.ForcedOutputMod
            tbTargetWidth.Text = CInt(((p.TargetWidth + modVal) / modVal) * modVal).ToInvStr
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            Dim modVal = p.ForcedOutputMod
            tbTargetWidth.Text = CInt(((p.TargetWidth - modVal) / modVal) * modVal).ToInvStr
        End If
    End Sub

    Sub tbTargetWidth_KeyUp(sender As Object, e As KeyEventArgs) Handles tbTargetWidth.KeyUp
        p.Script.GetInfo()
        Assistant()
    End Sub

    Sub tbTargetHeight_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetHeight.KeyDown
        If e.KeyData = Keys.Up Then
            e.Handled = True
            Dim modVal = p.ForcedOutputMod
            tbTargetHeight.Text = CInt(((p.TargetHeight + modVal) / modVal) * modVal).ToInvStr
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            Dim modVal = p.ForcedOutputMod
            tbTargetHeight.Text = CInt(((p.TargetHeight - modVal) / modVal) * modVal).ToInvStr
        End If
    End Sub

    Sub tbTargetHeight_KeyUp(sender As Object, e As KeyEventArgs) Handles tbTargetHeight.KeyUp
        p.Script.GetInfo()
        Assistant()
    End Sub

    Sub tbTargetWidth_MouseWheel(sender As Object, e As MouseEventArgs) Handles tbTargetWidth.MouseWheel
        If e.Delta < 0 Then
            Dim modVal = p.ForcedOutputMod
            tbTargetWidth.Text = CInt(((p.TargetWidth - modVal) / modVal) * modVal).ToInvStr
        Else
            Dim modVal = p.ForcedOutputMod
            tbTargetWidth.Text = CInt(((p.TargetWidth + modVal) / modVal) * modVal).ToInvStr
        End If
    End Sub

    Sub tbTargetHeight_MouseWheel(sender As Object, e As MouseEventArgs) Handles tbTargetHeight.MouseWheel
        If e.Delta < 0 Then
            Dim modVal = p.ForcedOutputMod
            tbTargetHeight.Text = CInt(((p.TargetHeight - modVal) / modVal) * modVal).ToInvStr
        Else
            Dim modVal = p.ForcedOutputMod
            tbTargetHeight.Text = CInt(((p.TargetHeight + modVal) / modVal) * modVal).ToInvStr
        End If
    End Sub

    Shared Function GetDefaultMenuSize() As CustomMenuItem
        Dim ret = New CustomMenuItem("Root")
        ret.Add("DVD/BD-5 (4480 MB)", NameOf(SetSize), {4480})
        ret.Add("DVD-DL/BD-9 (8145 MB)", NameOf(SetSize), {8145})
        ret.Add("-")
        ret.Add("BD (23450 MB)", NameOf(SetSize), {23450})
        ret.Add("BD-DL (46900 MB)", NameOf(SetSize), {46900})
        ret.Add("-")
        ret.Add("50%", NameOf(SetPercent), {50})
        ret.Add("60%", NameOf(SetPercent), {60})
        ret.Add("-")
        ret.Add("Edit Menu...", NameOf(ShowSizeMenuEditor))
        Return ret
    End Function

    <Command("Sets the target image size.")>
    Sub SetTargetImageSize(width As Integer, height As Integer)
        If Not g.EnableFilter("Resize") Then
            p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
        End If

        If height = 0 AndAlso width > 0 Then
            tbTargetWidth.Text = width.ToInvStr
            SetImageHeight()
        ElseIf width = 0 AndAlso height > 0 Then
            tbTargetHeight.Text = height.ToInvStr
            SetImageWidth()
        Else
            tbTargetWidth.Text = width.ToInvStr
            tbTargetHeight.Text = height.ToInvStr
        End If

        p.Script.GetInfo()
        Assistant()
    End Sub

    <Command("Sets the target image size by pixels (width x height).")>
    Sub SetTargetImageSizeByPixel(pixel As Integer)
        If Not g.EnableFilter("Resize") Then
            p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
        End If

        Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
        Dim ar = Calc.GetTargetDAR()
        Dim w = Calc.FixMod16(CInt(Math.Sqrt(pixel * ar)))

        If w > cropw Then
            w = (cropw \ 16) * 16
        End If

        Dim h = Calc.FixMod16(CInt(w / ar))

        tbTargetWidth.Text = w.ToString()
        tbTargetHeight.Text = h.ToString()

        p.Script.GetInfo()
        Assistant()
    End Sub

    <Command("Sets the target file size in MB.")>
    Sub SetSize(<DispName("Target File Size")> targetSize As Integer)
        tbTargetSize.Text = targetSize.ToString
        p.BitrateIsFixed = False
    End Sub

    <Command("Shows script info using various console tools.")>
    Sub ShowScriptInfo()
        p.Script.Synchronize()
        g.ShowAdvancedScriptInfo(p.Script)
    End Sub

    <Command("Sets the bitrate according to the compressibility.")>
    Sub SetPercent(<DispName("Percent Value")> value As Integer)
        tbTargetSize.Text = g.GetAutoSize(value).ToString
        p.BitrateIsFixed = False
    End Sub

    <Command("Sets the target video bitrate in Kbps.")>
    Sub SetBitrate(<DispName("Target Video Bitrate")> bitrate As Integer)
        tbBitrate.Text = bitrate.ToString
    End Sub

    <Command("Crops borders automatically.")>
    Sub StartAutoCrop()
        g.RunAutoCrop(Nothing)
        Assistant()
    End Sub

    <Command("Crops borders automatically until the proper aspect ratio is found.")>
    Sub StartSmartCrop()
        g.SmartCrop()
        Assistant()
    End Sub

    <Command("Menu editor for the size menu.")>
    Sub ShowSizeMenuEditor()
        s.CustomMenuSize = CustomSizeMenu.Edit()
    End Sub

    Sub UpdateSizeMenu()
        For Each i As ToolStripMenuItemEx In CustomSizeMenu.MenuItems
            If String.Equals(i.CustomMenuItem.MethodName, "SetPercent") Then
                i.Enabled = p.Compressibility > 0
            End If
        Next
    End Sub

    Function GetScriptAsProfile() As Profile
        Dim sb As New SelectionBox(Of TargetVideoScript)

        sb.Title = "New Profile"
        sb.Text = "Please select a profile."

        sb.AddItem("Current Project", p.Script)

        For Each i In VideoScript.GetDefaults
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedValue
        End If
    End Function

    Sub LoadFilterSetup(profileInterface As Profile)
        Dim profile = DirectCast(ObjectHelp.GetCopy(profileInterface), TargetVideoScript)

        If profile.Engine = ScriptEngine.AviSynth OrElse
            (Package.Python.VerifyOK(True) AndAlso Package.VapourSynth.VerifyOK(True) AndAlso Package.vspipe.VerifyOK(True)) Then

            Dim currentSetup = p.Script

            Try
                p.Script = profile
                ModifyFilters()
                FiltersListView.OnChanged()
                Assistant()
            Catch ex As Exception
                p.Script = currentSetup
                ModifyFilters()
                FiltersListView.OnChanged()
                Assistant()
            End Try
        End If

        FiltersListView.RebuildMenu()
    End Sub

    Sub ProcessCommandLine(args As String())
        If args.Length <= 1 Then Exit Sub

        Dim files As New List(Of String)
        Dim argA(args.Length - 2) As String
        For a = 1 To args.Length - 1
            argA(a) = args(a)
        Next a

        For Each arg In argA
            Try
                If Not File.Exists(arg) AndAlso files.Count > 0 Then
                    Dim fa = files.ToArray  'Dim l As New List(Of String)(files)
                    Refresh()
                    OpenAnyFile(fa) 'l
                    files.Clear()
                End If

                If File.Exists(arg) Then
                    files.Add(arg)
                Else
                    If Not CommandManager.ProcessCommandLineArgument(arg) Then
                        Throw New Exception
                    End If
                End If
            Catch ex As Exception
                MsgWarn("Error parsing argument:" & BR2 & arg & BR2 & ex.Message)
            End Try
        Next arg

        If files.Count > 0 Then
            Refresh()
            OpenAnyFile(files.ToArray)
        End If
    End Sub

    <Command("Sets the project option 'Hide dialogs asking to demux, source filter etc.'")>
    Sub SetHideDialogsOption(hide As Boolean)
        p.NoDialogs = hide
    End Sub

    <Command("Standby PC.")>
    Sub Standby()
        g.ShutdownPC(ShutdownMode.Standby)
    End Sub

    <Command("Shut down PC.")>
    Sub Shutdown()
        g.ShutdownPC(ShutdownMode.Shutdown)
    End Sub

    Sub SetEncoderControl(c As Control)
        pnEncoder.Controls.Add(c)

        If pnEncoder.Controls.Count > 1 Then
            Dim old = pnEncoder.Controls(0)
            pnEncoder.Controls.Remove(old)
            old.Dispose()
        End If
    End Sub

    Sub UpdateEncoderStateRelatedControls()
        blFilesize.Visible = Not p.VideoEncoder.QualityMode
        tbTargetSize.Visible = Not p.VideoEncoder.QualityMode
        laBitrate.Visible = Not p.VideoEncoder.QualityMode
        tbBitrate.Visible = Not p.VideoEncoder.QualityMode
        laTarget2.Visible = p.VideoEncoder.IsCompCheckEnabled

        tbTargetSize.ReadOnly = p.VideoEncoder.GetFixedBitrate <> 0
        tbBitrate.ReadOnly = p.VideoEncoder.GetFixedBitrate <> 0
        blFilesize.Enabled = p.VideoEncoder.GetFixedBitrate = 0
    End Sub

    <Command("Dialog to open a single file source.")>
    Sub ShowOpenSourceSingleFileDialog()
        Using dialog As New OpenFileDialog
            dialog.Filter = FileTypes.GetFilter(FileTypes.Video.ConcatA(FileTypes.Image))
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                OpenVideoSourceFiles(dialog.FileNames)
            End If
        End Using
    End Sub

    <Command("Dialog to open a Blu-ray folder source.")>
    Sub ShowOpenSourceBlurayFolderDialog()
        If p.SourceFile.NotNullOrEmptyS Then
            If Not OpenProject() Then
                Exit Sub
            End If
        End If

        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select a Blu-ray source folder."
            dialog.SetSelectedPath(s.Storage.GetString("last blu-ray source folder"))
            dialog.ShowNewFolderButton = False

            If dialog.ShowDialog = DialogResult.OK Then
                s.Storage.SetString("last blu-ray source folder", dialog.SelectedPath.FixDir)
                Dim srcPath = dialog.SelectedPath.FixDir

                If Directory.Exists(srcPath + "BDMV") Then
                    srcPath = srcPath + "BDMV\"
                End If

                If Directory.Exists(srcPath + "PLAYLIST") Then
                    srcPath = srcPath + "PLAYLIST\"
                End If

                If Not srcPath.ToUpper(InvCult).EndsWith("PLAYLIST\", StringComparison.Ordinal) Then
                    MsgWarn("No playlist directory found.")
                    Exit Sub
                End If

                Log.WriteEnvironment()
                Log.Write("Process Blu-Ray folder using eac3to", """" + Package.eac3to.Path + """ """ + srcPath + """" + BR2)
                Log.WriteLine("Source Drive Type: " + New DriveInfo(dialog.SelectedPath).DriveType.ToString + BR)

                Dim output = ProcessHelp.GetConsoleOutput(Package.eac3to.Path, srcPath.Escape).Replace(VB6.vbBack, "")
                Log.WriteLine(output)

                Dim a = Regex.Split(output, "^\d+\)", RegexOptions.Multiline).ToList

                If a(0).NullOrEmptyS Then
                    a.RemoveAt(0)
                End If

                Dim td2 As New TaskDialog(Of Integer) With {.MainInstruction = "Please select a playlist."}

                For Each i In a
                    If i.Contains(BR) Then
                        td2.AddCommand(i.Left(BR).Trim, i.Right(BR).TrimEnd, a.IndexOf(i) + 1)
                    End If
                Next

                If td2.Show() = 0 Then
                    Exit Sub
                End If

                OpenEac3toDemuxForm(srcPath, td2.SelectedValue)
            End If
        End Using
    End Sub

    <Command("Dialog to open a merged files source.")>
    Sub ShowOpenSourceMergeFilesDialog()
        Using form As New SourceFilesForm()
            form.Text = "Merge"
            form.IsMerge = True

            If form.ShowDialog() = DialogResult.OK AndAlso form.lb.Items.Count > 0 Then
                Dim files = form.GetFiles

                Select Case files(0).Ext
                    Case "mpg", "vob"
                        OpenVideoSourceFiles(files)
                    Case Else
                        Using proc As New Proc
                            proc.Header = "Merge source files"

                            For Each i In files
                                Log.WriteLine(MediaInfo.GetSummary(i) & "---------------------------------------------------------" & BR2)
                            Next

                            proc.Encoding = Encoding.UTF8
                            proc.Package = Package.mkvmerge
                            Dim outFile = files(0).DirAndBase & "_merged.mkv"
                            proc.Arguments = "-o " & outFile.Escape & " """ & String.Join(""" & """, files) & """"

                            Try
                                proc.Start()
                            Catch ex As Exception
                                g.ShowException(ex)
                                MsgInfo("Manual Merging", "Please merge the files manually with a appropriate tool or visit the support forum].")
                                Throw New AbortException
                            End Try

                            If Not g.FileExists(outFile) Then
                                Log.Write("Error merged output file is missing", outFile)
                                Exit Sub
                            Else
                                OpenVideoSourceFile(outFile)
                            End If
                        End Using
                End Select
            End If
        End Using
    End Sub

    <Command("Dialog to open a file batch source.")>
    Sub ShowOpenSourceBatchFilesDialog()
        If AbortDueToLowDiskSpace() Then
            Exit Sub
        End If

        Using form As New SourceFilesForm()
            form.Text = "File Batch"

            If p.DefaultTargetName = "%source_dir_name%" Then
                p.DefaultTargetName = "%source_name%"
            End If

            If form.ShowDialog() = DialogResult.OK AndAlso form.lb.Items.Count > 0 Then
                If p.SourceFiles.Count > 0 AndAlso Not LoadTemplateWithSelectionDialog() Then
                    Exit Sub
                End If

                For Each filepath In form.GetFiles
                    AddBatchJob(filepath)
                Next

                ShowJobsDialog()
            End If
        End Using
    End Sub

    <Command("Dialog to open source files.")>
    Sub ShowOpenSourceDialog()
        Dim td As New TaskDialog(Of String)

        td.MainInstruction = "Select a method for opening a source."
        td.AddCommand("Single File", "Single File")
        td.AddCommand("Blu-ray Folder", "Blu-ray Folder")
        td.AddCommand("Merge Files", "Merge Files")
        td.AddCommand("File Batch", "File Batch")

        Select Case td.Show
            Case "Single File"
                ShowOpenSourceSingleFileDialog()
            Case "Merge Files"
                ShowOpenSourceMergeFilesDialog()
            Case "File Batch"
                ShowOpenSourceBatchFilesDialog()
            Case "Blu-ray Folder"
                ShowOpenSourceBlurayFolderDialog()
        End Select
    End Sub

    Sub OpenEac3toDemuxForm(playlistFolder As String, playlistID As Integer)
        Using form As New eac3toForm(p)
            form.PlaylistFolder = playlistFolder
            form.PlaylistID = playlistID

            Dim workDir = playlistFolder.Parent.Parent

            Dim title = InputBox.Show("Enter a short title used as filename.",
                "Title", playlistFolder.Parent.Parent.DirName)

            If title.NullOrEmptyS Then
                Exit Sub
            End If

            If p.TempDir.NotNullOrEmptyS Then
                workDir = p.TempDir.Replace("%source_name%", title)
            Else
                workDir += title
            End If

            If Not g.IsFixedDrive(workDir) Then
                Using dialog As New FolderBrowserDialog
                    dialog.Description = "Please select a folder for temporary files on a fixed local drive."
                    dialog.SetSelectedPath(s.Storage.GetString("last blu-ray target folder").Parent)

                    If dialog.ShowDialog = DialogResult.OK Then
                        workDir = dialog.SelectedPath.FixDir
                    Else
                        Exit Sub
                    End If
                End Using
            End If

            form.teTempDir.Text = workDir

            If form.ShowDialog() = DialogResult.OK Then
                If Not Directory.Exists(form.OutputFolder) Then
                    Try
                        Directory.CreateDirectory(form.OutputFolder)
                    Catch ex As Exception
                        MsgError("The temp folder could not be created." + BR2 + form.OutputFolder)
                        Exit Sub
                    End Try
                End If

                If Not g.IsFixedDrive(form.OutputFolder) Then
                    MsgError("Only fixed local drives are supported as temp dir.")
                    Exit Sub
                End If

                Try
                    Dim di As New DriveInfo(form.OutputFolder)

                    If di.AvailableFreeSpace / 1024 ^ 3 < 50 Then
                        MsgError("The target drive has not enough free disk space.")
                        Exit Sub
                    End If

                    Using pr As New Proc
                        pr.Header = "Demux M2TS"
                        pr.TrimChars = {"-"c, " "c}
                        pr.SkipStrings = {"analyze: ", "process: "}
                        pr.Package = Package.eac3to
                        Dim outFiles As New List(Of String)
                        pr.Process.StartInfo.Arguments = form.GetArgs(playlistFolder.Escape + " " & playlistID & ")", title, outFiles)
                        pr.OutputFiles = outFiles

                        Try
                            pr.Start()
                        Catch ex As AbortException
                            Exit Sub
                        Catch ex As Exception
                            g.ShowException(ex)
                            Exit Sub
                        End Try
                    End Using

                    s.Storage.SetString("last blu-ray target folder", form.OutputFolder)
                    Dim fs = form.OutputFolder + title + "." + form.cbVideoOutput.Text.ToLower(InvCult)

                    If File.Exists(fs) Then
                        p.TempDir = form.OutputFolder
                        OpenVideoSourceFile(fs)
                    End If
                Catch
                End Try
            End If
        End Using
    End Sub

    Function GetSourceFilesFromDir(path As String) As String()
        For Each i In FileTypes.Video
            Dim files = Directory.GetFiles(path, "*." + i)
            Array.Sort(files)

            If files.Length > 0 Then
                Return files
            End If
        Next
    End Function

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case 800 'WM_DWMCOLORIZATIONCOLORCHANGED
                If ToolStripRendererEx.IsAutoRenderMode Then
                    ToolStripRendererEx.InitColors(s.ToolStripRenderModeEx)
                    SetMenuStyle()
                    'MenuStrip.Refresh()
                    MenuStrip.Invalidate(True)
                End If
        End Select

        MyBase.WndProc(m)
    End Sub

    Sub tbSource_DoubleClick() Handles tbSourceFile.DoubleClick
        Using dialog As New OpenFileDialog
            dialog.Filter = FileTypes.GetFilter(FileTypes.Video)
            dialog.Multiselect = True
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                Refresh()

                Dim l As New List(Of String)(dialog.FileNames)
                l.Sort()
                OpenVideoSourceFiles(l)
            End If
        End Using
    End Sub

    Function GetAudioProfile0() As Profile
        Return GetNewAudioProfile(p.Audio0)
    End Function

    Function GetAudioProfile1() As Profile
        Return GetNewAudioProfile(p.Audio1)
    End Function

    Function GetNewAudioProfile(currentProfile As AudioProfile) As AudioProfile
        Dim sb As New SelectionBox(Of AudioProfile)
        sb.Title = "New Profile"
        sb.Text = "Please select a profile."

        If Not currentProfile Is Nothing Then
            sb.AddItem("Current Project", currentProfile)
        End If

        For Each i In AudioProfile.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedValue
        End If
    End Function

    Sub tbAudioFile0_DoubleClick() Handles tbAudioFile0.DoubleClick
        Using dialog As New OpenFileDialog
            Dim filter(FileTypes.Audio.Length) As String
            filter(0) = "avs"
            Array.Copy(FileTypes.Audio, 0, filter, 1, FileTypes.Audio.Length)
            dialog.Filter = FileTypes.GetFilter(filter)
            dialog.SetInitDir(p.TempDir, s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                tbAudioFile0.Text = dialog.FileName
            End If
        End Using
    End Sub

    Sub tbAudioFile1_DoubleClick() Handles tbAudioFile1.DoubleClick
        Using dialog As New OpenFileDialog
            Dim filter(FileTypes.Audio.Length) As String
            filter(0) = "avs"
            Array.Copy(FileTypes.Audio, 0, filter, 1, FileTypes.Audio.Length)
            dialog.Filter = FileTypes.GetFilter(filter)
            dialog.SetInitDir(p.TempDir, s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                tbAudioFile1.Text = dialog.FileName
            End If
        End Using
    End Sub

    Sub lTip_Click() Handles laTip.Click
        If Not AssistantMethod Is Nothing Then
            AssistantMethod.Invoke()
            Assistant()
        End If
    End Sub

    Sub UpdateTargetParameters(seconds As Integer, frameRate As Double)
        p.TargetSeconds = seconds
        p.TargetFrameRate = frameRate
        UpdateSizeOrBitrate()
    End Sub

    Sub pEncoder_MouseLeave() Handles pnEncoder.MouseLeave
        Assistant()
    End Sub

    Sub AudioEdit0ToolStripMenuItemClick()
        p.Audio0.EditProject()
        UpdateAudioMenu()
        UpdateSizeOrBitrate()
        llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
        ShowAudioTip(p.Audio0)
    End Sub

    Sub AudioEdit1ToolStripMenuItemClick()
        p.Audio1.EditProject()
        UpdateAudioMenu()
        UpdateSizeOrBitrate()
        llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)
        ShowAudioTip(p.Audio1)
    End Sub

    Sub ShowAudioTip(ap As AudioProfile)
        If TypeOf ap Is GUIAudioProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)

            If ap.Decoder = AudioDecoderMode.ffmpeg AndAlso gap.GetEncoder = GuiAudioEncoder.ffmpeg Then
                Documentation.ShowTip(
                    "Please note that defining an audio decoder will always result in a separate " +
                    "decoding step, which is usually not necessary if the decoder and encoder are identical, " +
                    "it's a waste of time and disk space.")
            End If
        End If
    End Sub

    Sub AudioSource0ToolStripMenuItemClick()
        tbAudioFile0_DoubleClick()
    End Sub

    Sub AudioSource1ToolStripMenuItemClick()
        tbAudioFile1_DoubleClick()
    End Sub

    <Command("Dialog to manage audio profiles.")>
    Sub ShowAudioProfilesDialog(<DispName("Track Number (0 or 1)")> number As Integer)
        Dim form As ProfilesForm

        If number = 0 Then
            form = New ProfilesForm("Audio Profiles", s.AudioProfiles, AddressOf g.LoadAudioProfile0, AddressOf GetAudioProfile0, AddressOf AudioProfile.GetDefaults)
        Else
            form = New ProfilesForm("Audio Profiles", s.AudioProfiles, AddressOf g.LoadAudioProfile1, AddressOf GetAudioProfile1, AddressOf AudioProfile.GetDefaults)
        End If

        form.ShowDialog()
        form.Dispose()

        UpdateAudioMenu()
        PopulateProfileMenu(DynamicMenuItemID.Audio1Profiles)
        PopulateProfileMenu(DynamicMenuItemID.Audio2Profiles)
    End Sub

    Sub UpdateAudioMenu()
        AudioMenu0.SuspendLayout()
        AudioMenu1.SuspendLayout()
        AudioMenu0.Items.ClearAndDispose
        AudioMenu1.Items.ClearAndDispose
        g.PopulateProfileMenu(AudioMenu0.Items, s.AudioProfiles, Sub() ShowAudioProfilesDialog(0), AddressOf g.LoadAudioProfile0)
        g.PopulateProfileMenu(AudioMenu1.Items, s.AudioProfiles, Sub() ShowAudioProfilesDialog(1), AddressOf g.LoadAudioProfile1)
        AudioMenu0.ResumeLayout()
        AudioMenu1.ResumeLayout()
    End Sub

    Sub AviSynthListView_ScriptChanged() Handles FiltersListView.Changed
        If Not IsLoading AndAlso Not FiltersListView.IsLoading Then

            'DGIndexNV Back
            Package.DGDecodeNV.VerifyOK()

            If g.IsValidSource(False) Then
                UpdateSourceParameters()
                UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
            End If

            Assistant()
        End If
    End Sub

    Sub UpdateFilters()
        FiltersListView.Load()

        If g.IsValidSource(False) Then
            UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
        End If
    End Sub

    Sub AviSynthListView_DoubleClick() Handles FiltersListView.DoubleClick
        FiltersListView.ShowEditor()
    End Sub

    Sub gbFilters_MenuClick() Handles lgbFilters.LinkClick
        FiltersListView.ContextMenuStrip.Show(lgbFilters, 0, 16)
    End Sub

    Sub gbEncoder_LinkClick() Handles lgbEncoder.LinkClick
        EncoderMenu.SuspendLayout()
        EncoderMenu.Items.ClearAndDispose
        g.PopulateProfileMenu(EncoderMenu.Items, s.VideoEncoderProfiles, AddressOf ShowEncoderProfilesDialog, AddressOf g.LoadVideoEncoder)
        EncoderMenu.ResumeLayout()
        EncoderMenu.Show(lgbEncoder, 0, 16)
    End Sub

    Sub llSize_Click() Handles blFilesize.Click
        UpdateSizeMenu()
        SizeContextMenuStrip.Show(blFilesize, 0, 16)
    End Sub

    Sub lAudioProfile0_Click() Handles llAudioProfile0.Click
        AudioMenu0.Show(llAudioProfile0, 0, 16)
    End Sub

    Sub lAudioProfile1_Click() Handles llAudioProfile1.Click
        AudioMenu1.Show(llAudioProfile1, 0, 16)
    End Sub

    Sub llContainer_Click() Handles llMuxer.Click
        ContainerMenu.SuspendLayout()
        ContainerMenu.Items.ClearAndDispose
        g.PopulateProfileMenu(ContainerMenu.Items, s.MuxerProfiles, AddressOf ShowMuxerProfilesDialog, AddressOf p.VideoEncoder.LoadMuxer)
        ContainerMenu.ResumeLayout()
        ContainerMenu.Show(llMuxer, 0, 16)
    End Sub

    Sub gbResize_LinkClick() Handles lgbResize.LinkClick
        Dim cms = TextCustomMenu.GetMenu(s.TargetImageSizeMenu, lgbResize.Label, components, AddressOf TargetImageMenuClick)
        cms.SuspendLayout()
        cms.Items.AddRange({New ToolStripSeparator,
                            New ActionMenuItem("Image Options...", Sub() ShowOptionsDialog("Image")),
                            New ActionMenuItem("Edit Menu...", Sub() s.TargetImageSizeMenu = TextCustomMenu.EditMenu(s.TargetImageSizeMenu, ApplicationSettings.GetDefaultTargetImageSizeMenu, Me))})
        cms.ResumeLayout()
        cms.Show(lgbResize, 0, lgbResize.Label.Height)
    End Sub

    Sub blSourceParText_Click(sender As Object, e As EventArgs) Handles blSourceParText.Click
        Dim menu = TextCustomMenu.GetMenu(s.ParMenu, blSourceParText, components, AddressOf SourceParMenuClick)
        menu.Items.AddRange({New ToolStripSeparator,
                             New ActionMenuItem("Edit Menu...", Sub() s.ParMenu = TextCustomMenu.EditMenu(s.ParMenu, ApplicationSettings.GetParMenu, Me))})
        menu.Show(blSourceParText, 0, blSourceParText.Height)
    End Sub

    Sub blSourceDarText_Click(sender As Object, e As EventArgs) Handles blSourceDarText.Click
        Dim menu = TextCustomMenu.GetMenu(s.DarMenu, blSourceDarText, components, AddressOf SourceDarMenuClick)
        menu.Items.AddRange({New ToolStripSeparator,
                             New ActionMenuItem("Edit Menu...", Sub() s.DarMenu = TextCustomMenu.EditMenu(s.DarMenu, ApplicationSettings.GetDarMenu, Me))})
        menu.Show(blSourceDarText, 0, blSourceDarText.Height)
    End Sub

    Sub blTargetDarText_Click(sender As Object, e As EventArgs) Handles blTargetDarText.Click
        Dim menu = TextCustomMenu.GetMenu(s.DarMenu, blTargetDarText, components, AddressOf TargetDarMenuClick)
        menu.Items.AddRange({New ToolStripSeparator,
                             New ActionMenuItem("Edit Menu...", Sub() s.DarMenu = TextCustomMenu.EditMenu(s.DarMenu, ApplicationSettings.GetDarMenu, Me))})
        menu.Show(blTargetDarText, 0, blTargetDarText.Height)
    End Sub

    Sub blTargetParText_Click(sender As Object, e As EventArgs) Handles blTargetParText.Click
        Dim menu = TextCustomMenu.GetMenu(s.ParMenu, blTargetParText, components, AddressOf TargetParMenuClick)
        menu.Items.AddRange({New ToolStripSeparator,
                             New ActionMenuItem("Edit Menu...", Sub() s.ParMenu = TextCustomMenu.EditMenu(s.ParMenu, ApplicationSettings.GetParMenu, Me))})
        menu.Show(blTargetParText, 0, blTargetParText.Height)
    End Sub

    Sub SourceDarMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        p.CustomSourcePAR = ""
        p.CustomSourceDAR = tup.Value

        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then
            p.CustomSourceDAR = ""
        End If

        Assistant()
    End Sub

    Sub SourceParMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        p.CustomSourcePAR = tup.Value
        p.CustomSourceDAR = ""
        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then
            p.CustomSourcePAR = ""
        End If

        Assistant()
    End Sub

    Sub TargetDarMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        p.CustomTargetPAR = ""
        p.CustomTargetDAR = tup.Value

        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then
            p.CustomTargetDAR = ""
        End If

        Assistant()
    End Sub

    Sub TargetParMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        p.CustomTargetPAR = tup.Value
        p.CustomTargetDAR = ""

        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then
            p.CustomTargetPAR = ""
        End If

        Assistant()
    End Sub

    Sub TargetImageMenuClick(value As String)
        g.EnableFilter("Resize")

        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        value = tup.Value

        Dim vi = value.ToIntM
        If vi <> -2147483646I Then
            SetTargetImageSizeByPixel(vi)
            Exit Sub
        End If

        If value?.IndexOf("x"c) >= 0 Then
            Dim a = value.SplitNoEmptyAndWhiteSpace("x"c)

            If a.Length = 2 Then
                Dim a0i = a(0).ToIntM
                If a0i <> -2147483646I Then
                    Dim a1i = a(1).ToIntM
                    If a1i <> -2147483646I Then
                        SetTargetImageSize(a0i, a1i)
                        Exit Sub
                    End If
                End If
            End If
        End If

        MsgWarn("Invalid format")
    End Sub

    Sub SetAutoAspectRatio(isAnamorphic As Boolean)
        p.SourceAnamorphic = isAnamorphic
        p.CustomSourcePAR = ""
        p.CustomSourceDAR = ""

        If p.Script.IsFilterActive("Resize)") Then
            SetTargetImageSize(p.TargetWidth, 0)
        End If

        Assistant()
    End Sub

    Sub UpdateSourceParameters()
        If p.SourceScript IsNot Nothing Then
            Try
                p.SourceWidth = p.SourceScript.GetInfo.Width
                p.SourceHeight = p.SourceScript.GetInfo.Height
                p.SourceSeconds = CInt(p.SourceScript.GetFrameCount / p.SourceScript.GetFramerate)
                p.SourceFrameRate = p.SourceScript.GetFramerate
                p.SourceFrames = p.SourceScript.GetFrameCount
            Catch ex As Exception
                MsgError("Source filter returned invalid parameters", p.SourceScript.GetFullScript)
                Throw New AbortException()
            End Try
        End If
    End Sub

    Sub tbSource_TextChanged(sender As Object, e As EventArgs) Handles tbSourceFile.TextChanged
        If Not BlockSourceTextBoxTextChanged Then
            If File.Exists(tbSourceFile.Text) Then
                OpenVideoSourceFile(tbSourceFile.Text)
            End If
        End If
    End Sub

    Sub gbTarget_LinkClick() Handles lgbTarget.LinkClick
        UpdateTargetFileMenu()
        TargetFileMenu.Show(lgbTarget, 0, lgbTarget.Label.Height)
    End Sub

    Sub gbSource_MenuClick() Handles lgbSource.LinkClick
        UpdateSourceFileMenu()

        If File.Exists(p.SourceFile) Then
            SourceFileMenu.Show(lgbSource, 0, lgbSource.Label.Height)
        Else
            ShowOpenSourceDialog()
        End If
    End Sub

    Sub tbTargetFile_DoubleClick() Handles tbTargetFile.DoubleClick
        Using dialog As New SaveFileDialog
            dialog.FileName = p.TargetFile.Base
            dialog.SetInitDir(p.TargetFile.Dir)

            If dialog.ShowDialog() = DialogResult.OK Then
                Dim ext = p.VideoEncoder.Muxer.OutputExtFull
                p.TargetFile = If(dialog.FileName.Ext = ext, dialog.FileName, dialog.FileName + ext)
            End If
        End Using
    End Sub

    Sub tbTargetFile_MouseDown(sender As Object, e As MouseEventArgs) Handles tbTargetFile.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateTargetFileMenu()
        End If
    End Sub

    Sub tbSourceFile_MouseDown(sender As Object, e As MouseEventArgs) Handles tbSourceFile.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateSourceFileMenu()
        End If
    End Sub

    Sub tbAudioFile0_MouseDown(sender As Object, e As MouseEventArgs) Handles tbAudioFile0.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateAudioFileMenu(Audio0FileMenu, AddressOf tbAudioFile0_DoubleClick, p.Audio0, tbAudioFile0)
        End If
    End Sub

    Sub tbAudioFile1_MouseDown(sender As Object, e As MouseEventArgs) Handles tbAudioFile1.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateAudioFileMenu(Audio1FileMenu, AddressOf tbAudioFile1_DoubleClick, p.Audio1, tbAudioFile1)
        End If
    End Sub

    Sub UpdateAudioFileMenu(m As ContextMenuStripEx, a As Action, ap As AudioProfile, tb As TextBox)
        Dim existT = Task.Run(Function() File.Exists(ap.File))
        m.SuspendLayout()
        Dim mItms As ToolStripItemCollection = m.Items
        mItms.ClearAndDispose()
        Dim apSC As Integer = ap.Streams.Count

        If apSC > 0 Then
            For Each i In ap.Streams
                Dim temp = i

                Dim menuAction = Sub()
                                     If Not EqualsExS(ap.File, p.LastOriginalSourceFile) Then
                                         tb.Text = p.LastOriginalSourceFile
                                     End If

                                     tb.Text = temp.Name & " (" & ap.File.Ext & ")"
                                     ap.Stream = temp

                                     If tb Is tbAudioFile0 Then
                                         llAudioProfile0.Text = g.ConvertPath(ap.Name)
                                     Else
                                         llAudioProfile1.Text = g.ConvertPath(ap.Name)
                                     End If

                                     UpdateSizeOrBitrate()
                                 End Sub

                If apSC > 10 Then
                    m.Add("Streams | " & i.Name, menuAction)
                Else
                    mItms.Add(New ActionMenuItem(i.Name, menuAction))
                End If
            Next

            mItms.Add(New ToolStripSeparator)
        End If

        If p.SourceFile.Ext.Equals("avs") OrElse p.Script.GetScript.ToLower(InvCult).Contains("audiodub(") Then
            mItms.Add(New ActionMenuItem(p.Script.Path.FileName, Sub() tb.Text = p.Script.Path))
        End If

        If p.TempDir.NotNullOrEmptyS AndAlso Directory.Exists(p.TempDir) Then
            Dim audioFiles = Directory.GetFiles(p.TempDir).WhereF(Function(audioPath) FileTypes.Audio.ContainsString(audioPath.Ext))
            Array.Sort(audioFiles, New StringLogicalComparer)

            If audioFiles.Length > 0 Then
                Dim apFL10 As Boolean = audioFiles.Length > 10
                For Each i In audioFiles
                    Dim temp = i

                    If apFL10 Then
                        m.Add("Files | " & i.FileName, Sub() tb.Text = temp)
                    Else
                        mItms.Add(New ActionMenuItem(i.FileName, Sub() tb.Text = temp))
                    End If
                Next

                mItms.Add(New ToolStripSeparator)
            End If
        End If

        Dim tbTN0 As Boolean = tb.Text.NotNullOrEmptyS
        Dim tbST As String = tb.SelectedText
        Dim exist = existT.Result
        mItms.AddRange({
            New ActionMenuItem("Open File", a, ImageHelp.GetImageC(Symbol.OpenFile), "Change the audio source file."),
            New ActionMenuItem("Add more files...", Sub()
                                                        s.Storage.SetInt("last selected muxer tab", 1)
                                                        p.VideoEncoder.OpenMuxerConfigDialog()
                                                    End Sub) With {.Enabled = exist},
            New ActionMenuItem("Play", Sub() g.PlayAudio(ap), ImageHelp.GetImageC(Symbol.Play), "Plays the audio source file with a media player.") With {.Enabled = exist},
            New ActionMenuItem("Media Info...", Sub() g.DefaultCommands.ShowMediaInfo(ap.File), ImageHelp.GetImageC(Symbol.Info), "Show MediaInfo for the audio source file.") With {.Enabled = exist},
            New ActionMenuItem("Explore", Sub() g.SelectFileWithExplorer(ap.File), ImageHelp.GetImageC(Symbol.FileExplorer), "Open the audio source file directory with File Explorer.") With {.Enabled = exist},
            New ActionMenuItem("Execute", Sub() ExecuteAudio(ap), "Processes the audio profile.") With {.Enabled = exist},
            New ToolStripSeparator,
            New ActionMenuItem("Copy Path", Sub() Clipboard.SetText(ap.File)) With {.Enabled = ap.File.NotNullOrEmptyS},
            New ActionMenuItem("Copy Selection", Sub() Clipboard.SetText(tbST), ImageHelp.GetImageC(Symbol.Copy)) With {.Enabled = tbST.NotNullOrEmptyS},
            New ActionMenuItem("Paste", Sub() tb.Paste(), ImageHelp.GetImageC(Symbol.Paste)) With {.Enabled = Clipboard.ContainsText},
            New ToolStripSeparator,
            New ActionMenuItem("Remove", Sub() tb.Clear(), ImageHelp.GetImageC(Symbol.Remove), "Remove audio file") With {.Enabled = tbTN0}})
        m.ResumeLayout()
    End Sub

    Sub ExecuteAudio(ap As AudioProfile)
        If MsgQuestion("Confirm to process the track.") = DialogResult.OK Then
            Try
                If p.TempDir.NullOrEmptyS Then
                    p.TempDir = ap.File.Dir
                End If

                ap = ap.GetDeepClone
                Audio.Process(ap)
                ap.Encode()
            Catch
            End Try
        End If
    End Sub

    Sub UpdateTargetFileMenu()
        Dim sFET = Task.Run(Function() File.Exists(p.SourceFile))
        Dim tFET = Task.Run(Function() File.Exists(p.TargetFile))
        TargetFileMenu.SuspendLayout()
        TargetFileMenu.Items.ClearAndDispose

        Dim sFE = sFET.Result
        Dim tFE = tFET.Result
        TargetFileMenu.Items.AddRange({
            New ActionMenuItem("Browse File...", AddressOf tbTargetFile_DoubleClick, "Change the path of the target file.") With {.Enabled = sFE},
            New ActionMenuItem("Play", Sub() g.Play(p.TargetFile), ImageHelp.GetImageC(Symbol.Play), "Play the target file.") With {.Enabled = tFE},
            New ActionMenuItem("Media Info...", Sub() g.DefaultCommands.ShowMediaInfo(p.TargetFile), ImageHelp.GetImageC(Symbol.Info), "Show MediaInfo for the target file.") With {.Enabled = tFE},
            New ActionMenuItem("Explore...", Sub() g.SelectFileWithExplorer(p.TargetFile), ImageHelp.GetImageC(Symbol.FileExplorer), "Open the target file directory with File Explorer.") With {.Enabled = Directory.Exists(p.TargetFile.Dir)},
            New ToolStripSeparator,
            New ActionMenuItem("Copy", Sub() tbTargetFile.Copy(), ImageHelp.GetImageC(Symbol.Copy)) With {.Enabled = tbTargetFile.Text.NotNullOrEmptyS},
            New ActionMenuItem("Paste", Sub() tbTargetFile.Paste(), ImageHelp.GetImageC(Symbol.Paste)) With {.Enabled = Clipboard.ContainsText AndAlso sFE}})
        TargetFileMenu.ResumeLayout()
    End Sub

    Sub UpdateSourceFileMenu()
        Dim fExIsIdxT = Task.Run(Function() File.Exists(p.LastOriginalSourceFile) AndAlso Not FileTypes.VideoIndex.ContainsString(p.LastOriginalSourceFile.Ext))
        SourceFileMenu.SuspendLayout()
        SourceFileMenu.Items.ClearAndDispose()

        Dim fExIsIdx = fExIsIdxT.Result
        SourceFileMenu.Items.AddRange({
            New ActionMenuItem("Open...", AddressOf ShowOpenSourceDialog, ImageHelp.GetImageC(Symbol.OpenFile), "Open source files"),
            New ActionMenuItem("Play", Sub() g.Play(p.LastOriginalSourceFile), ImageHelp.GetImageC(Symbol.Play), "Play the source file.") With {.Enabled = fExIsIdx},
            New ActionMenuItem("Media Info...", Sub() g.DefaultCommands.ShowMediaInfo(p.LastOriginalSourceFile), ImageHelp.GetImageC(Symbol.Info), "Show MediaInfo for the source file.") With {.Enabled = fExIsIdx},
            New ActionMenuItem("Explore...", Sub() g.SelectFileWithExplorer(p.SourceFile), ImageHelp.GetImageC(Symbol.FileExplorer), "Open the source file directory with File Explorer.") With {.Enabled = File.Exists(p.SourceFile)},
            New ToolStripSeparator,
            New ActionMenuItem("Copy", Sub() tbSourceFile.Copy(), ImageHelp.GetImageC(Symbol.Copy), "Copies the selected text to the clipboard.") With {.Enabled = tbSourceFile.Text.NotNullOrEmptyS},
            New ActionMenuItem("Paste", Sub() tbSourceFile.Paste(), ImageHelp.GetImageC(Symbol.Paste), "Copies the full source file path to the clipboard.") With {.Enabled = Clipboard.ContainsText}})
        SourceFileMenu.ResumeLayout()
    End Sub

    Protected Overrides Sub OnDragEnter(e As DragEventArgs)
        MyBase.OnDragEnter(e)

        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Protected Overrides Sub OnDragDrop(e As DragEventArgs)
        MyBase.OnDragDrop(e)

        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            BeginInvoke(Sub() OpenAnyFile(files))
        End If
    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)
        MyBase.OnActivated(e)
        UpdateNextButton()
        ProcController.LastActivation = Environment.TickCount
        BeginInvoke(New Action(Sub()
                                   Application.DoEvents()
                                   Assistant()
                                   'UpdateScriptsMenuAsync() ' Opt. Moved to OnShow.UpdateDynamicMenuAsync()  for now. No Dynamic Script Menu Update
                               End Sub))
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        UpdateDynamicMenuAsync()
        Application.DoEvents()
        UpdateRecentProjectsMenu()
        UpdateTemplatesMenuAsync()
        IsLoading = False
        Refresh()
        ProcessCommandLine(Environment.GetCommandLineArgs)
        StaxRip.StaxRipUpdate.ShowUpdateQuestion()
        StaxRip.StaxRipUpdate.CheckForUpdate(False, s.CheckForUpdatesBeta, True) ' Environment.Is64BitProcess) -this is extrem. fast BTW
        g.RunTask(AddressOf g.LoadPowerShellScripts)
        g.RunTask(AddressOf FrameServerHelp.VerifyAviSynthLinks)
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
        MyBase.OnFormClosing(args)

        If Not ForceClose AndAlso IsSaveCanceled() Then
            args.Cancel = True
        End If
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        If g.ProcForm IsNot Nothing Then
            g.ProcForm.Invoke(Sub() g.ProcForm.Close())
        End If

        If Not ForceClose Then g.SaveSettings()
        g.RaiseAppEvent(ApplicationEvent.ApplicationExit)

        ActionMenuItem.ClearAdd2RangeList() 'Debug
        ContextMenuStripEx.ClearAdd2RangeList()
        MediaInfo.ClearCache(False)

        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce 'Debug
        GC.Collect(2, GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()

        If s.WriteDebugLog Then
            s.WriteDebugLog = False 'This Needs some Work !!!
            Trace.Flush()
            Trace.Close()
            For Each tl As TraceListener In Trace.Listeners
                tl.Flush()
                tl.Close()
                tl.Dispose()
            Next tl
            Trace.Listeners.Clear()
        End If

        RemoveHandler Application.ThreadException, AddressOf g.OnUnhandledException
    End Sub

    Protected Overrides Sub OnDeactivate(e As EventArgs)
        MyBase.OnDeactivate(e)
        UpdateNextButton()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        UpdateNextButton()
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)
        UpdateNextButton()
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            If ProcController.BlockActivation Then
                ProcController.BlockActivation = False

                If ProcController.IsLastActivationLessThan(60) Then
                    Return True
                End If
            End If

            Return MyBase.ShowWithoutActivation
        End Get
    End Property

    Sub UpdateNextButton()
        If AssistantPassed AndAlso CanIgnoreTip AndAlso
            (ModifierKeys.HasFlag(Keys.Shift) OrElse ModifierKeys.HasFlag(Keys.Control)) Then

            Dim txt = "Add"

            If ModifierKeys.HasFlag(Keys.Shift) Then
                txt += " to top"
            End If

            If ModifierKeys.HasFlag(Keys.Control) Then
                txt += BR + "w/o opening"
            End If

            bnNext.Text = txt
        Else
            bnNext.Text = "Next"
        End If
    End Sub

    Sub bnNext_Click(sender As Object, e As EventArgs) Handles bnNext.Click
        Dim showJobsDialog = Not Control.ModifierKeys.HasFlag(Keys.Control)
        Dim position = If(Control.ModifierKeys.HasFlag(Keys.Shift), 0, -1)

        AddJob(showJobsDialog, position)
    End Sub

    Sub bnNext_MouseDown(sender As Object, e As MouseEventArgs) Handles bnNext.MouseDown
        If e.Button = MouseButtons.Right AndAlso AssistantPassed AndAlso CanIgnoreTip Then
            NextContextMenuStrip.Show(bnNext, 0, bnNext.Height)
        End If
    End Sub
End Class
