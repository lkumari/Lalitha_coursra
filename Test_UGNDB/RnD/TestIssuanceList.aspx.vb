' ************************************************************************************************
' Name:	TestIssuanceList.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 02/24/2009    LRey			Created .Net application
' 06/21/2012    LRey            Added "Record Listed" to the search page.
' 01/30/2014    LRey            Replaced SoldTo|CABBV with a RowID next sequential. 
'                               Added CostSheetID per RD-3267 support request.
' ************************************************************************************************
Partial Class RnD_TIL
    Inherits System.Web.UI.Page
    Protected WithEvents lnkReqNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDesc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkClass As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkFac As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkIssuer As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkReqDat As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkReqCat As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkReqStat As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkProjID As System.Web.UI.WebControls.LinkButton

    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
  Handles lnkReqNo.Click, lnkDesc.Click, lnkClass.Click, lnkFac.Click, lnkIssuer.Click, lnkReqDat.Click, lnkReqCat.Click, lnkReqStat.Click, lnkProjID.Click

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
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = RnDModule.GetTestIssuanceRequests(ViewState("sRequestID"), ViewState("sSampleProdDesc"), IIf(ViewState("sSampleIssuer") = Nothing, 0, ViewState("sSampleIssuer")), ViewState("sUGNLocation"), IIf(ViewState("sCommodity") = Nothing, 0, ViewState("sCommodity")), ViewState("sReqStatus"), ViewState("sPNO"), IIf(ViewState("sReqCat") = Nothing, 0, ViewState("sReqCat")), ViewState("sRptID"), ViewState("sTAG"), IIf(ViewState("sProgramID") = Nothing, 0, ViewState("sProgramID")), IIf(ViewState("sTestClassID") = Nothing, 0, ViewState("sTestClassID")), ViewState("sProjectID"), ViewState("sProjectNo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpTestIssuance.DataSource = dv
                rpTestIssuance.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Test Issuance Search"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > Test Issuance Search"
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
            ctl = m.FindControl("RnDExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"
            'focus on Vehicle List screen Program field
            txtRequestID.Focus()

            If HttpContext.Current.Session("sessionTICurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionTICurrentPage")
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sRequestID") = ""
                ViewState("sSampleIssuer") = ""
                ViewState("sUGNLocation") = ""
                ViewState("sCommodity") = ""
                ViewState("sReqStatus") = ""
                ViewState("sPNO") = ""
                ViewState("sReqCat") = ""
                ViewState("sRptID") = ""
                ViewState("sTAG") = ""
                ViewState("sProgramID") = ""
                ViewState("sTestClassID") = ""
                ViewState("sProjectID") = ""
                ViewState("sProjectNo") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("TI_RequestID") Is Nothing Then
                    txtRequestID.Text = Server.HtmlEncode(Request.Cookies("TI_RequestID").Value)
                    ViewState("sRequestID") = Server.HtmlEncode(Request.Cookies("TI_RequestID").Value)
                End If


                If Not Request.Cookies("TI_SampleProdDesc") Is Nothing Then
                    txtSampleProdDesc.Text = Server.HtmlEncode(Request.Cookies("TI_SampleProdDesc").Value)
                    ViewState("sSampleProdDesc") = Server.HtmlEncode(Request.Cookies("TI_SampleProdDesc").Value)
                End If

                If Not Request.Cookies("TI_SampleIssuer") Is Nothing Then
                    ddSampleIssuer.SelectedValue = Server.HtmlEncode(Request.Cookies("TI_SampleIssuer").Value)
                    ViewState("sSampleIssuer") = Server.HtmlEncode(Request.Cookies("TI_SampleIssuer").Value)
                End If

                If Not Request.Cookies("TI_UGNLocation") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("TI_UGNLocation").Value)
                    ViewState("sUGNLocation") = Server.HtmlEncode(Request.Cookies("TI_UGNLocation").Value)
                End If

                If Not Request.Cookies("TI_Commodity") Is Nothing Then
                    ddCommodity.SelectedValue = Server.HtmlEncode(Request.Cookies("TI_Commodity").Value)
                    ViewState("sCommodity") = Server.HtmlEncode(Request.Cookies("TI_Commodity").Value)
                End If

                If Not Request.Cookies("TI_RequestStatus") Is Nothing Then
                    ddRequestStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("TI_RequestStatus").Value)
                    ViewState("sReqStatus") = Server.HtmlEncode(Request.Cookies("TI_RequestStatus").Value)
                End If

                If Not Request.Cookies("TI_PNO") Is Nothing Then
                    txtPNO.Text = Server.HtmlEncode(Request.Cookies("TI_PNO").Value)
                    ViewState("sPNO") = Server.HtmlEncode(Request.Cookies("TI_PNO").Value)
                End If

                If Not Request.Cookies("TI_ReqTyp") Is Nothing Then
                    ddRequestCategory.SelectedValue = Server.HtmlEncode(Request.Cookies("TI_ReqCat").Value)
                    ViewState("sReqCat") = Server.HtmlEncode(Request.Cookies("TI_ReqCat").Value)
                End If

                If Not Request.Cookies("TI_RptID") Is Nothing Then
                    txtTestRptNo.Text = Server.HtmlEncode(Request.Cookies("TI_RptID").Value)
                    ViewState("sRptID") = Server.HtmlEncode(Request.Cookies("TI_RptID").Value)
                End If

                If Not Request.Cookies("TI_TAG") Is Nothing Then
                    txtTAG.Text = Server.HtmlEncode(Request.Cookies("TI_TAG").Value)
                    ViewState("sTAG") = Server.HtmlEncode(Request.Cookies("TI_TAG").Value)
                End If

                If Not Request.Cookies("TI_ProgramID") Is Nothing Then
                    ddProgram.SelectedValue = Server.HtmlEncode(Request.Cookies("TI_ProgramID").Value)
                    ViewState("sProgramID") = Server.HtmlEncode(Request.Cookies("TI_ProgramID").Value)
                End If

                If Not Request.Cookies("TI_TestClassID") Is Nothing Then
                    ddTestClass.SelectedValue = Server.HtmlEncode(Request.Cookies("TI_TestClassID").Value)
                    ViewState("sTestClassID") = Server.HtmlEncode(Request.Cookies("TI_TestClassID").Value)
                End If

                If Not Request.Cookies("TI_ProjectID") Is Nothing Then
                    txtProjectID.Text = Server.HtmlEncode(Request.Cookies("TI_ProjectID").Value)
                    ViewState("sProjectID") = Server.HtmlEncode(Request.Cookies("TI_ProjectID").Value)
                End If

                If Not Request.Cookies("TI_ProjectNo") Is Nothing Then
                    txtAppropriation.Text = Server.HtmlEncode(Request.Cookies("TI_ProjectNo").Value)
                    ViewState("sProjectNo") = Server.HtmlEncode(Request.Cookies("TI_ProjectNo").Value)
                End If


                ''******
                '' Bind drop down lists
                ''******
                BindData()

            Else
                ViewState("sRequestID") = txtRequestID.Text
                ViewState("sSampleProdDesc") = txtSampleProdDesc.Text.ToString
                ViewState("sSampleIssuer") = ddSampleIssuer.SelectedValue
                ViewState("sUGNLocation") = ddUGNFacility.SelectedValue
                ViewState("sCommodity") = ddCommodity.SelectedValue
                ViewState("sReqStatus") = ddRequestStatus.SelectedValue
                ViewState("sPNO") = IIf(txtPNO.Text = Nothing, "", txtPNO.Text)
                ViewState("sReqCat") = ddRequestCategory.SelectedValue
                ViewState("sRptID") = txtTestRptNo.Text
                ViewState("sTAG") = txtTAG.Text
                ViewState("sProgramID") = ddProgram.SelectedValue
                ViewState("sTestClassID") = ddTestClass.SelectedValue
                ViewState("sProjectID") = txtProjectID.Text
                ViewState("sProjectNo") = txtAppropriation.Text
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
            Dim iFormID As Integer = 56 'Test Issuance Form ID
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
#End Region
    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Sample Issuer control for selection criteria for search
        ds = commonFunctions.GetTeamMember("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddSampleIssuer.DataSource = ds
            ddSampleIssuer.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddSampleIssuer.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            ddSampleIssuer.DataBind()
            ddSampleIssuer.Items.Insert(0, "")
        End If

        ''bind existing data to drop down UGN Location control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Commodity control for selection criteria 
        ds = commonFunctions.GetCommodity(0, "", "", 0)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddCommodity.DataSource = ds
            ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
            ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
            ddCommodity.DataBind()
            ddCommodity.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Program control for selection criteria for search
        ds = commonFunctions.GetProgram("", "", "")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProgram.DataSource = ds
            ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
            ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
            ddProgram.DataBind()
            ddProgram.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Test Classification control for selection criteria for search
        ds = RnDModule.GetTestingClassification("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddTestClass.DataSource = ds
            ddTestClass.DataTextField = ds.Tables(0).Columns("TestClassName").ColumnName.ToString()
            ddTestClass.DataValueField = ds.Tables(0).Columns("TestClassID").ColumnName.ToString()
            ddTestClass.DataBind()
            ddTestClass.Items.Insert(0, "")
        End If
    End Sub 'EOF BindCriteria

    Private Sub BindData()

        Try
            lblErrors.Text = ""

            Dim ds As DataSet = New DataSet

            'bind existing data                      
            ds = RnDModule.GetTestIssuanceRequests(ViewState("sRequestID"), ViewState("sSampleProdDesc"), IIf(ViewState("sSampleIssuer") = Nothing, 0, ViewState("sSampleIssuer")), ViewState("sUGNLocation"), IIf(ViewState("sCommodity") = Nothing, 0, ViewState("sCommodity")), ViewState("sReqStatus"), ViewState("sPNO"), IIf(ViewState("sReqCat") = Nothing, 0, ViewState("sReqCat")), ViewState("sRptID"), ViewState("sTAG"), IIf(ViewState("sProgramID") = Nothing, 0, ViewState("sProgramID")), IIf(ViewState("sTestClassID") = Nothing, 0, ViewState("sTestClassID")), ViewState("sProjectID"), ViewState("sProjectNo"))

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    rpTestIssuance.DataSource = ds
                    rpTestIssuance.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpTestIssuance.DataSource = objPds
                    rpTestIssuance.DataBind()

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
                        lblToRec.Text = rpTestIssuance.Items.Count
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

    End Sub 'EOF BindData

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
            HttpContext.Current.Session("sessionTICurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionTICurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionTICurrentPage") = CurrentPage

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


                HttpContext.Current.Session("sessionTICurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionTICurrentPage") = CurrentPage

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
        Response.Redirect("TestIssuanceNew.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionTICurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("TI_RequestID").Value = txtRequestID.Text
            Response.Cookies("TI_SampleIssuer").Value = ddSampleIssuer.SelectedValue
            Response.Cookies("TI_SampleProdDesc").Value = txtSampleProdDesc.Text
            Response.Cookies("TI_UGNLocation").Value = ddUGNFacility.SelectedValue
            Response.Cookies("TI_Commodity").Value = ddCommodity.SelectedValue
            Response.Cookies("TI_RequestStatus").Value = ddRequestStatus.SelectedValue
            Response.Cookies("TI_PNO").Value = txtPNO.Text
            Response.Cookies("TI_ReqCat").Value = ddRequestCategory.SelectedValue
            Response.Cookies("TI_RptID").Value = txtTestRptNo.Text
            Response.Cookies("TI_TAG").Value = txtTAG.Text
            Response.Cookies("TI_ProgramID").Value = ddProgram.SelectedValue
            Response.Cookies("TI_TestClassID").Value = ddTestClass.SelectedValue
            Response.Cookies("TI_ProjectID").Value = txtProjectID.Text
            Response.Cookies("TI_ProjectNo").Value = txtAppropriation.Text

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
            RnDModule.DeleteTestIssuanceCookies()
            HttpContext.Current.Session("sessionTICurrentPage") = Nothing

            Response.Redirect("TestIssuanceList.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click
    Public Function GoToAcoustic(ByVal ProjectID As Integer) As String
        Dim strReturnValue As String = "#"

        If Not IsDBNull(ProjectID) Then
            strReturnValue = "~/Acoustic/Acoustic_Project_Detail.aspx?pProjID=" & ProjectID
        End If

        GoToAcoustic = strReturnValue

    End Function 'EOF GoToAcoustic
End Class
