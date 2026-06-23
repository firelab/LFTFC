Imports System.IO
Imports System.Windows.Media
Imports ArcGIS.Desktop.Framework.Contracts
Imports ArcGIS.Desktop.Internal.Mapping.Locate.Controls

Friend Class BRaster
    Inherits Button

    Protected Overrides Async Sub OnClick()
        'Set local path variable
        Dim strProjectPath = gs_ProjectPath
        Dim MU = gs_MU

        'Set active map pane
        gs_SetActiveLFTFCPane()

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
                'Open the fuel rules to update the pixel counts
                'Dim FUELRules = New frmRule(gs_MU() + "_CMB", gs_MU() + "_Rulesets", gs_MU())
                'Await FUELRules.StartAsync()
                'FUELRules = Nothing

                'Check for index
                gf_CheckForDBIndex(gs_ProjectPath, gs_MU())

                Dim FUELGrid = New frmGRID()

                ' Hide before running python
                FUELGrid.Hide()

                ' Toolbox Parameters
                Dim myParams As New List(Of String)
                myParams.Add(strProjectPath) ' project path
                myParams.Add(MU) ' mu

                Dim tool As String = "Rules_Setup"
                Dim thetool As String = Path.Combine(gs_toolboxpath, tool)

                ' Run shared python call
                Await gt_PixelPYT(thetool, myParams, gs_MU())

                'Open Create Fuel GRID
                FUELGrid.Show()
                FUELGrid = Nothing
            Catch ex As Exception
                MsgBox("Fuel GRID " + ex.Message)
            End Try
        End If
    End Sub
End Class

