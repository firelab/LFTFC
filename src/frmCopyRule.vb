Imports System.Data
Imports System.Windows.Forms

Public Class frmCopyRule
    Private strSQL As String                                                'SQL variable for this module
    Private cmbEVT As ComboBox
    Private SN As String                                                    'Stores the session name
    Private comboR As String                                                'Stores the combo table name for rule making
    Private rulesR As String                                                'Stores the rules table name for rule making
    Private strProjectPath As String
    Private rdoName As Boolean                                              'Stores if sorting by EVT name

    Public Sub New(ByVal cmbTempEVT As ComboBox, ByVal SessionName As String,
                   ByVal RulesTable As String, ByVal ComboTable As String, ByVal ProjPath As String, rdoEVTName As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Dim i As String

        strProjectPath = ProjPath
        cmbEVT = cmbTempEVT
        SN = SessionName
        comboR = ComboTable
        rulesR = RulesTable
        rdoName = rdoEVTName 'Stores if sorting by EVT name

        'Populate cmbCopyMU set the starting value to the first in the list
        strSQL = "SELECT Name " &
                 "FROM DATA_MU_Name " &
                 "ORDER BY Name"

        gf_SetControl(cmbCopyMU, strSQL, strProjectPath)

        For Each i In cmbEVT.Items()
            cmbCopyEVT.Items.Add(i)
        Next i
        cmbCopyEVT.Text = cmbEVT.Text
    End Sub

    Private Sub cmbCopyMU_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) _
                                                   Handles cmbCopyMU.SelectionChangeCommitted
        Try
            Dim selectedMU As String = cmbCopyMU.SelectedItem & "_CMB" 'Stores the EVT table
            cmbCopyEVT.Items.Clear()

            'Populate the cmbEVT with EVT
            strSQL = "SELECT " & selectedMU & ".EVTR, " & selectedMU & ".DIST, XWALK_EVT_EVG_EVS.EVT_Name " &
                     "FROM " & selectedMU & " LEFT JOIN XWALK_EVT_EVG_EVS " &
                     "ON " & selectedMU & ".EVTR = XWALK_EVT_EVG_EVS.EVT " &
                     "GROUP BY " & selectedMU & ".EVTR, " & selectedMU & ".DIST, XWALK_EVT_EVG_EVS.EVT_Name " &
                     "ORDER BY " & selectedMU & ".DIST, XWALK_EVT_EVG_EVS.EVT_Name"

            gf_SetControl(cmbCopyEVT, strSQL, strProjectPath, rdoName)

            'See if the selectedEVT exists in the select MU. If it does set the selectedindex to it
            If cmbCopyEVT.FindString(cmbEVT.Text) <> -1 Then
                cmbCopyEVT.SelectedIndex = cmbCopyEVT.FindString(cmbEVT.Text)
            Else
                cmbCopyEVT.SelectedIndex = 0 'If not set the selected index to the first value
            End If

        Catch ex As Exception
            MsgBox("Error in cmbCopyMU_SelectionChangeCommitted - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdCopyRule_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles cmdCopyRule.MouseClick

        Dim dbPath As String = System.IO.Path.Combine(strProjectPath, gs_LFTFCSQliteName)
        Dim connString As String = "Data Source=" & dbPath & ";Version=3;"
        Dim strSQL As String = ""

        Try
            '-------------------------------------------------------
            ' Shared field lists (INSERT and SELECT parts)
            '-------------------------------------------------------
            Dim insertFields As String =
            "(EVT, DIST, Cover_Low, Cover_High, Height_Low, Height_High, " &
            "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, " &
            "CCover, CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, Notes)"

            ' Normal SELECT (when copying EVT=... from source)
            Dim selectFields As String =
            "Cover_Low, Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " &
            "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, " &
            "CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff"

            ' SELECT with an EVT/DIST override (used in EVT→EVT copies)
            Dim selectFieldsOverride As String =
            "@evt, @dist, Cover_Low, Cover_High, Height_Low, Height_High, " &
            "BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, " &
            "CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff"

            '-------------------------------------------------------
            ' Confirm user intent
            '-------------------------------------------------------
            Dim strPrompt As String
            Dim strTargetEVT As String
            Dim strTargetDIST As String
            Dim copyMU As String = cmbCopyMU.Text & "_Rulesets"

            If rdoCopyOnOff.Checked Then
                strPrompt = "all ON and OFF rules" & vbCrLf
            Else
                strPrompt = "all ON rules" & vbCrLf
            End If

            If rdoCopyMU.Checked AndAlso rdoAll.Checked Then
                strPrompt &= "in management unit " & cmbCopyMU.Text & vbCrLf &
                         "where rules may already exist?"
            ElseIf rdoCopyMU.Checked AndAlso Not rdoAll.Checked Then
                strPrompt &= "in management unit " & cmbCopyMU.Text & vbCrLf &
                         "only where rulesets are EMPTY?"
            Else
                strPrompt &= "from EVT " & cmbCopyEVT.Text & vbCrLf &
                         "in management unit " & cmbCopyMU.Text & "?"
            End If

            If MsgBox("Are you sure you want to copy " & strPrompt, MsgBoxStyle.YesNo) <> vbYes Then
                Exit Sub
            End If

            strTargetEVT = gf_GetNum(cmbCopyEVT.Text, "EVT")
            strTargetDIST = gf_GetNum(cmbCopyEVT.Text, "DIST")

            Dim noteText As String =
            Now.ToShortTimeString & " " &
            Now.ToShortDateString & " " &
            SN & ": Copied rule from MU - " &
            cmbCopyMU.Text & " EVT " &
            strTargetEVT & "[" & strTargetDIST & "]"

            Dim noteSimple As String =
            Now.ToShortTimeString & " " &
            Now.ToShortDateString & " " &
            SN & ": Copied rule from MU - " & cmbCopyMU.Text

            '-------------------------------------------------------
            ' Construct parametric SQL using shared field lists
            '-------------------------------------------------------

            If rdoCopyEVT.Checked AndAlso rdoCopyOn.Checked Then
                ' Copy ON rules for specific EVT→EVT
                strSQL =
                "INSERT INTO " & rulesR & " " & insertFields & " " &
                "SELECT " & selectFieldsOverride & ", @notes " &
                "FROM " & copyMU & " " &
                "WHERE EVT = @targetEvt AND DIST = @targetDist AND OnOff = 'On' " &
                "ORDER BY OnOff DESC, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, Height_Low, Height_High"

            ElseIf rdoCopyEVT.Checked AndAlso rdoCopyOnOff.Checked Then
                strSQL =
                "INSERT INTO " & rulesR & " " & insertFields & " " &
                "SELECT " & selectFieldsOverride & ", @notes " &
                "FROM " & copyMU & " " &
                "WHERE EVT = @targetEvt AND DIST = @targetDist " &
                "ORDER BY OnOff DESC, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, Height_Low, Height_High"

            ElseIf rdoCopyMU.Checked AndAlso rdoCopyOn.Checked AndAlso rdoAll.Checked Then
                strSQL =
                "INSERT INTO " & rulesR & " " & insertFields & " " &
                "SELECT EVT, DIST, " & selectFields & ", @notesSimple " &
                "FROM " & copyMU & " " &
                "WHERE OnOff = 'On' " &
                "ORDER BY OnOff DESC, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, Height_Low, Height_High"

            ElseIf rdoCopyMU.Checked AndAlso rdoCopyOnOff.Checked AndAlso rdoAll.Checked Then
                strSQL =
                "INSERT INTO " & rulesR & " " & insertFields & " " &
                "SELECT EVT, DIST, " & selectFields & ", @notesSimple " &
                "FROM " & copyMU & " " &
                "ORDER BY OnOff DESC, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, Height_Low, Height_High"

            ElseIf rdoCopyMU.Checked AndAlso rdoCopyOnOff.Checked AndAlso rdoEmpty.Checked Then
                strSQL =
                "INSERT INTO " & rulesR & " " & insertFields & " " &
                "SELECT c.EVT, c.DIST, " & selectFields.Replace("Cover_Low", "c.Cover_Low").Replace("Cover_High", "c.Cover_High") &
                ", @notesSimple " &
                "FROM " & copyMU & " c " &
                "WHERE NOT EXISTS (SELECT 1 FROM " & rulesR & " r WHERE r.EVT = c.EVT AND r.DIST = c.DIST) " &
                "ORDER BY c.OnOff DESC, c.BPSRF DESC, c.Wildcard DESC, c.Cover_Low, c.Cover_High, c.Height_Low, c.Height_High"

            ElseIf rdoCopyMU.Checked AndAlso rdoCopyOn.Checked AndAlso rdoEmpty.Checked Then
                strSQL =
                "INSERT INTO " & rulesR & " " & insertFields & " " &
                "SELECT c.EVT, c.DIST, " & selectFields.Replace("Cover_Low", "c.Cover_Low").Replace("Cover_High", "c.Cover_High") &
                ", @notesSimple " &
                "FROM " & copyMU & " c " &
                "WHERE c.OnOff = 'On' AND NOT EXISTS (SELECT 1 FROM " & rulesR & " r WHERE r.EVT = c.EVT AND r.DIST = c.DIST) " &
                "ORDER BY c.OnOff DESC, c.BPSRF DESC, c.Wildcard DESC, c.Cover_Low, c.Cover_High, c.Height_Low, c.Height_High"

            End If

            '-------------------------------------------------------
            ' Execute using SQLiteTransaction
            '-------------------------------------------------------
            Using conn As New SQLite.SQLiteConnection(connString)
                conn.Open()

                Using tx As SQLite.SQLiteTransaction = conn.BeginTransaction()
                    Using cmd As New SQLite.SQLiteCommand(strSQL, conn, tx)

                        cmd.Parameters.AddWithValue("@evt", gf_GetNum(cmbEVT.Text, "EVT"))
                        cmd.Parameters.AddWithValue("@dist", gf_GetNum(cmbEVT.Text, "DIST"))
                        cmd.Parameters.AddWithValue("@targetEvt", strTargetEVT)
                        cmd.Parameters.AddWithValue("@targetDist", strTargetDIST)
                        cmd.Parameters.AddWithValue("@notes", noteText)
                        cmd.Parameters.AddWithValue("@notesSimple", noteSimple)

                        cmd.ExecuteNonQuery()
                    End Using

                    tx.Commit()
                End Using
            End Using

            Visible = False

        Catch ex As Exception
            MsgBox("Error in cmdCopyRule_MouseClick - " & ex.Message)
        End Try

    End Sub

    Private Sub cmdCopyCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopyCancel.Click
        Try
            Close()
        Catch ex As Exception
            MsgBox("Error in cmdCopyCancel_Click - " & ex.Message)
        End Try

    End Sub

    Private Sub rdoCopyEVT_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoCopyEVT.CheckedChanged
        Try
            If rdoCopyEVT.Checked = True Then
                cmbCopyEVT.Enabled = True
                grpEmptyAll.Enabled = False
            Else
                cmbCopyEVT.Enabled = False
                grpEmptyAll.Enabled = True
            End If
        Catch ex As Exception
            MsgBox("Error in rdoCopyEVT_CheckedChanged - " & ex.Message)
        End Try

    End Sub
End Class