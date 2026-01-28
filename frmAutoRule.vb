Imports System.Data

Public Class frmAutoRule
    Private strSQL As String                                                'SQL variable for this module
    Private comboR As String                                                'Stores the combo table name for rule making
    Private rulesR As String                                                'Stores the rules table name for rule making
    Private EVTPixelCountCollection As Collection
    Private RulesetCollection As Collection
    Private strProjectPath As String
    Private cmbEVT As String
    Private strSessionName
    Private strAutoRuleSF As String                                         'Stores the selected value for the autorule surface fuel

    Public Sub New(ByVal SetComboR As String, ByVal SetRulesR As String, ByVal SetProjPath As String,
                   ByVal SetcmbEVT As String, ByVal SetSessionName As String, ByVal SetRulesetCol As Collection,
                   ByVal SetEVTPixelCountCollection As Collection)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        strProjectPath = SetProjPath

        'Set the MU
        comboR = SetComboR
        rulesR = SetRulesR

        'Set the collections to new collections
        RulesetCollection = SetRulesetCol
        EVTPixelCountCollection = SetEVTPixelCountCollection

        'Set the EVT
        cmbEVT = SetcmbEVT

        'Set Session Name
        strSessionName = SetSessionName
    End Sub

    Private Sub cmdGo_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdGo.Click
        'Set Surface fuel to autorule
        If rdoFM13.Checked Then SetAutoRuleCheck(rdoFM13.Name)
        If rdoFM40.Checked Then SetAutoRuleCheck(rdoFM40.Name)
        If rdoCanFM.Checked Then SetAutoRuleCheck(rdoCanFM.Name)
        If rdoFCCS.Checked Then SetAutoRuleCheck(rdoFCCS.Name)
        If rdoFLM.Checked Then SetAutoRuleCheck(rdoFLM.Name)

        AutoRule()
    End Sub

    Public Sub AutoRule()
        'Create rules from an existing fm grid in the wildcard slot
        Dim strNewRuleNote As String = ""                                   'Stores the new note
        Dim lngEVClow As Long                                               'EVC low cover number
        Dim lngEVChigh As Long                                              'EVC high cover number
        Dim lngEVHlow As Long                                               'EVH low height number
        Dim lngEVHhigh As Long                                              'EVH high height number
        Dim lngWildcard As Long                                             'Wildcard number
        Dim lngFM13 As Long                                                 'FM13 Wildcard number
        Dim lngFM40 As Long                                                 'FM40 Wildcard number
        Dim strFM40 As String
        Dim lngCanFM As Long                                                'CanFM Wildcard number
        Dim strCanFM As String
        Dim lngFCCS As Long                                                 'FCCS Wildcard number
        Dim lngFLM As Long                                                  'FLM Wildcard number
        Dim lngMaxHigh As Long                                              'Stores the tallest high height for the ruleset

        Dim rs1 As New ADODB.Recordset                                      'recordset for data
        Dim rs2 As New ADODB.Recordset                                      'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            'Set Default
            If rdoFM13.Name = strAutoRuleSF Then
                rdoFM13.Checked = True
            ElseIf rdoFM40.Name = strAutoRuleSF Then
                rdoFM40.Checked = True
            ElseIf rdoCanFM.Name = strAutoRuleSF Then
                rdoCanFM.Checked = True
            ElseIf rdoFCCS.Name = strAutoRuleSF Then
                rdoFCCS.Checked = True
            Else
                rdoFLM.Checked = True
            End If

            'If Majority exists drop it before you start
            Try
                strSQL = "DROP VIEW Majority"
                dbconn.Execute(strSQL)
            Catch
                'Ignore error
            End Try

            '   ******* Without BPS
            strSQL = "CREATE VIEW [Majority] AS " &
                    "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & comboR & ".EVCR, " &
                    comboR & ".EVHR, Max(" & comboR & ".COUNT) AS MaxOfCOUNT " &
                    "FROM " & comboR & " " &
                    "WHERE " & comboR & ".COUNT > 0 " &
                    "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & comboR & ".EVCR, " & comboR & ".EVHR "
            dbconn.Execute(strSQL)

            '   *******  With BPS
            '    strSQL = "CREATE VIEW [Majority] AS " & _
            '            "SELECT mv_CMB.EVTR, mv_CMB.BPSRF, mv_CMB.EVCR, mv_CMB.EVHR, Max(mv_CMB.COUNT) AS MaxOfCOUNT " & _
            '            "FROM mv_CMB " & _
            '            "GROUP BY mv_CMB.EVTR, mv_CMB.BPSRF, mv_CMB.EVCR, mv_CMB.EVHR "
            '    Mdbrs

            '   ******* Without BPS and ordered by EVCR first
            strSQL = "SELECT [Majority].EVTR, [Majority].DIST, [Majority].EVCR, [Majority].EVHR, " &
                    "[Majority].MaxOfCOUNT, " & comboR & ".WILDCARD " &
                    "FROM [Majority] INNER JOIN " & comboR & " " &
                    "ON ([Majority].MaxOfCOUNT = " & comboR & ".COUNT) AND " &
                    "([Majority].EVHR = " & comboR & ".EVHR) AND ([Majority].EVCR = " & comboR & ".EVCR) AND " &
                    "([Majority].EVTR = " & comboR & ".EVTR) AND ([Majority].DIST = " & comboR & ".DIST) " &
                    "WHERE ([Majority].EVTR = " & gf_GetNum(cmbEVT, "EVT") & ") " &
                    "And ([Majority].DIST = " & gf_GetNum(cmbEVT, "DIST") & ") " &
                    "ORDER BY " & comboR & ".EVCR, " & comboR & ".EVHR"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            'Filter through the assignments and group them into rules
            Do Until rs1.EOF
                lngEVClow = rs1.Fields!EVCR.Value
                lngEVChigh = rs1.Fields!EVCR.Value
                lngEVHlow = rs1.Fields!EVHR.Value
                lngEVHhigh = rs1.Fields!EVHR.Value
                lngFM13 = 9999 'Starting FM13 value 
                lngFM40 = 9999 'Starting FM40 value
                lngCanFM = 9999 'Starting CanFM value
                lngFCCS = 9999 'Starting FCCS value
                lngFLM = 9999 'Starting FLM value
                lngWildcard = rs1.Fields!Wildcard.Value
                If rdoFM13.Checked Then
                    lngFM13 = lngWildcard
                ElseIf rdoFM40.Checked Then
                    lngFM40 = lngWildcard
                ElseIf rdoCanFM.Checked Then
                    lngCanFM = lngWildcard
                ElseIf rdoFCCS.Checked Then
                    lngFCCS = lngWildcard
                ElseIf rdoFLM.Checked Then
                    lngFLM = lngWildcard
                End If

                rs1.MoveNext()

                'Find the starting cover and the ending cover of consecutive assignments.
                If rs1.EOF Then
                    'Do nothing
                ElseIf rs1.Fields!Wildcard.Value = lngWildcard Then 'Consecutive FMs
                    lngMaxHigh = lngEVHlow 'Reset the max height for the new rule
                    Do Until rs1.EOF
                        If rs1.Fields!Wildcard.Value <> lngWildcard Then Exit Do
                        If rs1.Fields!EVHR.Value < lngEVHlow Then Exit Do
                        If rs1.Fields!EVHR.Value > lngMaxHigh Then lngMaxHigh = rs1.Fields!EVHR.Value 'Keep track of the tallest high height
                        rs1.MoveNext()
                    Loop
                    rs1.MovePrevious()
                    lngEVChigh = rs1.Fields!EVCR.Value
                    lngEVHhigh = rs1.Fields!EVHR.Value
                    If lngEVHhigh < lngMaxHigh Then 'Check the heights to make sure EVH high is not lower than another height.
                        Do Until rs1.Fields!EVHR.Value = lngMaxHigh
                            rs1.MovePrevious()
                        Loop
                        lngEVHhigh = rs1.Fields!EVHR.Value
                        lngEVChigh = rs1.Fields!EVCR.Value
                    End If
                    rs1.MoveNext()
                End If 'No more consecutive FMs. Assign the FM rule

                'Convert to coded FBFM40 value like TL1/181 instead of just 181
                strFM40 = gf_ConvertFM40(lngFM40, strProjectPath)

                'Convert to coded CanFM value like TL1/181 instead of just 181
                strCanFM = gf_ConvertCanFM(lngCanFM, strProjectPath)

                'Get Time,date,calibration name and rule that was added to put into the notes for the new rule
                strNewRuleNote = Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                                strSessionName & ": AUTO RULE  " &
                gf_GetNum(cmbEVT, "EVT") & "_[" &
                gf_GetNum(cmbEVT, "DIST") & "]_" &
                gf_ConvertCode(lngEVClow & "", "cov", "low", strProjectPath) &
                gf_ConvertCode(lngEVChigh & "", "cov", "high", strProjectPath) & "_" &
                gf_ConvertCode(lngEVHlow & "", "hgt", "low", strProjectPath) &
                gf_ConvertCode(lngEVHhigh & "", "hgt", "high", strProjectPath) & "_" &
                "any_" &
                "any_" &
                lngFM13 & "_" &
                strFM40 & "_" &
                strCanFM & "_" &
                lngFCCS & "_" &
                lngFLM & "_" &
                "9999_" &
                "9999_" &
                "9999_" &
                "9999_" &
                "9999_" &
                "9999_" &
                "9999_" &
                "On"

                'Assign FM to View HgtRules
                strSQL = "INSERT INTO DATA_AutoRules (EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                        "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, CCover, CHeight, CBD13x100, CBD40x100, " &
                        "CBH13mx10, CBH40mx10, " &
                        "Canopy, OnOff, Notes) " &
                        "VALUES ( " & gf_GetNum(cmbEVT, "EVT") & ", " &
                        gf_GetNum(cmbEVT, "DIST") & ", " &
                        lngEVClow & ", " &
                        lngEVChigh & ", " &
                        lngEVHlow & ", " &
                        lngEVHhigh & ", '" &
                        "any', '" &
                        "any', " &
                        lngFM13 & ", '" &
                        strFM40 & "', '" &
                        strCanFM & "', " &
                        lngFCCS & ", " &
                        lngFLM & ", " &
                        "9999, " &
                        "9999, " &
                        "9999, " &
                        "9999, " &
                        "9999, " &
                        "9999, " &
                        "9999, '" &
                        "On', '" &
                        strNewRuleNote & "' )"
                dbconn.Execute(strSQL)                                                  'Run the SQL statement
            Loop

            'If Majority view exists drop it before you start
            Try
                strSQL = "DROP VIEW Majority"
                dbconn.Execute(strSQL)
            Catch ex As System.Exception
                'Do nothing
            Finally
                'Do nothing
            End Try

            'Get the auto rules from initial height compress
            strSQL = "SELECT Cover_Low, Cover_High, Height_Low, Height_High, FBFM13, FBFM40, CanFM, FCCS, FLM, Notes " &
                    "FROM DATA_AutoRules " &
                    "ORDER BY Height_Low, Height_High, Cover_Low, Cover_High"
            rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            'Reset variables
            lngEVClow = rs2.Fields!Cover_Low.Value
            lngEVChigh = rs2.Fields!Cover_High.Value
            lngEVHlow = rs2.Fields!Height_Low.Value
            lngEVHhigh = rs2.Fields!Height_High.Value
            lngFM13 = rs2.Fields!FBFM13.Value                                                   'Starting FM13
            strFM40 = rs2.Fields!FBFM40.Value                                                   'String value of FBFM40
            strCanFM = rs2.Fields!CanFM.Value                                                   'String value of CanFM
            lngFCCS = rs2.Fields!FCCS.Value                                                     'Starting FCCS
            lngFLM = rs2.Fields!FLM.Value                                                       'Starting FLM
            rs2.Delete()                                                                        'Delete this record
            rs2.MoveNext()
            If rs2.EOF = False Then 'There are more rules to evaluate
                'Sort through and find same heights and fm in multiple covers
                Do Until rs2.EOF 'Go until no more rules are left
                    If rs2.Fields!Height_Low.Value = lngEVHlow And rs2.Fields!Height_High.Value = lngEVHhigh And
                    rs2.Fields!FBFM13.Value = lngFM13 And
                    rs2.Fields!FBFM40.Value = strFM40 And rs2.Fields!CanFM.Value = strCanFM And
                    rs2.Fields!FCCS.Value = lngFCCS And rs2.Fields!FLM.Value = lngFLM Then
                        lngEVChigh = rs2.Fields!Cover_High.Value
                        rs2.Delete()
                        rs2.MoveNext()
                    Else
                        'Insert grouped rule into rulesets database
                        strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                            "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, CCover, CHeight, CBD13x100, CBD40x100, " &
                            "CBH13mx10, CBH40mx10, Canopy, OnOff, Notes) " &
                            "VALUES ( " & gf_GetNum(cmbEVT, "EVT") & ", " &
                            gf_GetNum(cmbEVT, "DIST") & ", " &
                            lngEVClow & ", " & lngEVChigh & ", " & lngEVHlow & ", " & lngEVHhigh & ", '" &
                            "any', 'any', " & lngFM13 & ", '" & strFM40 & "', '" & strCanFM & "', " &
                            lngFCCS & ", " & lngFLM & ", 9999, 9999, 9999, 9999, 9999, 9999, 9999, 'On', '" &
                            strNewRuleNote & "' )"
                        dbconn.Execute(strSQL)                                                  'Run the SQL statement

                        'Reassign the beginning rule to start again
                        lngEVClow = rs2.Fields!Cover_Low.Value
                        lngEVChigh = rs2.Fields!Cover_High.Value
                        lngEVHlow = rs2.Fields!Height_Low.Value
                        lngEVHhigh = rs2.Fields!Height_High.Value
                        lngFM13 = rs2.Fields!FBFM13.Value                                       'Starting FM13
                        strFM40 = rs2.Fields!FBFM40.Value                                       'Starting FM40
                        strCanFM = rs2.Fields!CanFM.Value                                       'String value of CanFM
                        lngFCCS = rs2.Fields!FCCS.Value                                         'Starting FCCS
                        lngFLM = rs2.Fields!FLM.Value                                           'Starting FLM
                        rs2.Delete()
                        rs2.MoveNext()
                    End If

                    If rs2.EOF = True Then  'Insert last rule into rulesets database
                        strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                            "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, CCover, CHeight, CBD13x100, CBD40x100, " &
                            "CBH13mx10, CBH40mx10, Canopy, OnOff, Notes) " &
                            "VALUES ( " & gf_GetNum(cmbEVT, "EVT") & ", " &
                            gf_GetNum(cmbEVT, "DIST") & ", " &
                            lngEVClow & ", " & lngEVChigh & ", " & lngEVHlow & ", " & lngEVHhigh & ", '" &
                            "any', 'any', " & lngFM13 & ", '" & strFM40 & "', '" & strCanFM & "', " &
                            lngFCCS & ", " & lngFLM & ", 9999, 9999, 9999, 9999, 9999, 9999, 9999, 'On', '" &
                            strNewRuleNote & "' )"
                        dbconn.Execute(strSQL)                                                  'Run the SQL statement
                    End If
                Loop
            Else
                'There is only one rule
                strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                            "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, CCover, CHeight, CBD13x100, CBD40x100, " &
                            "CBH13mx10, CBH40mx10, Canopy, OnOff, Notes) " &
                            "VALUES ( " & gf_GetNum(cmbEVT, "EVT") & ", " &
                            gf_GetNum(cmbEVT, "DIST") & ", " &
                            lngEVClow & ", " & lngEVChigh & ", " & lngEVHlow & ", " & lngEVHhigh & ", '" &
                            "any', 'any', " & lngFM13 & ", '" & strFM40 & "', '" & strCanFM & "', " &
                            lngFCCS & ", " & lngFLM & ", 9999, 9999, 9999, 9999, 9999, 9999, 9999, 'On', '" &
                            strNewRuleNote & "' )"
                dbconn.Execute(strSQL)                                                  'Run the SQL statement
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
            MsgBox("Auto Rule only works with surface fuel models." & vbCrLf &
                        "Check to make sure the wildcard value is a surface fuel model." & vbCrLf &
                        "Make a new Management Unit (MU) and include a surface fuel model GRID in the wildcard option." & vbCrLf &
                        ex.Message)
        End Try
    End Sub

    Public Function GetAutoRuleCheck() As String
        'Get the surface fuel used in the autorule
        GetAutoRuleCheck = strAutoRuleSF
    End Function

    Public Sub SetAutoRuleCheck(ByVal strSFChecked As String)
        'Set the surface fuel used in the autorule
        strAutoRuleSF = strSFChecked
    End Sub
End Class