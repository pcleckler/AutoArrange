Imports System.ComponentModel

Public Class WindowEntry
    Implements INotifyPropertyChanged

    '<DescriptionAttribute("Run these programs on certain conditions.")>
    '<CategoryAttribute("Trigger Actions")>
    'Public Property Actions As New List(Of ProgramSpecification)

    <DescriptionAttribute("The text or caption of the window.  Can contain the wild-card character '*' (asterisk) to match portions of the window caption.")>
    <CategoryAttribute("Identification")>
    Public Property Text As String = ""

    <DescriptionAttribute("The type or classification of the window.  Can contain the wild-card character '*' (asterisk) to match portions of the window type.")>
    <CategoryAttribute("Identification")>
    Public Property [Class] As String = ""

    <DescriptionAttribute("Whether or not the window is defined with a sizeable border.")>
    <CategoryAttribute("Identification")>
    Public Property IsSizable As WindowEntrySizableEnum = WindowEntrySizableEnum.DoesNotMatter

    <DescriptionAttribute("The size and position to be maintained for the window.")>
    <CategoryAttribute("Physical Attributes")>
    Public Property Bounds As New Rectangle

    <DescriptionAttribute("Whether or not the window is set to Maximized (full screen).  Window size and position is not captured while the window Maximized or minimized.")>
    <CategoryAttribute("Physical Attributes")>
    Public Property IsMaximized As Boolean = False

    <DescriptionAttribute("Whether or not the window should be centered on the current screen.")>
    <CategoryAttribute("Physical Attributes")>
    Public Property Center As Boolean = False

    <DescriptionAttribute("The 'DNA' of this window.  Used to locate the open windows.")>
    <CategoryAttribute("Identification")>
    Public ReadOnly Property Key As String
        Get
            Dim sb As New System.Text.StringBuilder

            sb.Append("Text " & Me.Text)

            sb.Append(", ")

            sb.Append("Class " & Me.Class)

            sb.Append(", ")

            sb.Append("IsSizable " & Me.IsSizable.ToString)

            Return sb.ToString().ToLower().Trim()
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub New(text As String, [class] As String)

        Me.Text = text
        Me.Class = [class]

    End Sub

    Public Sub New(text As String, [class] As String, bounds As Rectangle)

        Me.Text = text
        Me.Class = [class]
        Me.Bounds = bounds

    End Sub

    Public Function isMatch(text As String, [class] As String, isSizable As Boolean) As Boolean

        Dim reText As New System.Text.RegularExpressions.Regex("^" & Utilities.RegExEncode(Me.Text.Trim().Replace("*", Chr(1))).Replace(Chr(1), ".*") & "$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reClass As New System.Text.RegularExpressions.Regex("^" & Utilities.RegExEncode(Me.Class.Trim().Replace("*", Chr(1))).Replace(Chr(1), ".*") & "$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        If reText.IsMatch(text.Trim()) Then
            If reClass.IsMatch([class].Trim()) Then

                Select Case Me.IsSizable
                    Case WindowEntrySizableEnum.Yes
                        Return (isSizable = True)

                    Case WindowEntrySizableEnum.No
                        Return (isSizable = False)

                    Case WindowEntrySizableEnum.DoesNotMatter
                        Return True

                End Select

            End If
        End If

        Return False

    End Function

    Public Overrides Function ToString() As String

        Dim sb As New System.Text.StringBuilder

        sb.Append(Me.Text)

        If (sb.Length > 0) Then
            sb.Append(", ")
        End If

        sb.Append("Class '" & Me.Class & "'")

        If (sb.Length > 0) Then
            sb.Append(", ")
        End If

        sb.Append("IsSizable [" & Me.IsSizable.ToString() & "]")

        Return sb.ToString()

    End Function

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Function CreateTreeNode() As TreeNode

        Dim tnWindow As New TreeNode(Me.ToString())

        tnWindow.ImageKey = Imagekeys.Window.Key
        tnWindow.SelectedImageKey = tnWindow.ImageKey

        tnWindow.Tag = Me

        Return tnWindow

    End Function

    Public Sub UpdateTreeNode(tnWindow As TreeNode)

        If tnWindow IsNot Nothing Then

            tnWindow.Text = Me.ToString()
            tnWindow.ImageKey = Imagekeys.Window.Key
            tnWindow.SelectedImageKey = tnWindow.ImageKey

            tnWindow.Tag = Me

        End If

    End Sub

End Class