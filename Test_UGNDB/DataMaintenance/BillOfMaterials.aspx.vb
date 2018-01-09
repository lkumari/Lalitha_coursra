' ************************************************************************************************
' Name:	BillOfMaterials.aspx.vb
' Purpose:	This program is used to view the Bill of Materials and can call the Bill of Materials Tree View
'
' Date		    Author	    
' 04/2008       Roderick Carlson			Created .Net application
' 07/22/2008    Roderick Carlson            Cleaned Up Error Trapping
' 11/18/2012    Roderick Carlson            Turn off link to Child Part Tree
' 12/18/2013    LRey                        Replaced "BPCSPartNo" to "PartNo" wherever used. 


Partial Class DataMaintenance_BillOfMaterials
    Inherits System.Web.UI.Page    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'NO SECURITY NEEDED SINCE ALL FIELDS ARE FOR SEARCHING OR DISPLAYING

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Bill Of Materials"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Bill Of Materials"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then

                If Request.QueryString("PartNo") IsNot Nothing Then
                    txtPartNoSearch.Text = Server.UrlDecode(Request.QueryString("PartNo").ToString)
                End If

                If Request.QueryString("SubPartNo") IsNot Nothing Then
                    txtSubPartNoSearch.Text = Server.UrlDecode(Request.QueryString("SubPartNo").ToString)
                End If
            End If
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
            Response.Redirect("BillOfMaterials.aspx?PartNo=" & Server.UrlEncode(txtPartNoSearch.Text.Trim) & "&SubPartNo=" & Server.UrlEncode(txtSubPartNoSearch.Text.Trim), False)
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
            Response.Redirect("BillOfMaterials.aspx", False)
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
