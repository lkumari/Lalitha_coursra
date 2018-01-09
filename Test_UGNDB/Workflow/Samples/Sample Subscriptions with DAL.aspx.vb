Imports System.Data
Imports System.Data.SqlClient
Partial Class Workflow_Subscriptions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc.: Custom Application"
        m.ContentLabel = "Sample Subscriptions with DAL"
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
        BindSubscriptionsData()
        'AddRow()
    End Sub
    Protected Sub BindSubscriptionsData()
        Dim subscriptionsAdapter As New SubscriptionsTableAdapters.SubscriptionsTableAdapter
        Dim Subscriptions As Subscriptions.Subscriptions_MaintDataTable

        Subscriptions = subscriptionsAdapter.GetSubscriptions("")

        ' For Each subscriptionRow As Subscriptions.Subscriptions_MaintRow In Subscriptions
        'Response.Write("Subscription: " & subscriptionRow.Subscription & "<br />")
        ' Next
        GridView1.DataSource = Subscriptions
        GridView1.DataBind()

        'GridView2.DataSource = subscriptionsAdapter.GetSubscriptionsDataByDesc("Costing")
        'GridView2.DataBind()

        'GridView3.DataSource = subscriptionsAdapter.GetSubscriptionsDataByID(4)
        'GridView3.DataBind()
    End Sub
    Protected Sub AddRow()
        Dim subscriptionsAdapter As New SubscriptionsTableAdapters.SubscriptionsTableAdapter
        'Dim Subscriptions As Subscriptions.Subscriptions_MaintDataTable
        'Dim new_subID As Integer = Convert.ToInt32(subscriptionsAdapter.InsertSubscription("Testing", "lrey"))
        'Dim new_productID As Integer = Convert.ToInt32(productsAdapter.InsertProduct("New Product", 1, 1, "12 tins per carton", 14.95, 10, 0, 10, false))


    End Sub
    'Protected Sub gvSubscriptions_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
    '    Dim temp As TextBox
    '    If (e.CommandName = "Insert") Then
    '        temp = CType(gvSubscriptions.FooterRow.FindControl("Subscription"), TextBox)
    '        dsSubscription.InsertParameters("Subscription").DefaultValue = temp.Text
    '        dsSubscription.Insert()
    '    End If
    'End Sub
End Class
