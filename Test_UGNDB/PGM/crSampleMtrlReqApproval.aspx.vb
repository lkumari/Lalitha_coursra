' ************************************************************************************************
' Name:	crSampleMtrlReqApproval.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from ExpProj_Assets table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a capex asset project and approve/reject the project in one screen.
'
' Date		    Author	    
' 08/23/2010    LRey			Created .Net application
' 02/29/2012    LRey            Added new Net Mail method
' 07/19/2012	LRey	        Changed the data type to PONo from int to varchar to allow
'								Buyer's to type in PCARD when it doesn't required a PONo
' ************************************************************************************************
#Region "Directives"
Imports System.Net.Mail
Imports System.Threading
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
#End Region

Partial Class PGM_crSampleMtrlReqApproval
    Inherits System.Web.UI.Page

#Region "Page"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        ViewState("pSMRNo") = HttpContext.Current.Request.QueryString("pSMRNo")

        Dim m As ASP.crviewmasterpage_master = Master
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SampleMaterialRequestList.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1'><b>Sample Material Request Search</b></a> > <a href='SampleMaterialRequest.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1" & "'><b>Sample Material Request</b></a> > Approval"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
            Master.Page.Header.Title = "Sample Material Request # " & ViewState("pSMRNo") & " - Approval"
        End If

        ''************************************************************
        ''Code Below counts the number of chars used in comments area
        ''************************************************************
        txtComments.Attributes.Add("onkeypress", "return tbLimit();")
        txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblCommentsChar.ClientID + ");")
        txtComments.Attributes.Add("maxLength", "200")

        txtShippingComments.Attributes.Add("onkeypress", "return tbLimit();")
        txtShippingComments.Attributes.Add("onkeyup", "return tbCount(" + lblShippingCommentsChar.ClientID + ");")
        txtShippingComments.Attributes.Add("maxLength", "300")

        txtQC.Attributes.Add("onkeypress", "return tbLimit();")
        txtQC.Attributes.Add("onkeyup", "return tbCount(" + lblQCChar.ClientID + ");")
        txtQC.Attributes.Add("maxLength", "200")

        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"
        DefaultViewState()

        ''*********
        ''Get Data
        ''*********
        If Not Page.IsPostBack Then
            If ViewState("pSMRNo") <> "" Then
                BindCriteria()
                BindData()
            End If
        End If

        ''*************************************************
        '' "Form Level Security using Roles &/or Subscriptions"
        ''*************************************************
        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

    End Sub 'EOF Page_Load

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState("pSMRNo") = HttpContext.Current.Request.QueryString("pSMRNo")
        Dim oRpt As New ReportDocument()
        Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
        Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
        Dim dbConn As New TableLogOnInfo()

        If ViewState("pSMRNo") <> "" Then
            Try
                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crSampleMtrlReq.rpt")

                    'getting the database, the table and the LogOnInfo object which holds login onformation
                    crDatabase = oRpt.Database

                    'getting the table in an object array of one item 
                    Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
                    crDatabase.Tables.CopyTo(arrTables, 0)
                    ' assigning the first item of array to crTable by downcasting the object to Table 
                    crTable = arrTables(0)

                    ' setting values 
                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString
                    Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

                    ' defining report source 
                    CrystalReportViewer1.DisplayGroupTree = False
                    CrystalReportViewer1.ReportSource = oRpt
                    Session("TempCrystalRptFiles") = oRpt

                    'Check if there are parameters or not in report.
                    Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count
                    'setReportParameters()
                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                    oRpt.SetParameterValue("@SMRNo", ViewState("pSMRNo"))
                    oRpt.SetParameterValue("@URLLocation", strProdOrTestEnvironment)
                    Session("TempCrystalRptFiles") = oRpt
                Else
                    oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)
                    CrystalReportViewer1.ReportSource = oRpt
                End If
            Catch ex As Exception
                lblErrors.Text = "Error found in report view" & ex.Message
                lblErrors.Visible = "True"
            End Try
        End If
    End Sub 'EOF Page_Init

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'in order to clear crystal reports
        If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
            GC.Collect()
        End If
    End Sub 'EOF Page_Unload

#End Region 'EOF Page

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub DefaultViewState()
        ''***************
        ''Initiate Default Values as view state
        ''***************
        Dim a As String = commonFunctions.UserInfo()
        Response.Cookies("UGNDB_TMLoc").Value = HttpContext.Current.Session("UserFacility")

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

        Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
        Dim DefaultUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value
        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

        ViewState("DefaultUserEmail") = ""

        ViewState("DefaultUser") = DefaultUser
        ViewState("DefaultUserFullName") = DefaultUserFullName
        ViewState("strProdOrTestEnvironment") = strProdOrTestEnvironment
        ViewState("TMLoc") = HttpContext.Current.Session("UserFacility")
    End Sub

    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            ViewState("ObjectRole") = False
            btnSubmit.Enabled = False
            btnReset.Enabled = False
            btnSubmitCmplt.Enabled = False
            ViewState("iSETMID") = 0

            ''*********************************************************
            ''If Record is Void, do not allow Team Memember submission
            ''*********************************************************
            Dim dsSMR As DataSet = New DataSet
            ViewState("pRecStat") = Nothing
            ViewState("pRoutingStatus") = Nothing
            dsSMR = PGMModule.GetSampleMtrlReqRec(ViewState("pSMRNo"))
            If commonFunctions.CheckDataSet(dsSMR) = True Then
                ViewState("pRecStat") = dsSMR.Tables(0).Rows(0).Item("RecStatus").ToString()
                ViewState("pRoutingStatus") = dsSMR.Tables(0).Rows(0).Item("RoutingStatus").ToString()
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iTMEmail As String = ""
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 135 'Sample Material Request Form ID
            Dim iRoleID As Integer = 0
            Dim i As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            'dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Ana.Gutierrez", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iTMEmail = dsTeamMember.Tables(0).Rows(0).Item("Email")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")

                    ViewState("iTeamMemberID") = iTeamMemberID
                    ViewState("DefaultUserEmail") = iTMEmail

                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member

                        ''Locate the Shipping/EDI Coordinator to grant access Shipping Info
                        Dim dsSE As DataSet = New DataSet
                        dsSE = commonFunctions.GetTeamMemberBySubscription(147)
                        Dim iSEID As Integer = 0
                        Dim b As Integer = 0
                        ViewState("iSETMID") = 0
                        If (dsSE.Tables.Item(0).Rows.Count > 0) Then
                            For b = 0 To dsSE.Tables(0).Rows.Count - 1
                                If dsSE.Tables(0).Rows(b).Item("TMID") = iTeamMemberID Then
                                    iSEID = dsSE.Tables(0).Rows(b).Item("TMID")
                                    ViewState("iSETMID") = iSEID
                                End If
                            Next
                        End If

                        ''Locate the Manufacturing Manager who not be making the  Shipping/EDI Coordinator selection
                        Dim dsMtrlMgr As DataSet = New DataSet
                        dsMtrlMgr = commonFunctions.GetTeamMemberBySubscription(151)
                        Dim iMMTNID As Integer = 0
                        Dim c As Integer = 0
                        ViewState("iMMTNID") = 0
                        If (dsMtrlMgr.Tables.Item(0).Rows.Count > 0) Then
                            For c = 0 To dsMtrlMgr.Tables(0).Rows.Count - 1
                                If dsMtrlMgr.Tables(0).Rows(c).Item("TMID") = iTeamMemberID Then
                                    iMMTNID = dsMtrlMgr.Tables(0).Rows(c).Item("TMID")
                                    ViewState("iMMTNID") = iMMTNID
                                End If
                            Next
                        End If

                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)
                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                For i = 0 To dsRoleForm.Tables(0).Rows.Count - 1
                                    iRoleID = dsRoleForm.Tables(0).Rows(i).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ''Used by full admin such as Developers
                                            ViewState("ObjectRole") = True
                                            ViewState("Admin") = True
                                            btnSubmit.Enabled = True
                                            btnReset.Enabled = True
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ViewState("ObjectRole") = False
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            If iSEID = 0 Then
                                                If lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Pending" And (ViewState("pRecStat") <> "Void") Then
                                                    btnSubmit.Enabled = True
                                                    btnReset.Enabled = True
                                                ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing And (ViewState("pRecStat") <> "Void") Then
                                                    btnSubmit.Enabled = True
                                                    btnReset.Enabled = True
                                                    sDetail.Enabled = False
                                                ElseIf lblDateNotified.Text <> Nothing And ddStatus.SelectedValue = "Approved" And (ViewState("pRecStat") <> "Void") Then
                                                    btnSubmit.Enabled = True
                                                    btnReset.Enabled = True
                                                End If

                                                If ddShipEDICoord.SelectedValue <> Nothing And iMMTNID <> 0 Then
                                                    lblReqShipEDICoord.Visible = False
                                                    lblShipEDICoord.Visible = False
                                                    ddShipEDICoord.Visible = False
                                                End If
                                            End If
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            ViewState("ObjectRole") = False
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ViewState("ObjectRole") = True
                                            If iSEID <> 0 Then
                                                If lblDateNotified.Text <> Nothing And (ViewState("pRecStat") <> "Void") _
                                                        And (ViewState("pRecStat") <> "Completed") Then
                                                    btnSubmitCmplt.Enabled = True
                                                    gvShipping.ShowFooter = True
                                                    gvShipping.Columns(4).Visible = True
                                                    gvShipping.Columns(5).Visible = True
                                                Else
                                                    gvShipping.ShowFooter = False
                                                    gvShipping.Columns(4).Visible = False
                                                    gvShipping.Columns(5).Visible = False
                                                End If
                                            End If
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                            ''** No Entry allowed **''
                                    End Select 'EOF of "Select Case iRoleID"
                                Next
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"

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
#End Region 'EOF Form Level Security

#Region "General"
    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet
            ''bind existing data to drop down Account Manager control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(147, IIf(ViewState("TMLoc") = "UT", "", ViewState("TMLoc"))) '**SubscriptionID 147 is used for Shipping/EDI Coordinators
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddShipEDICoord.DataSource = ds
                ddShipEDICoord.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddShipEDICoord.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddShipEDICoord.DataBind()
                ddShipEDICoord.Items.Insert(0, "")
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message & " 3 "
            lblErrors.Visible = True
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF BindCriteria

    Public Sub BindData()
        Try
            Dim ds As DataSet = New DataSet
            ds = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, ViewState("iTeamMemberID"), False, False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                lblTeamMbr.Text = ds.Tables(0).Rows(0).Item("TeamMemberName").ToString()
                lblDateNotified.Text = ds.Tables(0).Rows(0).Item("DateNotified").ToString()
                ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("Status").ToString()
                txtComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString()
                hfSeqNo.Value = ds.Tables(0).Rows(0).Item("SeqNo").ToString()

                Dim ds2 As DataSet = New DataSet
                ds2 = PGMModule.GetSampleMtrlReqRec(ViewState("pSMRNo"))
                If (ds2.Tables.Item(0).Rows.Count > 0) Then
                    ddShipEDICoord.SelectedValue = ds2.Tables(0).Rows(0).Item("ShipEdiCoordTMID").ToString()
                    txtShippingComments.Text = ds2.Tables(0).Rows(0).Item("ShipComments").ToString()
                    txtProjNo.Text = ds2.Tables(0).Rows(0).Item("Projectno").ToString()
                    txtProjectTitle.Text = ds2.Tables(0).Rows(0).Item("ProjectTitle").ToString()
                    ViewState("SampleDesc") = ds2.Tables(0).Rows(0).Item("SampleDesc").ToString()
                    ViewState("DueDate") = ds2.Tables(0).Rows(0).Item("DueDate")
                    ViewState("UGNLocation") = ds2.Tables(0).Rows(0).Item("UGNFacilityName")
                    ViewState("Customer") = ds2.Tables(0).Rows(0).Item("Customer")
                    ViewState("TrialEvent") = ds2.Tables(0).Rows(0).Item("TrialEvent")
                    ViewState("Formula") = ds2.Tables(0).Rows(0).Item("Formula")
                    ViewState("RecoveryType") = ds2.Tables(0).Rows(0).Item("RecoveryType")
                    ViewState("ProdLevel") = ds2.Tables(0).Rows(0).Item("ProdLevel")
                    ViewState("ShipMethod") = ds2.Tables(0).Rows(0).Item("ShipMethod")
                    ViewState("RequestorName") = ds2.Tables(0).Rows(0).Item("RequestorName")
                    ViewState("RequestorEmail") = ds2.Tables(0).Rows(0).Item("RequestorEmail")
                    ViewState("RequestorTMID") = ds2.Tables(0).Rows(0).Item("RequestorTMID")
                    ViewState("AcctMgrEmail") = ds2.Tables(0).Rows(0).Item("AcctMgrEmail")
                    ViewState("NotifyActMgr") = ds2.Tables(0).Rows(0).Item("NotifyActMgr")
                    ViewState("QEngrEmail") = ds2.Tables(0).Rows(0).Item("QualityEngrEmail")
                    ViewState("NotifyPkgCoord") = ds2.Tables(0).Rows(0).Item("NotifyPkgCoord")
                    ViewState("PkgEmail") = ds2.Tables(0).Rows(0).Item("PackagingEmail")
                    ViewState("RecStatus") = ds2.Tables(0).Rows(0).Item("RecStatus")
                    If Not IsDBNull(ds2.Tables(0).Rows(0).Item("ShipEDICoordEmail")) Then
                        ViewState("ShipEDICoordEmail") = ds2.Tables(0).Rows(0).Item("ShipEDICoordEmail")
                    End If

                End If

                If (ds.Tables(0).Rows(0).Item("DateSigned").ToString() = Nothing Or ds.Tables(0).Rows(0).Item("DateSigned").ToString() = "") And ViewState("ObjectRole") = True Then
                    If ViewState("pRecStat") <> "Void" Then
                        btnSubmit.Enabled = True
                        btnReset.Enabled = True
                    End If
                Else
                    btnSubmit.Enabled = False
                    btnReset.Enabled = False
                End If
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
    End Sub 'EOF BindData()

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click, btnReset2.Click
        Response.Redirect("crSampleMtrlReqApproval.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1", False)
    End Sub 'EOF btnReset1_Click

    Public Function DisplayImage(ByVal EncodeType As String) As String
        Dim strReturn As String = ""

        If EncodeType = Nothing Then
            strReturn = ""
        ElseIf EncodeType = "application/vnd.ms-excel" Then
            strReturn = "~/images/xls.jpg"
        ElseIf EncodeType = "application/pdf" Then
            strReturn = "~/images/pdf.jpg"
        ElseIf EncodeType = "application/msword" Then
            strReturn = "~/images/doc.jpg"
        ElseIf EncodeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" Then
            strReturn = "~/images/xls.jpg"
        ElseIf EncodeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" Then
            strReturn = "~/images/doc.jpg"
        ElseIf EncodeType = "application/octet-stream" Then
            strReturn = "~/images/snp.jpg"
        ElseIf EncodeType = "image/pjpeg" Then
            strReturn = "~/images/pjpeg.jpg"
        End If

        Return strReturn
    End Function 'EOF DisplayImage
#End Region 'EOF General

#Region "Gridview"
    Protected Sub gvShipDocs_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShipDocs.RowDataBound

        '***
        'This section provides the user with the popup for confirming the delete of a record.
        'Called by the onClientClick event.
        '***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(4).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim FileDesc As PGM.SampleMtrlReq_DocumentsRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, PGM.SampleMtrlReq_DocumentsRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record (" & DataBinder.Eval(e.Row.DataItem, "Description") & ")?');")
                End If
            End If
        End If
    End Sub 'EOF gv...._RowDataBound

    Protected Sub gvShipping_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvShipping.RowCommand

        Try
            Dim ShipperNo As TextBox
            Dim TotalShippingCost As TextBox
            Dim FreightBillProNo As TextBox

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then
                odsShipping.InsertParameters("SMRNo").DefaultValue = ViewState("pSMRNo")

                ShipperNo = CType(gvShipping.FooterRow.FindControl("txtShipperNo"), TextBox)
                odsShipping.InsertParameters("ShipperNo").DefaultValue = ShipperNo.Text

                TotalShippingCost = CType(gvShipping.FooterRow.FindControl("txtTotalShippingCost"), TextBox)
                odsShipping.InsertParameters("TotalShippingCost").DefaultValue = TotalShippingCost.Text

                FreightBillProNo = CType(gvShipping.FooterRow.FindControl("txtFreightBillProNo"), TextBox)
                odsShipping.InsertParameters("FreightBillProNo").DefaultValue = FreightBillProNo.Text

                odsShipping.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvShipping.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvShipping.ShowFooter = True
                Else
                    gvShipping.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ShipperNo = CType(gvShipping.FooterRow.FindControl("txtShipperNo"), TextBox)
                ShipperNo.Text = ""

                TotalShippingCost = CType(gvShipping.FooterRow.FindControl("txtTotalShippingCost"), TextBox)
                TotalShippingCost.Text = ""

                FreightBillProNo = CType(gvShipping.FooterRow.FindControl("txtFreightBillProNo"), TextBox)
                FreightBillProNo.Text = ""
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF gvShipping_RowCommand

    Private Property LoadDataEmpty_gvShipping() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_gvShipping") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_gvShipping"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get

        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_gvShipping") = value
        End Set

    End Property 'EOF LoadDataEmpty_gvShipping

    Protected Sub odsShipping_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsShipping.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)


            Dim dt As PGM.SampleMtrlReq_ShippingDataTable = CType(e.ReturnValue, PGM.SampleMtrlReq_ShippingDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_gvShipping = True
            Else
                LoadDataEmpty_gvShipping = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF odsShipping_Selected

    Protected Sub gvShipping_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShipping.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_gvShipping
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br/>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF gvShipping_RowCreated

    Protected Sub gvShipping_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShipping.RowDataBound

        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(4).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim TotalShippingCost As PGM.SampleMtrlReq_ShippingRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, PGM.SampleMtrlReq_ShippingRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Shipper Number (" & DataBinder.Eval(e.Row.DataItem, "txtShipperNo") & ") entry?');")
                End If
            End If
        End If

    End Sub 'EOF gvShipping_RowDataBound

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim RSSID As Integer
            Dim drRSSID As PGM.SampleMtrlReq_RSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, PGM.SampleMtrlReq_RSSRow)

            If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                RSSID = drRSSID.RSSID
                ' Reference the rpCBRC ObjectDataSource
                Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                rpCBRC.SelectParameters("SMRNo").DefaultValue = drRSSID.SMRNo.ToString()
                rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
            End If
        End If
    End Sub 'EOF gvQuestion_RowDataBound
#End Region 'EOF Gridview 
#Region "Shipping Documents"
    Protected Sub btnUploadShipDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadShipDoc.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim RecStatus As String = Nothing
            RecStatus = ViewState("RecStatus")

            lblMessageView6.Text = Nothing
            lblMessageView6.Visible = False

            If ViewState("pSMRNo") <> 0 Then
                If uploadFileShipDoc.HasFile Then
                    If (uploadFileShipDoc.PostedFile.ContentLength <= 3500000) Then

                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFileShipDoc.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFileShipDoc.PostedFile.FileName)

                        '** With use of MS Office 2007 **/
                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(uploadFileShipDoc.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = uploadFileShipDoc.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        uploadFileShipDoc.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".xlsx") Or (FileExt = ".docx") Or (FileExt = ".snp") Or (FileExt = ".tif") Or (FileExt = ".jpg") Then

                            ''*************
                            '' Display File Info
                            ''*************
                            lblMessageView6.Text = "File name: " & uploadFileShipDoc.FileName & "<br/>" & _
                            "File Size: " & CType((SupportingDocFileSize / 1024), Integer) & " KB<br/>"
                            lblMessageView6.Visible = True
                            lblMessageView6.Width = 500
                            lblMessageView6.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            PGMModule.InsertSampleMtrlReqDocuments(ViewState("pSMRNo"), ViewState("iTeamMemberID"), "S", txtFileDesc6.Text, SupportingDocBinaryFile, uploadFileShipDoc.FileName, SupportingDocEncodeType, SupportingDocFileSize)

                            gvShipDocs.DataBind()
                            revUploadFileShipDoc.Enabled = False
                            txtFileDesc6.Text = Nothing
                        End If
                    Else
                        lblMessageView6.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                        lblMessageView6.Visible = True
                        btnUploadShipDoc.Enabled = False
                    End If
                End If

            End If
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnUploadShipDoc_Click
    Protected Sub btnResetShipDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetShipDoc.Click
        lblMessageView6.Text = Nothing
        lblMessageView6.Visible = False

        txtFileDesc6.Text = Nothing
    End Sub 'EOF btnReset 
#End Region 'EOF Shipping Documents
#Region "Email Notifications"
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        ''********
        ''* This function is used to submit email next level Approvers or to originators when rejected.
        ''********
        Dim ds1st As DataSet = New DataSet
        Dim ds2nd As DataSet = New DataSet
        Dim EmailTO As String = Nothing
        Dim EmpName As String = Nothing
        Dim EmailCC As String = Nothing
        Dim EmailFrom As String = Nothing
        Dim LvlApvlCmplt As Boolean = False
        Dim SeqNo As Integer = hfSeqNo.Value

        Dim CurrentEmpEmail As String = Nothing
        If ViewState("DefaultUserEmail") IsNot Nothing Then
            CurrentEmpEmail = ViewState("DefaultUserEmail")
            EmailFrom = CurrentEmpEmail
            EmailCC = CurrentEmpEmail & ";"
        Else
            CurrentEmpEmail = "Database.Notifications@ugnauto.com"
            EmailFrom = "Database.Notifications@ugnauto.com"
        End If

        lblErrors.Text = Nothing
        lblErrors.Visible = False
        ReqComments.Visible = False

        Try
            '********
            '* Only users with valid email accounts can send an email.
            '********
            If ddStatus.SelectedValue = "Pending" Then
                lblErrors.Text = "Action Cancelled. Please select a status Approved or Rejected."
                lblErrors.Visible = True
                CheckRights()
                Exit Sub
            End If

            If ddShipEDICoord.SelectedValue = Nothing And (ViewState("iMMTNID") = 0) Then
                lblErrors.Text = "Select a Shipping or EDI Coordinator to assign this Sample Material Request to."
                lblErrors.Visible = True
                CheckRights()
                Exit Sub
            End If

            If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> Nothing Then
                If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                    lblErrors.Text = "Your comments is required for Rejection."
                    lblErrors.Visible = True
                    ReqComments.Visible = True
                    CheckRights()
                    Exit Sub

                Else 'ELSE If ddStatus.SelectedValue = "Rejected" And txtComments.Text = Nothing Then
                    ''**********************************************************************
                    ''Check for same level records Rejected. IF so, cancel approval process.
                    ''**********************************************************************
                    If ddStatus.SelectedValue <> "Pending" Then
                        ''***********************************
                        ''Update Current Level Approver record.
                        ''***********************************
                        PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ViewState("iTeamMemberID"), True, ddStatus.SelectedValue, txtComments.Text, SeqNo, ViewState("DefaultUser"), Date.Now)

                        ''*****************
                        ''Level Completed
                        ''*****************
                        ds1st = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), SeqNo, 0, False, True)
                        'Locate any Rejected
                        If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                            'do nothing
                        Else
                            ds1st = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), SeqNo, 0, True, False)
                            'Located any Pending
                            If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                                'do nothing
                            Else 'otherwise all are approved
                                LvlApvlCmplt = True
                            End If
                        End If
                    End If

                    ''************************
                    ''Notify the Shipping / EDI Coordinator
                    ''************************
                    If ddStatus.SelectedValue <> "Rejected" And hfShipEdiCoordName.Text = Nothing And ViewState("iMMTNID") = 0 Then
                        PGMModule.InsertSampleMtrlReqAddLvl1Aprvl(ViewState("pSMRNo"), SeqNo, ddShipEDICoord.SelectedValue, ddShipEDICoord.SelectedValue, ViewState("DefaultUser"), Date.Now)

                        Dim dsSE As DataSet = New DataSet
                        dsSE = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, ddShipEDICoord.SelectedValue, False, False)
                        If commonFunctions.CheckDataSet(dsSE) = True Then
                            If IsDBNull(dsSE.Tables(0).Rows(0).Item("DateNotified")) Then
                                ''*****************
                                ''Update Ship/EDI Coordinator to routing list
                                ''*****************
                                PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ddShipEDICoord.SelectedValue, False, "Pending", "", SeqNo, ViewState("DefaultUser"), Date.Now)

                                ''*****************
                                ''Notify the Shipping/EDI Coordinator
                                ''*****************
                                EmailTO &= dsSE.Tables(0).Rows(0).Item("OrigEmail") & ";"
                                EmpName &= dsSE.Tables(0).Rows(0).Item("EmailTMName") & ", "
                                ViewState("ShipEDICoordEmail") = dsSE.Tables(0).Rows(0).Item("OrigEmail")
                                EmailCC &= ViewState("RequestorEmail") & ";"

                            End If 'EOF  If IsDBNull(dsSE.Tables(0).Rows(0).Item("DateNotified")) Then
                        End If 'EOF If commonFunctions.CheckDataSet(dsSE) = True Then
                    End If 'EOF If  hfShipEDICoordEmail.Text <> Nothing  Then

                    ''***********
                    ''* Verify that Row selected Team Member Sequence No is Last to Approve
                    ''***********
                    Dim dsLast As DataSet = New DataSet
                    Dim r As Integer = 0
                    Dim LastSeqNo As Boolean = False
                    Dim totalApprovers As Integer = 0
                    dsLast = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, 0, False, False)
                    If commonFunctions.CheckDataSet(dsLast) = True Then
                        For r = 0 To dsLast.Tables.Item(0).Rows.Count - 1
                            totalApprovers = totalApprovers + 1
                            If totalApprovers <= hfSeqNo.Value Then
                                LastSeqNo = True
                            Else
                                LastSeqNo = False
                            End If
                        Next
                    End If

                    ''**************************************************************
                    ''Locate Next Level Approver(s)
                    ''**************************************************************
                    If LvlApvlCmplt = True Then
                        ''Check at same sequence level
                        ds1st = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), SeqNo, 0, True, False)
                        If ds1st.Tables.Count > 0 And (ds1st.Tables.Item(0).Rows.Count > 0) Then
                            ''Do not send email at same level twice.
                        Else
                            If ddStatus.SelectedValue <> "Rejected" Then 'Team Member Approved
                                ds2nd = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), 0, True, False)
                                If ds2nd.Tables.Count > 0 And (ds2nd.Tables.Item(0).Rows.Count > 0) Then
                                    For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                        If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                        (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then

                                            EmailTO &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                            EmpName &= ds2nd.Tables(0).Rows(i).Item("EmailTMName") & ", "

                                            ''*****************************************
                                            ''Update Next Level Approvers DateNotified field.
                                            ''*****************************************
                                            PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ds2nd.Tables(0).Rows(i).Item("TeamMemberID"), False, "Pending", "", IIf(LastSeqNo = False, (SeqNo + 1), SeqNo), ViewState("DefaultUser"), Date.Now)

                                        End If
                                    Next
                                End If 'EOF ds2nd.Tables.Count > 0
                            End If 'EOF t.SelectedValue <> "Rejected"
                        End If 'EOF ds1st.Tables.Count > 0
                    End If 'EOF If LvlApvlCmplt = True Then


                    ''************************
                    ''* Update record
                    '*************************
                    PGMModule.UpdateSampleMtrlReqStatus(ViewState("pSMRNo"), IIf(ViewState("pRoutingStatus") <> "C", "In Process", "Completed"), IIf(ddStatus.SelectedValue = "Rejected", "R", IIf(ViewState("pRoutingStatus") <> "C", "T", "C")), IIf(ddShipEDICoord.SelectedValue = Nothing, 0, ddShipEDICoord.SelectedValue), "", ViewState("DefaultUser"))

                    'Rejected or last approval
                    If (ddStatus.SelectedValue = "Rejected" And ViewState("RecStatus") <> "Completed") _
                        Or (LvlApvlCmplt = True And ddStatus.SelectedValue = "Approved") _
                        And LastSeqNo = True Then
                        ''********************************************************
                        ''Notify SubmittedBy if Rejected or last approval
                        ''********************************************************

                        EmailTO &= ViewState("RequestorEmail") & ";"
                        EmpName &= ViewState("RequestorName") & ", "
                        If ViewState("NotifyActMgr") = True Then
                            EmailCC &= ViewState("AcctMgrEmail") & ";"
                        End If
                        EmailCC &= ViewState("QEngrEmail") & ";"
                        If ViewState("NotifyPkgCoord") = True Then
                            EmailCC &= ViewState("PkgEmail") & ";"
                        End If
                        If ViewState("ShipEDICoordEmail") <> Nothing Then EmailCC &= ViewState("ShipEDICoordEmail") & ";"
                    End If 'EOF If dsRej.Tables.Count > 0.....

                    ''********************************************************
                    ''Send Notification only if there is a valid Email Address
                    ''********************************************************
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        ''******************************************************
                        ''Carbon Copy Previous approvers
                        ''*******************************************************
                        If ddStatus.SelectedValue = "Rejected" _
                        Or (LvlApvlCmplt = True And ddStatus.SelectedValue = "Approved") _
                         And LastSeqNo = True Then
                            ds2nd = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, 0, False, False)
                            If commonFunctions.CheckDataSet(ds2nd) = True Then
                                For i = 0 To ds2nd.Tables.Item(0).Rows.Count - 1
                                    If (ds2nd.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                    (ds2nd.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                        EmailCC &= ds2nd.Tables(0).Rows(i).Item("Email") & ";"
                                    End If
                                Next
                            End If 'EOF ds2nd.Tab
                        Else
                            EmailCC &= ViewState("RequestorEmail") & ";"
                        End If

                        ''Test or Production Message display
                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                        End If

                        MyMessage.Subject &= "Sample Material Request - " & ViewState("SampleDesc")

                        If ddStatus.SelectedValue = "Rejected" Then
                            MyMessage.Subject &= " - REJECTED"
                            MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                            MyMessage.Body &= "<br/><br/>'" & ViewState("SampleDesc") & "' was <font color='red'>REJECTED</font>. <a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/SampleMaterialRequest.aspx?pSMRNo=" & ViewState("pSMRNo") & "'>Click here</a> to access the record.<br/><br/>Reason for rejection: <font color='red'>" & txtComments.Text & "</font><br/><br/>" & "</font>"
                        Else
                            If ViewState("ShipEDICoordEmail") <> Nothing And LvlApvlCmplt = False Then
                                MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                                MyMessage.Body &= "<p>'" & ViewState("SampleDesc") & "' is available for your Review/Completion. <a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/crSampleMtrlReqApproval.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>" & "</font>"
                            Else
                                If LastSeqNo = True Then
                                    MyMessage.Body &= "<font size='2' face='Tahoma'>" & "<p>'" & ViewState("SampleDesc") & "' was reviewed by all assigned team members. <a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/SampleMaterialRequest.aspx?pSMRNo=" & ViewState("pSMRNo") & "'>Click here</a> to access the record.</p>" & "</font>"
                                Else
                                    MyMessage.Body &= "<font size='2' face='Tahoma'>" & EmpName
                                    MyMessage.Body &= "<p>'" & ViewState("SampleDesc") & "' is available for your Review/Approval. <a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/crSampleMtrlReqApproval.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pAprv=1" & "'>Click here</a> to access the record.</p>" & "</font>"
                                End If
                            End If
                        End If

                        EmailBody(MyMessage, IIf(ViewState("pRecStat") = "Completed", "Completed", ""))

                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = "lynette.rey@ugnauto.com" 'CurrentEmpEmail
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        ''*****************
                        ''History Tracking
                        ''*****************
                        PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), ViewState("SampleDesc"), ViewState("iTeamMemberID"), ddStatus.SelectedValue & " " & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing))

                        ''*****************
                        ''History Tracking
                        ''*****************
                        If ddStatus.SelectedValue <> "Rejected" Then
                            If LvlApvlCmplt = True Then
                                PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), ViewState("SampleDesc"), ViewState("iTeamMemberID"), "Notification sent to all involved. ")
                            Else
                                PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), ViewState("SampleDesc"), ViewState("iTeamMemberID"), "Notification sent to " & EmpName)
                            End If
                        Else
                            PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), ViewState("SampleDesc"), ViewState("iTeamMemberID"), "Notification sent to " & EmpName)
                        End If

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Sample Material Request", ViewState("pSMRNo"))
                            lblErrors.Text = "Notification sent to " & EmpName & " successfully."
                        Catch ex As SmtpException
                            lblErrors.Text &= "Email Notification to " & EmpName & " is queued for the next automated release."
                            UGNErrorTrapping.InsertEmailQueue("Req#: " & ViewState("pSMRNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                        End Try
                        lblErrors.Visible = True
                        lblErrors.Font.Size = 12

                        ''**********************************
                        ''Rebind the data to the form
                        ''********************************** 
                        BindData()

                    Else
                        If ddStatus.SelectedValue <> "Pending" Then
                            ''*****************
                            ''History Tracking
                            ''*****************
                            PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), ViewState("SampleDesc"), ViewState("iTeamMemberID"), ddStatus.SelectedValue & IIf(txtComments.Text <> Nothing, "- Comments: " & txtComments.Text, Nothing))

                            ''**********************************
                            ''Rebind the data to the form
                            ''********************************** 
                            BindData()
                            lblErrors.Text = "Your response was submitted successfully."
                            lblErrors.Visible = True
                            lblErrors.Font.Size = 12

                        End If 'EOF  If ddStatus.SelectedValue <> "Pending" Then
                    End If
                End If
            End If 'EOF If ViewState("pSMRNo") <> Nothing Then
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

    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit2.Click
        Try
            ''********
            ''* This function is used to submit email next level Approvers or to originators when rejected.
            ''********
            Dim ds1st As DataSet = New DataSet
            Dim dsCC As DataSet = New DataSet
            Dim EmailTO As String = Nothing
            Dim EmpName As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            Dim i As Integer = 0

            Dim CurrentEmpEmail As String = Nothing
            If HttpContext.Current.Request.Cookies("UGNDB_User_Email") IsNot Nothing Then
                CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
                EmailFrom = CurrentEmpEmail
                EmailCC = CurrentEmpEmail & ";"
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If

            lblErrors.Text = Nothing
            lblErrors.Visible = False
            ReqComments.Visible = False


            '********
            '* Only users with valid email accounts can send an email.
            '********
            If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> Nothing Then
                Dim SeqNo As Integer = hfSeqNo.Value

                ''********************************************************
                ''Notify Submitter
                ''********************************************************
                EmailTO &= ViewState("RequestorEmail") & ";"
                EmpName &= ViewState("RequestorName") & ", "

                ''********************************************************
                ''Send Notification only if there is a valid Email Address
                ''********************************************************
                If EmailTO <> Nothing Then
                    Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                    Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                    ''***************************************************************
                    ''Carbon Approvers in same level
                    ''***************************************************************
                    dsCC = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), SeqNo, 0, False, False)
                    ''Check that the recipient(s) is a valid Team Member
                    If dsCC.Tables.Count > 0 And (dsCC.Tables.Item(0).Rows.Count > 0) Then
                        For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                            If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                            (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"
                            End If
                        Next
                    End If 'EOF  If dsCC.Tables.Count > 0

                    EmailCC &= ViewState("AcctMgrEmail") & ";"
                    EmailCC &= ViewState("QEngrEmail") & ";"
                    EmailCC &= ViewState("PkgEmail") & ";"
                    If ViewState("ShipEDICoordEmail") <> Nothing Then EmailCC &= ViewState("ShipEDICoordEmail") & ";"

                    'Test or Production Message display
                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Subject = "TEST: "
                        MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                    Else
                        MyMessage.Subject = ""
                        MyMessage.Body = ""
                    End If

                    MyMessage.Subject &= "Sample Material Request: " & ViewState("SampleDesc") & " - MESSAGE RECEIVED"

                    MyMessage.Body &= "<table style='font-size: 13; font-family: Tahoma;'>"
                    MyMessage.Body &= " <tr>"
                    MyMessage.Body &= "     <td valign='top' width='20%'>"
                    MyMessage.Body &= "         <img src='" & ViewState("strProdOrTestEnvironment") & "/images/messanger60.jpg'/>"
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= "     <td valign='top'>"
                    MyMessage.Body &= "             <b>Attention All,</b> "
                    MyMessage.Body &= "             <p><b>" & ViewState("DefaultUserFullName") & "</b> sent a message regarding Sample Material Request #"
                    MyMessage.Body &= "             <font color='red'>" & ViewState("pSMRNo") & " - " & ViewState("SampleDesc") & "</font>."
                    MyMessage.Body &= "             <br/><br/><i>Question:&nbsp;&nbsp;</i><b>" & txtQC.Text & "</b>"
                    MyMessage.Body &= "             </p>"
                    MyMessage.Body &= "             <p><a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/SampleMaterialRequest.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pRC=1" & "'>Click here</a> to respond."
                    MyMessage.Body &= "     </td>"
                    MyMessage.Body &= " </tr>"
                    MyMessage.Body &= "</table>"
                    MyMessage.Body &= "<br/><br/>"

                    If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                        MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                        MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                        EmailFrom = "Database.Notifications@ugnauto.com"
                        EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                        EmailCC = "lynette.rey@ugnauto.com"
                    End If

                    ''*****************
                    ''History Tracking
                    ''*****************
                    PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), ViewState("SampleDesc"), ViewState("iTeamMemberID"), "Message Sent")

                    ''*****************
                    ''Save Message
                    ''*****************
                    PGMModule.InsertSampleMtrlReqRSS(ViewState("pSMRNo"), ViewState("SampleDesc"), ViewState("iTeamMemberID"), SeqNo, txtQC.Text)

                    ''**********************************
                    ''Connect & Send email notification
                    ''**********************************
                    Try
                        commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Sample Material Request", ViewState("pSMRNo"))
                        lblErrors.Text = "Message sent to " & EmpName & " successfully."
                    Catch ex As SmtpException
                        lblErrors.Text &= "Message to " & EmpName & " is queued for the next automated release."
                        UGNErrorTrapping.InsertEmailQueue("Req#: " & ViewState("pSMRNo"), CurrentEmpEmail, EmailTO, EmailCC, MyMessage.Subject, MyMessage.Body, "")
                    End Try
                    lblErrors.Visible = True
                    lblErrors.Font.Size = 12

                    ''**********************************
                    ''Rebind the data to the form
                    ''********************************** 
                    txtQC.Text = Nothing
                    gvQuestion.DataBind()

                Else 'EmailTO = ''
                    lblErrors.Text = "Unable to locate a valid email address to send message to. Please contact the Application Department for assistance."
                    lblErrors.Visible = True
                End If 'EOF EmailTO <> ''
                BindData()
                CheckRights()
                'End If
            End If 'EOF  If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> Nothing Then
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSubmit2_Click

    Protected Sub btnSubmitCmplt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmitCmplt.Click
        Try
            Dim EmpName As String = Nothing
            Dim EmailTO As String = Nothing
            Dim EmailCC As String = Nothing
            Dim EmailFrom As String = Nothing
            Dim CurrentEmpEmail As String = Nothing
            Dim SeqNo As Integer = hfSeqNo.Value

            If ViewState("DefaultUserEmail") IsNot Nothing Then
                CurrentEmpEmail = ViewState("DefaultUserEmail")
                EmailFrom = CurrentEmpEmail
                EmailCC = CurrentEmpEmail & ";"
            Else
                CurrentEmpEmail = "Database.Notifications@ugnauto.com"
                EmailFrom = "Database.Notifications@ugnauto.com"
            End If

            lblErrors.Text = Nothing
            lblErrors.Visible = False

            If ViewState("iTeamMemberID") = ViewState("iSETMID") Then
                If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> Nothing Then

                    Dim dsSI As DataSet = New DataSet
                    Dim TotalCost As Decimal = 0
                    dsSI = PGMModule.GetSampleMtrlReqShipping(ViewState("pSMRNo"), 0)
                    If commonFunctions.CheckDataSet(dsSI) = False Then
                        lblErrors.Text = "Shipping Information is required."
                        lblErrors.Visible = True
                        MaintainScrollPositionOnPostBack = False
                        Exit Sub
                    Else
                        TotalCost += dsSI.Tables(0).Rows(0).Item("TotalShippingCost")
                    End If

                    ''*****************
                    ''Update Ship/EDI Coordinator to routing list
                    ''*****************
                    PGMModule.UpdateSampleMtrlReqApproval(ViewState("pSMRNo"), ViewState("iSETMID"), True, "Approved", txtShippingComments.Text, SeqNo, ViewState("DefaultUser"), Date.Now)

                    ''************************
                    ''* Update  record
                    '*************************
                    PGMModule.UpdateSampleMtrlReqStatus(ViewState("pSMRNo"), "Completed", "C", ViewState("iSETMID"), txtShippingComments.Text, ViewState("DefaultUser"))

                    ''*****************
                    ''History Tracking
                    ''*****************
                    PGMModule.InsertSampleMtrlReqHistory(ViewState("pSMRNo"), ViewState("SampleDesc"), ViewState("iTeamMemberID"), "Completed - Shipping Information. " & IIf(txtShippingComments.Text <> Nothing, "Comments: " & txtShippingComments.Text, Nothing))

                    ''********************************************************
                    ''Notify Project Lead
                    ''********************************************************
                    EmailTO &= ViewState("RequestorEmail") & ";"

                    ''********************************************************
                    ''Send Notification only if there is a valid Email Address
                    ''********************************************************
                    If EmailTO <> Nothing Then
                        Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim SendTo As MailAddress = New MailAddress(CurrentEmpEmail)
                        Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

                        ''*****************************
                        ''Carbon Copy List
                        ''*****************************
                        EmailCC &= ViewState("AcctMgrEmail") & ";"
                        EmailCC &= ViewState("QEngrEmail") & ";"
                        EmailCC &= ViewState("PkgEmail") & ";"

                        ''***************************************************************
                        ''Carbon Approvers in same level
                        ''***************************************************************
                        Dim dsCC As DataSet = New DataSet
                        dsCC = PGMModule.GetSampleMtrlReqApproval(ViewState("pSMRNo"), 0, 0, False, False)
                        ''Check that the recipient(s) is a valid Team Member
                        If dsCC.Tables.Count > 0 And (dsCC.Tables.Item(0).Rows.Count > 0) Then
                            For i = 0 To dsCC.Tables.Item(0).Rows.Count - 1
                                If (dsCC.Tables(0).Rows(i).Item("Email") <> CurrentEmpEmail) And _
                                (dsCC.Tables(0).Rows(i).Item("WorkStatus") = True) Then
                                    EmailCC &= dsCC.Tables(0).Rows(i).Item("Email") & ";"
                                End If
                            Next
                        End If 'EOF  If dsCC.Tables.Count > 0

                        'Test or Production Message display
                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Subject = "TEST: "
                            MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
                        Else
                            MyMessage.Subject = ""
                            MyMessage.Body = ""
                            'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
                        End If

                        MyMessage.Subject &= "Sample Material Request - " & ViewState("SampleDesc")

                        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"

                        MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>This Sample Material Request was 'COMPLETED' by " & ViewState("DefaultUserName") & ".</strong></td>"

                        MyMessage.Body &= "</table>"

                        EmailBody(MyMessage, "Completed")

                        If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                            MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                            MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                            EmailFrom = "Database.Notifications@ugnauto.com"
                            EmailTO = CurrentEmpEmail '"lynette.rey@ugnauto.com"
                            EmailCC = "lynette.rey@ugnauto.com"
                        End If

                        ''**********************************
                        ''Connect & Send email notification
                        ''**********************************
                        Try
                            commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTO, EmailCC, "", "Sample Material Request", ViewState("pSMRNo"))
                            lblErrors.Text = "Notification sent successfully to all involved."
                        Catch ex As Exception
                            lblErrors.Text &= "Email Notification is queued for the next automated release."

                            UGNErrorTrapping.InsertEmailQueue("Req#:" & ViewState("pSMRNo"), CurrentEmpEmail, EmailTO, CurrentEmpEmail & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                            'get current event name
                            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                            'log and email error
                            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
                        End Try
                        lblErrors.Visible = True
                        lblErrors.Font.Size = 12
                        MaintainScrollPositionOnPostBack = False

                        If ViewState("ShipMethod") = "Prepaid" And TotalCost > 0 Then
                            EmailFrieghtCompany(CurrentEmpEmail, "UGNAuto@chrobinson.com")
                        End If

                        ''*************************************************
                        '' "Form Level Security using Roles &/or Subscriptions"
                        ''*************************************************
                        CheckRights() '"Form Level Security using Roles &/or Subscriptions"

                    End If
                End If 'EOF  If CurrentEmpEmail <> Nothing And ViewState("pSMRNo") <> Nothing Then

            Else
                lblErrors.Text = "You do not have authorization to update Shipping Information."
                lblErrors.Visible = True
                MaintainScrollPositionOnPostBack = False
                Exit Sub
            End If 'EOF If ViewState("iTeamMemberID") = ViewState("iSETMID") Then
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub 'EOF btnSubmitCmplt_Click

    Public Function EmailFrieghtCompany(ByVal EmailFrom As String, ByVal EmailTo As String) As String
        Try
            Dim EmailCC As String = "Ron.Sintkowski@ugnauto.com; " & EmailFrom & "; " & ViewState("RequestorEmail")
            Dim SendFrom As MailAddress = New MailAddress(EmailFrom)
            Dim SendTo As MailAddress = New MailAddress(EmailFrom)
            Dim MyMessage As MailMessage = New MailMessage(SendFrom, SendTo)

            'Test or Production Message display
            If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                MyMessage.Subject = "TEST: "
                MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br/><br/>"
            Else
                MyMessage.Subject = ""
                MyMessage.Body = ""
                'MyMessage.Body = "THIS IS A TEST EMAIL. DATA IS NOT VALID FOR USE<br/><br/>"
            End If

            MyMessage.Subject &= "UGN Freight Information for Sample Material - " & ViewState("SampleDesc")

            MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px;  font-size: 13; font-family: Tahoma;'>"
            MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>UGN FREIGHT INFORMATION FOR SAMPLE MATERIAL</strong></td></tr>"

            MyMessage.Body &= "<tr>"

            MyMessage.Body &= "<table  style='font-size: 13; font-family: Tahoma;'>"
            If txtShippingComments.Text <> Nothing Then
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right'><b>Shipper/EDI Crd. Comments:</b>&nbsp;&nbsp;</td>"
                MyMessage.Body &= "<td style='width: 600px; '><font color='red'>" & txtShippingComments.Text & "</font></td>"
                MyMessage.Body &= "</tr>"
            End If
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right' >Request #:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & ViewState("pSMRNo") & "</td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>Sample Description:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & ViewState("SampleDesc") & "</td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & ViewState("UGNLocation") & "</td>"
            MyMessage.Body &= "</tr>"
            If txtProjNo.Text <> Nothing Then
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td class='p_text' align='right'>D Project No.:&nbsp;&nbsp; </td>"
                MyMessage.Body &= "<td>" & txtProjNo.Text & "</td>"
                MyMessage.Body &= "</tr>"
            End If
            MyMessage.Body &= "</table>"

            MyMessage.Body &= "</tr>"

            ''***************************************************
            ''Get Shipping Information 
            ''***************************************************
            Dim dsSI As DataSet
            dsSI = PGMModule.GetSampleMtrlReqShipping(ViewState("pSMRNo"), 0)
            If dsSI.Tables.Count > 0 And (dsSI.Tables.Item(0).Rows.Count > 0) Then
                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB; width: 100%'>"
                MyMessage.Body &= "<td colspan='2'><strong>SHIPPING INFORMATION:</strong></td>"
                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td colspan='2'>"
                MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma; width: 60%' >"
                MyMessage.Body &= "  <tr>"
                MyMessage.Body &= "   <td ><b>Shipper No</b></td>"
                MyMessage.Body &= "   <td ><b>Total Shipping Cost</b></td>"
                MyMessage.Body &= "   <td ><b>Freight Bill ProNo</b></td>"
                MyMessage.Body &= "</tr>"
                For i = 0 To dsSI.Tables.Item(0).Rows.Count - 1
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td height='25'>" & dsSI.Tables(0).Rows(i).Item("ShipperNo") & "</td>"
                    MyMessage.Body &= "<td height='25'>$ " & Format(dsSI.Tables(0).Rows(i).Item("TotalShippingCost"), "#,##0.0000") & "</td>"
                    MyMessage.Body &= "<td height='25'>" & dsSI.Tables(0).Rows(i).Item("FreightBillProNo") & "</td>"
                    MyMessage.Body &= "</tr>"
                Next
                MyMessage.Body &= "</table>"
                MyMessage.Body &= "</tr>"
            End If

            MyMessage.Body &= "</table>"

            If InStr(ViewState("strProdOrTestEnvironment"), "Test_", CompareMethod.Text) > 0 Then
                MyMessage.Body &= "<p>EmailTO: " & EmailTo & "</p>"
                MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                EmailFrom = "Database.Notifications@ugnauto.com"
                EmailTo = EmailFrom '"lynette.rey@ugnauto.com"
                EmailCC = "lynette.rey@ugnauto.com"
            End If

            ''**********************************
            ''Connect & Send email notification
            ''**********************************
            Try
                commonFunctions.Email.Send("", EmailFrom, MyMessage.Subject, MyMessage.Body, EmailTo, EmailCC, "", "Sample Material Request", ViewState("pSMRNo"))
                lblErrors.Text = "Notification sent successfully."
            Catch ex As Exception
                lblErrors.Text = "Email Notification is queued for the next automated release."

                UGNErrorTrapping.InsertEmailQueue("Req#:" & ViewState("pSMRNo"), EmailFrom, EmailTo, EmailFrom & ";" & EmailCC, MyMessage.Subject, MyMessage.Body, "")
                'get current event name
                Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

                'log and email error
                UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            End Try
            lblErrors.Visible = True
            lblErrors.Font.Size = 12
            MaintainScrollPositionOnPostBack = False

            Return True
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            Return False
        End Try
    End Function

    Public Function LinkLocationString() As String
        Dim LinkLocation As String = Nothing
        Select Case txtProjNo.Text.Substring(0, 1)
            Case "A"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjAssets.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjNo.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
            Case "D"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjDevelopment.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjNo.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
            Case "P"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjPackaging.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
            Case "R"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjRepair.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
            Case "T"
                If Not IsDBNull(txtProjectTitle.Text) Then
                    LinkLocation = "<a href='" & ViewState("strProdOrTestEnvironment") & "/EXP/crViewExpProjTooling.aspx?pProjNo=" & txtProjNo.Text & "' target='_blank'>" & txtProjNo.Text & " - " & txtProjectTitle.Text & "</a>"
                Else
                    LinkLocation = txtProjNo.Text
                End If
        End Select

        Return LinkLocation

    End Function 'EOF LinkLocationString

    Public Function EmailBody(ByVal MyMessage As MailMessage, ByVal RecStatus As String) As String
        MyMessage.Body &= "<table style='border: 1px solid #D0D0BF; width: 900px;  font-size: 13; font-family: Tahoma;'>"
        If ViewState("RecStatus") <> "Completed" And ViewState("RecStatus") <> "Void" Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB'><td colspan='2'><strong>SAMPLE MATERIAL REQUEST</strong></td></tr>"
        End If
        MyMessage.Body &= "<tr>"

        MyMessage.Body &= "<table  style='font-size: 13; font-family: Tahoma;'>"
        If RecStatus = "Completed" And txtShippingComments.Text <> Nothing Then
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'><b>Shipper/EDI Crd. Comments:</b>&nbsp;&nbsp;</td>"
            MyMessage.Body &= "<td style='width: 600px; '><font color='red'>" & txtShippingComments.Text & "</font></td>"
            MyMessage.Body &= "</tr>"
        End If
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right' >Request #:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("pSMRNo") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Sample Description:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("SampleDesc") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Requested By:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("RequestorName") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Due Date:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("DueDate") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("UGNLocation") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Customer:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("Customer") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Trial Event:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("TrialEvent") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Formula:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("Formula") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Recovery Type:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("RecoveryType") & "</td>"
        MyMessage.Body &= "</tr>"
        MyMessage.Body &= "<tr>"
        MyMessage.Body &= "<td class='p_text' align='right'>Production Level:&nbsp;&nbsp; </td>"
        MyMessage.Body &= "<td>" & ViewState("ProdLevel") & "</td>"
        MyMessage.Body &= "</tr>"
        If txtProjNo.Text <> Nothing Then
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td class='p_text' align='right'>D Project No.:&nbsp;&nbsp; </td>"
            MyMessage.Body &= "<td>" & LinkLocationString() & "</td>"
            MyMessage.Body &= "</tr>"
        End If
        MyMessage.Body &= "</table>"

        MyMessage.Body &= "</tr>"

        ''***************************************************
        ''Get list of Supporting Documentation
        ''***************************************************
        Dim dsSD As DataSet
        dsSD = PGMModule.GetSampleMtrlReqDocuments(ViewState("pSMRNo"), 0, "")
        If dsSD.Tables.Count > 0 And (dsSD.Tables.Item(0).Rows.Count > 0) Then
            MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB; width: 100%'>"
            MyMessage.Body &= "<td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
            MyMessage.Body &= "</tr>"
            MyMessage.Body &= "<tr>"
            MyMessage.Body &= "<td colspan='2'>"
            MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma;'>"
            For i = 0 To dsSD.Tables.Item(0).Rows.Count - 1
                MyMessage.Body &= "<tr>"
                Select Case dsSD.Tables(0).Rows(i).Item("Section")
                    Case "P"
                        MyMessage.Body &= "<td height='25' class='p_text'>Packaging Requirement:&nbsp;&nbsp; </td>"
                    Case "D"
                        MyMessage.Body &= "<td height='25' class='p_text'>Delivery Instructions&nbsp;&nbsp; </td>"
                    Case "L"
                        MyMessage.Body &= "<td height='25' class='p_text'>Label Requirement:&nbsp;&nbsp; </td>"
                    Case "I"
                        MyMessage.Body &= "<td height='25' class='p_text'>Invoice Information:&nbsp;&nbsp; </td>"
                    Case "S"
                        MyMessage.Body &= "<td height='25' class='p_text'>Shipping Documents:&nbsp;&nbsp; </td>"
                    Case "A"
                        MyMessage.Body &= "<td height='25' class='p_text'>Addt'l Documentation:&nbsp;&nbsp; </td>"
                End Select
                MyMessage.Body &= "<td height='25'><a href='" & ViewState("strProdOrTestEnvironment") & "/PGM/SampleMtrlReqDocument.aspx?pSMRNo=" & ViewState("pSMRNo") & "&pDocID=" & dsSD.Tables(0).Rows(i).Item("DocID") & "' target='_blank'>" & dsSD.Tables(0).Rows(i).Item("Description") & "</a></td>"
                MyMessage.Body &= "</tr>"
            Next
            MyMessage.Body &= "</table>"
            MyMessage.Body &= "</tr>"
        End If


        ''***************************************************
        ''Get Shipping Information 
        ''***************************************************
        If RecStatus = "Completed" Then
            Dim dsSI As DataSet
            dsSI = PGMModule.GetSampleMtrlReqShipping(ViewState("pSMRNo"), 0)
            If dsSI.Tables.Count > 0 And (dsSI.Tables.Item(0).Rows.Count > 0) Then
                MyMessage.Body &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB; width: 100%'>"
                MyMessage.Body &= "<td colspan='2'><strong>SHIPPING INFORMATION:</strong></td>"
                MyMessage.Body &= "</tr>"
                MyMessage.Body &= "<tr>"
                MyMessage.Body &= "<td colspan='2'>"
                MyMessage.Body &= "<table border='0' style='font-size: 13; font-family: Tahoma; width: 60%' >"
                MyMessage.Body &= "  <tr>"
                MyMessage.Body &= "   <td ><b>Shipper No</b></td>"
                MyMessage.Body &= "   <td ><b>Total Shipping Cost (USD)</b></td>"
                MyMessage.Body &= "   <td ><b>Freight Bill ProNo</b></td>"
                MyMessage.Body &= "</tr>"
                For i = 0 To dsSI.Tables.Item(0).Rows.Count - 1
                    MyMessage.Body &= "<tr>"
                    MyMessage.Body &= "<td height='25'>" & dsSI.Tables(0).Rows(i).Item("ShipperNo") & "</td>"
                    MyMessage.Body &= "<td height='25'>$ " & Format(dsSI.Tables(0).Rows(i).Item("TotalShippingCost"), "#,##0.0000") & "</td>"
                    MyMessage.Body &= "<td height='25'>" & dsSI.Tables(0).Rows(i).Item("FreightBillProNo") & "</td>"
                    MyMessage.Body &= "</tr>"
                Next
                MyMessage.Body &= "</table>"
                MyMessage.Body &= "</tr>"
            End If
        End If 'EOF  If RecStatus = "Completed" Then

        MyMessage.Body &= "</table>"

        Return True

    End Function 'EOF EmailBody()
#End Region

End Class
