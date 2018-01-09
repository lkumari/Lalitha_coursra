' ************************************************************************************************
' Name:	PlannerCodeMaint.aspx.vb
' Purpose:	This program is used to view, insert, update Make
'
' Date		    Author	    
' 07/10/2012    LREY			Created .Net application
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
Partial Class MfgProd_PlannerCodeMaint
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim a As String = commonFunctions.UserInfo()

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN: Planner Codes"
            m.ContentLabel = "Planner Codes by Facility for Cycle Counting"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Manufacturing</b> > Planner Codes"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False


            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If HttpContext.Current.Session("UserFacility") = Nothing Or HttpContext.Current.Session("UserFacility") = "UT" Then
                ViewState("TMFacility") = ""
            Else
                ViewState("TMFacility") = HttpContext.Current.Session("UserFacility")
            End If

            If Not Page.IsPostBack Then
                BindCriteria()

                ViewState("sFac") = ""

                If Not Request.Cookies("CCMPC_FAC") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("CCMPC_FAC").Value)
                    ViewState("sFac") = Server.HtmlEncode(Request.Cookies("CCMPC_FAC").Value)
                Else
                    If ViewState("TMFacility") <> Nothing Then
                        ddUGNFacility.SelectedValue = ViewState("TMFacility")
                        ViewState("sFac") = ViewState("TMFacility")
                    End If
                End If
            Else
                If ViewState("TMFacility") = Nothing Then
                    ViewState("sFac") = ddUGNFacility.SelectedValue
                Else
                    ViewState("sFac") = ViewState("TMFacility")
                End If
            End If


            CheckRights()


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

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Brandon.Hollowell", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    If HttpContext.Current.Session("UserFacility") <> "UT" Then
                        ddUGNFacility.Enabled = False
                    End If

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 53) 'Planner Code Maint Form ID

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("ObjectRole") = True
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvPlannerCode.Columns(0).Visible = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    gvPlannerCode.Columns(0).Visible = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    gvPlannerCode.Columns(0).Visible = False
                            End Select
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF CheckRights

    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Planning Year control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            If ViewState("TMLoc") <> "UT" Then
                ddUGNFacility.SelectedValue = ViewState("TMLoc")
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
    End Sub

#Region "Filter Planner Code"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            If ViewState("TMFacility") = Nothing Then
                Response.Cookies("CCMPC_FAC").Value = ddUGNFacility.SelectedValue
            Else
                Response.Cookies("CCMPC_FAC").Value = ViewState("TMFacility")
            End If


            Response.Redirect("PlannerCodeMaint.aspx?sFac=" & ViewState("sFac"), False)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            MPRModule.DeletePlannerCodeCookies()

            Response.Redirect("PlannerCodeMaint.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnReset_Click

    Protected Sub PlannerCode_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPlannerCode.DataBound
        PagingInformation.Text = String.Format("You are viewing page {0} of {1}...   Go to ", _
                                               gvPlannerCode.PageIndex + 1, gvPlannerCode.PageCount)

        ' Clear out all of the items in the DropDownList
        PageList.Items.Clear()

        ' Add a ListItem for each page
        For i As Integer = 0 To gvPlannerCode.PageCount - 1

            ' Add the new ListItem   
            Dim pageListItem As New ListItem(String.Concat("Page ", i + 1), i.ToString())
            PageList.Items.Add(pageListItem)
            ' select the current item, if needed   
            If i = gvPlannerCode.PageIndex Then
                pageListItem.Selected = True
            End If
        Next
    End Sub 'EOF PlannerCode_DataBound

    Protected Sub PageList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageList.SelectedIndexChanged

        ' Jump to the specified page       
        gvPlannerCode.PageIndex = Convert.ToInt32(PageList.SelectedValue)

    End Sub 'PageList_SelectIndexChanged

#End Region ' "Filter Planner Code"

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

    Protected Sub odsPlannerCode_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsPlannerCode.Selected

        Try
            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As MfgProd.Planner_Code_MaintDataTable = CType(e.ReturnValue, MfgProd.Planner_Code_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_MPR = True
            Else
                LoadDataEmpty_MPR = False
            End If
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvPlannerCode_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPlannerCode.RowCreated

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
            lblErrors.Text = ex.Message

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

        Response.AddHeader("content-disposition", "attachment; filename=" & ViewState("sFac") & "_Planner_Code_Maint_" & Tdate & ".xls")

        Response.Charset = ""

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.ContentType = "application/vnd.xls"

        Dim stringWrite As StringWriter = New StringWriter()

        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)

        gvPlannerCode.Columns(0).Visible = False
        gvPlannerCode.ShowFooter = False
        gvPlannerCode.AllowPaging = False
        gvPlannerCode.AllowSorting = False
        gvPlannerCode.DataBind()
        gvPlannerCode.HeaderStyle.BackColor = Color.White
        gvPlannerCode.HeaderStyle.ForeColor = Color.Black
        gvPlannerCode.HeaderStyle.Font.Bold = True
        gvPlannerCode.HeaderRow.ToString.ToUpper()
        gvPlannerCode.AlternatingRowStyle.ForeColor = Color.Black
        gvPlannerCode.RowStyle.ForeColor = Color.Black

        gvPlannerCode.BottomPagerRow.Visible = False
        gvPlannerCode.RenderControl(htmlWrite)


        Response.Write(stringWrite.ToString())

        Response.End()

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS
        'Confirms that an HtmlForm control is rendered for the
        'specified ASP.NET server control at run time.

    End Sub
End Class
