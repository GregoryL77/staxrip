
Imports System.Threading

Imports StaxRip.UI

Public Class MacrosForm
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

    Friend WithEvents lv As ListView
    Friend WithEvents stb As StaxRip.SearchTextBox
    Friend WithEvents lName As System.Windows.Forms.Label
    Friend WithEvents lValue As System.Windows.Forms.Label
    Friend WithEvents lDescriptionTitle As System.Windows.Forms.Label
    Friend WithEvents lNameTitle As System.Windows.Forms.Label
    Friend WithEvents lValueTitle As System.Windows.Forms.Label
    Friend WithEvents bnCopy As System.Windows.Forms.Button
    Friend WithEvents tlpRight As TableLayoutPanel
    Friend WithEvents lDescription As Label
    Friend WithEvents tlpLeft As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lv = New System.Windows.Forms.ListView()
        Me.stb = New StaxRip.SearchTextBox()
        Me.lName = New System.Windows.Forms.Label()
        Me.lValue = New System.Windows.Forms.Label()
        Me.lDescriptionTitle = New System.Windows.Forms.Label()
        Me.lNameTitle = New System.Windows.Forms.Label()
        Me.lValueTitle = New System.Windows.Forms.Label()
        Me.bnCopy = New System.Windows.Forms.Button()
        Me.tlpRight = New System.Windows.Forms.TableLayoutPanel()
        Me.lDescription = New System.Windows.Forms.Label()
        Me.tlpLeft = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpRight.SuspendLayout()
        Me.tlpLeft.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'lv
        '
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lv.Location = New System.Drawing.Point(3, 27)
        Me.lv.Margin = New System.Windows.Forms.Padding(3, 0, 0, 3)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(98, 154)
        Me.lv.TabIndex = 2
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'stb
        '
        Me.stb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stb.BackColor = System.Drawing.Color.Aqua
        Me.stb.Location = New System.Drawing.Point(3, 3)
        Me.stb.Margin = New System.Windows.Forms.Padding(3, 3, 0, 3)
        Me.stb.Name = "stb"
        Me.stb.Size = New System.Drawing.Size(98, 21)
        Me.stb.TabIndex = 4
        '
        'lName
        '
        Me.lName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lName.Location = New System.Drawing.Point(1, 17)
        Me.lName.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lName.Name = "lName"
        Me.lName.Size = New System.Drawing.Size(123, 17)
        Me.lName.TabIndex = 5
        Me.lName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lValue
        '
        Me.lValue.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lValue.Location = New System.Drawing.Point(1, 78)
        Me.lValue.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lValue.Name = "lValue"
        Me.lValue.Size = New System.Drawing.Size(123, 33)
        Me.lValue.TabIndex = 6
        Me.lValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDescriptionTitle
        '
        Me.lDescriptionTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDescriptionTitle.Location = New System.Drawing.Point(1, 111)
        Me.lDescriptionTitle.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lDescriptionTitle.Name = "lDescriptionTitle"
        Me.lDescriptionTitle.Size = New System.Drawing.Size(123, 18)
        Me.lDescriptionTitle.TabIndex = 7
        Me.lDescriptionTitle.Text = "Description:"
        '
        'lNameTitle
        '
        Me.lNameTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lNameTitle.Location = New System.Drawing.Point(1, 0)
        Me.lNameTitle.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lNameTitle.Name = "lNameTitle"
        Me.lNameTitle.Size = New System.Drawing.Size(123, 17)
        Me.lNameTitle.TabIndex = 9
        Me.lNameTitle.Text = "Name:"
        Me.lNameTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lValueTitle
        '
        Me.lValueTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lValueTitle.Location = New System.Drawing.Point(1, 61)
        Me.lValueTitle.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lValueTitle.Name = "lValueTitle"
        Me.lValueTitle.Size = New System.Drawing.Size(123, 17)
        Me.lValueTitle.TabIndex = 10
        Me.lValueTitle.Text = "Value:"
        Me.lValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnCopy
        '
        Me.bnCopy.Location = New System.Drawing.Point(1, 35)
        Me.bnCopy.Margin = New System.Windows.Forms.Padding(1)
        Me.bnCopy.Name = "bnCopy"
        Me.bnCopy.Size = New System.Drawing.Size(91, 25)
        Me.bnCopy.TabIndex = 11
        Me.bnCopy.Text = "Copy"
        Me.bnCopy.UseVisualStyleBackColor = True
        '
        'tlpRight
        '
        Me.tlpRight.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpRight.ColumnCount = 1
        Me.tlpRight.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRight.Controls.Add(Me.lNameTitle, 0, 0)
        Me.tlpRight.Controls.Add(Me.lValue, 0, 4)
        Me.tlpRight.Controls.Add(Me.lDescription, 0, 6)
        Me.tlpRight.Controls.Add(Me.lDescriptionTitle, 0, 5)
        Me.tlpRight.Controls.Add(Me.bnCopy, 0, 2)
        Me.tlpRight.Controls.Add(Me.lName, 0, 1)
        Me.tlpRight.Controls.Add(Me.lValueTitle, 0, 3)
        Me.tlpRight.Location = New System.Drawing.Point(104, 1)
        Me.tlpRight.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpRight.Name = "tlpRight"
        Me.tlpRight.RowCount = 7
        Me.tlpRight.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpRight.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpRight.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpRight.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpRight.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpRight.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpRight.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15.0!))
        Me.tlpRight.Size = New System.Drawing.Size(125, 184)
        Me.tlpRight.TabIndex = 12
        '
        'lDescription
        '
        Me.lDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDescription.Location = New System.Drawing.Point(1, 129)
        Me.lDescription.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.lDescription.Name = "lDescription"
        Me.lDescription.Size = New System.Drawing.Size(123, 55)
        Me.lDescription.TabIndex = 12
        '
        'tlpLeft
        '
        Me.tlpLeft.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpLeft.ColumnCount = 1
        Me.tlpLeft.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpLeft.Controls.Add(Me.stb, 0, 0)
        Me.tlpLeft.Controls.Add(Me.lv, 0, 1)
        Me.tlpLeft.Location = New System.Drawing.Point(1, 1)
        Me.tlpLeft.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpLeft.Name = "tlpLeft"
        Me.tlpLeft.RowCount = 2
        Me.tlpLeft.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpLeft.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpLeft.Size = New System.Drawing.Size(101, 184)
        Me.tlpLeft.TabIndex = 13
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.0!))
        Me.tlpMain.Controls.Add(Me.tlpLeft, 0, 0)
        Me.tlpMain.Controls.Add(Me.tlpRight, 1, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 1
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Size = New System.Drawing.Size(230, 186)
        Me.tlpMain.TabIndex = 14
        '
        'MacrosForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(230, 186)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "MacrosForm"
        Me.Text = "Macros"
        Me.tlpRight.ResumeLayout(False)
        Me.tlpLeft.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New()
        InitializeComponent()
        ScaleClientSize(32, 31, FontHeight)
        lv.View = View.Tile
        lv.FullRowSelect = True
        lv.MultiSelect = False
        lv.Columns.Add(New ColumnHeader())
        Native.SetWindowTheme(lv.Handle, "explorer", Nothing)
        ActiveControl = stb
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)
        Populate()
        lDescriptionTitle.SetFontStyle(FontStyle.Bold)
        lNameTitle.SetFontStyle(FontStyle.Bold)
        lValueTitle.SetFontStyle(FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        hevent.Handled = True
        g.ShellExecute("https://staxrip.readthedocs.io/macros.html")
        MyBase.OnHelpRequested(hevent)
    End Sub

    Shared Sub ShowDialogForm()
        Using form As New MacrosForm
            form.ShowDialog()
        End Using
    End Sub

    Function Match(search As String, ParamArray values As String()) As Boolean
        For Each i In values
            If i.NotNullOrEmptyS AndAlso i.ToLower.Contains(search.ToLower) Then
                Return True
            End If
        Next
    End Function

    Sub Populate() '(Optional sort As Boolean = True)
        lv.BeginUpdate()
        lv.Items.Clear()

        Dim macros As New StringPairList

        For Each mac In Macro.GetMacros(True, True, True)
            macros.Add(mac.Name, mac.Description)
        Next

        For Each i In macros
            If stb.Text.NullOrEmptyS OrElse Match(stb.Text, i.Name, i.Value) Then
                Dim item As New ListViewItem
                item.Text = i.Name
                item.Tag = i.Value
                lv.Items.Add(item)
            End If
        Next

        If lv.Items.Count > 0 Then
            lv.Items(0).Selected = True
            lv.Items(0).EnsureVisible()
            lv.TileSize = New Size(lv.Width - SystemInformation.VerticalScrollBarWidth - 5, CInt(If(s.UIScaleFactor = 1, 16, Font.Height) * 1.5)) 'Test This Experiment!!! NoScaling
            lv.Scrollable = True
            Native.SetWindowTheme(lv.Handle, "explorer", Nothing)
        End If

        lv.EndUpdate()
        'lv.Refresh()
        lv.Invalidate()
    End Sub

    Sub tbFilter_TextChanged() Handles stb.TextChanged
        Populate()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Select Case e.KeyData
            Case Keys.Enter
                If lName.Text.NotNullOrEmptyS Then
                    bnCopy.PerformClick()
                    Close()
                End If
        End Select

        If lv.Items.Count > 0 Then
            If lv.SelectedIndices.Count = 0 Then
                lv.Items(0).Selected = True
            End If

            Dim sel As Integer

            Select Case e.KeyData
                Case Keys.Up
                    e.Handled = True
                    sel = -1
                Case Keys.Down
                    e.Handled = True
                    sel = 1
            End Select

            If sel <> 0 Then
                sel = lv.SelectedItems(0).Index + sel

                If sel < 0 Then
                    sel = 0
                End If

                If sel >= lv.Items.Count Then
                    sel = lv.Items.Count - 1
                End If

                lv.Items(sel).Selected = True
                lv.Items(sel).EnsureVisible()
            End If
        End If
        MyBase.OnKeyDown(e)
    End Sub

    Sub lv_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lv.MouseDoubleClick
        If lName.Text.NullOrEmptyS Then Exit Sub
        bnCopy.PerformClick()
        Close()
    End Sub

    Sub lv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lv.SelectedIndexChanged
        'bnCopy.Text = "Copy"

        If lv.SelectedItems.Count > 0 Then
            bnCopy.Enabled = True
            Dim item = lv.SelectedItems(0)
            lName.Text = item.Text
            lValue.Text = Macro.Expand(item.Text)
            lDescription.Text = CStr(item.Tag)
        Else
            bnCopy.Enabled = False
            lName.Text = ""
            lValue.Text = ""
            lDescription.Text = ""
        End If
    End Sub

    Sub bnCopy_Click(sender As Object, e As EventArgs) Handles bnCopy.Click
        If lName.Text.NullOrEmptyS Then Exit Sub
        Clipboard.SetText(lName.Text)
        bnCopy.SetFontStyle(FontStyle.Bold)
        Application.DoEvents()
        Thread.Sleep(300)
        bnCopy.SetFontStyle(FontStyle.Regular)
    End Sub
End Class
