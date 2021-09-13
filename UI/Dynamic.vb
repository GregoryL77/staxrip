Imports System.ComponentModel
Imports System.Globalization
Imports System.Drawing.Design

Namespace UI
    <Serializable()>
    Public Class PropertyGridOption
        Public Name As String
        Public Value As Object

        Sub New(name As String, value As Object)
            Me.Name = name
            Me.Value = value
        End Sub

        Overrides Function ToString() As String
            Return Name
        End Function
    End Class

    Public Class DefaultValuesConverter
        Inherits TypeConverter

        Overloads Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Overloads Overrides Function GetStandardValues(context As ITypeDescriptorContext) As StandardValuesCollection
            Return New StandardValuesCollection(CType(context.PropertyDescriptor, GridPropertyDescriptor).GridProperty.DefaultValues)
        End Function

        Overloads Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
            If sourceType Is GetType(String) Then
                Return True
            End If

            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Overloads Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo, value As Object) As Object
            Dim sourceValue As String = TryCast(value, String)
            If sourceValue IsNot Nothing Then
                Dim gp As GridProperty = CType(context.PropertyDescriptor, GridPropertyDescriptor).GridProperty

                For Each i As Object In gp.DefaultValues
                    If EqualsExS(i.ToString, sourceValue) Then
                        Return i
                    End If
                Next

                If gp.Value.GetType Is GetType(String) Then
                    Return sourceValue
                End If

                Dim tc = TypeDescriptor.GetConverter(gp.Value)

                If tc IsNot Nothing Then
                    If tc.CanConvertFrom(GetType(String)) Then
                        Return tc.ConvertFrom(sourceValue)
                    End If
                End If

                Return gp.Value
            End If

            Return MyBase.ConvertFrom(context, culture, value)
        End Function

        Overloads Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType Is GetType(String) Then
                Return True
            End If

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Overloads Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType Is GetType(String) Then
                Return value.ToString
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class

    Public Class GridProperty
        Public Name As String
        Public Category As String
        Public Description As String
        Public TypeEditor As Type
        Public TypeConverter As Type
        Public DefaultValues As Object()

        Private ValueValue As Object

        Property Value() As Object
            Get
                Return ValueValue
            End Get
            Set(Value As Object)
                If Value.GetType.IsValueType OrElse TypeOf Value Is String Then
                    ValueValue = Value
                Else
                    ValueValue = ObjectHelp.GetCopy(Value)
                End If
            End Set
        End Property

        Sub SetGridOption(gridOptionValue As Object)
            TypeConverter = GetType(DefaultValuesConverter)

            For Each i As PropertyGridOption In DefaultValues
                If i.Value.Equals(gridOptionValue) Then
                    Value = i
                    Exit Sub
                End If
            Next

            Throw New Exception
        End Sub
    End Class

    Public Class GridTypeDescriptor
        Implements ICustomTypeDescriptor

        Private YesNoConverter As Type

        Sub New()
            Items = New List(Of Object)
            YesNoConverter = GetType(YesNoConverter)
        End Sub

        Sub New(yesNoConverter As Type)
            Items = New List(Of Object)
            Me.YesNoConverter = yesNoConverter
        End Sub

        Property Items As List(Of Object)

#Region "ICustomTypeDescriptor"
        Function GetAttributes() As AttributeCollection Implements ICustomTypeDescriptor.GetAttributes
            Return TypeDescriptor.GetAttributes(Me, True)
        End Function

        Function GetClassName() As String Implements ICustomTypeDescriptor.GetClassName
            Return TypeDescriptor.GetClassName(Me, True)
        End Function

        Function GetComponentName() As String Implements ICustomTypeDescriptor.GetComponentName
            Return TypeDescriptor.GetComponentName(Me, True)
        End Function

        Function GetConverter() As TypeConverter Implements ICustomTypeDescriptor.GetConverter
            Return TypeDescriptor.GetConverter(Me, True)
        End Function

        Function GetDefaultEvent() As EventDescriptor Implements ICustomTypeDescriptor.GetDefaultEvent
            Return TypeDescriptor.GetDefaultEvent(Me, True)
        End Function

        Function GetDefaultProperty() As PropertyDescriptor Implements ICustomTypeDescriptor.GetDefaultProperty
            Return Nothing
        End Function

        Function GetEditor(editorBaseType As Type) As Object Implements ICustomTypeDescriptor.GetEditor
            Return TypeDescriptor.GetEditor(Me, editorBaseType, True)
        End Function

        Function GetEvents() As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
            Return TypeDescriptor.GetEvents(Me, True)
        End Function

        Function GetEvents(attributes() As Attribute) As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
            Return TypeDescriptor.GetEvents(Me, attributes, True)
        End Function

        Function GetProperties() As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
            Return CType(Me, ICustomTypeDescriptor).GetProperties(New Attribute(0) {})
        End Function

        Function GetProperties(attributes() As Attribute) As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
            Dim p As New ArrayList

            For Each i As DictionaryEntry In Items
                Dim a As New ArrayList
                Dim gp As GridProperty = CType(i.Value, GridProperty)

                If gp.Category.NotNullOrEmptyS Then
                    a.Add(New CategoryAttribute(gp.Category))
                End If

                If gp.Description.NotNullOrEmptyS Then
                    a.Add(New DescriptionAttribute(gp.Description))
                End If

                If gp.TypeEditor IsNot Nothing Then
                    a.Add(New EditorAttribute(gp.TypeEditor, GetType(UITypeEditor)))
                End If

                If gp.TypeConverter IsNot Nothing Then
                    a.Add(New TypeConverterAttribute(gp.TypeConverter))
                Else
                    If gp.Value.GetType Is GetType(Boolean) Then
                        a.Add(New TypeConverterAttribute(YesNoConverter))
                    End If
                End If

                Dim d As New GridPropertyDescriptor(gp.Name, gp,
                    CType(a.ToArray(GetType(Attribute)), Attribute()))

                p.Add(d)
            Next

            Return New PropertyDescriptorCollection(CType(p.ToArray(GetType(
                PropertyDescriptor)), PropertyDescriptor()))
        End Function

        Function GetPropertyOwner(pd As PropertyDescriptor) As Object Implements ICustomTypeDescriptor.GetPropertyOwner
            Return Me
        End Function
#End Region

        Sub Add(gp As GridProperty)
            Items.Add(New DictionaryEntry(gp.Name, gp))
        End Sub

        Function GetValue(name As String) As Object
            For Each i As DictionaryEntry In Items
                If EqualsExS(CStr(i.Key), name) Then
                    Return CType(i.Value, GridProperty).Value
                End If
            Next

            Return Nothing
        End Function

        Function GetString(name As String) As String
            Return DirectCast(GetValue(name), String)
        End Function

        Function GetBool(name As String) As Boolean
            Return DirectCast(GetValue(name), Boolean)
        End Function

        Function GetInt(name As String) As Integer
            Return DirectCast(GetValue(name), Integer)
        End Function

        Function GetGridOption(name As String) As PropertyGridOption
            Return CType(GetValue(name), PropertyGridOption)
        End Function
    End Class

    Public Class GridPropertyDescriptor
        Inherits PropertyDescriptor

        Public GridProperty As GridProperty

        Sub New(name As String, item As GridProperty, attributes() As Attribute)
            MyBase.New(name, attributes)

            GridProperty = item
        End Sub

        Overrides ReadOnly Property ComponentType() As Type
            Get
                Return GridProperty.GetType()
            End Get
        End Property

        Overrides ReadOnly Property IsReadOnly() As Boolean
            Get
                Return Attributes.Matches(ReadOnlyAttribute.Yes)
            End Get
        End Property

        Overrides ReadOnly Property PropertyType() As Type
            Get
                Return GridProperty.Value.GetType()
            End Get
        End Property

        Overrides Function CanResetValue(component As Object) As Boolean
        End Function

        Overrides Function GetValue(component As Object) As Object
            Return GridProperty.Value
        End Function

        Overrides Sub ResetValue(component As Object)
        End Sub

        Overrides Sub SetValue(component As Object, value As Object)
            GridProperty.Value = value
        End Sub

        Overrides Function ShouldSerializeValue(component As Object) As Boolean
        End Function
    End Class

    Public Class YesNoConverter
        Inherits TypeConverter

        Protected Yes As String = "Yes"
        Protected No As String = "No"

        Overloads Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Overloads Overrides Function GetStandardValues(context As ITypeDescriptorContext) As StandardValuesCollection
            Return New StandardValuesCollection(New Boolean() {True, False})
        End Function

        Overloads Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
            If sourceType Is GetType(String) Then
                Return True
            End If

            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Overloads Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo, value As Object) As Object
            Dim vStr As String = TryCast(value, String)
            If vStr IsNot Nothing Then
                If String.Equals(vStr, Yes) Then
                    Return True
                Else
                    Return False
                End If
            End If

            Return MyBase.ConvertFrom(context, culture, value)
        End Function

        Overloads Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType Is GetType(String) Then
                Return True
            End If

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Overloads Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType Is GetType(String) Then
                If DirectCast(value, Boolean) Then
                    Return Yes
                Else
                    Return No
                End If
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class

    Public Class NonDisplayConverter
        Inherits TypeConverter

        Overloads Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType Is GetType(String) Then
                Return "<...>"
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class

    'base cannot be applied on enums
    <AttributeUsage(AttributeTargets.All)>
    Public Class DispNameAttribute
        Inherits DisplayNameAttribute

        Sub New(name As String)
            DisplayNameValue = name
        End Sub

        Shared Function GetValueForEnum(value As Object) As String
            For Each i In value.GetType.GetFields
                If i.GetValue(value).Equals(value) Then
                    For Each i2 In i.GetCustomAttributes(False)
                        Dim dna = TryCast(i2, DispNameAttribute) 'Test This
                        'If i2.GetType Is GetType(DispNameAttribute) Then
                        If dna IsNot Nothing Then
                            Return dna.DisplayName
                        End If
                    Next i2

                    Return i.Name
                End If
            Next i

            Return "Unknown Type"
        End Function

        Shared Function GetNamesForEnum(Of T)() As String()
            'Dim l As New List(Of String)
            Dim eGVA As Array = System.Enum.GetValues(GetType(T))
            Dim retA(eGVA.Length - 1) As String

            For Each i As T In eGVA
                Dim inc As Integer
                'l.Add(GetValueForEnum(i))
                retA(inc) = GetValueForEnum(i)
                inc += 1
            Next

            Return retA 'l.ToArray
        End Function

        Shared Function GetValue(attributes As Object()) As String
            For Each i In attributes
                Dim dnAtt As DispNameAttribute = TryCast(i, DispNameAttribute)
                If dnAtt IsNot Nothing Then
                    Return dnAtt.DisplayName
                End If
            Next

            Return Nothing
        End Function
    End Class

    Public Class DescriptionAttributeHelp
        Shared Function GetDescription(attributes As Object()) As String
            For Each i In attributes
                Dim dsAtt As DescriptionAttribute = TryCast(i, DescriptionAttribute)
                If dsAtt IsNot Nothing Then
                    Return dsAtt.Description
                End If
            Next

            Return Nothing
        End Function
    End Class

    Public Class EditorAttributeHelp
        Shared Function GetEditor(attributes As Object()) As Type
            For Each i In attributes
                Dim eAtt As EditorAttribute = TryCast(i, EditorAttribute)
                If eAtt IsNot Nothing Then
                    Return Type.GetType(eAtt.EditorTypeName)
                End If
            Next

            Return Nothing
        End Function
    End Class
End Namespace