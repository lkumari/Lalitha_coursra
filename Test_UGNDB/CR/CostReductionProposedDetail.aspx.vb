' *********************************************************************************************
' Name:	CR_CostReductionProposedDetail.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'
' Date		    Author	    
' 02/16/2010    RCarlson	Created .Net application
' 03/20/1010    RCarlson    CR-2870 - make sure administrators and plant controlers can also update this page
' 04/21/2010    RCarlson    CR-2879 - Notify Facility and Corporate Plant Controllers and Team Leaders when Financial Numbers Change Only allow Plant Controllers to change financial numbers once the option is checked that the Plant Controllers reviewed the project
' 11/02/2010    RCarlson    Added Description to email
' 02/22/2011    RCarlson    allow negative values
' 08/30/2011    RCarlson    added Customer Give Back field and Budget Fields
' 01/23/2012    RCarlson    Allow Word 2007 DocX and Excel 2007 xlsX files
' 01/08/2014    LRey        Replaced GetCustomer with GetOEMManufacturer. SOLDTO|CABBV values are not used in new ERP.
' 03/01/2014    LRey        Replaced "BPCS Part No" to "Part No" wherever used. 
' *********************************************************************************************

Partial Class CR_CostReductionProposedDetail
    Inherits System.Web.UI.Page

    Private Sub InitializeViewState()

        Try

            ViewState("pProjNo") = 0
            ViewState("pSD") = 0

            ViewState("Admin") = False
            ViewState("ObjectRole") = False
            ViewState("isViewable") = False
            ViewState("TeamMemberID") = 0
            ViewState("SubscriptionID") = 0

            ViewState("CurrentCustomerProgramRow") = 0
            ViewState("CustomerPartNo") = ""
            ViewState("DateSubmitted") = ""
            ViewState("Description") = ""
            ViewState("isNewRecord") = True
            ViewState("isPlantControllerReviewed") = False
            ViewState("LeaderTMID") = 0
            ViewState("OriginalAnnCostSave") = 0
            ViewState("OriginalCapEx") = 0
            ViewState("pAprv") = 0
            ViewState("ProjectCategoryID") = 0
            ViewState("UGNFacility") = ""

            ViewState("EmailSent") = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            'clear crystal reports
            CRModule.CleanCRCrystalReports()

            If Not Page.IsPostBack Then

                InitializeViewState()

                ViewState("pProjNo") = 0

                If HttpContext.Current.Request.QueryString("pProjNo") > 0 Then
                    ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
                End If

                ''Used to take user back to Supporting Documents Tab after save.
                If HttpContext.Current.Request.QueryString("pSD") <> "" Then
                    ViewState("pSD") = HttpContext.Current.Request.QueryString("pSD")
                Else
                    ViewState("pSD") = 0
                End If

                CheckRights()

                BindCriteria()

                HandleMultiLineFields()

                'search current Customer PartNo
                Dim strCustomerPartNoClientScript As String = HandleCustomerPartNoPopUps(txtCustomerPartNo.ClientID)
                ' ''iBtnCustomerPartNoSearch.Attributes.Add("onClick", strCustomerPartNoClientScript)

                BindData()

                If ViewState("pSD") > 0 Then
                    mvTabs.ActiveViewIndex = Int32.Parse(7)
                    mvTabs.GetActiveView()
                    menuTabs.Items(7).Selected = True
                End If

                ''****************************************************
                '' Update the title and heading on the Master Page
                ''****************************************************
                Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
                m.PageTitle = "UGN, Inc."

                m.ContentLabel = "Cost Reduction Project - Proposed Details"

                ''**************************************************
                '' Override the Master Page bread crumb navigation
                ''**************************************************
                Dim ctl As Control = m.FindControl("lblOtherSiteNode")
                If ctl IsNot Nothing Then
                    Dim lbl As Label = CType(ctl, Label)

                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Cost Reduction</b> > <a href='CostReductionList.aspx'><b>Cost Reduction Project Search</b></a> > <a href='CostReduction.aspx?pProjNo=" & ViewState("pProjNo") & "'><b>Cost Reduction Project </b></a> > Proposed Details"

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
                ctl = m.FindControl("CRExtender")
                If ctl IsNot Nothing Then
                    Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                    cpe.Collapsed = False
                End If

                Dim strPreviewClientScript As String = "javascript:void(window.open('crViewCostReductionDetail.aspx?pProjNo=" & ViewState("pProjNo") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                btnPreview.Attributes.Add("onclick", strPreviewClientScript)
                btnPreviewBottom.Attributes.Add("onclick", strPreviewClientScript)

            End If

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub menuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuTabs.MenuItemClick

        Try

            mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

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
            ViewState("TeamMemberID") = 0
            ViewState("isViewable") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()

            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsSubscription As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 97 'Cost Reduction Project Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            ViewState("TeamMemberID") = 0
            ViewState("SubscriptionID") = 0

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                If dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    'developer testing as another team member
                    If iTeamMemberID = 530 Then
                        'iTeamMemberID = 612 'dan marcon
                        iTeamMemberID = 171 'greg hall
                        'iTeamMemberID = 611 'Vincent Chavez
                        'iTeamMemberID = 246 'Mike Echevarria
                    End If

                    ViewState("TeamMemberID") = iTeamMemberID

                    If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                        iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                        If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member

                            'Get Team Member's Role assignment
                            dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)
                            If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                                'Is Plant Controller?
                                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 20)
                                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                                    ViewState("SubscriptionID") = 20
                                End If

                                'Is Sales?
                                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 9)
                                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                                    ViewState("SubscriptionID") = 9
                                End If

                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        ViewState("ObjectRole") = True
                                        ViewState("Admin") = True
                                        ViewState("isViewable") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        ViewState("ObjectRole") = True
                                        ViewState("isViewable") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        ViewState("ObjectRole") = True
                                        ViewState("isViewable") = True
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                        ViewState("isViewable") = True
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        ViewState("ObjectRole") = True
                                        ViewState("isViewable") = True

                                        If ViewState("SubscriptionID") = 20 Then
                                            ViewState("Admin") = True
                                        End If
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        ''** No Entry allowed **''
                                        ViewState("ObjectRole") = False
                                        ViewState("isViewable") = False
                                End Select 'EOF of "Select Case iRoleID"                            
                            End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                        End If 'EOF of "If iWorking = True Then"            
                    End If

                End If

            End If 'EOF of "If dsTeamMember IsNot Nothing Then"

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Private Sub BindCriteria()

        Try

            Dim ds As DataSet

            ' ''ds = commonFunctions.GetOEMManufacturer("")
            ' ''If commonFunctions.CheckDataSet(ds) = True Then
            ' ''    ddCustomer.DataSource = ds
            ' ''    ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            ' ''    ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            ' ''    ddCustomer.DataBind()
            ' ''    ddCustomer.Items.Insert(0, "")
            ' ''End If

            ds = commonFunctions.GetProgramMake()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddMake.DataSource = ds
                ddMake.DataTextField = ds.Tables(0).Columns("Make").ColumnName.ToString()
                ddMake.DataValueField = ds.Tables(0).Columns("Make").ColumnName
                ddMake.DataBind()
                ddMake.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProgram("", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddProgram.DataSource = ds
                ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddProgram.DataBind()
                ddProgram.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Team Member control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddTeamMember.DataSource = ds
                ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddTeamMember.DataBind()
                ddTeamMember.Items.Insert(0, "")
                ddTeamMember.Enabled = False
            End If
            ddTeamMember.SelectedValue = HttpContext.Current.Session("UserId")

            ''''''''''''''''''''''''''''''''''''''''''''''''''
            '' SUBSCRIPTION DROPDOWNS
            ''''''''''''''''''''''''''''''''''''''''''''''''''

            ''Process Engineer
            'ds = commonFunctions.GetTeamMemberBySubscription(66)
            'If commonFunctions.CheckDataset(ds) = True Then
            '    ddProcessTeamMember.DataSource = ds
            '    ddProcessTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
            '    ddProcessTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
            '    ddProcessTeamMember.DataBind()
            '    ddProcessTeamMember.Items.Insert(0, "")
            'End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub BindData()

        Try
            Dim ds As DataSet

            Dim dCustomerGiveBackDollar As Double = 0
            Dim dCustomerGiveBackPercent As Double = 0

            Dim dMaterialPriceSavings As Double = 0
            Dim dMaterialPriceSavingsBudget As Double = 0

            Dim dMaterialUsageSavings As Double = 0
            Dim dMaterialUsageSavingsBudget As Double = 0

            Dim dTotalCECapital As Double = 0
            Dim dMaterialPriceCECapital As Double = 0
            Dim dMaterialUsageCECapital As Double = 0
            Dim dCycleTimeCECapital As Double = 0
            Dim dHeadCountCECapital As Double = 0
            Dim dOverheadCECapital As Double = 0

            Dim dTotalCEMaterial As Double = 0
            Dim dMaterialPriceCEMaterial As Double = 0
            Dim dMaterialUsageCEMaterial As Double = 0
            Dim dCycleTimeCEMaterial As Double = 0
            Dim dHeadCountCEMaterial As Double = 0
            Dim dOverheadCEMaterial As Double = 0

            Dim dTotalCEOutsideSupport As Double = 0
            Dim dMaterialPriceCEOutsideSupport As Double = 0
            Dim dMaterialUsageCEOutsideSupport As Double = 0
            Dim dCycleTimeCEOutsideSupport As Double = 0
            Dim dHeadCountCEOutsideSupport As Double = 0
            Dim dOverheadCEOutsideSupport As Double = 0

            Dim dTotalCEMisc As Double = 0
            Dim dMaterialPriceCEMisc As Double = 0
            Dim dMaterialUsageCEMisc As Double = 0
            Dim dCycleTimeCEMisc As Double = 0
            Dim dHeadCountCEMisc As Double = 0
            Dim dOverheadCEMisc As Double = 0

            Dim dTotalCEInHouseSupport As Double = 0
            Dim dMaterialPriceCEInHouseSupport As Double = 0
            Dim dMaterialUsageCEInHouseSupport As Double = 0
            Dim dCycleTimeCEInHouseSupport As Double = 0
            Dim dHeadCountCEInHouseSupport As Double = 0
            Dim dOverheadCEInHouseSupport As Double = 0

            Dim dTotalCEWriteOff As Double = 0
            Dim dOverheadCEWriteOff As Double = 0

            Dim dTotalSavings As Double = 0
            Dim dTotalSavingsBudget As Double = 0

            lblProjectNo.Text = ViewState("pProjNo")
            ViewState("CustomerPartNo") = ""
            ViewState("isNewRecord") = True
            ViewState("DateSubmitted") = ""
            ViewState("OriginalAnnCostSave") = 0
            ViewState("OriginalCapEx") = 0
            ViewState("LeaderTMID") = 0
            ViewState("isPlantControllerReviewed") = False

            ds = CRModule.GetCostReduction(ViewState("pProjNo"), 0, "", 0, 0, "", 0, False, False, "")
            If commonFunctions.CheckDataSet(ds) = True Then
                lblDescription.Text = ds.Tables(0).Rows(0).Item("Description").ToString

                If ds.Tables(0).Rows(0).Item("LeaderTMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("LeaderTMID") > 0 Then
                        ViewState("LeaderTMID") = ds.Tables(0).Rows(0).Item("LeaderTMID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("ProjectCategoryID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ProjectCategoryID") > 0 Then
                        ViewState("ProjectCategoryID") = ds.Tables(0).Rows(0).Item("ProjectCategoryID")
                    End If
                End If

                ViewState("DateSubmitted") = ds.Tables(0).Rows(0).Item("DateSubmitted").ToString

                If ds.Tables(0).Rows(0).Item("EstAnnualCostSave") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("EstAnnualCostSave") <> 0 Then
                        ViewState("OriginalAnnCostSave") = ds.Tables(0).Rows(0).Item("EstAnnualCostSave")
                    End If
                End If

                ViewState("Description") = ds.Tables(0).Rows(0).Item("Description").ToString

                If ds.Tables(0).Rows(0).Item("CapEx") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CapEx") <> 0 Then
                        ViewState("OriginalCapEx") = ds.Tables(0).Rows(0).Item("CapEx")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("isPlantControllerReviewed") IsNot System.DBNull.Value Then
                    ViewState("isPlantControllerReviewed") = ds.Tables(0).Rows(0).Item("isPlantControllerReviewed")
                End If

                ViewState("UGNFacility") = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
            End If

            ds = CRModule.GetCostReductionDetail(ViewState("pProjNo"))
            If commonFunctions.CheckDataSet(ds) = True Then
                ViewState("isNewRecord") = False

                'get info from DB
                txtCurrentMethod.Text = ds.Tables(0).Rows(0).Item("CurrentMethod").ToString
                txtProposedMethod.Text = ds.Tables(0).Rows(0).Item("ProposedMethod").ToString
                txtBenefits.Text = ds.Tables(0).Rows(0).Item("Benefits").ToString
                txtCustomerPartNo.Text = ds.Tables(0).Rows(0).Item("CustomerPartNo").ToString
                ViewState("CustomerPartNo") = ds.Tables(0).Rows(0).Item("CustomerPartNo").ToString

                'material price
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPrice") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPrice") <> 0 Then
                        txtMaterialPriceCurrentPrice.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPrice"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceBudget") <> 0 Then
                        txtMaterialPriceCurrentPriceBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceBudget"), "##.00000")                        
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreight") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreight") <> 0 Then
                        txtMaterialPriceCurrentFreight.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreight"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightBudget") <> 0 Then
                        txtMaterialPriceCurrentFreightBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightBudget"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentVolume") <> 0 Then
                        txtMaterialPriceCurrentVolume.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentVolume"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentVolumeBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentVolumeBudget") <> 0 Then
                        txtMaterialPriceCurrentVolumeBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentVolumeBudget"), "##")
                    End If
                End If

                lblMaterialPriceCurrentPriceByVolume.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceByVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceByVolume") <> 0 Then
                        lblMaterialPriceCurrentPriceByVolume.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceByVolume"), "##0.00000")
                    End If
                End If

                lblMaterialPriceCurrentPriceByVolumeBudget.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceByVolumeBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceByVolumeBudget") <> 0 Then
                        lblMaterialPriceCurrentPriceByVolumeBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentPriceByVolumeBudget"), "##0.00000")
                    End If
                End If

                lblMaterialPriceCurrentFreightByVolume.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightByVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightByVolume") <> 0 Then
                        lblMaterialPriceCurrentFreightByVolume.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightByVolume"), "##0.00000")
                    End If
                End If

                lblMaterialPriceCurrentFreightByVolumeBudget.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightByVolumeBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightByVolumeBudget") <> 0 Then
                        lblMaterialPriceCurrentFreightByVolumeBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentFreightByVolumeBudget"), "##0.00000")
                    End If
                End If

                lblMaterialPriceCurrentMaterialLanded.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLanded") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLanded") <> 0 Then
                        lblMaterialPriceCurrentMaterialLanded.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLanded"), "##0.00000")
                    End If
                End If

                lblMaterialPriceCurrentMaterialLandedBudget.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedBudget") <> 0 Then
                        lblMaterialPriceCurrentMaterialLandedBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedBudget"), "##0.00000")
                    End If
                End If

                lblMaterialPriceCurrentMaterialLandedTotal.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedTotal") <> 0 Then
                        lblMaterialPriceCurrentMaterialLandedTotal.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedTotal"), "##0.00000")
                    End If
                End If

                lblMaterialPriceCurrentMaterialLandedTotalBudget.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedTotalBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedTotalBudget") <> 0 Then
                        lblMaterialPriceCurrentMaterialLandedTotalBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMaterialLandedTotalBudget"), "##0.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceProposedPrice") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceProposedPrice") <> 0 Then
                        txtMaterialPriceProposedPrice.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceProposedPrice"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceProposedFreight") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceProposedFreight") <> 0 Then
                        txtMaterialPriceProposedFreight.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceProposedFreight"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceProposedVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceProposedVolume") <> 0 Then
                        txtMaterialPriceProposedVolume.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceProposedVolume"), "##")
                    End If
                End If

                lblMaterialPriceProposedPriceByVolume.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceProposedPriceByVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceProposedPriceByVolume") <> 0 Then
                        lblMaterialPriceProposedPriceByVolume.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceProposedPriceByVolume"), "##0.00000")
                    End If
                End If

                lblMaterialPriceProposedFreightByVolume.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceProposedFreightByVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceProposedFreightByVolume") <> 0 Then
                        lblMaterialPriceProposedFreightByVolume.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceProposedFreightByVolume"), "##0.00000")
                    End If
                End If

                lblMaterialPriceProposedMaterialLanded.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceProposedMaterialLanded") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceProposedMaterialLanded") <> 0 Then
                        lblMaterialPriceProposedMaterialLanded.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceProposedMaterialLanded"), "##0.00000")
                    End If
                End If

                lblMaterialPriceProposedMaterialLandedTotal.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialPriceProposedMaterialLandedTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceProposedMaterialLandedTotal") <> 0 Then
                        lblMaterialPriceProposedMaterialLandedTotal.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceProposedMaterialLandedTotal"), "##0.00000")
                    End If
                End If

                lblMaterialPriceCurrentMethod.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMethod") <> 0 Then
                        lblMaterialPriceCurrentMethod.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMethod"), "##0.00")
                    End If
                End If

                lblMaterialPriceCurrentMethodBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMethodBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMethodBudget") <> 0 Then
                        lblMaterialPriceCurrentMethodBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCurrentMethodBudget"), "##0.00")
                    End If
                End If

                lblMaterialPriceProposedMethod.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialPriceProposedMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceProposedMethod") <> 0 Then
                        lblMaterialPriceProposedMethod.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceProposedMethod"), "##0.00")
                    End If
                End If

                lblMaterialPriceSavings.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialPriceSavings") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceSavings") <> 0 Then
                        lblMaterialPriceSavings.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceSavings"), "##0.00")
                        dMaterialPriceSavings = ds.Tables(0).Rows(0).Item("MaterialPriceSavings")
                    End If
                End If

                lblMaterialPriceSavingsBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialPriceSavingsBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceSavingsBudget") <> 0 Then
                        lblMaterialPriceSavingsBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceSavingsBudget"), "##0.00")
                        dMaterialPriceSavingsBudget = ds.Tables(0).Rows(0).Item("MaterialPriceSavingsBudget")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCECapital") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCECapital") <> 0 Then
                        txtMaterialPriceCECapital.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCECapital"), "##.00")
                        dMaterialPriceCECapital = ds.Tables(0).Rows(0).Item("MaterialPriceCECapital")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCEMaterial") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCEMaterial") <> 0 Then
                        txtMaterialPriceCEMaterial.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCEMaterial"), "##.00")
                        dMaterialPriceCEMaterial = ds.Tables(0).Rows(0).Item("MaterialPriceCEMaterial")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCEOutsideSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCEOutsideSupport") <> 0 Then
                        txtMaterialPriceCEOutsideSupport.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCEOutsideSupport"), "##.00")
                        dMaterialPriceCEOutsideSupport = ds.Tables(0).Rows(0).Item("MaterialPriceCEOutsideSupport")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCEMisc") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCEMisc") <> 0 Then
                        txtMaterialPriceCEMisc.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCEMisc"), "##.00")
                        dMaterialPriceCEMisc = ds.Tables(0).Rows(0).Item("MaterialPriceCEMisc")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialPriceCEInHouseSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCEInHouseSupport") <> 0 Then
                        txtMaterialPriceCEInHouseSupport.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCEInHouseSupport"), "##.00")
                        dMaterialPriceCEInHouseSupport = ds.Tables(0).Rows(0).Item("MaterialPriceCEInHouseSupport")
                    End If
                End If

                lblMaterialPriceCETotal.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialPriceCETotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPriceCETotal") <> 0 Then
                        lblMaterialPriceCETotal.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPriceCETotal"), "##0.00")
                    End If
                End If

                lblMaterialPriceSavingsANDCE.Text = lblMaterialPriceCETotal.Text & " / " & lblMaterialPriceSavings.Text & " = "

                lblMaterialPricePayback.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialPricePayback") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialPricePayback") <> 0 Then
                        lblMaterialPricePayback.Text = Format(ds.Tables(0).Rows(0).Item("MaterialPricePayback"), "##0.00")
                    End If
                End If

                'material usage
                If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostPerUnit") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostPerUnit") <> 0 Then
                        txtMaterialUsageCurrentCostPerUnit.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostPerUnit"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostPerUnitBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostPerUnitBudget") <> 0 Then
                        txtMaterialUsageCurrentCostPerUnitBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostPerUnitBudget"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentUnitPerParent") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentUnitPerParent") <> 0 Then
                        txtMaterialUsageCurrentUnitPerParent.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCurrentUnitPerParent"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentUnitPerParentBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentUnitPerParentBudget") <> 0 Then
                        txtMaterialUsageCurrentUnitPerParentBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCurrentUnitPerParentBudget"), "##.00000")
                    End If
                End If

                lblMaterialUsageCurrentCostTotal.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostTotal") <> 0 Then
                        lblMaterialUsageCurrentCostTotal.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostTotal"), "##0.00000")
                    End If
                End If

                lblMaterialUsageCurrentCostTotalBudget.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostTotalBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostTotalBudget") <> 0 Then
                        lblMaterialUsageCurrentCostTotalBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCurrentCostTotalBudget"), "##0.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageProposedCostPerUnit") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageProposedCostPerUnit") <> 0 Then
                        txtMaterialUsageProposedCostPerUnit.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageProposedCostPerUnit"), "##.00000")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageProposedUnitPerParent") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageProposedUnitPerParent") <> 0 Then
                        txtMaterialUsageProposedUnitPerParent.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageProposedUnitPerParent"), "##.00000")
                    End If
                End If

                lblMaterialUsageProposedCostTotal.Text = "0.00000"
                If ds.Tables(0).Rows(0).Item("MaterialUsageProposedCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageProposedCostTotal") <> 0 Then
                        lblMaterialUsageProposedCostTotal.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageProposedCostTotal"), "##0.00000")
                    End If
                End If

                lblMaterialUsageCurrentMethod.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentMethod") <> 0 Then
                        lblMaterialUsageCurrentMethod.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCurrentMethod"), "##0.00")
                    End If
                End If

                lblMaterialUsageCurrentMethodBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentMethodBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCurrentMethodBudget") <> 0 Then
                        lblMaterialUsageCurrentMethodBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCurrentMethodBudget"), "##0.00")
                    End If
                End If

                lblMaterialUsageProposedMethod.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialUsageProposedMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageProposedMethod") <> 0 Then
                        lblMaterialUsageProposedMethod.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageProposedMethod"), "##0.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageProgramVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageProgramVolume") <> 0 Then
                        txtMaterialUsageProgramVolume.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageProgramVolume"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageProgramVolumeBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageProgramVolumeBudget") <> 0 Then
                        txtMaterialUsageProgramVolumeBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageProgramVolumeBudget"), "##")
                    End If
                End If

                lblMaterialUsageSavings.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialUsageSavings") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageSavings") <> 0 Then
                        lblMaterialUsageSavings.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageSavings"), "##0.00")
                        dMaterialUsageSavings = ds.Tables(0).Rows(0).Item("MaterialUsageSavings")
                    End If
                End If

                lblMaterialUsageSavingsBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialUsageSavingsBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageSavingsBudget") <> 0 Then
                        lblMaterialUsageSavingsBudget.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageSavingsBudget"), "##0.00")
                        dMaterialUsageSavingsBudget = ds.Tables(0).Rows(0).Item("MaterialUsageSavingsBudget")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageCECapital") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCECapital") <> 0 Then
                        txtMaterialUsageCECapital.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCECapital"), "##.00")
                        dMaterialUsageCECapital = ds.Tables(0).Rows(0).Item("MaterialUsageCECapital")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageCEMaterial") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCEMaterial") <> 0 Then
                        txtMaterialUsageCEMaterial.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCEMaterial"), "##.00")
                        dMaterialPriceCEMaterial = ds.Tables(0).Rows(0).Item("MaterialUsageCEMaterial")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageCEOutsideSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCEOutsideSupport") <> 0 Then
                        txtMaterialUsageCEOutsideSupport.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCEOutsideSupport"), "##.00")
                        dMaterialUsageCEOutsideSupport = ds.Tables(0).Rows(0).Item("MaterialUsageCEOutsideSupport")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageCEMisc") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCEMisc") <> 0 Then
                        txtMaterialUsageCEMisc.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCEMisc"), "##.00")
                        dMaterialUsageCEMisc = ds.Tables(0).Rows(0).Item("MaterialUsageCEMisc")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("MaterialUsageCEInHouseSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCEInHouseSupport") <> 0 Then
                        txtMaterialUsageCEInHouseSupport.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCEInHouseSupport"), "##.00")
                        dMaterialUsageCEInHouseSupport = ds.Tables(0).Rows(0).Item("MaterialUsageCEInHouseSupport")
                    End If
                End If

                lblMaterialUsageCETotal.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialUsageCETotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsageCETotal") <> 0 Then
                        lblMaterialUsageCETotal.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsageCETotal"), "##0.00")
                    End If
                End If

                lblMaterialUsageSavingsANDCE.Text = lblMaterialUsageCETotal.Text & " / " & lblMaterialUsageSavings.Text & " = "

                lblMaterialUsagePayback.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("MaterialUsagePayback") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("MaterialUsagePayback") <> 0 Then
                        lblMaterialUsagePayback.Text = Format(ds.Tables(0).Rows(0).Item("MaterialUsagePayback"), "##0.00")
                    End If
                End If

                lblTotalSavingsMaterialPriceAndUsage.Text = Format((dMaterialPriceSavings + dMaterialUsageSavings), "##0.00")
                lblTotalSavingsMaterialPriceAndUsageBudget.Text = Format((dMaterialPriceSavingsBudget + dMaterialUsageSavingsBudget), "##0.00")

                'cycle time
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentPiecesPerHour") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentPiecesPerHour") <> 0 Then
                        txtCycleTimeCurrentPiecesPerHour.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentPiecesPerHour"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentPiecesPerHourBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentPiecesPerHourBudget") <> 0 Then
                        txtCycleTimeCurrentPiecesPerHourBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentPiecesPerHourBudget"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentCrewSize") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentCrewSize") <> 0 Then
                        txtCycleTimeCurrentCrewSize.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentCrewSize"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentCrewSizeBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentCrewSizeBudget") <> 0 Then
                        txtCycleTimeCurrentCrewSizeBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentCrewSizeBudget"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentVolume") <> 0 Then
                        txtCycleTimeCurrentVolume.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentVolume"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentVolumeBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentVolumeBudget") <> 0 Then
                        txtCycleTimeCurrentVolumeBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentVolumeBudget"), "##")
                    End If
                End If

                lblCycleTimeCurrentMachineHourPerPieces.Text = "0.0000"
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentMachineHourPerPieces") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentMachineHourPerPieces") <> 0 Then
                        lblCycleTimeCurrentMachineHourPerPieces.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentMachineHourPerPieces"), "##0.0000")
                    End If
                End If

                lblCycleTimeCurrentMachineHourPerPiecesBudget.Text = "0.0000"
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentMachineHourPerPiecesBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentMachineHourPerPiecesBudget") <> 0 Then
                        lblCycleTimeCurrentMachineHourPerPiecesBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentMachineHourPerPiecesBudget"), "##0.0000")
                    End If
                End If

                lblCycleTimeCurrentManHourPerPieces.Text = "0.0000"
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentManHourPerPieces") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentManHourPerPieces") <> 0 Then
                        lblCycleTimeCurrentManHourPerPieces.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentManHourPerPieces"), "##0.0000")
                    End If
                End If

                lblCycleTimeCurrentManHourPerPiecesBudget.Text = "0.0000"
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentManHourPerPiecesBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentManHourPerPiecesBudget") <> 0 Then
                        lblCycleTimeCurrentManHourPerPiecesBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentManHourPerPiecesBudget"), "##0.0000")
                    End If
                End If

                lblCycleTimeCurrentTotalManHours.Text = "0"
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentTotalManHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentTotalManHours") <> 0 Then
                        lblCycleTimeCurrentTotalManHours.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentTotalManHours"), "##")
                    End If
                End If

                lblCycleTimeCurrentTotalManHoursBudget.Text = "0"
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentTotalManHoursBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentTotalManHoursBudget") <> 0 Then
                        lblCycleTimeCurrentTotalManHoursBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentTotalManHoursBudget"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeProposedPiecesPerHour") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeProposedPiecesPerHour") <> 0 Then
                        txtCycleTimeProposedPiecesPerHour.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeProposedPiecesPerHour"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeProposedCrewSize") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeProposedCrewSize") <> 0 Then
                        txtCycleTimeProposedCrewSize.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeProposedCrewSize"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeProposedVolume") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeProposedVolume") <> 0 Then
                        txtCycleTimeProposedVolume.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeProposedVolume"), "##")
                    End If
                End If

                lblCycleTimeProposedMachineHourPerPieces.Text = "0.0000"
                If ds.Tables(0).Rows(0).Item("CycleTimeProposedMachineHourPerPieces") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeProposedMachineHourPerPieces") <> 0 Then
                        lblCycleTimeProposedMachineHourPerPieces.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeProposedMachineHourPerPieces"), "##0.0000")
                    End If
                End If

                lblCycleTimeProposedManHourPerPieces.Text = "0.0000"
                If ds.Tables(0).Rows(0).Item("CycleTimeProposedManHourPerPieces") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeProposedManHourPerPieces") <> 0 Then
                        lblCycleTimeProposedManHourPerPieces.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeProposedManHourPerPieces"), "##0.0000")
                    End If
                End If

                lblCycleTimeProposedTotalManHours.Text = "0"
                If ds.Tables(0).Rows(0).Item("CycleTimeProposedTotalManHours") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeProposedTotalManHours") <> 0 Then
                        lblCycleTimeProposedTotalManHours.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeProposedTotalManHours"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeFUTARate") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeFUTARate") <> 0 Then
                        txtCycleTimeFUTARate.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeFUTARate"), "##.00")
                        lblCycleTimeFUTARateDecimal.Text = "(=" & Format((ds.Tables(0).Rows(0).Item("CycleTimeFUTARate") / 100), "##0.0000") & ")"
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeSUTARate") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeSUTARate") <> 0 Then
                        txtCycleTimeSUTARate.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeSUTARate"), "##.00")
                        lblCycleTimeSUTARateDecimal.Text = "(=" & Format((ds.Tables(0).Rows(0).Item("CycleTimeSUTARate") / 100), "##0.0000") & ")"
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeFICARate") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeFICARate") <> 0 Then
                        txtCycleTimeFICARate.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeFICARate"), "##.00")
                        lblCycleTimeFICARateDecimal.Text = "(=" & Format((ds.Tables(0).Rows(0).Item("CycleTimeFICARate") / 100), "##0.0000") & ")"
                    End If
                End If

                lblCycleTimeVariableFringes.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeVariableFringes") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeVariableFringes") <> 0 Then
                        lblCycleTimeVariableFringes.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeVariableFringes"), "##0.00")
                        lblCycleTimeVariableFringesDecimal.Text = "(=" & Format((ds.Tables(0).Rows(0).Item("CycleTimeVariableFringes") / 100), "##0.0000") & ")"
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeWages") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeWages") <> 0 Then
                        txtCycleTimeWages.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeWages"), "##.00")
                    End If
                End If

                lblCycleTimeWagesPlusFringes.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeWagesPlusFringes") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeWagesPlusFringes") <> 0 Then
                        lblCycleTimeWagesPlusFringes.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeWagesPlusFringes"), "##0.00")
                    End If
                End If

                lblCycleTimeCurrentMethod.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentMethod") <> 0 Then
                        lblCycleTimeCurrentMethod.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentMethod"), "##0.00")
                    End If
                End If

                lblCycleTimeCurrentMethodBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeCurrentMethodBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCurrentMethodBudget") <> 0 Then
                        lblCycleTimeCurrentMethodBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCurrentMethodBudget"), "##0.00")
                    End If
                End If

                lblCycleTimeProposedMethod.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeProposedMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeProposedMethod") <> 0 Then
                        lblCycleTimeProposedMethod.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeProposedMethod"), "##0.00")
                    End If
                End If

                lblCycleTimeMethodDifference.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeMethodDifference") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeMethodDifference") <> 0 Then
                        lblCycleTimeMethodDifference.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeMethodDifference"), "##0.00")
                    End If
                End If

                lblCycleTimeMethodDifferenceBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeMethodDifferenceBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeMethodDifferenceBudget") <> 0 Then
                        lblCycleTimeMethodDifferenceBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeMethodDifferenceBudget"), "##0.00")
                    End If
                End If

                lblCycleTimeSavings.Text = "0.00"
                lblTotalSavingsCycleTime.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeSavings") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeSavings") <> 0 Then
                        lblCycleTimeSavings.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeSavings"), "##0.00")
                        lblTotalSavingsCycleTime.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeSavings"), "##0.00")
                    End If
                End If

                lblCycleTimeSavingsBudget.Text = "0.00"
                lblTotalSavingsCycleTimeBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeSavingsBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeSavingsBudget") <> 0 Then
                        lblCycleTimeSavingsBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeSavingsBudget"), "##0.00")
                        lblTotalSavingsCycleTimeBudget.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeSavingsBudget"), "##0.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCECapital") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCECapital") <> 0 Then
                        txtCycleTimeCECapital.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCECapital"), "##.00")
                        dCycleTimeCECapital = ds.Tables(0).Rows(0).Item("CycleTimeCECapital")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCEMaterial") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCEMaterial") <> 0 Then
                        txtCycleTimeCEMaterial.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCEMaterial"), "##.00")
                        dCycleTimeCEMaterial = ds.Tables(0).Rows(0).Item("CycleTimeCEMaterial")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCEOutsideSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCEOutsideSupport") <> 0 Then
                        txtCycleTimeCEOutsideSupport.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCEOutsideSupport"), "##.00")
                        dCycleTimeCEOutsideSupport = ds.Tables(0).Rows(0).Item("CycleTimeCEOutsideSupport")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCEMisc") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCEMisc") <> 0 Then
                        txtCycleTimeCEMisc.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCEMisc"), "##.00")
                        dCycleTimeCEMisc = ds.Tables(0).Rows(0).Item("CycleTimeCEMisc")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CycleTimeCEInHouseSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCEInHouseSupport") <> 0 Then
                        txtCycleTimeCEInHouseSupport.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCEInHouseSupport"), "##.00")
                        dCycleTimeCEInHouseSupport = ds.Tables(0).Rows(0).Item("CycleTimeCEInHouseSupport")
                    End If
                End If

                lblCycleTimeCETotal.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimeCETotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimeCETotal") <> 0 Then
                        lblCycleTimeCETotal.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimeCETotal"), "##0.00")
                    End If
                End If

                lblCycleTimeSavingsANDCE.Text = lblCycleTimeCETotal.Text & " / " & lblCycleTimeSavings.Text & " = "

                lblCycleTimePayback.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("CycleTimePayback") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CycleTimePayback") <> 0 Then
                        lblCycleTimePayback.Text = Format(ds.Tables(0).Rows(0).Item("CycleTimePayback"), "##0.00")
                    End If
                End If

                'head count
                If ds.Tables(0).Rows(0).Item("HeadCountWages") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountWages") <> 0 Then
                        txtHeadCountWages.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountWages"), "##.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountWagesBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountWagesBudget") <> 0 Then
                        txtHeadCountWagesBudget.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountWages"), "##.00")
                    End If
                End If

                lblHeadCountAnnualLaborCost.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountAnnualLaborCost") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountAnnualLaborCost") <> 0 Then
                        lblHeadCountAnnualLaborCost.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountAnnualLaborCost"), "##0.00")
                    End If
                End If

                lblHeadCountAnnualLaborCostBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountAnnualLaborCostBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountAnnualLaborCostBudget") <> 0 Then
                        lblHeadCountAnnualLaborCostBudget.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountAnnualLaborCostBudget"), "##0.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCount") <> 0 Then
                        txtHeadCountCurrentLaborCount.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCount"), "##")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCountBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCountBudget") <> 0 Then
                        txtHeadCountCurrentLaborCountBudget.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCountBudget"), "##")
                    End If
                End If

                lblHeadCountCurrentLaborCost.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCost") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCost") <> 0 Then
                        lblHeadCountCurrentLaborCost.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCost"), "##0.00")
                    End If
                End If

                lblHeadCountCurrentLaborCostBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCostBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCostBudget") <> 0 Then
                        lblHeadCountCurrentLaborCostBudget.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborCostBudget"), "##0.00")
                    End If
                End If

                lblHeadCountCurrentLaborFringes.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborFringes") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborFringes") <> 0 Then
                        lblHeadCountCurrentLaborFringes.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborFringes"), "##0.00")
                    End If
                End If

                lblHeadCountCurrentLaborTotal.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborTotal") <> 0 Then
                        lblHeadCountCurrentLaborTotal.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborTotal"), "##0.00")
                    End If
                End If

                lblHeadCountCurrentLaborTotalBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborTotalBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborTotalBudget") <> 0 Then
                        lblHeadCountCurrentLaborTotalBudget.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentLaborTotalBudget"), "##0.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountProposedLaborCount") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountProposedLaborCount") <> 0 Then
                        txtHeadCountProposedLaborCount.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountProposedLaborCount"), "##")
                    End If
                End If

                lblHeadCountProposedLaborCost.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountProposedLaborCost") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountProposedLaborCost") <> 0 Then
                        lblHeadCountProposedLaborCost.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountProposedLaborCost"), "##0.00")
                    End If
                End If

                lblHeadCountProposedLaborFringes.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountProposedLaborFringes") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountProposedLaborFringes") <> 0 Then
                        lblHeadCountProposedLaborFringes.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountProposedLaborFringes"), "##0.00")
                    End If
                End If

                lblHeadCountProposedLaborTotal.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountProposedLaborTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountProposedLaborTotal") <> 0 Then
                        lblHeadCountProposedLaborTotal.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountProposedLaborTotal"), "##0.00")
                    End If
                End If

                lblHeadCountCurrentMethod.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountCurrentMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentMethod") <> 0 Then
                        lblHeadCountCurrentMethod.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentMethod"), "##0.00")
                    End If
                End If

                lblHeadCountCurrentMethodBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountCurrentMethodBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCurrentMethodBudget") <> 0 Then
                        lblHeadCountCurrentMethodBudget.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCurrentMethodBudget"), "##0.00")
                    End If
                End If

                lblHeadCountProposedMethod.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountProposedMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountProposedMethod") <> 0 Then
                        lblHeadCountProposedMethod.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountProposedMethod"), "##0.00")
                    End If
                End If

                lblHeadCountSavings.Text = "0.00"
                lblTotalSavingsHeadCount.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountSavings") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountSavings") <> 0 Then
                        lblHeadCountSavings.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountSavings"), "##0.00")
                        lblTotalSavingsHeadCount.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountSavings"), "##0.00")
                    End If
                End If

                lblHeadCountSavingsBudget.Text = "0.00"
                lblTotalSavingsHeadCountBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountSavingsBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountSavingsBudget") <> 0 Then
                        lblHeadCountSavingsBudget.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountSavingsBudget"), "##0.00")
                        lblTotalSavingsHeadCountBudget.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountSavingsBudget"), "##0.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountFUTA") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountFUTA") <> 0 Then
                        txtHeadCountFUTA.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountFUTA"), "##.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountSUTA") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountSUTA") <> 0 Then
                        txtHeadCountSUTA.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountSUTA"), "##.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountFICA") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountFICA") <> 0 Then
                        txtHeadCountFICA.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountFICA"), "##.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountPension") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountPension") <> 0 Then
                        txtHeadCountPension.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountPension"), "##.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountBonus") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountBonus") <> 0 Then
                        txtHeadCountBonus.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountBonus"), "##.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountLife") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountLife") <> 0 Then
                        txtHeadCountLife.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountLife"), "##.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountGroupInsurance") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountGroupInsurance") <> 0 Then
                        txtHeadCountGroupInsurance.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountGroupInsurance"), "##.00")
                    End If
                End If

                'special case - fixed value
                lblHeadCountWorkersComp.Text = ""
                If ds.Tables(0).Rows(0).Item("HeadCountWorkersComp") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountWorkersComp") <> 0 Then
                        lblHeadCountWorkersComp.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountWorkersComp"), "##.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountPensionQuarterly") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountPensionQuarterly") <> 0 Then
                        txtHeadCountPensionQuarterly.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountPensionQuarterly"), "##.00")
                    End If
                End If

                lblHeadCountTotalFringes.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountTotalFringes") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountTotalFringes") <> 0 Then
                        lblHeadCountTotalFringes.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountTotalFringes"), "##0.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountCECapital") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCECapital") <> 0 Then
                        txtHeadCountCECapital.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCECapital"), "##.00")
                        dHeadCountCECapital = ds.Tables(0).Rows(0).Item("HeadCountCECapital")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountCEMaterial") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCEMaterial") <> 0 Then
                        txtHeadCountCEMaterial.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCEMaterial"), "##.00")
                        dHeadCountCEMaterial = ds.Tables(0).Rows(0).Item("HeadCountCEMaterial")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountCEOutsideSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCEOutsideSupport") <> 0 Then
                        txtHeadCountCEOutsideSupport.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCEOutsideSupport"), "##.00")
                        dHeadCountCEOutsideSupport = ds.Tables(0).Rows(0).Item("HeadCountCEOutsideSupport")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountCEMisc") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCEMisc") <> 0 Then
                        txtHeadCountCEMisc.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCEMisc"), "##.00")
                        dHeadCountCEMisc = ds.Tables(0).Rows(0).Item("HeadCountCEMisc")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("HeadCountCEInHouseSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCEInHouseSupport") <> 0 Then
                        txtHeadCountCEInHouseSupport.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCEInHouseSupport"), "##.00")
                        dHeadCountCEInHouseSupport = ds.Tables(0).Rows(0).Item("HeadCountCEInHouseSupport")
                    End If
                End If

                lblHeadCountCETotal.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountCETotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountCETotal") <> 0 Then
                        lblHeadCountCETotal.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountCETotal"), "##0.00")
                    End If
                End If

                lblHeadCountSavingsANDCE.Text = lblHeadCountCETotal.Text & " / " & lblHeadCountSavings.Text & " = "

                lblHeadCountPayback.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("HeadCountPayback") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("HeadCountPayback") <> 0 Then
                        lblHeadCountPayback.Text = Format(ds.Tables(0).Rows(0).Item("HeadCountPayback"), "##0.00")
                    End If
                End If

                'overhead

                lblOverheadCurrentMethod.Text = "0.00"
                lblOverheadCurrentTotalCost.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("OverheadCurrentMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCurrentMethod") <> 0 Then
                        lblOverheadCurrentMethod.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCurrentMethod"), "##0.00")
                        lblOverheadCurrentTotalCost.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCurrentMethod"), "##0.00")
                    End If
                End If

                lblOverheadCurrentMethodBudget.Text = "0.00"
                lblOverheadCurrentTotalCostBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("OverheadCurrentMethodBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCurrentMethodBudget") <> 0 Then
                        lblOverheadCurrentMethodBudget.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCurrentMethodBudget"), "##0.00")
                        lblOverheadCurrentTotalCostBudget.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCurrentMethodBudget"), "##0.00")
                    End If
                End If

                lblOverheadProposedMethod.Text = "0.00"
                lblOverheadProposedTotalCost.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("OverheadProposedMethod") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadProposedMethod") <> 0 Then
                        lblOverheadProposedMethod.Text = Format(ds.Tables(0).Rows(0).Item("OverheadProposedMethod"), "##0.00")
                        lblOverheadProposedTotalCost.Text = Format(ds.Tables(0).Rows(0).Item("OverheadProposedMethod"), "##0.00")
                    End If
                End If

                lblOverheadSavings.Text = "0.00"
                lblTotalSavingsOverhead.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("OverheadSavings") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadSavings") <> 0 Then
                        lblOverheadSavings.Text = Format(ds.Tables(0).Rows(0).Item("OverheadSavings"), "##0.00")
                        lblTotalSavingsOverhead.Text = Format(ds.Tables(0).Rows(0).Item("OverheadSavings"), "##0.00")
                    End If
                End If

                lblOverheadSavingsBudget.Text = "0.00"
                lblTotalSavingsOverheadBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("OverheadSavingsBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadSavingsBudget") <> 0 Then
                        lblOverheadSavingsBudget.Text = Format(ds.Tables(0).Rows(0).Item("OverheadSavingsBudget"), "##0.00")
                        lblTotalSavingsOverheadBudget.Text = Format(ds.Tables(0).Rows(0).Item("OverheadSavingsBudget"), "##0.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("OverheadCECapital") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCECapital") <> 0 Then
                        txtOverheadCECapital.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCECapital"), "##.00")
                        dOverheadCECapital = ds.Tables(0).Rows(0).Item("OverheadCECapital")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("OverheadCEMaterial") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCEMaterial") <> 0 Then
                        txtOverheadCEMaterial.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCEMaterial"), "##.00")
                        dOverheadCEMaterial = ds.Tables(0).Rows(0).Item("OverheadCEMaterial")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("OverheadCEOutsideSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCEOutsideSupport") <> 0 Then
                        txtOverheadCEOutsideSupport.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCEOutsideSupport"), "##.00")
                        dOverheadCEOutsideSupport = ds.Tables(0).Rows(0).Item("OverheadCEOutsideSupport")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("OverheadCEMisc") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCEMisc") <> 0 Then
                        txtOverheadCEMisc.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCEMisc"), "##.00")
                        dOverheadCEMisc = ds.Tables(0).Rows(0).Item("OverheadCEMisc")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("OverheadCEInHouseSupport") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCEInHouseSupport") <> 0 Then
                        txtOverheadCEInHouseSupport.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCEInHouseSupport"), "##.00")
                        dOverheadCEInHouseSupport = ds.Tables(0).Rows(0).Item("OverheadCEInHouseSupport")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("OverheadCEWriteOff") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCEWriteOff") <> 0 Then
                        txtOverheadCEWriteOff.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCEWriteOff"), "##.00")
                        dOverheadCEWriteOff = ds.Tables(0).Rows(0).Item("OverheadCEWriteOff")
                    End If
                End If

                lblOverheadCETotal.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("OverheadCETotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadCETotal") <> 0 Then
                        lblOverheadCETotal.Text = Format(ds.Tables(0).Rows(0).Item("OverheadCETotal"), "##0.00")
                    End If
                End If

                lblOverheadSavingsANDCE.Text = lblOverheadCETotal.Text & " / " & lblOverheadSavings.Text & " = "

                lblOverheadPayback.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("OverheadPayback") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("OverheadPayback") <> 0 Then
                        lblOverheadPayback.Text = Format(ds.Tables(0).Rows(0).Item("OverheadPayback"), "##0.00")
                    End If
                End If

                'totals
                dTotalCECapital = dMaterialPriceCECapital + dMaterialUsageCECapital + dCycleTimeCECapital + dHeadCountCECapital + dOverheadCECapital

                lblTotalCECapital.Text = "0.00"
                If dTotalCECapital > 0 Then
                    lblTotalCECapital.Text = Format(dTotalCECapital, "##0.00")
                End If

                dTotalCEMaterial = dMaterialPriceCEMaterial + dMaterialUsageCEMaterial + dCycleTimeCEMaterial + dHeadCountCEMaterial + dOverheadCEMaterial

                lblTotalCEMaterial.Text = "0.00"
                If dTotalCEMaterial > 0 Then
                    lblTotalCEMaterial.Text = Format(dTotalCEMaterial, "##0.00")
                End If

                dTotalCEOutsideSupport = dMaterialPriceCEOutsideSupport + dMaterialUsageCEOutsideSupport + dCycleTimeCEOutsideSupport + dHeadCountCEOutsideSupport + dOverheadCEOutsideSupport

                lblTotalCEOutsideSupport.Text = "0.00"
                If dTotalCEOutsideSupport > 0 Then
                    lblTotalCEOutsideSupport.Text = Format(dTotalCEOutsideSupport, "##0.00")
                End If

                dTotalCEMisc = dMaterialPriceCEMisc + dMaterialUsageCEMisc + dCycleTimeCEMisc + dHeadCountCEMisc + dOverheadCEMisc

                lblTotalCEMisc.Text = "0.00"
                If dTotalCEMisc > 0 Then
                    lblTotalCEMisc.Text = Format(dTotalCEMisc, "##0.00")
                End If

                dTotalCEInHouseSupport = dMaterialPriceCEInHouseSupport + dMaterialUsageCEInHouseSupport + dCycleTimeCEInHouseSupport + dHeadCountCEInHouseSupport + dOverheadCEInHouseSupport

                lblTotalCEInHouseSupport.Text = "0.00"
                If dTotalCEInHouseSupport > 0 Then
                    lblTotalCEInHouseSupport.Text = Format(dTotalCEInHouseSupport, "##0.00")
                End If

                dTotalCEWriteOff = dOverheadCEWriteOff

                lblTotalCEWriteOff.Text = "0.00"
                If dTotalCEWriteOff > 0 Then
                    lblTotalCEWriteOff.Text = Format(dTotalCEWriteOff, "##0.00")
                End If

                lblTotalSavings.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("TotalSavings") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalSavings") <> 0 Then
                        lblTotalSavings.Text = Format(ds.Tables(0).Rows(0).Item("TotalSavings"), "##0.00")
                        dTotalSavings = ds.Tables(0).Rows(0).Item("TotalSavings")
                    End If
                End If

                lblTotalSavingsBudget.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("TotalSavingsBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalSavingsBudget") <> 0 Then
                        lblTotalSavingsBudget.Text = Format(ds.Tables(0).Rows(0).Item("TotalSavingsBudget"), "##0.00")
                        dTotalSavingsBudget = ds.Tables(0).Rows(0).Item("TotalSavingsBudget")
                    End If
                End If

                lblTotalCE.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("TotalCE") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalCE") <> 0 Then
                        lblTotalCE.Text = Format(ds.Tables(0).Rows(0).Item("TotalCE"), "##0.00")
                    End If
                End If

                lblTotalAnnualSavingsANDCE.Text = lblTotalCE.Text & " / " & lblTotalSavings.Text & " = "

                lblTotalPayback.Text = "0.00"
                If ds.Tables(0).Rows(0).Item("TotalPayback") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TotalPayback") <> 0 Then
                        lblTotalPayback.Text = Format(ds.Tables(0).Rows(0).Item("TotalPayback"), "##0.00")
                    End If
                End If

                'customer non-grid info
                If ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar") <> 0 Then
                        rbCustomerGiveBack.SelectedValue = "D"
                        tblCustomerGiveBackByDollar.Visible = True
                        tblCustomerGiveBackByPercent.Visible = False
                        txtCustomerGiveBackDollar.Text = Format(ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar"), "##0.00")
                        lblCustomerGiveBack.Text = Format(ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar"), "##0.00")
                        dCustomerGiveBackDollar = ds.Tables(0).Rows(0).Item("CustomerGiveBackDollar")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CustomerGiveBackPercent") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CustomerGiveBackPercent") <> 0 Then
                        dCustomerGiveBackPercent = ds.Tables(0).Rows(0).Item("CustomerGiveBackPercent")
                    End If
                End If

                'dollar takes precedence over percent
                If dCustomerGiveBackPercent <> 0 And dCustomerGiveBackDollar = 0 Then
                    rbCustomerGiveBack.SelectedValue = "P"
                    tblCustomerGiveBackByDollar.Visible = False
                    tblCustomerGiveBackByPercent.Visible = True

                    txtCustomerGiveBackPercent.Text = dCustomerGiveBackPercent
                    dCustomerGiveBackDollar = dTotalSavings * (dCustomerGiveBackPercent / 100)
                    lblCustomerGiveBack.Text = Format(dCustomerGiveBackDollar, "##0.00")              
                End If

                lblTotalNetSavings.Text = Format(dTotalSavings - dCustomerGiveBackDollar, "##0.00")
                lblTotalNetSavingsBudget.Text = Format(dTotalSavingsBudget - dCustomerGiveBackDollar, "##0.00")

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ClearCalculations()

        Try

            'material price
            lblMaterialPriceCurrentPriceByVolume.Text = ""
            lblMaterialPriceCurrentFreightByVolume.Text = ""
            lblMaterialPriceCurrentMaterialLanded.Text = ""
            lblMaterialPriceCurrentMaterialLandedTotal.Text = ""

            lblMaterialPriceProposedPriceByVolume.Text = ""
            lblMaterialPriceProposedFreightByVolume.Text = ""
            lblMaterialPriceProposedMaterialLanded.Text = ""
            lblMaterialPriceProposedMaterialLandedTotal.Text = ""

            lblMaterialPriceCurrentMethod.Text = ""
            lblMaterialPriceProposedMethod.Text = ""
            lblMaterialPriceSavings.Text = ""
            lblMaterialPriceCETotal.Text = ""
            lblMaterialPriceSavingsANDCE.Text = ""
            lblMaterialPricePayback.Text = ""

            'material usage
            lblMaterialUsageCurrentCostTotal.Text = ""
            lblMaterialUsageProposedCostTotal.Text = ""
            lblMaterialUsageCurrentMethod.Text = ""
            lblMaterialUsageProposedMethod.Text = ""
            lblMaterialUsageSavings.Text = ""
            lblMaterialUsageCETotal.Text = ""
            lblMaterialUsageSavingsANDCE.Text = ""
            lblMaterialUsagePayback.Text = ""

            'cycle time
            lblCycleTimeCurrentMachineHourPerPieces.Text = ""
            lblCycleTimeCurrentManHourPerPieces.Text = ""
            lblCycleTimeCurrentTotalManHours.Text = ""
            lblCycleTimeProposedMachineHourPerPieces.Text = ""
            lblCycleTimeProposedManHourPerPieces.Text = ""
            lblCycleTimeProposedTotalManHours.Text = ""
            lblCycleTimeFUTARateDecimal.Text = ""
            lblCycleTimeCurrentMethod.Text = ""
            lblCycleTimeSUTARateDecimal.Text = ""
            lblCycleTimeProposedMethod.Text = ""
            lblCycleTimeFICARateDecimal.Text = ""
            lblCycleTimeMethodDifference.Text = ""
            lblCycleTimeVariableFringes.Text = ""
            lblCycleTimeWagesPlusFringes.Text = ""
            lblCycleTimeSavings.Text = ""
            lblCycleTimeCETotal.Text = ""
            lblCycleTimeSavingsANDCE.Text = ""
            lblCycleTimePayback.Text = ""

            'headcount
            lblHeadCountAnnualLaborCost.Text = ""
            lblHeadCountCurrentLaborCost.Text = ""
            lblHeadCountCurrentLaborFringes.Text = ""
            lblHeadCountCurrentLaborTotal.Text = ""
            lblHeadCountProposedLaborCost.Text = ""
            lblHeadCountProposedLaborFringes.Text = ""
            lblHeadCountProposedLaborTotal.Text = ""
            lblHeadCountCurrentMethod.Text = ""
            lblHeadCountProposedMethod.Text = ""
            lblHeadCountSavings.Text = ""
            lblHeadCountCETotal.Text = ""
            lblHeadCountSavingsANDCE.Text = ""
            lblHeadCountPayback.Text = ""
            lblHeadCountTotalFringes.Text = ""

            'overhead
            lblOverheadCurrentTotalCost.Text = ""
            lblOverheadProposedTotalCost.Text = ""
            lblOverheadCurrentMethod.Text = ""
            lblOverheadProposedMethod.Text = ""
            lblOverheadSavings.Text = ""
            lblOverheadCETotal.Text = ""
            lblOverheadSavingsANDCE.Text = ""
            lblOverheadPayback.Text = ""

            'top level totals
            lblTotalSavingsMaterialPriceAndUsage.Text = ""
            lblTotalSavingsCycleTime.Text = ""
            lblTotalSavingsHeadCount.Text = ""
            lblTotalSavingsOverhead.Text = ""
            lblTotalSavings.Text = ""

            lblTotalCECapital.Text = ""
            lblTotalCEMaterial.Text = ""
            lblTotalCEOutsideSupport.Text = ""
            lblTotalCEMisc.Text = ""
            lblTotalCEInHouseSupport.Text = ""
            lblTotalCEWriteOff.Text = ""
            lblTotalCE.Text = ""

            lblTotalAnnualSavingsANDCE.Text = ""
            lblTotalPayback.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Calculate(ByVal isUpdate As Boolean)

        Try
            ClearCalculations()

            Dim ds As DataSet

            Dim dCustomerGiveBackDollar As Double = 0
            Dim dCustomerGiveBackPercent As Double = 0

            Dim dMaterialPriceCurrentPrice As Double = 0
            Dim dMaterialPriceCurrentPriceBudget As Double = 0
            Dim iMaterialPriceCurrentVolume As Integer = 0
            Dim iMaterialPriceCurrentVolumeBudget As Integer = 0
            Dim dMaterialPriceCurrentPriceByVolume As Double = 0
            Dim dMaterialPriceCurrentPriceByVolumeBudget As Double = 0
            Dim dMaterialPriceCurrentFreight As Double = 0
            Dim dMaterialPriceCurrentFreightBudget As Double = 0
            Dim dMaterialPriceCurrentFreightByVolume As Double = 0
            Dim dMaterialPriceCurrentFreightByVolumeBudget As Double = 0
            Dim dMaterialPriceCurrentMaterialLanded As Double = 0
            Dim dMaterialPriceCurrentMaterialLandedBudget As Double = 0
            Dim dMaterialPriceCurrentMaterialLandedTotal As Double = 0
            Dim dMaterialPriceCurrentMaterialLandedTotalBudget As Double = 0

            Dim dMaterialPriceProposedPrice As Double = 0
            Dim iMaterialPriceProposedVolume As Integer = 0
            Dim dMaterialPriceProposedPriceByVolume As Double = 0
            Dim dMaterialPriceProposedFreight As Double = 0
            Dim dMaterialPriceProposedFreightByVolume As Double = 0
            Dim dMaterialPriceProposedMaterialLanded As Double = 0
            Dim dMaterialPriceProposedMaterialLandedTotal As Double = 0
            Dim dMaterialPriceSavings As Double = 0
            Dim dMaterialPriceSavingsBudget As Double = 0

            Dim dMaterialPriceCECapital As Double = 0
            Dim dMaterialPriceCEMaterial As Double = 0
            Dim dMaterialPriceCEOutsideSupport As Double = 0
            Dim dMaterialPriceCEMisc As Double = 0
            Dim dMaterialPriceCEInHouseSupport As Double = 0
            Dim dMaterialPriceCETotal As Double = 0
            Dim dMaterialPricePayback As Double = 0

            Dim dMaterialUsageCurrentCostPerUnit As Double = 0
            Dim dMaterialUsageCurrentCostPerUnitBudget As Double = 0
            Dim dMaterialUsageCurrentUnitPerParent As Double = 0
            Dim dMaterialUsageCurrentUnitPerParentBudget As Double = 0
            Dim dMaterialUsageCurrentCostTotal As Double = 0
            Dim dMaterialUsageCurrentCostTotalBudget As Double = 0

            Dim dMaterialUsageProposedCostPerUnit As Double = 0
            Dim dMaterialUsageProposedUnitPerParent As Double = 0
            Dim dMaterialUsageProposedCostTotal As Double = 0

            Dim iMaterialUsageProgramVolume As Integer = 0
            Dim iMaterialUsageProgramVolumeBudget As Integer = 0

            Dim dMaterialUsageCurrentMethod As Double = 0
            Dim dMaterialUsageCurrentMethodBudget As Double = 0
            Dim dMaterialUsageProposedMethod As Double = 0
            Dim dMaterialUsageSavings As Double = 0
            Dim dMaterialUsageSavingsBudget As Double = 0

            Dim dMaterialUsageCECapital As Double = 0
            Dim dMaterialUsageCEMaterial As Double = 0
            Dim dMaterialUsageCEOutsideSupport As Double = 0
            Dim dMaterialUsageCEMisc As Double = 0
            Dim dMaterialUsageCEInHouseSupport As Double = 0
            Dim dMaterialUsageCETotal As Double = 0
            Dim dMaterialUsagePayback As Double = 0

            Dim dCycleTimeCurrentPiecesPerHour As Double = 0
            Dim dCycleTimeCurrentPiecesPerHourBudget As Double = 0
            Dim dCycleTimeCurrentCrewSize As Double = 0
            Dim dCycleTimeCurrentCrewSizeBudget As Double = 0
            Dim dCycleTimeCurrentMachineHourPerPieces As Double = 0
            Dim dCycleTimeCurrentMachineHourPerPiecesBudget As Double = 0
            Dim dCycleTimeCurrentManHourPerPieces As Double = 0
            Dim dCycleTimeCurrentManHourPerPiecesBudget As Double = 0
            Dim dCycleTimeCurrentMethod As Double = 0
            Dim dCycleTimeCurrentMethodBudget As Double = 0

            Dim dCycleTimeProposedPiecesPerHour As Double = 0
            Dim dCycleTimeProposedCrewSize As Double = 0
            Dim dCycleTimeProposedMachineHourPerPieces As Double = 0
            Dim dCycleTimeProposedManHourPerPieces As Double = 0
            Dim dCycleTimeProposedMethod As Double = 0

            Dim dCycleTimeMethodDifference As Double = 0
            Dim dCycleTimeMethodDifferenceBudget As Double = 0

            Dim dCycleTimeFUTARate As Double = 0
            Dim dCycleTimeSUTARate As Double = 0
            Dim dCycleTimeFICARate As Double = 0

            Dim dCycleTimeWages As Double = 0
            Dim dCycleTimeWagesPlusFringes As Double = 0
            Dim dCycleTimeVariableFringes As Double = 0

            Dim iCycleTimeCurrentVolume As Integer = 0
            Dim iCycleTimeCurrentVolumeBudget As Integer = 0
            Dim iCycleTimeProposedVolume As Integer = 0

            Dim dCycleTimeSavings As Double = 0
            Dim dCycleTimeSavingsBudget As Double = 0

            Dim dCycleTimeCECapital As Double = 0
            Dim dCycleTimeCEMaterial As Double = 0
            Dim dCycleTimeCEOutsideSupport As Double = 0
            Dim dCycleTimeCEMisc As Double = 0
            Dim dCycleTimeCEInHouseSupport As Double = 0
            Dim dCycleTimeCETotal As Double = 0
            Dim dCycleTimePayback As Double = 0

            Dim dHeadCountWages As Double = 0
            Dim dHeadCountWagesBudget As Double = 0
            Dim dHeadCountAnnualLaborCost As Double = 0
            Dim dHeadCountAnnualLaborCostBudget As Double = 0

            Dim dHeadCountFUTA As Double = 0
            Dim dHeadCountSUTA As Double = 0
            Dim dHeadCountFICA As Double = 0
            Dim dHeadCountPension As Double = 0
            Dim dHeadCountBonus As Double = 0
            Dim dHeadCountLife As Double = 0
            Dim dHeadCountGroupInsurance As Double = 0
            Dim dHeadCountWorkersComp As Double = 0
            Dim dHeadCountPensionQuarterly As Double = 0
            Dim dHeadCountTotalFringes As Double = 0

            Dim dHeadCountCurrentLaborCount As Double = 0
            Dim dHeadCountCurrentLaborCountBudget As Double = 0
            Dim dHeadCountCurrentLaborCost As Double = 0
            Dim dHeadCountCurrentLaborCostBudget As Double = 0
            Dim dHeadCountCurrentLaborFringes As Double = 0
            Dim dHeadCountCurrentLaborTotal As Double = 0
            Dim dHeadCountCurrentLaborTotalBudget As Double = 0

            Dim dHeadCountProposedLaborCount As Double = 0
            Dim dHeadCountProposedLaborCost As Double = 0
            Dim dHeadCountProposedLaborFringes As Double = 0
            Dim dHeadCountProposedLaborTotal As Double = 0

            Dim dHeadCountSavings As Double = 0
            Dim dHeadCountSavingsBudget As Double = 0

            Dim dHeadCountCECapital As Double = 0
            Dim dHeadCountCEMaterial As Double = 0
            Dim dHeadCountCEOutsideSupport As Double = 0
            Dim dHeadCountCEMisc As Double = 0
            Dim dHeadCountCEInHouseSupport As Double = 0
            Dim dHeadCountCETotal As Double = 0
            Dim dHeadCountPayback As Double = 0

            Dim dOverheadCurrentTotalCost As Double = 0
            Dim dOverheadCurrentTotalCostBudget As Double = 0

            Dim dOverheadProposedTotalCost As Double = 0
            Dim dOverheadSavings As Double = 0
            Dim dOverheadSavingsBudget As Double = 0

            Dim dOverheadCECapital As Double = 0
            Dim dOverheadCEMaterial As Double = 0
            Dim dOverheadCEOutsideSupport As Double = 0
            Dim dOverheadCEMisc As Double = 0
            Dim dOverheadCEInHouseSupport As Double = 0
            Dim dOverheadCETotal As Double = 0
            Dim dOverheadCEWriteOff As Double = 0
            Dim dOverheadPayback As Double = 0

            Dim dTotalMaterialPriceAndUsage As Double = 0
            Dim dTotalMaterialPriceAndUsageBudget As Double = 0
            Dim dTotalSavings As Double = 0
            Dim dTotalSavingsBudget As Double = 0

            Dim dTotalCECapital As Double = 0
            Dim dTotalCEMaterial As Double = 0
            Dim dTotalCEOutsideSupport As Double = 0
            Dim dTotalCEMisc As Double = 0
            Dim dTotalCEInHouseSupport As Double = 0
            Dim dTotalCEWriteOff As Double = 0
            Dim dTotalCE As Double = 0
            Dim dTotalPayback As Double = 0

            '***********************************************************************
            'material price
            '***********************************************************************

            'current
            If txtMaterialPriceCurrentPrice.Text.Trim <> "" Then
                dMaterialPriceCurrentPrice = CType(txtMaterialPriceCurrentPrice.Text.Trim, Double)
            End If

            If txtMaterialPriceCurrentPriceBudget.Text.Trim <> "" Then
                dMaterialPriceCurrentPriceBudget = CType(txtMaterialPriceCurrentPriceBudget.Text.Trim, Double)
            End If

            If txtMaterialPriceCurrentVolume.Text.Trim <> "" Then
                iMaterialPriceCurrentVolume = CType(txtMaterialPriceCurrentVolume.Text.Trim, Integer)
            End If

            If txtMaterialPriceCurrentVolumeBudget.Text.Trim <> "" Then
                iMaterialPriceCurrentVolumeBudget = CType(txtMaterialPriceCurrentVolumeBudget.Text.Trim, Integer)
            End If

            dMaterialPriceCurrentPriceByVolume = dMaterialPriceCurrentPrice * iMaterialPriceCurrentVolume
            dMaterialPriceCurrentPriceByVolumeBudget = dMaterialPriceCurrentPriceBudget * iMaterialPriceCurrentVolumeBudget

            lblMaterialPriceCurrentPriceByVolume.Text = "0.00000"
            lblMaterialPriceCurrentMethod.Text = "0.00"
            If dMaterialPriceCurrentPriceByVolume <> 0 Then
                lblMaterialPriceCurrentPriceByVolume.Text = Format(dMaterialPriceCurrentPriceByVolume, "##0.00000")
                lblMaterialPriceCurrentMethod.Text = Format(dMaterialPriceCurrentPriceByVolume, "##0.00")
            End If

            lblMaterialPriceCurrentPriceByVolumeBudget.Text = "0.00000"
            lblMaterialPriceCurrentMethodBudget.Text = "0.00"
            If dMaterialPriceCurrentPriceByVolumeBudget <> 0 Then
                lblMaterialPriceCurrentPriceByVolumeBudget.Text = Format(dMaterialPriceCurrentPriceByVolumeBudget, "##0.00000")
                lblMaterialPriceCurrentMethodBudget.Text = Format(dMaterialPriceCurrentPriceByVolumeBudget, "##0.00")
            End If

            If txtMaterialPriceCurrentFreight.Text.Trim <> "" Then
                dMaterialPriceCurrentFreight = CType(txtMaterialPriceCurrentFreight.Text.Trim, Double)
            End If

            If txtMaterialPriceCurrentFreightBudget.Text.Trim <> "" Then
                dMaterialPriceCurrentFreightBudget = CType(txtMaterialPriceCurrentFreightBudget.Text.Trim, Double)
            End If

            dMaterialPriceCurrentFreightByVolume = dMaterialPriceCurrentFreight * iMaterialPriceCurrentVolume
            dMaterialPriceCurrentFreightByVolumeBudget = dMaterialPriceCurrentFreightBudget * iMaterialPriceCurrentVolumeBudget

            lblMaterialPriceCurrentFreightByVolume.Text = "0.00000"
            If dMaterialPriceCurrentFreightByVolume <> 0 Then
                lblMaterialPriceCurrentFreightByVolume.Text = Format(dMaterialPriceCurrentFreightByVolume, "##0.00000")
            End If

            lblMaterialPriceCurrentFreightByVolumeBudget.Text = "0.00000"
            If dMaterialPriceCurrentFreightByVolumeBudget <> 0 Then
                lblMaterialPriceCurrentFreightByVolumeBudget.Text = Format(dMaterialPriceCurrentFreightByVolumeBudget, "##0.00000")
            End If

            dMaterialPriceCurrentMaterialLanded = dMaterialPriceCurrentPrice + dMaterialPriceCurrentFreight
            dMaterialPriceCurrentMaterialLandedBudget = dMaterialPriceCurrentPriceBudget + dMaterialPriceCurrentFreightBudget

            lblMaterialPriceCurrentMaterialLanded.Text = "0.00000"
            If dMaterialPriceCurrentMaterialLanded <> 0 Then
                lblMaterialPriceCurrentMaterialLanded.Text = Format(dMaterialPriceCurrentMaterialLanded, "##0.00000")
            End If

            lblMaterialPriceCurrentMaterialLandedBudget.Text = "0.00000"
            If dMaterialPriceCurrentMaterialLandedBudget <> 0 Then
                lblMaterialPriceCurrentMaterialLandedBudget.Text = Format(dMaterialPriceCurrentMaterialLandedBudget, "##0.00000")
            End If

            dMaterialPriceCurrentMaterialLandedTotal = dMaterialPriceCurrentMaterialLanded * iMaterialPriceCurrentVolume
            dMaterialPriceCurrentMaterialLandedTotalBudget = dMaterialPriceCurrentMaterialLandedBudget * iMaterialPriceCurrentVolumeBudget

            lblMaterialPriceCurrentMaterialLandedTotal.Text = "0.00000"
            If dMaterialPriceCurrentMaterialLandedTotal <> 0 Then
                lblMaterialPriceCurrentMaterialLandedTotal.Text = Format(dMaterialPriceCurrentMaterialLandedTotal, "##0.00000")
            End If

            lblMaterialPriceCurrentMaterialLandedTotalBudget.Text = "0.00000"
            If dMaterialPriceCurrentMaterialLandedTotalBudget <> 0 Then
                lblMaterialPriceCurrentMaterialLandedTotalBudget.Text = Format(dMaterialPriceCurrentMaterialLandedTotalBudget, "##0.00000")
            End If

            'proposed
            If txtMaterialPriceProposedPrice.Text.Trim <> "" Then
                dMaterialPriceProposedPrice = CType(txtMaterialPriceProposedPrice.Text.Trim, Double)
            End If

            If txtMaterialPriceProposedVolume.Text.Trim <> "" Then
                iMaterialPriceProposedVolume = CType(txtMaterialPriceProposedVolume.Text.Trim, Double)
            End If

            dMaterialPriceProposedPriceByVolume = dMaterialPriceProposedPrice * iMaterialPriceProposedVolume

            lblMaterialPriceProposedPriceByVolume.Text = "0.00000"
            lblMaterialPriceProposedMethod.Text = "0.00"
            If dMaterialPriceProposedPriceByVolume <> 0 Then
                lblMaterialPriceProposedPriceByVolume.Text = Format(dMaterialPriceProposedPriceByVolume, "##0.00000")
                lblMaterialPriceProposedMethod.Text = Format(dMaterialPriceProposedPriceByVolume, "##.00")
            End If

            If txtMaterialPriceProposedFreight.Text.Trim <> "" Then
                dMaterialPriceProposedFreight = CType(txtMaterialPriceProposedFreight.Text.Trim, Double)
            End If

            dMaterialPriceProposedFreightByVolume = dMaterialPriceProposedFreight * iMaterialPriceProposedVolume

            lblMaterialPriceProposedFreightByVolume.Text = "0.00000"
            If dMaterialPriceProposedFreightByVolume <> 0 Then
                lblMaterialPriceProposedFreightByVolume.Text = Format(dMaterialPriceProposedFreightByVolume, "##0.00000")
            End If

            dMaterialPriceProposedMaterialLanded = dMaterialPriceProposedPrice + dMaterialPriceProposedFreight

            lblMaterialPriceProposedMaterialLanded.Text = "0.00000"
            If dMaterialPriceProposedMaterialLanded <> 0 Then
                lblMaterialPriceProposedMaterialLanded.Text = Format(dMaterialPriceProposedMaterialLanded, "##0.00000")
            End If

            dMaterialPriceProposedMaterialLandedTotal = dMaterialPriceProposedMaterialLanded * iMaterialPriceProposedVolume

            lblMaterialPriceProposedMaterialLandedTotal.Text = "0.00000"
            If dMaterialPriceProposedMaterialLandedTotal <> 0 Then
                lblMaterialPriceProposedMaterialLandedTotal.Text = Format(dMaterialPriceProposedMaterialLandedTotal, "##0.00000")
            End If

            dMaterialPriceSavings = dMaterialPriceCurrentPriceByVolume - dMaterialPriceProposedPriceByVolume
            dMaterialPriceSavingsBudget = dMaterialPriceCurrentPriceByVolumeBudget - dMaterialPriceProposedPriceByVolume

            lblMaterialPriceSavings.Text = "0.00"
            If dMaterialPriceSavings <> 0 Then
                lblMaterialPriceSavings.Text = Format(dMaterialPriceSavings, "##0.00")
            Else
                lblMaterialPriceSavings.Text = "0.00"
                dMaterialPriceSavings = 0
            End If

            lblMaterialPriceSavingsBudget.Text = "0.00"
            If dMaterialPriceSavingsBudget <> 0 Then
                lblMaterialPriceSavingsBudget.Text = Format(dMaterialPriceSavingsBudget, "##0.00")
            Else
                lblMaterialPriceSavingsBudget.Text = "0.00"
                dMaterialPriceSavingsBudget = 0
            End If

            If txtMaterialPriceCECapital.Text.Trim <> "" Then
                dMaterialPriceCECapital = CType(txtMaterialPriceCECapital.Text.Trim, Double)
            End If

            If txtMaterialPriceCEMaterial.Text.Trim <> "" Then
                dMaterialPriceCEMaterial = CType(txtMaterialPriceCEMaterial.Text.Trim, Double)
            End If

            If txtMaterialPriceCEOutsideSupport.Text.Trim <> "" Then
                dMaterialPriceCEOutsideSupport = CType(txtMaterialPriceCEOutsideSupport.Text.Trim, Double)
            End If

            If txtMaterialPriceCEMisc.Text.Trim <> "" Then
                dMaterialPriceCEMisc = CType(txtMaterialPriceCEMisc.Text.Trim, Double)
            End If

            If txtMaterialPriceCEInHouseSupport.Text.Trim <> "" Then
                dMaterialPriceCEInHouseSupport = CType(txtMaterialPriceCEInHouseSupport.Text.Trim, Double)
            End If

            dMaterialPriceCETotal = dMaterialPriceCECapital + dMaterialPriceCEMaterial + dMaterialPriceCEOutsideSupport + dMaterialPriceCEMisc + dMaterialPriceCEInHouseSupport
            lblMaterialPriceCETotal.Text = Format(dMaterialPriceCETotal, "##0.00")

            lblMaterialPriceSavingsANDCE.Text = Format(dMaterialPriceCETotal, "##0.00") & " / " & Format(dMaterialPriceSavings, "##0.00") & " = "

            If dMaterialPriceSavings > 0 Then
                dMaterialPricePayback = dMaterialPriceCETotal / dMaterialPriceSavings
            Else
                dMaterialPricePayback = 0
            End If

            lblMaterialPricePayback.Text = Format((dMaterialPricePayback), "##0.00")

            '***********************************************************************
            'material usage
            '***********************************************************************

            'current
            If txtMaterialUsageCurrentCostPerUnit.Text.Trim <> "" Then
                dMaterialUsageCurrentCostPerUnit = CType(txtMaterialUsageCurrentCostPerUnit.Text.Trim, Double)
            End If

            If txtMaterialUsageCurrentCostPerUnitBudget.Text.Trim <> "" Then
                dMaterialUsageCurrentCostPerUnitBudget = CType(txtMaterialUsageCurrentCostPerUnitBudget.Text.Trim, Double)
            End If

            If txtMaterialUsageCurrentUnitPerParent.Text.Trim <> "" Then
                dMaterialUsageCurrentUnitPerParent = CType(txtMaterialUsageCurrentUnitPerParent.Text.Trim, Double)
            End If

            If txtMaterialUsageCurrentUnitPerParentBudget.Text.Trim <> "" Then
                dMaterialUsageCurrentUnitPerParentBudget = CType(txtMaterialUsageCurrentUnitPerParentBudget.Text.Trim, Double)
            End If

            dMaterialUsageCurrentCostTotal = dMaterialUsageCurrentCostPerUnit * dMaterialUsageCurrentUnitPerParent
            dMaterialUsageCurrentCostTotalBudget = dMaterialUsageCurrentCostPerUnitBudget * dMaterialUsageCurrentUnitPerParentBudget

            lblMaterialUsageCurrentCostTotal.Text = "0.00000"
            If dMaterialUsageCurrentCostTotal <> 0 Then
                lblMaterialUsageCurrentCostTotal.Text = Format(dMaterialUsageCurrentCostTotal, "##0.00000")
            End If

            lblMaterialUsageCurrentCostTotalBudget.Text = "0.00000"
            If dMaterialUsageCurrentCostTotalBudget <> 0 Then
                lblMaterialUsageCurrentCostTotalBudget.Text = Format(dMaterialUsageCurrentCostTotalBudget, "##0.00000")
            End If

            'proposed
            If txtMaterialUsageProposedCostPerUnit.Text.Trim <> "" Then
                dMaterialUsageProposedCostPerUnit = CType(txtMaterialUsageProposedCostPerUnit.Text.Trim, Double)
            End If

            If txtMaterialUsageProposedUnitPerParent.Text.Trim <> "" Then
                dMaterialUsageProposedUnitPerParent = CType(txtMaterialUsageProposedUnitPerParent.Text.Trim, Double)
            End If

            dMaterialUsageProposedCostTotal = dMaterialUsageProposedCostPerUnit * dMaterialUsageProposedUnitPerParent

            lblMaterialUsageProposedCostTotal.Text = "0.00000"
            If dMaterialUsageProposedCostTotal <> 0 Then
                lblMaterialUsageProposedCostTotal.Text = Format(dMaterialUsageProposedCostTotal, "##0.00000")
            End If

            If txtMaterialUsageProgramVolume.Text.Trim <> "" Then
                iMaterialUsageProgramVolume = CType(txtMaterialUsageProgramVolume.Text.Trim, Integer)
            End If

            If txtMaterialUsageProgramVolumeBudget.Text.Trim <> "" Then
                iMaterialUsageProgramVolumeBudget = CType(txtMaterialUsageProgramVolumeBudget.Text.Trim, Integer)
            End If

            dMaterialUsageCurrentMethod = dMaterialUsageCurrentCostTotal * iMaterialUsageProgramVolume
            dMaterialUsageCurrentMethodBudget = dMaterialUsageCurrentCostTotalBudget * iMaterialUsageProgramVolumeBudget

            lblMaterialUsageCurrentMethod.Text = "0.00"
            If dMaterialUsageCurrentMethod <> 0 Then
                lblMaterialUsageCurrentMethod.Text = Format(dMaterialUsageCurrentMethod, "##0.00")
            End If

            lblMaterialUsageCurrentMethodBudget.Text = "0.00"
            If dMaterialUsageCurrentMethodBudget <> 0 Then
                lblMaterialUsageCurrentMethodBudget.Text = Format(dMaterialUsageCurrentMethodBudget, "##0.00")
            End If

            dMaterialUsageProposedMethod = dMaterialUsageProposedCostTotal * iMaterialUsageProgramVolume

            lblMaterialUsageProposedMethod.Text = "0.00"
            If dMaterialUsageProposedMethod <> 0 Then
                lblMaterialUsageProposedMethod.Text = Format(dMaterialUsageProposedMethod, "##0.00")
            End If

            dMaterialUsageSavings = dMaterialUsageCurrentMethod - dMaterialUsageProposedMethod
            dMaterialUsageSavingsBudget = dMaterialUsageCurrentMethodBudget - dMaterialUsageProposedMethod

            lblMaterialUsageSavings.Text = "0.00"
            If dMaterialUsageSavings <> 0 Then
                lblMaterialUsageSavings.Text = Format(dMaterialUsageSavings, "##0.00")
            End If

            lblMaterialUsageSavingsBudget.Text = "0.00"
            If dMaterialUsageSavingsBudget <> 0 Then
                lblMaterialUsageSavingsBudget.Text = Format(dMaterialUsageSavingsBudget, "##0.00")
            End If

            If txtMaterialUsageCECapital.Text.Trim <> "" Then
                dMaterialUsageCECapital = CType(txtMaterialUsageCECapital.Text.Trim, Double)
            End If

            If txtMaterialUsageCEMaterial.Text.Trim <> "" Then
                dMaterialUsageCEMaterial = CType(txtMaterialUsageCEMaterial.Text.Trim, Double)
            End If

            If txtMaterialUsageCEOutsideSupport.Text.Trim <> "" Then
                dMaterialUsageCEOutsideSupport = CType(txtMaterialUsageCEOutsideSupport.Text.Trim, Double)
            End If

            If txtMaterialUsageCEMisc.Text.Trim <> "" Then
                dMaterialUsageCEMisc = CType(txtMaterialUsageCEMisc.Text.Trim, Double)
            End If

            If txtMaterialUsageCEInHouseSupport.Text.Trim <> "" Then
                dMaterialUsageCEInHouseSupport = CType(txtMaterialUsageCEInHouseSupport.Text.Trim, Double)
            End If

            dMaterialUsageCETotal = dMaterialUsageCECapital + dMaterialUsageCEMaterial + dMaterialUsageCEOutsideSupport + dMaterialUsageCEMisc + dMaterialUsageCEInHouseSupport
            lblMaterialUsageCETotal.Text = Format(dMaterialUsageCETotal, "##0.00")

            lblMaterialUsageSavingsANDCE.Text = Format(dMaterialUsageCETotal, "##0.00") & " / " & Format(dMaterialUsageSavings, "##0.00") & " = "

            If dMaterialUsageSavings <> 0 Then
                dMaterialUsagePayback = dMaterialUsageCETotal / dMaterialUsageSavings
            End If

            lblMaterialUsagePayback.Text = Format((dMaterialUsagePayback), "##0.00")

            '***********************************************************************
            'cycle time
            '***********************************************************************

            'current
            If txtCycleTimeCurrentPiecesPerHour.Text.Trim <> "" Then
                dCycleTimeCurrentPiecesPerHour = CType(txtCycleTimeCurrentPiecesPerHour.Text.Trim, Double)
            End If

            If txtCycleTimeCurrentPiecesPerHourBudget.Text.Trim <> "" Then
                dCycleTimeCurrentPiecesPerHourBudget = CType(txtCycleTimeCurrentPiecesPerHourBudget.Text.Trim, Double)
            End If

            If txtCycleTimeCurrentCrewSize.Text.Trim <> "" Then
                dCycleTimeCurrentCrewSize = CType(txtCycleTimeCurrentCrewSize.Text.Trim, Double)
            End If

            If txtCycleTimeCurrentCrewSizeBudget.Text.Trim <> "" Then
                dCycleTimeCurrentCrewSizeBudget = CType(txtCycleTimeCurrentCrewSizeBudget.Text.Trim, Double)
            End If

            If dCycleTimeCurrentPiecesPerHour <> 0 Then
                dCycleTimeCurrentMachineHourPerPieces = (1 / dCycleTimeCurrentPiecesPerHour)
            End If

            If dCycleTimeCurrentPiecesPerHourBudget <> 0 Then
                dCycleTimeCurrentMachineHourPerPiecesBudget = (1 / dCycleTimeCurrentPiecesPerHourBudget)
            End If

            lblCycleTimeCurrentMachineHourPerPieces.Text = "0.0000"
            If dCycleTimeCurrentMachineHourPerPieces <> 0 Then
                lblCycleTimeCurrentMachineHourPerPieces.Text = Format(dCycleTimeCurrentMachineHourPerPieces, "##0.0000")
            End If

            lblCycleTimeCurrentMachineHourPerPiecesBudget.Text = "0.0000"
            If dCycleTimeCurrentMachineHourPerPiecesBudget <> 0 Then
                lblCycleTimeCurrentMachineHourPerPiecesBudget.Text = Format(dCycleTimeCurrentMachineHourPerPiecesBudget, "##0.0000")
            End If

            dCycleTimeCurrentManHourPerPieces = dCycleTimeCurrentCrewSize * dCycleTimeCurrentMachineHourPerPieces
            dCycleTimeCurrentManHourPerPiecesBudget = dCycleTimeCurrentCrewSizeBudget * dCycleTimeCurrentMachineHourPerPiecesBudget

            lblCycleTimeCurrentManHourPerPieces.Text = "0.0000"
            If dCycleTimeCurrentManHourPerPieces <> 0 Then
                lblCycleTimeCurrentManHourPerPieces.Text = Format(dCycleTimeCurrentManHourPerPieces, "##0.0000")
            End If

            lblCycleTimeCurrentManHourPerPiecesBudget.Text = "0.0000"
            If dCycleTimeCurrentManHourPerPiecesBudget <> 0 Then
                lblCycleTimeCurrentManHourPerPiecesBudget.Text = Format(dCycleTimeCurrentManHourPerPiecesBudget, "##0.0000")
            End If

            If txtCycleTimeCurrentVolume.Text.Trim <> "" Then
                iCycleTimeCurrentVolume = CType(txtCycleTimeCurrentVolume.Text.Trim, Integer)
            End If

            If txtCycleTimeCurrentVolumeBudget.Text.Trim <> "" Then
                iCycleTimeCurrentVolumeBudget = CType(txtCycleTimeCurrentVolumeBudget.Text.Trim, Integer)
            End If

            dCycleTimeCurrentMethod = dCycleTimeCurrentManHourPerPieces * iCycleTimeCurrentVolume
            dCycleTimeCurrentMethodBudget = dCycleTimeCurrentManHourPerPiecesBudget * iCycleTimeCurrentVolumeBudget

            lblCycleTimeCurrentTotalManHours.Text = "0"
            lblCycleTimeCurrentMethod.Text = "0"
            If dCycleTimeCurrentMethod <> 0 Then
                lblCycleTimeCurrentTotalManHours.Text = Format(dCycleTimeCurrentMethod, "##")
                lblCycleTimeCurrentMethod.Text = Format(dCycleTimeCurrentMethod, "##")
            End If

            lblCycleTimeCurrentTotalManHoursBudget.Text = "0"
            lblCycleTimeCurrentMethodBudget.Text = "0"
            If dCycleTimeCurrentMethodBudget <> 0 Then
                lblCycleTimeCurrentTotalManHoursBudget.Text = Format(dCycleTimeCurrentMethodBudget, "##")
                lblCycleTimeCurrentMethodBudget.Text = Format(dCycleTimeCurrentMethodBudget, "##")
            End If

            'proposed

            If txtCycleTimeProposedPiecesPerHour.Text.Trim <> "" Then
                dCycleTimeProposedPiecesPerHour = CType(txtCycleTimeProposedPiecesPerHour.Text.Trim, Double)
            End If

            If txtCycleTimeProposedCrewSize.Text.Trim <> "" Then
                dCycleTimeProposedCrewSize = CType(txtCycleTimeProposedCrewSize.Text.Trim, Double)
            End If

            If dCycleTimeProposedPiecesPerHour <> 0 Then
                dCycleTimeProposedMachineHourPerPieces = (1 / dCycleTimeProposedPiecesPerHour)
            End If

            lblCycleTimeProposedMachineHourPerPieces.Text = "0.0000"
            If dCycleTimeCurrentMachineHourPerPieces <> 0 Then
                lblCycleTimeProposedMachineHourPerPieces.Text = Format(dCycleTimeProposedMachineHourPerPieces, "##0.0000")
            End If

            dCycleTimeProposedManHourPerPieces = dCycleTimeProposedCrewSize * dCycleTimeProposedMachineHourPerPieces

            lblCycleTimeProposedManHourPerPieces.Text = "0.0000"
            If dCycleTimeProposedManHourPerPieces <> 0 Then
                lblCycleTimeProposedManHourPerPieces.Text = Format(dCycleTimeProposedManHourPerPieces, "##0.0000")
            End If

            If txtCycleTimeProposedVolume.Text.Trim <> "" Then
                iCycleTimeProposedVolume = CType(txtCycleTimeProposedVolume.Text.Trim, Integer)
            End If

            dCycleTimeProposedMethod = dCycleTimeProposedManHourPerPieces * iCycleTimeProposedVolume

            lblCycleTimeProposedTotalManHours.Text = "0"
            lblCycleTimeProposedMethod.Text = "0"
            If dCycleTimeProposedMethod <> 0 Then
                lblCycleTimeProposedTotalManHours.Text = Format(dCycleTimeProposedMethod, "##0")
                lblCycleTimeProposedMethod.Text = Format(dCycleTimeProposedMethod, "##0")
            End If

            dCycleTimeMethodDifference = dCycleTimeCurrentMethod - dCycleTimeProposedMethod
            dCycleTimeMethodDifferenceBudget = dCycleTimeCurrentMethodBudget - dCycleTimeProposedMethod

            lblCycleTimeMethodDifference.Text = Format(dCycleTimeMethodDifference, "##0")
            lblCycleTimeMethodDifferenceBudget.Text = Format(dCycleTimeMethodDifferenceBudget, "##0")

            If txtCycleTimeFUTARate.Text.Trim <> "" Then
                dCycleTimeFUTARate = CType(txtCycleTimeFUTARate.Text.Trim, Double)
                lblCycleTimeFUTARateDecimal.Text = "(= " & Format((dCycleTimeFUTARate / 100), "##0.0000") & ")"
            End If

            If txtCycleTimeSUTARate.Text.Trim <> "" Then
                dCycleTimeSUTARate = CType(txtCycleTimeSUTARate.Text.Trim, Double)
                lblCycleTimeSUTARateDecimal.Text = "(=" & Format((dCycleTimeSUTARate / 100), "##0.0000") & ")"
            End If

            If txtCycleTimeFICARate.Text.Trim <> "" Then
                dCycleTimeFICARate = CType(txtCycleTimeFICARate.Text.Trim, Double)
                lblCycleTimeFICARateDecimal.Text = "(=" & Format((dCycleTimeFICARate / 100), "##0.0000") & ")"
            End If

            dCycleTimeVariableFringes = dCycleTimeFUTARate + dCycleTimeSUTARate + dCycleTimeFICARate
            lblCycleTimeVariableFringes.Text = Format(dCycleTimeVariableFringes, "##0.00")
            lblCycleTimeVariableFringesDecimal.Text = "(=" & Format((dCycleTimeVariableFringes / 100), "##0.0000") & ")"

            If txtCycleTimeWages.Text.Trim <> "" Then
                dCycleTimeWages = CType(txtCycleTimeWages.Text.Trim, Double)
            End If

            dCycleTimeWagesPlusFringes = ((dCycleTimeWages * (dCycleTimeVariableFringes / 100)) + dCycleTimeWages)

            lblCycleTimeWagesPlusFringes.Text = "0.00"
            If dCycleTimeWagesPlusFringes <> 0 Then
                lblCycleTimeWagesPlusFringes.Text = Format(dCycleTimeWagesPlusFringes, "##0.00")
            End If

            lblCycleTimeSavings.Text = "0.00"
            If dCycleTimeMethodDifference <> 0 Then
                dCycleTimeSavings = dCycleTimeMethodDifference * dCycleTimeWagesPlusFringes
                lblCycleTimeSavings.Text = Format(dCycleTimeSavings, "##0.00")
            End If

            lblCycleTimeSavingsBudget.Text = "0.00"
            If dCycleTimeMethodDifferenceBudget <> 0 Then
                dCycleTimeSavingsBudget = dCycleTimeMethodDifferenceBudget * dCycleTimeWagesPlusFringes
                lblCycleTimeSavingsBudget.Text = Format(dCycleTimeSavingsBudget, "##0.00")
            End If

            If txtCycleTimeCECapital.Text.Trim <> "" Then
                dCycleTimeCECapital = CType(txtCycleTimeCECapital.Text.Trim, Double)
            End If

            If txtCycleTimeCEMaterial.Text.Trim <> "" Then
                dCycleTimeCEMaterial = CType(txtCycleTimeCEMaterial.Text.Trim, Double)
            End If

            If txtCycleTimeCEOutsideSupport.Text.Trim <> "" Then
                dCycleTimeCEOutsideSupport = CType(txtCycleTimeCEOutsideSupport.Text.Trim, Double)
            End If

            If txtCycleTimeCEMisc.Text.Trim <> "" Then
                dCycleTimeCEMisc = CType(txtCycleTimeCEMisc.Text.Trim, Double)
            End If

            If txtCycleTimeCEInHouseSupport.Text.Trim <> "" Then
                dCycleTimeCEInHouseSupport = CType(txtCycleTimeCEInHouseSupport.Text.Trim, Double)
            End If

            dCycleTimeCETotal = dCycleTimeCECapital + dCycleTimeCEMaterial + dCycleTimeCEOutsideSupport + dCycleTimeCEMisc + dCycleTimeCEInHouseSupport
            lblCycleTimeCETotal.Text = Format(dCycleTimeCETotal, "##0.00")

            lblCycleTimeSavingsANDCE.Text = Format(dCycleTimeCETotal, "##0.00") & " / " & Format(dCycleTimeSavings, "##0.00") & " = "

            If dCycleTimeSavings <> 0 Then
                dCycleTimePayback = dCycleTimeCETotal / dCycleTimeSavings
            End If

            lblCycleTimePayback.Text = Format((dCycleTimePayback), "##0.00")

            '***********************************************************************
            'head count
            '***********************************************************************

            If txtHeadCountWages.Text.Trim <> "" Then
                dHeadCountWages = CType(txtHeadCountWages.Text.Trim, Double)
            End If

            If txtHeadCountWagesBudget.Text.Trim <> "" Then
                dHeadCountWagesBudget = CType(txtHeadCountWagesBudget.Text.Trim, Double)
            End If

            dHeadCountAnnualLaborCost = dHeadCountWages * 2080
            dHeadCountAnnualLaborCostBudget = dHeadCountWagesBudget * 2080

            lblHeadCountAnnualLaborCost.Text = Format(dHeadCountAnnualLaborCost, "##0.00")
            lblHeadCountAnnualLaborCostBudget.Text = Format(dHeadCountAnnualLaborCostBudget, "##0.00")

            If txtHeadCountFUTA.Text.Trim <> "" Then
                dHeadCountFUTA = CType(txtHeadCountFUTA.Text.Trim, Double)
            End If

            If txtHeadCountSUTA.Text.Trim <> "" Then
                dHeadCountSUTA = CType(txtHeadCountSUTA.Text.Trim, Double)
            End If

            If txtHeadCountFICA.Text.Trim <> "" Then
                dHeadCountFICA = CType(txtHeadCountFICA.Text.Trim, Double)
            End If

            If txtHeadCountPension.Text.Trim <> "" Then
                dHeadCountPension = CType(txtHeadCountPension.Text.Trim, Double)
            End If

            If txtHeadCountBonus.Text.Trim <> "" Then
                dHeadCountBonus = CType(txtHeadCountBonus.Text.Trim, Double)
            End If

            If txtHeadCountLife.Text.Trim <> "" Then
                dHeadCountLife = CType(txtHeadCountLife.Text.Trim, Double)
            End If

            If txtHeadCountGroupInsurance.Text.Trim <> "" Then
                dHeadCountGroupInsurance = CType(txtHeadCountGroupInsurance.Text.Trim, Double)
            End If

            If lblHeadCountWorkersComp.Text.Trim <> "" Then
                dHeadCountWorkersComp = CType(lblHeadCountWorkersComp.Text.Trim, Double)
            End If

            If txtHeadCountPensionQuarterly.Text.Trim <> "" Then
                dHeadCountPensionQuarterly = CType(txtHeadCountPensionQuarterly.Text.Trim, Double)
            End If

            dHeadCountTotalFringes = dHeadCountFUTA + dHeadCountSUTA + dHeadCountFICA + dHeadCountPension + dHeadCountBonus + dHeadCountLife + dHeadCountGroupInsurance + dHeadCountWorkersComp + dHeadCountPensionQuarterly

            lblHeadCountTotalFringes.Text = "0.00"
            If dHeadCountTotalFringes <> 0 Then
                lblHeadCountTotalFringes.Text = Format(dHeadCountTotalFringes, "##0.00")
            End If

            'current
            If txtHeadCountCurrentLaborCount.Text.Trim <> "" Then
                dHeadCountCurrentLaborCount = CType(txtHeadCountCurrentLaborCount.Text.Trim, Double)
            End If

            If txtHeadCountCurrentLaborCountBudget.Text.Trim <> "" Then
                dHeadCountCurrentLaborCountBudget = CType(txtHeadCountCurrentLaborCountBudget.Text.Trim, Double)
            End If

            dHeadCountCurrentLaborCost = dHeadCountCurrentLaborCount * dHeadCountAnnualLaborCost
            dHeadCountCurrentLaborCostBudget = dHeadCountCurrentLaborCountBudget * dHeadCountAnnualLaborCostBudget

            lblHeadCountCurrentLaborCost.Text = "0.00"
            If dHeadCountCurrentLaborCost <> 0 Then
                lblHeadCountCurrentLaborCost.Text = Format(dHeadCountCurrentLaborCost, "##0.00")
            End If

            lblHeadCountCurrentLaborCostBudget.Text = "0.00"
            If dHeadCountCurrentLaborCostBudget <> 0 Then
                lblHeadCountCurrentLaborCostBudget.Text = Format(dHeadCountCurrentLaborCostBudget, "##0.00")
            End If

            lblHeadCountCurrentLaborFringes.Text = "0.00"
            If dHeadCountCurrentLaborCount * dHeadCountTotalFringes <> 0 Then
                dHeadCountCurrentLaborFringes = dHeadCountCurrentLaborCount * dHeadCountTotalFringes
                lblHeadCountCurrentLaborFringes.Text = Format(dHeadCountCurrentLaborFringes, "##0.00")
            End If

            dHeadCountCurrentLaborTotal = dHeadCountCurrentLaborFringes + dHeadCountCurrentLaborCost
            dHeadCountCurrentLaborTotalBudget = dHeadCountCurrentLaborFringes + dHeadCountCurrentLaborCostBudget

            lblHeadCountCurrentLaborTotal.Text = "0.00"
            lblHeadCountCurrentMethod.Text = "0.00"
            If dHeadCountCurrentLaborTotal <> 0 Then
                lblHeadCountCurrentLaborTotal.Text = Format(dHeadCountCurrentLaborTotal, "##0.00")
                lblHeadCountCurrentMethod.Text = Format(dHeadCountCurrentLaborTotal, "##0.00")
            End If

            lblHeadCountCurrentLaborTotalBudget.Text = "0.00"
            lblHeadCountCurrentMethodBudget.Text = "0.00"
            If dHeadCountCurrentLaborTotalBudget <> 0 Then
                lblHeadCountCurrentLaborTotalBudget.Text = Format(dHeadCountCurrentLaborTotalBudget, "##0.00")
                lblHeadCountCurrentMethodBudget.Text = Format(dHeadCountCurrentLaborTotalBudget, "##0.00")
            End If

            'proposed
            If txtHeadCountProposedLaborCount.Text.Trim <> "" Then
                dHeadCountProposedLaborCount = CType(txtHeadCountProposedLaborCount.Text.Trim, Double)
            End If

            dHeadCountProposedLaborCost = dHeadCountProposedLaborCount * dHeadCountAnnualLaborCost

            lblHeadCountProposedLaborCost.Text = "0.00"
            If dHeadCountProposedLaborCost <> 0 Then
                lblHeadCountProposedLaborCost.Text = Format(dHeadCountProposedLaborCost, "##0.00")
            End If

            lblHeadCountProposedLaborFringes.Text = "0.00"
            If dHeadCountProposedLaborCount * dHeadCountTotalFringes > 0 Then
                dHeadCountProposedLaborFringes = dHeadCountProposedLaborCount * dHeadCountTotalFringes
                lblHeadCountProposedLaborFringes.Text = Format(dHeadCountProposedLaborFringes, "##0.00")
            End If

            dHeadCountProposedLaborTotal = dHeadCountProposedLaborFringes + dHeadCountProposedLaborCost

            lblHeadCountProposedLaborTotal.Text = "0.00"
            lblHeadCountProposedMethod.Text = "0.00"
            If dHeadCountProposedLaborTotal <> 0 Then
                lblHeadCountProposedLaborTotal.Text = Format(dHeadCountProposedLaborTotal, "##0.00")
                lblHeadCountProposedMethod.Text = Format(dHeadCountProposedLaborTotal, "##0.00")
            End If

            dHeadCountSavings = dHeadCountCurrentLaborTotal - dHeadCountProposedLaborTotal
            dHeadCountSavingsBudget = dHeadCountCurrentLaborTotalBudget - dHeadCountProposedLaborTotal

            lblHeadCountSavings.Text = "0.00"
            If dHeadCountSavings <> 0 Then
                lblHeadCountSavings.Text = Format(dHeadCountSavings, "##0.00")
            End If

            lblHeadCountSavingsBudget.Text = "0.00"
            If dHeadCountSavingsBudget <> 0 Then
                lblHeadCountSavingsBudget.Text = Format(dHeadCountSavingsBudget, "##0.00")
            End If

            If txtHeadCountCECapital.Text.Trim <> "" Then
                dHeadCountCECapital = CType(txtHeadCountCECapital.Text.Trim, Double)
            End If

            If txtHeadCountCEMaterial.Text.Trim <> "" Then
                dHeadCountCEMaterial = CType(txtHeadCountCEMaterial.Text.Trim, Double)
            End If

            If txtHeadCountCEOutsideSupport.Text.Trim <> "" Then
                dHeadCountCEOutsideSupport = CType(txtHeadCountCEOutsideSupport.Text.Trim, Double)
            End If

            If txtHeadCountCEMisc.Text.Trim <> "" Then
                dHeadCountCEMisc = CType(txtHeadCountCEMisc.Text.Trim, Double)
            End If

            If txtHeadCountCEInHouseSupport.Text.Trim <> "" Then
                dHeadCountCEInHouseSupport = CType(txtHeadCountCEInHouseSupport.Text.Trim, Double)
            End If

            dHeadCountCETotal = dHeadCountCECapital + dHeadCountCEMaterial + dHeadCountCEOutsideSupport + dHeadCountCEMisc + dHeadCountCEInHouseSupport
            lblHeadCountCETotal.Text = Format(dHeadCountCETotal, "##0.00")

            lblHeadCountSavingsANDCE.Text = Format(dHeadCountCETotal, "##0.00") & " / " & Format(dHeadCountSavings, "##0.00") & " = "

            If dHeadCountSavings <> 0 Then
                dHeadCountPayback = dHeadCountCETotal / dHeadCountSavings
            End If

            lblHeadCountPayback.Text = Format(dHeadCountPayback, "##0.00")

            '***********************************************************************
            'overhead
            '***********************************************************************

            lblOverheadCurrentTotalCost.Text = "0.00"
            lblOverheadCurrentMethod.Text = "0.00"

            lblOverheadCurrentTotalCostBudget.Text = "0.00"
            lblOverheadCurrentMethodBudget.Text = "0.00"

            ds = CRModule.GetCostReductionOverheadCurrentTotal(ViewState("pProjNo"))
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("CurrentCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CurrentCostTotal") <> 0 Then
                        dOverheadCurrentTotalCost = ds.Tables(0).Rows(0).Item("CurrentCostTotal")
                        lblOverheadCurrentTotalCost.Text = Format(dOverheadCurrentTotalCost, "##0.00")
                        lblOverheadCurrentMethod.Text = Format(dOverheadCurrentTotalCost, "##0.00")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("CurrentCostTotalBudget") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CurrentCostTotalBudget") <> 0 Then
                        dOverheadCurrentTotalCostBudget = ds.Tables(0).Rows(0).Item("CurrentCostTotalBudget")
                        lblOverheadCurrentTotalCostBudget.Text = Format(dOverheadCurrentTotalCostBudget, "##0.00")
                        lblOverheadCurrentMethodBudget.Text = Format(dOverheadCurrentTotalCostBudget, "##0.00")
                    End If
                End If
            End If

            lblOverheadProposedTotalCost.Text = "0.00"
            lblOverheadProposedMethod.Text = "0.00"
            ds = CRModule.GetCostReductionOverheadProposedTotal(ViewState("pProjNo"))
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("ProposedCostTotal") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ProposedCostTotal") <> 0 Then
                        dOverheadProposedTotalCost = ds.Tables(0).Rows(0).Item("ProposedCostTotal")
                        lblOverheadProposedTotalCost.Text = Format(dOverheadProposedTotalCost, "##0.00")
                        lblOverheadProposedMethod.Text = Format(dOverheadProposedTotalCost, "##0.00")
                    End If
                End If
            End If

            dOverheadSavings = dOverheadCurrentTotalCost - dOverheadProposedTotalCost
            dOverheadSavingsBudget = dOverheadCurrentTotalCostBudget - dOverheadProposedTotalCost

            lblOverheadSavings.Text = Format(dOverheadSavings, "##0.00")
            lblOverheadSavingsBudget.Text = Format(dOverheadSavingsBudget, "##0.00")

            If txtOverheadCECapital.Text.Trim <> "" Then
                dOverheadCECapital = CType(txtOverheadCECapital.Text.Trim, Double)
            End If

            If txtOverheadCEMaterial.Text.Trim <> "" Then
                dOverheadCEMaterial = CType(txtOverheadCEMaterial.Text.Trim, Double)
            End If

            If txtOverheadCEOutsideSupport.Text.Trim <> "" Then
                dOverheadCEOutsideSupport = CType(txtOverheadCEOutsideSupport.Text.Trim, Double)
            End If

            If txtOverheadCEMisc.Text.Trim <> "" Then
                dOverheadCEMisc = CType(txtOverheadCEMisc.Text.Trim, Double)
            End If

            If txtOverheadCEInHouseSupport.Text.Trim <> "" Then
                dOverheadCEInHouseSupport = CType(txtOverheadCEInHouseSupport.Text.Trim, Double)
            End If

            If txtOverheadCEWriteOff.Text.Trim <> "" Then
                dOverheadCEWriteOff = CType(txtOverheadCEWriteOff.Text.Trim, Double)
            End If

            dOverheadCETotal = dOverheadCECapital + dOverheadCEMaterial + dOverheadCEOutsideSupport + dOverheadCEMisc + dOverheadCEInHouseSupport + dOverheadCEWriteOff
            lblOverheadCETotal.Text = Format(dOverheadCETotal, "##0.00")

            lblOverheadSavingsANDCE.Text = Format(dOverheadCETotal, "##0.00") & " / " & Format(dOverheadSavings, "##0.00") & " = "

            If dOverheadSavings <> 0 Then
                dOverheadPayback = dOverheadCETotal / dOverheadSavings
            End If

            lblOverheadPayback.Text = Format(dOverheadPayback, "##0.00")

            '***********************************************************************
            'top level totals
            '***********************************************************************

            'annual sales

            dTotalMaterialPriceAndUsage = dMaterialPriceSavings + dMaterialUsageSavings
            dTotalMaterialPriceAndUsageBudget = dMaterialPriceSavingsBudget + dMaterialUsageSavingsBudget

            lblTotalSavingsMaterialPriceAndUsage.Text = "0.00"
            If dTotalMaterialPriceAndUsage <> 0 Then
                lblTotalSavingsMaterialPriceAndUsage.Text = Format(dTotalMaterialPriceAndUsage, "##0.00")
            End If

            lblTotalSavingsMaterialPriceAndUsageBudget.Text = "0.00"
            If dTotalMaterialPriceAndUsageBudget <> 0 Then
                lblTotalSavingsMaterialPriceAndUsageBudget.Text = Format(dTotalMaterialPriceAndUsageBudget, "##0.00")
            End If

            lblTotalSavingsCycleTime.Text = "0.00"
            If dCycleTimeSavings <> 0 Then
                lblTotalSavingsCycleTime.Text = Format(dCycleTimeSavings, "##0.00")
            End If

            lblTotalSavingsCycleTimeBudget.Text = "0.00"
            If dCycleTimeSavingsBudget <> 0 Then
                lblTotalSavingsCycleTimeBudget.Text = Format(dCycleTimeSavingsBudget, "##0.00")
            End If

            lblTotalSavingsHeadCount.Text = "0.00"
            If dHeadCountSavings <> 0 Then
                lblTotalSavingsHeadCount.Text = Format(dHeadCountSavings, "##0.00")
            End If

            lblTotalSavingsHeadCountBudget.Text = "0.00"
            If dHeadCountSavingsBudget <> 0 Then
                lblTotalSavingsHeadCountBudget.Text = Format(dHeadCountSavingsBudget, "##0.00")
            End If

            lblTotalSavingsOverhead.Text = "0.00"
            If dOverheadSavings <> 0 Then
                lblTotalSavingsOverhead.Text = Format(dOverheadSavings, "##0.00")
            End If

            lblTotalSavingsOverheadBudget.Text = "0.00"
            If dOverheadSavingsBudget <> 0 Then
                lblTotalSavingsOverheadBudget.Text = Format(dOverheadSavingsBudget, "##0.00")
            End If

            dTotalSavings = dTotalMaterialPriceAndUsage + dCycleTimeSavings + dHeadCountSavings + dOverheadSavings
            dTotalSavingsBudget = dTotalMaterialPriceAndUsageBudget + dCycleTimeSavingsBudget + dHeadCountSavingsBudget + dOverheadSavingsBudget

            lblTotalSavings.Text = "0.00"
            If dTotalSavings <> 0 Then
                lblTotalSavings.Text = Format(dTotalSavings, "##0.00")
            End If

            lblTotalSavingsBudget.Text = "0.00"
            If dTotalSavingsBudget <> 0 Then
                lblTotalSavingsBudget.Text = Format(dTotalSavingsBudget, "##0.00")
            End If

            'CE

            dTotalCECapital = dMaterialPriceCECapital + dMaterialUsageCECapital + dCycleTimeCECapital + dHeadCountCECapital + dOverheadCECapital

            lblTotalCECapital.Text = "0.00"
            If dTotalCECapital <> 0 Then
                lblTotalCECapital.Text = Format(dTotalCECapital, "##0.00")
            End If

            dTotalCEMaterial = dMaterialPriceCEMaterial + dMaterialUsageCEMaterial + dCycleTimeCEMaterial + dHeadCountCEMaterial + dOverheadCEMaterial

            lblTotalCEMaterial.Text = "0.00"
            If dTotalCEMaterial <> 0 Then
                lblTotalCEMaterial.Text = Format(dTotalCEMaterial, "##0.00")
            End If

            dTotalCEOutsideSupport = dMaterialPriceCEOutsideSupport + dMaterialUsageCEOutsideSupport + dCycleTimeCEOutsideSupport + dHeadCountCEOutsideSupport + dOverheadCEOutsideSupport

            lblTotalCEOutsideSupport.Text = "0.00"
            If dTotalCEOutsideSupport <> 0 Then
                lblTotalCEOutsideSupport.Text = Format(dTotalCEOutsideSupport, "##0.00")
            End If

            dTotalCEMisc = dMaterialPriceCEMisc + dMaterialUsageCEMisc + dCycleTimeCEMisc + dHeadCountCEMisc + dOverheadCEMisc

            If dTotalCEMisc > 0 Then
                lblTotalCEMisc.Text = Format(dTotalCEMisc, "##.00")
            End If

            dTotalCEInHouseSupport = dMaterialPriceCEInHouseSupport + dMaterialUsageCEInHouseSupport + dCycleTimeCEInHouseSupport + dHeadCountCEInHouseSupport + dOverheadCEInHouseSupport

            lblTotalCEInHouseSupport.Text = "0.00"
            If dTotalCEInHouseSupport <> 0 Then
                lblTotalCEInHouseSupport.Text = Format(dTotalCEInHouseSupport, "##0.00")
            End If

            dTotalCEWriteOff = dOverheadCEWriteOff

            lblTotalCEWriteOff.Text = "0.00"
            If dTotalCEWriteOff <> 0 Then
                lblTotalCEWriteOff.Text = Format(dTotalCEWriteOff, "##0.00")
            End If

            dTotalCE = dTotalCECapital + dTotalCEMaterial + dTotalCEOutsideSupport + dTotalCEMisc + dTotalCEInHouseSupport + dTotalCEWriteOff

            lblTotalCE.Text = "0.00"
            If dTotalCE <> 0 Then
                lblTotalCE.Text = Format(dTotalCE, "##0.00")
            End If

            lblTotalAnnualSavingsANDCE.Text = Format(dTotalCE, "##0.00") & " / " & Format(dTotalSavings, "##0.00") & " = "

            If dTotalSavings <> 0 Then
                dTotalPayback = dTotalCE / dTotalSavings
            End If

            lblTotalPayback.Text = Format(dTotalPayback, "##0.00")

            If rbCustomerGiveBack.SelectedValue = "D" Then
                'if dollar chosen then wipe out percent
                dCustomerGiveBackPercent = 0
                txtCustomerGiveBackPercent.Text = ""

                If txtCustomerGiveBackDollar.Text.Trim <> "" Then
                    dCustomerGiveBackDollar = CType(txtCustomerGiveBackDollar.Text.Trim, Double)
                    lblCustomerGiveBack.Text = Format(dCustomerGiveBackDollar, "##0.00")
                End If
            Else
                'if percent selected than wipe out dollar amount
                txtCustomerGiveBackDollar.Text = ""
                dCustomerGiveBackDollar = 0

                If txtCustomerGiveBackPercent.Text.Trim <> "" Then
                    dCustomerGiveBackPercent = CType(txtCustomerGiveBackPercent.Text.Trim, Double)
                    dCustomerGiveBackDollar = dTotalSavings * (dCustomerGiveBackPercent / 100)
                    lblCustomerGiveBack.Text = Format(dCustomerGiveBackDollar, "##0.00")
                End If
            End If

            lblTotalNetSavings.Text = Format(dTotalSavings - dCustomerGiveBackDollar, "##0.00")
            lblTotalNetSavingsBudget.Text = Format(dTotalSavingsBudget - dCustomerGiveBackDollar, "##0.00")

            If rbCustomerGiveBack.SelectedValue <> "D" Then
                dCustomerGiveBackDollar = 0
            End If

            If isUpdate = True Then

                'save data in details table
                If ViewState("isNewRecord") = True Then
                    CRModule.InsertCostReductionDetail(ViewState("pProjNo"), _
                        txtCurrentMethod.Text.Trim, _
                        txtProposedMethod.Text.Trim, _
                        txtBenefits.Text.Trim, _
                        txtCustomerPartNo.Text.Trim, _
                        dMaterialPriceCurrentPrice, _
                        dMaterialPriceCurrentPriceBudget, _
                        dMaterialPriceCurrentFreight, _
                        dMaterialPriceCurrentFreightBudget, _
                        iMaterialPriceCurrentVolume, _
                        iMaterialPriceCurrentVolumeBudget, _
                        dMaterialPriceCurrentPriceByVolume, _
                        dMaterialPriceCurrentPriceByVolumeBudget, _
                        dMaterialPriceCurrentFreightByVolume, _
                        dMaterialPriceCurrentFreightByVolumeBudget, _
                        dMaterialPriceCurrentMaterialLanded, _
                        dMaterialPriceCurrentMaterialLandedBudget, _
                        dMaterialPriceCurrentMaterialLandedTotal, _
                        dMaterialPriceCurrentMaterialLandedTotalBudget, _
                        dMaterialPriceProposedPrice, _
                        dMaterialPriceProposedFreight, _
                        iMaterialPriceProposedVolume, _
                        dMaterialPriceProposedPriceByVolume, _
                        dMaterialPriceProposedFreightByVolume, _
                        dMaterialPriceProposedMaterialLanded, _
                        dMaterialPriceProposedMaterialLandedTotal, _
                        dMaterialPriceCurrentPriceByVolume, _
                        dMaterialPriceCurrentPriceByVolumeBudget, _
                        dMaterialPriceProposedPriceByVolume, _
                        dMaterialPriceSavings, _
                        dMaterialPriceSavingsBudget, _
                        dMaterialPriceCECapital, _
                        dMaterialPriceCEMaterial, _
                        dMaterialPriceCEOutsideSupport, _
                        dMaterialPriceCEMisc, _
                        dMaterialPriceCEInHouseSupport, _
                        dMaterialPriceCETotal, _
                        dMaterialPricePayback, _
                        dMaterialUsageCurrentCostPerUnit, _
                        dMaterialUsageCurrentCostPerUnitBudget, _
                        dMaterialUsageCurrentUnitPerParent, _
                        dMaterialUsageCurrentUnitPerParentBudget, _
                        dMaterialUsageCurrentCostTotal, _
                        dMaterialUsageCurrentCostTotalBudget, _
                        dMaterialUsageProposedCostPerUnit, _
                        dMaterialUsageProposedUnitPerParent, _
                        dMaterialUsageProposedCostTotal, _
                        iMaterialUsageProgramVolume, _
                        iMaterialUsageProgramVolumeBudget, _
                        dMaterialUsageCurrentMethod, _
                        dMaterialUsageCurrentMethodBudget, _
                        dMaterialUsageProposedMethod, _
                        dMaterialUsageSavings, _
                        dMaterialUsageSavingsBudget, _
                        dMaterialUsageCECapital, _
                        dMaterialUsageCEMaterial, _
                        dMaterialUsageCEOutsideSupport, _
                        dMaterialUsageCEMisc, _
                        dMaterialUsageCEInHouseSupport, _
                        dMaterialUsageCETotal, _
                        dMaterialUsagePayback, _
                        dCycleTimeCurrentPiecesPerHour, _
                        dCycleTimeCurrentPiecesPerHourBudget, _
                        dCycleTimeCurrentCrewSize, _
                        dCycleTimeCurrentCrewSizeBudget, _
                        iCycleTimeCurrentVolume, _
                        iCycleTimeCurrentVolumeBudget, _
                        dCycleTimeCurrentMachineHourPerPieces, _
                        dCycleTimeCurrentMachineHourPerPiecesBudget, _
                        dCycleTimeCurrentManHourPerPieces, _
                        dCycleTimeCurrentManHourPerPiecesBudget, _
                        dCycleTimeCurrentMethod, _
                        dCycleTimeCurrentMethodBudget, _
                        dCycleTimeProposedPiecesPerHour, _
                        dCycleTimeProposedCrewSize, _
                        iCycleTimeProposedVolume, _
                        dCycleTimeProposedMachineHourPerPieces, _
                        dCycleTimeProposedManHourPerPieces, _
                        dCycleTimeProposedMethod, _
                        dCycleTimeFUTARate, _
                        dCycleTimeSUTARate, _
                        dCycleTimeFICARate, _
                        dCycleTimeVariableFringes, _
                        dCycleTimeWages, _
                        dCycleTimeWagesPlusFringes, _
                        dCycleTimeCurrentMethod, _
                        dCycleTimeCurrentMethodBudget, _
                        dCycleTimeProposedMethod, _
                        dCycleTimeMethodDifference, _
                        dCycleTimeMethodDifferenceBudget, _
                        dCycleTimeSavings, _
                        dCycleTimeSavingsBudget, _
                        dCycleTimeCECapital, _
                        dCycleTimeCEMaterial, _
                        dCycleTimeCEOutsideSupport, _
                        dCycleTimeCEMisc, _
                        dCycleTimeCEInHouseSupport, _
                        dCycleTimeCETotal, _
                        dCycleTimePayback, _
                        dHeadCountWages, _
                        dHeadCountWagesBudget, _
                        dHeadCountAnnualLaborCost, _
                        dHeadCountAnnualLaborCostBudget, _
                        dHeadCountCurrentLaborCount, _
                        dHeadCountCurrentLaborCountBudget, _
                        dHeadCountCurrentLaborCost, _
                        dHeadCountCurrentLaborCostBudget, _
                        dHeadCountCurrentLaborFringes, _
                        dHeadCountCurrentLaborTotal, _
                        dHeadCountCurrentLaborTotalBudget, _
                        dHeadCountProposedLaborCount, _
                        dHeadCountProposedLaborCost, _
                        dHeadCountProposedLaborFringes, _
                        dHeadCountProposedLaborTotal, _
                        dHeadCountCurrentLaborTotal, _
                        dHeadCountCurrentLaborTotalBudget, _
                        dHeadCountProposedLaborTotal, _
                        dHeadCountSavings, _
                        dHeadCountSavingsBudget, _
                        dHeadCountFUTA, _
                        dHeadCountSUTA, _
                        dHeadCountFICA, _
                        dHeadCountPension, _
                        dHeadCountBonus, _
                        dHeadCountLife, _
                        dHeadCountGroupInsurance, _
                        dHeadCountWorkersComp, _
                        dHeadCountPensionQuarterly, _
                        dHeadCountTotalFringes, _
                        dHeadCountCECapital, _
                        dHeadCountCEMaterial, _
                        dHeadCountCEOutsideSupport, _
                        dHeadCountCEMisc, _
                        dHeadCountCEInHouseSupport, _
                        dHeadCountCETotal, _
                        dHeadCountPayback, _
                        dOverheadCurrentTotalCost, _
                        dOverheadCurrentTotalCostBudget, _
                        dOverheadProposedTotalCost, _
                        dOverheadSavings, _
                        dOverheadSavingsBudget, _
                        dOverheadCECapital, _
                        dOverheadCEMaterial, _
                        dOverheadCEOutsideSupport, _
                        dOverheadCEMisc, _
                        dOverheadCEInHouseSupport, _
                        dOverheadCEWriteOff, _
                        dOverheadCETotal, _
                        dOverheadPayback, _
                        dTotalSavings, _
                        dTotalSavingsBudget, _
                        dTotalCE, _
                        dTotalPayback, _
                        dCustomerGiveBackDollar, _
                        dCustomerGiveBackPercent)
                    ViewState("isNewRecord") = False
                Else
                    CRModule.UpdateCostReductionDetail(ViewState("pProjNo"), _
                        txtCurrentMethod.Text.Trim, _
                        txtProposedMethod.Text.Trim, _
                        txtBenefits.Text.Trim, _
                        txtCustomerPartNo.Text.Trim, _
                        dMaterialPriceCurrentPrice, dMaterialPriceCurrentPriceBudget, _
                        dMaterialPriceCurrentFreight, dMaterialPriceCurrentFreightBudget, _
                        iMaterialPriceCurrentVolume, iMaterialPriceCurrentVolumeBudget, _
                        dMaterialPriceCurrentPriceByVolume, dMaterialPriceCurrentPriceByVolumeBudget, _
                        dMaterialPriceCurrentFreightByVolume, dMaterialPriceCurrentFreightByVolumeBudget, _
                        dMaterialPriceCurrentMaterialLanded, dMaterialPriceCurrentMaterialLandedBudget, _
                        dMaterialPriceCurrentMaterialLandedTotal, dMaterialPriceCurrentMaterialLandedTotalBudget, _
                        dMaterialPriceProposedPrice, _
                        dMaterialPriceProposedFreight, _
                        iMaterialPriceProposedVolume, _
                        dMaterialPriceProposedPriceByVolume, _
                        dMaterialPriceProposedFreightByVolume, _
                        dMaterialPriceProposedMaterialLanded, _
                        dMaterialPriceProposedMaterialLandedTotal, _
                        dMaterialPriceCurrentPriceByVolume, dMaterialPriceCurrentPriceByVolumeBudget, _
                        dMaterialPriceProposedPriceByVolume, _
                        dMaterialPriceSavings, dMaterialPriceSavingsBudget, _
                        dMaterialPriceCECapital, _
                        dMaterialPriceCEMaterial, _
                        dMaterialPriceCEOutsideSupport, _
                        dMaterialPriceCEMisc, _
                        dMaterialPriceCEInHouseSupport, _
                        dMaterialPriceCETotal, _
                        dMaterialPricePayback, _
                        dMaterialUsageCurrentCostPerUnit, dMaterialUsageCurrentCostPerUnitBudget, _
                        dMaterialUsageCurrentUnitPerParent, dMaterialUsageCurrentUnitPerParentBudget, _
                        dMaterialUsageCurrentCostTotal, dMaterialUsageCurrentCostTotalBudget, _
                        dMaterialUsageProposedCostPerUnit, _
                        dMaterialUsageProposedUnitPerParent, _
                        dMaterialUsageProposedCostTotal, _
                        iMaterialUsageProgramVolume, iMaterialUsageProgramVolumeBudget, _
                        dMaterialUsageCurrentMethod, dMaterialUsageCurrentMethodBudget, _
                        dMaterialUsageProposedMethod, _
                        dMaterialUsageSavings, dMaterialUsageSavingsBudget, _
                        dMaterialUsageCECapital, _
                        dMaterialUsageCEMaterial, _
                        dMaterialUsageCEOutsideSupport, _
                        dMaterialUsageCEMisc, _
                        dMaterialUsageCEInHouseSupport, _
                        dMaterialUsageCETotal, _
                        dMaterialUsagePayback, _
                        dCycleTimeCurrentPiecesPerHour, dCycleTimeCurrentPiecesPerHourBudget, _
                        dCycleTimeCurrentCrewSize, dCycleTimeCurrentCrewSizeBudget, _
                        iCycleTimeCurrentVolume, iCycleTimeCurrentVolumeBudget, _
                        dCycleTimeCurrentMachineHourPerPieces, dCycleTimeCurrentMachineHourPerPiecesBudget, _
                        dCycleTimeCurrentManHourPerPieces, dCycleTimeCurrentManHourPerPiecesBudget, _
                        dCycleTimeCurrentMethod, dCycleTimeCurrentMethodBudget, _
                        dCycleTimeProposedPiecesPerHour, _
                        dCycleTimeProposedCrewSize, _
                        iCycleTimeProposedVolume, _
                        dCycleTimeProposedMachineHourPerPieces, _
                        dCycleTimeProposedManHourPerPieces, _
                        dCycleTimeProposedMethod, _
                        dCycleTimeFUTARate, _
                        dCycleTimeSUTARate, _
                        dCycleTimeFICARate, _
                        dCycleTimeVariableFringes, _
                        dCycleTimeWages, _
                        dCycleTimeWagesPlusFringes, _
                        dCycleTimeCurrentMethod, dCycleTimeCurrentMethodBudget, _
                        dCycleTimeProposedMethod, _
                        dCycleTimeMethodDifference, dCycleTimeMethodDifferenceBudget, _
                        dCycleTimeSavings, dCycleTimeSavingsBudget, _
                        dCycleTimeCECapital, _
                        dCycleTimeCEMaterial, _
                        dCycleTimeCEOutsideSupport, _
                        dCycleTimeCEMisc, _
                        dCycleTimeCEInHouseSupport, _
                        dCycleTimeCETotal, _
                        dCycleTimePayback, _
                        dHeadCountWages, dHeadCountWagesBudget, _
                        dHeadCountAnnualLaborCost, dHeadCountAnnualLaborCostBudget, _
                        dHeadCountCurrentLaborCount, dHeadCountCurrentLaborCountBudget, _
                        dHeadCountCurrentLaborCost, dHeadCountCurrentLaborCostBudget, _
                        dHeadCountCurrentLaborFringes, _
                        dHeadCountCurrentLaborTotal, dHeadCountCurrentLaborTotalBudget, _
                        dHeadCountProposedLaborCount, _
                        dHeadCountProposedLaborCost, _
                        dHeadCountProposedLaborFringes, _
                        dHeadCountProposedLaborTotal, _
                        dHeadCountCurrentLaborTotal, dHeadCountCurrentLaborTotalBudget, _
                        dHeadCountProposedLaborTotal, _
                        dHeadCountSavings, dHeadCountSavingsBudget, _
                        dHeadCountFUTA, _
                        dHeadCountSUTA, _
                        dHeadCountFICA, _
                        dHeadCountPension, _
                        dHeadCountBonus, _
                        dHeadCountLife, _
                        dHeadCountGroupInsurance, _
                        dHeadCountWorkersComp, _
                        dHeadCountPensionQuarterly, _
                        dHeadCountTotalFringes, _
                        dHeadCountCECapital, _
                        dHeadCountCEMaterial, _
                        dHeadCountCEOutsideSupport, _
                        dHeadCountCEMisc, _
                        dHeadCountCEInHouseSupport, _
                        dHeadCountCETotal, _
                        dHeadCountPayback, _
                        dOverheadCurrentTotalCost, dOverheadCurrentTotalCostBudget, _
                        dOverheadProposedTotalCost, _
                        dOverheadSavings, dOverheadSavingsBudget, _
                        dOverheadCECapital, _
                        dOverheadCEMaterial, _
                        dOverheadCEOutsideSupport, _
                        dOverheadCEMisc, _
                        dOverheadCEInHouseSupport, _
                        dOverheadCEWriteOff, _
                        dOverheadCETotal, _
                        dOverheadPayback, _
                        dTotalSavings, dTotalSavingsBudget, _
                        dTotalCE, _
                        dTotalPayback, _
                        dCustomerGiveBackDollar, _
                        dCustomerGiveBackPercent)
                End If

                'update top level project info page
                CRModule.UpdateCostReductionSavingsAndCapEx(ViewState("pProjNo"), dTotalSavings, dTotalCE)

                If ViewState("DateSubmitted") <> "" Then                    

                    If ViewState("OriginalAnnCostSave") <> dTotalSavings Then
                        CRModule.InsertCostReductionHistory(ViewState("pProjNo"), ViewState("TeamMemberID"), txtAnnCostChngRsn.Text, "Annual Cost Save", ViewState("OriginalAnnCostSave"), lblTotalSavings.Text)
                    End If

                    'If ViewState("OriginalCapEx").ToString <> lblTotalCE.Text Then
                    If ViewState("OriginalCapEx") <> dTotalCE Then
                        CRModule.InsertCostReductionHistory(ViewState("pProjNo"), ViewState("TeamMemberID"), txtCapExChngRsn.Text, "CAPEX", ViewState("OriginalCapEx"), lblTotalCE.Text)
                    End If

                    ''***************
                    ''* Send Notification to Default Admin when values change - only send once per screen usage
                    ''***************
                    If ViewState("EmailSent") = False And (ViewState("OriginalAnnCostSave") <> dTotalSavings Or ViewState("OriginalCapEx") <> dTotalCE) Then
                        SendEmailWhenValuesChange()
                        ViewState("EmailSent") = True
                    End If

                End If

                EnableControls()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Sub SendEmailWhenValuesChange()
        Try
            ''**************************************************************************
            ''Build Email Notification, Sender, Recipient(s), Subject, Body information
            ''**************************************************************************

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim i As Integer = 0
            Dim iRowCounter As Integer = 0

            Dim ds As DataSet
            Dim dsSubscription As DataSet
            Dim dsTeamMember As DataSet
            Dim EmailTO As String = Nothing
            Dim CurrentEmpEmail As String = Nothing
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value

            If (ViewState("ProjectCategoryID") = 5 Or ViewState("ProjectCategoryID") = 6) Then 'for kaizen events only notify kaizen group
                '****** DO NOTHING ********
            Else
                'get Plant Controller by UGN Facility
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(20, ViewState("UGNFacility"))
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then
                                    If EmailTO <> "" Then
                                        EmailTO &= ";"
                                    End If

                                    EmailTO &= dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString
                                End If

                            End If
                        End If
                    Next
                End If

                'get Plant Controller from Tinley
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(20, "UT")
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And dsSubscription.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then
                                    If EmailTO <> "" Then
                                        EmailTO &= ";"
                                    End If

                                    EmailTO &= dsSubscription.Tables(0).Rows(iRowCounter).Item("Email").ToString
                                End If

                            End If
                        End If
                    Next
                End If
            End If

            'get Project Leader Email
            dsTeamMember = SecurityModule.GetTeamMember(ViewState("LeaderTMID"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                    If dsTeamMember.Tables(0).Rows(0).Item("Working") = True Then
                        If EmailTO <> "" Then
                            EmailTO &= ";"
                        End If

                        EmailTO &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    End If
                End If
            End If

            ds = commonFunctions.GetTeamMemberBySubscription(77)
            ''Check that the recipient(s) is a valid Team Member
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables.Item(0).Rows.Count - 1
                    If ds.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True And ds.Tables(0).Rows(iRowCounter).Item("Email").ToString <> CurrentEmpEmail And ds.Tables(0).Rows(iRowCounter).Item("Email").ToString <> "" Then
                        If EmailTO <> "" Then
                            EmailTO &= ";"
                        End If

                        EmailTO &= ds.Tables(0).Rows(iRowCounter).Item("Email").ToString
                    End If
                Next
                'MyMessage.CC.Add(EmailTO)
            End If

            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing Then
                If ViewState("pProjNo") <> Nothing Then
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = Nothing

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            SendTo = New MailAddress(CurrentEmpEmail) 'use for testing only
                        Else
                            'SendTo = New MailAddress(EmailTO)
                            SendTo = New MailAddress(CurrentEmpEmail)
                        End If

                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br /><br />"
                        Else
                            'build email To list
                            Dim emailList As String() = EmailTO.Split(";")

                            For i = 0 To UBound(emailList)
                                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                                    MyMessage.To.Add(emailList(i))
                                End If
                            Next i

                            'MyMessage.CC.Add(CurrentEmpEmail)
                            'MyMessage.Bcc.Add("Roderick.Carlson@ugnauto.com;Lynette.Rey@ugnauto.com")
                            'MyMessage.Bcc.Add("Lynette.Rey@ugnauto.com")
                            'MyMessage.Bcc.Add("Roderick.Carlson@ugnauto.com")
                        End If

                        MyMessage.Subject &= "Cost Reduction Project No: " & ViewState("pProjNo") & " - Changed Value(s) Alert."
                        MyMessage.Body &= "<p><font size='2' face='Verdana'>There was a change to the Cost Reduction Proposed Details"
                        MyMessage.Body &= "Project No: <u>" & ViewState("pProjNo") & "</u>. "
                        MyMessage.Body &= "<br /><br />Description: " & ViewState("Description")
                        MyMessage.Body &= "<br /><br />Open IE browser, wait a few seconds... then <a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pEM=1&pProjNo=" & ViewState("pProjNo") & "'>click here</a> to access record.</font></p>"
                        MyMessage.Body &= "<table width='60%' style='border: 1px solid #D0D0BF; border-left-style:dotted; border-right-style:dotted; border-collapse: collapse;'>"

                        MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB;'>"
                        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Field Change</strong></font></td>"
                        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Previous Value</strong></font></td>"
                        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>New Value</strong></font></td>"
                        MyMessage.Body &= "<td><font size='2' face='Verdana'><strong>Change Reason</strong></font></td>"
                        MyMessage.Body &= "</tr>"

                        ''** Annual Cost Change **
                        If txtAnnCostChngRsn.Text.Trim <> "" And ViewState("OriginalAnnCostSave").ToString <> lblTotalSavings.Text Then
                            MyMessage.Body &= "<tr style='border-color:white;'>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'> Annual Cost Savings </font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>$ " & ViewState("OriginalAnnCostSave") & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>$ " & lblTotalSavings.Text & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & txtAnnCostChngRsn.Text.Trim & "</font></td>"
                            MyMessage.Body &= "</tr>"
                        End If

                        ''** CapEx Change **
                        If txtCapExChngRsn.Text.Trim <> "" And ViewState("OriginalCapEx").ToString <> lblTotalCE.Text Then
                            MyMessage.Body &= "<tr style='border-color:white;'>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'> Cap Ex </font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>$ " & ViewState("OriginalCapEx") & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>$ " & lblTotalCE.Text & "</font></td>"
                            MyMessage.Body &= "<td height='25'><font size='2' face='Verdana'>" & txtCapExChngRsn.Text.Trim & "</font></td>"
                            MyMessage.Body &= "</tr>"
                        End If


                        MyMessage.Body &= "</Table>"

                        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                        End If

                        MyMessage.IsBodyHtml = True

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Dim emailClient As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)
                        'emailClient.Send(MyMessage)

                        Try
                            emailClient.Send(MyMessage)
                            lblMessage.Text &= "Email Notification sent."
                        Catch ex As Exception
                            lblMessage.Text &= "Email Notification queued."
                            UGNErrorTrapping.InsertEmailQueue("Cost Reduction Proposed Details Change", CurrentEmpEmail, EmailTO, "", MyMessage.Subject, MyMessage.Body, "")
                        End Try
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF SendEmailWhenValuesChange

    Private Sub ClearCustomerProgramInputFields()

        Try
            Dim ds As DataSet

            ViewState("CurrentCustomerProgramRow") = 0

            gvCustomerProgram.DataBind()
            gvCustomerProgram.SelectedIndex = -1
            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = True

            ''bind existing data to drop down Program 
            ds = commonFunctions.GetProgram("", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddProgram.DataSource = ds
                ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddProgram.DataBind()
                ddProgram.Items.Insert(0, "")
            End If

            ddMake.SelectedIndex = -1
            ddProgram.SelectedIndex = -1
            ddYear.SelectedIndex = -1

            ' ''ddCustomer.SelectedIndex = -1

            btnSaveCustomerProgram.Text = "Add Customer/Program"
            btnCancelEditCustomerProgram.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub ClearMessages()

        Try
            lblMessage.Text = ""
            lblMessageBottom.Text = ""
            lblMessageGeneral.Text = ""
            lblMessageView4.Text = ""
            lblMessageFinishedGood.Text = ""
            lblMessageCustomerProgram.Text = ""
            lblMessageCustomerProgramBottom.Text = ""
            lblMessageMaterialPrice.Text = ""
            lblMessageMaterialUsage.Text = ""
            lblMessageCycleTime.Text = ""
            lblMessageHeadCount.Text = ""
            lblMessageOverhead.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub EnableControls()

        Try

            localPanel.Visible = ViewState("isViewable")

            txtAnnCostChngRsn.Visible = False
            lblAnnCostChngRsn.Visible = False
            lblReqAnnCostChngRsnMarker.Visible = False
            rfvAnnCostChngRsn.Enabled = False
            rfvAnnCostChngRsn.ValidationGroup = ""

            lblReqCapExChngRsnMarker.Visible = False
            lblCapExChngRsn.Visible = False
            txtCapExChngRsn.Visible = False
            rfvCapExChngRsn.Enabled = False
            rfvCapExChngRsn.ValidationGroup = ""

            btnPreview.Visible = Not ViewState("isNewRecord")
            btnPreviewBottom.Visible = Not ViewState("isNewRecord")

            btnCalculate.Visible = False
            btnCalculateBottom.Visible = False
            btnReset.Visible = False
            btnResetBottom.Visible = False
            btnSave.Visible = False
            btnSaveBottom.Visible = False

            btnSaveCustomerProgram.Visible = False
            lblMake.Visible = False
            lblProgram.Visible = False
            lblYear.Visible = False
            ddMake.Visible = False
            ddProgram.Visible = False
            ddYear.Visible = False
            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = False

            gvFinishedGood.Columns(gvFinishedGood.Columns.Count - 1).Visible = False
            gvFinishedGood.ShowFooter = False

            gvOverheadCurrent.Columns(gvOverheadCurrent.Columns.Count - 1).Visible = False
            gvOverheadCurrent.ShowFooter = False

            gvOverheadProposed.Columns(gvOverheadProposed.Columns.Count - 1).Visible = False
            gvOverheadProposed.ShowFooter = False


            txtAnnCostChngRsn.Enabled = False
            txtBenefits.Enabled = False
            txtCapExChngRsn.Enabled = False
            txtCurrentMethod.Enabled = False
            txtCustomerPartNo.Enabled = False

            txtCycleTimeCECapital.Enabled = False
            txtCycleTimeCEInHouseSupport.Enabled = False
            txtCycleTimeCEMaterial.Enabled = False
            txtCycleTimeCEMisc.Enabled = False
            txtCycleTimeCEOutsideSupport.Enabled = False
            txtCycleTimeCurrentCrewSize.Enabled = False
            txtCycleTimeCurrentCrewSizeBudget.Enabled = False
            txtCycleTimeCurrentPiecesPerHour.Enabled = False
            txtCycleTimeCurrentPiecesPerHourBudget.Enabled = False
            txtCycleTimeCurrentVolume.Enabled = False
            txtCycleTimeCurrentVolumeBudget.Enabled = False
            txtCycleTimeFICARate.Enabled = False
            txtCycleTimeFUTARate.Enabled = False
            txtCycleTimeProposedCrewSize.Enabled = False
            txtCycleTimeProposedPiecesPerHour.Enabled = False
            txtCycleTimeProposedVolume.Enabled = False
            txtCycleTimeSUTARate.Enabled = False
            txtCycleTimeWages.Enabled = False

            txtHeadCountBonus.Enabled = False
            txtHeadCountCECapital.Enabled = False
            txtHeadCountCEInHouseSupport.Enabled = False
            txtHeadCountCEMaterial.Enabled = False
            txtHeadCountCEMisc.Enabled = False
            txtHeadCountCEOutsideSupport.Enabled = False
            txtHeadCountCurrentLaborCount.Enabled = False
            txtHeadCountCurrentLaborCountBudget.Enabled = False
            txtHeadCountFICA.Enabled = False
            txtHeadCountFUTA.Enabled = False
            txtHeadCountGroupInsurance.Enabled = False
            txtHeadCountLife.Enabled = False
            txtHeadCountPension.Enabled = False
            txtHeadCountPensionQuarterly.Enabled = False
            txtHeadCountProposedLaborCount.Enabled = False
            txtHeadCountSUTA.Enabled = False
            txtHeadCountWages.Enabled = False
            txtHeadCountWagesBudget.Enabled = False

            txtMaterialPriceCECapital.Enabled = False
            txtMaterialPriceCEInHouseSupport.Enabled = False
            txtMaterialPriceCEMaterial.Enabled = False
            txtMaterialPriceCEMisc.Enabled = False
            txtMaterialPriceCEOutsideSupport.Enabled = False
            txtMaterialPriceCurrentFreight.Enabled = False
            txtMaterialPriceCurrentFreightBudget.Enabled = False
            txtMaterialPriceCurrentPrice.Enabled = False
            txtMaterialPriceCurrentPriceBudget.Enabled = False
            txtMaterialPriceCurrentVolume.Enabled = False
            txtMaterialPriceCurrentVolumeBudget.Enabled = False
            txtMaterialPriceProposedFreight.Enabled = False
            txtMaterialPriceProposedPrice.Enabled = False
            txtMaterialPriceProposedVolume.Enabled = False

            txtMaterialUsageCECapital.Enabled = False
            txtMaterialUsageCEInHouseSupport.Enabled = False
            txtMaterialUsageCEMaterial.Enabled = False
            txtMaterialUsageCEMisc.Enabled = False
            txtMaterialUsageCEOutsideSupport.Enabled = False
            txtMaterialUsageCurrentCostPerUnit.Enabled = False
            txtMaterialUsageCurrentCostPerUnitBudget.Enabled = False
            txtMaterialUsageCurrentUnitPerParent.Enabled = False
            txtMaterialUsageCurrentUnitPerParentBudget.Enabled = False
            txtMaterialUsageProgramVolume.Enabled = False
            txtMaterialUsageProgramVolumeBudget.Enabled = False
            txtMaterialUsageProposedCostPerUnit.Enabled = False
            txtMaterialUsageProposedUnitPerParent.Enabled = False

            txtOverheadCECapital.Enabled = False
            txtOverheadCEInHouseSupport.Enabled = False
            txtOverheadCEMaterial.Enabled = False
            txtOverheadCEMisc.Enabled = False
            txtOverheadCEOutsideSupport.Enabled = False
            txtOverheadCEWriteOff.Enabled = False
            txtProposedMethod.Enabled = False

            txtCustomerGiveBackDollar.Enabled = False
            txtCustomerGiveBackPercent.Enabled = False
            rbCustomerGiveBack.Enabled = False

            If ViewState("LeaderTMID") = ViewState("TeamMemberID") Or ViewState("Admin") = True Or ViewState("SubscriptionID") = 9 Then

                'after the plant controller has reviewed the project and checked the box on the main page
                'then only the plant controller can update this page with one exception
                'sales can update the customer giveback fields
                If ViewState("isPlantControllerReviewed") = True And ViewState("SubscriptionID") = 20 Then
                    ViewState("ObjectRole") = True
                End If

                If ViewState("isPlantControllerReviewed") = True And ViewState("SubscriptionID") <> 20 And ViewState("SubscriptionID") <> 9 Then
                    ViewState("ObjectRole") = False
                End If

                'sales can update the customer giveback fields

                'btnCalculate.Visible = ViewState("ObjectRole")
                'btnCalculateBottom.Visible = ViewState("ObjectRole")
                btnReset.Visible = ViewState("ObjectRole")
                btnResetBottom.Visible = ViewState("ObjectRole")
                btnSave.Visible = ViewState("ObjectRole")
                btnSaveBottom.Visible = ViewState("ObjectRole")
                btnSaveCustomerProgram.Visible = ViewState("ObjectRole")

                ' ''ddCustomer.Visible = ViewState("ObjectRole")
                ddMake.Visible = ViewState("ObjectRole")
                ddProgram.Visible = ViewState("ObjectRole")
                ddYear.Visible = ViewState("ObjectRole")

                ' ''iBtnCustomerPartNoSearch.Visible = ViewState("ObjectRole")

                gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = ViewState("ObjectRole")
                gvFinishedGood.Columns(gvFinishedGood.Columns.Count - 1).Visible = ViewState("ObjectRole")
                gvFinishedGood.ShowFooter = ViewState("ObjectRole")

                lblMake.Visible = ViewState("ObjectRole")
                lblProgram.Visible = ViewState("ObjectRole")
                lblYear.Visible = ViewState("ObjectRole")

                txtAnnCostChngRsn.Enabled = ViewState("ObjectRole")
                txtBenefits.Enabled = ViewState("ObjectRole")
                txtCapExChngRsn.Enabled = ViewState("ObjectRole")
                txtCurrentMethod.Enabled = ViewState("ObjectRole")
                txtCustomerPartNo.Enabled = ViewState("ObjectRole")

                txtProposedMethod.Enabled = ViewState("ObjectRole")

                txtCustomerGiveBackDollar.Enabled = ViewState("ObjectRole")
                txtCustomerGiveBackPercent.Enabled = ViewState("ObjectRole")
                rbCustomerGiveBack.Enabled = ViewState("ObjectRole")

                If ViewState("SubscriptionID") <> 9 Then
                    btnCalculate.Visible = ViewState("ObjectRole")
                    btnCalculateBottom.Visible = ViewState("ObjectRole")

                    gvOverheadCurrent.Columns(gvOverheadCurrent.Columns.Count - 1).Visible = ViewState("ObjectRole")
                    gvOverheadCurrent.ShowFooter = ViewState("ObjectRole")

                    gvOverheadProposed.Columns(gvOverheadProposed.Columns.Count - 1).Visible = ViewState("ObjectRole")
                    gvOverheadProposed.ShowFooter = ViewState("ObjectRole")

                    txtCycleTimeCECapital.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCEInHouseSupport.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCEMaterial.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCEMisc.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCEOutsideSupport.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCurrentCrewSize.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCurrentCrewSizeBudget.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCurrentPiecesPerHour.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCurrentPiecesPerHourBudget.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCurrentVolume.Enabled = ViewState("ObjectRole")
                    txtCycleTimeCurrentVolumeBudget.Enabled = ViewState("ObjectRole")
                    txtCycleTimeFICARate.Enabled = ViewState("ObjectRole")
                    txtCycleTimeFUTARate.Enabled = ViewState("ObjectRole")
                    txtCycleTimeProposedCrewSize.Enabled = ViewState("ObjectRole")
                    txtCycleTimeProposedPiecesPerHour.Enabled = ViewState("ObjectRole")
                    txtCycleTimeProposedVolume.Enabled = ViewState("ObjectRole")
                    txtCycleTimeSUTARate.Enabled = ViewState("ObjectRole")
                    txtCycleTimeWages.Enabled = ViewState("ObjectRole")

                    txtHeadCountBonus.Enabled = ViewState("ObjectRole")
                    txtHeadCountCECapital.Enabled = ViewState("ObjectRole")
                    txtHeadCountCEInHouseSupport.Enabled = ViewState("ObjectRole")
                    txtHeadCountCEMaterial.Enabled = ViewState("ObjectRole")
                    txtHeadCountCEMisc.Enabled = ViewState("ObjectRole")
                    txtHeadCountCEOutsideSupport.Enabled = ViewState("ObjectRole")
                    txtHeadCountCurrentLaborCount.Enabled = ViewState("ObjectRole")
                    txtHeadCountCurrentLaborCountBudget.Enabled = ViewState("ObjectRole")
                    txtHeadCountFICA.Enabled = ViewState("ObjectRole")
                    txtHeadCountFUTA.Enabled = ViewState("ObjectRole")
                    txtHeadCountGroupInsurance.Enabled = ViewState("ObjectRole")
                    txtHeadCountLife.Enabled = ViewState("ObjectRole")
                    txtHeadCountPension.Enabled = ViewState("ObjectRole")
                    txtHeadCountPensionQuarterly.Enabled = ViewState("ObjectRole")
                    txtHeadCountProposedLaborCount.Enabled = ViewState("ObjectRole")
                    txtHeadCountSUTA.Enabled = ViewState("ObjectRole")
                    txtHeadCountWages.Enabled = ViewState("ObjectRole")
                    txtHeadCountWagesBudget.Enabled = ViewState("ObjectRole")

                    txtMaterialPriceCECapital.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCEInHouseSupport.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCEMaterial.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCEMisc.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCEOutsideSupport.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCurrentFreight.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCurrentFreightBudget.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCurrentPrice.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCurrentPriceBudget.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCurrentVolume.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceCurrentVolumeBudget.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceProposedFreight.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceProposedPrice.Enabled = ViewState("ObjectRole")
                    txtMaterialPriceProposedVolume.Enabled = ViewState("ObjectRole")

                    txtMaterialUsageCECapital.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageCEInHouseSupport.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageCEMaterial.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageCEMisc.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageCEOutsideSupport.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageCurrentCostPerUnit.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageCurrentCostPerUnitBudget.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageCurrentUnitPerParent.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageCurrentUnitPerParentBudget.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageProgramVolume.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageProgramVolumeBudget.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageProposedCostPerUnit.Enabled = ViewState("ObjectRole")
                    txtMaterialUsageProposedUnitPerParent.Enabled = ViewState("ObjectRole")

                    txtOverheadCECapital.Enabled = ViewState("ObjectRole")
                    txtOverheadCEInHouseSupport.Enabled = ViewState("ObjectRole")
                    txtOverheadCEMaterial.Enabled = ViewState("ObjectRole")
                    txtOverheadCEMisc.Enabled = ViewState("ObjectRole")
                    txtOverheadCEOutsideSupport.Enabled = ViewState("ObjectRole")
                    txtOverheadCEWriteOff.Enabled = ViewState("ObjectRole")

                End If

                If ViewState("DateSubmitted") <> "" And ViewState("ObjectRole") = True Then
                    txtAnnCostChngRsn.Visible = True
                    lblAnnCostChngRsn.Visible = True
                    lblReqAnnCostChngRsnMarker.Visible = True
                    rfvAnnCostChngRsn.Enabled = True
                    rfvAnnCostChngRsn.ValidationGroup = "vgSave"

                    If ViewState("SubscriptionID") = 9 Then
                        txtAnnCostChngRsn.Text = "Updating Customer Give Back"
                    Else
                        lblReqCapExChngRsnMarker.Visible = True
                        lblCapExChngRsn.Visible = True
                        txtCapExChngRsn.Visible = True
                        rfvCapExChngRsn.Enabled = True
                        rfvCapExChngRsn.ValidationGroup = "vgSave"
                    End If

                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub FilterProgramList(ByVal Make As String)

        Try
            Dim dsProgram As DataSet

            dsProgram = commonFunctions.GetProgram("", "", Make)
            If commonFunctions.CheckDataSet(dsProgram) = True Then
                ddProgram.Items.Clear()
                ddProgram.DataSource = dsProgram
                ddProgram.DataTextField = dsProgram.Tables(0).Columns("ddProgramName").ColumnName.ToString
                ddProgram.DataValueField = dsProgram.Tables(0).Columns("ProgramID").ColumnName
                ddProgram.DataBind()
                ddProgram.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Function HandleCustomerPartNoPopUps(ByVal CustomerccPartNo As String) As String

        Try
            ' Build the client script to open a popup window 
            ' Pass the ClientID of the TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../DataMaintenance/CustomerPartNoLookUp.aspx?CustomervcPartNo=" & CustomerccPartNo
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','CustomerPartNoPopupSearch','" & _
                strWindowAttribs & "');return false;"

            HandleCustomerPartNoPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleCustomerPartNoPopUps = ""
        End Try

    End Function

    Protected Sub HandleMultiLineFields()

        Try

            txtCurrentMethod.Attributes.Add("onkeypress", "return tbLimit();")
            txtCurrentMethod.Attributes.Add("onkeyup", "return tbCount(" + lblCurrentMethodCharCount.ClientID + ");")
            txtCurrentMethod.Attributes.Add("maxLength", "400")

            txtProposedMethod.Attributes.Add("onkeypress", "return tbLimit();")
            txtProposedMethod.Attributes.Add("onkeyup", "return tbCount(" + lblProposedMethodCharCount.ClientID + ");")
            txtProposedMethod.Attributes.Add("maxLength", "400")

            txtBenefits.Attributes.Add("onkeypress", "return tbLimit();")
            txtBenefits.Attributes.Add("onkeyup", "return tbCount(" + lblBenefitsCharCount.ClientID + ");")
            txtBenefits.Attributes.Add("maxLength", "400")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub ddMake_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMake.SelectedIndexChanged

        Try
            ClearMessages()

            If ddMake.SelectedIndex > 0 Then
                FilterProgramList(ddMake.SelectedValue)
            Else
                FilterProgramList("")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub

    Protected Sub gvCustomerProgram_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerProgram.DataBound

        'hide header columns
        If gvCustomerProgram.Rows.Count > 0 Then
            gvCustomerProgram.HeaderRow.Cells(0).Visible = False
            gvCustomerProgram.HeaderRow.Cells(1).Visible = False

        End If

    End Sub

    Protected Sub gvCustomerProgram_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomerProgram.RowCreated

        'hide first column
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")

        End If

        'If e.Row.RowType = DataControlRowType.Footer Then
        '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
        'End If

    End Sub

    Protected Sub gvFinishedGood_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFinishedGood.DataBound

        'hide header columns
        If gvFinishedGood.Rows.Count > 0 Then
            gvFinishedGood.HeaderRow.Cells(0).Visible = False
            gvFinishedGood.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Private Property LoadDataEmpty_FinishedGood() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FinishedGood") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FinishedGood"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FinishedGood") = value
        End Set

    End Property

    Protected Sub gvFinishedGood_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFinishedGood.RowCommand

        Try

            ClearMessages()

            Dim ds As DataSet
            Dim bContinue As Boolean = False

            Dim txtFinishedGoodPartNoTemp As TextBox
            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("pProjNo") > 0) Then

                txtFinishedGoodPartNoTemp = CType(gvFinishedGood.FooterRow.FindControl("txtInsertPartNo"), TextBox)

                '' CHECK TO SEE IF THE PARTNO IS VALID BEFORE INSERTING
                ds = commonFunctions.GetBPCSPartNo(txtFinishedGoodPartNoTemp.Text, "")

                If commonFunctions.CheckDataSet(ds) = True Then

                    If txtCustomerPartNo.Text.Trim = "" Then
                        bContinue = True
                    End If

                If bContinue = True Then
                    odsFinishedGood.InsertParameters("ProjectNo").DefaultValue = ViewState("pProjNo")
                    odsFinishedGood.InsertParameters("PartNo").DefaultValue = txtFinishedGoodPartNoTemp.Text.Trim

                    intRowsAffected = odsFinishedGood.Insert()

                    lblMessage.Text &= "Record Saved Successfully.<br />"
                End If

            Else
                lblMessage.Text &= "Error: The Finished Good BPCS Part number is invalid. The information was NOT saved.<br />"
            End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFinishedGood.ShowFooter = False
            Else
                gvFinishedGood.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtFinishedGoodPartNoTemp = CType(gvFinishedGood.FooterRow.FindControl("txtInsertPartNo"), TextBox)
                txtFinishedGoodPartNoTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageFinishedGood.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text
        lblMessageGeneral.Text = lblMessage.Text

    End Sub

    Protected Sub gvFinishedGood_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFinishedGood.RowCreated

        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FinishedGood
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

    Protected Sub gvOverheadCurrent_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOverheadCurrent.DataBound

        'hide header columns
        If gvOverheadCurrent.Rows.Count > 0 Then
            gvOverheadCurrent.HeaderRow.Cells(0).Visible = False
            gvOverheadCurrent.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Protected Sub gvOverheadCurrent_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvOverheadCurrent.RowCommand

        Try

            ClearMessages()

            Dim txtOverheadExpensedNameTemp As TextBox
            Dim txtOverheadCurrentCostPerUnitTemp As TextBox
            Dim txtOverheadCurrentVolumeTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("pProjNo") > 0) Then

                txtOverheadExpensedNameTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertCurrentExpensedName"), TextBox)
                txtOverheadCurrentCostPerUnitTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertCurrentCostPerUnit"), TextBox)
                txtOverheadCurrentVolumeTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertCurrentVolume"), TextBox)

                odsOverheadCurrent.InsertParameters("ProjectNo").DefaultValue = ViewState("pProjNo")
                odsOverheadCurrent.InsertParameters("ExpensedName").DefaultValue = txtOverheadExpensedNameTemp.Text.Trim
                odsOverheadCurrent.InsertParameters("CurrentCostPerUnit").DefaultValue = txtOverheadCurrentCostPerUnitTemp.Text.Trim
                odsOverheadCurrent.InsertParameters("CurrentVolume").DefaultValue = txtOverheadCurrentVolumeTemp.Text.Trim

                intRowsAffected = odsOverheadCurrent.Insert()

                gvOverheadProposed.DataBind()

                lblMessage.Text = "Record Saved Successfully.<br />"

                Calculate(True)

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvOverheadCurrent.ShowFooter = False
                gvOverheadProposed.ShowFooter = False
                gvOverheadProposed.Columns(gvOverheadProposed.Columns.Count - 1).Visible = False
            Else
                gvOverheadCurrent.ShowFooter = True
                gvOverheadProposed.ShowFooter = True
                gvOverheadProposed.Columns(gvOverheadProposed.Columns.Count - 1).Visible = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtOverheadExpensedNameTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertCurrentExpensedName"), TextBox)
                txtOverheadExpensedNameTemp.Text = ""

                txtOverheadCurrentCostPerUnitTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertCurrentCostPerUnit"), TextBox)
                txtOverheadCurrentCostPerUnitTemp.Text = ""

                txtOverheadCurrentVolumeTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertCurrentVolume"), TextBox)
                txtOverheadCurrentVolumeTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageOverhead.Text = lblMessage.Text

    End Sub

    Protected Sub gvOverheadCurrent_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOverheadCurrent.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
        End If

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
        ' when binding a row, look for a zero row condition based on the flag.
        ' if we have zero data rows (but a dummy row), hide the grid view row
        ' and clear the controls off of that row so they don't cause binding errors

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_OverheadCurrent
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If

    End Sub

    Protected Sub gvOverheadProposed_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOverheadProposed.DataBound

        'hide header columns
        If gvOverheadProposed.Rows.Count > 0 Then
            gvOverheadProposed.HeaderRow.Cells(0).Visible = False
            gvOverheadProposed.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Protected Sub gvOverheadProposed_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvOverheadProposed.RowCommand

        Try

            ClearMessages()

            Dim txtOverheadExpensedNameTemp As TextBox
            Dim txtOverheadProposedCostPerUnitTemp As TextBox
            Dim txtOverheadProposedVolumeTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("pProjNo") > 0) Then

                txtOverheadExpensedNameTemp = CType(gvOverheadProposed.FooterRow.FindControl("txtInsertProposedExpensedName"), TextBox)
                txtOverheadProposedCostPerUnitTemp = CType(gvOverheadProposed.FooterRow.FindControl("txtInsertProposedCostPerUnit"), TextBox)
                txtOverheadProposedVolumeTemp = CType(gvOverheadProposed.FooterRow.FindControl("txtInsertProposedVolume"), TextBox)

                odsOverheadProposed.InsertParameters("ProjectNo").DefaultValue = ViewState("pProjNo")
                odsOverheadProposed.InsertParameters("ExpensedName").DefaultValue = txtOverheadExpensedNameTemp.Text.Trim
                odsOverheadProposed.InsertParameters("ProposedCostPerUnit").DefaultValue = txtOverheadProposedCostPerUnitTemp.Text.Trim
                odsOverheadProposed.InsertParameters("ProposedVolume").DefaultValue = txtOverheadProposedVolumeTemp.Text.Trim

                intRowsAffected = odsOverheadProposed.Insert()

                gvOverheadCurrent.DataBind()

                lblMessage.Text = "Record Saved Successfully.<br />"

                Calculate(True)

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvOverheadProposed.ShowFooter = False
                gvOverheadCurrent.ShowFooter = False
                gvOverheadCurrent.Columns(gvOverheadCurrent.Columns.Count - 1).Visible = False
            Else
                gvOverheadProposed.ShowFooter = True
                gvOverheadCurrent.ShowFooter = True
                gvOverheadCurrent.Columns(gvOverheadCurrent.Columns.Count - 1).Visible = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtOverheadExpensedNameTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertProposedExpensedName"), TextBox)
                txtOverheadExpensedNameTemp.Text = ""

                txtOverheadProposedCostPerUnitTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertProposedCostPerUnit"), TextBox)
                txtOverheadProposedCostPerUnitTemp.Text = ""

                txtOverheadProposedVolumeTemp = CType(gvOverheadCurrent.FooterRow.FindControl("txtInsertProposedVolume"), TextBox)
                txtOverheadProposedVolumeTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageOverhead.Text = lblMessage.Text

    End Sub

    Protected Sub gvOverheadProposed_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOverheadProposed.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
        End If

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
        ' when binding a row, look for a zero row condition based on the flag.
        ' if we have zero data rows (but a dummy row), hide the grid view row
        ' and clear the controls off of that row so they don't cause binding errors

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_OverheadProposed
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If

    End Sub

    Protected Function UpdateFinishedGoodList() As Boolean

        Dim bResult As Boolean = True

        Try

            Dim objCRFinishedGood As New CRFinishedGoodBLL
            Dim dtFGPartNos As DataTable
            Dim iFGPartNoRowCounter As Integer = 0
            Dim iRowID As Integer = 0
            Dim strFGPartNo As String = ""

            dtFGPartNos = objCRFinishedGood.GetCostReductionFinishedGood(ViewState("pProjNo"))

            'check to see if customer partno exists and (either has changed or if finished good list is empty)
            If txtCustomerPartNo.Text.Trim <> "" And (txtCustomerPartNo.Text.Trim <> ViewState("CustomerPartNo") Or dtFGPartNos.Rows.Count = 0) Then

                'obsolete current finished good list
                For iFGPartNoRowCounter = 0 To dtFGPartNos.Rows.Count - 1
                    If dtFGPartNos.Rows(iFGPartNoRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If dtFGPartNos.Rows(iFGPartNoRowCounter).Item("RowID") > 0 Then
                            iRowID = dtFGPartNos.Rows(iFGPartNoRowCounter).Item("RowID")
                            objCRFinishedGood.DeleteCostReductionFinishedGood(iRowID, ViewState("pProjNo"), iRowID)
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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return bResult

    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveBottom.Click

        Try
            ClearMessages()

            'Page.Validate()

            'If Page.IsValid Then

            'FIND ALL F.G. BPCS PartNos based on Customer PartNo.
            If UpdateFinishedGoodList() = True Then
                'do calculations and save
                Calculate(True)
            Else
                'do calculations and DO NOT SAVE
                Calculate(False)
            End If

            'Else
            'lblMessage.Text &= "Error: THE INFORMATION DID NOT SAVE!"
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageFinishedGood.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text
        lblMessageCustomerProgram.Text = lblMessage.Text
        lblMessageCustomerProgramBottom.Text = lblMessage.Text
        lblMessageGeneral.Text = lblMessage.Text

    End Sub

    Protected Sub odsFinishedGood_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFinishedGood.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As CostReduction.Cost_Reduction_Finished_GoodDataTable = CType(e.ReturnValue, CostReduction.Cost_Reduction_Finished_GoodDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FinishedGood = True
            Else
                LoadDataEmpty_FinishedGood = False
            End If
        End If

    End Sub

    Protected Sub gvFinishedGood_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFinishedGood.RowDataBound

        Try
            ' Build the client script to open a popup window
            ' Pass the ClientID of the  TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnEditSearchPartNo"), ImageButton)
                Dim txtEditPartNo As TextBox = CType(e.Row.FindControl("txtEditPartNo"), TextBox)

                If ibtn IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & txtEditPartNo.ClientID & "&vcPartRevision="
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
                End If

            End If

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnInsertSearchPartNo"), ImageButton)
                Dim txtInsertPartNo As TextBox = CType(e.Row.FindControl("txtInsertPartNo"), TextBox)
                If ibtn IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & txtInsertPartNo.ClientID & "&vcPartRevision="
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageFinishedGood.Text = lblMessage.Text

    End Sub

    'Protected Sub gvCustomerProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerProgram.SelectedIndexChanged

    '    Try
    '        ClearMessages()

    '        Dim ds As DataSet
    '        Dim iProgramID As Integer = 0
    '        Dim iProgramYear As Integer = 0

    '        ddMake.SelectedIndex = -1

    '        ''bind existing data to drop down Program 
    '        ds = commonFunctions.GetProgram("", "", "")
    '        If commonFunctions.CheckDataset(ds) = True Then
    '            ddProgram.DataSource = ds
    '            ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
    '            ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
    '            ddProgram.DataBind()
    '            ddProgram.Items.Insert(0, "")
    '        End If

    '        ViewState("CurrentCustomerProgramRow") = gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(0).Text

    '        iProgramID = IIf(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(1).Text = "", 0, gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(1).Text)
    '        If iProgramID > 0 Then
    '            ddProgram.SelectedValue = iProgramID
    '        End If

    '        If Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(3).Text, "&nbsp;", "") <> "" Then
    '            iProgramYear = CType(Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(4).Text, "&nbsp;", ""), Integer)
    '            If iProgramYear > 0 Then
    '                ddYear.SelectedValue = iProgramYear
    '            End If
    '        End If



    '        btnSaveCustomerProgram.Text = "Update Customer/Program"
    '        btnCancelEditCustomerProgram.Visible = True

    '        gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = False

    '        lblMessage.Text = "WHILE EDITING CUSTOMER/PROGRAM, PLEASE REMEMBER TO CLICK THE ADD/UPDATE BUTTON."
    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    '    lblMessageCustomerProgram.Text = lblMessage.Text

    'End Sub

    Protected Sub btnCancelEditCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelEditCustomerProgram.Click


        Try
            ClearMessages()

            ClearCustomerProgramInputFields()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub

    Private Property LoadDataEmpty_OverheadCurrent() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_OverheadCurrent") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_OverheadCurrent"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_OverheadCurrent") = value
        End Set

    End Property

    Protected Sub odsOverheadCurrent_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsOverheadCurrent.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As CostReduction.Cost_Reduction_OverheadDataTable = CType(e.ReturnValue, CostReduction.Cost_Reduction_OverheadDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_OverheadCurrent = True
            Else
                LoadDataEmpty_OverheadCurrent = False
            End If
        End If

    End Sub

    Private Property LoadDataEmpty_OverheadProposed() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_OverheadProposed") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_OverheadProposed"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_OverheadProposed") = value
        End Set

    End Property

    Protected Sub odsOverheadProposed_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsOverheadProposed.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As CostReduction.Cost_Reduction_OverheadDataTable = CType(e.ReturnValue, CostReduction.Cost_Reduction_OverheadDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_OverheadProposed = True
            Else
                LoadDataEmpty_OverheadProposed = False
            End If
        End If

    End Sub

    Protected Sub gvOverheadCurrent_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvOverheadCurrent.RowDeleted

        gvOverheadProposed.DataBind()
        Calculate(True)

    End Sub

    Protected Sub gvOverheadProposed_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvOverheadProposed.RowDeleted

        gvOverheadCurrent.DataBind()
        Calculate(True)

    End Sub

    Protected Sub gvOverheadProposed_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvOverheadProposed.RowUpdated

        gvOverheadCurrent.DataBind()
        Calculate(True)

    End Sub

    Protected Sub gvOverheadCurrent_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvOverheadCurrent.RowUpdated

        gvOverheadProposed.DataBind()
        Calculate(True)

    End Sub

    Protected Sub btnSaveCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCustomerProgram.Click

        ClearMessages()

        Try
            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0
            '(LREY) 01/08/2014
            ' ''Dim strCABBV As String = "" '' commonFunctions.GetCustomerCABBV(ddCustomer.SelectedValue)
            ' ''Dim iSoldTo As Integer = "" ''commonFunctions.GetCustomerSoldTo(ddCustomer.SelectedValue)

            If ddProgram.SelectedIndex > 0 Then
                iProgramID = ddProgram.SelectedValue
            End If

            If ddYear.SelectedIndex > 0 Then
                iProgramYear = ddYear.SelectedValue
            End If

            If iProgramID > 0 Then
                If ViewState("CurrentCustomerProgramRow") > 0 Then
                    CRModule.UpdateCostReductionCustomerProgram(ViewState("CurrentCustomerProgramRow"), ViewState("pProjNo"), iProgramID, iProgramYear)
                Else
                    CRModule.InsertCostReductionCustomerProgram(ViewState("pProjNo"), iProgramID, iProgramYear)
                End If
            End If

            ClearCustomerProgramInputFields()

            If HttpContext.Current.Session("BLLerror") IsNot Nothing Then
                lblMessage.Text &= HttpContext.Current.Session("BLLerror")
            Else
                HttpContext.Current.Session("BLLerror") = Nothing
                lblMessage.Text &= "Program added successfully."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click, btnResetBottom.Click

        Try
            ClearMessages()

            If ViewState("pProjNo") > 0 Then
                Response.Redirect("CostReductionProposedDetail.aspx?pProjNo=" & ViewState("pProjNo"), False)
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message & ", TESTING", System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalculate.Click, btnCalculateBottom.Click


        Try
            ClearMessages()

            'do calculations but do not save yet
            Calculate(False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub btnReturnToProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturnToProject.Click

        Try
            ClearMessages()

            If ViewState("pProjNo") > 0 Then
                Response.Redirect("CostReduction.aspx?pProjNo=" & ViewState("pProjNo"), False)
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageFinishedGood.Text = lblMessage.Text
        lblMessageBottom.Text = lblMessage.Text
        lblMessageCustomerProgram.Text = lblMessage.Text
        lblMessageCustomerProgramBottom.Text = lblMessage.Text
        lblMessageGeneral.Text = lblMessage.Text

    End Sub

    Public Function DisplayImage(ByVal EncodeType As String) As String
        Dim strReturn As String = ""

        'If EncodeType = Nothing Then
        '    strReturn = ""
        'ElseIf EncodeType = "application/vnd.ms-excel" Then
        '    strReturn = "~/images/xls.jpg"
        'ElseIf EncodeType = "application/pdf" Then
        '    strReturn = "~/images/pdf.jpg"
        'ElseIf EncodeType = "application/msword" Then
        '    strReturn = "~/images/doc.jpg"
        'End If
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
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultDate As Date = Date.Today

            ClearMessages()

            'lblMessageView4.Visible = False

            If ViewState("pProjNo") <> "" Then
                If uploadFile.HasFile Then
                    If uploadFile.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFile.FileName)
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFile.PostedFile.FileName)
                        'Dim BinaryFile(uploadFile.PostedFile.InputStream.Length) As Byte
                        'Dim EncodeType As String = uploadFile.PostedFile.ContentType
                        'uploadFile.PostedFile.InputStream.Read(BinaryFile, 0, BinaryFile.Length)
                        'Dim FileSize As Integer = uploadFile.PostedFile.ContentLength

                        'If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Then
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFile.PostedFile.InputStream.Length)

                        Dim SupportingDocEncodeType As String = uploadFile.PostedFile.ContentType

                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFile.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".xlsx") Or (FileExt = ".doc") Or (FileExt = ".docx") Or (FileExt = ".jpeg") Or (FileExt = ".jpg") Or (FileExt = ".tif") Or (FileExt = ".msg") Or (FileExt = ".ppt") Then
                            ''*************
                            '' Display File Info
                            ''*************
                            lblMessageView4.Text = "File name: " & uploadFile.FileName & "<br />" & _
                            "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br />"
                            'lblMessageView4.Visible = True
                            lblMessageView4.Width = 500
                            lblMessageView4.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            'CRModule.InsertCostReductionDocuments(ViewState("pProjNo"), ddTeamMember.SelectedValue, txtFileDesc.Text, BinaryFile, uploadFile.FileName, EncodeType, FileSize)
                            CRModule.InsertCostReductionDocuments(ViewState("pProjNo"), ddTeamMember.SelectedValue, txtFileDesc.Text, SupportingDocBinaryFile, uploadFile.FileName, SupportingDocEncodeType, SupportingDocFileSize)
                            gvSupportingDocument.DataBind()
                            revUploadFile.Enabled = False
                            txtFileDesc.Text = Nothing
                        End If
                    Else
                        lblMessageView4.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        'lblMessageView4.Visible = True
                        btnUpload.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF btnUpload_Click

    Protected Sub gvSupportingDocument_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupportingDocument.RowDataBound
        '***
        'This section provides the user with the popup for confirming the delete of a record.
        'Called by the onClientClick event.
        '***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(3).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As CostReduction.Cost_Reduction_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, CostReduction.Cost_Reduction_DocumentsRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record (" & DataBinder.Eval(e.Row.DataItem, "Description") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gvSupportingDocument_RowDataBound

    Protected Sub gvSupportingDocument_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSupportingDocument.RowCommand
        ''***
        ''This section allows the inserting of a new row when save button is clicked from the footer.
        ''***
        If e.CommandName = "Delete" Then
            ''Reprompt current page
            Dim Aprv As String = Nothing
            If ViewState("pAprv") = 1 Then
                Aprv = "&pAprv=1"
            End If
            Response.Redirect("CostReductionProposedDetail.aspx?pProjNo=" & ViewState("pProjNo") & "&pSD=1" & Aprv, False)
        End If
    End Sub 'EOF gvSupportingDocument_RowCommand

    Protected Sub rbCustomerGiveBack_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCustomerGiveBack.TextChanged

        Try
            If rbCustomerGiveBack.SelectedValue = "D" Then
                tblCustomerGiveBackByDollar.Visible = True
                tblCustomerGiveBackByPercent.Visible = False

                If txtCustomerGiveBackDollar.Text.Trim = "" Then
                    txtCustomerGiveBackDollar.Text = txtCustomerGiveBackPercent.Text
                End If
            Else
                tblCustomerGiveBackByDollar.Visible = False
                tblCustomerGiveBackByPercent.Visible = True

                If txtCustomerGiveBackPercent.Text.Trim = "" Then
                    txtCustomerGiveBackPercent.Text = txtCustomerGiveBackDollar.Text
                End If
            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
End Class
