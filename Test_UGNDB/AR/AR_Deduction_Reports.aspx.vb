' ************************************************************************************************
' Name:	AR_Deduction_Reports.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 05/21/2013    LRey			Created .Net application
' 12/20/2013    LRey            Replaced Customer DDL to OEMManufacturer.
' ************************************************************************************************
Partial Class AR_AR_Deduction_Reports
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Operations Deduction Reports"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable</b> > Operations Deduction Reports"
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
            ctl = m.FindControl("ARExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            'focus on Vehicle List screen Program field
            txtARDID.Focus()


            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pARDID") <> "" Then
                ViewState("pARDID") = HttpContext.Current.Request.QueryString("pARDID")
            Else
                ViewState("pARDID") = ""
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sARDID") = ""
                ViewState("sREFNO") = ""
                ViewState("sSBTMID") = 0
                ViewState("sDCOM") = ""
                ViewState("sDUFAC") = ""
                ViewState("sDCUST") = ""
                ViewState("sDSF") = ""
                ViewState("sDST") = ""
                ViewState("sDRSTS") = ""
                ViewState("sDRSN") = 0
                ViewState("sCDF") = ""
                ViewState("sCDT") = ""
                ViewState("sSB") = ""
                ViewState("sPNO") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("ARR_ARDID") Is Nothing Then
                    txtARDID.Text = Server.HtmlEncode(Request.Cookies("ARR_ARDID").Value)
                    ViewState("sARDID") = Server.HtmlEncode(Request.Cookies("ARR_ARDID").Value)
                End If

                If Not Request.Cookies("ARR_DREFNO") Is Nothing Then
                    txtReferenceNo.Text = Server.HtmlEncode(Request.Cookies("ARR_DREFNO").Value)
                    ViewState("sREFNO") = Server.HtmlEncode(Request.Cookies("ARR_DREFNO").Value)
                End If

                If Not Request.Cookies("ARR_SBTMID") Is Nothing Then
                    ddSubmittedBy.SelectedValue = Server.HtmlEncode(Request.Cookies("ARR_SBTMID").Value)
                    ViewState("sSBTMID") = Server.HtmlEncode(Request.Cookies("ARR_SBTMID").Value)
                End If

                If Not Request.Cookies("ARR_DCOM") Is Nothing Then
                    txtComments.Text = Server.HtmlEncode(Request.Cookies("ARR_DCOM").Value)
                    ViewState("sDCOM") = Server.HtmlEncode(Request.Cookies("ARR_DCOM").Value)
                End If

                If Not Request.Cookies("ARR_DUFAC") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("ARR_DUFAC").Value)
                    ViewState("sDUFAC") = Server.HtmlEncode(Request.Cookies("ARR_DUFAC").Value)
                End If

                If (Not Request.Cookies("ARR_DCUST") Is Nothing) Then
                    ViewState("sDCUST") = Server.HtmlEncode(Request.Cookies("ARR_DCUST").Value)
                End If

                If Not Request.Cookies("ARR_DSF") Is Nothing Then
                    txtDateSubFrom.Text = Server.HtmlEncode(Request.Cookies("ARR_DSF").Value)
                    ViewState("sDSF") = Server.HtmlEncode(Request.Cookies("ARR_DSF").Value)
                End If

                If Not Request.Cookies("ARR_DST") Is Nothing Then
                    txtDateSubTo.Text = Server.HtmlEncode(Request.Cookies("ARR_DST").Value)
                    ViewState("sDST") = Server.HtmlEncode(Request.Cookies("ARR_DST").Value)
                End If

                If Not Request.Cookies("ARR_DRSTS") Is Nothing Then
                    ddRecStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("ARR_DRSTS").Value)
                    ViewState("sDRSTS") = Server.HtmlEncode(Request.Cookies("ARR_DRSTS").Value)
                End If

                If Not Request.Cookies("ARR_DRSN") Is Nothing Then
                    ddReason.SelectedValue = Server.HtmlEncode(Request.Cookies("ARR_DRSN").Value)
                    ViewState("sDRSN") = Server.HtmlEncode(Request.Cookies("ARR_DRSN").Value)
                End If

                If Not Request.Cookies("ARR_CDF") Is Nothing Then
                    txtClosedDateFrom.Text = Server.HtmlEncode(Request.Cookies("ARR_CDF").Value)
                    ViewState("sCDF") = Server.HtmlEncode(Request.Cookies("ARR_CDF").Value)
                End If

                If Not Request.Cookies("ARR_CDT") Is Nothing Then
                    txtClosedDateTo.Text = Server.HtmlEncode(Request.Cookies("ARR_CDT").Value)
                    ViewState("sCDT") = Server.HtmlEncode(Request.Cookies("ARR_CDT").Value)
                End If

                If Not Request.Cookies("ARR_SB") Is Nothing Then
                    ddSortBy.SelectedValue = Server.HtmlEncode(Request.Cookies("ARR_SB").Value)
                    ViewState("sSB") = Server.HtmlEncode(Request.Cookies("ARR_SB").Value)
                End If

                If Not Request.Cookies("ARR_PNO") Is Nothing Then
                    txtPartNo.Text = Server.HtmlEncode(Request.Cookies("ARR_PNO").Value)
                    ViewState("sPNO") = Server.HtmlEncode(Request.Cookies("ARR_PNO").Value)
                End If

            Else
                ViewState("sARDID") = txtARDID.Text
                ViewState("sREFNO") = txtReferenceNo.Text
                ViewState("sSBTMID") = IIf(ddSubmittedBy.SelectedValue = Nothing, 0, ddSubmittedBy.SelectedValue)
                ViewState("sDCOM") = txtComments.Text
                ViewState("sDUFAC") = ddUGNFacility.SelectedValue
                ViewState("sDCUST") = ddCustomer.SelectedValue
                ViewState("sDSF") = txtDateSubFrom.Text
                ViewState("sDST") = txtDateSubTo.Text
                ViewState("sDRSTS") = ddRecStatus.SelectedValue
                ViewState("sDRSN") = IIf(ddReason.SelectedValue = Nothing, 0, ddReason.SelectedValue)
                ViewState("sCDF") = txtClosedDateFrom.Text
                ViewState("sCDT") = txtClosedDateTo.Text
                ViewState("sSB") = ddSortBy.SelectedValue
                ViewState("sPNO") = txtPartNo.Text
            End If


        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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
            ViewState("Admin") = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 139 'Operations Deduction Reports ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Mike.Alonzo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                For i = 0 To dsRoleForm.Tables(0).Rows.Count - 1
                                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ViewState("Admin") = True
                                            ViewState("ObjectRole") = True
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ViewState("ObjectRole") = True
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            ViewState("ObjectRole") = True
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            'N/A
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            'N/A
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                            'N/A
                                    End Select 'EOF of "Select Case iRoleID"
                                Next
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
#End Region 'EOF Security

    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down UGN Facility control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Project Leader control for selection criteria for search
        ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used for Project Leader
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddSubmittedBy.DataSource = ds
            ddSubmittedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddSubmittedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            ddSubmittedBy.DataBind()
            ddSubmittedBy.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = commonFunctions.GetOEMManufacturer("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddCustomer.DataSource = ds
            ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            ddCustomer.DataBind()
            ddCustomer.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = ARGroupModule.GetARDeductionReason("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddReason.DataSource = ds
            ddReason.DataTextField = ds.Tables(0).Columns("ddReasonDesc").ColumnName.ToString()
            ddReason.DataValueField = ds.Tables(0).Columns("RID").ColumnName.ToString()
            ddReason.DataBind()
            ddReason.Items.Insert(0, "")
        End If
    End Sub 'EOF BindCriteria

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ARGroupModule.DeleteARDeductionReportCookies()

            Response.Redirect("AR_Deduction_Reports.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        'set saved value of what criteria was used to search        
        Response.Cookies("ARR_ARDID").Value = txtARDID.Text
        Response.Cookies("ARR_DREFNO").Value = txtReferenceNo.Text
        Response.Cookies("ARR_SBTMID").Value = ddSubmittedBy.SelectedValue
        Response.Cookies("ARR_DCOM").Value = txtComments.Text
        Response.Cookies("ARR_DUFAC").Value = ddUGNFacility.SelectedValue
        Response.Cookies("ARR_DCUST").Value = ddCustomer.SelectedValue
        Response.Cookies("ARR_DSF").Value = txtDateSubFrom.Text
        Response.Cookies("ARR_DST").Value = txtDateSubTo.Text
        Response.Cookies("ARR_DRSTS").Value = ddRecStatus.SelectedValue
        Response.Cookies("ARR_DRSN").Value = ddReason.SelectedValue
        Response.Cookies("ARR_CDF").Value = txtClosedDateFrom.Text
        Response.Cookies("ARR_CDT").Value = txtClosedDateTo.Text
        Response.Cookies("ARR_SB").Value = ddSortBy.SelectedValue
        Response.Cookies("ARR_PNO").Value = txtPartNo.Text

        GoToReportBuilder(ddReportType.SelectedValue)
    End Sub 'EOF btnReport_Click

    Public Function GoToReportBuilder(ByVal CM As String) As Boolean
        Dim ARDID As String = ViewState("sARDID")
        Dim Refno As String = IIf(ViewState("sREFNo") <> Nothing, ViewState("sREFNo"), "")
        Dim SubBy As Integer = ViewState("sSBTMID")
        Dim Comments As String = ViewState("sDCOM")
        Dim UGNFac As String = ViewState("sDUFAC")
        Dim Customer As String = ViewState("sDCUST")
        Dim DSF As String = ViewState("sDSF")
        Dim DST As String = ViewState("sDST")
        Dim RSTS As String = ViewState("sDRSTS")
        Dim RSN As Integer = ViewState("sDRSN")
        Dim CDF As String = ViewState("sCDF")
        Dim CDT As String = ViewState("sCDT")
        Dim SB As String = ViewState("sSB")
        Dim PNO As String = ViewState("sPNO")

        ScriptManager.RegisterStartupScript(Me, GetType(String), "key1", _
                                            "window.open('crViewARDeductionReport.aspx?pARDID=" & ARDID & _
            "&pREFNo=" & Refno & _
            "&pSBTMID=" & SubBy & _
            "&pDCOM=" & Comments & _
            "&pDUFAC=" & UGNFac & _
            "&pDCUST=" & Customer & _
            "&pDSF=" & DSF & _
            "&pDST=" & DST & _
            "&pDRSTS=" & RSTS & _
            "&pDRSN=" & RSN & _
            "&pCDF=" & CDF & _
            "&pCDT=" & CDT & _
            "&pSB=" & SB & _
            "&pPNO=" & PNO & _
            "&pCM=" & CM & "');" & vbLf, True)

        Return True
    End Function 'EOF GoToReportBuilder


End Class
