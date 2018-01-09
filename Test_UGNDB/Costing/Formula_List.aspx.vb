' ************************************************************************************************
'
' Name:		Formula_List.aspx
' Purpose:	This Code Behind is to maintain the capital list used by the Costing Module
'
' Date		Author	    
' 10/13/2008 Roderick Carlson
' 06/22/1010 Roderick Carlson   Modified: Use CostingDepartmentList
' ************************************************************************************************
Partial Class Formula_List
    Inherits System.Web.UI.Page

    Protected WithEvents lnkFormulaName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDrawingNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartRevision As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDepartment As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkProcess As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkTemplate As System.Web.UI.WebControls.LinkButton

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = CostingModule.GetFormulaSearch(0, ViewState("FormulaName"), ViewState("DrawingNo"), _
           ViewState("PartNo"), ViewState("PartName"), ViewState("DepartmentID"), _
           ViewState("ProcessID"), ViewState("TemplateID"))

            If commonFunctions.CheckDataSet(ds) = True Then

                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpFormula.DataSource = dv
                rpFormula.DataBind()

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
       Handles lnkFormulaName.Click, lnkDrawingNo.Click, lnkPartNo.Click, lnkPartRevision.Click, lnkDepartment.Click, lnkProcess.Click, lnkTemplate.Click

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
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 68)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                            ViewState("isRestricted") = False
                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isEdit") = True
                            ViewState("isRestricted") = False
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                            ViewState("isRestricted") = True
                    End Select
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
            ViewState("_CurrentPage") = value
        End Set

    End Property
    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            lblMessage.Text = ""

            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionFormulaCurrentPage") = CurrentPage

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

    End Sub
    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            lblMessage.Text = ""

            If txtGoToPage.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If


                HttpContext.Current.Session("sessionFormulaCurrentPage") = CurrentPage

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

    End Sub
    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            lblMessage.Text = ""

            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionFormulaCurrentPage") = CurrentPage

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

    End Sub
    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            lblMessage.Text = ""

            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionFormulaCurrentPage") = CurrentPage

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

    End Sub
    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try

            lblMessage.Text = ""

            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionFormulaCurrentPage") = CurrentPage

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

    End Sub
    Protected Sub EnableControls()

        Try
            rpFormula.Visible = Not ViewState("isRestricted")
            'gvFormula.Visible = Not ViewState("isRestricted")
            lblSearchTip.Visible = Not ViewState("isRestricted")
            lblSearchFormulaName.Visible = Not ViewState("isRestricted")
            txtSearchFormulaName.Visible = Not ViewState("isRestricted")
            'lblSearchPartName.Visible = Not ViewState("isRestricted")
            'txtSearchPartName.Visible = Not ViewState("isRestricted")
            lblSearchDrawingNo.Visible = Not ViewState("isRestricted")
            txtSearchDrawingNo.Visible = Not ViewState("isRestricted")
            'lblSearchPartNo.Visible = Not ViewState("isRestricted")
            'txtSearchPartNo.Visible = Not ViewState("isRestricted")
            lblSearchDepartment.Visible = Not ViewState("isRestricted")
            ddSearchDepartment.Visible = Not ViewState("isRestricted")
            lblSearchProcess.Visible = Not ViewState("isRestricted")
            ddSearchProcess.Visible = Not ViewState("isRestricted")
            lblSearchTemplate.Visible = Not ViewState("isRestricted")
            ddSearchTemplate.Visible = Not ViewState("isRestricted")

            btnReset.Visible = Not ViewState("isRestricted")
            btnSearch.Visible = Not ViewState("isRestricted")
            lblReview1.Visible = Not ViewState("isRestricted")
            lblReview2.Visible = Not ViewState("isRestricted")
            btnAdd.Visible = Not ViewState("isRestricted")

            cmdFirst.Visible = Not ViewState("isRestricted")
            cmdNext.Visible = Not ViewState("isRestricted")
            txtGoToPage.Visible = Not ViewState("isRestricted")
            cmdGo.Visible = Not ViewState("isRestricted")
            cmdPrev.Visible = Not ViewState("isRestricted")
            cmdLast.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then

                btnAdd.Enabled = ViewState("isAdmin")

            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
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
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            'bind existing data to drop down Department 
            'ds = commonFunctions.GetDepartment("", "", False)
            ds = CostingModule.GetCostingDepartmentList("", "", False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchDepartment.DataSource = ds
                ddSearchDepartment.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName
                ddSearchDepartment.DataValueField = ds.Tables(0).Columns("DepartmentID").ColumnName
                ddSearchDepartment.DataBind()
                ddSearchDepartment.Items.Insert(0, "")
            End If

            'bind existing data to drop down Density 
            ds = CostingModule.GetProcess(0, "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProcess.DataSource = ds
                ddSearchProcess.DataTextField = ds.Tables(0).Columns("ddProcessName").ColumnName
                ddSearchProcess.DataValueField = ds.Tables(0).Columns("ProcessID").ColumnName
                ddSearchProcess.DataBind()
                ddSearchProcess.Items.Insert(0, "")
            End If

            'bind existing data to drop down Density 
            ds = CostingModule.GetTemplate(0, "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchTemplate.DataSource = ds
                ddSearchTemplate.DataTextField = ds.Tables(0).Columns("ddTemplateName").ColumnName
                ddSearchTemplate.DataValueField = ds.Tables(0).Columns("TemplateID").ColumnName
                ddSearchTemplate.DataBind()
                ddSearchTemplate.Items.Insert(0, "")
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
    Private Sub BindData()

        Try

            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = CostingModule.GetFormulaSearch(0, ViewState("FormulaName"), ViewState("DrawingNo"), _
            ViewState("PartNo"), ViewState("PartName"), ViewState("DepartmentID"), _
            ViewState("ProcessID"), ViewState("TemplateID"))

            If ViewState("isRestricted") = False Then
                If commonFunctions.CheckDataSet(ds) = True Then
                    rpFormula.DataSource = ds
                    rpFormula.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 15

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpFormula.DataSource = objPds
                    rpFormula.DataBind()

                    lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                    ViewState("LastPageCount") = objPds.PageCount - 1
                    txtGoToPage.Text = CurrentPage + 1

                    ' Disable Prev or Next buttons if necessary
                    cmdFirst.Enabled = Not objPds.IsFirstPage
                    cmdPrev.Enabled = Not objPds.IsFirstPage
                    cmdNext.Enabled = Not objPds.IsLastPage
                    cmdLast.Enabled = Not objPds.IsLastPage
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

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Formula List"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Formula List "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            CheckRights()

            If HttpContext.Current.Session("sessionFormulaCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionFormulaCurrentPage")
            End If

            If Not Page.IsPostBack Then
                'clear crystal reports
                CostingModule.CleanCostingCrystalReports()

                ViewState("FormulaName") = ""
                ViewState("DrawingNo") = ""
                ViewState("PartNo") = ""
                ViewState("PartName") = ""
                ViewState("DepartmentID") = 0
                ViewState("ProcessID") = 0
                ViewState("TemplateID") = 0

                BindCriteria()


                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******

                If HttpContext.Current.Request.QueryString("FormulaName") <> "" Then
                    txtSearchFormulaName.Text = HttpContext.Current.Request.QueryString("FormulaName")
                    ViewState("FormulaName") = HttpContext.Current.Request.QueryString("FormulaName")
                Else
                    If Not Request.Cookies("CostingModule_SaveFormulaNameSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveFormulaNameSearch").Value) <> "" Then
                            txtSearchFormulaName.Text = Request.Cookies("CostingModule_SaveFormulaNameSearch").Value
                            ViewState("FormulaName") = Request.Cookies("CostingModule_SaveFormulaNameSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtSearchDrawingNo.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                Else
                    If Not Request.Cookies("CostingModule_SaveFormulaDrawingNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveFormulaDrawingNoSearch").Value) <> "" Then
                            txtSearchDrawingNo.Text = Request.Cookies("CostingModule_SaveFormulaDrawingNoSearch").Value
                            ViewState("DrawingNo") = Request.Cookies("CostingModule_SaveFormulaDrawingNoSearch").Value
                        End If
                    End If
                End If

                ' ''If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                ' ''    txtSearchPartNo.Text = HttpContext.Current.Request.QueryString("PartNo")
                ' ''    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                ' ''Else
                ' ''    If Not Request.Cookies("CostingModule_SaveFormulaPartNoSearch") Is Nothing Then
                ' ''        If Trim(Request.Cookies("CostingModule_SaveFormulaPartNoSearch").Value) <> "" Then
                ' ''            txtSearchPartNo.Text = Request.Cookies("CostingModule_SaveFormulaPartNoSearch").Value
                ' ''            ViewState("PartNo") = Request.Cookies("CostingModule_SaveFormulaPartNoSearch").Value
                ' ''        End If
                ' ''    End If
                ' ''End If

                ' ''If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                ' ''    txtSearchPartName.Text = HttpContext.Current.Request.QueryString("PartName")
                ' ''    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                ' ''Else
                ' ''    If Not Request.Cookies("CostingModule_SaveFormulaPartNoSearch") Is Nothing Then
                ' ''        If Trim(Request.Cookies("CostingModule_SaveFormulaPartNoSearch").Value) <> "" Then
                ' ''            txtSearchPartNo.Text = Request.Cookies("CostingModule_SaveFormulaPartNoSearch").Value
                ' ''            ViewState("PartNo") = Request.Cookies("CostingModule_SaveFormulaPartNoSearch").Value
                ' ''        End If
                ' ''    End If
                ' ''End If

                If HttpContext.Current.Request.QueryString("DepartmentID") <> "" Then
                    ddSearchDepartment.SelectedValue = HttpContext.Current.Request.QueryString("DepartmentID")
                    ViewState("DepartmentID") = HttpContext.Current.Request.QueryString("DepartmentID")
                Else
                    If Not Request.Cookies("CostingModule_SaveFormulaDepartmentIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveFormulaDepartmentIDSearch").Value) <> "" Then
                            ddSearchDepartment.SelectedValue = Request.Cookies("CostingModule_SaveFormulaDepartmentIDSearch").Value
                            ViewState("DepartmentID") = Request.Cookies("CostingModule_SaveFormulaDepartmentIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ProcessID") <> "" Then
                    ddSearchProcess.SelectedValue = HttpContext.Current.Request.QueryString("ProcessID")
                    ViewState("ProcessID") = HttpContext.Current.Request.QueryString("ProcessID")
                Else
                    If Not Request.Cookies("CostingModule_SaveFormulaProcessIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveFormulaProcessIDSearch").Value) <> "" Then
                            ddSearchProcess.SelectedValue = Request.Cookies("CostingModule_SaveFormulaProcessIDSearch").Value
                            ViewState("ProcessID") = Request.Cookies("CostingModule_SaveFormulaProcessIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("TemplateID") <> "" Then
                    ddSearchTemplate.SelectedValue = HttpContext.Current.Request.QueryString("TemplateID")
                    ViewState("TemplateID") = HttpContext.Current.Request.QueryString("TemplateID")
                Else
                    If Not Request.Cookies("CostingModule_SaveFormulaTemplateIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveFormulaTemplateIDSearch").Value) <> "" Then
                            ddSearchTemplate.SelectedValue = Request.Cookies("CostingModule_SaveFormulaTemplateIDSearch").Value
                            ViewState("TemplateID") = Request.Cookies("CostingModule_SaveFormulaTemplateIDSearch").Value
                        End If
                    End If
                End If

                BindData()
            Else
                ViewState("FormulaName") = txtSearchFormulaName.Text
                ViewState("DrawingNo") = txtSearchDrawingNo.Text
                ' ''ViewState("PartNo") = txtSearchPartNo.Text
                ' ''ViewState("PartName") = txtSearchPartName.Text

                If ddSearchDepartment.SelectedIndex > 0 Then
                    ViewState("DepartmentID") = ddSearchDepartment.SelectedValue
                End If

                If ddSearchProcess.SelectedIndex > 0 Then
                    ViewState("ProcessID") = ddSearchProcess.SelectedValue
                End If

                If ddSearchTemplate.SelectedIndex > 0 Then
                    ViewState("TemplateID") = ddSearchTemplate.SelectedValue
                End If

            End If

            EnableControls()

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

            HttpContext.Current.Session("sessionFormulaCurrentPage") = Nothing

            Response.Cookies("CostingModule_SaveFormulaNameSearch").Value = txtSearchFormulaName.Text.Trim
            Response.Cookies("CostingModule_SaveFormulaDrawingNoSearch").Value = txtSearchDrawingNo.Text.Trim
            ' ''Response.Cookies("CostingModule_SaveFormulaPartNoSearch").Value = txtSearchPartNo.Text.Trim
            ' ''Response.Cookies("CostingModule_SaveFormulaPartNameSearch").Value = txtSearchPartName.Text.Trim

            If ddSearchDepartment.SelectedIndex > 0 Then
                Response.Cookies("CostingModule_SaveFormulaDepartmentIDSearch").Value = ddSearchDepartment.SelectedValue
            Else
                Response.Cookies("CostingModule_SaveFormulaDepartmentIDSearch").Value = ""
                Response.Cookies("CostingModule_SaveFormulaDepartmentIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchProcess.SelectedIndex > 0 Then
                Response.Cookies("CostingModule_SaveFormulaProcessIDSearch").Value = ddSearchProcess.SelectedValue
            Else
                Response.Cookies("CostingModule_SaveFormulaProcessIDSearch").Value = ""
                Response.Cookies("CostingModule_SaveFormulaProcessIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchTemplate.SelectedIndex > 0 Then
                Response.Cookies("CostingModule_SaveFormulaTemplateIDSearch").Value = ddSearchTemplate.SelectedValue
            Else
                Response.Cookies("CostingModule_SaveFormulaTemplateIDSearch").Value = ""
                Response.Cookies("CostingModule_SaveFormulaTemplateIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            'Response.Redirect("Formula_List.aspx?FormulaName=" & Server.UrlEncode(txtSearchFormulaName.Text.Trim) _
            '& "&DrawingNo=" & Server.UrlEncode(txtSearchDrawingNo.Text.Trim) _
            '& "&PartNo=" & Server.UrlEncode(txtSearchPartNo.Text.Trim) _
            '& "&PartName=" & Server.UrlEncode(txtSearchPartName.Text.Trim) _
            '& "&DepartmentID=" & ddSearchDepartment.SelectedValue _
            '& "&ProcessID=" & ddSearchProcess.SelectedValue _
            '& "&TemplateID=" & ddSearchTemplate.SelectedValue, False)

            Response.Redirect("Formula_List.aspx?FormulaName=" & Server.UrlEncode(txtSearchFormulaName.Text.Trim) _
           & "&DrawingNo=" & Server.UrlEncode(txtSearchDrawingNo.Text.Trim) _
           & "&PartNo=&PartName=&DepartmentID=" & ddSearchDepartment.SelectedValue _
           & "&ProcessID=" & ddSearchProcess.SelectedValue _
           & "&TemplateID=" & ddSearchTemplate.SelectedValue, False)

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

            CostingModule.DeleteFormulaCookies()

            HttpContext.Current.Session("sessionFormulaCurrentPage") = Nothing

            Response.Redirect("Formula_List.aspx", False)

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

            Response.Redirect("Formula_Maint.aspx", False)

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
