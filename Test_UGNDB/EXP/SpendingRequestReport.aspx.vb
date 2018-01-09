' ************************************************************************************************
' Name:	SpendingRequestReport.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 03/05/2012    LRey			Created .Net application
' ************************************************************************************************
Partial Class EXP_SpendingRequestReport
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Spending Request Report"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Request</b> > Spending Request Report"
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
            ctl = m.FindControl("SPRExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If


            'focus on Vehicle List screen Program field
            ddSRType.Focus()

            EXPModule.CleanExpCrystalReports()

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindCriteria()

                ViewState("sSRType") = ""
                ViewState("sUGNFac") = ""
                ViewState("sProjStat") = ""
                ViewState("sFromDate") = ""
                ViewState("sToDate") = ""

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******

                If Not Request.Cookies("SR_SRType") Is Nothing Then
                    ddSRType.SelectedValue = Server.HtmlEncode(Request.Cookies("SR_SRType").Value)
                    ViewState("sSRType") = Server.HtmlEncode(Request.Cookies("SR_SRType").Value)
                End If

                If Not Request.Cookies("SR_UGNFac") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("SR_UGNFac").Value)
                    ViewState("sUGNFac") = Server.HtmlEncode(Request.Cookies("SR_UGNFac").Value)
                End If

                If Not Request.Cookies("SR_ProjStat") Is Nothing Then
                    ddProjectStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("SR_ProjStat").Value)
                    ViewState("sProjStat") = Server.HtmlEncode(Request.Cookies("SR_ProjStat").Value)
                End If

                If Not Request.Cookies("SR_FromDate") Is Nothing Then
                    txtFromDate.Text = Server.HtmlEncode(Request.Cookies("SR_FromDate").Value)
                    ViewState("sFromDate") = Server.HtmlEncode(Request.Cookies("SR_FromDate").Value)
                End If

                If Not Request.Cookies("SR_ToDate") Is Nothing Then
                    txtToDate.Text = Server.HtmlEncode(Request.Cookies("SR_ToDate").Value)
                    ViewState("sToDate") = Server.HtmlEncode(Request.Cookies("SR_ToDate").Value)
                End If

            Else
                ViewState("sSRType") = ddSRType.SelectedValue
                ViewState("sUGNFac") = ddUGNFacility.SelectedValue
                ViewState("sProjStat") = ddProjectStatus.SelectedValue
                ViewState("sFromDate") = txtFromDate.Text
                ViewState("sToDate") = txtToDate.Text
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
        ''bind existing data to drop down UGN Location control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

    End Sub 'EOF BindCriteria
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            HttpContext.Current.Session("sessionDBACurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("SR_SRType").Value = ddSRType.SelectedValue
            Response.Cookies("SR_UGNFac").Value = ddUGNFacility.SelectedValue
            Response.Cookies("SR_ProjStat").Value = ddProjectStatus.SelectedValue
            Response.Cookies("SR_FromDate").Value = txtFromDate.Text
            Response.Cookies("SR_ToDate").Value = txtToDate.Text
            EXPModule.DeleteSpendingRequestReportCookies()

            Response.Redirect("crViewSpendingRequestReport.aspx?pSRType=" & ViewState("sSRType") & "&pUGNfac=" & ViewState("sUGNFac") & "&pProjStat=" & ViewState("sProjStat") & "&pFDt=" & ViewState("sFromDate") & "&pTDt=" & ViewState("sToDate"), False)

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
            EXPModule.DeleteSpendingRequestReportCookies()
            HttpContext.Current.Session("sessionSRCurrentPage") = Nothing

            Response.Redirect("SpendingRequestReport.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Protected Sub ddProjectStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProjectStatus.SelectedIndexChanged

        If ddProjectStatus.SelectedValue = "Approved" Or _
        ddProjectStatus.SelectedValue = "Completed" Or _
        ddProjectStatus.SelectedValue = "Closed" Or _
        ddProjectStatus.SelectedValue = "Capitalized" Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            imgFromDate.Visible = True
            imgToDate.Visible = True
            revFromDate.Enabled = True
            revToDate.Enabled = True
            txtFromDate.Focus()
        Else
            lblFromDate.Visible = False
            lblToDate.Visible = False
            txtFromDate.Visible = False
            txtToDate.Visible = False
            imgFromDate.Visible = False
            imgToDate.Visible = False
            revFromDate.Enabled = False
            revToDate.Enabled = False

        End If
    End Sub
End Class
