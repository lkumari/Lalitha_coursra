' ************************************************************************************************
' Name:	PartMaintenance.aspx.vb
' Purpose:	This program is used to view the Bill of Materials as a Tree View.
'
' Date		    Author	    
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 10/03/2008    RCarlson            Added Security Role Select Statement
' 12/10/2008    LRey                Commented out the CheckRights function.  This page is readonly.
' 02/02/2009    RCarlson            Removed Previous Part Number
' 12/18/2013    LRey                Replaced "BPCSPartNo" to "PartNo" wherever used. 

Partial Class DataMaintenance_PartMaintenance
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Internal Part Numbers"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Internal Part Numbers"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then

                If Request.QueryString("PartNo") IsNot Nothing Then
                    txtPartNoSearch.Text = Server.UrlDecode(Request.QueryString("PartNo").ToString)
                End If

                If Request.QueryString("PartName") IsNot Nothing Then
                    txtPartNameSearch.Text = Server.UrlDecode(Request.QueryString("PartName").ToString)
                End If

                If Request.QueryString("DrawingNo") IsNot Nothing Then
                    txtDrawingNoSearch.Text = Server.UrlDecode(Request.QueryString("DrawingNo").ToString)
                End If

                'If Request.QueryString("DesignationType") IsNot Nothing Then
                '    ddDesignationTypeSearch.SelectedValue = Server.UrlDecode(Request.QueryString("DesignationType").ToString)
                'End If

                If Request.QueryString("ActiveType") IsNot Nothing Then
                    ddActiveTypeSearch.SelectedValue = Server.UrlDecode(Request.QueryString("ActiveType").ToString)
                End If

            End If

            ' ''CheckRights()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Redirect("PartMaintenance.aspx?PartNo=" & Server.UrlEncode(txtPartNoSearch.Text.Trim) & "&PartName=" & Server.UrlEncode(txtPartNameSearch.Text.Trim) & "&DrawingNo=" & Server.UrlEncode(txtDrawingNoSearch.Text.Trim) & "&DesignationType=" & "" & "&ActiveType=" & Server.UrlEncode(ddActiveTypeSearch.SelectedValue), False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            Response.Redirect("PartMaintenance.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
