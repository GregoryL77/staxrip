
Imports JM.LinqFaster
Imports StaxRip.UI

Public Class SelectionBox(Of T)
    Property Text As String
    Property Title As String
    Property Items As New List(Of ListBag(Of T))(4)
    Property SelectedBag As ListBag(Of T)

    Property SelectedValue As T
        Get
            Return SelectedBag.Value
        End Get
        Set(value As T)
            For Each i In Items
                If i.Value.Equals(value) Then
                    SelectedBag = i
                End If
            Next
        End Set
    End Property

    Property SelectedText As String
        Get
            Return SelectedBag.Text
        End Get
        Set(value As String)
            For Each i In Items
                If EqualsExS(i.Text, value) Then
                    SelectedBag = i
                End If
            Next
        End Set
    End Property

    Sub AddItem(text As String, item As T)
        Items.Add(New ListBag(Of T)(text, item))

        If SelectedBag Is Nothing Then
            SelectedBag = Items.Last
        End If
    End Sub

    Sub AddItem(item As T)
        AddItem(item.ToString, item)
    End Sub

    Function Show() As DialogResult
        Using form As New SelectionBoxForm
            form.SuspendLayout()
            If Items.Count > 0 Then
                form.mb.Add(Items)
                form.mb.Value = SelectedBag
            End If

            Dim fW = form.Width
            Dim fMBW As Integer = form.mb.Width
            Dim fFn As Font = form.Font
            Dim maxL As Integer
            Dim maxTxt As String

            For i = 0 To Items.Count - 1
                Dim lbTxt = Items(i).Text
                If lbTxt.Length > maxL Then
                    maxL = lbTxt.Length
                    maxTxt = lbTxt
                End If
            Next i
            Dim textWidth = TextRenderer.MeasureText(maxTxt, fFn).Width + form.FontHeight * 4
            If fMBW < textWidth Then fW += textWidth - fMBW
            'For Each i In Items
            '    Dim textWidth = TextRenderer.MeasureText(i.Text, fFn).Width + form.FontHeight * 3 'was: form.mb.Font
            '    If fMBW < textWidth Then            '        fW += textWidth - fMBW
            'Next
            'audio conv muxer select profile width

            Dim LtextWidth As Integer = CInt(TextRenderer.MeasureText(Text, fFn).Width / 1.5) ' / 1.75) 'was: form.laText.Font
            'Dim fLaTW As Integer = form.laText.Width
            If fW < LtextWidth Then 'fLaTW
                fW += LtextWidth - fW 'fLaTW
            End If

            form.Width = fW
            form.Text = Title

            If form.Text.NullOrEmptyS Then
                form.Text = Application.ProductName
            End If

            form.laText.Text = Text
            form.ResumeLayout()
            Dim ret = form.ShowDialog
            SelectedBag = DirectCast(form.mb.Value, ListBag(Of T))

            Return ret
        End Using
    End Function
End Class
