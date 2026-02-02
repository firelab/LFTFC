Imports ArcGIS.Desktop.Framework.Contracts

Friend Class BWorkingDir
    Inherits Button

    Protected Overrides Sub OnClick()
        gs_SetProjectDir()
        'Add map to project
        gs_SetActiveLFTFCPane()
    End Sub
End Class

