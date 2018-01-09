' ***********************************************************************************************
'
' Name:		Cost_Sheet_Activity_Report.aspx
' Purpose:	This Code Behind is for the Cost_Sheet_Activity_Report to show the turnaround time for UGN Team Members of Cost Sheet Approvals
'
' Date		 Author	    
' 05/04/2009 Roderick Carlson  
' ************************************************************************************************

Partial Class Costing_Cost_Sheet_Activity_Report
    Inherits System.Web.UI.Page
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 63)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("isAdmin") = True
                                    ViewState("isRestricted") = False
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("isAdmin") = True
                                    ViewState("isRestricted") = False
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("isAdmin") = True
                                    ViewState("isRestricted") = False
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    ViewState("isRestricted") = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    ViewState("isRestricted") = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    ViewState("isRestricted") = True
                            End Select
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub EnableControls()

        Try
            btnDetail.Visible = Not ViewState("isRestricted")
            btnReset.Visible = Not ViewState("isRestricted")
            btnSummary.Visible = Not ViewState("isRestricted")
            lblQuoteDateFromLabel.Visible = Not ViewState("isRestricted")
            lblQuoteDateToLabel.Visible = Not ViewState("isRestricted")
            lblTeamMemberLabel.Visible = Not ViewState("isRestricted")
            lblUGNFacilityLabel.Visible = Not ViewState("isRestricted")
            txtQuoteDateFromValue.Visible = Not ViewState("isRestricted")
            txtQuoteDateToValue.Visible = Not ViewState("isRestricted")
            ddTeamMemberValue.Visible = Not ViewState("isRestricted")
            ddUGNFacilityValue.Visible = Not ViewState("isRestricted")
            imgQuoteDateFromValue.Visible = Not ViewState("isRestricted")
            imgQuoteDateToValue.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then
                'no real admin logic
            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ''bind existing data to drop down UGN Facility control for selection criteria 
            ds = commonFunctions.GetUGNFacility("")
            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddUGNFacilityValue.DataSource = ds
                    ddUGNFacilityValue.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                    ddUGNFacilityValue.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                    ddUGNFacilityValue.DataBind()
                    ddUGNFacilityValue.Items.Insert(0, "")
                End If
            End If

            'bind existing team member list for Team Members who need to approve cost sheets still
            ds = CostingModule.GetCostSheetPreApproverNames()
            If ds IsNot Nothing Then
                If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
                    ddTeamMemberValue.DataSource = ds
                    ddTeamMemberValue.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName
                    ddTeamMemberValue.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                    ddTeamMemberValue.DataBind()
                    ddTeamMemberValue.Items.Insert(0, "")
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Function BuildQueryString() As String

        Dim strQueryString As String = ""

        Try

            If txtQuoteDateFromValue.Text.Trim <> "" Then
                strQueryString += "&QuoteDateFrom=" & txtQuoteDateFromValue.Text.Trim
            End If

            If txtQuoteDateToValue.Text.Trim <> "" Then
                strQueryString += "&QuoteDateTo=" & txtQuoteDateToValue.Text.Trim
            End If

            If ddUGNFacilityValue.SelectedIndex > 0 Then
                strQueryString += "&UGNFacility=" & ddUGNFacilityValue.SelectedValue
            End If

            If ddTeamMemberValue.SelectedIndex > 0 Then
                strQueryString += "&TeamMember=" & ddTeamMemberValue.SelectedValue
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" + mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        BuildQueryString = strQueryString

    End Function
    Protected Sub btnSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSummary.Click

        lblMessage.Text = ""

        Try            

            'clear crystal reports
            CostingModule.CleanCostingCrystalReports()

            Dim strQueryString As String = BuildQueryString()

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Team Member Activity Report Preview", "window.open('Cost_Sheet_Activity_Preview.aspx?ReportType=Summary" & strQueryString & "'," & Now.Ticks & ",'top=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" + mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDetail.Click

        lblMessage.Text = ""

        Try

            'clear crystal reports
            CostingModule.CleanCostingCrystalReports()

            Dim strQueryString As String = BuildQueryString()

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Team Member Activity Report Preview", "window.open('Cost_Sheet_Activity_Preview.aspx?ReportType=Detail" & strQueryString & "'," & Now.Ticks & ",'top=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" + mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            Response.Redirect("Cost_Sheet_Activity_Report.aspx", False)
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" + mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Cost Sheet Activity"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Cost Sheet Activity"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            'testMasterPanel = CType(Master.FindControl("CostingExtender"), CollapsiblePanelExtender)
            testMasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            CheckRights()

            If Not Page.IsPostBack Then
                BindCriteria()

                'clear crystal reports
                CostingModule.CleanCostingCrystalReports()
            End If

            EnableControls()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" + mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
