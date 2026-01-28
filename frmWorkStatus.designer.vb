<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWorkStatus
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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(frmWorkStatus))
        cmdCancel = New System.Windows.Forms.Button()
        lstWorkStatus = New System.Windows.Forms.ListBox()
        SuspendLayout()
        ' 
        ' cmdCancel
        ' 
        cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), Drawing.Image)
        cmdCancel.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCancel.Location = New Drawing.Point(49, 281)
        cmdCancel.Margin = New System.Windows.Forms.Padding(6)
        cmdCancel.Name = "cmdCancel"
        cmdCancel.Size = New Drawing.Size(100, 43)
        cmdCancel.TabIndex = 122
        cmdCancel.Text = "Cancel"
        cmdCancel.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCancel.UseVisualStyleBackColor = True
        ' 
        ' lstWorkStatus
        ' 
        lstWorkStatus.BackColor = Drawing.SystemColors.Menu
        lstWorkStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        lstWorkStatus.FormattingEnabled = True
        lstWorkStatus.ItemHeight = 15
        lstWorkStatus.Location = New Drawing.Point(0, 5)
        lstWorkStatus.Margin = New System.Windows.Forms.Padding(4, 3, 4, 66)
        lstWorkStatus.Name = "lstWorkStatus"
        lstWorkStatus.SelectionMode = System.Windows.Forms.SelectionMode.None
        lstWorkStatus.Size = New Drawing.Size(217, 334)
        lstWorkStatus.TabIndex = 124
        ' 
        ' frmWorkStatus
        ' 
        AccessibleRole = System.Windows.Forms.AccessibleRole.List
        AutoScaleDimensions = New Drawing.SizeF(7.0F, 15.0F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        ClientSize = New Drawing.Size(217, 339)
        Controls.Add(lstWorkStatus)
        Controls.Add(cmdCancel)
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Name = "frmWorkStatus"
        Text = "Status"
        ResumeLayout(False)
    End Sub
    Private WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lstWorkStatus As System.Windows.Forms.ListBox
End Class
