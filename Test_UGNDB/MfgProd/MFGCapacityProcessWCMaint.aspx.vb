' ************************************************************************************************
' Name:	MFGCapacityProcessWCMaint.aspx.vb
' Purpose:	This program is used to view, insert, update Make
'
' Date		    Author	    
' 05/08/2012    LRey        Created .Net application
' ************************************************************************************************
Partial Class MFGCapacityProcesWCMaint
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Work Center by Capacity Process"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                If Request.QueryString("sMfgCPN") IsNot Nothing Then
                    ViewState("sMfgCPN") = Server.UrlDecode(Request.QueryString("sMfgCPN").ToString)
                End If

                If Request.QueryString("pPID") IsNot Nothing Then
                    ViewState("pPID") = Server.UrlDecode(Request.QueryString("pPID").ToString)
                End If

                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Manufacturing</b> > <a href='MFGCapacityProcessMaint.aspx?sMfgCPN=" & ViewState("sMfgCPN") & "'><b>Capacity Process</b></a> > Work Center by Capacity Process"

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
            If Not Page.IsPostBack Then
                BindData()
            End If

            CheckRights()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub CheckRights()

        Try
            'default to hide edit column
            gvProcessList.Columns(0).Visible = False
            If gvProcessList.FooterRow IsNot Nothing Then
                gvProcessList.FooterRow.Visible = False
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 44 'MFG Capacity Process Form ID
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
                                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ViewState("ObjectRole") = True
                                            gvProcessList.Columns(0).Visible = True
                                            If gvProcessList.FooterRow IsNot Nothing Then
                                                gvProcessList.FooterRow.Visible = True
                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            ViewState("ObjectRole") = True
                                            gvProcessList.Columns(0).Visible = True
                                            If gvProcessList.FooterRow IsNot Nothing Then
                                                gvProcessList.FooterRow.Visible = True
                                            End If
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            ViewState("ObjectRole") = False
                                            gvProcessList.Columns(0).Visible = False
                                            If gvProcessList.FooterRow IsNot Nothing Then
                                                gvProcessList.FooterRow.Visible = False
                                            End If
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
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
    Private Sub BindData()
        Try
            lblMessage.Text = ""
            lblMessage.Visible = False

            Dim ds As DataSet = New DataSet
            If ViewState("pPID") <> Nothing Then
                'bind data
                ds = MPRModule.GetMFGCapacityProcessWC(ViewState("pPID"), 0, "")
                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblProcess.Text = ds.Tables(0).Rows(0).Item("ddMFGProcessName").ToString()
                    lblStatus.Text = IIf(ds.Tables(0).Rows(0).Item("Obsolete") = False, "ACTIVE", "INACTIVE")
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
    End Sub 'EOF BindData
#Region "GridView Events"

    Protected Sub gvProcessList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProcessList.RowDataBound
        Try
            '***
            'This section provides the user with the popup for confirming the delete of a record.
            'Called by the onClientClick event.
            '***
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' reference the Delete ImageButton
                Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                If imgBtn IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(1).Controls(1), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim price As MfgProd.MFG_Capacity_Process_WCRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, MfgProd.MFG_Capacity_Process_WCRow)

                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record for """ & DataBinder.Eval(e.Row.DataItem, "ddWorkCenterName") & """?');")
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
    End Sub 'EOF gvAPLOEM_RowDataBound

    Protected Sub gvProcessList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            lblMessage.Text = Nothing
            lblMessage.Visible = False
            lblRaiseError.Text = Nothing
            lblRaiseError.Visible = False

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            Dim UGNFacility As DropDownList
            Dim WorkCenter As DropDownList
            Dim NoOfShifts As TextBox
            Dim HrsPerShift As TextBox

            If (e.CommandName = "Insert") Then
                odsProcessList.InsertParameters("PID").DefaultValue = ViewState("pPID")

                UGNFacility = CType(gvProcessList.FooterRow.FindControl("ddUGNLocationGVF"), DropDownList)
                odsProcessList.InsertParameters("UGNFacility").DefaultValue = UGNFacility.SelectedValue

                WorkCenter = CType(gvProcessList.FooterRow.FindControl("ddWorkCenterGVF"), DropDownList)
                odsProcessList.InsertParameters("WorkCenter").DefaultValue = WorkCenter.SelectedValue

                NoOfShifts = CType(gvProcessList.FooterRow.FindControl("txtNoOfShiftsGVF"), TextBox)
                odsProcessList.InsertParameters("NoOfShifts").DefaultValue = NoOfShifts.Text

                HrsPerShift = CType(gvProcessList.FooterRow.FindControl("txtHrsPerShiftGVF"), TextBox)
                odsProcessList.InsertParameters("HrsPerShift").DefaultValue = HrsPerShift.Text

                Dim ds As DataSet = MPRModule.GetMFGCapacityProcessWC(0, WorkCenter.SelectedValue, UGNFacility.SelectedValue)
                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblRaiseError.Text = "Insert Cancelled: Work Center " & WorkCenter.SelectedValue & " was assigned to '" & ds.Tables(0).Rows(0).Item("ddMFGProcessName").ToString() & "'."
                    lblRaiseError.Visible = True
                Else
                    odsProcessList.Insert()
                End If

            End If 'EOF Insert

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvProcessList.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvProcessList.ShowFooter = True
                Else
                    gvProcessList.ShowFooter = False
                End If
            End If 'EOF Edit

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                UGNFacility = CType(gvProcessList.FooterRow.FindControl("ddUGNLocationGVF"), DropDownList)
                UGNFacility.SelectedValue = Nothing

                WorkCenter = CType(gvProcessList.FooterRow.FindControl("ddWorkCenterGVF"), DropDownList)
                WorkCenter.SelectedValue = Nothing

                NoOfShifts = CType(gvProcessList.FooterRow.FindControl("txtNoOfShiftsGVF"), TextBox)
                NoOfShifts.Text = Nothing

                HrsPerShift = CType(gvProcessList.FooterRow.FindControl("txtHrsPerShiftGVF"), TextBox)
                HrsPerShift.Text = Nothing
            End If 'EOF Undo

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub gvProcessList_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            Dim strTemp As String

            Dim strKey As String

            For Each strKey In e.NewValues.Keys

                strTemp = e.NewValues(strKey).ToString

                If strTemp.Contains(":::") Then

                    e.NewValues(strKey) = CleanBindValue(strTemp)

                End If

            Next
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF gvProcessList_RowUpdating

    Private Function CleanBindValue(ByVal DirtyValue As String) As String

        'CascadingDropDown returns BIND values as value:::text 

        'and needs to be cleaned prior to database update

        Dim strSplit() As String

        strSplit = DirtyValue.Split(":::")

        Return strSplit(0).ToString

    End Function 'EOF CleanBindValue

    Protected Sub gvProcessList_RowDeleted(ByVal sender As Object, ByVal e As GridViewDeletedEventArgs)
        lblRaiseError.Text = Nothing
        lblRaiseError.Visible = False

        If e.Exception Is Nothing Then
            If e.AffectedRows > 0 Then
                lblRaiseError.Text = "Row deleted successfully."
                lblRaiseError.Visible = True
            Else
                lblRaiseError.Text = "Row deleted successfully."
                lblRaiseError.Visible = True
            End If
        Else
            lblRaiseError.Text = "An error occured while attempting to delete a row."
            lblRaiseError.Visible = True
        End If

    End Sub 'EOF gvProcessList_RowDeleted

#End Region


#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_ReasonList() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_ReasonList") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_ReasonList"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_ReasonList") = value
        End Set

    End Property

    Protected Sub odsReasonList_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsProcessList.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As MfgProd.MFG_Capacity_Process_WCDataTable = CType(e.ReturnValue, MfgProd.MFG_Capacity_Process_WCDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_ReasonList = True
            Else
                LoadDataEmpty_ReasonList = False
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

    Protected Sub gvReasonList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProcessList.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_ReasonList
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


End Class
