
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks

Imports Microsoft.Win32
Imports StaxRip.UI

Public Class ProcController
    Property Proc As Proc
    Property LogTextBox As TextBox
    Property ProgressBar As LabelProgressBar
    Property ProcForm As ProcessingForm
    Property CheckBox As CheckBoxEx

    Private LogAction As Action
    Private StatusAction As Action(Of String)

    Shared Property Procs As New List(Of ProcController)
    Shared Property Aborted As Boolean
    Shared Property LastActivation As Long

    Shared Property BlockActivation As Boolean

    Sub New(proc As Proc)
        Dim fnCon As New Font("Consolas", 9 * s.UIScaleFactor)
        Dim mszW = Task.Run(Function()
                                Dim cbTxt As String = " " & proc.Title & " "
                                Return (TextRenderer.MeasureText(cbTxt, fnCon).Width, cbTxt)
                            End Function) 'Or Inline This !!??
        LogAction = New Action(AddressOf LogHandler)
        StatusAction = New Action(Of String)(AddressOf StatusHandler)
        Me.Proc = proc

        ProgressBar = New LabelProgressBar With {.Dock = DockStyle.Fill, .Font = fnCon}
        LogTextBox = New TextBox With {.Dock = DockStyle.Fill, .ScrollBars = ScrollBars.Both, .Font = fnCon, .Multiline = True, .ReadOnly = True, .WordWrap = True}
        ProcForm = g.ProcForm
        ProcForm.pnLogHost.Controls.Add(LogTextBox)
        ProcForm.pnStatusHost.Controls.Add(ProgressBar)

        AddHandler proc.ProcDisposed, AddressOf ProcDisposed
        AddHandler proc.OutputDataReceived, AddressOf DataReceived
        AddHandler proc.ErrorDataReceived, AddressOf DataReceived

        Dim cbFH As Integer = If(s.UIScaleFactor <> 1, fnCon.Height, 15)  'fh=15
        Dim pad = ProcForm.FontHeight \ 6
        CheckBox = New CheckBoxEx With {.Appearance = Appearance.Button, .Margin = New Padding(pad, pad, 0, pad), .Font = fnCon,
             .TextAlign = ContentAlignment.MiddleCenter}
        AddHandler CheckBox.Click, AddressOf Click
        'If Not  mszW.IsCompleted Then  ProcForm.BeginInvoke(Sub() ...) else 'Add This maybe, if TitleTask not Compl ????
        CheckBox.Size = New Size(mszW.Result.Width + cbFH, CInt(cbFH * 1.5)) 'Or Like That ??? - check Assembler!!! ???
        CheckBox.Text = mszW.Result.cbTxt
        'CheckBox.Size = New Size(mszW.Result + cbFH, CInt(cbFH * 1.5)) 'CheckBox.Width = sz.Width + cbFH 'CheckBox.Height = CInt(cbFH * 1.5)
        ProcForm.flpNav.Controls.Add(CheckBox)
    End Sub

    Sub DataReceived(value As String)
        If value.NullOrEmptyS Then
            Exit Sub
        End If

        Dim ret = Proc.ProcessData(value)

        If ret.Data.NullOrEmptyS Then
            Exit Sub
        End If

        If ret.Skip Then
            If Proc.IntegerFrameOutput AndAlso Proc.FrameCount > 0 Then
                Dim rdi = ret.Data.ToIntM
                If rdi <> -2147483646I Then
                    ret.Data = "Progress: " & (rdi / Proc.FrameCount * 100).ToString("0.00") & "%"
                End If
            End If

            If Proc.IntegerPercentOutput AndAlso ret.Data.IsInt Then
                ret.Data = "Progress: " & ret.Data & "%"
            End If

            ProcForm.BeginInvoke(StatusAction, ret.Data) 'removed paramAr [ret.Data]
        Else
            If ret.Data.Trim?.Length > 0 Then
                Proc.Log.WriteLine(ret.Data)
            End If

            ProcForm.BeginInvoke(LogAction)
        End If
    End Sub

    Sub LogHandler()
        LogTextBox.Text = Proc.Log.ToString
    End Sub

    Sub StatusHandler(value As String)
        ProgressBar.Text = value
        SetProgress(value)
    End Sub

    Shared LastProgress As Double

    Sub SetProgress(value As String)
        If Proc.IsSilent Then
            Exit Sub
        End If

        Dim idx As Integer = value.IndexOf("%"c) 'left
        If idx = 0 Then Exit Sub
        If idx > 0 Then
            value = value.Substring(0, idx)
            idx = value.IndexOf("["c)

            If idx >= 0 Then
                value = value.Substring(idx + 1)
            End If

            idx = value.LastIndexOf(" "c)

            If idx >= 0 Then
                value = value.Substring(idx + 1)
            End If

            'If value.IsDouble Then  'Opt
            '    Dim val = value.ToDouble
            Dim val = value.ToDouble(Double.NaN)
            If Not Double.IsNaN(val) Then

                If LastProgress <> val Then
                    ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                    ProcForm.Taskbar?.SetValue(Math.Max(val, 1), 100)
                    ProcForm.NotifyIcon.Text = val & "%"
                    ProgressBar.Value = val
                    LastProgress = val
                End If

                Exit Sub
            End If
        ElseIf Proc.Duration <> TimeSpan.Zero AndAlso value.Contains(" time=") Then
            Dim tokens = value.Right(" time=").Left(".").Split(":"c) 'ff sec. progress fix

            If tokens.Length = 3 Then
                Dim ts As New TimeSpan(tokens(0).ToInt, tokens(1).ToInt, tokens(2).ToInt)
                Dim val = 100 / Proc.Duration.TotalSeconds * ts.TotalSeconds

                If LastProgress <> val Then
                    ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                    ProcForm.Taskbar?.SetValue(Math.Max(val, 1), 100)
                    ProcForm.NotifyIcon.Text = val & "%"
                    ProgressBar.Value = val
                    LastProgress = val
                End If

                Exit Sub
            End If

            ' QAAC StdIn mode
        ElseIf Proc.Duration <> TimeSpan.Zero AndAlso value.Contains("x)") Then
            Dim tokens = value.Left(".").TrimEnd.Split(":"c)

            Select Case tokens.Length
                Case 3, 2
                    Dim ts = If(tokens.Length = 3,
                        New TimeSpan(tokens(0).ToInt, tokens(1).ToInt, tokens(2).ToInt),
                        New TimeSpan(0, tokens(0).ToInt, tokens(1).ToInt))
                    Dim val = 100 / Proc.Duration.TotalSeconds * ts.TotalSeconds

                    If LastProgress <> val Then
                        ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                        ProcForm.Taskbar?.SetValue(Math.Max(val, 1), 100)
                        ProcForm.NotifyIcon.Text = val & "%"
                        ProgressBar.Value = val
                        LastProgress = val
                    End If
                    Exit Sub
            End Select

            'Opus enc StdIn mode
        ElseIf Proc.Duration <> TimeSpan.Zero AndAlso value.Contains("x realtime,") Then
            Dim tokens = value.Right("] ").Left(".").Split(":"c)

            If tokens.Length = 3 Then
                Dim ts = New TimeSpan(tokens(0).ToInt, tokens(1).ToInt, tokens(2).ToInt)
                Dim val = 100 / Proc.Duration.TotalSeconds * ts.TotalSeconds

                If LastProgress <> val Then
                    ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                    ProcForm.Taskbar?.SetValue(Math.Max(val, 1), 100)
                    ProcForm.NotifyIcon.Text = val & "%"
                    ProgressBar.Value = val
                    LastProgress = val
                End If
                Exit Sub
            End If

        ElseIf Proc.FrameCount > 0 AndAlso value.Contains("frame=") AndAlso value.Contains("fps=") Then
            Dim frameString = value.Left("fps=").Right("frame=")
            Dim frame = frameString.ToIntM ' opt.
            If frame <> -2147483646I Then
                'If frameString.IsInt Then
                '    Dim frame = frameString.ToInt

                If frame < Proc.FrameCount Then
                    Dim progressValue = CSng(frame / Proc.FrameCount * 100)

                    If LastProgress <> progressValue Then
                        ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                        ProcForm.Taskbar?.SetValue(Math.Max(progressValue, 1), 100)
                        ProcForm.NotifyIcon.Text = progressValue & "%"
                        ProgressBar.Value = progressValue
                        LastProgress = progressValue
                    End If

                    Exit Sub
                End If
            End If
        ElseIf value.Contains("/100)") Then
            Dim percentString = value.Right("(").Left("/")
            Dim percent = percentString.ToIntM ' opt.
            If percent <> -2147483646I Then
                'If percentString.IsInt Then
                '    Dim percent = percentString.ToInt

                If LastProgress <> percent Then
                    ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                    ProcForm.Taskbar?.SetValue(Math.Max(percent, 1), 100)
                    ProcForm.NotifyIcon.Text = percent & "%"
                    ProgressBar.Value = percent
                    LastProgress = percent
                End If

                Exit Sub
            End If
        End If

        If LastProgress <> 0 Then
            ProcForm.NotifyIcon.Text = "StaxRip"
            ProcForm.Taskbar?.SetState(TaskbarStates.NoProgress)
            LastProgress = 0
        End If
    End Sub

    Sub Click(sender As Object, e As EventArgs)
        SyncLock Procs
            For Each i In Procs
                If i.CheckBox IsNot sender Then i.Deactivate()
            Next

            For Each i In Procs
                If i.CheckBox Is sender Then i.Activate()
            Next
        End SyncLock
    End Sub

    Sub ProcDisposed()
        ProcForm.BeginInvoke(Sub() Cleanup())
    End Sub

    Shared Sub Abort()
        Aborted = True
        Registry.CurrentUser.Write("Software\" & Application.ProductName, "ShutdownMode", 0)

        For Each i In Procs.ToArray
            i.Proc.Abort = True
            i.Proc.Kill()
        Next
    End Sub

    Shared Sub Skip()
        For Each i In Procs.ToArray
            i.Proc.Skip = True
            i.Proc.Kill()
        Next
    End Sub

    Shared Sub Suspend()
        For Each process In GetProcesses()
            For Each thread As ProcessThread In process.Threads
                Dim handle = OpenThread(ThreadAccess.SUSPEND_RESUME, False, thread.Id)
                SuspendThread(handle)
                CloseHandle(handle)
            Next
        Next
    End Sub

    Shared Sub ResumeProcs()
        For Each process In GetProcesses()
            For x = process.Threads.Count - 1 To 0 Step -1
                Dim h = OpenThread(ThreadAccess.SUSPEND_RESUME, False, process.Threads(x).Id)
                ResumeThread(h)
                CloseHandle(h)
            Next
        Next
    End Sub

    Shared Function GetProcesses() As List(Of Process)
        Dim ret As New List(Of Process)

        For Each procButton In Procs.ToArray
            If String.Equals(procButton.Proc.Process.ProcessName, "cmd") Then
                For Each process In ProcessHelp.GetChilds(procButton.Proc.Process)
                    If {"conhost", "vspipe"}.ContainsString(process.ProcessName) Then
                        Continue For
                    End If

                    ret.Add(process)
                Next
            Else
                ret.Add(procButton.Proc.Process)
            End If
        Next

        Return ret
    End Function

    Sub Cleanup()
        SyncLock Procs
            Procs.Remove(Me)

            RemoveHandler Proc.ProcDisposed, AddressOf ProcDisposed
            RemoveHandler Proc.OutputDataReceived, AddressOf DataReceived
            RemoveHandler Proc.ErrorDataReceived, AddressOf DataReceived
            RemoveHandler CheckBox.Click, AddressOf Click

            ProcForm.flpNav.Controls.Remove(CheckBox)
            ProcForm.pnLogHost.Controls.Remove(LogTextBox)
            ProcForm.pnStatusHost.Controls.Remove(ProgressBar)

            CheckBox.Dispose()
            LogTextBox.Dispose()
            ProgressBar.Dispose()

            For Each i In Procs
                i.Deactivate()
            Next

            If Procs.Count > 0 Then
                Procs(0).Activate()
            End If
        End SyncLock

        If Not Proc.Succeeded And Not Proc.Skip Then
            Abort()
        End If

        Task.Run(Sub()
                     Thread.Sleep(500)

                     SyncLock Procs
                         If Procs.Count = 0 AndAlso Not g.IsJobProcessing Then
                             Finished()
                         End If
                     End SyncLock
                 End Sub)
    End Sub

    Shared Sub Finished()
        If g.ProcForm IsNot Nothing Then
            If g.ProcForm.tlpMain.InvokeRequired Then
                g.ProcForm.BeginInvoke(Sub() g.ProcForm.HideForm())
            Else
                g.ProcForm.HideForm()
            End If
        End If

        If g.MainForm.Disposing OrElse g.MainForm.IsDisposed Then
            Exit Sub
        End If

        Dim mainSub = Sub()
                          If Not Aborted Then
                              BlockActivation = True
                          End If

                          g.MainForm.Show()
                          g.MainForm.Refresh()
                          Aborted = False
                      End Sub

        If g.MainForm.tlpMain.InvokeRequired Then
            g.MainForm.BeginInvoke(mainSub)
        Else
            mainSub.Invoke
        End If
    End Sub

    Shared Function IsLastActivationLessThan(sec As Integer) As Boolean
        Return (LastActivation + sec * 1000) < Environment.TickCount
    End Function

    Sub Activate()
        CheckBox.Checked = True
        Proc.IsSilent = False
        LogTextBox.Visible = True
        LogTextBox.BringToFront()
        LogTextBox.Text = Proc.Log.ToString
        ProgressBar.Visible = True
        ProgressBar.BringToFront()
    End Sub

    Sub Deactivate()
        CheckBox.Checked = False
        Proc.IsSilent = True
        LogTextBox.Visible = False
        ProgressBar.Visible = False
    End Sub

    'Shared Sub AddProc(proc As Proc)
    '    SyncLock Procs
    '        Dim pc As New ProcController(proc)
    '        Procs.Add(pc)

    '        If Procs.Count = 1 Then
    '            pc.Activate()
    '        Else
    '            pc.Deactivate()
    '        End If
    '    End SyncLock
    'End Sub

    Shared Sub Start(proc As Proc)
        If Aborted Then
            Throw New AbortException
        End If

        Dim fnCon As New Font("Consolas", 9 * s.UIScaleFactor)
        Dim mszW = Task.Run(Function()
                                Dim cbTxt As String = " " & proc.Title & " "
                                Return (TextRenderer.MeasureText(cbTxt, fnCon).Width, cbTxt)
                            End Function) 'Or Inline This !!??

        If g.MainForm.Visible Then
            g.MainForm.Hide()
        End If

        SyncLock Procs
            If g.ProcForm Is Nothing Then
                Dim thread = New Thread(Sub()
                                            g.ProcForm = New ProcessingForm
                                            Application.Run(g.ProcForm)
                                        End Sub)

                thread.SetApartmentState(ApartmentState.STA)
                thread.Start()

                While Not ProcessingForm.WasHandleCreated
                    Thread.Sleep(50)
                End While
            End If
        End SyncLock

        g.ProcForm.Invoke(Sub()
                              If Not g.ProcForm.WindowState = FormWindowState.Minimized Then
                                  g.ProcForm.Show()
                                  g.ProcForm.WindowState = FormWindowState.Normal

                                  If Not BlockActivation Then
                                      g.ProcForm.Activate()
                                      BlockActivation = True
                                  End If
                              End If
                              'AddProc(proc) 'Inlined:
                              ' Dim pptt = proc.Title 'ToDO: Add par to new ProContr with as Task Title ???
                              SyncLock Procs
                                  Dim pc As New ProcController(proc)
                                  Procs.Add(pc)

                                  If Procs.Count = 1 Then
                                      pc.Activate()
                                  Else
                                      pc.Deactivate()
                                  End If
                              End SyncLock

                              g.ProcForm.UpdateControls()
                          End Sub)

    End Sub

    <DllImport("kernel32.dll")>
    Shared Function SuspendThread(hThread As IntPtr) As UInt32
    End Function

    <DllImport("kernel32.dll")>
    Shared Function OpenThread(dwDesiredAccess As ThreadAccess, bInheritHandle As Boolean, dwThreadId As Integer) As IntPtr
    End Function

    <DllImport("kernel32.dll")>
    Shared Function ResumeThread(hThread As IntPtr) As UInt32
    End Function

    <DllImport("kernel32.dll")>
    Shared Function CloseHandle(hObject As IntPtr) As Boolean
    End Function

    <Flags()>
    Public Enum ThreadAccess As Integer
        TERMINATE = &H1
        SUSPEND_RESUME = &H2
        GET_CONTEXT = &H8
        SET_CONTEXT = &H10
        SET_INFORMATION = &H20
        QUERY_INFORMATION = &H40
        SET_THREAD_TOKEN = &H80
        IMPERSONATE = &H100
        DIRECT_IMPERSONATION = &H200
    End Enum
End Class
