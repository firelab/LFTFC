Imports System.Data.SQLite
Imports System.IO
Imports ArcGIS.Core.Data
Imports ArcGIS.Core.Data.Raster
Imports ArcGIS.Desktop.Catalog
Imports ArcGIS.Desktop.Core
Imports ArcGIS.Desktop.Core.Geoprocessing
Imports ArcGIS.Desktop.Framework.Threading.Tasks
Imports ArcGIS.Desktop.Mapping

Public Class frmAddMU
    Private strSQL As String                                                'SQL variable for this module
    Private strchangeflag As String
    Private strProjectPath As String
    Private strInputDataPath As String

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'Check for project directory
        If gs_validProject = False Then
            Enabled = False
        Else
            'Get project path
            strProjectPath = gs_ProjectPath()
            strInputDataPath = gs_ProjectPath()

            'Set labels
            SetMULabels()
        End If
    End Sub

    Private Sub CopyTableToSQLite(rasPath As String, rasName As String, IsGRID As Boolean)

        ' SQLite database
        Dim dbPath As String = Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        ' Open raster datastore
        Dim connectionPath As New FileSystemConnectionPath(New Uri(rasPath), FileSystemDatastoreType.Raster)
        Dim dataStore As New FileSystemDatastore(connectionPath)

        Dim rasDS As RasterDataset
        If Not IsGRID Then
            rasDS = dataStore.OpenDataset(Of RasterDataset)(rasName & ".tif")
        Else
            rasDS = dataStore.OpenDataset(Of RasterDataset)(rasName)
        End If

        Dim rasBand = rasDS.GetBand(0)
        Dim rasTable = rasBand.GetAttributeTable()
        Dim pCursor = rasTable.Search()

        Try
            Using conn As New SQLiteConnection(connString)
                conn.Open()

                ' --------------------------------------------------------------
                ' New Management Unit (MU) name into DATA_MU_Name
                ' --------------------------------------------------------------
                Using cmd As New SQLiteCommand("INSERT INTO DATA_MU_Name (Name) VALUES (@nm)", conn)
                    cmd.Parameters.AddWithValue("@nm", rasName)
                    cmd.ExecuteNonQuery()
                End Using

                ' --------------------------------------------------------------
                ' CREATE CMB TABLE
                ' --------------------------------------------------------------
                Dim cmbTable As String = rasName & "_CMB"

                Dim sqlCreateCMB As String =
                "CREATE TABLE IF NOT EXISTS " & cmbTable & " (" &
                "VALUE INTEGER, COUNT INTEGER, EVTR INTEGER, DIST INTEGER, EVCR INTEGER, EVHR INTEGER, " &
                "BPSRF INTEGER, WILDCARD TEXT, " &
                "NewFBFM13 INTEGER, NewFBFM40 INTEGER, NewCanFM INTEGER, NewFCCS INTEGER, NewFLM INTEGER, " &
                "NewCCover INTEGER, NewCHeight INTEGER, NewCBH13mx10 INTEGER, NewCBH40mx10 INTEGER, " &
                "NewCBD13x100 INTEGER, NewCBD40x100 INTEGER, NewCanopy INTEGER)"

                Using cmd As New SQLiteCommand(sqlCreateCMB, conn)
                    cmd.ExecuteNonQuery()
                End Using

                ' --------------------------------------------------------------
                ' CREATE RULESETS TABLE
                ' --------------------------------------------------------------
                Dim rulesTable As String = rasName & "_Rulesets"

                Dim sqlCreateRules As String =
                "CREATE TABLE IF NOT EXISTS " & rulesTable & " (" &
                "ID INTEGER PRIMARY KEY AUTOINCREMENT, " &
                "EVT INTEGER, DIST INTEGER, Cover_Low INTEGER, Cover_High INTEGER, " &
                "Height_Low INTEGER, Height_High INTEGER, BPSRF TEXT, Wildcard TEXT, " &
                "FBFM13 INTEGER, FBFM40 TEXT, CanFM TEXT, FCCS INTEGER, FLM INTEGER, " &
                "CCover INTEGER, CHeight INTEGER, CBD13x100 INTEGER, CBD40x100 INTEGER, " &
                "CBH13mx10 INTEGER, CBH40mx10 INTEGER, Canopy INTEGER, OnOff TEXT, Notes TEXT, PixelCount TEXT)"

                Using cmd As New SQLiteCommand(sqlCreateRules, conn)
                    cmd.ExecuteNonQuery()
                End Using

                ' --------------------------------------------------------------
                ' INSERT FOR BATCH INSERTION
                ' --------------------------------------------------------------
                Dim insertSQL As String =
                "INSERT INTO " & cmbTable & " (" &
                "VALUE, COUNT, EVTR, DIST, EVCR, EVHR, BPSRF, WILDCARD, " &
                "NewFBFM13, NewFBFM40, NewCanFM, NewFCCS, NewFLM, " &
                "NewCCover, NewCHeight, NewCBH13mx10, NewCBH40mx10, " &
                "NewCBD13x100, NewCBD40x100, NewCanopy) " &
                "VALUES (@v, @cnt, @evtr, @dist, @evcr, @evhr, @bps, @wild, " &
                "@fb13, @fb40, @canfm, @fccs, @flm, @ccover, @cheight, " &
                "@cbh13, @cbh40, @cbd13, @cbd40, @canopy)"

                ' --------------------------------------------------------------
                ' BATCH INSERT TRANSACTION
                ' --------------------------------------------------------------
                Using tran As SQLiteTransaction = conn.BeginTransaction()
                    Using insertCmd As New SQLiteCommand(insertSQL, conn, tran)

                        Dim pRow As Row

                        While pCursor.MoveNext()

                            pRow = pCursor.Current()

                            insertCmd.Parameters.Clear()

                            Dim i As Integer = 1

                            insertCmd.Parameters.AddWithValue("@v", pRow.Item(i)) : i += 1
                            insertCmd.Parameters.AddWithValue("@cnt", pRow.Item(i)) : i += 1
                            insertCmd.Parameters.AddWithValue("@evtr", pRow.Item(i)) : i += 1

                            ' DIST logic: depends on user path
                            If txtDistPath.Text.Contains(":\") Then
                                insertCmd.Parameters.AddWithValue("@dist", pRow.Item(i)) : i += 1
                            Else
                                insertCmd.Parameters.AddWithValue("@dist", 0)
                            End If

                            insertCmd.Parameters.AddWithValue("@evcr", pRow.Item(i)) : i += 1
                            insertCmd.Parameters.AddWithValue("@evhr", pRow.Item(i)) : i += 1
                            insertCmd.Parameters.AddWithValue("@bps", pRow.Item(i)) : i += 1

                            ' WILDCARD logic
                            If txtWildPath.Text.Contains(":\") Then
                                insertCmd.Parameters.AddWithValue("@wild", pRow.Item(i))
                            Else
                                insertCmd.Parameters.AddWithValue("@wild", "None")
                            End If

                            ' Default values — now from shared constant
                            insertCmd.Parameters.AddWithValue("@fb13", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@fb40", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@canfm", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@fccs", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@flm", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@ccover", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@cheight", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@cbh13", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@cbh40", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@cbd13", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@cbd40", DEFAULT_FUEL_VAL)
                            insertCmd.Parameters.AddWithValue("@canopy", DEFAULT_FUEL_VAL)

                            insertCmd.ExecuteNonQuery()

                        End While

                    End Using

                    tran.Commit()
                End Using

            End Using

        Catch ex As Exception
            MsgBox("Error in CopyTableToSQLite - " & ex.Message)
        End Try

        ' cleanup raster resources
        pCursor.Dispose()
        rasTable.Dispose()

    End Sub

    Private Sub Update_LUT_BPS(ByVal rasTable As Table)

        Dim dbPath As String = Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Dim rasCursor = rasTable.Search()
        Dim rasRow As Row = Nothing

        Try
            ' Locate raster fields
            Dim BPSField As Integer = 1  'first column (VALUE)
            Dim BPS_CodeField As Integer = rasCursor.FindField("BPS_CODE")
            Dim BPS_ModelField As Integer = rasCursor.FindField("BPS_MODEL")
            Dim BPS_NameField As Integer = rasCursor.FindField("BPS_NAME")

            If BPSField <> -1 AndAlso
                BPS_CodeField <> -1 AndAlso
                BPS_ModelField <> -1 AndAlso
                BPS_NameField <> -1 Then

                Using conn As New SQLiteConnection(connString)
                    conn.Open()

                    ' Loop all raster rows
                    While rasCursor.MoveNext()

                        rasRow = rasCursor.Current

                        Dim bpsValue As Integer = CInt(rasRow.Item(BPSField))
                        Dim bpsCodeValue As Integer = If(IsNumeric(rasRow.Item(BPS_CodeField)),
                                                     CInt(rasRow.Item(BPS_CodeField)),
                                                     DEFAULT_FUEL_VAL)

                        Dim bpsModelValue As Integer = If(IsNumeric(rasRow.Item(BPS_ModelField)),
                                                      CInt(rasRow.Item(BPS_ModelField)),
                                                      DEFAULT_FUEL_VAL)

                        Dim bpsNameValue As String = rasRow.Item(BPS_NameField).ToString()

                        ' ----------------------------------------------------
                        ' Check whether BPS already exists
                        ' ----------------------------------------------------
                        Dim sqlCheck As String =
                        "SELECT BPS FROM LUT_BPS WHERE BPS = @bps LIMIT 1"

                        Dim exists As Boolean = False

                        Using cmdCheck As New SQLiteCommand(sqlCheck, conn)
                            cmdCheck.Parameters.AddWithValue("@bps", bpsValue)
                            Using rd As SQLiteDataReader = cmdCheck.ExecuteReader()
                                exists = rd.Read()
                            End Using
                        End Using

                        ' ----------------------------------------------------
                        ' Update existing BPS
                        ' ----------------------------------------------------
                        If exists Then

                            Dim sqlUpdate As String =
                            "UPDATE LUT_BPS " &
                            "SET BPS_Code = @code, " &
                            "    BPS_Model = @model, " &
                            "    Name = @name " &
                            "WHERE BPS = @bps"

                            Using cmdUpdate As New SQLiteCommand(sqlUpdate, conn)
                                cmdUpdate.Parameters.AddWithValue("@code", bpsCodeValue)
                                cmdUpdate.Parameters.AddWithValue("@model", bpsModelValue)
                                cmdUpdate.Parameters.AddWithValue("@name", bpsNameValue)
                                cmdUpdate.Parameters.AddWithValue("@bps", bpsValue)
                                cmdUpdate.ExecuteNonQuery()
                            End Using

                        Else
                            ' ----------------------------------------------------
                            ' Insert new BPS record
                            ' ----------------------------------------------------
                            Dim sqlInsert As String =
                            "INSERT INTO LUT_BPS (BPS, BPS_Code, BPS_Model, Name) " &
                            "VALUES (@bps, @code, @model, @name)"

                            Using cmdInsert As New SQLiteCommand(sqlInsert, conn)
                                cmdInsert.Parameters.AddWithValue("@bps", bpsValue)
                                cmdInsert.Parameters.AddWithValue("@code", bpsCodeValue)
                                cmdInsert.Parameters.AddWithValue("@model", bpsModelValue)
                                cmdInsert.Parameters.AddWithValue("@name", bpsNameValue)
                                cmdInsert.ExecuteNonQuery()
                            End Using

                        End If

                    End While
                End Using

            End If

        Catch ex As Exception
            MsgBox("Error in Update_LUT_BPS (SQLite) - " & ex.Message)

        Finally
            ' Clean up raster cursor/table
            rasCursor.Dispose()
            rasTable.Dispose()
        End Try

    End Sub

    Private Sub Update_EVT_EVG_EVS_w_EVTTable(rasTable As Table)

        Dim dbPath As String = Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Dim rasCursor = rasTable.Search()
        Dim rasRow As Row = Nothing

        Try
            ' Locate raster fields
            Dim EVTField As Integer = rasCursor.FindField("EVT_FUEL")
            If EVTField = -1 Then EVTField = 1     'Fallback to first field for older rasters

            Dim EVTNameField As Integer = rasCursor.FindField("EVT_FUEL_N")
            If EVTNameField = -1 Then
                'Nothing to update — name field missing
                rasCursor.Dispose()
                rasTable.Dispose()
                Exit Sub
            End If

            Using conn As New SQLiteConnection(connString)
                conn.Open()

                'Loop over raster attribute rows
                While rasCursor.MoveNext()

                    rasRow = rasCursor.Current()

                    Dim evtValue As Integer = CInt(rasRow.Item(EVTField))
                    Dim evtName As String = rasRow.Item(EVTNameField).ToString()

                    ' ----------------------------------------------------------
                    ' Check if EVT already exists in XWALK_EVT_EVG_EVS
                    ' ----------------------------------------------------------
                    Dim exists As Boolean = False
                    Dim sqlCheck As String =
                    "SELECT EVT FROM XWALK_EVT_EVG_EVS WHERE EVT = @evt LIMIT 1"

                    Using cmdCheck As New SQLiteCommand(sqlCheck, conn)
                        cmdCheck.Parameters.AddWithValue("@evt", evtValue)
                        Using rd As SQLiteDataReader = cmdCheck.ExecuteReader()
                            exists = rd.Read()
                        End Using
                    End Using

                    ' ----------------------------------------------------------
                    ' UPDATE EXISTING EVT NAME
                    ' ----------------------------------------------------------
                    If exists Then

                        Dim sqlUpdate As String =
                        "UPDATE XWALK_EVT_EVG_EVS " &
                        "SET EVT_Name = @name " &
                        "WHERE EVT = @evt"

                        Using cmdUpdate As New SQLiteCommand(sqlUpdate, conn)
                            cmdUpdate.Parameters.AddWithValue("@name", evtName)
                            cmdUpdate.Parameters.AddWithValue("@evt", evtValue)
                            cmdUpdate.ExecuteNonQuery()
                        End Using

                    Else
                        ' ----------------------------------------------------------
                        ' INSERT NEW EVT RECORD
                        ' ----------------------------------------------------------
                        Dim sqlInsert As String =
                        "INSERT INTO XWALK_EVT_EVG_EVS (EVT, EVT_Name) " &
                        "VALUES (@evt, @name)"

                        Using cmdInsert As New SQLiteCommand(sqlInsert, conn)
                            cmdInsert.Parameters.AddWithValue("@evt", evtValue)
                            cmdInsert.Parameters.AddWithValue("@name", evtName)
                            cmdInsert.ExecuteNonQuery()
                        End Using

                    End If

                End While

            End Using

        Catch ex As Exception
            MsgBox("Error in Update_EVT_EVG_EVS_w_EVTTable (SQLite) - " & ex.Message)

        Finally
            rasCursor.Dispose()
            rasTable.Dispose()
        End Try

    End Sub

    Private Async Sub GetRasterPath(pControl As System.Windows.Forms.Control) 'Set the control with the selected files path
        Dim openID As New OpenItemDialog
        openID.Title = "Select " + pControl.Name
        openID.InitialLocation = strInputDataPath
        openID.Filter = ItemFilters.Rasters
        openID.MultiSelect = False                          'Don't allow multiple selection. Make the users select the correct layers

        openID.ShowDialog()
        If openID.Items.Count = 0 Then
            'Do Nothing
        Else
            Dim item = openID.Items.First
            pControl.Text = item.Path

            If pControl.Name = "txtEVTPath" Then

                lblStatus.Visible = False
                lblStatus.Update()

                If ItemFactory.Instance.CanGetDataset(item) Then
                    lblStatus.Text = "Update FVT Names"
                    lblStatus.Update()
                    Await QueuedTask.Run(
                        Sub()
                            'Get raster table from selection and use it to update the XWALK_EVT_EVG_EVS table with updates
                            Dim strpath = Strings.Left(item.Path, Strings.Len(item.Path) - Strings.Len(item.Name))
                            Dim connectionPath = New FileSystemConnectionPath(New System.Uri(strpath), FileSystemDatastoreType.Raster)
                            Dim dataStore = New FileSystemDatastore(connectionPath)
                            Dim rasDS = dataStore.OpenDataset(Of RasterDataset)(item.Name)
                            Dim rasBand = rasDS.GetBand(0)
                            Dim rasTable = rasBand.GetAttributeTable
                            Update_EVT_EVG_EVS_w_EVTTable(rasTable)
                            rasTable.Dispose()
                        End Sub)
                End If
            ElseIf pControl.Name = "txtBPSPath" Then
                If ItemFactory.Instance.CanGetDataset(item) Then
                    lblStatus.Text = "Update BPS Names"
                    lblStatus.Update()
                    Await QueuedTask.Run(
                        Sub()
                            'Get raster table from selection and use it to update the XWALK_EVT_EVG_EVS table with updates
                            Dim strpath = Strings.Left(item.Path, Strings.Len(item.Path) - Strings.Len(item.Name))
                            Dim connectionPath = New FileSystemConnectionPath(New System.Uri(strpath), FileSystemDatastoreType.Raster)
                            Dim dataStore = New FileSystemDatastore(connectionPath)
                            Dim rasDS = dataStore.OpenDataset(Of RasterDataset)(item.Name)
                            Dim rasBand = rasDS.GetBand(0)
                            Dim rasTable = rasBand.GetAttributeTable
                            Update_LUT_BPS(rasTable)
                            rasTable.Dispose()
                        End Sub)
                End If
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Try
            'Remove the form
            Visible = False 'Remove the form
            Close()
        Catch ex As Exception
            MsgBox("ThenError in cmdCancel_Click - " & ex.Message)
        End Try
    End Sub

    Private Sub cmdCreateMU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreateMU.Click
        lblStatus.Visible = True
        lblStatus.Text = "Processing MU"

        cmdCreateMU.Enabled = False
        cmdCreateMU.Text = "Wait"

        Dim strName As String                                           'Short MU name for ASP ELV and SLP

        Try 'Check for valid names
            lblStatus.Text = "Check for valid names"
            lblStatus.Refresh()

            If gs_ValidName(txtAddMUName.Text, 13, strProjectPath, "MU", rdoOutTiff.Checked) = False Then
                Dim errMU As New System.Exception(txtAddMUName.Text & " is not a valid name or is already in use.")
                Throw errMU
            End If
            If Len(txtAddMUName.Text) <= 10 Then
                strName = txtAddMUName.Text
            Else
                strName = Strings.Left(txtAddMUName.Text, 12)
            End If
            If txtAspPath.Text.Contains(":\") Then
                If gs_ValidName(strName & "ASP", 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    Dim errASP As New System.Exception(strName & "A " & " is not a valid name or is already in use.")
                    Throw errASP
                End If
            End If
            If txtSlpPath.Text.Contains(":\") Then
                If gs_ValidName(strName & "SLP", 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    Dim errSLP As New System.Exception(strName & "S " & " is not a valid name or is already in use.")
                    Throw errSLP
                End If
            End If
            If txtElevPath.Text.Contains(":\") Then
                If gs_ValidName(strName & "ELV", 13, strProjectPath, "Output", rdoOutTiff.Checked) = False Then
                    Dim errELV As New System.Exception(strName & "E " & " is not a valid name or is already in use.")
                    Throw errELV
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            cmdCreateMU.Enabled = True
            cmdCreateMU.Text = "Make " & vbCrLf & "MU"
            Refresh()
            Exit Sub
        End Try

        'Check for rasters paths for required (FVT,FVC,FVH,BPS)
        If txtAddMUName.Text = "" Or txtEVTPath.Text.Contains(":\") = False Or txtEVCPath.Text.Contains(":\") = False Or
                                     txtEVHPath.Text.Contains(":\") = False Or txtBPSPath.Text.Contains(":\") = False Then
            MsgBox("Fill in all blanks that are not optional.")
        Else 'Continue processing
            'Make a collection of TopoClip to be created
            Dim theTopoClips As New List(Of TopoClip)
            Dim ext As String = ""
            If rdoOutTiff.Checked Then ext = ".tif"
            If txtAspPath.Text.Contains(":\") Then
                theTopoClips.Add(New TopoClip With {.Name = txtAspPath.Text, .SaveAs = strProjectPath + "\Output\" + strName + "A" + ext})
            End If
            If txtSlpPath.Text.Contains(":\") Then
                theTopoClips.Add(New TopoClip With {.Name = txtSlpPath.Text, .SaveAs = strProjectPath + "\Output\" + strName + "S" + ext})
            End If
            If txtElevPath.Text.Contains(":\") Then
                theTopoClips.Add(New TopoClip With {.Name = txtElevPath.Text, .SaveAs = strProjectPath + "\Output\" + strName + "E" + ext})
            End If

            'List of raster going in to the geoprocess
            Dim inRasList As String = ""

            'Set inRasList
            If txtEVTPath.Text.Contains(":\") Then inRasList = txtEVTPath.Text
            If txtDistPath.Text.Contains(":\") Then inRasList += "; " + txtDistPath.Text
            If txtEVCPath.Text.Contains(":\") Then inRasList += "; " + txtEVCPath.Text
            If txtEVHPath.Text.Contains(":\") Then inRasList += "; " + txtEVHPath.Text
            If txtBPSPath.Text.Contains(":\") Then inRasList += "; " + txtBPSPath.Text

            DoTheWork(strName, theTopoClips, txtAddMUName.Text, rdoOutGRID.Checked, inRasList) 'Make the combine, add rat to access database, clip Asp, Slp, Ele

            'Make it go away
            Close()
        End If
    End Sub

    Public Class TopoClip
        Public Property Name As String
        Public Property SaveAs As String
    End Class

    Private Async Sub DoTheWork(ByVal ASE_BaseName As String, ByVal theTopoClips As List(Of TopoClip),
                                ByVal MUSaveName As String, ByVal blnMUGRID As Boolean, ByVal inRasList As String) 'Make the combine, add rat to access database, clip Asp, Slp, Ele
        Await QueuedTask.Run(
           Sub()
               'Aspect Slope Elevation String Name
               Dim strName = ASE_BaseName

               'Work status
               Dim frmWork As New frmWorkStatus
               frmWork.Show()
               frmWork.BringToFront()

               Try
                   Dim strErrName As String
                   strErrName = ""

                   frmWork.UpdateStatus("Drawing Paused")
                   gs_Map.GetMapPanes.First.MapView.DrawingPaused = True

                   'Get current map view
                   Dim container = gs_Map

                   Dim MUSave As String
                   If blnMUGRID Then
                       MUSave = gs_ProjectPath + "\MU\" + MUSaveName
                   Else
                       MUSave = gs_ProjectPath + "\MU\" + MUSaveName + ".tif"
                   End If

                   'Execute Add Join to MU raster
                   frmWork.UpdateStatus("MU " & MUSave)
                   frmWork.UpdateStatus("Combine")

                   'Do the combine
                   Dim val_array = Geoprocessing.MakeValueArray(inRasList, MUSave)
                   Dim env_array As IReadOnlyList(Of KeyValuePair(Of String, String))

                   If chkExtent.Checked Then
                       'Set extent to the map view
                       env_array = Geoprocessing.MakeEnvironmentArray(extent:=MapView.Active.Extent, cellSize:=txtEVTPath.Text,
                                                                    snapRaster:=txtEVTPath.Text, overwriteoutput:=False)
                   Else
                       env_array = Geoprocessing.MakeEnvironmentArray(extent:="MINOF", cellSize:=txtEVTPath.Text, snapRaster:=txtEVTPath.Text,
                                                                      overwriteoutput:=False)
                   End If

                   Geoprocessing.ExecuteToolAsync("sa.Combine", val_array, env_array)

                   'The first layer is the combine from the geoprocessing of the MU 
                   Dim cmbLayer = container.GetLayersAsFlattenedList().OfType(Of RasterLayer).First

                   'Clip Aspect Slope Elevation to combine
                   Dim inRas As String = ""
                   Dim outRas As String = ""
                   For Each topo In theTopoClips
                       With topo
                           inRas = .Name
                           outRas = .SaveAs
                       End With

                       frmWork.UpdateStatus("Clipping" + inRas)

                       'Execute GP extract by mask
                       val_array = Geoprocessing.MakeValueArray(inRas, cmbLayer, outRas) '(InRas, MaskRas, OutRas)
                       env_array = Geoprocessing.MakeEnvironmentArray(cellSize:=cmbLayer, snapRaster:=cmbLayer, overwriteoutput:=False)
                       Geoprocessing.ExecuteToolAsync("sa.ExtractbyMask", val_array, env_array)

                       'Get the layer and remove it
                       Dim topoLayer = container.GetLayersAsFlattenedList().OfType(Of RasterLayer).First
                       container.RemoveLayer(topoLayer)
                   Next

                   'Remove layers
                   container.RemoveLayer(cmbLayer)

                   'Copy Table to access
                   frmWork.UpdateStatus("Copy MU table to database")

                   CopyTableToSQLite(gs_ProjectPath + "\MU\", MUSaveName, blnMUGRID)

                   'Add one to the count to trigger and update change
                   gs_MUCount += 1

                   'Unpause active view

               Catch ex As Exception
                   Dim errMessageString As String = ""
                   errMessageString = errMessageString & ex.Message
                   MsgBox("Error in work method - " & errMessageString & vbCrLf &
                           "Possible solutions " & vbCrLf &
                           "-> Make sure Microsoft Access is the " & vbCrLf &
                           "   64-bit version." & vbCrLf &
                           "-> Make sure that LFTFC_Pro is the " & vbCrLf &
                           "   selected map view tab" & vbCrLf &
                           "-> Make sure that all MU input rasters have " & vbCrLf &
                           "   the same projection" & vbCrLf &
                           "-> Make sure that all MU input rasters have " & vbCrLf &
                           "   data in the same general area")
               End Try
               frmWork.Close()
           End Sub)
        gs_Map.GetMapPanes.First.MapView.DrawingPaused = False
    End Sub

    Private Sub txtEVTPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEVTPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding FVT"

        'Disable txtEVTPath
        txtEVTPath.Enabled = False

        Try
            GetRasterPath(txtEVTPath)
        Catch ex As Exception
            MsgBox("Error in txtEVTPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtEVTPath.Enabled = True
    End Sub

    Private Sub txtEVCPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEVCPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding FVC"

        'Disable txtEVCPath
        txtEVCPath.Enabled = False

        Try
            GetRasterPath(txtEVCPath)
        Catch ex As Exception
            MsgBox("Error in txtEVCPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtEVCPath.Enabled = True
    End Sub

    Private Sub txtEVHPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEVHPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding FVH"

        'Disable txtEVHPath
        txtEVHPath.Enabled = False

        Try
            GetRasterPath(txtEVHPath)
        Catch ex As Exception
            MsgBox("Error in txtEVHPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtEVHPath.Enabled = True
    End Sub

    Private Sub txtBPSPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBPSPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding BPS"

        'Disable txtBPSPath
        txtBPSPath.Enabled = False

        Try
            GetRasterPath(txtBPSPath)
        Catch ex As Exception
            MsgBox("Error in txtBPSPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtBPSPath.Enabled = True
    End Sub

    Private Sub txtDistPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDistPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding Dist"

        'Disable txtDistPath
        txtDistPath.Enabled = False

        Try
            GetRasterPath(txtDistPath)
        Catch ex As Exception
            MsgBox("Error in txtDistPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtDistPath.Enabled = True
    End Sub

    Private Sub txtWildPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWildPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding Wild"

        'Disable txtWildPath
        txtWildPath.Enabled = False

        Try
            GetRasterPath(txtWildPath)
        Catch ex As Exception
            MsgBox("Error in txtWildPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtWildPath.Enabled = True
    End Sub

    Private Sub txtASPPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAspPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding ASP"

        'Disable txtAspPath
        txtAspPath.Enabled = False

        Try
            GetRasterPath(txtAspPath)
        Catch ex As Exception
            MsgBox("Error in txtASPPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtAspPath.Enabled = True
    End Sub

    Private Sub txtSLPPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSlpPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding SLP"

        'Disable txtSLPPath
        txtSlpPath.Enabled = False

        Try
            GetRasterPath(txtSlpPath)
        Catch ex As Exception
            MsgBox("Error in txtSLPPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtSlpPath.Enabled = True
    End Sub

    Private Sub txtELEVPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtElevPath.DoubleClick
        'Set label
        lblStatus.Visible = True
        lblStatus.Text = "Adding ELEV"

        'Disable txtELEVPath
        txtElevPath.Enabled = False

        Try
            GetRasterPath(txtElevPath)
        Catch ex As Exception
            MsgBox("Error in txtELEVPath_DoubleClick - " & ex.Message)
        End Try
        lblStatus.Visible = False
        txtElevPath.Enabled = True
    End Sub

    Private Sub SetMULabels()
        'Set tooltips
        TTFVT.SetToolTip(txtEVTPath, txtEVTPath.Tag)
        TTFVT.ShowAlways = True
        TTFVC.SetToolTip(txtEVCPath, txtEVCPath.Tag)
        TTFVC.ShowAlways = True
        TTFVH.SetToolTip(txtEVHPath, txtEVHPath.Tag)
        TTFVH.ShowAlways = True
        TTBPS.SetToolTip(txtBPSPath, txtBPSPath.Tag)
        TTBPS.ShowAlways = True
        TTFDIST.SetToolTip(txtDistPath, txtDistPath.Tag)
        TTFDIST.ShowAlways = True
        TTWildcard.SetToolTip(txtWildPath, txtWildPath.Tag)
        TTWildcard.ShowAlways = True
        TTAsp.SetToolTip(txtAspPath, txtAspPath.Tag)
        TTAsp.ShowAlways = True
        TTSlp.SetToolTip(txtSlpPath, txtSlpPath.Tag)
        TTSlp.ShowAlways = True
        TTElev.SetToolTip(txtElevPath, txtElevPath.Tag)
        TTElev.ShowAlways = True
    End Sub

End Class