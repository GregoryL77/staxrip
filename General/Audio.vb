
Imports System.Globalization
Imports System.Text
Imports JM.LinqFaster
Imports KGySoft.CoreLibraries

Public Class Audio
    Shared Sub Process(ap As AudioProfile)
        If Not File.Exists(ap.File) OrElse TypeOf ap Is NullAudioProfile Then
            Exit Sub
        End If

        If Not Directory.Exists(p.TempDir) Then
            p.TempDir = ap.File.Dir
        End If

        If Not ap.File.Equals(p.SourceFile) Then
            Log.Write("Media Info Audio Source " & ap.GetTrackID, MediaInfo.GetSummary(ap.File, ap.FileKeyHashValue))
        End If

        Dim cutting = p.Ranges.Count > 0
        Dim gap As GUIAudioProfile = TryCast(ap, GUIAudioProfile)
        Dim gapCff As Boolean

        If gap IsNot Nothing Then
            'To Do:   Normalize after Cut order fix
            gapCff = gap.CommandLines.ContainsEx("ffmpeg")
            If gapCff AndAlso (Not cutting OrElse gap.DecodingMode <> AudioDecodingMode.Pipe) Then
                gap.NormalizeFF()
            End If
        End If

        If ap.Decoder <> AudioDecoderMode.Automatic AndAlso Not If(gap?.ExtractCore, False) Then
            Convert(ap)
        End If

        If ap.HasStream Then
            Dim muxSuppStream = TypeOf ap Is MuxAudioProfile AndAlso p.VideoEncoder.Muxer.IsSupported(ap.Stream.Ext)
            Dim directMux = muxSuppStream AndAlso p.VideoEncoder.Muxer.IsSupported(ap.File.Ext) AndAlso Not cutting
            Dim trackIsSupportedButNotContainer = muxSuppStream AndAlso Not p.VideoEncoder.Muxer.IsSupported(ap.File.Ext)

            If ((cutting OrElse Not ap.IsInputSupported) AndAlso Not directMux) OrElse trackIsSupportedButNotContainer Then

                Select Case ap.File.ExtFull
                    Case ".mkv", ".webm"
                        mkvDemuxer.Demux(ap.File, {ap.Stream}, Nothing, ap, p, False, False, True)
                    Case ".mp4"
                        MP4BoxDemuxer.DemuxAudio(ap.File, ap.Stream, ap, p, True)
                    Case Else
                        If p.Script.GetFilter("Source").Script.IndexOf("directshowsource", StringComparison.OrdinalIgnoreCase) >= 0 AndAlso TypeOf ap IsNot MuxAudioProfile Then
                            ConvertDirectShowSource(ap)
                        ElseIf Not ap.File.Ext = "m2ts" Then
                            ffmpegDemuxer.DemuxAudio(ap.File, ap.Stream, ap, p, True)
                        End If
                End Select
            End If
        End If

        Cut(ap)

        'To Do:   Normalize after Cut order fix
        If gap IsNot Nothing Then
            If gapCff AndAlso cutting AndAlso gap.DecodingMode = AudioDecodingMode.Pipe Then
                gap.NormalizeFF()
            End If
        End If

        If TypeOf ap IsNot MuxAudioProfile AndAlso Not ap.IsInputSupported Then
            Convert(ap)
        End If
    End Sub

    Shared Function GetBaseNameForStream(path As String, stream As AudioStream) As String
        Dim base As String = path.Base

        If p.TempDir.EndsWithEx("_temp\") Then
            Dim psfB As String = p.SourceFile.Base
            If base.StartsWithEx(psfB) Then
                base = If(psfB.Length + 1 < base.Length, base.Substring(psfB.Length), ShortBegEnd(base))
                'To Do: empty pipe streams temp files
            End If
        End If

        If base.NullOrEmptyS Then
            base = "temp"
        End If

        Dim ret = base & " ID" & (stream.Index + 1)

        If stream.Delay <> 0 Then
            ret &= " " & stream.Delay & "ms"
        End If

        'If Not stream.Language.TwoLetterCode.Equals("iv") Then
        If stream.Language.LCID <> 127 Then
            ret &= " " & stream.Language.ToString
        End If

        If stream.Title?.Length > 0 Then
            ret &= " {" & stream.Title.Shorten(50).EscapeIllegalFileSysChars & "}"
        End If

        Return ret.Trim
    End Function

    Shared Sub Convert(ap As AudioProfile)
        If ap.File.Ext.Equals(ap.ConvertExt) Then
            Exit Sub
        End If

        If ap.File.Ext.Equals("avs") Then
            Dim outPath = ap.File.DirAndBase & "." & ap.ConvertExt
            Dim args = "-i " & ap.File.Escape & " -y -hide_banner " & outPath.Escape

            Using proc As New Proc
                proc.Header = "AVS to " & outPath.Ext.ToUpper(InvCult)
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

        If p.Script.GetFilter("Source").Script.IndexOf("directshowsource", StringComparison.OrdinalIgnoreCase) >= 0 Then
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

        If Not FileTypes.NicAudioInput.ContainsString(ap.File.Ext) Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = p.TempDir & ap.File.Base & "_cut_na.wav"
        d.Path = (p.TempDir & ap.File.Base & "_cut_na.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter(GetNicAudioCode(ap)))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " & d.Path.Escape & " -y -hide_banner " & wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) & BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath, ap.FileKeyHashValue))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertNicAudio(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not FileTypes.NicAudioInput.ContainsString(ap.File.Ext) Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = p.TempDir & ap.File.Base & "_DecodeNicAudio." & ap.ConvertExt
        d.Path = (p.TempDir & ap.File.Base & "_DecodeNicAudio.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter(GetNicAudioCode(ap)))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " & d.Path.Escape & " -y -hide_banner " & outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) & BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath, ap.FileKeyHashValue))
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
                If FileTypes.NicAudioInput.ContainsString(ap.File.Ext) AndAlso
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
                Return "AudioDub(last, NicAC3Source(""" & ap.File & """, Channels = " & ap.Channels & "))"
            Case "mpa", "mp2", "mp3"
                Return "AudioDub(last, NicMPASource(""" & ap.File & """))"
            Case "wav"
                Return "AudioDub(last, RaWavSource(""" & ap.File & """, Channels = " & ap.Channels & "))"
        End Select
    End Function

    Shared Sub ConvertEac3to(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not FileTypes.eac3toInput.ContainsString(ap.File.Ext) Then
            Exit Sub
        End If

        Dim outPath = p.TempDir & ap.File.Base & "." & ap.ConvertExt
        Dim args = ap.File.Escape & " " & outPath.Escape

        If ap.Channels = 6 Then
            args &= " -down6"
        ElseIf ap.Channels = 2 Then
            args &= " -down2"
        End If

        Dim gap = DirectCast(ap, GUIAudioProfile)
        If gap IsNot Nothing Then
            If gap.Params.Normalize Then
                args &= " -normalize"
                gap.Params.Normalize = True
            End If
        End If

        args &= " -simple -progressnumbers"

        Using proc As New Proc
            proc.Header = "Convert " & ap.File.Ext.ToUpper(InvCult) & " to " & outPath.Ext.ToUpper(InvCult) & " " & ap.GetTrackID
            proc.Package = Package.eac3to
            proc.Arguments = args
            proc.TrimChars = {"-"c, " "c}
            proc.SkipStrings = {"process:", "analyze:"}
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath, ap.FileKeyHashValue))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertFF(ap As AudioProfile)
        Dim apConvExt As String = ap.ConvertExt

        If ap.File.Ext.Equals(apConvExt) Then
            Exit Sub
        End If

        Dim gap = TryCast(ap, GUIAudioProfile)

        'Cut fail fix, normalize after cut which failed
        If gap IsNot Nothing Then
            If gap.DecodingMode <> AudioDecodingMode.Pipe OrElse gap.Decoder <> AudioDecoderMode.Automatic OrElse gap.SupportsNormalize Then
                gap.NormalizeFF()
                'gap.Params.Normalize = False
            End If
        End If

        Dim outPath = (p.TempDir & ap.File.Base & "." & apConvExt).LongPathPrefix

        If ap.File.Equals(outPath) Then
            outPath &= "." & apConvExt
        End If

        Dim args As New StringBuilder(256)

        If gap?.Params.ProbeSize <> 5 Then
            args.Append(" -probesize ").Append(gap.Params.ProbeSize).Append("M")
        End If

        If gap?.Params.AnalyzeDuration <> 5 Then
            args.Append(" -analyzeduration ").Append(gap.Params.AnalyzeDuration).Append("M")
        End If

        args.Append(" -sn -vn -dn -i ").Append(ap.File.LongPathPrefix.Escape)

        If Not ap.Stream Is Nothing Then
            args.Append(" -sn -vn -dn -map 0:").Append(ap.Stream.StreamOrder)
        End If

        If gap?.Params.ffmpegDither <> FFDither.Disabled Then
            args.Append(" -dither_method ").Append([Enum](Of FFDither).ToString(gap.Params.ffmpegDither))
        End If

        If gap?.Params.ffmpegResampSOX Then
            args.Append(" -resampler soxr -precision 28")
        End If

        'p.Ranges.Count = 0 OrElse
        '(p.Ranges.Count > 0 AndAlso ap.File.Contains("_cut_")) OrElse
        '(p.Ranges.Count > 0 AndAlso gap?.DecodingMode <> AudioDecodingMode.Pipe) Or

        'Not (p.Ranges.Count > 0 AndAlso Not ap.File.Contains("_cut_"))
        Dim fpp As Integer
        If gap?.DecodingMode <> AudioDecodingMode.Pipe OrElse gap?.Decoder <> AudioDecoderMode.Automatic OrElse gap?.SupportsNormalize Then
            If gap.Params.Normalize Then
                Select Case gap.Params.ffmpegNormalizeMode
                    Case ffmpegNormalizeMode.dynaudnorm
                        args.Append(" ").Append(Audio.GetDynAudNormArgs(gap.Params))
                        If ap.Gain <> 0 Then
                            args.Append(",volume=").Append(ap.Gain.ToInvStr).Append("dB")
                        End If
                    Case ffmpegNormalizeMode.loudnorm
                        'args.Append(" ").Append(Audio.GetLoudNormArgs(gap.Params, args))
                        args.Append(" ").Append(Audio.GetLoudNormArgs(gap.Params))
                        If ap.Gain <> 0 Then
                            args.Append(",volume=").Append(ap.Gain.ToInvStr).Append("dB")
                        End If
                        If gap.Params.SamplingRate = 0 Then     'Loudnorm auto x4 upsample
                            args.Append(" -ar ").Append(gap.SourceSamplingRate)
                        End If
                    Case ffmpegNormalizeMode.peak
                        If ap.Gain <> 0 Then
                            args.Append(" -af volume=").Append(ap.Gain.ToInvStr).Append("dB")
                            fpp += 1
                        End If
                End Select
                gap.Params.Normalize = False

            ElseIf ap.Gain <> 0 Then
                args.Append(" -af volume=").Append(ap.Gain.ToInvStr).Append("dB")
                fpp += 1
            End If

            ap.Gain = 0
        End If

        If gap?.Params.SamplingRate <> 0 Then
            args.Append(" -ar ").Append(gap.Params.SamplingRate)
        End If

        If gap?.Params.ChannelsMode <> ChannelsMode.Original Then
            args.Append(" -rematrix_maxval 1 -ac ").Append(ap.Channels)
            fpp += 1
            If gap.Params.ffmpegLFEMixLevel <> 0 AndAlso gap.Params.ChannelsMode < 3 Then
                args.Append(" -lfe_mix_level ").Append(gap.Params.ffmpegLFEMixLevel.ToInvStr)
            End If
        End If

        If gap?.DecodingMode = AudioDecodingMode.Pipe Then 'PIPE: keep intermediate files from loosing precision
            If apConvExt.Equals("wv") Then
                args.Append(" -compression_level 1")
                If fpp > 0 Then
                    args.Append(" -sample_fmt fltp")
                End If
            ElseIf apConvExt.Equals("wav") Then
                args.Append(" -c:a pcm_f32le")
            End If
        Else
            If apConvExt.Equals("wv") Then
                args.Append(" -compression_level 1")

                Select Case gap?.Depth
                    Case 24
                        args.Append(" -sample_fmt s32p")
                    Case 32
                        args.Append(" -sample_fmt fltp")
                    Case 16
                        args.Append(" -sample_fmt s16p")
                    Case Else
                        'should ffmpeg auto choose ???
                        If fpp > 0 Then
                            args.Append(" -sample_fmt fltp")
                        End If
                End Select
            ElseIf apConvExt.Equals("wav") Then

                Select Case gap?.Depth
                    Case 24
                        args.Append(" -c:a pcm_s24le")
                    Case 32
                        args.Append(" -c:a pcm_f32le")
                    Case 16
                        args.Append(" -c:a pcm_s16le")
                    Case Else
                        args.Append(" -c:a pcm_f32le")
                End Select
            End If
        End If

        args.Append(" -y -hide_banner").Append(s.GetFFLogLevel(FfLogLevel.info))

        args.Append(" ").Append(outPath.Escape)

        Using proc As New Proc
            proc.Header = "Convert " & ap.File.Ext.ToUpper(InvCult) & " to " & outPath.Ext.ToUpper(InvCult) & " " & ap.GetTrackID
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args.ToString()
            proc.Duration = ap.GetDuration()
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            If outPath.StartsWith(LongPathPref, StringComparison.Ordinal) Then
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
            Log.WriteLine(MediaInfo.GetSummary(outPath, ap.FileKeyHashValue))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    'Shared Sub GetLoudNormArgs(sb As StringBuilder, params As GUIAudioProfile.Parameters) 'As String
    Shared Function GetLoudNormArgs(params As GUIAudioProfile.Parameters) As String
        ''Dim sb As New StringBuilder("-af loudnorm=I=", 144)
        ''Return
        'sb.Append("-af loudnorm=I=").Append(params.ffmpegLoudnormIntegrated.ToInvStr).Append(
        '    ":TP=").Append(params.ffmpegLoudnormTruePeak.ToInvStr).Append(
        '    ":LRA=").Append(params.ffmpegLoudnormLRA.ToInvStr).Append(
        '    ":offset=").Append(params.ffmpegLoudnormOffset.ToInvStr).Append(
        '    ":measured_I=").Append(params.ffmpegLoudnormIntegratedMeasured.ToInvStr).Append(
        '    ":measured_TP=").Append(params.ffmpegLoudnormTruePeakMeasured.ToInvStr).Append(
        '    ":measured_LRA=").Append(params.ffmpegLoudnormLraMeasured.ToInvStr).Append(
        '    ":measured_thresh=").Append(params.ffmpegLoudnormThresholdMeasured.ToInvStr).Append(
        '    ":print_format=summary") '.ToString
    End Function

    'Shared Sub GetDynAudNormArgs(sb As StringBuilder, params As GUIAudioProfile.Parameters) 'As String
    Shared Function GetDynAudNormArgs(params As GUIAudioProfile.Parameters) As String
        ''Dim sb As New StringBuilder("-af dynaudnorm=", 64)
        'sb.Append("-af dynaudnorm=")
        'Dim oldLen = sb.Length

        'If params.ffmpegDynaudnormF <> 500 Then sb.Append(":f=").Append(params.ffmpegDynaudnormF.ToInvStr)
        'If params.ffmpegDynaudnormG <> 31 Then sb.Append(":g=").Append(params.ffmpegDynaudnormG.ToInvStr)
        'If params.ffmpegDynaudnormP <> 0.95 Then sb.Append(":p=").Append(params.ffmpegDynaudnormP.ToInvStr)
        'If params.ffmpegDynaudnormM <> 10 Then sb.Append(":m=").Append(params.ffmpegDynaudnormM.ToInvStr)
        'If params.ffmpegDynaudnormR <> 0 Then sb.Append(":r=").Append(params.ffmpegDynaudnormR.ToInvStr)
        'If params.ffmpegDynaudnormS <> 0 Then sb.Append(":s=").Append(params.ffmpegDynaudnormS.ToInvStr)
        'If Not params.ffmpegDynaudnormN Then sb.Append(":n=false")
        'If params.ffmpegDynaudnormC Then sb.Append(":c=true")
        'If params.ffmpegDynaudnormB Then sb.Append(":b=true")

        'If sb.Length > oldLen Then
        '    sb.Remove(oldLen, 1) '"-af dynaudnorm=" & sb.Trim(":"c)
        'Else
        '    sb.Remove(oldLen - 1, 1) '"-af dynaudnorm"
        'End If
        ''sb.Remove(If(sb.Length > oldLen, oldLen, oldLen - 1), 1)
    End Function

    Shared Sub ConvertDirectShowSource(ap As AudioProfile, Optional useFlac As Boolean = False) 'WavPack better
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
        Dim outPath = p.TempDir & ap.File.Base & "_convDSS." & ap.ConvertExt
        d.Path = (p.TempDir & ap.File.Base & "_DecDSS.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,DirectShowSource(""" & ap.File & """, video=false))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " & d.Path.Escape & " -y -hide_banner " & outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) & BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath, ap.FileKeyHashValue))
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
        Dim cachefile = p.TempDir & ap.File.Base & ".ffindex"
        g.ffmsindex(ap.File, cachefile)
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = p.TempDir & ap.File.Base & "_convFFAudioSource." & ap.ConvertExt
        d.Path = (p.TempDir & ap.File.Base & "_DecodeFFAudioSource.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,FFAudioSource(""" & ap.File & """, cachefile=""" & cachefile & """))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " & d.Path.Escape & " -y -hide_banner " & outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) & BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath, ap.FileKeyHashValue))
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
        Dim wavPath = p.TempDir & ap.File.Base & "_cut_ds.wav"
        d.Path = (p.TempDir & ap.File.Base & "_cut_ds.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,DirectShowSource(""" & ap.File & """, video=false))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " & d.Path.Escape & " -y -hide_banner " & wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) & BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath, ap.FileKeyHashValue))
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
        Dim cachefile = p.TempDir & ap.File.Base & ".ffindex"
        g.ffmsindex(ap.File, cachefile)
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = p.TempDir & ap.File.Base & "_cut_ff.wav"
        d.Path = (p.TempDir & ap.File.Base & "_cut_ff.avs").ToShortFilePath
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,FFAudioSource(""" & ap.File & """, cachefile=""" & cachefile & """))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " & d.Path.Escape & " -y -hide_banner " & wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) & BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath, ap.FileKeyHashValue))
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

        Dim aviPath = p.TempDir & ap.File.Base & "_cut_mm.avi"
        Dim d = (p.CutFrameCount / p.CutFrameRate).ToString("f9", InvCult)
        Dim r = p.CutFrameRate.ToString("f9", InvCult)
        Dim args = $"-f lavfi -i color=c=black:s=16x16:d={d}:r={r} -y -hide_banner -c:v copy {aviPath.Escape}"

        Using proc As New Proc
            proc.Header = "Create avi file for audio cutting"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog("mkvmerge cannot cut audio without video so a avi file has to be created" & BR2)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If Not File.Exists(aviPath) Then
            Throw New ErrorAbortException("Error", "Output file missing")
        Else
            Log.WriteLine(MediaInfo.GetSummary(aviPath, ap.FileKeyHashValue))
        End If

        Dim mkvPath = p.TempDir & ap.File.Base & "_cut_.mkv"

        Dim args2 = "-o " & mkvPath.Escape & " " & aviPath.Escape & " " & ap.File.Escape
        args2 &= " --split parts-frames:" & String.Join(",&", p.Ranges.ToArray.SelectF(Function(v) v.Start & "-" & (v.End + 1).ToInvStr))
        args2 &= " --ui-language en"

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
            Log.WriteLine(MediaInfo.GetSummary(mkvPath, ap.FileKeyHashValue))
            Dim streams = MediaInfo.GetAudioStreams(mkvPath, ap.FileKeyHashValue)

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
        If ap.File?.Length > 0 Then
            Dim gap = TryCast(ap, GUIAudioProfile)
            If gap IsNot Nothing Then
                If gap.GetEncoder = encoder Then
                    Return True
                End If
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

'Public Enum ffNormalizeMode
'    <UI.DispName("(true)peak based")> peak
'    loudnorm
'    dynaudnorm
'End Enum

Public Enum ffmpegNormalizeMode
    <UI.DispName("(true)peak based")> peak
    loudnorm
    dynaudnorm
End Enum