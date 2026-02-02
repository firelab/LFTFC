<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAddEdit
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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(frmAddEdit))
        lblRule = New System.Windows.Forms.Label()
        lblOnOff = New System.Windows.Forms.Label()
        lblCover = New System.Windows.Forms.Label()
        lblHeight = New System.Windows.Forms.Label()
        cmbOnOff = New System.Windows.Forms.ComboBox()
        cmbCoverLow = New System.Windows.Forms.ComboBox()
        cmbCoverHigh = New System.Windows.Forms.ComboBox()
        cmbHeightLow = New System.Windows.Forms.ComboBox()
        cmbHeightHigh = New System.Windows.Forms.ComboBox()
        lblBPSRule = New System.Windows.Forms.Label()
        cmbBPSRule = New System.Windows.Forms.ComboBox()
        lblWildRule = New System.Windows.Forms.Label()
        cmbWildRule = New System.Windows.Forms.ComboBox()
        lblSurfaceFuel = New System.Windows.Forms.Label()
        lblFM13 = New System.Windows.Forms.Label()
        lblFM40 = New System.Windows.Forms.Label()
        cmbFBFM13 = New System.Windows.Forms.ComboBox()
        cmbFBFM40 = New System.Windows.Forms.ComboBox()
        lblFCCS = New System.Windows.Forms.Label()
        cmbFCCS = New System.Windows.Forms.ComboBox()
        lblFLM = New System.Windows.Forms.Label()
        cmbFLM = New System.Windows.Forms.ComboBox()
        lblCanopyFuel = New System.Windows.Forms.Label()
        lblCFGuide = New System.Windows.Forms.Label()
        cmbCanopy = New System.Windows.Forms.ComboBox()
        lblCC = New System.Windows.Forms.Label()
        cmbCC = New System.Windows.Forms.ComboBox()
        lblCH = New System.Windows.Forms.Label()
        cmbCH = New System.Windows.Forms.ComboBox()
        CBD13x100 = New System.Windows.Forms.Label()
        lblCBD40 = New System.Windows.Forms.Label()
        lblCBH13 = New System.Windows.Forms.Label()
        lblCBH40 = New System.Windows.Forms.Label()
        txtCBD13x100 = New System.Windows.Forms.TextBox()
        txtCBD40x100 = New System.Windows.Forms.TextBox()
        txtCBH13mx10 = New System.Windows.Forms.TextBox()
        txtCBH40mx10 = New System.Windows.Forms.TextBox()
        grpLimUnLim = New System.Windows.Forms.GroupBox()
        rdoUnLim = New System.Windows.Forms.RadioButton()
        rdoLim = New System.Windows.Forms.RadioButton()
        cmbCanFM = New System.Windows.Forms.ComboBox()
        lblCanFM = New System.Windows.Forms.Label()
        chkAllowFM = New System.Windows.Forms.CheckBox()
        cmdCancel = New System.Windows.Forms.Button()
        cmdDone = New System.Windows.Forms.Button()
        cmdAddSave = New System.Windows.Forms.Button()
        grpLimUnLim.SuspendLayout()
        SuspendLayout()
        ' 
        ' lblRule
        ' 
        lblRule.BackColor = Drawing.Color.WhiteSmoke
        lblRule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        lblRule.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblRule.Font = New Drawing.Font("Comic Sans MS", 11.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point)
        lblRule.Location = New Drawing.Point(0, 0)
        lblRule.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblRule.Name = "lblRule"
        lblRule.Size = New Drawing.Size(845, 30)
        lblRule.TabIndex = 10
        lblRule.Text = "Rule Descriptor"
        lblRule.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblOnOff
        ' 
        lblOnOff.BackColor = Drawing.Color.White
        lblOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblOnOff.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblOnOff.Location = New Drawing.Point(295, 30)
        lblOnOff.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblOnOff.Name = "lblOnOff"
        lblOnOff.Size = New Drawing.Size(68, 23)
        lblOnOff.TabIndex = 34
        lblOnOff.Text = "On/Off"
        lblOnOff.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblCover
        ' 
        lblCover.BackColor = Drawing.Color.White
        lblCover.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCover.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCover.Location = New Drawing.Point(374, 30)
        lblCover.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCover.Name = "lblCover"
        lblCover.Size = New Drawing.Size(222, 23)
        lblCover.TabIndex = 35
        lblCover.Text = "Range of Cover"
        lblCover.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblHeight
        ' 
        lblHeight.BackColor = Drawing.Color.White
        lblHeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblHeight.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblHeight.Location = New Drawing.Point(608, 30)
        lblHeight.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblHeight.Name = "lblHeight"
        lblHeight.Size = New Drawing.Size(222, 23)
        lblHeight.TabIndex = 36
        lblHeight.Text = "Range of Height"
        lblHeight.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbOnOff
        ' 
        cmbOnOff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbOnOff.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbOnOff.FormattingEnabled = True
        cmbOnOff.Location = New Drawing.Point(295, 57)
        cmbOnOff.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbOnOff.Name = "cmbOnOff"
        cmbOnOff.Size = New Drawing.Size(67, 26)
        cmbOnOff.TabIndex = 37
        ' 
        ' cmbCoverLow
        ' 
        cmbCoverLow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCoverLow.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbCoverLow.FormattingEnabled = True
        cmbCoverLow.Location = New Drawing.Point(374, 57)
        cmbCoverLow.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbCoverLow.Name = "cmbCoverLow"
        cmbCoverLow.Size = New Drawing.Size(87, 26)
        cmbCoverLow.TabIndex = 38
        ' 
        ' cmbCoverHigh
        ' 
        cmbCoverHigh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCoverHigh.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbCoverHigh.FormattingEnabled = True
        cmbCoverHigh.Location = New Drawing.Point(469, 57)
        cmbCoverHigh.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbCoverHigh.Name = "cmbCoverHigh"
        cmbCoverHigh.Size = New Drawing.Size(126, 26)
        cmbCoverHigh.TabIndex = 39
        ' 
        ' cmbHeightLow
        ' 
        cmbHeightLow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbHeightLow.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbHeightLow.FormattingEnabled = True
        cmbHeightLow.Location = New Drawing.Point(608, 57)
        cmbHeightLow.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbHeightLow.Name = "cmbHeightLow"
        cmbHeightLow.Size = New Drawing.Size(87, 26)
        cmbHeightLow.TabIndex = 40
        ' 
        ' cmbHeightHigh
        ' 
        cmbHeightHigh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbHeightHigh.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbHeightHigh.FormattingEnabled = True
        cmbHeightHigh.Location = New Drawing.Point(702, 57)
        cmbHeightHigh.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbHeightHigh.Name = "cmbHeightHigh"
        cmbHeightHigh.Size = New Drawing.Size(126, 26)
        cmbHeightHigh.TabIndex = 41
        ' 
        ' lblBPSRule
        ' 
        lblBPSRule.BackColor = Drawing.Color.White
        lblBPSRule.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblBPSRule.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblBPSRule.Location = New Drawing.Point(16, 90)
        lblBPSRule.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblBPSRule.Name = "lblBPSRule"
        lblBPSRule.Size = New Drawing.Size(813, 23)
        lblBPSRule.TabIndex = 42
        lblBPSRule.Text = "Biophysical Setting Name and Model"
        lblBPSRule.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbBPSRule
        ' 
        cmbBPSRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbBPSRule.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbBPSRule.FormattingEnabled = True
        cmbBPSRule.Location = New Drawing.Point(16, 117)
        cmbBPSRule.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbBPSRule.Name = "cmbBPSRule"
        cmbBPSRule.Size = New Drawing.Size(812, 26)
        cmbBPSRule.TabIndex = 43
        ' 
        ' lblWildRule
        ' 
        lblWildRule.BackColor = Drawing.Color.White
        lblWildRule.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblWildRule.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblWildRule.Location = New Drawing.Point(16, 150)
        lblWildRule.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblWildRule.Name = "lblWildRule"
        lblWildRule.Size = New Drawing.Size(813, 23)
        lblWildRule.TabIndex = 44
        lblWildRule.Text = "Wildcard"
        lblWildRule.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbWildRule
        ' 
        cmbWildRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbWildRule.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbWildRule.FormattingEnabled = True
        cmbWildRule.Location = New Drawing.Point(16, 177)
        cmbWildRule.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbWildRule.Name = "cmbWildRule"
        cmbWildRule.Size = New Drawing.Size(812, 26)
        cmbWildRule.TabIndex = 45
        ' 
        ' lblSurfaceFuel
        ' 
        lblSurfaceFuel.BackColor = Drawing.Color.WhiteSmoke
        lblSurfaceFuel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        lblSurfaceFuel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblSurfaceFuel.Font = New Drawing.Font("Comic Sans MS", 11.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point)
        lblSurfaceFuel.Location = New Drawing.Point(-1, 211)
        lblSurfaceFuel.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblSurfaceFuel.Name = "lblSurfaceFuel"
        lblSurfaceFuel.Size = New Drawing.Size(845, 30)
        lblSurfaceFuel.TabIndex = 46
        lblSurfaceFuel.Text = "                                       Surface Fuel Associated With Above Rule Descriptor"
        lblSurfaceFuel.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblFM13
        ' 
        lblFM13.BackColor = Drawing.Color.White
        lblFM13.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblFM13.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblFM13.Location = New Drawing.Point(15, 241)
        lblFM13.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblFM13.Name = "lblFM13"
        lblFM13.Size = New Drawing.Size(813, 23)
        lblFM13.TabIndex = 47
        lblFM13.Text = "Fire Behavior Fuel Models, Anderson 13 (FBFM13)          "
        lblFM13.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblFM40
        ' 
        lblFM40.BackColor = Drawing.Color.White
        lblFM40.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblFM40.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblFM40.Location = New Drawing.Point(15, 301)
        lblFM40.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblFM40.Name = "lblFM40"
        lblFM40.Size = New Drawing.Size(813, 23)
        lblFM40.TabIndex = 48
        lblFM40.Text = "Fire Behavior Fuel Models, Scott and Burgan 40 (FBFM40)"
        lblFM40.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbFBFM13
        ' 
        cmbFBFM13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbFBFM13.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbFBFM13.FormattingEnabled = True
        cmbFBFM13.Location = New Drawing.Point(15, 268)
        cmbFBFM13.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbFBFM13.Name = "cmbFBFM13"
        cmbFBFM13.Size = New Drawing.Size(812, 26)
        cmbFBFM13.TabIndex = 49
        ' 
        ' cmbFBFM40
        ' 
        cmbFBFM40.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbFBFM40.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbFBFM40.FormattingEnabled = True
        cmbFBFM40.Location = New Drawing.Point(15, 328)
        cmbFBFM40.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbFBFM40.Name = "cmbFBFM40"
        cmbFBFM40.Size = New Drawing.Size(812, 26)
        cmbFBFM40.TabIndex = 50
        ' 
        ' lblFCCS
        ' 
        lblFCCS.BackColor = Drawing.Color.White
        lblFCCS.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblFCCS.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblFCCS.Location = New Drawing.Point(15, 421)
        lblFCCS.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblFCCS.Name = "lblFCCS"
        lblFCCS.Size = New Drawing.Size(813, 23)
        lblFCCS.TabIndex = 51
        lblFCCS.Text = "Fuel Characteristic Classification System, Ottmar (FCCS)"
        lblFCCS.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbFCCS
        ' 
        cmbFCCS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbFCCS.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbFCCS.FormattingEnabled = True
        cmbFCCS.Location = New Drawing.Point(15, 448)
        cmbFCCS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbFCCS.Name = "cmbFCCS"
        cmbFCCS.Size = New Drawing.Size(812, 26)
        cmbFCCS.TabIndex = 52
        ' 
        ' lblFLM
        ' 
        lblFLM.BackColor = Drawing.Color.White
        lblFLM.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblFLM.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblFLM.Location = New Drawing.Point(15, 481)
        lblFLM.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblFLM.Name = "lblFLM"
        lblFLM.Size = New Drawing.Size(813, 23)
        lblFLM.TabIndex = 53
        lblFLM.Text = "Fuel Loading Models, Lutes (FLM)"
        lblFLM.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbFLM
        ' 
        cmbFLM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbFLM.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbFLM.FormattingEnabled = True
        cmbFLM.Location = New Drawing.Point(15, 508)
        cmbFLM.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbFLM.Name = "cmbFLM"
        cmbFLM.Size = New Drawing.Size(812, 26)
        cmbFLM.TabIndex = 54
        ' 
        ' lblCanopyFuel
        ' 
        lblCanopyFuel.BackColor = Drawing.Color.WhiteSmoke
        lblCanopyFuel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        lblCanopyFuel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCanopyFuel.Font = New Drawing.Font("Comic Sans MS", 11.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point)
        lblCanopyFuel.Location = New Drawing.Point(-1, 541)
        lblCanopyFuel.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCanopyFuel.Name = "lblCanopyFuel"
        lblCanopyFuel.Size = New Drawing.Size(845, 30)
        lblCanopyFuel.TabIndex = 55
        lblCanopyFuel.Text = "Canopy Fuel Associated With Above Rule Descriptor"
        lblCanopyFuel.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblCFGuide
        ' 
        lblCFGuide.BackColor = Drawing.Color.White
        lblCFGuide.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCFGuide.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCFGuide.Location = New Drawing.Point(15, 571)
        lblCFGuide.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCFGuide.Name = "lblCFGuide"
        lblCFGuide.Size = New Drawing.Size(813, 23)
        lblCFGuide.TabIndex = 56
        lblCFGuide.Text = "Canopy Fuel Guide (CG)"
        lblCFGuide.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbCanopy
        ' 
        cmbCanopy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCanopy.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbCanopy.FormattingEnabled = True
        cmbCanopy.Location = New Drawing.Point(15, 598)
        cmbCanopy.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbCanopy.Name = "cmbCanopy"
        cmbCanopy.Size = New Drawing.Size(812, 26)
        cmbCanopy.TabIndex = 57
        ' 
        ' lblCC
        ' 
        lblCC.BackColor = Drawing.Color.White
        lblCC.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCC.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCC.Location = New Drawing.Point(55, 631)
        lblCC.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCC.Name = "lblCC"
        lblCC.Size = New Drawing.Size(68, 23)
        lblCC.TabIndex = 58
        lblCC.Text = "Cover"
        lblCC.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbCC
        ' 
        cmbCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCC.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbCC.FormattingEnabled = True
        cmbCC.Location = New Drawing.Point(55, 658)
        cmbCC.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbCC.Name = "cmbCC"
        cmbCC.Size = New Drawing.Size(67, 26)
        cmbCC.TabIndex = 59
        ' 
        ' lblCH
        ' 
        lblCH.BackColor = Drawing.Color.White
        lblCH.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCH.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCH.Location = New Drawing.Point(134, 631)
        lblCH.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCH.Name = "lblCH"
        lblCH.Size = New Drawing.Size(222, 23)
        lblCH.TabIndex = 60
        lblCH.Text = "Height Midpoint Mx10"
        lblCH.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' cmbCH
        ' 
        cmbCH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCH.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbCH.FormattingEnabled = True
        cmbCH.Location = New Drawing.Point(134, 658)
        cmbCH.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbCH.Name = "cmbCH"
        cmbCH.Size = New Drawing.Size(221, 26)
        cmbCH.TabIndex = 61
        ' 
        ' CBD13x100
        ' 
        CBD13x100.BackColor = Drawing.Color.White
        CBD13x100.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        CBD13x100.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        CBD13x100.Location = New Drawing.Point(368, 631)
        CBD13x100.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        CBD13x100.Name = "CBD13x100"
        CBD13x100.Size = New Drawing.Size(94, 23)
        CBD13x100.TabIndex = 62
        CBD13x100.Text = "CBD13x100"
        CBD13x100.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblCBD40
        ' 
        lblCBD40.BackColor = Drawing.Color.White
        lblCBD40.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCBD40.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCBD40.Location = New Drawing.Point(474, 631)
        lblCBD40.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCBD40.Name = "lblCBD40"
        lblCBD40.Size = New Drawing.Size(94, 23)
        lblCBD40.TabIndex = 63
        lblCBD40.Text = "CBD40x100"
        lblCBD40.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblCBH13
        ' 
        lblCBH13.BackColor = Drawing.Color.White
        lblCBH13.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCBH13.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCBH13.Location = New Drawing.Point(580, 631)
        lblCBH13.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCBH13.Name = "lblCBH13"
        lblCBH13.Size = New Drawing.Size(94, 23)
        lblCBH13.TabIndex = 64
        lblCBH13.Text = "CBH13x10"
        lblCBH13.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' lblCBH40
        ' 
        lblCBH40.BackColor = Drawing.Color.White
        lblCBH40.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCBH40.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCBH40.Location = New Drawing.Point(686, 631)
        lblCBH40.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCBH40.Name = "lblCBH40"
        lblCBH40.Size = New Drawing.Size(94, 23)
        lblCBH40.TabIndex = 65
        lblCBH40.Text = "CBH40x10"
        lblCBH40.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' txtCBD13x100
        ' 
        txtCBD13x100.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtCBD13x100.Location = New Drawing.Point(368, 658)
        txtCBD13x100.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        txtCBD13x100.Name = "txtCBD13x100"
        txtCBD13x100.Size = New Drawing.Size(94, 26)
        txtCBD13x100.TabIndex = 74
        ' 
        ' txtCBD40x100
        ' 
        txtCBD40x100.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtCBD40x100.Location = New Drawing.Point(474, 658)
        txtCBD40x100.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        txtCBD40x100.Name = "txtCBD40x100"
        txtCBD40x100.Size = New Drawing.Size(94, 26)
        txtCBD40x100.TabIndex = 75
        ' 
        ' txtCBH13mx10
        ' 
        txtCBH13mx10.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtCBH13mx10.Location = New Drawing.Point(580, 658)
        txtCBH13mx10.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        txtCBH13mx10.Name = "txtCBH13mx10"
        txtCBH13mx10.Size = New Drawing.Size(94, 26)
        txtCBH13mx10.TabIndex = 76
        ' 
        ' txtCBH40mx10
        ' 
        txtCBH40mx10.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        txtCBH40mx10.Location = New Drawing.Point(686, 658)
        txtCBH40mx10.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        txtCBH40mx10.Name = "txtCBH40mx10"
        txtCBH40mx10.Size = New Drawing.Size(94, 26)
        txtCBH40mx10.TabIndex = 77
        ' 
        ' grpLimUnLim
        ' 
        grpLimUnLim.BackColor = Drawing.Color.LightGray
        grpLimUnLim.Controls.Add(rdoUnLim)
        grpLimUnLim.Controls.Add(rdoLim)
        grpLimUnLim.Font = New Drawing.Font("Comic Sans MS", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        grpLimUnLim.Location = New Drawing.Point(16, 30)
        grpLimUnLim.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpLimUnLim.Name = "grpLimUnLim"
        grpLimUnLim.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        grpLimUnLim.Size = New Drawing.Size(270, 57)
        grpLimUnLim.TabIndex = 116
        grpLimUnLim.TabStop = False
        grpLimUnLim.Text = "Select Descriptor Type"
        ' 
        ' rdoUnLim
        ' 
        rdoUnLim.AutoSize = True
        rdoUnLim.Font = New Drawing.Font("Comic Sans MS", 9F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        rdoUnLim.Location = New Drawing.Point(145, 21)
        rdoUnLim.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoUnLim.Name = "rdoUnLim"
        rdoUnLim.Size = New Drawing.Size(79, 21)
        rdoUnLim.TabIndex = 1
        rdoUnLim.Text = "Unlimited"
        rdoUnLim.UseVisualStyleBackColor = True
        ' 
        ' rdoLim
        ' 
        rdoLim.AutoSize = True
        rdoLim.Checked = True
        rdoLim.Font = New Drawing.Font("Comic Sans MS", 9F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        rdoLim.Location = New Drawing.Point(8, 20)
        rdoLim.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        rdoLim.Name = "rdoLim"
        rdoLim.Size = New Drawing.Size(67, 21)
        rdoLim.TabIndex = 0
        rdoLim.TabStop = True
        rdoLim.Text = "Limited"
        rdoLim.UseVisualStyleBackColor = True
        ' 
        ' cmbCanFM
        ' 
        cmbCanFM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cmbCanFM.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        cmbCanFM.FormattingEnabled = True
        cmbCanFM.Location = New Drawing.Point(15, 388)
        cmbCanFM.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        cmbCanFM.Name = "cmbCanFM"
        cmbCanFM.Size = New Drawing.Size(812, 26)
        cmbCanFM.TabIndex = 118
        ' 
        ' lblCanFM
        ' 
        lblCanFM.BackColor = Drawing.Color.White
        lblCanFM.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        lblCanFM.Font = New Drawing.Font("Comic Sans MS", 9.75F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        lblCanFM.Location = New Drawing.Point(15, 361)
        lblCanFM.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        lblCanFM.Name = "lblCanFM"
        lblCanFM.Size = New Drawing.Size(813, 23)
        lblCanFM.TabIndex = 117
        lblCanFM.Text = "Canadian Fire Behavior Prediction System Fuel Types (FBPS FT)"
        lblCanFM.TextAlign = Drawing.ContentAlignment.MiddleCenter
        ' 
        ' chkAllowFM
        ' 
        chkAllowFM.AutoSize = True
        chkAllowFM.BackColor = Drawing.Color.FromArgb(CByte(224), CByte(224), CByte(224))
        chkAllowFM.Font = New Drawing.Font("Comic Sans MS", 8.25F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point)
        chkAllowFM.Location = New Drawing.Point(16, 216)
        chkAllowFM.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        chkAllowFM.Name = "chkAllowFM"
        chkAllowFM.Size = New Drawing.Size(264, 19)
        chkAllowFM.TabIndex = 119
        chkAllowFM.Text = "Allow CUSTOM FM and MIXING of 13 and 40"
        chkAllowFM.TextAlign = Drawing.ContentAlignment.MiddleRight
        chkAllowFM.UseVisualStyleBackColor = False
        ' 
        ' cmdCancel
        ' 
        cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), Drawing.Image)
        cmdCancel.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdCancel.Location = New Drawing.Point(400, 697)
        cmdCancel.Margin = New System.Windows.Forms.Padding(6)
        cmdCancel.Name = "cmdCancel"
        cmdCancel.Size = New Drawing.Size(93, 43)
        cmdCancel.TabIndex = 72
        cmdCancel.Text = "Cancel"
        cmdCancel.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdCancel.UseVisualStyleBackColor = True
        ' 
        ' cmdDone
        ' 
        cmdDone.Image = CType(resources.GetObject("cmdDone.Image"), Drawing.Image)
        cmdDone.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdDone.Location = New Drawing.Point(307, 697)
        cmdDone.Margin = New System.Windows.Forms.Padding(6)
        cmdDone.Name = "cmdDone"
        cmdDone.Size = New Drawing.Size(82, 43)
        cmdDone.TabIndex = 71
        cmdDone.Text = "Save" & vbCrLf & "Edits" & vbCrLf
        cmdDone.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdDone.UseVisualStyleBackColor = True
        ' 
        ' cmdAddSave
        ' 
        cmdAddSave.Image = CType(resources.GetObject("cmdAddSave.Image"), Drawing.Image)
        cmdAddSave.ImageAlign = Drawing.ContentAlignment.MiddleLeft
        cmdAddSave.Location = New Drawing.Point(214, 697)
        cmdAddSave.Margin = New System.Windows.Forms.Padding(6)
        cmdAddSave.Name = "cmdAddSave"
        cmdAddSave.Size = New Drawing.Size(82, 43)
        cmdAddSave.TabIndex = 70
        cmdAddSave.Text = "Save" & vbCrLf & "Rule"
        cmdAddSave.TextAlign = Drawing.ContentAlignment.MiddleRight
        cmdAddSave.UseVisualStyleBackColor = True
        ' 
        ' frmAddEdit
        ' 
        AutoScaleDimensions = New Drawing.SizeF(7F, 15F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        ClientSize = New Drawing.Size(845, 744)
        Controls.Add(chkAllowFM)
        Controls.Add(cmbCanFM)
        Controls.Add(lblCanFM)
        Controls.Add(grpLimUnLim)
        Controls.Add(txtCBH40mx10)
        Controls.Add(txtCBH13mx10)
        Controls.Add(txtCBD40x100)
        Controls.Add(txtCBD13x100)
        Controls.Add(cmdCancel)
        Controls.Add(cmdDone)
        Controls.Add(cmdAddSave)
        Controls.Add(lblCBH40)
        Controls.Add(lblCBH13)
        Controls.Add(lblCBD40)
        Controls.Add(CBD13x100)
        Controls.Add(cmbCH)
        Controls.Add(lblCH)
        Controls.Add(cmbCC)
        Controls.Add(lblCC)
        Controls.Add(cmbCanopy)
        Controls.Add(lblCFGuide)
        Controls.Add(lblCanopyFuel)
        Controls.Add(cmbFLM)
        Controls.Add(lblFLM)
        Controls.Add(cmbFCCS)
        Controls.Add(lblFCCS)
        Controls.Add(cmbFBFM40)
        Controls.Add(cmbFBFM13)
        Controls.Add(lblFM40)
        Controls.Add(lblFM13)
        Controls.Add(lblSurfaceFuel)
        Controls.Add(cmbWildRule)
        Controls.Add(lblWildRule)
        Controls.Add(cmbBPSRule)
        Controls.Add(lblBPSRule)
        Controls.Add(cmbHeightHigh)
        Controls.Add(cmbHeightLow)
        Controls.Add(cmbCoverHigh)
        Controls.Add(cmbCoverLow)
        Controls.Add(cmbOnOff)
        Controls.Add(lblHeight)
        Controls.Add(lblCover)
        Controls.Add(lblOnOff)
        Controls.Add(lblRule)
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
        Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Name = "frmAddEdit"
        Text = "Add and Edit Rules"
        grpLimUnLim.ResumeLayout(False)
        grpLimUnLim.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents cmbOnOff As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCoverLow As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCoverHigh As System.Windows.Forms.ComboBox
    Friend WithEvents cmbHeightLow As System.Windows.Forms.ComboBox
    Friend WithEvents cmbHeightHigh As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBPSRule As System.Windows.Forms.ComboBox
    Friend WithEvents cmbWildRule As System.Windows.Forms.ComboBox
    Friend WithEvents cmbFBFM13 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbFBFM40 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbFCCS As System.Windows.Forms.ComboBox
    Friend WithEvents cmbFLM As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCanopy As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCC As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCH As System.Windows.Forms.ComboBox
    Friend WithEvents lblRule As System.Windows.Forms.Label
    Friend WithEvents lblOnOff As System.Windows.Forms.Label
    Friend WithEvents lblCover As System.Windows.Forms.Label
    Friend WithEvents lblHeight As System.Windows.Forms.Label
    Friend WithEvents lblBPSRule As System.Windows.Forms.Label
    Friend WithEvents lblWildRule As System.Windows.Forms.Label
    Friend WithEvents lblSurfaceFuel As System.Windows.Forms.Label
    Friend WithEvents lblFM13 As System.Windows.Forms.Label
    Friend WithEvents lblFM40 As System.Windows.Forms.Label
    Friend WithEvents lblFCCS As System.Windows.Forms.Label
    Friend WithEvents lblFLM As System.Windows.Forms.Label
    Friend WithEvents lblCanopyFuel As System.Windows.Forms.Label
    Friend WithEvents lblCFGuide As System.Windows.Forms.Label
    Friend WithEvents lblCC As System.Windows.Forms.Label
    Friend WithEvents lblCH As System.Windows.Forms.Label
    Friend WithEvents CBD13x100 As System.Windows.Forms.Label
    Friend WithEvents lblCBD40 As System.Windows.Forms.Label
    Friend WithEvents lblCBH13 As System.Windows.Forms.Label
    Friend WithEvents lblCBH40 As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdDone As System.Windows.Forms.Button
    Friend WithEvents cmdAddSave As System.Windows.Forms.Button
    Friend WithEvents txtCBD13x100 As System.Windows.Forms.TextBox
    Friend WithEvents txtCBD40x100 As System.Windows.Forms.TextBox
    Friend WithEvents txtCBH13mx10 As System.Windows.Forms.TextBox
    Friend WithEvents txtCBH40mx10 As System.Windows.Forms.TextBox
    Friend WithEvents grpLimUnLim As System.Windows.Forms.GroupBox
    Friend WithEvents rdoUnLim As System.Windows.Forms.RadioButton
    Friend WithEvents rdoLim As System.Windows.Forms.RadioButton
    Friend WithEvents cmbCanFM As System.Windows.Forms.ComboBox
    Friend WithEvents lblCanFM As System.Windows.Forms.Label
    Friend WithEvents chkAllowFM As System.Windows.Forms.CheckBox
End Class
