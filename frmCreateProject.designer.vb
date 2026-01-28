<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateProject
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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(frmCreateProject))
        lblCreateProject = New System.Windows.Forms.Label()
        txtProjName = New System.Windows.Forms.TextBox()
        lblProjName = New System.Windows.Forms.Label()
        cmdDefault = New System.Windows.Forms.Button()
        cmdImport = New System.Windows.Forms.Button()
        cmdCancel = New System.Windows.Forms.Button()
        SuspendLayout()
        ' 
        ' lblCreateProject
        ' 
        lblCreateProject.BackColor = Drawing.Color.FromArgb(CByte(224), CByte(224), CByte(224))
        lblCreateProject.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        lblCreateProject.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCreateProject.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCreateProject.Location = New Drawing.Point(5, 5)
        lblCreateProject.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCreateProject.Name = "lblCreateProject"
        lblCreateProject.Size = New Drawing.Size(307, 247)
        lblCreateProject.TabIndex = 22
        lblCreateProject.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' txtProjName
        ' 
        txtProjName.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtProjName.Location = New Drawing.Point(5, 285)
        txtProjName.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        txtProjName.Name = "txtProjName"
        txtProjName.Size = New Drawing.Size(306, 26)
        txtProjName.TabIndex = 76
        ' 
        ' lblProjName
        ' 
        lblProjName.BackColor = Drawing.Color.White
        lblProjName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        lblProjName.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblProjName.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblProjName.Location = New Drawing.Point(5, 252)
        lblProjName.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblProjName.Name = "lblProjName"
        lblProjName.Size = New Drawing.Size(307, 30)
        lblProjName.TabIndex = 77
        lblProjName.Text = "Enter New Project Name"
        lblProjName.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmdDefault
        ' 
        cmdDefault.Font = New Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmdDefault.Image = CType(resources.GetObject("cmdDefault.Image"), Drawing.Image)
        cmdDefault.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdDefault.Location = New Drawing.Point(5, 324)
        cmdDefault.Margin = New System.Windows.Forms.Padding(6)
        cmdDefault.Name = "cmdDefault"
        cmdDefault.Size = New Drawing.Size(96, 43)
        cmdDefault.TabIndex = 26
        cmdDefault.Text = "Default"
        cmdDefault.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdDefault.UseVisualStyleBackColor = True
        ' 
        ' cmdImport
        ' 
        cmdImport.Font = New Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmdImport.Image = CType(resources.GetObject("cmdImport.Image"), Drawing.Image)
        cmdImport.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdImport.Location = New Drawing.Point(111, 324)
        cmdImport.Margin = New System.Windows.Forms.Padding(6)
        cmdImport.Name = "cmdImport"
        cmdImport.Size = New Drawing.Size(96, 43)
        cmdImport.TabIndex = 25
        cmdImport.Text = "Import"
        cmdImport.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdImport.UseVisualStyleBackColor = True
        ' 
        ' cmdCancel
        ' 
        cmdCancel.Font = New Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), Drawing.Image)
        cmdCancel.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCancel.Location = New Drawing.Point(216, 324)
        cmdCancel.Margin = New System.Windows.Forms.Padding(6)
        cmdCancel.Name = "cmdCancel"
        cmdCancel.Size = New Drawing.Size(96, 43)
        cmdCancel.TabIndex = 24
        cmdCancel.Text = "Cancel"
        cmdCancel.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCancel.UseVisualStyleBackColor = True
        ' 
        ' frmCreateProject
        ' 
        AutoScaleDimensions = New Drawing.SizeF(7F, 15F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        ClientSize = New Drawing.Size(316, 372)
        Controls.Add(lblProjName)
        Controls.Add(txtProjName)
        Controls.Add(cmdDefault)
        Controls.Add(cmdImport)
        Controls.Add(cmdCancel)
        Controls.Add(lblCreateProject)
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Name = "frmCreateProject"
        Text = "Create Project"
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Private WithEvents lblCreateProject As System.Windows.Forms.Label
    Private WithEvents cmdImport As System.Windows.Forms.Button
    Private WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents cmdDefault As System.Windows.Forms.Button
    Friend WithEvents txtProjName As System.Windows.Forms.TextBox
    Private WithEvents lblProjName As System.Windows.Forms.Label
End Class
