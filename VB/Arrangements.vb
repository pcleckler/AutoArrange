Imports System.Text
Imports System.ComponentModel

Public Class Arrangements

    Public Shared ReadOnly Filename As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), My.Application.Info.Title.toFilename, BaseFilename)
    Private Const BaseFilename As String = "Layouts.config"
    Private mLayouts As New List(Of Layout)

    Public ReadOnly Property CurrentLayout As Layout
        Get

            Dim activeLayout As Layout = GetActiveLayout()

            If Me.ContainsLayout(activeLayout.Key) Then

                Return Me.GetLayout(activeLayout.Key)
            Else
                Me.mLayouts.Add(activeLayout)

                Return activeLayout
            End If

        End Get
    End Property

    Public Property Layouts As List(Of Layout)
        Get

            Return Me.mLayouts

        End Get
        Set(value As List(Of Layout))

            Me.mLayouts = value

            Dim x = Me.CurrentLayout ' Ensures active layout is present

        End Set
    End Property

    Public Shared Function Open() As Arrangements

        Dim fs As Microsoft.VisualBasic.MyServices.FileSystemProxy = My.Computer.FileSystem
        Dim config As Arrangements = Nothing

        If Not fs.FileExists(Filename) Then
            config = New Arrangements
        Else
            config = Utilities.ReadXML(Filename, GetType(Arrangements))
        End If

        Dim x = config.CurrentLayout

        config.mLayouts = (From l As Layout In config.mLayouts Order By l.ToString().ToLower() Ascending Select l).ToList()

        For Each layout As Arrangements.Layout In config.mLayouts
            layout.WindowEntries = (From e As WindowEntry In layout.WindowEntries Order By e.ToString().ToLower() Ascending Select e).ToList()
        Next

        Return config

    End Function

    Public Function ContainsLayout(layoutKey As String) As Boolean

        For Each layout As Layout In Me.mLayouts
            If layoutKey.ToLower().Trim() = layout.Key Then
                Return True
            End If
        Next

        Return False

    End Function

    Public Function GetLayout(layoutKey As String) As Layout

        For Each layout As Layout In Me.mLayouts
            If layoutKey.ToLower().Trim() = layout.Key Then
                Return layout
            End If
        Next

        Return Nothing

    End Function

    Public Function GetWindowEntry(windowText As String, windowClass As String) As WindowEntry

        Dim layout As Layout = CurrentLayout

        If layout IsNot Nothing Then

            If layout.ContainsWindow(windowText, windowClass) Then

                Return layout.GetWindowEntry(windowText, windowClass)

            End If

        End If

        Return Nothing

    End Function

    Public Function isCurrentLayout(layout As Layout) As Boolean

        Return (CurrentLayout.Key = layout.Key)

    End Function

    Public Function isDirty() As Boolean

        Dim fs As Microsoft.VisualBasic.MyServices.FileSystemProxy = My.Computer.FileSystem

        If Not fs.FileExists(Filename) Then
            Return True
        Else
            Dim fileXML As String = fs.ReadAllText(Filename)
            Dim objXML As String = Utilities.SerialzeToXML(Me, System.Text.Encoding.UTF8)

            Return (fileXML <> objXML)
        End If

        Return True

    End Function

    Public Sub Save()

        Utilities.CreateDirectoryStructure(IO.Path.GetDirectoryName(Filename))

        Utilities.WriteXML(Filename, Me)

    End Sub

    Private Function GetActiveLayout() As Layout

        Dim layout As New Layout

        layout.ScreenCount = Screen.AllScreens.Count

        layout.Dimensions = SystemInformation.VirtualScreen

        Return layout

    End Function

    Public Class Layout

        <DescriptionAttribute("The total size and position of the layout.")>
        <CategoryAttribute("Physical Attributes")>
        <[ReadOnly](True)>
        Public Property Dimensions As New Rectangle

        <DescriptionAttribute("The name displayed for the layout.")>
        <CategoryAttribute("Identification")>
        Public Property DisplayName As String = ""

        <DescriptionAttribute("The 'DNA' of this layout.  Used to determine which layout is currently active.")>
        <CategoryAttribute("Identification")>
        Public ReadOnly Property Key As String
            Get
                Dim sb As New StringBuilder

                sb.Append("Screen Count " & ScreenCount.ToString("#,##0"))

                sb.Append(", ")

                sb.Append("Dimensions " & Dimensions.ToString())

                Return sb.ToString().ToLower().Trim()
            End Get
        End Property

        <DescriptionAttribute("The number of screens that define the layout.")>
        <CategoryAttribute("Physical Attributes")>
        <[ReadOnly](True)>
        Public Property ScreenCount As Integer = 0

        <DescriptionAttribute("The windows tracked in this layout.")>
        <CategoryAttribute("Configuration")>
        Public Property WindowEntries As New List(Of WindowEntry)

        Public Function ContainsWindow(windowText As String, windowClass As String) As Boolean

            Dim entry As New WindowEntry

            entry.Text = windowText
            entry.Class = windowClass

            Return ContainsWindow(entry.Key)

        End Function

        Public Function ContainsWindow(we As WindowEntry) As Boolean

            For Each entry As WindowEntry In Me.WindowEntries
                If we.Key = entry.Key Then
                    Return True
                End If
            Next

            Return False

        End Function

        Public Function ContainsWindow(windowKey As String) As Boolean

            For Each entry As WindowEntry In Me.WindowEntries
                If windowKey.ToLower().Trim() = entry.Key Then
                    Return True
                End If
            Next

            Return False

        End Function

        Public Function CreateTreeNode() As TreeNode

            Dim tnLayout As New TreeNode(Me.ToString())

            tnLayout.ImageKey = Imagekeys.MonitorLayout.Key
            tnLayout.SelectedImageKey = tnLayout.ImageKey

            tnLayout.Tag = Me

            Return tnLayout

        End Function

        Public Function GetWindowEntry(windowText As String, windowClass As String) As WindowEntry

            Dim entry As New WindowEntry

            entry.Text = windowText
            entry.Class = windowClass

            Return GetWindowEntry(entry.Key)

        End Function

        Public Function GetWindowEntry(windowKey As String) As WindowEntry

            For Each entry As WindowEntry In Me.WindowEntries
                If windowKey.ToLower().Trim() = entry.Key Then
                    Return entry
                End If
            Next

            Return Nothing

        End Function

        Public Overrides Function ToString() As String

            Dim sb As New StringBuilder

            If Me.DisplayName.Trim().Length > 0 Then
                sb.Append(Me.DisplayName)
            Else
                sb.Append("Screen Count " & ScreenCount.ToString("#,##0"))

                sb.Append(", ")

                sb.Append("Dimensions " & Dimensions.ToString())
            End If

            Return sb.ToString()

        End Function

        Public Sub UpdateTreeNode(tnLayout As TreeNode)

            If tnLayout IsNot Nothing Then

                tnLayout.Text = Me.ToString()
                tnLayout.ImageKey = Imagekeys.MonitorLayout.Key
                tnLayout.SelectedImageKey = tnLayout.ImageKey

                tnLayout.Tag = Me

            End If

        End Sub

    End Class

End Class