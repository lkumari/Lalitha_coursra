' ************************************************************************************************
'
' Name:		Manufacturing_Metric_List.aspx
' Purpose:	This Code Behind is for the main page of the Manufacturing Metric Module in Plant Specific Reports
'
' Date		    Author	    
' 06/01/2010    Roderick Carlson - Created
' 01/20/2011    Roderick Carlson - Roll up Work Center to Department
' 11/01/2012    Roderick Carlson - Modified - Removed ability to manually reload reports because it is now handled in SQL SSIS Packages
' ************************************************************************************************
Partial Class PlantSpecificReports_Manufacturing_Metric_List
    Inherits System.Web.UI.Page

    Protected WithEvents lnkStatus As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkMonth As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkYear As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkUGNFacility As System.Web.UI.WebControls.LinkButton

    Protected Function SetBackGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "White" 'N/A or 3-Completed 

        Try
            Select Case StatusID
                Case "1" 'open
                    strReturnValue = "Fuchsia"
                Case "2" 'in-process
                    strReturnValue = "Yellow"              
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetBackGroundColor = strReturnValue

    End Function

    Protected Function SetForeGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "Black"

        Try
            Select Case StatusID
                'Case "7", "8", "10"  'rejected and void
                '    strReturnValue = "White"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetForeGroundColor = strReturnValue

    End Function

    Protected Function SetPreviewHyperLink(ByVal ReportID As String) As String

        Dim strReturnValue As String = ""

        Try
            If ReportID <> "" Then
                strReturnValue = "javascript:void(window.open('crPreview_Manufacturing_Metric_Report.aspx?ReportType=M&ReportID=" & ReportID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewHyperLink = strReturnValue

    End Function

    Protected Function SetPreviewVisible(ByVal StatusID As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try
            'do not show voided reports
            If StatusID <> "4" Then
                bReturnValue = True
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewVisible = bReturnValue

    End Function

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
                ViewState("TeamMemberID") = iTeamMemberID

                ''Plant Controller
                'dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 20)
                'If commonFunctions.CheckDataset(dsSubscription) = True Then
                '    ViewState("SubscriptionID") = 20
                'End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 106)

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

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

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

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            ds = PSRModule.GetManufacturingMetricSearch(ViewState("ReportID"), ViewState("MonthID"), ViewState("YearID"), ViewState("UGNFacility"), ViewState("StatusID"), ViewState("CreatedByTMID"))

            If commonFunctions.CheckDataset(ds) = True Then

                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpSearchResult.DataSource = dv
                rpSearchResult.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()

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

    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles lnkStatus.Click, lnkMonth.Click, lnkYear.Click, lnkUGNFacility.Click

        Try
            Dim lnkButton As New LinkButton
            lnkButton = CType(sender, LinkButton)
            Dim order As String = lnkButton.CommandArgument
            Dim sortType As String = ViewState(lnkButton.ID)

            SortByColumn(order + " " + sortType)
            If sortType = "ASC" Then
                'if column set to ascending sort, change to descending for next click on that column
                lnkButton.CommandName = "DESC"
                ViewState(lnkButton.ID) = "DESC"
            Else
                lnkButton.CommandName = "ASC"
                ViewState(lnkButton.ID) = "ASC"
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

    Public Property CurrentPage() As Integer

        Get
            ' look for current page in ViewState
            Dim o As Object = ViewState("_CurrentPage")
            If (o Is Nothing) Then
                Return 0 ' default page index of 0
            Else
                Return o
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("_CurrentPage") = value
        End Set

    End Property

    Private Sub BindData()

        Try

            Dim ds As DataSet

            'bind existing AR Event data to repeater control at bottom of screen                       
            ds = PSRModule.GetManufacturingMetricSearch(ViewState("ReportID"), ViewState("MonthID"), ViewState("YearID"), ViewState("UGNFacility"), ViewState("StatusID"), ViewState("CreatedByTMID"))

            If commonFunctions.CheckDataset(ds) = True Then

                rpSearchResult.DataSource = ds
                rpSearchResult.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 15

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpSearchResult.DataSource = objPds
                rpSearchResult.DataBind()

                '' Disable Prev or Next buttons if necessary            
                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                ViewState("LastPageCount") = objPds.PageCount - 1
                txtGoToPage.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdFirst.Enabled = Not objPds.IsFirstPage
                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdNext.Enabled = Not objPds.IsLastPage
                cmdLast.Enabled = Not objPds.IsLastPage

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

    'Private Sub EnableControls()

    '    Try

    '        btnAdd.Enabled = ViewState("isAdmin")

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Private Sub BindCriteria()

        Try
            ''bind existing data to drop down controls for selection criteria for search       

            Dim ds As DataSet

            ds = commonFunctions.GetMonth("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddMonth.DataSource = ds
                ddMonth.DataTextField = ds.Tables(0).Columns("MonthName").ColumnName.ToString()
                ddMonth.DataValueField = ds.Tables(0).Columns("MonthID").ColumnName
                ddMonth.DataBind()
                ddMonth.Items.Insert(0, "")
            End If


            ds = PSRModule.GetManufacturingMetricStatusList()
            If commonFunctions.CheckDataset(ds) = True Then
                ddStatus.DataSource = ds
                ddStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddStatus.DataBind()
                ddStatus.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetTeamMemberBySubscription(20)
            If commonFunctions.CheckDataset(ds) = True Then
                ddCreatedByTMID.DataSource = ds
                ddCreatedByTMID.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddCreatedByTMID.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddCreatedByTMID.DataBind()
                ddCreatedByTMID.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Manufacturing Metric Monthly Reports"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Manufacturing - Plant Specific Reports </b> > Manufacturing Metric Monthly Report List "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

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



            If HttpContext.Current.Session("session-PSR-MM-CurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("session-PSR-MM-CurrentPage")
            End If

            PSRModule.CleanPSRMMCrystalReports()

            If Not Page.IsPostBack Then

                CheckRights()

                ViewState("lnkStatus") = "ASC"
                ViewState("lnkMonth") = "ASC"
                ViewState("lnkYear") = "ASC"
                ViewState("lnkUGNFacility") = "ASC"

                ViewState("StatusID") = 0
                ViewState("MonthID") = 0
                ViewState("YearID") = 0
                ViewState("UGNFacility") = ""
                ViewState("CreatedByTMID") = 0

                ' ''******
                ' '' Bind drop down lists
                ' ''******
                BindCriteria()

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******

                If HttpContext.Current.Request.QueryString("StatusID") <> "" Then
                    ViewState("StatusID") = HttpContext.Current.Request.QueryString("StatusID")
                    If ViewState("StatusID") > 0 Then
                        ddStatus.SelectedValue = HttpContext.Current.Request.QueryString("StatusID")
                    End If
                Else
                    If Not Request.Cookies("PSR-MM-Module_SaveStatusIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("PSR-MM-Module_SaveStatusIDSearch").Value) <> "" Then
                            ViewState("StatusID") = Request.Cookies("PSR-MM-Module_SaveStatusIDSearch").Value
                            If ViewState("StatusID") > 0 Then
                                ddStatus.SelectedValue = Request.Cookies("PSR-MM-Module_SaveStatusIDSearch").Value
                            End If
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("MonthID") <> "" Then
                    ViewState("MonthID") = HttpContext.Current.Request.QueryString("MonthID")
                    If ViewState("MonthID") > 0 Then
                        ddMonth.SelectedValue = HttpContext.Current.Request.QueryString("MonthID")
                    End If
                Else
                    If Not Request.Cookies("PSR-MM-Module_SaveMonthIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("PSR-MM-Module_SaveMonthIDSearch").Value) <> "" Then
                            ViewState("MonthID") = Request.Cookies("PSR-MM-Module_SaveMonthIDSearch").Value
                            If ViewState("MonthID") > 0 Then
                                ddMonth.SelectedValue = Request.Cookies("PSR-MM-Module_SaveMonthIDSearch").Value
                            End If
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("YearID") <> "" Then
                    ViewState("YearID") = HttpContext.Current.Request.QueryString("YearID")
                    If ViewState("YearID") > 0 Then
                        ddYear.SelectedValue = HttpContext.Current.Request.QueryString("YearID")
                    End If
                Else
                    If Not Request.Cookies("PSR-MM-Module_SaveYearIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("PSR-MM-Module_SaveYearIDSearch").Value) <> "" Then
                            ViewState("YearID") = Request.Cookies("PSR-MM-Module_SaveYearIDSearch").Value
                            If ViewState("YearID") > 0 Then
                                ddYear.SelectedValue = Request.Cookies("PSR-MM-Module_SaveYearIDSearch").Value
                            End If
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddUGNFacility.SelectedValue = HttpContext.Current.Request.QueryString("UGNFacility")
                    ViewState("UGNFacility") = HttpContext.Current.Request.QueryString("UGNFacility")
                Else
                    If Not Request.Cookies("PSR-MM-Module_SaveUGNFacilitySearch") Is Nothing Then
                        If Trim(Request.Cookies("PSR-MM-Module_SaveUGNFacilitySearch").Value) <> "" Then
                            ddUGNFacility.SelectedValue = Request.Cookies("PSR-MM-Module_SaveUGNFacilitySearch").Value
                            ViewState("UGNFacility") = Request.Cookies("PSR-MM-Module_SaveUGNFacilitySearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CreatedByTMID") <> "" Then
                    ddCreatedByTMID.SelectedValue = HttpContext.Current.Request.QueryString("CreatedByTMID")
                    ViewState("CreatedByTMID") = HttpContext.Current.Request.QueryString("CreatedByTMID")
                Else
                    If Not Request.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch").Value) <> "" Then
                            ddCreatedByTMID.SelectedValue = Request.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch").Value
                            ViewState("CreatedByTMID") = Request.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch").Value
                        End If
                    End If
                End If

                ''load repeater control
                BindData()
            Else

                If ddStatus.SelectedIndex > 0 Then
                    ViewState("StatusID") = ddStatus.SelectedValue
                Else
                    ViewState("StatusID") = 0
                End If

                If ddMonth.SelectedIndex > 0 Then
                    ViewState("MonthID") = ddMonth.SelectedValue
                Else
                    ViewState("MonthID") = 0
                End If

                If ddYear.SelectedIndex > 0 Then
                    ViewState("YearID") = ddYear.SelectedValue
                Else
                    ViewState("YearID") = 0
                End If

                If ddUGNFacility.SelectedIndex > 0 Then
                    ViewState("UGNFacility") = ddUGNFacility.SelectedValue
                Else
                    ViewState("UGNFacility") = ""
                End If

                If ddCreatedByTMID.SelectedIndex > 0 Then
                    ViewState("CreatedByTMID") = ddCreatedByTMID.SelectedValue
                Else
                    ViewState("CreatedByTMID") = 0
                End If

            End If

            'EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

    '    Try

    '        Response.Redirect("Manufacturing_Metric_Detail.aspx", False)

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try

            HttpContext.Current.Session("session-PSR-MM-CurrentPage") = Nothing

            If ddStatus.SelectedIndex > 0 Then
                Response.Cookies("PSR-MM-Module_SaveStatusIDSearch").Value = ddStatus.SelectedValue
            Else
                Response.Cookies("PSR-MM-Module_SaveStatusIDSearch").Value = 0
                Response.Cookies("PSR-MM-Module_SaveStatusIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddMonth.SelectedIndex > 0 Then
                Response.Cookies("PSR-MM-Module_SaveMonthIDSearch").Value = ddMonth.SelectedValue
            Else
                Response.Cookies("PSR-MM-Module_SaveMonthIDSearch").Value = 0
                Response.Cookies("PSR-MM-Module_SaveMonthIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddYear.SelectedIndex > 0 Then
                Response.Cookies("PSR-MM-Module_SaveYearIDSearch").Value = ddYear.SelectedValue
            Else
                Response.Cookies("PSR-MM-Module_SaveYearIDSearch").Value = 0
                Response.Cookies("PSR-MM-Module_SaveYearIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddUGNFacility.SelectedIndex > 0 Then
                Response.Cookies("PSR-MM-Module_SaveUGNFacilitySearch").Value = ddUGNFacility.SelectedValue
            Else
                Response.Cookies("PSR-MM-Module_SaveUGNFacilitySearch").Value = ""
                Response.Cookies("PSR-MM-Module_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddCreatedByTMID.SelectedIndex > 0 Then
                Response.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch").Value = ddCreatedByTMID.SelectedValue
            Else
                Response.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch").Value = 0
                Response.Cookies("PSR-MM-Module_SaveCreatedByTMIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Redirect("Manufacturing_Metric_List.aspx?StatusID=" & ViewState("StatusID") & "&MonthID=" & ViewState("MonthID") _
            & "&YearID=" & ViewState("YearID") & "&UGNFacility=" & ViewState("UGNFacility") _
            & "&CreatedByTMID=" & ViewState("CreatedByTMID"), False)

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

            PSRModule.DeletePSRMMCookies()

            HttpContext.Current.Session("session-PSR-MM-CurrentPage") = Nothing

            Response.Redirect("Manufacturing_Metric_List.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("session-PSR-MM-CurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("session-PSR-MM-CurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("session-PSR-MM-CurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            If txtGoToPage.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If


                HttpContext.Current.Session("session-PSR-MM-CurrentPage") = CurrentPage

                ' Reload control
                BindData()
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

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("session-PSR-MM-CurrentPage") = CurrentPage

            ' Reload control
            BindData()
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
