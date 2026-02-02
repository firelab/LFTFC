Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRule
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
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRule))
        PerEVT = New ColumnHeader()
        CBD13 = New ColumnHeader()
        CBH13 = New ColumnHeader()
        OnOff = New ColumnHeader()
        Acres = New ColumnHeader()
        DistributionGraph = New TabPage()
        grpCanopyLines = New GroupBox()
        rdoCHDistGraph = New RadioButton()
        rdoCCDistGraph = New RadioButton()
        rdoCBDDistGraph = New RadioButton()
        rdoCBHDistGraph = New RadioButton()
        rdoNoneDistGraph = New RadioButton()
        cmbWildGraph = New ComboBox()
        lblWildcard = New Label()
        cmbBPSGraph = New ComboBox()
        lblBPS = New Label()
        EVTDescription = New TabPage()
        txtEVTDescription = New TextBox()
        CH = New ColumnHeader()
        TabControl = New TabControl()
        Ruleset = New TabPage()
        lblPixelsLeftOver = New Label()
        txtNotes = New TextBox()
        lstVwRulesets = New ListView()
        ID = New ColumnHeader()
        RangeofCover = New ColumnHeader()
        RangeofHeight = New ColumnHeader()
        BPS = New ColumnHeader()
        Wild = New ColumnHeader()
        FM13 = New ColumnHeader()
        FM40 = New ColumnHeader()
        CanFM = New ColumnHeader()
        FCCS = New ColumnHeader()
        FLM = New ColumnHeader()
        CG = New ColumnHeader()
        CC = New ColumnHeader()
        CBD40 = New ColumnHeader()
        CBH40 = New ColumnHeader()
        CompareFM = New TabPage()
        cmdCustomFM = New Button()
        grpLiveFuelMoist = New GroupBox()
        rdoLM4 = New RadioButton()
        rdoLM3 = New RadioButton()
        rdoLM2 = New RadioButton()
        rdoLM1 = New RadioButton()
        grpDeadFuelMoist = New GroupBox()
        rdoDM4 = New RadioButton()
        rdoDM3 = New RadioButton()
        rdoDM2 = New RadioButton()
        rdoDM1 = New RadioButton()
        grpROSFL = New GroupBox()
        rdoCBH = New RadioButton()
        rdoFL = New RadioButton()
        rdoROS = New RadioButton()
        cmbSlope = New ComboBox()
        lblSlope = New Label()
        cmbFM4 = New ComboBox()
        cmbFM3 = New ComboBox()
        cmbFM2 = New ComboBox()
        cmbFM1 = New ComboBox()
        lblFuelModel = New Label()
        grpCustFM = New GroupBox()
        cmdDelCstFM = New Button()
        cmdSaveCSTFM = New Button()
        chkFMType = New CheckBox()
        lblDefaultFM = New Label()
        lblFMType = New Label()
        lblXtMoist = New Label()
        lblDepth = New Label()
        lblL_WSAV = New Label()
        lblL_HSAV = New Label()
        lbl1HSAV = New Label()
        lblLiveW = New Label()
        lblLiveH = New Label()
        lbl100H = New Label()
        lbl10H = New Label()
        lbl1H = New Label()
        cmbDefaultFM = New ComboBox()
        TrkBar = New TrackBar()
        rdoXtMoist = New RadioButton()
        rdoLiveW = New RadioButton()
        rdo1HSAV = New RadioButton()
        rdoLiveHSAV = New RadioButton()
        rdoLiveWSAV = New RadioButton()
        rdoDepth = New RadioButton()
        rdoLiveH = New RadioButton()
        rdo100H = New RadioButton()
        rdo10H = New RadioButton()
        rdo1H = New RadioButton()
        cmbEVT = New ComboBox()
        txtSessionName = New TextBox()
        cmsEditRule = New ContextMenuStrip(components)
        cmsLowHigh = New ContextMenuStrip(components)
        cmbSortRules = New ComboBox()
        grpEVTOrder = New GroupBox()
        rdoNumber = New RadioButton()
        rdoName = New RadioButton()
        grpSort = New GroupBox()
        cmdAutoRule = New Button()
        cmdCopyRule = New Button()
        cmdDeleteRule = New Button()
        cmdEditRule = New Button()
        cmdAddRule = New Button()
        TTSession = New ToolTip(components)
        lblDistCode = New Label()
        DistributionGraph.SuspendLayout()
        grpCanopyLines.SuspendLayout()
        EVTDescription.SuspendLayout()
        TabControl.SuspendLayout()
        Ruleset.SuspendLayout()
        CompareFM.SuspendLayout()
        grpLiveFuelMoist.SuspendLayout()
        grpDeadFuelMoist.SuspendLayout()
        grpROSFL.SuspendLayout()
        grpCustFM.SuspendLayout()
        CType(TrkBar, System.ComponentModel.ISupportInitialize).BeginInit()
        grpEVTOrder.SuspendLayout()
        grpSort.SuspendLayout()
        SuspendLayout()
        ' 
        ' PerEVT
        ' 
        PerEVT.Text = "% EVT"
        PerEVT.TextAlign = HorizontalAlignment.Center
        ' 
        ' CBD13
        ' 
        CBD13.Text = "CBD13"
        CBD13.TextAlign = HorizontalAlignment.Center
        CBD13.Width = 54
        ' 
        ' CBH13
        ' 
        CBH13.Text = "CBH13"
        CBH13.TextAlign = HorizontalAlignment.Center
        CBH13.Width = 56
        ' 
        ' OnOff
        ' 
        OnOff.Text = "On/Off"
        OnOff.TextAlign = HorizontalAlignment.Center
        OnOff.Width = 63
        ' 
        ' Acres
        ' 
        Acres.Text = "Acres"
        Acres.TextAlign = HorizontalAlignment.Center
        Acres.Width = 68
        ' 
        ' DistributionGraph
        ' 
        DistributionGraph.Controls.Add(grpCanopyLines)
        DistributionGraph.Controls.Add(cmbWildGraph)
        DistributionGraph.Controls.Add(lblWildcard)
        DistributionGraph.Controls.Add(cmbBPSGraph)
        DistributionGraph.Controls.Add(lblBPS)
        DistributionGraph.Location = New System.Drawing.Point(4, 24)
        DistributionGraph.Name = "DistributionGraph"
        DistributionGraph.Padding = New Padding(3)
        DistributionGraph.Size = New System.Drawing.Size(825, 438)
        DistributionGraph.TabIndex = 1
        DistributionGraph.Text = "Distribution Graph"
        DistributionGraph.UseVisualStyleBackColor = True
        ' 
        ' grpCanopyLines
        ' 
        grpCanopyLines.Controls.Add(rdoCHDistGraph)
        grpCanopyLines.Controls.Add(rdoCCDistGraph)
        grpCanopyLines.Controls.Add(rdoCBDDistGraph)
        grpCanopyLines.Controls.Add(rdoCBHDistGraph)
        grpCanopyLines.Controls.Add(rdoNoneDistGraph)
        grpCanopyLines.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        grpCanopyLines.Location = New System.Drawing.Point(1, 113)
        grpCanopyLines.Name = "grpCanopyLines"
        grpCanopyLines.Size = New System.Drawing.Size(672, 40)
        grpCanopyLines.TabIndex = 117
        grpCanopyLines.TabStop = False
        grpCanopyLines.Text = "Show lines of canopy fuel  that represent the EVT and disturbance select"
        ' 
        ' rdoCHDistGraph
        ' 
        rdoCHDistGraph.AutoSize = True
        rdoCHDistGraph.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoCHDistGraph.Location = New System.Drawing.Point(224, 15)
        rdoCHDistGraph.Name = "rdoCHDistGraph"
        rdoCHDistGraph.Size = New System.Drawing.Size(104, 21)
        rdoCHDistGraph.TabIndex = 5
        rdoCHDistGraph.Text = "Canopy Height"
        rdoCHDistGraph.UseVisualStyleBackColor = True
        ' 
        ' rdoCCDistGraph
        ' 
        rdoCCDistGraph.AutoSize = True
        rdoCCDistGraph.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoCCDistGraph.Location = New System.Drawing.Point(95, 15)
        rdoCCDistGraph.Name = "rdoCCDistGraph"
        rdoCCDistGraph.Size = New System.Drawing.Size(98, 21)
        rdoCCDistGraph.TabIndex = 4
        rdoCCDistGraph.Text = "Canopy Cover"
        rdoCCDistGraph.UseVisualStyleBackColor = True
        ' 
        ' rdoCBDDistGraph
        ' 
        rdoCBDDistGraph.AutoSize = True
        rdoCBDDistGraph.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoCBDDistGraph.Location = New System.Drawing.Point(524, 15)
        rdoCBDDistGraph.Name = "rdoCBDDistGraph"
        rdoCBDDistGraph.Size = New System.Drawing.Size(138, 21)
        rdoCBDDistGraph.TabIndex = 3
        rdoCBDDistGraph.Text = "Canopy Bulk Density"
        rdoCBDDistGraph.UseVisualStyleBackColor = True
        ' 
        ' rdoCBHDistGraph
        ' 
        rdoCBHDistGraph.AutoSize = True
        rdoCBHDistGraph.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoCBHDistGraph.Location = New System.Drawing.Point(359, 15)
        rdoCBHDistGraph.Name = "rdoCBHDistGraph"
        rdoCBHDistGraph.Size = New System.Drawing.Size(134, 21)
        rdoCBHDistGraph.TabIndex = 2
        rdoCBHDistGraph.Text = "Canopy Base Height"
        rdoCBHDistGraph.UseVisualStyleBackColor = True
        ' 
        ' rdoNoneDistGraph
        ' 
        rdoNoneDistGraph.AutoSize = True
        rdoNoneDistGraph.Checked = True
        rdoNoneDistGraph.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoNoneDistGraph.Location = New System.Drawing.Point(10, 15)
        rdoNoneDistGraph.Name = "rdoNoneDistGraph"
        rdoNoneDistGraph.Size = New System.Drawing.Size(54, 21)
        rdoNoneDistGraph.TabIndex = 1
        rdoNoneDistGraph.TabStop = True
        rdoNoneDistGraph.Text = "None"
        rdoNoneDistGraph.UseVisualStyleBackColor = True
        ' 
        ' cmbWildGraph
        ' 
        cmbWildGraph.FormattingEnabled = True
        cmbWildGraph.Location = New System.Drawing.Point(0, 79)
        cmbWildGraph.Margin = New Padding(5)
        cmbWildGraph.Name = "cmbWildGraph"
        cmbWildGraph.Size = New System.Drawing.Size(673, 31)
        cmbWildGraph.TabIndex = 13
        ' 
        ' lblWildcard
        ' 
        lblWildcard.BackColor = Drawing.Color.LightGreen
        lblWildcard.BorderStyle = BorderStyle.Fixed3D
        lblWildcard.FlatStyle = FlatStyle.Popup
        lblWildcard.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        lblWildcard.Location = New System.Drawing.Point(0, 58)
        lblWildcard.Margin = New Padding(5, 0, 5, 0)
        lblWildcard.Name = "lblWildcard"
        lblWildcard.Size = New System.Drawing.Size(673, 20)
        lblWildcard.TabIndex = 12
        lblWildcard.Text = "Wildcard"
        lblWildcard.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbBPSGraph
        ' 
        cmbBPSGraph.FormattingEnabled = True
        cmbBPSGraph.Location = New System.Drawing.Point(0, 22)
        cmbBPSGraph.Margin = New Padding(5)
        cmbBPSGraph.Name = "cmbBPSGraph"
        cmbBPSGraph.Size = New System.Drawing.Size(673, 31)
        cmbBPSGraph.TabIndex = 11
        ' 
        ' lblBPS
        ' 
        lblBPS.BackColor = Drawing.Color.LightGreen
        lblBPS.BorderStyle = BorderStyle.Fixed3D
        lblBPS.FlatStyle = FlatStyle.Popup
        lblBPS.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        lblBPS.Location = New System.Drawing.Point(0, 1)
        lblBPS.Margin = New Padding(5, 0, 5, 0)
        lblBPS.Name = "lblBPS"
        lblBPS.Size = New System.Drawing.Size(673, 20)
        lblBPS.TabIndex = 10
        lblBPS.Text = "Biophysical Setting Name and Model"
        lblBPS.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' EVTDescription
        ' 
        EVTDescription.Controls.Add(txtEVTDescription)
        EVTDescription.Location = New System.Drawing.Point(4, 24)
        EVTDescription.Name = "EVTDescription"
        EVTDescription.Padding = New Padding(3)
        EVTDescription.Size = New System.Drawing.Size(825, 438)
        EVTDescription.TabIndex = 2
        EVTDescription.Text = "EVT Description"
        EVTDescription.UseVisualStyleBackColor = True
        ' 
        ' txtEVTDescription
        ' 
        txtEVTDescription.Location = New System.Drawing.Point(3, 3)
        txtEVTDescription.Multiline = True
        txtEVTDescription.Name = "txtEVTDescription"
        txtEVTDescription.ReadOnly = True
        txtEVTDescription.ScrollBars = ScrollBars.Vertical
        txtEVTDescription.Size = New System.Drawing.Size(890, 378)
        txtEVTDescription.TabIndex = 0
        ' 
        ' CH
        ' 
        CH.Text = "CH"
        CH.TextAlign = HorizontalAlignment.Center
        CH.Width = 35
        ' 
        ' TabControl
        ' 
        TabControl.Controls.Add(Ruleset)
        TabControl.Controls.Add(CompareFM)
        TabControl.Controls.Add(DistributionGraph)
        TabControl.Controls.Add(EVTDescription)
        TabControl.Location = New System.Drawing.Point(108, 87)
        TabControl.Name = "TabControl"
        TabControl.SelectedIndex = 0
        TabControl.Size = New System.Drawing.Size(833, 466)
        TabControl.TabIndex = 23
        ' 
        ' Ruleset
        ' 
        Ruleset.Controls.Add(lblPixelsLeftOver)
        Ruleset.Controls.Add(txtNotes)
        Ruleset.Controls.Add(lstVwRulesets)
        Ruleset.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        Ruleset.Location = New System.Drawing.Point(4, 32)
        Ruleset.Name = "Ruleset"
        Ruleset.Padding = New Padding(3)
        Ruleset.Size = New System.Drawing.Size(825, 430)
        Ruleset.TabIndex = 0
        Ruleset.Text = "Ruleset"
        Ruleset.UseVisualStyleBackColor = True
        ' 
        ' lblPixelsLeftOver
        ' 
        lblPixelsLeftOver.BackColor = Drawing.Color.LightGreen
        lblPixelsLeftOver.BorderStyle = BorderStyle.Fixed3D
        lblPixelsLeftOver.FlatStyle = FlatStyle.Popup
        lblPixelsLeftOver.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        lblPixelsLeftOver.Location = New System.Drawing.Point(5, 2)
        lblPixelsLeftOver.Margin = New Padding(5, 0, 5, 0)
        lblPixelsLeftOver.Name = "lblPixelsLeftOver"
        lblPixelsLeftOver.Size = New System.Drawing.Size(897, 20)
        lblPixelsLeftOver.TabIndex = 40
        lblPixelsLeftOver.Text = "Pixels Left Behind"
        lblPixelsLeftOver.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' txtNotes
        ' 
        txtNotes.Location = New System.Drawing.Point(5, 272)
        txtNotes.Multiline = True
        txtNotes.Name = "txtNotes"
        txtNotes.ScrollBars = ScrollBars.Vertical
        txtNotes.Size = New System.Drawing.Size(902, 106)
        txtNotes.TabIndex = 39
        ' 
        ' lstVwRulesets
        ' 
        lstVwRulesets.AutoArrange = False
        lstVwRulesets.Columns.AddRange(New ColumnHeader() {ID, RangeofCover, RangeofHeight, BPS, Wild, FM13, FM40, CanFM, FCCS, FLM, CG, CC, CH, CBD13, CBD40, CBH13, CBH40, OnOff, Acres, PerEVT})
        lstVwRulesets.FullRowSelect = True
        lstVwRulesets.GridLines = True
        lstVwRulesets.Location = New System.Drawing.Point(5, 23)
        lstVwRulesets.Name = "lstVwRulesets"
        lstVwRulesets.Size = New System.Drawing.Size(905, 243)
        lstVwRulesets.TabIndex = 38
        lstVwRulesets.UseCompatibleStateImageBehavior = False
        lstVwRulesets.View = View.Details
        ' 
        ' ID
        ' 
        ID.Text = "ID"
        ID.Width = 0
        ' 
        ' RangeofCover
        ' 
        RangeofCover.Text = "Range of Cover"
        RangeofCover.TextAlign = HorizontalAlignment.Center
        RangeofCover.Width = 113
        ' 
        ' RangeofHeight
        ' 
        RangeofHeight.Text = "Range of Height"
        RangeofHeight.TextAlign = HorizontalAlignment.Center
        RangeofHeight.Width = 121
        ' 
        ' BPS
        ' 
        BPS.Text = "BPS"
        BPS.TextAlign = HorizontalAlignment.Center
        BPS.Width = 43
        ' 
        ' Wild
        ' 
        Wild.Text = "Wild"
        Wild.TextAlign = HorizontalAlignment.Center
        Wild.Width = 44
        ' 
        ' FM13
        ' 
        FM13.Text = "FM13"
        FM13.TextAlign = HorizontalAlignment.Center
        FM13.Width = 50
        ' 
        ' FM40
        ' 
        FM40.Text = "FM40"
        FM40.TextAlign = HorizontalAlignment.Center
        FM40.Width = 50
        ' 
        ' CanFM
        ' 
        CanFM.Text = "CanFM"
        CanFM.TextAlign = HorizontalAlignment.Center
        CanFM.Width = 56
        ' 
        ' FCCS
        ' 
        FCCS.Text = "FCCS"
        FCCS.TextAlign = HorizontalAlignment.Center
        FCCS.Width = 48
        ' 
        ' FLM
        ' 
        FLM.Text = "FLM"
        FLM.TextAlign = HorizontalAlignment.Center
        FLM.Width = 44
        ' 
        ' CG
        ' 
        CG.Text = "CG"
        CG.TextAlign = HorizontalAlignment.Center
        CG.Width = 52
        ' 
        ' CC
        ' 
        CC.Text = "CC"
        CC.TextAlign = HorizontalAlignment.Center
        CC.Width = 33
        ' 
        ' CBD40
        ' 
        CBD40.Text = "CBD40"
        CBD40.TextAlign = HorizontalAlignment.Center
        CBD40.Width = 56
        ' 
        ' CBH40
        ' 
        CBH40.Text = "CBH40"
        CBH40.TextAlign = HorizontalAlignment.Center
        CBH40.Width = 57
        ' 
        ' CompareFM
        ' 
        CompareFM.BackColor = Drawing.Color.Transparent
        CompareFM.Controls.Add(cmdCustomFM)
        CompareFM.Controls.Add(grpLiveFuelMoist)
        CompareFM.Controls.Add(grpDeadFuelMoist)
        CompareFM.Controls.Add(grpROSFL)
        CompareFM.Controls.Add(cmbSlope)
        CompareFM.Controls.Add(lblSlope)
        CompareFM.Controls.Add(cmbFM4)
        CompareFM.Controls.Add(cmbFM3)
        CompareFM.Controls.Add(cmbFM2)
        CompareFM.Controls.Add(cmbFM1)
        CompareFM.Controls.Add(lblFuelModel)
        CompareFM.Location = New System.Drawing.Point(4, 24)
        CompareFM.Name = "CompareFM"
        CompareFM.Size = New System.Drawing.Size(825, 438)
        CompareFM.TabIndex = 1
        CompareFM.Text = "Compare FM"
        ' 
        ' cmdCustomFM
        ' 
        cmdCustomFM.BackgroundImageLayout = ImageLayout.None
        cmdCustomFM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F)
        cmdCustomFM.Image = CType(resources.GetObject("cmdCustomFM.Image"), Drawing.Image)
        cmdCustomFM.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCustomFM.Location = New System.Drawing.Point(592, 52)
        cmdCustomFM.Margin = New Padding(5)
        cmdCustomFM.Name = "cmdCustomFM"
        cmdCustomFM.Size = New System.Drawing.Size(80, 37)
        cmdCustomFM.TabIndex = 120
        cmdCustomFM.Text = "Custom" & vbCrLf & "FM"
        cmdCustomFM.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCustomFM.UseVisualStyleBackColor = True
        ' 
        ' grpLiveFuelMoist
        ' 
        grpLiveFuelMoist.Controls.Add(rdoLM4)
        grpLiveFuelMoist.Controls.Add(rdoLM3)
        grpLiveFuelMoist.Controls.Add(rdoLM2)
        grpLiveFuelMoist.Controls.Add(rdoLM1)
        grpLiveFuelMoist.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        grpLiveFuelMoist.Location = New System.Drawing.Point(223, -1)
        grpLiveFuelMoist.Name = "grpLiveFuelMoist"
        grpLiveFuelMoist.Size = New System.Drawing.Size(135, 99)
        grpLiveFuelMoist.TabIndex = 118
        grpLiveFuelMoist.TabStop = False
        grpLiveFuelMoist.Text = "% Live Moisture"
        ' 
        ' rdoLM4
        ' 
        rdoLM4.AutoSize = True
        rdoLM4.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoLM4.Location = New System.Drawing.Point(4, 75)
        rdoLM4.Name = "rdoLM4"
        rdoLM4.Size = New System.Drawing.Size(125, 21)
        rdoLM4.TabIndex = 3
        rdoLM4.Text = "120 Hrb; 150 Wdy"
        rdoLM4.UseVisualStyleBackColor = True
        ' 
        ' rdoLM3
        ' 
        rdoLM3.AutoSize = True
        rdoLM3.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoLM3.Location = New System.Drawing.Point(4, 54)
        rdoLM3.Name = "rdoLM3"
        rdoLM3.Size = New System.Drawing.Size(120, 21)
        rdoLM3.TabIndex = 2
        rdoLM3.Text = "90 Hrb; 120 Wdy"
        rdoLM3.UseVisualStyleBackColor = True
        ' 
        ' rdoLM2
        ' 
        rdoLM2.AutoSize = True
        rdoLM2.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoLM2.Location = New System.Drawing.Point(4, 33)
        rdoLM2.Name = "rdoLM2"
        rdoLM2.Size = New System.Drawing.Size(115, 21)
        rdoLM2.TabIndex = 1
        rdoLM2.Text = "60 Hrb; 90 Wdy"
        rdoLM2.UseVisualStyleBackColor = True
        ' 
        ' rdoLM1
        ' 
        rdoLM1.AutoSize = True
        rdoLM1.Checked = True
        rdoLM1.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoLM1.Location = New System.Drawing.Point(4, 12)
        rdoLM1.Name = "rdoLM1"
        rdoLM1.Size = New System.Drawing.Size(115, 21)
        rdoLM1.TabIndex = 0
        rdoLM1.TabStop = True
        rdoLM1.Text = "30 Hrb; 60 Wdy"
        rdoLM1.UseVisualStyleBackColor = True
        ' 
        ' grpDeadFuelMoist
        ' 
        grpDeadFuelMoist.Controls.Add(rdoDM4)
        grpDeadFuelMoist.Controls.Add(rdoDM3)
        grpDeadFuelMoist.Controls.Add(rdoDM2)
        grpDeadFuelMoist.Controls.Add(rdoDM1)
        grpDeadFuelMoist.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        grpDeadFuelMoist.Location = New System.Drawing.Point(108, -1)
        grpDeadFuelMoist.Name = "grpDeadFuelMoist"
        grpDeadFuelMoist.Size = New System.Drawing.Size(115, 99)
        grpDeadFuelMoist.TabIndex = 117
        grpDeadFuelMoist.TabStop = False
        grpDeadFuelMoist.Text = "% Dead Moisture"
        ' 
        ' rdoDM4
        ' 
        rdoDM4.AutoSize = True
        rdoDM4.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoDM4.Location = New System.Drawing.Point(6, 75)
        rdoDM4.Name = "rdoDM4"
        rdoDM4.Size = New System.Drawing.Size(68, 21)
        rdoDM4.TabIndex = 3
        rdoDM4.Text = "12,13,14"
        rdoDM4.UseVisualStyleBackColor = True
        ' 
        ' rdoDM3
        ' 
        rdoDM3.AutoSize = True
        rdoDM3.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoDM3.Location = New System.Drawing.Point(6, 54)
        rdoDM3.Name = "rdoDM3"
        rdoDM3.Size = New System.Drawing.Size(61, 21)
        rdoDM3.TabIndex = 2
        rdoDM3.Text = "9,10,11"
        rdoDM3.UseVisualStyleBackColor = True
        ' 
        ' rdoDM2
        ' 
        rdoDM2.AutoSize = True
        rdoDM2.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoDM2.Location = New System.Drawing.Point(6, 33)
        rdoDM2.Name = "rdoDM2"
        rdoDM2.Size = New System.Drawing.Size(53, 21)
        rdoDM2.TabIndex = 1
        rdoDM2.Text = "6,7,8"
        rdoDM2.UseVisualStyleBackColor = True
        ' 
        ' rdoDM1
        ' 
        rdoDM1.AutoSize = True
        rdoDM1.Checked = True
        rdoDM1.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoDM1.Location = New System.Drawing.Point(6, 12)
        rdoDM1.Name = "rdoDM1"
        rdoDM1.Size = New System.Drawing.Size(53, 21)
        rdoDM1.TabIndex = 0
        rdoDM1.TabStop = True
        rdoDM1.Text = "3,4,5"
        rdoDM1.UseVisualStyleBackColor = True
        ' 
        ' grpROSFL
        ' 
        grpROSFL.Controls.Add(rdoCBH)
        grpROSFL.Controls.Add(rdoFL)
        grpROSFL.Controls.Add(rdoROS)
        grpROSFL.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        grpROSFL.Location = New System.Drawing.Point(4, -1)
        grpROSFL.Name = "grpROSFL"
        grpROSFL.Size = New System.Drawing.Size(104, 99)
        grpROSFL.TabIndex = 116
        grpROSFL.TabStop = False
        grpROSFL.Text = "ROS or FL"
        ' 
        ' rdoCBH
        ' 
        rdoCBH.AutoSize = True
        rdoCBH.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoCBH.Location = New System.Drawing.Point(5, 70)
        rdoCBH.Name = "rdoCBH"
        rdoCBH.Size = New System.Drawing.Size(94, 21)
        rdoCBH.TabIndex = 2
        rdoCBH.Text = "FL+ CBH(ft)"
        rdoCBH.UseVisualStyleBackColor = True
        ' 
        ' rdoFL
        ' 
        rdoFL.AutoSize = True
        rdoFL.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoFL.Location = New System.Drawing.Point(6, 43)
        rdoFL.Name = "rdoFL"
        rdoFL.Size = New System.Drawing.Size(60, 21)
        rdoFL.TabIndex = 1
        rdoFL.Text = "FL(ft)"
        rdoFL.UseVisualStyleBackColor = True
        ' 
        ' rdoROS
        ' 
        rdoROS.AutoSize = True
        rdoROS.Checked = True
        rdoROS.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoROS.Location = New System.Drawing.Point(5, 16)
        rdoROS.Name = "rdoROS"
        rdoROS.Size = New System.Drawing.Size(89, 21)
        rdoROS.TabIndex = 0
        rdoROS.TabStop = True
        rdoROS.Text = "ROS(ch/hr)"
        rdoROS.UseVisualStyleBackColor = True
        ' 
        ' cmbSlope
        ' 
        cmbSlope.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSlope.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        cmbSlope.FormattingEnabled = True
        cmbSlope.Location = New System.Drawing.Point(363, 26)
        cmbSlope.Name = "cmbSlope"
        cmbSlope.Size = New System.Drawing.Size(59, 26)
        cmbSlope.TabIndex = 75
        ' 
        ' lblSlope
        ' 
        lblSlope.BackColor = Drawing.Color.FromArgb(CByte(128), CByte(255), CByte(255))
        lblSlope.BorderStyle = BorderStyle.Fixed3D
        lblSlope.FlatStyle = FlatStyle.Popup
        lblSlope.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        lblSlope.Location = New System.Drawing.Point(363, 3)
        lblSlope.Margin = New Padding(5, 0, 5, 0)
        lblSlope.Name = "lblSlope"
        lblSlope.Size = New System.Drawing.Size(60, 20)
        lblSlope.TabIndex = 74
        lblSlope.Text = "% Slope"
        lblSlope.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbFM4
        ' 
        cmbFM4.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFM4.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        cmbFM4.FormattingEnabled = True
        cmbFM4.Location = New System.Drawing.Point(612, 25)
        cmbFM4.Name = "cmbFM4"
        cmbFM4.Size = New System.Drawing.Size(60, 26)
        cmbFM4.TabIndex = 72
        ' 
        ' cmbFM3
        ' 
        cmbFM3.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFM3.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        cmbFM3.FormattingEnabled = True
        cmbFM3.Location = New System.Drawing.Point(551, 25)
        cmbFM3.Name = "cmbFM3"
        cmbFM3.Size = New System.Drawing.Size(60, 26)
        cmbFM3.TabIndex = 71
        ' 
        ' cmbFM2
        ' 
        cmbFM2.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFM2.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        cmbFM2.FormattingEnabled = True
        cmbFM2.Location = New System.Drawing.Point(490, 25)
        cmbFM2.Name = "cmbFM2"
        cmbFM2.Size = New System.Drawing.Size(60, 26)
        cmbFM2.TabIndex = 70
        ' 
        ' cmbFM1
        ' 
        cmbFM1.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFM1.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        cmbFM1.FormattingEnabled = True
        cmbFM1.Location = New System.Drawing.Point(429, 25)
        cmbFM1.Name = "cmbFM1"
        cmbFM1.Size = New System.Drawing.Size(60, 26)
        cmbFM1.TabIndex = 69
        ' 
        ' lblFuelModel
        ' 
        lblFuelModel.BackColor = Drawing.Color.FromArgb(CByte(128), CByte(255), CByte(255))
        lblFuelModel.BorderStyle = BorderStyle.Fixed3D
        lblFuelModel.FlatStyle = FlatStyle.Popup
        lblFuelModel.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        lblFuelModel.Location = New System.Drawing.Point(429, 3)
        lblFuelModel.Margin = New Padding(5, 0, 5, 0)
        lblFuelModel.Name = "lblFuelModel"
        lblFuelModel.Size = New System.Drawing.Size(244, 20)
        lblFuelModel.TabIndex = 68
        lblFuelModel.Text = "Compare fuel models"
        lblFuelModel.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' grpCustFM
        ' 
        grpCustFM.BackColor = Drawing.SystemColors.ButtonFace
        grpCustFM.Controls.Add(cmdDelCstFM)
        grpCustFM.Controls.Add(cmdSaveCSTFM)
        grpCustFM.Controls.Add(chkFMType)
        grpCustFM.Controls.Add(lblDefaultFM)
        grpCustFM.Controls.Add(lblFMType)
        grpCustFM.Controls.Add(lblXtMoist)
        grpCustFM.Controls.Add(lblDepth)
        grpCustFM.Controls.Add(lblL_WSAV)
        grpCustFM.Controls.Add(lblL_HSAV)
        grpCustFM.Controls.Add(lbl1HSAV)
        grpCustFM.Controls.Add(lblLiveW)
        grpCustFM.Controls.Add(lblLiveH)
        grpCustFM.Controls.Add(lbl100H)
        grpCustFM.Controls.Add(lbl10H)
        grpCustFM.Controls.Add(lbl1H)
        grpCustFM.Controls.Add(cmbDefaultFM)
        grpCustFM.Controls.Add(TrkBar)
        grpCustFM.Controls.Add(rdoXtMoist)
        grpCustFM.Controls.Add(rdoLiveW)
        grpCustFM.Controls.Add(rdo1HSAV)
        grpCustFM.Controls.Add(rdoLiveHSAV)
        grpCustFM.Controls.Add(rdoLiveWSAV)
        grpCustFM.Controls.Add(rdoDepth)
        grpCustFM.Controls.Add(rdoLiveH)
        grpCustFM.Controls.Add(rdo100H)
        grpCustFM.Controls.Add(rdo10H)
        grpCustFM.Controls.Add(rdo1H)
        grpCustFM.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        grpCustFM.Location = New System.Drawing.Point(0, 2)
        grpCustFM.Name = "grpCustFM"
        grpCustFM.Size = New System.Drawing.Size(797, 111)
        grpCustFM.TabIndex = 121
        grpCustFM.TabStop = False
        grpCustFM.Text = "Select a starting fuel model / Make adjustments / Check the graph for fire behavior results"
        grpCustFM.Visible = False
        ' 
        ' cmdDelCstFM
        ' 
        cmdDelCstFM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F)
        cmdDelCstFM.Image = CType(resources.GetObject("cmdDelCstFM.Image"), Drawing.Image)
        cmdDelCstFM.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdDelCstFM.Location = New System.Drawing.Point(545, 70)
        cmdDelCstFM.Margin = New Padding(5)
        cmdDelCstFM.Name = "cmdDelCstFM"
        cmdDelCstFM.Size = New System.Drawing.Size(119, 37)
        cmdDelCstFM.TabIndex = 86
        cmdDelCstFM.Text = "Delete selected" & vbCrLf & "Fuel Model    " & vbCrLf
        cmdDelCstFM.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdDelCstFM.UseVisualStyleBackColor = True
        ' 
        ' cmdSaveCSTFM
        ' 
        cmdSaveCSTFM.Image = CType(resources.GetObject("cmdSaveCSTFM.Image"), Drawing.Image)
        cmdSaveCSTFM.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdSaveCSTFM.Location = New System.Drawing.Point(668, 68)
        cmdSaveCSTFM.Margin = New Padding(5)
        cmdSaveCSTFM.Name = "cmdSaveCSTFM"
        cmdSaveCSTFM.Size = New System.Drawing.Size(122, 39)
        cmdSaveCSTFM.TabIndex = 85
        cmdSaveCSTFM.Text = "Save  Custom " & vbCrLf & "Fuel Model As"
        cmdSaveCSTFM.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdSaveCSTFM.UseVisualStyleBackColor = True
        ' 
        ' chkFMType
        ' 
        chkFMType.AutoSize = True
        chkFMType.Checked = True
        chkFMType.CheckState = CheckState.Checked
        chkFMType.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        chkFMType.Location = New System.Drawing.Point(339, 38)
        chkFMType.Name = "chkFMType"
        chkFMType.Size = New System.Drawing.Size(77, 22)
        chkFMType.TabIndex = 84
        chkFMType.Text = "Dynamic"
        chkFMType.UseVisualStyleBackColor = True
        ' 
        ' lblDefaultFM
        ' 
        lblDefaultFM.BackColor = Drawing.Color.Cyan
        lblDefaultFM.BorderStyle = BorderStyle.Fixed3D
        lblDefaultFM.FlatStyle = FlatStyle.Popup
        lblDefaultFM.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        lblDefaultFM.Location = New System.Drawing.Point(339, 75)
        lblDefaultFM.Margin = New Padding(5, 0, 5, 0)
        lblDefaultFM.Name = "lblDefaultFM"
        lblDefaultFM.Size = New System.Drawing.Size(97, 26)
        lblDefaultFM.TabIndex = 83
        lblDefaultFM.Text = "Fuel Model"
        lblDefaultFM.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblFMType
        ' 
        lblFMType.BackColor = Drawing.Color.Cyan
        lblFMType.BorderStyle = BorderStyle.Fixed3D
        lblFMType.FlatStyle = FlatStyle.Popup
        lblFMType.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lblFMType.Location = New System.Drawing.Point(339, 16)
        lblFMType.Margin = New Padding(5, 0, 5, 0)
        lblFMType.Name = "lblFMType"
        lblFMType.Size = New System.Drawing.Size(77, 20)
        lblFMType.TabIndex = 82
        lblFMType.Text = "FMType"
        lblFMType.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblXtMoist
        ' 
        lblXtMoist.BackColor = Drawing.Color.Cyan
        lblXtMoist.BorderStyle = BorderStyle.Fixed3D
        lblXtMoist.FlatStyle = FlatStyle.Popup
        lblXtMoist.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lblXtMoist.Location = New System.Drawing.Point(687, 16)
        lblXtMoist.Margin = New Padding(5, 0, 5, 0)
        lblXtMoist.Name = "lblXtMoist"
        lblXtMoist.Size = New System.Drawing.Size(64, 20)
        lblXtMoist.TabIndex = 81
        lblXtMoist.Text = "XtMoist %"
        lblXtMoist.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblDepth
        ' 
        lblDepth.BackColor = Drawing.Color.Cyan
        lblDepth.BorderStyle = BorderStyle.Fixed3D
        lblDepth.FlatStyle = FlatStyle.Popup
        lblDepth.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lblDepth.Location = New System.Drawing.Point(620, 16)
        lblDepth.Margin = New Padding(5, 0, 5, 0)
        lblDepth.Name = "lblDepth"
        lblDepth.Size = New System.Drawing.Size(64, 20)
        lblDepth.TabIndex = 80
        lblDepth.Text = "Depth ft"
        lblDepth.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblL_WSAV
        ' 
        lblL_WSAV.BackColor = Drawing.Color.Cyan
        lblL_WSAV.BorderStyle = BorderStyle.Fixed3D
        lblL_WSAV.FlatStyle = FlatStyle.Popup
        lblL_WSAV.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lblL_WSAV.Location = New System.Drawing.Point(553, 16)
        lblL_WSAV.Margin = New Padding(5, 0, 5, 0)
        lblL_WSAV.Name = "lblL_WSAV"
        lblL_WSAV.Size = New System.Drawing.Size(64, 20)
        lblL_WSAV.TabIndex = 79
        lblL_WSAV.Text = "L_WSAV"
        lblL_WSAV.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblL_HSAV
        ' 
        lblL_HSAV.BackColor = Drawing.Color.Cyan
        lblL_HSAV.BorderStyle = BorderStyle.Fixed3D
        lblL_HSAV.FlatStyle = FlatStyle.Popup
        lblL_HSAV.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lblL_HSAV.Location = New System.Drawing.Point(486, 16)
        lblL_HSAV.Margin = New Padding(5, 0, 5, 0)
        lblL_HSAV.Name = "lblL_HSAV"
        lblL_HSAV.Size = New System.Drawing.Size(64, 20)
        lblL_HSAV.TabIndex = 78
        lblL_HSAV.Text = "L_HSAV"
        lblL_HSAV.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lbl1HSAV
        ' 
        lbl1HSAV.BackColor = Drawing.Color.Cyan
        lbl1HSAV.BorderStyle = BorderStyle.Fixed3D
        lbl1HSAV.FlatStyle = FlatStyle.Popup
        lbl1HSAV.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lbl1HSAV.Location = New System.Drawing.Point(419, 16)
        lbl1HSAV.Margin = New Padding(5, 0, 5, 0)
        lbl1HSAV.Name = "lbl1HSAV"
        lbl1HSAV.Size = New System.Drawing.Size(64, 20)
        lbl1HSAV.TabIndex = 77
        lbl1HSAV.Text = "1HSAV"
        lbl1HSAV.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblLiveW
        ' 
        lblLiveW.BackColor = Drawing.Color.Cyan
        lblLiveW.BorderStyle = BorderStyle.Fixed3D
        lblLiveW.FlatStyle = FlatStyle.Popup
        lblLiveW.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lblLiveW.Location = New System.Drawing.Point(272, 16)
        lblLiveW.Margin = New Padding(5, 0, 5, 0)
        lblLiveW.Name = "lblLiveW"
        lblLiveW.Size = New System.Drawing.Size(64, 20)
        lblLiveW.TabIndex = 75
        lblLiveW.Text = "LiveW t/a"
        lblLiveW.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblLiveH
        ' 
        lblLiveH.BackColor = Drawing.Color.Cyan
        lblLiveH.BorderStyle = BorderStyle.Fixed3D
        lblLiveH.FlatStyle = FlatStyle.Popup
        lblLiveH.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lblLiveH.Location = New System.Drawing.Point(205, 16)
        lblLiveH.Margin = New Padding(5, 0, 5, 0)
        lblLiveH.Name = "lblLiveH"
        lblLiveH.Size = New System.Drawing.Size(64, 20)
        lblLiveH.TabIndex = 74
        lblLiveH.Text = "LiveH t/a "
        lblLiveH.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lbl100H
        ' 
        lbl100H.BackColor = Drawing.Color.Cyan
        lbl100H.BorderStyle = BorderStyle.Fixed3D
        lbl100H.FlatStyle = FlatStyle.Popup
        lbl100H.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lbl100H.Location = New System.Drawing.Point(138, 16)
        lbl100H.Margin = New Padding(5, 0, 5, 0)
        lbl100H.Name = "lbl100H"
        lbl100H.Size = New System.Drawing.Size(64, 20)
        lbl100H.TabIndex = 73
        lbl100H.Text = "100H t/a"
        lbl100H.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lbl10H
        ' 
        lbl10H.BackColor = Drawing.Color.Cyan
        lbl10H.BorderStyle = BorderStyle.Fixed3D
        lbl10H.FlatStyle = FlatStyle.Popup
        lbl10H.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lbl10H.Location = New System.Drawing.Point(71, 16)
        lbl10H.Margin = New Padding(5, 0, 5, 0)
        lbl10H.Name = "lbl10H"
        lbl10H.Size = New System.Drawing.Size(64, 20)
        lbl10H.TabIndex = 72
        lbl10H.Text = "10H t/a"
        lbl10H.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lbl1H
        ' 
        lbl1H.BackColor = Drawing.Color.Cyan
        lbl1H.BorderStyle = BorderStyle.Fixed3D
        lbl1H.FlatStyle = FlatStyle.Popup
        lbl1H.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        lbl1H.Location = New System.Drawing.Point(4, 16)
        lbl1H.Margin = New Padding(5, 0, 5, 0)
        lbl1H.Name = "lbl1H"
        lbl1H.Size = New System.Drawing.Size(64, 20)
        lbl1H.TabIndex = 71
        lbl1H.Text = "1H t/a"
        lbl1H.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbDefaultFM
        ' 
        cmbDefaultFM.DropDownStyle = ComboBoxStyle.DropDownList
        cmbDefaultFM.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        cmbDefaultFM.FormattingEnabled = True
        cmbDefaultFM.Location = New System.Drawing.Point(440, 75)
        cmbDefaultFM.Name = "cmbDefaultFM"
        cmbDefaultFM.Size = New System.Drawing.Size(97, 26)
        cmbDefaultFM.TabIndex = 70
        ' 
        ' TrkBar
        ' 
        TrkBar.BackColor = Drawing.Color.Cyan
        TrkBar.Cursor = Cursors.Hand
        TrkBar.Location = New System.Drawing.Point(4, 65)
        TrkBar.Margin = New Padding(0)
        TrkBar.Name = "TrkBar"
        TrkBar.Size = New System.Drawing.Size(332, 45)
        TrkBar.TabIndex = 24
        TrkBar.TickStyle = TickStyle.Both
        ' 
        ' rdoXtMoist
        ' 
        rdoXtMoist.AutoSize = True
        rdoXtMoist.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoXtMoist.Location = New System.Drawing.Point(687, 39)
        rdoXtMoist.Name = "rdoXtMoist"
        rdoXtMoist.Size = New System.Drawing.Size(61, 21)
        rdoXtMoist.TabIndex = 23
        rdoXtMoist.Text = "00000"
        rdoXtMoist.UseVisualStyleBackColor = True
        ' 
        ' rdoLiveW
        ' 
        rdoLiveW.AutoSize = True
        rdoLiveW.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoLiveW.Location = New System.Drawing.Point(272, 39)
        rdoLiveW.Name = "rdoLiveW"
        rdoLiveW.Size = New System.Drawing.Size(61, 21)
        rdoLiveW.TabIndex = 9
        rdoLiveW.Text = "00000"
        rdoLiveW.UseVisualStyleBackColor = True
        ' 
        ' rdo1HSAV
        ' 
        rdo1HSAV.AutoSize = True
        rdo1HSAV.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdo1HSAV.Location = New System.Drawing.Point(419, 39)
        rdo1HSAV.Name = "rdo1HSAV"
        rdo1HSAV.Size = New System.Drawing.Size(61, 21)
        rdo1HSAV.TabIndex = 7
        rdo1HSAV.Text = "00000"
        rdo1HSAV.UseVisualStyleBackColor = True
        ' 
        ' rdoLiveHSAV
        ' 
        rdoLiveHSAV.AutoSize = True
        rdoLiveHSAV.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoLiveHSAV.Location = New System.Drawing.Point(486, 39)
        rdoLiveHSAV.Name = "rdoLiveHSAV"
        rdoLiveHSAV.Size = New System.Drawing.Size(61, 21)
        rdoLiveHSAV.TabIndex = 6
        rdoLiveHSAV.Text = "00000"
        rdoLiveHSAV.UseVisualStyleBackColor = True
        ' 
        ' rdoLiveWSAV
        ' 
        rdoLiveWSAV.AutoSize = True
        rdoLiveWSAV.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoLiveWSAV.Location = New System.Drawing.Point(553, 39)
        rdoLiveWSAV.Name = "rdoLiveWSAV"
        rdoLiveWSAV.Size = New System.Drawing.Size(61, 21)
        rdoLiveWSAV.TabIndex = 5
        rdoLiveWSAV.Text = "00000"
        rdoLiveWSAV.UseVisualStyleBackColor = True
        ' 
        ' rdoDepth
        ' 
        rdoDepth.AutoSize = True
        rdoDepth.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoDepth.Location = New System.Drawing.Point(620, 39)
        rdoDepth.Name = "rdoDepth"
        rdoDepth.Size = New System.Drawing.Size(61, 21)
        rdoDepth.TabIndex = 4
        rdoDepth.Text = "00000"
        rdoDepth.UseVisualStyleBackColor = True
        ' 
        ' rdoLiveH
        ' 
        rdoLiveH.AutoSize = True
        rdoLiveH.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoLiveH.Location = New System.Drawing.Point(205, 39)
        rdoLiveH.Name = "rdoLiveH"
        rdoLiveH.Size = New System.Drawing.Size(61, 21)
        rdoLiveH.TabIndex = 3
        rdoLiveH.Text = "00000"
        rdoLiveH.UseVisualStyleBackColor = True
        ' 
        ' rdo100H
        ' 
        rdo100H.AutoSize = True
        rdo100H.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdo100H.Location = New System.Drawing.Point(138, 39)
        rdo100H.Name = "rdo100H"
        rdo100H.Size = New System.Drawing.Size(61, 21)
        rdo100H.TabIndex = 2
        rdo100H.Text = "00000"
        rdo100H.UseVisualStyleBackColor = True
        ' 
        ' rdo10H
        ' 
        rdo10H.AutoSize = True
        rdo10H.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdo10H.Location = New System.Drawing.Point(71, 39)
        rdo10H.Name = "rdo10H"
        rdo10H.Size = New System.Drawing.Size(61, 21)
        rdo10H.TabIndex = 1
        rdo10H.Text = "00000"
        rdo10H.UseVisualStyleBackColor = True
        ' 
        ' rdo1H
        ' 
        rdo1H.AutoSize = True
        rdo1H.Checked = True
        rdo1H.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdo1H.Location = New System.Drawing.Point(4, 39)
        rdo1H.Name = "rdo1H"
        rdo1H.Size = New System.Drawing.Size(61, 21)
        rdo1H.TabIndex = 0
        rdo1H.TabStop = True
        rdo1H.Text = "00000"
        rdo1H.UseVisualStyleBackColor = True
        ' 
        ' cmbEVT
        ' 
        cmbEVT.AutoCompleteMode = AutoCompleteMode.Append
        cmbEVT.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbEVT.BackColor = Drawing.SystemColors.Info
        cmbEVT.DropDownStyle = ComboBoxStyle.DropDownList
        cmbEVT.FormattingEnabled = True
        cmbEVT.Location = New System.Drawing.Point(0, 49)
        cmbEVT.Margin = New Padding(5)
        cmbEVT.Name = "cmbEVT"
        cmbEVT.Size = New System.Drawing.Size(941, 31)
        cmbEVT.TabIndex = 22
        ' 
        ' txtSessionName
        ' 
        txtSessionName.BackColor = Drawing.Color.White
        txtSessionName.Location = New System.Drawing.Point(296, 14)
        txtSessionName.Name = "txtSessionName"
        txtSessionName.Size = New System.Drawing.Size(180, 30)
        txtSessionName.TabIndex = 20
        txtSessionName.Text = "New Session Name"
        ' 
        ' cmsEditRule
        ' 
        cmsEditRule.AccessibleRole = AccessibleRole.ComboBox
        cmsEditRule.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        cmsEditRule.Font = New System.Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Bold)
        cmsEditRule.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow
        cmsEditRule.MaximumSize = New System.Drawing.Size(0, 550)
        cmsEditRule.Name = "cmsEditRule"
        cmsEditRule.ShowImageMargin = False
        cmsEditRule.Size = New System.Drawing.Size(36, 4)
        ' 
        ' cmsLowHigh
        ' 
        cmsLowHigh.AccessibleRole = AccessibleRole.ComboBox
        cmsLowHigh.BackColor = Drawing.Color.FromArgb(CByte(192), CByte(255), CByte(192))
        cmsLowHigh.Font = New System.Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Bold)
        cmsLowHigh.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow
        cmsLowHigh.Name = "cmsLowHigh"
        cmsLowHigh.ShowImageMargin = False
        cmsLowHigh.Size = New System.Drawing.Size(36, 4)
        ' 
        ' cmbSortRules
        ' 
        cmbSortRules.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSortRules.Font = New System.Drawing.Font("Comic Sans MS", 9.75F)
        cmbSortRules.FormattingEnabled = True
        cmbSortRules.Location = New System.Drawing.Point(4, 15)
        cmbSortRules.Name = "cmbSortRules"
        cmbSortRules.Size = New System.Drawing.Size(136, 26)
        cmbSortRules.TabIndex = 71
        ' 
        ' grpEVTOrder
        ' 
        grpEVTOrder.Controls.Add(rdoNumber)
        grpEVTOrder.Controls.Add(rdoName)
        grpEVTOrder.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        grpEVTOrder.Location = New System.Drawing.Point(0, 3)
        grpEVTOrder.Name = "grpEVTOrder"
        grpEVTOrder.Size = New System.Drawing.Size(139, 41)
        grpEVTOrder.TabIndex = 120
        grpEVTOrder.TabStop = False
        grpEVTOrder.Text = "Order EVT by"
        ' 
        ' rdoNumber
        ' 
        rdoNumber.AutoSize = True
        rdoNumber.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoNumber.Location = New System.Drawing.Point(65, 14)
        rdoNumber.Name = "rdoNumber"
        rdoNumber.Size = New System.Drawing.Size(68, 21)
        rdoNumber.TabIndex = 2
        rdoNumber.Text = "Number"
        rdoNumber.UseVisualStyleBackColor = True
        ' 
        ' rdoName
        ' 
        rdoName.AutoSize = True
        rdoName.Checked = True
        rdoName.Font = New System.Drawing.Font("Comic Sans MS", 9F)
        rdoName.Location = New System.Drawing.Point(4, 14)
        rdoName.Name = "rdoName"
        rdoName.Size = New System.Drawing.Size(56, 21)
        rdoName.TabIndex = 0
        rdoName.TabStop = True
        rdoName.Text = "Name"
        rdoName.UseVisualStyleBackColor = True
        ' 
        ' grpSort
        ' 
        grpSort.Controls.Add(cmbSortRules)
        grpSort.Font = New System.Drawing.Font("Comic Sans MS", 8.25F)
        grpSort.Location = New System.Drawing.Point(145, 3)
        grpSort.Name = "grpSort"
        grpSort.Size = New System.Drawing.Size(145, 42)
        grpSort.TabIndex = 121
        grpSort.TabStop = False
        grpSort.Text = "Filter EVT by"
        ' 
        ' cmdAutoRule
        ' 
        cmdAutoRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F)
        cmdAutoRule.Image = CType(resources.GetObject("cmdAutoRule.Image"), Drawing.Image)
        cmdAutoRule.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdAutoRule.Location = New System.Drawing.Point(0, 280)
        cmdAutoRule.Margin = New Padding(5)
        cmdAutoRule.Name = "cmdAutoRule"
        cmdAutoRule.Size = New System.Drawing.Size(102, 36)
        cmdAutoRule.TabIndex = 12
        cmdAutoRule.Text = "Auto Rule" & vbCrLf & "Ctrl + U  "
        cmdAutoRule.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdAutoRule.UseVisualStyleBackColor = True
        cmdAutoRule.Visible = False
        ' 
        ' cmdCopyRule
        ' 
        cmdCopyRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F)
        cmdCopyRule.Image = CType(resources.GetObject("cmdCopyRule.Image"), Drawing.Image)
        cmdCopyRule.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCopyRule.Location = New System.Drawing.Point(0, 240)
        cmdCopyRule.Margin = New Padding(5)
        cmdCopyRule.Name = "cmdCopyRule"
        cmdCopyRule.Size = New System.Drawing.Size(102, 36)
        cmdCopyRule.TabIndex = 16
        cmdCopyRule.Text = "Copy Rule" & vbCrLf & "Ctrl + C  "
        cmdCopyRule.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCopyRule.UseVisualStyleBackColor = True
        ' 
        ' cmdDeleteRule
        ' 
        cmdDeleteRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F)
        cmdDeleteRule.Image = CType(resources.GetObject("cmdDeleteRule.Image"), Drawing.Image)
        cmdDeleteRule.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdDeleteRule.Location = New System.Drawing.Point(0, 200)
        cmdDeleteRule.Margin = New Padding(5)
        cmdDeleteRule.Name = "cmdDeleteRule"
        cmdDeleteRule.Size = New System.Drawing.Size(102, 36)
        cmdDeleteRule.TabIndex = 15
        cmdDeleteRule.Text = "Delete Rule" & vbCrLf & "Ctrl + D  "
        cmdDeleteRule.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdDeleteRule.UseVisualStyleBackColor = True
        ' 
        ' cmdEditRule
        ' 
        cmdEditRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F)
        cmdEditRule.Image = CType(resources.GetObject("cmdEditRule.Image"), Drawing.Image)
        cmdEditRule.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdEditRule.Location = New System.Drawing.Point(0, 160)
        cmdEditRule.Margin = New Padding(5)
        cmdEditRule.Name = "cmdEditRule"
        cmdEditRule.Size = New System.Drawing.Size(102, 36)
        cmdEditRule.TabIndex = 14
        cmdEditRule.Text = "Edit Rule" & vbCrLf & "Ctrl + E  "
        cmdEditRule.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdEditRule.UseVisualStyleBackColor = True
        ' 
        ' cmdAddRule
        ' 
        cmdAddRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F)
        cmdAddRule.Image = CType(resources.GetObject("cmdAddRule.Image"), Drawing.Image)
        cmdAddRule.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdAddRule.Location = New System.Drawing.Point(0, 120)
        cmdAddRule.Margin = New Padding(5)
        cmdAddRule.Name = "cmdAddRule"
        cmdAddRule.Size = New System.Drawing.Size(102, 36)
        cmdAddRule.TabIndex = 13
        cmdAddRule.Text = "Add Rule" & vbCrLf & "Ctrl + A  "
        cmdAddRule.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdAddRule.UseVisualStyleBackColor = True
        ' 
        ' lblDistCode
        ' 
        lblDistCode.BackColor = Drawing.SystemColors.Menu
        lblDistCode.FlatStyle = FlatStyle.Popup
        lblDistCode.Font = New System.Drawing.Font("Comic Sans MS", 12F)
        lblDistCode.Location = New System.Drawing.Point(484, 14)
        lblDistCode.Margin = New Padding(5, 0, 5, 0)
        lblDistCode.Name = "lblDistCode"
        lblDistCode.Size = New System.Drawing.Size(457, 30)
        lblDistCode.TabIndex = 122
        lblDistCode.Text = "Not disturbed"
        lblDistCode.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' frmRule
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(10F, 23F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        ClientSize = New System.Drawing.Size(944, 555)
        Controls.Add(grpCustFM)
        Controls.Add(lblDistCode)
        Controls.Add(cmdAutoRule)
        Controls.Add(txtSessionName)
        Controls.Add(cmdCopyRule)
        Controls.Add(cmdDeleteRule)
        Controls.Add(cmdEditRule)
        Controls.Add(cmdAddRule)
        Controls.Add(TabControl)
        Controls.Add(cmbEVT)
        Controls.Add(grpEVTOrder)
        Controls.Add(grpSort)
        Font = New System.Drawing.Font("Comic Sans MS", 12F)
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New Padding(5)
        MinimumSize = New System.Drawing.Size(825, 572)
        Name = "frmRule"
        SizeGripStyle = SizeGripStyle.Show
        Text = "FUEL Rules"
        DistributionGraph.ResumeLayout(False)
        grpCanopyLines.ResumeLayout(False)
        grpCanopyLines.PerformLayout()
        EVTDescription.ResumeLayout(False)
        EVTDescription.PerformLayout()
        TabControl.ResumeLayout(False)
        Ruleset.ResumeLayout(False)
        Ruleset.PerformLayout()
        CompareFM.ResumeLayout(False)
        grpLiveFuelMoist.ResumeLayout(False)
        grpLiveFuelMoist.PerformLayout()
        grpDeadFuelMoist.ResumeLayout(False)
        grpDeadFuelMoist.PerformLayout()
        grpROSFL.ResumeLayout(False)
        grpROSFL.PerformLayout()
        grpCustFM.ResumeLayout(False)
        grpCustFM.PerformLayout()
        CType(TrkBar, System.ComponentModel.ISupportInitialize).EndInit()
        grpEVTOrder.ResumeLayout(False)
        grpEVTOrder.PerformLayout()
        grpSort.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Private WithEvents cmdAutoRule As Button
    Friend WithEvents PerEVT As ColumnHeader
    Friend WithEvents CBD13 As ColumnHeader
    Friend WithEvents CBH13 As ColumnHeader
    Friend WithEvents OnOff As ColumnHeader
    Friend WithEvents Acres As ColumnHeader
    Friend WithEvents DistributionGraph As TabPage
    Friend WithEvents cmbWildGraph As ComboBox
    Private WithEvents lblWildcard As Label
    Friend WithEvents cmbBPSGraph As ComboBox
    Private WithEvents lblBPS As Label
    Friend WithEvents EVTDescription As TabPage
    Friend WithEvents txtEVTDescription As TextBox
    Friend WithEvents CH As ColumnHeader
    Friend WithEvents TabControl As TabControl
    Friend WithEvents Ruleset As TabPage
    Friend WithEvents txtNotes As TextBox
    Friend WithEvents lstVwRulesets As ListView
    Friend WithEvents ID As ColumnHeader
    Friend WithEvents RangeofCover As ColumnHeader
    Friend WithEvents RangeofHeight As ColumnHeader
    Friend WithEvents BPS As ColumnHeader
    Friend WithEvents Wild As ColumnHeader
    Friend WithEvents FM13 As ColumnHeader
    Friend WithEvents FCCS As ColumnHeader
    Friend WithEvents CG As ColumnHeader
    Friend WithEvents CC As ColumnHeader
    Friend WithEvents cmbEVT As ComboBox
    Friend WithEvents txtSessionName As TextBox
    Private WithEvents cmdCopyRule As Button
    Private WithEvents cmdDeleteRule As Button
    Private WithEvents cmdEditRule As Button
    Private WithEvents cmdAddRule As Button
    Friend WithEvents CompareFM As TabPage
    Friend WithEvents cmbFM4 As ComboBox
    Friend WithEvents cmbFM3 As ComboBox
    Friend WithEvents cmbFM2 As ComboBox
    Friend WithEvents cmbFM1 As ComboBox
    Friend WithEvents lblFuelModel As Label
    Friend WithEvents cmbSlope As ComboBox
    Friend WithEvents lblSlope As Label
    Friend WithEvents grpROSFL As GroupBox
    Friend WithEvents rdoFL As RadioButton
    Friend WithEvents rdoROS As RadioButton
    Friend WithEvents grpDeadFuelMoist As GroupBox
    Friend WithEvents rdoDM2 As RadioButton
    Friend WithEvents rdoDM1 As RadioButton
    Friend WithEvents rdoDM4 As RadioButton
    Friend WithEvents rdoDM3 As RadioButton
    Friend WithEvents grpLiveFuelMoist As GroupBox
    Friend WithEvents rdoLM4 As RadioButton
    Friend WithEvents rdoLM3 As RadioButton
    Friend WithEvents rdoLM2 As RadioButton
    Friend WithEvents rdoLM1 As RadioButton
    Friend WithEvents lblPixelsLeftOver As Label
    Friend WithEvents FLM As ColumnHeader
    Friend WithEvents rdoCBH As RadioButton
    'Friend WithEvents chrtCompFM As System.Windows.Forms.DataVisualization.Charting.Chart
    'Friend WithEvents chrtDist As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents CanFM As ColumnHeader
    Friend WithEvents FM40 As ColumnHeader
    Friend WithEvents CBD40 As ColumnHeader
    Friend WithEvents CBH40 As ColumnHeader
    Private WithEvents cmdCustomFM As Button
    Friend WithEvents cmsEditRule As ContextMenuStrip
    Friend WithEvents cmsLowHigh As ContextMenuStrip
    Friend WithEvents cmbSortRules As ComboBox
    Friend WithEvents grpEVTOrder As GroupBox
    Friend WithEvents rdoNumber As RadioButton
    Friend WithEvents rdoName As RadioButton
    Friend WithEvents grpSort As GroupBox
    Friend WithEvents grpCanopyLines As GroupBox
    Friend WithEvents rdoCBDDistGraph As RadioButton
    Friend WithEvents rdoCBHDistGraph As RadioButton
    Friend WithEvents rdoNoneDistGraph As RadioButton
    Friend WithEvents rdoCHDistGraph As RadioButton
    Friend WithEvents rdoCCDistGraph As RadioButton
    Friend WithEvents TTSession As ToolTip
    Private WithEvents lblDistCode As Label
    Friend WithEvents grpCustFM As GroupBox
    Private WithEvents cmdDelCstFM As Button
    Friend WithEvents cmdSaveCSTFM As Button
    Friend WithEvents chkFMType As CheckBox
    Private WithEvents lblDefaultFM As Label
    Private WithEvents lblFMType As Label
    Private WithEvents lblXtMoist As Label
    Private WithEvents lblDepth As Label
    Private WithEvents lblL_WSAV As Label
    Private WithEvents lblL_HSAV As Label
    Private WithEvents lbl1HSAV As Label
    Private WithEvents lblLiveW As Label
    Private WithEvents lblLiveH As Label
    Private WithEvents lbl100H As Label
    Private WithEvents lbl10H As Label
    Private WithEvents lbl1H As Label
    Friend WithEvents cmbDefaultFM As ComboBox
    Friend WithEvents TrkBar As TrackBar
    Friend WithEvents rdoXtMoist As RadioButton
    Friend WithEvents rdoLiveW As RadioButton
    Friend WithEvents rdo1HSAV As RadioButton
    Friend WithEvents rdoLiveHSAV As RadioButton
    Friend WithEvents rdoLiveWSAV As RadioButton
    Friend WithEvents rdoDepth As RadioButton
    Friend WithEvents rdoLiveH As RadioButton
    Friend WithEvents rdo100H As RadioButton
    Friend WithEvents rdo10H As RadioButton
    Friend WithEvents rdo1H As RadioButton
End Class
