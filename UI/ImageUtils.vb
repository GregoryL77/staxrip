Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Threading.Tasks

Public Class ImageHelp
    Private Shared FontFilesExist As Boolean '= File.Exists(Folder.Apps & "Fonts\FontAwesome.ttf") 'AndAlso File.Exists(Folder.Apps & "Fonts\Segoe-MDL2-Assets.ttf")
    Private Shared CollFontPrv As PrivateFontCollection 'New PrivateFontCollection
    Private Shared FontSagoe As Font 'New Font("Segoe MDL2 Assets", 12)
    Private Shared FontAwesome As Font
    Public Shared ImageCacheD As Dictionary(Of Integer, Image) 'New Dictionary(Of Integer, Image)(59)   46 Max As for 202106

    Public Shared Sub CreateFonts()
        If FontSagoe Is Nothing Then
            FontSagoe = New Font("Segoe MDL2 Assets", 12)
            FontFilesExist = File.Exists(Folder.Apps & "Fonts\FontAwesome.ttf")
        End If
        If FontFilesExist Then
            If CollFontPrv Is Nothing Then CollFontPrv = New PrivateFontCollection
            If CollFontPrv.Families.Length = 0 Then CollFontPrv.AddFontFile(Folder.Apps & "Fonts\FontAwesome.ttf")
            If FontAwesome Is Nothing Then FontAwesome = New Font(CollFontPrv.Families(0), 12)
        End If
        If ImageCacheD Is Nothing Then ImageCacheD = New Dictionary(Of Integer, Image)(59)
    End Sub
    Public Shared Sub DisposeFonts()
        If ImageCacheD IsNot Nothing Then
            ImageCacheD.Clear()
            ImageCacheD = Nothing
        End If
        If FontSagoe IsNot Nothing Then
            FontSagoe.Dispose()
            FontSagoe = Nothing
        End If
        If FontAwesome IsNot Nothing Then
            FontAwesome.Dispose()
            FontAwesome = Nothing
        End If
        If CollFontPrv IsNot Nothing Then
            'Parallel.ForEach(ImageCacheD.Values, New ParallelOptions With {.MaxDegreeOfParallelism = Math.Max(CPUsC \ 2, 1)}, Sub(i) i.Dispose()) ' Destroys existings menus! :(
            CollFontPrv.Dispose()
            CollFontPrv = Nothing
        End If
    End Sub
    'Shared Async Function GetSymbolImageAsync(symbol As Symbol) As Task(Of Image)
    '    Return Await Task.Run(Of Image)(Function() GetSymbolImage(symbol))
    'End Function
    Shared Function GetSymbolImage(symbol As Symbol) As Image
        If symbol > 61400 AndAlso Not FontFilesExist OrElse symbol = 0 Then Return Nothing
        'dim fHeight = 16 font.Height 'New Bitmap(CInt(fHeight * 1.1), CInt(fHeight * 1.1))

        Dim bitmap As New Bitmap(CInt(16 * 1.1), CInt(16 * 1.1))
        Using graphics = Drawing.Graphics.FromImage(bitmap)
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias
            graphics.DrawString(Convert.ToChar(symbol), If(symbol > 61400, FontAwesome, FontSagoe), Brushes.Black, -16 * 0.1F, 16 * 0.07F)
        End Using

        Return bitmap
    End Function

    Shared Function GetSymbolImageSmall(symbol As Symbol) As Image
        If symbol > 61400 AndAlso Not FontFilesExist Then Return Nothing 'symbol = 0 OrElse
        'Using fontS As New Font("Segoe MDL2 Assets", 11) FH15

        Dim bitmap As New Bitmap(16, 16) '=SystemInformation.SmallIconSize
        Using graphics = Drawing.Graphics.FromImage(bitmap)
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias
            graphics.DrawString(Convert.ToChar(symbol), If(symbol > 61400, FontAwesome, FontSagoe), Brushes.Black, -16 * 0.1F, 0F)
        End Using

        Return bitmap
    End Function

    Shared Function GetImageC(symbol As Symbol) As Image
        Dim img As Image
        If ImageCacheD.TryGetValue(symbol, img) Then Return img
        img = GetSymbolImage(symbol)
        ImageCacheD.Item(symbol) = img
        Return img
    End Function

    Shared Sub ClearCache()
        If ImageCacheD IsNot Nothing Then
            ImageCacheD.Clear()
            ImageCacheD = New Dictionary(Of Integer, Image)(59)
        End If
    End Sub
End Class

Public Class Thumbnails
    Shared Sub SaveThumbnails(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Exit Sub
        End If

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Const fontname = "DejaVu Serif"
        Const fontOptions = "Mikadan"

        Dim width = s.Storage.GetInt("Thumbnail Width", 500)
        Dim columnCount = s.Storage.GetInt("Thumbnail Columns", 4)
        Dim rowCount = s.Storage.GetInt("Thumbnail Rows", 6)
        Dim dar = MediaInfo.GetVideo(inputFile, "DisplayAspectRatio")
        Dim height = CInt(width / Convert.ToSingle(dar, InvCult))
        Dim gap = CInt((width * columnCount) * (s.Storage.GetInt("Thumbnail Margin", 5) / 1000))
        Dim font = New Font(fontname, (width * columnCount) \ 80, FontStyle.Bold, GraphicsUnit.Pixel)
        Dim foreColor = Color.Black

        width -= width Mod 16
        height -= height Mod 16

        Dim script As New VideoScript With {.Path = Path.Combine(Folder.Temp & "Thumbnails.avs")}

        If String.Equals(inputFile.Ext, "mp4") Then
            script.Filters.Add(New VideoFilter($"LSMASHVideoSource(""{inputFile}"")"))
        Else
            script.Filters.Add(New VideoFilter($"FFVideoSource(""{inputFile}"")"))
        End If

        script.Filters.Add(New VideoFilter($"Spline64Resize({width},{height})"))

        Dim mode = s.Storage.GetInt("Thumbnail Mode")
        Dim intervalSec = s.Storage.GetInt("Thumbnail Interval")

        If intervalSec <> 0 AndAlso mode = 1 Then
            rowCount = CInt((script.GetSeconds / intervalSec) / columnCount)
        End If

        Dim errorMsg = script.GetError

        If errorMsg.NotNullOrEmptyS Then
            MsgError("Failed to open file" & BR2 & inputFile, errorMsg)
            Exit Sub
        End If

        Dim frames = script.GetFrameCount
        Dim count = columnCount * rowCount
        Dim bitmaps As New List(Of Bitmap)

        script.Synchronize(True, True, True)

        Using server = FrameServerFactory.Create(script.Path)
            Dim serverPos As Integer

            For x = 1 To count
                serverPos = CInt((frames / count) * x) - CInt((frames / count) / 2)
                Dim bitmap = BitmapUtil.CreateBitmap(server, serverPos)

                Using g = Graphics.FromImage(bitmap)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.TextRenderingHint = TextRenderingHint.AntiAlias
                    g.SmoothingMode = SmoothingMode.AntiAlias
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality

                    Dim dur = TimeSpan.FromSeconds(server.Info.FrameCount / server.FrameRate)
                    Dim timestamp = StaxRip.g.GetTimeString(serverPos / server.FrameRate)
                    Dim ft As New Font("Segoe UI", font.Size, FontStyle.Bold, GraphicsUnit.Pixel)

                    Using gp As New GraphicsPath()
                        Dim sz = g.MeasureString(timestamp, ft)
                        Dim pt As Point
                        Dim pos = s.Storage.GetInt("Thumbnail Position", 1)
                        Dim ftH As Integer = ft.Height

                        If pos = 0 OrElse pos = 2 Then
                            pt.X = ftH \ 10
                        Else
                            pt.X = CInt(bitmap.Width - sz.Width - ftH / 10)
                        End If

                        If pos = 2 OrElse pos = 3 Then
                            pt.Y = CInt(bitmap.Height - sz.Height)
                        Else
                            pt.Y = 0
                        End If

                        gp.AddString(timestamp, ft.FontFamily, CInt(ft.Style), ft.Size, pt, New StringFormat())

                        Using pen As New Pen(Color.Black, ftH \ 5)
                            g.DrawPath(pen, gp)
                        End Using

                        g.FillPath(Brushes.Gainsboro, gp)
                    End Using
                End Using

                bitmaps.Add(bitmap)
            Next

            width += gap
            height += gap
        End Using

        If inputFile.Ext = "mp4" Then
            FileHelp.Delete(inputFile & ".lwi")
        Else
            FileHelp.Delete(inputFile & ".ffindex")
        End If

        Dim infoSize As String
        Dim infoWidth = MediaInfo.GetVideo(inputFile, "Width")
        Dim infoHeight = MediaInfo.GetVideo(inputFile, "Height")
        Dim infoLength = New FileInfo(inputFile).Length
        Dim infoDuration = MediaInfo.GetGeneral(inputFile, "Duration").ToInt
        Dim audioCodecs = MediaInfo.GetAudioCodecs(inputFile)

        If audioCodecs Is Nothing Then
            audioCodecs = ""
        End If

        Dim channels = MediaInfo.GetAudio(inputFile, "Channel(s)").ToInt
        Dim subSampling = MediaInfo.GetVideo(inputFile, "ChromaSubsampling").Replace(":", "")
        If subSampling Is Nothing Then subSampling = ""
        Dim colorSpace = MediaInfo.GetVideo(inputFile, "ColorSpace").ToLower
        If colorSpace Is Nothing Then colorSpace = ""
        Dim profile = MediaInfo.GetVideo(inputFile, "Format_Profile").Shorten(4)
        If profile.NullOrEmptyS Then profile = "Main"
        Dim scanType = MediaInfo.GetVideo(inputFile, "ScanType")

        Dim audioSound As String
        Select Case channels
            Case 2
                audioSound = "Stereo"
            Case 1
                audioSound = "Mono"
            Case 6
                audioSound = "Surround Sound"
            Case 8
                audioSound = "Surround Sound"
            Case 0
                audioSound = String.Empty
        End Select

        If infoLength / 1024 ^ 3 > 1 Then
            infoSize = (infoLength / 1024 ^ 3).ToInvStr("f2") & "GB"
        Else
            infoSize = CInt(infoLength / 1024 ^ 2).ToInvStr & "MB"
        End If

        Dim caption = "File: " & inputFile.FileName & BR & "Size: " & MediaInfo.GetGeneral(inputFile, "FileSize") & " bytes" & " (" & infoSize & ")" & ", " &
            "Duration: " & StaxRip.g.GetTimeString(infoDuration / 1000) & ", avg.bitrate: " & MediaInfo.GetGeneral(inputFile, "OverallBitRate_String") & BR &
            "Audio: " & audioCodecs & ", " & MediaInfo.GetAudio(inputFile, "SamplingRate_String") & ", " & audioSound & ", " & MediaInfo.GetAudio(inputFile, "BitRate_String") & BR &
            "Video: " & MediaInfo.GetVideo(inputFile, "Format") & " (" & profile & ")" & ", " & colorSpace & subSampling & scanType.Shorten(1).ToLower() & ", " & MediaInfo.GetVideo(inputFile, "Width") & "x" &
            MediaInfo.GetVideo(inputFile, "Height") & ", " & MediaInfo.GetVideo(inputFile, "BitRate_String") & ", " & MediaInfo.GetVideo(inputFile, "FrameRate").ToSingle.ToInvStr & "fps".Replace(", ", "")

        caption = caption.Replace(" ,", "")

        Dim captionSize = TextRenderer.MeasureText(caption, font)
        Dim fHeight As Integer = font.Height
        Dim captionHeight = captionSize.Height + fHeight \ 3
        Dim waterMark = s.Storage.GetBool("Logo", False)
        Dim imageWidth = width * columnCount + gap
        Dim imageHeight = height * rowCount + captionHeight

        Using bitmap As New Bitmap(imageWidth, imageHeight)
            Using g = Graphics.FromImage(bitmap)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.TextRenderingHint = TextRenderingHint.AntiAlias
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.Clear(s.ThumbnailBackgroundColor)

                Dim rect = New RectangleF(gap, 0, imageWidth - gap * 2, captionHeight)
                Dim format As New StringFormat
                format.LineAlignment = StringAlignment.Center

                Using brush As New SolidBrush(foreColor)
                    g.DrawString(caption, font, brush, rect, format)
                    format.Alignment = StringAlignment.Far
                    format.LineAlignment = StringAlignment.Center

                    If waterMark = False Then
                        g.DrawString("StaxRip", New Font(fontOptions, fHeight * 2, FontStyle.Bold, GraphicsUnit.Pixel), brush, rect, format)
                    End If
                End Using

                For x = 0 To bitmaps.Count - 1
                    Dim rowPos = x \ columnCount
                    Dim columnPos = x Mod columnCount
                    g.DrawImage(bitmaps(x), columnPos * width + gap, rowPos * height + captionHeight)
                Next
            End Using

            Dim directoryStatus = s.Storage.GetBool("StaxRipOutput", False)
            Dim directoryLocation = s.Storage.GetString("StaxRipDirectory", p.DefaultTargetFolder)
            Dim export = Path.Combine(directoryLocation, inputFile.Base)
            Dim options = s.Storage.GetString("Picture Format", "png")

            Try
                If options = "jpg" Then
                    Dim params = New EncoderParameters(1)
                    params.Param(0) = New EncoderParameter(Imaging.Encoder.Quality, s.Storage.GetInt("Thumbnail Compression Quality", 95))
                    Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Jpeg.Guid).First

                    If directoryStatus = True Then
                        bitmap.Save(export.ChangeExt("jpg"), info, params)
                    Else
                        bitmap.Save(inputFile.ChangeExt("jpg"), info, params)
                    End If
                ElseIf options = "png" Then
                    Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Png.Guid).First

                    If directoryStatus = True Then
                        bitmap.Save(export.ChangeExt("png"), info, Nothing)
                    Else
                        bitmap.Save(inputFile.ChangeExt("png"), info, Nothing)
                    End If
                ElseIf options = "tiff" Then
                    Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Tiff.Guid).First

                    If directoryStatus = True Then
                        bitmap.Save(export.ChangeExt("tiff"), info, Nothing)
                    Else
                        bitmap.Save(inputFile.ChangeExt("tiff"), info, Nothing)
                    End If
                ElseIf options = "bmp" Then
                    Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Bmp.Guid).First

                    If directoryStatus = True Then
                        bitmap.Save(export.ChangeExt("bmp"), info, Nothing)
                    Else
                        bitmap.Save(inputFile.ChangeExt("bmp"), info, Nothing)
                    End If
                End If
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End Using
    End Sub
End Class