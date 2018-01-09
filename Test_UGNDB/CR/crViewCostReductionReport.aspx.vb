' ************************************************************************************************
' Name:	crViewCostReductionReport.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from old [Test Issuance Requests] table
'
' Date		    Author	    
' 02/02/2010    LRey			Created .Net application
' 12/10/2012    RCarlson        Modified: fix spelling error recieve
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class CR_crViewCostReductionReport
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.crviewmasterpage_master = Master

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Cost Reduction</b> > <a href='CostReductionReport.aspx'><b>Cost Reduction Report</b></a>"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState("sImpDtFrom") = ""
        If HttpContext.Current.Request.QueryString("pImpDtFrom") IsNot Nothing Then
            ViewState("sImpDtFrom") = HttpContext.Current.Request.QueryString("pImpDtFrom")
        End If

        ViewState("sImpDtTo") = ""
        If HttpContext.Current.Request.QueryString("sImpDtTo") IsNot Nothing Then
            ViewState("sImpDtTo") = HttpContext.Current.Request.QueryString("pImpDtTo")
        End If

        ViewState("sFac") = ""
        If HttpContext.Current.Request.QueryString("pUGNFacility") IsNot Nothing Then
            ViewState("sFac") = HttpContext.Current.Request.QueryString("pUGNFacility")
        End If

        ViewState("sLTMID") = 0
        If HttpContext.Current.Request.QueryString("pLeader") IsNot Nothing Then
            If HttpContext.Current.Request.QueryString("pLeader") <> "" Then
                If CType(HttpContext.Current.Request.QueryString("pLeader"), Integer) > 0 Then
                    ViewState("sLTMID") = CType(HttpContext.Current.Request.QueryString("pLeader"), Integer)
                End If
            End If
        End If

        ViewState("sCID") = 0
        If HttpContext.Current.Request.QueryString("pCommodity") IsNot Nothing Then
            If HttpContext.Current.Request.QueryString("pCommodity") <> "" Then
                If CType(HttpContext.Current.Request.QueryString("pCommodity"), Integer) > 0 Then
                    ViewState("sCID") = CType(HttpContext.Current.Request.QueryString("pCommodity"), Integer)
                End If
            End If
        End If

        ViewState("sCABBV") = ""
        If HttpContext.Current.Request.QueryString("pCABBV") IsNot Nothing Then
            ViewState("pCABBV") = HttpContext.Current.Request.QueryString("pCABBV")
        End If

        ViewState("sSoldTo") = 0
        If HttpContext.Current.Request.QueryString("pSoldTo") IsNot Nothing Then
            If HttpContext.Current.Request.QueryString("pSoldTo") <> "" Then
                If CType(HttpContext.Current.Request.QueryString("pSoldTo"), Integer) > 0 Then
                    ViewState("sSoldTo") = CType(HttpContext.Current.Request.QueryString("pSoldTo"), Integer)
                End If
            End If
        End If

        ViewState("sPgm") = 0
        If HttpContext.Current.Request.QueryString("pPgm") IsNot Nothing Then
            If HttpContext.Current.Request.QueryString("pPgm") <> "" Then
                If CType(HttpContext.Current.Request.QueryString("pPgm"), Integer) > 0 Then
                    ViewState("sPgm") = CType(HttpContext.Current.Request.QueryString("pPgm"), Integer)
                End If
            End If
        End If

        ViewState("sPCID") = 0
        If HttpContext.Current.Request.QueryString("pProjCat") IsNot Nothing Then
            If HttpContext.Current.Request.QueryString("pProjCat") <> "" Then
                If CType(HttpContext.Current.Request.QueryString("pProjCat"), Integer) > 0 Then
                    ViewState("sPCID") = CType(HttpContext.Current.Request.QueryString("pProjCat"), Integer)
                End If
            End If
        End If

        ViewState("sPCR") = Nothing
        If HttpContext.Current.Request.QueryString("pPCR") IsNot Nothing Then
            If HttpContext.Current.Request.QueryString("pPCR") <> "" Then
                If CType(HttpContext.Current.Request.QueryString("pPCR"), Integer) > 0 Then
                    ViewState("sPCR") = CType(HttpContext.Current.Request.QueryString("pPCR"), Integer)
                End If
            End If
        End If

        ViewState("sSort") = ""
        If HttpContext.Current.Request.QueryString("pSortBy") IsNot Nothing Then
            ViewState("sSort") = HttpContext.Current.Request.QueryString("pSortBy")
        End If

        'If Not Page.IsPostBack Then

        Dim oRpt As ReportDocument = New ReportDocument()

        If Session("TempCrystalRptFiles") Is Nothing Then
            Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
            'Dim crTbl As CrystalDecisions.CrystalReports.Engine.Table
            Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
            Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

            ' new report document object 
            oRpt.Load(Server.MapPath(".\Forms\") & "crCostReductionReport.rpt")

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
            Dim pfImpDtFrom As ParameterField = New ParameterField
            Dim pfImpDtTo As ParameterField = New ParameterField
            Dim pfUGNFacility As ParameterField = New ParameterField
            Dim pfLeader As ParameterField = New ParameterField
            Dim pfCommodity As ParameterField = New ParameterField
            Dim pfProjCat As ParameterField = New ParameterField
            Dim pfStatusID As ParameterField = New ParameterField
            Dim pfStepID As ParameterField = New ParameterField
            Dim pfSortBy As ParameterField = New ParameterField
            Dim pfCABBV As ParameterField = New ParameterField
            Dim pfSoldTo As ParameterField = New ParameterField
            Dim pfProgramID As ParameterField = New ParameterField
            Dim pfPCR As ParameterField = New ParameterField


            ' setting the name of parameter fields with wich they will be received in report 
            pfImpDtFrom.ParameterFieldName = "@ImpDateFrom"
            pfImpDtTo.ParameterFieldName = "@ImpDateTo"
            pfUGNFacility.ParameterFieldName = "@UGNFacility"
            pfLeader.ParameterFieldName = "@LeaderTMID"
            pfCommodity.ParameterFieldName = "@CommodityID"
            pfProjCat.ParameterFieldName = "@ProjectCategoryID"
            pfStatusID.ParameterFieldName = "@StatusID"
            pfStepID.ParameterFieldName = "@StepID"
            pfSortBy.ParameterFieldName = "@SortBy"
            pfCABBV.ParameterFieldName = "@CABBV"
            pfSoldTo.ParameterFieldName = "@SoldTo"
            pfProgramID.ParameterFieldName = "@ProgramID"
            pfPCR.ParameterFieldName = "@isPCRev"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcImpDtFrom As New ParameterDiscreteValue
            Dim dcImpDtTo As New ParameterDiscreteValue
            Dim dcUGNFacility As New ParameterDiscreteValue
            Dim dcLeader As New ParameterDiscreteValue
            Dim dcCommodity As New ParameterDiscreteValue
            Dim dcProjCat As New ParameterDiscreteValue
            Dim dcStatusID As New ParameterDiscreteValue
            Dim dcStepID As New ParameterDiscreteValue
            Dim dcSortBy As New ParameterDiscreteValue
            Dim dcCABBV As New ParameterDiscreteValue
            Dim dcSoldTo As New ParameterDiscreteValue
            Dim dcProgramID As New ParameterDiscreteValue
            Dim dcPCR As New ParameterDiscreteValue

            ' setting the values of discrete objects 
            dcImpDtFrom.Value = ViewState("sImpDtFrom")
            dcImpDtTo.Value = ViewState("sImpDtTo")
            dcUGNFacility.Value = ViewState("sFac")
            dcLeader.Value = ViewState("sLTMID")
            dcCommodity.Value = ViewState("sCID")
            dcProjCat.Value = ViewState("sPCID")
            dcStatusID.Value = 0
            dcStepID.Value = 0
            dcSortBy.Value = ViewState("sSort")
            dcCABBV.Value = ViewState("sCABBV")
            dcSoldTo.Value = ViewState("sSoldTo")
            dcProgramID.Value = ViewState("sPgm")
            dcPCR.Value = ViewState("sPCR")

            ' now adding these discrete values to parameters 
            pfImpDtFrom.CurrentValues.Add(dcImpDtFrom)
            pfImpDtTo.CurrentValues.Add(dcImpDtTo)
            pfUGNFacility.CurrentValues.Add(dcUGNFacility)
            pfLeader.CurrentValues.Add(dcLeader)
            pfCommodity.CurrentValues.Add(dcCommodity)
            pfProjCat.CurrentValues.Add(dcProjCat)
            pfStatusID.CurrentValues.Add(dcStatusID)
            pfStepID.CurrentValues.Add(dcStepID)
            pfSortBy.CurrentValues.Add(dcSortBy)
            pfCABBV.CurrentValues.Add(dcCABBV)
            pfSoldTo.CurrentValues.Add(dcSoldTo)
            pfProgramID.CurrentValues.Add(dcProgramID)
            pfPCR.CurrentValues.Add(dcPCR)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfImpDtFrom)
            paramFields.Add(pfImpDtTo)
            paramFields.Add(pfUGNFacility)
            paramFields.Add(pfLeader)
            paramFields.Add(pfCommodity)
            paramFields.Add(pfCABBV)
            paramFields.Add(pfSoldTo)
            paramFields.Add(pfProgramID)
            paramFields.Add(pfProjCat)
            paramFields.Add(pfStatusID)
            paramFields.Add(pfStepID)
            paramFields.Add(pfSortBy)
            paramFields.Add(pfPCR)

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
