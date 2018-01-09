' ************************************************************************************************
' Name:	SampleMaterialRequestList.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 01/18/2013    LRey			Created .Net application
' ************************************************************************************************
#Region "Directives"

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Text

#End Region

Partial Class PGM_SampleMaterialRequestList
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim a As String = commonFunctions.UserInfo()
            ViewState("TMLoc") = HttpContext.Current.Session("UserFacility")

            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pSMRNo") <> "" Then
                ViewState("pSMRNo") = HttpContext.Current.Request.QueryString("pSMRNo")
            Else
                ViewState("pSMRNo") = ""
            End If


            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Sample Material Request Search"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pAprv") = 0 Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > Sample Material Request Search"
                Else
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > Sample Material Request Search > <a href='crSampleMaterialRequestApproval.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1'><b>Approval</b></a>"
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

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sSMRNo") = ""
                ViewState("sSDesc") = ""
                ViewState("sRTMID") = 0
                ViewState("sATMID") = 0
                ViewState("sUFac") = ""
                ViewState("sCust") = ""
                ViewState("sPNo") = ""
                ViewState("sIE") = ""
                ViewState("sPONo") = ""
                ViewState("sRStat") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("SMR_SMRNO") Is Nothing Then
                    txtSMRNo.Text = Server.HtmlEncode(Request.Cookies("SMR_SMRNO").Value)
                    ViewState("sSMRNo") = Server.HtmlEncode(Request.Cookies("SMR_SMRNO").Value)
                End If

                If Not Request.Cookies("SMR_SDESC") Is Nothing Then
                    txtSampleDesc.Text = Server.HtmlEncode(Request.Cookies("SMR_SDESC").Value)
                    ViewState("sSDesc") = Server.HtmlEncode(Request.Cookies("SMR_SDESC").Value)
                End If

                If Not Request.Cookies("SMR_RTMID") Is Nothing Then
                    ddRequestor.SelectedValue = Server.HtmlEncode(Request.Cookies("SMR_RTMID").Value)
                    ViewState("sRTMID") = Server.HtmlEncode(Request.Cookies("SMR_RTMID").Value)
                End If

                If Not Request.Cookies("SMR_ATMID") Is Nothing Then
                    ddAccountManager.SelectedValue = Server.HtmlEncode(Request.Cookies("SMR_ATMID").Value)
                    ViewState("sATMID") = Server.HtmlEncode(Request.Cookies("SMR_ATMID").Value)
                End If

                If Not Request.Cookies("SMR_UFAC") Is Nothing Then
                    ddUGNLocation.SelectedValue = Server.HtmlEncode(Request.Cookies("SMR_UFAC").Value)
                    ViewState("sUFac") = Server.HtmlEncode(Request.Cookies("SMR_UFAC").Value)
                End If

                If Not Request.Cookies("SMR_CUST") Is Nothing Then
                    ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("SMR_CUST").Value)
                    ViewState("sCust") = Server.HtmlEncode(Request.Cookies("SMR_CUST").Value)
                End If

                If Not Request.Cookies("SMR_PNO") Is Nothing Then
                    txtPartNo.Text = Server.HtmlEncode(Request.Cookies("SMR_PNO").Value)
                    ViewState("sPNo") = Server.HtmlEncode(Request.Cookies("SMR_PNO").Value)
                End If

                If Not Request.Cookies("SMR_IE") Is Nothing Then
                    ddIntExt.SelectedValue = Server.HtmlEncode(Request.Cookies("SMR_IE").Value)
                    ViewState("sIE") = Server.HtmlEncode(Request.Cookies("SMR_IE").Value)
                End If

                If Not Request.Cookies("SMR_PONO") Is Nothing Then
                    txtPONo.Text = Server.HtmlEncode(Request.Cookies("SMR_PONO").Value)
                    ViewState("sPONo") = Server.HtmlEncode(Request.Cookies("SMR_PONO").Value)
                End If

                If Not Request.Cookies("SMR_RSTAT") Is Nothing Then
                    ddRecStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("SMR_RSTAT").Value)
                    ViewState("sRStat") = Server.HtmlEncode(Request.Cookies("SMR_RSTAT").Value)
                End If

                BindData()
            Else
                ViewState("sSMRNo") = txtSMRNo.Text
                ViewState("sSDesc") = txtSampleDesc.Text
                ViewState("sRTMID") = ddRequestor.SelectedValue
                ViewState("sATMID") = ddAccountManager.SelectedValue
                ViewState("sUFac") = ddUGNLocation.SelectedValue
                ViewState("sCust") = ddCustomer.SelectedValue
                ViewState("sPNo") = txtPartNo.Text
                ViewState("sIE") = ddIntExt.SelectedValue
                ViewState("sPONo") = txtPONo.Text
                ViewState("sRStat") = ddRecStatus.SelectedValue
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
            Dim iFormID As Integer = 135 'Sample Material Request Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Mike.Kelley", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

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
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ViewState("Admin") = True
                                            btnAdd.Enabled = True
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
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region

#Region "General Process"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            HttpContext.Current.Session("sessionSMRCurrentPage") = Nothing
            HttpContext.Current.Session("SortExpSMR") = Nothing
            HttpContext.Current.Session("SortDirSMR") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("SMR_SMRNO").Value = txtSMRNo.Text
            Response.Cookies("SMR_SDESC").Value = txtSampleDesc.Text
            Response.Cookies("SMR_RTMID").Value = ddRequestor.SelectedValue
            Response.Cookies("SMR_ATMID").Value = ddAccountManager.SelectedValue
            Response.Cookies("SMR_UFAC").Value = ddUGNLocation.SelectedValue
            Response.Cookies("SMR_CUST").Value = ddCustomer.SelectedValue
            Response.Cookies("SMR_PNO").Value = txtPartNo.Text
            Response.Cookies("SMR_IE").Value = ddIntExt.SelectedValue
            Response.Cookies("SMR_PONO").Value = txtPONo.Text
            Response.Cookies("SMR_RSTAT").Value = ddRecStatus.SelectedValue


            ' Set viewstate variable to the first page
            CurrentPage = 0
            Response.Redirect("SampleMaterialRequestList.aspx", False)
            ' Reset # of Records Listed count
            BindData()


        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            PGMModule.DeleteSampleMtrlReqCookies()
            HttpContext.Current.Session("sessionSMRCurrentPage") = Nothing
            HttpContext.Current.Session("SortExpSMR") = Nothing
            HttpContext.Current.Session("SortDirSMR") = Nothing


            Response.Redirect("SampleMaterialRequestList.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnReset_Click

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try

            Response.Redirect("SampleMaterialRequest.aspx", False)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnAdd_Click

    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Team Member control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddRequestor.DataSource = ds
                ddRequestor.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddRequestor.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddRequestor.DataBind()
                ddRequestor.Items.Insert(0, "")
                'ddRequestor.Enabled = False
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


            ''bind existing data to drop down UGN Location control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNLocation.DataSource = ds
                ddUGNLocation.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNLocation.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNLocation.DataBind()
                ddUGNLocation.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetOEMMfgCABBV("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMMfg_CABBV").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("OEMMfg_CABBV").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message & " 3 "
            lblErrors.Visible = True
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF BindCriteria

#End Region

#Region "GridView Workaround"
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

    Protected Function SetBackGroundColor(ByVal RoutingStatus As String) As Color

        Dim strReturnValue As Color = Color.White

        Select Case RoutingStatus
            Case "A"
                strReturnValue = Color.Lime
            Case "C"
                strReturnValue = Color.White
            Case "N"
                strReturnValue = Color.Fuchsia
            Case "T"
                strReturnValue = Color.Yellow
            Case "R"
                strReturnValue = Color.Red
            Case "V"
                strReturnValue = Color.Gray
        End Select

        SetBackGroundColor = strReturnValue

    End Function 'EOF SetBackGroundColor

    Private Function GetSortColumnIndex(ByVal strCol As [String]) As Integer
        ' This is a helper method used to determine the index of the
        ' column being sorted. If no column is being sorted, -1 is returned.
        For Each field As DataControlField In gvSMR.Columns
            If field.SortExpression = strCol Then
                Return gvSMR.Columns.IndexOf(field)
            End If
        Next

        Return -1
    End Function 'EOF GetSortColumnIndex

#End Region 'EOF Gridview Work around

#Region "Paging Routine"
    Private Sub BindData()
        Try
            lblErrors.Text = ""

            Dim ds As DataSet = New DataSet
            ds = PGMModule.GetSampleMtrlReq(ViewState("sSMRNo"), ViewState("sSDesc"), IIf(ViewState("sRTMID") = Nothing, 0, ViewState("sRTMID")), IIf(ViewState("sATMID") = Nothing, 0, ViewState("sATMID")), ViewState("sUFac"), ViewState("sCust"), ViewState("sPNo"), ViewState("sIE"), ViewState("sPONo"), ViewState("sRStat"))

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    ' Display # of records
                    If (CurrentPage + 1) > 1 Then
                        lblFromRec.Text = (((CurrentPage + 1) * 30) - 30) + 1
                        lblToRec.Text = (CurrentPage + 1) * 30
                        'lblFromRec.Text = (((CurrentPage + 1) * 3) - 3) + 1 ''Use to test gridview with less # of recs
                        'lblToRec.Text = (CurrentPage + 1) * 3 ''Use to test gridview with less # of recs
                        If lblToRec.Text > objPds.DataSourceCount Then
                            lblToRec.Text = objPds.DataSourceCount
                        End If
                    Else
                        lblFromRec.Text = ds.Tables.Count
                        lblToRec.Text = (CurrentPage + 1) * 30
                        'lblToRec.Text = (CurrentPage + 1) * 3 ''Use to test gridview with less # of recs
                        If lblToRec.Text > objPds.DataSourceCount Then
                            lblToRec.Text = objPds.DataSourceCount
                        End If
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

    Protected Sub gvSMR_RowDataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSMR.DataBound

        PagingInformation.Text = String.Format("Page {0} of {1}...   Go to ", _
                                               gvSMR.PageIndex + 1, gvSMR.PageCount)

        ' Clear out all of the items in the DropDownList
        PageList.Items.Clear()

        ' Add a ListItem for each page
        For i As Integer = 0 To gvSMR.PageCount - 1

            ' Add the new ListItem   
            Dim pageListItem As New ListItem(String.Concat("Page ", i + 1), i.ToString())
            PageList.Items.Add(pageListItem)
            ' select the current item, if needed   
            If i = gvSMR.PageIndex Then
                pageListItem.Selected = True
            End If
        Next
    End Sub 'EOF gvSMR_DataBound

    Protected Sub PageList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageList.SelectedIndexChanged
        ' Jump to the specified page       
        gvSMR.PageIndex = Convert.ToInt32(PageList.SelectedValue)
        Try
            SaveCheckedValues()

            ' Set viewstate variable to the next page
            CurrentPage = gvSMR.PageIndex
            HttpContext.Current.Session("sessionSMRCurrentPage") = gvSMR.PageIndex

            ' Reload control
            BindData()

            PopulateCheckedValues()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'PageList_SelectIndexChanged

    Protected Sub gvSMR_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSMR.PageIndexChanged

        CurrentPage = gvSMR.PageIndex
        HttpContext.Current.Session("sessionSMRCurrentPage") = gvSMR.PageIndex

        ' Reload control
        BindData()
    End Sub 'EOF gvSMR_PageIndexChanged

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

    Protected Sub gvSMR_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        'SaveCheckedValues()
        'gvSMR.PageIndex = e.NewPageIndex
        ''gvSMR.DataBind()
        'PopulateCheckedValues()
        gvSMR.PageIndex = e.NewPageIndex
        SaveCheckedValues()
        gvSMR.DataBind()

    End Sub

    Private Sub PopulateCheckedValues()
        Dim RowIDList As ArrayList = DirectCast(Session("CHECKED_ITEMS"), ArrayList)
        If RowIDList IsNot Nothing AndAlso RowIDList.Count > 0 Then
            For Each row As GridViewRow In gvSMR.Rows
                Dim index As Integer = CInt(gvSMR.DataKeys(row.RowIndex).Value)
                If RowIDList.Contains(index) Then
                    DirectCast(row.FindControl("PrintSelector"), CheckBox).Checked = True
                End If
            Next
        End If

    End Sub

    Private Sub SaveCheckedValues()

        'Dim RowIDList As New ArrayList()
        'Dim index As Integer = -1
        'For Each row As GridViewRow In gvSMR.Rows
        '    index = CInt(gvSMR.DataKeys(row.RowIndex).Value)
        '    Dim result As Boolean = DirectCast(row.FindControl("PrintSelector"), CheckBox).Checked

        '    ' Check in the Session
        '    If Session("CHECKED_ITEMS") IsNot Nothing Then
        '        RowIDList = DirectCast(Session("CHECKED_ITEMS"), ArrayList)
        '    End If
        '    If result Then
        '        If Not RowIDList.Contains(index) Then
        '            RowIDList.Add(index)
        '        End If
        '    Else
        '        RowIDList.Remove(index)
        '    End If
        'Next
        'If RowIDList IsNot Nothing AndAlso RowIDList.Count > 0 Then
        '    Session("CHECKED_ITEMS") = RowIDList
        'End If

        'Dim hiddenIDs() As String = {}
        'If Not String.IsNullOrEmpty(hiddenCatIDs.Value) Then
        '    hiddenIDs = hiddenCatIDs.Value.Split(New Char() {"|"})
        'End If

        'Dim arrIDs As New ArrayList()
        'Dim CatID As String = "0"

        'If hiddenIDs.Length <> 0 Then
        '    arrIDs.AddRange(hiddenIDs)
        'End If

        'For Each rowItem As GridViewRow In gvSMR.Rows
        '    Dim chk As CheckBox

        '    chk = CType(rowItem.Cells(0).FindControl("PrintSelector"), CheckBox)
        '    CatID = gvSMR.DataKeys(rowItem.RowIndex)("SMRNo").ToString()
        '    If chk.Checked Then
        '        If Not arrIDs.Contains(CatID) Then
        '            arrIDs.Add(CatID)
        '        End If
        '    Else
        '        If (arrIDs.Contains(CatID)) Then
        '            arrIDs.Remove(CatID)
        '        End If
        '    End If
        'Next

        'hiddenIDs = CType(arrIDs.ToArray(Type.GetType("System.String")), String())

        'hiddenCatIDs.Value = String.Join("|", hiddenIDs)


    End Sub

#End Region 'EOF Paging Routine

#Region "Sorting Routine"

    Protected Sub gvSMR_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvSMR.Sorting
        ''This is invoked when the grid column is Clicked for Sorting, 
        ''Clicking again will Toggle Descending/Ascending through the Sort Expression

        GridViewSortExpression = e.SortExpression
        GridViewSortDirection = IIf(e.SortDirection = 0, "ASC", "DESC")

        SortInformationLabel.Text = "Sort By: " & GridViewSortExpression & " in " & GridViewSortDirection & " order."
        HttpContext.Current.Session("SortExpPKG") = GridViewSortExpression
        HttpContext.Current.Session("SortDirPKG") = e.SortDirection

    End Sub 'EOF gvSMR_Sorting

    Private Property GridViewSortExpression() As String
        'Gets or Sets the GridView SortExpression Property
        Get
            Return If(TryCast(ViewState("SortExpression"), String), String.Empty)
        End Get
        Set(ByVal value As String)
            ViewState("SortExpression") = value
        End Set
    End Property 'EOF GridViewSortExpression

    Private Property GridViewSortDirection() As String
        'Gets or Sets the GridView SortDirection Property
        Get
            Return If(TryCast(ViewState("SortDirection"), String), "ASC")
        End Get
        Set(ByVal value As String)
            ViewState("SortDirection") = value
        End Set
    End Property 'EOF GridViewSortDirection

#End Region 'EOF Sorting Routine


End Class
