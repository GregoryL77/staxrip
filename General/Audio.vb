
Imports System.Text
Imports System.Globalization

Public Class Audio
    Shared Sub Process(ap As AudioProfile)
        If Not File.Exists(ap.File) OrElse TypeOf ap Is NullAudioProfile Then
            Exit Sub
        End If

        If Not Directory.Exists(p.TempDir) Then
            p.TempDir = ap.File.Dir
        End If

        If ap.File <> p.SourceFile Then
            Log.Write("Media Info Audio Source " & ap.GetTrackID, MediaInfo.GetSummary(ap.File))
        End If

        If TypeOf ap Is GUIAudioProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)
            'To Do:   Normalize after Cut order fix
            If gap.ContainsCommand("ffmpeg") AndAlso (p.Ranges.Count = 0 OrElse gap.DecodingMode <> AudioDecodingMode.Pipe) Then
                gap.NormalizeFF()
            End If
        End If

        Dim extractCore = TypeOf ap Is GUIAudioProfile AndAlso DirectCast(ap, GUIAudioProfile).ExtractCore

        If ap.Decoder <> AudioDecoderMode.Automatic AndAlso Not extractCore Then
            Convert(ap)
        End If

        If ap.HasStream Then
            Dim cutting = p.Ranges.Count > 0

            Dim directMux = TypeOf ap Is MuxAudioProfile AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.Stream.Ext) AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.File.Ext) AndAlso Not cutting

            Dim trackIsSupportedButNotContainer = TypeOf ap Is MuxAudioProfile AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.Stream.Ext) AndAlso
                Not p.VideoEncoder.Muxer.IsSupported(ap.File.Ext)

            If ((cutting OrElse Not ap.IsInputSupported) AndAlso Not directMux) OrElse
                trackIsSupportedButNotContainer Then

                Select Case ap.File.ExtFull
                    Case ".mkv", ".webm"
                        mkvDemuxer.Demux(ap.File, {ap.Stream}, Nothing, ap, p, False, False, True)
                    Case ".mp4"
                        MP4BoxDemuxer.DemuxAudio(ap.File, ap.Stream, ap, p, True)
                    Case Else
                        If p.Script.GetFilter("Source").Script.ToLower.Contains("directshowsource") AndAlso
                            Not TypeOf ap Is MuxAudioProfile Then

                            ConvertDirectShowSource(ap)
                        ElseIf Not ap.File.Ext = "m2ts" Then
                            ffmpegDemuxer.DemuxAudio(ap.File, ap.Stream, ap, p, True)
                        End If
                End Select
            End If
        End If

        Cut(ap)

        'To Do:   Normalize after Cut order fix
        If TypeOf ap Is GUIAudioProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)
            If gap.ContainsCommand("ffmpeg") AndAlso p.Ranges.Count > 0 AndAlso gap.DecodingMode = AudioDecodingMode.Pipe Then
                gap.NormalizeFF()
            End If
        End If

        If Not TypeOf ap Is MuxAudioProfile AndAlso Not ap.IsInputSupported Then
            Convert(ap)
        End If
    End Sub

    Shared Function GetBaseNameForStream(path As String, stream As AudioStream) As String
        Dim base As String

        If p.TempDir.EndsWithEx("_temp\") AndAlso path.Base.StartsWithEx(p.SourceFile.Base) Then
            base = path.Base.Substring(p.SourceFile.Base.Length)

            'To Do: empty pipe streams temp files
            If base = "" Then ShortBegEnd(path.Base)
        Else
            base = path.Base
        End If

        If base = "" Then
            base = "temp"
        End If

        Dim ret = base + " ID" & (stream.Index + 1)

        If stream.Delay <> 0 Then
            ret += " " & stream.Delay & "ms"
        End If

        If stream.Language.TwoLetterCode <> "iv" Then
            ret += " " + stream.Language.ToString
        End If

        If stream.Title <> "" Then
            ret += " {" + stream.Title.Shorten(50).EscapeIllegalFileSysChars + "}"
        End If

        Return ret.Trim
    End Function

    Shared Sub Convert(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If ap.File.Ext = "avs" Then
            Dim outPath = ap.File.DirAndBase + "." + ap.ConvertExt
            Dim args = "-i " + ap.File.Escape + " -y -hide_banner " + outPath.Escape

            Using proc As New Proc
                proc.Header = "AVS to " + outPath.Ext.ToUpper
                proc.SkipStrings = {"frame=", "size="}
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.ffmpeg
                proc.Arguments = args
                proc.Start()
            End Using

            If g.FileExists(outPath) Then
                ap.File = outPath
                Exit Sub
            End If
        End If

        Select Case ap.Decoder
            Case AudioDecoderMode.ffmpeg, AudioDecoderMode.Automatic
                ConvertFF(ap)
            Case AudioDecoderMode.FFAudioSource
                ConvertFFAudioSource(ap)
            Case AudioDecoderMode.eac3to
                ConvertEac3to(ap)
            Case AudioDecoderMode.NicAudio
                ConvertNicAudio(ap)
            Case AudioDecoderMode.DirectShow
                ConvertDirectShowSource(ap)
        End Select

        If p.Script.GetFilter("Source").Script.ToLower.Contains("directshowsource") Then
            ConvertDirectShowSource(ap)
        End If

        ConvertFF(ap)
        ConvertEac3to(ap)
        ConvertDirectShowSource(ap)
    End Sub

    Shared Sub CutNicAudio(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then
            Exit Sub
        End If

        If Not FileTypes.NicAudioInput.Contains(ap.File.Ext) Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = p.TempDir + ap.File.Base + "_cut_na.wav"
        d.Path = (p.TempDir + ap.File.Base + "_cut_na.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter(GetNicAudioCode(ap)))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertNicAudio(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not FileTypes.NicAudioInput.Contains(ap.File.Ext) Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = p.TempDir + ap.File.Base + "_DecodeNicAudio." + ap.ConvertExt
        d.Path = (p.TempDir + ap.File.Base + "_DecodeNicAudio.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter(GetNicAudioCode(ap)))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub Cut(ap As AudioProfile)
        If p.Ranges.Count = 0 OrElse
            Not File.Exists(ap.File) OrElse
            TypeOf p.VideoEncoder Is NullEncoder OrElse
            ap.File.Contains("_cut_.") Then

            Exit Sub
        End If

        Select Case p.CuttingMode
            Case CuttingMode.mkvmerge
                CutMkvmerge(ap)
            Case CuttingMode.NicAudio
                If FileTypes.NicAudioInput.Contains(ap.File.Ext) AndAlso
                    Not TypeOf ap Is MuxAudioProfile Then

                    CutNicAudio(ap)
                Else
                    CutMkvmerge(ap)
                End If
            Case CuttingMode.DirectShow
                If Not TypeOf ap Is MuxAudioProfile Then
                    CutDirectShowSource(ap)
                Else
                    CutMkvmerge(ap)
                End If
        End Select
    End Sub

    Shared Function GetNicAudioCode(ap As AudioProfile) As String
        Select Case ap.File.Ext
            Case "ac3"
                Return "AudioDub(last, NicAC3Source(""" + ap.File + """, Channels = " & ap.Channels & "))"
            Case "mpa", "mp2", "mp3"
                Return "AudioDub(last, NicMPASource(""" + ap.File + """))"
            Case "wav"
                Return "AudioDub(last, RaWavSource(""" + ap.File + """, Channels = " & ap.Channels & "))"
        End Select
    End Function

    Shared Sub ConvertEac3to(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not FileTypes.eac3toInput.Contains(ap.File.Ext) Then
            Exit Sub
        End If

        Dim outPath = p.TempDir + ap.File.Base + "." + ap.ConvertExt
        Dim args = ap.File.Escape + " " + outPath.Escape

        If ap.Channels = 6 Then
            args += " -down6"
        ElseIf ap.Channels = 2 Then
            args += " -down2"
        End If

        If TypeOf ap Is GUIAudioProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)

            If gap.Params.Normalize Then
                args += " -normalize"
                gap.Params.Normalize = True
            End If
        End If

        args += " -simple -progressnumbers"

        Using proc As New Proc
            proc.Header = "Convert " + ap.File.Ext.ToUpper + " to " + outPath.Ext.ToUpper + " " & ap.GetTrackID
            proc.Package = Package.eac3to
            proc.Arguments = args
            proc.TrimChars = {"-"c, " "c}
            proc.SkipStrings = {"process:", "analyze:"}
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertFF(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        Dim gap = TryCast(ap, GUIAudioProfile)

        'Cut fail fix, normalize after cut which failed
        If gap IsNot Nothing Then
            If p.Ranges.Count = 0 OrElse (p.Ranges.Count > 0 AndAlso ap.File.Contains("_cut_")) Then
                gap.NormalizeFF()
                'gap.Params.Normalize = False
            End If
        End If

        Dim outPath = (p.TempDir + ap.File.Base + "." + ap.ConvertExt).LongPathPrefix

        If ap.File = outPath Then
            outPath += "." + ap.ConvertExt
        End If

        Dim args As String

        If gap?.Params.ProbeSize <> 5 Then
            args += $" -probesize {gap.Params.ProbeSize}M"
        End If

        If gap?.Params.AnalyzeDuration <> 5 Then
            args += $" -analyzeduration {gap.Params.AnalyzeDuration}M"
        End If

        args += " -sn -vn -dn -i " + ap.File.LongPathPrefix.Escape

        If Not ap.Stream Is Nothing Then
            args += " -sn -vn -dn -map 0:" & ap.Stream.StreamOrder
        End If

        If gap?.Params.ffmpegDither <> "Disabled" Then
            args += " -dither_method " & gap.Params.ffmpegDither
        End If

        If gap?.Params.ffmpegResampSOX Then
            args += " -resampler soxr -precision 28"
        End If

        'p.Ranges.Count = 0 OrElse
        '(p.Ranges.Count > 0 AndAlso ap.File.Contains("_cut_")) OrElse
        '(p.Ranges.Count > 0 AndAlso gap?.DecodingMode <> AudioDecodingMode.Pipe) Or

        'Not (p.Ranges.Count > 0 AndAlso Not ap.File.Contains("_cut_"))

        If p.Ranges.Count = 0 OrElse ap.File.Contains("_cut_") Then
            If gap?.Params.Normalize Then
                Select Case gap.Params.ffmpegNormalizeMode
                    Case ffmpegNormalizeMode.dynaudnorm
                        args += " " + Audio.GetDynAudNormArgs(gap.Params)
                        If ap.Gain <> 0 Then
                            args += ",volume=" + ap.Gain.ToInvariantString + "dB"
                        End If
                    Case ffmpegNormalizeMode.loudnorm
                        args += " " + Audio.GetLoudNormArgs(gap.Params)
                        If ap.Gain <> 0 Then
                            args += ",volume=" + ap.Gain.ToInvariantString + "dB"
                        End If
                        If gap.Params.SamplingRate = 0 Then     'Loudnorm auto x4 upsample
                            args += " -ar " & gap.SourceSamplingRate
                        End If
                    Case ffmpegNormalizeMode.peak
                        If ap.GainWasNormalized Then
                            args += " -af volume=" + ap.Gain.ToInvariantString + "dB"
                        End If
                End Select
            ElseIf ap.Gain <> 0 Then
                args += " -af volume=" + ap.Gain.ToInvariantString + "dB"
            End If
            ap.GainWasNormalized = True
        End If


        If gap?.Params.SamplingRate <> 0 Then
            args += " -ar " & gap.Params.SamplingRate
        End If

        If gap?.Params.ChannelsMode <> ChannelsMode.Original Then
            args += " -rematrix_maxval 1 -ac " & ap.Channels
            If gap.Params.ffmpegLFEMixLevel <> 0 AndAlso gap.Params.ChannelsMode < 3 Then
                args += " -lfe_mix_level " & gap.Params.ffmpegLFEMixLevel.ToInvariantString
            End If
        End If

        args += " -y -hide_banner" & s.GetFFLogLevel(FfLogLevel.info)

        If ap.ConvertExt.EqualsAny("wav") Then
            Select Case gap?.Depth
                Case 24
                    args += " -c:a pcm_s24le"
                Case 32
                    args += " -c:a pcm_f32le"
                Case 16
                    args += " -c:a pcm_s16le"
                Case Else
                    args += " -c:a pcm_f32le"
            End Select
        ElseIf ap.ConvertExt.EqualsAny("wv") Then
            args += " -compression_level 1"

            Select Case gap?.Depth
                Case 24
                    args += " -sample_fmt s32p"
                Case 32
                    args += " -sample_fmt fltp"
                Case 16
                    args += " -sample_fmt s16p"
                Case Else
                    'ffmpeg should auto choose ???
                    If args.ContainsAny(" -af ", "-rematrix_maxval 1 -ac") Then
                        args += " -sample_fmt fltp"
                    End If
            End Select
        End If

        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Header = "Convert " + ap.File.Ext.ToUpper + " to " + outPath.Ext.ToUpper + " " & ap.GetTrackID
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Duration = ap.GetDuration()
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            If outPath.StartsWith("\\?\") Then
                outPath = outPath.Substring(4)
            End If

            'normalize, Gain eac3to duplication
            'If gap?.GetEncoder() = GuiAudioEncoder.eac3to Then
            'ap.Gain = 0
            'If p.Ranges.Count > 0 AndAlso Not ap.File.Contains("_cut_") Then
            'Else
            'gap.Params.Normalize = False
            'End If
            'End If

            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Function GetLoudNormArgs(params As GUIAudioProfile.Parameters) As String
        Return "-af loudnorm=" +
            "I=" + params.ffmpegLoudnormIntegrated.ToInvariantString +
            ":TP=" + params.ffmpegLoudnormTruePeak.ToInvariantString +
            ":LRA=" + params.ffmpegLoudnormLRA.ToInvariantString +
            ":offset=" + params.ffmpegLoudnormOffset.ToInvariantString +
            ":measured_I=" + params.ffmpegLoudnormIntegratedMeasured.ToInvariantString +
            ":measured_TP=" + params.ffmpegLoudnormTruePeakMeasured.ToInvariantString +
            ":measured_LRA=" + params.ffmpegLoudnormLraMeasured.ToInvariantString +
            ":measured_thresh=" + params.ffmpegLoudnormThresholdMeasured.ToInvariantString +
            ":print_format=summary"
    End Function

    Shared Function GetDynAudNormArgs(params As GUIAudioProfile.Parameters) As String
        Dim ret As String

        If params.ffmpegDynaudnormF <> 500 Then ret += ":f=" + params.ffmpegDynaudnormF.ToInvariantString
        If params.ffmpegDynaudnormG <> 31 Then ret += ":g=" + params.ffmpegDynaudnormG.ToInvariantString
        If params.ffmpegDynaudnormP <> 0.95 Then ret += ":p=" + params.ffmpegDynaudnormP.ToInvariantString
        If params.ffmpegDynaudnormM <> 10 Then ret += ":m=" + params.ffmpegDynaudnormM.ToInvariantString
        If params.ffmpegDynaudnormR <> 0 Then ret += ":r=" + params.ffmpegDynaudnormR.ToInvariantString
        If params.ffmpegDynaudnormS <> 0 Then ret += ":s=" + params.ffmpegDynaudnormS.ToInvariantString
        If Not params.ffmpegDynaudnormN Then ret += ":n=false"
        If params.ffmpegDynaudnormC Then ret += ":c=true"
        If params.ffmpegDynaudnormB Then ret += ":b=true"

        If ret <> "" Then
            Return "-af dynaudnorm=" + ret.Trim(":"c)
        Else
            Return "-af dynaudnorm"
        End If
    End Function

    Shared Sub ConvertDirectShowSource(ap As AudioProfile, Optional useFlac As Boolean = False)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = p.TempDir + ap.File.Base + "_convDSS." + ap.ConvertExt
        d.Path = (p.TempDir + ap.File.Base + "_DecDSS.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,DirectShowSource(""" + ap.File + """, video=false))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertFFAudioSource(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim cachefile = p.TempDir + ap.File.Base + ".ffindex"
        g.ffmsindex(ap.File, cachefile)
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = p.TempDir + ap.File.Base + "_convFFAudioSource." + ap.ConvertExt
        d.Path = (p.TempDir + ap.File.Base + "_DecodeFFAudioSource.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,FFAudioSource(""" + ap.File + """, cachefile=""" + cachefile + """))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutDirectShowSource(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = p.TempDir + ap.File.Base + "_cut_ds.wav"
        d.Path = (p.TempDir + ap.File.Base + "_cut_ds.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,DirectShowSource(""" + ap.File + """, video=false))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutFFAudioSource(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim cachefile = p.TempDir + ap.File.Base + ".ffindex"
        g.ffmsindex(ap.File, cachefile)
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = p.TempDir + ap.File.Base + "_cut_ff.wav"
        d.Path = (p.TempDir + ap.File.Base + "_cut_ff.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,FFAudioSource(""" + ap.File + """, cachefile=""" + cachefile + """))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutMkvmerge(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        Dim aviPath = p.TempDir + ap.File.Base + "_cut_mm.avi"
        Dim d = (p.CutFrameCount / p.CutFrameRate).ToString("f9", CultureInfo.InvariantCulture)
        Dim r = p.CutFrameRate.ToString("f9", CultureInfo.InvariantCulture)
        Dim args = $"-f lavfi -i color=c=black:s=16x16:d={d}:r={r} -y -hide_banner -c:v copy " + aviPath.Escape

        Using proc As New Proc
            proc.Header = "Create avi file for audio cutting"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog("mkvmerge cannot cut audio without video so a avi file has to be created" + BR2)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If Not File.Exists(aviPath) Then
            Throw New ErrorAbortException("Error", "Output file missing")
        Else
            Log.WriteLine(MediaInfo.GetSummary(aviPath))
        End If

        Dim mkvPath = p.TempDir + ap.File.Base + "_cut_.mkv"

        Dim args2 = "-o " + mkvPath.Escape + " " + aviPath.Escape + " " + ap.File.Escape
        args2 += " --split parts-frames:" + p.Ranges.Select(Function(v) v.Start & "-" & (v.End + 1)).Join(",+")
        args2 += " --ui-language en"

        Using proc As New Proc
            proc.Header = "Cut audio"
            proc.SkipString = "Progress: "
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.mkvmerge
            proc.Arguments = args2
            proc.AllowedExitCodes = {0, 1, 2}
            proc.Start()
        End Using

        Dim fail As Boolean

        If File.Exists(mkvPath) Then
            Log.WriteLine(MediaInfo.GetSummary(mkvPath))
            Dim streams = MediaInfo.GetAudioStreams(mkvPath)

            If streams.Count > 0 Then
                mkvDemuxer.Demux(mkvPath, {streams(0)}, Nothing, ap, p, False, False, True)
            Else
                fail = True
            End If
        Else
            fail = True
        End If

        If fail AndAlso TypeOf ap Is GUIAudioProfile AndAlso Not ap.File.Ext.EqualsAny("wv", "wav") Then
            Log.Write("Error", "no output found")
            Convert(ap)

            If ap.File.Ext.EqualsAny("wv", "wav") Then
                Cut(ap)
            End If
        End If
    End Sub

    Shared Function GetDown2Code() As String
        Dim a =
      <a>
Audiochannels() >= 6 ? Down2(last) : last

function Down2(clip a) 
{
	a = ConvertAudioToFloat(a)
	fl = GetChannel(a, 1)
	fr = GetChannel(a, 2)
	c = GetChannel(a, 3)
	lfe = GetChannel(a, 4)
	sl = GetChannel(a, 5)
	sr = GetChannel(a, 6)
	l_sl = MixAudio(fl, sl, 0.2929, 0.2929)
	c_lfe = MixAudio(lfe, c, 0.2071, 0.2071)
	r_sr = MixAudio(fr, sr, 0.2929, 0.2929)
	l = MixAudio(l_sl, c_lfe, 1.0, 1.0)
	r = MixAudio(r_sr, c_lfe, 1.0, 1.0)
	return MergeChannels(l, r)
}
</a>
        Return a.Value.Trim
    End Function

    Shared Function FileExist(fp As String) As Boolean
        Return File.Exists(fp) AndAlso New FileInfo(fp).Length > 500
    End Function

    Shared Function IsEncoderUsed(encoder As GuiAudioEncoder) As Boolean
        If IsEncoderUsed(p.Audio0, encoder) OrElse IsEncoderUsed(p.Audio1, encoder) Then
            Return True
        End If

        If Not p.AudioTracks.NothingOrEmpty Then
            For Each ap In p.AudioTracks
                If IsEncoderUsed(ap, encoder) Then
                    Return True
                End If
            Next
        End If
    End Function

    Shared Function IsEncoderUsed(ap As AudioProfile, encoder As GuiAudioEncoder) As Boolean
        If TypeOf ap Is GUIAudioProfile AndAlso ap.File <> "" Then
            Dim gap = DirectCast(ap, GUIAudioProfile)

            If gap.GetEncoder = encoder Then
                Return True
            End If
        End If
    End Function

    Shared Function CommandContains(find As String) As Boolean
        If p.Audio0.IsUsedAndContainsCommand(find) OrElse p.Audio1.IsUsedAndContainsCommand(find) Then
            Return True
        End If

        If Not p.AudioTracks.NothingOrEmpty Then
            For Each ap In p.AudioTracks
                If ap.IsUsedAndContainsCommand(find) Then
                    Return True
                End If
            Next
        End If
    End Function
End Class

Public Enum AudioDecoderMode
    Automatic
    ffmpeg
    eac3to
    NicAudio
    DirectShow
    FFAudioSource
End Enum

Public Enum AudioDecodingMode
    Pipe
    WavPack
    WAVE
    FLAC
End Enum
'W64
Public Enum CuttingMode
    mkvmerge
    DirectShow
    NicAudio
End Enum

Public Enum ffNormalizeMode
    <UI.DispName("(true)peak based")> peak
    loudnorm
    dynaudnorm
End Enum

Public Enum ffmpegNormalizeMode
    <UI.DispName("(true)peak based")> peak
    loudnorm
    dynaudnorm
End Enum