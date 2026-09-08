Imports System.Data
Imports System.Data.SQLite
Imports ArcGIS.Desktop.Framework.Contracts


''' <summary>
''' Represents the ComboBox
''' </summary>
''' <remarks></remarks>
Public Class BCMU
    Inherits ComboBox

    Private _isInitialized As Boolean

    '''<summary>
    ''' Combo Box constructor
    '''</summary>
    Public Sub New()
        Enabled = False
    End Sub


    '''<summary>
    ''' Updates the combo box with all the items.
    '''</summary>

    'Private Sub UpdateCombo()
    Protected Overrides Sub OnUpdate()
        'TODO – customize this method to populate the combobox with desired items 
        Try
            'Check for project directory and change flag
            If gs_validProject = False Then
                'Do Nothing
                Exit Sub
            ElseIf (gs_MUCount <> ItemCollection.Count) Or (ItemCollection.Count = 0) Then
                'Reset the MU combobox items

                'Populate/Update the MU combo box
                Dim strSQL As String                                            'SQL variable for this module

                Using conn As New SQLiteConnection("Data Source=" & gs_ProjectPath & "\" & gs_LFTFCSQliteName)
                    conn.Open()

                    ' Clear the ComboBox before adding items
                    Clear()

                    strSQL = "
                    SELECT Name
                    FROM DATA_MU_Name
                    ORDER BY Name;
                "

                    Using cmd As New SQLiteCommand(strSQL, conn)
                        Using reader As SQLiteDataReader = cmd.ExecuteReader()

                            If Not reader.HasRows Then
                                Add(New ComboBoxItem("No MUs available"))
                            Else
                                While reader.Read()
                                    Add(New ComboBoxItem(reader("Name").ToString()))
                                End While
                            End If

                        End Using
                    End Using

                End Using

                'Update the MU count
                gs_MUCount = ItemCollection.Count

                SelectedIndex = 0 'Set as the first in the list
                'gs_MUChange = False

                Enabled = True 'enables the ComboBox
                Exit Sub
            ElseIf ItemCollection.Count <> 0 And IsNothing(SelectedItem) Then
                SelectedIndex = 0 'Set as the first in the list
                Enabled = True 'enables the ComboBox
            End If
        Catch ex As Exception
            MsgBox("Error in LFTFCTBCMU - " & ex.Message & vbCrLf &
                   "Possible solutions" & vbCrLf &
                   "    - SQLite database must be present" &  '"    - Microsoft Office Access needs to be 64 bit." &
                   "    - Project folder must have folders (Input, MU, and Output)" &
                   "    - Project folder must have accessdatabase LF_TFC_toolbar.mdb not .accdb ")
            Clear()
            'Add error to boc
            Add(New ComboBoxItem("Error in loading MUs"))

            gs_validProject = False
        End Try
    End Sub

    ''' <summary>
    ''' The on comboBox selection change event. 
    ''' </summary>
    ''' <param name="item">The newly selected combo box item</param>
    Protected Overrides Sub OnSelectionChange(item As ComboBoxItem)


        If (item Is Nothing) Then
            item = ItemCollection.Item(0)
        Else
            gs_MU() = item.Text
        End If

        ' TODO  Code behavior when selection changes.  
    End Sub

End Class
