Imports System.IO
Imports ArcGIS.Desktop.Framework
Imports ArcGIS.Desktop.Framework.Contracts
Imports ArcGIS.Desktop.Framework.Threading.Tasks


Friend Class BRule
    Inherits Button

    Protected Overrides Async Sub OnClick()
        'Set local path variable
        Dim strProjectPath = gs_ProjectPath
        Dim MU = gs_MU

        ' Check for project directory
        If gs_validProject = False Then
            If MsgBox("The project path must be set before this function is available." &
                      vbCrLf &
                     " Would you like to set the project path now?",
                     MsgBoxStyle.OkCancel,
                     "!!!!Set project path!!!!") = vbOK Then

                gs_SetProjectDir()
            Else
                Return
            End If
        End If

        Try
            ' Create form object — no async inside constructor
            Dim FUELRule As New frmRule(gs_MU() + "_CMB",
                                        gs_MU() + "_Rulesets",
                                        gs_MU())

            ' Run async initialization AFTER constructed
            Await FUELRule.StartAsync()

        Catch ex As Exception
            MsgBox("Fuel Rule " & ex.Message)
        End Try

    End Sub
End Class

