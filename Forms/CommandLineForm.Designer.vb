Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CommandLineForm
    Inherits DialogBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub dispose(ByVal disposing As Boolean)
        Try
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
        Me.components = New System.ComponentModel.Container()
        Me.cbGoTo = New System.Windows.Forms.ComboBox()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.cms = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.rtbCommandLine = New StaxRip.UI.CommandLineRichTextBox()
        Me.cmsCommandLine = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.PanelBn = New System.Windows.Forms.Panel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelBn.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbGoTo
        '
        Me.cbGoTo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbGoTo.FormattingEnabled = True
        Me.cbGoTo.Location = New System.Drawing.Point(5, 6)
        Me.cbGoTo.Margin = New System.Windows.Forms.Padding(5, 6, 0, 6)
        Me.cbGoTo.Name = "cbGoTo"
        Me.cbGoTo.Size = New System.Drawing.Size(224, 23)
        Me.cbGoTo.TabIndex = 1
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnMenu.ContextMenuStrip = Me.cms
        Me.bnMenu.Location = New System.Drawing.Point(591, 5)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0, 5, 3, 5)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(36, 25)
        '
        'cms
        '
        Me.cms.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cms.ImageScalingSize = New System.Drawing.Size(18, 18)
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(61, 4)
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(633, 5)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.bnOK.Size = New System.Drawing.Size(83, 25)
        Me.bnOK.Text = "OK"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(722, 5)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(3, 5, 5, 5)
        Me.bnCancel.Size = New System.Drawing.Size(83, 25)
        Me.bnCancel.Text = "Cancel"
        '
        'SimpleUI
        '
        Me.SimpleUI.FormSizeScaleFactor = New System.Drawing.SizeF(0!, 0!)
        Me.SimpleUI.Location = New System.Drawing.Point(5, 5)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(5)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(800, 480)
        Me.SimpleUI.TabIndex = 2
        Me.SimpleUI.Text = "SimpleUI"
        '
        'rtbCommandLine
        '
        Me.rtbCommandLine.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbCommandLine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbCommandLine.ContextMenuStrip = Me.cmsCommandLine
        Me.rtbCommandLine.Font = New System.Drawing.Font("Consolas", 10.0!)
        Me.rtbCommandLine.Location = New System.Drawing.Point(5, 490)
        Me.rtbCommandLine.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.rtbCommandLine.Name = "rtbCommandLine"
        Me.rtbCommandLine.ReadOnly = True
        Me.rtbCommandLine.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.rtbCommandLine.Size = New System.Drawing.Size(800, 18)
        Me.rtbCommandLine.TabIndex = 3
        Me.rtbCommandLine.Text = ""
        '
        'cmsCommandLine
        '
        Me.cmsCommandLine.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cmsCommandLine.Name = "cmsCommandLine"
        Me.cmsCommandLine.Size = New System.Drawing.Size(61, 4)
        '
        'PanelBn
        '
        Me.PanelBn.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelBn.Controls.Add(Me.cbGoTo)
        Me.PanelBn.Controls.Add(Me.bnMenu)
        Me.PanelBn.Controls.Add(Me.bnOK)
        Me.PanelBn.Controls.Add(Me.bnCancel)
        Me.PanelBn.Location = New System.Drawing.Point(0, 508)
        Me.PanelBn.Margin = New System.Windows.Forms.Padding(0)
        Me.PanelBn.Name = "PanelBn"
        Me.PanelBn.Size = New System.Drawing.Size(810, 35)
        Me.PanelBn.TabIndex = 10
        '
        'tlpMain
        '
        Me.tlpMain.AutoSize = True
        Me.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.PanelBn, 0, 2)
        Me.tlpMain.Controls.Add(Me.rtbCommandLine, 0, 1)
        Me.tlpMain.Controls.Add(Me.SimpleUI, 0, 0)
        Me.tlpMain.Location = New System.Drawing.Point(1, 1)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.tlpMain.Size = New System.Drawing.Size(810, 543)
        Me.tlpMain.TabIndex = 11
        '
        'CommandLineForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(812, 545)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Name = "CommandLineForm"
        Me.Padding = New System.Windows.Forms.Padding(1)
        Me.PanelBn.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbGoTo As System.Windows.Forms.ComboBox
    Friend WithEvents bnMenu As StaxRip.UI.ButtonEx
    Friend WithEvents cms As ContextMenuStripEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents rtbCommandLine As CommandLineRichTextBox
    Friend WithEvents cmsCommandLine As ContextMenuStripEx
    Friend WithEvents PanelBn As Panel
    Friend WithEvents tlpMain As TableLayoutPanel
End Class
