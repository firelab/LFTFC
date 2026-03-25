'System.Windows.Forms.DataVisualization.Charting
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports FastReport.DataVisualization.Charting

Public Class frmRule
    Private strSQL As String                                                        'SQL variable for this module
    Private strTempEVT As String                                                    'Stores the current EVT value before clearing the cmbEVT combobox
    Private comboR As String                                                        'Stores the combo table name for rule making
    Private rulesR As String                                                        'Stores the rules table name for rule making
    Private EVTPixelCountCollection As Collection
    Private RulesetCollection As Collection
    Private strProjectPath As String
    Private strAutoRuleSF As String                                                 'Stores the autorule surface fuel name
    Private strCMSItem As String                                                    'Stores the list box column item when right click rule
    Private ruleE As clsRule                                                        'The selected rule to Edit
    Private chrtDist As Chart
    Private chrtCompFM As Chart
    Private startIntervalMarque As Date = Date.Now                                  'Stores start time

    Public Sub New(ByVal setComboR As String, ByVal setRulesR As String, ByVal SetMUName As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'Set local path variable
        strProjectPath = gs_ProjectPath

        'Declare variables
        Dim LUT_Table As String 'Set the look up table
        Dim LUT_Name As String 'Set the lookup name field
        Dim LUT_Num As String 'Set the lookup number field
        Dim PCUprocessed As Integer 'Stores how have rules have been processed
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            TTSession.SetToolTip(txtSessionName, "This puts a name in the change notes for each fuel rule")

            'Add Compare FM Graph chart and area
            CreateChrtCompFM()

            'Add Distribution Graph chart and area
            CreateChrtDist()

            'Set the collections to new collections
            RulesetCollection = New Collection
            EVTPixelCountCollection = New Collection
            m_ColCW = New Collection

            'Set the MU
            comboR = setComboR
            rulesR = setRulesR
            Text = "Fuel Rules for MU " & SetMUName

            gs_EVTPixelCount(comboR, rulesR, EVTPixelCountCollection, strProjectPath) 'Totals count of pixels/evt and stores them in m_EVTPixelCountCollection

            'Populate cmbSortEVT
            With cmbSortRules
                .Items.Add("All by Type")
                .Items.Add("All by EVT")
                .Items.Add("Disturbed by Type")
                .Items.Add("Disturbed by EVT")
                .Items.Add("Specific EVT")
            End With

            strSQL = "SELECT LUT_DistCode.Type " &
                     "FROM " & comboR & " INNER JOIN LUT_DistCode ON " & comboR & ".DIST = LUT_DistCode.DistCode " &
                     "GROUP BY LUT_DistCode.Type " &
                     "ORDER BY Max([Lut_DistCode]![ID])"
            gf_SetControl(cmbSortRules, strSQL, strProjectPath)
            cmbSortRules.SelectedIndex = 0


            'Start cmbEVT filled with "All By Type"
            LUT_Table = "XWALK_EVT_EVG_EVS"
            LUT_Name = "EVT_Name"
            LUT_Num = "EVT"

            strSQL = "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                     "FROM " & comboR & " LEFT JOIN " & LUT_Table & " " &
                     "ON " & comboR & ".EVTR = " & LUT_Table & "." & LUT_Num & " " &
                     "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                     "ORDER BY " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name

            cmbEVT.Items.Clear()
            gf_SetControl(cmbEVT, strSQL, strProjectPath, rdoName.Checked)
            cmbEVT.SelectedIndex = 0

            '**************Check all rules for pixel count and calculate if needed
            '****Get all EVTs and DISTs for all the rules that exist in the CMB
            strSQL = "SELECT " & rulesR & ".EVT, " & rulesR & ".DIST, " & rulesR & ".OnOff, " & rulesR & ".PixelCount " &
                     "FROM " & comboR & " INNER JOIN " & rulesR & " ON (" & comboR & ".EVTR = " & rulesR & ".EVT) " &
                     "AND (" & comboR & ".DIST = " & rulesR & ".DIST) " &
                     "GROUP BY " & rulesR & ".EVT, " & rulesR & ".DIST, " & rulesR & ".OnOff, " & rulesR & ".PixelCount " &
                     "HAVING (((" & rulesR & ".OnOff)='On') AND ((" & rulesR & ".PixelCount)='')) OR (((" & rulesR & ".PixelCount) Is Null));"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            '****Get an instance of the progress bar for pixel update
            Dim frmPixelUpdate As New frmPixelCountUpdate(rs1.RecordCount)
            frmPixelUpdate.Show()

            '****Run through all of the records and calc pixels where needed
            PCUprocessed = 0

            Do Until rs1.EOF = True
                PCUprocessed = PCUprocessed + 1

                frmPixelUpdate.ChangeProcessText("EVT: " & rs1.Fields!EVT.Value & "   Dist: " & rs1.Fields!DIST.Value)
                gr_MakeRuleset(rs1.Fields!EVT.Value, rs1.Fields!DIST.Value, comboR, rulesR,
                              RulesetCollection, EVTPixelCountCollection, strProjectPath)
                'Update progress
                If PCUprocessed = 5 Then
                    'Change progress
                    frmPixelUpdate.ChangeProgress(PCUprocessed)
                    PCUprocessed = 0

                    'Check for cancel
                    If frmPixelUpdate.CancelSubmitted = True Then
                        Exit Do
                    End If
                End If

                rs1.MoveNext()
            Loop

            'Close pixel update form
            frmPixelUpdate.Close()
            frmPixelUpdate = Nothing
            '**************Finished

            'Make rulesets and display them
            gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                          RulesetCollection, EVTPixelCountCollection, strProjectPath)

            DisplayRuleset()
            AdjPer()

            'Set txtNotes
            With txtNotes
                .Text = ""
                .Enabled = False 'Remains disabled so changes can't be made until a rule is selected
            End With

            'Set txtEVTDescription
            With txtEVTDescription
                .Text = ""
            End With

            'Set Compare Fuel Models
            InitCompareFM()

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in frmRule New - " & ex.Message)
        End Try
    End Sub

    Private Sub frmFUEL_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.ResizeEnd
        Try
            'Adjust widths and heights according to user new adjustment
            TabControl.Width = Width - 5
            Ruleset.Width = Width - 5
            lstVwRulesets.Width = Width - 150
            txtNotes.Width = Width - 150
            DistributionGraph.Width = Width
            chrtDist.Width = Width - 150
            chrtCompFM.Width = Width - 150
            CompareFM.Width = Width
            EVTDescription.Width = Width - 150
            txtEVTDescription.Width = Width - 150
            lblPixelsLeftOver.Width = Width - 150
            cmbEVT.Width = Width - 20

            TabControl.Height = Height - 122
            Ruleset.Height = TabControl.Height - 36
            lstVwRulesets.Height = Ruleset.Height - txtNotes.Height - 55
            txtNotes.Top = lstVwRulesets.Top + lstVwRulesets.Height + 6
            DistributionGraph.Height = TabControl.Height - 36
            CompareFM.Height = TabControl.Height - 36
            chrtDist.Height = DistributionGraph.Height - 150
            chrtCompFM.Height = DistributionGraph.Height - 90
            EVTDescription.Height = TabControl.Height - 36
            txtEVTDescription.Height = EVTDescription.Height - 5
            'grpCBHCBD.Top = TabControl.Height - 65
            AdjPer()

            Refresh()
        Catch ex As Exception
            MsgBox("Error in frmFUEL_ResizeEND - " & ex.Message)
        End Try
    End Sub

    Private Sub cmbEVT_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbEVT.SelectionChangeCommitted
        Try
            'Update tabs
            SetTabs()
        Catch ex As Exception
            MsgBox("Error in cmbEVT_SelectionChangeCommitted - " & ex.Message)
        End Try
    End Sub

    Private Sub cmbBPSGraph_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBPSGraph.SelectionChangeCommitted
        Try
            'Update Wildcard
            gf_PopWild(cmbWildGraph, gf_GetNum(cmbEVT.SelectedItem, "EVT"), gf_GetNum(cmbEVT.SelectedItem, "DIST"),
                        gf_GetNum(cmbBPSGraph.SelectedItem, "General"), comboR, strProjectPath)
            DistGraph() 'Create the graph
        Catch ex As Exception
            MsgBox("Error in cmbBPSGraph_SelectionChangeCommitted - " & ex.Message)
        End Try
    End Sub

    Private Sub cmbWildGraph_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbWildGraph.SelectionChangeCommitted
        Try
            'Update DistGraph
            DistGraph() 'Create the graph
        Catch ex As Exception
            MsgBox("Error in cmbWildGraph_SelectionChangeCommitted - " & ex.Message)
        End Try
    End Sub

    Private Sub lstVwRulesets_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lstVwRulesets.ColumnClick
        Try
            If lstVwRulesets.Columns.Item(e.Column).Width > 7 Then
                lstVwRulesets.Columns.Item(e.Column).Width = 7
            Else
                lstVwRulesets.Columns.Item(e.Column).Width = -2
            End If
        Catch ex As Exception
            MsgBox("Error in lstVwRulesets_ColumnClick - " & ex.Message)
        End Try
    End Sub

    Private Sub lstVwRulesets_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstVwRulesets.MouseClick
        Try
            txtNotes.Text = RulesetCollection(lstVwRulesets.SelectedIndices.Item(0) + 1).Notes & ""
            txtNotes.Enabled = True 'Unlock once a rule has been selected
        Catch ex As Exception
            MsgBox("Error in lstVwRulesets_MouseClick - " & ex.Message)
        End Try

    End Sub

    Private Sub txtNotes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNotes.DoubleClick
        Try
            'Adds time add date on enter before adding notes if a rule has been selected first
            If txtNotes.Enabled = True Then
                txtNotes.Text = txtNotes.Text & Now.ToShortTimeString & " " & Now.ToShortDateString &
                                "  " & txtSessionName.Text & ":"
            End If
        Catch ex As Exception
            MsgBox("Error in txtNotes_DoubleClick - " & ex.Message)
        End Try

    End Sub

    Private Sub txtNotes_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNotes.LostFocus
        Try
            'Adds note changes to the ruleset collection then the collection updates the database
            If txtNotes.Enabled = True Then
                If RulesetCollection(lstVwRulesets.SelectedIndices.Item(0) + 1).Notes <>
                txtNotes.Text Then
                    'Check for invalid characters (' and ")
                    Dim Valid As Boolean
                    Valid = True
                    Dim i As Integer
                    For i = 1 To Len(txtNotes.Text)
                        If Asc(Mid(txtNotes.Text, i, 1)) = 39 Or Asc(Mid(txtNotes.Text, i, 1)) = 34 Then
                            Valid = False
                        End If
                    Next i
                    If Valid = False Then
                        MsgBox("Retype your notes without (apostrophies or quotation marks)")
                    Else
                        RulesetCollection(lstVwRulesets.SelectedIndices.Item(0) + 1).Notes =
                        txtNotes.Text
                    End If
                End If
            End If
            txtNotes.Enabled = False 'Lock after changes have been made
        Catch ex As Exception
            MsgBox("Error in txtNotes_LostFocus - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdAddRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddRule.Click
        If IsEVTSelected() Then
            Try
                Dim AddRule = New frmAddEdit("Add", gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), 0,
                                             txtSessionName.Text, rulesR, comboR, RulesetCollection, strProjectPath)

                AddRule.ShowDialog()

                System.Threading.Thread.Sleep(1000) 'Let the copy query catchup

                gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                              RulesetCollection, EVTPixelCountCollection, strProjectPath)
                DisplayRuleset()
                AdjPer()

                AddRule = Nothing
                txtNotes.Enabled = False
                Refresh()
            Catch ex As Exception
                MsgBox("Error in cmdAddRule_Click - " & ex.Message)
            End Try
        End If

    End Sub

    Private Sub cmdEditRule_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEditRule.Click
        If IsEVTSelected() Then
            Try
                'Check that a rule has been selected before clicking Edit button
                If lstVwRulesets.SelectedItems.Count = 0 Then
                    MsgBox("Select a record.")
                Else
                    Dim EditRule = New frmAddEdit("Edit", gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"),
                                                 lstVwRulesets.SelectedItems(0).Index + 1, txtSessionName.Text,
                                                 rulesR, comboR, RulesetCollection, strProjectPath)
                    EditRule.ShowDialog()

                    System.Threading.Thread.Sleep(1000) 'Let the copy query catchup

                    gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                                  RulesetCollection, EVTPixelCountCollection, strProjectPath)
                    DisplayRuleset()
                    AdjPer()

                    EditRule = Nothing
                    txtNotes.Enabled = False
                    Refresh()
                End If
            Catch ex As Exception
                MsgBox("Error in cmdEditRule_Click - " & ex.Message)
            End Try
        End If

    End Sub

    Private Sub cmdDeleteRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteRule.Click

        If IsEVTSelected() Then
            Dim rs1 As New ADODB.Recordset                                  'recordset for data

            Dim dbconn As New ADODB.Connection                              'DB connection
            dbconn.ConnectionString = gs_DBConnection &
            strProjectPath & "\" & gs_LFTFCDBName
            dbconn.Open()

            Try
                If lstVwRulesets.SelectedItems.Count = 0 Then
                    MsgBox("You must first select a rule to delete.")
                ElseIf MsgBox("Do you really want to delete this rule." & vbCrLf &
                    "If you turn the rule off you can keep it for notes" & vbCrLf &
                    "and it won't be included in your analysis.", vbYesNo, "Delete Rule?") = vbYes Then

                    'Remove rule from database
                    Dim i As Integer 'Use to count
                    i = 0
                    Do Until i = lstVwRulesets.SelectedItems.Count
                        strSQL = "DELETE FROM " & rulesR & " " &
                                "WHERE Id = " & RulesetCollection.Item(lstVwRulesets.SelectedItems(i).Index + 1).Id
                        dbconn.Execute(strSQL)

                        i = i + 1
                    Loop

                    'Close DB before moving to other operations
                    If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                    dbconn = Nothing

                    'Remove rule from Ruleset collection
                    gr_ClearPAP(RulesetCollection) 'Clears the pixel count, acres, and percent evt of the ruleset

                    gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                                  RulesetCollection, EVTPixelCountCollection, strProjectPath)
                    DisplayRuleset()
                    AdjPer()
                End If

            Catch ex As Exception

                If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
                MsgBox("Error in cmdDeleteRule_Click - " & ex.Message)
            End Try
        End If

        txtNotes.Enabled = False
        Refresh()
    End Sub

    Private Sub cmdAutoRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAutoRule.Click
        If IsEVTSelected() Then
            Dim AutoRule = New frmAutoRule(comboR, rulesR, strProjectPath, cmbEVT.Text, txtSessionName.Text,
                                           RulesetCollection, EVTPixelCountCollection)
            If strAutoRuleSF = Nothing Then
                AutoRule.ShowDialog()
                strAutoRuleSF = AutoRule.GetAutoRuleCheck()
            Else
                AutoRule.SetAutoRuleCheck(strAutoRuleSF)
                AutoRule.AutoRule()
            End If

            gr_ClearPAP(RulesetCollection) 'Clears the pixel count, acres, and percent evt of the ruleset

            gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                          RulesetCollection, EVTPixelCountCollection, strProjectPath)
            DisplayRuleset()
            AdjPer()
        End If
    End Sub

    Private Sub cmdCopyRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopyRule.Click
        If IsEVTSelected() Then
            Try
                Dim CopyRule As New frmCopyRule(cmbEVT, txtSessionName.Text, rulesR, comboR, strProjectPath, rdoName.Checked)

                CopyRule.ShowDialog()

                System.Threading.Thread.Sleep(1000) 'Let the copy query catchup

                gr_ClearPAP(RulesetCollection) 'Clears the pixel count, acres, and percent evt of the ruleset

                gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                              RulesetCollection, EVTPixelCountCollection, strProjectPath)

                DisplayRuleset()

                AdjPer()

                Refresh()

            Catch ex As Exception
                MsgBox("Error in cmdCopyRule_Click - " & ex.Message)
            End Try
        End If

    End Sub

    Private Sub TabControl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TabControl.KeyPress
        Try
            If TabControl.SelectedIndex = 0 Then
                Select Case Asc(e.KeyChar)
                    Case Is = 21 : Call cmdAutoRule_Click(sender, e)
                    Case Is = 1 : Call cmdAddRule_Click(sender, e)
                    Case Is = 5 : Call cmdEditRule_Click(sender, e)
                    Case Is = 4 : Call cmdDeleteRule_Click(sender, e)
                    Case Is = 3 : Call cmdCopyRule_Click(sender, e)
                End Select
            End If
        Catch ex As Exception
            MsgBox("Error in TabControl_KeyPress - " & ex.Message)
        End Try

    End Sub

    Private Sub TabControl_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabControl.MouseClick

        grpCanopyLines.Visible = False   'Don't show this group to start with

        If IsEVTSelected() Then
            Dim rs1 As New ADODB.Recordset                                  'recordset for data

            Dim dbconn As New ADODB.Connection                              'DB connection
            dbconn.ConnectionString = gs_DBConnection &
            strProjectPath & "\" & gs_LFTFCDBName
            dbconn.Open()

            Try
                Dim EOFFlag As Boolean 'Flags if the EVT exists

                'Check to see the the selected EVT in the combobox exists in CMB for the specific zone
                strSQL = "SELECT EVTR, DIST " &
                    "FROM " & comboR & " " &
                    "WHERE (EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") & " And DIST = " & gf_GetNum(cmbEVT.Text, "DIST") & ")"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                EOFFlag = rs1.EOF

                If EOFFlag = False Then
                    'Populate the active tab If not EOF it acts normal else clear values and do nothing else
                    If TabControl.SelectedIndex = 0 Then
                        gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                                      RulesetCollection, EVTPixelCountCollection, strProjectPath)
                        DisplayRuleset()
                        AdjPer()
                        cmdAutoRule.Enabled = False             'Disabled. Not in use anymore
                        cmdAddRule.Enabled = True
                        cmdCopyRule.Enabled = True
                        cmdDeleteRule.Enabled = True
                        cmdEditRule.Enabled = True
                    ElseIf TabControl.SelectedIndex = 2 Then
                        cmdAutoRule.Enabled = False
                        cmdAddRule.Enabled = False
                        cmdCopyRule.Enabled = False
                        cmdDeleteRule.Enabled = False
                        cmdEditRule.Enabled = False

                        gf_PopBPS(cmbBPSGraph, gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"),
                                                             comboR, strProjectPath)
                        gf_PopWild(cmbWildGraph, gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"),
                        gf_GetNum(cmbBPSGraph.Text, "General"), comboR, strProjectPath)
                        grpCanopyLines.Visible = True                               'Show the group cbh cbd option

                        DistGraph()                                                 'Create the graph
                    ElseIf TabControl.SelectedIndex = 3 Then
                        cmdAutoRule.Enabled = False
                        cmdAddRule.Enabled = False
                        cmdCopyRule.Enabled = False
                        cmdDeleteRule.Enabled = False
                        cmdEditRule.Enabled = False
                        GetEVTDescription() 'Get EVT Description and set txtEVTDescription box
                    End If
                Else
                    lstVwRulesets.Items.Clear()
                    chrtDist.Visible = False
                End If

                If TabControl.SelectedIndex = 1 Then
                    cmdAutoRule.Enabled = False
                    cmdAddRule.Enabled = False
                    cmdCopyRule.Enabled = False
                    cmdDeleteRule.Enabled = False
                    cmdEditRule.Enabled = False
                    InitCompareFM()
                    GraphCompareFM()
                End If

                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing

                If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
            Catch ex As Exception
                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing

                If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
                MsgBox("Error in TabControl_MouseClick - " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub SetTabs()
        Dim EOFFlag As Boolean                                          'Flags if the EVT exists
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()
        Try

            grpCanopyLines.Visible = False   'Don't show this group to start with

            If IsEVTSelected() Then
                'Check to see the the selected EVT in the combobox exists in CMB for the specific zone
                strSQL = "SELECT EVTR, DIST " &
                         "FROM " & comboR & " " &
                         "WHERE (EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") & " And DIST = " & gf_GetNum(cmbEVT.Text, "DIST") & ")"
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                EOFFlag = rs1.EOF

                If EOFFlag = False Then

                    lblDistCode.Text = "Not disturbed"

                    'Get Disturbance, Severity, and Time Since Disturbance and change the evt tooltip to reflect it
                    If gf_GetNum(cmbEVT.Text, "DIST") > 0 Then
                        strSQL = "SELECT Description " &
                            "FROM LUT_DistCode " &
                            "WHERE DistCode = " & gf_GetNum(cmbEVT.Text, "DIST")
                        rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                        If rs2.EOF = False Then
                            lblDistCode.Text = gf_GetNum(cmbEVT.Text, "DIST") & " " & rs2.Fields!Description.Value
                        Else
                            'Nothing
                        End If
                    End If

                    'Populate the active tab If not EOF it acts normal else clear values and do nothing else
                    If TabControl.SelectedIndex = 0 Then
                        gr_MakeRuleset(gf_GetNum(cmbEVT.SelectedItem, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                                      RulesetCollection, EVTPixelCountCollection, strProjectPath)
                        DisplayRuleset()
                        AdjPer()
                    ElseIf TabControl.SelectedIndex = 2 Then
                        gf_PopBPS(cmbBPSGraph, gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"),
                                                             comboR, strProjectPath)
                        gf_PopWild(cmbWildGraph, gf_GetNum(cmbEVT.SelectedItem, "EVT"), gf_GetNum(cmbEVT.SelectedItem, "DIST"),
                        gf_GetNum(cmbBPSGraph.SelectedItem, "General"), comboR, strProjectPath)

                        grpCanopyLines.Visible = True    'Show the group cbh cbd option

                        DistGraph()                     'Create the graph

                    ElseIf TabControl.SelectedIndex = 3 Then
                        GetEVTDescription()             'Get EVT Description and set txtEVTDescription box
                    End If
                Else
                    lstVwRulesets.Items.Clear()
                    chrtDist.Visible = False
                End If
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in SetTabs - " & ex.Message)
        End Try
    End Sub

    Private Sub DisplayRuleset()
        'Clear Rulesets and notes before displaying
        lstVwRulesets.Items.Clear()
        txtNotes.Clear()
        Refresh()

        'Display the ruleset from the ruleset collection
        Dim oLI As ListViewItem
        For i = 1 To RulesetCollection.Count
            oLI = lstVwRulesets.Items.Add(RulesetCollection.Item(i).Id)
            With oLI.SubItems
                If IsNumeric(RulesetCollection.Item(i).StrCovLow) Then
                    .Add(RulesetCollection.Item(i).StrCovLow & " - " & RulesetCollection.Item(i).StrCovHigh)
                    .Add(RulesetCollection.Item(i).StrHgtLow & " - " & RulesetCollection.Item(i).StrHgtHigh)
                Else
                    .Add(Strings.Right(RulesetCollection.Item(i).StrCovLow,
                                                            Len(RulesetCollection.Item(i).StrCovLow) - 2) &
                                                            RulesetCollection.Item(i).StrCovHigh)
                    .Add(Strings.Right(RulesetCollection.Item(i).StrHgtLow,
                                            Len(RulesetCollection.Item(i).StrHgtLow) - 2) &
                                            RulesetCollection.Item(i).StrHgtHigh)
                End If
                .Add(RulesetCollection.Item(i).BPS)
                .Add(RulesetCollection.Item(i).Wildcard)
                .Add(RulesetCollection.Item(i).FBFM13)
                .Add(RulesetCollection.Item(i).FBFM40)
                .Add(RulesetCollection.Item(i).CANFM)
                .Add(RulesetCollection.Item(i).FCCS)
                .Add(RulesetCollection.Item(i).FLM)
                .Add(RulesetCollection.Item(i).Canopy)
                .Add(RulesetCollection.Item(i).CCover)
                .Add(RulesetCollection.Item(i).CHeight)
                .Add(RulesetCollection.Item(i).CBD13)
                .Add(RulesetCollection.Item(i).CBD40)
                .Add(RulesetCollection.Item(i).CBH13)
                .Add(RulesetCollection.Item(i).CBH40)
                .Add(RulesetCollection.Item(i).OnOff)
                If RulesetCollection.Item(i).OnOff = "On" Then
                    .Add(RulesetCollection.Item(i).Acres)
                    .Add(RulesetCollection.Item(i).EvtPer)
                End If
            End With
            With lstVwRulesets.Items
                .Item(i - 1).SubItems.Item(1).Tag = "CovLH"
                .Item(i - 1).SubItems.Item(2).Tag = "HgtLH"
                .Item(i - 1).SubItems.Item(3).Tag = "BPS"
                .Item(i - 1).SubItems.Item(4).Tag = "Wild"
                .Item(i - 1).SubItems.Item(5).Tag = "FM13"
                .Item(i - 1).SubItems.Item(6).Tag = "FM40"
                .Item(i - 1).SubItems.Item(7).Tag = "CanFM"
                .Item(i - 1).SubItems.Item(8).Tag = "FCCS"
                .Item(i - 1).SubItems.Item(9).Tag = "FLM"
                .Item(i - 1).SubItems.Item(10).Tag = "CG"
                .Item(i - 1).SubItems.Item(11).Tag = "CC"
                .Item(i - 1).SubItems.Item(12).Tag = "CH"
                .Item(i - 1).SubItems.Item(13).Tag = "CBD13"
                .Item(i - 1).SubItems.Item(14).Tag = "CBD40"
                .Item(i - 1).SubItems.Item(15).Tag = "CBH13"
                .Item(i - 1).SubItems.Item(16).Tag = "CBH40"
                .Item(i - 1).SubItems.Item(17).Tag = "OnOff"
            End With
        Next i
    End Sub

    Private Sub AdjPer()
        Dim lngPixelsPerRulesetAll As Long 'Stores the total number of pixels affected per ruleset.
        Dim lngPixelsPerRulesetB As Long 'Stores the total number of pixels affected per ruleset with BPS.
        Dim lngPixelsPerRulesetW As Long 'Stores the total number of pixels affected per ruleset with Wildcard.
        Dim lngPixelsPerRulesetBW As Long 'Stores the total number of pixels affected per ruleset with BPS and Wildcard.
        Dim lngTotPixelsPerEVT As Long 'Stores the total pixel count per evt from the collection.

        'Assign values to variables
        For i = 1 To RulesetCollection.Count
            'If OnOff = Off do not display the acre or percent value
            If RulesetCollection.Item(i).OnOff = "On" Then
                'This separates the overall rule from the special BPS and Wildcard rules
                If RulesetCollection.Item(i).BPS <> "any" And RulesetCollection.Item(i).Wildcard <> "any" Then 'BPS and Wildcard Rule
                    'Sum the pixels the rules apply to so you can divide it by the total pixel count to get the percent
                    lngPixelsPerRulesetBW = lngPixelsPerRulesetBW + Int(RulesetCollection.Item(i).PixelCount)
                ElseIf RulesetCollection.Item(i).BPS = "any" And RulesetCollection.Item(i).Wildcard <> "any" Then 'Wildcard Rule
                    'Sum the pixels the rules apply to so you can divide it by the total pixel count to get the percent
                    lngPixelsPerRulesetW = lngPixelsPerRulesetW + Int(RulesetCollection.Item(i).PixelCount)
                ElseIf RulesetCollection.Item(i).BPS <> "any" And RulesetCollection.Item(i).Wildcard = "any" Then 'BPS Rule
                    'Sum the pixels the rules apply to so you can divide it by the total pixel count to get the percent
                    lngPixelsPerRulesetB = lngPixelsPerRulesetB + Int(RulesetCollection.Item(i).PixelCount)
                Else 'Core all rule, any BPS and any Wildcard
                    'Sum the pixels the rules apply to so you can divide it by the total pixel count to get the percent
                    lngPixelsPerRulesetAll = lngPixelsPerRulesetAll + Int(RulesetCollection.Item(i).PixelCount)
                End If
            End If
        Next i

        lngTotPixelsPerEVT = EVTPixelCountCollection(gf_GetNum(cmbEVT.Text, "EVT") & gf_GetNum(cmbEVT.Text, "DIST")) 'Get Total pixels per EVT

        'This displays the count and percent EVT for rules that are EVT only, no BPS or Wildcard
        If Math.Round(lngPixelsPerRulesetAll / lngTotPixelsPerEVT * 100, 3) > 100 Then
            'If percentage is over 100% it displays "Over 100%"
            lblPixelsLeftOver.Text = "Two or more rules are overlapping in Cover and/or Height!"
            lblPixelsLeftOver.BackColor = Color.Red
        Else
            'Calcs the total pixels left over not assigned to rules
            If (lngTotPixelsPerEVT - (lngPixelsPerRulesetAll + lngPixelsPerRulesetB _
                                      + lngPixelsPerRulesetW + lngPixelsPerRulesetBW) = 0) Then
                With lblPixelsLeftOver
                    .BackColor = Color.PaleGreen
                    .Text = "No pixels are left behind."
                End With
            Else
                lblPixelsLeftOver.BackColor = Color.Red
                lblPixelsLeftOver.Text = "Pixels left behind: " & lngTotPixelsPerEVT - (lngPixelsPerRulesetAll _
                                        + lngPixelsPerRulesetB + lngPixelsPerRulesetW + lngPixelsPerRulesetBW)
            End If
        End If
    End Sub

    Public Sub GetEVTDescription()
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        txtEVTDescription.Text = "See Landfire.gov website for updated vegetation descriptions."

        'Try
        '    If IsEVTSelected() Then
        '        'Selects the Description for the given EVT number
        '        strSQL = "SELECT XWALK_EVT_EVG_EVS.Description " &
        '                "FROM XWALK_EVT_EVG_EVS " &
        '                "WHERE ((Mid([XWALK_EVT_EVG_EVS]![EVT],2,3)=" & Strings.Mid(gf_GetNum(cmbEVT.SelectedItem, "EVT"), 2, 3) & "))"
        '        rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

        '        If rs1.EOF = False Then
        '            txtEVTDescription.Text = rs1.Fields(0).Value & ""
        '        Else
        '            txtEVTDescription.Text = ""
        '        End If
        '    End If

        '    If rs1.State <> 0 Then rs1.Close()
        '    rs1 = Nothing

        '    If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
        '    dbconn = Nothing
        'Catch ex As Exception
        '    If rs1.State <> 0 Then rs1.Close()
        '    rs1 = Nothing

        '    If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
        '    dbconn = Nothing

        '    MsgBox("Error in GetEVTDescription - " & ex.Message)
        'End Try
    End Sub

    Private Sub InitCompareFM()
        cmbSlope.Items.Clear()
        cmbFM1.Items.Clear()
        cmbFM2.Items.Clear()
        cmbFM3.Items.Clear()
        cmbFM4.Items.Clear()
        cmbDefaultFM.Items.Clear()

        'Populate Compare Fuel Model comboboxs
        cmbFM1.Items.Add("None")
        cmbFM2.Items.Add("None")
        cmbFM3.Items.Add("None")
        cmbFM4.Items.Add("None")

        strSQL = "SELECT FMNum " &
                "FROM LUT_FuelModelParameters " &
                "ORDER BY FMNum"

        gf_SetControl(cmbFM1, strSQL, strProjectPath)
        gf_SetControl(cmbFM2, strSQL, strProjectPath)
        gf_SetControl(cmbFM3, strSQL, strProjectPath)
        gf_SetControl(cmbFM4, strSQL, strProjectPath)

        'Populate starting Fuel Model combobox
        strSQL = "SELECT FMNum " &
                "FROM LUT_FuelModelParameters " &
                "ORDER BY FMNum"

        gf_SetControl(cmbDefaultFM, strSQL, strProjectPath)

        cmbDefaultFM.SelectedIndex = 0

        cmbFM1.Text = "None"
        cmbFM2.Text = "None"
        cmbFM3.Text = "None"
        cmbFM4.Text = "None"

        'Populate Slope Combobox
        With cmbSlope
            For i = 0 To 5
                .Items.Add(i * 20)
            Next i
        End With
        cmbSlope.Text = 0
    End Sub

    Public Sub GraphCompareFM()
        Dim i As Integer 'counter
        Dim strROSFLCBH As String 'Stores output type units
        Dim intFM01 As Integer 'Stores dead 1 hr fuel moisture
        Dim intFM10 As Integer 'Stores dead 10 hr fuel moisture
        Dim intFM100 As Integer 'Stores dead 100 hr fuel moisture
        Dim intLH As Integer 'Stores live herb fuel moisture
        Dim intLW As Integer 'Stores live woody fuel moisture
        Dim strFM As String 'Stores the current fuel model being used
        Dim fmNewFM As clsFM 'Stores the newly created FM
        Dim legendMax As Integer 'Stores the maximum allowable number of legends
        Dim legendType As String 'Stores the type of calculation

        legendType = "" 'Initialize legend type a nothing

        'Create new fuel model collection
        Dim colFM As New Collection 'Stores all the fuel model objects
        fmNewFM = Nothing

        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            'Populate the fuelmodel collection
            strSQL = "SELECT FMNum, FMCode, FL1H, FL10H, FL100H, FLLiveH, FLLiveW, FMType, H1SAV, LiveHSAV, LiveWSAV, " &
                     "Depth, XtMoist, DHt, LHt, FMName, DataType, Creator " &
                     "FROM LUT_FuelModelParameters"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            Do While rs1.EOF = False
                fmNewFM = New clsFM(rs1.Fields!DataType.Value, rs1.Fields!FMNum.Value, rs1.Fields!FMCode.Value,
                                    rs1.Fields!FL1H.Value, rs1.Fields!FL10H.Value, rs1.Fields!FL100H.Value,
                                    rs1.Fields!FLLiveH.Value, rs1.Fields!FLLiveW.Value, rs1.Fields!FMType.Value,
                                    rs1.Fields!H1SAV.Value, rs1.Fields!LiveHSAV.Value, rs1.Fields!LiveWSAV.Value,
                                    rs1.Fields!Depth.Value, rs1.Fields!XtMoist.Value, rs1.Fields!DHt.Value,
                                    rs1.Fields!LHt.Value, rs1.Fields!FMName.Value, rs1.Fields!Creator.Value)
                colFM.Add(fmNewFM, "FM" & rs1.Fields!FMNum.Value)
                rs1.MoveNext()
            Loop

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in gr_MakeRuleset - " & ex.Message)
        End Try

        'Create arrays for the x-values and the y-values
        Dim MFWArray() As Object = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12",
                                    "13", "14", "15", "16", "17", "18", "19", "20"}

        'Set graphing parameters
        If rdoROS.Checked Then
            strROSFLCBH = "ROS(ch/hr)"
        ElseIf rdoFL.Checked Then
            strROSFLCBH = "FL(ft)"
        Else
            strROSFLCBH = "FL(ft)"
        End If

        If rdoDM1.Checked Then
            intFM01 = 3
            intFM10 = 4
            intFM100 = 5
        ElseIf rdoDM2.Checked Then
            intFM01 = 6
            intFM10 = 7
            intFM100 = 8
        ElseIf rdoDM3.Checked Then
            intFM01 = 9
            intFM10 = 10
            intFM100 = 11
        ElseIf rdoDM4.Checked Then
            intFM01 = 12
            intFM10 = 13
            intFM100 = 14
        End If

        If rdoLM1.Checked Then
            intLH = 30
            intLW = 60
        ElseIf rdoLM2.Checked Then
            intLH = 60
            intLW = 90
        ElseIf rdoLM3.Checked Then
            intLH = 90
            intLW = 120
        ElseIf rdoLM4.Checked Then
            intLH = 120
            intLW = 150
        End If

        Dim seriesArray(20) As Object 'This holds the values of ROS or FL by MFWS for the yaxis

        chrtCompFM.Series.Clear()
        chrtCompFM.Refresh()

        i = 0 'Reset counter to zero

        If rdoFL.Checked Then
            legendMax = 4
            legendType = "FL"
        ElseIf rdoCBH.Checked Then
            legendMax = 9
            legendType = "FL" 'This calcs FL for the first 5 (0 - 4) and CBH for the last 5 (5 - 9)
        ElseIf rdoROS.Checked Then
            legendMax = 4
            legendType = "ROS"
        End If

        Do While i <= legendMax '0 - 9 for 5 fuel models and 5 CBHs to compare the 5th model and 10th are the new custom model
            'Set seriesArray to 0s
            Dim x As Integer 'Represent the wind speed up to 20 mph
            For x = 0 To 20
                seriesArray(x) = "0"
            Next

            'Calculate the fire behavior
            strFM = "None" 'Initialize
            Try
                If i = 0 And cmbFM1.Text <> "None" And cmbFM1.Text <> "" Then
                    seriesArray = CalcFB(seriesArray, colFM.Item("FM" & cmbFM1.Text), intFM01, intFM10, intFM100,
                                         intLH, intLW, legendType)
                    strFM = cmbFM1.Text
                ElseIf i = 1 And cmbFM2.Text <> "None" And cmbFM2.Text <> "" Then
                    seriesArray = CalcFB(seriesArray, colFM.Item("FM" & cmbFM2.Text), intFM01, intFM10, intFM100,
                                         intLH, intLW, legendType)
                    strFM = cmbFM2.Text
                ElseIf i = 2 And cmbFM3.Text <> "None" And cmbFM3.Text <> "" Then
                    seriesArray = CalcFB(seriesArray, colFM.Item("FM" & cmbFM3.Text), intFM01, intFM10, intFM100,
                                         intLH, intLW, legendType)
                    strFM = cmbFM3.Text
                ElseIf i = 3 And cmbFM4.Text <> "None" And cmbFM4.Text <> "" Then
                    seriesArray = CalcFB(seriesArray, colFM.Item("FM" & cmbFM4.Text), intFM01, intFM10, intFM100,
                                         intLH, intLW, legendType)
                    strFM = cmbFM4.Text
                ElseIf i = 4 And grpCustFM.Visible = True Then
                    Dim fmNewCustom As New clsFM("English", 999, "CST", rdo1H.Text, rdo10H.Text, rdo100H.Text,
                                                 rdoLiveH.Text, rdoLiveW.Text, chkFMType.Text, rdo1HSAV.Text,
                                                 rdoLiveHSAV.Text, rdoLiveWSAV.Text, rdoDepth.Text, rdoXtMoist.Text,
                                                 8000, 8000, "Custom_Edit", "Custom")
                    seriesArray = CalcFB(seriesArray, fmNewCustom, intFM01, intFM10, intFM100, intLH, intLW, legendType)
                    strFM = "Custom"
                ElseIf i = 5 And cmbFM1.Text <> "None" And cmbFM1.Text <> "" Then
                    seriesArray = CalcFB(seriesArray, colFM.Item("FM" & cmbFM1.Text), intFM01, intFM10, intFM100,
                                         intLH, intLW, "CBH")
                    strFM = "CBH_" & cmbFM1.Text
                ElseIf i = 6 And cmbFM2.Text <> "None" And cmbFM2.Text <> "" Then
                    seriesArray = CalcFB(seriesArray, colFM.Item("FM" & cmbFM2.Text), intFM01, intFM10, intFM100,
                                         intLH, intLW, "CBH")
                    strFM = "CBH_" & cmbFM2.Text
                ElseIf i = 7 And cmbFM3.Text <> "None" And cmbFM3.Text <> "" Then
                    seriesArray = CalcFB(seriesArray, colFM.Item("FM" & cmbFM3.Text), intFM01, intFM10, intFM100,
                                         intLH, intLW, "CBH")
                    strFM = "CBH_" & cmbFM3.Text
                ElseIf i = 8 And cmbFM4.Text <> "None" And cmbFM4.Text <> "" Then
                    seriesArray = CalcFB(seriesArray, colFM.Item("FM" & cmbFM4.Text), intFM01, intFM10, intFM100,
                                         intLH, intLW, "CBH")
                    strFM = "CBH_" & cmbFM4.Text
                ElseIf i = 9 And grpCustFM.Visible = True Then
                    Dim fmNewCustom As New clsFM("English", 999, "CST", rdo1H.Text, rdo10H.Text, rdo100H.Text,
                                                 rdoLiveH.Text, rdoLiveW.Text, chkFMType.Text, rdo1HSAV.Text,
                                                 rdoLiveHSAV.Text, rdoLiveWSAV.Text, rdoDepth.Text, rdoXtMoist.Text,
                                                 8000, 8000, "Custom_Edit", "Custom")
                    seriesArray = CalcFB(seriesArray, fmNewCustom, intFM01, intFM10, intFM100, intLH, intLW, "CBH")
                    strFM = "CBH_Custom"
                End If
            Catch ex As Exception
                Debug.Write(ex.Message)
            End Try

            'Add a series to the chart with the x-values and y-values
            'from the arrays and set the series type to a column chart
            If strFM <> "None" Then
                'Create a Series
                Dim oSeries As New Series
                With oSeries
                    .Name = strFM
                    .Points.DataBindXY(MFWArray, seriesArray)
                End With

                'Add series to chart
                chrtCompFM.Series.Add(oSeries)

                'Set ChartAdd a title to the chart
                With chrtCompFM.ChartAreas(0)
                    'Add a title to axis
                    .AxisY.Title = strROSFLCBH
                    If rdoCBH.Checked Then
                        .AxisY2.Enabled = AxisEnabled.True
                        .AxisY2.Title = "Max CBH for " & vbCrLf & "passive crown fire(ft)"
                        .AxisY2.TitleFont = New Font("ComicSans", 10)
                    Else
                        .AxisY2.Enabled = AxisEnabled.False
                    End If
                End With

                With chrtCompFM.Series(strFM)
                    'Set series chart type
                    .ChartType = SeriesChartType.Line
                    .ChartType = SeriesChartType.Spline
                    'Set marker
                    .MarkerStyle = MarkerStyle.Circle
                    .MarkerSize = 6
                    'Set series line   
                    .BorderWidth = 3
                End With
            End If
            i = i + 1
        Loop
        chrtCompFM.Refresh()
    End Sub

    Private Function CalcFB(ByVal inArray As Array, ByVal fmFM As clsFM, ByVal intFM01 As Integer,
                            ByVal intFM10 As Integer, ByVal intFM100 As Integer, ByVal intMCLH As Integer,
                            ByVal intMCLW As Integer, ByVal CalcFL_ROS_CBH As String) As Array
        For x = 0 To 20
            If CalcFL_ROS_CBH = "FL" Then
                'Set the Fuel environment
                inArray(x) = fmFM.CalcFL(cmbSlope.Text, x, intFM01, intFM10, intFM100, intMCLH, intMCLW)
                Debug.Write(CalcFL_ROS_CBH & " " & fmFM.CalcFL(cmbSlope.Text, x, intFM01, intFM10, intFM100, intMCLH, intMCLW & vbCrLf))
            ElseIf CalcFL_ROS_CBH = "ROS" Then
                'Set the Fuel environment
                inArray(x) = fmFM.CalcROS(cmbSlope.Text, x, intFM01, intFM10, intFM100, intMCLH, intMCLW)
            ElseIf CalcFL_ROS_CBH = "CBH" Then
                inArray(x) = 3.2808399 * fmFM.CalcCBH(cmbSlope.Text, x, intFM01, intFM10, intFM100, intMCLH, intMCLW) 'Assume foliar moisture content to be 100%
            End If
        Next
        CalcFB = inArray
    End Function

    Sub CreateChrtDist()
        chrtDist = New Chart

        ' Set chart control location & size
        chrtDist.Location = New System.Drawing.Point(0, 150)
        chrtDist.Size = New System.Drawing.Size(Width - 150, Height - 310)

        ' Add chart control to the form
        DistributionGraph.Controls.AddRange(New System.Windows.Forms.Control() {chrtDist})

        ' Add Chart Area to the Chart
        Dim chrtArea As New ChartArea
        chrtDist.ChartAreas.Add(chrtArea)

        'Set ChartAdd a title to the chart
        With chrtDist.ChartAreas(0)
            'Add a title to axis
            .AxisX.Title = "Pre-disturbance % Cover"
            .AxisY.Title = "Acres from stacked columns"
            '.AxisY2.Title = "CBH (m) x 10"         'Get set in distGraph so the tile can change with CBH or CBD
            '.AxisY2.Title = "CBD (kg/m^3) x 100"   'Get set in distGraph so the tile can change with CBH or CBD
            .AxisX.TitleFont = New Font("ComicSans", 10)
            .AxisY.TitleFont = New Font("ComicSans", 10)
            .AxisY2.TitleFont = New Font("ComicSans", 10)
            'Set X-Axis
            .AxisX.LabelStyle.Font = New Font("ComicSans", 10)
            .AxisX.MajorTickMark.Interval = 1
            .AxisX.Interval = 1
            .AxisX.Minimum = 1
            .AxisX.Maximum = 11
            .AxisX.MajorGrid.Enabled = False
            'Set Y-Axis
            .AxisY.Minimum = 0
            .AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount
            .AxisY.LabelStyle.Font = New Font("ComicSans", 10)
            .AxisY.MajorGrid.Enabled = True
            .AxisY.MajorGrid.LineWidth = 1
            .AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid
            .AxisY.MajorGrid.LineColor = Color.Blue
            'Set Y-Axis2
            .AxisY2.Minimum = 0
            .AxisY2.IntervalAutoMode = IntervalAutoMode.VariableCount
            .AxisY2.LabelStyle.Font = New Font("ComicSans", 10)
            .AxisY2.MajorGrid.Enabled = True
            .AxisY2.MajorGrid.LineWidth = 1
            .AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dot
            '.AxisY2.MajorGrid.LineColor = Color.Red
        End With

        ' Create a new legend.
        'Dim LegendHgt = New LegendItem()
        Dim LegendHgt = New Legend
        LegendHgt.Title = "Pre-disturbance Height"
        'LegendHgt.Name = "Height Class"
        'LegendHgt.MarkerSize = 10
        'chrtDist.Legends.Add("LegendHgt")
        chrtDist.Legends.Add(LegendHgt)
    End Sub

    Sub CreateChrtCompFM()
        Dim strROSFLCBH As String 'Stores output type units

        chrtCompFM = New Chart

        ' Add Chart Area to the Chart
        Dim chrtArea As New ChartArea
        chrtCompFM.ChartAreas.Add(chrtArea)

        ' Set chart control location & size
        chrtCompFM.Location = New System.Drawing.Point(0, 100)
        chrtCompFM.Size = New System.Drawing.Size(Width - 150, Height - 310)

        ' Add chart control to the form
        CompareFM.Controls.AddRange(New System.Windows.Forms.Control() {chrtCompFM})

        'Set graphing parameters
        If rdoROS.Checked Then
            strROSFLCBH = "ROS(ch/hr)"
        ElseIf rdoFL.Checked Then
            strROSFLCBH = "FL(ft)"
        Else
            strROSFLCBH = "FL(ft)"
        End If

        'Set ChartAdd a title to the chart
        With chrtCompFM.ChartAreas(0)
            'Add a title to axis
            .AxisX.Title = "MidFlame Wind Speed, Upslope (mi/hr)"
            .AxisX.TitleFont = New Font("ComicSans", 10)
            .AxisY.Title = strROSFLCBH
            .AxisY.TitleFont = New Font("ComicSans", 10)
            If rdoCBH.Checked Then
                .AxisY2.Enabled = AxisEnabled.True
                .AxisY2.Title = "Max CBH for " & vbCrLf & "passive crown fire(ft)"
                .AxisY2.TitleFont = New Font("ComicSans", 10)
            Else
                .AxisY2.Enabled = AxisEnabled.False
            End If

            'Set X-Axis scale to 0 - 20
            .AxisX.Minimum = 1
            .AxisX.Maximum = 21
            .AxisX.LabelStyle.Font = New Font("ComicSans", 10)
            .AxisX.MajorTickMark.Interval = 1
            .AxisX.Interval = 2
            'Set Y-Axis scale to auto
            .AxisY.Minimum = [Double].NaN
            .AxisY.Maximum = [Double].NaN
            .AxisY.LabelStyle.Font = New Font("ComicSans", 10)
        End With

        ' Create a new legend
        Dim LegendHgt = New LegendItem()
        LegendHgt.Name = "Fire Behavior"
        LegendHgt.MarkerSize = 10
        chrtCompFM.Legends.Add("Fire Behavior")
    End Sub

    Private Sub DistGraph()
        Dim rnd As New Random                                           'Used to set a random color
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim CanopySel As String              'Flag for CBH or CBD to be displayed in graph
            Dim Series1Index As Integer         'Stores the starting series index for the cbh and cbd series
            Dim SeriesEqual As Boolean = True   'Stores if series is equal or not

            If rdoNoneDistGraph.Checked Then
                CanopySel = "None"                                          'Neither selected, do not show line graph
            ElseIf rdoCCDistGraph.Checked Then
                chrtDist.ChartAreas(0).AxisY2.Title = "% CC in Dist code -  " &
                    gf_GetNum(cmbEVT.SelectedItem, "DIST")              'Set Y2 Axis title
                CanopySel = "Cover"                                         'Cover selected, show CC line graph
            ElseIf rdoCHDistGraph.Checked Then
                chrtDist.ChartAreas(0).AxisY2.Title = "CH(m) in Dist code -  " &
                    gf_GetNum(cmbEVT.SelectedItem, "DIST")              'Set Y2 Axis title
                CanopySel = "Height"                                        'Height selected, show CH line graph
            ElseIf rdoCBHDistGraph.Checked Then
                chrtDist.ChartAreas(0).AxisY2.Title = "CBH(m) in Dist code -  " &
                    gf_GetNum(cmbEVT.SelectedItem, "DIST")              'Set Y2 Axis title
                CanopySel = "CBH"                                           'CBH selected, show CBH line graph
            Else
                CanopySel = "CBD"                                           'CBD selected, show CBD line graph
                chrtDist.ChartAreas(0).AxisY2.Title = "CBD(kg/m^3) in Dist code -  " &
                    gf_GetNum(cmbEVT.SelectedItem, "DIST")              'Set Y2 Axis title
            End If

            Dim itemCounter As Integer = 1
            Dim treeCounter As Integer = 0

            Dim covArray() As Object = {"%", "15", "25", "35", "45", "55", "65", "75", "85", "95"}

            'Get the count of the number of tree lifeforms
            strSQL = "SELECT LUT_Height.Lifeform, Count(LUT_Height.Lifeform) AS CountOfLifeform " &
                     "FROM LUT_Height " &
                     "GROUP BY LUT_Height.Lifeform " &
                     "HAVING (((LUT_Height.Lifeform)='Tree'))"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            If rs1.EOF = False Then
                treeCounter = rs1.Fields!CountOfLifeform.Value
            End If
            rs1.Close()

            strSQL = "SELECT LUT_Height.Lifeform, LUT_Height.LowName, LUT_Height.HighName, LUT_Height.EVH " &
                     "FROM LUT_Height " &
                     "WHERE(((LUT_Height.EVH) > 100)) " &
                     "ORDER BY LUT_Height.EVH"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            itemCounter = 1

            Dim hgtArray(rs1.RecordCount + treeCounter) As Object

            Do While rs1.EOF() <> True

                If rs1.Fields!Lifeform.Value = "Tree" Then
                    'Canopy
                    hgtArray(itemCounter) = Mid(rs1.Fields!LowName.Value, 1, Len(rs1.Fields!LowName.Value) - 1) &
                                                            Trim(Mid(rs1.Fields!HighName.Value, 1, Len(rs1.Fields!HighName.Value) - 5))
                    hgtArray(itemCounter + treeCounter) = Mid(rs1.Fields!LowName.Value, 1, Len(rs1.Fields!LowName.Value) - 1) &
                                                            Trim(Mid(rs1.Fields!HighName.Value, 1, Len(rs1.Fields!HighName.Value) - 5))
                Else
                    'No Canopy
                    hgtArray(itemCounter) = Mid(rs1.Fields!LowName.Value, 1, Len(rs1.Fields!LowName.Value) - 1) &
                                                            Trim(Mid(rs1.Fields!HighName.Value, 1, Len(rs1.Fields!HighName.Value) - 5))
                End If

                itemCounter += 1
                rs1.MoveNext()
            Loop

            rs1.Close()

            'This holds the values of acres by cover class for the yaxis plus 2 space holders
            Dim seriesArray(covArray.Length - 1) As Object

            'Clear series data
            chrtDist.Series.Clear()

            'Get series values and store in arrays then add to the chart
            Dim hA As Integer = 1           'Stores the position in the height array
            Dim evhMidPoint As Integer      'Stores the midpoint of the cover used in CBH and CBD
            Dim evhCode As Integer          'Starting Height code

            'Get EVHs
            strSQL = "SELECT LUT_Height.Lifeform, LUT_Height.EVH " &
                 "FROM LUT_Height " &
                 "WHERE(((LUT_Height.EVH) > 100)) " &
                 "ORDER BY LUT_Height.EVH"
            rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            Do While hA <= hgtArray.Length - treeCounter - 1 'All the height + additional for canopy
                Dim cA As Short
                'Set seriesArray to 0s
                For cA = 1 To covArray.Length - 1 'cA Counts the position in the series array used further down aswell
                    seriesArray(cA) = "0"
                Next

                evhCode = rs2.Fields!EVH.Value 'Set next evh code

                If IsNumeric(gf_GetNum(cmbBPSGraph.SelectedItem, "General")) Then 'BPS is numeric not 'any'
                    strSQL = "SELECT LUT_Height.Lifeform, " & comboR & ".EVHR, " & comboR & ".EVCR, " &
                                "Sum(IIf(((" & comboR & ".EVTR=" & gf_GetNum(cmbEVT.SelectedItem, "EVT") & ") " &
                                "And (" & comboR & ".DIST=" & gf_GetNum(cmbEVT.SelectedItem, "DIST") & ") " &
                                "And (" & comboR & ".BPSRF = " & gf_GetNum(cmbBPSGraph.SelectedItem, "General") & ") " &
                                "And (" & comboR & ".Wildcard = '" & cmbWildGraph.SelectedItem & "')) " &
                                "Or ((" & comboR & ".EVTR=" & gf_GetNum(cmbEVT.SelectedItem, "EVT") & ") " &
                                "And (" & comboR & ".DIST=" & gf_GetNum(cmbEVT.SelectedItem, "DIST") & ") " &
                                "And (" & comboR & ".BPSRF = " & gf_GetNum(cmbBPSGraph.SelectedItem, "General") & ") " &
                                "And ('" & cmbWildGraph.SelectedItem & "' = 'any')), " &
                                "Round(" & comboR & ".COUNT * 0.2223948,0),0)) AS SUMCOUNT " &
                                "FROM (LUT_Cover Inner JOIN " & comboR & " ON LUT_Cover.EVC = " & comboR & ".EVCR) " &
                                "INNER JOIN LUT_Height ON " & comboR & ".EVHR = LUT_Height.EVH " &
                                "GROUP BY LUT_Height.Lifeform, " & comboR & ".EVHR, " & comboR & ".EVCR " &
                                "HAVING(((" & comboR & ".EVHR) = " & evhCode & ") And ((" & comboR & ".EVCR) > 100)) " &
                                "ORDER BY " & comboR & ".EVCR"
                Else 'BPS is text it is 'any'
                    strSQL = "SELECT LUT_Height.Lifeform, " & comboR & ".EVHR, " & comboR & ".EVCR, " &
                                "Sum(IIf(((" & comboR & ".EVTR=" & gf_GetNum(cmbEVT.SelectedItem, "EVT") & ") " &
                                "And (" & comboR & ".DIST=" & gf_GetNum(cmbEVT.SelectedItem, "DIST") & ") " &
                                "And (" & comboR & ".Wildcard = '" & cmbWildGraph.SelectedItem & "')) " &
                                "Or ((" & comboR & ".EVTR=" & gf_GetNum(cmbEVT.SelectedItem, "EVT") & ") " &
                                "And (" & comboR & ".DIST=" & gf_GetNum(cmbEVT.SelectedItem, "DIST") & ") " &
                                "And ('" & cmbWildGraph.SelectedItem & "' = 'any')), " &
                                "Round(" & comboR & ".COUNT*0.2223948,0),0)) AS SUMCOUNT " &
                                "FROM (LUT_Cover Inner JOIN " & comboR & " ON LUT_Cover.EVC = " & comboR & ".EVCR) " &
                                "INNER JOIN LUT_Height ON " & comboR & ".EVHR = LUT_Height.EVH " &
                                "GROUP BY LUT_Height.Lifeform, " & comboR & ".EVHR, " & comboR & ".EVCR " &
                                "HAVING(((" & comboR & ".EVHR) = " & evhCode & ") And ((" & comboR & ".EVCR) > 100)) " &
                                "ORDER BY " & comboR & ".EVCR"
                End If

                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Run three times per query
                Dim acreCheck As Boolean = False    'True if acres are present False if not
                cA = 1                              'Keeps track of what number is added to the series
                Do Until rs1.EOF
                    seriesArray(cA) = rs1.Fields!SUMCOUNT.Value
                    If seriesArray(cA) > 0 Then acreCheck = True
                    cA += 1
                    rs1.MoveNext()
                Loop
                rs1.Close()

                'Add a series to the chart with the x-values and y-values
                'from the arrays and set the series type to a column chart
                'Create a Series
                If acreCheck = True Then
                    Dim oSeries As New Series
                    If rdoNoneDistGraph.Checked <> False Then
                        With oSeries
                            .YAxisType = AxisType.Primary
                            .Name = hgtArray(hA)
                            'Set Data
                            .Points.DataBindXY(covArray, seriesArray)
                        End With

                        'Add series to chart
                        chrtDist.Series.Add(oSeries)

                        With chrtDist.Series(hgtArray(hA))
                            'Set series chart type
                            '.ChartType = SeriesChartType.Bar
                            .ChartType = SeriesChartType.StackedColumn
                            '.ChartType = SeriesChartType.Line
                            '.ChartType = SeriesChartType.Spline
                            .Color = Color.FromArgb(rnd.Next(50, 200), rnd.Next(50, 200), rnd.Next(50, 200))   'Assign a color.
                        End With

                        'Get canopy values for the series
                    ElseIf rs2.Fields!Lifeform.Value = "Tree" And rdoNoneDistGraph.Checked <> True Then
                        Dim distCC As Double                   'Stores the disturbed Canopy Cover
                        Dim distCH As Double                    'Stores the disturbed Canopy Height

                        cA = 1
                        evhMidPoint = gf_GetHeightMid(evhCode, strProjectPath) * 10
                        strSQL = "SELECT LUT_Cover.Lifeform, LUT_Cover.EVC " &
                                    "FROM LUT_Cover " &
                                    "WHERE(((LUT_Cover.Lifeform) = 'Tree')) " &
                                    "ORDER BY LUT_Cover.EVC"
                        rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                        Do Until rs1.EOF
                            'Get disturbed CC and CH if disturbed
                            If gf_GetNum(cmbEVT.SelectedItem, "DIST") > 0 Then
                                distCC = Canopy_LM_EQs((rs1.Fields!EVC.Value - 100) * 10 + 5,
                                                            evhMidPoint / 10, gf_GetNum(cmbEVT.SelectedItem, "EVT"),
                                                            gf_GetNum(cmbEVT.SelectedItem, "DIST"), "Cover")
                                distCH = Canopy_LM_EQs((rs1.Fields!EVC.Value - 100) * 10 + 5,
                                                            evhMidPoint / 10, gf_GetNum(cmbEVT.SelectedItem, "EVT"),
                                                            gf_GetNum(cmbEVT.SelectedItem, "DIST"), "Height")
                            Else                            'Get non disturbed CC and CH
                                distCC = (rs1.Fields!EVC.Value - 100) * 10 + 5
                                distCH = evhMidPoint / 10
                            End If

                            If distCC < 10 Or distCH <= 1.8 Then     'Check for 0 canopy
                                seriesArray(cA) = 0
                            ElseIf rdoCCDistGraph.Checked Then      'Get CCs
                                seriesArray(cA) = distCC
                            ElseIf rdoCHDistGraph.Checked Then      'Get CHs
                                seriesArray(cA) = distCH
                            ElseIf rdoCBHDistGraph.Checked Then     'Get CBHs
                                seriesArray(cA) = Canopy_LM_EQs(distCC, distCH, gf_GetNum(cmbEVT.SelectedItem, "EVT"),
                                                                gf_GetNum(cmbEVT.SelectedItem, "DIST"), "CBH")
                            ElseIf rdoCBDDistGraph.Checked Then     'Get CBDs
                                seriesArray(cA) = CalcCBDGLM(distCC, distCH) / 100
                            Else                                    'Skip this record
                                'Do Nothing
                            End If

                            cA += 1
                            rs1.MoveNext()
                        Loop
                        'Add a series to the chart with the x-values and y-values
                        'from the arrays and set the series type to a column chart
                        'Create a Series
                        oSeries = New Series
                        With oSeries
                            .YAxisType = AxisType.Secondary
                            .Name = hgtArray(hA + treeCounter)
                            'Set Data
                            .Points.DataBindXY(covArray, seriesArray)
                            'Set marker
                            .MarkerStyle = MarkerStyle.Circle
                            .MarkerSize = 15
                            '.BorderWidth = 20
                            'Set series line   
                            '.BorderWidth = 10
                            '.Color = Color.Firebrick
                            '.Palette = ChartColorPalette.Bright
                        End With

                        'Add series to chart
                        chrtDist.Series.Add(oSeries)

                        With chrtDist.Series(hgtArray(hA + treeCounter))
                            'Set series chart type
                            .ChartType = SeriesChartType.Line
                            '.ChartType = SeriesChartType.Candlestick
                            .Color = Color.FromArgb(rnd.Next(50, 200), rnd.Next(50, 200), rnd.Next(50, 200))   'Assign a color.
                        End With

                        Series1Index = chrtDist.Series.Count - 1                                        'Get starting count before adding CBH and CBD series
                        rs1.Close()
                    End If
                End If
                hA += 1
                If rs2.EOF = False Then rs2.MoveNext()
            Loop
            rs2.Close()
            chrtDist.Refresh()

            If rdoNoneDistGraph.Checked <> True Then                                                    'Set colors for CBH or CBD
                If Series1Index >= 1 Then
                    Series1Index = 0

                    For Series2Index = Series1Index + 1 To chrtDist.Series.Count - 1                    'SeriesIndex to be compared to
                        For pIndex = 1 To 9                                                             'Points Index
                            If chrtDist.Series(Series1Index).Points(pIndex).ToString <>
                                chrtDist.Series(Series2Index).Points(pIndex).ToString Then
                                SeriesEqual = False
                            End If
                        Next
                        If SeriesEqual = True Then
                            chrtDist.Series(Series2Index).Color = chrtDist.Series(Series1Index).Color
                        End If

                        SeriesEqual = True                                                              'Reset the flag to true
                        Series1Index = Series2Index
                    Next
                End If
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
            MsgBox("DistGraph() " & ex.Message)
        End Try
    End Sub

    Private Function Canopy_LM_EQs(ByVal numCC As Short, ByVal numCH As Integer, ByVal numEVT As Short,
                                   ByVal numDist As Short, ByVal canopyType As String) As Double
        Dim dbconn As New ADODB.Connection                                                  'DB connection
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            'Get the equation for the specified coefficients
            strSQL = "SELECT Tree_EVTs, HDist, intercept, HT_coef, CC_coef, EV_Structure " &
                     "FROM Master_Disturbance_Tbl " &
                     "WHERE (((Tree_EVTs)= " & numEVT & ") " &
                     "AND ((HDist)= " & numDist & ") AND " &
                     "((EV_Structure)='" & canopyType & "'))"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            Canopy_LM_EQs = rs1.Fields!intercept.Value + (rs1.Fields!HT_coef.Value * (numCH)) + (rs1.Fields!CC_coef.Value * numCC)

            rs1.Close()

            If canopyType = "CBH" And Math.Round(Canopy_LM_EQs, 1) < 0.3 Then
                Canopy_LM_EQs = 0.3
            ElseIf canopyType = "CBH" And Math.Round(Canopy_LM_EQs, 1) >= 10 Then
                Canopy_LM_EQs = 10
            ElseIf canopyType = "CBH" And Math.Round(Canopy_LM_EQs, 1) >= numCH Then
                Canopy_LM_EQs = numCH * 0.6666
            ElseIf canopyType = "CBH" Then
                Canopy_LM_EQs = Math.Round(Canopy_LM_EQs, 1)
            End If

            If canopyType = "Cover" And numDist = 0 Then
                Canopy_LM_EQs = numCC
            ElseIf canopyType = "Cover" And Math.Round(Canopy_LM_EQs, 0) < 10 Then
                Canopy_LM_EQs = 0
            ElseIf canopyType = "Cover" And Math.Round(Canopy_LM_EQs, 0) > 95 Then
                Canopy_LM_EQs = 95
            ElseIf canopyType = "Cover" Then
                Canopy_LM_EQs = Math.Round(Canopy_LM_EQs, 0)
            End If

            If canopyType = "Height" And numDist = 0 Then
                Canopy_LM_EQs = numCH
            ElseIf canopyType = "Height" And Math.Round(Canopy_LM_EQs, 1) < 1.3 Then
                Canopy_LM_EQs = 0
            ElseIf canopyType = "Height" And Math.Round(Canopy_LM_EQs, 1) >= 50 Then
                Canopy_LM_EQs = 50
            ElseIf canopyType = "Height" Then
                Canopy_LM_EQs = Math.Round(Canopy_LM_EQs, 1)
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If

            MsgBox("Error in Canopy_LM_EQs - " & ex.Message)
        End Try
    End Function

    Private Function CalcCBDGLM(ByVal CCMidpoint As Double, ByVal CHMidPoint As Double) As Integer
        Dim dblCBD As Double 'Stores the CBD GLM predicted value
        Dim dblHgt As Double 'Stores the height value in meters
        Dim intCov As Long 'Stores the cover value in percent cover
        Dim intPJ As Long = 0 'Stores the PJ switch
        Dim intSH1 As Long = 0 'Stores the Stand Height switch 1
        Dim intSH2 As Long = 0 'Stores the Stand Height switch 2
        Dim intEVT As Integer = gf_GetNum(cmbEVT.SelectedItem, "EVT")

        'Do GLM based method
        If intEVT <> 2016 And intEVT <> 2017 And intEVT <> 2019 And intEVT <> 2025 And
            intEVT <> 2059 And intEVT <> 2115 And intEVT <> 2116 And intEVT <> 2119 Then
            '***************************Calculate for non pj
            dblHgt = CHMidPoint  'Get the height in meters
            intCov = CCMidpoint  'Get the percent cover

            'EXP(-2.4887057+(0.0335917*CC)+(-0.356861*SH1_)+(-0.6006381*SH2_)+(-1.10691*PJ)+(-0.0010804*CC*SH1_)+(-0.0018324*CC*SH2_))
            'CBDpred = −2.489 + 0.034(CC)+−0.357(SH1)+−0.601(SH2)+−1.107(PJ)+−0.001(CC × SH1)+−0.002(CC × SH2)

            'This tells the equation that none of these are pj or j
            intPJ = 1

            If dblHgt < 15 Then
                intSH1 = 0
                intSH2 = 0
            ElseIf dblHgt < 30 Then
                intSH1 = 1
                intSH2 = 0
            ElseIf dblHgt > 30 Then
                intSH1 = 0
                intSH2 = 1
            End If
            dblCBD = -2.4887057 + (0.0335917 * intCov) + (-0.356861 * intSH1) + -(0.6006381 * intSH2) +
                    (-1.10691 * intPJ) + (-0.0010804 * (intCov * intSH1)) + (-0.0018324 * (intCov * intSH2))

            'The base natural logarithm raised to the dblCBD value multiply by 100 then integerize for kg/m^3 * 100
            dblCBD = Math.Round(Math.Exp(dblCBD), 2) * 100
        Else
            '**********************Calculate for just pj or j (This is only for pj that have a canopy of 1 no need to look at canopy 2
            '**********************because it is already assigned during the non pj above

            dblHgt = CHMidPoint         'Get the height in meters
            intCov = CCMidpoint         'Get the percent cover

            'EXP(-2.4887057+(0.0335917*CC)+(-0.356861*SH1_)+(-0.6006381*SH2_)+(-1.10691*PJ)+(-0.0010804*CC*SH1_)+(-0.0018324*CC*SH2_))
            'CBDpred = −2.489 + 0.034(CC)+−0.357(SH1)+−0.601(SH2)+−1.107(PJ)+−0.001(CC × SH1)+−0.002(CC × SH2)

            intPJ = 0  '0 Means it is a PJ and or J EVT

            If dblHgt < 15 Then
                intSH1 = 0
                intSH2 = 0
            ElseIf dblHgt < 30 Then
                intSH1 = 1
                intSH2 = 0
            ElseIf dblHgt > 30 Then
                intSH1 = 0
                intSH2 = 1
            End If
            dblCBD = -2.4887057 + (0.0335917 * intCov) + (-0.356861 * intSH1) + -(0.6006381 * intSH2) +
                    (-1.10691 * intPJ) + (-0.0010804 * (intCov * intSH1)) + (-0.0018324 * (intCov * intSH2))
            'The base natural logarithm raised to the dblCBD value multiply by 100 then integerize for kg/m^3 * 100
            dblCBD = Math.Round(Math.Exp(dblCBD), 2) * 100
        End If

        CalcCBDGLM = Int(dblCBD)        'Return dblCBD

        If CalcCBDGLM > 45 Then CalcCBDGLM = 45 'Check to make sure CBD does not exceed 45
    End Function

    Private Function AlreadySelected(ByVal cmbFM As ComboBox) As Boolean
        AlreadySelected = False
        If cmbFM.Text = "None" Then
            'do nothing it stays false
        Else
            If cmbFM.Name <> cmbFM1.Name And cmbFM.Text = cmbFM1.Text Then AlreadySelected = True
            If cmbFM.Name <> cmbFM2.Name And cmbFM.Text = cmbFM2.Text Then AlreadySelected = True
            If cmbFM.Name <> cmbFM3.Name And cmbFM.Text = cmbFM3.Text Then AlreadySelected = True
            If cmbFM.Name <> cmbFM4.Name And cmbFM.Text = cmbFM4.Text Then AlreadySelected = True
        End If
    End Function

    Private Sub cmbSlope_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSlope.SelectedIndexChanged
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in cmbSlope_SelectedIndexChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub cmbFM1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFM1.SelectedIndexChanged
        Try
            If TabControl.SelectedIndex = 1 Then
                If AlreadySelected(cmbFM1) = False Then
                    GraphCompareFM()
                Else
                    cmbFM1.Text = "None"
                    GraphCompareFM()
                End If
            End If
        Catch ex As Exception
            MsgBox("Error in cmbFM1_SelectedIndexChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub cmbFM2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFM2.SelectedIndexChanged
        Try
            If TabControl.SelectedIndex = 1 Then
                If AlreadySelected(cmbFM2) = False Then
                    GraphCompareFM()
                Else
                    cmbFM2.Text = "None"
                    GraphCompareFM()
                End If
            End If
        Catch ex As Exception
            MsgBox("Error in cmbFM2_SelectedIndexChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub cmbFM3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFM3.SelectedIndexChanged
        Try
            If TabControl.SelectedIndex = 1 Then
                If AlreadySelected(cmbFM3) = False Then
                    GraphCompareFM()
                Else
                    cmbFM3.Text = "None"
                    GraphCompareFM()
                End If
            End If
        Catch ex As Exception
            MsgBox("Error in cmbFM3_SelectedIndexChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub cmbFM4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFM4.SelectedIndexChanged
        Try
            If TabControl.SelectedIndex = 1 Then
                If AlreadySelected(cmbFM4) = False Then
                    GraphCompareFM()
                Else
                    cmbFM4.Text = "None"
                    GraphCompareFM()
                End If
            End If
        Catch ex As Exception
            MsgBox("Error in cmbFM4_SelectedIndexChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoROS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoROS.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoROS_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoFL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoFL.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoFL_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoCBH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoCBH.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoCBH_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoDM1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoDM1.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoDM1_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoDM2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoDM2.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoDM2_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoDM3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoDM3.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoDM3_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoDM4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoDM4.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoDM4_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLM1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoLM1.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoLM1_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLM2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoLM2.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoLM2_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLM3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoLM3.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoLM3_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLM4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoLM4.Click
        Try
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoLM4_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdCustomFM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCustomFM.Click
        Try
            If cmdCustomFM.Text = "Custom" & vbCrLf & "FM" Then
                cmdCustomFM.Text = "Close" & vbCrLf & "Custom"
                grpCustFM.Visible = True
                GraphCompareFM()
            Else
                cmdCustomFM.Text = "Custom" & vbCrLf & "FM"
                grpCustFM.Visible = False
                GraphCompareFM()
            End If
        Catch ex As Exception
            MsgBox("Error in cmdCustom_FM_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub TrkBar_Scroll(ByVal sender As Object, ByVal e As EventArgs) Handles TrkBar.Scroll
        Try
            If rdo1H.Checked Then rdo1H.Text = TrkBar.Value / 100
            If rdo10H.Checked Then rdo10H.Text = TrkBar.Value / 100
            If rdo100H.Checked Then rdo100H.Text = TrkBar.Value / 100
            If rdo1HSAV.Checked Then
                rdo1HSAV.Text = TrkBar.Value
                'If rdo1HSAV.Text = 3600 Then rdo1HSAV.Text = 9999
            End If
            If rdoLiveHSAV.Checked Then
                rdoLiveHSAV.Text = TrkBar.Value
                'If rdoLiveHSAV.Text = 2100 Then rdoLiveHSAV.Text = 9999
            End If
            If rdoLiveWSAV.Checked Then
                rdoLiveWSAV.Text = TrkBar.Value
                'If rdoLiveWSAV.Text = 2100 Then rdoLiveWSAV.Text = 9999
            End If

            If rdoDepth.Checked Then rdoDepth.Text = TrkBar.Value / 100
            If rdoLiveH.Checked Then rdoLiveH.Text = TrkBar.Value / 100
            If rdoLiveW.Checked Then rdoLiveW.Text = TrkBar.Value / 100
            If rdoXtMoist.Checked Then rdoXtMoist.Text = TrkBar.Value

            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
            Refresh()
        Catch ex As Exception
            MsgBox("Error in TrkBar_Scroll - " & ex.Message)
        End Try

    End Sub

    Private Sub rdo1H_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo1H.CheckedChanged
        Try
            If rdo1H.Text <> "" Then
                With TrkBar
                    .Minimum = 0
                    .Maximum = 701
                    .Value = rdo1H.Text * 100
                    .TickFrequency = 35
                End With
            End If
        Catch ex As Exception
            MsgBox("Error in rdo1H_CheckChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdo10H_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo10H.CheckedChanged
        Try
            With TrkBar
                .Minimum = 0
                .Maximum = 2304
                .Value = rdo10H.Text * 100
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20
            End With
        Catch ex As Exception
            MsgBox("Error in rdo10H_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdo100H_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo100H.CheckedChanged
        Try
            With TrkBar
                .Minimum = 0
                .Maximum = 2805
                .Value = rdo100H.Text * 100
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20
            End With
        Catch ex As Exception
            MsgBox("Error in rdo100H_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdo1HSAV_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdo1HSAV.CheckedChanged
        Try
            With TrkBar
                .Minimum = 750
                .Maximum = 3600
                If rdo1HSAV.Text > .Maximum Then
                    .Value = .Maximum
                Else
                    .Value = rdo1HSAV.Text
                End If
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20

            End With
        Catch ex As Exception
            MsgBox("Error in rdo1HSAV_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLiveHSAV_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoLiveHSAV.CheckedChanged
        Try
            With TrkBar
                .Minimum = 1300
                .Maximum = 2100
                If rdoLiveHSAV.Text > .Maximum Then
                    .Value = .Maximum
                Else
                    .Value = rdoLiveHSAV.Text
                End If
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20

            End With
        Catch ex As Exception
            MsgBox("Error in rdoLiveHSAV_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLiveWSAV_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoLiveWSAV.CheckedChanged
        Try
            With TrkBar
                .Minimum = 750
                .Maximum = 2100
                If rdoLiveWSAV.Text > .Maximum Then
                    .Value = .Maximum
                Else
                    .Value = rdoLiveWSAV.Text
                End If
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20

            End With
        Catch ex As Exception
            MsgBox("Error in rdoLiveWSAV_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoDepth_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoDepth.CheckedChanged
        Try
            With TrkBar
                .Minimum = 20
                .Maximum = 600
                .Value = rdoDepth.Text * 100
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20
            End With
        Catch ex As Exception
            MsgBox("Error in rdoDepth_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLiveH_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoLiveH.CheckedChanged
        Try
            With TrkBar
                .Minimum = 0
                .Maximum = 900
                .Value = rdoLiveH.Text * 100
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20
            End With
        Catch ex As Exception
            MsgBox("Error in rdoLiveH_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoLiveW_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoLiveW.CheckedChanged
        Try
            With TrkBar
                .Minimum = 0
                .Maximum = 710
                .Value = rdoLiveW.Text * 100
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20
            End With
        Catch ex As Exception
            MsgBox("Error in rdoLiveW_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoXtMoist_CheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoXtMoist.CheckedChanged
        Try
            With TrkBar
                .Minimum = 12
                .Maximum = 40
                .Value = rdoXtMoist.Text
                .TickFrequency = (TrkBar.Maximum - TrkBar.Minimum) / 20
            End With
        Catch ex As Exception
            MsgBox("Error in rdoXtMoist_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub chkFMType_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkFMType.CheckedChanged
        Try
            If chkFMType.Checked Then
                chkFMType.Text = "dynamic"
            Else
                chkFMType.Text = "static"
            End If
            If TabControl.SelectedIndex = 1 Then GraphCompareFM()
        Catch ex As Exception
            MsgBox("Error in rdoFMType_CheckedChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub cmbDefaultFM_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbDefaultFM.SelectedIndexChanged
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim strFMType As String

            'Populate starting values of parameters
            strSQL = "SELECT FMNum, FMCode, FL1H, FL10H, FL100H, FLLiveH, FLLiveW, FMType, H1SAV, LiveHSAV, LiveWSAV, " &
                     "Depth, XtMoist, DHt, LHt, FMName " &
                     "FROM LUT_FuelModelParameters " &
                     "WHERE FMNum = " & cmbDefaultFM.Text
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            rdo1H.Text = Math.Round(rs1.Fields!FL1H.Value, 2)
            rdo10H.Text = Math.Round(rs1.Fields!FL10H.Value, 2)
            rdo100H.Text = Math.Round(rs1.Fields!FL100H.Value, 2)
            rdoLiveH.Text = Math.Round(rs1.Fields!FLLiveH.Value, 2)
            rdoLiveW.Text = Math.Round(rs1.Fields!FLLiveW.Value, 2)
            rdo1HSAV.Text = rs1.Fields!H1SAV.Value
            rdoLiveHSAV.Text = rs1.Fields!LiveHSAV.Value
            rdoLiveWSAV.Text = rs1.Fields!LiveWSAV.Value
            rdoDepth.Text = Math.Round(rs1.Fields!Depth.Value, 2)
            rdoXtMoist.Text = rs1.Fields!XtMoist.Value

            strFMType = rs1.Fields!FMType.Value 'Save the value before closeing. 

            'Close before making change that might fire a new event that accesses the database
            'dbDAO.Close()
            'dbDAO = Nothing

            If strFMType = "dynamic" Then
                chkFMType.Checked = True
            Else
                chkFMType.Checked = False
            End If

            rdo1H.Checked = True

            With TrkBar
                .Minimum = 0
                .Maximum = 701
                .Value = rdo1H.Text * 100
                .TickFrequency = 35
            End With

            If TabControl.SelectedIndex = 1 Then GraphCompareFM()

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
            MsgBox("Error in cmbFMStart_SelectedIndexChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdSaveCSTFM_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open

        Try
            Dim strNewFMNum As String 'Stores the newly entered fuel model number
            Dim strNewFMName As String 'Stores the newly entered fuel model name
            Dim strNotAvail As String  'Fuel model already used
            Dim blnGood As Boolean  'Marks if the number is good or not
            Dim strError As String      'Stores why the new number is bad
            blnGood = False
            strNewFMName = "nothing"
            strNewFMNum = "nothing"
            strError = ""
            strNotAvail = ""

            strSQL = "SELECT FMNum " &
                     "FROM LUT_FuelModelParameters " &
                     "Order by FMNum"
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            Do While rs1.EOF = False
                strNotAvail = strNotAvail & ", " & rs1.Fields!FMNum.Value
                rs1.MoveNext
            Loop

            Do While blnGood = False
                'Get new Fuel Model Number from the user
                rs1.MovePrevious
                strNewFMNum = InputBox("Input a new 3 digit or less Custom Fuel Model number. " &
                         "Do not use Anderson 13 or Scott and Burgan existing Fuel Model Numbers. " &
                         "These numbers are already in use: " & strNotAvail, "New Custom FM Number",
                         Str(rs1.Fields!FMNum.Value + 1))

                'Check for bad number
                blnGood = True
                If IsNumeric(strNewFMNum) = False Then
                    blnGood = False
                    strError = "Error: " & strNewFMNum & " is not a number."
                ElseIf strNewFMNum > 1000 Or strNewFMNum < 0 Then
                    blnGood = False
                    strError = "Error: " & strNewFMNum & " is either < 0 OR > 999."
                End If

                rs1.MoveFirst
                Do While rs1.EOF = False And blnGood = True

                    If Int(strNewFMNum) = rs1.Fields!FMNum.Value Then
                        blnGood = False
                        strError = "Error: " & strNewFMNum & " =  an existing Fuel Model Number."
                    End If

                    rs1.MoveNext
                Loop
                rs1.Close

                'If blnGood is false let the user know why it is false
                If blnGood = False Then MsgBox(strError, , "Bad Fuel Model Number")
                If strNewFMNum = "" Then Exit Sub 'Cancel was pushed
            Loop

            'Get a new fuel model name
            blnGood = False 'False unless proven true
            Do While blnGood = False
                blnGood = True
                strNewFMName = InputBox("Input a new fuel model name less than 255 characters " &
                                        "long that describes the custom fuel model.",
                                        "New Custom FM Code", "ABC")
                If strNewFMName.Length = 0 Then
                    strError = "Enter a Name 255 characters or less."
                    blnGood = False
                ElseIf strNewFMName.Length > 255 Then
                    strError = strNewFMName & "is longer than 255 characters."
                    blnGood = False
                End If

                'If blnGood is false let the user know why it is false

                If blnGood = False Then MsgBox(strError, , "Bad Fuel Model Number")
                If strNewFMName = "" Then Exit Sub 'Cancel was pushed
            Loop

            strSQL = "INSERT INTO LUT_FuelModelParameters (FMNum, FMCode, FL1H, FL10H, FL100H, FLLiveH, FLLiveW, " &
                     "FMType, H1SAV, LiveHSAV, LiveWSAV, Depth, XtMoist, DHt, LHt, FMName, DataType, Creator) " &
                            "VALUES ( " & strNewFMNum & ", " &
                            "'CST', " &
                            rdo1H.Text & ", " &
                            rdo10H.Text & ", " &
                            rdo100H.Text & ", " &
                            rdoLiveH.Text & ", " &
                            rdoLiveW.Text & ", '" &
                            chkFMType.Text & "', " &
                            rdo1HSAV.Text & ", " &
                            rdoLiveHSAV.Text & ", " &
                            rdoLiveWSAV.Text & ", " &
                            rdoDepth.Text & ", " &
                            rdoXtMoist.Text & ", " &
                            "8000, " &
                            "8000, '" &
                            strNewFMName & "', " &
                            "'English', " &
                            "'Custom')"
            dbconn.Execute(strSQL)

            Threading.Thread.Sleep(1000) 'Let the query catchup

            'Close the interface
            cmdCustomFM.Text = "Custom" & vbCrLf & "FM"
            grpCustFM.Visible = False

            cmbFM1.Items.Clear
            cmbFM2.Items.Clear
            cmbFM3.Items.Clear
            cmbFM4.Items.Clear
            cmbDefaultFM.Items.Clear

            'Populate Compare Fuel Model comboboxs
            cmbFM1.Items.Add("None")
            cmbFM2.Items.Add("None")
            cmbFM3.Items.Add("None")
            cmbFM4.Items.Add("None")

            strSQL = "SELECT FMNum " &
                    "FROM LUT_FuelModelParameters " &
                    "ORDER BY FMNum"

            gf_SetControl(cmbFM1, strSQL, strProjectPath)
            gf_SetControl(cmbFM2, strSQL, strProjectPath)
            gf_SetControl(cmbFM3, strSQL, strProjectPath)
            gf_SetControl(cmbFM4, strSQL, strProjectPath)

            'Populate starting Fuel Model combobox
            strSQL = "SELECT FMNum " &
                    "FROM LUT_FuelModelParameters " &
                    "ORDER BY FMNum"

            gf_SetControl(cmbDefaultFM, strSQL, strProjectPath)

            cmbDefaultFM.SelectedIndex = 0

            cmbFM1.Text = "None"
            cmbFM2.Text = "None"
            cmbFM3.Text = "None"
            cmbFM4.Text = "None"

            If rs1.State <> 0 Then rs1.Close
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close 'Database needs to be closed
            dbconn = Nothing
            MsgBox("Error in cmdSaveCSTFM_Click - " & ex.Message)
        End Try
    End Sub

    Private Sub cmdDelCstFM_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open

        Try
            Dim strErrAnderson = "Error: Anderson 13 fuel models cannot be deleted"
            Dim strErrNonBurnable = "Error: Default nonburnable fuel models cannot be deleted"
            Dim strErrScottBurgan = "Error: Scott and Burgan fuel models cannot be deleted"

            If cmbDefaultFM.Text <= 13 Then
                MsgBox("Error: Anderson 13 fuel models cannot be deleted")
            ElseIf cmbDefaultFM.Text >= 91 And cmbDefaultFM.Text <= 93 Or
                   cmbDefaultFM.Text >= 98 And cmbDefaultFM.Text <= 99 Then
                MsgBox("Error: Default nonburnable fuel models cannot be deleted")
            ElseIf cmbDefaultFM.Text >= 101 And cmbDefaultFM.Text <= 109 Or
                   cmbDefaultFM.Text >= 121 And cmbDefaultFM.Text <= 124 Or
                   cmbDefaultFM.Text >= 141 And cmbDefaultFM.Text <= 149 Or
                   cmbDefaultFM.Text >= 161 And cmbDefaultFM.Text <= 165 Or
                   cmbDefaultFM.Text >= 181 And cmbDefaultFM.Text <= 189 Or
                   cmbDefaultFM.Text >= 201 And cmbDefaultFM.Text <= 204 Then
                MsgBox("Error: Scott and Burgan fuel models cannot be deleted")
            ElseIf MsgBox("Do you really want to delete fuel model " & cmbDefaultFM.Text & "." & vbCrLf,
                          vbYesNo, "Delete Rule?") = vbYes Then

                strSQL = "DELETE FROM LUT_FuelModelParameters " &
                         "WHERE FMNum = " & cmbDefaultFM.Text
                dbconn.Execute(strSQL)

                Threading.Thread.Sleep(1000) 'Give the database a change to catchup

                'Reset the comboxes
                cmbFM1.Items.Clear
                cmbFM2.Items.Clear
                cmbFM3.Items.Clear
                cmbFM4.Items.Clear
                cmbDefaultFM.Items.Clear

                'Populate Compare Fuel Model comboboxs
                cmbFM1.Items.Add("None")
                cmbFM2.Items.Add("None")
                cmbFM3.Items.Add("None")
                cmbFM4.Items.Add("None")

                strSQL = "SELECT FMNum " &
                        "FROM LUT_FuelModelParameters " &
                        "ORDER BY FMNum"

                gf_SetControl(cmbFM1, strSQL, strProjectPath)
                gf_SetControl(cmbFM2, strSQL, strProjectPath)
                gf_SetControl(cmbFM3, strSQL, strProjectPath)
                gf_SetControl(cmbFM4, strSQL, strProjectPath)

                'Populate starting Fuel Model combobox
                strSQL = "SELECT FMNum " &
                        "FROM LUT_FuelModelParameters " &
                        "ORDER BY FMNum"

                gf_SetControl(cmbDefaultFM, strSQL, strProjectPath)

                cmbDefaultFM.SelectedIndex = 0

                cmbFM1.Text = "None"
                cmbFM2.Text = "None"
                cmbFM3.Text = "None"
                cmbFM4.Text = "None"
            End If

            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If

            MsgBox("Error in cmdDelCstFM_Click - " & ex.Message)
        End Try
    End Sub

    'Private Sub lstVwRulesets_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstVwRulesets.MouseDoubleClick
    '    'The sql statements to pass to the Visual Rule Query
    '    Dim strSQLEVT, strSQLRUle As String
    '    'Boolean value for checking to see if a layer exists
    '    Dim checkForaLayer As Boolean
    '    'MU layer name
    '    Dim strMULayerName As String
    '    'Mouse point at the selected rule
    '    Dim MousePt As Point
    '    'Index of the selected rule
    '    Dim SelectedRuleIndex As Integer
    '    'EVT number only by EVT combination selection
    '    Dim evtNum As Integer
    '    'DIST number only by EVT combination selection
    '    Dim distNum As Integer
    '    'Get IMap
    '    Dim pMap As ESRI.ArcGIS.Carto.IMap
    '    'Get raster check layer
    '    Dim pLayer As ESRI.ArcGIS.Carto.IRasterLayer
    '    'Get EnumLayer to search through
    '    Dim pEnumLayer As ESRI.ArcGIS.Carto.IEnumLayer

    '    evtNum = gf_GetNum(cmbEVT.Text, "EVT")
    '    distNum = gf_GetNum(cmbEVT.Text, "DIST")

    '    MousePt = e.Location
    '    strCMSItem = lstVwRulesets.Items.Item(lstVwRulesets.GetItemAt(MousePt.X, MousePt.Y).Index).GetSubItemAt(MousePt.X, MousePt.Y).Tag
    '    lstVwRulesets.SelectedItems.Clear()
    '    lstVwRulesets.Items.Item(lstVwRulesets.GetItemAt(MousePt.X, MousePt.Y).Index).Selected = True
    '    SelectedRuleIndex = lstVwRulesets.SelectedItems(0).Index + 1

    '    'Set the Rule
    '    ruleE = RulesetCollection.Item(SelectedRuleIndex)
    '    Try
    '        strMULayerName = Strings.Left(comboR, Len(comboR) - 4)
    '        'Check for MU in TOC
    '        pMap = gs_pMxDoc.FocusMap

    '        pEnumLayer = pMap.Layers
    '        pLayer = pEnumLayer.Next

    '        Do While Not pLayer Is Nothing
    '            If UCase(pLayer.Name) = UCase(strMULayerName) Then
    '                checkForaLayer = True
    '                Exit Do
    '            End If
    '            pLayer = pEnumLayer.Next
    '        Loop

    '        If checkForaLayer = True Then
    '            'Query for selected EVT and Rule
    '            If IsNumeric(ruleE.BPS) Then 'Me.BPS is a number and does not equal "any"
    '                strSQLRUle = "SELECT Value FROM " & comboR & " WHERE " &
    '                    "(EVTR = " & evtNum & " And DIST = " & distNum & " And " &
    '                    "EVCR Between " & ruleE.IntCovLow & " And " & ruleE.IntCovHigh & " And " &
    '                    "EVHR Between " & ruleE.IntHgtLow & " And " & ruleE.IntHgtHigh & " And " &
    '                    "BPSRF = " & ruleE.BPS & " And Wildcard = '" & ruleE.Wildcard & "')" &
    '                    " Or " &
    '                    "(EVTR = " & evtNum & " And DIST = " & distNum & " And " &
    '                    "EVCR Between " & ruleE.IntCovLow & " And " & ruleE.IntCovHigh & " And " &
    '                    "EVHR Between " & ruleE.IntHgtLow & " And " & ruleE.IntHgtHigh & " And " &
    '                    "BPSRF = " & ruleE.BPS & " And '" & ruleE.Wildcard & "' = 'any')" &
    '                    " ORDER BY Value"
    '            Else 'Me. BPS is a string and equals "any"
    '                strSQLRUle = "SELECT Value FROM " & comboR & " WHERE " &
    '                    "(EVTR = " & evtNum & " And DIST = " & distNum & " And " &
    '                    "EVCR Between " & ruleE.IntCovLow & " And " & ruleE.IntCovHigh & " And " &
    '                    "EVHR Between " & ruleE.IntHgtLow & " And " & ruleE.IntHgtHigh & " And '" &
    '                    ruleE.BPS & "' = 'any' And Wildcard = '" & ruleE.Wildcard & "')" &
    '                    " Or " &
    '                    "(EVTR = " & evtNum & " And DIST = " & distNum & " And " &
    '                    "EVCR Between " & ruleE.IntCovLow & " And " & ruleE.IntCovHigh & " And " &
    '                    "EVHR Between " & ruleE.IntHgtLow & " And " & ruleE.IntHgtHigh & " And '" &
    '                    ruleE.BPS & "' = 'any' And '" & ruleE.Wildcard & "' = 'any')" &
    '                    " ORDER BY Value"
    '            End If

    '            strSQLEVT = "SELECT EVTR, DIST " &
    '                    "FROM " & comboR & " " &
    '                    "GROUP BY EVTR, DIST " &
    '                    "HAVING (EVTR = " & evtNum & " And DIST = " & distNum & ")"

    '            'Run VisualRule
    '            VisualRuleQuery(pLayer, strSQLEVT, strSQLRUle, strProjectPath)
    '        Else
    '            'layer is not in the TOC
    '            MsgBox(" Add " & strMULayerName & " from project " & strProjectPath & vbCrLf &
    '                   " MU folder to use double click Visual Rule Query.")
    '        End If

    '    Catch ex As System.Exception
    '        'Do nothing
    '        MsgBox("lstVwRulesets_MouseDoubleClick: " & ex.Message)
    '    End Try

    '    pLayer = Nothing
    '    pEnumLayer = Nothing
    '    pMap = Nothing
    'End Sub

    Private Sub lstVwRulesets_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstVwRulesets.MouseDown
        Dim MousePt As Point
        Dim Index As Integer

        If e.Button = System.Windows.Forms.MouseButtons.Right Then
            Try
                MousePt = e.Location
                strCMSItem = lstVwRulesets.Items.Item(lstVwRulesets.GetItemAt(MousePt.X, MousePt.Y).Index).GetSubItemAt(MousePt.X, MousePt.Y).Tag

                lstVwRulesets.SelectedItems.Clear()
                lstVwRulesets.Items.Item(lstVwRulesets.GetItemAt(MousePt.X, MousePt.Y).Index).Selected = True
                Index = lstVwRulesets.SelectedItems(0).Index + 1

                'Set the Rule
                ruleE = RulesetCollection.Item(Index)

                'Offset Mouse point
                MousePt.X = Location.X + MousePt.X
                MousePt.Y = Location.Y + MousePt.Y + 230

                If strCMSItem = "CovLH" Or strCMSItem = "HgtLH" Then
                    'Show high low cms
                    cmsLowHigh.Items.Add("Sort by Cover (Default)")
                    cmsLowHigh.Items.Add("Sort by Height")
                    cmsLowHigh.Items.Add("Add New rule to edit")
                    cmsLowHigh.Items.Add("Edit Low side of range")
                    cmsLowHigh.Items.Add("Edit High side of range")
                    cmsLowHigh.Show(MousePt)
                Else
                    PopCMSEditRule("", MousePt)
                End If
            Catch ex As Exception
                MsgBox("Right click the part of the rule you want to edit.")
            End Try
        End If
    End Sub

    'Public Sub VisualRuleQuery(ByVal pMULayer As ESRI.ArcGIS.Carto.IRasterLayer,
    '                           ByVal strSQLEVT As String, ByVal strSQLRule As String,
    '                           ByVal strProjPath As String)
    '    Dim rsFoundPoint As Integer 'Stores the last know found index
    '    Dim frmVisQuery = New frmVisualQueryStatus()

    '    Dim rs1 As New ADODB.Recordset                                  'recordset for data
    '    Dim rs2 As New ADODB.Recordset                                  'recordset for data

    '    Dim dbconn As New ADODB.Connection                              'DB connection
    '    dbconn.ConnectionString = gs_DBConnection &
    '    strProjectPath & "\" & gs_LFTFCDBName
    '    dbconn.Open()

    '    Try
    '        frmVisQuery.Show()

    '        ' Get raster input from layer
    '        Dim pRaster As ESRI.ArcGIS.Geodatabase.IRaster
    '        pRaster = pMULayer.Raster

    '        ' Get the number of rows from raster table
    '        Dim pTable As ESRI.ArcGIS.Geodatabase.ITable
    '        Dim pband As ESRI.ArcGIS.DataSourcesRaster.IRasterBand
    '        Dim pBandCol As ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection
    '        pBandCol = pRaster
    '        pband = pBandCol.Item(0)
    '        Dim TableExist As Boolean
    '        pband.HasTable(TableExist)
    '        If Not TableExist Then Exit Sub
    '        pTable = pband.AttributeTable
    '        Dim NumOfValues As Integer
    '        NumOfValues = pTable.RowCount(Nothing)

    '        '' Create random color
    '        'Dim pRamp As ESRI.ArcGIS.Display.IRandomColorRamp
    '        'pRamp = New ESRI.ArcGIS.Display.RandomColorRamp
    '        'pRamp.Size = NumOfValues
    '        'pRamp.Seed = 100
    '        'pRamp.CreateRamp(True)
    '        Dim pFSymbol As ESRI.ArcGIS.Display.ISimpleFillSymbol

    '        ' Create UniqueValue renderer and QI RasterRenderer
    '        Dim pRen As ESRI.ArcGIS.Carto.IRasterUniqueValueRenderer
    '        pRen = New ESRI.ArcGIS.Carto.RasterUniqueValueRenderer
    '        Dim pRasRen As ESRI.ArcGIS.Carto.IRasterRenderer
    '        pRasRen = pRen

    '        ' Connect renderer and raster
    '        pRasRen.Raster = pRaster
    '        pRasRen.Update()

    '        ' Set UniqueValue renderer
    '        pRen.HeadingCount = 1   ' Use one heading
    '        pRen.Heading(0) = "EVT:Blue & Rule:Yellow"
    '        pRen.ClassCount(0) = NumOfValues

    '        Dim i As Long
    '        Dim pRow As ESRI.ArcGIS.Geodatabase.IRow
    '        Dim LabelValue As Object

    '        'Open database, run the SQL statement
    '        rs1.Open(strSQLEVT, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
    '        rs2.Open(strSQLRule, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

    '        For i = 0 To NumOfValues - 1
    '            pRow = pTable.GetRow(i) 'Get a row from the table

    '            Dim pColor As ESRI.ArcGIS.Display.IRgbColor
    '            pColor = New ESRI.ArcGIS.Display.RgbColor

    '            LabelValue = pRow.Value(3)  ' Get value of the given index
    '            pColor.RGB = RGB(0, 0, 0)
    '            If LabelValue = rs1.Fields(0).Value Then
    '                LabelValue = pRow.Value(1)  ' Get value of the given index
    '                'Bright blue for EVT
    '                pColor.RGB = RGB(0, 255, 197)

    '                rs2.Find("VALUE = " & pRow.Value(1))
    '                If rs2.EOF = False Then
    '                    rsFoundPoint = rs2.AbsolutePosition
    '                    'Bright red for rule
    '                    pColor.RGB = RGB(255, 0, 0)
    '                End If
    '                If rs2.EOF = True Then
    '                    rs2.MoveFirst()
    '                    rs2.Move(rsFoundPoint)
    '                End If
    '            End If
    '            LabelValue = pRow.Value(1)  ' Get value of the given index
    '            pRen.AddValue(0, i, LabelValue)  'Set value for the renderer
    '            pRen.Label(0, i) = CStr(LabelValue)  ' Set label
    '            pFSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbol
    '            If pColor.RGB = RGB(0, 0, 0) Then
    '                pColor.NullColor = True
    '                pFSymbol.Color = pColor
    '            Else
    '                pFSymbol.Color = pColor
    '            End If

    '            pRen.Symbol(0, i) = pFSymbol  'Set symbol
    '            frmVisQuery.ChangeProgress(i / NumOfValues * 100, NumOfValues - i)
    '        Next i

    '        ' Update render and refresh layer
    '        pRasRen.Update()
    '        pMULayer.Renderer = pRen
    '        Dim pLegInfo As ESRI.ArcGIS.Carto.ILegendInfo
    '        pLegInfo = pRen
    '        Dim pLegGroup As ESRI.ArcGIS.Carto.ILegendGroup
    '        pLegGroup = pLegInfo.LegendGroup(0)
    '        pLegGroup.Visible = False
    '        gs_pMxDoc.ActiveView.Refresh()
    '        gs_pMxDoc.UpdateContents()

    '        'pDoc = ThisDocument
    '        'Dim pMap As IMap
    '        'pMap = pDoc.FocusMap

    '        'Dim pLayer As IRasterLayer
    '        'pLayer = pMap.Layer(0)

    '        'Dim pRend As IRasterRenderer
    '        'pRend = pLayer.Renderer
    '        'Dim pLegInfo As ILegendInfo
    '        'pLegInfo = pRend
    '        'Dim pLegGroup As ILegendGroup
    '        'pLegGroup = pLegInfo.LegendGroup(0)
    '        'pLegGroup.Visible = False
    '        'pDoc.UpdateContents()

    '        frmVisQuery.Close()

    '        ' Clean up
    '        frmVisQuery = Nothing
    '        pLegInfo = Nothing
    '        pLegGroup = Nothing
    '        pMULayer = Nothing
    '        pRen = Nothing
    '        pRasRen = Nothing
    '        'pRamp = Nothing
    '        pFSymbol = Nothing
    '        pRaster = Nothing
    '        pband = Nothing
    '        pBandCol = Nothing
    '        pTable = Nothing
    '        pRow = Nothing

    '        If rs1.State <> 0 Then rs1.Close()
    '        rs1 = Nothing
    '        If rs2.State <> 0 Then rs2.Close()
    '        rs2 = Nothing

    '        If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
    '        dbconn = Nothing
    '    Catch ex As Exception
    '        If rs1.State <> 0 Then rs1.Close()
    '        rs1 = Nothing
    '        If rs2.State <> 0 Then rs2.Close()
    '        rs2 = Nothing

    '        If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
    '        dbconn = Nothing
    '        MsgBox("Visual Rule Query: " & ex.Message)
    '    End Try
    'End Sub

    Private Sub cmsEditRule_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles cmsEditRule.Closing
        cmsEditRule.Items.Clear()
    End Sub

    Private Sub cmsLowHigh_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles cmsLowHigh.Closing
        cmsLowHigh.Items.Clear()
    End Sub

    Private Sub PopCMSEditRule(ByVal strClickedLowHigh As String, ByVal MPoint As Point)
        Dim strNum As String
        Dim strCode As String

        If IsEVTSelected() Then
            Dim rs1 As New ADODB.Recordset                                  'recordset for data
            Dim dbconn As New ADODB.Connection                              'DB connection
            dbconn.ConnectionString = gs_DBConnection &
            strProjectPath & "\" & gs_LFTFCDBName
            dbconn.Open()

            Try
                If strCMSItem = "CovLH" Then
                    'Populate cmsEditRule values
                    strSQL = "SELECT EVCR FROM " & comboR & " " &
                   "WHERE (EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") &
                   " And DIST = " & gf_GetNum(cmbEVT.Text, "DIST") & ")" &
                   " Group By EVCR ORDER BY EVCR"

                    addToCMSEditRule(strSQL, "cov", strClickedLowHigh, ruleE.IntCovLow, ruleE.IntCovHigh)
                ElseIf strCMSItem = "HgtLH" Then
                    'Populate cmsEditRule values
                    strSQL = "SELECT EVHR FROM " & comboR & " " &
                   "WHERE (EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") &
                   " And DIST = " & gf_GetNum(cmbEVT.Text, "DIST") & ")" &
                   " Group By EVHR ORDER BY EVHR"

                    addToCMSEditRule(strSQL, "hgt", strClickedLowHigh, ruleE.IntHgtLow, ruleE.IntHgtHigh)
                ElseIf strCMSItem = "BPS" Then
                    'Populate cmsEditRule
                    strSQL = "SELECT " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                             "FROM " & comboR & " " &
                             "LEFT JOIN LUT_BPS ON " & comboR & ".BPSRF = LUT_BPS.BPS " &
                             "WHERE (EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") &
                             " And DIST = " & gf_GetNum(cmbEVT.Text, "DIST") & ")" &
                             " GROUP BY " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                             " ORDER BY BPSRF"

                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    cmsEditRule.Items.Add("any")

                    Do Until rs1.EOF
                        cmsEditRule.Items.Add(rs1.Fields(0).Value & "   " & rs1.Fields(1).Value & " - " &
                                           rs1.Fields(2).Value) 'Combine BPS#,BPS Name,and BPS
                        rs1.MoveNext()
                    Loop
                ElseIf strCMSItem = "Wild" Then
                    'Populate cmsEditRule
                    strSQL = "SELECT WILDCARD FROM " & comboR & " " &
                   "WHERE (EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") &
                   " And DIST = " & gf_GetNum(cmbEVT.Text, "DIST") & ")" &
                   " Group By WILDCARD ORDER BY WILDCARD"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    cmsEditRule.Items.Add("any")

                    Do Until rs1.EOF
                        cmsEditRule.Items.Add(rs1.Fields(0).Value)
                        rs1.MoveNext()
                    Loop
                ElseIf strCMSItem = "FM13" Then
                    'Populate cmsEditRule
                    strSQL = "SELECT FMNum, FMName " &
                    "FROM LUT_FuelModelParameters " &
                    "WHERE (LUT_FuelModelParameters.Creator = 'Anderson13' Or " &
                    "LUT_FuelModelParameters.Creator = 'Nonburnable') " &
                    "ORDER BY FMNum"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    cmsEditRule.Items.Add("9999   Nothing Assigned")

                    Do Until rs1.EOF
                        cmsEditRule.Items.Add(rs1.Fields(0).Value & "   " & rs1.Fields(1).Value) '2 fields
                        rs1.MoveNext()
                    Loop
                ElseIf strCMSItem = "FM40" Then
                    'Populate cmsEditRule
                    strSQL = "SELECT FMNum, FMCode, FMName " &
                     "FROM LUT_FuelModelParameters " &
                     "WHERE (LUT_FuelModelParameters.Creator = 'ScottAndBurgan40' Or " &
                     "LUT_FuelModelParameters.Creator = 'Nonburnable') " &
                     " ORDER BY FMNum"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    cmsEditRule.Items.Add("     9999   Nothing Assigned")

                    Do Until rs1.EOF
                        strNum = rs1.Fields(0).Value
                        strCode = rs1.Fields(1).Value
                        If Strings.Len(strNum & "") = 1 Then strNum = "00" & strNum
                        If Strings.Len(strNum & "") = 2 Then strNum = "0" & strNum
                        If Strings.Len(strCode & "") = 1 Then strCode = "00" & strCode
                        If Strings.Len(strCode & "") = 2 Then strCode = "0" & strCode
                        cmsEditRule.Items.Add(strCode & " / " & strNum & "   " &
                                           rs1.Fields(2).Value) 'Combine FMNum / FMCode  FMName
                        rs1.MoveNext()
                    Loop
                ElseIf strCMSItem = "CanFM" Then
                    'Populate cmsEditRule
                    strSQL = "SELECT FM, Description " &
                     "FROM LUT_Canadian_FBPS_Fuel_Types " &
                     "WHERE (((LUT_Canadian_FBPS_Fuel_Types.FMID)<>0 And (LUT_Canadian_FBPS_Fuel_Types.FMID)<>-9999)) " &
                     "ORDER BY ID"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs1.EOF
                        cmsEditRule.Items.Add(rs1.Fields(0).Value & "   " & rs1.Fields(1).Value) '2 fields
                        rs1.MoveNext()
                    Loop
                    'ElseIf strCMSItem = "FCCS" Then
                    '    'Populate cmsEditRule
                    '    strSQL = "SELECT FCCS, Description " &
                    '     "FROM LUT_FCCS_FERA " &
                    '     " ORDER BY ID"
                    '    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    '    Do Until rs1.EOF
                    '        cmsEditRule.Items.Add(rs1.Fields(0).Value & "   " & rs1.Fields(1).Value) '2 fields
                    '        rs1.MoveNext()
                    '    Loop
                ElseIf strCMSItem = "FLM" Then
                    'Populate cmsEditRule
                    strSQL = "SELECT FLM, Description " &
                        "FROM LUT_FLM_Lutes " &
                        " ORDER BY ID"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs1.EOF
                        cmsEditRule.Items.Add(rs1.Fields(0).Value & "   " & rs1.Fields(1).Value) '2 fields
                        rs1.MoveNext()
                    Loop
                ElseIf strCMSItem = "CG" Then
                    'Populate cmsEditRule
                    strSQL = "SELECT Canopy_Fuel_Mask, Description " &
                     "FROM LUT_Canopy_Fuel_Mask " &
                     " ORDER BY ID"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs1.EOF
                        cmsEditRule.Items.Add(rs1.Fields(0).Value & "   " & rs1.Fields(1).Value) '2 fields
                        rs1.MoveNext()
                    Loop
                ElseIf strCMSItem = "CC" Then
                    'Populate cmsEditRule
                    cmsEditRule.Items.Add("9999")

                    strSQL = "SELECT LUT_Cover.Lifeform, LUT_Cover.MidPoint " &
                             "FROM LUT_Cover " &
                             "WHERE (((LUT_Cover.Lifeform)='Tree'))"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs1.EOF
                        cmsEditRule.Items.Add(rs1.Fields!MidPoint.Value) 'Fill with Midpoint Values
                        rs1.MoveNext()
                    Loop
                ElseIf strCMSItem = "CH" Then
                    'Populate cmsEditRule
                    cmsEditRule.Items.Add("9999")

                    strSQL = "SELECT LUT_Height.Lifeform, LUT_Height.MidPoint " &
                             "FROM LUT_Height " &
                             "WHERE (((LUT_Height.Lifeform)='Tree'))"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs1.EOF
                        cmsEditRule.Items.Add(rs1.Fields!MidPoint.Value * 10 & "(m x 10)") 'Fill with Midpoint Values
                        rs1.MoveNext()
                    Loop
                ElseIf strCMSItem = "CBD13" Then
                    'Populate cmsEditRule
                    With cmsEditRule
                        .Items.Add("9999")
                        For i = 1 To 45
                            .Items.Add(i & " kg/m^3x100")
                        Next i
                    End With
                ElseIf strCMSItem = "CBD40" Then
                    'Populate cmsEditRule
                    With cmsEditRule
                        .Items.Add("9999")
                        For i = 1 To 45
                            .Items.Add(i & " kg/m^3x100")
                        Next i
                    End With

                ElseIf strCMSItem = "CBH13" Then
                    'Populate cmsEditRule
                    With cmsEditRule
                        .Items.Add("9999")
                        For i = 1 To 100
                            .Items.Add(i & " mx10")
                        Next i
                    End With

                ElseIf strCMSItem = "CBH40" Then
                    'Populate cmsEditRule
                    With cmsEditRule
                        .Items.Add("9999")
                        For i = 1 To 100
                            .Items.Add(i & " mx10")
                        Next i
                    End With

                ElseIf strCMSItem = "OnOff" Then
                    'Populate cmsEditRule
                    With cmsEditRule
                        .Items.Add("On")
                        .Items.Add("Off")
                    End With
                End If

                cmsEditRule.Show(MPoint)

                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing

                If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
            Catch ex As Exception
                If rs1.State <> 0 Then rs1.Close()
                rs1 = Nothing

                If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
                dbconn = Nothing
                MsgBox("PopCMSEditRule - " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub cmsLowHigh_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmsLowHigh.ItemClicked
        PopCMSEditRule(e.ClickedItem.Text, cmsLowHigh.Location)
    End Sub

    Private Sub cmsEditRule_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmsEditRule.ItemClicked
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            If IsEVTSelected() Then
                Dim Index As Integer        'Stores the index of the selected value
                Dim strNewNote As String    'Stores the notes
                Dim minCov As Integer = 9999     'Used in dao seek for a given lifeform 
                Dim minHgt As Integer = 9999     'Used in dao seek for a given lifeform

                'Get Selected rule
                Index = lstVwRulesets.SelectedItems(0).Index + 1

                'Set the beginning of the note
                strNewNote = ruleE.Notes & vbCrLf & Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                                        txtSessionName.Text & ": Changed "

                'Update the collection and the database
                If strCMSItem = "cov low" Then
                    If ruleE.StrCovLow <> e.ClickedItem.Text Then
                        strNewNote = strNewNote & "  (" & ruleE.StrCovLow & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrCovLow = e.ClickedItem.Text
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                    'Make all the values the same if Low cover is lessthan 101
                    If Int(gf_ConvertBack(e.ClickedItem.Text, strProjectPath)) < 101 Then
                        'Cover High
                        strNewNote = strNewNote & "  (" & ruleE.StrCovHigh & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrCovHigh = e.ClickedItem.Text
                        'Height Low
                        strNewNote = strNewNote & "  (" & ruleE.StrHgtLow & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrHgtLow = e.ClickedItem.Text
                        'Height High
                        strNewNote = strNewNote & "  (" & ruleE.StrHgtHigh & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrHgtHigh = e.ClickedItem.Text
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                ElseIf strCMSItem = "cov high" Then
                    If ruleE.StrCovHigh <> e.ClickedItem.Text Then
                        strNewNote = strNewNote & "  (" & ruleE.StrCovHigh & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrCovHigh = e.ClickedItem.Text
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                ElseIf strCMSItem = "hgt low" Then
                    If ruleE.StrHgtLow <> e.ClickedItem.Text Then
                        strNewNote = strNewNote & "  (" & ruleE.StrHgtLow & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrHgtLow = e.ClickedItem.Text
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                    'Make all the values the same if Low cover is lessthan 101
                    If Int(gf_ConvertBack(e.ClickedItem.Text, strProjectPath)) < 101 Then
                        'Height High
                        strNewNote = strNewNote & "  (" & ruleE.StrHgtHigh & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrHgtHigh = e.ClickedItem.Text
                        'Cover Low
                        strNewNote = strNewNote & "  (" & ruleE.StrCovLow & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrCovLow = e.ClickedItem.Text
                        'Cover High
                        strNewNote = strNewNote & "  (" & ruleE.StrCovHigh & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrCovHigh = e.ClickedItem.Text
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                ElseIf strCMSItem = "hgt high" Then
                    If ruleE.StrHgtHigh <> e.ClickedItem.Text Then
                        strNewNote = strNewNote & "  (" & ruleE.StrHgtHigh & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.StrHgtHigh = e.ClickedItem.Text
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                ElseIf strCMSItem = "Add New Rule" Then
                    'Find lifeform of selected value for cover and height and populate low first
                    strSQL = "SELECT LUT_Cover.Lifeform, Min(LUT_Cover.EVC) AS MinOfEVC, Min(LUT_Height.EVH) AS MinOfEVH " &
                             "FROM LUT_Height INNER JOIN LUT_Cover ON LUT_Height.Lifeform = LUT_Cover.Lifeform " &
                             "GROUP BY LUT_Cover.Lifeform " &
                             "HAVING (((LUT_Cover.Lifeform)='Herb')) OR " &
                             "(((LUT_Cover.Lifeform)='Shrub')) OR " &
                             "(((LUT_Cover.Lifeform)='Sparse')) OR " &
                             "(((LUT_Cover.Lifeform)='Tree'))"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    If e.ClickedItem.Text = "Add Ag, Urban, Developed, Or Sparse Rule" Then
                        Do Until rs1.EOF = True
                            If rs1.Fields!Lifeform.Value = "Sparse" Then
                                minCov = rs1.Fields!MinOfEVC.Value
                                minHgt = rs1.Fields!MinOfEVH.Value
                                Exit Do
                            End If
                            rs1.MoveNext()
                        Loop

                        strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                                "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                                "CBH13mx10, CBH40mx10, OnOff, Notes) " &
                                "VALUES (" & gf_GetNum(cmbEVT.Text, "EVT") & ", " & gf_GetNum(cmbEVT.Text, "DIST") & ", " &
                                minCov & ", " & minCov & ", " & minHgt & ", " & minHgt & ", 'any', 'any', 9999, 9999, '9999', '9999', 9999, 9999, 0, 9999, 9999, " &
                                "9999, 9999, 9999, 'On', '" & Now.ToShortTimeString & " " & Now.ToShortDateString &
                                " " & txtSessionName.Text & ": NEW Ag, Urban, Developed, Or Sparse Rule')"
                    ElseIf e.ClickedItem.Text = "Add Herb Rule" Then
                        Do Until rs1.EOF = True
                            If rs1.Fields!Lifeform.Value = "Herb" Then
                                minCov = rs1.Fields!MinOfEVC.Value
                                minHgt = rs1.Fields!MinOfEVH.Value
                                Exit Do
                            End If
                            rs1.MoveNext()
                        Loop
                        minCov = rs1.Fields!MinOfEVC.Value
                        minHgt = rs1.Fields!MinOfEVH.Value
                        strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                                "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                                "CBH13mx10, CBH40mx10, OnOff, Notes) " &
                                "VALUES (" & gf_GetNum(cmbEVT.Text, "EVT") & ", " & gf_GetNum(cmbEVT.Text, "DIST") & ", " &
                                minCov & ", " & minCov & ", " & minHgt & ", " & minHgt & ", 'any', 'any', 9999, 9999, '9999', '9999', 9999, 9999, 0, 9999, 9999, " &
                                "9999, 9999, 9999, 'On', '" & Now.ToShortTimeString & " " & Now.ToShortDateString &
                                " " & txtSessionName.Text & ": NEW Herb Rule')"
                    ElseIf e.ClickedItem.Text = "Add Shrub Rule" Then
                        Do Until rs1.EOF = True
                            If rs1.Fields!Lifeform.Value = "Shrub" Then
                                minCov = rs1.Fields!MinOfEVC.Value
                                minHgt = rs1.Fields!MinOfEVH.Value
                                Exit Do
                            End If
                            rs1.MoveNext()
                        Loop
                        strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                                "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                                "CBH13mx10, CBH40mx10, OnOff, Notes) " &
                                "VALUES (" & gf_GetNum(cmbEVT.Text, "EVT") & ", " & gf_GetNum(cmbEVT.Text, "DIST") & ", " &
                                minCov & ", " & minCov & ", " & minHgt & ", " & minHgt & ", 'any', 'any', 9999, 9999, '9999', '9999', 9999, 9999, 0, 9999, 9999, " &
                                "9999, 9999, 9999, 'On', '" & Now.ToShortTimeString & " " & Now.ToShortDateString &
                                " " & txtSessionName.Text & ": NEW Shrub Rule')"
                    ElseIf e.ClickedItem.Text = "Add Tree Rule" Then
                        Do Until rs1.EOF = True
                            If rs1.Fields!Lifeform.Value = "Tree" Then
                                minCov = rs1.Fields!MinOfEVC.Value
                                minHgt = rs1.Fields!MinOfEVH.Value
                                Exit Do
                            End If
                            rs1.MoveNext()
                        Loop
                        strSQL = "INSERT INTO " & rulesR & "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                                "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                                "CBH13mx10, CBH40mx10, OnOff, Notes) " &
                                "VALUES (" & gf_GetNum(cmbEVT.Text, "EVT") & ", " & gf_GetNum(cmbEVT.Text, "DIST") & ", " &
                                minCov & ", " & minCov & ", " & minHgt & ", " & minHgt & ", 'any', 'any', 9999, 9999, '9999', '9999', 9999, 9999, 1, 9999, 9999, " &
                                "9999, 9999, 9999, 'On', '" & Now.ToShortTimeString & " " & Now.ToShortDateString &
                                " " & txtSessionName.Text & ": NEW Tree Rule')"
                    End If

                    'Run the SQL statement
                    dbconn.Execute(strSQL)

                    gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                                  RulesetCollection, EVTPixelCountCollection, strProjectPath)
                    DisplayRuleset()
                ElseIf strCMSItem = "BPS" Then
                    If ruleE.BPS <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.BPS & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.BPS = gf_GetNum(e.ClickedItem.Text, "General")
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                ElseIf strCMSItem = "Wild" Then
                    If ruleE.Wildcard <> e.ClickedItem.Text Then
                        strNewNote = strNewNote & "  (" & ruleE.Wildcard & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.Wildcard = e.ClickedItem.Text
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                ElseIf strCMSItem = "FM13" Then
                    If ruleE.FBFM13 <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.FBFM13 & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.FBFM13 = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "FM40" Then
                    If ruleE.FBFM40 <> Trim(Strings.Left(e.ClickedItem.Text, 9)) Then
                        strNewNote = strNewNote & "  (" & ruleE.FBFM40 & ") to (" & Trim(Strings.Left(e.ClickedItem.Text, 9)) & ")"
                        ruleE.FBFM40 = Trim(Strings.Left(e.ClickedItem.Text, 9))
                    End If
                ElseIf strCMSItem = "CanFM" Then
                    If ruleE.CanFM <> Trim(Strings.Left(e.ClickedItem.Text, 9)) Then
                        strNewNote = strNewNote & "  (" & ruleE.CanFM & ") to (" & Trim(Strings.Left(e.ClickedItem.Text, 9)) & ")"
                        ruleE.CanFM = Trim(Strings.Left(e.ClickedItem.Text, 9))
                    End If
                ElseIf strCMSItem = "FCCS" Then
                    If ruleE.FCCS <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.FCCS & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.FCCS = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "FLM" Then
                    If ruleE.FLM <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.FLM & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.FLM = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "CG" Then
                    If ruleE.Canopy <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.Canopy & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.Canopy = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "CC" Then
                    If ruleE.CCover <> e.ClickedItem.Text Then
                        strNewNote = strNewNote & "  (" & ruleE.CCover & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.CCover = e.ClickedItem.Text
                    End If
                ElseIf strCMSItem = "CH" Then
                    If ruleE.CHeight <> e.ClickedItem.Text Then
                        strNewNote = strNewNote & "  (" & ruleE.CHeight & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.CHeight = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "CBD13" Then
                    If ruleE.CBD13 <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.CBD13 & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.CBD13 = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "CBD40" Then
                    If ruleE.CBD40 <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.CBD40 & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.CBD40 = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "CBH13" Then
                    If ruleE.CBH13 <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.CBH13 & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.CBH13 = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "CBH40" Then
                    If ruleE.CBH40 <> gf_GetNum(e.ClickedItem.Text, "General") Then
                        strNewNote = strNewNote & "  (" & ruleE.CBH40 & ") to (" & gf_GetNum(e.ClickedItem.Text, "General") & ")"
                        ruleE.CBH40 = gf_GetNum(e.ClickedItem.Text, "General")
                    End If
                ElseIf strCMSItem = "OnOff" Then
                    If ruleE.OnOff <> e.ClickedItem.Text Then
                        strNewNote = strNewNote & "  (" & ruleE.OnOff & ") to (" & e.ClickedItem.Text & ")"
                        ruleE.OnOff = e.ClickedItem.Text
                        gr_ClearPAP(RulesetCollection) 'Clears the Pixel count, Acres, and Percent evt of the ruleset
                    End If
                End If
                ruleE.Notes = strNewNote

                gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR, RulesetCollection,
                                  EVTPixelCountCollection, strProjectPath)
                DisplayRuleset()
                AdjPer()
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in cmsEditRule_ItemClicked - " & ex.Message)
        End Try
    End Sub

    Private Sub addToCMSEditRule(ByVal strSQLCSM As String, ByVal strCovOrHgt As String,
                                 ByVal strLowOrHigh As String, ByVal intGTOET As Integer,
                                 ByVal intLTOET As Integer)
        Dim rs1 As New ADODB.Recordset                                  'recordset for data
        Dim rs2 As New ADODB.Recordset                                  'recordset for data
        Dim rs3 As New ADODB.Recordset                                  'recordset for data
        Dim rs4 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            If IsEVTSelected() Then
                'LTOET - Less than or equal to
                'GTOET - Greater than or equal to

                Dim insertValue As Integer

                If strLowOrHigh = "Sort by Cover (Default)" Then
                    gr_SetRuleSort = "Sort by Cover"
                    gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR, RulesetCollection,
                             EVTPixelCountCollection, strProjectPath)
                    DisplayRuleset()
                ElseIf strLowOrHigh = "Sort by Height" Then
                    gr_SetRuleSort = "Sort by Height"
                    gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR, RulesetCollection,
                             EVTPixelCountCollection, strProjectPath)
                    DisplayRuleset()
                ElseIf strLowOrHigh = "Add New rule to edit" Then
                    strCMSItem = "Add New Rule"
                    cmsEditRule.Items.Add("Add Ag, Urban, Developed, Or Sparse Rule")
                    cmsEditRule.Items.Add("Add Herb Rule")
                    cmsEditRule.Items.Add("Add Shrub Rule")
                    cmsEditRule.Items.Add("Add Tree Rule")
                ElseIf strLowOrHigh = "Edit Low side of range" Then
                    'Set the strCMSItem
                    strCMSItem = strCovOrHgt & " low"

                    'Find lifeform of selected value for cover and height and populate low first
                    strSQL = "SELECT LUT_Cover.Lifeform, Min(LUT_Cover.EVC) AS MinOfEVC, Min(LUT_Height.EVH) AS MinOfEVH " &
                             "FROM LUT_Height INNER JOIN LUT_Cover ON LUT_Height.Lifeform = LUT_Cover.Lifeform " &
                             "GROUP BY LUT_Cover.Lifeform " &
                             "HAVING (((LUT_Cover.Lifeform)='Herb')) OR " &
                             "(((LUT_Cover.Lifeform)='Shrub')) OR " &
                             "(((LUT_Cover.Lifeform)='Tree'))"
                    rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    'First loop is herb, second is shrub, third is tree
                    Do Until rs1.EOF
                        If strCovOrHgt = "cov" And intGTOET >= rs1.Fields!MinOfEVC.Value Then      'If cov use intGTOET gets cover
                            intGTOET = rs1.Fields!MinOfEVC.Value
                            Exit Do
                        ElseIf strCovOrHgt = "hgt" And intGTOET >= rs1.Fields!MinOfEVH.Value Then  'If hgt use intGTOET gets height
                            intGTOET = rs1.Fields!MinOfEVH.Value
                            Exit Do
                        ElseIf intGTOET <= 100 Then                                                 'If Sparse or 2 digit it gets lowest
                            intGTOET = 11
                            Exit Do
                        Else
                            rs1.MoveNext()
                        End If
                    Loop

                    'Populate cmsEditRule
                    strSQL = strSQLCSM
                    rs2.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs2.EOF
                        insertValue = rs2.Fields(0).Value

                        If insertValue >= intGTOET And insertValue <= intLTOET Then
                            cmsEditRule.Items.Add(gf_ConvertCode(insertValue, strCovOrHgt, "low", strProjectPath))
                        End If
                        rs2.MoveNext()
                    Loop

                ElseIf strLowOrHigh = "Edit High side of range" Then
                    'Set the strCMSItem
                    strCMSItem = strCovOrHgt & " high"

                    'Find lifeform of selected value for cover and height and populate low first
                    strSQL = "SELECT LUT_Cover.Lifeform, Max(LUT_Cover.EVC) AS MaxOfEVC, Max(LUT_Height.EVH) AS MaxOfEVH " &
                             "FROM LUT_Height INNER JOIN LUT_Cover ON LUT_Height.Lifeform = LUT_Cover.Lifeform " &
                             "GROUP BY LUT_Cover.Lifeform " &
                             "HAVING (((LUT_Cover.Lifeform)='Herb')) OR " &
                             "(((LUT_Cover.Lifeform)='Shrub')) OR " &
                             "(((LUT_Cover.Lifeform)='Tree')) " &
                             "ORDER BY LUT_Cover.Lifeform DESC"
                    rs3.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    'First loop is Tree, second is shrub, third is herb
                    Do Until rs3.EOF
                        If strCovOrHgt = "cov" And intLTOET <= rs3.Fields!MaxOfEVC.Value Then      'If cov use intLTOET gets cover
                            intLTOET = rs3.Fields!MaxOfEVC.Value
                            Exit Do
                        ElseIf strCovOrHgt = "hgt" And intLTOET <= rs3.Fields!MaxOfEVH.Value Then  'If hgt use intLTOET gets height
                            intLTOET = rs3.Fields!MaxOfEVH.Value
                            Exit Do
                        ElseIf intLTOET <= 100 Then                                                 'If Sparse or 2 digit it gets lowest
                            intLTOET = 100
                            Exit Do
                        Else
                            rs3.MoveNext()
                        End If
                    Loop

                    'Populate cmsEditRule
                    strSQL = strSQLCSM
                    rs4.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                    Do Until rs4.EOF
                        insertValue = rs4.Fields(0).Value

                        If insertValue >= intGTOET And insertValue <= intLTOET Then
                            cmsEditRule.Items.Add(gf_ConvertCode(insertValue, strCovOrHgt, "high", strProjectPath))
                        End If
                        rs4.MoveNext()
                    Loop
                Else
                    'It is not a proper selection
                End If
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing
            If rs4.State <> 0 Then rs4.Close()
            rs4 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing
            If rs2.State <> 0 Then rs2.Close()
            rs2 = Nothing
            If rs3.State <> 0 Then rs3.Close()
            rs3 = Nothing
            If rs4.State <> 0 Then rs4.Close()
            rs4 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in addToCMSEditRule - " & ex.Message)
        End Try
    End Sub

    Private Function IsEVTSelected() As Boolean
        If cmbEVT.Text <> "" Then
            IsEVTSelected = True
        Else
            IsEVTSelected = False
            MsgBox("No EVT selected!" & vbCrLf &
                   "Select an EVT from the dropdown or " & vbCrLf &
                   "change Sort EVTs setting.")
        End If
    End Function

    Private Sub OrderAndSortEVT()
        Dim tempEVT As String = "False"                                 'Start with false for do until
        Dim LUT_Table As String                                         'Set the look up table
        Dim LUT_Name As String                                          'Set the lookup name field
        Dim LUT_Num As String                                           'Set the lookup number field
        Dim orderNameOrNumber As String                                 'Stores the order by string
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try

            LUT_Table = "XWALK_EVT_EVG_EVS"
            LUT_Name = "EVT_Name"
            LUT_Num = "EVT"

            If rdoName.Checked Then
                orderNameOrNumber = LUT_Table & "." & LUT_Name
            Else
                orderNameOrNumber = comboR & ".EVTR"
            End If

            Select Case cmbSortRules.SelectedIndex
                Case 0      'All by Type
                    strSQL = "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                             "FROM " & comboR & " LEFT JOIN " & LUT_Table & " " &
                             "ON " & comboR & ".EVTR = " & LUT_Table & "." & LUT_Num & " " &
                             "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                             "ORDER BY " & comboR & ".DIST, " & orderNameOrNumber
                Case 1      'All by EVT
                    strSQL = "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                             "FROM " & comboR & " LEFT JOIN " & LUT_Table & " " &
                             "ON " & comboR & ".EVTR=" & LUT_Table & "." & LUT_Num & " " &
                             "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & ", " &
                             comboR & ".DIST ORDER BY " & orderNameOrNumber & ", " & comboR & ".DIST"
                Case 2      'Disturbed by Type
                    strSQL = "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                             "FROM " & comboR & " LEFT JOIN " & LUT_Table & " " &
                             "ON " & comboR & ".EVTR=" & LUT_Table & "." & LUT_Num & " " &
                             "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                             "HAVING (((" & comboR & ".DIST) > 0)) " &
                             "ORDER BY " & comboR & ".DIST, " & orderNameOrNumber
                Case 3      'Disturbed by EVT
                    strSQL = "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                             "FROM " & comboR & " LEFT JOIN " & LUT_Table & " " &
                             "ON " & comboR & ".EVTR= " & LUT_Table & "." & LUT_Num & " " &
                             "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                             "HAVING(((" & comboR & ".DIST) > 0)) " &
                             "ORDER BY " & orderNameOrNumber & ", " & comboR & ".DIST"
                Case 4      'Specific EVT
                    Do Until tempEVT <> "False"
                        tempEVT = InputBox("Enter the 4 digit " & LUT_Num & " code you want to sort." &
                                           "Example " & LUT_Num & " 2227[0]: Enter 2227", "Sort for specific " & LUT_Num, "")
                        If tempEVT = "" Then
                            cmbSortRules.SelectedIndex = 0
                        Else
                            If tempEVT.Length = 4 And IsNumeric(tempEVT) Then
                                strSQL = "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                                         "FROM " & comboR & " LEFT JOIN " & LUT_Table & " " &
                                         "ON " & comboR & ".EVTR = " & LUT_Table & "." & LUT_Num & " " &
                                         "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & ", " &
                                         comboR & ".DIST " &
                                         "HAVING(((" & comboR & ".EVTR) = " & tempEVT & ")) " &
                                         "ORDER BY " & orderNameOrNumber & ", " & comboR & ".DIST"
                            Else
                                MsgBox(LUT_Num & " #: " & tempEVT & " does not exist in the Managament Unit" & vbCrLf &
                                       "Try another " & LUT_Num & " #")
                                tempEVT = "False"
                            End If
                        End If
                    Loop
                Case Else   'By Specific Disturbance Type
                    strSQL = "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & " " &
                             "FROM (" & comboR & " LEFT JOIN " & LUT_Table & " " &
                             "ON " & comboR & ".EVTR = " & LUT_Table & "." & LUT_Num & ") " &
                             "INNER JOIN LUT_DistCode ON " & comboR & ".DIST = LUT_DistCode.DistCode " &
                             "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & LUT_Table & "." & LUT_Name & ", " &
                             "LUT_DistCode.Type " &
                             "HAVING(((LUT_DistCode.Type) = """ & cmbSortRules.Text & """)) ORDER BY " & orderNameOrNumber
            End Select

            'Check for EVTs after selection if none then clear the CMBEVT and the rulesets
            rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

            If rs1.EOF Then
                MsgBox("No values do not exist in this MU" & vbCrLf &
                       "for the selected filter. Returning to" & vbCrLf &
                       "All by type.")
                cmbSortRules.SelectedIndex = 0
            Else
                'Clear cmbEVT
                cmbEVT.Items.Clear()

                'Set the values in cmbEVT
                gf_SetControl(cmbEVT, strSQL, strProjectPath, rdoName.Checked)

                'Set first value in the cmbEVT to the SelectedIndex
                If cmbEVT.Items.Count <> 0 Then cmbEVT.SelectedIndex = 0

                'Make rulesets and display the
                gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR, RulesetCollection,
                              EVTPixelCountCollection, strProjectPath)
                DisplayRuleset()
                AdjPer()
            End If

            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1.State <> 0 Then rs1.Close()
            rs1 = Nothing

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in OrderAndSortEVT- " & ex.Message)
        End Try
    End Sub

    Private Sub cmbSortRules_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSortRules.SelectionChangeCommitted
        OrderAndSortEVT()
    End Sub

    Private Sub rdoEVTName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoName.Click
        OrderAndSortEVT()
    End Sub

    Private Sub rdoEVTNumber_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoNumber.Click
        OrderAndSortEVT()
    End Sub

    Private Sub rdoCBHDistGraph_CheckedChanged(sender As Object, e As EventArgs) Handles rdoCBHDistGraph.CheckedChanged
        If rdoCBHDistGraph.Checked Then DistGraph()
    End Sub

    Private Sub rdoCBDDistGraph_CheckedChanged(sender As Object, e As EventArgs) Handles rdoCBDDistGraph.CheckedChanged
        If rdoCBDDistGraph.Checked Then DistGraph()
    End Sub

    Private Sub rdoCCDistGraph_CheckedChanged(sender As Object, e As EventArgs) Handles rdoCCDistGraph.CheckedChanged
        If rdoCCDistGraph.Checked Then DistGraph()
    End Sub

    Private Sub rdoCHDistGraph_CheckedChanged(sender As Object, e As EventArgs) Handles rdoCHDistGraph.CheckedChanged
        If rdoCHDistGraph.Checked Then DistGraph()
    End Sub

    Private Sub rdoNoneDistGraph_CheckedChanged(sender As Object, e As EventArgs) Handles rdoNoneDistGraph.CheckedChanged
        If rdoNoneDistGraph.Checked And strProjectPath <> "" Then DistGraph()
    End Sub

End Class