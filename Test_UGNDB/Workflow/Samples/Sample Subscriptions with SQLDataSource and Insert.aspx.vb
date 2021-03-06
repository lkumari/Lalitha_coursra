Imports System.Data
Imports System.Data.SqlClient
Partial Class Workflow_Subscriptions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc.: Custom Application"
        m.ContentLabel = "Subscriptions"
        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        ''Dim mpContentPlaceHolder As ContentPlaceHolder
        Dim mpTextBox As Label
        ''mpContentPlaceHolder = CType(Master.FindControl("headerPlaceHolder"), ContentPlaceHolder)
        ''If Not mpContentPlaceHolder Is Nothing Then
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > Subscriptions"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If
        '' End If
    End Sub
    Protected Sub gvSubscriptions_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Dim temp As TextBox
        If (e.CommandName = "Insert") Then
            temp = CType(gvSubscriptions.FooterRow.FindControl("Subscription"), TextBox)
            dsSubscription.InsertParameters("Subscription").DefaultValue = temp.Text
            dsSubscription.Insert()
        End If
    End Sub
End Class
