Imports System.Threading
Imports System.Threading.Tasks
Imports Force.DeepCloner
Imports KGySoft.CoreLibraries
Imports JM.LinqFaster
Imports StaxRip.UI

Public Class AudioForm
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

    Friend WithEvents CommandLink1 As StaxRip.UI.CommandLink
    Friend WithEvents gbBasic As System.Windows.Forms.GroupBox
    Friend WithEvents numQuality As NumEdit
    Friend WithEvents numBitrate As NumEdit
    Friend WithEvents lQualiy As System.Windows.Forms.Label
    Friend WithEvents lCodec As System.Windows.Forms.Label
    Friend WithEvents mbCodec As StaxRip.UI.MenuButton
    Friend WithEvents mbLanguage As StaxRip.UI.MenuButton
    Friend WithEvents lLanguage As System.Windows.Forms.Label
    Friend WithEvents numDelay As NumEdit
    Friend WithEvents lDelay As System.Windows.Forms.Label
    Friend WithEvents mbChannels As StaxRip.UI.MenuButton
    Friend WithEvents lChannels As System.Windows.Forms.Label
    Friend WithEvents gbAdvanced As System.Windows.Forms.GroupBox
    Friend WithEvents laProfileName As System.Windows.Forms.Label
    Friend WithEvents tbProfileName As System.Windows.Forms.TextBox
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents mbSamplingRate As StaxRip.UI.MenuButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents mbEncoder As StaxRip.UI.MenuButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents numGain As StaxRip.UI.NumEdit
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents flpButtons As FlowLayoutPanel
    Friend WithEvents tlpBasic As TableLayoutPanel
    Friend WithEvents bnMenu As ButtonEx
    Friend WithEvents tlpRTB As TableLayoutPanel
    Friend WithEvents rtbCommandLine As CommandLineRichTextBox
    Friend WithEvents laStreamName As Label
    Friend WithEvents tbStreamName As TextBox
    Friend WithEvents laCustom As Label
    Friend WithEvents tbCustom As TextBox
    Friend WithEvents cbForcedTrack As CheckBoxEx
    Friend WithEvents cbDefaultTrack As CheckBoxEx
    Friend WithEvents laDecoder As Label
    Friend WithEvents mbDecoder As MenuButton
    Friend WithEvents tlpAdvanced As TableLayoutPanel
    Friend WithEvents bnAdvanced As ButtonEx
    Friend WithEvents cbNormalize As CheckBoxEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.gbBasic = New System.Windows.Forms.GroupBox()
        Me.tlpBasic = New System.Windows.Forms.TableLayoutPanel()
        Me.lCodec = New System.Windows.Forms.Label()
        Me.tbProfileName = New System.Windows.Forms.TextBox()
        Me.laProfileName = New System.Windows.Forms.Label()
        Me.mbCodec = New StaxRip.UI.MenuButton()
        Me.mbLanguage = New StaxRip.UI.MenuButton()
        Me.mbSamplingRate = New StaxRip.UI.MenuButton()
        Me.lLanguage = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.numBitrate = New StaxRip.UI.NumEdit()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.mbEncoder = New StaxRip.UI.MenuButton()
        Me.lChannels = New System.Windows.Forms.Label()
        Me.mbChannels = New StaxRip.UI.MenuButton()
        Me.laStreamName = New System.Windows.Forms.Label()
        Me.tbStreamName = New System.Windows.Forms.TextBox()
        Me.laCustom = New System.Windows.Forms.Label()
        Me.tbCustom = New System.Windows.Forms.TextBox()
        Me.cbDefaultTrack = New StaxRip.UI.CheckBoxEx()
        Me.cbForcedTrack = New StaxRip.UI.CheckBoxEx()
        Me.laDecoder = New System.Windows.Forms.Label()
        Me.mbDecoder = New StaxRip.UI.MenuButton()
        Me.lQualiy = New System.Windows.Forms.Label()
        Me.numQuality = New StaxRip.UI.NumEdit()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.numGain = New StaxRip.UI.NumEdit()
        Me.cbNormalize = New StaxRip.UI.CheckBoxEx()
        Me.numDelay = New StaxRip.UI.NumEdit()
        Me.lDelay = New System.Windows.Forms.Label()
        Me.gbAdvanced = New System.Windows.Forms.GroupBox()
        Me.tlpAdvanced = New System.Windows.Forms.TableLayoutPanel()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.bnAdvanced = New StaxRip.UI.ButtonEx()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.tlpRTB = New System.Windows.Forms.TableLayoutPanel()
        Me.rtbCommandLine = New StaxRip.UI.CommandLineRichTextBox()
        Me.gbBasic.SuspendLayout()
        Me.tlpBasic.SuspendLayout()
        Me.gbAdvanced.SuspendLayout()
        Me.tlpAdvanced.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.flpButtons.SuspendLayout()
        Me.tlpRTB.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbBasic
        '
        Me.gbBasic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBasic.Controls.Add(Me.tlpBasic)
        Me.gbBasic.Location = New System.Drawing.Point(6, 5)
        Me.gbBasic.Margin = New System.Windows.Forms.Padding(6, 5, 3, 5)
        Me.gbBasic.Name = "gbBasic"
        Me.gbBasic.Padding = New System.Windows.Forms.Padding(2)
        Me.gbBasic.Size = New System.Drawing.Size(367, 409)
        Me.gbBasic.TabIndex = 1
        Me.gbBasic.TabStop = False
        Me.gbBasic.Text = "Basic"
        '
        'tlpBasic
        '
        Me.tlpBasic.ColumnCount = 4
        Me.tlpBasic.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBasic.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.tlpBasic.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBasic.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.tlpBasic.Controls.Add(Me.lCodec, 0, 0)
        Me.tlpBasic.Controls.Add(Me.tbProfileName, 1, 7)
        Me.tlpBasic.Controls.Add(Me.laProfileName, 0, 7)
        Me.tlpBasic.Controls.Add(Me.mbCodec, 1, 0)
        Me.tlpBasic.Controls.Add(Me.mbLanguage, 1, 5)
        Me.tlpBasic.Controls.Add(Me.mbSamplingRate, 1, 4)
        Me.tlpBasic.Controls.Add(Me.lLanguage, 0, 5)
        Me.tlpBasic.Controls.Add(Me.Label1, 0, 4)
        Me.tlpBasic.Controls.Add(Me.Label3, 2, 0)
        Me.tlpBasic.Controls.Add(Me.numBitrate, 3, 0)
        Me.tlpBasic.Controls.Add(Me.Label2, 0, 2)
        Me.tlpBasic.Controls.Add(Me.mbEncoder, 1, 2)
        Me.tlpBasic.Controls.Add(Me.lChannels, 0, 3)
        Me.tlpBasic.Controls.Add(Me.mbChannels, 1, 3)
        Me.tlpBasic.Controls.Add(Me.laStreamName, 0, 8)
        Me.tlpBasic.Controls.Add(Me.tbStreamName, 1, 8)
        Me.tlpBasic.Controls.Add(Me.laCustom, 0, 9)
        Me.tlpBasic.Controls.Add(Me.tbCustom, 1, 9)
        Me.tlpBasic.Controls.Add(Me.cbDefaultTrack, 0, 11)
        Me.tlpBasic.Controls.Add(Me.cbForcedTrack, 0, 12)
        Me.tlpBasic.Controls.Add(Me.laDecoder, 0, 1)
        Me.tlpBasic.Controls.Add(Me.mbDecoder, 1, 1)
        Me.tlpBasic.Controls.Add(Me.lQualiy, 2, 1)
        Me.tlpBasic.Controls.Add(Me.numQuality, 3, 1)
        Me.tlpBasic.Controls.Add(Me.Label4, 2, 2)
        Me.tlpBasic.Controls.Add(Me.numGain, 3, 2)
        Me.tlpBasic.Controls.Add(Me.cbNormalize, 0, 10)
        Me.tlpBasic.Controls.Add(Me.numDelay, 3, 3)
        Me.tlpBasic.Controls.Add(Me.lDelay, 2, 3)
        Me.tlpBasic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBasic.Location = New System.Drawing.Point(2, 18)
        Me.tlpBasic.Margin = New System.Windows.Forms.Padding(2)
        Me.tlpBasic.Name = "tlpBasic"
        Me.tlpBasic.RowCount = 14
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3.0!))
        Me.tlpBasic.Size = New System.Drawing.Size(363, 389)
        Me.tlpBasic.TabIndex = 44
        '
        'lCodec
        '
        Me.lCodec.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lCodec.AutoSize = True
        Me.lCodec.Location = New System.Drawing.Point(2, 7)
        Me.lCodec.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lCodec.Name = "lCodec"
        Me.lCodec.Size = New System.Drawing.Size(44, 15)
        Me.lCodec.TabIndex = 0
        Me.lCodec.Text = "Codec:"
        '
        'tbProfileName
        '
        Me.tbProfileName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBasic.SetColumnSpan(Me.tbProfileName, 3)
        Me.tbProfileName.Location = New System.Drawing.Point(85, 182)
        Me.tbProfileName.Margin = New System.Windows.Forms.Padding(2)
        Me.tbProfileName.Name = "tbProfileName"
        Me.tbProfileName.Size = New System.Drawing.Size(276, 23)
        Me.tbProfileName.TabIndex = 16
        '
        'laProfileName
        '
        Me.laProfileName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laProfileName.AutoSize = True
        Me.laProfileName.Location = New System.Drawing.Point(2, 186)
        Me.laProfileName.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.laProfileName.Name = "laProfileName"
        Me.laProfileName.Size = New System.Drawing.Size(79, 15)
        Me.laProfileName.TabIndex = 15
        Me.laProfileName.Text = "Profile Name:"
        '
        'mbCodec
        '
        Me.mbCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbCodec.Location = New System.Drawing.Point(85, 2)
        Me.mbCodec.Margin = New System.Windows.Forms.Padding(2)
        Me.mbCodec.ShowMenuSymbol = True
        Me.mbCodec.Size = New System.Drawing.Size(132, 26)
        Me.mbCodec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'mbLanguage
        '
        Me.mbLanguage.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbLanguage.Location = New System.Drawing.Point(85, 152)
        Me.mbLanguage.Margin = New System.Windows.Forms.Padding(2)
        Me.mbLanguage.ShowMenuSymbol = True
        Me.mbLanguage.Size = New System.Drawing.Size(132, 26)
        Me.mbLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'mbSamplingRate
        '
        Me.mbSamplingRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbSamplingRate.Location = New System.Drawing.Point(85, 122)
        Me.mbSamplingRate.Margin = New System.Windows.Forms.Padding(2)
        Me.mbSamplingRate.ShowMenuSymbol = True
        Me.mbSamplingRate.Size = New System.Drawing.Size(132, 26)
        Me.mbSamplingRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lLanguage
        '
        Me.lLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lLanguage.AutoSize = True
        Me.lLanguage.Location = New System.Drawing.Point(2, 157)
        Me.lLanguage.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lLanguage.Name = "lLanguage"
        Me.lLanguage.Size = New System.Drawing.Size(62, 15)
        Me.lLanguage.TabIndex = 10
        Me.lLanguage.Text = "Language:"
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 127)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 15)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Sample Rate:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(221, 7)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 15)
        Me.Label3.TabIndex = 29
        Me.Label3.TabStop = True
        Me.Label3.Text = "Bitrate:"
        '
        'numBitrate
        '
        Me.numBitrate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numBitrate.Location = New System.Drawing.Point(273, 2)
        Me.numBitrate.Margin = New System.Windows.Forms.Padding(2)
        Me.numBitrate.Maximum = 25000.0R
        Me.numBitrate.Minimum = 1.0R
        Me.numBitrate.Name = "numBitrate"
        Me.numBitrate.Size = New System.Drawing.Size(88, 26)
        Me.numBitrate.TabIndex = 17
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 67)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 15)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "Encoder:"
        '
        'mbEncoder
        '
        Me.mbEncoder.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbEncoder.Location = New System.Drawing.Point(85, 62)
        Me.mbEncoder.Margin = New System.Windows.Forms.Padding(2)
        Me.mbEncoder.ShowMenuSymbol = True
        Me.mbEncoder.Size = New System.Drawing.Size(132, 26)
        Me.mbEncoder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lChannels
        '
        Me.lChannels.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lChannels.AutoSize = True
        Me.lChannels.Location = New System.Drawing.Point(2, 97)
        Me.lChannels.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lChannels.Name = "lChannels"
        Me.lChannels.Size = New System.Drawing.Size(59, 15)
        Me.lChannels.TabIndex = 5
        Me.lChannels.Text = "Channels:"
        '
        'mbChannels
        '
        Me.mbChannels.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbChannels.Location = New System.Drawing.Point(85, 92)
        Me.mbChannels.Margin = New System.Windows.Forms.Padding(2)
        Me.mbChannels.ShowMenuSymbol = True
        Me.mbChannels.Size = New System.Drawing.Size(132, 26)
        Me.mbChannels.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'laStreamName
        '
        Me.laStreamName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laStreamName.AutoSize = True
        Me.laStreamName.Location = New System.Drawing.Point(2, 213)
        Me.laStreamName.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.laStreamName.Name = "laStreamName"
        Me.laStreamName.Size = New System.Drawing.Size(72, 15)
        Me.laStreamName.TabIndex = 44
        Me.laStreamName.Text = "Track Name:"
        '
        'tbStreamName
        '
        Me.tbStreamName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBasic.SetColumnSpan(Me.tbStreamName, 3)
        Me.tbStreamName.Location = New System.Drawing.Point(85, 209)
        Me.tbStreamName.Margin = New System.Windows.Forms.Padding(2)
        Me.tbStreamName.Name = "tbStreamName"
        Me.tbStreamName.Size = New System.Drawing.Size(276, 23)
        Me.tbStreamName.TabIndex = 45
        '
        'laCustom
        '
        Me.laCustom.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laCustom.AutoSize = True
        Me.laCustom.Location = New System.Drawing.Point(2, 240)
        Me.laCustom.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.laCustom.Name = "laCustom"
        Me.laCustom.Size = New System.Drawing.Size(49, 15)
        Me.laCustom.TabIndex = 46
        Me.laCustom.Text = "Custom"
        '
        'tbCustom
        '
        Me.tbCustom.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBasic.SetColumnSpan(Me.tbCustom, 3)
        Me.tbCustom.Location = New System.Drawing.Point(85, 236)
        Me.tbCustom.Margin = New System.Windows.Forms.Padding(2)
        Me.tbCustom.Name = "tbCustom"
        Me.tbCustom.Size = New System.Drawing.Size(276, 23)
        Me.tbCustom.TabIndex = 47
        '
        'cbDefaultTrack
        '
        Me.cbDefaultTrack.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbDefaultTrack, 4)
        Me.cbDefaultTrack.Location = New System.Drawing.Point(6, 283)
        Me.cbDefaultTrack.Margin = New System.Windows.Forms.Padding(6, 1, 1, 1)
        Me.cbDefaultTrack.Size = New System.Drawing.Size(94, 19)
        Me.cbDefaultTrack.Text = "Default Track"
        Me.cbDefaultTrack.UseVisualStyleBackColor = True
        '
        'cbForcedTrack
        '
        Me.cbForcedTrack.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbForcedTrack, 4)
        Me.cbForcedTrack.Location = New System.Drawing.Point(6, 304)
        Me.cbForcedTrack.Margin = New System.Windows.Forms.Padding(6, 1, 1, 1)
        Me.cbForcedTrack.Size = New System.Drawing.Size(92, 19)
        Me.cbForcedTrack.Text = "Forced Track"
        Me.cbForcedTrack.UseVisualStyleBackColor = True
        '
        'laDecoder
        '
        Me.laDecoder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laDecoder.AutoSize = True
        Me.laDecoder.Location = New System.Drawing.Point(2, 37)
        Me.laDecoder.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.laDecoder.Name = "laDecoder"
        Me.laDecoder.Size = New System.Drawing.Size(54, 15)
        Me.laDecoder.TabIndex = 50
        Me.laDecoder.Text = "Decoder:"
        '
        'mbDecoder
        '
        Me.mbDecoder.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbDecoder.Location = New System.Drawing.Point(85, 32)
        Me.mbDecoder.Margin = New System.Windows.Forms.Padding(2)
        Me.mbDecoder.ShowMenuSymbol = True
        Me.mbDecoder.Size = New System.Drawing.Size(132, 26)
        Me.mbDecoder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lQualiy
        '
        Me.lQualiy.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lQualiy.AutoSize = True
        Me.lQualiy.Location = New System.Drawing.Point(221, 37)
        Me.lQualiy.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lQualiy.Name = "lQualiy"
        Me.lQualiy.Size = New System.Drawing.Size(48, 15)
        Me.lQualiy.TabIndex = 12
        Me.lQualiy.TabStop = True
        Me.lQualiy.Text = "Quality:"
        '
        'numQuality
        '
        Me.numQuality.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numQuality.Location = New System.Drawing.Point(273, 32)
        Me.numQuality.Margin = New System.Windows.Forms.Padding(2)
        Me.numQuality.Name = "numQuality"
        Me.numQuality.Size = New System.Drawing.Size(88, 26)
        Me.numQuality.TabIndex = 18
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(221, 67)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 15)
        Me.Label4.TabIndex = 36
        Me.Label4.TabStop = True
        Me.Label4.Text = "Gain:"
        '
        'numGain
        '
        Me.numGain.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numGain.DecimalPlaces = 1
        Me.numGain.Increment = 0.5R
        Me.numGain.Location = New System.Drawing.Point(273, 62)
        Me.numGain.Margin = New System.Windows.Forms.Padding(2)
        Me.numGain.Name = "numGain"
        Me.numGain.Size = New System.Drawing.Size(88, 26)
        Me.numGain.TabIndex = 37
        '
        'cbNormalize
        '
        Me.cbNormalize.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbNormalize, 4)
        Me.cbNormalize.Location = New System.Drawing.Point(6, 262)
        Me.cbNormalize.Margin = New System.Windows.Forms.Padding(6, 1, 1, 1)
        Me.cbNormalize.Size = New System.Drawing.Size(80, 19)
        Me.cbNormalize.Text = "Normalize"
        Me.cbNormalize.UseVisualStyleBackColor = True
        '
        'numDelay
        '
        Me.numDelay.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numDelay.Location = New System.Drawing.Point(273, 92)
        Me.numDelay.Margin = New System.Windows.Forms.Padding(2)
        Me.numDelay.Name = "numDelay"
        Me.numDelay.Size = New System.Drawing.Size(88, 26)
        Me.numDelay.TabIndex = 19
        '
        'lDelay
        '
        Me.lDelay.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lDelay.AutoSize = True
        Me.lDelay.Location = New System.Drawing.Point(221, 97)
        Me.lDelay.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lDelay.Name = "lDelay"
        Me.lDelay.Size = New System.Drawing.Size(39, 15)
        Me.lDelay.TabIndex = 14
        Me.lDelay.TabStop = True
        Me.lDelay.Text = "Delay:"
        '
        'gbAdvanced
        '
        Me.gbAdvanced.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAdvanced.Controls.Add(Me.tlpAdvanced)
        Me.gbAdvanced.Location = New System.Drawing.Point(379, 5)
        Me.gbAdvanced.Margin = New System.Windows.Forms.Padding(3, 5, 6, 5)
        Me.gbAdvanced.Name = "gbAdvanced"
        Me.gbAdvanced.Padding = New System.Windows.Forms.Padding(2)
        Me.gbAdvanced.Size = New System.Drawing.Size(367, 409)
        Me.gbAdvanced.TabIndex = 3
        Me.gbAdvanced.TabStop = False
        Me.gbAdvanced.Text = "Advanced"
        '
        'tlpAdvanced
        '
        Me.tlpAdvanced.ColumnCount = 1
        Me.tlpAdvanced.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAdvanced.Controls.Add(Me.SimpleUI, 0, 0)
        Me.tlpAdvanced.Controls.Add(Me.bnAdvanced, 0, 1)
        Me.tlpAdvanced.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAdvanced.Location = New System.Drawing.Point(2, 18)
        Me.tlpAdvanced.Name = "tlpAdvanced"
        Me.tlpAdvanced.RowCount = 2
        Me.tlpAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAdvanced.Size = New System.Drawing.Size(363, 389)
        Me.tlpAdvanced.TabIndex = 1
        '
        'SimpleUI
        '
        Me.SimpleUI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleUI.FormSizeScaleFactor = New System.Drawing.SizeF(0!, 0!)
        Me.SimpleUI.Location = New System.Drawing.Point(2, 1)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(2, 1, 2, 1)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(359, 355)
        Me.SimpleUI.TabIndex = 0
        Me.SimpleUI.Text = "SimpleUI1"
        '
        'bnAdvanced
        '
        Me.bnAdvanced.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdvanced.Location = New System.Drawing.Point(3, 360)
        Me.bnAdvanced.Size = New System.Drawing.Size(357, 26)
        Me.bnAdvanced.Text = "More..."
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(44, 6)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(6)
        Me.bnOK.Size = New System.Drawing.Size(94, 26)
        Me.bnOK.Text = "OK"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(144, 6)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(0, 6, 6, 6)
        Me.bnCancel.Size = New System.Drawing.Size(94, 26)
        Me.bnCancel.Text = "Cancel"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(10, 494)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(2)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(0, 0)
        Me.FlowLayoutPanel1.TabIndex = 4
        '
        'tlpMain
        '
        Me.tlpMain.AutoSize = True
        Me.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Controls.Add(Me.gbBasic, 0, 0)
        Me.tlpMain.Controls.Add(Me.flpButtons, 1, 2)
        Me.tlpMain.Controls.Add(Me.gbAdvanced, 1, 0)
        Me.tlpMain.Controls.Add(Me.tlpRTB, 0, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(752, 473)
        Me.tlpMain.TabIndex = 11
        '
        'flpButtons
        '
        Me.flpButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.flpButtons.AutoSize = True
        Me.flpButtons.Controls.Add(Me.bnMenu)
        Me.flpButtons.Controls.Add(Me.bnOK)
        Me.flpButtons.Controls.Add(Me.bnCancel)
        Me.flpButtons.Location = New System.Drawing.Point(508, 435)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(244, 38)
        Me.flpButtons.TabIndex = 11
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnMenu.Location = New System.Drawing.Point(0, 6)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(38, 26)
        '
        'tlpRTB
        '
        Me.tlpRTB.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpRTB.AutoSize = True
        Me.tlpRTB.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpRTB.ColumnCount = 1
        Me.tlpMain.SetColumnSpan(Me.tlpRTB, 2)
        Me.tlpRTB.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRTB.Controls.Add(Me.rtbCommandLine, 0, 0)
        Me.tlpRTB.Location = New System.Drawing.Point(6, 419)
        Me.tlpRTB.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tlpRTB.Name = "tlpRTB"
        Me.tlpRTB.RowCount = 1
        Me.tlpRTB.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRTB.Size = New System.Drawing.Size(740, 16)
        Me.tlpRTB.TabIndex = 12
        '
        'rtbCommandLine
        '
        Me.rtbCommandLine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbCommandLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbCommandLine.LastCommandLine = Nothing
        Me.rtbCommandLine.Location = New System.Drawing.Point(0, 0)
        Me.rtbCommandLine.Margin = New System.Windows.Forms.Padding(0)
        Me.rtbCommandLine.Name = "rtbCommandLine"
        Me.rtbCommandLine.ReadOnly = True
        Me.rtbCommandLine.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.rtbCommandLine.Size = New System.Drawing.Size(740, 16)
        Me.rtbCommandLine.TabIndex = 45
        Me.rtbCommandLine.Text = ""
        '
        'AudioForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(752, 473)
        Me.Controls.Add(Me.tlpMain)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Name = "AudioForm"
        Me.Text = "Audio Settings"
        Me.gbBasic.ResumeLayout(False)
        Me.tlpBasic.ResumeLayout(False)
        Me.tlpBasic.PerformLayout()
        Me.gbAdvanced.ResumeLayout(False)
        Me.tlpAdvanced.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.flpButtons.ResumeLayout(False)
        Me.tlpRTB.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Profile, TempProfile As GUIAudioProfile
    Private NumFFLFEMixLevel As SimpleUI.NumBlock
    Private MbMode As SimpleUI.MenuBlock(Of Integer)
    Private MbModeH As MenuButton.ValueChangedUserEventHandler
    Private CbQaacHE As SimpleUI.SimpleUICheckBox
    Private CbQaacHEH As EventHandler
    Private MbCompressionLevel As SimpleUI.NumBlock
    Private MbCompressionLevelH As NumEdit.ValueChangedEventHandler
    Private MbWavPackMode As SimpleUI.MenuBlock(Of Integer)
    Private MbWavPackModeH As MenuButton.ValueChangedUserEventHandler
    Private WasHandleCtreated As Boolean

    Sub New()
        MyBase.New()
        InitializeComponent()
        'rtbCommandLine.ReadOnly = True'InDesigner
        mbSamplingRate.Menu.SuspendLayout()
        mbSamplingRate.Add("Original", 0)
        mbSamplingRate.Add("11025 Hz", 11025)
        mbSamplingRate.Add("22050 Hz", 22050)
        mbSamplingRate.Add("44100 Hz", 44100)
        mbSamplingRate.Add("48000 Hz", 48000)
        mbSamplingRate.Add("88200 Hz", 88200)
        mbSamplingRate.Add("96000 Hz", 96000)
        mbSamplingRate.Menu.ResumeLayout(False)
        'numBitrate.Minimum = 1 'InDesigner
        'numBitrate.Maximum = 25000
        'numGain.DecimalPlaces = 1
        'numGain.Increment = 0.5
        cbDefaultTrack.Visible = TypeOf p.VideoEncoder.Muxer Is MkvMuxer
        cbForcedTrack.Visible = TypeOf p.VideoEncoder.Muxer Is MkvMuxer
        'If components Is Nothing then components = New System.ComponentModel.Container '????
        'rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None'InDesigner

        Dim cms As New ContextMenuStripEx(components)
        cms.SuspendLayout()
        bnMenu.ContextMenuStrip = cms
        cms.Add("Copy Command Line", Sub() Clipboard.SetText(TempProfile.GetCommandLine(True))).SetImage(Symbol.Copy)
        cms.Add("Execute Command Line", AddressOf Execute).SetImage(Symbol.fa_terminal)
        cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Line", TempProfile.GetCommandLine(True)))
        cms.Add("-")
        cms.Add("Save Profile...", AddressOf SaveProfile, "Saves the current settings as profile").SetImage(Symbol.Save)
        cms.Add("-")
        cms.Add("Help", AddressOf ShowHelp).SetImage(Symbol.Help)
        cms.Add("eac3to Help", Sub() g.ShellExecute("http://en.wikibooks.org/wiki/Eac3to"))
        cms.Add("ffmpeg Help", Sub() Package.ffmpeg.ShowHelp())
        cms.Add("qaac Help", Sub() Package.qaac.ShowHelp())
        cms.Add("qaac Formats", Sub() MsgInfo(ProcessHelp.GetConsoleOutput(Package.qaac.Path, "--formats")))
        cms.Add("Opus Help", Sub() Package.OpusEnc.ShowHelp())
        cms.ResumeLayout(False)

        TipProvider.SetTip("Defines which decoder to use and forces decoding even if not necessary.", laDecoder, mbDecoder)
        TipProvider.SetTip("Profile name that is auto generated when undefined.", laProfileName)
        TipProvider.SetTip("Language used by the muxer. Saved in projects/templates but not in profiles.", mbLanguage, lLanguage)
        TipProvider.SetTip("Delay in milliseconds. eac3to handles delay, ffmpeg don't but it is handled by the muxer. Saved in projects/templates but not in profiles.", numDelay, lDelay)
        TipProvider.SetTip("Track name used by the muxer.", tbStreamName, laStreamName)
        TipProvider.SetTip("Custom command line arguments.", tbCustom, laCustom)
        TipProvider.SetTip("Default MKV Track.", cbDefaultTrack)
        TipProvider.SetTip("Forced MKV Track.", cbForcedTrack)

        mbLanguage.BuildLangMenu(False)
        ActiveControl = mbCodec
    End Sub



    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        WasHandleCtreated = False
        If DialogResult = DialogResult.OK Then
            SetValues(Profile)
        End If
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        WasHandleCtreated = True
        UpdateControls() ' Needed???
        'mbLanguage.Menu.Invalidate(True) 
        'Invalidate(True)
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        ShowHelp()
        hevent.Handled = True
        MyBase.OnHelpRequested(hevent)
    End Sub

    Sub SetValues(gap As GUIAudioProfile)
        gap.Bitrate = TempProfile.Bitrate
        gap.Language = TempProfile.Language
        gap.Delay = TempProfile.Delay
        gap.DefaultnameValue = Nothing
        gap.Name = TempProfile.Name
        gap.StreamName = TempProfile.StreamName
        gap.Gain = TempProfile.Gain
        gap.Default = TempProfile.Default
        gap.Forced = TempProfile.Forced
        gap.Params = TempProfile.Params
        gap.Decoder = TempProfile.Decoder
        gap.DecodingMode = TempProfile.DecodingMode
        gap.ExtractDTSCore = TempProfile.ExtractDTSCore
        gap.Depth = TempProfile.Depth
    End Sub

    Sub UpdateBitrate()
        nudQuality_ValueChanged(numQuality)
    End Sub

    Sub nudQuality_ValueChanged(numEdit As NumEdit) Handles numQuality.ValueChanged
        If Not TempProfile Is Nothing Then
            TempProfile.Params.Quality = CSng(numQuality.Value)
            numBitrate.Value = TempProfile.GetBitrate
            UpdateControls()
        End If
    End Sub

    Sub nudBitrate_ValueChanged(numEdit As NumEdit) Handles numBitrate.ValueChanged
        If Not TempProfile Is Nothing Then
            TempProfile.Bitrate = CSng(numBitrate.Value)
            UpdateControls()
        End If
    End Sub

    Sub nudDelay_ValueChanged(numEdit As NumEdit) Handles numDelay.ValueChanged
        TempProfile.Delay = CInt(numDelay.Value)
        UpdateControls()
    End Sub

    Sub numGain_ValueChanged(numEdit As NumEdit) Handles numGain.ValueChanged
        TempProfile.Gain = Math.Round(numGain.Value, 1)
        UpdateControls()
    End Sub

    Sub SimpleUIValueChanged()
        SimpleUI.Save()
        UpdateControls()
    End Sub

    Sub UpdateControls()
        If WasHandleCtreated Then BeginInvoke(Sub() UpdateControlsA())
    End Sub
    Sub UpdateControlsA()
        If TempProfile.ExtractCore Then
            numQuality.Enabled = False
            numBitrate.Enabled = False
            If NumFFLFEMixLevel IsNot Nothing Then NumFFLFEMixLevel.Enabled = False
        Else
            Select Case TempProfile.Params.Codec
                Case AudioCodec.Opus, AudioCodec.FLAC, AudioCodec.W64, AudioCodec.WAV, AudioCodec.DTS, AudioCodec.WavPack
                    numQuality.Enabled = False
                Case Else
                    numQuality.Enabled = TempProfile.VBRMode()
            End Select

            Select Case TempProfile.Params.Codec
                Case AudioCodec.FLAC, AudioCodec.WAV, AudioCodec.W64
                    numBitrate.Enabled = False
                Case AudioCodec.WavPack
                    numBitrate.Enabled = TempProfile.Params.WavPackMode = 1
                Case Else
                    numBitrate.Enabled = Not numQuality.Enabled
            End Select

            If NumFFLFEMixLevel IsNot Nothing Then
                NumFFLFEMixLevel.Enabled = TempProfile.Params.ChannelsMode = 2 OrElse TempProfile.Params.ChannelsMode = 1
            End If
        End If

        mbDecoder.Enabled = Not TempProfile.ExtractCore
        mbChannels.Enabled = Not TempProfile.ExtractCore
        mbSamplingRate.Enabled = Not TempProfile.ExtractCore
        cbNormalize.Enabled = Not TempProfile.ExtractCore
        numGain.Enabled = Not TempProfile.ExtractCore
        numBitrate.Increment = If(TempProfile.Params.Codec = AudioCodec.AC3 OrElse TempProfile.Params.Codec = AudioCodec.EAC3, 32D, 8D)
        TempProfile.DefaultnameValue = Nothing
        tbProfileName.SendMessageCue(TempProfile.Name, False)

        Dim t As String = TempProfile.GetCommandLine(False)
        rtbCommandLine.SetText(t)
        Dim sh As Integer = (t.Length \ 88 + 1) * 16 + 2
        Dim rh = rtbCommandLine.Height + 1

        If rh < sh OrElse t.Length < 150 AndAlso rh > 36 Then
            rtbCommandLine.Size = New Size(rtbCommandLine.Width, sh)
        End If
        'rtbCommandLine.UpdateHeight()
        'Text = t.Length.ToInvariantString & "stringl|h:" & rtbCommandLine.Height & "|CalH:" & sh
    End Sub

    Sub mbCodec_ValueChangedUser() Handles mbCodec.ValueChangedUser
        TempProfile.Params.Codec = mbCodec.GetValue(Of AudioCodec)()

        Select Case TempProfile.Params.Codec
            Case AudioCodec.AAC
                Select Case TempProfile.Params.Encoder
                    Case GuiAudioEncoder.eac3to
                        SetQuality(0.5)
                    Case GuiAudioEncoder.ffmpeg, GuiAudioEncoder.fdkaac
                        If TempProfile.Params.Encoder = GuiAudioEncoder.fdkaac Then TempProfile.Depth = 16 'FDKAAC eats only int16
                        TempProfile.Params.SimpleRateMode = SimpleAudioRateMode.VBR
                        SetQuality(3)
                    Case Else
                        SetQuality(54)
                        'TempProfile.Params.qaacQuality = 2
                        TempProfile.Params.qaacLowpass = 0
                        TempProfile.Params.qaacRateMode = 0
                        TempProfile.Params.qaacHE = False
                End Select

            Case AudioCodec.AC3, AudioCodec.EAC3
                If TempProfile.Channels = 6 Then
                    numBitrate.Value = 448
                Else
                    numBitrate.Value = 224
                End If

                'TempProfile.Params.RateMode = AudioRateMode.CBR
            Case AudioCodec.FLAC
                TempProfile.Params.ffmpegCompressionLevel = 5
                TempProfile.Depth = 0
                numBitrate.Value = TempProfile.GetBitrate
                'TempProfile.Params.RateMode = AudioRateMode.VBR

            Case AudioCodec.WavPack
                TempProfile.Params.WavPackMode = 0
                numBitrate.Value = TempProfile.GetBitrate
                TempProfile.Params.WavPackCreateCorrection = False
                TempProfile.Params.ffmpegCompressionLevel = 1
                TempProfile.Depth = If(TempProfile.Params.Encoder = GuiAudioEncoder.ffmpeg, 0, 32)
                'TempProfile.Params.RateMode = AudioRateMode.VBR
                TempProfile.Params.WavPackCompression = 1
                TempProfile.Params.WavPackExtraCompression = 0
                TempProfile.Params.WavPackPreQuant = 0

            Case AudioCodec.W64
                TempProfile.Depth = 0
                'TempProfile.Params.RateMode = AudioRateMode.CBR

            Case AudioCodec.WAV
                TempProfile.Depth = 0
                'TempProfile.Params.RateMode = AudioRateMode.CBR

            Case AudioCodec.DTS
                If TempProfile.Channels = 6 Then
                    numBitrate.Value = 1536
                Else
                    numBitrate.Value = 768
                End If
                TempProfile.ExtractDTSCore = False

                'TempProfile.Params.RateMode = AudioRateMode.CBR
            Case AudioCodec.MP3
                SetQuality(4)
                TempProfile.Params.RateMode = AudioRateMode.VBR
                TempProfile.Params.ffmpegMp3Cutoff = 0
            Case AudioCodec.Vorbis
                SetQuality(1)
                TempProfile.Params.RateMode = AudioRateMode.VBR
                TempProfile.Params.ffmpegMp3Cutoff = 0
            Case AudioCodec.Opus
                numBitrate.Value = If(TempProfile.Channels = 6, 256, TempProfile.Channels * 96 / 2)
                'TempProfile.Params.RateMode = AudioRateMode.VBR
                TempProfile.Params.opusEncNoPhaseInv = False
                'TempProfile.Params.ffmpegOpusCompress = 10
                TempProfile.Params.ffmpegOpusRateMode = OpusRateMode.VBR
                'TempProfile.Params.ffmpegOpusApp = OpusApp.audio
                TempProfile.Params.ffmpegOpusFrame = 20
                'TempProfile.Params.ffmpegOpusPacket = 0
                Dim C12 = If(TempProfile.Channels > 0, 0, -1)
                TempProfile.Params.ffmpegOpusMap = If(TempProfile.Channels > 5, 1, C12)
                'TempProfile.Params.opusEncTuning = OpusEncTune.auto
                'TempProfile.Params.opusEncDelay = 1000
                'Opus workaround for side channels
                If TempProfile.Params.Encoder = GuiAudioEncoder.ffmpeg Then
                    ChannelsModeToChannel(TempProfile.Channels)
                End If
        End Select

        If Not TempProfile.IntegerCodec() Then TempProfile.Depth = 0

        UpdateBitrate()
        TempProfile.GetCommandLine(False) 'set encoder
        LoadAdvanced()
        UpdateControls()
    End Sub

    Private Sub ChannelsModeToChannel(Channels As Integer)
        Select Case Channels
            Case 1, 2, 6 To 8
                Dim channV = [Enum](Of ChannelsMode).Parse("_" & Channels)
                TempProfile.Params.ChannelsMode = channV
                mbChannels.Value = channV
        End Select
    End Sub

    Sub SetQuality(value As Single)
        If TempProfile.Params.Codec = AudioCodec.AAC Then
            Select Case TempProfile.Params.Encoder
                Case GuiAudioEncoder.qaac, GuiAudioEncoder.Automatic
                    numQuality.Minimum = 0
                    numQuality.Maximum = 127
                    numQuality.Increment = 9
                    numQuality.DecimalPlaces = 0
                Case GuiAudioEncoder.eac3to
                    numQuality.Minimum = 0
                    numQuality.Maximum = 1
                    numQuality.Increment = 0.01
                    numQuality.DecimalPlaces = 2
                Case GuiAudioEncoder.fdkaac, GuiAudioEncoder.ffmpeg
                    numQuality.Minimum = 1
                    numQuality.Maximum = 5
                    numQuality.Increment = 1
                    numQuality.DecimalPlaces = 0
                Case Else
                    numQuality.Minimum = 0
                    numQuality.Maximum = 127
                    numQuality.Increment = 9
                    numQuality.DecimalPlaces = 0
            End Select
        ElseIf TempProfile.Params.Codec = AudioCodec.MP3 Then
            numQuality.Minimum = 0
            numQuality.Maximum = 9
            numQuality.Increment = 1
            numQuality.DecimalPlaces = 0
        ElseIf TempProfile.Params.Codec = AudioCodec.Vorbis Then
            numQuality.Minimum = 0
            numQuality.Maximum = 10
            numQuality.Increment = 1
            numQuality.DecimalPlaces = 0
        Else
            numQuality.Minimum = 0
            numQuality.Maximum = Integer.MaxValue
            numQuality.Increment = 0.01
            numQuality.DecimalPlaces = 2
        End If

        numQuality.Value = value
    End Sub

    Sub mbSamplingRate_ValueChanged() Handles mbSamplingRate.ValueChangedUser
        TempProfile.Params.SamplingRate = mbSamplingRate.GetValue(Of Integer)()
        UpdateBitrate()
        UpdateControls()
    End Sub

    Sub mbLanguage_ValueChanged() Handles mbLanguage.ValueChangedUser
        TempProfile.Language = mbLanguage.GetValue(Of Language)()
        mbLanguage.Text = TempProfile.Language.Name
        UpdateControls()
    End Sub

    Sub tbName_TextChanged(sender As Object, e As EventArgs) Handles tbProfileName.TextChanged
        TempProfile.Name = tbProfileName.Text
        UpdateControls()
    End Sub

    Sub SaveProfile()
        TempProfile.DefaultnameValue = Nothing
        Dim gap = TempProfile.DeepClone
        Dim name = InputBox.Show("Enter the profile name.", "Save Profile", gap.Name)

        If name.NotNullOrEmptyS Then
            gap.Name = name
            s.AudioProfiles.Add(gap)
            MsgInfo("The profile was saved.")
        End If
    End Sub

    Sub LoadProfile(gap As GUIAudioProfile)
        gap.DefaultnameValue = Nothing
        Profile = gap
        TempProfile = gap.DeepClone
        LoadProfile()
    End Sub

    Sub LoadProfile()
        If TempProfile.Name <> TempProfile.DefaultName Then
            tbProfileName.Text = TempProfile.Name
        End If

        tbProfileName.SendMessageCue(TempProfile.Name, False)

        tbCustom.Text = TempProfile.Params.CustomSwitches
        tbStreamName.Text = TempProfile.StreamName

        cbDefaultTrack.Checked = TempProfile.Default
        cbForcedTrack.Checked = TempProfile.Forced
        cbNormalize.Checked = TempProfile.Params.Normalize

        mbCodec.Value = TempProfile.Params.Codec
        mbChannels.Value = TempProfile.Params.ChannelsMode
        mbLanguage.Value = TempProfile.Language
        mbSamplingRate.Value = TempProfile.Params.SamplingRate
        mbEncoder.Value = TempProfile.Params.Encoder
        mbDecoder.Value = TempProfile.Decoder

        SetQuality(TempProfile.Params.Quality)

        numBitrate.Value = TempProfile.Bitrate
        numDelay.Value = TempProfile.Delay
        numGain.Value = TempProfile.Gain

        LoadAdvanced()
        UpdateControlsA()
    End Sub

    Sub SetBitrate(v As Integer)
        numBitrate.Value = v
    End Sub

    Sub LoadAdvanced()
        RemoveHandler SimpleUI.ValueChanged, AddressOf SimpleUIValueChanged

        If MbCompressionLevel IsNot Nothing Then
            RemoveHandler SimpleUI.SaveValues, AddressOf NumFFLFEMixLevel.NumEdit.Save
            NumFFLFEMixLevel.NumEdit.Dispose()
            NumFFLFEMixLevel.Dispose()
        End If
        If CbQaacHE IsNot Nothing Then
            RemoveHandler SimpleUI.SaveValues, AddressOf CbQaacHE.Save
            RemoveHandler CbQaacHE.CheckedChanged, CbQaacHEH
            'CbQaacHE.Controls.Clear()
            CbQaacHE.Dispose()
        End If
        If MbMode IsNot Nothing Then
            RemoveHandler SimpleUI.SaveValues, AddressOf MbMode.Button.Save
            RemoveHandler MbMode.Button.ValueChangedUser, MbModeH
            MbMode.Button.Dispose()
            MbMode.Dispose()
        End If
        If MbCompressionLevel IsNot Nothing Then
            RemoveHandler SimpleUI.SaveValues, AddressOf MbCompressionLevel.NumEdit.Save
            RemoveHandler MbCompressionLevel.NumEdit.ValueChanged, MbCompressionLevelH
            MbCompressionLevel.NumEdit.Dispose()
            MbCompressionLevel.Dispose()
        End If
        If MbWavPackMode IsNot Nothing Then
            RemoveHandler SimpleUI.SaveValues, AddressOf MbWavPackMode.Button.Save
            RemoveHandler MbWavPackMode.Button.ValueChangedUser, MbWavPackModeH
            MbWavPackMode.Button.Dispose()
            MbWavPackMode.Dispose()
        End If

        Dim ui = SimpleUI
        ui.Store = TempProfile.Params

        'For Each ct In ui.Host.Controls
        '    DirectCast(ct, Control).Dispose()
        '    ct = Nothing
        'Next
        ui.Host.Controls.Clear()

        If ui.ActivePage IsNot Nothing Then
            DirectCast(ui.ActivePage, Control).Dispose()
        End If

        For Each pg In ui.Pages
            DirectCast(pg, SimpleUI.FlowPage).Dispose()
        Next
        ui.Pages.Clear()

        Dim page = ui.CreateFlowPage()
        '   Text = ui.Pages.Count.ToString & "pc | hc " & ui.Host.Controls.Count

        page.SuspendLayout()

        If TempProfile.Params.Encoder <> GuiAudioEncoder.eac3to Then
            NumFFLFEMixLevel = ui.AddNum(page)
            NumFFLFEMixLevel.Text = "FF LFE Downmix"
            NumFFLFEMixLevel.NumEdit.Config = {-31, 31, 0.1, 3}
            NumFFLFEMixLevel.Help = "Value 1.0 sets LFE matrix coef. equal to other channels,  ffmpeg default 0 means no LFE in downmix"
            NumFFLFEMixLevel.NumEdit.Value = TempProfile.Params.ffmpegLFEMixLevel
            NumFFLFEMixLevel.NumEdit.SaveAction = Sub(value) TempProfile.Params.ffmpegLFEMixLevel = Math.Round(value, 3)
        End If

        If TempProfile.IntegerCodec() Then
            Dim mDepth = ui.AddMenu(Of Integer)
            mDepth.Text = "Depth:"
            mDepth.Expandet = True
            mDepth.Add("Default", 0)
            mDepth.Add("16", 16)
            mDepth.Add("24", 24)
            If TempProfile.Params.Codec <> AudioCodec.FLAC Then
                mDepth.Add("32 FP", 32)
            End If
            mDepth.Button.Value = TempProfile.Depth
            mDepth.Button.SaveAction = Sub(val)
                                           If TempProfile.IntegerCodec() Then TempProfile.Depth = val
                                           UpdateBitrate()
                                           UpdateControls()
                                       End Sub
        End If

        Dim cb As SimpleUI.SimpleUICheckBox

        Select Case TempProfile.GetEncoder
            Case GuiAudioEncoder.eac3to
                Dim mbFrameRateMode = ui.AddMenu(Of AudioFrameRateMode)(page)
                mbFrameRateMode.Label.Text = "Frame rate:"
                mbFrameRateMode.Button.Expand = True
                mbFrameRateMode.Button.Value = TempProfile.Params.FrameRateMode
                mbFrameRateMode.Button.SaveAction = Sub(value) TempProfile.Params.FrameRateMode = value

                Dim mbStereoDownmix = ui.AddMenu(Of Integer)(page)
                mbStereoDownmix.Label.Text = "Stereo Downmix:"
                mbStereoDownmix.Button.Expand = True
                mbStereoDownmix.Button.Add("Simple", 0)
                mbStereoDownmix.Button.Add("DPL II", 1)
                mbStereoDownmix.Button.Value = TempProfile.Params.eac3toStereoDownmixMode
                mbStereoDownmix.Button.SaveAction = Sub(value) TempProfile.Params.eac3toStereoDownmixMode = value

                If TempProfile.Params.Codec = AudioCodec.DTS AndAlso (TempProfile.File.NullOrEmptyS OrElse TempProfile.File.ToLower.Contains("dts") OrElse
                    (TempProfile.Stream IsNot Nothing AndAlso TempProfile.Stream.Name.Contains("DTS"))) Then
                    cb = ui.AddBool(page)
                    cb.Text = "Extract DTS core"
                    cb.Checked = TempProfile.ExtractDTSCore
                    cb.SaveAction = Sub(value)
                                        TempProfile.ExtractDTSCore = value
                                        UpdateControls()
                                    End Sub
                End If
            Case GuiAudioEncoder.ffmpeg
                Select Case TempProfile.Params.Codec

                    Case AudioCodec.FLAC, AudioCodec.WavPack
                        MbCompressionLevel = ui.AddNum(page)
                        MbCompressionLevel.Text = "Compression Level"
                        If TempProfile.Params.Codec = AudioCodec.FLAC Then
                            MbCompressionLevel.NumEdit.Config = {0, 12}
                            MbCompressionLevel.Help = "Over 10 is non-subset"
                            MbCompressionLevelH = Sub()
                                                      If MbCompressionLevel.NumEdit.Value > 10 Then
                                                          MbCompressionLevel.NumEdit.SetColor(Color.Red)
                                                      Else
                                                          MbCompressionLevel.NumEdit.SetColor(Color.CadetBlue)
                                                      End If
                                                  End Sub
                            AddHandler MbCompressionLevel.NumEdit.ValueChanged, MbCompressionLevelH
                        Else
                            MbCompressionLevel.NumEdit.Config = {0, 8}
                            TempProfile.Params.WavPackMode = 0
                        End If
                        MbCompressionLevel.NumEdit.Value = TempProfile.Params.ffmpegCompressionLevel
                        MbCompressionLevel.NumEdit.SaveAction = Sub(value) TempProfile.Params.ffmpegCompressionLevel = CInt(value)

                    Case AudioCodec.AAC
                        Dim mbRateMode = ui.AddMenu(Of SimpleAudioRateMode)
                        mbRateMode.Text = "Rate Mode"
                        mbRateMode.Expandet = True
                        mbRateMode.Button.Value = TempProfile.Params.SimpleRateMode
                        mbRateMode.Button.SaveAction =
                            Sub(value)
                                If TempProfile.Params.Encoder = GuiAudioEncoder.ffmpeg AndAlso TempProfile.Params.Codec = AudioCodec.AAC Then
                                    TempProfile.Params.SimpleRateMode = value
                                    UpdateBitrate()
                                End If
                            End Sub

                        cb = ui.AddBool
                        cb.Text = "Use fdk-aac"
                        cb.Property = NameOf(TempProfile.Params.ffmpegLibFdkAAC)
                    Case AudioCodec.Opus
                        Dim mbRateMode = ui.AddMenu(Of OpusRateMode)
                        mbRateMode.Text = "Rate Mode"
                        mbRateMode.Expandet = True
                        mbRateMode.Button.Value = TempProfile.Params.ffmpegOpusRateMode
                        mbRateMode.Button.SaveAction =
                              Sub(value)
                                  If TempProfile.Params.Encoder = GuiAudioEncoder.ffmpeg AndAlso TempProfile.Params.Codec = AudioCodec.Opus Then
                                      TempProfile.Params.ffmpegOpusRateMode = value
                                      'TempProfile.Params.RateMode = If(TempProfile.Params.ffmpegOpusRateMode = OpusRateMode.VBR, AudioRateMode.VBR, AudioRateMode.CBR)
                                  End If
                              End Sub

                        'Dim mbOpusApp = ui.AddMenu(Of OpusApp)
                        'mbOpusApp.Text = "Application Type"
                        'mbOpusApp.Expandet = True
                        'mbOpusApp.Button.Value = TempProfile.Params.ffmpegOpusApp
                        'mbOpusApp.Button.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusApp = value

                        Dim frame = ui.AddMenu(Of Double)
                        frame.Text = "Frame Duration"
                        frame.Expandet = True
                        frame.Add("2.5 ms", 2.5)
                        frame.Add("5 ms", 5)
                        frame.Add("10 ms", 10)
                        frame.Add("20 ms", 20)
                        frame.Add("40 ms", 40)
                        frame.Add("60 ms", 60)
                        frame.Property = NameOf(TempProfile.Params.ffmpegOpusFrame)

                        Dim mMappingFamily = ui.AddMenu(Of Integer)
                        mMappingFamily.Text = "MappingFamily"
                        mMappingFamily.Expandet = True
                        mMappingFamily.Add("No surr.masking and LFE opt.", -1)
                        mMappingFamily.Add("Mono/Stereo 2 channels", 0)
                        mMappingFamily.Add("Masking and LFE opt.8Ch max", 1)
                        mMappingFamily.Add("Ambisonics as individual ch.", 2)
                        mMappingFamily.Add("Ambisonics with demixing", 3)
                        mMappingFamily.Add("Discrete channels 255Ch max", 255)
                        ui.AddLabel("Mapping Family 1 is the best for multichannel,")
                        ui.AddLabel("however in FFmpeg this may fail. Forcing channels can help")
                        ui.AddLabel("Using OpusEnc is recommended                                          ")
                        'mMappingFamily.Help = "https://ffmpeg.org/ffmpeg-codecs.html#Option-Mapping"
                        mMappingFamily.Help = "https://tools.ietf.org/html/draft-ietf-codec-ambisonics-10#section-8"
                        mMappingFamily.Property = NameOf(TempProfile.Params.ffmpegOpusMap)

                        'Dim comp = ui.AddNum(page) 'Is this really needed?
                        'comp.Text = "Compression Level"
                        'comp.Config = {0, 10, 1}
                        'comp.NumEdit.Value = TempProfile.Params.ffmpegOpusCompress
                        'comp.NumEdit.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusCompress = CInt(value)

                        'Dim packet = ui.AddNum(page) 'Is this really needed?
                        'packet.Text = "Packet Loss"
                        'packet.Config = {0, 100, 1}
                        'packet.NumEdit.Value = TempProfile.Params.ffmpegOpusPacket
                        'packet.NumEdit.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusPacket = CInt(value)

                        cb = ui.AddBool(page)
                        cb.Text = "No phase inversion for intensity stereo"
                        cb.Property = NameOf(TempProfile.Params.opusEncNoPhaseInv)

                    Case AudioCodec.MP3, AudioCodec.Vorbis
                        Dim mbRateMode = ui.AddMenu(Of AudioRateMode)
                        mbRateMode.Text = "Rate Mode:"
                        mbRateMode.Expandet = True
                        mbRateMode.Button.Value = TempProfile.Params.RateMode
                        mbRateMode.Button.SaveAction = Sub(value)
                                                           If {AudioCodec.MP3, AudioCodec.Vorbis}.ContainsF(TempProfile.Params.Codec) Then
                                                               TempProfile.Params.RateMode = value
                                                               UpdateBitrate()
                                                           End If
                                                       End Sub
                        Dim num = ui.AddNum(page)
                        num.Text = "Lowpass"
                        If TempProfile.Params.Codec = AudioCodec.Vorbis Then
                            num.Config = {0, 96000, 100}
                        Else
                            num.Config = {0, 48000, 100}
                        End If
                        num.Property = NameOf(TempProfile.Params.ffmpegMp3Cutoff)

                    Case AudioCodec.AC3, AudioCodec.EAC3
                    Case AudioCodec.DTS
                        If TempProfile.File.NullOrEmptyS OrElse TempProfile.File.ToLower.Contains("dts") OrElse
                            (TempProfile.Stream IsNot Nothing AndAlso TempProfile.Stream.Name.Contains("DTS")) Then
                            cb = ui.AddBool(page)
                            cb.Text = "Extract DTS core"
                            cb.Checked = TempProfile.ExtractDTSCore
                            cb.SaveAction = Sub(value)
                                                TempProfile.ExtractDTSCore = value
                                                UpdateControls()
                                            End Sub
                        End If
                End Select

            Case GuiAudioEncoder.fdkaac
                Dim getHelpAction = Function(switch As String) Sub() g.ShowCommandLineHelp(Package.fdkaac, switch)

                Dim modeMenu = ui.AddMenu(Of SimpleAudioRateMode)
                modeMenu.Text = "Rate Mode"
                modeMenu.Expandet = True
                modeMenu.HelpAction = getHelpAction("--bitrate-mode")
                modeMenu.Button.Value = TempProfile.Params.SimpleRateMode
                modeMenu.Button.SaveAction = Sub(value)
                                                 If TempProfile.Params.Codec = AudioCodec.AAC AndAlso TempProfile.Params.Encoder = GuiAudioEncoder.fdkaac Then
                                                     TempProfile.Params.SimpleRateMode = value
                                                     UpdateBitrate()
                                                 End If
                                             End Sub

                Dim profileMenu = ui.AddMenu(Of Integer)
                profileMenu.Text = "Profile"
                profileMenu.Expandet = True
                profileMenu.HelpAction = getHelpAction("--profile")
                profileMenu.Add("AAC LC", 2)
                profileMenu.Add("HE-AAC SBR", 5)
                profileMenu.Add("HE-AAC SBR+PS", 29)
                profileMenu.Add("AAC LD", 23)
                profileMenu.Add("AAC ELD", 39)
                profileMenu.Property = NameOf(TempProfile.Params.fdkaacProfile)

                'Dim lowDelaySBR = ui.AddMenu(Of Integer)
                'lowDelaySBR.Text = "Lowdelay SBR"
                'lowDelaySBR.Expandet = True
                'lowDelaySBR.HelpAction = getHelpAction("--lowdelay-sbr")
                'lowDelaySBR.Add("ELD SBR auto configuration", -1)
                'lowDelaySBR.Add("Disable SBR on ELD", 0)
                'lowDelaySBR.Add("Enable SBR on ELD", 1)
                'lowDelaySBR.Property = NameOf(TempProfile.Params.fdkaacLowDelaySBR)

                Dim sbrRatio = ui.AddMenu(Of Integer)
                sbrRatio.Text = "SBR Ratio"
                sbrRatio.Expandet = True
                sbrRatio.HelpAction = getHelpAction("--sbr-ratio")
                sbrRatio.Add("Library Default", 0)
                sbrRatio.Add("Downsampled SBR (ELD+SBR default)", 1)
                sbrRatio.Add("Dual-rate SBR (HE-AAC default)", 2)
                sbrRatio.Property = NameOf(TempProfile.Params.fdkaacSbrRatio)

                'Dim gaplessMode = ui.AddMenu(Of Integer)
                'gaplessMode.Text = "Gapless Mode"
                'gaplessMode.Expandet = True
                'gaplessMode.HelpAction = getHelpAction("--gapless-mode")
                'gaplessMode.Add("iTunSMPB", 0)
                'gaplessMode.Add("ISO Standard (EDTS And SGPD)", 1)
                'gaplessMode.Add("Both", 2)
                'gaplessMode.Property = NameOf(TempProfile.Params.fdkaacGaplessMode)

                Dim transportFormat = ui.AddMenu(Of Integer)
                transportFormat.Text = "Transport Format"
                transportFormat.Expandet = True
                transportFormat.HelpAction = getHelpAction("--transport-format")
                transportFormat.Add("M4A", 0)
                transportFormat.Add("ADIF", 1)
                transportFormat.Add("ADTS", 2)
                transportFormat.Add("LATM MCP=1", 6)
                transportFormat.Add("LATM MCP=0", 7)
                transportFormat.Add("LOAS/LATM (LATM within LOAS)", 10)
                transportFormat.Property = NameOf(TempProfile.Params.fdkaacTransportFormat)

                Dim n = ui.AddNum
                n.Text = "Bandwidth"
                n.HelpAction = getHelpAction("--bandwidth")
                n.Config = {Integer.MinValue, Integer.MaxValue}
                n.Property = NameOf(TempProfile.Params.fdkaacBandwidth)

                cb = ui.AddBool
                cb.Text = "Afterburner"
                cb.HelpAction = getHelpAction("--afterburner")
                cb.Property = NameOf(TempProfile.Params.fdkaacAfterburner)

                cb = ui.AddBool
                cb.Text = "Add CRC Check on ADTS header"
                cb.HelpAction = getHelpAction("--adts-crc-check")
                cb.Property = NameOf(TempProfile.Params.fdkaacAdtsCrcCheck)

                'cb = ui.AddBool
                'cb.Text = "Header Period"
                'cb.HelpAction = getHelpAction("--header-period")
                'cb.Property = NameOf(TempProfile.Params.fdkaacHeaderPeriod)

                'cb = ui.AddBool
                'cb.Text = "Include SBR Delay"
                'cb.HelpAction = getHelpAction("--include-sbr-delay")
                'cb.Property = NameOf(TempProfile.Params.fdkaacIncludeSbrDelay)

                cb = ui.AddBool
                cb.Text = "Place moov box before mdat box"
                cb.HelpAction = getHelpAction("--moov-before-mdat")
                cb.Property = NameOf(TempProfile.Params.fdkaacMoovBeforeMdat)
            Case GuiAudioEncoder.qaac
                Dim getHelpAction = Function(switch As String) Sub() g.ShowCommandLineHelp(Package.qaac, switch)
                MbMode = ui.AddMenu(Of Integer)
                MbMode.Text = "Mode"
                MbMode.Expandet = True
                MbMode.Add("True VBR", 0)
                MbMode.Add("Constrained VBR", 1)
                MbMode.Add("ABR", 2)
                MbMode.Add("CBR", 3)
                MbMode.Help = "https://github.com/nu774/qaac/wiki/Encoder-configuration#tvbr-quality-steps"
                MbMode.Button.Value = TempProfile.Params.qaacRateMode
                MbMode.Button.SaveAction = Sub(value)
                                               If TempProfile.Params.Codec = AudioCodec.AAC AndAlso TempProfile.GetEncoder() = GuiAudioEncoder.qaac Then
                                                   TempProfile.Params.qaacRateMode = value
                                                   'TempProfile.Params.RateMode = If(TempProfile.Params.qaacRateMode = 0, AudioRateMode.VBR, AudioRateMode.CBR)
                                                   UpdateBitrate()
                                               End If
                                           End Sub

                'Dim mbQuality = ui.AddMenu(Of Integer)(page)  'Is this really needed?
                'mbQuality.Label.Text = "Quality"
                'mbQuality.Button.Expand = True
                'mbQuality.Button.Add("Low", 0)
                'mbQuality.Button.Add("Medium", 1)
                'mbQuality.Button.Add("High", 2)
                'mbQuality.Help = "https://github.com/nu774/qaac/wiki/Encoder-configuration#selecting-output-mode"
                'mbQuality.Button.Value = TempProfile.Params.qaacQuality
                'mbQuality.Button.SaveAction = Sub(value) TempProfile.Params.qaacQuality = value

                Dim num = ui.AddNum(page)
                num.Text = "Lowpass"
                num.Config = {0, 48000, 100}
                num.HelpAction = getHelpAction("--lowpass")
                num.NumEdit.Value = TempProfile.Params.qaacLowpass
                num.NumEdit.SaveAction = Sub(value) TempProfile.Params.qaacLowpass = CInt(value)

                CbQaacHE = ui.AddBool(page)
                CbQaacHE.Text = "High Efficiency"
                CbQaacHE.HelpAction = getHelpAction("--he")
                CbQaacHE.Property = NameOf(TempProfile.Params.qaacHE)

                CbQaacHEH = Sub(sender As Object, e As EventArgs)
                                If CbQaacHE.Checked AndAlso MbMode.Button.Value = 0 Then
                                    MbMode.Button.Value = 1
                                    TempProfile.Params.qaacRateMode = 1
                                    'TempProfile.Params.RateMode = AudioRateMode.CBR
                                    Dim c21 As Double = If(TempProfile.Channels > 0, TempProfile.Channels * 64 / 2, 160)
                                    SetBitrate(CInt(If(TempProfile.Channels > 2, TempProfile.Channels * 64 / 2 - 32, c21)))
                                End If
                            End Sub
                AddHandler CbQaacHE.CheckedChanged, CbQaacHEH

                MbModeH = Sub()
                              'CbQaacHE.Enabled = MbMode.Button.Value <> 0
                              If MbMode.Button.Value = 0 Then CbQaacHE.Checked = False
                              'UpdateControls()
                          End Sub
                AddHandler MbMode.Button.ValueChangedUser, MbModeH

                'AFAIK dither is intended only for wav/ALAC out and depth (-b) param - no effect in Staxrip: 
                'limiter is quite usefull btw
                cb = ui.AddBool(page)
                cb.Text = "Smart Soft Limiter"
                cb.Help = "https://github.com/nu774/qaac/wiki/Sample-format-or-bit-depth-conversion%2C-and-limiter"
                cb.Property = NameOf(TempProfile.Params.qaacNoDither)


            Case GuiAudioEncoder.WavPack
                Dim getHelpAction = Function(switch As String) Sub() g.ShowCommandLineHelp(Package.WavPack, switch)

                MbWavPackMode = ui.AddMenu(Of Integer)
                MbWavPackMode.Text = "Mode"
                MbWavPackMode.Expandet = True
                MbWavPackMode.Add("Lossless", 0)
                MbWavPackMode.Add("Lossy CBR", 1)
                MbWavPackMode.Button.Value = TempProfile.Params.WavPackMode
                MbWavPackMode.Button.SaveAction =
                    Sub(value)
                        If TempProfile.Params.Codec = AudioCodec.WavPack AndAlso TempProfile.GetEncoder() = GuiAudioEncoder.WavPack Then
                            TempProfile.Params.WavPackMode = value
                        End If
                    End Sub

                Dim mCompression = ui.AddMenu(Of Integer)
                mCompression.Text = "Comp/Decomp Mode"
                mCompression.Expandet = True
                mCompression.Add("Fast", 0)
                mCompression.Add("Normal", 1)
                mCompression.Add("High", 2)
                mCompression.Add("Very High", 3)
                mCompression.Property = NameOf(TempProfile.Params.WavPackCompression)


                Dim mExtraCompression = ui.AddNum(page)
                mExtraCompression.Text = "Extra Compression"
                mExtraCompression.Config = {0, 6}
                mExtraCompression.Property = NameOf(TempProfile.Params.WavPackExtraCompression)

                Dim mPreQuant = ui.AddNum(page)
                mPreQuant.Text = "Pre-Quantitize bits"
                mPreQuant.Config = {0, 24}
                mPreQuant.HelpAction = getHelpAction("--pre-quantize=bits")
                mPreQuant.NumEdit.Value = TempProfile.Params.WavPackPreQuant
                mPreQuant.NumEdit.SaveAction =
                    Sub(value)
                        If TempProfile.Params.Codec = AudioCodec.WavPack AndAlso TempProfile.GetEncoder() = GuiAudioEncoder.WavPack Then
                            TempProfile.Params.WavPackPreQuant = CInt(value)
                            UpdateBitrate()
                        End If
                    End Sub

                Dim cbCreateCorrectionWVC = ui.AddBool(page)
                cbCreateCorrectionWVC.Text = "Create correction file"
                cbCreateCorrectionWVC.HelpAction = getHelpAction("create correction file")
                cbCreateCorrectionWVC.Property = NameOf(TempProfile.Params.WavPackCreateCorrection)

                cbCreateCorrectionWVC.Enabled = MbWavPackMode.Button.Value = 1

                MbWavPackModeH = Sub()
                                     If MbWavPackMode.Button.Value = 1 Then
                                         TempProfile.Params.WavPackMode = 1
                                         SetBitrate(CInt(If(TempProfile.Channels = 2, TempProfile.Channels * 384 / 2, TempProfile.Channels * 320 / 2)))
                                         cbCreateCorrectionWVC.Enabled = True
                                     Else
                                         'cbCreateCorrectionWVC.Checked = False
                                         cbCreateCorrectionWVC.Enabled = False
                                         TempProfile.Params.WavPackMode = 0
                                         UpdateBitrate()
                                     End If
                                 End Sub
                AddHandler MbWavPackMode.Button.ValueChangedUser, MbWavPackModeH

            Case GuiAudioEncoder.OpusEnc
                Dim getHelpAction = Function(switch As String) Sub() g.ShowCommandLineHelp(Package.OpusEnc, switch)
                Dim mbRateMode = ui.AddMenu(Of OpusRateMode)
                mbRateMode.Text = "Mode"
                mbRateMode.Expandet = True
                mbRateMode.HelpAction = getHelpAction("--vbr")
                mbRateMode.Button.Value = TempProfile.Params.ffmpegOpusRateMode
                mbRateMode.Button.SaveAction =
                    Sub(value)
                        If TempProfile.Params.Codec = AudioCodec.Opus AndAlso TempProfile.GetEncoder() = GuiAudioEncoder.OpusEnc Then
                            TempProfile.Params.ffmpegOpusRateMode = value
                            'TempProfile.Params.RateMode = If(TempProfile.Params.ffmpegOpusRateMode = 0, AudioRateMode.VBR, AudioRateMode.CBR)
                        End If
                    End Sub

                'Dim mbOpusApp = ui.AddMenu(Of OpusEncTune)
                'mbOpusApp.Text = "Force low bitrate tune"
                'mbOpusApp.Expandet = True
                'mbOpusApp.HelpAction = getHelpAction("--music")
                'mbOpusApp.Button.Value = TempProfile.Params.opusEncTuning
                'mbOpusApp.Button.SaveAction = Sub(value) TempProfile.Params.opusEncTuning = value

                Dim frame = ui.AddMenu(Of Double)
                frame.Text = "Frame Duration"
                frame.Expandet = True
                frame.Add("2.5 ms", 2.5)
                frame.Add("5 ms", 5)
                frame.Add("10 ms", 10)
                frame.Add("20 ms", 20)
                frame.Add("40 ms", 40)
                frame.Add("60 ms", 60)
                frame.HelpAction = getHelpAction("--framesize")
                frame.Property = NameOf(TempProfile.Params.ffmpegOpusFrame)

                'Dim comp = ui.AddNum(page) 'Is this really needed?
                'comp.Text = "Compression Level"
                'comp.Config = {0, 10, 1}
                'comp.HelpAction = getHelpAction("--comp")
                'comp.NumEdit.Value = TempProfile.Params.ffmpegOpusCompress
                'comp.NumEdit.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusCompress = CInt(value)

                'Dim packet = ui.AddNum(page) 'Is this really needed?
                'packet.Text = "Packet Loss %"
                'packet.Config = {0, 100, 1}
                'packet.HelpAction = getHelpAction("--expect-loss")
                'packet.NumEdit.Value = TempProfile.Params.ffmpegOpusPacket
                'packet.NumEdit.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusPacket = CInt(value)

                'Dim NOpusDelay = ui.AddNum(page) 'Is this really needed?
                'NOpusDelay.Text = "Max container delay ms"
                'NOpusDelay.Config = {0, 1000}
                'NOpusDelay.HelpAction = getHelpAction("--max-delay")
                'NOpusDelay.NumEdit.Value = TempProfile.Params.opusEncDelay
                'NOpusDelay.NumEdit.SaveAction = Sub(value) TempProfile.Params.opusEncDelay = CInt(value)

                cb = ui.AddBool(page)
                cb.Text = "No phase inversion for intensity stereo"
                cb.HelpAction = getHelpAction("--no-phase-inv")
                cb.Property = NameOf(TempProfile.Params.opusEncNoPhaseInv)

                'Some options are propably not worthy exposing, but be my guest

        End Select

        page.ResumeLayout()
        AddHandler SimpleUI.ValueChanged, AddressOf SimpleUIValueChanged
        GC.Collect(2, GCCollectionMode.Optimized)
    End Sub

    Sub nudBitrate_KeyUp(sender As Object, e As KeyEventArgs) Handles numBitrate.KeyUp
        Try
            Dim v = CInt(numBitrate.Text)

            If v Mod 16 = 0 Then
                numBitrate.Value = v
            End If
        Catch
        End Try
    End Sub

    Sub nudQuality_KeyUp(sender As Object, e As KeyEventArgs) Handles numQuality.KeyUp
        Try
            Dim v = CInt(numQuality.Text)
            numQuality.Value = v
        Catch
        End Try
    End Sub

    Sub Execute()
        If TempProfile.File.NotNullOrEmptyS Then
            If Not TempProfile.IsInputSupported AndAlso Not TempProfile.DecodingMode = AudioDecodingMode.Pipe Then
                MsgWarn("The input format isn't supported by the current encoder," + BR + "convert to WAV or WV first or enable piping in the options.")
            Else
                Dim pr As New Process
                pr.StartInfo.FileName = "cmd.exe"
                pr.StartInfo.Arguments = "/S /K """ + TempProfile.GetCommandLine(True) + """"
                pr.StartInfo.UseShellExecute = False
                Proc.SetEnvironmentVariables(pr)
                pr.Start()
            End If
        Else
            MsgWarn("Source file is missing!")
        End If
    End Sub

    Sub tbStreamName_TextChanged(sender As Object, e As EventArgs) Handles tbStreamName.TextChanged
        TempProfile.StreamName = tbStreamName.Text
    End Sub

    Sub tbCustom_TextChanged(sender As Object, e As EventArgs) Handles tbCustom.TextChanged
        TempProfile.Params.CustomSwitches = tbCustom.Text
        UpdateControls()
    End Sub

    Sub cbDefaultTrack_CheckedChanged(sender As Object, e As EventArgs) Handles cbDefaultTrack.CheckedChanged
        TempProfile.Default = cbDefaultTrack.Checked
    End Sub

    Sub cbForcedTrack_CheckedChanged(sender As Object, e As EventArgs) Handles cbForcedTrack.CheckedChanged
        TempProfile.Forced = cbForcedTrack.Checked
    End Sub

    Sub mbEncoder_ValueChangedUser(value As Object) Handles mbEncoder.ValueChangedUser
        TempProfile.Params.Encoder = mbEncoder.GetValue(Of GuiAudioEncoder)()
        mbCodec_ValueChangedUser()
    End Sub

    Sub ShowHelp()
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteTips(TipProvider.GetTips, SimpleUI.ActivePage.TipProvider.GetTips)
        form.Show()
    End Sub

    Sub mbChannels_ValueChanged(value As Object) Handles mbChannels.ValueChangedUser
        TempProfile.Params.ChannelsMode = mbChannels.GetValue(Of ChannelsMode)()
        UpdateBitrate()
        UpdateControls()
    End Sub

    Sub mbConvertMode_ValueChangedUser(value As Object) Handles mbDecoder.ValueChangedUser
        TempProfile.Decoder = mbDecoder.GetValue(Of AudioDecoderMode)()
        UpdateControls()
    End Sub

    Sub bnAdvanced_Click(sender As Object, e As EventArgs) Handles bnAdvanced.Click
        Using form As New SimpleSettingsForm("Advanced Audio Options")
            form.ScaleClientSize(30, 21)
            Dim ui = form.SimpleUI
            ui.Store = TempProfile.Params

            ui.CreateFlowPage("General", True)

            Dim convFormat = ui.AddMenu(Of AudioDecodingMode)
            convFormat.Text = "Decoding Method:"
            convFormat.Button.Value = TempProfile.DecodingMode
            convFormat.Button.SaveAction = Sub(val) TempProfile.DecodingMode = val

            Dim pageFF = ui.CreateFlowPage("ffmpeg", True)
            pageFF.SuspendLayout()

            Dim ffmpegNormalize = ui.AddMenu(Of ffmpegNormalizeMode)
            ffmpegNormalize.Text = "Normalize Method:"
            ffmpegNormalize.Property = NameOf(TempProfile.Params.ffmpegNormalizeMode)

            Dim mb = ui.AddMenu(Of FFDither)
            mb.Text = "Dither type"
            mb.Property = NameOf(TempProfile.Params.ffmpegDither)

            Dim b = ui.AddBool()
            b.Text = "SOX VHQ resampler"
            b.Property = NameOf(TempProfile.Params.ffmpegResampSOX)

            Dim n = ui.AddNum()
            n.Text = "Probe Size"
            n.Config = {1, 999, 5}
            n.Property = NameOf(TempProfile.Params.ProbeSize)

            n = ui.AddNum()
            n.Text = "Analyze Duration"
            n.Config = {1, 999, 5}
            n.Property = NameOf(TempProfile.Params.AnalyzeDuration)

            pageFF.ResumeLayout(False)
            ui.CreateFlowPage("ffmpeg | loudnorm", True)

            ui.AddLabel("EBU R128 Loudness Normalization")

            Dim helpUrl = "https://www.ffmpeg.org/ffmpeg-filters.html#loudnorm"

            n = ui.AddNum()
            n.Text = "Integrated"
            n.Help = helpUrl
            n.Config = {-70.0, -5.0, 0.5, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegLoudnormIntegrated)

            n = ui.AddNum()
            n.Text = "True Peak"
            n.Help = helpUrl
            n.Config = {-9.0, 0, 0.2, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegLoudnormTruePeak)

            n = ui.AddNum()
            n.Text = "LRA"
            n.Help = helpUrl
            n.Config = {1, 20, 0.5, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegLoudnormLRA)

            Dim pageFFDN = ui.CreateFlowPage("ffmpeg | dynaudnorm", True)
            pageFFDN.SuspendLayout()
            ui.AddLabel("Dynamic Audio Normalizer")

            helpUrl = "https://www.ffmpeg.org/ffmpeg-filters.html#dynaudnorm"

            n = ui.AddNum()
            n.Text = "Frame Length"
            n.Help = helpUrl
            n.Config = {10, 8000, 10}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormF)

            n = ui.AddNum()
            n.Text = "Gaus filter win size"
            n.Help = helpUrl
            n.Config = {3, 301}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormG)

            n = ui.AddNum()
            n.Text = "Target Peak"
            n.Help = helpUrl
            n.Config = {0, 1, 0.05, 2}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormP)

            n = ui.AddNum()
            n.Text = "Max gain factor"
            n.Help = helpUrl
            n.Config = {1, 100, 1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormM)

            n = ui.AddNum()
            n.Text = "Target RMS"
            n.Help = helpUrl
            n.Config = {0, 1, 0.1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormR)

            n = ui.AddNum()
            n.Text = "Compress factor"
            n.Help = helpUrl
            n.Config = {0, 30, 1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormS)

            b = ui.AddBool
            b.Text = "Enable channels coupling"
            b.Help = helpUrl
            b.Property = NameOf(TempProfile.Params.ffmpegDynaudnormN)

            b = ui.AddBool
            b.Text = "Enable DC bias correction"
            b.Help = helpUrl
            b.Property = NameOf(TempProfile.Params.ffmpegDynaudnormC)

            b = ui.AddBool
            b.Text = "Enable alternative boundary mode"
            b.Help = helpUrl
            b.Property = NameOf(TempProfile.Params.ffmpegDynaudnormB)

            ui.SelectLast("last advanced audio options page")
            pageFFDN.ResumeLayout(False)

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()
            End If

            pageFF.Dispose()
            pageFFDN.Dispose()
            LoadAdvanced()
            UpdateControls()
            ui.SaveLast("last advanced audio options page")
        End Using
    End Sub

    Sub cbNormalize_CheckedChanged(sender As Object, e As EventArgs) Handles cbNormalize.CheckedChanged
        TempProfile.Params.Normalize = cbNormalize.Checked
        UpdateControls()
    End Sub
End Class
