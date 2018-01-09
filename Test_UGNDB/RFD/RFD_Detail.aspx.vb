' ************************************************************************************************
'
' Name:		RFD_Detail.aspx
' Purpose:	This Code Behind is for the Request for Development Details
'
' Subscription		            SubscriptionID	Routing Level
' Product Engineering/Development   5				1
' Purchasing External RFQ           139             2
' Capital                           119             2
' Packaging			                108				2
' Process				            66				2
' Plant Controller		            20				2
' Tooling				            65				2
' Costing				            6				3
' QE					            22				4
' Purchasing Contract PO			7				5
'
' Date		Author	    
' 08/04/2008 Created : Roderick Carlson
' 02/11/2011  Roderick Carlson - Requested by John Mercado - send approval request to Plant Controller directly.
'                                So, what can be done is the following. Since the Facility is not determined 
'                                until after initial creations, then the Corporate Controller (John Mercado) 
'                                will be selected by default. Once the team member assigns the first facility,
'                                then the facility plant controller will be selected. Also, the Corporate 
'                                Controller will be notified when the Plant Controller has approved the RFD
' 03/08/2011  Roderick Carlson - set default create to be for duplicate, do not copy previous ID if so
' 05/03/2011  Roderick Carlson - New Approval Routing Rules
' 06/13/2011  Roderick Carlson - Prevent Process Approver from being refreshed when save button clicked, allow 
'                                any account manager to update RFQ type approval list instead of just Initiator
' 06/23/2011  Roderick Carlson - Prevent CostSheetID and ECI numbers from being copied
' 07/06/2011  Roderick Carlson - When copying, reset initiator to current team member
' 07/07/2011  Roderick Carlson - Fixed bug when approved RFD was not showing Customer Part Info
' 07/21/2011  Roderick Carlson - adjust height of process capital and tooling text boxes dynamically, perhaps 
'                                later apply this to all multiline text boxes
' 09/28/2011  Roderick Carlson - Oswaldo Amaya - do not let QE approve until either ECI is entered or NA is checked
' 10/20/2011  Roderick Carlson - fixed a spelling error in the email. fixed a bug in the copy approver list
' 11/30/2011  Roderick Carlson - allow DocX and xlsX files to be uploaded, add prompt for copy reason, require 
'                                number of cavities for Prod Dev and Tooling to approve
' 12/01/2011  Roderick Carlson - added Copy Reaosn and Cavity fields
' 01/03/2012  Roderick Carlson - added Email Cleanup function and Cascading Program Dropdowns
' 02/21/2012  Roderick Carlson - select Quality Engineer based on MAKE
' 03/23/2012  Roderick Carlson - Several Changes:
'                                   Remove Continuous Line and Material Size Change checkboxs
'                                   Enable cbQualityEngineeringRequired for Customer Driven Change
'                                   Adjust for Quote Only Business Process
' 04/11/2012  Roderick Carlson - add warning popup to submit button to ask if Initiator has sent all appropriate 
'                                information to Product Development
' 04/12/2012  Roderick Carlson - allow approvals even in open status
' 04/18/2012  Roderick Carlson - change Business Award, Business Process Type, and Business Process Actions
' 04/24/2012  Roderick Carlson - add Program Manager Assignment
' 05/02/2012  Roderick Carlson - make Product Development the only routing level 1
' 05/07/2012  Roderick Carlson - add Purchasing for External RFQ role
' 05/15/2012  Roderick Carlson - add isMeetingRequired checkbox
' 05/16/2012  Roderick Carlson - add Capital and Tooling Lead Time and Units, Supporting Doc updates and rules
' 05/24/2012  Roderick Carlson - add Lead time and units to Child Parts
' 06/05/2012  Roderick Carlson - add External RFQ Template
' 06/11/2012  Roderick Carlson - make QE required for Customer Driven Change
' 06/28/2012  Roderick Carlson - For Quote-Only - make program optional
' 07/20/2012  Roderick Carlson - Dan Cade - some changes for child part do not have cost sheets or external RFQs
' 07/20/2012  Roderick Carlson - Bryan Hall and Jim Reinking - some formulas do not have Part numbers assigned
' 07/26/2012  Roderick Carlson - For Customer Driven Change - add New Customer Part Number to Future Part Number List
' 08/15/2012  Roderick Carlson - Tighter control of Future Customer PartNo rules
' 10/09/2012  Roderick Carlson - Nicolas Leclercq - CC Product Design for Damper Commodities - Chris Sonnek 
'                                and Jennifer Grandon
' 10/16/2012  Roderick Carlson - Cleanup br
' 11/08/2012  Roderick Carlson - Emmanuel Reymond and Bill Schultz - CC Costing when PD approves
' 12/03/2012  Roderick Carlson - Mike Echevarria and Barry Bowhall - pick tooling based on Commodity
' 12/19/2012  Roderick Carlson - RFD-3238 - send notification when closed/voided
' 03/01/2013  Roderick Carlson - RFD-3260 - add new approval status - waiting on cost sheet approval
' 06/25/2013  Roderick Carlson - Make sure the new approval status - waiting on cost sheet approval is not 
'                                overwritten to in-process when checking to InsertUpdateApprovalList
' 09/16/2013  Roderick Carlson - EReymond - include RFD Initiator for Internal UGN Changes when PD approves
' 01/22/2014  LRey             - Replaced "BPCSPart" with "PART" and SoldTo|CABBV with Customer wherever used.
' 06/30/2014  LRey             - Added all team members in the Approval Routing to be carbon copies with the Champion
'                                submits the RFD for approval.
' 07/01/2014  LRey             - Added an "Autopostback" on the New Customer Part - DMS Drawing No field to check if
'                                old to new value. If the RFD is "in process" a series of checks and alerts will occur
' 07/02/2014  LRey             - Added a QuoteOnlySupDocUpdate function added to the btnSaveuploadSupportingDocument
'                                and the btnSaveNetworkFileReference to check for new uploads when an RFD is 
'                                "in process" for approval. A series of checks and alerts will occur. 
' ************************************************************************************************

Partial Class RFD_Detail
    Inherits System.Web.UI.Page

    Private Sub ClearMessages()

        Try

            lblMessage.Text = ""
            lblMessageApproval.Text = ""
            lblMessageApprovalBottom.Text = ""
            lblMessageChildPart.Text = ""
            lblMessageChildPartBottom.Text = ""
            lblMessageChildPartDetails.Text = ""
            lblMessageCommunicationBoard.Text = ""
            lblMessageCustomerPartNo.Text = ""
            lblMessageCustomerPartNoMiddle.Text = ""
            lblMessageCustomerPartNoBottom.Text = ""
            lblMessageCustomerProgram.Text = ""
            lblMessageCustomerProgramBottom.Text = ""
            lblMessageDescription.Text = ""
            lblMessageFacilityDepartment.Text = ""
            lblMessageFG.Text = lblMessage.Text
            lblMessageFGBottom.Text = lblMessage.Text
            lblMessageKIT.Text = ""
            lblMessageSupportingDocs.Text = ""
            lblMessageSupportingDocsBottom.Text = ""
            lblMessageVendor.Text = ""

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Function HandleDrawingPopUps(ByVal DrawingControlID As String) As String

        Try
            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../PE/DrawingLookUp.aspx?DrawingControlID=" & DrawingControlID
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','DrawingPartNos','" & _
                strWindowAttribs & "');return false;"

            HandleDrawingPopUps = strClientScript

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

    End Function

    Protected Function HandleBPCSPopUps(ByVal ccPartNo As String, ByVal ccPartRevision As String, ByVal ccPartDescr As String) As String

        Try
            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
               "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & ccPartNo & "&vcPartRevision=" & ccPartRevision & "&vcPartDescr=" & ccPartDescr
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','DrawingPartNos','" & _
                strWindowAttribs & "');return false;"

            HandleBPCSPopUps = strClientScript

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleBPCSPopUps = ""
        End Try

    End Function

    Protected Function HandleCustomerPartNoPopUps(ByVal CustomerccPartNo As String, ByVal CustomerccPartDescr As String) As String

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
                "../DataMaintenance/CustomerPartNoLookUp.aspx?CustomervcPartNo=" & CustomerccPartNo & "&CustomervcPartDescr=" & CustomerccPartDescr
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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleCustomerPartNoPopUps = ""
        End Try

    End Function
    Protected Function HandleECIPopUps(ByVal ECIControlID As String, ByVal RFDSelectionType As String, ByVal ChildRowID As Integer) As String

        Try
            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "./RFD_To_ECI.aspx?ECINoControlID=" & ECIControlID & "&RFDNo=" & ViewState("RFDNo") & "&RFDSelectionType=" & RFDSelectionType & "&ChildRowID=" & ChildRowID
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','ECINos','" & _
                strWindowAttribs & "');return false;"

            HandleECIPopUps = strClientScript

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleECIPopUps = ""
        End Try

    End Function

    Private Sub InitializeViewState()

        Try

            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            ViewState("AllApproved") = False
            ViewState("ApproverCount") = 0
            ViewState("ApprovalStatusID") = 0

            ViewState("bBusinessAwarded") = False
            ViewState("BusinessProcessActionID") = 0
            ViewState("BusinessProcessTypeID") = 0

            ViewState("CurrentChildDrawingLayoutType") = ""
            ViewState("CurrentChildPartRow") = 0
            ViewState("CurrentCustomerProgramRow") = 0
            ViewState("CurrentCustomerProgramID") = 0

            ViewState("CurrentFGRow") = 0
            ViewState("CurrentRSSID") = 0

            ViewState("DrawingNo") = ""

            ViewState("FGDrawingLayoutType") = ""

            ViewState("OriginalApproverID") = 0
            ViewState("pRC") = 0
            ViewState("RFDNo") = 0
            ViewState("SOPNo") = ""
            ViewState("SOPRev") = 0
            ViewState("StatusID") = 0
            ViewState("SubscriptionID") = 0
            ViewState("TeamMemberID") = 0

            ViewState("InitiatorTeamMemberID") = 0
            ViewState("InitiatorTeamMemberEmail") = ""
            ViewState("InitiatorTeamMemberName") = ""

            ViewState("AccountManagerEmail") = ""
            ViewState("AccountManagerName") = ""

            ViewState("isDefaultCapital") = False
            ViewState("isCapital") = False
            ViewState("CapitalStatusID") = 0
            ViewState("CapitalEmail") = ""
            ViewState("CapitalBackupEmail") = ""
            ViewState("CapitalTeamMemberID") = 0
            ViewState("CapitalTeamMemberName") = ""

            ViewState("isDefaultCosting") = False
            ViewState("isCosting") = False
            ViewState("CostingStatusID") = 0
            ViewState("CostingEmail") = ""
            ViewState("CostingBackupEmail") = ""
            ViewState("CostingTeamMemberID") = 0
            ViewState("CostingTeamMemberName") = ""
            ViewState("AllApprovedBeforeCosting") = False

            ViewState("isDefaultPackaging") = False
            ViewState("isPackaging") = False
            ViewState("PackagingStatusID") = 0
            ViewState("PackagingEmail") = ""
            ViewState("PackagingBackupEmail") = ""
            ViewState("PackagingTeamMemberID") = 0
            ViewState("PackagingTeamMemberName") = ""

            ViewState("isDefaultPlantController") = False
            ViewState("isPlantController") = False
            ViewState("DefaultPlantControllerTeamMemberID") = 0
            ViewState("DefaultPlantControllerEmail") = ""
            ViewState("PlantControllerStatusID") = 0
            ViewState("PlantControllerTeamMemberID") = 0
            ViewState("PlantControllerEmail") = ""
            ViewState("PlantControllerBackupEmail") = ""
            ViewState("PlantControllerTeamMemberName") = ""

            ViewState("isDefaultProcess") = False
            ViewState("isProcess") = False
            ViewState("ProcessStatusID") = 0
            ViewState("ProcessEmail") = ""
            ViewState("ProcessBackupEmail") = ""
            ViewState("ProcessTeamMemberID") = 0
            ViewState("ProcessTeamMemberName") = ""

            ViewState("isDefaultProductDevelopment") = False
            ViewState("isProductDevelopment") = False
            ViewState("DefaultProductDevelopmentTeamMemberID") = 0            
            ViewState("ProductDevelopmentStatusID") = 0
            ViewState("ProductDevelopmentCavityCount") = 0
            ViewState("ProductDevelopmentEmail") = 0
            ViewState("ProductDevelopmentBackupEmail") = ""
            ViewState("ProductDevelopmentTeamMemberID") = 0
            ViewState("ProductDevelopmentTeamMemberName") = ""

            ViewState("isDefaultPurchasing") = False
            ViewState("isPurchasing") = False
            ViewState("DefaultPurchasingTeamMemberID") = 0
            ViewState("PurchasingStatusID") = 0
            ViewState("PurchasingEmail") = ""
            ViewState("PurchasingBackupEmail") = ""
            ViewState("PurchasingTeamMemberID") = 0
            ViewState("PurchasingTeamMemberName") = ""
            ViewState("AllApprovedBeforePurchasing") = False

            ViewState("PurchasingExternalRFQStatusID") = 0
            ViewState("PurchasingExternalRFQEmail") = ""
            ViewState("PurchasingExternalRFQBackupEmail") = ""
            ViewState("PurchasingExternalRFQTeamMemberID") = 0
            ViewState("PurchasingExternalRFQTeamMemberName") = ""

            ViewState("isDefaultQualityEngineer") = False
            ViewState("isQualityEngineer") = False
            ViewState("QualityEngineerStatusID") = 0
            ViewState("QualityEngineerEmail") = ""
            ViewState("QualityEngineerBackupEmail") = ""
            ViewState("QualityEngineerTeamMemberID") = 0
            ViewState("QualityEngineerTeamMemberName") = ""
            ViewState("AllApprovedBeforeQualityEngineer") = False

            ViewState("isDefaultTooling") = False
            ViewState("isTooling") = False
            ViewState("ToolingStatusID") = 0            
            ViewState("ToolingEmail") = ""
            ViewState("ToolingBackupEmail") = ""
            ViewState("ToolingTeamMemberID") = 0
            ViewState("ToolingTeamMemberName") = ""

            ViewState("RndEmail") = ""
            ViewState("RnDTeamMemberName") = ""
            ViewState("RFDccEmailList") = ""

            ViewState("isSales") = False
            ViewState("isProgramManagement") = False

            ViewState("SelectedApproverSubscriptionID") = 0
            ViewState("SelectedApproverTeamMemberID") = 0
            ViewState("SelectedFacility") = ""

            ViewState("DefaultProgramManagementEmail") = ""
            ViewState("ProgramManagerEmail") = ""

            ViewState("CapitalTeamMemberWorking") = False
            ViewState("CostingTeamMemberWorking") = False
            ViewState("PackagingTeamMemberWorking") = False
            ViewState("PlantControllerTeamMemberWorking") = False
            ViewState("ProcessTeamMemberWorking") = False
            ViewState("ProductDevelopmentTeamMemberWorking") = False
            ViewState("PurchasingTeamMemberWorking") = False
            ViewState("QualityEngineerTeamMemberWorking") = False
            ViewState("ToolingTeamMemberWorking") = False

            ViewState("DirectorOfMaterialsEmail") = ""

            ViewState("OrigNewCustomerPartNo") = ""
            ViewState("OrigUGNFacility") = ""
            ViewState("OrigOEMManufacturer") = ""

            ViewState("ProductDesignEmailList") = ""
            ViewState("DMSDrawingNoUpdate") = ""
            ViewState("QuoteOnlySupDocUpdate") = ""

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
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            ViewState("strProdOrTestEnvironment") = strProdOrTestEnvironment

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

            'need to know team members specific subscription/role
            'allow team member to have multiple roles/role
            'determine also if team member has a default subscription/role
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

            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Barry.Barretto", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                '''' TESTING AS Different user
                If iTeamMemberID = 204 Then
                    'iTeamMemberID = 32 'Dan Cade                    
                    'iTeamMemberID = 433 'Derek Ames
                    'iTeamMemberID = 698 'Emmanuel Reymond
                    'iTeamMemberID = 246 'Mike Echevarria
                    'iTeamMemberID = 575 'Rick Matheny
                    'iTeamMemberID = 105 'Ron Davis
                    'iTeamMemberID = 672 ' John Mercado
                    'iTeamMemberID = 476 ' Pranav
                    'iTeamMemberID = 140 ' Bryan Hall                    
                    'iTeamMemberID = 428 'Tracy Theos
                    'iTeamMemberID = 222 'Jim Meade
                    'iTeamMemberID = 2 'Brett Barta
                    'iTeamMemberID = 4 'Kenta Shinohara 
                    'iTeamMemberID = 622 'Mary Cepek 
                    'iTeamMemberID = 44 'Ta-Cheng Shan 
                    'iTeamMemberID = 391 'Ta-Cheng Shan 
                    'iTeamMemberID = 582 ' Bill Schultz                    
                    'iTeamMemberID = 464 'Ryan.Bentley                     
                    'iTeamMemberID = 48 ' Barry.Bowhall   
                    'iTeamMemberID = 819 'David.Kanofsky 
                End If

                ViewState("TeamMemberID") = iTeamMemberID

                'Account Manager
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 9)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 9
                    ViewState("isSales") = True
                End If

                'Program Management
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 31)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 31
                    ViewState("isProgramManagement") = True
                End If

                'RFD Plant Champion
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 4)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 4
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

                ''Default Packaging
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 110)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultPackaging") = True
                End If

                ''Default PlantController
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 109)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultPlantController") = True
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
                            ViewState("isAdmin") = True
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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvQuestion.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim RSSID As Integer

                Dim drRSSID As RFD.RFDRSS_MaintRow = CType(CType(e.Row.DataItem, DataRowView).Row, RFD.RFDRSS_MaintRow)

                If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                    RSSID = drRSSID.RSSID
                    ' Reference the rpCBRC ObjectDataSource
                    Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                    ' Set the Parameter value
                    rpCBRC.SelectParameters("RFDNo").DefaultValue = drRSSID.RFDNo.ToString
                    rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Request For Development (RFD) Detail"

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("RFDExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            'used to help with uploading files
            Me.Form.Enctype = "multipart/form-data"

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub ResetCurrentApprovalUpdateSection()

        Try
            ViewState("OriginalApproverID") = 0

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Function GetCurrentFGDrawing(ByVal DrawingNo As String) As Boolean

        'get information from DMS Drawing
        'do not override existing information except current measurements
        'do not pull FG PartNos from DMS. They should be pulled from Wizard
        'Customer PartNo, Customer DrawingNo, Current and New Commodity, Current and New Product Technology, and
        'Measurements (overritable on current): Current AMD, Current WMD, Current Construction, Current Drawing Notes

        Dim bResult As Boolean = False

        Try
            Dim dsDrawing As DataSet
            Dim dsDrawingCustomerImage As DataSet

            Dim iFirstDashLocation As Integer = 0

            iBtnCurrentDrawingCopy.Visible = False
            hlnkCurrentCustomerDrawingNo.NavigateUrl = ""
            hlnkCurrentCustomerDrawingNo.Visible = False

            hlnkCurrentDrawingNo.NavigateUrl = ""
            hlnkCurrentDrawingNo.Visible = False

            ViewState("FGDrawingLayoutType") = ""

            ddCurrentCommodity.SelectedIndex = -1
            ddCurrentProductTechnology.SelectedIndex = -1

            txtCurrentFGInitialDimensionAndDensity.Text = ""
            txtCurrentFGInStepTracking.Text = ""

            txtCurrentFGAMDValue.Text = ""
            txtCurrentFGAMDTolerance.Text = ""
            ddCurrentFGAMDUnits.SelectedIndex = -1

            txtCurrentFGWMDValue.Text = ""
            txtCurrentFGWMDTolerance.Text = ""
            ddCurrentFGWMDUnits.SelectedIndex = -1

            txtCurrentFGDensityValue.Text = ""
            txtCurrentFGDensityTolerance.Text = ""
            txtCurrentFGDensityUnits.Text = ""

            txtCurrentFGConstruction.Text = ""
            txtCurrentFGDrawingNotes.Text = ""

            ddCurrentFGSubFamily.SelectedIndex = -1

            If DrawingNo <> "" Then            
                dsDrawing = PEModule.GetDrawing(DrawingNo)

                If commonFunctions.CheckDataSet(dsDrawing) = True Then
                    hlnkCurrentDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
                    hlnkCurrentDrawingNo.Visible = True

                    If txtCurrentCustomerPartNo.Text.Trim = "" Then
                        txtCurrentCustomerPartNo.Text = dsDrawing.Tables(0).Rows(0).Item("CustomerPartNo").ToString
                    End If

                    dsDrawingCustomerImage = PEModule.GetDrawingCustomerImages(DrawingNo)

                    If commonFunctions.CheckDataSet(dsDrawingCustomerImage) = True Then
                        If txtCurrentCustomerDrawingNo.Text.Trim = "" Then
                            txtCurrentCustomerDrawingNo.Text = dsDrawingCustomerImage.Tables(0).Rows(0).Item("CustomerDrawingNo").ToString.Trim
                        End If

                        hlnkCurrentCustomerDrawingNo.NavigateUrl = "~/PE/DrawingCustomerImageView.aspx?DrawingNo=" & DrawingNo
                        hlnkCurrentCustomerDrawingNo.Visible = True
                    End If

                    ViewState("FGDrawingLayoutType") = dsDrawing.Tables(0).Rows(0).Item("DrawingLayoutType").ToString

                    If dsDrawing.Tables(0).Rows(0).Item("CommodityID") IsNot System.DBNull.Value Then
                        If dsDrawing.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                            ddCurrentCommodity.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("CommodityID")
                        End If
                    End If


                    If dsDrawing.Tables(0).Rows(0).Item("ProductTechnologyID") IsNot System.DBNull.Value Then
                        If dsDrawing.Tables(0).Rows(0).Item("ProductTechnologyID") > 0 Then
                            ddCurrentProductTechnology.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("ProductTechnologyID")
                        End If
                    End If

                    iFirstDashLocation = InStr(DrawingNo, "-")
                    txtCurrentFGInitialDimensionAndDensity.Text = Mid$(DrawingNo, iFirstDashLocation + 1, 2)

                    If txtNewFGInitialDimensionAndDensity.Text.Trim = "" Then
                        txtNewFGInitialDimensionAndDensity.Text = Mid$(DrawingNo, iFirstDashLocation + 1, 2)
                    End If

                    If dsDrawing.Tables(0).Rows(0).Item("InStepTracking") IsNot System.DBNull.Value Then
                        If dsDrawing.Tables(0).Rows(0).Item("InStepTracking") > 0 Then
                            txtCurrentFGInStepTracking.Text = dsDrawing.Tables(0).Rows(0).Item("InStepTracking")
                        End If
                    End If

                    If txtNewFGInStepTracking.Text.Trim = "" Then
                        txtNewFGInStepTracking.Text = txtCurrentFGInStepTracking.Text.Trim
                    End If

                    If dsDrawing.Tables(0).Rows(0).Item("AMDvalue") IsNot System.DBNull.Value Then
                        If dsDrawing.Tables(0).Rows(0).Item("AMDvalue") > 0 Then
                            txtCurrentFGAMDValue.Text = dsDrawing.Tables(0).Rows(0).Item("AMDvalue")
                        End If
                    End If

                    txtCurrentFGAMDTolerance.Text = dsDrawing.Tables(0).Rows(0).Item("AMDTolerance").ToString.Trim

                    If dsDrawing.Tables(0).Rows(0).Item("AMDUnits") IsNot System.DBNull.Value Then
                        ddCurrentFGAMDUnits.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("AMDUnits")
                    End If

                    If dsDrawing.Tables(0).Rows(0).Item("WMDvalue") IsNot System.DBNull.Value Then
                        If dsDrawing.Tables(0).Rows(0).Item("WMDvalue") > 0 Then
                            txtCurrentFGWMDValue.Text = dsDrawing.Tables(0).Rows(0).Item("WMDvalue")
                        End If
                    End If

                    txtCurrentFGWMDTolerance.Text = dsDrawing.Tables(0).Rows(0).Item("WMDTolerance").ToString.Trim

                    If dsDrawing.Tables(0).Rows(0).Item("WMDUnits") IsNot System.DBNull.Value Then
                        ddCurrentFGWMDUnits.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("WMDUnits")
                    End If

                    If dsDrawing.Tables(0).Rows(0).Item("DensityValue") IsNot System.DBNull.Value Then
                        If dsDrawing.Tables(0).Rows(0).Item("DensityValue") > 0 Then
                            txtCurrentFGDensityValue.Text = dsDrawing.Tables(0).Rows(0).Item("DensityValue")
                        End If
                    End If

                    txtCurrentFGDensityTolerance.Text = dsDrawing.Tables(0).Rows(0).Item("DensityTolerance").ToString.Trim
                    txtCurrentFGDensityUnits.Text = dsDrawing.Tables(0).Rows(0).Item("DensityUnits").ToString.Trim
                    txtCurrentFGConstruction.Text = dsDrawing.Tables(0).Rows(0).Item("Construction").ToString.Trim
                    txtCurrentFGDrawingNotes.Text = dsDrawing.Tables(0).Rows(0).Item("Notes").ToString.Trim

                    If dsDrawing.Tables(0).Rows(0).Item("SubFamilyID") IsNot System.DBNull.Value Then
                        If dsDrawing.Tables(0).Rows(0).Item("SubFamilyID") > 0 Then
                            ddCurrentFGSubFamily.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("SubFamilyID")

                            'get left 2 digits of subfamily
                            Dim strFamilyID As String = Left(CType(ddCurrentFGSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                            If strFamilyID <> "" Then
                                ddCurrentFGFamily.SelectedValue = CType(strFamilyID, Integer)
                            End If
                        End If
                    End If

                    If ddNewFGSubFamily.SelectedIndex <= 0 And ddCurrentFGSubFamily.SelectedIndex > 0 Then
                        BindFamilySubFamily()

                        'get left 2 digits of subfamily
                        Dim strFamilyID As String = Left(CType(ddCurrentFGSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                        If strFamilyID <> "" Then
                            ddNewFGFamily.SelectedValue = CType(strFamilyID, Integer)
                        End If

                        ddNewFGSubFamily.SelectedValue = ddCurrentFGSubFamily.SelectedValue
                    End If

                    bResult = True
                Else
                    If InStr(lblMessage.Text, "The current DMS DrawingNo does not exist.") <= 0 Then
                        lblMessage.Text &= "<br />The current DMS DrawingNo does not exist."
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

        Return bResult

    End Function

    Protected Function GetNewFGDrawing(ByVal DrawingNo As String) As Boolean

        'get information from DMS Drawing
        'do not override existing information
        'do not pull FG PartNos from DMS. They should be pulled from Wizard
        'Get Customer DrawingNo, New Commodity,New Product Technology, and
        'Measurements: Current AMD, Current WMD, Current Construction, Current Drawing Notes

        Dim bResult As Boolean = False

        Try
            Dim dsDrawing As DataSet
            Dim dsDrawingCustomerImage As DataSet


            Dim iFirstDashLocation As Integer = 0

            iBtnNewDrawingCopy.Visible = False
            hlnkNewCustomerDrawingNo.NavigateUrl = ""
            hlnkNewCustomerDrawingNo.Visible = False

            hlnkNewDrawingNo.NavigateUrl = ""
            hlnkNewDrawingNo.Visible = False

          
            If DrawingNo <> "" Then

                dsDrawing = PEModule.GetDrawing(DrawingNo)

                If commonFunctions.CheckDataSet(dsDrawing) = True Then

                    hlnkNewDrawingNo.Visible = True
                    hlnkNewDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo

                    dsDrawingCustomerImage = PEModule.GetDrawingCustomerImages(DrawingNo)

                    If commonFunctions.CheckDataSet(dsDrawingCustomerImage) = True Then
                        If txtNewCustomerDrawingNo.Text.Trim = "" And ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 Then
                            txtNewCustomerDrawingNo.Text = dsDrawingCustomerImage.Tables(0).Rows(0).Item("CustomerDrawingNo").ToString.Trim
                        End If

                        hlnkNewCustomerDrawingNo.NavigateUrl = "~/PE/DrawingCustomerImageView.aspx?DrawingNo=" & DrawingNo
                        hlnkNewCustomerDrawingNo.Visible = True

                    End If

                    If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 And ViewState("StatusID") <> 8 Then
                        If dsDrawing.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                            ddNewCommodity.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("CommodityID")
                            ddWorkFlowCommodity.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("CommodityID")
                        End If
                        If dsDrawing.Tables(0).Rows(0).Item("ProductTechnologyID") > 0 Then
                            ddNewProductTechnology.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("ProductTechnologyID")
                        End If
                        iFirstDashLocation = InStr(DrawingNo, "-")
                        txtNewFGInitialDimensionAndDensity.Text = Mid$(DrawingNo, iFirstDashLocation + 1, 2)

                        txtNewFGInStepTracking.Text = dsDrawing.Tables(0).Rows(0).Item("InStepTracking")

                        txtNewFGAMDValue.Text = dsDrawing.Tables(0).Rows(0).Item("AMDvalue")


                        txtNewFGAMDTolerance.Text = dsDrawing.Tables(0).Rows(0).Item("AMDTolerance").ToString.Trim

                        If dsDrawing.Tables(0).Rows(0).Item("AMDUnits") IsNot System.DBNull.Value Then
                            ddNewFGAMDUnits.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("AMDUnits")
                        End If

                        If dsDrawing.Tables(0).Rows(0).Item("WMDvalue") IsNot System.DBNull.Value Then
                            If dsDrawing.Tables(0).Rows(0).Item("WMDvalue") > 0 Then
                                txtNewFGWMDValue.Text = dsDrawing.Tables(0).Rows(0).Item("WMDvalue")
                            End If
                        End If

                        txtNewFGWMDTolerance.Text = dsDrawing.Tables(0).Rows(0).Item("WMDTolerance").ToString.Trim

                        If dsDrawing.Tables(0).Rows(0).Item("WMDUnits") IsNot System.DBNull.Value Then
                            ddNewFGWMDUnits.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("WMDUnits")
                        End If

                        If dsDrawing.Tables(0).Rows(0).Item("DensityValue") IsNot System.DBNull.Value Then
                            If dsDrawing.Tables(0).Rows(0).Item("DensityValue") > 0 Then
                                txtNewFGDensityValue.Text = dsDrawing.Tables(0).Rows(0).Item("DensityValue")
                            End If
                        End If

                        txtNewFGDensityTolerance.Text = dsDrawing.Tables(0).Rows(0).Item("DensityTolerance").ToString.Trim
                        txtNewFGDensityUnits.Text = dsDrawing.Tables(0).Rows(0).Item("DensityUnits").ToString.Trim
                        txtNewFGConstruction.Text = dsDrawing.Tables(0).Rows(0).Item("Construction").ToString.Trim
                        txtNewFGDrawingNotes.Text = dsDrawing.Tables(0).Rows(0).Item("Notes").ToString.Trim

                        If dsDrawing.Tables(0).Rows(0).Item("SubFamilyID") IsNot System.DBNull.Value Then
                            If dsDrawing.Tables(0).Rows(0).Item("SubFamilyID") > 0 Then
                                ddNewFGSubFamily.SelectedValue = dsDrawing.Tables(0).Rows(0).Item("SubFamilyID")

                                'get left 2 digits of subfamily
                                Dim strFamilyID As String = Left(CType(ddNewFGSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                                If strFamilyID <> "" Then
                                    ddNewFGFamily.SelectedValue = CType(strFamilyID, Integer)
                                End If
                            End If
                        End If

                    End If

                    bResult = True
                Else
                    If InStr(lblMessage.Text, "The new DMS DrawingNo does not exist.") <= 0 Then
                        lblMessage.Text &= "<br />The new DMS DrawingNo does not exist."
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

        Return bResult

    End Function

    Private Sub BindApprovalSubscriptionByTeamMember(ByVal ApprovalTeamMemberID As Integer)

        Try

            Dim dsSubscription As DataSet

            Dim iTempSubscriptionID As Integer = ViewState("SubscriptionID")

            'available subscriptions for team member
            'allow UGN Admin/Assist to approve for anyone
            If ViewState("isAdmin") = True Then
                ApprovalTeamMemberID = 0
            End If

            dsSubscription = RFDModule.GetRFDSubscriptionByApprover(ApprovalTeamMemberID)
            If commonFunctions.CheckDataSet(dsSubscription) = True Then
                ddApprovalSubscription.DataSource = dsSubscription
                ddApprovalSubscription.DataTextField = dsSubscription.Tables(0).Columns("Subscription").ColumnName
                ddApprovalSubscription.DataValueField = dsSubscription.Tables(0).Columns("SubscriptionID").ColumnName
                ddApprovalSubscription.DataBind()

                If ViewState("isDefaultCosting") = True Then
                    ddApprovalSubscription.SelectedValue = 6
                End If

                If ViewState("isDefaultPackaging") = True Then
                    ddApprovalSubscription.SelectedValue = 108
                End If

                If ViewState("isDefaultPlantController") = True Then
                    ddApprovalSubscription.SelectedValue = 20
                End If

                If ViewState("isDefaultProcess") = True Then
                    ddApprovalSubscription.SelectedValue = 66
                End If

                If ViewState("isDefaultCapital") = True Then
                    ddApprovalSubscription.SelectedValue = 119
                End If

                If ViewState("isDefaultProductDevelopment") = True Then
                    ddApprovalSubscription.SelectedValue = 5
                End If

                If ViewState("isDefaultPurchasing") = True Then
                    ddApprovalSubscription.SelectedValue = 7
                End If

                If ViewState("isDefaultQualityEngineer") = True Then
                    ddApprovalSubscription.SelectedValue = 22
                End If

                If ViewState("isDefaultTooling") = True Then
                    ddApprovalSubscription.SelectedValue = 65
                End If

                If ddApprovalSubscription.SelectedIndex >= 0 Then
                    iTempSubscriptionID = ddApprovalSubscription.SelectedValue
                End If

                EnableApprovalControls(iTempSubscriptionID)

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

    Private Sub EnableApprovalControls(ByVal SubscriptionID As Integer)

        Try
            Dim dsCurrentApprover As DataSet

            lblApprovalNumberOfCavitiesMarker.Visible = False
            lblApprovalNumberOfCavitiesLabel.Visible = False
            txtApprovalNumberOfCavities.Visible = False
            txtApprovalNumberOfCavities.Enabled = False
     
            If SubscriptionID > 0 Then
                tblCurrentApprover.Visible = ViewState("isEdit")

                txtApprovalComments.Text = ""
                txtApprovalComments.Enabled = False

                btnApprovalStatusReset.Visible = False
                btnApprovalStatusSubmit.Visible = False

                ddApprovalStatus.Enabled = False
                ddApprovalStatus.Visible = False

                dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), SubscriptionID, 0, False, False, False, False, False)

                If commonFunctions.CheckDataSet(dsCurrentApprover) = True Then

                    lblApprovalTeamMember.Text = dsCurrentApprover.Tables(0).Rows(0).Item("FullTeamMemberName").ToString

                    'do not override another approver of same subscription
                    If dsCurrentApprover.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                        If dsCurrentApprover.Tables(0).Rows(0).Item("TeamMemberID") > 0 Then                         
                            If dsCurrentApprover.Tables(0).Rows(0).Item("TeamMemberID") = ViewState("TeamMemberID") Then
                                txtApprovalComments.Text = dsCurrentApprover.Tables(0).Rows(0).Item("Comments").ToString
                            Else
                                ViewState("OriginalApproverID") = dsCurrentApprover.Tables(0).Rows(0).Item("TeamMemberID")
                            End If
                        End If
                    End If

                    ViewState("ApprovalStatusID") = 0
                    If dsCurrentApprover.Tables(0).Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                        If dsCurrentApprover.Tables(0).Rows(0).Item("StatusID") > 0 Then
                            ddApprovalStatus.SelectedValue = dsCurrentApprover.Tables(0).Rows(0).Item("StatusID")
                            ViewState("ApprovalStatusID") = dsCurrentApprover.Tables(0).Rows(0).Item("StatusID")
                        End If
                    End If

                    Select Case CType(ViewState("ApprovalStatusID"), Integer)
                        Case 1, 2, 6, 7, 9

                            AdjustApprovalStatusControl()

                            btnApprovalStatusReset.Visible = ViewState("isEdit")
                            btnApprovalStatusSubmit.Visible = ViewState("isEdit")

                            ddApprovalStatus.Enabled = ViewState("isEdit")
                            txtApprovalComments.Text = ""
                            txtApprovalComments.Enabled = ViewState("isEdit")

                            If SubscriptionID = 5 Or SubscriptionID = 65 Then
                                lblApprovalNumberOfCavitiesMarker.Visible = ViewState("isEdit")
                                lblApprovalNumberOfCavitiesLabel.Visible = ViewState("isEdit")
                                txtApprovalNumberOfCavities.Visible = ViewState("isEdit")
                                txtApprovalNumberOfCavities.Enabled = ViewState("isEdit")
                            Else
                                txtApprovalNumberOfCavities.Text = ""
                            End If
                    End Select

                    ddApprovalStatus.Visible = True

                Else 'no details for this subscription
                    If ViewState("isAdmin") = True Then
                        tblCurrentApprover.Visible = True
                        lblApprovalTeamMember.Text = "Approval is NOT required for this role. Please select another role."
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


    Protected Sub BindData()

        Try

            Dim dsRFD As DataSet
            Dim dsRFDCapital As DataSet
            Dim dsRFDProcess As DataSet
            Dim dsRFDTooling As DataSet

            Dim iHeigthBySpecificCharCount As Integer = 0
            Dim iHeightByTextFieldLength As Integer = 0
            Dim iHeightToUse As Integer = 200

            ViewState("FGDrawingLayoutType") = ""
            ViewState("bBusinessAwarded") = False
            ViewState("SOPNo") = ""
            ViewState("SOPRev") = 0

            ViewState("BusinessProcessTypeID") = 0
            ViewState("BusinessProcessActionID") = 0

            dsRFD = RFDModule.GetRFD(ViewState("RFDNo"))

            If commonFunctions.CheckDataset(dsRFD) = True Then

                cbAffectsCostSheetOnly.Checked = dsRFD.Tables(0).Rows(0).Item("isAffectsCostSheetOnly")
                cbCapitalRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isCapitalRequired")
                ddisCostReduction.SelectedValue = dsRFD.Tables(0).Rows(0).Item("isCostReduction")
                cbCostingRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isCostingRequired")
                cbDVPRrequired.Checked = dsRFD.Tables(0).Rows(0).Item("isDVPRrequired")
                cbPlantControllerRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isPlantControllerRequired")
                cbPPAP.Checked = dsRFD.Tables(0).Rows(0).Item("isPPAP")
                cbPackagingRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isPackagingRequired")
                cbProcessRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isProcessRequired")
                cbProductDevelopmentRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isProductDevelopmentRequired")
                cbPurchasingExternalRFQRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isPurchasingExternalRFQRequired")
                cbPurchasingRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isPurchasingRequired")
                cbQualityEngineeringRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isQualityEngineeringRequired")
                cbRDrequired.Checked = dsRFD.Tables(0).Rows(0).Item("isRDRequired")
                cbToolingRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isToolingRequired")
                cbMeetingRequired.Checked = dsRFD.Tables(0).Rows(0).Item("isMeetingRequired")

                'reset approval update section
                ResetCurrentApprovalUpdateSection()

                If ViewState("isEdit") = True Then
                    BindApprovalSubscriptionByTeamMember(ViewState("TeamMemberID"))
                End If

                If dsRFD.Tables(0).Rows(0).Item("AccountManagerID") > 0 Then
                    ddAccountManager.SelectedValue = dsRFD.Tables(0).Rows(0).Item("AccountManagerID")
                End If

                If dsRFD.Tables(0).Rows(0).Item("ProgramManagerID") > 0 Then
                    ddProgramManager.SelectedValue = dsRFD.Tables(0).Rows(0).Item("ProgramManagerID")
                End If

                If dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID") > 0 Then
                    ddBusinessProcessType.SelectedValue = dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID")
                    ViewState("BusinessProcessTypeID") = dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID")

                    'must refresh business process actions AFTER business process type is found.
                    CheckBusinessProcessAction()

                End If

                If dsRFD.Tables(0).Rows(0).Item("BusinessProcessActionID") > 0 Then
                    ddBusinessProcessAction.SelectedValue = dsRFD.Tables(0).Rows(0).Item("BusinessProcessActionID")
                    ViewState("BusinessProcessActionID") = dsRFD.Tables(0).Rows(0).Item("BusinessProcessActionID")

                    'if anything but source quote then assume business is awarded
                    If ViewState("BusinessProcessActionID") <> 10 Then
                        ViewState("bBusinessAwarded") = True
                    End If
                End If

                ddDesignationType.SelectedValue = dsRFD.Tables(0).Rows(0).Item("DesignationType")

                If dsRFD.Tables(0).Rows(0).Item("Make").ToString <> "" Then
                    ddWorkFlowMake.SelectedValue = dsRFD.Tables(0).Rows(0).Item("Make").ToString
                    FilterPurchasingMakeList(dsRFD.Tables(0).Rows(0).Item("Make").ToString)
                Else
                    FilterPurchasingMakeList("")
                End If

                'this needs to be assigned after make is assigned
                If dsRFD.Tables(0).Rows(0).Item("PurchasingMakeTeamMemberID") > 0 Then
                    ddPurchasingTeamMemberByMake.SelectedValue = dsRFD.Tables(0).Rows(0).Item("PurchasingMakeTeamMemberID")
                End If

                FilterPurchasingFamilyList(0)
                If dsRFD.Tables(0).Rows(0).Item("FamilyID") > 0 Then
                    ddWorkflowFamily.SelectedValue = dsRFD.Tables(0).Rows(0).Item("FamilyID")
                    FilterPurchasingFamilyList(dsRFD.Tables(0).Rows(0).Item("FamilyID"))
                End If

                If dsRFD.Tables(0).Rows(0).Item("PurchasingFamilyTeamMemberID") > 0 Then
                    ddPurchasingTeamMemberByFamily.SelectedValue = dsRFD.Tables(0).Rows(0).Item("PurchasingFamilyTeamMemberID")
                End If

                FilterProductDevelopmentCommodityList(0)
                ddWorkFlowCommodity.SelectedIndex = -1
                If dsRFD.Tables(0).Rows(0).Item("NewCommodityID") > 0 Then
                    ddWorkFlowCommodity.SelectedValue = dsRFD.Tables(0).Rows(0).Item("NewCommodityID")
                    ddNewCommodity.SelectedValue = dsRFD.Tables(0).Rows(0).Item("NewCommodityID")
                    FilterProductDevelopmentCommodityList(dsRFD.Tables(0).Rows(0).Item("NewCommodityID"))
                End If

                'this needs to be assigned after commodity is assiged
                If dsRFD.Tables(0).Rows(0).Item("ProductDevelopmentCommodityTeamMemberID") > 0 Then
                    ddProductDevelopmentTeamMemberByCommodity.SelectedValue = dsRFD.Tables(0).Rows(0).Item("ProductDevelopmentCommodityTeamMemberID")
                End If

                If dsRFD.Tables(0).Rows(0).Item("NewProductTechnologyID") > 0 Then
                    ddNewProductTechnology.SelectedValue = dsRFD.Tables(0).Rows(0).Item("NewProductTechnologyID")
                End If

                ddPriceCode.SelectedValue = dsRFD.Tables(0).Rows(0).Item("PriceCode")

                If dsRFD.Tables(0).Rows(0).Item("PriorityID") > 0 Then
                    ddPriority.SelectedValue = dsRFD.Tables(0).Rows(0).Item("PriorityID")
                End If

                If dsRFD.Tables(0).Rows(0).Item("InitiatorTeamMemberID") > 0 Then
                    ddInitiator.SelectedValue = dsRFD.Tables(0).Rows(0).Item("InitiatorTeamMemberID")
                    ViewState("InitiatorTeamMemberID") = dsRFD.Tables(0).Rows(0).Item("InitiatorTeamMemberID")
                End If

                ViewState("StatusID") = 1
                ddStatus.SelectedValue = 1
                If dsRFD.Tables(0).Rows(0).Item("StatusID") > 0 Then
                    ddStatus.SelectedValue = dsRFD.Tables(0).Rows(0).Item("StatusID")
                    ViewState("StatusID") = dsRFD.Tables(0).Rows(0).Item("StatusID")
                End If

                If dsRFD.Tables(0).Rows(0).Item("PreviousRFDNo") > 0 Then
                    hlnkPreviousRFDNo.Text = dsRFD.Tables(0).Rows(0).Item("PreviousRFDNo")
                    hlnkPreviousRFDNo.Visible = True
                    lblPreviousRFDNo.Visible = True
                End If

                If dsRFD.Tables(0).Rows(0).Item("BusinessAwardDate").ToString <> "" Then
                    lblBusinessAwardedDateValue.Text = dsRFD.Tables(0).Rows(0).Item("BusinessAwardDate").ToString
                    lblBusinessAwaredDateLabel.Visible = True
                    lblBusinessAwardedDateValue.Visible = True
                    ViewState("bBusinessAwarded") = True
                End If

                lblCompletionDateLabel.Visible = False
                lblCompletionDateValue.Visible = False
                If dsRFD.Tables(0).Rows(0).Item("CompletionDate").ToString <> "" Then
                    lblCompletionDateValue.Text = dsRFD.Tables(0).Rows(0).Item("CompletionDate").ToString
                    lblCompletionDateLabel.Visible = True
                    lblCompletionDateValue.Visible = True
                End If

                lblCreatedOnDateValue.Text = dsRFD.Tables(0).Rows(0).Item("CreatedOn").ToString

                txtCopyReason.Text = dsRFD.Tables(0).Rows(0).Item("CopyReason").ToString.Trim
                txtCurrentCustomerDrawingNo.Text = dsRFD.Tables(0).Rows(0).Item("CurrentCustomerDrawingNo").ToString.Trim
                txtCurrentCustomerPartName.Text = dsRFD.Tables(0).Rows(0).Item("CurrentCustomerPartName").ToString.Trim
                txtCurrentCustomerPartNo.Text = dsRFD.Tables(0).Rows(0).Item("CurrentCustomerPartNo").ToString.Trim
                txtCurrentDesignLevel.Text = dsRFD.Tables(0).Rows(0).Item("CurrentDesignLevel").ToString.Trim
                txtCurrentDrawingNo.Text = dsRFD.Tables(0).Rows(0).Item("CurrentDrawingNo").ToString.Trim
                txtDueDate.Text = dsRFD.Tables(0).Rows(0).Item("DueDate").ToString.Trim
                txtImpactOnUGN.Text = dsRFD.Tables(0).Rows(0).Item("ImpactOnUGN").ToString.Trim
                iHeightToUse = 100
                If txtImpactOnUGN.Text.Trim <> "" And txtImpactOnUGN.Text.Trim.Length <> 0 Then

                    iHeigthBySpecificCharCount = 0
                    iHeightByTextFieldLength = 0

                    'count all characters
                    iHeigthBySpecificCharCount = (txtImpactOnUGN.Text.Trim.Length / 80) * 20
                    'count the number of carriage return line feeds
                    iHeightByTextFieldLength = (UBound(Split(txtImpactOnUGN.Text, vbCrLf)) * 40)

                    'if calculated heights are greater than 200 use the greater of the 2
                    If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                        If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                            iHeightToUse = iHeigthBySpecificCharCount
                        Else
                            iHeightToUse = iHeightByTextFieldLength
                        End If

                    End If
                End If
                txtImpactOnUGN.Height = iHeightToUse

                txtNewCapExProjectNo.Text = dsRFD.Tables(0).Rows(0).Item("CapExProjectNo").ToString.Trim

                If dsRFD.Tables(0).Rows(0).Item("CostSheetID") > 0 Then
                    txtNewCostSheetID.Text = dsRFD.Tables(0).Rows(0).Item("CostSheetID").ToString.Trim
                End If

                txtNewCustomerDrawingNo.Text = dsRFD.Tables(0).Rows(0).Item("NewCustomerDrawingNo").ToString.Trim
                txtNewCustomerPartName.Text = dsRFD.Tables(0).Rows(0).Item("NewCustomerPartName").ToString.Trim

                txtNewCustomerPartNo.Text = dsRFD.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString.Trim
                ViewState("OrigNewCustomerPartNo") = dsRFD.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString.Trim

                txtNewDesignLevel.Text = dsRFD.Tables(0).Rows(0).Item("NewDesignLevel").ToString.Trim
                txtNewDrawingNo.Text = dsRFD.Tables(0).Rows(0).Item("NewDrawingNo").ToString.Trim
                txtHDNewDrawingNo.Text = dsRFD.Tables(0).Rows(0).Item("NewDrawingNo").ToString.Trim

                cbNewECIOverrideNA.Checked = False
                cbNewECIOverrideNA.Checked = Not dsRFD.Tables(0).Rows(0).Item("isECIRequired")

                If dsRFD.Tables(0).Rows(0).Item("ECINo") > 0 Then
                    txtNewECINo.Text = dsRFD.Tables(0).Rows(0).Item("ECINo").ToString.Trim
                    cbNewECIOverrideNA.Checked = False
                End If

                txtNewPONo.Text = dsRFD.Tables(0).Rows(0).Item("PurchasingPONo").ToString.Trim

                If dsRFD.Tables(0).Rows(0).Item("TargetAnnualSales") > 0 Then
                    txtTargetAnnualSales.Text = Format(dsRFD.Tables(0).Rows(0).Item("TargetAnnualSales"), "#,##0.00")
                End If

                If dsRFD.Tables(0).Rows(0).Item("TargetAnnualVolume") > 0 Then
                    txtTargetAnnualVolume.Text = dsRFD.Tables(0).Rows(0).Item("TargetAnnualVolume")
                End If

                If dsRFD.Tables(0).Rows(0).Item("TargetPrice") > 0 Then
                    txtTargetPrice.Text = Format(dsRFD.Tables(0).Rows(0).Item("TargetPrice"), "#,##0.00")
                End If

                txtRFDDesc.Text = dsRFD.Tables(0).Rows(0).Item("RFDDesc").ToString.Trim
                iHeightToUse = 100
                If txtRFDDesc.Text.Trim <> "" And txtRFDDesc.Text.Trim.Length <> 0 Then

                    iHeigthBySpecificCharCount = 0
                    iHeightByTextFieldLength = 0

                    'count all characters
                    iHeigthBySpecificCharCount = (txtRFDDesc.Text.Trim.Length / 80) * 20
                    'count the number of carriage return line feeds
                    iHeightByTextFieldLength = (UBound(Split(txtRFDDesc.Text, vbCrLf)) * 40)

                    'if calculated heights are greater than 200 use the greater of the 2
                    If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                        If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                            iHeightToUse = iHeigthBySpecificCharCount
                        Else
                            iHeightToUse = iHeightByTextFieldLength
                        End If

                    End If
                End If
                txtRFDDesc.Height = iHeightToUse

                txtVendorRequirement.Text = dsRFD.Tables(0).Rows(0).Item("VendorRequirement").ToString.Trim
                txtVoidComment.Text = dsRFD.Tables(0).Rows(0).Item("VoidComment").ToString.Trim
                txtCloseComment.Text = dsRFD.Tables(0).Rows(0).Item("CloseComment").ToString.Trim

                If dsRFD.Tables(0).Rows(0).Item("NewAMDValue") > 0 Then
                    txtNewFGAMDValue.Text = dsRFD.Tables(0).Rows(0).Item("NewAMDValue")
                End If

                txtNewFGAMDTolerance.Text = dsRFD.Tables(0).Rows(0).Item("NewAMDTolerance").ToString

                ddNewFGAMDUnits.SelectedValue = dsRFD.Tables(0).Rows(0).Item("NewAMDUnits").ToString

                If dsRFD.Tables(0).Rows(0).Item("NewWMDValue") > 0 Then
                    txtNewFGWMDValue.Text = dsRFD.Tables(0).Rows(0).Item("NewWMDValue")
                End If

                txtNewFGWMDTolerance.Text = dsRFD.Tables(0).Rows(0).Item("NewWMDTolerance").ToString

                ddNewFGWMDUnits.SelectedValue = dsRFD.Tables(0).Rows(0).Item("NewWMDUnits").ToString

                If dsRFD.Tables(0).Rows(0).Item("NewDensityValue") > 0 Then
                    txtNewFGDensityValue.Text = dsRFD.Tables(0).Rows(0).Item("NewDensityValue")
                End If

                txtNewFGDensityTolerance.Text = dsRFD.Tables(0).Rows(0).Item("NewDensityTolerance").ToString

                txtNewFGDensityUnits.Text = dsRFD.Tables(0).Rows(0).Item("NewDensityUnits").ToString

                txtNewFGConstruction.Text = dsRFD.Tables(0).Rows(0).Item("NewConstruction").ToString

                txtNewFGDrawingNotes.Text = dsRFD.Tables(0).Rows(0).Item("NewDrawingNotes").ToString

                ddNewFGSubFamily.SelectedValue = 3401
                If dsRFD.Tables(0).Rows(0).Item("NewSubFamilyID") > 0 Then
                    ddNewFGSubFamily.SelectedValue = dsRFD.Tables(0).Rows(0).Item("NewSubFamilyID")

                    'get left 2 digits of subfamily
                    Dim strFamilyID As String = Left(CType(ddNewFGSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                    If strFamilyID <> "" Then
                        ddNewFGFamily.SelectedValue = CType(strFamilyID, Integer)
                    End If
                End If

                ViewState("SOPNo") = dsRFD.Tables(0).Rows(0).Item("SOPNo").ToString

                If dsRFD.Tables(0).Rows(0).Item("SOPRev") > 0 Then
                    ViewState("SOPRev") = dsRFD.Tables(0).Rows(0).Item("SOPRev")
                End If

                dsRFDProcess = RFDModule.GetRFDProcess(ViewState("RFDNo"))
                iHeightToUse = 200
                If commonFunctions.CheckDataSet(dsRFDProcess) = True Then
                    txtProcessNotes.Text = dsRFDProcess.Tables(0).Rows(0).Item("ProcessNotes").ToString

                    If txtProcessNotes.Text.Trim <> "" And txtProcessNotes.Text.Trim.Length <> 0 Then

                        iHeigthBySpecificCharCount = 0
                        iHeightByTextFieldLength = 0
                        iHeightToUse = 200

                        'count all characters
                        iHeigthBySpecificCharCount = (txtProcessNotes.Text.Trim.Length / 80) * 20
                        'count the number of carriage return line feeds
                        iHeightByTextFieldLength = (UBound(Split(txtProcessNotes.Text, vbCrLf)) * 40)

                        'if calculated heights are greater than 200 use the greater of the 2
                        If iHeigthBySpecificCharCount > 200 Or iHeightByTextFieldLength > 200 Then
                            If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                iHeightToUse = iHeigthBySpecificCharCount
                            Else
                                iHeightToUse = iHeightByTextFieldLength
                            End If

                            txtProcessNotes.Height = iHeightToUse
                        End If
                    End If
                End If

                dsRFDCapital = RFDModule.GetRFDCapital(ViewState("RFDNo"))
                iHeightToUse = 200
                If commonFunctions.CheckDataSet(dsRFDCapital) = True Then
                    txtCapitalNotes.Text = dsRFDCapital.Tables(0).Rows(0).Item("CapitalNotes").ToString

                    If txtCapitalNotes.Text.Trim <> "" And txtCapitalNotes.Text.Trim.Length <> 0 Then

                        iHeigthBySpecificCharCount = 0
                        iHeightByTextFieldLength = 0
                        iHeightToUse = 200

                        'count all characters
                        iHeigthBySpecificCharCount = (txtCapitalNotes.Text.Trim.Length / 80) * 20
                        'count the number of carriage return line feeds
                        iHeightByTextFieldLength = (UBound(Split(txtCapitalNotes.Text, vbCrLf)) * 40)

                        'if calculated heights are greater than 200 use the greater of the 2
                        If iHeigthBySpecificCharCount > 200 Or iHeightByTextFieldLength > 200 Then
                            If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                iHeightToUse = iHeigthBySpecificCharCount
                            Else
                                iHeightToUse = iHeightByTextFieldLength
                            End If

                            txtCapitalNotes.Height = iHeightToUse
                        End If
                    End If

                    If dsRFDCapital.Tables(0).Rows(0).Item("CapitalLeadTime") <> 0 Then
                        txtCapitalLeadTime.Text = Format(dsRFDCapital.Tables(0).Rows(0).Item("CapitalLeadTime"), "##")
                    End If

                    If dsRFDCapital.Tables(0).Rows(0).Item("CapitalLeadUnits").ToString.Trim <> "" Then
                        ddCapitalLeadUnits.SelectedValue = dsRFDCapital.Tables(0).Rows(0).Item("CapitalLeadUnits").ToString.Trim
                    End If

                End If

                dsRFDTooling = RFDModule.GetRFDTooling(ViewState("RFDNo"))
                iHeightToUse = 200
                If commonFunctions.CheckDataSet(dsRFDTooling) = True Then
                    txtToolingNotes.Text = dsRFDTooling.Tables(0).Rows(0).Item("ToolingNotes").ToString

                    If txtToolingNotes.Text.Trim <> "" And txtToolingNotes.Text.Trim.Length <> 0 Then
                        iHeigthBySpecificCharCount = 0
                        iHeightByTextFieldLength = 0
                        iHeightToUse = 200

                        'count all characters
                        iHeigthBySpecificCharCount = (txtToolingNotes.Text.Trim.Length / 80) * 20
                        'count the number of carriage return line feeds
                        iHeightByTextFieldLength = (UBound(Split(txtToolingNotes.Text, vbCrLf)) * 40)

                        'if calculated heights are greater than 200 use the greater of the 2
                        If iHeigthBySpecificCharCount > 200 Or iHeightByTextFieldLength > 200 Then
                            If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                iHeightToUse = iHeigthBySpecificCharCount
                            Else
                                iHeightToUse = iHeightByTextFieldLength
                            End If

                            txtToolingNotes.Height = iHeightToUse
                        End If
                    End If

                    If dsRFDTooling.Tables(0).Rows(0).Item("ToolingLeadTime") <> 0 Then
                        txtToolingLeadTime.Text = Format(dsRFDTooling.Tables(0).Rows(0).Item("ToolingLeadTime"), "##")
                    End If

                    If dsRFDTooling.Tables(0).Rows(0).Item("ToolingLeadUnits").ToString.Trim <> "" Then
                        ddToolingLeadUnits.SelectedValue = dsRFDTooling.Tables(0).Rows(0).Item("ToolingLeadUnits").ToString.Trim
                    End If
                End If

                ViewState("OrigUGNFacility") = GetDefaultUGNFacility()
                ViewState("OrigOEMManufacturer") = GetDefaultOEMManufacturer()

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
    Private Function GetDefaultOEMManufacturer() As String

        Dim strOEMManufacturer As String = ""

        Try

            Dim objRFDCustomerProgramBLL As New RFDCustomerProgramBLL
            Dim dtProgram As DataTable
            dtProgram = objRFDCustomerProgramBLL.GetRFDCustomerProgram(ViewState("RFDNo"))
            If commonFunctions.CheckDataTable(dtProgram) = True Then
                strOEMManufacturer = dtProgram.Rows(0).Item("OEMManufacturer").ToString
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return strOEMManufacturer

    End Function
    Private Function GetDefaultUGNFacility() As String

        Dim strUGNFacility = ""

        Try

            Dim objRFDFacilityDeptBLL As New RFDFacilityDeptBLL
            Dim dtFacility As DataTable
            dtFacility = objRFDFacilityDeptBLL.GetRFDFacilityDept(ViewState("RFDNo"))
            If commonFunctions.CheckDataTable(dtFacility) = True Then
                strUGNFacility = dtFacility.Rows(0).Item("UGNFacility").ToString
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return strUGNFacility

    End Function

    Protected Sub CompareCurrentAndNewFGDrawing()

        Try

            If txtCurrentFGInitialDimensionAndDensity.Text.Trim <> txtNewFGInitialDimensionAndDensity.Text.Trim And txtNewFGInitialDimensionAndDensity.Text.Trim <> "" Then
                txtNewFGInitialDimensionAndDensity.BackColor = Color.Yellow
            Else
                txtNewFGInitialDimensionAndDensity.BackColor = Color.White
            End If

            If txtCurrentFGInStepTracking.Text.Trim <> txtNewFGInStepTracking.Text.Trim And txtNewFGInStepTracking.Text.Trim <> "" Then
                txtNewFGInStepTracking.BackColor = Color.Yellow
            Else
                txtNewFGInStepTracking.BackColor = Color.White
            End If

            If txtCurrentFGAMDValue.Text.Trim <> txtNewFGAMDValue.Text.Trim And txtNewFGAMDValue.Text.Trim <> "" Then
                txtNewFGAMDValue.BackColor = Color.Yellow
            Else
                txtNewFGAMDValue.BackColor = Color.White
            End If

            If ddCurrentFGAMDUnits.SelectedIndex <> ddNewFGAMDUnits.SelectedIndex And ddNewFGAMDUnits.SelectedIndex > 0 Then
                ddNewFGAMDUnits.BackColor = Color.Yellow
            Else
                ddNewFGAMDUnits.BackColor = Color.White
            End If

            If txtCurrentFGAMDTolerance.Text.Trim <> txtNewFGAMDTolerance.Text.Trim And txtNewFGAMDTolerance.Text.Trim <> "" Then
                txtNewFGAMDTolerance.BackColor = Color.Yellow
            Else
                txtNewFGAMDTolerance.BackColor = Color.White
            End If

            If txtCurrentFGWMDValue.Text.Trim <> txtNewFGWMDValue.Text.Trim Then
                txtNewFGWMDValue.BackColor = Color.Yellow
            Else
                txtNewFGWMDValue.BackColor = Color.White
            End If

            If txtCurrentFGWMDTolerance.Text.Trim <> txtNewFGWMDTolerance.Text.Trim And txtNewFGWMDTolerance.Text.Trim <> "" Then
                txtNewFGWMDTolerance.BackColor = Color.Yellow
            Else
                txtNewFGWMDTolerance.BackColor = Color.White
            End If

            If ddCurrentFGWMDUnits.SelectedIndex <> ddNewFGWMDUnits.SelectedIndex And ddNewFGWMDUnits.SelectedIndex > 0 Then
                ddNewFGWMDUnits.BackColor = Color.Yellow
            Else
                ddNewFGWMDUnits.BackColor = Color.White
            End If

            If txtCurrentFGDensityValue.Text.Trim <> txtNewFGDensityValue.Text.Trim Then
                txtNewFGDensityValue.BackColor = Color.Yellow
            Else
                txtNewFGDensityValue.BackColor = Color.White
            End If

            If txtCurrentFGDensityTolerance.Text.Trim <> txtNewFGDensityTolerance.Text.Trim And txtNewFGDensityTolerance.Text.Trim <> "" Then
                txtNewFGDensityTolerance.BackColor = Color.Yellow
            Else
                txtNewFGDensityTolerance.BackColor = Color.White
            End If

            If txtCurrentFGDensityUnits.Text.Trim <> txtNewFGDensityUnits.Text.Trim And txtNewFGDensityUnits.Text.Trim <> "" Then
                txtNewFGDensityUnits.BackColor = Color.Yellow
            Else
                txtNewFGDensityUnits.BackColor = Color.White
            End If

            If txtCurrentFGConstruction.Text.Trim <> txtNewFGConstruction.Text.Trim And txtNewFGConstruction.Text.Trim <> "" Then
                txtNewFGConstruction.BackColor = Color.Yellow
            Else
                txtNewFGConstruction.BackColor = Color.White
            End If

            If txtCurrentFGDrawingNotes.Text.Trim <> txtNewFGDrawingNotes.Text.Trim And txtNewFGDrawingNotes.Text.Trim <> "" Then
                txtNewFGDrawingNotes.BackColor = Color.Yellow
            Else
                txtNewFGDrawingNotes.BackColor = Color.White
            End If

            If ddCurrentFGSubFamily.SelectedIndex <> ddNewFGSubFamily.SelectedIndex And ddNewFGSubFamily.SelectedIndex > 0 Then
                ddNewFGSubFamily.BackColor = Color.Yellow
            Else
                ddNewFGSubFamily.BackColor = Color.White
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

    Protected Sub CompareCurrentAndNewChildDrawing()

        Try

            If txtCurrentChildAMDValue.Text.Trim <> txtNewChildAMDValue.Text.Trim And txtNewChildAMDValue.Text.Trim <> "" Then
                txtNewChildAMDValue.BackColor = Color.Yellow
            Else
                txtNewChildAMDValue.BackColor = Color.White
            End If

            If txtCurrentChildAMDTolerance.Text.Trim <> txtNewChildAMDTolerance.Text.Trim And txtNewChildAMDTolerance.Text.Trim <> "" Then
                txtNewChildAMDTolerance.BackColor = Color.Yellow
            Else
                txtNewChildAMDTolerance.BackColor = Color.White
            End If

            If ddCurrentChildAMDUnits.SelectedIndex <> ddNewChildAMDUnits.SelectedIndex And ddNewChildAMDUnits.SelectedIndex > 0 Then
                ddNewChildAMDUnits.BackColor = Color.Yellow
            Else
                ddNewChildAMDUnits.BackColor = Color.White
            End If

            If txtCurrentChildWMDValue.Text.Trim <> txtNewChildWMDValue.Text.Trim And txtNewChildWMDValue.Text.Trim <> "" Then
                txtNewChildWMDValue.BackColor = Color.Yellow
            Else
                txtNewChildWMDValue.BackColor = Color.White
            End If

            If txtCurrentChildWMDTolerance.Text.Trim <> txtNewChildWMDTolerance.Text.Trim And txtNewChildWMDTolerance.Text.Trim <> "" Then
                txtNewChildWMDTolerance.BackColor = Color.Yellow
            Else
                txtNewChildWMDTolerance.BackColor = Color.White
            End If

            If ddCurrentChildWMDUnits.SelectedIndex <> ddNewChildWMDUnits.SelectedIndex And ddNewChildWMDUnits.SelectedIndex > 0 Then
                ddNewChildWMDUnits.BackColor = Color.Yellow
            Else
                ddNewChildWMDUnits.BackColor = Color.White
            End If

            If txtCurrentChildDensityValue.Text.Trim <> txtNewChildDensityValue.Text.Trim And txtNewChildDensityValue.Text.Trim <> "" Then
                txtNewChildDensityValue.BackColor = Color.Yellow
            Else
                txtNewChildDensityValue.BackColor = Color.White
            End If

            If txtCurrentChildDensityTolerance.Text.Trim <> txtNewChildDensityTolerance.Text.Trim And txtNewChildDensityTolerance.Text.Trim <> "" Then
                txtNewChildDensityTolerance.BackColor = Color.Yellow
            Else
                txtNewChildDensityTolerance.BackColor = Color.White
            End If

            If txtCurrentChildDensityUnits.Text.Trim <> txtNewChildDensityUnits.Text.Trim And txtNewChildDensityUnits.Text.Trim <> "" Then
                txtNewChildDensityUnits.BackColor = Color.Yellow
            Else
                txtNewChildDensityUnits.BackColor = Color.White
            End If

            If txtCurrentChildConstruction.Text.Trim <> txtNewChildConstruction.Text.Trim And txtNewChildConstruction.Text.Trim <> "" Then
                txtNewChildConstruction.BackColor = Color.Yellow
            Else
                txtNewChildConstruction.BackColor = Color.White
            End If

            If txtCurrentChildDrawingNotes.Text.Trim <> txtNewChildDrawingNotes.Text.Trim And txtNewChildDrawingNotes.Text.Trim <> "" Then
                txtNewChildDrawingNotes.BackColor = Color.Yellow
            Else
                txtNewChildDrawingNotes.BackColor = Color.White
            End If

            If ddCurrentChildDesignationType.SelectedIndex <> ddNewChildDesignationType.SelectedIndex And ddNewChildDesignationType.SelectedIndex > 0 Then
                ddNewChildDesignationType.BackColor = Color.Yellow
            Else
                ddNewChildDesignationType.BackColor = Color.White
            End If

            If ddCurrentChildFamily.SelectedIndex <> ddNewChildFamily.SelectedIndex And ddNewChildFamily.SelectedIndex > 0 Then
                ddNewChildFamily.BackColor = Color.Yellow
            Else
                ddNewChildFamily.BackColor = Color.White
            End If

            If ddCurrentChildSubFamily.SelectedIndex <> ddNewChildSubFamily.SelectedIndex And ddNewChildSubFamily.SelectedIndex > 0 Then
                ddNewChildSubFamily.BackColor = Color.Yellow
            Else
                ddNewChildSubFamily.BackColor = Color.White
            End If

            If ddCurrentChildPurchasedGood.SelectedIndex <> ddNewChildPurchasedGood.SelectedIndex And ddNewChildPurchasedGood.SelectedIndex > 0 Then
                ddNewChildPurchasedGood.BackColor = Color.Yellow
            Else
                ddNewChildPurchasedGood.BackColor = Color.White
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
   
    Protected Sub InitializeAllControls()

        Try
            Dim iRowCounter As Integer = 0

            'adjust text of button
            If ViewState("CurrentCustomerProgramRow") = 0 Then
                btnSaveCustomerProgram.Text = "Add Customer/Program"
                btnCancelCustomerProgram.Visible = False
            End If

            'disable and / or set invisible
            btnBusinessAwarded.Visible = False
            btnCalculateTargetAnnualSales.Visible = False
            btnCancelChild.Visible = False           
            btnSaveChild.Visible = False
            btnSaveChildDetails.Visible = False
            btnCopy.Visible = False           
            btnCurrentChildCopyAll.Visible = False
            btnCurrentChildCopyAMD.Visible = False
            btnCurrentChildCopyConstruction.Visible = False
            btnCurrentChildCopyDensity.Visible = False
            btnCurrentChildCopyDensity.Visible = False
            btnCurrentChildCopyDesignationType.Visible = False
            btnCurrentChildCopyNotes.Visible = False
            btnCurrentChildCopyPurchasedGood.Visible = False
            btnCurrentChildCopySubfamily.Visible = False
            btnCurrentChildCopyWMD.Visible = False
            btnCurrentFGCopyAll.Visible = False
            btnCurrentFGCopyAMD.Visible = False
            btnCurrentFGCopyConstruction.Visible = False
            btnCurrentFGCopyDensity.Visible = False
            btnCurrentFGCopyInitialDimensionAndDensity.Visible = False
            btnCurrentFGCopyInStepTracking.Visible = False
            btnCurrentFGCopyNotes.Visible = False
            btnCurrentFGCopyWMD.Visible = False
            btnGenerateNewChildDrawing.Visible = False
            btnGenerateNewFGDrawing.Visible = False
            btnGetPlanningForecastingVehicle.Visible = False
            btnGetFGDMSBOM.Visible = False
            btnPreview.Visible = False
            btnPreviewBottom.Visible = False
            btnResetReplyComment.Visible = False
            btnSaveCustomerProgram.Visible = False
            btnSaveDescription.Visible = False
            btnSaveCustomerPartNo.Visible = False
            btnSaveFGMeasurements.Visible = False
            btnSaveNetworkFileReference.Visible = False
            btnSaveProcess.Visible = False
            btnSaveReplyComment.Visible = False
            btnSaveTooling.Visible = False
            btnSaveUploadSupportingDocument.Visible = False
            btnSaveVendor.Visible = False
            btnSubmitApproval.Visible = False
            btnVoid.Visible = False
            btnClose.Visible = False
            cbDVPRrequired.Enabled = False
            cbAffectsCostSheetOnly.Visible = False
            cbCapitalRequired.Enabled = False
            cbCostingRequired.Enabled = False
            cbPackagingRequired.Enabled = False
            cbPlantControllerRequired.Enabled = False
            cbProcessRequired.Enabled = False
            cbProductDevelopmentRequired.Enabled = False
            cbPurchasingExternalRFQRequired.Enabled = False
            cbPurchasingRequired.Enabled = False
            cbQualityEngineeringRequired.Enabled = False
            cbToolingRequired.Enabled = False
            ddAccountManager.Enabled = False
            ddBusinessProcessAction.Enabled = False
            ddBusinessProcessType.Enabled = False
            ddDesignationType.Enabled = False
            ddInitiator.Enabled = False
            ddNewFGFamily.Enabled = False
            ddNewFGSubFamily.Enabled = False
            ddPriceCode.Enabled = False
            ddPriority.Enabled = False
            ddProductDevelopmentTeamMemberByCommodity.Enabled = False
            ddPurchasingTeamMemberByFamily.Enabled = False
            ddPurchasingTeamMemberByMake.Enabled = False
            ddWorkFlowCommodity.Enabled = False
            ddWorkflowFamily.Enabled = False
            ddWorkFlowMake.Enabled = False
            fileUploadSupportingDoc.Visible = False
            fileTextNetworkFileReference.Visible = False
            btnBrowserNetworkFileReference.Visible = False
            gvApproval.Columns(gvApproval.Columns.Count - 1).Visible = False
            gvApproval.ShowFooter = False
            gvChildPart.Columns(gvChildPart.Columns.Count - 1).Visible = False
            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = False
            gvFacilityDept.Columns(gvFacilityDept.Columns.Count - 1).Visible = False
            gvFacilityDept.ShowFooter = False
            gvKit.Columns(gvKit.Columns.Count - 1).Visible = False
            gvKit.ShowFooter = False
            gvQuestion.Columns(0).Visible = False
            gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = False
            gvVendor.Columns(gvVendor.Columns.Count - 1).Visible = False
            gvVendor.ShowFooter = False
            gvChildPartPackaging.Columns(gvChildPartPackaging.Columns.Count - 1).Visible = False

            If ddDesignationType.SelectedValue = "C" Then
                gvFinishedGoodPackaging.Visible = True
            Else
                gvFinishedGoodPackaging.Visible = False
            End If

            gvFinishedGoodPackaging.Columns(gvFinishedGoodPackaging.Columns.Count - 1).Visible = False

            If ViewState("isPlantController") = False Then
                gvLabor.Visible = False
                gvLabor.Columns(gvLabor.Columns.Count - 1).Visible = False
                gvLabor.ShowFooter = False

                gvOverhead.Visible = False
                gvOverhead.Columns(gvOverhead.Columns.Count - 1).Visible = False
                gvOverhead.ShowFooter = False
            End If

            iBtnCurrentCustomerPartNoSearch.Visible = False
            iBtnNewECINoSearch.Visible = False
            iBtnNewChildECINoSearch.Visible = False            
            imgDueDate.Visible = False
            lblChildTip2.Visible = False
            lblFileUploadLabel.Visible = False
            lblMaxNote.Visible = False
            lblNetworkFileLabel.Visible = False
            rbCopyType.Visible = False
            rbGenerateNewFGDrawing.Visible = False
            rbGenerateNewChildDrawing.Visible = False

            If ViewState("CurrentChildPartRow") = 0 Then
                acChildPart.Visible = False
            End If

            tblCommunicationBoardExistingQuestion.Visible = False
            tblCommunicationBoardNewQuestion.Visible = False
            tblCustomerProgram.Visible = False
            tblMakes.Visible = False
            txtCapitalNotes.Enabled = False
            txtCopyReason.Enabled = False
            txtDueDate.Enabled = False
            txtRFDDesc.Enabled = False
            txtImpactOnUGN.Enabled = False
            txtProcessNotes.Enabled = False
            txtTargetAnnualSales.Enabled = False
            txtTargetAnnualVolume.Enabled = False
            txtTargetPrice.Enabled = False
            txtToolingNotes.Enabled = False
            txtNewFGInitialDimensionAndDensity.Enabled = False
            txtNewFGAMDValue.Enabled = False
            txtNewFGAMDTolerance.Enabled = False
            ddNewFGAMDUnits.Enabled = False
            txtNewFGWMDValue.Enabled = False
            txtNewFGWMDTolerance.Enabled = False
            ddNewFGWMDUnits.Enabled = False
            txtNewFGDensityValue.Enabled = False
            txtNewFGDensityTolerance.Enabled = False
            txtNewFGDensityUnits.Enabled = False
            txtNewFGConstruction.Enabled = False
            txtNewFGDrawingNotes.Enabled = False
            txtCapitalLeadTime.Enabled = False
            ddCapitalLeadUnits.Enabled = False
            txtToolingLeadTime.Enabled = False
            ddToolingLeadUnits.Enabled = False

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub SetAdminControls()

        Try
            btnCalculateTargetAnnualSales.Visible = ViewState("isEdit")

            btnCurrentChildCopyAll.Visible = ViewState("isEdit")
            btnCurrentChildCopyAMD.Visible = ViewState("isEdit")
            btnCurrentChildCopyConstruction.Visible = ViewState("isEdit")
            btnCurrentChildCopyDensity.Visible = ViewState("isEdit")
            btnCurrentChildCopyDensity.Visible = ViewState("isEdit")
            btnCurrentChildCopyDesignationType.Visible = ViewState("isEdit")
            btnCurrentChildCopyNotes.Visible = ViewState("isEdit")
            btnCurrentChildCopyPurchasedGood.Visible = ViewState("isEdit")
            btnCurrentChildCopySubfamily.Visible = ViewState("isEdit")
            btnCurrentChildCopyWMD.Visible = ViewState("isEdit")
            btnCurrentFGCopyAll.Visible = ViewState("isEdit")
            btnCurrentFGCopyAMD.Visible = ViewState("isEdit")
            btnCurrentFGCopyConstruction.Visible = ViewState("isEdit")
            btnCurrentFGCopyDensity.Visible = ViewState("isEdit")
            btnCurrentFGCopyNotes.Visible = ViewState("isEdit")
            btnCurrentFGCopyWMD.Visible = ViewState("isEdit")
            btnCurrentFGCopySubFamily.Visible = ViewState("isEdit")

            If btnSaveChild.Text = "Update Child" _
            Or ViewState("InitiatorTeamMemberID") = ViewState("TeamMemberID") _
            Or ViewState("isProductDevelopment") = True Then
                btnSaveChild.Visible = ViewState("isEdit")
                btnSaveChildDetails.Visible = ViewState("isEdit")
            End If

            btnSaveCustomerProgram.Visible = ViewState("isEdit")

            If ViewState("CurrentCustomerProgramRow") = 0 Then
                tblMakes.Visible = ViewState("isEdit")
            End If

            If ViewState("CurrentRSSID") > 0 Then
                btnResetReplyComment.Visible = ViewState("isEdit")
                btnSaveReplyComment.Visible = ViewState("isEdit")
            End If

            btnSaveDescription.Visible = ViewState("isEdit")
            btnSaveCustomerPartNo.Visible = ViewState("isEdit")
            btnSaveFGMeasurements.Visible = ViewState("isEdit")
            btnSaveNetworkFileReference.Visible = ViewState("isEdit")
            fileTextNetworkFileReference.Visible = ViewState("isEdit")
            btnBrowserNetworkFileReference.Visible = ViewState("isEdit")
            btnSaveVendor.Visible = ViewState("isEdit")
            btnSaveUploadSupportingDocument.Visible = ViewState("isEdit")
            btnVoid.Visible = ViewState("isEdit")
            btnClose.Visible = ViewState("isEdit")
            cbDVPRrequired.Enabled = ViewState("isEdit")
            cbRDrequired.Enabled = ViewState("isEdit")
            ddAccountManager.Enabled = ViewState("isEdit")

            If ViewState("RFDNo") > 0 Then

                'if rfq and sales/program manager
                If ViewState("BusinessProcessTypeID") <> 2 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                    ddBusinessProcessType.Enabled = ViewState("isEdit")

                    If ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7 Then
                        ddBusinessProcessAction.Enabled = ViewState("isEdit")
                    End If
                End If

                'OLD RULE - only RFQs of New Parts needs the Business Awarded Button
                'NEW RULE - 04/18/2012 - only Quote Only Business Process Type of Source Quoting Business Process Action need this
                If ViewState("BusinessProcessTypeID") = 7 And ViewState("BusinessProcessActionID") = 10 Then
                    If ViewState("StatusID") = 2 And ViewState("bBusinessAwarded") = False _
                                                           And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then

                        btnBusinessAwarded.Visible = ViewState("isEdit")
                    End If
                End If

                'if rfc and NOT sales and NOT program management
                If ViewState("BusinessProcessTypeID") = 2 And ViewState("isSales") = False And ViewState("isProgramManagement") = False Then
                    ddBusinessProcessType.Enabled = ViewState("isEdit")
                End If

                If ViewState("isAdmin") Or ViewState("InitiatorTeamMemberID") = ViewState("TeamMemberID") Then
                    ddBusinessProcessType.Enabled = ViewState("isEdit")
                    ddBusinessProcessAction.Enabled = ViewState("isEdit")
                End If
            Else
                ddBusinessProcessType.Enabled = ViewState("isEdit")
            End If

            If ViewState("BusinessProcessTypeID") <> 1 Then 'IF RFQ then must be finished good
                ddDesignationType.Enabled = ViewState("isEdit")
            End If

            If ViewState("RFDNo") = 0 Or ViewState("isAdmin") Then
                ddInitiator.Enabled = ViewState("isEdit")
            End If

            ddPriceCode.Enabled = ViewState("isEdit")
            ddPriority.Enabled = ViewState("isEdit")
            ddProductDevelopmentTeamMemberByCommodity.Enabled = ViewState("isEdit")
            ddPurchasingTeamMemberByFamily.Enabled = ViewState("isEdit")
            ddPurchasingTeamMemberByMake.Enabled = ViewState("isEdit")
            ddWorkFlowCommodity.Enabled = ViewState("isEdit")
            ddWorkflowFamily.Enabled = ViewState("isEdit")
            ddWorkFlowMake.Enabled = ViewState("isEdit")

            ddNewChildAMDUnits.Enabled = ViewState("isEdit")
            ddNewChildPurchasedGood.Enabled = ViewState("isEdit")
            ddNewChildDesignationType.Enabled = ViewState("isEdit")
            ddNewChildWMDUnits.Enabled = ViewState("isEdit")

            ddNewFGAMDUnits.Enabled = ViewState("isEdit")
            ddNewFGWMDUnits.Enabled = ViewState("isEdit")

            ddNewChildFamily.Enabled = ViewState("isEdit")
            ddNewChildSubFamily.Enabled = ViewState("isEdit")

            ddNewFGFamily.Enabled = ViewState("isEdit")
            ddNewFGSubFamily.Enabled = ViewState("isEdit")

            If ViewState("isQualityEngineer") = True Or ViewState("isAdmin") = True Then
                iBtnNewECINoSearch.Visible = ViewState("isEdit")
            End If

            imgDueDate.Visible = ViewState("isEdit")
            lblNetworkFileLabel.Visible = ViewState("isEdit")
            tblCustomerProgram.Visible = ViewState("isEdit")
            txtNewChildAMDTolerance.Enabled = ViewState("isEdit")
            txtNewChildAMDValue.Enabled = ViewState("isEdit")
            txtNewChildConstruction.Enabled = ViewState("isEdit")
            txtNewChildDensityTolerance.Enabled = ViewState("isEdit")
            txtNewChildDensityUnits.Enabled = ViewState("isEdit")
            txtNewChildDrawingNotes.Enabled = ViewState("isEdit")
            txtNewChildWMDTolerance.Enabled = ViewState("isEdit")
            txtNewChildWMDValue.Enabled = ViewState("isEdit")
            txtNewFGAMDTolerance.Enabled = ViewState("isEdit")
            txtNewFGAMDValue.Enabled = ViewState("isEdit")
            txtNewFGConstruction.Enabled = ViewState("isEdit")
            txtNewFGDensityTolerance.Enabled = ViewState("isEdit")
            txtNewFGDensityUnits.Enabled = ViewState("isEdit")
            txtNewFGDensityValue.Enabled = ViewState("isEdit")
            txtNewFGDrawingNotes.Enabled = ViewState("isEdit")
            txtNewFGWMDTolerance.Enabled = ViewState("isEdit")
            txtNewFGWMDValue.Enabled = ViewState("isEdit")
            txtDueDate.Enabled = ViewState("isEdit")
            txtRFDDesc.Enabled = ViewState("isEdit")
            txtImpactOnUGN.Enabled = ViewState("isEdit")

            If ViewState("isProcess") = True _
                Or ViewState("isAdmin") = True _
                Or ViewState("isSales") = True _
                Or ViewState("isProgramManagement") = True _
                Or ViewState("InitiatorTeamMemberID") = ViewState("TeamMemberID") _
                Then

                txtProcessNotes.Enabled = ViewState("isEdit")
                btnSaveProcess.Visible = ViewState("isEdit")
            End If

            txtTargetAnnualSales.Enabled = ViewState("isEdit")
            txtTargetAnnualVolume.Enabled = ViewState("isEdit")
            txtTargetPrice.Enabled = ViewState("isEdit")

            If ViewState("isCapital") = True _
                Or ViewState("isAdmin") = True _
                Or ViewState("isSales") = True _
                Or ViewState("isProgramManagement") = True _
                Or ViewState("InitiatorTeamMemberID") = ViewState("TeamMemberID") _
                Then

                txtCapitalNotes.Enabled = ViewState("isEdit")
                btnSaveTooling.Visible = ViewState("isEdit")

                txtCapitalLeadTime.Enabled = ViewState("isEdit")
                ddCapitalLeadUnits.Enabled = ViewState("isEdit")
            End If

            If ViewState("isTooling") = True _
                Or ViewState("isAdmin") = True _
                Or ViewState("isSales") = True _
                Or ViewState("isProgramManagement") = True _
                Or ViewState("InitiatorTeamMemberID") = ViewState("TeamMemberID") _
                Then

                txtToolingNotes.Enabled = ViewState("isEdit")
                btnSaveTooling.Visible = ViewState("isEdit")

                txtToolingLeadTime.Enabled = ViewState("isEdit")
                ddToolingLeadUnits.Enabled = ViewState("isEdit")
            End If

            If ViewState("isProductDevelopment") = True Then 'Product Development
                btnCurrentChildCopyInitialDimensionAndDensity.Visible = ViewState("isEdit")
                btnCurrentChildCopyInStepTracking.Visible = ViewState("isEdit")
                btnCurrentChildCopySubfamily.Visible = ViewState("isEdit")

                btnCurrentFGCopyInitialDimensionAndDensity.Visible = ViewState("isEdit")
                btnCurrentFGCopyInStepTracking.Visible = ViewState("isEdit")

                btnGenerateNewFGDrawing.Visible = ViewState("isEdit")

                rbGenerateNewFGDrawing.Visible = ViewState("isEdit")

                txtNewChildInitialDimensionAndDensity.Enabled = ViewState("isEdit")
                txtNewChildInStepTracking.Enabled = ViewState("isEdit")

                txtNewFGInitialDimensionAndDensity.Enabled = ViewState("isEdit")
                txtNewFGInStepTracking.Enabled = ViewState("isEdit")

                If ViewState("CurrentChildPartRow") > 0 Then
                    btnGenerateNewChildDrawing.Visible = ViewState("isEdit")
                    rbGenerateNewChildDrawing.Visible = ViewState("isEdit")
                End If
            End If

            gvApproval.Columns(gvApproval.Columns.Count - 1).Visible = ViewState("isEdit")

            'if current team member is initiator 
            'or (a plant champion and NOT RFQ type)
            'or ((sales or program manager) and NOT RFC type)
            If ViewState("TeamMemberID") = ViewState("InitiatorTeamMemberID") Or _
                    ((ViewState("isSales") = True Or ViewState("isProgramManagement") = True) And ViewState("BusinessProcessTypeID") <> 2) Or _
                    (((ViewState("SubscriptionID") = 4 Or ViewState("isCosting") = True) And ViewState("BusinessProcessTypeID") <> 1)) _
                    Or ViewState("isAdmin") = True Then

                If ViewState("isSales") = False And ViewState("isProgramManagement") = False Then
                    cbAffectsCostSheetOnly.Visible = ViewState("isEdit")
                End If

                If ViewState("isSales") = True Or ViewState("isProgramManagement") = True Then
                    cbAffectsCostSheetOnly.Checked = False
                End If

                If ddStatus.SelectedIndex >= 0 Then
                    'if open or rejected
                    If ddStatus.SelectedValue = 1 Or ddStatus.SelectedValue = 5 Then
                        btnSubmitApproval.Visible = ViewState("isEdit")
                    End If
                End If
            End If

            gvChildPart.Columns(gvChildPart.Columns.Count - 1).Visible = ViewState("isEdit")
            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = ViewState("isEdit")
            gvFacilityDept.Columns(gvFacilityDept.Columns.Count - 1).Visible = ViewState("isEdit")
            gvFacilityDept.ShowFooter = ViewState("isEdit")
            gvKit.Columns(gvKit.Columns.Count - 1).Visible = ViewState("isEdit")
            gvKit.ShowFooter = ViewState("isEdit")
            gvQuestion.Columns(0).Visible = ViewState("isEdit")
            gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = ViewState("isEdit")
            gvVendor.Columns(gvVendor.Columns.Count - 1).Visible = ViewState("isEdit")
            gvVendor.ShowFooter = ViewState("isEdit")

            If ViewState("isPackaging") = True Or ViewState("isAdmin") = True Then
                gvChildPartPackaging.Columns(gvChildPartPackaging.Columns.Count - 1).Visible = ViewState("isEdit")
                gvFinishedGoodPackaging.Columns(gvFinishedGoodPackaging.Columns.Count - 1).Visible = ViewState("isEdit")
            End If

            If ViewState("isPlantController") = True Or ViewState("isAdmin") = True And gvLabor.ShowFooter = False Then
                gvLabor.Columns(gvLabor.Columns.Count - 1).Visible = ViewState("isEdit")
                gvLabor.ShowFooter = ViewState("isEdit")

                gvOverhead.Columns(gvOverhead.Columns.Count - 1).Visible = ViewState("isEdit")
                gvOverhead.ShowFooter = ViewState("isEdit")
            End If

            lblChildTip2.Visible = True

            tblChildPart.Visible = ViewState("isEdit")

            tblCommunicationBoardExistingQuestion.Visible = ViewState("isEdit")
            tblCommunicationBoardNewQuestion.Visible = ViewState("isEdit")

            'only show this button if an FG DMS Drawing number is assigned and the team member is product development, costing, purchasing
            If (ViewState("isProductDevelopment") = True Or ViewState("isPurchasing") = True Or ViewState("isCostring") = True) And txtNewDrawingNo.Text.Trim <> "" Then
                btnGetFGDMSBOM.Visible = ViewState("isEdit")
                '**LREY 07/01/2014
                If ViewState("DMSDrawingNoUpdate") <> "" Then
                    btnSubmitApproval.Visible = ViewState("isEdit")
                End If
            End If
            'Only show this button if RFD is for a Quote Only and a new Supporting Document has been uploaded
            If (ViewState("isProductDevelopment") = True And ViewState("QuoteOnlySupDocUpdate") <> "") Then
                If (ddStatus.SelectedValue <> 1 Or ddStatus.SelectedValue <> 5) And (ViewState("BusinessProcessTypeID") = 7) Then
                    btnSubmitApproval.Visible = ViewState("isEdit")
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
    Protected Sub EnableControls()

        Try

            'Dim iRowCounter As Integer = 0

            InitializeAllControls()

            'New RFDs
            If ViewState("RFDNo") = 0 Then
                'set RFD Initiator to Current Team Member ID
                If ddInitiator.SelectedIndex <= 0 And ViewState("SubscriptionID") > 0 And ViewState("TeamMemberID") > 0 Then
                    ddInitiator.SelectedValue = ViewState("TeamMemberID")
                End If

                If ddBusinessProcessType.SelectedIndex < 0 Then
                    If ViewState("SubscriptionID") = 9 Or ViewState("SubscriptionID") = 65 Then
                        'if subscriptions are Account Manager or Tooling, then set default Business Process Type to New
                        '1 - Customer Driven Change (RFQ)
                        ddBusinessProcessType.SelectedValue = 1
                        ddDesignationType.SelectedValue = "C"                        
                    Else 'all other subscriptions
                        '2 - UGN Driven Change (RFC) With SOP Timing
                        ddBusinessProcessType.SelectedValue = 2
                        ddDesignationType.SelectedValue = "R"
                    End If

                End If

            End If

            'Existing RFDs
            If ViewState("RFDNo") > 0 Then
                btnCopy.Visible = ViewState("isEdit")
                txtCopyReason.Enabled = ViewState("isEdit")
                rbCopyType.Visible = ViewState("isEdit")

                'sales and program management cannot create RFCs
                If ViewState("BusinessProcessTypeID") = 2 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                    If ViewState("isQualityEngineer") = False Then
                        ViewState("isAdmin") = False
                        ViewState("isEdit") = False
                    End If                    
                End If

                'show preview buttons if not voided
                If ViewState("StatusID") <> 4 Then
                    btnPreview.Visible = True
                    btnPreviewBottom.Visible = True
                End If

                'setup preview buttons
                Dim strPreviewRFDClientScript As String = "javascript:void(window.open('crRFD_Preview.aspx?RFDNo=" & ViewState("RFDNo") & "&BusinessProcessTypeID=" & ViewState("BusinessProcessTypeID") & "&SOPNo=" & ViewState("SOPNo") & "&SOPRev=" & ViewState("SOPRev") & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                btnPreview.Attributes.Add("onclick", strPreviewRFDClientScript)
                btnPreviewBottom.Attributes.Add("onclick", strPreviewRFDClientScript)

                CheckSupportingDocGrid()

                'not approved, closed, nor voided
                If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 And ViewState("StatusID") <> 8 Then
                    AdjustApprovalRouting()
                End If

                If ddDesignationType.SelectedIndex > 0 Then
                    AdjustDesignationType()
                End If

                If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 And ViewState("StatusID") <> 8 Then
                    SetAdminControls()
                End If

                'if voided
                If ViewState("StatusID") = 4 Then
                    lblVoidComment.Visible = True
                    txtVoidComment.Visible = True
                    txtVoidComment.Enabled = False
                End If


                GetCurrentFGDrawing(txtCurrentDrawingNo.Text.Trim)
                GetNewFGDrawing(txtNewDrawingNo.Text.Trim)
                CompareCurrentAndNewFGDrawing()
                GetChildPartLinks()
                GetCustomerPartLinks()

                If ViewState("isPlantController") = False Then
                    gvLabor.Visible = cbPlantControllerRequired.Checked
                    gvOverhead.Visible = cbPlantControllerRequired.Checked
                End If

                menuBottomTabs.Items(0).Enabled = cbPackagingRequired.Checked
                menuBottomTabs.Items(1).Enabled = cbPlantControllerRequired.Checked
                menuBottomTabs.Items(2).Enabled = cbProcessRequired.Checked

                If cbToolingRequired.Checked = True Or cbCapitalRequired.Checked = True Then
                    menuBottomTabs.Items(3).Enabled = True 'cbToolingRequired.Checked
                Else
                    menuBottomTabs.Items(3).Enabled = False
                End If

            End If

            If txtNewCustomerPartNo.Text.Trim <> "" Then
                lblNewCustomerPartNoTopValue.Text = txtNewCustomerPartNo.Text
                lblNewCustomerPartNoTopValue.Visible = True
                lblNewCustomerPartNoTopLabel.Visible = True
            End If

            If txtNewDesignLevel.Text.Trim <> "" Then
                lblNewDesignLevelTopValue.Text = txtNewDesignLevel.Text
                lblNewDesignLevelTopLabel.Visible = True
                lblNewDesignLevelTopValue.Visible = True
            End If

            If ddBusinessProcessType.SelectedIndex >= 0 Then
                If ddBusinessProcessType.SelectedValue = 1 Or ddBusinessProcessType.SelectedValue = 7 Then
                    ddBusinessProcessAction.Visible = True
                    lblBusinessProcessAction.Visible = True
                End If

                ShowHideProgramManager()

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

    Private Sub GetCustomerPartLinks()

        Try
            Dim dsCosting As DataSet
            Dim dsECI As DataSet

            Dim iCostSheetID As Integer = 0
            Dim iECINo As Integer = 0

            hlnkNewCostSheetID.NavigateUrl = ""
            hlnkNewCostSheetID.Visible = False

            hlnkNewDieLayout.NavigateUrl = ""
            hlnkNewDieLayout.Visible = False

            If txtNewCostSheetID.Text.Trim <> "" Then
                iCostSheetID = CType(txtNewCostSheetID.Text.Trim, Integer)

                If iCostSheetID > 0 Then
                    dsCosting = CostingModule.GetCostSheet(iCostSheetID)

                    If commonFunctions.CheckDataSet(dsCosting) = True Then
                        hlnkNewCostSheetID.NavigateUrl = "~/Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & iCostSheetID.ToString
                        hlnkNewCostSheetID.Visible = True

                        If dsCosting.Tables(0).Rows(0).Item("isDieCut") = True Then
                            hlnkNewDieLayout.NavigateUrl = "~/Costing/Die_Layout_Preview.aspx?CostSheetID=" & iCostSheetID.ToString
                            hlnkNewDieLayout.Visible = True
                        End If
                    End If
                End If
            End If

            hlnkNewECINo.NavigateUrl = ""
            hlnkNewECINo.Visible = False

            If txtNewECINo.Text.Trim <> "" Then               
                iECINo = CType(txtNewECINo.Text.Trim, Integer)

                If iECINo > 0 Then
                    dsECI = ECIModule.GetECI(iECINo)

                    If commonFunctions.CheckDataSet(dsECI) = True Then
                        hlnkNewECINo.NavigateUrl = "~/ECI/ECI_Detail.aspx?ECINo=" & txtNewECINo.Text.Trim
                        hlnkNewECINo.Visible = True
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
    Protected Sub HandleCommentFields()

        Try

            txtApprovalComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtApprovalComments.Attributes.Add("onkeyup", "return tbCount(" + lblApprovalCommentsCharCount.ClientID + ");")
            txtApprovalComments.Attributes.Add("maxLength", "400")

            txtCapitalNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtCapitalNotes.Attributes.Add("onkeyup", "return tbCount(" + lblCapitalNotesCharCount.ClientID + ");")
            txtCapitalNotes.Attributes.Add("maxLength", "400")

            txtCopyReason.Attributes.Add("onkeypress", "return tbLimit();")
            txtCopyReason.Attributes.Add("onkeyup", "return tbCount(" + lblCopyReasonCharCount.ClientID + ");")
            txtCopyReason.Attributes.Add("maxLength", "100")

            txtImpactOnUGN.Attributes.Add("onkeypress", "return tbLimit();")
            txtImpactOnUGN.Attributes.Add("onkeyup", "return tbCount(" + lblImpactOnUGNCharCount.ClientID + ");")
            txtImpactOnUGN.Attributes.Add("maxLength", "1500")

            txtNewChildConstruction.Attributes.Add("onkeypress", "return tbLimit();")
            txtNewChildConstruction.Attributes.Add("onkeyup", "return tbCount(" + lblNewChildConstructionCharCount.ClientID + ");")
            txtNewChildConstruction.Attributes.Add("maxLength", "400")

            txtNewChildDrawingNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNewChildDrawingNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNewChildDrawingNotesCharCount.ClientID + ");")
            txtNewChildDrawingNotes.Attributes.Add("maxLength", "400")

            txtNewFGConstruction.Attributes.Add("onkeypress", "return tbLimit();")
            txtNewFGConstruction.Attributes.Add("onkeyup", "return tbCount(" + lblNewFGConstructionCharCount.ClientID + ");")
            txtNewFGConstruction.Attributes.Add("maxLength", "400")

            txtNewFGDrawingNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNewFGDrawingNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNewFGDrawingNotesCharCount.ClientID + ");")
            txtNewFGDrawingNotes.Attributes.Add("maxLength", "400")

            txtProcessNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtProcessNotes.Attributes.Add("onkeyup", "return tbCount(" + lblProcessNotesCharCount.ClientID + ");")
            txtProcessNotes.Attributes.Add("maxLength", "1000")

            txtRFDDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtRFDDesc.Attributes.Add("onkeyup", "return tbCount(" + lblRFDDescCharCount.ClientID + ");")
            txtRFDDesc.Attributes.Add("maxLength", "1000")

            txtSupportingDocDesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtSupportingDocDesc.Attributes.Add("onkeyup", "return tbCount(" + lblSupportingDocDescCharCount.ClientID + ");")
            txtSupportingDocDesc.Attributes.Add("maxLength", "200")

            txtToolingNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtToolingNotes.Attributes.Add("onkeyup", "return tbCount(" + lblToolingNotesCharCount.ClientID + ");")
            txtToolingNotes.Attributes.Add("maxLength", "1000")

            txtVendorRequirement.Attributes.Add("onkeypress", "return tbLimit();")
            txtVendorRequirement.Attributes.Add("onkeyup", "return tbCount(" + lblVendorRequirementCharCount.ClientID + ");")
            txtVendorRequirement.Attributes.Add("maxLength", "400")

            txtVoidComment.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidComment.Attributes.Add("onkeyup", "return tbCount(" + lblVoidCommentCharCount.ClientID + ");")
            txtVoidComment.Attributes.Add("maxLength", "150")

            txtCloseComment.Attributes.Add("onkeypress", "return tbLimit();")
            txtCloseComment.Attributes.Add("onkeyup", "return tbCount(" + lblCloseCommentCharCount.ClientID + ");")
            txtCloseComment.Attributes.Add("maxLength", "150")

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub InitializeAllPopUps()

        Try

            'search current PartNo
            Dim strCurrentChildPartNoClientScript As String = HandleBPCSPopUps(txtCurrentChildPartNo.ClientID, "", txtNewChildPartNameValue.ClientID)
            iBtnCurrentChildPartNoSearch.Attributes.Add("onClick", strCurrentChildPartNoClientScript)

            ''search new Child PartNo
            Dim strNewChildPartNoClientScript As String = HandleBPCSPopUps(txtNewChildPartNoValue.ClientID, "", txtNewChildPartNameValue.ClientID)
            iBtnNewChildPartNoSearch.Attributes.Add("onClick", strNewChildPartNoClientScript)

            'search current Customer PartNo
            Dim strCurrentCustomerPartNoClientScript As String = HandleCustomerPartNoPopUps(txtCurrentCustomerPartNo.ClientID, txtCurrentCustomerPartName.ClientID)
            iBtnCurrentCustomerPartNoSearch.Attributes.Add("onClick", strCurrentCustomerPartNoClientScript)

            'search current drawingno popup
            Dim strCurrentDrawingNoClientScript As String = HandleDrawingPopUps(txtCurrentDrawingNo.ClientID)
            iBtnCurrentDrawingSearch.Attributes.Add("onClick", strCurrentDrawingNoClientScript)

            'search new drawingno popup
            Dim strNewDrawingNoClientScript As String = HandleDrawingPopUps(txtNewDrawingNo.ClientID)
            iBtnNewDrawingSearch.Attributes.Add("onClick", strNewDrawingNoClientScript)

            'search new F.G./Customer PartNo ECINo popup
            Dim strNewECINoClientScript As String = HandleECIPopUps(txtNewECINo.ClientID, "TL", 0)
            iBtnNewECINoSearch.Attributes.Add("onClick", strNewECINoClientScript)

            'search current child drawingno popup
            Dim strCurrentChildDrawingNoClientScript As String = HandleDrawingPopUps(txtCurrentChildDrawingNo.ClientID)
            iBtnCurrentChildDrawingNoSearch.Attributes.Add("onClick", strCurrentChildDrawingNoClientScript)

            'search new child drawingno popup
            Dim strNewChildDrawingNoClientScript As String = HandleDrawingPopUps(txtNewChildDrawingNo.ClientID)
            iBtnNewChildDrawingSearch.Attributes.Add("onClick", strNewChildDrawingNoClientScript)

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

            If Not Page.IsPostBack Then

                InitializeViewState()

                CheckRights()

                'clear crystal reports
                RFDModule.CleanRFDCrystalReports()

                BindCriteria()

                If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then

                    ''Used to allow TM(s) to Communicated with Approvers for Q&A
                    If HttpContext.Current.Request.QueryString("pRC") <> "" Then
                        ViewState("pRC") = HttpContext.Current.Request.QueryString("pRC")
                    Else
                        ViewState("pRC") = 0
                    End If

                    ViewState("RFDNo") = HttpContext.Current.Request.QueryString("RFDNo")

                    lblRFDNo.Text = ViewState("RFDNo")
                    If ViewState("RFDNo") >= 200000 Then
                        GetTeamMemberInfo()
                        BindData()
                    Else
                        'if an archived RFD is attempting to be loaded, redirect back to search page
                        Response.Redirect("RFD_List.aspx", False)
                    End If

                    InitializeAllPopUps()

                    ''***********************************************
                    ''Code Below overrides the breadcrumb navigation 
                    ''***********************************************
                    Dim mpTextBox As Label
                    mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
                    If Not mpTextBox Is Nothing Then
                        mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Request for Development (RFD) </b> > <a href='RFD_List.aspx'> <b> List and Search </b> </a> > Detail > <a href='RFD_History.aspx?RFDNo=" & ViewState("RFDNo") & "'> History </a>"
                        mpTextBox.Visible = True
                        Master.FindControl("SiteMapPath1").Visible = False
                    End If

                    revTextNetworkFileReference.ValidationExpression = "^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$"
                End If

                HandleCommentFields()

                menuTopTabs.Items(0).Selected = True
            End If

            EnableControls()

            'Visual Basic ignores this if inside the above IF-Statement: If Not Page.IsPostBack Then
            Page.ClientScript.RegisterStartupScript(Me.[GetType](), "jsCheckTarget", "function CheckTargetInfo(){" & vbCr & vbLf & " var TmpTargetPrice = document.getElementById('" & txtTargetPrice.ClientID & "').value; var TmpTargetAnnualVolume = document.getElementById('" & txtTargetAnnualVolume.ClientID & "').value; var TmpTargetAnnualSales = document.getElementById('" & txtTargetAnnualSales.ClientID & "').value;  /* alert(TmpTargetPrice); alert(TmpTargetAnnualVolume); alert(TmpTargetAnnualSales); */ if (TmpTargetPrice != null && TmpTargetAnnualVolume != null) { if (TmpTargetPrice > 0 && TmpTargetAnnualVolume > 0 && TmpTargetAnnualSales > 0) { if (TmpTargetPrice * TmpTargetAnnualVolume != TmpTargetAnnualSales) { alert('ERROR: Target Price * Target Annual Volume does NOT equal Target Annual Sales'); } } } " & vbCr & vbLf & " }", True)

            txtTargetPrice.Attributes.Add("onblur", "javascript:CheckTargetInfo();")
            txtTargetAnnualVolume.Attributes.Add("onblur", "javascript:CheckTargetInfo();")
            txtTargetAnnualSales.Attributes.Add("onblur", "javascript:CheckTargetInfo();")

            btnBusinessAwarded.Attributes.Add("onclick", "if(confirm('Are you sure that business has been awarded for this RFD? If so, click ok to continue. ')){}else{return false}")

            btnVoid.Attributes.Add("onclick", "if(confirm('Are you sure that you want to VOID this RFD? If so, click ok to see and update the VOID comment field. THEN CLICK VOID AGAIN. ')){}else{return false}")
            btnClose.Attributes.Add("onclick", "if(confirm('Are you sure that you want to CLOSE this RFD? If so, click ok to see and update the CLOSE comment field. THEN CLICK CLOSE AGAIN. ')){}else{return false}")

            If ViewState("RFDNo") > 0 Then
                btnCopy.Attributes.Add("onclick", "if(doCopyReason('" & ViewState("RFDNo") & "')){}else{return false};")

                btnSubmitApproval.Attributes.Add("onclick", "if(confirm('Did you send all required information to Product Engineering?')){}else{return false}")
            End If

            If ViewState("pRC") > 0 Then
                mvTabs.ActiveViewIndex = Int32.Parse(13)
                mvTabs.GetActiveView()

                menuBottomTabs.Items(menuBottomTabs.Items.Count - 1).Selected = True
                ViewState("pRC") = 0
            End If

            If HttpContext.Current.Session("CopyRFD") = "Copied" Then
                HttpContext.Current.Session("CopyRFD") = Nothing

                'save RFD which will validate approval routing first, then notify
                btnSave_Click(sender, e)

                lblMessage.Text &= "<br /><br />RFD Copied Successfully."
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
    Private Sub BindBusinessProcessAction(ByVal filterQuoteOnly As Boolean, ByVal isQuoteOnly As Boolean)

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetBusinessProcessAction(0, filterQuoteOnly, isQuoteOnly)
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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindCriteria()

        Try

            Dim ds As DataSet
           
            BindFamilySubFamily()

            BindBusinessProcessAction(False, False)

            ds = commonFunctions.GetBusinessProcessType(0)
            If (commonFunctions.CheckDataset(ds) = True) Then
                ddBusinessProcessType.DataSource = ds
                ddBusinessProcessType.DataTextField = ds.Tables(0).Columns("ddBusinessProcessTypeName").ColumnName
                ddBusinessProcessType.DataValueField = ds.Tables(0).Columns("BusinessProcessTypeID").ColumnName
                ddBusinessProcessType.DataBind()
                'ddBusinessProcessType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If (commonFunctions.CheckDataset(ds) = True) Then
                ddCurrentCommodity.DataSource = ds
                ddCurrentCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName
                ddCurrentCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCurrentCommodity.DataBind()
                ddCurrentCommodity.Items.Insert(0, "")

                ddNewCommodity.DataSource = ds
                ddNewCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName
                ddNewCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddNewCommodity.DataBind()
                ddNewCommodity.Items.Insert(0, "")

                ddWorkFlowCommodity.DataSource = ds
                ddWorkFlowCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName
                ddWorkFlowCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddWorkFlowCommodity.DataBind()
                ddWorkFlowCommodity.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetDesignationType()
            If (commonFunctions.CheckDataset(ds) = True) Then
                ddDesignationType.DataSource = ds
                ddDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName
                ddDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddDesignationType.DataBind()
                'ddDesignationType.Items.Insert(0, "")

                ddCurrentChildDesignationType.DataSource = ds
                ddCurrentChildDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName
                ddCurrentChildDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddCurrentChildDesignationType.DataBind()
                'ddCurrentChildDesignationType.Items.Insert(0, "")

                ddNewChildDesignationType.DataSource = ds
                ddNewChildDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName
                ddNewChildDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddNewChildDesignationType.DataBind()
                'ddNewChildDesignationType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPriceCode("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddPriceCode.DataSource = ds
                ddPriceCode.DataTextField = ds.Tables(0).Columns("ddPriceCodeName").ColumnName
                ddPriceCode.DataValueField = ds.Tables(0).Columns("PriceCode").ColumnName
                ddPriceCode.DataBind()
                ddPriceCode.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProductTechnology(0)
            If (commonFunctions.CheckDataset(ds) = True) Then
                ddCurrentProductTechnology.DataSource = ds
                ddCurrentProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName
                ddCurrentProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddCurrentProductTechnology.DataBind()
                ddCurrentProductTechnology.Items.Insert(0, "")

                ddNewProductTechnology.DataSource = ds
                ddNewProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName
                ddNewProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddNewProductTechnology.DataBind()
                ddNewProductTechnology.Items.Insert(0, "")
            End If

            ds = RFDModule.GetRFDPriority(0)
            If (commonFunctions.CheckDataset(ds) = True) Then
                ddPriority.DataSource = ds
                ddPriority.DataTextField = ds.Tables(0).Columns("ddPriorityName").ColumnName
                ddPriority.DataValueField = ds.Tables(0).Columns("PriorityID").ColumnName
                ddPriority.DataBind()
                'ddPriority.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddCurrentChildPurchasedGood.DataSource = ds
                ddCurrentChildPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddCurrentChildPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddCurrentChildPurchasedGood.DataBind()
                ddCurrentChildPurchasedGood.Items.Insert(0, "")

                ddNewChildPurchasedGood.DataSource = ds
                ddNewChildPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddNewChildPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddNewChildPurchasedGood.DataBind()
                ddNewChildPurchasedGood.Items.Insert(0, "")
            End If

            ds = RFDModule.GetRFDInitiatorList()
            If (commonFunctions.CheckDataset(ds) = True) Then
                ddInitiator.DataSource = ds
                ddInitiator.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName
                ddInitiator.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddInitiator.DataBind()
                'ddInitiator.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProgramMake()
            If commonFunctions.CheckDataset(ds) = True Then
                ddWorkFlowMake.DataSource = ds
                ddWorkFlowMake.DataTextField = ds.Tables(0).Columns("Make").ColumnName.ToString()
                ddWorkFlowMake.DataValueField = ds.Tables(0).Columns("Make").ColumnName
                ddWorkFlowMake.DataBind()
                ddWorkFlowMake.Items.Insert(0, "")
            End If

            'overall status
            ds = RFDModule.GetRFDStatus(0, False)
            If commonFunctions.CheckDataset(ds) = True Then
                ddStatus.DataSource = ds
                ddStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName
                ddStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddStatus.DataBind()
                'ddStatus.Items.Insert(0, "")
            End If

            'approver status
            ds = RFDModule.GetRFDStatus(0, True)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddApprovalStatus.DataSource = ds
                ddApprovalStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName
                ddApprovalStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddApprovalStatus.DataBind()
            End If

            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
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

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub menuTopTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuTopTabs.MenuItemClick

        Try

            menuBottomTabs.StaticMenuItemStyle.CssClass = "tab"
            menuBottomTabs.StaticSelectedStyle.CssClass = "tab"
            menuTopTabs.StaticSelectedStyle.CssClass = "selectedTab"

            mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)

            cddMakes.SelectedValue = Nothing

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub menuBottomTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuBottomTabs.MenuItemClick

        Try
            menuTopTabs.StaticMenuItemStyle.CssClass = "tab"
            menuTopTabs.StaticSelectedStyle.CssClass = "tab"
            menuBottomTabs.StaticSelectedStyle.CssClass = "selectedTab"

            mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)

            cddMakes.SelectedValue = Nothing

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub AdjustApprovalRouting()

        Try

            If (ViewState("BusinessProcessTypeID") <> 2 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True)) _
                Or (ViewState("SubscriptionID") = 4 And ViewState("BusinessProcessTypeID") = 2 And ViewState("isSales") = False And ViewState("isProgramManagement") = False) _
                Or ViewState("isAdmin") = True _
                Or ViewState("InitiatorTeamMemberID") = ViewState("TeamMemberID") _
                Or ViewState("isCosting") = True Then

                If cbAffectsCostSheetOnly.Checked = False Then

                    cbCapitalRequired.Enabled = True
                    cbCostingRequired.Enabled = True
                    cbPackagingRequired.Enabled = True
                    cbPlantControllerRequired.Enabled = True
                    cbProcessRequired.Enabled = True
                    cbProductDevelopmentRequired.Enabled = True
                    cbPurchasingExternalRFQRequired.Enabled = True
                    cbPurchasingRequired.Enabled = True
                    cbQualityEngineeringRequired.Enabled = True
                    cbToolingRequired.Enabled = True
                End If
            End If

            'reset all visible = false
            btnBusinessAwarded.Visible = False
            lblAccountManagerMarker.Visible = False
            lblProgramManagerMarker.Visible = False
            lblBusinessProcessActionMarker.Visible = False
            rfvAccountManager.Enabled = False
            rfvProgramManager.Enabled = False
            tblProgramManager.Visible = False
            rfvBusinessProcessAction.Enabled = False

            If cbAffectsCostSheetOnly.Checked = True Then
                cbCostingRequired.Checked = True
                cbCapitalRequired.Checked = False
                cbPackagingRequired.Checked = False
                cbPlantControllerRequired.Checked = False
                cbProcessRequired.Checked = False
                cbProductDevelopmentRequired.Checked = False
                cbPurchasingExternalRFQRequired.Checked = False
                cbPurchasingRequired.Checked = False
                cbQualityEngineeringRequired.Checked = False
                cbToolingRequired.Checked = False
            Else
 
                If ddBusinessProcessType.SelectedIndex >= 0 Then
                    Select Case ddBusinessProcessType.SelectedValue
                        Case 1 'RFQ 
                            cbCostingRequired.Checked = True
                            cbCostingRequired.Enabled = False
                            cbProductDevelopmentRequired.Checked = True
                            cbProductDevelopmentRequired.Enabled = False
                            cbQualityEngineeringRequired.Checked = True
                            cbQualityEngineeringRequired.Enabled = False
                            ddDesignationType.SelectedValue = "C"
                            ddDesignationType.Enabled = False
                            lblAccountManagerMarker.Visible = True
                            lblProgramManagerMarker.Visible = True
                            lblBusinessProcessActionMarker.Visible = True
                            lblDueDateMarker.Visible = False
                            rfvAccountManager.Enabled = True

                            If ViewState("isSales") = True Or ViewState("isProgramManagement") = True Then
                                rfvProgramManager.Enabled = True
                            End If

                            tblProgramManager.Visible = True
                            rfvBusinessProcessAction.Enabled = True
                            rfvDueDate.Enabled = False
                        Case 2 'RFC
                            cbCostingRequired.Checked = True
                            cbCostingRequired.Enabled = False
                            cbProductDevelopmentRequired.Checked = True
                            cbProductDevelopmentRequired.Enabled = False
                            cbQualityEngineeringRequired.Checked = True
                            cbQualityEngineeringRequired.Enabled = False
                        Case 5 'Going into Service
                            cbCostingRequired.Checked = True
                            cbCostingRequired.Enabled = False
                        Case 6 'End Of Life
                            cbCostingRequired.Checked = True
                            cbCostingRequired.Enabled = False
                        Case 7 'Quote Only
                            cbProductDevelopmentRequired.Checked = True
                            cbCostingRequired.Checked = True
                            cbQualityEngineeringRequired.Checked = False
                            cbQualityEngineeringRequired.Enabled = False
                            cbPurchasingRequired.Checked = False
                            cbPurchasingRequired.Enabled = False
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
    Protected Sub ddBusinessProcessType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddBusinessProcessType.SelectedIndexChanged

        Try
            ClearMessages()
            Dim iBusinessProcessTypeID As Integer = 1 'default RFQ

            If ddBusinessProcessType.SelectedIndex >= 0 Then
                iBusinessProcessTypeID = ddBusinessProcessType.SelectedValue
                ViewState("BusinessProcessTypeID") = iBusinessProcessTypeID

                'sales and program management can edit business process action
                If (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7) And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                    ddBusinessProcessAction.Enabled = ViewState("isEdit")
                End If

                'sales and program management cannot create RFCs
                If ViewState("BusinessProcessTypeID") = 2 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                    'bWrongBusinessProcessType = True

                    'reset values
                    iBusinessProcessTypeID = 1
                    ViewState("BusinessProcessTypeID") = iBusinessProcessTypeID
                    ddBusinessProcessType.SelectedValue = iBusinessProcessTypeID
                End If

                'other team members (not sales, not program management) can NOT create RFQs
                If (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7) And ViewState("isSales") = False And ViewState("isProgramManagement") = False Then
                    'bWrongBusinessProcessType = True

                    'reset values
                    iBusinessProcessTypeID = 2
                    ViewState("BusinessProcessTypeID") = iBusinessProcessTypeID
                    ddBusinessProcessType.SelectedValue = iBusinessProcessTypeID

                    lblAccountManagerMarker.Visible = False
                    lblProgramManagerMarker.Visible = False

                    rfvAccountManager.Enabled = False
                    rfvProgramManager.Enabled = False
                End If
            End If

            Select Case iBusinessProcessTypeID
                Case 1, 7
                    ddDesignationType.SelectedValue = "C"
                    CheckBusinessProcessAction()
                Case 2
                    ddDesignationType.SelectedValue = "R"
                Case Else
                    ddDesignationType.SelectedValue = "C"
            End Select

            AdjustApprovalRouting()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageDescription.Text = lblMessage.Text

    End Sub
    Private Sub ShowHideProgramManager()

        Try
            rfvProgramManager.Enabled = False
            tblProgramManager.Visible = False
            ddProgramManager.Enabled = False

            If ddBusinessProcessType.SelectedValue = 1 Or (ddBusinessProcessType.SelectedValue = 7 And ddBusinessProcessAction.SelectedValue = 10) Then
                If ViewState("isSales") = True Or ViewState("isProgramManagement") = True Then
                    rfvProgramManager.Enabled = True
                End If

                tblProgramManager.Visible = True
                ddProgramManager.Enabled = ViewState("isEdit")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageDescription.Text = lblMessage.Text
        
    End Sub
    Private Sub CheckBusinessProcessAction()

        Try
            If ddBusinessProcessType.SelectedValue = 1 Then 'Customer Driven
                BindBusinessProcessAction(True, False)
            End If

            If ddBusinessProcessType.SelectedValue = 7 Then 'Quote Only
                BindBusinessProcessAction(True, True)
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

    Protected Sub cbAffectsCostSheetOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbAffectsCostSheetOnly.CheckedChanged

        Try
            ClearMessages()

            AdjustApprovalRouting()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub AdjustDesignationType()

        Try
            Dim iTempBusinessProcessActionID As Integer = 0

            'reset all visible = false        
            acFinishedGoodMeasurements.Visible = False
            lblChildPartLink.Text = "Edit Child Part Details / Measurements"
            ddBusinessProcessAction.Visible = False
            ddCurrentCommodity.Enabled = False
            ddCurrentCommodity.Visible = False
            ddCurrentProductTechnology.Enabled = False
            ddCurrentProductTechnology.Visible = False
            ddProductDevelopmentTeamMemberByCommodity.Visible = False
            ddPurchasingTeamMemberByFamily.Visible = False
            ddPurchasingTeamMemberByMake.Visible = False
            ddWorkFlowCommodity.Visible = False
            ddWorkflowFamily.Visible = False
            ddWorkFlowMake.Visible = False
            gvKit.Visible = False
            iBtnCurrentCustomerPartNoSearch.Visible = False
            iBtnCurrentDrawingCopy.Visible = False
            iBtnCurrentDrawingSearch.Visible = False
            iBtnNewDrawingCopy.Visible = False
            iBtnNewDrawingSearch.Visible = False
            hlnkCurrentCustomerDrawingNo.Visible = False
            hlnkCurrentDrawingNo.Visible = False
            lblBusinessProcessAction.Visible = False
            lblCurrentChildPartTitle.Visible = False
            lblCurrentCommodity.Visible = False
            lblCurrentCommodityNote.Visible = False
            lblCurrentCustomerDrawingNo.Visible = False
            lblCurrentCustomerPartName.Visible = False
            lblCurrentCustomerPartNo.Visible = False
            lblCurrentCustomerPartTitle.Visible = False
            lblCurrentCustomerPartMeasurementsTitle.Visible = False
            lblCurrentDesignLevel.Visible = False
            lblCurrentDrawingNo.Visible = False
            lblCurrentProductTechnology.Visible = False
            lblProductDevelopmentTeamMemberByCommodity.Visible = False
            lblProductDevelopmentTeamMemberByCommodityTip.Visible = False
            lblPurchasingTeamMemberByFamily.Visible = False
            lblPurchasingTeamMemberByFamilyTip.Visible = False
            lblPurchasingTeamMemberByMake.Visible = False
            lblPurchasingTeamMemberByMakeTip.Visible = False
            lblWorkFlowCommodity.Visible = False
            lblWorkFlowCommodityMarker.Visible = False
            lblWorkFlowCommodityNote.Visible = False
            lblWorkflowFamily.Visible = False
            lblWorkflowFamilyMarker.Visible = False
            lblWorkFlowMake.Visible = False
            lblWorkFlowMakeMarker.Visible = False
            lblTitleBPCSChildPart.Visible = False
            rfvWorkFlowCommodity.Enabled = False
            rfvWorkflowFamily.Enabled = False
            rfvWorkFlowMake.Enabled = False
            tblChildPart.Visible = False
            tblCustomerPart.Visible = False
            tblCurrentChildPart.Visible = False
            tblCurrentChildPartMeasurements.Visible = False
            tblCurrentCustomerPart.Visible = False
            tblCurrentFGMeasurements.Visible = False
            txtCurrentCustomerDrawingNo.Enabled = False
            txtCurrentCustomerDrawingNo.Visible = False
            txtCurrentCustomerPartName.Enabled = False
            txtCurrentCustomerPartName.Visible = False
            txtCurrentCustomerPartNo.Enabled = False
            txtCurrentCustomerPartNo.Visible = False
            txtCurrentDesignLevel.Enabled = False
            txtCurrentDesignLevel.Visible = False
            txtCurrentDrawingNo.Enabled = False
            txtCurrentDrawingNo.Visible = False

            'reset child labels            
            lblNewChildPartNoLabel.Text = "New Child Part No.:"
            lblNewChildPartNameLabel.Text = "New Child Part Name"
            lblTitleBPCSChildPart.Text = "List of Child Part No(s)"

            '2011-July-06
            'Customer Part tab - new fields section
            txtNewCustomerPartNo.Enabled = False
            txtNewDesignLevel.Enabled = False
            txtNewCustomerDrawingNo.Enabled = False
            txtNewCustomerPartName.Enabled = False
            txtNewDrawingNo.Enabled = False           
            txtNewCostSheetID.Enabled = False         
            txtNewECINo.Enabled = False

            txtNewCapExProjectNo.Enabled = False
            txtNewPONo.Enabled = False
            ddNewCommodity.Enabled = False
            ddNewProductTechnology.Enabled = False

            cbNewECIOverrideNA.Enabled = False

            If ddDesignationType.SelectedIndex >= 0 Then

                ddProductDevelopmentTeamMemberByCommodity.Visible = True
                ddWorkFlowCommodity.Visible = True

                lblProductDevelopmentTeamMemberByCommodity.Visible = True               
                lblProductDevelopmentTeamMemberByCommodityTip.Visible = True
                lblWorkFlowCommodity.Visible = True
                lblWorkFlowCommodityNote.Visible = True
                lblTitleBPCSChildPart.Visible = True

                Select Case ddDesignationType.SelectedValue
                    Case "A", "B", "F", "G", "H", "I", "R", "0", "6"  'all potential child parts 'Semi-Finished Goods

                        If ddBusinessProcessType.SelectedIndex >= 0 Then
                            If ddBusinessProcessType.SelectedValue = 2 Then 'RFC 
                                lblCurrentChildPartTitle.Visible = True
                                tblCurrentChildPart.Visible = True
                                tblCurrentChildPartMeasurements.Visible = True
                            End If

                        End If

                        lblChildPartLink.Text = "Edit Part Details / Measurements"

                        lblNewChildPartNoLabel.Text = "New Part No.:"
                        lblNewChildPartNameLabel.Text = "New Part Name"
                        lblTitleBPCSChildPart.Text = "List of Part No(s) based on Docushare SOP Document: QA 167"

                        If ddDesignationType.SelectedValue = "R" Then
                            ddPurchasingTeamMemberByFamily.Visible = True
                            ddWorkflowFamily.Visible = True

                            lblPurchasingTeamMemberByFamily.Visible = True
                            lblPurchasingTeamMemberByFamilyTip.Visible = True
                            lblWorkflowFamily.Visible = True
                            lblWorkflowFamilyMarker.Visible = True

                            rfvWorkflowFamily.Enabled = True
                        End If

                        menuTopTabs.Items(1).Enabled = False 'kit
                        menuTopTabs.Items(2).Enabled = False 'finished good /customer part

                    Case "C" 'Finished Goods
                        'show customer part info table
                        tblCustomerPart.Visible = True
                        acFinishedGoodMeasurements.Visible = True
                        ddPurchasingTeamMemberByMake.Visible = True
                        ddWorkFlowMake.Visible = True
                        gvKit.Visible = True
                        menuTopTabs.Items(1).Enabled = False 'kit
                        menuTopTabs.Items(2).Enabled = True 'finished good /customer part
                        lblPurchasingTeamMemberByMake.Visible = True
                        lblPurchasingTeamMemberByMakeTip.Visible = True
                        lblWorkFlowMake.Visible = True
                        lblWorkFlowMakeMarker.Visible = True

                        'only make commodity required when the part is a finished good
                        lblWorkFlowCommodityMarker.Visible = True
                        lblWorkFlowCommodityNote.Visible = True
                        If ddProductDevelopmentTeamMemberByCommodity.SelectedIndex < 0 Then
                            rfvWorkFlowCommodity.Enabled = True
                        End If

                        If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 And ViewState("StatusID") <> 8 Then
                            iBtnNewDrawingSearch.Visible = ViewState("isEdit")
                            txtNewCustomerPartNo.Enabled = ViewState("isEdit")
                            txtNewDesignLevel.Enabled = ViewState("isEdit")
                            txtNewCustomerDrawingNo.Enabled = ViewState("isEdit")
                            txtNewCustomerPartName.Enabled = ViewState("isEdit")
                            txtNewDrawingNo.Enabled = ViewState("isEdit")
                            txtNewCostSheetID.Enabled = ViewState("isEdit")
                            txtNewECINo.Enabled = ViewState("isEdit")
                            cbNewECIOverrideNA.Enabled = ViewState("isEdit")
                            txtNewCapExProjectNo.Enabled = ViewState("isEdit")
                            txtNewPONo.Enabled = ViewState("isEdit")
                            ddNewCommodity.Enabled = ViewState("isEdit")
                            ddNewProductTechnology.Enabled = ViewState("isEdit")
                        End If

                        If ddBusinessProcessType.SelectedIndex >= 0 Then
                            If ddBusinessProcessAction.SelectedIndex >= 0 Then
                                iTempBusinessProcessActionID = ddBusinessProcessAction.SelectedValue
                            End If

                            ddCurrentCommodity.Visible = True
                            ddCurrentProductTechnology.Visible = True
                            lblCurrentChildPartTitle.Visible = True
                            lblCurrentCommodity.Visible = True
                            lblCurrentCommodityNote.Visible = True
                            lblCurrentCustomerDrawingNo.Visible = True
                            lblCurrentCustomerPartName.Visible = True
                            lblCurrentCustomerPartNo.Visible = True
                            lblCurrentCustomerPartTitle.Visible = True
                            lblCurrentCustomerPartMeasurementsTitle.Visible = True
                            lblCurrentDesignLevel.Visible = True
                            lblCurrentDrawingNo.Visible = True
                            lblCurrentProductTechnology.Visible = True
                            tblCurrentChildPart.Visible = True
                            tblCurrentChildPartMeasurements.Visible = True
                            tblCurrentCustomerPart.Visible = True
                            tblCurrentFGMeasurements.Visible = True

                            If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 And ViewState("StatusID") <> 8 Then
                                ddCurrentCommodity.Enabled = ViewState("isEdit")
                                ddCurrentProductTechnology.Enabled = ViewState("isEdit")
                                iBtnCurrentDrawingSearch.Visible = ViewState("isEdit")
                                txtCurrentCustomerDrawingNo.Enabled = ViewState("isEdit")
                                txtCurrentCustomerPartName.Enabled = ViewState("isEdit")
                                txtCurrentCustomerPartNo.Enabled = ViewState("isEdit")
                                txtCurrentDesignLevel.Enabled = ViewState("isEdit")
                                txtCurrentDrawingNo.Enabled = ViewState("isEdit")
                            End If

                            txtCurrentCustomerDrawingNo.Visible = True
                            txtCurrentCustomerPartName.Visible = True
                            txtCurrentCustomerPartNo.Visible = True
                            txtCurrentDesignLevel.Visible = True
                            txtCurrentDrawingNo.Visible = True
                        End If
                End Select
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
    Protected Sub ddDesignationType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddDesignationType.SelectedIndexChanged

        ClearMessages()
        AdjustDesignationType()

    End Sub

    Protected Sub InsertUpdateApprovalList()

        Try

            Dim dsCheckSubscription As DataSet
            Dim dsCurrentApprover As DataSet
            Dim dsDefaultApprover As DataSet

            Dim iTempApprovalStatus As Integer = 0
            Dim iWorkflowFamily As Integer = 0

            If ddWorkflowFamily.SelectedIndex > 0 Then
                iWorkflowFamily = ddWorkflowFamily.SelectedValue
            End If

            Dim iTempDefaultTeamMemberID As Integer = 0

            iTempApprovalStatus = 1

            If cbCostingRequired.Checked = True Then
                If ViewState("CostingStatusID") <> 3 And ViewState("CostingStatusID") <> 9 Then
                    'first check if a team member has been assigned yet, if not then insert record
                    dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 6, 0, False, False, False, True, True) 'costing
                    'if no approver found, then insert
                    If commonFunctions.CheckDataSet(dsCurrentApprover) = False Or ViewState("CostingTeamMemberID") = 0 Then
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

                                        If cbAffectsCostSheetOnly.Checked = True Or ViewState("AllApprovedBeforeCosting") = True Then
                                            If ViewState("StatusID") = 2 Then
                                                iTempApprovalStatus = 2
                                            End If

                                            If iTempDefaultTeamMemberID > 0 Then
                                                RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                                            End If
                                        End If

                                    End If
                                End If
                            End If
                        Else
                            lblMessage.Text &= "<br />ERROR: The Default subscription for Costing Coordinator does not have the General Costing subscription, please submit a support requestor."
                        End If
                    Else
                        If ViewState("CostingTeamMemberID") > 0 Then
                            If cbAffectsCostSheetOnly.Checked = True Or ViewState("AllApprovedBeforeCosting") = True Then
                                RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, ViewState("CostingTeamMemberID"), "", 0, 2, Today.Date)
                            Else
                                RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, ViewState("CostingTeamMemberID"), "", 0, 1, "")
                            End If
                        End If
                    End If
                End If  'costing has not approved already             
            Else 'removed costing approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 6)
            End If

            If cbAffectsCostSheetOnly.Checked = True Then
                'removed capital approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 119)

                'removed plant controller approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 20)

                'removed packaging approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 108)

                'removed process approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 66)

                'removed product development approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 5)

                'removed purchasing external rfqapproval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 139)

                'removed purchasing approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 7)

                'removed quality engineering approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 22)

                'removed tooling approval
                RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 65)

                If ViewState("SubscriptionID") <> 6 Then 'costing
                    ResetCurrentApprovalUpdateSection()
                End If
            Else 'update all lists where needed
                iTempApprovalStatus = 1
                iTempDefaultTeamMemberID = 0

                If cbCapitalRequired.Checked = True Then
                    If ViewState("CapitalStatusID") <> 3 Then
                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 119, 0, False, False, False, True, True) '
                        'if no approver found, then insert
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then
                            'get default approver
                            dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(63) 'Capital
                            If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                        iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                        'check if team member still has this subscription
                                        dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 119)
                                        If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                            'insert new record 
                                            RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 119, iTempDefaultTeamMemberID)

                                            If ViewState("StatusID") = 2 Then
                                                iTempApprovalStatus = 2
                                            End If

                                            RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 119, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                                        Else
                                            lblMessage.Text &= "<br />ERROR: The Default subscription for Capital does not have the default subscription, please submit a support requestor."
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Else 'removed plant controller approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 119)
                End If

                iTempApprovalStatus = 1
                iTempDefaultTeamMemberID = 0

                If cbPlantControllerRequired.Checked = True Then
                    If ViewState("PlantControllerStatusID") <> 3 Then
                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 20, 0, False, False, False, True, True) 'Finance
                        'if no approver found, then insert
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then
                            ''get default approver per facility if exists
                            Dim strUGNFacility As String = ""

                            'get first UGN Facility in the list
                            Dim dt As DataTable
                            Dim objRFDFacilityDeptBLL As New RFDFacilityDeptBLL

                            'get first UGN Facility in the list
                            dt = objRFDFacilityDeptBLL.GetRFDFacilityDept(ViewState("RFDNo"))

                            If commonFunctions.CheckDataTable(dt) = False Then
                                strUGNFacility = "UT"
                            Else
                                strUGNFacility = dt.Rows(0).Item("UGNFacility").ToString
                            End If

                            If strUGNFacility = "" Then
                                strUGNFacility = "UT"
                            End If

                            'get default plant controller for that facility
                            dsDefaultApprover = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(109, strUGNFacility)

                            'if facility does not have a default plant controller, then get the corporate
                            If commonFunctions.CheckDataSet(dsDefaultApprover) = False Then
                                dsDefaultApprover = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(109, "UT")
                            End If

                            If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                'check if team member still has this subscription
                                dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 20)
                                If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                    'insert new record                                    
                                    RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 20, iTempDefaultTeamMemberID)
                                    lblMessage.Text &= "<br />Plant Controller approver added to the list."

                                    If ViewState("StatusID") = 2 Then
                                        iTempApprovalStatus = 2
                                    End If

                                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 20, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                                End If
                            Else
                                lblMessage.Text &= "<br />ERROR: The Default subscription for Plant Controller does not have the General Finance subscription, please submit a support requestor."
                            End If
                        End If
                    End If
                Else 'removed plant controller approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 20)
                End If

                iTempDefaultTeamMemberID = 0
                iTempApprovalStatus = 1

                If cbPackagingRequired.Checked = True Then
                    If ViewState("PackagingStatusID") <> 3 Then
                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 108, 0, False, False, False, True, True) 'Packaging
                        'if no approver found, then insert
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then
                            'get default approver
                            dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(110) 'default Packaging
                            If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                        iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                        ''check if team member still has this subscription
                                        dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 108)
                                        If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                            'insert new record                                    
                                            RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 108, iTempDefaultTeamMemberID)
                                            lblMessage.Text &= "<br />Packaging approver added to the list."

                                            If ViewState("StatusID") = 2 Then
                                                iTempApprovalStatus = 2
                                            End If

                                            RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 108, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                                        End If
                                    End If
                                End If
                            Else
                                lblMessage.Text &= "<br />ERROR: The Default subscription for Packaging does not have the General Packaging subscription, please submit a support requestor."
                            End If
                        End If
                    End If
                Else 'removed packaging approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 108)
                End If

                iTempDefaultTeamMemberID = 0
                iTempApprovalStatus = 1

                If cbProcessRequired.Checked = True Then
                    If ViewState("ProcessStatusID") <> 3 Then

                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 66, 0, False, False, False, True, True) 'Process
                        'if no approver found, then insert
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then

                            ''get default approver per facility if exists
                            Dim strUGNFacility As String = ""

                            'get first UGN Facility in the list
                            Dim dt As DataTable
                            Dim objRFDFacilityDeptBLL As New RFDFacilityDeptBLL

                            'get first UGN Facility in the list
                            dt = objRFDFacilityDeptBLL.GetRFDFacilityDept(ViewState("RFDNo"))

                            If commonFunctions.CheckDataTable(dt) = True Then
                                strUGNFacility = dt.Rows(0).Item("UGNFacility").ToString

                                'get 'Process By Facility approver
                                dsDefaultApprover = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(66, strUGNFacility) 'Process By Facility
                                If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                            iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")
                                        End If
                                    End If
                                End If
                            End If

                            If iTempDefaultTeamMemberID = 0 Then
                                dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(60) 'default process
                                If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                                    If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                        If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then 'default approver found
                                            iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                                            'check if team member still has this subscription
                                            dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 60) 'default Process
                                            If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                                lblMessage.Text &= "<br />ERROR: The Default subscription for Process Engineer does not have the General Process Engineer subscription, please submit a support requestor."
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If iTempDefaultTeamMemberID > 0 Then
                                'insert new record                                    
                                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 66, iTempDefaultTeamMemberID)
                                lblMessage.Text &= "<br />Process approver added to the list."

                                If ViewState("StatusID") = 2 Then
                                    iTempApprovalStatus = 2
                                End If

                                If ViewState("ProcessTeamMemberID") <> iTempDefaultTeamMemberID Then
                                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 66, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                                End If
                            End If
                        End If

                    End If
                Else 'removed process approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 66)
                End If

                iTempDefaultTeamMemberID = 0
                iTempApprovalStatus = 1

                If cbProductDevelopmentRequired.Checked = True Then
                    If ViewState("ProductDevelopmentStatusID") <> 3 Then
                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 5, 0, False, False, False, True, True) 'Product Development
                        'if no approver found, then insert
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then
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
                                                lblMessage.Text &= "<br />ERROR: The Default subscription for Product Engineering does not have the General Product Engineering subscription, please submit a support requestor."
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If iTempDefaultTeamMemberID > 0 Then
                                'insert new record                                      
                                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 5, iTempDefaultTeamMemberID)
                                lblMessage.Text &= "<br />Product Engineering approver added to the list."

                                If ViewState("StatusID") = 2 Then
                                    iTempApprovalStatus = 2
                                End If

                                RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 5, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                            End If
                        End If
                    End If
                Else 'removed product development approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 5)
                End If

                iTempDefaultTeamMemberID = 0
                iTempApprovalStatus = 1

                If cbPurchasingExternalRFQRequired.Checked = True Then
                    If ViewState("PurchasingExternalRFQStatusID") <> 3 Then
                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 139, 0, False, False, False, True, True) 'PurchasingExternalRFQ
                        'if no approver found, then insert
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then
                            'first check for PurchasingExternalRFQ assigned to family in workflow
                            If ddWorkflowFamily.SelectedIndex > 0 Then
                                If ddPurchasingTeamMemberByFamily.SelectedIndex > 0 Then 'check to see if user selected team member first
                                    iTempDefaultTeamMemberID = ddPurchasingTeamMemberByFamily.SelectedValue
                                End If

                                If iTempDefaultTeamMemberID = 0 Then  'no team member was selected for this family
                                    dsDefaultApprover = commonFunctions.GetWorkFlowFamilyPurchasingAssignments(0, iWorkflowFamily)
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
                                                lblMessage.Text &= "<br />ERROR: The Default subscription for Purchasing External RFQ does not have the General Purchasing subscription, please submit a support requestor."
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If iTempDefaultTeamMemberID > 0 Then
                                'insert new record                                      
                                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 139, iTempDefaultTeamMemberID)
                                lblMessage.Text &= "<br />PurchasingExternalRFQ approver added to the list."
                            End If
                        End If
                    End If
                Else 'removed purchasing approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 139)
                End If

                iTempDefaultTeamMemberID = 0
                iTempApprovalStatus = 1

                If cbPurchasingRequired.Checked = True Then
                    If ViewState("PurchasingStatusID") <> 3 Then
                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 7, 0, False, False, False, True, True) 'purchasing
                        'if no approver found, then insert
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then
                            'first check for purchasing assigned to family in workflow
                            If ddWorkflowFamily.SelectedIndex > 0 Then
                                If ddPurchasingTeamMemberByFamily.SelectedIndex > 0 Then 'check to see if user selected team member first
                                    iTempDefaultTeamMemberID = ddPurchasingTeamMemberByFamily.SelectedValue
                                End If

                                If iTempDefaultTeamMemberID = 0 Then  'no team member was selected for this family
                                    dsDefaultApprover = commonFunctions.GetWorkFlowFamilyPurchasingAssignments(0, iWorkflowFamily)
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
                                                lblMessage.Text &= "<br />ERROR: The Default subscription for Purchasing does not have the General Purchasing subscription, please submit a support requestor."
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If iTempDefaultTeamMemberID > 0 Then
                                'insert new record                                      
                                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 7, iTempDefaultTeamMemberID)
                                lblMessage.Text &= "<br />Purchasing approver added to the list."

                                If ViewState("AllApprovedBeforePurchasing") = True Then
                                    If ViewState("StatusID") = 2 Then
                                        iTempApprovalStatus = 2
                                    End If

                                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 7, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                                End If

                            End If
                        End If
                    End If
                Else 'removed purchasing approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 7)
                End If

                iTempDefaultTeamMemberID = 0
                iTempApprovalStatus = 1

                If cbQualityEngineeringRequired.Checked = True Then
                    If ViewState("QualityEngineerStatusID") <> 3 Then
                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 22, 0, False, False, False, True, True) 'quality engineering
                        'if no approver found, then insert/update
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then
                            'dsDefaultApprover = commonFunctions.GetProgramMakeWithWorkFlowAssignments

                            'get Quality Engineer by MAKE
                            If ddWorkFlowMake.SelectedIndex > 0 Then
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
                                    lblMessage.Text &= "<br />ERROR: The Default subscription for Quality Engineer does not have the General Quality Engineer subscription, please submit a support requestor."
                                End If
                            End If

                            If iTempDefaultTeamMemberID > 0 Then
                                'check if team member still has this subscription
                                dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 22)
                                If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then
                                    'insert new record
                                    RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 22, iTempDefaultTeamMemberID)
                                    lblMessage.Text &= "<br />Quality Engineer approver added to the list."

                                    If ViewState("AllApprovedBeforeQualityEngineer") = True Then
                                        If ViewState("StatusID") = 2 Then
                                            iTempApprovalStatus = 2
                                        End If

                                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 22, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                                    End If

                                End If
                            End If

                        End If
                    End If
                Else 'removed quality engineering approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 22)
                End If

                iTempDefaultTeamMemberID = 0
                iTempApprovalStatus = 1

                If cbToolingRequired.Checked = True Then
                    If ViewState("ToolingStatusID") <> 3 Then
                        'first check if a team member has been assigned yet, if not then insert record
                        dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 65, 0, False, False, False, True, True) 'tooling
                        'if no approver found, then insert
                        If commonFunctions.CheckDataSet(dsCurrentApprover) = False Then
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
                                lblMessage.Text &= "<br />Tooling approver added to the list."

                                If ViewState("StatusID") = 2 Then
                                    iTempApprovalStatus = 2
                                End If

                                RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 65, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                            End If

                        End If
                    End If
                Else 'removed tooling approval
                    RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 65)
                End If

            End If 'end check if only costing

            gvApproval.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub UpdateRFDCustomerProgramBasedOnDrawing(ByVal DrawingNo As String)

        Try
            Dim ds As DataSet

            Dim iRowCounter As Integer = 0

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            ds = PEModule.GetDrawingCustomerProgram(DrawingNo)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iProgramID = 0
                    If ds.Tables(0).Rows(iRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("ProgramID") > 0 Then
                            iProgramID = ds.Tables(0).Rows(iRowCounter).Item("ProgramID")
                        End If
                    End If

                    iProgramYear = 0
                    If ds.Tables(0).Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("ProgramYear") > 0 Then
                            iProgramYear = ds.Tables(0).Rows(iRowCounter).Item("ProgramYear")
                        End If
                    End If

                    RFDModule.InsertRFDCustomerProgram(ViewState("RFDNo"), False, "", "", iProgramID, iProgramYear, "", "")

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

    End Sub

    Private Sub UpdateRFDVendorBasedOnDrawing(ByVal DrawingNo As String)

        Try
            Dim ds As DataSet

            Dim objRFDVendorBLL As RFDVendorBLL = New RFDVendorBLL

            Dim iRowCounter As Integer = 0

            Dim iUGNDBVendorID As Integer = 0
            Dim bObsolete As Boolean = False

            ds = PEModule.GetDrawingApprovedVendor(DrawingNo)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iUGNDBVendorID = 0
                    If ds.Tables(0).Rows(iRowCounter).Item("UGNDBVendorID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("UGNDBVendorID") > 0 Then
                            iUGNDBVendorID = ds.Tables(0).Rows(iRowCounter).Item("UGNDBVendorID")
                        End If
                    End If

                    bObsolete = False
                    If ds.Tables(0).Rows(iRowCounter).Item("Obsolete") IsNot System.DBNull.Value Then
                        bObsolete = ds.Tables(0).Rows(iRowCounter).Item("Obsolete")
                    End If

                    ' do not insert obsolete vendors
                    If bObsolete = False Then
                        objRFDVendorBLL.InsertRFDVendor(ViewState("RFDNo"), iUGNDBVendorID)
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

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDescription.Click, _
        btnSaveCustomerPartNo.Click, btnSaveFGMeasurements.Click, btnSaveVendor.Click, btnSaveProcess.Click, btnSaveTooling.Click

        Try
            ClearMessages()

            Dim ds As DataSet

            Dim bValidData As Boolean = True
            Dim bFoundObsolete As Boolean = False
            Dim bWrongBusinessProcessType As Boolean = False

            Dim dNewFinishedGoodAMDValue As Double = 0
            Dim dNewFinishedGoodDensityValue As Double = 0
            Dim dNewFinishedGoodWMDValue As Double = 0

            Dim dTargetAnnualSales As Double = 0
            Dim dTargetPrice As Double = 0

            Dim iAccountManagerID As Integer = 0
            Dim iProgramManagerID As Integer = 0

            Dim iBusinessProcessActionID As Integer = 0
            Dim iBusinessProcessTypeID As Integer = 0

            Dim iCostSheetID As Integer = 0
            Dim iCostingTeamMemberID As Integer = 0
            Dim iECINo As Integer = 0

            Dim iFamilyID As Integer = 0
            Dim iInitiatorTeamMemberID As Integer = 0

            Dim iNewInStepTracking As Integer = 0
            Dim iNewCommodityID As Integer = 0
            Dim iNewSubFamilyID As Integer = 0

            Dim iProdDevCommodityTeamMember As Integer = 0
            Dim iPriorityID As Integer = 0
            Dim iPurchasingMakeTeamMemberID As Integer = 0
            Dim iPurchasingFamilyTeamMember As Integer = 0
            Dim iTargetAnnualVolume As Integer = 0
            Dim iNewProductTechnologyID As Integer = 0

            Dim strDesignationType As String = ""
            Dim strMake As String = ""
            Dim strNewFinishedGoodAMDUnits As String = ""
            Dim strNewFinishedGoodWMDUnits As String = ""
            Dim strPriceCode As String = ""

            If ddAccountManager.SelectedIndex > 0 Then
                iAccountManagerID = ddAccountManager.SelectedValue
                If InStr(ddAccountManager.SelectedItem.Text, "**") > 0 Then
                    lblMessage.Text &= "<br />Account Manager is obsolete"
                    bFoundObsolete = True
                End If
            End If

            If ddProgramManager.SelectedIndex > 0 Then
                iProgramManagerID = ddProgramManager.SelectedValue
                If InStr(ddProgramManager.SelectedItem.Text, "**") > 0 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                    lblMessage.Text &= "<br />Program Manager is obsolete"
                    bFoundObsolete = True
                End If
            End If

            If ddInitiator.SelectedIndex >= 0 Then
                iInitiatorTeamMemberID = ddInitiator.SelectedValue
                ViewState("InitiatorTeamMemberID") = iInitiatorTeamMemberID
                If InStr(ddInitiator.SelectedItem.Text, "**") > 0 Then
                    ''Reset with Currect Team Member
                    iInitiatorTeamMemberID = ViewState("TeamMemberID")
                End If
                ViewState("InitiatorTeamMemberID") = iInitiatorTeamMemberID
            End If

            If ddBusinessProcessType.SelectedIndex >= 0 Then
                iBusinessProcessTypeID = ddBusinessProcessType.SelectedValue
                ViewState("BusinessProcessTypeID") = iBusinessProcessTypeID

                If InStr(ddBusinessProcessType.SelectedItem.Text, "**") > 0 Then

                    'adjust for Sales/PM
                    If ViewState("isSales") = True Or ViewState("isProgramManagement") = True Then
                        ViewState("BusinessProcessTypeID") = 1
                        iBusinessProcessTypeID = ViewState("BusinessProcessTypeID")
                        ddBusinessProcessType.SelectedValue = ViewState("BusinessProcessTypeID")
                    Else 'adjust for everyone else
                        ViewState("BusinessProcessTypeID") = 2
                        iBusinessProcessTypeID = ViewState("BusinessProcessTypeID")
                        ddBusinessProcessType.SelectedValue = ViewState("BusinessProcessTypeID")
                    End If

                    'lblMessage.Text &= "<br />Business Process Type is obsolete"
                    'bFoundObsolete = True
                End If

                If iBusinessProcessTypeID <> 1 And iBusinessProcessTypeID <> 7 Then
                    ddBusinessProcessAction.SelectedIndex = -1
                    iBusinessProcessActionID = 0
                End If
            End If

            If ddBusinessProcessAction.SelectedIndex >= 0 Then
                iBusinessProcessActionID = ddBusinessProcessAction.SelectedValue
                If InStr(ddBusinessProcessAction.SelectedItem.Text, "**") > 0 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                    lblMessage.Text &= "<br />Business Process Action is obsolete"
                    bFoundObsolete = True
                End If
            End If

            If ddDesignationType.SelectedIndex >= 0 Then
                strDesignationType = ddDesignationType.SelectedValue
                If InStr(ddDesignationType.SelectedItem.Text, "**") > 0 Then
                    strDesignationType = "C" 'Finished Good
                End If
            End If

            If ddNewCommodity.SelectedIndex > 0 And ddNewCommodity.Visible = True Then
                iNewCommodityID = ddNewCommodity.SelectedValue
                If InStr(ddNewCommodity.SelectedItem.Text, "**") > 0 Then
                    lblMessage.Text &= "<br />Commodity is obsolete on one or more tabs."
                    iNewCommodityID = 0
                End If
            End If

            If ddProductDevelopmentTeamMemberByCommodity.SelectedIndex >= 0 Then
                iProdDevCommodityTeamMember = ddProductDevelopmentTeamMemberByCommodity.SelectedValue
                If InStr(ddProductDevelopmentTeamMemberByCommodity.SelectedItem.Text, "**") > 0 Then
                    iProdDevCommodityTeamMember = ViewState("DefaultProductDevelopmentTeamMemberID")
                End If
            End If

            If ddPurchasingTeamMemberByMake.SelectedIndex >= 0 And ViewState("StatusID") <> 2 Then
                iPurchasingMakeTeamMemberID = ddPurchasingTeamMemberByMake.SelectedValue
                If InStr(ddPurchasingTeamMemberByMake.SelectedItem.Text, "**") > 0 Then
                    iPurchasingMakeTeamMemberID = ViewState("DefaultPurchasingTeamMemberID")
                End If
            End If

            If ddPurchasingTeamMemberByFamily.SelectedIndex >= 0 And ViewState("StatusID") <> 2 Then
                iPurchasingFamilyTeamMember = ddPurchasingTeamMemberByFamily.SelectedValue
                If InStr(ddPurchasingTeamMemberByFamily.SelectedItem.Text, "**") > 0 Then
                    iPurchasingFamilyTeamMember = ViewState("DefaultPurchasingTeamMemberID")
                End If
            End If

            'workflow commodity takes precedence over newcommodity
            If ddWorkFlowCommodity.SelectedIndex > 0 Then
                iNewCommodityID = ddWorkFlowCommodity.SelectedValue
                ddNewCommodity.SelectedValue = iNewCommodityID
                If InStr(ddWorkFlowCommodity.SelectedItem.Text, "**") > 0 And ddWorkFlowCommodity.Visible = True Then
                    'bFoundObsolete = True
                    iNewCommodityID = 0
                End If
            End If

            If ddNewFGAMDUnits.SelectedIndex > 0 Then
                strNewFinishedGoodAMDUnits = ddNewFGAMDUnits.SelectedValue
            End If

            If ddNewFGWMDUnits.SelectedIndex > 0 Then
                strNewFinishedGoodWMDUnits = ddNewFGWMDUnits.SelectedValue
            End If

            If ddNewProductTechnology.SelectedIndex > 0 Then
                iNewProductTechnologyID = ddNewProductTechnology.SelectedValue
                If InStr(ddNewProductTechnology.SelectedItem.Text, "**") > 0 Then
                    iNewProductTechnologyID = 0
                End If
            End If

            If ddNewFGSubFamily.SelectedIndex > 0 Then
                iNewSubFamilyID = ddNewFGSubFamily.SelectedValue
                If InStr(ddNewFGSubFamily.SelectedItem.Text, "**") > 0 Then
                    lblMessage.Text &= "<br />Subfamily is obsolete on one or more tabs."
                    iNewSubFamilyID = 0
                End If

                If iNewSubFamilyID > 0 Then
                    'get left 2 digits of subfamily
                    Dim strFamilyID As String = Left(CType(ddNewFGSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                    If strFamilyID <> "" Then
                        ddNewFGFamily.SelectedValue = CType(strFamilyID, Integer)
                    End If
                End If
            End If

            If ddPriceCode.SelectedIndex >= 0 Then
                strPriceCode = ddPriceCode.SelectedValue
                If InStr(ddPriceCode.SelectedItem.Text, "**") > 0 Then
                    lblMessage.Text &= "<br />Price Code is obsolete on one or more tabs."
                    bFoundObsolete = True
                End If
            End If

            If ddPriority.SelectedIndex >= 0 Then
                iPriorityID = ddPriority.SelectedValue
                If InStr(ddPriority.SelectedItem.Text, "**") > 0 Then
                    iPriorityID = 3
                End If
            End If

            If ddWorkflowFamily.SelectedIndex > 0 And ddWorkflowFamily.Visible = True Then
                iFamilyID = ddWorkflowFamily.SelectedValue
                If InStr(ddWorkflowFamily.SelectedItem.Text, "**") > 0 Then
                    iFamilyID = 0
                End If
            End If

            If ddWorkFlowMake.SelectedIndex > 0 Then
                strMake = ddWorkFlowMake.SelectedValue
                If InStr(ddWorkFlowMake.SelectedItem.Text, "**") > 0 And ddWorkFlowMake.Visible = True Then
                    strMake = ""
                End If
            End If

            If txtNewCostSheetID.Text.Trim <> "" Then
                iCostSheetID = CType(txtNewCostSheetID.Text.Trim, Integer)

                ds = CostingModule.GetCostSheet(iCostSheetID)

                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />ERROR: The new Cost Sheet ID does not exist."
                    bValidData = False
                End If
            End If

            If txtNewECINo.Text.Trim <> "" Then
                iECINo = CType(txtNewECINo.Text.Trim, Integer)

                ds = ECIModule.GetECI(iECINo)

                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />ERROR: The new ECI does not exist."
                    bValidData = False
                End If
            End If

            If txtNewFGAMDValue.Text.Trim <> "" Then
                dNewFinishedGoodAMDValue = CType(txtNewFGAMDValue.Text.Trim, Double)
            End If

            If txtNewFGDensityValue.Text.Trim <> "" Then
                dNewFinishedGoodDensityValue = CType(txtNewFGDensityValue.Text.Trim, Double)
            End If

            If txtNewFGInStepTracking.Text.Trim <> "" Then
                iNewInStepTracking = CType(txtNewFGInStepTracking.Text.Trim, Integer)
            End If

            If txtNewFGWMDValue.Text.Trim <> "" Then
                dNewFinishedGoodWMDValue = CType(txtNewFGWMDValue.Text.Trim, Double)
            End If

            If txtTargetAnnualSales.Text.Trim <> "" Then
                dTargetAnnualSales = CType(txtTargetAnnualSales.Text.Trim, Double)
            End If

            If txtTargetAnnualVolume.Text.Trim <> "" Then
                iTargetAnnualVolume = CType(txtTargetAnnualVolume.Text.Trim, Integer)
            End If

            If txtTargetPrice.Text.Trim <> "" Then
                dTargetPrice = CType(txtTargetPrice.Text.Trim, Double)
            End If

            If txtCurrentDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtCurrentDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = False Then
                    If InStr(lblMessage.Text, "ERROR: The current DMS drawing number does not exist") <= 0 Then
                        lblMessage.Text &= "<br />ERROR: The current DMS drawing number does not exist."
                    End If

                    bValidData = False
                End If
            End If

            If txtNewDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtNewDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = False Then
                    If InStr(lblMessage.Text, "ERROR: The new DMS drawing number does not exist.") <= 0 Then
                        lblMessage.Text &= "<br />ERROR: The new DMS drawing number does not exist."
                    End If

                    bValidData = False
                End If
            End If

            If bValidData = True Then
                If bWrongBusinessProcessType = True Then
                    lblMessage.Text &= "<br />ERROR: Only Sales and Program Management can create the Business Process type of Customer Driven Change (RFQ).<br /> Also UGN Driven Change (RFC) can NOT be created by Sales and Program Management."
                Else
                    If bFoundObsolete = False Then

                        GetTeamMemberInfo()

                        'if current PartNo exists but Current DMS draiwing is blank, then check for DMS Drawing No
                        If txtCurrentCustomerPartNo.Text.Trim <> "" And txtCurrentDrawingNo.Text.Trim = "" Then
                            ds = PEModule.GetDrawingSearch("", 0, "", "", txtCurrentCustomerPartNo.Text.Trim, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, False, "", "", "", 0)
                            If commonFunctions.CheckDataSet(ds) = True Then
                                txtCurrentDrawingNo.Text = ds.Tables(0).Rows(0).Item("DrawingNo").ToString
                            End If
                        End If

                        If txtNewCustomerPartName.Text.Trim = "" And ddDesignationType.SelectedValue = "C" Then
                            txtNewCustomerPartName.Text = txtCurrentCustomerPartName.Text.Trim
                        End If

                        ' if RFDNo exists then merely update all
                        If ViewState("RFDNo") > 0 Then
                            RFDModule.UpdateRFD(ViewState("RFDNo"), txtRFDDesc.Text.Trim, iBusinessProcessActionID, iBusinessProcessTypeID, strDesignationType, strPriceCode, iPriorityID, _
                                 txtDueDate.Text.Trim, iInitiatorTeamMemberID, iAccountManagerID, iProgramManagerID, txtImpactOnUGN.Text.Trim, dTargetPrice, iTargetAnnualVolume, _
                                 dTargetAnnualSales, txtCurrentCustomerPartNo.Text.Trim, txtNewCustomerPartNo.Text, txtCurrentCustomerDrawingNo.Text.Trim, _
                                 txtNewCustomerDrawingNo.Text.Trim, txtCurrentCustomerPartName.Text.Trim, txtNewCustomerPartName.Text.Trim, txtCurrentDesignLevel.Text.Trim, _
                                 txtNewDesignLevel.Text.Trim, txtCurrentDrawingNo.Text.Trim, txtNewDrawingNo.Text.Trim, iNewInStepTracking, dNewFinishedGoodAMDValue, strNewFinishedGoodAMDUnits, txtNewFGAMDTolerance.Text.Trim, _
                                 dNewFinishedGoodWMDValue, strNewFinishedGoodWMDUnits, txtNewFGWMDTolerance.Text.Trim, txtNewFGConstruction.Text.Trim, dNewFinishedGoodDensityValue, _
                                 txtNewFGDensityUnits.Text.Trim, txtNewFGDensityTolerance.Text.Trim, txtNewFGDrawingNotes.Text.Trim, iNewCommodityID, _
                                 iNewProductTechnologyID, iNewSubFamilyID, iFamilyID, strMake, iCostSheetID, iECINo, Not cbNewECIOverrideNA.Checked, _
                                 txtNewCapExProjectNo.Text.Trim, txtNewPONo.Text.Trim, cbAffectsCostSheetOnly.Checked, cbCostingRequired.Checked, _
                                 cbCustomerApprovalRequired.Checked, cbDVPRrequired.Checked, cbPackagingRequired.Checked, cbPlantControllerRequired.Checked, _
                                 cbProcessRequired.Checked, cbProductDevelopmentRequired.Checked, _
                                 cbPurchasingExternalRFQRequired.Checked, cbPurchasingRequired.Checked, _
                                 cbQualityEngineeringRequired.Checked, cbRDrequired.Checked, cbToolingRequired.Checked, _
                                 iProdDevCommodityTeamMember, iPurchasingMakeTeamMemberID, iPurchasingFamilyTeamMember, _
                                 cbPPAP.Checked, txtVendorRequirement.Text.Trim, 0, _
                                 0, cbCapitalRequired.Checked, txtCopyReason.Text.Trim, cbMeetingRequired.Checked, ddisCostReduction.SelectedValue)

                            Dim iCapitalLeadTime As Integer = 0
                            If txtCapitalLeadTime.Text.Trim <> "" Then
                                iCapitalLeadTime = CType(txtCapitalLeadTime.Text.Trim, Integer)
                            End If

                            RFDModule.UpdateRFDCapital(ViewState("RFDNo"), txtCapitalNotes.Text.Trim, iCapitalLeadTime, ddCapitalLeadUnits.SelectedValue)

                            RFDModule.UpdateRFDProcess(ViewState("RFDNo"), txtProcessNotes.Text.Trim)

                            Dim iToolingLeadTime As Integer = 0
                            If txtToolingLeadTime.Text.Trim <> "" Then
                                iToolingLeadTime = CType(txtToolingLeadTime.Text.Trim, Integer)
                            End If

                            RFDModule.UpdateRFDTooling(ViewState("RFDNo"), txtToolingNotes.Text.Trim, iToolingLeadTime, ddToolingLeadUnits.SelectedValue)

                            If ViewState("AllApproved") = True And ViewState("ApproverCount") > 0 Then
                                If (ViewState("BusinessProcessTypeID") <> 2 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True)) _
                                                Or (ViewState("SubscriptionID") = 4 And ViewState("BusinessProcessTypeID") = 2 And ViewState("isSales") = False And ViewState("isProgramManagement") = False) _
                                                Or ViewState("isAdmin") = True _
                                                Or ViewState("InitiatorTeamMemberID") = ViewState("TeamMemberID") _
                                                Or ViewState("isCosting") = True Then

                                    RFDModule.UpdateRFDOverallStatus(ViewState("RFDNo"), 3)

                                    ddStatus.SelectedValue = 3
                                    ViewState("StatusID") = 3

                                    'update history
                                    RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "All required team members have completed the RFD.")
                                End If

                            End If

                        Else 'if RFDNo does not exist, then insert and redirect to populate query string
                            ds = RFDModule.InsertRFD(0, 1, txtRFDDesc.Text.Trim, iBusinessProcessActionID, iBusinessProcessTypeID, strDesignationType, strPriceCode, iPriorityID, _
                                 txtDueDate.Text.Trim, iInitiatorTeamMemberID, iAccountManagerID, iProgramManagerID, txtImpactOnUGN.Text.Trim, dTargetPrice, iTargetAnnualVolume, _
                                 dTargetAnnualSales, iNewCommodityID, iFamilyID, strMake, cbAffectsCostSheetOnly.Checked, cbCostingRequired.Checked, _
                                 cbCustomerApprovalRequired.Checked, cbDVPRrequired.Checked, cbPackagingRequired.Checked, cbPlantControllerRequired.Checked, _
                                 cbProcessRequired.Checked, cbProductDevelopmentRequired.Checked, _
                                 cbPurchasingExternalRFQRequired.Checked, cbPurchasingRequired.Checked, _
                                 cbQualityEngineeringRequired.Checked, cbRDrequired.Checked, cbToolingRequired.Checked, _
                                 iProdDevCommodityTeamMember, iPurchasingMakeTeamMemberID, iPurchasingFamilyTeamMember, _
                                 0, 0, cbCapitalRequired.Checked, txtCopyReason.Text.Trim, cbMeetingRequired.Checked, ddisCostReduction.SelectedValue)

                            If commonFunctions.CheckDataSet(ds) = True Then
                                If ds.Tables(0).Rows(0).Item("NewRFDNo") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("NewRFDNo") > 0 Then
                                        ViewState("RFDNo") = ds.Tables(0).Rows(0).Item("NewRFDNo")
                                        lblRFDNo.Text = ViewState("RFDNo")
                                        ddStatus.SelectedValue = 1
                                        ViewState("StatusID") = 1

                                        'update history
                                        RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Created RFD")
                                    End If
                                End If
                            End If
                        End If

                        'insert / update Approval List based on checkboxes                       
                        InsertUpdateApprovalList()

                        If txtNewDrawingNo.Text.Trim <> "" And ViewState("RFDNo") > 0 And ViewState("isProductDevelopment") = True Then
                            RFDModule.UpdateDrawingCustomerProgramBasedOnRFD(txtNewDrawingNo.Text.Trim, ViewState("RFDNo"))
                        End If

                        'sales insert future part number for Customer Driven Change only
                        If ViewState("isSales") = True Or ViewState("isProgramManagement") = True Then
                            If ViewState("StatusID") <= 2 And txtNewCustomerPartNo.Text.Trim <> "" Then

                                'customer driven change for pre-production or mass production
                                If ViewState("BusinessProcessTypeID") = 1 And (ViewState("BusinessProcessActionID") = 6 Or ViewState("BusinessProcessActionID") = 7) Then
                                    Dim DefaultUser As String = ""
                                    If HttpContext.Current.Request.Cookies("UGNDB_User") IsNot Nothing Then
                                        DefaultUser = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                                    End If

                                    'check PXREF Internal FG Field
                                    Dim bAddFuturePartNo As Boolean = True
                                    Dim objFuturePartNoBLL As New Future_PartNoBLL
                                    If bAddFuturePartNo = True Then
                                        'see if facility or OEM Manufacturer changed
                                        Dim strNewDefaultUGNFacility As String = GetDefaultUGNFacility()
                                        Dim strNewOEMManufacturer As String = GetDefaultOEMManufacturer()

                                        'if New Customer Part No has changed then update it
                                        If ViewState("OrigNewCustomerPartNo") <> "" Then
                                            If ViewState("OrigNewCustomerPartNo") <> txtNewCustomerPartNo.Text.Trim Then
                                                objFuturePartNoBLL.UpdateFuturePartNo(txtNewCustomerPartNo.Text.Trim, txtNewCustomerPartName.Text.Trim, False, strNewDefaultUGNFacility & ":", "", strNewOEMManufacturer & ":", "C:", ViewState("OrigNewCustomerPartNo"), ViewState("OrigUGNFacility"), "", ViewState("OrigOEMManufacturer"), "C")
                                                bAddFuturePartNo = False
                                            End If
                                        End If

                                        'if New Customer Part No has just been defined then insert it
                                        If ViewState("OrigNewCustomerPartNo") = "" Then
                                            EXPModule.InsertFuturePartNo(txtNewCustomerPartNo.Text.Trim, txtNewCustomerPartName.Text.Trim, strNewDefaultUGNFacility, "", strNewOEMManufacturer, "C", ViewState("RFDNo"), DefaultUser)
                                            bAddFuturePartNo = False
                                        End If

                                        'if both the textbox and original do not exist in Future Part Maint then insert it.
                                        'check Future Part Maint for txtNewCustomerPartNo
                                        Dim dtFuturePartNoField As DataTable
                                        dtFuturePartNoField = objFuturePartNoBLL.GetFuturePartNo(txtNewCustomerPartNo.Text.Trim, "", "")

                                        Dim dtFuturePartNoOriginal As DataTable
                                        dtFuturePartNoOriginal = objFuturePartNoBLL.GetFuturePartNo(ViewState("OrigNewCustomerPartNo"), "", "")

                                        If bAddFuturePartNo = True And commonFunctions.CheckDataTable(dtFuturePartNoOriginal) = False And commonFunctions.CheckDataTable(dtFuturePartNoField) = False Then
                                            EXPModule.InsertFuturePartNo(txtNewCustomerPartNo.Text.Trim, txtNewCustomerPartName.Text.Trim, strNewDefaultUGNFacility, "", strNewOEMManufacturer, "C", ViewState("RFDNo"), DefaultUser)
                                        End If

                                        ViewState("OrigNewCustomerPartNo") = txtNewCustomerPartNo.Text.Trim
                                    End If
                                End If
                            End If



                        End If
                    Else
                        lblMessage.Text &= "<br />ERROR: The information could not be saved. Obsolete team members or items can not be saved on new selections."
                End If

                End If 'valid business process type

            End If 'valid data

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = ""
        lblMessageCustomerPartNoBottom.Text = ""
        lblMessageDescription.Text = lblMessage.Text
        lblMessageCustomerPartNoMiddle.Text = lblMessage.Text
        lblMessageVendor.Text = lblMessage.Text

    End Sub

    Protected Sub gvChildPart_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvChildPart.DataBound

        'hide header columns
        If gvChildPart.Rows.Count > 0 Then
            gvChildPart.HeaderRow.Cells(0).Visible = False
            gvChildPart.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Private Sub BindFamilySubFamily()

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetFamily()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCurrentFGFamily.DataSource = ds
                ddCurrentFGFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName
                ddCurrentFGFamily.DataValueField = ds.Tables(0).Columns("familyID").ColumnName
                ddCurrentFGFamily.DataBind()
                ddCurrentFGFamily.Items.Insert(0, "")

                ddNewFGFamily.DataSource = ds
                ddNewFGFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName
                ddNewFGFamily.DataValueField = ds.Tables(0).Columns("familyID").ColumnName
                ddNewFGFamily.DataBind()
                ddNewFGFamily.Items.Insert(0, "")

                ddCurrentChildFamily.DataSource = ds
                ddCurrentChildFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName
                ddCurrentChildFamily.DataValueField = ds.Tables(0).Columns("familyID").ColumnName
                ddCurrentChildFamily.DataBind()
                ddCurrentChildFamily.Items.Insert(0, "")

                ddNewChildFamily.DataSource = ds
                ddNewChildFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName
                ddNewChildFamily.DataValueField = ds.Tables(0).Columns("familyID").ColumnName
                ddNewChildFamily.DataBind()
                ddNewChildFamily.Items.Insert(0, "")

                ddWorkflowFamily.DataSource = ds
                ddWorkflowFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName
                ddWorkflowFamily.DataValueField = ds.Tables(0).Columns("familyID").ColumnName
                ddWorkflowFamily.DataBind()
                ddWorkflowFamily.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCurrentChildSubFamily.DataSource = ds
                ddCurrentChildSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddCurrentChildSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddCurrentChildSubFamily.DataBind()
                ddCurrentChildSubFamily.Items.Insert(0, "")

                ddCurrentFGSubFamily.DataSource = ds
                ddCurrentFGSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddCurrentFGSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddCurrentFGSubFamily.DataBind()
                ddCurrentFGSubFamily.Items.Insert(0, "")

                ddNewChildSubFamily.DataSource = ds
                ddNewChildSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddNewChildSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddNewChildSubFamily.DataBind()
                ddNewChildSubFamily.Items.Insert(0, "")

                ddNewFGSubFamily.DataSource = ds
                ddNewFGSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddNewFGSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddNewFGSubFamily.DataBind()
                ddNewFGSubFamily.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvChildPart_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvChildPart.RowCreated

        'hide first and second column
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
        End If

    End Sub
    Protected Sub GetCurrentChildDrawing()

        Try

            Dim dsCurrentChild As DataSet
            Dim iFirstDashLocation As Integer = 0

            hlnkCurrentChildDrawingNo.Visible = False

            ViewState("CurrentChildDrawingLayoutType") = ""

            If txtCurrentChildDrawingNo.Text.Trim <> "" Then

                dsCurrentChild = PEModule.GetDrawing(txtCurrentChildDrawingNo.Text.Trim)

                If commonFunctions.CheckDataSet(dsCurrentChild) = True Then

                    iBtnCurrentChildDrawingCopy.Visible = ViewState("isEdit")

                    ViewState("CurrentChildDrawingLayoutType") = dsCurrentChild.Tables(0).Rows(0).Item("DrawingLayoutType").ToString

                    iFirstDashLocation = InStr(txtCurrentChildDrawingNo.Text.Trim, "-")
                    txtCurrentChildInitialDimensionAndDensity.Text = Mid$(txtCurrentChildDrawingNo.Text.Trim, iFirstDashLocation + 1, 2)

                    'do not override new values if they exist
                    If txtNewChildInitialDimensionAndDensity.Text.Trim = "" Then
                        txtNewChildInitialDimensionAndDensity.Text = Mid$(txtCurrentChildDrawingNo.Text.Trim, iFirstDashLocation + 1, 2)
                    End If

                    If dsCurrentChild.Tables(0).Rows(0).Item("InStepTracking") IsNot System.DBNull.Value Then
                        If dsCurrentChild.Tables(0).Rows(0).Item("InStepTracking") > 0 Then
                            txtCurrentChildInStepTracking.Text = dsCurrentChild.Tables(0).Rows(0).Item("InStepTracking")
                        End If
                    End If

                    'do not override new values if they exist
                    If txtNewChildInStepTracking.Text.Trim = "" Then
                        txtNewChildInStepTracking.Text = txtCurrentChildInStepTracking.Text.Trim
                    End If

                    If dsCurrentChild.Tables(0).Rows(0).Item("AMDValue") IsNot System.DBNull.Value Then
                        If dsCurrentChild.Tables(0).Rows(0).Item("AMDValue") > 0 Then
                            txtCurrentChildAMDValue.Text = dsCurrentChild.Tables(0).Rows(0).Item("AMDValue")
                        End If
                    End If

                    txtCurrentChildAMDTolerance.Text = dsCurrentChild.Tables(0).Rows(0).Item("AMDTolerance").ToString

                    ddCurrentChildAMDUnits.SelectedValue = dsCurrentChild.Tables(0).Rows(0).Item("AMDUnits").ToString

                    If dsCurrentChild.Tables(0).Rows(0).Item("WMDValue") IsNot System.DBNull.Value Then
                        If dsCurrentChild.Tables(0).Rows(0).Item("WMDValue") > 0 Then
                            txtCurrentChildWMDValue.Text = dsCurrentChild.Tables(0).Rows(0).Item("WMDValue")
                        End If
                    End If

                    txtCurrentChildWMDTolerance.Text = dsCurrentChild.Tables(0).Rows(0).Item("WMDTolerance").ToString

                    ddCurrentChildWMDUnits.SelectedValue = dsCurrentChild.Tables(0).Rows(0).Item("WMDUnits").ToString

                    If dsCurrentChild.Tables(0).Rows(0).Item("DensityValue") IsNot System.DBNull.Value Then
                        If dsCurrentChild.Tables(0).Rows(0).Item("DensityValue") > 0 Then
                            txtCurrentChildDensityValue.Text = dsCurrentChild.Tables(0).Rows(0).Item("DensityValue")
                        End If
                    End If

                    txtCurrentChildDensityTolerance.Text = dsCurrentChild.Tables(0).Rows(0).Item("DensityTolerance").ToString

                    txtCurrentChildDensityUnits.Text = dsCurrentChild.Tables(0).Rows(0).Item("DensityUnits").ToString

                    txtCurrentChildConstruction.Text = dsCurrentChild.Tables(0).Rows(0).Item("Construction").ToString

                    txtCurrentChildDrawingNotes.Text = dsCurrentChild.Tables(0).Rows(0).Item("Notes").ToString

                    ddCurrentChildDesignationType.SelectedValue = "R"
                    If dsCurrentChild.Tables(0).Rows(0).Item("DesignationType") IsNot System.DBNull.Value Then
                        If dsCurrentChild.Tables(0).Rows(0).Item("DesignationType").ToString <> "" Then
                            ddCurrentChildDesignationType.SelectedValue = dsCurrentChild.Tables(0).Rows(0).Item("DesignationType").ToString
                        End If
                    End If

                    If dsCurrentChild.Tables(0).Rows(0).Item("SubFamilyID") IsNot System.DBNull.Value Then
                        If dsCurrentChild.Tables(0).Rows(0).Item("SubFamilyID") > 0 Then
                            'BindFamilySubFamily()

                            ddCurrentChildSubFamily.SelectedValue = dsCurrentChild.Tables(0).Rows(0).Item("SubFamilyID")

                            'get left 2 digits of subfamily
                            Dim strFamilyID As String = Left(CType(ddCurrentChildSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                            If strFamilyID <> "" Then
                                ddCurrentChildFamily.SelectedValue = CType(strFamilyID, Integer)
                            End If
                        End If
                    End If

                    If dsCurrentChild.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                        If dsCurrentChild.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
                            ddCurrentChildPurchasedGood.SelectedValue = dsCurrentChild.Tables(0).Rows(0).Item("PurchasedGoodID")
                        End If
                    End If

                    btnCurrentChildCopyAll.Visible = ViewState("isEdit")

                    btnCurrentChildCopyInitialDimensionAndDensity.Visible = ViewState("isEdit")
                    btnCurrentChildCopyInStepTracking.Visible = ViewState("isEdit")
                    btnCurrentChildCopyAMD.Visible = ViewState("isEdit")
                    btnCurrentChildCopyWMD.Visible = ViewState("isEdit")
                    btnCurrentChildCopyDensity.Visible = ViewState("isEdit")
                    btnCurrentChildCopyConstruction.Visible = ViewState("isEdit")
                    btnCurrentChildCopyNotes.Visible = ViewState("isEdit")
                    btnCurrentChildCopyDesignationType.Visible = ViewState("isEdit")
                    btnCurrentChildCopySubfamily.Visible = ViewState("isEdit")
                    btnCurrentChildCopyPurchasedGood.Visible = ViewState("isEdit")
                    'Else
                    '    ClearChildPartFields()
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub GetChildPartLinks()

        Try
           
            Dim ds As DataSet

            Dim iCostSheetID As Integer = 0
            Dim iECINo As Integer = 0

            hlnkCurrentChildPartBOM.Visible = False
            hlnkCurrentChildBPCSParentParts.Visible = False
            lblCurrentChildPartNo.Text = ""

            If txtCurrentChildPartNo.Text.Trim <> "" Then

                ds = commonFunctions.GetBPCSPartNo(txtCurrentChildPartNo.Text.Trim, "")

                If commonFunctions.CheckDataSet(ds) = True Then
                    lblCurrentChildPartNo.Text = txtCurrentChildPartNo.Text

                    hlnkCurrentChildPartBOM.NavigateUrl = "RFD_Child_Part_BOM.aspx?PartNo=" & txtCurrentChildPartNo.Text.Trim
                    hlnkCurrentChildPartBOM.Visible = True

                    hlnkCurrentChildBPCSParentParts.NavigateUrl = "RFD_Child_Part_Parents.aspx?PartNo=" & txtCurrentChildPartNo.Text.Trim
                    hlnkCurrentChildBPCSParentParts.Visible = True
                End If

            End If

            If txtNewChildPartNameValue.Text.Trim <> "" Then
                lblNewChildPartName.Text = txtNewChildPartNameValue.Text
            End If

            hlnkCurrentChildDrawingNo.NavigateUrl = ""
            hlnkCurrentChildDrawingNo.Visible = False

            If txtCurrentChildDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtCurrentChildDrawingNo.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then
                    hlnkCurrentChildDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & txtCurrentChildDrawingNo.Text
                    hlnkCurrentChildDrawingNo.Visible = True
                End If
            End If

            hlnkNewChildDrawingNo.Visible = False
            hlnkNewChildDrawingNo2.Visible = False
            lblNewChildDrawingNo.Text = ""

            If txtNewChildDrawingNo.Text <> "" Then
                lblNewChildDrawingNo.Text = txtNewChildDrawingNo.Text

                ds = PEModule.GetDrawing(txtCurrentChildDrawingNo.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then
                    hlnkNewChildDrawingNo.Visible = True
                    hlnkNewChildDrawingNo2.Visible = True

                    hlnkNewChildDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & txtNewChildDrawingNo.Text
                    hlnkNewChildDrawingNo2.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & txtNewChildDrawingNo.Text
                End If
            End If

            hlnkNewChildCostSheetID.NavigateUrl = ""
            hlnkNewChildCostSheetID.Visible = False

            hlnkNewChildDieLayout.NavigateUrl = ""
            hlnkNewChildDieLayout.Visible = False

            If txtNewChildCostSheetID.Text.Trim <> "" Then
                iCostSheetID = CType(txtNewChildCostSheetID.Text.Trim, Integer)

                If iCostSheetID > 0 Then
                    ds = CostingModule.GetCostSheet(iCostSheetID)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        hlnkNewChildCostSheetID.NavigateUrl = "~/Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & iCostSheetID.ToString
                        hlnkNewChildCostSheetID.Visible = True

                        If ds.Tables(0).Rows(0).Item("isDieCut") = True Then
                            hlnkNewChildDieLayout.NavigateUrl = "~/Costing/Die_Layout_Preview.aspx?CostSheetID=" & iCostSheetID.ToString
                            hlnkNewChildDieLayout.Visible = True
                        End If
                    End If
                End If
            End If

            hlnkNewChildECINo.NavigateUrl = ""
            hlnkNewChildECINo.Visible = False

            If txtNewChildECINo.Text.Trim <> "" Then
                iECINo = CType(txtNewChildECINo.Text.Trim, Integer)

                If iECINo > 0 Then
                    ds = ECIModule.GetECI(iECINo)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        hlnkNewChildECINo.NavigateUrl = "~/ECI/ECI_Detail.aspx?ECINo=" & txtNewChildECINo.Text.Trim
                        hlnkNewChildECINo.Visible = True
                    End If
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub gvChildPart_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvChildPart.SelectedIndexChanged

        Try
            ClearMessages()

            Dim dsSubFamily As DataSet
            Dim dsNewChild As DataSet
            Dim iFirstDashLocation As Integer = 0

            ClearChildPartFields()

            ViewState("CurrentChildPartRow") = gvChildPart.Rows(gvChildPart.SelectedIndex).Cells(0).Text

            dsNewChild = RFDModule.GetRFDChildPart(ViewState("CurrentChildPartRow"), ViewState("RFDNo"))

            If commonFunctions.CheckDataSet(dsNewChild) = True Then

                'search new child ECINo popup
                If ViewState("isQualityEngineer") = True Or ViewState("isAdmin") = True Then
                    Dim strNewChildECINoClientScript As String = HandleECIPopUps(txtNewChildECINo.ClientID, "CP", ViewState("CurrentChildPartRow"))
                    iBtnNewChildECINoSearch.Attributes.Add("onClick", strNewChildECINoClientScript)
                    iBtnNewChildECINoSearch.Visible = True
                End If

                txtNewChildPartNoValue.Text = dsNewChild.Tables(0).Rows(0).Item("NewPartNo").ToString
                lblNewChildPartNo.Text = dsNewChild.Tables(0).Rows(0).Item("NewPartNo").ToString

                txtNewChildPartNameValue.Text = dsNewChild.Tables(0).Rows(0).Item("NewPartName").ToString
                lblNewChildPartName.Text = dsNewChild.Tables(0).Rows(0).Item("NewPartName").ToString

                txtNewChildDrawingNo.Text = dsNewChild.Tables(0).Rows(0).Item("NewDrawingNo").ToString
                lblNewChildDrawingNo.Text = dsNewChild.Tables(0).Rows(0).Item("NewDrawingNo").ToString

                txtNewChildLeadTime.Text = Format(dsNewChild.Tables(0).Rows(0).Item("NewPartLeadTime"), "##")
                ddNewChildLeadUnits.SelectedValue = dsNewChild.Tables(0).Rows(0).Item("NewPartLeadUnits").ToString

                If dsNewChild.Tables(0).Rows(0).Item("CostSheetID") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("CostSheetID").ToString <> "" Then
                        txtNewChildCostSheetID.Text = dsNewChild.Tables(0).Rows(0).Item("CostSheetID").ToString
                    End If
                End If

                txtNewChildExternalRFQNo.Text = dsNewChild.Tables(0).Rows(0).Item("ExternalRFQNo").ToString
                If dsNewChild.Tables(0).Rows(0).Item("isExternalRFQrequired") IsNot System.DBNull.Value Then
                    cbNewChildExternalRFQNoNA.Checked = Not dsNewChild.Tables(0).Rows(0).Item("isExternalRFQrequired")
                End If

                cbNewChildECIOverrideNA.Checked = False
                If dsNewChild.Tables(0).Rows(0).Item("isECIRequired") IsNot System.DBNull.Value Then
                    cbNewChildECIOverrideNA.Checked = Not dsNewChild.Tables(0).Rows(0).Item("isECIRequired")
                End If

                If dsNewChild.Tables(0).Rows(0).Item("ECINo") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("ECINo").ToString <> "" Then
                        txtNewChildECINo.Text = dsNewChild.Tables(0).Rows(0).Item("ECINo").ToString
                        cbNewChildECIOverrideNA.Checked = False
                    End If
                End If

                txtNewChildPONo.Text = dsNewChild.Tables(0).Rows(0).Item("PurchasingPONo").ToString

                iFirstDashLocation = InStr(txtNewChildDrawingNo.Text.Trim, "-")
                If iFirstDashLocation > 0 Then
                    txtNewChildInitialDimensionAndDensity.Text = Mid$(txtNewChildDrawingNo.Text.Trim, iFirstDashLocation + 1, 2)
                End If

                If dsNewChild.Tables(0).Rows(0).Item("NewInStepTracking") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewInStepTracking") > 0 Then
                        txtNewChildInStepTracking.Text = dsNewChild.Tables(0).Rows(0).Item("NewInStepTracking")
                    End If
                End If

                If dsNewChild.Tables(0).Rows(0).Item("NewAMDValue") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewAMDValue").ToString <> "" Then
                        txtNewChildAMDValue.Text = dsNewChild.Tables(0).Rows(0).Item("NewAMDValue").ToString
                    End If
                End If

                txtNewChildAMDTolerance.Text = dsNewChild.Tables(0).Rows(0).Item("NewAMDTolerance").ToString

                If dsNewChild.Tables(0).Rows(0).Item("NewAMDUnits") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewAMDUnits").ToString <> "" Then
                        ddNewChildAMDUnits.SelectedValue = dsNewChild.Tables(0).Rows(0).Item("NewAMDUnits").ToString
                    End If
                End If

                If dsNewChild.Tables(0).Rows(0).Item("NewWMDValue") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewWMDValue").ToString <> "" Then
                        txtNewChildWMDValue.Text = dsNewChild.Tables(0).Rows(0).Item("NewWMDValue").ToString
                    End If
                End If

                txtNewChildWMDTolerance.Text = dsNewChild.Tables(0).Rows(0).Item("NewWMDTolerance").ToString

                If dsNewChild.Tables(0).Rows(0).Item("NewWMDUnits") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewWMDUnits").ToString <> "" Then
                        ddNewChildWMDUnits.SelectedValue = dsNewChild.Tables(0).Rows(0).Item("NewWMDUnits").ToString
                    End If
                End If

                If dsNewChild.Tables(0).Rows(0).Item("NewDensityValue") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewDensityValue").ToString <> "" Then
                        txtNewChildDensityValue.Text = dsNewChild.Tables(0).Rows(0).Item("NewDensityValue").ToString
                    End If
                End If

                txtNewChildDensityTolerance.Text = dsNewChild.Tables(0).Rows(0).Item("NewDensityTolerance").ToString
                txtNewChildDensityUnits.Text = dsNewChild.Tables(0).Rows(0).Item("NewDensityUnits").ToString

                txtNewChildConstruction.Text = dsNewChild.Tables(0).Rows(0).Item("NewConstruction").ToString
                txtNewChildDrawingNotes.Text = dsNewChild.Tables(0).Rows(0).Item("NewDrawingNotes").ToString

                ddNewChildDesignationType.SelectedValue = "R"
                If dsNewChild.Tables(0).Rows(0).Item("NewDesignationType") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewDesignationType").ToString <> "" Then
                        ddNewChildDesignationType.SelectedValue = dsNewChild.Tables(0).Rows(0).Item("NewDesignationType").ToString
                    End If
                End If

                If dsNewChild.Tables(0).Rows(0).Item("NewSubFamilyID") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewSubFamilyID") > 0 Then
                        'BindFamilySubFamily()

                        dsSubFamily = commonFunctions.GetSubFamily(0)
                        If commonFunctions.CheckDataSet(dsSubFamily) = True Then
                            ddNewChildSubFamily.DataSource = dsSubFamily
                            ddNewChildSubFamily.DataTextField = dsSubFamily.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                            ddNewChildSubFamily.DataValueField = dsSubFamily.Tables(0).Columns("SubFamilyID").ColumnName
                            ddNewChildSubFamily.DataBind()
                            ddNewChildSubFamily.Items.Insert(0, "")
                        End If

                        ddNewChildSubFamily.SelectedValue = dsNewChild.Tables(0).Rows(0).Item("NewSubFamilyID")

                        'get left 2 digits of subfamily
                        Dim strFamilyID As String = Left(CType(ddNewChildSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                        If strFamilyID <> "" Then
                            ddNewChildFamily.SelectedValue = CType(strFamilyID, Integer)
                        End If
                    End If
                End If

                If dsNewChild.Tables(0).Rows(0).Item("NewPurchasedGoodID") IsNot System.DBNull.Value Then
                    If dsNewChild.Tables(0).Rows(0).Item("NewPurchasedGoodID") > 0 Then
                        ddNewChildPurchasedGood.SelectedValue = dsNewChild.Tables(0).Rows(0).Item("NewPurchasedGoodID")
                    End If
                End If

                txtCurrentChildPartNo.Text = dsNewChild.Tables(0).Rows(0).Item("CurrentPartNo").ToString
                lblCurrentChildPartNo.Text = dsNewChild.Tables(0).Rows(0).Item("CurrentPartNo").ToString
                txtCurrentChildPartName.Text = dsNewChild.Tables(0).Rows(0).Item("CurrentPartName").ToString
                txtCurrentChildDrawingNo.Text = dsNewChild.Tables(0).Rows(0).Item("CurrentDrawingNo").ToString

                GetCurrentChildDrawing()

                CompareCurrentAndNewChildDrawing()

                GetChildPartLinks()

                SetAdminControls()

                acChildPart.Visible = True
                acChildPart.SelectedIndex = 0
                btnSaveChild.Text = "Update Child"
                btnCancelChild.Visible = True
                btnCancelChildDetails.Visible = True
                btnSaveChild.Visible = True

            End If

            gvChildPart.Columns(gvChildPart.Columns.Count - 1).Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub gvCustomerProgram_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerProgram.DataBound

        'hide header columns
        If gvCustomerProgram.Rows.Count > 0 Then
            gvCustomerProgram.HeaderRow.Cells(0).Visible = False
            gvCustomerProgram.HeaderRow.Cells(1).Visible = False
            gvCustomerProgram.HeaderRow.Cells(3).Visible = False
            gvCustomerProgram.HeaderRow.Cells(8).Visible = False
        End If

    End Sub

    Protected Sub gvCustomerProgram_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomerProgram.RowCreated

        'hide columns
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(3).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(8).Attributes.CssStyle.Add("display", "none")
        End If

    End Sub

    Protected Sub gvCustomerProgram_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvCustomerProgram.RowDeleted

        Try

            ClearMessages()

            'if NewDrawingNo exists, then synchronize customer program lists
            If txtNewDrawingNo.Text.Trim <> "" And ViewState("RFDNo") > 0 Then
                RFDModule.UpdateDrawingCustomerProgramBasedOnRFD(txtNewDrawingNo.Text.Trim, ViewState("RFDNo"))
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

    Protected Sub gvCustomerProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerProgram.SelectedIndexChanged

        Try

            ClearMessages()


            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0
            Dim iRowCounter As Integer = 0

            tblMakes.Visible = False

            ViewState("CurrentCustomerProgramRow") = gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(0).Text

            ViewState("CurrentCustomerProgramID") = gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(3).Text

            If Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(5).Text, "&nbsp;", "") <> "" Then
                iProgramYear = CType(Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(5).Text, "&nbsp;", ""), Integer)
                If iProgramYear > 0 Then
                    ddYear.SelectedValue = iProgramYear
                End If
            End If

            txtSOPDate.Text = Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(6).Text.Trim, "&nbsp;", "")
            txtEOPDate.Text = Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(7).Text.Trim, "&nbsp;", "")


            cbCustomerApprovalRequired.Checked = CType(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(8).Controls(0), CheckBox).Checked
            txtCustomerApprovalDate.Text = Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(9).Text.Trim, "&nbsp;", "")
            txtCustomerApprovalNo.Text = Replace(gvCustomerProgram.Rows(gvCustomerProgram.SelectedIndex).Cells(10).Text.Trim, "&nbsp;", "")

            btnSaveCustomerProgram.Text = "Update Customer/Program"
            btnCancelCustomerProgram.Visible = True
            btnGetPlanningForecastingVehicle.Visible = False

            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = False

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

    Private Sub ClearChildPartFields()

        Try

            ViewState("CurrentChildPartRow") = 0

            txtNewChildPartNoValue.Text = ""
            lblNewChildPartNo.Text = ""

            txtNewChildPartNameValue.Text = ""
            lblNewChildPartName.Text = ""

            txtNewChildDrawingNo.Text = ""
            lblNewChildDrawingNo.Text = ""

            hlnkNewChildDrawingNo.Visible = False
            hlnkNewChildDrawingNo2.Visible = False

            hlnkNewChildDrawingNo.NavigateUrl = ""
            hlnkNewChildDrawingNo2.NavigateUrl = ""

            txtNewChildCostSheetID.Text = ""
            hlnkNewChildCostSheetID.NavigateUrl = ""
            hlnkNewChildCostSheetID.Visible = False

            hlnkNewChildDieLayout.NavigateUrl = ""
            hlnkNewChildDieLayout.Visible = False

            txtNewChildExternalRFQNo.Text = ""
            cbNewChildExternalRFQNoNA.Checked = True

            txtNewChildLeadTime.Text = ""
            ddNewChildLeadUnits.SelectedIndex = -1

            txtNewChildECINo.Text = ""
            hlnkNewChildECINo.NavigateUrl = ""
            hlnkNewChildECINo.Visible = False
            cbNewChildECIOverrideNA.Checked = False

            txtNewChildPONo.Text = ""

            lblCurrentChildPartNo.Text = ""
            txtCurrentChildPartNo.Text = ""

            txtCurrentChildPartName.Text = ""
            txtCurrentChildDrawingNo.Text = ""

            txtCurrentChildInitialDimensionAndDensity.Text = ""
            txtCurrentChildInStepTracking.Text = ""

            txtCurrentChildAMDValue.Text = ""
            txtCurrentChildAMDTolerance.Text = ""
            ddCurrentChildAMDUnits.SelectedIndex = -1

            txtCurrentChildWMDValue.Text = ""
            txtCurrentChildWMDTolerance.Text = ""
            ddCurrentChildWMDUnits.SelectedIndex = -1

            txtCurrentChildDensityValue.Text = ""
            txtCurrentChildDensityTolerance.Text = ""
            txtCurrentChildDensityUnits.Text = ""

            txtCurrentChildConstruction.Text = ""
            txtCurrentChildDrawingNotes.Text = ""
            ddCurrentChildDesignationType.SelectedIndex = -1
            ddCurrentChildSubFamily.SelectedIndex = -1
            ddCurrentChildFamily.SelectedIndex = -1
            ddCurrentChildPurchasedGood.SelectedIndex = -1

            txtNewChildInitialDimensionAndDensity.Text = ""
            txtNewChildInStepTracking.Text = ""

            txtNewChildAMDValue.Text = ""
            txtNewChildAMDTolerance.Text = ""
            ddNewChildAMDUnits.SelectedIndex = -1

            txtNewChildWMDValue.Text = ""
            ddNewChildWMDUnits.SelectedIndex = -1
            txtNewChildWMDTolerance.Text = ""

            txtNewChildDensityValue.Text = ""
            txtNewChildDensityTolerance.Text = ""
            txtNewChildDensityUnits.Text = ""

            txtNewChildConstruction.Text = ""
            txtNewChildDrawingNotes.Text = ""

            ddNewChildDesignationType.SelectedIndex = -1
            ddNewChildSubFamily.SelectedIndex = -1
            ddNewChildFamily.SelectedIndex = -1
            ddNewChildPurchasedGood.SelectedIndex = -1

            txtNewChildInitialDimensionAndDensity.BackColor = Color.White
            txtNewChildInStepTracking.BackColor = Color.White

            txtNewChildAMDValue.BackColor = Color.White
            txtNewChildAMDTolerance.BackColor = Color.White
            ddNewChildAMDUnits.BackColor = Color.White

            txtNewChildWMDValue.BackColor = Color.White
            txtNewChildWMDTolerance.BackColor = Color.White
            ddNewChildWMDUnits.BackColor = Color.White

            txtNewChildDensityValue.BackColor = Color.White
            txtNewChildDensityTolerance.BackColor = Color.White
            txtNewChildDensityUnits.BackColor = Color.White

            txtNewChildConstruction.BackColor = Color.White
            txtNewChildDrawingNotes.BackColor = Color.White

            ddNewChildDesignationType.BackColor = Color.White
            ddNewChildSubFamily.BackColor = Color.White
            ddNewChildFamily.BackColor = Color.White
            ddNewChildPurchasedGood.BackColor = Color.White

            btnSaveChild.Text = "Add Child"

            btnSaveChild.Visible = False
            btnSaveChildDetails.Visible = False
            If btnSaveChild.Text = "Update Child" _
            Or ViewState("InitiatorTeamMemberID") = ViewState("TeamMemberID") _
            Or ViewState("isProductDevelopment") = True Then
                btnSaveChild.Visible = ViewState("isEdit")
                btnSaveChildDetails.Visible = ViewState("isEdit")
            End If

            gvChildPart.Columns(gvChildPart.Columns.Count - 1).Visible = ViewState("isEdit")

            btnCurrentChildCopyAll.Visible = False

            btnCurrentChildCopyInitialDimensionAndDensity.Visible = False
            btnCurrentChildCopyInStepTracking.Visible = False
            btnCurrentChildCopyAMD.Visible = False
            btnCurrentChildCopyWMD.Visible = False
            btnCurrentChildCopyDensity.Visible = False
            btnCurrentChildCopyConstruction.Visible = False
            btnCurrentChildCopyNotes.Visible = False
            btnCurrentChildCopyDesignationType.Visible = False
            btnCurrentChildCopySubfamily.Visible = False
            btnCurrentChildCopyPurchasedGood.Visible = False

            iBtnCurrentChildDrawingCopy.Visible = False
            iBtnNewChildECINoSearch.Visible = False

            btnGenerateNewChildDrawing.Visible = False
            rbGenerateNewChildDrawing.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub btnCancelChild_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelChild.Click, btnCancelChildDetails.Click

        Try
            ClearMessages()

            ClearChildPartFields()

            acChildPart.Visible = False
            acChildPart.SelectedIndex = -1

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub gvFacilityDept_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFacilityDept.DataBound

        'hide header columns
        If gvFacilityDept.Rows.Count > 0 Then
            gvFacilityDept.HeaderRow.Cells(0).Visible = False
            gvFacilityDept.HeaderRow.Cells(1).Visible = False
        End If

    End Sub
    Private Sub UpdatePlantControllerApprover(ByVal UGNFacility As String)

        Try

            Dim dsDefaultApprover As DataSet
            Dim dsCheckSubscription As DataSet

            Dim iTempApprovalStatus As Integer = 1
            Dim iTempDefaultTeamMemberID As Integer = 0

            If cbPlantControllerRequired.Checked = True Then

                If UGNFacility = "" Then
                    UGNFacility = "UT"
                End If

                'get default plant controller for that facility
                dsDefaultApprover = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(109, UGNFacility)

                If commonFunctions.CheckDataSet(dsDefaultApprover) = False Then                    
                    dsDefaultApprover = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(109, "UT")
                End If

                If commonFunctions.CheckDataSet(dsDefaultApprover) = True Then
                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                    If ViewState("PlantControllerTeamMemberID") <> iTempDefaultTeamMemberID Then
                        'check if team member still has this subscription
                        dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, 20)
                        If commonFunctions.CheckDataSet(dsCheckSubscription) = True Then

                            'if team memberid is different, then switch it
                            RFDModule.DeleteRFDApprovalStatus(ViewState("RFDNo"), 20)

                            'insert new record                                    
                            RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 20, iTempDefaultTeamMemberID)                            

                            If ViewState("StatusID") = 2 Then
                                iTempApprovalStatus = 2
                            End If

                            RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 20, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                        End If
                    End If

                Else
                    lblMessage.Text &= "<br />ERROR: The Default subscription for Plant Controller does not have the General Finance subscription, please submit a support requestor."
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

    Protected Sub gvFacilityDept_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFacilityDept.RowCommand

        Try

            ClearMessages()

            Dim ddFacilityTemp As DropDownList
            Dim ddDepartmentTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("RFDNo") > 0) Then

                ddFacilityTemp = CType(gvFacilityDept.FooterRow.FindControl("ddInsertFacility"), DropDownList)
                ddDepartmentTemp = CType(gvFacilityDept.FooterRow.FindControl("ddInsertDepartment"), DropDownList)

                If ddFacilityTemp.SelectedIndex >= 0 Then
                    odsFacilityDept.InsertParameters("RFDNo").DefaultValue = ViewState("RFDNo")
                    odsFacilityDept.InsertParameters("UGNFacility").DefaultValue = ddFacilityTemp.SelectedValue
                    odsFacilityDept.InsertParameters("DepartmentID").DefaultValue = ddDepartmentTemp.SelectedValue

                    intRowsAffected = odsFacilityDept.Insert()

                    lblMessage.Text &= "Record Saved Successfully.<br />"

                    UpdatePlantControllerApprover(ddFacilityTemp.SelectedValue)
                Else
                    lblMessage.Text &= "ERROR: the UGN Facility is required.<br />"
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFacilityDept.ShowFooter = False
            Else
                gvFacilityDept.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddFacilityTemp = CType(gvFacilityDept.FooterRow.FindControl("ddInsertFacility"), DropDownList)
                ddFacilityTemp.SelectedIndex = -1

                ddDepartmentTemp = CType(gvFacilityDept.FooterRow.FindControl("ddInsertDepartment"), DropDownList)
                ddDepartmentTemp.SelectedIndex = -1
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageFacilityDepartment.Text = lblMessage.Text

    End Sub

    Protected Sub gvFacilityDept_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFacilityDept.RowCreated

        Try
            'hide columns
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FacilityDept
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvKit_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvKit.DataBound

        'hide header columns
        If gvKit.Rows.Count > 0 Then
            gvKit.HeaderRow.Cells(0).Visible = False
            gvKit.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Protected Sub gvKit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvKit.RowCommand

        Try

            ClearMessages()

            Dim txtKitPartNoTemp As TextBox
            Dim txtKitPartRevisionTemp As TextBox
            Dim txtFinishedGoodPartNoTemp As TextBox
            Dim txtFinishedGoodPartRevisionTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("RFDNo") > 0) Then

                txtKitPartNoTemp = CType(gvKit.FooterRow.FindControl("txtInsertKitPartNo"), TextBox)
                txtKitPartRevisionTemp = CType(gvKit.FooterRow.FindControl("txtInsertKitPartRevision"), TextBox)
                txtFinishedGoodPartNoTemp = CType(gvKit.FooterRow.FindControl("txtInsertFinishedGoodPartNo"), TextBox)
                txtFinishedGoodPartRevisionTemp = CType(gvKit.FooterRow.FindControl("txtInsertFinishedGoodPartRevision"), TextBox)

                odsKit.InsertParameters("RFDNo").DefaultValue = ViewState("RFDNo")
                odsKit.InsertParameters("KitPartNo").DefaultValue = txtKitPartNoTemp.Text.Trim
                odsKit.InsertParameters("KitPartRevision").DefaultValue = txtKitPartRevisionTemp.Text.Trim
                odsKit.InsertParameters("FinishedGoodPartNo").DefaultValue = txtFinishedGoodPartNoTemp.Text.Trim
                odsKit.InsertParameters("FinishedGoodPartRevision").DefaultValue = txtFinishedGoodPartRevisionTemp.Text.Trim

                intRowsAffected = odsKit.Insert()

                lblMessage.Text = "Record Saved Successfully.<br />"

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvKit.ShowFooter = False
            Else
                gvKit.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtKitPartNoTemp = CType(gvKit.FooterRow.FindControl("txtInsertKitPartNo"), TextBox)
                txtKitPartNoTemp.Text = ""

                txtKitPartRevisionTemp = CType(gvKit.FooterRow.FindControl("txtInsertKitPartRevision"), TextBox)
                txtKitPartRevisionTemp.Text = ""

                txtFinishedGoodPartNoTemp = CType(gvKit.FooterRow.FindControl("txtInsertFinishedGoodPartNo"), TextBox)
                txtFinishedGoodPartNoTemp.Text = ""

                txtFinishedGoodPartRevisionTemp = CType(gvKit.FooterRow.FindControl("txtInsertFinishedGoodPartRevision"), TextBox)
                txtFinishedGoodPartRevisionTemp.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageKIT.Text = lblMessage.Text

    End Sub

    Protected Sub gvKit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvKit.RowCreated

        Try

            'hide data and footer columns
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Kit
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub CheckSupportingDocGrid()

        If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 And ViewState("StatusID") <> 8 And ViewState("isEdit") = True Then
            Dim bSupportingDocCountMaximum As Boolean = isSupportingDocCountMaximum()
            lblFileUploadLabel.Visible = Not bSupportingDocCountMaximum
            fileUploadSupportingDoc.Visible = Not bSupportingDocCountMaximum
            btnSaveUploadSupportingDocument.Visible = Not bSupportingDocCountMaximum
        End If

    End Sub
    Protected Sub gvSupportingDoc_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSupportingDoc.DataBound

        'hide header of columns
        If gvSupportingDoc.Rows.Count > 0 Then
            gvSupportingDoc.HeaderRow.Cells(0).Visible = False
            gvSupportingDoc.HeaderRow.Cells(2).Visible = False
        End If

        CheckSupportingDocGrid()

    End Sub

    Protected Sub gvSupportingDoc_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupportingDoc.RowCreated

        'hide columns
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
        End If

    End Sub

    Protected Sub gvVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVendor.DataBound

        'hide header columns
        If gvVendor.Rows.Count > 0 Then
            gvVendor.HeaderRow.Cells(0).Visible = False
            gvVendor.HeaderRow.Cells(1).Visible = False
        End If

    End Sub
    Private Property LoadDataEmpty_FacilityDept() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FacilityDept") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FacilityDept"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FacilityDept") = value
        End Set

    End Property
    Protected Sub odsFacilityDept_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFacilityDept.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As RFD.RFDFacilityDept_MaintDataTable = CType(e.ReturnValue, RFD.RFDFacilityDept_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FacilityDept = True
            Else
                LoadDataEmpty_FacilityDept = False
            End If
        End If

    End Sub
    Private Property LoadDataEmpty_Vendor() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Vendor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Vendor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Vendor") = value
        End Set

    End Property

    Protected Sub odsVendor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsVendor.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        'Console.WriteLine(e.ReturnValue)

        Dim dt As RFD.RFDVendor_MaintDataTable = CType(e.ReturnValue, RFD.RFDVendor_MaintDataTable)
  
        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Vendor = True
            Else
                LoadDataEmpty_Vendor = False
            End If
        End If

    End Sub

    Protected Sub gvVendor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVendor.RowCommand

        Try

            ClearMessages()

            Dim ddVendorTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("RFDNo") > 0) Then

                ddVendorTemp = CType(gvVendor.FooterRow.FindControl("ddInsertVendor"), DropDownList)

                odsVendor.InsertParameters("RFDNo").DefaultValue = ViewState("RFDNo")
                odsVendor.InsertParameters("UGNDBVendorID").DefaultValue = ddVendorTemp.SelectedValue

                intRowsAffected = odsVendor.Insert()

                lblMessage.Text = "Record Saved Successfully.<br />"

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvVendor.ShowFooter = False
            Else
                gvVendor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddVendorTemp = CType(gvVendor.FooterRow.FindControl("ddInsertVendor"), DropDownList)
                ddVendorTemp.SelectedIndex = -1
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageVendor.Text = lblMessage.Text

    End Sub
    Protected Sub gvVendor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVendor.RowCreated

        Try

            'hide columns
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Vendor
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Function SendEmail(ByVal EmailToAddress As String, ByVal EmailCCAddress As String, ByVal EmailSubject As String, ByVal EmailBody As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try

            Dim strSubject As String = ""
            Dim strBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strDrawingURL As String = strProdOrTestEnvironment & "PE/DMSDrawingPreview.aspx?DrawingNo="
            Dim strCostingURL As String = strProdOrTestEnvironment & "Costing/Cost_Sheet_Preview.aspx?CostSheetID="
            Dim strECIURL As String = strProdOrTestEnvironment & "ECI/ECI_Preview.aspx?ECINo="
            Dim strSupportingDocURL As String = strProdOrTestEnvironment & "RFD/RFD_Supporting_Doc_View.aspx?RowID="

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            Dim strEmailToAddress As String = commonFunctions.CleanEmailList(EmailToAddress)
            Dim strEmailCCAddress As String = commonFunctions.CleanEmailList(EmailCCAddress)

            'handle test environment
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
            End If

            strSubject &= EmailSubject
            strBody &= EmailBody

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''' Need links for DMS Drawings, Cost Sheets, ECIs, and Supporting Documents
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            strBody &= "<table style='border: 1px solid #D0D0BF; width: 100%'>"

            If txtNewCustomerPartNo.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Customer Part No.: </strong></font></td><td><font size='1' face='Verdana'>" & txtNewCustomerPartNo.Text.Trim & " <i>(assigned from customer)</i></font></td></tr>"
            End If

            If txtNewDesignLevel.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Design Level:</strong></font></td><td><font size='1' face='Verdana'>" & txtNewDesignLevel.Text.Trim & " <i>(assigned from customer)</i>.</strong></font></td></tr>"
            End If

            If txtNewCustomerDrawingNo.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Customer Drawing No.: </strong></font></td><td><font size='1' face='Verdana'>" & txtNewCustomerDrawingNo.Text.Trim & "</strong></font></td></tr>"
            End If

            If txtNewCustomerPartName.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Customer Part Name: </strong></font></td><td><font size='1' face='Verdana'>" & txtNewCustomerPartName.Text.Trim & "</strong></font></td></tr>"
            End If

            If txtNewDrawingNo.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Finished Good DMS Drawing No.: </strong></font></td><td><font size='1' face='Verdana'><a href=" & strDrawingURL & txtNewDrawingNo.Text.Trim & ">" & txtNewDrawingNo.Text.Trim & "</a></strong></font></td></tr>"
            End If

            If txtNewCostSheetID.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Finished Good Cost Sheet ID: </strong></font></td><td><font size='1' face='Verdana'><a href= " & strCostingURL & txtNewCostSheetID.Text.Trim & ">" & txtNewCostSheetID.Text.Trim & "</a></strong></font></td></tr>"
            End If

            If txtNewECINo.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Finished Good ECI No: </strong></font></td><td><font size='1' face='Verdana'><a href=" & strECIURL & txtNewECINo.Text.Trim & ">" & txtNewECINo.Text.Trim & "</a></strong></font></td></tr>"
            End If

            If txtNewCapExProjectNo.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Finished Good CapEx Tooling No.: </strong></font></td><td><font size='1' face='Verdana'>" & txtNewCapExProjectNo.Text.Trim & "</strong></font></td></tr>"
            End If

            If txtNewPONo.Text.Trim <> "" Then
                strBody &= "<tr ><td bgcolor='#EBEBEB' width='25%'><font size='1' face='Verdana'><strong> New Finished Good P.O. No.: </strong></font></td><td><font size='1' face='Verdana'>" & txtNewPONo.Text.Trim & "</strong></font></td></tr>"
            End If

            strBody &= "</table>"

            Dim objNewFinishedGoodList As RFDFinishedGoodBLL = New RFDFinishedGoodBLL
            Dim dt As DataTable
            Dim iRowCounter As Integer = 0

            dt = objNewFinishedGoodList.GetRFDFinishedGood(ViewState("RFDNo"))
            If commonFunctions.CheckDataTable(dt) = True Then

                strBody &= "<br /><br /><font size='1' face='Verdana'>Finished Good PartNo List based on Customer Part No.</font>"
                strBody &= "<font size='1' face='Verdana'><i>(If Drawing, Cost Sheet ID, etc.. is blank, then it matches the information above.)</i></font>"
                strBody &= "<table style='border: 1px solid #D0D0BF; width: 100%'>"

                strBody &= "<tr bgcolor='#EBEBEB'>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>New F.G. Part No.</b></font></td>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>New Revision</b></font></td>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>Name</b></font></td>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>DMS Drawing No.</b></font></td>"
                strBody &= "<td align='center'><font size='1' face='Verdana'><b>Cost Sheet ID</b></font></td>"
                strBody &= "<td align='center'><font size='1' face='Verdana'><b>ECI No.</b></font></td>"
                strBody &= "<td align='center'><font size='1' face='Verdana'><b>CapEx Project No.</b></font></td>"
                strBody &= "<td align='center'><font size='1' face='Verdana'><b>P.O. No.</b></font></td>"
                strBody &= "</tr>"

                For iRowCounter = 0 To dt.Rows.Count - 1

                    strBody &= "<tr>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("PartNo").ToString & "</font></td>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("PartRevision").ToString & "</font></td>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("PartName").ToString & "</font></td>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'><a href=" & strDrawingURL & dt.Rows(iRowCounter).Item("DrawingNo").ToString & ">" & dt.Rows(iRowCounter).Item("DrawingNo").ToString & "</a></font></td>"
                    strBody &= "<td align='center'><font size='1' face='Verdana'><a href=" & strCostingURL & dt.Rows(iRowCounter).Item("CostSheetID").ToString & ">" & dt.Rows(iRowCounter).Item("CostSheetID").ToString & "</a></font></td>"
                    strBody &= "<td align='center'><font size='1' face='Verdana'><a href=" & strECIURL & dt.Rows(iRowCounter).Item("ECINo").ToString & ">" & dt.Rows(iRowCounter).Item("ECINo").ToString & "</a></font></td>"
                    strBody &= "<td align='center'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("CapExProjectNo").ToString & "</font></td>"
                    strBody &= "<td align='center'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("PurchasingPONo").ToString & "</font></td>"
                    strBody &= "</tr>"
                Next

                strBody &= "</table>"
            End If

            Dim objRFDChildPartBLL As RFDChildPartBLL = New RFDChildPartBLL

            dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))
            If commonFunctions.CheckDataTable(dt) = True Then

                strBody &= "<br /><br /><font size='1' face='Verdana'>Child PartNo List</font>"
                strBody &= "<table style='border: 1px solid #D0D0BF; width: 100%'>"

                strBody &= "<tr bgcolor='#EBEBEB'>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>Current Part No.</b></font></td>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>Current DMS Drawing No.</b></font></td>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>New Part No.</b></font></td>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>Name</b></font></td>"
                strBody &= "<td align='left'><font size='1' face='Verdana'><b>New DMS Drawing No.</b></font></td>"
                strBody &= "<td align='center'><font size='1' face='Verdana'><b>Cost Sheet ID</b></font></td>"
                strBody &= "<td align='center'><font size='1' face='Verdana'><b>External RFQ#</b></font></td>"
                strBody &= "<td align='center'><font size='1' face='Verdana'><b>ECI No.</b></font></td>"
                strBody &= "<td align='center'><font size='1' face='Verdana'><b>P.O. No.</b></font></td>"
                strBody &= "</tr>"

                For iRowCounter = 0 To dt.Rows.Count - 1

                    strBody &= "<tr>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("CurrentPartNo") & "</font></td>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'><a href=" & strDrawingURL & dt.Rows(iRowCounter).Item("CurrentDrawingNo").ToString & ">" & dt.Rows(iRowCounter).Item("CurrentDrawingNo").ToString & "</a></font></td>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("NewPartNo") & "</font></td>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("NewPartName") & "</font></td>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'><a href=" & strDrawingURL & dt.Rows(iRowCounter).Item("NewDrawingNo").ToString & ">" & dt.Rows(iRowCounter).Item("NewDrawingNo").ToString & "</a></font></td>"
                    strBody &= "<td align='center'><font size='1' face='Verdana'><a href=" & strCostingURL & dt.Rows(iRowCounter).Item("CostSheetID").ToString & ">" & dt.Rows(iRowCounter).Item("CostSheetID").ToString & "</a></font></td>"
                    strBody &= "<td align='center'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("ExternalRFQNo").ToString & "</font></td>"
                    strBody &= "<td align='center'><font size='1' face='Verdana'><a href=" & strECIURL & dt.Rows(iRowCounter).Item("ECINo").ToString & ">" & dt.Rows(iRowCounter).Item("ECINo").ToString & "</a></font></td>"
                    strBody &= "<td align='center'><font size='1' face='Verdana'>" & dt.Rows(iRowCounter).Item("PurchasingPONo") & "</font></td>"
                    strBody &= "</tr>"
                Next

                strBody &= "</table>"
            End If

            Dim objRFDSupportingDocBLL As RFDSupportingDocBLL = New RFDSupportingDocBLL

            dt = objRFDSupportingDocBLL.GetRFDSupportingDoc(ViewState("RFDNo"))
            If commonFunctions.CheckDataTable(dt) = True Then

                strBody &= "<br /><br /><font size='1' face='Verdana'>Supporting Documents</font>"
                strBody &= "<table style='border: 1px solid #D0D0BF; width: 100%'>"

                For iRowCounter = 0 To dt.Rows.Count - 1
                    strBody &= "<tr>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'><a href=" & strSupportingDocURL & dt.Rows(iRowCounter).Item("RowID") & ">" & dt.Rows(iRowCounter).Item("SupportingDocName") & "</a></font></td>"
                    strBody &= "</tr>"
                Next

                strBody &= "</table>"
            End If

            Dim objRFDNetworkFilesBLL As RFDNetworkFilesBLL = New RFDNetworkFilesBLL

            dt = objRFDNetworkFilesBLL.GetRFDNetworkFiles(ViewState("RFDNo"))
            If commonFunctions.CheckDataTable(dt) = True Then

                strBody &= "<br /><br /><font size='1' face='Verdana'>Network File References</font>"
                strBody &= "<table style='border: 1px solid #D0D0BF; width: 100%'>"

                For iRowCounter = 0 To dt.Rows.Count - 1
                    strBody &= "<tr>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'><a href='" & dt.Rows(iRowCounter).Item("FilePath") & "' target='_blank'>" & dt.Rows(iRowCounter).Item("FilePath") & "</a></font></td>"
                    strBody &= "</tr>"
                Next

                strBody &= "</table>"
            End If

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br /><br />Email To Address List: " & EmailToAddress & "<br />"
                strBody &= "<br /><br />Email CC Address List: " & EmailCCAddress & "<br />"

                strEmailToAddress = "Lynette.Rey@ugnauto.com"
                strEmailCCAddress = ""
            End If

            'EmailCCAddress &= "; Lynette.Rey@ugnauto.com"

            strBody &= "<br /><br /><font size='1' face='Verdana'>"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"
            strBody &= "<br />If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the RFD Module."
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

            If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
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
            End If

            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "<br />Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "<br />Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("RFD Notification", strEmailFromAddress, EmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            End Try

            bReturnValue = True

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApproval.Text &= lblMessage.Text
        lblMessageApprovalBottom.Text &= lblMessage.Text

        Return bReturnValue

    End Function
    Protected Sub ddNewCommodity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddNewCommodity.SelectedIndexChanged

        Try
            ClearMessages()

            If ddNewCommodity.SelectedIndex > 0 Then
                ddWorkFlowCommodity.SelectedValue = ddNewCommodity.SelectedValue
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

    Protected Function GetTeamMembersBySelectedSubscription() As DataSet

        Try

            ClearMessages()

            'use same dropdown list for both purchasing groups
            Dim iSubscriptionID As Integer = ViewState("SelectedApproverSubscriptionID")

            If ViewState("SelectedApproverSubscriptionID") = 139 Then
                iSubscriptionID = 7
            End If

            Dim dsAppoversBySubscription As DataSet = commonFunctions.GetTeamMemberBySubscription(iSubscriptionID)

            GetTeamMembersBySelectedSubscription = dsAppoversBySubscription

        Catch ex As Exception
            GetTeamMembersBySelectedSubscription = Nothing

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Function

    Protected Sub gvApproval_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvApproval.DataBound

        'hide columns
        If gvApproval.Rows.Count > 0 Then
            gvApproval.HeaderRow.Cells(0).Visible = False
            gvApproval.HeaderRow.Cells(1).Visible = False
        End If

    End Sub

    Protected Sub gvApproval_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApproval.RowCreated

        'hide columns
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
        End If

    End Sub

    Protected Sub gvApproval_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApproval.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lblSubscriptionIDTemp As Label = CType(e.Row.FindControl("lblViewSubscriptionID"), Label)
                Dim iSubscriptionID As Integer = 0

                If lblSubscriptionIDTemp IsNot Nothing Then
                    If lblSubscriptionIDTemp.Text.Trim <> "" Then

                        Dim imgButton As ImageButton = CType(e.Row.FindControl("iBtnEditApprover"), ImageButton)
                        imgButton.CssClass = "none"

                        iSubscriptionID = CType(lblSubscriptionIDTemp.Text.Trim, Integer)

                        If ViewState("TeamMemberID") = ViewState("InitiatorTeamMemberID") _
                            Or (ViewState("BusinessProcessTypeID") = 1 And ViewState("isSales") = True) _
                            Or ViewState("SubscriptionID") = 4 _
                            Or ViewState("isAdmin") = True Then 'initiator or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 108 And ViewState("isPackaging") = True) Or ViewState("isAdmin") = True Then 'packaging or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 20 And ViewState("isPlantController") = True) Or ViewState("isAdmin") = True Then 'plant controller or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 66 And ViewState("isProcess") = True) Or ViewState("isAdmin") = True Then 'process or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 5 And ViewState("isProductDevelopment") = True) Or ViewState("isAdmin") = True Then 'ProductDevelopment or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 139 And (ViewState("isPurchasingExternalRFQ") = True Or ViewState("isPurchasing") = True)) Or ViewState("isAdmin") = True Then 'PurchasingExternalRFQ or admin
                            imgButton.CssClass = ""
                        End If

                        If (iSubscriptionID = 7 And ViewState("isPurchasing") = True) Or ViewState("isAdmin") = True Then 'Purchasing or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 65 And ViewState("isTooling") = True) Or ViewState("isAdmin") = True Then 'Tooling or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 6 And ViewState("isCosting") = True) Or ViewState("isAdmin") = True Then 'costing or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 22 And ViewState("isQualityEngineer") = True) Or ViewState("isAdmin") = True Then 'QualityEngineer or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If

                        If (iSubscriptionID = 119 And ViewState("isCapital") = True) Or ViewState("isAdmin") = True Then 'Tooling or admin
                            imgButton.CssClass = ""
                            'Else
                            '    imgButton.CssClass = "none"
                        End If
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

        lblMessageApproval.Text = lblMessage.Text

    End Sub
    Protected Sub gvApproval_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvApproval.RowEditing

        Try
            ClearMessages()

            ViewState("SelectedApproverSubscriptionID") = 0
            ViewState("SelectedApproverTeamMemberID") = 0

            'Dim dsApproverBySubscription As DataSet
            'Dim dsCheckSubscription As DataSet
            'Dim dsDefaultApprover As DataSet

            'Dim ddTeamMemberTemp As DropDownList
            Dim lblSubscriptionIDTemp As Label
            Dim lblTeamMemberIDTemp As Label

            Dim iSubscriptionID As Integer = 0
            Dim iTeamMemberID As Integer = 0

            Dim currentRowInEdit As Integer = e.NewEditIndex

            Dim TempRow As GridViewRow = gvApproval.Rows(e.NewEditIndex)

            If currentRowInEdit >= 0 Then

                'ddTeamMemberTemp = CType(gvApproval.Rows(currentRowInEdit).Cells(3).FindControl("ddEditApproverTeamMember"), DropDownList)
                'ddTeamMemberTemp = CType(TempRow.Cells(3).FindControl("ddEditApproverTeamMember"), DropDownList)
                'lblTeamMemberIDTemp = CType(gvApproval.Rows(currentRowInEdit).Cells(2).FindControl("lblEditTeamMemberID"), Label)
                'lblSubscriptionIDTemp = CType(gvApproval.Rows(currentRowInEdit).Cells(0).FindControl("lblEditSubscriptionID"), Label)

                lblTeamMemberIDTemp = CType(gvApproval.Rows(currentRowInEdit).Cells(2).FindControl("lblViewTeamMemberID"), Label)
                lblSubscriptionIDTemp = CType(gvApproval.Rows(currentRowInEdit).Cells(0).FindControl("lblViewSubscriptionID"), Label)

                iSubscriptionID = CType(lblSubscriptionIDTemp.Text, Integer)
                iTeamMemberID = CType(lblTeamMemberIDTemp.Text, Integer)

                'save for later use
                If iSubscriptionID > 0 And iTeamMemberID > 0 Then
                    ViewState("SelectedApproverSubscriptionID") = iSubscriptionID
                    ViewState("SelectedApproverTeamMemberID") = iTeamMemberID

                    ''get list of team members with this subscription
                    'dsApproverBySubscription = commonFunctions.GetTeamMemberBySubscription(iSubscriptionID)
                    'If commonFunctions.CheckDataset(dsApproverBySubscription) = True Then
                    '    ddTeamMemberTemp.DataSource = dsApproverBySubscription
                    '    ddTeamMemberTemp.DataTextField = dsApproverBySubscription.Tables(0).Columns("TMName").ColumnName
                    '    ddTeamMemberTemp.DataValueField = dsApproverBySubscription.Tables(0).Columns("TMID").ColumnName
                    '    ddTeamMemberTemp.DataBind()
                    'End If

                    ''check if team member still has this subscription
                    'If iTeamMemberID > 0 Then
                    '    dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, iSubscriptionID)

                    '    If commonFunctions.CheckDataset(dsCheckSubscription) = True Then
                    '        ddTeamMemberTemp.SelectedValue = iTeamMemberID
                    '    End If
                    'Else 'team member is not assigned yet, assign default approver of subscription

                    '    'find default subscription id for each subscription
                    '    Select Case iSubscriptionID
                    '        Case 6 ' costing
                    '            iTempDefaultSubscriptionID = 50 'default costing
                    '        Case 66 'Process
                    '            iTempDefaultSubscriptionID = 60 'default Process
                    '        Case 5 'Product Development
                    '            iTempDefaultSubscriptionID = 54 'default Product Development
                    '        Case 7 'Purchasing
                    '            iTempDefaultSubscriptionID = 53 'default Purchasing
                    '        Case 22 'Quality Engineer
                    '            iTempDefaultSubscriptionID = 51 'default Quality Engineer
                    '        Case 65 'Tooling
                    '            iTempDefaultSubscriptionID = 52 'default Quality Engineer
                    '    End Select

                    '    'get default team member
                    '    If iTempDefaultSubscriptionID > 0 Then
                    '        dsDefaultApprover = commonFunctions.GetTeamMemberBySubscription(iTempDefaultSubscriptionID)
                    '        If commonFunctions.CheckDataset(dsDefaultApprover) = True Then
                    '            If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    '                If dsDefaultApprover.Tables(0).Rows(0).Item("TMID") > 0 Then
                    '                    iTempDefaultTeamMemberID = dsDefaultApprover.Tables(0).Rows(0).Item("TMID")

                    '                    'check if team member still has this subscription
                    '                    dsCheckSubscription = SecurityModule.GetTMWorkHistory(iTempDefaultTeamMemberID, iSubscriptionID)

                    '                    If commonFunctions.CheckDataset(dsCheckSubscription) = True Then
                    '                        ddTeamMemberTemp.SelectedValue = iTempDefaultTeamMemberID
                    '                    End If
                    '                End If
                    '            End If
                    '        End If
                    '    End If
                    'End If
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

        lblMessageApproval.Text = lblMessage.Text

    End Sub

    'Protected Sub gvApproval_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvApproval.RowUpdated

    '    Try
    '        ClearMessages()

    '        gvApprovalHistory.DataBind()

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    lblMessageApproval.Text = lblMessage.Text

    'End Sub

    'Protected Sub btnCurrentApproverSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentApproverSave.Click

    '    Try
    '        ClearMessages()

    '        Dim iStatusID As Integer = 1
    '        Dim iSubscriptionID As Integer = 0
    '        Dim iTeamMemberID As Integer = 0

    '        If ddCurrentApproverStatus.SelectedIndex >= 0 Then
    '            iStatusID = ddCurrentApproverStatus.SelectedValue
    '        End If

    '        Select Case CType(ViewState("SubscriptionID"), Integer)
    '            Case 6, 66, 5, 7, 22, 65
    '                iSubscriptionID = ViewState("SubscriptionID")
    '        End Select

    '        If ddCurrentApproverTeamMember.SelectedIndex > 0 Then
    '            iTeamMemberID = ddCurrentApproverTeamMember.SelectedValue
    '        End If

    '        If iSubscriptionID > 0 And iTeamMemberID > 0 Then
    '            'first put previous team member info into history
    '            If iTeamMemberID <> ViewState("OriginalApproverID") Then
    '                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), iSubscriptionID, iTeamMemberID)
    '            End If

    '            RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), iSubscriptionID, iTeamMemberID, txtCurrentApproverComments.Text.Trim, iStatusID)
    '            lblMessage.Text = "Approval Status has been updated."
    '            gvApproval.DataBind()
    '            gvApprovalHistory.DataBind()
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    lblMessageCurrentApprover.Text = lblMessage.Text
    '    lblMessageApproval.Text = lblMessage.Text

    'End Sub

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        ' lblMessageCurrentApprover.Text = lblMessage.Text

    End Sub

    'Protected Sub FilterProgramList(ByVal Make As String)

    '    Try
    '        Dim dsProgram As DataSet

    '        dsProgram = commonFunctions.GetProgram("", "", Make)
    '        If commonFunctions.CheckDataSet(dsProgram) = True Then
    '            ddProgram.Items.Clear()
    '            ddProgram.DataSource = dsProgram
    '            ddProgram.DataTextField = dsProgram.Tables(0).Columns("ddProgramName").ColumnName.ToString()
    '            ddProgram.DataValueField = dsProgram.Tables(0).Columns("ProgramID").ColumnName
    '            ddProgram.DataBind()
    '            ddProgram.Items.Insert(0, "")
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub
    'Protected Sub ddMake_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMake.SelectedIndexChanged

    '    Try
    '        ClearMessages()

    '        If ddMake.SelectedIndex > 0 Then
    '            FilterProgramList(ddMake.SelectedValue)
    '        Else
    '            FilterProgramList("")
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    '    lblMessageCustomerProgram.Text = lblMessage.Text

    'End Sub

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageDescription.Text = lblMessage.Text

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageDescription.Text = lblMessage.Text

    End Sub
    Private Sub UpdateProductDevelopmentApprover()

        Try
            Dim dsCurrentApprover As DataSet
            Dim dsDefaultApprover As DataSet
            Dim dsCheckSubscription As DataSet

            Dim iTempApprovalStatus As Integer = 1
            Dim iTempDefaultTeamMemberID As Integer = 0

            If cbProductDevelopmentRequired.Checked = True Then
                'first check if a team member has been assigned yet, if not then insert record
                dsCurrentApprover = RFDModule.GetRFDApproval(ViewState("RFDNo"), 5, 0, False, False, False, True, True) 'Product Development

                If commonFunctions.CheckDataSet(dsCurrentApprover) = True Then
                    'first check for product development assigned to commodity in workflow
                    iTempApprovalStatus = dsCurrentApprover.Tables(0).Rows(0).Item("StatusID")

                    'if open or inprocess
                    If iTempApprovalStatus = 1 Or iTempApprovalStatus = 2 Then
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
                                                lblMessage.Text &= "<br />ERROR: The Default subscription for Product Engineering does not have the General Product Engineering subscription, please submit a support requestor."
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If iTempDefaultTeamMemberID > 0 And iTempApprovalStatus <> 3 Then
                                'insert new record                                      
                                RFDModule.InsertRFDApprovalStatus(ViewState("RFDNo"), 5, iTempDefaultTeamMemberID)
                                'lblMessage.Text &= "<br />Product Development approver added to the list."

                                RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 5, iTempDefaultTeamMemberID, "", 0, iTempApprovalStatus, "")
                            End If
                        End If
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

    End Sub

    Protected Sub ddWorkFlowCommodity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddWorkFlowCommodity.SelectedIndexChanged

        Try

            ClearMessages()

            If ddWorkFlowCommodity.SelectedIndex > 0 Then
                FilterProductDevelopmentCommodityList(ddWorkFlowCommodity.SelectedValue)

                If ViewState("TeamMemberID") = ViewState("InitiatorTeamMemberID") Or _
                                   ((ViewState("isSales") = True Or ViewState("isProgramManagement") = True) And ViewState("BusinessProcessTypeID") <> 2) Or _
                                   (ViewState("SubscriptionID") = 4 And ViewState("BusinessProcessTypeID") <> 1) _
                                   Or ViewState("isProductDevelopment") = True Then
                    UpdateProductDevelopmentApprover()
                End If
            Else
                FilterProductDevelopmentCommodityList(0)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageDescription.Text = lblMessage.Text

    End Sub

    Protected Function isSupportingDocCountMaximum() As Boolean

        Dim bMax As Boolean = False

        Try
            Dim ds As DataSet

            'check number of supporing docs
            ds = RFDModule.GetRFDSupportingDocList(ViewState("RFDNo"))
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows.Count >= 15 Then
                    bMax = True
                End If
            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            bMax = True
        End Try

        isSupportingDocCountMaximum = bMax

    End Function
    Protected Sub btnSaveUploadSupportingDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUploadSupportingDocument.Click

        Try
            ClearMessages()

            If QuoteOnlySupDocUpdate() = False Then
                Exit Sub
            End If

            If fileUploadSupportingDoc.PostedFile.ContentLength <= 3500000 Then
                Dim FileExt As String
                FileExt = System.IO.Path.GetExtension(fileUploadSupportingDoc.FileName).ToLower

                Dim SupportingDocFileSize As Integer = Convert.ToInt32(fileUploadSupportingDoc.PostedFile.InputStream.Length)
                Dim SupportingDocEncodeType As String = fileUploadSupportingDoc.PostedFile.ContentType
                Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                fileUploadSupportingDoc.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".xlsx") Or (FileExt = ".doc") Or (FileExt = ".docx") Or (FileExt = ".jpeg") Or (FileExt = ".jpg") Or (FileExt = ".tif") Or (FileExt = ".ppt") Or (FileExt = ".pptx") Or (FileExt = ".msg") Then

                    'set precendence for sales in case there are duplicate roles
                    If ViewState("isSales") = True Then
                        ViewState("SubscriptionID") = 9
                    End If

                    RFDModule.InsertRFDSupportingDoc(ViewState("RFDNo"), fileUploadSupportingDoc.FileName, txtSupportingDocDesc.Text.Trim, SupportingDocBinaryFile, SupportingDocEncodeType, SupportingDocFileSize, ViewState("TeamMemberID"), ViewState("SubscriptionID"))

                    revUploadFile.Enabled = False

                    lblMessage.Text &= "File Uploaded Successfully<br />"

                    Dim bSupportingDocCountMaximum As Boolean = isSupportingDocCountMaximum()
                    lblFileUploadLabel.Visible = Not bSupportingDocCountMaximum
                    fileUploadSupportingDoc.Visible = Not bSupportingDocCountMaximum
                    btnSaveUploadSupportingDocument.Visible = Not bSupportingDocCountMaximum

                    gvSupportingDoc.DataBind()
                    gvSupportingDoc.Visible = True

                    revUploadFile.Enabled = True
                End If
            Else
                lblMessage.Text &= "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageSupportingDocs.Text = lblMessage.Text
        lblMessageSupportingDocsBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvKit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvKit.RowDataBound

        Try
            ' Build the client script to open a popup window
            ' Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribsKit As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strWindowAttribsFinishedGood As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim ibtnEditSearchKitPartNo As ImageButton = CType(e.Row.FindControl("ibtnEditSearchKitPartNo"), ImageButton)
                Dim txtEditKitPartNo As TextBox = CType(e.Row.FindControl("txtEditKitPartNo"), TextBox)
                Dim txtEditKitPartRevision As TextBox = CType(e.Row.FindControl("txtEditKitPartRevision"), TextBox)

                If ibtnEditSearchKitPartNo IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?BPCSvcPartNo=" & txtEditKitPartNo.ClientID & "&BPCSvcPartRevision=" & txtEditKitPartRevision.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribsKit & "');return false;"
                    ibtnEditSearchKitPartNo.Attributes.Add("onClick", strClientScript)
                End If

                Dim ibtnEditSearchFinishedGoodPartNo As ImageButton = CType(e.Row.FindControl("ibtnEditSearchFinishedGoodPartNo"), ImageButton)
                Dim txtEditFinishedGoodPartNo As TextBox = CType(e.Row.FindControl("txtEditFinishedGoodPartNo"), TextBox)
                Dim txtEditFinishedGoodPartRevision As TextBox = CType(e.Row.FindControl("txtEditFinishedGoodPartRevision"), TextBox)

                If ibtnEditSearchFinishedGoodPartNo IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?BPCSvcPartNo=" & txtEditFinishedGoodPartNo.ClientID & "&BPCSvcPartRevision=" & txtEditFinishedGoodPartRevision.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribsFinishedGood & "');return false;"
                    ibtnEditSearchFinishedGoodPartNo.Attributes.Add("onClick", strClientScript)
                End If
            End If

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtnInsertSearchKitPartNo As ImageButton = CType(e.Row.FindControl("ibtnInsertSearchKitPartNo"), ImageButton)
                Dim txtFooterKitPartNo As TextBox = CType(e.Row.FindControl("txtInsertKitPartNo"), TextBox)
                Dim txtFooterKitPartRevision As TextBox = CType(e.Row.FindControl("txtInsertKitPartRevision"), TextBox)

                If ibtnInsertSearchKitPartNo IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?BPCSvcPartNo=" & txtFooterKitPartNo.ClientID & "&BPCSvcPartRevision=" & txtFooterKitPartRevision.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribsKit & "');return false;"
                    ibtnInsertSearchKitPartNo.Attributes.Add("onClick", strClientScript)
                End If

                Dim ibtnInsertSearchFinishedGoodPartNo As ImageButton = CType(e.Row.FindControl("ibtnInsertSearchFinishedGoodPartNo"), ImageButton)
                Dim txtFooterFinishedGoodPartNo As TextBox = CType(e.Row.FindControl("txtInsertFinishedGoodPartNo"), TextBox)
                Dim txtFooterFinishedGoodPartRevision As TextBox = CType(e.Row.FindControl("txtInsertFinishedGoodPartRevision"), TextBox)

                If ibtnInsertSearchFinishedGoodPartNo IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?BPCSvcPartNo=" & txtFooterFinishedGoodPartNo.ClientID & "&BPCSvcPartRevision=" & txtFooterFinishedGoodPartRevision.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribsFinishedGood & "');return false;"
                    ibtnInsertSearchFinishedGoodPartNo.Attributes.Add("onClick", strClientScript)
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Private Property LoadDataEmpty_Kit() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Kit") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Kit"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Kit") = value
        End Set

    End Property
    Protected Sub odsKit_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsKit.Selected


        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As RFD.RFDKit_MaintDataTable = CType(e.ReturnValue, RFD.RFDKit_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Kit = True
            Else
                LoadDataEmpty_Kit = False
            End If
        End If

    End Sub
    Private Sub ClearCustomerProgramInputFields()

        Try
            ViewState("CurrentCustomerProgramRow") = 0
            ViewState("CurrentCustomerProgramID") = 0

            btnSaveCustomerProgram.Text = "Add Customer/Program"
            btnCancelCustomerProgram.Visible = False

            cbCustomerApprovalRequired.Checked = False

            gvCustomerProgram.DataBind()
            gvCustomerProgram.SelectedIndex = -1
            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = ViewState("isEdit")

            tblMakes.Visible = True

            ' ''lblCustomerEdit.Visible = False
            ' ''ddCustomerEdit.Visible = False

            cddMakes.SelectedValue = Nothing

            ' ''ddCustomer.SelectedIndex = -1
            'ddCustomerByAccountManager.SelectedIndex = -1
            'ddMake.SelectedIndex = -1
            'ddProgram.SelectedIndex = -1
            ddYear.SelectedIndex = -1

            ' ''ddCustomerEdit.SelectedIndex = -1
            
            txtSOPDate.Text = ""
            txtEOPDate.Text = ""
            txtCustomerApprovalDate.Text = ""
            txtCustomerApprovalNo.Text = ""

            'FilterProgramList("")

            'If ddAccountManager.SelectedIndex > 0 And cbFilterCustomerByAccountManager.Checked = True Then

            '    FilterCustomerListByAccountManager()

            '    'cbFilterCustomerByAccountManager.Checked = True
            '    'trCustomerByAccountManager.Visible = True
            '    'trCustomerAll.Visible = False
            'Else
            '    cbFilterCustomerByAccountManager.Checked = False
            '    trCustomerByAccountManager.Visible = False
            '    trCustomerAll.Visible = True
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnCancelCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelCustomerProgram.Click

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

    Protected Sub btnSaveCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCustomerProgram.Click

        Try
            ClearMessages()

            Dim bContinue As Boolean = True

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            Dim dSOP As DateTime
            Dim dEOP As DateTime

            If ViewState("CurrentCustomerProgramRow") > 0 Then
                iProgramID = ViewState("CurrentCustomerProgramID")
            Else
                'If ViewState("CurrentCustomerProgramID") = 0 Then 'And ddProgram.SelectedIndex >= 0 Then
                ViewState("CurrentCustomerProgramID") = ddProgram.SelectedValue
                'End If

                iProgramID = ViewState("CurrentCustomerProgramID")
            End If

            If InStr(ddProgram.SelectedItem.Text, "**") > 0 And ViewState("CurrentCustomerProgramRow") = 0 Then
                lblMessage.Text &= "ERROR: An obsolete program cannot be selected. The information was NOT saved."
                ddModel.SelectedIndex = -1
                ddProgram.SelectedIndex = -1
            Else

                'make sure Year Selected is in range of SOP and EOP
                If ddYear.SelectedIndex > 0 Then
                    iProgramYear = ddYear.SelectedValue

                    If txtSOPDate.Text.Trim <> "" Then
                        dSOP = CType(txtSOPDate.Text.Trim, DateTime)

                        If iProgramYear < dSOP.Year Then
                            iProgramYear = dSOP.Year
                        End If
                    End If

                    If txtEOPDate.Text.Trim <> "" Then
                        dEOP = CType(txtEOPDate.Text.Trim, DateTime)

                        If iProgramYear > dEOP.Year Then
                            iProgramYear = dEOP.Year
                        End If
                    End If
                End If

                If txtSOPDate.Text.Trim <> "" And txtEOPDate.Text.Trim <> "" Then
                    If CType(txtSOPDate.Text.Trim, Date) > CType(txtEOPDate.Text.Trim, Date) Then
                        bContinue = False
                        lblMessage.Text &= "<br />ERROR: The SOP date cannot be later than the EOP date."
                    End If
                End If

                If bContinue = True And iProgramYear > 0 Then
                    If ViewState("CurrentCustomerProgramRow") > 0 Then

                        RFDModule.UpdateRFDCustomerProgram(ViewState("CurrentCustomerProgramRow"), ViewState("RFDNo"), cbCustomerApprovalRequired.Checked, txtCustomerApprovalDate.Text, txtCustomerApprovalNo.Text, iProgramID, iProgramYear, txtSOPDate.Text, txtEOPDate.Text)
                    Else
                      ''commonFunctions.GetCustomerSoldTo(ddCustomer.SelectedValue)

                        RFDModule.InsertRFDCustomerProgram(ViewState("RFDNo"), cbCustomerApprovalRequired.Checked, txtCustomerApprovalDate.Text, txtCustomerApprovalNo.Text, iProgramID, iProgramYear, txtSOPDate.Text, txtEOPDate.Text)
                    End If

                    ClearCustomerProgramInputFields()

                    'if NewDrawingNo exists, then synchronize customer program lists
                    If txtNewDrawingNo.Text.Trim <> "" And ViewState("RFDNo") > 0 Then
                        RFDModule.UpdateDrawingCustomerProgramBasedOnRFD(txtNewDrawingNo.Text.Trim, ViewState("RFDNo"))
                    End If

                    If HttpContext.Current.Session("BLLerror") IsNot Nothing Then
                        lblMessage.Text &= HttpContext.Current.Session("BLLerror")
                    Else
                        HttpContext.Current.Session("BLLerror") = Nothing
                        lblMessage.Text &= "Customer / Program information was saved."
                    End If

                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub

    Protected Sub GetCustomerProgram(ByVal PartNo As String)

        Try

            Dim dtCustomerProgram As New DataTable
            Dim dsVehicle As DataSet
            Dim dsProjectedSales As DataSet

            Dim iCustomerProgramRowCounter As Integer = 0
            Dim iVehicleRowCounter As Integer = 0
            Dim objPF As New Projected_Sales_Customer_ProgramBLL

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0
            Dim strSOPDate As String = ""
            Dim strEOPDate As String = ""
            Dim strUGNFacility As String = ""

            'If txtFGPartNo.Text.Trim <> "" Then
            lblMessage.Text = "This part was not found in the Planning And Forecasting Module."

            'dtCustomerProgram = objPF.GetProjectedSalesCustomerProgram(txtFGPartNo.Text.Trim)
            dtCustomerProgram = objPF.GetProjectedSalesCustomerProgram(PartNo)

            If dtCustomerProgram IsNot Nothing Then
                For iCustomerProgramRowCounter = 0 To dtCustomerProgram.Rows.Count - 1

                    iProgramID = 0
                    If dtCustomerProgram.Rows(iCustomerProgramRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                        If dtCustomerProgram.Rows(iCustomerProgramRowCounter).Item("ProgramID") > 0 Then
                            iProgramID = dtCustomerProgram.Rows(iCustomerProgramRowCounter).Item("ProgramID")
                        End If
                    End If

                    strUGNFacility = ""
                    strUGNFacility = dtCustomerProgram.Rows(iCustomerProgramRowCounter).Item("UGNFacility").ToString

                    dsVehicle = PFModule.GetVehicle(iProgramID, 0, "", 0, 0, "")
                    If commonFunctions.CheckDataSet(dsVehicle) = True Then
                        For iVehicleRowCounter = 0 To dsVehicle.Tables(0).Rows.Count - 1

                            iProgramYear = 0
                            If dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("PlanningYear") IsNot System.DBNull.Value Then
                                If dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("PlanningYear") > 0 Then
                                    iProgramYear = dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("PlanningYear")
                                End If
                            End If

                            strSOPDate = ""
                            strSOPDate = dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("SOP")

                            strEOPDate = ""
                            strEOPDate = dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("EOP")

                            If iProgramYear >= Now.Year Then
                                RFDModule.InsertRFDCustomerProgram(ViewState("RFDNo"), False, "", "", iProgramID, iProgramYear, strSOPDate, strEOPDate)
                                lblMessage.Text = ""

                                RFDModule.InsertRFDFacilityDept(ViewState("RFDNo"), strUGNFacility, 0)

                            End If

                        Next

                        gvCustomerProgram.DataBind()
                        gvFacilityDept.DataBind()
                    End If
                Next
            End If

            'dsProjectedSales = PFModule.GetProjectedSales(txtFGPartNo.Text.Trim)
            dsProjectedSales = PFModule.GetProjectedSales(PartNo)
            If commonFunctions.CheckDataSet(dsProjectedSales) = True Then

                If ddNewCommodity.SelectedIndex <= 0 Then
                    If dsProjectedSales.Tables(0).Rows(0).Item("CommodityID") IsNot System.DBNull.Value Then
                        If dsProjectedSales.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                            ddNewCommodity.SelectedValue = dsProjectedSales.Tables(0).Rows(0).Item("CommodityID")
                        End If
                    End If
                End If

                If ddNewProductTechnology.SelectedIndex <= 0 Then
                    If dsProjectedSales.Tables(0).Rows(0).Item("ProductTechnologyID") IsNot System.DBNull.Value Then
                        If dsProjectedSales.Tables(0).Rows(0).Item("ProductTechnologyID") > 0 Then
                            ddNewProductTechnology.SelectedValue = dsProjectedSales.Tables(0).Rows(0).Item("ProductTechnologyID")
                        End If
                    End If
                End If

            End If
            'End If

            If lblMessage.Text = "" Then
                lblMessage.Text = "Customer and Program information has been updated on the Customer tab"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    ' ''Protected Sub iBtnPFCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnPFCopy.Click

    ' ''    Try

    ' ''        ClearMessages()

    ' ''        If txtFGPartNo.Text.Trim <> "" Then
    ' ''            GetCustomerProgram(txtFGPartNo.Text.Trim)
    ' ''        End If

    ' ''    Catch ex As Exception

    ' ''        'get current event name
    ' ''        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    ' ''        'update error on web page
    ' ''        lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

    ' ''        'log and email error
    ' ''        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    ' ''    End Try

    ' ''    lblMessageFG.Text = lblMessage.Text
    ' ''    lblMessageFGBottom.Text = lblMessage.Text

    ' ''End Sub

    Protected Sub btnGetPlanningForecastingVehicle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetPlanningForecastingVehicle.Click

        Try

            ClearMessages()

            Dim dsVehicle As DataSet

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0
            Dim iVehicleRowCounter As Integer = 0

            Dim strSOPDate As String = ""
            Dim strEOPDate As String = ""
            Dim strUGNFacility As String = ""

            If ddYear.SelectedIndex > 0 Then
                iProgramYear = ddYear.SelectedValue
            End If

            If ddProgram.SelectedIndex > 0 Then
                iProgramID = ddProgram.SelectedValue

                dsVehicle = PFModule.GetVehicle(iProgramID, iProgramYear, "", 0, 0, "")
                If commonFunctions.CheckDataSet(dsVehicle) = True Then
                    For iVehicleRowCounter = 0 To dsVehicle.Tables(0).Rows.Count - 1

                        iProgramYear = 0
                        If dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("PlanningYear") IsNot System.DBNull.Value Then
                            If dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("PlanningYear") > 0 Then
                                iProgramYear = dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("PlanningYear")
                            End If
                        End If

                        strSOPDate = ""
                        strSOPDate = dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("SOP")

                        strEOPDate = ""
                        strEOPDate = dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("EOP")

                        RFDModule.InsertRFDCustomerProgram(ViewState("RFDNo"), False, "", "", iProgramID, iProgramYear, strSOPDate, strEOPDate)
                        lblMessage.Text = ""

                        RFDModule.InsertRFDFacilityDept(ViewState("RFDNo"), strUGNFacility, 0)

                        If ddAccountManager.SelectedIndex <= 0 Then
                            If dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("AcctMgrID") IsNot System.DBNull.Value Then
                                If dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("AcctMgrID") > 0 Then
                                    ddAccountManager.SelectedValue = dsVehicle.Tables(0).Rows(iVehicleRowCounter).Item("AcctMgrID")
                                End If
                            End If
                        End If
                    Next

                    gvCustomerProgram.DataBind()
                    gvFacilityDept.DataBind()
                Else
                    lblMessage.Text = "No information matches from the Planning and Forecasting Module."
                End If
            Else
                lblMessage.Text = "At least one program must be selected to pull from the Planning and Forecasting Module."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text
        lblMessageCustomerProgramBottom.Text = lblMessage.Text

    End Sub

    Protected Sub iBtnCurrentDrawingCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnCurrentDrawingCopy.Click

        Try

            ClearMessages()

            If txtCurrentDrawingNo.Text.Trim <> "" Then
                If GetCurrentFGDrawing(txtCurrentDrawingNo.Text.Trim) = True Then
                    lblMessage.Text &= "<br />Current Drawing Information Found."
                End If

                'if Current DrawingNo exists, then synchronize customer program list and vendor - PULL Current Drawing customer programs  and vendors into RFD
                If ViewState("RFDNo") > 0 Then
                    UpdateRFDCustomerProgramBasedOnDrawing(txtCurrentDrawingNo.Text.Trim)
                    UpdateRFDVendorBasedOnDrawing(txtCurrentDrawingNo.Text.Trim)

                    gvCustomerProgram.DataBind()
                    gvVendor.DataBind()

                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text

    End Sub

    Protected Sub iBtnNewDrawingCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnNewDrawingCopy.Click

        Try

            ClearMessages()

            If txtNewDrawingNo.Text.Trim <> "" Then
                If GetNewFGDrawing(txtNewDrawingNo.Text.Trim) = True Then
                    lblMessage.Text &= "<br />New Drawing Information Found."
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text

    End Sub

    Protected Function StepRevisionDrawingNo(ByVal StepOrRevision As String, ByVal OriginalDrawingNo As String, _
        ByVal InStepTracking As Integer, ByVal DrawingLayoutType As String) As String

        Dim strResult As String = ""

        Try

            Dim ds As DataSet
            Dim bNewPartMade As Boolean = False
            Dim strNewDrawingNo As String = ""

            'create new part part with an updated step number
            If InStepTracking < 9 Then

                'use this logic when the DMS enhancements are ready
                'Dim dsDrawingMaxRevision As DataSet = PEModule.GetDrawingMaxRevision(OriginalDrawingNo)
                'If commonFunctions.CheckDataSet(dsDrawingMaxRevision) = True Then
                '    'only create new revision if the drawing is max revision
                '    If OriginalDrawingNo = dsDrawingMaxRevision.Tables(0).Rows(0).Item("MaxRevisionDrawing").ToString Then
                '        ds = PEModule.CopyDrawing(OriginalDrawingNo, StepOrRevision)

                '        If commonFunctions.CheckDataSet(ds) = True Then
                '            strNewDrawingNo = ds.Tables(0).Rows(0).Item("newPart")
                '            PEModule.CopyDrawingImage(strNewDrawingNo, OriginalDrawingNo, DrawingLayoutType)
                '            PEModule.CopyDrawingApprovedVendor(strNewDrawingNo, OriginalDrawingNo)
                '            PEModule.CopyDrawingUnapprovedVendor(strNewDrawingNo, OriginalDrawingNo)

                '            'copy bill of materials of old parent to new parent
                '            PEModule.CopyDrawingBOM(strNewDrawingNo, OriginalDrawingNo)
                '            strResult = strNewDrawingNo
                '        End If
                '    Else
                '        lblMessage.Text &= "<br />Error occurred on Create Drawing Revision. The current drawing is not the highest revision and therefore could not be created into another revision."
                '    End If

                'End If

                'comment out this logic when the DMS enhancements are ready
                ds = PEModule.CopyDrawing(OriginalDrawingNo, StepOrRevision)

                If commonFunctions.CheckDataSet(ds) = True Then
                    strNewDrawingNo = ds.Tables(0).Rows(0).Item("newPart")
                    PEModule.CopyDrawingImage(strNewDrawingNo, OriginalDrawingNo, DrawingLayoutType)
                    PEModule.CopyDrawingApprovedVendor(strNewDrawingNo, OriginalDrawingNo)
                    PEModule.CopyDrawingUnapprovedVendor(strNewDrawingNo, OriginalDrawingNo)

                    strResult = strNewDrawingNo
                End If
            End If

            If strNewDrawingNo = "" Then
                lblMessage.Text &= "<br />Error occurred on Create Drawing Step.  It is possible that the maximum number of steps has been reached. If not, please contact IS Support."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        StepRevisionDrawingNo = strResult

    End Function
   
    Protected Sub btnGenerateNewFinishedGoodDrawing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateNewFGDrawing.Click

        Try
            ClearMessages()

            Dim dAMDValue As Double = 0
            Dim dDensityValue As Double = 0
            Dim dWMDValue As Double = 0

            Dim iInitialDimensionAndDensity As Integer = 0
            Dim iInStepTracking As Integer = 1
            Dim iNewCommodityID As Integer = 0
            Dim iSubFamilyID As Integer = 0
            Dim iProductTechnologyID As Integer = 0

            Dim strNewDrawingNo As String = ""

            Dim iRowCounter As Integer = 0

            Dim dtCustomerProgram As DataTable
            Dim objRFDCustomerProgramBLL As RFDCustomerProgramBLL = New RFDCustomerProgramBLL

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0


            If txtNewFGAMDValue.Text.Trim <> "" Then
                dAMDValue = CType(txtNewFGAMDValue.Text.Trim, Double)
            End If

            If txtNewFGDensityValue.Text.Trim <> "" Then
                dDensityValue = CType(txtNewFGDensityValue.Text.Trim, Double)
            End If

            If txtNewFGWMDValue.Text.Trim <> "" Then
                dWMDValue = CType(txtNewFGWMDValue.Text.Trim, Double)
            End If

            If txtNewFGInitialDimensionAndDensity.Text.Trim <> "" Then
                iInitialDimensionAndDensity = CType(txtNewFGInitialDimensionAndDensity.Text.Trim, Integer)
            Else
                If txtCurrentFGInitialDimensionAndDensity.Text.Trim <> "" Then
                    iInitialDimensionAndDensity = CType(txtCurrentFGInitialDimensionAndDensity.Text.Trim, Integer)
                End If
            End If

            If txtNewFGInStepTracking.Text.Trim <> "" Then
                iInStepTracking = CType(txtNewFGInStepTracking.Text.Trim, Integer)
            Else
                If txtCurrentFGInStepTracking.Text.Trim <> "" Then
                    iInStepTracking = CType(txtCurrentFGInStepTracking.Text.Trim, Integer)
                End If
            End If

            If ddNewCommodity.SelectedIndex > 0 Then
                iNewCommodityID = ddNewCommodity.SelectedValue
            End If

            If ddNewProductTechnology.SelectedIndex > 0 Then
                iProductTechnologyID = ddNewProductTechnology.SelectedValue
            End If

            If ddNewFGSubFamily.SelectedIndex > 0 Then
                iSubFamilyID = ddNewFGSubFamily.SelectedValue
            Else
                If ddCurrentFGSubFamily.SelectedIndex > 0 Then
                    iSubFamilyID = ddCurrentFGSubFamily.SelectedValue
                End If
            End If

            If txtCurrentDrawingNo.Text.Trim = "" Then
                rbGenerateNewFGDrawing.SelectedValue = "N"
            End If

            If txtNewDrawingNo.Text.Trim = "" Then

                If rbGenerateNewFGDrawing.SelectedValue = "N" Then
                    If iSubFamilyID > 0 Then
                        strNewDrawingNo = PEModule.GenerateDrawingNo(txtCurrentDrawingNo.Text.Trim, iSubFamilyID, iInitialDimensionAndDensity, iInStepTracking)

                        If strNewDrawingNo <> "" Then
                            PEModule.InsertDrawing(strNewDrawingNo, txtNewCustomerPartName.Text, 3, iInStepTracking, "", ViewState("RFDNo"), _
                                   txtNewCustomerPartNo.Text.Trim, ddDesignationType.SelectedValue, False, iSubFamilyID, iProductTechnologyID, _
                                   iNewCommodityID, 0, 0, ViewState("TeamMemberID"), 0, 0, 0, dDensityValue, txtNewFGDensityUnits.Text.Trim, _
                                   txtNewFGDensityTolerance.Text.Trim, 0, "", "", _
                                   ViewState("FGDrawingLayoutType"), dAMDValue, ddNewFGAMDUnits.SelectedValue, txtNewFGAMDTolerance.Text.Trim, _
                                   dWMDValue, ddNewFGWMDUnits.SelectedValue, txtNewFGWMDTolerance.Text.Trim, 0, txtNewFGConstruction.Text.Trim, "", _
                                   txtNewFGDrawingNotes.Text.Trim, "")

                            If txtCurrentDrawingNo.Text.Trim <> "" Then
                                PEModule.CopyDrawingImage(strNewDrawingNo, txtCurrentDrawingNo.Text.Trim, ViewState("FGDrawingLayoutType"))
                                PEModule.CopyDrawingApprovedVendor(ViewState("DrawingNo"), txtCurrentDrawingNo.Text.Trim)
                                PEModule.CopyDrawingUnapprovedVendor(ViewState("DrawingNo"), txtCurrentDrawingNo.Text.Trim)
                            End If


                        End If
                    End If
                End If

                If (rbGenerateNewFGDrawing.SelectedValue = "S" Or rbGenerateNewFGDrawing.SelectedValue = "R") And txtCurrentDrawingNo.Text.Trim <> "" Then
                    Dim StepOrRevision As String = "Rev"
                    If rbGenerateNewFGDrawing.SelectedValue = "S" Then
                        StepOrRevision = "Step"
                    End If
                    'this line of code actually creates the new drawing
                    strNewDrawingNo = StepRevisionDrawingNo(StepOrRevision, txtCurrentDrawingNo.Text.Trim, iInStepTracking, ViewState("FGDrawingLayoutType"))

                    If strNewDrawingNo <> "" Then
                        'this line of code updates the new drawing with these specific details
                        RFDModule.UpdateDrawingFromRFD(strNewDrawingNo, txtNewCustomerPartName.Text, 3, iInStepTracking, _
                            ViewState("RFDNo"), ddDesignationType.SelectedValue, iSubFamilyID, iProductTechnologyID, iNewCommodityID, 0, _
                            ViewState("TeamMemberID"), dDensityValue, txtNewFGDensityUnits.Text.Trim, txtNewFGDensityTolerance.Text.Trim, _
                            dAMDValue, ddNewFGAMDUnits.SelectedValue, txtNewFGAMDTolerance.Text.Trim, _
                            dWMDValue, ddNewFGWMDUnits.SelectedValue, txtNewFGWMDTolerance.Text.Trim, _
                            txtNewFGConstruction.Text.Trim, txtNewFGDrawingNotes.Text.Trim)
                    End If
                End If

                hlnkNewDrawingNo.Visible = False
                If strNewDrawingNo <> "" Then

                    hlnkNewDrawingNo.Visible = True
                    hlnkNewDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & strNewDrawingNo

                    txtNewDrawingNo.Text = strNewDrawingNo

                    Call btnSave_Click(sender, e)

                    ' ''btnGenerateFGPartNo.Visible = False
                    rbGenerateNewFGDrawing.Visible = False

                    'append to customer program list in DMS
                    dtCustomerProgram = objRFDCustomerProgramBLL.GetRFDCustomerProgram(ViewState("RFDNo"))

                    If commonFunctions.CheckDataTable(dtCustomerProgram) = True Then

                        For iRowCounter = 0 To dtCustomerProgram.Rows.Count - 1
                            iProgramID = 0
                            If dtCustomerProgram.Rows(iRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                                If dtCustomerProgram.Rows(iRowCounter).Item("ProgramID") > 0 Then
                                    iProgramID = dtCustomerProgram.Rows(iRowCounter).Item("ProgramID")
                                End If
                            End If

                            iProgramYear = 0
                            If dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                                If dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear") > 0 Then
                                    iProgramYear = dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear")
                                End If
                            End If

                            If iProgramID > 0 And iProgramYear > 0 Then
                                PEModule.InsertDrawingCustomerProgram(strNewDrawingNo, "", iProgramID, iProgramYear)
                            End If

                        Next
                    End If

                Else
                    lblMessage.Text &= "<br />ERROR: the New Drawing was NOT generated. Please contact IS."
                End If
            Else
                lblMessage.Text &= "<br />ERROR: the New Drawing was NOT generated. There is already a New Drawing Number. Please clear this field first."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopyAMD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopyAMD.Click

        Try
            ClearMessages()

            txtNewFGAMDValue.Text = txtCurrentFGAMDValue.Text
            txtNewFGAMDTolerance.Text = txtCurrentFGAMDTolerance.Text
            ddNewFGAMDUnits.SelectedValue = ddCurrentFGAMDUnits.SelectedValue

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopyWMD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopyWMD.Click

        Try
            ClearMessages()

            txtNewFGWMDValue.Text = txtCurrentFGWMDValue.Text
            txtNewFGWMDTolerance.Text = txtCurrentFGWMDTolerance.Text
            ddNewFGWMDUnits.SelectedValue = ddCurrentFGWMDUnits.SelectedValue

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopyDensity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopyDensity.Click

        Try
            ClearMessages()

            txtNewFGDensityValue.Text = txtCurrentFGDensityValue.Text
            txtNewFGDensityTolerance.Text = txtCurrentFGDensityTolerance.Text
            txtNewFGDensityUnits.Text = txtCurrentFGDensityUnits.Text

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopyNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopyNotes.Click

        Try
            ClearMessages()

            txtNewFGDrawingNotes.Text = txtCurrentFGDrawingNotes.Text

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopyConstruction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopyConstruction.Click

        Try
            ClearMessages()

            txtNewFGConstruction.Text = txtCurrentFGConstruction.Text

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopyAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopyAll.Click

        Try
            ClearMessages()

            'If ViewState("SubscriptionID") = 5 Then 'Product Development
            If ViewState("isProductDevelopment") = True Then
                txtNewFGInStepTracking.Text = txtCurrentFGInStepTracking.Text
                txtNewFGInitialDimensionAndDensity.Text = txtCurrentFGInitialDimensionAndDensity.Text
            End If

            txtNewFGAMDValue.Text = txtCurrentFGAMDValue.Text
            txtNewFGAMDTolerance.Text = txtCurrentFGAMDTolerance.Text
            ddNewFGAMDUnits.SelectedValue = ddCurrentFGAMDUnits.SelectedValue

            txtNewFGDensityValue.Text = txtCurrentFGDensityValue.Text
            txtNewFGDensityTolerance.Text = txtCurrentFGDensityTolerance.Text
            txtNewFGDensityUnits.Text = txtCurrentFGDensityUnits.Text

            txtNewFGWMDValue.Text = txtCurrentFGWMDValue.Text
            txtNewFGWMDTolerance.Text = txtCurrentFGWMDTolerance.Text
            ddNewFGWMDUnits.SelectedValue = ddCurrentFGWMDUnits.SelectedValue

            txtNewFGConstruction.Text = txtCurrentFGConstruction.Text
            txtNewFGDrawingNotes.Text = txtCurrentFGDrawingNotes.Text

            ddNewFGFamily.SelectedValue = ddCurrentFGFamily.SelectedValue
            ddNewFGSubFamily.SelectedValue = ddCurrentFGSubFamily.SelectedValue

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopySubFamily_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopySubFamily.Click

        Try
            ClearMessages()

            ddNewFGFamily.SelectedValue = ddCurrentFGFamily.SelectedValue
            ddNewFGSubFamily.SelectedValue = ddCurrentFGSubFamily.SelectedValue

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopyInitialDimensionAndDensity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopyInitialDimensionAndDensity.Click

        Try
            ClearMessages()

            If ViewState("isProductDevelopment") = True Then
                txtNewFGInitialDimensionAndDensity.Text = txtCurrentFGInitialDimensionAndDensity.Text
            End If

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentFGCopyInStepTracking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentFGCopyInStepTracking.Click

        Try
            ClearMessages()

            If ViewState("isProductDevelopment") = True Then
                txtNewFGInStepTracking.Text = txtCurrentFGInStepTracking.Text
            End If

            CompareCurrentAndNewFGDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerPartNo.Text = lblMessage.Text
        lblMessageCustomerPartNoBottom.Text = lblMessage.Text
        lblMessageFG.Text = lblMessage.Text
        lblMessageFGBottom.Text = lblMessage.Text

    End Sub
    ' ''Protected Sub ClearFinishedGoodInputFields()

    ' ''    Try

    ' ''        ViewState("CurrentFGRow") = 0

    ' ''        btnSaveFinishedGood.Text = "Add F.G. PartNo"
    ' ''        btnCancelFinishedGood.Visible = False

    ' ''        gvNewFinishedGood.DataBind()
    ' ''        gvNewFinishedGood.SelectedIndex = -1
    ' ''        gvNewFinishedGood.Columns(gvNewFinishedGood.Columns.Count - 1).Visible = ViewState("isEdit")

    ' ''        iBtnPFCopy.Visible = False

    ' ''        txtFGPartNo.Text = ""
    ' ''        ' ''txtFGPartRevision.Text = ""
    ' ''        txtFGPartName.Text = ""
    ' ''        txtFGDrawingNo.Text = ""
    ' ''        txtFGCostSheetID.Text = ""
    ' ''        txtFGECINo.Text = ""
    ' ''        txtFGCapExProjectNo.Text = ""
    ' ''        txtFGPONo.Text = ""

    ' ''    Catch ex As Exception

    ' ''        'get current event name
    ' ''        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    ' ''        'update error on web page
    ' ''        lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

    ' ''        'log and email error
    ' ''        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    ' ''    End Try

    ' ''End Sub
    ' ''Protected Sub btnSaveFinishedGood_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveFinishedGood.Click

    ' ''    Try
    ' ''        ClearMessages()

    ' ''        Dim iCostSheetID As Integer = 0
    ' ''        Dim iECINo As Integer = 0

    ' ''        If txtFGCostSheetID.Text.Trim <> "" Then
    ' ''            iCostSheetID = CType(txtFGCostSheetID.Text.Trim, Integer)
    ' ''        End If

    ' ''        If txtFGECINo.Text.Trim <> "" Then
    ' ''            iECINo = CType(txtFGECINo.Text.Trim, Integer)
    ' ''        End If

    ' ''        If ViewState("CurrentFGRow") > 0 Then
    ' ''            RFDModule.UpdateRFDFinishedGood(ViewState("CurrentFGRow"), ViewState("RFDNo"), txtFGPartNo.Text.Trim,  txtFGPartName.Text.Trim, txtFGDrawingNo.Text.Trim, iCostSheetID, iECINo, txtFGCapExProjectNo.Text.Trim, txtFGPONo.Text.Trim)
    ' ''        Else
    ' ''            RFDModule.InsertRFDFinishedGood(ViewState("RFDNo"), txtFGPartNo.Text.Trim,  txtFGPartName.Text.Trim, txtFGDrawingNo.Text.Trim, iCostSheetID, iECINo, txtFGCapExProjectNo.Text.Trim, txtFGPONo.Text.Trim)
    ' ''        End If

    ' ''        ClearFinishedGoodInputFields()

    ' ''        If HttpContext.Current.Session("BLLerror") IsNot Nothing Then
    ' ''            lblMessage.Text &= HttpContext.Current.Session("BLLerror")
    ' ''        Else
    ' ''            HttpContext.Current.Session("BLLerror") = Nothing
    ' ''            lblMessage.Text &= "Finished Good information was saved."
    ' ''        End If

    ' ''    Catch ex As Exception

    ' ''        'get current event name
    ' ''        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    ' ''        'update error on web page
    ' ''        lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

    ' ''        'log and email error
    ' ''        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    ' ''    End Try

    ' ''    lblMessageCustomerProgram.Text = lblMessage.Text
    ' ''    lblMessageFG.Text = lblMessage.Text
    ' ''    lblMessageFGBottom.Text = lblMessage.Text

    ' ''End Sub

    ' ''Protected Sub btnCancelFinishedGood_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelFinishedGood.Click

    ' ''    Try
    ' ''        ClearMessages()

    ' ''        ClearFinishedGoodInputFields()

    ' ''        acFinishedGood.SelectedIndex = -1

    ' ''    Catch ex As Exception

    ' ''        'get current event name
    ' ''        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    ' ''        'update error on web page
    ' ''        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    ' ''        'log and email error
    ' ''        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    ' ''    End Try

    ' ''    lblMessageCustomerPartNo.Text = lblMessage.Text
    ' ''    lblMessageCustomerPartNoBottom.Text = lblMessage.Text
    ' ''    lblMessageFG.Text = lblMessage.Text
    ' ''    lblMessageFGBottom.Text = lblMessage.Text

    ' ''End Sub

    ' ''Protected Sub gvNewFinishedGood_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvNewFinishedGood.Sorting

    ' ''    ClearFinishedGoodInputFields()

    ' ''End Sub

    Protected Sub gvCustomerProgram_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvCustomerProgram.Sorting

        ClearCustomerProgramInputFields()

    End Sub

    Protected Sub gvChildPart_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvChildPart.Sorting

        ClearChildPartFields()

    End Sub

    Protected Sub ChildAddUpdate()

        Try
            Dim ds As DataSet

            Dim bValidData As Boolean = True
            Dim bFoundObsolete As Boolean = False

            Dim dAMDvalue As Double = 0
            Dim dDensityValue As Double = 0
            Dim dWMDvalue As Double = 0
            Dim iCostSheetID As Integer = 0
            Dim iECINo As Integer = 0
            Dim iInStepTracking As Integer = 0
            Dim iNewPurchasedGoodID As Integer = 0
            Dim iNewSubFamilyID As Integer = 0
            Dim iNewChildLeadTime As Integer = 0

            If txtNewChildLeadTime.Text.Trim <> "" Then
                iNewChildLeadTime = CType(txtNewChildLeadTime.Text.Trim, Integer)
            End If

            If txtNewChildExternalRFQNo.Text.Trim <> "" Then
                cbNewChildExternalRFQNoNA.Checked = False
            End If

            If txtNewChildCostSheetID.Text.Trim <> "" Then
                iCostSheetID = CType(txtNewChildCostSheetID.Text.Trim, Integer)

                ds = CostingModule.GetCostSheet(iCostSheetID)

                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />ERROR: The new Cost Sheet ID does not exist."
                    bValidData = False
                End If
            End If

            If txtNewChildECINo.Text.Trim <> "" Then
                iECINo = CType(txtNewChildECINo.Text.Trim, Integer)

                ds = ECIModule.GetECI(iECINo)

                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />ERROR: The new ECI No does not exist."
                    bValidData = False
                End If
            End If

            If txtNewChildInStepTracking.Text.Trim <> "" Then
                iInStepTracking = CType(txtNewChildInStepTracking.Text.Trim, Integer)
            End If

            If txtNewChildAMDValue.Text.Trim <> "" Then
                dAMDvalue = CType(txtNewChildAMDValue.Text.Trim, Double)
            End If

            If txtNewChildWMDValue.Text.Trim <> "" Then
                dWMDvalue = CType(txtNewChildWMDValue.Text.Trim, Double)
            End If

            If txtNewChildDensityValue.Text.Trim <> "" Then
                dDensityValue = CType(txtNewChildDensityValue.Text.Trim, Double)
            End If

            If ddNewChildSubFamily.SelectedIndex > 0 Then
                iNewSubFamilyID = ddNewChildSubFamily.SelectedValue
                If InStr(ddNewChildSubFamily.SelectedItem.Text, "**") > 0 Then
                    bFoundObsolete = True
                End If
            End If

            If ddNewChildPurchasedGood.SelectedIndex > 0 Then
                iNewPurchasedGoodID = ddNewChildPurchasedGood.SelectedValue
                If InStr(ddNewChildPurchasedGood.SelectedItem.Text, "**") > 0 Then
                    bFoundObsolete = True
                End If
            End If

            If ddNewChildDesignationType.SelectedValue = "C" Then 'Finished Good
                bFoundObsolete = True
            End If

            If txtCurrentChildPartNo.Text.Trim <> "" Then
                ds = commonFunctions.GetBPCSPartNo(txtCurrentChildPartNo.Text.Trim, "")

                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />ERROR: The current child part number does not exist."
                    bValidData = False
                End If
            End If

            If txtNewChildDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtNewChildDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />ERROR: The new child DMS drawing number does not exist."
                    bValidData = False
                End If
            End If

            If txtCurrentChildDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtCurrentChildDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />ERROR: The current child DMS drawing number does not exist."
                    bValidData = False
                End If
            End If

            If bValidData = True Then
                'if the child part is new then set it to match the description tab
                If ViewState("CurrentChildPartRow") = 0 Then
                    If ddDesignationType.SelectedValue <> "C" Then
                        ddNewChildDesignationType.SelectedValue = ddDesignationType.SelectedValue
                    Else
                        ddNewChildDesignationType.SelectedValue = "R"
                    End If
                End If

                If bFoundObsolete = False Then

                    'if current PartNo exists but Current DMS draiwing is blank, then check for DMS Drawing No
                    If txtCurrentChildPartNo.Text.Trim <> "" And txtCurrentChildDrawingNo.Text.Trim = "" Then
                        ds = PEModule.GetDrawingSearch("", 0, txtCurrentChildPartNo.Text.Trim, "", "", "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, False, "", "", "", 0)
                        If commonFunctions.CheckDataSet(ds) = True Then
                            txtCurrentChildDrawingNo.Text = ds.Tables(0).Rows(0).Item("DrawingNo").ToString
                        End If
                    End If

                    If ViewState("CurrentChildPartRow") > 0 Then
                        RFDModule.UpdateRFDChildPart(ViewState("CurrentChildPartRow"), ViewState("RFDNo"), _
                            txtCurrentChildPartNo.Text.Trim, txtNewChildPartNoValue.Text.Trim, _
                            txtCurrentChildPartName.Text.Trim, txtNewChildPartNameValue.Text.Trim, _
                            txtCurrentChildDrawingNo.Text.Trim, txtNewChildDrawingNo.Text.Trim, _
                            iCostSheetID, iECINo, Not cbNewChildECIOverrideNA.Checked, _
                            txtNewChildPONo.Text.Trim, txtNewChildExternalRFQNo.Text.Trim, Not cbNewChildExternalRFQNoNA.Checked, iInStepTracking, _
                            dAMDvalue, ddNewChildAMDUnits.SelectedValue, txtNewChildAMDTolerance.Text.Trim, _
                            dWMDvalue, ddNewChildWMDUnits.SelectedValue, txtNewChildWMDTolerance.Text.Trim, _
                            txtNewChildConstruction.Text.Trim, _
                            dDensityValue, txtNewChildDensityUnits.Text.Trim, txtNewChildDensityTolerance.Text.Trim, _
                            txtNewChildDrawingNotes.Text.Trim, ddNewChildDesignationType.SelectedValue, _
                            iNewSubFamilyID, iNewPurchasedGoodID, _
                            iNewChildLeadTime, ddNewChildLeadUnits.SelectedValue)
                    Else
                        RFDModule.InsertRFDChildPart(ViewState("RFDNo"), txtCurrentChildPartNo.Text.Trim, _
                            txtNewChildPartNoValue.Text.Trim, _
                            txtCurrentChildPartName.Text.Trim, txtNewChildPartNameValue.Text.Trim, _
                            txtCurrentChildDrawingNo.Text.Trim, _
                            txtNewChildDrawingNo.Text.Trim, iCostSheetID, iECINo, txtNewChildPONo.Text.Trim, _
                            txtNewChildExternalRFQNo.Text.Trim, Not cbNewChildExternalRFQNoNA.Checked, iInStepTracking, _
                            dAMDvalue, ddNewChildAMDUnits.SelectedValue, txtNewChildAMDTolerance.Text.Trim, _
                            dWMDvalue, ddNewChildWMDUnits.SelectedValue, txtNewChildWMDTolerance.Text.Trim, txtNewChildConstruction.Text.Trim, _
                            dDensityValue, txtNewChildDensityUnits.Text.Trim, txtNewChildDensityTolerance.Text.Trim, _
                            txtNewChildDrawingNotes.Text.Trim, _
                            ddNewChildDesignationType.SelectedValue, _
                            iNewSubFamilyID, iNewPurchasedGoodID, _
                            iNewChildLeadTime, ddNewChildLeadUnits.SelectedValue)

                        txtCurrentChildPartNo.Text = ""
                        ' ''txtCurrentChildPartRevision.Text = ""

                        txtNewChildPartNoValue.Text = ""
                        ' ''txtNewChildPartRevisionValue.Text = ""
                        txtNewChildPartNameValue.Text = ""

                    End If

                    GetCurrentChildDrawing()

                    CompareCurrentAndNewChildDrawing()

                    gvChildPart.DataBind()

                    If HttpContext.Current.Session("BLLerror") IsNot Nothing Then
                        lblMessage.Text &= HttpContext.Current.Session("BLLerror")
                    Else
                        HttpContext.Current.Session("BLLerror") = Nothing
                        lblMessage.Text &= "<br />Child part information was saved."
                    End If
                Else
                    lblMessage.Text &= "<br />ERROR: The information could not be saved.<br />Child parts can not be set as finsished goods.<br />Obsolete items can not be saved on new selections."
                End If

            End If 'end valid data

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub
    Protected Sub btnSaveChild_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveChild.Click, btnSaveChildDetails.Click

        Try
            ClearMessages()

            ChildAddUpdate()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentChildCopyAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopyAll.Click

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim iFamilyID As Integer = 0

            'If ViewState("SubscriptionID") = 5 Then 'Product Development
            'If ViewState("isProductDevelopment") = True Then
            'txtNewFGInStepTracking.Text = txtCurrentFGInStepTracking.Text
            'txtNewFGInitialDimensionAndDensity.Text = txtCurrentFGInitialDimensionAndDensity.Text
            'End If

            txtNewChildAMDValue.Text = txtCurrentChildAMDValue.Text
            txtNewChildAMDTolerance.Text = txtCurrentChildAMDTolerance.Text
            ddNewChildAMDUnits.SelectedValue = ddCurrentChildAMDUnits.SelectedValue

            txtNewChildDensityValue.Text = txtCurrentChildDensityValue.Text
            txtNewChildDensityTolerance.Text = txtCurrentChildDensityTolerance.Text
            txtNewChildDensityUnits.Text = txtCurrentChildDensityUnits.Text

            txtNewChildWMDValue.Text = txtCurrentChildWMDValue.Text
            txtNewChildWMDTolerance.Text = txtCurrentChildWMDTolerance.Text
            ddNewChildWMDUnits.SelectedValue = ddCurrentChildWMDUnits.SelectedValue

            txtNewChildConstruction.Text = txtCurrentChildConstruction.Text
            txtNewChildDrawingNotes.Text = txtCurrentChildDrawingNotes.Text

            ddNewChildDesignationType.SelectedValue = ddCurrentChildDesignationType.SelectedValue

            ddNewChildPurchasedGood.SelectedValue = ddCurrentChildPurchasedGood.SelectedValue

            ddNewChildFamily.SelectedValue = ddCurrentChildFamily.SelectedValue

            If ddNewChildFamily.SelectedIndex > 0 Then
                iFamilyID = ddNewChildFamily.SelectedValue
            End If

            ds = commonFunctions.GetSubFamily(iFamilyID)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddNewChildSubFamily.DataSource = ds
                ddNewChildSubFamily.DataTextField = ds.Tables(0).Columns("subFamilyName").ColumnName
                ddNewChildSubFamily.DataValueField = ds.Tables(0).Columns("subFamilyID").ColumnName
                ddNewChildSubFamily.DataBind()
                ddNewChildSubFamily.Items.Insert(0, "")
            End If

            ddNewChildSubFamily.SelectedValue = ddCurrentChildSubFamily.SelectedValue

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        'two types of copy - keep same current and new information which is basically a revised RFD OR put make new information become current and leave blank the new in the new

        Try
            ClearMessages()

            Dim ds As DataSet

            Dim bFoundObsolete As Boolean = False

            Dim dNewFinishedGoodAMDValue As Double = 0
            Dim dNewFinishedGoodDensityValue As Double = 0
            Dim dNewFinishedGoodWMDValue As Double = 0

            Dim dTargetAnnualSales As Double = 0
            Dim dTargetPrice As Double = 0

            Dim iAccountManagerID As Integer = 0
            Dim iProgramManagerID As Integer = 0
            Dim iBusinessProcessActionID As Integer = 0
            Dim iBusinessProcessTypeID As Integer = 0

            Dim iCostSheetID As Integer = 0
            Dim iCostingTeamMemberID As Integer = 0
            Dim iECINo As Integer = 0
            Dim iFamilyID As Integer = 0
            Dim iInitiatorTeamMemberID As Integer = 0
            Dim iNewInStepTracking As Integer = 0
            Dim iNewCommodityID As Integer = 0
            Dim iNewSubFamilyID As Integer = 0

            Dim iPreviousRFDNo As Integer = 0
            Dim iPreviousRFDNoToBeReferenced As Integer = 0

            Dim iProdDevCommodityTeamMember As Integer = 0
            Dim iPriorityID As Integer = 0
            Dim iPurchasingMakeTeamMemberID As Integer = 0
            Dim iPurchasingFamilyTeamMember As Integer = 0
            Dim iTargetAnnualVolume As Integer = 0
            Dim iNewProductTechnologyID As Integer = 0

            Dim strDesignationType As String = ""
            Dim strMake As String = ""
            Dim strNewFinishedGoodAMDUnits As String = ""
            Dim strNewFinishedGoodWMDUnits As String = ""
            Dim strPriceCode As String = ""
            Dim strCopyType As String = "N"

            cbNewECIOverrideNA.Checked = False
            cbNewChildECIOverrideNA.Checked = False

            If ddAccountManager.SelectedIndex > 0 Then
                iAccountManagerID = ddAccountManager.SelectedValue
                If InStr(ddAccountManager.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iAccountManagerID = 0
                    ddAccountManager.SelectedIndex = -1
                End If
            End If

            If ddProgramManager.SelectedIndex > 0 Then
                iProgramManagerID = ddProgramManager.SelectedValue
                If InStr(ddProgramManager.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iProgramManagerID = 0
                    ddProgramManager.SelectedIndex = -1
                End If
            End If

            If ddBusinessProcessType.SelectedIndex >= 0 Then
                iBusinessProcessTypeID = ddBusinessProcessType.SelectedValue
                If InStr(ddBusinessProcessType.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True

                    If ViewState("isSales") = True Or ViewState("isProgramManagement") = True Then
                        iBusinessProcessTypeID = 7 'make quote only if obsolete for sales
                    Else
                        iBusinessProcessTypeID = 2 'make RFC if obsolete for others
                    End If
                End If
            End If

            If ddBusinessProcessAction.SelectedIndex >= 0 Then
                iBusinessProcessActionID = ddBusinessProcessAction.SelectedValue
                If InStr(ddBusinessProcessAction.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    Select Case iBusinessProcessTypeID
                        Case 1
                            iBusinessProcessActionID = 3 'make design change if customer driven change
                        Case 7
                            iBusinessProcessActionID = 1 'make estimated quote if quote only
                        Case Else
                            iBusinessProcessActionID = 0
                    End Select
                End If
            End If

            'Customer Driven Change or Quote-Only - if current teammember is not sales and is trying to copy then change type
            If (iBusinessProcessTypeID = 1 Or iBusinessProcessTypeID = 7) And ViewState("isSales") = False And ViewState("isProgramManagement") = False Then
                iBusinessProcessTypeID = 2
            End If

            'RFCs - if current team member is sales or program management then change type
            If iBusinessProcessTypeID = 2 And (ViewState("isSales") = True Or ViewState("isProgramManagement") = True) Then
                iBusinessProcessTypeID = 1
            End If

            If ddDesignationType.SelectedIndex >= 0 Then
                strDesignationType = ddDesignationType.SelectedValue
                If InStr(ddDesignationType.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    strDesignationType = "C" 'set to Finished Good
                End If
            End If

            iInitiatorTeamMemberID = ViewState("TeamMemberID")
            'If ddInitiator.SelectedIndex >= 0 Then
            '    iInitiatorTeamMemberID = ddInitiator.SelectedValue
            '    If InStr(ddInitiator.SelectedItem.Text, "**") > 0 Then
            '        'bFoundObsolete = True
            '        iInitiatorTeamMemberID = ViewState("TeamMemberID")
            '    End If
            'End If

            If ddNewCommodity.SelectedIndex > 0 Then
                iNewCommodityID = ddNewCommodity.SelectedValue
                If InStr(ddNewCommodity.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iNewCommodityID = 0
                    ddNewCommodity.SelectedIndex = -1
                End If
            End If

            If ddProductDevelopmentTeamMemberByCommodity.SelectedIndex >= 0 Then
                iProdDevCommodityTeamMember = ddProductDevelopmentTeamMemberByCommodity.SelectedValue
                If InStr(ddProductDevelopmentTeamMemberByCommodity.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iProdDevCommodityTeamMember = 0
                    ddProductDevelopmentTeamMemberByCommodity.SelectedIndex = -1
                End If
            End If

            If ddPurchasingTeamMemberByMake.SelectedIndex >= 0 Then
                iPurchasingMakeTeamMemberID = ddPurchasingTeamMemberByMake.SelectedValue
                If InStr(ddPurchasingTeamMemberByMake.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iPurchasingMakeTeamMemberID = 0
                    ddPurchasingTeamMemberByMake.SelectedIndex = -1
                End If
            End If

            If ddPurchasingTeamMemberByFamily.SelectedIndex >= 0 Then
                iPurchasingFamilyTeamMember = ddPurchasingTeamMemberByFamily.SelectedValue
                If InStr(ddPurchasingTeamMemberByFamily.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iPurchasingFamilyTeamMember = 0
                    ddPurchasingTeamMemberByFamily.SelectedIndex = -1
                End If
            End If

            'workflow commodity takes precedence over newcommodity
            If ddWorkFlowCommodity.SelectedIndex > 0 Then
                iNewCommodityID = ddWorkFlowCommodity.SelectedValue
                ddNewCommodity.SelectedValue = iNewCommodityID
                If InStr(ddWorkFlowCommodity.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iNewCommodityID = 0
                    ddWorkFlowCommodity.SelectedIndex = -1
                End If
            End If

            If ddNewFGAMDUnits.SelectedIndex > 0 Then
                strNewFinishedGoodAMDUnits = ddNewFGAMDUnits.SelectedValue
            End If

            If ddNewFGWMDUnits.SelectedIndex > 0 Then
                strNewFinishedGoodWMDUnits = ddNewFGWMDUnits.SelectedValue
            End If

            If ddNewProductTechnology.SelectedIndex > 0 Then
                iNewProductTechnologyID = ddNewProductTechnology.SelectedValue
                If InStr(ddNewProductTechnology.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iNewProductTechnologyID = 0
                End If
            End If

            If ddNewFGSubFamily.SelectedIndex > 0 Then
                iNewSubFamilyID = ddNewFGSubFamily.SelectedValue
                If InStr(ddNewFGSubFamily.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iNewSubFamilyID = 0
                End If

                'get left 2 digits of subfamily
                If iNewSubFamilyID > 0 Then
                    Dim strFamilyID As String = Left(CType(ddNewFGSubFamily.SelectedValue, String).PadLeft(4, "0"), 2)
                    If strFamilyID <> "" Then
                        ddNewFGFamily.SelectedValue = CType(strFamilyID, Integer)
                    End If
                End If                
            End If

            If ddPriceCode.SelectedIndex >= 0 Then
                strPriceCode = ddPriceCode.SelectedValue
                If InStr(ddPriceCode.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    strPriceCode = ""
                End If
            End If

            If ddPriority.SelectedIndex >= 0 Then
                iPriorityID = ddPriority.SelectedValue
                If InStr(ddPriority.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    iPriorityID = 3
                End If
            End If

            If ddWorkflowFamily.SelectedIndex > 0 Then
                iFamilyID = ddWorkflowFamily.SelectedValue
                If InStr(ddWorkflowFamily.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    ddWorkflowFamily.SelectedIndex = -1
                    iFamilyID = 0
                End If
            End If

            If ddWorkFlowMake.SelectedIndex > 0 Then
                strMake = ddWorkFlowMake.SelectedValue
                'ddMakes.SelectedValue = strMake
                'ddMake.SelectedValue = strMake
                'FilterProgramList(strMake)

                If InStr(ddWorkFlowMake.SelectedItem.Text, "**") > 0 Then
                    'bFoundObsolete = True
                    strMake = ""
                End If
            End If

            If txtNewCostSheetID.Text.Trim <> "" Then
                iCostSheetID = CType(txtNewCostSheetID.Text.Trim, Integer)
            End If

            If txtNewECINo.Text.Trim <> "" Then
                iECINo = CType(txtNewECINo.Text.Trim, Integer)
            End If

            If txtNewFGAMDValue.Text.Trim <> "" Then
                dNewFinishedGoodAMDValue = CType(txtNewFGAMDValue.Text.Trim, Double)
            End If

            If txtNewFGDensityValue.Text.Trim <> "" Then
                dNewFinishedGoodDensityValue = CType(txtNewFGDensityValue.Text.Trim, Double)
            End If

            If txtNewFGInStepTracking.Text.Trim <> "" Then
                iNewInStepTracking = CType(txtNewFGInStepTracking.Text.Trim, Integer)
            End If

            If txtNewFGWMDValue.Text.Trim <> "" Then
                dNewFinishedGoodWMDValue = CType(txtNewFGWMDValue.Text.Trim, Double)
            End If

            If txtTargetAnnualSales.Text.Trim <> "" Then
                dTargetAnnualSales = CType(txtTargetAnnualSales.Text.Trim, Double)
            End If

            If txtTargetAnnualVolume.Text.Trim <> "" Then
                iTargetAnnualVolume = CType(txtTargetAnnualVolume.Text.Trim, Integer)
            End If

            If txtTargetPrice.Text.Trim <> "" Then
                dTargetPrice = CType(txtTargetPrice.Text.Trim, Double)
            End If

            If rbCopyType.SelectedIndex > 0 Then
                strCopyType = rbCopyType.SelectedValue
            End If

            iPreviousRFDNo = ViewState("RFDNo")
            ViewState("RFDNo") = 0
            lblRFDNo.Text = 0

            txtDueDate.Text = Today.Date.AddDays(12)

            Select Case strCopyType
                Case "D"

                    iPreviousRFDNoToBeReferenced = 0
                Case "N"
                    'change the business process action to "change" instead of "new" 
                    If iBusinessProcessActionID = 1 Then
                        iBusinessProcessActionID = 2
                        ddBusinessProcessAction.SelectedValue = 2
                    End If

                    iPreviousRFDNoToBeReferenced = ViewState("RFDNo")

                    txtCurrentCustomerPartNo.Text = txtNewCustomerPartNo.Text.Trim
                    txtCurrentCustomerDrawingNo.Text = txtNewCustomerDrawingNo.Text.Trim
                    txtCurrentCustomerPartName.Text = txtNewCustomerPartName.Text.Trim
                    txtCurrentDesignLevel.Text = txtNewDesignLevel.Text.Trim
                    txtCurrentDrawingNo.Text = txtNewDrawingNo.Text.Trim

                    txtNewCustomerPartNo.Text = ""
                    txtNewCustomerDrawingNo.Text = ""
                    txtNewCustomerPartName.Text = ""
                    txtNewDesignLevel.Text = ""
                    txtNewDrawingNo.Text = ""
            End Select

            txtNewCapExProjectNo.Text = ""
            txtNewCostSheetID.Text = ""
            iCostSheetID = 0

            txtNewECINo.Text = ""
            iECINo = 0

            txtNewPONo.Text = ""
            cbAffectsCostSheetOnly.Checked = False

            'save new values, NOT on grids
            'If bFoundObsolete = False Then
            ds = RFDModule.InsertRFD(iPreviousRFDNoToBeReferenced, 1, txtRFDDesc.Text.Trim, iBusinessProcessActionID, _
                 iBusinessProcessTypeID, strDesignationType, strPriceCode, iPriorityID, _
                 txtDueDate.Text.Trim, iInitiatorTeamMemberID, iAccountManagerID, iProgramManagerID, _
                 txtImpactOnUGN.Text.Trim, dTargetPrice, iTargetAnnualVolume, _
                 dTargetAnnualSales, iNewCommodityID, iFamilyID, strMake, cbAffectsCostSheetOnly.Checked, cbCostingRequired.Checked, _
                 cbCustomerApprovalRequired.Checked, cbDVPRrequired.Checked, cbPackagingRequired.Checked, cbPlantControllerRequired.Checked, _
                 cbProcessRequired.Checked, cbProductDevelopmentRequired.Checked, _
                 cbPurchasingExternalRFQRequired.Checked, cbPurchasingRequired.Checked, _
                 cbQualityEngineeringRequired.Checked, cbRDrequired.Checked, cbToolingRequired.Checked, _
                 iProdDevCommodityTeamMember, iPurchasingMakeTeamMemberID, iPurchasingFamilyTeamMember, _
                 0, 0, cbCapitalRequired.Checked, txtCopyReason.Text.Trim, cbMeetingRequired.Checked, ddisCostReduction.SelectedValue)

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("NewRFDNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("NewRFDNo") > 0 Then
                        ViewState("RFDNo") = ds.Tables(0).Rows(0).Item("NewRFDNo")
                        lblRFDNo.Text = ViewState("RFDNo")
                        ddStatus.SelectedValue = 1
                        ViewState("StatusID") = 1

                        'update more information
                        RFDModule.UpdateRFD(ViewState("RFDNo"), txtRFDDesc.Text.Trim, iBusinessProcessActionID, _
                             iBusinessProcessTypeID, strDesignationType, strPriceCode, iPriorityID, _
                             txtDueDate.Text.Trim, iInitiatorTeamMemberID, iAccountManagerID, iProgramManagerID, _
                             txtImpactOnUGN.Text.Trim, dTargetPrice, iTargetAnnualVolume, _
                             dTargetAnnualSales, txtCurrentCustomerPartNo.Text.Trim, _
                             txtNewCustomerPartNo.Text, txtCurrentCustomerDrawingNo.Text.Trim, _
                             txtNewCustomerDrawingNo.Text.Trim, txtCurrentCustomerPartName.Text.Trim, _
                             txtNewCustomerPartName.Text.Trim, txtCurrentDesignLevel.Text.Trim, _
                             txtNewDesignLevel.Text.Trim, txtCurrentDrawingNo.Text.Trim, _
                             txtNewDrawingNo.Text.Trim, iNewInStepTracking, dNewFinishedGoodAMDValue, _
                             strNewFinishedGoodAMDUnits, txtNewFGAMDTolerance.Text.Trim, _
                             dNewFinishedGoodWMDValue, strNewFinishedGoodWMDUnits, _
                             txtNewFGWMDTolerance.Text.Trim, txtNewFGConstruction.Text.Trim, dNewFinishedGoodDensityValue, _
                             txtNewFGDensityUnits.Text.Trim, txtNewFGDensityTolerance.Text.Trim, txtNewFGDrawingNotes.Text.Trim, iNewCommodityID, _
                             iNewProductTechnologyID, iNewSubFamilyID, iFamilyID, strMake, iCostSheetID, iECINo, True, _
                             txtNewCapExProjectNo.Text.Trim, txtNewPONo.Text.Trim, cbAffectsCostSheetOnly.Checked, cbCostingRequired.Checked, _
                             cbCustomerApprovalRequired.Checked, cbDVPRrequired.Checked, cbPackagingRequired.Checked, cbPlantControllerRequired.Checked, _
                             cbProcessRequired.Checked, cbProductDevelopmentRequired.Checked, _
                             cbPurchasingExternalRFQRequired.Checked, cbPurchasingRequired.Checked, _
                             cbQualityEngineeringRequired.Checked, cbRDrequired.Checked, cbToolingRequired.Checked, _
                             iProdDevCommodityTeamMember, iPurchasingMakeTeamMemberID, iPurchasingFamilyTeamMember, _
                             cbPPAP.Checked, txtVendorRequirement.Text.Trim, 0, _
                             0, cbCapitalRequired.Checked, txtCopyReason.Text.Trim, cbMeetingRequired.Checked, ddisCostReduction.SelectedValue)

                        'copy grids                            
                        RFDModule.CopyRFDChildPart(strCopyType, ViewState("RFDNo"), iPreviousRFDNo)
                        RFDModule.CopyRFDCustomerProgram(ViewState("RFDNo"), iPreviousRFDNo)
                        RFDModule.CopyRFDFacilityDept(ViewState("RFDNo"), iPreviousRFDNo)
                        RFDModule.CopyRFDVendor(ViewState("RFDNo"), iPreviousRFDNo)

                        'insert / update Approval List based on checkboxes
                        InsertUpdateApprovalList()

                        'update history
                        Select Case strCopyType
                            Case "D"
                                RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Created RFD")
                            Case "N"
                                RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Copied RFD from RFDNo: " & iPreviousRFDNo.ToString)
                        End Select

                    End If
                End If
            End If

            HttpContext.Current.Session("CopyRFD") = "Copied"

            'refresh/redirect page
            Response.Redirect("RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnGenerateNewChildDrawing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateNewChildDrawing.Click

        Try
            ClearMessages()

            Dim dAMDValue As Double = 0
            Dim dDensityValue As Double = 0
            Dim dWMDValue As Double = 0

            Dim iInitialDimensionAndDensity As Integer = 0
            Dim iInStepTracking As Integer = 1
            Dim iPurchasedGoodID As Integer = 0
            Dim iSubFamilyID As Integer = 0
            Dim iProductTechnologyID As Integer = 0

            Dim strNewDrawingNo As String = ""

            Dim iRowCounter As Integer = 0

            Dim dtCustomerProgram As DataTable
            Dim objRFDCustomerProgramBLL As RFDCustomerProgramBLL = New RFDCustomerProgramBLL

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            If ViewState("CurrentChildPartRow") > 0 Then

                If txtNewChildAMDValue.Text.Trim <> "" Then
                    dAMDValue = CType(txtNewChildAMDValue.Text.Trim, Double)
                End If

                If txtNewChildDensityValue.Text.Trim <> "" Then
                    dDensityValue = CType(txtNewChildDensityValue.Text.Trim, Double)
                End If

                If txtNewChildWMDValue.Text.Trim <> "" Then
                    dWMDValue = CType(txtNewChildWMDValue.Text.Trim, Double)
                End If

                If txtNewChildInitialDimensionAndDensity.Text.Trim <> "" Then
                    iInitialDimensionAndDensity = CType(txtNewChildInitialDimensionAndDensity.Text.Trim, Integer)
                Else
                    If txtCurrentChildInitialDimensionAndDensity.Text.Trim <> "" Then
                        iInitialDimensionAndDensity = CType(txtCurrentChildInitialDimensionAndDensity.Text.Trim, Integer)
                    End If
                End If

                If txtNewChildInStepTracking.Text.Trim <> "" Then
                    iInStepTracking = CType(txtNewChildInStepTracking.Text.Trim, Integer)
                Else
                    If txtCurrentChildInStepTracking.Text.Trim <> "" Then
                        iInStepTracking = CType(txtCurrentChildInStepTracking.Text.Trim, Integer)
                    End If
                End If

                If ddNewChildPurchasedGood.SelectedIndex > 0 Then
                    iPurchasedGoodID = ddNewChildPurchasedGood.SelectedValue
                Else
                    If ddCurrentChildPurchasedGood.SelectedIndex > 0 Then
                        iPurchasedGoodID = ddCurrentChildPurchasedGood.SelectedValue
                        ddNewChildPurchasedGood.SelectedValue = iPurchasedGoodID
                    End If
                End If

                If ddNewChildSubFamily.SelectedIndex > 0 Then
                    iSubFamilyID = ddNewChildSubFamily.SelectedValue
                Else
                    If ddCurrentChildSubFamily.SelectedIndex > 0 Then
                        iSubFamilyID = ddCurrentChildSubFamily.SelectedValue
                        BindFamilySubFamily()
                        ddNewChildSubFamily.SelectedValue = iSubFamilyID
                    End If
                End If

                If txtCurrentChildDrawingNo.Text.Trim = "" Then
                    rbGenerateNewChildDrawing.SelectedValue = "N"
                End If

                If txtNewChildDrawingNo.Text.Trim = "" Then

                    If rbGenerateNewChildDrawing.SelectedValue = "N" Then
                        If iSubFamilyID > 0 Then
                            strNewDrawingNo = PEModule.GenerateDrawingNo(txtCurrentChildDrawingNo.Text.Trim, iSubFamilyID, iInitialDimensionAndDensity, iInStepTracking)

                            If strNewDrawingNo <> "" Then
                                ' make the new drawing
                                PEModule.InsertDrawing(strNewDrawingNo, txtNewChildPartNameValue.Text.Trim, 3, iInStepTracking, "", ViewState("RFDNo"), _
                                    "", ddNewChildDesignationType.SelectedValue, False, iSubFamilyID, iProductTechnologyID, _
                                    0, iPurchasedGoodID, 0, ViewState("TeamMemberID"), 0, 0, 0, dDensityValue, txtNewChildDensityUnits.Text.Trim, _
                                    txtNewChildDensityTolerance.Text.Trim, 0, "", "", _
                                    ViewState("CurrentChildDrawingLayoutType"), dAMDValue, ddNewChildAMDUnits.SelectedValue, txtNewChildAMDTolerance.Text.Trim, _
                                    dWMDValue, ddNewChildWMDUnits.SelectedValue, txtNewChildWMDTolerance.Text.Trim, 0, txtNewChildConstruction.Text.Trim, "", _
                                    txtNewChildDrawingNotes.Text.Trim, "")

                                If txtCurrentChildDrawingNo.Text.Trim <> "" Then
                                    PEModule.CopyDrawingImage(strNewDrawingNo, txtCurrentChildDrawingNo.Text.Trim, ViewState("CurrentChildDrawingLayoutType"))
                                    PEModule.CopyDrawingApprovedVendor(strNewDrawingNo, txtCurrentChildDrawingNo.Text.Trim)
                                    PEModule.CopyDrawingUnapprovedVendor(strNewDrawingNo, txtCurrentChildDrawingNo.Text.Trim)
                                End If

                            End If

                        End If
                    End If

                    If (rbGenerateNewChildDrawing.SelectedValue = "S" Or rbGenerateNewChildDrawing.SelectedValue = "R") And txtCurrentChildDrawingNo.Text.Trim <> "" Then
                        Dim StepOrRevision As String = "Rev"
                        If rbGenerateNewChildDrawing.SelectedValue = "S" Then
                            StepOrRevision = "Step"
                        End If

                        'this step actually creates the new drawing and copies all sub tables
                        strNewDrawingNo = StepRevisionDrawingNo(StepOrRevision, txtCurrentChildDrawingNo.Text.Trim, iInStepTracking, ViewState("CurrentChildDrawingLayoutType"))

                        If strNewDrawingNo <> "" Then
                            'this step updates the new drawing with these specific details
                            RFDModule.UpdateDrawingFromRFD(strNewDrawingNo, txtNewChildPartNameValue.Text.Trim, 3, iInStepTracking, _
                                ViewState("RFDNo"), ddNewChildDesignationType.SelectedValue, iSubFamilyID, 0, 0, iPurchasedGoodID, _
                                ViewState("TeamMemberID"), dDensityValue, txtNewChildDensityUnits.Text.Trim, txtNewChildDensityTolerance.Text.Trim, _
                                dAMDValue, ddNewChildAMDUnits.SelectedValue, txtNewChildAMDTolerance.Text.Trim, _
                                dWMDValue, ddNewChildWMDUnits.SelectedValue, txtNewChildWMDTolerance.Text.Trim, _
                                txtNewChildConstruction.Text.Trim, txtNewChildDrawingNotes.Text.Trim)
                        End If
                    End If

                    If strNewDrawingNo <> "" Then

                        lblNewChildDrawingNo.Text = strNewDrawingNo

                        hlnkNewChildDrawingNo.Visible = True
                        hlnkNewChildDrawingNo2.Visible = True

                        hlnkNewChildDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & strNewDrawingNo
                        hlnkNewChildDrawingNo2.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & strNewDrawingNo

                        txtNewChildDrawingNo.Text = strNewDrawingNo

                        ChildAddUpdate()

                        btnGenerateNewChildDrawing.Visible = False
                        rbGenerateNewChildDrawing.Visible = False

                        'append to customer program list in DMS
                        dtCustomerProgram = objRFDCustomerProgramBLL.GetRFDCustomerProgram(ViewState("RFDNo"))

                        If commonFunctions.CheckDataTable(dtCustomerProgram) = True Then

                            For iRowCounter = 0 To dtCustomerProgram.Rows.Count - 1
                                iProgramID = 0
                                If dtCustomerProgram.Rows(iRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                                    If dtCustomerProgram.Rows(iRowCounter).Item("ProgramID") > 0 Then
                                        iProgramID = dtCustomerProgram.Rows(iRowCounter).Item("ProgramID")
                                    End If
                                End If

                                iProgramYear = 0
                                If dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                                    If dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear") > 0 Then
                                        iProgramYear = dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear")
                                    End If
                                End If

                                If iProgramID > 0 Then
                                    PEModule.InsertDrawingCustomerProgram(strNewDrawingNo, "", iProgramID, iProgramYear)
                                End If

                            Next
                        End If
                    Else
                        lblMessage.Text &= "<br />ERROR: the New Drawing was NOT generated. Make sure all required fields have values."
                    End If
                Else
                    lblMessage.Text &= "<br />ERROR: the New Drawing was NOT generated. There is already a New Drawing Number. Please clear this field first."
                End If
            Else
                lblMessage.Text &= "<br />ERROR: Please make sure to select a row on the list of child parts."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnSubmitApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmitApproval.Click

        Try
            ClearMessages()

            GetTeamMemberInfo()

            'save RFD which will validate approval routing first, then notify
            btnSave_Click(sender, e)

            Dim bContinue As Boolean = True
            Dim bFirstTimeNotification As Boolean = False

            Dim iCommodityID As Integer = 0

            Dim dsCheckFirstTimeNotify As DataSet
            Dim dt As DataTable

            Dim objRFDCustomerProgramBLL As RFDCustomerProgramBLL = New RFDCustomerProgramBLL
            Dim objRFDChildPartBLL As RFDChildPartBLL = New RFDChildPartBLL
            Dim objRFDFacilityBLL As RFDFacilityDeptBLL = New RFDFacilityDeptBLL

            Dim iRowCount As Integer = 0

            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            Dim strEmailSubject As String = ""
            Dim strEmailBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailURL As String = strProdOrTestEnvironment & "RFD/RFD_Detail.aspx?RFDNo="

            ''need to make sure at least 1 program is selected           
            If cbCostingRequired.Checked = True And ViewState("BusinessProcessTypeID") <> 7 Then

                dt = objRFDCustomerProgramBLL.GetRFDCustomerProgram(ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = False Then
                    bContinue = False
                    lblMessage.Text &= "<br />ERROR: At least one Program is required for submission. If a Program does not exist, please contact the Account Manager to submit a support request to the Applications Group to add a new Program."
                End If

            End If

            ''if Designation Type is Finished Good then
            ''need a customer part number to submit
            If ddDesignationType.SelectedValue = "C" Then

                'RFQ type needs new customer part number
                If (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7) And txtNewCustomerPartNo.Text.Trim = "" Then
                    bContinue = False
                End If

                'RFC type needs current or new customer part number
                If ViewState("BusinessProcessTypeID") = 2 And txtCurrentCustomerPartNo.Text.Trim = "" And txtNewCustomerPartNo.Text.Trim = "" Then
                    bContinue = False
                End If

                If bContinue = False Then
                    lblMessage.Text &= "<br />ERROR: A Customer Part Number is required for submission."
                End If
            End If

            'if Child Part, then at least a current PartNo, New Revision, or Name for New/Changed part is assigned.
            If ddDesignationType.SelectedValue <> "C" And (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 2 Or ViewState("BusinessProcessTypeID") = 7) Then
                dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = True Then
                    For iRowCount = 0 To dt.Rows.Count - 1
                        ' ''If dt.Rows(iRowCount).Item("CurrentPartNo").ToString = "" And dt.Rows(iRowCount).Item("NewPartName").ToString = "" And dt.Rows(iRowCount).Item("NewPartRevision").ToString = "" Then
                        If dt.Rows(iRowCount).Item("CurrentPartNo").ToString = "" And dt.Rows(iRowCount).Item("NewPartName").ToString = "" Then
                            bContinue = False
                            Exit For
                        End If
                    Next
                Else
                    bContinue = False
                End If

                If bContinue = False Then
                    lblMessage.Text &= "<br />ERROR: All Raw Materials requires a current Part Number or Part Name for the changed/new part for submission."
                End If
            End If

            dt = objRFDFacilityBLL.GetRFDFacilityDept(ViewState("RFDNo"))
            If commonFunctions.CheckDataTable(dt) = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: At least one UGN Facility is required for submission."
            End If

            If cbCapitalRequired.Checked = True And ViewState("CapitalTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Capital Team Member and try submitting again."
            End If

            If cbCostingRequired.Checked = True And ViewState("CostingTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Costing Team Member and try submitting again."
            End If

            If cbPackagingRequired.Checked = True And ViewState("PackagingTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Packaging Team Member and try submitting again."
            End If

            If cbPlantControllerRequired.Checked = True And ViewState("PlantControllerTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Plant Controller and try submitting again."
            End If

            If cbProcessRequired.Checked = True And ViewState("ProcessTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Process Team Member and try submitting again."
            End If

            If cbProductDevelopmentRequired.Checked = True And ViewState("ProductDevelopmentTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Product Engineering Team Member and try submitting again."
            End If

            If cbPurchasingExternalRFQRequired.Checked = True And ViewState("PurchasingExternalRFQTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Purchasing External RFQ Team Member and try submitting again."
            End If

            If cbPurchasingRequired.Checked = True And ViewState("PurchasingTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Purchasing Contract P.O. Team Member and try submitting again."
            End If

            If cbQualityEngineeringRequired.Checked = True And ViewState("QualityEngineerTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Quality Engineering Team Member and try submitting again."
            End If

            If cbToolingRequired.Checked = True And ViewState("ToolingTeamMemberWorking") = False Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please confirm the Tooling Team Member and try submitting again."
            End If

            If bContinue = True Then
                'check if this is the first time anyone was notified  - check if any subscriptions or any team members have been notified                          
                dsCheckFirstTimeNotify = RFDModule.GetRFDApproval(ViewState("RFDNo"), 0, 0, True, True, True, True, True) 'all subscriptions for RFD
                'if no one has been notified yet, then it is first time
                bFirstTimeNotification = Not commonFunctions.CheckDataSet(dsCheckFirstTimeNotify)

                'get capital if checked
                If cbCapitalRequired.Checked = True Then
                    'open
                    If ViewState("CapitalTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 119, ViewState("CapitalTeamMemberID"), "", 0, 1, "")
                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("CapitalEmail")
                    End If

                End If

                'get packaging if checked
                If cbPackagingRequired.Checked = True Then
                    'Case 0, 1, 5 ' none, open, or rejected -> in-process
                    If ViewState("PackagingTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 108, ViewState("PackagingTeamMemberID"), "", 0, 1, "")
                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("PackagingEmail")
                    End If

                End If

                'get PlantController if checked
                If cbPlantControllerRequired.Checked = True Then
                    'open
                    If ViewState("PlantControllerTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 20, ViewState("PlantControllerTeamMemberID"), "", 0, 1, "")

                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("PlantControllerEmail")
                    End If

                End If

                'get Product Engineering (Development) if checked
                If cbProductDevelopmentRequired.Checked = True Then

                    'make sure team member is not already in either recipient list
                    If ViewState("ProductDevelopmentEmail") <> "" Then
                        If InStr(strEmailCCAddress, ViewState("ProductDevelopmentEmail")) <= 0 And InStr(strEmailToAddress, ViewState("ProductDevelopmentEmail")) <= 0 Then
                            If strEmailToAddress <> "" Then
                                strEmailToAddress &= ";"
                            End If

                            strEmailToAddress &= ViewState("ProductDevelopmentEmail")
                        End If
                    End If

                    'make sure backup team member is not already in either recipient list
                    If ViewState("ProductDevelopmentBackupEmail") <> "" Then
                        If InStr(strEmailCCAddress, ViewState("ProductDevelopmentBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("ProductDevelopmentBackupEmail")) <= 0 Then
                            If strEmailCCAddress <> "" Then
                                strEmailCCAddress &= ";"
                            End If

                            strEmailCCAddress &= ViewState("ProductDevelopmentBackupEmail")
                        End If
                    End If

                    '    Case 0, 1, 5 ' none, open, or rejected -> in-process
                    If ViewState("ProductDevelopmentTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 5, ViewState("ProductDevelopmentTeamMemberID"), "", 0, 2, Today.Date)
                    End If

                End If

                'get process if checked
                If cbProcessRequired.Checked = True Then
                    'open
                    If ViewState("ProcessTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 66, ViewState("ProcessTeamMemberID"), "", 0, 1, "")

                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("ProcessEmail")
                    End If
                End If

                'get tooling if checked
                If cbToolingRequired.Checked = True Then
                    'open
                    If ViewState("ToolingTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 65, ViewState("ToolingTeamMemberID"), "", 0, 1, "")
                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("ToolingEmail")
                    End If

                End If

                'get purchasing for External RFQ if checked
                If cbPurchasingExternalRFQRequired.Checked = True Then

                    '    Case 0, 1, 5 ' none, open, or rejected -> open
                    If ViewState("PurchasingExternalRFQTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 139, ViewState("PurchasingExternalRFQTeamMemberID"), "", 0, 1, "")

                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("PurchasingExternalRFQEmail")
                    End If

                End If

                'get costing if checked 
                If cbCostingRequired.Checked = True Then
                    'if first time or is routing level 1 or only costing
                    If bFirstTimeNotification = True Or cbAffectsCostSheetOnly.Checked = True Then
                        'Or ViewState("BusinessProcessTypeID") > 1
                        'notify first time and then do not need to notify until all of first level routing approves
                        'make sure team member is not already in either recipient list
                        If ViewState("CostingEmail") <> "" Then
                            If InStr(strEmailCCAddress, ViewState("CostingEmail")) <= 0 And InStr(strEmailToAddress, ViewState("CostingEmail")) <= 0 Then
                                If strEmailCCAddress <> "" Then
                                    strEmailCCAddress &= ";"
                                End If

                                strEmailCCAddress &= ViewState("CostingEmail")
                            End If
                        End If

                        'make sure backup team member is not already in either recipient list
                        'always CC backup - 2013-Jan-31 - BSchultz
                        'If ViewState("CostingBackupEmail") <> "" Then
                        If InStr(strEmailCCAddress, ViewState("CostingBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("CostingBackupEmail")) <= 0 Then
                            If strEmailCCAddress <> "" Then
                                strEmailCCAddress &= ";"
                            End If

                            strEmailCCAddress &= ViewState("CostingBackupEmail")
                        End If
                        'End If
                    End If

                    If ViewState("CostingTeamMemberID") > 0 Then
                        If cbAffectsCostSheetOnly.Checked = True Then
                            'Or ViewState("BusinessProcessTypeID") > 1
                            'set in-process
                            RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, ViewState("CostingTeamMemberID"), "", 0, 2, Today.Date)
                        Else
                            'keep open because routing level 1 needs to approve first 
                            RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, ViewState("CostingTeamMemberID"), "", 0, 1, "")
                        End If
                    End If

                End If

                'quality engineer must be checked after everyone except purchasing
                'see rules inside this logic about when QE is notified
                'get quality engineering if checked
                If cbQualityEngineeringRequired.Checked = True Then

                    If bFirstTimeNotification = True Then
                        'make sure team member is not already in either recipient list
                        If ViewState("QualityEngineerEmail") <> "" Then
                            If InStr(strEmailCCAddress, ViewState("QualityEngineerEmail")) <= 0 And InStr(strEmailToAddress, ViewState("QualityEngineerEmail")) <= 0 Then
                                If strEmailCCAddress <> "" Then
                                    strEmailCCAddress &= ";"
                                End If

                                strEmailCCAddress &= ViewState("QualityEngineerEmail")
                            End If
                        End If

                        'make sure backup team member is not already in either recipient list
                        If ViewState("QualityEngineerBackupEmail") <> "" Then
                            If InStr(strEmailCCAddress, ViewState("QualityEngineerBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("QualityEngineerBackupEmail")) <= 0 Then
                                If strEmailCCAddress <> "" Then
                                    strEmailCCAddress &= ";"
                                End If

                                strEmailCCAddress &= ViewState("QualityEngineerBackupEmail")
                            End If
                        End If
                    End If

                    '    Case 0, 1, 5 ' none, open, or rejected -> open
                    If ViewState("QualityEngineerTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 22, ViewState("QualityEngineerTeamMemberID"), "", 0, 1, "")
                    End If

                End If

                'purchasing must be checked last in this list because notification depends upon other departments status
                'get purchasing if checked
                If cbPurchasingRequired.Checked = True Then
                    '    Case 0, 1, 5 ' none, open, or rejected -> open
                    If ViewState("PurchasingTeamMemberID") > 0 Then
                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 7, ViewState("PurchasingTeamMemberID"), "", 0, 1, "")
                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("PurchasingEmail")
                    End If

                End If

                'include RFD Initiator
                If ViewState("InitiatorTeamMemberEmail") <> "" Then
                    'make sure backup team member is not already in either recipient list
                    If InStr(strEmailCCAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 And InStr(strEmailToAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 Then
                        If strEmailCCAddress.Trim <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("InitiatorTeamMemberEmail")
                    End If
                End If

                'include Account Manager on initial notification
                If ViewState("AccountManagerEmail") <> "" And bFirstTimeNotification = True Then
                    'make sure backup team member is not already in either recipient list
                    If InStr(strEmailCCAddress, ViewState("AccountManagerEmail")) <= 0 And InStr(strEmailToAddress, ViewState("AccountManagerEmail")) <= 0 Then
                        If strEmailCCAddress.Trim <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("AccountManagerEmail")
                    End If
                End If

                'include Program Manager on initial notification
                If ViewState("ProgramManagerEmail") <> "" And bFirstTimeNotification = True _
                    And (ViewState("BusinessProcessTypeID") = 1 Or (ViewState("BusinessProcessTypeID") = 7 And ViewState("BusinessProcessActionID") = 10)) Then
                    If InStr(strEmailToAddress, ViewState("ProgramManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProgramManagerEmail")) <= 0 Then
                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("ProgramManagerEmail")
                    End If
                End If

                'CC Default RnD Team Member and  email addresses on initial notification
                If cbRDrequired.Checked = True And bFirstTimeNotification = True And ViewState("RndEmail") <> "" Then
                    'make sure backup team member is not already in either recipient list
                    If InStr(strEmailCCAddress, ViewState("RndEmail")) <= 0 And InStr(strEmailToAddress, ViewState("RndEmail")) <= 0 Then
                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("RndEmail")
                    End If
                End If

                'CC team members first time for all but the Quote Only type
                If bFirstTimeNotification = True And ViewState("RFDccEmailList") <> "" And ViewState("BusinessProcessTypeID") <> 7 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("RFDccEmailList")
                End If

                'CC Default Program Management first time for Customer Driven Changes
                If bFirstTimeNotification = True And ViewState("DefaultProgramManagementEmail") <> "" _
                    And (ViewState("BusinessProcessTypeID") = 1 Or (ViewState("BusinessProcessTypeID") = 7 And ViewState("BusinessProcessActionID") = 10)) Then

                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("DefaultProgramManagementEmail")
                End If

                'cc Product Design if Damper Commodity
                If ddWorkFlowCommodity.SelectedIndex > 0 Then
                    iCommodityID = ddWorkFlowCommodity.SelectedValue
                ElseIf ddNewCommodity.SelectedIndex > 0 Then
                    iCommodityID = ddNewCommodity.SelectedValue
                End If

                If iCommodityID > 0 Then
                    If isDamper(iCommodityID) = True And ViewState("ProductDesignEmailList") <> "" Then
                        If strEmailCCAddress <> "" Then
                            strEmailCCAddress &= ";"
                        End If

                        strEmailCCAddress &= ViewState("ProductDesignEmailList")
                    End If
                End If

                Dim iOverallStatusID As Integer = 2 'set to in-process

                'update over RFD status to be inprocess
                RFDModule.UpdateRFDOverallStatus(ViewState("RFDNo"), 2)

                ddStatus.SelectedValue = 2
                ViewState("StatusID") = 2

                ''''''''''''''''''''''''''''''''''
                ''Build Email
                ''''''''''''''''''''''''''''''''''
                'assign email subject
                strEmailSubject = "APPROVAL REQUEST - RFDNo: " & ViewState("RFDNo") & " is ready for review"

                'build email body
                strEmailBody = "<font size='2' face='Verdana'>RFD No: <b>" & ViewState("RFDNo") & "</b></font><br /><br />"

                If ViewState("DMSDrawingNoUpdate") = "" And ViewState("QuoteOnlySupDocUpdate") = "" Then
                    strEmailBody &= "<font size='2' face='Verdana'>This has been issued by " & ViewState("InitiatorTeamMemberName") & "</font><br /><br />"
                End If

                If cbMeetingRequired.Checked = True And bFirstTimeNotification = True Then
                    strEmailBody &= "<font size='2' face='Verdana'><b>An RFD Meeting is required.</b></font><br /><br />"
                End If

                If ViewState("DMSDrawingNoUpdate") <> "" Then
                    strEmailBody &= "<font size='2' face='Verdana' color='red'>" + ViewState("DMSDrawingNoUpdate") + "</font><br /><br />"
                End If

                If ViewState("QuoteOnlySupDocUpdate") <> "" Then
                    strEmailBody &= "<font size='2' face='Verdana' color='red'>" + ViewState("QuoteOnlySupDocUpdate") + "</font><br /><br />"
                End If

                strEmailBody &= "<font size='2' face='Verdana'>The Request for Development (RFD) is ready for review:</font><br /><br />"

                If ddisCostReduction.SelectedValue = True Then
                    strEmailBody &= "<font size='3' face='Verdana' color='red'><b>THIS IS A COST REDUCTION.</b></font><br /><br />"
                End If


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''' Need to add explanation that Account Managers and R&D do not need to approve anything but are merely copied for informational purposes
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                If cbRDrequired.Checked = True Then
                    strEmailBody &= "<br /><font size='1' face='Verdana' >Please note: Research and Development Team Members do not need to approve anything. They are just being copied for informational purposes.</font>"
                End If

                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & ViewState("RFDNo") & "'>Click here to review</a></font><br /><br />"

                If txtCopyReason.Text.Trim <> "" Then
                    strEmailBody &= "<font size='4' face='Verdana' color='red'><b>COPY REASON : " & txtCopyReason.Text.Trim & "</b></font><br />"
                End If

                strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtRFDDesc.Text.Trim & "</font><br />"

                If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
                    '    lblMessage.Text &= "Notfication Sent."
                    'Else
                    '    lblMessage.Text &= "Notfication Failed. Please contact IS."
                End If

                ViewState("DMSDrawingNoUpdate") = ""
                ViewState("QuoteOnlySupDocUpdate") = ""

                gvApproval.DataBind()

                EnableControls()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApproval.Text = lblMessage.Text
        lblMessageApprovalBottom.Text = lblMessage.Text

    End Sub

    ' ''Protected Sub gvCurrentFinishedGood_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCurrentFinishedGood.SelectedIndexChanged

    ' ''    Try
    ' ''        ClearMessages()

    ' ''        Dim strPartNo As String = Replace(gvCurrentFinishedGood.Rows(gvCurrentFinishedGood.SelectedIndex).Cells(0).Text, "&nbsp;", "")

    ' ''        If strPartNo <> "" Then
    ' ''            GetCustomerProgram(strPartNo)
    ' ''        End If

    ' ''    Catch ex As Exception

    ' ''        'get current event name
    ' ''        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    ' ''        'update error on web page
    ' ''        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    ' ''        'log and email error
    ' ''        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    ' ''    End Try

    ' ''End Sub

    Protected Sub iBtnCurrentChildDrawingCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnCurrentChildDrawingCopy.Click

        Try
            ClearMessages()

            GetCurrentChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnCurrentChildCopyAMD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopyAMD.Click

        Try
            ClearMessages()

            txtNewChildAMDValue.Text = txtCurrentChildAMDValue.Text
            txtNewChildAMDTolerance.Text = txtCurrentChildAMDTolerance.Text
            ddNewChildAMDUnits.SelectedValue = ddCurrentChildAMDUnits.SelectedValue

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentChildCopyConstruction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopyConstruction.Click

        Try
            ClearMessages()

            txtNewChildConstruction.Text = txtCurrentChildConstruction.Text

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentChildCopyDensity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopyDensity.Click

        Try
            ClearMessages()

            txtNewChildDensityValue.Text = txtCurrentChildDensityValue.Text
            txtNewChildDensityTolerance.Text = txtCurrentChildDensityTolerance.Text
            txtNewChildDensityUnits.Text = txtCurrentChildDensityUnits.Text

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentChildCopyDesignationType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopyDesignationType.Click

        Try
            ClearMessages()

            ddNewChildDesignationType.SelectedValue = ddCurrentChildDesignationType.SelectedValue

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentChildCopyNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopyNotes.Click

        Try
            ClearMessages()

            txtNewChildDrawingNotes.Text = txtCurrentChildDrawingNotes.Text

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentChildCopyPurchasedGood_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopyPurchasedGood.Click

        Try
            ClearMessages()

            ddNewChildPurchasedGood.SelectedValue = ddCurrentChildPurchasedGood.SelectedValue

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentChildCopySubfamily_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopySubfamily.Click

        Try
            ClearMessages()

            ddNewChildFamily.SelectedValue = ddCurrentChildFamily.SelectedValue
            ddNewChildSubFamily.SelectedValue = ddCurrentChildSubFamily.SelectedValue

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub btnCurrentChildCopyWMD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentChildCopyWMD.Click

        Try
            ClearMessages()

            txtNewChildWMDValue.Text = txtCurrentChildWMDValue.Text
            txtNewChildWMDTolerance.Text = txtCurrentChildWMDTolerance.Text
            ddNewChildWMDUnits.SelectedValue = ddCurrentChildWMDUnits.SelectedValue

            CompareCurrentAndNewChildDrawing()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub ddNewFGFamily_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddNewFGFamily.SelectedIndexChanged

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim iFamilyID As Integer = 0

            If ddNewFGFamily.SelectedIndex > 0 Then
                iFamilyID = ddNewFGFamily.SelectedValue
            End If

            ds = commonFunctions.GetSubFamily(iFamilyID)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddNewFGSubFamily.DataSource = ds
                ddNewFGSubFamily.DataTextField = ds.Tables(0).Columns("subFamilyName").ColumnName
                ddNewFGSubFamily.DataValueField = ds.Tables(0).Columns("subFamilyID").ColumnName
                ddNewFGSubFamily.DataBind()
                ddNewFGSubFamily.Items.Insert(0, "")
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

    Protected Sub ddNewChildFamily_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddNewChildFamily.SelectedIndexChanged

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim iFamilyID As Integer = 0

            If ddNewChildFamily.SelectedIndex > 0 Then
                iFamilyID = ddNewChildFamily.SelectedValue
            End If

            ds = commonFunctions.GetSubFamily(iFamilyID)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddNewChildSubFamily.DataSource = ds
                ddNewChildSubFamily.DataTextField = ds.Tables(0).Columns("subFamilyName").ColumnName
                ddNewChildSubFamily.DataValueField = ds.Tables(0).Columns("subFamilyID").ColumnName
                ddNewChildSubFamily.DataBind()
                ddNewChildSubFamily.Items.Insert(0, "")
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

    Protected Sub btnBusinessAwarded_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBusinessAwarded.Click

        Try
            '04/18/2012 Notify RFD Approvers, Program Managers, and Director of Materials

            ClearMessages()

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "RFD/crRFD_Preview.aspx?RFDNo=" & ViewState("RFDNo")
            Dim strEmailDetailURL As String = strProdOrTestEnvironment & "RFD/RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo")

            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailBody As String = ""

            GetTeamMemberInfo()

            'update RFD Business Awarded date
            RFDModule.UpdateRFDBusinessAwarded(ViewState("RFDNo"))

            ViewState("bBusinessAwarded") = True
            lblBusinessAwardedDateValue.Text = Today.Date
            lblBusinessAwaredDateLabel.Visible = True
            lblBusinessAwardedDateValue.Visible = True

            'update history
            RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "BUSINESS AWARDED")

            If ViewState("InitiatorTeamMemberEmail") <> "" Then
                'make sure backup team member is not already in either recipient list
                If InStr(strEmailCCAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 And InStr(strEmailToAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 Then
                    If strEmailCCAddress.Trim <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("InitiatorTeamMemberEmail")
                End If
            End If

            'Account Manager
            If ViewState("AccountManagerEmail") <> "" Then
                If InStr(strEmailToAddress, ViewState("AccountManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("AccountManagerEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("AccountManagerEmail")
                End If
            End If

            'Program Manager
            If ViewState("ProgramManagerEmail") <> "" Then
                If InStr(strEmailToAddress, ViewState("ProgramManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProgramManagerEmail")) <= 0 Then
                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    strEmailToAddress &= ViewState("ProgramManagerEmail")
                End If
            End If

            'capital
            If ViewState("CapitalEmail") <> "" And cbCapitalRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("CapitalEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("CapitalEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("CapitalEmail")
                End If
            End If

            'costing
            If ViewState("CostingEmail") <> "" And cbCostingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("CostingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("CostingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("CostingEmail")
                End If
            End If

            'packaging
            If ViewState("PackagingEmail") <> "" And cbPackagingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("PackagingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PackagingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PackagingEmail")
                End If
            End If

            'plant controller
            If ViewState("PlantControllerEmail") <> "" And cbPlantControllerRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("PlantControllerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PlantControllerEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PlantControllerEmail")
                End If
            End If

            'process
            If ViewState("ProcessEmail") <> "" And cbProcessRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ProcessEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProcessEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ProcessEmail")
                End If
            End If

            'product development
            If ViewState("ProductDevelopmentEmail") <> "" And cbProductDevelopmentRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ProductDevelopmentEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProductDevelopmentEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ProductDevelopmentEmail")
                End If
            End If

            'tooling
            If ViewState("ToolingEmail") <> "" And cbToolingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ToolingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ToolingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ToolingEmail")
                End If
            End If

            'CC Default Program Management
            If ViewState("DefaultProgramManagementEmail") <> "" Then
                If InStr(strEmailToAddress, ViewState("DefaultProgramManagementEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("DefaultProgramManagementEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("DefaultProgramManagementEmail")
                End If
            End If

            'CC Director Of Materials
            If ViewState("DirectorOfMaterialsEmail") <> "" Then
                If InStr(strEmailToAddress, ViewState("DirectorOfMaterialsEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("DirectorOfMaterialsEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("DirectorOfMaterialsEmail")
                End If
            End If

            'assign email subject
            strEmailSubject = "RFDNo: " & ViewState("RFDNo") & " has been awarded business to UGN "

            If ViewState("AllApproved") = True Then
                strEmailSubject &= " and approved by all."

                RFDModule.UpdateRFDOverallStatus(ViewState("RFDNo"), 3)

                ddStatus.SelectedValue = 3
                ViewState("StatusID") = 3

                'update history
                RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "All required team members have completed the RFD.")

            End If

            'build email body
            strEmailBody = "<font size='2' face='Verdana'>The following Request for Development has been awarded business to UGN.</font><br /><br />"

            If ViewState("AllApproved") = True Then
                strEmailBody &= "<br /><font size='2' face='Verdana'>It has also been approved by all.</font><br /><br />"
            End If

            strEmailBody &= "<font size='2' face='Verdana'>RFDNo: <b>" & ViewState("RFDNo") & "</b></font><br /><br />"

            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailDetailURL & "'>Team Members can click here to take action</a></font><br /><br />"

            strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtRFDDesc.Text.Trim & "</font><br />"

            If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
                '    lblMessage.Text &= "Notfication Sent."
                'Else
                '    lblMessage.Text &= "Notfication Failed. Please contact IS."
            End If

            gvApproval.DataBind()

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub GetTeamMemberInfo()

        Try

            Dim ds As DataSet
            Dim dsTeamMember As DataSet
            Dim dsBackup As DataSet

            Dim dt As DataTable

            Dim objRFDFacilityDeptBLL As RFDFacilityDeptBLL = New RFDFacilityDeptBLL
            Dim strUGNFacility As String = ""

            Dim objRFDCustomerProgram As RFDCustomerProgramBLL = New RFDCustomerProgramBLL
            Dim strMake As String = ""
            Dim iCustomerProgramRowCounter As Integer = 0

            Dim iRowCounter As Integer = 0
            Dim iRowCounter2 As Integer = 0

            Dim iTempTeamMemberID As Integer = 0
            Dim bTempTeamMemberWorking As Boolean = False
            Dim iTempSubscriptionID As Integer = 0            
            Dim iTempStatusID As Integer = 0
            Dim iTempCavityCount As Integer = 0

            Dim strTempTeamMemberEmail As String = ""
            Dim strTempTeamMemberName As String = ""

            ViewState("ApproverCount") = 0

            ViewState("AllApprovedBeforeCosting") = False
            ViewState("AllApprovedBeforeQualityEngineer") = False
            ViewState("AllApprovedBeforePurchasing") = False
            ViewState("AllApproved") = False

            ViewState("AccountManagerEmail") = ""
            ViewState("AccountManagerName") = ""

            ViewState("InitiatorTeamMemberEmail") = ""
            ViewState("InitiatorTeamMemberName") = ""

            ViewState("RnDEmail") = ""
            ViewState("RnDTeamMemberName") = ""

            ViewState("CapitalTeamMemberID") = 0
            ViewState("CapitalTeamMemberName") = ""
            ViewState("CapitalTeamMemberWorking") = False
            ViewState("CapitalStatusID") = 0
            ViewState("CapitalEmail") = ""
            ViewState("CapitalBackupEmail") = ""

            ViewState("CostingTeamMemberID") = 0
            ViewState("CostingTeamMemberName") = ""
            ViewState("CostingTeamMemberWorking") = False
            ViewState("CostingStatusID") = 0
            ViewState("CostingEmail") = ""
            ViewState("CostingBackupEmail") = ""

            ViewState("DirectorOfMaterialsEmail") = ""

            ViewState("PackagingTeamMemberID") = 0
            ViewState("PackagingTeamMemberName") = ""
            ViewState("PackagingTeamMemberWorking") = False
            ViewState("PackagingStatusID") = 0
            ViewState("PackagingEmail") = ""
            ViewState("PackagingBackupEmail") = ""

            ViewState("PlantControllerTeamMemberID") = 0
            ViewState("PlantControllerTeamMemberName") = ""
            ViewState("PlantControllerTeamMemberWorking") = False
            ViewState("PlantControllerStatusID") = 0
            ViewState("PlantControllerEmail") = ""
            ViewState("PlantControllerBackUpEmail") = ""

            ViewState("DefaultPlantControllerTeamMemberID") = 0
            ViewState("DefaultPlantControllerEmail") = ""

            ViewState("ProcessTeamMemberID") = 0
            ViewState("ProcessTeamMemberName") = ""
            ViewState("ProcessTeamMemberWorking") = False
            ViewState("ProcessStatusID") = 0
            ViewState("ProcessEmail") = ""
            ViewState("ProcessBackupEmail") = ""

            ViewState("DefaultProductDevelopmentTeamMemberID") = 0
            ViewState("ProductDevelopmentTeamMemberID") = 0
            ViewState("ProductDevelopmentTeamMemberName") = ""
            ViewState("ProductDevelopmentTeamMemberWorking") = False
            ViewState("ProductDevelopmentStatusID") = 0
            ViewState("ProductDevelopmentCavityCount") = 0
            ViewState("ProductDevelopmentEmail") = ""
            ViewState("ProductDevelopmentBackupEmail") = ""

            ViewState("DefaultProgramManagementEmail") = ""
            ViewState("ProgramManagementEmail") = ""

            ViewState("DefaultPurchasingTeamMemberID") = 0
            ViewState("PurchasingTeamMemberID") = 0
            ViewState("PurchasingTeamMemberName") = ""
            ViewState("PurchasingTeamMemberWorking") = False
            ViewState("PurchasingStatusID") = 0
            ViewState("PurchasingEmail") = ""
            ViewState("PurchasingBackupEmail") = ""

            ViewState("PurchasingExternalRFQTeamMemberID") = 0
            ViewState("PurchasingExternalRFQTeamMemberName") = ""
            ViewState("PurchasingExternalRFQTeamMemberWorking") = False
            ViewState("PurchasingExternalRFQStatusID") = 0
            ViewState("PurchasingExternalRFQEmail") = ""
            ViewState("PurchasingExternalRFQBackupEmail") = ""

            ViewState("QualityEngineerTeamMemberID") = 0
            ViewState("QualityEngineerTeamMemberName") = ""
            ViewState("QualityEngineerTeamMemberWorking") = False
            ViewState("QualityEngineerStatusID") = 0
            ViewState("QualityEngineerEmail") = ""
            ViewState("QualityEngineerBackupEmail") = ""

            ViewState("ToolingTeamMemberID") = 0
            ViewState("ToolingTeamMemberName") = ""
            ViewState("ToolingTeamMemberWorking") = False
            ViewState("ToolingStatusID") = 0            
            ViewState("ToolingEmail") = ""
            ViewState("ToolingBackupEmail") = ""

            ViewState("RFDccEmailList") = ""

            ViewState("ProductDesignEmailList") = ""

            Dim iAccountManagerID As Integer = 0            
            If ddAccountManager.SelectedIndex > 0 Then
                iAccountManagerID = ddAccountManager.SelectedValue
            End If

            Dim iProgramManagerID As Integer = 0
            If ddProgramManager.SelectedIndex > 0 Then
                iProgramManagerID = ddProgramManager.SelectedValue
            End If

            'Initiator
            dsTeamMember = SecurityModule.GetTeamMember(ViewState("InitiatorTeamMemberID"), "", "", "", "", "", True, Nothing)
            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                ViewState("InitiatorTeamMemberEmail") = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                ViewState("InitiatorTeamMemberName") = dsTeamMember.Tables(0).Rows(0).Item("FirstName").ToString & " " & dsTeamMember.Tables(0).Rows(0).Item("LastName").ToString
            End If

            'Account Manager
            If iAccountManagerID > 0 Then
                dsTeamMember = SecurityModule.GetTeamMember(iAccountManagerID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                    ViewState("AccountManagerEmail") = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    ViewState("AccountManagerName") = dsTeamMember.Tables(0).Rows(0).Item("FirstName").ToString & " " & dsTeamMember.Tables(0).Rows(0).Item("LastName").ToString
                End If
            End If

            'Program Manager
            If iProgramManagerID > 0 Then
                dsTeamMember = SecurityModule.GetTeamMember(iProgramManagerID, "", "", "", "", "", True, Nothing)
                If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                    ViewState("ProgramManagerEmail") = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    'ViewState("ProgramManagerName") = dsTeamMember.Tables(0).Rows(0).Item("FirstName").ToString & " " & dsTeamMember.Tables(0).Rows(0).Item("LastName").ToString
                End If
            End If

            'Default RnD
            ds = commonFunctions.GetTeamMemberBySubscription(48)
            If commonFunctions.CheckDataSet(ds) = True Then
                'get email of Team Member
                If ds.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TMID") > 0 Then
                        dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(0).Item("TMID"), "", "", "", "", "", True, Nothing)
                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                            ViewState("RnDEmail") = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                            ViewState("RnDTeamMemberName") = dsTeamMember.Tables(0).Rows(0).Item("FirstName").ToString & " " & dsTeamMember.Tables(0).Rows(0).Item("LastName").ToString
                        End If
                    End If
                End If

            End If

            'Default Plant Controller for Corporate
            ds = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(109, "UT")
            If commonFunctions.CheckDataSet(ds) = True Then
                'get email of Team Member
                If ds.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TMID") > 0 Then
                        dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(0).Item("TMID"), "", "", "", "", "", True, Nothing)
                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                            ViewState("DefaultPlantControllerEmail") = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                            ViewState("DefaultPlantControllerTeamMemberID") = ds.Tables(0).Rows(0).Item("TMID")
                        End If
                    End If
                End If

            End If

            'each approver
            ds = RFDModule.GetRFDApproval(ViewState("RFDNo"), 0, 0, False, False, False, False, False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ViewState("ApproverCount") = ds.Tables(0).Rows.Count
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    iTempSubscriptionID = 0
                    If ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID") > 0 Then
                            iTempSubscriptionID = ds.Tables(0).Rows(iRowCounter).Item("SubscriptionID")
                        End If
                    End If

                    iTempTeamMemberID = 0
                    bTempTeamMemberWorking = False
                    strTempTeamMemberEmail = ""
                    strTempTeamMemberName = ""
                    If ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID") > 0 Then
                            iTempTeamMemberID = ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID")

                            'team member email
                            dsTeamMember = SecurityModule.GetTeamMember(iTempTeamMemberID, "", "", "", "", "", True, Nothing)
                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                                    bTempTeamMemberWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                                End If

                                strTempTeamMemberEmail = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                strTempTeamMemberName = dsTeamMember.Tables(0).Rows(0).Item("FirstName").ToString & " " & dsTeamMember.Tables(0).Rows(0).Item("LastName").ToString
                            End If
                        End If
                    End If

                    iTempStatusID = 1
                    If ds.Tables(0).Rows(iRowCounter).Item("StatusID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("StatusID") > 0 Then
                            iTempStatusID = ds.Tables(0).Rows(iRowCounter).Item("StatusID")
                        End If
                    End If

                    iTempCavityCount = 0
                    If iTempSubscriptionID = 5 Then
                        If ds.Tables(0).Rows(iRowCounter).Item("CavityCount") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCounter).Item("CavityCount").ToString <> "" Then
                                iTempCavityCount = CType(ds.Tables(0).Rows(iRowCounter).Item("CavityCount"), Integer)
                            End If
                        End If
                    End If
                    
                    'check specific subscriptions for approval

                    'Capital
                    If iTempSubscriptionID = 119 Then
                        ViewState("CapitalStatusID") = iTempStatusID
                        ViewState("CapitalEmail") = strTempTeamMemberEmail
                        ViewState("CapitalTeamMemberID") = iTempTeamMemberID
                        ViewState("CapitalTeamMemberName") = strTempTeamMemberName
                        ViewState("CapitalTeamMemberWorking") = bTempTeamMemberWorking

                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 119)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    ViewState("CapitalBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                    'Costing
                    If iTempSubscriptionID = 6 Then
                        ViewState("CostingStatusID") = iTempStatusID
                        ViewState("CostingEmail") = strTempTeamMemberEmail
                        ViewState("CostingTeamMemberID") = iTempTeamMemberID
                        ViewState("CostingTeamMemberName") = strTempTeamMemberName
                        ViewState("CostingTeamMemberWorking") = bTempTeamMemberWorking

                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 6)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    ViewState("CostingBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                    'Packaging
                    If iTempSubscriptionID = 108 Then
                        ViewState("PackagingStatusID") = iTempStatusID
                        ViewState("PackagingEmail") = strTempTeamMemberEmail
                        ViewState("PackagingTeamMemberID") = iTempTeamMemberID
                        ViewState("PackagingTeamMemberName") = strTempTeamMemberName
                        ViewState("PackagingTeamMemberWorking") = bTempTeamMemberWorking

                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 108)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    'iPackagingBackupID = dsBackup.Tables(0).Rows(0).Item("BackupID")
                                    ViewState("PackagingBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                    'PlantController
                    If iTempSubscriptionID = 20 Then
                        ViewState("PlantControllerStatusID") = iTempStatusID
                        ViewState("PlantControllerEmail") = strTempTeamMemberEmail
                        ViewState("PlantControllerTeamMemberID") = iTempTeamMemberID
                        ViewState("PlantControllerTeamMemberName") = strTempTeamMemberName
                        ViewState("PlantControllerTeamMemberWorking") = bTempTeamMemberWorking

                        'get backup if out - need to select by plant
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 20)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 Then
                                    'Dim objRFDFacilityDeptBLL As RFDFacilityDeptBLL = New RFDFacilityDeptBLL
                                    'Dim dt As DataTable
                                    strUGNFacility = ""

                                    'get first UGN Facility in the list
                                    dt = objRFDFacilityDeptBLL.GetRFDFacilityDept(ViewState("RFDNo"))
                                    If commonFunctions.CheckDataTable(dt) = True Then
                                        strUGNFacility = dt.Rows(0).Item("UGNFacility").ToString

                                        If strUGNFacility <> "" Then

                                            'get default plant controller for that facility
                                            ds = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(109, strUGNFacility)
                                            If commonFunctions.CheckDataSet(ds) = True Then
                                                'get email of Team Member
                                                If ds.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                                                    If ds.Tables(0).Rows(0).Item("TMID") > 0 Then
                                                        dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(0).Item("TMID"), "", "", "", "", "", True, Nothing)
                                                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                                            ViewState("PlantControllerBackupEmail") = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    'Process
                    If iTempSubscriptionID = 66 Then
                        ViewState("ProcessStatusID") = iTempStatusID
                        ViewState("ProcessEmail") = strTempTeamMemberEmail
                        ViewState("ProcessTeamMemberID") = iTempTeamMemberID
                        ViewState("ProcessTeamMemberName") = strTempTeamMemberName
                        ViewState("ProcessTeamMemberWorking") = bTempTeamMemberWorking

                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 66)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    ViewState("ProcessBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                    'Product Development
                    If iTempSubscriptionID = 5 Then
                        ViewState("ProductDevelopmentStatusID") = iTempStatusID
                        ViewState("ProductDevelopmentEmail") = strTempTeamMemberEmail
                        ViewState("ProductDevelopmentTeamMemberID") = iTempTeamMemberID
                        ViewState("ProductDevelopmentTeamMemberName") = strTempTeamMemberName
                        ViewState("ProductDevelopmentTeamMemberWorking") = bTempTeamMemberWorking
                        ViewState("ProductDevelopmentCavityCount") = iTempCavityCount

                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 5)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    ViewState("ProductDevelopmentBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                    'Tooling
                    If iTempSubscriptionID = 65 Then
                        ViewState("ToolingStatusID") = iTempStatusID
                        ViewState("ToolingEmail") = strTempTeamMemberEmail
                        ViewState("ToolingTeamMemberID") = iTempTeamMemberID
                        ViewState("ToolingTeamMemberName") = strTempTeamMemberName
                        ViewState("ToolingTeamMemberWorking") = bTempTeamMemberWorking
                       
                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 65)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    ViewState("ToolingBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                    'Quality Engineer
                    If iTempSubscriptionID = 22 Then
                        ViewState("QualityEngineerStatusID") = iTempStatusID
                        ViewState("QualityEngineerEmail") = strTempTeamMemberEmail
                        ViewState("QualityEngineerTeamMemberID") = iTempTeamMemberID
                        ViewState("QualityEngineerTeamMemberName") = strTempTeamMemberName
                        ViewState("QualityEngineerTeamMemberWorking") = bTempTeamMemberWorking

                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 22)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    ViewState("QualityEngineerBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                    'Purchasing External RFQ
                    If iTempSubscriptionID = 139 Then
                        ViewState("PurchasingExternalRFQStatusID") = iTempStatusID
                        ViewState("PurchasingExternalRFQEmail") = strTempTeamMemberEmail
                        ViewState("PurchasingExternalRFQTeamMemberID") = iTempTeamMemberID
                        ViewState("PurchasingExternalRFQTeamMemberName") = strTempTeamMemberName
                        ViewState("PurchasingExternalRFQTeamMemberWorking") = bTempTeamMemberWorking

                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 139)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    ViewState("PurchasingBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                    'Purchasing
                    If iTempSubscriptionID = 7 Then
                        ViewState("PurchasingStatusID") = iTempStatusID
                        ViewState("PurchasingEmail") = strTempTeamMemberEmail
                        ViewState("PurchasingTeamMemberID") = iTempTeamMemberID
                        ViewState("PurchasingTeamMemberName") = strTempTeamMemberName
                        ViewState("PurchasingTeamMemberWorking") = bTempTeamMemberWorking

                        'get backup if out
                        dsBackup = commonFunctions.GetTeamMemberAlertedBackupOrDeptInCharge(iTempTeamMemberID, 7)
                        If commonFunctions.CheckDataSet(dsBackup) = True Then
                            If dsBackup.Tables(0).Rows(0).Item("BackupID") IsNot System.DBNull.Value Then
                                If dsBackup.Tables(0).Rows(0).Item("BackupID") > 0 And dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString <> "" Then
                                    ViewState("PurchasingBackupEmail") = dsBackup.Tables(0).Rows(0).Item("BackupEmail").ToString
                                End If
                            End If
                        End If
                    End If

                Next
            End If


            'get Default Product Development Team Member ID
            ds = commonFunctions.GetTeamMemberBySubscription(54) 'default Product Development
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TMID") > 0 Then 'default  Product Development found
                        ViewState("DefaultProductDevelopmentTeamMemberID") = ds.Tables(0).Rows(0).Item("TMID")
                    End If
                End If
            End If

            'get Default Purchasing Team Member ID
            ds = commonFunctions.GetTeamMemberBySubscription(53) 'default Purchasing
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("TMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("TMID") > 0 Then 'default  Purchasing Team Member found
                        ViewState("DefaultPurchasingTeamMemberID") = ds.Tables(0).Rows(0).Item("TMID")
                    End If
                End If
            End If

            'check RFD approval routing level info
            If (ViewState("PackagingStatusID") = 3 Or cbPackagingRequired.Checked = False) And _
                   (ViewState("PlantControllerStatusID") = 3 Or cbPlantControllerRequired.Checked = False) And _
                   (ViewState("ProcessStatusID") = 3 Or cbProcessRequired.Checked = False) And _
                   (ViewState("ProductDevelopmentStatusID") = 3 Or cbProductDevelopmentRequired.Checked = False) And _
                   (ViewState("ToolingStatusID") = 3 Or cbToolingRequired.Checked = False) And _
                   (ViewState("PurchasingExternalRFQStatusID") = 3 Or cbPurchasingExternalRFQRequired.Checked = False) Then
                ViewState("AllApprovedBeforeCosting") = True
            End If

            If (ViewState("CostingStatusID") = 3 Or cbCostingRequired.Checked = False) And ViewState("AllApprovedBeforeCosting") = True Then
                ViewState("AllApprovedBeforeQualityEngineer") = True
            End If

            'if cost sheet is only required and is approved by costing, then complete
            If cbAffectsCostSheetOnly.Checked = True And ViewState("CostingStatusID") = 3 Then
                ViewState("AllApproved") = True
            End If

            'check if RFQ, if Tooling, Process, Costing, Product Development, and QE have approved, if so, notify Purchasing 
            If (ViewState("QualityEngineerStatusID") = 3 Or cbQualityEngineeringRequired.Checked = False) And _
                    ViewState("AllApprovedBeforeQualityEngineer") = True Then

                ViewState("AllApprovedBeforePurchasing") = True
            End If

            If (ViewState("PurchasingStatusID") = 3 Or cbPurchasingRequired.Checked = False) And _
                    ViewState("AllApprovedBeforePurchasing") = True Then

                ViewState("AllApproved") = True
            End If

            'RFD CC
            ds = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(111, "UT")
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    'get email of Team Member
                    If ds.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And ds.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                            dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(iRowCounter).Item("TMID"), "", "", "", "", "", True, Nothing)
                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                If InStr(ViewState("RFDccEmailList"), dsTeamMember.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then

                                    If ViewState("RFDccEmailList") <> "" Then
                                        ViewState("RFDccEmailList") &= ";"
                                    End If

                                    ViewState("RFDccEmailList") &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            'RFD CC from each Plant
            Dim objFacility As RFDFacilityDeptBLL = New RFDFacilityDeptBLL
            dt = objFacility.GetRFDFacilityDept(ViewState("RFDNo"))

            If commonFunctions.CheckDataTable(dt) = True Then
                For iRowCounter = 0 To dt.Rows.Count - 1
                    strUGNFacility = dt.Rows(iRowCounter).Item("UGNFacility").ToString

                    If strUGNFacility <> "" And strUGNFacility <> "UT" Then
                        'get each team member for each facility
                        ds = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(111, strUGNFacility)
                        If commonFunctions.CheckDataSet(ds) = True Then
                            For iRowCounter2 = 0 To ds.Tables(0).Rows.Count - 1
                                'get email of Team Member
                                If ds.Tables(0).Rows(iRowCounter2).Item("TMID") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(iRowCounter2).Item("TMID") > 0 And ds.Tables(0).Rows(iRowCounter2).Item("WorkStatus") = True Then
                                        dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(iRowCounter2).Item("TMID"), "", "", "", "", "", True, Nothing)
                                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                            If InStr(ViewState("RFDccEmailList"), dsTeamMember.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then
                                                If ViewState("RFDccEmailList") <> "" Then
                                                    ViewState("RFDccEmailList") &= ";"
                                                End If

                                                ViewState("RFDccEmailList") &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                            End If
                                        End If
                                    End If
                                End If
                            Next 'each team member
                        End If 'results returned on team member list
                    End If 'if UGN Facility NOT corporate

                Next 'each facility
            End If

            'Default Program management
            ds = commonFunctions.GetTeamMemberBySubscription(127)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    'get email of Team Member
                    If ds.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And ds.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                            dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(iRowCounter).Item("TMID"), "", "", "", "", "", True, Nothing)
                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                ViewState("DefaultProgramManagementEmail") = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                            End If
                        End If
                    End If
                Next
            End If

            'Director Of Materials
            ds = commonFunctions.GetTeamMemberBySubscription(137)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    'get email of Team Member
                    If ds.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And ds.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                            dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(iRowCounter).Item("TMID"), "", "", "", "", "", True, Nothing)
                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                ViewState("DirectorOfMaterialsEmail") = dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                            End If
                        End If
                    End If
                Next
            End If

            'Sales assigned to Make        
            'if no program is selected in workflow, such as RFC type, then get info based on customer program tab
            If ddWorkFlowMake.SelectedIndex > 0 Then
                ds = commonFunctions.GetWorkFlowMakeAssignments(ddWorkFlowMake.SelectedValue, 0, 9)
                If commonFunctions.CheckDataSet(ds) = True Then
                    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        If ViewState("InitiatorTeamMemberID") <> ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID") _
                            And iAccountManagerID <> ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID") _
                            And ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID") > 0 _
                            And ds.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then

                            dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID"), "", "", "", "", "", True, Nothing)
                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                If InStr(ViewState("RFDccEmailList"), dsTeamMember.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then
                                    If ViewState("RFDccEmailList") <> "" Then
                                        ViewState("RFDccEmailList") &= ";"
                                    End If

                                    ViewState("RFDccEmailList") &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                End If
                            End If
                        End If
                    Next

                End If
            Else
                dt = objRFDCustomerProgram.GetRFDCustomerProgram(ViewState("RFDNo"))
                If commonFunctions.CheckDataTable(dt) = True Then
                    For iCustomerProgramRowCounter = 0 To dt.Rows.Count - 1
                        strMake = ""

                        If dt.Rows(iCustomerProgramRowCounter).Item("MAKE").ToString <> "" Then
                            strMake = dt.Rows(iCustomerProgramRowCounter).Item("MAKE").ToString
                            ds = commonFunctions.GetWorkFlowMakeAssignments(strMake, 0, 9)
                            If commonFunctions.CheckDataSet(ds) = True Then
                                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                                    If ViewState("InitiatorTeamMemberID") <> ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID") _
                                        And ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID") > 0 _
                                        And ds.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then

                                        dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(iRowCounter).Item("TeamMemberID"), "", "", "", "", "", True, Nothing)
                                        If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                            If InStr(ViewState("RFDccEmailList"), dsTeamMember.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then
                                                If ViewState("RFDccEmailList") <> "" Then
                                                    ViewState("RFDccEmailList") &= ";"
                                                End If

                                                ViewState("RFDccEmailList") &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                            End If
                                        End If
                                    End If
                                Next

                            End If
                        End If
                    Next
                End If
            End If

            'RFD CC Product Design for Dampers
            ds = commonFunctions.GetTeamMemberBySubscription(144)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    'get email of Team Member
                    If ds.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And ds.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                            dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(iRowCounter).Item("TMID"), "", "", "", "", "", True, Nothing)
                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                If InStr(ViewState("RFDccEmailList"), dsTeamMember.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then

                                    If ViewState("ProductDesignEmailList") <> "" Then
                                        ViewState("ProductDesignEmailList") &= ";"
                                    End If

                                    ViewState("ProductDesignEmailList") &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Function isDamper(ByVal CommodityID As Integer) As Boolean

        Dim bResult As Boolean = False

        Try

            'if dampers based on Commodity ID
            Select Case CommodityID
                Case 17, 33, 60, 61, 62, 73
                    bResult = True
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return bResult

    End Function

    Protected Sub btnSaveReplyComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveReplyComment.Click

        Try

            ClearMessages()

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            'Dim strEmailApproveURL As String = strProdOrTestEnvironment & "RFD/crRFD_Approval.aspx?RFDNo=" & ViewState("RFDNo")
            Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "RFD/crRFD_Preview.aspx?RFDNo=" & ViewState("RFDNo")
            Dim strEmailDetailURL As String = strProdOrTestEnvironment & "RFD/RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo")

            Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim strEmailBody As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            Dim iBusinessProcessTypeID As Integer = 0

            If ViewState("CurrentRSSID") > 0 Then

                If ddBusinessProcessType.SelectedIndex >= 0 Then
                    iBusinessProcessTypeID = ddBusinessProcessType.SelectedValue
                End If

                'save comment
                RFDModule.InsertRFDRSSReply(ViewState("RFDNo"), ViewState("CurrentRSSID"), ViewState("TeamMemberID"), txtReply.Text.Trim)

                'update history
                RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Message Sent")

                gvQuestion.DataBind()

                GetTeamMemberInfo()

                If ViewState("InitiatorTeamMemberEmail") <> "" Then
                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    strEmailToAddress &= ViewState("InitiatorTeamMemberEmail")
                End If

                'costing
                If ViewState("CostingEmail") <> "" Then
                    If InStr(strEmailToAddress, ViewState("CostingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("CostingEmail")) <= 0 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= ViewState("CostingEmail")
                    End If
                End If

                'process
                If ViewState("ProcessEmail") <> "" Then
                    If InStr(strEmailToAddress, ViewState("ProcessEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProcessEmail")) <= 0 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= ViewState("ProcessEmail")
                    End If
                End If

                'product development
                If ViewState("ProductDevelopmentEmail") <> "" Then
                    If InStr(strEmailToAddress, ViewState("ProductDevelopmentEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProductDevelopmentEmail")) <= 0 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= ViewState("ProductDevelopmentEmail")
                    End If
                End If

                'tooling
                If ViewState("ToolingEmail") <> "" Then
                    If InStr(strEmailToAddress, ViewState("ToolingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ToolingEmail")) <= 0 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= ViewState("ToolingEmail")
                    End If
                End If

                'Quality Engineering
                If ViewState("QualityEngineeringEmail") <> "" And ViewState("NotifyQualityEngineer") = True Then
                    If InStr(strEmailToAddress, ViewState("QualityEngineeringEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("QualityEngineeringEmail")) <= 0 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= ViewState("QualityEngineeringEmail")
                    End If
                End If

                'purchasing for external RFQ
                If ViewState("PurchasingExternalRFQEmail") <> "" Then
                    If InStr(strEmailToAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= ViewState("PurchasingExternalRFQEmail")
                    End If
                End If


                'purchasing for contract PO
                If ViewState("PurchasingEmail") <> "" And ViewState("AllApprovedBeforePurchasing") = True Then
                    If InStr(strEmailToAddress, ViewState("PurchasingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingEmail")) <= 0 Then
                        If strEmailToAddress <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= ViewState("PurchasingEmail")
                    End If
                End If


                'If (iBusinessProcessTypeID = 1 And ViewState("bBusinessAwarded") = True) Or iBusinessProcessTypeID > 1 Then

                ''Quality Engineering
                'If ViewState("QualityEngineeringEmail") <> "" And ViewState("NotifyQualityEngineer") = True Then
                '    If InStr(strEmailToAddress, ViewState("QualityEngineeringEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("QualityEngineeringEmail")) <= 0 Then
                '        If strEmailToAddress <> "" Then
                '            strEmailToAddress &= ";"
                '        End If

                '        strEmailToAddress &= ViewState("QualityEngineeringEmail")
                '    End If
                'End If

                ''purchasing
                'If ViewState("PurchasingEmail") <> "" And ViewState("AllApprovedBeforePurchasing") = True Then
                '    If InStr(strEmailToAddress, ViewState("PurchasingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingEmail")) <= 0 Then
                '        If strEmailToAddress <> "" Then
                '            strEmailToAddress &= ";"
                '        End If

                '        strEmailToAddress &= ViewState("PurchasingEmail")
                '    End If
                'End If

                '''''''''''''''''''''''''''''''''''
                ' ''Build Email
                '''''''''''''''''''''''''''''''''''

                'assign email subject
                strEmailSubject = "RFD Question - RFD No: " & ViewState("RFDNo") & " - MESSAGE RECEIVED"

                strEmailBody = "<table><tr><td valign='top' width='20%'><img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger.jpg'  height='20%' width='20%'/></td><td valign='top'>"
                'strEmailBody &= "<font size='3' face='Verdana'><b>Attention</b> "
                strEmailBody &= "<font size='3' face='Verdana'><p><b>" & strCurrentUserFullName & "</b> replied to the message regarding regarding RFDNo: <font color='red'>" & ViewState("RFDNo") & "</font><br />"

                If txtQuestionComment.Text.Trim <> "" Then
                    strEmailBody &= "<p><b><font color='red' size='6'>Question:</font> </b><font color='green' size='6'>" & txtQuestionComment.Text.Trim & "</font></p><br /><br />"
                End If

                If txtReply.Text.Trim <> "" Then
                    strEmailBody &= "<p><b><font color='red' size='6'>Reply: </font></b><font color='green' size='6'>" & txtReply.Text.Trim & "</font></p><hr /><br /><br />"
                End If

                If txtCopyReason.Text.Trim <> "" Then
                    strEmailBody &= "<font size='4' face='Verdana' color='red'><b>COPY REASON: " & txtCopyReason.Text.Trim & "</b></font><br />"
                End If

                strEmailBody &= "<font size='3' face='Verdana'><p><b>Description: </b> <font>" & txtRFDDesc.Text.Trim & "</font>.</p><br />"

                strEmailBody &= "<p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/RFD/RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo") & "&pRC=1" & "'>Click here</a> if you need to respond.</font>"
                strEmailBody &= "</td></tr></table>"

                If strEmailToAddress <> "" And strEmailSubject <> "" And strEmailBody <> "" Then
                    If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
                        '    lblMessage.Text &= "Notfication Sent."
                        'Else
                        '    lblMessage.Text &= "Notfication Failed. Please contact IS."
                    End If
                End If

                txtQuestionComment.Text = ""
                txtReply.Text = ""

                ViewState("CurrentRSSID") = 0

                btnResetReplyComment.Visible = False
                btnSaveReplyComment.Visible = False

                'End If
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

    Protected Sub btnResetReplyComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetReplyComment.Click

        Try

            ClearMessages()

            txtReply.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click

        Try

            ClearMessages()

            ViewState("StatusID") = 4

            InitializeAllControls()

            lblVoidComment.Visible = True
            lblVoidCommentMarker.Visible = True
            txtVoidComment.Visible = True
            txtVoidComment.Enabled = True

            btnVoid.Attributes.Add("onclick", "")

            btnCopy.Visible = False
            btnPreview.Visible = False
            btnVoid.Visible = True

            rbCopyType.Visible = False

            If txtVoidComment.Text.Trim <> "" Then

                ddStatus.SelectedValue = 4

                RFDModule.DeleteRFD(ViewState("RFDNo"), txtVoidComment.Text.Trim)

                lblMessage.Text = "The RFD has been voided.<br>"

                btnVoid.Visible = False
                btnVoidCancel.Visible = False

                'update history
                RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Voided")

                NotifyStoppedRFD("voided")

            Else
                lblMessage.Text &= "To void, please fill in the reason in the Void Comment field and then CLICK THE VOID BUTTON AGAIN."
                txtVoidComment.Focus()
                btnVoidCancel.Visible = ViewState("isEdit")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnVoidCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoidCancel.Click

        Try
            ClearMessages()

            Response.Redirect("RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo"), False)

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Sub NotifyStoppedRFD(ByVal StopType As String)

        Try
            GetTeamMemberInfo()

            Dim iCommodityID As Integer = 0
            Dim ds As DataSet

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "RFD/crRFD_Preview.aspx?RFDNo=" & ViewState("RFDNo")
            Dim strEmailDetailURL As String = strProdOrTestEnvironment & "RFD/RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo")

            Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim strEmailBody As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""


            'assign email subject
            strEmailSubject = "RFDNo: " & ViewState("RFDNo") & " has been " & StopType & "."

            'build email body
            strEmailBody = "<font size='2' face='Verdana'>The following Request for Development has been " & StopType & ".<br/>Pending Activities should be stopped.</font><br /><br />"
            strEmailBody &= "<font size='2' face='Verdana'>RFDNo: <b>" & ViewState("RFDNo") & "</b></font><br /><br />"

            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailPreviewURL & "'>Team Members can click here to preview the information</a></font><br /><br />"

            If txtCloseComment.Text.Trim <> "" Then
                strEmailBody &= "<font size='4' face='Verdana' color='red'><b>REASON FOR CLOSE: " & txtCloseComment.Text.Trim & "</b></font><br />"
            End If

            If txtVoidComment.Text.Trim <> "" Then
                strEmailBody &= "<font size='4' face='Verdana' color='red'><b>REASON FOR VOID: " & txtVoidComment.Text.Trim & "</b></font><br />"
            End If

            strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtRFDDesc.Text.Trim & "</font><br />"

            If ViewState("InitiatorTeamMemberEmail") <> "" Then
                'make sure backup team member is not already in either recipient list
                If InStr(strEmailCCAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 And InStr(strEmailToAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 Then
                    If strEmailToAddress.Trim <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    strEmailToAddress &= ViewState("InitiatorTeamMemberEmail")
                End If
            End If

            If ViewState("AccountManagerEmail") <> "" And (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7) Then
                If InStr(strEmailToAddress, ViewState("AccountManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("AccountManagerEmail")) <= 0 Then
                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    strEmailToAddress &= ViewState("AccountManagerEmail")
                End If
            End If

            If ViewState("ProgramManagerEmail") <> "" And (ViewState("BusinessProcessTypeID") = 1 Or (ViewState("BusinessProcessTypeID") = 7 And ViewState("BusinessProcessActionID") = 10)) Then
                If InStr(strEmailToAddress, ViewState("ProgramManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProgramManagerEmail")) <= 0 Then
                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    strEmailToAddress &= ViewState("ProgramManagerEmail")
                End If
            End If

            'costing
            If ViewState("CostingEmail") <> "" And cbCostingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("CostingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("CostingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("CostingEmail")
                End If
            End If

            'Packaging
            If ViewState("PackagingEmail") <> "" And cbPackagingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("PackagingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PackagingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PackagingEmail")
                End If
            End If

            'PlantController
            If ViewState("PlantControllerEmail") <> "" And cbPlantControllerRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("PlantControllerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PlantControllerEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PlantControllerEmail")
                End If
            End If

            'process
            If ViewState("ProcessEmail") <> "" And cbProcessRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ProcessEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProcessEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ProcessEmail")
                End If
            End If

            'product development
            If ViewState("ProductDevelopmentEmail") <> "" And cbProductDevelopmentRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ProductDevelopmentEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProductDevelopmentEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ProductDevelopmentEmail")
                End If
            End If

            'Purchasing for External RFQ
            If ViewState("PurchasingExternalRFQEmail") <> "" And cbPurchasingExternalRFQRequired.Checked Then
                If InStr(strEmailToAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PurchasingExternalRFQEmail")
                End If
            End If

            'Purchasing for Contract PO
            If ViewState("PurchasingEmail") <> "" And cbPurchasingRequired.Checked Then
                If InStr(strEmailToAddress, ViewState("PurchasingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PurchasingEmail")
                End If
            End If

            'QualityEngineer
            If ViewState("QualityEngineerEmail") <> "" And cbQualityEngineeringRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("QualityEngineerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("QualityEngineerEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("QualityEngineerEmail")
                End If
            End If

            'tooling
            If ViewState("ToolingEmail") <> "" And cbToolingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ToolingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ToolingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ToolingEmail")
                End If
            End If

            If ViewState("RFDccEmailList") <> "" Then
                If strEmailCCAddress <> "" Then
                    strEmailCCAddress &= ";"
                End If

                strEmailCCAddress &= ViewState("RFDccEmailList")
            End If

            'cc Product Design if Damper Commodity                           
            If ddWorkFlowCommodity.SelectedIndex > 0 Then
                iCommodityID = ddWorkFlowCommodity.SelectedValue
            ElseIf ddNewCommodity.SelectedValue > 0 Then
                iCommodityID = ddNewCommodity.SelectedValue
            End If

            If iCommodityID > 0 Then
                If isDamper(iCommodityID) = True And ViewState("ProductDesignEmailList") <> "" Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ProductDesignEmailList")
                End If
            End If

            'wait until all this logic above is determined before sending emails
            If strEmailToAddress <> "" And strEmailSubject <> "" And strEmailBody <> "" Then

                'append original approver
                If ViewState("TeamMemberID") <> ViewState("OriginalApproverID") And ViewState("OriginalApproverID") > 0 Then
                    ds = SecurityModule.GetTeamMember(ViewState("OriginalApproverID"), "", "", "", "", "", True, Nothing)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        If InStr(strEmailToAddress, ds.Tables(0).Rows(0).Item("Email").ToString) <= 0 And _
                        InStr(strEmailCCAddress, ds.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then
                            If strEmailCCAddress <> "" Then
                                strEmailCCAddress &= ";"
                            End If

                            strEmailCCAddress &= ds.Tables(0).Rows(0).Item("Email").ToString
                        End If
                    End If
                End If

                If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
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

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click

        Try

            ClearMessages()

            ViewState("StatusID") = 8

            InitializeAllControls()

            lblCloseComment.Visible = True
            lblCloseCommentMarker.Visible = True
            txtCloseComment.Visible = True
            txtCloseComment.Enabled = True

            btnClose.Attributes.Add("onclick", "")

            btnCopy.Visible = False
            btnPreview.Visible = False
            btnClose.Visible = True

            rbCopyType.Visible = False

            If txtCloseComment.Text.Trim <> "" Then

                ddStatus.SelectedValue = 8
                ViewState("StatusID") = 8

                RFDModule.UpdateRFDClose(ViewState("RFDNo"), txtCloseComment.Text.Trim)
              
                lblMessage.Text = "The RFD has been Closed.<br />"

                btnClose.Visible = False
                btnCloseCancel.Visible = False

                'update history
                RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Closed")

                NotifyStoppedRFD("closed")

            Else
                lblMessage.Text &= "To Close, please fill in the reason in the Close Comment field and then CLICK THE Close BUTTON AGAIN."
                txtCloseComment.Focus()
                btnCloseCancel.Visible = ViewState("isEdit")
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

    Protected Sub btnCloseCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseCancel.Click

        Try
            lblMessage.Text = ""

            Response.Redirect("RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo"), False)

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub ddInsertUGNFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data to the Edit WorkCenter drop down list based on UGNFacility Selection
        '' from the Edittemplate in the grid view.
        ''*******

        ClearMessages()

        Try
            Dim ddTempUGNFacility As DropDownList
            'Dim ddTempDepartment As DropDownList

            'Dim ds As DataSet

            'Dim iRowCounter As Integer = 0
            Dim strUGNFacility As String = ""

            ddTempUGNFacility = CType(sender, DropDownList)
            'ddTempDepartment = CType(gvFacilityDept.FooterRow.FindControl("ddInsertDepartment"), DropDownList)

            If ddTempUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddTempUGNFacility.SelectedValue
            Else
                strUGNFacility = ddTempUGNFacility.Items(0).Value
            End If

            ViewState("SelectedFacility") = strUGNFacility

            'ddTempDepartment.Items.Clear()
            'ds = commonFunctions.GetDepartmentGLNo(strUGNFacility)
            'If commonFunctions.CheckDataSet(ds) = True Then
            '    ddTempDepartment.DataSource = ds
            '    ddTempDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
            '    ddTempDepartment.DataValueField = ds.Tables(0).Columns("DepartmentID").ColumnName
            '    ddTempDepartment.DataBind()
            'Else
            '    ddTempDepartment.Items.Insert(0, "")
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub gvFacilityDept_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFacilityDept.RowDataBound

        Dim ds As DataSet

        Dim ddTempUGNFacility As DropDownList
        Dim ddTempDepartment As DropDownList

        Dim strUGNFacility As String = ""
        Dim iDepartmentID As Integer = 0

        'If (e.Row.RowType = DataControlRowType.DataRow) Then

        '    ddTempUGNFacility = CType(e.Row.FindControl("ddEditFacility"), DropDownList)
        '    ddTempDepartment = CType(e.Row.FindControl("ddEditDepartment"), DropDownList)

        '    If ddTempUGNFacility IsNot Nothing And ddTempDepartment IsNot Nothing Then
        '        If ddTempUGNFacility IsNot Nothing Then
        '            If ddTempUGNFacility.SelectedIndex >= 0 Then
        '                strUGNFacility = ddTempUGNFacility.SelectedValue
        '            End If
        '        End If

        '        If strUGNFacility = "" Then
        '            strUGNFacility = "UN"
        '        End If

        '        ddTempDepartment.Items.Clear()
        '        ds = commonFunctions.GetDepartmentGLNo(strUGNFacility)
        '        If commonFunctions.CheckDataSet(ds) = True Then
        '            ddTempDepartment.DataSource = ds
        '            ddTempDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
        '            ddTempDepartment.DataValueField = ds.Tables(0).Columns("DepartmentID").ColumnName
        '            ddTempDepartment.DataBind()
        '            ddTempDepartment.Items.Insert(0, "")
        '        Else
        '            ddTempDepartment.Items.Insert(0, "")
        '        End If
        '    End If

        'End If

        If (e.Row.RowType = DataControlRowType.Footer) Then

            ddTempUGNFacility = CType(e.Row.FindControl("ddInsertFacility"), DropDownList)
            ddTempDepartment = CType(e.Row.FindControl("ddInsertDepartment"), DropDownList)

            If ViewState("SelectedFacility") IsNot Nothing Then
                strUGNFacility = ViewState("SelectedFacility")
            Else
                If ddTempUGNFacility IsNot Nothing Then
                    If ddTempUGNFacility.SelectedIndex >= 0 Then
                        strUGNFacility = ddTempUGNFacility.SelectedValue
                    End If
                End If

                If strUGNFacility = "" Then
                    strUGNFacility = "UN"
                End If
            End If

            ddTempUGNFacility.SelectedValue = strUGNFacility

            ddTempDepartment.Items.Clear()
            ds = commonFunctions.GetDepartmentGLNo(strUGNFacility)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddTempDepartment.DataSource = ds
                ddTempDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
                ddTempDepartment.DataValueField = ds.Tables(0).Columns("DepartmentID").ColumnName
                ddTempDepartment.DataBind()
                ddTempDepartment.Items.Insert(0, "")
            Else
                ddTempDepartment.Items.Insert(0, "")
            End If

        End If

    End Sub

    'Protected Sub gvFacilityDept_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvFacilityDept.RowEditing

    '    Dim currentRowInEdit As Integer = e.NewEditIndex

    '    Dim TempRow As GridViewRow = gvFacilityDept.Rows(e.NewEditIndex)

    '    Dim ds As DataSet

    '    Dim ddTempUGNFacility As DropDownList
    '    Dim ddTempDepartment As DropDownList
    '    Dim strUGNFacility As String = ""

    '    If currentRowInEdit >= 0 Then

    '        'ddTeamMemberTemp = CType(gvApproval.Rows(currentRowInEdit).Cells(3).FindControl("ddEditApproverTeamMember"), DropDownList)
    '        'ddTeamMemberTemp = CType(TempRow.Cells(3).FindControl("ddEditApproverTeamMember"), DropDownList)
    '        'lblTeamMemberIDTemp = CType(gvApproval.Rows(currentRowInEdit).Cells(2).FindControl("lblEditTeamMemberID"), Label)
    '        'lblSubscriptionIDTemp = CType(gvApproval.Rows(currentRowInEdit).Cells(0).FindControl("lblEditSubscriptionID"), Label)

    '        ddTempUGNFacility = CType(gvFacilityDept.Rows(currentRowInEdit).FindControl("ddEditFacility"), DropDownList)
    '        ddTempDepartment = CType(gvFacilityDept.Rows(currentRowInEdit).FindControl("ddEditDepartment"), DropDownList)

    '        If ddTempUGNFacility IsNot Nothing And ddTempDepartment IsNot Nothing Then
    '            If ddTempUGNFacility IsNot Nothing Then
    '                If ddTempUGNFacility.SelectedIndex >= 0 Then
    '                    strUGNFacility = ddTempUGNFacility.SelectedValue
    '                End If
    '            End If

    '            If strUGNFacility = "" Then
    '                strUGNFacility = "UN"
    '            End If

    '            ddTempDepartment.Items.Clear()
    '            ds = commonFunctions.GetDepartmentGLNo(strUGNFacility)
    '            If commonFunctions.CheckDataSet(ds) = True Then
    '                ddTempDepartment.DataSource = ds
    '                ddTempDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
    '                ddTempDepartment.DataValueField = ds.Tables(0).Columns("DepartmentID").ColumnName
    '                ddTempDepartment.DataBind()
    '                '    ddTempDepartment.Items.Insert(0, "")
    '                'Else
    '                '    ddTempDepartment.Items.Insert(0, "")
    '            End If
    '        End If
    '    End If

    'End Sub

    Private Property LoadDataEmpty_Labor() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Labor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Labor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Labor") = value
        End Set

    End Property

    Protected Sub odsLabor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsLabor.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As RFD.RFDLabor_MaintDataTable = CType(e.ReturnValue, RFD.RFDLabor_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Labor = True
            Else
                LoadDataEmpty_Labor = False
            End If
        End If

    End Sub

    Protected Sub gvLabor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLabor.RowCommand

        Try

            ClearMessages()

            Dim ddLaborTemp As DropDownList
            Dim txtLaborRateTemp As TextBox
            Dim txtLaborCrewSizeTemp As TextBox
            Dim cbOffline As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("RFDNo") > 0) Then

                ddLaborTemp = CType(gvLabor.FooterRow.FindControl("ddInsertLabor"), DropDownList)
                txtLaborRateTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborRate"), TextBox)
                txtLaborCrewSizeTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborCrewSize"), TextBox)
                cbOffline = CType(gvLabor.FooterRow.FindControl("cbInsertIsOffline"), CheckBox)

                If ddLaborTemp.SelectedIndex > 0 Then
                    odsLabor.InsertParameters("RFDNo").DefaultValue = ViewState("RFDNo")
                    odsLabor.InsertParameters("LaborID").DefaultValue = ddLaborTemp.SelectedValue
                    odsLabor.InsertParameters("Rate").DefaultValue = txtLaborRateTemp.Text
                    odsLabor.InsertParameters("CrewSize").DefaultValue = txtLaborCrewSizeTemp.Text
                    odsLabor.InsertParameters("isOffline").DefaultValue = cbOffline.Checked

                    intRowsAffected = odsLabor.Insert()

                    lblMessage.Text = "Record Saved Successfully.<br />"
                Else
                    lblMessage.Text = "Record NOT Saved. The Labor Name was not selected.<br />"
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvLabor.ShowFooter = False
            Else
                gvLabor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                ddLaborTemp = CType(gvLabor.FooterRow.FindControl("ddInsertLabor"), DropDownList)
                ddLaborTemp.SelectedIndex = -1

                txtLaborRateTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborRate"), TextBox)
                txtLaborRateTemp.Text = ""

                txtLaborCrewSizeTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborCrewSize"), TextBox)
                txtLaborCrewSizeTemp.Text = ""

                cbOffline = CType(gvLabor.FooterRow.FindControl("cbInsertIsOffline"), CheckBox)
                cbOffline.Checked = False

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLaborOverhead.Text = lblMessage.Text

    End Sub

    Protected Sub gvLabor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLabor.RowCreated

        Try

            ''hide columns
            'If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            '    e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Labor
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Private Property LoadDataEmpty_Overhead() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_Overhead") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_Overhead"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_Overhead") = value
        End Set

    End Property

    Protected Sub odsOverhead_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsOverhead.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As RFD.RFDOverhead_MaintDataTable = CType(e.ReturnValue, RFD.RFDOverhead_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_Overhead = True
            Else
                LoadDataEmpty_Overhead = False
            End If
        End If

    End Sub

    Protected Sub gvOverhead_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvOverhead.RowCommand

        Try

            ClearMessages()

            Dim ddOverheadTemp As DropDownList
            Dim txtOverheadFixedRateTemp As TextBox
            Dim txtOverheadVariableRateTemp As TextBox
            Dim txtOverheadCrewSizeTemp As TextBox
            Dim txtOverheadNumberOfCarriersTemp As TextBox
            Dim cbOffline As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("RFDNo") > 0) Then

                ddOverheadTemp = CType(gvOverhead.FooterRow.FindControl("ddInsertOverhead"), DropDownList)
                txtOverheadFixedRateTemp = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadFixedRate"), TextBox)
                txtOverheadVariableRateTemp = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadVariableRate"), TextBox)
                txtOverheadCrewSizeTemp = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadCrewSize"), TextBox)
                txtOverheadNumberOfCarriersTemp = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadNumberOfCarriers"), TextBox)
                cbOffline = CType(gvOverhead.FooterRow.FindControl("cbInsertIsOffline"), CheckBox)

                If ddOverheadTemp.SelectedIndex > 0 Then

                    odsOverhead.InsertParameters("RFDNo").DefaultValue = ViewState("RFDNo")
                    odsOverhead.InsertParameters("LaborID").DefaultValue = ddOverheadTemp.SelectedValue
                    odsOverhead.InsertParameters("FixedRate").DefaultValue = txtOverheadFixedRateTemp.Text
                    odsOverhead.InsertParameters("VariableRate").DefaultValue = txtOverheadVariableRateTemp.Text
                    odsOverhead.InsertParameters("CrewSize").DefaultValue = txtOverheadCrewSizeTemp.Text
                    odsOverhead.InsertParameters("NumberOfCarriers").DefaultValue = txtOverheadNumberOfCarriersTemp.Text
                    odsOverhead.InsertParameters("isOffline").DefaultValue = cbOffline.Checked

                    intRowsAffected = odsOverhead.Insert()

                    lblMessage.Text = "Record Saved Successfully.<br />"
                Else
                    lblMessage.Text = "Record NOT Saved. The Overhead Name was not selected.<br />"
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvOverhead.ShowFooter = False
            Else
                gvOverhead.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                ddOverheadTemp = CType(gvOverhead.FooterRow.FindControl("ddInsertOverhead"), DropDownList)
                ddOverheadTemp.SelectedIndex = -1

                txtOverheadFixedRateTemp = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadFixedRate"), TextBox)
                txtOverheadFixedRateTemp.Text = ""

                txtOverheadVariableRateTemp = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadVariableRate"), TextBox)
                txtOverheadVariableRateTemp.Text = ""

                txtOverheadCrewSizeTemp = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadCrewSize"), TextBox)
                txtOverheadCrewSizeTemp.Text = ""

                txtOverheadNumberOfCarriersTemp = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadNumberOfCarriers"), TextBox)
                txtOverheadNumberOfCarriersTemp.Text = ""

                cbOffline = CType(gvOverhead.FooterRow.FindControl("cbInsertIsOffline"), CheckBox)
                cbOffline.Checked = False

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageLaborOverhead.Text = lblMessage.Text

    End Sub

    Protected Sub gvOverhead_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOverhead.RowCreated

        Try

            ''hide columns
            'If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            '    e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_Overhead
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Private Sub AdjustApprovalStatusControl()

        Try

            If ddApprovalStatus.SelectedValue = 3 Then 'complete
                btnApprovalStatusSubmit.CausesValidation = False
                btnApprovalStatusSubmit.Attributes.Add("onclick", "if(confirm('Are you sure you want to submit the approval?')){}else{return false}")
                rvApprovalStatus.Enabled = False
            Else
                btnApprovalStatusSubmit.Attributes.Add("onclick", "")
                btnApprovalStatusSubmit.CausesValidation = True
                rvApprovalStatus.Enabled = True

                If ddApprovalStatus.SelectedValue = 5 Then 'rejected
                    rfvApprovalComments.Enabled = True
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ddApprovalStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddApprovalStatus.SelectedIndexChanged

        Try
            ClearMessages()

            AdjustApprovalStatusControl()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageApproval.Text = lblMessage.Text

    End Sub

    'Protected Sub ddAppovalSubscription_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddAppovalSubscription.SelectedIndexChanged

    '    Try

    '        Dim iSubscriptionID As Integer = 0

    '        If ddAppovalSubscription.SelectedIndex >= 0 Then
    '            iSubscriptionID = ddAppovalSubscription.SelectedValue
    '        Else
    '            iSubscriptionID = ViewState("SubscriptionID")
    '        End If

    '        BindApprovalTeamMemberBySubscription(iSubscriptionID)

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    lblMessageApproval.Text = lblMessage.Text

    'End Sub

    Protected Sub btnRSSReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSSReset.Click

        Try
            ClearMessages()

            txtRSSComment.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Protected Sub btnRSSSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSSSubmit.Click

        Try
            ClearMessages()

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "RFD/crRFD_Preview.aspx?RFDNo=" & ViewState("RFDNo")
            Dim strEmailDetailURL As String = strProdOrTestEnvironment & "RFD/RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo")

            Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim strEmailBody As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            'save comment
            RFDModule.InsertRFDRSS(ViewState("RFDNo"), ViewState("TeamMemberID"), ViewState("SubscriptionID"), txtRSSComment.Text.Trim)

            'update history
            RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Message Sent")

            gvQuestion.DataBind()

            GetTeamMemberInfo()

            'include RFD Initiator
            If ViewState("InitiatorTeamMemberEmail") <> "" Then
                'make sure backup team member is not already in either recipient list
                If InStr(strEmailCCAddress, ViewState("InitiatorManagerEmail")) <= 0 And InStr(strEmailToAddress, ViewState("InitiatorManagerEmail")) <= 0 Then
                    If strEmailToAddress.Trim <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    strEmailToAddress &= ViewState("InitiatorTeamMemberEmail")
                End If
            End If

            If ViewState("AccountManagerEmail") <> "" And (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7) Then
                If InStr(strEmailToAddress, ViewState("AccountManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("AccountManagerEmail")) <= 0 Then
                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    strEmailToAddress &= ViewState("AccountManagerEmail")
                End If
            End If

            'costing            
            'If ViewState("CostingEmail") <> "" And ViewState("AllApprovedBeforeCosting") = True And cbCostingRequired.Checked = True Then
            If ViewState("CostingEmail") <> "" And cbCostingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("CostingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("CostingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("CostingEmail")
                End If
            End If

            'Packaging
            If ViewState("PackagingEmail") <> "" And cbPackagingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("PackagingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PackagingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PackagingEmail")
                End If
            End If

            'PlantController
            If ViewState("PlantControllerEmail") <> "" And cbPlantControllerRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("PlantControllerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PlantControllerEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PlantControllerEmail")
                End If
            End If

            'process
            If ViewState("ProcessEmail") <> "" And cbProcessRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ProcessEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProcessEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ProcessEmail")
                End If
            End If

            'product development
            If ViewState("ProductDevelopmentEmail") <> "" And cbProductDevelopmentRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ProductDevelopmentEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProductDevelopmentEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ProductDevelopmentEmail")
                End If
            End If

            'Purchasing
            If ViewState("PurchasingExternalRFQEmail") <> "" And cbPurchasingExternalRFQRequired.Checked Then
                If InStr(strEmailToAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PurchasingExternalRFQEmail")
                End If
            End If

            'Purchasing
            If ViewState("PurchasingEmail") <> "" And ViewState("AllApprovedBeforePurchasing") = True And cbPurchasingRequired.Checked Then
                If InStr(strEmailToAddress, ViewState("PurchasingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("PurchasingEmail")
                End If
            End If

            'QualityEngineer
            If ViewState("QualityEngineerEmail") <> "" And ViewState("AllApprovedBeforeQualityEngineer") = True And cbQualityEngineeringRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("QualityEngineerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("QualityEngineerEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("QualityEngineerEmail")
                End If
            End If

            'tooling
            If ViewState("ToolingEmail") <> "" And cbToolingRequired.Checked = True Then
                If InStr(strEmailToAddress, ViewState("ToolingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ToolingEmail")) <= 0 Then
                    If strEmailCCAddress <> "" Then
                        strEmailCCAddress &= ";"
                    End If

                    strEmailCCAddress &= ViewState("ToolingEmail")
                End If
            End If

            '''''''''''''''''''''''''''''''''''
            ' ''Build Email
            '''''''''''''''''''''''''''''''''''

            'assign email subject
            strEmailSubject = "RFD Question - RFD No: " & ViewState("RFDNo") & " - MESSAGE RECEIVED"

            strEmailBody = "<table><tr><td valign='top' width='20%'><img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger.jpg'  height='20%' width='20%'/></td><td valign='top'>"
            'strEmailBody &= "<font size='3' face='Verdana'><b>Attention</b> "
            strEmailBody &= "<font size='3' face='Verdana'><p><b>" & strCurrentUserFullName & "</b> sent you message regarding RFD No.: <font color='red'>" & ViewState("RFDNo") & "</font><br />"

            If txtRSSComment.Text.Trim <> "" Then
                strEmailBody &= "<p><b><font color='red' size='6'>Question: </font></b><font color='green' size='6'>" & txtRSSComment.Text.Trim & "</font></p><hr /><br /><br />"
            End If

            If txtCopyReason.Text.Trim <> "" Then
                strEmailBody &= "<font size='4' face='Verdana' color='red'><b>COPY REASON: " & txtCopyReason.Text.Trim & "</b></font><br />"
            End If

            strEmailBody &= "<font size='3' face='Verdana'><p><b>Description: </b> <font>" & txtRFDDesc.Text.Trim & "</font>.</p><br />"

            strEmailBody &= "<br />PLEASE DO NOT REPLY TO THIS EMAIL. INSTEAD KEEP THE CONVERSATION IN THE SYSTEM BY USING THE LINK BELOW.<br />"

            strEmailBody &= "<p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/RFD/RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo") & "&pRC=1" & "'>Click here</a> to answer the message.</font>"
            strEmailBody &= "</td></tr></table>"

            If strEmailToAddress <> "" And strEmailSubject <> "" And strEmailBody <> "" Then
                If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
                    '    lblMessage.Text &= "Notfication Sent."
                    'Else
                    '    lblMessage.Text &= "Notfication Failed. Please contact IS."
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Protected Sub btnApprovalStatusSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprovalStatusSubmit.Click

        Try
            ClearMessages()

            Dim iCommodityID As Integer = 0

            Dim iApprovalStatusID As Integer = 0
            Dim iApprovalSubscriptionID As Integer = ViewState("SubscriptionID")

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "RFD/crRFD_Preview.aspx?RFDNo=" & ViewState("RFDNo")
            Dim strEmailDetailURL As String = strProdOrTestEnvironment & "RFD/RFD_Detail.aspx?RFDNo=" & ViewState("RFDNo")

            Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim strEmailBody As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            Dim objRFDCustomerProgramBLL As RFDCustomerProgramBLL = New RFDCustomerProgramBLL
            Dim objRFDFinishedGoodBLL As RFDFinishedGoodBLL = New RFDFinishedGoodBLL
            Dim objRFDChildPartBLL As RFDChildPartBLL = New RFDChildPartBLL
            Dim objRFDFinishedGoodPackagingBLL As RFDFinishedGoodPackagingBLL = New RFDFinishedGoodPackagingBLL
            Dim objRFDChildPartPackagingBLL As RFDChildPartPackagingBLL = New RFDChildPartPackagingBLL
            Dim objRFDLaborBLL As RFDLaborBLL = New RFDLaborBLL

            Dim iCavityCount As Integer = 0
            If txtApprovalNumberOfCavities.Text.Trim <> "" Then
                iCavityCount = CType(txtApprovalNumberOfCavities.Text.Trim, Integer)
            End If

            Dim iRowCount As Integer = 0

            Dim dt As DataTable
            Dim ds As DataSet

            Dim bContinue As Boolean = True
            Dim bNotifyDefaultPlantController As Boolean = False

            If ddApprovalStatus.SelectedIndex >= 0 Then
                iApprovalStatusID = ddApprovalStatus.SelectedValue
            End If

            ViewState("ApprovalStatusID") = iApprovalStatusID

            'for tooling and product development
            If iApprovalStatusID = 3 And (ddApprovalSubscription.SelectedValue = 5 Or ddApprovalSubscription.SelectedValue = 65) And txtApprovalNumberOfCavities.Text.Trim = "" Then
                bContinue = False
                lblMessage.Text &= "<br />ERROR: Please enter the number of cavities. Type 0 if none."
            End If

            'If (ViewState("isProductDevelopment") = True Or ViewState("isTooling") = True) And txtApprovalNumberOfCavities.Text.Trim <> "" And InStr(txtApprovalComments.Text.Trim, "umber of Cavities:", CompareMethod.Text) <= 0 Then
            '    txtApprovalComments.Text = "Number of cavities: " & txtApprovalNumberOfCavities.Text & vbCrLf & txtApprovalComments.Text.Trim
            'End If

            If ViewState("isProductDevelopment") = True And txtApprovalNumberOfCavities.Text.Trim <> "" And InStr(txtApprovalComments.Text.Trim, "umber of Cavities:", CompareMethod.Text) <= 0 Then
                txtApprovalComments.Text = "Number of cavities: " & txtApprovalNumberOfCavities.Text & vbCrLf & txtApprovalComments.Text.Trim
            End If

            ''need to make sure Product Development has at least 1 program is selected
            'quote only does not need program code
            If ViewState("BusinessProcessTypeID") <> 7 Then

                If iApprovalStatusID = 3 And cbProductDevelopmentRequired.Checked = True _
                    And (ViewState("isProductDevelopment") = True Or ViewState("isAdmin") = True) Then

                    dt = objRFDCustomerProgramBLL.GetRFDCustomerProgram(ViewState("RFDNo"))

                    If commonFunctions.CheckDataTable(dt) = False Then
                        bContinue = False
                        lblMessage.Text &= "<br />ERROR: Product Engineering cannot approve this RFD until at least one program is selected."
                    End If

                End If
            End If

            If iApprovalStatusID = 3 And cbToolingRequired.Checked = True And ddApprovalSubscription.SelectedValue = 65 Then
                Dim iToolingCavityCount As Integer = 0
                If txtApprovalNumberOfCavities.Text.Trim <> "" Then
                    iToolingCavityCount = CType(txtApprovalNumberOfCavities.Text.Trim, Integer)
                End If

                If ViewState("ProductDevelopmentCavityCount") > 0 Then
                    If iToolingCavityCount <> ViewState("ProductDevelopmentCavityCount") Then
                        bContinue = False
                        txtApprovalComments.Text = ""
                        lblMessage.Text &= "<br />ERROR: The number of cavities does not match what the Product Engineering team member assigned."
                    Else
                        txtApprovalComments.Text = "Number of cavities: " & txtApprovalNumberOfCavities.Text & vbCrLf & txtApprovalComments.Text.Trim
                    End If
                End If
            End If

            ''if Designation Type is Finished Good then
            ''need Product Development to have a New Drawing Number assigned
            '04/18/2012 - for quote only, no DMS drawing number is needed
            If ViewState("BusinessProcessTypeID") <> 7 _
                And iApprovalStatusID = 3 _
                And ddApprovalSubscription.SelectedValue = 5 _
                And ddDesignationType.SelectedValue = "C" _
                And cbProductDevelopmentRequired.Checked = True _
                And (ViewState("isProductDevelopment") = True Or ViewState("isAdmin") = True) Then

                'dt = objRFDFinishedGoodBLL.GetRFDFinishedGood(ViewState("RFDNo"))

                'If commonFunctions.CheckDataTable(dt) = True Then
                '    For iRowCount = 0 To dt.Rows.Count - 1
                '        If dt.Rows(iRowCount).Item("DrawingNo").ToString = "" And txtNewDrawingNo.Text = "" Then
                '            bContinue = False
                '            Exit For
                '        End If
                '    Next
                'Else
                '    bContinue = False
                'End If

                If txtNewDrawingNo.Text = "" Then
                    bContinue = False
                End If

                If bContinue = False Then
                    lblMessage.Text &= "<br />ERROR: Product Engineering cannot approve this RFD until a DMS Drawing number is assigned to the Customer Part Number."
                End If
            End If

            ''if child parts exist and designation type is Raw Material then
            ''need Product Development to have a New Drawing Number assigned
            '04/18/2012 - for quote only, no DMS drawing number is needed
            If ViewState("BusinessProcessTypeID") <> 7 _
                And iApprovalStatusID = 3 _
                And ddApprovalSubscription.SelectedValue = 5 _
                And cbProductDevelopmentRequired.Checked = True _
                And (ViewState("isProductDevelopment") = True Or ViewState("isAdmin") = True) Then

                dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = True Then
                    If dt.Rows.Count > 0 Then
                        For iRowCount = 0 To dt.Rows.Count - 1
                            If dt.Rows(iRowCount).Item("NewDrawingNo").ToString = "" And dt.Rows(iRowCount).Item("NewDesignationType").ToString = "R" Then
                                bContinue = False
                                Exit For
                            End If
                        Next

                        If bContinue = False Then
                            lblMessage.Text &= "<br />ERROR: Product Engineering cannot approve this RFD until a DMS Drawing number is assigned to each Raw Material."
                        End If
                    End If

                End If 'if chidl parts exist
            End If

            ''if Designation Type is Finished Good then
            ''need Costing to have CostSheet assigned
            If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 6 And ddDesignationType.SelectedValue = "C" And cbCostingRequired.Checked = True _
                And (ViewState("isCosting") = True Or ViewState("isAdmin") = True) Then

                'dt = objRFDFinishedGoodBLL.GetRFDFinishedGood(ViewState("RFDNo"))

                'If commonFunctions.CheckDataTable(dt) = True Then
                '    For iRowCount = 0 To dt.Rows.Count - 1
                '        If dt.Rows(iRowCount).Item("CostSheetID").ToString = "" And txtNewCostSheetID.Text = "" Then
                '            bContinue = False
                '            Exit For
                '        End If
                '    Next
                'Else
                '    bContinue = False
                'End If

                If txtNewCostSheetID.Text = "" Then
                    bContinue = False
                End If

                If bContinue = False Then
                    lblMessage.Text &= "<br />ERROR: Costing cannot approve this RFD until a Cost Sheet is assigned to the Customer Part No/Finished Good(s)."
                End If
            End If

            ''if Designation Type is Finished Good then
            ''need Packaging to have CostSheet assigned
            If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 108 _
                And ddDesignationType.SelectedValue = "C" And cbPackagingRequired.Checked = True _
                And (ViewState("isPackaging") = True Or ViewState("isAdmin") = True) Then

                dt = objRFDFinishedGoodPackagingBLL.GetRFDFinishedGoodPackaging(ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = True Then
                    For iRowCount = 0 To dt.Rows.Count - 1
                        If dt.Rows(iRowCount).Item("ContainerCount").ToString = "" _
                            And dt.Rows(iRowCount).Item("ContainerHeight").ToString = "" _
                            And dt.Rows(iRowCount).Item("ContainerWidth").ToString = "" _
                            And dt.Rows(iRowCount).Item("ContainerDepth").ToString = "" _
                            And dt.Rows(iRowCount).Item("PackagingAnnualVolume").ToString = "" _
                            And dt.Rows(iRowCount).Item("SystemDayCount").ToString = "" _
                            And dt.Rows(iRowCount).Item("PackagingComments").ToString = "" Then

                            bContinue = False
                            Exit For
                        End If
                    Next
                End If

                If bContinue = False Then
                    lblMessage.Text &= "<br />ERROR: Packaging cannot approve this RFD until packaging information has been added for the Finished Good."
                End If
            End If

            ''if Designation Type is Finished Good then
            ''need Plant Controller to have labor assigned
            If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 20 _
                And ddDesignationType.SelectedValue = "C" And cbPlantControllerRequired.Checked = True _
                And (ViewState("isPlantController") = True Or ViewState("isAdmin") = True) Then

                dt = objRFDLaborBLL.GetRFDLabor(ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = True Then
                    For iRowCount = 0 To dt.Rows.Count - 1
                        If dt.Rows(iRowCount).Item("LaborID").ToString = "" Then
                            bContinue = False
                            Exit For
                        End If
                    Next
                Else
                    bContinue = False
                End If

                If bContinue = False Then
                    lblMessage.Text &= "<br />ERROR: Finance/Plant Controller cannot approve this RFD until Labor is assigned."
                Else
                    bNotifyDefaultPlantController = True
                End If
            End If

            ''if Designation Type is Finished Good then
            ''need Quality Engineering to have all Finished Good Part Numbers assigned
            '2011-Apr-13 - Bryan Hall, not all changes require and ECI but may still require QE approval
            'If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 22 _
            '    And ddDesignationType.SelectedValue = "C" And cbQualityEngineeringRequired.Checked = True _
            '    And (ViewState("isQualityEngineer") = True Or ViewState("isAdmin") = True) Then

            '    'dt = objRFDFinishedGoodBLL.GetRFDFinishedGood(ViewState("RFDNo"))

            '    'If commonFunctions.CheckDataTable(dt) = True Then
            '    '    For iRowCount = 0 To dt.Rows.Count - 1
            '    '        If dt.Rows(iRowCount).Item("PartNo").ToString = "" And dt.Rows(iRowCount).Item("ECINo").ToString = "" And txtNewECINo.Text = "" Then
            '    '            bContinue = False
            '    '            Exit For
            '    '        End If
            '    '    Next
            '    'Else
            '    '    bContinue = False
            '    'End If

            '    'If bContinue = False Then
            '    '    lblMessage.Text &= "<br />ERROR: Quality Engineer cannot approve this RFD until all of the Finished Good Part Numbers are assigned and at least one ECI number is assigned."
            '    'End If

            '    If txtNewECINo.Text = "" Then
            '        bContinue = False
            '    End If

            '    If bContinue = False Then
            '        lblMessage.Text &= "<br />ERROR: Quality Engineer cannot approve this RFD until the Customer Part is assigned an ECI number."
            '    End If
            'End If

            'if Designation Type is Finished Good then
            '2011-Sep-28 - Oswaldo Amaya - need Quality Engineering to enter ECINo or check NA
            If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 22 _
                And ddDesignationType.SelectedValue = "C" And cbQualityEngineeringRequired.Checked = True _
                And (ViewState("isQualityEngineer") = True Or ViewState("isAdmin") = True) Then

                If txtNewECINo.Text.Trim = "" And cbNewECIOverrideNA.Checked = False Then
                    bContinue = False
                End If

                If bContinue = False Then
                    lblMessage.Text &= "<br />ERROR: Quality Engineer cannot approve this RFD until the Customer Part is assigned an ECI number or N/A is checked."
                End If
            End If

            'if Designation Type is NOT Finished Good and NOT Raw Material then
            'need Quality Engineering to make sure all NON Raw material Child Part Part Number assigned
            'Some Formula/Phantom parts do not get assigned Part numbers : Jim Reinking and Bryan Hall
            If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 22 _
                And cbQualityEngineeringRequired.Checked = True _
                And ViewState("isQualityEngineer") = True Then

                dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = True Then
                    If dt.Rows.Count > 0 Then
                        For iRowCount = 0 To dt.Rows.Count - 1
                            If dt.Rows(iRowCount).Item("NewPartNo").ToString = "" And dt.Rows(iRowCount).Item("NewDesignationType").ToString <> "0" And dt.Rows(iRowCount).Item("NewDesignationType").ToString <> "R" And dt.Rows(iRowCount).Item("NewDesignationType").ToString <> "C" Then
                                bContinue = False
                                Exit For
                            End If
                        Next

                        If bContinue = False Then
                            'lblMessage.Text &= "<br />ERROR: Quality Engineer cannot approve this RFD until all child parts that are NOT raw materials have been assigned a New Part Number or Revision."
                            lblMessage.Text &= "<br />ERROR: Quality Engineer cannot approve this RFD until all child parts that are semi-finished goods have been assigned a New Part Number or Revision."
                        End If
                    End If

                End If ' if child parts exist
            End If

            ''need Quality Engineering to make sure all child parts need an ECI number
            '2011-Apr-13 - Bryan Hall, not all changes require and ECI but may still require QE approval
            'If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 22 _
            '    And cbQualityEngineeringRequired.Checked = True _
            '    And ViewState("isQualityEngineer") = True Then

            '    dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))

            '    If commonFunctions.CheckDataTable(dt) = True Then
            '        If dt.Rows.Count > 0 Then
            '            For iRowCount = 0 To dt.Rows.Count - 1
            '                If dt.Rows(iRowCount).Item("ECINo").ToString = "" Then
            '                    bContinue = False
            '                    Exit For
            '                End If
            '            Next

            '            If bContinue = False Then
            '                lblMessage.Text &= "<br />ERROR: Quality Engineer cannot approve this RFD until all child parts have been assigned an ECI number."
            '            End If
            '        End If

            '    End If 'if child parts exist
            'End If

            ''need Quality Engineering to make sure all child parts need an ECI number
            '2011-Sep-28 - Oswaldo Amaya - need Quality Engineering to enter ECINo or check NA
            If bContinue = True And iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 22 _
                And cbQualityEngineeringRequired.Checked = True _
                And ViewState("isQualityEngineer") = True Then

                dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))
                Dim bChildECIOverrideNA As Boolean = False

                If commonFunctions.CheckDataTable(dt) = True Then
                    If dt.Rows.Count > 0 Then
                        For iRowCount = 0 To dt.Rows.Count - 1
                            bChildECIOverrideNA = False
                            If dt.Rows(iRowCount).Item("isECIRequired") IsNot System.DBNull.Value Then
                                bChildECIOverrideNA = Not dt.Rows(iRowCount).Item("isECIRequired")
                            End If

                            If dt.Rows(iRowCount).Item("ECINo").ToString = "" And bChildECIOverrideNA = False Then
                                bContinue = False
                                Exit For
                            End If
                        Next

                        If bContinue = False Then
                            lblMessage.Text &= "<br />ERROR: Quality Engineer cannot approve this RFD until all child parts have been assigned an ECI number or N/A is checked."
                        End If
                    End If

                End If 'if child parts exist
            End If

            ''if Designation Type is Raw Material then
            ''need to add code here for Costing to have at least 1 Raw Material Part Number assigned
            If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 6 _
                And cbCostingRequired.Checked = True _
                And (ViewState("isCosting") = True Or ViewState("isAdmin") = True) Then

                dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = True Then
                    If dt.Rows.Count > 0 Then
                        For iRowCount = 0 To dt.Rows.Count - 1
                            'If ((dt.Rows(iRowCount).Item("CostSheetID").ToString = "" And dt.Rows(iRowCount).Item("ExternalRFQNo").ToString = "")) And dt.Rows(iRowCount).Item("NewDesignationType").ToString = "R" Then
                            '    '12/08/2010 - Dan Cade and Nina Butler stated during the RFD Training that the Raw Material BPCS number might not be known at this point
                            '    'Or (dt.Rows(iRowCount).Item("NewPartNo").ToString = "")
                            '    bContinue = False
                            '    Exit For
                            'End If

                            ''02/10/2011 - at least one child part has to have a Cost SheetID or External RFQNo
                            'bContinue = False
                            'If ((dt.Rows(iRowCount).Item("CostSheetID").ToString <> "" Or dt.Rows(iRowCount).Item("ExternalRFQNo").ToString <> "")) And dt.Rows(iRowCount).Item("NewDesignationType").ToString = "R" Then
                            '    bContinue = True
                            '    Exit For
                            'End If

                            ''03/14/2011 - at least one child part has to have a Cost SheetID or External RFQNo
                            ''07/17/2012 - Dan Cade - some child parts may not have new Cost Sheet or External RFQ
                            bContinue = False
                            If (dt.Rows(iRowCount).Item("CostSheetID").ToString <> "" Or dt.Rows(iRowCount).Item("ExternalRFQNo").ToString <> "" Or dt.Rows(iRowCount).Item("isExternalRFQrequired") = False) Then
                                bContinue = True
                                Exit For
                            End If
                        Next

                        If bContinue = False Then
                            lblMessage.Text &= "<br />ERROR: Costing cannot approve this RFD until at least one raw material has been assigned either a Cost Sheet ID or an External RFQ No."
                        End If
                    End If

                End If ' if child parts exist
            End If

            ''if Designation Type is Raw Material then
            ''need Purchasing to  assigned PO Numbers 
            If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 7 _
                And ddDesignationType.SelectedValue = "R" And cbPurchasingRequired.Checked = True _
                And (ViewState("isPurchasing") = True Or ViewState("isAdmin") = True) Then

                dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = True Then
                    For iRowCount = 0 To dt.Rows.Count - 1
                        If dt.Rows(iRowCount).Item("PurchasingPONo").ToString = "" And dt.Rows(iRowCount).Item("NewDesignationType").ToString = "R" Then
                            bContinue = False
                            Exit For
                        End If
                    Next
                End If

                If bContinue = False Then
                    lblMessage.Text &= "<br />ERROR: Purchasing cannot approve this RFD until all raw materials have been assigned a a P.O. number."
                End If
            End If

            ''need Packaging to assign packaging child part info
            If iApprovalStatusID = 3 And ddApprovalSubscription.SelectedValue = 108 _
                And cbPackagingRequired.Checked = True _
                And (ViewState("isPackaging") = True Or ViewState("isAdmin")) Then

                dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))

                If commonFunctions.CheckDataTable(dt) = True Then
                    dt = objRFDChildPartPackagingBLL.GetRFDChildPartPackaging(ViewState("RFDNo"))

                    If commonFunctions.CheckDataTable(dt) = True Then
                        If dt.Rows.Count > 0 Then
                            For iRowCount = 0 To dt.Rows.Count - 1
                                If dt.Rows(iRowCount).Item("ContainerCount").ToString = "" _
                                    And dt.Rows(iRowCount).Item("ContainerHeight").ToString = "" _
                                    And dt.Rows(iRowCount).Item("ContainerWidth").ToString = "" _
                                    And dt.Rows(iRowCount).Item("ContainerDepth").ToString = "" _
                                    And dt.Rows(iRowCount).Item("PackagingAnnualVolume").ToString = "" _
                                    And dt.Rows(iRowCount).Item("SystemDayCount").ToString = "" _
                                    And dt.Rows(iRowCount).Item("PackagingComments").ToString = "" _
                                    And dt.Rows(iRowCount).Item("NewDesignationType").ToString = "R" Then

                                    bContinue = False
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                    If bContinue = False Then
                        lblMessage.Text &= "<br />ERROR: Packaging cannot approve this RFD until all raw materials have been assigned packaging information."
                    End If
                End If 'if child parts exist
            End If

            If bContinue = True Then

                'reset if someone other than costing tries to use the status of waiting for cost sheet approval
                If ddApprovalSubscription.SelectedValue <> 6 And iApprovalStatusID = 9 Then
                    iApprovalStatusID = 2
                End If

                If ddApprovalSubscription.SelectedIndex >= 0 Then
                    iApprovalSubscriptionID = ddApprovalSubscription.SelectedValue
                End If

                If ViewState("TeamMemberID") <> ViewState("OriginalApproverID") And ViewState("OriginalApproverID") > 0 Then
                    txtApprovalComments.Text = "Acting as backup team member." & Chr(13) & Chr(10) & txtApprovalComments.Text
                End If

                ''still thinking about this
                ''if the user has multiple roles, approve them all
                'If ViewState("isCosting") = True Then
                '    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, ViewState("TeamMemberID"), txtApprovalComment.Text.Trim, iApprovalStatusID, "")
                'End If

                'If ViewState("isProcess") = True Then
                '    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 66, ViewState("TeamMemberID"), txtApprovalComment.Text.Trim, iApprovalStatusID, "")
                'End If

                'If ViewState("isProductDevelopment") = True Then
                '    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 5, ViewState("TeamMemberID"), txtApprovalComment.Text.Trim, iApprovalStatusID, "")
                'End If

                'If ViewState("isPurchasing") = True Then
                '    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 7, ViewState("TeamMemberID"), txtApprovalComment.Text.Trim, iApprovalStatusID, "")
                'End If

                'If ViewState("isQualityEngineer") = True Then
                '    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 22, ViewState("TeamMemberID"), txtApprovalComment.Text.Trim, iApprovalStatusID, "")
                'End If

                'If ViewState("isTooling") = True Then
                '    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 65, ViewState("TeamMemberID"), txtApprovalComment.Text.Trim, iApprovalStatusID, "")
                'End If

                RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), iApprovalSubscriptionID, ViewState("TeamMemberID"), txtApprovalComments.Text.Trim, iCavityCount, iApprovalStatusID, "")

                GetTeamMemberInfo()

                'if one team member rejects RFD, then the over status is rejected
                If iApprovalStatusID = 5 Then
                    RFDModule.UpdateRFDOverallStatus(ViewState("RFDNo"), 5)

                    ddStatus.SelectedValue = 5
                    ViewState("StatusID") = 5

                    'assign email subject
                    strEmailSubject = "RFDNo: " & ViewState("RFDNo") & " has been rejected "

                    'build email body
                    strEmailBody = "<font size='2' face='Verdana'>The following Request for Development has been rejected by " & strCurrentUserFullName & ":</font><br /><br />"
                    strEmailBody &= "<font size='2' face='Verdana'>RFDNo: <b>" & ViewState("RFDNo") & "</b></font><br />"

                    If txtApprovalComments.Text.Trim <> "" Then
                        strEmailBody &= "<font size='2' face='Verdana'>Comment : " & txtApprovalComments.Text.Trim & "</font><br /><br />"
                    End If

                    strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailDetailURL & "'>Team Members can click here to fix the problem.</a></font><br /><br />"

                    If txtCopyReason.Text.Trim <> "" Then
                        strEmailBody &= "<font size='4' face='Verdana' color='red'><b>COPY REASON : " & txtCopyReason.Text.Trim & "</b></font><br />"
                    End If

                    strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtRFDDesc.Text.Trim & "</font><br />"

                    'update history
                    RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Rejected :" & txtApprovalComments.Text.Trim)

                    'notify team members who approve before Costing, QE and/or Purchasing
                    'make sure team member is not already in either recipient list

                    'include RFD Initiator
                    If ViewState("InitiatorTeamMemberEmail") <> "" Then
                        'make sure backup team member is not already in either recipient list
                        If InStr(strEmailCCAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 And InStr(strEmailToAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 Then
                            If strEmailToAddress.Trim <> "" Then
                                strEmailToAddress &= ";"
                            End If

                            strEmailToAddress &= ViewState("InitiatorTeamMemberEmail")
                        End If
                    End If

                    If ViewState("AccountManagerEmail") <> "" And (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7) Then
                        If InStr(strEmailToAddress, ViewState("AccountManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("AccountManagerEmail")) <= 0 Then
                            If strEmailToAddress <> "" Then
                                strEmailToAddress &= ";"
                            End If

                            strEmailToAddress &= ViewState("AccountManagerEmail")
                        End If
                    End If
                Else
                    If iApprovalStatusID = 6 Then
                        'update history
                        RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Role On Hold: " & txtApprovalComments.Text.Trim)
                    End If

                    If iApprovalStatusID = 7 Then
                        'update history
                        RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Role Tasked to another team member: " & txtApprovalComments.Text.Trim)
                    End If

                    If iApprovalStatusID = 9 Then
                        'update history
                        RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Role Waiting for Cost Sheet Approval: " & txtApprovalComments.Text.Trim)
                    End If

                    If iApprovalStatusID = 3 Then
                        'if not submitted then submit
                        If ViewState("StatusID") = 1 Then
                            RFDModule.UpdateRFDOverallStatus(ViewState("RFDNo"), 2)
                            ddStatus.SelectedValue = 2
                            ViewState("StatusID") = 2
                        End If

                        'update history

                        If txtApprovalComments.Text.Trim <> "" Then
                            RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Approved: " & txtApprovalComments.Text.Trim)
                        Else
                            RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Approved")
                        End If

                        'no need to notify team members of individual approvals, just when a group is done
                        'If (ViewState("BusinessProcessTypeID") = 7 And ViewState("BusinessProcessActionID") = 10 And ViewState("bBusinessAwarded") = True) Or ViewState("BusinessProcessTypeID") <> 7 Then
                        'If ViewState("BusinessProcessTypeID") <> 7 Then

                        If ViewState("AllApproved") = True And ViewState("bBusinessAwarded") = True Then

                            RFDModule.UpdateRFDOverallStatus(ViewState("RFDNo"), 3)

                            ddStatus.SelectedValue = 3
                            ViewState("StatusID") = 3

                            'update history
                            RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "All required team members have completed the RFD.")

                            'assign email subject
                            strEmailSubject = "RFDNo: " & ViewState("RFDNo") & " has been approved by all"

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following Request for Development has been approved by all:</font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>RFDNo: <b>" & ViewState("RFDNo") & "</b></font><br /><br />"

                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailPreviewURL & "'>Team Members can click here to preview the information</a></font><br /><br />"

                            If txtCopyReason.Text.Trim <> "" Then
                                strEmailBody &= "<font size='4' face='Verdana' color='red'><b>COPY REASON : " & txtCopyReason.Text.Trim & "</b></font><br />"
                            End If

                            strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtRFDDesc.Text.Trim & "</font><br />"

                            If ViewState("InitiatorTeamMemberEmail") <> "" Then
                                'make sure backup team member is not already in either recipient list
                                If InStr(strEmailCCAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 And InStr(strEmailToAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 Then
                                    If strEmailToAddress.Trim <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= ViewState("InitiatorTeamMemberEmail")
                                End If
                            End If

                            If ViewState("AccountManagerEmail") <> "" And (ViewState("BusinessProcessTypeID") = 1 Or ViewState("BusinessProcessTypeID") = 7) Then
                                If InStr(strEmailToAddress, ViewState("AccountManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("AccountManagerEmail")) <= 0 Then
                                    If strEmailToAddress <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= ViewState("AccountManagerEmail")
                                End If
                            End If

                            If ViewState("ProgramManagerEmail") <> "" And (ViewState("BusinessProcessTypeID") = 1 Or (ViewState("BusinessProcessTypeID") = 7 And ViewState("BusinessProcessActionID") = 10)) Then
                                If InStr(strEmailToAddress, ViewState("ProgramManagerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProgramManagerEmail")) <= 0 Then
                                    If strEmailToAddress <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= ViewState("ProgramManagerEmail")
                                End If
                            End If

                            'costing
                            If ViewState("CostingEmail") <> "" And cbCostingRequired.Checked = True Then
                                If InStr(strEmailToAddress, ViewState("CostingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("CostingEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("CostingEmail")
                                End If
                            End If

                            'Packaging
                            If ViewState("PackagingEmail") <> "" And cbPackagingRequired.Checked = True Then
                                If InStr(strEmailToAddress, ViewState("PackagingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PackagingEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("PackagingEmail")
                                End If
                            End If

                            'PlantController
                            If ViewState("PlantControllerEmail") <> "" And cbPlantControllerRequired.Checked = True Then
                                If InStr(strEmailToAddress, ViewState("PlantControllerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PlantControllerEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("PlantControllerEmail")
                                End If
                            End If

                            'process
                            If ViewState("ProcessEmail") <> "" And cbProcessRequired.Checked = True Then
                                If InStr(strEmailToAddress, ViewState("ProcessEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProcessEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("ProcessEmail")
                                End If
                            End If

                            'product development
                            If ViewState("ProductDevelopmentEmail") <> "" And cbProductDevelopmentRequired.Checked = True Then
                                If InStr(strEmailToAddress, ViewState("ProductDevelopmentEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ProductDevelopmentEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("ProductDevelopmentEmail")
                                End If
                            End If

                            'Purchasing for External RFQ
                            If ViewState("PurchasingExternalRFQEmail") <> "" And cbPurchasingExternalRFQRequired.Checked Then
                                If InStr(strEmailToAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("PurchasingExternalRFQEmail")
                                End If
                            End If


                            'Purchasing for Contract PO
                            If ViewState("PurchasingEmail") <> "" And cbPurchasingRequired.Checked Then
                                If InStr(strEmailToAddress, ViewState("PurchasingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("PurchasingEmail")
                                End If
                            End If

                            'QualityEngineer
                            If ViewState("QualityEngineerEmail") <> "" And cbQualityEngineeringRequired.Checked = True Then
                                If InStr(strEmailToAddress, ViewState("QualityEngineerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("QualityEngineerEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("QualityEngineerEmail")
                                End If
                            End If

                            'tooling
                            If ViewState("ToolingEmail") <> "" And cbToolingRequired.Checked = True Then
                                If InStr(strEmailToAddress, ViewState("ToolingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("ToolingEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("ToolingEmail")
                                End If
                            End If

                            If ViewState("RFDccEmailList") <> "" Then
                                If strEmailCCAddress <> "" Then
                                    strEmailCCAddress &= ";"
                                End If

                                strEmailCCAddress &= ViewState("RFDccEmailList")
                            End If

                            'cc Product Design if Damper Commodity                           
                            If ddWorkFlowCommodity.SelectedIndex > 0 Then
                                iCommodityID = ddWorkFlowCommodity.SelectedValue
                            ElseIf ddNewCommodity.SelectedValue > 0 Then
                                iCommodityID = ddNewCommodity.SelectedValue
                            End If

                            If iCommodityID > 0 Then
                                If isDamper(iCommodityID) = True And ViewState("ProductDesignEmailList") <> "" Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("ProductDesignEmailList")
                                End If
                            End If

                        Else 'current user approves but NOT ALL have approved

                            'assign email subject
                            strEmailSubject = "RFDNo: " & ViewState("RFDNo") & " has been approved by all previous levels" '& strCurrentUserFullName

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following Request for Development has been approved by all previous levels. Your action is required. </font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>RFDNo: <b>" & ViewState("RFDNo") & "</b></font><br /><br />"

                            'costing
                            If ViewState("CostingEmail") <> "" And cbCostingRequired.Checked = True Then
                                If InStr(strEmailToAddress, ViewState("CostingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("CostingEmail")) <= 0 Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("CostingEmail")
                                End If
                            End If


                            '2012-May-02 : if product development approved but while other levels before costing are pending
                            If ViewState("ProductDevelopmentStatusID") = 3 _
                                And ViewState("isProductDevelopment") = True _
                                And ddApprovalSubscription.SelectedValue = 5 _
                                And ViewState("AllApprovedBeforeCosting") = False _
                                And cbCostingRequired.Checked = True Then

                                '2013-09-16: EReymond - include RFD Initiator for Internal UGN Changes
                                If ViewState("InitiatorTeamMemberEmail") <> "" And ViewState("BusinessProcessTypeID") = 2 Then
                                    'make sure backup team member is not already in either recipient list
                                    If InStr(strEmailCCAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 And InStr(strEmailToAddress, ViewState("InitiatorTeamMemberEmail")) <= 0 Then
                                        If strEmailToAddress.Trim <> "" Then
                                            strEmailToAddress &= ";"
                                        End If

                                        strEmailToAddress &= ViewState("InitiatorTeamMemberEmail")
                                    End If
                                End If

                                'get capital if checked
                                If cbCapitalRequired.Checked = True Then

                                    'make sure team member is not already in either recipient list
                                    If ViewState("CapitalEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("CapitalEmail")) <= 0 And InStr(strEmailToAddress, ViewState("CapitalEmail")) <= 0 Then
                                            If strEmailToAddress <> "" Then
                                                strEmailToAddress &= ";"
                                            End If

                                            strEmailToAddress &= ViewState("CapitalEmail")
                                        End If
                                    End If

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("CapitalBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("CapitalBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("CapitalBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("CapitalBackupEmail")
                                        End If
                                    End If

                                    'set in-process
                                    If ViewState("CapitalTeamMemberID") > 0 And ViewState("CapitalStatusID") < 2 Then
                                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 119, ViewState("CapitalTeamMemberID"), "", 0, 2, Today.Date)
                                    End If

                                End If


                                'get packaging if checked
                                If cbPackagingRequired.Checked = True Then

                                    'make sure team member is not already in either recipient list
                                    If ViewState("PackagingEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("PackagingEmail")) <= 0 And InStr(strEmailToAddress, ViewState("PackagingEmail")) <= 0 Then
                                            If strEmailToAddress <> "" Then
                                                strEmailToAddress &= ";"
                                            End If

                                            strEmailToAddress &= ViewState("PackagingEmail")
                                        End If
                                    End If

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("PackagingBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("PackagingBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("PackagingBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("PackagingBackupEmail")
                                        End If
                                    End If

                                    'set in-process
                                    If ViewState("PackagingTeamMemberID") > 0 And ViewState("PackagingStatusID") < 2 Then
                                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 108, ViewState("PackagingTeamMemberID"), "", 0, 2, Today.Date)
                                    End If

                                End If

                                'get PlantController if checked
                                If cbPlantControllerRequired.Checked = True Then

                                    'make sure team member is not already in either recipient list
                                    If ViewState("PlantControllerEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("PlantControllerEmail")) <= 0 And InStr(strEmailToAddress, ViewState("PlantControllerEmail")) <= 0 Then
                                            If strEmailToAddress <> "" Then
                                                strEmailToAddress &= ";"
                                            End If

                                            strEmailToAddress &= ViewState("PlantControllerEmail")
                                        End If
                                    End If

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("PlantControllerBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("PlantControllerBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("PlantControllerBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("PlantControllerBackupEmail")
                                        End If
                                    End If

                                    'set in-process
                                    If ViewState("PlantControllerTeamMemberID") > 0 And ViewState("PlantControllerStatusID") < 2 Then
                                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 20, ViewState("PlantControllerTeamMemberID"), "", 0, 2, Today.Date)
                                    End If

                                End If

                                'get process if checked
                                If cbProcessRequired.Checked = True Then

                                    'make sure team member is not already in either recipient list
                                    If ViewState("ProcessEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("ProcessEmail")) <= 0 And InStr(strEmailToAddress, ViewState("ProcessEmail")) <= 0 Then
                                            If strEmailToAddress <> "" Then
                                                strEmailToAddress &= ";"
                                            End If

                                            strEmailToAddress &= ViewState("ProcessEmail")
                                        End If
                                    End If

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("ProcessBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("ProcessBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("ProcessBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("ProcessBackupEmail")
                                        End If
                                    End If

                                    'set in-process
                                    If ViewState("ProcessTeamMemberID") > 0 And ViewState("ProcessStatusID") < 2 Then
                                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 66, ViewState("ProcessTeamMemberID"), "", 0, 2, Today.Date)
                                    End If

                                End If

                                'get PurchasingExternalRFQ if checked
                                If cbPurchasingExternalRFQRequired.Checked = True Then

                                    'make sure team member is not already in either recipient list
                                    If ViewState("PurchasingExternalRFQEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 And InStr(strEmailToAddress, ViewState("PurchasingExternalRFQEmail")) <= 0 Then
                                            If strEmailToAddress <> "" Then
                                                strEmailToAddress &= ";"
                                            End If

                                            strEmailToAddress &= ViewState("PurchasingExternalRFQEmail")
                                        End If
                                    End If

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("PurchasingExternalRFQBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("PurchasingExternalRFQBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("PurchasingExternalRFQBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("PurchasingExternalRFQBackupEmail")
                                        End If
                                    End If

                                    'set in-process
                                    If ViewState("PurchasingExternalRFQTeamMemberID") > 0 And ViewState("PurchasingExternalRFQStatusID") < 2 Then
                                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 139, ViewState("PurchasingExternalRFQTeamMemberID"), "", 0, 2, Today.Date)
                                    End If

                                End If

                                'get tooling if checked
                                If cbToolingRequired.Checked = True Then

                                    'make sure team member is not already in either recipient list
                                    If ViewState("ToolingEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("ToolingEmail")) <= 0 And InStr(strEmailToAddress, ViewState("ToolingEmail")) <= 0 Then
                                            If strEmailToAddress <> "" Then
                                                strEmailToAddress &= ";"
                                            End If

                                            strEmailToAddress &= ViewState("ToolingEmail")
                                        End If
                                    End If

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("ToolingBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("ToolingBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("ToolingBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("ToolingBackupEmail")
                                        End If
                                    End If

                                    'set in-process
                                    If ViewState("ToolingTeamMemberID") > 0 And ViewState("ToolingStatusID") < 2 Then
                                        RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 65, ViewState("ToolingTeamMemberID"), "", 0, 2, Today.Date)
                                    End If

                                End If

                            End If

                            'costing
                            If ViewState("CostingEmail") <> "" _
                                And ViewState("AllApprovedBeforeCosting") = True _
                                And cbCostingRequired.Checked = True _
                                And ViewState("AllApprovedBeforeQualityEngineer") = False _
                                And ViewState("AllApprovedBeforePurchasing") = False Then

                                If InStr(strEmailToAddress, ViewState("CostingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("CostingEmail")) <= 0 Then
                                    If strEmailToAddress <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= ViewState("CostingEmail")

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("CostingBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("CostingBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("CostingBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("CostingBackupEmail")
                                        End If

                                        strEmailBody &= "<font size='2' face='Verdana'>The Costing backup has been included in this message.</font><br />"
                                    End If

                                End If

                                'set in-process
                                If ViewState("CostingTeamMemberID") > 0 And ViewState("CostingStatusID") = 1 Then
                                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, ViewState("CostingTeamMemberID"), "", 0, 2, Today.Date)
                                End If
                            End If

                            'QualityEngineer
                            If ViewState("QualityEngineerEmail") <> "" _
                                And ViewState("AllApprovedBeforeQualityEngineer") = True _
                                And ViewState("AllApprovedBeforePurchasing") = False _
                                And cbQualityEngineeringRequired.Checked = True Then

                                If InStr(strEmailToAddress, ViewState("QualityEngineerEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("QualityEngineerEmail")) <= 0 Then
                                    If strEmailToAddress <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= ViewState("QualityEngineerEmail")

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("QualityEngineerBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("QualityEngineerBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("QualityEngineerBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("QualityEngineerBackupEmail")
                                        End If

                                        strEmailBody &= "<font size='2' face='Verdana'>The Quality Engineer backup has been included in this message.</font><br />"
                                    End If
                                End If

                                'set in-process
                                If ViewState("QualityEngineerTeamMemberID") > 0 And ViewState("QualityEngineerStatusID") = 1 Then
                                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 22, ViewState("QualityEngineerTeamMemberID"), "", 0, 2, Today.Date)
                                End If
                            End If

                            'Purchasing
                            If ViewState("PurchasingEmail") <> "" _
                                And ViewState("AllApprovedBeforePurchasing") = True _
                                And cbPurchasingRequired.Checked Then

                                If InStr(strEmailToAddress, ViewState("PurchasingEmail")) <= 0 And InStr(strEmailCCAddress, ViewState("PurchasingEmail")) <= 0 Then
                                    If strEmailToAddress <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= ViewState("PurchasingEmail")

                                    'make sure backup team member is not already in either recipient list
                                    If ViewState("PurchasingBackupEmail") <> "" Then
                                        If InStr(strEmailCCAddress, ViewState("PurchasingBackupEmail")) <= 0 And InStr(strEmailToAddress, ViewState("PurchasingBackupEmail")) <= 0 Then
                                            If strEmailCCAddress <> "" Then
                                                strEmailCCAddress &= ";"
                                            End If

                                            strEmailCCAddress &= ViewState("PurchasingBackupEmail")
                                        End If

                                        strEmailBody &= "<font size='2' face='Verdana'>The Purchasing backup has been included in this message.</font><br />"
                                    End If

                                End If

                                'set in-process
                                If ViewState("PurchasingTeamMemberID") > 0 And ViewState("PurchasingStatusID") = 1 Then
                                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 7, ViewState("PurchasingTeamMemberID"), "", 0, 2, Today.Date)
                                End If
                            End If

                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailDetailURL & "'>Team Members can click here to update the information</a></font><br /><br />"

                            If txtCopyReason.Text.Trim <> "" Then
                                strEmailBody &= "<font size='4' face='Verdana' color='red'><b>COPY REASON : " & txtCopyReason.Text.Trim & "</b></font><br />"
                            End If

                            strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtRFDDesc.Text.Trim & "</font><br />"
                        End If
                    End If
                End If

                'wait until all this logic above is determined before sending emails
                If strEmailToAddress <> "" And strEmailSubject <> "" And strEmailBody <> "" Then

                    'append original approver
                    If ViewState("TeamMemberID") <> ViewState("OriginalApproverID") And ViewState("OriginalApproverID") > 0 Then
                        ds = SecurityModule.GetTeamMember(ViewState("OriginalApproverID"), "", "", "", "", "", True, Nothing)
                        If commonFunctions.CheckDataSet(ds) = True Then
                            If InStr(strEmailToAddress, ds.Tables(0).Rows(0).Item("Email").ToString) <= 0 And _
                            InStr(strEmailCCAddress, ds.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then
                                If strEmailCCAddress <> "" Then
                                    strEmailCCAddress &= ";"
                                End If

                                strEmailCCAddress &= ds.Tables(0).Rows(0).Item("Email").ToString
                            End If
                        End If
                    End If

                    If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
                        '    lblMessage.Text &= "Notfication Sent."
                        'Else
                        '    lblMessage.Text &= "Notfication Failed. Please contact IS."
                    End If
                End If

                EnableControls()

                If ddApprovalSubscription.SelectedIndex >= 0 Then
                    EnableApprovalControls(ddApprovalSubscription.SelectedValue)
                End If

                gvApproval.DataBind()

                'notify default plant controller if plant controllers have approved
                If bNotifyDefaultPlantController = True And ViewState("DefaultPlantControllerEmail") <> "" And _
                ViewState("DefaultPlantControllerTeamMemberID") <> ViewState("TeamMemberID") Then

                    strEmailSubject = "RFDNo: " & ViewState("RFDNo") & " has been approved by the Plant Controller"

                    'build email body
                    strEmailBody = "<font size='2' face='Verdana'>The following Request for Development has been approved by the Plant Controller. Labor information has been added.</font><br /><br />"
                    strEmailBody &= "<font size='2' face='Verdana'>RFDNo: <b>" & ViewState("RFDNo") & "</b></font><br /><br />"

                    If txtApprovalComments.Text.Trim <> "" Then
                        strEmailBody &= "<font size='2' face='Verdana'>Comment: " & txtApprovalComments.Text.Trim & "</font><br /><br />"
                    End If

                    strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailDetailURL & "'>Team Members can click here to update the RFD.</a></font><br /><br />"

                    If txtCopyReason.Text.Trim <> "" Then
                        strEmailBody &= "<font size='4' face='Verdana' color='red'><b>COPY REASON : " & txtCopyReason.Text.Trim & "</b></font><br />"
                    End If

                    strEmailBody &= "<font size='2' face='Verdana'>Description : " & txtRFDDesc.Text.Trim & "</font><br />"

                    strEmailToAddress = ViewState("DefaultPlantControllerEmail")
                    strEmailCCAddress = ""

                    If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
                    End If

                End If
            Else
                ddApprovalStatus.SelectedValue = 2
            End If 'bContinue

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageApproval.Text = lblMessage.Text
        lblMessageApprovalBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddApprovalSubscription_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddApprovalSubscription.SelectedIndexChanged

        Try

            Dim iTempSubscriptionID As Integer = 0

            If ddApprovalSubscription.SelectedIndex >= 0 Then
                iTempSubscriptionID = ddApprovalSubscription.SelectedValue
            End If

            EnableApprovalControls(iTempSubscriptionID)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageApproval.Text = lblMessage.Text

    End Sub

    Protected Sub btnSaveNetworkFileReference_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveNetworkFileReference.Click

        Try

            ClearMessages()

            If QuoteOnlySupDocUpdate() = False Then
                Exit Sub
            End If

            Dim strFilePath As String = fileTextNetworkFileReference.Value

            'Dim pat As String = "(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.jpeg|.jpg|.tif|.PDF|.XLS|.DOC|.JPEG|.JPG|.TIF)$"
            'Dim pat As String = " ^(([a-zA-Z]:|\\)\\)?(((\.)|(\.\.)|([^\\/:\*\?""\|<>\. ](([^\\/:\*\?""\|<>\. ])|([^\\/:\*\?""\|<>]*[^\\/:\*\?""\|<>\. ]))?))\\)*[^\\/:\*\?""\|<>\. ](([^\\/:\*\?""\|<>\. ])|([^\\/:\*\?""\|<>]*[^\\/:\*\?""\|<>\. ]))?$"
            Dim pat As String = "^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$"
            Dim r As Regex = New Regex(pat)
            Dim m As Match = r.Match(strFilePath)

            If m.Success = True And strFilePath.Length > 0 And strFilePath.Length < 200 Then

                RFDModule.InsertRFDNetworkFileReference(ViewState("RFDNo"), strFilePath)

                fileTextNetworkFileReference.Value = ""

                lblMessage.Text &= "File Reference Saved Successfully<br />"

                gvNetworkFiles.DataBind()

            Else
                lblMessage.Text &= "<br />ERROR: Please make sure you have selected a file.<br />"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageSupportingDocs.Text = lblMessage.Text
        lblMessageSupportingDocsBottom.Text = lblMessage.Text

    End Sub

    'Protected Sub cbFilterCustomerByAccountManager_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbFilterCustomerByAccountManager.CheckedChanged

    '    Try

    '        trCustomerByAccountManager.Visible = Not cbFilterCustomerByAccountManager.Checked
    '        trCustomerAll.Visible = cbFilterCustomerByAccountManager.Checked

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= "<br />" & ex.Message & mb.Name & "<br />"

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub

    'Protected Sub ddAccountManager_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddAccountManager.SelectedIndexChanged

    '    Try

    '        FilterCustomerListByAccountManager()

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= "<br />" & ex.Message & mb.Name & "<br />"

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub

    Protected Sub gvApproval_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvApproval.RowUpdated

        Try

            If ddApprovalSubscription.Visible = True Then
                If ddApprovalSubscription.SelectedIndex >= 0 Then
                    EnableApprovalControls(ddApprovalSubscription.SelectedValue)
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub ddFooterLabor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data based on the Footer Packaging drop down      
        ''*******

        ClearMessages()

        Try
            Dim ddTempLabor As DropDownList
            Dim txtTempLaborRate As TextBox
            Dim txtTempLaborCrewSize As TextBox
            Dim cbTempLaborIsOffline As CheckBox

            Dim dsLabor As DataSet

            ddTempLabor = CType(sender, DropDownList)

            txtTempLaborRate = CType(gvLabor.FooterRow.FindControl("txtInsertLaborRate"), TextBox)
            txtTempLaborCrewSize = CType(gvLabor.FooterRow.FindControl("txtInsertLaborCrewSize"), TextBox)
            cbTempLaborIsOffline = CType(gvLabor.FooterRow.FindControl("cbInsertIsOffline"), CheckBox)

            dsLabor = CostingModule.GetLabor(ddTempLabor.SelectedValue, "", False, False)
            If commonFunctions.CheckDataset(dsLabor) = True Then

                If dsLabor.Tables(0).Rows(0).Item("Rate") IsNot System.DBNull.Value Then
                    If dsLabor.Tables(0).Rows(0).Item("Rate") > 0 Then
                        txtTempLaborRate.Text = dsLabor.Tables(0).Rows(0).Item("Rate")
                    End If
                End If

                If dsLabor.Tables(0).Rows(0).Item("CrewSize") IsNot System.DBNull.Value Then
                    If dsLabor.Tables(0).Rows(0).Item("CrewSize") > 0 Then
                        txtTempLaborCrewSize.Text = dsLabor.Tables(0).Rows(0).Item("CrewSize")
                    End If
                End If

                If dsLabor.Tables(0).Rows(0).Item("isOffline") IsNot System.DBNull.Value Then
                    cbTempLaborIsOffline.Checked = dsLabor.Tables(0).Rows(0).Item("isOffline")
                End If
            End If

            lblMessage.Text = "Please make sure to click the SAVE button on the right side of the list."
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLaborOverhead.Text = lblMessage.Text

    End Sub
    Protected Sub ddFooterOverhead_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''*******
        '' This event is used to bind data based on the Footer Packaging drop down      
        ''*******

        ClearMessages()

        Try
            Dim ddTempOverhead As DropDownList
            Dim txtTempOverheadRate As TextBox
            Dim txtTempOverheadVariableRate As TextBox
            Dim txtTempOverheadCrewSize As TextBox
            Dim cbTempOverheadIsOffline As CheckBox

            Dim dsOverhead As DataSet

            ddTempOverhead = CType(sender, DropDownList)

            txtTempOverheadRate = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadFixedRate"), TextBox)
            txtTempOverheadVariableRate = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadVariableRate"), TextBox)
            txtTempOverheadCrewSize = CType(gvOverhead.FooterRow.FindControl("txtInsertOverheadCrewSize"), TextBox)
            cbTempOverheadIsOffline = CType(gvOverhead.FooterRow.FindControl("cbInsertIsOffline"), CheckBox)

            dsOverhead = CostingModule.GetOverhead(ddTempOverhead.SelectedValue, "")
            If commonFunctions.CheckDataset(dsOverhead) = True Then

                If dsOverhead.Tables(0).Rows(0).Item("Rate") IsNot System.DBNull.Value Then
                    If dsOverhead.Tables(0).Rows(0).Item("Rate") > 0 Then
                        txtTempOverheadRate.Text = dsOverhead.Tables(0).Rows(0).Item("Rate")
                    End If
                End If

                If dsOverhead.Tables(0).Rows(0).Item("VariableRate") IsNot System.DBNull.Value Then
                    If dsOverhead.Tables(0).Rows(0).Item("VariableRate") > 0 Then
                        txtTempOverheadVariableRate.Text = dsOverhead.Tables(0).Rows(0).Item("VariableRate")
                    End If
                End If

                If dsOverhead.Tables(0).Rows(0).Item("CrewSize") IsNot System.DBNull.Value Then
                    If dsOverhead.Tables(0).Rows(0).Item("CrewSize") > 0 Then
                        txtTempOverheadCrewSize.Text = dsOverhead.Tables(0).Rows(0).Item("CrewSize")
                    End If
                End If

                If dsOverhead.Tables(0).Rows(0).Item("isOffline") IsNot System.DBNull.Value Then
                    cbTempOverheadIsOffline.Checked = dsOverhead.Tables(0).Rows(0).Item("isOffline")
                End If
            End If

            lblMessage.Text = "Please make sure to click the SAVE button on the right side of the list."
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLaborOverhead.Text = lblMessage.Text

    End Sub

    Protected Sub btnApprovalStatusReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprovalStatusReset.Click

        Try
            ClearMessages()

            txtApprovalComments.Text = ""
            txtApprovalNumberOfCavities.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageApproval.Text = lblMessage.Text
        lblMessageApprovalBottom.Text = lblMessage.Text

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

    'Protected Sub ddBusinessProcessAction_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddBusinessProcessAction.SelectedIndexChanged

    '    Try
    '        'if  New then Business Awarded button is needed. Otherwise, it is automatically awarded
    '        ClearMessages()

    '        Dim iBusinessProcessAction As Integer = 0

    '        'if RFQ type of Business Process
    '        If ViewState("BusinessProcessTypeID") = 1 Then
    '            If ddBusinessProcessAction.SelectedIndex >= 0 Then
    '                iBusinessProcessAction = ddBusinessProcessAction.SelectedValue
    '            End If

    '            'if changed from existing back to new, then business awarded gets wiped out.
    '            If iBusinessProcessAction = 10 And ViewState("BusinessProcessActionID") > 10 Then
    '                RFDModule.DeleteRFDBusinessAwarded(ViewState("RFDNo"))

    '                If ViewState("isSales") = True Or ViewState("isProgramManager") = True Then
    '                    btnBusinessAwarded.Visible = ViewState("isEdit")
    '                End If


    '            End If

    '            'if changed from new to other, then business is automatically awarded
    '            If iBusinessProcessAction > 1 Then
    '                ViewState("bBusinessAwarded") = True

    '                If cbQualityEngineeringRequired.Checked = True And ViewState("AllApprovedBeforeQualityEngineer") = True Then
    '                    'update approval status for QE
    '                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 22, ViewState("QualityEngineerTeamMemberID"), "", 2, Today.Date)
    '                End If

    '                If cbPurchasingRequired.Checked = True And ViewState("AllApprovedBeforePurchasing") = True And ViewState("PurchasingTeamMemberID") > 0 Then
    '                    'update approval status for Purchasing
    '                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 7, ViewState("PurchasingTeamMemberID"), "", 2, Today.Date)
    '                End If
    '            End If

    '            ViewState("BusinessProcessActionID") = iBusinessProcessAction
    '        End If

    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub

    'Protected Sub ddProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProgram.SelectedIndexChanged

    '    Try

    '        ClearMessages()

    '        'If ddProgram.SelectedIndex >= 0 And ddPlatform.SelectedIndex >= 0 And ddMakes.SelectedIndex >= 0 Then
    '        If ddProgram.SelectedIndex >= 0 And ddMakes.SelectedIndex >= 0 Then
    '            'System.Threading.Thread.Sleep(3000)
    '            ViewState("CurrentCustomerProgramID") = ddProgram.SelectedValue

    '            Dim ds As DataSet = New DataSet
    '            'ds = commonFunctions.GetPlatformProgram(ddPlatform.SelectedValue, ddProgram.SelectedValue, "", "", ddMakes.SelectedValue)
    '            ds = commonFunctions.GetPlatformProgram(0, ddProgram.SelectedValue, "", "", ddMakes.SelectedValue)
    '            If commonFunctions.CheckDataSet(ds) = True Then
    '                Dim NoOfDays As String = ""
    '                Select Case ds.Tables(0).Rows(0).Item("EOPMM").ToString.Trim
    '                    Case "01"
    '                        NoOfDays = "31"
    '                    Case "02"
    '                        NoOfDays = "28"
    '                    Case "03"
    '                        NoOfDays = "31"
    '                    Case "04"
    '                        NoOfDays = "30"
    '                    Case "05"
    '                        NoOfDays = "31"
    '                    Case "06"
    '                        NoOfDays = "30"
    '                    Case "07"
    '                        NoOfDays = "31"
    '                    Case "08"
    '                        NoOfDays = "31"
    '                    Case "09"
    '                        NoOfDays = "30"
    '                    Case 10
    '                        NoOfDays = "31"
    '                    Case 11
    '                        NoOfDays = "30"
    '                    Case 12
    '                        NoOfDays = "31"
    '                End Select
    '                If ds.Tables(0).Rows(0).Item("EOPMM").ToString.Trim <> "" Then
    '                    txtEOPDate.Text = ds.Tables(0).Rows(0).Item("EOPMM").ToString() & "/" & NoOfDays & "/" & ds.Tables(0).Rows(0).Item("EOPYY").ToString()
    '                End If
    '                If ds.Tables(0).Rows(0).Item("SOPMM").ToString.Trim <> "" Then
    '                    txtSOPDate.Text = ds.Tables(0).Rows(0).Item("SOPMM").ToString.Trim & "/01/" & ds.Tables(0).Rows(0).Item("SOPYY").ToString.Trim

    '                    'pick current year if inside SOP and EOP range 
    '                    If ds.Tables(0).Rows(0).Item("SOPYY") < Today.Year And Today.Year <= ds.Tables(0).Rows(0).Item("EOPYY") Then
    '                        ddYear.SelectedValue = Today.Year
    '                    Else
    '                        ddYear.SelectedValue = ds.Tables(0).Rows(0).Item("SOPYY").ToString.Trim
    '                    End If

    '                End If

    '                '2012-Mar-03 - temporarily disabled - requested by Lynette
    '                '    iBtnPreviewDetail.Visible = True
    '                '    Dim strPreviewClientScript2 As String = "javascript:void(window.open('../DataMaintenance/ProgramDisplay.aspx?pPlatID=0&pPgmID=" & ddProgram.SelectedValue & " '," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=yes,location=yes'));"
    '                '    iBtnPreviewDetail.Attributes.Add("Onclick", strPreviewClientScript2)
    '                'Else
    '                '    iBtnPreviewDetail.Visible = False
    '            End If
    '        End If 'EOF ddProgram.SelectedValue

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub ddBusinessProcessAction_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddBusinessProcessAction.SelectedIndexChanged

        Try

            ShowHideProgramManager()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvSupportingDoc_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupportingDoc.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim lblSubscriptionIDTemp As Label = CType(e.Row.FindControl("lblViewSubscriptionID"), Label)
                Dim iSubscriptionID As Integer = 0

                If lblSubscriptionIDTemp IsNot Nothing Then
                    If lblSubscriptionIDTemp.Text.Trim <> "" Then

                        Dim imgButton As ImageButton = CType(e.Row.FindControl("iBtnSupportingDocDelete"), ImageButton)
                        imgButton.CssClass = "none"

                        iSubscriptionID = CType(lblSubscriptionIDTemp.Text.Trim, Integer)

                        If iSubscriptionID = 0 Then
                            imgButton.CssClass = ""
                        Else
                            If iSubscriptionID = ViewState("SubscriptionID") Then
                                imgButton.CssClass = ""
                            Else
                                If (iSubscriptionID = 108 And ViewState("isPackaging") = True) _
                                Or (iSubscriptionID = 139 And ViewState("isPurchasingExternalRFQ") = True) _
                                Or (iSubscriptionID = 7 And ViewState("isPurchasing") = True) _
                                Or (iSubscriptionID = 6 And ViewState("isCosting") = True) Then
                                    imgButton.CssClass = ""
                                End If

                                If (iSubscriptionID = 66 And ViewState("isProcess") = True) _
                                Or (iSubscriptionID = 65 And ViewState("isTooling") = True) _
                                Or (iSubscriptionID = 119 And ViewState("isCapital") = True) Then
                                    imgButton.CssClass = ""
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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageSupportingDocs.Text = lblMessage.Text
        lblMessageSupportingDocsBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnGetFGDMSBOM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetFGDMSBOM.Click

        Try
            ClearMessages()

            If txtNewDrawingNo.Text.Trim <> "" Then
                Dim strDMSBOMSelectionPage = "RFD_Drawing_BOM_Selection.aspx?DrawingNo=" & txtNewDrawingNo.Text.Trim & "&RFDNo=" & ViewState("RFDNo")

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Select Child Parts for RFD", "window.open('" & strDMSBOMSelectionPage & "'," & Now.Ticks & ",'resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text
        lblMessageChildPartBottom.Text = lblMessage.Text
        lblMessageChildPartDetails.Text = lblMessage.Text

    End Sub

    Protected Sub txtNewDrawingNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewDrawingNo.TextChanged
        Try
            lblMessage.Text = ""
            ''**LRey 07/01/2014**/
            ''Confirm that Product Engineer can issue the DMS# change
            If (ddStatus.SelectedValue <> 1 Or ddStatus.SelectedValue <> 5) Then
                ''Allow validation if RFD is neither in "Open" or "Rejected" status
                ''** Is the RFD "Waiting for Approval" or  "Approved" by Costing?
                Select Case ViewState("CostingStatusID")
                    Case 9 ''Waiting for approval
                        If txtNewDrawingNo.Text <> txtHDNewDrawingNo.Text Then
                            lblMessage.Text = "ERROR: This RFD is 'Waiting for Cost Sheet Approval'. Please contact the Costing Coordinator to reject the RFD to allow the DMS Drawing No. update."
                            lblMessage.Visible = True
                            If MsgBox("This RFD is 'Waiting for Cost Sheet Approval'. Please contact the Costing Coordinator to reject the RFD to allow the DMS Drawing No. update.", MsgBoxStyle.OkOnly, "DMS Drawing No. Alert") = MsgBoxResult.Ok Then
                                txtNewDrawingNo.Text = txtHDNewDrawingNo.Text
                            End If
                        End If
                    Case 3 ''Approved
                        If txtNewDrawingNo.Text <> txtHDNewDrawingNo.Text Then
                            lblMessage.Text = "ERROR: This RFD has been 'Approved' by Costing. Please contact all respective team members for next steps."
                            lblMessage.Visible = True
                            If MsgBox("This RFD has been 'Approved' by Costing. Please contact all respective team members for next steps.", MsgBoxStyle.OkOnly, "DMS Drawing No. Alert") = MsgBoxResult.Ok Then
                                txtNewDrawingNo.Text = txtHDNewDrawingNo.Text
                            End If
                        End If
                    Case Else
                        If txtNewDrawingNo.Text <> txtHDNewDrawingNo.Text Then
                            'If MsgBox("You have updated the DMS Drawing No. Do you wish to reset the Approval Routing? If YES, the Approval Routing will be reset and an email notification will be sent to all involved.", MsgBoxStyle.YesNo, "DMS Drawing No. Update") = MsgBoxResult.Yes Then
                            '    ''**Confirm DMS Drawing No is valid before resetting.
                            If MsgBox("You are about to update this RFD with a new DMS Drawing No; this will reset the Approval Routing for resubmission. Click 'YES' to accept this change or click 'NO' to cancel.", MsgBoxStyle.YesNo, "DMS Drawing No. Alert") = MsgBoxResult.Yes Then

                                ''**Reset approval Chain
                                ResetApprovalRoutingList()
                                RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "DMS Drawing No was updated" + IIf((txtHDNewDrawingNo.Text = "" And txtHDNewDrawingNo.Text = Nothing), "", " from " + txtHDNewDrawingNo.Text) + " to " + txtNewDrawingNo.Text + ". Approval Routing was reset for resubmission.")

                                ViewState("DMSDrawingNoUpdate") = "DMS Drawing No was updated " + IIf((txtHDNewDrawingNo.Text = "" And txtHDNewDrawingNo.Text = Nothing), "", " from " + txtHDNewDrawingNo.Text) + " to " + txtNewDrawingNo.Text + "."

                                EnableControls()
                                '' btnSubmitApproval_Click(sender, e)
                            Else
                                If MsgBox("Update Cancelled. Approval Routing will remain the same.", MsgBoxStyle.OkOnly, "DMS Drawing No. Update Cancelled") = MsgBoxResult.Ok Then
                                    txtNewDrawingNo.Text = txtHDNewDrawingNo.Text
                                End If
                            End If
                        End If
                End Select
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

    Private Sub ResetApprovalRoutingList()

        Try
            'get capital if checked
            If cbCapitalRequired.Checked = True Then
                'open
                If ViewState("CapitalTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 119, ViewState("CapitalTeamMemberID"), "", 0, 1, "")
                End If
            End If

            'get packaging if checked
            If cbPackagingRequired.Checked = True Then
                'Case 0, 1, 5 ' none, open, or rejected -> in-process
                If ViewState("PackagingTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 108, ViewState("PackagingTeamMemberID"), "", 0, 1, "")
                End If
            End If

            'get PlantController if checked
            If cbPlantControllerRequired.Checked = True Then
                'open
                If ViewState("PlantControllerTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 20, ViewState("PlantControllerTeamMemberID"), "", 0, 1, "")
                End If

            End If

            'get Product Engineering (Development) if checked
            If cbProductDevelopmentRequired.Checked = True Then
                '    Case 0, 1, 5 ' none, open, or rejected -> in-process
                If ViewState("ProductDevelopmentTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 5, ViewState("ProductDevelopmentTeamMemberID"), "", 0, 1, "")
                End If

            End If

            'get process if checked
            If cbProcessRequired.Checked = True Then
                'open
                If ViewState("ProcessTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 66, ViewState("ProcessTeamMemberID"), "", 0, 1, "")
                End If
            End If

            'get tooling if checked
            If cbToolingRequired.Checked = True Then
                'open
                If ViewState("ToolingTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 65, ViewState("ToolingTeamMemberID"), "", 0, 1, "")
                End If

            End If

            'get purchasing for External RFQ if checked
            If cbPurchasingExternalRFQRequired.Checked = True Then
                '    Case 0, 1, 5 ' none, open, or rejected -> open
                If ViewState("PurchasingExternalRFQTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 139, ViewState("PurchasingExternalRFQTeamMemberID"), "", 0, 1, "")
                End If
            End If

            'get costing if checked 
            If cbCostingRequired.Checked = True Then
                If ViewState("CostingTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 6, ViewState("CostingTeamMemberID"), "", 0, 1, "")
                End If
            End If

            'quality engineer must be checked after everyone except purchasing
            'see rules inside this logic about when QE is notified
            'get quality engineering if checked
            If cbQualityEngineeringRequired.Checked = True Then
                '    Case 0, 1, 5 ' none, open, or rejected -> open
                If ViewState("QualityEngineerTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 22, ViewState("QualityEngineerTeamMemberID"), "", 0, 1, "")
                End If
            End If

            'purchasing must be checked last in this list because notification depends upon other departments status
            'get purchasing if checked
            If cbPurchasingRequired.Checked = True Then
                '    Case 0, 1, 5 ' none, open, or rejected -> open
                If ViewState("PurchasingTeamMemberID") > 0 Then
                    RFDModule.UpdateRFDApprovalStatus(ViewState("RFDNo"), 7, ViewState("PurchasingTeamMemberID"), "", 0, 1, "")
                End If
            End If

            gvApproval.DataBind()


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Public Function QuoteOnlySupDocUpdate() As Boolean
        Dim dReturnValue As Boolean = True
        Try
            lblMessage.Text = ""
            ''**LRey 07/02/2014**/
            ''Confirm that Product Engineer can update the RFD with a new supporting document
            If (ddStatus.SelectedValue <> 1 Or ddStatus.SelectedValue <> 5) And (ViewState("BusinessProcessTypeID") = 7) Then
                ''Allow validation if RFD is neither in neither "Open" or "Rejected" status
                ''This is for "Quote Only" RFD's
                ''** Is the RFD "Waiting for Approval" or  "Approved" by Costing?
                Select Case ViewState("CostingStatusID")
                    Case 9 ''Waiting for approval
                        lblMessage.Text = "ERROR: This RFD is 'Waiting for Cost Sheet Approval'. Please contact the Costing Coordinator to reject the RFD to allow a file upload."
                        lblMessage.Visible = True
                        If MsgBox("This RFD is 'Waiting for Cost Sheet Approval'. Please contact the Costing Coordinator to reject the RFD to allow a file upload.", MsgBoxStyle.OkOnly, "Supporting Document Alert") = MsgBoxResult.Ok Then
                            dReturnValue = False
                        End If

                    Case 3 ''Approved

                        lblMessage.Text = "ERROR: This RFD has been 'Approved' by Costing. Please contact all respective team members for next steps."
                        lblMessage.Visible = True
                        If MsgBox("This RFD has been 'Approved' by Costing. Please contact all respective team members for next steps.", MsgBoxStyle.OkOnly, "Supporting Document Alert") = MsgBoxResult.Ok Then
                            dReturnValue = False
                        End If

                    Case Else
                        'If MsgBox("You have updated the DMS Drawing No. Do you wish to reset the Approval Routing? If YES, the Approval Routing will be reset and an email notification will be sent to all involved.", MsgBoxStyle.YesNo, "DMS Drawing No. Update") = MsgBoxResult.Yes Then
                        '    ''**Confirm DMS Drawing No is valid before resetting.
                        If MsgBox("You are about to update this RFD with a new Supporting Document upload; this will reset the Approval Routing for resubmission? Click 'YES' to accept this upload or click 'NO' to cancel.", MsgBoxStyle.YesNo, "Supporting Document Alert") = MsgBoxResult.Yes Then

                            ''**Reset approval Chain
                            ResetApprovalRoutingList()
                            RFDModule.InsertRFDHistory(ViewState("RFDNo"), ViewState("TeamMemberID"), "Supporting Document added. Approval Routing was reset for resubmission.")

                            ViewState("QuoteOnlySupDocUpdate") = "A new Supporting Document was added. It may contain updated information for this RFD, please review."

                            EnableControls()
                            '' btnSubmitApproval_Click(sender, e)
                        Else
                            If MsgBox("Upload Cancelled. Approval Routing will remain the same.", MsgBoxStyle.OkOnly, "Supporting Document Alert") = MsgBoxResult.Ok Then
                                dReturnValue = False
                            End If
                        End If

                End Select
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
        QuoteOnlySupDocUpdate = dReturnValue
    End Function 'EOF QuoteOnlySupDocUpdate

End Class
