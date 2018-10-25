<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportDB
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ImportDB))
        Me.Panel29 = New System.Windows.Forms.Panel()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.txtImportPath = New System.Windows.Forms.TextBox()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.txtImportHost = New System.Windows.Forms.TextBox()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.txtImportUser = New System.Windows.Forms.TextBox()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.txtImportPass = New System.Windows.Forms.TextBox()
        Me.Button21 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel28 = New System.Windows.Forms.Panel()
        Me.Panel29.SuspendLayout()
        Me.Panel28.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel29
        '
        Me.Panel29.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel29.Controls.Add(Me.Label71)
        Me.Panel29.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel29.Location = New System.Drawing.Point(0, 0)
        Me.Panel29.Name = "Panel29"
        Me.Panel29.Size = New System.Drawing.Size(366, 31)
        Me.Panel29.TabIndex = 2
        '
        'Label71
        '
        Me.Label71.AutoSize = True
        Me.Label71.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label71.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label71.Location = New System.Drawing.Point(10, 6)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(124, 18)
        Me.Label71.TabIndex = 6
        Me.Label71.Text = "Import Database"
        '
        'Label93
        '
        Me.Label93.AutoSize = True
        Me.Label93.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label93.ForeColor = System.Drawing.Color.White
        Me.Label93.Location = New System.Drawing.Point(43, 207)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(35, 16)
        Me.Label93.TabIndex = 59
        Me.Label93.Text = "Path"
        '
        'txtImportPath
        '
        Me.txtImportPath.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImportPath.Location = New System.Drawing.Point(84, 197)
        Me.txtImportPath.Multiline = True
        Me.txtImportPath.Name = "txtImportPath"
        Me.txtImportPath.ReadOnly = True
        Me.txtImportPath.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtImportPath.Size = New System.Drawing.Size(244, 26)
        Me.txtImportPath.TabIndex = 58
        '
        'Label92
        '
        Me.Label92.AutoSize = True
        Me.Label92.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label92.ForeColor = System.Drawing.Color.White
        Me.Label92.Location = New System.Drawing.Point(5, 81)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(73, 16)
        Me.Label92.TabIndex = 61
        Me.Label92.Text = "Host Name"
        '
        'txtImportHost
        '
        Me.txtImportHost.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImportHost.Location = New System.Drawing.Point(84, 71)
        Me.txtImportHost.Multiline = True
        Me.txtImportHost.Name = "txtImportHost"
        Me.txtImportHost.Size = New System.Drawing.Size(244, 26)
        Me.txtImportHost.TabIndex = 60
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label91.ForeColor = System.Drawing.Color.White
        Me.Label91.Location = New System.Drawing.Point(11, 123)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(67, 16)
        Me.Label91.TabIndex = 63
        Me.Label91.Text = "Username"
        '
        'txtImportUser
        '
        Me.txtImportUser.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImportUser.Location = New System.Drawing.Point(84, 113)
        Me.txtImportUser.Multiline = True
        Me.txtImportUser.Name = "txtImportUser"
        Me.txtImportUser.Size = New System.Drawing.Size(244, 26)
        Me.txtImportUser.TabIndex = 62
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label90.ForeColor = System.Drawing.Color.White
        Me.Label90.Location = New System.Drawing.Point(13, 165)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(65, 16)
        Me.Label90.TabIndex = 65
        Me.Label90.Text = "Password"
        '
        'txtImportPass
        '
        Me.txtImportPass.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImportPass.Location = New System.Drawing.Point(84, 155)
        Me.txtImportPass.Multiline = True
        Me.txtImportPass.Name = "txtImportPass"
        Me.txtImportPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.txtImportPass.Size = New System.Drawing.Size(244, 26)
        Me.txtImportPass.TabIndex = 64
        '
        'Button21
        '
        Me.Button21.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Button21.FlatAppearance.BorderSize = 0
        Me.Button21.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button21.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button21.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button21.Location = New System.Drawing.Point(180, 261)
        Me.Button21.Name = "Button21"
        Me.Button21.Size = New System.Drawing.Size(71, 35)
        Me.Button21.TabIndex = 53
        Me.Button21.Text = "Import"
        Me.Button21.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Button5.FlatAppearance.BorderSize = 0
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button5.Location = New System.Drawing.Point(104, 261)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(71, 35)
        Me.Button5.TabIndex = 69
        Me.Button5.Text = "Browse"
        Me.Button5.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button1.Location = New System.Drawing.Point(257, 261)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(71, 35)
        Me.Button1.TabIndex = 70
        Me.Button1.Text = "Cancel"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Panel28
        '
        Me.Panel28.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.Panel28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel28.Controls.Add(Me.Button1)
        Me.Panel28.Controls.Add(Me.Button5)
        Me.Panel28.Controls.Add(Me.Button21)
        Me.Panel28.Controls.Add(Me.txtImportPass)
        Me.Panel28.Controls.Add(Me.Label90)
        Me.Panel28.Controls.Add(Me.txtImportUser)
        Me.Panel28.Controls.Add(Me.Label91)
        Me.Panel28.Controls.Add(Me.txtImportHost)
        Me.Panel28.Controls.Add(Me.Label92)
        Me.Panel28.Controls.Add(Me.txtImportPath)
        Me.Panel28.Controls.Add(Me.Label93)
        Me.Panel28.Controls.Add(Me.Panel29)
        Me.Panel28.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel28.Location = New System.Drawing.Point(0, 0)
        Me.Panel28.Name = "Panel28"
        Me.Panel28.Size = New System.Drawing.Size(368, 380)
        Me.Panel28.TabIndex = 54
        '
        'ImportDB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 380)
        Me.Controls.Add(Me.Panel28)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ImportDB"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ImportDB"
        Me.Panel29.ResumeLayout(False)
        Me.Panel29.PerformLayout()
        Me.Panel28.ResumeLayout(False)
        Me.Panel28.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel29 As System.Windows.Forms.Panel
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents Label93 As System.Windows.Forms.Label
    Friend WithEvents txtImportPath As System.Windows.Forms.TextBox
    Friend WithEvents Label92 As System.Windows.Forms.Label
    Friend WithEvents txtImportHost As System.Windows.Forms.TextBox
    Friend WithEvents Label91 As System.Windows.Forms.Label
    Friend WithEvents txtImportUser As System.Windows.Forms.TextBox
    Friend WithEvents Label90 As System.Windows.Forms.Label
    Friend WithEvents txtImportPass As System.Windows.Forms.TextBox
    Friend WithEvents Button21 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Panel28 As System.Windows.Forms.Panel
End Class
