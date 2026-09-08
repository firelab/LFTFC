Imports System.IO

Imports ArcGIS.Desktop.Framework.Contracts


Friend Class Calc_PixelCount_SQL
    Inherits Button

    Protected Overrides Async Sub OnClick()
        'Set local path variable
        Dim strProjectPath = gs_ProjectPath
        Dim MU = gs_MU
        Dim dbtype = "sql" 'gs_db_type

        'Check for project directory
        If gs_validProject = False Then
            If MsgBox("The project path must be set before this function is available." + vbCrLf +
                   " Would you like to set the project path now?", MsgBoxStyle.OkCancel, "!!!!Set project path!!!!") = vbOK Then
                gs_SetProjectDir()
            Else
                'Do nothing
            End If
        Else
            Try
                ' Toolbox Parameters
                Dim myParams As New List(Of String)
                myParams.Add(strProjectPath) ' project path
                myParams.Add(MU) ' mu
                myParams.Add(dbtype)

                Dim tool As String = "Rules_Setup"
                Dim thetool As String = Path.Combine(gs_toolboxpath, tool)

                ' Run shared python call
                Await gt_PixelPYT(thetool, myParams, gs_MU())
            Catch ex As Exception
                MsgBox("Pixel Count " + ex.Message)
            End Try
        End If
    End Sub
End Class

