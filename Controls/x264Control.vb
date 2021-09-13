
Imports StaxRip.UI

Public Class x264Control
    Inherits UserControl

#Region " Designer "
    <DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents lv As StaxRip.UI.ListViewEx
    Friend WithEvents blConfigCodec As ButtonLabel
    Friend WithEvents llConfigContainer As ButtonLabel
    Friend WithEvents llCompCheck As ButtonLabel

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.blConfigCodec = New StaxRip.UI.ButtonLabel()
        Me.llConfigContainer = New StaxRip.UI.ButtonLabel()
        Me.llCompCheck = New StaxRip.UI.ButtonLabel()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'blConfigCodec
        '
        Me.blConfigCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.blConfigCodec.AutoSize = True
        Me.blConfigCodec.BackColor = System.Drawing.SystemColors.Window
        Me.blConfigCodec.ForeColor = System.Drawing.Color.DimGray
        Me.blConfigCodec.LinkColor = System.Drawing.Color.DimGray
        Me.blConfigCodec.Location = New System.Drawing.Point(3, 223)
        Me.blConfigCodec.Margin = New System.Windows.Forms.Padding(3)
        Me.blConfigCodec.Name = "blConfigCodec"
        Me.blConfigCodec.Size = New System.Drawing.Size(128, 37)
        Me.blConfigCodec.TabIndex = 1
        Me.blConfigCodec.TabStop = True
        Me.blConfigCodec.Text = "Options"
        '
        'llConfigContainer
        '
        Me.llConfigContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llConfigContainer.AutoSize = True
        Me.llConfigContainer.BackColor = System.Drawing.SystemColors.Window
        Me.llConfigContainer.ForeColor = System.Drawing.Color.DimGray
        Me.llConfigContainer.LinkColor = System.Drawing.Color.DimGray
        Me.llConfigContainer.Location = New System.Drawing.Point(280, 223)
        Me.llConfigContainer.Margin = New System.Windows.Forms.Padding(3)
        Me.llConfigContainer.Name = "llConfigContainer"
        Me.llConfigContainer.Size = New System.Drawing.Size(276, 37)
        Me.llConfigContainer.TabIndex = 2
        Me.llConfigContainer.TabStop = True
        Me.llConfigContainer.Text = "Container Options"
        '
        'llCompCheck
        '
        Me.llCompCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llCompCheck.AutoSize = True
        Me.llCompCheck.BackColor = System.Drawing.SystemColors.Window
        Me.llCompCheck.ForeColor = System.Drawing.Color.DimGray
        Me.llCompCheck.LinkColor = System.Drawing.Color.DimGray
        Me.llCompCheck.Location = New System.Drawing.Point(3, 180)
        Me.llCompCheck.Margin = New System.Windows.Forms.Padding(3)
        Me.llCompCheck.Name = "llCompCheck"
        Me.llCompCheck.Size = New System.Drawing.Size(399, 37)
        Me.llCompCheck.TabIndex = 3
        Me.llCompCheck.TabStop = True
        Me.llCompCheck.Text = "Run Compressibility Check"
        '
        'lv
        '
        Me.lv.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lv.HideSelection = False
        Me.lv.Location = New System.Drawing.Point(0, 0)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(559, 263)
        Me.lv.TabIndex = 0
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'x264Control
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.llConfigContainer)
        Me.Controls.Add(Me.blConfigCodec)
        Me.Controls.Add(Me.llCompCheck)
        Me.Controls.Add(Me.lv)
        Me.Name = "x264Control"
        Me.Size = New System.Drawing.Size(559, 263)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private ReadOnly Encoder As x264Enc
    Private ReadOnly Params As x264Params

    Private ReadOnly cms As ContextMenuStripEx
    Private ReadOnly QualityDefinitions As (Value As Single, Text As String, Tooltip As String)()

    Sub New(enc As x264Enc)
        InitializeComponent()
        components = New ComponentModel.Container()

        QualityDefinitions = {
          (14, "Super High", "Super high quality and file size)"),
          (16, "Very High", "Very high quality and file size)"),
          (18, "Higher", "Higher quality and file size)"),
          (20, "High", "High quality and file size)"),
          (22, "Medium", "Medium quality and file size)"),
          (24, "Low", "Low quality and file size)"),
          (26, "Lower", "Lower quality and file size)"),
          (28, "Very Low", "Very low quality and file size)"),
          (30, "Super Low", "Super low quality and file size)")}

        Encoder = enc
        Params = Encoder.Params

        cms = New ContextMenuStripEx(components)
        If s.UIScaleFactor <> 1 Then cms.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

        lv.BeginUpdate()
        lv.View = View.Details
        lv.HeaderStyle = ColumnHeaderStyle.None
        lv.FullRowSelect = True
        lv.MultiSelect = False
        lv.ContextMenuStrip = cms
        lv.ShowContextMenuOnLeftClick = True

        UpdateControls()
        AddHandler lv.UpdateContextMenu, AddressOf UpdateMenu
        lv.EndUpdate()
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)
        lv.BeginUpdate()
        If lv.Columns.Count = 0 Then
            lv.Columns.AddRange({New ColumnHeader, New ColumnHeader})
        End If

        Dim w As Integer = Width
        lv.Columns(0).Width = CInt(w * (32 / 100))
        lv.Columns(1).Width = CInt(w * (66 / 100))
        lv.EndUpdate()

        'couldn't get scaling to work trying everything
        Dim h As Integer = Height
        Dim blcch As Integer = blConfigCodec.Height
        blConfigCodec.SetBounds(5, h - blcch - 5, 0, 0, BoundsSpecified.Location)
        llCompCheck.SetBounds(5, h - blcch - llCompCheck.Height - 10, 0, 0, BoundsSpecified.Location)
        llConfigContainer.SetBounds(w - llConfigContainer.Width - 5, h - llConfigContainer.Height - 5, 0, 0, BoundsSpecified.Location)
    End Sub

    Sub UpdateMenu()
        cms.SuspendLayout()
        cms.Items.ClearAndDispose
        Dim offset = If(Params.Mode.Value = x264RateMode.Quality, 0, 1)

        If lv.SelectedItems.Count > 0 Then
            Dim ff As FontFamily = Font.FontFamily
            Dim fN As New Font(ff, 9 * s.UIScaleFactor)
            Dim fB As New Font(ff, 9 * s.UIScaleFactor, FontStyle.Bold)
            Select Case lv.SelectedIndices(0)
                Case 0 - offset
                    Dim pqV As Double = Params.Quant.Value
                    For n = 0 To QualityDefinitions.Length - 1
                        Dim qd = QualityDefinitions(n)
                        Dim qdV As Single = qd.Value
                        cms.Items.Add(New ActionMenuItem(qdV & " - " & qd.Text & "      ", Sub() SetQuality(qdV), qd.Tooltip) With {.Font = If(pqV = qdV, fB, fN)})
                    Next n
                Case 1 - offset
                    Dim pP As CommandLine.OptionParam = Params.Preset
                    Dim ppV As Integer = pP.Value
                    For x = 0 To pP.Options.Length - 1
                        Dim i = x
                        cms.Items.Add(New ActionMenuItem(pP.Options(i) & "      ", Sub() SetPreset(i), "x264 slower compares to x265 medium") With {.Font = If(ppV = i, fB, fN)})
                    Next
                Case 2 - offset
                    Dim pT As CommandLine.OptionParam = Params.Tune
                    Dim ptV As Integer = pT.Value
                    For x = 0 To pT.Options.Length - 1
                        Dim i As Integer = x
                        cms.Items.Add(New ActionMenuItem(pT.Options(i) & "      ", Sub() SetTune(i)) With {.Font = If(ptV = i, fB, fN)})
                    Next
            End Select
        End If
        cms.ResumeLayout()
    End Sub

    Sub SetQuality(v As Single)
        lv.BeginUpdate()
        Params.Quant.Value = v
        lv.Items(0).SubItems(1).Text = GetQualityCaption(v)
        lv.Items(0).Selected = False
        UpdateControls()
        lv.EndUpdate()
    End Sub

    Sub SetPreset(value As Integer)
        lv.BeginUpdate()
        Dim offset = If(Params.Mode.Value = x264RateMode.Quality, 0, 1)

        Params.Preset.Value = value
        Params.ApplyValues(True)
        Params.ApplyValues(False)

        lv.Items(1 - offset).SubItems(1).Text = value.ToString
        lv.Items(1 - offset).Selected = False

        UpdateControls()
        lv.EndUpdate()
    End Sub

    Sub SetTune(value As Integer)
        lv.BeginUpdate()
        Dim offset = If(Params.Mode.Value = x264RateMode.Quality, 0, 1)

        Params.Tune.Value = value
        Params.ApplyValues(True)
        Params.ApplyValues(False)

        lv.Items(2 - offset).SubItems(1).Text = value.ToString
        lv.Items(2 - offset).Selected = False
        UpdateControls()
        lv.EndUpdate()
    End Sub

    Function GetQualityCaption(value As Double) As String
        For n = 0 To QualityDefinitions.Length - 1
            Dim qd = QualityDefinitions(n)
            If qd.Value = value Then
                Return value & " - " & qd.Text
            End If
        Next n

        Return value.ToString
    End Function

    Sub UpdateControls()
        Dim pMv As Integer = Params.Mode.Value
        If pMv = x264RateMode.Quality AndAlso lv.Items.Count < 4 Then
            lv.Items.Clear()
            lv.Items.AddRange({New ListViewItem({"Quality", GetQualityCaption(Params.Quant.Value)}),
            New ListViewItem({"Preset", Params.Preset.OptionText}),
            New ListViewItem({"Tune", Params.Tune.OptionText})})
        ElseIf pMv <> 2 AndAlso lv.Items.Count <> 3 Then
            lv.Items.Clear()
            lv.Items.AddRange({New ListViewItem({"Preset", Params.Preset.OptionText}),
            New ListViewItem({"Tune", Params.Tune.OptionText})})
        End If

        'Dim offset = If(pMv = x264RateMode.Quality, 0, 1) 'Dead code ?
        llCompCheck.Visible = pMv >= 3 '= x264RateMode.TwoPass Or pMv = x264RateMode.ThreePass
    End Sub

    Sub llConfigCodec_Click(sender As Object, e As EventArgs) Handles blConfigCodec.Click
        Encoder.ShowConfigDialog()
    End Sub

    Sub llConfigContainer_Click(sender As Object, e As EventArgs) Handles llConfigContainer.Click
        Encoder.OpenMuxerConfigDialog()
    End Sub

    Sub llCompCheck_Click(sender As Object, e As EventArgs) Handles llCompCheck.Click
        Encoder.RunCompCheck()
    End Sub

End Class
