
Imports System.Runtime.ExceptionServices
Imports System.Threading.Tasks
Imports StaxRip.UI
Public Class AudioConverterForm
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

    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
    Friend WithEvents tpAudio As TabPage
    Friend WithEvents bnAudioPlay As ButtonEx
    Friend WithEvents bnAudioDown As ButtonEx
    Friend WithEvents bnAudioUp As ButtonEx
    Friend WithEvents bnAudioRemove As ButtonEx
    Friend WithEvents bnAudioAdd As ButtonEx
    Friend WithEvents dgvAudio As DataGridViewEx
    Friend WithEvents bnAudioEdit As ButtonEx
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents flpAudio As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents pnTab As Panel
    Friend WithEvents bnAudioEncodeAll As ButtonEx
    Friend WithEvents bnAudioEncodeSelected As ButtonEx
    Friend WithEvents bnAudioRemoveAll As ButtonEx
    Friend WithEvents bnMenuAudio As ButtonEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpAudio = New System.Windows.Forms.TabPage()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAudio = New System.Windows.Forms.FlowLayoutPanel()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.bnAudioAdd = New StaxRip.UI.ButtonEx()
        Me.bnAudioRemove = New StaxRip.UI.ButtonEx()
        Me.bnAudioRemoveAll = New StaxRip.UI.ButtonEx()
        Me.bnAudioUp = New StaxRip.UI.ButtonEx()
        Me.bnAudioDown = New StaxRip.UI.ButtonEx()
        Me.bnAudioPlay = New StaxRip.UI.ButtonEx()
        Me.bnAudioEncodeSelected = New StaxRip.UI.ButtonEx()
        Me.bnAudioEncodeAll = New StaxRip.UI.ButtonEx()
        Me.bnAudioEdit = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.bnMenuAudio = New StaxRip.UI.ButtonEx()
        Me.pnTab = New System.Windows.Forms.Panel()
        Me.tcMain.SuspendLayout()
        Me.tpAudio.SuspendLayout()
        Me.tlpAudio.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tlpMain.SuspendLayout()
        Me.pnTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tpAudio)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(869, 419)
        Me.tcMain.TabIndex = 5
        '
        'tpAudio
        '
        Me.tpAudio.Controls.Add(Me.tlpAudio)
        Me.tpAudio.Location = New System.Drawing.Point(4, 24)
        Me.tpAudio.Margin = New System.Windows.Forms.Padding(2)
        Me.tpAudio.Name = "tpAudio"
        Me.tpAudio.Padding = New System.Windows.Forms.Padding(2)
        Me.tpAudio.Size = New System.Drawing.Size(861, 391)
        Me.tpAudio.TabIndex = 4
        Me.tpAudio.Text = "   Audio   "
        Me.tpAudio.UseVisualStyleBackColor = True
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 2
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.Controls.Add(Me.flpAudio, 1, 0)
        Me.tlpAudio.Controls.Add(Me.dgvAudio, 0, 0)
        Me.tlpAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(2, 2)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 1
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAudio.Size = New System.Drawing.Size(857, 387)
        Me.tlpAudio.TabIndex = 7
        '
        'flpAudio
        '
        Me.flpAudio.AutoSize = True
        Me.flpAudio.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAudio.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpAudio.Location = New System.Drawing.Point(857, 0)
        Me.flpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAudio.Name = "flpAudio"
        Me.flpAudio.Size = New System.Drawing.Size(0, 0)
        Me.flpAudio.TabIndex = 6
        '
        'dgvAudio
        '
        Me.dgvAudio.AllowDrop = True
        Me.dgvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAudio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAudio.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2
        Me.dgvAudio.Location = New System.Drawing.Point(0, 0)
        Me.dgvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.RowHeadersWidth = 123
        Me.dgvAudio.RowTemplate.Height = 28
        Me.dgvAudio.Size = New System.Drawing.Size(857, 387)
        Me.dgvAudio.TabIndex = 0
        '
        'bnAudioAdd
        '
        Me.bnAudioAdd.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioAdd.Location = New System.Drawing.Point(44, 431)
        Me.bnAudioAdd.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioAdd.Text = "   Add..."
        '
        'bnAudioRemove
        '
        Me.bnAudioRemove.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioRemove.Location = New System.Drawing.Point(143, 431)
        Me.bnAudioRemove.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioRemove.Text = "  Remove"
        '
        'bnAudioRemoveAll
        '
        Me.bnAudioRemoveAll.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioRemoveAll.Location = New System.Drawing.Point(242, 431)
        Me.bnAudioRemoveAll.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioRemoveAll.Text = "Remove All"
        Me.bnAudioRemoveAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bnAudioUp
        '
        Me.bnAudioUp.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioUp.Location = New System.Drawing.Point(341, 431)
        Me.bnAudioUp.Size = New System.Drawing.Size(66, 26)
        Me.bnAudioUp.Text = " Up"
        '
        'bnAudioDown
        '
        Me.bnAudioDown.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioDown.Location = New System.Drawing.Point(413, 431)
        Me.bnAudioDown.Size = New System.Drawing.Size(66, 26)
        Me.bnAudioDown.Text = "     Down"
        '
        'bnAudioPlay
        '
        Me.bnAudioPlay.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioPlay.Location = New System.Drawing.Point(485, 431)
        Me.bnAudioPlay.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioPlay.Text = "  Play"
        '
        'bnAudioEncodeSelected
        '
        Me.bnAudioEncodeSelected.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioEncodeSelected.Location = New System.Drawing.Point(683, 431)
        Me.bnAudioEncodeSelected.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioEncodeSelected.Text = " Encode"
        '
        'bnAudioEncodeAll
        '
        Me.bnAudioEncodeAll.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioEncodeAll.Location = New System.Drawing.Point(782, 431)
        Me.bnAudioEncodeAll.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioEncodeAll.Text = "Encode All"
        Me.bnAudioEncodeAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bnAudioEdit
        '
        Me.bnAudioEdit.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioEdit.Location = New System.Drawing.Point(584, 431)
        Me.bnAudioEdit.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioEdit.Text = "   Edit..."
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 10
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.bnMenuAudio, 0, 1)
        Me.tlpMain.Controls.Add(Me.pnTab, 0, 0)
        Me.tlpMain.Controls.Add(Me.bnAudioAdd, 1, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioUp, 4, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioEdit, 7, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioDown, 5, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioPlay, 6, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioEncodeSelected, 8, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioEncodeAll, 9, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioRemoveAll, 3, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioRemove, 2, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tlpMain.RowCount = 2
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(880, 463)
        Me.tlpMain.TabIndex = 8
        '
        'bnMenuAudio
        '
        Me.bnMenuAudio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnMenuAudio.Location = New System.Drawing.Point(3, 431)
        Me.bnMenuAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenuAudio.ShowMenuSymbol = True
        Me.bnMenuAudio.Size = New System.Drawing.Size(38, 26)
        '
        'pnTab
        '
        Me.tlpMain.SetColumnSpan(Me.pnTab, 10)
        Me.pnTab.Controls.Add(Me.tcMain)
        Me.pnTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnTab.Location = New System.Drawing.Point(6, 6)
        Me.pnTab.Name = "pnTab"
        Me.pnTab.Size = New System.Drawing.Size(869, 419)
        Me.pnTab.TabIndex = 8
        '
        'AudioConverterForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(880, 463)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Name = "AudioConverterForm"
        Me.Text = "Audio Converter"
        Me.tcMain.ResumeLayout(False)
        Me.tpAudio.ResumeLayout(False)
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tlpMain.ResumeLayout(False)
        Me.pnTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private AudioBindingSource As New BindingSource

    Sub New()
        MyBase.New()
        InitializeComponent()
        SetMinimumSize(30, 21)
        RestoreClientSize(45, 22)

        AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks)

        dgvAudio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvAudio.MultiSelect = True
        dgvAudio.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAudio.AllowUserToResizeRows = False
        dgvAudio.RowHeadersVisible = False
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvAudio.AutoGenerateColumns = True
        dgvAudio.DataSource = AudioBindingSource
        dgvAudio.AllowUserToDeleteRows = True
        dgvAudio.AllowUserToOrderColumns = True
        dgvAudio.AllowUserToResizeColumns = True
        dgvAudio.AllowDrop = True
        dgvAudio.ReadOnly = True

        bnAudioAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnAudioRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnAudioPlay.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnAudioUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnAudioDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnAudioEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)
        bnAudioEncodeSelected.Image = ImageHelp.GetSymbolImage(Symbol.MusicNote)
        bnAudioEncodeAll.Image = ImageHelp.GetSymbolImage(Symbol.MusicInfo)
        bnAudioRemoveAll.Image = ImageHelp.GetSymbolImage(Symbol.Clear)

        For Each bn In {bnAudioAdd, bnAudioRemove, bnAudioPlay, bnAudioUp,
                        bnAudioDown, bnAudioRemoveAll, bnAudioEdit, bnAudioEncodeAll, bnAudioEncodeSelected}

            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        Dim profileName = dgvAudio.AddTextBoxColumn()
        profileName.DataPropertyName = "Name"
        profileName.HeaderText = "Profile"
        profileName.ReadOnly = True

        Dim pathColumn = dgvAudio.AddTextBoxColumn()
        pathColumn.DataPropertyName = "DisplayName"
        pathColumn.HeaderText = "Track"
        pathColumn.ReadOnly = True

        If dgvAudio.RowCount > 0 Then
            dgvAudio.Rows(0).Selected = True
        End If


        Dim cms As New ContextMenuStripEx(components)
        bnMenuAudio.ContextMenuStrip = cms

        'cms.Add("Copy Command Line", Sub() Clipboard.SetText(TempProfile.GetCommandLine(True))).SetImage(Symbol.Copy)
        'cms.Add("Execute Command Line", AddressOf Execute).SetImage(Symbol.fa_terminal)
        'cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Line", TempProfile.GetCommandLine(True)))
        cms.Add("-")
        'cms.Add("Save Profile...", AddressOf SaveProfile, "Saves the current settings as profile").SetImage(Symbol.Save)
        cms.Add("-")
        'cms.Add("Help", AddressOf ShowHelp).SetImage(Symbol.Help)
        cms.Add("eac3to Help", Sub() g.ShellExecute("http://en.wikibooks.org/wiki/Eac3to"))
        cms.Add("ffmpeg Help", Sub() Package.ffmpeg.ShowHelp())
        cms.Add("qaac Help", Sub() Package.qaac.ShowHelp())
        cms.Add("qaac Formats", Sub() MsgInfo(ProcessHelp.GetConsoleOutput(Package.qaac.Path, "--formats")))
        cms.Add("Opus Help", Sub() Package.OpusEnc.ShowHelp())

        'TipProvider.SetTip("Profile name that is auto generated when undefined.", laProfileName)
        'TipProvider.SetTip("Delay in milliseconds. eac3to handles delay, ffmpeg don't but it is handled by the muxer. Saved in projects/templates but not in profiles.", numDelay, lDelay)
        'TipProvider.SetTip("Track name used by the muxer.", tbStreamName, laStreamName)
        'TipProvider.SetTip("Default MKV Track.", cbDefaultTrack)

    End Sub


    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Dim lastAction As Action
        lastAction?.Invoke
        bnAudioAdd.Select()
        UpdateControls()
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        's.Storage.SetInt("last selected AudioConverter tab", tcMain.SelectedIndex)

        'If DialogResult = DialogResult.OK Then
        'p.AudioTracks = DirectCast(AudioBindingSource.DataSource, List(Of AudioProfile))
        'Else

        dgvAudio.Rows.Clear()
        dgvAudio.Dispose()
        Me.Dispose(True)

        'End If
    End Sub

    Public Shared Function OpenAudioConverterDialog() As DialogResult
        Using form As New AudioConverterForm
            Return form.ShowDialog()
        End Using
    End Function

    Sub UpdateControls()
        bnAudioRemove.Enabled = dgvAudio.SelectedRows.Count > 0
        bnAudioPlay.Enabled = dgvAudio.SelectedRows.Count = 1
        bnAudioEdit.Enabled = dgvAudio.SelectedRows.Count = 1
        bnAudioRemoveAll.Enabled = dgvAudio.Rows.Count > 0
        bnAudioEncodeAll.Enabled = dgvAudio.Rows.Count > 0
        bnAudioEncodeSelected.Enabled = dgvAudio.SelectedRows.Count > 0
        bnAudioUp.Enabled = dgvAudio.CanMoveUp AndAlso dgvAudio.SelectedRows.Count = 1
        bnAudioDown.Enabled = dgvAudio.CanMoveDown AndAlso dgvAudio.SelectedRows.Count = 1
        'dgvAudio.ReadOnly = True

    End Sub
    Private Sub bnAudioEncodeAll_Click(sender As Object, e As EventArgs) Handles bnAudioEncodeAll.Click
        Dim OutPath As String
        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select output directory or cancel to use source path"
            dialog.UseDescriptionForTitle = True
            dialog.RootFolder = Environment.SpecialFolder.MyComputer
            dialog.SelectedPath = DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File.Dir

            If dialog.ShowDialog = DialogResult.OK Then
                OutPath = dialog.SelectedPath.FixDir
                'Else
                '  dialog.Dispose()
                '  Exit Sub
            End If
        End Using

        If MsgQuestion("Confirm to process all tracks") = DialogResult.OK Then
            If OutPath Is Nothing Then OutPath = ""
            Try
                Dim actions As New List(Of Action)
                For Each af In AudioBindingSource
                    actions.Add(Sub() EncodeAudio(DirectCast(af, AudioProfile), OutPath))
                Next
                Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = s.ParallelProcsNum}, actions.ToArray)
            Catch ex As AggregateException
                ExceptionDispatchInfo.Capture(ex.InnerExceptions(0)).Throw()
            End Try
        End If
    End Sub

    Private Sub bnAudioEncodeSelected_Click(sender As Object, e As EventArgs) Handles bnAudioEncodeSelected.Click
        Dim OutPath As String
        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select output directory or cancel to use source path"
            dialog.UseDescriptionForTitle = True
            dialog.RootFolder = Environment.SpecialFolder.MyComputer
            dialog.SelectedPath = DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File.Dir

            If dialog.ShowDialog = DialogResult.OK Then
                OutPath = dialog.SelectedPath.FixDir
                'Else
                'dialog.Dispose()
                'Exit Sub
            End If
        End Using

        If OutPath Is Nothing Then OutPath = ""
        If MsgQuestion("Confirm to process selected track") = DialogResult.OK Then
            EncodeAudio(DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile), OutPath)
        End If
    End Sub
    Private Sub EncodeAudio(ap As AudioProfile, Optional OutPath As String = "")
        Try
            If p.TempDir = "" OrElse p.TempDir.Contains("%") Then
                p.TempDir = ap.File.Dir
            End If
            If OutPath <> "" Then
                p.TempDir = OutPath
            End If

            ap = ObjectHelp.GetCopy(ap)
            Audio.Process(ap)
            ap.Encode()
            p.SourceFile = p.TempDir
            Log.Save(p)
        Catch
        End Try
    End Sub
    Sub AddAudio(path As String, ap As AudioProfile)
        'Dim ap As AudioProfile

        ap = ObjectHelp.GetCopy(ap)
        ap.File = path

        If FileTypes.VideoAudio.Contains(ap.File.Ext) Then
            ap.Streams = MediaInfo.GetAudioStreams(ap.File)
            ap.SetStreamOrLanguage()
        End If

        If Not ap.Stream Is Nothing Then
            Dim streamSelection As New SelectionBox(Of AudioStream)
            streamSelection.Title = "Stream Selection"
            streamSelection.Text = "Please select an audio stream."

            For Each stream In ap.Streams
                streamSelection.AddItem(stream)
            Next

            If streamSelection.Show <> DialogResult.OK Then
                Exit Sub
            End If

            ap.Stream = streamSelection.SelectedValue
        End If

        'g.MainForm.UpdateSizeOrBitrate()
        AudioBindingSource.Add(ap)
        AudioBindingSource.Position = AudioBindingSource.Count - 1

    End Sub

    Sub ProcessInputAudioFiles(files As String())
        If files.Count > 2000 Then
            If MsgQuestion("Are you sure to add this many (" & files.Count & ")files ?") = DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        Dim vfc = (files.Where(Function(file) FileTypes.VideoAudio.ContainsAny(file.Ext))).Count
        If vfc > 20 Then
            If MsgQuestion("Are you sure to add this many (" & vfc & ") video files ?") = DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        If files.Count > 0 Then
            Dim ap As AudioProfile

            Dim profileSelection As New SelectionBox(Of AudioProfile)
            profileSelection.Title = "Please select Audio Profile"

            If Not TypeOf p.Audio0 Is NullAudioProfile Then
                profileSelection.AddItem("Current Project 1: " & p.Audio0.ToString, p.Audio0)
            End If
            If Not TypeOf p.Audio1 Is NullAudioProfile Then
                profileSelection.AddItem("Current Project 2: " & p.Audio1.ToString, p.Audio1)
            End If
            For Each AudioProfile In s.AudioProfiles
                profileSelection.AddItem(AudioProfile)
            Next

            If profileSelection.Show <> DialogResult.OK Then
                Exit Sub
            End If

            ap = profileSelection.SelectedValue

            For Each path In files
                AddAudio(path, ap)
            Next
        End If

    End Sub

    Sub bnAudioAdd_Click(sender As Object, e As EventArgs) Handles bnAudioAdd.Click

        Using dialog As New OpenFileDialog
            dialog.Multiselect = True
            Dim ft = FileTypes.Audio.Union(FileTypes.VideoAudio)
            'dialog.SetFilter(fset))
            dialog.Filter = "*." + ft.Join(";*.") + "|*." + ft.Join(";*.")
            dialog.SetInitDir(p.TempDir)

            If dialog.ShowDialog = DialogResult.OK Then

                Dim av = dialog.FileNames.Where(Function(file) ft.ContainsAny(file.Ext))
                'Dim af= 
                ProcessInputAudioFiles(av.ToArray)

            End If
        End Using

        UpdateControls()
    End Sub
    Sub dgvAudio_DragEnter(sender As Object, e As DragEventArgs) Handles dgvAudio.DragEnter
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            If FileTypes.VideoAudio.Union(FileTypes.Audio).ContainsAny(files.Select(Function(item) item.Ext)) Then
                e.Effect = DragDropEffects.Copy
            End If
        End If
    End Sub
    Sub dgvAudio_DragDrop(sender As Object, e As DragEventArgs) Handles dgvAudio.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            ProcessInputAudioFiles(files)
        End If

        UpdateControls()
    End Sub

    Sub bnAudioEdit_Click(sender As Object, e As EventArgs) Handles bnAudioEdit.Click

        AudioBindingSource.ResetBindings(False)

        Dim ap = DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile)

        ap.EditProject()
        g.MainForm.UpdateAudioMenu()
        g.MainForm.UpdateSizeOrBitrate()
        AudioBindingSource.ResetBindings(False)
        UpdateControls()

        If AudioProfile.AudioEditDialogResult = DialogResult.OK AndAlso dgvAudio.Rows.Count > 1 Then
            If MsgQuestion("Apply to all files?", TaskDialogButtons.YesNo) = DialogResult.Yes Then

                dgvAudio.SelectAll()
                For i = 0 To dgvAudio.SelectedRows.Count - 1
                    AudioBindingSource(dgvAudio.SelectedRows(i).Index) = ap
                Next

                g.MainForm.UpdateAudioMenu()
                g.MainForm.UpdateSizeOrBitrate()
                AudioBindingSource.ResetBindings(False)
                UpdateControls()
            End If
        End If
    End Sub

    Sub bnAudioRemove_Click(sender As Object, e As EventArgs) Handles bnAudioRemove.Click
        dgvAudio.RemoveSelection
        UpdateControls()
    End Sub

    Private Sub bnAudioRemoveAll_Click(sender As Object, e As EventArgs) Handles bnAudioRemoveAll.Click
        If dgvAudio.Rows.Count > 1 Then
            If MsgQuestion("Remove all ?") = DialogResult.OK Then
                dgvAudio.Rows.Clear()
                UpdateControls()
            End If
        Else
            dgvAudio.Rows.Clear()
            UpdateControls()
        End If
    End Sub

    Private Sub dgvAudio_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles dgvAudio.UserDeletedRow
        If dgvAudio.Rows.Count = 0 Then
            dgvAudio.Rows.Clear()
        End If
        UpdateControls()
    End Sub

    Sub dgvAudio_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvAudio.MouseUp
        UpdateControls()
    End Sub

    Sub bnAudioUp_Click(sender As Object, e As EventArgs) Handles bnAudioUp.Click
        dgvAudio.MoveSelectionUp
        UpdateControls()
    End Sub

    Sub bnAudioDown_Click(sender As Object, e As EventArgs) Handles bnAudioDown.Click
        dgvAudio.MoveSelectionDown
        UpdateControls()
    End Sub
    Sub bnAudioPlay_Click(sender As Object, e As EventArgs) Handles bnAudioPlay.Click
        g.Play(DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File)
    End Sub

    Private Sub dgvAudio_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAudio.SelectionChanged
        UpdateControls()
    End Sub

    Private Sub dgvAudio_KeyUp(sender As Object, e As KeyEventArgs) Handles dgvAudio.KeyUp
        UpdateControls()
    End Sub

    Private Sub dgvAudio_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAudio.CellContentDoubleClick
        dgvAudio.ReadOnly = False
    End Sub

    Private Sub dgvAudio_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvAudio.CellMouseClick
        dgvAudio.ReadOnly = True
    End Sub

    'Private Sub bnMenuAudio_Click(sender As Object, e As EventArgs) Handles bnMenuAudio.Click
    'bnCommandLinePreview_Click(sender As Object, e As EventArgs) Handles bnCommandLinePreview.Click

    'g.ShowCommandLinePreview("Command Line", AudioConverter.GetCommandLine)
    'End Sub
End Class
