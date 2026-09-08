
Imports System.Data
Imports System.Data.SQLite
Imports System.Linq
Imports System.IO
Imports System.Windows
Imports ArcGIS.Desktop.Core
Imports ArcGIS.Desktop.Framework.Contracts
Imports ArcGIS.Desktop.Framework.Threading.Tasks
Imports ArcGIS.Desktop.Mapping

Module GeneralSettings
    'Public gs_ThreadCollection As New Collection 'Holds threads to be processed
    Private strProjPath As String = "Not Set"                           'Stores the path
    Private ReadOnly str_LFTFC_DB As String = "\LF_TFC_Toolbar.mdb"     'Stores the name of the database
    Private ReadOnly str_LFTFC_SQL As String = "LFTFC_new.sqlite"     'Stores the name of the SQLite database
    Private ReadOnly strDBConnection As String = "Provider=Microsoft.ACE.OLEDB.12.0;Persist Security Info=False;Data Source=" 'Stores connection string for ADODB
    'Private pHook As ArcGIS.Desktop.Framework.FrameworkApplicationion                 'ArcMap application
    'Private pApp As ESRI.ArcGIS.Framework.IApplication 'ArcMap application
    Private strMU As String 'Stores the MU name
    Private MUCount As Integer = 0                                      'Stores if MU combo box needs updated
    Private strSQL As String                                            'SQL variable for this module
    Private mapLFTFC As Map = Nothing                                   'Stores map
    Private paneLFTFC As Pane = Nothing                                 'Stores map pane
    Private validProject As Boolean = False                             'Flag for valid project
    Public Const DEFAULT_FUEL_VAL As Integer = 9999

    Public ReadOnly Property gs_Install_Path() As String
        Get
            'Make install path constant
            gs_Install_Path = "C:\Landfire\LFTFC_Pro"
        End Get
    End Property

    Public ReadOnly Property gs_db_type() As String
        Get
            'Set Database type mdb or sqlite
            gs_db_type = "mdb"
        End Get
    End Property

    Public ReadOnly Property gs_toolboxpath() As String
        Get
            'Make install path constant
            Dim installPath As String = Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().Location)
            gs_toolboxpath = Path.Combine(installPath, "tools", "SetInitialFuelPixels.pyt")
        End Get
    End Property

    Public Property gs_ProjectPath() As String
        Get
            gs_ProjectPath = strProjPath
        End Get
        Set(ByVal value As String)
            strProjPath = value
        End Set
    End Property

    Public ReadOnly Property gs_LFTFCSQliteName() As String
        Get
            gs_LFTFCSQliteName = str_LFTFC_SQL
        End Get
    End Property

    Public ReadOnly Property gs_LFTFCDBName() As String
        Get
            gs_LFTFCDBName = str_LFTFC_DB
        End Get
    End Property

    Public ReadOnly Property gs_LFTFCDatabase() As String
        Get
            'gs_LFTFCDatabase = gs_ProjectPath() & "\" & gs_LFTFCSQliteName
            gs_LFTFCDatabase = Path.Combine(gs_ProjectPath(), gs_LFTFCSQliteName)
        End Get
    End Property

    Public ReadOnly Property gs_DBConnection() As String
        Get
            gs_DBConnection = strDBConnection
        End Get
    End Property

    Public Property gs_MU() As String
        Get
            gs_MU = strMU
        End Get
        Set(ByVal value As String)
            strMU = value
        End Set
    End Property


    Public Sub gs_EVTPixelCount(ByVal ComboTable As String, ByVal RulesTable As String,
                               ByRef EVTPixelCountCollection As Collection, ByVal ProjPath As String)

        Try
            Using conn As New SQLiteConnection("Data Source=" & gs_ProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                Dim strSQL As String =
                "SELECT EVTR, DIST, SUM(COUNT) AS TotOfCount " &
                "FROM " & ComboTable & " " &
                "GROUP BY EVTR, DIST"

                Using cmd As New SQLiteCommand(strSQL, conn)
                    Using rd As SQLiteDataReader = cmd.ExecuteReader()
                        While rd.Read()
                            Dim tot As Integer = CInt(rd("TotOfCount"))
                            Dim key As String = rd("EVTR").ToString() &
                                           rd("DIST").ToString()

                            EVTPixelCountCollection.Add(tot, key)
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Error in gs_EVTPixelCount - " & ex.Message)
        End Try

    End Sub

    Public Property gs_MUCount() As Integer
        Get
            gs_MUCount = MUCount
        End Get
        Set(ByVal value As Integer)
            MUCount = value
        End Set
    End Property

    Public Property gs_validProject() As Boolean
        Get
            gs_validProject = validProject
        End Get
        Set(ByVal value As Boolean)
            validProject = value
        End Set
    End Property

    Public Function gs_ValidName(ByVal strName As String, ByVal intMaxLen As Integer, ByVal ProjPath As String, ByVal strTestFlag As String, ByRef tifBool As Boolean) As Boolean
        'If strTestFlag = MU then test MU name else test Output names
        Dim strPath As String
        strPath = ProjPath
        gs_ValidName = True

        'make everything lowercase
        strName = LCase(strName)

        'Check for 13 characters or less and if the first character is a number
        If strName = "" Then
            gs_ValidName = False
            Exit Function
        ElseIf Len(strName) > intMaxLen Then
            gs_ValidName = False
            Exit Function
        ElseIf IsNumeric(Left(strName, 1)) Then
            gs_ValidName = False
            Exit Function
        End If

        'Check for characters other than numbers and letters
        Dim i As Integer
        For i = 1 To Len(strName)
            If Asc(Mid(strName, i, 1)) < 97 Or Asc(Mid(strName, i, 1)) > 122 Then ' 97 to 122 is the lc alphabet
                If Asc(Mid(strName, i, 1)) < 48 Or Asc(Mid(strName, i, 1)) > 57 Then '48 to 57 is num 1-9
                    If Asc(Mid(strName, i, 1)) <> 95 Then '95 is an _
                        gs_ValidName = False
                        Exit Function
                    End If
                End If
            End If
        Next i

        'Test for tif filename otherwise it is GRID and does not need extension
        If tifBool Then strName = strName + ".tif"

        'Check to see if a raster already exists in the MU directory
        If strTestFlag = "MU" Then
            If ItemFactory.Instance.CanGetDataset(ItemFactory.Instance.Create(strPath + "\MU\" + strName)) Then
                gs_ValidName = False 'If no error then it already exists

                Exit Function
            End If
        Else 'Check the output folder
            If ItemFactory.Instance.CanGetDataset(ItemFactory.Instance.Create(strPath + "\Output\" + strName)) Then
                gs_ValidName = False 'If no error then it already exists

                Exit Function
            End If
        End If
    End Function

    Public Async Sub gs_SetProjectDir()
        Try
            'gs_MUChange = True
            Dim strMPP As String = "" 'Stores which projects pieces are missing
            ' Create a FolderBrowserDialog object
            Dim FBDialog As New Forms.FolderBrowserDialog
            ' Create the Dialog window
            ' Change the .SelectedPath property to the default location
            With FBDialog
                ' Desktop is the root folder in the dialog.
                .RootFolder = Environment.SpecialFolder.Desktop
                ' Select the C:\ directory on entry.
                .SelectedPath = "c:\"
                ' Prompt the user.
                .Description = "Select a project folder or location for new project."
                If .ShowDialog = System.Windows.Forms.DialogResult.OK Then
                    'Set the new Project path
                    gs_ProjectPath = .SelectedPath

                    'Check for missing project pieces
                    If File.Exists(gs_ProjectPath & "\" & gs_LFTFCSQliteName) = False Then _
                            strMPP = vbCrLf & gs_LFTFCSQliteName
                    'If File.Exists(gs_ProjectPath & "\" & gs_LFTFCDBName) = False Then _
                    '        strMPP = vbCrLf & gs_LFTFCDBName
                    If Directory.Exists(gs_ProjectPath & "\" & "Input") = False Then _
                        strMPP = strMPP & vbCrLf & "- Input Folder"
                    If Directory.Exists(gs_ProjectPath & "\" & "MU") = False Then _
                        strMPP = strMPP & vbCrLf & "- MU Folder"
                    If Directory.Exists(gs_ProjectPath & "\" & "Output") = False Then _
                        strMPP = strMPP & vbCrLf & "- Output Folder"

                    'If pieces are missing create a new project
                    If strMPP <> "" Then
                        Dim NewProject As New frmCreateProject(gs_ProjectPath, strMPP)
                        NewProject.ShowDialog()
                    Else
                        gs_validProject = True
                    End If
                End If
                'Reset the MU count
                MUCount = 0
            End With

            'Add folder connection to the project
            If (Directory.Exists(gs_ProjectPath)) Then
                Await QueuedTask.Run(
                Sub()
                    Dim item = TryCast(ItemFactory.Instance.Create(gs_ProjectPath), IProjectItem)
                    Project.Current.AddItem(item)
                    MessageBox.Show("A folder connection has been added for this LFTFC project")
                End Sub)
            End If

        Catch ex As Exception
            gs_validProject = False
            MsgBox("Project Directory " & ex.Message)
        End Try
    End Sub

    Public ReadOnly Property gs_Map As Map
        Get
            gs_Map = mapLFTFC
        End Get
    End Property

    Public ReadOnly Property gs_Pane As Pane
        Get
            'Make install path constant
            gs_Pane = paneLFTFC
        End Get
    End Property

    Private Const LFTFC_MAP_NAME As String = "LFTFC_Pro_map"

    Public Async Sub gs_SetActiveLFTFCPane()
        Try
            'mapLFTFC is module state, so it is Nothing again every time Pro restarts. A
            'project saved with the map already in it therefore fell through to CreateMap,
            'and Pro renamed the duplicate to LFTFC_Pro_map1. Look the map up in the
            'project first and only create one when it genuinely is not there.
            If mapLFTFC Is Nothing Then
                mapLFTFC = Await QueuedTask.Run(
                    Function()
                        Dim mapItem As MapProjectItem = Project.Current.GetItems(Of MapProjectItem)().FirstOrDefault(
                            Function(mi) String.Equals(mi.Name, LFTFC_MAP_NAME, StringComparison.OrdinalIgnoreCase))

                        If mapItem IsNot Nothing Then
                            Return mapItem.GetMap()     'Reuse the map already in the project
                        End If

                        Return MapFactory.Instance.CreateMap(LFTFC_MAP_NAME, basemap:=Basemap.None)
                    End Function)
            End If

            'Pane work belongs on the UI thread, not the MCT. Awaiting CreateMapPaneAsync
            'also gives us the real pane instead of whatever ActivePane happened to be
            'while the pane was still being created.
            Dim mapPane As IMapPane = mapLFTFC.GetMapPanes().FirstOrDefault()

            If mapPane Is Nothing Then
                mapPane = Await ProApp.Panes.CreateMapPaneAsync(mapLFTFC)
            End If

            paneLFTFC = TryCast(mapPane, Pane)
            If paneLFTFC IsNot Nothing Then paneLFTFC.Activate()

        Catch ex As Exception
            MsgBox("Error in gs_SetActiveLFTFCPane - " & ex.Message)
        End Try
    End Sub
End Module

