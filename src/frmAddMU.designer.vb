Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAddMU
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
        components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddMU))
        lblAddMUName = New Label()
        txtAddMUName = New TextBox()
        grpInputGRIDs = New GroupBox()
        txtDistPath = New TextBox()
        txtElevPath = New TextBox()
        txtSlpPath = New TextBox()
        txtAspPath = New TextBox()
        txtWildPath = New TextBox()
        txtBPSPath = New TextBox()
        txtEVHPath = New TextBox()
        txtEVCPath = New TextBox()
        txtEVTPath = New TextBox()
        TabCntrlAddMU = New TabControl()
        TabAddMU = New TabPage()
        chkExtent = New CheckBox()
        grpGRIDorTiff = New GroupBox()
        rdoOutTiff = New RadioButton()
        rdoOutGRID = New RadioButton()
        NumericUpDown1 = New NumericUpDown()
        NumericUpDown3 = New NumericUpDown()
        NumericUpDown4 = New NumericUpDown()
        lblStatus = New Label()
        cmdCreateMU = New Button()
        cmdCancel = New Button()
        TTFVT = New ToolTip(components)
        TTFVC = New ToolTip(components)
        TTFVH = New ToolTip(components)
        TTBPS = New ToolTip(components)
        TTFDIST = New ToolTip(components)
        TTWildcard = New ToolTip(components)
        TTAsp = New ToolTip(components)
        TTSlp = New ToolTip(components)
        TTElev = New ToolTip(components)
        lblShapeChanges = New Label()
        txtShapePath = New TextBox()
        cmbChgAreaName = New ComboBox()
        lblChgAreaName = New Label()
        cmbToDist = New ComboBox()
        cmdAddFDistChange = New Button()
        lblFromDist = New Label()
        cmbFromDist = New ComboBox()
        lblToDist = New Label()
        cmdAddStructureChange = New Button()
        cmbFromLF = New ComboBox()
        cmbToLF = New ComboBox()
        lblFromLF = New Label()
        lblToLF = New Label()
        cmbToHeight = New ComboBox()
        cmbToCover = New ComboBox()
        lblToHeight = New Label()
        lblToCover = New Label()
        cmbToEVT = New ComboBox()
        cmdAddEVTChange = New Button()
        cmbFromEVT = New ComboBox()
        lblEVTFrom = New Label()
        lblEVTTo = New Label()
        grpInputGRIDs.SuspendLayout()
        TabCntrlAddMU.SuspendLayout()
        TabAddMU.SuspendLayout()
        grpGRIDorTiff.SuspendLayout()
        CType(NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown4, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblAddMUName
        ' 
        lblAddMUName.BackColor = Drawing.Color.FromArgb(CByte(224), CByte(224), CByte(224))
        lblAddMUName.FlatStyle = FlatStyle.Popup
        lblAddMUName.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblAddMUName.Location = New Drawing.Point(5, 6)
        lblAddMUName.Margin = New Padding(6, 0, 6, 0)
        lblAddMUName.Name = "lblAddMUName"
        lblAddMUName.Size = New Drawing.Size(186, 30)
        lblAddMUName.TabIndex = 9
        lblAddMUName.Text = "Management Unit Name"
        lblAddMUName.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' txtAddMUName
        ' 
        txtAddMUName.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtAddMUName.Location = New Drawing.Point(200, 6)
        txtAddMUName.Margin = New Padding(4, 3, 4, 3)
        txtAddMUName.Name = "txtAddMUName"
        txtAddMUName.Size = New Drawing.Size(154, 26)
        txtAddMUName.TabIndex = 76
        ' 
        ' grpInputGRIDs
        ' 
        grpInputGRIDs.Controls.Add(txtDistPath)
        grpInputGRIDs.Controls.Add(txtElevPath)
        grpInputGRIDs.Controls.Add(txtSlpPath)
        grpInputGRIDs.Controls.Add(txtAspPath)
        grpInputGRIDs.Controls.Add(txtWildPath)
        grpInputGRIDs.Controls.Add(txtBPSPath)
        grpInputGRIDs.Controls.Add(txtEVHPath)
        grpInputGRIDs.Controls.Add(txtEVCPath)
        grpInputGRIDs.Controls.Add(txtEVTPath)
        grpInputGRIDs.Location = New Drawing.Point(5, 64)
        grpInputGRIDs.Margin = New Padding(4, 3, 4, 3)
        grpInputGRIDs.Name = "grpInputGRIDs"
        grpInputGRIDs.Padding = New Padding(4, 3, 4, 3)
        grpInputGRIDs.Size = New Drawing.Size(733, 355)
        grpInputGRIDs.TabIndex = 78
        grpInputGRIDs.TabStop = False
        grpInputGRIDs.Text = "LANDFIRE INPUT GRIDs or TIFFs"
        ' 
        ' txtDistPath
        ' 
        txtDistPath.BackColor = Drawing.Color.White
        txtDistPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtDistPath.Location = New Drawing.Point(7, 162)
        txtDistPath.Margin = New Padding(4, 3, 4, 3)
        txtDistPath.Name = "txtDistPath"
        txtDistPath.ReadOnly = True
        txtDistPath.Size = New Drawing.Size(718, 26)
        txtDistPath.TabIndex = 94
        txtDistPath.Tag = "Fuel Disturbance (optional)"
        txtDistPath.Text = "FDIST     (Double - Click to Set Raster Path)"
        txtDistPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtElevPath
        ' 
        txtElevPath.BackColor = Drawing.Color.White
        txtElevPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtElevPath.Location = New Drawing.Point(7, 310)
        txtElevPath.Margin = New Padding(4, 3, 4, 3)
        txtElevPath.Name = "txtElevPath"
        txtElevPath.ReadOnly = True
        txtElevPath.Size = New Drawing.Size(718, 26)
        txtElevPath.TabIndex = 92
        txtElevPath.Tag = "Not used in analysis. Output is clipped to specified extent (optional)"
        txtElevPath.Text = "Elevation     (Double - Click to Set Raster Path)"
        txtElevPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtSlpPath
        ' 
        txtSlpPath.BackColor = Drawing.Color.White
        txtSlpPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtSlpPath.Location = New Drawing.Point(7, 273)
        txtSlpPath.Margin = New Padding(4, 3, 4, 3)
        txtSlpPath.Name = "txtSlpPath"
        txtSlpPath.ReadOnly = True
        txtSlpPath.Size = New Drawing.Size(718, 26)
        txtSlpPath.TabIndex = 90
        txtSlpPath.Tag = "Not used in analysis. Output is clipped to specified extent (optional)"
        txtSlpPath.Text = "Slope     (Double - Click to Set Raster Path)"
        txtSlpPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtAspPath
        ' 
        txtAspPath.BackColor = Drawing.Color.White
        txtAspPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtAspPath.Location = New Drawing.Point(7, 236)
        txtAspPath.Margin = New Padding(4, 3, 4, 3)
        txtAspPath.Name = "txtAspPath"
        txtAspPath.ReadOnly = True
        txtAspPath.Size = New Drawing.Size(718, 26)
        txtAspPath.TabIndex = 88
        txtAspPath.Tag = "Not used in analysis. Output is clipped to specified extent (optional)"
        txtAspPath.Text = "Aspect     (Double - Click to Set Raster Path)"
        txtAspPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtWildPath
        ' 
        txtWildPath.BackColor = Drawing.Color.White
        txtWildPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtWildPath.Location = New Drawing.Point(7, 199)
        txtWildPath.Margin = New Padding(4, 3, 4, 3)
        txtWildPath.Name = "txtWildPath"
        txtWildPath.ReadOnly = True
        txtWildPath.Size = New Drawing.Size(718, 26)
        txtWildPath.TabIndex = 86
        txtWildPath.Tag = "Additional (optional)"
        txtWildPath.Text = "Wildcard     (Double - Click to Set Raster Path)"
        txtWildPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtBPSPath
        ' 
        txtBPSPath.BackColor = Drawing.Color.White
        txtBPSPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtBPSPath.Location = New Drawing.Point(7, 125)
        txtBPSPath.Margin = New Padding(4, 3, 4, 3)
        txtBPSPath.Name = "txtBPSPath"
        txtBPSPath.ReadOnly = True
        txtBPSPath.Size = New Drawing.Size(718, 26)
        txtBPSPath.TabIndex = 84
        txtBPSPath.Tag = "Biophysical Setting (required)"
        txtBPSPath.Text = "BPS     (Double - Click to Set Raster Path)"
        txtBPSPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtEVHPath
        ' 
        txtEVHPath.BackColor = Drawing.Color.White
        txtEVHPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtEVHPath.Location = New Drawing.Point(7, 88)
        txtEVHPath.Margin = New Padding(4, 3, 4, 3)
        txtEVHPath.Name = "txtEVHPath"
        txtEVHPath.ReadOnly = True
        txtEVHPath.Size = New Drawing.Size(718, 26)
        txtEVHPath.TabIndex = 82
        txtEVHPath.Tag = "Fuel Vegetation Height (required)"
        txtEVHPath.Text = "FVH     (Double - Click to Set Raster Path)"
        txtEVHPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtEVCPath
        ' 
        txtEVCPath.BackColor = Drawing.Color.White
        txtEVCPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtEVCPath.Location = New Drawing.Point(7, 51)
        txtEVCPath.Margin = New Padding(4, 3, 4, 3)
        txtEVCPath.Name = "txtEVCPath"
        txtEVCPath.ReadOnly = True
        txtEVCPath.Size = New Drawing.Size(718, 26)
        txtEVCPath.TabIndex = 80
        txtEVCPath.Tag = "Fuel Vegetation Cover (required)"
        txtEVCPath.Text = "FVC     (Double - Click to Set Raster Path)"
        txtEVCPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtEVTPath
        ' 
        txtEVTPath.BackColor = Drawing.Color.White
        txtEVTPath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtEVTPath.Location = New Drawing.Point(7, 14)
        txtEVTPath.Margin = New Padding(4, 3, 4, 3)
        txtEVTPath.Name = "txtEVTPath"
        txtEVTPath.ReadOnly = True
        txtEVTPath.Size = New Drawing.Size(718, 26)
        txtEVTPath.TabIndex = 78
        txtEVTPath.Tag = "Fuel Vegetation Type (required)"
        txtEVTPath.Text = "FVT     (Double - Click to Set Raster Path)"
        txtEVTPath.TextAlign = HorizontalAlignment.Center
        ' 
        ' TabCntrlAddMU
        ' 
        TabCntrlAddMU.Controls.Add(TabAddMU)
        TabCntrlAddMU.Location = New Drawing.Point(4, 14)
        TabCntrlAddMU.Margin = New Padding(4, 3, 4, 3)
        TabCntrlAddMU.Name = "TabCntrlAddMU"
        TabCntrlAddMU.SelectedIndex = 0
        TabCntrlAddMU.Size = New Drawing.Size(760, 512)
        TabCntrlAddMU.TabIndex = 82
        ' 
        ' TabAddMU
        ' 
        TabAddMU.BackColor = Drawing.Color.WhiteSmoke
        TabAddMU.Controls.Add(chkExtent)
        TabAddMU.Controls.Add(grpGRIDorTiff)
        TabAddMU.Controls.Add(lblStatus)
        TabAddMU.Controls.Add(cmdCreateMU)
        TabAddMU.Controls.Add(lblAddMUName)
        TabAddMU.Controls.Add(cmdCancel)
        TabAddMU.Controls.Add(txtAddMUName)
        TabAddMU.Controls.Add(grpInputGRIDs)
        TabAddMU.Location = New Drawing.Point(4, 24)
        TabAddMU.Margin = New Padding(4, 3, 4, 3)
        TabAddMU.Name = "TabAddMU"
        TabAddMU.Padding = New Padding(4, 3, 4, 3)
        TabAddMU.Size = New Drawing.Size(752, 484)
        TabAddMU.TabIndex = 0
        TabAddMU.Text = "Add MU"
        ' 
        ' chkExtent
        ' 
        chkExtent.AutoSize = True
        chkExtent.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Bold Or Drawing.FontStyle.Underline, Drawing.GraphicsUnit.Point)
        chkExtent.ForeColor = Drawing.Color.Black
        chkExtent.Location = New Drawing.Point(9, 39)
        chkExtent.Name = "chkExtent"
        chkExtent.Size = New Drawing.Size(364, 23)
        chkExtent.TabIndex = 0
        chkExtent.Text = "Limit the extent of the MU to the map view display"
        chkExtent.UseVisualStyleBackColor = True
        ' 
        ' grpGRIDorTiff
        ' 
        grpGRIDorTiff.Controls.Add(rdoOutTiff)
        grpGRIDorTiff.Controls.Add(rdoOutGRID)
        grpGRIDorTiff.Controls.Add(NumericUpDown1)
        grpGRIDorTiff.Controls.Add(NumericUpDown3)
        grpGRIDorTiff.Controls.Add(NumericUpDown4)
        grpGRIDorTiff.Location = New Drawing.Point(558, 426)
        grpGRIDorTiff.Margin = New Padding(4, 3, 4, 3)
        grpGRIDorTiff.Name = "grpGRIDorTiff"
        grpGRIDorTiff.Padding = New Padding(4, 3, 4, 3)
        grpGRIDorTiff.Size = New Drawing.Size(180, 46)
        grpGRIDorTiff.TabIndex = 85
        grpGRIDorTiff.TabStop = False
        grpGRIDorTiff.Text = "Set MU output raster format"
        ' 
        ' rdoOutTiff
        ' 
        rdoOutTiff.AutoSize = True
        rdoOutTiff.Checked = True
        rdoOutTiff.Location = New Drawing.Point(107, 16)
        rdoOutTiff.Margin = New Padding(4, 3, 4, 3)
        rdoOutTiff.Name = "rdoOutTiff"
        rdoOutTiff.Size = New Drawing.Size(46, 19)
        rdoOutTiff.TabIndex = 86
        rdoOutTiff.TabStop = True
        rdoOutTiff.Text = "TIFF"
        rdoOutTiff.UseVisualStyleBackColor = True
        ' 
        ' rdoOutGRID
        ' 
        rdoOutGRID.AutoSize = True
        rdoOutGRID.Location = New Drawing.Point(15, 16)
        rdoOutGRID.Margin = New Padding(4, 3, 4, 3)
        rdoOutGRID.Name = "rdoOutGRID"
        rdoOutGRID.Size = New Drawing.Size(51, 19)
        rdoOutGRID.TabIndex = 85
        rdoOutGRID.Text = "GRID"
        rdoOutGRID.UseVisualStyleBackColor = True
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        NumericUpDown1.Location = New Drawing.Point(90, 124)
        NumericUpDown1.Margin = New Padding(4, 3, 4, 3)
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Drawing.Size(167, 26)
        NumericUpDown1.TabIndex = 84
        ' 
        ' NumericUpDown3
        ' 
        NumericUpDown3.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        NumericUpDown3.Location = New Drawing.Point(7, 87)
        NumericUpDown3.Margin = New Padding(4, 3, 4, 3)
        NumericUpDown3.Name = "NumericUpDown3"
        NumericUpDown3.Size = New Drawing.Size(167, 26)
        NumericUpDown3.TabIndex = 82
        ' 
        ' NumericUpDown4
        ' 
        NumericUpDown4.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        NumericUpDown4.Location = New Drawing.Point(176, 87)
        NumericUpDown4.Margin = New Padding(4, 3, 4, 3)
        NumericUpDown4.Name = "NumericUpDown4"
        NumericUpDown4.Size = New Drawing.Size(167, 26)
        NumericUpDown4.TabIndex = 81
        ' 
        ' lblStatus
        ' 
        lblStatus.BackColor = Drawing.Color.FromArgb(CByte(224), CByte(224), CByte(224))
        lblStatus.FlatStyle = FlatStyle.Popup
        lblStatus.Font = New Drawing.Font("Microsoft Sans Serif", 9F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblStatus.Location = New Drawing.Point(9, 434)
        lblStatus.Margin = New Padding(6, 0, 6, 0)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Drawing.Size(216, 30)
        lblStatus.TabIndex = 95
        lblStatus.Text = "Status Updates"
        lblStatus.TextAlign = Drawing.ContentAlignment.MiddleCenter
        lblStatus.Visible = False
        ' 
        ' cmdCreateMU
        ' 
        cmdCreateMU.Image = CType(resources.GetObject("cmdCreateMU.Image"), Drawing.Image)
        cmdCreateMU.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCreateMU.Location = New Drawing.Point(265, 428)
        cmdCreateMU.Margin = New Padding(6)
        cmdCreateMU.Name = "cmdCreateMU"
        cmdCreateMU.Size = New Drawing.Size(93, 43)
        cmdCreateMU.TabIndex = 81
        cmdCreateMU.Text = "Make" & vbCrLf & "MU"
        cmdCreateMU.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCreateMU.UseVisualStyleBackColor = True
        ' 
        ' cmdCancel
        ' 
        cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), Drawing.Image)
        cmdCancel.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCancel.Location = New Drawing.Point(419, 428)
        cmdCancel.Margin = New Padding(6)
        cmdCancel.Name = "cmdCancel"
        cmdCancel.Size = New Drawing.Size(93, 43)
        cmdCancel.TabIndex = 80
        cmdCancel.Text = "Cancel"
        cmdCancel.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCancel.UseVisualStyleBackColor = True
        ' 
        ' TTFVT
        ' 
        TTFVT.IsBalloon = True
        ' 
        ' TTFVC
        ' 
        TTFVC.IsBalloon = True
        ' 
        ' TTFVH
        ' 
        TTFVH.IsBalloon = True
        ' 
        ' TTBPS
        ' 
        TTBPS.IsBalloon = True
        ' 
        ' TTFDIST
        ' 
        TTFDIST.IsBalloon = True
        ' 
        ' TTWildcard
        ' 
        TTWildcard.IsBalloon = True
        ' 
        ' TTAsp
        ' 
        TTAsp.IsBalloon = True
        ' 
        ' TTSlp
        ' 
        TTSlp.IsBalloon = True
        ' 
        ' TTElev
        ' 
        TTElev.IsBalloon = True
        ' 
        ' lblShapeChanges
        ' 
        lblShapeChanges.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblShapeChanges.BorderStyle = BorderStyle.Fixed3D
        lblShapeChanges.FlatStyle = FlatStyle.Popup
        lblShapeChanges.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblShapeChanges.Location = New Drawing.Point(8, 11)
        lblShapeChanges.Margin = New Padding(5, 0, 5, 0)
        lblShapeChanges.Name = "lblShapeChanges"
        lblShapeChanges.Size = New Drawing.Size(185, 26)
        lblShapeChanges.TabIndex = 77
        lblShapeChanges.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' txtShapePath
        ' 
        txtShapePath.BackColor = Drawing.SystemColors.Control
        txtShapePath.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtShapePath.Location = New Drawing.Point(201, 12)
        txtShapePath.Name = "txtShapePath"
        txtShapePath.Size = New Drawing.Size(422, 26)
        txtShapePath.TabIndex = 78
        txtShapePath.TextAlign = HorizontalAlignment.Center
        ' 
        ' cmbChgAreaName
        ' 
        cmbChgAreaName.DropDownStyle = ComboBoxStyle.DropDownList
        cmbChgAreaName.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbChgAreaName.FormattingEnabled = True
        cmbChgAreaName.Location = New Drawing.Point(201, 46)
        cmbChgAreaName.Name = "cmbChgAreaName"
        cmbChgAreaName.Size = New Drawing.Size(422, 26)
        cmbChgAreaName.TabIndex = 126
        ' 
        ' lblChgAreaName
        ' 
        lblChgAreaName.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblChgAreaName.BorderStyle = BorderStyle.Fixed3D
        lblChgAreaName.FlatStyle = FlatStyle.Popup
        lblChgAreaName.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblChgAreaName.Location = New Drawing.Point(8, 47)
        lblChgAreaName.Margin = New Padding(5, 0, 5, 0)
        lblChgAreaName.Name = "lblChgAreaName"
        lblChgAreaName.Size = New Drawing.Size(185, 26)
        lblChgAreaName.TabIndex = 127
        lblChgAreaName.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbToDist
        ' 
        cmbToDist.DropDownStyle = ComboBoxStyle.DropDownList
        cmbToDist.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbToDist.FormattingEnabled = True
        cmbToDist.Location = New Drawing.Point(70, 42)
        cmbToDist.Name = "cmbToDist"
        cmbToDist.Size = New Drawing.Size(552, 26)
        cmbToDist.TabIndex = 149
        ' 
        ' cmdAddFDistChange
        ' 
        cmdAddFDistChange.Font = New Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmdAddFDistChange.Image = CType(resources.GetObject("cmdAddFDistChange.Image"), Drawing.Image)
        cmdAddFDistChange.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdAddFDistChange.Location = New Drawing.Point(237, 71)
        cmdAddFDistChange.Margin = New Padding(5)
        cmdAddFDistChange.Name = "cmdAddFDistChange"
        cmdAddFDistChange.Size = New Drawing.Size(156, 37)
        cmdAddFDistChange.TabIndex = 147
        cmdAddFDistChange.Text = "Change Disturbance"
        cmdAddFDistChange.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdAddFDistChange.UseVisualStyleBackColor = True
        ' 
        ' lblFromDist
        ' 
        lblFromDist.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblFromDist.BorderStyle = BorderStyle.Fixed3D
        lblFromDist.FlatStyle = FlatStyle.Popup
        lblFromDist.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblFromDist.Location = New Drawing.Point(8, 11)
        lblFromDist.Margin = New Padding(5, 0, 5, 0)
        lblFromDist.Name = "lblFromDist"
        lblFromDist.Size = New Drawing.Size(55, 26)
        lblFromDist.TabIndex = 159
        lblFromDist.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbFromDist
        ' 
        cmbFromDist.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFromDist.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbFromDist.FormattingEnabled = True
        cmbFromDist.Location = New Drawing.Point(70, 11)
        cmbFromDist.Name = "cmbFromDist"
        cmbFromDist.Size = New Drawing.Size(552, 26)
        cmbFromDist.TabIndex = 150
        ' 
        ' lblToDist
        ' 
        lblToDist.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblToDist.BorderStyle = BorderStyle.Fixed3D
        lblToDist.FlatStyle = FlatStyle.Popup
        lblToDist.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblToDist.Location = New Drawing.Point(8, 42)
        lblToDist.Margin = New Padding(5, 0, 5, 0)
        lblToDist.Name = "lblToDist"
        lblToDist.Size = New Drawing.Size(55, 26)
        lblToDist.TabIndex = 160
        lblToDist.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmdAddStructureChange
        ' 
        cmdAddStructureChange.Font = New Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmdAddStructureChange.Image = CType(resources.GetObject("cmdAddStructureChange.Image"), Drawing.Image)
        cmdAddStructureChange.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdAddStructureChange.Location = New Drawing.Point(237, 73)
        cmdAddStructureChange.Margin = New Padding(5)
        cmdAddStructureChange.Name = "cmdAddStructureChange"
        cmdAddStructureChange.Size = New Drawing.Size(156, 37)
        cmdAddStructureChange.TabIndex = 147
        cmdAddStructureChange.Text = "Change Structure"
        cmdAddStructureChange.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdAddStructureChange.UseVisualStyleBackColor = True
        ' 
        ' cmbFromLF
        ' 
        cmbFromLF.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFromLF.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbFromLF.FormattingEnabled = True
        cmbFromLF.Location = New Drawing.Point(8, 44)
        cmbFromLF.Name = "cmbFromLF"
        cmbFromLF.Size = New Drawing.Size(195, 26)
        cmbFromLF.TabIndex = 148
        ' 
        ' cmbToLF
        ' 
        cmbToLF.DropDownStyle = ComboBoxStyle.DropDownList
        cmbToLF.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbToLF.FormattingEnabled = True
        cmbToLF.Location = New Drawing.Point(213, 44)
        cmbToLF.Name = "cmbToLF"
        cmbToLF.Size = New Drawing.Size(195, 26)
        cmbToLF.TabIndex = 150
        ' 
        ' lblFromLF
        ' 
        lblFromLF.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblFromLF.BorderStyle = BorderStyle.Fixed3D
        lblFromLF.FlatStyle = FlatStyle.Popup
        lblFromLF.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblFromLF.Location = New Drawing.Point(8, 14)
        lblFromLF.Margin = New Padding(5, 0, 5, 0)
        lblFromLF.Name = "lblFromLF"
        lblFromLF.Size = New Drawing.Size(197, 26)
        lblFromLF.TabIndex = 151
        lblFromLF.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblToLF
        ' 
        lblToLF.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblToLF.BorderStyle = BorderStyle.Fixed3D
        lblToLF.FlatStyle = FlatStyle.Popup
        lblToLF.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblToLF.Location = New Drawing.Point(213, 14)
        lblToLF.Margin = New Padding(5, 0, 5, 0)
        lblToLF.Name = "lblToLF"
        lblToLF.Size = New Drawing.Size(198, 26)
        lblToLF.TabIndex = 152
        lblToLF.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbToHeight
        ' 
        cmbToHeight.DropDownStyle = ComboBoxStyle.DropDownList
        cmbToHeight.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbToHeight.FormattingEnabled = True
        cmbToHeight.Location = New Drawing.Point(522, 44)
        cmbToHeight.Name = "cmbToHeight"
        cmbToHeight.Size = New Drawing.Size(98, 26)
        cmbToHeight.TabIndex = 149
        ' 
        ' cmbToCover
        ' 
        cmbToCover.DropDownStyle = ComboBoxStyle.DropDownList
        cmbToCover.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbToCover.FormattingEnabled = True
        cmbToCover.Location = New Drawing.Point(418, 44)
        cmbToCover.Name = "cmbToCover"
        cmbToCover.Size = New Drawing.Size(96, 26)
        cmbToCover.TabIndex = 154
        ' 
        ' lblToHeight
        ' 
        lblToHeight.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblToHeight.BorderStyle = BorderStyle.Fixed3D
        lblToHeight.FlatStyle = FlatStyle.Popup
        lblToHeight.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblToHeight.Location = New Drawing.Point(523, 13)
        lblToHeight.Margin = New Padding(5, 0, 5, 0)
        lblToHeight.Name = "lblToHeight"
        lblToHeight.Size = New Drawing.Size(98, 26)
        lblToHeight.TabIndex = 153
        lblToHeight.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblToCover
        ' 
        lblToCover.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblToCover.BorderStyle = BorderStyle.Fixed3D
        lblToCover.FlatStyle = FlatStyle.Popup
        lblToCover.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblToCover.Location = New Drawing.Point(418, 13)
        lblToCover.Margin = New Padding(5, 0, 5, 0)
        lblToCover.Name = "lblToCover"
        lblToCover.Size = New Drawing.Size(98, 26)
        lblToCover.TabIndex = 155
        lblToCover.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbToEVT
        ' 
        cmbToEVT.DropDownStyle = ComboBoxStyle.DropDownList
        cmbToEVT.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbToEVT.FormattingEnabled = True
        cmbToEVT.Location = New Drawing.Point(70, 45)
        cmbToEVT.Name = "cmbToEVT"
        cmbToEVT.Size = New Drawing.Size(550, 26)
        cmbToEVT.TabIndex = 149
        ' 
        ' cmdAddEVTChange
        ' 
        cmdAddEVTChange.Font = New Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmdAddEVTChange.Image = CType(resources.GetObject("cmdAddEVTChange.Image"), Drawing.Image)
        cmdAddEVTChange.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdAddEVTChange.Location = New Drawing.Point(237, 73)
        cmdAddEVTChange.Margin = New Padding(5)
        cmdAddEVTChange.Name = "cmdAddEVTChange"
        cmdAddEVTChange.Size = New Drawing.Size(156, 37)
        cmdAddEVTChange.TabIndex = 150
        cmdAddEVTChange.Text = "Change EVT"
        cmdAddEVTChange.UseVisualStyleBackColor = True
        ' 
        ' cmbFromEVT
        ' 
        cmbFromEVT.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFromEVT.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbFromEVT.FormattingEnabled = True
        cmbFromEVT.Location = New Drawing.Point(70, 15)
        cmbFromEVT.Name = "cmbFromEVT"
        cmbFromEVT.Size = New Drawing.Size(550, 26)
        cmbFromEVT.TabIndex = 151
        ' 
        ' lblEVTFrom
        ' 
        lblEVTFrom.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblEVTFrom.BorderStyle = BorderStyle.Fixed3D
        lblEVTFrom.FlatStyle = FlatStyle.Popup
        lblEVTFrom.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblEVTFrom.Location = New Drawing.Point(7, 15)
        lblEVTFrom.Margin = New Padding(5, 0, 5, 0)
        lblEVTFrom.Name = "lblEVTFrom"
        lblEVTFrom.Size = New Drawing.Size(55, 26)
        lblEVTFrom.TabIndex = 157
        lblEVTFrom.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblEVTTo
        ' 
        lblEVTTo.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        lblEVTTo.BorderStyle = BorderStyle.Fixed3D
        lblEVTTo.FlatStyle = FlatStyle.Popup
        lblEVTTo.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblEVTTo.Location = New Drawing.Point(7, 45)
        lblEVTTo.Margin = New Padding(5, 0, 5, 0)
        lblEVTTo.Name = "lblEVTTo"
        lblEVTTo.Size = New Drawing.Size(55, 26)
        lblEVTTo.TabIndex = 158
        lblEVTTo.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' frmAddMU
        ' 
        AutoScaleDimensions = New Drawing.SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Drawing.Size(763, 526)
        Controls.Add(TabCntrlAddMU)
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New Padding(4, 3, 4, 3)
        Name = "frmAddMU"
        Text = "Create New Management Unit"
        grpInputGRIDs.ResumeLayout(False)
        grpInputGRIDs.PerformLayout()
        TabCntrlAddMU.ResumeLayout(False)
        TabAddMU.ResumeLayout(False)
        TabAddMU.PerformLayout()
        grpGRIDorTiff.ResumeLayout(False)
        grpGRIDorTiff.PerformLayout()
        CType(NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown4, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub
    Private WithEvents lblAddMUName As Label
    Friend WithEvents txtAddMUName As TextBox
    Friend WithEvents grpInputGRIDs As GroupBox
    Friend WithEvents txtEVTPath As TextBox
    Friend WithEvents txtElevPath As TextBox
    Friend WithEvents txtSlpPath As TextBox
    Friend WithEvents txtAspPath As TextBox
    Friend WithEvents txtWildPath As TextBox
    Friend WithEvents txtBPSPath As TextBox
    Friend WithEvents txtEVHPath As TextBox
    Friend WithEvents txtEVCPath As TextBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdCreateMU As Button
    Friend WithEvents TabCntrlAddMU As TabControl
    Friend WithEvents TabAddMU As TabPage
    Friend WithEvents txtDistPath As TextBox
    Private WithEvents lblStatus As Label
    Friend WithEvents TTFVT As ToolTip
    Friend WithEvents TTFVC As ToolTip
    Friend WithEvents TTFVH As ToolTip
    Friend WithEvents TTBPS As ToolTip
    Friend WithEvents TTFDIST As ToolTip
    Friend WithEvents TTWildcard As ToolTip
    Friend WithEvents TTAsp As ToolTip
    Friend WithEvents TTSlp As ToolTip
    Friend WithEvents TTElev As ToolTip
    Private WithEvents lblShapeChanges As Label
    Friend WithEvents txtShapePath As TextBox
    Friend WithEvents cmbChgAreaName As ComboBox
    Private WithEvents lblChgAreaName As Label
    Friend WithEvents cmbToDist As ComboBox
    Friend WithEvents cmdAddFDistChange As Button
    Private WithEvents lblFromDist As Label
    Friend WithEvents cmbFromDist As ComboBox
    Private WithEvents lblToDist As Label
    Friend WithEvents cmdAddStructureChange As Button
    Friend WithEvents cmbFromLF As ComboBox
    Friend WithEvents cmbToLF As ComboBox
    Private WithEvents lblFromLF As Label
    Private WithEvents lblToLF As Label
    Friend WithEvents cmbToHeight As ComboBox
    Friend WithEvents cmbToCover As ComboBox
    Private WithEvents lblToHeight As Label
    Private WithEvents lblToCover As Label
    Friend WithEvents cmbToEVT As ComboBox
    Friend WithEvents cmdAddEVTChange As Button
    Friend WithEvents cmbFromEVT As ComboBox
    Private WithEvents lblEVTFrom As Label
    Private WithEvents lblEVTTo As Label
    Friend WithEvents grpGRIDorTiff As GroupBox
    Friend WithEvents rdoOutTiff As RadioButton
    Friend WithEvents rdoOutGRID As RadioButton
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents NumericUpDown3 As NumericUpDown
    Friend WithEvents NumericUpDown4 As NumericUpDown
    Friend WithEvents chkExtent As CheckBox
End Class
