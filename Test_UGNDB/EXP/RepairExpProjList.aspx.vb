' ************************************************************************************************
' Name:	RepairExpProjList.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 11/22/2010    LRey			Created .Net application
' 06/21/2012    LRey            Added "Record Listed" to the search page.
' 10/25/2012    LRey            Added an image button to IOR's if submitted.
' ************************************************************************************************
Partial Class EXP_RepairExpProjList
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "R Project Search"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > R Project Search"
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
            ctl = m.FindControl("SPRExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            'focus on Vehicle List screen Program field
            txtProjectNo.Focus()

            If HttpContext.Current.Session("sessionExpCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionExpCurrentPage")
            End If

            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = ""
            End If
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
                HyperLink1.NavigateUrl = "~/EXP/crExpProjRepairApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1"
            Else
                ViewState("pAprv") = 0
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sProjNo") = ""
                ViewState("sSupProjNo") = ""
                ViewState("sProjTitle") = ""
                ViewState("sUGNFacility") = ""
                ViewState("sPLDRID") = ""
                ViewState("sDept") = ""
                ViewState("sPStat") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("EXPR_ProjNo") Is Nothing Then
                    txtProjectNo.Text = Server.HtmlEncode(Request.Cookies("EXPR_ProjNo").Value)
                    ViewState("sProjNo") = Server.HtmlEncode(Request.Cookies("EXPR_ProjNo").Value)
                End If


                If Not Request.Cookies("EXPR_SupProjNo") Is Nothing Then
                    txtSupProjectNo.Text = Server.HtmlEncode(Request.Cookies("EXPR_SupProjNo").Value)
                    ViewState("sSupProjNo") = Server.HtmlEncode(Request.Cookies("EXPR_SupProjNo").Value)
                End If

                If Not Request.Cookies("EXPR_ProjTitle") Is Nothing Then
                    txtProjectTitle.Text = Server.HtmlEncode(Request.Cookies("EXPR_ProjTitle").Value)
                    ViewState("sProjTitle") = Server.HtmlEncode(Request.Cookies("EXPR_ProjTitle").Value)
                End If

                If Not Request.Cookies("EXPR_UGNFacility") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPR_UGNFacility").Value)
                    ViewState("sUGNFacility") = Server.HtmlEncode(Request.Cookies("EXPR_UGNFacility").Value)
                End If

                If Not Request.Cookies("EXPR_PLDRID") Is Nothing Then
                    ddProjectLeader.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPR_PLDRID").Value)
                    ViewState("sPLDRID") = Server.HtmlEncode(Request.Cookies("EXPR_PLDRID").Value)
                End If

                If Not Request.Cookies("EXPR_DEPT") Is Nothing Then
                    txtDepartment.Text = Server.HtmlEncode(Request.Cookies("EXPR_DEPT").Value)
                    ViewState("sDept") = Server.HtmlEncode(Request.Cookies("EXPR_DEPT").Value)
                End If

                If Not Request.Cookies("EXPR_PSTAT") Is Nothing Then
                    ddProjectStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPR_PSTAT").Value)
                    ViewState("sPStat") = Server.HtmlEncode(Request.Cookies("EXPR_PSTAT").Value)
                End If

                ''******
                '' Bind drop down lists
                ''******
                BindData()

            Else
                ViewState("sProjNo") = txtProjectNo.Text
                ViewState("sSupProjNo") = txtSupProjectNo.Text
                ViewState("sProjTitle") = txtProjectTitle.Text
                ViewState("sUGNFacility") = ddUGNFacility.SelectedValue
                ViewState("sPLDRID") = ddProjectLeader.SelectedValue
                ViewState("sDept") = txtDepartment.Text
                ViewState("sPStat") = ddProjectStatus.SelectedValue
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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
            btnAdd.Enabled = False
            ViewState("Admin") = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 116 'Repair Expense Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Mike.Alonzo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                For i = 0 To dsRoleForm.Tables(0).Rows.Count - 1
                                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ViewState("Admin") = True
                                            btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            'N/A
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            'N/A
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                            'N/A
                                    End Select 'EOF of "Select Case iRoleID"
                                Next
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
#End Region 'EOF Security

    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down UGN Facility control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Project Leader control for selection criteria for search
        ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used for Project Leader
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProjectLeader.DataSource = ds
            ddProjectLeader.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddProjectLeader.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            ddProjectLeader.DataBind()
            ddProjectLeader.Items.Insert(0, "")
        End If

    End Sub 'EOF BindCriteria

    Private Sub BindData()
        Try
            lblErrors.Text = ""
            Dim ds As DataSet = New DataSet

            'bind existing AR Event data to repeater control at bottom of screen        
            ds = EXPModule.GetExpProjRepair(ViewState("sProjNo"), ViewState("sSupProjNo"), ViewState("sProjTitle"), ViewState("sUGNFacility"), IIf(ViewState("sPLDRID") = Nothing, 0, ViewState("sPLDRID")), ViewState("sDept"), ViewState("sPStat"))

            ' '', IIf(ViewState("sCatID") = Nothing, 0, ViewState("sCatID")), ViewState("sCapCls")

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    rpRepairExpProj.DataSource = ds
                    rpRepairExpProj.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpRepairExpProj.DataSource = objPds
                    rpRepairExpProj.DataBind()

                    lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                    ViewState("LastPageCount") = objPds.PageCount - 1
                    txtGoToPage.Text = CurrentPage + 1

                    ' Disable Prev or Next buttons if necessary
                    cmdFirst.Enabled = Not objPds.IsFirstPage
                    cmdPrev.Enabled = Not objPds.IsFirstPage
                    cmdNext.Enabled = Not objPds.IsLastPage
                    cmdLast.Enabled = Not objPds.IsLastPage

                    ' Display # of records
                    If (CurrentPage + 1) > 1 Then
                        lblFromRec.Text = (((CurrentPage + 1) * 30) - 30) + 1
                        lblToRec.Text = (CurrentPage + 1) * 30
                        If lblToRec.Text > objPds.DataSourceCount Then
                            lblToRec.Text = objPds.DataSourceCount
                        End If
                    Else
                        lblFromRec.Text = ds.Tables.Count
                        lblToRec.Text = rpRepairExpProj.Items.Count
                    End If
                    lblTotalRecords.Text = objPds.DataSourceCount
                End If
            End If
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF of BindData

#Region "Paging Routine"
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

    End Property 'EOF CurrentPage

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdPrev_Click

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdNext_Click

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdFirst_Click

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            If txtGoToPage.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If

                HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdGo_Click

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdLast_Click

#End Region 'EOF Paging Routine

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("RepairExpProj.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionExpCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("EXPR_ProjNo").Value = txtProjectNo.Text
            Response.Cookies("EXPR_SupProjNo").Value = txtSupProjectNo.Text
            Response.Cookies("EXPR_ProjTitle").Value = txtProjectTitle.Text
            Response.Cookies("EXPR_UGNFacility").Value = ddUGNFacility.SelectedValue
            Response.Cookies("EXPR_PLDRID").Value = ddProjectLeader.SelectedValue
            Response.Cookies("EXPR_DEPT").Value = txtDepartment.Text
            Response.Cookies("EXPR_PSTAT").Value = ddProjectStatus.SelectedValue

            ' Set viewstate variable to the first page
            CurrentPage = 0

            ' Reload control
            BindData()

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            EXPModule.DeleteRepairExpProjCookies()
            HttpContext.Current.Session("sessionExpCurrentPage") = Nothing

            Response.Redirect("RepairExpProjList.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Public Function GoToAppend(ByVal ProjectNo As String, ByVal ParentProjectNo As String) As String

        If ParentProjectNo = Nothing Or ParentProjectNo Is DBNull.Value Then
            Return "RepairExpProj.aspx?pProjNo=&pPrntProjNo=" & ProjectNo
        Else
            Return ""
        End If

    End Function 'EOF GoToAppend

    Public Function ShowHideImageAppend(ByVal ParentProjectNo As String, ByVal RoutingStatus As String) As Boolean

        If (ParentProjectNo = Nothing Or ParentProjectNo Is DBNull.Value) And (RoutingStatus = "A" Or RoutingStatus = "C") Then
            Return True
        Else
            Return False
        End If

    End Function 'EOF ShowHideImageAppend

    Public Function ShowHideHistory(ByVal ProjectStatus As String) As Boolean
        If ProjectStatus = "Open" Then
            Return False
        Else
            Return True
        End If
    End Function 'EOF ShowHideHistory

    Public Function ShowHideIORimg(ByVal ProjectNo As String) As Boolean

        Dim ds As DataSet = New DataSet
        ds = PURModule.GetInternalOrderRequestwSecurity("", "", "", 0, 0, 0, 0, "", "", 0, "", "", ProjectNo, "", "", "", 0, 0)
        If commonFunctions.CheckDataSet(ds) = True Then
            Return True
        Else
            Return False
        End If

    End Function 'EOF ShowHideIORimg
End Class
