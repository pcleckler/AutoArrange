Public Class Imagekeys

    Public Shared Property Window As New Imagekeys("Window", My.Resources.Window)
    Public Shared Property Edit As New Imagekeys("Edit", My.Resources.edit)
    Public Shared Property Delete As New Imagekeys("Delete", My.Resources.Delete)
    Public Shared Property MonitorLayout As New Imagekeys("MonitorLayout", My.Resources.MonitorLayoutPNG)
    Public Shared Property Refresh As New Imagekeys("Refresh", My.Resources.Refresh)
    Public Shared Property Record As New Imagekeys("Record", My.Resources.Record)
    Public Shared Property Properties As New Imagekeys("Properties", My.Resources.properties)
    Public Shared Property [Stop] As New Imagekeys("Stop", My.Resources.StopExecution)
    Public Shared Property Play As New Imagekeys("Play", My.Resources.Play)
    Public Shared Property Pause As New Imagekeys("Pause", My.Resources.Pause)
    Public Shared Property WindowAdd As New Imagekeys("WindowAdd", My.Resources.WindowAdd)
    Public Shared Property Save As New Imagekeys("Save", My.Resources.Save)

    Public ReadOnly Property Key As String
        Get
            Return mKey
        End Get
    End Property

    Public ReadOnly Property Image As Image
        Get
            Return mImage
        End Get
    End Property

    Private mKey As String = ""
    Private mImage As Image = Nothing

    Private Sub New(key As String, image As Image)
        Me.mKey = key
        Me.mImage = image
    End Sub

    Public Shared Function Values() As Imagekeys()

        Dim list As New List(Of Imagekeys)

        For Each pInfo As Reflection.PropertyInfo In GetType(Imagekeys).GetProperties()
            If pInfo.PropertyType Is GetType(Imagekeys) Then
                Dim pValue As Imagekeys = pInfo.GetValue(GetType(Imagekeys), Nothing)

                list.Add(pValue)
            End If
        Next

        Return list.ToArray
    End Function

    Public Shared Function valueOf(value As String, defaultValue As Imagekeys) As Imagekeys

        For Each imageKey As Imagekeys In Imagekeys.Values
            If imageKey.Key.ToLower().Trim() = value.ToLower().Trim() Then
                Return imageKey
            End If
        Next

        Return defaultValue

    End Function

    Public Shared Operator =(a As Imagekeys, b As Imagekeys) As Boolean
        Return (a.Key.ToLower().Trim() = b.Key.ToLower().Trim())
    End Operator

    Public Shared Operator <>(a As Imagekeys, b As Imagekeys) As Boolean
        Return Not (a = b)
    End Operator

End Class
