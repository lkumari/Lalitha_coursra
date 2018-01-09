Imports System.Data
Imports System.Data.SqlClient
Partial Class Workflow_Sample_Subscriptions_with_BLL
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc.: Custom Application"
        m.ContentLabel = "Sample Subscriptions with BLL"
        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > Subscriptions"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

        BindSubcriptionsData()

        ' Testing UpdateSubscription error trapping.
        Dim subscriptionsLogic As New SubscriptionsBLL
        Try
            subscriptionsLogic.UpdateSubscription(34, "", False, "lrey")
        Catch ex As Exception

            MsgBox(ex.Message)

        End Try
    End Sub
    Protected Sub BindSubcriptionsData()
        Dim subscriptionsLogic As New SubscriptionsBLL
        GridView1.DataSource = subscriptionsLogic.GetSubscriptions("")
        GridView1.DataBind()
    End Sub
End Class
