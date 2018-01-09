' ************************************************************************************************
'
' Name:		AR_Event_Accrual.aspx
' Purpose:	This Code Behind is for the AR Event Accrual
'
' Date		Author	    
' 03/25/2010   Created  Roderick Carlson
' 12/27/2011   Modified Roderick Carlson - adjusted delete override row to also update accrual details
' 03/02/2012   Modified Roderick Carlson - add button to allow accounting to refresh accrual calculations
' 08/14/2012   Modified Roderick Carlson - allow Override comments to be 2000 characters
' 12/26/2013    LRey    Disabled the Save and Update Accrual button.

Partial Class AR_Event_Accrual
    Inherits System.Web.UI.Page
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS

    End Sub
    Private Sub PrepareGridViewForExport(ByRef gv As Control)

        'Dim l As Literal = New Literal()
        'Dim i As Integer


        'For i = 0 To gv.Controls.Count

        '    If ((Nothing <> htControls(gv.Controls(i).GetType().Name)) Or (Nothing <> htControls(gv.Controls(i).GetType().BaseType.Name))) Then
        '        l.Text = GetControlPropertyValue(gv.Controls(i))

        '        gv.Controls.Remove(gv.Controls(i))

        '        gv.Controls.AddAt(i, l)

        '    End If

        '    If (gv.Controls(i).HasControls()) Then

        '        PrepareGridViewForExport(gv.Controls(i))

        '    End If

        'Next

    End Sub
    Private Function GetControlPropertyValue(ByVal control As Control) As String
        'Dim controlType As Type = control.[GetType]()
        'Dim strControlType As String = controlType.Name
        Dim strReturn As String = "Error"
        'Dim bReturn As Boolean

        'Dim ctrlProps As System.Reflection.PropertyInfo() = controlType.GetProperties()
        'Dim ExcelPropertyName As String = DirectCast(htControls(strControlType), String)

        'If ExcelPropertyName Is Nothing Then
        '    ExcelPropertyName = DirectCast(htControls(control.[GetType]().BaseType.Name), String)
        '    If ExcelPropertyName Is Nothing Then
        '        Return strReturn
        '    End If
        'End If

        'For Each ctrlProp As System.Reflection.PropertyInfo In ctrlProps

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(String) Then
        '        Try
        '            strReturn = DirectCast(ctrlProp.GetValue(control, Nothing), String)
        '            Exit Try
        '        Catch
        '            strReturn = ""
        '        End Try
        '    End If

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(Boolean) Then
        '        Try
        '            bReturn = CBool(ctrlProp.GetValue(control, Nothing))
        '            strReturn = IIf(bReturn, "True", "False")
        '            Exit Try
        '        Catch
        '            strReturn = "Error"
        '        End Try
        '    End If

        '    If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(ListItem) Then
        '        Try
        '            strReturn = DirectCast((ctrlProp.GetValue(control, Nothing)), ListItem).Text
        '            Exit Try
        '        Catch
        '            strReturn = ""
        '        End Try
        '    End If
        'Next
        Return strReturn
    End Function
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Try
            ClearMessages()

            Dim attachment As String = "attachment; filename=AREventAccrualInfo.xls"

            Response.ClearContent()

            Response.AddHeader("content-disposition", attachment)

            Response.ContentType = "application/ms-excel"

            Dim sw As StringWriter = New StringWriter()

            Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)

            'EnablePartialRendering = False

            'gvShippingInfo.RenderControl(htw)

            Dim tempDataGridView As New GridView
            tempDataGridView = gvAccrual
            tempDataGridView.Columns(tempDataGridView.Columns.Count - 3).Visible = False
            tempDataGridView.Columns(tempDataGridView.Columns.Count - 4).Visible = False
            tempDataGridView.PageSize = 64000
            tempDataGridView.DataBind()

            'tempDataGridView.AllowPaging = False
            'tempDataGridView.AllowSorting = False

            tempDataGridView.HeaderStyle.BackColor = Color.White 'System.Drawing.Color.LightGray
            tempDataGridView.HeaderStyle.ForeColor = Color.Black 'System.Drawing.Color.Black
            tempDataGridView.HeaderStyle.Font.Bold = True

            tempDataGridView.BottomPagerRow.Visible = False

            'always make sure the ASPX page has EnableEventValidation="false" 
            tempDataGridView.RenderControl(htw)

            Response.Write(sw.ToString())

            Response.End()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RememberAccrualRowSelectedValues()

        Try
            Dim SelectedAccrualRowList As New Collections.ArrayList()

            Dim index As Integer = -1
            'Dim index As String = ""

            For Each row As GridViewRow In gvAccrual.Rows

                index = CInt(gvAccrual.DataKeys(row.RowIndex).Value)
                'index = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS") IsNot Nothing Then
                    SelectedAccrualRowList = DirectCast(Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS"), Collections.ArrayList)
                End If

                If result Then
                    If Not SelectedAccrualRowList.Contains(index) Then
                        SelectedAccrualRowList.Add(index)
                    End If
                Else
                    SelectedAccrualRowList.Remove(index)
                End If
            Next

            If SelectedAccrualRowList IsNot Nothing AndAlso SelectedAccrualRowList.Count > 0 Then
                Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS") = SelectedAccrualRowList
            Else
                Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS") = Nothing
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RememberAccrualRowUNSelectedValues()

        Try
            Dim UnSelectedAccrualRowList As New Collections.ArrayList()

            Dim index As Integer = -1

            For Each row As GridViewRow In gvAccrual.Rows

                index = CInt(gvAccrual.DataKeys(row.RowIndex).Value)

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS") IsNot Nothing Then
                    UnSelectedAccrualRowList = DirectCast(Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS"), Collections.ArrayList)
                End If

                If result = False Then
                    If Not UnSelectedAccrualRowList.Contains(index) Then
                        UnSelectedAccrualRowList.Add(index)
                    End If
                Else
                    UnSelectedAccrualRowList.Remove(index)
                End If
            Next

            If UnSelectedAccrualRowList IsNot Nothing AndAlso UnSelectedAccrualRowList.Count > 0 Then
                Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS") = UnSelectedAccrualRowList
            Else
                Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS") = Nothing
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RePopulateAccrualRowSelectedValues()

        Try
            Dim SelectedAccrualRowList As Collections.ArrayList = DirectCast(Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS"), Collections.ArrayList)

            If SelectedAccrualRowList IsNot Nothing AndAlso SelectedAccrualRowList.Count > 0 Then
                For Each row As GridViewRow In gvAccrual.Rows

                    Dim index As Integer = CInt(gvAccrual.DataKeys(row.RowIndex).Value)
                    'Dim index As String = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                    If SelectedAccrualRowList.Contains(index) Then
                        Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                        myCheckBox.Checked = True
                    End If

                Next

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RePopulateAccrualRowUnSelectedValues()

        Try
            Dim UnSelectedAccrualRowList As Collections.ArrayList = DirectCast(Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS"), Collections.ArrayList)

            If UnSelectedAccrualRowList IsNot Nothing AndAlso UnSelectedAccrualRowList.Count > 0 Then
                For Each row As GridViewRow In gvAccrual.Rows

                    Dim index As Integer = CInt(gvAccrual.DataKeys(row.RowIndex).Value)

                    If UnSelectedAccrualRowList.Contains(index) Then
                        Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                        myCheckBox.Checked = False
                    End If

                Next

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub ClearMessages()

        Try
            lblMessage.Text = ""
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cbSelectAccrualRow_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            ClearMessages()

            Dim cbSelectedCheckbox As CheckBox

            'ViewState("CheckedAllRows") = False

            cbSelectedCheckbox = CType(sender, CheckBox)

            'If cbSelectedCheckbox.Checked = True Then
            '    lblMessage.Text = cbSelectedCheckbox.ToolTip & " was checked."
            'Else
            '    lblMessage.Text = cbSelectedCheckbox.ToolTip & " was UNchecked."
            'End If

            If ViewState("CheckedAllRows") = True Then
                RememberAccrualRowUNSelectedValues()
            Else
                RememberAccrualRowSelectedValues()
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindCriteria()

        Try
            'bind existing data to drop down controls for selection criteria for search       

            Dim ds As DataSet

            ds = ARGroupModule.GetAREventTypeList(False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddEventType.DataSource = ds
                ddEventType.DataTextField = ds.Tables(0).Columns("ddEventTypeName").ColumnName.ToString()
                ddEventType.DataValueField = ds.Tables(0).Columns("EventTypeID").ColumnName
                ddEventType.DataBind()
                ddEventType.Items.Insert(0, "")
            End If

            'ds =ARGroupModule.GetAREventStatusList()
            'If commonFunctions.CheckDataset(ds) = True Then
            '    ddEventStatus.DataSource = ds
            '    ddEventStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
            '    ddEventStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
            '    ddEventStatus.DataBind()
            '    ddEventStatus.Items.Insert(0, "")
            'End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InitializeViewState()

        Try

            ViewState("isAdmin") = False
            ViewState("isDefaultBilling") = False

            ViewState("CheckedAllRows") = False
            ViewState("AREID") = 0
            ViewState("CurrentTeamMemberID") = 0
            ViewState("EventStatusID") = 0
            ViewState("SubscriptionID") = 0

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindData()

        Try
            Dim ds As DataSet

            ds = ARGroupModule.GetAREvent(ViewState("AREID"))

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("EventTypeID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("EventTypeID") > 0 Then
                        ddEventType.SelectedValue = ds.Tables(0).Rows(0).Item("EventTypeID")
                    End If
                End If

                txtOverrideCurrentPriceComment.Text = ds.Tables(0).Rows(0).Item("OverrideCurrentPriceComment").ToString

                If ds.Tables(0).Rows(0).Item("EventStatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("EventStatusID") > 0 Then
                        ViewState("EventStatusID") = ds.Tables(0).Rows(0).Item("EventStatusID")
                    End If
                End If

                lblShipDateFrom.Text = ds.Tables(0).Rows(0).Item("CustApprvEffDate").ToString

                If ds.Tables(0).Rows(0).Item("CustApprvEndDate").ToString.Trim <> "" Then
                    lblShipDateTo.Text = ds.Tables(0).Rows(0).Item("CustApprvEndDate").ToString
                Else
                    lblShipDateTo.Text = "None"
                End If
            End If

            ds = ARGroupModule.GetAREventAccrualTotals(ViewState("AREID"))

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("TotalQTYSHP") IsNot System.DBNull.Value Then
                    lblTotalQuantityShipped.Text = Format(ds.Tables(0).Rows(0).Item("TotalQTYSHP"), "#,###,###")                    
                End If

                If ds.Tables(0).Rows(0).Item("TotalShippingPriceByQTYSHP") IsNot System.DBNull.Value Then
                    lblTotalShippedPriceByQuantityShipped.Text = Format(ds.Tables(0).Rows(0).Item("TotalShippingPriceByQTYSHP"), "$#,###,###,##0.000000")
                End If

                If ds.Tables(0).Rows(0).Item("TotalAccrual") IsNot System.DBNull.Value Then
                    lblCalculatedDeductionAmount.Text = Format(ds.Tables(0).Rows(0).Item("TotalAccrual"), "$#,###,###,##0.000000")
                End If

                If ds.Tables(0).Rows(0).Item("TotalOverridePriceByQTYSHP") IsNot System.DBNull.Value Then
                    lblTotalOverrideShippedPriceByQuantityShipped.Text = Format(ds.Tables(0).Rows(0).Item("TotalOverridePriceByQTYSHP"), "$#,###,###,##0.000000")
                End If

                If ds.Tables(0).Rows(0).Item("TotalOverrideAccrual") IsNot System.DBNull.Value Then
                    lblOverrideCalculatedDeductionAmount.Text = Format(ds.Tables(0).Rows(0).Item("TotalOverrideAccrual"), "$#,###,###,##0.000000")
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub EnableControls()

        Try

            gvAccrual.Columns(gvAccrual.Columns.Count - 3).Visible = False
            gvAccrual.Columns(gvAccrual.Columns.Count - 4).Visible = False

            gvAccrualOverride.Visible = False

            btnSave.Visible = False
            btnSelectAllRows.Visible = False
            btnUnselectAllRows.Visible = False
            btnUpdateAccrual.Visible = False

            lblNote1.Visible = False
            lblOverrideCurrentPrice.Visible = False

            btnUpdateCurrentPrice.Visible = False

            txtOverrideCurrentPrice.Visible = False
            txtOverrideCurrentPriceComment.Enabled = False

            If ViewState("EventStatusID") <> 10 And ViewState("EventStatusID") <> 1 Then
                If lblTotalQuantityShipped.Text.Trim <> "" Then
                    If CType(lblTotalQuantityShipped.Text.Trim, Integer) <> 0 Then
                        btnExportToExcel.Visible = True
                    End If
                End If
            End If

            If ViewState("EventStatusID") <> 9 And ViewState("EventStatusID") <> 10 Then
                If ViewState("SubscriptionID") = 21 And ViewState("isAdmin") = True Then
                    txtOverrideCurrentPriceComment.Enabled = ViewState("isAdmin")
                    btnSave.Visible = ViewState("isAdmin")
                    btnUpdateAccrual.Visible = ViewState("isAdmin")
                End If

                If ViewState("SubscriptionID") = 21 And ViewState("isAdmin") = True And txtOverrideCurrentPriceComment.Text.Trim <> "" Then
                    gvAccrual.Columns(gvAccrual.Columns.Count - 3).Visible = ViewState("isAdmin")
                    gvAccrual.Columns(gvAccrual.Columns.Count - 4).Visible = ViewState("isAdmin")

                    gvAccrualOverride.Visible = ViewState("isAdmin")

                    btnSelectAllRows.Visible = ViewState("isAdmin")
                    btnUnselectAllRows.Visible = ViewState("isAdmin")

                    lblNote1.Visible = ViewState("isAdmin")
                    lblOverrideCurrentPrice.Visible = ViewState("isAdmin")

                    btnUpdateCurrentPrice.Visible = ViewState("isAdmin")
                    txtOverrideCurrentPrice.Visible = ViewState("isAdmin")

                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub CheckRights()

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
            ViewState("CurrentTeamMemberID") = 0
            ViewState("isAdmin") = False
            'ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            'dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Gina.Lacny", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                ''test developer as another team member
                If iTeamMemberID = 530 Then
                    'mike echevarria
                    'iTeamMemberID = 246

                    ' ''gina lacny
                    iTeamMemberID = 627

                    ' ''gary hibbler
                    'iTeamMemberID = 671

                    'Ilysa.Albright 
                    'iTeamMemberID = 636

                    'Kara.North 
                    'iTeamMemberID = 667

                    'Kelly.Carolyn 
                    'iTeamMemberID = 638

                    'Jeffrey.Kist 
                    'iTeamMemberID = 718
                End If

                ViewState("CurrentTeamMemberID") = iTeamMemberID

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

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 49) '52

                If dsRoleForm IsNot Nothing Then
                    If dsRoleForm.Tables.Count > 0 And dsRoleForm.Tables(0).Rows.Count > 0 Then
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
                                ViewState("isAdmin") = True
                            Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                        End Select
                    End If
                End If
            End If


        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page            
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "AR Event Accrual"

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("ARExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HandleMultiLineTextBoxes()

        Try

            txtOverrideCurrentPriceComment.Attributes.Add("onkeypress", "return tbLimit();")
            txtOverrideCurrentPriceComment.Attributes.Add("onkeyup", "return tbCount(" + lblOverrideCurrentPriceCommentCharCount.ClientID + ");")
            txtOverrideCurrentPriceComment.Attributes.Add("maxLength", "2000")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                InitializeViewState()

                'ViewState("CheckedAllRows") = False
                Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS") = Nothing
                Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS") = Nothing

                CheckRights()

                BindCriteria()

                If HttpContext.Current.Request.QueryString("AREID") <> "" Then
                    ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")
                    lblAREID.Text = ViewState("AREID")

                    HandleMultiLineTextBoxes()

                    BindData()

                    EnableControls()
                End If

            End If

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID against the deduction table, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > <a href='AR_Event_List.aspx'>AR Event Search </a> > <a href='AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "'> Event Detail </a> > Event Accrual "

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvAccrual_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAccrual.PageIndexChanging

        Try

            If ViewState("CheckedAllRows") = True Then
                RememberAccrualRowUNSelectedValues()
            Else
                RememberAccrualRowSelectedValues()
            End If

            gvAccrual.PageIndex = e.NewPageIndex
            gvAccrual.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvAccrual_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAccrual.RowDataBound

        Try

            Dim index As Integer = 0

            If ViewState("CheckedAllRows") = True Then
                'parse all rows to check
                For Each row As GridViewRow In gvAccrual.Rows

                    index = CInt(gvAccrual.DataKeys(row.RowIndex).Value)

                    Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                    myCheckBox.Checked = True

                Next

                RePopulateAccrualRowUnSelectedValues()
            Else

                'parse all rows to Uncheck
                For Each row As GridViewRow In gvAccrual.Rows

                    index = CInt(gvAccrual.DataKeys(row.RowIndex).Value)

                    Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                    myCheckBox.Checked = False

                Next

                RePopulateAccrualRowSelectedValues()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvAccrual_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvAccrual.RowUpdated

        Try
            ClearMessages()

            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnUpdateCurrentPrice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateCurrentPrice.Click

        Try
            ClearMessages()

            'see if CheckAll was selected
            'parse all checked boxes
            'update by rowID

            Dim ds As DataSet

            Dim CollectionRowList As New Collections.ArrayList()

            Dim iAccrualRowCounter As Integer = 0
            Dim iAccrualRowTotal As Integer = 0
            Dim iAccrualRowID As Integer = 0

            Dim iCollectionRowCounter As Integer = 0
            Dim iCollectionRowTotal As Integer = 0
            Dim iCollectionRowID As Integer = 0

            Dim bFoundIT As Boolean = False

            Dim dOverridePrice As Double = 0

            If txtOverrideCurrentPrice.Text.Trim <> "" Then
                dOverridePrice = CType(txtOverrideCurrentPrice.Text.Trim, Double)
            End If

            If ViewState("CheckedAllRows") = True Then
                'update all rows regardless of gridview, except for specifically UNchecked rows

                If Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS") IsNot Nothing Then
                    CollectionRowList = DirectCast(Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS"), Collections.ArrayList)
                End If

                If Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS") IsNot Nothing And CollectionRowList IsNot Nothing Then
                    iCollectionRowTotal = CollectionRowList.Count
                End If

                ds = ARGroupModule.GetAREventAccrual(ViewState("AREID"))

                If commonFunctions.CheckDataSet(ds) = True Then
                    For iAccrualRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        bFoundIT = False
                        If ds.Tables(0).Rows(iAccrualRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iAccrualRowCounter).Item("RowID") > 0 Then
                                iAccrualRowID = ds.Tables(0).Rows(iAccrualRowCounter).Item("RowID")

                                For iCollectionRowCounter = 0 To iCollectionRowTotal - 1
                                    'an unchecked row was found
                                    iCollectionRowID = CollectionRowList.Item(iCollectionRowCounter)

                                    'do not update unchecked rows
                                    If iAccrualRowID = iCollectionRowID Then
                                        bFoundIT = True
                                    End If
                                Next

                                If bFoundIT = False Then
                                    ARGroupModule.UpdateAREventAccrualCurrentPrice(ViewState("AREID"), iAccrualRowID, dOverridePrice, iAccrualRowID)
                                End If

                            End If
                        End If
                    Next
                End If
            Else
                'update only rows selected/checked on gridview
                If Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS") IsNot Nothing Then

                    CollectionRowList = DirectCast(Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS"), Collections.ArrayList)

                    If Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS") IsNot Nothing And CollectionRowList IsNot Nothing Then
                        iCollectionRowTotal = CollectionRowList.Count
                    End If

                    For iCollectionRowCounter = 0 To iCollectionRowTotal - 1
                        iCollectionRowID = CollectionRowList.Item(iCollectionRowCounter)

                        ARGroupModule.UpdateAREventAccrualCurrentPrice(ViewState("AREID"), iCollectionRowID, dOverridePrice, iCollectionRowID)
                    Next

                End If

            End If

            ARGroupModule.UpdateAREventOverridePriceComment(ViewState("AREID"), txtOverrideCurrentPriceComment.Text.Trim)

            UnselectAllRows()

            BindData()

            gvAccrual.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSelectAllRows_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAllRows.Click

        Try

            ClearMessages()

            ViewState("CheckedAllRows") = True

            Dim index As Integer = 0

            'parse all rows to check
            For Each row As GridViewRow In gvAccrual.Rows

                index = CInt(gvAccrual.DataKeys(row.RowIndex).Value)

                Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                myCheckBox.Checked = True

            Next

            Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS") = Nothing

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub UnselectAllRows()

        Try
            ViewState("CheckedAllRows") = False

            Dim index As Integer = 0

            'parse all rows to Uncheck
            For Each row As GridViewRow In gvAccrual.Rows

                index = CInt(gvAccrual.DataKeys(row.RowIndex).Value)

                Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                myCheckBox.Checked = False

            Next

            Session("CHECKED_ACCRUAL_ROW_SELECTED_ITEMS") = Nothing
            Session("CHECKED_ACCRUAL_ROW_UNSELECTED_ITEMS") = Nothing

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnUnselectAllRows_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnselectAllRows.Click

        Try

            ClearMessages()

            UnselectAllRows()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            ClearMessages()

            ARGroupModule.UpdateAREventOverridePriceComment(ViewState("AREID"), txtOverrideCurrentPriceComment.Text.Trim)

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Property LoadDataEmpty_AccrualOverride() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_AccrualOverride") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_AccrualOverride"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_AccrualOverride") = value
        End Set

    End Property
    Protected Sub odsAccrualOverride_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsAccrualOverride.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            'Dim dt As Costing.CostSheetMaterial_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetMaterial_MaintDataTable)
            Dim dt As AR.AREventAccrualOverrideCriteriaDataTable = CType(e.ReturnValue, AR.AREventAccrualOverrideCriteriaDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_AccrualOverride = True
            Else
                LoadDataEmpty_AccrualOverride = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub gvAccrualOverride_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAccrualOverride.RowCommand

        Try

            ClearMessages()

            Dim ddTempInsertPartNo As DropDownList
            Dim ddTempInsertPriceCode As DropDownList
            Dim txtTempInsertStartShipDate As TextBox
            Dim txtTempInsertEndShipDate As TextBox
            Dim txtTempInsertOverrideRELPRC As TextBox
            Dim dTempInsertOverrideRELPRC As Double = 0
            Dim bValidDate As Boolean = True

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddTempInsertPartNo = CType(gvAccrualOverride.FooterRow.FindControl("ddInsertPartNo"), DropDownList)
                ddTempInsertPriceCode = CType(gvAccrualOverride.FooterRow.FindControl("ddInsertPriceCode"), DropDownList)

                txtTempInsertStartShipDate = CType(gvAccrualOverride.FooterRow.FindControl("txtInsertStartShipDate"), TextBox)
                txtTempInsertEndShipDate = CType(gvAccrualOverride.FooterRow.FindControl("txtInsertEndShipDate"), TextBox)
                txtTempInsertOverrideRELPRC = CType(gvAccrualOverride.FooterRow.FindControl("txtInsertAccrualOverrideCurrentPrice"), TextBox)

                If ddTempInsertPartNo.SelectedIndex > 0 Or ddTempInsertPriceCode.SelectedIndex > 0 Or txtTempInsertStartShipDate.Text.Trim <> "" Or txtTempInsertEndShipDate.Text.Trim <> "" Then

                    'check date range to make sure start date is less than end date
                    If txtTempInsertStartShipDate.Text.Trim <> "" And txtTempInsertEndShipDate.Text.Trim <> "" Then
                        If CType(txtTempInsertStartShipDate.Text.Trim, Date) > CType(txtTempInsertEndShipDate.Text.Trim, Date) Then
                            bValidDate = False
                        End If
                    End If

                    If bValidDate = True Then
                        If txtTempInsertOverrideRELPRC.Text.Trim <> "" Then
                            dTempInsertOverrideRELPRC = CType(txtTempInsertOverrideRELPRC.Text.Trim, Double)
                        End If

                        odsAccrualOverride.InsertParameters("AREID").DefaultValue = ViewState("AREID")
                        odsAccrualOverride.InsertParameters("PartNo").DefaultValue = ddTempInsertPartNo.SelectedValue.Trim
                        odsAccrualOverride.InsertParameters("PRCCDE").DefaultValue = ddTempInsertPriceCode.SelectedValue.Trim
                        odsAccrualOverride.InsertParameters("Override_RELPRC").DefaultValue = dTempInsertOverrideRELPRC
                        odsAccrualOverride.InsertParameters("StartShipDate").DefaultValue = txtTempInsertStartShipDate.Text.Trim
                        odsAccrualOverride.InsertParameters("EndShipDate").DefaultValue = txtTempInsertEndShipDate.Text.Trim

                        intRowsAffected = odsAccrualOverride.Insert()

                        gvAccrual.DataBind()
                        BindData()

                    Else
                        lblMessage.Text &= "Error: The start date must be less than the end date."
                    End If

                Else
                    lblMessage.Text &= "Error: At least one criteria must be used to be added to the list."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvAccrualOverride.ShowFooter = False
            Else
                gvAccrualOverride.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                ddTempInsertPartNo = CType(gvAccrualOverride.FooterRow.FindControl("ddInsertPartNo"), DropDownList)
                ddTempInsertPartNo.SelectedIndex = -1

                ddTempInsertPriceCode = CType(gvAccrualOverride.FooterRow.FindControl("ddInsertPriceCode"), DropDownList)
                ddTempInsertPriceCode.SelectedIndex = -1

                txtTempInsertStartShipDate = CType(gvAccrualOverride.FooterRow.FindControl("txtInsertStartShipDate"), TextBox)
                txtTempInsertStartShipDate.Text = ""

                txtTempInsertEndShipDate = CType(gvAccrualOverride.FooterRow.FindControl("txtInsertEndShipDate"), TextBox)
                txtTempInsertEndShipDate.Text = ""

                txtTempInsertOverrideRELPRC = CType(gvAccrualOverride.FooterRow.FindControl("txtInsertAccrualOverrideCurrentPrice"), TextBox)
                txtTempInsertOverrideRELPRC.Text = ""

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub gvAccrualOverride_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAccrualOverride.RowCreated

        Try
            'hide first column
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If


            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_AccrualOverride
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub gvAccrualOverride_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAccrualOverride.RowDataBound

        Try
            ClearMessages()

            Dim ds As DataSet
         
            Dim ddTempInsertPartNo As DropDownList
            Dim ddTempInsertPriceCode As DropDownList

            If e.Row.RowType = DataControlRowType.Footer Then

                If ViewState("AREID") > 0 Then
                    'get distinct partno list for accruing ar event
                    ddTempInsertPartNo = CType(e.Row.FindControl("ddInsertPartNo"), DropDownList)
                    If ddTempInsertPartNo IsNot Nothing Then

                        ddTempInsertPartNo.Items.Clear()

                        ds = ARGroupModule.GetAREventAccrualPartList(ViewState("AREID"))
                        If commonFunctions.CheckDataSet(ds) = True Then
                            ddTempInsertPartNo.DataSource = ds
                            ddTempInsertPartNo.DataTextField = ds.Tables(0).Columns("PARTNO").ColumnName
                            ddTempInsertPartNo.DataValueField = ds.Tables(0).Columns("PARTNO").ColumnName
                            ddTempInsertPartNo.DataBind()

                            'ddTempInsertPartNo.DataBind()
                            ddTempInsertPartNo.Items.Insert(0, "")
                        End If
                    End If

                    'get distinct price code list for accruing ar event
                    ddTempInsertPriceCode = CType(e.Row.FindControl("ddInsertPriceCode"), DropDownList)
                    If ddTempInsertPriceCode IsNot Nothing Then

                        ddTempInsertPriceCode.Items.Clear()

                        ds = ARGroupModule.GetAREventAccrualPriceCodeList(ViewState("AREID"))
                        If commonFunctions.CheckDataSet(ds) = True Then
                            ddTempInsertPriceCode.DataSource = ds
                            ddTempInsertPriceCode.DataTextField = ds.Tables(0).Columns("ddPriceCodeName").ColumnName
                            ddTempInsertPriceCode.DataValueField = ds.Tables(0).Columns("PRCCDE").ColumnName
                            ddTempInsertPriceCode.DataBind()

                            'ddTempInsertPriceCode.DataBind()
                            ddTempInsertPriceCode.Items.Insert(0, "")
                        End If
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvAccrualOverride_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvAccrualOverride.RowDeleted

        Try

            gvAccrual.DataBind()
            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvAccrualOverride_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvAccrualOverride.RowUpdated

        Try

            gvAccrual.DataBind()
            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnUpdateAccrual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateAccrual.Click

        Try
            ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
