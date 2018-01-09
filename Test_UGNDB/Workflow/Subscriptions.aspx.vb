''******************************************************************************************************
''* Subscriptions.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new Subscription data.
''*
''* Author  : LRey 03/12/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Partial Class Workflow_Sample_Subscriptions_with_ObjectDataSource
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Subscriptions"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Work Flow</b> > Subscriptions"
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
            gvSubscriptions.Columns(4).Visible = False
            gvSubscriptions.ShowFooter = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 11 'Subscriptions form id
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
                                        gvSubscriptions.Columns(4).Visible = True
                                        gvSubscriptions.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        gvSubscriptions.Columns(4).Visible = True
                                        gvSubscriptions.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        gvSubscriptions.Columns(4).Visible = True
                                        gvSubscriptions.ShowFooter = True
                                        ViewState("ObjectRole") = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        gvSubscriptions.Columns(4).Visible = False
                                        gvSubscriptions.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        gvSubscriptions.Columns(4).Visible = True
                                        gvSubscriptions.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        gvSubscriptions.Columns(4).Visible = False
                                        gvSubscriptions.ShowFooter = False
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

    Protected Sub gvSubscriptions_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        ''***
        ''This section allows the inserting of a new row when called by the OnInserting event call.
        ''***
        Dim temp As TextBox
        If (e.CommandName = "Insert") Then
            temp = CType(gvSubscriptions.FooterRow.FindControl("newSubscription"), TextBox)
            odsSubscriptions.InsertParameters("Subscription").DefaultValue = temp.Text
            odsSubscriptions.Insert()

            '' Indicate that the user needs to be sent to the last page
            SendUserToLastPage = True
        End If
        ''***
        ''This section allows show/hides the footer row when the Edit control is clicked
        ''***
        If e.CommandName = "Edit" Then
            gvSubscriptions.ShowFooter = False
        Else
            If ViewState("ObjectRole") = True Then
                gvSubscriptions.ShowFooter = True
            Else
                gvSubscriptions.ShowFooter = False
            End If
        End If
        ''***
        ''This section clears out the values in the footer row
        ''***
        If e.CommandName = "Undo" Then
            Dim Subscription As TextBox
            Subscription = CType(gvSubscriptions.FooterRow.FindControl("newSubscription"), TextBox)
            Subscription.Text = Nothing
        End If
    End Sub

    Protected Sub gvSubscriptions_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSubscriptions.DataBound
        If (SendUserToLastPage) Then
            gvSubscriptions.PageIndex = gvSubscriptions.PageCount - 1
        End If
    End Sub
  
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Response.Redirect("Subscriptions.aspx?Subscription=" & txtSuscriptionDescr.Text)
    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Response.Redirect("Subscriptions.aspx")
    End Sub
End Class
