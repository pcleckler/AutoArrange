<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSelectWindows
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectWindows))
        Me.timerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.panelDesktop = New System.Windows.Forms.Panel()
        Me.bOk = New System.Windows.Forms.Button()
        Me.bCancel = New System.Windows.Forms.Button()
        Me.lblText = New System.Windows.Forms.Label()
        Me.lblClass = New System.Windows.Forms.Label()
        Me.lblBounds = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'timerRefresh
        '
        Me.timerRefresh.Interval = 1000
        '
        'panelDesktop
        '
        Me.panelDesktop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelDesktop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.panelDesktop.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.panelDesktop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panelDesktop.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.panelDesktop.Location = New System.Drawing.Point(10, 12)
        Me.panelDesktop.MinimumSize = New System.Drawing.Size(20, 20)
        Me.panelDesktop.Name = "panelDesktop"
        Me.panelDesktop.Size = New System.Drawing.Size(800, 426)
        Me.panelDesktop.TabIndex = 0
        '
        'bOk
        '
        Me.bOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bOk.Location = New System.Drawing.Point(735, 471)
        Me.bOk.Name = "bOk"
        Me.bOk.Size = New System.Drawing.Size(75, 23)
        Me.bOk.TabIndex = 1
        Me.bOk.Text = "Ok"
        Me.bOk.UseVisualStyleBackColor = True
        '
        'bCancel
        '
        Me.bCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bCancel.Location = New System.Drawing.Point(654, 471)
        Me.bCancel.Name = "bCancel"
        Me.bCancel.Size = New System.Drawing.Size(75, 23)
        Me.bCancel.TabIndex = 2
        Me.bCancel.Text = "Cancel"
        Me.bCancel.UseVisualStyleBackColor = True
        '
        'lblText
        '
        Me.lblText.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblText.AutoSize = True
        Me.lblText.Location = New System.Drawing.Point(7, 445)
        Me.lblText.Name = "lblText"
        Me.lblText.Size = New System.Drawing.Size(36, 13)
        Me.lblText.TabIndex = 3
        Me.lblText.Text = "<text>"
        '
        'lblClass
        '
        Me.lblClass.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblClass.AutoSize = True
        Me.lblClass.Location = New System.Drawing.Point(7, 467)
        Me.lblClass.Name = "lblClass"
        Me.lblClass.Size = New System.Drawing.Size(43, 13)
        Me.lblClass.TabIndex = 4
        Me.lblClass.Text = "<class>"
        '
        'lblBounds
        '
        Me.lblBounds.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblBounds.AutoSize = True
        Me.lblBounds.Location = New System.Drawing.Point(7, 488)
        Me.lblBounds.Name = "lblBounds"
        Me.lblBounds.Size = New System.Drawing.Size(54, 13)
        Me.lblBounds.TabIndex = 5
        Me.lblBounds.Text = "<bounds>"
        '
        'frmSelectWindows
        '
        Me.AcceptButton = Me.bOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.bCancel
        Me.ClientSize = New System.Drawing.Size(821, 506)
        Me.Controls.Add(Me.lblBounds)
        Me.Controls.Add(Me.lblClass)
        Me.Controls.Add(Me.lblText)
        Me.Controls.Add(Me.bCancel)
        Me.Controls.Add(Me.bOk)
        Me.Controls.Add(Me.panelDesktop)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSelectWindows"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents timerRefresh As System.Windows.Forms.Timer
    Friend WithEvents panelDesktop As System.Windows.Forms.Panel
    Friend WithEvents bOk As System.Windows.Forms.Button
    Friend WithEvents bCancel As System.Windows.Forms.Button
    Friend WithEvents lblText As Label
    Friend WithEvents lblClass As Label
    Friend WithEvents lblBounds As Label
End Class
