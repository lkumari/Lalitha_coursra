' ************************************************************************************************
' Name:	UGNFacilityMaintenance.aspx.vb
' Purpose:	This program is used to view, insert, update UGN Facilities information
'
' Date		    Author	    
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 10/03/2008    RCarlson            Added Security Role Select Statement

Partial Class DataMaintenance_UGNFacilityMaintenance
    Inherits System.Web.UI.Page
    Protected Sub CheckRights()

        Try
            'default to hide edit column
            gvUGNFacilityList.Columns(4).Visible = False
            If gvUGNFacilityList.FooterRow IsNot Nothing Then
                gvUGNFacilityList.FooterRow.Visible = False
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

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 28)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            'If iRoleID = 11 Then ' ADMIN RIGHTS                                
                            '    gvUGNFacilityList.Columns(4).Visible = True
                            '    If gvUGNFacilityList.FooterRow IsNot Nothing Then
                            '        gvUGNFacilityList.FooterRow.Visible = True
                            '    End If
                            'End If
                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    gvUGNFacilityList.Columns(4).Visible = True
                                    If gvUGNFacilityList.FooterRow IsNot Nothing Then
                                        gvUGNFacilityList.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    gvUGNFacilityList.Columns(4).Visible = True
                                    If gvUGNFacilityList.FooterRow IsNot Nothing Then
                                        gvUGNFacilityList.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvUGNFacilityList.Columns(4).Visible = False
                                    If gvUGNFacilityList.FooterRow IsNot Nothing Then
                                        gvUGNFacilityList.FooterRow.Visible = False
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
            m.PageTitle = "UGN, Inc.: UGN Facility Maintenance"
            m.ContentLabel = "UGN Facility"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > UGN Facility"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

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
    Protected Sub gvUGNFacilityList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUGNFacilityList.RowCommand

        Try
            Dim txtUGNFacilityTemp As TextBox
            Dim txtUGNFacilityNameTemp As TextBox

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then
                txtUGNFacilityTemp = CType(gvUGNFacilityList.FooterRow.FindControl("txtUGNFacilityInsert"), TextBox)
                txtUGNFacilityNameTemp = CType(gvUGNFacilityList.FooterRow.FindControl("txtUGNFacilityNameInsert"), TextBox)

                odsUGNFacilityList.InsertParameters("UGNFacility").DefaultValue = txtUGNFacilityTemp.Text
                odsUGNFacilityList.InsertParameters("UGNFacilityName").DefaultValue = txtUGNFacilityNameTemp.Text
                odsUGNFacilityList.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvUGNFacilityList.ShowFooter = False
            Else
                gvUGNFacilityList.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtUGNFacilityTemp = CType(gvUGNFacilityList.FooterRow.FindControl("txtUGNFacilityInsert"), TextBox)
                txtUGNFacilityTemp.Text = Nothing

                txtUGNFacilityNameTemp = CType(gvUGNFacilityList.FooterRow.FindControl("txtUGNFacilityNameInsert"), TextBox)
                txtUGNFacilityNameTemp.Text = Nothing
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
End Class
