Imports System.Windows.Forms
Imports ArcGIS.Desktop.Core
Imports ArcGIS.Desktop.Core.Geoprocessing
Imports ArcGIS.Desktop.Framework.Threading.Tasks
Imports ArcGIS.Desktop.Internal.Mapping.Views.PropertyPages
Imports ArcGIS.Desktop.Mapping

Public Class frmGRID
    Private strSQL As String 'SQL variable for this module
    Private strProjectPath As String

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strProjectPath = gs_ProjectPath

    End Sub

    Private Sub chkFBFM13_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkFBFM13.CheckStateChanged
        Try
            'Set text box to match
            txtFBFM13.Enabled = chkFBFM13.Checked
        Catch ex As Exception
            MsgBox("Error in chkFBFM13_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkFBFM40_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkFBFM40.CheckStateChanged
        Try
            'Set text box to match
            txtFBFM40.Enabled = chkFBFM40.Checked
        Catch ex As Exception
            MsgBox("Error in chkFBFM40_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkCanFM_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCanFM.CheckStateChanged
        Try
            'Set text box to match
            txtCanFM.Enabled = chkCanFM.Checked
        Catch ex As Exception
            MsgBox("Error in chkCanFM_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkFCCS_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkFCCS.CheckStateChanged
        Try
            txtFCCS.Enabled = chkFCCS.Checked
        Catch ex As Exception
            MsgBox("Error in chkFCCS_CheckChanged - " & ex.Message)
        End Try
        'Set text box to match

    End Sub

    Private Sub chkFLM_CheckStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkFLM.CheckStateChanged
        Try
            txtFLM.Enabled = chkFLM.Checked
        Catch ex As Exception
            MsgBox("Error in chkFLM_CheckStateChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkGuide_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkGuide.CheckStateChanged
        Try
            txtGuide.Enabled = chkGuide.Checked
        Catch ex As Exception
            MsgBox("Error in chkGuide_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkCoverHeight_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCoverHeight.CheckStateChanged
        Try
            txtCover.Enabled = chkCoverHeight.Checked
            txtHeight.Enabled = chkCoverHeight.Checked
        Catch ex As Exception
            MsgBox("Error in chkCover_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkCBH13_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCBH13.CheckedChanged
        Try
            txtCBH13.Enabled = chkCBH13.Checked
        Catch ex As Exception
            MsgBox("Error in chkCBH13_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkCBH40_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCBH40.CheckedChanged
        Try
            txtCBH40.Enabled = chkCBH40.Checked
        Catch ex As Exception
            MsgBox("Error in chkCBH40_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkCBD13_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCBD13.CheckedChanged
        Try
            txtCBD13.Enabled = chkCBD13.Checked
        Catch ex As Exception
            MsgBox("Error in chkCBD13_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkCBD40_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCBD40.CheckedChanged
        Try
            txtCBD40.Enabled = chkCBD40.Checked
        Catch ex As Exception
            MsgBox("Error in chkCBD40_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkCBHMult_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCBHMult.CheckedChanged
        Try
            txtCBH13Mult.Enabled = chkCBHMult.Checked
            txtCBH40Mult.Enabled = chkCBHMult.Checked
        Catch ex As Exception
            MsgBox("Error in chkCBHMult_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkCBDMult_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCBDMult.CheckedChanged
        Try
            txtCBD13Mult.Enabled = chkCBDMult.Checked
            txtCBD40Mult.Enabled = chkCBDMult.Checked
        Catch ex As Exception
            MsgBox("Error in chkCBDMult_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdCreateGRID_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreateGRID.Click
        Try
            'Stores the fuel database interation for fuel creation
            Dim strFuelDatabase As String = "FuelDatabase1" 'Start as 1"
            Dim MU As String = gs_MU 'Get currently selected

            'Disable the create button
            cmdCreateGRID.Text = "Wait"   'So people know to wait
            cmdCreateGRID.Enabled = False 'Disable the button so it does not get clicked when processing

            'Delete fuel databases
            DeleteFuelDatabase(strFuelDatabase)

            'Set the start fuel database
            strFuelDatabase = SetFuelDatabase(strFuelDatabase, MU)
            Threading.Thread.Sleep(5000)

            'Check for valid fuel names
            If ValidFuelName() = False Then
                cmdCreateGRID.Text = "Create " & vbCrLf & "GRIDs" 'Change back to default text
                cmdCreateGRID.Enabled = True                      'Enable the button
                Refresh()                                      'Refresh the page to show reflect the changes
                Exit Sub 'Exit sub if a valid name is not used
            End If

            'Make a collection of Fuel to be created
            Dim theFuels As New List(Of Fuel)

            'Assign fuel values to each specified CMB fields
            If chkFBFM13.Checked Then
                AssignValues("FBFM13", MU, strFuelDatabase)
                If PixLeftBehind("FBFM13", MU, strFuelDatabase) = False Then GoTo CloseAndExit     'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "FBFM13", .SaveAs = chkForTiff(txtFBFM13.Text)})
            End If
            If chkFBFM40.Checked Then
                AssignValues("FBFM40", MU, strFuelDatabase)
                If PixLeftBehind("FBFM40", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "FBFM40", .SaveAs = chkForTiff(txtFBFM40.Text)})
            End If
            If chkCanFM.Checked Then
                AssignValues("CanFM", MU, strFuelDatabase)
                If PixLeftBehind("CanFM", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "CanFM", .SaveAs = chkForTiff(txtCanFM.Text)})
            End If
            If chkFCCS.Checked Then
                AssignValues("FCCS", MU, strFuelDatabase)
                If PixLeftBehind("FCCS", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "FCCS", .SaveAs = chkForTiff(txtFCCS.Text)})
            End If
            If chkFLM.Checked Then
                AssignValues("FLM", MU, strFuelDatabase)
                If PixLeftBehind("FLM", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "FLM", .SaveAs = chkForTiff(txtFLM.Text)})
            End If
            If chkGuide.Checked Then
                AssignValues("Canopy", MU, strFuelDatabase)
                If PixLeftBehind("Canopy", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "Canopy", .SaveAs = chkForTiff(txtGuide.Text)})
            End If
            If chkCoverHeight.Checked Then
                AssignCC_CHProg(MU, strFuelDatabase)
                If PixLeftBehind("CCover", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "CCover", .SaveAs = chkForTiff(txtCover.Text)})
                If PixLeftBehind("CHeight", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "CHeight", .SaveAs = chkForTiff(txtHeight.Text)})
            End If
            If chkCBH13.Checked Then
                'CC and CH If not checked assign values so CBH can be calculated
                If chkCoverHeight.Checked = False Then
                    AssignCC_CHProg(MU, strFuelDatabase)
                    If PixLeftBehind("CCover", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                    If PixLeftBehind("CHeight", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                End If
                'Assign CBH where rules are not 9999 before CC calculation for everything else
                If chkCBHRules.Checked Then
                    AssignValues("CBH13mx10", MU, strFuelDatabase)
                Else
                    Assign9999("CBH13mx10", MU)
                End If
                'Do Napoli Plot based CBH LM Equations method
                CBH_LM_EQs("CBH13mx10", CDbl(txtCBH13Mult.Text), MU)
                If PixLeftBehind("CBH13mx10", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "CBH13mx10", .SaveAs = chkForTiff(txtCBH13.Text)})
            End If
            If chkCBH40.Checked Then
                'CC and CH If not checked assign values so CBH can be calculated
                If chkCoverHeight.Checked = False And chkCBH13.Checked = False Then
                    AssignCC_CHProg(MU, strFuelDatabase)
                    If PixLeftBehind("CCover", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                    If PixLeftBehind("CHeight", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                End If
                'Assign CBH where rules are not 9999 before CC calculation for everything else
                If chkCBHRules.Checked Then
                    AssignValues("CBH40mx10", MU, strFuelDatabase)
                Else
                    Assign9999("CBH40mx10", MU)
                End If
                'Do Napoli Plot based CBH LM Equations method
                CBH_LM_EQs("CBH40mx10", CDbl(txtCBH40Mult.Text), MU)
                If PixLeftBehind("CBH40mx10", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "CBH40mx10", .SaveAs = chkForTiff(txtCBH40.Text)})
            End If
            If chkCBD13.Checked Then
                'CC and CH If not checked assign values so CBD can be calculated
                If chkCoverHeight.Checked = False And chkCBH13.Checked = False And
                    chkCBH40.Checked = False Then
                    AssignCC_CHProg(MU, strFuelDatabase)
                    If PixLeftBehind("CCover", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                    If PixLeftBehind("CHeight", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                End If
                'Assign CBD where rules are not 9999 before CC calculation for everything else
                If chkCBDRules.Checked Then
                    AssignValues("CBD13x100", MU, strFuelDatabase)
                Else
                    Assign9999("CBD13x100", MU)
                End If
                'Calculate CBD using plot based GLM
                CalcCBDGLM("CBD13x100", CDbl(txtCBD13Mult.Text), MU)
                If PixLeftBehind("CBD13x100", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "CBD13x100", .SaveAs = chkForTiff(txtCBD13.Text)})
            End If
            If chkCBD40.Checked Then
                If chkCoverHeight.Checked = False And chkCBH13.Checked = False And
                    chkCBH40.Checked = False And chkCBD13.Checked = False Then
                    AssignCC_CHProg(MU, strFuelDatabase)
                    If PixLeftBehind("CCover", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                    If PixLeftBehind("CHeight", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                End If
                'Assign CBD where rules are not 9999 before CC calculation for everything else
                If chkCBDRules.Checked Then
                    AssignValues("CBD40x100", MU, strFuelDatabase)
                Else
                    Assign9999("CBD40x100", MU)
                End If
                'Calculate CBD using plot based GLM
                CalcCBDGLM("CBD40x100", CDbl(txtCBD40Mult.Text), MU)
                If PixLeftBehind("CBD40x100", MU, strFuelDatabase) = False Then GoTo CloseAndExit      'If cancel close the form and do not continue
                theFuels.Add(New Fuel With {.FuelType = "CBD40x100", .SaveAs = chkForTiff(txtCBD40.Text)})
            End If

            'Set raster values and make rasters
            SetRasterValues(theFuels, MU)

            Close()
        Catch ex As Exception
            MsgBox("Error in cmdCreateGRID_Click - " & ex.Message)
        End Try
CloseAndExit:
        Close()
        Exit Sub
    End Sub

    Public Class Fuel
        Public Property FuelType As String
        Public Property SaveAs As String
    End Class

    Private Function SetFuelDatabase(ByVal DBName As String, ByVal MUName As String) As String
        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            'Set FuelDatabase
            If Strings.Right(DBName, 1) > 3 Then
                MsgBox("To many diffent MU Fuel Grids being created at the same time." & vbCrLf &
                       "Wait for some Fuel Grids to finish and try again.")
            Else
                strSQL = "SELECT * INTO " & DBName & " FROM " & MUName & "_Rulesets"
                dbconn.Execute(strSQL)
            End If

            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
            DBName = SetFuelDatabase(Strings.Left(DBName, 12) & Strings.Right(DBName, 1) + 1, MUName)
        End Try

        SetFuelDatabase = DBName
    End Function

    Private Sub DeleteFuelDatabase(ByVal DBName As String)
        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            If Strings.Right(DBName, 1) < 4 Then
                strSQL = "DROP TABLE " & DBName
                dbconn.Execute(strSQL)
                DBName = Strings.Left(DBName, 12) & Strings.Right(DBName, 1) + 1
                DeleteFuelDatabase(DBName)
            End If

            If dbconn.State <> System.Data.ConnectionState.Closed Then                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            'Then there are not any fuel databases so do nothing
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Try
            Close()
        Catch ex As Exception
            MsgBox("Error in cmdCancel_Click - " & ex.Message)
        End Try

    End Sub

    Private Function ValidFuelName() As Boolean
        Dim strSaveAs As String     'The new name for the current GRID
        Dim colName As Collection   'Stores the proposed names of GRIDs

        colName = New Collection    'Collection stores all the valid names for processing

        Refresh()

        Try
            If chkFBFM13.Checked = True Then
                If gs_ValidName(txtFBFM13.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtFBFM13.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtFBFM13.Text)
            End If
            If chkFBFM40.Checked = True Then
                If gs_ValidName(txtFBFM40.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtFBFM40.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtFBFM40.Text)
            End If
            If chkCanFM.Checked = True Then
                If gs_ValidName(txtCanFM.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtCanFM.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtCanFM.Text)
            End If
            If chkFCCS.Checked = True Then
                If gs_ValidName(txtFCCS.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtFCCS.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtFCCS.Text)
            End If
            If chkFLM.Checked = True Then
                If gs_ValidName(txtFLM.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtFLM.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtFLM.Text)
            End If
            If chkCoverHeight.Checked = True Then
                If gs_ValidName(txtCover.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtCover.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtCover.Text)
            End If
            If chkCoverHeight.Checked = True Then
                If gs_ValidName(txtHeight.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtHeight.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtHeight.Text)
            End If
            If chkCBH13.Checked = True Then
                If gs_ValidName(txtCBH13.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtCBH13.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtCBH13.Text)
            End If
            If chkCBH40.Checked = True Then
                If gs_ValidName(txtCBH40.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtCBH40.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtCBH40.Text)
            End If
            If chkCBD13.Checked = True Then
                If gs_ValidName(txtCBD13.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtCBD13.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtCBD13.Text)
            End If
            If chkCBD40.Checked = True Then
                If gs_ValidName(txtCBD40.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtCBD40.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtCBD40.Text)
            End If
            If chkGuide.Checked = True Then
                If gs_ValidName(txtGuide.Text, 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    strSaveAs = txtGuide.Text
                    Dim errName As New System.Exception(strSaveAs & " is not a valid name or is already in use.")
                    Throw errName
                End If
                colName.Add(txtGuide.Text)
            End If

            'Test to see if the names are the same
            If sameName(colName) = True Then
                Dim errName As New System.Exception("Cannot use duplicate names.")
                Throw errName
            End If

            ValidFuelName = True
        Catch ex As Exception
            MsgBox(ex.Message)
            ValidFuelName = False
        End Try

        Refresh()
    End Function

    Private Function sameName(ByVal colName As Collection) As Boolean
        Dim lngName As Long 'Collection Name number location
        Dim i As Long 'Collecting index

        sameName = False

        If colName.Count > 1 Then
            For lngName = 1 To colName.Count
                i = lngName
                Do While i <= colName.Count - 1
                    If colName.Item(lngName) = colName.Item(i + 1) Then
                        sameName = True
                        Exit Function
                    End If
                    i = i + 1
                Loop
            Next lngName
        End If
    End Function

    Private Sub AssignValues(ByVal FuelName As String, ByVal MUName As String, ByVal RulesTable As String)
        'Dim rs1 As New ADODB.Recordset                                      'recordset for data
        Dim dbconn As New ADODB.Connection                                  'DB connection
        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim MUTable = MUName + "_CMB"
            Dim adjSET As String                                            'Adjusts the SET portion of the SQL statement
            Dim adjWHERE As String                                          'Adjusts the WHERE portion of the SQL statement

            'Reset selected fuel in cmbrf table variable to 9999
            strSQL = "UPDATE " & MUTable & " " &
                     "SET " & MUTable & ".New" & FuelName & " = 9999"
            dbconn.Execute(strSQL)                                           'Run the SQL statement

            If FuelName = "FBFM40" Then
                adjSET = "FBFM40 = Int(Right(" & RulesTable & ".FBFM40, 3))"
                adjWHERE = "IIf([" & RulesTable & "]![" & FuelName & "]='9999',9999,Int(Right([" & RulesTable & "]![" & FuelName & "],3)))"
            ElseIf FuelName = "CanFM" Then
                adjSET = "CanFM = Int(Right(" & RulesTable & ".CanFM,3))"
                adjWHERE = "IIf([" & RulesTable & "]![" & FuelName & "]='9999',9999,Int(Right([" & RulesTable & "]![" & FuelName & "],3)))"
            Else
                adjSET = FuelName & " = " & RulesTable & "." & FuelName
                adjWHERE = "[" & RulesTable & "]![" & FuelName & "]"
            End If

            'Update all cmbrf table variables with Rules and where BPS and Wildcard are equal to "any"
            strSQL = "UPDATE " & MUTable & " " &
                     "INNER JOIN " & RulesTable & " ON " & MUTable & ".DIST = " & RulesTable & ".DIST " &
                     "AND " & MUTable & ".EVTR = " & RulesTable & ".EVT " &
                     "SET " & MUTable & ".New" & adjSET & " " &
                     "WHERE (([" & RulesTable & "]![OnOff]='On') AND " &
                            "(" & adjWHERE & "<>9999) And " &
                            "([" & MUTable & "]![EVCR] Between Int([" & RulesTable & "]![Cover_Low]) And Int([" & RulesTable & "]![Cover_High])) And " &
                            "([" & MUTable & "]![EVHR] Between Int([" & RulesTable & "]![Height_Low]) And Int([" & RulesTable & "]![Height_High])) AND " &
                            "([" & RulesTable & "]![BPSRF]='any') AND " &
                            "([" & RulesTable & "]![Wildcard]='any'))"
            dbconn.Execute(strSQL)                                            'Run the SQL statement

            'Update all cmbrf table variables with Rules and where BPS has a specific selection and Wildcard is "any"
            'to trump previous BPS and Wildcard "any"
            strSQL = "UPDATE " & MUTable & " " &
                     "INNER JOIN " & RulesTable & " ON " & MUTable & ".DIST = " & RulesTable & ".DIST " &
                     "AND " & MUTable & ".EVTR = " & RulesTable & ".EVT " &
                     "SET " & MUTable & ".New" & adjSET & " " &
                     "WHERE (([" & RulesTable & "]![OnOff]='On') AND " &
                     "(" & adjWHERE & "<>9999) And " &
                     "([" & MUTable & "]![EVCR] Between Int([" & RulesTable & "]![Cover_Low]) And Int([" & RulesTable & "]![Cover_High])) AND " &
                     "([" & MUTable & "]![EVHR] Between Int([" & RulesTable & "]![Height_Low]) And Int([" & RulesTable & "]![Height_High])) AND " &
                     "([" & RulesTable & "]![Wildcard]='any') AND " &
                     "([" & MUTable & "]![BPSRF] & """" =[" & RulesTable & "]![BPSRF]))"
            dbconn.Execute(strSQL)                                          'Run the SQL statement

            'Update all cmbrf table variables with Rules and where Wildcard has a specific selection and BPS is "any"
            'to trump previous BPS and Wildcard "any" and BPS specific selection and wildcard "any"
            strSQL = "UPDATE " & MUTable & " " &
                     "INNER JOIN " & RulesTable & " ON " & MUTable & ".DIST = " & RulesTable & ".DIST " &
                     "AND " & MUTable & ".EVTR = " & RulesTable & ".EVT " &
                     "SET " & MUTable & ".New" & adjSET & " " &
                     "WHERE (([" & RulesTable & "]![OnOff]='On') AND " &
                            "(" & adjWHERE & "<>9999) And " &
                            "([" & MUTable & "]![EVCR] Between Int([" & RulesTable & "]![Cover_Low]) And Int([" & RulesTable & "]![Cover_High])) AND " &
                            "([" & MUTable & "]![EVHR] Between Int([" & RulesTable & "]![Height_Low]) And Int([" & RulesTable & "]![Height_High])) AND " &
                            "([" & MUTable & "]![Wildcard]=[" & RulesTable & "]![Wildcard]) AND " &
                            "([" & RulesTable & "]![BPSRF]='any'))"
            dbconn.Execute(strSQL)                                          'Run the SQL statement

            'Update all cmbrf table variables with Rules and where BPS and Wildcard have specific selections to trump previous
            'BPS and Wildcard "any"
            strSQL = "UPDATE " & MUTable & " " &
                     "INNER JOIN " & RulesTable & " ON " & MUTable & ".DIST = " & RulesTable & ".DIST " &
                     "AND " & MUTable & ".EVTR = " & RulesTable & ".EVT " &
                     "SET " & MUTable & ".New" & adjSET & " " &
                     "WHERE (([" & RulesTable & "]![OnOff]='On') AND " &
                            "(" & adjWHERE & "<>9999) And " &
                            "([" & MUTable & "]![EVCR] Between Int([" & RulesTable & "]![Cover_Low]) And Int([" & RulesTable & "]![Cover_High])) AND " &
                            "([" & MUTable & "]![EVHR] Between Int([" & RulesTable & "]![Height_Low]) And Int([" & RulesTable & "]![Height_High])) AND " &
                            "([" & MUTable & "]![Wildcard]=[" & RulesTable & "]![Wildcard]) AND " &
                            "([" & MUTable & "]![BPSRF] & """" =[" & RulesTable & "]![BPSRF]))"
            dbconn.Execute(strSQL)                                          'Run the SQL statement

            'Set Canopy Fuel to obey Canopy Guide when assigning rule based canopy fuel
            If FuelName = "CCover" Or FuelName = "CHeight" Or FuelName = "CBH13mx10" Or
               FuelName = "CBH40mx10" Or FuelName = "CBD13x100" Or FuelName = "CBD40x100" Then

                strSQL = "UPDATE " & MUTable & " " &                         'CG=0 No canopy fuel
                         "SET New" & FuelName & " = 0 " &
                         "WHERE (NewCanopy=0) AND " &
                            "(New" & FuelName & "<>9999)"
                dbconn.Execute(strSQL)                                      'Run the SQL statement
            End If
            If FuelName = "CBH13mx10" Or FuelName = "CBH40mx10" Then
                strSQL = "UPDATE " & MUTable & " " &                         'CG=2 CBD set low and CBH set high
                         "SET New" & FuelName & " = 100 " &
                         "WHERE (NewCanopy=2) AND " &
                            "(New" & FuelName & "<>9999)"
                dbconn.Execute(strSQL)                                      'Run the SQL statement
            End If
            If FuelName = "CBD13x100" Or FuelName = "CBD40x100" Then
                strSQL = "UPDATE " & MUTable & " " &                         'CG=2 CBD set low and CBH set high
                         "SET New" & FuelName & " = 1 " &
                         "WHERE (NewCanopy=2) AND " &
                            "(New" & FuelName & "<>9999)"
                dbconn.Execute(strSQL)                                      'Run the SQL statement
            End If
            If FuelName = "CBD13x100" Or FuelName = "CBD40x100" Then
                strSQL = "UPDATE " & MUTable & " " &                         'CG=3 CBD set low 4/25/2019
                         "SET New" & FuelName & " = 5 " &
                         "WHERE (NewCanopy=3) AND " &
                            "(New" & FuelName & "<>9999)"
                dbconn.Execute(strSQL)                                      'Run the SQL statement
            End If

            If dbconn.State <> System.Data.ConnectionState.Closed Then                     'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> System.Data.ConnectionState.Closed Then                     'Database needs to be closed
                dbconn = Nothing
            End If

            MsgBox("Error in AssignValues - " & ex.Message)
        End Try
    End Sub

    Private Sub Assign9999(ByVal FuelName As String, ByVal MUName As String)
        Dim dbconn As New ADODB.Connection                                              'DB connection
        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim MUTable = MUName + "_CMB"
            strSQL = "UPDATE " & MUTable & " SET " & MUTable & ".New" & FuelName & " = 9999"

            dbconn.Execute(strSQL)                                                      'Run the SQL statement

            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If

            MsgBox("Error in Assign9999 - " & ex.Message)
        End Try
    End Sub

    Private Sub CalcCCandCH(ByVal MUName As String, ByVal CCMult As Double, ByVal CHMult As Double)
        Refresh()

        Const lowHeight As Integer = 18                                                     'Stores the lowValue for midpoint assignment 1.8m or 6ft

        Dim rs1 As New ADODB.Recordset                                                      'recordset for data
        Dim dbconn As New ADODB.Connection                                                  'DB connection

        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim MUTable = MUName + "_CMB"
            'Calculate CC and CH before calculating GLM

            If chkCCEquation.Checked = False Then                                                         'Update NewCCover to Midpoint
                cmdCreateGRID.Text = "CC mid_pt"
                Refresh()

                strSQL = "UPDATE " & MUTable & " INNER JOIN LUT_Cover " &
                                     "ON " & MUTable & ".EVCR = LUT_Cover.EVC SET " & MUTable & ".NewCCover = [LUT_Cover].[MidPoint] * " & CCMult & " " &
                                     "WHERE (((" & MUTable & ".NewCanopy = 1) Or (" & MUTable & ".NewCanopy = 2) " &
                                     "Or (" & MUTable & ".NewCanopy = 3)) And NewCCover = 9999)"
                dbconn.Execute(strSQL)
            Else                                                                            'Update NewCCover with equations
                cmdCreateGRID.Text = "CC Eqs"
                Refresh()

                'Get Disturbed NewCCover
                strSQL = "UPDATE ((" & MUTable & " INNER JOIN Master_Disturbance_Tbl On " &
                                         "(" & MUTable & ".DIST = Master_Disturbance_Tbl.HDist) And " &
                                         "(" & MUTable & ".EVTR = Master_Disturbance_Tbl.Tree_EVTs)) " &
                                         "INNER JOIN LUT_Cover On " & MUTable & ".EVCR = LUT_Cover.EVC) " &
                                         "INNER JOIN LUT_Height On " & MUTable & ".EVHR = LUT_Height.EVH " &
                                         "Set " & MUTable & ".NewCCover = " &
                                         "IIf(Round((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * [LUT_Height]![MidPoint]) + ([Master_Disturbance_Tbl]![CC_coef] * [LUT_Cover]![MidPoint])) * " & CCMult & ",0) < 0, 0, " &
                                         "IIf(Round((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * [LUT_Height]![MidPoint]) + ([Master_Disturbance_Tbl]![CC_coef] * [LUT_Cover]![MidPoint])) * " & CCMult & ",0) >= 95, 95, " &
                                         "Round((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * [LUT_Height]![MidPoint]) + ([Master_Disturbance_Tbl]![CC_coef] * [LUT_Cover]![MidPoint])) * " & CCMult & ",0))) " &
                                         "WHERE ((" & MUTable & ".DIST > 0) And (Master_Disturbance_Tbl.EVT_Fill <> 9999) And " &
                                         "(Master_Disturbance_Tbl.EV_Structure = 'Cover') AND (NewCCover = 9999) AND " &
                                         "((" & MUTable & ".NewCanopy = 1) Or (" & MUTable & ".NewCanopy = 2) Or (" & MUTable & ".NewCanopy = 3)))"
                dbconn.Execute(strSQL)
            End If

            If chkCHEquation.Checked = False Then                                                         'Update NewCHeight to Midpoint
                cmdCreateGRID.Text = "CH mid_pt"
                Refresh()
                strSQL = "UPDATE " & MUTable & " INNER JOIN LUT_Height " &
                                     "ON " & MUTable & ".EVHR = LUT_Height.EVH SET " & MUTable & ".NewCHeight = [LUT_Height].[MidPoint] * " & CHMult & " * 10 " &
                                     "WHERE (((" & MUTable & ".NewCanopy = 1) Or (" & MUTable & ".NewCanopy = 2) " &
                                     "Or (" & MUTable & ".NewCanopy = 3)) And NewCHeight = 9999)"
                dbconn.Execute(strSQL)
            Else                                                                            'Update NewCHeight with equations
                cmdCreateGRID.Text = "CH Eqs"
                Refresh()
                'Get Disturbed NewCHeight
                strSQL = "UPDATE ((" & MUTable & " INNER JOIN Master_Disturbance_Tbl ON " &
                                         "(" & MUTable & ".DIST = Master_Disturbance_Tbl.HDist) And " &
                                         "(" & MUTable & ".EVTR = Master_Disturbance_Tbl.Tree_EVTs)) " &
                                         "INNER JOIN LUT_Cover ON " & MUTable & ".EVCR = LUT_Cover.EVC) " &
                                         "INNER JOIN LUT_Height ON " & MUTable & ".EVHR = LUT_Height.EVH " &
                                         "SET " & MUTable & ".NewCHeight = " &
                                         "IIf((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * [LUT_Height]![MidPoint]) + ([Master_Disturbance_Tbl]![CC_coef] * [LUT_Cover]![MidPoint])) * " & CHMult & " < 0, 0, " &
                                         "IIf((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * [LUT_Height]![MidPoint]) + ([Master_Disturbance_Tbl]![CC_coef] * [LUT_Cover]![MidPoint])) * " & CHMult & " >= 50, 500, " &
                                         "Round((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * [LUT_Height]![MidPoint]) + ([Master_Disturbance_Tbl]![CC_coef] * [LUT_Cover]![MidPoint])) * " & CHMult & " * 10,0))) " &
                                         "WHERE (((" & MUTable & ".DIST)>0) And ((Master_Disturbance_Tbl.EVT_Fill)<>9999) And " &
                                         "((Master_Disturbance_Tbl.EV_Structure)='Height') AND " &
                                         "((" & MUTable & ".NewCanopy) = 1 Or (" & MUTable & ".NewCanopy = 2) Or (" & MUTable & ".NewCanopy = 3)))"
                dbconn.Execute(strSQL)
            End If

            'Bin disturbed NewCCover to 15,25,35,45,55,65,75,85,95 % If CC<10% goes to 0% CC OR If CH <= lowHeight variable CC goes to 0
            strSQL = "UPDATE " & MUTable & " SET " & MUTable & ".NewCCover = " &
                     "IIf(([" & MUTable & "]![NewCCover] < 10) Or ([" & MUTable & "]![NewCHeight] <= " & lowHeight & "), 0, Int([" & MUTable & "]![NewCCover]/10)*10+5) " &
                     "WHERE ((" & MUTable & ".NewCCover <> 9999) AND (" & MUTable & ".NewCHeight <> 9999))"
            dbconn.Execute(strSQL)

            'Bin NewCHeight in Mx10 and if CC is 0 CH gets 0
            strSQL = "UPDATE " & MUTable & " SET " & MUTable & ".NewCHeight = " &
                     "IIf(([" & MUTable & "]![NewCCover] = 0), 0, [" & MUTable & "]![NewCHeight]) " &
                     "WHERE ((" & MUTable & ".NewCCover <> 9999) AND (" & MUTable & ".NewCHeight <> 9999))"
            dbconn.Execute(strSQL)

            'Get midpoint values of tree heights
            strSQL = "SELECT LUT_Height.Lifeform, LUT_Height.MidPoint, LUT_Height.[Lower], LUT_Height.[Upper] " &
                     "FROM LUT_Height " &
                     "WHERE LUT_Height.Lifeform ='Tree'"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            'Assign midpoints
            Do Until rs1.EOF
                strSQL = "UPDATE " & MUTable & " SET " & MUTable & ".NewCHeight = " &
                         "IIf(([" & MUTable & "]![NewCHeight] >= " & rs1.Fields!Lower.Value * 10 & ") " &
                         "AND ([" & MUTable & "]![NewCHeight] < " & rs1.Fields!Upper.Value * 10 & "), " &
                         rs1.Fields!MidPoint.Value * 10 & ", " &
                         "[" & MUTable & "]![NewCHeight]) " &
                         "WHERE ((" & MUTable & ".NewCCover <> 9999) AND (" & MUTable & ".NewCHeight <> 9999))"
                dbconn.Execute(strSQL)
                rs1.MoveNext()
            Loop

            'Assign non disturbed CC
            strSQL = "UPDATE " & MUTable & " INNER JOIN LUT_Cover " &
                                 "ON " & MUTable & ".EVCR = LUT_Cover.EVC SET " & MUTable & ".NewCCover = [LUT_Cover].[MidPoint] * " & CCMult & " " &
                                 "WHERE (((" & MUTable & ".NewCanopy = 1) " &
                                 "Or (" & MUTable & ".NewCanopy = 2) " &
                                 "Or (" & MUTable & ".NewCanopy = 3)) " &
                                 "And (" & MUTable & ".DIST = 0) And (NewCCover = 9999))"
            dbconn.Execute(strSQL)

            'Assign non disturbed CH
            strSQL = "UPDATE " & MUTable & " INNER JOIN LUT_Height " &
                                 "ON " & MUTable & ".EVHR = LUT_Height.EVH SET " & MUTable & ".NewCHeight = [LUT_Height].[MidPoint] * " & CHMult & " * 10 " &
                                 "WHERE (((" & MUTable & ".NewCanopy = 1) " &
                                 "Or (" & MUTable & ".NewCanopy = 2) " &
                                 "Or (" & MUTable & ".NewCanopy = 3)) " &
                                 "And (" & MUTable & ".DIST = 0) And (NewCHeight = 9999))"
            dbconn.Execute(strSQL)

            Refresh()

            'Update combo table for everywhere that Canopy = 0
            strSQL = "Update " & MUTable & " " &
                 "SET NewCCover = NewCanopy, NewCHeight = NewCanopy " &
                 "WHERE (NewCanopy = 0) Or (NewCanopy = 9999)"
            dbconn.Execute(strSQL)                                                      'Run the SQL statement

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in CalcCCandCH- " & ex.Message)
        End Try
    End Sub

    Private Sub CBH_LM_EQs(ByVal FuelName As String, CBHMult As Double, ByVal MUName As String)
        Dim dbconn As New ADODB.Connection                                                  'DB connection

        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim MUTable = MUName + "_CMB"
            cmdCreateGRID.Text = "CBH EQs"
            Refresh()

            'Update combo table with CBH EQs
            strSQL = "UPDATE (" & MUTable & " INNER JOIN Master_Disturbance_Tbl ON " &
                            "(" & MUTable & ".DIST = Master_Disturbance_Tbl.HDist) And " &
                            "(" & MUTable & ".EVTR = Master_Disturbance_Tbl.Tree_EVTs)) " &
                            "SET " & MUTable & ".New" & FuelName & " = " &
                            "IIf((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * ([" & MUTable & "].[NewCHeight]/10)) + ([Master_Disturbance_Tbl]![CC_coef] * [" & MUTable & "].[NewCCover])) * " & CBHMult & " < 0.3, 3, " &
                            "IIf((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * ([" & MUTable & "].[NewCHeight]/10)) + ([Master_Disturbance_Tbl]![CC_coef] * [" & MUTable & "].[NewCCover])) * " & CBHMult & " >= 10, 100, " &
                            "Round((([Master_Disturbance_Tbl]![intercept]) + ([Master_Disturbance_Tbl]![HT_coef] * ([" & MUTable & "].[NewCHeight]/10)) + ([Master_Disturbance_Tbl]![CC_coef] * [" & MUTable & "].[NewCCover])) * " & CBHMult & " * 10,0))) " &
                            "WHERE (((Master_Disturbance_Tbl.EVT_Fill) <> 9999) And ((" & MUTable & ".NewCCover) <> 9999) And " &
                            "((Master_Disturbance_Tbl.EV_Structure) = 'CBH') AND ((New" & FuelName & ") = 9999) AND " &
                            "(((" & MUTable & ".NewCanopy) = 1) Or ((" & MUTable & ".NewCanopy) = 3)))"
            dbconn.Execute(strSQL)

            'Update combo table for everywhere that Canopy = 2 or CBH > 100
            strSQL = "Update " & MUTable & " " &
                 "SET New" & FuelName & " = 100 " &
                 "WHERE (NewCanopy = 2) Or (New" & FuelName & " > 100 And New" & FuelName & " <> 9999)"
            dbconn.Execute(strSQL)                                             'Run the SQL statement

            'Update combo table for everywhere that Canopy = 0
            strSQL = "Update " & MUTable & " " &
                 "SET New" & FuelName & " = NewCanopy " &
                 "WHERE (NewCanopy = 0) Or (NewCanopy = 9999)"
            dbconn.Execute(strSQL)                                             'Run the SQL statement

            'Update combo table for everywhere that NewCCover = 0
            strSQL = "Update " & MUTable & " " &
                 "SET New" & FuelName & " = NewCCover " &
                 "WHERE NewCCover = 0"
            dbconn.Execute(strSQL)                                             'Run the SQL statement

            'Update CBH = 2/3 the CH in combo table for everywhere that CBH > CH
            'CBH13
            strSQL = "Update " & MUTable & " " &
                    "SET New" & FuelName & " = Int(" & MUTable & "![NewCHeight]/10*0.6666*10) " &
                    "WHERE (((" & MUTable & ".New" & FuelName & ")<>9999 And " &
                    "(" & MUTable & ".New" & FuelName & ")>=[" & MUTable & "]![NewCHeight]))"
            dbconn.Execute(strSQL)                                             'Run the SQL statement

            'CBH40
            strSQL = "Update " & MUTable & " " &
                   "SET New" & FuelName & " = Int(" & MUTable & "![NewCHeight]/10*0.6666*10) " &
                   "WHERE (((" & MUTable & ".NewCBH40mx10)<>9999 And " &
                   "(" & MUTable & ".NewCBH40mx10)>=[" & MUTable & "]![NewCHeight]))"
            dbconn.Execute(strSQL)                                             'Run the SQL statement

            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If

            MsgBox("Error in CBH_LM_EQs - " & ex.Message)
        End Try
    End Sub

    Private Sub CalcCBDGLM(ByVal FuelName As String, CBDMult As Double, ByVal MUName As String)
        cmdCreateGRID.Text = "CBD GLM"
        Refresh()

        Dim lngEVT As Long                                              'Stores the EVT code value
        Dim lngCan As Long                                              'Stores the Canopy Mask value
        Dim dblCBD As Double                                            'Stores the CBD GLM predicted value
        Dim dblHgt As Double                                            'Stores the height value in meters
        Dim lngCov As Long                                              'Stores the cover value in percent cover
        Dim lngPJ As Long = 0                                           'Stores the PJ switch
        Dim lngSH1 As Long = 0                                          'Stores the Stand Height switch 1
        Dim lngSH2 As Long = 0                                          'Stores the Stand Height switch 2
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim MUTable = MUName + "_CMB"
            'Do GLM based method
            '***************************Calculate for non pj
            strSQL = "SELECT NewCCover, NewCHeight, NewCanopy " &
                    "FROM " & MUTable & " " &
                    "WHERE (NewCanopy = 1) Or (NewCanopy = 2) Or (NewCanopy = 3)" &
                    "Group By NewCCover, NewCHeight, NewCanopy " &
                    "HAVING NewCCover <> 9999"

            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            Do Until rs1.EOF
                dblHgt = rs1.Fields!NewCHeight.Value / 10   'Get the height in meters from the height code
                lngCov = rs1.Fields!NewCCover.Value 'Get the percent cover from EVCR code
                lngCan = rs1.Fields!NewCanopy.Value

                If lngCov = 0 Then
                    dblCBD = 0 'If CC = 0 then CBD = 0
                ElseIf lngCan = 2 Then
                    dblCBD = 1 'Canopy mask is 2 so CBD gets 0.012 or 1 in kg/m^*100
                ElseIf lngCan = 3 Then
                    dblCBD = 5 'Canopy mask is 3 so CBD gets 0.05 or 5 in kg/m^*100
                Else
                    'EXP(-2.4887057+(0.0335917*CC)+(-0.356861*SH1_)+(-0.6006381*SH2_)+(-1.10691*PJ)+(-0.0010804*CC*SH1_)+(-0.0018324*CC*SH2_))
                    'CBDpred = −2.489 + 0.034(CC)+−0.357(SH1)+−0.601(SH2)+−1.107(PJ)+−0.001(CC × SH1)+−0.002(CC × SH2)

                    'This tells the equation that none of these are pj or j
                    lngPJ = 1

                    If dblHgt < 15 Then
                        lngSH1 = 0
                        lngSH2 = 0
                    ElseIf dblHgt < 30 Then
                        lngSH1 = 1
                        lngSH2 = 0
                    ElseIf dblHgt >= 30 Then
                        lngSH1 = 0
                        lngSH2 = 1
                    End If
                    dblCBD = -2.4887057 + (0.0335917 * lngCov) + (-0.356861 * lngSH1) + -(0.6006381 * lngSH2) +
                            (-1.10691 * lngPJ) + (-0.0010804 * (lngCov * lngSH1)) + (-0.0018324 * (lngCov * lngSH2))
                    'The base natural logarithm raised to the dblCBD value multiply by 100 then integerize for kg/m^3 * 100
                    dblCBD = System.Math.Round(System.Math.Exp(dblCBD) * CBDMult, 2) * 100
                End If

                If dblCBD > 45 Then
                    dblCBD = 45
                End If

                'Set where values of FuelName
                strSQL = "Update " & MUTable & " " &
                         "Set New" & FuelName & " = " & dblCBD & " " &
                         "WHERE (NewCCover = " & rs1.Fields!NewCCover.Value & ") And " &
                         "(NewCHeight = " & rs1.Fields!NewCHeight.Value & ") And (New" & FuelName & " = 9999) And " &
                         "(NewCanopy = " & rs1.Fields!NewCanopy.Value & ")"
                dbconn.Execute(strSQL)                                                  'Run the SQL statement

                rs1.MoveNext()
            Loop

            '**********************Calculate for just pj or j (This is only for pj that have a canopy of 1 no need to look at canopy 2
            '**********************because it is already assigned during the non pj above

            strSQL = "Select " & MUTable & ".EVTR, " &
                     MUTable & ".NewCCover, " &
                     MUTable & ".NewCHeight, " &
                     MUTable & ".NewCanopy " &
                     "FROM(" & MUTable & ") " &
                     "GROUP BY " & MUTable & ".EVTR, " &
                     MUTable & ".NewCCover, " &
                     MUTable & ".NewCHeight, " &
                     MUTable & ".NewCanopy " &
                     "HAVING (((" & MUTable & ".EVTR)=2016) And ((" & MUTable & ".NewCanopy)=1)) " &
                     "Or (((" & MUTable & ".EVTR)=2017) And ((" & MUTable & ".NewCanopy)=1)) " &
                     "Or (((" & MUTable & ".EVTR)=2019) And ((" & MUTable & ".NewCanopy)=1)) " &
                     "Or (((" & MUTable & ".EVTR)=2025) And ((" & MUTable & ".NewCanopy)=1)) " &
                     "Or (((" & MUTable & ".EVTR)=2059) And ((" & MUTable & ".NewCanopy)=1)) " &
                     "Or (((" & MUTable & ".EVTR)=2115) And ((" & MUTable & ".NewCanopy)=1)) " &
                     "Or (((" & MUTable & ".EVTR)=2116) And ((" & MUTable & ".NewCanopy)=1)) " &
                     "Or (((" & MUTable & ".EVTR)=2119) And ((" & MUTable & ".NewCanopy)=1))"
            rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            Do Until rs2.EOF
                dblHgt = rs2.Fields!NewCHeight.Value / 10   'Get the height in meters from the height code
                lngCov = rs2.Fields!NewCCover.Value 'Get the percent cover from EVCR code
                lngEVT = rs2.Fields!EVTR.Value
                lngCan = rs2.Fields!NewCanopy.Value

                If lngCov = 0 Then
                    dblCBD = 0 'If CC = 0 then CBD = 0
                ElseIf lngCan = 2 Then
                    dblCBD = 1 'Canopy mask is 2 so CBD gets 0.012 or 1 in kg/m^*100
                ElseIf lngCan = 3 Then
                    dblCBD = 5 'Canopy mask is 3 so CBD gets 0.05 or 5 in kg/m^*100
                Else
                    'EXP(-2.4887057+(0.0335917*CC)+(-0.356861*SH1_)+(-0.6006381*SH2_)+(-1.10691*PJ)+(-0.0010804*CC*SH1_)+(-0.0018324*CC*SH2_))
                    'CBDpred = −2.489 + 0.034(CC)+−0.357(SH1)+−0.601(SH2)+−1.107(PJ)+−0.001(CC × SH1)+−0.002(CC × SH2)

                    lngPJ = 0  '0 Means it is a PJ and or J EVT all EVTs selected in the strSQL are pj or j

                    If dblHgt < 15 Then
                        lngSH1 = 0
                        lngSH2 = 0
                    ElseIf dblHgt < 30 Then
                        lngSH1 = 1
                        lngSH2 = 0
                    ElseIf dblHgt >= 30 Then
                        lngSH1 = 0
                        lngSH2 = 1
                    End If
                    dblCBD = -2.4887057 + (0.0335917 * lngCov) + (-0.356861 * lngSH1) + -(0.6006381 * lngSH2) +
                            (-1.10691 * lngPJ) + (-0.0010804 * (lngCov * lngSH1)) + (-0.0018324 * (lngCov * lngSH2))
                    'The base natural logarithm raised to the dblCBD value multiply by 100 then integerize for kg/m^3 * 100
                    dblCBD = System.Math.Round(System.Math.Exp(dblCBD) * CBDMult, 2) * 100
                End If
                If dblCBD > 45 Then
                    dblCBD = 45
                End If

                'Set where values of FuelName
                strSQL = "Update " & MUTable & " " &
                         "Set New" & FuelName & " = " & dblCBD & " " &
                         "WHERE (EVTR = " & lngEVT & ") And (NewCCover = " & rs2.Fields!NewCCover.Value & ") And " &
                         "(NewCHeight = " & rs2.Fields!NewCHeight.Value & ") And " &
                         "(NewCanopy = " & rs2.Fields!NewCanopy.Value & ")"
                dbconn.Execute(strSQL)                                                  'Run the SQL statement
                rs2.MoveNext()
            Loop

            'Update combo table for everywhere that Canopy = 0 and 9999
            strSQL = "Update " & MUTable & " " &
                 "Set New" & FuelName & " = NewCanopy " &
                 "WHERE (NewCanopy = 0) Or (NewCanopy = 9999)"
            dbconn.Execute(strSQL)                                                      'Run the SQL statement

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error In CalcCBDGLM " & ex.Message)
        End Try
    End Sub

    Private Sub AssignCC_CHProg(ByVal MUName As String, ByVal rulesG As String)

        'Assign Canopy first so we know where the canopy fuel belongs
        If chkGuide.Checked = False Then AssignValues("Canopy", MUName, rulesG)
        'Assign Cover where rules are not 9999 before CC calculation for everything else
        If chkCoverRules.Checked Then
            AssignValues("CCover", MUName, rulesG)
        Else
            Assign9999("CCover", MUName)
        End If

        'Assign Height where rules are not 9999 before CH calculation for everything else
        If chkHeightRules.Checked Then
            AssignValues("CHeight", MUName, rulesG)
        Else
            Assign9999("CHeight", MUName)
        End If

        'Calculate Canopy Cover
        CalcCCandCH(MUName, CDbl(txtCoverMult.Text), CDbl(txtHeightMult.Text))
    End Sub

    Private Function chkForTiff(ByVal SaveName As String) As String
        Try
            If rdoOutTiff.Checked Then
                Return SaveName & ".tif"
            Else
                Return SaveName
            End If
        Catch ex As Exception
            MsgBox("Check for tif " & ex.Message)
        End Try
    End Function

    Private Function PixLeftBehind(ByVal strName As String, ByVal MUName As String, ByVal RulesTable As String) As Boolean
        'Return FALSE is cancel is pushed, TRUE is continue is pushed

        Dim msgResult As String = ""
        Dim PLB = New frmPLB(strName)                                   'List box of pixels left behind
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data
        Dim rs3 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim MUTable = MUName + "_CMB"
            'Check for 9999 missing pixel assignments
            strSQL = "Select " & MUTable & ".EVTR, " & MUTable & ".DIST, Sum(" & MUTable & ".COUNT) As SumOfCOUNT " &
                     "FROM " & MUTable & " " &
                     "GROUP BY " & MUTable & ".EVTR, " & MUTable & ".DIST, " & MUTable & ".New" & strName & " " &
                     "HAVING (((" & MUTable & ".New" & strName & ")=9999))"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            Do While rs1.EOF = False
                PLB.AddPLB(rs1.Fields!EVTR.Value & "[" & rs1.Fields!DIST.Value & "]" & vbTab & "pixels left behind " &
                           rs1.Fields!SumOfCOUNT.Value)
                rs1.MoveNext()
            Loop

            '****Check for overlapping or missing rules
            '**** Get cmb pixel counts
            strSQL = "Select " & MUTable & ".EVTR, " & MUTable & ".DIST, Sum(" & MUTable & ".COUNT) As SumOfCOUNT " &
                     "FROM " & MUTable & " " &
                     "GROUP BY " & MUTable & ".EVTR, " & MUTable & ".DIST " &
                     "ORDER BY " & MUTable & ".EVTR, " & MUTable & ".DIST;"
            rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            '**** Get ruleset pixel counts
            strSQL = "Select " & RulesTable & ".EVT, " & RulesTable & ".DIST, Sum(IIf((IsNull([FuelDatabase1].[PixelCount]) " &
                     "Or ([FuelDatabase1].[PixelCount] = '')),-1, " &
                     "CLng([" & RulesTable & "].[PixelCount]))) AS PC, [" & RulesTable & "].OnOff " &
                     "FROM " & RulesTable & " " &
                     "GROUP BY " & RulesTable & ".EVT, " & RulesTable & ".DIST , [" & RulesTable & "].OnOff " &
                     "HAVING(((Sum(IIf((IsNull([FuelDatabase1].[PixelCount]) Or ([FuelDatabase1].[PixelCount] = '')), -1, " &
                     "CLng([" & RulesTable & "].[PixelCount])))) >= 0) AND (([" & RulesTable & "].OnOff)='On')) " &
                     "ORDER BY " & RulesTable & ".EVT, " & RulesTable & ".DIST;"
            rs3.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            '****Compare and report on overlapping or missing rules
            Do While rs2.EOF = False
                If rs3.EOF = False Then
                    'EVT and DIST numbers are the same and can be compared
                    If ((rs2.Fields!EVTR.Value = rs3.Fields!EVT.Value) And (rs2.Fields!DIST.Value = rs3.Fields!DIST.Value)) Then
                        If rs2.Fields!SumOfCOUNT.Value < rs3.Fields!PC.Value Then
                            PLB.AddPLB(rs2.Fields!EVTR.Value & "[" & rs2.Fields!DIST.Value & "]" & vbTab & "overlapping rules")
                        End If
                        rs2.MoveNext()
                        If (rs3.EOF = False) Then rs3.MoveNext()
                    Else 'EVT and DIST numbers are not the same so the ruleset is missing some rules
                        PLB.AddPLB(rs2.Fields!EVTR.Value & "[" & rs2.Fields!DIST.Value & "]" & vbTab & "no ruleset")
                        rs2.MoveNext() 'Only move cmb to the next ruleset to line up the next EVT and DIST numbers
                    End If
                Else 'No more rules associated with the cmb evts left so count them as missing
                    PLB.AddPLB(rs2.Fields!EVTR.Value & "[" & rs2.Fields!DIST.Value & "]" & vbTab & "no ruleset")
                    rs2.MoveNext() 'Only move cmb to the next ruleset to line up the next EVT and DIST numbers
                End If
            Loop

            If PLB.GetCount > 0 Then
                PLB.ShowDialog()                            'Show PLB List
                If PLB.GetAnswer = False Then
                    Return False                            'Do not continue
                Else
                    Return True                             'Pixels left bedind - continue anyway
                End If
            Else
                Return True                                 'No pixel left behind - continue
            End If

            PLB = Nothing

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in PixLeftBehind " & ex.Message)
        End Try
    End Function

    Private Async Sub SetRasterValues(ByVal FuelList As List(Of Fuel), ByVal MUName As String)
        Await QueuedTask.Run(
            Sub()
                Dim frmWork As New frmWorkStatus
                frmWork.Show()
                Try
                    frmWork.UpdateStatus("Drawing Paused")
                    gs_Map.GetMapPanes.First.MapView.DrawingPaused = True

                    frmWorkStatus.UpdateStatus("Clear table of contents.")
                    Dim container = gs_Map

                    'Added a check here for ArcPro 3.5 throws an error if the container does not have any layers to get
                    If container.GetLayersAsFlattenedList().Count() <> 0 Then container.RemoveLayers(container.GetLayersAsFlattenedList())
                    If container.GetStandaloneTablesAsFlattenedList().Count() <> 0 Then container.RemoveStandaloneTables(container.GetStandaloneTablesAsFlattenedList())

                    frmWork.UpdateStatus("Make " + MUName + "_LUT")
                    Dim muLayer As RasterLayer

                    Dim oWrite As New System.IO.StreamWriter(strProjectPath + "\MU\tempMULUT.csv")
                    Dim rs1 As New ADODB.Recordset                                  'recordset for data
                    Dim dbconn As New ADODB.Connection                              'DB connection
                    dbconn.ConnectionString = gs_DBConnection + strProjectPath + "\" + gs_LFTFCDBName
                    dbconn.Open()
                    Dim strSQL As String 'SQL variable for this module

                    strSQL = "SELECT " + MUName + "_CMB.VALUE, " + MUName + "_CMB.NewFBFM13, " + MUName + "_CMB.NewFBFM40, " +
                             MUName + "_CMB.NewCanFM, " + MUName + "_CMB.NewFCCS, " + MUName + "_CMB.NewFLM, " +
                             MUName + "_CMB.NewCCover, " + MUName + "_CMB.NewCHeight, " + MUName + "_CMB.NewCBH13mx10, " +
                             MUName + "_CMB.NewCBH40mx10, " + MUName + "_CMB.NewCBD13x100, " + MUName + "_CMB.NewCBD40x100, " +
                             MUName + "_CMB.NewCanopy FROM " + MUName + "_CMB"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    'Field Headings
                    oWrite.WriteLine("VALUE, NewFBFM13, NewFBFM40, NewCanFM, NewFCCS, NewFLM, NewCCover, NewCHeight, NewCBH13mx10, " &
                                     "NewCBH40mx10, NewCBD13x100,NewCBD40x100, NewCanopy")

                    'Make lookup csv
                    Do Until rs1.EOF
                        oWrite.WriteLine(rs1.Fields!VALUE.Value & "," & rs1.Fields!NewFBFM13.Value & "," & rs1.Fields!NewFBFM40.Value & "," &
                                         rs1.Fields!NewCanFM.Value & "," & rs1.Fields!NewFCCS.Value & "," & +rs1.Fields!NewFLM.Value & "," &
                                         rs1.Fields!NewCCover.Value & "," & rs1.Fields!NewCHeight.Value & "," & rs1.Fields!NewCBH13mx10.Value & "," &
                                         rs1.Fields!NewCBH40mx10.Value & "," & rs1.Fields!NewCBD13x100.Value & "," & rs1.Fields!NewCBD40x100.Value & "," &
                                         rs1.Fields!NewCanopy.Value)
                        rs1.MoveNext()
                    Loop
                    oWrite.Close()

                    'Export to dbf for OIDs and faster prfrmWork.UpdateStatus("Make DBF LUT")
                    Dim val_array = Geoprocessing.MakeValueArray(strProjectPath + "\MU\tempMULUT.csv", strProjectPath + "\MU\tempMULUT.dbf")
                    Geoprocessing.ExecuteToolAsync("conversion.ExportTable", val_array)
                    Dim LUT_DBF = container.GetStandaloneTablesAsFlattenedList.OfType(Of StandaloneTable).First
                    'Dim LUT_DBF = strProjectPath + "\MU\tempMULUT.dbf"

                    Dim rasMU As String
                    If ItemFactory.Instance.CanGetDataset(ItemFactory.Instance.Create(strProjectPath + "\MU\" + MUName + ".tif")) Then
                        rasMU = strProjectPath + "\MU\" + MUName + ".tif"
                    Else
                        rasMU = strProjectPath + "\MU\" + MUName
                    End If

                    'Make raster layer
                    frmWork.UpdateStatus("MakeRasterLayer -" & MUName)
                    val_array = Geoprocessing.MakeValueArray(rasMU, MUName)
                    Geoprocessing.ExecuteToolAsync("management.MakeRasterLayer", val_array)

                    'Execute Add Join to MU raster
                    frmWork.UpdateStatus("AddJoin")

                    'val_array = Geoprocessing.MakeValueArray(layerRasMU, "VALUE", LUT_DBF, "VALUE", "KEEP_All", "NO_INDEX_JOIN_FIELDS")
                    val_array = Geoprocessing.MakeValueArray(MUName, "VALUE", LUT_DBF, "VALUE")
                    Geoprocessing.ExecuteToolAsync("management.AddJoin", val_array)

                    'Get layer once it is made so it can be removed later
                    muLayer = container.GetLayersAsFlattenedList().OfType(Of RasterLayer).First

                    Dim strRemapValue As String = ""
                    Dim strSaveAs As String = ""

                    'Make fuel from the list
                    For Each fuel In FuelList
                        With fuel
                            strRemapValue = .FuelType
                            strSaveAs = .SaveAs
                        End With

                        'Lookup 
                        frmWork.UpdateStatus("Lookup - " + strRemapValue)

                        'Dim inLayer = MUName + "_Layer"
                        Dim inLayer = MUName
                        'When table is joined the field gets limited to 10 characters
                        val_array = Geoprocessing.MakeValueArray(inLayer, Strings.Left("New" + strRemapValue, 10), "tempFuel")
                        Dim env_array = Geoprocessing.MakeEnvironmentArray(overwriteoutput:=True)
                        Geoprocessing.ExecuteToolAsync("sa.Lookup", val_array, env_array)

                        'Get layer once it is made so it can be removed later
                        Dim tempLayer = container.GetLayersAsFlattenedList().OfType(Of RasterLayer).First

                        'FCCS needs to be a 32-bit data type for its large values
                        'else use 16-bit for all others
                        If strRemapValue = "FCCS" Then
                            'Convert grid to signed 32 bit and NoData = -9999
                            frmWork.UpdateStatus(strRemapValue + "- Set Nodata -9999")

                            Dim outRaster = strProjectPath + "\Output\" + strSaveAs
                            inLayer = "tempFuel"
                            val_array = Geoprocessing.MakeValueArray(inLayer, outRaster, Nothing, Nothing, "-9999",
                                                                 Nothing, Nothing, "32_BIT_SIGNED")
                            env_array = Geoprocessing.MakeEnvironmentArray(overwriteoutput:=False)
                            Geoprocessing.ExecuteToolAsync("management.CopyRaster", val_array, env_array)
                        Else
                            'Convert grid to signed 16 bit and NoData = -9999
                            frmWork.UpdateStatus(strRemapValue + "- Set Nodata -9999")

                            Dim outRaster = strProjectPath + "\Output\" + strSaveAs
                            inLayer = "tempFuel"
                            val_array = Geoprocessing.MakeValueArray(inLayer, outRaster, Nothing, Nothing, "-9999",
                                                                 Nothing, Nothing, "16_BIT_SIGNED")
                            env_array = Geoprocessing.MakeEnvironmentArray(overwriteoutput:=False)
                            Geoprocessing.ExecuteToolAsync("management.CopyRaster", val_array, env_array)
                        End If

                        'Add colormap, except for FLMs (FLMs do not have a color file)
                        If strRemapValue <> "FLM" Then
                            frmWork.UpdateStatus(strRemapValue + "- Add Colormap")
                            'Get layer once it Is made so LF fields can be added
                            Dim FuelLayer = container.GetLayersAsFlattenedList().OfType(Of RasterLayer).First
                            val_array = Geoprocessing.MakeValueArray(FuelLayer, Nothing, gs_Install_Path + "\" + strRemapValue + "_color.clr")

                            Geoprocessing.ExecuteToolAsync("management.AddColormap", val_array)

                            'Remove FuelLayer
                            container.RemoveLayer(FuelLayer)
                        End If
                        'Remove tempFuel
                        container.RemoveLayer(tempLayer)
                    Next
                    container.RemoveStandaloneTable(LUT_DBF) 'Remove the dbf lut
                    container.RemoveLayer(muLayer)      'Remove The make raster layer of MU

                    MessageBox.Show("Finished! Rasters are in:" + strProjectPath + "\Output")
                Catch ex As Exception
                    Dim errMessageString As String = ""
                    errMessageString = errMessageString & ex.Message
                    MsgBox("Error in SetRasterValues - " & errMessageString & vbCrLf &
                           "Possible solutions " & vbCrLf &
                           "-> Make sure there is an MU raster " & vbCrLf &
                           "   in the MU folder for that MU" & vbCrLf &
                           "-> Make sure that LFTFC_Pro is the " & vbCrLf &
                           "   selected map view tab")
                End Try
                frmWork.Close()
            End Sub)
        'Unpause active view
        gs_Map.GetMapPanes.First.MapView.DrawingPaused = False
    End Sub
End Class

