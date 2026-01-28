Public Class frmCreateProject
    Private strProjectPath As String

    Public Sub New(ByVal ProjPath As String, ByVal strMissProjPiec As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strProjectPath = ProjPath

        With lblCreateProject
            .Text = strProjectPath & " Is missing these project pieces:" & strMissProjPiec & "." &
                    vbCrLf & "Specify a new project name and create a new project using the default values " &
                    " or import from another project. "
        End With
    End Sub

    Private Sub cmdDefault_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDefault.Click
        Try
            cmdDefault.Text = "Wait"
            cmdCancel.Enabled = False
            cmdDefault.Enabled = False
            cmdImport.Enabled = False

            System.IO.Directory.CreateDirectory(strProjectPath & "\" & txtProjName.Text)
            gs_ProjectPath = strProjectPath & "\" & txtProjName.Text

            FileIO.FileSystem.CopyFile(gs_Install_Path & "\" & gs_LFTFCDBName,
                                gs_ProjectPath() & "\" & gs_LFTFCDBName, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs)

            System.IO.Directory.CreateDirectory(gs_ProjectPath() & "\Input")
            System.IO.Directory.CreateDirectory(gs_ProjectPath() & "\MU")
            System.IO.Directory.CreateDirectory(gs_ProjectPath() & "\Output")

            Close()
            gs_validProject = True

            'Reset the MU count
            gs_MUCount = 0
        Catch ex As Exception
            MsgBox("Error in cmdDefault_Click - " & vbCrLf & ex.Message)

            gs_validProject = False
            cmdDefault.Text = "Default"
            cmdCancel.Enabled = True
            cmdDefault.Enabled = True
            cmdImport.Enabled = True
        End Try
        'Reset the MU count
        gs_MUCount = 0
    End Sub

    Private Sub cmdImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImport.Click
        Try
            cmdImport.Text = "Wait"

            cmdCancel.Enabled = False
            cmdDefault.Enabled = False
            cmdImport.Enabled = False

            Dim FBDialog As New System.Windows.Forms.FolderBrowserDialog
            ' Create the Dialog window
            ' Change the .SelectedPath property to the default location
            With FBDialog
                ' Desktop is the root folder in the dialog.
                .RootFolder = Environment.SpecialFolder.Desktop
                ' Select the C:\ directory on entry.
                .SelectedPath = "c:\"
                ' Prompt the user.
                .Description = "Select Project Directory to import from"
                If .ShowDialog = DialogResult.OK Then

                    If System.IO.Directory.Exists(strProjectPath & "\" & txtProjName.Text) = False Then
                        System.IO.Directory.CreateDirectory(strProjectPath & "\" & txtProjName.Text)
                    Else
                        Throw New System.IO.IOException("Destination directory already exists: " _
                                                        & strProjectPath & "\" & txtProjName.Text)
                    End If

                    gs_ProjectPath = strProjectPath & "\" & txtProjName.Text

                    FileIO.FileSystem.CopyFile(.SelectedPath & "\LF_TFC_Toolbar.mdb", gs_ProjectPath() & "\" &
                                               gs_LFTFCDBName, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs)

                    FileIO.FileSystem.CopyDirectory(.SelectedPath & "\Input", gs_ProjectPath() & "\Input" _
                                                    , Microsoft.VisualBasic.FileIO.UIOption.AllDialogs)
                    FileIO.FileSystem.CopyDirectory(.SelectedPath & "\MU", gs_ProjectPath() & "\MU" _
                                                    , Microsoft.VisualBasic.FileIO.UIOption.AllDialogs)
                    FileIO.FileSystem.CopyDirectory(.SelectedPath & "\Output", gs_ProjectPath() & "\Output" _
                                                    , Microsoft.VisualBasic.FileIO.UIOption.AllDialogs)
                End If
            End With
            Close()
            gs_validProject = True

            'Reset the MU count
            gs_MUCount = 0
        Catch ex As Exception
            gs_validProject = False
            MsgBox("Error in cmdImport_Click - " & vbCrLf & ex.Message)
            cmdImport.Text = "Import"
            cmdCancel.Enabled = True
            cmdDefault.Enabled = True
            cmdImport.Enabled = True
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Try
            Close()
        Catch ex As Exception
            MsgBox("Error in cmdCancel_Click - " & vbCrLf & ex.Message)
        End Try
        gs_validProject = False
    End Sub
End Class