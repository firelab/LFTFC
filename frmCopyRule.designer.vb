<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCopyRule
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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(frmCopyRule))
        cmbCopyMU = New System.Windows.Forms.ComboBox()
        lblCopyMUName = New System.Windows.Forms.Label()
        cmdAutoRule = New System.Windows.Forms.Button()
        cmbCopyEVT = New System.Windows.Forms.ComboBox()
        lblCopyEVT = New System.Windows.Forms.Label()
        cmdCopyRule = New System.Windows.Forms.Button()
        grpMUEVT = New System.Windows.Forms.GroupBox()
        rdoCopyEVT = New System.Windows.Forms.RadioButton()
        rdoCopyMU = New System.Windows.Forms.RadioButton()
        grpRulesOnOff = New System.Windows.Forms.GroupBox()
        rdoCopyOn = New System.Windows.Forms.RadioButton()
        rdoCopyOnOff = New System.Windows.Forms.RadioButton()
        cmdCopyCancel = New System.Windows.Forms.Button()
        rdoAll = New System.Windows.Forms.RadioButton()
        rdoEmpty = New System.Windows.Forms.RadioButton()
        grpEmptyAll = New System.Windows.Forms.GroupBox()
        grpMUEVT.SuspendLayout()
        grpRulesOnOff.SuspendLayout()
        grpEmptyAll.SuspendLayout()
        SuspendLayout()
        ' 
        ' cmbCopyMU
        ' 
        cmbCopyMU.BackColor = Drawing.Color.White
        cmbCopyMU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCopyMU.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbCopyMU.FormattingEnabled = True
        cmbCopyMU.Location = New Drawing.Point(6, 35)
        cmbCopyMU.Margin = New System.Windows.Forms.Padding(6)
        cmbCopyMU.Name = "cmbCopyMU"
        cmbCopyMU.Size = New Drawing.Size(214, 26)
        cmbCopyMU.TabIndex = 9
        ' 
        ' lblCopyMUName
        ' 
        lblCopyMUName.BackColor = Drawing.SystemColors.Control
        lblCopyMUName.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCopyMUName.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCopyMUName.Location = New Drawing.Point(6, 10)
        lblCopyMUName.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCopyMUName.Name = "lblCopyMUName"
        lblCopyMUName.Size = New Drawing.Size(215, 23)
        lblCopyMUName.TabIndex = 8
        lblCopyMUName.Text = "Copy from MU"
        lblCopyMUName.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmdAutoRule
        ' 
        cmdAutoRule.Image = CType(resources.GetObject("cmdAutoRule.Image"), Drawing.Image)
        cmdAutoRule.Location = New Drawing.Point(-342, 8)
        cmdAutoRule.Margin = New System.Windows.Forms.Padding(6)
        cmdAutoRule.Name = "cmdAutoRule"
        cmdAutoRule.Size = New Drawing.Size(43, 43)
        cmdAutoRule.TabIndex = 7
        cmdAutoRule.UseVisualStyleBackColor = True
        ' 
        ' cmbCopyEVT
        ' 
        cmbCopyEVT.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        cmbCopyEVT.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        cmbCopyEVT.BackColor = Drawing.Color.White
        cmbCopyEVT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCopyEVT.Enabled = False
        cmbCopyEVT.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbCopyEVT.FormattingEnabled = True
        cmbCopyEVT.Location = New Drawing.Point(232, 35)
        cmbCopyEVT.Margin = New System.Windows.Forms.Padding(6)
        cmbCopyEVT.Name = "cmbCopyEVT"
        cmbCopyEVT.Size = New Drawing.Size(789, 26)
        cmbCopyEVT.TabIndex = 12
        ' 
        ' lblCopyEVT
        ' 
        lblCopyEVT.BackColor = Drawing.SystemColors.Control
        lblCopyEVT.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCopyEVT.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCopyEVT.Location = New Drawing.Point(232, 10)
        lblCopyEVT.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCopyEVT.Name = "lblCopyEVT"
        lblCopyEVT.Size = New Drawing.Size(790, 23)
        lblCopyEVT.TabIndex = 11
        lblCopyEVT.Text = "Existing Vegetation Number and Name to be copied"
        lblCopyEVT.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmdCopyRule
        ' 
        cmdCopyRule.Image = CType(resources.GetObject("cmdCopyRule.Image"), Drawing.Image)
        cmdCopyRule.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCopyRule.Location = New Drawing.Point(6, 70)
        cmdCopyRule.Margin = New System.Windows.Forms.Padding(6)
        cmdCopyRule.Name = "cmdCopyRule"
        cmdCopyRule.Size = New Drawing.Size(86, 43)
        cmdCopyRule.TabIndex = 13
        cmdCopyRule.Text = "Copy" & vbCrLf & "Rules"
        cmdCopyRule.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCopyRule.UseVisualStyleBackColor = True
        ' 
        ' grpMUEVT
        ' 
        grpMUEVT.Controls.Add(rdoCopyEVT)
        grpMUEVT.Controls.Add(rdoCopyMU)
        grpMUEVT.Font = New Drawing.Font("Comic Sans MS", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        grpMUEVT.Location = New Drawing.Point(232, 68)
        grpMUEVT.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpMUEVT.Name = "grpMUEVT"
        grpMUEVT.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpMUEVT.Size = New Drawing.Size(268, 48)
        grpMUEVT.TabIndex = 14
        grpMUEVT.TabStop = False
        grpMUEVT.Text = "Copy all rules in this MU or just this EVT "
        ' 
        ' rdoCopyEVT
        ' 
        rdoCopyEVT.AutoSize = True
        rdoCopyEVT.Location = New Drawing.Point(172, 21)
        rdoCopyEVT.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoCopyEVT.Name = "rdoCopyEVT"
        rdoCopyEVT.Size = New Drawing.Size(74, 19)
        rdoCopyEVT.TabIndex = 17
        rdoCopyEVT.Text = "Copy EVT"
        rdoCopyEVT.TextAlign = Drawing.ContentAlignment.BottomCenter
        rdoCopyEVT.UseVisualStyleBackColor = True
        ' 
        ' rdoCopyMU
        ' 
        rdoCopyMU.AutoSize = True
        rdoCopyMU.Checked = True
        rdoCopyMU.Location = New Drawing.Point(14, 21)
        rdoCopyMU.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoCopyMU.Name = "rdoCopyMU"
        rdoCopyMU.Size = New Drawing.Size(71, 19)
        rdoCopyMU.TabIndex = 16
        rdoCopyMU.TabStop = True
        rdoCopyMU.Text = "Copy MU"
        rdoCopyMU.TextAlign = Drawing.ContentAlignment.BottomCenter
        rdoCopyMU.UseVisualStyleBackColor = True
        ' 
        ' grpRulesOnOff
        ' 
        grpRulesOnOff.Controls.Add(rdoCopyOn)
        grpRulesOnOff.Controls.Add(rdoCopyOnOff)
        grpRulesOnOff.Font = New Drawing.Font("Comic Sans MS", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        grpRulesOnOff.Location = New Drawing.Point(772, 68)
        grpRulesOnOff.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpRulesOnOff.Name = "grpRulesOnOff"
        grpRulesOnOff.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpRulesOnOff.Size = New Drawing.Size(250, 48)
        grpRulesOnOff.TabIndex = 15
        grpRulesOnOff.TabStop = False
        grpRulesOnOff.Text = "Copy all rules ON and OFF or just ON"
        ' 
        ' rdoCopyOn
        ' 
        rdoCopyOn.AutoSize = True
        rdoCopyOn.Location = New Drawing.Point(158, 21)
        rdoCopyOn.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoCopyOn.Name = "rdoCopyOn"
        rdoCopyOn.Size = New Drawing.Size(71, 19)
        rdoCopyOn.TabIndex = 19
        rdoCopyOn.Text = "Copy ON"
        rdoCopyOn.TextAlign = Drawing.ContentAlignment.BottomCenter
        rdoCopyOn.UseVisualStyleBackColor = True
        ' 
        ' rdoCopyOnOff
        ' 
        rdoCopyOnOff.AutoSize = True
        rdoCopyOnOff.Checked = True
        rdoCopyOnOff.Location = New Drawing.Point(14, 21)
        rdoCopyOnOff.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoCopyOnOff.Name = "rdoCopyOnOff"
        rdoCopyOnOff.Size = New Drawing.Size(116, 19)
        rdoCopyOnOff.TabIndex = 18
        rdoCopyOnOff.TabStop = True
        rdoCopyOnOff.Text = "Copy ON and OFF"
        rdoCopyOnOff.TextAlign = Drawing.ContentAlignment.BottomCenter
        rdoCopyOnOff.UseVisualStyleBackColor = True
        ' 
        ' cmdCopyCancel
        ' 
        cmdCopyCancel.Image = CType(resources.GetObject("cmdCopyCancel.Image"), Drawing.Image)
        cmdCopyCancel.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCopyCancel.Location = New Drawing.Point(127, 70)
        cmdCopyCancel.Margin = New System.Windows.Forms.Padding(6)
        cmdCopyCancel.Name = "cmdCopyCancel"
        cmdCopyCancel.Size = New Drawing.Size(93, 43)
        cmdCopyCancel.TabIndex = 73
        cmdCopyCancel.Text = "Cancel"
        cmdCopyCancel.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCopyCancel.UseVisualStyleBackColor = True
        ' 
        ' rdoAll
        ' 
        rdoAll.AutoSize = True
        rdoAll.Location = New Drawing.Point(154, 21)
        rdoAll.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoAll.Name = "rdoAll"
        rdoAll.Size = New Drawing.Size(86, 19)
        rdoAll.TabIndex = 19
        rdoAll.Text = "Copy to ALL"
        rdoAll.TextAlign = Drawing.ContentAlignment.BottomCenter
        rdoAll.UseVisualStyleBackColor = True
        ' 
        ' rdoEmpty
        ' 
        rdoEmpty.AutoSize = True
        rdoEmpty.Checked = True
        rdoEmpty.Location = New Drawing.Point(13, 21)
        rdoEmpty.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoEmpty.Name = "rdoEmpty"
        rdoEmpty.Size = New Drawing.Size(103, 19)
        rdoEmpty.TabIndex = 18
        rdoEmpty.TabStop = True
        rdoEmpty.Text = "Copy to EMPTY"
        rdoEmpty.TextAlign = Drawing.ContentAlignment.BottomCenter
        rdoEmpty.UseVisualStyleBackColor = True
        ' 
        ' grpEmptyAll
        ' 
        grpEmptyAll.Controls.Add(rdoAll)
        grpEmptyAll.Controls.Add(rdoEmpty)
        grpEmptyAll.Font = New Drawing.Font("Comic Sans MS", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        grpEmptyAll.Location = New Drawing.Point(507, 68)
        grpEmptyAll.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpEmptyAll.Name = "grpEmptyAll"
        grpEmptyAll.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpEmptyAll.Size = New Drawing.Size(259, 48)
        grpEmptyAll.TabIndex = 74
        grpEmptyAll.TabStop = False
        grpEmptyAll.Text = "Copy to EMPTY rulesets or ALL rulesets"
        ' 
        ' frmCopyRule
        ' 
        AutoScaleDimensions = New Drawing.SizeF(7F, 15F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        ClientSize = New Drawing.Size(1028, 120)
        Controls.Add(grpEmptyAll)
        Controls.Add(cmdCopyCancel)
        Controls.Add(grpRulesOnOff)
        Controls.Add(grpMUEVT)
        Controls.Add(cmdCopyRule)
        Controls.Add(cmbCopyEVT)
        Controls.Add(lblCopyEVT)
        Controls.Add(cmbCopyMU)
        Controls.Add(lblCopyMUName)
        Controls.Add(cmdAutoRule)
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Name = "frmCopyRule"
        Text = "Copy Rules"
        grpMUEVT.ResumeLayout(False)
        grpMUEVT.PerformLayout()
        grpRulesOnOff.ResumeLayout(False)
        grpRulesOnOff.PerformLayout()
        grpEmptyAll.ResumeLayout(False)
        grpEmptyAll.PerformLayout()
        ResumeLayout(False)
    End Sub
    Friend WithEvents cmbCopyMU As System.Windows.Forms.ComboBox
    Private WithEvents lblCopyMUName As System.Windows.Forms.Label
    Private WithEvents cmdAutoRule As System.Windows.Forms.Button
    Friend WithEvents cmbCopyEVT As System.Windows.Forms.ComboBox
    Private WithEvents lblCopyEVT As System.Windows.Forms.Label
    Private WithEvents cmdCopyRule As System.Windows.Forms.Button
    Friend WithEvents grpMUEVT As System.Windows.Forms.GroupBox
    Friend WithEvents grpRulesOnOff As System.Windows.Forms.GroupBox
    Friend WithEvents rdoCopyMU As System.Windows.Forms.RadioButton
    Friend WithEvents rdoCopyEVT As System.Windows.Forms.RadioButton
    Friend WithEvents rdoCopyOn As System.Windows.Forms.RadioButton
    Friend WithEvents rdoCopyOnOff As System.Windows.Forms.RadioButton
    Friend WithEvents cmdCopyCancel As System.Windows.Forms.Button
    Friend WithEvents rdoAll As System.Windows.Forms.RadioButton
    Friend WithEvents rdoEmpty As System.Windows.Forms.RadioButton
    Friend WithEvents grpEmptyAll As System.Windows.Forms.GroupBox
End Class
