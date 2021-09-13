
Imports System.Drawing.Imaging
Imports System.Reflection
Imports StaxRip.UI

Public Class VideoComparisonForm
    Shared Property Pos As Integer

    Public CropLeft, CropTop, CropRight, CropBottom As Integer

    Shadows Menu As ContextMenuStripEx

    Event UpdateMenu()

    Sub New()
        InitializeComponent()
        RestoreClientSize(53, 36)

        KeyPreview = True
        bnMenu.TabStop = False
        TabControl.AllowDrop = True
        TrackBar.NoMouseWheelEvent = True

        Dim enabledFunc = Function() TabControl.SelectedTab IsNot Nothing
        Menu = New ContextMenuStripEx()
        Menu.SuspendLayout()
        Menu.Form = Me

        bnMenu.ContextMenuStrip = Menu
        TabControl.ContextMenuStrip = Menu

        ContextMenuStripEx.CreateAdd2RangeList(10)  ' Test This!!
        Menu.Add2RangeList("Add files to compare...", AddressOf Add, Keys.O, "Video files to compare, the file browser has multiselect enabled.")
        Menu.Add2RangeList("Close selected tab", AddressOf Remove, Keys.Delete, enabledFunc)
        Menu.Add2RangeList("Save PNGs at current position", AddressOf Save, Keys.S, enabledFunc, "Saves a PNG image for every file/tab at the current position in the directory of the source file.")
        Menu.Add2RangeList("Crop and Zoom...", AddressOf CropZoom, Keys.C)
        Menu.Add2RangeList("Go To Frame...", AddressOf GoToFrame, Keys.F, enabledFunc)
        Menu.Add2RangeList("Go To Time...", AddressOf GoToTime, Keys.T, enabledFunc)
        Menu.Add2RangeList("Select next tab", AddressOf NextTab, Keys.Space, enabledFunc)
        Dim navMenu = New ActionMenuItem("Navigate") 'Test This
        Menu.AddTStripMenuItem2List(navMenu)
        Menu.Add2RangeList("Help", AddressOf Me.Help, Keys.F1)
        ContextMenuStripEx.AddRangeList2Menu(Menu.Items)

        navMenu.DropDown.SuspendLayout()
        ContextMenuStripEx.CreateAdd2RangeList(4)
        Menu.Add2RangeList("1 frame backward", Sub() TrackBar.Value -= 1, Keys.Left, enabledFunc)
        Menu.Add2RangeList("1 frame forward", Sub() TrackBar.Value += 1, Keys.Right, enabledFunc)
        Menu.Add2RangeList("100 frame backward", Sub() TrackBar.Value -= 100, Keys.Left Or Keys.Control, enabledFunc)
        Menu.Add2RangeList("100 frame forward", Sub() TrackBar.Value += 100, Keys.Right Or Keys.Control, enabledFunc)
        ContextMenuStripEx.AddRangeList2Menu(navMenu.DropDownItems)

        navMenu.DropDown.ResumeLayout(False)
        Menu.ResumeLayout(False)
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim ret = MyBase.CreateParams
            ret.ExStyle = ret.ExStyle Or &H2000000 'WS_EX_COMPOSITED
            Return ret
        End Get
    End Property

    Sub Add()
        If Not Package.AviSynth.VerifyOK(True) Then
            Exit Sub
        End If

        Using dialog As New OpenFileDialog
            dialog.Filter = FileTypes.GetFilter(FileTypes.Video)
            dialog.Multiselect = True
            dialog.SetInitDir(s.Storage.GetString("video comparison folder"))

            If dialog.ShowDialog() = DialogResult.OK Then
                s.Storage.SetString("video comparison folder", dialog.FileName.Dir)

                For Each file In dialog.FileNames
                    Add(file)
                Next
            End If
        End Using
    End Sub

    Sub Remove()
        If Not TabControl.SelectedTab Is Nothing Then
            Dim tab = TabControl.SelectedTab
            TabControl.TabPages.Remove(tab)
            tab.Dispose()
            RaiseEvent UpdateMenu()
        End If
    End Sub

    Sub Save()
        For Each tab As VideoTab In TabControl.TabPages
            Dim outputPath = tab.SourceFile.Dir & Pos & " " & tab.SourceFile.Base & ".png"

            Using bmp = tab.GetBitmap
                bmp.Save(outputPath, ImageFormat.Png)
            End Using
        Next

        MsgInfo("Images were saved in the source file directory.")
    End Sub

    Sub Add(sourePath As String)
        Dim tab As New VideoTab With {.Form = Me}
        tab.VideoPanel.ContextMenuStrip = TabControl.ContextMenuStrip

        If tab.Open(sourePath) Then
            TabControl.TabPages.Add(tab)
            Dim page = DirectCast(TabControl.SelectedTab, VideoTab)
            page.DoLayout()
            page.TrackBarValueChanged()
            RaiseEvent UpdateMenu()
        Else
            tab.Dispose()
        End If
    End Sub

    Sub TrackBar_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar.ValueChanged
        TrackBarValueChanged()
    End Sub

    Sub TrackBarValueChanged()
        If TabControl.TabPages.Count > 0 Then
            DirectCast(TabControl.SelectedTab, VideoTab).TrackBarValueChanged()
        End If
    End Sub

    Sub Help()
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteParagraph("In the statistic tab of the x265 dialog select Log Level Frame and enable CSV log file creation, the video comparison tool can displays containing frame info.")
        form.Doc.WriteTips(Menu.GetTips)
        form.Doc.WriteTable("Shortcut Keys", Menu.GetKeys, False)
        form.Show()
    End Sub

    Sub NextTab()
        Dim index = TabControl.SelectedIndex + 1

        If index >= TabControl.TabPages.Count Then
            index = 0
        End If

        If index <> TabControl.SelectedIndex Then
            TabControl.SelectedIndex = index
        End If
    End Sub

    Sub Reload()
        For Each tab As VideoTab In TabControl.TabPages
            tab.Reload()
        Next
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MyBase.OnMouseWheel(e)

        Dim value = 100

        If e.Delta < 0 Then
            value = value * -1
        End If

        If s.ReverseVideoScrollDirection Then
            value = value * -1
        End If

        TrackBar.Value += value
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        Dispose()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Add()
    End Sub

    Sub CropZoom()
        Using form As New SimpleSettingsForm("Crop and Zoom")
            form.Height = CInt(form.Height * 0.9)
            form.Width = CInt(form.Width * 0.9)
            Dim ui = form.SimpleUI
            Dim page = ui.CreateFlowPage("main page")
            page.SuspendLayout()

            Dim nb = ui.AddNum(page)
            nb.Label.Text = "Crop Left:"
            nb.NumEdit.Config = {0, 10000, 10}
            nb.NumEdit.Value = CropLeft
            nb.NumEdit.SaveAction = Sub(value) CropLeft = CInt(value)

            nb = ui.AddNum(page)
            nb.Label.Text = "Crop Top:"
            nb.NumEdit.Config = {0, 10000, 10}
            nb.NumEdit.Value = CropTop
            nb.NumEdit.SaveAction = Sub(value) CropTop = CInt(value)

            nb = ui.AddNum(page)
            nb.Label.Text = "Crop Right:"
            nb.NumEdit.Config = {0, 10000, 10}
            nb.NumEdit.Value = CropRight
            nb.NumEdit.SaveAction = Sub(value) CropRight = CInt(value)

            nb = ui.AddNum(page)
            nb.Label.Text = "Crop Bottom:"
            nb.NumEdit.Config = {0, 10000, 10}
            nb.NumEdit.Value = CropBottom
            nb.NumEdit.SaveAction = Sub(value) CropBottom = CInt(value)

            page.ResumeLayout()

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()
                Reload()
                TrackBarValueChanged()
            End If
        End Using
    End Sub

    Sub GoToFrame()
        Dim value = InputBox.Show("Frame:", "Go To Frame", TrackBar.Value.ToString)
        Dim pos As Integer

        If Integer.TryParse(value, pos) Then
            TrackBar.Value = pos
        End If
    End Sub

    Sub GoToTime()
        Dim tab = DirectCast(TabControl.SelectedTab, VideoTab)
        Dim d As Date
        d = d.AddSeconds(Pos / tab.Server.FrameRate)
        Dim value = InputBox.Show("Time:", "Go To Time", d.ToString("HH:mm:ss.fff"))
        Dim time As TimeSpan

        If value.NotNullOrEmptyS AndAlso TimeSpan.TryParse(value, time) Then
            TrackBar.Value = CInt((time.TotalMilliseconds / 1000) * tab.Server.FrameRate)
        End If
    End Sub

    Public Class VideoTab
        Inherits TabPage

        Property Server As IFrameServer
        Property Form As VideoComparisonForm
        Property SourceFile As String
        Property VideoPanel As Panel

        Private Renderer As VideoRenderer
        Private FrameInfo As String()

        Sub New()
            VideoPanel = New Panel
            AddHandler VideoPanel.Paint, Sub() Draw()
            Controls.Add(VideoPanel)
        End Sub

        Sub Reload()
            Server.Dispose()
            Open(SourceFile)
        End Sub

        Function Open(sourePath As String) As Boolean
            Text = sourePath.Base
            SourceFile = sourePath

            Dim script As New VideoScript With {.Engine = ScriptEngine.AviSynth, .Path = Folder.Temp & Guid.NewGuid.ToString & ".avs"}
            Dim deh1 As EventHandler = Sub()
                                           RemoveHandler Me.Disposed, deh1
                                           FileHelp.Delete(script.Path)
                                       End Sub
            AddHandler Disposed, deh1

            script.Filters.Add(New VideoFilter("SetMemoryMax(512)"))

            If sourePath.Ext = "png" Then
                script.Filters.Add(New VideoFilter("ImageSource(""" & sourePath & """, end = 0)"))
            Else
                Try
                    Dim cachePath = Folder.Temp & Guid.NewGuid.ToString & ".ffindex"
                    Dim deh2 As EventHandler = Sub()
                                                   RemoveHandler Me.Disposed, deh2
                                                   FileHelp.Delete(cachePath)
                                               End Sub
                    AddHandler Disposed, deh2
                Catch
                End Try

                If sourePath.EndsWith("mp4") Then
                    script.Filters.Add(New VideoFilter("LSMASHVideoSource(""" & sourePath & "" & """, format = ""YV12"")"))
                Else
                    script.Filters.Add(New VideoFilter("FFVideoSource(""" & sourePath & "" & """, colorspace = ""YV12"")"))
                End If
            End If

            If (Form.CropLeft Or Form.CropTop Or Form.CropRight Or Form.CropBottom) <> 0 Then
                script.Filters.Add(New VideoFilter("Crop(" & Form.CropLeft & ", " & Form.CropTop & ", -" & Form.CropRight & ", -" & Form.CropBottom & ")"))
            End If

            script.Synchronize(True, True, True)
            Server = FrameServerFactory.Create(script.Path)
            Renderer = New VideoRenderer(VideoPanel, Server)

            FileHelp.Delete(sourePath & ".ffindex")

            If Form.TrackBar.Maximum < Server.Info.FrameCount - 1 Then
                Form.TrackBar.Maximum = Server.Info.FrameCount - 1
            End If

            Dim csvFile = sourePath.DirAndBase & ".csv"

            If File.Exists(csvFile) Then
                Dim len = Form.TrackBar.Maximum
                Dim lines = File.ReadAllLines(csvFile)

                If lines.Length > len Then
                    FrameInfo = New String(len) {}
                    Dim headers = lines(0).Split({","c})

                    For x = 1 To len + 1
                        Dim values = lines(x).Split({","c})

                        For x2 = 0 To headers.Length - 1
                            Dim value = values(x2).Trim

                            If value.NotNullOrEmptyS AndAlso Not value.Equals("-") Then
                                FrameInfo(x - 1) &= headers(x2).Trim & ": " & value & ", "
                            End If
                        Next

                        FrameInfo(x - 1) = FrameInfo(x - 1).TrimEnd(" ,".ToCharArray)
                    Next
                End If
            End If

            Return True
        End Function

        Sub Draw()
            Renderer.Position = Pos
            Renderer.Draw()
        End Sub

        Function GetBitmap() As Bitmap
            Return BitmapUtil.CreateBitmap(Server, Pos)
        End Function

        Sub TrackBarValueChanged()
            Try
                Pos = Form.TrackBar.Value
                Draw()

                If FrameInfo IsNot Nothing Then
                    Form.laInfo.Text = FrameInfo(Form.TrackBar.Value)
                Else
                    Dim frameRate = If(Calc.IsValidFrameRate(Server.FrameRate), Server.FrameRate, 25)
                    Dim dt = DateTime.Today.AddSeconds(Pos / frameRate)
                    Form.laInfo.Text = "Position: " & Pos & ", Time: " & dt.ToString("HH:mm:ss.fff") & ", Size: " & Server.Info.Width & " x " & Server.Info.Height
                End If

                Form.laInfo.Refresh()
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End Sub

        Sub DoLayout() 'Remove padding & Rectangle Dims
            If Server Is Nothing Then Exit Sub
            'Dim sizeToFit = New Size(Server.Info.Width, Server.Info.Height)
            'If sizeToFit.IsEmpty Then Exit Sub
            Dim sfW = Server.Info.Width
            Dim sfH = Server.Info.Height
            If sfW = 0 AndAlso sfW = 0 Then Exit Sub

            ' Dim padding As Padding    'This is NonSense??? RemoveIt?
            'Dim rect As New Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical) 'Remove padding & rect Dims
            Dim rW As Integer = Width
            Dim rH As Integer = Height

            'Dim targetPoint As Point
            'Dim targetSize As Size
            Dim tX, Ty, tW, tH As Integer
            'Dim ar1 = rW / rH 'rect.Width / rect.Height
            'Dim ar2 = sizeToFit.Width / sizeToFit.Height
            If sfW / sfH < rW / rH Then 'ar2 < ar1
                tH = rH 'targetSize.Height = rect.Height
                tW = CInt(sfW / (sfH / rH)) 'targetSize.Width = CInt(sizeToFit.Width / (sizeToFit.Height / rect.Height))
                tX = CInt((rW - tW) / 2)  'targetPoint.X = CInt((rect.Width - targetSize.Width) / 2) + padding.Left
                'targetPoint.Y = padding.Top
            Else
                tW = rW 'targetSize.Width = rect.Width
                tH = CInt(sfH / (sfW / rW)) 'targetSize.Height = CInt(sizeToFit.Height / (sizeToFit.Width / rect.Width))
                Ty = CInt((rH - tH) / 2)  'targetPoint.Y=CInt((rect.Height - targetSize.Height) / 2) + padding.Top
                'targetPoint.X = padding.Left
            End If

            'Dim targetRect = New Rectangle(targetPoint, targetSize)
            'Dim reg As New Region(ClientRectangle) 'Seems Dead???
            'reg.Exclude(targetRect)

            VideoPanel.Left = tX 'targetRect.Left
            VideoPanel.Top = Ty 'targetRect.Top
            VideoPanel.Width = tW 'targetRect.Width
            VideoPanel.Height = tH  'targetRect.Height
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If Server IsNot Nothing Then
                Server.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub OnResize(eventargs As EventArgs)
            MyBase.OnResize(eventargs)
            DoLayout()
        End Sub
        'Sub VideoTab_Resize(sender As Object, e As EventArgs) Handles_Me.Resize
        '    DoLayout()
        'End Sub
    End Class
End Class
