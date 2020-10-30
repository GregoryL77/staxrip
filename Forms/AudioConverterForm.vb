
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
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents pnTab As Panel
    Friend WithEvents bnAudioConvert As ButtonEx
    Friend WithEvents bnMenuAudio As ButtonEx
    Friend WithEvents laAC As Label
    Friend WithEvents flpAudio As FlowLayoutPanel
    Friend WithEvents numThreads As NumEdit
    Friend WithEvents laThreads As Label

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.numThreads = New StaxRip.UI.NumEdit()
        Me.laThreads = New System.Windows.Forms.Label()
        Me.bnAudioRemove = New StaxRip.UI.ButtonEx()
        Me.bnAudioConvert = New StaxRip.UI.ButtonEx()
        Me.bnAudioEdit = New StaxRip.UI.ButtonEx()
        Me.laAC = New System.Windows.Forms.Label()
        Me.bnMenuAudio = New StaxRip.UI.ButtonEx()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpAudio = New System.Windows.Forms.TabPage()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAudio = New System.Windows.Forms.FlowLayoutPanel()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.bnAudioAdd = New StaxRip.UI.ButtonEx()
        Me.bnAudioUp = New StaxRip.UI.ButtonEx()
        Me.bnAudioDown = New StaxRip.UI.ButtonEx()
        Me.bnAudioPlay = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnTab = New System.Windows.Forms.Panel()
        Me.tcMain.SuspendLayout()
        Me.tpAudio.SuspendLayout()
        Me.tlpAudio.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tlpMain.SuspendLayout()
        Me.pnTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'numThreads
        '
        Me.numThreads.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.numThreads.BackColor = System.Drawing.SystemColors.Control
        Me.numThreads.Location = New System.Drawing.Point(584, 328)
        Me.numThreads.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.numThreads.Maximum = 32.0R
        Me.numThreads.Minimum = 1.0R
        Me.numThreads.Name = "numThreads"
        Me.numThreads.Size = New System.Drawing.Size(40, 20)
        Me.numThreads.TabIndex = 4
        Me.numThreads.TabStop = False
        Me.numThreads.Tag = "No. threads"
        Me.TipProvider.SetTipText(Me.numThreads, "Number of parallel processes, set the default in settings")
        Me.numThreads.Value = 2.0R
        '
        'laThreads
        '
        Me.laThreads.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laThreads.AutoSize = True
        Me.laThreads.BackColor = System.Drawing.SystemColors.Control
        Me.laThreads.Location = New System.Drawing.Point(524, 328)
        Me.laThreads.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.laThreads.Name = "laThreads"
        Me.laThreads.Size = New System.Drawing.Size(54, 18)
        Me.laThreads.TabIndex = 19
        Me.laThreads.Text = "Threads :"
        Me.laThreads.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TipProvider.SetTipText(Me.laThreads, "Number of parallel processes, set the default in settings")
        '
        'bnAudioRemove
        '
        Me.bnAudioRemove.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioRemove.Location = New System.Drawing.Point(143, 351)
        Me.bnAudioRemove.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioRemove.Text = "  Remove"
        Me.TipProvider.SetTipText(Me.bnAudioRemove, "Removes Selection <Delete>")
        '
        'bnAudioConvert
        '
        Me.bnAudioConvert.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioConvert.Location = New System.Drawing.Point(584, 351)
        Me.bnAudioConvert.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioConvert.Text = "     Convert..."
        Me.TipProvider.SetTipText(Me.bnAudioConvert, "Converts Selection")
        '
        'bnAudioEdit
        '
        Me.bnAudioEdit.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioEdit.Location = New System.Drawing.Point(485, 351)
        Me.bnAudioEdit.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioEdit.Text = "    Edit..."
        Me.TipProvider.SetTipText(Me.bnAudioEdit, "Edit assigned Audio Profile")
        '
        'laAC
        '
        Me.laAC.AutoSize = True
        Me.tlpMain.SetColumnSpan(Me.laAC, 5)
        Me.laAC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.laAC.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.laAC.Location = New System.Drawing.Point(44, 328)
        Me.laAC.Name = "laAC"
        Me.laAC.Size = New System.Drawing.Size(435, 20)
        Me.laAC.TabIndex = 18
        Me.laAC.Text = "Please add or drag music files..."
        Me.TipProvider.SetTipText(Me.laAC, "Double Click change resize mode")
        '
        'bnMenuAudio
        '
        Me.bnMenuAudio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnMenuAudio.Location = New System.Drawing.Point(3, 351)
        Me.bnMenuAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenuAudio.ShowMenuSymbol = True
        Me.bnMenuAudio.Size = New System.Drawing.Size(38, 26)
        Me.TipProvider.SetTipText(Me.bnMenuAudio, "Click to open menu")
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tpAudio)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.ItemSize = New System.Drawing.Size(62, 15)
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(674, 324)
        Me.tcMain.TabIndex = 5
        Me.tcMain.TabStop = False
        '
        'tpAudio
        '
        Me.tpAudio.Controls.Add(Me.tlpAudio)
        Me.tpAudio.Location = New System.Drawing.Point(4, 19)
        Me.tpAudio.Margin = New System.Windows.Forms.Padding(2, 2, 2, 0)
        Me.tpAudio.Name = "tpAudio"
        Me.tpAudio.Padding = New System.Windows.Forms.Padding(2, 2, 2, 0)
        Me.tpAudio.Size = New System.Drawing.Size(666, 301)
        Me.tpAudio.TabIndex = 4
        Me.tpAudio.Text = "   Audio   "
        Me.tpAudio.UseVisualStyleBackColor = True
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 2
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.Controls.Add(Me.flpAudio, 1, 0)
        Me.tlpAudio.Controls.Add(Me.dgvAudio, 0, 0)
        Me.tlpAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(2, 2)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 1
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.Size = New System.Drawing.Size(662, 299)
        Me.tlpAudio.TabIndex = 7
        '
        'flpAudio
        '
        Me.flpAudio.AutoSize = True
        Me.flpAudio.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAudio.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpAudio.Location = New System.Drawing.Point(662, 0)
        Me.flpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAudio.Name = "flpAudio"
        Me.flpAudio.Size = New System.Drawing.Size(0, 0)
        Me.flpAudio.TabIndex = 6
        '
        'dgvAudio
        '
        Me.dgvAudio.AllowDrop = True
        Me.dgvAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAudio.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2
        Me.dgvAudio.Location = New System.Drawing.Point(0, 0)
        Me.dgvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.RowHeadersWidth = 22
        Me.dgvAudio.Size = New System.Drawing.Size(662, 299)
        Me.dgvAudio.StandardTab = True
        Me.dgvAudio.TabIndex = 1
        '
        'bnAudioAdd
        '
        Me.bnAudioAdd.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioAdd.Location = New System.Drawing.Point(44, 351)
        Me.bnAudioAdd.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioAdd.Text = "    Add..."
        '
        'bnAudioUp
        '
        Me.bnAudioUp.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioUp.Location = New System.Drawing.Point(242, 351)
        Me.bnAudioUp.Size = New System.Drawing.Size(66, 26)
        Me.bnAudioUp.Text = " Up"
        '
        'bnAudioDown
        '
        Me.bnAudioDown.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioDown.Location = New System.Drawing.Point(314, 351)
        Me.bnAudioDown.Size = New System.Drawing.Size(66, 26)
        Me.bnAudioDown.Text = "     Down"
        '
        'bnAudioPlay
        '
        Me.bnAudioPlay.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnAudioPlay.Location = New System.Drawing.Point(386, 351)
        Me.bnAudioPlay.Size = New System.Drawing.Size(93, 26)
        Me.bnAudioPlay.Text = "  Play"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 8
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.laAC, 1, 1)
        Me.tlpMain.Controls.Add(Me.bnMenuAudio, 0, 2)
        Me.tlpMain.Controls.Add(Me.pnTab, 0, 0)
        Me.tlpMain.Controls.Add(Me.bnAudioAdd, 1, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioRemove, 2, 2)
        Me.tlpMain.Controls.Add(Me.laThreads, 6, 1)
        Me.tlpMain.Controls.Add(Me.numThreads, 7, 1)
        Me.tlpMain.Controls.Add(Me.bnAudioUp, 3, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioDown, 4, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioPlay, 5, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioEdit, 6, 2)
        Me.tlpMain.Controls.Add(Me.bnAudioConvert, 7, 2)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(1)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(3, 2, 2, 3)
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(681, 383)
        Me.tlpMain.TabIndex = 2
        Me.tlpMain.TabStop = True
        '
        'pnTab
        '
        Me.tlpMain.SetColumnSpan(Me.pnTab, 10)
        Me.pnTab.Controls.Add(Me.tcMain)
        Me.pnTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnTab.Location = New System.Drawing.Point(5, 4)
        Me.pnTab.Margin = New System.Windows.Forms.Padding(2, 2, 1, 0)
        Me.pnTab.Name = "pnTab"
        Me.pnTab.Size = New System.Drawing.Size(674, 324)
        Me.pnTab.TabIndex = 8
        '
        'AudioConverterForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(681, 383)
        Me.Controls.Add(Me.tlpMain)
        Me.DoubleBuffered = True
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
        Me.tlpMain.PerformLayout()
        Me.pnTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private AudioBindingSource As New BindingSource
    Private MaxThreads As Integer
    Private AutoStream As Boolean
    Sub New()
        MyBase.New()
        InitializeComponent()
        SetMinimumSize(40, 18)
        RestoreClientSize(45, 25)

        AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks)

        dgvAudio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAudio.MultiSelect = True
        dgvAudio.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAudio.AllowUserToResizeRows = False
        dgvAudio.RowHeadersVisible = True
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvAudio.AutoGenerateColumns = False
        dgvAudio.DataSource = AudioBindingSource
        dgvAudio.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        dgvAudio.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing
        dgvAudio.RowTemplate.Height = 20
        dgvAudio.RowHeadersWidth = 64

        dgvAudio.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        dgvAudio.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvAudio.RowHeadersDefaultCellStyle.Padding = Padding.Empty
        dgvAudio.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False

        dgvAudio.AllowUserToDeleteRows = True
        dgvAudio.AllowUserToOrderColumns = True
        dgvAudio.AllowUserToResizeColumns = True

        dgvAudio.AllowDrop = True
        'dgvAudio.ReadOnly = True

        bnAudioAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnAudioRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnAudioPlay.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnAudioUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnAudioDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnAudioEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)
        bnAudioConvert.Image = ImageHelp.GetSymbolImage(Symbol.MusicInfo)

        For Each bn In {bnAudioAdd, bnAudioRemove, bnAudioPlay, bnAudioUp,
                        bnAudioDown, bnAudioEdit, bnAudioConvert}

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
        profileName.FillWeight = 10

        Dim dispNameColumn = dgvAudio.AddTextBoxColumn()
        dispNameColumn.DataPropertyName = "DisplayName"
        dispNameColumn.HeaderText = "Track"
        dispNameColumn.ReadOnly = True
        dispNameColumn.FillWeight = 40

        Dim pathColumn = dgvAudio.AddTextBoxColumn()
        pathColumn.DataPropertyName = "File"
        pathColumn.HeaderText = "Full Path"
        pathColumn.ReadOnly = True
        pathColumn.FillWeight = 50

        For Each col As DataGridViewColumn In dgvAudio.Columns
            col.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        MaxThreads = s.ParallelProcsNum
        numThreads.Value = MaxThreads

        Dim cms As New ContextMenuStripEx(components)
        bnMenuAudio.ContextMenuStrip = cms
        cms.Add("Select all  <Ctrl+A>", Sub() dgvAudio.SelectAll()).SetImage(Symbol.SelectAll)
        cms.Add("Remove all", Sub() dgvAudio.Rows.Clear()).SetImage(Symbol.Clear)
        cms.Add("Save LOG...", Sub() SaveConverterLog(UseDialog:=True)).SetImage(Symbol.Save)
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

        'Log = New LogBuilder
    End Sub



    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Dim lastAction As Action
        lastAction?.Invoke
        UpdateControls()
        bnAudioAdd.Select()
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        SaveConverterLog()
        MyBase.OnFormClosed(e)
        AudioBindingSource.Dispose()
        AudioBindingSource.Clear()
        dgvAudio.Rows.Clear()
        dgvAudio.Columns.Clear()
    End Sub

    Public Shared Function OpenAudioConverterDialog() As DialogResult 'Debug
        Using form As New AudioConverterForm
            Return form.ShowDialog()
        End Using
    End Function

    Sub UpdateControls()
        bnAudioRemove.Enabled = dgvAudio.SelectedRows.Count > 0
        bnAudioPlay.Enabled = dgvAudio.SelectedRows.Count = 1
        bnAudioEdit.Enabled = dgvAudio.SelectedRows.Count > 0 'AndAlso dgvAudio.SelectedRows.Count < 1000
        bnAudioConvert.Enabled = dgvAudio.SelectedRows.Count > 0
        bnAudioUp.Enabled = dgvAudio.CanMoveUp AndAlso dgvAudio.SelectedRows.Count = 1
        bnAudioDown.Enabled = dgvAudio.CanMoveDown AndAlso dgvAudio.SelectedRows.Count = 1
        numThreads.Enabled = dgvAudio.SelectedRows.Count > 1

        If dgvAudio.Rows.Count > 0 Then
            laAC.Text = "Pos: " & AudioBindingSource?.Position & "   |   Sel: " & dgvAudio.SelectedRows.Count & " / Tot: " & dgvAudio.Rows.Count
        Else
            laAC.Text = "Please add or drag music files..."
        End If
        'dgvAudio.ReadOnly = True
    End Sub

    Sub AutoResizeColumns()

        For Each r As DataGridViewRow In dgvAudio.Rows
            r.HeaderCell.Value = (r.Index).ToString
        Next

        dgvAudio.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)
        dgvAudio.AutoResizeColumnHeadersHeight()
        dgvAudio.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        'dgvAudio.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
    End Sub
    Sub SaveConverterLog(Optional LogPath As String = "", Optional UseDialog As Boolean = False)
        If Not Log.IsEmpty Then
            Log.Save()
            If UseDialog Then
                Using dialog As New FolderBrowserDialog
                    dialog.Description = "Please select LOG File location :"
                    dialog.UseDescriptionForTitle = True
                    dialog.RootFolder = Environment.SpecialFolder.MyComputer
                    If p.TempDir <> "" AndAlso Not p.TempDir.Contains("%") Then dialog.SetSelectedPath(p.TempDir)
                    If dialog.ShowDialog = DialogResult.OK Then
                        LogPath = dialog.SelectedPath
                    End If
                End Using
            End If

            If LogPath <> "" Then
                Try
                    Dim lName = "\" & Date.Now & "_staxrip.log"
                    If File.Exists(Log.GetPath) Then
                        If File.Exists(LogPath & lName) Then File.Delete(LogPath & lName)
                        File.Move(Log.GetPath, LogPath & lName)
                    End If
                Catch ex As Exception
                End Try
            End If
        ElseIf UseDialog Then
            MsgInfo("Log is empty")
        End If
    End Sub

    Public Shared Function GetCPUCount() As Integer
        Try
            Dim val = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS")
            Return If(val.IsInt, val.ToInt, s.ParallelProcsNum)
        Catch ex As Exception
            Return s.ParallelProcsNum
        End Try
    End Function

    Private Sub EncodeAudio(ap As AudioProfile, Optional OutPath As String = "")
        If p.TempDir = "" OrElse p.TempDir.Contains("%") Then
            p.TempDir = ap.File.Dir
        End If
        If OutPath <> "" Then
            p.TempDir = OutPath
        End If

        ap = ObjectHelp.GetCopy(ap)
        Audio.Process(ap)
        ap.Encode()
        'SaveConverterLog(OutPath)
    End Sub
    Private Sub bnAudioConvert_Click(sender As Object, e As EventArgs) Handles bnAudioConvert.Click
        Dim OutPath As String
        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select output directory or cancel to use source path"
            dialog.UseDescriptionForTitle = True
            dialog.RootFolder = Environment.SpecialFolder.MyComputer
            dialog.SetSelectedPath(DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File.Dir)

            If dialog.ShowDialog = DialogResult.OK Then
                OutPath = dialog.SelectedPath.FixDir
            End If
        End Using

        If MsgQuestion("Confirm to process selected tracks") = DialogResult.OK Then
            If OutPath Is Nothing Then OutPath = ""
            Try
                Dim actions As New List(Of Action)
                'Dim bs As Object
                'For i = 0 To dgvAudio.SelectedRows.Count - 1  --->   To DO : encode in gridview Order
                'bs = AudioBindingSource(dgvAudio. >selectedRows(i) < .Index)
                'actions.Add(Sub() EncodeAudio(DirectCast(bs, AudioProfile), OutPath))
                'Next
                For Each sr As DataGridViewRow In dgvAudio.SelectedRows
                    Dim srbs As Object = AudioBindingSource(dgvAudio.Rows(sr.Index).Index)
                    'EncodeAudio(DirectCast(srbs, AudioProfile), OutPath)
                    actions.Add(Sub() EncodeAudio(DirectCast(srbs, AudioProfile), OutPath))
                Next

                Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = MaxThreads}, actions.ToArray)

                For Each sr As DataGridViewRow In dgvAudio.SelectedRows
                    Dim srbs As Object = AudioBindingSource(dgvAudio.Rows(sr.Index).Index)
                    Dim ap As AudioProfile = DirectCast(srbs, AudioProfile)
                    If File.Exists(If(OutPath = "", ap.GetOutputFile(), OutPath & ap.GetOutputFile.FileName())) Then
                        sr.HeaderCell.Value = "OK"
                    End If
                Next

                SaveConverterLog(OutPath)
            Catch ex As AggregateException
                SaveConverterLog(OutPath)
                ExceptionDispatchInfo.Capture(ex.InnerExceptions(0)).Throw()
            End Try
        End If
    End Sub

    Sub AddAudio(path As String, ap As AudioProfile)
        ap = ObjectHelp.GetCopy(ap)
        ap.File = path

        If FileTypes.VideoAudio.Contains(ap.File.Ext) Then
            ap.Streams = MediaInfo.GetAudioStreams(ap.File)
            If ap.Streams.Count > 0 Then
                ap.Stream = ap.Streams(0)
            End If

            If Not AutoStream AndAlso ap.Streams.Count > 1 AndAlso Not ap.Stream Is Nothing Then
                Dim streamSelection As New SelectionBox(Of AudioStream)
                Dim NullDummyStream As New AudioStream
                NullDummyStream.Index = 30574
                streamSelection.Title = "Stream Selection"
                streamSelection.Text = "Please select an audio stream for: " & BR & ap.File.ToString.ShortBegEnd(60, 60)

                For Each stream In ap.Streams
                    streamSelection.AddItem(stream)
                Next

                streamSelection.AddItem(" > Use first stream and don't ask me again <", NullDummyStream)

                If streamSelection.Show <> DialogResult.OK Then
                    Exit Sub
                End If

                If streamSelection.SelectedValue IsNot NullDummyStream Then
                    ap.Stream = streamSelection.SelectedValue
                Else
                    AutoStream = True
                End If

            End If
        End If

        AudioBindingSource.Add(ap)
    End Sub

    Sub ProcessInputAudioFiles(files As String())
        If files.Count > 100 Then
            If MsgQuestion("Add " & files.Count & " files ?") <> DialogResult.OK Then
                Exit Sub
            End If
        End If

        If files.Count > 0 Then
            Dim OldLastIDX = If(dgvAudio.Rows.Count > 0, dgvAudio.Rows(dgvAudio.Rows.Count - 1).Index, 0)

            Dim ap As AudioProfile

            Dim profileSelection As New SelectionBox(Of AudioProfile)
            SelectionBoxForm.StartPosition = FormStartPosition.CenterScreen   'Drag Drop is out of center, still..
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

            AutoStream = False
            For Each path In files
                AddAudio(path, ap)
            Next

            'dgvAudio.Sort(dgvAudio.Columns(2), System.ComponentModel.ListSortDirection.Ascending)

            AudioBindingSource.Position = AudioBindingSource.Count - 1
            g.MainForm.UpdateSizeOrBitrate()
            UpdateControls()
            AutoResizeColumns()
            dgvAudio.Select()
            dgvAudio.Rows(OldLastIDX).Selected = True
        End If
    End Sub

    Sub bnAudioAdd_Click(sender As Object, e As EventArgs) Handles bnAudioAdd.Click
        Using td As New TaskDialog(Of String)
            Dim ftav = FileTypes.Audio.Union(FileTypes.VideoAudio)

            td.AddCommand("Add files", "files")
            td.AddCommand("Add folder", "folder")

            Select Case td.Show
                Case "files"
                    Using dialog As New OpenFileDialog
                        dialog.Multiselect = True
                        dialog.SetFilter(ftav)
                        If p.TempDir <> "" AndAlso Not p.TempDir.Contains("%") Then dialog.SetInitDir(p.TempDir)

                        If dialog.ShowDialog = DialogResult.OK Then
                            Dim av = dialog.FileNames.Where(Function(file) ftav.Contains(file.Ext))
                            ProcessInputAudioFiles(av.ToArray)
                        End If
                    End Using

                Case "folder"
                    Using dialog As New FolderBrowserDialog
                        dialog.RootFolder = Environment.SpecialFolder.MyComputer
                        If p.TempDir <> "" AndAlso Not p.TempDir.Contains("%") Then dialog.SetSelectedPath(p.TempDir)

                        If dialog.ShowDialog = DialogResult.OK Then
                            Dim subfolders = Directory.GetDirectories(dialog.SelectedPath)
                            Dim opt = SearchOption.TopDirectoryOnly

                            If Directory.GetDirectories(dialog.SelectedPath).Count > 0 Then
                                If MsgQuestion("Include sub folders?", TaskDialogButtons.YesNo) = DialogResult.Yes Then
                                    opt = SearchOption.AllDirectories
                                End If
                            End If

                            Dim avd = Directory.GetFiles(dialog.SelectedPath, "*.*", opt).Where(Function(val) ftav.Contains(val.Ext))
                            ProcessInputAudioFiles(avd.ToArray)
                        End If
                    End Using
            End Select
        End Using
    End Sub
    Sub dgvAudio_DragEnter(sender As Object, e As DragEventArgs) Handles dgvAudio.DragEnter
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            If FileTypes.VideoAudio.Union(FileTypes.Audio).ContainsAny(files.Select(Function(item) item.Ext)) Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        End If
    End Sub
    Sub dgvAudio_DragDrop(sender As Object, e As DragEventArgs) Handles dgvAudio.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            Dim ftav = FileTypes.Audio.Union(FileTypes.VideoAudio)
            Dim av = files.Where(Function(file) ftav.ContainsAny(file.Ext))
            ProcessInputAudioFiles(av.ToArray)
        End If
    End Sub

    Sub bnAudioEdit_Click(sender As Object, e As EventArgs) Handles bnAudioEdit.Click
        Dim sr = dgvAudio.SelectedRows
        Dim srlast = sr(sr.Count - 1) 'First in selecting order is used for editing
        'Dim ap0 = DirectCast(AudioBindingSource(sr(0).Index), AudioProfile)
        Dim ap0 = DirectCast(AudioBindingSource(srlast.Index), AudioProfile)
        ap0.EditProject()

        If AudioProfile.AudioEditDialogResult = DialogResult.OK Then
            g.MainForm.UpdateAudioMenu()
            g.MainForm.UpdateSizeOrBitrate()

            If sr.Count > 1 Then
                For i = 0 To sr.Count - 2
                    Dim ap = DirectCast(AudioBindingSource(sr(i).Index), AudioProfile)
                    ap0 = ObjectHelp.GetCopy(ap0)
                    ap0.File = ap.File
                    ap0.Stream = ap.Stream
                    ap0.Streams = ap.Streams
                    ap0.SourceSamplingRate = ap.SourceSamplingRate
                    AudioBindingSource(sr(i).Index) = ap0
                Next
            End If

            g.MainForm.UpdateAudioMenu()
            g.MainForm.UpdateSizeOrBitrate()

            Dim srCache(dgvAudio.Rows.Count) as Boolean  'not perfect...
            For Each oldSR As DataGridViewRow In sr
                srCache(oldSR.Index) = True
            Next

            AudioBindingSource.ResetBindings(False)

            For Each nRow As DataGridViewRow In dgvAudio.Rows
                If srCache(nRow.Index) = True Then
                    dgvAudio.Rows(nRow.Index).Selected = True
                End If
            Next

            AutoResizeColumns()
        End If
    End Sub

    Private Sub dgvAudio_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAudio.SelectionChanged
        UpdateControls()
    End Sub

    Sub bnAudioRemove_Click(sender As Object, e As EventArgs) Handles bnAudioRemove.Click
        If dgvAudio.Rows.Count = dgvAudio.SelectedRows.Count Then
            dgvAudio.Rows.Clear()
        End If
        dgvAudio.RemoveSelection
        AutoResizeColumns()
    End Sub
    Private Sub dgvAudio_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles dgvAudio.UserDeletedRow
        If dgvAudio.Rows.Count = dgvAudio.SelectedRows.Count Then
            dgvAudio.Rows.Clear()
        End If
        If dgvAudio.SelectedRows.Count = 0 AndAlso dgvAudio.RowCount > 0 Then
            dgvAudio.Rows(dgvAudio.RowCount - 1).Selected = True
        End If
        For Each r As DataGridViewRow In dgvAudio.Rows
            r.HeaderCell.Value = (r.Index).ToString
        Next
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

    Private Sub laAC_Click(sender As Object, e As EventArgs) Handles laAC.DoubleClick
        'AudioBindingSource.ResetBindings(False)

        If dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None Then
            dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        ElseIf dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill Then
            dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        End If

        Refresh()

        AutoResizeColumns()
    End Sub

    Private Sub numThreads_ValueChanged(numEdit As NumEdit) Handles numThreads.ValueChanged
        Static CPUCount As Integer = GetCPUCount()
        MaxThreads = CInt(numThreads.Value)
        If numThreads.Value > CPUCount Then
            numThreads.SetColor(Color.Red)
        Else
            numThreads.SetColor(Color.CadetBlue)
        End If
    End Sub

    Private Sub laThreads_DoubleClick(sender As Object, e As EventArgs) Handles laThreads.DoubleClick
        numThreads.Value = s.ParallelProcsNum
    End Sub

    Private Sub tcMain_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles tcMain.MouseDoubleClick 'Remove this
        AudioBindingSource.ResetBindings(False)

        Refresh()

        AutoResizeColumns()
        dgvAudio.SelectAll()
    End Sub

End Class
