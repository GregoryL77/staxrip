Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CodeEditor
    Inherits DialogBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub dispose(ByVal disposing As Boolean)
        Try
            'RemoveHandler MainFlowLayoutPanel.Layout, AddressOf MainFlowLayoutPanelLayout
            'If MenuImageDict IsNot Nothing Then
            '    For Each img In MenuImageDict.Values
            '        img.Dispose()
            '    Next img
            '    MenuImageDict = Nothing
            'End If

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
        Me.MainFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.MainFlowLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me.MainFlowLayoutPanel.Margin = New System.Windows.Forms.Padding(0, 0, 0, 25)
        Me.MainFlowLayoutPanel.MinimumSize = New System.Drawing.Size(304, 60)
        Me.MainFlowLayoutPanel.Name = "MainFlowLayoutPanel"
        Me.MainFlowLayoutPanel.Padding = New System.Windows.Forms.Padding(4, 2, 2, 2)
        Me.MainFlowLayoutPanel.Size = New System.Drawing.Size(304, 60)
        Me.MainFlowLayoutPanel.TabIndex = 3
        Me.MainFlowLayoutPanel.WrapContents = False
        '
        'bnCancel
        '
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(211, 62)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.bnCancel.Size = New System.Drawing.Size(83, 23)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(112, 62)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(112, 0, 0, 0)
        Me.bnOK.Size = New System.Drawing.Size(83, 23)
        Me.bnOK.Text = "OK"
        '
        'CodeEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(305, 88)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.MainFlowLayoutPanel)
        Me.KeyPreview = True
        Me.Name = "CodeEditor"
        Me.Text = "Code Editor"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents MainFlowLayoutPanel As System.Windows.Forms.FlowLayoutPanel
End Class
'Me.MinimumSize = New System.Drawing.Size(321, 127) 'Overrided
'Me.Padding = New System.Windows.Forms.Padding(0, 0, 1, 3)

