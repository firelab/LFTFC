Imports ArcGIS.Desktop.Framework.Contracts

Friend Class BRaster
    Inherits Button

    Protected Overrides Sub OnClick()
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
                Dim FUELRules = New frmRule(gs_MU() + "_CMB", gs_MU() + "_Rulesets", gs_MU())
                FUELRules = Nothing

                'Check for index
                gf_CheckForDBIndex(gs_ProjectPath, gs_MU())

                'Open Create Fuel GRID
                Dim FUELGrid = New frmGRID()
                FUELGrid.Show()
                FUELGrid = Nothing
            Catch ex As Exception
                MsgBox("Fuel GRID " + ex.Message)
            End Try
        End If
    End Sub
End Class

