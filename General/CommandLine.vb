
Imports System.Text
Imports StaxRip.UI

Namespace CommandLine
    Public MustInherit Class CommandLineParams
        Property Title As String
        Property Separator As String = " "

        Event ValueChanged(item As CommandLineParam)
        MustOverride ReadOnly Property Items As List(Of CommandLineParam)

        MustOverride Function GetCommandLine(includePaths As Boolean, includeExecutable As Boolean, Optional pass As Integer = 1) As String

        MustOverride Function GetPackage() As Package

        Sub Init(store As PrimitiveStore)
            For n = 0 To Items.Count - 1
                Items(n).InitParam(store, Me)
            Next n
        End Sub

        Protected ItemsValue As List(Of CommandLineParam)

        <Runtime.CompilerServices.MethodImpl(AggrInlin)>
        Protected Sub Add(path As String, ParamArray items As CommandLineParam())
            For n = 0 To items.Length - 1
                Dim i = items(n)
                i.Path = path
                ItemsValue.Add(i)
            Next n
        End Sub

        Function GetStringParam(switch As String) As StringParam
            Return Items.OfType(Of StringParam).FirstOrDefault(Function(item) item.Switch.EqualsExS(switch))
        End Function

        Function GetOptionParam(switch As String) As OptionParam
            Return Items.OfType(Of OptionParam).FirstOrDefault(Function(item) item.Switch.EqualsExS(switch))
        End Function

        Function GetNumParamByName(name As String) As NumParam
            Return Items.OfType(Of NumParam).FirstOrDefault(Function(item) item.Name.EqualsExS(name))
        End Function

        Sub RaiseValueChanged(item As CommandLineParam)
            OnValueChanged(item)
        End Sub

        Overridable Sub ShowHelp(id As String)
        End Sub

        Protected Overridable Sub OnValueChanged(item As CommandLineParam)
            For n = 0 To Items.Count - 1
                Dim i = Items(n)
                If i.VisibleFunc IsNot Nothing Then
                    i.Visible = i.Visible
                End If
            Next n

            RaiseEvent ValueChanged(item)
        End Sub

        Function GetSAR() As String
            Dim param = GetStringParam("--sar")

            If param?.Value?.Length > 0 Then
                Dim targetPAR = Calc.GetTargetPAR
                Dim val = Calc.ParseCustomAR(param.Value, targetPAR.X, targetPAR.Y)
                Dim isInTolerance = val = targetPAR AndAlso Not Calc.IsARSignalingRequired

                If val.X <> 0 AndAlso val <> New Point(1, 1) AndAlso Not isInTolerance Then
                    Return "--sar " & val.X & ":" & val.Y
                End If
            End If
        End Function

        Sub Execute()
            If Not g.VerifyRequirements Then
                Exit Sub
            End If

            p.Script.Synchronize()

            If g.IsWindowsTerminalAvailable Then
                Dim cl = "cmd.exe /S /K --% """ + GetCommandLine(True, True) + """"
                Dim base64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(cl)) 'UTF16LE
                g.Execute("wt.exe", "powershell.exe -NoLogo -NoExit -NoProfile -EncodedCommand """ + base64 + """")
            Else
                g.Execute("cmd.exe", "/S /K """ + GetCommandLine(True, True) + """")
            End If
        End Sub
    End Class

    Public MustInherit Class CommandLineParam
        Property AlwaysOn As Boolean
        Property ArgsFunc As Func(Of String)
        Property Help As String
        Property HelpSwitch As String
        Property ImportAction As Action(Of String, String)
        Property Label As String
        Property LeftMargin As Double
        Property Name As String
        Property NoSwitch As String
        Property Path As String
        Property Switch As String
        Property Switches As IEnumerable(Of String) 'ToDo Make switches array!!!!!!
        Property Text As String
        Property URLs As List(Of String)
        Property VisibleFunc As Func(Of Boolean)

        Friend Store As PrimitiveStore
        Friend Params As CommandLineParams

        MustOverride Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
        MustOverride Function GetControl() As Control

        Overridable Function GetArgs() As String
        End Function

        Function GetSwitches() As HashSet(Of String)
            Dim ret As New HashSet(Of String)(StringComparer.Ordinal)

            If Switch?.Length > 0 Then ret.Add(Switch)
            If NoSwitch?.Length > 0 Then ret.Add(NoSwitch)
            If HelpSwitch?.Length > 0 Then ret.Add(HelpSwitch)

            If Switches IsNot Nothing Then
                For Each i In Switches
                    If i?.Length > 0 Then
                        ret.Add(i)
                    End If
                Next
            End If

            Return ret
        End Function

        Property VisibleValue As Boolean = True

        Property Visible As Boolean
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                If VisibleFunc IsNot Nothing Then
                    Return VisibleFunc.Invoke
                End If

                Return VisibleValue
            End Get
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Boolean)
                If value <> VisibleValue Then
                    VisibleValue = value

                    Dim c = GetControl()

                    If c IsNot Nothing Then
                        If TypeOf c.Parent Is SimpleUI.EmptyBlock Then
                            c = c.Parent
                        End If

                        c.Visible = value
                    End If
                End If
            End Set
        End Property

        <Runtime.CompilerServices.MethodImpl(AggrInlin)> Function GetKey() As String
            If Name?.Length > 0 Then
                Return Name
            End If

            If Switch?.Length > 0 Then
                Return Switch
            End If

            If HelpSwitch?.Length > 0 Then
                Return Text & HelpSwitch
            End If

            Return Text
        End Function
    End Class

    Public Class BoolParam
        Inherits CommandLineParam

        Property DefaultValue As Boolean
        Property CheckBox As CheckBox
        Property IntegerValue As Boolean

        Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            Dim gk As String = GetKey()
            If Not store.Bool.ContainsKey(gk) Then
                store.Bool(gk) = ValueValue
            End If
        End Sub

        Overloads Sub InitParam(cb As CheckBox)
            CheckBox = cb
            CheckBox.Checked = Value
            AddHandler CheckBox.CheckedChanged, AddressOf CheckedChanged
            Dim cbdeh As EventHandler = Sub()
                                            RemoveHandler CheckBox.Disposed, cbdeh
                                            RemoveHandler CheckBox.CheckedChanged, AddressOf CheckedChanged
                                            CheckBox = Nothing
                                        End Sub
            AddHandler CheckBox.Disposed, cbdeh
        End Sub

        Sub CheckedChanged(sender As Object, e As EventArgs)
            Value = CheckBox.Checked
            Params.RaiseValueChanged(Me)
        End Sub

        Overrides Function GetArgs() As String
            If Switch.NullOrEmptyS AndAlso NoSwitch.NullOrEmptyS AndAlso ArgsFunc Is Nothing Then
                Return Nothing
            End If

            If Not Visible Then
                Return Nothing
            End If

            If ArgsFunc Is Nothing Then
                Dim val As Boolean = Value
                If val AndAlso DefaultValue = False Then
                    If IntegerValue Then
                        Return Switch & Params.Separator & "1"
                    Else
                        Return Switch
                    End If
                ElseIf Not val AndAlso DefaultValue Then
                    If IntegerValue Then
                        Return Switch & Params.Separator & "0"
                    Else
                        Return NoSwitch
                    End If
                End If
            Else
                Return ArgsFunc.Invoke()
            End If
        End Function

        Private ValueValue As Boolean

        Property Value As Boolean
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                Return Store.Bool(GetKey)
            End Get
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Boolean)
                ValueValue = value

                If Store IsNot Nothing Then
                    Store.Bool(GetKey) = value
                End If

                If CheckBox IsNot Nothing Then
                    CheckBox.Checked = value
                End If
            End Set
        End Property

        WriteOnly Property Init As Boolean
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Boolean)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Public Overrides Function GetControl() As Control
            Return CheckBox
        End Function
    End Class

    Public Class NumParam
        Inherits CommandLineParam

        Property NumEdit As NumEdit
        Property DefaultValue As Double

        Private ConfigValue As Double()

        Property Config As Double() 'min, max, step, decimal places
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                If ConfigValue Is Nothing Then
                    Return {Double.MinValue, Double.MaxValue, 1, 0}
                End If

                Return ConfigValue
            End Get
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Double())
                ConfigValue = {value(0), value(1), 1, 0}

                If value.Length > 2 Then ConfigValue(2) = value(2)
                If value.Length > 3 Then ConfigValue(3) = value(3)

                If ConfigValue(0) = 0 AndAlso ConfigValue(1) = 0 Then
                    ConfigValue(0) = Double.MinValue
                    ConfigValue(1) = Double.MaxValue
                End If
            End Set
        End Property

        Overloads Sub InitParam(ne As NumEdit)
            NumEdit = ne
            NumEdit.Value = Value
            AddHandler NumEdit.ValueChanged, AddressOf ValueChanged
            Dim nedeh As EventHandler = Sub()
                                            RemoveHandler NumEdit.Disposed, nedeh
                                            RemoveHandler NumEdit.ValueChanged, AddressOf ValueChanged
                                            NumEdit = Nothing
                                        End Sub
            AddHandler NumEdit.Disposed, nedeh
        End Sub

        Overloads Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            Dim gk As String = GetKey()
            If Not store.Double.ContainsKey(gk) Then
                store.Double(gk) = ValueValue
            End If
        End Sub

        Sub ValueChanged(ne As NumEdit)
            If Config(3) = 0 Then
                Value = CInt(ne.Value)
            Else
                Value = ne.Value
            End If

            Params.RaiseValueChanged(Me)
        End Sub

        Private ValueValue As Double

        Property Value As Double
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                Return Store.Double(GetKey)
            End Get
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Double)
                ValueValue = value
                If Store IsNot Nothing Then Store.Double(GetKey) = value
                If NumEdit IsNot Nothing Then NumEdit.Value = value
            End Set
        End Property

        WriteOnly Property Init As Double
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Double)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Overrides Function GetArgs() As String
            If Not Visible Then Return Nothing
            If Switch.NullOrEmptyS AndAlso ArgsFunc Is Nothing Then Return Nothing

            If ArgsFunc Is Nothing Then
                Dim val As Double = Value
                If val <> DefaultValue OrElse AlwaysOn Then
                    Return Switch & Params.Separator & val.ToInvStr
                End If
            Else
                Return ArgsFunc.Invoke()
            End If
        End Function

        Public Overrides Function GetControl() As Control
            Return NumEdit
        End Function
    End Class

    Public Class OptionParam
        Inherits CommandLineParam

        Property Options As String()
        Property Values As String()
        Property Expand As Boolean
        Property MenuButton As MenuButton
        Property DefaultValue As Integer
        Property IntegerValue As Boolean

        Sub ShowOption(value As Integer, visible As Boolean)
            If MenuButton IsNot Nothing Then
                For Each i In MenuButton.Menu.Items.OfType(Of ToolStripMenuItem)
                    If value.Equals(i.Tag) Then i.Visible = visible
                Next
            End If
        End Sub

        Overloads Sub InitParam(mb As MenuButton)
            MenuButton = mb
            MenuButton.Value = Value
            AddHandler MenuButton.ValueChangedUser, AddressOf ValueChangedUser
            Dim mbeh As EventHandler = Sub()
                                           RemoveHandler MenuButton.Disposed, mbeh
                                           RemoveHandler MenuButton.ValueChangedUser, AddressOf ValueChangedUser
                                           MenuButton = Nothing
                                       End Sub
            AddHandler MenuButton.Disposed, mbeh
        End Sub

        Public Overloads Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            Dim gk As String = GetKey()
            If Not store.Int.ContainsKey(gk) Then
                store.Int(gk) = ValueValue
            End If
        End Sub

        ReadOnly Property OptionText As String
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                Return Options(Value)
            End Get
        End Property

        ReadOnly Property ValueText As String
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                If Values Is Nothing Then
                    Return Options(Value).ToLower(InvCult)
                Else
                    Return Values(Value)
                End If
            End Get
        End Property

        Sub ValueChangedUser(obj As Object)
            Value = CInt(obj)
            Params.RaiseValueChanged(Me)
        End Sub

        Private ValueValue As Integer

        Property Value As Integer
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                Dim ret = Store.Int(GetKey)
                Dim oL As Integer = Options.Length - 1
                If ret > oL Then ret = oL
                Return ret
            End Get
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Integer)
                ValueValue = value
                If Store IsNot Nothing Then
                    Store.Int(GetKey) = value
                End If

                If MenuButton IsNot Nothing Then
                    MenuButton.Value = ValueValue
                End If
            End Set
        End Property

        WriteOnly Property Init As Integer
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Integer)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Overrides Function GetArgs() As String
            If Not Visible Then Return Nothing

            If ArgsFunc Is Nothing Then
                Dim val As Integer = Value
                If val <> DefaultValue OrElse AlwaysOn Then
                    If Values IsNot Nothing Then
                        If Values(val).StartsWith("--", StringComparison.Ordinal) Then
                            Return Values(val)
                        ElseIf Switch?.Length > 0 Then
                            Return Switch & Params.Separator & Values(val)
                        End If
                    ElseIf Switch?.Length > 0 Then
                        If IntegerValue Then
                            Return Switch & Params.Separator & val
                        Else
                            Return Switch & Params.Separator & Options(val).ToLower(InvCult).Replace(" ", "")
                        End If
                    End If
                End If
            Else
                Return ArgsFunc.Invoke
            End If
        End Function

        Public Overrides Function GetControl() As Control
            Return MenuButton
        End Function
    End Class

    Public Class StringParam
        Inherits CommandLineParam

        Property DefaultValue As String
        Property TextEdit As TextEdit
        Property Quotes As QuotesMode
        Property RemoveSpace As Boolean
        Property InitAction As Action(Of SimpleUI.TextBlock)
        Property BrowseFileFilter As String
        Property BrowseFolderText As String
        Property Menu As String
        Property Expand As Boolean = True

        WriteOnly Property BrowseFile As Boolean
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As Boolean)
                BrowseFileFilter = "*.*|*.*"
            End Set
        End Property

        Overloads Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            Dim gk As String = GetKey()
            If Not store.String.ContainsKey(gk) Then
                store.String(gk) = ValueValue
            End If
        End Sub

        Overloads Sub InitParam(te As SimpleUI.TextBlock)
            TextEdit = te.Edit
            TextEdit.Text = Value
            AddHandler TextEdit.TextChanged, AddressOf TextChanged
            Dim tedeh As EventHandler = Sub()
                                            If TextEdit IsNot Nothing Then
                                                RemoveHandler TextEdit.Disposed, tedeh
                                                RemoveHandler TextEdit.TextChanged, AddressOf TextChanged
                                                TextEdit = Nothing
                                            End If
                                        End Sub
            AddHandler TextEdit.Disposed, tedeh

            InitAction?.Invoke(te)
        End Sub

        Sub TextChanged()
            Value = TextEdit.Text
            Params.RaiseValueChanged(Me)
        End Sub

        Overrides Function GetArgs() As String
            If Not Visible Then
                Return Nothing
            End If

            If ArgsFunc IsNot Nothing Then
                Return ArgsFunc.Invoke
            Else
                Dim val = Value
                If val?.Length > 0 Then
                    If RemoveSpace Then
                        val = val.Replace(" ", "")
                    End If

                    If Not String.Equals(val, DefaultValue) Then
                        If Switch.NullOrEmptyS Then
                            If AlwaysOn Then
                                If Quotes = QuotesMode.Always Then
                                    Return """" & val & """"
                                ElseIf Quotes = QuotesMode.Auto Then
                                    Return val.Escape
                                Else
                                    Return val
                                End If
                            End If
                        Else
                            If Quotes = QuotesMode.Always Then
                                Return Switch & Params.Separator & """" & val & """"
                            ElseIf Quotes = QuotesMode.Auto Then
                                Return Switch & Params.Separator & val.Escape
                            Else
                                Return Switch & Params.Separator & val
                            End If
                        End If
                    End If
                End If
            End If
        End Function

        Private ValueValue As String

        Property Value As String
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Get
                Return Store.String(GetKey)
            End Get
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As String)
                ValueValue = value

                If Store IsNot Nothing Then
                    Store.String(GetKey) = value
                End If

                If TextEdit IsNot Nothing Then
                    TextEdit.Text = value
                End If
            End Set
        End Property

        WriteOnly Property Init As String
            <Runtime.CompilerServices.MethodImpl(AggrInlin)> Set(value As String)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Public Overrides Function GetControl() As Control
            Return TextEdit
        End Function
    End Class
End Namespace