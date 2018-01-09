' ************************************************************************************************
' Name:	CostSavingsReport.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 07/28/2009    LRey			Created .Net application
' ************************************************************************************************
Partial Class RnD_CostSavingsReport
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Cost Savings Report"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > Cost Savings Report"
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
            ctl = m.FindControl("RnDExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If


            'focus on Vehicle List screen Program field
            txtReqDtFrom.Focus()

            RnDModule.CleanRnDCrystalReports()

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sReqDtFrom") = ""
                ViewState("sReqDtTo") = ""
                ViewState("sUGNFacility") = ""
                ViewState("sReqStatus") = ""


                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("RDCS_ReqDtFrom") Is Nothing Then
                    txtReqDtFrom.Text = Server.HtmlEncode(Request.Cookies("RDCS_ReqDtFrom").Value)
                    ViewState("sReqDtFrom") = Server.HtmlEncode(Request.Cookies("RDCS_ReqDtFrom").Value)
                End If

                If Not Request.Cookies("RDCS_ReqDtTo") Is Nothing Then
                    txtReqDtTo.Text = Server.HtmlEncode(Request.Cookies("RDCS_ReqDtTo").Value)
                    ViewState("sReqDtTo") = Server.HtmlEncode(Request.Cookies("RDCS_ReqDtTo").Value)
                End If

                If Not Request.Cookies("RDCS_UGNFacility") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("RDCS_UGNFacility").Value)
                    ViewState("sUGNFacility") = Server.HtmlEncode(Request.Cookies("RDCS_UGNFacility").Value)
                End If

                If Not Request.Cookies("RDCS_ReqStatus") Is Nothing Then
                    ddRequestStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("RDCS_ReqStatus").Value)
                    ViewState("sReqStatus") = Server.HtmlEncode(Request.Cookies("RDCS_ReqStatus").Value)
                End If

            Else
                ViewState("sReqDtFrom") = txtReqDtFrom.Text.ToString
                ViewState("sReqDtTo") = txtReqDtTo.Text.ToString
                ViewState("sUGNFacility") = ddUGNFacility.SelectedValue
                ViewState("sReqStatus") = ddRequestStatus.SelectedValue
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
            Session("TempCrystalRptFiles") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("RDCS_ReqDtFrom").Value = txtReqDtFrom.Text
            Response.Cookies("RDCS_ReqDtTo").Value = txtReqDtTo.Text
            Response.Cookies("RDCS_UGNFacility").Value = ddUGNFacility.SelectedValue
            Response.Cookies("RDCS_ReqStatus").Value = ddRequestStatus.SelectedValue

            Response.Redirect("crViewCostSavingsReport.aspx?pReqDtFrom=" & ViewState("sReqDtFrom") & "&pReqDtTo=" & ViewState("sReqDtTo") & "&pUGNFacility=" & ViewState("sUGNFacility") & "&pReqStatus=" & ViewState("sReqStatus") & "&pTestClass=3", False)
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
            RnDModule.DeleteCostSavingsReportCookies()
            Session("TempCrystalRptFiles") = Nothing

            Response.Redirect("CostSavingsReport.aspx", False)
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
