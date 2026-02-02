Public Class clsFM
    Private strDataType As String 'English or Metric
    Private strCreator As String 'Anderson13, ScottAndBurgan40, Nonburnable, Custom
    Private intFMNum As Integer 'FMNum Fuel Model Number integer number 14-89
    Private strFMCode As String 'Up to 7 characters
    Private dblFL1H, dblFL10H, dblFL100H, dblFLLiveH, dblFLLiveW As Double '1H, 10H, 100H, LiveH, LiveW Fuel Loading decimal tons/acre metric tonnes/hectare
    Private strFMType As String 'FMType Fuel Model Type string "static" or "dynamic"
    Private intH1SAV, intLiveHSAV, intLiveWSAV As Integer '1HSAV, LiveHSAV, LiveWSAV Surface to Volume Ratio integer 1/ft 1/cm
    Private dblDepth As Double 'Depth Fuel Bed Depth decimal ft cm
    Private intXtMoist As Integer 'XtMoist Moisture of Extinction integer percent
    Private intDHt, intLHt As Integer 'DHt, LHt Heat Content, live & dead fuels integer BTU/lb J/Kg
    Private strFMName As String 'FMName Fuel Model Name string user defined up to 256 characters

    Dim lngSLOP As Long 'Slope
    Dim lngOWND As Long 'Open wind speed used as Midflame wind speed because there is no cover

    Private arrCalcsI(8, 21) As Double 'Stores FM calculations

    '***Column numbers arrCalcsI***
    Private lngWTA As Long 'Stores the weight ton/acre
    Private lngWTr As Long 'Stores the weight transfered
    Private lngWAd As Long 'Stores the weight adjusted
    Private lngWAC As Long 'Stores the weight adjusted converted to lb/ft^2
    Private lngSAV As Long 'Stores the surface area/volume ratio
    Private lngSA As Long 'Stores the surface area
    Private lngWF As Long  'Stores the weight factor
    Private lngEWF As Long 'Stores the exponential weighting factor
    Private lngEWF2 As Long 'Stores the exponential weighting factor 2
    Private lngMC As Long  'Stores the fuel moisture Content
    Private lngQig As Long 'Stores the Qig OR Heat of preignition, kJ kg^-1
    Private lngWn As Long  'Stores the Wn OR weight with the mineral content removed, kg m^-2
    Private lngSWF1 As Long 'Stores the subclass weighting factor 1
    Private lngSWF2 As Long 'Stores the subclass weighting factor 2
    Private lngSWF3 As Long 'Stores the subclass weighting factor 3
    Private lngSWF4 As Long 'Stores the subclass weighting factor 4
    Private lngSWF5 As Long 'Stores the subclass weighting factor 5
    Private lngSNL1 As Long 'Stores the subclass net loading 1
    Private lngSNL2 As Long 'Stores the subclass net loading 2
    Private lngSNL3 As Long 'Stores the subclass net loading 3
    Private lngSNL4 As Long 'Stores the subclass net loading 4
    Private lngSNL5 As Long 'Stores the subclass net loading 5
    '***Row numbers arrCalcsI***
    Private lngDH As Long 'Stores the dead herbaceous
    Private lngD1 As Long 'Stores the dead 1-hr
    Private lngD10 As Long 'Stores the dead 10-hr
    Private lngD100 As Long 'Stores the dead 100-hr
    Private lngDT As Long 'Stores the dead total
    Private lngLH As Long 'Stores the live herbaceous
    Private lngLW As Long 'Stores the live woody
    Private lngLT As Long 'Stores the live total
    Private lngTL As Long 'Stores the total loading

    Public Sub New(ByVal DataType As String, ByVal FMNum As Integer, ByVal FMCode As String, ByVal FL1H As Double, _
                   ByVal FL10H As Double, ByVal FL100H As Double, ByVal FLLiveH As Double, ByVal FLLiveW As Double, _
                   ByVal FMType As String, ByVal H1SAV As Integer, ByVal LiveHSAV As Integer, ByVal LiveWSAV As Integer, _
                   ByVal Depth As Double, ByVal XtMoist As Integer, ByVal DHt As Integer, _
                   ByVal LHt As Integer, ByVal FMName As String, ByVal Creator As String)
        strDataType = DataType
        intFMNum = FMNum
        strFMCode = FMCode
        dblFL1H = FL1H
        dblFL10H = FL10H
        dblFL100H = FL100H
        dblFLLiveH = FLLiveH
        dblFLLiveW = FLLiveW
        strFMType = FMType
        intH1SAV = H1SAV
        intLiveHSAV = LiveHSAV
        intLiveWSAV = LiveWSAV
        dblDepth = Depth
        intXtMoist = XtMoist
        intDHt = DHt
        intLHt = LHt
        strFMName = FMName
        strCreator = Creator
    End Sub
    Public Property DataType() As String
        Get
            DataType = strDataType
        End Get
        Set(ByVal value As String)
            strDataType = DataType
        End Set
    End Property

    Public Property FMNum() As Integer
        Get
            FMNum = intFMNum
        End Get
        Set(ByVal value As Integer)
            intFMNum = FMNum
        End Set
    End Property

    Public Property FMCode() As String
        Get
            FMCode = strFMCode
        End Get
        Set(ByVal value As String)
            strFMCode = FMCode
        End Set
    End Property

    Public Property FL1H() As Double
        Get
            FL1H = dblFL1H
        End Get
        Set(ByVal value As Double)
            dblFL1H = FL1H
        End Set
    End Property

    Public Property FL10H() As Double
        Get
            FL10H = dblFL10H
        End Get
        Set(ByVal value As Double)
            dblFL10H = FL10H
        End Set
    End Property

    Public Property FL100H() As Double
        Get
            FL100H = dblFL100H
        End Get
        Set(ByVal value As Double)
            dblFL100H = FL100H
        End Set
    End Property

    Public Property FLLiveH() As Double
        Get
            FLLiveH = dblFLLiveH
        End Get
        Set(ByVal value As Double)
            dblFLLiveH = FLLiveH
        End Set
    End Property

    Public Property FLLiveW() As Double
        Get
            FLLiveW = dblFLLiveW
        End Get
        Set(ByVal value As Double)
            dblFLLiveW = FLLiveW
        End Set
    End Property

    Public Property FMType() As String
        Get
            FMType = strFMType
        End Get
        Set(ByVal value As String)
            strFMType = FMType
        End Set
    End Property

    Public Property H1SAV() As Integer
        Get
            H1SAV = intH1SAV
        End Get
        Set(ByVal value As Integer)
            intH1SAV = H1SAV
        End Set
    End Property

    Public Property LiveHSAV() As Integer
        Get
            LiveHSAV = intLiveHSAV
        End Get
        Set(ByVal value As Integer)
            intLiveHSAV = LiveHSAV
        End Set
    End Property

    Public Property LiveWSAV() As Integer
        Get
            LiveWSAV = intLiveWSAV
        End Get
        Set(ByVal value As Integer)
            intLiveWSAV = LiveWSAV
        End Set
    End Property

    Public Property Depth() As Double
        Get
            Depth = dblDepth
        End Get
        Set(ByVal value As Double)
            dblDepth = Depth
        End Set
    End Property

    Public Property XtMoist() As Integer
        Get
            XtMoist = intXtMoist
        End Get
        Set(ByVal value As Integer)
            intXtMoist = XtMoist
        End Set
    End Property

    Public Property DHt() As Integer
        Get
            DHt = intDHt
        End Get
        Set(ByVal value As Integer)
            intDHt = DHt
        End Set
    End Property

    Public Property LHt() As Integer
        Get
            LHt = intLHt
        End Get
        Set(ByVal value As Integer)
            intLHt = LHt
        End Set
    End Property

    Public Property FMName() As String
        Get
            FMName = strFMName
        End Get
        Set(ByVal value As String)
            strFMName = FMName
        End Set
    End Property

    Public Property Creator() As String
        Get
            Creator = strCreator
        End Get
        Set(ByVal value As String)
            strCreator = Creator
        End Set
    End Property

    Public Function CalcCBH(ByVal inSLOPE As Long, ByVal inMFWS As Long, ByVal inMC01 As Long, _
                            ByVal inMC10 As Long, ByVal inMC100 As Long, ByVal inMCLH As Long, _
                            ByVal inMCLW As Long) As Double
        Const lngFOMC As Long = 100 'Foliar Moisture content is a constant at 100

        lngSLOP = inSLOPE
        lngOWND = inMFWS

        CalcsI(inMC01, inMC10, inMC100, inMCLH, inMCLW, lngFOMC)

        'Convert back to m/min from chains/hr (GetHROS()/60*66/3.2808399)
        CalcCBH = (100 * (11.349 * (GetHROS() / 60 * 66 / 3.2808399) * GetHPUA() / 60) ^ (2 / 3)) / (460 + 25.9 * lngFOMC)
    End Function

    Public Function CalcFL(ByVal inSLOPE As Long, ByVal inMFWS As Long, ByVal inMC01 As Long, _
                            ByVal inMC10 As Long, ByVal inMC100 As Long, ByVal inMCLH As Long, _
                            ByVal inMCLW As Long) As Double

        Const lngFOMC As Long = 100 'Foliar Moisture content is a constant at 100
        lngSLOP = inSLOPE
        lngOWND = inMFWS

        CalcsI(inMC01, inMC10, inMC100, inMCLH, inMCLW, lngFOMC)

        CalcFL = (0.45 * GetFLIN() ^ 0.46)
    End Function

    Public Function CalcROS(ByVal inSLOPE As Long, ByVal inMFWS As Long, ByVal inMC01 As Long, _
                            ByVal inMC10 As Long, ByVal inMC100 As Long, ByVal inMCLH As Long, _
                            ByVal inMCLW As Long) As Double 'Headfire rate of spread m/min

        Const lngFOMC As Long = 100 'Foliar Moisture content is a constant at 100
        lngSLOP = inSLOPE
        lngOWND = inMFWS

        CalcsI(inMC01, inMC10, inMC100, inMCLH, inMCLW, lngFOMC)

        CalcROS = GetHROS()
    End Function

    Private Function GetHROS() As Double 'Headfire rate of spread m/min
        If GetMRAT() > 1 Then
            GetHROS = 0
        Else
            GetHROS = GetNWNS() * (1 + GetWDSF()) * 2.98257
        End If
    End Function

    Private Function GetFLIN() As Double 'Fireline intensity
        GetFLIN = GetHROS() * 0.0183 * GetHPUA()
    End Function

    Private Function GetHPUA() As Double 'Heat Per Unit Area
        GetHPUA = GetRINT() * GetRSTM()
    End Function

    Private Function GetRSTM() As Double 'Residence time
        Const FLIM As Integer = 1 'Constant Flame Length Intensity Modifier
        GetRSTM = 384 / GetSigma() * FLIM
    End Function


    Private Function GetMRAT() As Double 'Moisture ratio low
        GetMRAT = GetMCDF() / GetMCEX()
    End Function

    Private Function GetNWNS() As Double 'Assume as no windspeed rate of spread m/min low
        'Declare variables
        Const lngROSM As Long = 1 'Rate of spread modifier low constant at 1
        GetNWNS = (GetRINT() * GetPFR() * lngROSM) / (GetBDEN() * GetEQig() * 3.281)
    End Function

    Private Function GetWDSF() As Double 'Windspeed slope factor low
        'Declare variables
        Const dblDeg2Pi As Double = 57.29578 'Degrees to pi
        Const ConstWDIR As Long = 0 'Always upslope
        GetWDSF = ((GetWNDF() * Math.Sin(ConstWDIR / dblDeg2Pi)) ^ 2 + _
                  ((GetWNDF() * Math.Cos(ConstWDIR / dblDeg2Pi) + GetSLPF())) ^ 2) ^ 0.5
    End Function

    Private Function GetRINT() As Double 'Reaction intensity low
        GetRINT = GetORV() * GetHEAT() * GetENES() * (GetMDCF() * GetDTWn() + GetLTWn() * GetMDCL())
    End Function

    Private Function GetPFR() As Double 'Propogating flux ratio low
        GetPFR = (((192 + 0.2595 * GetSigma()) ^ -1) *
                  (Math.Exp((0.792 + 0.681 * GetSigma() ^ 0.5) * (GetPRAT() + 0.1))))
    End Function

    Private Function GetBDEN() As Double 'Bulk density low
        GetBDEN = GetLOAD() / GetFDEP()
    End Function

    Private Function GetEQig() As Double 'Eponential weight factor heat of preignition, kJ kg^-1 low
        GetEQig = GetDFRC() * GetDWFEWFQig() + GetLFRC() * GetLWFEWFQig()
    End Function

    'Private Function GetWNDF() As Double 'Wind factor low
    '    'Declare variables
    '    Const lngWNDUnits As Long = 88 'Constant for miles/hour
    '    If GetWNDFMax() < GetC() * (lngWNDUnits * GetWNDR() * lngOWND) ^ GetB() * GetBBOP() ^ -GetE() Then
    '        GetWNDF = GetWNDFMax()
    '    Else
    '        GetWNDF = GetC() * (lngWNDUnits * GetWNDR() * lngOWND) ^ GetB() * GetBBOP() ^ -GetE()
    '    End If
    'End Function

    Private Function GetWNDF() As Double 'Wind factor low
        'Declare variables
        Const lngWNDUnits As Long = 88 'Constant for miles/hour
        If GetWNDFMax() < GetC() * (lngWNDUnits * lngOWND) ^ GetB() * GetBBOP() ^ -GetE() Then
            GetWNDF = GetWNDFMax()
        Else
            GetWNDF = GetC() * (lngWNDUnits * lngOWND) ^ GetB() * GetBBOP() ^ -GetE()
        End If
    End Function

    Private Function GetSLPF() As Double 'Slope factor
        GetSLPF = 5.275 * GetPRAT() ^ -0.3 * (lngSLOP / 100) ^ 2
    End Function

    Private Function GetORV() As Double 'Optimum reation velocity
        GetORV = (GetVMAX() * GetBBOP() ^ GetA() * Math.Exp(GetA() * (1 - GetBBOP())))
    End Function

    Private Function GetHEAT() As Double 'Heat content low
        GetHEAT = GetDFHC() * GetDFRC() + GetLFHC() * GetLFRC()
    End Function

    Private Function GetENES() As Double 'Mineral damping coeficient low
        'Declare variables
        Const dblEMFR As Double = 0.01 'Effective mineral fraction canstant at 0.01

        GetENES = 0.174 * dblEMFR ^ -0.19
    End Function

    Private Function GetMDCF() As Double 'Moisture dampening coefficient
        GetMDCF = 1 - 2.59 * GetMRAT() + 5.11 * GetMRAT() ^ 2 - 3.52 * GetMRAT() ^ 3
    End Function

    Private Function GetMDCL() As Double 'Moisture dampening coeffiecient live
        GetMDCL = 1 - 2.59 * GetLRAT() + 5.11 * GetLRAT() ^ 2 - 3.52 * GetLRAT() ^ 3
    End Function

    Private Function GetPRAT() As Double 'Packing ration low
        'Declare Variables
        Const lngPDEN As Long = 32    'Particle density lbs/ft^3 constant at 32

        GetPRAT = GetBDEN() / lngPDEN
    End Function

    Private Function GetWNDFMax() As Double 'Wind factor maximum
        GetWNDFMax = GetC() * GetWNDLim() ^ GetB() * GetBBOP() ^ -GetE()
    End Function

    Private Function GetA() As Double 'Term in Rothermel's (1972) model,all function of sigma
        GetA = 133 * GetSigma() ^ -0.7913
    End Function

    Private Function GetB() As Double 'Term in Rothermel's (1972) model,all function of sigma
        GetB = (0.02526 * GetSigma() ^ 0.54)
    End Function

    Private Function GetC() As Double 'Term in Rothermel's (1972) model,all function of sigma
        GetC = (7.47 * Math.Exp(-0.133 * GetSigma() ^ 0.55))
    End Function

    Private Function GetE() As Double 'Term in Rothermel's (1972) model,all function of sigma
        GetE = (0.715 * Math.Exp(-0.000359 * GetSigma()))
    End Function

    Private Function GetBBOP() As Double 'Relative packing ration, packing ratio / optimum packing ratio
        GetBBOP = GetPRAT() / GetBOPT()
    End Function

    'Private Function GetWNDR() As Double 'Wind reduction factor
    '    'Use no cover and no height
    '    GetWNDR = GetWNDNoCover()
    'End Function

    Private Function GetVMAX() As Double 'Velocity max
        GetVMAX = GetSigma() ^ 1.5 * (495 + 0.0594 * GetSigma() ^ 1.5) ^ -1
    End Function

    Private Function GetLRAT() As Double 'Live Moisture ratio low
        If GetLTFM() / 100 / GetMxl() < 1 Then
            GetLRAT = GetLTFM() / 100 / GetMxl()
        Else
            GetLRAT = 1
        End If
    End Function

    Private Function GetWNDLim() As Double 'Wind limit (max MFWS, ft/min)
        GetWNDLim = GetRINT() * 0.9
    End Function

    Private Function GetBOPT() As Double 'Bopt Optimum packing ration
        GetBOPT = 3.348 * GetSigma() ^ -0.8189
    End Function

    Private Function GetWNDNoCover() As Double 'Wind without cover
        'Declare variables
        Const lngFLEXratio As Long = 1 'Flame extension ratio constant at 1
        GetWNDNoCover = (1 + 0.36 / lngFLEXratio) / GetLN((20 + 0.36 * GetFDEP()) / (0.13 * GetFDEP())) _
                       * (GetLN((lngFLEXratio + 0.36) / 0.13) - 1)
    End Function

    Private Function GetMxl() As Double 'Mxl
        If GetMxl1() > GetMCEX() Then
            GetMxl = GetMxl1()
        Else
            GetMxl = GetMCEX()
        End If
    End Function

    Private Function GetMxl1() As Double 'Mxl1 Live fuel moisture of extinction
        GetMxl1 = (2.9 * GetWPrime() * (1 - GetMPrimeDead() / GetMCEX()) - 0.226)
    End Function

    Private Function GetWPrime() As Double 'WPrime Dead live load ratio
        If GetLTWAC() = 0 Then
            GetWPrime = 0
        Else
            GetWPrime = GetDWACDEWF() / GetLWACLEWF()
        End If
    End Function

    Private Function GetMPrimeDead() 'MPrimeDead "Fine" dead fuel moisture (Mf,dead)
        GetMPrimeDead = GetDEWFDWnDMC() / (100 * GetDEWFDWn())
    End Function

    Private Function GetLN(ByVal x As Double) As Double
        Const E As Double = 2.718282 'The base e constant
        GetLN = Math.Log(x) / Math.Log(E) 'Convert Log to Natural Log Ln
    End Function

    Private Sub CalcsI(ByVal lngMC01, ByVal lngMC10, ByVal lngMC100, ByVal lngMCLH, ByVal lngMCLW, ByVal lngFOMC)

        '***Column arrCalcsI***
        lngWTA = 0
        lngWTr = 1
        lngWAd = 2
        lngWAC = 3
        lngSAV = 4
        lngSA = 5
        lngWF = 6
        lngEWF = 7
        lngEWF2 = 8
        lngMC = 9
        lngQig = 10
        lngWn = 11
        lngSWF1 = 12
        lngSWF2 = 13
        lngSWF3 = 14
        lngSWF4 = 15
        lngSWF5 = 16
        lngSNL1 = 17
        lngSNL2 = 18
        lngSNL3 = 19
        lngSNL4 = 20
        lngSNL5 = 21
        '***Rows arrCalcsI***
        lngDH = 0
        lngD1 = 1
        lngD10 = 2
        lngD100 = 3
        lngDT = 4
        lngLH = 5
        lngLW = 6
        lngLT = 7
        lngTL = 8

        'Clear the array
        Array.Clear(arrCalcsI, 8, 21)

        'Fill array:
        '****Dead Fuel****
        arrCalcsI(lngDH, lngWTA) = 0 'Herbaceous is always empty
        arrCalcsI(lngD1, lngWTA) = FL1H '1-hr
        arrCalcsI(lngD10, lngWTA) = FL10H '10 - hr
        arrCalcsI(lngD100, lngWTA) = FL100H '100 - hr
        arrCalcsI(lngDT, lngWTA) = arrCalcsI(lngD1, lngWTA) + arrCalcsI(lngD10, lngWTA) _
                                    + arrCalcsI(lngD100, lngWTA) 'dead total

        '****Live Fuel****
        arrCalcsI(lngLH, lngWTA) = FLLiveH 'Herbaceous
        arrCalcsI(lngLW, lngWTA) = FLLiveW 'Woody
        arrCalcsI(lngLT, lngWTA) = arrCalcsI(lngLH, lngWTA) + arrCalcsI(lngLW, lngWTA) 'live total

        '****Total Fuel****
        arrCalcsI(lngTL, lngWTA) = arrCalcsI(lngDT, lngWTA) + arrCalcsI(lngLT, lngWTA) ' Total

        '****Load Transfered Per Dynamic Fuel Modeling****
        If FMType = "dynamic" Then 'Live Herbaceous is multipled by calced % cured
            If (-0.0111 * lngMCLH + 1.333) > 0 And (-0.0111 * lngMCLH + 1.333) < 1 Then
                'Percent cured is < 100% some live herb is transfered
                arrCalcsI(lngDH, lngWTr) = arrCalcsI(lngLH, lngWTA) * (-0.0111 * lngMCLH + 1.333)
            ElseIf (-0.0111 * lngMCLH + 1.333) >= 1 Then
                'Percent Cured is > 100% so all live herb is transfered to dead fuel
                arrCalcsI(lngDH, lngWTr) = arrCalcsI(lngLH, lngWTA)
            End If
        Else
            'Percent cured is < 0% no live herb is transfered OR it is not dynamic fuel model
            arrCalcsI(0, 1) = 0
        End If
        arrCalcsI(lngD1, lngWTr) = arrCalcsI(lngD1, lngWTA)                            '1-hr dead fuel same value
        arrCalcsI(lngD10, lngWTr) = arrCalcsI(lngD10, lngWTA)                         '10-hr dead fuel same value
        arrCalcsI(lngD100, lngWTr) = arrCalcsI(lngD100, lngWTA)                       '100-hr dead fuel same value
        arrCalcsI(lngDT, lngWTr) = arrCalcsI(lngDH, lngWTr) + arrCalcsI(lngD1, lngWTr) + arrCalcsI(lngD10, lngWTr) _
                                + arrCalcsI(lngD100, lngWTr)                           'New tot dead with transfered weight
        arrCalcsI(lngLH, lngWTr) = arrCalcsI(lngLH, lngWTA) - arrCalcsI(lngDH, lngWTr) 'New live herb load
        arrCalcsI(lngLW, lngWTr) = arrCalcsI(lngLW, lngWTA)                           'Woody loading same value
        arrCalcsI(lngLT, lngWTr) = arrCalcsI(lngLH, lngWTr) + arrCalcsI(lngLW, lngWTr) 'New live total
        arrCalcsI(lngTL, lngWTr) = arrCalcsI(lngDT, lngWTr) + arrCalcsI(lngLT, lngWTr) 'New total loading

        '****Weight adjusted for load and depth multiplier****
        'Weight multiplier is always 1 so the values are the same as load transfered per dynamic fuel modeling
        arrCalcsI(lngDH, lngWAd) = arrCalcsI(lngDH, lngWTr)
        arrCalcsI(lngD1, lngWAd) = arrCalcsI(lngD1, lngWTr)
        arrCalcsI(lngD10, lngWAd) = arrCalcsI(lngD10, lngWTr)
        arrCalcsI(lngD100, lngWAd) = arrCalcsI(lngD100, lngWTr)
        arrCalcsI(lngDT, lngWAd) = arrCalcsI(lngDT, lngWTr)
        arrCalcsI(lngLH, lngWAd) = arrCalcsI(lngLH, lngWTr)
        arrCalcsI(lngLW, lngWAd) = arrCalcsI(lngLW, lngWTr)
        arrCalcsI(lngLT, lngWAd) = arrCalcsI(lngLT, lngWTr)
        arrCalcsI(lngTL, lngWAd) = arrCalcsI(lngTL, lngWTr)

        '****Weight adjusted for load and depth multiplier converted to lb/ft^2****
        arrCalcsI(lngDH, lngWAC) = arrCalcsI(lngDH, lngWAd) * 0.04591
        arrCalcsI(lngD1, lngWAC) = arrCalcsI(lngD1, lngWAd) * 0.04591
        arrCalcsI(lngD10, lngWAC) = arrCalcsI(lngD10, lngWAd) * 0.04591
        arrCalcsI(lngD100, lngWAC) = arrCalcsI(lngD100, lngWAd) * 0.04591
        arrCalcsI(lngDT, lngWAC) = arrCalcsI(lngDT, lngWAd) * 0.04591
        arrCalcsI(lngLH, lngWAC) = arrCalcsI(lngLH, lngWAd) * 0.04591
        arrCalcsI(lngLW, lngWAC) = arrCalcsI(lngLW, lngWAd) * 0.04591
        arrCalcsI(lngLT, lngWAC) = arrCalcsI(lngLT, lngWAd) * 0.04591
        arrCalcsI(lngTL, lngWAC) = arrCalcsI(lngTL, lngWAd) * 0.04591

        '****Surface area to volume ratios****
        arrCalcsI(lngDH, lngSAV) = LiveHSAV 'Surface area/vol ratio dead herbaceous same as live
        arrCalcsI(lngD1, lngSAV) = H1SAV    'Surface area/vol ratio dead 1-hr
        arrCalcsI(lngD10, lngSAV) = 109     'Surface area/vol ratio dead 10-hr constant
        arrCalcsI(lngD100, lngSAV) = 30     'Surface area/vol ratio dead 100-hr constant
        arrCalcsI(lngLH, lngSAV) = LiveHSAV 'Surface area/vol ratio for live herbaceous same as dead
        arrCalcsI(lngLW, lngSAV) = LiveWSAV 'Surface area/vol ratio for live woody
        '****Surface area and weight factor need to be calculated before totaling dead,live, and total load****
        'Surface Area
        arrCalcsI(lngDH, lngSA) = arrCalcsI(lngDH, lngWAC) * arrCalcsI(lngDH, lngSAV)
        arrCalcsI(lngD1, lngSA) = arrCalcsI(lngD1, lngWAC) * arrCalcsI(lngD1, lngSAV)
        arrCalcsI(lngD10, lngSA) = arrCalcsI(lngD10, lngWAC) * arrCalcsI(lngD10, lngSAV)
        arrCalcsI(lngD100, lngSA) = arrCalcsI(lngD100, lngWAC) * arrCalcsI(lngD100, lngSAV)
        arrCalcsI(lngDT, lngSA) = arrCalcsI(lngDH, lngSA) + arrCalcsI(lngD1, lngSA) _
                                + arrCalcsI(lngD10, lngSA) + arrCalcsI(lngD100, lngSA)
        arrCalcsI(lngLH, lngSA) = arrCalcsI(lngLH, lngWAC) * arrCalcsI(lngLH, lngSAV)
        arrCalcsI(lngLW, lngSA) = arrCalcsI(lngLW, lngWAC) * arrCalcsI(lngLW, lngSAV)
        arrCalcsI(lngLT, lngSA) = arrCalcsI(lngLH, lngSA) + arrCalcsI(lngLW, lngSA)
        'Weight Factor
        arrCalcsI(lngDH, lngWF) = arrCalcsI(lngDH, lngSA) / arrCalcsI(lngDT, lngSA)
        arrCalcsI(lngD1, lngWF) = arrCalcsI(lngD1, lngSA) / arrCalcsI(lngDT, lngSA)
        arrCalcsI(lngD10, lngWF) = arrCalcsI(lngD10, lngSA) / arrCalcsI(lngDT, lngSA)
        arrCalcsI(lngD100, lngWF) = arrCalcsI(lngD100, lngSA) / arrCalcsI(lngDT, lngSA)
        arrCalcsI(lngDT, lngWF) = arrCalcsI(lngDT, lngSA) / (arrCalcsI(lngDT, lngSA) + arrCalcsI(lngLT, lngSA))
        If arrCalcsI(lngLH, lngSA) = 0 And arrCalcsI(lngLT, lngSA) = 0 Then
            arrCalcsI(lngLH, lngWF) = 0
        Else
            arrCalcsI(lngLH, lngWF) = arrCalcsI(lngLH, lngSA) / arrCalcsI(lngLT, lngSA)
        End If
        If arrCalcsI(lngLW, lngSA) = 0 And arrCalcsI(lngLT, lngSA) = 0 Then
            arrCalcsI(lngLW, lngWF) = 0
        Else
            arrCalcsI(lngLW, lngWF) = arrCalcsI(lngLW, lngSA) / arrCalcsI(lngLT, lngSA)
        End If
        arrCalcsI(lngLT, lngWF) = arrCalcsI(lngLT, lngSA) / (arrCalcsI(lngDT, lngSA) + arrCalcsI(lngLT, lngSA))
        arrCalcsI(lngDT, lngSAV) = arrCalcsI(lngDH, lngSAV) * arrCalcsI(lngDH, lngWF) _
                                 + arrCalcsI(lngD1, lngSAV) * arrCalcsI(lngD1, lngWF) _
                                 + arrCalcsI(lngD10, lngSAV) * arrCalcsI(lngD10, lngWF) _
                                 + arrCalcsI(lngD100, lngSAV) * arrCalcsI(lngD100, lngWF)
        arrCalcsI(lngLT, lngSAV) = arrCalcsI(lngLH, lngSAV) * arrCalcsI(lngLH, lngWF) _
                                 + arrCalcsI(lngLW, lngSAV) * arrCalcsI(lngLW, lngWF)
        arrCalcsI(lngTL, lngSAV) = (arrCalcsI(lngDT, lngSAV) * arrCalcsI(lngDT, lngWF)) _
                                 + (arrCalcsI(lngLT, lngSAV) * arrCalcsI(lngLT, lngWF))

        '****Exponential Weighting Factor****
        arrCalcsI(lngDH, lngEWF) = Math.Exp(-138 / arrCalcsI(lngDH, lngSAV))
        arrCalcsI(lngD1, lngEWF) = Math.Exp(-138 / arrCalcsI(lngD1, lngSAV))
        arrCalcsI(lngD10, lngEWF) = Math.Exp(-138 / arrCalcsI(lngD10, lngSAV))
        arrCalcsI(lngD100, lngEWF) = Math.Exp(-138 / arrCalcsI(lngD100, lngSAV))
        arrCalcsI(lngLH, lngEWF) = Math.Exp(-500 / arrCalcsI(lngLH, lngSAV))
        arrCalcsI(lngLW, lngEWF) = Math.Exp(-500 / arrCalcsI(lngLW, lngSAV))

        '****Exponential Weighting Factor 2**** Effective heating number
        arrCalcsI(lngDH, lngEWF2) = arrCalcsI(lngDH, lngEWF) 'Same as Exponential Weighting Factor
        arrCalcsI(lngD1, lngEWF2) = arrCalcsI(lngD1, lngEWF) 'Same as Exponential Weighting Factor
        arrCalcsI(lngD10, lngEWF2) = arrCalcsI(lngD10, lngEWF) 'Same as Exponential Weighting Factor
        arrCalcsI(lngD100, lngEWF2) = arrCalcsI(lngD100, lngEWF) 'Same as Exponential Weighting Factor
        arrCalcsI(lngLH, lngEWF2) = Math.Exp(-138 / arrCalcsI(lngLH, lngSAV))
        arrCalcsI(lngLW, lngEWF2) = Math.Exp(-138 / arrCalcsI(lngLW, lngSAV))

        '****Fuel Moisture****
        arrCalcsI(lngDH, lngMC) = lngMC01
        arrCalcsI(lngD1, lngMC) = lngMC01
        arrCalcsI(lngD10, lngMC) = lngMC10
        arrCalcsI(lngD100, lngMC) = lngMC100
        arrCalcsI(lngDT, lngMC) = arrCalcsI(lngDH, lngMC) * arrCalcsI(lngDH, lngWF) _
                                + arrCalcsI(lngD1, lngMC) * arrCalcsI(lngD1, lngWF) _
                                + arrCalcsI(lngD10, lngMC) * arrCalcsI(lngD10, lngWF) _
                                + arrCalcsI(lngD100, lngMC) * arrCalcsI(lngD100, lngWF)
        arrCalcsI(lngLH, lngMC) = lngMCLH
        arrCalcsI(lngLW, lngMC) = lngMCLW
        arrCalcsI(lngLT, lngMC) = arrCalcsI(lngLH, lngMC) * arrCalcsI(lngLH, lngWF) _
                                + arrCalcsI(lngLW, lngMC) * arrCalcsI(lngLW, lngWF)

        '****Qig OR Heat of preignition, kJ kg^-1****
        arrCalcsI(lngDH, lngQig) = 250 + 11.16 * arrCalcsI(lngDH, lngMC)
        arrCalcsI(lngD1, lngQig) = 250 + 11.16 * arrCalcsI(lngD1, lngMC)
        arrCalcsI(lngD10, lngQig) = 250 + 11.16 * arrCalcsI(lngD10, lngMC)
        arrCalcsI(lngD100, lngQig) = 250 + 11.16 * arrCalcsI(lngD100, lngMC)
        arrCalcsI(lngLH, lngQig) = 250 + 11.16 * arrCalcsI(lngLH, lngMC)
        arrCalcsI(lngLW, lngQig) = 250 + 11.16 * arrCalcsI(lngLW, lngMC)

        '****Wn OR weight with the mineral content removed kg m^-2****
        arrCalcsI(lngDH, lngWn) = arrCalcsI(lngDH, lngWAC) * (1 - 0.0555) 'Mineral Fraction is constant at 0.0555
        arrCalcsI(lngD1, lngWn) = arrCalcsI(lngD1, lngWAC) * (1 - 0.0555) 'Mineral Fraction is constant at 0.0555
        arrCalcsI(lngD10, lngWn) = arrCalcsI(lngD10, lngWAC) * (1 - 0.0555) 'Mineral Fraction is constant at 0.0555
        arrCalcsI(lngD100, lngWn) = arrCalcsI(lngD100, lngWAC) * (1 - 0.0555) 'Mineral Fraction is constant at 0.0555
        arrCalcsI(lngLH, lngWn) = arrCalcsI(lngLH, lngWAC) * (1 - 0.0555) 'Mineral Fraction is constant at 0.0555
        arrCalcsI(lngLW, lngWn) = arrCalcsI(lngLW, lngWAC) * (1 - 0.0555) 'Mineral Fraction is constant at 0.0555
        arrCalcsI(lngTL, lngWn) = arrCalcsI(lngTL, lngWAC) * (1 - 0.0555) 'Mineral Fraction is constant at 0.0555
        '****Subclass weighting factors and net loads need to be calculated before totaling dead and live****

        'Subclass weighting factor 1
        arrCalcsI(lngDH, lngSWF1) = SCWF(1, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWF))
        arrCalcsI(lngD1, lngSWF1) = SCWF(1, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWF))
        arrCalcsI(lngD10, lngSWF1) = SCWF(1, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWF))
        arrCalcsI(lngD100, lngSWF1) = SCWF(1, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWF))
        arrCalcsI(lngDT, lngSWF1) = arrCalcsI(lngDH, lngSWF1) + arrCalcsI(lngD1, lngSWF1) _
                                 + arrCalcsI(lngD10, lngSWF1) + arrCalcsI(lngD100, lngSWF1)
        arrCalcsI(lngLH, lngSWF1) = SCWF(1, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWF))
        arrCalcsI(lngLW, lngSWF1) = SCWF(1, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWF))
        arrCalcsI(lngLT, lngSWF1) = arrCalcsI(lngLH, lngSWF1) + arrCalcsI(lngLW, lngSWF1)
        'Subclass weighting factor 2
        arrCalcsI(lngDH, lngSWF2) = SCWF(2, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWF))
        arrCalcsI(lngD1, lngSWF2) = SCWF(2, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWF))
        arrCalcsI(lngD10, lngSWF2) = SCWF(2, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWF))
        arrCalcsI(lngD100, lngSWF2) = SCWF(2, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWF))
        arrCalcsI(lngDT, lngSWF2) = arrCalcsI(lngDH, lngSWF2) + arrCalcsI(lngD1, lngSWF2) _
                                 + arrCalcsI(lngD10, lngSWF2) + arrCalcsI(lngD100, lngSWF2)
        arrCalcsI(lngLH, lngSWF2) = SCWF(2, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWF))
        arrCalcsI(lngLW, lngSWF2) = SCWF(2, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWF))
        arrCalcsI(lngLT, lngSWF2) = arrCalcsI(lngLH, lngSWF2) + arrCalcsI(lngLW, lngSWF2)
        'Subclass weighting factor 3
        arrCalcsI(lngDH, lngSWF3) = SCWF(3, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWF))
        arrCalcsI(lngD1, lngSWF3) = SCWF(3, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWF))
        arrCalcsI(lngD10, lngSWF3) = SCWF(3, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWF))
        arrCalcsI(lngD100, lngSWF3) = SCWF(3, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWF))
        arrCalcsI(lngDT, lngSWF3) = arrCalcsI(lngDH, lngSWF3) + arrCalcsI(lngD1, lngSWF3) _
                                 + arrCalcsI(lngD10, lngSWF3) + arrCalcsI(lngD100, lngSWF3)
        arrCalcsI(lngLH, lngSWF3) = SCWF(3, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWF))
        arrCalcsI(lngLW, lngSWF3) = SCWF(3, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWF))
        arrCalcsI(lngLT, lngSWF3) = arrCalcsI(lngLH, lngSWF3) + arrCalcsI(lngLW, lngSWF3)
        'Subclass weighting factor 4
        arrCalcsI(lngDH, lngSWF4) = SCWF(4, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWF))
        arrCalcsI(lngD1, lngSWF4) = SCWF(4, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWF))
        arrCalcsI(lngD10, lngSWF4) = SCWF(4, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWF))
        arrCalcsI(lngD100, lngSWF4) = SCWF(4, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWF))
        arrCalcsI(lngDT, lngSWF4) = arrCalcsI(lngDH, lngSWF4) + arrCalcsI(lngD1, lngSWF4) _
                                 + arrCalcsI(lngD10, lngSWF4) + arrCalcsI(lngD100, lngSWF4)
        arrCalcsI(lngLH, lngSWF4) = SCWF(4, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWF))
        arrCalcsI(lngLW, lngSWF4) = SCWF(4, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWF))
        arrCalcsI(lngLT, lngSWF4) = arrCalcsI(lngLH, lngSWF4) + arrCalcsI(lngLW, lngSWF4)
        'Subclass weighting factor 5
        arrCalcsI(lngDH, lngSWF5) = SCWF(5, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWF))
        arrCalcsI(lngD1, lngSWF5) = SCWF(5, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWF))
        arrCalcsI(lngD10, lngSWF5) = SCWF(5, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWF))
        arrCalcsI(lngD100, lngSWF5) = SCWF(5, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWF))
        arrCalcsI(lngDT, lngSWF5) = arrCalcsI(lngDH, lngSWF5) + arrCalcsI(lngD1, lngSWF5) _
                                 + arrCalcsI(lngD10, lngSWF5) + arrCalcsI(lngD100, lngSWF5)
        arrCalcsI(lngLH, lngSWF5) = SCWF(5, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWF))
        arrCalcsI(lngLW, lngSWF5) = SCWF(5, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWF))
        arrCalcsI(lngLT, lngSWF5) = arrCalcsI(lngLH, lngSWF5) + arrCalcsI(lngLW, lngSWF5)
        'Subclass net load 1
        arrCalcsI(lngDH, lngSNL1) = SCNL(1, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWn))
        arrCalcsI(lngD1, lngSNL1) = SCNL(1, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWn))
        arrCalcsI(lngD10, lngSNL1) = SCNL(1, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWn))
        arrCalcsI(lngD100, lngSNL1) = SCNL(1, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWn))
        arrCalcsI(lngDT, lngSNL1) = arrCalcsI(lngDH, lngSNL1) + arrCalcsI(lngD1, lngSNL1) _
                                 + arrCalcsI(lngD10, lngSNL1) + arrCalcsI(lngD100, lngSNL1)
        arrCalcsI(lngLH, lngSNL1) = SCNL(1, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWn))
        arrCalcsI(lngLW, lngSNL1) = SCNL(1, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWn))
        arrCalcsI(lngLT, lngSNL1) = arrCalcsI(lngLH, lngSNL1) + arrCalcsI(lngLW, lngSNL1)
        'Subclass net load 2
        arrCalcsI(lngDH, lngSNL2) = SCNL(2, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWn))
        arrCalcsI(lngD1, lngSNL2) = SCNL(2, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWn))
        arrCalcsI(lngD10, lngSNL2) = SCNL(2, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWn))
        arrCalcsI(lngD100, lngSNL2) = SCNL(2, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWn))
        arrCalcsI(lngDT, lngSNL2) = arrCalcsI(lngDH, lngSNL2) + arrCalcsI(lngD1, lngSNL2) _
                                 + arrCalcsI(lngD10, lngSNL2) + arrCalcsI(lngD100, lngSNL2)
        arrCalcsI(lngLH, lngSNL2) = SCNL(2, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWn))
        arrCalcsI(lngLW, lngSNL2) = SCNL(2, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWn))
        arrCalcsI(lngLT, lngSNL2) = arrCalcsI(lngLH, lngSNL2) + arrCalcsI(lngLW, lngSNL2)
        'Subclass net load 3
        arrCalcsI(lngDH, lngSNL3) = SCNL(3, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWn))
        arrCalcsI(lngD1, lngSNL3) = SCNL(3, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWn))
        arrCalcsI(lngD10, lngSNL3) = SCNL(3, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWn))
        arrCalcsI(lngD100, lngSNL3) = SCNL(3, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWn))
        arrCalcsI(lngDT, lngSNL3) = arrCalcsI(lngDH, lngSNL3) + arrCalcsI(lngD1, lngSNL3) _
                                 + arrCalcsI(lngD10, lngSNL3) + arrCalcsI(lngD100, lngSNL3)
        arrCalcsI(lngLH, lngSNL3) = SCNL(3, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWn))
        arrCalcsI(lngLW, lngSNL3) = SCNL(3, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWn))
        arrCalcsI(lngLT, lngSNL3) = arrCalcsI(lngLH, lngSNL3) + arrCalcsI(lngLW, lngSNL3)
        'Subclass net load 4
        arrCalcsI(lngDH, lngSNL4) = SCNL(4, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWn))
        arrCalcsI(lngD1, lngSNL4) = SCNL(4, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWn))
        arrCalcsI(lngD10, lngSNL4) = SCNL(4, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWn))
        arrCalcsI(lngD100, lngSNL4) = SCNL(4, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWn))
        arrCalcsI(lngDT, lngSNL4) = arrCalcsI(lngDH, lngSNL4) + arrCalcsI(lngD1, lngSNL4) _
                                 + arrCalcsI(lngD10, lngSNL4) + arrCalcsI(lngD100, lngSNL4)
        arrCalcsI(lngLH, lngSNL4) = SCNL(4, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWn))
        arrCalcsI(lngLW, lngSNL4) = SCNL(4, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWn))
        arrCalcsI(lngLT, lngSNL4) = arrCalcsI(lngLH, lngSNL4) + arrCalcsI(lngLW, lngSNL4)
        'Subclass net load 5
        arrCalcsI(lngDH, lngSNL5) = SCNL(5, arrCalcsI(lngDH, lngSAV), arrCalcsI(lngDH, lngWn))
        arrCalcsI(lngD1, lngSNL5) = SCNL(5, arrCalcsI(lngD1, lngSAV), arrCalcsI(lngD1, lngWn))
        arrCalcsI(lngD10, lngSNL5) = SCNL(5, arrCalcsI(lngD10, lngSAV), arrCalcsI(lngD10, lngWn))
        arrCalcsI(lngD100, lngSNL5) = SCNL(5, arrCalcsI(lngD100, lngSAV), arrCalcsI(lngD100, lngWn))
        arrCalcsI(lngDT, lngSNL5) = arrCalcsI(lngDH, lngSNL5) + arrCalcsI(lngD1, lngSNL5) _
                                 + arrCalcsI(lngD10, lngSNL5) + arrCalcsI(lngD100, lngSNL5)
        arrCalcsI(lngLH, lngSNL5) = SCNL(5, arrCalcsI(lngLH, lngSAV), arrCalcsI(lngLH, lngWn))
        arrCalcsI(lngLW, lngSNL5) = SCNL(5, arrCalcsI(lngLW, lngSAV), arrCalcsI(lngLW, lngWn))
        arrCalcsI(lngLT, lngSNL5) = arrCalcsI(lngLH, lngSNL5) + arrCalcsI(lngLW, lngSNL5)
        arrCalcsI(lngDT, lngWn) = arrCalcsI(lngDT, lngSWF1) * arrCalcsI(lngDT, lngSNL1) _
                                + arrCalcsI(lngDT, lngSWF2) * arrCalcsI(lngDT, lngSNL2) _
                                + arrCalcsI(lngDT, lngSWF3) * arrCalcsI(lngDT, lngSNL3) _
                                + arrCalcsI(lngDT, lngSWF4) * arrCalcsI(lngDT, lngSNL4) _
                                + arrCalcsI(lngDT, lngSWF5) * arrCalcsI(lngDT, lngSNL5)
        arrCalcsI(lngLT, lngWn) = arrCalcsI(lngLT, lngSWF1) * arrCalcsI(lngLT, lngSNL1) _
                                + arrCalcsI(lngLT, lngSWF2) * arrCalcsI(lngLT, lngSNL2) _
                                + arrCalcsI(lngLT, lngSWF3) * arrCalcsI(lngLT, lngSNL3) _
                                + arrCalcsI(lngLT, lngSWF4) * arrCalcsI(lngLT, lngSNL4) _
                                + arrCalcsI(lngLT, lngSWF5) * arrCalcsI(lngLT, lngSNL5)
    End Sub

    Public Function GetFMRow(ByVal FMNum As Long) As Long 'Returns the row number from the selected FM number
        Select Case FMNum
            Case Is <= 13 : GetFMRow = FMNum - 1
            Case Is <= 109 : GetFMRow = FMNum - 88
            Case Is <= 124 : GetFMRow = FMNum - 99
            Case Is <= 149 : GetFMRow = FMNum - 115
            Case Is <= 165 : GetFMRow = FMNum - 126
            Case Is <= 189 : GetFMRow = FMNum - 141
            Case Is <= 204 : GetFMRow = FMNum - 152
        End Select
    End Function

    Public Function SCWF(ByVal lngSCnum As Long, ByVal dblSAVVal As Double, ByVal dblWFVal As Double) As Double
        'Runs calculation for subclass weighting factor and return the result
        SCWF = 0
        If lngSCnum = 1 And dblSAVVal > 1200 Then
            SCWF = dblWFVal
        ElseIf lngSCnum = 2 And dblSAVVal > 192 And dblSAVVal < 1199.9999 Then
            SCWF = dblWFVal
        ElseIf lngSCnum = 3 And dblSAVVal > 96 And dblSAVVal < 191.9999 Then
            SCWF = dblWFVal
        ElseIf lngSCnum = 4 And dblSAVVal > 48 And dblSAVVal < 95.9999 Then
            SCWF = dblWFVal
        ElseIf lngSCnum = 5 And dblSAVVal > 16 And dblSAVVal < 47.9999 Then
            SCWF = dblWFVal
        End If
    End Function

    Public Function SCNL(ByVal lngSCnum As Long, ByVal dblSAVVal As Double, ByVal dblWnVal As Double) As Double
        'Runs calculation for subclass weighting factor and return the result
        SCNL = 0
        If lngSCnum = 1 And dblSAVVal > 1200 Then
            SCNL = dblWnVal
        ElseIf lngSCnum = 2 And dblSAVVal > 192 And dblSAVVal < 1199.9999 Then
            SCNL = dblWnVal
        ElseIf lngSCnum = 3 And dblSAVVal > 96 And dblSAVVal < 191.9999 Then
            SCNL = dblWnVal
        ElseIf lngSCnum = 4 And dblSAVVal > 48 And dblSAVVal < 95.9999 Then
            SCNL = dblWnVal
        ElseIf lngSCnum = 5 And dblSAVVal > 16 And dblSAVVal < 47.9999 Then
            SCNL = dblWnVal
        End If
    End Function

    Private Function GetMCDF() As Double 'Moisture content of dead fuel low
        GetMCDF = arrCalcsI(lngDT, lngMC) / 100
    End Function

    Private Function GetLTFM() As Double 'Live total fuel moisture
        GetLTFM = arrCalcsI(lngLT, lngMC)
    End Function

    Private Function GetMCEX() As Double 'Extinction moisture content low
        GetMCEX = XtMoist / 100
    End Function

    Private Function GetDTWn() As Double 'Dead Total weight, mineral content removed
        GetDTWn = arrCalcsI(lngDT, lngWn)
    End Function

    Private Function GetLTWn() As Double 'Live Total weight, mineral content removed
        GetLTWn = arrCalcsI(lngLT, lngWn)
    End Function

    Private Function GetLOAD() As Double 'Weight adusted Total load converted to lb/ft^2
        GetLOAD = arrCalcsI(lngTL, lngWAC)
    End Function

    Private Function GetFDEP() As Double 'Fuel bed depth from models
        'Declare variables
        Const lngLADM As Long = 1 'LADM low? Is a constant modifier of 1
        GetFDEP = Depth * lngLADM
    End Function

    Private Function GetDWFEWFQig() As Double
        'Heat Sink equation bulkdensity x heat of ignition * heat of ignition
        'Sum product of the dead  fuel weight factor,exponential weight factor, and heat of preignition, kJ kg^-1
        GetDWFEWFQig = arrCalcsI(lngDH, lngWF) * arrCalcsI(lngDH, lngEWF2) * arrCalcsI(lngDH, lngQig) _
                     + arrCalcsI(lngD1, lngWF) * arrCalcsI(lngD1, lngEWF2) * arrCalcsI(lngD1, lngQig) _
                     + arrCalcsI(lngD10, lngWF) * arrCalcsI(lngD10, lngEWF2) * arrCalcsI(lngD10, lngQig) _
                     + arrCalcsI(lngD100, lngWF) * arrCalcsI(lngD100, lngEWF2) * arrCalcsI(lngD100, lngQig)
    End Function

    Private Function GetLWFEWFQig() As Double
        'Heat Sink equation bulkdensity x heat of ignition * heat of ignition
        'Sum product of the live fuel weight factor,exponential weight factor, and heat of preignition, kJ kg^-1
        GetLWFEWFQig = arrCalcsI(lngLH, lngWF) * arrCalcsI(lngLH, lngEWF2) * arrCalcsI(lngLH, lngQig) _
                       + arrCalcsI(lngLW, lngWF) * arrCalcsI(lngLW, lngEWF2) * arrCalcsI(lngLW, lngQig)
    End Function

    Private Function GetDFHC() As Long 'Dead fuel heat content
        GetDFHC = DHt
    End Function

    Private Function GetLFHC() As Long 'Live fuel heat content
        GetLFHC = LHt
    End Function

    Private Function GetSigma() As Double 'Surface area to volume ratio of fuel particles, cm^-1 low total load SAV
        GetSigma = arrCalcsI(lngTL, lngSAV)
    End Function

    Private Function GetDFRC() As Double 'Dead fraction low Dead total weight factor
        GetDFRC = arrCalcsI(lngDT, lngWF)
    End Function

    Private Function GetLFRC() As Double 'Live fraction low Live total weight factor
        GetLFRC = arrCalcsI(lngLT, lngWF)
    End Function

    Private Function GetLTWAC() As Double 'Live total weight adjusted and converted to lb/ft^2
        GetLTWAC = arrCalcsI(lngLT, lngWAC)
    End Function

    Private Function GetDWACDEWF() As Double
        'Sum product of dead weight adjusted converted to lb/ft^2 and dead exponential weight factor
        GetDWACDEWF = arrCalcsI(lngDH, lngWAC) * arrCalcsI(lngDH, lngEWF) _
                      + arrCalcsI(lngD1, lngWAC) * arrCalcsI(lngD1, lngEWF) _
                      + arrCalcsI(lngD10, lngWAC) * arrCalcsI(lngD10, lngEWF) _
                      + arrCalcsI(lngD100, lngWAC) * arrCalcsI(lngD100, lngEWF)
    End Function

    Private Function GetLWACLEWF() As Double
        'Sum product of live weight adjusted converted to lb/ft^2 and live exponential weight factor
        GetLWACLEWF = arrCalcsI(lngLH, lngWAC) * arrCalcsI(lngLH, lngEWF) _
                      + arrCalcsI(lngLW, lngWAC) * arrCalcsI(lngLW, lngEWF)
    End Function

    Private Function GetDEWFDWnDMC() As Double
        'Sum product of dead exponential weight factor, dead weight with mineral content removed, and dead fuel moisture content
        GetDEWFDWnDMC = arrCalcsI(lngDH, lngEWF) * arrCalcsI(lngDH, lngWn) * arrCalcsI(lngDH, lngMC) _
                        + arrCalcsI(lngD1, lngEWF) * arrCalcsI(lngD1, lngWn) * arrCalcsI(lngD1, lngMC) _
                        + arrCalcsI(lngD10, lngEWF) * arrCalcsI(lngD10, lngWn) * arrCalcsI(lngD10, lngMC) _
                        + arrCalcsI(lngD100, lngEWF) * arrCalcsI(lngD100, lngWn) * arrCalcsI(lngD100, lngMC)
    End Function

    Private Function GetDEWFDWn() As Double
        'Sum product of dead eponential weighting factor and dead weight with mineral content removed
        GetDEWFDWn = arrCalcsI(lngDH, lngEWF) * arrCalcsI(lngDH, lngWn) _
                     + arrCalcsI(lngD1, lngEWF) * arrCalcsI(lngD1, lngWn) _
                     + arrCalcsI(lngD10, lngEWF) * arrCalcsI(lngD10, lngWn) _
                     + arrCalcsI(lngD100, lngEWF) * arrCalcsI(lngD100, lngWn)
    End Function
End Class
