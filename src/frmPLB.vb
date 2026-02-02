Public Class frmPLB
    'Private MU As String
    'Private Ruleset As String
    Private StrName As String
    Private ButtonContinue As Boolean
    Private strProjectPath As String

    Public Sub New(ByVal inStrName)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        strProjectPath = gs_ProjectPath                                 'get projectpath
        StrName = inStrName
        Text = "Pixels Left Behind - " + StrName
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdContinue.Click
        ButtonContinue = True
        Hide()
    End Sub

    Private Sub cmdSaveList_Click(sender As Object, e As EventArgs) Handles cmdSaveList.Click
        Dim strFileName As String
        strFileName = gs_MU() & "-" & StrName & "-PLB.txt"

        If My.Computer.FileSystem.FileExists(strProjectPath & "\Output\" & strFileName) Then
            Dim result As System.Windows.Forms.DialogResult = System.Windows.Forms.MessageBox.Show("Overwrite " & strFileName & "with new PLB list?",
                                                         "Overwrite?", System.Windows.Forms.MessageBoxButtons.YesNo)
            If result = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If

        Try
            'Make a new log file
            Dim oWrite As New System.IO.StreamWriter(strProjectPath & "\Output\" & strFileName)

            For Each i As String In lstPLB.Items
                oWrite.WriteLine(i)
            Next

            oWrite.Close()

            MsgBox("Pixels left behind are saved in text file: " & vbCrLf &
                        strFileName & vbCrLf & vbCrLf &
                       "Located in project output folder: " & vbCrLf &
                        strProjectPath & "\Output\")
        Catch ex As Exception
            MsgBox("Error creating PLB file." & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        ButtonContinue = False
        Hide()
    End Sub

    Public ReadOnly Property GetAnswer() As Boolean
        Get
            GetAnswer = ButtonContinue
        End Get
    End Property

    Public Sub AddPLB(ByVal inPLB As String)
        lstPLB.Items.Add(inPLB)
    End Sub

    Public ReadOnly Property GetCount() As Integer
        Get
            GetCount = lstPLB.Items.Count
        End Get
    End Property

    Private Sub frmPLB_ResizeEnd(sender As Object, e As EventArgs) Handles MyBase.ResizeEnd
        lstPLB.Height = Height - 67
    End Sub
End Class