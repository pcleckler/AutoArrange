Public Class WindowDefinition

    Public Property IntPtr As IntPtr = IntPtr.Zero

    Public ReadOnly Property Text As String
        Get
            Return GetWindowText(Me.IntPtr)
        End Get
    End Property

    Public ReadOnly Property [Class] As String
        Get
            Static className As String = ""

            If className.Length < 1 Then
                className = GetWindowClass(Me.IntPtr)
            End If

            Return className
        End Get
    End Property

    Public ReadOnly Property IsProgMan As Boolean
        Get
            Return (Me.Text.ToLower().Trim() = "program manager")
        End Get
    End Property

    Public ReadOnly Property RECT As RECT
        Get
            Dim r As New RECT

            If GetWindowRect(Me.IntPtr, r) Then
                Return r
            End If
        End Get
    End Property

    Public Property Bounds As Rectangle
        Get

            Dim r As RECT = Me.RECT

            If (r.Width + r.Height) < 1 Then
                Return New Rectangle
            Else
                Return New Rectangle(r.X, r.Y, r.Width, r.Height)
            End If

        End Get
        Set(value As Rectangle)

            MoveWindow(Me.IntPtr, value.X, value.Y, value.Width, value.Height, True)

        End Set
    End Property

    Public ReadOnly Property Placement As WINDOWPLACEMENT
        Get
            Dim wp As New WINDOWPLACEMENT

            If GetWindowPlacement(Me.IntPtr, wp) Then
                Return wp
            End If

            Return wp
        End Get
    End Property

    Public Property WindowState As ShowWindowCommands
        Get
            Return Me.Placement.showCmd
        End Get
        Set(value As ShowWindowCommands)
            ShowWindow(Me.IntPtr, value)
        End Set
    End Property

    Public ReadOnly Property IsSizable As Boolean
        Get
            Dim style As IntPtr = GetWindowLongPtr(Me.IntPtr, WindowLongFlags.GWL_STYLE)

            If style.ToInt64() <> IntPtr.Zero.ToInt64() Then

                Dim ws As WindowStyles = Nothing

                If IntPtr.Size = 8 Then ' 64 bits
                    ws = CType(style.ToInt64(), WindowStyles)
                Else
                    ws = CType(style.ToInt32(), WindowStyles)
                End If

                Return (ws And WindowStyles.WS_SIZEFRAME) <> 0

            End If

            'Throw New Exception("Failed to get window style.", New System.ComponentModel.Win32Exception())

            Return False

        End Get
    End Property

    Public ReadOnly Property ScreenShot As Image
        Get

            Return (New ScreenCapture).CaptureWindow(Me.IntPtr)

        End Get
    End Property


    Public Sub New(intPtr As IntPtr)

        Me.IntPtr = intPtr

    End Sub

End Class