' ***********************************************************************************************
'
' Name:		Cost_Sheet_Post_Approval_List.aspx
' Purpose:	This Code Behind is to build the notification list of post-approvals of a cost sheet
'
' Date		Author	    
' 10/27/2008 Roderick Carlson  
' 11/17/2009 Roderick Carlson - Cleaned up formatting a bit
' 09/09/2010 Roderick Carlson - CO-2974 - Check for Unapproved Vendors. If found, then notify purchasing team member with admin rights to UGNDBVendor maintenance
' 12/07/2011 Roderick Carlson - Prevent Duplicate Emails in list
' 02/20/2013 Roderick Carlson - Update logic for Default Purchasing when unapproved vendor is used
' ************************************************************************************************


Partial Class Cost_Sheet_Post_Approval_List
    Inherits System.Web.UI.Page

    Private Function getPurchasingVendorAdminEmail() As String

        'get purchasing team members with admin rights to UGN DB Vendor Data Maintenance page
        Dim bEmailAddress As String = ""

        Try
            Dim ds As DataSet
            Dim dsTeamMember As DataSet
            'Dim dsRoleForm As DataSet
            Dim objCostSheetPostApprovalBLL As CostSheetPostApprovalBLL = New CostSheetPostApprovalBLL

            'Dim iRowCounter As Integer = 0
            'Dim iRole As Integer = 0
            Dim iTempTeamMemberID As Integer = 0


            ds = commonFunctions.GetTeamMemberBySubscription(53) 'default Purchasing
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                        iTempTeamMemberID = ds.Tables(0).Rows(0).Item("TMID")

                        dsTeamMember = SecurityModule.GetTeamMember(iTempTeamMemberID, "", "", "", "", "", True, Nothing)
                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                            If bEmailAddress <> "" Then
                                bEmailAddress &= ";"
                            End If

                            bEmailAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                            'insert new row
                            objCostSheetPostApprovalBLL.InsertCostSheetPostApprovalItem(ViewState("CostSheetID"), iTempTeamMemberID)

                            'update notification date of row
                            CostingModule.UpdateCostSheetPostApprovalItem(ViewState("CostSheetID"), iTempTeamMemberID)
                        End If
                    End If
                End If
            End If

            ''purchasing group
            'ds = commonFunctions.GetTeamMemberBySubscription(7)
            'If commonFunctions.CheckDataSet(ds) = True Then
            '    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
            '        If ds.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
            '            iTempTeamMemberID = ds.Tables(0).Rows(iRowCounter).Item("TMID")

            '            'ugndbvendor page
            '            dsRoleForm = SecurityModule.GetTMRoleForm(iTempTeamMemberID, Nothing, 73)

            '            If commonFunctions.CheckDataSet(dsRoleForm) = True Then
            '                iRole = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
            '                ' = 11, 12,13 'Admin, Assist, or Chamption
            '                Select Case iRole
            '                    Case 11, 12, 13
            '                        If ds.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then
            '                            If bEmailAddress <> "" Then
            '                                bEmailAddress &= ";"
            '                            End If

            '                            bEmailAddress &= ds.Tables(0).Rows(iRowCounter).Item("Email").ToString

            '                            'insert new row
            '                            objCostSheetPostApprovalBLL.InsertCostSheetPostApprovalItem(ViewState("CostSheetID"), iTempTeamMemberID)

            '                            'update notification date of row
            '                            CostingModule.UpdateCostSheetPostApprovalItem(ViewState("CostSheetID"), iTempTeamMemberID)

            '                        End If
            '                End Select

            '            End If
            '        End If
            '    Next
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
        Return bEmailAddress

    End Function
    Private Function isApprovedVendor() As Boolean

        Dim bApprovedVendor As Boolean = True

        Try
            Dim ds As DataSet

            Dim iRowCounter As Integer = 0

            ds = CostingModule.GetCostSheetMaterial(ViewState("CostSheetID"))
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("isApprovedVendor") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("isApprovedVendor") = 0 Then
                            bApprovedVendor = False

                            'add note to email but avoid duplicate lines
                            If ds.Tables(0).Rows(iRowCounter).Item("ddUGNDBVendorName").ToString <> "" Then
                                If InStr(txtCommentsValue.Text, ds.Tables(0).Rows(iRowCounter).Item("ddUGNDBVendorName").ToString) <= 0 Then
                                    txtCommentsValue.Text = txtCommentsValue.Text & vbCrLf & " Note: " & ds.Tables(0).Rows(iRowCounter).Item("ddUGNDBVendorName").ToString & " is an unapproved vendor. "
                                End If

                            End If
                        End If
                    End If


                Next
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return bApprovedVendor

    End Function

    Protected Function GetNotificationList(ByVal filterNotified As Boolean, ByVal isNotified As Boolean) As String

        Dim strEmailToAddress As String = ""

        Try
            Dim dsNotificationList As DataSet
            Dim dsTeamMember As DataSet
            Dim iRowCounter As Integer = 0
            Dim iNotificationTeamMemberID As Integer = 0
            Dim strPurchasingEmail As String = ""

            ''check if any vendors are unapproved. if so, include purchasing vendor admin (most likely silvia talavera)
            If isApprovedVendor() = False Then
                strPurchasingEmail = getPurchasingVendorAdminEmail()

                If strPurchasingEmail <> "" Then

                    If InStr(strEmailToAddress, strPurchasingEmail, CompareMethod.Binary) <= 0 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= strPurchasingEmail
                    End If

                    'gvPostApproval.DataBind()

                End If
            End If

            'get recipients
            'if cbNotifyOnlyNew is checked then  get only people who have NOT been notified, else get all 
            dsNotificationList = CostingModule.GetCostSheetPostApprovalList(ViewState("CostSheetID"), filterNotified, isNotified)
            If dsNotificationList IsNot Nothing Then
                If dsNotificationList.Tables.Count > 0 And dsNotificationList.Tables(0).Rows.Count > 0 Then
                    For iRowCounter = 0 To dsNotificationList.Tables(0).Rows.Count - 1

                        iNotificationTeamMemberID = dsNotificationList.Tables(0).Rows(iRowCounter).Item("TeamMemberID")

                        If iNotificationTeamMemberID > 0 Then
                            dsTeamMember = SecurityModule.GetTeamMember(iNotificationTeamMemberID, "", "", "", "", "", True, Nothing)
                            If dsTeamMember IsNot Nothing Then
                                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then

                                    If InStr(strEmailToAddress, dsTeamMember.Tables(0).Rows(0).Item("Email").ToString, CompareMethod.Binary) <= 0 Then
                                        If strEmailToAddress <> "" Then
                                            strEmailToAddress &= ";"
                                        End If

                                        If dsTeamMember.Tables(0).Rows(0).Item("Email").ToString <> "" Then
                                            strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                                            'update notification date of row
                                            CostingModule.UpdateCostSheetPostApprovalItem(ViewState("CostSheetID"), iNotificationTeamMemberID)
                                        End If

                                    End If

                                End If ' If dsTeamMember.Tables.Count > 0
                            End If ' If dsTeamMember IsNot Nothing
                        End If ' If iNotificationTeamMemberID > 0
                    Next
                End If ' If dsNotificationList.Tables.Count > 0 
            End If ' If dsNotificationList IsNot Nothing 

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        GetNotificationList = strEmailToAddress

    End Function
    Protected Function SendEmail(ByVal EmailToAddress As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try
            Dim strEmailCCAddress As String = ""
            Dim iRowCounter As Integer = 0
            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strPreviewCostFormURL As String = strProdOrTestEnvironment & "Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & ViewState("CostSheetID")
            Dim strPreviewDieLayoutURL As String = strProdOrTestEnvironment & "Costing/Die_Layout_Preview.aspx?CostSheetID=" & ViewState("CostSheetID")

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"
            strEmailCCAddress = strEmailFromAddress

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
                strBody &= "<h1>This information is purely for testing and is NOT valid!!!</h1><br /><br />"
            End If

            strSubject &= "Quote Post Approval Notification for Cost Sheet: " & ViewState("CostSheetID")

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            strBody &= "<font size='3' face='Verdana'>The following information refers to the approved Quote/Cost Form.</font><br /><br />"
            strBody &= "<font size='2' face='Verdana'>Cost Sheet: <b>" & ViewState("CostSheetID") & "</b></font><br />"

            If txtCommentsValue.Text.Trim <> "" Then
                strBody &= "<br /><font size='2' face='Verdana'>" & txtCommentsValue.Text.Trim & "</font><br /><br />"
            End If

            'strBody &= "Cost Sheet:<br />"
            strBody &= "<a href='" & strPreviewCostFormURL & "'><b><u>Click here to Preview the Cost Sheet</u></b></a><br /><br />"

            If ViewState("isDiecut") = True Then
                'strBody &= "Die Layout:<br /><br />"
                strBody &= "<a href='" & strPreviewDieLayoutURL & "'><b><u>Click here to Preview the Die Layout</u></b></a><br /><br />"
            End If

            If ViewState("RFDNo") > 0 Then
                strBody &= "<font size='2' face='Verdana'>RFQ/RFC No. : " & ViewState("RFDNo").ToString & "</font><br />"
            End If

            If ViewState("NewCustomerPartNo") <> "" Then
                strBody &= "<font size='2' face='Verdana'>New Customer PartNo : " & ViewState("NewCustomerPartNo") & "</font><br />"
            End If

            If ViewState("NewDesignLevel") <> "" Then
                strBody &= "<font size='2' face='Verdana'>New Design Level : " & ViewState("NewDesignLevel") & "</font><br />"
            End If

            If ViewState("NewPartName") <> "" Then
                strBody &= "<font size='2' face='Verdana'>Part Name : " & ViewState("NewPartName") & "</font><br />"
            End If

            If ViewState("NewDrawingNo") <> "" Then
                strBody &= "<font size='2' face='Verdana'>New DrawingNo: " & ViewState("NewDrawingNo") & "</font><br />"
            End If

            If ViewState("OldPartNo") <> "" Then
                strBody &= "<font size='2' face='Verdana'>PartNo: " & ViewState("OldPartNo") & "</font><br />"
            End If

            If ViewState("OldFinishedGoodPartNo") <> "" Then
                strBody &= "<font size='2' face='Verdana'>PartNo: " & ViewState("OldFinishedGoodPartNo") & "</font><br />"
            End If

            If ViewState("NewPartNo") <> "" Then
                strBody &= "<font size='2' face='Verdana'>BPCS PartNo: " & ViewState("NewPartNo") & "</font>"

                If ViewState("NewPartRevision") <> "" Then
                    strBody &= "<font size='2' face='Verdana'>Revision: " & ViewState("NewPartRevision") & "</font><br />"
                End If
            End If

            If ViewState("FinishedGoodPart") <> "" Then
                strBody &= "<font size='2' face='Verdana'>Finished Good BPCS Part(s):<br /> " & ViewState("FinishedGoodPart") & "</font><br />"
            End If

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br /><br />Email To Address List: " & EmailToAddress & "<br />"
                strBody &= "<br />Email CC Address List: " & strEmailCCAddress & "<br />"

                EmailToAddress = "Roderick.Carlson@ugnauto.com"
                strEmailCCAddress = ""
            End If

            'set the content
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = EmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    mail.To.Add(emailList(i))
                End If
            Next i

            'build email CC List
            If strEmailCCAddress IsNot Nothing Then
                emailList = strEmailCCAddress.Split(";")

                For i = 0 To UBound(emailList)
                    If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                        mail.CC.Add(emailList(i))
                    End If
                Next i
            End If

            'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "<br>Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "<br>Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("Costing Post Approval Notification", strEmailFromAddress, EmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            End Try


            bReturnValue = True
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        SendEmail = bReturnValue

    End Function
    Protected Sub BindData()

        Try

            Dim ds As DataSet
            Dim dsTopLevelPartInfo As DataSet
            Dim dsRFD As DataSet

            Dim iRowCounter As Integer = 0

            ds = CostingModule.GetCostSheetPostApprovalComments(ViewState("CostSheetID"))

            If commonFunctions.CheckDataSet(ds) = True Then
                txtCommentsValue.Text = ds.Tables(0).Rows(0).Item("PostApprovalComments").ToString
            End If

            ds = CostingModule.GetCostSheet(ViewState("CostSheetID"))
            If commonFunctions.CheckDataSet(ds) = True Then

                If ds.Tables(0).Rows(0).Item("PreviousCostSheetID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PreviousCostSheetID") > 0 Then
                        txtChooseAnotherCostSheetID.Text = ds.Tables(0).Rows(0).Item("PreviousCostSheetID")
                    End If
                End If

                ViewState("isDiecut") = ds.Tables(0).Rows(0).Item("isDiecut")

                ViewState("NewCustomerPartNo") = ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString
                ViewState("NewDesignLevel") = ds.Tables(0).Rows(0).Item("NewDesignLevel").ToString
                ViewState("NewPartName") = ds.Tables(0).Rows(0).Item("NewPartName").ToString
                ViewState("NewDrawingNo") = ds.Tables(0).Rows(0).Item("NewDrawingNo").ToString

                ViewState("isDiecut") = ds.Tables(0).Rows(0).Item("isDiecut")

                ViewState("OldPartNo") = ds.Tables(0).Rows(0).Item("OldPartNo").ToString
                ViewState("OldFinishedGoodPartNo") = ds.Tables(0).Rows(0).Item("OldFinishedGoodPartNo").ToString

                ViewState("NewPartNo") = ds.Tables(0).Rows(0).Item("NewBPCSPartNo").ToString
                ViewState("NewPartRevision") = ds.Tables(0).Rows(0).Item("NewBPCSPartRevision").ToString

                ViewState("RFDNewCustomerPartNo") = ""
                ViewState("bBusinessAwarded") = False

                If ds.Tables(0).Rows(0).Item("RFDNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RFDNo") > 0 Then
                        ViewState("RFDNo") = ds.Tables(0).Rows(0).Item("RFDNo")

                        dsRFD = RFDModule.GetRFD(ViewState("RFDNo"))

                        If commonFunctions.CheckDataSet(dsRFD) = True Then
                            hlnkRFD.Visible = True
                            hlnkRFD.NavigateUrl = "~/RFD/RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo")

                            ViewState("RFDNewCustomerPartNo") = dsRFD.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString

                            If dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID") IsNot System.DBNull.Value Then
                                If dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID") > 0 Then
                                    ViewState("BusinessProcessTypeID") = dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID")
                                End If
                            End If

                            If dsRFD.Tables(0).Rows(0).Item("BusinessProcessActionID") IsNot System.DBNull.Value Then
                                If dsRFD.Tables(0).Rows(0).Item("BusinessProcessActionID") > 0 Then
                                    ViewState("BusinessProcessActionID") = dsRFD.Tables(0).Rows(0).Item("BusinessProcessActionID")
                                End If
                            End If

                            If dsRFD.Tables(0).Rows(0).Item("BusinessAwardDate").ToString <> "" Then
                                ViewState("bBusinessAwarded") = True
                            End If

                        End If
                    End If
                End If

                dsTopLevelPartInfo = CostingModule.GetCostSheetTopLevelPartInfo(ViewState("CostSheetID"))
                If commonFunctions.CheckDataSet(dsTopLevelPartInfo) = True Then
                    For iRowCounter = 0 To dsTopLevelPartInfo.Tables(0).Rows.Count - 1
                        ViewState("FinishedGoodPart") &= dsTopLevelPartInfo.Tables(0).Rows(iRowCounter).Item("PartNo").ToString.PadRight(15, " ") & "   REVISION: " & dsTopLevelPartInfo.Tables(0).Rows(iRowCounter).Item("PartRevision").ToString.PadRight(2, " ") & "    NAME: " & dsTopLevelPartInfo.Tables(0).Rows(iRowCounter).Item("PartName").ToString & "<br />"
                    Next
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
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
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                If iTeamMemberID = 530 Then
                    iTeamMemberID = 582 ' Bill Schultz                                     
                End If

                ViewState("TeamMemberID") = iTeamMemberID

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 57)
                If commonFunctions.CheckDataSet(dsRoleForm) = True Then
                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                            ViewState("isRestricted") = False
                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                            ViewState("isRestricted") = True
                    End Select
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub EnableControls()

        Try

            gvPostApproval.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then

                lblCostSheetLabel.Visible = ViewState("isAdmin")
                lblCostSheetValue.Visible = ViewState("isAdmin")
                lblCommentsLabel.Visible = ViewState("isAdmin")
                txtCommentsValue.Visible = ViewState("isAdmin")
                btnSave.Visible = ViewState("isAdmin")
                btnNotify.Visible = ViewState("isAdmin")

                gvPostApproval.Columns(gvPostApproval.Columns.Count - 1).Visible = ViewState("isAdmin")

                If gvPostApproval.FooterRow IsNot Nothing Then
                    gvPostApproval.FooterRow.Visible = ViewState("isAdmin")
                End If

                If ViewState("isAdmin") = True Then

                End If
            Else
                lblMessage.Text &= "You do not have access to this information. Please contact the Costing Manager."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            CheckRights()

            If Not Page.IsPostBack Then

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then

                    ViewState("CostSheetID") = CType(HttpContext.Current.Request.QueryString("CostSheetID"), Integer)
                    ViewState("isDiecut") = False
                    If ViewState("CostSheetID") > 0 Then
                        BindData()
                    End If
                    lblCostSheetValue.Text = ViewState("CostSheetID")

                End If

            End If

            If ViewState("CostSheetID") > 0 Then
                EnableControls()
            End If

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Cost Sheet Post-Approval List"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > <b> <a href='Cost_Sheet_Detail.aspx?CostSheetID=" & ViewState("CostSheetID") & "'> Cost Sheet Detail </a> </b> > Cost Sheet Post-Approval List  "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            txtCommentsValue.Attributes.Add("onkeypress", "return tbLimit();")
            txtCommentsValue.Attributes.Add("onkeyup", "return tbCount(" + lblCommentsCharCount.ClientID + ");")
            txtCommentsValue.Attributes.Add("maxLength", "400")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub gvPostApproval_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPostApproval.DataBound

        ''hide header of first column
        'If gvPostApproval.Rows.Count > 0 Then
        '    gvPostApproval.HeaderRow.Cells(0).Visible = False
        'End If

    End Sub
    Protected Sub gvPostApproval_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPostApproval.RowCommand

        Try

            Dim ddTeamMemberNameTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddTeamMemberNameTemp = CType(gvPostApproval.FooterRow.FindControl("ddFooterPostApprovalTeamMember"), DropDownList)

                odsPostApproval.InsertParameters("CostSheetID").DefaultValue = ViewState("CostSheetID")
                odsPostApproval.InsertParameters("TeamMemberID").DefaultValue = ddTeamMemberNameTemp.SelectedValue

                intRowsAffected = odsPostApproval.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvPostApproval.ShowFooter = False
            Else
                gvPostApproval.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddTeamMemberNameTemp = CType(gvPostApproval.FooterRow.FindControl("ddFooterPostApprovalTeamMember"), DropDownList)
                ddTeamMemberNameTemp.SelectedIndex = -1

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_PostApproval() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_PostApproval") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_PostApproval"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_PostApproval") = value
        End Set

    End Property
    Protected Sub odsPostApproval_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPostApproval.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.CostSheetPostApproval_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetPostApproval_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_PostApproval = True
            Else
                LoadDataEmpty_PostApproval = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub gvPostApproval_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPostApproval.RowCreated

        Try
            ''hide first column
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_PostApproval
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            lblMessage.Text = ""

            CostingModule.UpdateCostSheetPostApprovalComments(ViewState("CostSheetID"), txtCommentsValue.Text)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Private Function isAllRFDApprovedBeforeCosting() As Boolean

        Dim bRetVal As Boolean = False

        Try

            Dim ds As DataSet
            Dim iTempSubscriptionID As Integer = 0
            Dim iTempStatusID As Integer = 1

            ViewState("CapitalStatusID") = 0
            ViewState("PackagingStatusID") = 0
            ViewState("PlantControllerStatusID") = 0
            ViewState("ProcessStatusID") = 0
            ViewState("ProductDevelopmentStatusID") = 0
            ViewState("ToolingStatusID") = 0
            ViewState("PurchasingExternalRFQStatusID") = 0

            ds = RFDModule.GetRFDApproval(ViewState("RFDNo"), 0, 0, False, False, False, False, False)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    iTempSubscriptionID = 0
                    If ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID") > 0 Then
                            iTempSubscriptionID = ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID")
                        End If
                    End If

                    iTempStatusID = 1
                    If ds.Tables(0).Rows(iRowCounter).Item("StatusID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StatusID") > 0 Then
                            iTempStatusID = ds.Tables(0).Rows(iRowCounter).Item("StatusID")
                        End If
                    End If

                    If iTempSubscriptionID = 119 Then
                        ViewState("CapitalStatusID") = iTempStatusID
                    End If

                    If iTempSubscriptionID = 108 Then
                        ViewState("PackagingStatusID") = iTempStatusID
                    End If

                    If iTempSubscriptionID = 20 Then
                        ViewState("PlantControllerStatusID") = iTempStatusID
                    End If

                    If iTempSubscriptionID = 66 Then
                        ViewState("ProcessStatusID") = iTempStatusID
                    End If

                    If iTempSubscriptionID = 5 Then
                        ViewState("ProductDevelopmentStatusID") = iTempStatusID
                    End If

                    If iTempSubscriptionID = 65 Then
                        ViewState("ToolingStatusID") = iTempStatusID
                    End If

                    If iTempSubscriptionID = 139 Then
                        ViewState("PurchasingExternalRFQStatusID") = iTempStatusID
                    End If
                Next
            End If

            If ViewState("ProductDevelopmentStatusID") = 3 And _
                (ViewState("CapitalStatusID") = 0 Or ViewState("CapitalStatusID") = 3) And _
                (ViewState("PackagingStatusID") = 0 Or ViewState("PackagingStatusID") = 3) And _
                (ViewState("PlantControllerStatusID") = 0 Or ViewState("PlantControllerStatusID") = 3) And _
                (ViewState("ProcessStatusID") = 0 Or ViewState("ProcessStatusID") = 3) And _
                (ViewState("ToolingStatusID") = 0 Or ViewState("ToolingStatusID") = 3) And _
                (ViewState("PurchasingExternalRFQStatusID") = 0 Or ViewState("PurchasingExternalRFQStatusID") = 3) Then
                bRetVal = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return bRetVal

    End Function
    Private Function UpdateRFDQualityEngineerApprover() As Boolean

        Dim bRetVal As Boolean = False

        Try
            Dim ds As DataSet

            ds = RFDModule.GetRFDApproval(ViewState("RFDNo"), 22, 0, False, False, False, False, False)
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then
                        'if QE assigned then set to IN-Process
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 22, ds.Tables(0).Rows(0).Item("TeamMemberID"), "", 0, 2, Today.Date)
                        lblMessage.Text &= "<br>Quality Engineer for RFD has been set to In-Process."
                        bRetVal = True
                    End If
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return bRetVal

    End Function

    Protected Sub btnNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotify.Click

        Try
            lblMessage.Text = ""

            Dim strEmailToAddress As String = ""
            Dim iRowCounter As Integer = 0
            Dim dsRFDChild As DataSet

            strEmailToAddress = GetNotificationList(cbNotifyOnlyNew.Checked, False)

            If strEmailToAddress <> "" Then
                CostingModule.UpdateCostSheetApproved(ViewState("CostSheetID"))

                If ViewState("RFDNo") > 0 Then

                    'push CostSheetID back to RFD Top Level if New Customer Part Numbers match
                    If (ViewState("RFDNewCustomerPartNo") = ViewState("NewCustomerPartNo") Or ViewState("RFDNewCustomerPartNo") = ViewState("NewPartNo")) And ViewState("NewCustomerPartNo") <> "" Then
                        RFDModule.UpdateRFDFromCosting(ViewState("RFDNo"), ViewState("CostSheetID"))

                        'also if there are no child parts referenced on RFD then save costing approval
                        dsRFDChild = RFDModule.GetRFDChildPart(0, ViewState("RFDNo"))
                        If commonFunctions.CheckDataSet(dsRFDChild) = False Then
                            RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, ViewState("TeamMemberID"), "", 0, 3, "")
                            lblMessage.Text &= "<br>RFD " & ViewState("RFDNo") & " Approved"

                            If UpdateRFDQualityEngineerApprover() = False And isAllRFDApprovedBeforeCosting() = True Then
                                'if Quote Only Source Quote Waiting for Business Award then notify Sales
                                If (ViewState("BusinessProcessTypeID") = 7 And ViewState("BusinessProcessActionID") = 10 And ViewState("bBusinessAwarded") = True) Or _
                                    (ViewState("BusinessProcessTypeID") = 7 And ViewState("BusinessProcessActionID") <> 10) Or _
                                    ViewState("BusinessProcessTypeID") <> 7 Then
                                    RFDModule.UpdateRFDOverallStatus(ViewState("RFDNo"), 3)
                                    lblMessage.Text &= "<br>RFD is Approved at all levels and complete"
                                End If
                            End If

                        End If
                    End If


                End If

                If SendEmail(strEmailToAddress) = True Then
                    'lblMessage.Text &= "Notifications were successfully sent."
                    gvPostApproval.DataBind()
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub iBtnGetAnotherCostSheetPostApprovalList_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnGetAnotherCostSheetPostApprovalList.Click

        Try
            lblMessage.Text = ""

            Dim bFountIt As Boolean = False
            Dim iOldCostSheetID As Integer = 0

            If txtChooseAnotherCostSheetID.Text.Trim <> "" Then
                iOldCostSheetID = CType(txtChooseAnotherCostSheetID.Text.Trim, Integer)

                'check to make sure the other cost sheet actually exists
                Dim ds As DataSet
                ds = CostingModule.GetCostSheet(ViewState("CostSheetID"))

                If commonFunctions.CheckDataset(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("Obsolete") = False Then
                        bFountIt = True
                    End If
                End If

                If bFountIt = True Then
                    CostingModule.CopyCostSheetPostApprovalList(ViewState("CostSheetID"), iOldCostSheetID)
                    lblMessage.Text &= "The notification list has been copied from a previous cost sheet."
                    gvPostApproval.DataBind()
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
End Class
