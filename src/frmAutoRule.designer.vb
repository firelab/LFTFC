<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAutoRule
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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(frmAutoRule))
        grpSurfaceFUel = New System.Windows.Forms.GroupBox()
        rdoFLM = New System.Windows.Forms.RadioButton()
        rdoFCCS = New System.Windows.Forms.RadioButton()
        rdoFM40 = New System.Windows.Forms.RadioButton()
        rdoFM13 = New System.Windows.Forms.RadioButton()
        rdoCanFM = New System.Windows.Forms.RadioButton()
        cmdGo = New System.Windows.Forms.Button()
        grpSurfaceFUel.SuspendLayout()
        SuspendLayout()
        ' 
        ' grpSurfaceFUel
        ' 
        grpSurfaceFUel.Controls.Add(rdoFLM)
        grpSurfaceFUel.Controls.Add(rdoFCCS)
        grpSurfaceFUel.Controls.Add(rdoFM40)
        grpSurfaceFUel.Controls.Add(rdoFM13)
        grpSurfaceFUel.Controls.Add(rdoCanFM)
        grpSurfaceFUel.Font = New Drawing.Font("Comic Sans MS", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        grpSurfaceFUel.Location = New Drawing.Point(16, 14)
        grpSurfaceFUel.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpSurfaceFUel.Name = "grpSurfaceFUel"
        grpSurfaceFUel.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpSurfaceFUel.Size = New Drawing.Size(398, 167)
        grpSurfaceFUel.TabIndex = 119
        grpSurfaceFUel.TabStop = False
        grpSurfaceFUel.Text = "Select the surface fuel GRID that is in the wildcard slot of the MU?"
        ' 
        ' rdoFLM
        ' 
        rdoFLM.AutoSize = True
        rdoFLM.Font = New Drawing.Font("Comic Sans MS", 9F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        rdoFLM.Location = New Drawing.Point(7, 135)
        rdoFLM.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoFLM.Name = "rdoFLM"
        rdoFLM.Size = New Drawing.Size(130, 21)
        rdoFLM.TabIndex = 4
        rdoFLM.Text = "Fuel Loading Model"
        rdoFLM.UseVisualStyleBackColor = True
        ' 
        ' rdoFCCS
        ' 
        rdoFCCS.AutoSize = True
        rdoFCCS.Font = New Drawing.Font("Comic Sans MS", 9F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        rdoFCCS.Location = New Drawing.Point(7, 104)
        rdoFCCS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoFCCS.Name = "rdoFCCS"
        rdoFCCS.Size = New Drawing.Size(277, 21)
        rdoFCCS.TabIndex = 3
        rdoFCCS.Text = "Fuelbed Characteristic Classification System"
        rdoFCCS.UseVisualStyleBackColor = True
        ' 
        ' rdoFM40
        ' 
        rdoFM40.AutoSize = True
        rdoFM40.Checked = True
        rdoFM40.Font = New Drawing.Font("Comic Sans MS", 9F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        rdoFM40.Location = New Drawing.Point(7, 46)
        rdoFM40.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoFM40.Name = "rdoFM40"
        rdoFM40.Size = New Drawing.Size(140, 21)
        rdoFM40.TabIndex = 2
        rdoFM40.TabStop = True
        rdoFM40.Text = "Scott and Burgan 40"
        rdoFM40.UseVisualStyleBackColor = True
        ' 
        ' rdoFM13
        ' 
        rdoFM13.AutoSize = True
        rdoFM13.Font = New Drawing.Font("Comic Sans MS", 9F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        rdoFM13.Location = New Drawing.Point(7, 20)
        rdoFM13.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoFM13.Name = "rdoFM13"
        rdoFM13.Size = New Drawing.Size(92, 21)
        rdoFM13.TabIndex = 1
        rdoFM13.Text = "Anderson 13"
        rdoFM13.UseVisualStyleBackColor = True
        ' 
        ' rdoCanFM
        ' 
        rdoCanFM.AutoSize = True
        rdoCanFM.Font = New Drawing.Font("Comic Sans MS", 9F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        rdoCanFM.Location = New Drawing.Point(7, 73)
        rdoCanFM.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoCanFM.Name = "rdoCanFM"
        rdoCanFM.Size = New Drawing.Size(106, 21)
        rdoCanFM.TabIndex = 0
        rdoCanFM.Text = "Canadian FBPS"
        rdoCanFM.UseVisualStyleBackColor = True
        ' 
        ' cmdGo
        ' 
        cmdGo.Font = New Drawing.Font("Microsoft Sans Serif", 14.25F, Drawing.FontStyle.Bold Or Drawing.FontStyle.Italic, Drawing.GraphicsUnit.Point)
        cmdGo.ForeColor = Drawing.Color.White
        cmdGo.Image = CType(resources.GetObject("cmdGo.Image"), Drawing.Image)
        cmdGo.Location = New Drawing.Point(187, 231)
        cmdGo.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        cmdGo.Name = "cmdGo"
        cmdGo.Size = New Drawing.Size(59, 48)
        cmdGo.TabIndex = 118
        cmdGo.Text = "Go!"
        cmdGo.UseVisualStyleBackColor = True
        ' 
        ' frmAutoRule
        ' 
        AutoScaleDimensions = New Drawing.SizeF(7F, 15F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        ClientSize = New Drawing.Size(428, 290)
        Controls.Add(grpSurfaceFUel)
        Controls.Add(cmdGo)
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Name = "frmAutoRule"
        Text = "Autorule Surface Fuel Selection"
        grpSurfaceFUel.ResumeLayout(False)
        grpSurfaceFUel.PerformLayout()
        ResumeLayout(False)
    End Sub
    Friend WithEvents grpSurfaceFUel As System.Windows.Forms.GroupBox
    Friend WithEvents rdoFM40 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoFM13 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoCanFM As System.Windows.Forms.RadioButton
    Private WithEvents cmdGo As System.Windows.Forms.Button
    Friend WithEvents rdoFLM As System.Windows.Forms.RadioButton
    Friend WithEvents rdoFCCS As System.Windows.Forms.RadioButton
End Class
