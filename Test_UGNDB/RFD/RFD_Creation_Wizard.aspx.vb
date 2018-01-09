' ************************************************************************************************
'
' Name:		RFD_Creation_Wizard.aspx
' Purpose:	This Code Behind is for the Request for Development Creation Wizard - which will push Data to the RFD Details page when the user completes it
'
' Date		Author	    
' 08/17/2010 Roderick Carlson
' 03/08/2011 Modified: Roderick Carlson -  do not let obsolete account managers to be selected
' 05/03/2011 Modified: Roderick Carlson -  New Approval Routing Rules
' 02/21/2012 Modified: Roderick Carlson -  select Quality Engineer based on MAKE
' 03/23/2012 Modified: Roderick Carlson - Several Changes:
'                                           Remove Continuous Line and Material Size Change checkboxs
'                                           Enable cbQualityEngineeringRequired for Customer Driven Change
'                                           Adjust for Quote Only Business Process
' 04/17/2012 Modified: Roderick Carlson - hide cbAffectsCostSheetOnly for Sales/PM
' 04/27/2012 Modified: Roderick Carlson - add Program Manager selection
' 05/07/2012 Modified: Roderick Carlson - add isPurchasingExternalRFQ
' 05/07/2014 Modified: LREY             - Added isCostReduction
' ************************************************************************************************

Partial Class RFD_Creation_Wizard
    Inherits System.Web.UI.Page
    Private Sub EnableControls()

        Try

            If ViewState("isSales") = True Or ViewState("isProgramManagement") = True Then
                cbAffectsCostSheetOnly.Visible = False
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
    Private Sub AdjustApprovalRouting()

        Try
            'Dim iCommodityID As Integer = 0

            'cbContinuousLine.Enabled = True
            'cbMaterialSizeChange.Enabled = True
            cbDVPRrequired.Enabled = True

            cbCapitalRequired.Enabled = True
            cbPackagingRequired.Enabled = True
            cbPlantControllerRequired.Enabled = True
            cbProcessRequired.Enabled = True
            cbProductDevelopmentRequired.Enabled = True
            cbPurchasingExternalRFQRequired.Enabled = True
            cbPurchasingRequired.Enabled = True
            cbQualityEngineeringRequired.Enabled = True
            cbToolingRequired.Enabled = True

            If cbAffectsCostSheetOnly.Checked = True Then
                cbCostingRequired.Checked = True

                cbCapitalRequired.Checked = False
                cbPlantControllerRequired.Checked = False
                cbPackagingRequired.Checked = False
                cbProcessRequired.Checked = False
                cbProductDevelopmentRequired.Checked = False
                cbPurchasingExternalRFQRequired.Checked = False
                cbPurchasingRequired.Checked = False
                cbQualityEngineeringRequired.Checked = False
                cbToolingRequired.Checked = False
            Else

                'If ddWorkflowFamily.SelectedIndex > 0 Then
                '    iCommodityID = ddWorkflowFamily.SelectedValue
                'End If

                ''if the commodity is a damper then it is a continuous line
                'Select Case iCommodityID
                '    Case 33, 60, 61, 62
                '        cbContinuousLine.Checked = True
                'End Select

                'If cbContinuousLine.Checked = True Or cbMaterialSizeChange.Checked = True Then
                '    cbCapitalRequired.Checked = False
                '    cbProcessRequired.Checked = False
                '    cbToolingRequired.Checked = False
                '    'Else
                '    '    cbCapitalRequired.Checked = True
                '    '    cbProcessRequired.Checked = True
                '    '    cbToolingRequired.Checked = True
                'End If

                Select Case CType(ViewState("BusinessProcessTypeID"), Integer)

                    Case 1 'RFQ                                       
                        cbCostingRequired.Checked = True
                        cbCostingRequired.Enabled = False

                        cbProductDevelopmentRequired.Checked = True
                        cbProductDevelopmentRequired.Enabled = False

                        cbQualityEngineeringRequired.Checked = True
                        cbQualityEngineeringRequired.Enabled = False

                    Case 2 'RFC

                        cbCostingRequired.Checked = True
                        cbCostingRequired.Enabled = False

                        cbProductDevelopmentRequired.Checked = True
                        cbProductDevelopmentRequired.Enabled = False

                        cbQualityEngineeringRequired.Checked = True
                        cbQualityEngineeringRequired.Enabled = False


                        'Case 3 'Cost Reduction Idea

                        '    cbCostingRequired.Enabled = True
                        '    cbPackagingRequired.Enabled = True
                        '    cbPlantControllerRequired.Enabled = True
                        '    cbProcessRequired.Enabled = True
                        '    cbProductDevelopmentRequired.Enabled = True
                        '    cbPurchasingRequired.Enabled = True
                        '    cbQualityEngineeringRequired.Enabled = True
                        '    cbToolingRequired.Enabled = True

                    Case 5 'Going into Service

                        cbCostingRequired.Checked = True
                        cbCostingRequired.Enabled = False
                        '    cbProcessRequired.Checked = True
                        '    cbProductDevelopmentRequired.Checked = True
                        '    cbPurchasingRequired.Checked = True
                        '    cbQualityEngineeringRequired.Checked = True

                        '    If ddDesignationType.SelectedIndex > 0 Then
                        '        If ddDesignationType.SelectedValue = "C" Then
                        '            cbToolingRequired.Checked = True
                        '        Else
                        '            cbToolingRequired.Checked = False
                        '        End If
                        '    End If

                    Case 6 'End Of Life

                        cbCostingRequired.Checked = True
                        cbCostingRequired.Enabled = False
                        'cbProcessRequired.Checked = True
                        'cbProductDevelopmentRequired.Checked = True
                        'cbPurchasingRequired.Checked = True
                        'cbQualityEngineeringRequired.Checked = True

                        'If ddDesignationType.SelectedIndex > 0 Then
                        '    If ddDesignationType.SelectedValue = "C" Then
                        '        cbToolingRequired.Checked = True
                        '    Else
                        '        cbToolingRequired.Checked = False
                        '    End If
                        'End If

                    Case 7 'Quote Only

                        cbProductDevelopmentRequired.Checked = True
                        cbCostingRequired.Checked = True

                        cbQualityEngineeringRequired.Checked = False
                        cbQualityEngineeringRequired.Enabled = False

                        cbPurchasingRequired.Checked = False
                        cbPurchasingRequired.Enabled = False

                        ' Case 8 'Other

                        'cbCostingRequired.Enabled = True
                        '    cbPackagingRequired.Enabled = True
                        '    cbPlantControllerRequired.Enabled = True
                        '    cbProcessRequired.Enabled = True
                        '    cbProductDevelopmentRequired.Enabled = True
                        '    cbPurchasingRequired.Enabled = True
                        '    cbQualityEngineeringRequired.Enabled = True
                        '    cbToolingRequired.Enabled = True
                End Select

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
    Private Sub BindBusinessProcessAction(ByVal isQuoteOnly As Boolean)

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetBusinessProcessAction(0, True, isQuoteOnly)
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddBusinessProcessAction.DataSource = ds
                ddBusinessProcessAction.DataTextField = ds.Tables(0).Columns("ddBusinessProcessActionName").ColumnName
                ddBusinessProcessAction.DataValueField = ds.Tables(0).Columns("BusinessProcessActionID").ColumnName
                ddBusinessProcessAction.DataBind()
                'ddBusinessProcessAction.Items.Insert(0, "")
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
    Private Sub BindCriteria()

        Try

            Dim ds As DataSet

            BindBusinessProcessAction(False)
            'ds = commonFunctions.GetBusinessProcessAction(0, 0)
            'If (commonFunctions.CheckDataSet(ds) = True) Then
            '    ddBusinessProcessAction.DataSource = ds
            '    ddBusinessProcessAction.DataTextField = ds.Tables(0).Columns("ddBusinessProcessActionName").ColumnName
            '    ddBusinessProcessAction.DataValueField = ds.Tables(0).Columns("BusinessProcessActionID").ColumnName
            '    ddBusinessProcessAction.DataBind()
            '    'ddBusinessProcessAction.Items.Insert(0, "")
            'End If

            ds = commonFunctions.GetBusinessProcessType(0)
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddBusinessProcessType.DataSource = ds
                ddBusinessProcessType.DataTextField = ds.Tables(0).Columns("ddBusinessProcessTypeName").ColumnName
                ddBusinessProcessType.DataValueField = ds.Tables(0).Columns("BusinessProcessTypeID").ColumnName
                ddBusinessProcessType.DataBind()
            End If

            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddWorkFlowCommodity.DataSource = ds
                ddWorkFlowCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName
                ddWorkFlowCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddWorkFlowCommodity.DataBind()
                ddWorkFlowCommodity.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetDesignationType()
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddDesignationType.DataSource = ds
                ddDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName
                ddDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddDesignationType.DataBind()
            End If

            ds = commonFunctions.GetFamily()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddWorkflowFamily.DataSource = ds
                ddWorkflowFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName
                ddWorkflowFamily.DataValueField = ds.Tables(0).Columns("familyID").ColumnName
                ddWorkflowFamily.DataBind()
                ddWorkflowFamily.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPriceCode("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPriceCode.DataSource = ds
                ddPriceCode.DataTextField = ds.Tables(0).Columns("ddPriceCodeName").ColumnName
                ddPriceCode.DataValueField = ds.Tables(0).Columns("PriceCode").ColumnName
                ddPriceCode.DataBind()
                'ddPriceCode.Items.Insert(0, "")
            End If

            ds = RFDModule.GetRFDPriority(0)
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddPriority.DataSource = ds
                ddPriority.DataTextField = ds.Tables(0).Columns("ddPriorityName").ColumnName
                ddPriority.DataValueField = ds.Tables(0).Columns("PriorityID").ColumnName
                ddPriority.DataBind()
            End If

            ds = RFDModule.GetRFDInitiatorList()
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddInitiator.DataSource = ds
                ddInitiator.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName
                ddInitiator.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddInitiator.DataBind()
            End If

            ds = commonFunctions.GetProgramMake()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddWorkFlowMake.DataSource = ds
                ddWorkFlowMake.DataTextField = ds.Tables(0).Columns("Make").ColumnName.ToString()
                ddWorkFlowMake.DataValueField = ds.Tables(0).Columns("Make").ColumnName
                ddWorkFlowMake.DataBind()
                'ddWorkFlowMake.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.SelectedValue = ViewState("TeamMemberUGNFacility").ToString
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''
            '' SUBSCRIPTION DROPDOWNS
            ''''''''''''''''''''''''''''''''''''''''''''''''''
            'Account Manager
            ds = commonFunctions.GetTeamMemberBySubscription(9)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddAccountManager.DataBind()
                ddAccountManager.Items.Insert(0, "")
            End If

            'Program Manager
            ds = commonFunctions.GetTeamMemberBySubscription(31)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddProgramManager.DataSource = ds
                ddProgramManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddProgramManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddProgramManager.DataBind()
                ddProgramManager.Items.Insert(0, "")
            End If

            FilterProductDevelopmentCommodityList(0)

            FilterPurchasingFamilyList(0)

            FilterPurchasingMakeList("")

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub CheckRights()

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

            ViewState("TeamMemberID") = 0
            ViewState("SubscriptionID") = 0
            ViewState("TeamMemberUGNFacility") = ""

            ViewState("isDefaultCapital") = False
            ViewState("isCapital") = False

            ViewState("isDefaultCosting") = False
            ViewState("isCosting") = False

            ViewState("isDefaultPackaging") = False
            ViewState("isPackaging") = False

            ViewState("isDefaultPlantController") = False
            ViewState("isPlantController") = False

            ViewState("isDefaultProcess") = False
            ViewState("isProcess") = False

            ViewState("isDefaultProductDevelopment") = False
            ViewState("isProductDevelopment") = False

            ViewState("isDefaultPurchasing") = False
            ViewState("isPurchasing") = False

            ViewState("isDefaultQualityEngineer") = False
            ViewState("isQualityEngineer") = False

            ViewState("isDefaultTooling") = False
            ViewState("isTooling") = False

            ViewState("isSales") = False
            ViewState("isProgramManagement") = False

            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                '''' TESTING AS Different user
                If iTeamMemberID = 530 Then
                    'iTeamMemberID = 401 'Aisah Brown
                    'iTeamMemberID = 32 'Dan Cade
                    'iTeamMemberID = 433 'Derek Ames
                    iTeamMemberID = 246 'Mike Echevarria
                    'iTeamMemberID = 575 'Rick Matheny
                    'iTeamMemberID = 428 'Tracy Theos
                    'iTeamMemberID = 105 'Ron Davis
                    'iTeamMemberID = 611 'Vincente.Chavez
                    'iTeamMemberID = 222 'Jim Meade
                    'iTeamMemberID = 476 ' Pranav
                    'iTeamMemberID = 140 ' Bryan Hall
                    'iTeamMemberID = 698 'Emmanuel Reymond   
                    'iTeamMemberID = 188 'Duane Rushing
                End If

                ViewState("TeamMemberID") = iTeamMemberID

                'Account Manager
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 9)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 9
                    ViewState("isSales") = True
                End If

                'Champion
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 4)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 4
                    Select Case dsSubscription.Tables(0).Rows(0).Item("UGNFacility").ToString
                        Case "UN", "UP", "UR", "US", "UW", "OH"
                            ViewState("TeamMemberUGNFacility") = dsSubscription.Tables(0).Rows(0).Item("UGNFacility").ToString
                    End Select
                End If

                'Capital
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 119)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 119
                    ViewState("isCapital") = True
                End If

                'Costing
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 6)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 6
                    ViewState("isCosting") = True
                End If

                'Packaging
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 108)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 108
                    ViewState("isPackaging") = True
                End If

                'Plant Controller
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 20)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 20
                    ViewState("isPlantController") = True
                End If

                'Process
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 66)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 66
                    ViewState("isProcess") = True
                End If

                'Product Development
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 5)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 5
                    ViewState("isProductDevelopment") = True
                End If

                'Program Management
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 31)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 31
                    ViewState("isProgramManagement") = True
                End If

                'Purchasing
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 7)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 7
                    ViewState("isPurchasing") = True
                End If

                'Quality Engineer
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 22)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 22
                    ViewState("isQualityEngineer") = True
                End If

                'Tooling
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 65)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 65
                    ViewState("isTooling") = True
                End If

                'Default Capital
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 63)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultCapital") = True
                End If

                'Default Costing
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 50)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultCosting") = True
                End If

                ''Default Process
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 60)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultProcess") = True
                End If

                'Default Product Development
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 54)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultProductDevelopment") = True
                End If

                'Default Purchasing
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 53)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultPurchasing") = True
                End If

                'Default QualityEngineer
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 51)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultQualityEngineer") = True
                End If

                'Default Tooling
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 52)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultTooling") = True
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 37)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then
                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    'there should be no read only viewers of this page, unless admin users see an approved quote
                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isEdit") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isEdit") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isEdit") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
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

    Private Sub ClearMessages()

        lblMessage.Text = ""

    End Sub

    Protected Sub FilterProductDevelopmentCommodityList(ByVal CommodityID As Integer)

        Try

            Dim ds As DataSet

            ddProductDevelopmentTeamMemberByCommodity.Items.Clear()

            ds = commonFunctions.GetCommodityWithWorkFlowAssignments(CommodityID, "", 0)
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddProductDevelopmentTeamMemberByCommodity.DataSource = ds
                ddProductDevelopmentTeamMemberByCommodity.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName
                ddProductDevelopmentTeamMemberByCommodity.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddProductDevelopmentTeamMemberByCommodity.DataBind()
            Else
                'if no results came back, get default product development team member
                ds = commonFunctions.GetTeamMemberBySubscription(54)
                If commonFunctions.CheckDataSet(ds) = True Then
                    ddProductDevelopmentTeamMemberByCommodity.DataSource = ds
                    ddProductDevelopmentTeamMemberByCommodity.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                    ddProductDevelopmentTeamMemberByCommodity.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                    ddProductDevelopmentTeamMemberByCommodity.DataBind()
                Else
                    Dim liListItem As New ListItem
                    liListItem.Text = "None Found"
                    liListItem.Value = 0
                    ddProductDevelopmentTeamMemberByCommodity.Items.Add(liListItem)
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

    Protected Sub FilterPurchasingFamilyList(ByVal FamilyID As Integer)

        Try
            Dim ds As DataSet

            ddPurchasingTeamMemberByFamily.Items.Clear()

            ds = commonFunctions.GetFamilyWithWorkFlowAssignments(FamilyID, "", 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPurchasingTeamMemberByFamily.DataSource = ds
                ddPurchasingTeamMemberByFamily.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName
                ddPurchasingTeamMemberByFamily.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddPurchasingTeamMemberByFamily.DataBind()
            Else
                'if no results came back, get default purchasing team member
                ds = commonFunctions.GetTeamMemberBySubscription(53)
                If commonFunctions.CheckDataSet(ds) = True Then
                    ddPurchasingTeamMemberByFamily.DataSource = ds
                    ddPurchasingTeamMemberByFamily.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                    ddPurchasingTeamMemberByFamily.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                    ddPurchasingTeamMemberByFamily.DataBind()
                Else
                    Dim liListItem As New ListItem
                    liListItem.Text = "None Found"
                    liListItem.Value = 0
                    ddPurchasingTeamMemberByFamily.Items.Add(liListItem)
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

    Protected Sub FilterPurchasingMakeList(ByVal Make As String)

        Try
            Dim ds As DataSet

            ddPurchasingTeamMemberByMake.Items.Clear()

            ds = commonFunctions.GetProgramMakeWithWorkFlowAssignments(Make, 0, 7)
            If commonFunctions.CheckDataSet(ds) = True And Make <> "" Then
                ddPurchasingTeamMemberByMake.DataSource = ds
                ddPurchasingTeamMemberByMake.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
                ddPurchasingTeamMemberByMake.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddPurchasingTeamMemberByMake.DataBind()
                ddPurchasingTeamMemberByMake.SelectedIndex = 0
            Else 'if no results came back, get default purchasing team member
                ds = commonFunctions.GetTeamMemberBySubscription(53)
                If commonFunctions.CheckDataSet(ds) = True Then
                    ddPurchasingTeamMemberByMake.DataSource = ds
                    ddPurchasingTeamMemberByMake.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                    ddPurchasingTeamMemberByMake.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                    ddPurchasingTeamMemberByMake.DataBind()
                Else
                    Dim liListItem As New ListItem
                    liListItem.Text = "None Found"
                    liListItem.Value = 0
                    ddPurchasingTeamMemberByMake.Items.Add(liListItem)
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

    Protected Sub HandleCommentFields()

        Try

            txtImpactOnUGN.Attributes.Add("onkeypress", "return tbLimit();")
            txtImpactOnUGN.Attributes.Add("onkeyup", "return tbCount(" + lblImpactOnUGNCharCount.ClientID + ");")
            txtImpactOnUGN.Attributes.Add("maxLength", "1500")

            txtRFDDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtRFDDesc.Attributes.Add("onkeyup", "return tbCount(" + lblRFDDescCharCount.ClientID + ");")
            txtRFDDesc.Attributes.Add("maxLength", "1000")

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub ShowInitiator()

        Try
            btnInitiatorEdit.Visible = False

            btnInitiatorNext.Visible = ViewState("isEdit")

            ddInitiator.Enabled = ViewState("isEdit")

            trInitiator.Attributes.Add("style", "background-color: White")

            ddInitiator.SelectedValue = ViewState("TeamMemberID")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InitializeAllControls()

        Try
            ShowInitiator()

            'if sales or program management then default to RFQ else default to RFC
            If ViewState("isSales") = True Or ViewState("isProgramManagement") = True Then
                ddBusinessProcessType.SelectedValue = 1
                ViewState("BusinessProcessTypeID") = 1

                If ViewState("isSales") = True Then
                    ddAccountManager.SelectedValue = ViewState("TeamMemberID")
                End If

            Else
                ddBusinessProcessType.SelectedValue = 2
                ViewState("BusinessProcessTypeID") = 2
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Request For Development (RFD) Creation Wizard"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Request for Development (RFD) </b> > <a href='RFD_List.aspx'><b> List and Search </b></a> > Creation Wizard "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("RFDExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

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

            'clear crystal reports
            RFDModule.CleanRFDCrystalReports()

            If Not Page.IsPostBack Then
                CheckRights()

                BindCriteria()

                HandleCommentFields()

                InitializeAllControls()

                EnableControls()

            End If

            'Visual Basic ignores this if inside the above IF-Statement: If Not Page.IsPostBack Then
            Page.ClientScript.RegisterStartupScript(Me.[GetType](), "jsCheckTarget", "function CheckTargetInfo(){" & vbCr & vbLf & " var TmpTargetPrice = document.getElementById('" & txtTargetPrice.ClientID & "').value; var TmpTargetAnnualVolume = document.getElementById('" & txtTargetAnnualVolume.ClientID & "').value; var TmpTargetAnnualSales = document.getElementById('" & txtTargetAnnualSales.ClientID & "').value;  /* alert(TmpTargetPrice); alert(TmpTargetAnnualVolume); alert(TmpTargetAnnualSales); */ if (TmpTargetPrice != null && TmpTargetAnnualVolume != null) { if (TmpTargetPrice > 0 && TmpTargetAnnualVolume > 0 && TmpTargetAnnualSales > 0) { if (TmpTargetPrice * TmpTargetAnnualVolume != TmpTargetAnnualSales) { alert('Error: Target Price * Target Annual Volume does NOT equal Target Annual Sales'); } } } " & vbCr & vbLf & " }", True)

            txtTargetPrice.Attributes.Add("onblur", "javascript:CheckTargetInfo();")
            txtTargetAnnualVolume.Attributes.Add("onblur", "javascript:CheckTargetInfo();")
            txtTargetAnnualSales.Attributes.Add("onblur", "javascript:CheckTargetInfo();")

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub HideInitiator()

        Try
            btnInitiatorEdit.Visible = ViewState("isEdit")

            btnInitiatorNext.Visible = False

            ddInitiator.Enabled = False

            trInitiator.Attributes.Add("style", "background-color: LightGray")
            trInitiator.Visible = ViewState("isEdit")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ShowBusinessProcessType()

        Try
            btnBusinessProcessTypeEdit.Visible = False
            btnBusinessProcessTypePrevious.Visible = ViewState("isEdit")
            btnBusinessProcessTypeNext.Visible = ViewState("isEdit")

            ddBusinessProcessType.Enabled = ViewState("isEdit")

            trBusinessProcessType.Visible = ViewState("isEdit")
            trBusinessProcessType.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ShowIsCostReduction()

        Try
            btnIsCostReductionEdit.Visible = False
            btnIsCostReductionPrevious.Visible = ViewState("isEdit")
            btnIsCostReductionNext.Visible = ViewState("isEdit")

            ddIsCostReduction.Enabled = ViewState("isEdit")

            trIsCostReduction.Visible = ViewState("isEdit")
            trIsCostReduction.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnInitiatorNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInitiatorNext.Click

        Try

            ClearMessages()

            HideInitiator()

            ShowBusinessProcessType()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnBusinessProcessTypePrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBusinessProcessTypePrevious.Click

        Try

            ShowInitiator()

            HideBusinessProcessType(False)

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub HideBusinessProcessType(ByVal isUsed As Boolean)

        Try

            btnBusinessProcessTypeEdit.Visible = ViewState("isEdit")
            btnBusinessProcessTypePrevious.Visible = False
            btnBusinessProcessTypeNext.Visible = False

            If isUsed = True Then
                ddBusinessProcessType.Enabled = False

                trBusinessProcessType.Attributes.Add("style", "background-color: LightGray")
                trBusinessProcessType.Visible = ViewState("isEdit")
            Else
                trBusinessProcessType.Visible = False
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

    Private Sub HideIsCostReduction(ByVal isUsed As Boolean)

        Try

            btnIsCostReductionEdit.Visible = ViewState("isEdit")
            btnIsCostReductionPrevious.Visible = False
            btnIsCostReductionNext.Visible = False

            If isUsed = True Then
                ddIsCostReduction.Enabled = False

                trIsCostReduction.Attributes.Add("style", "background-color: LightGray")
                trIsCostReduction.Visible = ViewState("isEdit")
            Else
                trIsCostReduction.Visible = False
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

    Private Sub ShowBusinessProcessAction()

        Try
            If ViewState("BusinessProcessTypeID") = 7 Then
                BindBusinessProcessAction(True)
            Else
                BindBusinessProcessAction(False)
            End If

            btnBusinessProcessActionEdit.Visible = False
            btnBusinessProcessActionPrevious.Visible = ViewState("isEdit")
            btnBusinessProcessActionNext.Visible = ViewState("isEdit")

            ddBusinessProcessAction.Enabled = ViewState("isEdit")

            trBusinessProcessAction.Visible = ViewState("isEdit")
            trBusinessProcessAction.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ShowUGNFacility()

        Try

            btnUGNFacilityEdit.Visible = False
            btnUGNFacilityPrevious.Visible = ViewState("isEdit")
            btnUGNFacilityNext.Visible = ViewState("isEdit")

            ddUGNFacility.Enabled = ViewState("isEdit")

            trUGNFacility.Visible = ViewState("isEdit")
            trUGNFacility.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ShowDesignationType()

        Try

            btnDesignationTypeEdit.Visible = False
            btnDesignationTypePrevious.Visible = ViewState("isEdit")
            btnDesignationTypeNext.Visible = ViewState("isEdit")

            ddDesignationType.Enabled = ViewState("isEdit")

            trDesignationType.Visible = ViewState("isEdit")
            trDesignationType.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnBusinessProcessTypeNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBusinessProcessTypeNext.Click

        Try

            ClearMessages()

            If InStr(ddBusinessProcessType.SelectedItem.Text, "**") > 0 Then
                lblMessage.Text &= "Error: Please choose a non-obsolete business process action."
            Else

                'Dim bWrongBusinessProcessType As Boolean = False

                ViewState("BusinessProcessTypeID") = 1 'default RFQ

                If ddBusinessProcessType.SelectedIndex >= 0 Then

                    ViewState("BusinessProcessTypeID") = ddBusinessProcessType.SelectedValue

                    'sales and program management cannot create RFCs
                    If ViewState("BusinessProcessTypeID") = 2 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                        'just force the change and move on
                        'bWrongBusinessProcessType = True

                        'reset values                    
                        ViewState("BusinessProcessTypeID") = 1
                        ddBusinessProcessType.SelectedValue = ViewState("BusinessProcessTypeID")
                    End If

                    'other team members (not sales, not program management) can NOT create RFQs Or Quote Only
                    If (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7) And ViewState("isSales") = False And ViewState("isProgramManagement") = False Then
                        'bWrongBusinessProcessType = True
                        'just force the change and move on
                        'reset values                    
                        ViewState("BusinessProcessTypeID") = 2
                        ddBusinessProcessType.SelectedValue = ViewState("BusinessProcessTypeID")
                    End If
                End If


                'If bWrongBusinessProcessType = True Then
                '    lblMessage.Text = "Error: Only Sales and Program Management can create the Business Process type of Customer Driven Change (RFQ).<br> Also UGN Driven Change (RFC) can NOT be created by Sales and Program Management."
                'Else
                HideBusinessProcessType(True)

                'if Customer Driven Change or Quote Only
                If ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7 Then
                    ShowBusinessProcessAction()
                Else
                    ShowUGNFacility()
                    'ShowDesignationType()

                End If

                Select Case CType(ViewState("BusinessProcessTypeID"), Integer)
                    Case 1
                        ddDesignationType.SelectedValue = "C"
                    Case 2
                        ddDesignationType.SelectedValue = "R"
                        'Case 5
                        '    ddDesignationType.SelectedValue = "H"
                    Case Else
                        ddDesignationType.SelectedValue = "C"
                End Select

                AdjustApprovalRouting()
                'End If
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

    Protected Sub btnDesignationTypePrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDesignationTypePrevious.Click

        Try

            ClearMessages()

            HideDesignationType(False)

            ShowUGNFacility()
            'If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ

            '    ShowBusinessProcessAction()

            'Else

            '    ShowBusinessProcessType()

            '    HideBusinessProcessAction(False)

            'End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnDesignationTypeNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDesignationTypeNext.Click

        Try

            ClearMessages()

            HideDesignationType(True)

            If ViewState("isSales") = True Then
                If ddDesignationType.SelectedIndex >= 0 Then
                    Select Case ddDesignationType.SelectedValue

                        Case "A", "B", "F", "G", "H", "I", "R", "0", "6"  'all potential child parts 'Semi-Finished Goods
                            'Case "R"  'Raw Materials
                            ShowWorkflowFamily()

                        Case "C" 'Finished Goods
                            'ShowWorkflowMake()                            
                            ShowProgramManager()
                    End Select
                End If
            Else
                ShowAccountManager()
            End If

            AdjustApprovalRouting()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub HideBusinessProcessAction(ByVal isUsed As Boolean)

        Try

            btnBusinessProcessActionEdit.Visible = ViewState("isEdit")
            btnBusinessProcessActionPrevious.Visible = False
            btnBusinessProcessActionNext.Visible = False

            If isUsed = True Then
                ddBusinessProcessAction.Enabled = False

                trBusinessProcessAction.Attributes.Add("style", "background-color: LightGray")
                trBusinessProcessAction.Visible = ViewState("isEdit")
            Else
                trBusinessProcessAction.Visible = False
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

    Private Sub HideUGNFacility(ByVal isUsed As Boolean)

        Try

            btnUGNFacilityEdit.Visible = ViewState("isEdit")
            btnUGNFacilityPrevious.Visible = False
            btnUGNFacilityNext.Visible = False

            If isUsed = True Then
                ddUGNFacility.Enabled = False

                trUGNFacility.Attributes.Add("style", "background-color: LightGray")
                trUGNFacility.Visible = ViewState("isEdit")
            Else
                trUGNFacility.Visible = False
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

    Private Sub HideDesignationType(ByVal isUsed As Boolean)

        Try

            btnDesignationTypeEdit.Visible = ViewState("isEdit")
            btnDesignationTypePrevious.Visible = False
            btnDesignationTypeNext.Visible = False

            If isUsed = True Then
                ddDesignationType.Enabled = False

                trDesignationType.Attributes.Add("style", "background-color: LightGray")
                trDesignationType.Visible = ViewState("isEdit")
            Else
                trDesignationType.Visible = False
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

    Protected Sub btnBusinessProcessActionNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBusinessProcessActionNext.Click

        Try

            ClearMessages()

            If InStr(ddBusinessProcessAction.SelectedItem.Text, "**") > 0 Then
                lblMessage.Text &= "Error: Please choose a non-obsolete business process action."
            Else

                HideBusinessProcessAction(True)
                ShowIsCostReduction()

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

    Protected Sub btnBusinessProcessActionPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBusinessProcessActionPrevious.Click

        Try

            ClearMessages()

            HideBusinessProcessAction(False)

            ShowBusinessProcessType()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnAccountManagerPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccountManagerPrevious.Click

        Try

            ClearMessages()

            HideAccountManager(False)

            ShowDesignationType()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub GetAccountManager()

        Try

            Dim ds As DataSet
            '(LREY) 01/07/2014
            Dim strCABBV As String = "" ''commonFunctions.GetCustomerCABBV(ddCustomer.SelectedValue)
            Dim iSoldTo As Integer = "" ''commonFunctions.GetCustomerSoldTo(ddCustomer.SelectedValue)

            If ddCustomer.SelectedIndex > 0 Then
                ds = commonFunctions.GetTeamMemberByWorkFlowAssignments(0, 0, 0, strCABBV, iSoldTo)

                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("TMID") > 0 Then
                            ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("TMID")
                        End If
                    End If
                Else
                    ddAccountManager.SelectedIndex = -1
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

    Protected Sub ddCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCustomer.SelectedIndexChanged

        Try

            ClearMessages()

            GetAccountManager()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub HideAccountManager(ByVal isUsed As Boolean)

        Try

            btnAccountManagerEdit.Visible = ViewState("isEdit")
            btnAccountManagerPrevious.Visible = False
            btnAccountManagerNext.Visible = False

            ddAccountManager.Enabled = False

            trMessageAccountManager.Visible = False

            If isUsed = True Then
                trAccountManager.Attributes.Add("style", "background-color: LightGray")
                trAccountManager.Visible = ViewState("isEdit")
            Else
                trAccountManager.Visible = False
            End If

            trCustomer.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HideProgramManager(ByVal isUsed As Boolean)

        Try

            btnProgramManagerEdit.Visible = ViewState("isEdit")
            btnProgramManagerPrevious.Visible = False
            btnProgramManagerNext.Visible = False

            ddProgramManager.Enabled = False

            If isUsed = True Then
                trProgramManager.Attributes.Add("style", "background-color: LightGray")
                trProgramManager.Visible = ViewState("isEdit")
            Else
                trProgramManager.Visible = False
            End If

            'trCustomer.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub ShowWorkflowFamily()

        Try

            btnWorkflowFamilyEdit.Visible = False
            btnWorkflowFamilyPrevious.Visible = ViewState("isEdit")
            btnWorkflowFamilyNext.Visible = ViewState("isEdit")

            ddPurchasingTeamMemberByFamily.Enabled = ViewState("isEdit")
            ddWorkflowFamily.Enabled = ViewState("isEdit")

            trFamily.Visible = ViewState("isEdit")
            trFamily.Attributes.Add("style", "background-color: White")

            trPurchasingTeamMemberByFamily.Visible = ViewState("isEdit")
            trPurchasingTeamMemberByFamily.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ShowWorkflowMake()

        Try

            btnWorkflowMakeEdit.Visible = False
            btnWorkflowMakePrevious.Visible = ViewState("isEdit")
            btnWorkflowMakeNext.Visible = ViewState("isEdit")

            ddPurchasingTeamMemberByMake.Enabled = ViewState("isEdit")
            ddWorkFlowMake.Enabled = ViewState("isEdit")

            trMake.Visible = ViewState("isEdit")
            trMake.Attributes.Add("style", "background-color: White")

            trPurchasingTeamMemberByMake.Visible = ViewState("isEdit")
            trPurchasingTeamMemberByMake.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HideWorkflowMake(ByVal isUsed As Boolean)

        Try
            btnWorkflowMakeEdit.Visible = ViewState("isEdit")
            btnWorkflowMakePrevious.Visible = False
            btnWorkflowMakeNext.Visible = False

            If isUsed = True Then

                ddPurchasingTeamMemberByMake.Visible = ViewState("isEdit")
                ddPurchasingTeamMemberByMake.Enabled = False

                ddWorkFlowMake.Enabled = False

                trPurchasingTeamMemberByMake.Attributes.Add("style", "background-color: LightGray")
                trPurchasingTeamMemberByMake.Visible = ViewState("isEdit")

            Else

                trPurchasingTeamMemberByMake.Visible = False
                trMake.Visible = False
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

    Protected Sub btnAccountManagerNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccountManagerNext.Click

        Try

            ClearMessages()

            If InStr(ddAccountManager.SelectedItem.Text, "**") <= 0 Then
                HideAccountManager(True)

                If ddDesignationType.SelectedIndex >= 0 Then
                    Select Case ddDesignationType.SelectedValue

                        Case "A", "B", "F", "G", "H", "I", "R", "0", "6"  'all potential child parts 'Semi-Finished Goods
                            'Case "R"  'Raw Materials
                            ShowWorkflowFamily()

                        Case "C" 'Finished Goods
                            ShowWorkflowMake()
                    End Select
                End If
            Else
                lblMessage.Text = "Error: Please select another account manager. This team member is listed as obsolete."
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

    Protected Sub cbAffectsCostSheetOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbAffectsCostSheetOnly.CheckedChanged

        Try

            ClearMessages()

            AdjustApprovalRouting()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub ddWorkflowFamily_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddWorkflowFamily.SelectedIndexChanged

        Try

            ClearMessages()

            If ddWorkflowFamily.SelectedIndex > 0 Then
                FilterPurchasingFamilyList(ddWorkflowFamily.SelectedValue)
            Else
                FilterPurchasingFamilyList(0)
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

    Protected Sub ddWorkFlowMake_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddWorkFlowMake.SelectedIndexChanged

        Try

            ClearMessages()

            If ddWorkFlowMake.SelectedIndex > 0 Then
                FilterPurchasingMakeList(ddWorkFlowMake.SelectedValue)
            Else
                FilterPurchasingMakeList("")
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

    Private Sub HideWorkflowFamily(ByVal isUsed As Boolean)

        Try

            btnWorkflowFamilyEdit.Visible = ViewState("isEdit")
            btnWorkflowFamilyPrevious.Visible = False
            btnWorkflowFamilyNext.Visible = False

            If isUsed = True Then
                ddPurchasingTeamMemberByFamily.Enabled = False
                ddWorkflowFamily.Enabled = False

                trPurchasingTeamMemberByFamily.Attributes.Add("style", "background-color: LightGray")
                trPurchasingTeamMemberByFamily.Visible = ViewState("isEdit")
            Else
                trFamily.Visible = False
                trPurchasingTeamMemberByFamily.Visible = False
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

    Private Sub ShowAccountManager()

        Try

            btnAccountManagerEdit.Visible = False
            btnAccountManagerPrevious.Visible = ViewState("isEdit")
            btnAccountManagerNext.Visible = ViewState("isEdit")

            ddAccountManager.Enabled = ViewState("isEdit")

            trMessageAccountManager.Visible = True

            trAccountManager.Visible = ViewState("isEdit")
            trAccountManager.Attributes.Add("style", "background-color: White")

            trCustomer.Visible = ViewState("isEdit")
            trCustomer.Attributes.Add("style", "background-color: White")

            If ViewState("BusinessProcessTypeID") = 1 Then
                lblMessageAccountManager.Text = "Account Manager is Required for RFQ"

                rfvAccountManager.Enabled = True
                btnAccountManagerNext.CausesValidation = True
            Else
                lblMessageAccountManager.Text = "Account Manager is optional"
                rfvAccountManager.Enabled = False
                btnAccountManagerNext.CausesValidation = False
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

    Private Sub ShowProgramManager()

        Try

            btnProgramManagerEdit.Visible = False
            btnProgramManagerPrevious.Visible = ViewState("isEdit")
            btnProgramManagerNext.Visible = ViewState("isEdit")

            ddProgramManager.Enabled = ViewState("isEdit")

            trProgramManager.Visible = ViewState("isEdit")
            trProgramManager.Attributes.Add("style", "background-color: White")

            'trCustomer.Visible = ViewState("isEdit")
            trCustomer.Attributes.Add("style", "background-color: White")

            If ViewState("BusinessProcessTypeID") = 1 Or (ViewState("BusinessProcessTypeID") = 1 And ViewState("BusinessProcessActionID") = 10) Then                
                rfvProgramManager.Enabled = True
                btnProgramManagerNext.CausesValidation = True
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

    Protected Sub btnWorkflowFamilyPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkflowFamilyPrevious.Click

        Try

            ClearMessages()

            HideWorkflowFamily(False)

            If ViewState("isSales") = True Then
                HideAccountManager(False)
                If ViewState("BusinessProcessTypeID") = 1 Then
                    ShowBusinessProcessAction()
                Else
                    ShowDesignationType()
                End If
            Else
                ShowAccountManager()

                If ViewState("BusinessProcessTypeID") = 1 Then
                    lblMessageAccountManager.Text = "Account Manager is Required for RFQ"

                    rfvAccountManager.Enabled = True
                    btnAccountManagerNext.CausesValidation = True
                Else
                    lblMessageAccountManager.Text = "Account Manager is optional"

                    rfvAccountManager.Enabled = False
                    btnAccountManagerNext.CausesValidation = False
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

    Protected Sub btnWorkflowMakePrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkflowMakePrevious.Click

        Try

            ClearMessages()

            HideWorkflowMake(False)

            If ViewState("isSales") = True Then               
                'HideDesignationType(False)
                'HideAccountManager(False)
                'ShowBusinessProcessAction()
                ShowProgramManager()
            Else
                ShowAccountManager()
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

    Private Sub ShowWorkflowCommodity()

        Try

            btnWorkflowCommodityEdit.Visible = False
            btnWorkFlowCommodityPrevious.Visible = ViewState("isEdit")
            btnWorkFlowCommodityNext.Visible = ViewState("isEdit")

            ddWorkFlowCommodity.Enabled = ViewState("isEdit")
            ddProductDevelopmentTeamMemberByCommodity.Enabled = ViewState("isEdit")

            trCommodity.Attributes.Add("style", "background-color: White")
            trCommodity.Visible = ViewState("isEdit")

            trProductDevelopmentTeamMemberByCommodity.Attributes.Add("style", "background-color: White")
            trProductDevelopmentTeamMemberByCommodity.Visible = ViewState("isEdit")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HideWorkflowCommodity(ByVal isUsed As Boolean)

        Try

            btnWorkflowCommodityEdit.Visible = ViewState("isEdit")
            btnWorkFlowCommodityPrevious.Visible = False
            btnWorkFlowCommodityNext.Visible = False

            If isUsed = True Then
                ddWorkFlowCommodity.Enabled = False
                ddProductDevelopmentTeamMemberByCommodity.Enabled = False

                trProductDevelopmentTeamMemberByCommodity.Attributes.Add("style", "background-color: LightGray")
                trProductDevelopmentTeamMemberByCommodity.Visible = ViewState("isEdit")
            Else
                trProductDevelopmentTeamMemberByCommodity.Visible = False
                trCommodity.Visible = False

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

    Protected Sub btnWorkflowMakeNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkflowMakeNext.Click

        Try

            HideWorkflowMake(True)

            ShowWorkflowCommodity()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub ddWorkFlowCommodity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddWorkFlowCommodity.SelectedIndexChanged

        Try

            ClearMessages()

            If ddWorkFlowCommodity.SelectedIndex > 0 Then
                FilterProductDevelopmentCommodityList(ddWorkFlowCommodity.SelectedValue)
            Else
                FilterProductDevelopmentCommodityList(0)
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

    Protected Sub btnWorkFlowCommodityPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkFlowCommodityPrevious.Click

        Try

            ClearMessages()

            HideWorkflowCommodity(False)

            If ddDesignationType.SelectedValue = "C" Then
                ShowWorkflowMake()
            Else
                ShowWorkflowFamily()
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

    Protected Sub btnWorkflowFamilyNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkflowFamilyNext.Click

        Try

            ClearMessages()

            HideWorkflowFamily(True)

            ShowWorkflowCommodity()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub ShowPriceCode()

        Try

            btnPriceCodeEdit.Visible = False
            btnPriceCodePrevious.Visible = ViewState("isEdit")
            btnPriceCodeNext.Visible = ViewState("isEdit")

            ddPriceCode.Enabled = ViewState("isEdit")

            If ddDesignationType.SelectedValue = "C" Then
                ddPriceCode.SelectedValue = "A"
            End If

            trPriceCode.Visible = ViewState("isEdit")
            trPriceCode.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HidePriceCode(ByVal isUsed As Boolean)

        Try

            btnPriceCodeEdit.Visible = ViewState("isEdit")
            btnPriceCodePrevious.Visible = False
            btnPriceCodeNext.Visible = False

            If isUsed = True Then
                ddPriceCode.Enabled = False

                trPriceCode.Attributes.Add("style", "background-color: LightGray")
                trPriceCode.Visible = ViewState("isEdit")
            Else
                trPriceCode.Visible = False
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

    Private Sub ShowPriority()

        Try

            btnPriorityEdit.Visible = False
            btnPriorityPrevious.Visible = ViewState("isEdit")
            btnPriorityNext.Visible = ViewState("isEdit")

            ddPriority.Enabled = ViewState("isEdit")

            trPriority.Visible = ViewState("isEdit")
            trPriority.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnWorkFlowCommodityNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkFlowCommodityNext.Click

        Try

            ClearMessages()

            HideWorkflowCommodity(True)

            ShowPriceCode()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnPriceCodePrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPriceCodePrevious.Click

        Try

            ClearMessages()

            HidePriceCode(False)

            ShowWorkflowCommodity()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnPriceCodeNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPriceCodeNext.Click

        Try

            ClearMessages()

            HidePriceCode(True)

            ShowPriority()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub HidePriority(ByVal isUsed As Boolean)

        Try

            btnPriorityEdit.Visible = ViewState("isEdit")
            btnPriorityPrevious.Visible = False
            btnPriorityNext.Visible = False

            If isUsed = True Then
                ddPriority.Enabled = False

                trPriority.Attributes.Add("style", "background-color: LightGray")
                trPriority.Visible = ViewState("isEdit")
            Else
                trPriority.Visible = False
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

    Protected Sub btnPriorityPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPriorityPrevious.Click

        Try

            ClearMessages()

            HidePriority(False)

            ShowPriceCode()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub ShowDueDate()

        Try

            btnDueDateEdit.Visible = False
            btnDueDateNext.Visible = ViewState("isEdit")
            btnDueDatePrevious.Visible = ViewState("isEdit")

            imgDueDate.Visible = ViewState("isEdit")

            'if RFC
            If ViewState("BusinessProcessTypeID") = 2 Then
                lblMessageDueDate.Visible = True
                btnDueDateNext.CausesValidation = True
                rfvDueDate.Enabled = True

                txtDueDate.Text = Today.Date.AddDays(12)
            Else
                lblMessageDueDate.Visible = False
                btnDueDateNext.CausesValidation = False
                rfvDueDate.Enabled = False
            End If

            txtDueDate.Enabled = ViewState("isEdit")

            trDueDate.Visible = True
            trDueDate.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub HideDueDate(ByVal isUsed As Boolean)

        Try

            btnDueDateEdit.Visible = ViewState("isEdit")
            btnDueDateNext.Visible = False
            btnDueDatePrevious.Visible = False

            imgDueDate.Visible = False

            lblMessageDueDate.Visible = False

            If isUsed = True Then
                txtDueDate.Enabled = False

                trDueDate.Attributes.Add("style", "background-color: LightGray")
                trDueDate.Visible = ViewState("isEdit")
            Else
                trDueDate.Visible = False
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

    Protected Sub btnPriorityNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPriorityNext.Click

        Try

            ClearMessages()

            HidePriority(True)

            ShowDueDate()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnDueDatePrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDueDatePrevious.Click

        Try

            ClearMessages()

            HideDueDate(False)

            ShowPriority()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub ShowRFDDesc()

        Try

            btnRFDDescEdit.Visible = False
            btnRFDDescPrevious.Visible = ViewState("isEdit")
            btnRFDDescNext.Visible = ViewState("isEdit")

            txtRFDDesc.Enabled = ViewState("isEdit")

            trRFDDesc.Visible = ViewState("isEdit")
            trRFDDesc.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HideRFDDesc(ByVal isUsed As Boolean)

        Try

            btnRFDDescEdit.Visible = ViewState("isEdit")
            btnRFDDescPrevious.Visible = False
            btnRFDDescNext.Visible = False

            If isUsed = True Then
                txtRFDDesc.Enabled = False

                trRFDDesc.Attributes.Add("style", "background-color: LightGray")
                trRFDDesc.Visible = ViewState("isEdit")
            Else
                trRFDDesc.Visible = False
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

    Protected Sub btnDueDateNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDueDateNext.Click

        Try

            ClearMessages()

            HideDueDate(True)

            ShowRFDDesc()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnRFDDescPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRFDDescPrevious.Click

        Try

            ClearMessages()

            HideRFDDesc(False)

            ShowDueDate()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub ShowImpactOnUGN()

        Try

            btnImpactOnUGNEdit.Visible = False
            btnImpactOnUGNNext.Visible = ViewState("isEdit")
            btnImpactOnUGNPrevious.Visible = ViewState("isEdit")

            txtImpactOnUGN.Enabled = ViewState("isEdit")

            trImpactOnUGN.Visible = ViewState("isEdit")
            trImpactOnUGN.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HideImpactOnUGN(ByVal isUsed As Boolean)

        Try

            btnImpactOnUGNEdit.Visible = ViewState("isEdit")
            btnImpactOnUGNNext.Visible = False
            btnImpactOnUGNPrevious.Visible = False

            If isUsed = True Then
                txtImpactOnUGN.Enabled = False

                trImpactOnUGN.Attributes.Add("style", "background-color: LightGray")
                trImpactOnUGN.Visible = ViewState("isEdit")
            Else
                trImpactOnUGN.Visible = False
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

    Protected Sub btnRFDDescNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRFDDescNext.Click

        Try

            ClearMessages()

            HideRFDDesc(True)

            ShowImpactOnUGN()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnImpactOnUGNPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImpactOnUGNPrevious.Click

        Try

            ClearMessages()

            HideImpactOnUGN(False)

            ShowRFDDesc()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub ShowTarget()

        Try

            btnCalculateTargetAnnualSales.Visible = ViewState("isEdit")

            btnTargetEdit.Visible = False
            btnTargetPrevious.Visible = ViewState("isEdit")
            btnTargetNext.Visible = ViewState("isEdit")

            txtTargetPrice.Enabled = ViewState("isEdit")
            txtTargetAnnualVolume.Enabled = ViewState("isEdit")
            txtTargetAnnualSales.Enabled = ViewState("isEdit")

            trTargetPrice.Visible = ViewState("isEdit")
            trTargetPrice.Attributes.Add("style", "background-color: White")

            trTargetAnnualVolume.Visible = ViewState("isEdit")
            trTargetAnnualVolume.Attributes.Add("style", "background-color: White")

            trTargetAnnualSales.Visible = ViewState("isEdit")
            trTargetAnnualSales.Attributes.Add("style", "background-color: White")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HideTarget(ByVal isUsed As Boolean)

        Try

            btnCalculateTargetAnnualSales.Visible = False

            btnTargetEdit.Visible = ViewState("isEdit")
            btnTargetPrevious.Visible = False
            btnTargetNext.Visible = False

            If isUsed = True Then
                txtTargetPrice.Enabled = False
                txtTargetAnnualVolume.Enabled = False
                txtTargetAnnualSales.Enabled = False

                trTargetPrice.Attributes.Add("style", "background-color: LightGray")
                trTargetAnnualVolume.Attributes.Add("style", "background-color: LightGray")
                trTargetAnnualSales.Attributes.Add("style", "background-color: LightGray")

                trTargetPrice.Visible = ViewState("isEdit")
                trTargetAnnualVolume.Visible = ViewState("isEdit")
                trTargetAnnualSales.Visible = ViewState("isEdit")
            Else
                trTargetPrice.Visible = False
                trTargetAnnualVolume.Visible = False
                trTargetAnnualSales.Visible = False
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

    Protected Sub btnImpactOnUGNNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImpactOnUGNNext.Click

        Try

            ClearMessages()

            HideImpactOnUGN(True)

            ShowTarget()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnTargetPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTargetPrevious.Click

        Try

            ClearMessages()

            HideTarget(False)

            ShowImpactOnUGN()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Function CheckTarget() As Boolean

        Dim bReturn As Boolean = True

        Try

            Dim dTargetAnnualSales As Double = 0
            If txtTargetAnnualSales.Text.Trim <> "" Then
                dTargetAnnualSales = CType(txtTargetAnnualSales.Text.Trim, Double)
            End If

            Dim iTargetAnnualVolume As Integer = 0
            If txtTargetAnnualVolume.Text.Trim <> "" Then
                iTargetAnnualVolume = CType(txtTargetAnnualVolume.Text.Trim, Integer)
            End If

            Dim dTargetPrice As Double = 0
            If txtTargetPrice.Text.Trim <> "" Then
                dTargetPrice = CType(txtTargetPrice.Text.Trim, Double)
            End If

            If txtTargetPrice.Text.Trim <> "" And txtTargetAnnualVolume.Text.Trim <> "" Then
                If dTargetAnnualSales <> iTargetAnnualVolume * dTargetPrice Then
                    bReturn = False
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

        Return bReturn

    End Function

    Protected Sub btnCalculateTargetAnnualSales_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalculateTargetAnnualSales.Click

        Try

            ClearMessages()

            Dim dTargetPrice As Double = 0
            Dim dTargetAnnualVolume As Double = 0

            If txtTargetPrice.Text.Trim <> "" Then
                dTargetPrice = CType(txtTargetPrice.Text.Trim, Double)
            End If

            If txtTargetAnnualVolume.Text <> "" Then
                dTargetAnnualVolume = CType(txtTargetAnnualVolume.Text, Double)
            End If

            txtTargetAnnualSales.Text = dTargetPrice * dTargetAnnualVolume

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ShowRequiredTeamMembers()

        Try

            btnCreateRFD.Visible = ViewState("isEdit")
            btnRequiredTeamMembersPrevious.Visible = ViewState("isEdit")

            tblRequiredTeamMembers.Visible = ViewState("isEdit")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HideRequiredTeamMembers()

        Try

            btnCreateRFD.Visible = False
            btnRequiredTeamMembersPrevious.Visible = False

            tblRequiredTeamMembers.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnTargetNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTargetNext.Click

        Try

            ClearMessages()

            HideTarget(True)

            ShowRequiredTeamMembers()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnRequiredTeamMembersPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequiredTeamMembersPrevious.Click

        Try

            ClearMessages()

            HideRequiredTeamMembers()

            ShowTarget()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HideAll()

        Try

            HideAccountManager(False)
            HideBusinessProcessAction(False)
            HideBusinessProcessType(False)
            HideUGNFacility(False)
            HideDesignationType(False)
            HideDueDate(False)
            HideImpactOnUGN(False)
            HideInitiator()
            HidePriceCode(False)
            HidePriority(False)
            HideProgramManager(False)
            HideRequiredTeamMembers()
            HideRFDDesc(False)
            HideTarget(False)
            HideWorkflowCommodity(False)
            HideWorkflowFamily(False)
            HideWorkflowMake(False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnInitiatorEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInitiatorEdit.Click

        Try

            ClearMessages()

            HideAll()

            ShowInitiator()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnBusinessProcessTypeEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBusinessProcessTypeEdit.Click

        Try

            ClearMessages()

            HideAll()

            ShowBusinessProcessType()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnDesignationTypeEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDesignationTypeEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7 Then
                HideBusinessProcessAction(True)
            End If

            ShowDesignationType()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnBusinessProcessActionEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBusinessProcessActionEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            ShowBusinessProcessAction()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnAccountManagerEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccountManagerEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7 Then
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            ShowAccountManager()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnWorkflowMakeEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkflowMakeEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ
                HideBusinessProcessAction(True)
            End If

            If ViewState("BusinessProcessTypeID") = 1 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                HideDesignationType(False)
            Else
                HideDesignationType(True)
            End If

            If ViewState("isSales") = True Then
                HideAccountManager(False)
            Else
                HideAccountManager(True)
            End If

            HideWorkflowFamily(False)

            ShowWorkflowMake()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnWorkflowFamilyEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkflowFamilyEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7 Then
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            HideAccountManager(True)

            HideWorkflowMake(False)

            ShowWorkflowFamily()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnWorkflowCommodityEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkflowCommodityEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ
                HideBusinessProcessAction(True)
            End If

            If ViewState("BusinessProcessTypeID") = 1 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                HideDesignationType(False)
            Else
                HideDesignationType(True)
            End If

            If ViewState("isSales") = True Then
                HideAccountManager(False)
            Else
                HideAccountManager(True)
            End If

            If ddDesignationType.SelectedValue = "C" Then 'Finished Good
                HideWorkflowFamily(False)
                HideWorkflowMake(True)
            Else
                HideWorkflowFamily(True)
                HideWorkflowMake(False)
            End If

            ShowWorkflowCommodity()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnPriceCodeEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPriceCodeEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            HideAccountManager(True)

            If ddDesignationType.SelectedValue = "C" Then 'Finished Good
                HideWorkflowFamily(False)
                HideWorkflowMake(True)
            Else
                HideWorkflowFamily(True)
                HideWorkflowMake(False)
            End If

            HideWorkflowCommodity(True)

            ShowPriceCode()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnPriorityEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPriorityEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            HideAccountManager(True)

            If ddDesignationType.SelectedValue = "C" Then 'Finished Good
                HideWorkflowFamily(False)
                HideWorkflowMake(True)
            Else
                HideWorkflowFamily(True)
                HideWorkflowMake(False)
            End If

            HideWorkflowCommodity(True)

            HidePriceCode(True)

            ShowPriority()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnDueDateEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDueDateEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            HideAccountManager(True)

            If ddDesignationType.SelectedValue = "C" Then 'Finished Good
                HideWorkflowFamily(False)
                HideWorkflowMake(True)
            Else
                HideWorkflowFamily(True)
                HideWorkflowMake(False)
            End If

            HideWorkflowCommodity(True)

            HidePriceCode(True)

            HidePriority(True)

            ShowDueDate()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnRFDDescEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRFDDescEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            HideAccountManager(True)

            If ddDesignationType.SelectedValue = "C" Then 'Finished Good
                HideWorkflowFamily(False)
                HideWorkflowMake(True)
            Else
                HideWorkflowFamily(True)
                HideWorkflowMake(False)
            End If

            HideWorkflowCommodity(True)

            HidePriceCode(True)

            HidePriority(True)

            HideDueDate(True)

            ShowRFDDesc()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnImpactOnUGNEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImpactOnUGNEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            HideAccountManager(True)

            If ddDesignationType.SelectedValue = "C" Then 'Finished Good
                HideWorkflowFamily(False)
                HideWorkflowMake(True)
            Else
                HideWorkflowFamily(True)
                HideWorkflowMake(False)
            End If

            HideWorkflowCommodity(True)

            HidePriceCode(True)

            HidePriority(True)

            HideDueDate(True)

            HideRFDDesc(True)

            ShowImpactOnUGN()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnTargetEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTargetEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            HideAccountManager(True)

            If ddDesignationType.SelectedValue = "C" Then 'Finished Good
                HideWorkflowFamily(False)
                HideWorkflowMake(True)
            Else
                HideWorkflowFamily(True)
                HideWorkflowMake(False)
            End If

            HideWorkflowCommodity(True)

            HidePriceCode(True)

            HidePriority(True)

            HideDueDate(True)

            HideRFDDesc(True)

            HideImpactOnUGN(True)

            ShowTarget()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub CreateApprovalList()

        Try

            Dim dsCheckSubscription As DataSet
            'Dim dsCurrentApprover As DataSet
            Dim dsDefaultApprover As DataSet
            Dim strUGNFacility As String = ""

            Dim iTempDefaultTeamMemberID As Integer = 0

            If ddUGNFacility.SelectedIndex >= 0 Then
                Select Case ddUGNFacility.SelectedValue
                    Case "UN", "UP", "UR", "US"
                        strUGNFacility = ddUGNFacility.SelectedValue
                End Select
            End If

            If cbAffectsCostSheetOnly.Checked = True Then
                cbCostingRequired.Checked = True

                'get default approver
                dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(50) 'default costing
                If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                    If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                            iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                            'check if team member still has this subscription
                            dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 6)
                            If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                'insert new record
                                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 6, iTempDefaultTeamMemberID)

                            Else
                                lblMessage.Text += "<br>ERROR: The Default subscription for Costing Coordinator does not have the General Costing subscription, please submit a support requestor."
                            End If
                        End If
                    End If
                End If

            Else 'update all lists where needed
                If cbCapitalRequired.Checked = True Then
                    'get default approver
                    dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(63) 'default Capital
                    If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                'check if team member still has this subscription
                                dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 119)
                                If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                    'insert new record                                    
                                    RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 119, iTempDefaultTeamMemberID)
                                Else
                                    lblMessage.Text += "<br>ERROR: The Default subscription for Capital does not have the General Capital subscription, please submit a support requestor."
                                End If
                            End If
                        End If
                    End If

                End If

                If cbCostingRequired.Checked = True Then

                    'get default approver
                    dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(50) 'default costing
                    If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                'check if team member still has this subscription
                                dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 6)
                                If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                    'insert new record
                                    RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 6, iTempDefaultTeamMemberID)
                                Else
                                    lblMessage.Text += "<br>ERROR: The Default subscription for Costing Coordinator does not have the General Costing subscription, please submit a support requestor."
                                End If
                            End If
                        End If
                    End If
                End If

                iTempDefaultTeamMemberID = 0

                If cbPlantControllerRequired.Checked = True Then

                    dsDefaultApprover = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(109, strUGNFacility) 'Plant Controller by UGN Facility

                    If commonFunctions.CheckDataSet(dsDefaultApprover) = False Then
                        'get default approver
                        dsDefaultApprover = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(109, "UT") 'default Finance/Plant Controller for Tinley Park
                    End If

                    If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                'check if team member still has this subscription
                                dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 20)
                                If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                    'insert new record                                    
                                    RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 20, iTempDefaultTeamMemberID)
                                Else
                                    lblMessage.Text += "<br>ERROR: The Default subscription for Plant Controller does not have the General Finance Engineer subscription, please submit a support requestor."
                                End If
                            End If
                        End If
                    End If
                End If

                iTempDefaultTeamMemberID = 0

                If cbPackagingRequired.Checked = True Then

                    'get default approver
                    dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(108) 'default Packaging
                    If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                ''check if team member still has this subscription                                            
                                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 108, iTempDefaultTeamMemberID)
                            End If
                        End If
                    End If
                End If

                iTempDefaultTeamMemberID = 0

                If cbProcessRequired.Checked = True Then

                    'get 'Process By Facility approver
                    dsDefaultApprover = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(66, strUGNFacility) 'Process By Facility
                    If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")
                            End If
                        End If
                    End If

                    If iTempDefaultTeamMemberID = 0 Then
                        dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(60) 'default
                        If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                    'check if team member still has this subscription
                                    dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 60) 'default Process
                                    If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then

                                        lblMessage.Text += "<br>ERROR: The Default subscription for Process Engineer does not have the General Process Engineer subscription, please submit a support requestor."
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If iTempDefaultTeamMemberID > 0 Then
                        'insert new record                                    
                        RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 66, iTempDefaultTeamMemberID)
                    End If

                End If

                iTempDefaultTeamMemberID = 0

                If cbProductDevelopmentRequired.Checked = True Then

                    'first check for product development assigned to commodity in workflow
                    If ddWorkFlowCommodity.SelectedIndex > 0 Then
                        'check to see if user selected team member first
                        If ddProductDevelopmentTeamMemberByCommodity.SelectedIndex > 0 Then
                            iTempDefaultTeamMemberID = ddProductDevelopmentTeamMemberByCommodity.SelectedValue
                        End If

                        If iTempDefaultTeamMemberID = 0 Then 'no team member was selected for this commodity
                            dsDefaultApprover = commonFunctions.GetTeamMemberByWorkFlowAssignments(0, 5, ddWorkFlowCommodity.SelectedValue, "", 0)
                            If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default commodity found
                                        iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")
                                    End If
                                End If
                            End If
                        End If

                    End If

                    If iTempDefaultTeamMemberID = 0 Then
                        'if no commodity was assigned or no product development team member was assigned for the commodity then do this
                        'get default approver
                        dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(54) 'default Product Development
                        If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                    'check if team member still has this subscription
                                    dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 5)
                                    If commonFunctions.CheckDataSet(dsCheckSubscription) = False Then
                                        'default user no longer has subscription
                                        iTempDefaultTeamMemberID = 0
                                        lblMessage.Text += "<br>ERROR: The Default subscription for Product Development does not have the General Product Development subscription, please submit a support requestor."
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If iTempDefaultTeamMemberID > 0 Then
                        'insert new record                                      
                        RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 5, iTempDefaultTeamMemberID)
                    End If

                End If

                iTempDefaultTeamMemberID = 0

                If cbPurchasingRequired.Checked = True Then

                    'first check for PurchasingExternalRFQ assigned to family in workflow
                    If ddWorkflowFamily.SelectedIndex > 0 Then
                        If ddPurchasingTeamMemberByFamily.SelectedIndex > 0 Then 'check to see if user selected team member first
                            iTempDefaultTeamMemberID = ddPurchasingTeamMemberByFamily.SelectedValue
                        End If

                        If iTempDefaultTeamMemberID = 0 Then  'no team member was selected for this family
                            dsDefaultApprover = commonFunctions.GetWorkFlowFamilyPurchasingAssignments(0, ddWorkflowFamily.SelectedValue)
                            If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then 'default family found
                                        iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID")
                                    End If
                                End If
                            End If
                        End If

                    End If

                    'if Family Default is not found, check make default
                    If iTempDefaultTeamMemberID = 0 Then
                        If ddWorkFlowMake.SelectedIndex > 0 Then
                            'first check for PurchasingExternalRFQ assigned to make in workflow
                            If ddPurchasingTeamMemberByMake.SelectedIndex > 0 Then
                                iTempDefaultTeamMemberID = ddPurchasingTeamMemberByMake.SelectedValue
                            End If

                            If iTempDefaultTeamMemberID = 0 Then  'no team member was selected for this make
                                dsDefaultApprover = commonFunctions.GetWorkFlowMakeAssignments(ddWorkFlowMake.SelectedValue, 0, 7)
                                If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                        If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then 'default make found
                                            iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    'if no family or no make was assigned or no PurchasingExternalRFQ team member was assigned for the family then do this
                    If iTempDefaultTeamMemberID = 0 Then
                        'get default approver
                        dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(53) 'default Purchasing
                        If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                    'check if team member still has this subscription
                                    dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 7)
                                    If commonFunctions.CheckDataSet(dsCheckSubscription) = False Then
                                        'default user no longer has subscription
                                        iTempDefaultTeamMemberID = 0
                                        lblMessage.Text += "<br>ERROR: The Default subscription for Purchasing External RFQ does not have the General Purchasing External RFQ subscription, please submit a support requestor."
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If iTempDefaultTeamMemberID > 0 Then
                        'insert new record                                      
                        RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 139, iTempDefaultTeamMemberID)
                    End If
                End If

                iTempDefaultTeamMemberID = 0

                If cbPurchasingRequired.Checked = True Then

                    'first check for purchasing assigned to family in workflow
                    If ddWorkflowFamily.SelectedIndex > 0 Then
                        If ddPurchasingTeamMemberByFamily.SelectedIndex > 0 Then 'check to see if user selected team member first
                            iTempDefaultTeamMemberID = ddPurchasingTeamMemberByFamily.SelectedValue
                        End If

                        If iTempDefaultTeamMemberID = 0 Then  'no team member was selected for this family
                            dsDefaultApprover = commonFunctions.GetWorkFlowFamilyPurchasingAssignments(0, ddWorkflowFamily.SelectedValue)
                            If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then 'default family found
                                        iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID")
                                    End If
                                End If
                            End If
                        End If

                    End If

                    'if Family Default is not found, check make default
                    If iTempDefaultTeamMemberID = 0 Then
                        If ddWorkFlowMake.SelectedIndex > 0 Then
                            'first check for purchasing assigned to make in workflow
                            If ddPurchasingTeamMemberByMake.SelectedIndex > 0 Then
                                iTempDefaultTeamMemberID = ddPurchasingTeamMemberByMake.SelectedValue
                            End If

                            If iTempDefaultTeamMemberID = 0 Then  'no team member was selected for this make
                                dsDefaultApprover = commonFunctions.GetWorkFlowMakeAssignments(ddWorkFlowMake.SelectedValue, 0, 7)
                                If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                        If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then 'default make found
                                            iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    'if no family or no make was assigned or no purchasing team member was assigned for the family then do this
                    If iTempDefaultTeamMemberID = 0 Then
                        'get default approver
                        dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(53) 'default Purchasing
                        If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                    'check if team member still has this subscription
                                    dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 7)
                                    If commonFunctions.CheckDataSet(dsCheckSubscription) = False Then
                                        'default user no longer has subscription
                                        iTempDefaultTeamMemberID = 0
                                        lblMessage.Text += "<br>ERROR: The Default subscription for Purchasing does not have the General Purchasing subscription, please submit a support requestor."
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If iTempDefaultTeamMemberID > 0 Then
                        'insert new record                                      
                        RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 7, iTempDefaultTeamMemberID)
                    End If
                End If

                iTempDefaultTeamMemberID = 0

                If cbQualityEngineeringRequired.Checked = True Then

                    'get Quality Engineer by MAKE
                    If ddWorkFlowMake.SelectedIndex >= 0 Then
                        dsDefaultApprover = commonFunctions.GetWorkFlowMakeAssignments(ddWorkFlowMake.SelectedValue, 0, 22)
                        If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then 'default make found
                                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TeamMemberID")
                                End If
                            End If
                        End If
                    End If

                    'if not Quality Engineer found by MAKE, then get over all default
                    If iTempDefaultTeamMemberID = 0 Then
                        dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(51) 'default quality engineering

                        If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")
                                End If
                            End If
                        Else
                            lblMessage.Text += "<br>ERROR: The Default subscription for Quality Engineer does not have the General Quality Engineer subscription, please submit a support requestor."
                        End If
                    End If

                    If iTempDefaultTeamMemberID > 0 Then
                        'check if team member still has this subscription
                        dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 22)
                        If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                            'insert new record
                            RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 22, iTempDefaultTeamMemberID)
                        End If
                    End If

                    ''get default approver
                    'dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(51) 'default quality engineering
                    'If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                    '    If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    '        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                    '            iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                    '            'check if team member still has this subscription
                    '            dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 22)
                    '            If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                    '                'insert new record
                    '                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 22, iTempDefaultTeamMemberID)
                    '            Else
                    '                lblMessage.Text += "<br>ERROR: The Default subscription for Quality Engineer does not have the General Quality Engineer subscription, please submit a support requestor."
                    '            End If
                    '        End If
                    '    End If
                    'End If
                End If

                iTempDefaultTeamMemberID = 0

                If cbToolingRequired.Checked = True Then

                    ''get default approver
                    'dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(52) 'default tooling
                    'If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                    '    If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    '        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                    '            iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                    '            'check if team member still has this subscription
                    '            dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 65)
                    '            If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                    '                'insert new record
                    '                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 65, iTempDefaultTeamMemberID)
                    '            Else
                    '                lblMessage.Text += "<br>ERROR: The Default subscription for Tooling does not have the General Tooling subscription, please submit a support requestor."
                    '            End If
                    '        End If
                    '    End If
                    'End If

                    '2012-Dec-03 - pick tooling based on commodity
                    If ddWorkFlowCommodity.SelectedIndex > 0 Then
                        dsDefaultApprover = commonFunctions.GetTeamMemberByWorkFlowAssignments(0, 65, ddWorkFlowCommodity.SelectedValue, "", 0)
                        If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default commodity found
                                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")
                                End If
                            End If
                        End If
                    End If

                    If iTempDefaultTeamMemberID = 0 Then
                        'if no commodity was assigned or no tooling team member was assigned for the commodity then do this
                        'get default approver
                        dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(52) 'default tooling
                        If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                    'check if team member still has this subscription
                                    dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 65)
                                    If commonFunctions.CheckDataSet(dsCheckSubscription) = False Then
                                        'default user no longer has subscription
                                        iTempDefaultTeamMemberID = 0
                                        lblMessage.Text &= "<br />ERROR: The Default subscription for Tooling does not have the General Tooling subscription, please submit a support requestor."
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If iTempDefaultTeamMemberID > 0 Then
                        'insert new record                                                                      
                        RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 65, iTempDefaultTeamMemberID)                       
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

    Protected Sub btnCreateRFD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateRFD.Click

        Try

            ClearMessages()

            Dim ds As DataSet

            Dim iInitiatorTeamMemberID As Integer = 0
            If ddInitiator.SelectedIndex > 0 Then
                iInitiatorTeamMemberID = ddInitiator.SelectedValue
            End If

            Dim iBusinessProcessTypeID As Integer = 0
            If ddBusinessProcessType.SelectedIndex >= 0 Then
                iBusinessProcessTypeID = ddBusinessProcessType.SelectedValue
            End If

            Dim iBusinessProcessActionID As Integer = 0
            If ddBusinessProcessAction.SelectedIndex >= 0 Then
                iBusinessProcessActionID = ddBusinessProcessAction.SelectedValue
            End If

            Dim strUGNFacility As String = ""
            If ddUGNFacility.SelectedIndex >= 0 Then
                Select Case ddUGNFacility.SelectedValue
                    Case "UN", "UP", "UR", "US", "UW", "OH"
                        strUGNFacility = ddUGNFacility.SelectedValue
                End Select
            End If

            Dim strDesignationType As String = ""
            If ddDesignationType.SelectedIndex >= 0 Then
                strDesignationType = ddDesignationType.SelectedValue
            End If

            Dim iAccountManagerID As Integer = 0
            If ddAccountManager.SelectedIndex > 0 Then
                iAccountManagerID = ddAccountManager.SelectedValue
            End If

            Dim iProgramManagerID As Integer = 0
            If ddProgramManager.SelectedIndex > 0 Then
                iProgramManagerID = ddProgramManager.SelectedValue
            End If

            Dim strMake As String = ""
            If ddWorkFlowMake.SelectedIndex >= 0 Then
                strMake = ddWorkFlowMake.SelectedValue
            End If

            Dim iFamilyID As Integer = 0
            If ddWorkflowFamily.SelectedIndex > 0 Then
                iFamilyID = ddWorkflowFamily.SelectedValue
            End If

            Dim iPurchasingTeamMemberByMakeID As Integer = 0
            If ddPurchasingTeamMemberByMake.SelectedIndex >= 0 Then
                iPurchasingTeamMemberByMakeID = ddPurchasingTeamMemberByMake.SelectedValue
            End If

            Dim iPurchasingTeamMemberByFamilyID As Integer = 0
            If ddPurchasingTeamMemberByFamily.SelectedIndex >= 0 Then
                iPurchasingTeamMemberByFamilyID = ddPurchasingTeamMemberByFamily.SelectedValue
            End If

            Dim iCommodityID As Integer = 0
            If ddWorkFlowCommodity.SelectedIndex > 0 Then
                iCommodityID = ddWorkFlowCommodity.SelectedValue
            End If

            Dim iProdDevTeamMember As Integer = 0
            If ddProductDevelopmentTeamMemberByCommodity.SelectedIndex >= 0 Then
                iProdDevTeamMember = ddProductDevelopmentTeamMemberByCommodity.SelectedValue
            End If

            Dim strPriceCode As String = ""
            If ddPriceCode.SelectedIndex >= 0 Then
                strPriceCode = ddPriceCode.SelectedValue
            Else
                strPriceCode = "A"
            End If

            Dim iPriorityID As Integer = 0
            If ddPriority.SelectedIndex >= 0 Then
                iPriorityID = ddPriority.SelectedValue
            End If

            Dim dTargetPrice As Double = 0
            If txtTargetPrice.Text.Trim <> "" Then
                dTargetPrice = CType(txtTargetPrice.Text.Trim, Double)
            End If

            Dim iTargetAnnualVolume As Integer = 0
            If txtTargetAnnualVolume.Text.Trim <> "" Then
                iTargetAnnualVolume = CType(txtTargetAnnualVolume.Text.Trim, Integer)
            End If

            Dim dTargetAnnualSales As Double = 0
            If txtTargetAnnualSales.Text.Trim <> "" Then
                dTargetAnnualSales = CType(txtTargetAnnualSales.Text.Trim, Double)
            End If

            If dTargetPrice > 0 And iTargetAnnualVolume > 0 Then
                dTargetAnnualSales = dTargetPrice * iTargetAnnualVolume
            End If

            Dim bIsCostReduction As Boolean = False
            bIsCostReduction = CType(ddIsCostReduction.SelectedValue, Boolean)

            'set default status to open
            ds = RFDModule.InsertRFD(0, 1, txtRFDDesc.Text.Trim, iBusinessProcessActionID, iBusinessProcessTypeID, _
                         strDesignationType, strPriceCode, iPriorityID, _
                         txtDueDate.Text.Trim, iInitiatorTeamMemberID, iAccountManagerID, iProgramManagerID, txtImpactOnUGN.Text.Trim, _
                         dTargetPrice, iTargetAnnualVolume, _
                         dTargetAnnualSales, iCommodityID, iFamilyID, strMake, cbAffectsCostSheetOnly.Checked, cbCostingRequired.Checked, _
                         cbCustomerApprovalRequired.Checked, cbDVPRrequired.Checked, cbPackagingRequired.Checked, _
                         cbPlantControllerRequired.Checked, _
                         cbProcessRequired.Checked, cbProductDevelopmentRequired.Checked, _
                         cbPurchasingExternalRFQRequired.Checked, cbPurchasingRequired.Checked, _
                         cbQualityEngineeringRequired.Checked, cbRDrequired.Checked, _
                         cbToolingRequired.Checked, _
                         iProdDevTeamMember, iPurchasingTeamMemberByFamilyID, iPurchasingTeamMemberByMakeID, _
                         0, 0, cbCapitalRequired.Checked, "", cbMeetingRequired.Checked, bIsCostReduction)

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("NewRFDNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("NewRFDNo") > 0 Then
                        ViewState("RFDNo") = ds.Tables(0).Rows(0).Item("NewRFDNo")

                        If strUGNFacility <> "" Then
                            RFDModule.InsertRFDFacilityDept(ViewState("RFDNo"), strUGNFacility, 0)
                        End If

                        'create Approval List based on checkboxes
                        CreateApprovalList()

                        'update history
                        RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Created RFD")

                        Response.Redirect("RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo"), False)
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

    Protected Sub btnUGNFacilityEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUGNFacilityEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            HideBusinessProcessAction(True)

            HideIsCostReduction(True)

            HideUGNFacility(True)

            ShowUGNFacility()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnUGNFacilityNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUGNFacilityNext.Click

        Try

            ClearMessages()

            HideUGNFacility(True)

            ShowDesignationType()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub btnUGNFacilityPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUGNFacilityPrevious.Click

        Try

            ClearMessages()

            HideUGNFacility(False)

            If ViewState("BusinessProcessTypeID") = 1 Then 'RFQ

                ShowIsCostReduction()

            Else

                ShowBusinessProcessType()

                HideIsCostReduction(False)

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

    'Protected Sub cbContinuousLine_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbContinuousLine.CheckedChanged

    '    Try
    '        ClearMessages()

    '        AdjustApprovalRouting()

    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try
    'End Sub

    'Protected Sub cbMaterialSizeChange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbMaterialSizeChange.CheckedChanged

    '    Try

    '        ClearMessages()

    '        AdjustApprovalRouting()

    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try
    'End Sub

    Protected Sub btnProgramManagerNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProgramManagerNext.Click

        Try

            ClearMessages()

            HideProgramManager(True)

            ShowWorkflowMake()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnProgramManagerPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProgramManagerPrevious.Click

        Try
            ClearMessages()

            HideProgramManager(False)

            ShowDesignationType()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnProgramManagerEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProgramManagerEdit.Click

        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            If ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7 Then
                HideBusinessProcessAction(True)
            End If

            HideDesignationType(True)

            HideProgramManager(True)

            ShowProgramManager()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnIsCostReductionEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIsCostReductionEdit.Click
        Try

            ClearMessages()

            HideAll()

            HideBusinessProcessType(True)

            HideBusinessProcessAction(True)

            HideIsCostReduction(True)

            ShowIsCostReduction()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub btnIsCostReductionNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIsCostReductionNext.Click
        Try

            ClearMessages()

            HideIsCostReduction(True)
            ShowUGNFacility()


        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub btnIsCostReductionPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIsCostReductionPrevious.Click
        Try

            ClearMessages()

            HideIsCostReduction(False)

            ShowBusinessProcessAction()

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
