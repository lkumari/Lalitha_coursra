' ************************************************************************************************
'
' Name:		Manufacturing_Metric_Report_Selection.aspx
' Purpose:	This Code Behind is for the detail page of the Manufacturing Metric Report selection in Plant Specific Reports
'
' Date		    Author	    
' 06/29/2010    Roderick Carlson - Created
' 01/11/2011    Roderick Carlson - Modified: Handle if no reports for new year have been made
' 01/27/2011    Roderick Carlson - Modified: Allow Reports for "ALL UGN FACILITIES"
' 03/10/2011    Roderick Carlson - Modified: Add MTD Report Selection
' 03/31/2011    Roderick Carlson - Modified: Add Date Range Reports  
' 04/08/2013    Roderick Carlson - Modified: Add Monthly Actuals Compare 
' ************************************************************************************************

Partial Class Manufacturing_Metric_Report_Selection
    Inherits System.Web.UI.Page

    Private Sub BindCriteria()

        Try
            ''bind existing data to drop down controls for selection criteria for search       
            Dim ds As DataSet

            ds = PSRModule.GetManufacturingMetricDailyReportDateList()
            If commonFunctions.CheckDataset(ds) = True Then
                ddDay.DataSource = ds
                ddDay.DataTextField = ds.Tables(0).Columns("ddReportDate").ColumnName.ToString()
                ddDay.DataValueField = ds.Tables(0).Columns("ddReportDate").ColumnName
                ddDay.DataBind()                
            End If

            'ds = PSRModule.GetManufacturingMetricMonthList()
            ds = commonFunctions.GetMonth("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddMonth.DataSource = ds
                'ddMonth.DataTextField = ds.Tables(0).Columns("ddMonthName").ColumnName.ToString()
                ddMonth.DataTextField = ds.Tables(0).Columns("MonthName").ColumnName.ToString()
                ddMonth.DataValueField = ds.Tables(0).Columns("MonthID").ColumnName
                ddMonth.DataBind()                
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()               
            End If

            ds = PSRModule.GetManufacturingMetricWeeklyReportDateList()
            If commonFunctions.CheckDataset(ds) = True Then
                ddWeek.DataSource = ds
                ddWeek.DataTextField = ds.Tables(0).Columns("ddReportDateText").ColumnName.ToString()
                ddWeek.DataValueField = ds.Tables(0).Columns("ddReportDateValue").ColumnName
                ddWeek.DataBind()
            End If

            'ds = PSRModule.GetManufacturingMetricYearList()
            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()             
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            'Dim dsSubscription As DataSet

            ViewState("SubscriptionID") = 0
            ViewState("isAdmin") = False
            ViewState("TeamMemberID") = 0

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0


            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'If iTeamMemberID = 530 Then
                '    iTeamMemberID = 171
                'End If

                ViewState("TeamMemberID") = iTeamMemberID

                ''Plant Controller
                'dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 20)
                'If commonFunctions.CheckDataset(dsSubscription) = True Then
                '    ViewState("SubscriptionID") = 20
                'End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 107)

                If commonFunctions.CheckDataset(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                            ViewState("isAdmin") = True
                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                            ViewState("isAdmin") = False
                    End Select
                End If

            End If

            ' ''test developer as another team member
            'If ViewState("TeamMemberID") = 530 Then                
            '    ViewState("TeamMemberID") = 246
            '    ViewState("SubscriptionID") = 9
            '    ViewState("isAdmin") = True
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub EnableControls()

        Try

            Dim ds As DataSet
            Dim dsSubscription As DataSet

            Dim bFoundYear As Boolean = False
            Dim iRowCounter As Integer = 0

            Dim strDay As String = "" 'DateTime.Now.ToString("MM/dd/yyyy")
            Dim strWeek As String = ""

            Dim iMonthID As Integer = Today.Month - 1

            If iMonthID = 0 Then
                iMonthID = 12
            End If

            Dim iYear As Integer = Today.Year

            For iRowCounter = 0 To ddYear.Items.Count - 1
                If iYear = ddYear.Items(iRowCounter).Value Then
                    bFoundYear = True
                End If
            Next

            If bFoundYear = False Then
                iYear = iYear - 1
            End If

            Dim strReportType As String = "D"
            Dim strUGNFacility As String = ""

            If ddReportType.SelectedIndex > 0 Then
                strReportType = ddReportType.SelectedValue
            Else
                ddReportType.SelectedValue = strReportType
            End If

            If ddDay.SelectedIndex > 0 Then
                strDay = ddDay.SelectedValue
            Else
                If strReportType = "D" Then
                    ddDay.SelectedValue = ddDay.Items(0).Value 'strDay
                End If
            End If

            If ddWeek.SelectedIndex > 0 Then
                strWeek = ddWeek.SelectedValue
            Else
                If strReportType = "W" Then
                    ddWeek.SelectedValue = ddWeek.Items(0).Value
                End If
            End If

            If ddYear.SelectedIndex > 0 Then
                iYear = ddYear.SelectedValue
            Else
                If strReportType = "M" Or strReportType = "Y" Or strReportType = "MAC" Then
                    ddYear.SelectedValue = iYear
                End If
            End If

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            Else
                'find default UGNFacility 
                dsSubscription = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(20, "")
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    For iRowCounter = 0 To dsSubscription.Tables(0).Rows.Count - 1
                        If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
                            If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") > 0 Then
                                If dsSubscription.Tables(0).Rows(iRowCounter).Item("TMID") = ViewState("TeamMemberID") Then
                                    ddUGNFacility.SelectedValue = dsSubscription.Tables(0).Rows(iRowCounter).Item("UGNFacility").ToString
                                End If '= ViewState("TeamMemberID")
                            End If 'Item("TMID") > 0 
                        End If 'System.DBNull.Value
                    Next
                End If 'not empty
            End If

            If ddMonth.SelectedIndex > 0 Then
                iMonthID = ddMonth.SelectedValue
            Else
                If strReportType = "M" Then
                    'check if report exist for the month
                    ds = PSRModule.GetManufacturingMetricSearch("", iMonthID, iYear, strUGNFacility, 0, 0)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        ddMonth.SelectedValue = iMonthID
                    End If
                End If
            End If

            ddDay.Visible = False
            ddMonth.Visible = False
            ddWeek.Visible = False
            ddYear.Visible = False

            lblDayLabel.Visible = False
            lblDayMarker.Visible = False

            lblMonthLabel.Visible = False
            lblMonthMarker.Visible = False

            lblWeekLabel.Visible = False
            lblWeekMarker.Visible = False

            lblYearLabel.Visible = False
            lblYearMarker.Visible = False

            lblStartDateLabel.Visible = False
            lblStartDateMarker.Visible = False

            lblEndDateLabel.Visible = False
            lblEndDateMarker.Visible = False

            imgStartDate.Visible = False
            imgEndDate.Visible = False

            txtStartDate.Visible = False
            txtEndDate.Visible = False

            rfvDay.Enabled = False
            rfvMonth.Enabled = False
            rfvWeek.Enabled = False
            rfvYear.Enabled = False

            revStartDate.Enabled = False
            rfvStartDate.Enabled = False

            revEndDate.Enabled = False
            rfvEndDate.Enabled = False

            lblUGNFacilityMarker.Visible = False
            lblUGNFacilityLabel.Visible = False
            ddUGNFacility.Visible = False

            Select Case strReportType
                Case "D"

                    ddDay.Visible = True

                    lblDayLabel.Visible = True
                    lblDayMarker.Visible = True

                    rfvDay.Enabled = True

                    lblUGNFacilityMarker.Visible = True
                    lblUGNFacilityLabel.Visible = True
                    ddUGNFacility.Visible = True

                Case "M"

                    ddMonth.Visible = True
                    ddYear.Visible = True

                    lblMonthLabel.Visible = True
                    lblMonthMarker.Visible = True

                    lblYearLabel.Visible = True
                    lblYearMarker.Visible = True

                    rfvMonth.Enabled = True
                    rfvYear.Enabled = True

                    lblUGNFacilityMarker.Visible = True
                    lblUGNFacilityLabel.Visible = True
                    ddUGNFacility.Visible = True

                Case "MAC"

                    ddMonth.Visible = True
                    ddYear.Visible = True

                    lblMonthLabel.Visible = True
                    lblMonthMarker.Visible = True

                    lblYearLabel.Visible = True
                    lblYearMarker.Visible = True

                    rfvMonth.Enabled = True
                    rfvYear.Enabled = True

                Case "MTD"

                    lblUGNFacilityMarker.Visible = True
                    lblUGNFacilityLabel.Visible = True
                    ddUGNFacility.Visible = True

                Case "W"

                    ddWeek.Visible = True

                    lblWeekLabel.Visible = True
                    lblWeekMarker.Visible = True

                    rfvWeek.Enabled = True

                    lblUGNFacilityMarker.Visible = True
                    lblUGNFacilityLabel.Visible = True
                    ddUGNFacility.Visible = True

                Case "Y"

                    ddYear.Visible = True

                    lblYearLabel.Visible = True
                    lblYearMarker.Visible = True

                    rfvYear.Enabled = True

                    lblUGNFacilityMarker.Visible = True
                    lblUGNFacilityLabel.Visible = True
                    ddUGNFacility.Visible = True

                Case "YTD"

                    lblUGNFacilityMarker.Visible = True
                    lblUGNFacilityLabel.Visible = True
                    ddUGNFacility.Visible = True

                Case "DR"

                    lblStartDateLabel.Visible = True
                    lblStartDateMarker.Visible = True

                    lblEndDateLabel.Visible = True
                    lblEndDateMarker.Visible = True

                    imgStartDate.Visible = True
                    imgEndDate.Visible = True

                    txtStartDate.Visible = True
                    txtEndDate.Visible = True

                    revStartDate.Enabled = True
                    rfvStartDate.Enabled = True

                    revEndDate.Enabled = True
                    rfvEndDate.Enabled = True

                    lblUGNFacilityMarker.Visible = True
                    lblUGNFacilityLabel.Visible = True
                    ddUGNFacility.Visible = True

            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Manufacturing Metrics Report Selection"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Manufacturing - Plant Specific Reports </b> > <a href='Manufacturing_Metric_List.aspx'><b> Manufacturing Metric Monthly Report List </b></a> > Manufacturing Metric Report Selection "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            '*****
            'Expand menu item
            '*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            PSRModule.CleanPSRMMCrystalReports()

            If Not Page.IsPostBack Then

                CheckRights()

                BindCriteria()

                ddReportType.SelectedValue = "D"

                EnableControls()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Try
            lblMessage.Text = ""

            Dim bValidDateRange As Boolean = False

            'Dim strDay As String = "" 'DateTime.Now.ToString("MM/dd/yyyy")
            Dim strWeek As String = ""
            Dim strReportStartDate As String = ""
            Dim strReportEndDate As String = ""
            Dim strSplitterText As String = "::"

            Dim iSplitterLocation As Integer = 0

            Dim iFirstSlash As Integer = 0

            Dim strMonth As String = ""
            Dim strDay As String = ""
            Dim strYear As String = ""

            Dim iMonthID As Integer = Today.Month - 1

            If iMonthID = 0 Then
                iMonthID = 12
            End If

            Dim iYear As Integer = Today.Year

            Dim strReportType As String = "D"
            Dim strUGNFacility As String = ""

            If ddReportType.SelectedIndex > 0 Then
                strReportType = ddReportType.SelectedValue
            End If

            If strReportType = "D" Then
                If ddDay.SelectedIndex > 0 Then
                    'strDay = ddDay.SelectedValue
                    strReportStartDate = ddDay.SelectedValue
                    strReportEndDate = ddDay.SelectedValue
                Else
                    'strDay = ddDay.Items(0).Value
                    strReportStartDate = ddDay.Items(0).Value
                    strReportEndDate = ddDay.Items(0).Value
                End If
            End If

            If strReportType = "M" Or strReportType = "MAC" Then
                If ddMonth.SelectedIndex > 0 Then
                    iMonthID = ddMonth.SelectedValue
                Else
                    iMonthID = ddMonth.Items(0).Value
                End If
            End If

            If strReportType = "W" Then
                If ddWeek.SelectedIndex > 0 Then
                    strWeek = ddWeek.SelectedValue
                Else
                    strWeek = ddWeek.Items(0).Value
                End If

                If strWeek <> "" Then
                    iSplitterLocation = InStr(strWeek, strSplitterText)

                    If iSplitterLocation > 0 Then
                        strReportStartDate = Mid(strWeek, 1, iSplitterLocation - 1)
                        strReportEndDate = Mid(strWeek, iSplitterLocation + strSplitterText.Length)
                    End If
                End If
            End If

            'drop downs will take precedence over date range text fields
            If txtStartDate.Text.Trim <> "" And strReportStartDate = "" Then
                strReportStartDate = txtStartDate.Text.Trim
            End If

            If txtEndDate.Text.Trim <> "" And strReportEndDate = "" Then
                strReportEndDate = txtEndDate.Text.Trim
            End If

            If strReportStartDate <> "" Then
                strReportStartDate = strReportStartDate.Replace("\", "/")
                strReportStartDate = strReportStartDate.Replace("-", "/")
                iFirstSlash = InStr(strReportStartDate, "/")
                strMonth = Mid$(strReportStartDate, 1, iFirstSlash)
                strMonth = strMonth.Replace("/", "").PadLeft(2, "0")
                strDay = Mid$(strReportStartDate, iFirstSlash + 1, 2)
                strDay = strDay.Replace("/", "").PadLeft(2, "0")
                strYear = Right$(strReportStartDate, 4)
                strReportStartDate = strMonth & "/" & strDay & "/" & strYear
            End If

            If strReportEndDate <> "" Then
                strReportEndDate = strReportEndDate.Replace("\", "/")
                strReportEndDate = strReportEndDate.Replace("-", "/")
                iFirstSlash = InStr(strReportEndDate, "/")
                strMonth = Mid$(strReportEndDate, 1, iFirstSlash)
                strMonth = strMonth.Replace("/", "").PadLeft(2, "0")
                strDay = Mid$(strReportEndDate, iFirstSlash + 1, 2)
                strDay = strDay.Replace("/", "").PadLeft(2, "0")
                strYear = Right$(strReportEndDate, 4)
                strReportEndDate = strMonth & "/" & strDay & "/" & strYear
            End If

            If strReportType = "Y" Or strReportType = "M" Or strReportType = "MAC" Then
                If ddYear.SelectedIndex > 0 Then
                    iYear = ddYear.SelectedValue
                Else
                    iYear = ddYear.Items(0).Value
                End If
            End If

            If ddUGNFacility.SelectedIndex > 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            Else
                strUGNFacility = ddUGNFacility.Items(0).Value
            End If

            'If ddReportType.SelectedIndex > 0 Then
            '    strReportType = ddReportType.SelectedValue
            'End If

            'Response.Redirect("crPreview_Manufacturing_Metric_Report.aspx?ReportType=" & strReportType & "&MonthID=" & iMonthID & "&YearID=" & iYear & "&UGNFacility=" & strUGNFacility, False)

            'clear crystal reports
            PSRModule.CleanPSRMMCrystalReports()

            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Manufacturing Metric Report", "window.open('crPreview_Manufacturing_Metric_Report.aspx?ReportType=" & strReportType & "&ReportDate=" & strDay & "&MonthID=" & iMonthID & "&YearID=" & iYear & "&ReportStartDate=" & strReportStartDate & "&ReportEndDate=" & strReportEndDate & "&UGNFacility=" & strUGNFacility & "'," & Now.Ticks & ",'top=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)

            If strReportType = "DR" Then
                If strReportStartDate <> "" And strReportEndDate <> "" Then
                    If CType(strReportStartDate, Date) <= CType(strReportEndDate, Date) Then
                        bValidDateRange = True
                    End If
                End If
            Else
                bValidDateRange = True
            End If

            If bValidDateRange = True Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Manufacturing Metric Report", "window.open('crPreview_Manufacturing_Metric_Report.aspx?ReportType=" & strReportType & "&MonthID=" & iMonthID & "&YearID=" & iYear & "&ReportStartDate=" & strReportStartDate & "&ReportEndDate=" & strReportEndDate & "&UGNFacility=" & strUGNFacility & "'," & Now.Ticks & ",'top=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)
            Else
                lblMessage.Text = "Error: Invalid Date Range"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            'clear crystal reports
            PSRModule.CleanPSRMMCrystalReports()

            ddDay.SelectedIndex = -1
            ddMonth.SelectedIndex = -1
            ddWeek.SelectedIndex = -1
            ddYear.SelectedIndex = -1
            ddUGNFacility.SelectedIndex = -1
            ddReportType.SelectedValue = "D"

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ddReportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReportType.SelectedIndexChanged

        Try
            lblMessage.Text = ""

            EnableControls()

         Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
