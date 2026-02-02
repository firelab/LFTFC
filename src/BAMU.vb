
Imports ArcGIS.Desktop.Framework.Contracts

Friend Class BAMU
    Inherits Button

    Protected Overrides Sub OnClick()
        If gs_validProject = False Then
            MsgBox("Click the LFTFC menu button and select (Set Project Path) before proceeding")
        Else
            'Set map view
            gs_SetActiveLFTFCPane()

            Enabled = True
            AddMU()
        End If
    End Sub

    Private Sub AddMU()
        Try
            Dim frmNewMU As New frmAddMU()
            Enabled = False
            frmNewMU.ShowDialog()
            frmNewMU = Nothing
            Enabled = True

        Catch ex As Exception
            MsgBox("Add MU " & ex.Message)
            Enabled = True
        End Try
    End Sub
End Class

