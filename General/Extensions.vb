
Imports System.Collections.Immutable
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Numerics
Imports System.Reflection
Imports System.Runtime
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Force.DeepCloner
Imports JM.LinqFaster
Imports JM.LinqFaster.SIMD
Imports KGySoft.Collections
Imports KGySoft.CoreLibraries
Imports Microsoft.VisualBasic
Imports Microsoft.Win32
Module StringExtensions
    Public Const Max_PathLen As Integer = 260
    Public Const LongPathPref As String = "\\?\"
    Public Const SeparatorD As Char = "\"c 'Path.DirectorySeparatorChar
    Public ReadOnly InvalidFSChI16 As UShort() = {AscW(":"c), AscW("\"c), AscW("/"c), AscW("?"c), AscW(""""c), AscW("|"c), AscW(">"c), AscW("<"c), AscW("*"c), AscW("^"c)} '.SelectF(Function(c) Convert.ToUInt16(c))
    Public ReadOnly InvalidFChI16 As UShort() = {AscW(":"c), AscW("\"c), AscW("/"c), AscW("?"c), AscW(""""c), AscW("|"c), AscW(">"c), AscW("<"c), AscW("*"c)} '.SelectF(Function(c) Convert.ToUInt16(c))
    Public ReadOnly EscapeChI16 As UShort() = {AscW(" "c), AscW("("c), AscW(")"c), AscW(";"c), AscW("="c), AscW("~"c), AscW("*"c), AscW("&"c), AscW("$"c), AscW("%"c)} '.SelectF(Function(c) Convert.ToUInt16(c)) 'ToDO: Revert To HashSet for Early Exit Or Not!???
    'Public EscapeCh As Char() = {" "c, "("c, ")"c, ";"c, "="c, "~"c, "*"c, "&"c, "$"c, "%"c}
    'Public EscapeChHS As New HashSet(Of Char)(EscapeCh)

    Public ReadOnly CurrCult As CultureInfo = CultureInfo.CurrentCulture
    Public ReadOnly CurrCultNeutral As CultureInfo = If(CurrCult.IsNeutralCulture, CurrCult, CurrCult.Parent)
    Public ReadOnly CurrCultTxtInfo As TextInfo = CurrCult.TextInfo

    Public ReadOnly InvCult As CultureInfo = CultureInfo.InvariantCulture
    Public ReadOnly InvCultTxtInf As TextInfo = InvCult.TextInfo
    Public ReadOnly InvCultCompInf As CompareInfo = InvCult.CompareInfo

    '<Extension>
    'Function TrimTrailingSeparator(instance As String) As String
    '    If instance Is Nothing Then Return String.Empty
    '    If instance.Length > 3 Then Return instance.TrimEnd(SeparatorD)
    '    Return instance
    'End Function
    <Extension()> <MethodImpl(AggrInlin)>
    Function Parent(path As String) As String
        If path Is Nothing Then Return String.Empty
        Dim sIdx As Integer = path.Length - 1
        If sIdx = -1 Then Return String.Empty

        While sIdx >= 3 AndAlso path(sIdx) = SeparatorD
            sIdx -= 1
        End While
        sIdx = path.LastIndexOf(SeparatorD, sIdx, sIdx + 1)
        If sIdx >= 0 Then
            path = path.Substring(0, sIdx + 1)
        End If

        'If path.Length > 3 Then path.TrimEnd(SeparatorD)
        'Dim si As Integer = path.LastIndexOf(SeparatorD)
        'If si >= 0 Then
        '    path = path.Substring(0, si + 1)
        'End If
        Return path
    End Function

    <Extension>
    Function IndentLines(instance As String, value As String) As String
        If instance Is Nothing Then
            Return String.Empty
        End If

        instance = value & instance
        instance = instance.Replace(BR, BR & value)
        Return instance
    End Function

    <Extension> <MethodImpl(AggrInlin)>
    Function StartsWithEx(instance As String, value As String) As Boolean
        If instance IsNot Nothing AndAlso value?.Length > 0 Then
            'Dim aaa = InvCompInf.IsPrefix(instance, value, CompareOptions.Ordinal)
            Return instance.StartsWith(value, StringComparison.Ordinal)
        End If
    End Function

    <Extension> <MethodImpl(AggrInlin)>
    Function EndsWithEx(instance As String, value As String) As Boolean
        If instance IsNot Nothing AndAlso value IsNot Nothing Then
            Return instance.EndsWith(value, StringComparison.Ordinal)
        End If
    End Function

    <Extension> <MethodImpl(AggrInlin)>
    Function ContainsEx(instance As String, value As String) As Boolean
        If instance IsNot Nothing AndAlso value IsNot Nothing Then
            Return InvCultCompInf.IndexOf(instance, value, CompareOptions.Ordinal) >= 0
        End If
    End Function

    <Extension> <MethodImpl(AggrInlin)>
    Function ToLowerEx(instance As String) As String
        If instance Is Nothing Then Return String.Empty
        Return instance.ToLower(InvCult)
    End Function

    <Extension> <MethodImpl(AggrInlin)>
    Function TrimEx(instance As String) As String
        If instance Is Nothing Then
            Return String.Empty
        End If

        Return instance.Trim
    End Function

    <Extension> <MethodImpl(AggrInlin)>
    Function PathStartsWith(instance As String, value As String) As Boolean
        If instance IsNot Nothing AndAlso value?.Length > 0 Then
            Return instance.StartsWith(value, StringComparison.OrdinalIgnoreCase)
        End If
    End Function

    <Extension>
    Sub ThrowIfContainsNewLine(instance As String)
        If instance?.IndexOf(BR, StringComparison.Ordinal) >= 0 Then
            Throw New Exception("String contains a line break char: " & instance)
        End If
    End Sub

    <Extension> <MethodImpl(AggrInlin)>
    Function Multiply(instance As Char, multiplier As Integer) As String
        If multiplier < 1 Then multiplier = 1
        'Dim sb As New StringBuilder(multiplier * instance.Length)  'sb.Append(instance, 0, multiplier)
        Return New String(instance, multiplier) 'sb.ToString()
    End Function

    <Extension>
    Function IsValidFileName(instance As String) As Boolean
        If instance Is Nothing Then Return False

        For i = 0 To instance.Length - 1
            Dim chInt As UShort = Convert.ToUInt16(instance(i))

            If chInt < 32 OrElse InvalidFChI16.ContainsS(chInt) Then
                Return False
            End If
        Next i

        Return True
    End Function

    <Extension>
    Function IsDosCompatible(instance As String) As Boolean
        If instance.NullOrEmptyS Then Return True

        For i = 0 To instance.Length - 1 'TODO: Tests!!! or merge with Stax
            If Convert.ToUInt16(instance(i)) > 127 Then Return False
        Next i
        Return True
        'Dim bytes = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(ConsoleHelp.DosCodePage), Encoding.Unicode.GetBytes(instance))
        'Return instance.Equals(Encoding.Unicode.GetString(Encoding.Convert(Encoding.GetEncoding(ConsoleHelp.DosCodePage), Encoding.Unicode, bytes)))
    End Function

    <Extension>
    Function IsANSICompatible(instance As String) As Boolean
        If instance.NullOrEmptyS Then Return True

        Dim bytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, Encoding.Unicode.GetBytes(instance))
        Return instance = Encoding.Unicode.GetString(Encoding.Convert(Encoding.Default, Encoding.Unicode, bytes))
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function FileName(instance As String) As String
        If instance Is Nothing Then Return String.Empty

        Dim index = instance.LastIndexOf(SeparatorD)
        If index > -1 Then
            Return instance.Substring(index + 1)
        End If

        Return instance
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ChangeExt(instance As String, value As String) As String
        If instance.NullOrEmptyS Then Return String.Empty
        If value.NullOrEmptyS Then Return instance

        value = value.ToLower(InvCult)
        If value(0) <> "."c Then
            value = "." & value
        End If

        Return instance.DirAndBase & value
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Escape(instance As String) As String
        If instance.NullOrEmptyS Then
            Return String.Empty
        End If
        'For iCh = 0 To EscapeChI16.Length - 1 'Worst Perf - string-ForLoop->ChArray.Contains is Faster than string.contains, string.ToCharArray.containsF is Faster;'2550ms-ContainsF ; ForLoop 2400ms ; 2xForLoop NoCharArray 2350ms-Fastest NoSimd,Simd~1200ms
        'Dim iChA = instance.ToCharArray.SelectF(Function(ch) Convert.ToUInt16(ch))

        ''Dim instHS = instance.ToHashSet ' This = HS.Union(AddIfNotPresent) ForEach
        'If EscapeChHS.Overlaps(instance) Then 'This = HS.Contains ForEach
        '    Return """" & instance & """"
        'End If

        Dim iChA(instance.Length - 1) As UShort 'Test if this is faster than ToCharArr>ConvertToUint16 !!!!
        For i = 0 To instance.Length - 1
            iChA(i) = Convert.ToUInt16(instance(i)) 'Test this : CUShort(AscW(instance(i))) !!!!
        Next i

        For iCh = 0 To EscapeChI16.Length - 1
            'If instHS.Contains(EscapeCh(iCh)) Then Return """" & instance & """" ' try with HashSet Or Not Try String.IndexOfAny ???
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

    <Extension()> <MethodImpl(AggrInlin)>
    Function FileExists(instance As String) As Boolean
        Return File.Exists(instance)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function DirExists(instance As String) As Boolean
        Return Directory.Exists(instance)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Ext(instance As String) As String
        If instance Is Nothing Then Return String.Empty

        For x = instance.Length - 1 To 0 Step -1
            If instance(x) = SeparatorD Then Return String.Empty

            If instance(x) = "."c Then
                Return instance.Substring(x + 1).ToLower(InvCult)
            End If
        Next

        Return String.Empty
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ExtFull(instance As String) As String
        If instance Is Nothing Then Return String.Empty

        For x = instance.Length - 1 To 0 Step -1
            If instance(x) = SeparatorD Then Return String.Empty

            If instance(x) = "."c Then
                Return instance.Substring(x).ToLower(InvCult)
            End If
        Next

        Return String.Empty
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Base(instance As String) As String
        If instance Is Nothing Then
            Return String.Empty
        End If

        Dim idxS As Integer = instance.LastIndexOf(SeparatorD)
        Dim idxD = instance.LastIndexOf("."c, instance.Length - 1, instance.Length - idxS) 'Check with -1  - needs noSep case check!
        If idxS >= 0 OrElse idxD >= 0 Then instance = instance.Substring(idxS + 1, idxD - idxS - 1) ' Or >=0 ??? what if . is first

        'Dim idx As Integer = instance.LastIndexOf(SeparatorD)
        'If idx >= 0 Then instance = instance.Substring(idx + 1)
        'idx = instance.LastIndexOf("."c)
        'If idx = 0 Then Return String.Empty
        'If idx >= 0 Then instance = instance.Substring(0, idx) ' Or >=0 and no 0 idx check??? what if . is first
        Return instance
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Dir(instance As String) As String
        If instance Is Nothing Then
            Return String.Empty
        End If

        Dim idx As Integer = instance.LastIndexOf(SeparatorD)

        If idx >= 0 Then
            instance = instance.Substring(0, idx + 1)
        End If

        Return instance
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function LongPathPrefix(instance As String) As String
        If instance Is Nothing Then
            Return String.Empty
        End If

        If instance.Length > Max_PathLen Then
            If Not instance.StartsWith(LongPathPref, StringComparison.Ordinal) Then
                Return LongPathPref & instance
            End If
        End If

        Return instance
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToShortFilePath(instance As String) As String
        If instance Is Nothing Then
            Return String.Empty
        End If

        If instance.StartsWith(LongPathPref, StringComparison.Ordinal) Then
            instance = instance.Substring(4)
        End If

        If instance.Length <= Max_PathLen Then
            Return instance
        End If

        Dim sb As New StringBuilder(Max_PathLen)
        Native.GetShortPathName(instance.Dir, sb, sb.Capacity)
        Dim ret = sb.ToString & instance.FileName

        If ret.Length <= Max_PathLen Then
            Return ret
        End If

        Native.GetShortPathName(instance, sb, sb.Capacity)

        Return sb.ToString
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function DirName(instance As String) As String
        If instance Is Nothing Then
            Return String.Empty
        End If

        If instance.Length > 3 Then
            instance = instance.TrimEnd(SeparatorD)
        End If

        Dim idx As Integer = instance.LastIndexOf(SeparatorD)
        Return If(idx >= 0, instance.Substring(idx + 1), String.Empty)

    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function DirAndBase(path As String) As String 'ToDo , Test This !!!!!!!!!!!!!!!!!!!!!!

        If path Is Nothing Then
            Return String.Empty
        End If

        Dim idxS As Integer = path.LastIndexOf(SeparatorD)
        'If idxS >= 0 Then
        '    Dim retD = path.Substring(0, idxS + 1)
        '    Dim retB0 = path.Substring(idxS + 1)
        'End If

        If idxS >= 0 Then
            idxS = path.LastIndexOf("."c, path.Length - 1, path.Length - idxS)
            'Else
            'idxS = path.LastIndexOf("."c)
        End If

        If idxS >= 0 Then ' Or >=0 ???
            path = path.Substring(0, idxS)
        End If

        Return path

        'Return path.Dir & path.Base
    End Function

    <Extension()>
    Function ContainsAll(instance As String, all As String()) As Boolean
        If instance?.Length > 0 Then
            Return all.AllF(Function(arg) instance.IndexOf(arg, StringComparison.Ordinal) >= 0)
        End If
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ContainsAny(instance As String, ParamArray any As String()) As Boolean
        If instance.NotNullOrEmptyS AndAlso any IsNot Nothing Then
            For i = 0 To any.Length - 1
                If instance.IndexOf(any(i), StringComparison.Ordinal) >= 0 Then Return True
            Next i
            'If instance.IndexOfAny(any) >= 0 Then Return True 'Test This !!!
        End If
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function EqualsExS(instance As String, value As String) As Boolean 'marginally slower for nulls/"", 4xfaster in NoEmpt strings than =/<>
        If instance Is Nothing Then ' if instance="" andalso value="" return true
            If value Is Nothing OrElse value.Length = 0 Then Return True Else Return False
            'instance = String.Empty
        End If
        If value Is Nothing Then
            If instance Is Nothing OrElse instance.Length = 0 Then Return True Else Return False
            'value = String.Empty
        End If
        Return String.Equals(value, instance)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function EqualsExS(instance As String, value As String, ComparisonType As StringComparison) As Boolean
        If instance Is Nothing Then
            If value Is Nothing OrElse value.Length = 0 Then Return True Else Return False
        End If
        If value Is Nothing Then
            If instance Is Nothing OrElse instance.Length = 0 Then Return True Else Return False
        End If
        Return String.Equals(value, instance, ComparisonType)
    End Function


    <Extension()> <MethodImpl(AggrInlin)>
    Function EqualsAny(instance As String, ParamArray values As String()) As Boolean
        If instance.NullOrEmptyS OrElse values Is Nothing Then
            Return False
        End If
        For i = 0 To values.Length - 1
            If String.Equals(values(i), instance) Then Return True
        Next i
        'Return values.Contains(instance) '-Org Stax
    End Function
    <Extension()> <MethodImpl(AggrInlin)>
    Function EqualsAny(instance As String, strComparison As StringComparison, ParamArray values As String()) As Boolean
        If values Is Nothing OrElse instance.NullOrEmptyS Then
            Return False
        End If
        For i = 0 To values.Length - 1
            If String.Equals(values(i), instance, strComparison) Then Return True
        Next i
        'Return values.Contains(instance) '-Org Stax
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Public Function NullOrEmptyS(instance As String) As Boolean
        Return instance Is Nothing OrElse instance.Length = 0
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Public Function NotNullOrEmptyS(instance As String) As Boolean
        Return instance IsNot Nothing AndAlso instance.Length > 0
    End Function

    '<Extension()>
    'Public Function NullOrWhiteSpace(instance As String) As Boolean
    '    Return String.IsNullOrWhiteSpace(instance) ' Test this !!!
    'End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function FixDir(instance As String) As String
        If instance.NullOrEmptyS Then Return String.Empty ' is nothing
        For i = instance.Length - 1 To 0 Step -1
            Dim c As Integer
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
        Return SeparatorD ' If(instance.Length > 0, SeparatorD, "")
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function FixBreak(value As String) As String
        'Dim c = vbCr = ChrW(13)
        'Dim b = vbLf = ChrW(10)
        'Dim fff = ChrW(13) & ChrW(10) = BR
        'Dim enn = Environment.NewLine = BR = vbCrLf
        'Dim t2 = vbTab = ChrW(9) '+ AscW() All are Consts!

        'value = value.Replace(ChrW(13) & ChrW(10), ChrW(10))
        'value = value.Replace(ChrW(13), ChrW(10))
        'Return value.Replace(ChrW(10), ChrW(13) & ChrW(10))
        Return value.Replace(BR, vbLf).Replace(ChrW(13), ChrW(10)).Replace(vbLf, BR)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToTitleCase(value As String) As String
        'TextInfo.ToTitleCase won't work on all upper strings
        'Return CurrCult.TextInfo.ToTitleCase(value.ToLowerInvariant) 'Org
        Return InvCult.TextInfo.ToTitleCase(InvCult.TextInfo.ToLower(value))
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function IsInt(value As String) As Boolean
        Return Integer.TryParse(value, NumberStyles.Integer, InvCult, Nothing)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToInt(value As String, Optional defaultValue As Integer = 0I) As Integer
        Dim ret As Integer
        Return If(Integer.TryParse(value, NumberStyles.Integer, InvCult, ret), ret, defaultValue)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToIntM(value As String) As Integer '-&H7FFFFFFEI) 'opt -2147483646I 
        Dim ret As Integer
        Return If(Integer.TryParse(value, NumberStyles.Integer, InvCult, ret), ret, -2147483646I) 'int.min -2
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToSingle(value As String, Optional defaultValue As Single = 0F) As Single
        If value?.Length > 0 Then
            If value.Length > 1 Then value = value.Replace(","c, "."c)

            Dim ret As Single
            If Single.TryParse(value, NumberStyles.Float, InvCult, ret) Then
                Return ret
            End If
        End If
        Return defaultValue
    End Function
    '<Extension()>
    'Function IsDouble(value As String) As Boolean
    '    If value?.Length > 0 Then
    '        value = value.Replace(",", ".")
    '        Return Double.TryParse(value, NumberStyles.Float, InvCult, Nothing)
    '    End If
    'End Function
    <Extension()> <MethodImpl(AggrInlin)>
    Function ToDouble(value As String, Optional defaultValue As Double = 0R) As Double
        If value?.Length > 0 Then
            If value.Length > 1 Then value = value.Replace(","c, "."c)

            Dim ret As Double
            If Double.TryParse(value, NumberStyles.Float, InvCult, ret) Then
                Return ret
            End If
        End If
        Return defaultValue
    End Function

    <Extension()>
    Function FormatColumn(value As String, delimiter As Char) As String
        If value.NullOrEmptyS Then Return String.Empty
        Dim lines = value.Split({BR}, StringSplitOptions.None)
        Dim ret(lines.Length - 1) As String
        Dim highest As Integer

        For i = 0 To lines.Length - 1
            Dim line = lines(i)
            Dim ldi = line.IndexOf(delimiter)

            If ldi > 0 Then
                Dim st As String = line.Substring(0, ldi).Trim
                If st.Length > highest Then highest = st.Length
                ret(i) = st
            Else
                If line.Length > highest Then highest = line.Length
                ret(i) = line
            End If
        Next i

        Dim dlmP = " " & delimiter & " "
        For i = 0 To lines.Length - 1
            Dim line = lines(i)
            Dim ldi As Integer = line.IndexOf(delimiter)

            If ldi >= 0 Then
                ret(i) = ret(i).PadRight(highest) & dlmP & line.Substring(ldi + 1).Trim
            End If
        Next i
        'Dim testttt = value & BR & "------------------------------------------------------" & BR & String.Join(BR, ret)
        Return String.Join(BR, ret)
    End Function

    <Extension()>
    Function ReadAllText(instance As String) As String
        If Not File.Exists(instance) Then
            Return String.Empty
        End If

        Return File.ReadAllText(instance)
    End Function

    <Extension()>
    Function ReadAllTextDefault(instance As String) As String
        If Not File.Exists(instance) Then
            Return String.Empty
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
        WriteFile(instance, path, Encoding.UTF8) 'New UTF8Encoding(True))
    End Sub

    <Extension()>
    Sub WriteFile(instance As String, path As String, encoding As Encoding)
        Try
            File.WriteAllText(path, instance, encoding)
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Extension()> <MethodImpl(AggrInlin)>
    Function Left(value As String, iLength As Integer) As String
        If value Is Nothing OrElse iLength < 0 Then Return String.Empty
        If iLength > value.Length Then Return value
        Return value.Substring(0, iLength)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Left(value As String, start As Char) As String 'ToDO: Change rest to Char overload !!!!!
        If value Is Nothing Then Return String.Empty
        Dim idx As Integer = value.IndexOf(start)
        Return If(idx > 0, value.Substring(0, idx), String.Empty)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Left(value As String, start As String) As String
        If value Is Nothing Then Return String.Empty 'OrElse start Is Nothing Then
        Dim idx As Integer = value.IndexOf(start, StringComparison.Ordinal)
        Return If(idx > 0, value.Substring(0, idx), String.Empty)
    End Function
    '<Extension()> <MethodImpl(AggrInlin)> Function LeftLast(value As String, start As Char) As String
    '    Dim idx As Integer = value.LastIndexOf(start)
    '    Return If(idx > 0, value.Substring(0, idx), String.Empty)
    'End Function
    <Extension()> <MethodImpl(AggrInlin)>
    Function Right(value As String, start As Char) As String
        If value Is Nothing Then Return String.Empty ' OrElse start.Length <= 0 OrElse start.NullOrEmptyS MUST be Empty String 'start' proof!
        Dim idx As Integer = value.IndexOf(start)
        Return If(idx >= 0, value.Substring(idx + 1), String.Empty)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Right(value As String, start As String) As String
        If value Is Nothing Then Return String.Empty ' OrElse start.Length <= 0 OrElse start.NullOrEmptyS MUST be Empty String 'start' proof!
        Dim idx As Integer = value.IndexOf(start, StringComparison.Ordinal)
        Return If(idx >= 0, value.Substring(idx + start.Length), String.Empty)
    End Function
    '<Extension()> <MethodImpl(AggrInlin)> Function RightLast(value As String, start As Char) As String 'ToDO : Change to Char start index
    '    If value Is Nothing Then Return String.Empty  ' OrElse start.Length <= 0 OrElse start.NullOrEmptyS MUST be Empty String 'start' proof!
    '    Dim idx As Integer = value.LastIndexOf(start)
    '    Return If(idx >= 0, value.Substring(idx + 1), String.Empty) '+start.Length = +1
    'End Function
    <Extension()> <MethodImpl(AggrInlin)>
    Function IsEqualIgnoreCase(value1 As String, value2 As String) As Boolean
        Return String.Equals(value1, value2, StringComparison.OrdinalIgnoreCase)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Shorten(value As String, maxLength As Integer) As String
        If value Is Nothing OrElse value.Length <= maxLength Then Return value

        Return value.Substring(0, maxLength)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ShortBegEnd(instance As String, Optional StartLen As Integer = 32, Optional EndLen As Integer = 18) As String
        Return If(instance.Length > StartLen + EndLen + 1,
            instance.Substring(0, StartLen) & "_" & instance.Substring(instance.Length - EndLen, EndLen), 'OR:instance.Remove(StartLen, instance.Length - StartLen - EndLen) + string.insert    
            instance)
    End Function

    <Extension()>
    Function IsValidFileSystemName(instance As String) As Boolean
        If instance Is Nothing Then Return False

        For i = 0 To instance.Length
            Dim chInt As UShort = Convert.ToUInt16(instance(i))
            If chInt < 32 OrElse InvalidFSChI16.ContainsS(chInt) Then
                Return False
            End If
        Next i

        Return True
    End Function

    <Extension()>
    Function IsSameBase(instance As String, b As String) As Boolean
        Return String.Equals(instance.Base, b.Base, StringComparison.OrdinalIgnoreCase)
    End Function

    <Extension()>
    Function EscapeIllegalFileSysChars(value As String) As String
        If value Is Nothing Then Return String.Empty

        For i = 0 To value.Length - 1
            Dim chInt As UShort = Convert.ToUInt16(value(i))
            If chInt < 32 OrElse InvalidFSChI16.ContainsS(chInt) Then
                value = value.Replace(value(i), "__" & Uri.EscapeDataString(value(i)).TrimStart("%"c) & "__")
            End If
        Next

        Return value
    End Function

    <Extension()>
    Function UnescapeIllegalFileSysChars(value As String) As String
        If value Is Nothing Then
            Return String.Empty
        End If

        For Each match As Match In Regex.Matches(value, "__(\w\w)__")
            value = value.Replace(match.Value, Uri.UnescapeDataString("%" + match.Groups(1).Value))
        Next

        Return value
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function SplitNoEmptyAndWhiteSpace(value As String, ParamArray delimiters As Char()) As String()
        If value Is Nothing Then Return {}
        Dim sAr = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
        'Dim ret As New List(Of String)(sAr.Length)
        Dim inc As Integer

        For i = 0 To sAr.Length - 1
            Dim ts As String = sAr(i).Trim
            If ts.Length > 0 Then
                sAr(inc) = ts
                inc += 1
            End If
            'If ts.Length > 0 Then ret.Add(ss)
        Next i

        If inc = sAr.Length Then
            Return sAr
        Else
            Dim ret(inc - 1) As String
            '??? If inc > 3 Then Array.Copy(sAr, 0, ret, 0, inc) 'Seems only used with few el; ArrCopyString(): faster 4 OrMore!!!
            For i = 0 To inc - 1
                ret(i) = sAr(i)
            Next
            Return ret
        End If
    End Function
    'Function SplitLinesNoEmpty(value As String) As String()
    '    Return value.Split({BR}, StringSplitOptions.RemoveEmptyEntries) 'Environment.NewLine = BR = vbCrLf
    'End Function
    <Extension()>
    Function RemoveChars(value As String, charsHS As HashSet(Of Char)) As String 'ToDO Inline This !!!!!
        If value?.Length > 0 Then
            Dim vcA = value.ToCharArray 'HashSet is Fastest 2-3X 'Arr Add verion
            Dim inc As Integer
            For i = 0 To vcA.Length - 1
                Dim ch As Char = vcA(i)
                If Not charsHS.Contains(ch) Then
                    vcA(inc) = ch
                    inc += 1
                End If
            Next i
            If inc = vcA.Length Then
                Return value
            Else
                Dim retS(inc - 1) As Char 'ForLoopInt16/Char: Sign. 2x Gains 4 OrLess - max 3x-1el., 10 OrLess begins faster!!!
                Array.Copy(vcA, 0, retS, 0, inc) 'ForLoopInt32: Sign. 2x Gains 7 OrLess - max 3x-2el., 15 OrLess begins faster!!!
                Return New String(vcA)
            End If
            'Dim retS As New CircularList(Of Char)(value) 'List Remove Version
            'For i = retS.Count - 1 To 0 Step -1
            '    If charsHS.Contains(retS(i)) Then
            '        retS.RemoveAt(i)
            '    End If
            'Next i
            'Return If(retS.Count = value.Length, value, New String(retS.ToArray))
        End If
        Return String.Empty
        'Dim ret = value 'Replace version
        'For ic = 0 To chars.Length - 1
        '    ret = ret.Replace(chars(ic), "")
        'Next
        'Return ret
    End Function

    <Extension()>
    Function DeleteRight(value As String, count As Integer) As String
        Return Left(value, value.Length - count)
    End Function

    <Extension()>
    Function ReplaceRecursive(value As String, find As String, replace As String) As String
        If value Is Nothing Then
            Return String.Empty
        End If

        'While  value.LastIndexOf(find, StringComparison.Ordinal) >= 0
        Do While InvCultCompInf.LastIndexOf(value, find, CompareOptions.Ordinal) >= 0
            value = value.Replace(find, replace)
        Loop

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
        If value?.Length > 0 Then
            Clipboard.SetText(value)
        Else
            Clipboard.Clear()
        End If
    End Sub

End Module

Module MiscExtensions

    Public Const AggrInlin As MethodImplOptions = MethodImplOptions.AggressiveInlining
    Public ReadOnly ScreenResPrim As Size = Screen.PrimaryScreen.Bounds.Size 'Seems equals to WorkingArea
    Public ReadOnly ScreenResWAPrim As Size = Screen.PrimaryScreen.WorkingArea.Size 'Screen.FromControl(Me).WorkingArea
    Public ReadOnly CPUsC As Integer = Environment.ProcessorCount
    Public ReadOnly SWFreq As Integer = CInt(Stopwatch.Frequency / 1000)

    '<MethodImpl(MethodImplOptions.NoInlining)> 'Needed???
    Public Sub WarmUpCpu()
        Dim wr1 As Single = 2.0F
        Dim wr2 As Single = wr1
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(CInt(wr2), GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()
        Application.DoEvents()
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce
        GC.Collect(CInt(wr1), GCCollectionMode.Forced, True, True)
        GC.WaitForPendingFinalizers()
        Application.DoEvents()
        For w = 1 To 10_000_000 '~200ms
            If w / 5.34567F > Single.Epsilon Then wr2 = wr1 * 1.31539571F * (w / 45.5234642F) / (wr1 / 2.2422F) / 1.146747F
            If w / 35.6346F > Single.Epsilon Then wr1 = w / 33.13513F * (wr2 / 5.12573F) / (w / 37.3234558F) * 1.133479F ' Or last mul to div ~50ms more
        Next w
    End Sub

    Function TestBenchModule() As Integer
        Dim sWatch = Stopwatch.StartNew
        Dim sbT As New StringBuilder(512)
        Dim cnt As Integer = -2
        WarmUpCpu()
        Thread.Sleep(120)
Again:  Try
            'GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency   ' Or Sustained SustainedLowLatency is better sometimes,LowLat can be horrible!!!
            'If GCSettings.LatencyMode <> GCLatencyMode.NoGCRegion Then
            '    WarmUpCpu()
            '    Const noGCRegTot As Long = &HFFFF_0000L '&HFFFF_0000L=~4GB
            '    Const noGCRegLOH As Long = noGCRegTot - &HF3C_0000L '<MaxEphemerSeg= &HF3C_E464L= Highest poss SOH ~243MB
            '    If GC.TryStartNoGCRegion(noGCRegTot, noGCRegLOH) Then Console.Beep(5000, 100)
            'End If
            Test_Body(sbT, sWatch)
        Catch ex As Exception
            sbT = New StringBuilder(ex.ToString)
        End Try
        'If GCSettings.LatencyMode = GCLatencyMode.NoGCRegion Then
        '    sbT.Append("NoGCRegion Active:").AppendLine(GCSettings.LatencyMode.ToString)
        '    GC.EndNoGCRegion()
        '    Console.Beep(300, 100) 'ElseIf
        If GCSettings.LatencyMode <> GCLatencyMode.Interactive Then
            sbT.Append("GCLatencyMode:").AppendLine(GCSettings.LatencyMode.ToString)
            'GCSettings.LatencyMode = GCLatencyMode.Interactive
        End If
        'If sbT.Length > 0 AndAlso sbT(sbT.Length - 1) <> ChrW(10) Then sbT.AppendLine()
        'sbT.AppendLine()
        WarmUpCpu()
        cnt += 1
        If cnt < 0 Then GoTo Again
        Tasks.Task.Run(Sub()
                           Thread.Sleep(90)
                           WarmUpCpu()
                       End Sub)
        Dim mm = Msg(sbT.ToString, $"{cnt}Cn,{sbT.Length}sbL.  <YES> to Repeat, <NO> to Delete Log!, <CANCEL> to Exit", MsgIcon.Question, TaskDialogButtons.YesNoCancel, DialogResult.Yes, 384)
        sbT.AppendLine()
        If mm = DialogResult.Yes Then
            'If retSB.Length > 512 Then retSB = New StringBuilder(retSB.Length) Else retSB.Clear()
            GoTo Again
        ElseIf mm = DialogResult.No Then
            FileHelp.Delete("C:\abc\1StaxTestTimes_log.txt", FileIO.RecycleOption.SendToRecycleBin)
            File.AppendAllText("C:\abc\1StaxTestTimes_log.txt", Date.Now.ToString("HH:mm:ss,fff") & " Count:" & cnt & BR & sbT.ToString)
            Return -2
        Else 'Cancel - Exit App
            File.AppendAllText("C:\abc\1StaxTestTimes_log.txt", Date.Now.ToString("HH:mm:ss,fff") & " Count:" & cnt & BR & sbT.ToString)
            Return -1
        End If
    End Function
    'Public rr111 As Integer
    'Public rr222 As Integer
    'Public itr1B As Integer = 1_000_000_000
    Sub Test_Body(sbT As StringBuilder, sW1 As Stopwatch) 'As StringBuilder
        Dim itr1B = 10000 '_000_000
        'Dim ccCi = CultureInfo.CurrentCulture
        'Dim ccTi = CultureInfo.CurrentCulture.TextInfo 'TexInfo Field- samewhat PerfGain!
        'Dim invCi = CultureInfo.InvariantCulture
        'Dim invTi = CultureInfo.InvariantCulture.TextInfo 'CultureInfo.InvariantCulture.TextInfo
        'Dim rndF As New Random
        'Dim a1 = {"s"c, "f"c, "G"c, "r"c, "Q"c, "i"c}
        'Dim a1 = Enumerable.Range(1, 32).ToArray
        'Dim a1(itr1B - 1) As Integer
        'Dim l1 As List(Of UShort)
        'Dim l2 As List(Of UShort)
        'Dim n1 As Integer = 32
        'Dim mutHS As HashSet(Of String) = FileTypes.Video.ToHashSet(StringComparer.OrdinalIgnoreCase)
        Dim s1 = "sDGrQi zSeLYEbh3"
        Dim s2 = "dfco"
        Dim s3 As String = "dfco fR "
        'Dim s4 As String = ""
        'Dim s5 As String = "vb15 dfgh"
        'Dim s6 As String = "dfco sdfgdf dfg "
        'Dim s7 As String = "wdżźąÓŹŻćą 3dgfEnFdgd"
        'Dim s8 As String = "dfco vfnifR dfb  drgn drt dth dt "
        'Dim s9 As String = "dfco vfnifR dfb dfdxfbvdfxbgdfg wgdf6 ąśążśćąĘŚĄŃ"
        'Dim s10 As String = "dfco"
        Dim r1 As String = ""
        Dim ssb As New StringBuilder(8000) ' Def SB maxChunk =8000

        ' Dim r1 As Integer
        WarmUpCpu()
        sW1.Restart()

        For n = 1 To itr1B
            'r1 = r1.Replace(s2, s3)
            r1 &= s1 & s2 & s3 & 's4 '& s5 & s6 & s7 & s8 & s9 & s10
            ' If s1(0) = "s"c Then '25ms(2CT per idx)
            'If s1.StartsWith("sD", StringComparison.Ordinal) Then
            'If s1(0) = "s"c AndAlso s1(1) = "D"c AndAlso s1(2) = "G"c AndAlso s1(3) = "r"c AndAlso s1(4) = "Q"c AndAlso s1(5) = "i"c Then '25ms(2CT per idx)
            'l1 = New List(Of UShort)(16)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'l1.Add(s1)
            'rr111 = s1.IndexOf("JHtyą", StringComparison.Ordinal)
        Next n

        sW1.Stop()
        sbT.Append("-->T1> ").Append(CStr(sW1.ElapsedTicks / SWFreq)).AppendLine("ms").AppendLine($"[{r1.Length }] / [{s1.Length }]")
        WarmUpCpu()
        sW1.Restart()

        For n = 1 To itr1B
            'ssb.Replace(s2, s3)
            ssb.Append(s1).Append(s2).Append(s3) '.Append(s4) ' .Append(s5).Append(s6).Append(s7).Append(s8).Append(s9).Append(s10)
            'If s1.StartsWith("sD", StringComparison.InvariantCulture) Then
            'If itr1B > 2 Then
            'l2 = New List(Of UShort)(16)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2.Add(s1)
            'l2 = l1
            'Thread.SpinWait(1000)
            'Thread.Sleep(1)
            'End If
            'End If
            'rr222 = s2.IndexOf("JHtyą", StringComparison.OrdinalIgnoreCase)
            's1 = CChar(s2.ToUpper(ccCi))
        Next n

        sW1.Stop()
        sbT.Append("-->T2> ").Append(CStr(sW1.ElapsedTicks / SWFreq)).AppendLine("ms").AppendLine($"[{ssb.Length }] / [{ssb.Capacity}]")
        'Return retSB
    End Sub

    <Extension()> <MethodImpl(AggrInlin)>
    Public Function GetDeepClone(Of T)(obj As T) As T
        Return DeepClonerExtensions.DeepClone(Of T)(obj)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToInvStr(value As Double, format As String) As String
        Return value.ToString(format, InvCult)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToInvStr(value As Double) As String
        Return value.ToString(InvCult)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToInvStr(value As Single) As String
        Return value.ToString(InvCult)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToInvStr(instance As Integer) As String
        Return instance.ToString(InvCult)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToInvStr(instance As Long) As String
        Return instance.ToString(InvCult)
    End Function
    '<Extension()>
    'Function ToInvStr(instance As IConvertible) As String
    '    If instance Is Nothing Then Return String.Empty
    '    Return instance.ToString(InvCult)
    'End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToIntInvStr(value As Double) As String '4-8 x faster, for int floats
        Return CInt(value).ToString(InvCult)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToIntInvStr(value As Single) As String '4-8 x faster, for int floats
        Return CInt(value).ToString(InvCult)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ToInvStr(instance As Date, format As String) As String
        Return instance.ToString(format, InvCult)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ContainsString(instance As String(), value As String) As Boolean
        If instance IsNot Nothing AndAlso value?.Length > 0 Then
            For i = 0 To instance.Length - 1
                If String.Equals(instance(i), value) Then Return True
            Next i
        End If
    End Function
    '<Extension()> <MethodImpl(AggressiveInlin)> Function ContainsString(instance As String(), value As String, strComparison As StringComparison) As Boolean
    '    If instance IsNot Nothing AndAlso value?.Length > 0 Then
    '        For i = 0 To instance.Length - 1
    '            If String.Equals(instance(i), value, strComparison) Then Return True
    '        Next i
    '    End If
    'End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function ContainsAny(instance As String(), values As String()) As Boolean
        For i = 0 To instance.Length - 1 'Todo test This !!!!!!!!!!!!! & Remove ParamArray
            'If ContainsString(values, instance(i)) Then Return True
            Dim inst As String = instance(i)
            For v = 0 To values.Length - 1
                If String.Equals(values(v), inst) Then Return True
            Next v
        Next i
        'Return instance.AnyF(Function(arg) values.ContainsString(arg))
    End Function
    '<Extension()> Function ContainsAny(Of T)(instance As IEnumerable(Of T), values As IEnumerable(Of T)) As Boolean
    '    Return instance.Any(Function(arg) values.Contains(arg))
    'End Function
    <Extension()> <MethodImpl(AggrInlin)>
    Function Sort(instance As String()) As String()
        Array.Sort(Of String)(instance, StringComparer.OrdinalIgnoreCase)
        Return instance
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function Join(instance As String(), delimiter As String) As String ', Optional removeEmpty As Boolean = False) 'Try Inline This
        If instance Is Nothing Then Return Nothing
        'If instance.Length <= 0 Then Return String.Empty
        'If removeEmpty Then instance = instance.WhereF(Function(arg) arg.NotNullOrEmptyS)
        Return String.Join(delimiter, instance, 0, instance.Length)
    End Function
    '<Extension()>
    'Function Join(instance As List(Of String), delimiter As String) As String ', Optional removeEmpty As Boolean = False)
    '    If instance Is Nothing Then Return Nothing
    '    Dim iCn As Integer = instance.Count
    '    If iCn <= 0 Then Return String.Empty
    '    'If removeEmpty Then instance = instance.WhereF(Function(arg) arg.NotNullOrEmptyS)
    '    Return String.Join(delimiter, instance.ToArray, 0, iCn) 'or Not ToArray ???
    'End Function
    '<Extension()>
    'Function Join(instance As IEnumerable(Of String), delimiter As String) As String ' , Optional removeEmpty As Boolean = False)
    '    If instance Is Nothing Then Return Nothing
    '    'If removeEmpty Then instance = instance.Where(Function(arg) arg.NotNullOrEmptyS)
    '    Return String.Join(delimiter, instance)
    'End Function
    <Extension> <MethodImpl(AggrInlin)>
    Public Function ConcatA(Of T)(first As T(), second As T()) As T()
        'If second Is Nothing Then
        '    Return first
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

    <Extension()> <MethodImpl(AggrInlin)>
    Function NeutralCulture(ci As CultureInfo) As CultureInfo
        Return If(ci.IsNeutralCulture, ci, ci.Parent)
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function NothingOrEmpty(strings As String()) As Boolean
        If strings?.Length > 0 Then
            For i = 0 To strings.Length - 1
                If strings(i)?.Length > 0 Then
                    Return False
                End If
            Next i
        End If

        Return True
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
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

    <Extension()> <MethodImpl(AggrInlin)>
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
        End If
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
    Function NothingOrEmpty(strings As IEnumerable(Of String)) As Boolean
        If strings?.Any(Function(itm) itm IsNot Nothing AndAlso itm.Length > 0) Then
            Return False
        End If

        Return True
    End Function

    <Extension()> <MethodImpl(AggrInlin)>
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
            If key IsNot Nothing Then
                Dim value = key.GetValue(name)

                If value IsNot Nothing Then
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
            If subKey IsNot Nothing Then
                Return subKey.GetSubKeyNames
            End If
        End Using

        Return {}
    End Function

    <Extension()>
    Function GetValueNames(root As RegistryKey, path As String) As String()
        Using subKey = root.OpenSubKey(path)
            If subKey IsNot Nothing Then
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
            If key IsNot Nothing Then
                key.DeleteValue(name, False)
            End If
        End Using
    End Sub
End Module

Module ControlExtension
    <Extension()> <MethodImpl(AggrInlin)>
    Sub ScaleClientSize(instance As Control, width As Single, height As Single, Optional fontHeightC As Integer = -1)
        If fontHeightC <= 0 Then fontHeightC = If(s.UIScaleFactor = 1, 16, instance.Font.Height)
        instance.ClientSize = New Size(CInt(fontHeightC * width), CInt(fontHeightC * height))
    End Sub

    <Extension()> <MethodImpl(AggrInlin)>
    Sub SetFontStyle(instance As Control, style As FontStyle)
        'instance.Font = New Font(instance.Font.FontFamily, instance.Font.Size, style) 
        instance.Font = New Font(instance.Font, style) ' Test this !!!
    End Sub

    <Extension()> <MethodImpl(AggrInlin)>
    Sub SetFontSize(instance As Control, fontSize As Single)
        instance.Font = New Font(instance.Font.FontFamily, fontSize)
    End Sub

    <Extension()> <MethodImpl(AggrInlin)>
    Sub AddClickAction(instance As Control, action As Action)
        AddHandler instance.Click, Sub() action()
    End Sub

    <Extension()> <MethodImpl(AggrInlin)>
    Function ClientMousePos(instance As Control) As Point
        Return instance.PointToClient(Control.MousePosition)
    End Function

    <Extension()>
    Function GetMaxTextSpace(instance As Control, ParamArray values As String()) As String
        Dim ret As String
        Dim iFn As Font = instance.Font
        Dim iW As Integer = instance.Width

        For x = 4 To 2 Step -1
            ret = String.Join("".PadRight(x), values)
            If TextRenderer.MeasureText(ret, iFn).Width < iW Then
                Return ret
            End If
        Next

        Return ret
    End Function
End Module

Module UIExtensions
    <Extension()> <MethodImpl(AggrInlin)>
    Sub ClearAndDispose(instance As ToolStripItemCollection)
        Dim iCn As Integer = instance.Count - 1
        If iCn < 0 Then Exit Sub

        Dim tsiA(iCn) As ToolStripItem '2x Faster than InstanceLoop indexOf In IntArrayRemove, Clear first, seems dispose is not doing much now, needs test for leaks ???
        instance.CopyTo(tsiA, 0)
        instance.Clear()
        For i = iCn To 0 Step -1
            tsiA(i).Dispose()
        Next i
        'For i = iCn To 0 Step -1
        '    'If TypeOf instance(i) Is IDisposable
        '    instance.Item(i).Dispose()
        'Next i
        'instance.Clear()
    End Sub

    <Extension()>
    Function ResizeIconSize(img As Image, ByRef newSize As Size) As Image 'Remove ByRef??
        If img IsNot Nothing AndAlso img.Size <> newSize Then
            Dim ret As New Bitmap(newSize.Width, newSize.Height)

            Using g = Graphics.FromImage(ret)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.DrawImage(img, 0, 0, newSize.Width, newSize.Height)
            End Using
            Return ret
        End If

        Return img
    End Function
    '<Extension()> Function ResizeImage(image As Image, ByVal height As Integer) As Image
    '    Dim iSz = image.PhysicalDimension 'Bitmap only,size in pixels, else 0.01mm
    '    Dim percH = height / iSz.Height 'was:image.Height
    '    If percH = 1 OrElse Single.IsPositiveInfinity(percH) Then Return image
    '    Dim ret = New Bitmap(CInt(iSz.Width * percH), height)
    '    Using g = Graphics.FromImage(ret)
    '        g.InterpolationMode = InterpolationMode.HighQualityBicubic
    '        g.DrawImage(image, 0, 0, CInt(iSz.Width * percH), height) '(image, 0, 0, ret.Width, ret.Height)
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
    <Extension()> <MethodImpl(AggrInlin)>
    Sub SendMessageCue(tb As TextBox, value As String, hideWhenFocused As Boolean)
        Native.SendMessage(tb.Handle, Native.EM_SETCUEBANNER, If(hideWhenFocused, 0, 1), value)
    End Sub

    <Extension()>
    Sub SendMessageCue(c As ComboBox, value As String)
        Native.SendMessage(c.Handle, Native.CB_SETCUEBANNER, 1, value)
    End Sub

    Function GetPropertyValue(obj As String, propertyName As String) As Object
        obj.GetType.GetProperty(propertyName).GetValue(obj)
    End Function

End Module
