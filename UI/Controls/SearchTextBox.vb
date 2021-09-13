﻿
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms.VisualStyles

Imports StaxRip.UI

Public Class SearchTextBox
    Inherits UserControl

#Region "Designer"

    Private Sub InitializeComponent()
        Me.Edit = New StaxRip.UI.TextEdit()
        Me.Button = New StaxRip.SearchTextBox.SearchTextBoxButton()
        Me.SuspendLayout()
        '
        'Edit
        '
        Me.Edit.BackColor = System.Drawing.Color.White
        Me.Edit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Edit.Location = New System.Drawing.Point(0, 0)
        Me.Edit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Edit.Name = "Edit"
        Me.Edit.Size = New System.Drawing.Size(200, 70)
        Me.Edit.TabIndex = 3
        '
        'Button
        '
        Me.Button.Location = New System.Drawing.Point(90, 24)
        Me.Button.Name = "Button"
        Me.Button.Size = New System.Drawing.Size(27, 23)
        Me.Button.TabIndex = 2
        Me.Button.Text = "Button1"
        Me.Button.Visible = False
        '
        'SearchTextBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Button)
        Me.Controls.Add(Me.Edit)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "SearchTextBox"
        Me.Size = New System.Drawing.Size(200, 70)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Edit As TextEdit
    Private Button As SearchTextBoxButton

    Sub New()
        InitializeComponent()
        Edit.TextBox.SendMessageCue("Search", False)
        AddHandler Edit.TextChanged, Sub() OnTextChanged(New EventArgs)
        AddHandler Button.Click, Sub() Edit.Text = String.Empty
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Button.Visible = Edit.Text.NotNullOrEmptyS
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)
        'Button.Top = 2
        Dim h As Integer = Height
        'Button.Height = h - 4 ' bh=Height-4
        'Button.Width = h - 4 ' bw=Button.Height
        Dim w As Integer = Width
        'Button.Left = w - h - 4 - 2 ' bl=Width - Button.Width - Button.Top
        Button.SetBounds(w - h - 4 - 2, 2, h - 4, h - 4)

        Dim eh As Integer = Edit.Height
        If h <> eh Then Height = eh

        Edit.Width = w
    End Sub

    Overrides Property Text As String
        Get
            Return Edit.Text
        End Get
        Set(value As String)
            Edit.Text = value
        End Set
    End Property

    Class SearchTextBoxButton
        Inherits Control

        Private MouseIsOver As Boolean

        Sub New()
            SetStyle(
                ControlStyles.AllPaintingInWmPaint Or
                ControlStyles.OptimizedDoubleBuffer,
                True)
        End Sub

        Protected Overrides Sub OnMouseEnter(eventArgs As EventArgs)
            MyBase.OnMouseEnter(eventArgs)
            MouseIsOver = True
            Invalidate()
            'Refresh()
        End Sub

        Protected Overrides Sub OnMouseLeave(eventArgs As EventArgs)
            MyBase.OnMouseLeave(eventArgs)
            MouseIsOver = False
            Invalidate()
            'Refresh()
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality

            If MouseIsOver Then
                ' Dim rect = New Rectangle(Point.Empty, Size)'DeadCode ???

                'If VisualStyleInformation.IsEnabledByUser Then 'Windows 10 Assume Visual stryles ON
                Dim Renderer = New VisualStyleRenderer(VisualStyleElement.Button.PushButton.Hot)
                Renderer.DrawBackground(e.Graphics, ClientRectangle)
                'Else
                'ControlPaint.DrawButton(e.Graphics, ClientRectangle, ButtonState.Flat) 'This was previous - Gray Button
                'End If
            Else
                e.Graphics.Clear(Color.White)
            End If

            Using pen = New Pen(Color.DarkSlateGray, FontHeight / 16.0F)
                Dim offset = CSng(Width / 3.3)
                e.Graphics.DrawLine(pen, offset, offset, Width - offset, Height - offset)
                e.Graphics.DrawLine(pen, Width - offset, offset, offset, Height - offset)
            End Using
        End Sub
    End Class
End Class
