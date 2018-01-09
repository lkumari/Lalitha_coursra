''******************************************************************************************************
''* Team_Member_Family_Purchasing.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new Team_Member_Family_Purchasing data.
''*
''* Author  : Roderick Carlson 12/14/2009
''* Modified: 09/22/2010 - LRey - Completed page development
''******************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Partial Class Workflow_Team_Member_Family_Purchasing
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Team Member Family Accounts"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Work Flow</b> > Team Member Family Accounts"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("WFExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            'focus on Vehicle List screen Program field
            ddTeamMember.Focus()

            If Not Page.IsPostBack Then
                ViewState("sTM") = ""

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("WFFA_TeamMember") Is Nothing Then
                    ddTeamMember.SelectedValue = Server.HtmlEncode(Request.Cookies("WFFA_TeamMember").Value)
                    ViewState("sTM") = Server.HtmlEncode(Request.Cookies("WFFA_TeamMember").Value)
                End If

                If Not Request.Cookies("WFFA_Family") Is Nothing Then
                    ddFamily.SelectedValue = Server.HtmlEncode(Request.Cookies("WFFA_Family").Value)
                    ViewState("sFam") = Server.HtmlEncode(Request.Cookies("WFFA_Family").Value)
                End If
            Else
                ViewState("sTM") = ddTeamMember.SelectedValue
                ViewState("sFam") = ddFamily.SelectedValue
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

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
            Dim dsTM As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 114 'Team Member Family Accounts form id
            Dim iRoleID As Integer = 0

            dsTM = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTM = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTM IsNot Nothing Then
                If dsTM.Tables.Count And dsTM.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTM.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTM.Tables(0).Rows(0).Item("Working")
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
                End If 'EOF of "If dsTM.Tables.Count And dsTM.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTM IsNot Nothing Then"
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF CheckRights
#End Region 'EOF  "Form Level Security using Roles &/or Subscriptions"

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

        ''bind existing data to drop down Commodity Primary control for selection criteria for search
        ds = commonFunctions.GetFamily()
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddFamily.DataSource = ds
            ddFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName.ToString()
            ddFamily.DataValueField = ds.Tables(0).Columns("FamilyID").ColumnName.ToString()
            ddFamily.DataBind()
            ddFamily.Items.Insert(0, "")
        End If
    End Sub 'EOF BindCriteria

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ''******
        '' Store search parameters
        ''******
        Response.Cookies("WFFA_TeamMember").Value = ddTeamMember.SelectedValue
        Response.Cookies("WFFA_Family").Value = ddFamily.SelectedValue

        ''******
        '' Redirect to the Team Member Backups List page
        ''******
        Response.Redirect("Team_Member_Family_Purchasing.aspx?sTM=" & ddTeamMember.SelectedValue & "&sFam=" & ddFamily.SelectedValue, True)

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ddTeamMember.SelectedValue = ""

        ''******
        '' Delete cookies in search parameters.
        ''******
        WorkFlowModule.DeleteWFCookies_TeamMemberFamilyAssignments()

        ''******
        '' Redirect to the Team Member Backups List page
        ''******
        Response.Redirect("Team_Member_Family_Purchasing.aspx", True)
    End Sub 'EOF btnReset_Click

    Protected Sub gvWorkFlow_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvWorkFlow.RowCommand

        Dim TeamMember As DropDownList
        Dim FamilyID As DropDownList

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

            FamilyID = CType(gvWorkFlow.FooterRow.FindControl("ddFamily"), DropDownList)
            odsWorkFlow.InsertParameters("FamilyID").DefaultValue = FamilyID.SelectedValue

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

            FamilyID = CType(gvWorkFlow.FooterRow.FindControl("ddFamily"), DropDownList)
            FamilyID.ClearSelection()
            FamilyID.Items.Add("")
            FamilyID.SelectedValue = ""
        End If
    End Sub 'EOF gvWorkFlow_RowCommand

    Protected Sub gvWorkFlow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkFlow.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' ' reference the Delete ImageButtone
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(3).Controls(3), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim TeamMember As WorkFlow.WorkFlow_Family_Purchasing_AssignmentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, WorkFlow.WorkFlow_Family_Purchasing_AssignmentsRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete Team Member (" & DataBinder.Eval(e.Row.DataItem, "TeamMemberName") & ")  Family (" & DataBinder.Eval(e.Row.DataItem, "FamilyName") & ")?');")
                End If
            End If
        End If
    End Sub

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty() As Boolean
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
    End Property 'EOF LoadDataEmpty

    Protected Sub odsWorkFlow_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsWorkFlow.Selected
        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As WorkFlow.WorkFlow_Family_Purchasing_AssignmentsDataTable = CType(e.ReturnValue, WorkFlow.WorkFlow_Family_Purchasing_AssignmentsDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty = True
        Else
            LoadDataEmpty = False
        End If
    End Sub 'EOF odsWorkFlow_Selected

    Protected Sub gvWorkFlow_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkFlow.RowCreated

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub 'EOF gvWorkFlow_RowCreated

#End Region ' Insert Empty GridView Work-Around

End Class
