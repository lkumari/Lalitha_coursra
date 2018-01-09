' ************************************************************************************************
' Name:	crViewCostDownUpCalculator.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from old [Test Issuance Requests] table
'
' Date		    Author	    
' 07/01/2009    LRey			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class PF_crViewCostDownUpCalculator
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.crviewmasterpage_master = Master

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > <a href='Cost_Down_Up_Calculator.aspx'> Cost Down/Up Calculator </a>"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState("pPlanningYear") = HttpContext.Current.Request.QueryString("pPlanningYear")
        ViewState("pRecordType") = HttpContext.Current.Request.QueryString("pRecordType")
        ViewState("pRecordTypeNo") = HttpContext.Current.Request.QueryString("pRecordTypeNo")
        ViewState("pCalculate") = HttpContext.Current.Request.QueryString("pCalculate")
        ViewState("pCalcDI") = HttpContext.Current.Request.QueryString("pCalcDI")

        ViewState("pJan") = HttpContext.Current.Request.QueryString("pJan")
        ViewState("pFeb") = HttpContext.Current.Request.QueryString("pFeb")
        ViewState("pMar") = HttpContext.Current.Request.QueryString("pMar")
        ViewState("pApr") = HttpContext.Current.Request.QueryString("pApr")
        ViewState("pMay") = HttpContext.Current.Request.QueryString("pMay")
        ViewState("pJun") = HttpContext.Current.Request.QueryString("pJun")
        ViewState("pJul") = HttpContext.Current.Request.QueryString("pJul")
        ViewState("pAug") = HttpContext.Current.Request.QueryString("pAug")
        ViewState("pSep") = HttpContext.Current.Request.QueryString("pSep")
        ViewState("pOct") = HttpContext.Current.Request.QueryString("pOct")
        ViewState("pNov") = HttpContext.Current.Request.QueryString("pNov")
        ViewState("pDec") = HttpContext.Current.Request.QueryString("pDec")

        Dim oRpt As ReportDocument = New ReportDocument()

        If Session("TempCrystalRptFiles") Is Nothing Then
            Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
            'Dim crTbl As CrystalDecisions.CrystalReports.Engine.Table
            Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
            Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

            ' new report document object 
            oRpt.Load(Server.MapPath(".\Forms\") & "crCostDownUpCalculator.rpt")

            'getting the database, the table and the LogOnInfo object which holds login onformation 
            crDatabase = oRpt.Database

            'getting the table in an object array of one item 
            Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
            crDatabase.Tables.CopyTo(arrTables, 0)
            ' assigning the first item of array to crTable by downcasting the object to Table 
            crTable = arrTables(0)

            ' setting values 
            dbConn = crTable.LogOnInfo
            dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGN_HR" or "UGN_HR"
            dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"TAPS1"
            dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
            dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

            ' applying login info to the table object 
            crTable.ApplyLogOnInfo(dbConn)

            ' defining report source 
            CrystalReportViewer1.DisplayGroupTree = True
            CrystalReportViewer1.ReportSource = oRpt

            ' so uptil now we have created everything 
            ' what remains is to pass parameters to our report, so it 
            ' shows only selected records. so calling a method to set 
            ' those parameters. 

            'Check if there are parameters or not in report.
            Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

            setReportParameters()
            Session("TempCrystalRptFiles") = oRpt
        Else
            oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)

            CrystalReportViewer1.ReportSource = oRpt
        End If
    End Sub

    Private Sub setReportParameters()

        Try
            ' all the parameter fields will be added to this collection 
            Dim paramFields As New ParameterFields

            ' the parameter fields to be sent to the report 
            Dim pfPlanningYear As ParameterField = New ParameterField
            Dim pfRecordType As ParameterField = New ParameterField
            Dim pfRecordTypeNo As ParameterField = New ParameterField
            Dim pfCalculate As ParameterField = New ParameterField
            Dim pfCalculateDI As ParameterField = New ParameterField
            Dim pfJan As ParameterField = New ParameterField
            Dim pfFeb As ParameterField = New ParameterField
            Dim pfMar As ParameterField = New ParameterField
            Dim pfApr As ParameterField = New ParameterField
            Dim pfMay As ParameterField = New ParameterField
            Dim pfJun As ParameterField = New ParameterField
            Dim pfJul As ParameterField = New ParameterField
            Dim pfAug As ParameterField = New ParameterField
            Dim pfSep As ParameterField = New ParameterField
            Dim pfOct As ParameterField = New ParameterField
            Dim pfNov As ParameterField = New ParameterField
            Dim pfDec As ParameterField = New ParameterField

            ' setting the name of parameter fields with wich they will be recieved in report 
            pfPlanningYear.ParameterFieldName = "@PlanningYear"
            pfRecordType.ParameterFieldName = "@RecordType"
            pfRecordTypeNo.ParameterFieldName = "@RecordTypeNo"
            pfCalculate.ParameterFieldName = "@Calculate"
            pfCalculateDI.ParameterFieldName = "@CalculateDI"
            pfJan.ParameterFieldName = "@Jan"
            pfFeb.ParameterFieldName = "@Feb"
            pfMar.ParameterFieldName = "@Mar"
            pfApr.ParameterFieldName = "@Apr"
            pfMay.ParameterFieldName = "@May"
            pfJun.ParameterFieldName = "@Jun"
            pfJul.ParameterFieldName = "@Jul"
            pfAug.ParameterFieldName = "@Aug"
            pfSep.ParameterFieldName = "@Sep"
            pfOct.ParameterFieldName = "@Oct"
            pfNov.ParameterFieldName = "@Nov"
            pfDec.ParameterFieldName = "@Dec"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcPlanningYear As New ParameterDiscreteValue
            Dim dcRecordType As New ParameterDiscreteValue
            Dim dcRecordTypeNo As New ParameterDiscreteValue
            Dim dcCalculate As New ParameterDiscreteValue
            Dim dcCalculateDI As New ParameterDiscreteValue

            Dim dcJan As New ParameterDiscreteValue
            Dim dcFeb As New ParameterDiscreteValue
            Dim dcMar As New ParameterDiscreteValue
            Dim dcApr As New ParameterDiscreteValue
            Dim dcMay As New ParameterDiscreteValue
            Dim dcJun As New ParameterDiscreteValue
            Dim dcJul As New ParameterDiscreteValue
            Dim dcAug As New ParameterDiscreteValue
            Dim dcSep As New ParameterDiscreteValue
            Dim dcOct As New ParameterDiscreteValue
            Dim dcNov As New ParameterDiscreteValue
            Dim dcDec As New ParameterDiscreteValue

            ' setting the values of discrete objects 
            dcPlanningYear.Value = ViewState("pPlanningYear")
            dcRecordType.Value = ViewState("pRecordType")
            dcRecordTypeNo.Value = ViewState("pRecordTypeNo")
            dcCalculate.Value = ViewState("pCalculate")
            dcCalculateDI.Value = ViewState("pCalcDI")
            dcJan.Value = ViewState("pJan")
            dcFeb.Value = ViewState("pFeb")
            dcMar.Value = ViewState("pMar")
            dcApr.Value = ViewState("pApr")
            dcMay.Value = ViewState("pMay")
            dcJun.Value = ViewState("pJun")
            dcJul.Value = ViewState("pJul")
            dcAug.Value = ViewState("pAug")
            dcSep.Value = ViewState("pSep")
            dcOct.Value = ViewState("pOct")
            dcNov.Value = ViewState("pNov")
            dcDec.Value = ViewState("pDec")

            ' now adding these discrete values to parameters 
            pfPlanningYear.CurrentValues.Add(dcPlanningYear)
            pfRecordType.CurrentValues.Add(dcRecordType)
            pfRecordTypeNo.CurrentValues.Add(dcRecordTypeNo)
            pfCalculate.CurrentValues.Add(dcCalculate)
            pfCalculateDI.CurrentValues.Add(dcCalculateDI)
            pfJan.CurrentValues.Add(dcJan)
            pfFeb.CurrentValues.Add(dcFeb)
            pfMar.CurrentValues.Add(dcMar)
            pfApr.CurrentValues.Add(dcApr)
            pfMay.CurrentValues.Add(dcMay)
            pfJun.CurrentValues.Add(dcJun)
            pfJul.CurrentValues.Add(dcJul)
            pfAug.CurrentValues.Add(dcAug)
            pfSep.CurrentValues.Add(dcSep)
            pfOct.CurrentValues.Add(dcOct)
            pfNov.CurrentValues.Add(dcNov)
            pfDec.CurrentValues.Add(dcDec)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfPlanningYear)
            paramFields.Add(pfRecordType)
            paramFields.Add(pfRecordTypeNo)
            paramFields.Add(pfCalculate)
            paramFields.Add(pfCalculateDI)
            paramFields.Add(pfJan)
            paramFields.Add(pfFeb)
            paramFields.Add(pfMar)
            paramFields.Add(pfApr)
            paramFields.Add(pfMay)
            paramFields.Add(pfJun)
            paramFields.Add(pfJul)
            paramFields.Add(pfAug)
            paramFields.Add(pfSep)
            paramFields.Add(pfOct)
            paramFields.Add(pfNov)
            paramFields.Add(pfDec)

            ' finally add the parameter collection to the crystal report viewer 
            CrystalReportViewer1.ParameterFieldInfo = paramFields

        Catch ex As Exception
            lblErrors.Text = "Error found in parameter search " & ex.Message
            lblErrors.Visible = True
        End Try
    End Sub
End Class
