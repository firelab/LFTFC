Public Class frmWorkStatus
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.BringToFront()
    End Sub

    Public Sub UpdateStatus(Status As String)
        Me.lstWorkStatus.Items.Add(Status)
        Me.lstWorkStatus.SelectionMode = System.Windows.Forms.SelectionMode.One
        Me.lstWorkStatus.SelectedIndex = Me.lstWorkStatus.Items.Count - 1
        Me.BringToFront()
        Me.Refresh()
    End Sub

    Public Sub UpdateFormName(frmName As String)
        Me.Text = frmName
        Me.Refresh()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Try
            Close()
        Catch ex As Exception
            MsgBox("Error in cmdCancel_Click - " & ex.Message)
        End Try
    End Sub
End Class