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

    Private Sub cmdCopyRule_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdCopyRule.MouseClick
        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            Dim strPrompt As String
            Dim strTargetEVT As String
            Dim strTargetDIST As String
            Dim copyMU As String = cmbCopyMU.Text & "_Rulesets"

            'On and off rules or just on
            If rdoCopyOnOff.Checked = True Then
                strPrompt = "all ON and OFF rules " & vbCrLf
            Else
                strPrompt = "all ON rules " & vbCrLf
            End If

            'MU or EVT if MU then only where rulesets are empty or all rulesets
            If rdoCopyMU.Checked = True And rdoAll.Checked = True Then
                strPrompt = strPrompt & "in management unit " & cmbCopyMU.Text & vbCrLf &
                                        "where rules may already exist?"
            ElseIf rdoCopyMU.Checked = True And rdoAll.Checked = False Then
                strPrompt = strPrompt & "in management unit " & cmbCopyMU.Text & vbCrLf &
                                        "only where rulesets are EMPTY?"
            Else 'EVT only
                strPrompt = strPrompt & "from EVT " & cmbCopyEVT.Text & vbCrLf &
                                        "in management unit " & cmbCopyMU.Text & "?"
            End If

            If MsgBox("Are you sure you want to copy " & strPrompt, MsgBoxStyle.YesNo).Equals(vbYes) Then

                strTargetEVT = gf_GetNum(cmbCopyEVT.Text, "EVT")
                strTargetDIST = gf_GetNum(cmbCopyEVT.Text, "DIST")

                If rdoCopyEVT.Checked And rdoCopyOn.Checked Then
                    'Get the rules for the selected zone and EVT but only the rules that are turned On
                    strSQL = "INSERT INTO " & rulesR & " ( " &
                        "EVT, DIST, Cover_Low ,Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " &
                        "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                        "CBH13mx10, CBH40mx10, OnOff, Notes ) " &
                        "SELECT " & gf_GetNum(cmbEVT.Text, "EVT") & " AS EVT, " & gf_GetNum(cmbEVT.Text, "DIST") & " AS DIST, Cover_Low ,Cover_High, " &
                        "Height_Low, Height_High, BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, " &
                        "Canopy, CCover, CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, """ &
                        Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                        SN & ": Copied rule from MU - " & cmbCopyMU.Text &
                        " EVT " & strTargetEVT & "[" & strTargetDIST & "]"" AS Notes " &
                        "FROM " & cmbCopyMU.Text & "_Rulesets " &
                        "WHERE (EVT = " & strTargetEVT & ") " &
                        "And (DIST = " & strTargetDIST & ") " &
                        "And (OnOff = 'On') " &
                        "ORDER BY OnOff DESC, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, Height_Low, Height_High"
                ElseIf rdoCopyEVT.Checked And rdoCopyOnOff.Checked Then
                    'Get the rules for the selected zone and EVT
                    strSQL = "INSERT INTO " & rulesR & " ( " &
                        "EVT, DIST, Cover_Low ,Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " &
                        "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                        "CBH13mx10, CBH40mx10, OnOff, Notes ) " &
                        "SELECT " & gf_GetNum(cmbEVT.Text, "EVT") & " AS EVT, " & gf_GetNum(cmbEVT.Text, "DIST") & " AS DIST, Cover_Low ,Cover_High, " &
                        "Height_Low, Height_High, BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, " &
                        "Canopy, CCover, CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, """ &
                        Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                        SN & ": Copied rule from MU - " & cmbCopyMU.Text &
                        " EVT " & strTargetEVT & "[" & strTargetDIST & "]"" AS Notes " &
                        "FROM " & cmbCopyMU.Text & "_Rulesets " &
                        "WHERE (EVT = " & strTargetEVT & ") " &
                        "And (DIST = " & strTargetDIST & ") " &
                        "ORDER BY OnOff DESC, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, Height_Low, Height_High"
                ElseIf rdoCopyMU.Checked And rdoCopyOn.Checked And rdoAll.Checked Then
                    'Get the rules for the entire selected Zone and only the rules that are turned On
                    strSQL = "INSERT INTO " & rulesR & " ( " &
                        "EVT, DIST, Cover_Low ,Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " &
                        "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                        "CBH13mx10, CBH40mx10, OnOff, Notes ) " &
                        "SELECT EVT, DIST, Cover_Low ,Cover_High, " &
                        "Height_Low, Height_High, BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, " &
                        "Canopy, CCover, CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, """ &
                        Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                        SN & ": Copied rule from MU - " & cmbCopyMU.Text &
                        """ AS Notes " &
                        "FROM " & copyMU & " " &
                        "WHERE (OnOff = 'On') " &
                        "ORDER BY OnOff DESC, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, Height_Low, Height_High"
                ElseIf rdoCopyMU.Checked And rdoCopyOnOff.Checked And rdoAll.Checked Then
                    'Get the rules for the entire selected Zone
                    strSQL = "INSERT INTO " & rulesR & " ( " &
                        "EVT, DIST, Cover_Low ,Cover_High, Height_Low, Height_High, BPSRF, Wildcard, " &
                        "FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, CHeight, CBD13x100, CBD40x100, " &
                        "CBH13mx10, CBH40mx10, OnOff, Notes ) " &
                        "SELECT EVT, DIST, Cover_Low ,Cover_High, " &
                        "Height_Low, Height_High, BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, " &
                        "Canopy, CCover, CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, """ &
                        Now.ToShortTimeString & " " & Now.ToShortDateString & " " &
                        SN & ": Copied rule from MU - " & cmbCopyMU.Text &
                        """ AS Notes " &
                        "FROM " & copyMU & " " &
                        "ORDER BY OnOff DESC, BPSRF DESC, Wildcard DESC, Cover_Low, Cover_High, Height_Low, Height_High"
                ElseIf rdoCopyMU.Checked And rdoCopyOnOff.Checked And rdoEmpty.Checked Then
                    'Get rules On and Off for the entire selected Zone and insert in only where rulesets are empty
                    strSQL = "INSERT INTO " & rulesR & " ( EVT, DIST, Cover_Low, Cover_High, Height_Low, " &
                             "Height_High, BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, " &
                             "CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, Notes ) " &
                             "SELECT " & copyMU & ".EVT, " & copyMU & ".DIST, " & copyMU & ".Cover_Low, " & copyMU & ".Cover_High, " &
                             copyMU & ".Height_Low, " & copyMU & ".Height_High, " & copyMU & ".BPSRF, " & copyMU & ".Wildcard, " &
                             copyMU & ".FBFM13, " & copyMU & ".FBFM40, " & copyMU & ".CanFM, " & copyMU & ".FCCS, " &
                             copyMU & ".FLM, " & copyMU & ".Canopy, " & copyMU & ".CCover, " & copyMU & ".CHeight, " &
                             copyMU & ".CBD13x100, " & copyMU & ".CBD40x100, " & copyMU & ".CBH13mx10, " & copyMU & ".CBH40mx10, " &
                             copyMU & ".OnOff, """ & SN & ": Copied rule from MU - " & cmbCopyMU.Text & """ AS Notes " &
                             "FROM " & copyMU & " WHERE (((Exists (SELECT 1 FROM " & rulesR & " " &
                             "WHERE " & rulesR & ".EVT=" & copyMU & ".EVT AND " & copyMU & ".DIST=" & rulesR & ".DIST))=False)) " &
                             "ORDER BY " & copyMU & ".OnOff DESC , " & copyMU & ".BPSRF DESC , " & copyMU & ".Wildcard DESC , " &
                             copyMU & ".Cover_Low, " & copyMU & ".Cover_High, " & copyMU & ".Height_Low, " &
                             copyMU & ".Height_High;"
                ElseIf rdoCopyMU.Checked And rdoCopyOn.Checked And rdoEmpty.Checked Then
                    'Get only On rules for the entire selected Zone and insert in only where rulesets are empty
                    strSQL = "INSERT INTO " & rulesR & " ( EVT, DIST, Cover_Low, Cover_High, Height_Low, " &
                             "Height_High, BPSRF, Wildcard, FBFM13, FBFM40, CanFM, FCCS, FLM, Canopy, CCover, " &
                             "CHeight, CBD13x100, CBD40x100, CBH13mx10, CBH40mx10, OnOff, Notes ) " &
                             "SELECT " & copyMU & ".EVT, " & copyMU & ".DIST, " & copyMU & ".Cover_Low, " & copyMU & ".Cover_High, " &
                             copyMU & ".Height_Low, " & copyMU & ".Height_High, " & copyMU & ".BPSRF, " & copyMU & ".Wildcard, " &
                             copyMU & ".FBFM13, " & copyMU & ".FBFM40, " & copyMU & ".CanFM, " & copyMU & ".FCCS, " &
                             copyMU & ".FLM, " & copyMU & ".Canopy, " & copyMU & ".CCover, " & copyMU & ".CHeight, " &
                             copyMU & ".CBD13x100, " & copyMU & ".CBD40x100, " & copyMU & ".CBH13mx10, " & copyMU & ".CBH40mx10, " &
                             copyMU & ".OnOff, """ & SN & ": Copied rule from MU - " & cmbCopyMU.Text & """ AS Notes " &
                             "FROM " & copyMU & " WHERE (((" & copyMU & ".OnOff)='On') AND ((Exists (SELECT 1 FROM " & rulesR & " " &
                             "WHERE " & rulesR & ".EVT=" & copyMU & ".EVT AND " & copyMU & ".DIST=" & rulesR & ".DIST))=False)) " &
                             "ORDER BY " & copyMU & ".OnOff DESC , " & copyMU & ".BPSRF DESC , " & copyMU & ".Wildcard DESC , " &
                             copyMU & ".Cover_Low, " & copyMU & ".Cover_High, " & copyMU & ".Height_Low, " &
                             copyMU & ".Height_High;"
                End If
                dbconn.Execute(strSQL)                                                   'Run the SQL statement

                Visible = False
            End If
            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
        Catch ex As Exception
            If dbconn.State <> ConnectionState.Closed Then                                 'Database needs to be closed
                dbconn = Nothing
            End If
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