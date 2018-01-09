' ************************************************************************************************
' Name:	ChartSpecList.aspx.vb
' Purpose:	This program is used to view, insert, update Make
'
' Date		    Author	    
' 09/28/2011    LREY			Created .Net application
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
Partial Class MfgProd_ChartSpecList
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Production"
            m.ContentLabel = "Part Specification Search"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Manufacturing</b> > Part Specification Search"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False


            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pCSID") <> "" Then
                ViewState("pCSID") = HttpContext.Current.Request.QueryString("pCSID")
            Else
                ViewState("pCSID") = ""
            End If

            ''Used to determine what is the Form Transaction (Add, Edit or Search)
            If HttpContext.Current.Request.QueryString("pFT") <> "" Then
                ViewState("pFT") = HttpContext.Current.Request.QueryString("pFT")
            Else
                ViewState("pFT") = ""
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sFac") = ""
                ViewState("sOMfg") = ""
                ViewState("sCust") = ""
                ViewState("sPNo") = ""
                ViewState("sDept") = ""
                ViewState("sWrkCntr") = ""
                ViewState("sFormula") = ""
                ViewState("sRecStatus") = ""

                If Not Request.Cookies("MPR_FAC") Is Nothing Then
                    cddUGNLocation.SelectedValue = Server.HtmlEncode(Request.Cookies("MPR_FAC").Value)
                    ViewState("sFac") = Server.HtmlEncode(Request.Cookies("MPR_FAC").Value)
                End If

                If Not Request.Cookies("MPR_OMFG") Is Nothing Then
                    ddOEMMfg.SelectedValue = Server.HtmlEncode(Request.Cookies("MPR_OMFG").Value)
                    ViewState("sOMfg") = Server.HtmlEncode(Request.Cookies("MPR_OMFG").Value)
                End If

                If Not Request.Cookies("MPR_CUST") Is Nothing Then
                    txtCustLoc.Text = Server.HtmlEncode(Request.Cookies("MPR_CUST").Value)
                    ViewState("sCust") = Server.HtmlEncode(Request.Cookies("MPR_CUST").Value)
                End If

                If Not Request.Cookies("MPR_PNO") Is Nothing Then
                    txtPartNo.Text = Server.HtmlEncode(Request.Cookies("MPR_PNO").Value)
                    ViewState("sPNo") = Server.HtmlEncode(Request.Cookies("MPR_PNO").Value)
                End If

                If Not Request.Cookies("MPR_DEPT") Is Nothing Then
                    cddDepartment.SelectedValue = Server.HtmlEncode(Request.Cookies("MPR_DEPT").Value)
                    ViewState("sDept") = Server.HtmlEncode(Request.Cookies("MPR_DEPT").Value)
                End If

                If Not Request.Cookies("MPR_WC") Is Nothing Then
                    cddWorkCenter.SelectedValue = Server.HtmlEncode(Request.Cookies("MPR_WC").Value)
                    ViewState("sWrkCntr") = Server.HtmlEncode(Request.Cookies("MPR_WC").Value)
                End If

                If Not Request.Cookies("MPR_FORMULA") Is Nothing Then
                    txtFormula.Text = Server.HtmlEncode(Request.Cookies("MPR_FORMULA").Value)
                    ViewState("sFormula") = Server.HtmlEncode(Request.Cookies("MPR_FORMULA").Value)
                End If

                If Not Request.Cookies("MPR_RECSTATUS") Is Nothing Then
                    ddRecStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("MPR_RECSTATUS").Value)
                    ViewState("sRecStatus") = Server.HtmlEncode(Request.Cookies("MPR_RECSTATUS").Value)
                End If

                BindCriteria()
            Else
                ViewState("sFac") = ddUGNLocation.SelectedValue
                ViewState("sOMfg") = ddOEMMfg.SelectedValue
                ViewState("sCust") = txtCustLoc.Text
                ViewState("sPNo") = txtPartNo.Text
                ViewState("sDept") = ddDepartment.SelectedValue
                ViewState("sWrkCntr") = ddWorkCenter.SelectedValue
                ViewState("sFormula") = txtFormula.Text
                ViewState("sRecStatus") = ddRecStatus.SelectedValue
            End If

            CheckRights()

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF Page_Load

    Protected Sub CheckRights()
        Try
            'default to hide edit column
            FEExtender.Collapsed = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 129) 'Part Specification Form ID

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("ObjectRole") = True
                                    btnAdd.Enabled = True
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                    btnAdd.Enabled = True
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    btnAdd.Enabled = True
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    btnAdd.Enabled = False
                                    gvChartSpec.Columns(0).Visible = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    btnAdd.Enabled = False
                                    gvChartSpec.Columns(0).Visible = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    btnAdd.Enabled = False
                                    FEContentPanel.Visible = False
                                    FEPanel.Visible = False
                                    gvChartSpec.Columns(0).Visible = False
                            End Select
                        End If
                    End If
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

    End Sub 'EOF CheckRights

    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Part Designation Type control for selection criteria for search
            ds = commonFunctions.GetDesignationType()
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddDesignationType.DataSource = ds
                ddDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName.ToString()
                ddDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName.ToString()
                ddDesignationType.DataBind()
                ddDesignationType.Items.Insert(0, "")
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

#Region "Filter Chart Spec"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Cookies("MPR_FAC").Value = cddUGNLocation.SelectedValue
            Response.Cookies("MPR_OMFG").Value = ddOEMMfg.SelectedValue
            Response.Cookies("MPR_CUST").Value = txtCustLoc.Text
            Response.Cookies("MPR_PNO").Value = txtPartNo.Text
            Response.Cookies("MPR_DEPT").Value = cddDepartment.SelectedValue
            Response.Cookies("MPR_WC").Value = cddWorkCenter.SelectedValue
            Response.Cookies("MPR_FORMULA").Value = txtFormula.Text
            Response.Cookies("MPR_RECSTATUS").Value = ddRecStatus.SelectedValue

            Response.Redirect("ChartSpecList.aspx?sFac=" & ViewState("sFac") & "&sOMfg=" & ViewState("sOMfg") & "&sCust=" & ViewState("sCust") & "&sPNo=" & ViewState("sPNo") & "&sDept=" & ViewState("sDept") & "&sWrkCntr=" & ViewState("sWrkCntr") & "&sFormula=" & ViewState("sFormula") & "&sRecStatus=" & IIf(ViewState("sRecStatus") = 1, True, False), False)

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            MPRModule.DeleteChartSpecCookies()

            Response.Redirect("ChartSpecList.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnReset_Click

    Protected Sub ChartSpec_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvChartSpec.DataBound
        PagingInformation.Text = String.Format("You are viewing page {0} of {1}...   Go to ", _
                                               gvChartSpec.PageIndex + 1, gvChartSpec.PageCount)

        ' Clear out all of the items in the DropDownList
        PageList.Items.Clear()

        ' Add a ListItem for each page
        For i As Integer = 0 To gvChartSpec.PageCount - 1

            ' Add the new ListItem   
            Dim pageListItem As New ListItem(String.Concat("Page ", i + 1), i.ToString())
            PageList.Items.Add(pageListItem)
            ' select the current item, if needed   
            If i = gvChartSpec.PageIndex Then
                pageListItem.Selected = True
            End If
        Next
    End Sub 'EOF ChartSpec_DataBound

    Protected Sub PageList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageList.SelectedIndexChanged

        ' Jump to the specified page       
        gvChartSpec.PageIndex = Convert.ToInt32(PageList.SelectedValue)
    End Sub 'PageList_SelectIndexChanged

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'If String.IsNullOrEmpty(gvChartSpec.SortExpression) Then
        '    'Set a Default Sort By - Formula Name
        '    gvChartSpec.Sort("FormulaName", SortDirection.Ascending)
        'End If
        ' Only add the sorting UI if the GridView is sorted
        If Not String.IsNullOrEmpty(gvChartSpec.SortExpression) Then
            ' Determine the index and HeaderText of the column that 
            'the data is sorted by
            Dim sortColumnIndex As Integer = -1
            Dim sortColumnHeaderText As String = String.Empty
            For i As Integer = 0 To gvChartSpec.Columns.Count - 1
                If gvChartSpec.Columns(i).SortExpression.CompareTo(gvChartSpec.SortExpression) = 0 Then
                    sortColumnIndex = i
                    sortColumnHeaderText = gvChartSpec.Columns(i).HeaderText
                    Exit For
                End If
            Next

            ' Reference the Table the GridView has been rendered into
            Dim gridTable As Table = CType(gvChartSpec.Controls(0), Table)

            ' Enumerate each TableRow, adding a sorting UI header if
            ' the sorted value has changed
            Dim lastValue As String = String.Empty
            For Each gvr As GridViewRow In gvChartSpec.Rows
                Dim currentValue As String = String.Empty
                If gvr.Cells(sortColumnIndex).Controls.Count > 0 Then
                    If TypeOf gvr.Cells(sortColumnIndex).Controls(0) Is CheckBox Then
                        If CType(gvr.Cells(sortColumnIndex).Controls(0), CheckBox).Checked Then
                            currentValue = "Yes"
                        Else
                            currentValue = "No"
                        End If

                        ' ... Add other checks here if using columns with other
                        '      Web controls in them (Calendars, DropDownLists, etc.) ...
                    End If
                Else
                    currentValue = gvr.Cells(sortColumnIndex).Text
                End If
                If lastValue.CompareTo(currentValue) <> 0 Then
                    ' there's been a change in value in the sorted column
                    Dim rowIndex As Integer = gridTable.Rows.GetRowIndex(gvr)

                    ' Add a new sort header row
                    Dim sortRow As New GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal)
                    Dim sortCell As New TableCell()
                    sortCell.ColumnSpan = gvChartSpec.Columns.Count
                    sortCell.Text = String.Format("{0}: {1}", sortColumnHeaderText, currentValue)
                    sortCell.CssClass = "SortHeaderRowStyle"

                    ' Add sortCell to sortRow, and sortRow to gridTable
                    sortRow.Cells.Add(sortCell)
                    gridTable.Controls.AddAt(rowIndex, sortRow)

                    ' Update lastValue
                    lastValue = currentValue
                End If
            Next
        End If

        MyBase.Render(writer)
    End Sub 'Render

    Protected Function ShowEdit(ByVal CSID As Integer) As Boolean
        Dim dReturnValue As Boolean = False
        If CSID > 0 Then
            dReturnValue = True
        End If
        ShowEdit = dReturnValue
    End Function 'ShowEdit
#End Region ' "Filter Chart Spec"

#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_MPR() As Boolean
        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_MPR") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_MPR"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_MPR") = value
        End Set

    End Property

    Protected Sub odsChartSpec_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsChartSpec.Selected

        Try
            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As MfgProd.Chart_SpecDataTable = CType(e.ReturnValue, MfgProd.Chart_SpecDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_MPR = True
            Else
                LoadDataEmpty_MPR = False
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

    Protected Sub gvChartSpec_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvChartSpec.RowCreated

        Try
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_MPR
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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
#End Region ' Insert Empty GridView Work-Around

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim Tdate As String = Replace(Date.Today.ToShortDateString, "/", "-")

        Response.Clear()

        Response.AddHeader("content-disposition", "attachment; filename=PartSpec_" & Tdate & ".xls")

        Response.Charset = ""

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.ContentType = "application/vnd.xls"

        Dim stringWrite As StringWriter = New StringWriter()

        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)

        gvChartSpec.Columns(0).Visible = False
        gvChartSpec.ShowFooter = False
        gvChartSpec.AllowPaging = False
        gvChartSpec.AllowSorting = False
        gvChartSpec.DataBind()
        gvChartSpec.HeaderStyle.BackColor = Color.White
        gvChartSpec.HeaderStyle.ForeColor = Color.Black
        gvChartSpec.HeaderStyle.Font.Bold = True
        gvChartSpec.HeaderRow.ToString.ToUpper()
        gvChartSpec.AlternatingRowStyle.ForeColor = Color.Black
        gvChartSpec.RowStyle.ForeColor = Color.Black

        gvChartSpec.BottomPagerRow.Visible = False
        gvChartSpec.RenderControl(htmlWrite)


        Response.Write(stringWrite.ToString())

        Response.End()

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS
        'Confirms that an HtmlForm control is rendered for the
        'specified ASP.NET server control at run time.

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("ChartSpec.aspx", False)
    End Sub
End Class
