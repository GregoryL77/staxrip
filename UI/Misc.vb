
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Text
Imports JM.LinqFaster

Namespace UI
    Public Class FormBase
        Inherits Form

        Event FilesDropped(files As String())

        Private FileDropValue As Boolean
        Private DefaultWidthScale As Single
        Private DefaultHeightScale As Single

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property FontHeight As Integer

        Sub New()
            Font = New Font("Segoe UI", 9)
            FontHeight = If(s.UIScaleFactor = 1, 16, Font.Height) ' Test This Experiment!!! NoScaling
        End Sub

        <DefaultValue(False)>
        Property FileDrop As Boolean
            Get
                Return FileDropValue
            End Get
            Set(value As Boolean)
                FileDropValue = value
                AllowDrop = value
            End Set
        End Property

        Protected Overrides Sub OnDragEnter(e As DragEventArgs)
            MyBase.OnDragEnter(e)

            If FileDrop Then
                Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

                If Not files.NothingOrEmpty Then
                    e.Effect = DragDropEffects.Copy
                End If
            End If
        End Sub

        Protected Overrides Sub OnDragDrop(args As DragEventArgs)
            MyBase.OnDragDrop(args)

            If FileDrop Then
                Dim files = TryCast(args.Data.GetData(DataFormats.FileDrop), String())

                If Not files.NothingOrEmpty Then
                    RaiseEvent FilesDropped(files)
                End If
            End If
        End Sub

        Sub SetMinimumSize(w As Integer, h As Integer)
            MinimumSize = New Size(CInt(FontHeight * w), CInt(FontHeight * h))
        End Sub

        Protected Overrides Sub OnLoad(args As EventArgs)
            KeyPreview = True
            SetTabIndexes(Me)

            If s.UIScaleFactor <> 1 Then
                Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
                Scale(New SizeF(1 * s.UIScaleFactor, 1 * s.UIScaleFactor))
            End If

            Dim workAr As Size
            Dim w As Integer = -1
            Dim h As Integer = -1
            If DefaultWidthScale <> 0 Then
                Dim fh As Integer = FontHeight 'NoScaling with this !!!???  Font.Height OK
                Dim defaultWidth = CInt(fh * DefaultWidthScale)
                Dim defaultHeight = CInt(fh * DefaultHeightScale)

                Dim fName As String = Me.GetType().Name
                w = s.Storage.GetInt(fName + "width")
                h = s.Storage.GetInt(fName + "height")

                If w < 0 OrElse w < (defaultWidth \ 2) OrElse h < 0 OrElse h < (defaultHeight \ 2) Then 'was:w=0,h=0
                    w = defaultWidth
                    h = defaultHeight
                End If

                workAr = Screen.FromControl(Me).WorkingArea.Size
                If w > workAr.Width OrElse h > workAr.Height Then
                    w = workAr.Width
                    h = workAr.Height
                End If
                'Size = New Size(w, h)
            End If

            If StartPosition = FormStartPosition.CenterScreen Then
                If w < 0 Then w = Width
                If h < 0 Then h = Height
                If workAr.IsEmpty Then workAr = Screen.FromControl(Me).WorkingArea.Size
                StartPosition = FormStartPosition.Manual 'WindowPositions.CenterScreen(Me, workAr)
                Location = New Point((workAr.Width - w) \ 2, (workAr.Height - h) \ 2)
            End If

            Dim pos As New Point(-100000, 0)
            If Not DesignHelp.IsDesignMode Then 'Needed????
                If TypeOf Me IsNot UI.InputBoxForm AndAlso s.WindowPositionsRemembered?.Length > 0 AndAlso s.WindowPositions IsNot Nothing Then
                    pos = s.WindowPositions.RestorePosition(Me)
                    If pos.X <> -100000 Then
                        If w < 0 Then w = Width
                        If h < 0 Then h = Height
                        If workAr.IsEmpty Then workAr = Screen.FromControl(Me).WorkingArea.Size

                        If pos.X < 0 OrElse pos.Y < 0 OrElse pos.X + w > workAr.Width OrElse pos.Y + h > workAr.Height Then
                            pos = New Point((workAr.Width - w) \ 2, (workAr.Height - h) \ 2) 'CenterScreen(form, screenSz)
                        End If
                    End If
                End If
                'Else
                'Dim dddddddddd__ddd = DesignHelp.IsDesignMode 'Debug
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

            MyBase.OnLoad(args)
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

        Sub SetTabIndexes(c As Control)
            Dim ccn As Integer = c.Controls.Count - 1
            If ccn < 0 Then Exit Sub

            Dim ca(ccn) As Control
            c.Controls.CopyTo(ca, 0)
            Dim ka(ccn) As Double
            Dim no0 As Boolean
            For i = 0 To ccn
                Dim ci = ca(i)
                Dim ovc As Double = Math.Sqrt(ci.Top ^ 2 + ci.Left ^ 2) 'Test This
                If ovc > 0 Then no0 = True
                ka(i) = If(ovc > 0, ovc, i)
            Next i
            If no0 Then Array.Sort(ka, ca)
            For i = 0 To ccn
                Dim ctrl As Control = ca(i)
                ctrl.TabIndex = i
                SetTabIndexes(ctrl)
            Next i
            'Dim ctrls = From i In c.Controls.OfType(Of Control)() Order By Math.Sqrt(i.Top ^ 2 + i.Left ^ 2)
            'For Each i In ctrls
            '    Dim index As Integer
            '    i.TabIndex = index
            '    index += 1
            '    SetTabIndexes(i)
            'Next i
        End Sub

        Sub RestoreClientSize(defaultWidthScale As Single, defaultHeightScale As Single)
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
                If i.Value.Equals(value) Then selectItem = i
            Next

            If selectItem IsNot Nothing Then cb.SelectedItem = selectItem
        End Sub

        Shared Function GetValue(cb As ComboBox) As T
            Return DirectCast(DirectCast(cb.SelectedItem, ListBag(Of T)).Value, T)
        End Function

        Shared Function GetBagsForEnumType() As ListBag(Of T)()
            Dim ret As New List(Of ListBag(Of T))

            For Each i As T In System.Enum.GetValues(GetType(T))
                ret.Add(New ListBag(Of T)(UI.DispNameAttribute.GetValueForEnum(i), i))
            Next

            Return ret.ToArray
        End Function

        Overrides Function ToString() As String
            Return Text
        End Function

        Function CompareTo(other As ListBag(Of T)) As Integer Implements IComparable(Of ListBag(Of T)).CompareTo
            Return String.Compare(Text, other.Text, StringComparison.OrdinalIgnoreCase)
        End Function
    End Class

    <Serializable()>
    Public Class WindowPositions
        Private Positions As New Dictionary(Of String, Point)(37, StringComparer.Ordinal)
        Sub Save(form As Form)
            If form.WindowState = FormWindowState.Normal Then Positions(form.Name & form.GetType().FullName & GetText(form)) = form.Location
            'SavePosition(form)
            'SaveWindowState(form) 'Dead Code ???
        End Sub
        'Private WindowStates As New Dictionary(Of String, FormWindowState)(37, StringComparer.Ordinal) 'Dead Code ???
        'Sub SaveWindowState(form As Form) 'Dead Code ???
        '    WindowStates(GetKey(form)) = form.WindowState
        'End Sub
        'Sub SavePosition(form As Form)
        '    If form.WindowState = FormWindowState.Normal Then Positions(GetKey(form)) = form.Location
        'End Sub
        'Shared Sub CenterScreen(form As Form, screenSize As Size) ' Screen.FromControl(form).WorkingArea
        '    form.StartPosition = FormStartPosition.Manual
        '    form.Location = New Point((screenSize.Width - form.Width) \ 2, (screenSize.Height - form.Height) \ 2)
        'End Sub
        'Sub RestorePosition(form As Form, screenSz As Size) 'TODO make Function and SetBounds in one step
        '    If Not s.WindowPositionsRemembered.NothingOrEmpty AndAlso TypeOf form IsNot UI.InputBoxForm Then
        '        Dim text = GetText(form)
        '        For Each i In s.WindowPositionsRemembered
        '            If text.StartsWith(i, StringComparison.Ordinal) OrElse String.Equals(i, "all") Then
        '                Dim pos As Point
        '                If Positions.TryGetValue(GetKey(form), pos) Then
        '                    form.StartPosition = FormStartPosition.Manual
        '                    form.Location = If(pos.X < 0 OrElse pos.Y < 0 OrElse pos.X + form.Width > screenSz.Width OrElse pos.Y + form.Height > screenSz.Height,
        '                        New Point((screenSz.Width - form.Width) \ 2, (screenSz.Height - form.Height) \ 2), 'CenterScreen(form, screenSz)
        '                        pos)
        '                End If
        '                Exit For
        '            End If
        '        Next
        '    End If
        'End Sub
        'Function GetKey(form As Form) As String
        '    Return form.Name & form.GetType().FullName & GetText(form)
        'End Function
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
                Return "Audio Settings"
            ElseIf TypeOf form Is CodeEditor Then
                Return "Code Editor"
            ElseIf TypeOf form Is CustomMenuEditor Then
                Return "Menu Editor"
            ElseIf TypeOf form Is HelpForm Then
                Return "Help"
            ElseIf TypeOf form Is PreviewForm Then
                Return "Preview"
            End If

            Return form.Text
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
