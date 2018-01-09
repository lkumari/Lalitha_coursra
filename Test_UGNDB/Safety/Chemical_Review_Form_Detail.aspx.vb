' ************************************************************************************************
'
' Name:		Safety_Chemical_Review_Form_Detail.aspx
' Purpose:	This Code Behind is for the Chemical Review Form Detail
'
' Date		Author	    
' 01/12/2010   Roderick Carlson
' 03/25/2010   Roderick Carlson     Modified - Save Approval Info when Approvers click notify
'NOT TURNED OVER TO PRODUCTION YET 02/28/2011   Roderick Carlson     Modified - Add Active Checkbox and Update Button 
' 04/11/2011   Roderick Carlson : Modified - When approver changes who should be the approver, fixed bug so new approver is saved instead of current user when the save or notify button is pressed

Partial Class Safety_Chemical_Review_Form_Detail
    Inherits System.Web.UI.Page

    Protected strEmailToAddress As String = ""
    Protected strEmailCCAddress As String = ""
    Protected Sub ClearMessages()

        Try
            lblMessage.Text = ""
            lblMessageApprovals.Text = ""
            lblMessageBottom.Text = ""

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

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

            Dim dsRoleForm As DataSet
            Dim dsSubscription As DataSet
            Dim dsTeamMember As DataSet

            Dim iRoleID As Integer = 0

            ViewState("TeamMemberID") = 0

            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            ViewState("isHR") = False
            ViewState("isRnD") = False
            ViewState("isCorpEnv") = False
            ViewState("isPlantEnv") = False
            ViewState("isPurchasing") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                ViewState("TeamMemberID") = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'If ViewState("TeamMemberID") = 530 Then
                '    'ViewState("TeamMemberID") = 614 'Giovanna.Blaylock
                '    'ViewState("TeamMemberID") = 39 'Mike.Berdine 
                '    'ViewState("TeamMemberID") = 626 'Emily.Battig 
                '    'ViewState("TeamMemberID") = 663 'Ken.DeRolf 
                '    ViewState("TeamMemberID") = 575 'Rick.Matheny 
                'End If

                'Chem Form Safey Mgr
                dsSubscription = SecurityModule.GetTMWorkHistory(ViewState("TeamMemberID"), 69)
                If commonFunctions.CheckDataset(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 69
                    ViewState("isHR") = True
                End If

                'Chem Form Rnd Mgr
                dsSubscription = SecurityModule.GetTMWorkHistory(ViewState("TeamMemberID"), 70)
                If commonFunctions.CheckDataset(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 70
                    ViewState("isRnD") = True
                End If

                'Chem Form Corp Env Mgr
                dsSubscription = SecurityModule.GetTMWorkHistory(ViewState("TeamMemberID"), 71)
                If commonFunctions.CheckDataset(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 71
                    ViewState("isCorpEnv") = True
                End If

                'Chem Form Plant Env Mgr
                dsSubscription = SecurityModule.GetTMWorkHistory(ViewState("TeamMemberID"), 72)
                If commonFunctions.CheckDataset(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 72
                    ViewState("isPlantEnv") = True
                End If

                'Chem Form Purchasing Mgr
                dsSubscription = SecurityModule.GetTMWorkHistory(ViewState("TeamMemberID"), 73)
                If commonFunctions.CheckDataset(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 73
                    ViewState("isPurchasing") = True
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(ViewState("TeamMemberID"), Nothing, 96)

                If commonFunctions.CheckDataset(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isEdit") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete

                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isEdit") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If
            End If

            ''developer testing as another team member
            'If ViewState("TeamMemberID") = 530 Then
            '    'mike berdine
            '    'ViewState("TeamMemberID") = 39
            '    'ViewState("SubscriptionID") = 70
            '    'ViewState("isRnD") = True
            '    'ViewState("isAdmin") = False
            '    'ViewState("isEdit") = True

            '    ' ''mike omery
            '    'ViewState("TeamMemberID") = 45
            '    'ViewState("SubscriptionID") = 70
            '    'ViewState("isRnD") = True
            '    'ViewState("isAdmin") = False
            '    'ViewState("isEdit") = True

            '    ''scott track
            '    'ViewState("TeamMemberID") = 621
            '    'ViewState("SubscriptionID") = 69
            '    'ViewState("isHR") = True
            '    'ViewState("isAdmin") = False
            '    'ViewState("isEdit") = True

            '    ''ken derolf
            '    'ViewState("TeamMemberID") = 663
            '    'ViewState("SubscriptionID") = 71
            '    'ViewState("isCorpEnv") = True
            '    'ViewState("isAdmin") = False
            '    'ViewState("isEdit") = True

            '    ''Adrian Way
            '    'ViewState("TeamMemberID") = 571
            '    'ViewState("SubscriptionID") = 72
            '    'ViewState("isPlantEnv") = True
            '    'ViewState("isAdmin") = False
            '    'ViewState("isEdit") = True

            '    ''vincent chavez
            '    'ViewState("TeamMemberID") = 611
            '    'ViewState("SubscriptionID") = 73
            '    'ViewState("isPurchasing") = True
            '    'ViewState("isAdmin") = False
            '    'ViewState("isEdit") = True

            '    ''Anthony Plunk
            '    'ViewState("TeamMemberID") = 169
            '    'ViewState("SubscriptionID") = 73
            '    'ViewState("isPurchasing") = True
            '    'ViewState("isAdmin") = False
            '    'ViewState("isEdit") = True

            '    ''Ron Myotte
            '    'ViewState("TeamMemberID") = 371
            '    'ViewState("SubscriptionID") = 74                
            '    'ViewState("isAdmin") = False
            '    'ViewState("isEdit") = True
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

    Protected Sub CheckSupportingDocGrid()

        If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 And ViewState("isEdit") = True And ViewState("ChemRevFormID") > 0 Then
            Dim bSupportingDocCountMaximum As Boolean = isSupportingDocCountMaximum()
            lblFileUploadLabel.Visible = Not bSupportingDocCountMaximum
            fileUploadSupportingDoc.Visible = Not bSupportingDocCountMaximum
            btnSaveUploadSupportingDocument.Visible = Not bSupportingDocCountMaximum
        End If

    End Sub

    Protected Function isSupportingDocCountMaximum() As Boolean

        Dim bMax As Boolean = False

        Try
            Dim ds As DataSet

            'check number of supporing docs
            ds = SafetyModule.GetChemicalReviewFormSupportingDocList(ViewState("ChemRevFormID"))
            If commonFunctions.CheckDataset(ds) = True Then
                If ds.Tables(0).Rows.Count >= 3 Then
                    bMax = True
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            bMax = True
        End Try

        isSupportingDocCountMaximum = bMax

    End Function

    Protected Sub menuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuTabs.MenuItemClick

        Try

            ClearMessages()

            mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)

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
            m.ContentLabel = "Chemical Review Form"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab - Safety</b> > <a href='Chemical_Review_Form_List.aspx'><b> Chemical Review Form Search </b></a> > Chemical Review Form Details "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("RnDExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''bind existing data to drop downs for selection criteria of search
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ds = SafetyModule.GetChemicalReviewFormRequestedByTeamMembers()
            If commonFunctions.CheckDataset(ds) = True Then
                ddRequestedByTeamMember.DataSource = ds
                ddRequestedByTeamMember.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
                ddRequestedByTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddRequestedByTeamMember.DataBind()
                ddRequestedByTeamMember.Items.Insert(0, "")
            End If

            ds = SafetyModule.GetChemicalReviewFormStatus(0, False)
            If commonFunctions.CheckDataset(ds) = True Then
                ddStatus.DataSource = ds
                ddStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddStatus.DataBind()
                ddStatus.Items.Insert(0, "")
            End If

            ds = SafetyModule.GetChemicalReviewFormStatus(0, True)
            If commonFunctions.CheckDataset(ds) = True Then
                ddCorpEnvStatus.DataSource = ds
                ddCorpEnvStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddCorpEnvStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddCorpEnvStatus.DataBind()
                'ddCorpEnvStatus.Items.Insert(0, "")

                ddHRSafetyStatus.DataSource = ds
                ddHRSafetyStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddHRSafetyStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddHRSafetyStatus.DataBind()
                'ddHRSafetyStatus.Items.Insert(0, "")

                ddPlantEnvStatus.DataSource = ds
                ddPlantEnvStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddPlantEnvStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddPlantEnvStatus.DataBind()
                'ddPlantEnvStatus.Items.Insert(0, "")

                ddPurchasingStatus.DataSource = ds
                ddPurchasingStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddPurchasingStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddPurchasingStatus.DataBind()
                'ddPurchasingStatus.Items.Insert(0, "")

                ddRnDStatus.DataSource = ds
                ddRnDStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddRnDStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddRnDStatus.DataBind()
                'ddRnDStatus.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            'bind existing team member list for Corp Env Mgr Approver
            ds = commonFunctions.GetTeamMemberBySubscription(71)
            If commonFunctions.CheckDataset(ds) = True Then
                ddCorpEnvTeamMember.DataSource = ds
                ddCorpEnvTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddCorpEnvTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddCorpEnvTeamMember.DataBind()
                ddCorpEnvTeamMember.Items.Insert(0, "")
            End If

            'bind existing team member list for HR Safety Approver
            ds = commonFunctions.GetTeamMemberBySubscription(69)
            If commonFunctions.CheckDataset(ds) = True Then
                ddHRSafetyTeamMember.DataSource = ds
                ddHRSafetyTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddHRSafetyTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddHRSafetyTeamMember.DataBind()
                ddHRSafetyTeamMember.Items.Insert(0, "")
            End If

            'bind existing team member list for Plant Env Approver
            ds = commonFunctions.GetTeamMemberBySubscription(72)
            If commonFunctions.CheckDataset(ds) = True Then
                ddPlantEnvTeamMember.DataSource = ds
                ddPlantEnvTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddPlantEnvTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddPlantEnvTeamMember.DataBind()
                ddPlantEnvTeamMember.Items.Insert(0, "")
            End If

            'bind existing team member list for Purchasing Approver
            ds = commonFunctions.GetTeamMemberBySubscription(73)
            If commonFunctions.CheckDataset(ds) = True Then
                ddPurchasingTeamMember.DataSource = ds
                ddPurchasingTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddPurchasingTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddPurchasingTeamMember.DataBind()
                ddPurchasingTeamMember.Items.Insert(0, "")
            End If

            'bind existing team member list for RnD Approver
            ds = commonFunctions.GetTeamMemberBySubscription(70)
            If commonFunctions.CheckDataset(ds) = True Then
                ddRnDTeamMember.DataSource = ds
                ddRnDTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddRnDTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddRnDTeamMember.DataBind()
                ddRnDTeamMember.Items.Insert(0, "")
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

    Protected Sub BindData()

        Try

            Dim ds As DataSet
            ViewState("isLocked") = True

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = SafetyModule.GetChemicalReviewForm(ViewState("ChemRevFormID"), 0, "", 0, "", "", 0, "", "", "", "", "", False, False)

            If commonFunctions.CheckDataset(ds) = True Then
                lblChemicalReviewFormIDValue.Text = ViewState("ChemRevFormID")

                If ds.Tables(0).Rows(0).Item("isLocked") IsNot System.DBNull.Value Then
                    ViewState("isLocked") = ds.Tables(0).Rows(0).Item("isLocked")
                End If

                If ds.Tables(0).Rows(0).Item("isAspectListEnv") IsNot System.DBNull.Value Then
                    cbAspectListEnv.Checked = ds.Tables(0).Rows(0).Item("isAspectListEnv")
                End If

                If ds.Tables(0).Rows(0).Item("isContainmentEng") IsNot System.DBNull.Value Then
                    cbContainmentEng.Checked = ds.Tables(0).Rows(0).Item("isContainmentEng")
                End If

                If ds.Tables(0).Rows(0).Item("isEMPEnv") IsNot System.DBNull.Value Then
                    cbEMPEnv.Checked = ds.Tables(0).Rows(0).Item("isEMPEnv")
                End If

                If ds.Tables(0).Rows(0).Item("isEnvironmentalHazard") IsNot System.DBNull.Value Then
                    cbEnvironmentalHazard.Checked = ds.Tables(0).Rows(0).Item("isEnvironmentalHazard")
                End If

                If ds.Tables(0).Rows(0).Item("isGlovesEquip") IsNot System.DBNull.Value Then
                    cbGlovesEquip.Checked = ds.Tables(0).Rows(0).Item("isGlovesEquip")
                End If

                If ds.Tables(0).Rows(0).Item("isGogglesEquip") IsNot System.DBNull.Value Then
                    cbGogglesEquip.Checked = ds.Tables(0).Rows(0).Item("isGogglesEquip")
                End If

                If ds.Tables(0).Rows(0).Item("isHealthHazard") IsNot System.DBNull.Value Then
                    cbHealthHazard.Checked = ds.Tables(0).Rows(0).Item("isHealthHazard")
                End If

                If ds.Tables(0).Rows(0).Item("isLabUsage") IsNot System.DBNull.Value Then
                    cbLabUsage.Checked = ds.Tables(0).Rows(0).Item("isLabUsage")
                End If

                If ds.Tables(0).Rows(0).Item("isMaintenanceUsage") IsNot System.DBNull.Value Then
                    cbMaintenanceUsage.Checked = ds.Tables(0).Rows(0).Item("isMaintenanceUsage")
                End If

                If ds.Tables(0).Rows(0).Item("isMSDSEnv") IsNot System.DBNull.Value Then
                    cbMSDSEnv.Checked = ds.Tables(0).Rows(0).Item("isMSDSEnv")
                End If

                If ds.Tables(0).Rows(0).Item("isOtherEng") IsNot System.DBNull.Value Then
                    cbOtherEng.Checked = ds.Tables(0).Rows(0).Item("isOtherEng")
                End If

                If ds.Tables(0).Rows(0).Item("isOtherEquip") IsNot System.DBNull.Value Then
                    cbOtherEquip.Checked = ds.Tables(0).Rows(0).Item("isOtherEquip")
                End If

                If ds.Tables(0).Rows(0).Item("isOtherHazard") IsNot System.DBNull.Value Then
                    cbOtherHazard.Checked = ds.Tables(0).Rows(0).Item("isOtherHazard")
                End If

                If ds.Tables(0).Rows(0).Item("isOtherUsage") IsNot System.DBNull.Value Then
                    cbOtherUsage.Checked = ds.Tables(0).Rows(0).Item("isOtherUsage")
                End If

                If ds.Tables(0).Rows(0).Item("isPhysicalHazard") IsNot System.DBNull.Value Then
                    cbPhysicalHazard.Checked = ds.Tables(0).Rows(0).Item("isPhysicalHazard")
                End If

                If ds.Tables(0).Rows(0).Item("isProductionUsage") IsNot System.DBNull.Value Then
                    cbProductionUsage.Checked = ds.Tables(0).Rows(0).Item("isProductionUsage")
                End If

                If ds.Tables(0).Rows(0).Item("isRespiratoryEquip") IsNot System.DBNull.Value Then
                    cbRespiratoryEquip.Checked = ds.Tables(0).Rows(0).Item("isRespiratoryEquip")
                End If

                If ds.Tables(0).Rows(0).Item("isVentilationEng") IsNot System.DBNull.Value Then
                    cbVentilationEng.Checked = ds.Tables(0).Rows(0).Item("isVentilationEng")
                End If

                ddCorpEnvStatus.SelectedValue = 1
                If ds.Tables(0).Rows(0).Item("CorpEnvStatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CorpEnvStatusID") > 0 Then
                        ddCorpEnvStatus.SelectedValue = ds.Tables(0).Rows(0).Item("CorpEnvStatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CorpEnvTeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CorpEnvTeamMemberID") > 0 Then
                        ddCorpEnvTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("CorpEnvTeamMemberID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("FlammabilityLevel") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("FlammabilityLevel") > 0 Then
                        ddFlammabilityLevel.SelectedValue = ds.Tables(0).Rows(0).Item("FlammabilityLevel")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HealthLevel") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HealthLevel") > 0 Then
                        ddHealthLevel.SelectedValue = ds.Tables(0).Rows(0).Item("HealthLevel")
                    End If
                End If

                ddHRSafetyStatus.SelectedValue = 1
                If ds.Tables(0).Rows(0).Item("HRSafetyStatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HRSafetyStatusID") > 0 Then
                        ddHRSafetyStatus.SelectedValue = ds.Tables(0).Rows(0).Item("HRSafetyStatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HRSafetyTeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HRSafetyTeamMemberID") > 0 Then
                        ddHRSafetyTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("HRSafetyTeamMemberID")
                    End If
                End If

                ddPlantEnvStatus.SelectedValue = 1
                If ds.Tables(0).Rows(0).Item("PlantEnvStatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PlantEnvStatusID") > 0 Then
                        ddPlantEnvStatus.SelectedValue = ds.Tables(0).Rows(0).Item("PlantEnvStatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("PlantEnvTeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PlantEnvTeamMemberID") > 0 Then
                        ddPlantEnvTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("PlantEnvTeamMemberID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("ProtectiveEquipmentLevel") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ProtectiveEquipmentLevel") > 0 Then
                        ddProtectiveEquipmentLevel.SelectedValue = ds.Tables(0).Rows(0).Item("ProtectiveEquipmentLevel")
                    End If
                End If

                ddPurchasingStatus.SelectedValue = 1
                If ds.Tables(0).Rows(0).Item("PurchasingStatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PurchasingStatusID") > 0 Then
                        ddPurchasingStatus.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasingStatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("PurchasingTeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("PurchasingTeamMemberID") > 0 Then
                        ddPurchasingTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasingTeamMemberID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("ReactivityLevel") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ReactivityLevel") > 0 Then
                        ddReactivityLevel.SelectedValue = ds.Tables(0).Rows(0).Item("ReactivityLevel")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("RequestedByTeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RequestedByTeamMemberID") > 0 Then
                        ddRequestedByTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("RequestedByTeamMemberID")
                    End If
                End If

                ddRnDStatus.SelectedValue = 1
                If ds.Tables(0).Rows(0).Item("RnDStatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RnDStatusID") > 0 Then
                        ddRnDStatus.SelectedValue = ds.Tables(0).Rows(0).Item("RnDStatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("RnDTeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RnDTeamMemberID") > 0 Then
                        ddRnDTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("RnDTeamMemberID")
                    End If
                End If

                ddStatus.SelectedValue = 1
                ViewState("StatusID") = 1
                If ds.Tables(0).Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("StatusID") > 0 Then
                        ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("StatusID")
                        ViewState("StatusID") = ds.Tables(0).Rows(0).Item("StatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("UGNFacility") IsNot System.DBNull.Value Then
                    ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString
                End If

                lblCorpEnvLastNotified.Text = ds.Tables(0).Rows(0).Item("CorpEnvLastNotified").ToString
                lblCorpEnvLastUpdated.Text = ds.Tables(0).Rows(0).Item("CorpEnvLastUpdated").ToString

                lblHRSafetyLastNotified.Text = ds.Tables(0).Rows(0).Item("HRSafetyLastNotified").ToString
                lblHRSafetyLastUpdated.Text = ds.Tables(0).Rows(0).Item("HRSafetyLastUpdated").ToString

                lblLastUpdatedByValue.Text = ds.Tables(0).Rows(0).Item("UpdatedBy").ToString
                lblLastUpdatedOnValue.Text = ds.Tables(0).Rows(0).Item("UpdatedOn").ToString

                lblPlantEnvLastNotified.Text = ds.Tables(0).Rows(0).Item("PlantEnvLastNotified").ToString
                lblPlantEnvLastUpdated.Text = ds.Tables(0).Rows(0).Item("PlantEnvLastUpdated").ToString

                lblPurchasingLastNotified.Text = ds.Tables(0).Rows(0).Item("PurchasingLastNotified").ToString
                lblPurchasingLastUpdated.Text = ds.Tables(0).Rows(0).Item("PurchasingLastUpdated").ToString

                lblRnDLastNotified.Text = ds.Tables(0).Rows(0).Item("RnDLastNotified").ToString
                lblRnDLastUpdated.Text = ds.Tables(0).Rows(0).Item("RnDLastUpdated").ToString

                If ds.Tables(0).Rows(0).Item("AspectType").ToString <> "" Then
                    rbAspectType.SelectedValue = ds.Tables(0).Rows(0).Item("AspectType").ToString
                Else
                    rbAspectType.SelectedValue = "N"
                End If

                txtDeptArea.Text = ds.Tables(0).Rows(0).Item("DeptArea").ToString
                txtChemicalDesc.Text = ds.Tables(0).Rows(0).Item("ChemicalDesc").ToString
                txtCorpEnvComments.Text = ds.Tables(0).Rows(0).Item("CorpEnvComments").ToString
                txtDisposalDesc.Text = ds.Tables(0).Rows(0).Item("DisposalDesc").ToString
                txtHRSafetyComments.Text = ds.Tables(0).Rows(0).Item("HRSafetyComments").ToString
                txtIncompatibleWith.Text = ds.Tables(0).Rows(0).Item("IncompatibleWith").ToString
                txtOtherEngDesc.Text = ds.Tables(0).Rows(0).Item("OtherEngDesc").ToString
                txtOtherEquipDesc.Text = ds.Tables(0).Rows(0).Item("OtherEquipDesc").ToString
                txtOtherHazardDesc.Text = ds.Tables(0).Rows(0).Item("OtherHazardDesc").ToString
                txtOtherUsageDesc.Text = ds.Tables(0).Rows(0).Item("OtherUsageDesc").ToString
                txtPlantEnvComments.Text = ds.Tables(0).Rows(0).Item("PlantEnvComments").ToString
                txtProductManufacturer.Text = ds.Tables(0).Rows(0).Item("ProductManufacturer").ToString
                txtProductName.Text = ds.Tables(0).Rows(0).Item("ProductName").ToString
                txtPurchaseFrom.Text = ds.Tables(0).Rows(0).Item("PurchaseFrom").ToString
                txtPurchasingComments.Text = ds.Tables(0).Rows(0).Item("PurchasingComments").ToString
                txtRequestDate.Text = ds.Tables(0).Rows(0).Item("RequestDate").ToString
                txtRndComments.Text = ds.Tables(0).Rows(0).Item("RndComments").ToString
                txtStorageDesc.Text = ds.Tables(0).Rows(0).Item("StorageDesc").ToString
                txtVoidComment.Text = ds.Tables(0).Rows(0).Item("VoidComment").ToString

                If ds.Tables(0).Rows(0).Item("isActive") IsNot System.DBNull.Value Then
                    cbActive.Checked = ds.Tables(0).Rows(0).Item("isActive")
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

            InitializeAllControls()
            
            If ViewState("isAdmin") = True Or ViewState("isEdit") = True Then
                btnSave.Visible = True
                cbAspectListEnv.Enabled = True
                cbContainmentEng.Enabled = True
                cbEMPEnv.Enabled = True
                cbEnvironmentalHazard.Enabled = True
                cbGlovesEquip.Enabled = True
                cbGogglesEquip.Enabled = True
                cbHealthHazard.Enabled = True
                cbLabUsage.Enabled = True
                cbMaintenanceUsage.Enabled = True
                cbMSDSEnv.Enabled = True
                cbPhysicalHazard.Enabled = True
                cbOtherEng.Enabled = True
                cbOtherEquip.Enabled = True
                cbOtherHazard.Enabled = True
                cbOtherUsage.Enabled = True
                cbProductionUsage.Enabled = True
                cbProtectiveClothingEquip.Enabled = True
                cbRespiratoryEquip.Enabled = True
                cbRespiratoryEquip.Enabled = True
                cbVentilationEng.Enabled = True

                ddFlammabilityLevel.Enabled = True
                ddHealthLevel.Enabled = True
                ddProtectiveEquipmentLevel.Enabled = True
                ddReactivityLevel.Enabled = True
                ddUGNFacility.Enabled = True

                rbAspectType.Enabled = True

                txtChemicalDesc.Enabled = True
                txtDeptArea.Enabled = True
                txtDisposalDesc.Enabled = True
                txtIncompatibleWith.Enabled = True
                txtOtherEngDesc.Enabled = True
                txtOtherEquipDesc.Enabled = True
                txtOtherHazardDesc.Enabled = True
                txtOtherUsageDesc.Enabled = True
                txtProductManufacturer.Enabled = True
                txtProductName.Enabled = True
                txtPurchaseFrom.Enabled = True
                txtStorageDesc.Enabled = True
            End If

            If ViewState("ChemRevFormID") = 0 Then
                ddRequestedByTeamMember.Enabled = ViewState("isEdit")

                imgRequestDate.Visible = ViewState("isEdit")

                txtRequestDate.Enabled = ViewState("isEdit")

                ddCorpEnvStatus.SelectedValue = 1
                ddHRSafetyStatus.SelectedValue = 1
                ddPlantEnvStatus.SelectedValue = 1
                ddPurchasingStatus.SelectedValue = 1
                ddRnDStatus.SelectedValue = 1

                lblCorpEnvLastNotified.Text = ""
                lblCorpEnvLastUpdated.Text = ""
                lblHRSafetyLastNotified.Text = ""
                lblHRSafetyLastUpdated.Text = ""
                lblPlantEnvLastNotified.Text = ""
                lblPlantEnvLastUpdated.Text = ""
                lblPurchasingLastNotified.Text = ""
                lblPurchasingLastUpdated.Text = ""
                lblRnDLastNotified.Text = ""
                lblRnDLastUpdated.Text = ""

                txtCorpEnvComments.Text = ""
                txtHRSafetyComments.Text = ""
                txtPlantEnvComments.Text = ""
                txtPurchasingComments.Text = ""
                txtRndComments.Text = ""

            Else 'ChemRevFormID > 0

                gvSupportingDoc.Visible = True
                gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = ViewState("isEdit")

                lblChemicalReviewFormIDLabel.Visible = True
                lblChemicalReviewFormIDValue.Visible = True
                lblFileUploadLabel.Visible = ViewState("isEdit")
                lblMaxNote.Visible = ViewState("isEdit")
                lblLastUpdatedByLabel.Visible = True
                lblLastUpdatedByValue.Visible = True
                lblLastUpdatedOnLabel.Visible = True
                lblLastUpdatedOnValue.Visible = True
                lblOverallStatus.Visible = True

                ddStatus.Visible = True

                fileUploadSupportingDoc.Visible = ViewState("isEdit")

                btnSaveApprovers.Visible = ViewState("isEdit")
                btnSaveUploadSupportingDocument.Visible = ViewState("isEdit")
                btnCopy.Visible = ViewState("isEdit")
                btnPreview.Visible = True
                btnPreviewBottom.Visible = True

                'setup preview buttons
                Dim strPreviewClientScript As String = "javascript:void(window.open('Chemical_Review_Form_Preview.aspx?ChemRevFormID=" & ViewState("ChemRevFormID") & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=600,width=950,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                btnPreview.Attributes.Add("onclick", strPreviewClientScript)
                btnPreviewBottom.Attributes.Add("onclick", strPreviewClientScript)

                menuTabs.Items(1).Enabled = True

                'If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 Then 'not complete and not voided
                If ViewState("isLocked") = False Then 'not complete and not voided

                    txtVoidComment.Enabled = ViewState("isEdit")
                    btnVoid.Visible = ViewState("isEdit")


                    ddHRSafetyTeamMember.Enabled = ViewState("isEdit")
                    ddRnDTeamMember.Enabled = ViewState("isEdit")
                    ddCorpEnvTeamMember.Enabled = ViewState("isEdit")
                    ddPlantEnvTeamMember.Enabled = ViewState("isEdit")
                    ddPurchasingTeamMember.Enabled = ViewState("isEdit")

                    If ddRequestedByTeamMember.SelectedIndex > 0 Then
                        'initiator is connected
                        If ViewState("TeamMemberID") = ddRequestedByTeamMember.SelectedValue Or ViewState("isAdmin") = True Then

                            If lblCorpEnvLastNotified.Text = "" And lblHRSafetyLastNotified.Text = "" _
                                And lblPlantEnvLastNotified.Text = "" And lblPurchasingLastNotified.Text = "" _
                                And lblRnDLastNotified.Text = "" _
                                And ViewState("TeamMemberID") = ddRequestedByTeamMember.SelectedValue Then
                                btnNotify.Visible = ViewState("isEdit")
                            Else
                                btnNotify.Visible = ViewState("isAdmin")
                            End If

                            ddRequestedByTeamMember.Enabled = ViewState("isEdit")

                            imgRequestDate.Visible = ViewState("isEdit")

                            txtRequestDate.Enabled = ViewState("isEdit")
                        End If

                        If lblCorpEnvLastNotified.Text <> "" Or lblHRSafetyLastNotified.Text <> "" _
                                Or lblPlantEnvLastNotified.Text <> "" Or lblPurchasingLastNotified.Text <> "" _
                                Or lblRnDLastNotified.Text <> "" Then

                            ' approver is connected - allow users to have multiple roles
                            If ViewState("isHR") = True Then '69 'Chem Form Safey Mgr
                                btnHRSafetySave.Visible = ViewState("isEdit")
                                btnHRSafetyNotify.Visible = ViewState("isEdit")

                                ddHRSafetyTeamMember.Enabled = ViewState("isEdit")
                                ddHRSafetyStatus.Enabled = ViewState("isEdit")

                                txtHRSafetyComments.Enabled = ViewState("isEdit")
                            End If

                            If ViewState("isRnD") = True Then '70  'Chem Form Rnd Mgr
                                btnRnDSave.Visible = ViewState("isEdit")
                                btnRnDNotify.Visible = ViewState("isEdit")

                                ddRnDTeamMember.Enabled = ViewState("isEdit")
                                ddRnDStatus.Enabled = ViewState("isEdit")

                                txtRndComments.Enabled = ViewState("isEdit")
                            End If

                            If ViewState("isCorpEnv") = True Then '71 'Chem Form Corp Env Mgr
                                btnCorpEnvSave.Visible = ViewState("isEdit")
                                btnCorpEnvNotify.Visible = ViewState("isEdit")

                                ddCorpEnvTeamMember.Enabled = ViewState("isEdit")
                                ddCorpEnvStatus.Enabled = ViewState("isEdit")

                                txtCorpEnvComments.Enabled = ViewState("isEdit")
                            End If

                            If ViewState("isPlantEnv") = True Then '72 'Chem Form Plant Env Mgr
                                btnPlantEnvSave.Visible = ViewState("isEdit")
                                btnPlantEnvNotify.Visible = ViewState("isEdit")

                                ddPlantEnvTeamMember.Enabled = ViewState("isEdit")
                                ddPlantEnvStatus.Enabled = ViewState("isEdit")

                                txtPlantEnvComments.Enabled = ViewState("isEdit")
                            End If

                            If ViewState("isPurchasing") = True Then '73 'Chem Form Purchasing Mgr
                                btnPurchasingSave.Visible = ViewState("isEdit")
                                btnPurchasingNotify.Visible = ViewState("isEdit")

                                ddPurchasingTeamMember.Enabled = ViewState("isEdit")
                                ddPurchasingStatus.Enabled = ViewState("isEdit")

                                txtPurchasingComments.Enabled = ViewState("isEdit")
                            End If

                        End If

                    End If
                End If
            End If

            'If ViewState("StatusID") = 3 Or ViewState("StatusID") = 4 Then
            If ViewState("isLocked") = True Then
                DisableUpdateControls()

                If txtVoidComment.Text.Trim <> "" Then
                    lblVoidComment.Visible = True
                    txtVoidComment.Visible = True
                    btnPreview.Visible = False
                    btnPreviewBottom.Visible = False
                End If

                If ViewState("StatusID") <> 4 Then
                    cbActive.Visible = True
                    cbActive.Enabled = ViewState("isEdit")

                    btnUpdateActive.Visible = ViewState("isEdit")
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
    Protected Sub HandleMultilineFields()

        Try

            txtChemicalDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtChemicalDesc.Attributes.Add("onkeyup", "return tbCount(" + lblChemicalDescCharCount.ClientID + ");")
            txtChemicalDesc.Attributes.Add("maxLength", "400")

            txtStorageDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtStorageDesc.Attributes.Add("onkeyup", "return tbCount(" + lblStorageDescCharCount.ClientID + ");")
            txtStorageDesc.Attributes.Add("maxLength", "400")

            txtDisposalDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtDisposalDesc.Attributes.Add("onkeyup", "return tbCount(" + lblDisposalDescCharCount.ClientID + ");")
            txtDisposalDesc.Attributes.Add("maxLength", "400")

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub HanldeButtons()

        Try

            btnVoid.Attributes.Add("onclick", "if(confirm('Are you sure that you want to void this form?.  ')){}else{return false}")

            btnCorpEnvNotify.Attributes.Add("onclick", "if(confirm('Are you sure that you want to notify all approvers of updates. Notification is optional. Clicking the save button is the only requirement. ')){}else{return false}")
            btnHRSafetyNotify.Attributes.Add("onclick", "if(confirm('Are you sure that you want to notify all approvers of updates. Notification is optional. Clicking the save button is the only requirement. ')){}else{return false}")
            btnPlantEnvNotify.Attributes.Add("onclick", "if(confirm('Are you sure that you want to notify all approvers of updates. Notification is optional. Clicking the save button is the only requirement. ')){}else{return false}")
            btnPurchasingNotify.Attributes.Add("onclick", "if(confirm('Are you sure that you want to notify all approvers of updates. Notification is optional. Clicking the save button is the only requirement. ')){}else{return false}")
            btnRnDNotify.Attributes.Add("onclick", "if(confirm('Are you sure that you want to notify all approvers of updates. Notification is optional. Clicking the save button is the only requirement. ')){}else{return false}")

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

            CheckRights()

            'clear crystal reports
            SafetyModule.CleanChemicalReviewFormCrystalReports()

            If Not Page.IsPostBack Then

                BindCriteria()

                ViewState("ChemRevFormID") = 0
                If HttpContext.Current.Request.QueryString("ChemRevFormID") <> "" Then
                    ViewState("ChemRevFormID") = HttpContext.Current.Request.QueryString("ChemRevFormID")

                    If ViewState("ChemRevFormID") > 0 Then                        
                        BindData()
                    End If

                End If

                HandleMultilineFields()

                HanldeButtons()

            End If

            EnableControls()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub
    Protected Sub GetChemicalReviewFormApprovers()

        Try
            Dim dsSubscription As DataSet                  

            Dim strUGNFacility As String = ""
            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            'Chem Form Safey Mgr
            'If ddHRSafetyTeamMember.SelectedIndex <= 0 Then
            'check first by ugnfacility
            dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(69, strUGNFacility)
            If commonFunctions.CheckDataset(dsSubscription) = True Then
                If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                        ddHRSafetyTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                    End If
                End If
            Else  'set default if by ugn facility not found
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(69, "")
                If commonFunctions.CheckDataset(dsSubscription) = True Then

                    'set default to first - then look for ugn facility specific
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                        If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                            ddHRSafetyTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                        End If
                    End If
                End If
            End If
            'End If

            'Chem Form Rnd Mgr
            'If ddRnDTeamMember.SelectedIndex <= 0 Then
            'check first by ugnfacility
            dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(70, strUGNFacility)
            If commonFunctions.CheckDataset(dsSubscription) = True Then
                If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                        ddRnDTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                    End If
                End If
            Else  'set default if by ugn facility not found
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(70, "")
                If commonFunctions.CheckDataset(dsSubscription) = True Then

                    'set default to first - then look for ugn facility specific
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                        If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                            ddRnDTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                        End If
                    End If
                End If
            End If
            'End If


            'Chem Form Corp Env Mgr
            'If ddCorpEnvTeamMember.SelectedIndex <= 0 Then
            'check first by ugnfacility
            dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(71, strUGNFacility)
            If commonFunctions.CheckDataset(dsSubscription) = True Then
                If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                        ddCorpEnvTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                    End If
                End If
            Else  'set default if by ugn facility not found
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(71, "")
                If commonFunctions.CheckDataset(dsSubscription) = True Then

                    'set default to first - then look for ugn facility specific
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                        If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                            ddCorpEnvTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                        End If
                    End If
                End If
            End If
            'End If

            'Chem Form Plant Env Mgr
            'If ddPlantEnvTeamMember.SelectedIndex <= 0 Then
            'check first by ugnfacility
            dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(72, strUGNFacility)
            If commonFunctions.CheckDataset(dsSubscription) = True Then
                If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                        ddPlantEnvTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                    End If
                End If
            Else  'set default if by ugn facility not found
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(72, "")
                If commonFunctions.CheckDataset(dsSubscription) = True Then

                    'set default to first - then look for ugn facility specific
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                        If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                            ddPlantEnvTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                        End If
                    End If
                End If
            End If
            'End If

            'Chem Form Purchasing Mgr
            'If ddPurchasingTeamMember.SelectedIndex <= 0 Then
            'check first by ugnfacility
            dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(73, strUGNFacility)
            If commonFunctions.CheckDataset(dsSubscription) = True Then
                If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                        ddPurchasingTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                    End If
                End If
            Else  'set default if by ugn facility not found
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(73, "")
                If commonFunctions.CheckDataset(dsSubscription) = True Then

                    'set default to first - then look for ugn facility specific
                    If dsSubscription.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                        If dsSubscription.Tables(0).Rows(0).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(0).Item("WorkStatus") = True Then
                            ddPurchasingTeamMember.SelectedValue = dsSubscription.Tables(0).Rows(0).Item("TMID")
                        End If
                    End If
                End If
            End If
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
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveApprovers.Click, btnUpdateActive.Click

        Try
            ClearMessages()

            Dim dsNewChemRevForm As DataSet

            Dim iCorpEnvTeammemberID As Integer = 0
            Dim iFlammabilityLevel As Integer = 0
            Dim iHealthLevel As Integer = 0
            Dim iHRSafetyTeamMemberID As Integer = 0
            Dim iPlantEnvTeammemberID As Integer = 0
            Dim iProtectiveEquipmentLevel As Integer = 0
            Dim iPurchasingTeamMemberID As Integer = 0
            Dim iReactivityLevel As Integer = 0
            Dim iRequestedByTeamMemberID As Integer = 0
            Dim iRndTeamMemberID As Integer = 0

            Dim strAspectType As String = ""
            Dim strUGNFacility As String = ""

            If ddCorpEnvTeamMember.SelectedIndex > 0 Then
                iCorpEnvTeammemberID = ddCorpEnvTeamMember.SelectedValue
            End If

            If ddFlammabilityLevel.SelectedIndex > 0 Then
                iFlammabilityLevel = ddFlammabilityLevel.SelectedValue
            End If

            If ddHealthLevel.SelectedIndex > 0 Then
                iHealthLevel = ddHealthLevel.SelectedValue
            End If

            If ddHRSafetyTeamMember.SelectedIndex > 0 Then
                iHRSafetyTeamMemberID = ddHRSafetyTeamMember.SelectedValue
            End If

            If ddPlantEnvTeamMember.SelectedIndex > 0 Then
                iPlantEnvTeammemberID = ddPlantEnvTeamMember.SelectedValue
            End If

            If ddProtectiveEquipmentLevel.SelectedIndex > 0 Then
                iProtectiveEquipmentLevel = ddProtectiveEquipmentLevel.SelectedValue
            End If

            If ddPurchasingTeamMember.SelectedIndex > 0 Then
                iPurchasingTeamMemberID = ddPurchasingTeamMember.SelectedValue
            End If

            If ddReactivityLevel.SelectedIndex > 0 Then
                iReactivityLevel = ddReactivityLevel.SelectedValue
            End If

            If ddRequestedByTeamMember.SelectedIndex > 0 Then
                iRequestedByTeamMemberID = ddRequestedByTeamMember.SelectedValue
            End If

            If ddRnDTeamMember.SelectedIndex > 0 Then
                iRndTeamMemberID = ddRnDTeamMember.SelectedValue
            End If

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            strAspectType = rbAspectType.SelectedValue

            If ViewState("ChemRevFormID") > 0 Then
                'update
                SafetyModule.UpdateChemicalReviewForm(ViewState("ChemRevFormID"), strUGNFacility, iRequestedByTeamMemberID, txtRequestDate.Text.Trim, _
                txtProductName.Text.Trim, txtProductManufacturer.Text.Trim, txtPurchaseFrom.Text.Trim, txtDeptArea.Text.Trim, txtChemicalDesc.Text.Trim, _
                cbProductionUsage.Checked, cbLabUsage.Checked, cbMaintenanceUsage.Checked, cbOtherUsage.Checked, txtOtherUsageDesc.Text, _
                iHealthLevel, iFlammabilityLevel, iReactivityLevel, iProtectiveEquipmentLevel, cbPhysicalHazard.Checked, cbHealthHazard.Checked, _
                cbEnvironmentalHazard.Checked, cbOtherHazard.Checked, txtOtherHazardDesc.Text.Trim, cbGlovesEquip.Checked, cbGogglesEquip.Checked, _
                cbRespiratoryEquip.Checked, cbProtectiveClothingEquip.Checked, cbOtherEquip.Checked, txtOtherEquipDesc.Text, cbVentilationEng.Checked, _
                cbContainmentEng.Checked, cbOtherEng.Checked, txtOtherEngDesc.Text, txtIncompatibleWith.Text.Trim, txtStorageDesc.Text.Trim, _
                txtDisposalDesc.Text.Trim, cbMSDSEnv.Checked, cbAspectListEnv.Checked, cbEMPEnv.Checked, strAspectType, iRndTeamMemberID, _
                iHRSafetyTeamMemberID, iCorpEnvTeammemberID, iPlantEnvTeammemberID, iPurchasingTeamMemberID, cbActive.Checked)

                lblMessage.Text = "<br>Record updated successfully."
            Else
                'insert
                dsNewChemRevForm = SafetyModule.InsertChemicalReviewForm(strUGNFacility, iRequestedByTeamMemberID, txtRequestDate.Text.Trim, _
                txtProductName.Text.Trim, txtProductManufacturer.Text.Trim, txtPurchaseFrom.Text.Trim, txtDeptArea.Text.Trim, txtChemicalDesc.Text.Trim, _
                cbProductionUsage.Checked, cbLabUsage.Checked, cbMaintenanceUsage.Checked, cbOtherUsage.Checked, txtOtherUsageDesc.Text, _
                iHealthLevel, iFlammabilityLevel, iReactivityLevel, iProtectiveEquipmentLevel, cbPhysicalHazard.Checked, cbHealthHazard.Checked, _
                cbEnvironmentalHazard.Checked, cbOtherHazard.Checked, txtOtherHazardDesc.Text.Trim, cbGlovesEquip.Checked, cbGogglesEquip.Checked, _
                cbRespiratoryEquip.Checked, cbProtectiveClothingEquip.Checked, cbOtherEquip.Checked, txtOtherEquipDesc.Text, cbVentilationEng.Checked, _
                cbContainmentEng.Checked, cbOtherEng.Checked, txtOtherEngDesc.Text, txtIncompatibleWith.Text.Trim, txtStorageDesc.Text.Trim, _
                txtDisposalDesc.Text.Trim, cbMSDSEnv.Checked, cbAspectListEnv.Checked, cbEMPEnv.Checked, strAspectType, iRndTeamMemberID, _
                iHRSafetyTeamMemberID, iCorpEnvTeammemberID, iPlantEnvTeammemberID, iPurchasingTeamMemberID)

                If commonFunctions.CheckDataSet(dsNewChemRevForm) = True Then
                    If dsNewChemRevForm.Tables(0).Rows(0).Item("NewChemRevFormID") IsNot System.DBNull.Value Then
                        If dsNewChemRevForm.Tables(0).Rows(0).Item("NewChemRevFormID") > 0 Then
                            ViewState("ChemRevFormID") = dsNewChemRevForm.Tables(0).Rows(0).Item("NewChemRevFormID")
                            lblChemicalReviewFormIDValue.Text = ViewState("ChemRevFormID")
                            ddStatus.SelectedValue = 1
                            ViewState("StatusID") = 1

                            lblMessage.Text = "<br>The record was created successfully."
                        End If
                    End If
                End If
            End If

            BindData()

            EnableControls()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApprovals.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddUGNFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUGNFacility.SelectedIndexChanged

        Try
            ClearMessages()

            GetChemicalReviewFormApprovers()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub BuildEmailNotificationList()

        Try

            Dim dsTeamMember As DataSet
            Dim dsBackup As DataSet

            Dim iRnDTeamMemberID As Integer = 0
            Dim iRnDBackupTeamMemberID As Integer = 0
            Dim strRnDEmail As String = ""
            Dim strRnDBackupEmail As String = ""

            Dim iHRSafetyTeamMemberID As Integer = 0
            Dim iHRSafetyBackupTeamMemberID As Integer = 0
            Dim strHRSafetyEmail As String = ""
            Dim strHRSafetyBackupEmail As String = ""

            Dim iCorpEnvTeamMemberID As Integer = 0
            Dim iCorpEnvBackupTeamMemberID As Integer = 0
            Dim strCorpEnvEmail As String = ""
            Dim strCorpEnvBackupEmail As String = ""

            Dim iPlantEnvTeamMemberID As Integer = 0
            Dim iPlantEnvBackupTeamMemberID As Integer = 0
            Dim strPlantEnvEmail As String = ""
            Dim strPlantEnvBackupEmail As String = ""

            Dim iPurchasingTeamMemberID As Integer = 0
            Dim iPurchasingBackupTeamMemberID As Integer = 0
            Dim strPurchasingEmail As String = ""
            Dim strPurchasingBackupEmail As String = ""

            strEmailToAddress = ""
            strEmailCCAddress = ""

            'Rnd
            If ddRnDTeamMember.SelectedIndex > 0 Then
                iRnDTeamMemberID = ddRnDTeamMember.SelectedValue
                dsTeamMember = SecurityModule.GetTeamMember(iRnDTeamMemberID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataset(dsTeamMember) = True Then
                    strRnDEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                    'make sure backup team member is not already in either recipient list
                    If InStr(strEmailCCAddress, strRnDEmail) <= 0 And InStr(strEmailToAddress, strRnDEmail) <= 0 Then
                        If strEmailToAddress.Trim <> "" Then
                            strEmailToAddress += ";"
                        End If

                        strEmailToAddress += strRnDEmail
                    End If

                    'get backup if out
                    dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iRnDTeamMemberID, 70) 'Chem Form Rnd Mgr
                    If commonFunctions.CheckDataset(dsBackup) = True Then
                        If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                iRnDBackupTeamMemberID = dsBackup.Tables(0).Rows(0).Item("BackupID")
                                strRnDBackupEmail = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString

                                'make sure backup team member is not already in either recipient list
                                If InStr(strEmailCCAddress, strRnDBackupEmail) <= 0 And InStr(strEmailToAddress, strRnDBackupEmail) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress += ";"
                                    End If

                                    strEmailCCAddress += strRnDBackupEmail
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            'HR Safety
            If ddHRSafetyTeamMember.SelectedIndex > 0 Then
                iHRSafetyTeamMemberID = ddHRSafetyTeamMember.SelectedValue
                dsTeamMember = SecurityModule.GetTeamMember(iHRSafetyTeamMemberID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataset(dsTeamMember) = True Then
                    strHRSafetyEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                    'make sure backup team member is not already in either recipient list
                    If InStr(strEmailCCAddress, strHRSafetyEmail) <= 0 And InStr(strEmailToAddress, strHRSafetyEmail) <= 0 Then
                        If strEmailToAddress.Trim <> "" Then
                            strEmailToAddress += ";"
                        End If

                        strEmailToAddress += strHRSafetyEmail
                    End If

                    'get backup if out
                    dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iHRSafetyTeamMemberID, 69) 'Chem Form HR Safety Mgr
                    If commonFunctions.CheckDataset(dsBackup) = True Then
                        If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                iHRSafetyBackupTeamMemberID = dsBackup.Tables(0).Rows(0).Item("BackupID")
                                strHRSafetyBackupEmail = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString

                                'make sure backup team member is not already in either recipient list
                                If InStr(strEmailCCAddress, strHRSafetyBackupEmail) <= 0 And InStr(strEmailToAddress, strHRSafetyBackupEmail) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress += ";"
                                    End If

                                    strEmailCCAddress += strHRSafetyBackupEmail
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            'Corp Env
            If ddCorpEnvTeamMember.SelectedIndex > 0 Then
                iCorpEnvTeamMemberID = ddCorpEnvTeamMember.SelectedValue
                dsTeamMember = SecurityModule.GetTeamMember(iCorpEnvTeamMemberID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataset(dsTeamMember) = True Then
                    strCorpEnvEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                    'make sure backup team member is not already in either recipient list
                    If InStr(strEmailCCAddress, strCorpEnvEmail) <= 0 And InStr(strEmailToAddress, strCorpEnvEmail) <= 0 Then
                        If strEmailToAddress.Trim <> "" Then
                            strEmailToAddress += ";"
                        End If

                        strEmailToAddress += strCorpEnvEmail
                    End If

                    'get backup if out
                    dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iCorpEnvTeamMemberID, 71) 'Chem Form CorpEnv Mgr
                    If commonFunctions.CheckDataset(dsBackup) = True Then
                        If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                iCorpEnvBackupTeamMemberID = dsBackup.Tables(0).Rows(0).Item("BackupID")
                                strCorpEnvBackupEmail = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString

                                'make sure backup team member is not already in either recipient list
                                If InStr(strEmailCCAddress, strCorpEnvBackupEmail) <= 0 And InStr(strEmailToAddress, strCorpEnvBackupEmail) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress += ";"
                                    End If

                                    strEmailCCAddress += strCorpEnvBackupEmail
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            'Plant Env
            If ddPlantEnvTeamMember.SelectedIndex > 0 Then
                iPlantEnvTeamMemberID = ddPlantEnvTeamMember.SelectedValue
                dsTeamMember = SecurityModule.GetTeamMember(iPlantEnvTeamMemberID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataset(dsTeamMember) = True Then
                    strPlantEnvEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                    'make sure backup team member is not already in either recipient list
                    If InStr(strEmailCCAddress, strPlantEnvEmail) <= 0 And InStr(strEmailToAddress, strPlantEnvEmail) <= 0 Then
                        If strEmailToAddress.Trim <> "" Then
                            strEmailToAddress += ";"
                        End If

                        strEmailToAddress += strPlantEnvEmail
                    End If

                    'get backup if out
                    dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iPlantEnvTeamMemberID, 72) 'Chem Form Plant Mgr
                    If commonFunctions.CheckDataset(dsBackup) = True Then
                        If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                iPlantEnvBackupTeamMemberID = dsBackup.Tables(0).Rows(0).Item("BackupID")
                                strPlantEnvBackupEmail = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString

                                'make sure backup team member is not already in either recipient list
                                If InStr(strEmailCCAddress, strPlantEnvBackupEmail) <= 0 And InStr(strEmailToAddress, strPlantEnvBackupEmail) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress += ";"
                                    End If

                                    strEmailCCAddress += strPlantEnvBackupEmail
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            'Purchasing
            If ddPurchasingTeamMember.SelectedIndex > 0 Then
                iPurchasingTeamMemberID = ddPurchasingTeamMember.SelectedValue
                dsTeamMember = SecurityModule.GetTeamMember(iPurchasingTeamMemberID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataset(dsTeamMember) = True Then
                    strPurchasingEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                    'make sure backup team member is not already in either recipient list
                    If InStr(strEmailCCAddress, strPurchasingEmail) <= 0 And InStr(strEmailToAddress, strPurchasingEmail) <= 0 Then
                        If strEmailToAddress.Trim <> "" Then
                            strEmailToAddress += ";"
                        End If

                        strEmailToAddress += strPurchasingEmail
                    End If

                    'get backup if out
                    dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iPurchasingTeamMemberID, 73) 'Chem Form Purchasing
                    If commonFunctions.CheckDataset(dsBackup) = True Then
                        If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                iPurchasingBackupTeamMemberID = dsBackup.Tables(0).Rows(0).Item("BackupID")
                                strPurchasingBackupEmail = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString

                                'make sure backup team member is not already in either recipient list
                                If InStr(strEmailCCAddress, strPurchasingBackupEmail) <= 0 And InStr(strEmailToAddress, strPurchasingBackupEmail) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress += ";"
                                    End If

                                    strEmailCCAddress += strPurchasingBackupEmail
                                End If
                            End If
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

    Protected Sub btnNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotify.Click

        Try

            ClearMessages()

            BuildEmailNotificationList()

            UpdateOverallStatus(False)

            If SendRequestedByNotificationToAllRolesEmail() = True Then
                'lblMessage.Text = "<br>Notifications sent"
                'Else
                'lblMessage.Text = "<br>Notifications not sent"
            End If

            'update notification sent columns
            SafetyModule.UpdateChemicalReviewFormNotification(ViewState("ChemRevFormID"))

            BindData()

            EnableControls()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub UpdateOverallStatus(ByVal NotifyApprovers As Boolean)

        Try
            'see notes below about approval
            'if anyone has rejected, then the entire form is rejected. The HR Safety Manager and Requested By Team Member are notified.

            Dim iRnDStatusID As Integer = 0
            Dim iHRSafetyStatusID As Integer = 0
            Dim iCorpEnvStatusID As Integer = 0
            Dim iOverallStatusID As Integer = 1
            Dim iPlantEnvStatusID As Integer = 0
            Dim iPurchasingStatusID As Integer = 0

            If ddRnDStatus.SelectedIndex >= 0 Then
                iRnDStatusID = ddRnDStatus.SelectedValue
            End If

            If ddHRSafetyStatus.SelectedIndex >= 0 Then
                iHRSafetyStatusID = ddHRSafetyStatus.SelectedValue
            End If

            If ddCorpEnvStatus.SelectedIndex >= 0 Then
                iCorpEnvStatusID = ddCorpEnvStatus.SelectedValue
            End If

            If ddPlantEnvStatus.SelectedIndex >= 0 Then
                iPlantEnvStatusID = ddPlantEnvStatus.SelectedValue
            End If

            If ddPurchasingStatus.SelectedIndex >= 0 Then
                iPurchasingStatusID = ddPurchasingStatus.SelectedValue
            End If

            If iRnDStatusID = 2 Or iRnDStatusID = 3 Or iRnDStatusID = 6 Or iRnDStatusID = 7 Then
                iOverallStatusID = 2 ' In Process
            End If

            If iRnDStatusID = 5 Then
                iOverallStatusID = 5 ' Rejected
            End If

            'if not rejected yet then check more
            If iOverallStatusID <> 5 Then

                If iHRSafetyStatusID = 2 Or iHRSafetyStatusID = 3 Or iHRSafetyStatusID = 6 Or iHRSafetyStatusID = 7 Then
                    iOverallStatusID = 2 ' In Process
                End If

                If iHRSafetyStatusID = 5 Then
                    iOverallStatusID = 5 ' Rejected
                End If

                'if not rejected yet then check more
                If iOverallStatusID <> 5 Then

                    If iCorpEnvStatusID = 2 Or iCorpEnvStatusID = 3 Or iCorpEnvStatusID = 6 Or iCorpEnvStatusID = 7 Then
                        iOverallStatusID = 2 ' In Process
                    End If

                    If iCorpEnvStatusID = 5 Then
                        iOverallStatusID = 5 ' Rejected
                    End If

                    'if not rejected yet then check more
                    If iOverallStatusID <> 5 Then

                        If iPlantEnvStatusID = 2 Or iPlantEnvStatusID = 3 Or iPlantEnvStatusID = 6 Or iPlantEnvStatusID = 7 Then
                            iOverallStatusID = 2 ' In Process
                        End If

                        If iPlantEnvStatusID = 5 Then
                            iOverallStatusID = 5 ' Rejected
                        End If

                        'if not rejected yet then check more
                        If iOverallStatusID <> 5 Then

                            If iPurchasingStatusID = 2 Or iPurchasingStatusID = 3 Or iPurchasingStatusID = 6 Or iPurchasingStatusID = 7 Then
                                iOverallStatusID = 2 ' In Process
                            End If

                            If iPurchasingStatusID = 5 Then
                                iOverallStatusID = 5 ' Rejected
                            End If

                        End If
                    End If
                End If
            End If

            'if all stages complete then overall status is complete
            'if no one has rejected the form and the HR Safety Manager and at least one Environmental Engineer (Coporate or Plant) must approve for the form to be approved
            'AND it has been more than 7 calendar days since original creation, then the form is approved
            'All involved parties will be notified.
            'If iRnDStatusID = 3 And iHRSafetyStatusID = 3 And iCorpEnvStatusID = 3 And iPlantEnvStatusID = 3 And iPurchasingStatusID = 3 Then
            If iRnDStatusID <> 5 And iHRSafetyStatusID = 3 And (iCorpEnvStatusID = 3 Or iPlantEnvStatusID = 3) And iCorpEnvStatusID <> 5 And iPlantEnvStatusID <> 5 And iPurchasingStatusID <> 5 Then
                iOverallStatusID = 3

                'notify initiator that all has been approved
                If NotifyApprovers = True And ViewState("StatusID") <> 3 And ViewState("StatusID") <> 5 Then
                    If SendFormApprovedEmail() = True Then
                        'lblMessage.Text = "<br>Notifications were sent to the Requested By Team Member and HR Safety Manager that the form has been approved by all."
                    End If
                End If
            End If

            ddStatus.SelectedValue = iOverallStatusID
            ViewState("StatusID") = iOverallStatusID

            SafetyModule.UpdateChemicalReviewFormOverallStatus(ViewState("ChemRevFormID"), iOverallStatusID)

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub InitializeAllControls()

        Try
            btnCopy.Visible = False
            btnPreview.Visible = False
            btnPreviewBottom.Visible = False          

            lblChemicalReviewFormIDLabel.Visible = False
            lblChemicalReviewFormIDValue.Visible = False
            lblLastUpdatedByLabel.Visible = False
            lblLastUpdatedByValue.Visible = False
            lblLastUpdatedOnLabel.Visible = False
            lblLastUpdatedOnValue.Visible = False
            lblOverallStatus.Visible = False

            menuTabs.Items(1).Enabled = False

            DisableUpdateControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub DisableUpdateControls()

        btnCorpEnvNotify.Visible = False
        btnCorpEnvSave.Visible = False
        btnHRSafetySave.Visible = False
        btnHRSafetyNotify.Visible = False
        btnNotify.Visible = False
        btnPlantEnvNotify.Visible = False
        btnPlantEnvSave.Visible = False
        btnPurchasingNotify.Visible = False
        btnPurchasingSave.Visible = False
        btnRnDNotify.Visible = False
        btnRnDSave.Visible = False
        btnSave.Visible = False
        btnSaveApprovers.Visible = False
        btnSaveUploadSupportingDocument.Visible = False
        btnVoid.Visible = False
        btnVoidCancel.Visible = False

        cbAspectListEnv.Enabled = False
        cbContainmentEng.Enabled = False
        cbEMPEnv.Enabled = False
        cbEnvironmentalHazard.Enabled = False
        cbGlovesEquip.Enabled = False
        cbGogglesEquip.Enabled = False
        cbHealthHazard.Enabled = False
        cbLabUsage.Enabled = False
        cbMaintenanceUsage.Enabled = False
        cbMSDSEnv.Enabled = False
        cbPhysicalHazard.Enabled = False
        cbOtherEng.Enabled = False
        cbOtherEquip.Enabled = False
        cbOtherHazard.Enabled = False
        cbOtherUsage.Enabled = False
        cbProductionUsage.Enabled = False
        cbProtectiveClothingEquip.Enabled = False
        cbRespiratoryEquip.Enabled = False
        cbRespiratoryEquip.Enabled = False
        cbVentilationEng.Enabled = False

        ddCorpEnvStatus.Enabled = False
        ddCorpEnvTeamMember.Enabled = False
        ddFlammabilityLevel.Enabled = False
        ddHealthLevel.Enabled = False
        ddHRSafetyStatus.Enabled = False
        ddHRSafetyTeamMember.Enabled = False
        ddPlantEnvStatus.Enabled = False
        ddPlantEnvTeamMember.Enabled = False
        ddProtectiveEquipmentLevel.Enabled = False
        ddPurchasingStatus.Enabled = False
        ddPurchasingTeamMember.Enabled = False
        ddReactivityLevel.Enabled = False
        ddRequestedByTeamMember.Enabled = False
        ddRnDStatus.Enabled = False
        ddRnDTeamMember.Enabled = False
        ddStatus.Enabled = False
        ddUGNFacility.Enabled = False

        fileUploadSupportingDoc.Visible = False

        gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = False

        imgRequestDate.Visible = False

        lblFileUploadLabel.Visible = False
        lblMaxNote.Visible = False
        lblVoidComment.Visible = False
        lblVoidCommentMarker.Visible = False

        rbAspectType.Enabled = False

        txtChemicalDesc.Enabled = False
        txtCorpEnvComments.Enabled = False
        txtDeptArea.Enabled = False
        txtDisposalDesc.Enabled = False
        txtHRSafetyComments.Enabled = False
        txtIncompatibleWith.Enabled = False
        txtOtherEngDesc.Enabled = False
        txtOtherEquipDesc.Enabled = False
        txtOtherHazardDesc.Enabled = False
        txtOtherUsageDesc.Enabled = False
        txtPlantEnvComments.Enabled = False
        txtProductManufacturer.Enabled = False
        txtProductName.Enabled = False
        txtPurchaseFrom.Enabled = False
        txtRequestDate.Enabled = False
        txtRndComments.Enabled = False
        txtStorageDesc.Enabled = False

        txtVoidComment.Enabled = False
        txtVoidComment.Visible = False

    End Sub

    Protected Function SendRequestedByNotificationToAllRolesEmail() As Boolean

        Dim bReturnValue As Boolean = False

        Try
            Dim dsSupportingDocs As DataSet

            Dim iRowCounter As Integer = 0

            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strApproveURL As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Detail.aspx?ChemRevFormID=" & ViewState("ChemRevFormID")
            Dim strPreviewFormURL As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Preview.aspx?ChemRevFormID=" & ViewState("ChemRevFormID")
            Dim strPreviewSupportingDocs As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Supporting_Doc_View.aspx?RowID="

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            If strEmailCCAddress <> "" Then
                strEmailCCAddress += ";"
            End If

            'append current user
            strEmailCCAddress += strEmailFromAddress

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br><br>"
                strBody += "<h1>This information is purely for testing and is NOT valid!!!</h1><br><br>"
            End If

            strSubject += "Chemical Review Form Notification ID: " & ViewState("ChemRevFormID")

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            strBody += "<font size='3' face='Verdana'>The following Chemical Review Form is ready for your review: </font><br><br>"

            strBody += "<font size='2' face='Verdana'>Chemical Review Form ID: <b>" & ViewState("ChemRevFormID") & "</b></font><br><br>"

            strBody += "<font size='1' color='red' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link(s) below.</font>"

            strBody += "<br><font size='2' face='Verdana'><a href='" & strApproveURL & "'>Click here to update or approve the Chemical Review Details</a></font>"

            strBody += "<br><font size='2' face='Verdana'><a href='" & strPreviewFormURL & "'>Click here to Preview the Form</a></font><br><br>"

            If ddRequestedByTeamMember.SelectedIndex > 0 Then
                strBody += "<font size='2' face='Verdana'>Requested By : " & ddRequestedByTeamMember.SelectedItem.Text & "</font><br>"
            End If

            If txtRequestDate.Text.Trim <> "" Then
                strBody += "<font size='2' face='Verdana'>Request Date : " & txtRequestDate.Text.Trim & "</font><br>"
            End If

            If txtProductName.Text.Trim <> "" Then
                strBody += "<font size='2' face='Verdana'>Product : " & txtProductName.Text.Trim & "</font><br>"
            End If

            If txtChemicalDesc.Text.Trim <> "" Then
                strBody += "<font size='2' face='Verdana'>Description : " & txtChemicalDesc.Text.Trim & "</font><br>"
            End If

            dsSupportingDocs = SafetyModule.GetChemicalReviewFormSupportingDocList(ViewState("ChemRevFormID"))
            If commonFunctions.CheckDataset(dsSupportingDocs) = True Then
                strBody += "<br><table width='90%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"
                strBody += "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                strBody += "<td><font size='2' face='Verdana'><strong>Other Supporting Documents</strong></font></td>"
                strBody += "</tr>"

                For iRowCounter = 0 To dsSupportingDocs.Tables(0).Rows.Count - 1
                    strBody += "<tr style='border-color:white'><font size='2' face='Verdana'>"

                    strBody += "<td><a href='" & strPreviewSupportingDocs & dsSupportingDocs.Tables(0).Rows(iRowCounter).Item("RowID") & "'><u>" & dsSupportingDocs.Tables(0).Rows(iRowCounter).Item("SupportingDocName") & "</u></a></td>"

                    strBody += "</font></tr>"
                Next

                strBody += "</table><br>"
            End If

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody += "<br><br>Email To Address List: " & strEmailToAddress & "<br>"
                strBody += "<br>Email CC Address List: " & strEmailCCAddress & "<br>"

                strEmailToAddress = "Roderick.Carlson@ugnauto.com"
                strEmailCCAddress = ""
            End If

            strBody += "<br><br><font size='1' face='Verdana'> +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "
            strBody += "<br>If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the Safety Module<br>"
            strBody += "<br>Please <u>do not</u> reply back to this email because you will not receive a response.  Please use a seperate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br>"
            strBody += "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ </font>"

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

            'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("Chemical Review Form Notify All", strEmailFromAddress, strEmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            End Try

            bReturnValue = True
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        SendRequestedByNotificationToAllRolesEmail = bReturnValue

    End Function

    Protected Function SendApproverUpdateStatusEmail(ByVal TeamMemberName As String, ByVal RoleName As String, ByVal StatusName As String, ByVal Comments As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try

            Dim dsSupportingDocs As DataSet

            Dim iRowCounter As Integer = 0

            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strApproveURL As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Detail.aspx?ChemRevFormID=" & ViewState("ChemRevFormID")
            Dim strPreviewFormURL As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Preview.aspx?ChemRevFormID=" & ViewState("ChemRevFormID")
            Dim strPreviewSupportingDocs As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Supporting_Doc_View.aspx?RowID="

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            If strEmailCCAddress <> "" Then
                strEmailCCAddress += ";"
            End If

            'append current user
            strEmailCCAddress += strEmailFromAddress

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br><br>"
                strBody += "<h1>This information is purely for testing and is NOT valid!!!</h1><br><br>"
            End If

            strSubject += "Chemical Review Form ID: " & ViewState("ChemRevFormID") & " has been updated by " & TeamMemberName

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            strBody += "<font size='3' face='Verdana'>The following Chemical Review Form has been updated by:<b> " & TeamMemberName & " </b></font><br><br>"

            strBody += "<font size='2' face='Verdana'>Chemical Review Form ID: <b>" & ViewState("ChemRevFormID") & "</b></font><br><br>"

            strBody += "<font size='2' face='Verdana'>Approver Status: <b>" & StatusName & "</b> for role " & RoleName & " </font><br><br>"

            If Comments.Trim <> "" Then
                strBody += "<font size='2' face='Verdana'>Comments:" & Comments & "</font><br><br>"
            End If

            strBody += "<font size='1' color='red' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link(s) below.</font>"

            strBody += "<br><font size='2' face='Verdana'><a href='" & strApproveURL & "'>Click here to update or approve the Chemical Review Details</a></font>"

            strBody += "<br><font size='2' face='Verdana'><a href='" & strPreviewFormURL & "'>Click here to Preview the Form</a></font><br><br>"

            If ddRequestedByTeamMember.SelectedIndex > 0 Then
                strBody += "<font size='2' face='Verdana'>Requested By : " & ddRequestedByTeamMember.SelectedItem.Text & "</font><br>"
            End If

            If txtRequestDate.Text.Trim <> "" Then
                strBody += "<font size='2' face='Verdana'>Request Date : " & txtRequestDate.Text.Trim & "</font><br>"
            End If

            If txtProductName.Text.Trim <> "" Then
                strBody += "<font size='2' face='Verdana'>Product : " & txtProductName.Text.Trim & "</font><br>"
            End If

            If txtChemicalDesc.Text.Trim <> "" Then
                strBody += "<font size='2' face='Verdana'>Description : " & txtChemicalDesc.Text.Trim & "</font><br>"
            End If

            dsSupportingDocs = SafetyModule.GetChemicalReviewFormSupportingDocList(ViewState("ChemRevFormID"))
            If commonFunctions.CheckDataset(dsSupportingDocs) = True Then
                strBody += "<br><table width='90%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"
                strBody += "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                strBody += "<td><font size='2' face='Verdana'><strong>Other Supporting Documents</strong></font></td>"
                strBody += "</tr>"

                For iRowCounter = 0 To dsSupportingDocs.Tables(0).Rows.Count - 1
                    strBody += "<tr style='border-color:white'><font size='2' face='Verdana'>"


                    strBody += "<td><a href='" & strPreviewSupportingDocs & dsSupportingDocs.Tables(0).Rows(iRowCounter).Item("RowID") & "'><u>" & dsSupportingDocs.Tables(0).Rows(iRowCounter).Item("SupportingDocName") & "</u></a></td>"

                    strBody += "</font></tr>"
                Next

                strBody += "</table><br>"
            End If

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody += "<br><br>Email To Address List: " & strEmailToAddress & "<br>"
                strBody += "<br>Email CC Address List: " & strEmailCCAddress & "<br>"

                strEmailToAddress = "Roderick.Carlson@ugnauto.com"
                strEmailCCAddress = ""
            End If

            strBody += "<br><br><font size='1' face='Verdana'> +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "
            strBody += "<br>If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the Safety Module<br>"
            strBody += "<br>Please <u>do not</u> reply back to this email because you will not receive a response.  Please use a seperate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br>"
            strBody += "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ </font>"

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

            'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("Chemical Review Form Approver Update", strEmailFromAddress, strEmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            End Try

            bReturnValue = True
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        SendApproverUpdateStatusEmail = bReturnValue

    End Function

    Protected Function SendRejectedEmail(ByVal TeamMemberName As String, ByVal RoleName As String, ByVal Comments As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try
            Dim dsTeamMember As DataSet
            Dim dsSupportingDocs As DataSet

            Dim iRowCounter As Integer = 0
            Dim iRequestedByTeamMemberID As Integer = 0
            Dim iHRSafetyTeamMemberID As Integer = 0

            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strApproveURL As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Detail.aspx?ChemRevFormID=" & ViewState("ChemRevFormID")
            Dim strPreviewFormURL As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Preview.aspx?ChemRevFormID=" & ViewState("ChemRevFormID")
            Dim strPreviewSupportingDocs As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Supporting_Doc_View.aspx?RowID="

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            If ddRequestedByTeamMember.SelectedIndex > 0 Then

                iRequestedByTeamMemberID = ddRequestedByTeamMember.SelectedValue

                'get requested by team member email
                dsTeamMember = SecurityModule.GetTeamMember(iRequestedByTeamMemberID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataset(dsTeamMember) = True Then
                    strEmailToAddress = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                End If

                'get HR Safety Manager Email
                If ddHRSafetyTeamMember.SelectedIndex > 0 Then
                    iHRSafetyTeamMemberID = ddHRSafetyTeamMember.SelectedValue

                    dsTeamMember = SecurityModule.GetTeamMember(iHRSafetyTeamMemberID, "", "", "", "", "", True, Nothing)
                    If commonFunctions.CheckDataset(dsTeamMember) = True Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress += ";"
                        End If

                        strEmailToAddress += dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    End If
                End If

                If strEmailCCAddress <> "" Then
                    strEmailCCAddress += ";"
                End If

                'append current user
                strEmailCCAddress += strEmailFromAddress

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    strSubject = "TEST PLEASE DISREGARD: "
                    'strSubject = "TEST: "
                    strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br><br>"
                    'strBody = "THIS IS AN EMAIL IN  THE TEST SYSTEM. USERS ARE TESTING THIS NEW MODULE.<br><br>"
                    strBody += "<h1>This information is purely for testing and is NOT valid!!!</h1><br><br>"
                End If

                strSubject += "Chemical Review Form ID: " & ViewState("ChemRevFormID") & " has been REJECTED by " & TeamMemberName

                'create the mail message using new System.Net.Mail (not CDonts)
                Dim mail As New MailMessage()

                strBody += "<font size='3' face='Verdana'>The following Chemical Review Form has been <b>REJECTED</b> by:<b> " & TeamMemberName & " </b></font><br><br>"

                strBody += "<font size='2' face='Verdana'>Chemical Review Form ID: <b>" & ViewState("ChemRevFormID") & "</b></font><br><br>"

                If Comments.Trim <> "" Then
                    strBody += "<font size='2' face='Verdana'>Comments: " & Comments & "</font><br><br>"
                End If

                strBody += "<font size='1' color='red' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link(s) below.</font>"

                strBody += "<br><font size='2' face='Verdana'><a href='" & strApproveURL & "'>Click here to view the Chemical Review Details</a></font>"

                strBody += "<br><font size='2' face='Verdana'><a href='" & strPreviewFormURL & "'>Click here to Preview the Form</a></font><br><br>"

                If ddRequestedByTeamMember.SelectedIndex > 0 Then
                    strBody += "<font size='2' face='Verdana'>Requested By : " & ddRequestedByTeamMember.SelectedItem.Text & "</font><br>"
                End If

                If txtRequestDate.Text.Trim <> "" Then
                    strBody += "<font size='2' face='Verdana'>Request Date : " & txtRequestDate.Text.Trim & "</font><br>"
                End If

                If txtProductName.Text.Trim <> "" Then
                    strBody += "<font size='2' face='Verdana'>Product : " & txtProductName.Text.Trim & "</font><br>"
                End If

                If txtChemicalDesc.Text.Trim <> "" Then
                    strBody += "<font size='2' face='Verdana'>Description : " & txtChemicalDesc.Text.Trim & "</font><br>"
                End If

                dsSupportingDocs = SafetyModule.GetChemicalReviewFormSupportingDocList(ViewState("ChemRevFormID"))
                If commonFunctions.CheckDataset(dsSupportingDocs) = True Then
                    strBody += "<br><table width='90%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"
                    strBody += "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                    strBody += "<td><font size='2' face='Verdana'><strong>Other Supporting Documents</strong></font></td>"
                    strBody += "</tr>"

                    For iRowCounter = 0 To dsSupportingDocs.Tables(0).Rows.Count - 1
                        strBody += "<tr style='border-color:white'><font size='2' face='Verdana'>"


                        strBody += "<td><a href='" & strPreviewSupportingDocs & dsSupportingDocs.Tables(0).Rows(iRowCounter).Item("RowID") & "'><u>" & dsSupportingDocs.Tables(0).Rows(iRowCounter).Item("SupportingDocName") & "</u></a></td>"

                        strBody += "</font></tr>"
                    Next

                    strBody += "</table><br>"
                End If

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    strBody += "<br><br>Email To Address List: " & strEmailToAddress & "<br>"
                    strBody += "<br>Email CC Address List: " & strEmailCCAddress & "<br>"

                    strEmailToAddress = "Roderick.Carlson@ugnauto.com"
                    strEmailCCAddress = ""
                End If

                'strEmailToAddress = "Roderick.Carlson@ugnauto.com"
                'strEmailCCAddress = ""

                strBody += "<br><br><font size='1' face='Verdana'> +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "
                strBody += "<br>If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the Safety Module<br>"
                strBody += "<br>Please <u>do not</u> reply back to this email because you will not receive a response.  Please use a seperate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br>"
                strBody += "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ </font>"

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

                'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
                mail.IsBodyHtml = True

                'send the message 
                Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

                Try
                    smtp.Send(mail)
                    lblMessage.Text &= "Email Notification sent."
                Catch ex As Exception
                    lblMessage.Text &= "Email Notification queued."
                    UGNErrorTrapping.InsertEmailQueue("Chemical Review Form Reject", strEmailFromAddress, strEmailToAddress, strEmailCCAddress, strSubject, strBody, "")
                End Try

                bReturnValue = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        SendRejectedEmail = bReturnValue

    End Function

    Protected Function SendFormApprovedEmail() As Boolean

        Dim bReturnValue As Boolean = False

        Try
            Dim dsTeamMember As DataSet

            Dim iRequestedByTeamMemberID As Integer = 0
            Dim iHRSafetyTeamMemberID As Integer = 0

            Dim strSubject As String = ""
            Dim strBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strApproveURL As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Detail.aspx?ChemRevFormID=" & ViewState("ChemRevFormID")
            Dim strPreviewFormURL As String = strProdOrTestEnvironment & "Safety/Chemical_Review_Form_Preview.aspx?ChemRevFormID=" & ViewState("ChemRevFormID")

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            strEmailToAddress = ""
            strEmailCCAddress = ""

            If ddRequestedByTeamMember.SelectedIndex > 0 Then
                iRequestedByTeamMemberID = ddRequestedByTeamMember.SelectedValue

                'get requested by team member email
                dsTeamMember = SecurityModule.GetTeamMember(iRequestedByTeamMemberID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataset(dsTeamMember) = True Then
                    strEmailToAddress = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                End If

                'get HR Safety Manager Email
                If ddHRSafetyTeamMember.SelectedIndex > 0 Then
                    iHRSafetyTeamMemberID = ddHRSafetyTeamMember.SelectedValue

                    dsTeamMember = SecurityModule.GetTeamMember(iHRSafetyTeamMemberID, "", "", "", "", "", True, Nothing)
                    If commonFunctions.CheckDataset(dsTeamMember) = True Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress += ";"
                        End If

                        strEmailToAddress += dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    End If
                End If

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    strSubject = "TEST PLEASE DISREGARD: "
                    strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br><br>"
                    strBody += "<h1>This information is purely for testing and is NOT valid!!!</h1><br><br>"
                End If

                strSubject += "Chemical Review Form ID: " & ViewState("ChemRevFormID") & " has been completed by all"

                'create the mail message using new System.Net.Mail (not CDonts)
                Dim mail As New MailMessage()

                strBody += "<font size='3' face='Verdana'>The following Chemical Review Form has been approved by at least the minumum required team members. Other team members will have up to 7 calendar days from the Request Date to still review the form. After that date, the form will be locked.</font><br><br>"

                strBody += "<font size='2' face='Verdana'>Chemical Review Form ID: <b>" & ViewState("ChemRevFormID") & "</b></font><br><br>"

                strBody += "<font size='1' color='red' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link(s) below.</font>"

                strBody += "<br><font size='2' face='Verdana'><a href='" & strApproveURL & "'>Click here to view the Chemical Review Details</a></font>"

                strBody += "<br><font size='2' face='Verdana'><a href='" & strPreviewFormURL & "'>Click here to Preview the Form</a></font><br><br>"

                If ddRequestedByTeamMember.SelectedIndex > 0 Then
                    strBody += "<font size='2' face='Verdana'>Requested By : " & ddRequestedByTeamMember.SelectedItem.Text & "</font><br>"
                End If

                If txtRequestDate.Text.Trim <> "" Then
                    strBody += "<font size='2' face='Verdana'>Request Date : " & txtRequestDate.Text.Trim & "</font><br>"
                End If

                If txtProductName.Text.Trim <> "" Then
                    strBody += "<font size='2' face='Verdana'>Product : " & txtProductName.Text.Trim & "</font><br>"
                End If

                If txtChemicalDesc.Text.Trim <> "" Then
                    strBody += "<font size='2' face='Verdana'>Description : " & txtChemicalDesc.Text.Trim & "</font><br>"
                End If

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    strBody += "<br><br>Email To Address List: " & strEmailToAddress & "<br>"

                    strEmailToAddress = "Roderick.Carlson@ugnauto.com"
                    strEmailCCAddress = ""
                End If

                strBody += "<br><br><font size='1' face='Verdana'> +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "
                strBody += "<br>If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the Safety Module<br>"
                strBody += "<br>Please <u>do not</u> reply back to this email because you will not receive a response.  Please use a seperate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br>"
                strBody += "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ </font>"

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

                ''build email CC List
                'If strEmailCCAddress IsNot Nothing Then
                '    emailList = strEmailCCAddress.Split(";")

                '    For i = 0 To UBound(emailList)
                '        If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                '            mail.CC.Add(emailList(i))
                '        End If
                '    Next i
                'End If

                'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
                mail.IsBodyHtml = True

                'send the message 
                Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

                Try
                    smtp.Send(mail)
                    lblMessage.Text &= "Email Notification sent."
                Catch ex As Exception
                    lblMessage.Text &= "Email Notification queued."
                    UGNErrorTrapping.InsertEmailQueue("Chemical Review Form Approved", strEmailFromAddress, strEmailToAddress, strEmailCCAddress, strSubject, strBody, "")
                End Try

                bReturnValue = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        SendFormApprovedEmail = bReturnValue

    End Function
    Protected Sub btnRnDSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRnDSave.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddRnDStatus.SelectedIndex >= 0 Then
                iStatusID = ddRnDStatus.SelectedValue
                iRoleTeamMemberID = ddRnDTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 70, iStatusID, txtRndComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 70, iStatusID, txtRndComments.Text.Trim)

                UpdateOverallStatus(False)

                'ddRnDTeamMember.SelectedValue = ViewState("TeamMemberID")

                lblMessage.Text += "<br>Status updated."

                If iStatusID = 5 Then
                    If SendRejectedEmail(ddRnDTeamMember.SelectedItem.Text, "Research and Development", txtRndComments.Text.Trim) = True Then
                        'lblMessage.Text += "<br>The form requested by team member and HR Safety Manager have been notified."
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

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnCorpEnvSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCorpEnvSave.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddCorpEnvStatus.SelectedIndex >= 0 Then
                iStatusID = ddCorpEnvStatus.SelectedValue
                iRoleTeamMemberID = ddCorpEnvTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                    ddCorpEnvTeamMember.SelectedValue = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 71, iStatusID, txtCorpEnvComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 71, iStatusID, txtCorpEnvComments.Text.Trim)

                UpdateOverallStatus(False)

                ' ddCorpEnvTeamMember.SelectedValue = ViewState("TeamMemberID")

                lblMessage.Text += "<br>Status updated."

                If iStatusID = 5 Then
                    If SendRejectedEmail(ddCorpEnvTeamMember.SelectedItem.Text, "Corporate Environment", txtCorpEnvComments.Text.Trim) = True Then
                        'lblMessage.Text += "<br>The form requested by team member and HR Safety Manager have been notified."
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

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnHRSafetySave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHRSafetySave.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddHRSafetyStatus.SelectedIndex >= 0 Then
                iStatusID = ddHRSafetyStatus.SelectedValue
                iRoleTeamMemberID = ddHRSafetyTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                    ddHRSafetyTeamMember.SelectedValue = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 69, iStatusID, txtHRSafetyComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 69, iStatusID, txtHRSafetyComments.Text.Trim)

                UpdateOverallStatus(False)

                'ddHRSafetyTeamMember.SelectedValue = ViewState("TeamMemberID")

                lblMessage.Text += "<br>Status updated."

                If iStatusID = 5 Then
                    If SendRejectedEmail(ddHRSafetyTeamMember.SelectedItem.Text, "HR Safety", txtHRSafetyComments.Text.Trim) = True Then
                        'lblMessage.Text += "<br>The form requested by team member and HR Safety Manager have been notified."
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

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnPlantEnvSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlantEnvSave.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddPlantEnvStatus.SelectedIndex >= 0 Then
                iStatusID = ddPlantEnvStatus.SelectedValue
                iRoleTeamMemberID = ddPlantEnvTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                    ddPlantEnvTeamMember.SelectedValue = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 72, iStatusID, txtPlantEnvComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 72, iStatusID, txtPlantEnvComments.Text.Trim)

                UpdateOverallStatus(False)

                'ddPlantEnvTeamMember.SelectedValue = ViewState("TeamMemberID")

                lblMessage.Text += "<br>Status updated."

                If iStatusID = 5 Then
                    If SendRejectedEmail(ddPlantEnvTeamMember.SelectedItem.Text, "Plant Environment", txtPlantEnvComments.Text.Trim) = True Then
                        'lblMessage.Text += "<br>The form requested by team member and HR Safety Manager have been notified."
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

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnPurchasingSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPurchasingSave.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddPurchasingStatus.SelectedIndex >= 0 Then
                iStatusID = ddPurchasingStatus.SelectedValue

                iRoleTeamMemberID = ddPurchasingTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                    ddPurchasingTeamMember.SelectedValue = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 73, iStatusID, txtPurchasingComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 73, iStatusID, txtPurchasingComments.Text.Trim)

                UpdateOverallStatus(False)

                'ddPurchasingTeamMember.SelectedValue = ViewState("TeamMemberID")

                lblMessage.Text += "<br>Status updated."

                If iStatusID = 5 Then
                    If SendRejectedEmail(ddPurchasingTeamMember.SelectedItem.Text, "Purchasing", txtPurchasingComments.Text.Trim) = True Then
                        'lblMessage.Text += "<br>The form requested by team member and HR Safety Manager have been notified."
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

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnRnDNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRnDNotify.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddRnDStatus.SelectedIndex >= 0 Then
                iStatusID = ddRnDStatus.SelectedValue
                iRoleTeamMemberID = ddRnDTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 70, iStatusID, txtRndComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 70, iStatusID, txtRndComments.Text.Trim)

                BuildEmailNotificationList()

                UpdateOverallStatus(True)

                If SendApproverUpdateStatusEmail(ddRnDTeamMember.SelectedItem.Text, "Research and Development", ddRnDStatus.SelectedItem.Text, txtRndComments.Text.Trim) = True Then
                    '    lblMessage.Text = "<br>Notifications sent"
                    'Else
                    '    lblMessage.Text = "<br>Notifications not sent"
                End If

                'update notification sent columns
                SafetyModule.UpdateChemicalReviewFormNotification(ViewState("ChemRevFormID"))

                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnCorpEnvNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCorpEnvNotify.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddCorpEnvStatus.SelectedIndex >= 0 Then
                iStatusID = ddCorpEnvStatus.SelectedValue
                iRoleTeamMemberID = ddCorpEnvTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 71, iStatusID, txtCorpEnvComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 71, iStatusID, txtCorpEnvComments.Text.Trim)

                BuildEmailNotificationList()

                UpdateOverallStatus(True)

                If SendApproverUpdateStatusEmail(ddCorpEnvTeamMember.SelectedItem.Text, "Corporate Environmental", ddCorpEnvStatus.SelectedItem.Text, txtCorpEnvComments.Text.Trim) = True Then
                    '    lblMessage.Text = "<br>Notifications sent"
                    'Else
                    '    lblMessage.Text = "<br>Notifications not sent"
                End If

                'update notification sent columns
                SafetyModule.UpdateChemicalReviewFormNotification(ViewState("ChemRevFormID"))

                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnHRSafetyNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHRSafetyNotify.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddHRSafetyStatus.SelectedIndex >= 0 Then
                iStatusID = ddHRSafetyStatus.SelectedValue
                iRoleTeamMemberID = ddHRSafetyTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 69, iStatusID, txtHRSafetyComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 69, iStatusID, txtHRSafetyComments.Text.Trim)

                BuildEmailNotificationList()

                UpdateOverallStatus(True)

                If SendApproverUpdateStatusEmail(ddHRSafetyTeamMember.SelectedItem.Text, "HR Safety", ddHRSafetyStatus.SelectedItem.Text, txtHRSafetyComments.Text.Trim) = True Then
                    '    lblMessage.Text = "<br>Notifications sent"
                    'Else
                    '    lblMessage.Text = "<br>Notifications not sent"
                End If

                'update notification sent columns
                SafetyModule.UpdateChemicalReviewFormNotification(ViewState("ChemRevFormID"))

                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnPlantEnvNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlantEnvNotify.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddPlantEnvStatus.SelectedIndex >= 0 Then
                iStatusID = ddPlantEnvStatus.SelectedValue
                iRoleTeamMemberID = ddPlantEnvTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 72, iStatusID, txtPlantEnvComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 72, iStatusID, txtPlantEnvComments.Text.Trim)

                BuildEmailNotificationList()

                UpdateOverallStatus(True)

                If SendApproverUpdateStatusEmail(ddPlantEnvTeamMember.SelectedItem.Text, "Plant Environmental", ddPlantEnvStatus.SelectedItem.Text, txtPlantEnvComments.Text.Trim) = True Then
                    '    lblMessage.Text = "<br>Notifications sent"
                    'Else
                    '    lblMessage.Text = "<br>Notifications not sent"
                End If

                'update notification sent columns
                SafetyModule.UpdateChemicalReviewFormNotification(ViewState("ChemRevFormID"))

                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnPurchasingNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPurchasingNotify.Click

        Try
            ClearMessages()

            Dim iStatusID As Integer = 0
            Dim iRoleTeamMemberID As Integer = 0

            If ddPurchasingStatus.SelectedIndex >= 0 Then
                iStatusID = ddPurchasingStatus.SelectedValue
                iRoleTeamMemberID = ddPurchasingTeamMember.SelectedValue

                'if approved, then current user must be selected
                If iStatusID = 3 Then
                    iRoleTeamMemberID = ViewState("TeamMemberID")               
                End If

                'SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ViewState("TeamMemberID"), 73, iStatusID, txtPurchasingComments.Text.Trim)
                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), iRoleTeamMemberID, 73, iStatusID, txtPurchasingComments.Text.Trim)

                BuildEmailNotificationList()

                UpdateOverallStatus(True)

                If SendApproverUpdateStatusEmail(ddPurchasingTeamMember.SelectedItem.Text, "Purchasing", ddPurchasingStatus.SelectedItem.Text, txtPurchasingComments.Text.Trim) = True Then
                    '    lblMessage.Text = "<br>Notifications sent"
                    'Else
                    '    lblMessage.Text = "<br>Notifications not sent"
                End If

                'update notification sent columns
                SafetyModule.UpdateChemicalReviewFormNotification(ViewState("ChemRevFormID"))

                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApprovals.Text &= lblMessage.Text
        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click

        Try
            ClearMessages()

            DisableUpdateControls()

            btnCopy.Visible = False
            btnPreview.Visible = False
            btnPreviewBottom.Visible = False
            btnNotify.Visible = False
            btnVoid.Visible = ViewState("isEdit")

            lblVoidComment.Visible = True
            lblVoidCommentMarker.Visible = True
            txtVoidComment.Visible = True
            txtVoidComment.Enabled = True

            btnVoid.Attributes.Add("onclick", "")

            If txtVoidComment.Text.Trim <> "" Then

                SafetyModule.DeleteChemicalReviewForm(ViewState("ChemRevFormID"), txtVoidComment.Text.Trim)

                lblMessage.Text &= "The form has been voided.<br>"
                btnVoid.Visible = False
                btnVoidCancel.Visible = False
                EnableControls()
            Else
                lblMessage.Text &= "To void this form, please fill in the Void Comment field and then CLICK THE VOID BUTTON AGAIN."
                txtVoidComment.Focus()
                btnVoidCancel.Visible = True
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub btnVoidCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoidCancel.Click

        Try
            ClearMessages()

            txtVoidComment.Text = ""

            EnableControls()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        Try
            lblMessage.Text = ""

            ViewState("ChemRevFormID") = 0
            ViewState("StatusID") = 1
            ddStatus.SelectedValue = 1
            ViewState("isLocked") = False

            EnableControls()

            ddRequestedByTeamMember.Enabled = ViewState("isEdit")
            txtRequestDate.Enabled = ViewState("isEdit")
            imgRequestDate.Visible = ViewState("isEdit")

            mvTabs.ActiveViewIndex = 0

            gvSupportingDoc.DataBind()

            lblMessage.Text += "The information is copied. PLEASE MAKE SURE TO SAVE YOUR WORK!<br>"

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddRnDTeamMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddRnDTeamMember.SelectedIndexChanged

        Try
            ClearMessages()

            If ddRnDTeamMember.SelectedIndex > 0 Then

                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ddRnDTeamMember.SelectedValue, 70, 1, "")

                UpdateOverallStatus(False)

                ddRnDStatus.SelectedValue = 1

                lblMessage.Text &= "<br>RnD Team Member changed."

            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text &= lblMessage.Text

    End Sub

    Protected Sub ddHRSafetyTeamMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddHRSafetyTeamMember.SelectedIndexChanged


        Try
            ClearMessages()

            If ddHRSafetyTeamMember.SelectedIndex > 0 Then

                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ddHRSafetyTeamMember.SelectedValue, 69, 1, "")

                UpdateOverallStatus(False)

                ddHRSafetyStatus.SelectedValue = 1

                lblMessage.Text &= "<br>HR Safety Team Member changed."

            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddPlantEnvTeamMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPlantEnvTeamMember.SelectedIndexChanged

        Try
            ClearMessages()

            If ddPlantEnvTeamMember.SelectedIndex > 0 Then

                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ddPlantEnvTeamMember.SelectedValue, 72, 1, "")

                UpdateOverallStatus(False)

                ddPlantEnvStatus.SelectedValue = 1

                lblMessage.Text &= "<br>Plant Env Team Member changed."

            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddCorpEnvTeamMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCorpEnvTeamMember.SelectedIndexChanged

        Try
            ClearMessages()

            If ddCorpEnvTeamMember.SelectedIndex > 0 Then

                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ddCorpEnvTeamMember.SelectedValue, 71, 1, "")

                UpdateOverallStatus(False)

                ddCorpEnvStatus.SelectedValue = 1

                lblMessage.Text &= "<br>Corp Env Team Member changed."

            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddPurchasingTeamMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPurchasingTeamMember.SelectedIndexChanged

        Try
            ClearMessages()

            If ddPurchasingTeamMember.SelectedIndex > 0 Then

                SafetyModule.UpdateChemicalReviewFormApprovalStatus(ViewState("ChemRevFormID"), ddPurchasingTeamMember.SelectedValue, 73, 1, "")

                UpdateOverallStatus(False)

                ddPurchasingStatus.SelectedValue = 1

                lblMessage.Text &= "<br>Purchasing Team Member changed."

            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvSupportingDoc_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSupportingDoc.DataBound

        'hide header of columns
        If gvSupportingDoc.Rows.Count > 0 Then
            gvSupportingDoc.HeaderRow.Cells(0).Visible = False
        End If

        CheckSupportingDocGrid()

    End Sub

    Protected Sub gvSupportingDoc_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupportingDoc.RowCreated

        'hide columns
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
        End If

    End Sub

    Protected Sub btnSaveUploadSupportingDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUploadSupportingDocument.Click

        Try
            ClearMessages()

            Dim strFileName As String = fileUploadSupportingDoc.FileName

            If strFileName.Length > 0 Then

                If InStr(UCase(strFileName), ".PDF") = 0 Then
                    '-- Selection of non-PDF file
                    lblMessage.Text += "Only PDF files can uploaded.<br>"
                Else

                    If strFileName.Length > 96 Then
                        strFileName = commonFunctions.convertSpecialChar(Strings.Left(fileUploadSupportingDoc.FileName.Substring(0, fileUploadSupportingDoc.FileName.Length - 4), 96), True) & ".pdf"
                    Else
                        strFileName = commonFunctions.convertSpecialChar(fileUploadSupportingDoc.FileName.Substring(0, fileUploadSupportingDoc.FileName.Length - 4), True) & ".pdf"
                    End If

                    'Load FileUpload's InputStream into Byte array
                    Dim docBytes(fileUploadSupportingDoc.PostedFile.InputStream.Length) As Byte
                    fileUploadSupportingDoc.PostedFile.InputStream.Read(docBytes, 0, docBytes.Length)

                    SafetyModule.InsertChemicalReviewFormSupportingDoc(ViewState("ChemRevFormID"), strFileName, docBytes)

                    lblMessage.Text += "File Uploaded Successfully<br>"

                    Dim bSupportingDocCountMaximum As Boolean = isSupportingDocCountMaximum()
                    lblFileUploadLabel.Visible = Not bSupportingDocCountMaximum
                    fileUploadSupportingDoc.Visible = Not bSupportingDocCountMaximum
                    btnSaveUploadSupportingDocument.Visible = Not bSupportingDocCountMaximum

                    gvSupportingDoc.DataBind()
                    gvSupportingDoc.Visible = True
                End If
            Else
                lblMessage.Text += "Error: Please make sure you have selected a file.<br>"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += "<br>" & ex.Message & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageSupportingDocs.Text = lblMessage.Text

    End Sub
End Class
