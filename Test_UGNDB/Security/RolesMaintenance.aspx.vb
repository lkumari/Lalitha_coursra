Option Explicit On
Option Strict On

''*****************************************************************************
'' Name    : RolesMaintenance.aspx.vb
'' Purpose : Displays and updates the Roles_Maint table in the UGNDB database.
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
''
''*****************************************************************************


Partial Class Security_RolesMaintenance
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
            m.ContentLabel = "Roles"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = _
                    "<a href='../Home.aspx'><b>Home</b></a> > <b> Security </b> > Roles"
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
            txtRoleName.Text = ""
            txtDescription.Text = ""
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

    Protected Sub gvRoles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRoles.RowCommand
        Try
            ' When in Edit mode, hide the footer (the insert row)
            If e.CommandName = "Edit" Then
                gvRoles.ShowFooter = False
            Else
                gvRoles.ShowFooter = True
            End If

            If e.CommandName = "InsertCustom" AndAlso Page.IsValid() Then
                ' Get the data for inserting a new record
                Dim strRoleName As String = _
                    CType(gvRoles.FooterRow.FindControl("txtRoleNameInsert"), TextBox).Text
                Dim strDescription As String = _
                    CType(gvRoles.FooterRow.FindControl("txtDescriptionInsert"), TextBox).Text
                Dim blnObsolete As Boolean = _
                    CType(gvRoles.FooterRow.FindControl("chkObsoleteInsert"), CheckBox).Checked
                InsertNewRow(strRoleName, strDescription, blnObsolete)
            ElseIf e.CommandName = "Cancel" Then
                DisplayStatus("")
            ElseIf e.CommandName = "Edit" Then
                DisplayStatus("")
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
    End Sub ' gvRoles_RowCommand

    Protected Sub ibtnUpdate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            ' Get the GridView row that contains the Update button that was was clicked
            Dim ibtn As ImageButton = CType(sender, ImageButton)
            Dim gvr As GridViewRow = CType(ibtn.NamingContainer, GridViewRow)

            ' Extract the new data from the GridViewRow
            Dim intRoleID As Integer = _
                CType(CType(gvr.FindControl("lblRoleIdPreEdit"), Label).Text, Integer)
            Dim strRoleName As String = _
                CType(gvr.FindControl("txtRoleNameEdit"), TextBox).Text
            Dim strDescription As String = _
                CType(gvr.FindControl("txtDescriptionEdit"), TextBox).Text
            Dim blnObsolete As Boolean = _
                CType(gvr.FindControl("chkObsoleteEdit"), CheckBox).Checked
            Dim blnSuccess As Boolean = _
                UpdateRow(intRoleID, strRoleName, strDescription, blnObsolete)

            If blnSuccess = True Then
                ' Update was successful.
                ' Take the row out of Edit mode,
                '   show the footer row, and
                '   rebind the data.
                gvRoles.EditIndex = -1
                gvRoles.ShowFooter = True
                gvRoles.DataBind()
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

#End Region ' Event Handlers


#Region "Private Methods"

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
    ''' Insert a new record in the Roles_Maint table
    ''' </summary>
    ''' <param name="RoleName"></param>
    ''' <param name="Description"></param>
    ''' <param name="obsolete"></param>
    ''' <remarks></remarks>
    Private Sub InsertNewRow(ByVal RoleName As String, ByVal Description As String, ByVal obsolete As Boolean)
        Try
            ' Find out if the RoleName already exists in another record.
            Dim intRecordCount As Integer = SecurityModule.GetRoleCount(Nothing, RoleName, Nothing, Nothing)
            If intRecordCount < 0 Then
                ' Error was encountered
                DisplayStatus("Error was encounter. Record was not inserted.", True)
                Exit Sub
            ElseIf intRecordCount > 0 Then
                ' The RoleName already exists.
                ' Do not insert it into the table.
                DisplayStatus("Duplicate Role Name found. New record was not inserted.", True)
                Exit Sub
            End If

            ' Insert the new record
            Dim intNewID As Integer = SecurityModule.InsertRole( _
                RoleName, Description, obsolete)
            If intNewID = -1 Then
                DisplayStatus("Error - record not saved", True)
            Else
                DisplayStatus("")
                gvRoles.DataBind()
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
    ''' Updates a record in the Roles_Maint table
    ''' </summary>
    ''' <param name="RoleID">The RoleID of the record to update</param>
    ''' <param name="RoleName">New value of RoleName</param>
    ''' <param name="Description">New value of Description</param>
    ''' <param name="Obsolete">New value of Obsolete</param>
    ''' <remarks></remarks>
    Private Function UpdateRow(ByVal RoleID As Integer, _
        ByVal RoleName As String, ByVal Description As String, ByVal Obsolete As Boolean) As Boolean

        Dim blnSuccess As Boolean = False
        Try
            ' Check the RoleName to make sure it is not already used in another record.
            Dim ds As DataSet = New DataSet()
            ds = SecurityModule.GetRole(Nothing, RoleName, Nothing, Nothing, Nothing)
            For Each dr As DataRow In ds.Tables(0).Rows
                Dim strIdInTable As String = dr.Item("RoleID").ToString()
                If Not strIdInTable.Equals(RoleID.ToString()) Then
                    ' A match was found.
                    ' Report error, and cancel the update
                    DisplayStatus("Role Name '" & RoleName & _
                        "' already exists for RoleID = " & strIdInTable, True)
                    Return False
                End If
            Next

            ' Update the record
            blnSuccess = SecurityModule.UpdateRole(RoleID, RoleName, Description, Obsolete)
            If blnSuccess Then
                DisplayStatus("")
            Else
                DisplayStatus("Error - update has failed", True)
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

    Protected Sub odsRoles_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsRoles.Selected
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
    End Sub ' odsRoles_Selected

    Protected Sub gvRoles_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRoles.RowCreated
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
    End Sub ' gvRoles_RowCreated

#End Region ' Insert Empty GridView Work-Around

End Class ' Security_RolesMaintenance
