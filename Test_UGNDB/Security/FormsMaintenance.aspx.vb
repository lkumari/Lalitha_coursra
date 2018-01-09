Option Explicit On
Option Strict On

''*****************************************************************************
'' Name    : FormsMaintenance.aspx.vb
'' Purpose : Displays and updates the Forms_Maint table in the UGNDB database.
''
'' Date        Author     Modifications	    
'' 05/30/2008  MWeyker    Created .Net application
'' 06/02/2008  MWeyker    Expand this menu item on the Master Page.
''                        Override breadcrumb navigation on Master Page.
''                        Modify the Master Page PageTitle and Content Label.
'' 07/15/2008  LRey       Added MenuID to the Insert and Update event handlers
''                        functions.
'' 08/25/2008  MWeyker    Added standard exception reporting,
''                        using UGNErrorTrapping class.
''                               
''*****************************************************************************

Partial Class Security_FormsMaintenance
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
            m.ContentLabel = "Forms"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = _
                    "<a href='../Home.aspx'><b>Home</b></a> > <b> Security </b> > Forms"
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
            ''*****************************
            '' Clear the search fields
            ''*****************************
            txtFormName.Text = ""
            txtHyperlinkID.Text = ""
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

    Protected Sub gvForms_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvForms.RowCommand
        Try
            ' Don't show two editing rows.
            ' When in edit mode, hide the footer (the insert row)
            If (e.CommandName = "Edit") Or (e.CommandName = "UpdateCustom") Then
                gvForms.ShowFooter = False
            Else
                gvForms.ShowFooter = True
            End If

            If e.CommandName = "InsertCustom" AndAlso Page.IsValid() Then
                ' Get the data for inserting a new record
                Dim strFormName As String = _
                    CType(gvForms.FooterRow.FindControl("txtFormNameInsert"), TextBox).Text
                Dim strHyperlinkID As String = _
                    CType(gvForms.FooterRow.FindControl("ddlHyperlinkIDInsert"), DropDownList).SelectedItem.Text
                Dim strMenuID As Integer = _
                    Integer.Parse(CType(gvForms.FooterRow.FindControl("ddMenu"), DropDownList).SelectedItem.Value)
                Dim blnObsolete As Boolean = _
                    CType(gvForms.FooterRow.FindControl("chkObsoleteInsert"), CheckBox).Checked
                InsertNewRow(strFormName, strHyperlinkID, blnObsolete, strMenuID)
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
    End Sub ' gvForms_RowCommand

    Protected Sub ibtnUpdate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            ' Get the GridView row that contains the Update button that was was clicked
            Dim ibtn As ImageButton = CType(sender, ImageButton)
            Dim gvr As GridViewRow = CType(ibtn.NamingContainer, GridViewRow)

            ' Extract the new data from the GridViewRow
            Dim intFormID As Integer = _
                CType(CType(gvr.FindControl("lblFormIdPreEdit"), Label).Text, Integer)
            Dim strFormName As String = _
                CType(gvr.FindControl("txtFormNameEdit"), TextBox).Text
            Dim strHyperlinkID As String = _
                CType(gvr.FindControl("ddlHyperlinkIDEdit"), DropDownList).SelectedItem.Text
            Dim strMenuID As Integer = _
               Integer.Parse(CType(gvr.FindControl("ddMenu"), DropDownList).SelectedItem.Value)
            Dim blnObsolete As Boolean = _
                CType(gvr.FindControl("chkObsoleteEdit"), CheckBox).Checked
            Dim blnSuccess As Boolean = _
                UpdateRow(intFormID, strFormName, strHyperlinkID, blnObsolete, strMenuID)

            If blnSuccess = True Then
                ' The update was successful.
                ' Take the row out of Edit mode,
                '   show the footer row, and
                '   rebind the data.
                gvForms.EditIndex = -1
                gvForms.ShowFooter = True
                gvForms.DataBind()
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
    ''' Insert a new record in the Forms_Maint table
    ''' </summary>
    ''' <param name="FormName"></param>
    ''' <param name="HyperlinkID"></param>
    ''' <param name="obsolete"></param>
    ''' <param name="MenuID"></param>
    ''' <remarks></remarks>
    Private Sub InsertNewRow(ByVal FormName As String, ByVal HyperlinkID As String, ByVal obsolete As Boolean, ByVal MenuID As Integer)
        Try
            ' Find out if the FormName already exists in another record.
            Dim intRecordCount As Integer = SecurityModule.GetFormCount(Nothing, FormName, Nothing, Nothing)
            If intRecordCount < 0 Then
                ' Error was encountered
                DisplayStatus("Error was encounter. Record was not inserted.", True)
                Exit Sub
            ElseIf intRecordCount > 0 Then
                ' The FormName already exists.
                ' Do not insert it into the table.
                DisplayStatus("Duplicate Form Name found. New record was not inserted.", True)
                Exit Sub
            End If

            ' Find out if the HyperlinkID already exists in another record.
            intRecordCount = SecurityModule.GetFormCount(Nothing, Nothing, HyperlinkID, Nothing)
            If intRecordCount < 0 Then
                ' Error was encountered
                DisplayStatus("Error was encounter. Record was not inserted.", True)
                Exit Sub
            ElseIf intRecordCount > 0 Then
                ' The HyperlinkID already exists.
                ' Do not insert it into the table.
                DisplayStatus("Duplicate Hyperlink ID found. New record was not inserted.", True)
                Exit Sub
            End If

            ' Insert the new record
            Dim intNewID As Integer = SecurityModule.InsertForm( _
                FormName, HyperlinkID, obsolete, MenuID)
            If intNewID = -1 Then
                DisplayStatus("Error - record not saved", True)
            Else
                DisplayStatus("")
                gvForms.DataBind()
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
    ''' Updates a record in the Forms_Maint table
    ''' </summary>
    ''' <param name="FormID">The FormID of the record to update</param>
    ''' <param name="FormName">New value of FormName</param>
    ''' <param name="HyperlinkID">New value of HyperlinkID</param>
    ''' <param name="Obsolete">New value of Obsolete</param>
    ''' <param name="MenuID"></param>
    ''' <returns>true if success; otherwise false.</returns>
    ''' <remarks></remarks>
    Private Function UpdateRow(ByVal FormID As Integer, _
        ByVal FormName As String, ByVal HyperlinkID As String, ByVal Obsolete As Boolean, ByVal MenuID As Integer) As Boolean
        Dim blnSuccess As Boolean = False
        Try
            ' Check the formName to make sure it is not already used in another record.
            Dim ds As DataSet = New DataSet()
            ds = SecurityModule.GetForm(Nothing, FormName, Nothing, Nothing, Nothing)
            For Each dr As DataRow In ds.Tables(0).Rows
                Dim strIdInTable As String = dr.Item("FormID").ToString()
                If Not strIdInTable.Equals(FormID.ToString()) Then
                    ' A match was found.
                    ' Report error, and cancel the update
                    DisplayStatus("Form Name '" & FormName & _
                        "' already exists for FormID = " & strIdInTable, True)
                    Return False
                End If
            Next

            ' Check the HyperlinkID to make sure it is not already used in another record.
            ds = SecurityModule.GetForm(Nothing, Nothing, HyperlinkID, Nothing, Nothing)
            For Each dr As DataRow In ds.Tables(0).Rows
                Dim strIdInTable As String = dr.Item("FormID").ToString()
                If Not strIdInTable.Equals(FormID.ToString()) Then
                    ' A match was found.
                    ' Report error, and cancel the update
                    DisplayStatus("Hyperlink ID '" & HyperlinkID & _
                        "' already exists for FormID = " & strIdInTable, True)
                    Return False
                End If
            Next

            ' Update the record
            blnSuccess = SecurityModule.UpdateForm(FormID, FormName, HyperlinkID, Obsolete, MenuID)
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

    Protected Sub odsForms_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsForms.Selected
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
    End Sub ' odsForms_Selected

    Protected Sub gvForms_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvForms.RowCreated
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
    End Sub ' gvForms_RowCreated

#End Region ' Insert Empty GridView Work-Around

End Class
