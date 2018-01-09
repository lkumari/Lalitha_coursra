' ************************************************************************************************
' Name:	InternalOrderRequestList.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 08/23/2010    LRey			Created .Net application
' 06/28/2012    LRey            Modified this page to include Buyer and Submitted From/To filters, in addition to modify the      
'                               security so that Supervisors, Execs not only view records they initiated but the records
'                               they are/were involved for approval.
' 07/20/2012    LRey            Added functionality for IS Infrastructure to issue IOR's for other Requisitioner's
'                               Subscription ID 141 created
' 02/25/2014    LRey            Oracle iProcurement replaces this E-IOR module. Disabled the btnAdd feature. 
'                               Allow only the update/edit to complete the existing E-IOR records.
' ************************************************************************************************
Partial Class IOR_InternalOrderRequestList
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pIORNo") <> "" Then
                ViewState("pIORNo") = HttpContext.Current.Request.QueryString("pIORNo")
            Else
                ViewState("pIORNo") = ""
            End If

            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
            Else
                ViewState("pAprv") = 0
            End If


            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Internal Order Request Search"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pAprv") = 0 Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > Internal Order Request Search"
                Else
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > Internal Order Request Search > <a href='crInternalOrderRequestApproval.aspx?pIORNo=" & ViewState("pIORNo") & "&pAprv=1'><b>Approval</b></a>"
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
            ctl = m.FindControl("PURExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            'focus on Vehicle List screen Program field
            txtIORNo.Focus()

            If HttpContext.Current.Session("sessionPurCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionPurCurrentPage")
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sIORNo") = ""
                ViewState("sIDesc") = ""
                ViewState("sLoc") = ""
                ViewState("sRBID") = 0
                ViewState("sDeptID") = ""
                ViewState("sGLNo") = 0
                ViewState("sPONO") = ""
                ViewState("sVTYPE") = ""
                ViewState("sVNDNO") = 0
                ViewState("sIStat") = ""
                ViewState("sRStat") = ""
                ViewState("sCapEx") = ""
                ViewState("sBuy") = ""
                ViewState("sDSF") = ""
                ViewState("sDST") = ""
                ViewState("sSUB") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("IOR_IORNo") Is Nothing Then
                    txtIORNo.Text = Server.HtmlEncode(Request.Cookies("IOR_IORNo").Value)
                    ViewState("sIORNo") = Server.HtmlEncode(Request.Cookies("IOR_IORNo").Value)
                End If

                If Not Request.Cookies("IOR_IDesc") Is Nothing Then
                    txtIORDescription.Text = Server.HtmlEncode(Request.Cookies("IOR_IDesc").Value)
                    ViewState("sIDesc") = Server.HtmlEncode(Request.Cookies("IOR_IDesc").Value)
                End If

                If Not Request.Cookies("IOR_Loc") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("IOR_Loc").Value)
                    ViewState("sLoc") = Server.HtmlEncode(Request.Cookies("IOR_Loc").Value)
                End If

                If Not Request.Cookies("IOR_RBID") Is Nothing Then
                    ddRequestedBy.SelectedValue = Server.HtmlEncode(Request.Cookies("IOR_RBID").Value)
                    ViewState("sRBID") = Server.HtmlEncode(Request.Cookies("IOR_RBID").Value)
                End If

                If Not Request.Cookies("IOR_DeptID") Is Nothing Then
                    txtDepartment.Text = Server.HtmlEncode(Request.Cookies("IOR_DeptID").Value)
                    ViewState("sDeptID") = Server.HtmlEncode(Request.Cookies("IOR_DeptID").Value)
                End If

                If Not Request.Cookies("IOR_GLNo") Is Nothing Then
                    ddGLAccount.SelectedValue = Server.HtmlEncode(Request.Cookies("IOR_GLNo").Value)
                    ViewState("sGLNo") = Server.HtmlEncode(Request.Cookies("IOR_GLNo").Value)
                End If

                If Not Request.Cookies("IOR_PONO") Is Nothing Then
                    txtPONo.Text = Server.HtmlEncode(Request.Cookies("IOR_PONO").Value)
                    ViewState("sPONO") = Server.HtmlEncode(Request.Cookies("IOR_PONO").Value)
                End If

                If Not Request.Cookies("IOR_VTYPE") Is Nothing Then
                    ddVendorType.SelectedValue = Server.HtmlEncode(Request.Cookies("IOR_VTYPE").Value)
                    ViewState("sPONO") = Server.HtmlEncode(Request.Cookies("IOR_VTYPE").Value)
                End If

                If Not Request.Cookies("IOR_VNDNO") Is Nothing Then
                    ddVendor.SelectedValue = Server.HtmlEncode(Request.Cookies("IOR_VNDNO").Value)
                    ViewState("sVNDNO") = Server.HtmlEncode(Request.Cookies("IOR_VNDNO").Value)
                End If

                If Not Request.Cookies("IOR_IStat") Is Nothing Then
                    ddIORStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("IOR_IStat").Value)
                    ViewState("IStat") = Server.HtmlEncode(Request.Cookies("IOR_IStat").Value)
                End If

                If Not Request.Cookies("IOR_CapEx") Is Nothing Then
                    txtAppropriationCode.Text = Server.HtmlEncode(Request.Cookies("IOR_CapEx").Value)
                    ViewState("sCapEx") = Server.HtmlEncode(Request.Cookies("IOR_CapEx").Value)
                End If

                If Not Request.Cookies("IOR_BUYER") Is Nothing Then
                    ddBuyer.SelectedValue = Server.HtmlEncode(Request.Cookies("IOR_BUYER").Value)
                    ViewState("sBuy") = Server.HtmlEncode(Request.Cookies("IOR_BUYER").Value)
                End If

                If Not Request.Cookies("IOR_DSF") Is Nothing Then
                    txtDateSubFrom.Text = Server.HtmlEncode(Request.Cookies("IOR_DSF").Value)
                    ViewState("sDSF") = Server.HtmlEncode(Request.Cookies("IOR_DSF").Value)
                End If

                If Not Request.Cookies("IOR_DST") Is Nothing Then
                    txtDateSubTo.Text = Server.HtmlEncode(Request.Cookies("IOR_DST").Value)
                    ViewState("sDST") = Server.HtmlEncode(Request.Cookies("IOR_DST").Value)
                End If

                If Not Request.Cookies("IOR_SUB") Is Nothing Then
                    ddSubmittedBy.SelectedValue = Server.HtmlEncode(Request.Cookies("IOR_SUB").Value)
                    ViewState("sSUB") = Server.HtmlEncode(Request.Cookies("IOR_SUB").Value)
                End If

                ''******
                '' Bind drop down lists
                ''******
                BindData()

            Else
                ViewState("sIORNo") = txtIORNo.Text
                ViewState("sIDesc") = txtIORDescription.Text
                ViewState("sLoc") = ddUGNFacility.SelectedValue
                ViewState("sRBID") = ddRequestedBy.SelectedValue
                ViewState("sDeptID") = txtDepartment.Text
                ViewState("sGLNo") = ddGLAccount.SelectedValue
                ViewState("sPONO") = txtPONo.Text
                ViewState("sVTYPE") = ddVendorType.SelectedValue
                ViewState("sVNDNO") = ddVendor.SelectedValue
                ViewState("sIStat") = ddIORStatus.SelectedValue
                ViewState("sRStat") = ""
                ViewState("sCapEx") = txtAppropriationCode.Text
                ViewState("sBuy") = ddBuyer.SelectedValue
                ViewState("sDSF") = txtDateSubFrom.Text
                ViewState("sDST") = txtDateSubTo.Text
                ViewState("sSUB") = ddSubmittedBy.SelectedValue
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
            ' ''btnAdd.Enabled = False
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
            Dim iFormID As Integer = 109 'Internal Order Request Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
           
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Mike.Berdine", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    ViewState("iTeamMemberID") = iTeamMemberID
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member

                        ''Locate the Buyer to grant access to Purchase Order # field
                        Dim dsBuyer As DataSet = New DataSet
                        dsBuyer = commonFunctions.GetTeamMemberBySubscription(99)
                        Dim iBuyerID As Integer = 0
                        Dim b As Integer = 0
                        ViewState("iBuyerID") = 0
                        If (dsBuyer.Tables.Item(0).Rows.Count > 0) Then
                            For b = 0 To dsBuyer.Tables(0).Rows.Count - 1
                                If dsBuyer.Tables(0).Rows(b).Item("TMID") = iTeamMemberID Then
                                    iBuyerID = dsBuyer.Tables(0).Rows(b).Item("TMID")
                                    ViewState("iBuyerID") = iBuyerID
                                End If
                            Next
                        End If

                        ''Locate the IS Infrastructure
                        Dim dsIS As DataSet = New DataSet
                        dsIS = commonFunctions.GetTeamMemberBySubscription(141)
                        Dim iISINF As Integer = 0
                        Dim t As Integer = 0
                        ViewState("iISINF") = 0
                        If (dsIS.Tables.Item(0).Rows.Count > 0) Then
                            For t = 0 To dsIS.Tables(0).Rows.Count - 1
                                If dsIS.Tables(0).Rows(t).Item("TMID") = iTeamMemberID Then
                                    iISINF = dsIS.Tables(0).Rows(t).Item("TMID")
                                    ViewState("iISINF") = IIf(iISINF <> 204, iISINF, 0)
                                End If
                            Next
                        End If

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
                                            ' ''btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ' ''btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
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
        Try
            Dim ds As DataSet = New DataSet
            ''bind existing data to drop down Requested By control for selection criteria for search
            ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used for Project Leader
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddRequestedBy.DataSource = ds
                ddRequestedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddRequestedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddRequestedBy.DataBind()
                ddRequestedBy.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Ship To control for selection criteria for search
            ds = PURModule.GetIORUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Ship To Attention control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(99)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddBuyer.DataSource = ds
                ddBuyer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddBuyer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddBuyer.DataBind()
                ddBuyer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Department or Cost Center control for selection criteria for search
            'ds = commonFunctions.GetDepartmentGLNo("")
            'If (ds.Tables.Item(0).Rows.Count > 0) Then
            '    ddDepartment.DataSource = ds
            '    ddDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName.ToString()
            '    ddDepartment.DataValueField = ds.Tables(0).Columns("GLNo").ColumnName.ToString()
            '    ddDepartment.DataBind()
            '    ddDepartment.Items.Insert(0, "")
            'End If

            ''bind existing data to drop down GLAccounts or Cost Center control for selection criteria for search
            ds = commonFunctions.GetGLAccounts("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddGLAccount.DataSource = ds
                ddGLAccount.DataTextField = ds.Tables(0).Columns("ddGLAccountName").ColumnName.ToString()
                ddGLAccount.DataValueField = ds.Tables(0).Columns("GLNo").ColumnName.ToString()
                ddGLAccount.DataBind()
                ddGLAccount.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Vendor Type control for selection criteria for search
            ds = commonFunctions.GetVendorType(False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendorType.DataSource = ds
                ddVendorType.DataTextField = ds.Tables(0).Columns("ddVType").ColumnName.ToString()
                ddVendorType.DataValueField = ds.Tables(0).Columns("VType").ColumnName.ToString()
                ddVendorType.DataBind()
                ddVendorType.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Vendor control for selection criteria for search
            ds = commonFunctions.GetVendor(0, "", "", "", "", "", "", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendor.DataSource = ds
                ddVendor.DataTextField = ds.Tables(0).Columns("ddVNDNAMcombo").ColumnName.ToString()
                ddVendor.DataValueField = ds.Tables(0).Columns("VENDOR").ColumnName.ToString()
                ddVendor.DataBind()
                ddVendor.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Submitted By control for selection criteria for search
            ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used for Project Leader
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSubmittedBy.DataSource = ds
                ddSubmittedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddSubmittedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddSubmittedBy.DataBind()
                ddSubmittedBy.Items.Insert(0, "")
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
    End Sub 'EOF BindCriteria

    Private Sub BindData()

        Try
            lblErrors.Text = ""

            Dim ds As DataSet = New DataSet
            Dim DefaultUserLocation As String = Nothing
            Dim DefaultTMID As Integer = ViewState("iTeamMemberID")
            Dim SubscriptionID As Integer = 0
            Dim i As Integer = 0
            Dim sid As Integer = 0

            'get current team member's facility locationDefaultTMID
            ds = PURModule.GetTeamMemberLocation(DefaultTMID)
            If commonFunctions.CheckDataSet(ds) = True Then
                DefaultUserLocation = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
            End If

            'make sure team member is authorized IOR userDefaultTMID
            ds = commonFunctions.GetWorkFlow(DefaultTMID, 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    sid = ds.Tables(0).Rows(i).Item("SubscriptionID")
                    Select Case sid
                        Case 98
                            SubscriptionID = 98
                        Case 99
                            SubscriptionID = 99
                            DefaultTMID = 0
                        Case 102
                            SubscriptionID = 102
                            DefaultTMID = 0
                        Case 101
                            SubscriptionID = 101
                            DefaultTMID = 0
                        Case 100
                            SubscriptionID = 100
                            DefaultTMID = 0
                    End Select
                Next
            End If

            If DefaultTMID = 204 Or ViewState("iBuyerID") <> 0 Or ViewState("iISINF") <> 0 Then ''grant full view access to LRey
                ViewState("Admin") = True
            End If

            If ViewState("Admin") = True Then
                DefaultTMID = 0
            End If

            'bind data to repeater for Buyer's, exec's or current team member view only                      
            ds = PURModule.GetInternalOrderRequestwSecurity(ViewState("sIORNo"), ViewState("sIDesc"), ViewState("sLoc"), IIf(ViewState("sRBID") = Nothing, IIf(ViewState("iISINF") = 0, IIf(ViewState("Admin") = True, 0, DefaultTMID), ViewState("iISINF")), ViewState("sRBID")), IIf(ViewState("sBuy") = Nothing, 0, ViewState("sBuy")), ViewState("sDeptID"), IIf(ViewState("sGLNo") = Nothing, 0, ViewState("sGLNo")), ViewState("sPONO"), ViewState("sVTYPE"), IIf(ViewState("sVNDNO") = Nothing, 0, ViewState("sVNDNO")), ViewState("sIStat"), ViewState("sRStat"), ViewState("sCapEx"), txtDateSubFrom.Text, txtDateSubTo.Text, "", IIf(ViewState("iISINF") = 0, 0, 141), IIf(ViewState("sSUB") = Nothing, IIf(ViewState("iISINF") = 0, DefaultTMID, ViewState("iISINF")), ViewState("sSUB")))

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    rpIOR.DataSource = ds
                    rpIOR.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpIOR.DataSource = objPds
                    rpIOR.DataBind()

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
                        lblToRec.Text = rpIOR.Items.Count
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
            HttpContext.Current.Session("sessionPurCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionPurCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionPurCurrentPage") = CurrentPage

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


                HttpContext.Current.Session("sessionPurCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionPurCurrentPage") = CurrentPage

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

    ' ''Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    ' ''    Response.Redirect("InternalOrderRequest.aspx", False)
    ' ''End Sub 'EOF btnAdd_Click

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionPurCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("IOR_IORNo").Value = txtIORNo.Text
            Response.Cookies("IOR_IDesc").Value = txtIORDescription.Text
            Response.Cookies("IOR_Loc").Value = ddUGNFacility.SelectedValue
            Response.Cookies("IOR_RBID").Value = ddRequestedBy.SelectedValue
            Response.Cookies("IOR_DeptID").Value = txtDepartment.Text
            Response.Cookies("IOR_GLNo").Value = ddGLAccount.SelectedValue
            Response.Cookies("IOR_PONO").Value = txtPONo.Text
            Response.Cookies("IOR_VTYPE").Value = ddVendorType.SelectedValue
            Response.Cookies("IOR_VNDNO").Value = ddVendor.SelectedValue
            Response.Cookies("IOR_IStat").Value = ddIORStatus.SelectedValue
            Response.Cookies("IOR_CapEx").Value = txtAppropriationCode.Text
            Response.Cookies("IOR_BUYER").Value = ddBuyer.SelectedValue
            Response.Cookies("IOR_DSF").Value = txtDateSubFrom.Text
            Response.Cookies("IOR_DST").Value = txtDateSubTo.Text
            Response.Cookies("IOR_SUB").Value = ddSubmittedBy.SelectedValue

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
            PURModule.DeleteInternalOrderRequestCookies()
            HttpContext.Current.Session("sessionPurCurrentPage") = Nothing

            Response.Redirect("InternalOrderRequestList.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Public Function ShowHideHistory(ByVal ProjectStatus As String) As Boolean
        If ProjectStatus = "Open" Then
            Return False
        Else
            Return True
        End If
    End Function 'EOF ShowHideHistory

    Public Function ShowHidePONo(ByVal ProjectStatus As String, ByVal RequestetdByTMID As Integer) As Boolean
        If (ProjectStatus = "Approved") And RequestetdByTMID = ViewState("iTeamMemberID") Then
            Return False
        Else
            Return True
        End If
    End Function 'EOF ShowHidePONo

    Public Function GoToCapEx(ByVal IORNO As String, ByVal ProjNo As String) As String
        Dim ds2 As DataSet = New DataSet
        Dim strReturnValue As String = ""

        ds2 = PURModule.GetInternalOrderRequestCapEx(0, ProjNo)
        If commonFunctions.CheckDataSet(ds2) = True Then
            If Not IsDBNull(ds2.Tables(0).Rows(0).Item("DefinedCapEx")) Then
                If (ProjNo <> Nothing Or ProjNo <> "") And (ds2.Tables(0).Rows(0).Item("ProjectTitle") <> Nothing) Then
                    Select Case ProjNo.Substring(0, 1)
                        Case "A"
                            strReturnValue = "~/EXP/crViewExpProjAssets.aspx?pProjNo=" & ProjNo
                        Case "D"
                            strReturnValue = "~/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & ProjNo
                        Case "P"
                            strReturnValue = "~/EXP/crViewExpProjPackaging.aspx?pProjNo=" & ProjNo
                        Case "R"
                            strReturnValue = "~/EXP/crViewExpProjRepair.aspx?pProjNo=" & ProjNo
                        Case "T"
                            strReturnValue = "~/EXP/crViewExpProjTooling.aspx?pProjNo=" & ProjNo
                    End Select
                End If
            End If
        End If
        GoToCapEx = strReturnValue

    End Function 'EOF GoToCapEx
    Protected Function SetCapExForeColor(ByVal IORNO As String, ByVal ProjNo As String) As Color

        Dim strReturnValue As Color = Color.Black
        Dim ds2 As DataSet = New DataSet

        ds2 = PURModule.GetInternalOrderRequestCapEx(0, ProjNo)
        If commonFunctions.CheckDataSet(ds2) = True Then
            If Not IsDBNull(ds2.Tables(0).Rows(0).Item("DefinedCapEx")) Then
                If (ProjNo <> Nothing Or ProjNo <> "") And (ds2.Tables(0).Rows(0).Item("ProjectTitle") <> Nothing) Then
                    Select Case ProjNo.Substring(0, 1)
                        Case "A"
                            strReturnValue = Color.Blue
                        Case "D"
                            strReturnValue = Color.Blue
                        Case "P"
                            strReturnValue = Color.Blue
                        Case "R"
                            strReturnValue = Color.Blue
                        Case "T"
                            strReturnValue = Color.Blue
                    End Select
                End If
            End If
        End If

        SetCapExForeColor = strReturnValue

    End Function 'EOF SetCapExForeColor

    Protected Function SetCapExFontUnderline(ByVal IORNO As String, ByVal ProjNo As String) As Boolean

        Dim strReturnValue As Boolean = False
        Dim ds2 As DataSet = New DataSet

        ds2 = PURModule.GetInternalOrderRequestCapEx(0, ProjNo)
        If commonFunctions.CheckDataSet(ds2) = True Then
            If Not IsDBNull(ds2.Tables(0).Rows(0).Item("DefinedCapEx")) Then
                If (ProjNo <> Nothing Or ProjNo <> "") And (ds2.Tables(0).Rows(0).Item("ProjectTitle") <> Nothing) Then
                    Select Case ProjNo.Substring(0, 1)
                        Case "A"
                            strReturnValue = True
                        Case "D"
                            strReturnValue = True
                        Case "P"
                            strReturnValue = True
                        Case "R"
                            strReturnValue = True
                        Case "T"
                            strReturnValue = True
                    End Select
                End If
            End If
        End If

        SetCapExFontUnderline = strReturnValue

    End Function 'EOF SetCapExFontUnderline

    Protected Function SetTextColor(ByVal RoutingStatus As String) As Color

        Dim strReturnValue As Color = Color.Black

        Select Case RoutingStatus
            Case "A"
                strReturnValue = Color.Black
            Case "C"
                strReturnValue = Color.Black
            Case "N"
                strReturnValue = Color.Black
            Case "T"
                strReturnValue = Color.Black
            Case "R"
                strReturnValue = Color.White
            Case "V"
                strReturnValue = Color.White
        End Select

        SetTextColor = strReturnValue

    End Function 'EOF SetTextColor

    Protected Function SetBackGroundColor(ByVal RoutingStatus As String, ByVal IORStatus As String) As String

        Dim strReturnValue As String = "White"

        Select Case RoutingStatus
            Case "A"
                If IORStatus = "Approved" Then
                    strReturnValue = "Lime"
                ElseIf IORStatus = "Completed" Then
                    strReturnValue = "Aqua"
                End If
            Case "C"
                strReturnValue = "White'"
            Case "N"
                strReturnValue = "Fuchsia"
            Case "T"
                strReturnValue = "Yellow"
            Case "R"
                strReturnValue = "Red"
            Case "V"
                strReturnValue = "Gray"
        End Select

        SetBackGroundColor = strReturnValue

    End Function 'EOF SetBackGroundColor

End Class
