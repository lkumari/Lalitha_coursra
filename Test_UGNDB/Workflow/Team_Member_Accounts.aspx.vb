''******************************************************************************************************
''* Team_Member_Backup.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new Team_Member_Backup data.
''*
''* Author  : LRey 05/28/2008
''* Modified: LRey 08/18/2008 Added SoldTo to functions
''* Modified: RCarlson 08/29/2012 Adjusted gvWorkFlow_RowDataBound

''******************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Partial Class Workflow_Team_Member_Accounts
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Team Member Accounts"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Work Flow</b> > Team Member Accounts"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("WFExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            'focus on Vehicle List screen Program field
            ddTeamMember.Focus()

            If Not Page.IsPostBack Then
                ViewState("sTeamMember") = ""

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("WFA_TeamMember") Is Nothing Then
                    ddTeamMember.SelectedValue = Server.HtmlEncode(Request.Cookies("WFA_TeamMember").Value)
                    ViewState("sTeamMember") = Server.HtmlEncode(Request.Cookies("WFA_TeamMember").Value)
                End If

            Else
                ViewState("sTeamMember") = ddTeamMember.SelectedValue
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub
#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        ''** To disable gridview ibtnDelete control, initialize Visible='<%# ViewState("ObjectRole")%>' in aspx page
        ''** Make sure to check ViewState in gvWorkFlow_RowCommand events for Edit.

        Try
            ''*******
            '' Disable controls by default
            ''*******
            gvWorkFlow.Columns(3).Visible = False
            gvWorkFlow.ShowFooter = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 31 'Team Member Accounts form id
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        gvWorkFlow.Columns(3).Visible = True
                                        gvWorkFlow.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        gvWorkFlow.Columns(3).Visible = True
                                        gvWorkFlow.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        gvWorkFlow.Columns(3).Visible = True
                                        gvWorkFlow.ShowFooter = True
                                        ViewState("ObjectRole") = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        gvWorkFlow.Columns(3).Visible = False
                                        gvWorkFlow.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        gvWorkFlow.Columns(3).Visible = True
                                        gvWorkFlow.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        gvWorkFlow.Columns(3).Visible = False
                                        gvWorkFlow.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub
#End Region
    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Team Member control for selection criteria for search
        ds = commonFunctions.GetTeamMember("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddTeamMember.DataSource = ds
            ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            ddTeamMember.DataBind()
            ddTeamMember.Items.Insert(0, "")
        End If

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ''******
        '' Store search parameters
        ''******
        Response.Cookies("WFA_TeamMember").Value = ddTeamMember.SelectedValue

        ''******
        '' Redirect to the Team Member Backups List page
        ''******
        Response.Redirect("Team_Member_Accounts.aspx?sTeamMember=" & ddTeamMember.SelectedValue, True)

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ddTeamMember.SelectedValue = ""

        ''******
        '' Delete cookies in search parameters.
        ''******
        WorkFlowModule.DeleteWFCookies_TeamMemberAssignments()

        ''******
        '' Redirect to the Team Member Backups List page
        ''******
        Response.Redirect("Team_Member_Accounts.aspx", True)
    End Sub

    Protected Sub gvWorkFlow_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvWorkFlow.RowCommand

        Dim TeamMember As DropDownList
        Dim CABBV As DropDownList

        ''***
        ''This section allows the inserting of a new row when called by the OnInserting event call.
        ''***
        If e.CommandName = "Insert" Then
            ''Insert data

            If gvWorkFlow.Rows.Count = 0 Then
                '' We are inserting through the DetailsView in the EmptyDataTemplate
                Return
            End If

            '' Only perform the following logic when inserting through the footer
            TeamMember = CType(gvWorkFlow.FooterRow.FindControl("ddTeamMember"), DropDownList)
            odsWorkFlow.InsertParameters("TeamMemberID").DefaultValue = TeamMember.SelectedValue

            CABBV = CType(gvWorkFlow.FooterRow.FindControl("ddCABBV"), DropDownList)
            Dim Pos As Integer = InStr(CABBV.SelectedValue, "|")
            Dim tempCABBV As String = Nothing
            Dim tempSoldTo As Integer = Nothing
            If Not (Pos = 0) Then
                tempCABBV = Microsoft.VisualBasic.Right(CABBV.SelectedValue, Len(CABBV.SelectedValue) - Pos)
                tempSoldTo = Microsoft.VisualBasic.Left(CABBV.SelectedValue, Pos - 1)
            End If
            'odsWorkFlow.InsertParameters("CABBV").DefaultValue = CABBV.SelectedValue
            odsWorkFlow.InsertParameters("CABBV").DefaultValue = tempCABBV
            odsWorkFlow.InsertParameters("SoldTo").DefaultValue = tempSoldTo

            odsWorkFlow.Insert()
        End If

        ''***
        ''This section allows show/hides the footer row when the Edit control is clicked
        ''***
        If e.CommandName = "Edit" Then
            gvWorkFlow.ShowFooter = False
        Else
            If ViewState("ObjectRole") = True Then
                gvWorkFlow.ShowFooter = True
            Else
                gvWorkFlow.ShowFooter = False
            End If
        End If

        ''***
        ''This section clears out the values in the footer row
        ''***
        If e.CommandName = "Undo" Then
            TeamMember = CType(gvWorkFlow.FooterRow.FindControl("ddTeamMember"), DropDownList)
            TeamMember.ClearSelection()
            TeamMember.Items.Add("")
            TeamMember.SelectedValue = ""

            CABBV = CType(gvWorkFlow.FooterRow.FindControl("ddCABBV"), DropDownList)
            CABBV.ClearSelection()
            CABBV.Items.Add("")
            CABBV.SelectedValue = ""
        End If
    End Sub

    Protected Sub gvWorkFlow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkFlow.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ' reference the Delete ImageButtone
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("ibtnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                'Dim db As ImageButton = CType(e.Row.Cells(3).Controls(3), ImageButton)
                Dim db As ImageButton
                If e.Row.Cells(3).Controls(3) IsNot Nothing Then
                    db = CType(e.Row.Cells(3).Controls(3), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim TeamMember As WorkFlow.WorkFlow_AssignmentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, WorkFlow.WorkFlow_AssignmentsRow)

                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete Team Member (" & DataBinder.Eval(e.Row.DataItem, "TeamMemberName") & ")  Customer (" & DataBinder.Eval(e.Row.DataItem, "ddCustomerValue") & ")?');")
                    End If
                End If
            End If
        End If
    End Sub

#Region "Insert Empty GridView Work-Around"

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

    Protected Sub odsWorkFlow_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsWorkFlow.Selected
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        'Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        'Dim dt As DataTable = ds.Tables(0)
        Dim dt As WorkFlow.WorkFlow_AssignmentsDataTable = CType(e.ReturnValue, WorkFlow.WorkFlow_AssignmentsDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty = True
            Else
                LoadDataEmpty = False
            End If
        End If

    End Sub

    Protected Sub gvWorkFlow_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkFlow.RowCreated
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

#End Region ' Insert Empty GridView Work-Around

End Class
