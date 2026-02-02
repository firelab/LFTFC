Imports System.Windows.Forms

Public Class frmPixelCountUpdate
    Private totalCount As Integer
    Private totalProcessed As Integer = 0
    Private cmdCancel As Boolean = False
    Private updateAmount As Integer

    Public Sub New(inTotalCount As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        'Set variables
        totalCount = inTotalCount

        'Set status
        grpProcess.Text = "Please Wait"

        ' Add any initialization after the InitializeComponent() call.
        'CheckForIllegalCrossThreadCalls = False
    End Sub

    Public Sub ChangeProgress(inUpdateAmount As Integer)
        'Add one to process count
        updateAmount = inUpdateAmount
        totalProcessed = totalProcessed + updateAmount

        Dim percentDone As Double
        percentDone = totalProcessed / totalCount * 100

        prgBarBlue.Value = percentDone
        Application.DoEvents()
    End Sub

    Public Sub ChangeProcessText(updateText As String)
        grpProcess.Text = updateText
    End Sub

    Private Sub cmdPCUCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdPCUCancel.Click
        cmdCancel = True
        grpProcess.Text = "Cancel Submitted"
    End Sub

    Public ReadOnly Property CancelSubmitted() As Boolean
        Get
            CancelSubmitted = cmdCancel
        End Get
    End Property
End Class