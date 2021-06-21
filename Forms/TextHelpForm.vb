
Public Class TextHelpForm
    Property Find As String

    Sub New(text As String, find As String)
        InitializeComponent()
        rtb.Text = text
        rtb.BackColor = Color.Black
        rtb.ForeColor = Color.White
        rtb.ReadOnly = True
        Me.Find = find
        ScaleClientSize(45, 30, FontHeight)
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        rtb.Find(Find)
        rtb.ScrollToCaret()
    End Sub
End Class
