
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports JM.LinqFaster
Imports JM.LinqFaster.SIMD
Imports KGySoft.CoreLibraries
Imports Microsoft.Win32
Imports VB6 = Microsoft.VisualBasic
Imports Force.DeepCloner

Module StringExtensions
    Public SeparatorD As Char = Path.DirectorySeparatorChar
    'Public InvalidFileSystemCh As Char() = {":"c, "\"c, "/"c, "?"c, """"c, "|"c, ">"c, "<"c, "*"c, "^"c}
    Public InvalidFSChI16 As UShort() = {":"c, "\"c, "/"c, "?"c, """"c, "|"c, ">"c, "<"c, "*"c, "^"c}.SelectF(Function(c) Convert.ToUInt16(c))
    'Public InvalidFileCh As Char() = {":"c, "\"c, "/"c, "?"c, """"c, "|"c, ">"c, "<"c, "*"c}
    Public InvalidFChI16 As UShort() = {":"c, "\"c, "/"c, "?"c, """"c, "|"c, ">"c, "<"c, "*"c}.SelectF(Function(c) Convert.ToUInt16(c))
    'Public EscapeCh As Char() = {" "c, "("c, ")"c, ";"c, "="c, "~"c, "*"c, "&"c, "$"c, "%"c}
    Public EscapeChI16 As UShort() = {" "c, "("c, ")"c, ";"c, "="c, "~"c, "*"c, "&"c, "$"c, "%"c}.SelectF(Function(c) Convert.ToUInt16(c))
    'Public EscapeChHS As HashSet(Of Char) = EscapeCh.ToHashSet

    <Extension>
    Function TrimTrailingSeparator(instance As String) As String
        If instance Is Nothing Then Return ""
        If instance.Length > 3 Then Return instance.TrimEnd(SeparatorD)
        Return instance
    End Function

    <Extension()>
    Function Parent(path As String) As String
        If path Is Nothing Then Return ""

        Dim temp = If(path.Length > 3, path.TrimEnd({SeparatorD}), path)
        Dim si As Integer = temp.LastIndexOf(SeparatorD)

        If si >= 0 Then
            path = temp.Substring(0, si + 1)
        End If

        Return path
    End Function

    <Extension>
    Function IndentLines(instance As String, value As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        instance = value & instance
        instance = instance.Replace(BR, BR & value)
        Return instance
    End Function

    <Extension>
    Function StartsWithEx(instance As String, value As String) As Boolean
        If instance IsNot Nothing AndAlso value.NotNullOrEmptyS Then
            Return instance.StartsWith(value, StringComparison.Ordinal)
        End If
    End Function

    <Extension>
    Function EndsWithEx(instance As String, value As String) As Boolean
        If instance IsNot Nothing AndAlso value IsNot Nothing Then
            Return instance.EndsWith(value, StringComparison.Ordinal)
        End If
    End Function

    <Extension>
    Function ContainsEx(instance As String, value As String) As Boolean
        If instance IsNot Nothing AndAlso value IsNot Nothing Then
            Return instance.Contains(value)
        End If
    End Function

    <Extension>
    Function ToLowerEx(instance As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        Return instance.ToLowerInvariant
    End Function

    <Extension>
    Function TrimEx(instance As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        Return instance.Trim
    End Function

    <Extension>
    Function PathStartsWith(instance As String, value As String) As Boolean
        If instance IsNot Nothing AndAlso value.NotNullOrEmptyS Then
            Return instance.ToLowerInvariant.StartsWith(value.ToLowerInvariant, StringComparison.Ordinal)
        End If
    End Function

    <Extension>
    Sub ThrowIfContainsNewLine(instance As String)
        If instance?.Contains(BR) Then
            Throw New Exception("String contains a line break char: " + instance)
        End If
    End Sub

    <Extension>
    Function Multiply(instance As String, multiplier As Integer) As String
        If multiplier < 1 Then
            multiplier = 1
        End If

        Dim sb As New StringBuilder(multiplier * instance.Length)

        For i = 1 To multiplier
            sb.Append(instance)
        Next

        Return sb.ToString()
    End Function

    <Extension>
    Function IsValidFileName(instance As String) As Boolean
        If instance Is Nothing Then
            Return False
        End If

        For i = 0 To instance.Length - 1
            Dim chInt As UShort = Convert.ToUInt16(instance(i))

            If chInt < 32 OrElse InvalidFChI16.ContainsS(chInt) Then 'If InvalidFileCh.ContainsF(i) Then
                Return False
            End If
        Next i

        Return True
    End Function

    <Extension>
    Function IsDosCompatible(instance As String) As Boolean
        If instance.NullOrEmptyS Then
            Return True
        End If

        For i = 0 To instance.Length - 1 'TODO: Tests!!! or merge with Stax
            If Convert.ToUInt16(instance(i)) > 127 Then Return False
        Next i
        Return True

        'Dim bytes = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(ConsoleHelp.DosCodePage), Encoding.Unicode.GetBytes(instance))
        'Return instance.Equals(Encoding.Unicode.GetString(Encoding.Convert(Encoding.GetEncoding(ConsoleHelp.DosCodePage), Encoding.Unicode, bytes)))
    End Function

    <Extension>
    Function IsANSICompatible(instance As String) As Boolean
        If instance.NullOrEmptyS Then
            Return True
        End If

        Dim bytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, Encoding.Unicode.GetBytes(instance))
        Return instance = Encoding.Unicode.GetString(Encoding.Convert(Encoding.Default, Encoding.Unicode, bytes))
    End Function

    <Extension()>
    Function FileName(instance As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        Dim index = instance.LastIndexOf(SeparatorD)

        If index > -1 Then
            Return instance.Substring(index + 1)
        End If

        Return instance
    End Function

    <Extension()>
    Function Upper(instance As String) As String
        If instance Is Nothing Then Return ""
        Return instance.ToUpperInvariant
    End Function

    <Extension()>
    Function Lower(instance As String) As String
        If instance Is Nothing Then Return ""
        Return instance.ToLowerInvariant
    End Function

    <Extension()>
    Function ChangeExt(instance As String, value As String) As String
        If instance.NullOrEmptyS Then Return ""
        If value.NullOrEmptyS Then Return instance

        If Not value.StartsWith(".", StringComparison.Ordinal) Then
            value = "." & value
        End If

        Return instance.DirAndBase & value.ToLowerInvariant
    End Function

    <Extension()>
    Function Escape(instance As String) As String
        If instance.NullOrEmptyS Then
            Return ""
        End If
        'For iCh = 0 To EscapeChI16.Length - 1 'Worst Perf - string-ForLoop->ChArray.Contains is Faster than string.contains, string.ToCharArray.containsF is Faster;'2550ms-ContainsF ; ForLoop 2400ms ; 2xForLoop NoCharArray 2350ms-Fastest NoSimd,Simd~1200ms
        'Dim iChA = instance.ToCharArray.SelectF(Function(ch) Convert.ToUInt16(ch))

        ''Dim instHS = instance.ToHashSet
        'If EscapeChHS.Overlaps(instance) Then
        '    Return """" & instance & """"
        'End If

        Dim iChA(instance.Length - 1) As UShort
        For i = 0 To instance.Length - 1
            iChA(i) = Convert.ToUInt16(instance(i))
        Next i

        For iCh = 0 To EscapeChI16.Length - 1
            'If instHS.Contains(EscapeCh(iCh)) Then Return """" & instance & """" ' try with HashSet
            If iChA.ContainsS(EscapeChI16(iCh)) Then Return """" & instance & """"
            'For c = 0 To instance.Length - 1
            'If instance(c) = EscapeCh(iCh) Then
            ' Return """" & instance & """"
            'End If
            'Next c
        Next iCh

        Return instance
    End Function

    <Extension()>
    Function ExistingParent(instance As String) As String
        Dim ret = instance.Parent
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        Return ret
    End Function

    <Extension()>
    Function FileExists(instance As String) As Boolean
        Return File.Exists(instance)
    End Function

    <Extension()>
    Function DirExists(instance As String) As Boolean
        Return Directory.Exists(instance)
    End Function

    <Extension()>
    Function Ext(instance As String) As String
        If instance Is Nothing Then Return ""

        For x = instance.Length - 1 To 0 Step -1
            If instance(x) = SeparatorD Then Return ""

            If instance(x) = "."c Then
                Return instance.Substring(x + 1).ToLowerInvariant
            End If
        Next

        Return ""
    End Function

    <Extension()>
    Function ExtFull(instance As String) As String
        If instance Is Nothing Then Return ""

        For x = instance.Length - 1 To 0 Step -1
            If instance(x) = SeparatorD Then Return ""

            If instance(x) = "."c Then
                Return instance.Substring(x).ToLowerInvariant
            End If
        Next

        Return ""
    End Function

    <Extension()>
    Function Base(instance As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        Dim ret = instance
        Dim idx As Integer = ret.LastIndexOf(SeparatorD)
        If idx >= 0 Then ret = ret.Substring(idx + 1)

        idx = ret.LastIndexOf("."c)
        If idx > 0 Then ret = ret.Substring(0, idx)

        Return ret
    End Function

    <Extension()>
    Function Dir(instance As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        Dim ln As Integer = instance.LastIndexOf(SeparatorD)

        If ln >= 0 Then
            instance = instance.Substring(0, ln + 1)
        End If

        Return instance
    End Function

    <Extension()>
    Function LongPathPrefix(instance As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        Dim MAX_PATH = 260
        Dim prefix = "\\?\"

        If instance.Length > MAX_PATH AndAlso Not instance.StartsWith(prefix, StringComparison.Ordinal) Then
            Return prefix & instance
        End If

        Return instance
    End Function

    <Extension()>
    Function ToShortFilePath(instance As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        If instance.StartsWith("\\?\", StringComparison.Ordinal) Then
            instance = instance.Substring(4)
        End If

        Const MAX_PATH = 260

        If instance.Length <= MAX_PATH Then
            Return instance
        End If

        Dim sb As New StringBuilder(MAX_PATH)
        Native.GetShortPathName(instance.Dir, sb, sb.Capacity)
        Dim ret = sb.ToString & instance.FileName

        If ret.Length <= MAX_PATH Then
            Return ret
        End If

        Native.GetShortPathName(instance, sb, sb.Capacity)

        Return sb.ToString
    End Function

    <Extension()>
    Function DirName(instance As String) As String
        If instance Is Nothing Then
            Return ""
        End If

        If instance.Length > 3 Then
            instance = instance.TrimEnd(SeparatorD)
        End If

        Dim idx As Integer = instance.LastIndexOf(SeparatorD)
        Return If(idx >= 0, instance.Substring(idx + 1), "")

    End Function

    <Extension()>
    Function DirAndBase(path As String) As String
        Return path.Dir & path.Base
    End Function

    <Extension()>
    Function ContainsAll(instance As String, ParamArray all As String()) As Boolean
        If instance.NotNullOrEmptyS Then
            Return all.AllF(Function(arg) instance.Contains(arg))
        End If
    End Function

    <Extension()>
    Function ContainsAny(instance As String, ParamArray any As String()) As Boolean
        If instance.NotNullOrEmptyS AndAlso any IsNot Nothing Then
            For i = 0 To any.Length - 1
                If instance.Contains(any(i)) Then Return True
            Next i
        End If

        Return False
    End Function

    <Extension()>
    Function EqualsEx(instance As String, value As String) As Boolean 'marginally slower for nulls/"", 4xfaster in NoEmpt strings than =/<>
        If instance Is Nothing Then ' if instance="" andalso value="" return true
            instance = ""
        End If
        If value Is Nothing Then
            value = ""
        End If
        Return String.Equals(value, instance)
    End Function

    <Extension()>
    Function EqualsAny(instance As String, ParamArray values As String()) As Boolean
        If instance.NullOrEmptyS OrElse values Is Nothing Then
            Return False
        End If
        For i = 0 To values.Length - 1
            If String.Equals(values(i), instance) Then Return True
        Next i
        Return False
    End Function

    <Extension()>
    Public Function NullOrEmptyS(instance As String) As Boolean
        Return instance Is Nothing OrElse instance Is ""
        'Return String.IsNullOrEmpty(instance) ' Test this !!!
    End Function

    <Extension()>
    Public Function NotNullOrEmptyS(instance As String) As Boolean
        Return instance IsNot Nothing AndAlso instance IsNot ""
        'Return Not String.IsNullOrEmpty(instance) ' Test this !!!
    End Function

    '<Extension()>
    'Public Function NullOrWhiteSpace(instance As String) As Boolean
    '    Return String.IsNullOrWhiteSpace(instance) ' Test this !!!
    'End Function

    <Extension()>
    Function FixDir(instance As String) As String
        If instance.NullOrEmptyS Then Return "" ' is nothing
        Dim c As Integer
        For i = instance.Length - 1 To 0 Step -1
            If instance(i) = SeparatorD Then
                c += 1
            Else
                Select Case c
                    Case 0
                        Return instance & SeparatorD
                    Case 1
                        Return instance
                    Case Is > 1
                        Return instance.Substring(0, instance.Length - c + 1)
                End Select
            End If
        Next i
        Return SeparatorD ' If(instance isnot "", SeparatorD, "")
    End Function

    <Extension()>
    Function FixBreak(value As String) As String
        value = value.Replace(VB6.ChrW(13) & VB6.ChrW(10), VB6.ChrW(10))
        value = value.Replace(VB6.ChrW(13), VB6.ChrW(10))
        Return value.Replace(VB6.ChrW(10), VB6.ChrW(13) & VB6.ChrW(10))
    End Function

    <Extension()>
    Function ToTitleCase(value As String) As String
        'TextInfo.ToTitleCase won't work on all upper strings
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLowerInvariant)
    End Function

    <Extension()>
    Function IsInt(value As String) As Boolean
        Return Integer.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, Nothing)
    End Function

    <Extension()>
    Function ToInt(value As String, Optional defaultValue As Integer = 0I) As Integer
        Dim ret As Integer
        Return If(Integer.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, ret), ret, defaultValue)
    End Function

    <Extension()>
    Function ToIntM(value As String) As Integer '-&H7FFFFFFEI) 'opt -2147483646I
        Dim ret As Integer
        Return If(Integer.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, ret), ret, -2147483646I)
    End Function

    <Extension()>
    Function IsSingle(value As String) As Boolean
        If value.NotNullOrEmptyS Then
            value = value.Replace(",", ".")

            Return Single.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, Nothing)
        End If
    End Function

    <Extension()>
    Function ToSingle(value As String, Optional defaultValue As Single = 0F) As Single
        If value.NotNullOrEmptyS Then
            value = value.Replace(",", ".")

            Dim ret As Single

            If Single.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, ret) Then
                Return ret
            End If
        End If

        Return defaultValue
    End Function

    <Extension()>
    Function IsDouble(value As String) As Boolean
        If value.NotNullOrEmptyS Then
            value = value.Replace(",", ".")

            Return Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, Nothing)
        End If
    End Function

    <Extension()>
    Function ToDouble(value As String, Optional defaultValue As Double = 0R) As Double
        If value.NotNullOrEmptyS Then
            value = value.Replace(",", ".")

            Dim ret As Double

            If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, ret) Then
                Return ret
            End If
        End If

        Return defaultValue
    End Function

    <Extension()>
    Function FormatColumn(value As String, delimiter As String) As String
        If value.NullOrEmptyS Then Return ""
        Dim lines = value.SplitKeepEmpty(BR)
        Dim leftSides As New List(Of String)
        Dim highest As Integer

        For Each i In lines
            Dim pos = i.IndexOf(delimiter, StringComparison.Ordinal)

            If pos > 0 Then
                Dim st As String = i.Substring(0, pos).Trim
                If st.Length > highest Then highest = st.Length
                leftSides.Add(st)
            Else
                If i.Length > highest Then highest = i.Length
                leftSides.Add(i)
            End If
        Next

        'highest = Aggregate i In leftSides Into Max(i.Length)
        'highest = leftSides.MaxF(Function(le) le.Length)
        Dim ret As New List(Of String)

        For i = 0 To lines.Length - 1
            Dim line = lines(i)

            If line.Contains(delimiter) Then
                ret.Add(leftSides(i).PadRight(highest) + " " + delimiter + " " + line.Substring(line.IndexOf(delimiter, StringComparison.Ordinal) + 1).Trim)
            Else
                ret.Add(leftSides(i))
            End If
        Next

        Return ret.Join(BR)
    End Function

    <Extension()>
    Function ReadAllText(instance As String) As String
        If Not File.Exists(instance) Then
            Return ""
        End If

        Return File.ReadAllText(instance)
    End Function

    <Extension()>
    Function ReadAllTextDefault(instance As String) As String
        If Not File.Exists(instance) Then
            Return ""
        End If

        Return File.ReadAllText(instance, Encoding.Default)
    End Function

    <Extension()>
    Sub WriteFileDefault(instance As String, path As String)
        WriteFile(instance, path, Encoding.Default)
    End Sub

    <Extension()>
    Sub WriteFileUTF8(instance As String, path As String)
        WriteFile(instance, path, New UTF8Encoding(False))
    End Sub

    <Extension()>
    Sub WriteFileUTF8BOM(instance As String, path As String)
        WriteFile(instance, path, New UTF8Encoding(True))
    End Sub

    <Extension()>
    Sub WriteFile(instance As String, path As String, encoding As Encoding)
        Try
            File.WriteAllText(path, instance, encoding)
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Extension()>
    Function Left(value As String, index As Integer) As String
        If value Is Nothing OrElse index < 0 Then
            Return ""
        End If

        If index > value.Length Then
            Return value
        End If

        Return value.Substring(0, index)
    End Function

    <Extension()>
    Function Left(value As String, start As String) As String
        If value Is Nothing Then 'OrElse start Is Nothing Then
            Return ""
        End If

        Dim ln As Integer = value.IndexOf(start, StringComparison.Ordinal)
        Return If(ln > 0, value.Substring(0, ln), "")

    End Function

    <Extension()>
    Function LeftLast(value As String, start As String) As String

        Dim ln As Integer = value.LastIndexOf(start, StringComparison.Ordinal)
        Return If(ln > 0, value.Substring(0, ln), "")

    End Function

    <Extension()>
    Function Right(value As String, start As String) As String
        If value Is Nothing Then ' OrElse start.Length <= 0 OrElse start.NullOrEmptyS MUST be Empty String 'start' proof!
            Return ""
        End If

        Dim idx As Integer = value.IndexOf(start, StringComparison.Ordinal)
        Return If(idx >= 0, value.Substring(idx + start.Length), "")
    End Function

    <Extension()>
    Function RightLast(value As String, start As String) As String
        If value Is Nothing Then ' OrElse start.Length <= 0 OrElse start.NullOrEmptyS MUST be Empty String 'start' proof!
            Return ""
        End If

        Dim idx As Integer = value.LastIndexOf(start, StringComparison.Ordinal)
        Return If(idx >= 0, value.Substring(idx + start.Length), "")
    End Function

    <Extension()>
    Function IsEqualIgnoreCase(value1 As String, value2 As String) As Boolean
        Return String.Equals(value1, value2, StringComparison.OrdinalIgnoreCase)
    End Function

    <Extension()>
    Function Shorten(value As String, maxLength As Integer) As String
        If value Is Nothing OrElse value.Length <= maxLength Then
            Return value
        End If

        Return value.Substring(0, maxLength)
    End Function

    <Extension()>
    Function ShortBegEnd(instance As String, Optional StartLen As Integer = 32, Optional EndLen As Integer = 18) As String
        If instance.Length > StartLen + EndLen + 1 Then
            Return instance.Substring(0, StartLen) & "_" & instance.Substring(instance.Length - EndLen)
        Else
            Return instance
        End If
    End Function

    <Extension()>
    Function IsValidFileSystemName(instance As String) As Boolean
        If instance Is Nothing Then
            Return False
        End If

        For i = 0 To instance.Length
            Dim chInt As UShort = Convert.ToUInt16(instance(i))
            If chInt < 32 OrElse InvalidFSChI16.ContainsS(chInt) Then ''If InvalidFileSystemCh.ContainsF(i) Then
                Return False
            End If
        Next i

        Return True
    End Function

    <Extension()>
    Function IsSameBase(instance As String, b As String) As Boolean
        Return instance.Base.IsEqualIgnoreCase(b.Base)
    End Function

    <Extension()>
    Function EscapeIllegalFileSysChars(value As String) As String
        If value Is Nothing Then Return ""

        For i = 0 To value.Length - 1
            Dim chInt As UShort = Convert.ToUInt16(value(i))
            If chInt < 32 OrElse InvalidFSChI16.ContainsS(chInt) Then
                value = value.Replace(value(i), "__" + Uri.EscapeDataString(value(i)).TrimStart("%"c) + "__")
            End If
        Next

        Return value
    End Function

    <Extension()>
    Function UnescapeIllegalFileSysChars(value As String) As String
        If value Is Nothing Then
            Return ""
        End If

        For Each match As Match In Regex.Matches(value, "__(\w\w)__")
            value = value.Replace(match.Value, Uri.UnescapeDataString("%" + match.Groups(1).Value))
        Next

        Return value
    End Function

    <Extension()>
    Function SplitNoEmpty(value As String, ParamArray delimiters As String()) As String()
        Return value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
    End Function

    <Extension()>
    Function SplitKeepEmpty(value As String, ParamArray delimiters As String()) As String()
        Return value.Split(delimiters, StringSplitOptions.None)
    End Function

    <Extension()>
    Function SplitNoEmptyAndNoWSDelim(value As String, ParamArray delimiters As String()) As String()
        If value Is Nothing Then Return {}
        Return value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
    End Function

    <Extension()>
    Function SplitNoEmptyAndWhiteSpace(value As String, ParamArray delimiters As Char()) As String()
        If value Is Nothing Then Return {}

        Dim a = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
        Dim ret As New List(Of String)(a.Length)

        For i = 0 To a.Length - 1
            a(i) = a(i).Trim

            If a(i) IsNot "" Then
                ret.Add(a(i))
            End If
        Next

        Return ret.ToArray
    End Function

    <Extension()>
    Function SplitLinesNoEmpty(value As String) As String()
        Return SplitNoEmpty(value, Environment.NewLine)
    End Function

    <Extension()>
    Function RemoveChars(value As String, chars As Char()) As String
        'Dim ivnC = chars.ToHashSet 'Fastest 2-3X
        'Dim ret = value.ToList
        'For iCV = ret.Count - 1 To 0 Step -1
        '    If ivnC.Contains(ret(iCV)) Then ret.RemoveAt(iCV)
        'Next iCV
        'Return New String(ret.ToArray)
        Dim ret = value
        For ic = 0 To chars.Length - 1
            ret = ret.Replace(chars(ic), "")
        Next
        Return ret
    End Function

    <Extension()>
    Function DeleteRight(value As String, count As Integer) As String
        Return Left(value, value.Length - count)
    End Function

    <Extension()>
    Function ReplaceRecursive(value As String, find As String, replace As String) As String
        If value Is Nothing Then
            Return ""
        End If

        While value.Contains(find)
            value = value.Replace(find, replace)
        End While

        Return value
    End Function

    <Extension()>
    Function MD5Hash(instance As String) As String
        Using m = MD5.Create()
            Dim inputBuffer = Encoding.UTF8.GetBytes(instance)
            Dim hashBuffer = m.ComputeHash(inputBuffer)
            Return BitConverter.ToString(m.ComputeHash(inputBuffer))
        End Using
    End Function

    <Extension()>
    Sub ToClipboard(value As String)
        If value.NotNullOrEmptyS Then
            Clipboard.SetText(value)
        Else
            Clipboard.Clear()
        End If
    End Sub

End Module

Module MiscExtensions

    Public ReadOnly ScreenResPrim As Size = Screen.PrimaryScreen.Bounds.Size
    Public ReadOnly CPUsC As Integer = Environment.ProcessorCount
    Public ReadOnly SWFreq As Double = Stopwatch.Frequency / 1000

    Public Sub WarmUpCpu()
        Dim itr As Integer = 1000000 '~200ms
        Dim wr1 As Double
        Dim wr2 As Double
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(2, GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()
        Application.DoEvents()
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(2, GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()
        For w = 1 To itr
            If w / 5.34567 > 10.2343 Then wr2 = Math.Sin(wr1 / 1.3153957) ^ (w / 45234633.2342492) / Math.Cos(wr1 / 23456.242361) * 1.34645743
            If w / 35.6346 > 25.2353 Then wr1 = Math.Atan(w / 573214.245763) * Math.Exp(wr2 / 45645531.6151) ^ (w / 73234541.435716) * 1.3234476
        Next w
        Application.DoEvents()
    End Sub

    <Extension()>
    Public Function GetDeepClone(Of T)(obj As T) As T
        Return DeepClonerExtensions.DeepClone(Of T)(obj)
    End Function

    <Extension()>
    Function ToInvariantString(value As Double, format As String) As String
        Return value.ToString(format, CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ToInvariantString(value As Double) As String
        Return value.ToString(CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ToInvariantString(value As Single) As String
        Return value.ToString(CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ToInvariantString(instance As Integer) As String
        Return instance.ToString(CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ToInvariantString(instance As Long) As String
        Return instance.ToString(CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ToInvariantString(instance As IConvertible) As String
        If instance Is Nothing Then
            Return ""
        End If

        Return instance.ToString(CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ToInvariantString(instance As Date, format As String) As String
        Return instance.ToString(format, CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ContainsString(instance As String(), value As String) As Boolean
        If instance IsNot Nothing AndAlso value.NotNullOrEmptyS Then
            For i = 0 To instance.Length - 1
                If String.Equals(instance(i), value) Then Return True
            Next i
        End If

        Return False
    End Function

    <Extension()>
    Function ContainsAny(instance As String(), ParamArray values As String()) As Boolean
        Return instance.AnyF(Function(arg) values.ContainsString(arg))
    End Function
    '<Extension()> Function ContainsAny(Of T)(instance As IEnumerable(Of T), values As IEnumerable(Of T)) As Boolean
    '    Return instance.Any(Function(arg) values.Contains(arg))
    'End Function
    <Extension()>
    Function Sort(Of T)(instance As T()) As T()
        Array.Sort(Of T)(instance)
        Return instance
    End Function

    <Extension()>
    Function Sort(Of T)(instance As IEnumerable(Of T)) As IEnumerable(Of T)
        Dim ret = instance.ToArray
        Array.Sort(Of T)(ret)
        Return ret
    End Function

    <Extension()>
    Function Join(instance As List(Of String), delimiter As String, Optional removeEmpty As Boolean = False) As String
        If instance Is Nothing Then Return Nothing
        If removeEmpty Then instance = instance.WhereF(Function(arg) arg.NotNullOrEmptyS)
        Return String.Join(delimiter, instance)
    End Function

    <Extension()>
    Function Join(instance As String(), delimiter As String, Optional removeEmpty As Boolean = False) As String
        If instance Is Nothing Then Return Nothing
        If removeEmpty Then instance = instance.WhereF(Function(arg) arg.NotNullOrEmptyS)
        Return String.Join(delimiter, instance)
    End Function

    <Extension()>
    Function Join(instance As IEnumerable(Of String), delimiter As String, Optional removeEmpty As Boolean = False) As String

        If instance Is Nothing Then
            Return Nothing
        End If

        If removeEmpty Then
            instance = instance.Where(Function(arg) arg.NotNullOrEmptyS)
        End If

        Return String.Join(delimiter, instance)
    End Function

    <Extension>
    Public Function ConcatA(Of T)(first As T(), second As T()) As T()
        'If second Is Nothing Then
        '    Return If(first, Nothing)
        'ElseIf first Is Nothing Then
        '    Return second
        'End If
        Dim ol = first.Length
        Array.Resize(Of T)(first, ol + second.Length)
        Array.Copy(second, 0, first, ol, second.Length)
        Return first
    End Function

    <Extension()>
    Function GetAttribute(Of T)(mi As MemberInfo) As T
        Dim attributes = mi.GetCustomAttributes(True)

        If Not attributes.NothingOrEmpty Then
            If attributes.Length = 1 Then
                If TypeOf attributes(0) Is T Then
                    Return DirectCast(attributes(0), T)
                End If
            Else
                For Each i In attributes
                    If TypeOf i Is T Then
                        Return DirectCast(i, T)
                    End If
                Next
            End If
        End If
    End Function

    <Extension()>
    Function IsDigitEx(c As Char) As Boolean
        Return Char.IsDigit(c)
    End Function

    <Extension()>
    Function NeutralCulture(ci As CultureInfo) As CultureInfo
        If ci.IsNeutralCulture Then
            Return ci
        Else
            Return ci.Parent
        End If
    End Function

    <Extension()>
    Function NothingOrEmpty(strings As String()) As Boolean
        If strings?.Length > 0 Then
            For i = 0 To strings.Length - 1
                If strings(i).NotNullOrEmptyS Then
                    Return False
                End If
            Next i
        End If

        Return True
    End Function

    <Extension()>
    Function NothingOrEmpty(Of T)(objects As List(Of T)) As Boolean
        If objects?.Count > 0 Then
            For i = 0 To objects.Count - 1
                If objects(i) IsNot Nothing Then
                    Return False
                End If
            Next i
        End If

        Return True
    End Function

    <Extension()>
    Function NothingOrEmpty(strings As HashSet(Of String)) As Boolean
        If strings Is Nothing Then
            Return True
        Else
            Select Case strings.Count
                Case 0
                    Return True
                Case 1
                    If strings.Contains(Nothing) OrElse strings.Contains("") Then Return True
                Case 2
                    If strings.Contains(Nothing) AndAlso strings.Contains("") Then Return True
            End Select
            Return False
        End If
    End Function

    <Extension()>
    Function NothingOrEmpty(strings As IEnumerable(Of String)) As Boolean
        If strings?.Any(Function(itm) itm.NotNullOrEmptyS) Then
            Return False
        End If

        Return True
    End Function

    <Extension()>
    Function NothingOrEmpty(objects As IEnumerable(Of Object)) As Boolean
        If objects?.Any(Function(itm) itm IsNot Nothing) Then
            Return False
        End If

        Return True
    End Function

End Module

Module RegistryKeyExtensions
    Function GetValue(Of T)(root As RegistryKey, path As String, name As String) As T
        Using key = root.OpenSubKey(path)
            If Not key Is Nothing Then
                Dim value = key.GetValue(name)

                If Not value Is Nothing Then
                    Try
                        Return CType(value, T)
                    Catch
                    End Try
                End If
            End If
        End Using
    End Function

    <Extension()>
    Function GetString(root As RegistryKey, path As String, name As String) As String
        Return GetValue(Of String)(root, path, name)
    End Function

    <Extension()>
    Function GetInt(root As RegistryKey, path As String, name As String) As Integer
        Return GetValue(Of Integer)(root, path, name)
    End Function

    <Extension()>
    Function GetBoolean(root As RegistryKey, path As String, name As String) As Boolean
        Return GetValue(Of Boolean)(root, path, name)
    End Function

    <Extension()>
    Function GetKeyNames(root As RegistryKey, path As String) As String()
        Using subKey = root.OpenSubKey(path)
            If Not subKey Is Nothing Then
                Return subKey.GetSubKeyNames
            End If
        End Using

        Return {}
    End Function

    <Extension()>
    Function GetValueNames(root As RegistryKey, path As String) As String()
        Using subKey = root.OpenSubKey(path)
            If Not subKey Is Nothing Then
                Return subKey.GetValueNames
            End If
        End Using

        Return {}
    End Function

    <Extension()>
    Sub Write(root As RegistryKey, path As String, name As String, value As Object)
        Dim subKey = root.OpenSubKey(path, True)

        If subKey Is Nothing Then
            subKey = root.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If

        subKey.SetValue(name, value)
        subKey.Close()
    End Sub

    <Extension()>
    Sub DeleteValue(root As RegistryKey, path As String, name As String)
        Using key = root.OpenSubKey(path, True)
            If Not key Is Nothing Then
                key.DeleteValue(name, False)
            End If
        End Using
    End Sub
End Module

Module ControlExtension
    <Extension()>
    Sub ScaleClientSize(instance As Control, width As Single, height As Single, Optional fontHeightC As Integer = -1)
        If fontHeightC = -1 Then fontHeightC = instance.Font.Height
        instance.ClientSize = New Size(CInt(fontHeightC * width), CInt(fontHeightC * height))
    End Sub

    <Extension()>
    Sub SetFontStyle(instance As Control, style As FontStyle)
        'instance.Font = New Font(instance.Font.FontFamily, instance.Font.Size, style) 
        instance.Font = New Font(instance.Font, style) ' Test this !!!
    End Sub

    <Extension()>
    Sub SetFontSize(instance As Control, fontSize As Single)
        instance.Font = New Font(instance.Font.FontFamily, fontSize)
    End Sub

    <Extension()>
    Sub AddClickAction(instance As Control, action As Action)
        AddHandler instance.Click, Sub() action()
    End Sub

    <Extension()>
    Function ClientMousePos(instance As Control) As Point
        Return instance.PointToClient(Control.MousePosition)
    End Function

    <Extension()>
    Function GetMaxTextSpace(instance As Control, ParamArray values As String()) As String
        Dim ret As String

        For x = 4 To 2 Step -1
            ret = values.Join("".PadRight(x))
            Dim testWidth = TextRenderer.MeasureText(ret, instance.Font).Width

            If testWidth < instance.Width Then
                Return ret
            End If
        Next

        Return ret
    End Function
End Module

Module UIExtensions
    <Extension()>
    Sub ClearAndDispose(instance As ToolStripItemCollection)
        For i = instance.Count - 1 To 0 Step -1
            'If TypeOf instance(i) Is IDisposable Then  'dim instance(i) Experiment! Test it!
            instance(i).Dispose()
            'End If
        Next i
        instance.Clear()
    End Sub

    <Extension()>
    Function ResizeIconSize(img As Image, newSize As Size) As Image

        If img IsNot Nothing AndAlso img.Size <> newSize Then
            Dim s = newSize
            Dim r As New Bitmap(s.Width, s.Height)

            Using g = Graphics.FromImage(r)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.DrawImage(img, 0, 0, s.Width, s.Height)
            End Using

            Return r
        End If

        Return img
    End Function
    '<Extension()>
    'Function ResizeImage(image As Image, ByVal height As Integer) As Image
    '    Dim percentHeight = height / image.Height
    '    Dim ret = New Bitmap(CInt(image.Width * percentHeight), CInt(height))
    '    Using g = Graphics.FromImage(ret)
    '        g.InterpolationMode = InterpolationMode.HighQualityBicubic
    '        g.DrawImage(image, 0, 0, ret.Width, ret.Height)
    '    End Using
    '    Return ret
    'End Function
    <Extension()>
    Sub SetSelectedPath(d As FolderBrowserDialog, path As String)
        If Not Directory.Exists(path) Then
            path = path.ExistingParent
        End If

        If Directory.Exists(path) Then
            d.SelectedPath = path
        End If
    End Sub

    <Extension()>
    Sub SetInitDir(dialog As FileDialog, ParamArray paths As String())
        For Each path In paths
            If Not Directory.Exists(path) Then
                path = path.ExistingParent
            End If

            If Directory.Exists(path) Then
                dialog.InitialDirectory = path
                Exit For
            End If
        Next
    End Sub
    '<Extension()> Sub SetFilter(dialog As FileDialog, values As IEnumerable(Of String))
    '    dialog.Filter = FileTypes.GetFilter(values)
    'End Sub
    <Extension()>
    Sub SendMessageCue(tb As TextBox, value As String, hideWhenFocused As Boolean)
        Dim wParam = If(hideWhenFocused, 0, 1)
        Native.SendMessage(tb.Handle, Native.EM_SETCUEBANNER, wParam, value)
    End Sub

    <Extension()>
    Sub SendMessageCue(c As ComboBox, value As String)
        Native.SendMessage(c.Handle, Native.CB_SETCUEBANNER, 1, value)
    End Sub

    Function GetPropertyValue(obj As String, propertyName As String) As Object
        obj.GetType.GetProperty(propertyName).GetValue(obj)
    End Function

End Module
