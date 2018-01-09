''*****************************************************************************************************
''* Cost_Down_Up_Calculator.aspx.vb
''* The purpose of this page is to allow users to estimate CostDown/Up Volume/Sales for
''* (Budget or Forecast) later applying the change for BI reporting.
''*
''* Author  : LRey 07/01/2009
''* Modified: {Name} {Date} - {Notes}
''*****************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Partial Class PF_Volume_Adjustment
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Volume Adjustment"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > Volume Adjustment"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PFExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            If Not Page.IsPostBack Then
                cbJan.Checked = True
                cbFeb.Checked = True
                cbMar.Checked = True
                cbApr.Checked = True
                cbMay.Checked = True
                cbJun.Checked = True
                cbJul.Checked = True
                cbAug.Checked = True
                cbSep.Checked = True
                cbOct.Checked = True
                cbNov.Checked = True
                cbDec.Checked = True

                ViewState("sPYear") = 0
                ViewState("sRType") = ""
                ViewState("sRTypeNo") = ""
                ViewState("sCalc") = 0
                ViewState("sCalcDI") = ""
                ViewState("sJan") = True
                ViewState("sFeb") = True
                ViewState("sMar") = True
                ViewState("sApr") = True
                ViewState("sMay") = True
                ViewState("sJun") = True
                ViewState("sJul") = True
                ViewState("sAug") = True
                ViewState("sSep") = True
                ViewState("sOct") = True
                ViewState("sNov") = True
                ViewState("sDec") = True

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("PRV_PYear") Is Nothing Then
                    ddYear.SelectedValue = Server.HtmlEncode(Request.Cookies("PRV_PYear").Value)
                    ViewState("sPYear") = Server.HtmlEncode(Request.Cookies("PRV_PYear").Value)
                End If

                If Not Request.Cookies("PRV_RType") Is Nothing Then
                    ddRecordType.SelectedValue = Server.HtmlEncode(Request.Cookies("PRV_RType").Value)
                    ViewState("sRType") = Server.HtmlEncode(Request.Cookies("PRV_RType").Value)
                End If

                If Not Request.Cookies("PRV_RTypeNo") Is Nothing Then
                    ddRecordTypeNo.SelectedValue = Server.HtmlEncode(Request.Cookies("PRV_RTypeNo").Value)
                    ViewState("sRTypeNo") = Server.HtmlEncode(Request.Cookies("PRV_RTypeNo").Value)
                End If

                If Not Request.Cookies("PRV_Calc") Is Nothing Then
                    txtDecInc.Text = Server.HtmlEncode(Request.Cookies("PRV_Calc").Value)
                    ViewState("sCalc") = Server.HtmlEncode(Request.Cookies("PRV_Calc").Value)
                End If

                If Not Request.Cookies("PRV_CalcDI") Is Nothing Then
                    ddDecInc.SelectedValue = Server.HtmlEncode(Request.Cookies("PRV_CalcDI").Value)
                    ViewState("sCalcDI") = Server.HtmlEncode(Request.Cookies("PRV_CalcDI").Value)
                End If

                If Not Request.Cookies("PRV_Jan") Is Nothing Then
                    cbJan.Checked = Server.HtmlEncode(Request.Cookies("PRV_Jan").Value)
                    ViewState("sJan") = Server.HtmlEncode(Request.Cookies("PRV_Jan").Value)
                End If

                If Not Request.Cookies("PRV_Feb") Is Nothing Then
                    cbFeb.Checked = Server.HtmlEncode(Request.Cookies("PRV_Feb").Value)
                    ViewState("sFeb") = Server.HtmlEncode(Request.Cookies("PRV_Feb").Value)
                End If

                If Not Request.Cookies("PRV_Mar") Is Nothing Then
                    cbMar.Checked = Server.HtmlEncode(Request.Cookies("PRV_Mar").Value)
                    ViewState("sMar") = Server.HtmlEncode(Request.Cookies("PRV_Mar").Value)
                End If

                If Not Request.Cookies("PRV_Apr") Is Nothing Then
                    cbApr.Checked = Server.HtmlEncode(Request.Cookies("PRV_Apr").Value)
                    ViewState("sApr") = Server.HtmlEncode(Request.Cookies("PRV_Apr").Value)
                End If

                If Not Request.Cookies("PRV_May") Is Nothing Then
                    cbMay.Checked = Server.HtmlEncode(Request.Cookies("PRV_May").Value)
                    ViewState("sMay") = Server.HtmlEncode(Request.Cookies("PRV_May").Value)
                End If

                If Not Request.Cookies("PRV_Jun") Is Nothing Then
                    cbJun.Checked = Server.HtmlEncode(Request.Cookies("PRV_Jun").Value)
                    ViewState("sJun") = Server.HtmlEncode(Request.Cookies("PRV_Jun").Value)
                End If

                If Not Request.Cookies("PRV_Jul") Is Nothing Then
                    cbJul.Checked = Server.HtmlEncode(Request.Cookies("PRV_Jul").Value)
                    ViewState("sJul") = Server.HtmlEncode(Request.Cookies("PRV_Jul").Value)
                End If

                If Not Request.Cookies("PRV_Aug") Is Nothing Then
                    cbAug.Checked = Server.HtmlEncode(Request.Cookies("PRV_Aug").Value)
                    ViewState("sAug") = Server.HtmlEncode(Request.Cookies("PRV_Aug").Value)
                End If

                If Not Request.Cookies("PRV_Sep") Is Nothing Then
                    cbSep.Checked = Server.HtmlEncode(Request.Cookies("PRV_Sep").Value)
                    ViewState("sSep") = Server.HtmlEncode(Request.Cookies("PRV_Sep").Value)
                End If

                If Not Request.Cookies("PRV_Oct") Is Nothing Then
                    cbOct.Checked = Server.HtmlEncode(Request.Cookies("PRV_Oct").Value)
                    ViewState("sOct") = Server.HtmlEncode(Request.Cookies("PRV_Oct").Value)
                End If

                If Not Request.Cookies("PRV_Nov") Is Nothing Then
                    cbNov.Checked = Server.HtmlEncode(Request.Cookies("PRV_Nov").Value)
                    ViewState("sNov") = Server.HtmlEncode(Request.Cookies("PRV_Nov").Value)
                End If

                If Not Request.Cookies("PRV_Dec") Is Nothing Then
                    cbDec.Checked = Server.HtmlEncode(Request.Cookies("PRV_Dec").Value)
                    ViewState("sDec") = Server.HtmlEncode(Request.Cookies("PRV_Dec").Value)
                End If

                ''Set Focus
                ddYear.Focus()
            Else
                ViewState("sPYear") = ddYear.SelectedValue
                ViewState("sRType") = ddRecordType.SelectedValue
                If ddRecordType.SelectedValue = "Budget" Or ddRecordType.SelectedValue = "Current" Then
                    ViewState("sRTypeNo") = IIf(ddRecordTypeNo.SelectedValue = Nothing, 0, ddRecordTypeNo.SelectedValue)
                Else
                    ViewState("sRTypeNo") = IIf(ddRecordTypeNo.SelectedValue = Nothing, 1, ddRecordTypeNo.SelectedValue)
                End If
                ViewState("sCalc") = IIf(txtDecInc.Text = Nothing, 0, txtDecInc.Text)
                ViewState("sCalcDI") = ddDecInc.SelectedValue
                ViewState("sJan") = cbJan.Checked
                ViewState("sFeb") = cbFeb.Checked
                ViewState("sMar") = cbMar.Checked
                ViewState("sApr") = cbApr.Checked
                ViewState("sMay") = cbMay.Checked
                ViewState("sJun") = cbJun.Checked
                ViewState("sJul") = cbJul.Checked
                ViewState("sAug") = cbAug.Checked
                ViewState("sSep") = cbSep.Checked
                ViewState("sOct") = cbOct.Checked
                ViewState("sNov") = cbNov.Checked
                ViewState("sDec") = cbDec.Checked

            End If

            Dim sScript As String
            sScript = "<script language=""JavaScript"">" & vbCrLf
            sScript += "function ConfirmButton(name)" & vbCrLf
            sScript += "{" & vbCrLf
            sScript += vbTab & "var Valid = true;" & vbCrLf
            sScript += vbTab & "if(typeof(Page_ClientValidate) == 'function')" & vbCrLf
            sScript += vbTab & "{" & vbCrLf
            sScript += vbTab & vbTab & "Valid = Page_ClientValidate(); " & vbCrLf
            sScript += vbTab & "}" & vbCrLf
            sScript += vbTab & "if(Valid)" & vbCrLf
            sScript += vbTab & "{" & vbCrLf
            sScript += vbTab & vbTab & "var Status= true;" & vbCrLf
            sScript += vbTab & vbTab & "if (name=='Submit'){" & vbCrLf
            sScript += vbTab & vbTab & vbTab & "Status = confirm('Are you sure you want to Apply Cost Down/Up to specified Sales Projection?');}" & vbCrLf
            sScript += vbTab & vbTab & vbTab & "return Status;" & vbCrLf
            sScript += vbTab & vbTab & "}" & vbCrLf
            sScript += vbTab & "else" & vbCrLf
            sScript += vbTab & "{" & vbCrLf
            sScript += vbTab & vbTab & vbTab & "return false;" & vbCrLf
            sScript += vbTab & "}" & vbCrLf
            sScript += "}" & vbCrLf
            sScript += "// -->" & vbCrLf
            sScript += "</script>" & vbCrLf

            If (Not ClientScript.IsClientScriptBlockRegistered("MyScript")) Then
                ClientScript.RegisterClientScriptBlock(Page.GetType, "MyScript", sScript)
            End If

            btnApply.Attributes.Add("onClick", "return ConfirmButton('Submit');")

            CheckRights()
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

        Try
            ''*******
            '' Disable controls by default
            ''*******
            btnCalculate.Enabled = False
            btnReset.Enabled = False
            btnApply.Enabled = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 87 'Volume Adjustment form id
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
                                        btnCalculate.Enabled = True
                                        btnReset.Enabled = True
                                        btnApply.Enabled = True
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        btnCalculate.Enabled = True
                                        btnReset.Enabled = True
                                        btnApply.Enabled = True
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        btnCalculate.Enabled = True
                                        btnReset.Enabled = True
                                        ViewState("ObjectRole") = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        btnCalculate.Enabled = True
                                        btnReset.Enabled = True
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        btnCalculate.Enabled = True
                                        btnReset.Enabled = True
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
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
#End Region 'EOF Form Level Security

    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Planning Year control for selection criteria for search
        ds = commonFunctions.GetYear("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddYear.DataSource = ds
            ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
            ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
            ddYear.DataBind()
            ddYear.Items.Insert(0, "")
        End If
    End Sub 'EOF BindCriteria

    Protected Sub ddRecordType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddRecordType.SelectedIndexChanged
        If ddRecordType.SelectedValue = "Budget" Or ddRecordType.SelectedValue = "Current" Then
            ddRecordTypeNo.Enabled = False
        Else
            ddRecordTypeNo.Enabled = True
        End If
    End Sub 'EOF ddRecordType_SelectedIndexChanged

    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        ' ''Me.Validate()
        ' ''Dim ds As DataSet


        ' ''If Page.IsValid Then
        ' ''    Dim PlanningYear As Integer = ddYear.SelectedValue
        ' ''    Dim RecordType As String = ddRecordType.SelectedValue
        ' ''    Dim RecordTypeNo As Integer = CType(IIf(ddRecordTypeNo.SelectedValue = "", 0, ddRecordTypeNo.SelectedValue), Integer)
        ' ''    Dim MsgDesc As String

        ' ''    If RecordTypeNo = 0 Then
        ' ''        MsgDesc = PlanningYear & " " & RecordType
        ' ''    Else
        ' ''        MsgDesc = PlanningYear & " " & RecordType & "/" & RecordTypeNo
        ' ''    End If
        ' ''    lblErrors.Visible = False

        ' ''    Try
        ' ''        ''*****
        ' ''        ''Verify that data does not already exist in Archive, display message if true.
        ' ''        ''*****
        ' ''        ' ''ds = PFModule.GetArchiveData(PlanningYear, RecordType, RecordTypeNo)
        ' ''        ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
        ' ''        ' ''    lblErrors.Text = "Cannot overwrite existing archived data for " & MsgDesc & "."
        ' ''        ' ''    lblErrors.Visible = "True"
        ' ''        ' ''Else
        ' ''        ''*****
        ' ''        ''Archive data according to the selected Planning Year and Record Type.
        ' ''        ''*****
        ' ''        ' ''PFModule.UpdateSalesProjection(PlanningYear, RecordType, RecordTypeNo)
        ' ''        ' ''lblErrors.Text = "Cost Down/Up applied for " & MsgDesc & " successfully."
        ' ''        ' ''lblErrors.Visible = "True"
        ' ''        ' ''End If

        ' ''    Catch ex As Exception
        ' ''        lblErrors.Text = "Error occurred during archiving.  Please contact the IS Application Group." & ex.Message
        ' ''        lblErrors.Visible = "True"
        ' ''    End Try
        ' ''End If
    End Sub 'EOF btnApply_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        PFModule.DeletePFCCookies_VolumeAdjustment()
        Session("TempCrystalRptFiles") = Nothing

        ''******
        '' Redirect to the Volume Adjustment page
        ''******
        Response.Redirect("Volume_Adjustment.aspx", False)
    End Sub 'EOF btnReset_Click

    Protected Sub rbCheckMonths_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCheckMonths.SelectedIndexChanged
        If rbCheckMonths.SelectedValue = True Then
            cbJan.Checked = True
            cbFeb.Checked = True
            cbMar.Checked = True
            cbApr.Checked = True
            cbMay.Checked = True
            cbJun.Checked = True
            cbJul.Checked = True
            cbAug.Checked = True
            cbSep.Checked = True
            cbOct.Checked = True
            cbNov.Checked = True
            cbDec.Checked = True
        Else
            cbJan.Checked = False
            cbFeb.Checked = False
            cbMar.Checked = False
            cbApr.Checked = False
            cbMay.Checked = False
            cbJun.Checked = False
            cbJul.Checked = False
            cbAug.Checked = False
            cbSep.Checked = False
            cbOct.Checked = False
            cbNov.Checked = False
            cbDec.Checked = False
        End If
    End Sub 'EOF rbCheckMonths_SelectedIndexChanged

    Protected Sub btnCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalculate.Click
        Try
            'set saved value of what criteria was used to search     
            Dim RTypeNo As Integer
            Response.Cookies("PRV_PYear").Value = ddYear.SelectedValue
            Response.Cookies("PRV_RType").Value = ddRecordType.SelectedValue
            If ddRecordType.SelectedValue = "Budget" Or ddRecordType.SelectedValue = "Current" Then
                Response.Cookies("PRV_RTypeNo").Value = IIf(ddRecordTypeNo.SelectedValue = Nothing, 0, ddRecordTypeNo.SelectedValue)
                RTypeNo = IIf(ddRecordTypeNo.SelectedValue = Nothing, 0, ddRecordTypeNo.SelectedValue)
            Else
                Response.Cookies("PRV_RTypeNo").Value = IIf(ddRecordTypeNo.SelectedValue = Nothing, 1, ddRecordTypeNo.SelectedValue)
                RTypeNo = IIf(ddRecordTypeNo.SelectedValue = Nothing, 1, ddRecordTypeNo.SelectedValue)
            End If
            Response.Cookies("PRV_Calc").Value = IIf(txtDecInc.Text = Nothing, 0, txtDecInc.Text)
            Response.Cookies("PRV_CalcDI").Value = ddDecInc.SelectedValue
            Response.Cookies("PRV_Jan").Value = cbJan.Checked
            Response.Cookies("PRV_Feb").Value = cbFeb.Checked
            Response.Cookies("PRV_Mar").Value = cbMar.Checked
            Response.Cookies("PRV_Apr").Value = cbApr.Checked
            Response.Cookies("PRV_May").Value = cbMay.Checked
            Response.Cookies("PRV_Jun").Value = cbJun.Checked
            Response.Cookies("PRV_Jul").Value = cbJul.Checked
            Response.Cookies("PRV_Aug").Value = cbAug.Checked
            Response.Cookies("PRV_Sep").Value = cbSep.Checked
            Response.Cookies("PRV_Oct").Value = cbOct.Checked
            Response.Cookies("PRV_Nov").Value = cbNov.Checked
            Response.Cookies("PRV_Dec").Value = cbDec.Checked

            Session("TempCrystalRptFiles") = Nothing

            Response.Redirect("crViewVolumeAdjustment.aspx?pPlanningYear=" & ddYear.SelectedValue & "&pRecordType=" & ddRecordType.SelectedValue & "&pRecordTypeNo=" & RTypeNo & "&pCalculate=" & IIf(txtDecInc.Text = Nothing, 0, txtDecInc.Text) & "&pJan=" & cbJan.Checked & "&pFeb=" & cbFeb.Checked & "&pMar=" & cbMar.Checked & "&pApr=" & cbApr.Checked & "&pMay=" & cbMay.Checked & "&pJun=" & cbJun.Checked & "&pJul=" & cbJul.Checked & "&pAug=" & cbAug.Checked & "&pSep=" & cbSep.Checked & "&pOct=" & cbOct.Checked & "&pNov=" & cbNov.Checked & "&pDec=" & cbDec.Checked & "&pCalcDI=" & ddDecInc.SelectedValue, False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnCalculate_Click
End Class
