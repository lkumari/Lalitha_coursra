''******************************************************************************************************
''* Priorities_Maint.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new Subscription data.
''*
''* Author  : LRey 03/12/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System
Imports System.Web
Imports System.Reflection
Imports System.Drawing
Imports System.Collections.Generic
Partial Class RnD_Priorities_Maint
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Priorities"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > Testing Classification"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If
            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("RnDExtender"), CollapsiblePanelExtender)
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

        Try
            ''*******
            '' Disable controls by default
            ''*******
            gvPriority.Columns(5).Visible = False
            gvPriority.ShowFooter = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 79 'Testing Classification form id
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
                                        gvPriority.Columns(5).Visible = True
                                        gvPriority.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        gvPriority.Columns(5).Visible = True
                                        gvPriority.ShowFooter = True
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        gvPriority.Columns(5).Visible = True
                                        gvPriority.ShowFooter = True
                                        ViewState("ObjectRole") = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        gvPriority.Columns(5).Visible = False
                                        gvPriority.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        gvPriority.Columns(5).Visible = True
                                        gvPriority.ShowFooter = False
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        gvPriority.Columns(5).Visible = False
                                        gvPriority.ShowFooter = False
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

    Protected Sub gvPriority_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        ''***
        ''This section allows the inserting of a new row when save button is clicked from the footer.
        ''***
        If e.CommandName = "Insert" Then
            ''Insert data
            Dim PriorityDesc As TextBox
            Dim ColorCode As DropDownList

            If gvPriority.Rows.Count = 0 Then
                '' We are inserting through the DetailsView in the EmptyDataTemplate
                Return
            End If

            '' Only perform the following logic when inserting through the footer
            ColorCode = CType(gvPriority.FooterRow.FindControl("ddColorCode"), DropDownList)
            odsPriority.InsertParameters("ColorCode").DefaultValue = ColorCode.SelectedValue

            PriorityDesc = CType(gvPriority.FooterRow.FindControl("txtPriorityDesc"), TextBox)
            odsPriority.InsertParameters("PriorityDescription").DefaultValue = PriorityDesc.Text

            odsPriority.Insert()
        End If

        ''***
        ''This section allows show/hides the footer row when the Edit control is clicked
        ''***
        If e.CommandName = "Edit" Then
            gvPriority.ShowFooter = False
        Else
            If ViewState("ObjectRole") = True Then
                gvPriority.ShowFooter = True
            Else
                gvPriority.ShowFooter = False
            End If
        End If

        ''***
        ''This section clears out the values in the footer row
        ''***
        If e.CommandName = "Undo" Then
            Dim PriorityDesc As TextBox
            Dim ColorCode As TextBox
            PriorityDesc = CType(gvPriority.FooterRow.FindControl("txtPriorityDesc"), TextBox)
            PriorityDesc.Text = Nothing
            ColorCode = CType(gvPriority.FooterRow.FindControl("txtColorCode"), TextBox)
            'ColorCode.Text = "crNoColor"
        End If
    End Sub

    Protected Sub gvPriority_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPriority.DataBound
        If (SendUserToLastPage) Then
            gvPriority.PageIndex = gvPriority.PageCount - 1
        End If

    End Sub
   
    Protected Sub gvPriority_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvPriority.RowDataBound
        Try
            Dim listColorCode As DropDownList = e.Row.FindControl("ddColorCode")

            Dim allColors As String() = [Enum].GetNames(GetType(System.Drawing.KnownColor))
            Dim systemEnvironmentColors As String() = New String((GetType(System.Drawing.SystemColors)).GetProperties().Length - 1) {}

            ' ''If e.Row.RowType = DataControlRowType.DataRow Then

            ' ''    Dim index As Integer = 0

            ' ''    For Each member As MemberInfo In (GetType(System.Drawing.SystemColors)).GetProperties()
            ' ''        ' systemEnvironmentColors(System.Math.Max(System.Threading.Interlocked.Increment(index), index - 1)) = member.Name
            ' ''    Next


            ' ''    For Each color As String In allColors
            ' ''        'If Array.IndexOf(systemEnvironmentColors, color) < 0 Then
            ' ''        'listColorCode.ClearSelection()
            ' ''        Dim finalColorListItem As New ListItem
            ' ''        finalColorListItem.Text = color.ToString
            ' ''        finalColorListItem.Value = color.ToString

            ' ''        listColorCode.Items.Add(finalColorListItem)
            ' ''        ' End If
            ' ''    Next
            ' ''End If

            ''******************
            ''Below statement is used for Cascading Drop-Down list in a gridview.
            ''******************
            If (e.Row.RowState And DataControlRowState.Edit) = DataControlRowState.Edit Then
                Dim dv As System.Data.DataRowView = e.Row.DataItem
                Dim tempLabel As Label = e.Row.FindControl("lblEditColor")
                Dim msgEditColor As Label = e.Row.FindControl("msgEditColor")
                If tempLabel IsNot Nothing Then
                    tempLabel.BackColor = Color.FromName(listColorCode.SelectedItem.Text)
                    msgEditColor.BackColor = Color.FromName(listColorCode.SelectedItem.Text)

                End If
            Else
                Dim tempLabel As Label = e.Row.FindControl("lblViewColor")
                If tempLabel IsNot Nothing Then
                    tempLabel.BackColor = Color.FromName(listColorCode.SelectedItem.Text)
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_Priority() As Boolean

        Get
            If ViewState("LoadDataEmpty_Priority") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Priority"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Priority") = value
        End Set
    End Property

    Protected Sub gvPriority_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPriority.RowCreated

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Priority
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub
#End Region

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Response.Redirect("Priorities_Maint.aspx?sPdesc=" & txtPriority.Text)
    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Response.Redirect("Priorities_Maint.aspx")
    End Sub
End Class
