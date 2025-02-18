
Imports System.ComponentModel
Imports System.Drawing.Design

Namespace UI
    Public Class FormBase
        Inherits Form

        Private DefaultWidthScale As Single
        Private DefaultHeightScale As Single

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property FontHeight As Integer

        Sub New()
            Font = New Font("Segoe UI", 9) 'FH=16
            FontHeight = 16 'If(s.UIScaleFactor = 1, 16, Font.Height) ' Test This Experiment!!! NoScaling
        End Sub

        Sub SetMinimumSize(w As Integer, h As Integer)
            MinimumSize = New Size(CInt(FontHeight * w), CInt(FontHeight * h))
        End Sub

        Protected Overrides Sub OnLoad(args As EventArgs)
            If FontHeight >= -1 OrElse FontHeight = -3 Then  'Experiment, Supress FormBase.OnLoad Test This !!! Debug

                KeyPreview = True
                SetTabIndexes(Me)

                If s.UIScaleFactor <> 1 Then
                    Dim fn As New Font("Segoe UI", 9 * s.UIScaleFactor)
                    FontHeight = fn.Height 'Added ??? Experimental ?
                    Font = fn
                    Scale(New SizeF(1 * s.UIScaleFactor, 1 * s.UIScaleFactor))
                End If

                Dim workAr As Size = ScreenResWAPrim
                Dim w As Integer = -1
                Dim h As Integer = -1
                If DefaultWidthScale <> 0 Then
                    Dim fh As Integer = 16 'FontHeight 'Was FontHeight if set to fixed 16 Scaling OK??? NoScaling with this !!!???  Font.Height OK
                    Dim defaultWidth = CInt(fh * DefaultWidthScale)
                    Dim defaultHeight = CInt(fh * DefaultHeightScale)

                    Dim fName As String = Me.GetType().Name
                    w = s.Storage.GetInt(fName & "width")
                    h = s.Storage.GetInt(fName & "height")

                    If w < 0 OrElse w < (defaultWidth \ 2) OrElse h < 0 OrElse h < (defaultHeight \ 2) Then 'was:w=0,h=0
                        w = defaultWidth
                        h = defaultHeight
                    End If

                    'workAr = ScreenResWAPrim 'Screen.FromControl(Me).WorkingArea.Size
                    If w > workAr.Width OrElse h > workAr.Height Then
                        w = workAr.Width
                        h = workAr.Height
                    End If
                    'Size = New Size(w, h)
                End If

                If StartPosition = FormStartPosition.CenterScreen Then
                    If w < 0 Then w = Width
                    If h < 0 Then h = Height
                    'If workAr.IsEmpty Then workAr = ScreenResWAPrim
                    StartPosition = FormStartPosition.Manual 'WindowPositions.CenterScreen(Me, workAr)
                    Location = New Point((workAr.Width - w) \ 2, (workAr.Height - h) \ 2)
                End If

                Dim pos As New Point(-100000, 0)
                If Not DesignHelp.IsDesignMode Then 'Needed????
                    If TypeOf Me IsNot UI.InputBoxForm AndAlso s.WindowPositions IsNot Nothing AndAlso s.WindowPositionsRemembered?.Length > 0 Then
                        pos = s.WindowPositions.RestorePosition(Me)
                        If pos.X <> -100000 Then
                            If w < 0 Then w = Width
                            If h < 0 Then h = Height
                            ' If workAr.IsEmpty Then workAr = ScreenResWAPrim

                            If pos.X < 0 OrElse pos.Y < 0 OrElse pos.X + w > workAr.Width OrElse pos.Y + h > workAr.Height Then
                                pos = New Point((workAr.Width - w) \ 2, (workAr.Height - h) \ 2) 'CenterScreen(form, screenSz)
                            End If
                        End If
                    End If
                End If

                If pos.X >= 0 Then
                    Me.StartPosition = FormStartPosition.Manual
                    If DefaultWidthScale <> 0 Then
                        SetBounds(pos.X, pos.Y, w, h)
                    Else
                        SetBounds(pos.X, pos.Y, 0, 0, BoundsSpecified.Location)
                    End If
                ElseIf DefaultWidthScale <> 0 Then
                    SetBounds(0, 0, w, h, BoundsSpecified.Size)
                End If
            End If

            If FontHeight >= -2 Then 'Experiment, Supress FormBase.OnLoad Test This !!! Debug
                MyBase.OnLoad(args)
            End If
        End Sub

        Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
            MyBase.OnFormClosing(args)

            If s.WindowPositions IsNot Nothing Then
                s.WindowPositions.Save(Me)
            End If

            If DefaultWidthScale <> 0 Then
                SaveClientSize()
            End If
        End Sub

        <Runtime.CompilerServices.MethodImpl(AggrInlin)>
        Sub SetTabIndexes(c As Control)
            Dim ctrls As Control.ControlCollection = c.Controls
            Dim ccn As Integer = ctrls.Count - 1
            Select Case ccn
                Case Is < 0
                    Exit Sub
                Case 0
                    SetTabIndexes(ctrls.Item(0))
                Case Else
                    Dim ca(ccn) As Control
                    ctrls.CopyTo(ca, 0)
                    Dim ka(ccn) As Double
                    Dim no0 As Boolean
                    For i = 0 To ccn
                        Dim loc = ca(i).Location
                        If loc.IsEmpty Then
                            ka(i) = i
                        Else
                            no0 = True
                            ka(i) = Math.Sqrt(loc.X ^ 2 + loc.Y ^ 2) 'Test This (ca(i).Top ^ 2 + ca(i).Left ^ 2)
                        End If
                    Next i
                    If no0 Then Array.Sort(ka, ca)
                    For i = 0 To ccn
                        Dim ci As Control = ca(i)
                        ci.TabIndex = i
                        SetTabIndexes(ci)
                    Next i
            End Select
            'Dim ctrls = From i In c.Controls.OfType(Of Control)() Order By Math.Sqrt(i.Top ^ 2 + i.Left ^ 2)
            'For Each i In ctrls
            '    Dim index As Integer
            '    i.TabIndex = index
            '    index += 1
            '    SetTabIndexes(i)
            'Next i
        End Sub

        <Runtime.CompilerServices.MethodImpl(AggrInlin)>
        Sub RestoreClientSize(defaultWidthScale As Single, defaultHeightScale As Single) 'Add CMDAudioEnc Here ???
            Me.DefaultWidthScale = defaultWidthScale
            Me.DefaultHeightScale = defaultHeightScale
        End Sub

        Sub SaveClientSize()
            Dim fName As String = Me.GetType().Name
            s.Storage.SetInt(fName & "width", Width)
            s.Storage.SetInt(fName & "height", Height)
        End Sub
    End Class

    Public Class DialogBase
        Inherits FormBase

        Sub New()
            'FormBorderStyle = FormBorderStyle.FixedDialog
            HelpButton = True
            'MaximizeBox = False
            'MinimizeBox = False
            ShowIcon = False
            ShowInTaskbar = False
            StartPosition = FormStartPosition.CenterParent
        End Sub

        Protected Overrides Sub OnHelpButtonClicked(args As CancelEventArgs)
            MyBase.OnHelpButtonClicked(args)
            args.Cancel = True
            OnHelpRequested(New HelpEventArgs(MousePosition))
        End Sub
    End Class

    <Serializable()>
    Public Class WindowPositions
        Private Positions As New Dictionary(Of String, Point)(37, StringComparer.Ordinal)
        Sub Save(form As Form)
            If form.WindowState = FormWindowState.Normal Then Positions(form.Name & form.GetType().FullName & GetText(form)) = form.Location
            'SaveWindowState(form) 'Dead Code ???
        End Sub

        Function RestorePosition(form As Form) As Point
            Dim txt As String = GetText(form)
            Dim wpr As String() = s.WindowPositionsRemembered
            For i = 0 To wpr.Length - 1
                Dim r = wpr(i)
                If txt.StartsWith(r, StringComparison.Ordinal) OrElse String.Equals(r, "all") Then
                    'Dim pos As Point
                    Dim pos As Point = If(Positions.TryGetValue(form.Name & form.GetType().FullName & txt, pos), pos, New Point(-100000, 0))
                    Return pos
                End If
            Next i
            Return New Point(-100000, 0)
        End Function

        Function GetText(form As Form) As String

            If TypeOf form Is AudioConverterForm Then
                Return "AudioConverter"
            ElseIf TypeOf form Is MainForm Then
                Return "StaxRip"
            ElseIf TypeOf form Is AudioForm Then
                Return "AudioSettings"
            ElseIf TypeOf form Is CodeEditor Then
                Return "CodeEditor"
            ElseIf TypeOf form Is CustomMenuEditor Then
                Return "MenuEditor"
            ElseIf TypeOf form Is HelpForm Then
                Return "Help"
            ElseIf TypeOf form Is LogForm Then
                Return "LogForm"
            ElseIf TypeOf form Is PreviewForm Then
                Return "Preview"
            End If

            Return form.Text
        End Function
        'Private WindowStates As New Dictionary(Of String, FormWindowState)(37, StringComparer.Ordinal) 'Dead Code ???
        'Sub SaveWindowState(form As Form) 'Dead Code ???
        '    WindowStates(GetKey(form)) = form.WindowState
        'End Sub
    End Class

    Public Class ListBag(Of T)
        Implements IComparable(Of ListBag(Of T))

        Property Text As String
        Property Value As T

        Sub New(text As String, value As T)
            Me.Text = text
            Me.Value = value
        End Sub

        Shared Sub SelectItem(cb As ComboBox, value As T)
            Dim selectItem As Object = Nothing

            For Each i As ListBag(Of T) In cb.Items
                If i.Value.Equals(value) Then
                    selectItem = i
                    Exit For 'Added 202108 !!!
                End If
            Next
            If selectItem IsNot Nothing Then cb.SelectedItem = selectItem
        End Sub

        Shared Function GetValue(cb As ComboBox) As T
            Return DirectCast(cb.SelectedItem, ListBag(Of T)).Value
        End Function

        Shared Function GetBagsForEnumType() As ListBag(Of T)()
            'Dim ret As New List(Of ListBag(Of T))(12)
            'For Each i As T In System.Enum.GetValues(GetType(T))
            '    ret.Add(New ListBag(Of T)(UI.DispNameAttribute.GetValueForEnum(i), i))
            'Next
            'Return ret.ToArray
            Dim enumGVA = System.Enum.GetValues(GetType(T))
            Dim ret(enumGVA.Length - 1) As ListBag(Of T)
            For Each i As T In enumGVA
                Dim inc As Integer
                ret(inc) = New ListBag(Of T)(UI.DispNameAttribute.GetValueForEnum(i), i)
                inc += 1
            Next i
            Return ret
        End Function

        Overrides Function ToString() As String
            Return Text
        End Function

        Function CompareTo(other As ListBag(Of T)) As Integer Implements IComparable(Of ListBag(Of T)).CompareTo
            Return String.Compare(Text, other.Text, StringComparison.OrdinalIgnoreCase)
        End Function
    End Class

    Public Class OpenFileDialogEditor
        Inherits UITypeEditor

        Overloads Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Using f As New OpenFileDialog
                If f.ShowDialog = DialogResult.OK Then
                    Return f.FileName
                Else
                    Return value
                End If
            End Using
        End Function

        Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
    End Class

    Public Class StringEditor
        Inherits UITypeEditor

        Sub New()
        End Sub

        Overloads Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Dim form As New UI.StringEditorForm
            form.rtb.Text = DirectCast(value, String)

            If form.ShowDialog() = DialogResult.OK Then
                Return form.rtb.Text
            Else
                Return value
            End If
        End Function

        Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
    End Class

    Public Class DesignHelp
        Private Shared IsDesignModeValue As Boolean?

        Shared ReadOnly Property IsDesignMode As Boolean
            Get
                If Not IsDesignModeValue.HasValue Then
                    IsDesignModeValue = String.Equals(Process.GetCurrentProcess.ProcessName, "devenv")
                End If

                Return IsDesignModeValue.Value
            End Get
        End Property
    End Class
End Namespace
