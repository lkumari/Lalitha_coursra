''*****************************************************************************************************
''* Lock_in_Sales_Projection.aspx.vb
''* The purpose of this page is to allow users to archive data for a planning year and record type 
''* (Budget or Forecast) used for BI reporting.
''*
''* Author  : LRey 05/16/2008
''* Modified: {Name} {Date} - {Notes}
''*****************************************************************************************************
#Region "Directives"

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Text

#End Region

Partial Class CCM_CycleCounterMatrix
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN: Cycle Count"
            m.ContentLabel = "Cycle Count"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Manufacturing</b> > Cycle Count"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If


            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            CheckRights()

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                'Bind data to drop down lists
                BindCriteria()

            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

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

        Try
            ''*******
            '' Disable controls by default
            ''*******
            ViewState("ObjectRole") = False

            If ddReportFormat.SelectedValue = "Detail" Then
                If ddReportType.SelectedValue = "Matrix" Then
                    lblSortBy.Visible = True
                    ddSortBy.Visible = True
                    lblStoreMonthEndValues.Visible = False
                    cbStoreMEV.Visible = False
                    lblCheckBox.Visible = False
                Else
                    lblSortBy.Visible = False
                    ddSortBy.Visible = False
                    lblStoreMonthEndValues.Visible = False
                    cbStoreMEV.Visible = False
                    lblCheckBox.Visible = False
                    txtFromDate.Enabled = False
                    txtToDate.Enabled = False
                End If
            Else
                lblSortBy.Visible = False
                ddSortBy.Visible = False
                If ddReportFormat.SelectedValue = Nothing Then
                    lblStoreMonthEndValues.Visible = False
                    cbStoreMEV.Visible = False
                    lblCheckBox.Visible = False
                Else
                    lblStoreMonthEndValues.Visible = True
                    cbStoreMEV.Visible = True
                    lblCheckBox.Visible = True
                End If
            End If
            rfvFromDate.Enabled = True
            rfvToDate.Enabled = True
            rfvReportFormat.Enabled = True


            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 58  'Cycle Counter form id
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Karla.Gifford", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")

                    Dim a As String = commonFunctions.UserInfo()
                    ViewState("TMLoc") = HttpContext.Current.Session("UserFacility")
                    If ViewState("TMLoc") <> "UT" Then
                        ddUGNFacility.Enabled = False
                    End If

                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        If ddReportType.SelectedValue = "Matrix" Then
                                            ViewState("ObjectRole") = True
                                        Else
                                            rfvFromDate.Enabled = False
                                            rfvToDate.Enabled = False
                                            rfvReportFormat.Enabled = False
                                        End If
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        If ddReportType.SelectedValue = "Matrix" Then
                                            ViewState("ObjectRole") = True
                                            cbStoreMEV.Enabled = False
                                        Else
                                            rfvFromDate.Enabled = False
                                            rfvToDate.Enabled = False
                                            rfvReportFormat.Enabled = False
                                        End If
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        ViewState("ObjectRole") = False
                                        cbStoreMEV.Enabled = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                        cbStoreMEV.Enabled = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        ViewState("ObjectRole") = False
                                        cbStoreMEV.Enabled = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        ViewState("ObjectRole") = False
                                        cbStoreMEV.Enabled = False
                                        btnSubmit.Enabled = False
                                        btnReset.Enabled = False
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
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Planning Year control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            If ViewState("TMLoc") <> "UT" Then
                ddUGNFacility.SelectedValue = ViewState("TMLoc")
            End If
            'ddUGNFacility.SelectedValue = "UN" 'used for testing

            If Day(Date.Today) <> 1 Then
                txtFromDate.Text = Month(Date.Today) & "/1/" & Year(Date.Today)
            Else
                txtFromDate.Text = Date.Today
            End If

            txtToDate.Text = Date.Today

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Response.Redirect("CycleCounterMatrix.aspx", False)
    End Sub

    Protected Sub ddReportFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReportFormat.SelectedIndexChanged
        Try
            If ddReportFormat.SelectedValue = "Detail" Then
                lblSortBy.Visible = True
                ddSortBy.Visible = True
                lblStoreMonthEndValues.Visible = False
                cbStoreMEV.Visible = False
                lblCheckBox.Visible = False
            Else
                lblSortBy.Visible = False
                ddSortBy.Visible = False
                lblStoreMonthEndValues.Visible = True
                cbStoreMEV.Visible = True
                lblCheckBox.Visible = True
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF ddReportFormat_SelectedIndexChanged

    Protected Sub ddReportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReportType.SelectedIndexChanged
        If ddReportType.SelectedValue = "Matrix" Then
            If Day(Date.Today) <> 1 Then
                txtFromDate.Text = Month(Date.Today) & "/1/" & Year(Date.Today)
            Else
                txtFromDate.Text = Date.Today
            End If

            txtToDate.Text = Date.Today
        Else
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            lblSortBy.Visible = False
            ddSortBy.Visible = False
            lblStoreMonthEndValues.Visible = False
            cbStoreMEV.Visible = False
            lblCheckBox.Visible = False
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If ddReportType.SelectedValue = "Matrix" Then
            If ddReportFormat.SelectedValue = "Detail" Then
             
                ScriptManager.RegisterStartupScript(Me, GetType(String), "key1", "window.open('crViewCycleCounterMatrixDetail.aspx?pFac=" & IIf(ddUGNFacility.SelectedValue = Nothing, ViewState("Facility"), ddUGNFacility.SelectedValue) & "&pFD=" & txtFromDate.Text & "&pTD=" & txtToDate.Text & "&pSB=" & ddSortBy.Text & "&pSMEV=" & cbStoreMEV.Checked & "');" & vbLf, True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(String), "key1", "window.open('crViewCycleCounterMatrixSummary.aspx?pFac=" & ddUGNFacility.SelectedValue & "&pFD=" & txtFromDate.Text & "&pTD=" & txtToDate.Text & "&pFormat=" & IIf(ddReportFormat.SelectedValue = "Grid View Summary", "GV", "CV") & "&pSMEV=" & cbStoreMEV.Checked & "');" & vbLf, True)
            End If
        Else
            If ddReportFormat.SelectedValue = "Detail" Then
                ScriptManager.RegisterStartupScript(Me, GetType(String), "key1", "window.open('crViewCycleCountClassification.aspx?pFac=" & ddUGNFacility.SelectedValue & "');" & vbLf, True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(String), "key1", "window.open('crViewCycleCountClassification.aspx?pFac=" & ddUGNFacility.SelectedValue & "&pFormat=" & IIf(ddReportFormat.SelectedValue = "Grid View Summary", "GV", "CV") & "');" & vbLf, True)
            End If
        End If
    End Sub

    
End Class
