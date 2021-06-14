
Imports StaxRip.UI

Public Class MacroEditorDialog
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

    Friend WithEvents MacroEditorControl As StaxRip.MacroEditorControl
    Friend WithEvents bnContext As System.Windows.Forms.Button
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tlp As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.MacroEditorControl = New StaxRip.MacroEditorControl()
        Me.bnContext = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlp = New System.Windows.Forms.TableLayoutPanel()
        Me.tlp.SuspendLayout()
        Me.SuspendLayout()
        '
        'MacroEditorControl
        '
        Me.MacroEditorControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.SetColumnSpan(Me.MacroEditorControl, 3)
        Me.MacroEditorControl.Location = New System.Drawing.Point(0, 0)
        Me.MacroEditorControl.Margin = New System.Windows.Forms.Padding(0)
        Me.MacroEditorControl.Name = "MacroEditorControl"
        Me.MacroEditorControl.Size = New System.Drawing.Size(292, 203)
        Me.MacroEditorControl.TabIndex = 0
        '
        'bnContext
        '
        Me.bnContext.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnContext.Location = New System.Drawing.Point(5, 208)
        Me.bnContext.Margin = New System.Windows.Forms.Padding(5)
        Me.bnContext.Name = "bnContext"
        Me.bnContext.Size = New System.Drawing.Size(61, 23)
        Me.bnContext.TabIndex = 2
        Me.bnContext.UseVisualStyleBackColor = True
        Me.bnContext.Visible = False
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(204, 208)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(5)
        Me.bnCancel.Size = New System.Drawing.Size(83, 23)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(116, 208)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0)
        Me.bnOK.Size = New System.Drawing.Size(83, 23)
        Me.bnOK.Text = "OK"
        '
        'tlp
        '
        Me.tlp.ColumnCount = 3
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlp.Controls.Add(Me.bnCancel, 2, 1)
        Me.tlp.Controls.Add(Me.MacroEditorControl, 0, 0)
        Me.tlp.Controls.Add(Me.bnContext, 0, 1)
        Me.tlp.Controls.Add(Me.bnOK, 1, 1)
        Me.tlp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlp.Location = New System.Drawing.Point(0, 0)
        Me.tlp.Margin = New System.Windows.Forms.Padding(0)
        Me.tlp.Name = "tlp"
        Me.tlp.RowCount = 2
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7.0!))
        Me.tlp.Size = New System.Drawing.Size(292, 236)
        Me.tlp.TabIndex = 3
        '
        'MacroEditorDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(292, 236)
        Me.Controls.Add(Me.tlp)
        Me.DoubleBuffered = True
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "MacroEditorDialog"
        Me.Text = "Text"
        Me.tlp.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private HelpText As String

    Sub New()
        MyBase.New()
        InitializeComponent()

        SetMacroDefaults()
    End Sub

    Sub SetBatchDefaults()
        Text = "Batch Editor"
        HelpText = "Editor for batch script editing."
        MacroEditorControl.SetCommandLineDefaults()
        ScaleClientSize(35, 22, FontHeight)
    End Sub

    Sub SetMacroDefaults()
        Text = "Macro Editor"
        HelpText = "Editor for strings that may contain macros."
        MacroEditorControl.SetMacroDefaults()
        ScaleClientSize(35, 25, FontHeight)
    End Sub

    Sub SetScriptDefaults()
        Text = "Script Editor"
        HelpText = "Editor for scripts that may contain macros."
        MacroEditorControl.SetScriptDefaults()
        ScaleClientSize(59, 37, FontHeight)
    End Sub

    Sub UniversalEditor_HelpRequested(sender As Object, e As HelpEventArgs) Handles Me.HelpRequested
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteParagraph(HelpText)
        form.Doc.WriteTips(MacroEditorControl.TipProvider.GetTips)
        form.Doc.WriteTable("Macros", Macro.GetTips(False, True, False))
        form.Show()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        bnContext.AutoSize = True
        'Dim editorControl = MacroEditorControl
        'Dim textSize = TextRenderer.MeasureText(editorControl.rtbEdit.Text, editorControl.rtbEdit.Font)
        Dim workingArea = Screen.FromControl(Me).WorkingArea

        If Width > workingArea.Width * 0.8 Then
            Width = CInt(workingArea.Width * 0.8)
        End If

        If Height > workingArea.Height * 0.8 Then
            Height = CInt(workingArea.Height * 0.8)
        End If
    End Sub
End Class
