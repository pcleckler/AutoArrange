Imports System.Runtime.InteropServices
Imports System.Text

Public Class frmSelectWindows

    Public Property Selected As New Dictionary(Of String, VirtualWindow)

    Public Class DesktopPanel
        Inherits Panel

        Public Property OriginalBounds As Rectangle = Nothing
        Public Property Windows As New List(Of VirtualWindow)

        Sub New()
            MyBase.New()

            Me.DoubleBuffered = True
        End Sub
    End Class

    Public Class VirtualWindow
        Inherits Button

        Public Property OriginalBounds As Rectangle = Nothing

        Public Property IsMaximized As Boolean = False

        Public Property IsSizable As Boolean = True

        Public Property ClassName As String = ""

        Public ReadOnly Property Key As String
            Get
                Return "text" & Me.Text & "_class" & Me.ClassName & "_isSizable" & Me.IsSizable
            End Get
        End Property

    End Class

    Private Sub VirtualWindow_Click(sender As Object, e As EventArgs)

        Dim vw As VirtualWindow = CType(sender, VirtualWindow)

        'MessageBox.Show("Title: " & vw.Text & vbNewLine & "Class: " & vw.ClassName, "Virtual Window")

        If selected.ContainsKey(vw.Key) Then
            selected.Remove(vw.Key)

            vw.BackColor = SystemColors.Control
            vw.ForeColor = SystemColors.ControlText

        Else
            selected.Add(vw.Key, vw)

            vw.BackColor = SystemColors.Highlight
            vw.ForeColor = SystemColors.HighlightText
        End If

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = "Select one or More Windows - " & My.Application.Info.Title

        lblText.Text = ""
        lblClass.Text = ""
        lblBounds.Text = ""

        timerRefresh.Interval = 10

        timerRefresh.Start()

    End Sub

    Private Sub timerRefresh_Tick(sender As Object, e As EventArgs) Handles timerRefresh.Tick

        Try
            timerRefresh.Interval = 1000

            Dim originalControls As New List(Of Control)

            For Each control As Control In panelDesktop.Controls
                originalControls.Add(control)
            Next

            Dim ratio As Double = (Me.ClientSize.Width - 0) / SystemInformation.VirtualScreen.Width

            Dim panels As New List(Of DesktopPanel)
            Dim maxDesktopHeight As Integer = 0
            Dim totalWidth As Integer = 0

            Dim boundAdjustX As Integer = 0
            Dim boundAdjustY As Integer = 0

            If SystemInformation.VirtualScreen.X < 0 Then
                boundAdjustX = Math.Abs(SystemInformation.VirtualScreen.X)
            End If

            If SystemInformation.VirtualScreen.Y < 0 Then
                boundAdjustY = Math.Abs(SystemInformation.VirtualScreen.Y)
            End If

            For Each s As Screen In Screen.AllScreens
                Dim panel As New DesktopPanel

                panel.BackgroundImageLayout = ImageLayout.Stretch
                panel.BackgroundImage = App.WallPaper
                panel.Tag = s.ToString()

                Dim sBoundsAdj = New Rectangle(s.Bounds.X + boundAdjustX, s.Bounds.Y + boundAdjustY, s.Bounds.Width, s.Bounds.Height)

                panel.Bounds = ScaleBounds(sBoundsAdj, ratio)
                panel.OriginalBounds = s.Bounds

                panelDesktop.Controls.Add(panel)

                panels.Add(panel)

                If maxDesktopHeight < panel.Bounds.Height Then
                    maxDesktopHeight = panel.Bounds.Height
                End If

                totalWidth += panel.Bounds.Width
            Next

            Dim wl As New WindowListing

            For Each wd As WindowDefinition In wl.ActiveWindows

                If wd.IsProgMan Then Continue For

                If wd.IntPtr.ToString() = Me.Owner.Handle.ToString() Then Continue For

                If wd.IntPtr.ToString() = Me.Handle.ToString() Then Continue For

                If wd.Placement.showCmd = ShowWindowCommands.Minimize Then
                    Continue For
                End If

                Dim wDisplayBounds As Rectangle = wd.Bounds

                wDisplayBounds.X += boundAdjustX
                wDisplayBounds.Y += boundAdjustY

                Dim vw As New VirtualWindow

                vw.AutoSize = False
                vw.BackColor = SystemColors.Control
                vw.ForeColor = SystemColors.ControlText
                vw.Bounds = ScaleBounds(wDisplayBounds, ratio)
                vw.OriginalBounds = wd.Bounds
                vw.Text = wd.Text.Replace("&", "&&")
                vw.ClassName = wd.Class
                vw.FlatStyle = FlatStyle.Flat
                'vw.BackgroundImage = windowImage
                'vw.BackgroundImageLayout = ImageLayout.Stretch
                vw.IsMaximized = (wd.Placement.showCmd = ShowWindowCommands.Maximize)
                vw.IsSizable = wd.IsSizable

                'ToolTipInfo.SetToolTip(vw, $"Text:   {vw.Text}{vbNewLine}Class:  {wd.Class}{vbNewLine}Bounds: {wd.Bounds.ToString()}")

                AddHandler vw.Click, AddressOf VirtualWindow_Click
                AddHandler vw.MouseMove, AddressOf VirtualWindow_MouseMove

                panelDesktop.Controls.Add(vw)

                If (Selected.ContainsKey(vw.Key)) Then
                    Selected.Item(vw.Key) = vw

                    vw.BackColor = SystemColors.Highlight
                    vw.ForeColor = SystemColors.HighlightText
                End If

                vw.SendToBack()
            Next

            For Each panel As DesktopPanel In panels
                panel.SendToBack()
            Next

            'panelDesktop.Width = (SystemInformation.VirtualScreen.Width * ratio) - panelDesktop.Left
            'panelDesktop.Height = (SystemInformation.VirtualScreen.Height * ratio) - panelDesktop.Top

            For Each control As Control In originalControls
                panelDesktop.Controls.Remove(control)
            Next

        Catch ex As Exception

            timerRefresh.Stop()

            Me.DialogResult = Windows.Forms.DialogResult.Cancel

            Me.Hide()

            App.DisplayException("Failed to refresh windows or desktop.", ex)

        End Try

    End Sub

    Public Function ScaleBounds(bounds As Rectangle, ratio As Double) As Rectangle

        Return New Rectangle(bounds.X * ratio, bounds.Y * ratio, bounds.Width * ratio, bounds.Height * ratio)

    End Function

    Private Sub frmSelectWindows_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Try

            timerRefresh.Stop()

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub

    Private Sub bOk_Click(sender As Object, e As EventArgs) Handles bOk.Click

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Hide()

    End Sub

    Private Sub bCancel_Click(sender As Object, e As EventArgs) Handles bCancel.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Hide()

    End Sub

    Private Sub VirtualWindow_MouseMove(sender As Object, e As MouseEventArgs) Handles bOk.MouseMove

        Try

            Dim vw As VirtualWindow = CType(sender, VirtualWindow)

            'ToolTipInfo.SetToolTip(vw, $"Text:   {vw.Text}{vbNewLine}Class:  {wd.Class}{vbNewLine}Bounds: {wd.Bounds.ToString()}")

            lblText.Text = IIf(vw.Text.Length > 0, vw.Text, "(None)")
            lblClass.Text = vw.ClassName
            lblBounds.Text = vw.OriginalBounds.ToString()

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub
End Class
