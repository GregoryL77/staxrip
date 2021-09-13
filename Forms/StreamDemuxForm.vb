
Imports System.Globalization

Imports JM.LinqFaster
Imports StaxRip.UI

Public Class StreamDemuxForm
    Property AudioStreams As List(Of AudioStream)
    Property Subtitles As List(Of Subtitle)

    Sub New(demuxer As Demuxer, sourceFile As String, attachments As List(Of Attachment))
        InitializeComponent()

        cbDemuxChapters.Checked = demuxer.ChaptersDemuxing
        cbDemuxChapters.Visible = MediaInfo.GetMenu(sourceFile, "Chapters_Pos_End").ToInt - MediaInfo.GetMenu(sourceFile, "Chapters_Pos_Begin").ToInt > 0
        cbDemuxVideo.Checked = demuxer.VideoDemuxing

        ScaleClientSize(42, 30, FontHeight)
        StartPosition = FormStartPosition.CenterParent

        lvAudio.BeginUpdate()
        lvSubtitles.BeginUpdate()
        lvAttachments.BeginUpdate()

        lvAudio.View = View.Details
        lvAudio.Columns.Add("")
        lvAudio.CheckBoxes = True
        lvAudio.HeaderStyle = ColumnHeaderStyle.None
        lvAudio.ShowItemToolTips = True
        lvAudio.FullRowSelect = True
        lvAudio.MultiSelect = False
        lvAudio.HideFocusRectange()
        lvAudio.AutoCheckMode = AutoCheckMode.SingleClick

        lvSubtitles.View = View.SmallIcon
        lvSubtitles.CheckBoxes = True
        lvSubtitles.HeaderStyle = ColumnHeaderStyle.None
        lvSubtitles.AutoCheckMode = AutoCheckMode.SingleClick

        lvAttachments.View = View.SmallIcon
        lvAttachments.CheckBoxes = True
        lvAttachments.HeaderStyle = ColumnHeaderStyle.None
        lvAttachments.AutoCheckMode = AutoCheckMode.SingleClick

        AddHandler Load, Sub() lvAudio.Columns(0).Width = lvAudio.ClientSize.Width

        AudioStreams = MediaInfo.GetAudioStreams(sourceFile)
        Subtitles = MediaInfo.GetSubtitles(sourceFile)

        gbAudio.Enabled = AudioStreams.Count > 0
        gbSubtitles.Enabled = Subtitles.Count > 0
        gbAttachments.Enabled = Not attachments.NothingOrEmpty

        Dim langName As String = CurrCultNeutral.EnglishName
        Dim notEng As Boolean = Not CurrCult.TwoLetterISOLanguageName.Equals("en")
        bnAudioEnglish.Enabled = AudioStreams.AnyF(Function(stream) stream.Language.TwoLetterCode.Equals("en"))
        bnAudioNative.Visible = notEng
        bnAudioNative.Text = langName
        bnAudioNative.Enabled = AudioStreams.AnyF(Function(stream) stream.Language.TwoLetterCode.Equals(CurrCult.TwoLetterISOLanguageName))

        bnSubtitleEnglish.Enabled = Subtitles.AnyF(Function(stream) stream.Language.TwoLetterCode.Equals("en"))
        bnSubtitleNative.Visible = notEng
        bnSubtitleNative.Text = langName
        bnSubtitleNative.Enabled = Subtitles.AnyF(Function(stream) stream.Language.TwoLetterCode.Equals(CurrCult.TwoLetterISOLanguageName))

        If AudioStreams.Count > 0 Then
            lvAudio.Items.AddRange(AudioStreams.ToArray.SelectF(Function(a) New ListViewItem(a.Name) With {.Tag = a, .Checked = a.Enabled}))
        End If

        If Subtitles.Count > 0 Then
            lvSubtitles.Items.AddRange(Subtitles.ToArray.SelectF(Function(s) New ListViewItem(
                      s.Language.ToString & " (" & s.TypeName & ")" & If(s.Title.NotNullOrEmptyS, " - " & s.Title, "")) With {.Tag = s, .Checked = s.Enabled}))
        End If

        If attachments IsNot Nothing AndAlso attachments.Count > 0 Then
            lvAttachments.Items.AddRange(attachments.ToArray.SelectF(Function(a) New ListViewItem(a.Name) With {.Tag = a, .Checked = a.Enabled}))
        End If

        lvAudio.EndUpdate()
        lvSubtitles.EndUpdate()
        lvAttachments.EndUpdate()
    End Sub

    Sub lvAudio_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvAudio.ItemChecked
        If Visible Then
            DirectCast(e.Item.Tag, AudioStream).Enabled = e.Item.Checked
        End If
    End Sub

    Sub lvSubtitles_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvSubtitles.ItemChecked
        If Visible Then
            DirectCast(e.Item.Tag, Subtitle).Enabled = e.Item.Checked
        End If
    End Sub

    Sub lvAttachments_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvAttachments.ItemChecked
        If Visible Then
            DirectCast(e.Item.Tag, Attachment).Enabled = e.Item.Checked
        End If
    End Sub

    Sub bnAudioAll_Click(sender As Object, e As EventArgs) Handles bnAudioAll.Click
        For Each item As ListViewItem In lvAudio.Items
            item.Checked = True
        Next
    End Sub

    Sub bnAudioNone_Click(sender As Object, e As EventArgs) Handles bnAudioNone.Click
        For Each item As ListViewItem In lvAudio.Items
            item.Checked = False
        Next
    End Sub

    Sub bnAudioEnglish_Click(sender As Object, e As EventArgs) Handles bnAudioEnglish.Click
        For Each item As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(item.Tag, AudioStream)

            If stream.Language.TwoLetterCode = "en" Then
                item.Checked = True
            End If
        Next
    End Sub

    Sub bnAudioNative_Click(sender As Object, e As EventArgs) Handles bnAudioNative.Click
        For Each item As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(item.Tag, AudioStream)

            If stream.Language.TwoLetterCode = CurrCult.TwoLetterISOLanguageName Then
                item.Checked = True
            End If
        Next
    End Sub

    Sub bnSubtitleAll_Click(sender As Object, e As EventArgs) Handles bnSubtitleAll.Click
        For Each item As ListViewItem In lvSubtitles.Items
            item.Checked = True
        Next
    End Sub

    Sub bnSubtitleNone_Click(sender As Object, e As EventArgs) Handles bnSubtitleNone.Click
        For Each item As ListViewItem In lvSubtitles.Items
            item.Checked = False
        Next
    End Sub

    Sub bnSubtitleEnglish_Click(sender As Object, e As EventArgs) Handles bnSubtitleEnglish.Click
        For Each item As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(item.Tag, Subtitle)

            If stream.Language.TwoLetterCode = "en" Then
                item.Checked = True
            End If
        Next
    End Sub

    Sub bnSubtitleNative_Click(sender As Object, e As EventArgs) Handles bnSubtitleNative.Click
        For Each item As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(item.Tag, Subtitle)

            If stream.Language.TwoLetterCode = CurrCult.TwoLetterISOLanguageName Then
                item.Checked = True
            End If
        Next
    End Sub
End Class
