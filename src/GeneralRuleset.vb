Imports System.Data
Imports System.Data.SQLite
Imports System.IO

Module GeneralRuleset
    Public m_ColCW As Collection                                            'Stores col widths for list rulesets
    Private strSQL As String                                                'SQL variable for this module
    Private ruleSort As String = "Sort by Cover"                            'Stores the rule sorting option


    Public Sub RunNonQuery(conn As SQLiteConnection, sql As String)
        Using cmd As New SQLiteCommand(sql, conn)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

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


        ' Build the SQLite connection string
        Dim dbPath As String = Path.Combine(gs_ProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Using dbconn As New SQLiteConnection(connString)
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
                Using cmd As New SQLiteCommand(strSQL, dbconn)

                    Using reader As SQLiteDataReader = cmd.ExecuteReader()

                        If reader.HasRows Then

                            While reader.Read()

                                ' Pixel count check
                                Dim pixelCountVal = reader("PixelCount").ToString()
                                Dim onOffVal = reader("OnOff").ToString()

                                If pixelCountVal = "" AndAlso onOffVal = "On" Then
                                    strPixelCount = "no"
                                End If


                                'Adds a new Rule
                                NewRule = New clsRule(
                                    reader("Id").ToString(),
                                    reader("EVT").ToString(),
                                    reader("DIST").ToString(),
                                    reader("Cover_Low").ToString(),
                                    reader("Cover_High").ToString(),
                                    reader("Height_Low").ToString(),
                                    reader("Height_High").ToString(),
                                    reader("BPSRF").ToString(),
                                    reader("Wildcard").ToString(),
                                    reader("FBFM13").ToString(),
                                    reader("FBFM40").ToString(),
                                    reader("CanFM").ToString(),
                                    reader("FCCS").ToString(),
                                    reader("FLM").ToString(),
                                    reader("Canopy").ToString(),
                                    reader("CCover").ToString(),
                                    reader("CHeight").ToString(),
                                    reader("CBD13x100").ToString(),
                                    reader("CBD40x100").ToString(),
                                    reader("CBH13mx10").ToString(),
                                    reader("CBH40mx10").ToString(),
                                    onOffVal,
                                    reader("Notes").ToString(),
                                    pixelCountVal,
                                    ComboTable, RulesTable, EVTPixelCountCollection, ProjPath)
                                RulesetCollection.Add(NewRule)
                            End While

                        Else  'When no rules have been created for an EVT 
                            'Adds a new Rule
                            NewRule = New clsRule("0",
                                    EvtNum & "", DistNum & "",
                                    "9999", "9999", "9999", "9999", "any", "any", "9999", "9999", "9999", "9999",
                                    "9999", "9999", "9999", "9999", "9999", "9999", "9999", "9999", "Off",
                                    "No rule", "9999", ComboTable, RulesTable, EVTPixelCountCollection, ProjPath)
                            RulesetCollection.Add(NewRule)
                        End If
                    End Using
                End Using

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

                MsgBox("Error in gr_MakeRuleset - " & ex.Message)
            End Try

        End Using

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
    Private Function RuleOverLap(varC As clsRule,
                             varE As clsRule,
                             varW As clsRule,
                             varEW As clsRule,
                             EVTNum As String,
                             DistNum As String,
                             ComboTable As String,
                             RulesTable As String,
                             ProjPath As String) As Long

        Dim dbPath As String = Path.Combine(ProjPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"


        '---------------------------------------------------------------
        ' Build list of rule filters based on non-empty Id values
        '---------------------------------------------------------------
        Dim filters As New List(Of String)

        If varC.Id <> "" Then filters.Add(CreateSQL(varC))
        If varE.Id <> "" Then filters.Add(CreateSQL(varE))
        If varW.Id <> "" Then filters.Add(CreateSQL(varW))
        If varEW.Id <> "" Then filters.Add(CreateSQL(varEW))

        '---------------------------------------------------------------
        ' An overlap needs at least TWO populated rules. The original
        ' Access version had no case for a single populated rule, so it
        ' fell through to "RuleOverLap = 0". Returning the lone rule's own
        ' count here makes the caller compute PixelCount - PixelCount = 0,
        ' which wipes the count of every plain (any/any) rule.
        '---------------------------------------------------------------
        If filters.Count < 2 Then Return 0

        '---------------------------------------------------------------
        ' Special logic applied when E, W and EW are all populated, with
        ' or without C. The original guarded both of those cases; keying
        ' it off all four being populated skipped the C-empty case.
        '---------------------------------------------------------------
        If varE.Id <> "" AndAlso varW.Id <> "" AndAlso varEW.Id <> "" Then
            If (varE.BPS <> varEW.BPS) AndAlso (varW.Wildcard <> varEW.Wildcard) Then
                'allowed
            Else
                Return 0        'logic says no pixel overlap case applies
            End If
        End If

        '---------------------------------------------------------------
        ' Build WHERE SQL
        '---------------------------------------------------------------
        Dim whereClause As String =
        "EVTR=" & EVTNum & " AND DIST=" & DistNum

        For Each f In filters
            whereClause &= " AND " & f
        Next

        Dim sql As String =
        "SELECT SUM(COUNT) AS SumOfCount " &
        "FROM " & ComboTable & " " &
        "WHERE " & whereClause


        '---------------------------------------------------------------
        ' Execute SQLite query
        '---------------------------------------------------------------
        Try
            Using conn As New SQLiteConnection(connString)
                conn.Open()

                Using cmd As New SQLiteCommand(sql, conn)
                    Using rd As SQLiteDataReader = cmd.ExecuteReader()

                        If rd.Read() AndAlso Not rd.IsDBNull(0) Then
                            Return CLng(rd.GetValue(0))
                        End If

                    End Using
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Error in RuleOverLap - " & ex.Message)
        End Try

        Return 0
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
