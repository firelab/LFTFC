Imports System.Data
Imports ArcGIS.Desktop.Framework.Contracts
Imports ArcGIS.Desktop.Framework.Threading.Tasks

Friend Class BFuleLog
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
               Dim oWrite As New System.IO.StreamWriter(strProjectPath + "\Output\" + gs_MU() + "FuelLog.csv")
               Dim acresCount As String                                        'Stores the calculates acres from the pixel count.
               Dim rs1 As New ADODB.Recordset                                  'recordset for data
               Dim preEVTDist As String                                        'Stores the previous EVT and dist see if it changed
               Dim dbconn As New ADODB.Connection                              'DB connection
               dbconn.ConnectionString = gs_DBConnection + strProjectPath + "\" + gs_LFTFCDBName
               dbconn.Open()
               Dim strSQL As String 'SQL variable for this module
               Dim frmStatusLog As New frmWorkStatus

               Try
                   frmStatusLog.Show()
                   frmStatusLog.UpdateStatus("Writing FuelLog.csv")

                   oWrite.WriteLine("Fuel Log created at " + Now.ToShortTimeString + " on " + Now.ToShortDateString)
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
                   oWrite.WriteLine("Acres - Number of acres effected by the rule.")
                   oWrite.WriteLine()
                   oWrite.WriteLine("Range of Cover, Range of Height, BPS, WILD, FM13, FM40, CFBPS FT, FCCS, FLM, CG,CC, CH, " &
                                 "CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, Notes, Acres")

                   strSQL = "SELECT Id, EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " +
                     "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, CBH13mx10, " +
                     "CBH40mx10, OnOff, Notes, PixelCount " +
                     "FROM " + rulesR + " " +
                     "ORDER BY OnOff Desc, EVT, DIST, BPSRF Desc, Wildcard Desc, Cover_Low, Cover_High, " +
                     "Height_Low, Height_High"
                   rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                   preEVTDist = "" 'Set the preEVT as nothing

                   'Make a New log file
                   Do Until rs1.EOF
                       'Get acres
                       If (rs1.Fields!PixelCount.Value & "") = "" Then
                           acresCount = "Not Calculated"
                       Else
                           acresCount = Math.Round(Int(rs1.Fields!PixelCount.Value) * 0.2223948, 3)
                       End If

                       If preEVTDist <> (rs1.Fields!EVT.Value & "") Then
                           preEVTDist = rs1.Fields!EVT.Value & ""
                           frmStatusLog.UpdateStatus("Writting FVT - " & rs1.Fields!EVT.Value)
                       End If
                       oWrite.WriteLine(GetEVTName(rs1.Fields!EVT.Value, strProjectPath) &
                         "," & rs1.Fields!DIST.Value &
                         "," & GetRangeCover(rs1.Fields!Cover_Low.Value, rs1.Fields!Cover_High.Value, strProjectPath) &
                         "," & GetRangeHeight(rs1.Fields!Height_Low.Value, rs1.Fields!Height_High.Value, strProjectPath) &
                         "," & rs1.Fields!BPSRF.Value &
                         "," & rs1.Fields!Wildcard.Value &
                         "," & rs1.Fields!FBFM13.Value &
                         "," & rs1.Fields!FBFM40.Value &
                         "," & rs1.Fields!CanFM.Value &
                         "," & rs1.Fields!FCCS.Value &
                         "," & rs1.Fields!FLM.Value &
                         "," & rs1.Fields!Canopy.Value &
                         "," & rs1.Fields!CCover.Value &
                         "," & rs1.Fields!CHeight.Value &
                         "," & rs1.Fields!CBH13mx10.Value &
                         "," & rs1.Fields!CBH40mx10.Value &
                         "," & rs1.Fields!CBD13x100.Value &
                         "," & rs1.Fields!CBD40x100.Value &
                         "," & rs1.Fields!OnOff.Value &
                         "," & rs1.Fields!Notes.Value &
                         "," & acresCount)
                       rs1.MoveNext()
                   Loop

                   oWrite.Close()
                   If rs1.State <> 0 Then rs1.Close()
                   rs1 = Nothing

                   If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                   dbconn = Nothing
                   MsgBox("Fuel Log is in the output folder as " + gs_MU() + "FuelLog.txt")

               Catch ex As Exception
                   If rs1.State <> 0 Then rs1.Close()
                   rs1 = Nothing

                   If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                   dbconn = Nothing

                   MsgBox("Error creating FuelLog file." + vbCrLf + ex.Message)
               End Try

               frmStatusLog.Close()
           End Sub)
    End Sub

    Private Function GetEVTName(ByVal numEVT As Integer, ByVal strProjectPath As String) As String
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection + strProjectPath + "\" + gs_LFTFCDBName
        dbconn.Open()

        Dim strSQL2 As String 'SQL variable for this module
        Dim EVTTable As String = "" 'Stores the type of evt being used
        Dim EVTField As String = "" 'Stores the field being used

        If numEVT < 10000 Then              'Use EVT code
            EVTField = "EVT"
        Else                                'Use EVT_Dist code
            EVTField = "EVT_Dist"
        End If

        strSQL2 = "SELECT XWALK_EVT_EVG_EVS." + EVTField + ", XWALK_EVT_EVG_EVS.EVT_Name " +
            "FROM XWALK_EVT_EVG_EVS " +
            "GROUP BY XWALK_EVT_EVG_EVS." + EVTField + ", XWALK_EVT_EVG_EVS.EVT_Name " +
            "HAVING (((XWALK_EVT_EVG_EVS." + EVTField + ")=" + CStr(numEVT) + "))"
        rs1.Open(strSQL2, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

        If IsNothing(rs1.Fields!EVT_Name.Value) Then 'Return just the number
            GetEVTName = CStr(numEVT) + "   Name not available."
        Else
            GetEVTName = CStr(numEVT) + "  " + CStr(rs1.Fields!EVT_Name.Value)
        End If

        If rs1.State <> 0 Then rs1.Close()
        rs1 = Nothing

        If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
        dbconn = Nothing
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

