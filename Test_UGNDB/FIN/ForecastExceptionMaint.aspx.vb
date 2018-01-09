''******************************************************************************************************
''* ForecastExceptionMaint.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new Forecast_Exception_Maint data.
''*
''* Author  : LRey 01/17/2011
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Partial Class Forecast_Exception_Maint
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Forecast Exception"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Forecast Exception"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False


            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pRowID") <> "" Then
                ViewState("pRowID") = HttpContext.Current.Request.QueryString("pRowID")
            Else
                ViewState("pRowID") = ""
            End If


            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindCriteria()
                If ViewState("pRowID") <> "" Then
                    BindData()
                End If
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF Page_Load

    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet
            ''bind existing data to drop down UGN Location control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNfacility.DataSource = ds
                ddUGNfacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNfacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNfacility.DataBind()
                ddUGNfacility.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetOEM()
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddOEM.DataSource = ds
                ddOEM.DataTextField = ds.Tables(0).Columns("OEM").ColumnName.ToString()
                ddOEM.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("OEM").ColumnName.ToString()))
                ddOEM.DataBind()
                ddOEM.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetCustomer(True)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetSoldTo()
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSoldTo.DataSource = ds
                ddSoldTo.DataTextField = ds.Tables(0).Columns("SoldTo").ColumnName.ToString()
                ddSoldTo.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("SoldTo").ColumnName.ToString()))
                ddSoldTo.DataBind()
                ddSoldTo.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Destination control for selection criteria for search
            ds = commonFunctions.GetCustomerDestination("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddDestination.DataSource = ds
                ddDestination.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
                ddDestination.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("DABBV").ColumnName.ToString()))
                ddDestination.DataBind()
                ddDestination.Items.Insert(0, "")
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

    End Sub 'EOF BindCriteria

    Protected Sub BindData()
        Dim ds As DataSet = New DataSet
        Try
            lblErrors.Text = ""
            lblErrors.Visible = False

            ''***********************************************************************
            ''Validate Primary/Supplement TE exists in UGN_Database CapitalExpenditure table
            ''***********************************************************************
            ds = FINModule.GetForecastException(ViewState("pRowID"))

            If (ds.Tables.Item(0).Rows.Count > 0) Then
                lblRowID.Text = "Row ID: " & ViewState("pRowID")
                ddCompnyValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("COMPNYValidator").ToString()))
                ddUGNfacility.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("COMPNY").ToString()))
                ddOEMValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("OEMValidator").ToString()))
                ddOEM.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("OEM").ToString()))
                ddCabbvValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("CabbvValidator").ToString()))
                ddCustomer.SelectedValue = ds.Tables(0).Rows(0).Item("CABBV").ToString()
                ddSoldToValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("SoldToValidator").ToString()))
                ddSoldTo.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("SoldTo").ToString()))
                ddPartNoValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("PartNoValidator").ToString()))
                txtPartNo.Text = LTrim(RTrim(ds.Tables(0).Rows(0).Item("PARTNO").ToString()))
                ddDabbvValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("DabbvValidator").ToString()))
                ddDestination.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("DABBV").ToString()))
                ddTrnTyp.SelectedValue = ds.Tables(0).Rows(0).Item("TRNTYP").ToString()
                ddREQTYP.SelectedValue = ds.Tables(0).Rows(0).Item("REQTYP").ToString()
                ddREQFRQ.SelectedValue = ds.Tables(0).Rows(0).Item("REQFRQ").ToString()
                ddDayOfWeek.SelectedValue = ds.Tables(0).Rows(0).Item("DayOfWeekID").ToString()
                ddWeekValidator.SelectedValue = ds.Tables(0).Rows(0).Item("WeekValidator").ToString()
                txtStartOfWeek.Text = ds.Tables(0).Rows(0).Item("SWeekID").ToString()
                txtEndOfWeek.Text = ds.Tables(0).Rows(0).Item("EWeekID").ToString()
                ddMonthValidator.SelectedValue = ds.Tables(0).Rows(0).Item("MonthValidator").ToString()
                txtStartOfMonth.Text = ds.Tables(0).Rows(0).Item("SMonthID").ToString()
                txtEndOfMonth.Text = ds.Tables(0).Rows(0).Item("EMonthID").ToString()
                ddYearValidator.SelectedValue = ds.Tables(0).Rows(0).Item("YearValidator").ToString()
                txtStartOfYear.Text = ds.Tables(0).Rows(0).Item("SYearID").ToString()
                txtEndOfYear.Text = ds.Tables(0).Rows(0).Item("EYearID").ToString()
                cbWKNEFWOM.Checked = ds.Tables(0).Rows(0).Item("WKNEFWOM").ToString()
                cbWKEQFWOM.Checked = ds.Tables(0).Rows(0).Item("WKEQFWOM").ToString()
                cbRDTGTFDOM.Checked = ds.Tables(0).Rows(0).Item("RDTGTFDOM").ToString()
                cbRDTLTFDOM.Checked = ds.Tables(0).Rows(0).Item("RDTLTFDOM").ToString()
                txtQTYRQ.Text = ds.Tables(0).Rows(0).Item("ReplaceQTYRQ").ToString()
                txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString()
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
    End Sub 'EOF BindData

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on 
        '' TM() 's Security/Subscription
        ''********

        Try
            ''*******
            '' Disable controls by default
            ''*******
            btnSubmit.Enabled = False
            btnReset.Enabled = False
            ViewState("ObjectRole") = False

            If ddWeekValidator.SelectedValue = "BETWEEN" Then
                txtEndOfWeek.Enabled = True
            Else
                txtEndOfWeek.Enabled = False
            End If

            If ddMonthValidator.SelectedValue = "BETWEEN" Then
                txtEndOfMonth.Enabled = True
            Else
                txtEndOfMonth.Enabled = False
            End If

            If ddYearValidator.SelectedValue = "BETWEEN" Then
                txtEndOfYear.Enabled = True
            Else
                txtEndOfYear.Enabled = False
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 119 'Forecast Exception Maint form id
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
                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        btnSubmit.Enabled = True
                                        btnReset.Enabled = True
                                        ViewState("Admin") = "true"
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        btnSubmit.Enabled = True
                                        btnReset.Enabled = True
                                        ViewState("Admin") = "true"
                                        ViewState("ObjectRole") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        ViewState("ObjectRole") = False
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        ViewState("ObjectRole") = False
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        ViewState("ObjectRole") = False
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub
#End Region

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            Dim DefaultDate As Date = Date.Today
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUserName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value
            Dim colVal As String = Nothing
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            colVal = LTrim(RTrim(ddUGNfacility.SelectedValue & " " & ddOEM.SelectedValue & " " & ddCustomer.SelectedValue & " " & ddSoldTo.SelectedValue & " " & " " & txtPartNo.Text & " " & ddDestination.SelectedValue & " " & ddTrnTyp.SelectedValue & " " & ddREQTYP.SelectedValue & " " & ddREQFRQ.SelectedValue & " " & ddDayOfWeek.SelectedValue & " " & ddWeekValidator.SelectedValue & " " & txtStartOfWeek.Text & " " & txtEndOfWeek.Text & " " & ddMonthValidator.SelectedValue & " " & txtStartOfMonth.Text & " " & txtEndOfMonth.Text & " " & ddYearValidator.SelectedValue & " " & txtStartOfYear.Text & " " & txtEndOfYear.Text & " " & txtQTYRQ.Text & " " & txtNotes.Text))

            If colVal <> Nothing Then
                If (ViewState("pRowID") <> Nothing Or ViewState("pRowID") <> "") Then
                    FINModule.UpdateForecastException(ViewState("pRowID"), IIf(ddUGNfacility.SelectedValue = Nothing, "", IIf(ddCompnyValidator.SelectedValue = Nothing, "=", ddCompnyValidator.SelectedValue)), ddUGNfacility.SelectedValue, IIf(ddOEM.SelectedValue = Nothing, "", IIf(ddOEMValidator.SelectedValue = Nothing, "=", ddOEMValidator.SelectedValue)), ddOEM.SelectedValue, IIf(ddCustomer.SelectedValue = Nothing, "", IIf(ddCabbvValidator.SelectedValue = Nothing, "=", ddCabbvValidator.SelectedValue)), ddCustomer.SelectedValue, IIf(ddSoldTo.SelectedValue = Nothing, "", IIf(ddSoldToValidator.SelectedValue = Nothing, "=", ddSoldToValidator.SelectedValue)), ddSoldTo.SelectedValue, IIf(txtPartNo.Text = Nothing, "", IIf(ddPartNoValidator.SelectedValue = Nothing, "=", ddPartNoValidator.SelectedValue)), txtPartNo.Text, IIf(ddDestination.SelectedValue = Nothing, "", IIf(ddDabbvValidator.SelectedValue = Nothing, "=", ddDabbvValidator.SelectedValue)), ddDestination.SelectedValue, ddTrnTyp.SelectedValue, ddREQTYP.SelectedValue, ddREQFRQ.SelectedValue, IIf(ddDayOfWeek.SelectedValue = Nothing, 0, ddDayOfWeek.SelectedValue), IIf(txtStartOfWeek.Text <> Nothing And txtEndOfWeek.Text = Nothing And ddWeekValidator.SelectedValue = Nothing, "=", ddWeekValidator.SelectedValue), IIf(txtStartOfWeek.Text = Nothing, 0, txtStartOfWeek.Text), IIf(txtEndOfWeek.Text = Nothing, 0, txtEndOfWeek.Text), IIf(txtStartOfMonth.Text <> Nothing And txtEndOfMonth.Text = Nothing And ddMonthValidator.SelectedValue = Nothing, "=", ddMonthValidator.SelectedValue), IIf(txtStartOfMonth.Text = Nothing, 0, txtStartOfMonth.Text), IIf(txtEndOfMonth.Text = Nothing, 0, txtEndOfMonth.Text), IIf(txtStartOfYear.Text <> Nothing And txtEndOfYear.Text = Nothing And ddYearValidator.SelectedValue = Nothing, "=", ddYearValidator.SelectedValue), IIf(txtStartOfYear.Text = Nothing, 0, txtStartOfYear.Text), IIf(txtEndOfYear.Text = Nothing, 0, txtEndOfYear.Text), IIf(txtQTYRQ.Text = Nothing, 0, txtQTYRQ.Text), txtNotes.Text, cbWKNEFWOM.Checked, cbWKEQFWOM.Checked, cbRDTGTFDOM.Checked, cbRDTLTFDOM.Checked, DefaultUser, DefaultDate)

                Else
                    FINModule.InsertForecastException(IIf(ddUGNfacility.SelectedValue = Nothing, "", IIf(ddCompnyValidator.SelectedValue = Nothing, "=", ddCompnyValidator.SelectedValue)), ddUGNfacility.SelectedValue, IIf(ddOEM.SelectedValue = Nothing, "", IIf(ddOEMValidator.SelectedValue = Nothing, "=", ddOEMValidator.SelectedValue)), ddOEM.SelectedValue, IIf(ddCustomer.SelectedValue = Nothing, "", IIf(ddCabbvValidator.SelectedValue = Nothing, "=", ddCabbvValidator.SelectedValue)), ddCustomer.SelectedValue, IIf(ddSoldTo.SelectedValue = Nothing, "", IIf(ddSoldToValidator.SelectedValue = Nothing, "=", ddSoldToValidator.SelectedValue)), ddSoldTo.SelectedValue, IIf(txtPartNo.Text = Nothing, "", IIf(ddPartNoValidator.SelectedValue = Nothing, "=", ddPartNoValidator.SelectedValue)), txtPartNo.Text, IIf(ddDestination.SelectedValue = Nothing, "", IIf(ddDabbvValidator.SelectedValue = Nothing, "=", ddDabbvValidator.SelectedValue)), ddDestination.SelectedValue, ddTrnTyp.SelectedValue, ddREQTYP.SelectedValue, ddREQFRQ.SelectedValue, IIf(ddDayOfWeek.SelectedValue = Nothing, 0, ddDayOfWeek.SelectedValue), IIf(txtStartOfWeek.Text <> Nothing And txtEndOfWeek.Text = Nothing And ddWeekValidator.SelectedValue = Nothing, "=", ddWeekValidator.SelectedValue), IIf(txtStartOfWeek.Text = Nothing, 0, txtStartOfWeek.Text), IIf(txtEndOfWeek.Text = Nothing, 0, txtEndOfWeek.Text), IIf(txtStartOfMonth.Text <> Nothing And txtEndOfMonth.Text = Nothing And ddMonthValidator.SelectedValue = Nothing, "=", ddMonthValidator.SelectedValue), IIf(txtStartOfMonth.Text = Nothing, 0, txtStartOfMonth.Text), IIf(txtEndOfMonth.Text = Nothing, 0, txtEndOfMonth.Text), IIf(txtStartOfYear.Text <> Nothing And txtEndOfYear.Text = Nothing And ddYearValidator.SelectedValue = Nothing, "=", ddYearValidator.SelectedValue), IIf(txtStartOfYear.Text = Nothing, 0, txtStartOfYear.Text), IIf(txtEndOfYear.Text = Nothing, 0, txtEndOfYear.Text), IIf(txtQTYRQ.Text = Nothing, 0, txtQTYRQ.Text), txtNotes.Text, cbWKNEFWOM.Checked, cbWKEQFWOM.Checked, cbRDTGTFDOM.Checked, cbRDTLTFDOM.Checked, DefaultUser, DefaultDate)


                    '** Locate Max RowID in Forecast_Excption
                    Dim ds As DataSet = New DataSet
                    ds = FINModule.GetLastForecastExceptionRowID()
                    If commonFunctions.CheckDataSet(ds) = True Then
                        ViewState("pRowID") = ds.Tables(0).Rows(0).Item("LastRowID").ToString()
                    Else
                        ViewState("pRowID") = 0
                    End If
                End If

                FINModule.UpdateForecast(ViewState("pRowID"))
                gvForecastException.DataBind()

                Response.Redirect("ForecastExceptionMaint.aspx", False)
            Else
                lblErrors.Text = "Submission cancelled. An exception entry is required."
                lblErrors.Visible = True
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
    End Sub 'EOF btnSubmit_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Response.Redirect("ForecastExceptionMaint.aspx")
    End Sub 'EOF btnReset_Click

    Protected Sub gvForecastException_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvForecastException.RowCreated
        ''Do nothing
    End Sub 'EOF gvForecastException_RowCreated

    Protected Sub gvForecastException_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvForecastException.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(20).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As Financials.Forecast_ExceptionRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Financials.Forecast_ExceptionRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Exception?');")
                End If
            End If
        End If
    End Sub 'EOF gvForecastException_RowDataBound

    Protected Sub ddWeekValidator_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddWeekValidator.SelectedIndexChanged
        If ddWeekValidator.SelectedValue = "BETWEEN" Then
            txtEndOfWeek.Enabled = True
        Else
            txtEndOfWeek.Enabled = False
        End If
    End Sub

    Protected Sub ddMonthValidator_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMonthValidator.SelectedIndexChanged
        If ddMonthValidator.SelectedValue = "BETWEEN" Then
            txtEndOfMonth.Enabled = True
        Else
            txtEndOfMonth.Enabled = False
        End If
    End Sub

    Protected Sub ddYearValidator_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddYearValidator.SelectedIndexChanged
        If ddYearValidator.SelectedValue = "BETWEEN" Then
            txtEndOfYear.Enabled = True
        Else
            txtEndOfYear.Enabled = False
        End If

    End Sub

    Protected Sub ddUGNfacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUGNfacility.SelectedIndexChanged
        Try
            'BindCriteria()
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetOEMbyCOMPNY(IIf(ddUGNfacility.SelectedValue = Nothing, "", ddUGNfacility.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddOEM.DataSource = ds
                ddOEM.DataTextField = ds.Tables(0).Columns("ddOEM").ColumnName.ToString()
                ddOEM.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("OEM").ColumnName.ToString()))

                ddOEM.DataBind()
                ddOEM.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetCABBVbyOEM(IIf(ddUGNfacility.SelectedValue = Nothing, "", ddUGNfacility.SelectedValue), IIf(ddOEM.SelectedValue = Nothing, "", ddOEM.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("CABBV_OEM").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()

                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetSOLDTObyCOMPNYbyCABBVbyOEM(IIf(ddUGNfacility.SelectedValue = Nothing, "", ddUGNfacility.SelectedValue), IIf(ddCustomer.SelectedValue = Nothing, "", ddCustomer.SelectedValue), IIf(ddOEM.SelectedValue = Nothing, "", ddOEM.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSoldTo.DataSource = ds
                ddSoldTo.DataTextField = ds.Tables(0).Columns("SoldTo").ColumnName.ToString()
                ddSoldTo.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("SoldTo").ColumnName.ToString()))

                ddSoldTo.DataBind()
                ddSoldTo.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Destination control for selection criteria for search
            ds = commonFunctions.GetDABBV(IIf(ddUGNfacility.SelectedValue = Nothing, IIf(ddOEM.SelectedValue = Nothing, "", ddOEM.SelectedValue), ddUGNfacility.SelectedValue), "", 0)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddDestination.DataSource = ds
                ddDestination.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
                ddDestination.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("DABBV").ColumnName.ToString()))

                ddDestination.DataBind()
                ddDestination.Items.Insert(0, "")
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

    Protected Sub ddOEM_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOEM.SelectedIndexChanged
        Try

            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetCABBVbyOEM(IIf(ddUGNfacility.SelectedValue = Nothing, "", ddUGNfacility.SelectedValue), IIf(ddOEM.SelectedValue = Nothing, "", ddOEM.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("CABBV_OEM").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()

                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetSOLDTObyCOMPNYbyCABBVbyOEM(IIf(ddUGNfacility.SelectedValue = Nothing, "", ddUGNfacility.SelectedValue), IIf(ddCustomer.SelectedValue = Nothing, "", ddCustomer.SelectedValue), IIf(ddOEM.SelectedValue = Nothing, "", ddOEM.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSoldTo.DataSource = ds
                ddSoldTo.DataTextField = ds.Tables(0).Columns("SoldTo").ColumnName.ToString()
                ddSoldTo.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("SoldTo").ColumnName.ToString()))

                ddSoldTo.DataBind()
                ddSoldTo.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Destination control for selection criteria for search
            ds = commonFunctions.GetDABBV(IIf(ddUGNfacility.SelectedValue = Nothing, IIf(ddOEM.SelectedValue = Nothing, "", ddOEM.SelectedValue), ddUGNfacility.SelectedValue), "", 0)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddDestination.DataSource = ds
                ddDestination.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
                ddDestination.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("DABBV").ColumnName.ToString()))

                ddDestination.DataBind()
                ddDestination.Items.Insert(0, "")
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

    Protected Sub ddCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCustomer.SelectedIndexChanged
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetSOLDTObyCOMPNYbyCABBVbyOEM(IIf(ddUGNfacility.SelectedValue = Nothing, "", ddUGNfacility.SelectedValue), IIf(ddCustomer.SelectedValue = Nothing, "", ddCustomer.SelectedValue), IIf(ddOEM.SelectedValue = Nothing, "", ddOEM.SelectedValue))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSoldTo.DataSource = ds
                ddSoldTo.DataTextField = ds.Tables(0).Columns("SoldTo").ColumnName.ToString()
                ddSoldTo.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("SoldTo").ColumnName.ToString()))

                ddSoldTo.DataBind()
                ddSoldTo.Items.Insert(0, "")
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
End Class
