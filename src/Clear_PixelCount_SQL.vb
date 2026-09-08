Imports System.IO

Imports ArcGIS.Desktop.Framework.Contracts



Friend Class Clear_PixelCount_SQL
    Inherits Button
    Protected Overrides Async Sub OnClick()
        'Set local path variable
        Dim strProjectPath = gs_ProjectPath
        Dim MU = gs_MU
        Dim dbtype = "sql" 'gs_db_type

        ' Toolbox Parameters
        Dim myParams As New List(Of String)
        myParams.Add(strProjectPath) ' project path
        myParams.Add(MU) ' mu
        'myParams.Add(dbtype)

        Dim tool As String = "Clear_Selected_MU"
        Dim thetool As String = Path.Combine(gs_toolboxpath, tool)

        ' Run shared python call
        Await gt_PixelPYT(thetool, myParams, gs_MU())

    End Sub
End Class

