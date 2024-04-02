Public Class WindowListing

    Private mActiveWindows As New List(Of WindowDefinition)

    Public ReadOnly Property ActiveWindows As List(Of WindowDefinition)
        Get
            EnumerateActiveWindows()

            Return mActiveWindows
        End Get
    End Property

    Private Sub EnumerateActiveWindows()
        mActiveWindows.Clear()

        EnumWindows(AddressOf Enumerator, 0)
    End Sub

    Private Function Enumerator(ByVal hwnd As IntPtr, ByVal lParam As Integer) As Boolean
        If IsWindowVisible(hwnd) Then
            mActiveWindows.Add(New WindowDefinition(hwnd))
        End If

        Return True
    End Function

End Class