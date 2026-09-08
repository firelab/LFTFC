Imports System.Data
Imports System.Data.SQLite
Imports System.IO

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

        'Chosen FM wildcard assignment (FM13/FM40/CanFM/FCCS/FLM)
        Dim selectedFM As String = strAutoRuleSF

        'Determine which radio button should be default-selected
        rdoFM13.Checked = (selectedFM = rdoFM13.Name)
        rdoFM40.Checked = (selectedFM = rdoFM40.Name)
        rdoCanFM.Checked = (selectedFM = rdoCanFM.Name)
        rdoFCCS.Checked = (selectedFM = rdoFCCS.Name)
        rdoFLM.Checked = (selectedFM <> rdoFM13.Name AndAlso selectedFM <> rdoFM40.Name AndAlso
                      selectedFM <> rdoCanFM.Name AndAlso selectedFM <> rdoFCCS.Name)

        Dim EVT As Integer = gf_GetNum(cmbEVT, "EVT")
        Dim DIST As Integer = gf_GetNum(cmbEVT, "DIST")

        Dim dbPath As String = Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Try

            Using conn As New SQLiteConnection(connString)
                conn.Open()

                ' STEP 1: Fetch majority fuel-condition rows (sorted)
                Dim sqlMajority As String =
                "SELECT EVCR, EVHR, COUNT, WILDCARD " &
                "FROM " & comboR & " " &
                "WHERE COUNT > 0 AND EVTR = @evt AND DIST = @dist " &
                "ORDER BY EVCR, EVHR"

                Dim majorityList As New List(Of MajorityRow)

                Using cmd As New SQLiteCommand(sqlMajority, conn)
                    cmd.Parameters.AddWithValue("@evt", EVT)
                    cmd.Parameters.AddWithValue("@dist", DIST)

                    Using rd As SQLiteDataReader = cmd.ExecuteReader()
                        While rd.Read()
                            majorityList.Add(New MajorityRow With {
                            .EVCR = CInt(rd("EVCR")),
                            .EVHR = CInt(rd("EVHR")),
                            .Wildcard = CInt(rd("WILDCARD"))
                        })
                        End While
                    End Using
                End Using

                If majorityList.Count = 0 Then
                    MsgBox("No majority data found.")
                    Exit Sub
                End If


                ' STEP 2: Build initial non-compressed AutoRule records into DATA_AutoRules
                Dim autoRules As New List(Of AutoRuleRow)

                Dim i As Integer = 0

                While i < majorityList.Count

                    Dim startCR As Integer = majorityList(i).EVCR
                    Dim startHR As Integer = majorityList(i).EVHR
                    Dim wld As Integer = majorityList(i).Wildcard

                    Dim endCR As Integer = startCR
                    Dim endHR As Integer = startHR
                    Dim maxHR As Integer = startHR

                    'Assign FM based on selected radio button
                    Dim fm13 As Integer = DEFAULT_FUEL_VAL
                    Dim fm40 As Integer = DEFAULT_FUEL_VAL
                    Dim canFM As Integer = DEFAULT_FUEL_VAL
                    Dim fccs As Integer = DEFAULT_FUEL_VAL
                    Dim flm As Integer = DEFAULT_FUEL_VAL

                    If rdoFM13.Checked Then fm13 = wld
                    If rdoFM40.Checked Then fm40 = wld
                    If rdoCanFM.Checked Then canFM = wld
                    If rdoFCCS.Checked Then fccs = wld
                    If rdoFLM.Checked Then flm = wld

                    Dim j As Integer = i + 1

                    'Group consecutive same-wildcard rows
                    While j < majorityList.Count AndAlso
                      majorityList(j).Wildcard = wld AndAlso
                      majorityList(j).EVHR >= startHR

                        If majorityList(j).EVHR > maxHR Then maxHR = majorityList(j).EVHR

                        j += 1
                    End While

                    'Step backward if needed to max height
                    j -= 1

                    endCR = majorityList(j).EVCR
                    endHR = majorityList(j).EVHR

                    If endHR < maxHR Then
                        'Search backward to find the record with maxHR
                        Dim k As Integer = j
                        While k >= i AndAlso majorityList(k).EVHR <> maxHR
                            k -= 1
                        End While
                        If k >= i Then
                            endHR = majorityList(k).EVHR
                            endCR = majorityList(k).EVCR
                        End If
                    End If

                    'Create AutoRuleRow
                    autoRules.Add(New AutoRuleRow With {
                    .CoverLow = startCR,
                    .CoverHigh = endCR,
                    .HeightLow = startHR,
                    .HeightHigh = endHR,
                    .FBFM13 = fm13,
                    .FBFM40 = fm40,
                    .canFM = canFM,
                    .fccs = fccs,
                    .flm = flm
                })

                    i = j + 1
                End While


                ' STEP 3: Write initial auto rules into DATA_AutoRules
                Dim sqlInsertAuto As String =
                "INSERT INTO DATA_AutoRules (EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, CCover, CHeight, CBD13x100, CBD40x100, " &
                "CBH13mx10, CBH40mx10, Canopy, OnOff, Notes) " &
                "VALUES (@evt, @dist, @clow, @chigh, @hlow, @hhigh, 'any', 'any', @fm13, @fm40, @canfm, @fccs, @flm, " &
                "@ccov, @cheight, @cbd13, @cbd40, @cbh13, @cbh40, @canopy, 'On', @notes)"

                Dim nowNote As String =
                $"{Now.ToShortTimeString} {Now.ToShortDateString} {strSessionName}: AUTO RULE {EVT}_[{DIST}]"

                For Each ar In autoRules
                    Using cmdInsert As New SQLiteCommand(sqlInsertAuto, conn)
                        cmdInsert.Parameters.AddWithValue("@evt", EVT)
                        cmdInsert.Parameters.AddWithValue("@dist", DIST)
                        cmdInsert.Parameters.AddWithValue("@clow", ar.CoverLow)
                        cmdInsert.Parameters.AddWithValue("@chigh", ar.CoverHigh)
                        cmdInsert.Parameters.AddWithValue("@hlow", ar.HeightLow)
                        cmdInsert.Parameters.AddWithValue("@hhigh", ar.HeightHigh)
                        cmdInsert.Parameters.AddWithValue("@fm13", ar.FBFM13)
                        cmdInsert.Parameters.AddWithValue("@fm40", gf_ConvertFM40(ar.FBFM40, strProjectPath))
                        cmdInsert.Parameters.AddWithValue("@canfm", gf_ConvertCanFM(ar.CanFM, strProjectPath))
                        cmdInsert.Parameters.AddWithValue("@fccs", ar.FCCS)
                        cmdInsert.Parameters.AddWithValue("@flm", ar.FLM)
                        cmdInsert.Parameters.AddWithValue("@ccov", DEFAULT_FUEL_VAL)
                        cmdInsert.Parameters.AddWithValue("@cheight", DEFAULT_FUEL_VAL)
                        cmdInsert.Parameters.AddWithValue("@cbd13", DEFAULT_FUEL_VAL)
                        cmdInsert.Parameters.AddWithValue("@cbd40", DEFAULT_FUEL_VAL)
                        cmdInsert.Parameters.AddWithValue("@cbh13", DEFAULT_FUEL_VAL)
                        cmdInsert.Parameters.AddWithValue("@cbh40", DEFAULT_FUEL_VAL)
                        cmdInsert.Parameters.AddWithValue("@canopy", DEFAULT_FUEL_VAL)
                        cmdInsert.Parameters.AddWithValue("@notes", nowNote)
                        cmdInsert.ExecuteNonQuery()
                    End Using
                Next


                ' STEP 4: Read back DATA_AutoRules for compression
                Dim compressRules As New List(Of AutoRuleRow)

                Dim sqlFetchAuto As String =
                "SELECT Cover_Low, Cover_High, Height_Low, Height_High, FBFM13, FBFM40, CanFM, FCCS, FLM, Notes " &
                "FROM DATA_AutoRules " &
                "ORDER BY Height_Low, Height_High, Cover_Low, Cover_High"

                Using cmdFetch As New SQLiteCommand(sqlFetchAuto, conn)
                    Using rd As SQLiteDataReader = cmdFetch.ExecuteReader()
                        While rd.Read()
                            compressRules.Add(New AutoRuleRow With {
                            .CoverLow = CInt(rd("Cover_Low")),
                            .CoverHigh = CInt(rd("Cover_High")),
                            .HeightLow = CInt(rd("Height_Low")),
                            .HeightHigh = CInt(rd("Height_High")),
                            .FBFM13 = CInt(rd("FBFM13")),
                            .FBFM40 = CInt(rd("FBFM40")),
                            .CanFM = CInt(rd("CanFM")),
                            .FCCS = CInt(rd("FCCS")),
                            .FLM = CInt(rd("FLM")),
                            .Notes = rd("Notes").ToString()
                        })
                        End While
                    End Using
                End Using


                ' STEP 5: Compress adjacent rules with identical FM + Height ranges
                Dim finalRules As New List(Of AutoRuleRow)

                Dim a As Integer = 0
                While a < compressRules.Count

                    Dim cur As AutoRuleRow = compressRules(a)
                    Dim curLowCR As Integer = cur.CoverLow
                    Dim curHighCR As Integer = cur.CoverHigh

                    Dim aFM13 = cur.FBFM13
                    Dim aFM40 = cur.FBFM40
                    Dim aCanFM = cur.CanFM
                    Dim aFCCS = cur.FCCS
                    Dim aFLM = cur.FLM

                    Dim b As Integer = a + 1

                    While b < compressRules.Count AndAlso
                      compressRules(b).HeightLow = cur.HeightLow AndAlso
                      compressRules(b).HeightHigh = cur.HeightHigh AndAlso
                      compressRules(b).FBFM13 = aFM13 AndAlso
                      compressRules(b).FBFM40 = aFM40 AndAlso
                      compressRules(b).CanFM = aCanFM AndAlso
                      compressRules(b).FCCS = aFCCS AndAlso
                      compressRules(b).FLM = aFLM

                        curHighCR = compressRules(b).CoverHigh
                        b += 1
                    End While

                    cur.CoverLow = curLowCR
                    cur.CoverHigh = curHighCR

                    finalRules.Add(cur)

                    a = b
                End While


                ' STEP 6: Insert final rules into rulesR
                Dim sqlInsertFinal As String =
                "INSERT INTO " & rulesR & " (EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, CCover, CHeight, CBD13x100, CBD40x100, " &
                "CBH13mx10, CBH40mx10, Canopy, OnOff, Notes) " &
                "VALUES (@evt, @dist, @clow, @chigh, @hlow, @hhigh, 'any', 'any', @fm13, @fm40, @canfm, @fccs, @flm, " &
                "@ccov, @cheight, @cbd13, @cbd40, @cbh13, @cbh40, @canopy, 'On', @notes)"

                For Each fr In finalRules
                    Using cmdFinal As New SQLiteCommand(sqlInsertFinal, conn)
                        cmdFinal.Parameters.AddWithValue("@evt", EVT)
                        cmdFinal.Parameters.AddWithValue("@dist", DIST)
                        cmdFinal.Parameters.AddWithValue("@clow", fr.CoverLow)
                        cmdFinal.Parameters.AddWithValue("@chigh", fr.CoverHigh)
                        cmdFinal.Parameters.AddWithValue("@hlow", fr.HeightLow)
                        cmdFinal.Parameters.AddWithValue("@hhigh", fr.HeightHigh)
                        cmdFinal.Parameters.AddWithValue("@fm13", fr.FBFM13)
                        cmdFinal.Parameters.AddWithValue("@fm40", gf_ConvertFM40(fr.FBFM40, strProjectPath))
                        cmdFinal.Parameters.AddWithValue("@canfm", gf_ConvertCanFM(fr.CanFM, strProjectPath))
                        cmdFinal.Parameters.AddWithValue("@fccs", fr.FCCS)
                        cmdFinal.Parameters.AddWithValue("@flm", fr.FLM)
                        cmdFinal.Parameters.AddWithValue("@ccov", DEFAULT_FUEL_VAL)
                        cmdFinal.Parameters.AddWithValue("@cheight", DEFAULT_FUEL_VAL)
                        cmdFinal.Parameters.AddWithValue("@cbd13", DEFAULT_FUEL_VAL)
                        cmdFinal.Parameters.AddWithValue("@cbd40", DEFAULT_FUEL_VAL)
                        cmdFinal.Parameters.AddWithValue("@cbh13", DEFAULT_FUEL_VAL)
                        cmdFinal.Parameters.AddWithValue("@cbh40", DEFAULT_FUEL_VAL)
                        cmdFinal.Parameters.AddWithValue("@canopy", DEFAULT_FUEL_VAL)
                        cmdFinal.Parameters.AddWithValue("@notes", fr.Notes)
                        cmdFinal.ExecuteNonQuery()
                    End Using
                Next

            End Using

        Catch ex As Exception
            MsgBox("Auto Rule only works with surface fuel models." &
               vbCrLf & "Check the wildcard value." &
               vbCrLf & ex.Message)
        End Try

    End Sub

    Private Class MajorityRow
        Public EVCR As Integer
        Public EVHR As Integer
        Public Wildcard As Integer
    End Class

    Private Class AutoRuleRow
        Public CoverLow As Integer
        Public CoverHigh As Integer
        Public HeightLow As Integer
        Public HeightHigh As Integer
        Public FBFM13 As Integer
        Public FBFM40 As Integer
        Public CanFM As Integer
        Public FCCS As Integer
        Public FLM As Integer
        Public Notes As String
    End Class

    Public Function GetAutoRuleCheck() As String
        'Get the surface fuel used in the autorule
        GetAutoRuleCheck = strAutoRuleSF
    End Function

    Public Sub SetAutoRuleCheck(ByVal strSFChecked As String)
        'Set the surface fuel used in the autorule
        strAutoRuleSF = strSFChecked
    End Sub
End Class