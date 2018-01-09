''******************************************************************************************************
''* CustomerShutDownCalendar.aspx.vb
''* The purpose of this page is to allow users to maintain and insert Customer Shut Down days.  The data
''* will be used to calculate remaining number of production days to determine an estimated number of 
''* parts to be produced/shipped for Forecasting Future 3 data.  Calculation differs by OEM.
''*
''* Author  : LRey 04/08/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
Partial Class CustomerShutDownCalendar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc."
        m.ContentLabel = "Customer Shut Down Calendar"

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Manufacturing - Calendars</b> > Customer Shut Down Calendar"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

        ''*****
        ''Expand menu item
        ''*****
        Dim testMasterPanel As CollapsiblePanelExtender
        testMasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
        testMasterPanel.Collapsed = False

        ''*****
        ''Display Today's list of Customer Events and also include four days out 
        ''for a five day calendar view
        ''*****
        lblDay1.Text = Date.Today
        Session("Date1") = Date.Today
        lblWeekDay1.Text = WeekdayName(Weekday(Date.Today)) & ", "

        lblDay2.Text = Date.Today.AddDays(1)
        Session("Date2") = Date.Today.AddDays(1)
        lblWeekDay2.Text = WeekdayName(Weekday(Date.Today.AddDays(1))) & ", "

        lblDay3.Text = Date.Today.AddDays(2)
        Session("Date3") = Date.Today.AddDays(2)
        lblWeekDay3.Text = WeekdayName(Weekday(Date.Today.AddDays(2))) & ", "

        lblDay4.Text = Date.Today.AddDays(3)
        Session("Date4") = Date.Today.AddDays(3)
        lblWeekDay4.Text = WeekdayName(Weekday(Date.Today.AddDays(3))) & ", "

        lblDay5.Text = Date.Today.AddDays(4)
        Session("Date5") = Date.Today.AddDays(4)
        lblWeekDay5.Text = WeekdayName(Weekday(Date.Today.AddDays(4))) & ", "

        lblDay6.Text = Date.Today.AddDays(5)
        Session("Date6") = Date.Today.AddDays(5)
        lblWeekDay6.Text = WeekdayName(Weekday(Date.Today.AddDays(5))) & ", "

        lblDay7.Text = Date.Today.AddDays(6)
        Session("Date7") = Date.Today.AddDays(6)
        lblWeekDay7.Text = WeekdayName(Weekday(Date.Today.AddDays(6))) & ", "


        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

        ''*************************************************
        ''Check if IsPostBack
        ''*************************************************
        If Not Page.IsPostBack Then
            btnDelete.Attributes.Add("onclientclick", "return confirm('Are you sure you want to delete this record?');")
            BindCriteria()
            If HttpContext.Current.Request.QueryString("sCID") <> "" And HttpContext.Current.Request.QueryString("sCID") <> Nothing Then
                BindDataPerRecord() 'used to bind data at the record level
            Else
                btnDelete.Visible = False
            End If

            lnkWeek.Enabled = True
            lnkMonth.Enabled = True

            If HttpContext.Current.Request.QueryString("sView") = "Week" Then
                lnkWeek.Enabled = False
                lnkMonth.Enabled = True
            End If
            If HttpContext.Current.Request.QueryString("sView") = "Month" Then
                lnkWeek.Enabled = True
                lnkMonth.Enabled = False
            End If

        End If
    End Sub

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            btnSave.Enabled = False
            btnReset.Enabled = False
            btnDelete.Enabled = False
            ViewState("ObjectRole") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 78 'Customer Shut Down Form ID
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
                                        ViewState("Admin") = True
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = True
                                        ViewState("ObjectRole") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                        btnDelete.Enabled = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        btnSave.Enabled = True
                                        btnReset.Enabled = True
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        '' Use Default
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        '' Use Default
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        '' Use Default
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
    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        ''*****
        ''Display calendar details when date selected and also include four days out 
        ''for a five day calendar view
        ''*****
        lblDay1.Text = Calendar1.SelectedDate.Date
        Session("Date1") = Calendar1.SelectedDate.Date
        lblWeekDay1.Text = WeekdayName(Weekday(Calendar1.SelectedDate.Date)) & ", "

        lblDay2.Text = Calendar1.SelectedDate.Date.AddDays(1)
        Session("Date2") = Calendar1.SelectedDate.Date.AddDays(1)
        lblWeekDay2.Text = WeekdayName(Weekday(Calendar1.SelectedDate.Date.AddDays(1))) & ", "

        lblDay3.Text = Calendar1.SelectedDate.Date.AddDays(2)
        Session("Date3") = Calendar1.SelectedDate.Date.AddDays(2)
        lblWeekDay3.Text = WeekdayName(Weekday(Calendar1.SelectedDate.Date.AddDays(2))) & ", "

        lblDay4.Text = Calendar1.SelectedDate.Date.AddDays(3)
        Session("Date4") = Calendar1.SelectedDate.Date.AddDays(3)
        lblWeekDay4.Text = WeekdayName(Weekday(Calendar1.SelectedDate.Date.AddDays(3))) & ", "

        lblDay5.Text = Calendar1.SelectedDate.Date.AddDays(4)
        Session("Date5") = Calendar1.SelectedDate.Date.AddDays(4)
        lblWeekDay5.Text = WeekdayName(Weekday(Calendar1.SelectedDate.Date.AddDays(4))) & ", "

        lblDay6.Text = Calendar1.SelectedDate.Date.AddDays(5)
        Session("Date6") = Calendar1.SelectedDate.Date.AddDays(5)
        lblWeekDay6.Text = WeekdayName(Weekday(Calendar1.SelectedDate.Date.AddDays(5))) & ", "

        lblDay7.Text = Calendar1.SelectedDate.Date.AddDays(6)
        Session("Date7") = Calendar1.SelectedDate.Date.AddDays(6)
        lblWeekDay7.Text = WeekdayName(Weekday(Calendar1.SelectedDate.Date.AddDays(6))) & ", "

    End Sub
    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down UGN Facility control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

        commonFunctions.UserInfo()
        '' ddUGNFacility.SelectedValue = IIf(HttpContext.Current.Session("UserLocation") <> "UT", HttpContext.Current.Session("UserLocation"), "")
        ddUGNFacility.SelectedValue = HttpContext.Current.Session("UserLocation")


        ''bind existing data to drop down OEM control for selection criteria for search
        ds = commonFunctions.GetOEM()
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddOEM.DataSource = ds
            ddOEM.DataTextField = ds.Tables(0).Columns("OEM").ColumnName.ToString()
            ddOEM.DataValueField = ds.Tables(0).Columns("OEM").ColumnName.ToString()
            ddOEM.DataBind()
            ddOEM.Items.Insert(0, "")
        End If

        commonFunctions.UserInfo()
        ''ddUGNFacility.SelectedValue = HttpContext.Current.Session("UserId")

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = commonFunctions.GetCustomer(True)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddCustomer.DataSource = ds
            ddCustomer.DataTextField = ds.Tables(0).Columns("ddCustomerDesc").ColumnName.ToString()
            ddCustomer.DataValueField = ds.Tables(0).Columns("ddCustomerValue").ColumnName.ToString()

            ddCustomer.DataBind()
            ddCustomer.Items.Insert(0, "")
        End If

    End Sub
    Protected Sub BindDataPerRecord()
        ''*************************************************
        ''following code used to bind data at the record level
        ''*************************************************
        Dim ds As DataSet = New DataSet
        Dim CID As Integer = IIf(HttpContext.Current.Request.QueryString("sCID") = Nothing, 0, HttpContext.Current.Request.QueryString("sCID"))

        Try
            ds = CalendarModule.GetCustomerShutDownCalendarByCID(CID)

            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                If ds.Tables(0).Rows(0).Item("OEM").ToString() IsNot System.DBNull.Value Then
                    ddOEM.SelectedValue = ds.Tables(0).Rows(0).Item("OEM").ToString()
                End If
                If ds.Tables(0).Rows(0).Item("SoldToCABBV").ToString() IsNot System.DBNull.Value Then
                    ddCustomer.SelectedValue = ds.Tables(0).Rows(0).Item("SoldToCABBV").ToString()
                End If
                txtStartDate.Text = ds.Tables(0).Rows(0).Item("StartDate").ToString()

                If ds.Tables(0).Rows(0).Item("CABBV").ToString() = "Holiday" Then
                    Label2.Visible = False
                    Label3.Visible = False
                    chkHoliday.Checked = True
                End If

            End If
        Catch ex As Exception
            lblErrors.Text = "Error occurred with data bind.  Please contact the IS Application Group." & ex.Message
            lblErrors.Visible = "True"
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Dim View As String = HttpContext.Current.Request.QueryString("sView")
        Response.Redirect("CustomerShutDownCalendar.aspx?sView=" & View, False)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim CID As Integer = HttpContext.Current.Request.QueryString("sCID")
        Dim View As String = HttpContext.Current.Request.QueryString("sView")

        Try
            Dim CABBV As String = Nothing
            Dim SoldTo As Integer = 0
            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            If Not (Pos = 0) Then
                CABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                SoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If

            If CID <> Nothing Then
                '*****
                '* Update Record
                '*****
                CalendarModule.UpdateCustomerShutDownCalendar(CID, ddUGNFacility.SelectedValue, ddOEM.SelectedValue, CABBV, SoldTo, txtStartDate.Text, txtEndDate.Text, chkWkEndWorkDay.Checked)

                Response.Redirect("CustomerShutDownCalendar.aspx?sView=" & View, False)
            Else 'EOF of Update

                '*****
                '* Insert Record
                '*****
                If chkHoliday.Checked = False Then
                    CalendarModule.InsertCustomerShutDownCalendar(ddUGNFacility.SelectedValue, ddOEM.SelectedValue, CABBV, SoldTo, txtStartDate.Text, txtEndDate.Text, chkWkEndWorkDay.Checked)
                Else
                    CalendarModule.InsertCustomerShutDownCalendar(ddUGNFacility.SelectedValue, "-", "-", 0, txtStartDate.Text, txtEndDate.Text, chkWkEndWorkDay.Checked)
                End If

                Response.Redirect("CustomerShutDownCalendar.aspx?sView=" & View, False)

            End If

        Catch ex As Exception
            lblErrors.Text = "Error occurred during save record.  Please contact the IS Application Group." & ex.Message
            lblErrors.Visible = "True"
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim CID As Integer = HttpContext.Current.Request.QueryString("sCID")
        Dim View As String = HttpContext.Current.Request.QueryString("sView")

        Try
            '*****
            '* Delete Record
            '*****
            CalendarModule.DeleteCustomerShutDownCalendar(CID)

            Response.Redirect("CustomerShutDownCalendar.aspx?sView=" & View, False)
        Catch ex As Exception
            lblErrors.Text = "Error occurred during delete record.  Please contact the IS Application Group." & ex.Message
            lblErrors.Visible = "True"
        End Try
    End Sub

    Sub Calendar2_DayRender(ByVal sender As Object, ByVal e As DayRenderEventArgs)

        Dim ds As DataSet = New DataSet
        ds = CalendarModule.GetCustomerShutDownCalendar("")
        Dim row As DataRow
        Dim wkday As Integer


        Dim link As String = "<div align='left'><font size='1'><a href='CustomerShutDownCalendar.aspx?sView=Month&sCID="
        Dim s As String = e.Day.Date.ToShortDateString()

        e.Cell.Text = e.Day.Date.Day.ToString() & "<br>"

        wkday = e.Day.Date.DayOfWeek

        Dim l As LiteralControl = New LiteralControl()
        l.Text = e.Day.Date.Day.ToString() & "<br>"
        e.Cell.Controls.Add(l)

        For Each row In ds.Tables(0).Rows
            Dim scheduledDate As String = Convert.ToDateTime(row("StartDate")).ToShortDateString
            If (scheduledDate.Equals(s)) Then
                Dim lb As LinkButton = New LinkButton()

                'lb.Text = link & row("CID") & "'>" & "<img src='Images/selectuser.gif' width='15' height='14' border='0'> " & row("DisplayInfo") & "</a></font></div>" & "<br>"
                lb.Text = link & row("CID") & "'>" & row("DisplayInfo") & "</a></font></div>" & "<br>"
                e.Cell.Controls.Add(lb)
                If wkday = 0 Or wkday = 6 Then
                    e.Cell.BackColor = Color.White
                    e.Cell.BorderColor = Color.Gainsboro
                Else
                    e.Cell.BackColor = Color.WhiteSmoke
                    e.Cell.BorderColor = Color.Gainsboro
                End If
            End If
        Next

    End Sub

    Protected Sub chkHoliday_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHoliday.CheckedChanged
        If chkHoliday.Checked = True Then
            Label2.Visible = False
            Label3.Visible = False
            rfvOEM.Enabled = False
            rfvCustomer.Enabled = False
        Else
            Label2.Visible = True
            Label3.Visible = True
            rfvOEM.Enabled = True
            rfvCustomer.Enabled = True
        End If
    End Sub
End Class
