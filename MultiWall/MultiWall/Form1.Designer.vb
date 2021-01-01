<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.btnApply = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.mnuTray = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuShow = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkStartup = New System.Windows.Forms.CheckBox()
        Me.picPreviewMonBg = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.mnuTray.SuspendLayout()
        CType(Me.picPreviewMonBg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnApply
        '
        Me.btnApply.BackColor = System.Drawing.Color.SteelBlue
        Me.btnApply.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnApply.Font = New System.Drawing.Font("BankGothic Lt BT", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApply.ForeColor = System.Drawing.Color.White
        Me.btnApply.Location = New System.Drawing.Point(140, 244)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(279, 62)
        Me.btnApply.TabIndex = 0
        Me.btnApply.Text = "Apply wallpaper"
        Me.btnApply.UseVisualStyleBackColor = False
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = """Pictures|*.jpg;*.jpeg;*.png;*.bmp"";"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.BalloonTipText = "EasyMultiWall"
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "EasyMultiWall"
        Me.NotifyIcon1.Visible = True
        '
        'mnuTray
        '
        Me.mnuTray.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShow, Me.ToolStripSeparator1, Me.mnuExit})
        Me.mnuTray.Name = "mnuTray"
        Me.mnuTray.Size = New System.Drawing.Size(104, 54)
        '
        'mnuShow
        '
        Me.mnuShow.Name = "mnuShow"
        Me.mnuShow.Size = New System.Drawing.Size(103, 22)
        Me.mnuShow.Text = "Show"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(100, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(103, 22)
        Me.mnuExit.Text = "Exit"
        '
        'chkStartup
        '
        Me.chkStartup.AutoSize = True
        Me.chkStartup.Location = New System.Drawing.Point(4, 289)
        Me.chkStartup.Name = "chkStartup"
        Me.chkStartup.Size = New System.Drawing.Size(96, 17)
        Me.chkStartup.TabIndex = 1
        Me.chkStartup.Text = "Run on startup"
        Me.chkStartup.UseVisualStyleBackColor = True
        '
        'picPreviewMonBg
        '
        Me.picPreviewMonBg.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer))
        Me.picPreviewMonBg.Location = New System.Drawing.Point(0, 3)
        Me.picPreviewMonBg.Name = "picPreviewMonBg"
        Me.picPreviewMonBg.Size = New System.Drawing.Size(309, 216)
        Me.picPreviewMonBg.TabIndex = 2
        Me.picPreviewMonBg.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label1.Location = New System.Drawing.Point(-3, 224)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(542, 14)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = " Drag And Drop picture on specific monitor or Double click on monitor to choose a" &
    " different picture."
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(549, 310)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.picPreviewMonBg)
        Me.Controls.Add(Me.chkStartup)
        Me.Controls.Add(Me.btnApply)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(555, 336)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EasyMultiWall - by Antgiann v1.0"
        Me.mnuTray.ResumeLayout(False)
        CType(Me.picPreviewMonBg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnApply As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents mnuTray As ContextMenuStrip
    Friend WithEvents mnuShow As ToolStripMenuItem
    Friend WithEvents mnuExit As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents chkStartup As CheckBox
    Friend WithEvents picPreviewMonBg As PictureBox
    Friend WithEvents Label1 As Label
End Class
