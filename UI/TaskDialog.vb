﻿
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions

Public Delegate Function PFTASKDIALOGCALLBACK(
    hwnd As IntPtr, msg As UInteger, wParam As IntPtr, lParam As IntPtr, lpRefData As IntPtr) As Integer

Public Class TaskDialog(Of T)
    Inherits TaskDialog
    Implements IDisposable

    Private IdValueDic As New Dictionary(Of Integer, T)
    Private IdTextDic As New Dictionary(Of Integer, String)
    Private CommandLinkShieldList As New List(Of Integer)
    Private ButtonArray As IntPtr, RadioButtonArray As IntPtr
    Private Buttons As New List(Of TASKDIALOG_BUTTON)
    Private RadioButtons As New List(Of TASKDIALOG_BUTTON)

    Property Config As TASKDIALOGCONFIG

    Const TDN_CREATED As Integer = 0
    Const TDN_BUTTON_CLICKED As Integer = 2
    Const TDN_HYPERLINK_CLICKED As Integer = 3
    Const TDN_TIMER As Integer = 4
    Const TDN_RADIO_BUTTON_CLICKED As Integer = 6
    Const TDM_CLICK_BUTTON As Integer = &H400 + 102
    Const TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE As Integer = &H400 + 115

    Sub New(Optional cWidth As UInteger = 0)
        Config = New TASKDIALOGCONFIG()
        Config.cbSize = CUInt(Marshal.SizeOf(Config))
        Config.cButtons = 0
        Config.cRadioButtons = 0
        Config.cxWidth = cWidth  '320-224 'Width of dialog, 0-Auto(ideal)
        Config.dwCommonButtons = TaskDialogButtons.None
        Config.dwFlags = Flags.TDF_ALLOW_DIALOG_CANCELLATION
        Config.FooterIcon = New TASKDIALOGCONFIG_ICON_UNION(0)
        Config.hInstance = IntPtr.Zero
        Config.hwndParent = GetHandle()
        Config.MainIcon = New TASKDIALOGCONFIG_ICON_UNION(0)
        Config.nDefaultButton = 0
        Config.nDefaultRadioButton = 0
        Config.pButtons = IntPtr.Zero
        Config.pfCallback = New PFTASKDIALOGCALLBACK(AddressOf DialogProc)
        Config.pRadioButtons = IntPtr.Zero
        Config.pszCollapsedControlText = Nothing
        Config.pszContent = ""
        Config.pszExpandedControlText = Nothing
        Config.pszExpandedInformation = Nothing
        Config.pszFooter = Nothing
        Config.pszMainInstruction = ""
        Config.pszVerificationText = Nothing
        Config.pszWindowTitle = Application.ProductName
    End Sub

    WriteOnly Property AllowCancel() As Boolean
        Set(Value As Boolean)
            If Value Then
                Config.dwFlags = Config.dwFlags Or Flags.TDF_ALLOW_DIALOG_CANCELLATION
            ElseIf (Config.dwFlags And Flags.TDF_ALLOW_DIALOG_CANCELLATION) = Flags.TDF_ALLOW_DIALOG_CANCELLATION Then
                Config.dwFlags = Config.dwFlags Xor Flags.TDF_ALLOW_DIALOG_CANCELLATION
            End If
        End Set
    End Property

    Property MainInstruction() As String
        Get
            Return Config.pszMainInstruction
        End Get
        Set(Value As String)
            Config.pszMainInstruction = Value
        End Set
    End Property

    Property Content() As String
        Get
            Return Config.pszContent
        End Get
        Set(Value As String)
            Config.pszContent = ExpandWikiMarkup(Value)
        End Set
    End Property

    Property ExpandedInformation() As String
        Get
            Return Config.pszExpandedInformation
        End Get
        Set(Value As String)
            Config.pszExpandedInformation = ExpandWikiMarkup(Value)
        End Set
    End Property

    Property VerificationText() As String
        Get
            Return Config.pszVerificationText
        End Get
        Set(Value As String)
            Config.pszVerificationText = Value
        End Set
    End Property

    Property DefaultButton() As DialogResult
        Get
            Return CType(Config.nDefaultButton, DialogResult)
        End Get
        Set(Value As DialogResult)
            Config.nDefaultButton = Value
        End Set
    End Property

    Property Footer() As String
        Get
            Return Config.pszFooter
        End Get
        Set(Value As String)
            Config.pszFooter = ExpandWikiMarkup(Value)
        End Set
    End Property

    WriteOnly Property MainIcon() As TaskDialogIcon
        Set(Value As TaskDialogIcon)
            Config.MainIcon = New TASKDIALOGCONFIG_ICON_UNION(Value)
        End Set
    End Property

    Private SelectedIDValue As Integer = -1

    Property SelectedID As Integer
        Get
            Return SelectedIDValue
        End Get
        Set(value As Integer)
            For Each i In IdValueDic
                If i.Key = value Then
                    SelectedIDValue = value
                End If
            Next
        End Set
    End Property

    Private SelectedValueValue As T

    Property SelectedValue() As T
        Get
            Dim ret As T
            If IdValueDic.TryGetValue(SelectedID, ret) Then
                Return ret
            End If

            Return SelectedValueValue
        End Get
        Set(value As T)
            SelectedValueValue = value
        End Set
    End Property

    Private SelectedTextValue As String

    Property SelectedText() As String
        Get
            Dim ret As String
            If IdTextDic.TryGetValue(SelectedID, ret) Then
                Return ret
            End If

            Return SelectedTextValue
        End Get
        Set(value As String)
            SelectedTextValue = value
        End Set
    End Property

    Property CheckBoxChecked() As Boolean
        Get
            Return (Config.dwFlags And Flags.TDF_VERIFICATION_FLAG_CHECKED) = Flags.TDF_VERIFICATION_FLAG_CHECKED
        End Get
        Set(value As Boolean)
            If value Then
                Config.dwFlags = Config.dwFlags Or Flags.TDF_VERIFICATION_FLAG_CHECKED
            ElseIf CheckBoxChecked Then
                Config.dwFlags = Config.dwFlags Xor Flags.TDF_VERIFICATION_FLAG_CHECKED
            End If
        End Set
    End Property

    Property CommonButtons() As TaskDialogButtons
        Get
            Return Config.dwCommonButtons
        End Get
        Set(Value As TaskDialogButtons)
            Config.dwCommonButtons = Value
        End Set
    End Property

    Private TimeoutValue As Integer

    Property Timeout As Integer
        Get
            Return CInt(TimeoutValue / 1000)
        End Get
        Set(Value As Integer)
            TimeoutValue = Value * 1000

            If Value > 0 Then
                Config.dwFlags = Config.dwFlags Or Flags.TDF_CALLBACK_TIMER
            End If
        End Set
    End Property

    Function GetHandle() As IntPtr
        Dim sb As New StringBuilder(512)
        Dim handle = Native.GetForegroundWindow
        Native.GetWindowModuleFileName(handle, sb, CUInt(sb.Capacity))

        If String.Equals(sb.ToString.Replace(".vshost", "").Base, Application.ExecutablePath.Base) Then
            Return handle
        End If
    End Function

    Sub AddButton(text As String, value As T)
        Dim id = 1000 + IdValueDic.Count + 1
        IdValueDic(id) = value
        Buttons.Add(New TASKDIALOG_BUTTON(id, text))
    End Sub

    Function ExpandWikiMarkup(value As String) As String
        If value.IndexOf("["c) >= 0 Then
            Dim regex As New Regex("\[(\w+?:.*?) (.+?)\]")
            Dim match = regex.Match(value)

            If match.Success Then
                Config.dwFlags = Config.dwFlags Or Flags.TDF_ENABLE_HYPERLINKS
                value = regex.Replace(value, "<a href=""$1"">$2</a>")
            End If
        End If

        Return value
    End Function

    Sub AddCommand(text As String, Optional value As T = Nothing)
        Dim id = 1000 + IdValueDic.Count + 1
        Dim temp As Object = text
        IdValueDic(id) = If(value Is Nothing, CType(temp, T), value)
        IdTextDic(id) = text
        Buttons.Add(New TASKDIALOG_BUTTON(id, text))
        Config.dwFlags = Config.dwFlags Or Flags.TDF_USE_COMMAND_LINKS
    End Sub

    Sub AddCommand(text As String, description As String, value As T, Optional setShield As Boolean = False)

        Dim id = 1000 + IdValueDic.Count + 1
        IdValueDic(id) = value

        If setShield Then
            CommandLinkShieldList.Add(id)
        End If

        If description.NotNullOrEmptyS Then
            text = text & BR & description
        End If

        Buttons.Add(New TASKDIALOG_BUTTON(id, text))
        Config.dwFlags = Config.dwFlags Or Flags.TDF_USE_COMMAND_LINKS
    End Sub

    Sub AddRadioButton(text As String, value As T)
        Dim id = 1000 + IdValueDic.Count + 1
        IdValueDic(id) = value
        RadioButtons.Add(New TASKDIALOG_BUTTON(id, text))
    End Sub

    Function Show() As T
        MarshalDialogControlStructs()
        Dim checked As Boolean
        Dim hr = TaskDialogIndirect(Config, Nothing, Nothing, checked)
        CheckBoxChecked = checked

        If hr < 0 Then
            Marshal.ThrowExceptionForHR(hr)
        End If

        If TypeOf SelectedValue Is DialogResult Then
            SelectedValue = DirectCast(CObj(SelectedID), T)
        End If

        Return SelectedValue
    End Function

    Private ExitTickCount As Integer

    Function DialogProc(hwnd As IntPtr, msg As UInteger, wParam As IntPtr, lParam As IntPtr, lpRefData As IntPtr) As Integer
        Select Case msg
            Case TDN_BUTTON_CLICKED, TDN_RADIO_BUTTON_CLICKED
                If TypeOf SelectedValue Is DialogResult Then
                    SelectedIDValue = wParam.ToInt32
                Else
                    SelectedID = wParam.ToInt32
                End If
            Case TDN_TIMER
                If ExitTickCount = 0 Then
                    ExitTickCount = Environment.TickCount + Timeout * 1000
                End If

                If Environment.TickCount > ExitTickCount Then
                    Native.SendMessage(hwnd, TDM_CLICK_BUTTON, DialogResult.OK, 0)
                End If
            Case TDN_HYPERLINK_CLICKED
                Dim url = Marshal.PtrToStringUni(lParam)

                If url.StartsWith("mailto:", StringComparison.Ordinal) OrElse url Like "http*://*" Then
                    g.ShellExecute(url)
                ElseIf String.Equals(url, "copymsg:") Then
                    g.MainForm.BeginInvoke(Sub()
                                               Clipboard.SetText(MainInstruction & BR2 & Content & BR2 & ExpandedInformation)
                                               MsgInfo("Message was copied to clipboard.")
                                           End Sub)
                End If
            Case TDN_CREATED
                For Each i In CommandLinkShieldList
                    Native.SendMessage(hwnd, TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE, i, 1)
                Next
        End Select

        Return 0
    End Function

    Sub MarshalDialogControlStructs()
        If Buttons IsNot Nothing AndAlso Buttons.Count > 0 Then
            ButtonArray = AllocateAndMarshalButtons(Buttons)
            Config.pButtons = ButtonArray
            Config.cButtons = CUInt(Buttons.Count)
        End If

        If RadioButtons IsNot Nothing AndAlso RadioButtons.Count > 0 Then
            RadioButtonArray = AllocateAndMarshalButtons(RadioButtons)
            Config.pRadioButtons = RadioButtonArray
            Config.cRadioButtons = CUInt(RadioButtons.Count)
        End If
    End Sub

    Shared Function AllocateAndMarshalButtons(structs As List(Of TASKDIALOG_BUTTON)) As IntPtr
        Dim initialPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(TASKDIALOG_BUTTON)) * structs.Count)
        Dim currentPtr = initialPtr

        For Each button In structs
            Marshal.StructureToPtr(button, currentPtr, False)
            currentPtr = CType((currentPtr.ToInt64 + Marshal.SizeOf(button)), IntPtr)
        Next

        Return initialPtr
    End Function

    Private disposed As Boolean

    Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Try
            Dispose(False)
        Finally
            MyBase.Finalize()
        End Try
    End Sub

    Protected Sub Dispose(disposing As Boolean)
        If Not disposed Then
            disposed = True

            If ButtonArray <> IntPtr.Zero Then
                Marshal.FreeHGlobal(ButtonArray)
                ButtonArray = IntPtr.Zero
            End If

            If RadioButtonArray <> IntPtr.Zero Then
                Marshal.FreeHGlobal(RadioButtonArray)
                RadioButtonArray = IntPtr.Zero
            End If
        End If
    End Sub
End Class

Public Class TaskDialog
    <DllImport("comctl32.dll", CharSet:=CharSet.Unicode)>
    Shared Function TaskDialogIndirect(pTaskConfig As TASKDIALOGCONFIG, ByRef pnButton As Integer,
        ByRef pnRadioButton As Integer, ByRef pVerificationFlagChecked As Boolean) As Integer
    End Function

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
    Public Class TASKDIALOGCONFIG
        Public cbSize As UInteger
        Public hwndParent As IntPtr
        Public hInstance As IntPtr
        Public dwFlags As Flags
        Public dwCommonButtons As TaskDialogButtons
        Public pszWindowTitle As String
        Public MainIcon As TASKDIALOGCONFIG_ICON_UNION
        Public pszMainInstruction As String
        Public pszContent As String
        Public cButtons As UInteger
        Public pButtons As IntPtr
        Public nDefaultButton As Integer
        Public cRadioButtons As UInteger
        Public pRadioButtons As IntPtr
        Public nDefaultRadioButton As Integer
        Public pszVerificationText As String
        Public pszExpandedInformation As String
        Public pszExpandedControlText As String
        Public pszCollapsedControlText As String
        Public FooterIcon As TASKDIALOGCONFIG_ICON_UNION
        Public pszFooter As String
        Public pfCallback As PFTASKDIALOGCALLBACK
        Public lpCallbackData As IntPtr
        Public cxWidth As UInteger 'Width of dialog, 0-Auto
    End Class

    Public Enum Flags
        NONE = 0
        TDF_ENABLE_HYPERLINKS = &H1
        TDF_USE_HICON_MAIN = &H2
        TDF_USE_HICON_FOOTER = &H4
        TDF_ALLOW_DIALOG_CANCELLATION = &H8
        TDF_USE_COMMAND_LINKS = &H10
        TDF_USE_COMMAND_LINKS_NO_ICON = &H20
        TDF_EXPAND_FOOTER_AREA = &H40
        TDF_EXPANDED_BY_DEFAULT = &H80
        TDF_VERIFICATION_FLAG_CHECKED = &H100
        TDF_SHOW_PROGRESS_BAR = &H200
        TDF_SHOW_MARQUEE_PROGRESS_BAR = &H400
        TDF_CALLBACK_TIMER = &H800
        TDF_POSITION_RELATIVE_TO_WINDOW = &H1000
        TDF_RTL_LAYOUT = &H2000
        TDF_NO_DEFAULT_RADIO_BUTTON = &H4000
    End Enum

    <StructLayout(LayoutKind.Explicit, CharSet:=CharSet.Unicode)>
    Public Structure TASKDIALOGCONFIG_ICON_UNION
        <FieldOffset(0)> Public hMainIcon As Integer
        <FieldOffset(0)> Public pszIcon As Integer
        <FieldOffset(0)> Public spacer As IntPtr

        Sub New(i As Integer)
            spacer = IntPtr.Zero
            pszIcon = 0
            hMainIcon = i
        End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
    Public Structure TASKDIALOG_BUTTON
        Public nButtonID As Integer
        Public pszButtonText As String

        Sub New(n As Integer, txt As String)
            nButtonID = n
            pszButtonText = txt
        End Sub
    End Structure
End Class

Public Enum TaskDialogButtons
    None = &H0
    Ok = &H1
    Yes = &H2
    No = &H4
    Cancel = &H8
    Retry = &H10
    RetryCancel = Retry Or Cancel
    Close = &H20
    OkCancel = Ok Or Cancel
    YesNo = Yes Or No
    YesNoCancel = YesNo Or Cancel
End Enum

Public Enum TaskDialogIcon
    Warning = 65535
    [Error] = 65534
    Info = 65533
    Shield = 65532
    SecurityShieldBlue = 65531
    SecurityWarning = 65530
    SecurityError = 65529
    SecuritySuccess = 65528
    SecurityShieldGray = 65527
End Enum