' ************************************************************************************************
'
' Name:		crAR_Event_Approval.aspx
' Purpose:	This Code Behind is for the AR Event Approval and Crystal Report
'
' Date		Author	    
' 04/01/2010    Roderick Carlson
' 08/19/2011    Roderick Carlson - when a team member of UGN Assist approves, do not change approver ID
'                                - when accounting manager approves and Invoices on Hold event, close it also
' 09/20/2012    Roderick Carlson - adjusted for new approval routing
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class crAR_Event_Approval
    Inherits System.Web.UI.Page

    Protected Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = ARGroupModule.GetARApprovalStatusList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddApprovalStatus.DataSource = ds
                ddApprovalStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddApprovalStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddApprovalStatus.DataBind()
                'ddApprovalStatus.Items.Insert(0, "")
            End If

            ds = ARGroupModule.GetAREventStatusList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddEventStatus.DataSource = ds
                ddEventStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddEventStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddEventStatus.DataBind()
                'ddEventStatus.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

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
            Dim dsSubscription As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("SubscriptionID") = 0
            ViewState("isDefaultBilling") = False

            ViewState("TeamMemberID") = 0

            ViewState("isAdmin") = False
            ViewState("isAssist") = False
            'ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            'dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Gina.Lacny", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                ''test developer as another team member
                If iTeamMemberID = 530 Then
                    'mike echevarria
                    'iTeamMemberID = 246

                    'Brett.Barta 
                    'iTeamMemberID = 2

                    'gina lacny
                    iTeamMemberID = 627

                    'Ilysa.Albright 
                    'iTeamMemberID = 636

                    'Kara.North 
                    'iTeamMemberID = 667

                    'Kelly.Carolyn 
                    'iTeamMemberID = 638

                    'gary hibbler
                    'iTeamMemberID = 671

                    'randy.khalaf 
                    'iTeamMemberID = 569

                    'Peter.Anthony 
                    'iTeamMemberID = 6

                    'Julie.Sinchak()
                    'iTeamMemberID = 303

                    'Kenta.Shinohara 
                    'iTeamMemberID = 4
                End If

                ViewState("TeamMemberID") = iTeamMemberID

                'Accounting
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 21)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 21
                End If

                'is Default Accounting Manager
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 79)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultBilling") = True
                End If

                'Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 9)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 9
                End If

                'VP of Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 23)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 23
                End If

                'CFO
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 33)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 33
                End If

                'CEO
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 24)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 24
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 49)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)                                    
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                            ViewState("isAssist") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.crviewmasterpage_master = Master

            InitializeViewState()

            ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")

            If ViewState("AREID") <> "" Then

                Dim oRpt As ReportDocument = New ReportDocument()

                If ViewState("AREID") <> Session("ARPreviewAREID") Then
                    Session("ARPreviewAREID") = Nothing
                    Session("ARPreview") = Nothing
                End If

                If Session("ARPreview") Is Nothing Then

                    Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                    Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                    Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crAREvent.rpt")

                    'getting the database, the table and the LogOnInfo object which holds login onformation 
                    crDatabase = oRpt.Database

                    'getting the table in an object array of one item 
                    Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
                    crDatabase.Tables.CopyTo(arrTables, 0)
                    ' assigning the first item of array to crTable by downcasting the object to Table 
                    crTable = arrTables(0)

                    ' setting values 
                    dbConn = crTable.LogOnInfo
                    dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()
                    dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString()
                    dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                    ' applying login info to the table object 
                    crTable.ApplyLogOnInfo(dbConn)

                    ' defining report source 
                    CrystalReportViewer1.DisplayGroupTree = False
                    CrystalReportViewer1.ReportSource = oRpt
                    Session("ARPreview") = oRpt

                    ' so uptil now we have created everything 
                    ' what remains is to pass parameters to our report, so it 
                    ' shows only selected records. so calling a method to set 
                    ' those parameters. 

                    'Check if there are parameters or not in report.
                    Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count
                    setReportParameters()
                    Session("ARPreview") = oRpt
                    Session("ARPreviewAREID") = ViewState("AREID")

                Else
                    oRpt = CType(Session("ARPreview"), ReportDocument)
                    CrystalReportViewer1.ReportSource = oRpt
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub EnableControls()

        Try
            btnStatusReset.Visible = False
            btnRSSReset.Visible = False

            btnStatusSubmit.Visible = False
            btnRSSSubmit.Visible = False

            ddApprovalStatus.Enabled = False
            txtApprovalComment.Enabled = False

            lblPriceChangeDate.Visible = False

            lblApprovalRole.Visible = False
            ddApprrovalRole.Visible = False

            'UGN Assist
            'If ViewState("SubscriptionID") = 0 And ViewState("isAdmin") Then
            If ViewState("isAssist") = True Then
                lblApprovalRole.Visible = ViewState("isAdmin")
                ddApprrovalRole.Enabled = ViewState("isAdmin")
                ddApprrovalRole.Visible = ViewState("isAdmin")
            Else

                'VP of Sales
                If ViewState("SubscriptionID") = 23 And ViewState("isVPSalesActingAsSales") = True And ViewState("EventStatusID") <> 8 Then
                    'show Role Dropdown box so VP of Sales can approve for Sales or VP of Sales
                    lblApprovalRole.Visible = ViewState("isAdmin")
                    ddApprrovalRole.Enabled = ViewState("isAdmin")
                    ddApprrovalRole.Visible = ViewState("isAdmin")
                    ddApprrovalRole.Items.Clear()

                    Dim liSalesListItem As New System.Web.UI.WebControls.ListItem
                    liSalesListItem.Text = "Sales"
                    liSalesListItem.Value = 9
                    ddApprrovalRole.Items.Add(liSalesListItem)

                    Dim liVPofSalesListItem As New System.Web.UI.WebControls.ListItem
                    liVPofSalesListItem.Text = "VP of Sales"
                    liVPofSalesListItem.Value = 23
                    ddApprrovalRole.Items.Add(liVPofSalesListItem)

                    'vp of sales approving as sales
                    If ViewState("isVPSalesActingAsSales") = True Then
                        ddApprrovalRole.SelectedValue = 9
                    Else
                        ddApprrovalRole.SelectedValue = 23
                    End If
                End If
            End If

            'billing
            If ViewState("SubscriptionID") = 21 Then
                lblPriceChangeDate.Visible = True
            End If

            'allow billing to approve when open or inprocess temporarily
            If (ViewState("ApprovalStatusID") <= 2 And ViewState("SubscriptionID") = 21 And ViewState("isAssist") = False) _
                Or (ViewState("ApprovalStatusID") = 2) _
                 Then
                'And ViewState("SubscriptionID") <> 21

                AdjustApprovalStatusControl()

                btnStatusReset.Visible = ViewState("isAdmin")
                btnRSSReset.Visible = ViewState("isAdmin")

                btnStatusSubmit.Visible = ViewState("isAdmin")
                btnRSSSubmit.Visible = ViewState("isAdmin")

                ddApprovalStatus.Enabled = ViewState("isAdmin")
                txtApprovalComment.Enabled = ViewState("isAdmin")

                'if backup team member approving then
                If ViewState("TeamMemberID") <> ViewState("ApprovalTeamMemberID") Then
                    lblTeamMbr.Text = "Backup Teammember TO: " & lblTeamMbr.Text
                End If
            End If
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Private Sub InitializeViewState()

        Try
            ViewState("AREID") = 0

            ViewState("isAdmin") = False
            ViewState("isAssist") = False

            ViewState("AcctMgrTMID") = 0
            ViewState("AcctMgrEmail") = ""
            ViewState("ApprovalStatusID") = 1
            ViewState("ApprovalTeamMemberID") = 0

            ViewState("CalculatedDeductionAmount") = 0
            ViewState("CEOEmail") = ""
            ViewState("CFOEmail") = ""
            ViewState("DefaultBillingEmail") = ""
            ViewState("EventDesc") = ""
            ViewState("EventStatusID") = 0
            ViewState("EventTypeID") = 0

            ViewState("isDefaultBilling") = False
            ViewState("RowID") = 0
            ViewState("RoutingLevel") = 0

            ViewState("SubscriptionID") = 0
            ViewState("TeamMemberID") = 0
            ViewState("VPSalesEmail") = ""
            ViewState("isVPSalesActingAsSales") = False

            Session("ARPreviewAREID") = Nothing
            Session("ARPreview") = Nothing

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub BindData()

        Try
            Dim ds As DataSet
            Dim dt As DataTable

            Dim iSalesApprovalStatusID As Integer = 0

            Dim objApproval As ARApprovalBLL = New ARApprovalBLL

            'get AR Event info
            ds = ARGroupModule.GetAREvent(ViewState("AREID"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ViewState("EventDesc") = ds.Tables(0).Rows(0).Item("EventDesc").ToString

                If ds.Tables(0).Rows(0).Item("AcctMgrTMID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("AcctMgrTMID") > 0 Then
                        ViewState("AcctMgrTMID") = ds.Tables(0).Rows(0).Item("AcctMgrTMID")
                    End If
                End If

                'could be positive or negative
                ViewState("CalculatedDeductionAmount") = 0
                If ds.Tables(0).Rows(0).Item("CalculatedDeductionAmount") IsNot System.DBNull.Value Then
                    ViewState("CalculatedDeductionAmount") = ds.Tables(0).Rows(0).Item("CalculatedDeductionAmount")
                End If

                ViewState("EventDesc") = ds.Tables(0).Rows(0).Item("EventDesc").ToString

                If ds.Tables(0).Rows(0).Item("EventStatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("EventStatusID") > 0 Then
                        ViewState("EventStatusID") = ds.Tables(0).Rows(0).Item("EventStatusID")
                        ddEventStatus.SelectedValue = ds.Tables(0).Rows(0).Item("EventStatusID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("EventTypeID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("EventTypeID") > 0 Then
                        ViewState("EventTypeID") = ds.Tables(0).Rows(0).Item("EventTypeID")
                    End If
                End If

                lblPriceChangeDate.Text = "NOTE: The price was NOT updated in Future 3."
                If ds.Tables(0).Rows(0).Item("isPriceUpdatedByAccounting") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("isPriceUpdatedByAccounting") = True Then
                        lblPriceChangeDate.Text = "The price was updated in Future 3 on " & ds.Tables(0).Rows(0).Item("PriceChangeDate").ToString
                    End If
                End If

            End If 'get AR Event info

            'get approval info based on Team Member
            If ViewState("SubscriptionID") > 0 Then
                'VP of Sales and the event is not rejected
                If ViewState("SubscriptionID") = 23 And ViewState("EventStatusID") <> 8 Then
                    'first check sales, sales must approve before VP of Sales
                    dt = objApproval.GetAREventApprovalStatus(ViewState("AREID"), 9)

                    If dt.Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                        If dt.Rows(0).Item("StatusID") > 0 Then
                            iSalesApprovalStatusID = dt.Rows(0).Item("StatusID")

                            'If sales is pending then show sales info
                            If iSalesApprovalStatusID <= 2 Then
                                ViewState("isVPSalesActingAsSales") = True
                            Else 'get VP of Sales info
                                dt = objApproval.GetAREventApprovalStatus(ViewState("AREID"), 23)
                            End If
                        End If
                    End If
                Else 'get current approver info
                    dt = objApproval.GetAREventApprovalStatus(ViewState("AREID"), ViewState("SubscriptionID"))
                End If

                If commonFunctions.CheckDataTable(dt) = True Then

                    lblNotificationDate.Text = dt.Rows(0).Item("NotificationDate").ToString
                    lblTeamMbr.Text = dt.Rows(0).Item("ddTeamMemberName").ToString
                    txtApprovalComment.Text = dt.Rows(0).Item("Comment").ToString

                    If dt.Rows(0).Item("RoutingLevel") IsNot System.DBNull.Value Then
                        If dt.Rows(0).Item("RoutingLevel") > 0 Then
                            ViewState("RoutingLevel") = dt.Rows(0).Item("RoutingLevel")
                        End If
                    End If

                    If dt.Rows(0).Item("RowID") IsNot System.DBNull.Value Then
                        If dt.Rows(0).Item("RowID") > 0 Then
                            ViewState("RowID") = dt.Rows(0).Item("RowID")
                        End If
                    End If

                    If dt.Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                        If dt.Rows(0).Item("StatusID") > 0 Then
                            ddApprovalStatus.SelectedValue = dt.Rows(0).Item("StatusID")
                            ViewState("ApprovalStatusID") = dt.Rows(0).Item("StatusID")
                        End If
                    End If

                    If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                        If dt.Rows(0).Item("TeamMemberID") > 0 Then
                            ViewState("ApprovalTeamMemberID") = dt.Rows(0).Item("TeamMemberID")
                        End If
                    End If
                End If
            Else
                ViewState("ApprovalStatusID") = 0
            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Or Response.Cookies("UGNDB_UserFullName") Is Nothing Then
                    ' commonFunctions.SetUGNDBUser()
                    Dim FullName As String = commonFunctions.getUserName()
                    Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                    Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                    If FullName = Nothing Then
                        FullName = "Demo.Demo"  '* This account has restricted read only rights.
                    End If
                    Dim LocationOfDot As Integer = InStr(FullName, ".")
                    If LocationOfDot > 0 Then
                        Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                        Dim FirstInitial As String = Left(FullName, 1)
                        Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                        Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                        Response.Cookies("UGNDB_UserFullName").Value = Mid(FirstName, 1, 1).ToUpper & Mid(FirstName, 2, Len(FirstName) - 1) & " " & Mid(LastName, 1, 1).ToUpper & Mid(LastName, 2, Len(LastName) - 1)
                    Else
                        Response.Cookies("UGNDB_User").Value = FullName
                        Response.Cookies("UGNDB_UserFullName").Value = FullName

                    End If
                End If

                CheckRights()

                BindCriteria()

                BindData()

                lblAREID.Text = ViewState("AREID")

                EnableControls()

                txtApprovalComment.Attributes.Add("onkeypress", "return tbLimit();")
                txtApprovalComment.Attributes.Add("onkeyup", "return tbCount(" + lblApprovalCommentCharCount.ClientID + ");")
                txtApprovalComment.Attributes.Add("maxLength", "400")

            End If

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID against the deduction table, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > <a href='AR_Event_List.aspx'>AR Event Search </a> > <a href='AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & " '>AR Event Detail </a> >AR Event Approval "

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Private Sub setReportParameters()

        Try
            ' all the parameter fields will be added to this collection 
            Dim paramFields As New ParameterFields

            ' the parameter fields to be sent to the report 
            Dim pfAREID As ParameterField = New ParameterField

            ' setting the name of parameter fields with which they will be received in report 
            pfAREID.ParameterFieldName = "@AREID"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcAREID As New ParameterDiscreteValue

            ' setting the values of discrete objects 
            dcAREID.Value = ViewState("AREID")

            ' now adding these discrete values to parameters 
            pfAREID.CurrentValues.Add(dcAREID)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfAREID)

            ' finally add the parameter collection to the crystal report viewer 
            CrystalReportViewer1.ParameterFieldInfo = paramFields

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        Try
            'in order to clear crystal reports
            If HttpContext.Current.Session("ARPreview") IsNot Nothing Then
                Dim tempRpt As ReportDocument = New ReportDocument()
                tempRpt = CType(HttpContext.Current.Session("ARPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("ARPreviewAREID") = Nothing
                HttpContext.Current.Session("ARPreview") = Nothing
                GC.Collect()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnStatusSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStatusSubmit.Click

        Try
            lblMessage.Text = ""

            GetTeamMemberInfo()

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strEmailApproveURL As String = strProdOrTestEnvironment & "AR/crAR_Event_Approval.aspx?AREID="
            Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "AR/crPreview_AR_Event_Detail.aspx?AREID="
            Dim strEmailEventURL As String = strProdOrTestEnvironment & "AR/AR_Event_Detail.aspx?AREID="

            Dim objARApproval As ARApprovalBLL = New ARApprovalBLL

            Dim iApprovalStatusID As Integer = 0

            Dim strEmailBody As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            If ddApprovalStatus.SelectedIndex > 0 Then
                iApprovalStatusID = ddApprovalStatus.SelectedValue
            End If

            If ViewState("TeamMemberID") <> ViewState("ApprovalTeamMemberID") Then
                txtApprovalComment.Text = "<br /><br />Backup Team Member reviewed. " & Chr(13) & Chr(10) & txtApprovalComment.Text
            End If

            'only allow rejecting with a comment or just approving
            If (iApprovalStatusID = 3 And txtApprovalComment.Text.Trim <> "") Or iApprovalStatusID = 4 Then

                'update approval status of AR Event
                If ViewState("SubscriptionID") = 23 And ViewState("isVPSalesActingAsSales") = True Then
                    objARApproval.UpdateAREventApprovalStatus(ViewState("AREID"), ViewState("RoutingLevel"), ViewState("TeamMemberID"), 9, txtApprovalComment.Text.Trim, iApprovalStatusID, ViewState("RowID"), ViewState("RowID"))
                Else
                    If ViewState("isAssist") = True Then
                        'keep real approver ID
                        objARApproval.UpdateAREventApprovalStatus(ViewState("AREID"), ViewState("RoutingLevel"), ViewState("ApprovalTeamMemberID"), ViewState("SubscriptionID"), txtApprovalComment.Text.Trim, iApprovalStatusID, ViewState("RowID"), ViewState("RowID"))
                    Else 'real approver is approving
                        objARApproval.UpdateAREventApprovalStatus(ViewState("AREID"), ViewState("RoutingLevel"), ViewState("TeamMemberID"), ViewState("SubscriptionID"), txtApprovalComment.Text.Trim, iApprovalStatusID, ViewState("RowID"), ViewState("RowID"))
                    End If
                End If
                ViewState("ApprovalStatusID") = iApprovalStatusID

                'Accounting Manager Approval AND Status = In-Process (Pending Accountant Event Approval)
                If ViewState("EventStatusID") = 2 And ViewState("SubscriptionID") = 21 Then

                    'if price change no accrual then close the event
                    If ViewState("EventTypeID") = 1 And iApprovalStatusID = 4 Then
                        'ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 6) 'In-Process (Pending Accountant Close)
                        'ViewState("EventStatusID") = 6
                        'ddEventStatus.SelectedValue = 6
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 9) 'Closed
                        ViewState("EventStatusID") = 9
                        ddEventStatus.SelectedValue = 9

                        '2011-July 15 - no need to send email for Accounting Mgr Approvals of NON-accruing events
                        ''assign email subject
                        'strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been approved " 'is pending Accounting Manager to close the event"

                        ''build email body
                        'strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved.</font><br /><br />" 'and is pending Accounting Manager to close the event:</font><br /><br />"
                        'strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        'strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        ''strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>The Accounting Manager can click here to close the event</a></font><br /><br />"
                        'strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Team members can click here to review the event</a></font><br /><br />"
                        'strEmailBody &= "<font size='2' face='Verdana'>Description : " & ViewState("EventDesc") & "</font><br />"

                        'If txtApprovalComment.Text.Trim <> "" Then
                        '    strEmailBody &= "<font size='2' face='Verdana'>Comment : " & txtApprovalComment.Text.Trim & "</font><br />"
                        'End If

                        'update history
                        'ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager approved and is pending to close. " & txtApprovalComment.Text.Trim)
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager approved and closed the event. " & txtApprovalComment.Text.Trim)
                    End If

                    'if part or customer accrual then set event status id 3 - In-Process (Pending Sales for Customer Approval) if approved
                    If (ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3) And iApprovalStatusID = 4 Then
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 3) 'In-Process (Pending Sales for Customer Approval)
                        ViewState("EventStatusID") = 3
                        ddEventStatus.SelectedValue = 3

                        'assign email subject
                        strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been approved and is now pending Sales to obtain customer approval"

                        'build email body
                        strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved is now pending Sales to obtain customer approval:</font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><b>Description</b> : " & ViewState("EventDesc") & "</font><br /><br /><br />"

                        If txtApprovalComment.Text.Trim <> "" Then
                            strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b> : " & txtApprovalComment.Text.Trim & "</font><br /><br /><br />"
                        End If

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager approved and the event is now pending Sales to obtain customer approval. " & txtApprovalComment.Text.Trim)

                    End If

                    'if invoices on hold approved then set to In-Process (Pending Accounting Mgr Close)
                    If ViewState("EventTypeID") = 5 And iApprovalStatusID = 4 Then
                        'ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 6) 'In-Process (Pending Accounting Mgr Close)
                        'ViewState("EventStatusID") = 6
                        'ddEventStatus.SelectedValue = 6
                        '2011-Aug-18 Gina Lacny - if approved, then close the event
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 9) 'Closed
                        ViewState("EventStatusID") = 9
                        ddEventStatus.SelectedValue = 9

                        '2011-July 15 - no need to send email for Accounting Mgr Approvals of NON-accruing events
                        'assign email subject
                        'strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been approved and is now pending the accounting team to update the price, invoice, and AR Event"

                        ''build email body
                        'strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved is now pending the accounting team to update the price, invoice, and AR Event:</font><br /><br />"
                        'strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        'strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        'strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                        'strEmailBody &= "<font size='2' face='Verdana'>Description : " & ViewState("EventDesc") & "</font><br />"

                        'If txtApprovalComment.Text.Trim <> "" Then
                        '    strEmailBody &= "<font size='2' face='Verdana'>Comment : " & txtApprovalComment.Text.Trim & "</font><br />"
                        'End If

                        'update history
                        'ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager approved and the event is now pending the accounting team to update the price, invoice, and AR Event. " & txtApprovalComment.Text.Trim)
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager approved and closed the event. " & txtApprovalComment.Text.Trim)

                    End If

                    'if rejected (3), regardless of EventType Type, then overall status =  7 - Rejected (Pending Sales Fix)
                    If iApprovalStatusID = 3 Then
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 7) 'Rejected (Pending Sales Fix)
                        ViewState("EventStatusID") = 7
                        ddEventStatus.SelectedValue = 7

                        'assign email subject
                        strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been rejected by Accounting Manager. Sales must fix and resubmit the event"

                        'build email body
                        strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been rejected by Accounting Manager. Sales must fix and resubmit the event:</font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                        If txtApprovalComment.Text.Trim <> "" Then
                            strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                        End If

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Accounting Manager rejected. Sales must fix the event. " & txtApprovalComment.Text.Trim)
                    End If

                    'notify sales
                    strEmailToAddress = ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

                End If

                'if In-Process (Pending Deduction Form Approval) and Event Type is Customer or Part Accrual
                If ViewState("EventStatusID") = 5 And (ViewState("EventTypeID") = 2 Or ViewState("EventTypeID") = 3) Then

                    'sales approval
                    If ViewState("SubscriptionID") = 9 Then

                        'if approved
                        If iApprovalStatusID = 4 Then

                            'check if needed to notify VP of Sales to approve
                            'VP of Sales has been deactived 10-01-2014 LR
                            ''If (ViewState("CalculatedDeductionAmount") <= -2500 Or ViewState("CalculatedDeductionAmount") >= 2500) Then

                            ''    'update approval status to inprocess and notification sent date for VP of Sales
                            ''    ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 23)

                            ''    'send email to VP of Sales to approve
                            ''    'notify VP of sales but no backup
                            ''    strEmailToAddress = ViewState("VPSalesEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)

                            ''    'assign email subject
                            ''    strEmailSubject = "APPROVAL REQUEST: AR Event ID:" & ViewState("AREID") & " has been approved by Sales and is pending VP of Sales review"

                            ''    'build email body
                            ''    strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by Sales. VP of Sales must review:</font><br /><br />"
                            ''    strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                            ''    strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                            ''    strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailApproveURL & ViewState("AREID") & "'>Click here to review the event</a></font><br /><br />"
                            ''    strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                            ''    If txtApprovalComment.Text.Trim <> "" Then
                            ''        strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                            ''    End If

                            ''    'update history
                            ''    ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Sales approved. VP of Sales is pending approval. " & txtApprovalComment.Text.Trim)
                            ''Else

                            'if VP of Sales is NOT needed then Billing can close
                            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 6)  'In-Process (Pending Accountant Close)
                            ViewState("EventStatusID") = 6
                            ddEventStatus.SelectedValue = 6

                            'send email to billing to close
                            'notify default billing
                            strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                            'include interested billing team members
                            If ViewState("BillingEmail") <> "" Then
                                If strEmailCCAddress <> "" Then
                                    strEmailCCAddress &= ";"
                                End If

                                strEmailCCAddress &= ViewState("BillingEmail")
                            End If

                            'assign email subject
                            strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been approved by sales and is pending Accounting Manager to close the event"

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by Sales and is pending Accounting Manager to close the event:</font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                            If txtApprovalComment.Text.Trim <> "" Then
                                strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                            End If

                            'update history
                            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Sales approved. Accounting Manager is pending to close the event. " & txtApprovalComment.Text.Trim)
                            'End If

                        End If

                        'if rejected
                        If iApprovalStatusID = 3 Then
                            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 8) 'Rejected (Pending Accountant Fix)
                            ViewState("EventStatusID") = 8
                            ddEventStatus.SelectedValue = 8

                            'send email to billing that form is rejected and they need to fix and resubmit
                            'notify default billing
                            strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                            'assign email subject
                            strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been rejected by Sales and is pending Accounting Manager to fix and resubmit the event"

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been rejected by Sales and is pending Accounting Manager to fix and resubmit the event:</font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                            If txtApprovalComment.Text.Trim <> "" Then
                                strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                            End If

                            'update history
                            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Sales rejected. Accounting Manager must fix and resubmit the event. " & txtApprovalComment.Text.Trim)

                        End If
                    End If

                    'VP of sales approval
                    If ViewState("SubscriptionID") = 23 Then 'And (ViewState("CalculatedDeductionAmount") <= -2500 Or ViewState("CalculatedDeductionAmount") >= 2500) Then
                        'if approved
                        If iApprovalStatusID = 4 Then
                            If ViewState("isVPSalesActingAsSales") = True Then
                                'update history
                                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "VP of Sales approved as Sales team member " & txtApprovalComment.Text.Trim)
                                ViewState("isVPSalesActingAsSales") = False

                                'update approval status to inprocess  
                                ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 23)
                            Else 'vp of sales approving as vp of sales

                                'update approval status to inprocess and notification sent date for VP of Finance
                                ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 33)

                                'notify VP of Finance but no backup
                                strEmailToAddress = ViewState("CFOEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(33, "", False)

                                'include VP of Sales as CC EMail if Approved By Assist team members
                                If ViewState("isAssist") = True Then
                                    If ViewState("VPSalesEmail") <> "" Then
                                        If strEmailCCAddress <> "" Then
                                            strEmailCCAddress &= ";"
                                        End If

                                        strEmailCCAddress &= ViewState("VPSalesEmail")
                                    End If
                                End If

                                'assign email subject
                                strEmailSubject = "APPROVAL REQUEST: AR Event ID:" & ViewState("AREID") & " has been approved by the VP of Sales and is pending the VP of Finance to review"

                                'build email body
                                strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by the VP of Sales and is pending the VP of Finance to review:</font><br /><br />"
                                strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailApproveURL & ViewState("AREID") & "'>Click here to review the event</a></font><br /><br />"
                                strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                                If txtApprovalComment.Text.Trim <> "" Then
                                    strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                                End If

                                'update history
                                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "VP of Sales approved. VP of Finance is pending approval. " & txtApprovalComment.Text.Trim)

                            End If

                        End If

                        'if rejected
                        If iApprovalStatusID = 3 Then
                            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 8) 'Rejected (Pending Accountant Fix)
                            ViewState("EventStatusID") = 8
                            ddEventStatus.SelectedValue = 8

                            'reset sales approval
                            ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("TeamMemberID"), ViewState("EventTypeID"))

                            'send email to billing that form is rejected and they need to fix and resubmit
                            'notify default billing
                            strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                            'assign email subject
                            strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been rejected by the VP of Sales and is pending Accounting Manager to fix and resubmit the event"

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been rejected by the VP of Sales and is pending the Accounting Manager to fix and resubmit the event:</font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                            If txtApprovalComment.Text.Trim <> "" Then
                                strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                            End If

                            'update history
                            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "VP of Sales rejected. Accounting Manager must fix the form and resubmit. " & txtApprovalComment.Text.Trim)
                        End If
                    End If

                    'VP of finance approval
                    If ViewState("SubscriptionID") = 33 Then ' And (ViewState("CalculatedDeductionAmount") <= -2500 Or ViewState("CalculatedDeductionAmount") >= 2500) Then
                        'if approved
                        If iApprovalStatusID = 4 Then
                            'event status remains the same

                            'check if CEO approval is needed
                            If ViewState("CalculatedDeductionAmount") <= -5000 Or ViewState("CalculatedDeductionAmount") >= 5000 Then
                                'update approval status to inprocess and notification sent date for CEO
                                ARGroupModule.UpdateAREventApprovalNotify(ViewState("AREID"), 24)

                                'notify CEO  
                                strEmailToAddress = ViewState("CEOEmail")

                                'include CFO CC EMail if Approved By Assist team members
                                If ViewState("isAssist") = True Then
                                    If ViewState("CFOEmail") <> "" Then
                                        If strEmailCCAddress <> "" Then
                                            strEmailCCAddress &= ";"
                                        End If

                                        strEmailCCAddress &= ViewState("CFOEmail")
                                    End If
                                End If

                                'assign email subject
                                strEmailSubject = "APPROVAL REQUEST: AR Event ID:" & ViewState("AREID") & " has been approved by the VP of Finance and is pending the CEO to review"

                                'build email body
                                strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by the VP of Finance and is pending the CEO to review:</font><br /><br />"
                                strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailApproveURL & ViewState("AREID") & "'>Click here to review the event</a></font><br /><br />"
                                strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                                If txtApprovalComment.Text.Trim <> "" Then
                                    strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                                End If

                                'update history
                                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "VP of Finance approved. CEO is pending approval. " & txtApprovalComment.Text.Trim)

                            Else
                                'if CEO is NOT needed then Billing can close
                                ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 6)  'In-Process (Pending Accountant Close)
                                ViewState("EventStatusID") = 6
                                ddEventStatus.SelectedValue = 6

                                'send email to billing to close
                                'notify default billing
                                strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                                'include interested billing team members
                                If ViewState("BillingEmail") <> "" Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("BillingEmail")
                                End If

                                'assign email subject
                                strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been approved by the VP of Finance and is pending Accounting Manager to close"

                                'build email body
                                strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by the VP of Finance and is pending Accounting Manager to close the event:</font><br /><br />"
                                strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                                strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                                strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                                If txtApprovalComment.Text.Trim <> "" Then
                                    strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                                End If

                                'update history
                                ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "VP of Finance approved. Accounting Manager is pending to close the event. " & txtApprovalComment.Text.Trim)

                            End If

                        End If

                        'if rejected
                        If iApprovalStatusID = 3 Then
                            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 8) 'Rejected (Pending Accountant Fix)
                            ViewState("EventStatusID") = 8
                            ddEventStatus.SelectedValue = 8

                            'reset sales and VP of Sales approval
                            ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("TeamMemberID"), ViewState("EventTypeID"))

                            'send email to billing that form is rejected and they need to fix and resubmit
                            'notify default billing
                            strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                            'assign email subject
                            strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been rejected by the VP of Finance and is pending Accounting Manager to fix and resubmit"

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been rejected by the VP of Finance and is pending Accounting Manager to fix and resubmit the event:</font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                            If txtApprovalComment.Text.Trim <> "" Then
                                strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                            End If

                            'update history
                            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "VP of Finance rejected. Accounting Manager must fix the form and resubmit. " & txtApprovalComment.Text.Trim)

                        End If
                    End If

                    'CEO approval
                    If ViewState("SubscriptionID") = 24 Then 'And (ViewState("CalculatedDeductionAmount") <= -5000 Or ViewState("CalculatedDeductionAmount") >= 5000) Then

                        'if approved
                        If iApprovalStatusID = 4 Then
                            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 6)  'In-Process (Pending Accountant Close)
                            ViewState("EventStatusID") = 6
                            ddEventStatus.SelectedValue = 6

                            'send email to billing to close
                            'assign email subject
                            strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been approved by the CEO and is pending Accounting Manager to close"

                            'include interested billing team members
                            If ViewState("BillingEmail") <> "" Then
                                If strEmailCCAddress <> "" Then
                                    strEmailCCAddress &= ";"
                                End If

                                strEmailCCAddress &= ViewState("BillingEmail")
                            End If

                            'include CEO CC EMail if Approved By Assist team members
                            If ViewState("isAssist") = True Then
                                If ViewState("CEOEmail") <> "" Then
                                    If strEmailCCAddress <> "" Then
                                        strEmailCCAddress &= ";"
                                    End If

                                    strEmailCCAddress &= ViewState("CEOEmail")
                                End If
                            End If

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by the CEO and is pending Accounting Manager to close the event:</font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                            If txtApprovalComment.Text.Trim <> "" Then
                                strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                            End If

                            'update history
                            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "CEO approved. Accounting Manager is pending to close the event. " & txtApprovalComment.Text.Trim)

                        End If

                        'if rejected
                        If iApprovalStatusID = 3 Then
                            ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 8) 'Rejected (Pending Accountant Fix)
                            ViewState("EventStatusID") = 8
                            ddEventStatus.SelectedValue = 8

                            'reset sales, vp of sales, and vp of finance approval
                            ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), ViewState("TeamMemberID"), ViewState("EventTypeID"))

                            'assign email subject
                            strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been rejected by the CEO and is pending Accounting Manager to fix and resubmit"

                            'build email body
                            strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been rejected by the CEO and is pending Accounting Manager to fix and resubmit the event:</font><br /><br />"
                            strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                            strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                            strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                            If txtApprovalComment.Text.Trim <> "" Then
                                strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                            End If

                            'update history
                            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "CEO rejected. Accounting Manager must fix and resubmit the event. " & txtApprovalComment.Text.Trim)

                        End If

                        'notify default billing
                        strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                    End If

                End If

                'if In-Process (Pending Deduction Form Approval) and Event Type IS Accounting Accrual
                If ViewState("SubscriptionID") = 33 And ViewState("EventStatusID") = 5 And ViewState("EventTypeID") = 4 Then

                    'if approved
                    If iApprovalStatusID = 4 Then
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 6)  'In-Process (Pending Accountant Close)
                        ViewState("EventStatusID") = 6
                        ddEventStatus.SelectedValue = 6

                        'send email to billing to close

                        'assign email subject
                        strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been approved by the VP of Finance and is pending Accounting Manager to close the event"

                        'include interested billing team members
                        If ViewState("BillingEmail") <> "" Then
                            If strEmailCCAddress <> "" Then
                                strEmailCCAddress &= ";"
                            End If

                            strEmailCCAddress &= ViewState("BillingEmail")
                        End If

                        'include CFO CC EMail if Approved By Assist team members
                        If ViewState("isAssist") = True Then
                            If ViewState("CFOEmail") <> "" Then
                                If strEmailCCAddress <> "" Then
                                    strEmailCCAddress &= ";"
                                End If

                                strEmailCCAddress &= ViewState("CFOEmail")
                            End If
                        End If

                        'build email body
                        strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by the VP of Finance and is pending Accounting Manager to close the event:</font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                        If txtApprovalComment.Text.Trim <> "" Then
                            strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                        End If

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "VP of Finance approved. Accounting Manager is pending to close the event. " & txtApprovalComment.Text.Trim)

                    End If

                    'if rejected
                    If iApprovalStatusID = 3 Then
                        ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 8) 'Rejected (Pending Accountant Fix)
                        ViewState("EventStatusID") = 8
                        ddEventStatus.SelectedValue = 8

                        'send email to billing that form is rejected and they need to fix and resubmit

                        'assign email subject
                        strEmailSubject = "AR Event ID:" & ViewState("AREID") & " has been rejected by the VP of Finance and is pending Accounting Manager to fix and resubmit the event"

                        'build email body
                        strEmailBody = "<font size='2' face='Verdana'>The following AR Event has been approved by the VP of Finance and is pending Accounting Manager to fix and resubmit the event:</font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'>AR Event ID: <b>" & ViewState("AREID") & "</b></font><br /><br />"
                        strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><a href='" & strEmailEventURL & ViewState("AREID") & "'>Click here to update the event</a></font><br /><br />"
                        strEmailBody &= "<font size='2' face='Verdana'><b>Description</b>: " & ViewState("EventDesc") & "</font><br /><br /><br />"

                        If txtApprovalComment.Text.Trim <> "" Then
                            strEmailBody &= "<br /><br /><font size='2' face='Verdana'><b>Comment</b>: " & txtApprovalComment.Text.Trim & "</font><br /><br />"
                        End If

                        'update history
                        ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "VP of Finance rejected. Accounting Manager must fix the form and resubmit. " & txtApprovalComment.Text.Trim)

                    End If

                    'notify default billing
                    strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)
                End If

                If strEmailToAddress <> "" And strEmailSubject <> "" And strEmailBody <> "" Then
                    If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
                        'lblMessage.Text &= "<br />" & "Notfication Sent."
                        'Else
                        '    lblMessage.Text &= "<br />" & "Notfication Failed. Please contact IS."
                    End If
                End If

                'VP of Sales
                If ViewState("SubscriptionID") = 23 Then
                    BindData()
                End If

                EnableControls()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub GetTeamMemberInfo()

        Try
            Dim dsTeamMember As DataSet
            Dim iRowCounter As Integer = 0

            ViewState("BillingEmail") = ""
            dsTeamMember = commonFunctions.GetTeamMemberBySubscription(35)
            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                For iRowCounter = 0 To dsTeamMember.Tables(0).Rows.Count - 1
                    If dsTeamMember.Tables(0).Rows(iRowCounter).Item("WorkStatus") IsNot System.DBNull.Value Then
                        If dsTeamMember.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                            If InStr(dsTeamMember.Tables(0).Rows(iRowCounter).Item("TMName"), "**") <= 0 Then
                                If ViewState("BillingEmail") <> "" Then
                                    ViewState("BillingEmail") &= ";"
                                End If

                                ViewState("BillingEmail") &= dsTeamMember.Tables(0).Rows(iRowCounter).Item("Email").ToString
                            End If
                        End If
                    End If
                Next
            End If

            ViewState("AcctMgrEmail") = ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))
            ViewState("DefaultBillingEmail") = ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)
            '**VP of Sales is not needed until someone fullfills this position 20140101 LR
            ''ViewState("VPSalesEmail") = ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)
            ViewState("CFOEmail") = ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(33, "", False)
            ViewState("CEOEmail") = ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(24, "", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnRSSReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSSReset.Click

        Try

            lblMessage.Text = ""

            txtRSSComment.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        'lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub
    Protected Function SendEmail(ByVal EmailToAddress As String, ByVal EmailCCAddress As String, ByVal EmailSubject As String, ByVal EmailBody As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try

            Dim strSubject As String = ""
            Dim strBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            Dim strEmailToAddress As String = EmailToAddress
            Dim strEmailCCAddress As String = EmailCCAddress

            If strEmailCCAddress.Trim <> "" Then
                strEmailCCAddress &= ";"
            End If

            strEmailCCAddress &= strEmailFromAddress

            'handle test environment
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
            End If

            strSubject &= EmailSubject
            strBody &= EmailBody

            Dim dt As DataTable
            Dim objARSupportingDocBLL As AREventSupportingDocBLL = New AREventSupportingDocBLL
            Dim strSupportingDocURL As String = strProdOrTestEnvironment & "AR/AR_Supporting_Doc_Viewer.aspx?RowID="

            dt = objARSupportingDocBLL.GetAREventSupportingDoc(ViewState("AREID"))
            If commonFunctions.CheckDataTable(dt) = True Then

                strBody &= "<br /><br /><font size='1' face='Verdana'>Supporting Documents</font>"
                strBody &= "<table style='border: 1px solid #D0D0BF; width: 100%'>"

                For iRowCounter = 0 To dt.Rows.Count - 1
                    strBody &= "<tr>"
                    strBody &= "<td align='left'><font size='1' face='Verdana'><a href=" & strSupportingDocURL & dt.Rows(iRowCounter).Item("RowID") & "&AREID=" & ViewState("AREID") & ">" & dt.Rows(iRowCounter).Item("SupportingDocName") & "</a></font></td>"
                    strBody &= "</tr>"
                Next

                strBody &= "</table>"
            End If

            'create the mail message using new System.Net.Mail (not CDonts)
            Dim mail As New MailMessage()

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br /><br />Email To Address List: " & EmailToAddress & "<br />"
                strBody &= "<br /><br />Email CC Address List: " & strEmailCCAddress & "<br />"

                strEmailToAddress = "Roderick.Carlson@ugnauto.com"
                strEmailCCAddress = ""
            End If

            strBody &= "<br /><br /><font size='1' face='Verdana'>"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"
            strBody &= "<br />If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the AR Module."
            strBody &= "<br />Please <u>do not</u> reply back to this email because you will not receive a response."
            strBody &= "<br />Please use a separate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br />"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++</font>"

            'set the content
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = strEmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    mail.To.Add(emailList(i))
                End If
            Next i

            'build email CC List
            If strEmailCCAddress IsNot Nothing Then
                emailList = strEmailCCAddress.Split(";")

                For i = 0 To UBound(emailList)
                    If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                        mail.CC.Add(emailList(i))
                    End If
                Next i
            End If

            'If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
            '    mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            'End If

            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "<br />" & "Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "<br />" & "Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("AR Approval", strEmailFromAddress, EmailToAddress, "", strSubject, strBody, "")
            End Try

            bReturnValue = True

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBottom.Text = lblMessage.Text

        SendEmail = bReturnValue

    End Function

    Protected Sub btnRSSSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSSSubmit.Click

        Try
            lblMessage.Text = ""

            GetTeamMemberInfo()

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "AR/crPreview_AR_Event_Detail.aspx?AREID=" & ViewState("AREID")
            Dim strEmailDetailURL As String = strProdOrTestEnvironment & "AR/AR_Event_Detail.aspx?AREID=" & ViewState("AREID")

            Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim strEmailBody As String = ""
            Dim strEmailSubject As String = ""
            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""

            'update RSS Question List
            ARGroupModule.InsertARRSS(ViewState("AREID"), ViewState("TeamMemberID"), ViewState("SubscriptionID"), txtRSSComment.Text.Trim)

            'update AR Event History
            ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Message Sent:" & txtRSSComment.Text.Trim)

            'current user is billing, then notify sales
            If ViewState("SubscriptionID") = 21 Then
                strEmailToAddress = ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))                
            End If

            'current user is sales, then notify default billing
            If ViewState("SubscriptionID") = 9 Then
                strEmailToAddress = ViewState("DefaultBillingEmail") ' ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)
            End If

            'include interested billing team members
            If ViewState("BillingEmail") <> "" Then
                If strEmailCCAddress <> "" Then
                    strEmailCCAddress &= ";"
                End If

                strEmailCCAddress = ViewState("BillingEmail")
            End If

            'current user is vp of sales
            If ViewState("SubscriptionID") = 23 Then
                'notify default billing
                strEmailToAddress = ViewState("DefaultBillingEmail") ' ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                If strEmailToAddress <> "" Then
                    strEmailToAddress &= ";"
                End If

                'notify sales
                strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

            End If

            'current user is vp of finance
            If ViewState("SubscriptionID") = 33 Then

                ''notify default billing
                strEmailToAddress = ViewState("DefaultBillingEmail")  'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                'if not accounting accrual
                If ViewState("EventTypeID") <> 4 Then
                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    'notify sales
                    strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

                    If strEmailToAddress <> "" Then
                        strEmailToAddress &= ";"
                    End If

                    'notify VP of sales but no backup
                    '20141001 LR
                    ' ''strEmailToAddress &= ViewState("VPSalesEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)
                End If

            End If

            'current user is CEO
            If ViewState("SubscriptionID") = 24 Then
                'notify default billing
                strEmailToAddress = ViewState("DefaultBillingEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(79, "", True)

                If strEmailToAddress <> "" Then
                    strEmailToAddress &= ";"
                End If

                'notify sales
                strEmailToAddress &= ViewState("AcctMgrEmail") 'ARGroupModule.GetAccountManagerEmailAndBackUp(ViewState("AcctMgrTMID"))

                If strEmailToAddress <> "" Then
                    strEmailToAddress &= ";"
                End If

                'notify VP of sales but no backup
                ''20141001 LR
                ' ''strEmailToAddress &= ViewState("VPSalesEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(23, "", False)

                If strEmailToAddress <> "" Then
                    strEmailToAddress &= ";"
                End If

                'notify CFO but no backup
                strEmailToAddress &= ViewState("CFOEmail") 'ARGroupModule.GetTeamMemberEmailAndBackUpBySubscriptionID(33, "", False)
            End If

            ''''''''''''''''''''''''''''''''''
            ''Build Email
            ''''''''''''''''''''''''''''''''''
            'assign email subject
            strEmailSubject = "AR Question  - Event ID:" & ViewState("AREID") & " - MESSAGE receiveD"

            strEmailBody = "<table><tr><td valign='top' width='20%'><img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger.jpg'  height='20%' width='20%'/></td><td valign='top'>"
            'strEmailBody &= "<font size='3' face='Verdana'><b>Attention</b> "
            strEmailBody &= "<font size='3' face='Verdana'><p><b>" & strCurrentUserFullName & "</b> sent you message regarding AR Event ID: <font color='red'>" & ViewState("AREID") & "</font><br />"
            strEmailBody &= "<font size='3' face='Verdana'><p><b>Event Description:</b> <font>" & ViewState("EventDesc") & "</font>.</p><br />"
            strEmailBody &= "<p><b>Question: </b><font>" & txtRSSComment.Text.Trim & "</font></p><br /><br />"

            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br /><br />"
            strEmailBody &= "<p><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/AR/AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "&pRC=1" & "'>Click here</a> to answer the message.</font>"
            strEmailBody &= "</td></tr><tr><td colspan='2'>"

            If SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody) = True Then
                'lblMessage.Text &= "<br />" & "Message Sent."
                'Else
                '    lblMessage.Text &= "<br />" & "Message Failed. Please contact IS."
            End If

            gvQuestion.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        'lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Private Sub AdjustApprovalStatusControl()

        Try

            If ddApprovalStatus.SelectedValue = 4 Then
                btnStatusSubmit.CausesValidation = False
                btnStatusSubmit.Attributes.Add("onclick", "if(confirm('Are you sure you want to submit the approval?')){}else{return false}")
                rvApprovalStatus.Enabled = False
            Else
                btnStatusSubmit.Attributes.Add("onclick", "")
                btnStatusSubmit.CausesValidation = True
                rvApprovalStatus.Enabled = True

                If ddApprovalStatus.SelectedValue = 3 Then
                    rfvApprovalComment.Enabled = True
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ddApprovalStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddApprovalStatus.SelectedIndexChanged

        Try
            AdjustApprovalStatusControl()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvQuestion.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim RSSID As Integer

                Dim drRSSID As AR.ARRSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, AR.ARRSSRow)

                If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                    RSSID = drRSSID.RSSID
                    ' Reference the rpCBRC ObjectDataSource
                    Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                    ' Set the Parameter value
                    rpCBRC.SelectParameters("AREID").DefaultValue = drRSSID.AREID.ToString
                    rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ddApprrovalRole_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddApprrovalRole.SelectedIndexChanged

        Try
            lblTeamMbr.Text = ""
            txtApprovalComment.Text = ""
            lblNotificationDate.Text = ""
            ddApprovalStatus.SelectedIndex = 0

            ViewState("SubscriptionID") = ddApprrovalRole.SelectedValue
            BindData()
            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
