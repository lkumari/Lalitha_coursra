' ************************************************************************************************
'
' Name:		Support_List.aspx
' Purpose:	This Code Behind is for the Workflow Support List page. 
'
' Date		    Author	    
' 1/09/2012    Roderick Carlson
'
' ************************************************************************************************
Partial Class Support_List
    Inherits System.Web.UI.Page

    Protected WithEvents lnkJobNumber As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkStatus As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRequestBy As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRequestDate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkModule As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCategory As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkJobDescription As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDateCompleted As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkAssignedTo As System.Web.UI.WebControls.LinkButton

    Private htControls As New System.Collections.Hashtable

    Protected Function SetBackGroundColor(ByVal Status As String) As String

        Dim strReturnValue As String = "White" 'N/A or Complete or Closed

        Try
            Select Case Status
                Case "Open"
                    strReturnValue = "Fuchsia"
                Case "In Process"
                    strReturnValue = "Yellow"
                Case "Hold"
                    strReturnValue = "Blue"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetBackGroundColor = strReturnValue

    End Function

    Protected Function SetForeGroundColor(ByVal Status As String) As String

        Dim strReturnValue As String = "Black" 'default

        Try
            Select Case Status
                Case "Open", "Hold"
                    strReturnValue = "White"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetForeGroundColor = strReturnValue

    End Function

    Protected Function SetDetailHyperlink(ByVal JobNumber As String) As String

        Dim strReturnValue As String = ""

        Try

            strReturnValue = "Support_Detail.aspx?JobNumber=" & JobNumber

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetDetailHyperlink = strReturnValue

    End Function

    Protected Function SetPreviewHyperLink(ByVal JobNumber As String) As String

        Dim strReturnValue As String = ""

        Try
            strReturnValue = "javascript:void(window.open('crSupport_Preview.aspx?JobNumber=" & JobNumber & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewHyperLink = strReturnValue

    End Function

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            ds = SupportModule.GetSupportSearch(ViewState("JobNumber"), ViewState("DBCID"), ViewState("DBMID"), ViewState("Status"), ViewState("RelatedTo"), ViewState("RequestBy"), ViewState("JobDescription"), ViewState("AssignedTo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpInfo.DataSource = dv
                rpInfo.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()            
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

    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
       Handles lnkJobNumber.Click, lnkAssignedTo.Click, lnkCategory.Click, lnkDateCompleted.Click, lnkJobDescription.Click, lnkModule.Click, lnkRequestBy.Click, lnkRequestDate.Click, lnkStatus.Click

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
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master

            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Support List Search"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Work Flow</b> > Support List"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("WFExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False
            ' End If

            'update Support Link
            Dim hlnkSupport As HtmlAnchor = CType(Master.FindControl("hlnkSupportDetail"), HtmlAnchor)
            If hlnkSupport IsNot Nothing Then
                Session("SupportUrl") = Request.ServerVariables("URL")
                Session("SupportQueryString") = Request.ServerVariables("QUERY_STRING")

                'hlnkSupport.Attributes.Remove("href")
                'hlnkSupport.Attributes.Add("onclick", "javascript:void(window.open('../WorkFlow/Support_Detail_Popup.aspx?BMID=SAF','_blank','top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));")

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
            ViewState("_CurrentPage") = Value
        End Set

    End Property

    Private Sub CheckRights()

        Try

            ViewState("TeamMemberID") = 0

            Dim FullName As String = commonFunctions.getUserName()

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Or Response.Cookies("UGNDB_UserFullName") Is Nothing Then
                Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                If FullName = Nothing Then
                    FullName = "Demo.Demo"  '* This account has restricted read only rights.
                End If
                Dim LocationOfDot As Integer = InStr(FullName, ".")
                If LocationOfDot > 0 Then
                    Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                    Dim FirstInitial As String = Left(FullName, 1)
                    Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                    Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                    Response.Cookies("UGNDB_UserFullName").Value = Mid(FirstName, 1, 1).ToUpper & Mid(FirstName, 2, Len(FirstName) - 1) & " " & Mid(LastName, 1, 1).ToUpper & Mid(LastName, 2, Len(LastName) - 1)
                Else
                    Response.Cookies("UGNDB_User").Value = FullName
                    Response.Cookies("UGNDB_UserFullName").Value = FullName

                End If
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim ds As DataSet = New DataSet
            Dim TMWorking As Boolean = False

            ds = SecurityModule.GetTeamMember(Nothing, FullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''ds = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If commonFunctions.CheckDataSet(ds) = True Then

                If ds.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                    ViewState("TeamMemberID") = ds.Tables(0).Rows(0).Item("TeamMemberID")
                End If
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

    'Protected Sub EnableControls()

    '    Try

    '        btnAdd.Enabled = True

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub
    Private Sub BindData()

        Try
            'If ViewState("TeamMemberID") > 0 Then
            '    If ddTeamMember.Items.FindByValue(ViewState("TeamMemberID")) IsNot Nothing Then
            '        ddTeamMember.SelectedValue = ViewState("TeamMemberID")
            '    End If
            'Else
            '    ddTeamMember.Enabled = True
            'End If

            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = SupportModule.GetSupportSearch(ViewState("JobNumber"), ViewState("DBCID"), ViewState("DBMID"), ViewState("Status"), ViewState("RelatedTo"), ViewState("RequestBy"), ViewState("JobDescription"), ViewState("AssignedTo"))

            rpInfo.Visible = False
            tblPageNavigation.Visible = False

            If commonFunctions.CheckDataSet(ds) = True Then
                rpInfo.Visible = True
                tblPageNavigation.Visible = True

                rpInfo.DataSource = ds
                rpInfo.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 15

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpInfo.DataSource = objPds
                rpInfo.DataBind()

                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                ViewState("LastPageCount") = objPds.PageCount - 1
                txtGoToPage.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdFirst.Enabled = Not objPds.IsFirstPage
                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdNext.Enabled = Not objPds.IsLastPage
                cmdLast.Enabled = Not objPds.IsLastPage
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'clear crystal reports
        'RFDModule.CleanRFDCrystalReports()

        If HttpContext.Current.Session("sessionSupportCurrentPage") IsNot Nothing Then
            CurrentPage = HttpContext.Current.Session("sessionSupportCurrentPage")
        End If

        If Not Page.IsPostBack Then

            CheckRights()

            ViewState("lnkJobNumber") = "DESC"
            ViewState("lnkStatus") = "ASC"
            ViewState("lnkRequestBy") = "ASC"
            ViewState("lnkRequestDate") = "ASC"
            ViewState("lnkModule") = "ASC"
            ViewState("lnkCategory") = "ASC"
            ViewState("lnkJobDescription") = "ASC"
            ViewState("lnkDateCompleted") = "ASC"
            ViewState("lnkAssignedTo") = "ASC"

            ViewState("JobNumber") = ""
            ViewState("Status") = ""
            ViewState("RequestBy") = ""
            ViewState("RequestDate") = ""
            ViewState("DBMID") = ""
            ViewState("DBCID") = 0
            ViewState("JobDescription") = ""
            ViewState("DateCompleted") = ""
            ViewState("AssignedTo") = ""
            ViewState("RelatedTo") = ""

            '' ''******
            '' '' Bind drop down lists
            '' ''******
            BindCriteria()

            '' ''******
            ' ''get saved value of past search criteria or query string, query string takes precedence
            '' ''******

            If HttpContext.Current.Request.QueryString("JobNumber") <> "" Then
                txtSearchJobNumber.Text = HttpContext.Current.Request.QueryString("JobNumber")
                ViewState("JobNumber") = HttpContext.Current.Request.QueryString("JobNumber")
            Else
                If Not Request.Cookies("SupportModule_SaveJobNumberSearch") Is Nothing Then
                    If Trim(Request.Cookies("SupportModule_SaveJobNumberSearch").Value) <> "" Then
                        txtSearchJobNumber.Text = Request.Cookies("SupportModule_SaveJobNumberSearch").Value
                        ViewState("JobNumber") = Request.Cookies("SupportModule_SaveJobNumberSearch").Value
                    End If
                End If
            End If

            If HttpContext.Current.Request.QueryString("Status") <> "" Then
                ddSearchStatus.SelectedValue = HttpContext.Current.Request.QueryString("Status")
                ViewState("Status") = HttpContext.Current.Request.QueryString("Status")
            Else
                If Not Request.Cookies("SupportModule_SaveStatusSearch") Is Nothing Then
                    If Trim(Request.Cookies("SupportModule_SaveStatusSearch").Value) <> "" Then
                        ddSearchStatus.SelectedValue = Request.Cookies("SupportModule_SaveStatusSearch").Value
                        ViewState("Status") = Request.Cookies("SupportModule_SaveStatusSearch").Value
                    End If
                End If
            End If

            If HttpContext.Current.Request.QueryString("CategoryID") <> "" Then
                ddSearchCategory.SelectedValue = HttpContext.Current.Request.QueryString("CategoryID")
                ViewState("DBCID") = CType(HttpContext.Current.Request.QueryString("CategoryID"), Integer)
            Else
                If Not Request.Cookies("SupportModule_SaveCategoryIDSearch") Is Nothing Then
                    If Trim(Request.Cookies("SupportModule_SaveCategoryIDSearch").Value) <> "" Then
                        ddSearchCategory.SelectedValue = Request.Cookies("SupportModule_SaveCategoryIDSearch").Value
                        ViewState("DBCID") = CType(Request.Cookies("SupportModule_SaveCategoryIDSearch").Value, Integer)
                    End If
                End If
            End If

            If HttpContext.Current.Request.QueryString("RelatedTo") <> "" Then
                ddSearchRelatedTo.SelectedValue = HttpContext.Current.Request.QueryString("RelatedTo")
                ViewState("RelatedTo") = HttpContext.Current.Request.QueryString("RelatedTo")
            Else
                If Not Request.Cookies("SupportModule_SaveRelatedToSearch") Is Nothing Then
                    If Trim(Request.Cookies("SupportModule_SaveRelatedToSearch").Value) <> "" Then
                        ddSearchRelatedTo.SelectedValue = Request.Cookies("SupportModule_SaveRelatedToSearch").Value
                        ViewState("RelatedTo") = Request.Cookies("SupportModule_SaveRelatedToSearch").Value
                    End If
                End If
            End If

            If HttpContext.Current.Request.QueryString("RequestBy") <> "" Then
                txtSearchRequestBy.Text = HttpContext.Current.Request.QueryString("RequestBy")
                ViewState("RequestBy") = HttpContext.Current.Request.QueryString("RequestBy")
            Else
                If Not Request.Cookies("SupportModule_SaveRequestBySearch") Is Nothing Then
                    If Trim(Request.Cookies("SupportModule_SaveRequestBySearch").Value) <> "" Then
                        txtSearchRequestBy.Text = Request.Cookies("SupportModule_SaveRequestBySearch").Value
                        ViewState("RequestBy") = Request.Cookies("SupportModule_SaveRequestBySearch").Value
                    End If
                End If
            End If

            If HttpContext.Current.Request.QueryString("ModuleID") <> "" Then
                ddSearchModule.SelectedValue = HttpContext.Current.Request.QueryString("ModuleID")
                ViewState("DBMID") = HttpContext.Current.Request.QueryString("ModuleID")
            Else
                If Not Request.Cookies("SupportModule_SaveModuleIDSearch") Is Nothing Then
                    If Trim(Request.Cookies("SupportModule_SaveModuleIDSearch").Value) <> "" Then
                        ddSearchModule.SelectedValue = Request.Cookies("SupportModule_SaveModuleIDSearch").Value
                        ViewState("DBMID") = Request.Cookies("SupportModule_SaveModuleIDSearch").Value
                    End If
                End If
            End If

            If HttpContext.Current.Request.QueryString("JobDescription") <> "" Then
                txtSearchJobDescription.Text = HttpContext.Current.Request.QueryString("JobDescription")
                ViewState("JobDescription") = HttpContext.Current.Request.QueryString("JobDescription")
            Else
                If Not Request.Cookies("SupportModule_SaveJobDescriptionSearch") Is Nothing Then
                    If Trim(Request.Cookies("SupportModule_SaveJobDescriptionSearch").Value) <> "" Then
                        txtSearchJobDescription.Text = Request.Cookies("SupportModule_SaveJobDescriptionSearch").Value
                        ViewState("JobDescription") = Request.Cookies("SupportModule_SaveJobDescriptionSearch").Value
                    End If
                End If
            End If

            If HttpContext.Current.Request.QueryString("AssignedTo") <> "" Then
                txtSearchAssignedTo.Text = HttpContext.Current.Request.QueryString("AssignedTo")
                ViewState("AssignedTo") = HttpContext.Current.Request.QueryString("AssignedTo")
            Else
                If Not Request.Cookies("SupportModule_SaveAssignedToSearch") Is Nothing Then
                    If Trim(Request.Cookies("SupportModule_SaveAssignedToSearch").Value) <> "" Then
                        txtSearchAssignedTo.Text = Request.Cookies("SupportModule_SaveAssignedToSearch").Value
                        ViewState("AssignedTo") = Request.Cookies("SupportModule_SaveAssignedToSearch").Value
                    End If
                End If
            End If

            '' ''******
            'load repeater control
            '' ''******
            BindData()
        Else
            If txtSearchJobNumber.Text.Trim <> "" Then
                ViewState("JobNumber") = txtSearchJobNumber.Text.Trim
            End If

            If ddSearchStatus.SelectedIndex > 0 Then
                ViewState("Status") = ddSearchStatus.SelectedValue
            End If

            If ddSearchCategory.SelectedIndex > 0 Then
                ViewState("DBCID") = CType(ddSearchCategory.SelectedValue, Integer)
            End If

            If ddSearchRelatedTo.SelectedIndex > 0 Then
                ViewState("RelatedTo") = ddSearchRelatedTo.SelectedValue
            End If

            If txtSearchRequestBy.Text.Trim <> "" Then
                ViewState("RequestBy") = txtSearchRequestBy.Text.Trim
            End If

            If ddSearchModule.SelectedIndex > 0 Then
                ViewState("DBMID") = ddSearchModule.SelectedValue
            End If

            If txtSearchJobDescription.Text.Trim <> "" Then
                ViewState("JobDescription") = txtSearchJobDescription.Text.Trim
            End If

            If txtSearchAssignedTo.Text.Trim <> "" Then
                ViewState("AssignedTo") = txtSearchAssignedTo.Text.Trim
            End If

            'focus on RFDNo field
            txtSearchJobNumber.Focus()
        End If

        ' EnableControls()

    End Sub

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            'ds = commonFunctions.GetTeamMember("")
            'If commonFunctions.CheckDataSet(ds) = True Then
            '    ddSearchTeamMember.DataSource = ds
            '    ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName
            '    ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
            '    ddTeamMember.DataBind()
            '    ddTeamMember.Items.Insert(0, "")
            'End If

            ds = SupportModule.GetModule("", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchModule.DataSource = ds
                ddSearchModule.DataTextField = ds.Tables(0).Columns("Description").ColumnName
                ddSearchModule.DataValueField = ds.Tables(0).Columns("DBMID").ColumnName
                ddSearchModule.DataBind()
                ddSearchModule.Items.Insert(0, "")
            End If

            ds = SupportModule.GetCategory("", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchCategory.DataSource = ds
                ddSearchCategory.DataTextField = ds.Tables(0).Columns("Category").ColumnName
                ddSearchCategory.DataValueField = ds.Tables(0).Columns("DBCID").ColumnName
                ddSearchCategory.DataBind()
                ddSearchCategory.Items.Insert(0, "")
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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try
            lblMessage.Text = ""

            Response.Redirect("Support_Detail.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            lblMessage.Text = ""

            HttpContext.Current.Session("sessionSupportCurrentPage") = Nothing

            '''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''set saved value of what criteria was used to search   
            '''''''''''''''''''''''''''''''''''''''''''''''''''''

            Response.Cookies("SupportModule_SaveJobNumberSearch").Value = txtSearchJobNumber.Text.Trim

            If ddSearchStatus.SelectedIndex > 0 Then
                Response.Cookies("SupportModule_SaveStatusSearch").Value = ddSearchStatus.SelectedValue
            Else
                Response.Cookies("SupportModule_SaveStatusSearch").Value = ""
                Response.Cookies("SupportModule_SaveStatusSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchCategory.SelectedIndex > 0 Then
                Response.Cookies("SupportModule_SaveCategoryIDSearch").Value = ddSearchCategory.SelectedValue
            Else
                Response.Cookies("SupportModule_SaveCategoryIDSearch").Value = 0
                Response.Cookies("SupportModule_SaveCategoryIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchRelatedTo.SelectedIndex > 0 Then
                Response.Cookies("SupportModule_SaveRelatedToSearch").Value = ddSearchRelatedTo.SelectedValue
            Else
                Response.Cookies("SupportModule_SaveRelatedToSearch").Value = ""
                Response.Cookies("SupportModule_SaveRelatedToSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("SupportModule_SaveRequestBySearch").Value = txtSearchRequestBy.Text.Trim

            If ddSearchModule.SelectedIndex > 0 Then
                Response.Cookies("SupportModule_SaveModuleIDSearch").Value = ddSearchModule.SelectedValue
            Else
                Response.Cookies("SupportModule_SaveModuleIDSearch").Value = ""
                Response.Cookies("SupportModule_SaveModuleIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("SupportModule_SaveJobDescriptionSearch").Value = txtSearchJobDescription.Text.Trim

            Response.Cookies("SupportModule_SaveAssignedToSearch").Value = txtSearchAssignedTo.Text.Trim

            Response.Redirect("Support_List.aspx?JobNumber=" & Server.UrlEncode(txtSearchJobNumber.Text.Trim) _
            & "&Status=" & ddSearchStatus.SelectedValue _
            & "&CategoryID=" & ddSearchCategory.SelectedValue _
            & "&RelatedTo=" & ddSearchRelatedTo.SelectedValue _
            & "&RequestBy=" & Server.UrlEncode(txtSearchRequestBy.Text.Trim) _
            & "&ModuleID=" & ddSearchModule.SelectedValue _
            & "&JobDescription=" & Server.UrlEncode(txtSearchJobDescription.Text.Trim) _
            & "&AssignedTo=" & Server.UrlEncode(txtSearchAssignedTo.Text.Trim) _
            , False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            SupportModule.DeleteSupportCookies()

            HttpContext.Current.Session("sessionSupportCurrentPage") = Nothing

            Response.Redirect("Support_List.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionSupportCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionSupportCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionSupportCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            If txtGoToPage.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If

                HttpContext.Current.Session("sessionSupportCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionSupportCurrentPage") = CurrentPage

            ' Reload control
            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS

    End Sub
    Private Sub PrepareGridViewForExport(ByRef gv As Control)

        Dim l As Literal = New Literal()
        Dim i As Integer


        For i = 0 To gv.Controls.Count

            If ((Nothing <> htControls(gv.Controls(i).GetType().Name)) Or (Nothing <> htControls(gv.Controls(i).GetType().BaseType.Name))) Then
                l.Text = GetControlPropertyValue(gv.Controls(i))

                gv.Controls.Remove(gv.Controls(i))

                gv.Controls.AddAt(i, l)

            End If

            If (gv.Controls(i).HasControls()) Then

                PrepareGridViewForExport(gv.Controls(i))

            End If

        Next

    End Sub
    Private Function GetControlPropertyValue(ByVal control As Control) As String
        Dim controlType As Type = control.[GetType]()
        Dim strControlType As String = controlType.Name
        Dim strReturn As String = "Error"
        Dim bReturn As Boolean

        Dim ctrlProps As System.Reflection.PropertyInfo() = controlType.GetProperties()
        Dim ExcelPropertyName As String = DirectCast(htControls(strControlType), String)

        If ExcelPropertyName Is Nothing Then
            ExcelPropertyName = DirectCast(htControls(control.[GetType]().BaseType.Name), String)
            If ExcelPropertyName Is Nothing Then
                Return strReturn
            End If
        End If

        For Each ctrlProp As System.Reflection.PropertyInfo In ctrlProps

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(String) Then
                Try
                    strReturn = DirectCast(ctrlProp.GetValue(control, Nothing), String)
                    Exit Try
                Catch
                    strReturn = ""
                End Try
            End If

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(Boolean) Then
                Try
                    bReturn = CBool(ctrlProp.GetValue(control, Nothing))
                    strReturn = IIf(bReturn, "True", "False")
                    Exit Try
                Catch
                    strReturn = "Error"
                End Try
            End If

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(ListItem) Then
                Try
                    strReturn = DirectCast((ctrlProp.GetValue(control, Nothing)), ListItem).Text
                    Exit Try
                Catch
                    strReturn = ""
                End Try
            End If
        Next
        Return strReturn
    End Function

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Try

            Dim attachment As String = "attachment; filename=SupportList.xls"

            Response.ClearContent()

            Response.AddHeader("content-disposition", attachment)

            Response.ContentType = "application/ms-excel"

            Dim sw As StringWriter = New StringWriter()

            Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)

            'EnablePartialRendering = False

            Dim ds As DataSet
            ds = SupportModule.GetSupportSearch(ViewState("JobNumber"), ViewState("DBCID"), ViewState("DBMID"), _
                ViewState("Status"), ViewState("RelatedTo"), ViewState("RequestBy"), _
                ViewState("JobDescription"), ViewState("AssignedTo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                Dim tempDataGridView As New GridView

                tempDataGridView.HeaderStyle.BackColor = Color.White 'System.Drawing.Color.LightGray
                tempDataGridView.HeaderStyle.ForeColor = Color.Black 'System.Drawing.Color.Black
                tempDataGridView.HeaderStyle.Font.Bold = True

                tempDataGridView.AutoGenerateColumns = False

                Dim JobNumberColumn As New BoundField
                JobNumberColumn.HeaderText = "Requestor ID"
                JobNumberColumn.DataField = "JobNumber"
                tempDataGridView.Columns.Add(JobNumberColumn)

                Dim CategoryColumn As New BoundField
                CategoryColumn.HeaderText = "Category"
                CategoryColumn.DataField = "Category"
                tempDataGridView.Columns.Add(CategoryColumn)

                Dim ModuleColumn As New BoundField
                ModuleColumn.HeaderText = "Module"
                ModuleColumn.DataField = "Module"
                tempDataGridView.Columns.Add(ModuleColumn)

                Dim RequestDateColumn As New BoundField
                RequestDateColumn.HeaderText = "Request Date"
                RequestDateColumn.DataField = "RequestDate"
                tempDataGridView.Columns.Add(RequestDateColumn)

                Dim RequestByColumn As New BoundField
                RequestByColumn.HeaderText = "Requested By"
                RequestByColumn.DataField = "RequestBy"
                tempDataGridView.Columns.Add(RequestByColumn)

                Dim AssignedToColumn As New BoundField
                AssignedToColumn.HeaderText = "Assigned To"
                AssignedToColumn.DataField = "AssignedTo"
                tempDataGridView.Columns.Add(AssignedToColumn)

                Dim DateCompletedColumn As New BoundField
                DateCompletedColumn.HeaderText = "Date Completed"
                DateCompletedColumn.DataField = "DateCompleted"
                tempDataGridView.Columns.Add(DateCompletedColumn)

                Dim JobDescriptionColumn As New BoundField
                JobDescriptionColumn.HeaderText = "Description"
                JobDescriptionColumn.DataField = "JobDescription"
                tempDataGridView.Columns.Add(JobDescriptionColumn)

                Dim RelatedToColumn As New BoundField
                RelatedToColumn.HeaderText = "Related To"
                RelatedToColumn.DataField = "RelatedTo"
                tempDataGridView.Columns.Add(RelatedToColumn)

                tempDataGridView.DataSource = ds
                tempDataGridView.DataBind()

                tempDataGridView.RenderControl(htw)

                Response.Write(sw.ToString())

                Response.End()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
