Imports System.Data
Imports System.Data.SqlClient
Partial Class Workflow_Sample_Subscriptions_with_ObjectDataSource
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc.: Custom Application"
        m.ContentLabel = "Sample Subscriptions using ObjectDataSource"
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
    End Sub
    Protected Sub gvSubscriptions_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        ''***
        ''This section allows the inserting of a new row when called by the OnInserting event call.
        ''***
        Dim temp As TextBox
        If (e.CommandName = "Insert") Then
            temp = CType(gvSubscriptions.FooterRow.FindControl("newSubscription"), TextBox)
            odsSubscriptions.InsertParameters("Subscription").DefaultValue = temp.Text
            odsSubscriptions.Insert()
        End If
    End Sub
    Protected Sub gvSubscriptions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubscriptions.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim db As ImageButton = CType(e.Row.Cells(3).Controls(3), ImageButton)

            ' Get information about the product bound to the row
            If db.CommandName = "Delete" Then
                Dim subscription As Subscriptions.Subscriptions_MaintRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Subscriptions.Subscriptions_MaintRow)

                db.OnClientClick = String.Format("return confirm('Are you certain you want to delete:  " & DataBinder.Eval(e.Row.DataItem, "Subscription") & "?');")
            End If
        End If
    End Sub
End Class
