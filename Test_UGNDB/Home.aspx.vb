' ************************************************************************************************
' Name:		Home.aspx
' Purpose:	This Code Behind is for the UGNDB Home page.
'
' Date		    Author	    
' 01/22/2008    LRey			Created .Net application
' 07/23/2008    RCarlson        Began researching how to have single signon to OLD UGN Database
' 07/24/2008    RCarlson        Insert Links to Old UGN DB Sign On
' 05/11/2009    RCarlson        Added Accordion and Linke for Costing Module - pending and last 5 tasks
' 08/26/2009    RCarlson        Added ECI to Pending Items and Recent Items taks
' 02/10/2010    RCarlson        Added Chemical Review From to Pending and Recent Tasks
' 03/01/2010    LRey            Added Capital Projects (T) to Pending tasks.
' 03/02/1020    RCarlson        Added Cost Reduction Pending Tasks
' 07/21/2010    LRey            Added Capital Projects (A) to Pending tasks.
' 07/28/2010    RCarlson        Changed AR Module Links
' 10/28/2010    RCarlson        Added RFD Module
' 06/08/2011    RCarlson        Adjusted new AR Module links
' 09/11/2011    LREy            Added new Spending Request (P) to pending tasks.
' 03/27/2012    LRey            Added Color Code column to show how many hours TM's are past due on assigned tasks
' 05/07/2012    LRey            Added AR Operations Deduction to pending tasks.
' 05/12/2012    LRey            Added Spending Request (D) to Pending Tasks
' 10/10/2012    RCarlson        Added Spending Request (U) to Pending Tasks (Tooling Authorization Dieshop)
' 02/12/2013    RCarlson        Added Support Request
' 04/11/2013    LRey            Added Sample Material Request
' ************************************************************************************************
Partial Class Home
    Inherits System.Web.UI.Page
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet

            Dim iTeamMemberID As Integer = 0

            ViewState("TeamMemberID") = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Bryan.Hall", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
            End If

            ViewState("TeamMemberID") = iTeamMemberID

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Function GoToView(ByVal RecType As String, ByVal SecondaryPreview As String, ByVal RecID As String) As String
        ''************************************
        ''Used for recent task view.
        ''************************************
        Dim strReturnValue As String = ""

        Try
            If RecID <> "" Then
                Select Case RecType
                    Case "AR Event"
                        strReturnValue = "~/AR/AR_Event_Detail.aspx?AREID=" & RecID
                    Case "Costing"
                        strReturnValue = "~/Costing/Cost_Sheet_List.aspx?CostSheetID=" & RecID
                    Case "Costing Admin"
                        strReturnValue = "~/Costing/Cost_Sheet_Detail.aspx?CostSheetID=" & RecID
                    Case "ECI"
                        strReturnValue = "~/ECI/ECI_Detail.aspx?ECINo=" & RecID
                    Case "Safety - Chem Rev"
                        strReturnValue = "~/Safety/Chemical_Review_Form_Detail.aspx?ChemRevFormID=" & RecID
                    Case "Manuf. Metric"
                        strReturnValue = "~/PlantSpecificReports/Manufacturing_Metric_Detail.aspx?ReportID=" & RecID
                    Case "RFD"
                        strReturnValue = "~/RFD/RFD_Detail.aspx?RFDNo=" & RecID
                    Case "Purchasing (IOR)"
                        strReturnValue = "~/PUR/InternalOrderRequest.aspx?pIORNo=" & RecID & "&pProjNo=" & SecondaryPreview
                    Case "Purchasing (Supplier)"
                        strReturnValue = "~/SUP/SupplierRequest.aspx?pSUPNo=" & RecID
                    Case "Spending Request (A)"
                        strReturnValue = "~/EXP/AssetExpProj.aspx?pProjNo=" & RecID
                    Case "Spending Request (D)"
                        strReturnValue = "~/EXP/DevelopmentExpProj.aspx?pProjNo=" & RecID
                    Case "Spending Request (P)"
                        strReturnValue = "~/EXP/PackagingExpProj.aspx?pProjNo=" & RecID
                    Case "Spending Request (R)"
                        strReturnValue = "~/EXP/RepairExpProj.aspx?pProjNo=" & RecID
                    Case "Spending Request (T)"
                        strReturnValue = "~/EXP/ToolingExpProj.aspx?pProjNo=" & RecID
                    Case "Spending Request (U)"
                        strReturnValue = "~/EXP/ToolingAuthExpProj.aspx?TANo=" & RecID
                    Case "Operations Deduction"
                        strReturnValue = "~/AR/AR_Deduction.aspx?pARDID=" & RecID
                    Case "UGN Support Request"
                        strReturnValue = "~/Workflow/crSupport_Approval.aspx?JobNumber=" & RecID
                    Case "Sample Material Request"
                        strReturnValue = "~/PGM/SampleMaterialRequest.aspx?pSMRNo=" & RecID
                    Case "SMR Near Due Date"
                        strReturnValue = "~/PGM/SampleMaterialRequest.aspx?pSMRNo=" & RecID
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

        GoToView = strReturnValue

    End Function

    Public Function GoToApproval(ByVal RecType As String, ByVal SecondaryPreview As String, ByVal RecID As String, ByVal Status As String) As String
        ''************************************
        ''Used for pending task view.
        ''************************************
        Dim strReturnValue As String = ""

        Try
            If RecID <> "" Then
                Select Case RecType
                    Case "AR Event"
                        strReturnValue = "~/AR/AR_Event_Detail.aspx?AREID=" & RecID
                    Case "AR Approval"
                        strReturnValue = "~/AR/crAR_Event_Approval.aspx?AREID=" & RecID
                    Case "Costing"
                        strReturnValue = "~/Costing/Cost_Sheet_Approve.aspx?CostSheetID=" & RecID
                    Case "ECI"
                        strReturnValue = "~/ECI/ECI_Detail.aspx?ECINo=" & RecID
                    Case "Safety - Chem Rev"
                        strReturnValue = "~/Safety/Chemical_Review_Form_Detail.aspx?ChemRevFormID=" & RecID
                    Case "Spending Request (A)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/AssetExpProj.aspx?pProjNo=" & RecID
                        Else
                            strReturnValue = "~/EXP/crExpProjAssetsApproval.aspx?pProjNo=" & RecID & "&pAprv=1"
                        End If
                    Case "Spending Request (D)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Project close to Est Cmplt Dt - Pending TM(s) Approval" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/DevelopmentExpProj.aspx?pProjNo=" & RecID
                        Else
                            strReturnValue = "~/EXP/crExpProjDevelopmentApproval.aspx?pProjNo=" & RecID & "&pAprv=1"
                        End If
                    Case "Capital Projects (P)"
                        strReturnValue = "javascript:void(window.open('http://tweb1.ugnnet.com/prod_ugndb/ugndb_login.asp?ProjectNumber=" & RecID & "&psu=" & SecondaryPreview & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "Spending Request (P)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/PackagingExpProj.aspx?pProjNo=" & RecID
                        Else
                            strReturnValue = "~/EXP/crExpProjPackagingApproval.aspx?pProjNo=" & RecID & "&pAprv=1"
                        End If
                    Case "Spending Request (R)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/RepairExpProj.aspx?pProjNo=" & RecID
                        Else
                            strReturnValue = "~/EXP/crExpProjRepairApproval.aspx?pProjNo=" & RecID & "&pAprv=1"
                        End If
                    Case "Spending Request (T)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/ToolingExpProj.aspx?pProjNo=" & RecID
                        Else
                            strReturnValue = "~/EXP/crExpProjToolingApproval.aspx?pProjNo=" & RecID & "&pAprv=1"
                        End If
                    Case "Spending Request (U)"
                        strReturnValue = "~/EXP/ToolingAuthExpProj.aspx?TANo=" & RecID
                    Case "Cost Reduction"
                        strReturnValue = "~/CR/CostReduction.aspx?pProjNo=" & RecID
                    Case "Manuf. Metric"
                        strReturnValue = "~/PlantSpecificReports/Manufacturing_Metric_Detail.aspx?ReportID=" & RecID
                    Case "RFD"
                        strReturnValue = "~/RFD/RFD_Detail.aspx?RFDNo=" & RecID
                    Case "UGNHR-TA"
                        strReturnValue = "javascript:void(window.open('http://tweb1.ugnnet.com:8086/Prod_UGNHR" & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "UGNHR-REQ"
                        strReturnValue = "javascript:void(window.open('http://tweb1.ugnnet.com:8086/Prod_UGNHR" & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "Purchasing (IOR)"
                        If Status = "Pending Submission" Then
                            strReturnValue = "~/PUR/InternalOrderRequest.aspx?pIORNo=" & RecID & "&pProjNo=" & SecondaryPreview
                        Else
                            strReturnValue = "~/PUR/crInternalOrderRequestApproval.aspx?pIORNo=" & RecID & "&pProjNo=" & SecondaryPreview & "&pAprv=1"
                        End If
                    Case "Purchasing (Supplier)"
                        If Status = "Pending Submission" Then
                            strReturnValue = "~/SUP/SupplierRequest.aspx?pSUPNo=" & RecID
                        Else
                            strReturnValue = "~/SUP/crSupplierRequestApproval.aspx?pSUPNo=" & RecID & "&pAprv=1"
                        End If

                    Case "Operations Deduction"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "60 days past due - Pending TM(s) Approval" Then
                            strReturnValue = "~/AR/AR_Deduction.aspx?pARDID=" & RecID
                        Else
                            strReturnValue = "~/AR/crARDeductionApproval.aspx?pARDID=" & RecID & "&pAprv=1"
                        End If
                    Case "UGN Support Request"
                        strReturnValue = "~/Workflow/crSupport_Approval.aspx?JobNumber=" & RecID
                    Case "Sample Material Request"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Then
                            strReturnValue = "~/PGM/SampleMaterialRequest.aspx?pSMRNo=" & RecID
                        Else
                            strReturnValue = "~/PGM/crSampleMtrlReqApproval.aspx?pSMRNo=" & RecID & "&pAprv=1"
                        End If
                    Case "SMR Near Due Date"
                        strReturnValue = "~/PGM/SampleMaterialRequest.aspx?pSMRNo=" & RecID

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

        GoToApproval = strReturnValue

    End Function

    Protected Function SetBackGroundColor(ByVal NoOfHoursOverdue As Integer) As String

        Dim strReturnValue As String = "Lime"

        If NoOfHoursOverdue >= 24 And NoOfHoursOverdue <= 47 Then
            strReturnValue = "Yellow"
        ElseIf NoOfHoursOverdue >= 48 Then
            strReturnValue = "Red"
        End If

        SetBackGroundColor = strReturnValue

    End Function

    Protected Function SetTextColor(ByVal NoOfHoursOverdue As Integer) As Color

        Dim strReturnValue As Color = Color.Black

        If NoOfHoursOverdue >= 24 And NoOfHoursOverdue <= 47 Then
            strReturnValue = Color.Black
        ElseIf NoOfHoursOverdue >= 48 Then
            strReturnValue = Color.White
        End If

        SetTextColor = strReturnValue

    End Function

    Protected Function SetTextLabel(ByVal NoOfHoursOverdue As Integer) As String

        Dim strReturnValue As String = NoOfHoursOverdue & " hrs past due"

        If NoOfHoursOverdue >= 24 And NoOfHoursOverdue <= 47 Then
            strReturnValue = NoOfHoursOverdue & " hrs past due"
        ElseIf NoOfHoursOverdue >= 48 Then
            strReturnValue = "48+ hrs past due"
        End If

        SetTextLabel = strReturnValue

    End Function

    Protected Function SetPreviewVisible(ByVal PreviewType As String) As Boolean

        Dim dReturnValue As Boolean = False

        Try
            If PreviewType <> "" Then
                dReturnValue = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewVisible = dReturnValue
    End Function

    Protected Function SetPrimaryPreviewHyperLinkOnClick(ByVal PrimaryPreview As String, ByVal SecondaryPreview As String, ByVal RecID As String) As String
        ''************************************
        ''Used for pending task view - onclick event for popups.
        ''************************************
        Dim strReturnValue As String = ""
        ViewState("vHplnk") = False
        Try
            If RecID <> "" Then
                Select Case PrimaryPreview
                    Case "AR Event"
                        strReturnValue = "javascript:void(window.open('./AR/crPreview_AR_Event_Detail.aspx?AREID=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=600,width=900,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "CostForm"
                        strReturnValue = "javascript:void(window.open('./Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "ECI"
                        strReturnValue = "javascript:void(window.open('./ECI/ECI_Preview.aspx?ECINo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "Safety - Chem Rev"
                        strReturnValue = "javascript:void(window.open('./Safety/Chemical_Review_Form_Preview.aspx?ChemRevFormID=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "Spending Request (A)"
                        strReturnValue = "javascript:void(window.open('./EXP/crViewExpProjAssets.aspx?pProjNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Spending Request (D)"
                        strReturnValue = "javascript:void(window.open('./EXP/crViewExpProjDevelopment.aspx?pProjNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Capital Projects (P)"
                        strReturnValue = "javascript:void(window.open('http://tweb1.ugnnet.com/prod_ugndb/ugndb_login.asp?ProjectNumber=" & RecID & "&psu=" & SecondaryPreview & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "Spending Request (P)"
                        strReturnValue = "javascript:void(window.open('./EXP/crViewExpProjPackaging.aspx?pProjNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Spending Request (R)"
                        strReturnValue = "javascript:void(window.open('./EXP/crViewExpProjRepair.aspx?pProjNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Spending Request (T)"
                        strReturnValue = "javascript:void(window.open('./EXP/crViewExpProjTooling.aspx?pProjNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Spending Request (U)"
                        strReturnValue = "javascript:void(window.open('./EXP/crViewExpProjToolingAuth.aspx?FormType=TA&ArchiveData=0&TAProjectNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Cost Reduction"
                        strReturnValue = "javascript:void(window.open('./CR/crViewCostReductionDetail.aspx?pProjNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "MonthlyManufMetric"
                        strReturnValue = "javascript:void(window.open('./PlantSpecificReports/crPreview_Manufacturing_Metric_Report.aspx?&ReportType=M&ReportID=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "RFD"
                        strReturnValue = "javascript:void(window.open('./RFD/crRFD_Preview.aspx?RFDNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "UGNHR-TA"
                        strReturnValue = "javascript:void(window.open('http://tweb1.ugnnet.com:8086/Prod_UGNHR" & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "UGNHR-REQ"
                        strReturnValue = "javascript:void(window.open('http://tweb1.ugnnet.com:8086/Prod_UGNHR" & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                    Case "Purchasing (IOR)"
                        strReturnValue = "javascript:void(window.open('./PUR/crViewInternalOrderRequest.aspx?pIORNo=" & RecID & "&pProjNo=" & SecondaryPreview & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Purchasing (Supplier)"
                        strReturnValue = "javascript:void(window.open('./SUP/crViewSupplierRequest.aspx?pSUPNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Operations Deduction"
                        strReturnValue = "javascript:void(window.open('./AR/crViewARDeduction.aspx?pARDID=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "UGN Support Request"
                        strReturnValue = "javascript:void(window.open('./Workflow/crSupport_Preview.aspx?JobNumber=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                    Case "Sample Material Request"
                        strReturnValue = "javascript:void(window.open('./PGM/crViewSampleMtrlReq.aspx?pSMRNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
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

        SetPrimaryPreviewHyperLinkOnClick = strReturnValue
    End Function

    Protected Function SetPrimaryPreviewHyperLinkHREF(ByVal PrimaryPreview As String, ByVal RecID As String) As String
        ''************************************
        ''Used for pending task view. HREF for redirecting to page (NOT Popup)
        ''************************************
        Dim strReturnValue As String = ""
        ViewState("vHplnk") = False
        Try
            If RecID <> "" Then
                Select Case PrimaryPreview
                    Case "AR Event"
                        strReturnValue = "#"
                    Case "CostForm"
                        strReturnValue = "#"
                    Case "ECI"
                        strReturnValue = "#"
                    Case "Safety - Chem Rev"
                        strReturnValue = "#"
                    Case "Spending Request (A)"
                        strReturnValue = "#"
                    Case "Spending Request (D)"
                        strReturnValue = "#"
                    Case "Capital Projects (P)"
                        strReturnValue = "#"
                    Case "Spending Request (P)"
                        strReturnValue = "#"
                    Case "Spending Request (R)"
                        strReturnValue = "#"
                    Case "Spending Request (T)"
                        strReturnValue = "#"
                    Case "Spending Request (U)"
                        strReturnValue = "#"
                    Case "Cost Reduction"
                        strReturnValue = "#"
                    Case "MonthlyManufMetric"
                        strReturnValue = "#"
                    Case "RFD"
                        strReturnValue = "#"
                    Case "Purchasing (IOR)"
                        strReturnValue = "#"
                    Case "Purchasing (Supplier)"
                        strReturnValue = "#"
                    Case "Operations Deduction"
                        strReturnValue = "#"
                    Case "UGN Support Request"
                        strReturnValue = "#"
                    Case "Sample Material Request"
                        strReturnValue = "#"
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

        SetPrimaryPreviewHyperLinkHREF = strReturnValue
    End Function

    Protected Function SetSecondaryPreviewHyperLink(ByVal SecondaryPreview As String, ByVal RecID As String) As String

        Dim strReturnValue As String = ""

        Try
            If RecID <> "" Then
                Select Case SecondaryPreview
                    Case "Spending Request (U)"
                        strReturnValue = "javascript:void(window.open('./EXP/crViewExpProjToolingAuth.aspx?FormType=DS&ArchiveData=0&TAProjectNo=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"

                    Case "DieLayout"
                        strReturnValue = "javascript:void(window.open('./Costing/Die_Layout_Preview.aspx?CostSheetID=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=810,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
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

        SetSecondaryPreviewHyperLink = strReturnValue

    End Function

    Protected Function SetHistoryHyperLinkHREF(ByVal RecType As String, ByVal RecID As String) As String
        ''************************************
        ''Used for pending task view. HREF for redirecting to page (NOT Popup)
        ''************************************
        'ViewState("vHplnk") = False
        Dim strReturnValue As String = ""

        Try
            If RecID <> "" Then
                Select Case RecType
                    Case "AR Event", "AR Approval"
                        strReturnValue = "~/AR/AR_Event_History.aspx?AREID=" & RecID
                    Case "Exp Proj - Tooling"
                        'ViewState("vHplnk") = True
                        strReturnValue = "~/EXP/ToolingExpProjHistory.aspx?pProjNo=" & RecID & "&pAprv=1"
                    Case "Spending Request (A)"
                        strReturnValue = "~/EXP/AssetsExpProjHistory.aspx?pProjNo=" & RecID & "&pAprv=1"
                    Case "Spending Request (D)"
                        strReturnValue = "~/EXP/DevelopmentExpProjHistory.aspx?pProjNo=" & RecID & "&pAprv=1"
                    Case "Spending Request (P)"
                        strReturnValue = "~/EXP/PackagingExpProjHistory.aspx?pProjNo=" & RecID & "&pAprv=1"
                    Case "Spending Request (R)"
                        strReturnValue = "~/EXP/RepairExpProjHistory.aspx?pProjNo=" & RecID & "&pAprv=1"
                    Case "Spending Request (T)"
                        strReturnValue = "~/EXP/ToolingExpProjHistory.aspx?pProjNo=" & RecID & "&pAprv=1"
                    Case "Spending Request (U)"
                        strReturnValue = "~/EXP/ToolingAuthExpProjHistory.aspx?TANo=" & RecID
                    Case "Operations Deduction"
                        strReturnValue = "~/AR/AR_Deduction_History.aspx?pARDID=" & RecID & "&pAprv=1"
                    Case "Manuf. Metric"
                        strReturnValue = "~/PlantSpecificReports/Manufacturing_Metric_History.aspx?ReportID=" & RecID
                    Case "RFD"
                        strReturnValue = "~/RFD/RFD_History.aspx?RFDNo=" & RecID
                    Case "Sample Material Request"
                        strReturnValue = "~/PGM/SampleMtrlReqHistory.aspx?pSMRNo=" & RecID & "&pAprv=1"
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

        SetHistoryHyperLinkHREF = strReturnValue

    End Function

    Protected Function SetHistoryHyperLinkONCLICK(ByVal RecType As String, ByVal RecID As String) As String
        ''************************************
        ''Used for pending task view.  onclick event for popups.
        ''************************************

        Dim strReturnValue As String = ""

        Try
            If RecID <> "" Then
                Select Case RecType
                    Case "Costing"
                        strReturnValue = "javascript:void(window.open('./Costing/Cost_Sheet_Pre_Approval_PopUp.aspx?CostSheetID=" & RecID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=300,width=500,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
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

        SetHistoryHyperLinkONCLICK = strReturnValue

    End Function

    Protected Function GetToolTip(ByVal PreviewType As String) As String

        Dim strReturnValue As String = ""

        Try

            strReturnValue = PreviewType

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        GetToolTip = strReturnValue

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim m As ASP.masterpage_master = Master

        ' ''check test or production environments
        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()
        If strProdOrTestEnvironment = "Test_UGNDB" Then
            m.PageTitle = "UGN, Inc.: TEST"
            m.ContentLabel = "Welcome to the TEST UGN Database"
        Else
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Welcome to the UGN Database"
        End If

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "Home"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If
        ''End If

        If HttpContext.Current.Session("sessionHomeCurrentPageRecent") IsNot Nothing Then
            CurrentPageRecent = HttpContext.Current.Session("sessionHomeCurrentPageRecent")
        End If

        If HttpContext.Current.Session("sessionHomeCurrentPagePending") IsNot Nothing Then
            CurrentPagePending = HttpContext.Current.Session("sessionHomeCurrentPagePending")
        End If

        If Not Page.IsPostBack Then
            ''********************
            ''Check Roles, Rights, and Subscriptions
            ''********************
            CheckRights()

            ''********************
            ''Build and bind data
            ''********************
            BindData()

            'warning: if you try to set a default to these values, you might see the accordion open and close. RC
            If Request.Cookies("UGNDB_ShowPendingTasks") IsNot Nothing Then
                If Request.Cookies("UGNDB_ShowPendingTasks").Value.Trim <> "" Then
                    If CType(Request.Cookies("UGNDB_ShowPendingTasks").Value, Integer) = 1 Then
                        accPending.SelectedIndex = 0
                        cbShowPendingTasks.Checked = True
                    Else
                        accPending.SelectedIndex = -1
                        cbShowPendingTasks.Checked = False
                    End If
                End If

            Else
                accPending.SelectedIndex = 0
                cbShowPendingTasks.Checked = True
            End If

            If Request.Cookies("UGNDB_ShowRecentTasks") IsNot Nothing Then
                If Request.Cookies("UGNDB_ShowRecentTasks").Value.Trim <> "" Then
                    If CType(Request.Cookies("UGNDB_ShowRecentTasks").Value, Integer) = 1 Then
                        accRecent.SelectedIndex = 0
                        cbShowRecentTasks.Checked = True
                    Else
                        accRecent.SelectedIndex = -1
                        cbShowRecentTasks.Checked = False
                    End If
                End If

            Else
                accRecent.SelectedIndex = -1
                cbShowRecentTasks.Checked = False
            End If
        End If

        'update link for appropriate Classic UGN DB SignOn Screen, depending upon test or prod environment
        lnkOldUGNDBSignOn.PostBackUrl = System.Configuration.ConfigurationManager.AppSettings("logout").ToString()

        ' ''07/23/2008 - In the event that Rod can figure out how to provide single signon to the classic asp web applications
        ' ''If commonFunctions.connectToOldUGNDatabase = True Then
        ' ''    lnkOldUGNDBMainMenu.Visible = True
        ' ''    Dim LogoutNowCookie As New HttpCookie("MM_Logout")
        ' ''    LogoutNowCookie.Domain = "tweb1.ugnnet.com"
        ' ''    LogoutNowCookie.Value = "False"
        ' ''    Response.Cookies.Add(LogoutNowCookie)

        ' ''    If strProdOrTestEnvironment = "Test_UGNDB" Then
        ' ''        lnkOldUGNDBMainMenu.PostBackUrl = "http://tweb1.ugnnet.com/Test_UGNDB/Main_Menu.asp?UgnFacility=" & Session("MM_UGNfacility")
        ' ''    Else
        ' ''        lnkOldUGNDBMainMenu.PostBackUrl = "http://tweb1.ugnnet.com/Prod_UGNDB/Main_Menu.asp?UgnFacility=" & Session("MM_UGNfacility")
        ' ''    End If
        ' ''End If


    End Sub

    Private Sub BindData()

        Try
            lblWelcomeText.Text = "For the New UGN Database Applications, please make your selection to the left."

            Dim ds As DataSet

            If ViewState("TeamMemberID") > 0 Then
                ds = commonFunctions.GetUGNDBPendingTasks(ViewState("TeamMemberID"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    emTipPending.Visible = True
                    lblCurrentPagePending.Visible = True
                    cmdFirstPending.Visible = True
                    cmdPrevPending.Visible = True
                    txtGoToPagePending.Visible = True
                    cmdGoPending.Visible = True
                    cmdNextPending.Visible = True
                    cmdLastPending.Visible = True
                    accPending.Visible = True

                    rpTasksPending.Visible = True

                    ' Populate the repeater control with the Items DataSet
                    Dim objPdsPending As PagedDataSource = New PagedDataSource
                    objPdsPending.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPdsPending.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPdsPending.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPdsPending.CurrentPageIndex = CurrentPagePending

                    rpTasksPending.DataSource = objPdsPending
                    rpTasksPending.DataBind()

                    lblCurrentPagePending.Text = "Page: " + (CurrentPagePending + 1).ToString() + " of " + objPdsPending.PageCount.ToString()
                    ViewState("LastPageCountPending") = objPdsPending.PageCount - 1
                    txtGoToPagePending.Text = CurrentPagePending + 1

                    ' Disable Prev or Next buttons if necessary
                    cmdFirstPending.Enabled = Not objPdsPending.IsFirstPage
                    cmdPrevPending.Enabled = Not objPdsPending.IsFirstPage
                    cmdNextPending.Enabled = Not objPdsPending.IsLastPage
                    cmdLastPending.Enabled = Not objPdsPending.IsLastPage
                Else
                    HttpContext.Current.Session("sessionHomeCurrentPagePending") = Nothing
                    ViewState("LastPageCountPending") = 0
                    CurrentPagePending = 0

                    emTipPending.Visible = False
                    lblCurrentPagePending.Visible = False
                    cmdFirstPending.Visible = False
                    cmdPrevPending.Visible = False
                    txtGoToPagePending.Visible = False
                    cmdGoPending.Visible = False
                    cmdNextPending.Visible = False
                    cmdLastPending.Visible = False

                    rpTasksPending.Visible = False

                    rpTasksPending.DataSource = Nothing
                    rpTasksPending.DataBind()
                End If

                ds = commonFunctions.GetUGNDBRecentTasks(ViewState("TeamMemberID"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    emTipRecent.Visible = True
                    lblCurrentPageRecent.Visible = True
                    cmdFirstRecent.Visible = True
                    cmdPrevRecent.Visible = True
                    txtGoToPageRecent.Visible = True
                    cmdGoRecent.Visible = True
                    cmdNextRecent.Visible = True
                    cmdLastRecent.Visible = True
                    accRecent.Visible = True

                    rpTasksRecent.Visible = True

                    ' Populate the repeater control with the Items DataSet
                    Dim objPdsRecent As PagedDataSource = New PagedDataSource
                    objPdsRecent.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPdsRecent.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPdsRecent.PageSize = 25

                    ' Set the PagedDataSource's current page
                    objPdsRecent.CurrentPageIndex = CurrentPageRecent

                    rpTasksRecent.DataSource = objPdsRecent
                    rpTasksRecent.DataBind()

                    lblCurrentPageRecent.Text = "Page: " + (CurrentPageRecent + 1).ToString() + " of " + objPdsRecent.PageCount.ToString()
                    ViewState("LastPageCountRecent") = objPdsRecent.PageCount - 1
                    txtGoToPageRecent.Text = CurrentPageRecent + 1

                    ' Disable Prev or Next buttons if necessary
                    cmdFirstRecent.Enabled = Not objPdsRecent.IsFirstPage
                    cmdPrevRecent.Enabled = Not objPdsRecent.IsFirstPage
                    cmdNextRecent.Enabled = Not objPdsRecent.IsLastPage
                    cmdLastRecent.Enabled = Not objPdsRecent.IsLastPage
                Else
                    HttpContext.Current.Session("sessionHomeCurrentPageRecent") = Nothing
                    ViewState("LastPageCountRecent") = 0
                    CurrentPageRecent = 0

                    emTipRecent.Visible = False
                    lblCurrentPageRecent.Visible = False
                    cmdFirstRecent.Visible = False
                    cmdPrevRecent.Visible = False
                    txtGoToPageRecent.Visible = False
                    cmdGoRecent.Visible = False
                    cmdNextRecent.Visible = False
                    cmdLastRecent.Visible = False

                    rpTasksRecent.Visible = False

                    rpTasksRecent.DataSource = Nothing
                    rpTasksRecent.DataBind()
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

    Public Property CurrentPagePending() As Integer

        Get
            ' look for current page in ViewState
            Dim o As Object = ViewState("_CurrentPagePending")
            If (o Is Nothing) Then
                Return 0 ' default page index of 0
            Else
                Return o
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("_CurrentPagePending") = value
        End Set

    End Property

    Public Property CurrentPageRecent() As Integer

        Get
            ' look for current page in ViewState
            Dim o As Object = ViewState("_CurrentPageRecent")
            If (o Is Nothing) Then
                Return 0 ' default page index of 0
            Else
                Return o
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("_CurrentPageRecent") = value
        End Set

    End Property

    Protected Sub cmdLastPending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLastPending.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the last page
            CurrentPagePending = ViewState("LastPageCountPending")
            HttpContext.Current.Session("sessionHomeCurrentPagePending") = CurrentPagePending

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

    Protected Sub cmdFirstPending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirstPending.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the first page
            CurrentPagePending = 0
            HttpContext.Current.Session("sessionHomeCurrentPagePending") = CurrentPagePending

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

    Protected Sub cmdGoPending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGoPending.Click

        lblMessage.Text = ""

        Try
            If txtGoToPagePending.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPagePending.Text > ViewState("LastPageCountPending") Then
                    CurrentPagePending = ViewState("LastPageCountPending")
                Else
                    CurrentPagePending = txtGoToPagePending.Text - 1
                End If

                HttpContext.Current.Session("sessionHomeCurrentPagePending") = CurrentPagePending

                ' Reload control
                BindData()
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

    Protected Sub cmdNextPending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNextPending.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the next page
            CurrentPagePending += 1
            HttpContext.Current.Session("sessionHomeCurrentPagePending") = CurrentPagePending

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

    Protected Sub cmdPrevPending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevPending.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the previous page
            CurrentPagePending -= 1
            HttpContext.Current.Session("sessionHomeCurrentPagePending") = CurrentPagePending

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

    Protected Sub cmdFirstRecent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirstRecent.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the first page
            CurrentPageRecent = 0
            HttpContext.Current.Session("sessionHomeCurrentPageRecent") = CurrentPageRecent

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

    Protected Sub cmdGoRecent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGoRecent.Click

        lblMessage.Text = ""

        Try
            If txtGoToPageRecent.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPageRecent.Text > ViewState("LastPageCountRecent") Then
                    CurrentPageRecent = ViewState("LastPageCountRecent")
                Else
                    CurrentPageRecent = txtGoToPageRecent.Text - 1
                End If

                HttpContext.Current.Session("sessionHomeCurrentPageRecent") = CurrentPageRecent

                ' Reload control
                BindData()
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

    Protected Sub cmdLastRecent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLastRecent.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the last page
            CurrentPageRecent = ViewState("LastPageCountRecent")
            HttpContext.Current.Session("sessionHomeCurrentPageRecent") = CurrentPageRecent

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

    Protected Sub cmdNextRecent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNextRecent.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the next page
            CurrentPageRecent += 1
            HttpContext.Current.Session("sessionHomeCurrentPageRecent") = CurrentPageRecent

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

    Protected Sub cmdPrevRecent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevRecent.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the previous page
            CurrentPageRecent -= 1
            HttpContext.Current.Session("sessionHomeCurrentPageRecent") = CurrentPageRecent

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

    Protected Sub cbShowRecentTasks_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbShowRecentTasks.CheckedChanged

        If cbShowRecentTasks.Checked = False Then
            Response.Cookies("UGNDB_ShowRecentTasks").Value = 0
            accRecent.SelectedIndex = -1
        Else
            Response.Cookies("UGNDB_ShowRecentTasks").Value = 1
        End If

    End Sub

    Protected Sub cbShowPendingTasks_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbShowPendingTasks.CheckedChanged

        If cbShowPendingTasks.Checked = False Then
            Response.Cookies("UGNDB_ShowPendingTasks").Value = 0
            accPending.SelectedIndex = -1
        Else
            Response.Cookies("UGNDB_ShowPendingTasks").Value = 1
        End If

    End Sub
End Class
