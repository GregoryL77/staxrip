
Imports System.ComponentModel

Namespace UI
    <ToolboxItem(False)>
    Class CriteriaItemControl
        Sub New(allCriteria As List(Of Criteria))
            InitializeComponent()

            mbProperties.Menu.SuspendLayout()
            ActionMenuItem.LayoutSuspendCreate(32)
            For Each c In allCriteria
                If c.Name?.StartsWith("Source ", StringComparison.Ordinal) Then
                    mbProperties.Add("Source | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("Target ", StringComparison.Ordinal) Then
                    mbProperties.Add("Target | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("Audio ", StringComparison.Ordinal) Then
                    mbProperties.Add("Audio | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("Crop ", StringComparison.Ordinal) Then
                    mbProperties.Add("Crop | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("Encoder ", StringComparison.Ordinal) Then
                    mbProperties.Add("Encoder | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("AviSynth/VapourSynth", StringComparison.Ordinal) Then
                    mbProperties.Add("AviSynth/VapourSynth | " + c.Name, c, c.Description)
                ElseIf Not c.Name?.EndsWith(" Directory", StringComparison.Ordinal) AndAlso Not c.Name?.EndsWith(" File", StringComparison.Ordinal) Then
                    mbProperties.Add(c.Name, c, c.Description)
                End If

                If c.Name?.EndsWith(" Directory", StringComparison.Ordinal) Then
                    mbProperties.Add("Directories | " + c.Name, c, c.Description)
                ElseIf c.Name?.EndsWith(" Folder", StringComparison.Ordinal) Then
                    mbProperties.Add("Folders | " + c.Name, c, c.Description)
                ElseIf c.Name?.EndsWith(" File", StringComparison.Ordinal) Then
                    mbProperties.Add("Files | " + c.Name, c, c.Description)
                End If
            Next c
            ActionMenuItem.LayoutResume(False)
            mbProperties.Menu.ResumeLayout(False)
        End Sub

        Protected Overrides Sub OnLayout(e As LayoutEventArgs)
            Height = bnRemove.Height + 6
            te.Height = bnRemove.Height
            mbCondition.Height = bnRemove.Height
            mbProperties.Height = bnRemove.Height
            MyBase.OnLayout(e)
        End Sub

        Private CriteriaValue As Criteria

        Property Criteria() As Criteria
            Get
                Return CriteriaValue
            End Get
            Set(Value As Criteria)
                If Not Value Is Nothing Then
                    CriteriaValue = DirectCast(ObjectHelp.GetCopy(Value), Criteria)

                    For Each c As Criteria In mbProperties.Items
                        If c.Name = Criteria.Name Then
                            mbProperties.Value = c
                            Exit For
                        End If
                    Next

                    mbCondition.Menu.Items.ClearAndDispose
                    mbCondition.Add(CriteriaValue.ConditionNames)
                    mbCondition.Value = CriteriaValue.ConditionName

                    te.TextBox.SetTextWithoutTextChangedEvent(CriteriaValue.ValueString)
                End If
            End Set
        End Property

        Sub bnRemove_Click() Handles bnRemove.Click
            Parent.Controls.Remove(Me)
        End Sub

        Sub te_TextChanged() Handles te.TextChanged
            Criteria.ValueString = te.Text
        End Sub

        Sub mbCondition_ValueChangedUser(value As Object) Handles mbCondition.ValueChangedUser
            Criteria.ConditionName = mbCondition.Value.ToString
        End Sub

        Sub mbProperties_ValueChangedUser(value As Object) Handles mbProperties.ValueChangedUser
            Criteria = DirectCast(value, Criteria)
        End Sub
    End Class
End Namespace
