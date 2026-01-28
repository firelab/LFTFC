Imports ArcGIS.Desktop.Framework.Contracts

Friend Class BRule
    Inherits Button

    Protected Overrides Sub OnClick()
        'Check for project directory
        If gs_validProject = False Then
            If MsgBox("The project path must be set before this function is available." + vbCrLf +
                   " Would you like to set the project path now?", MsgBoxStyle.OkCancel, "!!!!Set project path!!!!") = vbOK Then
                gs_SetProjectDir()
            Else
                'Do nothing and leave project directory Not Set
            End If
        Else
            Try
                Dim FUELRule = New frmRule(gs_MU() + "_CMB", gs_MU() + "_Rulesets", gs_MU())

                'Check for index
                gf_CheckForDBIndex(gs_ProjectPath, gs_MU())

                FUELRule.Show()
                FUELRule = Nothing

            Catch ex As Exception
                MsgBox("Fuel Rule " + ex.Message)
            End Try
        End If
    End Sub
End Class

