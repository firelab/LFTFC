Imports System.Data

Public Class clsRule
    Private varId, varEVT, varDist, varStrCovLow, varStrCovHigh, varStrHgtLow, varStrHgtHigh, varBPS, varWildcard,
        varOnOff, varNotes, varIntCovLow, varIntCovHigh, varIntHgtLow, varIntHgtHigh, varFBFM13, varFBFM40,
        varCanFM, varFCCS, varFLM, varCanopy, varCCover, varCHeight, varCBD13, varCBD40, varCBH13, varCBH40,
        varPixelCount, varAcres, varEVTPer As String
    Private strSQL As String
    Private comboR As String                                                'Stores the combo table name for rule making
    Private rulesR As String                                                'Stores the rules table name for rule making
    Private strProjectPath As String

    Public Sub New(ByVal Id As String, ByVal EVT As String, ByVal Dist As String, ByVal IntCovLow As String, ByVal IntCovHigh As String,
                   ByVal IntHgtLow As String, ByVal IntHgtHigh As String, ByVal BPS As String, ByVal Wildcard As String,
                   ByVal FBFM13 As String, ByVal FBFM40 As String, ByVal CanFM As String, ByVal FCCS As String,
                   ByVal FLM As String, ByVal Canopy As String, ByVal CCover As String, ByVal CHeight As String,
                   ByVal CBD13 As String, ByVal CBD40 As String, ByVal CBH13 As String, ByVal CBH40 As String,
                   ByVal OnOff As String, ByVal Notes As String, ByVal PixelCount As String, ByVal ComboTable As String,
                   ByVal RulesTable As String, ByRef EVTPixelCountCollection As Collection, ByVal ProjPath As String)
        'Assign values to object variables
        strProjectPath = ProjPath
        comboR = ComboTable
        rulesR = RulesTable
        varId = Id
        varEVT = EVT
        varDist = Dist
        varIntCovLow = IntCovLow
        varIntCovHigh = IntCovHigh
        varIntHgtLow = IntHgtLow
        varIntHgtHigh = IntHgtHigh
        If EVT > 99 Or varIntCovLow > 99 Then 'If less than 99 the values are water,rock,ag.....
            varStrCovLow = gf_ConvertCode(IntCovLow, "cov", "low", strProjectPath)
            varStrCovHigh = gf_ConvertCode(IntCovHigh, "cov", "high", strProjectPath)
            varStrHgtLow = gf_ConvertCode(IntHgtLow, "hgt", "low", strProjectPath)
            varStrHgtHigh = gf_ConvertCode(IntHgtHigh, "hgt", "high", strProjectPath)
        Else
            varStrCovLow = IntCovLow
            varStrCovHigh = IntCovHigh
            varStrHgtLow = IntHgtLow
            varStrHgtHigh = IntHgtHigh
        End If
        varBPS = BPS
        varWildcard = Wildcard
        varFBFM13 = FBFM13
        varFBFM40 = FBFM40
        varCanFM = CanFM
        varFCCS = FCCS
        varFLM = FLM
        varCanopy = Canopy
        varCCover = CCover
        varCHeight = CHeight
        varCBD13 = CBD13
        varCBD40 = CBD40
        varCBH13 = CBH13
        varCBH40 = CBH40
        varOnOff = OnOff
        varNotes = Notes
        If IsNumeric(PixelCount) = True Then 'Checks to see if the pixel count has already been calculated
            varPixelCount = PixelCount
            Acres = Math.Round(Int(Me.PixelCount) * 0.2223948, 2) 'Calculate Acres from pixel count

            'This covers Pixel count from a specific EVT,any BPS and any Wildcard values
            EvtPer = Math.Round(Int(Me.PixelCount) / Int(EVTPixelCountCollection.Item _
                (Me.EVT & Me.Dist)) * 100, 2) & "%" 'Calc % EVT from the total pixel count
        Else ' If pixel count has not already been calculated then it calculates it
            CalcPixels(Me.EVT, Me.Dist, EVTPixelCountCollection) 'Calculates pixels for the new rule
        End If
    End Sub

    Public Property Id() As String
        Get
            Id = varId
        End Get
        Set(ByVal value As String)
            varId = value
        End Set
    End Property

    Public ReadOnly Property EVT() As String
        Get
            EVT = Trim(varEVT)
        End Get
    End Property

    Public ReadOnly Property Dist() As String
        Get
            Dist = Trim(varDist)
        End Get
    End Property

    Public Property StrCovLow() As String
        Get
            StrCovLow = varStrCovLow
        End Get
        Set(ByVal value As String)
            varStrCovLow = value
            If IsNumeric(value) = True Then 'If code is < 100 it is rock water ....
                IntCovLow = Int(value)
            Else
                IntCovLow = gf_ConvertBack(value, strProjectPath)
            End If
        End Set
    End Property

    Public Property IntCovLow() As String
        Get
            IntCovLow = varIntCovLow
        End Get
        Set(ByVal value As String)
            varIntCovLow = value
            UpdateDB("Cover_Low", value)
        End Set
    End Property

    Public Property StrHgtLow() As String
        Get
            StrHgtLow = varStrHgtLow
        End Get
        Set(ByVal value As String)
            varStrHgtLow = value
            If IsNumeric(value) = True Then 'If code is < 100 it is rock water ....
                IntHgtLow = Int(value)
            Else
                IntHgtLow = gf_ConvertBack(value, strProjectPath)
            End If
        End Set
    End Property

    Public Property IntHgtLow() As String
        Get
            IntHgtLow = varIntHgtLow
        End Get
        Set(ByVal value As String)
            varIntHgtLow = value
            UpdateDB("Height_Low", value)
        End Set
    End Property

    Public Property StrCovHigh() As String
        Get
            StrCovHigh = varStrCovHigh
        End Get
        Set(ByVal value As String)
            varStrCovHigh = value
            If IsNumeric(value) = True Then 'If code is < 100 it is rock water ....
                IntCovHigh = Int(value)
            Else
                IntCovHigh = gf_ConvertBack(value, strProjectPath)
            End If
        End Set
    End Property

    Public Property IntCovHigh() As String
        Get
            IntCovHigh = varIntCovHigh
        End Get
        Set(ByVal value As String)
            varIntCovHigh = value
            UpdateDB("Cover_High", value)
        End Set
    End Property

    Public Property StrHgtHigh() As String
        Get
            StrHgtHigh = varStrHgtHigh
        End Get
        Set(ByVal value As String)
            varStrHgtHigh = value
            If IsNumeric(value) = True Then 'If code is < 100 it is rock water ....
                IntHgtHigh = Int(value)
            Else
                IntHgtHigh = gf_ConvertBack(value, strProjectPath)
            End If
        End Set
    End Property

    Public Property IntHgtHigh() As String
        Get
            IntHgtHigh = varIntHgtHigh
        End Get
        Set(ByVal value As String)
            varIntHgtHigh = value
            UpdateDB("Height_High", value)
        End Set
    End Property

    Public Property BPS()
        Get
            BPS = varBPS
        End Get
        Set(ByVal value)
            varBPS = value
            UpdateDB("BPSRF", value)
        End Set
    End Property

    Public Property Wildcard() As String
        Get
            Wildcard = varWildcard
        End Get
        Set(ByVal value As String)
            varWildcard = value
            UpdateDB("Wildcard", value)
        End Set
    End Property

    Public Property FBFM13() As String
        Get
            FBFM13 = varFBFM13
        End Get
        Set(ByVal value As String)
            varFBFM13 = value
            UpdateDB("FBFM13", value)
        End Set
    End Property

    Public Property FBFM40() As String
        Get
            FBFM40 = varFBFM40
        End Get
        Set(ByVal value As String)
            varFBFM40 = value
            UpdateDB("FBFM40", value)
        End Set
    End Property

    Public Property CanFM() As String
        Get
            CanFM = varCanFM
        End Get
        Set(ByVal value As String)
            varCanFM = value
            UpdateDB("CanFM", value)
        End Set
    End Property

    Public Property FCCS() As String
        Get
            FCCS = varFCCS
        End Get
        Set(ByVal value As String)
            varFCCS = value
            UpdateDB("FCCS", value)
        End Set
    End Property

    Public Property FLM() As String
        Get
            FLM = varFLM
        End Get
        Set(ByVal value As String)
            varFLM = value
            UpdateDB("FLM", value)
        End Set
    End Property

    Public Property Canopy() As String
        Get
            Canopy = varCanopy
        End Get
        Set(ByVal value As String)
            varCanopy = value
            UpdateDB("Canopy", value)
        End Set
    End Property

    Public Property CCover() As String
        Get
            CCover = varCCover
        End Get
        Set(ByVal value As String)
            varCCover = value
            UpdateDB("CCover", value)
        End Set
    End Property

    Public Property CHeight() As String
        Get
            CHeight = varCHeight
        End Get
        Set(ByVal value As String)
            varCHeight = value
            UpdateDB("CHeight", value)
        End Set
    End Property

    Public Property CBD13() As String
        Get
            CBD13 = varCBD13
        End Get
        Set(ByVal value As String)
            varCBD13 = value
            UpdateDB("CBD13x100", value)
        End Set
    End Property

    Public Property CBD40() As String
        Get
            CBD40 = varCBD40
        End Get
        Set(ByVal value As String)
            varCBD40 = value
            UpdateDB("CBD40x100", value)
        End Set
    End Property

    Public Property CBH13() As String
        Get
            CBH13 = varCBH13
        End Get
        Set(ByVal value As String)
            varCBH13 = value
            UpdateDB("CBH13mx10", value)
        End Set
    End Property

    Public Property CBH40() As String
        Get
            CBH40 = varCBH40
        End Get
        Set(ByVal value As String)
            varCBH40 = value
            UpdateDB("CBH40mx10", value)
        End Set
    End Property

    Public Property OnOff() As String
        Get
            OnOff = varOnOff
        End Get
        Set(ByVal value As String)
            varOnOff = value
            UpdateDB("OnOff", value)
        End Set
    End Property

    Public Property Notes() As String
        Get
            Notes = varNotes
        End Get
        Set(ByVal value As String)
            varNotes = value
            UpdateDB("Notes", value)
        End Set
    End Property

    Public Property PixelCount() As String
        Get
            PixelCount = varPixelCount
        End Get
        Set(ByVal value As String)
            varPixelCount = value
            UpdateDB("PixelCount", value)
        End Set
    End Property

    Public Property Acres() As String
        Get
            Acres = varAcres
        End Get
        Set(ByVal value As String)
            varAcres = value
        End Set
    End Property

    Public Property EvtPer() As String
        Get
            EvtPer = varEVTPer
        End Get
        Set(ByVal value As String)
            varEVTPer = value
        End Set
    End Property

    Public Sub UpdateDB(ByVal strField As String, ByVal strValue As String)             'Updates DB with a single value
        Dim dbconn As New ADODB.Connection                                              'DB connection
        Try
            dbconn.ConnectionString = gs_DBConnection &
            strProjectPath & "\" & gs_LFTFCDBName
            dbconn.Open()

            strSQL = "Update " & rulesR & " " &
                     "SET " & strField & " = '" & strValue & "'" &
                     " WHERE Id = " & Id
            dbconn.Execute(strSQL)

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

        Catch ex As Exception

            If dbconn.State <> ConnectionState.Closed Then dbconn.Close() 'Database needs to be closed
            dbconn = Nothing

            MsgBox("Error in UpdateDB - " & ex.Message)
        End Try
    End Sub

    Public Sub CalcPixels(ByVal EVTNum As String, ByVal DistNum As String, ByRef EVTPixelCountCollection As Collection)
        Dim rs1 As New ADODB.Recordset                                  'recordset for data

        Dim dbconn As New ADODB.Connection                              'DB connection
        dbconn.ConnectionString = gs_DBConnection &
        strProjectPath & "\" & gs_LFTFCDBName
        dbconn.Open()

        Try
            If OnOff = "On" Then 'Calculate pixels but leave empty if "Off"
                'Declare variables
                Dim strTmpCnt As String 'Stores Temp pixel count

                'Assign Variables
                'This SQL gets all values and count from CMB table that match the rule
                If IsNumeric(BPS) Then 'Me.BPS is a number and does not equal "any"
                    strSQL = "SELECT EVTR, DIST, SUM(COUNT) AS SumOfCount FROM " & comboR & " WHERE " &
                        "(EVTR = " & EVTNum & " And " &
                        "DIST = " & DistNum & " And " &
                        "EVCR Between " & IntCovLow & " And " & IntCovHigh & " And " &
                        "EVHR Between " & IntHgtLow & " And " & IntHgtHigh & " And " &
                        "BPSRF = " & BPS & " And Wildcard = '" & Wildcard & "')" &
                        " Or " &
                        "(EVTR = " & EVTNum & " And " &
                        "DIST = " & DistNum & " And " &
                        "EVCR Between " & IntCovLow & " And " & IntCovHigh & " And " &
                        "EVHR Between " & IntHgtLow & " And " & IntHgtHigh & " And " &
                        "BPSRF = " & BPS & " And '" & Wildcard & "' = 'any')" &
                        " GROUP BY EVTR, DIST"
                Else 'Me. BPS is a string and equals "any"
                    strSQL = "SELECT EVTR,SUM(COUNT) AS SumOfCount FROM " & comboR & " WHERE " &
                        "(EVTR = " & EVTNum & " And " &
                        "DIST = " & DistNum & " And " &
                        "EVCR Between " & IntCovLow & " And " & IntCovHigh & " And " &
                        "EVHR Between " & IntHgtLow & " And " & IntHgtHigh & " And '" &
                        BPS & "' = 'any' And Wildcard = '" & Wildcard & "')" &
                        " Or " &
                        "(EVTR = " & EVTNum & " And " &
                        "DIST = " & DistNum & " And " &
                        "EVCR Between " & IntCovLow & " And " & IntCovHigh & " And " &
                        "EVHR Between " & IntHgtLow & " And " & IntHgtHigh & " And '" &
                        BPS & "' = 'any' And '" & Wildcard & "' = 'any')" &
                        " GROUP BY EVTR, DIST"
                End If
                rs1.Open(strSQL, dbconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

                'Checks to see if Pixel count is null if it is the pixel count gets a zero or EOF
                If IsNothing(rs1.Fields!SumOfCount) = False And rs1.EOF = False Then
                    strTmpCnt = rs1.Fields!SumOfCount.Value & ""

                    PixelCount = strTmpCnt 'Set the count of the pixels
                Else
                    PixelCount = 0
                End If

                CalcAcresAndPercent(EVTPixelCountCollection) 'Calculates acres and percent evt
            Else 'Clear the pixel count and acres because it is turned off
                PixelCount = ""
                Acres = ""
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

            MsgBox("Error in CalcPixels - " & ex.Message)
        End Try
    End Sub

    Public Sub CalcAcresAndPercent(ByRef EVTPixelCountCollection As Collection)
        Acres = Math.Round(Int(PixelCount) * 0.2223948, 2) 'Calculate Acres from pixel count

        'Calc % EVT from the selected EVT, any BPS, any Wildcard and the total pixel count
        EvtPer = Math.Round(Int(PixelCount) / Int(EVTPixelCountCollection.Item(EVT & Dist)) * 100, 2) & "%"
    End Sub
End Class
