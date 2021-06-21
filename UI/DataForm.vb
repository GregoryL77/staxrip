
Public Class DataForm
    Property HelpAction As Action

    Sub New()
        InitializeComponent()
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        HelpAction?.Invoke
        hevent.Handled = True
        MyBase.OnHelpRequested(hevent)
    End Sub
End Class
