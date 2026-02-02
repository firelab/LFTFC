Imports System.Data
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
        'TODO – customize this method to populate the combobox with your desired items 
        Try
            'Check for project directory and change flag
            If gs_validProject = False Then
                'Do Nothing
                Exit Sub
            ElseIf (gs_MUCount <> ItemCollection.Count) Or (ItemCollection.Count = 0) Then
                'Reset the MU combobox items

                'Populate/Update the MU combo box
                Dim strSQL As String                                            'SQL variable for this module
                Dim rs1 As New ADODB.Recordset                                  'recordset for data

                Dim dbconn As New ADODB.Connection                              'DB connection
                dbconn.ConnectionString = gs_DBConnection &
                    gs_ProjectPath & "\" & gs_LFTFCDBName
                dbconn.Open()

                'Clear before adding 
                Clear()

                strSQL = "SELECT Name " &
                             "FROM DATA_MU_Name " &
                             "ORDER BY Name"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                If rs1.EOF = True Then
                    Add(New ComboBoxItem("No MUs available"))
                Else
                    Do While rs1.EOF <> True
                        Add(New ComboBoxItem(rs1.Fields!Name.Value))
                        rs1.MoveNext()
                    Loop
                End If

                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing

                If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing

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
                   "    - Microsoft Office Access needs to be 64 bit." &
                   "    - Project folder must have folders (Input, MU, and Output)" &
                   "    - Project folder must have accessdatabase LF_TFC_toolbar.mdb not .accdb ")
            Clear()
            'Add error to boc
            Add(New ComboBoxItem("Error in loading MUs"))

            gs_validProject = False
        End Try
        'End If
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
