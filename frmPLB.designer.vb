<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPLB
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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(frmPLB))
        cmdContinue = New System.Windows.Forms.Button()
        cmdCancel = New System.Windows.Forms.Button()
        lstPLB = New System.Windows.Forms.ListBox()
        cmdSaveList = New System.Windows.Forms.Button()
        SuspendLayout()
        ' 
        ' cmdContinue
        ' 
        cmdContinue.Image = CType(resources.GetObject("cmdContinue.Image"), Drawing.Image)
        cmdContinue.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdContinue.Location = New Drawing.Point(5, 5)
        cmdContinue.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        cmdContinue.Name = "cmdContinue"
        cmdContinue.Size = New Drawing.Size(103, 43)
        cmdContinue.TabIndex = 83
        cmdContinue.Text = "Continue"
        cmdContinue.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdContinue.UseVisualStyleBackColor = True
        ' 
        ' cmdCancel
        ' 
        cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), Drawing.Image)
        cmdCancel.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCancel.Location = New Drawing.Point(196, 5)
        cmdCancel.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        cmdCancel.Name = "cmdCancel"
        cmdCancel.Size = New Drawing.Size(93, 43)
        cmdCancel.TabIndex = 82
        cmdCancel.Text = "Cancel"
        cmdCancel.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCancel.UseVisualStyleBackColor = True
        ' 
        ' lstPLB
        ' 
        lstPLB.Dock = System.Windows.Forms.DockStyle.Bottom
        lstPLB.FormattingEnabled = True
        lstPLB.ItemHeight = 15
        lstPLB.Location = New Drawing.Point(0, 54)
        lstPLB.Margin = New System.Windows.Forms.Padding(4, 3, 4, 66)
        lstPLB.Name = "lstPLB"
        lstPLB.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        lstPLB.Size = New Drawing.Size(294, 394)
        lstPLB.Sorted = True
        lstPLB.TabIndex = 84
        ' 
        ' cmdSaveList
        ' 
        cmdSaveList.Image = CType(resources.GetObject("cmdSaveList.Image"), Drawing.Image)
        cmdSaveList.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdSaveList.Location = New Drawing.Point(111, 5)
        cmdSaveList.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        cmdSaveList.Name = "cmdSaveList"
        cmdSaveList.Size = New Drawing.Size(82, 43)
        cmdSaveList.TabIndex = 85
        cmdSaveList.Text = " Save" & vbCrLf & "List"
        cmdSaveList.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdSaveList.UseVisualStyleBackColor = True
        ' 
        ' frmPLB
        ' 
        AutoScaleDimensions = New Drawing.SizeF(7.0F, 15.0F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        AutoScroll = True
        AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        ClientSize = New Drawing.Size(294, 448)
        Controls.Add(cmdSaveList)
        Controls.Add(lstPLB)
        Controls.Add(cmdContinue)
        Controls.Add(cmdCancel)
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmPLB"
        SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Text = "Pixels Left Behind"
        TopMost = True
        ResumeLayout(False)
    End Sub
    Friend WithEvents cmdContinue As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lstPLB As System.Windows.Forms.ListBox
    Friend WithEvents cmdSaveList As System.Windows.Forms.Button
End Class
