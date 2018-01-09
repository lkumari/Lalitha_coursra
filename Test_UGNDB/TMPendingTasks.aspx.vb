' ************************************************************************************************
' Name:		Home.aspx
' Purpose:	This Code Behind is for the UGNDB Home page.
'
' Date		    Author	    
' 01/22/2008    LRey			Created .Net application
' 12/02/2010    RCarlson        Fixed AR Module Links - they new version of the AR Module is not ready yet so old links still need to be there
' 02/02/2011    RCarlson        Added new RFD Module
' 06/08/2011    RCarlson        Adjusted new AR Module links
' 09/19/2011    LRey            Added Spending Request (P)
' 03/27/2012    LRey            Added Color Code column to show how many hours TM's are past due on assigned tasks
' 05/07/2012    LRey            Added AR Operations Deduction to pending tasks.
' 05/12/2012    LRey            Added Spending Request (D) to Pending Tasks
' 10/10/2012    RCarlson        Added Spending Request (U) to Pending Tasks (Tooling Authorization Dieshop)
' ************************************************************************************************

Partial Class TMPendingTasks
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim m As ASP.crviewtmmasterpage_master = Master
        ' ''check test or production environments
        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "Please Review/Complete the following PENDING UGNDB or UGNHR records."
            mpTextBox.Visible = True
            mpTextBox.Font.Bold = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

        If HttpContext.Current.Session("sessionDrawingCurrentPage") IsNot Nothing Then
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

        End If
    End Sub 'EOF Page_Load

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
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Pam.DeLor", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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

    End Sub 'EOF CheckRights

    Public Function GoToApproval(ByVal RecType As String, ByVal SecondaryPreview As String, ByVal RecID As String, ByVal Status As String) As String
        ''************************************
        ''Used for pending task view.
        ''************************************
        Dim strReturnValue As String = ""

        Try
            If RecID <> "" Then
                Select RecType
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
                    Case "Spending Request (T)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/ToolingExpProj.aspx?pProjNo=" & RecID
                        Else
                            strReturnValue = "~/EXP/crExpProjToolingApproval.aspx?pProjNo=" & RecID & "&pAprv=1"
                        End If
                    Case "Spending Request (A)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/AssetsExpProj.aspx?pProjNo=" & RecID
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
                        strReturnValue = "http://tweb1.ugnnet.com/prod_ugndb/ugndb_login.asp?ProjectNumber=" & RecID & "&psu=" & SecondaryPreview
                    Case "Spending Request (R)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/RepairExpProj.aspx?pProjNo=" & RecID
                        Else
                            strReturnValue = "~/EXP/crExpProjRepairApproval.aspx?pProjNo=" & RecID & "&pAprv=1"
                        End If
                    Case "Spending Request (P)"
                        If Status = "Pending Submission" Or Status = "Rejected" Or Status = "Pending Response to Question" Or Status = "Please complete Est Cmplt Dt has been met." Then
                            strReturnValue = "~/EXP/PackagingExpProj.aspx?pProjNo=" & RecID
                        Else
                            strReturnValue = "~/EXP/crExpProjPackagingApproval.aspx?pProjNo=" & RecID & "&pAprv=1"
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
                        strReturnValue = "http://tweb1.ugnnet.com:8086/Prod_UGNHR"
                    Case "UGNHR-REQ"
                        strReturnValue = "http://tweb1.ugnnet.com:8086/Prod_UGNHR"
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

    End Function 'EOF GoToApproval

    Protected Function SetBackGroundColor(ByVal NoOfHoursOverdue As Integer) As String

        Dim strReturnValue As String = "Lime"

        If NoOfHoursOverdue >= 24 And NoOfHoursOverdue <= 47 Then
            strReturnValue = "Yellow"
        ElseIf NoOfHoursOverdue >= 48 Then
            strReturnValue = "Red"
        End If

        SetBackGroundColor = strReturnValue

    End Function 'EOF SetBackGroundColor

    Protected Function SetTextColor(ByVal NoOfHoursOverdue As Integer) As Color

        Dim strReturnValue As Color = Color.Black

        If NoOfHoursOverdue >= 24 And NoOfHoursOverdue <= 47 Then
            strReturnValue = Color.Black
        ElseIf NoOfHoursOverdue >= 48 Then
            strReturnValue = Color.White
        End If

        SetTextColor = strReturnValue

    End Function 'EOF SetTextColor

    Protected Function SetTextLabel(ByVal NoOfHoursOverdue As Integer) As String

        Dim strReturnValue As String = NoOfHoursOverdue & " hrs past due"

        If NoOfHoursOverdue >= 24 And NoOfHoursOverdue <= 47 Then
            strReturnValue = NoOfHoursOverdue & " hrs past due"
        ElseIf NoOfHoursOverdue >= 48 Then
            strReturnValue = "48+ hrs past due"
        End If

        SetTextLabel = strReturnValue

    End Function 'EOF SetTextLabel

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
    End Function 'EOF SetPreviewVisible

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

    End Function 'EOF

    Private Sub BindData()
        Try
            Dim ds As DataSet

            If ViewState("TeamMemberID") > 0 Then
                ds = commonFunctions.GetUGNDBPendingTasks(ViewState("TeamMemberID"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    lblCurrentPagePending.Visible = True
                    cmdFirstPending.Visible = True
                    cmdPrevPending.Visible = True
                    txtGoToPagePending.Visible = True
                    cmdGoPending.Visible = True
                    cmdNextPending.Visible = True
                    cmdLastPending.Visible = True
                    ' ''accPending.Visible = True

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

                    Dim mpTextBox As Label
                    mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
                    If Not mpTextBox Is Nothing Then
                        mpTextBox.Text = "You Have NO Pending Activities."
                        mpTextBox.Visible = True
                        mpTextBox.Font.Bold = True
                        Master.FindControl("SiteMapPath1").Visible = False
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
    End Sub 'EOF BindData

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

    End Property 'EOF CurrentPagePending

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

    End Sub 'EOF cmdLastPending_Click

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

    End Sub 'EOF cmdFirstPending_Click

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

    End Sub 'EOF cmdGoPending_Click

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

    End Sub 'EOF cmdNextPending_Click

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

    End Sub 'EOF cmdPrevPending_Click

End Class
