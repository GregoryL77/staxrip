Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CodeEditor
    Inherits DialogBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub dispose(ByVal disposing As Boolean)
        Try
            RemoveHandler MainFlowLayoutPanel.Layout, AddressOf MainFlowLayoutPanelLayout
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.MainFlowLayoutPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.SuspendLayout()
        '
        'MainFlowLayoutPanel
        '
        Me.MainFlowLayoutPanel.AutoScrollMargin = New System.Drawing.Size(0, 24)
        Me.MainFlowLayoutPanel.AutoSize = True
        Me.MainFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.MainFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.MainFlowLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me.MainFlowLayoutPanel.Margin = New System.Windows.Forms.Padding(0, 0, 0, 24)
        Me.MainFlowLayoutPanel.Name = "MainFlowLayoutPanel"
        Me.MainFlowLayoutPanel.Padding = New System.Windows.Forms.Padding(6, 2, 3, 2)
        Me.MainFlowLayoutPanel.Size = New System.Drawing.Size(9, 4)
        Me.MainFlowLayoutPanel.TabIndex = 3
        Me.MainFlowLayoutPanel.WrapContents = False
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(395, 287)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(0, 0, 18, 0)
        Me.bnCancel.Size = New System.Drawing.Size(83, 23)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(296, 287)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Me.bnOK.Size = New System.Drawing.Size(83, 23)
        Me.bnOK.Text = "OK"
        '
        'CodeEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(496, 313)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.MainFlowLayoutPanel)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(288, 128)
        Me.Name = "CodeEditor"
        Me.Padding = New System.Windows.Forms.Padding(0, 0, 0, 3)
        Me.Text = "Code Editor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents MainFlowLayoutPanel As System.Windows.Forms.FlowLayoutPanel
End Class
'Me.MinimumSize = New System.Drawing.Size(288, 128)
'Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
'Me.KeyPreview = True