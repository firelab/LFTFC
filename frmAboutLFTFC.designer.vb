<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAboutLFTFC
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAboutLFTFC))
        OK_Button = New System.Windows.Forms.Button()
        Pan = New System.Windows.Forms.Panel()
        lblDeveloper = New System.Windows.Forms.Label()
        lblVersion = New System.Windows.Forms.Label()
        lblTitle = New System.Windows.Forms.Label()
        Pan.SuspendLayout()
        SuspendLayout()
        ' 
        ' OK_Button
        ' 
        OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        OK_Button.Location = New System.Drawing.Point(112, 373)
        OK_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        OK_Button.Name = "OK_Button"
        OK_Button.Size = New System.Drawing.Size(78, 27)
        OK_Button.TabIndex = 0
        OK_Button.Text = "OK"
        ' 
        ' Pan
        ' 
        Pan.BackColor = Drawing.Color.Silver
        Pan.BackgroundImage = CType(resources.GetObject("Pan.BackgroundImage"), Drawing.Image)
        Pan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Pan.Controls.Add(lblDeveloper)
        Pan.Controls.Add(lblVersion)
        Pan.Controls.Add(lblTitle)
        Pan.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        Pan.Location = New System.Drawing.Point(1, 2)
        Pan.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Pan.Name = "Pan"
        Pan.Size = New System.Drawing.Size(483, 363)
        Pan.TabIndex = 1
        ' 
        ' lblDeveloper
        ' 
        lblDeveloper.AutoSize = True
        lblDeveloper.BackColor = Drawing.Color.Transparent
        lblDeveloper.Font = New System.Drawing.Font("Comic Sans MS", 6.75F)
        lblDeveloper.Location = New System.Drawing.Point(28, 65)
        lblDeveloper.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        lblDeveloper.Name = "lblDeveloper"
        lblDeveloper.Size = New System.Drawing.Size(221, 52)
        lblDeveloper.TabIndex = 3
        lblDeveloper.Text = "LANDFIRE Interagency Program (Fuel)" & vbCrLf & "Ver. 4.03 ArcPro 3" & vbCrLf & "Dev: Tobin Smail USFS RMRS FMI LANDFIRE" & vbCrLf & "        Michelle Hawks FS-FSIC-Enterprise Program" & vbCrLf
        ' 
        ' lblVersion
        ' 
        lblVersion.AutoSize = True
        lblVersion.Location = New System.Drawing.Point(54, 36)
        lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        lblVersion.Name = "lblVersion"
        lblVersion.Size = New System.Drawing.Size(184, 18)
        lblVersion.TabIndex = 1
        lblVersion.Text = "(Landfire Total Fuel Change)"
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New System.Drawing.Font("Comic Sans MS", 18F)
        lblTitle.Location = New System.Drawing.Point(89, 7)
        lblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New System.Drawing.Size(95, 33)
        lblTitle.TabIndex = 0
        lblTitle.Text = "LF TFC"
        ' 
        ' frmAboutLFTFC
        ' 
        AcceptButton = OK_Button
        AutoScaleDimensions = New System.Drawing.SizeF(7F, 15F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        BackColor = Drawing.SystemColors.ScrollBar
        ClientSize = New System.Drawing.Size(285, 413)
        Controls.Add(OK_Button)
        Controls.Add(Pan)
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmAboutLFTFC"
        ShowInTaskbar = False
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Text = "About LF TFC Toolbar"
        Pan.ResumeLayout(False)
        Pan.PerformLayout()
        ResumeLayout(False)
    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Pan As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lblDeveloper As System.Windows.Forms.Label

End Class
