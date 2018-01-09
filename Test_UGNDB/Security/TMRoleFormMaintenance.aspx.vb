Option Explicit On
Option Strict On

''*****************************************************************************
'' Name    : TMRoleFormMaintenance.aspx.vb
'' Purpose : Displays and updates the TeamMember_RolesForms table in the UGNDB database.
''
'' Date        Author     Modifications	    
'' 05/30/2008  MWeyker    Created .Net application
''
'' 06/02/2008  MWeyker    Expand this menu item on the Master Page.
''                        Override breadcrumb navigation on Master Page.
''                        Modify the Master Page PageTitle and Content Label.
''
'' 07/16/2008  MWeyker    Make the gvTMRoleForm GridView column "Form Id" updatable.
''                        Add an EDIT, UPDATE, AND CANCEL(update) button to
''                        the gvTMRoleForm GridView.
''                        Add an ibtnUpdate_Click event method to update
''                        the TeamMember_Maint with a new FormID.
'' 
'' 08/25/2008  MWeyker    Added standard exception reporting,
''                        using UGNErrorTrapping class.
''
''*****************************************************************************

Partial Class Security_TMRoleFormMaintenance
    Inherits System.Web.UI.Page


#Region "Module Level Variables"
#End Region ' Module Level Variables"


#Region "Loading and Initialization"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Team Members - Roles and Forms"

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

            If Not IsPostBack Then
                ' Populate the drop down lists
                LoadUserLookupList()
                LoadUserCopyFromList()
            End If

            If (Not IsPostBack) AndAlso (Request.QueryString("TeamMemberId") IsNot Nothing) Then
                Dim strId As String = Request.QueryString("TeamMemberId")
                If Not strId.Equals("") Then
                    ' The TeamMemberId was sent in the Query string.
                    ' Select it from the TeamMember drop down list.
                    Try
                        ddlLookupUser.SelectedValue = strId
                        lblTeamMemberId.Text = "Id: " & ddlLookupUser.SelectedValue
                        lblTeamMemberId.Font.Italic = False
                        hfTeamMemberId.Value = ddlLookupUser.SelectedValue
                        DisplayGridView(True)
                    Catch ex As Exception
                    End Try
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

    Protected Sub btnCopyFrom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopyFrom.Click
        Try
            Dim intCopyFrom As Integer = Integer.Parse(ddlCopyFrom.SelectedValue)
            Dim intCopyTo As Integer = Integer.Parse(ddlLookupUser.SelectedValue)
            Dim strOutputMessage As String = ""
            Dim intRecordsCopied As Integer = 0
            Dim blnSuccess As Boolean = SecurityModule.CopyTMRoleForm( _
                intCopyFrom, intCopyTo, strOutputMessage, intRecordsCopied)
            If blnSuccess = True Then
                lblCopyMessage.Text = "Copy complete!"
                btnCopyFrom.Enabled = False
                lblCopyMessage.ForeColor = Color.DarkSlateGray
                gvTMRoleForm.DataBind()
            Else
                lblCopyMessage.ForeColor = Color.Red
                lblCopyMessage.Text = strOutputMessage
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
    End Sub ' btnCopyFrom_Click

    Protected Sub ddlCopyFrom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCopyFrom.SelectedIndexChanged
        Try
            SetCopyButtonEnable()
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' ddlCopyFrom_SelectedIndexChanged

    Protected Sub ddlLookupUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLookupUser.SelectedIndexChanged
        Try
            ' A different user item has been selected from ddlLookupUser.
            If (IsAUserSelected() = False) Then
                ' No user is currently selected
                lblTeamMemberId.Text = "Please select a Team Member"
                lblTeamMemberId.Font.Italic = True
                hfTeamMemberId.Value = ""
                DisplayGridView(False)
            Else
                ' A user is currently selected
                lblTeamMemberId.Text = "Id: " & ddlLookupUser.SelectedValue
                lblTeamMemberId.Font.Italic = False
                hfTeamMemberId.Value = ddlLookupUser.SelectedValue
                DisplayGridView(True)
            End If

            SetCopyButtonEnable()
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' ddlLookupUser_SelectedIndexChanged

    Protected Sub gvTMRoleForm_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTMRoleForm.RowCommand
        Try
            ' When in Edit mode, hide the footer (the insert row)
            If e.CommandName = "Edit" Then
                gvTMRoleForm.ShowFooter = False
            Else
                gvTMRoleForm.ShowFooter = True
            End If

            If e.CommandName = "InsertCustom" AndAlso Page.IsValid() Then
                ' Get the data for inserting a new record
                Dim intFormID As Integer = _
                    CType(CType(gvTMRoleForm.FooterRow.FindControl("ddlNewForm"), _
                    DropDownList).SelectedValue, Integer)
                Dim intRoleID As Integer = _
                    CType(CType(gvTMRoleForm.FooterRow.FindControl("ddlNewRole"), _
                    DropDownList).SelectedValue, Integer)
                InsertNewRow(CType(hfTeamMemberId.Value, Integer), intRoleID, intFormID)
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
    End Sub ' gvTMRoleForm_RowCommand

    Protected Sub gvTMRoleForm_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTMRoleForm.RowDataBound
        Try
            ' If the current RowType is a DataRow, then attach a client script to the 
            ' Delete button's onclick event.
            '
            ' When the delete button is clicked, the script executes the following steps:
            ' 1) Save the row's original background color
            ' 2) Change the row's background color (to highlight the row)
            ' 3) Display a confirmation box to confirm the delete
            ' 4) If the confirmation cancel button was clicked, then
            '      restore the row's background color to its original.

            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnDelete"), ImageButton)
                If ibtn IsNot Nothing Then
                    ibtn.Attributes.Add("onclick", _
                        "this.originalcolor=this.style.backgroundColor;" & _
                        " this.parentNode.parentNode.style.backgroundColor='#9BB8D5'; " & _
                        "if (confirm('Are you sure you want to delete this entry?')) return true; " & _
                        "else {this.parentNode.parentNode.style.backgroundColor=this.originalcolor; return false;}")
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
    End Sub ' gvTMRoleForm_RowDataBound

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            ' Get the GridViewRow that contains the delete button that
            ' was clicked.
            Dim ibtn As ImageButton = CType(sender, ImageButton)
            Dim gvr As GridViewRow = CType(ibtn.NamingContainer, GridViewRow)

            ' Extract the FormID and RoleID from the GridViewRow
            Dim intFormID As Integer = _
                CType(CType(gvr.FindControl("lblFormID"), Label).Text, Integer)
            Dim intRoleID As Integer = _
                 CType(CType(gvr.FindControl("lblRoleID"), Label).Text, Integer)

            ' Delete the record.
            DeleteRow(CType(hfTeamMemberId.Value, Integer), intRoleID, intFormID)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' ibtnDelete_Click

    Protected Sub ibtnUpdate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            ' If Page fails validation, do not update.
            If Page.IsValid = False Then
                Exit Sub
            End If

            ' Get the GridView row that contains the Update button that was clicked
            Dim ibtn As ImageButton = CType(sender, ImageButton)
            Dim gvr As GridViewRow = CType(ibtn.NamingContainer, GridViewRow)

            ' Extract the new data from the GridViewRow
            Dim intTeamMemberID As Integer = Integer.Parse(hfTeamMemberId.Value)
            Dim intFormId As Integer = Integer.Parse(CType(gvr.FindControl("lblFormID"), Label).Text)
            Dim intRoleIdOld As Integer = Integer.Parse(CType(gvr.FindControl("lblRoleID"), Label).Text)
            Dim intRoleIdNew As Integer = Integer.Parse(CType(gvr.FindControl("ddlEditRole"), DropDownList).SelectedValue)

            Dim intRecordCount As Integer = SecurityModule.GetTMRoleFormCount(intTeamMemberID, intRoleIdNew, intFormId)
            If intRecordCount < 0 Then
                ' Error was encountered
                DisplayStatus("Error was encountered. Record was not inserted.", True)
                Exit Sub
            ElseIf intRecordCount > 0 Then
                ' New Record already exists.
                ' Do not insert it into the table.
                DisplayStatus("Duplicate record found. New record was not inserted.", True)
                Exit Sub
            End If

            ' Update the row with the new data
            Dim blnSuccess As Boolean
            blnSuccess = SecurityModule.UpdateTMRoleForm(intTeamMemberID, _
                intFormId, intRoleIdOld, intRoleIdNew)

            If blnSuccess = True Then
                ' Update was successful.
                ' Take the row out of Edit mode,
                '   show the footer row, and
                '   rebind the data.
                DisplayStatus("")
                gvTMRoleForm.EditIndex = -1
                gvTMRoleForm.ShowFooter = True
                gvTMRoleForm.DataBind()
            Else
                DisplayStatus("Database error. Update failed", True)
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

    Protected Sub lbTMGeneralTab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTMGeneralTab.Click
        Try
            RedirectPage("TMGeneralMaintenance.aspx")
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' lbTMGeneralTab_Click

    Protected Sub lbTMWorkHistoryTab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTMWorkHistoryTab.Click
        Try
            RedirectPage("TMWorkHistoryMaintenance.aspx")
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
#End Region ' Event Handlers


#Region "Private Methods"

    ''' <summary>
    ''' Add a client confirm popup to the onclick event of the "Copy" button.
    ''' </summary>
    ''' <param name="ToTeamMember">Username include in confirm message.</param>
    ''' <remarks></remarks>
    Private Sub AddClientScriptToCopyButton(ByVal ToTeamMember As String)
        Try
            btnCopyFrom.Attributes("onclick") = _
                "if (confirm('You are about to replace the Role/Form profile for\n" & _
                ToTeamMember & "." & _
                "\n\nDo you want to continue?')) return true; " & _
                "else return false;"
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' AddClientScriptToCopyButton

    Private Sub DeleteRow(ByVal TeamMemberID As Integer, ByVal RoleID As Integer, ByVal FormID As Integer)
        Try
            Dim blnSuccess As Boolean = SecurityModule.DeleteTMRoleForm(TeamMemberID, RoleID, FormID)
            If blnSuccess Then
                DisplayStatus("", False)
                ' Show the new collection
                gvTMRoleForm.DataBind()
            Else
                DisplayStatus("Error - record not deleted", True)
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
    End Sub ' DeleteRow

    Private Sub DisplayGridView(ByVal MakeVisible As Boolean)
        Try
            gvTMRoleForm.Enabled = MakeVisible
            gvTMRoleForm.Visible = MakeVisible
            'lblCurrentPage.Visible = MakeVisible
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' DisplayGridView

    ''' <summary>
    ''' Display a message in the lblStatus label
    ''' </summary>
    ''' <param name="Message">The message to be displayed</param>
    ''' <param name="IsError">True if message is an error, otherwise false</param>
    ''' <remarks></remarks>
    Private Sub DisplayStatus(ByVal Message As String, Optional ByVal IsError As Boolean = False)
        Try
            lblStatus.Text = Message
            If IsError Then
                lblStatus.ForeColor = Color.Red
            Else
                lblStatus.ForeColor = Color.DarkSlateGray
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

    Private Sub InsertNewRow(ByVal TeamMemberID As Integer, ByVal RoleID As Integer, ByVal FormID As Integer)
        Try
            Dim intRecordCount As Integer = SecurityModule.GetTMRoleFormCount(TeamMemberID, RoleID, FormID)
            If intRecordCount < 0 Then
                ' Error was encountered
                DisplayStatus("Error was encountered. Record was not inserted.", True)
                Exit Sub
            ElseIf intRecordCount > 0 Then
                ' New Record already exists.
                ' Do not insert it into the table.
                DisplayStatus("Duplicate record found. New record was not inserted.", True)
                Exit Sub
            End If

            Dim blnSuccess As Boolean = SecurityModule.InsertTMRoleForm(TeamMemberID, RoleID, FormID)
            If blnSuccess Then
                DisplayStatus("", False)
                ' Show the new record
                gvTMRoleForm.DataBind()
            Else
                DisplayStatus("Error - record not saved", True)
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
    ''' Returns a Boolean value indicating whether a "copy from" user is selected.
    ''' </summary>
    ''' <returns><b>true</b> if a user is selected; otherwise <b>false</b>.</returns>
    ''' <remarks></remarks>
    Private Function IsACopyfromUserSelected() As Boolean
        Dim blnSelected As Boolean = False
        Try
            If (ddlCopyFrom.SelectedIndex = -1) Or _
               (String.IsNullOrEmpty(ddlCopyFrom.Text.Trim) = True) Then
                blnSelected = False
            Else
                blnSelected = True
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
        Return blnSelected
    End Function ' IsACopyfromUserSelected

    ''' <summary>
    ''' Returns a Boolean value indicating whether a user is selected.
    ''' </summary>
    ''' <returns><b>true</b> if a user is selected; otherwise <b>false</b>.</returns>
    ''' <remarks></remarks>
    Private Function IsAUserSelected() As Boolean
        Dim blnSelected As Boolean = False
        Try
            If (ddlLookupUser.SelectedIndex = -1) Or _
               (String.IsNullOrEmpty(ddlLookupUser.Text.Trim) = True) Then
                blnSelected = False
            Else
                blnSelected = True
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
        Return blnSelected
    End Function ' IsAUserSelected

    Private Sub LoadUserCopyFromList()
        ' -------------------------------------------
        '  Populate the CopyFrom drop down list
        ' -------------------------------------------
        Try
            With ddlCopyFrom
                .DataSource = SecurityModule.GetTeamMember(Nothing, _
                    Nothing, Nothing, Nothing, Nothing, _
                    Nothing, Nothing, Nothing)
                .DataTextField = "UserName"
                .DataValueField = "TeamMemberID"
                .DataBind()
                .Items.Insert(0, "")
                .SelectedIndex = -1
            End With
        Catch ex As Exception
            DisplayStatus("Database error", True)

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' LoadUserCopyFromList

    Private Sub LoadUserLookupList()
        ' -------------------------------------------
        '  Populate the Username drop down list
        ' -------------------------------------------
        Try
            With ddlLookupUser
                .DataSource = SecurityModule.GetTeamMember(Nothing, _
                    Nothing, Nothing, Nothing, Nothing, _
                    Nothing, Nothing, Nothing)
                .DataTextField = "UserName"
                .DataValueField = "TeamMemberID"
                .DataBind()
                .Items.Insert(0, "")
                .SelectedIndex = -1
            End With

            lblTeamMemberId.Text = "Please select a Team Member"
            lblTeamMemberId.Font.Italic = True
            hfTeamMemberId.Value = ""
            DisplayGridView(False)
            ddlLookupUser.Focus()
        Catch ex As Exception
            DisplayStatus("Database error", True)

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' LoadUserLookupList

    ''' <summary>
    ''' Redirects to new page with confirmation.
    ''' </summary>
    ''' <param name="PageName">Name of next page</param>
    ''' <remarks>
    ''' Includes the selected TeamMemberId and UserName as QueryString parameters.
    ''' </remarks>
    Private Sub RedirectPage(ByVal PageName As String)
        Try
            Dim strQueryString As String = ""

            If Not hfTeamMemberId.Value.Equals("") Then
                ' Send the current TeamMemberId and Username to the next page
                strQueryString = "?TeamMemberId=" & hfTeamMemberId.Value & _
                    "&UserName=" & ddlLookupUser.SelectedItem.Text
            End If

            Response.Redirect(PageName & strQueryString, False)
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
    ''' Sets the "Copy" Button Enabled property.
    ''' </summary>
    ''' <remarks>
    ''' If either the "Users" or "Copy From Users" lists have no items
    ''' selected, disable the "Copy" Button; otherwise enable.
    ''' </remarks>
    Private Sub SetCopyButtonEnable()
        Try
            If (IsAUserSelected() = False) Or _
                (IsACopyfromUserSelected() = False) Or _
                (ddlLookupUser.SelectedValue = ddlCopyFrom.SelectedValue) Then
                ' One of the user DropDowns does not have a selection.
                ' Disable the "Copy From" button.
                btnCopyFrom.Enabled = False
                btnCopyFrom.ToolTip = ""
                lblCopyMessage.Visible = False
            Else
                ' Both user DropDowns have selected items.
                ' Enable the "Copy From" button.
                btnCopyFrom.Enabled = True
                btnCopyFrom.ToolTip = "Copy the Role/Form profile from " & _
                    ddlCopyFrom.SelectedItem.Text & "."
                lblCopyMessage.Visible = True
                lblCopyMessage.Text = _
                    ddlCopyFrom.SelectedItem.Text & _
                    "(" & ddlCopyFrom.SelectedValue.ToString & _
                    ") -> " & ddlLookupUser.SelectedItem.Text & _
                    "(" & ddlLookupUser.SelectedValue.ToString & ")"
                ddlCopyFrom.SelectedValue.ToString()
                lblCopyMessage.ForeColor = Color.DarkSlateGray

                ' Insert the "Copy to" name in the Copy Button's confirm message.
                AddClientScriptToCopyButton( _
                    ddlLookupUser.SelectedItem.Text & _
                    " (" & ddlLookupUser.SelectedValue.ToString & ")")
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
    End Sub ' SetCopyButtonEnable

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

    Protected Sub odsTMRoleForm_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsTMRoleForm.Selected
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
    End Sub ' odsTMRoleForm_Selected

    Protected Sub gvTMRoleForm_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTMRoleForm.RowCreated
        Try
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' The RoleID field is stored in column ? of gvTMRoleForm.
            ' The FormID field is stored in column ? of gvTMRoleForm.
            ' These columns are used by this program, and should not be visible to the user.
            ' Make the columns invisible AFTER the data is bound. 
            ' If the column is made invisible BEFORE data binding, the binding
            ' never occurs. (Applies to: VS2005; .NET 2.0)
            ' See: GridView Hidden Column Problem (And Two Common Solutions)
            ' http://www.beansoftware.com/ASP.NET-Tutorials/GridView-Hidden-Column.aspx
            '''' MAY NOT NEED THIS CODE '''''''''''''''''''''
            ''If e.Row.Cells.Count >= ? Then
            ''    e.Row.Cells(?).Visible = False
            ''    e.Row.Cells(?).Visible = False
            ''End If
            '''''''''''''''''''''''''''''''''''''''''''''''''

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
    End Sub ' gvTMRoleForm_RowCreated

#End Region ' Insert Empty GridView Work-Around

 
End Class
