''******************************************************************************************************
''* ExpProj_Category_Maint.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new ExpProj_Category data.
''*
''* Author  : LRey 08/27/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Partial Class ExpProj_Category_Maint
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Category"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > Category (SR)"
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
                If Request.QueryString("CategoryName") IsNot Nothing Then
                    txtCategoryName.Text = Server.UrlDecode(Request.QueryString("CategoryName").ToString)
                End If
            End If

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
        '' This function is used to enable/disable controls on the form based on 
        '' TM() 's Security/Subscription
        ''********

        Try
            ''*******
            '' Disable controls by default
            ''*******
            gvCategory.Columns(6).Visible = False
            gvCategory.ShowFooter = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 90 'ExpProj_Category form id
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
                                        gvCategory.Columns(6).Visible = True
                                        gvCategory.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        gvCategory.Columns(6).Visible = True
                                        gvCategory.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        gvCategory.Columns(6).Visible = True
                                        gvCategory.ShowFooter = True
                                        ViewState("ObjectRole") = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        gvCategory.Columns(6).Visible = False
                                        gvCategory.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        gvCategory.Columns(6).Visible = True
                                        gvCategory.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        gvCategory.Columns(6).Visible = False
                                        gvCategory.ShowFooter = False
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

    Protected Sub gvCategory_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        ''***
        ''This section allows the inserting of a new row when save button is 
        ''clicked from the footer.
        ''***
        If e.CommandName = "Insert" Then
            ''Insert data
            Dim Category As TextBox
            Dim GLNo As TextBox
            Dim UsefulLife As TextBox

            If gvCategory.Rows.Count = 0 Then
                '' We are inserting through the DetailsView in the EmptyDataTemplate
                Return
            End If

            '' Only perform the following logic when inserting through the footer
            Category = CType(gvCategory.FooterRow.FindControl("txtCategory"), TextBox)
            odsCategory.InsertParameters("CategoryName").DefaultValue = Category.Text

            GLNo = CType(gvCategory.FooterRow.FindControl("txtGLNo"), TextBox)
            odsCategory.InsertParameters("GLNo").DefaultValue = GLNo.Text

            UsefulLife = CType(gvCategory.FooterRow.FindControl("txtUsefulLife"), TextBox)
            odsCategory.InsertParameters("UsefulLife").DefaultValue = UsefulLife.Text

            odsCategory.Insert()
        End If

        ''***
        ''This section allows show/hides the footer row when the Edit control is clicked
        ''***
        If e.CommandName = "Edit" Then
            gvCategory.ShowFooter = False
        Else
            If ViewState("ObjectRole") = True Then
                gvCategory.ShowFooter = True
            Else
                gvCategory.ShowFooter = False
            End If
        End If

        ''***
        ''This section clears out the values in the footer row
        ''***
        If e.CommandName = "Undo" Then
            Dim Category As TextBox
            Dim GLNo As TextBox
            Dim UsefulLife As TextBox

            Category = CType(gvCategory.FooterRow.FindControl("txtCategory"), TextBox)
            Category.Text = Nothing

            GLNo = CType(gvCategory.FooterRow.FindControl("txtGLNo"), TextBox)
            GLNo.Text = Nothing

            UsefulLife = CType(gvCategory.FooterRow.FindControl("txtUsefulLife"), TextBox)
            UsefulLife.Text = Nothing
        End If
    End Sub 'EOF gvCategory_RowCommand

    Protected Sub gvCategory_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCategory.DataBound
        If (SendUserToLastPage) Then
            gvCategory.PageIndex = gvCategory.PageCount - 1
        End If
    End Sub 'EOF gvCategory_DataBound
#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_Category() As Boolean

        Get
            If ViewState("LoadDataEmpty_Category") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Category"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Category") = value
        End Set
    End Property 'EOF LoadDataEmpty_Category()

    Protected Sub gvCategory_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCategory.RowCreated

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Category
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub 'EOF gvCategory_RowCreated

    Protected Sub odsCategory_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCategory.Selected

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        Dim dt As ExpProj.ExpProj_CategoryDataTable = CType(e.ReturnValue, ExpProj.ExpProj_CategoryDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_Category = True
        Else
            LoadDataEmpty_Category = False
        End If
    End Sub 'EOF odsCapitalSpending_Selected
#End Region 'EOF "Insert Empty GridView Work-Around"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Response.Redirect("ExpProj_Category_Maint.aspx?CategoryName=" & txtCategoryName.Text)
    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Response.Redirect("ExpProj_Category_Maint.aspx")
    End Sub
End Class 'EOF btnReset_Click
