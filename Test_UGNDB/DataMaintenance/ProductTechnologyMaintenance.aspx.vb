' ************************************************************************************************
' Name:	ProductTechnologyMaintenance.aspx.vb
' Purpose:	This program is used to view, insert, update Product Technology
'
' Date		    Author	    
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 10/03/2008    RCarlson            Added Security Role Select Statement

Partial Class DataMaintenance_ProductTechnologyMaintenance
    Inherits System.Web.UI.Page
    Protected Sub CheckRights()

        Try
            'default to hide edit column
            gvProductTechnologyList.Columns(4).Visible = False
            If gvProductTechnologyList.FooterRow IsNot Nothing Then
                gvProductTechnologyList.FooterRow.Visible = False
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 25)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            'If iRoleID = 11 Then ' ADMIN RIGHTS                                
                            '    gvProductTechnologyList.Columns(4).Visible = True
                            '    If gvProductTechnologyList.FooterRow IsNot Nothing Then
                            '        gvProductTechnologyList.FooterRow.Visible = True
                            '    End If
                            'End If
                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    gvProductTechnologyList.Columns(4).Visible = True
                                    If gvProductTechnologyList.FooterRow IsNot Nothing Then
                                        gvProductTechnologyList.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    gvProductTechnologyList.Columns(4).Visible = True
                                    If gvProductTechnologyList.FooterRow IsNot Nothing Then
                                        gvProductTechnologyList.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvProductTechnologyList.Columns(4).Visible = False
                                    If gvProductTechnologyList.FooterRow IsNot Nothing Then
                                        gvProductTechnologyList.FooterRow.Visible = False
                                    End If
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete

                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                            End Select
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Product Technology"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Product Technology"
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
                If Request.QueryString("ProductTechnologyName") IsNot Nothing Then
                    txtProductTechnologyNameSearch.Text = Server.UrlDecode(Request.QueryString("ProductTechnologyName").ToString)
                End If
            End If

            CheckRights()
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub gvProductTechnologyList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            Dim temp As TextBox
            If (e.CommandName = "Insert") Then
                temp = CType(gvProductTechnologyList.FooterRow.FindControl("txtProductTechnologyNameInsert"), TextBox)
                odsProductTechnologyList.InsertParameters("ProductTechnologyName").DefaultValue = temp.Text
                odsProductTechnologyList.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvProductTechnologyList.ShowFooter = False
            Else
                gvProductTechnologyList.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                temp = CType(gvProductTechnologyList.FooterRow.FindControl("txtProductTechnologyNameInsert"), TextBox)
                temp.Text = Nothing
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Redirect("ProductTechnologyMaintenance.aspx?ProductTechnologyName=" & Server.UrlEncode(txtProductTechnologyNameSearch.Text.Trim), False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            Response.Redirect("ProductTechnologyMaintenance.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
