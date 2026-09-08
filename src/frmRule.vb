'System.Windows.Forms.DataVisualization.Charting
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports FastReport.DataVisualization.Charting
Imports Windows.Win32.System.Diagnostics
Imports System.Data.SQLite

Public Class frmRule

    ' Stored constructor parameters
    Private _comboR As String
    Private _rulesR As String
    Private _muName As String

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

        ' Save initial values (no async allowed in constructor)
        _comboR = setComboR
        _rulesR = setRulesR
        _muName = SetMUName

        ' Hide the form until after the python code is done
        Me.Hide()

    End Sub

    Public Async Function StartAsync() As Task

        Await InitializeFormAsync(_comboR, _rulesR, _muName)

        ' Show the Form
        Me.Show()
        Me.Activate()
    End Function

    Private Async Function InitializeFormAsync(setComboR As String,
                                               setRulesR As String,
                                               MU As String) As Task

        'Set local path variable
        strProjectPath = gs_ProjectPath

        'Declare variables
        Dim LUT_Table As String 'Set the look up table
        Dim LUT_Name As String 'Set the lookup name field
        Dim LUT_Num As String 'Set the lookup number field
        Dim PCUprocessed As Integer 'Stores how have rules have been processed
        Dim dbtype = "sql" 'gs_db_type

        ' Toolbox Parameters
        Dim myParams As New List(Of String)
        myParams.Add(strProjectPath) ' project path
        myParams.Add(MU) ' mu
        'myParams.Add(dbtype)

        Dim tool As String = "Rules_Setup"
        Dim thetool As String = Path.Combine(gs_toolboxpath, tool)

        Dim pixel_result = Await gt_PixelPYT(thetool, myParams, MU)


        ' Build the SQLite connection string
        'Dim dbPath As String = Path.Combine(strProjectPath, "LFTFC_new.sqlite")
        Dim dbPath As String = Path.Combine(gs_ProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

        Using dbConn As New SQLiteConnection(connString)
            dbConn.Open()

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
                Text = "Fuel Rules for MU " & MU

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
                         "ORDER BY Max(LUT_DistCode.ID)"
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

            Catch ex As Exception

                MsgBox("Error in frmRule New - " & ex.Message)
            End Try
        End Using
    End Function

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

                Thread.Sleep(1000) 'Let the copy query catchup

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

                    Thread.Sleep(1000) 'Let the copy query catchup

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

            Try
                If lstVwRulesets.SelectedItems.Count = 0 Then
                    MsgBox("You must first select a rule to delete.")
                ElseIf MsgBox("Do you really want to delete this rule." & vbCrLf &
                    "If you turn the rule off you can keep it for notes" & vbCrLf &
                    "and it won't be included in your analysis.", vbYesNo, "Delete Rule?") = vbYes Then

                    ' Build the SQLite connection string
                    Dim dbPath As String = Path.Combine(gs_ProjectPath, gs_LFTFCSQliteName)
                    Dim connString As String = "Data Source=" & dbPath & ";Version=3;"

                    'Do all database work here and close the connection before recalculating.
                    'SQLite locks the file, so holding this connection open while gr_ClearPAP
                    'and gr_MakeRuleset open their own connections makes their writes fail.
                    'Access/Jet tolerated the overlapping connections, SQLite does not.
                    Using dbconn As New SQLiteConnection(connString)
                        dbconn.Open()

                        'Remove rule from database
                        Dim i As Integer 'Use to count
                        i = 0
                        Do Until i = lstVwRulesets.SelectedItems.Count
                            strSQL = "DELETE FROM " & rulesR & " " &
                                    "WHERE Id = " & RulesetCollection.Item(lstVwRulesets.SelectedItems(i).Index + 1).Id
                            RunNonQuery(dbconn, strSQL)

                            i = i + 1
                        Loop

                        'Blank the pixel count on the remaining rules for this EVT/DIST. That empty
                        'value is the sentinel gr_MakeRuleset looks for to decide it must recalculate.
                        'Done in one statement rather than relying on the clsRule.PixelCount setter,
                        'which opens a connection per rule and swallows any failure.
                        strSQL = "UPDATE " & rulesR & " SET PixelCount = '' " &
                                "WHERE EVT = " & gf_GetNum(cmbEVT.Text, "EVT") & " " &
                                "AND DIST = " & gf_GetNum(cmbEVT.Text, "DIST")
                        RunNonQuery(dbconn, strSQL)
                    End Using

                    'Remove rule from Ruleset collection
                    gr_ClearPAP(RulesetCollection) 'Clears the pixel count, acres, and percent evt of the ruleset
                    gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                                  RulesetCollection, EVTPixelCountCollection, strProjectPath)
                    DisplayRuleset()
                    AdjPer()
                End If

            Catch ex As Exception
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

    Private Sub TabControl_MouseClick(sender As Object, e As MouseEventArgs) Handles TabControl.MouseClick

        grpCanopyLines.Visible = False

        If IsEVTSelected() Then

            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                Try
                    Dim EOFFlag As Boolean

                    strSQL =
                    "SELECT EVTR, DIST " &
                    "FROM " & comboR & " " &
                    "WHERE EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") &
                    " AND DIST = " & gf_GetNum(cmbEVT.Text, "DIST")

                    Using cmd As New SQLiteCommand(strSQL, conn)
                        Using reader As SQLiteDataReader = cmd.ExecuteReader()

                            EOFFlag = Not reader.HasRows

                            If EOFFlag = False Then

                                If TabControl.SelectedIndex = 0 Then
                                    gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"),
                                               gf_GetNum(cmbEVT.Text, "DIST"),
                                               comboR, rulesR,
                                               RulesetCollection, EVTPixelCountCollection,
                                               strProjectPath)
                                    DisplayRuleset()
                                    AdjPer()
                                    cmdAutoRule.Enabled = False
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

                                    gf_PopBPS(cmbBPSGraph,
                                          gf_GetNum(cmbEVT.Text, "EVT"),
                                          gf_GetNum(cmbEVT.Text, "DIST"),
                                          comboR, strProjectPath)

                                    gf_PopWild(cmbWildGraph,
                                           gf_GetNum(cmbEVT.Text, "EVT"),
                                           gf_GetNum(cmbEVT.Text, "DIST"),
                                           gf_GetNum(cmbBPSGraph.Text, "General"),
                                           comboR, strProjectPath)

                                    grpCanopyLines.Visible = True
                                    DistGraph()

                                ElseIf TabControl.SelectedIndex = 3 Then
                                    cmdAutoRule.Enabled = False
                                    cmdAddRule.Enabled = False
                                    cmdCopyRule.Enabled = False
                                    cmdDeleteRule.Enabled = False
                                    cmdEditRule.Enabled = False
                                    GetEVTDescription()
                                End If

                            Else
                                lstVwRulesets.Items.Clear()
                                chrtDist.Visible = False
                            End If

                        End Using
                    End Using

                    If TabControl.SelectedIndex = 1 Then
                        cmdAutoRule.Enabled = False
                        cmdAddRule.Enabled = False
                        cmdCopyRule.Enabled = False
                        cmdDeleteRule.Enabled = False
                        cmdEditRule.Enabled = False
                        InitCompareFM()
                        GraphCompareFM()
                    End If

                Catch ex As Exception
                    MsgBox("Error in TabControl_MouseClick - " & ex.Message)
                End Try

            End Using ' auto cleans connection

        End If

    End Sub

    Private Sub SetTabs()

        grpCanopyLines.Visible = False

        Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
            conn.Open()

            Try
                If IsEVTSelected() Then

                    ' Check EVT/DIST exists in comboR table
                    Dim sqlEVT As String =
                    "SELECT EVTR, DIST FROM " & comboR &
                    " WHERE EVTR = @evt AND DIST = @dist"

                    Dim evtExists As Boolean

                    Using cmd As New SQLiteCommand(sqlEVT, conn)
                        cmd.Parameters.AddWithValue("@evt", gf_GetNum(cmbEVT.Text, "EVT"))
                        cmd.Parameters.AddWithValue("@dist", gf_GetNum(cmbEVT.Text, "DIST"))

                        Using reader As SQLiteDataReader = cmd.ExecuteReader()
                            evtExists = reader.HasRows
                        End Using
                    End Using

                    If evtExists Then

                        lblDistCode.Text = "Not disturbed"

                        ' If DIST > 0, lookup disturbance description
                        If gf_GetNum(cmbEVT.Text, "DIST") > 0 Then

                            Dim sqlDist As String =
                            "SELECT Description FROM LUT_DistCode WHERE DistCode = @dist"

                            Using cmd2 As New SQLiteCommand(sqlDist, conn)
                                cmd2.Parameters.AddWithValue("@dist", gf_GetNum(cmbEVT.Text, "DIST"))

                                Using reader2 As SQLiteDataReader = cmd2.ExecuteReader()
                                    If reader2.Read() Then
                                        lblDistCode.Text =
                                        gf_GetNum(cmbEVT.Text, "DIST") & " " &
                                        reader2("Description").ToString()
                                    End If
                                End Using
                            End Using
                        End If


                        ' === Active Tab Actions ===
                        Select Case TabControl.SelectedIndex

                            Case 0
                                gr_MakeRuleset(
                                gf_GetNum(cmbEVT.SelectedItem, "EVT"),
                                gf_GetNum(cmbEVT.Text, "DIST"),
                                comboR,
                                rulesR,
                                RulesetCollection,
                                EVTPixelCountCollection,
                                strProjectPath)
                                DisplayRuleset()
                                AdjPer()

                            Case 2
                                gf_PopBPS(cmbBPSGraph,
                                      gf_GetNum(cmbEVT.Text, "EVT"),
                                      gf_GetNum(cmbEVT.Text, "DIST"),
                                      comboR,
                                      strProjectPath)

                                gf_PopWild(cmbWildGraph,
                                       gf_GetNum(cmbEVT.SelectedItem, "EVT"),
                                       gf_GetNum(cmbEVT.SelectedItem, "DIST"),
                                       gf_GetNum(cmbBPSGraph.SelectedItem, "General"),
                                       comboR,
                                       strProjectPath)

                                grpCanopyLines.Visible = True
                                rdoNoneDistGraph.Checked = True
                                DistGraph()

                            Case 3
                                GetEVTDescription()

                        End Select

                    Else
                        lstVwRulesets.Items.Clear()
                        chrtDist.Visible = False
                    End If
                End If

            Catch ex As Exception
                MsgBox("Error in SetTabs - " & ex.Message)
            End Try

        End Using ' connection auto-closes

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
            lblPixelsLeftOver.BackColor = Drawing.Color.Red
        Else
            'Calcs the total pixels left over not assigned to rules
            If (lngTotPixelsPerEVT - (lngPixelsPerRulesetAll + lngPixelsPerRulesetB _
                                      + lngPixelsPerRulesetW + lngPixelsPerRulesetBW) = 0) Then
                With lblPixelsLeftOver
                    .BackColor = Drawing.Color.PaleGreen
                    .Text = "No pixels are left behind."
                End With
            Else
                lblPixelsLeftOver.BackColor = Drawing.Color.Red
                lblPixelsLeftOver.Text = "Pixels left behind: " & lngTotPixelsPerEVT - (lngPixelsPerRulesetAll _
                                        + lngPixelsPerRulesetB + lngPixelsPerRulesetW + lngPixelsPerRulesetBW)
            End If
        End If
    End Sub

    Public Sub GetEVTDescription()

        txtEVTDescription.Text = "See Landfire.gov website for updated vegetation descriptions."

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

        Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
            conn.Open()

            Try
                'Populate the fuelmodel collection
                strSQL = "SELECT FMNum, FMCode, FL1H, FL10H, FL100H, FLLiveH, FLLiveW, FMType, H1SAV, LiveHSAV, LiveWSAV, " &
                    "Depth, XtMoist, DHt, LHt, FMName, DataType, Creator " &
                    "FROM LUT_FuelModelParameters"

                Using cmd As New SQLiteCommand(strSQL, conn)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()


                        While reader.Read()
                            fmNewFM = New clsFM(
                                reader("DataType").ToString(),
                                CInt(reader("FMNum")),
                                reader("FMCode").ToString(),
                                CDbl(reader("FL1H")),
                                CDbl(reader("FL10H")),
                                CDbl(reader("FL100H")),
                                CDbl(reader("FLLiveH")),
                                CDbl(reader("FLLiveW")),
                                reader("FMType").ToString(),
                                CDbl(reader("H1SAV")),
                                CDbl(reader("LiveHSAV")),
                                CDbl(reader("LiveWSAV")),
                                CDbl(reader("Depth")),
                                CDbl(reader("XtMoist")),
                                CDbl(reader("DHt")),
                                CDbl(reader("LHt")),
                                reader("FMName").ToString(),
                                reader("Creator").ToString()
                            )

                            colFM.Add(fmNewFM, "FM" & reader("FMNum").ToString())
                        End While


                    End Using
                End Using

            Catch ex As Exception

                MsgBox("Error in gr_MakeRuleset - " & ex.Message)

            End Try
        End Using


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


        Dim values As Integer() = Nothing

        If rdoDM1.Checked Then values = {3, 4, 5}
        If rdoDM2.Checked Then values = {6, 7, 8}
        If rdoDM3.Checked Then values = {9, 10, 11}
        If rdoDM4.Checked Then values = {12, 13, 14}

        If values IsNot Nothing Then
            intFM01 = values(0)
            intFM10 = values(1)
            intFM100 = values(2)
        End If

        Dim values2 As Integer() = Nothing

        If rdoLM1.Checked Then values2 = {30, 60}
        If rdoLM2.Checked Then values2 = {60, 90}
        If rdoLM3.Checked Then values2 = {90, 120}
        If rdoLM4.Checked Then values2 = {120, 150}

        If values2 IsNot Nothing Then
            intLH = values2(0)
            intLW = values2(1)
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
            .AxisY.MajorGrid.LineColor = Drawing.Color.Blue
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

    ' Emulates Access/VBA banker’s rounding exactly (Round(x,0)).
    ' SQLite ROUND(x) does not match Access in half-even cases.
    Public Function AccessRound(value As Double) As Double
        ' Banker's rounding: halves go to nearest even number
        Dim floorVal As Double = Math.Floor(value)
        Dim diff As Double = value - floorVal

        If diff = 0.5 Then
            ' exactly .5 → round to even
            If floorVal Mod 2 = 0 Then
                Return floorVal
            Else
                Return floorVal + 1
            End If
        Else
            ' normal rounding
            Return Math.Round(value)
        End If
    End Function

    Private Sub DistGraph()
        Dim rnd As New Random                                           'Used to set a random color
        Dim cmd1 As SQLiteCommand                                       'command for rs1 data
        Dim cmd2 As SQLiteCommand                                       'command for rs2 data
        Dim rs1 As SQLiteDataReader                                     'reader for data
        Dim rs2 As SQLiteDataReader                                     'reader for data

        Dim err As String = ""                                          'Used to store any errors that occur   

        Dim dbconn As New SQLiteConnection("Data Source=" &
        strProjectPath & "\" & gs_LFTFCSQliteName & ";")                    'DB connection
        dbconn.Open()


        Dim CanopySel As String             'Flag for CBH or CBD to be displayed in graph
        Dim Series1Index As Integer         'Stores the starting series index for the cbh and cbd series
        Dim SeriesEqual As Boolean = True   'Stores if series is equal or not

        Dim retryCount As Integer = 0   'Count of number of times loop has been rerun for same values due to an error.

        Try
            If rdoNoneDistGraph.Checked Then
                CanopySel = "None"                                          'Neither selected, do not show line graph
            ElseIf rdoCCDistGraph.Checked Then
                chrtDist.ChartAreas(0).AxisY2.Title = "% CC in Dist code -  " &
                    gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST")              'Set Y2 Axis title
                CanopySel = "Cover"                                         'Cover selected, show CC line graph
            ElseIf rdoCHDistGraph.Checked Then
                chrtDist.ChartAreas(0).AxisY2.Title = "CH(m) in Dist code -  " &
                    gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST")              'Set Y2 Axis title
                CanopySel = "Height"                                        'Height selected, show CH line graph
            ElseIf rdoCBHDistGraph.Checked Then
                chrtDist.ChartAreas(0).AxisY2.Title = "CBH(m) in Dist code -  " &
                    gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST")              'Set Y2 Axis title
                CanopySel = "CBH"                                           'CBH selected, show CBH line graph
            Else
                CanopySel = "CBD"                                           'CBD selected, show CBD line graph
                chrtDist.ChartAreas(0).AxisY2.Title = "CBD(kg/m^3) in Dist code -  " &
                    gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST")              'Set Y2 Axis title
            End If

            Dim itemCounter As Integer = 1
            Dim treeCounter As Integer = 0

            Dim covArray() As Object = {"%", "15", "25", "35", "45", "55", "65", "75", "85", "95"}

            'Get the count of the number of tree lifeforms
            strSQL = "SELECT LUT_Height.Lifeform, Count(LUT_Height.Lifeform) AS CountOfLifeform " &
                     "FROM LUT_Height " &
                     "GROUP BY LUT_Height.Lifeform " &
                     "HAVING (((LUT_Height.Lifeform)='Tree'))"
            cmd1 = New SQLiteCommand(strSQL, dbconn)
            rs1 = cmd1.ExecuteReader()

            If rs1.Read() Then
                treeCounter = CInt(rs1("CountOfLifeform"))
            End If
            rs1.Close()
            cmd1.Dispose()

            strSQL = "SELECT LUT_Height.Lifeform, LUT_Height.LowName, LUT_Height.HighName, LUT_Height.EVH " &
                     "FROM LUT_Height " &
                     "WHERE(((LUT_Height.EVH) > 100)) " &
                     "ORDER BY LUT_Height.EVH"

            'SQLiteDataReader has no RecordCount, get the row count first for array sizing
            cmd1 = New SQLiteCommand("SELECT Count(*) FROM LUT_Height WHERE LUT_Height.EVH > 100", dbconn)
            Dim evhRowCount As Integer = CInt(cmd1.ExecuteScalar())
            cmd1.Dispose()

            cmd1 = New SQLiteCommand(strSQL, dbconn)
            rs1 = cmd1.ExecuteReader()

            itemCounter = 1

            Dim hgtArray(evhRowCount + treeCounter) As Object

            Do While rs1.Read()
                If rs1("Lifeform").ToString = "Tree" Then
                    'Canopy
                    hgtArray(itemCounter) = Mid(rs1("LowName").ToString, 1, Len(rs1("LowName")) - 1) &
                                                        Trim(Mid(rs1("HighName").ToString, 1, Len(rs1("HighName")) - 5))
                    hgtArray(itemCounter + treeCounter) = Mid(rs1("LowName").ToString, 1, Len(rs1("LowName")) - 1) &
                                                        Trim(Mid(rs1("HighName").ToString, 1, Len(rs1("HighName")) - 5))
                Else
                    'No Canopy
                    hgtArray(itemCounter) = Mid(rs1("LowName").ToString, 1, Len(rs1("LowName")) - 1) &
                                                        Trim(Mid(rs1("HighName").ToString, 1, Len(rs1("HighName")) - 5))
                End If
                itemCounter += 1
            Loop

            rs1.Close()
            cmd1.Dispose()
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
            cmd2 = New SQLiteCommand(strSQL, dbconn)
            rs2 = cmd2.ExecuteReader()
            Dim rs2HasRow As Boolean = rs2.Read()   'Position on first row (reader has no non-advancing "current row")

            Do While hA <= hgtArray.Length - treeCounter - 1 'All the height + additional for canopy
                Dim cA As Short
                'Set seriesArray to 0s
                For cA = 1 To CShort(covArray.Length - 1) 'cA Counts the position in the series array used further down aswell
                    seriesArray(cA) = "0"
                Next

                evhCode = CInt(rs2("EVH")) 'Set next evh code

                If IsNumeric(gf_GetNum(cmbBPSGraph.SelectedItem.ToString, "General")) Then 'BPS is numeric not 'any'
                    strSQL = "SELECT LUT_Height.Lifeform, " & comboR & ".EVHR, " & comboR & ".EVCR, " &
                    "Sum(CASE WHEN ((" & comboR & ".EVTR=" & gf_GetNum(cmbEVT.SelectedItem.ToString, "EVT") & ") " &
                    "And (" & comboR & ".DIST=" & gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST") & ") " &
                    "And (" & comboR & ".BPSRF = " & gf_GetNum(cmbBPSGraph.SelectedItem.ToString, "General") & ") " &
                    "And (" & comboR & ".WILDCARD = '" & cmbWildGraph.SelectedItem.ToString & "')) " &
                    "Or ((" & comboR & ".EVTR=" & gf_GetNum(cmbEVT.SelectedItem.ToString, "EVT") & ") " &
                    "And (" & comboR & ".DIST=" & gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST") & ") " &
                    "And (" & comboR & ".BPSRF = " & gf_GetNum(cmbBPSGraph.SelectedItem.ToString, "General") & ") " &
                    "And ('" & cmbWildGraph.SelectedItem.ToString & "' = 'any')) " &
                    "THEN ROUND(" & comboR & ".""COUNT"" * 0.2223948) ELSE 0 END) AS SUMCOUNT " &
                    "FROM (LUT_Cover INNER JOIN " & comboR & " ON LUT_Cover.EVC = " & comboR & ".EVCR) " &
                    "INNER JOIN LUT_Height ON " & comboR & ".EVHR = LUT_Height.EVH " &
                    "GROUP BY LUT_Height.Lifeform, " & comboR & ".EVHR, " & comboR & ".EVCR " &
                    "HAVING(((" & comboR & ".EVHR) = " & evhCode & ") And ((" & comboR & ".EVCR) > 100)) " &
                    "ORDER BY " & comboR & ".EVCR"
                Else 'BPS is text it is 'any'
                    strSQL = "SELECT LUT_Height.Lifeform, " & comboR & ".EVHR, " & comboR & ".EVCR, " &
                    "Sum(CASE WHEN ((" & comboR & ".EVTR=" & gf_GetNum(cmbEVT.SelectedItem.ToString, "EVT") & ") " &
                    "And (" & comboR & ".DIST=" & gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST") & ") " &
                    "And (" & comboR & ".WILDCARD = '" & cmbWildGraph.SelectedItem.ToString & "')) " &
                    "Or ((" & comboR & ".EVTR=" & gf_GetNum(cmbEVT.SelectedItem.ToString, "EVT") & ") " &
                    "And (" & comboR & ".DIST=" & gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST") & ") " &
                    "And ('" & cmbWildGraph.SelectedItem.ToString & "' = 'any')) " &
                    "THEN ROUND(" & comboR & ".""COUNT"" * 0.2223948) ELSE 0 END) AS SUMCOUNT " &
                    "FROM (LUT_Cover INNER JOIN " & comboR & " ON LUT_Cover.EVC = " & comboR & ".EVCR) " &
                    "INNER JOIN LUT_Height ON " & comboR & ".EVHR = LUT_Height.EVH " &
                    "GROUP BY LUT_Height.Lifeform, " & comboR & ".EVHR, " & comboR & ".EVCR " &
                    "HAVING(((" & comboR & ".EVHR) = " & evhCode & ") And ((" & comboR & ".EVCR) > 100)) " &
                    "ORDER BY " & comboR & ".EVCR"
                End If

                cmd1 = New SQLiteCommand(strSQL, dbconn)
                rs1 = cmd1.ExecuteReader()

                'Run three times per query
                Dim acreCheck As Boolean = False    'True if acres are present False if not
                cA = 1                              'Keeps track of what number is added to the series
                Do While rs1.Read()
                    seriesArray(cA) = AccessRound(CDbl(rs1("SUMCOUNT")))
                    If CInt(seriesArray(cA)) > 0 Then acreCheck = True
                    cA += CShort(1)
                Loop
                rs1.Close()
                cmd1.Dispose()

                'Add a series to the chart with the x-values and y-values
                'from the arrays and set the series type to a column chart
                'Create a Series
                If acreCheck = True Then
                    Dim oSeries As New Series
                    If rdoNoneDistGraph.Checked <> False Then
                        With oSeries
                            .YAxisType = AxisType.Primary
                            .Name = hgtArray(hA).ToString
                            'Set Data
                            .Points.DataBindXY(covArray, seriesArray)
                        End With

                        'Add series to chart
                        chrtDist.Series.Add(oSeries)

                        With chrtDist.Series(CStr(hgtArray(hA)))
                            'Set series chart type
                            '.ChartType = SeriesChartType.Bar
                            .ChartType = SeriesChartType.StackedColumn
                            '.ChartType = SeriesChartType.Line
                            '.ChartType = SeriesChartType.Spline
                            .Color = Drawing.Color.FromArgb(rnd.Next(50, 200), rnd.Next(50, 200), rnd.Next(50, 200))   'Assign a color.
                        End With

                        'Get canopy values for the series
                    ElseIf rs2("Lifeform").ToString = "Tree" And rdoNoneDistGraph.Checked <> True Then
                        Dim distCC As Double                  'Stores the disturbed Canopy Cover
                        Dim distCH As Double                  'Stores the disturbed Canopy Height

                        cA = 1
                        evhMidPoint = CInt(gf_GetHeightMid(evhCode, strProjectPath) * 10)
                        strSQL = "SELECT LUT_Cover.Lifeform, LUT_Cover.EVC " &
                        "FROM LUT_Cover " &
                        "WHERE(((LUT_Cover.Lifeform) = 'Tree')) " &
                        "ORDER BY LUT_Cover.EVC"
                        cmd1 = New SQLiteCommand(strSQL, dbconn)
                        rs1 = cmd1.ExecuteReader()
                        Dim rs1HasRow As Boolean = rs1.Read()   'Position on first row

                        Do While rs1HasRow
                            Application.DoEvents() 'Allow form to update while loop is running
                            Try
                                'Get disturbed CC and CH if disturbed
                                If CInt(gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST")) > 0 Then
                                    distCC = Canopy_LM_EQs(CShort((CInt(rs1("EVC")) - 100) * 10 + 5),
                                                evhMidPoint / 10, CShort(gf_GetNum(cmbEVT.SelectedItem.ToString, "EVT")),
                                                CShort(gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST")), "Cover")
                                    distCH = Canopy_LM_EQs(CShort((CInt(rs1("EVC")) - 100) * 10 + 5),
                                                           evhMidPoint / 10, CShort(gf_GetNum(cmbEVT.SelectedItem.ToString, "EVT")),
                                                           CShort(gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST")), "Height")
                                    If distCC < 10 Or distCH <= 1.8 Then
                                        distCC = 0
                                        distCH = 0
                                    End If
                                Else
                                    'Get non disturbed CC and CH
                                    distCC = (CDbl(rs1("EVC")) - 100) * 10 + 5
                                    distCH = evhMidPoint / 10
                                End If

                                If distCC < 10 Or distCH <= 1.8 Then     'Check for 0 canopy
                                    seriesArray(cA) = 0
                                ElseIf rdoCCDistGraph.Checked Then      'Get CCs
                                    seriesArray(cA) = distCC
                                ElseIf rdoCHDistGraph.Checked Then      'Get CHs
                                    seriesArray(cA) = distCH
                                ElseIf rdoCBHDistGraph.Checked Then     'Get CBHs
                                    seriesArray(cA) = 0
                                    seriesArray(cA) = Canopy_LM_EQs(distCC, distCH, CShort(gf_GetNum(cmbEVT.SelectedItem.ToString, "EVT")),
                                                                    CShort(gf_GetNum(cmbEVT.SelectedItem.ToString, "DIST")), "CBH")
                                ElseIf rdoCBDDistGraph.Checked Then     'Get CBDs
                                    seriesArray(cA) = 0
                                    seriesArray(cA) = CalcCBDGLM(distCC, distCH) / 100
                                Else                                    'Skip this record
                                    'Do Nothing
                                End If

                                cA += CShort(1)
                                If rs1HasRow Then rs1HasRow = rs1.Read()
                                retryCount = 0 'Reset retry count for next record
                            Catch ex As Exception
                                If retryCount > 5000 Then
                                    seriesArray(cA) = 0 'Set value to 0 if there is an error to avoid infinite loop
                                    cA += CShort(1)
                                    If rs1HasRow Then rs1HasRow = rs1.Read()
                                Else
                                    retryCount += 1
                                End If
                                'Do nothing sometimes there is an unexpected error. Rerun the loop with the same values
                            End Try
                        Loop
                        'Add a series to the chart with the x-values and y-values
                        'from the arrays and set the series type to a column chart
                        'Create a Series
                        oSeries = New Series
                        With oSeries
                            .YAxisType = AxisType.Secondary
                            .Name = hgtArray(hA + treeCounter).ToString
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
                        With chrtDist.Series(CStr(hgtArray(hA + treeCounter)))
                            'Set series chart type
                            .ChartType = SeriesChartType.Line
                            '.ChartType = SeriesChartType.Candlestick
                            .Color = Drawing.Color.FromArgb(rnd.Next(50, 200), rnd.Next(50, 200), rnd.Next(50, 200))   'Assign a color.
                        End With
                        Series1Index = chrtDist.Series.Count - 1                                        'Get starting count before adding CBH and CBD series
                        rs1.Close()
                        cmd1.Dispose()
                    End If
                End If
                hA += 1

                If rs2HasRow Then rs2HasRow = rs2.Read()
                'Debug.Print("hA = " & hA)
            Loop
            rs2.Close()
            cmd2.Dispose()
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

            If rs1 IsNot Nothing AndAlso Not rs1.IsClosed Then rs1.Close()
            rs1 = Nothing
            If cmd1 IsNot Nothing Then cmd1.Dispose()

            If rs2 IsNot Nothing AndAlso Not rs2.IsClosed Then rs2.Close()
            rs2 = Nothing
            If cmd2 IsNot Nothing Then cmd2.Dispose()

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
        Catch ex As Exception
            If rs1 IsNot Nothing AndAlso Not rs1.IsClosed Then rs1.Close()
            rs1 = Nothing
            If cmd1 IsNot Nothing Then cmd1.Dispose()

            If rs2 IsNot Nothing AndAlso Not rs2.IsClosed Then rs2.Close()
            rs2 = Nothing
            If cmd2 IsNot Nothing Then cmd2.Dispose()

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing
            MsgBox("DistGraph " & ex.Message)
        End Try
    End Sub

    Private Function AdjustCBH(value As Double, numCH As Double) As Double
        Dim rounded = Math.Round(value, 1)

        If rounded < 0.3 Then Return 0.3
        If rounded >= 10 Then Return 10
        If rounded >= numCH Then Return numCH * 0.6666
        Return rounded
    End Function

    Private Function AdjustCover(value As Double, numCC As Double, numDist As Short) As Double
        If numDist = 0 Then Return numCC

        Dim rounded = Math.Round(value, 0)
        If rounded < 10 Then Return 0
        If rounded > 95 Then Return 95
        Return rounded
    End Function

    Private Function AdjustHeight(value As Double, numCH As Double, numDist As Short) As Double
        If numDist = 0 Then Return numCH

        Dim rounded = Math.Round(value, 1)
        If rounded < 1.3 Then Return 0
        If rounded >= 50 Then Return 50
        Return rounded
    End Function

    Private Function Canopy_LM_EQs(ByVal numCC As Double, ByVal numCH As Double, ByVal numEVT As Short,
                                   ByVal numDist As Short, ByVal canopyType As String) As Double

        Dim connString As String = "Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName
        Dim result As Double = 0

        Try
            Dim intercept As Double = 0
            Dim htCoef As Double = 0
            Dim ccCoef As Double = 0

            Using conn As New SQLite.SQLiteConnection(connString)
                conn.Open()

                Using cmd As New SQLite.SQLiteCommand("
                SELECT intercept, HT_coef, CC_coef
                FROM Master_Disturbance_Tbl
                WHERE Tree_EVTs = @evt
                  AND HDist = @dist
                  AND EV_Structure = @struct;", conn)

                    cmd.Parameters.AddWithValue("@evt", numEVT)
                    cmd.Parameters.AddWithValue("@dist", numDist)
                    cmd.Parameters.AddWithValue("@struct", canopyType)

                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            intercept = CDbl(reader("intercept"))
                            htCoef = CDbl(reader("HT_coef"))
                            ccCoef = CDbl(reader("CC_coef"))
                        Else
                            Return 0   ' No record; original function implicitly returned 0 on missing data
                        End If
                    End Using

                End Using
            End Using

            ' Base computed equation
            result = intercept + (htCoef * numCH) + (ccCoef * numCC)

            ' Apply canopy-type specific adjustments
            Select Case canopyType
                Case "CBH"
                    result = AdjustCBH(result, numCH)

                Case "Cover"
                    result = AdjustCover(result, numCC, numDist)

                Case "Height"
                    result = AdjustHeight(result, numCH, numDist)

            End Select

            Return result

        Catch ex As Exception
            MsgBox("Error in Canopy_LM_EQs - " & ex.Message)
            Return 0
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

    Private Function AlreadySelected(ByVal cmbFM As System.Windows.Forms.ComboBox) As Boolean
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

    Private Sub cmbDefaultFM_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDefaultFM.SelectedIndexChanged

        Try
            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                Dim strFMType As String = ""

                '-----------------------------------------
                ' Retrieve starting FM parameters
                '-----------------------------------------
                Dim sql As String =
                "SELECT FMNum, FMCode, FL1H, FL10H, FL100H, FLLiveH, FLLiveW, FMType, " &
                "H1SAV, LiveHSAV, LiveWSAV, Depth, XtMoist, DHt, LHt, FMName " &
                "FROM LUT_FuelModelParameters " &
                "WHERE FMNum = @fmnum"

                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@fmnum", CInt(cmbDefaultFM.Text))

                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        If reader.Read() Then

                            '-----------------------------------------
                            ' Assign values
                            '-----------------------------------------
                            rdo1H.Text = Math.Round(CDbl(reader("FL1H")), 2)
                            rdo10H.Text = Math.Round(CDbl(reader("FL10H")), 2)
                            rdo100H.Text = Math.Round(CDbl(reader("FL100H")), 2)
                            rdoLiveH.Text = Math.Round(CDbl(reader("FLLiveH")), 2)
                            rdoLiveW.Text = Math.Round(CDbl(reader("FLLiveW")), 2)

                            rdo1HSAV.Text = reader("H1SAV").ToString()
                            rdoLiveHSAV.Text = reader("LiveHSAV").ToString()
                            rdoLiveWSAV.Text = reader("LiveWSAV").ToString()

                            rdoDepth.Text = Math.Round(CDbl(reader("Depth")), 2)
                            rdoXtMoist.Text = reader("XtMoist").ToString()

                            strFMType = reader("FMType").ToString()

                        Else
                            ' If record not found: do nothing
                            Exit Sub
                        End If
                    End Using
                End Using

                '-----------------------------------------
                ' FMType → checkbox flag
                '-----------------------------------------
                If strFMType = "dynamic" Then
                    chkFMType.Checked = True
                Else
                    chkFMType.Checked = False
                End If

                '-----------------------------------------
                ' UI state exactly as original
                '-----------------------------------------
                rdo1H.Checked = True

                With TrkBar
                    .Minimum = 0
                    .Maximum = 701
                    .Value = CInt(CDbl(rdo1H.Text) * 100)
                    .TickFrequency = 35
                End With

                ' Fire graph if correct tab is selected
                If TabControl.SelectedIndex = 1 Then
                    GraphCompareFM()
                End If

            End Using

        Catch ex As Exception
            MsgBox("Error in cmbDefaultFM_SelectedIndexChanged - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdSaveCSTFM_Click(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                Dim strNewFMNum As String = ""
                Dim strNewFMName As String = ""
                Dim strNotAvail As String = ""
                Dim blnGood As Boolean = False
                Dim strError As String = ""

                '----------------------------------------------------------
                ' Load all existing FMNum values
                '----------------------------------------------------------
                Dim existingNums As New List(Of Integer)

                Dim sqlLoad As String = "SELECT FMNum FROM LUT_FuelModelParameters ORDER BY FMNum"
                Using cmd As New SQLiteCommand(sqlLoad, conn)
                    Using rd As SQLiteDataReader = cmd.ExecuteReader()
                        While rd.Read()
                            existingNums.Add(CInt(rd("FMNum")))
                        End While
                    End Using
                End Using

                ' Build "not available" message
                For Each n In existingNums
                    strNotAvail &= ", " & n.ToString()
                Next

                '----------------------------------------------------------
                ' Loop until user provides a valid FM number
                '----------------------------------------------------------
                Do While blnGood = False

                    ' Default suggestion from last FMNum
                    Dim suggestion As String = (existingNums.LastOrDefault() + 1).ToString()

                    strNewFMNum = InputBox(
                    "Input a new 3 digit or less Custom Fuel Model number. " &
                    "Do not use Anderson 13 or Scott and Burgan existing Fuel Model Numbers. " &
                    "These numbers are already in use: " & strNotAvail,
                    "New Custom FM Number",
                    suggestion)

                    If strNewFMNum = "" Then Exit Sub      ' cancel pressed

                    blnGood = True     ' prove false

                    If Not IsNumeric(strNewFMNum) Then
                        blnGood = False
                        strError = "Error: " & strNewFMNum & " is not a number."
                    ElseIf CInt(strNewFMNum) < 0 Or CInt(strNewFMNum) > 999 Then
                        blnGood = False
                        strError = "Error: " & strNewFMNum & " is either < 0 OR > 999."
                    Else
                        ' conflict with existing FMNum?
                        For Each n In existingNums
                            If CInt(strNewFMNum) = n Then
                                blnGood = False
                                strError = "Error: " & strNewFMNum & " = an existing Fuel Model Number."
                                Exit For
                            End If
                        Next
                    End If

                    If blnGood = False Then MsgBox(strError, , "Bad Fuel Model Number")

                Loop

                '----------------------------------------------------------
                ' Loop until user provides a valid FM Name
                '----------------------------------------------------------
                blnGood = False
                Do While blnGood = False

                    strNewFMName = InputBox(
                    "Input a new fuel model name less than 255 characters " &
                    "long that describes the custom fuel model.",
                    "New Custom FM Code",
                    "ABC")

                    If strNewFMName = "" Then Exit Sub    ' cancel pressed

                    blnGood = True

                    If strNewFMName.Length = 0 Then
                        blnGood = False
                        strError = "Enter a Name 255 characters or less."
                    ElseIf strNewFMName.Length > 255 Then
                        blnGood = False
                        strError = strNewFMName & " is longer than 255 characters."
                    End If

                    If blnGood = False Then MsgBox(strError, , "Bad Fuel Model Number")

                Loop

                '----------------------------------------------------------
                ' INSERT new Fuel Model into LUT_FuelModelParameters
                '----------------------------------------------------------
                Dim sqlInsert As String =
                "INSERT INTO LUT_FuelModelParameters (" &
                "FMNum, FMCode, FL1H, FL10H, FL100H, FLLiveH, FLLiveW, FMType, " &
                "H1SAV, LiveHSAV, LiveWSAV, Depth, XtMoist, DHt, LHt, FMName, " &
                "DataType, Creator)" &
                "VALUES (" &
                "@FMNum, @FMCode, @FL1H, @FL10H, @FL100H, @FLLiveH, @FLLiveW, @FMType, " &
                "@H1SAV, @LiveHSAV, @LiveWSAV, @Depth, @XtMoist, @DHt, @LHt, @FMName, " &
                "@DataType, @Creator)"

                Using cmd As New SQLiteCommand(sqlInsert, conn)

                    cmd.Parameters.AddWithValue("@FMNum", CInt(strNewFMNum))
                    cmd.Parameters.AddWithValue("@FMCode", "CST")

                    cmd.Parameters.AddWithValue("@FL1H", CDbl(rdo1H.Text))
                    cmd.Parameters.AddWithValue("@FL10H", CDbl(rdo10H.Text))
                    cmd.Parameters.AddWithValue("@FL100H", CDbl(rdo100H.Text))
                    cmd.Parameters.AddWithValue("@FLLiveH", CDbl(rdoLiveH.Text))
                    cmd.Parameters.AddWithValue("@FLLiveW", CDbl(rdoLiveW.Text))

                    cmd.Parameters.AddWithValue("@FMType", chkFMType.Text)

                    cmd.Parameters.AddWithValue("@H1SAV", CInt(rdo1HSAV.Text))
                    cmd.Parameters.AddWithValue("@LiveHSAV", CInt(rdoLiveHSAV.Text))
                    cmd.Parameters.AddWithValue("@LiveWSAV", CInt(rdoLiveWSAV.Text))

                    cmd.Parameters.AddWithValue("@Depth", CDbl(rdoDepth.Text))
                    cmd.Parameters.AddWithValue("@XtMoist", CInt(rdoXtMoist.Text))

                    cmd.Parameters.AddWithValue("@DHt", 8000)
                    cmd.Parameters.AddWithValue("@LHt", 8000)

                    cmd.Parameters.AddWithValue("@FMName", strNewFMName)
                    cmd.Parameters.AddWithValue("@DataType", "English")
                    cmd.Parameters.AddWithValue("@Creator", "Custom")

                    cmd.ExecuteNonQuery()
                End Using

                System.Threading.Thread.Sleep(1000)

                '----------------------------------------------------------
                ' Reset UI exactly like original behavior
                '----------------------------------------------------------
                cmdCustomFM.Text = "Custom" & vbCrLf & "FM"
                grpCustFM.Visible = False

                cmbFM1.Items.Clear()
                cmbFM2.Items.Clear()
                cmbFM3.Items.Clear()
                cmbFM4.Items.Clear()
                cmbDefaultFM.Items.Clear()

                cmbFM1.Items.Add("None")
                cmbFM2.Items.Add("None")
                cmbFM3.Items.Add("None")
                cmbFM3.Items.Add("None")

                ' load back all FMNums into the compare FM combos
                Dim sqlReload As String =
                "SELECT FMNum FROM LUT_FuelModelParameters ORDER BY FMNum"

                gf_SetControl(cmbFM1, sqlReload, strProjectPath)
                gf_SetControl(cmbFM2, sqlReload, strProjectPath)
                gf_SetControl(cmbFM3, sqlReload, strProjectPath)
                gf_SetControl(cmbFM4, sqlReload, strProjectPath)
                gf_SetControl(cmbDefaultFM, sqlReload, strProjectPath)

                cmbDefaultFM.SelectedIndex = 0

                cmbFM1.Text = "None"
                cmbFM2.Text = "None"
                cmbFM3.Text = "None"
                cmbFM4.Text = "None"

            End Using

        Catch ex As Exception
            MsgBox("Error in cmdSaveCSTFM_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdDelCstFM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelCstFM.Click

        Try
            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                Dim fmNum As Integer = CInt(cmbDefaultFM.Text)

                '---------------------------------------------------------
                ' Protect Anderson 13, Nonburnable, Scott & Burgan models
                '---------------------------------------------------------
                If fmNum <= 13 Then
                    MsgBox("Error: Anderson 13 fuel models cannot be deleted")
                    Return

                ElseIf (fmNum >= 91 And fmNum <= 93) Or
                   (fmNum >= 98 And fmNum <= 99) Then
                    MsgBox("Error: Default nonburnable fuel models cannot be deleted")
                    Return

                ElseIf (fmNum >= 101 And fmNum <= 109) Or
                   (fmNum >= 121 And fmNum <= 124) Or
                   (fmNum >= 141 And fmNum <= 149) Or
                   (fmNum >= 161 And fmNum <= 165) Or
                   (fmNum >= 181 And fmNum <= 189) Or
                   (fmNum >= 201 And fmNum <= 204) Then
                    MsgBox("Error: Scott and Burgan fuel models cannot be deleted")
                    Return
                End If

                '---------------------------------------------------------
                ' Confirmation prompt
                '---------------------------------------------------------
                If MsgBox("Do you really want to delete fuel model " & cmbDefaultFM.Text & "." & vbCrLf,
                      vbYesNo, "Delete Rule?") <> vbYes Then
                    Return
                End If

                '---------------------------------------------------------
                ' Perform DELETE using SQLiteCommand
                '---------------------------------------------------------
                Dim sqlDelete As String =
                "DELETE FROM LUT_FuelModelParameters WHERE FMNum = @fmnum"

                Using cmd As New SQLiteCommand(sqlDelete, conn)
                    cmd.Parameters.AddWithValue("@fmnum", fmNum)
                    cmd.ExecuteNonQuery()
                End Using

                Thread.Sleep(1000)

                '---------------------------------------------------------
                ' Reset UI — identical logic to original
                '---------------------------------------------------------
                cmbFM1.Items.Clear()
                cmbFM2.Items.Clear()
                cmbFM3.Items.Clear()
                cmbFM4.Items.Clear()
                cmbDefaultFM.Items.Clear()

                cmbFM1.Items.Add("None")
                cmbFM2.Items.Add("None")
                cmbFM3.Items.Add("None")
                cmbFM4.Items.Add("None")

                Dim sqlReload As String =
                "SELECT FMNum FROM LUT_FuelModelParameters ORDER BY FMNum"

                gf_SetControl(cmbFM1, sqlReload, strProjectPath)
                gf_SetControl(cmbFM2, sqlReload, strProjectPath)
                gf_SetControl(cmbFM3, sqlReload, strProjectPath)
                gf_SetControl(cmbFM4, sqlReload, strProjectPath)
                gf_SetControl(cmbDefaultFM, sqlReload, strProjectPath)

                cmbDefaultFM.SelectedIndex = 0

                cmbFM1.Text = "None"
                cmbFM2.Text = "None"
                cmbFM3.Text = "None"
                cmbFM4.Text = "None"

            End Using

        Catch ex As Exception
            MsgBox("Error in cmdDelCstFM_Click - " & ex.Message)
        End Try

    End Sub



    Private Sub lstVwRulesets_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstVwRulesets.MouseDown
        Dim MousePt As Drawing.Point
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


    Private Sub cmsEditRule_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles cmsEditRule.Closing
        cmsEditRule.Items.Clear()
    End Sub

    Private Sub cmsLowHigh_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles cmsLowHigh.Closing
        cmsLowHigh.Items.Clear()
    End Sub

    Private Sub PopCMSEditRule(ByVal strClickedLowHigh As String, ByVal MPoint As Drawing.Point)

        Dim strNum As String
        Dim strCode As String
        Dim strSQL As String

        If IsEVTSelected() = False Then Exit Sub

        Try

            Dim evtNum As Integer = gf_GetNum(cmbEVT.Text, "EVT")
            Dim distNum As Integer

            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName)
                conn.Open()

                cmsEditRule.Items.Clear()

                Select Case strCMSItem

            ' ---------------------------------------------------------
            ' CovLH
            ' ---------------------------------------------------------
                    Case "CovLH"
                        strSQL =
                            "SELECT EVCR FROM " & comboR &
                            " WHERE EVTR=@EVT AND DIST=@DIST " &
                            " GROUP BY EVCR ORDER BY EVCR"

                        addToCMSEditRule(strSQL, "cov", strClickedLowHigh,
                                               ruleE.IntCovLow, ruleE.IntCovHigh)

            ' ---------------------------------------------------------
            ' HgtLH
            ' ---------------------------------------------------------
                    Case "HgtLH"
                        strSQL =
                            "SELECT EVHR FROM " & comboR &
                            " WHERE EVTR=@EVT AND DIST=@DIST " &
                            " GROUP BY EVHR ORDER BY EVHR"

                        addToCMSEditRule(strSQL, "hgt", strClickedLowHigh,
                                               ruleE.IntHgtLow, ruleE.IntHgtHigh)

            ' ---------------------------------------------------------
            ' BPS
            ' ---------------------------------------------------------
                    Case "BPS"
                        strSQL =
                            "SELECT " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                            "FROM " & comboR &
                            " LEFT JOIN LUT_BPS ON " & comboR & ".BPSRF = LUT_BPS.BPS " &
                            "WHERE EVTR=@EVT AND DIST=@DIST " &
                            " GROUP BY " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                            " ORDER BY BPSRF"

                        cmsEditRule.Items.Add("any")

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            cmd.Parameters.AddWithValue("@EVT", evtNum)
                            cmd.Parameters.AddWithValue("@DIST", distNum)

                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    cmsEditRule.Items.Add(
                                        rd.GetValue(0).ToString() & "   " &
                                        rd.GetValue(1).ToString() & " - " &
                                        rd.GetValue(2).ToString()
                                    )
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' Wild
            ' ---------------------------------------------------------
                    Case "Wild"
                        strSQL =
                            "SELECT WILDCARD FROM " & comboR &
                            " WHERE EVTR=@EVT AND DIST=@DIST " &
                            " GROUP BY WILDCARD ORDER BY WILDCARD"

                        cmsEditRule.Items.Add("any")

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            cmd.Parameters.AddWithValue("@EVT", evtNum)
                            cmd.Parameters.AddWithValue("@DIST", distNum)

                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    cmsEditRule.Items.Add(rd.GetValue(0).ToString())
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' FM13
            ' ---------------------------------------------------------
                    Case "FM13"
                        strSQL =
                            "SELECT FMNum, FMName " &
                            "FROM LUT_FuelModelParameters " &
                            "WHERE Creator='Anderson13' OR Creator='Nonburnable' " &
                            "ORDER BY FMNum"

                        cmsEditRule.Items.Add("9999   Nothing Assigned")

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    cmsEditRule.Items.Add(
                                        rd.GetValue(0).ToString() & "   " &
                                        rd.GetValue(1).ToString()
                                    )
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' FM40
            ' ---------------------------------------------------------
                    Case "FM40"
                        strSQL =
                            "SELECT FMNum, FMCode, FMName " &
                            "FROM LUT_FuelModelParameters " &
                            "WHERE Creator='ScottAndBurgan40' OR Creator='Nonburnable' " &
                            "ORDER BY FMNum"

                        cmsEditRule.Items.Add("     9999   Nothing Assigned")

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    strNum = rd.GetValue(0).ToString().PadLeft(3, "0"c)
                                    strCode = rd.GetValue(1).ToString().PadLeft(3, "0"c)

                                    cmsEditRule.Items.Add(
                                        strCode & " / " & strNum &
                                        "   " & rd.GetValue(2).ToString()
                                    )
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' CanFM
            ' ---------------------------------------------------------
                    Case "CanFM"
                        strSQL =
                            "SELECT FM, Description " &
                            "FROM LUT_Canadian_FBPS_Fuel_Types " &
                            "WHERE FMID<>0 AND FMID<>-9999 " &
                            "ORDER BY ID"

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    cmsEditRule.Items.Add(
                                        rd.GetValue(0).ToString() & "   " &
                                        rd.GetValue(1).ToString()
                                    )
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' FLM
            ' ---------------------------------------------------------
                    Case "FLM"
                        strSQL =
                            "SELECT FLM, Description " &
                            "FROM LUT_FLM_Lutes " &
                            "ORDER BY ID"

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    cmsEditRule.Items.Add(
                                        rd.GetValue(0).ToString() & "   " &
                                        rd.GetValue(1).ToString()
                                    )
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' CG
            ' ---------------------------------------------------------
                    Case "CG"
                        strSQL =
                            "SELECT Canopy_Fuel_Mask, Description " &
                            "FROM LUT_Canopy_Fuel_Mask " &
                            "ORDER BY ID"

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    cmsEditRule.Items.Add(
                                        rd.GetValue(0).ToString() & "   " &
                                        rd.GetValue(1).ToString()
                                    )
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' CC
            ' ---------------------------------------------------------
                    Case "CC"
                        cmsEditRule.Items.Add("9999")

                        strSQL =
                            "SELECT MidPoint FROM LUT_Cover WHERE Lifeform='Tree'"

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    cmsEditRule.Items.Add(rd.GetValue(0).ToString())
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' CH
            ' ---------------------------------------------------------
                    Case "CH"
                        cmsEditRule.Items.Add("9999")

                        strSQL =
                            "SELECT MidPoint FROM LUT_Height WHERE Lifeform='Tree'"

                        Using cmd As New SQLiteCommand(strSQL, conn)
                            Using rd As SQLiteDataReader = cmd.ExecuteReader()
                                While rd.Read()
                                    cmsEditRule.Items.Add(
                                        (CInt(rd.GetValue(0)) * 10).ToString() &
                                        "(m x 10)"
                                    )
                                End While
                            End Using
                        End Using


            ' ---------------------------------------------------------
            ' CBD13 / CBD40
            ' ---------------------------------------------------------
                    Case "CBD13", "CBD40"
                        cmsEditRule.Items.Add("9999")
                        For i As Integer = 1 To 45
                            cmsEditRule.Items.Add(i & " kg/m^3x100")
                        Next


            ' ---------------------------------------------------------
            ' CBH13 / CBH40
            ' ---------------------------------------------------------
                    Case "CBH13", "CBH40"
                        cmsEditRule.Items.Add("9999")
                        For i As Integer = 1 To 100
                            cmsEditRule.Items.Add(i & " mx10")
                        Next


            ' ---------------------------------------------------------
            ' OnOff
            ' ---------------------------------------------------------
                    Case "OnOff"
                        cmsEditRule.Items.Add("On")
                        cmsEditRule.Items.Add("Off")

                End Select

                cmsEditRule.Show(MPoint)


                '' ---------------------------------------------------------
                '' CovLH → Cover Low/High
                '' ---------------------------------------------------------
                'If strCMSItem = "CovLH" Then

                '    strSQL =
                '    "SELECT EVCR FROM " & comboR &
                '    " WHERE EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") &
                '    " AND DIST = " & gf_GetNum(cmbEVT.Text, "DIST") &
                '    " GROUP BY EVCR ORDER BY EVCR"

                '    addToCMSEditRule(strSQL, "cov", strClickedLowHigh, ruleE.IntCovLow, ruleE.IntCovHigh)
                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' HgtLH → Height Low/High
                '' ---------------------------------------------------------
                'If strCMSItem = "HgtLH" Then

                '    strSQL =
                '    "SELECT EVHR FROM " & comboR &
                '    " WHERE EVTR = " & gf_GetNum(cmbEVT.Text, "EVT") &
                '    " AND DIST = " & gf_GetNum(cmbEVT.Text, "DIST") &
                '    " GROUP BY EVHR ORDER BY EVHR"

                '    addToCMSEditRule(strSQL, "hgt", strClickedLowHigh, ruleE.IntHgtLow, ruleE.IntHgtHigh)
                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' BPS
                '' ---------------------------------------------------------
                'If strCMSItem = "BPS" Then

                '    strSQL =
                '    "SELECT " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                '    "FROM " & comboR &
                '    " LEFT JOIN LUT_BPS ON " & comboR & ".BPSRF = LUT_BPS.BPS " &
                '    "WHERE EVTR=" & gf_GetNum(cmbEVT.Text, "EVT") &
                '    " AND DIST=" & gf_GetNum(cmbEVT.Text, "DIST") &
                '    " GROUP BY " & comboR & ".BPSRF, LUT_BPS.Name, LUT_BPS.BPS_Model " &
                '    " ORDER BY BPSRF"

                '    cmsEditRule.Items.Add("any")

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                cmsEditRule.Items.Add(
                '                rd(0).ToString() & "   " &
                '                rd(1).ToString() & " - " &
                '                rd(2).ToString()
                '            )
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' Wild
                '' ---------------------------------------------------------
                'If strCMSItem = "Wild" Then

                '    strSQL =
                '    "SELECT WILDCARD FROM " & comboR &
                '    " WHERE EVTR=" & gf_GetNum(cmbEVT.Text, "EVT") &
                '    " AND DIST=" & gf_GetNum(cmbEVT.Text, "DIST") &
                '    " GROUP BY WILDCARD ORDER BY WILDCARD"

                '    cmsEditRule.Items.Add("any")

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                cmsEditRule.Items.Add(rd(0).ToString())
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' FM13
                '' ---------------------------------------------------------
                'If strCMSItem = "FM13" Then

                '    strSQL =
                '    "SELECT FMNum, FMName " &
                '    "FROM LUT_FuelModelParameters " &
                '    "WHERE Creator='Anderson13' OR Creator='Nonburnable' " &
                '    "ORDER BY FMNum"

                '    cmsEditRule.Items.Add("9999   Nothing Assigned")

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                cmsEditRule.Items.Add(rd(0).ToString() & "   " & rd(1).ToString())
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' FM40
                '' ---------------------------------------------------------
                'If strCMSItem = "FM40" Then

                '    strSQL =
                '    "SELECT FMNum, FMCode, FMName " &
                '    "FROM LUT_FuelModelParameters " &
                '    "WHERE Creator='ScottAndBurgan40' OR Creator='Nonburnable' " &
                '    "ORDER BY FMNum"

                '    cmsEditRule.Items.Add("     9999   Nothing Assigned")

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                strNum = rd(0).ToString()
                '                strCode = rd(1).ToString()

                '                If strNum.Length = 1 Then strNum = "00" & strNum
                '                If strNum.Length = 2 Then strNum = "0" & strNum
                '                If strCode.Length = 1 Then strCode = "00" & strCode
                '                If strCode.Length = 2 Then strCode = "0" & strCode

                '                cmsEditRule.Items.Add(strCode & " / " & strNum & "   " & rd(2).ToString())
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' CanFM
                '' ---------------------------------------------------------
                'If strCMSItem = "CanFM" Then

                '    strSQL =
                '    "SELECT FM, Description " &
                '    "FROM LUT_Canadian_FBPS_Fuel_Types " &
                '    "WHERE FMID<>0 AND FMID<>-9999 " &
                '    "ORDER BY ID"

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                cmsEditRule.Items.Add(rd(0).ToString() & "   " & rd(1).ToString())
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' FLM
                '' ---------------------------------------------------------
                'If strCMSItem = "FLM" Then

                '    strSQL =
                '    "SELECT FLM, Description " &
                '    "FROM LUT_FLM_Lutes " &
                '    "ORDER BY ID"

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                cmsEditRule.Items.Add(rd(0).ToString() & "   " & rd(1).ToString())
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' CG
                '' ---------------------------------------------------------
                'If strCMSItem = "CG" Then

                '    strSQL =
                '    "SELECT Canopy_Fuel_Mask, Description " &
                '    "FROM LUT_Canopy_Fuel_Mask " &
                '    "ORDER BY ID"

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                cmsEditRule.Items.Add(rd(0).ToString() & "   " & rd(1).ToString())
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' CC (Canopy Cover)
                '' ---------------------------------------------------------
                'If strCMSItem = "CC" Then

                '    cmsEditRule.Items.Add("9999")

                '    strSQL =
                '    "SELECT MidPoint FROM LUT_Cover WHERE Lifeform='Tree'"

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                cmsEditRule.Items.Add(rd("MidPoint").ToString())
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' CH (Canopy Height)
                '' ---------------------------------------------------------
                'If strCMSItem = "CH" Then

                '    cmsEditRule.Items.Add("9999")

                '    strSQL =
                '    "SELECT MidPoint FROM LUT_Height WHERE Lifeform='Tree'"

                '    Using cmd As New SQLiteCommand(strSQL, conn)
                '        Using rd = cmd.ExecuteReader()
                '            While rd.Read()
                '                cmsEditRule.Items.Add((CDbl(rd("MidPoint")) * 10).ToString() & "(m x 10)")
                '            End While
                '        End Using
                '    End Using

                '    cmsEditRule.Show(MPoint)
                '    Exit Sub

                'End If

                '' ---------------------------------------------------------
                '' CBD13
                '' ---------------------------------------------------------
                'If strCMSItem = "CBD13" Then
                '    cmsEditRule.Items.Add("9999")
                '    For i As Integer = 1 To 45
                '        cmsEditRule.Items.Add(i & " kg/m^3x100")
                '    Next
                '    cmsEditRule.Show(MPoint)
                '    Exit Sub
                'End If

                '' ---------------------------------------------------------
                '' CBD40
                '' ---------------------------------------------------------
                'If strCMSItem = "CBD40" Then
                '    cmsEditRule.Items.Add("9999")
                '    For i As Integer = 1 To 45
                '        cmsEditRule.Items.Add(i & " kg/m^3x100")
                '    Next
                '    cmsEditRule.Show(MPoint)
                '    Exit Sub
                'End If

                '' ---------------------------------------------------------
                '' CBH13 / CBH40
                '' ---------------------------------------------------------
                'If strCMSItem = "CBH13" Then
                '    cmsEditRule.Items.Add("9999")
                '    For i As Integer = 1 To 100
                '        cmsEditRule.Items.Add(i & " mx10")
                '    Next
                '    cmsEditRule.Show(MPoint)
                '    Exit Sub
                'End If

                'If strCMSItem = "CBH40" Then
                '    cmsEditRule.Items.Add("9999")
                '    For i As Integer = 1 To 100
                '        cmsEditRule.Items.Add(i & " mx10")
                '    Next
                '    cmsEditRule.Show(MPoint)
                '    Exit Sub
                'End If

                '' ---------------------------------------------------------
                '' OnOff
                '' ---------------------------------------------------------
                'If strCMSItem = "OnOff" Then
                '    cmsEditRule.Items.Add("On")
                '    cmsEditRule.Items.Add("Off")
                '    cmsEditRule.Show(MPoint)
                '    Exit Sub
                'End If

            End Using

        Catch ex As Exception
            MsgBox("PopCMSEditRule - " & ex.Message)
        End Try

    End Sub

    Private Sub cmsLowHigh_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmsLowHigh.ItemClicked
        PopCMSEditRule(e.ClickedItem.Text, cmsLowHigh.Location)
    End Sub

    Private Function ChangeNote(note As String, oldVal As Object, newVal As Object) As String
        Return note & "  (" & oldVal & ") to (" & newVal & ")"
    End Function

    Private Sub cmsEditRule_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmsEditRule.ItemClicked
        Try
            If Not IsEVTSelected() Then Return

            'Accessing SelectedItems(0) also validates a ruleset row is selected
            '(throws into the Catch below if not). Value itself is unused — preserved from original.
            Dim selectedIndex As Integer = lstVwRulesets.SelectedItems(0).Index + 1

            Dim clicked As String = e.ClickedItem.Text

            'Set the beginning of the note
            Dim note As String = ruleE.Notes & vbCrLf & Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                                 txtSessionName.Text & ": Changed "

            Select Case strCMSItem
                Case "cov low"
                    If ruleE.StrCovLow <> clicked Then
                        note = ChangeNote(note, ruleE.StrCovLow, clicked)
                        ruleE.StrCovLow = clicked
                        gr_ClearPAP(RulesetCollection)
                    End If
                    'Make all the values the same if Low cover is lessthan 101
                    If Int(gf_ConvertBack(clicked, strProjectPath)) < 101 Then
                        note = ChangeNote(note, ruleE.StrCovHigh, clicked) : ruleE.StrCovHigh = clicked
                        note = ChangeNote(note, ruleE.StrHgtLow, clicked) : ruleE.StrHgtLow = clicked
                        note = ChangeNote(note, ruleE.StrHgtHigh, clicked) : ruleE.StrHgtHigh = clicked
                        gr_ClearPAP(RulesetCollection)
                    End If

                Case "cov high"
                    If ruleE.StrCovHigh <> clicked Then
                        note = ChangeNote(note, ruleE.StrCovHigh, clicked)
                        ruleE.StrCovHigh = clicked
                        gr_ClearPAP(RulesetCollection)
                    End If

                Case "hgt low"
                    If ruleE.StrHgtLow <> clicked Then
                        note = ChangeNote(note, ruleE.StrHgtLow, clicked)
                        ruleE.StrHgtLow = clicked
                        gr_ClearPAP(RulesetCollection)
                    End If
                    'Make all the values the same if Low cover is lessthan 101
                    If Int(gf_ConvertBack(clicked, strProjectPath)) < 101 Then
                        note = ChangeNote(note, ruleE.StrHgtHigh, clicked) : ruleE.StrHgtHigh = clicked
                        note = ChangeNote(note, ruleE.StrCovLow, clicked) : ruleE.StrCovLow = clicked
                        note = ChangeNote(note, ruleE.StrCovHigh, clicked) : ruleE.StrCovHigh = clicked
                        gr_ClearPAP(RulesetCollection)
                    End If

                Case "hgt high"
                    If ruleE.StrHgtHigh <> clicked Then
                        note = ChangeNote(note, ruleE.StrHgtHigh, clicked)
                        ruleE.StrHgtHigh = clicked
                        gr_ClearPAP(RulesetCollection)
                    End If

                Case "Add New Rule"
                    AddNewRule(clicked)

                Case "BPS"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.BPS <> newVal Then
                        note = ChangeNote(note, ruleE.BPS, newVal)
                        ruleE.BPS = newVal
                        gr_ClearPAP(RulesetCollection)
                    End If

                Case "Wild"
                    If ruleE.Wildcard <> clicked Then
                        note = ChangeNote(note, ruleE.Wildcard, clicked)
                        ruleE.Wildcard = clicked
                        gr_ClearPAP(RulesetCollection)
                    End If

                Case "FM13"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.FBFM13 <> newVal Then
                        note = ChangeNote(note, ruleE.FBFM13, newVal)
                        ruleE.FBFM13 = newVal
                    End If

                Case "FM40"
                    Dim newVal As String = Trim(Strings.Left(clicked, 9))
                    If ruleE.FBFM40 <> newVal Then
                        note = ChangeNote(note, ruleE.FBFM40, newVal)
                        ruleE.FBFM40 = newVal
                    End If

                Case "CanFM"
                    Dim newVal As String = Trim(Strings.Left(clicked, 9))
                    If ruleE.CanFM <> newVal Then
                        note = ChangeNote(note, ruleE.CanFM, newVal)
                        ruleE.CanFM = newVal
                    End If

                Case "FCCS"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.FCCS <> newVal Then
                        note = ChangeNote(note, ruleE.FCCS, newVal)
                        ruleE.FCCS = newVal
                    End If

                Case "FLM"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.FLM <> newVal Then
                        note = ChangeNote(note, ruleE.FLM, newVal)
                        ruleE.FLM = newVal
                    End If

                Case "CG"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.Canopy <> newVal Then
                        note = ChangeNote(note, ruleE.Canopy, newVal)
                        ruleE.Canopy = newVal
                    End If

                Case "CC"
                    If ruleE.CCover <> clicked Then
                        note = ChangeNote(note, ruleE.CCover, clicked)
                        ruleE.CCover = clicked
                    End If

                Case "CH"
                    'Preserved quirk: compares against raw text, but stores/notes the gf_GetNum value.
                    If ruleE.CHeight <> clicked Then
                        note = ChangeNote(note, ruleE.CHeight, gf_GetNum(clicked, "General"))
                        ruleE.CHeight = gf_GetNum(clicked, "General")
                    End If

                Case "CBD13"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.CBD13 <> newVal Then
                        note = ChangeNote(note, ruleE.CBD13, newVal)
                        ruleE.CBD13 = newVal
                    End If

                Case "CBD40"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.CBD40 <> newVal Then
                        note = ChangeNote(note, ruleE.CBD40, newVal)
                        ruleE.CBD40 = newVal
                    End If

                Case "CBH13"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.CBH13 <> newVal Then
                        note = ChangeNote(note, ruleE.CBH13, newVal)
                        ruleE.CBH13 = newVal
                    End If

                Case "CBH40"
                    Dim newVal As Object = gf_GetNum(clicked, "General")
                    If ruleE.CBH40 <> newVal Then
                        note = ChangeNote(note, ruleE.CBH40, newVal)
                        ruleE.CBH40 = newVal
                    End If

                Case "OnOff"
                    If ruleE.OnOff <> clicked Then
                        note = ChangeNote(note, ruleE.OnOff, clicked)
                        ruleE.OnOff = clicked
                        gr_ClearPAP(RulesetCollection)
                    End If
            End Select

            ruleE.Notes = note

            gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                           RulesetCollection, EVTPixelCountCollection, strProjectPath)
            DisplayRuleset()
            AdjPer()
        Catch ex As Exception
            MsgBox("Error in cmsEditRule_ItemClicked - " & ex.Message)
        End Try
    End Sub

    Private Sub AddNewRule(clickedText As String)
        Dim lifeform As String
        Dim cCover As Integer
        Dim label As String

        Select Case clickedText
            Case "Add Ag, Urban, Developed, Or Sparse Rule"
                lifeform = "Sparse" : cCover = 0 : label = "NEW Ag, Urban, Developed, Or Sparse Rule"
            Case "Add Herb Rule"
                lifeform = "Herb" : cCover = 0 : label = "NEW Herb Rule"
            Case "Add Shrub Rule"
                lifeform = "Shrub" : cCover = 0 : label = "NEW Shrub Rule"
            Case "Add Tree Rule"
                lifeform = "Tree" : cCover = 1 : label = "NEW Tree Rule"
            Case Else
                Return 'No matching add option — nothing inserted (matches original)
        End Select

        Dim minCov As Integer = 9999    'Used for a given lifeform, default if not found
        Dim minHgt As Integer = 9999    'Used for a given lifeform, default if not found

        Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName & ";")
            conn.Open()

            'Find lifeform min for cover and height
            Dim lookupSql As String =
                "SELECT LUT_Cover.Lifeform, Min(LUT_Cover.EVC) AS MinOfEVC, Min(LUT_Height.EVH) AS MinOfEVH " &
                "FROM LUT_Height INNER JOIN LUT_Cover ON LUT_Height.Lifeform = LUT_Cover.Lifeform " &
                "GROUP BY LUT_Cover.Lifeform " &
                "HAVING LUT_Cover.Lifeform IN ('Herb', 'Shrub', 'Sparse', 'Tree')"

            Using cmd As New SQLiteCommand(lookupSql, conn)
                Using reader As SQLiteDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        If reader("Lifeform").ToString() = lifeform Then
                            minCov = CInt(reader("MinOfEVC"))
                            minHgt = CInt(reader("MinOfEVH"))
                            Exit While
                        End If
                    End While
                End Using
            End Using

            Dim insertSql As String =
                "INSERT INTO " & rulesR & " (EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
                "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                "CBH13mx10, CBH40mx10, OnOff, Notes) " &
                "VALUES (@evt, @dist, @cov, @cov, @hgt, @hgt, 'any', 'any', 9999, 9999, '9999', '9999', 9999, 9999, " &
                "@ccover, 9999, 9999, 9999, 9999, 9999, 'On', @note)"

            Dim noteText As String = Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                                     txtSessionName.Text & ": " & label

            Using cmd As New SQLiteCommand(insertSql, conn)
                cmd.Parameters.AddWithValue("@evt", gf_GetNum(cmbEVT.Text, "EVT"))
                cmd.Parameters.AddWithValue("@dist", gf_GetNum(cmbEVT.Text, "DIST"))
                cmd.Parameters.AddWithValue("@cov", minCov)
                cmd.Parameters.AddWithValue("@hgt", minHgt)
                cmd.Parameters.AddWithValue("@ccover", cCover)
                cmd.Parameters.AddWithValue("@note", noteText)
                cmd.ExecuteNonQuery()
            End Using
        End Using

        'NOTE: original ran these here AND again in the caller's tail (double call). Preserved.
        gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                       RulesetCollection, EVTPixelCountCollection, strProjectPath)
        DisplayRuleset()
    End Sub

    Private Sub addToCMSEditRule(ByVal strSQLCSM As String, ByVal strCovOrHgt As String,
                                 ByVal strLowOrHigh As String, ByVal intGTOET As Integer,
                                 ByVal intLTOET As Integer)
        'LTOET - Less than or equal to
        'GTOET - Greater than or equal to
        Try
            If Not IsEVTSelected() Then Return

            Select Case strLowOrHigh
                Case "Sort by Cover (Default)"
                    ApplyRuleSort("Sort by Cover")

                Case "Sort by Height"
                    ApplyRuleSort("Sort by Height")

                Case "Add New rule to edit"
                    strCMSItem = "Add New Rule"
                    cmsEditRule.Items.Add("Add Ag, Urban, Developed, Or Sparse Rule")
                    cmsEditRule.Items.Add("Add Herb Rule")
                    cmsEditRule.Items.Add("Add Shrub Rule")
                    cmsEditRule.Items.Add("Add Tree Rule")

                Case "Edit Low side of range"
                    strCMSItem = strCovOrHgt & " low"
                    PopulateRangeItems(strSQLCSM, strCovOrHgt, "low", intGTOET, intLTOET)

                Case "Edit High side of range"
                    strCMSItem = strCovOrHgt & " high"
                    PopulateRangeItems(strSQLCSM, strCovOrHgt, "high", intGTOET, intLTOET)

                Case Else
                    'It is not a proper selection
            End Select
        Catch ex As Exception
            MsgBox("Error in addToCMSEditRule - " & ex.Message)
        End Try
    End Sub


    Private Sub ApplyRuleSort(sortMode As String)
        gr_SetRuleSort = sortMode
        gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                       RulesetCollection, EVTPixelCountCollection, strProjectPath)
        DisplayRuleset()
    End Sub

    Private Sub PopulateRangeItems(sqlCms As String, covOrHgt As String, side As String,
                                   gtoet As Integer, ltoet As Integer)
        Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName & ";")
            conn.Open()

            'Find lifeform of selected value for cover and height and bound the range
            If side = "low" Then
                gtoet = ResolveLowBound(conn, covOrHgt, gtoet)
            Else
                ltoet = ResolveHighBound(conn, covOrHgt, ltoet)
            End If

            'Populate cmsEditRule
            Using cmd As New SQLiteCommand(sqlCms, conn)
                Using reader As SQLiteDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim insertValue As Integer = CInt(reader.GetValue(0))

                        If insertValue >= gtoet And insertValue <= ltoet Then
                            cmsEditRule.Items.Add(gf_ConvertCode(insertValue, covOrHgt, side, strProjectPath))
                        End If
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Function ResolveLowBound(conn As SQLiteConnection, covOrHgt As String, gtoet As Integer) As Integer
        Dim sql As String =
            "SELECT LUT_Cover.Lifeform, Min(LUT_Cover.EVC) AS MinOfEVC, Min(LUT_Height.EVH) AS MinOfEVH " &
            "FROM LUT_Height INNER JOIN LUT_Cover ON LUT_Height.Lifeform = LUT_Cover.Lifeform " &
            "GROUP BY LUT_Cover.Lifeform " &
            "HAVING LUT_Cover.Lifeform IN ('Herb', 'Shrub', 'Tree') " &
            "ORDER BY LUT_Cover.Lifeform"

        'First loop is herb, second is shrub, third is tree
        Using cmd As New SQLiteCommand(sql, conn)
            Using reader As SQLiteDataReader = cmd.ExecuteReader()
                While reader.Read()
                    If covOrHgt = "cov" And gtoet >= CInt(reader("MinOfEVC")) Then      'If cov use gtoet gets cover
                        Return CInt(reader("MinOfEVC"))
                    ElseIf covOrHgt = "hgt" And gtoet >= CInt(reader("MinOfEVH")) Then  'If hgt use gtoet gets height
                        Return CInt(reader("MinOfEVH"))
                    ElseIf gtoet <= 100 Then                                            'If Sparse or 2 digit it gets lowest
                        Return 11
                    End If
                End While
            End Using
        End Using

        Return gtoet 'No match found — leave unchanged (matches original)
    End Function

    Private Function ResolveHighBound(conn As SQLiteConnection, covOrHgt As String, ltoet As Integer) As Integer
        Dim sql As String =
            "SELECT LUT_Cover.Lifeform, Max(LUT_Cover.EVC) AS MaxOfEVC, Max(LUT_Height.EVH) AS MaxOfEVH " &
            "FROM LUT_Height INNER JOIN LUT_Cover ON LUT_Height.Lifeform = LUT_Cover.Lifeform " &
            "GROUP BY LUT_Cover.Lifeform " &
            "HAVING LUT_Cover.Lifeform IN ('Herb', 'Shrub', 'Tree') " &
            "ORDER BY LUT_Cover.Lifeform DESC"

        'First loop is Tree, second is shrub, third is herb
        Using cmd As New SQLiteCommand(sql, conn)
            Using reader As SQLiteDataReader = cmd.ExecuteReader()
                While reader.Read()
                    If covOrHgt = "cov" And ltoet <= CInt(reader("MaxOfEVC")) Then      'If cov use ltoet gets cover
                        Return CInt(reader("MaxOfEVC"))
                    ElseIf covOrHgt = "hgt" And ltoet <= CInt(reader("MaxOfEVH")) Then  'If hgt use ltoet gets height
                        Return CInt(reader("MaxOfEVH"))
                    ElseIf ltoet <= 100 Then                                            'If Sparse or 2 digit it gets lowest
                        Return 100
                    End If
                End While
            End Using
        End Using

        Return ltoet 'No match found — leave unchanged (matches original)
    End Function

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

    'strSQL is re-executed downstream by gf_SetControl, so these queries cannot be
    'parameterized — literals must be escaped instead.
    Private Function SqlLiteral(value As String) As String
        Return "'" & value.Replace("'", "''") & "'"
    End Function

    Private Sub OrderAndSortEVT()
        Const LUT_Table As String = "XWALK_EVT_EVG_EVS"     'Set the look up table
        Const LUT_Name As String = "EVT_Name"               'Set the lookup name field
        Const LUT_Num As String = "EVT"                     'Set the lookup number field

        Try
            'Stores the order by string
            Dim orderNameOrNumber As String = If(rdoName.Checked,
                                                 LUT_Table & "." & LUT_Name,
                                                 comboR & ".EVTR")

            strSQL = BuildSortSql(LUT_Table, LUT_Name, LUT_Num, orderNameOrNumber)

            'Check for EVTs after selection if none then clear the CMBEVT and the rulesets
            Dim hasRows As Boolean
            Using conn As New SQLiteConnection("Data Source=" & strProjectPath & "\" & gs_LFTFCSQliteName & ";")
                conn.Open()
                Using cmd As New SQLiteCommand(strSQL, conn)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        hasRows = reader.Read()
                    End Using
                End Using
            End Using

            If Not hasRows Then
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
                gr_MakeRuleset(gf_GetNum(cmbEVT.Text, "EVT"), gf_GetNum(cmbEVT.Text, "DIST"), comboR, rulesR,
                               RulesetCollection, EVTPixelCountCollection, strProjectPath)
                DisplayRuleset()
                AdjPer()
            End If
        Catch ex As Exception
            MsgBox("Error in OrderAndSortEVT- " & ex.Message)
        End Try
    End Sub

    Private Function BuildSortSql(lutTable As String, lutName As String, lutNum As String,
                                  orderNameOrNumber As String) As String
        Dim selectFrom As String =
            "SELECT " & comboR & ".EVTR, " & comboR & ".DIST, " & lutTable & "." & lutName & " " &
            "FROM " & comboR & " LEFT JOIN " & lutTable & " " &
            "ON " & comboR & ".EVTR = " & lutTable & "." & lutNum & " "

        Dim groupBy As String =
            "GROUP BY " & comboR & ".EVTR, " & comboR & ".DIST, " & lutTable & "." & lutName & " "

        Select Case cmbSortRules.SelectedIndex
            Case 0      'All by Type
                Return selectFrom & groupBy &
                       "ORDER BY " & comboR & ".DIST, " & orderNameOrNumber

            Case 1      'All by EVT
                Return selectFrom & groupBy &
                       "ORDER BY " & orderNameOrNumber & ", " & comboR & ".DIST"

            Case 2      'Disturbed by Type
                Return selectFrom & groupBy &
                       "HAVING " & comboR & ".DIST > 0 " &
                       "ORDER BY " & comboR & ".DIST, " & orderNameOrNumber

            Case 3      'Disturbed by EVT
                Return selectFrom & groupBy &
                       "HAVING " & comboR & ".DIST > 0 " &
                       "ORDER BY " & orderNameOrNumber & ", " & comboR & ".DIST"

            Case 4      'Specific EVT
                Dim tempEVT As String = "False"     'Start with false for do until
                Do Until tempEVT <> "False"
                    tempEVT = InputBox("Enter the 4 digit " & lutNum & " code you want to sort." &
                                       "Example " & lutNum & " 2227[0]: Enter 2227", "Sort for specific " & lutNum, "")
                    If tempEVT = "" Then
                        cmbSortRules.SelectedIndex = 0
                    ElseIf tempEVT.Length = 4 AndAlso IsNumeric(tempEVT) Then
                        'tempEVT is validated as 4-digit numeric, safe to embed
                        Return selectFrom & groupBy &
                               "HAVING " & comboR & ".EVTR = " & tempEVT & " " &
                               "ORDER BY " & orderNameOrNumber & ", " & comboR & ".DIST"
                    Else
                        MsgBox(lutNum & " #: " & tempEVT & " does not exist in the Managament Unit" & vbCrLf &
                               "Try another " & lutNum & " #")
                        tempEVT = "False"
                    End If
                Loop

                'Empty input: original assigns no SQL on this path and falls through with
                'whatever strSQL currently holds. Preserved — see notes.
                Return strSQL

            Case Else   'By Specific Disturbance Type
                Return selectFrom &
                       "INNER JOIN LUT_DistCode ON " & comboR & ".DIST = LUT_DistCode.DistCode " &
                       groupBy.TrimEnd() & ", LUT_DistCode.Type " &
                       "HAVING LUT_DistCode.Type = " & SqlLiteral(cmbSortRules.Text) & " " &
                       "ORDER BY " & orderNameOrNumber
        End Select
    End Function

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