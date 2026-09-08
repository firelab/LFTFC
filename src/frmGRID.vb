Imports System.Data
Imports System.Data.SQLite
Imports System.IO
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

    'Access UPDATE...INNER JOIN has no SQLite equivalent — workaround as a correlated
    'subquery for the value plus a matching EXISTS to restrict which rows are touched.

    Public Class Fuel
        Public Property FuelType As String
        Public Property SaveAs As String
    End Class

    Private Function SetFuelDatabase(DBName As String, MUName As String) As String

        Dim dbPath As String = System.IO.Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"
        Dim strSQL As String = ""

        Try
            'Check for too many MU fuel grids
            If Strings.Right(DBName, 1) > 3 Then
                MsgBox("Too many different MU Fuel Grids being created at the same time." & vbCrLf &
                   "Wait for some Fuel Grids to finish and try again.")
            Else
                'SQLite equivalent of SELECT * INTO NewTable FROM OldTable
                strSQL =
                "CREATE TABLE " & DBName & " AS " &
                "SELECT * FROM " & MUName & "_Rulesets"

                Using conn As New SQLite.SQLiteConnection(connString)
                    conn.Open()

                    Using tx As SQLite.SQLiteTransaction = conn.BeginTransaction()
                        Using cmd As New SQLite.SQLiteCommand(strSQL, conn, tx)
                            cmd.ExecuteNonQuery()
                        End Using
                        tx.Commit()
                    End Using
                End Using
            End If

        Catch ex As Exception
            'Retry with incremented last digit, identical logic to original
            Dim newDBName As String =
            Strings.Left(DBName, 12) & (CInt(Strings.Right(DBName, 1)) + 1).ToString()

            DBName = SetFuelDatabase(newDBName, MUName)
        End Try

        Return DBName

    End Function

    Private Sub DeleteFuelDatabase(DBName As String)

        Dim dbPath As String = System.IO.Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"
        Dim strSQL As String = ""

        Try
            'Original rule: delete DBName, DBName+1, DBName+2, DBName+3 (Right() < 4)
            If CInt(Strings.Right(DBName, 1)) < 4 Then

                strSQL = "DROP TABLE IF EXISTS " & DBName

                Using conn As New SQLite.SQLiteConnection(connString)
                    conn.Open()

                    Using cmd As New SQLite.SQLiteCommand(strSQL, conn)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                'Increment DBName suffix just like original Access version
                Dim nextSuffix As Integer = CInt(Strings.Right(DBName, 1)) + 1
                Dim nextDBName As String = Strings.Left(DBName, 12) & nextSuffix.ToString()

                DeleteFuelDatabase(nextDBName)
            End If

        Catch ex As Exception
            'SQLite throws if table doesn't exist beyond IF EXISTS handling.
            'We follow the original rule: do nothing.
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

    '---------------------------------------------------------
    ' MODEL: FuelRow (represents one row in MUTable)
    '---------------------------------------------------------
    Public Class FuelRow
        Public Property VALUE As Integer
        Public Property COUNT As Integer
        Public Property EVTR As Integer
        Public Property DIST As Integer
        Public Property EVCR As Integer
        Public Property EVHR As Integer
        Public Property BPSRF As Integer
        Public Property Wildcard As String

        Public Property NewFBFM13 As Integer
        Public Property NewFBFM40 As Integer
        Public Property NewCanFM As Integer
        Public Property NewFCCS As Integer
        Public Property NewFLM As Integer
        Public Property NewCCover As Integer
        Public Property NewCHeight As Integer
        Public Property NewCBH13mx10 As Integer
        Public Property NewCBH40mx10 As Integer
        Public Property NewCBD13x100 As Integer
        Public Property NewCBD40x100 As Integer
        Public Property NewCanopy As Integer
    End Class


    '---------------------------------------------------------
    ' MODEL: RuleRow (represents one row in RulesTable)
    '---------------------------------------------------------
    Public Class RuleRow
        Public Property EVT As Integer
        Public Property DIST As Integer
        Public Property Cover_Low As Integer
        Public Property Cover_High As Integer
        Public Property Height_Low As Integer
        Public Property Height_High As Integer
        Public Property BPSRF As String
        Public Property Wildcard As String

        Public Property FBFM13 As Integer
        Public Property FBFM40 As String
        Public Property CanFM As String
        Public Property FCCS As Integer
        Public Property FLM As Integer
        Public Property CCover As Integer
        Public Property CHeight As Integer
        Public Property CBD13x100 As Integer
        Public Property CBD40x100 As Integer
        Public Property CBH13mx10 As Integer
        Public Property CBH40mx10 As Integer
        Public Property Canopy As Integer
        Public Property OnOff As String
    End Class

    Private Sub AssignValues(ByVal FuelName As String, ByVal MUName As String, ByVal RulesTable As String)
        Dim dbPath As String = strProjectPath & "\" & gs_LFTFCSQliteName
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Try
            Dim MUTable As String = MUName + "_CMB"
            Dim defVal As String = DEFAULT_FUEL_VAL.ToString()

            'Value assigned to the MU column, and the guard that skips default-valued rules
            Dim valueExpr As String
            Dim guardExpr As String
            If FuelName = "FBFM40" OrElse FuelName = "CanFM" Then
                'Access Right(x,3) -> substr(x,-3); Int() -> CAST AS INTEGER
                valueExpr = "CAST(substr(R." & FuelName & ", -3) AS INTEGER)"
                guardExpr = "CASE WHEN R." & FuelName & " = '" & defVal & "' THEN " & defVal &
                            " ELSE CAST(substr(R." & FuelName & ", -3) AS INTEGER) END <> " & defVal
            Else
                valueExpr = "R." & FuelName
                guardExpr = "R." & FuelName & " <> " & defVal
            End If

            Using conn As New SQLiteConnection(connString)
                conn.Open()

                'All four tiers must succeed or none of them — later tiers trump earlier ones
                Using tx As SQLiteTransaction = conn.BeginTransaction()

                    'Reset selected fuel in cmbrf table variable to the default
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " SET New" & FuelName & " = " & defVal)

                    'BPS and Wildcard are both "any"
                    ExecuteUpdate(conn, tx,
                        BuildTierSql(MUTable, RulesTable, FuelName, valueExpr, guardExpr,
                                     "R.BPSRF = 'any' AND R.Wildcard = 'any'"))

                    'BPS has a specific selection and Wildcard is "any" — trumps the previous tier
                    ExecuteUpdate(conn, tx,
                        BuildTierSql(MUTable, RulesTable, FuelName, valueExpr, guardExpr,
                                     "R.Wildcard = 'any' AND CAST(M.BPSRF AS TEXT) = R.BPSRF"))

                    'Wildcard has a specific selection and BPS is "any" — trumps both previous tiers
                    ExecuteUpdate(conn, tx,
                        BuildTierSql(MUTable, RulesTable, FuelName, valueExpr, guardExpr,
                                     "M.WILDCARD = R.Wildcard AND R.BPSRF = 'any'"))

                    'BPS and Wildcard both have specific selections — trumps everything above
                    ExecuteUpdate(conn, tx,
                        BuildTierSql(MUTable, RulesTable, FuelName, valueExpr, guardExpr,
                                     "M.WILDCARD = R.Wildcard AND CAST(M.BPSRF AS TEXT) = R.BPSRF"))

                    'Set Canopy Fuel to obey Canopy Guide when assigning rule based canopy fuel
                    If FuelName = "CCover" Or FuelName = "CHeight" Or FuelName = "CBH13mx10" Or
                       FuelName = "CBH40mx10" Or FuelName = "CBD13x100" Or FuelName = "CBD40x100" Then
                        'CG=0 No canopy fuel
                        ExecuteUpdate(conn, tx, BuildCanopyGuideSql(MUTable, FuelName, 0, 0, defVal))
                    End If
                    If FuelName = "CBH13mx10" Or FuelName = "CBH40mx10" Then
                        'CG=2 CBD set low and CBH set high
                        ExecuteUpdate(conn, tx, BuildCanopyGuideSql(MUTable, FuelName, 2, 100, defVal))
                    End If
                    If FuelName = "CBD13x100" Or FuelName = "CBD40x100" Then
                        'CG=2 CBD set low and CBH set high
                        ExecuteUpdate(conn, tx, BuildCanopyGuideSql(MUTable, FuelName, 2, 1, defVal))
                    End If
                    If FuelName = "CBD13x100" Or FuelName = "CBD40x100" Then
                        'CG=3 CBD set low 4/25/2019
                        ExecuteUpdate(conn, tx, BuildCanopyGuideSql(MUTable, FuelName, 3, 5, defVal))
                    End If

                    tx.Commit()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error in AssignValues - " & ex.Message)
        End Try
    End Sub
    Private Sub ExecuteUpdate(conn As SQLiteConnection, tx As SQLiteTransaction, sql As String)
        strSQL = sql
        Using cmd As New SQLiteCommand(sql, conn, tx)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function BuildTierSql(MUTable As String, RulesTable As String, FuelName As String,
                                  valueExpr As String, guardExpr As String,
                                  tierCondition As String) As String
        Dim match As String =
            "FROM " & RulesTable & " R " &
            "WHERE M.DIST = R.DIST " &
            "AND M.EVTR = R.EVT " &
            "AND R.OnOff = 'On' " &
            "AND " & guardExpr & " " &
            "AND M.EVCR BETWEEN CAST(R.Cover_Low AS INTEGER) AND CAST(R.Cover_High AS INTEGER) " &
            "AND M.EVHR BETWEEN CAST(R.Height_Low AS INTEGER) AND CAST(R.Height_High AS INTEGER) " &
            "AND " & tierCondition

        Return "UPDATE " & MUTable & " AS M " &
               "SET New" & FuelName & " = (SELECT " & valueExpr & " " & match & " LIMIT 1) " &
               "WHERE EXISTS (SELECT 1 " & match & ")"
    End Function

    Private Function BuildCanopyGuideSql(MUTable As String, FuelName As String,
                                         canopyGuide As Integer, setValue As Integer,
                                         defVal As String) As String
        Return "UPDATE " & MUTable & " " &
               "SET New" & FuelName & " = " & setValue & " " &
               "WHERE NewCanopy = " & canopyGuide & " " &
               "AND New" & FuelName & " <> " & defVal
    End Function

    Private Sub Assign9999(FuelName As String, MUName As String)

        Dim dbPath As String = System.IO.Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Dim MUTable As String = MUName & "_CMB"
        Dim fieldName As String = "New" & FuelName

        Dim sql As String =
        "UPDATE " & MUTable & " " &
        "SET " & fieldName & " = @defaultFuel"

        Try
            Using conn As New SQLiteConnection(connString)
                conn.Open()

                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@defaultFuel", DEFAULT_FUEL_VAL)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Error in Assign9999 - " & ex.Message)
        End Try

    End Sub

    Private Sub CalcCCandCH(ByVal MUName As String, ByVal CCMult As Double, ByVal CHMult As Double)
        Refresh()

        Const lowHeight As Integer = 18     'Stores the lowValue for midpoint assignment 1.8m or 6ft

        Dim dbPath As String = strProjectPath & "\" & gs_LFTFCSQliteName
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Try
            Dim MUTable As String = MUName + "_CMB"
            Dim defVal As String = DEFAULT_FUEL_VAL.ToString()
            Dim canopySet As String = "M.NewCanopy IN (1, 2, 3)"

            Using conn As New SQLiteConnection(connString)
                conn.Open()

                'Read the tree height midpoints up front — no reader can be open while updates run
                Dim treeBands As List(Of HeightBand) = LoadTreeHeightBands(conn)

                Using tx As SQLiteTransaction = conn.BeginTransaction()

                    'Calculate CC and CH before calculating GLM
                    If chkCCEquation.Checked = False Then        'Update NewCCover to Midpoint
                        cmdCreateGRID.Text = "CC mid_pt"
                        Refresh()

                        ExecuteUpdate(conn, tx, BuildLutJoinUpdate(
                            MUTable, "NewCCover", "LUT_Cover", "EVCR", "EVC",
                            "L.MidPoint * " & Dbl(CCMult),
                            canopySet & " AND M.NewCCover = " & defVal))
                    Else                                        'Update NewCCover with equations
                        cmdCreateGRID.Text = "CC Eqs"
                        Refresh()

                        'Get Disturbed NewCCover
                        Dim eq As String = DisturbanceEquation(CCMult)
                        ExecuteUpdate(conn, tx, BuildDisturbanceUpdate(
                            MUTable, "NewCCover",
                            "CASE WHEN ROUND(" & eq & ") < 0 THEN 0 " &
                            "WHEN ROUND(" & eq & ") >= 95 THEN 95 " &
                            "ELSE ROUND(" & eq & ") END",
                            "Cover", defVal, canopySet & " AND M.NewCCover = " & defVal))
                    End If

                    If chkCHEquation.Checked = False Then        'Update NewCHeight to Midpoint
                        cmdCreateGRID.Text = "CH mid_pt"
                        Refresh()

                        ExecuteUpdate(conn, tx, BuildLutJoinUpdate(
                            MUTable, "NewCHeight", "LUT_Height", "EVHR", "EVH",
                            "L.MidPoint * " & Dbl(CHMult) & " * 10",
                            canopySet & " AND M.NewCHeight = " & defVal))
                    Else                                        'Update NewCHeight with equations
                        cmdCreateGRID.Text = "CH Eqs"
                        Refresh()

                        'Get Disturbed NewCHeight
                        Dim eq As String = DisturbanceEquation(CHMult)
                        ExecuteUpdate(conn, tx, BuildDisturbanceUpdate(
                            MUTable, "NewCHeight",
                            "CASE WHEN " & eq & " < 0 THEN 0 " &
                            "WHEN " & eq & " >= 50 THEN 500 " &
                            "ELSE ROUND((" & eq & ") * 10) END",
                            "Height", defVal, canopySet))
                    End If

                    'Bin disturbed NewCCover to 15,25,35,45,55,65,75,85,95 % If CC<10% goes to 0% CC OR If CH <= lowHeight variable CC goes to 0
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " SET NewCCover = " &
                        "CASE WHEN NewCCover < 10 OR NewCHeight <= " & lowHeight & " THEN 0 " &
                        "ELSE CAST(NewCCover / 10 AS INTEGER) * 10 + 5 END " &
                        "WHERE NewCCover <> " & defVal & " AND NewCHeight <> " & defVal)

                    'Bin NewCHeight in Mx10 and if CC is 0 CH gets 0
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " SET NewCHeight = " &
                        "CASE WHEN NewCCover = 0 THEN 0 ELSE NewCHeight END " &
                        "WHERE NewCCover <> " & defVal & " AND NewCHeight <> " & defVal)

                    'Assign midpoints
                    Using cmd As New SQLiteCommand(
                        "UPDATE " & MUTable & " SET NewCHeight = " &
                        "CASE WHEN NewCHeight >= @lower AND NewCHeight < @upper " &
                        "THEN @mid ELSE NewCHeight END " &
                        "WHERE NewCCover <> " & defVal & " AND NewCHeight <> " & defVal, conn, tx)

                        cmd.Parameters.Add("@lower", DbType.Double)
                        cmd.Parameters.Add("@upper", DbType.Double)
                        cmd.Parameters.Add("@mid", DbType.Double)

                        For Each b As HeightBand In treeBands
                            cmd.Parameters("@lower").Value = b.Lower * 10
                            cmd.Parameters("@upper").Value = b.Upper * 10
                            cmd.Parameters("@mid").Value = b.MidPoint * 10
                            cmd.ExecuteNonQuery()
                        Next
                    End Using

                    'Assign non disturbed CC
                    ExecuteUpdate(conn, tx, BuildLutJoinUpdate(
                        MUTable, "NewCCover", "LUT_Cover", "EVCR", "EVC",
                        "L.MidPoint * " & Dbl(CCMult),
                        canopySet & " AND M.DIST = 0 AND M.NewCCover = " & defVal))

                    'Assign non disturbed CH
                    ExecuteUpdate(conn, tx, BuildLutJoinUpdate(
                        MUTable, "NewCHeight", "LUT_Height", "EVHR", "EVH",
                        "L.MidPoint * " & Dbl(CHMult) & " * 10",
                        canopySet & " AND M.DIST = 0 AND M.NewCHeight = " & defVal))

                    Refresh()

                    'Update combo table for everywhere that Canopy = 0
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " " &
                        "SET NewCCover = NewCanopy, NewCHeight = NewCanopy " &
                        "WHERE NewCanopy = 0 OR NewCanopy = " & defVal)

                    tx.Commit()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error in CalcCCandCH- " & ex.Message)
        End Try
    End Sub

    Private Structure HeightBand
        Public MidPoint As Double
        Public Lower As Double
        Public Upper As Double
    End Structure

    'Forces invariant formatting so a comma decimal separator can't corrupt the SQL
    Private Function Dbl(value As Double) As String
        Return value.ToString(System.Globalization.CultureInfo.InvariantCulture)
    End Function

    'The regression shared by the CC and CH equation branches
    Private Function DisturbanceEquation(mult As Double) As String
        Return "(D.intercept + (D.HT_coef * H.MidPoint) + (D.CC_coef * C.MidPoint)) * " & Dbl(mult)
    End Function

    'Access UPDATE...INNER JOIN <lookup> has no SQLite equivalent
    Private Function BuildLutJoinUpdate(MUTable As String, targetCol As String,
                                        lutTable As String, muKey As String, lutKey As String,
                                        valueExpr As String, extraWhere As String) As String
        Dim match As String = "FROM " & lutTable & " L WHERE L." & lutKey & " = M." & muKey

        Return "UPDATE " & MUTable & " AS M " &
               "SET " & targetCol & " = (SELECT " & valueExpr & " " & match & " LIMIT 1) " &
               "WHERE EXISTS (SELECT 1 " & match & ") AND " & extraWhere
    End Function

    'The three-way join used by both equation branches
    Private Function BuildDisturbanceUpdate(MUTable As String, targetCol As String,
                                            valueExpr As String, evStructure As String,
                                            defVal As String, extraWhere As String) As String
        Dim match As String =
            "FROM Master_Disturbance_Tbl D " &
            "INNER JOIN LUT_Cover C ON C.EVC = M.EVCR " &
            "INNER JOIN LUT_Height H ON H.EVH = M.EVHR " &
            "WHERE D.HDist = M.DIST AND D.Tree_EVTs = M.EVTR " &
            "AND D.EVT_Fill <> " & defVal & " " &
            "AND D.EV_Structure = '" & evStructure & "'"

        Return "UPDATE " & MUTable & " AS M " &
               "SET " & targetCol & " = (SELECT " & valueExpr & " " & match & " LIMIT 1) " &
               "WHERE EXISTS (SELECT 1 " & match & ") " &
               "AND M.DIST > 0 AND " & extraWhere
    End Function

    Private Function LoadTreeHeightBands(conn As SQLiteConnection) As List(Of HeightBand)
        Dim bands As New List(Of HeightBand)

        'Get midpoint values of tree heights
        Dim sql As String =
            "SELECT MidPoint, ""Lower"", ""Upper"" " &
            "FROM LUT_Height " &
            "WHERE Lifeform = 'Tree'"

        strSQL = sql
        Using cmd As New SQLiteCommand(sql, conn)
            Using reader As SQLiteDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim b As New HeightBand
                    b.MidPoint = CDbl(reader("MidPoint"))
                    b.Lower = CDbl(reader("Lower"))
                    b.Upper = CDbl(reader("Upper"))
                    bands.Add(b)
                End While
            End Using
        End Using

        Return bands
    End Function

    Private Sub CBH_LM_EQs(ByVal FuelName As String, CBHMult As Double, ByVal MUName As String)
        Dim dbPath As String = strProjectPath & "\" & gs_LFTFCSQliteName
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Try
            Dim MUTable As String = MUName + "_CMB"
            Dim defVal As String = DEFAULT_FUEL_VAL.ToString()

            cmdCreateGRID.Text = "CBH EQs"
            Refresh()

            'The regression, evaluated against the MU row's current CH and CC
            Dim eq As String = "(D.intercept + (D.HT_coef * (M.NewCHeight / 10.0)) + " &
                               "(D.CC_coef * M.NewCCover)) * " & Dbl(CBHMult)

            Dim match As String =
                "FROM Master_Disturbance_Tbl D " &
                "WHERE D.HDist = M.DIST AND D.Tree_EVTs = M.EVTR " &
                "AND D.EVT_Fill <> " & defVal & " " &
                "AND D.EV_Structure = 'CBH'"

            Using conn As New SQLiteConnection(connString)
                conn.Open()

                Using tx As SQLiteTransaction = conn.BeginTransaction()

                    'Update combo table with CBH EQs
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " AS M " &
                        "SET New" & FuelName & " = (SELECT " &
                            "CASE WHEN " & eq & " < 0.3 THEN 3 " &
                            "WHEN " & eq & " >= 10 THEN 100 " &
                            "ELSE ROUND((" & eq & ") * 10) END " &
                            match & " LIMIT 1) " &
                        "WHERE EXISTS (SELECT 1 " & match & ") " &
                        "AND M.NewCCover <> " & defVal & " " &
                        "AND M.New" & FuelName & " = " & defVal & " " &
                        "AND M.NewCanopy IN (1, 3)")

                    'Update combo table for everywhere that Canopy = 2 or CBH > 100
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " " &
                        "SET New" & FuelName & " = 100 " &
                        "WHERE NewCanopy = 2 " &
                        "OR (New" & FuelName & " > 100 AND New" & FuelName & " <> " & defVal & ")")

                    'Update combo table for everywhere that Canopy = 0
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " " &
                        "SET New" & FuelName & " = NewCanopy " &
                        "WHERE NewCanopy = 0 OR NewCanopy = " & defVal)

                    'Update combo table for everywhere that NewCCover = 0
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " " &
                        "SET New" & FuelName & " = NewCCover " &
                        "WHERE NewCCover = 0")

                    'Update CBH = 2/3 the CH in combo table for everywhere that CBH > CH
                    'CBH13 or CBH40
                    ExecuteUpdate(conn, tx,
                        "UPDATE " & MUTable & " " &
                        "SET New" & FuelName & " = CAST(NewCHeight / 10.0 * 0.6666 * 10 AS INTEGER) " &
                        "WHERE New" & FuelName & " <> " & defVal & " " &
                        "AND New" & FuelName & " >= NewCHeight")

                    tx.Commit()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error in CBH_LM_EQs - " & ex.Message)
        End Try
    End Sub

    Private Structure CbdGroup
        Public EVTR As Long
        Public NewCCover As Long
        Public NewCHeight As Long
        Public NewCanopy As Long
    End Structure

    'Shared by both passes. isPJ = False sets the PJ switch to 1 (not PJ/J),
    'isPJ = True sets it to 0 — matching the original's lngPJ assignments.
    Private Function ComputeCBD(lngCov As Long, dblHgt As Double, lngCan As Long,
                                isPJ As Boolean, CBDMult As Double) As Double
        Dim dblCBD As Double

        If lngCov = 0 Then
            dblCBD = 0 'If CC = 0 then CBD = 0
        ElseIf lngCan = 2 Then
            dblCBD = 1 'Canopy mask is 2 so CBD gets 0.012 or 1 in kg/m^*100
        ElseIf lngCan = 3 Then
            dblCBD = 5 'Canopy mask is 3 so CBD gets 0.05 or 5 in kg/m^*100
        Else
            'EXP(-2.4887057+(0.0335917*CC)+(-0.356861*SH1_)+(-0.6006381*SH2_)+(-1.10691*PJ)+(-0.0010804*CC*SH1_)+(-0.0018324*CC*SH2_))
            'CBDpred = −2.489 + 0.034(CC)+−0.357(SH1)+−0.601(SH2)+−1.107(PJ)+−0.001(CC × SH1)+−0.002(CC × SH2)

            '0 Means it is a PJ and or J EVT; 1 means none of these are pj or j
            Dim lngPJ As Long = If(isPJ, 0L, 1L)
            Dim lngSH1 As Long = 0
            Dim lngSH2 As Long = 0

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

        Return dblCBD
    End Function


    Private Sub CalcCBDGLM(ByVal FuelName As String, CBDMult As Double, ByVal MUName As String)
        cmdCreateGRID.Text = "CBD GLM"
        Refresh()

        Dim dbPath As String = strProjectPath & "\" & gs_LFTFCSQliteName
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Try
            Dim MUTable As String = MUName + "_CMB"
            Dim defVal As String = DEFAULT_FUEL_VAL.ToString()

            'EVTs that are pj or j
            Dim pjEvts As String = "2016, 2017, 2019, 2025, 2059, 2115, 2116, 2119"

            Using conn As New SQLiteConnection(connString)
                conn.Open()

                'Do GLM based method
                '***************************Calculate for non pj
                Dim nonPjSql As String =
                    "SELECT 0 AS EVTR, NewCCover, NewCHeight, NewCanopy " &
                    "FROM " & MUTable & " " &
                    "WHERE NewCanopy IN (1, 2, 3) " &
                    "GROUP BY NewCCover, NewCHeight, NewCanopy " &
                    "HAVING NewCCover <> " & defVal
                Dim nonPjGroups As List(Of CbdGroup) = LoadCbdGroups(conn, nonPjSql)

                '**********************Calculate for just pj or j (This is only for pj that have a canopy of 1 no need to look at canopy 2
                '**********************because it is already assigned during the non pj above
                Dim pjSql As String =
                    "SELECT EVTR, NewCCover, NewCHeight, NewCanopy " &
                    "FROM " & MUTable & " " &
                    "GROUP BY EVTR, NewCCover, NewCHeight, NewCanopy " &
                    "HAVING EVTR IN (" & pjEvts & ") AND NewCanopy = 1"
                Dim pjGroups As List(Of CbdGroup) = LoadCbdGroups(conn, pjSql)

                Using tx As SQLiteTransaction = conn.BeginTransaction()

                    'Non pj: only fills rows still holding the default value
                    Dim nonPjUpdate As String =
                        "UPDATE " & MUTable & " " &
                        "SET New" & FuelName & " = @cbd " &
                        "WHERE NewCCover = @cov AND NewCHeight = @hgt " &
                        "AND New" & FuelName & " = " & defVal & " " &
                        "AND NewCanopy = @can"

                    Using cmd As New SQLiteCommand(nonPjUpdate, conn, tx)
                        cmd.Parameters.Add("@cbd", DbType.Double)
                        cmd.Parameters.Add("@cov", DbType.Int64)
                        cmd.Parameters.Add("@hgt", DbType.Int64)
                        cmd.Parameters.Add("@can", DbType.Int64)

                        For Each g As CbdGroup In nonPjGroups
                            'Get the height in meters from the height code
                            cmd.Parameters("@cbd").Value = ComputeCBD(g.NewCCover, g.NewCHeight / 10.0,
                                                                      g.NewCanopy, False, CBDMult)
                            cmd.Parameters("@cov").Value = g.NewCCover
                            cmd.Parameters("@hgt").Value = g.NewCHeight
                            cmd.Parameters("@can").Value = g.NewCanopy
                            cmd.ExecuteNonQuery()
                        Next
                    End Using

                    'PJ: no default-value guard, so these overwrite the non pj pass
                    Dim pjUpdate As String =
                        "UPDATE " & MUTable & " " &
                        "SET New" & FuelName & " = @cbd " &
                        "WHERE EVTR = @evt AND NewCCover = @cov " &
                        "AND NewCHeight = @hgt AND NewCanopy = @can"

                    Using cmd As New SQLiteCommand(pjUpdate, conn, tx)
                        cmd.Parameters.Add("@cbd", DbType.Double)
                        cmd.Parameters.Add("@evt", DbType.Int64)
                        cmd.Parameters.Add("@cov", DbType.Int64)
                        cmd.Parameters.Add("@hgt", DbType.Int64)
                        cmd.Parameters.Add("@can", DbType.Int64)

                        For Each g As CbdGroup In pjGroups
                            cmd.Parameters("@cbd").Value = ComputeCBD(g.NewCCover, g.NewCHeight / 10.0,
                                                                      g.NewCanopy, True, CBDMult)
                            cmd.Parameters("@evt").Value = g.EVTR
                            cmd.Parameters("@cov").Value = g.NewCCover
                            cmd.Parameters("@hgt").Value = g.NewCHeight
                            cmd.Parameters("@can").Value = g.NewCanopy
                            cmd.ExecuteNonQuery()
                        Next
                    End Using

                    'Update combo table for everywhere that Canopy = 0 and the default value
                    Using cmd As New SQLiteCommand(
                        "UPDATE " & MUTable & " " &
                        "SET New" & FuelName & " = NewCanopy " &
                        "WHERE NewCanopy = 0 OR NewCanopy = " & defVal, conn, tx)
                        cmd.ExecuteNonQuery()
                    End Using

                    tx.Commit()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error In CalcCBDGLM " & ex.Message)
        End Try
    End Sub

    Private Function LoadCbdGroups(conn As SQLiteConnection, sql As String) As List(Of CbdGroup)
        Dim groups As New List(Of CbdGroup)

        strSQL = sql
        Using cmd As New SQLiteCommand(sql, conn)
            Using reader As SQLiteDataReader = cmd.ExecuteReader()
                Dim iEvt As Integer = reader.GetOrdinal("EVTR")
                Dim iCov As Integer = reader.GetOrdinal("NewCCover")
                Dim iHgt As Integer = reader.GetOrdinal("NewCHeight")
                Dim iCan As Integer = reader.GetOrdinal("NewCanopy")

                While reader.Read()
                    Dim g As New CbdGroup
                    g.EVTR = If(reader.IsDBNull(iEvt), 0L, CLng(reader.GetValue(iEvt)))
                    g.NewCCover = If(reader.IsDBNull(iCov), 0L, CLng(reader.GetValue(iCov)))
                    g.NewCHeight = If(reader.IsDBNull(iHgt), 0L, CLng(reader.GetValue(iHgt)))
                    g.NewCanopy = If(reader.IsDBNull(iCan), 0L, CLng(reader.GetValue(iCan)))
                    groups.Add(g)
                End While
            End Using
        End Using

        Return groups
    End Function

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

    Private Structure PixelGroup
        Public EVTR As Long
        Public DIST As Long
        Public SumOfCOUNT As Long
    End Structure

    Private Structure RulePixelGroup
        Public EVT As Long
        Public DIST As Long
        Public PC As Long
    End Structure


    Private Function PixLeftBehind(ByVal strName As String, ByVal MUName As String, ByVal RulesTable As String) As Boolean
        'Return FALSE is cancel is pushed, TRUE is continue is pushed

        Dim PLB = New frmPLB(strName)                                   'List box of pixels left behind

        Dim dbPath As String = strProjectPath & "\" & gs_LFTFCSQliteName
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Try
            Dim MUTable As String = MUName + "_CMB"
            Dim defVal As String = DEFAULT_FUEL_VAL.ToString()

            Dim missing As New List(Of PixelGroup)
            Dim cmbCounts As New List(Of PixelGroup)
            Dim ruleCounts As New List(Of RulePixelGroup)

            Using conn As New SQLiteConnection(connString)
                conn.Open()

                'Check for missing pixel assignments
                missing = LoadPixelGroups(conn,
                    "SELECT EVTR, DIST, SUM(""COUNT"") AS SumOfCOUNT " &
                    "FROM " & MUTable & " " &
                    "GROUP BY EVTR, DIST, New" & strName & " " &
                    "HAVING New" & strName & " = " & defVal)

                '****Check for overlapping or missing rules
                '**** Get cmb pixel counts
                cmbCounts = LoadPixelGroups(conn,
                    "SELECT EVTR, DIST, SUM(""COUNT"") AS SumOfCOUNT " &
                    "FROM " & MUTable & " " &
                    "GROUP BY EVTR, DIST " &
                    "ORDER BY EVTR, DIST")

                '**** Get ruleset pixel counts
                'A NULL or empty PixelCount contributes -1, forcing the group's total negative
                'so the HAVING drops it — i.e. any rule with an uncalculated count disqualifies
                'the whole EVT/DIST group.
                ruleCounts = LoadRulePixelGroups(conn,
                    "SELECT EVT, DIST, " &
                    "SUM(CASE WHEN PixelCount IS NULL OR PixelCount = '' " &
                        "THEN -1 ELSE CAST(PixelCount AS INTEGER) END) AS PC " &
                    "FROM " & RulesTable & " " &
                    "GROUP BY EVT, DIST, OnOff " &
                    "HAVING SUM(CASE WHEN PixelCount IS NULL OR PixelCount = '' " &
                        "THEN -1 ELSE CAST(PixelCount AS INTEGER) END) >= 0 " &
                        "AND OnOff = 'On' " &
                    "ORDER BY EVT, DIST")
            End Using

            For Each g As PixelGroup In missing
                PLB.AddPLB(g.EVTR & "[" & g.DIST & "]" & vbTab & "pixels left behind " & g.SumOfCOUNT)
            Next

            '****Compare and report on overlapping or missing rules
            Dim r As Integer = 0        'Position in ruleCounts
            For Each c As PixelGroup In cmbCounts
                If r < ruleCounts.Count Then
                    'EVT and DIST numbers are the same and can be compared
                    If c.EVTR = ruleCounts(r).EVT AndAlso c.DIST = ruleCounts(r).DIST Then
                        If c.SumOfCOUNT < ruleCounts(r).PC Then
                            PLB.AddPLB(c.EVTR & "[" & c.DIST & "]" & vbTab & "overlapping rules")
                        End If
                        r += 1  'Only advance the ruleset when it lined up
                    Else 'EVT and DIST numbers are not the same so the ruleset is missing some rules
                        PLB.AddPLB(c.EVTR & "[" & c.DIST & "]" & vbTab & "no ruleset")
                    End If
                Else 'No more rules associated with the cmb evts left so count them as missing
                    PLB.AddPLB(c.EVTR & "[" & c.DIST & "]" & vbTab & "no ruleset")
                End If
            Next

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
        Catch ex As Exception
            MsgBox("Error in PixLeftBehind " & ex.Message)
            Return False
        End Try
    End Function

    Private Function LoadPixelGroups(conn As SQLiteConnection, sql As String) As List(Of PixelGroup)
        Dim rows As New List(Of PixelGroup)

        strSQL = sql
        Using cmd As New SQLiteCommand(sql, conn)
            Using reader As SQLiteDataReader = cmd.ExecuteReader()
                Dim iEvt As Integer = reader.GetOrdinal("EVTR")
                Dim iDist As Integer = reader.GetOrdinal("DIST")
                Dim iSum As Integer = reader.GetOrdinal("SumOfCOUNT")

                While reader.Read()
                    Dim g As New PixelGroup
                    g.EVTR = If(reader.IsDBNull(iEvt), 0L, CLng(reader.GetValue(iEvt)))
                    g.DIST = If(reader.IsDBNull(iDist), 0L, CLng(reader.GetValue(iDist)))
                    g.SumOfCOUNT = If(reader.IsDBNull(iSum), 0L, CLng(reader.GetValue(iSum)))
                    rows.Add(g)
                End While
            End Using
        End Using

        Return rows
    End Function

    Private Function LoadRulePixelGroups(conn As SQLiteConnection, sql As String) As List(Of RulePixelGroup)
        Dim rows As New List(Of RulePixelGroup)

        strSQL = sql
        Using cmd As New SQLiteCommand(sql, conn)
            Using reader As SQLiteDataReader = cmd.ExecuteReader()
                Dim iEvt As Integer = reader.GetOrdinal("EVT")
                Dim iDist As Integer = reader.GetOrdinal("DIST")
                Dim iPc As Integer = reader.GetOrdinal("PC")

                While reader.Read()
                    Dim g As New RulePixelGroup
                    g.EVT = If(reader.IsDBNull(iEvt), 0L, CLng(reader.GetValue(iEvt)))
                    g.DIST = If(reader.IsDBNull(iDist), 0L, CLng(reader.GetValue(iDist)))
                    g.PC = If(reader.IsDBNull(iPc), 0L, CLng(reader.GetValue(iPc)))
                    rows.Add(g)
                End While
            End Using
        End Using

        Return rows
    End Function

    Private Sub WriteMULookupCsv(ByVal MUName As String, ByVal csvPath As String)
        Dim MUTable As String = MUName + "_CMB"

        Dim dbPath As String = strProjectPath + "\" + gs_LFTFCSQliteName
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Dim cols As String() = {"VALUE", "NewFBFM13", "NewFBFM40", "NewCanFM", "NewFCCS", "NewFLM",
                                "NewCCover", "NewCHeight", "NewCBH13mx10", "NewCBH40mx10",
                                "NewCBD13x100", "NewCBD40x100", "NewCanopy"}

        Dim sql As String = "SELECT """ & String.Join(""", """, cols) & """ FROM " & MUTable

        Using oWrite As New System.IO.StreamWriter(csvPath, False)
            'Field Headings
            oWrite.WriteLine(String.Join(",", cols))

            Using conn As New SQLiteConnection(connString)
                conn.Open()
                Using cmd As New SQLiteCommand(sql, conn)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        Dim ordinals(cols.Length - 1) As Integer
                        For i As Integer = 0 To cols.Length - 1
                            ordinals(i) = reader.GetOrdinal(cols(i))
                        Next

                        'Make lookup csv
                        Dim fields(cols.Length - 1) As String
                        While reader.Read()
                            For i As Integer = 0 To cols.Length - 1
                                fields(i) = If(reader.IsDBNull(ordinals(i)), "",
                                               reader.GetValue(ordinals(i)).ToString())
                            Next
                            oWrite.WriteLine(String.Join(",", fields))
                        End While
                    End Using
                End Using
            End Using
        End Using
    End Sub

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

                    'Make lookup csv
                    WriteMULookupCsv(MUName, strProjectPath + "\MU\tempMULUT.csv")

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

                        If fuel.Equals(FuelList.Last) Then
                            container.RemoveLayer(tempLayer)
                        End If
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

