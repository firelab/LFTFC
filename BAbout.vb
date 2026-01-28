
Imports ArcGIS.Desktop.Framework.Contracts

Friend Class BAbout
    Inherits Button

    Protected Overrides Sub OnClick()
        Dim NewfrmAboutLFTFC = New frmAboutLFTFC
        NewfrmAboutLFTFC.Show()
    End Sub
End Class

