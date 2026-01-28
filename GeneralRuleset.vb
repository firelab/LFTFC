Imports System.Data

Module GeneralRuleset
    Public m_ColCW As Collection                                            'Stores col widths for list rulesets
    Private strSQL As String                                                'SQL variable for this module
    Private ruleSort As String = "Sort by Cover"                            'Stores the rule sorting option

    Public Sub gr_MakeRuleset(ByVal EvtNum As String, ByVal DistNum As String,
                              ByVal ComboTable As String, ByVal RulesTable As String,
                             ByRef RulesetCollection As Collection, ByRef EVTPixelCountCollection As Collection,
                             ByVal ProjPath As String)
        'EVTNum - 4 digit number, DistNum - 3 digit number, cmsSort - (Default, Sort Low to High, Sort High to Low)

        'Declare variables
        RulesetCollection = New Collection
        Dim ColC As New Collection
        Dim ColE As New Collection
        Dim ColW As New Collection
        Dim ColEW As New Collection
        Dim thing, thingC, thingE, thingW, thingEW As clsRule
        Dim strPixelCount As String
        Dim NewRule As clsRule 'Creates new object of type rule
        Dim debugi As Integer = 1  'Used for debug

        Dim rs1 As New ADODB.Recordset                                                  'recordset for data
        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            'Get Rules
            If ruleSort = "Sort by Height" Then 'Sort using Height_Low then Cover_Low
                strSQL = "SELECT Id, EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " &
                                 "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                                 "CBH13mx10, CBH40mx10, OnOff, Notes, PixelCount " &
                                 "FROM(" & RulesTable & ") " &
                                 "WHERE(((" & RulesTable & ".[EVT]) = " & EvtNum & ") " &
                                 "And ((" & RulesTable & ".DIST) = " & DistNum & ")) " &
                                 "ORDER BY " & RulesTable & ".OnOff DESC, " & RulesTable & ".BPSRF DESC, " &
                                 RulesTable & ".Wildcard DESC, " & RulesTable & ".Height_Low, " & RulesTable & ".Cover_Low"
            Else                                'Sort using default Cover_Low then Height_Low
                strSQL = "SELECT Id, EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " &
                                 "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                                 "CBH13mx10, CBH40mx10, OnOff, Notes, PixelCount " &
                                 "FROM(" & RulesTable & ") " &
                                 "WHERE(((" & RulesTable & ".[EVT]) = " & EvtNum & ") " &
                                 "And ((" & RulesTable & ".DIST) = " & DistNum & ")) " &
                                 "ORDER BY " & RulesTable & ".OnOff DESC, " & RulesTable & ".BPSRF DESC, " &
                                 RulesTable & ".Wildcard DESC, " & RulesTable & ".Cover_Low, " & RulesTable & ".Height_Low"
            End If

            strPixelCount = "yes" 'Set strPixelCount to start with yes

            'Set collection of rules
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            If rs1.EOF = False Then
                Do Until rs1.EOF
                    'Checks to see if the pixel count has been calculated and the rule is "On"
                    If rs1.Fields!PixelCount.Value & "" = "" And rs1.Fields!OnOff.Value & "" = "On" Then
                        strPixelCount = "no"
                    End If

                    'Adds a new Rule
                    NewRule = New clsRule(rs1.Fields!Id.Value & "",
                                        rs1.Fields!EVT.Value & "",
                                        rs1.Fields!DIST.Value & "",
                                        rs1.Fields!Cover_Low.Value & "",
                                        rs1.Fields!Cover_High.Value & "",
                                        rs1.Fields!Height_Low.Value & "",
                                        rs1.Fields!Height_High.Value & "",
                                        rs1.Fields!BPSRF.Value & "",
                                        rs1.Fields!Wildcard.Value & "",
                                        rs1.Fields!FBFM13.Value & "",
                                        rs1.Fields!FBFM40.Value & "",
                                        rs1.Fields!CanFM.Value & "",
                                        rs1.Fields!FCCS.Value & "",
                                        rs1.Fields!FLM.Value & "",
                                        rs1.Fields!Canopy.Value & "",
                                        rs1.Fields!CCover.Value & "",
                                        rs1.Fields!CHeight.Value & "",
                                        rs1.Fields!CBD13x100.Value & "",
                                        rs1.Fields!CBD40x100.Value & "",
                                        rs1.Fields!CBH13mx10.Value & "",
                                        rs1.Fields!CBH40mx10.Value & "",
                                        rs1.Fields!OnOff.Value & "",
                                        rs1.Fields!Notes.Value & "",
                                        rs1.Fields!PixelCount.Value & "",
                                        ComboTable, RulesTable, EVTPixelCountCollection, ProjPath)
                    RulesetCollection.Add(NewRule)

                    rs1.MoveNext()

                Loop
            Else 'When no rules have been created for an EVT
                'Adds a new Rule
                NewRule = New clsRule("0",
                                    EvtNum & "", DistNum & "",
                                    "9999", "9999", "9999", "9999", "any", "any", "9999", "9999", "9999", "9999",
                                    "9999", "9999", "9999", "9999", "9999", "9999", "9999", "9999", "Off",
                                    "No rule", "9999", ComboTable, RulesTable, EVTPixelCountCollection, ProjPath)
                RulesetCollection.Add(NewRule)
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            NewRule = Nothing 'Clean up the old object

            If strPixelCount = "no" Then 'If "no" then check for rule overlap
                'Check all rules and correct rule overlap if pixel count has not been calculated on one of the rules
                For Each thing In RulesetCollection 'Sort types of rules into different collections
                    If thing.OnOff = "On" Then
                        If thing.BPS <> "any" And thing.Wildcard <> "any" Then
                            ColEW.Add(thing)
                        ElseIf thing.BPS <> "any" Then
                            ColE.Add(thing)
                        ElseIf thing.Wildcard <> "any" Then
                            ColW.Add(thing)
                        Else
                            ColC.Add(thing)
                        End If
                    End If
                Next thing

                'If collection is empty then add a null string
                thing = New clsRule("", EvtNum, DistNum, "100", "100", "100", "100", "any", "any", "9999", "9999", "9999", "9999", "9999",
                                    "9999", "9999", "9999", "9999", "9999", "9999", "9999", "9999", "", "0", ComboTable, RulesTable,
                                    EVTPixelCountCollection, ProjPath)
                ColEW.Add(thing)
                ColW.Add(thing)
                ColE.Add(thing)

                For Each thingC In ColC
                    For Each thingE In ColE
                        For Each thingW In ColW
                            For Each thingEW In ColEW
                                If (thingE.Id <> "" And thingW.Id <> "") Or (thingE.Id <> "" And thingEW.Id <> "") _
                                    Or (thingW.Id <> "" And thingEW.Id <> "") Then
                                    thingC.PixelCount = thingC.PixelCount + RuleOverLap(thingC, thingE, thingW, thingEW,
                                                                                        EvtNum, DistNum, ComboTable, RulesTable,
                                                                                        ProjPath)
                                Else
                                    thingC.PixelCount = thingC.PixelCount - RuleOverLap(thingC, thingE, thingW, thingEW,
                                                                                        EvtNum, DistNum, ComboTable, RulesTable,
                                                                                        ProjPath)
                                End If
                            Next thingEW
                        Next thingW
                    Next thingE
                    thingC.CalcAcresAndPercent(EVTPixelCountCollection) 'Calc new percent and acres
                Next thingC
                thingC = thing 'Set thingC equal to a new rule so it does not have any interaction
                ColE.Remove(ColE.Count) 'Remove the last value that is set to a new unpopulated rule
                For Each thingE In ColE
                    For Each thingW In ColW
                        For Each thingEW In ColEW
                            If thingW.Id <> "" And thingEW.Id <> "" Then
                                thingE.PixelCount = thingE.PixelCount + RuleOverLap(thingC, thingE, thingW, thingEW,
                                                                                    EvtNum, DistNum, ComboTable, RulesTable,
                                                                                    ProjPath)
                            Else
                                thingE.PixelCount = thingE.PixelCount - RuleOverLap(thingC, thingE, thingW, thingEW,
                                                                                    EvtNum, DistNum, ComboTable, RulesTable,
                                                                                    ProjPath)
                            End If
                        Next thingEW
                    Next thingW
                    thingE.CalcAcresAndPercent(EVTPixelCountCollection) 'Calc new percent and acres
                Next thingE
                thingE = thing 'Set thingE equal to a new rule so it does not have any interaction
                ColW.Remove(ColW.Count) 'Remove the last value that is set to a new unpopulated rule
                For Each thingW In ColW
                    For Each thingEW In ColEW
                        thingW.PixelCount = thingW.PixelCount - RuleOverLap(thingC, thingE, thingW, thingEW,
                                                                            EvtNum, DistNum, ComboTable, RulesTable,
                                                                            ProjPath)
                    Next thingEW
                    thingW.CalcAcresAndPercent(EVTPixelCountCollection) 'Calc new percent and acres
                Next thingW
            End If

        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in gr_MakeRuleset - " & ex.Message)
        End Try

        thing = Nothing
        thingC = Nothing
        thingE = Nothing
        thingW = Nothing
        thingEW = Nothing
        ColC = Nothing
        ColE = Nothing
        ColW = Nothing
        ColEW = Nothing
    End Sub

    Public WriteOnly Property gr_SetRuleSort() As String
        Set(ByVal value As String)
            ruleSort = value
        End Set
    End Property

    'Checks the rule For overlaping pixel counts
    Private Function RuleOverLap(ByVal varC As Object, ByVal varE As Object, ByVal varW As Object,
                                 ByVal varEW As Object, ByVal EVTNum As String, ByVal DistNum As String, ByVal ComboTable As String,
                                 ByVal RulesTable As String, ByVal ProjPath As String) As Long

        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            If varC.Id <> "" And varE.Id <> "" And varW.Id <> "" And varEW.Id <> "" Then
                If varE.BPS <> varEW.BPS And varW.Wildcard <> varEW.Wildcard Then
                    strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                             "FROM " & ComboTable &
                             " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                             "And " & CreateSQL(varEW) & " And " & CreateSQL(varW) & " And " &
                             CreateSQL(varE) & " And " &
                             CreateSQL(varC) & ") " &
                             "GROUP BY EVTR, DIST"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    'Checks to see if Pixel count is not null and not EOF then we have a difference
                    If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                        RuleOverLap = rs1.Fields!SumOfCount.Value
                    End If
                Else
                    RuleOverLap = 0
                End If
            ElseIf varC.Id <> "" And varE.Id = "" And varW.Id <> "" And varEW.Id <> "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varEW) & " And " & CreateSQL(varW) & " And " & CreateSQL(varC) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            ElseIf varC.Id <> "" And varE.Id <> "" And varW.Id = "" And varEW.Id <> "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varEW) & " And " & CreateSQL(varE) & " And " & CreateSQL(varC) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            ElseIf varC.Id <> "" And varE.Id <> "" And varW.Id <> "" And varEW.Id = "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varW) & " And " & CreateSQL(varE) & " And " & CreateSQL(varC) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            ElseIf varC.Id <> "" And varE.Id <> "" And varW.Id = "" And varEW.Id = "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varE) & " And " & CreateSQL(varC) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            ElseIf varC.Id <> "" And varE.Id = "" And varW.Id <> "" And varEW.Id = "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varW) & " And " & CreateSQL(varC) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            ElseIf varC.Id <> "" And varE.Id = "" And varW.Id = "" And varEW.Id <> "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varEW) & " And " & CreateSQL(varC) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            ElseIf varC.Id = "" And varE.Id <> "" And varW.Id <> "" And varEW.Id <> "" Then
                If varE.BPS <> varEW.BPS And varW.Wildcard <> varEW.Wildcard Then
                    strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varEW) & " And " & CreateSQL(varW) & " And " & CreateSQL(varE) &
                         ") GROUP BY EVTR, DIST"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    'Checks to see if Pixel count is not null and not EOF then we have a difference
                    If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                        RuleOverLap = rs1.Fields!SumOfCount.Value
                    End If
                Else
                    RuleOverLap = 0
                End If
            ElseIf varC.Id = "" And varE.Id <> "" And varW.Id = "" And varEW.Id <> "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varEW) & " And " & CreateSQL(varE) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            ElseIf varC.Id = "" And varE.Id <> "" And varW.Id <> "" And varEW.Id = "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varW) & " And " & CreateSQL(varE) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            ElseIf varC.Id = "" And varE.Id = "" And varW.Id <> "" And varEW.Id <> "" Then
                strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount " &
                         "FROM " & ComboTable &
                         " WHERE (((EVTR)=" & EVTNum & ") AND ((DIST)=" & DistNum & ") " &
                         "And " & CreateSQL(varEW) & " And " & CreateSQL(varW) &
                         ") GROUP BY EVTR, DIST"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is not null and not EOF then we have a difference
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    RuleOverLap = rs1.Fields!SumOfCount.Value
                End If
            Else
                RuleOverLap = 0
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in RuleOverLap - " & ex.Message)
        End Try
    End Function

    Private Function CreateSQL(ByVal varRule As Object) As String
        If varRule.Id = "" Then
            CreateSQL = ""
        ElseIf varRule.BPS <> "any" And varRule.Wildcard <> "any" Then
            CreateSQL = "EVCR Between " & varRule.IntCovLow & " And " & varRule.IntCovHigh & " And " &
                        "EVHR Between " & varRule.IntHgtLow & " And " & varRule.IntHgtHigh & " And " &
                        "BPSRF = " & varRule.BPS & " And Wildcard = '" & varRule.Wildcard & "'"
        ElseIf varRule.BPS <> "any" And varRule.Wildcard = "any" Then
            CreateSQL = "EVCR Between " & varRule.IntCovLow & " And " & varRule.IntCovHigh & " And " &
                        "EVHR Between " & varRule.IntHgtLow & " And " & varRule.IntHgtHigh & " And " &
                        "BPSRF = " & varRule.BPS & " And '" & varRule.Wildcard & "' = 'any'"
        ElseIf varRule.BPS = "any" And varRule.Wildcard <> "any" Then
            CreateSQL = "EVCR Between " & varRule.IntCovLow & " And " & varRule.IntCovHigh & " And " &
                        "EVHR Between " & varRule.IntHgtLow & " And " & varRule.IntHgtHigh & " And '" &
                        varRule.BPS & "' = 'any' And Wildcard = '" & varRule.Wildcard & "'"
        ElseIf varRule.BPS = "any" And varRule.Wildcard = "any" Then
            CreateSQL = "EVCR Between " & varRule.IntCovLow & " And " & varRule.IntCovHigh & " And " &
                        "EVHR Between " & varRule.IntHgtLow & " And " & varRule.IntHgtHigh & " And '" &
                        varRule.BPS & "' = 'any' And '" & varRule.Wildcard & "' = 'any'"
        Else
            CreateSQL = ""
        End If
    End Function

    Public Sub gr_ClearPAP(ByRef RulesetCollection As Collection) 'Clears the pixel count,acres, and percent evt
        Dim i As Long 'Used for a counter
        For i = 1 To RulesetCollection.Count 'Clear the pixel count and acres for the ruleset
            RulesetCollection.Item(i).PixelCount = ""
            RulesetCollection.Item(i).Acres = ""
            RulesetCollection.Item(i).EvtPer = ""
        Next i
    End Sub
End Module
