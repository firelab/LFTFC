Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports ArcGIS.Core.CIM
Imports ArcGIS.Core.Data
Imports ArcGIS.Core.Geometry
Imports ArcGIS.Desktop.Catalog
Imports ArcGIS.Desktop.Core
Imports ArcGIS.Desktop.Editing
Imports ArcGIS.Desktop.Extensions
Imports ArcGIS.Desktop.Framework
Imports ArcGIS.Desktop.Framework.Contracts
Imports ArcGIS.Desktop.Framework.Dialogs
Imports ArcGIS.Desktop.Framework.Threading.Tasks
Imports ArcGIS.Desktop.Layouts
Imports ArcGIS.Desktop.Mapping
Imports ArcGIS.Desktop.Core.Geoprocessing


Friend Class LoadCSV
    Inherits Button

    Protected Overrides Sub OnClick()
        'Protected Overrides Async Sub OnClick()
        'Set local path variable
        Dim strProjectPath = gs_ProjectPath
        Dim csvlist = "" 'gs_MU
        'Dim dbtype = gs_db_type

        ' Toolbox Parameters
        Dim myParams As New List(Of String)
        myParams.Add(strProjectPath) ' project path
        myParams.Add(csvlist) ' mu
        'myParams.Add(dbtype)

        Dim tool As String = "import_csv_to_sqlite"
        Dim thetool As String = Path.Combine(gs_toolboxpath, tool)

        ' Run shared python call
        'Await gt_PixelPYT(thetool, myParams, gs_MU())
        ' Call the Geoprocessor with the flag to open the tool dialog in the Geoprocessing pane
        'Dim flag As GPExecuteToolFlags = GPExecuteToolFlags.None
        Geoprocessing.OpenToolDialog(thetool, myParams)
    End Sub
End Class

