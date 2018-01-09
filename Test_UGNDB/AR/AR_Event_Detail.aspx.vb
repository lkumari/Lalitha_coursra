' ************************************************************************************************
'
' Name:		AR_Event_Detail.aspx
' Purpose:	This Code Behind is for the AR Event Detail
'
' Date		Author	    
' 03/25/2010   Roderick Carlson
'
' Event Types
' 4 Accounting Accrual
' 3 Customer Accrual
' 5 Invoice On Hold (No Accrual)
' 2 Part Accrual
' 1 Price Change (No Accrual) 

' ROUTING LEVELS
' Level 1 - Default Accounting Manager - Subscription ID 79  
' Level 2 - Sales - Subscription ID 9
' Level 3 - VP of Sales - Subscription ID 23
' Level 4 - CFO - Subscription ID 33
' Level 5 - CEO  - Subscription 24

' Modified 07/20/2011 - Roderick Carlson - added Notify Update Button for Sales to press to notify Accounting
'                                          added a subscription Notify Billing so that only certain Accounting Team members are notified instead of the entire group
'                                          allow btnNotifyPriceUpdatedByAccounting to always appear for Billing
'                                          Notify Billing if Price/Percent is updated
'                                          Changed Deduction Label to say Deduction/Recovery
' Modified 08/24/2011 - Roderick Carlson - adjust end date logic when copying and only wipe out event details for certain copy scenarios
' Modified 10/04/2011 - Roderick Carlson - Gina Lacny - do not copy Calculated Accrual to Final Accrual until closing
' Modified 04/16/2012 - Roderick Carlson - ref# ART-3157 - add export to excel button for selection details
' Modified 08/15/2012 - Roderick Carlson - Add Estimated Price to Invoice On Hold Event
' Modified 09/20/2012 - Roderick Carlson - Simplify Approvals
'                                       1) Sales submits (and thus auto approves) the event
'                                       2) Accounting would approve the event
'                                       3) Sales would click the "Customer Approved" button
'                                       4) Accounting would then submit the event (previous from step1 - sales already approved the event)
'                                       5) If over 2500, VP of Sales and CFO approves 
'                                       6) If over 5000, CEO approves 
'                                       7) If all have approved and customer approved, then Accounting can close the event.
' Modified 09/21/2012 - Roderick Carlson - Added logic to copy accrual override criteria
' Modified 10/02/2012 - Roderick Carlson - Allow Accounting Accrual to select UGN Facility and SoldTo
' Modified 12/23/2013 - LRey    Replaced SoldTo/CABBV with Customer and BPCSPartNo with PartNo wherever used.

Partial Class AR_Event_Detail
    Inherits System.Web.UI.Page
    Private htControls As New System.Collections.Hashtable

    Private Sub BindCriteria()

        Try
            'bind existing data to drop down controls for selection criteria for search       

            Dim ds As DataSet

            ds = ARGroupModule.GetAREventStatusList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddEventStatus.DataSource = ds
                ddEventStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddEventStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddEventStatus.DataBind()
                ddEventStatus.Items.Insert(0, "")
            End If

            ds = ARGroupModule.GetAREventTypeList(False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddEventType.DataSource = ds
                ddEventType.DataTextField = ds.Tables(0).Columns("ddEventTypeName").ColumnName.ToString()
                ddEventType.DataValueField = ds.Tables(0).Columns("EventTypeID").ColumnName
                ddEventType.DataBind()
                'ddEventType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetTeamMemberBySubscription(9)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddAccountManager.DataBind()
                '  ddAccountManager.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InitializeViewState()

        Try
            ViewState("EventStatusID") = 0
            ViewState("EventTypeID") = 0

            ViewState("TeamMemberID") = 0
            ViewState("CurrentRSSID") = 0

            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            ViewState("SubscriptionID") = 0

            ViewState("ActiveApproverEmail") = ""
            ViewState("AcctMgrTMID") = 0
            ViewState("AcctMgrEmail") = ""
            ViewState("SalesApprovalStatusID") = 0

            ViewState("isDefaultBilling") = False
            ViewState("DefaultBillingTMID") = 0
            ViewState("BillingEmail") = ""
            ViewState("DefaultBillingEmail") = ""
            ViewState("BillingApprovalStatusID") = 0
            ViewState("isBillingNotified") = False

            ViewState("PlantControllerEmail") = ""

            ViewState("VPSalesTMID") = 0
            ViewState("VPSalesEmail") = ""
            ViewState("VPSalesApprovalStatusID") = 0

            ViewState("CFOTMID") = 0
            ViewState("CFOEmail") = ""
            ViewState("CFOApprovalStatusID") = 0

            ViewState("CEOTMID") = 0
            ViewState("CEOEmail") = ""
            ViewState("CEOApprovalStatusID") = 0

            ViewState("pRC") = 0

            ViewState("SalesApprovalRowID") = 0
            ViewState("BillingApprovalRowID") = 0
            ViewState("VPSalesApprovalRowID") = 0
            ViewState("CFOApprovalRowID") = 0
            ViewState("CEOApprovalRowID") = 0

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub BindData()

        Try
            Dim ds As DataSet

            ds = ARGroupModule.GetAREvent(ViewState("AREID"))

            If commonFunctions.CheckDataSet(ds) = True Then

                If ds.Tables(0).Rows(0).Item("isCustomerApproved") IsNot System.DBNull.Value Then
                    cbCustomerApproved.Checked = ds.Tables(0).Rows(0).Item("isCustomerApproved")
                End If

                ViewState("AcctMgrTMID") = 0
                If ds.Tables(0).Rows(0).Item("AcctMgrTMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("AcctMgrTMID") > 0 Then
                        ViewState("AcctMgrTMID") = ds.Tables(0).Rows(0).Item("AcctMgrTMID")
                        ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AcctMgrTMID")
                    End If
                End If

                txtBPCSInvoiceNo.Text = ds.Tables(0).Rows(0).Item("BPCSInvoiceNo").ToString

                If ds.Tables(0).Rows(0).Item("EventStatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("EventStatusID") > 0 Then
                        ddEventStatus.SelectedValue = ds.Tables(0).Rows(0).Item("EventStatusID")
                        ViewState("EventStatusID") = ds.Tables(0).Rows(0).Item("EventStatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("EventTypeID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("EventTypeID") > 0 Then
                        ddEventType.SelectedValue = ds.Tables(0).Rows(0).Item("EventTypeID")
                        ViewState("EventTypeID") = ddEventType.SelectedValue
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CalculatedQuantityShipped") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CalculatedQuantityShipped") <> 0 Then
                        lblQuantityShippedValue.Text = Format(ds.Tables(0).Rows(0).Item("CalculatedQuantityShipped"), "#,##0")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CalculatedDeductionAmount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CalculatedDeductionAmount") <> 0 Then
                        lblCalculatedDeductionAmountValue.Text = Format(ds.Tables(0).Rows(0).Item("CalculatedDeductionAmount"), "$#,##0.000000")
                        'txtFinalDeductionAmount.Text = ds.Tables(0).Rows(0).Item("CalculatedDeductionAmount")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CloseDate").ToString <> "" Then
                    lblCloseDateValue.Text = ds.Tables(0).Rows(0).Item("CloseDate").ToString
                End If

                txtCustApprvEffDate.Text = ds.Tables(0).Rows(0).Item("CustApprvEffDate").ToString
                txtCustApprvEndDate.Text = ds.Tables(0).Rows(0).Item("CustApprvEndDate").ToString

                If ds.Tables(0).Rows(0).Item("CreditDebitDate").ToString <> "" Then
                    txtCreditDebitDate.Text = ds.Tables(0).Rows(0).Item("CreditDebitDate").ToString
                End If

                If ds.Tables(0).Rows(0).Item("CreditDebitMemo").ToString <> "" Then
                    txtCreditDebitMemo.Text = ds.Tables(0).Rows(0).Item("CreditDebitMemo").ToString
                End If

                txtDeductionReason.Text = ds.Tables(0).Rows(0).Item("DeductionReason").ToString
                txtEventDesc.Text = ds.Tables(0).Rows(0).Item("EventDesc").ToString

                If ds.Tables(0).Rows(0).Item("FinalDeductionAmount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FinalDeductionAmount") <> 0 Then
                        txtFinalDeductionAmount.Text = ds.Tables(0).Rows(0).Item("FinalDeductionAmount")
                    End If
                End If

                'txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString
                txtVoidReason.Text = ds.Tables(0).Rows(0).Item("VoidReason").ToString

                If ds.Tables(0).Rows(0).Item("isPriceUpdatedByAccounting") IsNot System.DBNull.Value Then
                    cbPriceUpdatedByAccounting.Checked = ds.Tables(0).Rows(0).Item("isPriceUpdatedByAccounting")

                    lblPriceChangeDate.Text = ds.Tables(0).Rows(0).Item("PriceChangeDate").ToString

                    'If ds.Tables(0).Rows(0).Item("PriceChangeDate").ToString <> "" Then
                    '    cbPriceUpdatedByAccounting.Visible = True
                    '    lblPriceChangeDate.Visible = True
                    'End If

                End If

                RecalculateTotals()

                'get a few detail items
                ds = ARGroupModule.GetAREventDetail(ViewState("AREID"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("isFuture") IsNot Nothing Then
                        If ds.Tables(0).Rows(0).Item("isFuture") = True Then

                            'if price change no accrual
                            'LR ''If ViewState("EventTypeID") = 1 Then
                            'LR ''    rbSelectionWizard.SelectedValue = "F"
                            'LR ''    'SetFutureParts()
                            'LR ''End If
                        End If
                    End If

                    'if invoice on hold no accrual
                    If ViewState("EventTypeID") = 5 Then
                        txtInvoicePartNo.Text = ds.Tables(0).Rows(0).Item("PartNo").ToString
                        txtInvoicePriceCode.Text = ds.Tables(0).Rows(0).Item("PRCCDE").ToString
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RecalculateTotals()

        Try
            Dim ds As DataSet

            'not closed and not void
            If ViewState("EventStatusID") <> 9 And ViewState("EventStatusID") <> 10 Then
                ds = ARGroupModule.GetAREventAccrualTotals(ViewState("AREID"))

                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("TotalQTYSHP") IsNot System.DBNull.Value Then
                        lblQuantityShippedValue.Text = Format(ds.Tables(0).Rows(0).Item("TotalQTYSHP"), "#,###,###")
                    End If

                    ' ''If ds.Tables(0).Rows(0).Item("TotalOverrideAccrual") IsNot System.DBNull.Value Then
                    ' ''    lblCalculatedDeductionAmountValue.Text = Format(ds.Tables(0).Rows(0).Item("TotalOverrideAccrual"), "$#,###,###,##0.000000")
                    ' ''End If

                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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
            Dim dsSubscription As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("SubscriptionID") = 0
            ViewState("isDefaultBilling") = False

            ViewState("TeamMemberID") = 0

            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Gina.Lacny", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                ''test developer as another team member
                If iTeamMemberID = 530 Then
                    'mike echevarria
                    'iTeamMemberID = 246

                    'Brett.Barta 
                    'iTeamMemberID = 2

                    'gina lacny
                    'iTeamMemberID = 627

                    'gary hibbler
                    'iTeamMemberID = 671

                    'Ilysa.Albright 
                    'iTeamMemberID = 636

                    'Kara.North 
                    'iTeamMemberID = 667

                    'Kelly.Carolyn 
                    'iTeamMemberID = 638

                    'randy.khalaf 
                    'iTeamMemberID = 569

                    'Daniel.Marcon 
                    iTeamMemberID = 612

                    'Kenta.Shinohara 
                    'iTeamMemberID = 4
                End If

                ViewState("TeamMemberID") = iTeamMemberID
                lblTeamMemberID.Text = ViewState("TeamMemberID")

                'Accounting
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 21)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 21
                End If

                'is Default Accounting Manager
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 79)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultBilling") = True
                End If

                'Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 9)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 9
                End If

                'VP of Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 23)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 23
                End If

                'CFO
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 33)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 33
                End If

                'CEO
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 24)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 24
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 49)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)                                    
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isEdit") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select

                    'allow CEO and CFO to be just edit mode. in other pages they will be admin to do an actual approval. that is why they need to be champions
                    If ViewState("SubscriptionID") = 33 Or ViewState("SubscriptionID") = 24 Then
                        ViewState("isAdmin") = False
                    End If
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ClearMessages()

        Try
            lblMessage.Text = ""
            lblMessageBottom.Text = ""
            lblMessageButtons.Text = ""
            lblMessageCommunicationBoard.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub DisableAll()

        Try

            btnAccrual.Visible = False
            btnClose.Visible = False
            btnCopy.Visible = False
            btnCreateAccountingAccrual.Visible = False
            btnCustomerApproved.Visible = False
            btnExportToExcel.Visible = False
            btnNotifyAccounting.Visible = False
            btnNotifyPriceUpdatedByAccounting.Visible = False
            btnPreview.Visible = False
            btnPushAdjustments.Visible = False
            btnReset.Visible = False
            btnSave.Visible = False
            btnSaveReplyComment.Visible = False
            btnResetReplyComment.Visible = False
            'LR ''btnSelectionWizard.Visible = False
            btnSubmitApproval.Visible = False
            'btnUpload.Visible = False
            btnVoid.Visible = False

            cbPriceUpdatedByAccounting.Enabled = False
            cbPriceUpdatedByAccounting.Visible = False
            'cbCustomerApproved.Enabled = False
            cbCustomerApproved.Visible = False

            ddAccountManager.Enabled = False
            ddEventStatus.Visible = False
            ddEventType.Enabled = False

            gvAffectedInvoicesOnHold.Visible = False

            gvApproval.Visible = False
            gvApproval.Columns(gvApproval.Columns.Count - 1).Visible = False

            gvDetail.Visible = False
            gvDetail.Columns(gvDetail.Columns.Count - 1).Visible = False

            gvInvoicesOnHold.Visible = False

            'gvPriceMaster.Visible = False
            gvSupportingDoc.Visible = False
            gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = False

            imgCreditDebitDate.Visible = False
            imgCustApprvEffDate.Visible = False
            imgCustApprvEndDate.Visible = False

            lblAffectedInvoicesOnHoldLabel.Visible = False
            lblAREID.Visible = False
            lblCloseDateLabel.Visible = False
            lblCloseDateValue.Visible = False
            lblCreditDebitDate.Visible = False
            lblCreditDebitMemo.Visible = False
            lblCurrentInvoicesOnHoldLabel.Visible = False
            lblCustApprvEffDateNote.Visible = False
            lblCustApprvEndDate.Visible = False
            lblEventStatus.Visible = False
            lblMessageAREIDNew.Visible = False
            lblPricePercent.Visible = False
            lblPricePercentSign.Visible = False
            lblPricePercentDecimal.Visible = False
            lblPriceDollar.Visible = False
            lblSQC.Visible = False

            menuTabs.Visible = False

            ' ''tblPriceAdjustment.Visible = False
            tblUpload.Visible = False

            tblCommunicationBoardExistingQuestion.Visible = False
            tblCommunicationBoardNewQuestion.Visible = False

            txtBPCSInvoiceNo.Enabled = False
            txtBPCSInvoiceNo.Visible = False

            txtCreditDebitDate.Enabled = False
            txtCreditDebitDate.Visible = False

            txtCreditDebitMemo.Enabled = False
            txtCreditDebitMemo.Visible = False

            txtCustApprvEffDate.Enabled = False

            txtCustApprvEndDate.Enabled = False
            txtCustApprvEndDate.Visible = False

            'txtDeductionInstr.Enabled = False
            'txtDeductionInstr.Visible = False
            txtDeductionReason.Enabled = False
            txtDeductionReason.Visible = False
            txtEventDesc.Enabled = False
            txtFinalDeductionAmount.Enabled = False
            'txtNotes.Enabled = False
            txtPricePercent.Visible = False
            txtPriceDollar.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub GetTeamMemberInfo()

        Try
            Dim dsFacility As DataSet
            Dim strFacility As String = ""

            Dim dsTeamMember As DataSet

            Dim dtApproval As DataTable
            Dim objARApprovalBLL As ARApprovalBLL = New ARApprovalBLL

            Dim iRowCounter As Integer = 0
            Dim jRowCounter As Integer = 0

            Dim iApprovalRowID As Integer = 0
            Dim iApprovalStatusID As Integer = 0
            Dim iApproverTMID As Integer = 0
            Dim iApproverSubscriptionID As Integer = 0

            ViewState("BillingEmail") = ""

            'dsTeamMember = commonFunctions.GetTeamMemberBySubscription(21)
            'instead of notifying the entire accounting group, just notify certain team members
            dsTeamMember = commonFunctions.GetTeamMemberBySubscription(35)
            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                For iRowCounter = 0 To dsTeamMember.Tables(0).Rows.Count - 1
                    If dsTeamMember.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then
                        If dsTeamMember.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                            If InStr(dsTeamMember.Tables(0).Rows(iRowCounter).Item("TMName"), "**") <= 0 Then
                                If ViewState("BillingEmail") <> "" Then
                                    ViewState("BillingEmail") &= ";"
                                End If

                                ViewState("BillingEmail") &= dsTeamMember.Tables(0).Rows(iRowCounter).Item("Email").ToString
                            End If
                        End If
                    End If
                Next
            End If

            If ViewState("AcctMgrTMID") > 0 Then
                ViewState("AcctMgrEmail") = ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))
            End If

            ViewState("DefaultBillingTMID") = GetTeamMemberIDBySubscriptionID(79, "")
            ViewState("DefaultBillingEmail") = ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

            ViewState("VPSalesTMID") = GetTeamMemberIDBySubscriptionID(23, "")
            ViewState("VPSalesEmail") = ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)

            ViewState("CFOTMID") = GetTeamMemberIDBySubscriptionID(33, "")
            ViewState("CFOEmail") = ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(33, "", False)

            ViewState("CEOTMID") = GetTeamMemberIDBySubscriptionID(24, "")
            ViewState("CEOEmail") = ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(24, "", False)

            dtApproval = objARApprovalBLL.GetAREventApprovalStatus(ViewState("AREID"), 0)

            ViewState("ActiveApproverEmail") = ""

            ViewState("SalesApprovalStatusID") = 0
            ViewState("BillingApprovalStatusID") = 0
            ViewState("VPSalesApprovalStatusID") = 0
            ViewState("CFOApprovalStatusID") = 0
            ViewState("CEOApprovalStatusID") = 0

            ViewState("SalesApprovalRowID") = 0
            ViewState("BillingApprovalRowID") = 0
            ViewState("VPSalesApprovalRowID") = 0
            ViewState("CFOApprovalRowID") = 0
            ViewState("CEOApprovalRowID") = 0

            'get all approvers
            If commonFunctions.CheckDataTable(dtApproval) = True Then
                For iRowCounter = 0 To dtApproval.Rows.Count - 1
                    iApprovalRowID = CType(dtApproval.Rows(iRowCounter).Item("RowID").ToString, Integer)
                    iApprovalStatusID = CType(dtApproval.Rows(iRowCounter).Item("StatusID").ToString, Integer)
                    iApproverSubscriptionID = CType(dtApproval.Rows(iRowCounter).Item("SubscriptionID").ToString, Integer)

                    If iApproverSubscriptionID = 9 Then
                        ViewState("SalesApprovalRowID") = iApprovalRowID
                        ViewState("SalesApprovalStatusID") = iApprovalStatusID
                    End If

                    If iApproverSubscriptionID = 21 Then
                        ViewState("BillingApprovalRowID") = iApprovalRowID
                        ViewState("BillingApprovalStatusID") = iApprovalStatusID
                    End If

                    If iApproverSubscriptionID = 23 Then
                        ViewState("VPSalesApprovalRowID") = iApprovalRowID
                        ViewState("VPSalesApprovalStatusID") = iApprovalStatusID
                    End If

                    If iApproverSubscriptionID = 33 Then
                        ViewState("CFOApprovalRowID") = iApprovalRowID
                        ViewState("CFOApprovalStatusID") = iApprovalStatusID
                    End If

                    If iApproverSubscriptionID = 24 Then
                        ViewState("CEOApprovalRowID") = iApprovalRowID
                        ViewState("CEOApprovalStatusID") = iApprovalStatusID
                    End If

                    'only include approvers who are pending, rejected, or approved NOT OPEN (thus not active) but still working
                    If iApprovalStatusID > 1 Then
                        iApproverTMID = CType(dtApproval.Rows(iRowCounter).Item("TeamMemberID").ToString, Integer)

                        dsTeamMember = SecurityModule.GetTeamMember(iApproverTMID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                            If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                                If dsTeamMember.Tables(0).Rows(0).Item("Working") = True Then
                                    If ViewState("ActiveApproverEmail") <> "" Then
                                        ViewState("ActiveApproverEmail") &= ";"
                                    End If

                                    ViewState("ActiveApproverEmail") &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            ViewState("PlantControllerEmail") = ""
            'get Plant Controllers per referenced UGN Facility
            dsFacility = ARGroupModule.GetAREventFacility(ViewState("AREID"))
            If commonFunctions.CheckDataSet(dsFacility) = True Then
                For iRowCounter = 0 To dsFacility.Tables(0).Rows.Count - 1
                    strFacility = dsFacility.Tables(0).Rows(iRowCounter).Item("COMPNY").ToString.Trim

                    dsTeamMember = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(20, strFacility)
                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        For jRowCounter = 0 To dsTeamMember.Tables(0).Rows.Count - 1
                            If dsTeamMember.Tables(0).Rows(jRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then
                                If dsTeamMember.Tables(0).Rows(jRowCounter).Item("WorkStatus") = True Then
                                    If InStr(dsTeamMember.Tables(0).Rows(jRowCounter).Item("TMName"), "**") <= 0 Then
                                        If ViewState("PlantControllerEmail") <> "" Then
                                            ViewState("PlantControllerEmail") &= ";"
                                        End If

                                        ViewState("PlantControllerEmail") &= dsTeamMember.Tables(0).Rows(jRowCounter).Item("Email").ToString
                                    End If
                                End If
                            End If
                        Next
                    End If

                Next
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnRSSSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSSSubmit.Click

        Try
            ClearMessages()

            GetTeamMemberInfo()

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            'Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "AR/crPreview_AR_Event_Detail.aspx?AREID=" & ViewState("AREID")
            'Dim strEmailDetailURL As String = strProdOrTestEnvironment & "AR/AR_Event_Detail.aspx?AREID=" & ViewState("AREID")

            Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim strEmailBody As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            'update RSS Question List
            ARGroupModule.InsertARRSS(ViewState("AREID"), ViewState("TeamMemberID"), ViewState("SubscriptionID"), txtRSSComment.Text.Trim)

            'update AR Event History
            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Message Sent:" & txtRSSComment.Text.Trim)

            'current user is billing, then notify sales
            If ViewState("SubscriptionID") = 21 Then
                strEmailToAddress = ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))                
            End If

            'current user is sales, then notify default billing
            If ViewState("SubscriptionID") = 9 Then
                strEmailToAddress = ViewState("DefaultBillingEmail") ' ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)
            End If

            'include interested billing team members
            If ViewState("BillingEmail") <> "" Then
                If strEmailCCAddress <> "" Then
                    strEmailCCAddress &= ";"
                End If

                strEmailCCAddress = ViewState("BillingEmail")
            End If

            'append active approvers
            If ViewState("ActiveApproverEmail") <> "" Then
                If strEmailCCAddress <> "" Then
                    strEmailCCAddress &= ";"
                End If

                strEmailCCAddress = ViewState("ActiveApproverEmail")
            End If

            'current user is vp of sales
            If ViewState("SubscriptionID") = 23 Then
                'notify default billing
                strEmailToAddress = ViewState("DefaultBillingEmail") ' ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                If strEmailToAddress <> "" Then
                    strEmailToAddress &= ";"
                End If

                'notify sales
                strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

            End If

            'current user is vp of finance
            If ViewState("SubscriptionID") = 33 Then

                ''notify default billing
                strEmailToAddress = ViewState("DefaultBillingEmail")  'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                'if not accounting accrual
                If ViewState("EventTypeID") <> 4 Then
                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    'notify sales
                    strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    'notify VP of sales but no backup
                    strEmailToAddress &= ViewState("VPSalesEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)
                End If

            End If

            'current user is CEO
            If ViewState("SubscriptionID") = 24 Then
                'notify default billing
                strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                If strEmailToAddress <> "" Then
                    strEmailToAddress &= ";"
                End If

                'notify sales
                strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

                If strEmailToAddress <> "" Then
                    strEmailToAddress &= ";"
                End If

                'notify VP of sales but no backup
                strEmailToAddress &= ViewState("VPSalesEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)

                If strEmailToAddress <> "" Then
                    strEmailToAddress &= ";"
                End If

                'notify CFO but no backup
                strEmailToAddress &= ViewState("CFOEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(33, "", False)
            End If

            ''''''''''''''''''''''''''''''''''
            ''Build Email
            ''''''''''''''''''''''''''''''''''

            'assign email subject
            strEmailSubject = "AR Question  - Event ID: " & ViewState("AREID") & " - MESSAGE receiveD"

            strEmailBody = "<table><tr><td valign='top' width='20%'><img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger.jpg'  height='20%' width='20%'/></td><td valign='top'>"
            'strEmailBody &= "<font size='3' face='Verdana'><b>Attention</b> "
            strEmailBody &= "<font size='3' face='Verdana'><p><b>" & strCurrentUserFullName & "</b> sent you message regarding AR Event ID: <font color='red'>" & ViewState("AREID") & "</font><br />"
            strEmailBody &= "<font size='3' face='Verdana'><p><b>Event Description:</b> <font>" & txtEventDesc.Text.Trim & "</font>.</p><br />"
            strEmailBody &= "<p><b>Question: </b><font>" & txtRSSComment.Text.Trim & "</font></p><br /><br />"

            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br /><br />"
            strEmailBody &= "<p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "&pRC=1" & "'>Click here</a> to answer the message.</font>"
            strEmailBody &= "</td></tr><tr><td colspan='2'>"

            SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, True)

            gvQuestion.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Private Sub HandlePercentDollarChoice()

        Try
            lblPricePercent.Visible = False
            lblPricePercentSign.Visible = False
            lblPricePercentDecimal.Visible = False
            'lblPricePercentNote.Visible = False

            txtPricePercent.Visible = False

            lblPriceDollar.Visible = False
            txtPriceDollar.Visible = False

            If rbPriceAdjustment.SelectedValue = "P" Then
                lblPricePercent.Visible = ViewState("isAdmin")
                lblPricePercentSign.Visible = ViewState("isAdmin")
                lblPricePercentDecimal.Visible = ViewState("isAdmin")
                'lblPricePercentNote.Visible = ViewState("isAdmin")

                txtPricePercent.Visible = ViewState("isAdmin")
            Else
                lblPriceDollar.Visible = ViewState("isAdmin")
                txtPriceDollar.Visible = ViewState("isAdmin")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub EnableControls()

        Try
            Dim ds As DataSet

            DisableAll()

            'not voided and not closed
            If ViewState("EventStatusID") <> 9 And ViewState("EventStatusID") <> 10 Then
                lblSQC.Visible = ViewState("isEdit")
                ddAccountManager.Enabled = ViewState("isAdmin")

                'only sales can change event types 
                If ViewState("EventTypeID") <> 4 Then
                    Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                        Case 9, 23
                            'ddAccountManager.Enabled = ViewState("isAdmin")

                            ddEventType.Enabled = ViewState("isAdmin")

                            'refresh event type list so that only certain types can be selected
                            ds = ARGroupModule.GetAREventTypeList(True)

                            If commonFunctions.CheckDataSet(ds) = True Then
                                ddEventType.Items.Clear()
                                ddEventType.DataSource = ds
                                ddEventType.DataTextField = ds.Tables(0).Columns("ddEventTypeName").ColumnName.ToString()
                                ddEventType.DataValueField = ds.Tables(0).Columns("EventTypeID").ColumnName
                                ddEventType.DataBind()

                                If ViewState("EventTypeID") <= 1 Then
                                    ViewState("EventTypeID") = 1
                                    ddEventType.SelectedValue = 1
                                End If
                            End If
                    End Select
                End If

                'if record exists
                If ViewState("AREID") > 0 Then
                    tblCommunicationBoardExistingQuestion.Visible = ViewState("isEdit")
                    tblCommunicationBoardNewQuestion.Visible = ViewState("isEdit")

                    btnReset.Visible = ViewState("isEdit")
                    btnSave.Visible = ViewState("isEdit")

                    If ViewState("isAdmin") = True Then
                        hlnkApprovalPage.NavigateUrl = "crAR_Event_Approval.aspx?AREID=" & ViewState("AREID")
                        hlnkApprovalPage.Visible = True
                    End If

                    'if not an accouting accrual  
                    If ViewState("EventTypeID") <> 4 Then
                        cbPriceUpdatedByAccounting.Visible = True

                        If ViewState("SubscriptionID") = 21 Then
                            cbPriceUpdatedByAccounting.Enabled = ViewState("isEdit")
                            btnNotifyPriceUpdatedByAccounting.Visible = ViewState("isEdit")
                        End If

                        If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Then
                            cbCustomerApproved.Visible = True

                            Select Case CType(ViewState("SubscriptionID"), Integer) 'accouting, sales, vp of sales 
                                Case 9, 21, 23
                                    txtCustApprvEndDate.Enabled = ViewState("isAdmin")
                                    imgCustApprvEndDate.Visible = ViewState("isAdmin")
                            End Select
                        End If

                        Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                            Case 9, 23
                                btnReset.Visible = ViewState("isAdmin")
                                btnNotifyAccounting.Visible = ViewState("isAdmin")
                                btnSave.Visible = ViewState("isAdmin")
                                'LR ''btnSelectionWizard.Visible = ViewState("isAdmin")
                                btnVoid.Visible = ViewState("isAdmin")

                                gvDetail.Columns(gvDetail.Columns.Count - 1).Visible = ViewState("isAdmin")

                                'part accrual or customer accrual                               
                                Select Case CType(ViewState("EventTypeID"), Integer)
                                    Case 1, 2, 3
                                        If ViewState("EventTypeID") = 0 Then
                                            ViewState("EventTypeID") = 1
                                        Else
                                            ddEventType.Attributes.Add("onchange", "alert('WARNING: By changing the event type, you may lose your selection criteria below for parts or customers.')")
                                        End If

                                        ddEventType.AutoPostBack = True
                                        ddEventType.SelectedValue = ViewState("EventTypeID")
                                End Select

                                'LR ''If ViewState("EventTypeID") = 1 Then
                                'LR ''    rbSelectionWizard.Visible = ViewState("isAdmin")
                                'LR ''End If

                                'Price Change No Accrual OR Part Accrual
                                If ViewState("EventTypeID") = 1 Or ViewState("EventTypeID") = 2 Then
                                    HandlePercentDollarChoice()

                                    If gvDetail.Rows.Count > 0 Then
                                        btnPushAdjustments.Visible = ViewState("isAdmin")
                                    End If

                                End If

                                'Customer Accrual
                                If ViewState("EventTypeID") = 3 Then
                                    rbPriceAdjustment.Visible = False
                                    rbPriceAdjustment.SelectedValue = "P"

                                    lblPricePercentSign.Visible = ViewState("isAdmin")
                                    lblPricePercentDecimal.Visible = ViewState("isAdmin")

                                    lblPricePercent.Visible = ViewState("isAdmin")
                                    txtPricePercent.Visible = ViewState("isAdmin")

                                    lblPriceDollar.Visible = False
                                    txtPriceDollar.Visible = False

                                    If gvDetail.Rows.Count > 0 Then
                                        btnPushAdjustments.Visible = ViewState("isAdmin")
                                    End If

                                End If

                                txtCustApprvEffDate.Enabled = ViewState("isAdmin")
                                imgCustApprvEffDate.Visible = ViewState("isAdmin")
                                lblCustApprvEffDateNote.Visible = ViewState("isAdmin")

                                txtEventDesc.Enabled = ViewState("isAdmin")

                                'Open (Pending Sales Submission) or Rejected (Pending Sales Fix)
                                If (ViewState("EventStatusID") = 1 Or ViewState("EventStatusID") = 7) And (isNewPriceSetForAll() = True Or ViewState("EventTypeID") = 3) Then
                                    btnSubmitApproval.Visible = ViewState("isAdmin")
                                End If

                        End Select

                    End If

                    'accruing 
                    If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 4 Then

                        If ViewState("EventStatusID") = 3 Then 'In-Process (Pending Sales for Customer Approval)
                            Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                                Case 9, 23
                                    btnCustomerApproved.Visible = ViewState("isAdmin")
                            End Select
                        End If

                        'only Accounting with admin rights can submit for approval
                        If ViewState("SubscriptionID") = 21 Then
                            'make sure at least the accountant has approved the original event first
                            If ViewState("EventStatusID") > 2 Then
                                btnCreateAccountingAccrual.Visible = ViewState("isAdmin")
                            End If

                            txtEventDesc.Enabled = ViewState("isAdmin")
                            txtDeductionReason.Enabled = ViewState("isAdmin")
                            txtFinalDeductionAmount.Enabled = ViewState("isAdmin")

                            'only show when status is pending accountant submission or fixing a rejection
                            'or if customer accrual - pending open sales submission (see later)
                            If ViewState("EventStatusID") = 4 Or ViewState("EventStatusID") = 8 Then
                                btnSubmitApproval.Visible = ViewState("isAdmin")
                            End If

                            'customer or accounting accrual can see 
                            If ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 4 Then
                                'LR ''btnSelectionWizard.Visible = ViewState("isAdmin")
                                gvDetail.Columns(gvDetail.Columns.Count - 1).Visible = ViewState("isAdmin")
                            End If

                            ''In-Process (Pending Accountant Close)
                            If ViewState("EventStatusID") = 6 Then
                                btnClose.Visible = ViewState("isEdit")
                            End If

                            imgCreditDebitDate.Visible = ViewState("isEdit")
                            txtCreditDebitDate.Enabled = ViewState("isEdit")
                            txtCreditDebitMemo.Enabled = ViewState("isEdit")

                            'Customer Accrual
                            If ViewState("EventTypeID") = 3 Then
                                rbPriceAdjustment.Visible = False
                                rbPriceAdjustment.SelectedValue = "P"

                                lblPricePercentSign.Visible = ViewState("isAdmin")
                                lblPricePercentDecimal.Visible = ViewState("isAdmin")

                                lblPricePercent.Visible = ViewState("isAdmin")
                                txtPricePercent.Visible = ViewState("isAdmin")

                                lblPriceDollar.Visible = False
                                txtPriceDollar.Visible = False

                                If gvDetail.Rows.Count > 0 Then
                                    btnPushAdjustments.Visible = ViewState("isAdmin")
                                End If

                                gvDetail.Columns(gvDetail.Columns.Count - 1).Visible = False

                                txtCustApprvEffDate.Enabled = ViewState("isAdmin")
                                imgCustApprvEffDate.Visible = ViewState("isAdmin")
                                lblCustApprvEffDateNote.Visible = ViewState("isAdmin")

                                txtEventDesc.Enabled = ViewState("isAdmin")

                                If ViewState("EventStatusID") = 1 Then
                                    btnSubmitApproval.Visible = ViewState("isAdmin")
                                End If

                            End If
                        End If

                    End If 'end for accrual type logic

                    'invoices on hold no accrual
                    If ViewState("EventTypeID") = 5 Then
                        If ViewState("SubscriptionID") = 21 Then
                            txtBPCSInvoiceNo.Enabled = ViewState("isEdit")
                        End If
                    End If


                    'LR ''SetFutureParts()

                Else 'new record
                    'only new AR Events can have these enabled
                    lblMessageAREIDNew.Visible = True
                    'tblPriceAdjustment.Visible = False

                    'accounting can create custom accruals or accounting accruals
                    If ViewState("SubscriptionID") = 21 Then
                        btnSave.Visible = ViewState("isAdmin")

                        imgCustApprvEffDate.Visible = ViewState("isAdmin")
                        txtCustApprvEffDate.Enabled = ViewState("isAdmin")
                        lblCustApprvEffDateNote.Visible = ViewState("isAdmin")
                        txtEventDesc.Enabled = ViewState("isAdmin")

                        'when accounting creates a new event and since this is NOT from a copy, then it must be a customer accrual
                        If ViewState("EventTypeID") = 0 Then
                            ViewState("EventTypeID") = 3
                            ddEventType.SelectedValue = 3
                        End If
                    End If

                    Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                        Case 9, 23
                            btnSave.Visible = ViewState("isAdmin")

                            ddEventType.Enabled = ViewState("isAdmin")

                            If ViewState("SubscriptionID") = 9 Then
                                If ddAccountManager.Items.FindByValue(ViewState("TeamMemberID")) IsNot Nothing Then
                                    ddAccountManager.SelectedValue = ViewState("TeamMemberID")
                                End If
                            End If

                            imgCustApprvEffDate.Visible = ViewState("isAdmin")

                            txtCustApprvEffDate.Enabled = ViewState("isAdmin")
                            lblCustApprvEffDateNote.Visible = ViewState("isAdmin")

                            txtEventDesc.Enabled = ViewState("isAdmin")
                            'txtNotes.Enabled = ViewState("isAdmin")
                    End Select

                End If 'new record
            End If  'not voided and not closed

            'certain pieces can be shown regardless of any event status
            If ViewState("AREID") > 0 Then
                gvApproval.Visible = True
                gvDetail.Visible = True

                'if there are details selected and NOT an accounting accrual
                btnExportToExcel.Visible = False
                If ViewState("EventTypeID") <> 4 Then
                    If gvDetail.Rows.Count > 0 Then
                        btnExportToExcel.Visible = True
                    End If
                End If

                gvSupportingDoc.Visible = True

                ddEventStatus.Visible = True

                lblAREID.Visible = True
                lblEventStatus.Visible = True

                menuTabs.Visible = True

                If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 4 Then 'accruing

                    lblQuantityShippedLabel.Visible = True
                    lblQuantityShippedValue.Visible = True

                    'lblCalculatedDeductionAmountLabel.Visible = True
                    'lblCalculatedDeductionAmountValue.Visible = True

                    lblCustApprvEndDate.Visible = True

                    lblFinalDeductionAmount.Visible = True

                    If cbCustomerApproved.Checked = True Or ViewState("EventTypeID") = 4 Then
                        'lblDeductionInstr.Visible = True
                        lblDeductionReason.Visible = True

                        'txtDeductionInstr.Visible = True
                        txtDeductionReason.Visible = True
                    End If

                    If ViewState("EventTypeID") = 4 Then 'Accounting Accrual
                        tblPriceAdjustment.Visible = False
                        'gvDetail.Visible = False
                        'menuTabs.Items(0).Enabled = False
                        gvDetail.Columns(4).Visible = False 'PartNo
                        gvDetail.Columns(8).Visible = False 'Current Price
                        gvDetail.Columns(9).Visible = False 'PRCPRNT
                        gvDetail.Columns(10).Visible = False 'New Price

                    End If

                    txtCustApprvEndDate.Visible = True
                    txtFinalDeductionAmount.Visible = True

                    gvDetail.Columns(11).Visible = False 'ESTPRC

                    'customer accrual
                    If ViewState("EventTypeID") = 3 Then
                        gvDetail.Columns(8).Visible = False 'Current Price
                        gvDetail.Columns(10).Visible = False 'New Price                       
                    End If

                Else
                    'price change no accrual
                    If ViewState("EventTypeID") = 1 Then
                        'LR ''gvDetail.Columns(3).Visible = False 'ugn facility
                        'LR ''gvDetail.Columns(7).Visible = False 'soldto 
                        'LR ''gvDetail.Columns(8).Visible = False 'cabbv
                        'LR ''gvDetail.Columns(7).Visible = False 'customer
                        gvDetail.Columns(11).Visible = False 'ESTPRC 
                    End If

                    'invoices on hold no accrual
                    If ViewState("EventTypeID") = 5 Then
                        tblPriceAdjustment.Visible = False

                        'LR ''gvDetail.Columns(3).Visible = False 'ugn facility                        
                        'LR ''gvDetail.Columns(7).Visible = False 'soldto 
                        'LR ''gvDetail.Columns(8).Visible = False 'cabbv
                        'LR ''gvDetail.Columns(7).Visible = False 'customer
                        gvDetail.Columns(8).Visible = False 'Current Price 
                        gvDetail.Columns(9).Visible = False 'PRCPRNT 

                        lblAffectedInvoicesOnHoldLabel.Visible = True
                        lblCurrentInvoicesOnHoldLabel.Visible = True

                        gvAffectedInvoicesOnHold.Visible = True
                        'gvInvoicesOnHold.Visible = True

                        If txtInvoicePartNo.Text.Trim <> "" Then
                            gvInvoicesOnHold.Visible = True
                        End If

                        lblBPCSInvoiceNo.Visible = True
                        txtBPCSInvoiceNo.Visible = True

                    End If
                End If

                'not voided can preview
                If ViewState("EventStatusID") <> 10 Then

                    If lblCloseDateValue.Text <> "" Then
                        lblCloseDateLabel.Visible = True
                        lblCloseDateValue.Visible = True
                    End If

                    tblUpload.Visible = ViewState("isEdit")
                    gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = ViewState("isEdit")

                    'anyone can see preview button
                    btnPreview.Visible = True

                    If lblPriceChangeDate.Text.Trim <> "" Then
                        cbPriceUpdatedByAccounting.Visible = True
                        lblPriceChangeDate.Visible = True
                    End If

                    Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                        Case 9, 23
                            'price change no accrual, part accrual or customer accrual                               
                            Select Case CType(ViewState("EventTypeID"), Integer)
                                Case 1, 2, 3
                                    btnCopy.Visible = ViewState("isAdmin")
                            End Select
                    End Select

                    'accruing
                    If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 4 Then

                        'only show accrual if not void and not submitted (not open)
                        If (ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3) And ViewState("EventStatusID") <> 1 Then
                            btnAccrual.Visible = True
                        End If
                        lblCreditDebitDate.Visible = True
                        txtCreditDebitDate.Visible = True

                        lblCreditDebitMemo.Visible = True
                        txtCreditDebitMemo.Visible = True
                    End If

                    If ViewState("CurrentRSSID") > 0 Then
                        btnResetReplyComment.Visible = ViewState("isAdmin")
                        btnSaveReplyComment.Visible = ViewState("isAdmin")
                    End If

                    Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                        Case 9, 23

                            'if 9 - Closed
                            If ViewState("EventStatusID") = 9 And (txtInvoicePriceCode.Text.Trim = "A" Or txtInvoicePriceCode.Text.Trim = "S") Then
                                btnCreatePriceChangeNoAccrual.Visible = ViewState("isAdmin")
                            End If
                    End Select

                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub SetFutureParts()

        Try

            'LR ''If rbSelectionWizard.SelectedValue = "F" Or ViewState("EventTypeID") = 4 Or ViewState("EventTypeID") = 5 Then
            'LR ''    tblPriceAdjustment.Visible = False
            'LR ''Else
            'if sales, vp of sales then enable OR if Accounting AND Customer Accrual then enable
            If ViewState("SubscriptionID") = 9 Or ViewState("SubscriptionID") = 23 Or (ViewState("SubscriptionID") = 21 And ViewState("EventTypeID") = 3) Then
                tblPriceAdjustment.Visible = ViewState("isAdmin")
            End If
            'LR ''End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub HanldeBreadCrumbs()

        Try

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > <a href='AR_Event_List.aspx'>AR Event Search </a> > AR Event Detail > <a href='AR_Event_History.aspx?AREID=" & ViewState("AREID") & "'> Event History </a> "

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvQuestionAppendReply_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ClearMessages()

        Try

            Dim iBtnAppendReply As ImageButton
            Dim iRSSID As Integer = 0

            iBtnAppendReply = CType(sender, ImageButton)

            If iBtnAppendReply.CommandName.ToString <> "" Then
                iRSSID = CType(iBtnAppendReply.CommandName, Integer)

                If iRSSID > 0 Then
                    ViewState("CurrentRSSID") = iRSSID
                    txtQuestionComment.Text = iBtnAppendReply.AlternateText

                    btnSaveReplyComment.Visible = ViewState("isEdit")
                    btnResetReplyComment.Visible = ViewState("isEdit")

                End If
            End If
            'lblMessage.Text = "Question " & iBtnAppendReply.ToolTip & " was selected."


        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub menuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuTabs.MenuItemClick

        Try

            ClearMessages()

            mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "AR Event Detail"

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("ARExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub HandleMultiLineTextBoxes()

        Try

            'txtDeductionInstr.Attributes.Add("onkeypress", "return tbLimit();")
            'txtDeductionInstr.Attributes.Add("onkeyup", "return tbCount(" + lblDeductionInstrCharCount.ClientID + ");")
            'txtDeductionInstr.Attributes.Add("maxLength", "1000")

            txtDeductionReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtDeductionReason.Attributes.Add("onkeyup", "return tbCount(" + lblDeductionReasonCharCount.ClientID + ");")
            txtDeductionReason.Attributes.Add("maxLength", "1000")

            txtEventDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtEventDesc.Attributes.Add("onkeyup", "return tbCount(" + lblEventDescCharCount.ClientID + ");")
            txtEventDesc.Attributes.Add("maxLength", "1000")

            txtSupportingDocDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtSupportingDocDesc.Attributes.Add("onkeyup", "return tbCount(" + lblSupportingDocDescCharCount.ClientID + ");")
            txtSupportingDocDesc.Attributes.Add("maxLength", "200")

            'txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            'txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotesCharCount.ClientID + ");")
            'txtNotes.Attributes.Add("maxLength", "2000")

            txtReply.Attributes.Add("onkeypress", "return tbLimit();")
            txtReply.Attributes.Add("onkeyup", "return tbCount(" + lblReplyCharCount.ClientID + ");")
            txtReply.Attributes.Add("maxLength", "2000")

            txtVoidReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidReason.Attributes.Add("onkeyup", "return tbCount(" + lblVoidReasonCharCount.ClientID + ");")
            txtVoidReason.Attributes.Add("maxLength", "200")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub HandleButtonAttributes()

        Try

            Dim redirstr As String = "javascript:void(window.open('crPreview_AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "'," & Now.Ticks.ToString & ",'top=10,height=600,width=950,resizable=yes,status=yes,toolbar=yes,scrollbars=yes,menubar=yes,location=no'));"
            btnPreview.Attributes.Add("onclick", redirstr)

            btnCopy.Attributes.Add("onclick", "if(confirm('Are you sure that you want to copy this event?')){}else{return false}")
            btnCreateAccountingAccrual.Attributes.Add("onclick", "if(confirm('Are you sure that you want to copy this event?')){}else{return false}")
            btnVoid.Attributes.Add("onclick", "if(confirm('Are you sure that you want to void? If so, click ok to see and update the Void Reason field. Then click void again. ')){}else{return false}")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                InitializeViewState()

                ''Used to allow TM(s) to Communicated with Approvers for Q&A
                If HttpContext.Current.Request.QueryString("pRC") <> "" Then
                    ViewState("pRC") = HttpContext.Current.Request.QueryString("pRC")
                Else
                    ViewState("pRC") = 0
                End If

                CheckRights()

                'clear crystal reports
                ARGroupModule.CleanARCrystalReports()

                BindCriteria()

                ViewState("AREID") = 0

                If HttpContext.Current.Request.QueryString("AREID") <> "" Then

                    ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")
                    lblAREID.Text = ViewState("AREID")

                    BindData()

                    'btnVoid.Attributes.Add("onclick", "if(confirm('Are you sure that you want to void this AR Event? If so, click ok. ')){}else{return false}")
                End If

                EnableControls()

                HandleMultiLineTextBoxes()

                HandleButtonAttributes()

                HanldeBreadCrumbs()

                If Session("RecordCopied") = "1" Then
                    lblMessage.Text = "<br />Record Copied Successfully"
                    Session("RecordCopied") = Nothing
                End If

                If ViewState("pRC") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(3)
                    mvTabs.GetActiveView()
                    menuTabs.Items(menuTabs.Items.Count - 1).Selected = True
                Else
                    If ViewState("AREID") > 0 Then
                        mvTabs.ActiveViewIndex = Int32.Parse(0)
                        mvTabs.GetActiveView()
                        menuTabs.Items(0).Selected = True
                        ''accounting accrual should select different tab
                        'If ViewState("EventTypeID") = 4 Then
                        '    mvTabs.ActiveViewIndex = Int32.Parse(2)
                        '    mvTabs.GetActiveView()
                        '    menuTabs.Items(2).Selected = True
                        'Else
                        '    mvTabs.ActiveViewIndex = Int32.Parse(0)
                        '    mvTabs.GetActiveView()
                        '    menuTabs.Items(0).Selected = True
                        'End If
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub btnSelectionWizard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectionWizard.Click

    '    Try
    '        ClearMessages()

    '        Select Case CType(ViewState("EventTypeID"), Integer)
    '            Case 1 'Price Change (No Accrual)
    '                If rbSelectionWizard.SelectedValue = "C" Then
    '                    Response.Redirect("AR_Price_Change_No_Accrual_Wizard_Current.aspx?AREID=" & ViewState("AREID"), False)
    '                Else
    '                    Response.Redirect("AR_Price_Change_No_Accrual_Wizard_Future.aspx?AREID=" & ViewState("AREID"), False)
    '                End If

    '            Case 2 'Part Acrual
    '                Response.Redirect("AR_Part_Accrual_Wizard.aspx?AREID=" & ViewState("AREID"), False)
    '            Case 3 'Customer Acrual
    '                Response.Redirect("AR_Customer_Accrual_Wizard.aspx?AREID=" & ViewState("AREID"), False)
    '            Case 4 'Accounting Acrual
    '                Response.Redirect("AR_Accounting_Accrual_Wizard.aspx?AREID=" & ViewState("AREID"), False)
    '            Case 5 'Invoice On Hold (No Accrual)
    '                Response.Redirect("AR_Invoices_On_Hold_Wizard.aspx?AREID=" & ViewState("AREID"), False)
    '        End Select

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    lblMessageButtons.Text = lblMessage.Text
    'End Sub

    Protected Sub btnAccrual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccrual.Click

        Try
            ClearMessages()

            Response.Redirect("AR_Event_Accrual.aspx?AREID=" & ViewState("AREID") & "&EventTypeID=" & ViewState("EventTypeID"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text

    End Sub

    Protected Sub gvSupportingDoc_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSupportingDoc.DataBound

        Try

            If gvSupportingDoc.HeaderRow IsNot Nothing Then
                gvSupportingDoc.HeaderRow.Cells(0).Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text &= "<br />" & lblMessage.Text

    End Sub

    Protected Sub gvApproval_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvApproval.DataBound

        Try
            If gvApproval.HeaderRow IsNot Nothing Then
                gvApproval.HeaderRow.Cells(0).Visible = False
                gvApproval.HeaderRow.Cells(1).Visible = False
                gvApproval.HeaderRow.Cells(2).Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text &= "<br />" & lblMessage.Text

    End Sub

    Private Function GetTeamMemberIDBySubscriptionID(ByVal SubscriptionID As Integer, ByVal UGNFacility As String) As Integer

        Dim dsSubscription As DataSet
        Dim bWorking As Boolean = False
        Dim iTMID As Integer = 0

        Try
            dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(SubscriptionID, UGNFacility)

            If commonFunctions.CheckDataSet(dsSubscription) = True Then
                If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If dsSubscription.Tables(0).Rows(0).Item("WorkStatus") IsNot System.DBNull.Value Then
                        bWorking = dsSubscription.Tables(0).Rows(0).Item("WorkStatus")

                        'only get working team members
                        If bWorking = True Then
                            If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 Then
                                iTMID = dsSubscription.Tables(0).Rows(0).Item("TMID")
                            End If
                        End If
                    End If
                End If
            End If

            If iTMID = 0 Then 'notify application group if subscription has not been assigned to working team member
                UGNErrorTrapping.UpdateUGNErrorLog("AR Module: Failed getting team member ID to subscriptionID: " & SubscriptionID & ", UGNFacility: " & UGNFacility, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return iTMID

    End Function

    Private Sub InsertAccrualApprovalRoutingLevelCFO()

        Try

            'add vp of finance if not already exists  
            If ViewState("CFOTMID") > 0 And ViewState("CFOApprovalRowID") = 0 Then
                ARGroupModule.InsertAREventApproval(ViewState("AREID"), 4, ViewState("CFOTMID"), 33, 1)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            Dim strErrorMessage As String = "<br />" & ex.Message _
            & ", CFOTMID = " & ViewState("CFOTMID") _
            & "<br />" & mb.Name

            lblMessage.Text = strErrorMessage
            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & strErrorMessage, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub InsertAccrualApprovalRoutingLevelMiddle()

        Dim dCalculatedDeductionAmount As Double = 0
        Dim dFinalDeductionAmount As Double = 0

        Try

            If lblCalculatedDeductionAmountValue.Text.Trim <> "" Then
                dCalculatedDeductionAmount = CType(lblCalculatedDeductionAmountValue.Text.Trim, Double)
            End If

            If txtFinalDeductionAmount.Text.Trim <> "" Then
                dFinalDeductionAmount = CType(txtFinalDeductionAmount.Text.Trim, Double)
            End If

            'if needed, add VP of Sales and VP of Finance
            ' ''If dCalculatedDeductionAmount <= -2500 _
            ' ''    Or dCalculatedDeductionAmount >= 2500 _
            ' ''    Or dFinalDeductionAmount <= -2500 _
            ' ''    Or dFinalDeductionAmount >= 2500 Then

            If dFinalDeductionAmount <= -2500 Or dFinalDeductionAmount >= 2500 Then
                'add vp of sales if not already exists      
                If ViewState("VPSalesTMID") > 0 And ViewState("VPSalesApprovalRowID") = 0 Then
                    ARGroupModule.InsertAREventApproval(ViewState("AREID"), 3, ViewState("VPSalesTMID"), 23, 1)
                End If

                InsertAccrualApprovalRoutingLevelCFO()

            Else 'remove if exists
                ARGroupModule.DeleteAREventApprovalStatus(ViewState("AREID"), 23)
                ARGroupModule.DeleteAREventApprovalStatus(ViewState("AREID"), 33)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            Dim strErrorMessage As String = ""

            'update error on web page
            strErrorMessage &= "<br />" & ex.Message _
            & ", dCalculatedDeductionAmount = " & dCalculatedDeductionAmount _
            & ", dFinalDeductionAmount = " & dFinalDeductionAmount _
            & ", VPSalesTMID = " & ViewState("VPSalesTMID") _
            & ", CFOTMID = " & ViewState("CFOTMID") _
            & "<br />" & mb.Name

            lblMessage.Text = strErrorMessage
            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & strErrorMessage, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InsertAccrualApprovalRoutingLevelLast()

        Dim dCalculatedDeductionAmount As Double = 0
        Dim dFinalDeductionAmount As Double = 0

        Try

            If lblCalculatedDeductionAmountValue.Text.Trim <> "" Then
                dCalculatedDeductionAmount = CType(lblCalculatedDeductionAmountValue.Text.Trim, Double)
            End If

            If txtFinalDeductionAmount.Text.Trim <> "" Then
                dFinalDeductionAmount = CType(txtFinalDeductionAmount.Text.Trim, Double)
            End If

            'if needed, add CEO
            ' ''If dCalculatedDeductionAmount <= -5000 _
            ' ''    Or dCalculatedDeductionAmount >= 5000 _
            ' ''    Or dFinalDeductionAmount <= -5000 _
            ' ''    Or dFinalDeductionAmount >= 5000 Then

            If dFinalDeductionAmount <= -5000   Or dFinalDeductionAmount >= 5000 Then
                'if approver not already exists 
                If ViewState("CEOTMID") > 0 And ViewState("CEOApprovalRowID") = 0 Then
                    ARGroupModule.InsertAREventApproval(ViewState("AREID"), 5, ViewState("CEOTMID"), 24, 1)
                End If

            Else 'remove if exists
                ARGroupModule.DeleteAREventApprovalStatus(ViewState("AREID"), 24)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            Dim strErrorMessage As String = ""

            'update error on web page
            strErrorMessage &= "<br />" & ex.Message _
            & ", dCalculatedDeductionAmount = " & dCalculatedDeductionAmount _
            & ", dFinalDeductionAmount = " & dFinalDeductionAmount _
            & ", CEOTMID = " & ViewState("CEOTMID") _
            & "<br />" & mb.Name

            lblMessage.Text = strErrorMessage
            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & strErrorMessage, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InsertAccrualApprovalRoutingAcountingManager()

        Try

            'save just billing subscription ID because backup could eventually approve
            If ViewState("DefaultBillingTMID") > 0 And ViewState("BillingApprovalRowID") = 0 Then
                ARGroupModule.InsertAREventApproval(ViewState("AREID"), 1, ViewState("DefaultBillingTMID"), 21, 1)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            Dim strErrorMessage As String = "<br />" & ex.Message _
            & ", DefaultBillingTMID = " & ViewState("DefaultBillingTMID") _
            & ", BillingApprovalStatusID = " & ViewState("BillingApprovalStatusID") _
            & "<br />" & mb.Name

            lblMessage.Text = strErrorMessage
            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & strErrorMessage, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InsertAccrualApprovalRoutingSales()

        Try

            'add sales if not already exists
            If ViewState("AcctMgrTMID") > 0 And ViewState("SalesApprovalRowID") = 0 Then
                ARGroupModule.InsertAREventApproval(ViewState("AREID"), 2, ViewState("AcctMgrTMID"), 9, 1)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            Dim strErrorMessage As String = "<br />" & ex.Message _
            & ", SalesApprovalStatusID = " & ViewState("SalesApprovalStatusID") _
            & ", AcctMgrTMID = " & ViewState("AcctMgrTMID") _
            & "<br />" & mb.Name

            lblMessage.Text = strErrorMessage
            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & strErrorMessage, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Private Sub InsertUpdateAccrualApprovalRouting()

    '    Dim dCalculatedDeductionAmount As Double = 0
    '    Dim dFinalDeductionAmount As Double = 0

    '    Try

    '        If ViewState("AREID") > 0 Then
    '            'save just billing subscription ID because backup could eventually approve
    '            If ViewState("DefaultBillingTMID") > 0 And ViewState("BillingApprovalStatusID") <= 1 Then
    '                ARGroupModule.InsertAREventApproval(ViewState("AREID"), 1, ViewState("DefaultBillingTMID"), 21, 1)
    '                ViewState("BillingApprovalStatusID") = 1
    '            End If

    '            'add/refresh sales unless approved
    '            If ViewState("AcctMgrTMID") > 0 And ViewState("SalesApprovalStatusID") <> 4 Then
    '                'ARGroupModule.InsertAREventApproval(ViewState("AREID"), 2, ViewState("AcctMgrTMID"), 9, 1)

    '                '2012-Sep-04
    '                'Auto-Approve Sales
    '                ARGroupModule.InsertAREventApproval(ViewState("AREID"), 2, ViewState("AcctMgrTMID"), 9, 4)
    '                ViewState("SalesApprovalStatusID") = 4
    '            End If

    '            'If lblCalculatedDeductionAmountValue.Text.Trim <> "" Then
    '            '    dCalculatedDeductionAmount = CType(lblCalculatedDeductionAmountValue.Text.Trim, Double)
    '            'End If

    '            'If txtFinalDeductionAmount.Text.Trim <> "" Then
    '            '    dFinalDeductionAmount = CType(txtFinalDeductionAmount.Text.Trim, Double)
    '            'End If

    '            ''if needed, add VP of Sales and VP of Finance
    '            'If dCalculatedDeductionAmount <= -2500 _
    '            '    Or dCalculatedDeductionAmount >= 2500 _
    '            '    Or dFinalDeductionAmount <= -2500 _
    '            '    Or dFinalDeductionAmount >= 2500 Then

    '            '    'add vp of sales       
    '            '    If ViewState("VPSalesTMID") > 0 Then
    '            '        If ViewState("BillingApprovalStatusID") <> 4 Then
    '            '            'if accounting manager has not approved then set VP of Sales to Open
    '            '            ARGroupModule.InsertAREventApproval(ViewState("AREID"), 3, ViewState("VPSalesTMID"), 23, 1)
    '            '        Else
    '            '            'if accounting manager approved and submitted then set VP of Sales to In-Process for approval
    '            '            ARGroupModule.InsertAREventApproval(ViewState("AREID"), 3, ViewState("VPSalesTMID"), 23, 2)

    '            '            'update event status
    '            '            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 5) 'In-Process (Pending Deduction Form Approval)
    '            '            ViewState("EventStatusID") = 5
    '            '            ddEventStatus.SelectedValue = 5
    '            '        End If
    '            '    End If

    '            '    'add vp of finance   
    '            '    If ViewState("CFOTMID") > 0 Then
    '            '        ARGroupModule.InsertAREventApproval(ViewState("AREID"), 4, ViewState("CFOTMID"), 33, 1)
    '            '    End If

    '            'Else 'remove if exists
    '            '    ARGroupModule.DeleteAREventApprovalStatus(ViewState("AREID"), 23)
    '            '    ARGroupModule.DeleteAREventApprovalStatus(ViewState("AREID"), 33)

    '            '    'if VP of Sales is NOT needed then Billing can close
    '            '    ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 6)  'In-Process (Pending Accountant Close)
    '            '    ViewState("EventStatusID") = 6
    '            '    ddEventStatus.SelectedValue = 6
    '            'End If

    '            ''if needed, add CEO
    '            'If dCalculatedDeductionAmount <= -5000 _
    '            '    Or dCalculatedDeductionAmount >= 5000 _
    '            '    Or dFinalDeductionAmount <= -5000 _
    '            '    Or dFinalDeductionAmount >= 5000 Then

    '            '    If ViewState("CEOTMID") > 0 Then
    '            '        ARGroupModule.InsertAREventApproval(ViewState("AREID"), 5, ViewState("CEOTMID"), 24, 1)
    '            '    End If

    '            'Else 'remove if exists
    '            '    ARGroupModule.DeleteAREventApprovalStatus(ViewState("AREID"), 24)
    '            'End If
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        Dim strErrorMessage As String = ""

    '        'update error on web page
    '        strErrorMessage &= "<br />" & ex.Message _
    '        & ", DefaultBillingTMID = " & ViewState("DefaultBillingTMID") _
    '        & ", BillingApprovalStatusID = " & ViewState("BillingApprovalStatusID") _
    '        & ", SalesApprovalStatusID = " & ViewState("SalesApprovalStatusID") _
    '        & ", AcctMgrTMID = " & ViewState("AcctMgrTMID") _
    '        & ", dCalculatedDeductionAmount = " & dCalculatedDeductionAmount _
    '        & ", dFinalDeductionAmount = " & dFinalDeductionAmount _
    '        & ", VPSalesTMID = " & ViewState("VPSalesTMID") _
    '        & ", CFOTMID = " & ViewState("CFOTMID") _
    '        & ", CEOTMID = " & ViewState("CEOTMID") _
    '        & "<br />" & mb.Name

    '        lblMessage.Text = strErrorMessage
    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & strErrorMessage, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            ClearMessages()

            GetTeamMemberInfo()

            Dim ds As DataSet
            'Dim dt As DataTable
            'Dim objAREventApproval As New ARApprovalBLL

            Dim dCalculatedQuantityShipped As Integer = 0
            Dim dCalculatedDeductionAmount As Double = 0
            Dim dFinalDeductionAmount As Double = 0

            'Dim iApprovalStatus As Integer = 0
            'Dim iApprovalRowID As Integer = 0

            'if the event type has not been saved yet, then get its value
            If ViewState("EventTypeID") = 0 Or ViewState("AREID") = 0 Then
                If ddEventType.SelectedIndex >= 0 Then
                    ViewState("EventTypeID") = ddEventType.SelectedValue
                Else
                    ViewState("EventTypeID") = 1
                End If
            End If

            If ddAccountManager.SelectedIndex >= 0 Then
                ViewState("AcctMgrTMID") = ddAccountManager.SelectedValue
            End If

            If lblQuantityShippedValue.Text.Trim <> "" Then
                dCalculatedQuantityShipped = CType(lblQuantityShippedValue.Text.Trim, Integer)
            End If

            If lblCalculatedDeductionAmountValue.Text.Trim <> "" Then
                dCalculatedDeductionAmount = CType(lblCalculatedDeductionAmountValue.Text.Trim, Double)
            End If

            If txtFinalDeductionAmount.Text.Trim.Trim <> "" Then
                dFinalDeductionAmount = CType(txtFinalDeductionAmount.Text.Trim, Double)
            End If

            'do not allow end date to be earlier than start date
            'switch dashes to slashes
            txtCustApprvEffDate.Text = txtCustApprvEffDate.Text.Trim.Replace("-", "/")
            txtCustApprvEndDate.Text = txtCustApprvEndDate.Text.Trim.Replace("-", "/")

            Dim tempDate As String = ""
            If txtCustApprvEndDate.Text.Trim = "" Then
                tempDate = Today.Date
            Else
                tempDate = txtCustApprvEndDate.Text
            End If

            'wipe out end date if less than effective date
            If txtCustApprvEndDate.Text.Trim <> "" Then
                If CType(txtCustApprvEndDate.Text.Trim, Date) < CType(txtCustApprvEffDate.Text.Trim, Date) Then
                    txtCustApprvEndDate.Text = ""
                End If
            End If

            If (CType(txtCustApprvEffDate.Text.Trim, Date) <= CType(tempDate, Date)) Or txtCustApprvEndDate.Text.Trim = "" Then

                If ViewState("EventStatusID") <> 10 And ViewState("EventStatusID") <> 9 And ViewState("isEdit") = True Then
                    'update existing record
                    If ViewState("AREID") > 0 Then
                        If ViewState("SubscriptionID") = 21 Then 'billing/accounting

                            If cbPriceUpdatedByAccounting.Checked = True And lblPriceChangeDate.Text.Trim = "" Then
                                lblPriceChangeDate.Text = Today.Date
                            End If

                            ARGroupModule.UpdateAREventBilling(ViewState("AREID"), txtEventDesc.Text.Trim, ViewState("AcctMgrTMID"), txtCustApprvEndDate.Text.Trim, dCalculatedQuantityShipped, dCalculatedDeductionAmount, dFinalDeductionAmount, txtDeductionReason.Text.Trim, cbPriceUpdatedByAccounting.Checked, lblPriceChangeDate.Text.Trim, txtCreditDebitMemo.Text.Trim, txtCreditDebitDate.Text.Trim, txtBPCSInvoiceNo.Text.Trim)

                            If ViewState("EventTypeID") = 3 And ViewState("isAdmin") = True Then
                                ARGroupModule.UpdateAREventSales(ViewState("AREID"), txtEventDesc.Text.Trim, ViewState("EventTypeID"), ViewState("AcctMgrTMID"), txtCustApprvEffDate.Text.Trim, txtCustApprvEndDate.Text.Trim)
                            End If
                        End If

                        Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                            Case 9, 23
                                ARGroupModule.UpdateAREventSales(ViewState("AREID"), txtEventDesc.Text.Trim, ViewState("EventTypeID"), ViewState("AcctMgrTMID"), txtCustApprvEffDate.Text.Trim, txtCustApprvEndDate.Text.Trim)
                                lblMessage.Text &= "<br />Record Updated Successfully"

                        End Select

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Updated Event")

                    Else 'insert new record

                        If ViewState("EventTypeID") = 1 Or ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 5 Then
                            Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                                Case 9, 23

                                    ds = ARGroupModule.InsertAREvent(0, ViewState("EventTypeID"), 1, txtEventDesc.Text.Trim, ViewState("AcctMgrTMID"), txtCustApprvEffDate.Text.Trim, txtCustApprvEndDate.Text.Trim)

                                    If commonFunctions.CheckDataSet(ds) = True Then
                                        lblAREID.Text = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                                        ViewState("AREID") = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                                        ViewState("EventStatusID") = 1
                                        ddEventStatus.SelectedValue = ViewState("EventStatusID")
                                        lblMessage.Text &= "<br />Record Created Successfully"

                                        'update history
                                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Created Event")
                                    End If
                            End Select

                        End If

                        'customer or accounting accrual by Accounting Manager
                        If ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 4 Then
                            If ViewState("SubscriptionID") = 21 Then

                                'if customer accrual, then set status to Open (Pending Sales Submission)
                                If ViewState("EventTypeID") = 3 Then
                                    ViewState("EventStatusID") = 1
                                End If

                                'if accounting accrual, then set to In-Process (Pending Accounting Mgr Submission for Approval)
                                If ViewState("EventTypeID") = 4 Then
                                    ViewState("EventStatusID") = 4
                                End If

                                'set status immediately to pending accounting manager submission
                                ds = ARGroupModule.InsertAREvent(0, ViewState("EventTypeID"), ViewState("EventStatusID"), txtEventDesc.Text.Trim, ViewState("AcctMgrTMID"), txtCustApprvEffDate.Text.Trim, txtCustApprvEndDate.Text.Trim)

                                If commonFunctions.CheckDataSet(ds) = True Then
                                    lblAREID.Text = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                                    ViewState("AREID") = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                                    ViewState("EventStatusID") = 4
                                    ddEventStatus.SelectedValue = 4
                                    lblMessage.Text &= "<br />Record Created Successfully"

                                    'update history
                                    ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Created Event")
                                End If
                            End If
                        End If

                        'for new records, hightlight tab
                        If ViewState("AREID") > 0 Then
                            'accounting accrual should select different tab
                            If ViewState("EventTypeID") = 4 Then
                                mvTabs.ActiveViewIndex = Int32.Parse(1)
                                mvTabs.GetActiveView()
                                menuTabs.Items(1).Selected = True
                            Else
                                mvTabs.ActiveViewIndex = Int32.Parse(0)
                                mvTabs.GetActiveView()
                                menuTabs.Items(0).Selected = True
                            End If
                        End If
                    End If

                    ''update Approval Status Tab with Subscriptions and Routing Levels
                    ''routing may add more levels if accrual amount changes

                    'price change no accrual or invoice on hold no accrual
                    If ViewState("EventTypeID") = 1 Or ViewState("EventTypeID") = 5 Then
                        'add default billing
                        'iDefaultBillingTMID = ViewState("DefaultBillingTMID") 'GetTeamMemberIDBySubscriptionID(79, "")

                        'save just billing subscription ID because backup could eventually approve
                        InsertAccrualApprovalRoutingAcountingManager()
                      
                    End If

                    'accrual by part or customer
                    If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Then
                        'HandleSalesAccrualSubmission()
                        Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                            Case 9, 23
                                'update accrual details
                                ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                                RecalculateTotals()

                        End Select

                        If ViewState("EventTypeID") = 3 Then
                            If ViewState("SubscriptionID") = 21 And ViewState("isAdmin") = True Then
                                'update accrual details
                                ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                                RecalculateTotals()

                            End If
                        End If

                        If ViewState("isAdmin") = True Then
                            Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, billing admin, vp of sales 
                                Case 9, 21, 23
                                    'InsertUpdateAccrualApprovalRouting()
                                    InsertAccrualApprovalRoutingSales()
                                    InsertAccrualApprovalRoutingLevelMiddle()
                                    InsertAccrualApprovalRoutingLevelLast()
                            End Select
                        End If

                    End If

                    'Acounting Accrual Type
                    If ViewState("EventTypeID") = 4 Then
                        'add vp of finance
                        If ViewState("CFOApprovalStatusID") = 4 Then
                            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 4) 'In-Process (Pending Accounting Mgr Submission for Approval)
                            ViewState("EventStatusID") = 4
                            ddEventStatus.SelectedValue = 4
                        Else
                            InsertAccrualApprovalRoutingLevelCFO()
                        End If

                        If ViewState("SubscriptionID") = 21 And ViewState("isAdmin") = True Then
                            ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))
                        End If

                    End If

                    gvApproval.DataBind()
                    BindData()
                    EnableControls()
                End If
            Else
                lblMessage.Text = "<br />Error: Please make sure the effective date is less than the end date if it exists. The event could not be saved."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
    End Sub

    Protected Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click

        Dim bEmailSuccess As Boolean = False

        Try
            ClearMessages()

            Dim bOpenStatus As Boolean = False

            lblVoidReason.Visible = True
            txtVoidReason.Visible = True
            txtVoidReason.Enabled = True

            rfvVoidReason.Enabled = True

            btnVoid.Attributes.Add("onclick", "")
            btnVoid.CausesValidation = True

            btnAccrual.Visible = False
            btnCopy.Visible = False
            btnCustomerApproved.Visible = False
            btnPreview.Visible = False
            btnPushAdjustments.Visible = False
            btnReset.Visible = False
            btnSave.Visible = False
            'LR ''btnSelectionWizard.Visible = False
            btnUpload.Visible = False

            txtEventDesc.Enabled = False
            txtCustApprvEffDate.Enabled = False
            txtCustApprvEndDate.Enabled = False
            lblCustApprvEffDateNote.Visible = False
            'txtNotes.Enabled = False

            menuTabs.Enabled = False

            If txtVoidReason.Text.Trim <> "" Then
                ARGroupModule.DeleteAREvent(ViewState("AREID"), txtVoidReason.Text.Trim)

                'append to history
                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Voided")

                'update status to voided  
                ViewState("StatusID") = 10
                ddEventStatus.SelectedValue = 10

                'EnableControls()
                DisableAll()

            Else
                lblMessage.Text &= "<br />To void an AR Event, please fill in the Void Reason field."
                txtVoidReason.Focus()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        Try

            ClearMessages()

            Session("RecordCopied") = Nothing

            Dim ds As DataSet

            Dim iPreviousAREID As Integer = 0

            Dim iSalesID As Integer = 0
            Dim iEventTypeID As Integer = 0
            Dim dCalculatedDeductionAmount As Double = 0

            If ddAccountManager.SelectedIndex >= 0 Then
                iSalesID = ddAccountManager.SelectedValue
            End If

            If ddEventType.SelectedIndex >= 0 Then
                iEventTypeID = ddEventType.SelectedValue
            End If

            If lblCalculatedDeductionAmountValue.Text <> "" Then
                dCalculatedDeductionAmount = CType(lblCalculatedDeductionAmountValue.Text, Double)
            End If

            iPreviousAREID = ViewState("AREID")

            Select Case iEventTypeID
                Case 1, 2, 3
                    Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                        Case 9, 23
                            If iEventTypeID = 1 Then
                                txtCustApprvEndDate.Text = ""
                            End If

                            'wipe out end date if less than effective date
                            If txtCustApprvEndDate.Text.Trim <> "" Then
                                If CType(txtCustApprvEndDate.Text.Trim, Date) < CType(txtCustApprvEffDate.Text.Trim, Date) Then
                                    txtCustApprvEndDate.Text = ""
                                End If
                            End If

                            ds = ARGroupModule.InsertAREvent(iPreviousAREID, iEventTypeID, 1, "Copy of AR Event ID: " & ViewState("AREID") & " - " & txtEventDesc.Text.Trim, iSalesID, txtCustApprvEffDate.Text.Trim, txtCustApprvEndDate.Text.Trim)

                            If commonFunctions.CheckDataSet(ds) = True Then
                                lblAREID.Text = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                                ViewState("AREID") = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                                ViewState("EventStatusID") = 1

                                'need to copy detail grid
                                ARGroupModule.CopyAREventDetail(ViewState("AREID"), iPreviousAREID)

                                Select Case iEventTypeID
                                    Case 2, 3
                                        'recalculate accruals
                                        ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                                        'copy override criteria
                                        ARGroupModule.CopyAREventAccrualOverrideCriteria(ViewState("AREID"), iPreviousAREID)
                                End Select

                                'update history
                                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Created Event - copied from AR Event ID: " & iPreviousAREID)

                                If ViewState("DefaultBillingTMID") > 0 Then
                                    ARGroupModule.InsertAREventApproval(ViewState("AREID"), 1, ViewState("DefaultBillingTMID"), 21, 1)
                                End If

                                Session("RecordCopied") = 1
                                Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

                            End If
                    End Select
                    'Case 4 'accounting accrual??

            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            ClearMessages()

            Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text

    End Sub

    Protected Sub btnCreateAccountingAccrual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateAccountingAccrual.Click

        Try
            ClearMessages()

            GetTeamMemberInfo()

            Dim ds As DataSet

            Dim iPreviousAREID As Integer = 0

            Dim iAcctMgrTMID As Integer = 0
            'Dim iVPofFinanceTMID As Integer = 0

            If ddAccountManager.SelectedIndex >= 0 Then
                iAcctMgrTMID = ddAccountManager.SelectedValue
            End If

            iPreviousAREID = ViewState("AREID")

            Select Case CType(ViewState("SubscriptionID"), Integer) 'billing with admin rights
                Case 21
                    'will always be eventtype of accounting accrual
                    'set event status of pending accountant submission
                    ds = ARGroupModule.InsertAREvent(iPreviousAREID, 4, 4, "Accountant Copied AR Event ID: " & ViewState("AREID"), iAcctMgrTMID, txtCustApprvEffDate.Text.Trim, txtCustApprvEndDate.Text.Trim)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        lblAREID.Text = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                        ViewState("AREID") = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                        ViewState("EventStatusID") = 4
                        ddEventStatus.SelectedValue = 4
                        lblMessage.Text &= "<br />Record copied and created successfully"

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Created Event - copied from AR Event ID: " & iPreviousAREID)
                    End If

                    'need to add CFO approval - then later after updating final accrual amount, check to see if CEO should be added
                    'iVPofFinanceTMID = GetTeamMemberIDBySubscriptionID(33, "")

                    If ViewState("CFOTMID") > 0 Then
                        ARGroupModule.InsertAREventApproval(ViewState("AREID"), 4, ViewState("CFOTMID"), 33, 1)
                    End If

                    Session("RecordCopied") = 1
                    Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text

    End Sub

    Protected Sub SendEmail(ByVal EmailToAddress As String, ByVal EmailCCAddress As String, _
        ByVal EmailSubject As String, ByVal EmailBody As String, ByVal IncludeCurrentTeamMember As Boolean)

        Dim bReturnValue As Boolean = False

        Try

            Dim strSubject As String = ""
            Dim strBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            Dim strEmailToAddress As String = commonFunctions.CleanEmailList(EmailToAddress)
            Dim strEmailCCAddress As String = commonFunctions.CleanEmailList(EmailCCAddress)

            If IncludeCurrentTeamMember = True Then
                If strEmailCCAddress.Trim <> "" Then
                    strEmailCCAddress &= ";"
                End If

                strEmailCCAddress &= strEmailFromAddress
            End If

            'handle test environment
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
            End If

            strSubject &= EmailSubject
            strBody &= EmailBody

            Dim dt As DataTable
            Dim objARSupportingDocBLL As AREventSupportingDocBLL = New AREventSupportingDocBLL
            Dim strSupportingDocURL As String = strProdOrTestEnvironment & "AR/AR_Supporting_Doc_Viewer.aspx?RowID="

            dt = objARSupportingDocBLL.GetAREventSupportingDoc(ViewState("AREID"))
            If commonFunctions.CheckDataTable(dt) = True Then

                strBody &= "<br /><br /><font size='1' face='Verdana'>Supporting Documents</font>"
                strBody &= "<table style='border: 1px solid #D0D0BF; width: 100%'>"

                For iRowCounter = 0 To dt.Rows.Count - 1
                    strBody &= "<tr>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'><a href=" & strSupportingDocURL & dt.Rows(iRowCounter).Item("RowID") & "&AREID=" & ViewState("AREID") & ">" & dt.Rows(iRowCounter).Item("SupportingDocName") & "</a></font></td>"
                    strBody &= "</tr>"
                Next

                strBody &= "</table>"
            End If

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br /><br />Email To Address List: " & EmailToAddress & "<br />"
                strBody &= "<br /><br />Email CC Address List: " & EmailCCAddress & "<br />"

                strEmailToAddress = "Lynette.Rey@ugnauto.com"
                strEmailCCAddress = ""
            End If



            strBody &= "<br /><br /><font size='1' face='Verdana'>"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"
            strBody &= "<br />If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the AR Module."
            strBody &= "<br />Please <u>do not</u> reply back to this email because you will not receive a response."
            strBody &= "<br />Please use a separate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br />"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++</font>"

            'set the content
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = strEmailToAddress.Split(";")

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

            'If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
            '    mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            'End If

            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "<br />Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "<br />Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("AR Submission", strEmailFromAddress, EmailToAddress, "", strSubject, strBody, "")
            End Try

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnRSSReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSSReset.Click

        Try
            ClearMessages()

            txtRSSComment.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Private Sub UpdateApprovalRoutingAcountingManager(ByVal StatusID As Integer)

        Try
            Dim objApproval As ARApprovalBLL = New ARApprovalBLL

            If ViewState("DefaultBillingTMID") > 0 And ViewState("BillingApprovalStatusID") <> 4 Then
                If ViewState("BillingApprovalRowID") = 0 Then
                    GetTeamMemberInfo()
                End If
                objApproval.UpdateAREventApprovalStatus(ViewState("AREID"), 1, ViewState("DefaultBillingTMID"), 21, "", StatusID, ViewState("BillingApprovalRowID"), ViewState("BillingApprovalRowID"))
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Private Sub UpdateApprovalRoutingSales(ByVal StatusID As Integer)

        Try
            Dim objApproval As ARApprovalBLL = New ARApprovalBLL

            If ViewState("AcctMgrTMID") > 0 And ViewState("SalesApprovalStatusID") <> 4 Then
                If ViewState("SalesApprovalRowID") = 0 Then
                    GetTeamMemberInfo()
                End If
                objApproval.UpdateAREventApprovalStatus(ViewState("AREID"), 2, ViewState("AcctMgrTMID"), 9, "", StatusID, ViewState("SalesApprovalRowID"), ViewState("SalesApprovalRowID"))
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub


    Protected Sub btnSubmitApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmitApproval.Click

        Try
            ClearMessages()

            If ViewState("BillingApprovalRowID") = 0 Then
                InsertAccrualApprovalRoutingAcountingManager()
            End If

            btnSave_Click(sender, e)

            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailApprovalURL As String = strProdOrTestEnvironment & "AR/crAR_Event_Approval.aspx?AREID="
            Dim strEmailDetailURL As String = strProdOrTestEnvironment & "AR/AR_Event_Detail.aspx?AREID="

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''Handle Overall Status and Team Member Approval Status 
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'if Sales (price change no accrual or invoice on hold no accrual), submit to Accounting Manager to In-Process for approval
            If ViewState("EventTypeID") = 1 Or ViewState("EventTypeID") = 5 Then
                Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                    Case 9, 23
                        'update event status
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 2) 'In-Process (Pending Accountant Event Approval)
                        ViewState("EventStatusID") = 2
                        ddEventStatus.SelectedValue = 2

                        'update approval status to inprocess and notification sent date for Billing
                        ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 21)

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Sales submitted for approval to Accounting Manager")

                        'notify default Accounting Manager
                        strEmailToAddress = ViewState("DefaultBillingEmail")

                        'assign email subject
                        strEmailSubject = "AR Event ID: " & ViewState("AREID") & " has been submitted and is pending Finance Accounting Manager approval"

                        'build email body
                        strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been has been submitted and is pending the Finance Accounting Manager approval:</font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailApprovalURL & ViewState("AREID") & "'>Click here to approve the event</a></font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & txtEventDesc.Text.Trim & "</font><br />"

                End Select
            End If

            'if part or customer accrual
            If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Then
                'if Sales, submit to Accounting Manager, set Sales as approved, set Accounting Manager as open
                Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                    Case 9, 23
                        'update event status
                        'ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 3) 'In-Process (Pending Sales for Customer Approval)
                        'ViewState("EventStatusID") = 3
                        'ddEventStatus.SelectedValue = 3

                        'update event status
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 2) 'In-Process (Pending Accountant Event Approval)
                        ViewState("EventStatusID") = 2
                        ddEventStatus.SelectedValue = 2

                        'UpdateApprovalRoutingAcountingManager(1) 'open
                        InsertAccrualApprovalRoutingAcountingManager()

                        'update approval status to inprocess and notification sent date for Billing baed on meeting on 09/18/2012
                        ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 21)

                        UpdateApprovalRoutingSales(4) 'approved

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Sales submitted to Accounting Manager")

                        'notify default Accounting Manager
                        strEmailToAddress = ViewState("DefaultBillingEmail")

                        'assign email subject
                        'strEmailSubject = "AR Event ID: " & ViewState("AREID") & " has been submitted and is pending the CUSTOMER APPROVAL by SALES"
                        strEmailSubject = "AR Event ID: " & ViewState("AREID") & " has been submitted and is pending ACCOUNTING MANAGER APPROVAL"

                        'build email body
                        'strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been has been submitted and is pending the CUSTOMER APPROVAL by SALES:</font><br /><br />"
                        strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been has been submitted and is pending the ACCOUNTING MANAGER APPROVAL:</font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailDetailURL & ViewState("AREID") & "'>Click here to review the event</a></font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & txtEventDesc.Text.Trim & "</font><br />"

                End Select

                'if Accounting Manager                
                If ViewState("SubscriptionID") = 21 Then
                    'set Accounting Manager and Sales as approved, if not done already - lots of changes to this logic so old events need to be caught up
                    UpdateApprovalRoutingAcountingManager(4) 'approved
                    UpdateApprovalRoutingSales(4) 'approved

                    'check to see if VPs are needed
                    'if so then set VP of Sales to in-process
                    If ViewState("VPSalesApprovalRowID") > 0 Then
                        'update event status
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 5) 'In-Process (Pending Deduction Approval)
                        ViewState("EventStatusID") = 5
                        ddEventStatus.SelectedValue = 5

                        'update approval status to inprocess and notification sent date for VPSales
                        ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 23)

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager submitted for approval to VP of Sales")

                        'notify VP of Sales
                        strEmailToAddress = ViewState("VPSalesEmail")

                        strEmailSubject = "APPROVAL REQUEST: AR Event ID:" & ViewState("AREID") & " has been approved by Accounting and Sales and is pending VP of Sales review"

                        'build email body
                        strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by Accounting and Sales. VP of Sales must review:</font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailApprovalURL & ViewState("AREID") & "'>Click here to approve the event</a></font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & txtEventDesc.Text.Trim & "</font><br />"

                    Else 'if not then set overall status to pending close
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 6) 'In-Process (Pending Accountant Close)
                        ViewState("EventStatusID") = 6
                        ddEventStatus.SelectedValue = 6

                        'no need to email anyone at this point
                        'strEmailToAddress = ViewState("BillingEmail")
                    End If

                End If

                ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                RecalculateTotals()
            End If

            'if Accounting Manager and Accounting Accrual, set VP of Finance to approve
            If ViewState("EventTypeID") = 4 And ViewState("SubscriptionID") = 21 Then
                ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                'update event status
                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 5) 'In-Process (Pending Deduction Form Approval)
                ViewState("EventStatusID") = 5
                ddEventStatus.SelectedValue = 5

                'update approval status to inprocess and notification sent date for VP of Finance
                ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 33)

                'update history
                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager submitted for approval to CFO")

                'notify CFO
                strEmailToAddress = ViewState("CFOEmail")

                'assign email subject
                strEmailSubject = "AR Event ID: " & ViewState("AREID") & " has been submitted and is pending CFO approval"

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been has been submitted and is pending the CFO approval:</font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailApprovalURL & ViewState("AREID") & "'>Click here to approve the event</a></font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & txtEventDesc.Text.Trim & "</font><br />"

            End If

            '''''''''''''''''''''''''''''''''
            ''need to CC all plant controllers
            ''''''''''''''''''''''''''''''''''
            'If ViewState("PlantControllerEmail") <> "" And (ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 4) Then
            If ViewState("PlantControllerEmail") <> "" And (ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3) Then
                If strEmailCCAddress <> "" Then
                    strEmailCCAddress &= ";"
                End If

                strEmailCCAddress &= ViewState("PlantControllerEmail")

                strEmailBody &= "<font size='2' face='Verdana' color='red'>Plant Controllers are NOT required to do any activity. They are only being copied for informational purposes.</font><br />"
            End If

            '''''''''''''''''''''''''''''''''''
            ' ''need to CC interested billing if Price Change or Invoice on Hold
            '''''''''''''''''''''''''''''''''''
            If ViewState("BillingEmail") <> "" And (ViewState("EventTypeID") = 1 Or ViewState("EventTypeID") = 5) Then
                If strEmailCCAddress <> "" Then
                    strEmailCCAddress &= ";"
                End If

                strEmailCCAddress &= ViewState("BillingEmail")
            End If

            If ViewState("EventStatusID") <> 6 Then
                SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, True)
            End If

            gvApproval.DataBind()

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

        '2012-Sep-04
        'Try
        '    ClearMessages()

        '    btnSave_Click(sender, e)

        '    Dim bIncludePlantControllers As Boolean = False

        '    Dim strEmailToAddress As String = ""
        '    Dim strEmailCCAddress As String = ""
        '    Dim strEmailSubject As String = ""
        '    Dim strEmailBody As String = ""

        '    Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
        '    Dim strEmailURL As String = strProdOrTestEnvironment & "AR/crAR_Event_Approval.aspx?AREID="

        '    '''''''''''''''''''''''''''''''''''''''''''
        '    'build list of recipients including backups
        '    '''''''''''''''''''''''''''''''''''''''''''

        '    If ViewState("EventStatusID") = 1 Or ViewState("EventStatusID") = 7 Then 'Open (Pending Sales Submission) OR Rejected (Pending Sales Fix)
        '        'notify default Accounting Manager
        '        strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

        '        'update event status
        '        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 2) 'In-Process (Pending Accountant Event Approval)

        '        ViewState("EventStatusID") = 2
        '        ddEventStatus.SelectedValue = 2

        '        'update approval status to inprocess and notification sent date for Billing
        '        ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 21)

        '        'update history
        '        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Sales submitted for approval to Accounting Manager")

        '        'update accruals and approver info               
        '        'update accrual details
        '        If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Then

        '            Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
        '                Case 9, 23
        '                    'update accrual details
        '                    ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

        '                    RecalculateTotals()

        '                    InsertUpdateAccrualApprovalRouting()
        '            End Select

        '            If ViewState("EventTypeID") = 3 Then
        '                If ViewState("SubscriptionID") = 21 And ViewState("isAdmin") = True Then
        '                    'update accrual details
        '                    ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

        '                    RecalculateTotals()

        '                    InsertUpdateAccrualApprovalRouting()
        '                End If
        '            End If
        '        End If

        '        ''notify billing if Price Change NO ACCRUAL
        '        'If ViewState("EventTypeID") = 1 Then
        '        '    ''''''''''''''''''''''''''''''''''
        '        '    ''need to CC all billing
        '        '    ''''''''''''''''''''''''''''''''''
        '        '    If ViewState("BillingEmail") <> "" Then
        '        '        strEmailCCAddress &= ViewState("BillingEmail")
        '        '    End If
        '        'End If

        '        ''''''''''''''''''''''''''''''''''
        '        ''need to CC all plant controllers
        '        ''''''''''''''''''''''''''''''''''
        '        If ViewState("PlantControllerEmail") <> "" And (ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 4) Then
        '            If strEmailCCAddress <> "" Then
        '                strEmailCCAddress &= ";"
        '            End If
        '            bIncludePlantControllers = True
        '            strEmailCCAddress &= ViewState("PlantControllerEmail")
        '        End If
        '    End If

        '    If ViewState("EventStatusID") = 4 Or ViewState("EventStatusID") = 8 Then 'In-Process (In-Process (Pending Accounting Mgr Submission for Approval) OR Rejected (Pending Accountant Fix)

        '        ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("DefaultBillingTMID"), ViewState("EventTypeID"))

        '        If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Then 'Part Accrual or Customer Accrual
        '            'notify sales
        '            strEmailToAddress = ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(iAcctMgrTMID)

        '            'update approval status to inprocess and notification sent date for Sales
        '            ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 9)

        '            'update history
        '            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager submitted for approval to Sales")
        '        End If

        '        If ViewState("EventTypeID") = 4 Then 'Accounting Accrual Only
        '            'notify CFO
        '            strEmailToAddress = ViewState("CFOEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(33, "", False)

        '            'update approval status to inprocess and notification sent date for VP of Finance
        '            ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 33)

        '            'update history
        '            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager submitted for approval to CFO")
        '        End If

        '        'update event status
        '        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 5) 'In-Process (Pending Deduction Form Approval)
        '        ViewState("EventStatusID") = 5
        '        ddEventStatus.SelectedValue = 5

        '    End If

        '    '''''''''''''''''''''''''''''''''''
        '    ' ''need to CC interested billing if Price Change or Invoice on Hold
        '    '''''''''''''''''''''''''''''''''''
        '    If ViewState("BillingEmail") <> "" And (ViewState("EventTypeID") = 1 Or ViewState("EventTypeID") = 5) Then
        '        If strEmailCCAddress <> "" Then
        '            strEmailCCAddress &= ";"
        '        End If

        '        strEmailCCAddress &= ViewState("BillingEmail")
        '    End If

        '    '''''''''''''''''''''''''''''''''''
        '    ' ''need to CC all plant controllers
        '    '''''''''''''''''''''''''''''''''''
        '    'If ViewState("PlantControllerEmail") <> "" Then
        '    '    If strEmailCCAddress <> "" Then
        '    '        strEmailCCAddress &= ";"
        '    '    End If

        '    '    strEmailCCAddress &= ViewState("PlantControllerEmail")
        '    'End If


        '    ''''''''''''''''''''''''''''''''''
        '    ''Build Email
        '    ''''''''''''''''''''''''''''''''''

        '    'assign email subject
        '    strEmailSubject = "APPROVAL REQUEST: AR Event ID: " & ViewState("AREID") & " is ready for review"

        '    'build email body
        '    strEmailBody = "<font size='2' face='Verdana'>The following AR Event is ready for your review:</font><br /><br />"
        '    strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
        '    strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
        '    strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & ViewState("AREID") & "'>Click here to review</a></font><br /><br />"
        '    strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtEventDesc.Text.Trim & "</font><br />"

        '    If bIncludePlantControllers = True Then
        '        strEmailBody &= "<font size='2' face='Verdana' color='red'>Plant Controllers are NOT required to do any activity. They are only being copied for informational purposes.</font><br />"
        '    End If

        '    If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, True) = True Then
        '        '    lblMessage.Text &= "<br />" & "Notfication Sent."
        '        'Else
        '        '    lblMessage.Text &= "<br />" & "Notfication Failed. Please contact IS."
        '    End If

        '    gvApproval.DataBind()

        '    EnableControls()

        'Catch ex As Exception

        '    'get current event name
        '    Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

        '    'update error on web page
        '    lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

        '    'log and email error
        '    UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        'End Try

        'lblMessageButtons.Text = lblMessage.Text
        'lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCustomerApproved_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomerApproved.Click

        Try

            ClearMessages()

            'set Accounting Manager approval status from open to in-process
            'set overall status to In-Process (Pending Accountant Deduction Form Submission)
            'notify billing

            Dim objApproval As ARApprovalBLL = New ARApprovalBLL

            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailEventURL As String = strProdOrTestEnvironment & "AR/AR_Event_Detail.aspx?AREID="

            If ViewState("EventStatusID") = 3 Then 'In-Process (Pending Sales for Customer Approval)

                If ViewState("SalesApprovalRowID") = 0 Then
                    InsertAccrualApprovalRoutingSales()
                End If

                btnSave_Click(sender, e)

                ARGroupModule.UpdateAREventCustomerApproved(ViewState("AREID"), True)

                'this may have been done already but a lot of old events before the logic changed need to be adjusted
                UpdateApprovalRoutingSales(4) 'approved

                '''''''''''''''''''''''''''''''''''''''''''
                'build list of recipients including backups
                '''''''''''''''''''''''''''''''''''''''''''
                'notify default billing
                strEmailToAddress = ViewState("DefaultBillingEmail")

                '''''''''''''''''''''''''''''''''''''''''''
                'include billing team members
                '''''''''''''''''''''''''''''''''''''''''''
                If ViewState("BillingEmail") <> "" Then
                    strEmailCCAddress = ViewState("BillingEmail")
                End If

                'not needed 09/18/2012
                ''set Accounting Manager from Open to In-Process to approve
                'If ViewState("DefaultBillingTMID") > 0 And ViewState("BillingApprovalStatusID") <> 4 Then
                '    objApproval.UpdateAREventApprovalStatus(ViewState("AREID"), 1, ViewState("DefaultBillingTMID"), 79, "", 2, ViewState("BillingApprovalRowID"), ViewState("BillingApprovalRowID"))
                'End If

                'update event status
                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 4) 'In-Process (Pending Accountant Deduction Form Submission)
                ViewState("EventStatusID") = 4
                ddEventStatus.SelectedValue = 4
                cbCustomerApproved.Checked = True

                'update history
                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Sales received Customer Approval")

                ''''''''''''''''''''''''''''''''''
                ''Build Email
                ''''''''''''''''''''''''''''''''''

                'assign email subject
                strEmailSubject = "CUSTOMER APPROVAL RECEIVED: AR Event ID: " & ViewState("AREID")

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>Sales has received customer approval for the following AR Event:</font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"

                strEmailBody = "<font size='2' face='Verdana'><b>PLEASE SUBMIT THE EVENT FOR APPROVAL TO ALL APPROPRIATE UGN TEAM MEMBERS</b></font><br /><br />"

                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to submit approval to all UGN Team Members</a></font><br /><br />"
                strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtEventDesc.Text.Trim & "</font><br />"

                SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, True)

                EnableControls()

                gvApproval.DataBind()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

        'Try

        '    ClearMessages()

        '    Dim strEmailToAddress As String = ""
        '    Dim strEmailCCAddress As String = ""
        '    Dim strEmailSubject As String = ""
        '    Dim strEmailBody As String = ""

        '    Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
        '    Dim strEmailEventURL As String = strProdOrTestEnvironment & "AR/AR_Event_Detail.aspx?AREID="

        '    If ViewState("EventStatusID") = 3 Then 'In-Process (Pending Sales for Customer Approval)

        '        btnSave_Click(sender, e)

        '        ARGroupModule.UpdateAREventCustomerApproved(ViewState("AREID"), True)

        '        '''''''''''''''''''''''''''''''''''''''''''
        '        'build list of recipients including backups
        '        '''''''''''''''''''''''''''''''''''''''''''
        '        'notify default billing
        '        strEmailToAddress = ViewState("DefaultBillingEmail") ' ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

        '        '''''''''''''''''''''''''''''''''''''''''''
        '        'include billing team members
        '        '''''''''''''''''''''''''''''''''''''''''''
        '        If ViewState("BillingEmail") <> "" Then
        '            strEmailCCAddress = ViewState("BillingEmail")
        '        End If


        '        'update event status
        '        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 4) 'In-Process (Pending Accountant Deduction Form Submission)
        '        ViewState("EventStatusID") = 4
        '        ddEventStatus.SelectedValue = 4
        '        cbCustomerApproved.Checked = True

        '        'update history
        '        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Sales received Customer Approval")

        '        ''''''''''''''''''''''''''''''''''
        '        ''Build Email
        '        ''''''''''''''''''''''''''''''''''

        '        'assign email subject
        '        strEmailSubject = "CUSTOMER APPROVAL RECEIVED: AR Event ID: " & ViewState("AREID")

        '        'build email body
        '        strEmailBody = "<font size='2' face='Verdana'>Sales has received customer approval for the following AR Event:</font><br /><br />"
        '        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"

        '        strEmailBody = "<font size='2' face='Verdana'><b>PLEASE SUBMIT THE EVENT FOR APPROVAL TO ALL APPROPRIATE UGN TEAM MEMBERS</b></font><br /><br />"

        '        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
        '        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to submit approval to all UGN Team Members</a></font><br /><br />"
        '        strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtEventDesc.Text.Trim & "</font><br />"

        '        If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, True) = True Then
        '            '    lblMessage.Text &= "<br />" & "Notfication Sent."
        '            'Else
        '            '    lblMessage.Text &= "<br />" & "Notfication Failed. Please contact IS."
        '        End If

        '        EnableControls()

        '    End If

        'Catch ex As Exception

        '    'get current event name
        '    Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

        '    'update error on web page
        '    lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

        '    'log and email error
        '    UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        'End Try

        'lblMessageButtons.Text = lblMessage.Text
        'lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnSaveReplyComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveReplyComment.Click

        Try
            ClearMessages()

            GetTeamMemberInfo()

            Dim strEmailBody As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            If ViewState("CurrentRSSID") > 0 Then
                'save reply
                ARGroupModule.InsertARRSSReply(ViewState("AREID"), ViewState("CurrentRSSID"), ViewState("TeamMemberID"), txtReply.Text.Trim)

                'update AR Event History
                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Message Sent: " & txtReply.Text.Trim)

                'current user is billing, then notify sales
                If ViewState("SubscriptionID") = 21 Then
                    strEmailToAddress = ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))                
                End If

                'current user is sales, then notify default billing
                If ViewState("SubscriptionID") = 9 Then
                    strEmailToAddress = ViewState("DefaultBillingEmail") ' ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)
                End If

                'include interested billing team members
                If ViewState("BillingEmail") <> "" Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress = ViewState("BillingEmail")
                End If

                'append active approvers
                If ViewState("ActiveApproverEmail") <> "" Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress = ViewState("ActiveApproverEmail")
                End If

                'current user is vp of sales
                If ViewState("SubscriptionID") = 23 Then
                    'notify default billing
                    strEmailToAddress = ViewState("DefaultBillingEmail") ' ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    'notify sales
                    strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

                End If

                'current user is vp of finance
                If ViewState("SubscriptionID") = 33 Then

                    ''notify default billing
                    strEmailToAddress = ViewState("DefaultBillingEmail")  'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                    'if not accounting accrual
                    If ViewState("EventTypeID") <> 4 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        'notify sales
                        strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        'notify VP of sales but no backup
                        strEmailToAddress &= ViewState("VPSalesEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)
                    End If

                End If

                'current user is CEO
                If ViewState("SubscriptionID") = 24 Then
                    'notify default billing
                    strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    'notify sales
                    strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    'notify VP of sales but no backup
                    strEmailToAddress &= ViewState("VPSalesEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)

                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    'notify CFO but no backup
                    strEmailToAddress &= ViewState("CFOEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(33, "", False)
                End If
                ''''''''''''''''''''''''''''''''''
                ''Build Email
                ''''''''''''''''''''''''''''''''''

                ''append active approvers
                'If ViewState("ActiveApproverEmail") <> "" Then
                '    If strEmailToAddress <> "" Then
                '        strEmailToAddress &= ";"
                '    End If

                '    strEmailToAddress = ViewState("ActiveApproverEmail")
                'End If

                ''dtApproval = objApproval.GetAREventApprovalStatus(ViewState("AREID"), 0)

                ' ''get all approvers
                ''For iRowCounter = 0 To dtApproval.Rows.Count - 1
                ''    iApprovalStatus = CType(dtApproval.Rows(iRowCounter).Item("StatusID").ToString, Integer)

                ''    'only include approvers who are pending, rejected, or approved
                ''    If iApprovalStatus > 1 Then
                ''        iApproverTMID = CType(dtApproval.Rows(iRowCounter).Item("TeamMemberID").ToString, Integer)

                ''        dsTeamMember = SecurityModule.GetTeamMember(iApproverTMID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

                ''        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                ''            If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                ''                If dsTeamMember.Tables(0).Rows(0).Item("Working") = True Then
                ''                    If strEmailToAddress <> "" Then
                ''                        strEmailToAddress &= ";"
                ''                    End If

                ''                    strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                ''                End If
                ''            End If
                ''        End If
                ''    End If
                ''Next

                'assign email subject
                strEmailSubject = "AR Question  - Event ID: " & ViewState("AREID") & " - MESSAGE receiveD"

                strEmailBody = "<table><tr><td valign='top' width='20%'><img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger.jpg'  height='20%' width='20%'/></td><td valign='top'>"
                'strEmailBody &= "<font size='3' face='Verdana'><b>Attention</b> "
                strEmailBody &= "<font size='3' face='Verdana'><p><b>" & strCurrentUserFullName & "</b> replied to the message regarding regarding AR Event ID: <font color='red'>" & ViewState("AREID") & "</font><br />"
                strEmailBody &= "<font size='3' face='Verdana'><p><b>Event Description:</b> <font>" & txtEventDesc.Text.Trim & "</font>.</p><br />"
                strEmailBody &= "<p><b>Question: </b><font>" & txtQuestionComment.Text.Trim & "</font></p><br /><br />"
                strEmailBody &= "<p><b>Reply: </b><font>" & txtReply.Text.Trim & "</font></p><br /><br />"

                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br /><br />"
                strEmailBody &= "<p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "&pRC=1" & "'>Click here</a> to answer the message.</font>"
                strEmailBody &= "</td></tr><tr><td colspan='2'>"

                SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, True)

                txtQuestionComment.Text = ""
                txtReply.Text = ""

                gvQuestion.DataBind()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text
        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub



    Protected Sub btnResetReplyComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetReplyComment.Click

        Try
            ClearMessages()

            ViewState("CurrentRSSID") = 0
            txtQuestionComment.Text = ""
            txtReply.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text
        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click

        Try
            ClearMessages()

            'GetTeamMemberInfo()

            'Dim objApproval As ARApprovalBLL = New ARApprovalBLL

            'Dim strEmailToAddress As String = ""
            'Dim strEmailCCAddress As String = ""
            'Dim strEmailSubject As String = ""
            'Dim strEmailBody As String = ""

            'Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            'Dim strEmailURL As String = strProdOrTestEnvironment & "AR/crPreview_AR_Event_Detail.aspx?AREID="

            '''''''''''''''''''''''''''''''''''''''''''
            'build list of recipients including backups
            '''''''''''''''''''''''''''''''''''''''''''

            If ViewState("EventStatusID") = 6 Then 'In-Process (Pending Accountant Close)

                'make sure it is an accruing type before updating the final deduction amount
                Select Case CType(ViewState("EventTypeID"), Integer)
                    Case 2, 3, 4
                        If txtFinalDeductionAmount.Text.Trim = "" Then
                            txtFinalDeductionAmount.Text = lblCalculatedDeductionAmountValue.Text.Trim
                        End If
                End Select

                btnSave_Click(sender, e)

                'update event status
                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 9) 'Closed
                ViewState("EventStatusID") = 9
                ddEventStatus.SelectedValue = 9

                'move data from current accrual data to archive table
                ARGroupModule.UpdateAREventAccrualClose(ViewState("AREID"))

                'update history
                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager closed the event.")

                '2011-June-03 - no need to notify anyone when event closes
                'price change NO accrual
                'If ViewState("EventTypeID") = 1 Then
                '    'notify sales
                '    strEmailToAddress = ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(iSalesID)
                'Else 'accruing                    
                '    strEmailToAddress &= ViewState("ActiveApproverEmail")

                '    ''''''''''''''''''''''''''''''''''
                '    ''need to CC all plant controllers
                '    ''''''''''''''''''''''''''''''''''
                '    If ViewState("PlantControllerEmail") <> "" Then
                '        If strEmailCCAddress <> "" Then
                '            strEmailCCAddress &= ";"
                '        End If

                '        strEmailCCAddress &= ViewState("PlantControllerEmail")
                '    End If
                'End If

                '''''''''''''''''''''''''''''''''''
                ' ''need to CC all billing
                '''''''''''''''''''''''''''''''''''
                'If ViewState("BillingEmail") <> "" Then
                '    If strEmailCCAddress <> "" Then
                '        strEmailCCAddress &= ";"
                '    End If

                '    strEmailCCAddress = ViewState("BillingEmail")
                'End If


                '''''''''''''''''''''''''''''''''''
                ' ''Build Email
                '''''''''''''''''''''''''''''''''''

                'If strEmailToAddress <> "" Then
                '    'assign email subject
                '    strEmailSubject = "AR Event ID: " & ViewState("AREID") & " has been closed"

                '    'build email body
                '    strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been closed:</font><br /><br />"
                '    strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                '    strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                '    strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & ViewState("AREID") & "'>Click here to review</a></font><br /><br />"
                '    strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtEventDesc.Text.Trim & "</font><br />"

                '    If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, true) = True Then
                '        '    lblMessage.Text &= "<br />" & "Notfication Sent."
                '        'Else
                '        '    lblMessage.Text &= "<br />" & "Notfication Failed. Please contact IS."
                '    End If
                'End If

                EnableControls()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Private Sub PushPriceAdjustmentForPartAccrual()

        Try

            'Dim dt As DataTable
            Dim objAREventApproval As New ARApprovalBLL

            Dim dNewPriceDollarAdjust As Double = 0
            Dim dNewPricePercent As Double = 0

            Dim iApprovalStatus As Integer = 0
            Dim iApprovalRowID As Integer = 0

            If rbPriceAdjustment.SelectedValue = "P" Then
                If txtPricePercent.Text.Trim <> "" Then
                    dNewPricePercent = CType(txtPricePercent.Text.Trim, Double)
                    lblPricePercentDecimal.Text = "( " & Format(dNewPricePercent / 100, "0.######") & " )"

                    dNewPriceDollarAdjust = 0
                    txtPriceDollar.Text = ""
                End If
            Else
                If txtPriceDollar.Text.Trim <> "" Then
                    txtPricePercent.Text = ""
                    dNewPriceDollarAdjust = CType(txtPriceDollar.Text.Trim, Double)

                    dNewPricePercent = 0
                    txtPricePercent.Text = ""
                End If
            End If

            If dNewPriceDollarAdjust <> 0 Or dNewPricePercent <> 0 Then
                ARGroupModule.UpdateAREventDetailPrice(ViewState("AREID"), dNewPricePercent, dNewPriceDollarAdjust)

                'update accrual details
                ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                RecalculateTotals()

                'InsertUpdateAccrualApprovalRouting()
                InsertAccrualApprovalRoutingLevelMiddle()
                InsertAccrualApprovalRoutingLevelLast()

                'if already approved by billing - set to open   
                If ViewState("BillingApprovalStatusID") = 4 Then
                    ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("DefaultBillingTMID"), ViewState("EventTypeID"))
                    ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                    ViewState("EventStatusID") = 1
                    ddEventStatus.SelectedValue = 1
                End If
                'dt = objAREventApproval.GetAREventApprovalStatus(ViewState("AREID"), 21)
                'If commonFunctions.CheckDataTable(dt) = True Then
                '    If dt.Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                '        iApprovalStatus = dt.Rows(0).Item("StatusID")

                '        'If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                '        '    iAccountingManagerID = dt.Rows(0).Item("TeamMemberID")
                '        'End If

                '        If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                '            iApprovalRowID = dt.Rows(0).Item("RowID")
                '        End If

                '        Select Case iApprovalStatus
                '            Case 0, 1, 3
                '                'do nothing
                '            Case 4 'already approved - set to open                                 
                '                'reset Accounting Manager approval
                '                ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("DefaultBillingTMID"), ViewState("EventTypeID"))
                '                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                '                ViewState("EventStatusID") = 1
                '                ddEventStatus.SelectedValue = 1
                '        End Select

                '    End If
                'End If 'end if dt has values

                gvDetail.DataBind()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub PushPriceAdjustmentForCustomerAccrual()

        Try

            'Dim ds As DataSet
            'Dim dt As DataTable
            Dim objAREventApproval As New ARApprovalBLL

            'Dim dNewPriceDollarAdjust As Double = 0
            Dim dNewPricePercent As Double = 0

            Dim iApprovalStatus As Integer = 0
            Dim iApprovalRowID As Integer = 0

            If rbPriceAdjustment.SelectedValue = "P" Then
                If txtPricePercent.Text.Trim <> "" Then
                    dNewPricePercent = CType(txtPricePercent.Text.Trim, Double)
                    lblPricePercentDecimal.Text = "( " & Format(dNewPricePercent / 100, "0.######") & " )"

                    'dNewPriceDollarAdjust = 0
                    'txtPriceDollar.Text = ""
                End If
            End If

            'If dNewPriceDollarAdjust <> 0 Or dNewPricePercent <> 0 Then
            If dNewPricePercent <> 0 Then
                ARGroupModule.UpdateAREventDetailPrice(ViewState("AREID"), dNewPricePercent, 0)

                If ViewState("EventStatusID") <> 9 And ViewState("EventStatusID") <> 10 Then
                    'update accrual details
                    ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))
                    RecalculateTotals()
                    'InsertUpdateAccrualApprovalRouting()
                    InsertAccrualApprovalRoutingLevelMiddle()
                    InsertAccrualApprovalRoutingLevelLast()
                End If


                'if already approved by billing - set to open   
                If ViewState("BillingApprovalStatusID") = 4 Then
                    ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("DefaultBillingTMID"), ViewState("EventTypeID"))
                    ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                    ViewState("EventStatusID") = 1
                    ddEventStatus.SelectedValue = 1
                End If

                'dt = objAREventApproval.GetAREventApprovalStatus(ViewState("AREID"), 21)
                'If commonFunctions.CheckDataTable(dt) = True Then
                '    If dt.Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                '        iApprovalStatus = dt.Rows(0).Item("StatusID")

                '        'If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                '        '    iAccountingManagerID = dt.Rows(0).Item("TeamMemberID")
                '        'End If

                '        If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                '            iApprovalRowID = dt.Rows(0).Item("RowID")
                '        End If

                '        Select Case iApprovalStatus
                '            Case 0, 1, 3
                '                'do nothing
                '            Case 4 'already approved - set to open                                 
                '                'reset Accounting Manager approval
                '                ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("DefaultBillingTMID"), ViewState("EventTypeID"))
                '                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                '                ViewState("EventStatusID") = 1
                '                ddEventStatus.SelectedValue = 1
                '        End Select

                '      End If
                'End If 'end if dt has values
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub PushPriceAdjustmentForPriceChangeNoAccrual()

        Try
            'Dim ds As DataSet
            Dim dt As DataTable
            Dim objAREventApproval As New ARApprovalBLL

            'Dim dCurrentPrice As Double = 0
            'Dim dNewPrice As Double = 0
            Dim dNewPriceDollarAdjust As Double = 0
            Dim dNewPricePercent As Double = 0

            Dim iApprovalStatus As Integer = 0
            'Dim iAccountingManagerID As Integer = 0
            Dim iApprovalRowID As Integer = 0

            'Dim iEventTypeID As Integer = 0

            'If ddEventType.SelectedIndex >= 0 Then
            ' iEventTypeID = ddEventType.SelectedValue
            'End If

            ''get most recent current price
            'ds = ARGroupModule.GetAREventPriceMasterList(ViewState("AREID"))
            'If commonFunctions.CheckDataSet(ds) = True Then
            '    If ds.Tables(0).Rows(0).Item("RELPRC") IsNot System.DBNull.Value Then
            '        dCurrentPrice = ds.Tables(0).Rows(0).Item("RELPRC")
            '    End If
            'End If

            ''precentage takes precedence over price
            'If txtPricePercent.Text.Trim <> "" Then
            '    dNewPricePercent = CType(txtPricePercent.Text.Trim, Double)
            '    lblPricePercentDecimal.Text = "( " & Format(dNewPricePercent / 100, "0.######") & " )"

            '    If dNewPricePercent <> 0 Then
            '        dNewPrice = ((dNewPricePercent / 100) * dCurrentPrice) + dCurrentPrice
            '        txtPriceDollar.Text = Format(dNewPrice, "0.######")
            '    End If
            'End If

            'If txtPriceDollar.Text.Trim <> "" And dNewPricePercent = 0 Then
            '    dNewPrice = CType(txtPriceDollar.Text.Trim, Double)

            '    'recalculate percentage if the set percentage is 0 and a price has a value
            '    If dCurrentPrice > 0 Then
            '        dNewPricePercent = ((dNewPrice - dCurrentPrice) / dCurrentPrice) * 100
            '        txtPricePercent.Text = Format(dNewPricePercent, "0.######")
            '    End If
            'End If

            If rbPriceAdjustment.SelectedValue = "P" Then
                If txtPricePercent.Text.Trim <> "" Then
                    dNewPricePercent = CType(txtPricePercent.Text.Trim, Double)
                    lblPricePercentDecimal.Text = "( " & Format(dNewPricePercent / 100, "0.######") & " )"

                    dNewPriceDollarAdjust = 0
                    txtPriceDollar.Text = ""
                End If
            Else
                If txtPriceDollar.Text.Trim <> "" Then
                    txtPricePercent.Text = ""
                    dNewPriceDollarAdjust = CType(txtPriceDollar.Text.Trim, Double)

                    dNewPricePercent = 0
                    txtPricePercent.Text = ""
                End If
            End If

            If dNewPriceDollarAdjust <> 0 Or dNewPricePercent <> 0 Then
                ARGroupModule.UpdateAREventDetailPrice(ViewState("AREID"), dNewPricePercent, dNewPriceDollarAdjust)

                dt = objAREventApproval.GetAREventApprovalStatus(ViewState("AREID"), 21)
                If commonFunctions.CheckDataTable(dt) = True Then
                    If dt.Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                        iApprovalStatus = dt.Rows(0).Item("StatusID")

                        'If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                        '    iAccountingManagerID = dt.Rows(0).Item("TeamMemberID")
                        'End If

                        If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                            iApprovalRowID = dt.Rows(0).Item("RowID")
                        End If

                        Select Case iApprovalStatus
                            Case 0, 1, 3
                                'do nothing
                            Case 4 'already approved - set to open
                                'objAREventApproval.UpdateAREventApprovalStatus(ViewState("AREID"), 1, iAccountingManagerID, 21, "", 1, iApprovalRowID, iApprovalRowID)
                                'reset Accounting Manager approval
                                ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("DefaultBillingTMID"), ViewState("EventTypeID"))
                                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                                ViewState("EventStatusID") = 1
                                ddEventStatus.SelectedValue = 1
                        End Select
                    End If
                End If 'end if dt has values

                gvDetail.DataBind()

            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub PushPriceAdjustmentForInvoiceOnHoldNoAccrual()

        Try

            Dim dt As DataTable
            Dim objAREventApproval As New ARApprovalBLL

            Dim dCurrentPrice As Double = 0
            Dim dNewPrice As Double = 0
            Dim dNewPricePercent As Double = 0

            Dim iApprovalStatus As Integer = 0
            'Dim iAccountingManagerID As Integer = 0
            Dim iApprovalRowID As Integer = 0

            'Dim iEventTypeID As Integer = 0

            'If ddEventType.SelectedIndex >= 0 Then
            '    iEventTypeID = ddEventType.SelectedValue
            'End If

            If txtPriceDollar.Text.Trim <> "" Then
                dNewPrice = CType(txtPriceDollar.Text.Trim, Double)
            End If

            If dNewPrice > 0 Then
                ARGroupModule.UpdateAREventDetailPrice(ViewState("AREID"), 0, dNewPrice)

                gvAffectedInvoicesOnHold.DataBind()
                gvInvoicesOnHold.DataBind()

                dt = objAREventApproval.GetAREventApprovalStatus(ViewState("AREID"), 21)
                If commonFunctions.CheckDataTable(dt) = True Then
                    If dt.Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                        iApprovalStatus = dt.Rows(0).Item("StatusID")

                        'If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                        '    iAccountingManagerID = dt.Rows(0).Item("TeamMemberID")
                        'End If

                        If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                            iApprovalRowID = dt.Rows(0).Item("RowID")
                        End If

                        Select Case iApprovalStatus
                            Case 0, 1, 3
                                'do nothing
                            Case 4 'already approved - set to open
                                'objAREventApproval.UpdateAREventApprovalStatus(ViewState("AREID"), 1, iAccountingManagerID, 21, "", 1, iApprovalRowID, iApprovalRowID)
                                'reset Accounting Manager approval
                                ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("DefaultBillingTMID"), ViewState("EventTypeID"))
                                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                                ViewState("EventStatusID") = 1
                                ddEventStatus.SelectedValue = 1
                        End Select
                    End If
                End If 'end if dt has values
            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnPushAdjustments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPushAdjustments.Click

        Try
            ClearMessages()

            Select Case CType(ViewState("EventTypeID"), Integer)
                Case 1
                    PushPriceAdjustmentForPriceChangeNoAccrual()
                Case 2
                    PushPriceAdjustmentForPartAccrual()
                Case 3
                    PushPriceAdjustmentForCustomerAccrual()
                Case 5
                    PushPriceAdjustmentForInvoiceOnHoldNoAccrual()
            End Select

            'notify accounting that price has been updated if the overall status is in-process
            'only notify once per grid and not every row
            If ViewState("EventStatusID") > 1 And ViewState("isBillingNotified") = False And (ViewState("EventTypeID") = 1 Or ViewState("EventTypeID") = 5) Then
                ViewState("isBillingNotified") = True

                NotifyBilling()
            End If

            gvDetail.DataBind()

            If lblMessage.Text = "" Then
                lblMessage.Text = "<br />New Pricing has been saved."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text

    End Sub

    ' ''Protected Sub gvDetail_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvDetail.RowCancelingEdit

    ' ''    SetFutureParts()

    ' ''End Sub

    Protected Sub gvDetail_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try
            Dim UGNFacility As DropDownList
            Dim PartNo As TextBox
            Dim PriceCode As DropDownList
            Dim Customer As DropDownList
            Dim CurrentPrice As TextBox
            Dim PricePercent As TextBox
            Dim NewPrice As TextBox
            Dim EstimatedPrice As TextBox

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then
                If gvDetail.Rows.Count = 0 Then
                    '' We are inserting through the DetailsView in the EmptyDataTemplate
                    Return
                End If

                UGNFacility = CType(gvDetail.FooterRow.FindControl("ddUGNFacilityInsert"), DropDownList)
                odsDetail.InsertParameters("COMPNY").DefaultValue = UGNFacility.SelectedValue

                Customer = CType(gvDetail.FooterRow.FindControl("ddCustomerInsert"), DropDownList)
                odsDetail.InsertParameters("Customer").DefaultValue = Customer.SelectedValue

                PartNo = CType(gvDetail.FooterRow.FindControl("txtPartNoInsert"), TextBox)
                odsDetail.InsertParameters("PartNo").DefaultValue = PartNo.Text

                PriceCode = CType(gvDetail.FooterRow.FindControl("ddPriceCodeInsert"), DropDownList)
                odsDetail.InsertParameters("PRCCDE").DefaultValue = PriceCode.SelectedValue

                PricePercent = CType(gvDetail.FooterRow.FindControl("txtPricePercentInsert"), TextBox)
                odsDetail.InsertParameters("PRCPRNT").DefaultValue = PricePercent.Text

                NewPrice = CType(gvDetail.FooterRow.FindControl("txtPriceDollarInsert"), TextBox)
                odsDetail.InsertParameters("PRCDOLR").DefaultValue = NewPrice.Text

                CurrentPrice = CType(gvDetail.FooterRow.FindControl("txtCurrentPriceInsert"), TextBox)
                odsDetail.InsertParameters("USE_RELPRC").DefaultValue = CurrentPrice.Text

                EstimatedPrice = CType(gvDetail.FooterRow.FindControl("txtEstimatedPriceInsert"), TextBox)
                odsDetail.InsertParameters("ESTPRC").DefaultValue = EstimatedPrice.Text

                odsDetail.Insert()

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDetail.ShowFooter = False
            Else
                If gvDetail.Rows.Count = 1 And ViewState("EventTypeID") = 5 Then
                    gvDetail.ShowFooter = False
                Else
                    gvDetail.ShowFooter = True
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                UGNFacility = CType(gvDetail.FooterRow.FindControl("ddUGNFacilityInsert"), DropDownList)
                UGNFacility.SelectedValue = ""

                PartNo = CType(gvDetail.FooterRow.FindControl("txtPartNoInsert"), TextBox)
                PartNo.Text = ""

                PriceCode = CType(gvDetail.FooterRow.FindControl("ddPriceCodeInsert"), DropDownList)
                PriceCode.SelectedValue = "A"

                PricePercent = CType(gvDetail.FooterRow.FindControl("txtPricePercentInsert"), TextBox)
                PricePercent.Text = ""

                NewPrice = CType(gvDetail.FooterRow.FindControl("txtPriceDollarInsert"), TextBox)
                NewPrice.Text = ""

                CurrentPrice = CType(gvDetail.FooterRow.FindControl("txtCurrentPriceInsert"), TextBox)
                CurrentPrice.Text = ""

                EstimatedPrice = CType(gvDetail.FooterRow.FindControl("txtEstimatedPriceInsert"), TextBox)
                EstimatedPrice.Text = ""
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

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_gvDetail() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_gvDetail") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_gvDetail"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_gvDetail") = value
        End Set

    End Property

    Protected Sub odsDetail_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsDetail.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As AR.AREventDetailDataTable = CType(e.ReturnValue, AR.AREventDetailDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_gvDetail = True
            Else
                LoadDataEmpty_gvDetail = False
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

    Protected Sub gvDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetail.RowCreated

        Try
            'hide columns
            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(5).Attributes.CssStyle.Add("display", "none")
            End If

            If ViewState("EventTypeID") = 5 Then
                Dim objAREventDetailBLL As New AREventDetailBLL
                Dim dtAREventDetail As DataTable
                dtAREventDetail = objAREventDetailBLL.GetAREventDetail(ViewState("AREID"))
                If commonFunctions.CheckDataTable(dtAREventDetail) = False Then
                    gvDetail.ShowFooter = True
                Else
                    gvDetail.ShowFooter = False
                End If
            End If
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_gvDetail
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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
#End Region ' Insert Empty GridView Work-Around

    Protected Sub gvDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetail.RowDataBound

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim strWhereClause As String = ""

            Dim strTempSQLRowID As String = ""
            Dim iSQLRowID As Integer = 0

            Dim strTempCOMPNY As String = ""
            Dim strTempPARTNO As String = ""
            Dim strTempPRCCDE As String = ""

            Dim ddTempCurrentPrice As DropDownList
            Dim lblTempCurrentPrice As Label
            Dim strTempCurrentPrice As String = ""

            Dim lblTempViewPartNo As Label
            Dim lnkTempViewPFSalesProjection As HyperLink

            If e.Row.RowType = DataControlRowType.DataRow Then

                lblTempViewPartNo = CType(e.Row.FindControl("lblViewPartNo"), Label)
                lnkTempViewPFSalesProjection = CType(e.Row.FindControl("lnkViewPFSalesProjection"), HyperLink)

                If ViewState("SubscriptionID") = 9 And lblTempViewPartNo.Text.ToUpper <> "ALL" Then
                    lblTempViewPartNo.Visible = False
                    lnkTempViewPFSalesProjection.Visible = True
                Else
                    lblTempViewPartNo.Visible = True
                    lnkTempViewPFSalesProjection.Visible = False
                End If


                strTempSQLRowID = e.Row.Cells(1).Text.Trim

                If strTempSQLRowID IsNot Nothing Then
                    If strTempSQLRowID <> "" Then

                        strTempCOMPNY = e.Row.Cells(2).Text.Trim
                        strTempPARTNO = lblTempViewPartNo.Text.Trim 'e.Row.Cells(4).Text.Trim
                        strTempPRCCDE = e.Row.Cells(5).Text.Trim

                        If strTempCOMPNY <> "" And strTempCOMPNY.ToUpper <> "ALL" Then
                            strWhereClause &= " AND COMPNY = '" & strTempCOMPNY & "'"
                        End If

                        If strTempPARTNO <> "" And strTempPARTNO.ToUpper <> "ALL" Then
                            strWhereClause &= " AND PARTNO = '" & strTempPARTNO & "'"
                        End If

                        If strTempPRCCDE <> "" And strTempPRCCDE.ToUpper <> "ALL" Then
                            strWhereClause &= " AND PRCCDE = '" & strTempPRCCDE & "'"
                        End If

                        iSQLRowID = CType(strTempSQLRowID, Integer)

                        If ViewState("AREID") > 0 And iSQLRowID > 0 And strWhereClause <> "" Then

                            ddTempCurrentPrice = CType(e.Row.FindControl("ddEditCurrentPrice"), DropDownList)
                            If ddTempCurrentPrice IsNot Nothing Then
                                lblTempCurrentPrice = CType(e.Row.FindControl("lblEditUSE_RELPRC"), Label) 'e.Row.Cells(8).Text.Trim
                                If lblTempCurrentPrice IsNot Nothing Then
                                    strTempCurrentPrice = lblTempCurrentPrice.Text.Trim
                                End If

                                ddTempCurrentPrice.Items.Clear()

                                'ds = ARGroupModule.GetARShippingPriceDynamically(ViewState("AREID"), iSQLRowID, strWhereClause)
                                ds = ARGroupModule.GetARShippingPriceDynamically(ViewState("AREID"), strWhereClause)
                                If commonFunctions.CheckDataSet(ds) = True Then
                                    ddTempCurrentPrice.DataSource = ds
                                    ddTempCurrentPrice.DataTextField = ds.Tables(0).Columns("ddPriceWithDate").ColumnName
                                    ddTempCurrentPrice.DataValueField = ds.Tables(0).Columns("USE_RELPRC").ColumnName
                                    ddTempCurrentPrice.DataBind()

                                    ddTempCurrentPrice.SelectedValue = strTempCurrentPrice 'dTempCurrentPrice
                                    ddTempCurrentPrice.DataBind()
                                    'ddTempPrice.Items.Insert(0, "")
                                End If
                            End If

                        End If
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvDetail_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvDetail.RowDeleted

        Try

            If ViewState("EventTypeID") = 5 Then
                gvAffectedInvoicesOnHold.DataBind()
                gvInvoicesOnHold.DataBind()
            End If

            'if any inprocess accruing event
            If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Or ViewState("EventTypeID") = 4 Then
                If (ViewState("SubscriptionID") = 9 Or ViewState("SubscriptionID") = 21 Or ViewState("SubscriptionID") = 23) And ViewState("isAdmin") = True Then

                    'if inprocess
                    If ViewState("EventStatusID") <> 9 And ViewState("EventStatusID") <> 10 Then
                        'update accrual details
                        ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                        'only update for part and customer accrual
                        If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Then
                            RecalculateTotals()
                            'InsertUpdateAccrualApprovalRouting()
                            InsertAccrualApprovalRoutingLevelMiddle()
                            InsertAccrualApprovalRoutingLevelLast()
                        End If

                        EnableControls()
                    End If

                End If

            End If

            ''SetFutureParts()

            'if there are details selected and NOT an accounting accrual
            btnExportToExcel.Visible = False
            If ViewState("EventTypeID") <> 4 Then
                If gvDetail.Rows.Count > 1 Then
                    btnExportToExcel.Visible = True
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        gvDetail.DataBind()
        lblMessageButtons.Text &= "<br />" & lblMessage.Text
        lblMessageBottom.Text &= "<br />" & lblMessage.Text

    End Sub

    Public Function DisplayImage(ByVal EncodeType As String) As String
        Dim strReturn As String = ""

        If EncodeType = Nothing Then
            strReturn = ""
        ElseIf EncodeType = "application/vnd.ms-excel" Or EncodeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" Then
            strReturn = "~/images/xls.jpg"
        ElseIf EncodeType = "application/pdf" Then
            strReturn = "~/images/pdf.jpg"
        ElseIf EncodeType = "application/msword" Or EncodeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" Then
            strReturn = "~/images/doc.jpg"
        Else
            strReturn = "~/images/PreviewUp.jpg"
        End If

        Return strReturn
    End Function 'EOF DisplayImage

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click

        Try
            ClearMessages()

            If ViewState("AREID") > 0 Then
                If fileUploadSupportingDoc.HasFile Then
                    If fileUploadSupportingDoc.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(fileUploadSupportingDoc.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(fileUploadSupportingDoc.PostedFile.FileName)

                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(fileUploadSupportingDoc.PostedFile.InputStream.Length)

                        Dim SupportingDocEncodeType As String = fileUploadSupportingDoc.PostedFile.ContentType

                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        fileUploadSupportingDoc.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".xlsx") Or (FileExt = ".doc") Or (FileExt = ".docx") Or (FileExt = ".jpeg") Or (FileExt = ".jpg") Or (FileExt = ".tif") Or (FileExt = ".msg") Or (FileExt = ".ppt") Or (FileExt = ".pptx") Then

                            ''***************
                            '' Insert Record
                            ''***************
                            ARGroupModule.InsertAREventSupportingDocument(ViewState("AREID"), fileUploadSupportingDoc.FileName, txtSupportingDocDesc.Text.Trim, SupportingDocBinaryFile, SupportingDocFileSize, SupportingDocEncodeType)

                            revUploadFile.Enabled = False

                            lblMessage.Text &= "<br />File Uploaded Successfully<br />"

                            gvSupportingDoc.DataBind()
                            gvSupportingDoc.Visible = True

                            revUploadFile.Enabled = True

                            txtSupportingDocDesc.Text = ""
                        End If

                    Else
                        lblMessage.Text = "File exceeds size limit.  Please select a file less than 3MB (3000KB)."
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
    End Sub

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvQuestion.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim RSSID As Integer

                Dim drRSSID As AR.ARRSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, AR.ARRSSRow)

                If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                    RSSID = drRSSID.RSSID
                    ' Reference the rpCBRC ObjectDataSource
                    Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                    ' Set the Parameter value
                    rpCBRC.SelectParameters("AREID").DefaultValue = drRSSID.AREID.ToString
                    rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnNotifyPriceUpdatedByAccounting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotifyPriceUpdatedByAccounting.Click

        Try
            ClearMessages()

            GetTeamMemberInfo()

            'Dim iEventTypeID As Integer = 0

            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            Dim strEmailSubject As String = ""
            Dim strEmailBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailURL As String = strProdOrTestEnvironment & "AR/crAR_Event_Approval.aspx?AREID="

            Dim dCalculatedQuantityShipped As Integer = 0
            Dim dCalculatedDeductionAmount As Double = 0
            Dim dFinalDeductionAmount As Double = 0

            If lblQuantityShippedValue.Text.Trim <> "" Then
                dCalculatedQuantityShipped = CType(lblQuantityShippedValue.Text.Trim, Double)
            End If

            If lblCalculatedDeductionAmountValue.Text.Trim <> "" Then
                dCalculatedDeductionAmount = CType(lblCalculatedDeductionAmountValue.Text.Trim, Double)
            End If

            If txtFinalDeductionAmount.Text.Trim <> "" Then
                dFinalDeductionAmount = CType(txtFinalDeductionAmount.Text.Trim, Double)
            End If

            If ViewState("AREID") > 0 Then
                If ViewState("SubscriptionID") = 21 Then 'billing/accounting

                    If cbPriceUpdatedByAccounting.Checked = True And lblPriceChangeDate.Text.Trim = "" Then
                        lblPriceChangeDate.Text = Today.Date
                    End If

                    ARGroupModule.UpdateAREventBilling(ViewState("AREID"), txtEventDesc.Text.Trim, ViewState("AcctMgrTMID"), txtCustApprvEndDate.Text.Trim, dCalculatedQuantityShipped, dCalculatedDeductionAmount, dFinalDeductionAmount, txtDeductionReason.Text.Trim, cbPriceUpdatedByAccounting.Checked, lblPriceChangeDate.Text.Trim, txtCreditDebitMemo.Text.Trim, txtCreditDebitDate.Text.Trim, txtBPCSInvoiceNo.Text.Trim)

                    If cbPriceUpdatedByAccounting.Checked = True Then

                        'If ddEventType.SelectedIndex > 0 Then
                        '    iEventTypeID = ddEventType.SelectedValue
                        'End If

                        '''''''''''''''''''''''''''''''''''''''''''
                        'build list of recipients including backups
                        '''''''''''''''''''''''''''''''''''''''''''

                        'notify default Accounting Manager
                        strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting updated price in Future 3")

                        If strEmailToAddress <> "" Then
                            ''''''''''''''''''''''''''''''''''
                            ''Build Email
                            ''''''''''''''''''''''''''''''''''

                            'assign email subject
                            strEmailSubject = "Pricing has been updated in Future 3 for AR Event ID: " & ViewState("AREID")

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following AR Event is ready for your approval because pricing or other accounting information has been updated in the AR Event or Future 3:</font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & ViewState("AREID") & "'>Click here to review</a></font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtEventDesc.Text.Trim & "</font><br />"

                            SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, False)

                        End If

                        EnableControls()

                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvSupportingDoc_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvSupportingDoc.RowDeleted

        ClearMessages()

    End Sub

    'Protected Sub gvDetail_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvDetail.RowEditing

    '    tblPriceAdjustment.Visible = False

    'End Sub

    Protected Sub gvDetail_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvDetail.RowUpdated

        Try

            'this should not be affected by accounting accrual
            If (ViewState("SubscriptionID") = 9 Or ViewState("SubscriptionID") = 21 Or ViewState("SubscriptionID") = 23) And ViewState("isAdmin") = True Then

                If ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3 Then

                    If ViewState("EventStatusID") <> 9 And ViewState("EventStatusID") <> 10 Then

                        'update accrual details
                        ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))
                        RecalculateTotals()
                        'InsertUpdateAccrualApprovalRouting()
                        InsertAccrualApprovalRoutingLevelMiddle()
                        InsertAccrualApprovalRoutingLevelLast()
                    End If
                End If

                EnableControls()

                'notify accounting that price has been updated if the overall status is in-process
                'only notify once per grid and not every row
                If ViewState("EventStatusID") > 1 And ViewState("isBillingNotified") = False And (ViewState("EventTypeID") = 1 Or ViewState("EventTypeID") = 5) Then
                    ViewState("isBillingNotified") = True

                    If ViewState("NewPriceFound") = True Then
                        NotifyBilling()
                    End If
                End If
            End If

            'SetFutureParts()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text &= "<br />" & lblMessage.Text
        lblMessageBottom.Text &= "<br />" & lblMessage.Text

    End Sub

    Protected Sub gvDetail_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvDetail.RowUpdating

        Try

            ' Retrieve the row being edited.
            Dim row As GridViewRow = gvDetail.Rows(gvDetail.EditIndex)

            ' Retrieve the DropDownList control from the row.
            'LR ''Dim ddTempCurrentPrice As DropDownList = CType(row.FindControl("ddEditCurrentPrice"), DropDownList)

            Dim ddTempCurrentPrice As TextBox
            ddTempCurrentPrice = CType(row.FindControl("txtEditCurrentPrice"), TextBox)


            Dim txtTempEditPriceDollar As TextBox
            txtTempEditPriceDollar = CType(row.FindControl("txtEditPriceDollar"), TextBox)

            If ddTempCurrentPrice IsNot Nothing Then
                ' Add the selected value of the DropDownList control to 
                ' the NewValues collection. The NewValues collection is
                ' passed to the data source control, which then updates the 
                ' data source.
                e.NewValues("USE_RELPRC") = ddTempCurrentPrice.Text
            End If

            ViewState("NewPriceFound") = False
            If txtTempEditPriceDollar IsNot Nothing Then
                If txtTempEditPriceDollar.Text.Trim <> "" Then
                    If CType(txtTempEditPriceDollar.Text.Trim, Double) <> 0 Then
                        ViewState("NewPriceFound") = True
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub rbPriceAdjustment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbPriceAdjustment.SelectedIndexChanged

        Try
            HandlePercentDollarChoice()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddEventType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddEventType.SelectedIndexChanged

        Try
            Dim iEventTypeID As Integer = 0

            If ddEventType.SelectedIndex >= 0 And ViewState("AREID") > 0 Then
                iEventTypeID = ddEventType.SelectedValue

                'sales can change certain event types, but selection criteria will be lost
                Select Case CType(ViewState("SubscriptionID"), Integer) 'sales, vp of sales 
                    Case 9, 23

                        If iEventTypeID <> ViewState("EventTypeID") Then

                            'only wipe out details if switching to or from customer accrual
                            If (iEventTypeID = 1 And ViewState("EventTypeID") = 3) Or (iEventTypeID = 3) Then
                                ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "")
                            End If

                            'wipe out end date if going to or from price change no accrual
                            If ViewState("EventTypeID") = 1 Or iEventTypeID = 1 Then
                                txtCustApprvEndDate.Text = ""
                            End If

                            'wipe out end date if less than effective date
                            If txtCustApprvEndDate.Text.Trim <> "" Then
                                If CType(txtCustApprvEndDate.Text.Trim, Date) < CType(txtCustApprvEffDate.Text.Trim, Date) Then
                                    txtCustApprvEndDate.Text = ""
                                End If
                            End If

                            ViewState("EventTypeID") = iEventTypeID

                            ARGroupModule.UpdateAREventSales(ViewState("AREID"), txtEventDesc.Text.Trim, ViewState("EventTypeID"), ViewState("AcctMgrTMID"), txtCustApprvEffDate.Text.Trim, txtCustApprvEndDate.Text.Trim)

                            ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                            ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("DefaultBillingTMID"), ViewState("EventTypeID"))
                            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                            ViewState("EventStatusID") = 1
                            ddEventStatus.SelectedValue = 1

                            If iEventTypeID = 2 Or iEventTypeID = 3 Then
                                RecalculateTotals()

                                'InsertUpdateAccrualApprovalRouting()
                                InsertAccrualApprovalRoutingLevelMiddle()
                                InsertAccrualApprovalRoutingLevelLast()
                            Else
                                'remove sales from approval list 
                                ARGroupModule.DeleteAREventApprovalStatus(ViewState("AREID"), 9)
                            End If

                            gvApproval.DataBind()
                            gvDetail.DataBind()

                            EnableControls()
                        End If
                End Select
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCreatePriceChangeNoAccrual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreatePriceChangeNoAccrual.Click

        Try
            ClearMessages()

            Session("RecordCopied") = Nothing

            Dim ds As DataSet

            Dim iPreviousAREID As Integer = 0

            Dim iSalesID As Integer = 0
            'Dim iEventTypeID As Integer = 0

            If ddAccountManager.SelectedIndex >= 0 Then
                iSalesID = ddAccountManager.SelectedValue
            End If

            iPreviousAREID = ViewState("AREID")

            If ViewState("EventTypeID") = 5 And ViewState("SubscriptionID") = 21 Then
                ds = ARGroupModule.InsertAREvent(iPreviousAREID, 1, 2, "Copy of AR Event ID: " & ViewState("AREID") & " - " & txtEventDesc.Text.Trim, iSalesID, txtCustApprvEffDate.Text.Trim, "")

                If commonFunctions.CheckDataSet(ds) = True Then
                    lblAREID.Text = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                    ViewState("AREID") = ds.Tables(0).Rows(0).Item("NewAREID").ToString
                    ViewState("EventStatusID") = 2

                    'need to copy detail grid
                    ARGroupModule.CopyAREventDetail(ViewState("AREID"), iPreviousAREID)

                    'update history
                    ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Created Event - copied from AR Event ID: " & iPreviousAREID)

                    If ViewState("DefaultBillingTMID") > 0 Then
                        ARGroupModule.InsertAREventApproval(ViewState("AREID"), 1, ViewState("DefaultBillingTMID"), 21, 2)
                    End If

                    Session("RecordCopied") = 1
                    Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddAccountManager_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddAccountManager.SelectedIndexChanged

        Try
            If ddAccountManager.SelectedIndex >= 0 Then
                ViewState("AcctMgrTMID") = ddAccountManager.SelectedValue
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnNotifyAccounting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotifyAccounting.Click


        Try
            ClearMessages()

            NotifyBilling()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageButtons.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Private Sub NotifyBilling()

        GetTeamMemberInfo()

        Dim strEmailToAddress As String = ""
        Dim strEmailCCAddress As String = ""
        Dim strEmailSubject As String = ""
        Dim strEmailBody As String = ""

        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
        Dim strEmailURL As String = strProdOrTestEnvironment & "AR/AR_Event_Detail.aspx?AREID="

        '''''''''''''''''''''''''''''''''''''''''''
        'build list of recipients including backups
        '''''''''''''''''''''''''''''''''''''''''''
        'notify default Accounting Manager
        strEmailToAddress = ViewState("DefaultBillingEmail")

        ''''''''''''''''''''''''''''''''''
        ''need to CC interested billing
        ''''''''''''''''''''''''''''''''''
        If ViewState("BillingEmail") <> "" Then
            strEmailCCAddress = ViewState("BillingEmail")
        End If

        ''''''''''''''''''''''''''''''''''
        ''Build Email
        ''''''''''''''''''''''''''''''''''

        'assign email subject
        strEmailSubject = "Sales updated AR Event ID: " & ViewState("AREID")

        'build email body
        strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been updated by Sales:</font><br /><br />"
        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & ViewState("AREID") & "'>Click here to review</a></font><br /><br />"
        strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtEventDesc.Text.Trim & "</font><br />"

        SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody, True)

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS

    End Sub
    Private Sub PrepareGridViewForExport(ByRef gv As Control)

        'Dim l As Literal = New Literal()
        'Dim i As Integer


        'For i = 0 To gv.Controls.Count

        '    If ((Nothing <> htControls(gv.Controls(i).GetType().Name)) Or (Nothing <> htControls(gv.Controls(i).GetType().BaseType.Name))) Then
        '        l.Text = GetControlPropertyValue(gv.Controls(i))

        '        gv.Controls.Remove(gv.Controls(i))

        '        gv.Controls.AddAt(i, l)

        '    End If

        '    If (gv.Controls(i).HasControls()) Then

        '        PrepareGridViewForExport(gv.Controls(i))

        '    End If

        'Next

    End Sub
    Private Function GetControlPropertyValue(ByVal control As Control) As String
        'Dim controlType As Type = control.[GetType]()
        'Dim strControlType As String = controlType.Name
        Dim strReturn As String = "Error"
        'Dim bReturn As Boolean

        'Dim ctrlProps As System.Reflection.PropertyInfo() = controlType.GetProperties()
        'Dim ExcelPropertyName As String = DirectCast(htControls(strControlType), String)

        'If ExcelPropertyName Is Nothing Then
        '    ExcelPropertyName = DirectCast(htControls(control.[GetType]().BaseType.Name), String)
        '    If ExcelPropertyName Is Nothing Then
        '        Return strReturn
        '    End If
        'End If

        'For Each ctrlProp As System.Reflection.PropertyInfo In ctrlProps

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(String) Then
        '        Try
        '            strReturn = DirectCast(ctrlProp.GetValue(control, Nothing), String)
        '            Exit Try
        '        Catch
        '            strReturn = ""
        '        End Try
        '    End If

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(Boolean) Then
        '        Try
        '            bReturn = CBool(ctrlProp.GetValue(control, Nothing))
        '            strReturn = IIf(bReturn, "True", "False")
        '            Exit Try
        '        Catch
        '            strReturn = "Error"
        '        End Try
        '    End If

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(ListItem) Then
        '        Try
        '            strReturn = DirectCast((ctrlProp.GetValue(control, Nothing)), ListItem).Text
        '            Exit Try
        '        Catch
        '            strReturn = ""
        '        End Try
        '    End If
        'Next
        Return strReturn
    End Function
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Try

            Dim attachment As String = "attachment; filename=AREventDetails.xls"

            Response.ClearContent()

            Response.AddHeader("content-disposition", attachment)

            Response.ContentType = "application/ms-excel"

            Dim sw As StringWriter = New StringWriter()

            Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)

            'EnablePartialRendering = False

            Dim ds As DataSet
            ds = ARGroupModule.GetAREventDetail(ViewState("AREID"))

            If commonFunctions.CheckDataSet(ds) = True Then
                'Dim tempDataGridView As New GridView

                ''tempDataGridView = gvDetail
                'tempDataGridView.PageSize = 5000
                'tempDataGridView.DataBind()

                ''tempDataGridView.AllowPaging = False
                ''tempDataGridView.AllowSorting = False

                'tempDataGridView.HeaderStyle.BackColor = Color.White 'System.Drawing.Color.LightGray
                'tempDataGridView.HeaderStyle.ForeColor = Color.Black 'System.Drawing.Color.Black
                'tempDataGridView.HeaderStyle.Font.Bold = True
                Dim tempDataGridView As New GridView


                tempDataGridView.HeaderStyle.BackColor = Color.White 'System.Drawing.Color.LightGray
                tempDataGridView.HeaderStyle.ForeColor = Color.Black 'System.Drawing.Color.Black
                tempDataGridView.HeaderStyle.Font.Bold = True

                tempDataGridView.AutoGenerateColumns = False

                Dim AREIDColumn As New BoundField
                AREIDColumn.HeaderText = "AREID"
                AREIDColumn.DataField = "AREID"
                tempDataGridView.Columns.Add(AREIDColumn)

                Dim UGNFacilityColumn As New BoundField
                UGNFacilityColumn.HeaderText = "UGN Facility"
                UGNFacilityColumn.DataField = "UGNFacilityName"
                tempDataGridView.Columns.Add(UGNFacilityColumn)

                Dim PARTNOColumn As New BoundField
                PARTNOColumn.HeaderText = "Part No"
                PARTNOColumn.DataField = "PARTNO"
                tempDataGridView.Columns.Add(PARTNOColumn)

                Dim PriceCodeNameColumn As New BoundField
                PriceCodeNameColumn.HeaderText = "Price Code"
                PriceCodeNameColumn.DataField = "PriceCodeName"
                tempDataGridView.Columns.Add(PriceCodeNameColumn)

                ''Dim SOLDTOColumn As New BoundField
                ''SOLDTOColumn.HeaderText = "SOLDTO"
                ''SOLDTOColumn.DataField = "SOLDTO"
                ''tempDataGridView.Columns.Add(SOLDTOColumn)

                ''Dim CABBVColumn As New BoundField
                ''CABBVColumn.HeaderText = "CABBV"
                ''CABBVColumn.DataField = "CABBV"
                ''tempDataGridView.Columns.Add(CABBVColumn)

                Dim USE_RELPRCColumn As New BoundField
                USE_RELPRCColumn.HeaderText = "Current Price"
                USE_RELPRCColumn.DataField = "USE_RELPRC"
                tempDataGridView.Columns.Add(USE_RELPRCColumn)

                Dim PRCDOLRCColumn As New BoundField
                PRCDOLRCColumn.HeaderText = "New Price"
                PRCDOLRCColumn.DataField = "PRCDOLR"
                tempDataGridView.Columns.Add(PRCDOLRCColumn)

                'tempDataGridView.BottomPagerRow.Visible = False
                tempDataGridView.DataSource = ds
                tempDataGridView.DataBind()

                tempDataGridView.RenderControl(htw)

                Response.Write(sw.ToString())

                Response.End()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Function isNewPriceSetForAll() As Boolean

        Dim bReturn As Boolean = True

        Try
            Dim objAREventDetailBLL As New AREventDetailBLL
            Dim dtAREventDetail As DataTable

            Dim iRowCounter As Integer = 0

            dtAREventDetail = objAREventDetailBLL.GetAREventDetail(ViewState("AREID"))
            If commonFunctions.CheckDataTable(dtAREventDetail) = True Then
                For iRowCounter = 0 To dtAREventDetail.Rows.Count - 1
                    If dtAREventDetail.Rows(iRowCounter).Item("PRCDOLR") IsNot System.DBNull.Value Then
                        If dtAREventDetail.Rows(iRowCounter).Item("PRCDOLR") = 0 Then
                            bReturn = False
                        End If
                    End If
                Next
            Else
                bReturn = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return bReturn

    End Function

End Class
