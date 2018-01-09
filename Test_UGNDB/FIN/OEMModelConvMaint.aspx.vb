''******************************************************************************************************
''* OEMModelConvMaint.aspx.vb
''* The purpose of this page is to allow users to maintain and insert new OEM_Model_Conv data.
''*
''* Author  : LRey 07/15/2011
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
#Region "Directives"

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Text

#End Region

Partial Class OEM_Model_Conv_Maint
    Inherits System.Web.UI.Page
    Dim SendUserToLastPage As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "OEM Model Type Conversion"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > OEM Model Type Conversion"
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
                ViewState("sOEM") = ""
                ViewState("sCABBV") = ""
                ViewState("sSoldTo") = 0
                ViewState("sDABBV") = ""
                ViewState("sPartField") = ""
                ViewState("sOEMMfg") = ""

                BindCriteria()
                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("OEM_OEM") Is Nothing Then
                    sddOEM.SelectedValue = Server.HtmlEncode(Request.Cookies("OEM_OEM").Value)
                    ViewState("sOEM") = Server.HtmlEncode(Request.Cookies("OEM_OEM").Value)
                End If
                If Not Request.Cookies("OEM_CABBV") Is Nothing Then
                    sddCABBV.SelectedValue = Server.HtmlEncode(Request.Cookies("OEM_CABBV").Value)
                    ViewState("sCABBV") = Server.HtmlEncode(Request.Cookies("OEM_CABBV").Value)
                End If
                If Not Request.Cookies("OEM_SOLDTO") Is Nothing Then
                    sddSoldTo.SelectedValue = Server.HtmlEncode(Request.Cookies("OEM_SOLDTO").Value)
                    ViewState("sSoldTo") = Server.HtmlEncode(Request.Cookies("OEM_SOLDTO").Value)
                End If
                If Not Request.Cookies("OEM_DABBV") Is Nothing Then
                    sddDABBV.SelectedValue = Server.HtmlEncode(Request.Cookies("OEM_DABBV").Value)
                    ViewState("sDABBV") = Server.HtmlEncode(Request.Cookies("OEM_DABBV").Value)
                End If
                If Not Request.Cookies("OEM_PARTFIELD") Is Nothing Then
                    sddPartField.SelectedValue = Server.HtmlEncode(Request.Cookies("OEM_PARTFIELD").Value)
                    ViewState("sPartField") = Server.HtmlEncode(Request.Cookies("OEM_PARTFIELD").Value)
                End If
                If Not Request.Cookies("OEM_OEMMFG") Is Nothing Then
                    sddOEMMfg.SelectedValue = Server.HtmlEncode(Request.Cookies("OEM_OEMMFG").Value)
                    ViewState("sOEMMfg") = Server.HtmlEncode(Request.Cookies("OEM_OEMMFG").Value)
                End If

                If ViewState("pRowID") <> "" Then
                    BindData()
                End If
            Else
                ViewState("sOEM") = sddOEM.SelectedValue
                ViewState("sCABBV") = sddCABBV.SelectedValue
                ViewState("sSoldTo") = sddSoldTo.SelectedValue
                ViewState("sDABBV") = sddDABBV.SelectedValue
                ViewState("sPartField") = sddPartField.SelectedValue
                ViewState("sOEMMfg") = sddOEMMfg.SelectedValue
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            txtSQLQuery.Attributes.Add("onkeypress", "return tbLimit();")
            txtSQLQuery.Attributes.Add("onkeyup", "return tbCount(" + lblSQLQuery.ClientID + ");")
            txtSQLQuery.Attributes.Add("maxLength", "1000")

            txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotes.ClientID + ");")
            txtNotes.Attributes.Add("maxLength", "200")


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

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetOEMbyOEMMfg("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                sddOEM.DataSource = ds
                sddOEM.DataTextField = ds.Tables(0).Columns("ddOEMDesc").ColumnName.ToString()
                sddOEM.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("OEM").ColumnName.ToString()))

                sddOEM.DataBind()
                sddOEM.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetCABBVbyOEM("", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                sddCABBV.DataSource = ds
                sddCABBV.DataTextField = ds.Tables(0).Columns("CABBV_OEM").ColumnName.ToString()
                sddCABBV.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()

                sddCABBV.DataBind()
                sddCABBV.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetSOLDTObyCOMPNYbyCABBVbyOEM("", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                sddSoldTo.DataSource = ds
                sddSoldTo.DataTextField = ds.Tables(0).Columns("ddSoldTo").ColumnName.ToString()
                sddSoldTo.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("SoldTo").ColumnName.ToString()))

                sddSoldTo.DataBind()
                sddSoldTo.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Destination control for selection criteria for search
            ds = commonFunctions.GetCustomerDestination("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                sddDABBV.DataSource = ds
                sddDABBV.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
                sddDABBV.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("DABBV").ColumnName.ToString()))

                sddDABBV.DataBind()
                sddDABBV.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Sold To control for selection criteria for search
            ds = commonFunctions.GetOEMManufacturer("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                sddOEMMfg.DataSource = ds
                sddOEMMfg.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                sddOEMMfg.DataValueField = LTrim(RTrim(ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()))

                sddOEMMfg.DataBind()
                sddOEMMfg.Items.Insert(0, "")
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

            ds = FINModule.GetOEMModelConv(ViewState("pRowID"))

            If (ds.Tables.Item(0).Rows.Count > 0) Then
                lblRowID.Text = "Row ID: " & ViewState("pRowID")
                ddOEMValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("OEMValidator").ToString()))
                cddOEM.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("OEM").ToString()))
                ddCabbvValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("CabbvValidator").ToString()))
                cddCustomer.SelectedValue = ds.Tables(0).Rows(0).Item("CABBV").ToString()
                ddSoldToValidator.SelectedValue = LTrim(RTrim(ds.Tables(0).Rows(0).Item("SoldToValidator").ToString()))
                cddSoldTo.SelectedValue = ds.Tables(0).Rows(0).Item("SoldTo").ToString()
                txtMiscValue.Text = LTrim(RTrim(ds.Tables(0).Rows(0).Item("MiscValue").ToString()))
                cddOEMMfg.SelectedValue = ds.Tables(0).Rows(0).Item("ALTOEMManufacturer").ToString()

                ddPartField.SelectedValue = ds.Tables(0).Rows(0).Item("PartField").ToString()
                txtCPartLoc1.Text = ds.Tables(0).Rows(0).Item("CPART_LOC1").ToString()
                txtCPartLoc2.Text = ds.Tables(0).Rows(0).Item("CPART_LOC2").ToString()

                ddPartField2.SelectedValue = ds.Tables(0).Rows(0).Item("PartField2").ToString()
                txtPartSuffixLoc1.Text = ds.Tables(0).Rows(0).Item("PartSuffix_LOC1").ToString()
                txtPartSuffixLoc2.Text = ds.Tables(0).Rows(0).Item("PartSuffix_LOC2").ToString()

                txtSQLQuery.Text = ds.Tables(0).Rows(0).Item("SQLQuery").ToString()
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
            gvOEMModelConv.Columns(10).Visible = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 128 'OEM Model Type Conversion Maint form id
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionExpCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("OEM_OEM").Value = sddOEM.SelectedValue
            Response.Cookies("OEM_CABBV").Value = sddCABBV.SelectedValue
            Response.Cookies("OEM_SOLDTO").Value = sddSoldTo.SelectedValue
            Response.Cookies("OEM_DABBV").Value = sddDABBV.SelectedValue
            Response.Cookies("OEM_PARTFIELD").Value = sddPartField.SelectedValue
            Response.Cookies("OEM_OEMMFG").Value = sddOEMMfg.SelectedValue

            ' Set viewstate variable to the first page
            gvOEMModelConv.PageIndex = 1

            ' Reload control
            Response.Redirect("OEMModelConvMaint.aspx?sOEM=" & ViewState("sOEM") & "&sCABBV=" & ViewState("sCABBV") & "&sSoldTo=" & ViewState("sSoldTo") & "&sDABBV=" & ViewState("sDABBV") & "&sPartField=" & ViewState("sPartField") & "&sOEMMfg=" & ViewState("sOEMMfg"), False)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset1.Click
        Try
            FINModule.DeleteOEMConvCookies()
            HttpContext.Current.Session("sessionOEMConvCurrentPage") = Nothing

            Response.Redirect("OEMModelConvMaint.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_Click

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            Dim DefaultDate As Date = Date.Today
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUserName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value
            Dim colVal As String = Nothing
            lblErrors.Text = Nothing
            lblErrors.Visible = False

            Dim Customer As String = cddCustomer.SelectedValue
            Dim sCustomer = cddCustomer.SelectedValue.Trim
            Dim iCustomerLen As Integer = Len(sCustomer)
            Dim iCustomerStartPos As Integer = InStr(Customer, ":")
            sCustomer = sCustomer.Substring(0, iCustomerStartPos - 1)
            Customer = commonFunctions.convertSpecialChar(sCustomer, False)

            Dim AltOEMMfg As String = cddOEMMfg.SelectedValue
            Dim sAltOEMMfg = cddOEMMfg.SelectedValue.Trim
            Dim iAltOEMMfgLen As Integer = Len(sAltOEMMfg)
            Dim iAltOEMMfgStartPos As Integer = InStr(AltOEMMfg, ":")
            sAltOEMMfg = sAltOEMMfg.Substring(0, iAltOEMMfgStartPos - 1)
            AltOEMMfg = commonFunctions.convertSpecialChar(sAltOEMMfg, False)


            colVal = (ddOEM.SelectedValue & " " & Customer & " " & ddSoldTo.SelectedValue & " " & " " & txtMiscValue.Text & " " & txtCPartLoc1.Text & " " & txtCPartLoc2.Text & " " & txtNotes.Text) '' & " " & ddDestination.SelectedValue

            If colVal <> Nothing Then
                If (ViewState("pRowID") <> Nothing Or ViewState("pRowID") <> "") Then
                    FINModule.UpdateOEMModelConv(ViewState("pRowID"), IIf(ddOEM.SelectedValue = Nothing, "", IIf(ddOEMValidator.SelectedValue = Nothing, "=", ddOEMValidator.SelectedValue)), ddOEM.SelectedValue, IIf(Customer = Nothing, "", IIf(ddCabbvValidator.SelectedValue = Nothing, "=", ddCabbvValidator.SelectedValue)), Customer, IIf(ddSoldTo.SelectedValue = Nothing, "", IIf(ddSoldToValidator.SelectedValue = Nothing, "=", ddSoldToValidator.SelectedValue)), ddSoldTo.SelectedValue, "", "", IIf(txtCPartLoc1.Text = Nothing, 0, txtCPartLoc1.Text), IIf(txtCPartLoc2.Text = Nothing, 0, txtCPartLoc2.Text), txtMiscValue.Text, txtNotes.Text, ddPartField.SelectedValue, txtSQLQuery.Text, AltOEMMfg, ddPartField2.SelectedValue, IIf(txtPartSuffixLoc1.Text = Nothing, 0, txtPartSuffixLoc1.Text), IIf(txtPartSuffixLoc2.Text = Nothing, 0, txtPartSuffixLoc2.Text), DefaultUser, DefaultDate)


                Else
                    FINModule.InsertOEMModelConv(IIf(ddOEM.SelectedValue = Nothing, "", IIf(ddOEMValidator.SelectedValue = Nothing, "=", ddOEMValidator.SelectedValue)), ddOEM.SelectedValue, IIf(Customer = Nothing, "", IIf(ddCabbvValidator.SelectedValue = Nothing, "=", ddCabbvValidator.SelectedValue)), Customer, IIf(ddSoldTo.SelectedValue = Nothing, "", IIf(ddSoldToValidator.SelectedValue = Nothing, "=", ddSoldToValidator.SelectedValue)), ddSoldTo.SelectedValue, "", "", IIf(txtCPartLoc1.Text = Nothing, 0, txtCPartLoc1.Text), IIf(txtCPartLoc2.Text = Nothing, 0, txtCPartLoc2.Text), txtMiscValue.Text, txtNotes.Text, ddPartField.SelectedValue, txtSQLQuery.Text, AltOEMMfg, ddPartField2.SelectedValue, IIf(txtPartSuffixLoc1.Text = Nothing, 0, txtPartSuffixLoc1.Text), IIf(txtPartSuffixLoc2.Text = Nothing, 0, txtPartSuffixLoc2.Text), DefaultUser, DefaultDate)

                    '***************
                    '* Locate Max RequestID
                    '***************
                    Dim ds As DataSet = Nothing
                    ds = FINModule.GetLastOEMModelConvRowID()

                    ViewState("pRowID") = ds.Tables(0).Rows(0).Item("LastRowID").ToString


                End If
                gvOEMModelConv.DataBind()

                FINModule.UpdatePartNoByOEMbyRowID(ViewState("pRowID"), DefaultUser)
                FINModule.InsertPartNoByOEMbyRowID(ViewState("pRowID"), DefaultUser)

                Response.Redirect("OEMModelConvMaint.aspx?sOEM=" & ViewState("sOEM") & "&sCABBV=" & ViewState("sCABBV") & "&sSoldTo=" & ViewState("sSoldTo") & "&sDABBV=" & ViewState("sDABBV") & "&sPartField=" & ViewState("sPartField") & "&sOEMMfg=" & ViewState("sOEMMfg"), False)


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
        Response.Redirect("OEMModelConvMaint.aspx?sOEM=" & ViewState("sOEM") & "&sCABBV=" & ViewState("sCABBV") & "&sSoldTo=" & ViewState("sSoldTo") & "&sDABBV=" & ViewState("sDABBV") & "&sPartField=" & ViewState("sPartField") & "&sOEMMfg=" & ViewState("sOEMMfg"))
    End Sub 'EOF btnReset_Click

    Protected Sub gvOEMModelConv_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOEMModelConv.RowCreated
        ''Do nothing
    End Sub 'EOF gvOEMModelConv_RowCreated

    Protected Sub gvOEMModelConv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOEMModelConv.RowDataBound
        ''***
        ''This section provides the user with the popup for confirming the delete of a record.
        ''Called by the onClientClick event.
        ''***
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' reference the Delete ImageButton
            Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            If imgBtn IsNot Nothing Then
                Dim db As ImageButton = CType(e.Row.Cells(10).Controls(1), ImageButton)

                ' Get information about the product bound to the row
                If db.CommandName = "Delete" Then
                    Dim price As Financials.OEM_Model_ConvRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Financials.OEM_Model_ConvRow)

                    db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this conversion record?');")
                End If
            End If
        End If
    End Sub 'EOF gvOEMModelConv_RowDataBound

    Protected Sub gvOEMModelConv_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOEMModelConv.DataBound
        PagingInformation.Text = String.Format("You are viewing page {0} of {1}...   Go to ", _
                                               gvOEMModelConv.PageIndex + 1, gvOEMModelConv.PageCount)

        ' Clear out all of the items in the DropDownList
        PageList.Items.Clear()

        ' Add a ListItem for each page
        For i As Integer = 0 To gvOEMModelConv.PageCount - 1

            ' Add the new ListItem   
            Dim pageListItem As New ListItem(String.Concat("Page ", i + 1), i.ToString())
            PageList.Items.Add(pageListItem)
            ' select the current item, if needed   
            If i = gvOEMModelConv.PageIndex Then
                pageListItem.Selected = True
            End If
        Next
    End Sub 'EOF ChartSpec_DataBound

    Protected Sub PageList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageList.SelectedIndexChanged

        ' Jump to the specified page       
        gvOEMModelConv.PageIndex = Convert.ToInt32(PageList.SelectedValue)
    End Sub 'PageList_SelectIndexChanged

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'If String.IsNullOrEmpty(gvOEMModelConv.SortExpression) Then
        '    'Set a Default Sort By - Formula Name
        '    gvOEMModelConv.Sort("OEMStmt", SortDirection.Ascending)
        'End If

        ' Only add the sorting UI if the GridView is sorted
        If Not String.IsNullOrEmpty(gvOEMModelConv.SortExpression) Then
            ' Determine the index and HeaderText of the column that 
            'the data is sorted by
            Dim sortColumnIndex As Integer = -1
            Dim sortColumnHeaderText As String = String.Empty
            For i As Integer = 0 To gvOEMModelConv.Columns.Count - 1
                If gvOEMModelConv.Columns(i).SortExpression.CompareTo(gvOEMModelConv.SortExpression) = 0 Then
                    sortColumnIndex = i
                    sortColumnHeaderText = gvOEMModelConv.Columns(i).HeaderText
                    Exit For
                End If
            Next

            ' Reference the Table the GridView has been rendered into
            Dim gridTable As Table = CType(gvOEMModelConv.Controls(0), Table)

            ' Enumerate each TableRow, adding a sorting UI header if
            ' the sorted value has changed
            Dim lastValue As String = String.Empty
            For Each gvr As GridViewRow In gvOEMModelConv.Rows
                Dim currentValue As String = String.Empty
                If gvr.Cells(sortColumnIndex).Controls.Count > 0 Then
                    If TypeOf gvr.Cells(sortColumnIndex).Controls(0) Is CheckBox Then
                        If CType(gvr.Cells(sortColumnIndex).Controls(0), CheckBox).Checked Then
                            currentValue = "Yes"
                        Else
                            currentValue = "No"
                        End If

                        ' ... Add other checks here if using columns with other
                        '      Web controls in them (Calendars, DropDownLists, etc.) ...
                    End If
                Else
                    currentValue = gvr.Cells(sortColumnIndex).Text
                End If

                If lastValue.CompareTo(currentValue) <> 0 Then
                    ' there's been a change in value in the sorted column
                    Dim rowIndex As Integer = gridTable.Rows.GetRowIndex(gvr)

                    ' Add a new sort header row
                    Dim sortRow As New GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal)
                    Dim sortCell As New TableCell()
                    sortCell.ColumnSpan = gvOEMModelConv.Columns.Count
                    sortCell.Text = String.Format("{0}: {1}", sortColumnHeaderText, currentValue)
                    sortCell.CssClass = "SortHeaderRowStyle"

                    ' Add sortCell to sortRow, and sortRow to gridTable
                    sortRow.Cells.Add(sortCell)
                    gridTable.Controls.AddAt(rowIndex, sortRow)

                    ' Update lastValue
                    lastValue = currentValue
                End If
            Next
        End If

        MyBase.Render(writer)
    End Sub 'Render

End Class
