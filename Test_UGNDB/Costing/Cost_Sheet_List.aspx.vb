' ************************************************************************************************
'
' Name:		Cost_Sheet_List.aspx
' Purpose:	This Code Behind is for the main page of the Costing Forms
'
' Date		    Author	   
' 01/19/2009    RCarlson        Created
' 11/19/2009    RCarlson        Modified : added security for Die Layout View only users
' 11/20/2009    RCarlson        Modified : for Admin Users, set default status search to current
' 12/09/2009    RCarlson        Modified : Moved Account Manager Dropdown to main search, adjusted cookied of status for all
' 03/10/2010    RCarlson        Modified : Make Readonly, NonApprovers see only Approved-Current Cost Forms
' 04/27/2010    RCalrson        Modified : CO-2884 - added MaterialID parameter
' 06/24/2010    RCarlson        Modified : CO-2919 - do not let DieLayout ViewOnly Team members see this page, they can just use link in ECI email, use Costing Department List only
' 08/26/2010    RCarlson        Modified : CO-2919 - rolled back
' 11/09/2010    RCarlson        Modified : CO-3023 - RFQ number was not deleted from cookie if empty
' 03/29/2012    RCarlson        Modified : Allow Readonly Team Member to use checkbox to search BOM
' 01/08/2014    LRey            Replaced GetCustomer with GetOEMManufacturer. SOLDTO|CABBV not used in new ERP.
' ************************************************************************************************
Partial Class Cost_Sheet_List
    Inherits System.Web.UI.Page

    Protected WithEvents lnkCostSheetID As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCostSheetStatus As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewDesignLevel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewCustomerPartName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRFDNo As System.Web.UI.WebControls.LinkButton
    Protected Function SetCostFormHyperLink(ByVal CostSheetID As String) As String

        Dim strReturnValue As String = ""

        Try
            If CostSheetID <> "" Then
                strReturnValue = "javascript:void(window.open('Cost_Sheet_Preview.aspx?CostSheetID=" & CostSheetID & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetCostFormHyperLink = strReturnValue

    End Function

    Protected Function SetDieLayoutHyperLink(ByVal CostSheetID As String) As String

        Dim strReturnValue As String = ""

        Try
            If CostSheetID <> "" Then
                strReturnValue = "javascript:void(window.open('Die_Layout_Preview.aspx?CostSheetID=" & CostSheetID & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetDieLayoutHyperLink = strReturnValue

    End Function
    Protected Function SetBackGroundColor(ByVal strDate As String, ByVal RejectedCount As Integer) As String

        Dim strReturnValue As String = "White"

        Try
            If strDate = "" Then
                strReturnValue = "Yellow"
            End If

            If RejectedCount > 0 Then
                strReturnValue = "Red"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetBackGroundColor = strReturnValue

    End Function
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsSubscription As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False

            ViewState("isDieLayoutOnly") = False
            ViewState("TeamMemberID") = 0
            ViewState("SubscriptionID") = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    'If iTeamMemberID = 530 Then
                    '    'iTeamMemberID = 303 'Julie.Sinchak 
                    '    'iTeamMemberID = 32 'Dan Cade
                    '    iTeamMemberID = 736 'Eva.Leach 
                    '    'iTeamMemberID = 691 'Dory Moeller
                    'End If

                    ViewState("TeamMemberID") = iTeamMemberID

                    'Die Layout View
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 2)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 2
                        ViewState("isDieLayoutOnly") = True
                    End If

                    'CST Costing Coordinator
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 41)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 41
                    End If

                    'CST Corporate Engineering
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 42)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 42
                    End If

                    'CST Plant Manager
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 43)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 43
                    End If

                    'CST(Purchasing)
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 44)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 44
                    End If

                    'CST Product Development
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 45)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 45
                    End If

                    'CST Sales
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 46)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 46
                    End If

                    'CST VP of Operations
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 47)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 47
                    End If

                    'Program Management
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 31)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 31
                    End If

                    'Purchasing
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 7)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 7
                    End If

                    'Accounting 21
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 21)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 21
                    End If

                    'Plant Controller 20
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 20)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 20
                    End If

                    'VP Sales 23
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 23)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 23
                    End If

                    'CEO 24
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 24)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 24
                    End If

                    'CFO 33
                    dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 33)
                    If commonFunctions.CheckDataSet(dsSubscription) = True Then
                        ViewState("SubscriptionID") = 33
                    End If

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 57)

                    If commonFunctions.CheckDataSet(dsRoleForm) = True Then

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
                                ViewState("isRestricted") = False
                        End Select
                    End If

                    'If ViewState("isDieLayoutOnly") = True Then
                    '    ViewState("isRestricted") = True
                    'End If

                End If
            End If


            ''''''' DEVELOPER TESTING AS OTHER USER
            'If ViewState("TeamMemberID") = 530 Then
            '    'ViewState("isDieLayoutOnly") = True
            '    'ViewState("SubscriptionID") = 2
            '    'ViewState("TeamMemberID") = 22 ' Terry Turnquist

            '    'ViewState("SubscriptionID") = 45
            '    'ViewState("TeamMemberID") = 433 'Derek Ames
            '    'ViewState("isAdmin") = False

            '    'ViewState("SubscriptionID") = 46
            '    ''ViewState("TeamMemberID") = 246 'Mike Echevarria
            '    'ViewState("TeamMemberID") = 666 'Chris Sleath
            '    'ViewState("isAdmin") = False

            '    'ViewState("SubscriptionID") = 0
            '    'ViewState("TeamMemberID") = 108 ' David Schurke
            '    'ViewState("isAdmin") = False

            '    ViewState("SubscriptionID") = 41
            '    ViewState("TeamMemberID") = 32 'Can Cade
            '    ViewState("isAdmin") = True
            'End If

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
            accAdvancedSearch.Visible = Not ViewState("isRestricted")
            cbBOM.Visible = Not ViewState("isRestricted")
            lblSearchTip.Visible = Not ViewState("isRestricted")
            lblReview1.Visible = Not ViewState("isRestricted")
            lblReview2.Visible = Not ViewState("isRestricted")
            btnAdd.Visible = Not ViewState("isRestricted")

            btnReset.Visible = Not ViewState("isRestricted")
            btnSearch.Visible = Not ViewState("isRestricted")

            cmdFirst.Visible = Not ViewState("isRestricted")
            cmdNext.Visible = Not ViewState("isRestricted")
            txtGoToPage.Visible = Not ViewState("isRestricted")
            cmdGo.Visible = Not ViewState("isRestricted")
            cmdPrev.Visible = Not ViewState("isRestricted")
            cmdLast.Visible = Not ViewState("isRestricted")

            lblCurrentPage.Visible = Not ViewState("isRestricted")
            rpCostSheetInfo.Visible = Not ViewState("isRestricted")

            lblSearchAccountManagerLabel.Visible = Not ViewState("isRestricted")
            ddSearchAccountManagerValue.Visible = Not ViewState("isRestricted")

            lblSearchApprovedByTeamMemberLabel.Visible = Not ViewState("isRestricted")
            ddSearchApprovedByTeamMemberValue.Visible = Not ViewState("isRestricted")

            lblSearchPartNoLabel.Visible = Not ViewState("isRestricted")
            txtSearchPartNoValue.Visible = Not ViewState("isRestricted")

            lblSearchCustomerLabel.Visible = Not ViewState("isRestricted")
            ddSearchCustomerValue.Visible = Not ViewState("isRestricted")

            lblSearchCostSheetIDLabel.Visible = Not ViewState("isRestricted")
            txtSearchCostSheetIDValue.Visible = Not ViewState("isRestricted")

            lblSearchCostSheetStatusLabel.Visible = Not ViewState("isRestricted")
            ddSearchCostSheetStatusValue.Visible = Not ViewState("isRestricted")

            lblSearchCustomerPartNoLabel.Visible = Not ViewState("isRestricted")
            txtSearchCustomerPartNoValue.Visible = Not ViewState("isRestricted")

            lblSearchDepartmentLabel.Visible = Not ViewState("isRestricted")
            ddSearchDepartmentValue.Visible = Not ViewState("isRestricted")

            lblSearchDesignLevelLabel.Visible = Not ViewState("isRestricted")
            txtSearchDesignLevelValue.Visible = Not ViewState("isRestricted")

            lblSearchDrawingNoLabel.Visible = Not ViewState("isRestricted")
            txtSearchDrawingNoValue.Visible = Not ViewState("isRestricted")

            lblSearchFormulaLabel.Visible = Not ViewState("isRestricted")
            ddSearchFormulaValue.Visible = Not ViewState("isRestricted")

            lblSearchPartNameLabel.Visible = Not ViewState("isRestricted")
            txtSearchPartNameValue.Visible = Not ViewState("isRestricted")

            lblSearchCommodityLabel.Visible = Not ViewState("isRestricted")
            ddSearchCommodityValue.Visible = Not ViewState("isRestricted")

            lblSearchProgramLabel.Visible = Not ViewState("isRestricted")
            ddSearchProgramValue.Visible = Not ViewState("isRestricted")

            lblSearchRFDNoLabel.Visible = Not ViewState("isRestricted")
            txtSearchRFDNoValue.Visible = Not ViewState("isRestricted")

            lblSearchUGNFacilityLabel.Visible = Not ViewState("isRestricted")
            ddSearchUGNFacilityValue.Visible = Not ViewState("isRestricted")

            lblSearchWaitingForTeamMemberApprovalLabel.Visible = Not ViewState("isRestricted")
            ddSearchWaitingForTeamMemberApprovalValue.Visible = Not ViewState("isRestricted")

            lblSearchYearLabel.Visible = Not ViewState("isRestricted")
            ddSearchYearValue.Visible = Not ViewState("isRestricted")

            lblSearchApprovedLabel.Visible = Not ViewState("isRestricted")
            ddSearchApprovedValue.Visible = Not ViewState("isRestricted")

            lblSearchMaterialIDLabel.Visible = Not ViewState("isRestricted")
            txtSearchMaterialIDValue.Visible = Not ViewState("isRestricted")

            emTip.Visible = Not ViewState("isRestricted")

            '2012-Mar-28 - allow read only users to search bom
            cbBOM.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then

                'cbBOM.Visible = ViewState("isAdmin")
                btnAdd.Enabled = ViewState("isAdmin")

                'if readonly users with NO subcriptions/Not an approver, then do not show pending approval dropdown
                If ViewState("SubscriptionID") = 41 Or ViewState("SubscriptionID") = 42 Or ViewState("SubscriptionID") = 43 Or ViewState("SubscriptionID") = 44 Or ViewState("SubscriptionID") = 45 Or ViewState("SubscriptionID") = 46 Or ViewState("SubscriptionID") = 47 Or ViewState("isAdmin") Then
                    lblSearchWaitingForTeamMemberApprovalLabel.Visible = True
                    ddSearchWaitingForTeamMemberApprovalValue.Visible = True
                Else
                    lblSearchWaitingForTeamMemberApprovalLabel.Visible = False
                    ddSearchWaitingForTeamMemberApprovalValue.Visible = False

                    ddSearchCostSheetStatusValue.Visible = False
                    lblSearchCostSheetStatusLabel.Visible = False
                End If

                lblSearchApprovedLabel.Visible = ViewState("isAdmin")
                ddSearchApprovedValue.Visible = ViewState("isAdmin")

            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
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
    Public Property CurrentPage() As Integer

        Get
            ' look for current page in ViewState
            Dim o As Object = ViewState("_CurrentPage")
            If (o Is Nothing) Then
                Return 0 ' default page index of 0
            Else
                Return o
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("_CurrentPage") = value
        End Set

    End Property

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ''bind existing data to drop down Program Control for selection criteria for search
            ds = commonFunctions.GetProgram("", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProgramValue.DataSource = ds
                ddSearchProgramValue.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddSearchProgramValue.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddSearchProgramValue.DataBind()
                ddSearchProgramValue.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Year control for selection criteria for search
            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchYearValue.DataSource = ds
                ddSearchYearValue.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddSearchYearValue.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddSearchYearValue.DataBind()
                ddSearchYearValue.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchUGNFacilityValue.DataSource = ds
                ddSearchUGNFacilityValue.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddSearchUGNFacilityValue.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddSearchUGNFacilityValue.DataBind()
                ddSearchUGNFacilityValue.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchCustomerValue.DataSource = ds
                ddSearchCustomerValue.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddSearchCustomerValue.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddSearchCustomerValue.DataBind()
                ddSearchCustomerValue.Items.Insert(0, "")
            End If

            'bind existing team member list for Account Managers who created cost sheets
            ds = CostingModule.GetCostSheetAccountManagers()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchAccountManagerValue.DataSource = ds
                ddSearchAccountManagerValue.DataTextField = ds.Tables(0).Columns("ddAccountManagerFullName").ColumnName
                ddSearchAccountManagerValue.DataValueField = ds.Tables(0).Columns("AccountManagerID").ColumnName
                ddSearchAccountManagerValue.DataBind()
                ddSearchAccountManagerValue.Items.Insert(0, "")
            End If

            'bind existing data to drop down Department control for selection criteria for search
            'ds = commonFunctions.GetDepartment("", "", False)
            ds = CostingModule.GetCostingDepartmentList("", "", False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchDepartmentValue.DataSource = ds
                ddSearchDepartmentValue.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
                ddSearchDepartmentValue.DataValueField = ds.Tables(0).Columns("DepartmentID").ColumnName
                ddSearchDepartmentValue.DataBind()
                ddSearchDepartmentValue.Items.Insert(0, "")
            End If

            'bind existing data to Formula DropDown for selection criteria for search
            'ds = CostingModule.GetFormula(0, "", "", "", "", 0, 0, 0)
            ds = CostingModule.GetFormula(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchFormulaValue.DataSource = ds
                ddSearchFormulaValue.DataTextField = ds.Tables(0).Columns("ddFormulaName").ColumnName
                ddSearchFormulaValue.DataValueField = ds.Tables(0).Columns("FormulaID").ColumnName
                ddSearchFormulaValue.DataBind()
                ddSearchFormulaValue.Items.Insert(0, "")
            End If

            'bind existing team member list for Team Members who need to approve cost sheets still
            ds = CostingModule.GetCostSheetWaitingForTeamMemberApprovals()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchWaitingForTeamMemberApprovalValue.DataSource = ds
                ddSearchWaitingForTeamMemberApprovalValue.DataTextField = ds.Tables(0).Columns("ddTeamMemberFullName").ColumnName
                ddSearchWaitingForTeamMemberApprovalValue.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddSearchWaitingForTeamMemberApprovalValue.DataBind()
                ddSearchWaitingForTeamMemberApprovalValue.Items.Insert(0, "")
            End If

            'bind existing team member list for Team Members who approved cost sheets
            ds = CostingModule.GetCostSheetApprovedByTeamMembers()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchApprovedByTeamMemberValue.DataSource = ds
                ddSearchApprovedByTeamMemberValue.DataTextField = ds.Tables(0).Columns("ddTeamMemberFullName").ColumnName
                ddSearchApprovedByTeamMemberValue.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddSearchApprovedByTeamMemberValue.DataBind()
                ddSearchApprovedByTeamMemberValue.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Commodity control for selection criteria 
            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchCommodityValue.DataSource = ds
                ddSearchCommodityValue.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddSearchCommodityValue.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddSearchCommodityValue.DataBind()
                ddSearchCommodityValue.Items.Insert(0, "")
                ddSearchCommodityValue.SelectedIndex = 0
            End If


            ' ''bind existing data to drop down PartFamily control for selection criteria for search
            'ds = commonFunctions.GetSubFamily(0)
            'If ds IsNot Nothing Then
            '    If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
            '        ddSubFamily.DataSource = ds
            '        ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
            '        ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
            '        ddSubFamily.DataBind()
            '        ddSubFamily.Items.Insert(0, "")
            '    End If
            'End If


            ''bind existing data to drop down Density control for selection criteria for search
            'ds = commonFunctions.GetPurchasedGood("")
            'If ds IsNot Nothing Then
            '    If (ds.Tables.Count > 0 And ds.Tables.Item(0).Rows.Count > 0) Then
            '        ddPurchasedGood.DataSource = ds
            '        ddPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
            '        ddPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
            '        ddPurchasedGood.DataBind()
            '        ddPurchasedGood.Items.Insert(0, "")
            '    End If
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub
    Private Sub BindData()

        Try

            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = CostingModule.GetCostSheetSearch(ViewState("CostSheetID"), ViewState("CostSheetStatus"), _
            ViewState("AccountManagerID"), ViewState("DepartmentID"), ViewState("FormulaID"), _
            ViewState("DrawingNo"), ViewState("PartNo"), ViewState("CustomerPartNo"), ViewState("DesignLevel"), _
            ViewState("PartName"), ViewState("RFDNo"), ViewState("ProgramID"), ViewState("CommodityID"), ViewState("Year"), _
            ViewState("Customer"), ViewState("UGNFacility"), ViewState("WaitingForTeamMemberApproval"), _
            ViewState("ApprovedByTeamMember"), ViewState("isAdmin"), ViewState("SubscriptionID"), _
            ViewState("filterApproved"), ViewState("isApproved"), ViewState("checkBOM"), ViewState("MaterialID"), ViewState("QuickQuote"))

            If ViewState("isRestricted") = False Then
                If commonFunctions.CheckDataSet(ds) = True Then
                    rpCostSheetInfo.DataSource = ds
                    rpCostSheetInfo.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 15

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpCostSheetInfo.DataSource = objPds
                    rpCostSheetInfo.DataBind()

                    lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                    ViewState("LastPageCount") = objPds.PageCount - 1
                    txtGoToPage.Text = CurrentPage + 1

                    ' Disable Prev or Next buttons if necessary
                    cmdFirst.Enabled = Not objPds.IsFirstPage
                    cmdPrev.Enabled = Not objPds.IsFirstPage
                    cmdNext.Enabled = Not objPds.IsLastPage
                    cmdLast.Enabled = Not objPds.IsLastPage
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

        lblMessageBottom.Text = lblMessage.Text
    End Sub
    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = CostingModule.GetCostSheetSearch(ViewState("CostSheetID"), ViewState("CostSheetStatus"), _
            ViewState("AccountManagerID"), ViewState("DepartmentID"), ViewState("FormulaID"), _
            ViewState("DrawingNo"), ViewState("PartNo"), ViewState("CustomerPartNo"), ViewState("DesignLevel"), _
            ViewState("PartName"), ViewState("RFDNo"), ViewState("ProgramID"), ViewState("CommodityID"), ViewState("Year"), _
            ViewState("Customer"), ViewState("UGNFacility"), ViewState("WaitingForTeamMemberApproval"), _
            ViewState("ApprovedByTeamMember"), ViewState("isAdmin"), ViewState("SubscriptionID"), _
            ViewState("filterApproved"), ViewState("isApproved"), ViewState("checkBOM"), ViewState("MaterialID"), ViewState("QuickQuote"))

            If commonFunctions.CheckDataSet(ds) = True Then

                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpCostSheetInfo.DataSource = dv
                rpCostSheetInfo.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text
    End Sub
    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
       Handles lnkCostSheetID.Click, lnkCostSheetStatus.Click, lnkNewCustomerPartName.Click, lnkPartNo.Click, lnkNewDesignLevel.Click, lnkRFDNo.Click

        Try
            Dim lnkButton As New LinkButton
            lnkButton = CType(sender, LinkButton)
            Dim order As String = lnkButton.CommandArgument
            Dim sortType As String = ViewState(lnkButton.ID)

            SortByColumn(order + " " + sortType)
            If sortType = "ASC" Then
                'if column set to ascending sort, change to descending for next click on that column
                lnkButton.CommandName = "DESC"
                ViewState(lnkButton.ID) = "DESC"
            Else
                lnkButton.CommandName = "ASC"
                ViewState(lnkButton.ID) = "ASC"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search for Cost Sheet"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Costing </b> > Cost Sheet Search "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            CheckRights()

            If HttpContext.Current.Session("sessionCostingCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionCostingCurrentPage")
            End If

            'clear crystal reports
            CostingModule.CleanCostingCrystalReports()

            If Not Page.IsPostBack Then

                ViewState("lnkCostSheetID") = "DESC"
                ViewState("lnkCostSheetStatus") = "ASC"
                ViewState("lnkNewDesignLevel") = "ASC"
                ViewState("lnkOriginalDesignLevel") = "ASC"
                ViewState("lnkNewCustomerPartName") = "ASC"
                ViewState("lnkNewCustomerPartNo") = "ASC"
                ViewState("lnkRFDNo") = "ASC"
                ViewState("lnkPartNo") = "ASC"

                ViewState("CostSheetID") = ""
                ViewState("NewDesignLevel") = ""
                ViewState("OriginalDesignLevel") = ""
                ViewState("NewCustomerPartName") = ""
                ViewState("lnkNewCustomerPartNo") = ""
                ViewState("RFDNo") = 0
                ViewState("CostSheetStatus") = ""
                ViewState("AccountManagerID") = 0
                ViewState("DepartmentID") = 0
                ViewState("FormulaID") = 0
                ViewState("DrawingNo") = ""
                ViewState("PartNo") = ""
                ViewState("CustomerPartNo") = ""
                ViewState("DesignLevel") = ""
                ViewState("PartName") = ""
                ViewState("RFDNo") = 0
                ViewState("ProgramID") = 0
                ViewState("CommodityID") = 0
                ViewState("Year") = 0
                ViewState("CustomerValue") = ""
                ViewState("UGNFacility") = ""
                ViewState("WaitingForTeamMemberApproval") = 0
                ViewState("ApprovedByTeamMember") = 0
                ViewState("filterApproved") = 0
                ViewState("isApproved") = 0
                ViewState("checkBOM") = 0
                ViewState("MaterialID") = ""
                ViewState("QuickQuote") = 0

                '' ''******
                '' '' Bind drop down lists
                '' ''******
                BindCriteria()

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                    txtSearchCostSheetIDValue.Text = HttpContext.Current.Request.QueryString("CostSheetID")
                    ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")
                Else
                    If Not Request.Cookies("CostingModule_SaveCostSheetIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveCostSheetIDSearch").Value) <> "" Then
                            txtSearchCostSheetIDValue.Text = Request.Cookies("CostingModule_SaveCostSheetIDSearch").Value
                            ViewState("CostSheetID") = Request.Cookies("CostingModule_SaveCostSheetIDSearch").Value
                        End If
                    End If
                End If

                'If ViewState("isAdmin") = True Then
                ddSearchCostSheetStatusValue.SelectedValue = "Current"
                ViewState("CostSheetStatus") = "Current"
                'End If

                If HttpContext.Current.Request.QueryString("CostSheetStatus") <> "" Then
                    ddSearchCostSheetStatusValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("CostSheetStatus"))
                    ViewState("CostSheetStatus") = Server.UrlDecode(HttpContext.Current.Request.QueryString("CostSheetStatus"))
                Else
                    If Not Request.Cookies("CostingModule_SaveCostSheetStatusSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveCostSheetStatusSearch").Value) <> "" Then
                            ddSearchCostSheetStatusValue.SelectedValue = Request.Cookies("CostingModule_SaveCostSheetStatusSearch").Value
                            ViewState("CostSheetStatus") = Request.Cookies("CostingModule_SaveCostSheetStatusSearch").Value
                        End If
                    End If
                End If

                If ViewState("CostSheetStatus") = "All" Then
                    ddSearchCostSheetStatusValue.SelectedValue = "All"
                    ViewState("CostSheetStatus") = ""
                End If

                'if the user is an account manager, default the search to the account manager
                'if the account manager selects a different account manager or NO account manager, then make the change to the search parameter                
                If HttpContext.Current.Request.QueryString("AccountManagerID") <> "" Then
                    ddSearchAccountManagerValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("AccountManagerID"))
                    ViewState("AccountManagerID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("AccountManagerID"))
                Else
                    If ViewState("SubscriptionID") = 46 Then
                        ViewState("AccountManagerID") = ViewState("TeamMemberID")
                        ddSearchAccountManagerValue.SelectedValue = ViewState("AccountManagerID")

                        If Not Request.Cookies("CostingModule_SaveAccountManagerWantsAllSearch") Is Nothing Then
                            If Trim(Request.Cookies("CostingModule_SaveAccountManagerWantsAllSearch").Value) <> "" Then
                                If Trim(Request.Cookies("CostingModule_SaveAccountManagerWantsAllSearch").Value) = "ALL" Then
                                    ViewState("AccountManagerID") = 0
                                    ddSearchAccountManagerValue.SelectedIndex = -1
                                End If
                            End If
                        End If
                    End If

                    If Not Request.Cookies("CostingModule_SaveAccountManagerIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveAccountManagerIDSearch").Value) <> "" Then
                            ddSearchAccountManagerValue.SelectedValue = Request.Cookies("CostingModule_SaveAccountManagerIDSearch").Value
                            ViewState("AccountManagerID") = Request.Cookies("CostingModule_SaveAccountManagerIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DepartmentID") <> "" Then
                    ddSearchDepartmentValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("DepartmentID"))
                    ViewState("DepartmentID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("DepartmentID"))
                Else
                    If Not Request.Cookies("CostingModule_SaveDepartmentIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveDepartmentIDSearch").Value) <> "" Then
                            ddSearchDepartmentValue.SelectedValue = Request.Cookies("CostingModule_SaveDepartmentIDSearch").Value
                            ViewState("DepartmentID") = Request.Cookies("CostingModule_SaveDepartmentIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("FormulaID") <> "" Then
                    ddSearchFormulaValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("FormulaID"))
                    ViewState("FormulaID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("FormulaID"))
                Else
                    If Not Request.Cookies("CostingModule_SaveFormulaIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveFormulaIDSearch").Value) <> "" Then
                            ddSearchFormulaValue.SelectedValue = Request.Cookies("CostingModule_SaveFormulaIDSearch").Value
                            ViewState("FormulaID") = Request.Cookies("CostingModule_SaveFormulaIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtSearchDrawingNoValue.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                Else
                    If Not Request.Cookies("CostingModule_SaveDrawingNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveDrawingNoSearch").Value) <> "" Then
                            txtSearchDrawingNoValue.Text = Request.Cookies("CostingModule_SaveDrawingNoSearch").Value
                            ViewState("DrawingNo") = Request.Cookies("CostingModule_SaveDrawingNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtSearchPartNoValue.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                Else
                    If Not Request.Cookies("CostingModule_SavePartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SavePartNoSearch").Value) <> "" Then
                            txtSearchPartNoValue.Text = Request.Cookies("CostingModule_SavePartNoSearch").Value
                            ViewState("PartNo") = Request.Cookies("CostingModule_SavePartNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CustomerPartNo") <> "" Then
                    txtSearchCustomerPartNoValue.Text = HttpContext.Current.Request.QueryString("CustomerPartNo")
                    ViewState("CustomerPartNo") = HttpContext.Current.Request.QueryString("CustomerPartNo")
                Else
                    If Not Request.Cookies("CostingModule_SaveCustomerPartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveCustomerPartNoSearch").Value) <> "" Then
                            txtSearchCustomerPartNoValue.Text = Request.Cookies("CostingModule_SaveCustomerPartNoSearch").Value
                            ViewState("CustomerPartNo") = Request.Cookies("CostingModule_SaveCustomerPartNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DesignLevel") <> "" Then
                    txtSearchDesignLevelValue.Text = HttpContext.Current.Request.QueryString("DesignLevel")
                    ViewState("DesignLevel") = HttpContext.Current.Request.QueryString("DesignLevel")
                Else
                    If Not Request.Cookies("CostingModule_SaveDesignLevelSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveDesignLevelSearch").Value) <> "" Then
                            txtSearchDesignLevelValue.Text = Request.Cookies("CostingModule_SaveDesignLevelSearch").Value
                            ViewState("DesignLevel") = Request.Cookies("CostingModule_SaveDesignLevelSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtSearchPartNameValue.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                Else
                    If Not Request.Cookies("CostingModule_SavePartNameSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SavePartNameSearch").Value) <> "" Then
                            txtSearchPartNameValue.Text = Request.Cookies("CostingModule_SavePartNameSearch").Value
                            ViewState("PartName") = Request.Cookies("CostingModule_SavePartNameSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then
                    txtSearchRFDNoValue.Text = Server.UrlDecode(HttpContext.Current.Request.QueryString("RFDNo"))
                    ViewState("RFDNo") = Server.UrlDecode(HttpContext.Current.Request.QueryString("RFDNO"))
                Else
                    If Not Request.Cookies("CostingModule_SaveRFDNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveRFDNoSearch").Value) <> "" Then
                            txtSearchRFDNoValue.Text = Request.Cookies("CostingModule_SaveRFDNoSearch").Value
                            ViewState("RFDNo") = Request.Cookies("CostingModule_SaveRFDNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ProgramID") <> "" Then
                    ddSearchProgramValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProgramID"))
                    ViewState("ProgramID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProgramID"))
                Else
                    If Not Request.Cookies("CostingModule_SaveProgramIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveProgramIDSearch").Value) <> "" Then
                            ddSearchProgramValue.SelectedValue = Request.Cookies("CostingModule_SaveProgramIDSearch").Value
                            ViewState("ProgramID") = Request.Cookies("CostingModule_SaveProgramIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CommodityID") <> "" Then
                    ddSearchCommodityValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("CommodityID"))
                    ViewState("CommodityID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("CommodityID"))
                Else
                    If Not Request.Cookies("CostingModule_SaveCommodityIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveCommodityIDSearch").Value) <> "" Then
                            ddSearchCommodityValue.SelectedValue = Request.Cookies("CostingModule_SaveCommodityIDSearch").Value
                            ViewState("CommodityID") = Request.Cookies("CostingModule_SaveCommodityIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("Year") <> "" Then
                    ddSearchYearValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("Year"))
                    ViewState("Year") = Server.UrlDecode(HttpContext.Current.Request.QueryString("Year"))
                Else
                    If Not Request.Cookies("CostingModule_SaveYearSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveYearSearch").Value) <> "" Then
                            ddSearchYearValue.SelectedValue = Request.Cookies("CostingModule_SaveYearSearch").Value
                            ViewState("Year") = Request.Cookies("CostingModule_SaveYearSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CustomerValue") <> "" Then
                    ddSearchCustomerValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("CustomerValue"))
                    ViewState("CustomerValue") = Server.UrlDecode(HttpContext.Current.Request.QueryString("CustomerValue"))

                Else
                    If Not Request.Cookies("CostingModule_SaveCustomerSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveCustomerSearch").Value) <> "" Then
                            ddSearchCustomerValue.SelectedValue = Request.Cookies("CostingModule_SaveCustomerSearch").Value
                            ViewState("CustomerValue") = Request.Cookies("CostingModule_SaveCustomerSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddSearchUGNFacilityValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNFacility"))
                    ViewState("UGNFacility") = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNFacility"))
                Else
                    If Not Request.Cookies("CostingModule_SaveUGNFacilitySearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveUGNFacilitySearch").Value) <> "" Then
                            ddSearchUGNFacilityValue.SelectedValue = Request.Cookies("CostingModule_SaveUGNFacilitySearch").Value
                            ViewState("UGNFacility") = Request.Cookies("CostingModule_SaveUGNFacilitySearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("WaitingForTeamMemberApproval") <> "" Then
                    ddSearchWaitingForTeamMemberApprovalValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("WaitingForTeamMemberApproval"))
                    ViewState("WaitingForTeamMemberApproval") = Server.UrlDecode(HttpContext.Current.Request.QueryString("WaitingForTeamMemberApproval"))
                Else
                    If Not Request.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch").Value) <> "" Then
                            ddSearchWaitingForTeamMemberApprovalValue.SelectedValue = Request.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch").Value
                            ViewState("WaitingForTeamMemberApproval") = Request.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ApprovedByTeamMember") <> "" Then
                    ddSearchApprovedByTeamMemberValue.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("ApprovedByTeamMember"))
                    ViewState("ApprovedByTeamMember") = Server.UrlDecode(HttpContext.Current.Request.QueryString("ApprovedByTeamMember"))
                Else
                    If Not Request.Cookies("CostingModule_SaveApprovedByTeamMemberSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveApprovedByTeamMemberSearch").Value) <> "" Then
                            ddSearchApprovedByTeamMemberValue.SelectedValue = Request.Cookies("CostingModule_SaveApprovedByTeamMemberSearch").Value
                            ViewState("ApprovedByTeamMember") = Request.Cookies("CostingModule_SaveApprovedByTeamMemberSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("filterApproved") <> "" Then
                    ViewState("filterApproved") = CType(Server.UrlDecode(HttpContext.Current.Request.QueryString("filterApproved")), Integer)
                Else
                    If Not Request.Cookies("CostingModule_SaveFilterApprovedSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveFilterApprovedSearch").Value) <> "" Then
                            ViewState("filterApproved") = CType(Request.Cookies("CostingModule_SaveFilterApprovedSearch").Value, Integer)
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("isApproved") <> "" Then
                    ViewState("isApproved") = CType(Server.UrlDecode(HttpContext.Current.Request.QueryString("isApproved")), Integer)
                Else
                    If Not Request.Cookies("CostingModule_SaveIsApprovedSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveIsApprovedSearch").Value) <> "" Then
                            ViewState("isApproved") = CType(Request.Cookies("CostingModule_SaveIsApprovedSearch").Value, Integer)
                        End If
                    End If
                End If

                If ViewState("filterApproved") > 0 And ViewState("isApproved") > 0 Then
                    ddSearchApprovedValue.SelectedValue = "Approved"
                End If

                If ViewState("filterApproved") > 0 And ViewState("isApproved") = 0 Then
                    ddSearchApprovedValue.SelectedValue = "Pending"
                End If

                If HttpContext.Current.Request.QueryString("checkBOM") <> "" Then
                    ViewState("checkBOM") = CType(Server.UrlDecode(HttpContext.Current.Request.QueryString("checkBOM")), Integer)
                    cbBOM.Checked = ViewState("checkBOM")
                Else
                    If Not Request.Cookies("CostingModule_SaveCheckBOMSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveCheckBOMSearch").Value) <> "" Then
                            ViewState("checkBOM") = CType(Request.Cookies("CostingModule_SaveCheckBOMSearch").Value, Integer)
                            cbBOM.Checked = ViewState("checkBOM")
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("MaterialID") <> "" Then
                    txtSearchMaterialIDValue.Text = HttpContext.Current.Request.QueryString("MaterialID")
                    ViewState("MaterialID") = HttpContext.Current.Request.QueryString("MaterialID")
                Else
                    If Not Request.Cookies("CostingModule_SaveListMaterialIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveListMaterialIDSearch").Value) <> "" Then
                            txtSearchMaterialIDValue.Text = Request.Cookies("CostingModule_SaveListMaterialIDSearch").Value
                            ViewState("MaterialID") = Request.Cookies("CostingModule_SaveListMaterialIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("QuickQuote") <> "" Then
                    ViewState("QuickQuote") = CType(Server.UrlDecode(HttpContext.Current.Request.QueryString("QuickQuote")), Integer)
                    cbQuickQuote.Checked = ViewState("QuickQuote")
                Else
                    If Not Request.Cookies("CostingModule_SaveCheckQuickQuoteSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveCheckQuickQuoteSearch").Value) <> "" Then
                            ViewState("checkQuickQuote") = CType(Request.Cookies("CostingModule_SaveCheckQuickQuoteSearch").Value, Integer)
                            cbQuickQuote.Checked = ViewState("checkQuickQuote")
                        End If
                    End If
                End If

                'load repeater control
                BindData()

                'handle if accordion should be opened or closed - default to closed
                If Request.Cookies("UGNDB_ShowCostingAdvancedSearch") IsNot Nothing Then
                    If Request.Cookies("UGNDB_ShowCostingAdvancedSearch").Value.Trim <> "" Then
                        If CType(Request.Cookies("UGNDB_ShowCostingAdvancedSearch").Value, Integer) = 1 Then
                            accAdvancedSearch.SelectedIndex = 0
                            cbShowAdvancedSearch.Checked = True
                        Else
                            accAdvancedSearch.SelectedIndex = -1
                            cbShowAdvancedSearch.Checked = False
                        End If
                    End If

                Else
                    accAdvancedSearch.SelectedIndex = -1
                    cbShowAdvancedSearch.Checked = False
                End If

                If ViewState("ProgramID") > 0 _
                    Or ViewState("CommodityID") > 0 _
                    Or ViewState("Year") > 0 _
                    Or ViewState("DepartmentID") > 0 _
                    Or ViewState("FormulaID") > 0 _
                    Or ViewState("UGNFacility") <> "" _
                    Or ViewState("CustomerValue") <> "" Then

                    accAdvancedSearch.SelectedIndex = 0
                    cbShowAdvancedSearch.Checked = True
                End If

            Else
                If txtSearchCostSheetIDValue.Text.Length > 0 Then
                    ViewState("CostSheetID") = txtSearchCostSheetIDValue.Text.Trim
                Else
                    ViewState("CostSheetID") = ""
                End If

                If ddSearchCostSheetStatusValue.SelectedIndex > 0 Then
                    ViewState("CostSheetStatus") = ddSearchCostSheetStatusValue.SelectedValue
                End If

                If ddSearchCostSheetStatusValue.SelectedValue = "All" Then
                    ViewState("CostSheetStatus") = ""
                End If

                If ddSearchAccountManagerValue.SelectedIndex > 0 Then
                    ViewState("AccountManagerID") = ddSearchAccountManagerValue.SelectedValue
                Else
                    ViewState("AccountManagerID") = 0
                End If

                If ddSearchDepartmentValue.SelectedIndex > 0 Then
                    ViewState("DepartmentID") = ddSearchDepartmentValue.SelectedValue
                Else
                    ViewState("DepartmentID") = 0
                End If

                If ddSearchFormulaValue.SelectedIndex > 0 Then
                    ViewState("FormulaID") = ddSearchFormulaValue.SelectedValue
                Else
                    ViewState("FormulaID") = 0
                End If

                ViewState("DrawingNo") = txtSearchDrawingNoValue.Text.Trim
                ViewState("PartNo") = txtSearchPartNoValue.Text.Trim
                ViewState("CustomerPartNo") = txtSearchCustomerPartNoValue.Text.Trim
                ViewState("DesignLevel") = txtSearchDesignLevelValue.Text.Trim
                ViewState("PartName") = txtSearchPartNameValue.Text.Trim

                If txtSearchRFDNoValue.Text.Length > 0 Then
                    ViewState("RFDNo") = txtSearchRFDNoValue.Text.Trim
                Else
                    ViewState("RFDNo") = ""
                End If

                If ddSearchProgramValue.SelectedIndex > 0 Then
                    ViewState("ProgramID") = ddSearchProgramValue.SelectedValue
                Else
                    ViewState("ProgramID") = 0
                End If

                If ddSearchCommodityValue.SelectedIndex > 0 Then
                    ViewState("CommodityID") = ddSearchCommodityValue.SelectedValue
                Else
                    ViewState("CommodityID") = 0
                End If

                If ddSearchYearValue.SelectedIndex > 0 Then
                    ViewState("Year") = ddSearchYearValue.SelectedValue
                Else
                    ViewState("Year") = 0
                End If

                If ddSearchCustomerValue.SelectedIndex > 0 Then
                    ViewState("CustomerValue") = ddSearchCustomerValue.SelectedValue
                Else
                    ViewState("CustomerValue") = ""
                End If

                If ddSearchUGNFacilityValue.SelectedIndex > 0 Then
                    ViewState("UGNFacility") = ddSearchUGNFacilityValue.SelectedValue
                Else
                    ViewState("UGNFacility") = ""
                End If

                If ddSearchWaitingForTeamMemberApprovalValue.SelectedIndex > 0 Then
                    ViewState("WaitingForTeamMemberApproval") = ddSearchWaitingForTeamMemberApprovalValue.SelectedValue
                Else
                    ViewState("WaitingForTeamMemberApproval") = 0
                End If

                If ddSearchApprovedByTeamMemberValue.SelectedIndex > 0 Then
                    ViewState("ApprovedByTeamMember") = ddSearchApprovedByTeamMemberValue.SelectedValue
                Else
                    ViewState("ApprovedByTeamMember") = 0
                End If

                ViewState("isApproved") = 0
                ViewState("filterApproved") = 0

                If ddSearchApprovedValue.SelectedIndex > 0 Then
                    If ddSearchApprovedValue.SelectedValue = "Approved" Then
                        ViewState("isApproved") = 1
                        ViewState("filterApproved") = 1
                    End If

                    If ddSearchApprovedValue.SelectedValue = "Pending" Then
                        ViewState("isApproved") = 0
                        ViewState("filterApproved") = 1
                    End If
                End If

                If cbBOM.Checked = True Then
                    ViewState("checkBOM") = 1
                Else
                    ViewState("checkBOM") = 0
                End If
            End If

            If cbQuickQuote.Checked = True Then
                ViewState("QuickQuote") = 1
            Else
                ViewState("QuickQuote") = 0
            End If

            If txtSearchMaterialIDValue.Text.Length > 0 Then
                ViewState("MaterialID") = txtSearchMaterialIDValue.Text.Trim
            Else
                ViewState("MaterialID") = ""
            End If

            EnableControls()

            'focus on CostSheet ID field
            txtSearchCostSheetIDValue.Focus()

            If HttpContext.Current.Session("DeletedCostSheet") IsNot Nothing Then
                If HttpContext.Current.Session("DeletedCostSheet") <> "" Then
                    lblMessage.Text = "The Cost sheet " & HttpContext.Current.Session("DeletedCostSheet") & " was deleted."
                    HttpContext.Current.Session("DeletedCostSheet") = Nothing
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

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        lblMessage.Text = ""

        Try
            HttpContext.Current.Session("sessionCostingCurrentPage") = Nothing

            If ddSearchWaitingForTeamMemberApprovalValue.SelectedIndex > 0 And ddSearchApprovedByTeamMemberValue.SelectedIndex > 0 Then
                lblMessage.Text = "FAILED SEARCH: Searching for team members waiting to approve and searching for team members who already approved are mutually exlcusive. Please change your search criteria."
                'ddSearchWaitingForTeamMemberApprovalValue.SelectedIndex = 0
                'ddSearchApprovedByTeamMemberValue.SelectedIndex = 0
            Else
                'set saved value of what criteria was used to search   
                If txtSearchCostSheetIDValue.Text.Trim <> "" Then
                    Response.Cookies("CostingModule_SaveCostSheetIDSearch").Value = Replace(txtSearchCostSheetIDValue.Text.Trim, "'", "")

                Else
                    Response.Cookies("CostingModule_SaveCostSheetIDSearch").Value = ""
                    Response.Cookies("CostingModule_SaveCostSheetIDSearch").Expires = DateTime.Now.AddDays(-1)
                End If
                txtSearchCostSheetIDValue.Text = Replace(txtSearchCostSheetIDValue.Text.Trim, "'", "")

                If ddSearchCostSheetStatusValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveCostSheetStatusSearch").Value = ddSearchCostSheetStatusValue.SelectedValue
                Else
                    'Response.Cookies("CostingModule_SaveCostSheetStatusSearch").Value = ""
                    Response.Cookies("CostingModule_SaveCostSheetStatusSearch").Value = "All"
                    'Response.Cookies("CostingModule_SaveCostSheetStatusSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchAccountManagerValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveAccountManagerIDSearch").Value = ddSearchAccountManagerValue.SelectedValue

                    'Account Manager just wants unique account manager
                    Response.Cookies("CostingModule_SaveAccountManagerWantsAllSearch").Value = 0
                    Response.Cookies("CostingModule_SaveAccountManagerWantsAllSearch").Expires = DateTime.Now.AddDays(-1)
                Else
                    'Account Manager just wants unique account manager
                    If ViewState("SubscriptionID") = 46 Then 'CST Sales
                        Response.Cookies("CostingModule_SaveAccountManagerWantsAllSearch").Value = "ALL"
                    End If

                    'Everyone else wants all.
                    Response.Cookies("CostingModule_SaveAccountManagerIDSearch").Value = 0
                    Response.Cookies("CostingModule_SaveAccountManagerIDSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchDepartmentValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveDepartmentIDSearch").Value = ddSearchDepartmentValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveDepartmentIDSearch").Value = 0
                    Response.Cookies("CostingModule_SaveDepartmentIDSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchFormulaValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveFormulaIDSearch").Value = ddSearchFormulaValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveFormulaIDSearch").Value = 0
                    Response.Cookies("CostingModule_SaveFormulaIDSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                Response.Cookies("CostingModule_SaveDrawingNoSearch").Value = Replace(txtSearchDrawingNoValue.Text.Trim, "'", "")
                txtSearchDrawingNoValue.Text = Replace(txtSearchDrawingNoValue.Text.Trim, "'", "")

                Response.Cookies("CostingModule_SavePartNoSearch").Value = Replace(txtSearchPartNoValue.Text.Trim, "'", "")
                txtSearchPartNoValue.Text = Replace(txtSearchPartNoValue.Text.Trim, "'", "")

                Response.Cookies("CostingModule_SaveCustomerPartNoSearch").Value = Replace(txtSearchCustomerPartNoValue.Text.Trim, "'", "")
                txtSearchCustomerPartNoValue.Text = Replace(txtSearchCustomerPartNoValue.Text.Trim, "'", "")

                Response.Cookies("CostingModule_SaveDesignLevelSearch").Value = Replace(txtSearchDesignLevelValue.Text.Trim, "'", "")
                txtSearchDesignLevelValue.Text = Replace(txtSearchDesignLevelValue.Text.Trim, "'", "")

                Response.Cookies("CostingModule_SavePartNameSearch").Value = Replace(txtSearchPartNameValue.Text.Trim, "'", "")
                txtSearchPartNameValue.Text = Replace(txtSearchPartNameValue.Text.Trim, "'", "")

                If txtSearchRFDNoValue.Text.Trim <> "" Then
                    Response.Cookies("CostingModule_SaveRFDNoSearch").Value = Replace(txtSearchRFDNoValue.Text.Trim, "'", "")
                Else
                    Response.Cookies("CostingModule_SaveRFDNoSearch").Value = ""
                    Response.Cookies("CostingModule_SaveRFDNoSearch").Expires = DateTime.Now.AddDays(-1)
                End If
                txtSearchRFDNoValue.Text = Replace(txtSearchRFDNoValue.Text.Trim, "'", "")

                If ddSearchProgramValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveProgramIDSearch").Value = ddSearchProgramValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveProgramIDSearch").Value = 0
                    Response.Cookies("CostingModule_SaveProgramIDSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchCommodityValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveCommodityIDSearch").Value = ddSearchCommodityValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveCommodityIDSearch").Value = 0
                    Response.Cookies("CostingModule_SaveCommodityIDSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchYearValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveYearSearch").Value = ddSearchYearValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveYearSearch").Value = 0
                    Response.Cookies("CostingModule_SaveYearSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchCustomerValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveCustomerSearch").Value = ddSearchCustomerValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveCustomerSearch").Value = ""
                    Response.Cookies("CostingModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchUGNFacilityValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveUGNFacilitySearch").Value = ddSearchUGNFacilityValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveUGNFacilitySearch").Value = ""
                    Response.Cookies("CostingModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchWaitingForTeamMemberApprovalValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch").Value = ddSearchWaitingForTeamMemberApprovalValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch").Value = 0
                    Response.Cookies("CostingModule_SaveWaitingForTeamMemberApprovalSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If ddSearchApprovedByTeamMemberValue.SelectedIndex > 0 Then
                    Response.Cookies("CostingModule_SaveApprovedByTeamMemberSearch").Value = ddSearchApprovedByTeamMemberValue.SelectedValue
                Else
                    Response.Cookies("CostingModule_SaveApprovedByTeamMemberSearch").Value = 0
                    Response.Cookies("CostingModule_SaveApprovedByTeamMemberSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                ViewState("isApproved") = 0
                ViewState("filterApproved") = 0
                Response.Cookies("CostingModule_SaveIsApprovedSearch").Value = 0
                Response.Cookies("CostingModule_SaveIsApprovedSearch").Expires = DateTime.Now.AddDays(-1)

                Response.Cookies("CostingModule_SaveFilterApprovedSearch").Value = 0
                Response.Cookies("CostingModule_SaveFilterApprovedSearch").Expires = DateTime.Now.AddDays(-1)

                If ddSearchApprovedValue.SelectedIndex > 0 Then
                    If ddSearchApprovedValue.SelectedValue = "Approved" Then
                        Response.Cookies("CostingModule_SaveIsApprovedSearch").Value = 1
                        Response.Cookies("CostingModule_SaveFilterApprovedSearch").Value = 1
                        ViewState("isApproved") = 1
                        ViewState("filterApproved") = 1
                    End If

                    If ddSearchApprovedValue.SelectedValue = "Pending" Then
                        Response.Cookies("CostingModule_SaveIsApprovedSearch").Value = 0
                        Response.Cookies("CostingModule_SaveFilterApprovedSearch").Value = 1
                        ViewState("isApproved") = 0
                        ViewState("filterApproved") = 1
                    End If
                End If

                If cbBOM.Checked = True Then
                    ViewState("checkBOM") = 1
                    Response.Cookies("CostingModule_SaveCheckBOMSearch").Value = 1
                Else
                    ViewState("checkBOM") = 0
                    Response.Cookies("CostingModule_SaveCheckBOMSearch").Value = 0
                    Response.Cookies("CostingModule_SaveCheckBOMSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                If txtSearchMaterialIDValue.Text <> "" Then
                    Response.Cookies("CostingModule_SaveListMaterialIDSearch").Value = Replace(txtSearchMaterialIDValue.Text.Trim, "'", "")
                Else
                    Response.Cookies("CostingModule_SaveListMaterialIDSearch").Value = ""
                    Response.Cookies("CostingModule_SaveListMaterialIDSearch").Expires = DateTime.Now.AddDays(-1)
                End If
                txtSearchMaterialIDValue.Text = Replace(txtSearchMaterialIDValue.Text.Trim, "'", "")
                If cbQuickQuote.Checked = True Then
                    ViewState("QuickQuote") = 1
                    Response.Cookies("CostingModule_SaveCheckQuickQuoteSearch").Value = 1
                Else
                    ViewState("QuickQuote") = 0
                    Response.Cookies("CostingModule_SaveCheckQuickQuoteSearch").Value = 0
                    Response.Cookies("CostingModule_SaveCheckQuickQuoteSearch").Expires = DateTime.Now.AddDays(-1)
                End If

                Response.Redirect("Cost_Sheet_List.aspx?CostSheetID=" & Server.UrlEncode(txtSearchCostSheetIDValue.Text.Trim) _
                & "&CostSheetStatus=" & Server.UrlEncode(ddSearchCostSheetStatusValue.SelectedValue) _
                & "&AccountManagerID=" & Server.UrlEncode(ddSearchAccountManagerValue.SelectedValue) _
                & "&DepartmentID=" & Server.UrlEncode(ddSearchDepartmentValue.SelectedValue) _
                & "&FormulaID=" & Server.UrlEncode(ddSearchFormulaValue.SelectedValue) _
                & "&DrawingNo=" & Server.UrlEncode(txtSearchDrawingNoValue.Text.Trim) _
                & "&PartNo=" & Server.UrlEncode(txtSearchPartNoValue.Text.Trim) _
                & "&CustomerPartNo=" & Server.UrlEncode(txtSearchCustomerPartNoValue.Text.Trim) _
                & "&DesignLevel=" & Server.UrlEncode(txtSearchDesignLevelValue.Text.Trim) _
                & "&PartName=" & Server.UrlEncode(txtSearchPartNameValue.Text.Trim) _
                & "&RFDNo=" & Server.UrlEncode(txtSearchRFDNoValue.Text.Trim) _
                & "&ProgramID=" & Server.UrlEncode(ddSearchProgramValue.SelectedValue) _
                & "&CommodityID=" & Server.UrlEncode(ddSearchCommodityValue.SelectedValue) _
                & "&Year=" & Server.UrlEncode(ddSearchYearValue.SelectedValue) _
                & "&CustomerValue=" & Server.UrlEncode(ddSearchCustomerValue.SelectedValue) _
                & "&UGNFacility=" & Server.UrlEncode(ddSearchUGNFacilityValue.SelectedValue) _
                & "&WaitingForTeamMemberApproval=" & Server.UrlEncode(ddSearchWaitingForTeamMemberApprovalValue.SelectedValue) _
                & "&ApprovedByTeamMember=" & Server.UrlEncode(ddSearchApprovedByTeamMemberValue.SelectedValue) _
                & "&isApproved=" & ViewState("isApproved") _
                & "&filterApproved=" & ViewState("filterApproved") _
                & "&checkBOM=" & ViewState("checkBOM") _
                & "&MaterialID=" & ViewState("MaterialID") _
                & "&QuickQuote=" & ViewState("QuickQuote"), False)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        lblMessage.Text = ""

        Try
            CostingModule.DeleteCostingCookies()

            HttpContext.Current.Session("sessionCostingCurrentPage") = Nothing

            Response.Redirect("Cost_Sheet_List.aspx", False)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        'lblMessage.Text = ""

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionCostingCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        'lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        'lblMessage.Text = ""

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionCostingCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        'lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try

            Response.Redirect("Cost_Sheet_Detail.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionCostingCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionCostingCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try

            If txtGoToPage.Text.Trim <> "" Then
                If CType(txtGoToPage.Text.Trim, Integer) > 0 Then

                    ' Set viewstate variable to the specific page
                    If txtGoToPage.Text > ViewState("LastPageCount") Then
                        CurrentPage = ViewState("LastPageCount")
                    Else
                        CurrentPage = txtGoToPage.Text - 1
                    End If


                    HttpContext.Current.Session("sessionCostingCurrentPage") = CurrentPage

                    ' Reload control
                    BindData()
                End If
            Else
                txtGoToPage.Text = ""
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

    Protected Sub cbShowAdvancedSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbShowAdvancedSearch.CheckedChanged

        Try

            If cbShowAdvancedSearch.Checked = False Then
                Response.Cookies("UGNDB_ShowCostingAdvancedSearch").Value = 0
                accAdvancedSearch.SelectedIndex = -1
            Else
                Response.Cookies("UGNDB_ShowCostingAdvancedSearch").Value = 1
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
End Class
