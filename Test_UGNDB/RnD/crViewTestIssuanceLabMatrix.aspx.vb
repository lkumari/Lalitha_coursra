' ************************************************************************************************
' Name:	crViewTestIssuanceLabMatrix.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from old [Test Issuance Requests] table
'
' Date		    Author	    
' 05/26/2009    LRey			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class RnD_crViewTestIssuanceLabMatrix
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.crviewmasterpage_master = Master

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Research and Development</b> > <a href='LabRequestMatrix.aspx'><b>Lab Request Matrix</b></a> "
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState("pRequestID") = HttpContext.Current.Request.QueryString("pRequestID")
        ViewState("pReqDtFrom") = HttpContext.Current.Request.QueryString("pReqDtFrom")
        ViewState("pReqDtTo") = HttpContext.Current.Request.QueryString("pReqDtTo")
        ViewState("pReqStatus") = HttpContext.Current.Request.QueryString("pReqStatus")
        ViewState("pUGNFacility") = HttpContext.Current.Request.QueryString("pUGNFacility")
        ViewState("pTestClass") = HttpContext.Current.Request.QueryString("pTestClass")

        'If Not Page.IsPostBack Then

        Dim oRpt As ReportDocument = New ReportDocument()

        If Session("TempCrystalRptFiles") Is Nothing Then
            Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
            'Dim crTbl As CrystalDecisions.CrystalReports.Engine.Table
            Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
            Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

            ' new report document object 
            oRpt.Load(Server.MapPath(".\Forms\") & "crLabRequestMatrix.rpt")

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
            Dim pfRequestID As ParameterField = New ParameterField
            Dim pfReqDtFrom As ParameterField = New ParameterField
            Dim pfReqDtTo As ParameterField = New ParameterField
            Dim pfReqStatus As ParameterField = New ParameterField
            Dim pfUGNFacility As ParameterField = New ParameterField
            Dim pfTestClass As ParameterField = New ParameterField

            ' setting the name of parameter fields with wich they will be recieved in report 
            pfRequestID.ParameterFieldName = "@RequestID"
            pfReqDtFrom.ParameterFieldName = "@ReqDtFrom"
            pfReqDtTo.ParameterFieldName = "@ReqDtTo"
            pfReqStatus.ParameterFieldName = "@ReqStatus"
            pfUGNFacility.ParameterFieldName = "@UGNFacility"
            pfTestClass.ParameterFieldName = "@TestClass"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcRequestID As New ParameterDiscreteValue
            Dim dcReqDtFrom As New ParameterDiscreteValue
            Dim dcReqDtTo As New ParameterDiscreteValue
            Dim dcReqStatus As New ParameterDiscreteValue
            Dim dcUGNFacility As New ParameterDiscreteValue
            Dim dcTestClass As New ParameterDiscreteValue


            ' setting the values of discrete objects 
            dcRequestID.Value = ViewState("pRequestID")
            dcReqDtFrom.Value = ViewState("pReqDtFrom")
            dcReqDtTo.Value = ViewState("pReqDtTo")
            dcReqStatus.Value = ViewState("pReqStatus")
            dcUGNFacility.Value = ViewState("pUGNFacility")
            dcTestClass.Value = ViewState("pTestClass")

            ' now adding these discrete values to parameters 
            pfRequestID.CurrentValues.Add(dcRequestID)
            pfReqDtFrom.CurrentValues.Add(dcReqDtFrom)
            pfReqDtTo.CurrentValues.Add(dcReqDtTo)
            pfReqStatus.CurrentValues.Add(dcReqStatus)
            pfUGNFacility.CurrentValues.Add(dcUGNFacility)
            pfTestClass.CurrentValues.Add(dcTestClass)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfRequestID)
            paramFields.Add(pfReqDtFrom)
            paramFields.Add(pfReqDtTo)
            paramFields.Add(pfReqStatus)
            paramFields.Add(pfUGNFacility)
            paramFields.Add(pfTestClass)

            ' finally add the parameter collection to the crystal report viewer 
            CrystalReportViewer1.ParameterFieldInfo = paramFields

        Catch ex As Exception
            lblErrors.Text = "Error found in parameter search " & ex.Message
            lblErrors.Visible = True
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'in order to clear crystal reports
        If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
            GC.Collect()
        End If
    End Sub
End Class
