' ************************************************************************************************
' Name:	Colors.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 09/21/2012    SHoward		Created .Net application
' ************************************************************************************************
#Region "Directives"

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Text

#End Region
Partial Class PKG_PackagingList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Packaging"
            m.ContentLabel = "Packaging Layout Search"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Packaging</b> > Layout Search"
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
                ViewState("sLDesc") = ""
                ViewState("sCNO") = ""
                ViewState("sOEMMfg") = ""
                ViewState("sMake") = ""
                ViewState("sModel") = ""
                ViewState("sCustomer") = ""
                ViewState("sFAC") = ""
                ViewState("sDPT") = 0
                ViewState("sWC") = 0
                ViewState("sPNO") = ""

                ' ''BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("PL_LDESC") Is Nothing Then
                    txtDescription.Text = Server.HtmlEncode(Request.Cookies("PL_LDESC").Value)
                    ViewState("sLDesc") = Server.HtmlEncode(Request.Cookies("PL_LDESC").Value)
                End If

                If Not Request.Cookies("PL_CNO") Is Nothing Then
                    txtContainerNo.Text = Server.HtmlEncode(Request.Cookies("PL_CNO").Value)
                    ViewState("sCNO") = Server.HtmlEncode(Request.Cookies("PL_CNO").Value)
                End If

                If Not Request.Cookies("PL_OEMMFG") Is Nothing Then
                    cddOEMMfg.SelectedValue = Server.HtmlEncode(Request.Cookies("PL_OEMMFG").Value)
                    ViewState("sOEMMfg") = Server.HtmlEncode(Request.Cookies("PL_OEMMFG").Value)
                End If

                If Not Request.Cookies("PL_MAKE") Is Nothing Then
                    cddMake.SelectedValue = Server.HtmlEncode(Request.Cookies("PL_MAKE").Value)
                    ViewState("sMake") = Server.HtmlEncode(Request.Cookies("PL_MAKE").Value)
                End If

                If Not Request.Cookies("PL_MODEL") Is Nothing Then
                    cddModel.SelectedValue = Server.HtmlEncode(Request.Cookies("PL_MODEL").Value)
                    ViewState("sModel") = Server.HtmlEncode(Request.Cookies("PL_MODEL").Value)
                End If

                If (Not Request.Cookies("PL_Customer") Is Nothing) Then
                    txtCustomer.Text = Server.HtmlEncode(Request.Cookies("PL_Customer").Value)
                    ViewState("sCustomer") = Server.HtmlEncode(Request.Cookies("PL_Customer").Value)
                End If

                If Not Request.Cookies("PL_FAC") Is Nothing Then
                    cddUGNLocation.SelectedValue = Server.HtmlEncode(Request.Cookies("PL_FAC").Value)
                    ViewState("sFAC") = Server.HtmlEncode(Request.Cookies("PL_FAC").Value)
                End If

                If Not Request.Cookies("PL_DPT") Is Nothing Then
                    cddDepartment.SelectedValue = Server.HtmlEncode(Request.Cookies("PL_DPT").Value)
                    ViewState("sDPT") = Server.HtmlEncode(Request.Cookies("PL_DPT").Value)
                End If

                If Not Request.Cookies("PL_WC") Is Nothing Then
                    cddWorkCenter.SelectedValue = Server.HtmlEncode(Request.Cookies("PL_WC").Value)
                    ViewState("sWC") = Server.HtmlEncode(Request.Cookies("PL_WC").Value)
                End If

                If Not Request.Cookies("PL_PNO") Is Nothing Then
                    txtContainerNo.Text = Server.HtmlEncode(Request.Cookies("PL_PNO").Value)
                    ViewState("sPNO") = Server.HtmlEncode(Request.Cookies("PL_PNO").Value)
                End If
                ''******
                '' Bind drop down lists
                ' ''******
                BindData()

            Else
                ViewState("sLDesc") = txtDescription.Text
                ViewState("sCNO") = txtContainerNo.Text
                ViewState("sOEMMfg") = commonFunctions.GetCCDValue(cddOEMMfg.SelectedValue)
                ViewState("sMake") = commonFunctions.GetCCDValue(cddMake.SelectedValue)
                ViewState("sModel") = commonFunctions.GetCCDValue(cddModel.SelectedValue)
                ViewState("sCustomer") = txtCustomer.Text
                ViewState("sFAC") = commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)
                ViewState("sDPT") = commonFunctions.GetCCDValue(cddDepartment.SelectedValue)
                ViewState("sWC") = commonFunctions.GetCCDValue(cddWorkCenter.SelectedValue)
                ViewState("sPNO") = txtPartNo.Text
            End If

            'Set a value to CurrentPage
            If HttpContext.Current.Session("SortExpPKG") IsNot Nothing Then
                GridViewSortExpression = HttpContext.Current.Session("SortExpPKG")
                GridViewSortDirection = HttpContext.Current.Session("SortDirPKG")
                gvLayout.Sort(GridViewSortExpression, GridViewSortDirection)
            End If

            If HttpContext.Current.Session("sessionPKGCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionPKGCurrentPage")
                gvLayout.PageIndex = HttpContext.Current.Session("sessionPKGCurrentPage")
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
    End Sub

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
            Dim iFormID As Integer = 50 'Packaging Layout Form ID
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
#End Region

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            HttpContext.Current.Session("sessionPKGCurrentPage") = Nothing
            HttpContext.Current.Session("SortExpPKG") = Nothing
            HttpContext.Current.Session("SortDirPKG") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("PL_LDESC").Value = txtDescription.Text
            Response.Cookies("PL_CNO").Value = txtContainerNo.Text
            Response.Cookies("PL_OEMMFG").Value = commonFunctions.GetCCDValue(cddOEMMfg.SelectedValue)
            Response.Cookies("PL_MAKE").Value = commonFunctions.GetCCDValue(cddMake.SelectedValue)
            Response.Cookies("PL_MODEL").Value = commonFunctions.GetCCDValue(cddModel.SelectedValue)
            Response.Cookies("PL_Customer").Value = txtCustomer.Text
            Response.Cookies("PL_FAC").Value = commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)
            Response.Cookies("PL_DPT").Value = commonFunctions.GetCCDValue(cddDepartment.SelectedValue)
            Response.Cookies("PL_WC").Value = commonFunctions.GetCCDValue(cddWorkCenter.SelectedValue)
            Response.Cookies("PL_PNO").Value = txtContainerNo.Text

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

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            PKGModule.DeletePKGLayoutCookies()
            HttpContext.Current.Session("sessionPkgCurrentPage") = Nothing
            HttpContext.Current.Session("SortExpPKG") = Nothing
            HttpContext.Current.Session("SortDirPKG") = Nothing


            Response.Redirect("PackagingList.aspx", False)
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

            Response.Redirect("Packaging.aspx", False)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnAdd_Click

    'Protected Sub BindCriteria()
    '    Try
    '        Dim ds As DataSet = New DataSet

    '        ' ''bind existing data to drop down Customer control for selection criteria for search
    '        ds = commonFunctions.GetOEMManufacturer("")
    '        If (ds.Tables.Item(0).Rows.Count > 0) Then
    '            ddCustomer.DataSource = ds
    '            ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
    '            ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
    '            ddCustomer.DataBind()
    '            ddCustomer.Items.Insert(0, "")
    '        End If

    '    Catch ex As Exception
    '        'update error on web page
    '        lblMessage.Text = ex.Message
    '        lblMessage.Visible = True

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try
    'End Sub 'EOF BindCriteria

#Region "GridView Wrokaround"
    Protected Sub gvLayout_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLayout.RowCreated

        If e.Row.RowType = DataControlRowType.Header Then
            AddSortImage(e.Row)
        End If

    End Sub 'EOF gvLayout_RowCreated

    Private Sub AddSortImage(ByVal headerRow As GridViewRow)
        Dim selCol As Integer = GetSortColumnIndex(HttpContext.Current.Session("SortExpPKG"))

        If -1 = selCol Then
            Return
        End If

        ' Create the sorting image based on the sort direction
        Dim sortImage As New System.Web.UI.WebControls.Image()
        If selCol > 0 Then
            If System.Web.UI.WebControls.SortDirection.Ascending = HttpContext.Current.Session("SortDirPKG") Then
                'sortImage.ImageUrl = "~/images/collapse.jpg"
                sortImage.ImageUrl = "~/images/red up.jpg"
                sortImage.AlternateText = "Ascending"
            Else
                'sortImage.ImageUrl = "~/images/expand.jpg"
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
        For Each field As DataControlField In gvLayout.Columns
            If field.SortExpression = strCol Then
                Return gvLayout.Columns.IndexOf(field)
            End If
        Next

        Return -1
    End Function 'EOF GetSortColumnIndex

#End Region 'EOF Gridview Work around

#Region "Multi Print Routine"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) _
Handles btnPrint.Click
        Dim PrintSelections As Boolean = False
        Dim RecIds As String = Nothing

        For Each row As GridViewRow In gvLayout.Rows
            Dim cb As CheckBox = row.FindControl("PrintSelector")
            If cb IsNot Nothing AndAlso cb.Checked Then
                PrintSelections = True
                ' First, get the RecID for the selected row         
                Dim recID As Integer = gvLayout.DataKeys(row.RowIndex).Value

                PrintResults.Text &= "<br />"
                PrintResults.Text &= String.Format("RecID# {0} sent to preview... ", recID)
                RecIds &= recID & ","
            End If
        Next

        ''Send to preview PDF
        'ScriptManager.RegisterStartupScript(Me, GetType(String), "key1", "open('TMRequisitions/crViewPackagingLayout.aspx?hm=1&pPKGID=" & Microsoft.VisualBasic.Left(RecIds, Len(RecIds) - 1) & "');" & vbLf, True)

        ' Show the Label if at least one row was printed  
        PrintResults.Visible = PrintSelections
    End Sub 'EOF btnPrint_Click

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        For Each rowItem As GridViewRow In gvLayout.Rows
            chk = CType(rowItem.Cells(0).FindControl("PrintSelector"), CheckBox)
            chk.Checked = CType(sender, CheckBox).Checked
        Next
        PrintResults.Text = Nothing
    End Sub 'EOF chkSelectAll_CheckedChanged
#End Region 'EOF Multi Print Routine

#Region "Paging Routine"
    Private Sub BindData()

        Try
            lblMessage.Text = ""

            Dim ds As DataSet = New DataSet
            ds = PKGModule.GetPKGLayoutSearch("", ViewState("sLDesc"), ViewState("sCNO"), ViewState("sOEMMfg"), ViewState("sMake"), ViewState("sModel"), ViewState("sFAC"), IIf(ViewState("sDPT") = Nothing, 0, ViewState("sDPT")), IIf(ViewState("sWC") = Nothing, 0, ViewState("sWC")), ViewState("sCustomer"), ViewState("sPNO"))

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
                        'lblFromRec.Text = (((CurrentPage + 1) * 30) - 30) + 1
                        'lblToRec.Text = (CurrentPage + 1) * 30
                        lblFromRec.Text = (((CurrentPage + 1) * 3) - 3) + 1 ''Use to test gridview with less # of recs
                        lblToRec.Text = (CurrentPage + 1) * 3 ''Use to test gridview with less # of recs
                        If lblToRec.Text > objPds.DataSourceCount Then
                            lblToRec.Text = objPds.DataSourceCount
                        End If
                    Else
                        lblFromRec.Text = ds.Tables.Count
                        'lblToRec.Text = (CurrentPage + 1) * 30
                        lblToRec.Text = (CurrentPage + 1) * 3 ''Use to test gridview with less # of recs
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

    Protected Sub gvLayout_RowDataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLayout.DataBound

        PagingInformation.Text = String.Format("Page {0} of {1}...   Go to ", _
                                               gvLayout.PageIndex + 1, gvLayout.PageCount)

        ' Clear out all of the items in the DropDownList
        PageList.Items.Clear()

        ' Add a ListItem for each page
        For i As Integer = 0 To gvLayout.PageCount - 1

            ' Add the new ListItem   
            Dim pageListItem As New ListItem(String.Concat("Page ", i + 1), i.ToString())
            PageList.Items.Add(pageListItem)
            ' select the current item, if needed   
            If i = gvLayout.PageIndex Then
                pageListItem.Selected = True
            End If
        Next
    End Sub 'EOF gvLayout_DataBound

    Protected Sub PageList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageList.SelectedIndexChanged
        ' Jump to the specified page       
        gvLayout.PageIndex = Convert.ToInt32(PageList.SelectedValue)
        Try
            ' Set viewstate variable to the next page
            CurrentPage = gvLayout.PageIndex
            HttpContext.Current.Session("sessionPKGCurrentPage") = gvLayout.PageIndex

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

    End Sub 'PageList_SelectIndexChanged

    Protected Sub gvLayout_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLayout.PageIndexChanged

        CurrentPage = gvLayout.PageIndex
        HttpContext.Current.Session("sessionPKGCurrentPage") = gvLayout.PageIndex

        ' Reload control
        BindData()
    End Sub 'EOF gvLayout_PageIndexChanged

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

#End Region 'EOF Paging Routine

#Region "Sorting Routine"

    Protected Sub gvLayout_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvLayout.Sorting
        ''This is invoked when the grid column is Clicked for Sorting, 
        ''Clicking again will Toggle Descending/Ascending through the Sort Expression

        GridViewSortExpression = e.SortExpression
        GridViewSortDirection = IIf(e.SortDirection = 0, "ASC", "DESC")

        SortInformationLabel.Text = "Sort By: " & GridViewSortExpression & " in " & GridViewSortDirection & " order."
        HttpContext.Current.Session("SortExpPKG") = GridViewSortExpression
        HttpContext.Current.Session("SortDirPKG") = e.SortDirection

    End Sub 'EOF gvLayout_Sorting

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
