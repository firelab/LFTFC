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

    Private Sub CopyTableToAccess(ByVal rasPath As String, ByVal rasName As String, ByVal IsGRID As Boolean) 'Copies ITable to Access in the correct format

        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName

        dbconn.Open()

        'Get raster table
        'Dim strpath = Strings.Left(Item.Path, Strings.Len(Item.Path) - Strings.Len(Item.Name))
        Dim connectionPath = New FileSystemConnectionPath(New System.Uri(rasPath), FileSystemDatastoreType.Raster)
        Dim dataStore = New FileSystemDatastore(connectionPath)
        Dim rasDS As RasterDataset
        If IsGRID = False Then
            rasDS = dataStore.OpenDataset(Of RasterDataset)(rasName & ".tif")
        Else
            rasDS = dataStore.OpenDataset(Of RasterDataset)(rasName)
        End If

        Dim rasBand = rasDS.GetBand(0)
        Dim rasTable = rasBand.GetAttributeTable
        Dim pCursor = rasTable.Search()

        Try
            'Add Data Name to DATA_MU_Name Table
            strSQL = "INSERT INTO DATA_MU_Name " &
                     "VALUES ('" & rasName & "')"
            dbconn.Execute(strSQL)

            'Create Management Unit table in access to store new combo grid
            strSQL = "CREATE TABLE " & rasName & "_CMB " &
                               "( [VALUE] int, [COUNT] int, EVTR int, DIST int, EVCR int, EVHR int, BPSRF int, WILDCARD text(255), " &
                               "NewFBFM13 int, NewFBFM40 int, NewCanFM int, NewFCCS int, " &
                               "NewFLM int, NewCCover int, NewCHeight int, " &
                               "NewCBH13mx10 int, NewCBH40mx10 int, NewCBD13x100 int, " &
                               "NewCBD40x100 int, NewCanopy int )"
            dbconn.Execute(strSQL)

            'Create new Rulesets table in access for the new Management Unit
            strSQL = "CREATE TABLE " & rasName & "_Rulesets " &
                               "( ID AUTOINCREMENT, EVT int, DIST int, Cover_Low int, Cover_High int, Height_Low int, " &
                               "Height_High int, BPSRF text, Wildcard text(255), FBFM13 int, FBFM40 text, CanFM text, FCCS int, " &
                               "FLM int, CCover int, CHeight int, CBD13x100 int, CBD40x100 int, CBH13mx10 int, CBH40mx10 int, " &
                               "Canopy int, OnOff text, Notes Memo, PixelCount text )"
            dbconn.Execute(strSQL)

            Dim i As Integer                                                'Index for column identifier in pRow
            i = 1

            rs1.CursorLocation = ADODB.CursorLocationEnum.adUseClient           '<<<< important!

            'strSQL = "SELECT " & rasName & "_CMB " & ".* FROM " & rasName & "_CMB"

            'rs1.Open(rasName & "_CMB ", dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
            rs1.Open(rasName & "_CMB ", dbconn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockBatchOptimistic)

            Dim pRow As Row = Nothing
            While pCursor.MoveNext
                pRow = pCursor.Current()
                With rs1
                    .AddNew()

                    .Fields("VALUE").Value = pRow.Item(i)
                    i += 1
                    .Fields("COUNT").Value = pRow.Item(i)
                    i += 1
                    .Fields("EVTR").Value = pRow.Item(i)
                    i += 1
                    If txtDistPath.Text.Contains(":\") = False Then
                        .Fields("DIST").Value = "0"
                    Else
                        .Fields("DIST").Value = pRow.Item(i)
                        i += 1
                    End If
                    .Fields("EVCR").Value = pRow.Item(i)
                    i += 1
                    .Fields("EVHR").Value = pRow.Item(i)
                    i += 1
                    .Fields("BPSRF").Value = pRow.Item(i)
                    i += 1
                    If txtWildPath.Text.Contains(":\") = False Then
                        .Fields("WILDCARD").Value = "None"
                    Else
                        .Fields("WILDCARD").Value = pRow.Item(i)
                    End If
                    .Fields("NewFBFM13").Value = 9999
                    .Fields("NewFBFM40").Value = 9999
                    .Fields("NewCanFM").Value = 9999
                    .Fields("NewFCCS").Value = 9999
                    .Fields("NewFLM").Value = 9999
                    .Fields("NewCCover").Value = 9999
                    .Fields("NewCHeight").Value = 9999
                    .Fields("NewCBH13mx10").Value = 9999
                    .Fields("NewCBH40mx10").Value = 9999
                    .Fields("NewCBD13x100").Value = 9999
                    .Fields("NewCBD40x100").Value = 9999
                    .Fields("NewCanopy").Value = 9999
                    i = 1 'Reset i back to 1 to start over at the first field
                End With
            End While

            rs1.UpdateBatch()

            If rs1.State <> 0 Then rs1.Close() 'Recordset needs to be closed
            rs1 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
            MsgBox("Error in CopyTableToAccess - " & ex.Message)
        End Try
        pCursor.Dispose()                                                                          'Dispose of the table cursor
        rasTable.Dispose()
    End Sub

    Private Sub Update_LUT_BPS(ByVal rasTable As Table) 'Copies ITable to Access in the correct format
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Dim rasCursor = rasTable.Search()
        Dim rasRow As Row = Nothing
        Try
            Dim BPSField, BPS_CodeField, BPS_ModelField As Integer
            Dim BPS_NameField As Integer
            Dim BPS_CodeValue, BPS_ModelValue As Integer

            BPSField = 1                                         'BPS Field which is the value field
            BPS_CodeField = rasCursor.FindField("BPS_CODE")    'BPS_Code Field
            BPS_ModelField = rasCursor.FindField("BPS_MODEL")  'BPS_Model Field
            BPS_NameField = rasCursor.FindField("BPS_NAME")    'BPS_Name Field

            If BPSField <> -1 And BPS_CodeField <> -1 And BPS_ModelField <> -1 And BPS_NameField <> -1 Then
                'We have the fields needed to update the EVT_EVG_EVS table
                While rasCursor.MoveNext
                    rasRow = rasCursor.Current
                    BPS_CodeValue = 9999
                    BPS_ModelValue = 9999
                    If IsNumeric(rasRow.Item(BPS_CodeField)) Then BPS_CodeValue = rasRow.Item(BPS_CodeField)
                    If IsNumeric(rasRow.Item(BPS_ModelField)) Then BPS_ModelValue = rasRow.Item(BPS_ModelField)

                    strSQL = "SELECT LUT_BPS.BPS " &
                             "FROM(LUT_BPS) " &
                             "GROUP BY LUT_BPS.BPS " &
                             "HAVING (((LUT_BPS.BPS)=" & rasRow.Item(BPSField) & "))"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    If rs1.EOF = False Then 'Update the values for this BPS

                        strSQL = "UPDATE LUT_BPS " &
                                 "SET LUT_BPS.BPS_Code = """ & BPS_CodeValue & """, " &
                                 "LUT_BPS.BPS_Model = " & BPS_ModelValue & ", " &
                                 "LUT_BPS.Name = """ & rasRow.Item(BPS_NameField) & """ " &
                                 "WHERE (((LUT_BPS.BPS)=" & rasRow.Item(BPSField) & "))"
                        dbconn.Execute(strSQL)
                    Else                  'Append a new BPS with values
                        strSQL = "INSERT INTO LUT_BPS ( BPS, BPS_Code, BPS_Model, Name ) " &
                                 "SELECT " & rasRow.Item(BPSField) & " AS Expr1, """ &
                                 BPS_CodeValue & """ AS Expr2, " &
                                 BPS_ModelValue & " AS Expr3, """ &
                                 rasRow.Item(BPS_NameField) & """ AS Expr4 "
                        dbconn.Execute(strSQL)
                    End If

                    rs1.Close()
                End While
            End If

            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing
                If rs2.State <> 0 Then rs2.Close()
                rs2 = Nothing

                If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
            End If
            rasTable.Dispose()
            rasCursor.Dispose()
        Catch ex As Exception
            If dbconn.State <> System.Data.ConnectionState.Closed Then                                 'Database needs to be closed
                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing
                If rs2.State <> 0 Then rs2.Close()
                rs2 = Nothing

                If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
            End If
            rasTable.Dispose()
            rasCursor.Dispose()
            MsgBox("Error in Update_LUT_BPS - " & ex.Message)
        End Try
    End Sub

    Private Sub Update_EVT_EVG_EVS_w_EVTTable(ByVal rasTable As Table) 'Copies ITable to Access in the correct format
        'lblStatus.Visible = True
        'lblStatus.Update()

        Dim EVTField As Integer
        Dim EVTNameField As Integer
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data
        Dim rs3 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Dim rasCursor = rasTable.Search()
        Dim rasRow As Row = Nothing
        Try
            EVTField = rasCursor.FindField("EVT_FUEL")                        'EVT_Fuel Field
            If EVTField = -1 Then EVTField = 1 '                            Value Field if old GRID or no attached attributes
            EVTNameField = rasCursor.FindField("EVT_FUEL_N")                  'EVT Name Field
            If EVTNameField <> -1 Then
                'We have the fields needed to update the EVT_EVG_EVS table
                While rasCursor.MoveNext
                    rasRow = rasCursor.Current
                    strSQL = "SELECT XWALK_EVT_EVG_EVS.EVT " &
                             "FROM(XWALK_EVT_EVG_EVS) " &
                             "GROUP BY XWALK_EVT_EVG_EVS.EVT " &
                             "HAVING (((XWALK_EVT_EVG_EVS.EVT)=" & rasRow.Item(EVTField) & "))"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    If rs1.EOF = False Then 'Update the values for this EVT

                        strSQL = "UPDATE XWALK_EVT_EVG_EVS " &
                                 "SET XWALK_EVT_EVG_EVS.EVT_Name = """ & rasRow.Item(EVTNameField) & """ " &
                                 "WHERE (((XWALK_EVT_EVG_EVS.EVT)=" & rasRow.Item(EVTField) & "))"
                        dbconn.Execute(strSQL)
                    Else                  'Append a new evt with values

                        strSQL = "INSERT INTO XWALK_EVT_EVG_EVS ( EVT, EVT_Name ) " &
                                 "SELECT " & rasRow.Item(EVTField) & " AS Expr1, """ &
                                 rasRow.Item(EVTNameField) & """ AS Expr2"
                        dbconn.Execute(strSQL)
                    End If

                    rs1.Close()
                End While
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            rasTable.Dispose()
            rasCursor.Dispose()
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing

            If dbconn.State <> System.Data.ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in Update_EVT_EVG_EVS_w_EVTTable - " & ex.Message)
            rasTable.Dispose()
            rasCursor.Dispose()
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

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection & strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

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

                   CopyTableToAccess(gs_ProjectPath + "\MU\", MUSaveName, blnMUGRID)

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