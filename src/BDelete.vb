Imports ArcGIS.Desktop.Framework.Contracts

Friend Class BDelete
    Inherits Button

    Protected Overrides Sub OnClick()
        gs_SetActiveLFTFCPane()

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
                gf_DeleteMU(gs_ProjectPath)
                'Subtract one to the count to trigger and update change
                gs_MUCount -= 1
                'gs_MUChange = True
            Catch ex As Exception
                MsgBox("FUEL MU Delete " + ex.Message)
            End Try
        End If
    End Sub
End Class

