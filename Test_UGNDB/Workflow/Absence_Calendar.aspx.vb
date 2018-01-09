''******************************************************************************************************
''* Absence_Calendar.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new Team_Member_Absence data.
''*
''* Author  : LRey 05/28/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
Partial Class Workflow_Absence_Calendar
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.masterpage_master = Master
        m.PageTitle = "UGN, Inc."
        m.ContentLabel = "Team Member Scheduler"

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Work Flow</b> > Team Member Scheduler"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

        ''*****
        ''Expand menu item
        ''*****
        Dim testMasterPanel As CollapsiblePanelExtender
        testMasterPanel = CType(Master.FindControl("WFExtender"), CollapsiblePanelExtender)
        testMasterPanel.Collapsed = False

        ''*****
        ''Display Today's list of Team Member Events and also include four days out 
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


        End If
    End Sub

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
    End Sub
    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = commonFunctions.GetTeamMember("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddTeamMember.DataSource = ds
            ddTeamMember.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
            ddTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
            ddTeamMember.DataBind()
            ddTeamMember.Items.Insert(0, "")
        End If

        commonFunctions.UserInfo()
        ddTeamMember.SelectedValue = HttpContext.Current.Session("UserId")

    End Sub
    Protected Sub BindDataPerRecord()
        ''*************************************************
        ''following code used to bind data at the record level
        ''*************************************************
        Dim ds As DataSet = New DataSet
        Dim CID As String = HttpContext.Current.Request.QueryString("sCID")

        Try
            ds = WorkFlowModule.GetTeamMemberCalendarByCID(CID)

            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("TeamMemberID").ToString()
                txtEvent.Text = ds.Tables(0).Rows(0).Item("Event").ToString()
                txtStartDate.Text = ds.Tables(0).Rows(0).Item("StartDate").ToString()
                chkBackup.Checked = ds.Tables(0).Rows(0).Item("AlertBackup").ToString()

            End If
        Catch ex As Exception
            lblErrors.Text = "Error occurred with data bind.  Please contact the IS Application Group." & ex.Message
            lblErrors.Visible = "True"
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Response.Redirect("Absence_Calendar.aspx")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim CID As Integer = HttpContext.Current.Request.QueryString("sCID")

        Try
            If CID <> Nothing Then
                '*****
                '* Update Record
                '*****
                WorkFlowModule.UpdateTeamMemberCalendar(CID, ddTeamMember.SelectedValue, txtEvent.Text, txtStartDate.Text, txtEndDate.Text, chkBackup.Checked)

                Response.Redirect("Absence_Calendar.aspx")
            Else 'EOF of Update

                '*****
                '* Insert Record
                '*****
                WorkFlowModule.InsertTeamMemberCalendar(ddTeamMember.SelectedValue, txtEvent.Text, txtStartDate.Text, txtEndDate.Text, chkBackup.Checked)

                Response.Redirect("Absence_Calendar.aspx")

            End If


        Catch ex As Exception
            lblErrors.Text = "Error occurred during save record.  Please contact the IS Application Group." & ex.Message
            lblErrors.Visible = "True"
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim CID As Integer = HttpContext.Current.Request.QueryString("sCID")

        Try
            '*****
            '* Delete Record
            '*****
            WorkFlowModule.DeleteTeamMemberCalendar(CID)

            Response.Redirect("Absence_Calendar.aspx")
        Catch ex As Exception
            lblErrors.Text = "Error occurred during delete record.  Please contact the IS Application Group." & ex.Message
            lblErrors.Visible = "True"
        End Try
    End Sub
End Class
