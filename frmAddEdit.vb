Imports System.Data
Imports System.Windows.Forms

Public Class frmAddEdit
    Private strSQL As String                'SQL variable for this module
    Private ruleAOE As clsRule              'The selected rule to Add or Edit
    Private AOE As String                   'Add or Edit a rule ("Add" or "Edit")
    Private EVT As Integer                  'stores the 4 digit EVT value
    Private DIST As Integer                 'stores the disturbance code
    Private SN As String                    'Stores the session name
    Private comboR As String                'Stores the combo table name for rule making
    Private rulesR As String                'Stores the rules table name for rule making
    Private RulesetCollection As Collection 'Stores and changes the ruleset collection
    Private rdoFuel As Boolean              'Stores the checked state passed in from frmRule rdoFuel 
    Private strProjectPath As String        'Stores the path of the project

    Public Sub New(ByVal AddOrEdit As String, ByVal EVTNum As Integer, ByVal DISTNum As Integer, ByVal Index As Integer,
                   ByVal SessionName As String, ByVal RulesTable As String, ByVal ComboTable As String,
                   ByRef RlesetCol As Collection, ByVal ProjPath As String)

        InitializeComponent()               ' This call is required by the Windows Form Designer.

        ' Add any initialization after the InitializeComponent() call.

        'Set variables
        strProjectPath = ProjPath
        AOE = AddOrEdit
        EVT = EVTNum
        DIST = DISTNum
        SN = SessionName
        comboR = ComboTable
        rulesR = RulesTable
        RulesetCollection = RlesetCol

        Dim strNewRuleNote As String

        'Set starting values
        If AOE = "Add" Then
            InitAllCMB()
            With Me
                .cmdAddSave.Visible = True
                .cmdCancel.Visible = True
                .cmdDone.Visible = False
                .cmbCoverLow.SelectedIndex = 0
                .cmbCoverHigh.SelectedIndex = .cmbCoverHigh.Items.Count - 1
                .cmbHeightLow.SelectedIndex = 0
                .cmbHeightHigh.SelectedIndex = .cmbHeightHigh.Items.Count - 1
                .cmbBPSRule.SelectedIndex = 0
                .cmbWildRule.SelectedIndex = 0
                .cmbFBFM13.SelectedIndex = 0
                .cmbFBFM40.SelectedIndex = 0
                .cmbCanFM.SelectedIndex = 0
                .cmbFCCS.SelectedIndex = 0
                .cmbFLM.SelectedIndex = 0
                .cmbCanopy.SelectedIndex = 0
                .cmbCC.SelectedIndex = 0
                .cmbCH.SelectedIndex = 0
                .txtCBD13x100.Text = 9999
                .txtCBD40x100.Text = 9999
                .txtCBH13mx10.Text = 9999
                .txtCBH40mx10.Text = 9999
                .cmbOnOff.Text = "On"
            End With
        ElseIf AOE = "Edit" Then
            'Set the Rule
            ruleAOE = RulesetCollection.Item(Index)

            'Gets the start time and date of edit
            strNewRuleNote = Now.ToShortTimeString & " " & Now.ToShortDateString & "  " & SN & ": Changed "
            'Add values of the rule to the individual controls on the edit form
            With Me
                .cmbCoverLow.Items.Add(RulesetCollection.Item(Index).StrCovLow)
                .cmbCoverLow.Text = RulesetCollection.Item(Index).StrCovLow
                .cmbCoverHigh.Items.Add(RulesetCollection.Item(Index).StrCovHigh)
                .cmbCoverHigh.Text = RulesetCollection.Item(Index).StrCovHigh
                .cmbHeightLow.Items.Add(RulesetCollection.Item(Index).StrHgtLow)
                .cmbHeightLow.Text = RulesetCollection.Item(Index).StrHgtLow
                .cmbHeightHigh.Items.Add(RulesetCollection.Item(Index).StrHgtHigh)
                .cmbHeightHigh.Text = RulesetCollection.Item(Index).StrHgtHigh
                .cmbBPSRule.Items.Add(RulesetCollection.Item(Index).BPS)
                .cmbBPSRule.Text = RulesetCollection.Item(Index).BPS
                .cmbWildRule.Items.Add(RulesetCollection.Item(Index).Wildcard)
                .cmbWildRule.Text = RulesetCollection.Item(Index).Wildcard
                .cmbFBFM13.Items.Add(RulesetCollection.Item(Index).FBFM13)
                .cmbFBFM13.Text = RulesetCollection.Item(Index).FBFM13
                .cmbFBFM40.Items.Add(RulesetCollection.Item(Index).FBFM40)
                .cmbFBFM40.Text = RulesetCollection.Item(Index).FBFM40
                .cmbCanFM.Items.Add(RulesetCollection.Item(Index).CanFM)
                .cmbCanFM.Text = RulesetCollection.Item(Index).CanFM
                .cmbFCCS.Items.Add(RulesetCollection.Item(Index).FCCS)
                .cmbFCCS.Text = RulesetCollection.Item(Index).FCCS
                .cmbFLM.Items.Add(RulesetCollection.Item(Index).FLM)
                .cmbFLM.Text = RulesetCollection.Item(Index).FLM
                .cmbCanopy.Items.Add(RulesetCollection.Item(Index).Canopy)
                .cmbCanopy.Text = RulesetCollection.Item(Index).Canopy
                .cmbCC.Items.Add(RulesetCollection.Item(Index).CCover)
                .cmbCC.Text = RulesetCollection.Item(Index).CCover
                .cmbCH.Items.Add(RulesetCollection.Item(Index).CHeight)
                .cmbCH.Text = RulesetCollection.Item(Index).CHeight
                .txtCBD13x100.Text = RulesetCollection.Item(Index).CBD13
                .txtCBD40x100.Text = RulesetCollection.Item(Index).CBD40
                .txtCBH13mx10.Text = RulesetCollection.Item(Index).CBH13
                .txtCBH40mx10.Text = RulesetCollection.Item(Index).CBH40
                .cmbOnOff.Items.Add(RulesetCollection.Item(Index).OnOff)
                .cmbOnOff.Text = RulesetCollection.Item(Index).OnOff
                .cmdDone.Visible = True
                cmdAddSave.Visible = False
                cmdCancel.Visible = True
            End With
            InitAllCMB()
        End If
    End Sub

    Private Sub cmbCoverLow_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCoverLow.SelectionChangeCommitted
        Dim TempCH As String = cmbCoverHigh.Text
        Dim TempCH_Code As Integer = gf_ConvertBack(TempCH, strProjectPath)
        Dim TempCL_Code As Integer = gf_ConvertBack(cmbCoverLow.Text, strProjectPath)
        Dim TempHL As String = cmbHeightLow.Text
        Dim TempHL_Code As Integer = gf_ConvertBack(cmbHeightLow.Text, strProjectPath)
        Dim TempHH As String = cmbHeightHigh.Text
        Dim TempHH_Code As Integer = gf_ConvertBack(cmbHeightHigh.Text, strProjectPath)
        Dim bln_SameLifeform As Boolean = True
        Dim strLifeformCL As String
        Dim strLifeformTest As String
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data
        Dim rs3 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            'Clear the contents of the comboboxes
            cmbCoverHigh.Items.Clear()
            cmbHeightLow.Items.Clear()
            cmbHeightHigh.Items.Clear()

            'Repopulate the comboboxes
            PopCovHgt(cmbCoverHigh)
            PopCovHgt(cmbHeightLow)
            PopCovHgt(cmbHeightHigh)

            'Convert code to text cover and height
            ConvertCodecmbCovHgt()

            'Check to see if high cover selection is still valid
            If TempCH_Code >= TempCL_Code And (TempCH_Code < (Math.Ceiling(TempCL_Code / 10) * 10)) Then 'It still is a valid value
                cmbCoverHigh.Items.Add(TempCH)
                cmbCoverHigh.Text = TempCH
            Else
                cmbCoverHigh.SelectedIndex = cmbCoverHigh.Items.Count - 1
            End If

            'Check to see if height low selection is still valid
            'Check to make sure the lifeform is the same

            'Find lifeform for tempCL_code
            strSQL = "SELECT LUT_Cover.EVC, LUT_Cover.Lifeform " &
                     "FROM LUT_Cover WHERE (((LUT_Cover.EVC)=" & TempCL_Code & "))"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            strLifeformCL = rs1.Fields!Lifeform.Value

            'Find lifeform for tempHL_code
            strSQL = "SELECT LUT_Height.EVH, LUT_Height.Lifeform " &
                     "FROM LUT_Height WHERE (((LUT_Height.EVH)=" & TempHL_Code & "))"
            rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            strLifeformTest = rs2.Fields!Lifeform.Value

            If strLifeformCL <> strLifeformTest Then bln_SameLifeform = False

            If bln_SameLifeform = True Then
                cmbHeightLow.Items.Add(TempHL)
                cmbHeightLow.Text = TempHL
            Else
                cmbHeightLow.SelectedIndex = 0
            End If

            bln_SameLifeform = True                     'Reset to true

            'Check to see if height high selection is still valid
            'Check to make sure the lifeform is the same

            'Find lifeform for tempHH_code
            strSQL = "SELECT LUT_Height.EVH, LUT_Height.Lifeform " &
                     "FROM LUT_Height WHERE (((LUT_Height.EVH)=" & TempHH_Code & "))"
            rs3.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            strLifeformTest = rs3.Fields!Lifeform.Value

            If strLifeformCL <> strLifeformTest Then bln_SameLifeform = False

            If TempHH_Code >= TempHL_Code And bln_SameLifeform = True Then 'It still is a valid value
                cmbHeightHigh.Items.Add(TempHH)
                cmbHeightHigh.Text = TempHH
            Else 'It is not in the data check to see if it is higher than height low and adjust if needed
                cmbHeightHigh.SelectedIndex = cmbHeightHigh.Items.Count - 1
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
            MsgBox("Error in cmbCoverLow_SelectionChangeCommitted - " & ex.Message)
        End Try
    End Sub

    Private Sub cmbHeightLow_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbHeightLow.SelectionChangeCommitted
        Dim TempHH_Code As Integer = gf_ConvertBack(cmbHeightHigh.Text, strProjectPath)
        Dim TempHH As String = cmbHeightHigh.Text
        Dim TempHL_Code As Integer = gf_ConvertBack(cmbHeightLow.Text, strProjectPath)
        Dim bln_SameLifeform As Boolean = True 'Temp values are in the same lifeform or not
        Dim strLifeformHL As String
        Dim strLifeformTest As String
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try

            'Clear values
            cmbHeightHigh.Items.Clear()

            'Repopulate the comboboxe
            PopCovHgt(cmbHeightHigh)

            'Convert code to text cover and height
            ConvertCodecmbCovHgt()

            'Check to see if height high selection is still valid
            'Check to make sure the lifeform is the same

            'Find lifeform for tempHL_code
            strSQL = "SELECT LUT_Height.EVH, LUT_Height.Lifeform " &
                     "FROM LUT_Height WHERE (((LUT_Height.EVH)=" & TempHL_Code & "))"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            strLifeformHL = rs1.Fields!Lifeform.Value

            'Find lifeform for tempHH_code
            strSQL = "SELECT LUT_Height.EVH, LUT_Height.Lifeform " &
                     "FROM LUT_Height WHERE (((LUT_Height.EVH)=" & TempHH_Code & "))"
            rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            strLifeformTest = rs2.Fields!Lifeform.Value

            If strLifeformHL <> strLifeformTest Then bln_SameLifeform = False

            If TempHH_Code >= TempHL_Code And bln_SameLifeform = True Then 'It still is a valid value
                cmbHeightHigh.Items.Add(TempHH)
                cmbHeightHigh.Text = TempHH
            Else 'It is not in the data check to see if it is higher than height low and adjust if needed
                cmbHeightHigh.SelectedIndex = cmbHeightHigh.Items.Count - 1
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing


            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
            MsgBox("Error in cmbHeightLow_SelectionChangeCommitted - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdAddSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddSave.Click
        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim strNewRuleNote As String

            'Convert Canopy Height midpoint to x10
            If CDbl(cmbCH.Text) > 0 And CDbl(cmbCH.Text) < 100 Then
                cmbCH.Items.Add(CDbl(cmbCH.Text) * 10)
                cmbCH.Text = CDbl(cmbCH.Text) * 10
            Else
                'Do nothing
            End If

            'Check to make sure all the blanks a filled with values
            If cmbCoverLow.Text <> "" And cmbCoverHigh.Text <> "" And cmbHeightLow.Text <> "" And
                cmbHeightHigh.Text <> "" And IsNumeric(txtCBD13x100.Text) And IsNumeric(txtCBD40x100.Text) And
                IsNumeric(txtCBH13mx10.Text) And IsNumeric(txtCBH40mx10.Text) Then

                'Get Time,date,calibration name and rule that was added to put into the notes for the new rule
                strNewRuleNote = Now.ToShortTimeString & " " & Now.ToShortDateString & " " & SN & ": NEW RULE  " &
                EVT & "[" & DIST & "]." &
                Trim(Strings.Right(cmbCoverLow.Text, 3)) & cmbCoverHigh.Text & "." &
                Strings.Right(cmbHeightLow.Text, Len(cmbHeightLow.Text) - 2) & cmbHeightHigh.Text & "." &
                gf_GetNum(cmbBPSRule.Text, "General") & "." &
                Trim(Strings.Left(cmbWildRule.Text, 13)) & "." &
                gf_GetNum(cmbFBFM13.Text, "General") & "/" &
                Trim(Strings.Left(cmbFBFM40.Text, 9)) & "." &
                Trim(Strings.Left(cmbCanFM.Text, 9)) & "." &
                gf_GetNum(cmbFCCS.Text, "General") & "/" &
                gf_GetNum(cmbFLM.Text, "General") & "." &
                gf_GetNum(cmbCanopy.Text, "General") & "." &
                cmbCC.Text & "." &
                cmbCH.Text & "." &
                txtCBD13x100.Text & "/" &
                txtCBD40x100.Text & "." &
                txtCBH13mx10.Text & "/" &
                txtCBH40mx10.Text & "." &
                cmbOnOff.Text

                'This SQL inserts the new rule into the Ruleset table
                If EVT > 99 Or IsNumeric(cmbCoverLow.Text) = False Then
                    strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                        "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                        "CBH13mx10, CBH40mx10, OnOff, Notes) " &
                        "VALUES (" & EVT & ", " & DIST & ", " &
                        gf_ConvertBack(cmbCoverLow.Text, strProjectPath) & ", " &
                        gf_ConvertBack(cmbCoverHigh.Text, strProjectPath) & ", " &
                        gf_ConvertBack(cmbHeightLow.Text, strProjectPath) & ", " &
                        gf_ConvertBack(cmbHeightHigh.Text, strProjectPath) & ", '" &
                        gf_GetNum(cmbBPSRule.Text, "General") & "', '" &
                        cmbWildRule.Text & "', " &
                        gf_GetNum(cmbFBFM13.Text, "General") & ", '" &
                        Trim(Strings.Left(cmbFBFM40.Text, 9)) & "', '" &
                        Trim(Strings.Left(cmbCanFM.Text, 9)) & "', " &
                        gf_GetNum(cmbFCCS.Text, "General") & ", " &
                        gf_GetNum(cmbFLM.Text, "General") & ", " &
                        gf_GetNum(cmbCanopy.Text, "General") & ", " &
                        cmbCC.Text & ", " &
                        cmbCH.Text & ", " &
                        txtCBD13x100.Text & ", " &
                        txtCBD40x100.Text & ", " &
                        txtCBH13mx10.Text & ", " &
                        txtCBH40mx10.Text & ", '" &
                        cmbOnOff.Text & "', '" &
                        strNewRuleNote & "' )"
                Else 'Deals with rock,water,ag .....
                    strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                        "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                        "CBH13mx10, CBH40mx10, OnOff, Notes) " &
                        "VALUES (" & EVT & ", " & DIST & ", " &
                        cmbCoverLow.Text & ", " &
                        cmbCoverHigh.Text & ", " &
                        cmbHeightLow.Text & ", " &
                        cmbHeightHigh.Text & ", '" &
                        gf_GetNum(cmbBPSRule.Text, "General") & "', '" &
                        cmbWildRule.Text & "', " &
                        gf_GetNum(cmbFBFM13.Text, "General") & ", '" &
                        Trim(Strings.Left(cmbFBFM40.Text, 9)) & "', '" &
                        Trim(Strings.Left(cmbCanFM.Text, 9)) & "', " &
                        gf_GetNum(cmbFCCS.Text, "General") & ", " &
                        gf_GetNum(cmbFLM.Text, "General") & ", " &
                        gf_GetNum(cmbCanopy.Text, "General") & ", " &
                        cmbCC.Text & ", " &
                        cmbCH.Text & ", " &
                        txtCBD13x100.Text & ", " &
                        txtCBD40x100.Text & ", " &
                        txtCBH13mx10.Text & ", " &
                        txtCBH40mx10.Text & ", '" &
                        cmbOnOff.Text & "', '" &
                        strNewRuleNote & "' )"
                End If

                'Open database, run the SQL statement, close the database
                dbconn.Execute(strSQL)

                'Sleep for one second while the database catches up
                Threading.Thread.Sleep(1000)

                gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset

                Visible = False                  'Removes the form
            Else
                MsgBox("Make sure you fill in all blanks " _
                       & vbCrLf & "with valid values")
            End If

            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If

            MsgBox("Error in cmdAddSave_Click - " & ex.Message)
        End Try
    End Sub

    Private Sub cmdDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDone.Click
        Try
            Dim strNewNote As String

            'Convert Canopy Height range to midpoint
            'Convert Canopy Height midpoint to x10
            If CDbl(cmbCH.Text) > 0 And CDbl(cmbCH.Text) < 100 Then
                cmbCH.Items.Add(CDbl(cmbCH.Text) * 10)
                cmbCH.Text = CDbl(cmbCH.Text) * 10
            Else
                'Do nothing
            End If

            'Create new note string
            strNewNote = vbCrLf & Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                                SN & ": Changed "

            If ruleAOE.StrCovLow <> cmbCoverLow.Text Then
                strNewNote = strNewNote & "  (" & ruleAOE.StrCovLow & ") to (" & cmbCoverLow.Text & ")"
                ruleAOE.StrCovLow = cmbCoverLow.Text
            End If

            If ruleAOE.StrCovHigh <> cmbCoverHigh.Text Then
                strNewNote = strNewNote & "  (" & ruleAOE.StrCovHigh & " ) to ( " & cmbCoverHigh.Text & ")"
                ruleAOE.StrCovHigh = cmbCoverHigh.Text
            End If

            If ruleAOE.StrHgtLow <> cmbHeightLow.Text Then
                strNewNote = strNewNote & "  (" & ruleAOE.StrHgtLow & " ) to ( " & cmbHeightLow.Text & ")"
                ruleAOE.StrHgtLow = cmbHeightLow.Text
            End If

            If ruleAOE.StrHgtHigh <> cmbHeightHigh.Text Then
                strNewNote = strNewNote & "  (" & ruleAOE.StrHgtHigh & " ) to ( " & cmbHeightHigh.Text & ")"
                ruleAOE.StrHgtHigh = cmbHeightHigh.Text
            End If

            If ruleAOE.BPS <> gf_GetNum(cmbBPSRule.Text, "General") Then
                strNewNote = strNewNote & "  (" & ruleAOE.BPS & " ) to ( " & gf_GetNum(cmbBPSRule.Text, "General") & ")"
                ruleAOE.BPS = gf_GetNum(cmbBPSRule.Text, "General")
            End If

            If ruleAOE.Wildcard <> cmbWildRule.Text Then
                strNewNote = strNewNote & "  (" & Trim(Strings.Left(ruleAOE.Wildcard, 13)) & " ) to ( " &
                                                      Trim(Strings.Left(cmbWildRule.Text, 13)) & ")"
                ruleAOE.Wildcard = cmbWildRule.Text
            End If

            If ruleAOE.FBFM13 <> gf_GetNum(cmbFBFM13.Text, "General") Then
                strNewNote = strNewNote & "  (" & ruleAOE.FBFM13 & " ) to ( " & gf_GetNum(cmbFBFM13.Text, "General") & ")"
                ruleAOE.FBFM13 = gf_GetNum(cmbFBFM13.Text, "General")
            End If

            If ruleAOE.FBFM40 <> Trim(Strings.Left(cmbFBFM40.Text, 9)) Then
                strNewNote = strNewNote & "  (" & ruleAOE.FBFM40 & " ) to ( " & Trim(Strings.Left(cmbFBFM40.Text, 9)) & ")"
                ruleAOE.FBFM40 = Trim(Strings.Left(cmbFBFM40.Text, 9))
            End If

            If ruleAOE.CanFM <> Trim(Strings.Left(cmbCanFM.Text, 9)) Then
                strNewNote = strNewNote & "  (" & ruleAOE.CanFM & " ) to ( " & Trim(Strings.Left(cmbCanFM.Text, 9)) & ")"
                ruleAOE.CanFM = Trim(Strings.Left(cmbCanFM.Text, 9))
            End If

            If ruleAOE.FCCS <> gf_GetNum(cmbFCCS.Text, "General") Then
                strNewNote = strNewNote & "  (" & ruleAOE.FCCS & " ) to ( " & gf_GetNum(cmbFCCS.Text, "General") & ")"
                ruleAOE.FCCS = gf_GetNum(cmbFCCS.Text, "General")
            End If

            If ruleAOE.FLM <> gf_GetNum(cmbFLM.Text, "General") Then
                strNewNote = strNewNote & "  (" & ruleAOE.FLM & " ) to ( " & gf_GetNum(cmbFLM.Text, "General") & ")"
                ruleAOE.FLM = gf_GetNum(cmbFLM.Text, "General")
            End If

            If ruleAOE.Canopy <> gf_GetNum(cmbCanopy.Text, "General") Then
                strNewNote = strNewNote & "  (" & ruleAOE.Canopy & " ) to ( " & gf_GetNum(cmbCanopy.Text, "General") & ")"
                ruleAOE.Canopy = gf_GetNum(cmbCanopy.Text, "General")
            End If

            If ruleAOE.CCover <> cmbCC.Text Then
                strNewNote = strNewNote & "  (" & ruleAOE.CCover & " ) to ( " & cmbCC.Text & ")"
                ruleAOE.CCover = cmbCC.Text
            End If

            If ruleAOE.CHeight <> cmbCH.Text Then
                strNewNote = strNewNote & "  (" & ruleAOE.CHeight & " ) to ( " & cmbCH.Text & ")"
                ruleAOE.CHeight = cmbCH.Text
            End If

            If ruleAOE.CBD13 <> txtCBD13x100.Text Then
                If IsNumeric(txtCBD13x100.Text) Then
                    strNewNote = strNewNote & "  (" & ruleAOE.CBD13 & " ) to ( " & txtCBD13x100.Text & ")"
                    ruleAOE.CBD13 = txtCBD13x100.Text
                Else
                    MsgBox(txtCBD13x100.Text & " is not a valid number")
                End If
            End If

            If ruleAOE.CBD40 <> txtCBD40x100.Text Then
                If IsNumeric(txtCBD40x100.Text) Then
                    strNewNote = strNewNote & "  (" & ruleAOE.CBD40 & " ) to ( " & txtCBD40x100.Text & ")"
                    ruleAOE.CBD40 = txtCBD40x100.Text
                Else
                    MsgBox(txtCBD40x100.Text & " is not a valid number")
                End If
            End If

            If ruleAOE.CBH13 <> txtCBH13mx10.Text Then
                If IsNumeric(txtCBH13mx10.Text) Then
                    strNewNote = strNewNote & "  (" & ruleAOE.CBH13 & " ) to ( " & txtCBH13mx10.Text & ")"
                    ruleAOE.CBH13 = txtCBH13mx10.Text
                Else
                    MsgBox(txtCBH13mx10.Text & " is not a valid number")
                End If
            End If

            If ruleAOE.CBH40 <> txtCBH40mx10.Text Then
                If IsNumeric(txtCBH13mx10.Text) Then
                    strNewNote = strNewNote & "  (" & ruleAOE.CBH40 & " ) to ( " & txtCBH40mx10.Text & ")"
                    ruleAOE.CBH40 = txtCBH40mx10.Text
                Else
                    MsgBox(txtCBH40mx10.Text & " is not a valid number")
                End If
            End If

            If ruleAOE.OnOff <> cmbOnOff.Text Then
                strNewNote = strNewNote & "  (" & ruleAOE.OnOff & " ) to ( " & cmbOnOff.Text & ")"
                ruleAOE.OnOff = cmbOnOff.Text
            End If
            ruleAOE.Notes = ruleAOE.Notes & strNewNote

            gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset

            Visible = False 'Closes Editor form
        Catch ex As Exception
            MsgBox("Error in cmdDone_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub InitAllCMB()
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            'Populate Cover low comboboxes with coded values
            PopCovHgt(cmbCoverLow)

            'Populate cover high combo box with coded values
            If IsNumeric(cmbCoverLow.Items(0)) Then 'This number does not represent cover. It is rock,barren.....
                cmbCoverHigh.Items.Clear()
                cmbCoverHigh.Items.Add(cmbCoverLow.Items(0))
                PopCovHgt(cmbCoverHigh)
            Else
                PopCovHgt(cmbCoverHigh)
            End If

            'Populate Height low combo boxes with coded values
            If IsNumeric(cmbCoverLow.Items(0)) Then 'This number does not represent height. It is rock,barren.....
                If cmbCoverLow.Items(0) < 100 Then
                    cmbHeightLow.Items.Clear()
                    cmbHeightLow.Items.Add(cmbCoverLow.Items(0))
                    PopCovHgt(cmbHeightLow)
                Else
                    PopCovHgt(cmbHeightLow)
                End If
            Else
                PopCovHgt(cmbHeightLow)
            End If

            'Populate Height high combo boxes with coded values
            If IsNumeric(cmbCoverLow.Items(0)) Then 'This number does not represent height. It is rock,barren.....
                If cmbCoverLow.Items(0) < 100 Then
                    cmbHeightHigh.Items.Clear()
                    cmbHeightHigh.Items.Add(cmbCoverLow.Items(0))
                    PopCovHgt(cmbHeightHigh)
                Else
                    PopCovHgt(cmbHeightHigh)
                End If
            Else
                PopCovHgt(cmbHeightHigh)
            End If

            'Populate Canopy Cover combobox with coded values
            strSQL = "SELECT LUT_Cover.MidPoint, LUT_Cover.Lifeform " &
                     "FROM LUT_Cover WHERE (((LUT_Cover.Lifeform)='Tree'))"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            cmbCC.Items.Add("9999")
            Do While rs1.EOF <> True
                cmbCC.Items.Add(rs1.Fields!MidPoint.Value)
                rs1.MoveNext()
            Loop

            'Populate Canopy Height combobox with coded values
            strSQL = "SELECT LUT_Height.MidPoint, LUT_Height.Lifeform " &
                     "FROM LUT_Height WHERE (((LUT_Height.Lifeform)='Tree'))"
            rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            cmbCH.Items.Add("9999")
            Do While rs2.EOF <> True
                cmbCH.Items.Add(rs2.Fields!MidPoint.Value)
                rs2.MoveNext()
            Loop

            'Populate the BPSRule box with the new BPSs for the selected EVT
            cmbBPSRule.Items.Add("any")

            strSQL = "SELECT " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                                     "FROM " & comboR & " " &
                                     "LEFT JOIN LUT_BPS ON " & comboR & ".BPSRF = LUT_BPS.BPS " &
                                    "WHERE (EVTR = " & EVT &
                                    " And DIST = " & DIST & ")" &
                                     " GROUP BY " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                                     " ORDER BY BPSRF"

            gf_SetControl(cmbBPSRule, strSQL, strProjectPath) 'Fill the cmbBPSRule with value

            'Populate the WildRule box with the new Wildcard values for the selected EVT
            cmbWildRule.Items.Add("any")

            strSQL = "Select " & comboR & ".WILDCARD " &
                     "FROM(" & comboR & ") " &
                     "GROUP BY " & comboR & ".WILDCARD, " & comboR & ".EVTR, " & comboR & ".DIST " &
                     "HAVING(((" & comboR & ".EVTR) = " & EVT & ") And ((" & comboR & ".DIST) = " & DIST & ")) " &
                     "ORDER BY " & comboR & ".WILDCARD"

            gf_SetControl(cmbWildRule, strSQL, strProjectPath) 'Fill the cmbWildWild with values

            'Populate FBFM13 combobox without custom and FBFM40
            strSQL = "SELECT FMNum, FMName " &
                    "FROM LUT_FuelModelParameters " &
                    "WHERE (LUT_FuelModelParameters.Creator = 'Anderson13' Or " &
                    "LUT_FuelModelParameters.Creator = 'Nonburnable') " &
                    "ORDER BY FMNum"

            cmbFBFM13.Items.Add("9999   Nothing Assigned")
            gf_SetControl(cmbFBFM13, strSQL, strProjectPath) 'Fill the cmbFBFM13 with values

            'Populate FBFM40 combobox without custom and FBFM13
            strSQL = "SELECT FMNum, FMCode, FMName " &
                     "FROM LUT_FuelModelParameters " &
                     "WHERE (LUT_FuelModelParameters.Creator = 'ScottAndBurgan40' Or " &
                     "LUT_FuelModelParameters.Creator = 'Nonburnable') " &
                     " ORDER BY FMNum"

            cmbFBFM40.Items.Add("     9999   Nothing Assigned")
            gf_SetControl(cmbFBFM40, strSQL, strProjectPath) 'Fill the cmbFBFM40 with values

            'Populate CanFM combobox
            strSQL = "SELECT FM, Description " &
                     "FROM LUT_Canadian_FBPS_Fuel_Types " &
                     "WHERE (((LUT_Canadian_FBPS_Fuel_Types.FMID)<>0 And (LUT_Canadian_FBPS_Fuel_Types.FMID)<>-9999)) " &
                     "ORDER BY ID"

            gf_SetControl(cmbCanFM, strSQL, strProjectPath) 'Fill the cmbCanFM with values

            'Populate FCCS combobox
            strSQL = "SELECT ID_Num, FCCS, Description " &
                     "FROM LUT_FCCS_FERA " &
                     " ORDER BY ID"

            gf_SetControl(cmbFCCS, strSQL, strProjectPath) 'Fill the cmbFCCS with values

            'Populate FLM combobox
            strSQL = "SELECT FLM, Description " &
                        "FROM LUT_FLM_Lutes " &
                        " ORDER BY ID"

            gf_SetControl(cmbFLM, strSQL, strProjectPath) 'Fill the cmbFLM with values

            'Populate Canopy combobox
            strSQL = "SELECT Canopy_Fuel_Mask, Description " &
                     "FROM LUT_Canopy_Fuel_Mask " &
                     " ORDER BY ID"

            gf_SetControl(cmbCanopy, strSQL, strProjectPath) 'Fill the cmbCanopy with values

            'Populate On/Off combobox
            With cmbOnOff
                .Items.Add("On")
                .Items.Add("Off")
            End With

            'Convert codes for cove rand height comboboxes
            ConvertCodecmbCovHgt()

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in InitAllCMB - " & ex.Message)
        End Try
    End Sub

    Private Sub ConvertCodecmbCovHgt()

        'Convert codes for cover
        For i = 0 To cmbCoverLow.Items.Count - 1
            If IsNumeric(cmbCoverLow.Items(i)) Then
                If Int(cmbCoverLow.Items(i)) > 99 Then
                    cmbCoverLow.Items(i) = gf_ConvertCode(cmbCoverLow.Items(i), "cov", "low", strProjectPath)
                End If
            End If
        Next i

        'Convert codes for high cover comboboxes
        For i = 0 To cmbCoverHigh.Items.Count - 1
            If IsNumeric(cmbCoverHigh.Items(i)) Then
                If Int(cmbCoverHigh.Items(i)) > 99 Then
                    cmbCoverHigh.Items(i) = gf_ConvertCode(cmbCoverHigh.Items(i), "cov", "high", strProjectPath)
                End If
            End If
        Next i

        'Convert codes for low height combobox
        For i = 0 To cmbHeightLow.Items.Count - 1
            If IsNumeric(cmbHeightLow.Items(i)) Then
                If Int(cmbHeightLow.Items(i)) > 99 Then
                    cmbHeightLow.Items(i) = gf_ConvertCode(cmbHeightLow.Items(i), "hgt", "low", strProjectPath)
                End If
            End If
        Next i

        'Convert codes for high height combobox
        For i = 0 To cmbHeightHigh.Items.Count - 1
            If IsNumeric(cmbHeightHigh.Items(i)) Then
                If Int(cmbHeightHigh.Items(i)) > 99 Then
                    cmbHeightHigh.Items(i) = gf_ConvertCode(cmbHeightHigh.Items(i), "hgt", "high", strProjectPath)
                End If
            End If
        Next i
    End Sub

    Private Sub PopCovHgt(ByVal cmbBox As ComboBox) 'Used to populate cover and height comboboxes
        'Declare variables
        Dim intExistingCovLow As Integer
        Dim intExistingHgtLow As Integer
        Dim strLifeForm As String
        Dim rs1, rs2, rs3, rs4, rs5, rs6 As New ADODB.Recordset         'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            If cmbBox.Name = "cmbCoverLow" Then 'Populate Low Cover
                If rdoLim.Checked Then
                    'Limited - Populate with just the cover values and lifeforms present in the grid
                    strSQL = "SELECT EVCR " &
                       "FROM " & comboR & " " &
                       "WHERE (EVTR = " & EVT & ") And (DIST = " & DIST & ")" &
                       " Group By EVCR" &
                       " ORDER BY EVCR"
                Else
                    'Unlimited - Populate with all the cover values of all lifeforms
                    strSQL = "SELECT EVC " &
                       "FROM LUT_Cover " &
                       " ORDER BY EVC"
                End If

                gf_SetControl(cmbBox, strSQL, strProjectPath)

            ElseIf cmbBox.Name = "cmbCoverHigh" Then 'Populate High Cover


                If Visible = True Then 'Form has already loaded
                    intExistingCovLow = Int(gf_ConvertBack(cmbCoverLow.SelectedItem, strProjectPath)) 'Gets the code value
                ElseIf AOE = "Add" Then 'Before form is shown
                    intExistingCovLow = cmbCoverLow.Items(0)
                ElseIf AOE = "Edit" Then 'Before form is shown
                    intExistingCovLow = Int(gf_ConvertBack(cmbCoverLow.Text, strProjectPath)) 'Gets the code value
                End If

                'Find lifeform from intExistingCovLow
                strSQL = "SELECT LUT_Cover.EVC, LUT_Cover.Lifeform " &
                         "FROM LUT_Cover WHERE (((LUT_Cover.EVC)=" & intExistingCovLow & "))"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                strLifeForm = rs1.Fields!Lifeform.Value

                If rdoLim.Checked Then
                    'Liited - Populate with just the cover values, present in the grid, for the selected lifeform
                    strSQL = "SELECT LUT_Cover.Lifeform, " & comboR & ".EVCR " &
                             "FROM " & comboR & " " &
                             "INNER JOIN LUT_Cover ON " & comboR & ".EVCR = LUT_Cover.EVC " &
                             "WHERE(((" & comboR & ".[EVTR]) = " & EVT & ") AND (" & comboR & ".[DIST] = " & DIST & ") AND " &
                             "((" & comboR & ".EVCR)>=" & intExistingCovLow & ")) " &
                             "GROUP BY LUT_Cover.Lifeform, " & comboR & ".EVCR " &
                             "HAVING (((LUT_Cover.Lifeform)='" & strLifeForm & "')) " &
                             "ORDER BY " & comboR & ".EVCR"

                    rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs2.EOF
                        cmbBox.Items.Add(rs2.Fields!EVCR.Value)
                        rs2.MoveNext()
                    Loop
                Else
                    'Unlimited - Populate with all the cover values of all lifeforms
                    strSQL = "SELECT EVC " &
                       "FROM LUT_Cover " &
                       " ORDER BY EVC"

                    rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs2.EOF
                        cmbBox.Items.Add(rs2.Fields!EVC.Value)
                        rs2.MoveNext()
                    Loop
                End If

            ElseIf cmbBox.Name = "cmbHeightLow" Then 'Populate low heights
                If Visible = True Then 'Form has already loaded
                    intExistingCovLow = Int(gf_ConvertBack(cmbCoverLow.SelectedItem, strProjectPath)) 'Gets the code value
                ElseIf AOE = "Add" Then 'Before form is shown
                    intExistingCovLow = cmbCoverLow.Items(0)
                ElseIf AOE = "Edit" Then 'Before form is shown
                    intExistingCovLow = Int(gf_ConvertBack(cmbCoverLow.Text, strProjectPath)) 'Gets the code value
                    intExistingHgtLow = Int(gf_ConvertBack(cmbHeightLow.Text, strProjectPath)) 'Gets the code value
                End If

                'Find lifeform from intExistingCovLow
                strSQL = "SELECT LUT_Cover.EVC, LUT_Cover.Lifeform " &
                         "FROM LUT_Cover WHERE (((LUT_Cover.EVC)=" & intExistingCovLow & "))"
                rs3.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                strLifeForm = rs3.Fields!Lifeform.Value

                If rdoLim.Checked Then
                    'Limited - Populate with just the height values, present in the grid, for the selected lifeform
                    strSQL = "SELECT LUT_Height.Lifeform, " & comboR & ".EVHR " &
                             "FROM " & comboR & " " &
                             "INNER JOIN LUT_Height ON " & comboR & ".EVHR = LUT_Height.EVH " &
                             "WHERE((" & comboR & ".[EVTR] = " & EVT & ") And (" & comboR & ".[DIST] = " & DIST & ")) " &
                             "GROUP BY LUT_Height.Lifeform, " & comboR & ".EVHR " &
                             "HAVING (LUT_Height.Lifeform ='" & strLifeForm & "') " &
                             "ORDER BY " & comboR & ".EVHR"

                    rs4.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs4.EOF
                        cmbBox.Items.Add(rs4.Fields!EVHR.Value)
                        rs4.MoveNext()
                    Loop
                Else
                    'Unlimited - Populate with all the cover values of all lifeforms
                    strSQL = "SELECT EVH " &
                       "FROM LUT_Height " &
                       " ORDER BY EVH"

                    rs4.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs4.EOF
                        cmbBox.Items.Add(rs4.Fields!EVH.Value)
                        rs4.MoveNext()
                    Loop
                End If

            ElseIf cmbBox.Name = "cmbHeightHigh" Then 'Populate high heights
                If Visible = True Then 'Form has already loaded
                    intExistingCovLow = Int(gf_ConvertBack(cmbCoverLow.SelectedItem, strProjectPath)) 'Gets the code value
                    If IsNumeric(cmbHeightLow.Text) Then
                        intExistingHgtLow = cmbHeightLow.Text 'Gets the code value
                    Else
                        intExistingHgtLow = Int(gf_ConvertBack(cmbHeightLow.Text, strProjectPath)) 'Gets the code value
                    End If
                ElseIf AOE = "Add" Then 'Before form is shown
                    intExistingCovLow = cmbCoverLow.Items(0)
                    intExistingHgtLow = cmbHeightLow.Items(0)
                ElseIf AOE = "Edit" Then 'Before form is shown
                    intExistingCovLow = Int(gf_ConvertBack(cmbCoverLow.Text, strProjectPath)) 'Gets the code value
                    intExistingHgtLow = Int(gf_ConvertBack(cmbHeightLow.Text, strProjectPath)) 'Gets the code value
                End If

                'Find lifeform from intExistingCovLow
                strSQL = "SELECT LUT_Cover.EVC, LUT_Cover.Lifeform " &
                         "FROM LUT_Cover WHERE (((LUT_Cover.EVC)=" & intExistingCovLow & "))"
                rs5.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                strLifeForm = rs5.Fields!Lifeform.Value

                If rdoLim.Checked Then
                    'Limited - Populate with just the height values, present in the grid, for the selected lifeform
                    strSQL = "SELECT LUT_Height.Lifeform, " & comboR & ".EVHR " &
                             "FROM " & comboR & " " &
                             "INNER JOIN LUT_Height ON " & comboR & ".EVHR = LUT_Height.EVH " &
                             "WHERE(((" & comboR & ".[EVTR]) = " & EVT & ") AND (" & comboR & ".[DIST] = " & DIST & ") AND " &
                             "((" & comboR & ".EVHR)>=" & intExistingHgtLow & ")) " &
                             "GROUP BY LUT_Height.Lifeform, " & comboR & ".EVHR " &
                             "HAVING (((LUT_Height.Lifeform)='" & strLifeForm & "')) " &
                             "ORDER BY " & comboR & ".EVHR"

                    rs6.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs6.EOF
                        cmbBox.Items.Add(rs6.Fields!EVHR.Value)
                        rs6.MoveNext()
                    Loop
                Else
                    'Unlimited - Populate with all the cover values of all lifeforms
                    strSQL = "SELECT EVH " &
                       "FROM LUT_Height " &
                       " ORDER BY EVH"

                    rs6.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs6.EOF
                        cmbBox.Items.Add(rs6.Fields!EVH.Value)
                        rs6.MoveNext()
                    Loop
                End If
            End If

            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing
                If rs2.State <> 0 Then rs2.Close()
                rs2 = Nothing
                If rs3.State <> 0 Then rs3.Close()
                rs3 = Nothing
                If rs4.State <> 0 Then rs4.Close()
                rs4 = Nothing
                If rs5.State <> 0 Then rs5.Close()
                rs5 = Nothing
                If rs6.State <> 0 Then rs6.Close()
                rs6 = Nothing

                If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing
                If rs2.State <> 0 Then rs2.Close()
                rs2 = Nothing
                If rs3.State <> 0 Then rs3.Close()
                rs3 = Nothing
                If rs4.State <> 0 Then rs4.Close()
                rs4 = Nothing
                If rs5.State <> 0 Then rs5.Close()
                rs5 = Nothing
                If rs6.State <> 0 Then rs6.Close()
                rs6 = Nothing

                If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
            End If
            MsgBox("Error in PopCovHgt_Click - " & ex.Message)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Try
            Close()
        Catch ex As Exception
            MsgBox("Error in cmdCancel_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoLim.Click
        Dim tempCL As String 'Stores original value

        Try
            tempCL = cmbCoverLow.Text
            cmbCoverLow.Items.Clear()
            InitAllCMB()
            If cmbCoverLow.Items.Contains(tempCL) Then
                cmbCoverLow.Text = tempCL
            Else
                cmbCoverLow.SelectedIndex = 0
            End If
            Call cmbCoverLow_SelectionChangeCommitted(sender, e)
            cmbCoverLow.Refresh()
        Catch ex As Exception
            MsgBox("Error in rdoSup_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoUnLim_Click(ByVal sender As Object, ByVal e As EventArgs) Handles rdoUnLim.Click
        Dim tempCL As String 'Stores original value

        Try
            tempCL = cmbCoverLow.Text
            cmbCoverLow.Items.Clear()
            InitAllCMB()
            cmbCoverLow.Text = tempCL
            cmbCoverLow.Refresh()
        Catch ex As Exception
            MsgBox("Error in rdoUnSup_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub chkAllowFM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllowFM.CheckedChanged
        Try
            cmbFBFM13.Items.Clear()
            cmbFBFM40.Items.Clear()

            If chkAllowFM.Checked Then
                'Populate FBFM13 combobox with custom and FBFM40
                strSQL = "SELECT FMNum, FMName " &
                        "FROM LUT_FuelModelParameters " &
                        "ORDER BY FMNum"
            Else
                'Populate FBFM13 combobox without custom and FBFM40
                strSQL = "SELECT FMNum, FMName " &
                        "FROM LUT_FuelModelParameters " &
                        "WHERE (LUT_FuelModelParameters.Creator = 'Anderson13' Or " &
                        "LUT_FuelModelParameters.Creator = 'Nonburnable') " &
                        "ORDER BY FMNum"
            End If
            cmbFBFM13.Items.Add("9999   Nothing Assigned")
            gf_SetControl(cmbFBFM13, strSQL, strProjectPath) 'Fill the cmbFBFM13 with values


            If chkAllowFM.Checked Then
                'Populate FBFM40 combobox with custom and FBFM13
                strSQL = "SELECT FMNum, FMCode, FMName " &
                         "FROM LUT_FuelModelParameters " &
                         " ORDER BY FMNum"

            Else
                'Populate FBFM40 combobox without custom and FBFM13
                strSQL = "SELECT FMNum, FMCode, FMName " &
                         "FROM LUT_FuelModelParameters " &
                         "WHERE (LUT_FuelModelParameters.Creator = 'ScottAndBurgan40' Or " &
                         "LUT_FuelModelParameters.Creator = 'Nonburnable') " &
                         " ORDER BY FMNum"

            End If
            cmbFBFM40.Items.Add("     9999   Nothing Assigned")
            gf_SetControl(cmbFBFM40, strSQL, strProjectPath) 'Fill the cmbFBFM40 with values

            cmbFBFM13.SelectedIndex = 0
            cmbFBFM40.SelectedIndex = 0
        Catch ex As Exception
            MsgBox("Error in chkAllowFM_CheckedChanged - " & ex.Message)
        End Try

    End Sub
End Class
