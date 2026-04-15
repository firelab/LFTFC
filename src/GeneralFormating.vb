Imports System.Data
Imports System.Windows.Forms
Imports ArcGIS.Desktop.Core
Imports ArcGIS.Desktop.Core.Geoprocessing
Imports ArcGIS.Desktop.Framework.Threading.Tasks

Module GeneralFormating
    Private strSQL As String 'SQL variable for this module

    Public Sub gf_PopBPS(ByVal cmbBox As ComboBox, ByVal EVTNum As String, ByVal DISTNum As String,
                        ByVal ComboTable As String, ByVal ProjPath As String)
        Dim strSQL As String 'SQL statement
        'Populate the BPSGraph box with the new BPSs for the selected EVT
        strSQL = "SELECT " & ComboTable & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                 "FROM " & ComboTable & " " &
                 "LEFT JOIN LUT_BPS ON " & ComboTable & ".BPSRF = LUT_BPS.BPS " &
                 "WHERE (EVTR = " & EVTNum & " And DIST = " & DISTNum & ")" &
                 " GROUP BY " & ComboTable & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                 " ORDER BY BPSRF"

        cmbBox.Items.Clear()
        cmbBox.Items.Add("any")
        gf_SetControl(cmbBox, strSQL, ProjPath)
        cmbBox.SelectedIndex = 0
    End Sub

    Public Function gf_GetNum(ByVal value As String, ByVal numType As String) As String
        Dim record As Boolean = False

        gf_GetNum = ""
        Try
            For Each i In value
                If numType = "DIST" Then
                    If i = "[" Then
                        record = True
                    ElseIf record = True And IsNumeric(i) Then
                        gf_GetNum = gf_GetNum & i
                    ElseIf record = True And i = "]" Then
                        Exit For
                    End If
                ElseIf numType = "EVT" Then
                    If IsNumeric(i) Then
                        gf_GetNum = gf_GetNum & i
                    ElseIf i = "[" Then
                        Exit For
                    End If
                Else
                    If IsNumeric(i) Then
                        gf_GetNum = gf_GetNum & i
                    Else
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            Debug.Write("Error in gf_GetNum: " & ex.Message)
        End Try
        If gf_GetNum = "" Then gf_GetNum = value 'If nothing is assigned to gf_GetNum then assign it the value
    End Function

    Public Sub gf_SetControl(ByVal pControl As ComboBox, ByVal strSQLstatement As String, ByVal ProjPath As String,
                            Optional ByVal rdoNameChecked As Boolean = True) 'Adds values to controls
        'The RadioButton is for EVT Name of Number for ordering cmbEVT

        Dim strNum As String = ""                                                       'Stores first number for FM40
        Dim strCode As String = ""                                                      'Stores second number for FM40
        Dim rs1 As New ADODB.Recordset                                                  'recordset for data

        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            If ProjPath <> "" Then
                'Assign Values
                strSQL = strSQLstatement

                'Add values to specified control
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)


                If rs1.Fields.Count = 1 Then
                    If IsNothing(rs1.Fields(0).Value) = False Then
                        Do Until rs1.EOF
                            pControl.Items.Add(rs1.Fields(0).Value)                     'Populate with 1 field.
                            rs1.MoveNext()
                        Loop
                    Else
                        pControl.Items.Add("N/A")
                    End If
                ElseIf rs1.Fields.Count = 2 Then
                    Do Until rs1.EOF
                        pControl.Items.Add(rs1.Fields(0).Value & "   " & rs1.Fields(1).Value)
                        rs1.MoveNext()
                    Loop
                ElseIf rs1.Fields.Count = 3 And pControl.Name = "cmbFCCS" Then
                    Do Until rs1.EOF
                        pControl.Items.Add(rs1.Fields(0).Value & " (" & rs1.Fields(1).Value & ")   " & rs1.Fields(2).Value)
                        rs1.MoveNext()
                    Loop
                ElseIf rs1.Fields.Count = 3 And pControl.Name = "cmbEVT" Or pControl.Name = "cmbCopyEVT" Then
                    Do Until rs1.EOF
                        If rdoNameChecked = True Then
                            pControl.Items.Add(rs1.Fields(2).Value &
                                           " " & rs1.Fields(0).Value & "[" & rs1.Fields(1).Value & "]")
                        Else
                            pControl.Items.Add(rs1.Fields(0).Value & "[" & rs1.Fields(1).Value & "]" &
                                               " " & rs1.Fields(2).Value)
                        End If
                        rs1.MoveNext()
                    Loop
                ElseIf rs1.Fields.Count = 3 And pControl.Name = "cmbFBFM40" Then
                    Do Until rs1.EOF
                        strNum = rs1.Fields(0).Value
                        strCode = rs1.Fields(1).Value
                        If Strings.Len(strNum & "") = 1 Then strNum = "00" & strNum
                        If Strings.Len(strNum & "") = 2 Then strNum = "0" & strNum
                        If Strings.Len(strCode & "") = 1 Then strCode = "00" & strCode
                        If Strings.Len(strCode & "") = 2 Then strCode = "0" & strCode
                        pControl.Items.Add(strCode & " / " & strNum & "   " &
                                           rs1.Fields(2).Value)                         'Combine FMNum / FMCode  FMName
                        rs1.MoveNext()
                    Loop
                ElseIf rs1.Fields.Count = 3 And pControl.Name = "cmbCBHCBD" Then
                    Do Until rs1.EOF
                        pControl.Items.Add(rs1.Fields(0).Value & "=(" & rs1.Fields(1).Value & ") Find " &
                                           rs1.Fields(2).Value)                         'Combine BPS#,BPS Name,and BPS
                        rs1.MoveNext()
                    Loop
                ElseIf rs1.Fields.Count = 3 Then
                    Do Until rs1.EOF
                        pControl.Items.Add(rs1.Fields(0).Value & "   " & rs1.Fields(1).Value & " - " &
                                           rs1.Fields(2).Value)                         'Combine BPS#,BPS Name,and BPS
                        rs1.MoveNext()
                    Loop
                End If
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

            MsgBox("m_SetControl " & ex.Message)
        End Try
    End Sub

    Public Sub gf_PopWild(ByVal cmbBox As ComboBox, ByVal EVTNum As String, ByVal DISTNum As String, ByVal BPSNum As String,
                         ByVal ComboTable As String, ByVal ProjPath As String)
        Dim strSQL As String
        'Populate the Wildcard box with the new Wildcard values for the selected EVT and BPS
        If IsNumeric(BPSNum) Then  'BPS value is a number not 'any'
            strSQL = "Select WILDCARD " &
                "FROM " & ComboTable & " " &
                "WHERE (EVTR = " & EVTNum &
                " And DIST = " & DISTNum &
                " And BPSRF = " & BPSNum & ")" &
                " Group By WILDCARD " &
                " ORDER BY WILDCARD"
        Else 'BPS value is a text it is an 'any' value
            strSQL = "Select WILDCARD " &
                "FROM " & ComboTable & " " &
                "WHERE (EVTR = " & EVTNum &
                " And DIST = " & DISTNum & ")" &
                " Group By WILDCARD " &
                " ORDER BY WILDCARD"
        End If
        cmbBox.Items.Clear()
        cmbBox.Items.Add("any")
        gf_SetControl(cmbBox, strSQL, ProjPath)
        cmbBox.SelectedIndex = 0
    End Sub

    Public Function gf_ConvertBack(ByVal strCode As String, ByVal ProjPath As String) As Long
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data
        Dim rs3 As New ADODB.Recordset                                  'recordset for data
        Dim rs4 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        'strCode - 11,12,13,31,99, "T20%"..... ProjPath - "c:\...."
        If IsNumeric(strCode) Then
            gf_ConvertBack = Int(strCode)
        Else
            gf_ConvertBack = 0
        End If

        Try
            strSQL = "SELECT EVC " &
                    "FROM LUT_Cover " &
                    "WHERE LowName = '" & strCode & "'"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            If rs1.EOF = False Then              'Return the EVC code
                gf_ConvertBack = rs1.Fields!EVC.Value
            Else                                'Look in cover HighName
                strSQL = "SELECT EVC " &
                        "FROM LUT_Cover " &
                        "WHERE HighName = '" & strCode & "'"
                rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                If rs2.EOF = False Then          'Return the EVC code
                    gf_ConvertBack = rs2.Fields!EVC.Value
                Else                            'Look in Height LowName
                    strSQL = "SELECT EVH " &
                            "FROM LUT_Height " &
                            "WHERE LowName = '" & strCode & "'"
                    rs3.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                    If rs3.EOF = False Then      'Return the EVH code
                        gf_ConvertBack = rs3.Fields!EVH.Value
                    Else                        'Look in Height HighName
                        strSQL = "SELECT EVH " &
                                "FROM LUT_Height " &
                                "WHERE HighName = '" & strCode & "'"
                        rs4.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                        If rs4.EOF = False Then  'Return the EVH code
                            gf_ConvertBack = rs4.Fields!EVH.Value
                        End If
                    End If
                End If
            End If
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing
            If rs4.State <> 0 Then rs4.Close()
            rs4 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing
            If rs4.State <> 0 Then rs4.Close()
            rs4 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
            MsgBox("Error in " & gf_ConvertBack & ": " & ex.Message)
        End Try
    End Function

    Public Function gf_ConvertCode(ByVal strCode As String, ByVal strcovhgt As String, ByVal strlowhigh As String,
                                  ByVal ProjPath As String) As String
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data
        Dim rs3 As New ADODB.Recordset                                  'recordset for data
        Dim rs4 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        'strCode - Number code, strCovhgt - "cov" or "hgt", strlowhigh - "low" or "high", ProjPath - "c:\....."
        gf_ConvertCode = strCode
        Try
            If strcovhgt = "cov" Then 'This is a cover conversion
                If strlowhigh = "low" Then
                    strSQL = "SELECT LowName " &
                             "FROM LUT_Cover " &
                             "WHERE EVC = " & strCode
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    If rs1.EOF = False Then              'Return the LowName text conversion
                        gf_ConvertCode = rs1.Fields!LowName.Value
                    End If
                Else
                    strSQL = "SELECT HighName " &
                             "FROM LUT_Cover " &
                             "WHERE EVC = " & strCode
                    rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                    If rs2.EOF = False Then              'Return the HighName text conversion
                        gf_ConvertCode = rs2.Fields!HighName.Value
                    End If
                End If
            ElseIf strcovhgt = "hgt" Then 'This is a height conversion
                If strlowhigh = "low" Then
                    strSQL = "SELECT LowName " &
                             "FROM LUT_Height " &
                             "WHERE EVH = " & strCode
                    rs3.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                    If rs3.EOF = False Then              'Return the LowName text conversion
                        gf_ConvertCode = rs3.Fields!LowName.Value
                    End If
                Else
                    strSQL = "SELECT HighName " &
                             "FROM LUT_Height " &
                             "WHERE EVH = " & strCode
                    rs4.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                    If rs4.EOF = False Then              'Return the HighName text conversion
                        gf_ConvertCode = rs4.Fields!HighName.Value
                    End If
                End If
            Else
                gf_ConvertCode = strCode 'We don't have a conversion
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing
            If rs4.State <> 0 Then rs4.Close()
            rs4 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing
            If rs4.State <> 0 Then rs4.Close()
            rs4 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
            MsgBox("Error in gf_ConvertCode: " & ex.Message)
        End Try
    End Function

    Public Function gf_ConvertFM40(ByVal FM40 As Long, ByVal ProjPath As String) As String 'Returns the coded FM40
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        'Populate FBFM40 combobox
        strSQL = "SELECT FMNum, FMCode, FMName " &
                     "FROM LUT_FuelModelParameters " &
                     "WHERE FMNum = " & FM40 &
                     " ORDER BY FMNum"
        rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

        gf_ConvertFM40 = rs1.Fields(1).Value & " / " & rs1.Fields(0).Value          'Combine FMNum / FMCode  FMName

        If rs1.State <> 0 Then rs1.Close()
        rs1 = Nothing

        If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
        dbconn = Nothing
    End Function

    Public Function gf_ConvertCanFM(ByVal CanFM As Long, ByVal ProjPath As String) As String 'Returns the coded CanFM
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        'Populate CanFM combobox
        strSQL = "SELECT FM " &
                "FROM LUT_Canadian_FBPS_Fuel_Types " &
                "WHERE FMID = " & CanFM &
                " ORDER BY ID"
        rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

        gf_ConvertCanFM = rs1.Fields!FM.Value

        If rs1.State <> 0 Then rs1.Close()
        rs1 = Nothing

        If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
        dbconn = Nothing
    End Function

    Public Function gf_GetHeightMid(ByVal lngHgt As Long, ByVal ProjPath As String) As Double 'Get height in meters from height code
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        strSQL = "SELECT MidPoint " &
                 "FROM LUT_Height " &
                 "WHERE EVH = " & lngHgt
        rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

        If rs1.EOF = False Then
            gf_GetHeightMid = rs1.Fields!MidPoint.Value
        Else
            gf_GetHeightMid = 0
        End If

        If rs1.State <> 0 Then rs1.Close()
        rs1 = Nothing

        If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
        dbconn = Nothing
    End Function

    Public Sub gf_DeleteMU(ByVal ProjPath As String)
        Dim Response As Object

        Response = MsgBox("Do you really want to delete MU " & gs_MU & "?", 3, "Delete MU ")
        If Response = vbYes Then
            deleteGRIDs(ProjPath) 'Deletes the selected GRID

            Try
                DeleteMUTable(gs_MU, ProjPath) 'Delete the MU Table data from access
            Catch
                MsgBox("Error deleting MU table" & gs_MU & " at this time, try again later.")
            End Try
        End If
    End Sub

    Private Sub DeleteMUTable(ByVal strMUName As String, ByVal ProjPath As String)

        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection & ProjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            strSQL = "DROP TABLE " & strMUName & "_CMB"
            dbconn.Execute(strSQL)
            strSQL = "DROP TABLE " & strMUName & "_Rulesets"
            dbconn.Execute(strSQL)
            strSQL = "DELETE FROM DATA_MU_Name WHERE (Name = '" & strMUName & "')"
            dbconn.Execute(strSQL)

            'Wait for database to catchup
            Threading.Thread.Sleep(1000)

            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If

            MsgBox("Error in DeleteMUTable - " & ex.Message)
        End Try
    End Sub

    Private Async Sub deleteGRIDs(ByVal ProjPath As String)
        Dim MU = gs_MU()

        Try
            Dim container = gs_Map
            Await QueuedTask.Run(
                    Sub()
                        'Delete MU
                        If ItemFactory.Instance.CanGetDataset(ItemFactory.Instance.Create(ProjPath + "\MU\" + MU + ".tif")) Then
                            Dim val_array = Geoprocessing.MakeValueArray(ProjPath + "\MU\" + MU + ".tif", "")
                            Geoprocessing.ExecuteToolAsync("Delete", val_array)
                        ElseIf ItemFactory.Instance.CanGetDataset(ItemFactory.Instance.Create(ProjPath + "\MU\" + MU)) Then
                            Dim val_array = Geoprocessing.MakeValueArray(ProjPath + "\MU\" + MU, "")
                            Geoprocessing.ExecuteToolAsync("Delete", val_array)
                        End If
                    End Sub)
        Catch ex As Exception
            MsgBox("Error deleting selected MU layer" & gs_MU & " at this time, try again later." & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub gf_CheckForDBIndex(prjPath As String, MUName As String)
        'Check for index
        Dim dbconn As New ADODB.Connection                                  'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        prjPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try                                                                 'Add index to Ruleset
            strSQL = "CREATE INDEX LFTFC_ID ON " & MUName & "_Rulesets (ID)"
            dbconn.Execute(strSQL)
        Catch
            'Index exists....Do nothing
        End Try

        Try                                                                 'Add index to CMB
            strSQL = "CREATE INDEX LFTFC_ID ON " & MUName & "_CMB ([VALUE])"
            dbconn.Execute(strSQL)
        Catch
            'Index exists....Do nothing
        End Try

        If dbconn.State <> ConnectionState.Closed Then                     'Database needs to be closed
            dbconn = Nothing
        End If
    End Sub
End Module
