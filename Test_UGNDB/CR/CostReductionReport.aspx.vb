' ************************************************************************************************
' Name:	CostReductionReport.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 02/02/2010    LRey		Created .Net application
' 03/08/2010    RCarlson    Modified: Needed to make sure Project Leader TMID was passed to Crystal Report, put more dataset checking in BindCriteria
' 01/08/2014    LRey        Replaced GetCustomer with GetOEMManufacturer. SOLDTO|CABBV is not used in the ERP.
' ************************************************************************************************
Partial Class CR_CostReductionReport
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Cost Reduction Report"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                'lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Cost Reduction</b> > Cost Reduction Report"
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Cost Reduction</b> > <a href='CostReductionList.aspx'><b>Cost Reduction Project Search</b></a>"
                lbl.Visible = True
            End If

            ctl = m.FindControl("SiteMapPath1")
            If ctl IsNot Nothing Then
                Dim smp As SiteMapPath = CType(ctl, SiteMapPath)
                smp.Visible = False
            End If

            ''******************************************
            '' Expand this Master Page menu item
            ''******************************************
            ctl = m.FindControl("CRExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If


            'focus on Vehicle List screen Program field
            txtImpDtFrom.Focus()

            CRModule.CleanCRCrystalReports()

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sImpDtFrom") = ""
                ViewState("sImpDtTo") = ""
                ViewState("sFac") = ""
                ViewState("sLTMID") = 0
                ViewState("sCID") = 0
                ViewState("sPCID") = 0
                ViewState("sSort") = ""
                ViewState("sCABBV") = ""
                ViewState("sSoldTo") = 0
                ViewState("sPgm") = 0
                ViewState("sPCR") = Nothing
                ViewState("sRptFrmt") = Nothing

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("CR_ImpDtFrom") Is Nothing Then
                    txtImpDtFrom.Text = Server.HtmlEncode(Request.Cookies("CR_ImpDtFrom").Value)
                    ViewState("sImpDtFrom") = Server.HtmlEncode(Request.Cookies("CR_ImpDtFrom").Value)
                End If

                If Not Request.Cookies("CR_ImpDtTo") Is Nothing Then
                    txtImpDtTo.Text = Server.HtmlEncode(Request.Cookies("CR_ImpDtTo").Value)
                    ViewState("sImpDtTo") = Server.HtmlEncode(Request.Cookies("CR_ImpDtTo").Value)
                End If

                If Not Request.Cookies("CR_Fac") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_Fac").Value)
                    ViewState("sFac") = Server.HtmlEncode(Request.Cookies("CR_Fac").Value)
                End If

                If Not Request.Cookies("CR_LTMID") Is Nothing Then
                    ddLeader.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_LTMID").Value)
                    ViewState("sLTMID") = Server.HtmlEncode(Request.Cookies("CR_LTMID").Value)
                End If

                If Not Request.Cookies("CR_CID") Is Nothing Then
                    ddCommodity.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_CID").Value)
                    ViewState("sCID") = Server.HtmlEncode(Request.Cookies("CR_CID").Value)
                End If

                If Not Request.Cookies("CR_PCID") Is Nothing Then
                    ddProjectCategory.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_PCID").Value)
                    ViewState("sPCID") = Server.HtmlEncode(Request.Cookies("CR_PCID").Value)
                End If

                If Not Request.Cookies("CR_Sort") Is Nothing Then
                    ddSortBy.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_Sort").Value)
                    ViewState("sSort") = Server.HtmlEncode(Request.Cookies("CR_Sort").Value)
                End If

                If (Not Request.Cookies("CR_CABBV") Is Nothing) And (Not Request.Cookies("CR_SoldTo") Is Nothing) Then
                    ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_SoldTo").Value) & "|" & Server.HtmlEncode(Request.Cookies("CR_CABBV").Value)
                    ViewState("sCABBV") = Server.HtmlEncode(Request.Cookies("CR_CABBV").Value)
                    ViewState("sSoldTo") = Server.HtmlEncode(Request.Cookies("CR_SoldTo").Value)
                End If

                If Not Request.Cookies("CR_Pgm") Is Nothing Then
                    ddProgram.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_Pgm").Value)
                    ViewState("sPgm") = Server.HtmlEncode(Request.Cookies("CR_Pgm").Value)
                End If

                If Not Request.Cookies("CR_PCR") Is Nothing Then
                    ddPCR.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_PCR").Value)
                    ViewState("sPCR") = Server.HtmlEncode(Request.Cookies("CR_PCR").Value)
                End If

                If Not Request.Cookies("CR_Rpt") Is Nothing Then
                    ddReportFormat.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_Rpt").Value)
                    ViewState("sRptFrmt") = Server.HtmlEncode(Request.Cookies("CR_Rpt").Value)
                End If


            Else
                ViewState("sImpDtFrom") = txtImpDtFrom.Text.ToString
                ViewState("sImpDtTo") = txtImpDtTo.Text.ToString
                ViewState("sFac") = ddUGNFacility.SelectedValue
                ViewState("sLTMID") = ddLeader.SelectedValue
                ViewState("sCID") = ddCommodity.SelectedValue
                ViewState("sPCID") = ddProjectCategory.SelectedValue
                ViewState("sSort") = ddSortBy.SelectedValue
                ViewState("sPgm") = ddProgram.SelectedValue
                ViewState("sPCR") = ddPCR.SelectedValue
                ViewState("sRptFrmt") = ddReportFormat.SelectedValue

                Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                If Not (Pos = 0) Then
                    ViewState("sCABBV") = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                    ViewState("sSoldTo") = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
                End If
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Project Leader control for selection criteria for search
        ds = commonFunctions.GetTeamMember("")
        If commonFunctions.CheckDataset(ds) = True Then
            ddLeader.DataSource = ds
            ddLeader.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddLeader.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
            ddLeader.DataBind()
            ddLeader.Items.Insert(0, "")
        End If

        ''bind existing data to drop down UGN Location control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If commonFunctions.CheckDataset(ds) = True Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Commodity control for selection criteria 
        ds = commonFunctions.GetCommodity(0, "", "", 0)
        If commonFunctions.CheckDataset(ds) = True Then
            ddCommodity.DataSource = ds
            ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
            ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
            ddCommodity.DataBind()
            ddCommodity.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Project Category control for selection criteria for search
        ds = CRModule.GetProjectCategory("")
        If commonFunctions.CheckDataset(ds) = True Then
            ddProjectCategory.DataSource = ds
            ddProjectCategory.DataTextField = ds.Tables(0).Columns("ddProjectCategoryName").ColumnName.ToString()
            ddProjectCategory.DataValueField = ds.Tables(0).Columns("PCID").ColumnName.ToString()
            ddProjectCategory.DataBind()
            ddProjectCategory.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = commonFunctions.GetOEMManufacturer("")
        If commonFunctions.CheckDataSet(ds) = True Then
            ddCustomer.DataSource = ds
            ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            ddCustomer.DataBind()
            ddCustomer.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Program control for selection criteria for search
        ds = commonFunctions.GetProgram("", "", "")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProgram.DataSource = ds
            ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
            ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
            ddProgram.DataBind()
            ddProgram.Items.Insert(0, "")
        End If
    End Sub 'EOF BindCriteria

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            Session("TempCrystalRptFiles") = Nothing
            'set saved value of what criteria was used to search        
            Response.Cookies("CR_ImpDtFrom").Value = txtImpDtFrom.Text.ToString
            Response.Cookies("CR_ImpDtTo").Value = txtImpDtTo.Text.ToString
            Response.Cookies("CR_Fac").Value = ddUGNFacility.SelectedValue
            Response.Cookies("CR_LMTID").Value = ddLeader.SelectedValue
            Response.Cookies("CR_CID").Value = ddCommodity.SelectedValue
            Response.Cookies("CR_PCID").Value = ddProjectCategory.SelectedValue
            Response.Cookies("CR_Sort").Value = ddSortBy.SelectedValue
            Response.Cookies("CR_Pgm").Value = ddProgram.SelectedValue
            Response.Cookies("CR_PCR").Value = ddPCR.SelectedValue
            Response.Cookies("CR_Rpt").Value = ddReportFormat.SelectedValue

            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            If Not (Pos = 0) Then
                Response.Cookies("CR_CABBV").Value = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                Response.Cookies("CR_SoldTo").Value = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If

            If ViewState("sRptFrmt") = "Detail" Then
                Response.Redirect("crViewCostReductionReport.aspx?pImpDtFrom=" & ViewState("sImpDtFrom") & "&pImpDtTo=" & ViewState("sImpDtTo") & "&pUGNFacility=" & ViewState("sFac") & "&pLeader=" & ViewState("sLTMID") & "&pCommodity=" & ViewState("sCID") & "&pCABBV=" & ViewState("sCABBV") & "&pSoldTo=" & ViewState("sSoldTo") & "&pPGM=" & ViewState("sPGM") & "&pProjCat=" & ViewState("sPCID") & "&pSortBy=" & ViewState("sSort") & "&pPCR=" & ViewState("sPCR"), False)
            ElseIf ViewState("sRptFrmt") = "Summary" Then
                Response.Redirect("crViewCostReductionReportSummary.aspx?pImpDtFrom=" & ViewState("sImpDtFrom") & "&pImpDtTo=" & ViewState("sImpDtTo") & "&pUGNFacility=" & ViewState("sFac") & "&pLeader=" & ViewState("sLTMID") & "&pCommodity=" & ViewState("sCID") & "&pCABBV=" & ViewState("sCABBV") & "&pSoldTo=" & ViewState("sSoldTo") & "&pPGM=" & ViewState("sPGM") & "&pProjCat=" & ViewState("sPCID") & "&pSortBy=" & ViewState("sSort") & "&pPCR=" & ViewState("sPCR"), False)

            ElseIf ViewState("sRptFrmt") = "Daily" Then
                Response.Redirect("crViewDailyCostReductionReport.aspx?pImpDtFrom=" & ViewState("sImpDtFrom") & "&pImpDtTo=" & ViewState("sImpDtTo") & "&pUGNFacility=" & ViewState("sFac") & "&pLeader=" & ViewState("sLTMID") & "&pCommodity=" & ViewState("sCID") & "&pCABBV=" & ViewState("sCABBV") & "&pSoldTo=" & ViewState("sSoldTo") & "&pPGM=" & ViewState("sPGM") & "&pProjCat=" & ViewState("sPCID") & "&pSortBy=" & ViewState("sSort") & "&pPCR=" & ViewState("sPCR"), False)

            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            CRModule.DeleteCostReductionReportCookies()
            Session("TempCrystalRptFiles") = Nothing

            Response.Redirect("CostReductionReport.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

End Class
