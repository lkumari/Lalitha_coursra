' ************************************************************************************************
' Name:	AR_Deduction_List.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 04/10/2012    LRey			Created .Net application
' 06/21/2012    LRey            Added "Record Listed" to the search page.
' 12/20/2013    LRey            Replaced Customer DDL to OEMManufacturer.
' ************************************************************************************************
Partial Class AR_Deduction_List
    Inherits System.Web.UI.Page

    Protected WithEvents lnkRecStatus As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRecNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRsnDeduct As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkUGNFac As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRefNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDeductAmt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDateSub As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDaysOld As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkClosedDate As System.Web.UI.WebControls.LinkButton

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Operations Deduction Form Search"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable</b> > Operations Deduction Form Search"
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
            ctl = m.FindControl("ARExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            'focus on Vehicle List screen Program field
            txtARDID.Focus()

            If HttpContext.Current.Session("sessionExpCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionExpCurrentPage")
            End If

            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pARDID") <> "" Then
                ViewState("pARDID") = HttpContext.Current.Request.QueryString("pARDID")
            Else
                ViewState("pARDID") = ""
            End If
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
                HyperLink1.NavigateUrl = "~/AR/crARDeductionApproval.aspx?pARDID=" & ViewState("pARDID") & "&pAprv=1"
            Else
                ViewState("pAprv") = 0
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sARDID") = ""
                ViewState("sREFNO") = ""
                ViewState("sSBTMID") = 0
                ViewState("sDCOM") = ""
                ViewState("sDUFAC") = ""
                ViewState("sDCUST") = ""
                ViewState("sDSF") = ""
                ViewState("sDST") = ""
                ViewState("sDRSTS") = ""
                ViewState("sDRSN") = 0
                ViewState("sCDF") = ""
                ViewState("sCDT") = ""
                ViewState("sSB") = ""
                ViewState("sPNO") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("AR_ARDID") Is Nothing Then
                    txtARDID.Text = Server.HtmlEncode(Request.Cookies("AR_ARDID").Value)
                    ViewState("sARDID") = Server.HtmlEncode(Request.Cookies("AR_ARDID").Value)
                End If

                If Not Request.Cookies("AR_DREFNO") Is Nothing Then
                    txtReferenceNo.Text = Server.HtmlEncode(Request.Cookies("AR_DREFNO").Value)
                    ViewState("sREFNO") = Server.HtmlEncode(Request.Cookies("AR_DREFNO").Value)
                End If

                If Not Request.Cookies("AR_SBTMID") Is Nothing Then
                    ddSubmittedBy.SelectedValue = Server.HtmlEncode(Request.Cookies("AR_SBTMID").Value)
                    ViewState("sSBTMID") = Server.HtmlEncode(Request.Cookies("AR_SBTMID").Value)
                End If

                If Not Request.Cookies("AR_DCOM") Is Nothing Then
                    txtComments.Text = Server.HtmlEncode(Request.Cookies("AR_DCOM").Value)
                    ViewState("sDCOM") = Server.HtmlEncode(Request.Cookies("AR_DCOM").Value)
                End If

                If Not Request.Cookies("AR_DUFAC") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("AR_DUFAC").Value)
                    ViewState("sDUFAC") = Server.HtmlEncode(Request.Cookies("AR_DUFAC").Value)
                End If

                If (Not Request.Cookies("AR_DCUST") Is Nothing) Then
                    ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("AR_DCUST").Value)
                    ViewState("sDCUST") = Server.HtmlEncode(Request.Cookies("AR_DCUST").Value)
                End If

                If Not Request.Cookies("AR_DSF") Is Nothing Then
                    txtDateSubFrom.Text = Server.HtmlEncode(Request.Cookies("AR_DSF").Value)
                    ViewState("sDSF") = Server.HtmlEncode(Request.Cookies("AR_DSF").Value)
                End If

                If Not Request.Cookies("AR_DST") Is Nothing Then
                    txtDateSubTo.Text = Server.HtmlEncode(Request.Cookies("AR_DST").Value)
                    ViewState("sDST") = Server.HtmlEncode(Request.Cookies("AR_DST").Value)
                End If

                If Not Request.Cookies("AR_DRSTS") Is Nothing Then
                    ddRecStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("AR_DRSTS").Value)
                    ViewState("sDRSTS") = Server.HtmlEncode(Request.Cookies("AR_DRSTS").Value)
                End If

                If Not Request.Cookies("AR_DRSN") Is Nothing Then
                    ddReason.SelectedValue = Server.HtmlEncode(Request.Cookies("AR_DRSN").Value)
                    ViewState("sDRSN") = Server.HtmlEncode(Request.Cookies("AR_DRSN").Value)
                End If

                If Not Request.Cookies("AR_CDF") Is Nothing Then
                    txtClosedDateFrom.Text = Server.HtmlEncode(Request.Cookies("AR_CDF").Value)
                    ViewState("sCDF") = Server.HtmlEncode(Request.Cookies("AR_CDF").Value)
                End If

                If Not Request.Cookies("AR_CDT") Is Nothing Then
                    txtClosedDateTo.Text = Server.HtmlEncode(Request.Cookies("AR_CDT").Value)
                    ViewState("sCDT") = Server.HtmlEncode(Request.Cookies("AR_CDT").Value)
                End If

                If Not Request.Cookies("AR_SB") Is Nothing Then
                    ddSortBy.SelectedValue = Server.HtmlEncode(Request.Cookies("AR_SB").Value)
                    ViewState("sSB") = Server.HtmlEncode(Request.Cookies("AR_SB").Value)
                End If

                If Not Request.Cookies("AR_PNO") Is Nothing Then
                    txtPartNo.Text = Server.HtmlEncode(Request.Cookies("AR_PNO").Value)
                    ViewState("sPNO") = Server.HtmlEncode(Request.Cookies("AR_PNO").Value)
                End If

                ''******
                '' Bind drop down lists
                ''******
                BindData()
                'lblErrors.Text = String.Format("{0:dddd", DateTime.Now)
                'lblErrors.Visible = True

            Else
                ViewState("sARDID") = txtARDID.Text
                ViewState("sREFNO") = txtReferenceNo.Text
                ViewState("sSBTMID") = IIf(ddSubmittedBy.SelectedValue = Nothing, 0, ddSubmittedBy.SelectedValue)
                ViewState("sDCOM") = txtComments.Text
                ViewState("sDUFAC") = ddUGNFacility.SelectedValue
                ViewState("sDCUST") = ddCustomer.SelectedValue
                ViewState("sDSF") = txtDateSubFrom.Text
                ViewState("sDST") = txtDateSubTo.Text
                ViewState("sDRSTS") = ddRecStatus.SelectedValue
                ViewState("sDRSN") = IIf(ddReason.SelectedValue = Nothing, 0, ddReason.SelectedValue)
                ViewState("sCDF") = txtClosedDateFrom.Text
                ViewState("sCDT") = txtClosedDateTo.Text
                ViewState("sSB") = ddSortBy.SelectedValue
                ViewState("sPNO") = txtPartNo.Text
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
            Dim iFormID As Integer = 132 'Operations Deduction Form ID
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
            ddSubmittedBy.DataSource = ds
            ddSubmittedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddSubmittedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            ddSubmittedBy.DataBind()
            ddSubmittedBy.Items.Insert(0, "")
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

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = ARGroupModule.GetARDeductionReason("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddReason.DataSource = ds
            ddReason.DataTextField = ds.Tables(0).Columns("ddReasonDesc").ColumnName.ToString()
            ddReason.DataValueField = ds.Tables(0).Columns("RID").ColumnName.ToString()
            ddReason.DataBind()
            ddReason.Items.Insert(0, "")
        End If
    End Sub 'EOF BindCriteria

    Private Sub BindData()
        Try
            lblErrors.Text = ""
            Dim ds As DataSet = New DataSet

            'bind existing AR Event data to repeater control at bottom of screen        
            ds = ARGroupModule.GetARDeduction(ViewState("sARDID"), ViewState("sREFNO"), IIf(ViewState("sSBTMID") = Nothing, 0, ViewState("sSBTMID")), ViewState("sDCOM"), ViewState("sDUFAC"), ViewState("sDCUST"), ViewState("sDSF"), ViewState("sDST"), ViewState("sDRSTS"), IIf(ViewState("sDRSN") = Nothing, 0, ViewState("sDRSN")), ViewState("sCDF"), ViewState("sCDT"), ViewState("sSB"), ViewState("sPNO"))

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    rpARDeduction.DataSource = ds
                    rpARDeduction.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpARDeduction.DataSource = objPds
                    rpARDeduction.DataBind()

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
                        lblToRec.Text = rpARDeduction.Items.Count
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

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            ds = ARGroupModule.GetARDeduction(ViewState("sARDID"), ViewState("sREFNO"), IIf(ViewState("sSBTMID") = Nothing, 0, ViewState("sSBTMID")), ViewState("sDCOM"), ViewState("sDUFAC"), ViewState("sDCUST"), ViewState("sDSF"), ViewState("sDST"), ViewState("sDRSTS"), IIf(ViewState("sDRSN") = Nothing, 0, ViewState("sDRSN")), ViewState("sCDF"), ViewState("sCDT"), ViewState("sSB"), ViewState("sPNO"))

            If commonFunctions.CheckDataSet(ds) = True Then

                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpARDeduction.DataSource = dv
                rpARDeduction.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()
            Else
                cmdFirst.Enabled = False
                cmdGo.Enabled = False
                cmdPrev.Enabled = False
                cmdNext.Enabled = False
                cmdLast.Enabled = False

                rpARDeduction.Visible = False

                txtGoToPage.Visible = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
      Handles lnkRecStatus.Click, lnkRecNo.Click, lnkRsnDeduct.Click, lnkUGNFac.Click, lnkRefNo.Click, lnkDeductAmt.Click, lnkDateSub.Click, lnkDaysOld.Click, lnkClosedDate.Click

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
            lblErrors.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

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
        Response.Redirect("AR_Deduction.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionExpCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("AR_ARDID").Value = txtARDID.Text
            Response.Cookies("AR_DREFNO").Value = txtReferenceNo.Text
            Response.Cookies("AR_SBTMID").Value = ddSubmittedBy.SelectedValue
            Response.Cookies("AR_DCOM").Value = txtComments.Text
            Response.Cookies("AR_DUFAC").Value = ddUGNFacility.SelectedValue
            Response.Cookies("AR_DCUST").Value = ddCustomer.SelectedValue
            Response.Cookies("AR_DSF").Value = txtDateSubFrom.Text
            Response.Cookies("AR_DST").Value = txtDateSubTo.Text
            Response.Cookies("AR_DRSTS").Value = ddRecStatus.SelectedValue
            Response.Cookies("AR_DRSN").Value = ddReason.SelectedValue
            Response.Cookies("AR_CDF").Value = txtClosedDateFrom.Text
            Response.Cookies("AR_CDT").Value = txtClosedDateTo.Text
            Response.Cookies("AR_SB").Value = ddSortBy.SelectedValue
            Response.Cookies("AR_PNO").Value = txtPartNo.Text

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
            ARGroupModule.DeleteARDeductionCookies()
            HttpContext.Current.Session("sessionExpCurrentPage") = Nothing

            Response.Redirect("AR_Deduction_List.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Public Function ShowHideHistory(ByVal RecStatus As String) As Boolean
        If RecStatus = "Open" Then
            Return False
        Else
            Return True
        End If
    End Function 'EOF ShowHideHistory

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

    Protected Function SetBackGroundColor(ByVal RoutingStatus As String) As String

        Dim strReturnValue As String = "White"

        Select Case RoutingStatus
            Case "A"
                strReturnValue = "Lime"
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
