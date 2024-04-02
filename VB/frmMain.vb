Imports System.ComponentModel

Public Class frmMain

    Private Const CURRENT_LAYOUT_SUFFIX As String = " (Current Layout)"

    Private windowWorker As BackgroundWorker = Nothing
    Private positioningState As PositioningStateEnum = PositioningStateEnum.Stopped
    Private exitForm As Boolean = False
    Private copiedWindowEntries As List(Of WindowEntry) = Nothing

    Private windowHistory As New Dictionary(Of IntPtr, WindowHistoryEntry)

    Public Property PropertiesSelectedObject As Object
        Get
            Return propertyGrid.SelectedObject
        End Get
        Set(value As Object)

            'Try
            '    ' store the provider
            '    Dim provider As TypeDescriptionProvider = TypeDescriptor.AddAttributes(value, New Attribute() {New ReadOnlyAttribute(True)})
            'Catch ex As Exception
            '    ' Ignore
            'End Try

            propertyGrid.SelectedObject = value

            '' remove the provider
            'TypeDescriptor.RemoveProvider(provider, Me.SelectedObject)
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try

            Me.Text = My.Application.Info.Title
            Me.notifyIcon.Text = Me.Text

            copiedWindowEntries = New List(Of WindowEntry)

            ' Initialize ImageList
            For Each imageKey As Imagekeys In Imagekeys.Values
                Me.imageList.Images.Add(imageKey.Key, imageKey.Image)
            Next

            Me.tvEditor.ImageList = Me.imageList

            ' Restore GUI Positions
            RestoreWindowPosition()

            ' Initialize AutoArrange Window/Layout configuration
            App.arrangements = Arrangements.Open()

            bFilter.Checked = (Not My.Settings.HideEmptyLayouts)

            RefreshLayouts()

            ' Begin Positioning
            ArrangeWindows(IIf(My.Settings.StartWindowPositioning, PositioningStateEnum.Started, PositioningStateEnum.Stopped))

            ' Minimize to SysTray
            Me.Hide()

        Catch ex As Exception

            App.DisplayException("Failed to initialize application.", ex)

            Application.Exit()

        End Try

    End Sub

    Private Sub RefreshLayouts()

        Try

            tvEditor.Nodes.Clear()

            For Each layout As Arrangements.Layout In App.arrangements.Layouts

                ' Hide Empty Layouts
                If (My.Settings.HideEmptyLayouts) And (Not App.arrangements.isCurrentLayout(layout)) And (layout.WindowEntries.Count < 1) Then
                    Continue For
                End If

                Dim tnLayout As TreeNode = layout.createTreeNode()

                tvEditor.Nodes.Add(tnLayout)

                For Each entry As WindowEntry In layout.WindowEntries
                    tnLayout.Nodes.Add(entry.createTreeNode())
                Next

                If App.arrangements.isCurrentLayout(layout) Then
                    tnLayout.Expand()
                    tnLayout.Text = layout.ToString() & CURRENT_LAYOUT_SUFFIX
                End If

            Next

        Catch ex As Exception
            App.DisplayException("Failed to refresh layouts.", ex)
        End Try

    End Sub

    Private Sub DisableOperations()

        For Each control As ToolStripItem In {bRenameLayout, bAddWindows, bRemove, tsmiRename, tsmiAddWindows, tsmiCopy, tsmiPaste, tsmiRemove}
            control.Enabled = False
        Next

    End Sub

    Private Function Save()

        Try

            App.arrangements.Save()

            Return True

        Catch ex As Exception
            App.DisplayException("Failed to save layout data.", ex)

            Return False

        End Try

    End Function

    Private Sub ArrangeWindows(newState As PositioningStateEnum)

        Try

            Me.positioningState = newState

            Select Case Me.positioningState

                Case PositioningStateEnum.Stopped

                    bStart.Text = "Start Window Positioning"
                    bStart.Image = Imagekeys.Play.Image
                    bStart.Enabled = True
                    bStop.Enabled = False
                    bRecord.Enabled = True

                    My.Settings.StartWindowPositioning = False

                Case PositioningStateEnum.Started

                    bStart.Text = "Pause Window Positioning"
                    bStart.Image = Imagekeys.Pause.Image
                    bStart.Enabled = True
                    bStop.Enabled = True
                    bRecord.Enabled = False

                    My.Settings.StartWindowPositioning = True

                    If Me.windowWorker Is Nothing Then
                        Me.windowWorker = Utilities.StartBackgroundWorker(True, AddressOf windowWorker_DoWork, Me, AddressOf windowWorker_RunWorkerCompleted)
                    End If

                Case PositioningStateEnum.Paused

                    bStart.Text = "Resume Window Positioning"
                    bStart.Image = Imagekeys.Play.Image
                    bStart.Enabled = True
                    bStop.Enabled = True
                    bRecord.Enabled = True

            End Select

            ' Mirror settings on SysTray icon
            bStart2.Text = bStart.Text
            bStart2.Image = bStart.Image
            bStart2.Enabled = bStart.Enabled
            bStop2.Enabled = bStop.Enabled
            bRecord2.Enabled = bRecord.Enabled

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub

    Delegate Sub LayoutChangedDelegate(oldLayout As Arrangements.Layout, newLayout As Arrangements.Layout)

    Private Sub LayoutChanged(oldLayout As Arrangements.Layout, newLayout As Arrangements.Layout)

        Try

            bRefresh_Click(bRefresh, New EventArgs)

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub

    Private Sub windowWorker_DoWork(sender As Object, e As DoWorkEventArgs)

        Dim form As frmMain = CType(e.Argument, frmMain)

        Static lastLayout As Arrangements.Layout = Nothing
        Static autoPosition As Dictionary(Of String, Dictionary(Of IntPtr, WindowHistoryEntry)) = Nothing

        If autoPosition Is Nothing Then
            autoPosition = New Dictionary(Of String, Dictionary(Of IntPtr, WindowHistoryEntry))(StringComparer.OrdinalIgnoreCase)
        End If

        Select Case form.positioningState

            Case PositioningStateEnum.Started

                ' Get current layout
                Dim layout As Arrangements.Layout = App.arrangements.CurrentLayout
                Dim layoutHasChanged As Boolean = False

                If Not autoPosition.ContainsKey(layout.Key) Then
                    autoPosition.Add(layout.Key, New Dictionary(Of IntPtr, WindowHistoryEntry))
                End If

                ' Handle Layout changes
                If lastLayout IsNot Nothing AndAlso (lastLayout.Key <> layout.Key) Then
                    layoutHasChanged = True
                End If

                ' Locate windows
                Dim wl As New WindowListing
                Dim targetWindows As New Stack(Of WindowDefinition)
                Dim foundWindows As New Dictionary(Of IntPtr, WindowDefinition)

                For Each wd As WindowDefinition In wl.ActiveWindows

                    Dim isConfigured As Boolean = False

                    For Each entry As WindowEntry In layout.WindowEntries

                        If entry.isMatch(wd.Text, wd.Class, wd.IsSizable) Then

                            isConfigured = True

                            targetWindows.Push(wd)

                            If Not foundWindows.ContainsKey(wd.IntPtr) Then
                                foundWindows.Add(wd.IntPtr, wd)
                            End If

                            Exit For

                        End If
                    Next

                    If Not isConfigured Then

                        Dim whe As WindowHistoryEntry = Nothing

                        If autoPosition.Item(layout.Key).ContainsKey(wd.IntPtr) Then

                            whe = autoPosition.Item(layout.Key).Item(wd.IntPtr)

                            If layoutHasChanged Then
                                wd.Bounds = whe.Bounds
                                wd.WindowState = IIf(whe.isMaximized, ShowWindowCommands.Maximize, wd.WindowState)
                            Else
                                whe.Bounds = wd.Bounds
                                whe.isMaximized = (wd.WindowState = ShowWindowCommands.Maximize)
                            End If

                        Else

                            whe = New WindowHistoryEntry(wd.IntPtr, wd.Text, wd.Class, wd.IsSizable) With {
                                .Bounds = wd.Bounds,
                                .IsMaximized = (wd.WindowState = ShowWindowCommands.Maximize)
                            }

                            autoPosition.Item(layout.Key).Add(wd.IntPtr, whe)

                        End If

                    End If

                Next

                ' Reposition
                Do While targetWindows.Count > 0

                    Dim wd As WindowDefinition = targetWindows.Pop

                    For Each entry As WindowEntry In layout.WindowEntries

                        If entry.isMatch(wd.Text, wd.Class, wd.IsSizable) Then

                            If Not windowHistory.ContainsKey(wd.IntPtr) Then

                                windowHistory.Add(wd.IntPtr, New WindowHistoryEntry(wd.IntPtr, wd.Text, wd.Class, wd.IsSizable))

                                'For Each progSpec As ProgramSpecification In entry.Actions

                                '    If Not progSpec.Enabled Then Continue For

                                '    If progSpec.TriggerType = TriggerTypeEnum.OnOpen Then

                                '        Process.Start(progSpec.GetStartupInfo(wd.IntPtr, wd.Class, wd.Text))

                                '    End If

                                'Next

                            End If

                            Select Case wd.WindowState

                                Case ShowWindowCommands.Normal

                                    If entry.IsMaximized Then

                                        wd.Bounds = entry.Bounds

                                        wd.WindowState = ShowWindowCommands.ShowMaximized
                                    Else
                                        wd.Bounds = entry.Bounds
                                    End If

                                Case ShowWindowCommands.ShowMaximized

                                    If Not entry.IsMaximized Then

                                        wd.WindowState = ShowWindowCommands.Normal

                                        wd.Bounds = entry.Bounds

                                    End If

                                Case Else

                                    wd.Bounds = entry.Bounds

                            End Select

                            If entry.Center Then

                                For Each monitor As Screen In Screen.AllScreens

                                    If monitor.Bounds.Contains(entry.Bounds) Then

                                        Dim newX As Integer = (monitor.Bounds.Width / 2) - (entry.Bounds.Width / 2)
                                        Dim newY As Integer = (monitor.Bounds.Height / 2) - (entry.Bounds.Height / 2)

                                        wd.Bounds = New Rectangle(newX, newY, entry.Bounds.Width, entry.Bounds.Height)

                                        Exit For

                                    End If

                                Next

                            End If

                            Exit For

                        End If
                    Next

                Loop

                ' Clean Up
                Dim removeWindows As New List(Of IntPtr)

                For Each wheHandle As IntPtr In windowHistory.Keys

                    Dim whe As WindowHistoryEntry = windowHistory.Item(wheHandle)

                    If Not foundWindows.ContainsKey(wheHandle) Then

                        For Each entry As WindowEntry In layout.WindowEntries

                            If entry.isMatch(whe.Text, whe.Class, whe.isSizable) Then

                                'For Each progSpec As ProgramSpecification In entry.Actions

                                '    If Not progSpec.Enabled Then Continue For

                                '    If progSpec.TriggerType = TriggerTypeEnum.OnClose Then

                                '        Process.Start(progSpec.GetStartupInfo(whe.Handle, whe.Class, whe.Text))

                                '    End If

                                'Next

                            End If

                            Exit For

                        Next

                        removeWindows.Add(wheHandle)

                    End If
                Next

                For Each wheHandle As IntPtr In removeWindows
                    windowHistory.Remove(wheHandle)
                Next

                If layoutHasChanged Then

                    ' Inform frmMain
                    form.Invoke(New LayoutChangedDelegate(AddressOf LayoutChanged), lastLayout, layout)

                End If

                lastLayout = layout

            Case PositioningStateEnum.Stopped,
                 PositioningStateEnum.ShuttingDown,
                 PositioningStateEnum.Paused

                ' Do Nothing

        End Select

        Threading.Thread.Sleep(My.Settings.PollingInterval * 1000)

    End Sub

    Private Sub windowWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)

        Me.windowWorker = Nothing

        Select Case Me.positioningState

            Case PositioningStateEnum.ShuttingDown

                ArrangeWindows(Me.positioningState)

            Case Else

                ArrangeWindows(Me.positioningState)

        End Select

    End Sub

    Private Sub RestoreWindowPosition()

        ' Restore window position
        If My.Settings.WindowState.Trim().Length > 0 Then

            Try

                Me.WindowState = [Enum].Parse(GetType(FormWindowState), My.Settings.WindowState)

            Catch ex As Exception
                ' Ignore
            End Try

        End If

        If My.Settings.WindowBounds.Trim().Length > 0 Then

            Try

                Dim myLayouts As Arrangements = Utilities.DeserialzeFromXML(My.Settings.WindowBounds, GetType(Arrangements), System.Text.Encoding.UTF8)
                Dim myEntry As WindowEntry = myLayouts.GetWindowEntry(Me.Text, Win32.GetWindowClass(Me.Handle))

                If myEntry IsNot Nothing Then

                    Me.Bounds = myEntry.Bounds

                End If

            Catch ex As Exception

                ' Ignore

            End Try

        End If

        split.SplitterDistance = My.Settings.PropertiesWidth

    End Sub

    Private Sub SaveWindowPosition()

        Try

            My.Settings.WindowState = Me.WindowState.ToString()

            Select Case Me.WindowState

                Case FormWindowState.Normal

                    Dim myLayouts As New Arrangements

                    myLayouts.CurrentLayout.WindowEntries.Add(New WindowEntry(Me.Text, Win32.GetWindowClass(Me.Handle), Me.Bounds))

                    My.Settings.WindowBounds = Utilities.SerialzeToXML(myLayouts, System.Text.Encoding.UTF8)

            End Select

            My.Settings.PropertiesWidth = split.SplitterDistance

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub

    Private Sub bRefresh_Click(sender As Object, e As EventArgs) Handles bRefresh.Click

        DisableOperations()

        RefreshLayouts()

    End Sub

    Private Sub tvEditor_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvEditor.AfterSelect

        Try

            DisableOperations()

            Try
                PropertiesSelectedObject = tvEditor.SelectedNode.Tag

            Catch ex As Exception
                ' Ignore
            End Try

            Select Case e.Node.Tag.GetType()

                Case GetType(Arrangements.Layout)

                    bRenameLayout.Enabled = True
                    tsmiRename.Enabled = True
                    bAddWindows.Enabled = True
                    tsmiAddWindows.Enabled = True
                    bRemove.Enabled = True
                    tsmiRemove.Enabled = True

                    bRemove.Text = "Remove Layout"
                    tsmiRemove.Text = bRemove.Text

                    tsmiPaste.Enabled = (copiedWindowEntries.Count > 0)
                    tsmiCopy.Enabled = (CType(e.Node.Tag, Arrangements.Layout).WindowEntries.Count > 0)

                    tsmiCopy.Text = "Copy All Windows"

                Case GetType(WindowEntry)

                    bRenameLayout.Enabled = True
                    tsmiRename.Enabled = True
                    bAddWindows.Enabled = True
                    tsmiAddWindows.Enabled = True
                    bRemove.Enabled = True
                    tsmiRemove.Enabled = True

                    bRemove.Text = "Remove Window"
                    tsmiRemove.Text = bRemove.Text

                    tsmiCopy.Enabled = True

                    tsmiCopy.Text = "Copy Window"

            End Select

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub

    Private Sub bRenameLayout_Click(sender As Object, e As EventArgs) Handles bRenameLayout.Click, tsmiRename.Click

        Try

            Dim layout As Arrangements.Layout = Nothing
            Dim layoutNode As TreeNode = Nothing

            Select Case tvEditor.SelectedNode.Tag.GetType()

                Case GetType(Arrangements.Layout)

                    layoutNode = tvEditor.SelectedNode

                Case GetType(WindowEntry)

                    layoutNode = tvEditor.SelectedNode.Parent

            End Select

            layout = CType(layoutNode.Tag, Arrangements.Layout)

            Dim newName As String = InputBox("Type the new name of the layout below.", My.Application.Info.Title, layout.DisplayName)

            If newName.Trim().Length > 0 Then

                layout.DisplayName = newName.Trim()
            Else

                If Confirm("Clear the display name of the layout?") Then
                    layout.DisplayName = ""
                End If
            End If

            If App.arrangements.isCurrentLayout(layout) Then
                layoutNode.Text = layout.ToString() & CURRENT_LAYOUT_SUFFIX
            Else
                layoutNode.Text = layout.ToString()
            End If

            Save()

        Catch ex As Exception

            App.DisplayException("Failed to rename layout.", ex)

        End Try

    End Sub

    Private Sub bAddWindows_Click(sender As Object, e As EventArgs) Handles bAddWindows.Click, tsmiAddWindows.Click

        Try
            Dim layout As Arrangements.Layout = Nothing
            Dim layoutNode As TreeNode = Nothing

            Select Case tvEditor.SelectedNode.Tag.GetType()

                Case GetType(Arrangements.Layout)

                    layoutNode = tvEditor.SelectedNode

                Case GetType(WindowEntry)

                    layoutNode = tvEditor.SelectedNode.Parent

            End Select

            layout = CType(layoutNode.Tag, Arrangements.Layout)

            Dim sw As New frmSelectWindows()

            Select Case sw.ShowDialog(Me)

                Case Windows.Forms.DialogResult.OK

                    Dim updated As Boolean = False

                    For Each vwKey As String In sw.Selected.Keys

                        Dim vw As frmSelectWindows.VirtualWindow = sw.Selected.Item(vwKey)

                        Dim entry As New WindowEntry With {
                            .Text = vw.Text,
                            .Class = vw.ClassName,
                            .IsSizable = IIf(vw.IsSizable, WindowEntrySizableEnum.Yes, WindowEntrySizableEnum.No)
                        }

                        If Not vw.isMaximized Then
                            entry.Bounds = vw.OriginalBounds
                        End If

                        entry.isMaximized = vw.isMaximized

                        If layout.ContainsWindow(entry) Then

                            Dim existingEntry As WindowEntry = layout.GetWindowEntry(entry.Key)

                            existingEntry.Bounds = vw.OriginalBounds

                            updated = True

                        Else

                            layout.WindowEntries.Add(entry)

                            layoutNode.Nodes.Add(entry.createTreeNode())

                            layoutNode.Expand()

                        End If

                    Next

                    Save()

                    If updated Then
                        Alert("One or more windows were previously selected.  These windows' positions have been recorded.")
                    End If

                Case Else
                    Exit Sub

            End Select

            Try
                sw.Close()
            Catch ex As Exception
                ' Ignore
            End Try

        Catch ex As Exception
            App.DisplayException("Failed to select windows.", ex)
        End Try

    End Sub

    Private Sub bRemove_Click(sender As Object, e As EventArgs) Handles bRemove.Click, tsmiRemove.Click

        Try

            If Not App.Confirm("Are you sure you want to remove '" & tvEditor.SelectedNode.Text & "'?") Then
                Exit Sub
            End If

            DisableOperations()

            Select Case tvEditor.SelectedNode.Tag.GetType()

                Case GetType(Arrangements.Layout)

                    Dim layout As Arrangements.Layout = CType(tvEditor.SelectedNode.Tag, Arrangements.Layout)

                    tvEditor.Nodes.Remove(tvEditor.SelectedNode)

                Case GetType(WindowEntry)

                    Dim layout As Arrangements.Layout = CType(tvEditor.SelectedNode.Parent.Tag, Arrangements.Layout)

                    Dim entry As WindowEntry = CType(tvEditor.SelectedNode.Tag, WindowEntry)

                    layout.WindowEntries.Remove(entry)

                    tvEditor.SelectedNode.Parent.Nodes.Remove(tvEditor.SelectedNode)

            End Select

            Save()

        Catch ex As Exception
            App.DisplayException("Failed to remove item.", ex)
        End Try

    End Sub

    Private Sub bRecord_Click(sender As Object, e As EventArgs) Handles bRecord.Click, bRecord2.Click

        Try

            Dim layout As Arrangements.Layout = App.arrangements.CurrentLayout

            Dim wl As New WindowListing
            Dim targetEntries As New Stack(Of WindowEntry)

            For Each wd As WindowDefinition In wl.ActiveWindows

                For Each entry As WindowEntry In layout.WindowEntries

                    If entry.isMatch(wd.Text, wd.Class, wd.IsSizable) Then

                        targetEntries.Push(entry)
                        Exit For

                    End If
                Next
            Next

            Do While targetEntries.Count > 0

                Dim entry As WindowEntry = targetEntries.Pop

                For Each wd As WindowDefinition In wl.ActiveWindows

                    If entry.isMatch(wd.Text, wd.Class, wd.IsSizable) Then

                        If wd.WindowState = ShowWindowCommands.Normal Then
                            entry.Bounds = wd.Bounds
                        End If

                        entry.IsMaximized = (wd.WindowState = ShowWindowCommands.Maximize)

                        Exit For
                    End If
                Next
            Loop

            Save()

            RefreshLayouts()

            Alert("Available window positions recorded.")

        Catch ex As Exception
            App.DisplayException("Failed to record current positions.", ex)
        End Try

    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        SaveWindowPosition()

        If Not Save() Then
            e.Cancel = True
            Exit Sub
        End If

        If e.CloseReason = CloseReason.UserClosing Then

            'If App.arrangements.isDirty() Then

            '    Select Case App.YesNoCancel("Do you want to save your changes?", True)

            '        Case DialogResult.Yes

            '            If Not Save() Then
            '                e.Cancel = True
            '                Exit Sub
            '            End If

            '        Case DialogResult.No
            '            ' Do Nothing

            '        Case DialogResult.Cancel

            '            ' Cancel
            '            e.Cancel = True
            '            Exit Sub

            '    End Select

            'End If

            If Not exitForm Then

                Me.Hide()
                e.Cancel = True
                Exit Sub
            End If

        End If

        Me.positioningState = PositioningStateEnum.ShuttingDown

        Me.notifyIcon.Visible = False

    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        RestoreWindowPosition()

        ' Minimize to SysTray
        Me.Hide()

    End Sub

    Private Sub bStart_Click(sender As Object, e As EventArgs) Handles bStart.Click, bStart2.Click

        ArrangeWindows(IIf(positioningState = PositioningStateEnum.Started, PositioningStateEnum.Paused, PositioningStateEnum.Started))

    End Sub

    Private Sub bStop_Click(sender As Object, e As EventArgs) Handles bStop.Click, bStop2.Click

        ArrangeWindows(PositioningStateEnum.Stopped)

    End Sub

    Private Sub notifyIcon_DoubleClick(sender As Object, e As EventArgs) Handles notifyIcon.DoubleClick, bManage.Click

        Try

            Me.Show()

        Catch ex As Exception

            App.DisplayException("Failed to show Management console.", ex)

        End Try

    End Sub

    Private Sub bExit_Click(sender As Object, e As EventArgs) Handles bExit.Click

        Me.exitForm = True

        Me.Close()

    End Sub

    Private Sub timerPostOpen_Tick(sender As Object, e As EventArgs) Handles timerPostOpen.Tick

        Try

            timerPostOpen.Stop()

            ' Minimize to SysTray
            Me.Hide()

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub

    Private Sub bSave_Click(sender As Object, e As EventArgs) Handles bSave.Click

        Try
            Save()
        Catch ex As Exception
            ' Ignore (problems reported in Save())
        End Try

    End Sub

    Private Sub bFilter_Click(sender As Object, e As EventArgs) Handles bFilter.Click

        Try
            My.Settings.HideEmptyLayouts = Not My.Settings.HideEmptyLayouts
            My.Settings.Save()

            bFilter.Checked = (Not My.Settings.HideEmptyLayouts)

            DisableOperations()

            RefreshLayouts()

        Catch ex As Exception
            App.DisplayException("Failed to display item properties.", ex)
        End Try

    End Sub

    Private Sub tsmiCopy_Click(sender As Object, e As EventArgs) Handles tsmiCopy.Click

        Try
            copiedWindowEntries.Clear()

            Select Case tvEditor.SelectedNode.Tag.GetType()

                Case GetType(Arrangements.Layout)

                    For Each entry As WindowEntry In CType(tvEditor.SelectedNode.Tag, Arrangements.Layout).WindowEntries
                        copiedWindowEntries.Add(entry)
                    Next


                Case GetType(WindowEntry)

                    copiedWindowEntries.Add(CType(tvEditor.SelectedNode.Tag, WindowEntry))

            End Select


        Catch ex As Exception
            App.DisplayException("Failed to copy window entries.", ex)
        End Try

    End Sub

    Private Sub tsmiPaste_Click(sender As Object, e As EventArgs) Handles tsmiPaste.Click

        Try

            Dim pasteLayout As Arrangements.Layout = Nothing
            Dim pasteLayoutNode As TreeNode = Nothing

            Select Case tvEditor.SelectedNode.Tag.GetType()

                Case GetType(Arrangements.Layout)

                    pasteLayoutNode = tvEditor.SelectedNode

                Case GetType(WindowEntry)

                    pasteLayoutNode = tvEditor.SelectedNode.Parent

            End Select

            pasteLayout = CType(pasteLayoutNode.Tag, Arrangements.Layout)


            Dim allowUpdates As Boolean = False

            For Each copiedEntry As WindowEntry In copiedWindowEntries

                If pasteLayout.ContainsWindow(copiedEntry) Then

                    Select Case MessageBox.Show("One or more windows already exist in the layout.  Update these windows' bounds from the copied windows?", My.Application.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)

                        Case DialogResult.Cancel
                            Exit Sub

                        Case DialogResult.No
                            allowUpdates = False

                        Case DialogResult.Yes
                            allowUpdates = True

                    End Select

                    Exit For
                End If

            Next

            For Each copiedEntry As WindowEntry In copiedWindowEntries

                If pasteLayout.ContainsWindow(copiedEntry) Then

                    If allowUpdates Then

                        Dim existingEntry As WindowEntry = pasteLayout.GetWindowEntry(copiedEntry.Key)

                        existingEntry.Bounds = New Rectangle(copiedEntry.Bounds.Location, copiedEntry.Bounds.Size)

                    End If

                Else

                    Dim entry As New WindowEntry With {
                        .Text = copiedEntry.Text,
                        .Class = copiedEntry.Class
                    }

                    If Not copiedEntry.isMaximized Then
                        entry.Bounds = New Rectangle(copiedEntry.Bounds.Location, copiedEntry.Bounds.Size)
                    End If

                    entry.isMaximized = copiedEntry.isMaximized

                    pasteLayout.WindowEntries.Add(entry)

                    pasteLayoutNode.Nodes.Add(entry.createTreeNode())

                    pasteLayoutNode.Expand()

                End If

            Next

            Save()

        Catch ex As Exception
            App.DisplayException("Failed to paste windows.", ex)
        End Try

    End Sub

    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Try

            bRefresh_Click(bRefresh, New EventArgs)

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub

    Private Sub propertyGrid_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles propertyGrid.PropertyValueChanged

        Try

            If tvEditor.SelectedNode Is Nothing Then Exit Sub

            Dim node As TreeNode = tvEditor.SelectedNode

            If node.Tag Is Nothing Then Exit Sub

            Select Case node.Tag.GetType()

                Case GetType(Arrangements.Layout)

                    Dim layout As Arrangements.Layout = node.Tag

                    layout.UpdateTreeNode(node)

                Case GetType(WindowEntry)

                    Dim entry As WindowEntry = node.Tag

                    entry.UpdateTreeNode(node)

            End Select

        Catch ex As Exception
            ' Ignore
        End Try

    End Sub
End Class