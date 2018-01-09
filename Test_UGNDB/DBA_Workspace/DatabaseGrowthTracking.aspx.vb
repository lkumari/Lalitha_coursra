' ************************************************************************************************
' Name:	DatabaseGrowthTracking.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 06/02/2009    LRey			Created .Net application
' ************************************************************************************************
Partial Class DBA_Workspace_DatabaseGrowthTracking
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Database Growth Tracking"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Security - DBA Workspace </b> > Database Growth Tracking"
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
            ctl = m.FindControl("SECExtender")
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
                ViewState("sDtRecFrom") = ""
                ViewState("sDtRecTo") = ""
                ViewState("sServerName") = ""

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******

                If Not Request.Cookies("DBA_DtRecFrom") Is Nothing Then
                    txtReqDtFrom.Text = Server.HtmlEncode(Request.Cookies("DBA_DtRecFrom").Value)
                    ViewState("sDtRecFrom") = Server.HtmlEncode(Request.Cookies("DBA_DtRecFrom").Value)
                End If

                If Not Request.Cookies("DBA_DtRecTo") Is Nothing Then
                    txtReqDtTo.Text = Server.HtmlEncode(Request.Cookies("DBA_DtRecTo").Value)
                    ViewState("sDtRecTo") = Server.HtmlEncode(Request.Cookies("DBA_DtRecTo").Value)
                End If

                If Not Request.Cookies("DBA_ServerName") Is Nothing Then
                    ddServerName.SelectedValue = Server.HtmlEncode(Request.Cookies("DBA_ServerName").Value)
                    ViewState("sServerName") = Server.HtmlEncode(Request.Cookies("DBA_ServerName").Value)
                End If

            Else
                ViewState("sDtRecFrom") = txtReqDtFrom.Text.ToString
                ViewState("sDtRecTo") = txtReqDtTo.Text.ToString
                ViewState("sServerName") = ddServerName.SelectedValue
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

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            HttpContext.Current.Session("sessionDBACurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("DBA_DtRecFrom").Value = txtReqDtFrom.Text
            Response.Cookies("DBA_DtRecTo").Value = txtReqDtTo.Text
            Response.Cookies("DBA_ServerName").Value = ddServerName.SelectedValue

            Response.Redirect("crViewDatabaseGrowthTracking.aspx?pDtRecFrom=" & ViewState("sDtRecFrom") & "&pDtRecTo=" & ViewState("sDtRecTo") & "&pServerName=" & ViewState("sServerName"), False)

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
            DBAModule.DeleteDatabaseGrowthTrackingCookies()
            HttpContext.Current.Session("sessionDBACurrentPage") = Nothing

            Response.Redirect("DatabaseGrowthTracking.aspx", False)
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
