Imports System.Data
Imports System.Data.SQLite
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

    ' Converts database values safely to string.
    Public Function SafeStr(value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then
            Return ""
        End If
        Return value.ToString()
    End Function

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

    Private Function SetControl_read(conn As SQLiteConnection, strSQLstatement As String, items As List(Of List(Of Object)))
        Using cmd As New SQLiteCommand(strSQLstatement, conn)
            Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                While rdr.Read()
                    Dim row As New List(Of Object)
                    For i As Integer = 0 To rdr.FieldCount - 1
                        row.Add(If(rdr.IsDBNull(i), Nothing, rdr.GetValue(i)))
                    Next
                    items.Add(row)
                End While
            End Using
        End Using
        Return items
    End Function


    Public Sub gf_SetControl(pControl As ComboBox,
                             strSQLstatement As String,
                             projPath As String,
                             Optional rdoNameChecked As Boolean = True,
                             Optional ByVal theconn As SQLiteConnection = Nothing)

        If String.IsNullOrWhiteSpace(projPath) Then
            Return
        End If

        Dim dbPath As String = IO.Path.Combine(projPath, gs_LFTFCSQliteName)
        Dim items As New List(Of List(Of Object))()


        Try
            If theconn Is Nothing Then
                Using conn As New SQLiteConnection("Data Source=" & dbPath)
                    conn.Open()
                    items = SetControl_read(conn, strSQLstatement, items)
                End Using
            Else
                items = SetControl_read(theconn, strSQLstatement, items)
            End If

            'Using cmd As New SQLiteCommand(strSQLstatement, conn)
            '    Using rdr As SQLiteDataReader = cmd.ExecuteReader()
            '        While rdr.Read()
            '            Dim row As New List(Of Object)
            '            For i As Integer = 0 To rdr.FieldCount - 1
            '                row.Add(If(rdr.IsDBNull(i), Nothing, rdr.GetValue(i)))
            '            Next
            '            items.Add(row)
            '        End While

            '    End Using
            'End Using
            'End Using

            'pControl.Items.Clear()

            If items.Count = 0 Then
                pControl.Items.Add("N/A")
                Return
            End If

            Dim fieldCount As Integer = items(0).Count

            For Each row In items

                Dim v0 As String = If(fieldCount >= 1, SafeStr(row(0)), "")
                Dim v1 As String = If(fieldCount >= 2, SafeStr(row(1)), "")
                Dim v2 As String = If(fieldCount >= 3, SafeStr(row(2)), "")

                Select Case fieldCount

                    Case 1
                        If v0 <> "" Then
                            pControl.Items.Add(v0)
                        Else
                            pControl.Items.Add("N/A")
                        End If

                    Case 2
                        pControl.Items.Add(v0 & "   " & v1)

                    Case 3

                        Select Case pControl.Name

                            Case "cmbFCCS"
                                pControl.Items.Add(v0 & " (" & v1 & ")   " & v2)

                            Case "cmbEVT", "cmbCopyEVT"
                                If rdoNameChecked Then
                                    pControl.Items.Add(v2 & " " & v0 & "[" & v1 & "]")
                                Else
                                    pControl.Items.Add(v0 & "[" & v1 & "] " & v2)
                                End If

                            Case "cmbFBFM40"
                                Dim strNum As String = v0
                                Dim strCode As String = v1
                                Dim desc As String = v2

                                If strNum.Length = 1 Then strNum = "00" & strNum
                                If strNum.Length = 2 Then strNum = "0" & strNum
                                If strCode.Length = 1 Then strCode = "00" & strCode
                                If strCode.Length = 2 Then strCode = "0" & strCode

                                pControl.Items.Add(strCode & " / " & strNum & "   " & desc)

                            Case "cmbCBHCBD"
                                pControl.Items.Add(v0 & "=(" & v1 & ") Find " & v2)

                            Case Else
                                pControl.Items.Add(v0 & "   " & v1 & " - " & v2)

                        End Select

                End Select

            Next

        Catch ex As Exception
            MsgBox("gf_SetControl " & ex.Message)
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

    Public Function gf_ConvertBack(strCode As String, projPath As String) As Long
        Dim dbPath As String = System.IO.Path.Combine(projPath, gs_LFTFCSQliteName)
        Dim resultVal As Long = 0

        ' strCode can be numeric (11,12,13...) or "T20%"
        If IsNumeric(strCode) Then
            resultVal = CLng(strCode)
        Else
            resultVal = 0
        End If

        Try
            Using conn As New SQLiteConnection("Data Source=" & dbPath)
                conn.Open()

                ' --- 1. Look in LUT_Cover by LowName ---
                Using cmd As New SQLiteCommand(
                "SELECT EVC FROM LUT_Cover WHERE LowName = @code;", conn)

                    cmd.Parameters.AddWithValue("@code", strCode)

                    Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            resultVal = CLng(rdr("EVC"))
                            Return resultVal
                        End If
                    End Using
                End Using

                ' --- 2. Look in LUT_Cover by HighName ---
                Using cmd As New SQLiteCommand(
                "SELECT EVC FROM LUT_Cover WHERE HighName = @code;", conn)

                    cmd.Parameters.AddWithValue("@code", strCode)

                    Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            resultVal = CLng(rdr("EVC"))
                            Return resultVal
                        End If
                    End Using
                End Using

                ' --- 3. Look in LUT_Height by LowName ---
                Using cmd As New SQLiteCommand(
                "SELECT EVH FROM LUT_Height WHERE LowName = @code;", conn)

                    cmd.Parameters.AddWithValue("@code", strCode)

                    Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            resultVal = CLng(rdr("EVH"))
                            Return resultVal
                        End If
                    End Using
                End Using

                ' --- 4. Look in LUT_Height by HighName ---
                Using cmd As New SQLiteCommand(
                "SELECT EVH FROM LUT_Height WHERE HighName = @code;", conn)

                    cmd.Parameters.AddWithValue("@code", strCode)

                    Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            resultVal = CLng(rdr("EVH"))
                            Return resultVal
                        End If
                    End Using
                End Using

            End Using

        Catch ex As Exception
            MsgBox("Error in gf_ConvertBack: " & ex.Message)
        End Try

        Return resultVal
    End Function

    Public Function gf_ConvertCode(strCode As String,
                               strcovhgt As String,
                               strlowhigh As String,
                               projPath As String) As String

        Dim dbPath As String = System.IO.Path.Combine(projPath, gs_LFTFCSQliteName)
        Dim resultVal As String = strCode   ' default: return input if no match

        Try
            Using conn As New SQLiteConnection("Data Source=" & dbPath)
                conn.Open()

                ' -------------------------------------------------------------
                ' COVER CONVERSION
                ' -------------------------------------------------------------
                If strcovhgt = "cov" Then

                    If strlowhigh = "low" Then
                        Using cmd As New SQLiteCommand(
                        "SELECT LowName FROM LUT_Cover WHERE EVC = @evc;", conn)
                            cmd.Parameters.AddWithValue("@evc", strCode)

                            Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                                If rdr.Read() Then
                                    resultVal = rdr("LowName").ToString()
                                    Return resultVal
                                End If
                            End Using
                        End Using

                    Else   ' cov + high
                        Using cmd As New SQLiteCommand(
                        "SELECT HighName FROM LUT_Cover WHERE EVC = @evc;", conn)
                            cmd.Parameters.AddWithValue("@evc", strCode)

                            Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                                If rdr.Read() Then
                                    resultVal = rdr("HighName").ToString()
                                    Return resultVal
                                End If
                            End Using
                        End Using
                    End If

                    ' -------------------------------------------------------------
                    ' HEIGHT CONVERSION
                    ' -------------------------------------------------------------
                ElseIf strcovhgt = "hgt" Then

                    If strlowhigh = "low" Then
                        Using cmd As New SQLiteCommand(
                        "SELECT LowName FROM LUT_Height WHERE EVH = @evh;", conn)
                            cmd.Parameters.AddWithValue("@evh", strCode)

                            Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                                If rdr.Read() Then
                                    resultVal = rdr("LowName").ToString()
                                    Return resultVal
                                End If
                            End Using
                        End Using

                    Else   ' hgt + high
                        Using cmd As New SQLiteCommand(
                        "SELECT HighName FROM LUT_Height WHERE EVH = @evh;", conn)
                            cmd.Parameters.AddWithValue("@evh", strCode)

                            Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                                If rdr.Read() Then
                                    resultVal = rdr("HighName").ToString()
                                    Return resultVal
                                End If
                            End Using
                        End Using
                    End If

                Else
                    ' Unknown conversion category → return original
                    resultVal = strCode
                End If

            End Using

        Catch ex As Exception
            MsgBox("Error in gf_ConvertCode: " & ex.Message)
        End Try

        Return resultVal
    End Function

    Public Function gf_ConvertFM40(FM40 As Long, projPath As String) As String
        Dim dbPath As String = System.IO.Path.Combine(projPath, gs_LFTFCSQliteName)
        Dim resultVal As String = ""

        Try
            Using conn As New SQLiteConnection("Data Source=" & dbPath)
                conn.Open()

                Using cmd As New SQLiteCommand(
                "SELECT FMNum, FMCode, FMName 
                 FROM LUT_FuelModelParameters
                 WHERE FMNum = @fm
                 ORDER BY FMNum;", conn)

                    cmd.Parameters.AddWithValue("@fm", FM40)

                    Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            Dim fmNum As String = rdr("FMNum").ToString()
                            Dim fmCode As String = rdr("FMCode").ToString()

                            resultVal = fmCode & " / " & fmNum   ' same output as original
                        End If
                    End Using
                End Using

            End Using

        Catch ex As Exception
            MsgBox("Error in gf_ConvertFM40: " & ex.Message)
        End Try

        Return resultVal
    End Function

    Public Function gf_ConvertCanFM(CanFM As Long, projPath As String) As String
        Dim dbPath As String = System.IO.Path.Combine(projPath, gs_LFTFCSQliteName)
        Dim resultVal As String = ""

        Try
            Using conn As New SQLiteConnection("Data Source=" & dbPath)
                conn.Open()

                Using cmd As New SQLiteCommand(
                "SELECT FM
                 FROM LUT_Canadian_FBPS_Fuel_Types
                 WHERE FMID = @id
                 ORDER BY ID;", conn)

                    cmd.Parameters.AddWithValue("@id", CanFM)

                    Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            resultVal = rdr("FM").ToString()
                        End If
                    End Using
                End Using

            End Using

        Catch ex As Exception
            MsgBox("Error in gf_ConvertCanFM: " & ex.Message)
        End Try

        Return resultVal
    End Function

    Public Function gf_GetHeightMid(lngHgt As Long, projPath As String) As Double
        Dim dbPath As String = System.IO.Path.Combine(projPath, gs_LFTFCSQliteName)
        Dim resultVal As Double = 0

        Try
            Using conn As New SQLiteConnection("Data Source=" & dbPath)
                conn.Open()

                Using cmd As New SQLiteCommand(
                "SELECT MidPoint
                 FROM LUT_Height
                 WHERE EVH = @evh;", conn)

                    cmd.Parameters.AddWithValue("@evh", lngHgt)

                    Using rdr As SQLiteDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            resultVal = Convert.ToDouble(rdr("MidPoint"))
                        End If
                    End Using
                End Using

            End Using

        Catch ex As Exception
            MsgBox("Error in gf_GetHeightMid: " & ex.Message)
        End Try

        Return resultVal
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

    Private Sub DeleteMUTable(strMUName As String, projPath As String)
        Dim dbPath As String = System.IO.Path.Combine(projPath, gs_LFTFCSQliteName)

        Try
            Using conn As New SQLiteConnection("Data Source=" & dbPath)
                conn.Open()

                ' DROP TABLE commands cannot use parameters (SQLite limitation)
                Using cmdDrop1 As New SQLiteCommand($"DROP TABLE IF EXISTS {strMUName}_CMB;", conn)
                    cmdDrop1.ExecuteNonQuery()
                End Using

                Using cmdDrop2 As New SQLiteCommand($"DROP TABLE IF EXISTS {strMUName}_Rulesets;", conn)
                    cmdDrop2.ExecuteNonQuery()
                End Using

                ' DELETE statement must be parameterized
                Using cmdDel As New SQLiteCommand(
                "DELETE FROM DATA_MU_Name WHERE Name = @name;", conn)
                    cmdDel.Parameters.AddWithValue("@name", strMUName)
                    cmdDel.ExecuteNonQuery()
                End Using

                ' Allow database file refresh like original code
                Threading.Thread.Sleep(1000)

            End Using

        Catch ex As Exception
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
        Dim dbPath As String = System.IO.Path.Combine(prjPath, gs_LFTFCSQliteName)

        Try
            Using conn As New SQLiteConnection("Data Source=" & dbPath)
                conn.Open()

                ' -------------------------------------------------------------
                ' Create index on <MUName>_Rulesets(ID)
                ' -------------------------------------------------------------
                Try
                    Using cmd As New SQLiteCommand(
                        $"CREATE INDEX IF NOT EXISTS LFTFC_ID_{MUName}_Rulesets ON {MUName}_Rulesets (ID);",
                        conn)
                        cmd.ExecuteNonQuery()
                    End Using
                Catch
                    ' Index exists — do nothing
                End Try

                ' -------------------------------------------------------------
                ' Create index on <MUName>_CMB(Value)
                ' -------------------------------------------------------------
                Try
                    Using cmd As New SQLiteCommand(
                        $"CREATE INDEX IF NOT EXISTS LFTFC_VAL_{MUName}_CMB ON {MUName}_CMB (Value);",
                        conn)
                        cmd.ExecuteNonQuery()
                    End Using
                Catch
                    ' Index exists — do nothing
                End Try

            End Using

        Catch ex As Exception
            MsgBox("Error in gf_CheckForDBIndex - " & ex.Message)
        End Try
    End Sub
End Module