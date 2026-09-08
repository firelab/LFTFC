Imports System.Data
Imports System.IO
Imports ArcGIS.Desktop.Framework.Contracts
Imports ArcGIS.Desktop.Framework.Threading.Tasks

Friend Class BFuelLog
    Inherits Button

    Protected Overrides Sub OnClick()
        'TODO : Add implementation here
        'Check for project directory
        If gs_validProject = False Then
            If MsgBox("The project path must be set before this function is available." + vbCrLf +
                   " Would you like to set the project path now?", MsgBoxStyle.OkCancel, "!!!!Set project path!!!!") = vbOK Then
                gs_SetProjectDir()
            Else
                'Do nothing and leave project directory Not Set
            End If
        Else
            Try
                'Make Fuel Log file
                MakeLogFile(gs_ProjectPath, gs_MU() + "_Rulesets")
            Catch ex As Exception
                MsgBox("Fuel Log " + ex.Message)
            End Try
        End If
    End Sub

    Private Async Sub MakeLogFile(ByVal strProjectPath As String, ByVal rulesR As String)

        Await QueuedTask.Run(
        Sub()

            Dim outputPath As String = Path.Combine(strProjectPath, "Output", gs_MU() & "FuelLog.csv")
            Using oWrite As New StreamWriter(outputPath)

                Dim dbPath As String = Path.Combine(strProjectPath, gs_LFTFCSQliteName)
                Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

                Dim preEVTDist As String = ""
                Dim acresCount As String = ""
                Dim frmStatusLog As New frmWorkStatus()

                frmStatusLog.Show()
                frmStatusLog.UpdateStatus("Writing FuelLog.csv")

                ' ---------------------------------------------------
                ' Write header rows to CSV
                ' ---------------------------------------------------
                oWrite.WriteLine("Fuel Log created at " & Now.ToShortTimeString & " on " & Now.ToShortDateString)
                oWrite.WriteLine("A product of LF TFC Toolbar")
                oWrite.WriteLine("Tobin Smail LANDFIRE Project / LANDFIRE (Fuel)")
                oWrite.WriteLine()
                oWrite.WriteLine()
                oWrite.WriteLine("Descriptions")
                oWrite.WriteLine("EVT - Existing Vegetation Type")
                oWrite.WriteLine("DIST - Fuel Disturbance")
                oWrite.WriteLine("EVC - Existing Vegetation Cover")
                oWrite.WriteLine("EVH - Existing Vegetation Height")
                oWrite.WriteLine("BPS - Biophysical Setting")
                oWrite.WriteLine("Wild - Any additional GRID, Refresh uses a post disturbance GRID")
                oWrite.WriteLine("FBFM13 - Fire Behavior Fuel Model Anderson 13")
                oWrite.WriteLine("FBFM40 - Fire Behavior Fuel Model Scott and Burgan 40")
                oWrite.WriteLine("CFBPS FT - Canadian Fire Behavior Prediction System Fuel Type")
                oWrite.WriteLine("FCCS - Fuel Characterization Classification System, Ottmar")
                oWrite.WriteLine("FLM - Fuel Loading Models, Lutes")
                oWrite.WriteLine("CG - Canopy Fuel Guide")
                oWrite.WriteLine("CC - Canopy Cover")
                oWrite.WriteLine("CH - Canopy Height")
                oWrite.WriteLine("CBH13mx10 - Canopy Base Height meters times 10 associated with the FBFM13")
                oWrite.WriteLine("CBH40mx10 - Canopy Base Height meters times 10 associated with the FBFM40")
                oWrite.WriteLine("CBD13x100 - Canopy Bulk Density Kg/m^3 times 100 associated with the FBFM13")
                oWrite.WriteLine("CBD40x100 - Canopy Bulk Density Kg/m^3 times 100 associated with the FBFM40")
                oWrite.WriteLine("On/Off - Rule turned on or rule turned off")
                oWrite.WriteLine("Acres - Number of acres affected by the rule.")
                oWrite.WriteLine()
                oWrite.WriteLine("Range of Cover, Range of Height, BPS, WILD, FM13, FM40, CFBPS FT, FCCS, FLM, CG, CC, CH, " &
                                 "CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, Notes, Acres")

                ' ---------------------------------------------------
                ' SQLite SELECT query
                ' ---------------------------------------------------
                Dim sql As String =
                    "SELECT Id, EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " &
                    "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                    "CBH13mx10, CBH40mx10, OnOff, Notes, PixelCount " &
                    "FROM " & rulesR & " " &
                    "ORDER BY OnOff DESC, EVT, DIST, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, " &
                    "Height_Low, Height_High"

                Try
                    Using conn As New SQLite.SQLiteConnection(connString)
                        conn.Open()

                        Using cmd As New SQLite.SQLiteCommand(sql, conn)
                            Using rd As SQLite.SQLiteDataReader = cmd.ExecuteReader()

                                While rd.Read()

                                    ' ---------------------------------------------------
                                    ' Pixel Count → Acres conversion
                                    ' ---------------------------------------------------
                                    If rd("PixelCount") Is DBNull.Value OrElse rd("PixelCount").ToString() = "" Then
                                        acresCount = "Not Calculated"
                                    Else
                                        Dim pix As Integer = CInt(rd("PixelCount"))
                                        acresCount = Math.Round(pix * 0.2223948, 3).ToString()
                                    End If

                                    ' ---------------------------------------------------
                                    ' EVT status display
                                    ' ---------------------------------------------------
                                    Dim evtVal As String = rd("EVT").ToString()
                                    If preEVTDist <> evtVal Then
                                        preEVTDist = evtVal
                                        frmStatusLog.UpdateStatus("Writing EVT - " & evtVal)
                                    End If

                                    ' ---------------------------------------------------
                                    ' Write CSV row
                                    ' ---------------------------------------------------
                                    oWrite.WriteLine(
                                        GetEVTName(evtVal, strProjectPath) & "," &
                                        rd("DIST").ToString() & "," &
                                        GetRangeCover(rd("Cover_Low"), rd("Cover_High"), strProjectPath) & "," &
                                        GetRangeHeight(rd("Height_Low"), rd("Height_High"), strProjectPath) & "," &
                                        rd("BPSRF").ToString() & "," &
                                        rd("Wildcard").ToString() & "," &
                                        rd("FBFM13").ToString() & "," &
                                        rd("FBFM40").ToString() & "," &
                                        rd("CanFM").ToString() & "," &
                                        rd("FCCS").ToString() & "," &
                                        rd("FLM").ToString() & "," &
                                        rd("Canopy").ToString() & "," &
                                        rd("CCover").ToString() & "," &
                                        rd("CHeight").ToString() & "," &
                                        rd("CBH13mx10").ToString() & "," &
                                        rd("CBH40mx10").ToString() & "," &
                                        rd("CBD13x100").ToString() & "," &
                                        rd("CBD40x100").ToString() & "," &
                                        rd("OnOff").ToString() & "," &
                                        rd("Notes").ToString() & "," &
                                        acresCount)

                                End While

                            End Using
                        End Using
                    End Using

                Catch ex As Exception
                    MsgBox("Error creating FuelLog file." & vbCrLf & ex.Message)
                End Try

                frmStatusLog.Close()

            End Using  ' oWrite
        End Sub)

    End Sub

    Private Function GetEVTName(ByVal numEVT As Integer,
                            ByVal strProjectPath As String) As String

        Dim dbPath As String = Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        'Determine whether EVT or EVT_Dist column applies
        Dim evtField As String = If(numEVT < 10000, "EVT", "EVT_Dist")

        'SQL: parameterized & simplified
        Dim sql As String =
        "SELECT " & evtField & ", EVT_Name " &
        "FROM XWALK_EVT_EVG_EVS " &
        "WHERE " & evtField & " = @EVT " &
        "LIMIT 1"

        Try
            Using conn As New SQLite.SQLiteConnection(connString)
                conn.Open()

                Using cmd As New SQLite.SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@EVT", numEVT)

                    Using rd As SQLite.SQLiteDataReader = cmd.ExecuteReader()

                        If rd.Read() Then
                            If rd("EVT_Name") Is DBNull.Value OrElse rd("EVT_Name").ToString() = "" Then
                                Return numEVT & "   Name not available."
                            Else
                                Return numEVT & "  " & rd("EVT_Name").ToString()
                            End If
                        Else
                            'no match found in table
                            Return numEVT & "   Name not available."
                        End If

                    End Using
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Error in GetEVTName: " & ex.Message)
            Return numEVT & "   Name not available."
        End Try

    End Function

    Private Function GetRangeCover(ByVal covLow As Integer, ByVal covHigh As Integer, ByVal strprojectPath As String) As String
        GetRangeCover = gf_ConvertCode(CStr(covLow), "cov", "low", strprojectPath) +
                        gf_ConvertCode(CStr(covHigh), "cov", "high", strprojectPath)
    End Function

    Private Function GetRangeHeight(ByVal hgtLow As Integer, ByVal hgtHigh As Integer, ByVal strprojectPath As String) As String
        GetRangeHeight = gf_ConvertCode(CStr(hgtLow), "hgt", "low", strprojectPath) +
                        gf_ConvertCode(CStr(hgtHigh), "hgt", "high", strprojectPath)
    End Function
End Class

