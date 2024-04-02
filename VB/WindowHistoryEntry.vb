Public Class WindowHistoryEntry

    Public Property Handle As IntPtr = IntPtr.Zero
    Public Property Text As String = ""
    Public Property [Class] As String = ""
    Public Property Bounds As Rectangle = Nothing
    Public Property IsMaximized As Boolean = False
    Public Property IsSizable As Boolean = True

    Public Sub New(Handle As IntPtr, Text As String, [Class] As String, isSizable As Boolean)

        Me.Handle = Handle
        Me.Text = Text
        Me.Class = [Class]
        Me.IsSizable = isSizable

    End Sub

End Class
