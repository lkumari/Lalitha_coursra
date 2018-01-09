
Partial Class Packaging_ContainerList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Packaging"
            m.ContentLabel = "Container Search"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Packaging</b> > Container Search"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PKGExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"


            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sCNo") = ""
                ViewState("sDesc") = ""
                ViewState("sType") = ""
                ViewState("sVendor") = 0
                ViewState("sOEM") = ""
                ViewState("sCustomer") = ""

                BindCriteria()
                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("PCO_CNO") Is Nothing Then
                    txtContainerNo.Text = Server.HtmlEncode(Request.Cookies("PCO_CNO").Value)
                    ViewState("sCNo") = Server.HtmlEncode(Request.Cookies("PCO_CNO").Value)
                End If

                If Not Request.Cookies("PCO_CDesc") Is Nothing Then
                    txtDescription.Text = Server.HtmlEncode(Request.Cookies("PCO_CDesc").Value)
                    ViewState("sDesc") = Server.HtmlEncode(Request.Cookies("PCO_CDesc").Value)
                End If

                If Not Request.Cookies("PCO_Type") Is Nothing Then
                    txtType.Text = Server.HtmlEncode(Request.Cookies("PCO_Type").Value)
                    ViewState("sType") = Server.HtmlEncode(Request.Cookies("PCO_Type").Value)
                End If

                If (Not Request.Cookies("PCO_Customer") Is Nothing) Then
                    txtCustomer.Text = Server.HtmlEncode(Request.Cookies("PCO_Customer").Value)
                    ViewState("sCustomer") = Server.HtmlEncode(Request.Cookies("PCO_Customer").Value)
                End If

                If Not Request.Cookies("PCO_Vendor") Is Nothing Then
                    ddVendor.SelectedValue = Server.HtmlEncode(Request.Cookies("PCO_Vendor").Value)
                    ViewState("sVendor") = Server.HtmlEncode(Request.Cookies("PCO_Vendor").Value)
                End If

                If Not Request.Cookies("PCO_OEM") Is Nothing Then
                    ddOEM.SelectedValue = Server.HtmlEncode(Request.Cookies("PCO_OEM").Value)
                    ViewState("sOEM") = Server.HtmlEncode(Request.Cookies("PCO_OEM").Value)
                End If

                ''******
                '' Bind drop down lists
                ''******
                BindData()

            Else
                ViewState("sCNo") = txtContainerNo.Text
                ViewState("sDesc") = txtDescription.Text
                ViewState("sType") = txtType.Text
                ViewState("sVendor") = ddVendor.SelectedValue
                ViewState("sOEM") = ddOEM.SelectedValue
                ViewState("sCustomer") = txtCustomer.Text
            End If

            'Set a value to CurrentPage
            If HttpContext.Current.Session("SortExpPCO") IsNot Nothing Then
                GridViewSortExpression = HttpContext.Current.Session("SortExpPCO")
                GridViewSortDirection = HttpContext.Current.Session("SortDirPCO")
                gvContainer.Sort(GridViewSortExpression, GridViewSortDirection)
            End If

            If HttpContext.Current.Session("sessionPCOCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionPCOCurrentPage")
                gvContainer.PageIndex = HttpContext.Current.Session("sessionPCOCurrentPage")
                BindData()
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

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
            Dim iFormID As Integer = 64 'Container Form ID
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
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region 'EOF Security

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            HttpContext.Current.Session("sessionPCOCurrentPage") = Nothing
            HttpContext.Current.Session("SortExpPCO") = Nothing
            HttpContext.Current.Session("SortDirPCO") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("PCO_CNO").Value = txtContainerNo.Text
            Response.Cookies("PCO_CDesc").Value = txtDescription.Text
            Response.Cookies("PCO_Type").Value = txtType.Text
            Response.Cookies("PCO_Customer").Value = txtCustomer.Text
            Response.Cookies("PCO_Vendor").Value = ddVendor.SelectedValue
            Response.Cookies("PCO_OEM").Value = ddOEM.SelectedValue

            ' Set viewstate variable to the first page
            CurrentPage = 0

            ' Reset # of Records Listed count
            BindData()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            PKGModule.DeletePkgContainerCookies()
            HttpContext.Current.Session("sessionPCOCurrentPage") = Nothing
            HttpContext.Current.Session("SortExpPCO") = Nothing
            HttpContext.Current.Session("SortDirPCO") = Nothing

            Response.Cookies("PCO_CNO").Value = ""
            Response.Cookies("PCO_CDesc").Value = ""
            Response.Cookies("PCO_Type").Value = ""
            Response.Cookies("PCO_Customer").Value = ""
            Response.Cookies("PCO_Vendor").Value = 0
            Response.Cookies("PCO_OEM").Value = ""

            Response.Redirect("ContainerList.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnReset_Click

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try

            Response.Redirect("Container.aspx", False)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnAdd_Click

    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet
            ''bind existing data to drop down Vendor control for selection criteria for search
            ds = SUPModule.GetSupplierLookUp("", "", "", "", "", 1)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendor.DataSource = ds
                ddVendor.DataTextField = ds.Tables(0).Columns("ddVendorName").ColumnName.ToString()
                ddVendor.DataValueField = ds.Tables(0).Columns("ddVendorNo").ColumnName.ToString()
                ddVendor.DataBind()
                ddVendor.Items.Insert(0, "")
            End If

            ' ''bind existing data to drop down Customer control for selection criteria for search
            'ds = commonFunctions.GetOEMManufacturer("")
            'If (ds.Tables.Item(0).Rows.Count > 0) Then
            '    ddCustomer.DataSource = ds
            '    ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            '    ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            '    ddCustomer.DataBind()
            '    ddCustomer.Items.Insert(0, "")
            'End If

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetOEMbyOEMMfg("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddOEM.DataSource = ds
                ddOEM.DataTextField = ds.Tables(0).Columns("ddOEMDesc").ColumnName.ToString()
                ddOEM.DataValueField = ds.Tables(0).Columns("OEM").ColumnName.ToString()
                ddOEM.DataBind()
                ddOEM.Items.Insert(0, "")
            End If
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindCriteria

#Region "GridView Wrokaround"
    Protected Sub gvContainer_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvContainer.RowCreated

        If e.Row.RowType = DataControlRowType.Header Then
            AddSortImage(e.Row)
        End If

    End Sub 'EOF gvContainer_RowCreated

    Private Sub AddSortImage(ByVal headerRow As GridViewRow)
        Dim selCol As Integer = GetSortColumnIndex(HttpContext.Current.Session("SortExpPCO"))

        If -1 = selCol Then
            Return
        End If

        ' Create the sorting image based on the sort direction
        Dim sortImage As New System.Web.UI.WebControls.Image()
        If selCol > 0 Then
            If System.Web.UI.WebControls.SortDirection.Ascending = HttpContext.Current.Session("SortDirPCO") Then
                'sortImage.ImageUrl = "~/images/collapse.jpg"
                sortImage.ImageUrl = "~/images/red up.jpg"
                sortImage.AlternateText = "Ascending"
            Else
                ' sortImage.ImageUrl = "~/images/expand.jpg"
                sortImage.ImageUrl = "~/images/red down.jpg"
                sortImage.AlternateText = "Descending"
            End If

            ' Add the image to the appropriate header cell
            headerRow.Cells(selCol).Controls.Add(sortImage)
        End If
    End Sub 'EOF AddSortImage

    Private Function GetSortColumnIndex(ByVal strCol As [String]) As Integer
        ' This is a helper method used to determine the index of the
        ' column being sorted. If no column is being sorted, -1 is returned.
        For Each field As DataControlField In gvContainer.Columns
            If field.SortExpression = strCol Then
                Return gvContainer.Columns.IndexOf(field)
            End If
        Next

        Return -1
    End Function 'EOF GetSortColumnIndex

#End Region 'EOF Gridview Work around

#Region "Paging Routine"
    Private Sub BindData()
        Try
            lblMessage.Text = ""
            lblMessage.Visible = False

            Dim ds As DataSet = New DataSet
            'bind data to repeater for Buyer's, exec's or current team member view only                      
            ds = PKGModule.GetPkgContainer(0, ViewState("sCNo"), ViewState("sDesc"), ViewState("sType"), ViewState("sOEM"), ViewState("sCustomer"), IIf(ViewState("sVendor") = Nothing, 0, ViewState("sVendor")))

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
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF of BindData

    Protected Sub gvContainer_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvContainer.RowDataBound

        PagingInformation.Text = String.Format("Page {0} of {1}...   Go to ", _
                                               gvContainer.PageIndex + 1, gvContainer.PageCount)

        ' Clear out all of the items in the DropDownList
        PageList.Items.Clear()

        ' Add a ListItem for each page
        For i As Integer = 0 To gvContainer.PageCount - 1

            ' Add the new ListItem   
            Dim pageListItem As New ListItem(String.Concat("Page ", i + 1), i.ToString())
            PageList.Items.Add(pageListItem)
            ' select the current item, if needed   
            If i = gvContainer.PageIndex Then
                pageListItem.Selected = True
            End If
        Next
    End Sub 'EOF gvLayout_DataBound

    Protected Sub PageList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageList.SelectedIndexChanged
        ' Jump to the specified page       
        gvContainer.PageIndex = Convert.ToInt32(PageList.SelectedValue)
        Try
            ' Set viewstate variable to the next page
            CurrentPage = gvContainer.PageIndex
            HttpContext.Current.Session("sessionPCOCurrentPage") = gvContainer.PageIndex

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF PageList_SelectIndexChanged

    Protected Sub gvContainer_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvContainer.PageIndexChanged

        CurrentPage = gvContainer.PageIndex
        HttpContext.Current.Session("sessionPCOCurrentPage") = gvContainer.PageIndex

        ' Reload control
        BindData()
    End Sub 'EOF gvContainer_PageIndexChanged

    Public Property CurrentPage() As Integer

        Get
            'Used for the Record listed in the BindData()
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

#End Region 'EOF Paging Routine

#Region "Sorting Routine"

    Protected Sub gvContainer_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvContainer.Sorting
        ''This is invoked when the grid column is Clicked for Sorting, 
        ''Clicking again will Toggle Descending/Ascending through the Sort Expression

        GridViewSortExpression = e.SortExpression
        GridViewSortDirection = IIf(e.SortDirection = 0, "ASC", "DESC")

        SortInformationLabel.Text = "Sort By: " & GridViewSortExpression & " in " & GridViewSortDirection & " order."
        HttpContext.Current.Session("SortExpPCO") = GridViewSortExpression
        HttpContext.Current.Session("SortDirPCO") = e.SortDirection

    End Sub 'EOF gvContainer_Sorting

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
