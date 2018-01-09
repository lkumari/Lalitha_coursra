' ************************************************************************************************
' Name:	ToolingExpProjList.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 06/04/2009   LRey			Created .Net application
' 06/21/2012    LRey            Added "Record Listed" to the search page.
' 10/29/2012    LRey            Added an image button to IOR's if submitted.
' ************************************************************************************************
Partial Class EXP_ToolingExpProjList
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = ""
            End If

            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
            Else
                ViewState("pAprv") = 0
            End If

            If HttpContext.Current.Request.QueryString("pPrntProjNo") <> "" Then
                txtLastSupplementNo.Text = HttpContext.Current.Request.QueryString("pPrntProjNo")
            End If


            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Customer Owned Tooling Search"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pAprv") = 0 Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > Customer Owned Tooling Search"
                Else
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Spending Requests</b> > Customer Owned Tooling Search > <a href='crExpProjToolingApproval.aspx?pProjNo=" & ViewState("pProjNo") & "&pAprv=1'><b>Approval</b></a>"
                End If
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



            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sProjNo") = ""
                ViewState("sSupProjNo") = ""
                ViewState("sProjTitle") = ""
                ViewState("sUGNFacility") = ""
                ViewState("sCABBV") = ""
                'ViewState("sSoldTo") = 0
                ViewState("sProgramID") = ""
                ViewState("sAMGRID") = ""
                ViewState("sPMID") = ""
                ViewState("sTLID") = ""
                ViewState("sPLID") = ""
                ViewState("sProjType") = ""
                ViewState("sPartNo") = ""
                ViewState("sPartDesc") = ""
                ViewState("sProjStatus") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("EXPT_ProjNo") Is Nothing Then
                    txtProjectNo.Text = Server.HtmlEncode(Request.Cookies("EXPT_ProjNo").Value)
                    ViewState("sProjNo") = Server.HtmlEncode(Request.Cookies("EXPT_ProjNo").Value)
                End If


                If Not Request.Cookies("EXPT_SupProjNo") Is Nothing Then
                    txtSupProjectNo.Text = Server.HtmlEncode(Request.Cookies("EXPT_SupProjNo").Value)
                    ViewState("sSupProjNo") = Server.HtmlEncode(Request.Cookies("EXPT_SupProjNo").Value)
                End If

                If Not Request.Cookies("EXPT_ProjTitle") Is Nothing Then
                    txtProjectTitle.Text = Server.HtmlEncode(Request.Cookies("EXPT_ProjTitle").Value)
                    ViewState("sProjTitle") = Server.HtmlEncode(Request.Cookies("EXPT_ProjTitle").Value)
                End If

                If Not Request.Cookies("EXPT_UGNFacility") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_UGNFacility").Value)
                    ViewState("sUGNFacility") = Server.HtmlEncode(Request.Cookies("EXPT_UGNFacility").Value)
                End If

                If (Not Request.Cookies("EXPT_CABBV") Is Nothing) And (Not Request.Cookies("EXPT_SoldTo") Is Nothing) Then
                    'ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_SoldTo").Value) & "|" & Server.HtmlEncode(Request.Cookies("EXPT_CABBV").Value)
                    ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_CABBV").Value)
                    ViewState("sCABBV") = Server.HtmlEncode(Request.Cookies("EXPT_CABBV").Value)
                    'ViewState("sSoldTo") = Server.HtmlEncode(Request.Cookies("EXPT_SoldTo").Value)
                End If

                If Not Request.Cookies("EXPT_Program") Is Nothing Then
                    ddProgram.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_Program").Value)
                    ViewState("sProgramID") = Server.HtmlEncode(Request.Cookies("EXPT_Program").Value)
                End If

                If Not Request.Cookies("EXPT_AMGRID") Is Nothing Then
                    ddAccountManager.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_AMGRID").Value)
                    ViewState("sAMGRID") = Server.HtmlEncode(Request.Cookies("EXPT_AMGRID").Value)
                End If

                If Not Request.Cookies("EXPT_PMID") Is Nothing Then
                    ddProgramManager.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_PMID").Value)
                    ViewState("sPMID") = Server.HtmlEncode(Request.Cookies("EXPT_PMID").Value)
                End If

                If Not Request.Cookies("EXPT_TLID") Is Nothing Then
                    ddToolingLead.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_TLID").Value)
                    ViewState("sTLID") = Server.HtmlEncode(Request.Cookies("EXPT_TLID").Value)
                End If

                If Not Request.Cookies("EXPT_PLID") Is Nothing Then
                    ddPurchasingLead.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_PLID").Value)
                    ViewState("sPLID") = Server.HtmlEncode(Request.Cookies("EXPT_PLID").Value)
                End If

                If Not Request.Cookies("EXPT_ProjType") Is Nothing Then
                    ddProjectType.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_ProjType").Value)
                    ViewState("sProjType") = Server.HtmlEncode(Request.Cookies("EXPT_ProjType").Value)
                End If

                If Not Request.Cookies("EXPT_PartNo") Is Nothing Then
                    txtPartNo.Text = Server.HtmlEncode(Request.Cookies("EXPT_PartNo").Value)
                    ViewState("sPartNo") = Server.HtmlEncode(Request.Cookies("EXPT_PartNo").Value)
                End If

                If Not Request.Cookies("EXPT_PartDesc") Is Nothing Then
                    txtPartDesc.Text = Server.HtmlEncode(Request.Cookies("EXPT_PartDesc").Value)
                    ViewState("sPartDesc") = Server.HtmlEncode(Request.Cookies("EXPT_PartDesc").Value)
                End If

                If Not Request.Cookies("EXPT_ProjStatus") Is Nothing Then
                    ddProjectStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("EXPT_ProjStatus").Value)
                    ViewState("sProjStatus") = Server.HtmlEncode(Request.Cookies("EXPT_ProjStatus").Value)
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
                'Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                'If Not (Pos = 0) Then
                '    ViewState("sCABBV") = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                '    ViewState("sSoldTo") = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
                'End If
                ViewState("sCABBV") = ddCustomer.SelectedValue
                ViewState("sProgramID") = ddProgram.SelectedValue
                ViewState("sAMGRID") = ddAccountManager.SelectedValue
                ViewState("sPMID") = ddProgramManager.SelectedValue
                ViewState("sTLID") = ddToolingLead.SelectedValue
                ViewState("sPLID") = ddPurchasingLead.SelectedValue
                ViewState("sProjType") = ddProjectType.SelectedValue
                ViewState("sPartNo") = txtPartNo.Text
                ViewState("sPartDesc") = txtPartDesc.Text
                ViewState("sProjStatus") = ddProjectStatus.SelectedValue
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
            txtLastSupplementNo.Enabled = False
            btnGo.Enabled = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 82 'Expensed Project Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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
                                    iRoleID = dsRoleForm.Tables(0).Rows(i).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ''Used by full admin such as Developers
                                            ViewState("Admin") = True
                                            btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
                                            txtLastSupplementNo.Enabled = True
                                            btnGo.Enabled = True
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ViewState("Admin") = True
                                            btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
                                            txtLastSupplementNo.Enabled = True
                                            btnGo.Enabled = True

                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Backup persons
                                            'N/A
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            'N/A
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
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

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = commonFunctions.GetOEMManufacturer("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddCustomer.DataSource = ds
            ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            ddCustomer.DataBind()
            ddCustomer.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Program control for selection criteria for search
        ds = commonFunctions.GetPlatformProgram(0, 0, "", "", "")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProgram.DataSource = ds
            ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramModelPlatformAssembly").ColumnName.ToString()
            ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
            ddProgram.DataBind()
            ddProgram.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Account Manager control for selection criteria for search
        ds = commonFunctions.GetTeamMemberBySubscription(9) '**SubscriptionID 9 is used for Account Manager
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddAccountManager.DataSource = ds
            ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
            ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
            ddAccountManager.DataBind()
            ddAccountManager.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Program Manager control for selection criteria for search
        ds = commonFunctions.GetTeamMemberBySubscription(31) '**SubscriptionID 31 is used for Program Manager
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProgramManager.DataSource = ds
            ddProgramManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
            ddProgramManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
            ddProgramManager.DataBind()
            ddProgramManager.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Tooling Lead control for selection criteria for search
        ds = commonFunctions.GetTeamMemberBySubscription(65) '**SubscriptionID 9 is used for Tooling Lead
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddToolingLead.DataSource = ds
            ddToolingLead.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
            ddToolingLead.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
            ddToolingLead.DataBind()
            ddToolingLead.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Purchasing Lead control for selection criteria for search
        ds = commonFunctions.GetTeamMemberBySubscription(7) '**SubscriptionID 7 is used for Purchasing Lead
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddPurchasingLead.DataSource = ds
            ddPurchasingLead.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
            ddPurchasingLead.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
            ddPurchasingLead.DataBind()
            ddPurchasingLead.Items.Insert(0, "")
        End If

    End Sub 'EOF BindCriteria

    Private Sub BindData()

        Try
            lblErrors.Text = ""

            Dim ds As DataSet = New DataSet

            'bind existing AR Event data to repeater control at bottom of screen                       
            ds = EXPModule.GetExpProjTooling(ViewState("sProjNo"), ViewState("sSupProjNo"), ViewState("sProjTitle"), ViewState("sUGNFacility"), ViewState("sCABBV"), IIf(ViewState("sProgramID") = Nothing, 0, ViewState("sProgramID")), IIf(ViewState("sAMGRID") = Nothing, 0, ViewState("sAMGRID")), IIf(ViewState("sPMID") = Nothing, 0, ViewState("sPMID")), IIf(ViewState("sTLID") = Nothing, 0, ViewState("sTLID")), IIf(ViewState("sPLID") = Nothing, 0, ViewState("sPLID")), ViewState("sProjType"), ViewState("sPartNo"), ViewState("sPartDesc"), ViewState("sProjStatus")) '

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    rpToolingExpProj.DataSource = ds
                    rpToolingExpProj.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpToolingExpProj.DataSource = objPds
                    rpToolingExpProj.DataBind()

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
                        lblToRec.Text = rpToolingExpProj.Items.Count
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
        Response.Redirect("ToolingExpProj.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionExpCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("EXPT_ProjNo").Value = txtProjectNo.Text
            Response.Cookies("EXPT_SupProjNo").Value = txtSupProjectNo.Text
            Response.Cookies("EXPT_ProjTitle").Value = txtProjectTitle.Text
            Response.Cookies("EXPT_UGNFacility").Value = ddUGNFacility.SelectedValue
            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            If Not (Pos = 0) Then
                Response.Cookies("EXPT_CABBV").Value = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                Response.Cookies("EXPT_SoldTo").Value = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If
            Response.Cookies("EXPT_Program").Value = ddProgram.SelectedValue
            Response.Cookies("EXPT_AMGRID").Value = ddAccountManager.SelectedValue
            Response.Cookies("EXPT_PMID").Value = ddProgramManager.SelectedValue
            Response.Cookies("EXPT_TLID").Value = ddToolingLead.SelectedValue
            Response.Cookies("EXPT_PLID").Value = ddPurchasingLead.SelectedValue
            Response.Cookies("EXPT_ProjType").Value = ddProjectType.SelectedValue
            Response.Cookies("EXPT_PartNo").Value = txtPartNo.Text
            Response.Cookies("EXPT_PartDesc").Value = txtPartDesc.Text
            Response.Cookies("EXPT_ProjStatus").Value = ddProjectStatus.SelectedValue

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
            EXPModule.DeleteToolingExpProjCookies()
            HttpContext.Current.Session("sessionExpCurrentPage") = Nothing

            Response.Redirect("ToolingExpProjList.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Public Function GoToAppend(ByVal ProjectNo As String, ByVal ParentProjectNo As String, ByVal CarriedOver As Boolean) As String
        If ParentProjectNo = Nothing Or ParentProjectNo Is DBNull.Value Then
            Return "ToolingExpProj.aspx?pProjNo=&pPrntProjNo=" & ProjectNo
        Else
            If CarriedOver = True Then
                Return "ToolingExpProj.aspx?pProjNo=&pPrntProjNo=" & ParentProjectNo & "&pCO=1"
            Else
                Return ""
            End If
        End If
    End Function 'EOF GoToAppend

    Public Function ShowHideImageAppend(ByVal ParentProjectNo As String, ByVal RoutingStatus As String, ByVal CarriedOver As Boolean) As Boolean

        If ((ParentProjectNo = Nothing Or ParentProjectNo Is DBNull.Value)) And (RoutingStatus <> "N" And RoutingStatus <> "S" And RoutingStatus <> "T") Then
            Return True
        Else
            If (CarriedOver = True) And (RoutingStatus <> "N" And RoutingStatus <> "S" And RoutingStatus <> "T") Then
                Return True
            Else
                Return False
            End If
        End If

    End Function 'EOF ShowHideImageAppend

    Public Function ShowHideHistory(ByVal ProjectStatus As String) As Boolean
        'If ProjectStatus = "Open" Then
        '    Return False
        'Else
        Return True
        'End If
    End Function 'EOF ShowHideHistory

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Try
            lblErrors.Text = ""
            Dim ds As DataSet = New DataSet
            Dim ParentProjectNo As String = Nothing
            Dim ProjectNo As String = txtLastSupplementNo.Text
            Dim ExpProject As String = "Tooling"
            Dim UGNFacility As String = Nothing
            Dim ProjectTitle As String = Nothing
            Dim ProjectType As String = Nothing
            Dim DefaultDate As Date = Date.Today
            Dim OriginalCEAApprovedDt As String = Nothing
            Dim AccountMgrTMID As Integer = Nothing
            Dim PrgmMgrTMID As Integer = Nothing
            Dim ToolLeadTMID As Integer = Nothing
            Dim PurchLeadTMID As Integer = Nothing
            Dim RoutingFlag As String = Nothing
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''***********************************************************************
            ''Validate Primary/Supplement TE exists in UGN_Database CapitalExpenditure table
            ''***********************************************************************
            ds = EXPModule.GetExpProjToolingLastSupplementNo(ProjectNo)

            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ParentProjectNo = IIf(ds.Tables(0).Rows(0).Item("ParentProjectNumber").ToString() = Nothing, ProjectNo, ds.Tables(0).Rows(0).Item("ParentProjectNumber").ToString())
                UGNFacility = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                ProjectTitle = ds.Tables(0).Rows(0).Item("ProjectTitle").ToString()
                ProjectType = ds.Tables(0).Rows(0).Item("ProjectType").ToString()
                OriginalCEAApprovedDt = ds.Tables(0).Rows(0).Item("OriginalCEAApprovedDt").ToString()
                AccountMgrTMID = ds.Tables(0).Rows(0).Item("AccountMgrTMID").ToString()
                PrgmMgrTMID = ds.Tables(0).Rows(0).Item("PrgmMgrTMID").ToString()
                ToolLeadTMID = ds.Tables(0).Rows(0).Item("ToolLeadTMID").ToString()
                PurchLeadTMID = ds.Tables(0).Rows(0).Item("PurchLeadTMID").ToString()
                RoutingFlag = ds.Tables(0).Rows(0).Item("RoutingFlag").ToString()

                If RoutingFlag = "A" Then
                    'allow carryover of records that have been only approved

                    ''***************
                    ''Get next SeqNo
                    ''***************
                    Dim ds2 As DataSet = Nothing
                    ds2 = EXPModule.GetUGNDatabaseNextProjNo(ParentProjectNo.ToUpper, ProjectNo.ToUpper, ExpProject, UGNFacility)
                    ViewState("pProjNo") = CType(ds2.Tables(0).Rows(0).Item("NextAvailProjNo").ToString, String)

                    '***************
                    '* Save Data
                    '***************
                    EXPModule.InsertExpProjTooling(ViewState("pProjNo"), ParentProjectNo.ToUpper, ProjectTitle, "Open", ProjectType, UGNFacility, AccountMgrTMID, PrgmMgrTMID, ToolLeadTMID, PurchLeadTMID, "", DefaultDate, "", "", "", "", 0, OriginalCEAApprovedDt, 0, DefaultUser, DefaultDate, True)

                    ''*****************
                    ''History Tracking
                    ''*****************
                    EXPModule.InsertExpProjToolingHistory(ViewState("pProjNo"), ProjectTitle, DefaultTMID, "Record created.")

                    ''****************************************
                    ''Redirect to new Project Number
                    ''****************************************
                    Response.Redirect("ToolingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pPrntProjNo=" & ParentProjectNo & "&pLS=1", False)
                Else
                    lblErrors.Text = "Unable to process request. " & ProjectNo & " is or has a series of records pending in TE Tracking system. Please review."
                    lblErrors.Visible = True
                End If
            Else
                lblErrors.Text = "Unable to process request. " & ProjectNo & " not in TE Tracking system. Please Try Again."
                lblErrors.Visible = True
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

        'NOTE: The system will default required values found in UGN_Database 
        '      CapitalExpenditire table on the web form.
    End Sub 'EOF btnGo_Click

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