<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.toolbar = New System.Windows.Forms.ToolStrip()
        Me.bSave = New System.Windows.Forms.ToolStripButton()
        Me.bRenameLayout = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.bAddWindows = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.bRemove = New System.Windows.Forms.ToolStripButton()
        Me.bFilter = New System.Windows.Forms.ToolStripButton()
        Me.bRefresh = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.bRecord = New System.Windows.Forms.ToolStripButton()
        Me.bStart = New System.Windows.Forms.ToolStripButton()
        Me.bStop = New System.Windows.Forms.ToolStripButton()
        Me.tvEditor = New System.Windows.Forms.TreeView()
        Me.cmsLayouts = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmiAddWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.imageList = New System.Windows.Forms.ImageList(Me.components)
        Me.split = New System.Windows.Forms.SplitContainer()
        Me.propertyGrid = New System.Windows.Forms.PropertyGrid()
        Me.notifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.cmsNotifyIcon = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.bManage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.bRecord2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.bStart2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.bStop2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.bExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.timerPostOpen = New System.Windows.Forms.Timer(Me.components)
        Me.toolbar.SuspendLayout()
        Me.cmsLayouts.SuspendLayout()
        CType(Me.split, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.split.Panel1.SuspendLayout()
        Me.split.Panel2.SuspendLayout()
        Me.split.SuspendLayout()
        Me.cmsNotifyIcon.SuspendLayout()
        Me.SuspendLayout()
        '
        'toolbar
        '
        Me.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.toolbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bSave, Me.bRenameLayout, Me.ToolStripSeparator1, Me.bAddWindows, Me.ToolStripSeparator2, Me.bRemove, Me.bFilter, Me.bRefresh, Me.ToolStripSeparator3, Me.bRecord, Me.bStart, Me.bStop})
        Me.toolbar.Location = New System.Drawing.Point(0, 0)
        Me.toolbar.Name = "toolbar"
        Me.toolbar.Padding = New System.Windows.Forms.Padding(2, 0, 1, 0)
        Me.toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.toolbar.Size = New System.Drawing.Size(702, 25)
        Me.toolbar.TabIndex = 0
        Me.toolbar.Text = "ToolStrip1"
        '
        'bSave
        '
        Me.bSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bSave.Image = Global.AutoArrange.My.Resources.Resources.Save
        Me.bSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bSave.Name = "bSave"
        Me.bSave.Size = New System.Drawing.Size(23, 22)
        Me.bSave.Text = "Save Configuration Changes"
        '
        'bRenameLayout
        '
        Me.bRenameLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bRenameLayout.Enabled = False
        Me.bRenameLayout.Image = Global.AutoArrange.My.Resources.Resources.Rename
        Me.bRenameLayout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bRenameLayout.Name = "bRenameLayout"
        Me.bRenameLayout.Size = New System.Drawing.Size(23, 22)
        Me.bRenameLayout.Text = "Rename Layout..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'bAddWindows
        '
        Me.bAddWindows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bAddWindows.Enabled = False
        Me.bAddWindows.Image = Global.AutoArrange.My.Resources.Resources.WindowAdd
        Me.bAddWindows.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bAddWindows.Name = "bAddWindows"
        Me.bAddWindows.Size = New System.Drawing.Size(23, 22)
        Me.bAddWindows.Text = "Add Windows..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'bRemove
        '
        Me.bRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bRemove.Enabled = False
        Me.bRemove.Image = Global.AutoArrange.My.Resources.Resources.Delete
        Me.bRemove.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bRemove.Name = "bRemove"
        Me.bRemove.Size = New System.Drawing.Size(23, 22)
        Me.bRemove.Text = "Remove"
        '
        'bFilter
        '
        Me.bFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bFilter.Image = Global.AutoArrange.My.Resources.Resources.MonitorLayoutPNG
        Me.bFilter.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bFilter.Name = "bFilter"
        Me.bFilter.Size = New System.Drawing.Size(23, 22)
        Me.bFilter.Text = "Show Empty Layouts"
        '
        'bRefresh
        '
        Me.bRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bRefresh.Image = Global.AutoArrange.My.Resources.Resources.Refresh
        Me.bRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bRefresh.Name = "bRefresh"
        Me.bRefresh.Size = New System.Drawing.Size(23, 22)
        Me.bRefresh.Text = "Refresh"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'bRecord
        '
        Me.bRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bRecord.Enabled = False
        Me.bRecord.Image = Global.AutoArrange.My.Resources.Resources.Record
        Me.bRecord.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bRecord.Name = "bRecord"
        Me.bRecord.Size = New System.Drawing.Size(23, 22)
        Me.bRecord.Text = "Record Current Positions"
        '
        'bStart
        '
        Me.bStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bStart.Image = Global.AutoArrange.My.Resources.Resources.Play
        Me.bStart.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bStart.Name = "bStart"
        Me.bStart.Size = New System.Drawing.Size(23, 22)
        Me.bStart.Text = "Start Window Positioning"
        '
        'bStop
        '
        Me.bStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bStop.Enabled = False
        Me.bStop.Image = Global.AutoArrange.My.Resources.Resources.StopExecution
        Me.bStop.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bStop.Name = "bStop"
        Me.bStop.Size = New System.Drawing.Size(23, 22)
        Me.bStop.Text = "Stop Window Positioning"
        '
        'tvEditor
        '
        Me.tvEditor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvEditor.ContextMenuStrip = Me.cmsLayouts
        Me.tvEditor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvEditor.FullRowSelect = True
        Me.tvEditor.HideSelection = False
        Me.tvEditor.ImageIndex = 0
        Me.tvEditor.ImageList = Me.imageList
        Me.tvEditor.Location = New System.Drawing.Point(3, 1)
        Me.tvEditor.Name = "tvEditor"
        Me.tvEditor.SelectedImageIndex = 0
        Me.tvEditor.ShowLines = False
        Me.tvEditor.Size = New System.Drawing.Size(394, 419)
        Me.tvEditor.TabIndex = 1
        '
        'cmsLayouts
        '
        Me.cmsLayouts.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiAddWindows, Me.tsmiRename, Me.ToolStripMenuItem3, Me.tsmiCopy, Me.tsmiPaste, Me.tsmiRemove})
        Me.cmsLayouts.Name = "cmsLayouts"
        Me.cmsLayouts.Size = New System.Drawing.Size(158, 120)
        '
        'tsmiAddWindows
        '
        Me.tsmiAddWindows.Image = Global.AutoArrange.My.Resources.Resources.WindowAdd
        Me.tsmiAddWindows.Name = "tsmiAddWindows"
        Me.tsmiAddWindows.Size = New System.Drawing.Size(157, 22)
        Me.tsmiAddWindows.Text = "Add Windows..."
        '
        'tsmiRename
        '
        Me.tsmiRename.Name = "tsmiRename"
        Me.tsmiRename.Size = New System.Drawing.Size(157, 22)
        Me.tsmiRename.Text = "Rename..."
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(154, 6)
        '
        'tsmiCopy
        '
        Me.tsmiCopy.Image = Global.AutoArrange.My.Resources.Resources.Copy
        Me.tsmiCopy.Name = "tsmiCopy"
        Me.tsmiCopy.Size = New System.Drawing.Size(157, 22)
        Me.tsmiCopy.Text = "Copy"
        '
        'tsmiPaste
        '
        Me.tsmiPaste.Name = "tsmiPaste"
        Me.tsmiPaste.Size = New System.Drawing.Size(157, 22)
        Me.tsmiPaste.Text = "Paste"
        '
        'tsmiRemove
        '
        Me.tsmiRemove.Image = Global.AutoArrange.My.Resources.Resources.Delete
        Me.tsmiRemove.Name = "tsmiRemove"
        Me.tsmiRemove.Size = New System.Drawing.Size(157, 22)
        Me.tsmiRemove.Text = "Remove"
        '
        'imageList
        '
        Me.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.imageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.imageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'split
        '
        Me.split.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.split.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.split.Location = New System.Drawing.Point(0, 26)
        Me.split.Name = "split"
        '
        'split.Panel1
        '
        Me.split.Panel1.Controls.Add(Me.tvEditor)
        '
        'split.Panel2
        '
        Me.split.Panel2.Controls.Add(Me.propertyGrid)
        Me.split.Size = New System.Drawing.Size(702, 423)
        Me.split.SplitterDistance = 398
        Me.split.TabIndex = 2
        '
        'propertyGrid
        '
        Me.propertyGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.propertyGrid.LineColor = System.Drawing.SystemColors.ControlDark
        Me.propertyGrid.Location = New System.Drawing.Point(1, 1)
        Me.propertyGrid.Name = "propertyGrid"
        Me.propertyGrid.Size = New System.Drawing.Size(296, 419)
        Me.propertyGrid.TabIndex = 0
        '
        'notifyIcon
        '
        Me.notifyIcon.ContextMenuStrip = Me.cmsNotifyIcon
        Me.notifyIcon.Icon = CType(resources.GetObject("notifyIcon.Icon"), System.Drawing.Icon)
        Me.notifyIcon.Text = "<App Title>"
        Me.notifyIcon.Visible = True
        '
        'cmsNotifyIcon
        '
        Me.cmsNotifyIcon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bManage, Me.ToolStripMenuItem1, Me.bRecord2, Me.bStart2, Me.bStop2, Me.ToolStripMenuItem2, Me.bExit})
        Me.cmsNotifyIcon.Name = "cmsNotifyIcon"
        Me.cmsNotifyIcon.Size = New System.Drawing.Size(206, 126)
        '
        'bManage
        '
        Me.bManage.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bManage.Image = Global.AutoArrange.My.Resources.Resources.MonitorLayoutPNG
        Me.bManage.Name = "bManage"
        Me.bManage.Size = New System.Drawing.Size(205, 22)
        Me.bManage.Text = "Manage"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(202, 6)
        '
        'bRecord2
        '
        Me.bRecord2.Enabled = False
        Me.bRecord2.Image = Global.AutoArrange.My.Resources.Resources.Record
        Me.bRecord2.Name = "bRecord2"
        Me.bRecord2.Size = New System.Drawing.Size(205, 22)
        Me.bRecord2.Text = "Record Current Positions"
        '
        'bStart2
        '
        Me.bStart2.Enabled = False
        Me.bStart2.Image = Global.AutoArrange.My.Resources.Resources.Play
        Me.bStart2.Name = "bStart2"
        Me.bStart2.Size = New System.Drawing.Size(205, 22)
        Me.bStart2.Text = "Start Positioning"
        '
        'bStop2
        '
        Me.bStop2.Enabled = False
        Me.bStop2.Image = Global.AutoArrange.My.Resources.Resources.StopExecution
        Me.bStop2.Name = "bStop2"
        Me.bStop2.Size = New System.Drawing.Size(205, 22)
        Me.bStop2.Text = "Stop Positioning"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(202, 6)
        '
        'bExit
        '
        Me.bExit.Name = "bExit"
        Me.bExit.Size = New System.Drawing.Size(205, 22)
        Me.bExit.Text = "Exit"
        '
        'timerPostOpen
        '
        Me.timerPostOpen.Enabled = True
        Me.timerPostOpen.Interval = 10
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(702, 449)
        Me.Controls.Add(Me.split)
        Me.Controls.Add(Me.toolbar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Form1"
        Me.toolbar.ResumeLayout(False)
        Me.toolbar.PerformLayout()
        Me.cmsLayouts.ResumeLayout(False)
        Me.split.Panel1.ResumeLayout(False)
        Me.split.Panel2.ResumeLayout(False)
        CType(Me.split, System.ComponentModel.ISupportInitialize).EndInit()
        Me.split.ResumeLayout(False)
        Me.cmsNotifyIcon.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents toolbar As System.Windows.Forms.ToolStrip
    Friend WithEvents tvEditor As System.Windows.Forms.TreeView
    Friend WithEvents imageList As System.Windows.Forms.ImageList
    Friend WithEvents bRenameLayout As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bAddWindows As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bRemove As System.Windows.Forms.ToolStripButton
    Friend WithEvents bRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents bRecord As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bStart As System.Windows.Forms.ToolStripButton
    Friend WithEvents bStop As System.Windows.Forms.ToolStripButton
    Friend WithEvents split As System.Windows.Forms.SplitContainer
    Friend WithEvents propertyGrid As System.Windows.Forms.PropertyGrid
    Friend WithEvents notifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents cmsNotifyIcon As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents bManage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bRecord2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bStart2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bStop2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents timerPostOpen As System.Windows.Forms.Timer
    Friend WithEvents bSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents bFilter As ToolStripButton
    Friend WithEvents cmsLayouts As ContextMenuStrip
    Friend WithEvents tsmiAddWindows As ToolStripMenuItem
    Friend WithEvents tsmiRename As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents tsmiCopy As ToolStripMenuItem
    Friend WithEvents tsmiPaste As ToolStripMenuItem
    Friend WithEvents tsmiRemove As ToolStripMenuItem
End Class
