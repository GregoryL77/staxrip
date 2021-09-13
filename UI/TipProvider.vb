
Imports System.ComponentModel
Imports System.Drawing.Design

Namespace UI
    <ProvideProperty("TipText", GetType(Control))>
    Public Class TipProvider
        Inherits Component
        Implements IExtenderProvider

        Private ToolTip As New ToolTip
        Private TipTitles As New Dictionary(Of Control, String)(17)
        Private TipTexts As New Dictionary(Of Control, String)(17)

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property TipsFunc As Func(Of StringPairList)

        Sub New(Optional component As IContainer = Nothing)
            If component IsNot Nothing Then
                component.Add(Me)
            End If
            ToolTip.AutomaticDelay = 1000
            ToolTip.AutoPopDelay = 10000
            ToolTip.InitialDelay = 1000
            ToolTip.ReshowDelay = 1000
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            ToolTip.Dispose()
            MyBase.Dispose(disposing)
        End Sub

        Function CanExtend(obj As Object) As Boolean Implements IExtenderProvider.CanExtend
            Return TypeOf obj Is Control
        End Function

        <Category("TipProvider")>
        <DefaultValue("")>
        <Editor(GetType(StringEditor), GetType(UITypeEditor))>
        Function GetTipText(ctrl As Control) As String
            Dim ret As String
            If TipTexts.TryGetValue(ctrl, ret) Then
                Return ret
            End If

            Return ""
        End Function

        Sub SetTipText(c As Control, value As String)
            TipTexts(c) = value
            Init(value, c)
        End Sub

        Sub SetTip(tipText As String, tipTitle As String, c As Control)
            TipTitles(c) = tipTitle
            SetTipText(c, tipText)
        End Sub

        Sub SetTip(tipText As String, ParamArray controls As Control())
            If tipText.NullOrEmptyS Then
                Exit Sub
            End If
            'Dim title As String
            'For i = 0 To controls.Length - 1 'TODO: Test This : in One loop
            '    Dim ctrl = controls(i)
            '    If TypeOf ctrl Is Label OrElse TypeOf ctrl Is CheckBox Then
            '        title = FormatName(ctrl.Text)
            '    End If
            'Next i
            For i = 0 To controls.Length - 1
                Dim ctrl = controls(i)

                If TypeOf ctrl Is Label OrElse TypeOf ctrl Is CheckBox Then
                    Dim title = FormatName(ctrl.Text)
                    If title.Length > 0 Then
                        TipTitles(ctrl) = title
                    End If
                End If

                TipTexts(ctrl) = tipText
                Init(tipText, ctrl)
            Next
        End Sub

        Sub Init(tipText As String, control As Control)
            If Not DesignMode Then
                AddHandler control.MouseDown, AddressOf TipMouseDown
                ' tipText = tipText.TrimEnd("."c)

                If tipText.Length > 240 Then '80 ToDO Test This !!!
                    If HasContextMenu(control) Then
                        tipText = Nothing
                    Else
                        tipText = tipText.Substring(0, 240) & "..." '"Right-click for help" was  80
                    End If
                ElseIf Not AudioConverterForm.AudioConverterMode AndAlso tipText.Length <= 80 AndAlso HelpDocument.MustConvert(tipText) Then 'AudionConverter Opt.
                    tipText = HelpDocument.ConvertMarkup(tipText, True)
                End If

                If tipText?.Length > 0 Then
                    Dim ehc As EventHandler = Sub()
                                                  ToolTip.SetToolTip(control, tipText)
                                                  RemoveHandler control.HandleCreated, ehc 'ToDO TEst This Experiment !!!! Needed?
                                              End Sub
                    AddHandler control.HandleCreated, ehc
                End If
            End If
        End Sub

        Sub TipMouseDown(sender As Object, e As MouseEventArgs)
            If e.Button = MouseButtons.Right AndAlso Not HasContextMenu(DirectCast(sender, Control)) Then

                ShowHelp(DirectCast(sender, Control))
            End If
        End Sub

        Sub ShowHelp(ctrl As Control)
            Dim tip = GetTip(ctrl)
            g.ShowHelp(tip.Name, tip.Value)
        End Sub

        Function GetTip(ctrl As Control) As StringPair
            Dim ret As New StringPair
            Dim val As String

            If TipTitles.TryGetValue(ctrl, val) Then
                ret.Name = FormatName(val)
            End If

            If TipTexts.TryGetValue(ctrl, val) Then
                ret.Value = val
            End If

            Return ret
        End Function

        Function HasContextMenu(ctrl As Control) As Boolean
            If TypeOf ctrl Is TextBox Then Return True
            If TypeOf ctrl Is RichTextBox Then Return True
            If TypeOf ctrl Is NumericUpDown Then Return True

            If TypeOf ctrl Is ComboBox AndAlso DirectCast(ctrl, ComboBox).DropDownStyle = ComboBoxStyle.DropDown Then
                Return True
            End If

            If ctrl.ContextMenuStrip IsNot Nothing Then
                Return True
            End If
        End Function

        Function FormatName(value As String) As String
            If value.Length > 0 Then
                value = value.Trim

                'If Not value.Contains(" & ") Then 'Remove These ???? Test This !!!!!!!!!!!!!!!!!!!!!! REMOVED 202107 !!!!!!!!
                '    'If value.Contains("&") AndAlso Not value.Contains(" & ")
                '    value = value.Replace("&", "")
                'End If
                If value.EndsWith("...", StringComparison.Ordinal) Then 'Remove These ????
                    value = value.TrimEnd("."c) 'Remove These ????
                End If

                value = value.TrimEnd(":"c) 'Remove These ????
            End If

            Return value
        End Function

        Function GetTips() As StringPairList
            Dim ret As New StringPairList
            'Dim temp As New HashSet(Of String)(7, StringComparer.Ordinal)

            For Each ctrl In TipTexts.Keys
                If Not ctrl.IsDisposed AndAlso ctrl.Visible Then
                    Dim valD As String
                    Dim n As String = If(TipTitles.TryGetValue(ctrl, valD), valD, ctrl.Text)
                    Dim temp As New HashSet(Of String)(7, StringComparer.Ordinal) 'Test This Moved inside loop !!!!!!
                    If temp.Add(n) Then
                        ret.Add(New StringPair(FormatName(n), TipTexts(ctrl)))
                    End If
                End If
            Next ctrl

            If TipsFunc IsNot Nothing Then
                ret.AddRange(TipsFunc.Invoke)
            End If

            ret.Sort()

            Return ret
        End Function
    End Class
End Namespace
