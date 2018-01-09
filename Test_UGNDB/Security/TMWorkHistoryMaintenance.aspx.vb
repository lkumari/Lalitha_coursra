Option Explicit On
Option Strict On

''*****************************************************************************
'' Name    : TMWorkHistoryMaintenance.aspx.vb
'' Purpose : Displays and updates the TeamMember_WorkHistory table.
''
'' ---Date---  --Author--  --Modifications--
'' 05/30/2008  MWeyker     Created .Net application
''
'' 06/02/2008  MWeyker     Expand this menu item on the Master Page.
''                         Override breadcrumb navigation on Master Page.
''                         Modify the Master Page PageTitle and Content Label.
''                               
'' 06/11/2008  MWeyker     Replace the Title column with a Subscription (AKA Group)
''                         column. Change the primary key in the TeamMember_WorkHistory
''                         table from (TeamMemberID, StartDate) to (TeamMemberID,
''                         SubscriptionID). Populate the Subscription column from
''                         the Subscriptions_Maint table.
''
''                         Do not allow entry of EndDate for record insert.
''                         
''*****************************************************************************

''' 
Partial Class Security_TMWorkHistoryMaintenance
    Inherits System.Web.UI.Page


#Region "Module Level Variables"
#End Region ' Module Level Variables"


#Region "Loading and Initialization"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''****************************************************
        '' Update the title and heading on the Master Page
        ''****************************************************
        Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
        m.PageTitle = "UGN, Inc."
        m.ContentLabel = "Team Members - Work History"

        ''**************************************************
        '' Override the Master Page bread crumb navigation
        ''**************************************************
        Dim ctl As Control = m.FindControl("lblOtherSiteNode")
        If ctl IsNot Nothing Then
            Dim lbl As Label = CType(ctl, Label)
            lbl.Text = _
                "<a href='../Home.aspx'><b>Home</b></a> > <b> Security </b> > Team Members"
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

        If Not IsPostBack Then
            ' Load the User lookup list.
            LoadUserLookupList()
        End If

        ''**********************************************************************
        '' Check whether previous page sent a TeamMemberID in the QueryString.
        ''**********************************************************************
        If (Not IsPostBack) AndAlso (Request.QueryString("TeamMemberId") IsNot Nothing) Then
            Dim strId As String = Request.QueryString("TeamMemberId")
            If Not strId.Equals("") Then
                ' The TeamMemberId was sent in the query string.
                ' Select it from the TeamMember drop down list.
                Try
                    ddlLookupUser.SelectedValue = strId
                    lblTeamMemberId.Text = "Id: " & ddlLookupUser.SelectedValue
                    lblTeamMemberId.Font.Italic = False
                    hfTeamMemberId.Value = ddlLookupUser.SelectedValue
                    DisplayGridView(True)
                Catch ex As Exception
                    ' EMPTY
                End Try
            End If
        End If
    End Sub

#End Region ' Loading and Initialization


#Region "Event Handlers"

    Protected Sub ddlLookupUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLookupUser.SelectedIndexChanged
        ' A different user has been selected from ddlLookupUser
        If (ddlLookupUser.SelectedIndex = -1) Or _
            (String.IsNullOrEmpty(ddlLookupUser.Text.Trim())) Then
            lblTeamMemberId.Text = "Please select a TeamMember"
            lblTeamMemberId.Font.Italic = True
            hfTeamMemberId.Value = ""
            DisplayGridView(False)
        Else
            lblTeamMemberId.Text = "Id: " & ddlLookupUser.SelectedValue
            lblTeamMemberId.Font.Italic = False
            hfTeamMemberId.Value = ddlLookupUser.SelectedValue
            DisplayGridView(True)
        End If
    End Sub

    Protected Sub gvWorkHistory_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWorkHistory.DataBound
        ' Display the current page
        lblCurrentPage.Text = String.Format("Page {0} of {1}", _
            gvWorkHistory.PageIndex + 1, gvWorkHistory.PageCount)
    End Sub

    Protected Sub gvWorkHistory_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvWorkHistory.RowCommand
        ' Don't expose more than one row for editing.
        ' When in Edit mode, hide the footer row (the insert row).
        If e.CommandName = "Edit" Then
            gvWorkHistory.ShowFooter = False
        Else
            gvWorkHistory.ShowFooter = True
        End If

        If e.CommandName = "InsertCustom" AndAlso Page.IsValid() Then
            ' Get the data for inserting a new record
            Dim gvr As GridViewRow = gvWorkHistory.FooterRow
            Dim intTeamMemberID As Integer = Integer.Parse(hfTeamMemberId.Value)
            Dim dteStartDate As DateTime = _
                DateTime.Parse(CType(gvr.FindControl("txtStartDateInsert"), TextBox).Text)
            Dim intSubscriptionID As Integer = _
                Integer.Parse(CType(gvr.FindControl("ddlSubscriptionInsert"), DropDownList).SelectedValue)
            Dim strSubscriptionName As String = _
                CType(gvr.FindControl("ddlSubscriptionInsert"), DropDownList).SelectedItem.Text
            Dim strFacility As String = CType(gvr.FindControl("ddlFacilityInsert"), DropDownList).SelectedValue
            InsertNewRow(intTeamMemberID, intSubscriptionID, strSubscriptionName, dteStartDate, strFacility)
        ElseIf e.CommandName = "Cancel" Then
        DisplayStatus("", False)
        ElseIf e.CommandName = "Edit" Then
        DisplayStatus("", False)
        End If
    End Sub

    Protected Sub gvWorkHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkHistory.RowDataBound

        ' If the current RowType is a DataRow, then attach a client script to the 
        ' Delete button's onclick event.
        '
        ' When the delete button is clicked, the script executes the following steps:
        ' 1) Save the row's original background color
        ' 2) Change the row's background color (to highlight the row)
        ' 3) Display a confirmation box to confirm the delete
        ' 4) If the confirmation cancel button was clicked, then
        '      restore the row's background color to its original.

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnDelete"), ImageButton)
            If ibtn IsNot Nothing Then
                ibtn.Attributes.Add("onclick", _
                    "this.originalcolor=this.style.backgroundColor;" & _
                    " this.parentNode.parentNode.style.backgroundColor='#9BB8D5'; " & _
                    "if (confirm('Are you sure you want to delete this entry?')) return true; " & _
                    "else {this.parentNode.parentNode.style.backgroundColor=this.originalcolor; return false;}")
            End If
        End If
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' Get the GridViewRow that contains the delete button that got clicked.
        Dim ibtn As ImageButton = CType(sender, ImageButton)
        Dim gvr As GridViewRow = CType(ibtn.NamingContainer, GridViewRow)

        ' Get the TeamMemberID and the SubscriptionID
        Dim intTeamMemberID As Integer = Integer.Parse(hfTeamMemberId.Value)
        Dim intSubscriptionID As Integer = Integer.Parse(CType(gvr.FindControl("hfSubscriptionIDPreEdit"), HiddenField).Value)
        Dim strUGNFacility As String = (CType(gvr.FindControl("hfUGNFacilityPreEdit"), HiddenField).Value)

        ' Delete the record
        Dim blnsuccess As Boolean = SecurityModule.DeleteTMWorkHistory(intTeamMemberID, intSubscriptionID, strUGNFacility)
        If blnsuccess Then
            DisplayStatus("", False)
            gvWorkHistory.DataBind()   ' Show the new collection
        Else
            DisplayStatus("Database Error - Record not deleted", True)
        End If
    End Sub

    Protected Sub ibtnUpdate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' If Page fails validation, do not update.
        If Page.IsValid = False Then
            Exit Sub
        End If

        ' Get the GridView row that contains the Update button that was clicked
        Dim ibtn As ImageButton = CType(sender, ImageButton)
        Dim gvr As GridViewRow = CType(ibtn.NamingContainer, GridViewRow)

        ' Extract the new data from the GridViewRow
        Dim intTeamMemberID As Integer = Integer.Parse(hfTeamMemberId.Value)
        Dim dteStartDate As DateTime = DateTime.Parse(CType(gvr.FindControl("txtStartDateEdit"), TextBox).Text)
        Dim strEndDate As String = CType(gvr.FindControl("txtEndDateEdit"), TextBox).Text.Trim()
        Dim dteEndDate As DateTime = Nothing
        If Not String.IsNullOrEmpty(strEndDate) Then
            ' If the End Date is not empty, save it
            dteEndDate = DateTime.Parse(CType(gvr.FindControl("txtEndDateEdit"), TextBox).Text)
        End If
        Dim strFacility As String = CType(gvr.FindControl("ddlFacilityEdit"), DropDownList).SelectedValue
        Dim intSubscriptionID As Integer = _
            Integer.Parse(CType(gvr.FindControl("hfSubscriptionIDPreEdit"), HiddenField).Value)

        ' Update the row with the new data
        Dim blnSuccess As Boolean
        If String.IsNullOrEmpty(strEndDate) Then
            ' EndDate was not supplied.
            ' Omit it
            blnSuccess = SecurityModule.UpdateTMWorkHistory(intTeamMemberID, _
                intSubscriptionID, dteStartDate, Nothing, strFacility)
        Else
            ' Use the EndDate
            blnSuccess = SecurityModule.UpdateTMWorkHistory(intTeamMemberID, _
                intSubscriptionID, dteStartDate, dteEndDate, strFacility)
        End If

        If blnSuccess = True Then
            ' Update was successful.
            ' Take the row out of Edit mode,
            '   show the footer row, and
            '   rebind the data.
            DisplayStatus("")
            gvWorkHistory.EditIndex = -1
            gvWorkHistory.ShowFooter = True
            gvWorkHistory.DataBind()
        Else
            DisplayStatus("Database error. Update failed", True)
        End If
    End Sub

    Protected Sub lbTMGeneralTab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTMGeneralTab.Click
        RedirectPage("TMGeneralMaintenance.aspx")
    End Sub

    Protected Sub lbTMRolesTab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTMRolesTab.Click
        RedirectPage("TMRoleFormMaintenance.aspx")
    End Sub

#End Region ' Event Handlers


#Region "Private Methods"

    ''' <summary>
    ''' Sets the visibility of the GridView control.
    ''' </summary>
    ''' <param name="MakeVisible">True to make visible; False to make invisible.</param>
    ''' <remarks>This feature is used to make the GridView visible when a user
    ''' is selected, and invisible when no user is selected.</remarks>
    Private Sub DisplayGridView(ByVal MakeVisible As Boolean)
        gvWorkHistory.Enabled = MakeVisible
        gvWorkHistory.Visible = MakeVisible
        lblCurrentPage.Visible = MakeVisible
    End Sub

    ''' <summary>
    ''' Display a message in the lblStatus label
    ''' </summary>
    ''' <param name="Message">The message to be displayed</param>
    ''' <param name="IsError">True if message is an error, otherwise false</param>
    ''' <remarks></remarks>
    Private Sub DisplayStatus(ByVal Message As String, Optional ByVal IsError As Boolean = False)
        lblStatus.Text = Message
        If IsError Then
            ' Display message as an error
            lblStatus.ForeColor = Color.Red
        Else
            ' Display normal message
            lblStatus.ForeColor = Color.DarkGreen
        End If
    End Sub

    ''' <summary>
    ''' Inserts a new WorkHistory row.
    ''' </summary>
    ''' <param name="TeamMemberID"></param>
    ''' <param name="SubscriptionID"></param>
    ''' <param name="SubscriptionName"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="UGNFacility"></param>
    ''' <remarks>This method checks for duplicates before allowing the insert.</remarks>
    Private Sub InsertNewRow(ByVal TeamMemberID As Integer, _
        ByVal SubscriptionID As Integer, _
        ByVal SubscriptionName As String, _
        ByVal StartDate As DateTime, _
        ByVal UGNFacility As String)

        ' Determine if a record already exists with this TeamMemberID and SubscriptionID
        ' ''Dim intRecordCount As Integer = SecurityModule.GetTMWorkHistoryCount(TeamMemberID, SubscriptionID)

        ' ''If intRecordCount <> 0 Then
        ' ''    ' A record already exists.
        ' ''    ' Don't allow the insert.
        ' ''    DisplayStatus("Warning: Duplicate Group " & _
        ' ''        SubscriptionName & " was not added to WorkHistory", True)
        ' ''    Exit Sub
        ' ''End If

        ' Insert the record
        Dim blnSuccess As Boolean = SecurityModule.InsertTMWorkHistory(TeamMemberID, _
            SubscriptionID, StartDate, UGNFacility)

        If blnSuccess Then
            DisplayStatus("")
            gvWorkHistory.DataBind()      ' Refresh gvWorkHistory after adding new record
        Else
            DisplayStatus("Error adding Work History", True)
        End If
    End Sub

    ''' <summary>
    ''' Populates the Username drop down list
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserLookupList()
        Try
            With ddlLookupUser
                .DataSource = SecurityModule.GetTeamMember(Nothing, _
                    Nothing, Nothing, Nothing, Nothing, _
                    Nothing, Nothing, Nothing)
                .DataTextField = "UserName"
                .DataValueField = "TeamMemberID"
                .DataBind()
                .Items.Insert(0, "")
                .SelectedIndex = -1
            End With
            lblTeamMemberId.Text = "Please select a Team Member"
            lblTeamMemberId.Font.Italic = True
            DisplayGridView(False)
            ddlLookupUser.Focus()
        Catch ex As Exception
            DisplayStatus("Database Error", True)
        End Try
    End Sub

    ''' <summary>
    ''' Redirects to new page with confirmation.
    ''' </summary>
    ''' <param name="PageName">Name of next page</param>
    ''' <remarks>Posts a confirmation popup if there are unsaved changes.</remarks>
    Private Sub RedirectPage(ByVal PageName As String)
        Dim strQueryString As String = ""

        If Not hfTeamMemberId.Value.Equals("") Then
            ' Send the current TeamMemberId and Username to the next page
            strQueryString = "?TeamMemberId=" & hfTeamMemberId.Value & _
                "&UserName=" & ddlLookupUser.SelectedItem.Text
        End If

        Response.Redirect(PageName & strQueryString)
    End Sub

#End Region ' Private Methods


#Region "Insert Empty GridView Work-Around"

    ''*****************************************************
    '' Empty GridView Work-Around:
    '' When a GridView has no data to bind to, it does
    '' not display any rows, which makes it impossible to
    '' for the user to enter new data.
    '' 
    '' This work-around displays an empty GridView 
    '' FooterRow, when there is no data to display,
    '' which makes it possible to enter new data.
    ''*****************************************************

    Private Property LoadDataEmpty() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty") = value
        End Set
    End Property

    Protected Sub gvWorkHistory_RowCreated(ByVal sender As Object, _
        ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkHistory.RowCreated

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
        ' when binding a row, look for a zero row condition based on the flag.
        ' if we have zero data rows (but a dummy row), hide the grid view row
        ' and clear the controls off of that row so they don't cause binding errors

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub

    Protected Sub odsWorkHistory_Selected(ByVal sender As Object, _
        ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsWorkHistory.Selected
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        Dim dt As DataTable = ds.Tables(0)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty = True
        Else
            LoadDataEmpty = False
        End If
    End Sub
#End Region 'Insert Empty GridView Work-Around

End Class
