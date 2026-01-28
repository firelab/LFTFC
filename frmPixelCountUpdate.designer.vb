<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPixelCountUpdate
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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(frmPixelCountUpdate))
        grpProcess = New System.Windows.Forms.GroupBox()
        prgBarBlue = New System.Windows.Forms.ProgressBar()
        cmdPCUCancel = New System.Windows.Forms.Button()
        BGActivity = New ComponentModel.BackgroundWorker()
        grpProcess.SuspendLayout()
        SuspendLayout()
        ' 
        ' grpProcess
        ' 
        grpProcess.Controls.Add(prgBarBlue)
        grpProcess.Font = New Drawing.Font("Comic Sans MS", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        grpProcess.Location = New Drawing.Point(1, 1)
        grpProcess.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpProcess.Name = "grpProcess"
        grpProcess.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpProcess.Size = New Drawing.Size(338, 60)
        grpProcess.TabIndex = 122
        grpProcess.TabStop = False
        grpProcess.Text = "Processing Status"
        ' 
        ' prgBarBlue
        ' 
        prgBarBlue.Location = New Drawing.Point(7, 25)
        prgBarBlue.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        prgBarBlue.Name = "prgBarBlue"
        prgBarBlue.Size = New Drawing.Size(324, 21)
        prgBarBlue.Step = 20
        prgBarBlue.TabIndex = 118
        ' 
        ' cmdPCUCancel
        ' 
        cmdPCUCancel.Image = CType(resources.GetObject("cmdPCUCancel.Image"), Drawing.Image)
        cmdPCUCancel.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdPCUCancel.Location = New Drawing.Point(127, 70)
        cmdPCUCancel.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        cmdPCUCancel.Name = "cmdPCUCancel"
        cmdPCUCancel.Size = New Drawing.Size(93, 43)
        cmdPCUCancel.TabIndex = 1
        cmdPCUCancel.Text = "Cancel"
        cmdPCUCancel.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdPCUCancel.UseVisualStyleBackColor = True
        ' 
        ' BGActivity
        ' 
        BGActivity.WorkerReportsProgress = True
        ' 
        ' frmPixelCountUpdate
        ' 
        AutoScaleDimensions = New Drawing.SizeF(7F, 15F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        ClientSize = New Drawing.Size(341, 115)
        Controls.Add(cmdPCUCancel)
        Controls.Add(grpProcess)
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Name = "frmPixelCountUpdate"
        Text = "Pixel Count Updates"
        grpProcess.ResumeLayout(False)
        ResumeLayout(False)
    End Sub
    Friend WithEvents grpProcess As System.Windows.Forms.GroupBox
    Friend WithEvents prgBarBlue As System.Windows.Forms.ProgressBar
    Friend WithEvents cmdPCUCancel As System.Windows.Forms.Button
    Friend WithEvents BGActivity As System.ComponentModel.BackgroundWorker
End Class
