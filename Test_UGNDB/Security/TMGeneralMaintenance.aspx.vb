Option Explicit On
Option Strict On

''*****************************************************************************
'' Name    : TMGeneralMaintenance.aspx.vb
'' Purpose : Displays and updates the TeamMember_Maint table in the UGNDB database.
''
'' Date        Author     Modifications	    
'' 05/30/2008  MWeyker    Created .Net application
''
'' 06/02/2008  MWeyker    Expand this menu item on the Master Page.
''                        Override breadcrumb navigation on Master Page.
''                        Modify the Master Page PageTitle and Content Label.
''   
'' 08/25/2008  MWeyker    Added standard exception reporting,
''                        using UGNErrorTrapping class.
'' 09/26/2008  MWeyker    Added "endResponse=False" parameter to Response.Redirect
''                        methods to avoid thread abort exceptions.
''
''*****************************************************************************


Imports System.Diagnostics

Partial Class Security_TMGeneralMaintenance
    Inherits System.Web.UI.Page

#Region "Module Level Variables"
#End Region


#Region "Loading and Initialization"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Team Members - General"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = _
                    "<a href='../Home.aspx'><b>Home</b></a> > <b> Security </b> > Team Members"
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
            ctl = m.FindControl("SECExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''****************************************************
            '' If TeamMember Insert has been turned off,
            ''   don't show the GridView FooterRow.
            ''****************************************************
            If IsInsertEnabled() = False Then
                gvTeamMembers.ShowFooter = False
            End If

            ''*************************************************
            '' If a UserName was sent from another page,
            '' use it in a search control.
            ''*************************************************
            If (Not IsPostBack) AndAlso (Request.QueryString("UserName") IsNot Nothing) Then
                Dim strUserName As String = Request.QueryString("UserName")
                If Not strUserName.Equals("") Then
                    ' The UserName was sent in the Query string.
                    ' Use it in a TextBox search control.
                    txtUserName.Text = strUserName
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' Page_Load

#End Region ' Loading and Initialization


#Region "Event Handlers"

    Protected Sub btnResetSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSearch.Click
        Try
            txtUserName.Text = ""
            txtLName.Text = ""
            txtFName.Text = ""
            txtEmail.Text = ""
            optWorkStatusList.Items.FindByValue("Both").Selected = True
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' btnResetSearch_Click

    Protected Sub gvTeamMembers_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTeamMembers.DataBound
        Try
            ' Display the current page
            lblCurrentPage.Text = String.Format("Page {0} of {1}", _
                gvTeamMembers.PageIndex + 1, gvTeamMembers.PageCount)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' gvTeamMembers_DataBound

    Protected Sub gvTeamMembers_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTeamMembers.RowCommand
        Try
            ' When in Edit mode, hide the footer (the insert row)
            If e.CommandName = "Edit" Then
                gvTeamMembers.ShowFooter = False
            Else
                If IsInsertEnabled() = True Then
                    gvTeamMembers.ShowFooter = True
                Else
                    gvTeamMembers.ShowFooter = False
                End If
            End If

            If e.CommandName = "InsertCustom" AndAlso Page.IsValid() Then
                ' Get the data for inserting a new record
                Dim gvr As GridViewRow = gvTeamMembers.FooterRow
                Dim strUserName As String = CType(gvr.FindControl("txtUserNameInsert"), TextBox).Text
                Dim strFirstName As String = CType(gvr.FindControl("txtFirstNameInsert"), TextBox).Text
                Dim strLastName As String = CType(gvr.FindControl("txtLastNameInsert"), TextBox).Text
                Dim strEmail As String = CType(gvr.FindControl("txtEmailInsert"), TextBox).Text
                Dim blnWorking As Boolean = CType(gvr.FindControl("chkWorkingInsert"), CheckBox).Checked
                InsertNewRow(strUserName, strFirstName, strLastName, strEmail, blnWorking)
            ElseIf e.CommandName = "Cancel" Then
                DisplayStatus("", False)
            ElseIf e.CommandName = "Edit" Then
                DisplayStatus("", False)
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' gvTeamMembers_RowCommand

    Protected Sub gvTeamMembers_RowDataBound(ByVal sender As Object, _
        ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTeamMembers.RowDataBound
        Try
            ' Build the client script to open a popup window containing
            ' Active Directory Users. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=830px," & _
                "height=600px," & _
                "left='+((screen.width-830)/2)+'," & _
                "top='+((screen.height-600)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnEditGetAD"), ImageButton)
                If ibtn IsNot Nothing Then
                    Dim txtUserName As TextBox = CType(e.Row.FindControl("txtUserNameEdit"), TextBox)
                    Dim txtLastName As TextBox = CType(e.Row.FindControl("txtLastNameEdit"), TextBox)
                    Dim txtFirstName As TextBox = CType(e.Row.FindControl("txtFirstNameEdit"), TextBox)
                    Dim txtEmail As TextBox = CType(e.Row.FindControl("txtEmailEdit"), TextBox)
                    Dim strPagePath As String = _
                        "ActiveDirectoryLookup.aspx?textbox1=" & txtLastName.ClientID & _
                        "&textbox2=" & txtFirstName.ClientID & _
                        "&textbox3=" & txtUserName.ClientID & _
                        "&textbox4=" & txtEmail.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','AD_Users','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
                End If
            ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnInsertGetAD"), ImageButton)
                If ibtn IsNot Nothing Then
                    Dim txtUserName As TextBox = CType(e.Row.FindControl("txtUserNameInsert"), TextBox)
                    Dim txtLastName As TextBox = CType(e.Row.FindControl("txtLastNameInsert"), TextBox)
                    Dim txtFirstName As TextBox = CType(e.Row.FindControl("txtFirstNameInsert"), TextBox)
                    Dim txtEmail As TextBox = CType(e.Row.FindControl("txtEmailInsert"), TextBox)
                    Dim strPagePath As String = _
                        "ActiveDirectoryLookup.aspx?textbox1=" & txtLastName.ClientID & _
                        "&textbox2=" & txtFirstName.ClientID & _
                        "&textbox3=" & txtUserName.ClientID & _
                        "&textbox4=" & txtEmail.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','AD_Users','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' gvTeamMembers_RowDataBound

    Protected Sub gvTeamMembers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTeamMembers.SelectedIndexChanged
        Try
            Dim r As GridViewRow = gvTeamMembers.SelectedRow
            Dim strFormId As String = r.Cells(1).Text
            Dim intFormId As Integer

            If Integer.TryParse(strFormId, intFormId) Then
                Response.Redirect("SecurityTeamMemberMaint.aspx?TeamMemberId=" & intFormId, False)
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' gvTeamMembers_SelectedIndexChanged

    Protected Sub ibtnUpdate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            ' Get the GridView row that contains the Update button that was clicked
            Dim ibtn As ImageButton = CType(sender, ImageButton)
            Dim gvr As GridViewRow = CType(ibtn.NamingContainer, GridViewRow)

            ' Extract the new data from the GridViewRow
            Dim intTeamMemberID As Integer = CType(CType(gvr.FindControl("lnkTeamMemberId"), LinkButton).Text, Integer)
            Dim strUserName As String = CType(gvr.FindControl("txtUserNameEdit"), TextBox).Text
            Dim strFirstName As String = CType(gvr.FindControl("txtFirstNameEdit"), TextBox).Text
            Dim strLastName As String = CType(gvr.FindControl("txtLastNameEdit"), TextBox).Text
            Dim strEmail As String = CType(gvr.FindControl("txtEmailEdit"), TextBox).Text
            Dim blnWorking As Boolean = CType(gvr.FindControl("chkWorkingEdit"), CheckBox).Checked
            Dim blnSuccess As Boolean = _
                UpdateRow(intTeamMemberID, strUserName, strFirstName, strLastName, strEmail, blnWorking)

            If blnSuccess = True Then
                ' Update was successful.
                ' Take the row out of Edit mode,
                '   show the footer row, and
                '   rebind the data.
                Debug.WriteLine("1. RowState: " & gvr.RowState.ToString)
                Debug.WriteLine("1. SelectedIndex: " & gvTeamMembers.SelectedIndex.ToString)
                Debug.WriteLine("1. EditIndex: " & gvTeamMembers.EditIndex.ToString)
                gvTeamMembers.EditIndex = -1
                If IsInsertEnabled() = True Then
                    gvTeamMembers.ShowFooter = True
                End If
                gvTeamMembers.DataBind()
                Debug.WriteLine("2. RowState: " & gvr.RowState.ToString)
                Debug.WriteLine("2. SelectedIndex: " & gvTeamMembers.SelectedIndex.ToString)
                Debug.WriteLine("2. EditIndex: " & gvTeamMembers.EditIndex.ToString)
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' ibtnUpdate_Click

    Protected Sub lbTMWorkHistoryTab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTMWorkHistoryTab.Click
        Try
            ' Redirects the user to the WorkHistory screen.
            ' If only one row is displayed in the GridView, and
            '   that row contains data, include the TeamMemberID in the
            '   QueryString.
            Dim strQueryString As String = ""
            Dim obj As Object
            If gvTeamMembers.Rows.Count = 1 Then
                ' GridView contains 1 row.
                obj = gvTeamMembers.Rows(0).FindControl("lnkTeamMemberId")
                If (obj IsNot Nothing) AndAlso (TypeOf obj Is LinkButton) Then
                    ' The lnkTeamMemberID button exists in that row.
                    ' Build a QueryString parameter with the TeamMemberId.
                    Dim lbtn As LinkButton = CType(obj, LinkButton)
                    Dim strTeamMemberID As String = lbtn.Text
                    strQueryString = "?TeamMemberId=" & strTeamMemberID
                End If
            End If

            Response.Redirect("TMWorkHistoryMaintenance.aspx" & strQueryString, False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' lbTMWorkHistoryTab_Click

    Protected Sub lbTMRolesTab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTMRolesTab.Click
        Try
            ' Redirects the user to the RoleForm screen.
            ' If only one row is displayed in the GridView, and
            '   that row contains data, include the TeamMemberID in the
            '   QueryString.
            Dim strQueryString As String = ""
            Dim obj As Object
            If gvTeamMembers.Rows.Count = 1 Then
                ' GridView contains 1 row.
                obj = gvTeamMembers.Rows(0).FindControl("lnkTeamMemberId")
                If (obj IsNot Nothing) AndAlso (TypeOf obj Is LinkButton) Then
                    ' The lnkTeamMemberID button exists in that row.
                    ' Build a QueryString parameter with the TeamMemberId.
                    Dim lbtn As LinkButton = CType(obj, LinkButton)
                    Dim strTeamMemberID As String = lbtn.Text
                    strQueryString = "?TeamMemberId=" & strTeamMemberID
                End If
            End If

            Response.Redirect("TMRoleFormMaintenance.aspx" & strQueryString, False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' lbTMRolesTab_Click

    Protected Sub lnkTeamMemberId_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ' Get the GridView row that contains the LinkButton that was clicked
            Dim lbtn As LinkButton = CType(sender, LinkButton)
            Dim gvr As GridViewRow = CType(lbtn.NamingContainer, GridViewRow)

            ' Extract the TeamMemberID from the clicked row.
            Dim intTeamMemberID As Integer = _
                CType(CType(gvr.FindControl("lnkTeamMemberId"), LinkButton).Text, Integer)
            Response.Redirect("TMWorkHistoryMaintenance.aspx?TeamMemberId=" & intTeamMemberID.ToString(), False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' lnkTeamMemberId_Click

    Protected Sub lnkUserName_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ' Get the GridView row that contains the LinkButton that was clicked
            Dim lbtn As LinkButton = CType(sender, LinkButton)
            Dim gvr As GridViewRow = CType(lbtn.NamingContainer, GridViewRow)

            ' Extract the TeamMemberID from the clicked row.
            Dim intTeamMemberID As Integer = _
                CType(CType(gvr.FindControl("lnkTeamMemberId"), LinkButton).Text, Integer)
            Response.Redirect("TMRoleFormMaintenance.aspx?TeamMemberId=" & intTeamMemberID.ToString(), False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' lnkUserName_Click

#End Region ' Event Handlers


#Region "Private Methods"

    ''' <summary>
    ''' Checks a web.config setting to determine if TeamMember Insert has been disabled.
    ''' </summary>
    ''' <returns>True if Team Member insert should be enabled; otherwise False.</returns>
    ''' <remarks>
    ''' Based on the "EnableTeamMemberInsert" key in the web.config AppSettings.
    ''' If the "EnableTeamMemberInsert" key is missing or misspelled, this function
    ''' returns True. "EnableTeamMemberInsert" should be initially set to "false", when
    ''' all team members are inserted into the Employee_M table.
    ''' </remarks>
    Private Function IsInsertEnabled() As Boolean
        Dim blnEnabled As Boolean = True
        Try
            Dim obj As Object = _
                System.Configuration.ConfigurationManager.AppSettings("EnableTeamMemberInsert")
            If obj IsNot Nothing Then
                Dim strSetting As String = obj.ToString().ToLower()
                If strSetting = "false" Then
                    blnEnabled = False
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
        Return blnEnabled
    End Function ' IsInsertEnabled

    ''' <summary>
    ''' Displays a message in the lblStatus field
    ''' </summary>
    ''' <param name="Message">The message to display</param>
    ''' <param name="IsError">True displays in error color, false displays normal color</param>
    ''' <remarks></remarks>
    Private Sub DisplayStatus(ByVal Message As String, Optional ByVal IsError As Boolean = False)
        Try
            lblStatus.Text = Message
            If IsError Then
                lblStatus.ForeColor = Color.Red
            Else
                lblStatus.ForeColor = Color.DarkGreen
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' DisplayStatus

    ''' <summary>
    ''' Inserts a new row into the TeamMember_Maint table
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="FirstName"></param>
    ''' <param name="LastName"></param>
    ''' <param name="Email"></param>
    ''' <param name="Working"></param>
    ''' <remarks></remarks>
    Private Sub InsertNewRow(ByVal UserName As String, _
        ByVal FirstName As String, ByVal LastName As String, _
        ByVal Email As String, ByVal Working As Boolean)
        Try
            ' Determine if the UserName already exists
            Dim intcount As Integer = SecurityModule.GetTeamMemberCount( _
                Nothing, UserName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If intcount = -1 Then
                DisplayStatus("Database error")
                Exit Sub
            ElseIf intcount > 0 Then
                DisplayStatus("Error: User Name already exists", True)
                Exit Sub
            End If

            ' Insert the record, and retrieve the new role id
            Dim intID As Integer = SecurityModule.InsertTeamMember( _
                UserName, _
                LCase(Trim(Left(FirstName, 1) & LastName)), _
                LastName, _
                FirstName, _
                Email, _
                Working)

            If intID = -1 Then
                DisplayStatus("Error adding new team member", True)
            Else
                ' Display the newly added record
                txtUserName.Text = UserName
                gvTeamMembers.DataBind()
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' InsertNewRow

    ''' <summary>
    ''' Redirects the client to the specified page
    ''' </summary>
    ''' <param name="PageName">Name of requested page</param>
    ''' <remarks>Sends the selected FormId in the QueryString</remarks>
    Private Sub RedirectPage(ByVal PageName As String)
        Try
            Dim strQueryString As String = ""
            ''If Not hfTeamMemberId.Value.Equals("") Then
            ''    ' Store the current FormId in the QueryString
            ''    strQueryString = "?TeamMemberId=" & hfTeamMemberId.Value
            ''End If
            Response.Redirect(PageName & strQueryString)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' RedirectPage

    ''' <summary>
    ''' Updates a row in the TeamMember_Maint table
    ''' </summary>
    ''' <param name="TeamMemberID">The primary key of the record to update</param>
    ''' <param name="UserName">New value of UserName</param>
    ''' <param name="FirstName">New value of FirstName</param>
    ''' <param name="LastName">New value of LastName</param>
    ''' <param name="Email">New value of Email</param>
    ''' <param name="Working">New value of Working</param>
    ''' <returns>true if success; otherwise false.</returns>
    ''' <remarks></remarks>
    Private Function UpdateRow(ByVal TeamMemberID As Integer, ByVal UserName As String, _
        ByVal FirstName As String, ByVal LastName As String, _
        ByVal Email As String, ByVal Working As Boolean) As Boolean
        Dim blnSuccess As Boolean = False

        Try
            ' Make sure the UserName is not already used in another record.
            Dim ds As DataSet = New DataSet()
            ds = SecurityModule.GetTeamMember(Nothing, UserName, _
                Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            For Each dr As DataRow In ds.Tables(0).Rows
                Dim strIdInTable As String = dr.Item("TeamMemberID").ToString()
                If Not strIdInTable.Equals(TeamMemberID.ToString) Then
                    ' A match was found in another record
                    ' Report the error, and cancel the update.
                    DisplayStatus("User Name '" & UserName & _
                        "' already exists for teammemberID " & strIdInTable, True)
                    Return False
                End If
            Next

            ' Update the record
            blnSuccess = SecurityModule.UpdateTeamMember( _
                TeamMemberID, _
                UserName, _
                LCase(Trim(Left(FirstName, 1) & LastName)), _
                LastName, _
                FirstName, _
                Email, _
                Working)

            If Not blnSuccess Then
                DisplayStatus("database error")
                Return False
            End If

        Catch ex As Exception
            DisplayStatus("Database error", True)
            blnSuccess = False

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return blnSuccess
    End Function ' UpdateRow

#End Region ' Private Methods


#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty") = value
        End Set
    End Property ' LoadDataEmpty

    Protected Sub odsTeamMembers_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsTeamMembers.Selected
        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)
            Dim ds As DataSet = CType(e.ReturnValue, DataSet)
            Dim dt As DataTable = ds.Tables(0)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty = True
            Else
                LoadDataEmpty = False
            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub ' odsTeamMembers_Selected

    Protected Sub gvTeamMembers_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTeamMembers.RowCreated
        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty
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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' gvTeamMembers_RowCreated

#End Region ' Insert Empty GridView Work-Around



End Class ' Security_TMGeneralMaintenance
