Imports System.Data
Imports System.Data.SQLite
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
        Dim strLifeformCL As String = ""
        Dim strLifeformTest As String = ""

        Try
            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

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

                '-------------------------------------------------------
                ' Cover High Validity Check
                '-------------------------------------------------------
                If TempCH_Code >= TempCL_Code And (TempCH_Code < (Math.Ceiling(TempCL_Code / 10) * 10)) Then
                    cmbCoverHigh.Items.Add(TempCH)
                    cmbCoverHigh.Text = TempCH
                Else
                    cmbCoverHigh.SelectedIndex = cmbCoverHigh.Items.Count - 1
                End If

                '-------------------------------------------------------
                ' Lifeform Lookup for Cover-Low Code (TempCL_Code)
                '-------------------------------------------------------
                Using cmd As New SQLiteCommand("SELECT Lifeform FROM LUT_Cover WHERE EVC = @code;", conn)
                    cmd.Parameters.AddWithValue("@code", TempCL_Code)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        strLifeformCL = result.ToString()
                    End If
                End Using

                '-------------------------------------------------------
                ' Lifeform Lookup for Height-Low Code (TempHL_Code)
                '-------------------------------------------------------
                Using cmd As New SQLiteCommand("SELECT Lifeform FROM LUT_Height WHERE EVH = @code;", conn)
                    cmd.Parameters.AddWithValue("@code", TempHL_Code)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        strLifeformTest = result.ToString()
                    End If
                End Using

                If strLifeformCL <> strLifeformTest Then bln_SameLifeform = False

                If bln_SameLifeform = True Then
                    cmbHeightLow.Items.Add(TempHL)
                    cmbHeightLow.Text = TempHL
                Else
                    cmbHeightLow.SelectedIndex = 0
                End If

                bln_SameLifeform = True 'Reset

                '-------------------------------------------------------
                ' Lifeform Lookup for Height-High Code (TempHH_Code)
                '-------------------------------------------------------
                Using cmd As New SQLiteCommand("SELECT Lifeform FROM LUT_Height WHERE EVH = @code;", conn)
                    cmd.Parameters.AddWithValue("@code", TempHH_Code)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        strLifeformTest = result.ToString()
                    End If
                End Using

                If strLifeformCL <> strLifeformTest Then bln_SameLifeform = False

                '-------------------------------------------------------
                ' Height-High Validity Check
                '-------------------------------------------------------
                If TempHH_Code >= TempHL_Code And bln_SameLifeform = True Then
                    cmbHeightHigh.Items.Add(TempHH)
                    cmbHeightHigh.Text = TempHH
                Else
                    cmbHeightHigh.SelectedIndex = cmbHeightHigh.Items.Count - 1
                End If

            End Using

        Catch ex As Exception
            MsgBox("Error in cmbCoverLow_SelectionChangeCommitted - " & ex.Message)
        End Try

    End Sub

    Private Sub cmbHeightLow_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbHeightLow.SelectionChangeCommitted

        Dim TempHH_Code As Integer = gf_ConvertBack(cmbHeightHigh.Text, strProjectPath)
        Dim TempHH As String = cmbHeightHigh.Text
        Dim TempHL_Code As Integer = gf_ConvertBack(cmbHeightLow.Text, strProjectPath)

        Dim bln_SameLifeform As Boolean = True
        Dim strLifeformHL As String = ""
        Dim strLifeformTest As String = ""

        Try
            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                'Clear values
                cmbHeightHigh.Items.Clear()

                'Repopulate the combobox
                PopCovHgt(cmbHeightHigh)

                'Convert code to text cover and height
                ConvertCodecmbCovHgt()

                '-------------------------------------------------------
                ' Look up lifeform for TempHL_Code
                '-------------------------------------------------------
                Using cmd As New SQLiteCommand("SELECT Lifeform FROM LUT_Height WHERE EVH = @code;", conn)
                    cmd.Parameters.AddWithValue("@code", TempHL_Code)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        strLifeformHL = result.ToString()
                    End If
                End Using

                '-------------------------------------------------------
                ' Look up lifeform for TempHH_Code
                '-------------------------------------------------------
                Using cmd As New SQLiteCommand("SELECT Lifeform FROM LUT_Height WHERE EVH = @code;", conn)
                    cmd.Parameters.AddWithValue("@code", TempHH_Code)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        strLifeformTest = result.ToString()
                    End If
                End Using

                ' Check lifeform match
                If strLifeformHL <> strLifeformTest Then bln_SameLifeform = False

                '-------------------------------------------------------
                ' Validate TempHH_Code
                '-------------------------------------------------------
                If TempHH_Code >= TempHL_Code And bln_SameLifeform = True Then
                    cmbHeightHigh.Items.Add(TempHH)
                    cmbHeightHigh.Text = TempHH
                Else
                    cmbHeightHigh.SelectedIndex = cmbHeightHigh.Items.Count - 1
                End If

            End Using

        Catch ex As Exception
            MsgBox("Error in cmbHeightLow_SelectionChangeCommitted - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdAddSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddSave.Click
        Try
            ' Convert canopy height midpoint to x10 if needed
            Dim chVal As Double
            If Double.TryParse(cmbCH.Text, chVal) Then
                If chVal > 0 AndAlso chVal < 100 Then
                    Dim newCH As Double = chVal * 10
                    cmbCH.Items.Add(newCH)
                    cmbCH.Text = newCH.ToString()
                End If
            End If

            ' Validate required fields
            If cmbCoverLow.Text <> "" AndAlso cmbCoverHigh.Text <> "" AndAlso
           cmbHeightLow.Text <> "" AndAlso cmbHeightHigh.Text <> "" AndAlso
           IsNumeric(txtCBD13x100.Text) AndAlso IsNumeric(txtCBD40x100.Text) AndAlso
           IsNumeric(txtCBH13mx10.Text) AndAlso IsNumeric(txtCBH40mx10.Text) Then

                ' -------------------------------------------
                ' Build the note string exactly like original
                ' -------------------------------------------
                Dim strNewRuleNote As String =
                Now.ToShortTimeString & " " &
                Now.ToShortDateString & " " &
                SN & ": NEW RULE  " &
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

                ' Need to set these prior to opening conn because gf_ConvertBack uses a connection as well
                Dim cLowVal As Long
                Dim cHighVal As Long
                Dim hLowVal As Long
                Dim hHighVal As Long

                If EVT > 99 OrElse Not IsNumeric(cmbCoverLow.Text) Then
                    cLowVal = gf_ConvertBack(cmbCoverLow.Text, strProjectPath)
                    cHighVal = gf_ConvertBack(cmbCoverHigh.Text, strProjectPath)
                    hLowVal = gf_ConvertBack(cmbHeightLow.Text, strProjectPath)
                    hHighVal = gf_ConvertBack(cmbHeightHigh.Text, strProjectPath)
                End If

                ' -------------------------------------------
                ' Open SQLite connection
                ' -------------------------------------------
                Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                    conn.Open()

                    ' Build INSERT using parameters (safer than string concatenation)
                    Dim sql As String =
                    "INSERT INTO " & rulesR & " (" &
                    "EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                    "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, " &
                    "CCover, CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, Notes) " &
                    "VALUES (@EVT, @DIST, @CLow, @CHigh, @HLow, @HHigh, @BPSRF, @Wild, @FBFM13, @FBFM40, " &
                    "@CanFM, @FCCS, @FLM, @Canopy, @CCover, @CHeight, @CBD13, @CBD40, @CBH13, @CBH40, @OnOff, @Notes)"

                    Using cmd As New SQLiteCommand(sql, conn)

                        ' ----- Parameters for both cases -----
                        cmd.Parameters.AddWithValue("@EVT", EVT)
                        cmd.Parameters.AddWithValue("@DIST", DIST)

                        ' Cover/Height except EVT special-case
                        If EVT > 99 OrElse Not IsNumeric(cmbCoverLow.Text) Then
                            cmd.Parameters.AddWithValue("@CLow", cLowVal)
                            cmd.Parameters.AddWithValue("@CHigh", cHighVal)
                            cmd.Parameters.AddWithValue("@HLow", hLowVal)
                            cmd.Parameters.AddWithValue("@HHigh", hHighVal)
                        Else
                            cmd.Parameters.AddWithValue("@CLow", cmbCoverLow.Text)
                            cmd.Parameters.AddWithValue("@CHigh", cmbCoverHigh.Text)
                            cmd.Parameters.AddWithValue("@HLow", cmbHeightLow.Text)
                            cmd.Parameters.AddWithValue("@HHigh", cmbHeightHigh.Text)
                        End If

                        cmd.Parameters.AddWithValue("@BPSRF", gf_GetNum(cmbBPSRule.Text, "General"))
                        cmd.Parameters.AddWithValue("@Wild", cmbWildRule.Text)
                        cmd.Parameters.AddWithValue("@FBFM13", gf_GetNum(cmbFBFM13.Text, "General"))
                        cmd.Parameters.AddWithValue("@FBFM40", Trim(Strings.Left(cmbFBFM40.Text, 9)))
                        cmd.Parameters.AddWithValue("@CanFM", Trim(Strings.Left(cmbCanFM.Text, 9)))
                        cmd.Parameters.AddWithValue("@FCCS", gf_GetNum(cmbFCCS.Text, "General"))
                        cmd.Parameters.AddWithValue("@FLM", gf_GetNum(cmbFLM.Text, "General"))
                        cmd.Parameters.AddWithValue("@Canopy", gf_GetNum(cmbCanopy.Text, "General"))
                        cmd.Parameters.AddWithValue("@CCover", cmbCC.Text)
                        cmd.Parameters.AddWithValue("@CHeight", cmbCH.Text)
                        cmd.Parameters.AddWithValue("@CBD13", txtCBD13x100.Text)
                        cmd.Parameters.AddWithValue("@CBD40", txtCBD40x100.Text)
                        cmd.Parameters.AddWithValue("@CBH13", txtCBH13mx10.Text)
                        cmd.Parameters.AddWithValue("@CBH40", txtCBH40mx10.Text)
                        cmd.Parameters.AddWithValue("@OnOff", cmbOnOff.Text)
                        cmd.Parameters.AddWithValue("@Notes", strNewRuleNote)

                        cmd.ExecuteNonQuery()
                        End Using
                    End Using

                ' Let DB settle if required in your workflow
                Threading.Thread.Sleep(1000)

                ' Clear pixel, acres, percent
                ' gr_ClearPAP(RulesetCollection)

                ' Close form
                Visible = False

            Else
                MsgBox("Make sure you fill in all blanks" & vbCrLf & "with valid values")
            End If

        Catch ex As Exception
            MsgBox("Error in cmdAddSave_Click - " & ex.Message)
        End Try

    End Sub

    ' Helper sub for logging and assigning changes
    Private Sub AppendChange(ByRef note As String,
                         oldVal As String,
                         newVal As String,
                         updateAction As Action(Of String))

        If oldVal <> newVal Then
            note &= "  (" & oldVal & ") to (" & newVal & ")"
            updateAction(newVal)
        End If

    End Sub

    Private Sub cmdDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDone.Click
        Try
            Dim chVal As Double

            ' Convert Canopy Height midpoint (if needed)
            If Double.TryParse(cmbCH.Text, chVal) Then
                If chVal > 0 AndAlso chVal < 100 Then
                    Dim newCHMid As Double = chVal * 10
                    cmbCH.Items.Add(newCHMid)
                    cmbCH.Text = newCHMid.ToString()
                End If
            End If

            ' Start new note
            Dim strNewNote As String =
            vbCrLf & Now.ToShortTimeString() & " " & Now.ToShortDateString() & " " &
            SN & ": Changed "

            ' --- Cover Low / High ---
            AppendChange(strNewNote, ruleAOE.StrCovLow, cmbCoverLow.Text,
                     Sub(v) ruleAOE.StrCovLow = v)

            AppendChange(strNewNote, ruleAOE.StrCovHigh, cmbCoverHigh.Text,
                     Sub(v) ruleAOE.StrCovHigh = v)

            ' --- Height Low / High ---
            AppendChange(strNewNote, ruleAOE.StrHgtLow, cmbHeightLow.Text,
                     Sub(v) ruleAOE.StrHgtLow = v)

            AppendChange(strNewNote, ruleAOE.StrHgtHigh, cmbHeightHigh.Text,
                     Sub(v) ruleAOE.StrHgtHigh = v)

            ' --- BPS ---
            Dim newBPS As String = gf_GetNum(cmbBPSRule.Text, "General").ToString()
            AppendChange(strNewNote, ruleAOE.BPS.ToString(), newBPS,
                     Sub(v) ruleAOE.BPS = gf_GetNum(v, "General"))

            ' --- Wildcard ---
            Dim oldWild As String = Trim(Strings.Left(ruleAOE.Wildcard, 13))
            Dim newWild As String = Trim(Strings.Left(cmbWildRule.Text, 13))

            AppendChange(strNewNote, oldWild, newWild,
                     Sub(v) ruleAOE.Wildcard = cmbWildRule.Text)

            ' --- FBFM13 ---
            Dim newFBFM13 As String = gf_GetNum(cmbFBFM13.Text, "General").ToString()
            AppendChange(strNewNote, ruleAOE.FBFM13.ToString(), newFBFM13,
                     Sub(v) ruleAOE.FBFM13 = gf_GetNum(v, "General"))

            ' --- FBFM40 ---
            Dim newFBFM40 As String = Trim(Strings.Left(cmbFBFM40.Text, 9))
            AppendChange(strNewNote, ruleAOE.FBFM40, newFBFM40,
                     Sub(v) ruleAOE.FBFM40 = newFBFM40)

            ' --- CanFM ---
            Dim newCanFM As String = Trim(Strings.Left(cmbCanFM.Text, 9))
            AppendChange(strNewNote, ruleAOE.CanFM, newCanFM,
                     Sub(v) ruleAOE.CanFM = newCanFM)

            ' --- FCCS ---
            Dim newFCCS As String = gf_GetNum(cmbFCCS.Text, "General").ToString()
            AppendChange(strNewNote, ruleAOE.FCCS.ToString(), newFCCS,
                     Sub(v) ruleAOE.FCCS = gf_GetNum(v, "General"))

            ' --- FLM ---
            Dim newFLM As String = gf_GetNum(cmbFLM.Text, "General").ToString()
            AppendChange(strNewNote, ruleAOE.FLM.ToString(), newFLM,
                     Sub(v) ruleAOE.FLM = gf_GetNum(v, "General"))

            ' --- Canopy ---
            Dim newCanopy As String = gf_GetNum(cmbCanopy.Text, "General").ToString()
            AppendChange(strNewNote, ruleAOE.Canopy.ToString(), newCanopy,
                     Sub(v) ruleAOE.Canopy = gf_GetNum(v, "General"))

            ' --- CCover / CHeight ---
            AppendChange(strNewNote, ruleAOE.CCover, cmbCC.Text,
                     Sub(v) ruleAOE.CCover = v)

            AppendChange(strNewNote, ruleAOE.CHeight, cmbCH.Text,
                     Sub(v) ruleAOE.CHeight = v)

            ' --- CBD13 ---
            If ruleAOE.CBD13 <> txtCBD13x100.Text Then
                If IsNumeric(txtCBD13x100.Text) Then
                    AppendChange(strNewNote, ruleAOE.CBD13, txtCBD13x100.Text,
                             Sub(v) ruleAOE.CBD13 = v)
                Else
                    MsgBox(txtCBD13x100.Text & " is not a valid number")
                End If
            End If

            ' --- CBD40 ---
            If ruleAOE.CBD40 <> txtCBD40x100.Text Then
                If IsNumeric(txtCBD40x100.Text) Then
                    AppendChange(strNewNote, ruleAOE.CBD40, txtCBD40x100.Text,
                             Sub(v) ruleAOE.CBD40 = v)
                Else
                    MsgBox(txtCBD40x100.Text & " is not a valid number")
                End If
            End If

            ' --- CBH13 ---
            If ruleAOE.CBH13 <> txtCBH13mx10.Text Then
                If IsNumeric(txtCBH13mx10.Text) Then
                    AppendChange(strNewNote, ruleAOE.CBH13, txtCBH13mx10.Text,
                             Sub(v) ruleAOE.CBH13 = v)
                Else
                    MsgBox(txtCBH13mx10.Text & " is not a valid number")
                End If
            End If

            ' --- CBH40 (BUG FIXED: valid check must examine txtCBH40mx10) ---
            If ruleAOE.CBH40 <> txtCBH40mx10.Text Then
                If IsNumeric(txtCBH40mx10.Text) Then
                    AppendChange(strNewNote, ruleAOE.CBH40, txtCBH40mx10.Text,
                             Sub(v) ruleAOE.CBH40 = v)
                Else
                    MsgBox(txtCBH40mx10.Text & " is not a valid number")
                End If
            End If

            ' --- On/Off ---
            AppendChange(strNewNote, ruleAOE.OnOff, cmbOnOff.Text,
                     Sub(v) ruleAOE.OnOff = v)

            ' Add final note
            ruleAOE.Notes &= strNewNote

            ' Clear pixel count, acres, percent EVT in ruleset
            gr_ClearPAP(RulesetCollection)

            Visible = False

        Catch ex As Exception
            MsgBox("Error in cmdDone_Click - " & ex.Message)
        End Try
    End Sub

    Private Sub InitAllCMB()

        Try
            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                'Populate Cover low comboboxes with coded values
                PopCovHgt(cmbCoverLow)

                'Populate cover high combo box with coded values
                If IsNumeric(cmbCoverLow.Items(0)) Then
                    cmbCoverHigh.Items.Clear()
                    cmbCoverHigh.Items.Add(cmbCoverLow.Items(0))
                    PopCovHgt(cmbCoverHigh)
                Else
                    PopCovHgt(cmbCoverHigh)
                End If

                'Populate Height low combo boxes with coded values
                If IsNumeric(cmbCoverLow.Items(0)) Then
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
                If IsNumeric(cmbCoverLow.Items(0)) Then
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

                '--------------------------------------------------------------------
                ' Populate Canopy Cover combobox (MidPoint values where Lifeform='Tree')
                '--------------------------------------------------------------------
                cmbCC.Items.Clear()
                cmbCC.Items.Add("9999")

                Using cmd As New SQLiteCommand(
                    "SELECT MidPoint 
                    FROM LUT_Cover 
                    WHERE Lifeform='Tree'
                    ORDER BY MidPoint;", conn)

                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            cmbCC.Items.Add(reader("MidPoint").ToString())
                        End While
                    End Using
                End Using

                '--------------------------------------------------------------------
                ' Populate Canopy Height combobox (MidPoint where Lifeform='Tree')
                '--------------------------------------------------------------------
                cmbCH.Items.Clear()
                cmbCH.Items.Add("9999")

                Using cmd As New SQLiteCommand(
                    "SELECT MidPoint
                    FROM LUT_Height
                    WHERE Lifeform='Tree'
                    ORDER BY MidPoint;", conn)

                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            cmbCH.Items.Add(reader("MidPoint").ToString())
                        End While
                    End Using
                End Using

                '--------------------------------------------------------------------
                ' Populate BPSRule combobox using gf_SetControl
                '--------------------------------------------------------------------
                cmbBPSRule.Items.Clear()
                cmbBPSRule.Items.Add("any")

                Dim sqlBPS As String =
                    "SELECT " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                    "FROM " & comboR & " " &
                    "LEFT JOIN LUT_BPS ON " & comboR & ".BPSRF = LUT_BPS.BPS " &
                    "WHERE (EVTR = " & EVT & " And DIST = " & DIST & ") " &
                    "GROUP BY " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                    "ORDER BY BPSRF"

                gf_SetControl(cmbBPSRule, sqlBPS, strProjectPath, theconn:=conn)

                '--------------------------------------------------------------------
                ' Populate WildRule combobox using gf_SetControl
                '--------------------------------------------------------------------
                cmbWildRule.Items.Clear()
                cmbWildRule.Items.Add("any")

                Dim sqlWild As String =
                    "SELECT " & comboR & ".WILDCARD " &
                    "FROM " & comboR & " " &
                    "GROUP BY " & comboR & ".WILDCARD, " & comboR & ".EVTR, " & comboR & ".DIST " &
                    "HAVING ((" & comboR & ".EVTR) = " & EVT & ") AND ((" & comboR & ".DIST) = " & DIST & ") " &
                    "ORDER BY " & comboR & ".WILDCARD"

                gf_SetControl(cmbWildRule, sqlWild, strProjectPath, theconn:=conn)

                '--------------------------------------------------------------------
                ' Populate FBFM13 (Anderson13 + Nonburnable)
                '--------------------------------------------------------------------
                cmbFBFM13.Items.Clear()
                cmbFBFM13.Items.Add("9999   Nothing Assigned")

                Dim sqlFBFM13 As String =
                    "SELECT FMNum, FMName " &
                    "FROM LUT_FuelModelParameters " &
                    "WHERE Creator='Anderson13' OR Creator='Nonburnable' " &
                    "ORDER BY FMNum"

                gf_SetControl(cmbFBFM13, sqlFBFM13, strProjectPath, theconn:=conn)

                '--------------------------------------------------------------------
                ' Populate FBFM40 (Scott&Burgan40 + Nonburnable)
                '--------------------------------------------------------------------
                cmbFBFM40.Items.Clear()
                cmbFBFM40.Items.Add("     9999   Nothing Assigned")

                Dim sqlFBFM40 As String =
                    "SELECT FMNum, FMCode, FMName " &
                    "FROM LUT_FuelModelParameters " &
                    "WHERE Creator='ScottAndBurgan40' OR Creator='Nonburnable' " &
                    "ORDER BY FMNum"

                gf_SetControl(cmbFBFM40, sqlFBFM40, strProjectPath, theconn:=conn)

                '--------------------------------------------------------------------
                ' Populate CanFM
                '--------------------------------------------------------------------
                Dim sqlCanFM As String =
                    "SELECT FM, Description " &
                    "FROM LUT_Canadian_FBPS_Fuel_Types " &
                    "WHERE FMID <> 0 AND FMID <> -9999 " &
                    "ORDER BY ID"

                gf_SetControl(cmbCanFM, sqlCanFM, strProjectPath, theconn:=conn)

                '--------------------------------------------------------------------
                ' Populate FCCS
                '--------------------------------------------------------------------
                Dim sqlFCCS As String =
                    "SELECT ID_Num, FCCS, Description " &
                    "FROM LUT_FCCS_FERA " &
                    "ORDER BY ID"

                gf_SetControl(cmbFCCS, sqlFCCS, strProjectPath, theconn:=conn)

                '--------------------------------------------------------------------
                ' Populate FLM
                '--------------------------------------------------------------------
                Dim sqlFLM As String =
                    "SELECT FLM, Description " &
                    "FROM LUT_FLM_Lutes " &
                    "ORDER BY ID"

                gf_SetControl(cmbFLM, sqlFLM, strProjectPath, theconn:=conn)

                '--------------------------------------------------------------------
                ' Populate Canopy
                '--------------------------------------------------------------------
                Dim sqlCanopy As String =
                    "SELECT Canopy_Fuel_Mask, Description " &
                    "FROM LUT_Canopy_Fuel_Mask " &
                    "ORDER BY ID"

                gf_SetControl(cmbCanopy, sqlCanopy, strProjectPath, theconn:=conn)

                '--------------------------------------------------------------------
                ' Populate On/Off
                '--------------------------------------------------------------------
                With cmbOnOff
                    .Items.Clear()
                    .Items.Add("On")
                    .Items.Add("Off")
                End With

                'Convert codes for cover and height comboboxes
                ConvertCodecmbCovHgt()

            End Using

        Catch ex As Exception
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

    Private Sub PopCovHgt(ByVal cmbBox As ComboBox)

        Dim intExistingCovLow As Integer = 0
        Dim intExistingHgtLow As Integer = 0
        Dim strLifeForm As String = ""
        Try
            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                '-----------------------------------------------------------
                ' Populate CmbCoverLow (Low Cover)
                '-----------------------------------------------------------
                If cmbBox.Name = "cmbCoverLow" Then

                    Dim sql As String
                    If rdoLim.Checked Then
                        sql =
                        "SELECT EVCR FROM " & comboR &
                        " WHERE EVTR = " & EVT & " AND DIST = " & DIST &
                        " GROUP BY EVCR ORDER BY EVCR"
                    Else
                        sql = "SELECT EVC FROM LUT_Cover ORDER BY EVC"
                    End If

                    gf_SetControl(cmbBox, sql, strProjectPath, theconn:=conn)
                    Return
                End If

                '-----------------------------------------------------------
                ' Populate CmbCoverHigh (High Cover)
                '-----------------------------------------------------------
                If cmbBox.Name = "cmbCoverHigh" Then

                    'Determine existing low cover selection
                    If Visible = True Then
                        intExistingCovLow = gf_ConvertBack(cmbCoverLow.SelectedItem, strProjectPath)
                    ElseIf AOE = "Add" Then
                        intExistingCovLow = cmbCoverLow.Items(0)
                    ElseIf AOE = "Edit" Then
                        intExistingCovLow = gf_ConvertBack(cmbCoverLow.Text, strProjectPath)
                    End If

                    'Look up lifeform for existing low cover
                    Using cmd As New SQLiteCommand("SELECT Lifeform FROM LUT_Cover WHERE EVC=@code", conn)
                        cmd.Parameters.AddWithValue("@code", intExistingCovLow)
                        Dim result = cmd.ExecuteScalar()
                        If result IsNot Nothing Then strLifeForm = result.ToString()
                    End Using

                    If rdoLim.Checked Then
                        'Limited → only cover values present in grid for the same lifeform
                        Dim sql As String =
                        "SELECT " & comboR & ".EVCR " &
                        "FROM " & comboR &
                        " INNER JOIN LUT_Cover ON " & comboR & ".EVCR = LUT_Cover.EVC " &
                        "WHERE " & comboR & ".EVTR=" & EVT &
                        " AND " & comboR & ".DIST=" & DIST &
                        " AND " & comboR & ".EVCR >= " & intExistingCovLow &
                        " AND LUT_Cover.Lifeform='" & strLifeForm & "' " &
                        "GROUP BY " & comboR & ".EVCR " &
                        "ORDER BY " & comboR & ".EVCR"

                        cmbBox.Items.Clear()
                        Using cmd As New SQLiteCommand(sql, conn)
                            Using rd = cmd.ExecuteReader()
                                While rd.Read()
                                    cmbBox.Items.Add(rd("EVCR"))
                                End While
                            End Using
                        End Using

                    Else
                        'Unlimited → all cover values
                        Dim sql As String = "SELECT EVC FROM LUT_Cover ORDER BY EVC"
                        cmbBox.Items.Clear()
                        Using cmd As New SQLiteCommand(sql, conn)
                            Using rd = cmd.ExecuteReader()
                                While rd.Read()
                                    cmbBox.Items.Add(rd("EVC"))
                                End While
                            End Using
                        End Using
                    End If

                    Return
                End If

                '-----------------------------------------------------------
                ' Populate CmbHeightLow (Low Height)
                '-----------------------------------------------------------
                If cmbBox.Name = "cmbHeightLow" Then

                    'Determine existing cover/height low selections
                    If Visible = True Then
                        intExistingCovLow = gf_ConvertBack(cmbCoverLow.SelectedItem, strProjectPath)
                    ElseIf AOE = "Add" Then
                        intExistingCovLow = cmbCoverLow.Items(0)
                    ElseIf AOE = "Edit" Then
                        intExistingCovLow = gf_ConvertBack(cmbCoverLow.Text, strProjectPath)
                        intExistingHgtLow = gf_ConvertBack(cmbHeightLow.Text, strProjectPath)
                    End If

                    'Lifeform for the selected cover-low
                    Using cmd As New SQLiteCommand("SELECT Lifeform FROM LUT_Cover WHERE EVC=@code", conn)
                        cmd.Parameters.AddWithValue("@code", intExistingCovLow)
                        Dim result = cmd.ExecuteScalar()
                        If result IsNot Nothing Then strLifeForm = result.ToString()
                    End Using

                    If rdoLim.Checked Then
                        'Limited → height values present in grid for same lifeform
                        Dim sql As String =
                        "SELECT " & comboR & ".EVHR " &
                        "FROM " & comboR &
                        " INNER JOIN LUT_Height ON " & comboR & ".EVHR = LUT_Height.EVH " &
                        "WHERE " & comboR & ".EVTR=" & EVT &
                        " AND " & comboR & ".DIST=" & DIST &
                        " AND LUT_Height.Lifeform='" & strLifeForm & "' " &
                        "GROUP BY " & comboR & ".EVHR " &
                        "ORDER BY " & comboR & ".EVHR"

                        cmbBox.Items.Clear()
                        Using cmd As New SQLiteCommand(sql, conn)
                            Using rd = cmd.ExecuteReader()
                                While rd.Read()
                                    cmbBox.Items.Add(rd("EVHR"))
                                End While
                            End Using
                        End Using

                    Else
                        'Unlimited → all height values
                        Dim sql As String = "SELECT EVH FROM LUT_Height ORDER BY EVH"
                        cmbBox.Items.Clear()
                        Using cmd As New SQLiteCommand(sql, conn)
                            Using rd = cmd.ExecuteReader()
                                While rd.Read()
                                    cmbBox.Items.Add(rd("EVH"))
                                End While
                            End Using
                        End Using
                    End If

                    Return
                End If

                '-----------------------------------------------------------
                ' Populate CmbHeightHigh (High Height)
                '-----------------------------------------------------------
                If cmbBox.Name = "cmbHeightHigh" Then

                    'Determine existing cover and height low selection
                    If Visible = True Then
                        intExistingCovLow = gf_ConvertBack(cmbCoverLow.SelectedItem, strProjectPath)
                        If IsNumeric(cmbHeightLow.Text) Then
                            intExistingHgtLow = cmbHeightLow.Text
                        Else
                            intExistingHgtLow = gf_ConvertBack(cmbHeightLow.Text, strProjectPath)
                        End If
                    ElseIf AOE = "Add" Then
                        intExistingCovLow = cmbCoverLow.Items(0)
                        intExistingHgtLow = cmbHeightLow.Items(0)
                    ElseIf AOE = "Edit" Then
                        intExistingCovLow = gf_ConvertBack(cmbCoverLow.Text, strProjectPath)
                        intExistingHgtLow = gf_ConvertBack(cmbHeightLow.Text, strProjectPath)
                    End If

                    'Lifeform from cover-low code
                    Using cmd As New SQLiteCommand("SELECT Lifeform FROM LUT_Cover WHERE EVC=@code", conn)
                        cmd.Parameters.AddWithValue("@code", intExistingCovLow)
                        Dim result = cmd.ExecuteScalar()
                        If result IsNot Nothing Then strLifeForm = result.ToString()
                    End Using

                    If rdoLim.Checked Then
                        'Limited → height values present in grid, same lifeform, >= low height
                        Dim sql As String =
                        "SELECT " & comboR & ".EVHR " &
                        "FROM " & comboR &
                        " INNER JOIN LUT_Height ON " & comboR & ".EVHR = LUT_Height.EVH " &
                        "WHERE " & comboR & ".EVTR=" & EVT &
                        " AND " & comboR & ".DIST=" & DIST &
                        " AND " & comboR & ".EVHR >= " & intExistingHgtLow &
                        " AND LUT_Height.Lifeform='" & strLifeForm & "' " &
                        "GROUP BY " & comboR & ".EVHR " &
                        "ORDER BY " & comboR & ".EVHR"

                        cmbBox.Items.Clear()
                        Using cmd As New SQLiteCommand(sql, conn)
                            Using rd = cmd.ExecuteReader()
                                While rd.Read()
                                    cmbBox.Items.Add(rd("EVHR"))
                                End While
                            End Using
                        End Using

                    Else
                        'Unlimited → all height-high values
                        Dim sql As String = "SELECT EVH FROM LUT_Height ORDER BY EVH"
                        cmbBox.Items.Clear()
                        Using cmd As New SQLiteCommand(sql, conn)
                            Using rd = cmd.ExecuteReader()
                                While rd.Read()
                                    cmbBox.Items.Add(rd("EVH"))
                                End While
                            End Using
                        End Using
                    End If

                    Return
                End If
            End Using

        Catch ex As Exception
            MsgBox("Error in PopCovHgt - " & ex.Message)
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
