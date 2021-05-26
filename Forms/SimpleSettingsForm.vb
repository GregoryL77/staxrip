
Imports StaxRip.UI

Public Class SimpleSettingsForm
    Inherits DialogBase

#Region " Designer "

    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents LineControl As StaxRip.UI.LineControl
    Private components As System.ComponentModel.IContainer
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents flpButtons As FlowLayoutPanel
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.LineControl = New StaxRip.UI.LineControl()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpMain.SuspendLayout()
        Me.flpButtons.SuspendLayout()
        Me.SuspendLayout()
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
        'SimpleUI
        '
        Me.SimpleUI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleUI.FormSizeScaleFactor = New System.Drawing.SizeF(0!, 0!)
        Me.SimpleUI.Location = New System.Drawing.Point(5, 5)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(387, 223)
        Me.SimpleUI.TabIndex = 2
        '
        'LineControl
        '
        Me.LineControl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl.Location = New System.Drawing.Point(5, 228)
        Me.LineControl.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.LineControl.Name = "LineControl"
        Me.LineControl.Size = New System.Drawing.Size(387, 10)
        Me.LineControl.TabIndex = 5
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.SimpleUI, 0, 0)
        Me.tlpMain.Controls.Add(Me.LineControl, 0, 1)
        Me.tlpMain.Controls.Add(Me.flpButtons, 0, 2)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(397, 266)
        Me.tlpMain.TabIndex = 8
        '
        'flpButtons
        '
        Me.flpButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.flpButtons.AutoSize = True
        Me.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpButtons.Controls.Add(Me.bnOK)
        Me.flpButtons.Controls.Add(Me.bnCancel)
        Me.flpButtons.Location = New System.Drawing.Point(221, 238)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(0, 0, 5, 5)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(171, 23)
        Me.flpButtons.TabIndex = 6
        '
        'SimpleSettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(397, 266)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Name = "SimpleSettingsForm"
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.flpButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private HelpParagraphs As String()

    Sub New(title As String, ParamArray helpParagraphs As String())
        InitializeComponent()
        ScaleClientSize(40, 27, FontHeight)
        Text = title
        Me.HelpParagraphs = helpParagraphs
        SimpleUI.Tree.Select()
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        Dim form As New HelpForm()
        hevent.Handled = True
        form.Doc.WriteStart(Text)

        If Not HelpParagraphs Is Nothing Then
            For Each i As String In HelpParagraphs
                form.Doc.WriteParagraph(i)
            Next
        End If

        If Not SimpleUI.ActivePage.TipProvider Is Nothing Then
            form.Doc.WriteTips(SimpleUI.ActivePage.TipProvider.GetTips)
        End If

        form.Show()
        MyBase.OnHelpRequested(hevent)
    End Sub
End Class
